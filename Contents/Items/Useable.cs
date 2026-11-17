using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [ItemCategory("coc:consumable", "Icons/inventory_consumable.svg", "Items.Type.Consumable")]
    [Register<Useable>("coc:useable")]
    public class Useable : Item
    {
        public Action<Item>? UseAction;

        public override void RegisterInteractions(ItemInteractionMenu menu, ItemInteractionContext context)
        {
            ItemInteractions.RegisterUse(menu, context);

            base.RegisterInteractions(menu, context);
        }

        public override bool CanUse() => UseAction != null;

        public override bool UseItem()
        {
            if (UseAction == null)
                return false;

            UseAction(this);

            return true;
        }
    }
}