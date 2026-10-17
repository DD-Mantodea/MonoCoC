using Hjson;
using MonoCoC.Contents.Combat;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class CombatAbilityManager : ContentManager<CombatAbilityDefinition>
    {
        public override string Folder => "Data/Combat/Abilities";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, CombatAbilityDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = Str(root, "id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid ability id '{id}' in '{path}'");

            var ability = new CombatAbilityDefinition
            {
                Id = identifier,

                NameKey = Str(root, "nameKey"),

                DescriptionKey = Str(root, "descriptionKey"),

                Type = ParseType(Str(root, "type", "physical")),

                Category = ParseCategory(Str(root, "category", "enemy")),

                Range = ParseRange(Str(root, "range", "melee")),

                Cost = Int(root, "cost"),

                Fatigue = ParseFatigue(Str(root, "fatigueType", "none")),

                Cooldown = Int(root, "cooldown"),

                DominikaOrder = Int(root, "dominikaOrder"),

                Power = Float(root, "power"),

                DisabledTooltip = Str(root, "disabledTooltip"),

                IsSelf = Bool(root, "isSelf"),

                IsHeal = Bool(root, "isHeal")
            };

            if (Identifier.TryParse(Str(root, "knownWhen"), out var knownWhen))
                ability.KnownWhen = knownWhen;

            if (Identifier.TryParse(Str(root, "availableWhen"), out var availableWhen))
                ability.AvailableWhen = availableWhen;

            if (Identifier.TryParse(Str(root, "disabledWhen"), out var disabledWhen))
                ability.DisabledWhen = disabledWhen;

            if (Identifier.TryParse(Str(root, "handler"), out var handler))
                ability.Handler = handler;

            if (Identifier.TryParse(Str(root, "damageType"), out var damageType))
                ability.DamageType = damageType;

            entries[identifier.ToString()] = ability;
        }

        private static CombatAbilityType ParseType(string value) => value.ToLowerInvariant() switch
        {
            "magical" => CombatAbilityType.Magical,

            "tease" => CombatAbilityType.Tease,

            "support" => CombatAbilityType.Support,

            _ => CombatAbilityType.Physical
        };

        private static CombatAbilityCategory ParseCategory(string value) => value.ToLowerInvariant() switch
        {
            "white" => CombatAbilityCategory.WhiteMagic,

            "black" => CombatAbilityCategory.BlackMagic,

            "gray" => CombatAbilityCategory.GrayMagic,

            "physical" => CombatAbilityCategory.Physical,

            "magical" => CombatAbilityCategory.Magical,

            _ => CombatAbilityCategory.Enemy
        };

        private static bool Bool(JsonObject root, string key, bool fallback = false) =>

            bool.TryParse(Str(root, key), out var value) ? value : fallback;

        internal static FatigueType ParseFatigue(string value) => value.ToLowerInvariant() switch
        {
            "magical" => FatigueType.Magical,

            "physical" => FatigueType.Physical,

            "magical_heal" => FatigueType.MagicalHeal,

            _ => FatigueType.None
        };

        private static CombatRange ParseRange(string value) => value.ToLowerInvariant() switch
        {
            "ranged" => CombatRange.Ranged,

            "self" => CombatRange.Self,

            "tease" => CombatRange.Tease,

            "omni" => CombatRange.Omni,

            "flying_melee" => CombatRange.FlyingMelee,

            "charging_melee" => CombatRange.ChargingMelee,

            _ => CombatRange.Melee
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
    }

    public class CombatAiManager : ContentManager<CombatAiDefinition>
    {
        public override string Folder => "Data/Combat/Ai";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, CombatAiDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = Str(root, "id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid AI id '{id}' in '{path}'");

            var ai = new CombatAiDefinition
            {
                Id = identifier,

                NameKey = Str(root, "nameKey")
            };

            if (root.TryGetValue("actions", out var value) && value is JsonArray actions)
                foreach (var entry in actions)
                {
                    if (entry is not JsonObject actionJson)
                        continue;

                    var action = new CombatAiAction
                    {
                        Weight = Float(actionJson, "weight", 1f),

                        Priority = Int(actionJson, "priority")
                    };

                    // 条目级消耗覆盖（原版 MonsterAI.add 的 cost / fatigueType 参数）
                    if (actionJson.ContainsKey("cost"))
                        action.Cost = Int(actionJson, "cost");

                    if (actionJson.ContainsKey("fatigueType"))
                        action.Fatigue = CombatAbilityManager.ParseFatigue(Str(actionJson, "fatigueType", "none"));

                    if (Identifier.TryParse(Str(actionJson, "ability"), out var ability))
                        action.Ability = ability;

                    if (Identifier.TryParse(Str(actionJson, "when"), out var when))
                        action.When = when;

                    if (Identifier.TryParse(Str(actionJson, "requiresStatus"), out var required))
                        action.RequiresStatus = required;

                    if (Identifier.TryParse(Str(actionJson, "forbidsStatus"), out var forbidden))
                        action.ForbidStatus = forbidden;

                    ai.Actions.Add(action);
                }

            entries[identifier.ToString()] = ai;
        }

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
    }
}