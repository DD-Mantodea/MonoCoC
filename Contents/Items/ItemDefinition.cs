using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{
    public class ItemDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Type = "default";

        public string Behavior = "";

        public string ShortNameKey = "";

        public string LongNameKey = "";

        public string DescriptionKey = "";

        public string UseTextKey = "";

        public string HeaderKey = "";

        public float Value;

        public int MaxStackSize = 5;

        public bool InvUseOnly;

        public int Durability;

        public string DegradesInto = "";

        public List<string> Tags = [];

        public float Attack;

        public string Verb = "";

        public float Accuracy = 1;

        public float Defense;

        public float Block;

        public string Perk = "";

        public bool TwoHanded;

        public bool SupportsBulge;

        public bool SupportsUndergarment = true;

        public string UndergarmentSlot = "";

        public int Sexiness;

        public int ArmorDefense;

        public float EffectId;

        public float EffectMagnitude;

        public string Effect = "";

        public List<Identifier> Effects = [];

        public string LegacyId = "";

        public static bool operator ==(ItemDefinition item, Identifier id) => item.Id == id;

        public static bool operator !=(ItemDefinition item, Identifier id) => item.Id != id;

        public override bool Equals(object? obj) => obj switch
        {
            ItemDefinition other => Id.Equals(other.Id),

            Identifier id => Id.Equals(id),

            _ => false,
        };

        public override int GetHashCode() => Id.GetHashCode();
    }
}