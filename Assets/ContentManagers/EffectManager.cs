using Hjson;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    // 状态效果的数据表。目录约定同 Data/Combat/Abilities。
    public class EffectManager : ContentManager<EffectDefinition>
    {
        public override string Folder => "Data/Effects";

        public override string Pattern => "*.hjson";

        // 全局查表入口：状态按 id 懒解析，不往存档里塞定义对象。
        public static EffectDefinition? Get(Identifier id) =>

            ManagerInstance<EffectManager>.Instance.Dict.TryGetValue(id.ToString(), out var definition)

                ? definition

                : null;

        public override void FromText(string path, string text, IDictionary<string, EffectDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = Str(root, "id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid effect id '{id}' in '{path}'");

            var definition = new EffectDefinition
            {
                Id = identifier,

                NameKey = Str(root, "nameKey"),

                DescriptionKey = Str(root, "descriptionKey"),

                Kind = ParseKind(Str(root, "kind", "combat")),

                Duration = Float(root, "duration"),

                Stacking = ParseStacking(Str(root, "stacking", "refresh")),

                MaxStacks = Int(root, "maxStacks", 1),

                CombatOnly = Bool(root, "combatOnly", true),

                BlockedTextKey = Str(root, "blockedText")
            };

            if (root.TryGetValue("flags", out var flagsValue) && flagsValue is JsonArray flags)
                foreach (var flag in flags)
                    definition.Flags |= ParseFlag(flag.ToString());

            ReadHandler(root, "onApply", id => definition.OnApply = id);

            ReadHandler(root, "onRound", id => definition.OnRound = id);

            ReadHandler(root, "onExpire", id => definition.OnExpire = id);

            entries[identifier.ToString()] = definition;
        }

        private static void ReadHandler(JsonObject root, string key, Action<Identifier> set)
        {
            if (Identifier.TryParse(Str(root, key), out var handler))
                set(handler);
        }

        private static EffectKind ParseKind(string value) => value.ToLowerInvariant() switch
        {
            "timed" => EffectKind.Timed,

            "permanent" => EffectKind.Permanent,

            _ => EffectKind.Combat
        };

        private static EffectStacking ParseStacking(string value) => value.ToLowerInvariant() switch
        {
            "extend" => EffectStacking.Extend,

            "stack" => EffectStacking.Stack,

            "ignore" => EffectStacking.Ignore,

            _ => EffectStacking.Refresh
        };

        private static EffectFlags ParseFlag(string value) => value.Trim('"').ToLowerInvariant() switch
        {
            "blocks_magic" => EffectFlags.BlocksMagic,

            "blocks_flee" => EffectFlags.BlocksFlee,

            "prevents_action" => EffectFlags.PreventsAction,

            "prevents_tease" => EffectFlags.PreventsTease,

            "prevents_physical" => EffectFlags.PreventsPhysical,

            _ => EffectFlags.None
        };

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            var text = value.ToString();

            if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
                text = text[1..^1];

            return text;
        }

        private static int Int(JsonObject root, string key, int fallback = 0) =>

            int.TryParse(Str(root, key), out var value) ? value : fallback;

        private static float Float(JsonObject root, string key, float fallback = 0) =>

            float.TryParse(Str(root, key), out var value) ? value : fallback;

        private static bool Bool(JsonObject root, string key, bool fallback = false) =>

            bool.TryParse(Str(root, key), out var value) ? value : fallback;
    }
}