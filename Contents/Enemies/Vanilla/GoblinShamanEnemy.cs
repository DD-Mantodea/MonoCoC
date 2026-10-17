using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<GoblinShamanEnemy>("coc:goblin_shaman")]
    public class GoblinShamanEnemy : GoblinEnemy
    {
        protected override void ConfigureBody()
        {

            CreateVagina(false, 4, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 44 + Random.Shared.Next(7);

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "indigo";

            hair.Length = 4;

            WeaponName = "wizard staff";

            WeaponVerb = "bludgeon";

            WeaponAttack = 14;

            ArmorName = "fur loincloth";

            ArmorDefense = 6;

            Fatigue = 0;

            LustVulnerability = 0.4f;

            Gems = Random.Shared.Next(15) + 15;

        }

        public override string Description =>

            base.Description

            + (CoC.Instance?.Player?.HasCock() == true

                ? "Npcs.GoblinShaman.DescCockSuffix".Localized()

                : "");

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.GoblinShamanScene>
                .Get().GoblinShamanRapeIntro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.Gender == Creatures.Gender.Genderless)
            {
                CombatController.Instance?.Context.WriteAction(
                    "Combat.GoblinShaman.Won.YouCollapseIn".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.GoblinShamanScene>
                .Get().GoblinShamanBeatYaUp();

            return true;
        }
    }
}
