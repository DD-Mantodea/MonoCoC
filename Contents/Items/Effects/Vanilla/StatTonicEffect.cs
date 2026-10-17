using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public class StatTonicEffect(
        float libido,

        float sensitivity,

        float corruption,

        float lust,

        float strength,

        float toughness) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.Libido = Math.Clamp(player.Libido + libido, 0, 100);

            player.Sensitivity = Math.Clamp(player.Sensitivity + sensitivity, 0, 100);

            player.Corruption = Math.Clamp(player.Corruption + corruption, 0, 100);

            player.Lust = Math.Clamp(player.Lust + lust, 0, 100);

            player.Strength = Math.Max(0, player.Strength + strength);

            player.Toughness = Math.Max(0, player.Toughness + toughness);

            context.Write("Items.Tonic.Use".Localized(context.Item.Name));
        }
    }

}