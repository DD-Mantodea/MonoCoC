using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    [Register<HealingPillEffect>("coc:healing_pill")]
    public class HealingPillEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            context.Write("Items.HealingPill.Use".Localized());

            var before = context.Player.Health;

            context.Player.HealthChange(50 + context.Player.Toughness);

            context.Write(before < context.Player.Health

                ? "Items.HealingPill.Healed".Localized()

                : "Items.HealingPill.NoHeal".Localized());

            var roll = context.Random.Next(100);

            if (roll < 70 && context.Player.Libido < 40)
            {
                context.Write("Items.HealingPill.Warmth".Localized());

                context.Player.Libido += 1;
            }
            else if (roll is >= 70 and <= 90)
            {
                context.Write("Items.HealingPill.Tingle".Localized());

                context.Player.Sensitivity += 4;
            }
            else if (roll > 90)
            {
                context.Write("Items.HealingPill.Orgasm".Localized());

                context.Player.Lust += 5;
            }
        }
    }
}