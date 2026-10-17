using MonoCoC.Registry;

namespace MonoCoC.Contents.Items
{
    public static class IceWeapon
    {
        public const int Axe = 0;

        public const int Mace = 1;

        public const int Spear = 2;

        public const int Sword = 3;

        public const int Dagger = 4;

        public const int Scythe = 5;

        public const int Staff = 6;

        public static readonly string[] TypeStrings = ["axe", "mace", "spear", "sword", "dagger", "scythe", "staff"];

        public static readonly string[] TypeNouns = ["swing", "smash", "thrust", "slash", "stab", "slash", "strike"];

        public static readonly string[] TypeVerbs = ["hack", "smash", "stab", "slash", "stab", "reap", "strike"];

        public static string PlayerHasIceWeapon()
        {
            var player = CoC.Instance.Player;

            if (player.Weapon.IsMelting())
                return player.Weapon.Name;

            foreach (var type in TypeStrings)
            {
                var item = ItemRegistry.Create(new Identifier("coc", "ice_" + type));

                if (player.HasItem(item))
                    return item.Name;
            }

            return "";
        }
    }
}