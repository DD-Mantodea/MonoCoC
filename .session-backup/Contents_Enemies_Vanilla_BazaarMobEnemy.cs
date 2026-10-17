using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/BazaarMob.hx（extends Monster）
    // 「一群暴徒」：生命值按 100 点/只折算，回合内每只各挥一次武器。
    [Register("coc:bazaar_mob")]
    public class BazaarMobEnemy : Enemy
    {
        // 原版 unitHP = 50 / unitAmount = 4（BazaarMob.hx:96-97）。
        // 注意原版 maxHP() 覆写用的是 unitAmount * 100（BazaarMob.hx:7-9），不是 unitHP；
        // unitHP 在原版里没有被读取，保留字段只为对齐原文。
        public float UnitHp = 50f;

        public int UnitAmount = 4;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（BazaarMob.hx:61-99）的赋值顺序照抄 ──
            // a = "the " / short = "Bazaar Mob" / long = "" / initedGenitals = true；
            // pronoun1-3 = they/them/their、plural = true 由数据表表达。

            // createBreastRow(Appearance.breastCupInverse("E")) = 7
            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / ass.analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            // createStatusEffect(StatusEffects.BonusACapacity, 30, 0, 0, 0)
            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 30);

            // tallness = 35 + Utils.rand(4)
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

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：死字段，无实际作用。
            // 原版 drop = new WeightedDrop()（空表，不掉落）：数据表 loot 留空。

            // 原版 lustVuln = 0.5
            LustVulnerability = 0.5f;

            Gems = Random.Shared.Next(5) + 5;

            // 原版 maxHP() 覆写为 unitAmount * 100（BazaarMob.hx:7-9）。
            // C# 的 Creature.MaxHealth 不是虚属性，这里用一条动态 MaxHealth 加成把它改写成 unitAmount*100：
            //   基础值 = Creature 构造函数里的 provider（Toughness*2 + 50 + Level*15，上限 9999），
            //   加成 = 目标值 - 基础值，所以 (基础 + 加成) == unitAmount*100。
            // 数据表的 bonusHealth 已改成 0（原版这条覆写会吞掉 bonusHP）。
            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => UnitAmount * 100f - Math.Min(9999f, Toughness * 2 + 50 + Level * 15),
                new TemporarySource(Guid.NewGuid()));

            // 原版从满血开始（HP = maxHP() = 4 * 100），上面重算后同步一次。
            Health = MaxHealth;
        }

        // 原版 performCombatAction（BazaarMob.hx:11-20）：
        //   unitAmount = fceil(HP / 100) → 每只暴徒各自 recombobulateWeapon() + eAttack()
        // C# 的 ChooseCombatAction 只能返回一招，所以整段放在 BeforeCombatAction 里自己做。
        public override bool BeforeCombatAction(CombatContext context)
        {
            UnitAmount = (int)MathF.Ceiling(Health / 100f);

            context.Append("\n");

            for (var i = 0; i < UnitAmount; i++)
            {
                RecombobulateWeapon();

                CombatController.Instance!.Resolver.EnemyBasicAttack();

                // 原版 eAttack 在玩家倒下后仍会继续循环；这里保持一致，不额外 break。
            }

            return true;
        }

        // 原版 recombobulateWeapon（BazaarMob.hx:22-50）：Utils.rand(4) → 0..3。
        private void RecombobulateWeapon()
        {
            switch (Random.Shared.Next(4))
            {
                case 0:
                    WeaponVerb = "stab";
                    WeaponName = "spear";
                    WeaponAttack = 25;
                    break;
                case 1:
                    WeaponVerb = "slash";
                    WeaponName = "scimitar";
                    WeaponAttack = 25;
                    break;
                case 2:
                    WeaponVerb = "crush";
                    WeaponName = "mace";
                    WeaponAttack = 15;
                    break;
                case 3:
                    WeaponVerb = "bash";
                    WeaponName = "club";
                    WeaponAttack = 30;
                    break;
                default:
                    // 原版 default（BazaarMob.hx:45-48）：Utils.rand(4) 永远不会走到，保留以对齐原文。
                    WeaponVerb = "pick";
                    WeaponName = "pickaxe";
                    WeaponAttack = 20;
                    break;
            }
        }

        // 原版 defeated() / won() 都是空实现（BazaarMob.hx:53-59），不需要接场景。
    }
}
