using Hjson;
using MonoCoC.Contents.Races;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class RaceColorManager : ContentManager<RaceColorPalette>
    {
        public override string Folder => "Data/RaceColors";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, RaceColorPalette> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (!root.TryGetValue("palettes", out var value) || value is not JsonArray palettes)
                throw new FormatException($"Race color file '{path}' has no 'palettes' array");

            foreach (var entry in palettes)
            {
                if (entry is not JsonObject json)
                    continue;

                var id = json.Str("id");

                if (!Identifier.TryParse(id, out var identifier))
                    throw new FormatException($"Invalid race color palette id '{id}' in '{path}'");

                var palette = new RaceColorPalette { Id = identifier };

                if (json.TryGetValue("colors", out var colorsValue) && colorsValue is JsonArray colors)
                    foreach (var color in colors)
                    {
                        var colorText = color?.ToString().Trim('"') ?? "";

                        if (colorText != "")
                            palette.Colors.Add(colorText);
                    }

                entries[identifier.ToString()] = palette;
            }
        }
    }
}