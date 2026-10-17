using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    // 原版 CombatAbilities.hx:2729 起：Arian 护符里存的 5 种法术
    public static class ArianCombatHandlers
    {

        /// <summary>原版 CombatAbilities.hx:2732 availableWhen：护符里存的是护盾（值为 5）。</summary>
        [Register("coc:arian_shield_available")]
        public static bool ShieldAvailable(CombatContext context, CombatState state) =>

            context.Player.KeyItemV1("Arian's Charged Talisman") == 5;

        /// <summary>原版 CombatAbilities.hx:2741 shieldingSpell()</summary>
        [Register("coc:arian_shield")]
        public static void ShieldingSpell(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Arian.ShieldingSpell.BarrierOfLight".Localized());

            player.CreateStatusEffect(StatusEffectIds.Shielding);

            // 原版 :2745：用掉护符，变回空的 Arian's Talisman
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.ArianScene>.Get().ClearTalisman();
        }
    }

    // 原版 CombatAbilities.hx:1908 Tail Slam（龙尾重击）
    public static class TailSlamCombatHandlers
    {
        /// <summary>原版 CombatAbilities.hx:1921 availableWhen：只有龙尾能用。</summary>
        [Register("coc:tail_slam_available")]
        public static bool Available(CombatContext context, CombatState state) =>

            context.Player.Body.Get<BodyParts.Vanilla.TailPart>().Id

                == BodyPartTypes.Draconic;

        /// <summary>原版 CombatAbilities.hx:5271 tailSlamAttack()</summary>
        [Register("coc:tail_slam")]
        public static void TailSlam(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            // 原版 :5276 闪避判定（不格挡不招架）
            var avoid = CombatCalculator.Avoid(
                context,

                new Enemies.AvoidDamageParameters { DoParry = false, DoBlock = false });

            if (avoid.AttackFailed)
            {
                context.WriteAction("Combat.TailSlam.YouSwingYourMighty".Localized());

                return;
            }

            context.WriteAction("Combat.TailSlam.WithGreatSweepYou".Localized());

            // 原版 :5283 tailslamcalc()
            var damage = TailSlamCalc(context);

            CombatCalculator.DamageEnemy(
                context, damage, Enemies.CombatDamageKind.Melee);

            // 原版 :5286 晕眩概率随被砸次数衰减（100 / (2 + times)，下限 10%）
            var times = enemy.HasStatusEffect(StatusEffectIds.TimesBashed)

                ? (int)enemy.StatusEffectValue(StatusEffectIds.TimesBashed, 0)

                : 0;

            var chance = 100 / (2 + times);

            if (chance < 10)
                chance = 10;

            if (enemy.Stun(1, chance))
            {
                context.WriteAction("Combat.TailSlam.TheHarshBlowAlso".Localized());

                if (!enemy.HasStatusEffect(StatusEffectIds.TimesBashed))
                    enemy.CreateStatusEffect(StatusEffectIds.TimesBashed, 1, 0, 0, 0);
                else
                    enemy.AddStatusValue(StatusEffectIds.TimesBashed, 0, 1);
            }

            // 原版 :5298 流血
            if (enemy.Bleed(player))
                context.WriteAction("Combat.TailSlam.MonsterPluralProfuselyFrom".Localized());
        }

        /// <summary>原版 CombatAbilities.hx:5309 tailslamcalc()</summary>
        public static float TailSlamCalc(CombatContext context)
        {
            var player = context.Player;

            var damage = (float)Math.Floor(10 + player.Strength / 1.1f);

            if (damage > 0)
            {
                damage *= player.Stats.Multiplier(GameStats.GlobalDamage);

                damage = CombatCalculator.BodyMod(player, damage);

                damage = CombatCalculator.GlobalMod(player, damage);
            }

            return damage;
        }
    }

    // 原版 CombatAbilities.hx:3738-3796：美人鱼之歌（mermaidChant）
    public static class MermaidChantCombatHandlers
    {
        /// <summary>原版 CombatAbilities.hx 的 availableWhen：需要美人鱼之歌天赋。</summary>
        [Register("coc:mermaid_chant_available")]
        public static bool Available(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.MermaidChant);

        /// <summary>原版 CombatAbilities.hx:3738 mermaidChantLust()</summary>
        public static int ChantLust(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var teaseMulti = enemy.LustVulnerability;

            if (player.HasPerk(PerkIds.HistoryWhore))
                teaseMulti *= 1.15f;

            if (player.HasPerk(PerkIds.AscensionSeduction))
                teaseMulti *= 1 + player.PerkValue(PerkIds.AscensionSeduction, 1) * 0.05f;

            var damage = Math.Max(player.Stats.Get(GameStats.TeaseDamage), 20) * teaseMulti / 2;

            var min = (int)MathF.Round(damage * 0.85f);

            var max = (int)MathF.Round(damage * 1.15f);

            return Random.Shared.Next(Math.Min(min, max), Math.Max(min, max) + 1);
        }

        /// <summary>原版 CombatAbilities.hx:3752 mermaidChantDebuff()</summary>
        public static int ChantDebuff(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var teaseAdd = player.Stats.Addition(GameStats.TeaseDamage);

            var teaseMulti = player.Stats.Multiplier(GameStats.TeaseDamage);

            var flatPart = (10 + teaseAdd * 2 / 3) * teaseMulti;

            var percentPart = (0.1f + teaseAdd / 300) * teaseMulti * enemy.Speed;

            var avg = (flatPart + percentPart) / 2;

            var min = (int)MathF.Round(avg * 0.8f);

            var max = (int)MathF.Round(avg * 1.2f);

            return Random.Shared.Next(Math.Min(min, max), Math.Max(min, max) + 1);
        }

        /// <summary>原版 CombatAbilities.hx:3769 mermaidChant()</summary>
        [Register("coc:mermaid_chant")]
        public static void MermaidChant(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.MermaidChant.DespiteTheRentsYou".Localized());

            var enemy = context.Enemy;

            var speedBefore = enemy.Speed;

            var lust = ChantLust(context);

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + lust);

            var debuff = ChantDebuff(context);

            // 原版用 ChantedDebuff.applyEffect() 直接压 speed，这里用战斗增益系统等价实现
            CombatCalculator.AddCombatBuff(enemy, GameStats.Speed, -debuff, "MermaidChant", StatusEffectIds.Chanted);

            var speedAfter = enemy.Speed;

            context.WriteAction(
                "Combat.MermaidChant.Line5300".Localized(
                    MathF.Round(speedBefore - speedAfter).ToString(),

                    ""));
        }
    }

    // 原版 CombatAbilities.hx:310-357：RagingInferno 连续火焰法术加成
    public static class InfernoModifiers
    {
        /// <summary>原版 CombatAbilities.hx:310 boostInferno()：累计连续施放的火焰法术。</summary>
        public static void BoostInferno(CombatContext context)
        {
            if (!context.Player.HasPerk(PerkIds.RagingInferno))
                return;

            var state = CombatController.Instance!.State;

            if (state.Round - state.FireMagicLastTurn == 1)
                state.FireMagicCumulated += 1;
            else
                state.FireMagicCumulated = 1;

            state.FireMagicLastTurn = state.Round;
        }

        /// <summary>原版 CombatAbilities.hx:334 calcInfernoMod()</summary>
        public static int CalcInfernoMod(CombatContext context, float damage)
        {
            var state = CombatController.Instance!.State;

            if (context.Player.HasPerk(PerkIds.RagingInferno) && state.Round - state.FireMagicLastTurn == 1)
            {
                var multiplier = state.FireMagicCumulated switch
                {
                    0 or 1 => 1.2f,

                    2 => 1.35f,

                    3 => 1.45f,

                    _ => 1.5f + (state.FireMagicCumulated - 5) * 0.05f,
                };

                damage *= multiplier;
            }

            return (int)damage;
        }

        /// <summary>原版 CombatAbilities.hx:321 infernoDisplay()：只提示，不改动计数。</summary>
        public static void InfernoDisplay(CombatContext context)
        {
            var state = CombatController.Instance!.State;

            if (!context.Player.HasPerk(PerkIds.RagingInferno))
                return;

            if (state.Round - state.FireMagicLastTurn == 1)
                context.Append("Combat.Inferno.WithSimpleThoughtYou".Localized());
            else if (state.FireMagicLastTurn > 0)
                context.Append("Combat.Inferno.TwirlingLikeTopYou".Localized());
        }

        /// <summary>原版 CombatAbilities.hx:359 getInfernoCount()</summary>
        public static int GetInfernoCount(CombatContext context)
        {
            var state = CombatController.Instance!.State;

            if (context.Player.HasPerk(PerkIds.RagingInferno) && state.Round - state.FireMagicLastTurn <= 1)
                return state.FireMagicCumulated;

            return 0;
        }
    }

    // 原版 CombatAbilities.hx:2575-2760：Arian 护符的其余四个法术
    // 存储值：1=Dispelling 2=Healing 3=Immolation 4=Lust Reduction 5=Shielding（见 ArianScene.ArianSpellPlace）
    public static class ArianTalismanSpells
    {
        private static bool HasTalisman(CombatContext context, int value) =>

            context.Player.KeyItemV1("Arian's Charged Talisman") == value;

        private static void ConsumeTalisman() =>

            Scenes.SceneInstance<Scenes.Vanilla.Npcs.ArianScene>.Get().ClearTalisman();

        // ---------- 1) 解咒术 ----------

        [Register("coc:arian_talisman_one")]
        public static bool DispelAvailable(CombatContext context, CombatState state) => HasTalisman(context, 1);

        [Register("coc:arian_dispel")]
        public static void Dispel(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.WriteAction("Combat.Arian.Dispel.TwirlingLikeTopYou".Localized());

            // 原版 :2578 起：逐条移除玩家身上的增益/诅咒
            foreach (var (status, key) in PLAYER_DISPEL_TARGETS)
                if (player.HasStatusEffect(status))
                {
                    context.Append(key.Localized());

                    player.RemoveStatusEffect(status);
                }

            // 原版 :2623 起：再移除敌人身上的增益
            foreach (var (status, key) in ENEMY_DISPEL_TARGETS)
                if (enemy.HasStatusEffect(status))
                {
                    context.Append(key.Localized());

                    enemy.RemoveStatusEffect(status);
                }

            ConsumeTalisman();
        }

        private static readonly (MonoCoC.Registry.Identifier Status, string Key)[] PLAYER_DISPEL_TARGETS =

        [
            (StatusEffectIds.ChargeWeapon, "Combat.Arian.Dispel.NtheTalismanSuccessfullyRemoves"),

            (StatusEffectIds.Leeching, "Combat.Arian.Dispel.NyouFeelBitWeaker"),

            (StatusEffectIds.TFInflame, "Combat.Arian.Dispel.NtheGlowAround"),

            (StatusEffectIds.TFPlate, "Combat.Arian.Dispel.NtheGolemShimmeringShield"),

            (StatusEffectIds.TFMoltenPlate, "Combat.Arian.Dispel.NtheDarkIllusionAround"),

            (StatusEffectIds.ParasiteNephila, "Combat.Arian.Dispel.NtheRealityAroundFinally"),

            (StatusEffectIds.ParasiteQueen, "Combat.Arian.Dispel.NyouFeelBitWeakerYour"),

            (StatusEffectIds.NephilaQueen, "Combat.Arian.Dispel.NtheMagicalShellAround"),

            (StatusEffectIds.TrueWhispered, "Combat.Arian.Dispel.Line2631"),

            (StatusEffectIds.Marked, "Combat.Arian.Dispel.YouGatherEnergyYour"),

            (StatusEffectIds.Nothingness, "Combat.Arian.Dispel.YouGatherEnergyYourTalisman"),

            (StatusEffectIds.Might, "Combat.Arian.Dispel.TheGolemDoesnSeem"),
        ];

        private static readonly (MonoCoC.Registry.Identifier Status, string Key)[] ENEMY_DISPEL_TARGETS =

        [
            (StatusEffectIds.ChargeWeapon, "Combat.Arian.Dispel.YourSpellOverpowersThe"),

            (StatusEffectIds.VolcanicUberHeal, "Combat.Arian.Dispel.YouGatherEnergyYourTalismanAnd"),

            (StatusEffectIds.Fear, "Combat.Arian.Dispel.YouRollForwardOnto"),

            (StatusEffectIds.Illusion, "Combat.Arian.Dispel.Line2638"),

            (StatusEffectIds.Shell, "Combat.Arian.Dispel.YouGatherEnergyYourTalismanAndUnleash"),
        ];

        // ---------- 2) 治疗术 ----------

        [Register("coc:arian_talisman_two")]
        public static bool HealAvailable(CombatContext context, CombatState state) => HasTalisman(context, 2);

        /// <summary>原版 CombatAbilities.hx:2664 talismanHealCalc()</summary>
        public static int TalismanHealCalc(CombatContext context, bool maxHeal = false)
        {
            var player = context.Player;

            var bonus = maxHeal ? player.Intelligence : Random.Shared.Next((int)Math.Max(1, player.Intelligence));

            return (int)(((player.Level * 5) + (player.Intelligence / 1.5f) + bonus) * player.SpellMod() * 1.5f);
        }

        [Register("coc:arian_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Arian.Heal.YouTryReadySpecial".Localized());

            context.Player.HealthChange(TalismanHealCalc(context));

            ConsumeTalisman();
        }

        // ---------- 3) 献祭术（火） ----------

        [Register("coc:arian_talisman_three")]
        public static bool ImmolateAvailable(CombatContext context, CombatState state) => HasTalisman(context, 3);

        /// <summary>原版 CombatAbilities.hx:2690 immolationDamageCalc()</summary>
        public static int ImmolationDamageCalc(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = (float)(int)(75 + (player.Intelligence / 2f + Random.Shared.Next((int)Math.Max(1, player.Intelligence))) * player.SpellMod());

            damage = InfernoModifiers.CalcInfernoMod(context, damage);

            if (enemy.Id == EnemyIds.VolcanicGolem)
                damage *= 0.2f;

            return (int)CombatCalculator.GlobalMod(player, damage);
        }

        [Register("coc:arian_immolate")]
        public static void Immolate(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.WriteAction("Combat.Arian.Immolate.YouRoarAndUnleash".Localized());

            var damage = ImmolationDamageCalc(context);

            // 原版 :2703：熔岩魔像不吃火伤，但吸血护盾仍会被消耗
            if (enemy.Id == EnemyIds.VolcanicGolem)
            {
                context.Append("Combat.Arian.Immolate.TheGolemDoesnSeem".Localized());

                ConsumeTalisman();

                return;
            }

            InfernoModifiers.InfernoDisplay(context);

            // 原版 :2712 doFireDamage()：先显示加成提示，造成伤害，再累计连击
            CombatCalculator.DamageEnemy(context, damage, Enemies.CombatDamageKind.Magical);

            InfernoModifiers.BoostInferno(context);

            enemy.CreateStatusEffect(
                StatusEffectIds.OnFire,

                2 + Random.Shared.Next((int)(context.Player.Intelligence / 25)),

                0, 0, 0);

            ConsumeTalisman();
        }

        // ---------- 4) 降欲术 ----------

        [Register("coc:arian_talisman_four")]
        public static bool LustReduceAvailable(CombatContext context, CombatState state) => HasTalisman(context, 4);

        [Register("coc:arian_lust_reduce")]
        public static void LustReduce(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Arian.LustReduce.YouGatherEnergyYour".Localized());

            var temp = (int)(30 + Random.Shared.Next((int)Math.Max(1, player.Intelligence / 5)) * player.SpellMod());

            player.ApplyStats(
                new Stats.DynStatsOptions(Scale: false),

                new Stats.DynStat(GameStats.Lust, -temp));

            ConsumeTalisman();
        }
    }

    // 原版 Combat.hx:658 packAttack / :710 lustAttack —— 群体类敌人共用的两个通用招式
    public static class PackCombatHandlers
    {
        /// <summary>原版 Combat.hx:658 packAttack()</summary>
        [Register("coc:pack_attack")]
        public static void PackAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(
                context,

                new Enemies.AvoidDamageParameters
                {
                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false,
                });

            if (avoid.AttackFailed)
            {
                // 原版 :666-681：按闪避原因分别播报
                context.Append(avoid.DodgeReason switch
                {
                    "Evade" => "Combat.PackAttack.YouThrustYourPalm".Localized(),

                    "Misdirection" => "Combat.PackAttack.Them".Localized(),

                    "Flexibility" => "Combat.PackAttack.BackFewFeet".Localized(),

                    _ => "Combat.PackAttack.SuperEffective".Localized(),
                });

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0)
            {
                context.Append("Combat.PackAttack.YouThrustYourPalmForward".Localized(enemy.WeaponVerb));

                return;
            }

            context.Append(damage switch
            {
                <= 5 => "Combat.PackAttack.Testtest".Localized(),

                <= 10 => "Combat.PackAttack.YouRaiseFistYour".Localized(),

                <= 20 => "Combat.PackAttack.NtheSpellHitsYour".Localized(enemy.WeaponVerb),

                _ => "Combat.PackAttack.NtheSpellHitsBut".Localized(enemy.WeaponVerb),
            });

            player.HealthChange(-damage);
        }

        /// <summary>原版 Combat.hx:710 lustAttack()</summary>
        [Register("coc:pack_lust_attack")]
        public static void PackLustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (player.Lust < 35)
                context.Append("Combat.PackLust.YouGatherEnergyYour".Localized());
            else if (player.Lust < 65)
                context.Append("Combat.PackLust.NyourWeaponLongerGlows".Localized());
            else
                context.Append("Combat.PackLust.TheIncantationSurroundingYour".Localized());

            // 原版 :738：lustDmg = 10 + 敏感度/10
            player.Lust = Math.Min(player.MaxLust, player.Lust + (int)(10 + player.Sensitivity / 10f));
        }
    }
}