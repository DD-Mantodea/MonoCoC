using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public class DraftEffect(bool incubi, bool purified, bool fuck) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            if (incubi)
            {
                context.Write(purified

                    ? "Items.Draft.Purified".Localized()

                    : "Items.Draft.Incubi".Localized());

                player.Intelligence += 1;

                player.Libido = Math.Min(100, player.Libido + 2);

                player.Sensitivity = Math.Min(100, player.Sensitivity + 1);

                player.Lust = Math.Min(100, player.Lust + 5);

                if (!purified)
                    player.Corruption = Math.Min(100, player.Corruption + 1);

                return;
            }

            context.Write(fuck

                ? "Items.Draft.Fuck".Localized()

                : "Items.Draft.Lust".Localized());

            player.Lust = Math.Min(100, player.Lust + 30);

            player.Libido = Math.Min(100, player.Libido + 2);

            player.Sensitivity = Math.Min(100, player.Sensitivity + 1);
        }
    }

}