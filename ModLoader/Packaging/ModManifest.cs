using Hjson;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.ModLoader
{
    public sealed class ModManifest
    {
        public required string Id { get; init; }

        public string Name { get; init; } = "";

        public Version Version { get; init; } = new(1, 0);

        public int ApiVersion { get; init; } = 1;

        public required string Entry { get; init; }

        public required string Assembly { get; init; }

        public List<ModDependency> Dependencies { get; init; } = [];

        public List<string> LoadAfter { get; init; } = [];

        public List<string> LoadBefore { get; init; } = [];

        public string Source { get; init; } = "";

        public string DisplayName => string.IsNullOrWhiteSpace(Name) ? Id : Name;

        public static ModManifest Parse(string text, string source)
        {
            if (HjsonValue.Parse(text) is not JsonObject root)
                throw new InvalidDataException($"{source}: mod.hjson must be an object");

            var id = root.Str("id").Trim();

            if (id.Length == 0)
                throw new InvalidDataException($"{source}: missing mod id");

            if (id.Contains('/') || id.Contains('\\'))
                throw new InvalidDataException($"{source}: invalid mod id '{id}'");

            var versionText = root.Str("version", "1.0.0");

            if (!Version.TryParse(versionText, out var version))
                throw new InvalidDataException($"{source}: invalid version '{versionText}'");

            var assembly = root.Str("assembly", id + ".dll");

            return new ModManifest
            {
                Id = id,

                Name = root.Str("name"),

                Version = version,

                ApiVersion = root.Int("apiVersion", 1),

                Entry = root.Str("entry"),

                Assembly = assembly,

                Dependencies = ReadDependencies(root, source),

                LoadAfter = ReadStringArray(root, "loadAfter"),

                LoadBefore = ReadStringArray(root, "loadBefore"),

                Source = source
            };
        }

        private static List<ModDependency> ReadDependencies(JsonObject root, string source)
        {
            var result = new List<ModDependency>();

            if (!root.TryGetValue("dependencies", out var value) || value is not JsonArray array)
                return result;

            foreach (var item in array)
            {
                if (item is not JsonObject json)
                    throw new InvalidDataException($"{source}: dependency entry must be an object");

                var dependencyId = json.Str("id").Trim();

                if (dependencyId.Length == 0)
                    throw new InvalidDataException($"{source}: dependency is missing id");

                var rangeText = json.Str("version", "*");

                if (!ModVersionRange.TryParse(rangeText, out var range))
                    throw new InvalidDataException($"{source}: invalid dependency version range '{rangeText}'");

                result.Add(new ModDependency
                {
                    Id = dependencyId,

                    Range = range,

                    Weak = json.Bool("weak")
                });
            }

            return result;
        }

        private static List<string> ReadStringArray(JsonObject root, string key)
        {
            var result = new List<string>();

            if (!root.TryGetValue(key, out var value) || value is not JsonArray array)
                return result;

            foreach (var item in array)
                if (item is not null)
                {
                    var text = item.ToString().Trim();

                    if (text.Length > 0)
                        result.Add(text);
                }

            return result;
        }
    }
}
