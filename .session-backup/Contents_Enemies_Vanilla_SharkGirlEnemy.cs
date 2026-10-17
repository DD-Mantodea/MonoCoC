using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 SharkGirl extends Monster（src/classes/scenes/places/boat/SharkGirl.hx）
    // 只移植类本体；defeated/won 依赖 SharkGirlScene（未移植，原版 SharkGirl.hx:41-52）。
    [Register("coc:shark_girl")]
    public class SharkGirlEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createVagina(false, WETNESS_DROOLING=4, LOOSENESS_NORMAL=1)
            CreateVagina(false, 4, 1);

            // 原版 createStatusEffect(BonusVCapacity, 15)
            if (!HasStatusEffect(VanillaStatusEffects.BonusVCapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 15);

            // 原版 createBreastRow(Appearance.breastCupInverse("D")) → VanillaBreastCups.D = 4
            CreateBreastRow(VanillaBreastCups.D);

            // 原版 ass.analLooseness = LOOSENESS_TIGHT(1); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;
            anal.Wetness = 0;

            // 原版 createStatusEffect(BonusACapacity, 40)
            if (!HasStatusEffect(VanillaStatusEffects.BonusACapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 40);

            // 原版 tallness = 5 * 12 + 5
            Tallness = 5 * 12 + 5;

            // 原版 hips.rating = RATING_AMPLE(6) + 2；butt.rating = RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            // 原版 skin.tone = "gray"; hair.color = "silver"; hair.length = 16
            Body.Get<SkinPart>().Tone = "gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "silver";
            hair.Length = 16;

            // 原版 weaponName = "shark teeth"; weaponVerb = "bite"; weaponAttack = 3
            WeaponName = "shark teeth";
            WeaponVerb = "bite";
            WeaponAttack = 3;

            // 原版 normalAttackAvoid.doParry = false; doCounter = false
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            // 原版 armorName = "tough skin"; armorDef = 5
            ArmorName = "tough skin";
            ArmorDefense = 5;

            // 原版 lustVuln = .9
            LustVulnerability = 0.9f;

            // 原版 gems = Utils.rand(15) + 5：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(15) + 5;

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            //
            // 原版 defeated()：SharkGirl.hx:41-43（boat.sharkGirlScene.sharkWinChoices）
            // 原版 won()：SharkGirl.hx:45-52（boat.sharkGirlScene.sharkLossRape）
            // C# 的 SharkGirlScene 未移植（原版 SharkGirl.hx:41-52；Contents/Scenes 全量无此类），故不覆写 Defeated()/Won()。
        }

        // 原版 outputAttack（SharkGirl.hx:32-39）：先走基底战报，命中后 1/3 概率撕咬流血。
        public override void OutputAttack(CombatContext context, float damage)
        {
            base.OutputAttack(context, damage);

            if (damage > 0 && context.Random.Next(3) == 0 && context.Player.Bleed(this))
                context.Append("Combat.SharkGirl.OutputAttack.TheSharkGirl".Localized());
        }

        // 原版 defeated()：SharkGirl.hx:41-43（game.boat.sharkGirlScene.sharkWinChoices()）
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Boat.SharkGirlScene>.Get().SharkWinChoices();

            return true;
        }

        // 原版 won()：SharkGirl.hx:45-52（pcCameWorms 走 endLustLoss；否则 sharkLossRape）
        // C# 侧 pcCameWorms 对应 came_worms_after_combat 旗标（同 AmarokEnemy.cs:55 的约定）。
        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(MonoCoC.Contents.Flags.VanillaFlags.CameWormsAfterCombat) > 0)
            {
                MonoCoC.Contents.Scenes.Scene.OutputText("Combat.SharkGirl.Won.YourFoeDoesn".Localized());

                return false;
            }

            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Boat.SharkGirlScene>.Get().SharkLossRape();

            return true;
        }
    }
}
