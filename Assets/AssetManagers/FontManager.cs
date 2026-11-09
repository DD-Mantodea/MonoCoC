using FontStashSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.AssetManagers
{
    public class FontManager : AssetManager<FontSystem>
    {
        private const string GlobalFallback = "JetBrainsMono-Regular";

        private static readonly string[] SystemFallbacks =
        [
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "segoeui.ttf"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "seguisym.ttf")
        ];

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
            var key = family + "|" + string.Join("|", fallbacks) + "|" + GlobalFallback;

            if (_composites.TryGetValue(key, out var cached))
                return cached;

            var paths = new List<string>();

            if (_paths.TryGetValue(family, out var primary))
                paths.Add(primary);

            foreach (var fallback in fallbacks)
                if (_paths.TryGetValue(fallback, out var path) && !paths.Contains(path))
                    paths.Add(path);

            // Keep JetBrainsMono as the final glyph fallback for every family.
            // It is intentionally last so the primary font's style wins.
            if (_paths.TryGetValue(GlobalFallback, out var global) && !paths.Contains(global))
                paths.Add(global);

            // DSH/web text resolves to Segoe UI on Windows; Segoe UI Symbol
            // supplies the remaining pictographs such as U+2726 (✦).
            foreach (var systemFont in SystemFallbacks)
                if (File.Exists(systemFont) && !paths.Contains(systemFont))
                    paths.Add(systemFont);

            if (paths.Count == 0)
                return null;

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