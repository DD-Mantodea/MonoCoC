using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Attributes;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{
    [ItemCategory("coc:armor", "Icons/inventory_armor.svg", "Items.Type.Armor")]
    [Register<Armor>("coc:armor")]
    public class Armor : Item
    {
        public float Defense;

        public override void RegisterInteractions(ItemInteractionMenu menu, ItemInteractionContext context)
        {
            ItemInteractions.RegisterEquip(menu, context);

            base.RegisterInteractions(menu, context);
        }

        public override List<ItemStat> GetStats()
        {
            var stats = base.GetStats();

            if (Defense != 0)
                stats.Insert(0, new ItemStat("Items.Stats.Defense", Defense.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture)));

            return stats;
        }

        public string Perk = "";

        public bool SupportsBulge;

        public bool SupportsUndergarment = true;

        public int Tier;

        public override Armor PlayerEquip() => this;

        public override Armor PlayerRemove() => this;

        public override void Configure(ItemDefinition definition)
        {
            base.Configure(definition);

            Defense = definition.Defense;

            Perk = definition.Perk;

            SupportsBulge = definition.SupportsBulge;

            SupportsUndergarment = definition.SupportsUndergarment;

            MaxStackSize = 1;
        }

        public float DefenseFor(Player player) =>

            Behavior is IArmorDefenseBehavior behavior ? behavior.GetDefense(this, player) : Defense;
    }
}