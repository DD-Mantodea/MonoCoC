using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public class CaninePepperEffect(string flavor) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write(flavor.Localized());

            player.Hunger = Math.Min(100, player.Hunger + 10);

            player.Libido = Math.Min(100, player.Libido + 2 + context.Random.Next(4));

            player.Lust = Math.Min(100, player.Lust + 5 + context.Random.Next(5));

            player.Corruption = Math.Min(100, player.Corruption + 2 + context.Random.Next(4));

            player.Strength += 1;

            player.Speed += 1;

            context.Write("Items.CaninePepperepper.After".Localized());
        }
    }

}