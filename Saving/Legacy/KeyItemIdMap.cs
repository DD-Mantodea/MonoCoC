using MonoCoC.Contents.Items;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Saving.Legacy
{
    public static class KeyItemIdMap
    {
        private static readonly Dictionary<string, Identifier> Map = new(StringComparer.OrdinalIgnoreCase);

        static KeyItemIdMap()
        {
            Register("Backpack", KeyItemIds.Backpack);

            Register("Camp - Chest", KeyItemIds.CampChest);

            Register("Camp - Murky Chest", KeyItemIds.CampMurkyChest);

            Register("Camp - Ornate Chest", KeyItemIds.CampOrnateChest);

            Register("Carpenter's Toolbox", KeyItemIds.CarpentersToolbox);

            Register("Equipment Rack - Weapons", KeyItemIds.EquipmentRackWeapons);

            Register("Equipment Rack - Armor", KeyItemIds.EquipmentRackArmor);

            Register("Equipment Rack - Shields", KeyItemIds.EquipmentRackShields);

            Register("Equipment Storage - Jewelry Box", KeyItemIds.EquipmentStorageJewelryBox);
        }

        public static void Register(string legacyName, Identifier id) => Map[legacyName.Trim()] = id;

        public static bool Resolve(string value, out Identifier id)
        {
            if (Identifier.TryParse(value, out id))
                return true;

            var key = value.Trim();

            if (Map.TryGetValue(key, out id))
                return true;

            if (key == "")
                return false;

            id = new Identifier("coc", key.ToLowerInvariant().Replace(' ', '_'));

            return false;
        }
    }
}