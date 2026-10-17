using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Necromancer extends Monster（scenes/dungeons/manor/Necromancer.hx:7）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / AI / 增援 / 恐惧免疫按 Haxe 原文人工补齐。
    // 招式效果见 Contents/Combat/VanillaHandlers/NecromancerCombatHandlers.cs。
    [Register("coc:necromancer")]
    public class NecromancerEnemy : Enemy
    {
        // 原版 Necromancer.hx:8 `public var hasSummonedElite:Bool = false`
        public bool HasSummonedElite;

        // 原版 Necromancer.hx:9 `public var skellyHordeLocation:Int = 1`
        public int SkellyHordeLocation = 1;

        private static readonly Identifier RaiseDeadId = new("coc", "necromancer_raise_dead");
        private static readonly Identifier RaiseEliteId = new("coc", "necromancer_raise_elite");
        private static readonly Identifier AttackId = new("coc", "necromancer_attack");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（Necromancer.hx:75）的赋值顺序照抄 ──
            // a = "the " / short = "Necromancer" / long / pronoun1-3 = he/him/his 由数据表表达。

            // createBreastRow(Appearance.breastCupInverse("E")) = 7
            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / ass.analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            // createStatusEffect(StatusEffects.BonusACapacity, 30, 0, 0, 0)
            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 30);

            Tallness = 6 * 12 + 10;

            // hips.rating = Hips.RATING_AMPLE(6) + 2 / butt.rating = Butt.RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark gray";

            var hair = Body.Get<HairPart>();
            hair.Color = "gray";
            hair.Length = 4;

            // initStrTouSpeInte(90, 100, 80, 160) / initLibSensCor(10, 10, 100) / level = 25 / lust = 0
            // bonusHP = 2500 由数据表给（base.Configure 会加成到 MaxHealth）。

            WeaponName = "clawed fingers";
            WeaponVerb = "claw";
            WeaponAttack = 40;

            ArmorName = "crimson cloak";

            // temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：原版死字段，无实际作用。
            // drop = new WeightedDrop()（空表）：数据表 loot 留空。

            Gems = Random.Shared.Next(15) + 250;
            LustVulnerability = 0;
        }

        // 原版 performCombatAction（Necromancer.hx:18）：
        //   1) 记录场上 SkeletonHorde 的位置（默认 1）
        //   2) 血量 < 50% 且没召唤过 → raiseElite，直接结束本回合
        //   3) 否则按权重 raiseDead:2 / eAttack:1 抽签
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var horde = FindSkellyHorde(context, out var location);

            SkellyHordeLocation = location;

            // 原版 Necromancer.hx:28 `if (!hasSummonedElite && HPRatio() < 0.5)`
            if (!HasSummonedElite && HealthRatio() < 0.5f)
                return new CombatAiAction { Ability = RaiseEliteId };

            // 原版 MonsterAI：raiseDead 权重 2、eAttack 权重 1（Necromancer.hx:33-34）。
            if (horde != null && CanRaiseDead(horde) && context.Random.Next(3) < 2)
                return new CombatAiAction { Ability = RaiseDeadId };

            return new CombatAiAction { Ability = AttackId };
        }

        // 原版 Necromancer.hx:33 的 when 条件，展开后是
        //   (unitAmount < 10 && !flag512) || (unitAmount < 5 && flag512)
        // 其中 flag512 = (MANOR_PROGRESS & 512) != 0。
        private static bool CanRaiseDead(SkeletonHordeEnemy horde)
        {
            var flag512 = (CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 512) != 0;

            return (horde.UnitAmount < 10 && !flag512) || (horde.UnitAmount < 5 && flag512);
        }

        // 原版 Necromancer.hx:19-27：在 game.monsterArray 里找第一只 SkeletonHorde。
        // C# 的等价容器是 CombatContext.Enemies（顺序一致）；找不到时 location 保持初值 1（与原文相同）。
        public static SkeletonHordeEnemy? FindSkellyHorde(CombatContext context, out int location)
        {
            location = 1;

            for (var i = 0; i < context.Enemies.Count; i++)
            {
                if (context.Enemies[i] is not SkeletonHordeEnemy horde)
                    continue;

                location = i;

                return horde;
            }

            return null;
        }

        // 原版 Monster.hx:1825：被 GuardAB 保护时每回合递减、归零解除（BoneGeneral.guardAlly 会挂，
        // 且优先保护 Necromancer）。迁到 TickStatuses（Enemy.cs:322，派发点 CombatController.cs:787/813），
        // 与 CombatRoundUpdate 只留一份，避免双倍递减。
        public override void TickStatuses(CombatContext context) =>
            BoneGeneralEnemy.TickGuardAb(context, this);

        // 原版 handleFear（Necromancer.hx:11）：死灵法师不吃恐惧，直接清掉并照常行动。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.Necromancer.HandleFear.TheNecromancerIsn".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 Necromancer.hx:67-72：
        //   won      → game.dungeons.manor.loseToNecro()（Manor.hx:799）
        //   defeated → game.dungeons.manor.defeatNecro()（Manor.hx:787）
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DefeatNecro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToNecro();

            return true;
        }
    }
}
