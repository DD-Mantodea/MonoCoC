using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 UnlabeledBrownMilkBottle.hx ────────────────────────────────
    public class UnlabeledBrownMilkBottleEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            if (!player.Udder.HasUdder)
            {
                if (player.IsMale())
                    context.Write("Items.UnlabeledMilk.GrowUdderMale".Localized());
                else if (player.IsFemale())
                    context.Write("Items.UnlabeledMilk.GrowUdderFemale".Localized());

                player.Udder.HasUdder = true;

                return;
            }

            if (player.Udder.Fullness >= 1000)
            {
                context.Write("Items.UnlabeledMilk.Overflow".Localized());

                player.Udder.Fullness = 0;
            }
            else
            {
                context.Write("Items.UnlabeledMilk.Fill".Localized());

                player.Udder.Fullness += 10;
            }
        }
    }
}