using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items
{
    [Register<Armor>("coc:armor")]
    public class Armor : Item
    {
        public static new readonly Armor Nothing = new() { Id = new Identifier("coc", "nothing"), Name = "nothing", LongName = "nothing" };

        public float Defense;

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