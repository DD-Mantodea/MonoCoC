using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/IvorySuccubus.hx（extends Monster）
    // 12 级象牙魅魔：鞭击、飞行、白焰、定身，欲望过半后转用魅惑系法术。
    [Register("coc:ivory_succubus")]
    public class IvorySuccubusEnemy : Enemy
    {
        // 原版 var flightCooldown:Int = 0（IvorySuccubus.hx:8）
        public int FlightCooldown;

        // 原版 StatusEffects.PermaFlyStatus / StatusEffects.Attracted：C# 的 VanillaStatusEffects
        // 里没有这两条，按项目 Identifier 约定本地声明（两者都只是标记 + 已有距离/文本逻辑）。
        public static readonly Identifier PermaFly = new("coc", "perma_fly");

        public static readonly Identifier Attracted = new("coc", "attracted");

        private static readonly Identifier LashId = new("coc", "ivory_succubus_lash");

        private static readonly Identifier TeaseId = new("coc", "ivory_succubus_tease");

        private static readonly Identifier FlightId = new("coc", "ivory_succubus_flight");

        private static readonly Identifier BlindId = new("coc", "ivory_succubus_blind");

        private static readonly Identifier WhitefireId = new("coc", "ivory_succubus_whitefire");

        private static readonly Identifier ChargeId = new("coc", "ivory_succubus_charge");

        private static readonly Identifier ArouseId = new("coc", "ivory_succubus_arouse");

        private static readonly Identifier AttractId = new("coc", "ivory_succubus_attract");

        private static readonly Identifier LockUpId = new("coc", "ivory_succubus_lock_up");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new() 逐项照抄（IvorySuccubus.hx:135-180）──
            // createVagina(false, WETNESS_WET(2), LOOSENESS_NORMAL(2))
            CreateVagina(false, 2, 2);

            // createBreastRow(Appearance.breastCupInverse("B")) = 2
            CreateBreastRow(VanillaBreastCups.B);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            Tallness = 73;

            // hips.rating = RATING_AVERAGE(4) / butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("average");
            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();
            hair.Color = "light gray";
            hair.Length = 4;

            // 原版 level = 12 / lust = 17+rand(7) / bonusHP = 300 / 属性都在数据表里。
            Lust = 17 + Random.Shared.Next(7);

            WeaponName = "whip";
            WeaponVerb = "lash";
            WeaponAttack = 35;

            // normalAttackAvoid.doParry = false; doCounter = false;
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            ArmorName = "ivory corset";
            ArmorDefense = 10;

            Fatigue = 0;

            // 原版 lustVuln = 1（默认）；bonusLust = 50 在 C# 无对应字段（Creature.MaxLust 恒 100）。

            // 原版 gems = Utils.rand(10) + 15
            Gems = Random.Shared.Next(10) + 15;

            // 原版 drop 见 IvorySuccubus.hjson loot。
        }

        // 原版 combatRoundUpdate()（IvorySuccubus.hx:102-107）：飞行时每回合额外 +5 疲劳。
        public override void CombatRoundUpdate(CombatContext context)
        {
            if (HasStatusEffect(PermaFly))
                ChangeFatigue(5);

            base.CombatRoundUpdate(context);
        }

        // 原版 performCombatAction（IvorySuccubus.hx:109-125）：
        //   `--flightCooldown` 每回合减 1；飞行中且（冷却 < 4 或体力不够 5）则落地，否则走 AI 权重表。
        public override bool BeforeCombatAction(CombatContext context)
        {
            FlightCooldown--;

            if (HasStatusEffect(PermaFly)
                && (FlightCooldown < 4 || !HasFatigue(5, FatigueType.Physical)))
            {
                Land(context);

                return true;
            }

            var options = new List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue)>();

            // lash：weight 4, cost 0 none, Melee
            options.Add((LashId, 4, 0, FatigueType.None));

            // tease：weight 2, cost 0 none, Tease
            options.Add((TeaseId, 2, 0, FatigueType.None));

            // flight：weight 1, cost 0 none, Self，条件 flightCooldown < 1
            if (FlightCooldown < 1)
                options.Add((FlightId, 1, 0, FatigueType.None));

            // 欲望分档权重：lust < 50 → 1 : .25；lust >= 50 → 1 : .25
            var lowLustWeight = Lust < 50 ? 1f : 0.25f;
            var highLustWeight = Lust >= 50 ? 1f : 0.25f;

            AddSpell(options, BlindId, lowLustWeight, 20);
            AddSpell(options, WhitefireId, lowLustWeight, 15);
            AddSpell(options, ChargeId, lowLustWeight, 10);
            AddSpell(options, ArouseId, highLustWeight, 10);

            // attract：weight 同上，条件 !player.hasStatusEffect(StatusEffects.Attracted)
            if (!context.Player.HasStatusEffect(Attracted))
                AddSpell(options, AttractId, highLustWeight, 15);

            AddSpell(options, LockUpId, highLustWeight, 20);

            if (options.Count == 0)
            {
                // 原版 MonsterAI.exec：没有可选动作时直接 eAttack。
                CombatController.Instance!.Resolver.EnemyBasicAttack();

                return true;
            }

            var total = 0f;

            foreach (var option in options)
                total += option.Weight;

            var roll = context.Random.NextDouble() * total;

            var chosen = options[^1];

            foreach (var option in options)
            {
                roll -= option.Weight;

                if (roll <= 0)
                {
                    chosen = option;
                    break;
                }
            }

            var ability = CombatAbilityRegistry.Get(chosen.Id)
                ?? new CombatAbilityDefinition { Id = chosen.Id, Handler = chosen.Id };

            if (chosen.Cost > 0)
                ChangeFatigue(chosen.Cost, chosen.Fatigue);

            CombatController.Instance!.Resolver.EnemyAction(ability);

            return true;
        }

        private void AddSpell(
            List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue)> options,
            Identifier id,
            float weight,
            int cost)
        {
            if (HasFatigue(cost, FatigueType.Magical))
                options.Add((id, weight, cost, FatigueType.Magical));
        }

        // 原版 land()（IvorySuccubus.hx:45-49）
        private void Land(CombatContext context)
        {
            context.Append("Combat.IvorySuccubus.Land.TheSuccubusHolds".Localized());

            RemoveStatusEffect(PermaFly);

            CombatRangeData.MoveDistantSafe(this);
        }

        // 原版 defeated() → game.ivorySuccubusScene.defeated(hpVictory)（IvorySuccubus.hx:127-129）
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.IvorySuccubusScene>
                .Get().Defeated(hpVictory);

            return true;
        }

        // 原版 won() → game.ivorySuccubusScene.won(hpVictory)（IvorySuccubus.hx:131-133）
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.IvorySuccubusScene>
                .Get().Won(hpVictory);

            return true;
        }
    }
}
