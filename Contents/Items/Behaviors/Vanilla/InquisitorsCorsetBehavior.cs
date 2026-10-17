using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<InquisitorsCorsetBehavior>("coc:inquisitors_corset")]
    public class InquisitorsCorsetBehavior : ItemBehavior, IItemDescriptionBehavior, IItemEquipBehavior
    {
        public string GetDescription(Item item, Player player, string baseDescription) =>

            baseDescription + "\n" + "Items.InquisitorsCorset.Special".Localized();

        public void OnEquip(Player player, Item item) => player.CreatePerk(PerkIds.BloodMage);

        public void OnRemove(Player player, Item item) => player.RemovePerk(PerkIds.BloodMage);
    }
}