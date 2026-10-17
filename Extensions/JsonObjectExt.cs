using Hjson;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.Globalization;

namespace MonoCoC.Extensions
{
    public static class JsonObjectExt
    {
        extension(JsonObject json)
        {
            public string Str(string key, string fallback = "")
            {
                if (!json.TryGetValue(key, out var value) || value is null)
                    return fallback;

                var text = value.ToString();

                if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
                    text = text[1..^1];

                return text == "" ? fallback : text;
            }

            public int Int(string key, int fallback = 0) =>
                int.TryParse(json.Str(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : fallback;

            public float Float(string key, float fallback = 0) =>
            float.TryParse(json.Str(key), NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;

            public bool Bool(string key, bool fallback = false) =>
                bool.TryParse(json.Str(key), out var value) ? value : fallback;

            public bool TryId(string key, out Identifier id) =>
                Identifier.TryParse(json.Str(key), out id);

            public Identifier Id(string key, string fallback = "")
            {
                var text = json.Str(key, fallback);

                return Identifier.TryParse(text, out var id) ? id : default;
            }

            public List<string> Strings(string key)
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

            public List<Identifier> Identifiers(string key)
            {
                var result = new List<Identifier>();

                foreach (var text in Strings(json, key))
                {
                    if (Identifier.TryParse(text, out var id))
                        result.Add(id);
                }

                return result;
            }

            public JsonArray? Array(string key) =>
                json.TryGetValue(key, out var value) && value is JsonArray array ? array : null;

            public JsonObject? Object(string key) =>
                json.TryGetValue(key, out var value) && value is JsonObject nested ? nested : null;
        }
    }
}
