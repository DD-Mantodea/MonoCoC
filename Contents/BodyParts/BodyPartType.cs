using Hjson;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.BodyParts
{
    public class BodyPartType : IIdentifiable
    {
        public Identifier Id { get; set; }

        public Identifier Slot { get; set; }

        public string Name
        {
            get
            {
                var key = $"BodyParts.{Slot.Path}.{Id.Path}.Name";

                var value = key.Localized();

                return value == key ? Id.Path.Replace('_', ' ') : value;
            }
        }

        public string DescKey = "";

        public JsonObject Config;

        public bool GetBool(string key) => Config != null && Config.TryGetValue(key, out var value) && (bool)value;

        public string Appearance(string skinKind)
        {
            if (Config != null && Config.TryGetValue("appearances", out var appearances) && appearances is JsonObject map && map.TryGetValue(skinKind, out var value))
                return value.ToString().Trim('"');

            return GetString("appearance");
        }

        public string GetString(string key) => Config != null && Config.TryGetValue(key, out var value) ? (string)value : "";

        public int GetInt(string key) => Config != null && Config.TryGetValue(key, out var value) ? (int)value : 0;

        public bool HasRatings => Config != null && Config.TryGetValue("ratings", out _);

        public float RatingAnchor(string anchor)
        {
            if (Config != null && Config.TryGetValue("anchors", out var anchors) && anchors is JsonObject map
                && map.TryGetValue(anchor, out var value))

                return (float)(double)value;

            return 0f;
        }

        public string DescribeRating(float rating, float tone)
        {
            if (Config == null)
                return Name;

            var toneLevel = tone >= 65f ? "2" : tone >= 30f ? "1" : "0";

            if (TryGetTier(rating, toneLevel, out var tierKeys))
            {

                var text = Pick(tierKeys);

                return text.EndsWith(' ') ? text + PickSuffix() : text;
            }

            var description = "";

            if (Config.TryGetValue("lowest", out var lowestValue) && lowestValue is JsonObject lowest)
            {
                var firmText = lowest.TryGetValue("firmText", out var firm) ? (string)firm : "";

                if (tone >= 60f && firmText.Length > 0)
                    description = firmText;
                else if (lowest.TryGetValue("keys", out var lowKeys) && lowKeys is JsonArray lowList)
                    description = Pick(lowList);

                if (tone < 60f && tone <= 30f && Random.Shared.Next(3) == 0)
                {
                    var softKey = lowest.TryGetValue("softKey", out var soft) ? (string)soft : "";

                    if (softKey.Length > 0)
                        description += softKey.Localized();
                }
            }

            if (description.EndsWith(' '))
                description += PickSuffix();

            return description;
        }

        public bool TryGetTier(float rating, string toneLevel, out JsonArray keys)
        {
            keys = null;

            if (!Config.TryGetValue("ratings", out var tiersValue) || tiersValue is not JsonArray tiers)
                return false;

            foreach (var item in tiers)
            {
                if (item is not JsonObject tier)
                    continue;

                var min = tier.TryGetValue("min", out var minValue) ? (float)(double)minValue : 0f;

                if (rating < min)
                    continue;

                if (tier.TryGetValue("keys", out var keysValue) && keysValue is JsonObject map
                    && map.TryGetValue(toneLevel, out var listValue) && listValue is JsonArray list)
                {
                    keys = list;

                    return true;
                }

                return false;
            }

            return false;
        }

        public string PickSuffix() =>

            Config.TryGetValue("suffixKeys", out var suffix) && suffix is JsonArray list ? Pick(list) : "";

        public string DescribeHips(float rating, float thickness, bool smallFlared)
        {
            if (Config == null || !Config.TryGetValue("ratings", out var tiersValue) || tiersValue is not JsonArray tiers)
                return Name;

            foreach (var item in tiers)
            {
                if (item is not JsonObject tier)
                    continue;

                var min = tier.TryGetValue("min", out var minValue) ? (float)(double)minValue : 0f;

                if (rating < min)
                    continue;

                var mode = tier.TryGetValue("mode", out var modeValue) ? (string)modeValue : "alt";

                var body = tier.TryGetValue("body", out var bodyValue) && bodyValue is JsonArray bodyList ? bodyList : null;

                JsonArray prefix = null;

                if (mode == "add")
                {
                    var thin = tier.TryGetValue("thin", out var thinValue) ? (float)(double)thinValue : 0f;

                    if (thickness < thin && tier.TryGetValue("thinPrefix", out var thinPrefixValue) && thinPrefixValue is JsonArray thinList)
                        prefix = thinList;
                }
                else
                    if (tier.TryGetValue("thin", out var thinValue) && thickness < (float)(double)thinValue
                        && tier.TryGetValue("thinPrefix", out var thinPrefixValue) && thinPrefixValue is JsonArray thinList)

                        prefix = thinList;
                    else if (smallFlared && tier.TryGetValue("smallPrefix", out var smallPrefixValue) && smallPrefixValue is JsonArray smallList)
                        prefix = smallList;

                var text = mode == "add" ? Pick(prefix) + Pick(body) : Pick(prefix) + Pick(body);

                if (mode == "alt" && prefix != null)
                    return Pick(prefix) + PickSuffix();

                return text + PickSuffix();
            }

            if (Config.TryGetValue("lowest", out var lowestValue) && lowestValue is JsonObject lowest
                && lowest.TryGetValue("keys", out var lowKeys) && lowKeys is JsonArray lowList)

                return Pick(lowList) + PickSuffix();

            return Name;
        }

        public static string Pick(JsonArray list)
        {
            if (list == null || list.Count == 0)
                return "";

            return ((string)list[Random.Shared.Next(list.Count)]).Localized();
        }
    }
}