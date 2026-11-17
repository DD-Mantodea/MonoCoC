using MonoCoC.Registry;
using MonoCoC.Attributes;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{
    [ItemCategory("coc:shield", "Icons/inventory_shield.svg", "Items.Type.Shield")]
    [Register<Shield>("coc:shield")]
    public class Shield : Item
    {
        public float Block;

        public override void RegisterInteractions(ItemInteractionMenu menu, ItemInteractionContext context)
        {
            ItemInteractions.RegisterEquip(menu, context);

            base.RegisterInteractions(menu, context);
        }

        public override List<ItemStat> GetStats()
        {
            var stats = base.GetStats();

            if (Block != 0)
                stats.Insert(0, new ItemStat("Items.Stats.Block", Block.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture)));

            return stats;
        }

        public string Perk = "";

        public string Weight = "Medium";

        public override Shield PlayerEquip() => this;

        public override Shield PlayerRemove() => this;

        public override void Configure(ItemDefinition definition)
        {
            base.Configure(definition);

            Block = definition.Block;

            Perk = definition.Perk;

            MaxStackSize = 1;
        }
    }
}