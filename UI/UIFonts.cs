using FontStashSharp;
using Hjson;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.UI
{
    public static class UIFonts
    {
        private class Role
        {
            public string Family = "pala";

            public int Size = 20;

            public string Bold = "palab";

            public string Italic = "palai";

            public string BoldItalic = "palabi";

            public List<string> Fallback = [];
        }

        private static readonly Dictionary<string, Role> Defaults = [];

        private static readonly Dictionary<string, Role> Roles = [];

        private static readonly Dictionary<(string Family, int Size, string Fallbacks), SpriteFontBase> Cache = [];

        private static readonly Dictionary<string, List<string>> FamilyFallbacks = [];

        public static int Revision { get; private set; }

        public static void Initialize()
        {
            Defaults.Clear();

            LoadInto(Defaults, Path.Combine(Paths.AssetPath("Data/UI"), "Fonts.hjson"));

            if (Defaults.Count == 0)
                Defaults["body"] = new Role();

            Roles.Clear();

            foreach (var pair in Defaults)
                Roles[pair.Key] = Copy(pair.Value);

            BuildFamilyFallbacks();

            Revision++;
        }

        public static void ApplyLanguage(string language)
        {
            Roles.Clear();

            foreach (var pair in Defaults)
                Roles[pair.Key] = Copy(pair.Value);

            if (!string.IsNullOrEmpty(language))
            {
                var path = Path.Combine(Paths.AssetPath("Localization"), language, "Language.hjson");

                if (File.Exists(path))
                    try
                    {
                        var root = (JsonObject)HjsonValue.Parse(File.ReadAllText(path));

                        if (root.TryGetValue("fonts", out var fonts) && fonts is JsonObject map)
                            LoadInto(Roles, map, merge: true);
                    }
                    catch
                    {
                        // Broken language font config: keep defaults.
                    }
            }

            BuildFamilyFallbacks();

            Revision++;
        }

        public static SpriteFontBase Get(string role)
        {
            var info = RoleOf(role);

            return Resolve(info.Family, info.Size, info.Fallback);
        }

        public static SpriteFontBase GetAt(string family, int size) =>

            Resolve(family, size, FamilyFallbacks.TryGetValue(family, out var fallbacks) ? fallbacks : []);

        public static int Size(string role) => RoleOf(role).Size;

        public static string Family(string role) => RoleOf(role).Family;

        public static string Style(string role, string style)
        {
            var info = RoleOf(role);

            return style switch
            {
                "bold" => info.Bold,

                "italic" => info.Italic,

                "boldItalic" => info.BoldItalic,

                _ => info.Family
            };
        }

        public static void ApplyToParser(Scripting.ScriptContext context)
        {
            context.FontFamily = Family("body");

            context.BoldFont = Style("body", "bold");

            context.ItalicFont = Style("body", "italic");

            context.BoldItalicFont = Style("body", "boldItalic");

            context.FontSize = Size("body");
        }

        private static SpriteFontBase Resolve(string family, int size, IReadOnlyList<string> fallbacks)
        {
            var key = (family, size, string.Join("|", fallbacks));

            if (Cache.TryGetValue(key, out var font))
                return font;

            var system = ManagerInstance<FontManager>.Instance.GetSystem(family, fallbacks);

            font = system?.GetFont(size) ?? ManagerInstance<FontManager>.Instance[family, size];

            if (font == null)
                font = ManagerInstance<FontManager>.Instance["pala", size];

            Cache[key] = font;

            return font;
        }

        private static void BuildFamilyFallbacks()
        {
            FamilyFallbacks.Clear();

            foreach (var info in Roles.Values)
            {
                Add(info.Family, info.Fallback);

                Add(info.Bold, info.Fallback);

                Add(info.Italic, info.Fallback);

                Add(info.BoldItalic, info.Fallback);
            }
        }

        private static void Add(string family, List<string> fallbacks)
        {
            if (string.IsNullOrEmpty(family))
                return;

            FamilyFallbacks[family] = [.. fallbacks];
        }

        private static Role RoleOf(string role)
        {
            if (Roles.TryGetValue(role, out var info))
                return info;

            if (Roles.TryGetValue("body", out var body))
                return body;

            return new Role();
        }

        private static Role Copy(Role source) => new()
        {
            Family = source.Family,

            Size = source.Size,

            Bold = source.Bold,

            Italic = source.Italic,

            BoldItalic = source.BoldItalic,

            Fallback = [.. source.Fallback]
        };

        private static void LoadInto(Dictionary<string, Role> target, string path)
        {
            if (!File.Exists(path))
                return;

            try
            {
                LoadInto(target, (JsonObject)HjsonValue.Parse(File.ReadAllText(path)));
            }
            catch
            {
                // Broken font config: keep whatever is already loaded.
            }
        }

        private static void LoadInto(Dictionary<string, Role> target, JsonObject root, bool merge = false)
        {
            foreach (var pair in root)
            {
                if (pair.Value is not JsonObject entry)
                    continue;

                if (!target.TryGetValue(pair.Key, out var role) || !merge)
                    role = new Role();

                role.Family = Str(entry, "family", role.Family);

                role.Size = Int(entry, "size", role.Size);

                role.Bold = Str(entry, "bold", role.Bold);

                role.Italic = Str(entry, "italic", role.Italic);

                role.BoldItalic = Str(entry, "boldItalic", role.BoldItalic);

                if (entry.TryGetValue("fallback", out var fallback))
                    role.Fallback = ReadFallback(fallback);

                target[pair.Key] = role;
            }
        }

        private static List<string> ReadFallback(JsonValue value)
        {
            var result = new List<string>();

            if (value is JsonArray array)
            {
                foreach (var item in array)
                    Add(item?.ToString().Trim('"') ?? "");

                return result;
            }

            var text = value.ToString().Trim('"');

            foreach (var part in text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                Add(part);

            return result;

            void Add(string item)
            {
                if (!string.IsNullOrWhiteSpace(item))
                    result.Add(item);
            }
        }

        private static string Str(JsonObject root, string key, string fallback)
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            var text = value.ToString();

            if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
                text = text[1..^1];

            return text;
        }

        private static int Int(JsonObject root, string key, int fallback) =>

            int.TryParse(Str(root, key, ""), out var value) ? value : fallback;
    }
}