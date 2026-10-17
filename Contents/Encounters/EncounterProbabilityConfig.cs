using System;
using System.Collections.Generic;
using System.IO;
using Hjson;
using MonoCoC.Assets;

namespace MonoCoC.Contents.Encounters
{
    /// <summary>
    /// 玩家可编辑的遭遇概率表：Assets/Data/Encounters/Probabilities.hjson
    /// 值可为数字（倍率）或对象 { multiplier, chance }。
    /// chance 是绝对概率，设置后忽略倍率。
    /// </summary>
    public static class EncounterProbabilityConfig
    {
        private sealed class Entry
        {
            public float Multiplier = 1f;

            public float? Chance;
        }

        private static readonly Dictionary<string, Entry> Entries = Load();

        private static Dictionary<string, Entry> Load()
        {
            var result = new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);

            var path = Path.Combine(Paths.Root, "Assets", "Data", "Encounters", "Probabilities.hjson");

            if (!File.Exists(path))
                return result;

            try
            {
                if (HjsonValue.Parse(File.ReadAllText(path)) is not JsonObject root)
                    return result;

                foreach (var pair in root)
                {
                    var entry = new Entry();

                    if (pair.Value is JsonObject obj)
                    {
                        if (obj.TryGetValue("multiplier", out var multiplier) && float.TryParse(multiplier.ToString(), out var m))
                            entry.Multiplier = m;

                        if (obj.TryGetValue("chance", out var chance) && float.TryParse(chance.ToString(), out var c))
                            entry.Chance = c;
                    }
                    else if (float.TryParse(pair.Value.ToString(), out var multiplier))
                        entry.Multiplier = multiplier;

                    result[pair.Key] = entry;
                }
            }
            catch
            {
                // 配置损坏时安全回退到原版概率
            }

            return result;
        }

        public static float Apply(string name, float baseChance)
        {
            if (!Entries.TryGetValue(name, out var entry))
                return baseChance;

            if (entry.Chance.HasValue)
                return entry.Chance.Value;

            return baseChance * entry.Multiplier;
        }
    }
}