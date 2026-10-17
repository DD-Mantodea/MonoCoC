using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<DeBimboBehavior>("coc:de_bimbo")]
    public class DeBimboBehavior : ItemBehavior, IItemDescriptionBehavior
    {
        public string GetDescription(Item item, Player player, string baseDescription) =>

            player.HasPerk(PerkIds.BimboBrains) || player.HasPerk(PerkIds.FutaFaculties)

                ? "Items.DeBimbo.BimboDesc".Localized()

                : baseDescription;
    }
}