using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Attributes;
using System.Collections.Generic;

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

        public override List<ItemStat> GetStats()
        {
            var stats = base.GetStats();

            var magnitude = EffectMagnitude.ToString("0.##");

            switch ((int)EffectId)
            {
                case 1:
                    stats.Add(new("Items.Stats.MinLibido", "+" + magnitude));
                    break;
                case 2:
                    stats.Add(new("Items.Stats.Fertility", "+" + magnitude));
                    break;
                case 3:
                    stats.Add(new("Items.Stats.CriticalChance", "+" + magnitude + "%"));
                    break;
                case 4:
                    stats.Add(new("Items.Stats.HealthRegen", "+" + magnitude + "%"));
                    break;
                case 5:
                    stats.Add(new("Items.Stats.MaxHealth", "+" + magnitude));
                    break;
                case 6:
                    stats.Add(new("Items.Stats.PhysicalDamage", "+" + magnitude + "%"));
                    break;
                case 7:
                    stats.Add(new("Items.Stats.SpellPower", "+" + magnitude));
                    break;
                case 8:
                    stats.Add(new("Items.Stats.MinLibido", "-" + magnitude));
                    break;
                case 10:
                    stats.Add(new("Items.Stats.SpellPower", "+" + (EffectMagnitude * 10).ToString("0.##")));
                    break;
                case 11:
                    stats.Add(new("Items.Stats.Accuracy", "+" + magnitude));
                    break;
                case 13:
                    stats.Add(new("Items.Stats.Dodge", "+20"));
                    stats.Add(new("Items.Stats.CriticalChance", "+15%"));
                    stats.Add(new("Items.Stats.MaxHealth", "-40%"));
                    break;
            }

            return stats;
        }

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