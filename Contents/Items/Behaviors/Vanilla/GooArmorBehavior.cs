using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<GooArmorBehavior>("coc:goo_armor")]
    public class GooArmorBehavior : ItemBehavior, IItemEquipBehavior
    {
        public void OnEquip(Player player, Item item) =>

            CoC.Instance.Flags.Set(GameFlagIds.ValariaAtCamp, 0);

        public void OnRemove(Player player, Item item) =>

            CoC.Instance.Flags.Set(GameFlagIds.ValariaAtCamp, 1);
    }
}