using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MiffixEnemy>("coc:miffix")]
    public class MiffixEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(2) + 10, 2.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 2;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = Random.Shared.Next(24) + 25;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 5;

            WeaponName = "claws";

            WeaponVerb = "claw-slash";

            ArmorName = "leathery skin";

            Body.Get<WingsPart>().Id = BodyPartTypes.Imp;

            Gems = Random.Shared.Next(5) + 5;

        }
    }
}