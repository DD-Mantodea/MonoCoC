using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Storage
{
    public static class StorageRegistry
    {
        public static readonly Identifier ItemsId = new("coc", "items");

        public static readonly Identifier GearWeaponsId = new("coc", "gear_weapons");

        public static readonly Identifier GearArmorId = new("coc", "gear_armor");

        public static readonly Identifier GearShieldsId = new("coc", "gear_shields");

        public static readonly Identifier GearUndergarmentsId = new("coc", "gear_undergarments");

        public static readonly Identifier GearJewelryId = new("coc", "gear_jewelry");

        public static Registry<StorageDefinition> Registered => ManagerInstance<StorageManager>.Instance.Registry;

        public static IReadOnlyList<StorageDefinition> Definitions =>

            [.. Registered.Ids.Select(Registered.Get).OrderBy(definition => definition.Order)];
    }
}