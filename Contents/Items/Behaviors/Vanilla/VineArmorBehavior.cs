using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<VineArmorBehavior>("coc:vine_armor")]
    public class VineArmorBehavior : ItemBehavior, IItemDescriptionBehavior, IItemEquipBehavior
    {
        public void OnEquip(Player player, Item item) => player.CreatePerk(PerkIds.AlrauneVines);

        public void OnRemove(Player player, Item item) => player.RemovePerk(PerkIds.AlrauneVines);

        public string GetDescription(Item item, Player player, string baseDescription)
        {
            var text = baseDescription + "\n" + "Items.VineArmor.Special".Localized();

            if (item.Stage > 2)
                text += "\n" + "Items.VineArmor.Thorns".Localized();

            if (item.Stage > 3)
                text += "\n" + "Items.VineArmor.Trip".Localized();

            if (item.Stage > 4)
                text += "\n" + "Items.VineArmor.Grab".Localized();

            return text;
        }
    }
}