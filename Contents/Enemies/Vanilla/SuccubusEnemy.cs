using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SuccubusEnemy>("coc:succubus")]
    public class SuccubusEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateBreastRow(BreastCups.G);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 35 + Random.Shared.Next(4);

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "ivory";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 4;

            WeaponName = "fists";

            WeaponVerb = "punch";

            ArmorName = "skin";

            Gems = Random.Shared.Next(5) + 5;

        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.SuccubusScene>
                .Get().WinAgainstSuccubus();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.SuccubusScene>
                .Get().LoseToSuccubus();

            return true;
        }
    }
}
