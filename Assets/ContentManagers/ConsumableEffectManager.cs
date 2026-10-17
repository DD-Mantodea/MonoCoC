using Hjson;
using MonoCoC.Contents.Items.Effects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class ConsumableEffectManager : ContentManager<ConsumableEffectDefinition>
    {
        public override string Folder => "Data/Items/Effects";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, ConsumableEffectDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (!root.TryGetValue("effects", out var value) || value is not JsonArray effects)
                throw new FormatException($"Consumable effect file '{path}' has no 'effects' array");

            foreach (var entry in effects)
            {
                if (entry is not JsonObject json)
                    continue;

                var id = json.Str("id");

                if (!Identifier.TryParse(id, out var identifier))
                    throw new FormatException($"Invalid consumable effect id '{id}' in '{path}'");

                var definition = new ConsumableEffectDefinition
                {
                    Id = identifier,

                    Type = json.Str("type"),

                    Params = json
                };

                if (definition.Type == "")
                    throw new FormatException($"Consumable effect '{identifier}' has no type in '{path}'");

                definition.Effect = ConsumableEffectRegistry.Create(definition);

                entries[identifier.ToString()] = definition;
            }
        }
    }
}