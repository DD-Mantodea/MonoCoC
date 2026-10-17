using Hjson;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class AreaManager : ContentManager<AreaDefinition>
    {
        public override string Folder => "Data/Areas";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, AreaDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = Str(root, "id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid area id '{id}' in '{path}'");

            var area = new AreaDefinition
            {
                Id = identifier,

                SceneId = Identifier.TryParse(Str(root, "scene"), out var scene) ? scene : identifier,

                NameKey = Str(root, "nameKey"),

                DescriptionKey = Str(root, "descriptionKey"),

                DiscoveryKey = Str(root, "discoveryKey"),

                ExploreKey = Str(root, "exploreKey"),

                Condition = ParseCondition(root, path)
            };

            var order = Str(root, "order");

            if (int.TryParse(order, out var orderValue))
                area.Order = orderValue;

            var explorationFlag = Str(root, "explorationFlag");

            if (explorationFlag != "")
                area.ExplorationFlag = new Flag<int>(Identifier.Parse(explorationFlag));

            entries[identifier.ToString()] = area;
        }

        private static AreaCondition ParseCondition(JsonObject root, string path)
        {
            if (!root.TryGetValue("condition", out var value) || value is not JsonObject condition)
                return AreaCondition.Always();

            var type = Str(condition, "type", "always");

            return type.ToLowerInvariant() switch
            {
                "always" => AreaCondition.Always(),

                "flag" => AreaCondition.Flag(new Flag<bool>(Identifier.Parse(Str(condition, "flag")))),

                "compare" => AreaCondition.Compare(
                                        new Flag<int>(Identifier.Parse(Str(condition, "flag"))),

                                        ParseComparison(Str(condition, "op", ">=")),

                                        Int(condition, "value")),

                "code" => AreaCondition.Code(Identifier.Parse(Str(condition, "unlockFlag"))),

                _ => throw new FormatException($"Unknown area condition '{type}' in '{path}'"),
            };
        }

        private static AreaComparison ParseComparison(string value) => value switch
        {
            "==" => AreaComparison.Equal,

            "!=" => AreaComparison.NotEqual,

            ">" => AreaComparison.Greater,

            ">=" => AreaComparison.GreaterOrEqual,

            "<" => AreaComparison.Less,

            "<=" => AreaComparison.LessOrEqual,

            _ => throw new FormatException($"Unknown comparison '{value}'")
        };

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            var text = value.ToString();

            if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
                text = text[1..^1];

            return text;
        }

        private static int Int(JsonObject root, string key, int fallback = 0) =>

            int.TryParse(Str(root, key), out var value) ? value : fallback;
    }
}