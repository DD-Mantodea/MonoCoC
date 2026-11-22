using Hjson;
using System;
using System.Collections.Generic;

namespace MonoCoC.Saving
{
    /// <summary>
    /// 模组自定义存档数据接口。
    /// 模组在加载时注册 id + 捕获/读取回调，存档时自动写入，读档时自动回填。
    /// </summary>
    public static class ModSaveData
    {
        private sealed record Entry(Func<JsonObject?> Capture, Action<JsonObject> Load);

        private static readonly Dictionary<string, Entry> Entries = new(StringComparer.OrdinalIgnoreCase);

        public static void Register(string id, Func<JsonObject?> capture, Action<JsonObject> load)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(id);

            Entries[id] = new Entry(capture, load);
        }

        public static bool Unregister(string id) => Entries.Remove(id);

        internal static JsonObject Capture()
        {
            var result = new JsonObject();

            foreach (var (id, entry) in Entries)
            {
                try
                {
                    var data = entry.Capture();

                    if (data is not null)
                        result[id] = data;
                }
                catch (Exception exception)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[ModSaveData] capture '{id}' failed: {exception.Message}");
                }
            }

            return result;
        }

        internal static void Load(JsonObject data)
        {
            if (data is null)
                return;

            foreach (var (id, entry) in Entries)
            {
                if (!data.TryGetValue(id, out var value) || value is not JsonObject modData)
                    continue;

                try
                {
                    entry.Load(modData);
                }
                catch (Exception exception)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[ModSaveData] load '{id}' failed: {exception.Message}");
                }
            }
        }
    }
}
