using FontStashSharp;
using Hjson;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.UI
{
    // 字体角色表：唯一事实来源是 Assets/Localization/<lang>/Language.hjson 的 fonts: 节点。
    // 每次切语言构建一份不可变快照并整体替换，避免原地清空/合并导致的状态污染。
    public static class UIFonts
    {
        private readonly record struct FontRole(
            string Family,
            int Size,
            string[] Fallbacks,
            string Bold,
            string Italic,
            string BoldItalic);

        private static readonly FontRole BuiltInBody = new(
            "pala",
            20,
            [],
            "palab",
            "palai",
            "palabi");

        private static Dictionary<string, FontRole> _roles = [];

        private static Dictionary<string, string[]> _familyFallbacks = [];

        private static readonly Dictionary<(string Family, int Size, string Fallbacks), SpriteFontBase> Cache = [];

        private static readonly HashSet<string> WarnedRoles = [];

        public static int Revision { get; private set; }

        // 语言管理器可能尚未就绪（Initialize 早期），此时退回 en-us。
        private static string FallbackLanguage
        {
            get
            {
                try
                {
                    var manager = ManagerInstance<LocalizationManager>.Instance;

                    if (manager is not null && !string.IsNullOrEmpty(manager.Fallback))
                        return manager.Fallback;
                }
                catch
                {
                }

                return "en-us";
            }
        }


        // 初始化：以 fallback 语言（通常是 en-us）作为基线快照。
        public static void Initialize() => Rebuild(FallbackLanguage);

        // 切换语言：以 fallback 语言打底，再用目标语言覆盖同名角色。
        public static void ApplyLanguage(string language) => Rebuild(language);

        public static SpriteFontBase Get(string role)
        {
            var info = RoleOf(role);

            return Resolve(info.Family, info.Size, info.Fallbacks);
        }

        public static SpriteFontBase GetAt(string family, int size) =>

            Resolve(family, size, _familyFallbacks.TryGetValue(family, out var fallbacks) ? fallbacks : []);

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

        private static void Rebuild(string? language)
        {
            var roles = new Dictionary<string, FontRole>(StringComparer.OrdinalIgnoreCase);

            // 语言文件是唯一来源；fallback 语言先打底，目标语言覆盖。
            // 同一个语言不重复读。
            LoadLanguage(roles, FallbackLanguage);

            if (!string.IsNullOrEmpty(language) &&
                !string.Equals(language, FallbackLanguage, StringComparison.OrdinalIgnoreCase))
                LoadLanguage(roles, language);

            if (roles.Count == 0)
                roles["body"] = BuiltInBody;

            _roles = roles;

            _familyFallbacks = BuildFamilyFallbacks(roles);

            Cache.Clear();

            WarnedRoles.Clear();

            Revision++;
        }

        private static void LoadLanguage(Dictionary<string, FontRole> target, string? language)
        {
            if (string.IsNullOrEmpty(language))
                return;

            var path = Path.Combine(LocalizationManager.RootPath, language, "Language.hjson");

            if (!File.Exists(path))
                return;

            try
            {
                if (MonoCoC.HjsonParser.Parse(File.ReadAllText(path)) is not JsonObject root ||
                    !root.TryGetValue("fonts", out var value) ||
                    value is not JsonObject fonts)
                    return;

                foreach (var pair in fonts)
                {
                    if (pair.Value is not JsonObject entry)
                        continue;

                    var key = NormalizeRole(pair.Key);

                    target[key] = ReadRole(entry, target.GetValueOrDefault(key));
                }
            }
            catch
            {
                // 语言文件损坏：保留已有快照（fallback 语言那层）。
            }
        }

        private static FontRole ReadRole(JsonObject entry, FontRole? inherit)
        {
            var fallbacks = entry.TryGetValue("fallback", out var fallbackValue)
                ? ReadFallback(fallbackValue)
                : inherit?.Fallbacks ?? [];

            return new FontRole(
                Str(entry, "family", inherit?.Family ?? BuiltInBody.Family),
                Int(entry, "size", inherit?.Size ?? BuiltInBody.Size),
                fallbacks,
                Str(entry, "bold", inherit?.Bold ?? BuiltInBody.Bold),
                Str(entry, "italic", inherit?.Italic ?? BuiltInBody.Italic),
                Str(entry, "boldItalic", inherit?.BoldItalic ?? BuiltInBody.BoldItalic));
        }

        private static Dictionary<string, string[]> BuildFamilyFallbacks(IReadOnlyDictionary<string, FontRole> roles)
        {
            var result = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

            foreach (var info in roles.Values)
            {
                Add(info.Family, info.Fallbacks);

                Add(info.Bold, info.Fallbacks);

                Add(info.Italic, info.Fallbacks);

                Add(info.BoldItalic, info.Fallbacks);
            }

            return result;

            void Add(string family, string[] fallbacks)
            {
                if (!string.IsNullOrEmpty(family))
                    result[family] = fallbacks;
            }
        }

        private static SpriteFontBase Resolve(string family, int size, IReadOnlyList<string> fallbacks)
        {
            var key = (family, size, string.Join("|", fallbacks));

            if (Cache.TryGetValue(key, out var cached) && cached is not null)
                return cached;

            var manager = ManagerInstance<FontManager>.Instance;

            var font = manager.GetSystem(family, fallbacks)?.GetFont(size) ?? manager[family, size];

            if (font is null)
            {
                WarnOnce("字体缺失，退回 pala: " + family + " " + size);

                font = manager["pala", size];
            }

            Cache[key] = font;

            return font;
        }

        private static FontRole RoleOf(string role)
        {
            var key = NormalizeRole(role);

            if (_roles.TryGetValue(key, out var info))
                return info;

            if (_roles.TryGetValue("body", out var body))
            {
                WarnOnce("字体角色未定义，退回 body: " + key);

                return body;
            }

            WarnOnce("字体角色表为空，使用内置 body: " + key);

            return BuiltInBody;
        }

        private static void WarnOnce(string message)
        {
            if (!WarnedRoles.Add(message))
                return;

            try
            {
                Console.ConsoleOverlay.WriteWarning("[UIFonts] " + message);
            }
            catch
            {
                // 控制台未就绪时忽略诊断输出。
            }
        }

        private static string NormalizeRole(string role) =>
            role.StartsWith("coc:", StringComparison.OrdinalIgnoreCase)
                ? role[4..]
                : role;

        private static string[] ReadFallback(JsonValue value)
        {
            var result = new List<string>();

            if (value is JsonArray array)
            {
                foreach (var item in array)
                    Add(item?.ToString().Trim('"') ?? "");

                return [.. result];
            }

            var text = value.ToString().Trim('"');

            foreach (var part in text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                Add(part);

            return [.. result];

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
