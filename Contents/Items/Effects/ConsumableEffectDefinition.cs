using Hjson;
using MonoCoC.Registry;
using System.Globalization;

namespace MonoCoC.Contents.Items.Effects
{
    public class ConsumableEffectDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Type = "";

        public JsonObject Params = [];

        public IConsumableEffect? Effect;

        public string GetString(string key, string fallback = "")
        {
            if (!Params.TryGetValue(key, out var value) || value == null)
                return fallback;

            return value.ToString().Trim('"');
        }

        public bool GetBool(string key, bool fallback = false) =>

            bool.TryParse(GetString(key), out var value) ? value : fallback;

        public int GetInt(string key, int fallback = 0) =>

            int.TryParse(GetString(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : fallback;

        public float GetFloat(string key, float fallback = 0) =>

            float.TryParse(GetString(key), NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;
    }
}