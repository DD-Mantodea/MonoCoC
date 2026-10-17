using Hjson;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World.Cabin;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Assets.ContentManagers
{

internal static class CabinParser
{
    public static List<ICabinRequirement> ParseRequirements(JsonObject json, string path)
    {
        var result = new List<ICabinRequirement>();

        if (!json.TryGetValue("requires", out var value) || value is not JsonArray requires)
            return result;

        foreach (var entry in requires)
        {
            if (entry is not JsonObject requirement)
                continue;

            var type = requirement.Str("type").ToLowerInvariant();

            switch (type)
            {
                case "keyitem":
                    result.Add(new CabinKeyItemRequirement(ParseId(requirement, "item", path)));

                    break;

                case "flag":
                    result.Add(new CabinFlagRequirement(new Flag<bool>(ParseId(requirement, "flag", path))));

                    break;

                case "resource":
                    result.Add(new CabinResourceRequirement(
                        ResolveResource(ParseId(requirement, "resource", path), path),

                        requirement.Int("amount")));

                    break;

                case "stage":
                    result.Add(new CabinStageRequirement(ParseId(requirement, "stage", path)));

                    break;

                default:
                    throw new FormatException($"Unknown cabin requirement type '{type}' in '{path}'");
            }
        }

        return result;
    }

    public static List<CabinCost> ParseCosts(JsonObject json, string path)
    {
        var result = new List<CabinCost>();

        if (!json.TryGetValue("costs", out var value) || value is not JsonArray costs)
            return result;

        foreach (var entry in costs)
        {
            if (entry is not JsonObject cost)
                continue;

            result.Add(new CabinCost(
                ResolveResource(ParseId(cost, "resource", path), path),

                cost.Int("amount")));
        }

        return result;
    }

    private static CabinResource ResolveResource(Identifier id, string path)
    {
        if (CabinResourceRegistry.Resources.TryGet(id, out var resource))
            return resource;

        throw new FormatException($"Unknown cabin resource '{id}' in '{path}'");
    }

    private static Identifier ParseId(JsonObject json, string key, string path)
    {
        var id = json.Str(key);

        return Identifier.TryParse(id, out var identifier)

            ? identifier

            : throw new FormatException($"Invalid cabin identifier '{id}' for '{key}' in '{path}'");
    }
}
}