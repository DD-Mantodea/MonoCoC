using Hjson;
using MonoCoC.Assets;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Configuration
{
    public static class OptionCatalog
    {
        public static List<OptionPaneDefinition> Load()
        {
            var panes = new List<OptionPaneDefinition>();

            var files = Paths.GetAssets("Data/Options", "*.hjson");

            Array.Sort(files, StringComparer.OrdinalIgnoreCase);

            foreach (var file in files)
                try
                {
                    var root = (JsonObject)HjsonValue.Parse(File.ReadAllText(file));

                    var pane = ParsePane(root);

                    if (pane != null)
                        panes.Add(pane);
                }
                catch
                {
                    // Ignore broken option files so a bad mod cannot break startup.
                }

            return panes;
        }

        private static OptionPaneDefinition? ParsePane(JsonObject root)
        {
            var pane = new OptionPaneDefinition
            {
                Id = Str(root, "pane"),

                TitleKey = Str(root, "title"),

                DescriptionKey = Str(root, "description"),

                ButtonKey = Str(root, "button"),

                Global = Bool(root, "global")
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
                        Id = Str(entry, "id"),

                        LabelKey = Str(entry, "label"),

                        DescriptionKey = Str(entry, "desc"),

                        Action = Str(entry, "action"),

                        Kind = Str(entry, "kind"),

                        Apply = Str(entry, "apply"),

                        Source = Str(entry, "source"),

                        OverridesLabel = Bool(entry, "overridesLabel"),

                        DefaultValue = Int(entry, "default")
                    };

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

                    if (setting.Id != "")
                        pane.Settings.Add(setting);
                }

            return pane;
        }

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