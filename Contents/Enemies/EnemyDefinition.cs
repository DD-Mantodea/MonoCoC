using MonoCoC.Contents.Tags;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies
{
    public class EnemyLoot
    {
        public Identifier Item;

        public float Chance = 1f;

        public int Min = 1;

        public int Max = 1;

        public float Weight = 1;
    }

    public class EnemyEquipment
    {
        public Identifier Item;

        public float Weight = 1f;
    }

    public class EnemyAvoidProfile
    {
        public bool Dodge = true;

        public bool Parry = true;

        public bool Block = true;

        public bool Counter = true;

        public bool Fatigue = true;
    }

    public class EnemyDefinition : IIdentifiable, ITaggable
    {
        public Identifier Id { get; set; }

        public string Type = "";

        public string NameKey = "";

        public string DescriptionKey = "";

        public string Article = "the ";

        public float Level;

        public float Health;

        public float BonusHealth;

        public float Lust;

        public float Strength;

        public float Toughness;

        public float Speed;

        public float Intelligence;

        public float Libido;

        public float Sensitivity;

        public float Corruption;

        public int Gems;

        public float Experience;

        public Identifier AiId;

        public List<EnemyLoot> Loot = [];

        public bool ChainLoot;

        public bool WeightedLoot;

        public List<string> Tags = [];

    IReadOnlyList<string> ITaggable.Tags => Tags;

        public EnemyAvoidProfile AttackAvoid = new();

        public Identifier DefeatScene;

        public Identifier VictoryScene;

        public Identifier FleeScene;

        public bool Plural;

        public string Pronoun1 = "";

        public string Pronoun2 = "";

        public string Pronoun3 = "";

        public List<EnemyEquipment> Weapons = [];

        public List<EnemyEquipment> Armors = [];

        public List<EnemyEquipment> Shields = [];

        public float? WeaponAttack;

        public float? ArmorDefense;

        public float? ShieldBlock;

        public float? FireResistance;
    }
}