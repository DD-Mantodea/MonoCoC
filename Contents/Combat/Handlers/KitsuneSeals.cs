using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public enum KitsuneSealType
    {
        Attack = 0,

        Tease = 1,

        Spells = 2,

        Items = 3,

        Movement = 4,

        Physical = 5,

        Magical = 6,
    }

    public static class KitsuneSeals
    {
        public const int SealDuration = 4;

        public static int CalculateAttackResist(Player player)
        {
            var resist = (int)Math.Min(player.Intelligence / 2f, 40);

            if (player.HasPerk(PerkIds.Whispered))
                resist += 20;

            if (player.IsPureEnough(20))
                resist += (int)(20 - player.CorAdjustedDown());

            if (player.HasPerk(PerkIds.EnlightenedNinetails) || player.HasPerk(PerkIds.CorruptedNinetails))
                resist += 30;

            return resist;
        }

        public static void Seal(CombatContext context, KitsuneSealType? type = null)
        {
            var player = context.Player;

            var selected = type ?? (KitsuneSealType)context.Random.Next(7);

            switch (selected)
            {
                case KitsuneSealType.Attack:
                    context.WriteAction("Combat.Kitsune.Seal.Attack.One".Localized());

                    context.WriteAction("Combat.Kitsune.Seal.Attack.Two".Localized());

                    context.WriteAction("Combat.Kitsune.Seal.Attack.Three".Localized());

                    break;

                case KitsuneSealType.Tease:
                    context.WriteAction("Combat.Kitsune.Seal.Tease".Localized());

                    break;

                case KitsuneSealType.Spells:
                    context.WriteAction("Combat.Kitsune.Seal.Spells".Localized());

                    break;

                case KitsuneSealType.Items:
                    context.WriteAction("Combat.Kitsune.Seal.Items".Localized());

                    break;

                case KitsuneSealType.Movement:
                    context.WriteAction("Combat.Kitsune.Seal.Movement".Localized());

                    break;

                case KitsuneSealType.Physical:
                    context.WriteAction("Combat.Kitsune.Seal.Physical.One".Localized());

                    context.WriteAction("Combat.Kitsune.Seal.Physical.Two".Localized());

                    context.WriteAction("Combat.Kitsune.Seal.Physical.Three".Localized());

                    break;

                case KitsuneSealType.Magical:
                    context.WriteAction("Combat.Kitsune.Seal.Magical.One".Localized());

                    context.WriteAction("Combat.Kitsune.Seal.Magical.Two".Localized());

                    context.WriteAction("Combat.Kitsune.Seal.Magical.Three".Localized());

                    break;
            }

            player.CreateStatusEffect(StatusEffectIds.Sealed, SealDuration, (int)selected);

            var resist = CalculateAttackResist(player);

            if (resist >= context.Random.Next(100))
            {
                context.WriteAction("Combat.Kitsune.Seal.Resisted".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Sealed);
            }
        }
    }
}