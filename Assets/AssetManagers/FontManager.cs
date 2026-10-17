using FontStashSharp;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.AssetManagers
{
    public class FontManager : AssetManager<FontSystem>
    {
        private readonly Dictionary<string, string> _paths = [];

        private readonly Dictionary<string, FontSystem> _composites = [];

        public override string Folder => "Fonts";

        public override string Pattern => "*.ttf";

        public override void FromFile(string path, IDictionary<string, FontSystem> entries)
        {
            var key = Path.GetRelativePath(Paths.AssetPath(Folder), path.Replace(".ttf", ""));

            _paths[key] = path;

            var font = CreateSystem([path]);

            entries[key] = font;
        }

        public SpriteFontBase this[string index, int size] => this[index]?.GetFont(size);

        public FontSystem? GetSystem(string family, IReadOnlyList<string> fallbacks)
        {
            if (fallbacks.Count == 0)
                return this[family];

            var key = family + "|" + string.Join("|", fallbacks);

            if (_composites.TryGetValue(key, out var cached))
                return cached;

            var paths = new List<string>();

            if (!_paths.TryGetValue(family, out var primary))
                return null;

            paths.Add(primary);

            foreach (var fallback in fallbacks)
                if (_paths.TryGetValue(fallback, out var path) && !paths.Contains(path))
                    paths.Add(path);

            var system = CreateSystem(paths);

            _composites[key] = system;

            return system;
        }

        private static FontSystem CreateSystem(IEnumerable<string> paths)
        {
            var font = new FontSystem();

            foreach (var path in paths)
                font.AddFont(File.ReadAllBytes(path));

            return font;
        }
    }
}