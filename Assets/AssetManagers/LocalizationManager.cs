using Hjson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MonoCoC.Assets.AssetManagers
{
    public class LanguageInfo
    {
        public string Code = "";

        public string Name = "";

        public int Order;

        // 缺 key 时回退到哪种语言（Language.hjson 里可写 "fallback": "en-us" 覆盖）
        public string Fallback = "en-us";
    }

    public partial class LocalizationManager : AssetManager<Dictionary<string, string>>
    {
        public string Language = "";

        // 当前语言的兜底语言：zh-cn 这种还没翻的语言，缺的 key 落回 en-us，避免界面出现裸 key
        public string Fallback = "en-us";

        public List<LanguageInfo> Languages = [];

        public override string Folder => "Localization";

        public override string Pattern => "*.hjson";

        public Dictionary<string, string> Strings => Dict.TryGetValue(Language, out var strings) ? strings : [];

        public override void FromFile(string path, IDictionary<string, Dictionary<string, string>> entries) =>

            entries[Path.GetFileNameWithoutExtension(path)] = ReadStrings(path);

        private static Dictionary<string, string> ReadStrings(string path)
        {
            var root = (JsonObject)HjsonValue.Parse(File.ReadAllText(path));

            var strings = new Dictionary<string, string>();

            foreach (var pair in root)
                strings[pair.Key] = ((string)pair.Value).Replace("\r\n", "\n");

            return strings;
        }

        public override void Load()
        {
            var root = Paths.AssetPath(Folder);

            if (!Directory.Exists(root))
                return;

            Languages.Clear();

            foreach (var directory in Directory.GetDirectories(root))
            {
                var code = Path.GetFileName(directory);

                var strings = new Dictionary<string, string>();

                var sources = new Dictionary<string, string>();

                foreach (var file in Directory.GetFiles(directory, Pattern, SearchOption.AllDirectories))
                {
                    if (Path.GetFileName(file) == "Language.hjson")
                        continue;

                    var values = ReadStrings(file);

                    foreach (var pair in values)
                    {
                        if (sources.TryGetValue(pair.Key, out var existing))
                            throw new Exception($"Duplicate localization key '{pair.Key}' in '{existing}' and '{file}'");

                        sources[pair.Key] = file;

                        strings[pair.Key] = pair.Value;
                    }
                }

                Dict[code] = strings;

                Languages.Add(ReadLanguageInfo(directory, code));
            }

            Languages = [.. Languages.OrderBy(language => language.Order).ThenBy(language => language.Name)];
        }

        public void Use(string language)
        {
            if (!Dict.ContainsKey(language))
                throw new Exception($"Missing localization: {language}");

            Language = language;

            Fallback = Languages.FirstOrDefault(candidate => candidate.Code == language)?.Fallback ?? "en-us";
        }

        // 运行时自检：loc 值里有 {n} 占位符，但调用没传够实参时，把 key 记下来。
        // 目的：不依赖任何离线脚本/人工判断，由程序自己报告哪些 key 真的会显示成 "{0}"。
        // 规则：占位符 = 单个花括号包住纯数字（{0} {1} ...）；tag 里的字面花括号在抽取时已被
        // 转义成 {{ }}（双花括号），不会匹配到这里，因此不存在 tag 误判。
        private static readonly Regex PlaceholderPattern =

            MyRegex();

        private static readonly HashSet<string> MissingArgsReported = [];

        public static int MissingArgsCount;

        public static string MissingArgsLogPath => Path.Combine(AppContext.BaseDirectory, "missing-args.log");

        private static void ReportMissingArgs(string key, string value, int given)
        {
            var matches = PlaceholderPattern.Matches(value);

            if (matches.Count == 0)
                return;

            var need = 0;

            foreach (Match m in matches)
                need = Math.Max(need, int.Parse(m.Groups[1].Value) + 1);

            // 部分传参（0 < given < need）会抛 FormatException，等于崩；0 实参只是显示 {0}。
            if (given >= need)
                return;

            MissingArgsCount++;

            if (!MissingArgsReported.Add(key))
                return;

            try
            {
                File.AppendAllText(MissingArgsLogPath,

                    $"{key}\tneed={need}\tgot={given}\tvalue={value.Replace("\r", "").Replace("\n", "\\n")}\n");
            }
            catch
            {
                // 日志写不进去不影响游戏
            }
        }

        // 查表 + 可选格式化。args 为空时只查表；给了参数就按 string.Format 套进去。
        public string Localize(string key, params object[] args)
        {
            if (Strings.TryGetValue(key, out var value))
            {
                ReportMissingArgs(key, value, args.Length);

                return args.Length == 0 ? value : string.Format(value, args);
            }

            // 当前语言没有这条 key：落回兜底语言（默认 en-us），再没有才显示 key 本身。
            if (Fallback != Language && Dict.TryGetValue(Fallback, out var fallbackStrings)
                && fallbackStrings.TryGetValue(key, out var fallbackValue))
            {
                ReportMissingArgs(key, fallbackValue, args.Length);

                return args.Length == 0 ? fallbackValue : string.Format(fallbackValue, args);
            }

            return args.Length == 0 ? key : string.Format(key, args);
        }

        private static LanguageInfo ReadLanguageInfo(string directory, string code)
        {
            var info = new LanguageInfo
            {
                Code = code,

                Name = code
            };

            var path = Path.Combine(directory, "Language.hjson");

            if (!File.Exists(path))
                return info;

            try
            {
                var root = (JsonObject)HjsonValue.Parse(File.ReadAllText(path));

                if (root.TryGetValue("code", out var codeValue) && codeValue != null)
                    info.Code = codeValue.ToString().Trim('"');

                if (root.TryGetValue("name", out var nameValue) && nameValue != null)
                    info.Name = nameValue.ToString().Trim('"');

                if (root.TryGetValue("order", out var orderValue) && orderValue != null && int.TryParse(orderValue.ToString(), out var order))
                    info.Order = order;

                if (root.TryGetValue("fallback", out var fallbackValue) && fallbackValue != null)
                {
                    var fallback = fallbackValue.ToString().Trim('"');

                    if (fallback.Length > 0)
                        info.Fallback = fallback;
                }
            }
            catch
            {
                // Broken language config: keep directory defaults.
            }

            return info;
        }

        [GeneratedRegex(@"\{(\d+)(?:[:}])", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
    }
}