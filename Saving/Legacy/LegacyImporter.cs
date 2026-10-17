using MonoCoC.Contents.Flags;
using Newtonsoft.Json.Linq;

namespace MonoCoC.Saving.Legacy
{
    public static class LegacyImporter
    {
        public static int ImportFlags(string path, GameFlags flags)
        {
            var data = SaveLoader.Load(path);

            return ImportFlags(data, flags);
        }

        public static int ImportFlags(JObject data, GameFlags flags)
        {
            if (data["flags"] is not JObject source)
                return 0;

            var imported = 0;

            foreach (var property in source.Properties())
            {
                if (!LegacyFlagMap.ByName.TryGetValue(property.Name, out var id))
                    continue;

                flags.SetRaw(id, Convert(property.Value));

                imported++;
            }

            return imported;
        }

        private static object Convert(JToken token) => token.Type switch
        {
            JTokenType.Integer => (double)token,

            JTokenType.Float => (double)token,

            JTokenType.Boolean => (bool)token,

            JTokenType.String => (string)token,

            _ => null
        };
    }
}