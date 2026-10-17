using Hjson;
using MonoCoC.Contents.Items.Behaviors;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class ItemBehaviorManager : ContentManager<ItemBehaviorDefinition>
    {
        public override string Folder => "Data/Items/Behaviors";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, ItemBehaviorDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (!root.TryGetValue("behaviors", out var value) || value is not JsonArray behaviors)
                throw new FormatException($"Item behavior file '{path}' has no 'behaviors' array");

            foreach (var entry in behaviors)
            {
                if (entry is not JsonObject json)
                    continue;

                var id = json.Str("id");

                if (!Identifier.TryParse(id, out var identifier))
                    throw new FormatException($"Invalid item behavior id '{id}' in '{path}'");

                var definition = new ItemBehaviorDefinition
                {
                    Id = identifier,

                    Type = json.Str("type")
                };

                if (definition.Type == "")
                    throw new FormatException($"Item behavior '{identifier}' has no type in '{path}'");

                try
                {
                    definition.Behavior = ItemBehaviorRegistry.Create(definition);
                }
                catch (InvalidOperationException)
                {
                    // 外部数据缺失：该 behavior type 在 C# 侧未注册，跳过该项
                    continue;
                }

                entries[identifier.ToString()] = definition;
            }
        }
    }
}