using Hjson;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Cabin;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{

public class CabinFurnitureManager : ContentManager<CabinFurniture>
{
    public override string Folder => "Data/Cabin/Furniture";

    public override string Pattern => "*.hjson";

    public override void FromText(string path, string text, IDictionary<string, CabinFurniture> entries)
    {
        var root = (JsonObject)HjsonValue.Parse(text);

        if (!root.TryGetValue("furniture", out var value) || value is not JsonArray furniture)
            throw new FormatException($"Cabin furniture file '{path}' has no 'furniture' array");

        foreach (var entry in furniture)
        {
            if (entry is not JsonObject json)
                continue;

            var id = json.Str("id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid cabin furniture id '{id}' in '{path}'");

            var definition = new CabinFurniture
            {
                Id = identifier,

                NameKey = json.Str("nameKey"),

                DescriptionKey = json.Str("descriptionKey"),

                Requirements = CabinParser.ParseRequirements(json, path),

                Costs = CabinParser.ParseCosts(json, path)
            };

            if (json.TryId("flag", out var flag))
                definition.BuiltFlag = new Flag<int>(flag);

            if (json.TryId("secondFlag", out var secondFlag))
                definition.SecondFlag = new Flag<int>(secondFlag);

            entries[identifier.ToString()] = definition;
        }
    }
}
}