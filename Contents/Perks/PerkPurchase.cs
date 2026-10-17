using Hjson;
using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Perks
{
    /// <summary>
    /// 升级时可购买的一条专长。数据来源：Assets/Data/Perks/*.hjson 的 purchasable 数组
    /// （由现有 PerkManager 解析），模组也可以调用 PerkPurchaseCatalog.Register 直接注册。
    /// </summary>
    public class PerkPurchase
    {
        public Identifier Perk = new("coc", "none");

        /// <summary>留空时使用专长自己的本地化名（PerkType.Name）。</summary>
        public string NameKey = "";

        /// <summary>留空时使用专长自己的本地化描述（PerkType.Description）。</summary>
        public string DescriptionKey = "";

        public int Cost = 1;

        public List<PerkRequirement> Requirements = [];

        public string DisplayName()
        {
            if (NameKey.Length > 0)
            {
                var text = NameKey.Localized();

                if (text != NameKey)
                    return text;
            }

            return PerkRegistry.GetName(Perk);
        }

        public string Description()
        {
            if (DescriptionKey.Length > 0)
            {
                var text = DescriptionKey.Localized();

                if (text != DescriptionKey)
                    return text;
            }

            return PerkRegistry.Get(Perk)?.Description ?? "";
        }

        public bool IsAvailable(Player player) =>

            !player.HasPerk(Perk) && Requirements.All(requirement => requirement.IsMet(player));

        public string RequirementText() =>

            string.Join(", ", Requirements.Select(requirement => requirement.Describe()));
    }

    public static class PerkPurchaseCatalog
    {
        private static readonly Dictionary<string, PerkPurchase> Entries = new(StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyCollection<PerkPurchase> All => Entries.Values;

        /// <summary>注册或覆盖一条可购买专长（同一专长 id 重复注册时后者覆盖，便于数据重载/模组覆盖）。</summary>
        public static void Register(PerkPurchase purchase) => Entries[purchase.Perk.ToString()] = purchase;

        public static void Clear() => Entries.Clear();

        public static PerkPurchase? Find(Identifier perk) =>

            Entries.TryGetValue(perk.ToString(), out var purchase) ? purchase : null;

        public static List<PerkPurchase> Available(Player player) =>

            Entries.Values.Where(purchase => purchase.IsAvailable(player)).OrderBy(purchase => purchase.Perk.Path).ToList();

        public static List<PerkPurchase> Unavailable(Player player) =>

            Entries.Values.Where(purchase => !player.HasPerk(purchase.Perk) && !purchase.IsAvailable(player))
                .OrderBy(purchase => purchase.Perk.Path).ToList();

        // ── 数据解析（PerkManager 调用）────────────────────────────────

        public static void LoadFromJson(JsonObject root)
        {
            if (!root.TryGetValue("purchasable", out var value) || value is not JsonArray array)
                return;

            foreach (var item in array)
            {
                if (item is not JsonObject entry)
                    continue;

                var purchase = Parse(entry);

                if (purchase != null)
                    Register(purchase);
            }
        }

        private static PerkPurchase? Parse(JsonObject entry)
        {
            var perkId = Str(entry, "perk");

            if (!Identifier.TryParse(perkId, out var perk))
                return null;

            var purchase = new PerkPurchase
            {
                Perk = perk,

                NameKey = Str(entry, "nameKey"),

                DescriptionKey = Str(entry, "descriptionKey"),

                Cost = Math.Max(1, (int)Num(entry, "cost", 1)),
            };

            if (entry.TryGetValue("requirements", out var value) && value is JsonArray requirements)
                foreach (var item in requirements)
                {
                    if (item is not JsonObject requirement)
                        continue;

                    var parsed = ParseRequirement(requirement);

                    if (parsed != null)
                        purchase.Requirements.Add(parsed);
                }

            return purchase;
        }

        private static PerkRequirement? ParseRequirement(JsonObject json)
        {
            var type = Str(json, "type");

            if (type.Length == 0)
                return null;

            var requirement = new PerkRequirement
            {
                Type = type,

                Stat = Str(json, "stat"),

                Value = Num(json, "value", 0),

                ItemName = Str(json, "keyItem"),

                TextKey = Str(json, "textKey"),
            };

            if (Identifier.TryParse(Str(json, "perk"), out var perk))
                requirement.Perk = perk;

            if (Identifier.TryParse(Str(json, "status"), out var status))
                requirement.Status = status;

            if (Identifier.TryParse(Str(json, "mastery"), out var mastery))
                requirement.Mastery = mastery;

            if (json.TryGetValue("requirements", out var children) && children is JsonArray array)
                foreach (var item in array)
                {
                    if (item is not JsonObject child)
                        continue;

                    var parsed = ParseRequirement(child);

                    if (parsed != null)
                        requirement.Children.Add(parsed);
                }

            return requirement;
        }

        private static string Str(JsonObject json, string key, string fallback = "") =>

            json.TryGetValue(key, out var value) && value != null ? value.ToString().Trim('"') : fallback;

        private static float Num(JsonObject json, string key, float fallback = 0f) =>

            json.TryGetValue(key, out var value) && value != null && double.TryParse(value.ToString(), out var number) ? (float)number : fallback;
    }
}
