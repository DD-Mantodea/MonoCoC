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
public class CabinResourceManager : ContentManager<CabinResource>
{
    public override string Folder => "Data/Cabin/Resources";

    public override string Pattern => "*.hjson";

    public override void FromText(string path, string text, IDictionary<string, CabinResource> entries)
    {
        var root = (JsonObject)HjsonValue.Parse(text);

        foreach (var json in Entries(root, "resources", path))
        {
            var resource = new CabinResource
            {
                Id = ParseId(json, path),

                NameKey = json.Str("nameKey"),

                Max = json.Int("max")
            };

            if (json.TryId("flag", out var flag))
                resource.Flag = new Flag<int>(flag);

            if (json.TryId("keyItem", out var keyItem))
                resource.KeyItemId = keyItem;

            resource.KeyItemValue = json.Int("value");

            entries[resource.Id.ToString()] = resource;
        }
    }

    private static IEnumerable<JsonObject> Entries(JsonObject root, string key, string path)
    {
        if (root.TryGetValue(key, out var value) && value is JsonArray array)
        {
            foreach (var entry in array)
                if (entry is JsonObject json)
                    yield return json;

            yield break;
        }

        yield return root;
    }

    private static Identifier ParseId(JsonObject json, string path)
    {
        var id = json.Str("id");

        return Identifier.TryParse(id, out var identifier)

            ? identifier

            : throw new FormatException($"Invalid cabin resource id '{id}' in '{path}'");
    }
}
}