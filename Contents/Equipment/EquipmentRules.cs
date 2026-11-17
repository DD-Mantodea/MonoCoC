using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items;
using System.Collections.Generic;

namespace MonoCoC.Contents.Equipment
{
    public static class EquipmentRules
    {
        public static Dictionary<string, int> Capacity(Player player)
        {
            var result = new Dictionary<string, int>();

            foreach (var slot in EquipmentCatalog.Compose(player.EquipmentTags))
                foreach (var pair in slot.Capacity)
                {
                    result.TryGetValue(pair.Key, out var current);
                    result[pair.Key] = current + pair.Value;
                }

            return result;
        }

        public static Dictionary<string, int> Used(Player player)
        {
            var result = new Dictionary<string, int>();

            AddIfEquipped(result, player, "armor");
            AddIfEquipped(result, player, "upper");
            AddIfEquipped(result, player, "lower");
            AddIfEquipped(result, player, "jewelry");

            // 手牌是 extra 槽（weapon/shield 都在里面），统一在这里统计，避免重复计数。
            foreach (var item in player.ExtraEquipment.Values)
                Add(result, Uses(item));

            return result;
        }

        public static bool CanEquip(Player player, Item item, Item? replacing, out string reasonKey)
        {
            reasonKey = "";

            var capacity = Capacity(player);
            var used = Used(player);

            if (replacing is not null)
                foreach (var pair in Uses(replacing))
                {
                    used.TryGetValue(pair.Key, out var current);
                    used[pair.Key] = System.Math.Max(0, current - pair.Value);
                }

            foreach (var pair in Uses(item))
            {
                capacity.TryGetValue(pair.Key, out var max);
                used.TryGetValue(pair.Key, out var current);

                if (current + pair.Value <= max)
                    continue;

                reasonKey = "Scenes.Inventory.NotEnoughEquipmentCapacity";

                return false;
            }

            return true;
        }

        public static Dictionary<string, int> Uses(Item item)
        {
            if (item.Definition is { Uses.Count: > 0 } definition)
                return new Dictionary<string, int>(definition.Uses);

            return item switch
            {
                Weapon weapon => new() { ["hand"] = weapon.IsTwoHanded() ? 2 : 1 },
                Shield => new() { ["hand"] = 1 },
                Armor => new() { ["armor"] = 1 },
                Undergarment undergarment when undergarment.Type == Undergarment.TypeUpperwear => new() { ["upper"] = 1 },
                Undergarment => new() { ["lower"] = 1 },
                Jewelry => new() { ["jewelry"] = 1 },
                _ => []
            };
        }

        private static void AddIfEquipped(Dictionary<string, int> target, Player player, string kind)
        {
            var item = player.GetEquipped(kind);

            if (item is not null)
                Add(target, Uses(item));
        }

        private static void Add(Dictionary<string, int> target, Dictionary<string, int> source)
        {
            foreach (var pair in source)
            {
                target.TryGetValue(pair.Key, out var current);
                target[pair.Key] = current + pair.Value;
            }
        }
    }
}
