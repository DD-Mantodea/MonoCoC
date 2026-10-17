using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<RhinoBouncerEnemy>("coc:rhino_bouncer")]
    public class RhinoBouncerEnemy : Enemy
    {

        public bool Charging;

        public static readonly Identifier BackbrokenStatus = new("coc", "backbroken");

        public override void TickStatuses(CombatContext context)
        {
            var player = context.Player;

            var status = player.StatusEffectByType(BackbrokenStatus);

            if (status == null)
                return;

            if (status.Values[0] <= 0)
            {
                player.RemoveStatusEffect(BackbrokenStatus);

                player.IsCrippled = false;

                context.Append("Combat.RhinoBouncer.Backbroken.Recovered".Localized());

                return;
            }

            player.IsCrippled = true;

            status.Values[0] -= 1;

            if (status.Values[0] <= 0)
            {
                player.RemoveStatusEffect(BackbrokenStatus);

                player.IsCrippled = false;

                context.Append("Combat.RhinoBouncer.Backbroken.Recovered".Localized());
            }
            else
                context.Append("Combat.RhinoBouncer.Backbroken.StillCrippled".Localized());
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(2) + 15, 2.5f, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 3;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = 7 * 12;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "none";

            hair.Length = 0;

            Body.Get<HornsPart>().Id = BodyPartTypes.Rhino;

            WeaponName = "fists";

            WeaponVerb = "haymaker";

            WeaponAttack = 0;

            ArmorName = "thick hide";

            ArmorDefense = 35;

            Gems = Random.Shared.Next(5) + 250;

        }

    }
}