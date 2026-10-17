using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Registry;
using MonoCoC.Attributes;
using System.Linq;

namespace MonoCoC.Contents.Items
{
    [Register<Weapon>("coc:weapon")]
    [Register<Weapon>("coc:sword")]
    [Register<Weapon>("coc:dagger")]
    [Register<Weapon>("coc:spear")]
    [Register<Weapon>("coc:mace")]
    [Register<Weapon>("coc:axe")]
    [Register<Weapon>("coc:blunt")]
    [Register<Weapon>("coc:staff")]
    [Register<Weapon>("coc:whip")]
    [Register<Weapon>("coc:fist")]
    public class Weapon : Item
    {
        public const string UnarmedTag = "coc:unarmed";

        public static readonly Weapon Fists = new()
        {
            Id = new Identifier("coc", "fists"),

            Name = "fists",

            LongName = "fists",

            Tags = [UnarmedTag]
        };

        public static readonly Weapon Claws = new()
        {
            Id = new Identifier("coc", "claws"),

            Name = "claws",

            LongName = "claws",

            Tags = [UnarmedTag, "claw", "bare_claw"]
        };

        static Weapon() =>
            // 原版 UnarmedClaws.hx:15：徒手爪的攻击/穿透按玩家爪子类型动态计算
            Claws.Behavior = new Behaviors.Vanilla.UnarmedClawsBehavior();

        public float Attack;

        public string Verb = "";

        public float Accuracy = 1;

        public string Weight = "Medium";

        public string Perk = "";

        public bool TwoHanded;

        public bool IsUnarmed() => Matches("unarmed");

        public bool IsFist() => Matches("fist");

        public bool IsBareFist() => Matches("bare_fist");

        public bool IsClaw() => Matches("claw");

        public bool IsBareClaw() => Matches("bare_claw");

        public bool IsBow() => Matches("bow");

        public bool IsCrossbow() => Matches("crossbow");

        public bool IsFirearm() => Matches("firearm");

        public bool IsHolySword() => Matches("holy_sword");

        public bool IsUnholy() => Matches("unholy");

        public bool IsSpear() => Matches("spear");

        public bool IsKnife() => Matches("knife");

        public bool IsAxe() => Matches("axe");

        public bool IsStaff() => Matches("staff");

        public bool IsMagicStaff() => Matches("magic_staff");

        public bool IsPolearm() => Matches("polearm");

        public bool IsScythe() => Matches("scythe");

        public bool IsWhip() => Matches("whip");

        public bool IsKatana() => Matches("katana");

        public bool IsSummoned() => Matches("summoned");

        public bool IsMelting() => Matches("melting");

        public bool IsCunning() => Matches("cunning");

        public bool IsAphrodisiac() => Matches("aphrodisiac");

        public bool IsAttached() => Matches("attached");

        public bool IsLarge() => Matches("large");

        private static readonly (string Match, string Id)[] MasteryTags =

        [
            ("1hsword", "1H Sword"), ("2hsword", "2H Sword"), ("knife", "Knife"),

            ("1hblunt", "1H Blunt"), ("2hblunt", "2H Blunt"), ("spear", "Spear"),

            ("axe", "Axe"), ("staff", "Staff"), ("polearm", "Polearm"), ("scythe", "Scythe"),

            ("whip", "Whip"), ("crossbow", "Crossbow"), ("firearm", "Firearm"), ("bow", "Bow"),

            ("shield", "Shield"), ("claw", "Claw"), ("fist", "Fist"), ("casting", "Casting"),

            ("tease", "Tease"), ("terrestrialfire", "Terrestrial Fire")
        ];

        // 原版 Weapon.hx masteryLevel()：匹配 masteries 的平均等级，未拥有返回 -1
        public int MasteryLevel(Player player)
        {
            var matched = MasteryTags.Where(tag =>

                Tags.Contains(tag.Match, System.StringComparer.OrdinalIgnoreCase)).ToList();

            if (matched.Count == 0)
                return 0;

            var levels = 0;

            foreach (var tag in matched)
                levels += player.MasteryLevel(new Identifier("coc", tag.Id));

            return levels / matched.Count;
        }

        public bool IsDual() => Matches("dual");

        public bool IsTwoHanded() => Matches("two_handed");

        public bool Is2HBlunt() => HasTag("2H Blunt");

        public bool Is2HSword() => HasTag("2H Sword");

        public bool IsRanged() => Matches("ranged");

        public bool IsSharp() => Matches("sharp");

        public bool IsBladed() => Matches("bladed");

        public bool IsStabby() => Matches("stabby");

        public bool IsBlunt() => Matches("blunt");

        public bool IsOneHandedMelee() => Matches("one_handed_melee");

        public bool IsChanneling(Player player) => IsMagicStaff() && player.HasPerk(PerkIds.StaffChanneling);

        public Combat.CombatRange GetAttackRange(Player player) =>

            IsChanneling(player) || IsRanged() ? Combat.CombatRange.Ranged : Combat.CombatRange.Melee;

        public float AttackFor(Player player) =>

            Behavior is IWeaponAttackBehavior behavior ? behavior.GetAttack(this, player) : Attack;

        public override Weapon PlayerEquip() => this;

        public override Weapon PlayerRemove() => this;

        public override void Configure(ItemDefinition definition)
        {
            base.Configure(definition);

            Attack = definition.Attack;

            Accuracy = definition.Accuracy;

            Perk = definition.Perk;

            Verb = definition.Verb;

            TwoHanded = definition.TwoHanded;

            MaxStackSize = 1;
        }
    }
}