using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<GoblinBroodmotherEnemy>("coc:goblin_broodmother")]
    public class GoblinBroodmotherEnemy : GoblinEnemy
    {

        protected override void ConfigureBody()
        {

            CreateVagina(false, 4, 1);

            if (!HasStatusEffect(StatusEffectIds.BonusVCapacity))
                CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 35 + Random.Shared.Next(4);

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "purple";

            hair.Length = 4;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            WeaponName = "fists";

            WeaponVerb = "tiny punch";

            WeaponAttack = 20;

            ArmorName = "leather straps";

            LustVulnerability = 0.5f;

            Gems = Random.Shared.Next(5) + 5;

        }

        public override bool Defeated(bool hpVictory)
        {
            Scene.ClearOutput();

            Scene.OutputText("Combat.GoblinBroodmother.Defeated.TheGoblinBroodmother".Localized());

            Scene.OutputText("Combat.GoblinBroodmother.Defeated.YouCouldUse".Localized());

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Fuck".Localized(), () => SceneInstance<UrtaQuestScene>.Get().WinFuckAGoblinBroodmotherAsUrta());

            Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<UrtaQuestScene>.Get().NagaPleaseNagaStoleMyDick());

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaLosesToGoblin();

            return true;
        }
    }
}