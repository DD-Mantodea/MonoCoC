using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ImpEnemy>("coc:imp")]
    public class ImpEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(11 + Random.Shared.Next(2), 2.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 1;

            CreateBreastRow(0f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 1;

            Tallness = 25 + Random.Shared.Next(24);

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 5;

            ArmorName = "leathery skin";

            WeaponName = "claws";

            WeaponVerb = "claw-slash";

            Gems = 5 + Random.Shared.Next(5);
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.DemonsDefeated, CoC.Instance.Flags.Get(GameFlagIds.DemonsDefeated) + 1);

            return false;
        }
    }
}