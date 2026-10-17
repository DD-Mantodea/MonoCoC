using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<GoblinAssassinEnemy>("coc:goblin_assassin")]
    public class GoblinAssassinEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 90);

            CreateBreastRow(BreastCups.E);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;

            anal.Wetness = 0;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 50);

            Tallness = 35 + Random.Shared.Next(4);

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "blue";

            hair.Length = 7;

            WeaponName = "needles";

            WeaponVerb = "stabbing needles";

            ArmorName = "leather straps";

            Gems = Random.Shared.Next(50) + 25;

            CreatePerk(PerkIds.Evade);

            AdditionalExperience = 150;

        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<GoblinAssassinScene>.Get().GobboAssassinRapeIntro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<GoblinAssassinScene>.Get().GobboAssassinBeatYaUp();

            return true;
        }
    }
}