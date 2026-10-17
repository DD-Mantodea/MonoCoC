using Hjson;
using MonoCoC.Contents.World.Cabin;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{

public class CabinStageManager : ContentManager<CabinStage>
{
    public override string Folder => "Data/Cabin/Stages";

    public override string Pattern => "*.hjson";

    public override void FromText(string path, string text, IDictionary<string, CabinStage> entries)
    {
        var root = (JsonObject)HjsonValue.Parse(text);

        if (!root.TryGetValue("stages", out var value) || value is not JsonArray stages)
            throw new FormatException($"Cabin stage file '{path}' has no 'stages' array");

        foreach (var entry in stages)
        {
            if (entry is not JsonObject json)
                continue;

            var id = json.Str("id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid cabin stage id '{id}' in '{path}'");

            var stage = new CabinStage
            {
                Id = identifier,

                NameKey = json.Str("nameKey"),

                DescriptionKey = json.Str("descriptionKey"),

                Order = json.Int("order"),

                Fatigue = json.Int("fatigue"),

                Hours = json.Int("hours"),

                Requirements = CabinParser.ParseRequirements(json, path),

                Costs = CabinParser.ParseCosts(json, path)
            };

            if (json.TryId("handler", out var handler))
            {
                if (!CabinHandlerRegistry.Has(handler))
                    throw new FormatException($"Unknown cabin handler '{handler}' in '{path}'");

                stage.Handler = handler;
            }

            entries[identifier.ToString()] = stage;
        }
    }
}
}