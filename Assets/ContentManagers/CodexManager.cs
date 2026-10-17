using Hjson;
using MonoCoC.Contents.Codex;
using MonoCoC.Contents.Flags;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class CodexManager : ContentManager<CodexEntry>
    {
        public override string Folder => "Data/Codex";

        public override string Pattern => "*.hjson";

        // GameFlagIds 里 CodexEntry* 的 Flag<int>：常量名与 id 路径都能查到
        private static readonly Dictionary<string, Flag<int>> KnownUnlockFlags = BuildKnownUnlockFlags();

        private static Dictionary<string, Flag<int>> BuildKnownUnlockFlags()
        {
            var map = new Dictionary<string, Flag<int>>(StringComparer.Ordinal);

            foreach (var field in typeof(GameFlagIds).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType != typeof(Flag<int>) || !field.Name.StartsWith("CodexEntry", StringComparison.Ordinal))
                    continue;

                var flag = (Flag<int>)field.GetValue(null)!;

                map[field.Name] = flag;

                map[flag.Id.Path] = flag;
            }

            return map;
        }

        public override void FromText(string path, string text, IDictionary<string, CodexEntry> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (!root.TryGetValue("entries", out var value) || value is not JsonArray array)
                return;

            foreach (var element in array)
            {
                if (element is not JsonObject json)
                    continue;

                if (!Identifier.TryParse(Str(json, "id"), out var id))
                    continue;

                var unlockFlag = Str(json, "unlockFlag");

                entries[id.ToString()] = new CodexEntry
                {
                    Id = id,

                    Category = Str(json, "category"),

                    NameKey = Str(json, "nameKey"),

                    TextKey = Str(json, "textKey"),

                    TextKeys = StrList(json, "textKeys"),

                    UnlockFlag = unlockFlag,

                    Unlock = ResolveUnlock(unlockFlag),

                    Order = Int(json, "order")
                };
            }
        }

        private static Flag<int>? ResolveUnlock(string flag)
        {
            if (string.IsNullOrEmpty(flag))
                return null;

            return KnownUnlockFlags.TryGetValue(flag, out var resolved) ? resolved : null;
        }

        /// <summary>
        /// 原版 Codex.hx 每个条目由多个本地化段落组成（性别 / 身高 / 皮肤……），
        /// 数据里用 textKeys 数组按渲染顺序列出。
        /// </summary>
        private static List<string> StrList(JsonObject root, string key)
        {
            if (!root.TryGetValue(key, out var value) || value is not JsonArray array)
                return [];

            var list = new List<string>();

            foreach (var item in array)
            {
                var text = item?.ToString().Trim('"') ?? "";

                if (text.Length > 0)
                    list.Add(text);
            }

            return list;
        }

        private static int Int(JsonObject root, string key, int fallback = 0)
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            return int.TryParse(value.ToString().Trim('"'), out var parsed) ? parsed : fallback;
        }

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            return value.ToString().Trim('"');
        }
    }
}