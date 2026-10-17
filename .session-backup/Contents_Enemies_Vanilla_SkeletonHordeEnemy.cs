using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 SkeletonHorde extends Monster（scenes/dungeons/manor/SkeletonHorde.hx:6）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / 连击 / 恐惧免疫按 Haxe 原文人工补齐。
    [Register("coc:skeleton_horde")]
    public class SkeletonHordeEnemy : Enemy
    {
        // 原版 unitHP = 100 / unitAmount = 4（SkeletonHorde.hx:107-108）。
        // 每 100 点血代表一只骷髅：攻击次数、最大生命都由它算。
        public float UnitHp = 100f;

        public int UnitAmount = 4;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（SkeletonHorde.hx:70）的赋值顺序照抄 ──
            // a = "the " / short = "Horde of Skeletons" / long / plural = true / pronoun1-3 由数据表表达。
            // initedGenitals = true。

            // createBreastRow(Appearance.breastCupInverse("E")) = 7
            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / ass.analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            // createStatusEffect(StatusEffects.BonusACapacity, 30, 0, 0, 0)
            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 30);

            Tallness = 35 + Random.Shared.Next(4);

            // hips.rating = Hips.RATING_AMPLE(6) + 2 / butt.rating = Butt.RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();
            hair.Color = "purple";
            hair.Length = 4;

            // initStrTouSpeInte(20, 60, 35, 42) / initLibSensCor(45, 45, 60) / level = 15 / lust = 0 由数据表给。

            WeaponName = "fists";
            WeaponVerb = "tiny punch";
            ArmorName = "leather straps";

            // temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：原版死字段，无实际作用。
            // drop = new WeightedDrop()（空表，不掉落）：数据表 loot 留空。

            Gems = Random.Shared.Next(5) + 5;
            LustVulnerability = 0;

            // createPerk(PerkLib.BleedImmune) / createPerk(PerkLib.PoisonImmune)
            CreatePerk(VanillaPerks.BleedImmune);
            CreatePerk(VanillaPerks.PoisonImmune);

            // 原版 maxHP() 覆写为 unitAmount * 100（SkeletonHorde.hx:7）。
            // C# 的 Creature.MaxHealth 不是虚属性，这里用一条动态 MaxHealth 加成把它改写成 unitAmount*100：
            //   基础值 = Creature 构造函数里的 provider（Toughness*2 + 50 + Level*15，上限 9999），
            //   加成 = 目标值 - 基础值，所以 (基础 + 加成) == unitAmount*100。
            // 数据表的 bonusHealth 已改成 0（否则会叠加在 400 之上）。
            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => UnitAmount * UnitHp - Math.Min(9999f, Toughness * 2 + 50 + Level * 15),
                new TemporarySource(Guid.NewGuid()));

            // 原版从满血开始（HP = maxHP() = 4 * 100），上面重算后同步一次。
            Health = MaxHealth;
        }

        // 原版 performCombatAction（SkeletonHorde.hx:11）：
        //   unitAmount = fceil(HP / 100)  → 每只骷髅各自 recombobulateWeapon() + eAttack()
        // C# 的 ChooseCombatAction 只能返回一招，所以整段放在 BeforeCombatAction 里自己做。
        // 数据表 ai 留空（不走 CombatAiInterpreter）：本方法只有这一条路径、必定 return true，
        // 不存在落到空 AI 的可能。
        public override bool BeforeCombatAction(CombatContext context)
        {
            UnitAmount = (int)Math.Ceiling(Health / UnitHp);

            context.Append("\n");

            for (var i = 0; i < UnitAmount; i++)
            {
                RecombobulateWeapon();

                CombatController.Instance!.Resolver.EnemyBasicAttack();
            }

            return true;
        }

        // 原版 recombobulateWeapon（SkeletonHorde.hx:22）：换个武器再打（Utils.rand(4) → 0..3）。
        private void RecombobulateWeapon()
        {
            switch (Random.Shared.Next(4))
            {
                case 0:
                    WeaponVerb = "stab";
                    WeaponName = "pitchfork";
                    WeaponAttack = 50;
                    break;
                case 1:
                    WeaponVerb = "slash";
                    WeaponName = "rusty sword";
                    WeaponAttack = 70;
                    break;
                case 2:
                    WeaponVerb = "crush";
                    WeaponName = "rusty mace";
                    WeaponAttack = 60;
                    break;
                case 3:
                    WeaponVerb = "slap";
                    WeaponName = "femur";
                    WeaponAttack = 30;
                    break;
                default:
                    // 原版 default 分支（SkeletonHorde.hx:45）：Utils.rand(4) 永远不会走到，保留以对齐原文。
                    WeaponVerb = "pick";
                    WeaponName = "pickaxe";
                    WeaponAttack = 40;
                    break;
            }
        }

        // 原版 Monster.hx:1825：被 GuardAB 保护时每回合递减、归零解除（BoneGeneral.guardAlly 会挂）。
        // 原版 get_plural() = unitAmount != 1（SkeletonHorde.hx:66）。
        // C# 的 Enemy.Plural 现为 virtual（Enemy.cs:458），直接按 unitAmount 动态返回。
        public override bool Plural => UnitAmount != 1;

        // 原版 Monster.hx:1825：被 GuardAB 保护时每回合递减、归零解除（BoneGeneral.guardAlly 会挂）。
        // 迁到 TickStatuses（Enemy.cs:322，派发点 CombatController.cs:787/813），
        // 与 CombatRoundUpdate 只留一份，避免双倍递减。
        public override void TickStatuses(CombatContext context) =>
            BoneGeneralEnemy.TickGuardAb(context, this);

        // 原版 handleFear（SkeletonHorde.hx:52）：骷髅不吃恐惧，直接清掉并照常行动。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.SkeletonHorde.HandleFear.TheSkeletonsAren".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 SkeletonHorde.hx:58-63：
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

        // 动态复数已接：get_plural()（SkeletonHorde.hx:66）→ 上面的 Plural 覆写。
    }
}
