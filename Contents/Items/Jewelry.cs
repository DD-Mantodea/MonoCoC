using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [ItemCategory("coc:jewelry", "Icons/inventory_jewelry.svg", "Items.Type.Jewelry")]
    [Register<Jewelry>("coc:jewelry")]
    public class Jewelry : Item
    {
        public float EffectId;

        public override void RegisterInteractions(ItemInteractionMenu menu, ItemInteractionContext context)
        {
            ItemInteractions.RegisterEquip(menu, context);

            base.RegisterInteractions(menu, context);
        }

        public float EffectMagnitude;

        public string Perk = "";

        public override Jewelry PlayerEquip() => this;

        public override Jewelry PlayerRemove() => this;

        public override void Configure(ItemDefinition definition)
        {
            base.Configure(definition);

            EffectId = definition.EffectId;

            EffectMagnitude = definition.EffectMagnitude;

            Perk = definition.Perk;

            MaxStackSize = 1;

            Behavior ??= ItemBehaviorRegistry.Resolve(JewelryIds.Effects);
        }
    }
}