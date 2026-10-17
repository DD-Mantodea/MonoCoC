using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Races
{
    public class RaceColorPalette : IIdentifiable
    {
        public Identifier Id { get; set; }

        public List<string> Colors = [];

        public bool Has(string color) => Colors.Contains(color);
    }

    public static class RaceColorRegistry
    {
        public static Registry<RaceColorPalette> Palettes => ManagerInstance<RaceColorManager>.Instance.Registry;

        public static bool Has(string path, string color) =>

            Palettes.TryGet(new Identifier("coc", path), out var palette) && palette.Has(color);
    }
}