using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Items;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat
{
    public static class CombatCalculator
    {

        public static string GetHealText(float heal)
        {
            var color = heal > 0 ? "#6f6" : heal < 0 ? "#f66" : "#fff";

            return $" <b>(<font color=\"{color}\">{Math.Round(heal)}</font>)</b>";
        }

        public static void AddCombatBuff(Creature creature, Identifier stat, float amount, string sourceName)
        {
            var id = new Identifier("coc", "combat_buff_" + sourceName.ToLowerInvariant().Replace(' ', '_'));

            var source = new StatusSource(id, sourceName);

            creature.Stats.AddModifier(stat, ModifierOperation.Addition, () => amount, source);
        }

        public static readonly Dictionary<Identifier, List<(Identifier Stat, string Source)>> LinkedBuffs = [];

        public static void AddCombatBuff(Creature creature, Identifier stat, float amount, string sourceName, Identifier? linkedStatus)
        {
            AddCombatBuff(creature, stat, amount, sourceName);

            if (linkedStatus != null)
                LinkBuff(linkedStatus.Value, stat, sourceName);
        }

        // 只对状态存在期间生效的乘法战斗增益
        public static void AddCombatBuffMultiplier(Creature creature, Identifier stat, float multiplier, string sourceName, Identifier? linkedStatus = null)
        {
            var id = new Identifier("coc", "combat_buff_" + sourceName.ToLowerInvariant().Replace(' ', '_'));

            var source = new StatusSource(id, sourceName);

            creature.Stats.AddModifier(stat, ModifierOperation.Multiplier, () => multiplier, source);

            if (linkedStatus != null)
                LinkBuff(linkedStatus.Value, stat, sourceName);
        }

        private static void LinkBuff(Identifier status, Identifier stat, string sourceName)
        {
            if (!LinkedBuffs.TryGetValue(status, out var list))
                LinkedBuffs[status] = list = [];

            list.Add((stat, sourceName));
        }

        public static void OnStatusRemoved(Creature creature, Identifier statusId)
        {
            if (!LinkedBuffs.TryGetValue(statusId, out var buffs))
                return;

            foreach (var (stat, source) in buffs)
            {
                var id = new Identifier("coc", "combat_buff_" + source.ToLowerInvariant().Replace(' ', '_'));

                creature.Stats.RemoveFrom(new StatusSource(id, source));
            }

            LinkedBuffs.Remove(statusId);
        }

        public static void ClearLinkedBuffs(Creature creature)
        {
            foreach (var buffs in LinkedBuffs.Values)
                foreach (var (stat, source) in buffs)
                {
                    var id = new Identifier("coc", "combat_buff_" + source.ToLowerInvariant().Replace(' ', '_'));

                    creature.Stats.RemoveFrom(new StatusSource(id, source));
                }
        }

        public static Identifier StatByName(string name) => name switch
        {
            "spe" => GameStats.Speed,

            "str" => GameStats.Strength,

            "tou" => GameStats.Toughness,

            "inte" => GameStats.Intelligence,

            _ => GameStats.Strength
        };

        public static void AddArmorBonus(Creature creature, float amount) =>

            creature.ArmorDefense = Math.Max(0, creature.ArmorDefense + amount);

        public static AvoidResult Avoid(CombatContext context, AvoidDamageParameters def)
        {
            var attacker = def.Attacker ?? context.Player;

            var defender = def.Defender ?? context.Enemy;

            var toHitChance = float.IsNaN(def.ToHitChance)

                ? defender.StandardDodgeFunc(attacker)

                : def.ToHitChance;

            if (attacker.HasStatusEffect(StatusEffectIds.TimeFrozen))
                return new AvoidResult { Outcome = AvoidOutcome.Hit };

            var countered = def.DoCounter && attacker.AttackCountered(defender);

            string? dodgeReason = null;

            if (def.DoDodge)
            {
                dodgeReason = defender.GetEvasionReason(attacker, toHitChance);

                if (attacker.HasPerk(PerkIds.Frustration))
                    if (dodgeReason != null)
                        attacker.AddPerkValue(PerkIds.Frustration, 1, 10);
                    else
                        attacker.SetPerkValue(PerkIds.Frustration, 1, 0);
            }

            if (defender.HasStatusEffect(StatusEffectIds.Backstab) && dodgeReason == null)
                defender.ChangeStatusValue(StatusEffectIds.Backstab, 0, 1);

            var parried = false;

            var blocked = false;

            if (dodgeReason == null)
            {
                if (def.DoParry)
                    parried = defender.CombatParry();

                if (!parried && def.DoBlock)
                    blocked = CombatBlock(context, attacker, defender, def.DoFatigue);
            }

            var outcome = countered ? AvoidOutcome.Counter

                : dodgeReason != null ? AvoidOutcome.Dodge

                : parried ? AvoidOutcome.Parry

                : blocked ? AvoidOutcome.Block

                : AvoidOutcome.Hit;

            return new AvoidResult { Outcome = outcome, DodgeReason = dodgeReason };
        }

        public static AvoidResult Avoid(
            CombatContext context,

            Creature attacker,

            Creature defender,

            bool doDodge = true,

            bool doParry = false,

            bool doBlock = false) =>

            Avoid(context, new AvoidDamageParameters
            {
                Attacker = attacker,

                Defender = defender,

                DoDodge = doDodge,

                DoParry = doParry,

                DoBlock = doBlock
            });

        public static bool Hit(CombatContext context, Creature attacker, Creature defender) =>

            Avoid(context, attacker, defender).AttackHit;

        public static bool CombatBlock(CombatContext context, Creature attacker, Creature defender, bool doFatigue = false)
        {
            if (defender.HasStatusEffect(StatusEffectIds.Retribution))
                return false;

            if (defender.HasStatusEffect(StatusEffectIds.TFSupercharging))
                return false;

            var blockChance = (int)(20 + defender.ShieldBlock + MathF.Floor((defender.Strength - attacker.Strength) / 5f));

            if (defender.HasPerk(PerkIds.ShieldMastery) && defender.Toughness >= 50)
                blockChance += (int)((defender.Toughness - 50) / 5f);

            if (blockChance < 10)
                blockChance = 10;

            var fatigueLimit = (int)(defender.MaxFatigue() - defender.PhysicalCost(10));

            var hasShield = defender.ShieldName != "nothing" && defender.ShieldName != "";

            if (blockChance >= context.Random.Next(100) + 1 && defender.Fatigue <= fatigueLimit && hasShield)
            {
                if (doFatigue)
                    defender.ChangeFatigue(defender.PhysicalCost(10));

                return true;
            }

            return false;
        }

        public static float HitChance(Creature attacker, Creature defender) =>

            Math.Clamp(defender.StandardDodgeFunc(attacker) / 100f, 0f, 1f);

        public static float PhysicalDamage(CombatContext context, Creature attacker, Creature defender, float power = 0)
        {

            var raw = attacker.Strength + attacker.WeaponAttack + power +

                attacker.StatusEffectValue(StatusEffectIds.ChargeWeapon, 0) +

                attacker.StatusEffectValue(StatusEffectIds.TFInflame, 0) +

                attacker.StatusEffectValue(StatusEffectIds.TFInflame, 1);

            // 原版 BerserkBuff/LustserkBuff：持械时武器伤害 +30（徒手走身体伤害不重复计算）
            if (!attacker.IsUnarmed()
                && (attacker.HasStatusEffect(StatusEffectIds.Berserking)

                    || attacker.HasStatusEffect(StatusEffectIds.Lustserking)))

                raw += 30;

            var damage = raw * DamagePercent(context, defender) / 100f * attacker.PhysMod();

            // 原版 UnarmedClaws.hx:63 get_armorMod()：自然武器 perk 下的爪类护甲穿透
            if (attacker is Player clawPlayer && clawPlayer.Weapon.Id == Weapon.Claws.Id)
                damage *= Items.Behaviors.Vanilla.UnarmedClawsBehavior.ArmorMod(clawPlayer);

            if (context.Random.NextDouble() < CriticalChance(attacker))
                damage *= 1.75f;

            return Math.Max(1, damage);
        }

        public static void DamageEnemy(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (damage <= 0)
                return;

            var final = context.Enemy.ModifyIncomingDamage(context, damage, kind);

            context.Enemy.HealthChange(-final);

            context.Enemy.HandleDamaged(context, final, kind);
        }

        // 原版 allMonsters()：依次把上下文指向每个存活敌人，结算完再还原
        public static void ForEachEnemy(CombatContext context, Action<Enemy> action)
        {
            var original = context.Enemy;

            foreach (var enemy in context.Enemies.ToArray())
            {
                if (enemy.Health <= 0)
                    continue;

                context.Enemy = enemy;

                action(enemy);
            }

            context.Enemy = original;
        }

        public static void ApplyLeech(Creature attacker, float damage)
        {
            var leeching = attacker.StatusEffectByType(StatusEffectIds.Leeching);

            if (leeching == null || damage <= 0 || leeching.Values[0] <= 0)
                return;

            var healed = damage * leeching.Values[1] / 100f;

            attacker.HealthChange(healed);

            leeching.Values[0] -= 1;

            if (leeching.Values[0] <= 0)
                attacker.RemoveStatusEffect(StatusEffectIds.Leeching);
        }

        public static float GlobalMod(Creature source, float damage) =>

            damage * source.Stats.Multiplier(GameStats.GlobalDamage);

        public static float BodyMod(Creature source, float damage) =>

            (damage + source.Stats.Addition(GameStats.BodyDamage)) * source.Stats.Multiplier(GameStats.BodyDamage);

        public static float ReduceDamage(CombatContext context, Creature defender, float damage)
        {
            // 原版 Creature.hx:4390：护盾直接减 30 点（下限 1），再乘减伤系数
            if (defender.HasStatusEffect(StatusEffectIds.Shielding))
            {
                damage -= 30;

                if (damage < 1)
                    damage = 1;
            }

            return damage * DamagePercent(context, defender) / 100f

                * CombatStatusModifiers.DamageTakenMultiplier(defender);
        }

        public static float DamagePercent(CombatContext context, Creature defender)
        {
            var modifier = ToughnessModifier(context, defender);
            // 原版 Creature.hx:4403：Soulburst 期间受到的伤害按 100% 计算（无视护甲与减伤）
            if (defender.HasStatusEffect(Handlers.SoulburstHandlers.Soulburst)) return 100f;

            var percent = 100f - modifier;

            if (percent < 75)
                percent = 75;

            var armor = defender.ArmorDefense;

            // 原版 BrawlerStanceBuff.boostsArmorPenetration：玩家拳法姿态期间按精通穿透目标护甲
            if (defender != context.Player
                && context.Player.HasStatusEffect(StatusEffectIds.BrawlerStance))
            {
                var fistMastery = context.Player.MasteryLevel(new Identifier("coc", "Fist"));

                armor *= Math.Max(0f, 1f - 0.03f * fistMastery);
            }

            percent -= armor;

            if (defender.HasPerk(PerkIds.Masochist) && defender.Libido >= 60)
                percent *= 0.8f;

            if (defender.HasPerk(PerkIds.ImmovableObject) && defender.Toughness >= 75)
                percent *= 0.9f;

            return Math.Clamp(percent, 20f, 100f);
        }

        public static float ToughnessModifier(CombatContext context, Creature defender)
        {
            var toughness = defender.Toughness;

            var modifier = toughness switch
            {
                < 25 => toughness * 0.4f,

                < 50 => 10 + (toughness - 25) * 0.3f,

                < 75 => 17.5f + (toughness - 50) * 0.2f,

                < 100 => 22.5f + (toughness - 75) * 0.1f,

                _ => 25f
            };

            return (float)(context.Random.NextDouble() * modifier);
        }

        public static float TeaseDamage(CombatContext context, Creature attacker, float power = 0)
        {

            var teaseBase = 20f + attacker.Stats.Addition(GameStats.TeaseDamage);

            teaseBase *= attacker.Stats.Factor(GameStats.TeaseDamage);

            var damage = Math.Max(1, teaseBase / 20f * (attacker.Libido / 5f + 5 + power));

            if (context.Random.NextDouble() < CriticalChance(attacker))
                damage *= 1.5f;

            return damage;
        }

        public static float CriticalChance(Creature attacker)
        {
            var chance = attacker.GetBaseCritChance();

            // 原版 LurkBuff.hx：潜伏（value1 > 0）提供 +15 暴击
            if (attacker.HasStatusEffect(StatusEffectIds.Lurk)
                && attacker.StatusEffectValue(StatusEffectIds.Lurk, 0) > 0)

                chance += 15;

            return chance / 100f;
        }

        public static float EscapeChance(CombatContext context)
        {
            var chance = 0.5f + (context.Player.Speed - context.Enemy.Speed) * 0.01f;

            chance += context.Player.Stats.Addition(GameStats.EscapeChance);

            return Math.Clamp(chance, 0.1f, 0.9f);
        }
    }
}