using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 GoblinBroodmother extends Goblin（src/classes/scenes/quests/urtaQuest/GoblinBroodmother.hx）
    // 只移植类本体；defeated/won 依赖 Urta 任务线场景（原版 GoblinBroodmother.hx:12-31）。
    [Register("coc:goblin_broodmother")]
    public class GoblinBroodmotherEnemy : GoblinEnemy
    {
        // 原版 new() 第一行是 super(true)：跳过 Goblin 的默认身体配置，下面这套才是母巢地精的身体。
        // C# 的 GoblinEnemy 把默认身体放在 virtual ConfigureBody()，这里整套覆写即可。
        protected override void ConfigureBody()
        {
            // 原版 createVagina(false, WETNESS_DROOLING=4, LOOSENESS_NORMAL=1)
            CreateVagina(false, 4, 1);

            // 原版 createStatusEffect(BonusVCapacity, 40)
            if (!HasStatusEffect(VanillaStatusEffects.BonusVCapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 40);

            // 原版 createBreastRow(Appearance.breastCupInverse("E")) → VanillaBreastCups.E = 7
            CreateBreastRow(VanillaBreastCups.E);

            // 原版 ass.analLooseness = LOOSENESS_TIGHT(1); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;
            anal.Wetness = 0;

            // 原版 createStatusEffect(BonusACapacity, 30)
            if (!HasStatusEffect(VanillaStatusEffects.BonusACapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 30);

            // 原版 tallness = 35 + Utils.rand(4)
            Tallness = 35 + Random.Shared.Next(4);

            // 原版 hips.rating = Hips.RATING_AMPLE(6) + 2；butt.rating = Butt.RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            // 原版 skin.tone = "dark green"
            Body.Get<SkinPart>().Tone = "dark green";

            // 原版 hair.color = "purple"; hair.length = 4
            var hair = Body.Get<HairPart>();

            hair.Color = "purple";
            hair.Length = 4;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 weaponName = "fists"; weaponVerb = "tiny punch"; weaponAttack = 20
            WeaponName = "fists";
            WeaponVerb = "tiny punch";
            WeaponAttack = 20;

            // 原版 armorName = "leather straps"
            ArmorName = "leather straps";

            // 原版 lustVuln = 0.5
            LustVulnerability = 0.5f;

            // 原版 gems = Utils.rand(5) + 5：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(5) + 5;

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：
            // 该字段只在原版 Monster.hx 的「grappling 待实现」注释块里被读取，是死字段，不移植。
        }

        // 原版 defeated()（GoblinBroodmother.hx:12-19）：输出 + Fuck/Leave 两个按钮。
        public override bool Defeated(bool hpVictory)
        {
            Scene.ClearOutput();
            Scene.OutputText("Combat.GoblinBroodmother.Defeated.TheGoblinBroodmother".Localized());
            Scene.OutputText("Combat.GoblinBroodmother.Defeated.YouCouldUse".Localized());
            Scene.ClearButtons();
            Scene.AddButton(0, "Fuck", () => SceneInstance<UrtaQuestScene>.Get().WinFuckAGoblinBroodmotherAsUrta());
            Scene.AddButton(14, "Leave", () => SceneInstance<UrtaQuestScene>.Get().NagaPleaseNagaStoleMyDick());

            return true;
        }

        // 原版 won()（GoblinBroodmother.hx:29-31）
        public override bool Won(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaLosesToGoblin();

            return true;
        }
    }
}
