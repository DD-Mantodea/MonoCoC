using MonoCoC.Registry;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [Register<Shield>("coc:shield")]
    public class Shield : Item
    {
        public static new readonly Shield Nothing = new() { Id = new Identifier("coc", "nothing"), Name = "nothing", LongName = "nothing" };

        public float Block;

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