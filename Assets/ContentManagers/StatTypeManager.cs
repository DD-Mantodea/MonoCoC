using Hjson;
using MonoCoC.Contents.Stats;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class StatTypeManager : ContentManager<StatType>
    {
        public override string Folder => "Data/Stats";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, StatType> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = Identifier.Parse((string)root["id"]);

            var definition = new StatType
            {
                Id = id,

                Default = (float)root["default"],

                HigherIsBetter = (bool)root["higherIsBetter"],

                Format = ParseFormat((string)root["format"]),

                Visible = (bool)root["visible"]
            };

            entries[id.ToString()] = definition;
        }

        public bool TryGet(Stat stat, out StatType definition) => Registry.TryGet(stat.Id, out definition);

        private static StatFormat ParseFormat(string format) => format.ToLowerInvariant() switch
        {
            "int" => StatFormat.Int,

            "decimal" => StatFormat.Decimal,

            "percent" => StatFormat.Percent,

            _ => throw new FormatException($"Unknown stat format '{format}'")
        };
    }
}