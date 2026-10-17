using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Saving.Legacy
{
    public static class ItemIdMap
    {
        private static readonly Dictionary<string, Identifier> Map = new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, Identifier> NormalizedMap = new(StringComparer.OrdinalIgnoreCase);

        public static void Register(string legacyId, Identifier id)
        {
            var key = legacyId.Trim();

            if (key == "")
                return;

            Map[key] = id;

            var normalized = Normalize(key);

            if (normalized != "")
                NormalizedMap[normalized] = id;
        }

        public static bool Resolve(string value, out Identifier id)
        {
            var key = value.Trim();

            if (key != "")
            {
                if (Map.TryGetValue(key, out id))
                    return true;

                var normalizedKey = Normalize(key);

                if (normalizedKey != "" && NormalizedMap.TryGetValue(normalizedKey, out id))
                    return true;
            }

            return Identifier.TryParse(value, out id);
        }

        private static string Normalize(string value)
        {
            var chars = new System.Text.StringBuilder(value.Length);

            foreach (var c in value)
                if (char.IsLetterOrDigit(c))
                    chars.Append(char.ToUpperInvariant(c));

            return chars.ToString();
        }
    }
}