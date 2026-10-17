using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public class BeeHoneyEffect(bool pure, bool special) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write(special

                ? "Items.BeHoney.Special".Localized()

                : "Items.BeHoney.Drink".Localized());

            player.Hunger = Math.Min(100, player.Hunger + 15);

            if (pure)
            {
                var loss = 1 + player.Corruption / 20;

                player.Corruption = Math.Max(0, player.Corruption - loss);

                if (player.Libido > 40)
                {
                    player.Libido = Math.Max(0, player.Libido - 3);

                    player.Lust = Math.Max(0, player.Lust - 20);
                }

                context.Write("Items.BeHoney.Pure".Localized());
            }

            if (context.Random.Next(2) == 0 && player.Intelligence < 80)
            {
                player.Intelligence += 0.1f * (80 - player.Intelligence);

                context.Write("Items.BeHoney.Smart".Localized());
            }

            var hair = player.Body.Get<HairPart>();

            if (hair.Length > 10 && context.Random.Next(5) == 0)
            {
                hair.Color = context.Random.Next(9) == 0 ? "black and yellow" : "shiny black";

                context.Write("Items.BeHoney.Hair".Localized());
            }
        }
    }

}