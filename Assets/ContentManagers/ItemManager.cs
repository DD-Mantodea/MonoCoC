using Hjson;
using MonoCoC.Contents.Items;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class ItemManager : ContentManager<ItemDefinition>
    {
        public override string Folder => "Data/Items";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, ItemDefinition> entries)
        {
            var value = HjsonValue.Parse(text);

            var definitions = new List<ItemDefinition>();

            if (value is JsonArray array)
                ParseItems(array, definitions);
            else if (value is JsonObject root && root.TryGetValue("items", out var items) && items is JsonArray list)
                ParseItems(list, definitions);

            foreach (var definition in definitions)
            {
                entries[definition.Id.ToString()] = definition;

                // register legacy item id map
                if (definition.LegacyId != "")
                    Saving.Legacy.ItemIdMap.Register(definition.LegacyId, definition.Id);
            }
        }

        private static void ParseItems(JsonArray items, List<ItemDefinition> definitions)
        {
            foreach (var entry in items)
                if (entry is JsonObject json)
                    definitions.Add(ParseDefinition(json));
        }

        private static ItemDefinition ParseDefinition(JsonObject json) => new()
        {
            Id = Identifier.Parse(Str(json, "id")),

            Type = Str(json, "type", "default"),

            Behavior = Str(json, "behavior"),

            ShortNameKey = Str(json, "shortNameKey"),

            LongNameKey = Str(json, "longNameKey"),

            DescriptionKey = Str(json, "descriptionKey"),

            UseTextKey = Str(json, "useTextKey"),

            HeaderKey = Str(json, "headerKey"),

            Value = Float(json, "value"),

            MaxStackSize = Int(json, "maxStackSize", 5),

            InvUseOnly = Bool(json, "invUseOnly"),

            Durability = Int(json, "durability"),

            DegradesInto = Str(json, "degradesInto"),

            Tags = Strings(json, "tags"),

            Attack = Float(json, "attack"),

            Verb = Str(json, "verb"),

            Accuracy = Float(json, "accuracy", 1),

            Defense = Float(json, "defense"),

            Block = Float(json, "block"),

            Perk = Str(json, "perk"),

            TwoHanded = Bool(json, "twoHanded"),

            SupportsBulge = Bool(json, "supportsBulge"),

            SupportsUndergarment = Bool(json, "supportsUndergarment", true),

            UndergarmentSlot = Str(json, "slot"),

            Sexiness = Int(json, "sexiness"),

            ArmorDefense = Int(json, "armorDefense"),

            EffectId = Float(json, "effectId"),

            EffectMagnitude = Float(json, "effectMagnitude"),

            Effect = Str(json, "effect"),

            Effects = Identifiers(json, "effects"),

            LegacyId = Str(json, "legacyId")
        };

        private static List<Identifier> Identifiers(JsonObject json, string key)
        {
            var result = new List<Identifier>();

            if (!json.TryGetValue(key, out var value) || value is not JsonArray array)
                return result;

            foreach (var entry in array)
            {
                var text = entry?.ToString().Trim('"') ?? "";

                if (Identifier.TryParse(text, out var id))
                    result.Add(id);
            }

            return result;
        }

        private static string Str(JsonObject json, string key, string fallback = "")
        {
            if (!json.TryGetValue(key, out var value) || value == null)
                return fallback;

            return value.ToString().Trim('"');
        }

        private static int Int(JsonObject json, string key, int fallback = 0) =>

            int.TryParse(Str(json, key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : fallback;

        private static float Float(JsonObject json, string key, float fallback = 0) =>

            float.TryParse(Str(json, key), NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;

        private static bool Bool(JsonObject json, string key, bool fallback = false) =>

            bool.TryParse(Str(json, key), out var value) ? value : fallback;

        private static List<string> Strings(JsonObject json, string key)
        {
            var result = new List<string>();

            if (!json.TryGetValue(key, out var value) || value is not JsonArray array)
                return result;

            foreach (var entry in array)
            {
                var text = entry?.ToString().Trim('"') ?? "";

                if (text != "")
                    result.Add(text);
            }

            return result;
        }
    }
}