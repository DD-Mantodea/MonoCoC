using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Items.Behaviors
{
    public interface IItemEquipBehavior : IItemBehavior
    {
        void OnEquip(Player player, Item item);

        void OnRemove(Player player, Item item);
    }
}