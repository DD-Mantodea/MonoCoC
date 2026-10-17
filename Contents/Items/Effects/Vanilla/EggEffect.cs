using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public class EggEffect(bool large) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write("Items.Egg.Use".Localized());

            player.Hunger = Math.Min(100, player.Hunger + 20);

            if (player.Vaginas.Count > 0)
            {
                player.RemoveVagina();

                context.Write("Items.Egg.VaginaGone".Localized());
            }

            foreach (var cock in player.Cocks)
            {
                cock.Length += large ? 5 : 2;

                cock.Thickness += large ? 1.5f : 1f;
            }

            context.Write(large

                ? "Items.Egg.Large".Localized()

                : "Items.Egg.Small".Localized());
        }
    }

}