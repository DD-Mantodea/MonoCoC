using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{

    public class ItemSave
    {
        public readonly Dictionary<string, double> values = [];

        public int GetInt(string key, int fallback = 0) =>

            values.TryGetValue(key, out var value) ? (int)value : fallback;

        public bool GetBool(string key, bool fallback = false) =>

            values.TryGetValue(key, out var value) ? value != 0 : fallback;

        public void SetInt(string key, int value) => values[key] = value;

        public void SetBool(string key, bool value) => values[key] = value ? 1 : 0;
    }
}