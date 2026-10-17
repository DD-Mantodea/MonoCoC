using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public class FoodEffect(int hunger, float strength = 0, float toughness = 0, float lust = 0) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.Hunger = Math.Min(100, player.Hunger + hunger);

            if (strength != 0)
                player.Strength = Math.Max(0, player.Strength + strength);

            if (toughness != 0)
                player.Toughness = Math.Max(0, player.Toughness + toughness);

            if (lust != 0)
                player.Lust = Math.Min(100, player.Lust + lust);

            context.Write("Items.Food.Use".Localized(context.Item.Name));
        }
    }

    public class DrinkEffect(int hunger, float strength = 0, float toughness = 0, float lust = 0) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.Hunger = Math.Min(100, player.Hunger + hunger);

            if (strength != 0)
                player.Strength = Math.Max(0, player.Strength + strength);

            if (toughness != 0)
                player.Toughness = Math.Max(0, player.Toughness + toughness);

            if (lust != 0)
                player.Lust = Math.Min(100, player.Lust + lust);

            context.Write("Items.Drink.Use".Localized(context.Item.Name));
        }
    }

}