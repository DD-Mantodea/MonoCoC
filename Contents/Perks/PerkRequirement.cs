using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Perks
{
    /// <summary>
    /// 原版 PerkType.requirements 的数据化表达：数据里写 type + 参数，运行时由注册的 handler 判定并生成本地化描述。
    /// 支持的 type 与参数见 PerkRequirementRegistry 里的注册（模组可自行 Register 扩展）。
    /// </summary>
    public class PerkRequirement
    {
        public string Type = "";

        public string Stat = "";

        public float Value;

        public Identifier Perk = new("coc", "none");

        public Identifier Status = new("coc", "none");

        public Identifier Mastery = new("coc", "none");

        public string ItemName = "";

        /// <summary>可选：覆盖自动生成的描述键（例如状态/自定义条件想用自己的文案）。</summary>
        public string TextKey = "";

        public List<PerkRequirement> Children = [];

        public bool IsMet(Player player) =>

            PerkRequirementRegistry.TryGet(Type, out var handler) && handler.Met(this, player);

        public string Describe()
        {
            if (TextKey.Length > 0)
            {
                var text = TextKey.Localized();

                if (text != TextKey)
                    return text;
            }

            return PerkRequirementRegistry.TryGet(Type, out var handler)

                ? handler.Describe(this)

                : Type;
        }
    }

    public record PerkRequirementHandler(
        Func<PerkRequirement, Player, bool> Met,

        Func<PerkRequirement, string> Describe);

    public static class PerkRequirementRegistry
    {
        private static readonly Dictionary<string, PerkRequirementHandler> Handlers = new(StringComparer.OrdinalIgnoreCase);

        static PerkRequirementRegistry()
        {
            Register("attribute", (req, player) =>
            {
                return GetStat(player, req.Stat) >= req.Value;
            }, req => "Perk.Requirement.Attribute".Localized(StatName(req.Stat), req.Value));

            Register("attribute_less", (req, player) => GetStat(player, req.Stat) < req.Value,

                req => "Perk.Requirement.AttributeLess".Localized(StatName(req.Stat), req.Value));

            Register("level", (req, player) => player.Level >= req.Value,

                req => "Perk.Requirement.Level".Localized(req.Value));

            Register("min_lust", (req, player) => player.MinLust() >= req.Value,

                req => "Perk.Requirement.MinLust".Localized(req.Value));

            Register("perk", (req, player) => player.HasPerk(req.Perk),

                req => PerkRegistry.GetName(req.Perk));

            Register("status", (req, player) => player.HasStatusEffect(req.Status),

                req => "Perk.Requirement.Status".Localized(req.Status.Path));

            Register("mastery", (req, player) => player.MasteryLevel(req.Mastery) >= req.Value,

                req => "Perk.Requirement.Mastery".Localized(req.Mastery.Path, req.Value));

            Register("hunger", (req, player) => Configuration.OptionStore.Instance.Get("survival", 0) > 0,

                _ => "Perk.Requirement.Hunger".Localized());

            Register("key_item", (req, player) => player.Inventory.HasItem(new Identifier("coc", req.ItemName)) || player.HasKeyItem(req.ItemName),

                req => "Perk.Requirement.Item".Localized(req.ItemName));

            Register("key_item_named", (req, player) => player.HasKeyItem(req.ItemName),

                req => "Perk.Requirement.Item".Localized(req.ItemName));

            Register("any", (req, player) =>
            {
                foreach (var child in req.Children)
                    if (child.IsMet(player))
                        return true;

                return false;
            }, req =>
            {
                var parts = new List<string>();

                foreach (var child in req.Children)
                    parts.Add(child.Describe());

                return "Perk.Requirement.Any".Localized(string.Join(", ", parts));
            });
        }

        public static void Register(string type, Func<PerkRequirement, Player, bool> met, Func<PerkRequirement, string> describe) =>

            Handlers[type] = new PerkRequirementHandler(met, describe);

        public static bool TryGet(string type, out PerkRequirementHandler handler) => Handlers.TryGetValue(type, out handler);

        private static float GetStat(Player player, string stat) => stat.ToLowerInvariant() switch
        {
            "strength" => player.Strength,

            "toughness" => player.Toughness,

            "speed" => player.Speed,

            "intelligence" => player.Intelligence,

            "libido" => player.Libido,

            "corruption" => player.Corruption,

            _ => 0f,
        };

        private static string StatName(string stat) => stat.ToLowerInvariant() switch
        {
            "strength" => "Scenes.LevelUp.StatName.Strength".Localized(),

            "toughness" => "Scenes.LevelUp.StatName.Toughness".Localized(),

            "speed" => "Scenes.LevelUp.StatName.Speed".Localized(),

            "intelligence" => "Scenes.LevelUp.StatName.Intelligence".Localized(),

            "libido" => "Perk.Requirement.Libido".Localized(),

            "corruption" => "Perk.Requirement.Corruption".Localized(),

            _ => stat,
        };
    }
}