using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    public enum SpellBookKind
    {
        White,

        Black,

        Gray
    }

    public class SpellBookEffect(SpellBookKind kind) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write(kind switch
            {
                SpellBookKind.White => "Items.WhiteBook.Use".Localized(),

                SpellBookKind.Black => "Items.BlackBook.Use".Localized(),

                _ => "Items.GrayBook.Use".Localized()
            });

            RaiseIntelligence(context);

            switch (kind)
            {
                case SpellBookKind.White:
                    ApplyWhite(context);

                    break;

                case SpellBookKind.Black:
                    ApplyBlack(context);

                    break;

                default:
                    ApplyGray(context);

                    break;
            }
        }

        public static void ApplyWhite(ItemUseContext context)
        {
            var player = context.Player;

            if (player.Intelligence >= 25 && !player.HasStatusEffect(StatusEffectIds.KnowsCharge))
            {
                context.Write("Items.WhiteBook.SpellCharge".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsCharge);

                return;
            }

            if (player.Intelligence >= 30 && !player.HasStatusEffect(StatusEffectIds.KnowsBlind))
            {
                context.Write("Items.WhiteBook.SpellBlind".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsBlind);

                return;
            }

            if (player.Intelligence >= 40 && !player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
            {
                context.Write("Items.WhiteBook.SpellWhitefire".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsWhitefire);
            }
        }

        public static void ApplyBlack(ItemUseContext context)
        {
            var player = context.Player;

            if (player.Intelligence >= 25 && !player.HasStatusEffect(StatusEffectIds.KnowsArouse))
            {
                context.Write("Items.BlackBook.SpellArouse".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsArouse);

                return;
            }

            if (player.Intelligence >= 30 && !player.HasStatusEffect(StatusEffectIds.KnowsHeal))
            {
                context.Write("Items.BlackBook.SpellHeal".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsHeal);

                return;
            }

            if (player.Intelligence >= 40 && !player.HasStatusEffect(StatusEffectIds.KnowsMight))
            {
                context.Write("Items.BlackBook.SpellMight".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsMight);
            }
        }

        public static void ApplyGray(ItemUseContext context)
        {
            var player = context.Player;

            if (player.Intelligence >= 55 && !player.HasStatusEffect(StatusEffectIds.KnowsTKBlast))
            {
                context.Write("Items.GrayBook.SpellTKBlast".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsTKBlast);

                return;
            }

            if (player.Intelligence >= 70 && !player.HasStatusEffect(StatusEffectIds.KnowsLeech))
            {
                context.Write("Items.GrayBook.SpellLeech".Localized());

                player.CreateStatusEffect(StatusEffectIds.KnowsLeech);
            }
        }

        public static void RaiseIntelligence(ItemUseContext context)
        {
            var player = context.Player;

            if (player.Intelligence < 30)
            {
                context.Write("Items.Book.IntHigh".Localized());

                player.Intelligence += 4;
            }
            else if (player.Intelligence < 60)
            {
                context.Write("Items.Book.IntMedium".Localized());

                player.Intelligence += 2;
            }
            else if (player.Intelligence < 80)
            {
                context.Write("Items.Book.IntLow".Localized());

                player.Intelligence += 1;
            }
            else
            {
                context.Write("Items.Book.IntNone".Localized());

                player.Intelligence += 0.6f;
            }
        }
    }
}