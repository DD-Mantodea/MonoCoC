using MonoCoC.Registry;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [Register<Undergarment>("coc:undergarment")]
    public class Undergarment : Item
    {
        public static new readonly Undergarment Nothing = new() { Id = new Identifier("coc", "nothing"), Name = "nothing", LongName = "nothing" };

        public const int TypeNone = 0;

        public const int TypeUpperwear = 1;

        public const int TypeLowerwear = 2;

        public int Type;

        public string Perk = "";

        public int Sexiness;

        public int ArmorDefense;

        public override Undergarment PlayerEquip() => this;

        public override Undergarment PlayerRemove() => this;

        public override void Configure(ItemDefinition definition)
        {
            base.Configure(definition);

            Type = definition.UndergarmentSlot switch
            {
                "upper" => TypeUpperwear,

                "lower" => TypeLowerwear,

                _ => TypeNone
            };

            Perk = definition.Perk;

            Sexiness = definition.Sexiness;

            ArmorDefense = definition.ArmorDefense;

            MaxStackSize = 1;
        }
    }
}