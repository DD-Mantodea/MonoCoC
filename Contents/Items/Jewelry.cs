using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [Register<Jewelry>("coc:jewelry")]
    public class Jewelry : Item
    {
        public static new readonly Jewelry Nothing = new() { Id = new Identifier("coc", "nothing"), Name = "nothing", LongName = "nothing" };

        public float EffectId;

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