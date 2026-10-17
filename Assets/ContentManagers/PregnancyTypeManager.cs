using Hjson;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoCoC.Assets.ContentManagers
{
    // 妊娠 father 数据：支持两种布局
    //   1. Assets/Data/Pregnancy/PregnancyTypes.hjson —— { types: [ ... ] }（旧聚合，兼容保留）
    //   2. Assets/Data/Pregnancy/<Father>.hjson —— 单对象（新布局，一 father 一文件）
    // 同名 id：聚合先加载，单文件后加载覆盖（对应 Haxe register 可覆盖语义）。
    public class PregnancyTypeManager : ContentManager<PregnancyType>
    {
        public override string Folder => "Data/Pregnancy";

        public override string Pattern => "*.hjson";

        public override void Load()
        {
            var files = Paths.GetAssets(Folder, Pattern)
                .OrderBy(path => Path.GetFileName(path).Equals("PregnancyTypes.hjson", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                .ThenBy(path => path, StringComparer.OrdinalIgnoreCase);

            foreach (var file in files)
            {
                var entries = new Dictionary<string, PregnancyType>();

                FromText(file, File.ReadAllText(file), entries);

                foreach (var pair in entries)
                    Store(pair.Key, pair.Value);
            }
        }

        public override void FromText(string path, string text, IDictionary<string, PregnancyType> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (root.TryGetValue("types", out var value) && value is JsonArray array)
            {
                foreach (var item in array)
                {
                    if (item is not JsonObject entry)
                        continue;

                    var type = ParseType(entry);

                    if (type != null)
                        entries[type.Id.ToString()] = type;
                }

                return;
            }

            // 单对象文件
            if (root.ContainsKey("id"))
            {
                var single = ParseType(root);

                if (single != null)
                    entries[single.Id.ToString()] = single;
            }
        }

        private static PregnancyType? ParseType(JsonObject entry)
        {
            var id = Str(entry, "id");

            if (!Identifier.TryParse(id, out var identifier))
                return null;

            var type = new PregnancyType
            {
                Id = identifier,

                Kind = Str(entry, "kind", "vaginal"),

                Special = Str(entry, "special"),

                FullTermKey = Str(entry, "fullTerm"),

                Incubation = Int(entry, "incubation"),

                LegacyId = Int(entry, "legacyId")
            };

            if (entry.TryGetValue("events", out var eventsValue) && eventsValue is JsonArray events)
                foreach (var item in events)
                    if (int.TryParse(item.ToString().Trim('"'), out var threshold))
                        type.Events.Add(threshold);

            if (entry.TryGetValue("deliveries", out var deliveriesValue) && deliveriesValue is JsonObject deliveries)
                foreach (var pair in deliveries)
                {
                    if (pair.Value == null)
                        continue;

                    var deliveryId = pair.Value.ToString().Trim('"');

                    if (Identifier.TryParse(deliveryId, out var parsedDelivery))
                        type.Deliveries[pair.Key] = parsedDelivery;
                }

            if (entry.TryGetValue("stages", out var stagesValue) && stagesValue is JsonArray array)
                foreach (var item in array)
                {
                    if (item is not JsonObject stageEntry)
                        continue;

                    type.Stages.Add(new PregnancyStage
                    {
                        Min = Int(stageEntry, "min"),

                        Max = Int(stageEntry, "max"),

                        TextKey = Str(stageEntry, "text")
                    });
                }

            return type;
        }

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            return value.ToString().Trim('"');
        }

        private static int Int(JsonObject root, string key, int fallback = 0) =>

            int.TryParse(Str(root, key), out var value) ? value : fallback;
    }
}
