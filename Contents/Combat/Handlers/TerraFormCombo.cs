using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5834-5906 tfComboReady / tfCombod
    public static class TerraFormCombo
    {
        private static readonly Identifier TerraMastery = new("coc", "TerrestrialFire");

        public static int Mastery(Player player) => player.MasteryLevel(TerraMastery);

        public static bool ComboReady(Player player, string spell)
        {
            var mastery = Mastery(player);

            return spell switch
            {
                "Molten Plate" => mastery >= 1

                                        && (player.HasStatusEffect(StatusEffectIds.TFInflame)

                                            || player.HasStatusEffect(StatusEffectIds.TFPlate)),

                "Shell" => mastery >= 2 && player.HasStatusEffect(StatusEffectIds.TFMoltenPlate),

                "Eruption" => mastery >= 2 && player.HasStatusEffect(StatusEffectIds.TFQuake),

                "Meteor Shower" => mastery >= 3 && player.HasStatusEffect(StatusEffectIds.TFScorch),

                "Terra Star" => mastery >= 5

                                        && player.HasStatusEffect(StatusEffectIds.TFMoltenPlate)

                                        && player.HasStatusEffect(StatusEffectIds.TFQuake)

                                        && player.HasStatusEffect(StatusEffectIds.TFScorch)

                                        && player.Strength + player.Intelligence >= 150

                                        && player.SpellMod() >= 2,

                _ => false,
            };
        }

        public static bool Combod(Player player, string spell)
        {
            return spell switch
            {
                "Plate" or "Inflame" => ComboReady(player, "Molten Plate") || ComboReady(player, "Shell"),

                "Molten Plate" => ComboReady(player, "Shell"),

                "Spout" => ComboReady(player, "Eruption"),

                "Stone Knives" => ComboReady(player, "Meteor Shower"),

                "Terra Core" or "Terra Flames" => ComboReady(player, "Terra Star"),

                _ => false,
            };
        }
    }
}