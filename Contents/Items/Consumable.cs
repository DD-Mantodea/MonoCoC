using MonoCoC.Contents.Items.Effects;
using MonoCoC.Registry;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [Register<Consumable>("coc:consumable")]
    public class Consumable : Item
    {
        public List<Identifier> Effects = [];

        public override bool CanUse() => (Effects.Count > 0 || ConsumableEffectRegistry.HasItem(Id)) && ConsumableEffectRegistry.CanUse(Id);

        public override bool UseItem() => false;

        public bool UseItem(ItemUseContext context)
        {
            if (Effects.Count == 0)
            {
                var direct = ConsumableEffectRegistry.ResolveItem(Id);

                if (direct == null)
                    return false;

                direct.Apply(context);

                return true;
            }

            foreach (var id in Effects)
                ConsumableEffectRegistry.Resolve(id)?.Apply(context);

            return true;
        }

        public override void Configure(ItemDefinition definition)
        {
            base.Configure(definition);

            Effects = definition.Effects;
        }
    }
}