using Hjson;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Storage;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class StorageManager : ContentManager<StorageDefinition>
    {
        public override string Folder => "Data/Storage";

        public override string Pattern => "*.hjson";

        /// <summary>物品栏槽位默认数量（来自 Assets/Data/Storage/Storage.hjson 的 player 段）。</summary>
        public static int DefaultItemStackSlots = 10;

        /// <summary>初始解锁槽位数（同上）。</summary>
        public static int DefaultUnlockedItemStacks = 3;

        public override void FromText(string path, string text, IDictionary<string, StorageDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (root.TryGetValue("player", out var playerValue) && playerValue is JsonObject playerJson)
            {
                DefaultItemStackSlots = Int(playerJson, "itemStackSlots", DefaultItemStackSlots);

                DefaultUnlockedItemStacks = Int(playerJson, "unlockedItemStacks", DefaultUnlockedItemStacks);
            }

            if (!root.TryGetValue("areas", out var value) || value is not JsonArray array)
                return;

            foreach (var item in array)
            {
                if (item is not JsonObject entry)
                    continue;

                var id = Str(entry, "id");

                if (!Identifier.TryParse(id, out var identifier))
                    continue;

                var definition = new StorageDefinition(
                    identifier,

                    Str(entry, "nameKey"),

                    Int(entry, "slots", 9),

                    ParseAccepts(Str(entry, "accepts", "all")),

                    ParseAvailability(entry),

                    Bool(entry, "fallback"),

                    Int(entry, "order"));

                entries[identifier.ToString()] = definition;
            }
        }

        private static Func<Item, bool> ParseAccepts(string value) => value.ToLowerInvariant() switch
        {
            "weapon" => item => item is Weapon,

            "armor" => item => item is Armor,

            "shield" => item => item is Shield,

            "undergarment" => item => item is Undergarment,

            "jewelry" => item => item is Jewelry,

            _ => _ => true
        };

        private static Func<Player, bool> ParseAvailability(JsonObject entry)
        {
            var keyItem = Str(entry, "keyItem");

            var flag = Str(entry, "flag");

            return player =>
            {
                if (keyItem != "" && !player.HasKeyItem(Identifier.Parse(keyItem)))
                    return false;

                if (flag != "" && CoC.Instance.Flags.Get(new Flag<int>(Identifier.Parse(flag))) <= 0)
                    return false;

                return true;
            };
        }

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            return value.ToString().Trim('"');
        }

        private static int Int(JsonObject root, string key, int fallback = 0) =>

            int.TryParse(Str(root, key), out var value) ? value : fallback;

        private static bool Bool(JsonObject root, string key, bool fallback = false) =>

            bool.TryParse(Str(root, key), out var value) ? value : fallback;
    }
}