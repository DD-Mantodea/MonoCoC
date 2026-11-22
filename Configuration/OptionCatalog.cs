using Hjson;
using MonoCoC.Assets;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Configuration
{
    public static class OptionCatalog
    {
        public static List<OptionPaneDefinition> Load(string optionNamespace = "coc")
        {
            var panes = new List<OptionPaneDefinition>();

            var files = Paths.GetAssets("Data/Options", "*.hjson");

            Array.Sort(files, StringComparer.OrdinalIgnoreCase);

            for (var fileIndex = 0; fileIndex < files.Length; fileIndex++)
                try
                {
                    var root = (JsonObject)MonoCoC.HjsonParser.Parse(File.ReadAllText(files[fileIndex]));

                    var pane = ParsePane(root, optionNamespace, fileIndex * 10);

                    if (pane != null)
                        panes.Add(pane);
                }
                catch
                {
                }

            panes.Sort((left, right) => left.Order.CompareTo(right.Order));

            return panes;
        }

        private static OptionPaneDefinition? ParsePane(JsonObject root, string optionNamespace, int defaultOrder)
        {
            var pane = new OptionPaneDefinition
            {
                Id = Str(root, "pane"),

                TitleKey = Str(root, "title"),

                DescriptionKey = Str(root, "description"),

                ButtonKey = Str(root, "button"),

                Global = Bool(root, "global"),

                Kind = Str(root, "kind"),

                Order = root.TryGetValue("order", out var orderValue) ? Int(root, "order") : defaultOrder
            };

            if (pane.Id == "")
                return null;

            if (root.TryGetValue("settings", out var list) && list is JsonArray array)
                foreach (var item in array)
                {
                    if (item is not JsonObject entry)
                        continue;

                    var setting = new OptionDefinition
                    {
                        Id = ParseOptionId(Str(entry, "id"), optionNamespace),

                        LabelKey = Str(entry, "label"),

                        DescriptionKey = Str(entry, "desc"),

                        Action = Str(entry, "action"),

                        Kind = Str(entry, "kind"),

                        Apply = Str(entry, "apply"),

                        Source = Str(entry, "source"),

                        Control = Str(entry, "control"),

                        OverridesLabel = Bool(entry, "overridesLabel"),

                        DefaultValue = Int(entry, "default")
                    };

                    if (entry.TryGetValue("sub", out var subValue) && subValue is JsonObject subPage)
                        setting.SubPage = subPage;

                    if (entry.TryGetValue("values", out var values) && values is JsonArray valueArray)
                        foreach (var valueItem in valueArray)
                        {
                            if (valueItem is not JsonObject valueEntry)
                                continue;

                            setting.Values.Add(new OptionValueDefinition
                            {
                                Value = Int(valueEntry, "value"),

                                LabelKey = Str(valueEntry, "label"),

                                DescriptionKey = Str(valueEntry, "desc"),

                                Data = Str(valueEntry, "data")
                            });
                        }

                    if (!string.IsNullOrEmpty(setting.Id.Path))
                        pane.Settings.Add(setting);
                }

            return pane;
        }

        private static Identifier ParseOptionId(string id, string optionNamespace) =>
            Identifier.TryParse(id, out var identifier)
                ? identifier
                : new Identifier(optionNamespace, id);

        private static string Str(JsonObject root, string key)
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return "";

            var text = value.ToString();

            if (text.Length > 1 && text[0] == '\"' && text[^1] == '\"')
                text = text[1..^1];

            return text;
        }

        private static bool Bool(JsonObject root, string key) =>

            Str(root, key).Equals("true", StringComparison.OrdinalIgnoreCase);

        private static int Int(JsonObject root, string key) =>

            int.TryParse(Str(root, key), out var result) ? result : 0;
    }
}
