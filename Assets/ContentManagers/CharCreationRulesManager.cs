using Hjson;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.CharacterCreation;
using MonoCoC.Contents.Stats;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class CharCreationRulesManager : AssetManager<CharCreationRules>
    {
        public override string Folder => "Data/CharacterCreation";

        public override string Pattern => "*.hjson";

        public CharCreationRules Rules => Dict["coc:charCreation"];

        public override void FromText(string path, string text, IDictionary<string, CharCreationRules> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var rules = new CharCreationRules();

            foreach (var pair in (JsonObject)root["ages"])
                rules.Ages[Enum.Parse<Age>(pair.Key, true)] = ParseAge((JsonObject)pair.Value);

            foreach (var pair in (JsonObject)root["builds"])
                rules.Builds[Enum.Parse<Build>(pair.Key, true)] = ParseBuild((JsonObject)pair.Value);

            foreach (var pair in (JsonObject)root["defaultHairLength"])
                rules.DefaultHairLength[Enum.Parse<Gender>(pair.Key, true)] = (float)pair.Value;

            foreach (var pair in (JsonObject)root["statBonuses"])
                rules.StatBonuses[Enum.Parse<Gender>(pair.Key, true)] = ParseStats((JsonArray)pair.Value);

            entries["coc:charCreation"] = rules;
        }

        private static AgeStats ParseAge(JsonObject json) => new()
        {
            Strength = (float)json["strength"],

            Toughness = (float)json["toughness"],

            Speed = (float)json["speed"],

            Intelligence = (float)json["intelligence"],

            Sensitivity = (float)json["sensitivity"],

            Libido = (float)json["libido"],

            Lust = (float)json["lust"],

            Bonus = (int)json["bonus"],

            NippleLength = (float)json["nippleLength"],

            ClitLength = (float)json["clitLength"],

            CockLength = (float)json["cockLength"],

            CockThickness = (float)json["cockThickness"],

            BallSize = (float)json["ballSize"],

            TallnessMale = (float)json["tallnessMale"],

            TallnessFemale = (float)json["tallnessFemale"],

            TallnessHerm = (float)json["tallnessHerm"],

            FertilityMale = (float)json["fertilityMale"],

            FertilityFemale = (float)json["fertilityFemale"],

            FertilityHerm = (float)json["fertilityHerm"],

            ToneMale = (float)json["toneMale"],

            ToneFemale = (float)json["toneFemale"],

            ToneHerm = (float)json["toneHerm"]
        };

        private static BuildStats ParseBuild(JsonObject json)
        {
            var build = new BuildStats
            {
                Thickness = (float)json["thickness"],

                Strength = (float)json["strength"],

                Toughness = (float)json["toughness"],

                Speed = (float)json["speed"],

                Tone = (float)json["tone"],

                SetToneMale = json.TryGetValue("setToneMale", out var maleTone) ? (float)maleTone : null,

                SetToneFemale = json.TryGetValue("setToneFemale", out var femaleTone) ? (float)femaleTone : null,

                Male = ParseMods((JsonObject)json["male"]),

                Female = ParseMods((JsonObject)json["female"])
            };

            return build;
        }

        private static Dictionary<Age, BuildMods> ParseMods(JsonObject json)
        {
            var mods = new Dictionary<Age, BuildMods>();

            foreach (var pair in json)
                mods[Enum.Parse<Age>(pair.Key, true)] = new BuildMods
                {
                    Femininity = (float)((JsonObject)pair.Value)["femininity"],

                    Breasts = (float)((JsonObject)pair.Value)["breasts"],

                    Butt = (float)((JsonObject)pair.Value)["butt"],

                    Hips = (float)((JsonObject)pair.Value)["hips"]
                };

            return mods;
        }

        private static Stat[] ParseStats(JsonArray json)
        {
            var stats = new Stat[json.Count];

            for (var index = 0; index < json.Count; index++)
                stats[index] = (string)json[index];

            return stats;
        }
    }
}