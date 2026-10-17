using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class AnalPart : BodyPart
    {
        public const int WetnessDry = 0;

        public const int WetnessNormal = 1;

        public const int WetnessMoist = 2;

        public const int WetnessSlimy = 3;

        public const int WetnessDrooling = 4;

        public const int WetnessSlimeDrooling = 5;

        public override Identifier SlotId => BodyPartSlots.Anal;

        public int Wetness;

        public int Looseness;

        public int Fullness;

        public bool Virgin = true;

        public override void Restore()
        {
            base.Restore();

            Wetness = 0;

            Looseness = 0;

            Fullness = 0;

            Virgin = true;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["wetness"] = Wetness;

            json["looseness"] = Looseness;

            json["fullness"] = Fullness;

            json["virgin"] = Virgin;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Wetness = json.TryGetValue("wetness", out var wetness) ? (int)wetness : 0;

            Looseness = json.TryGetValue("looseness", out var looseness) ? (int)looseness : 0;

            Fullness = json.TryGetValue("fullness", out var fullness) ? (int)fullness : 0;

            Virgin = json.TryGetValue("virgin", out var virgin) && (bool)virgin;
        }

        public string AssholeDescript(bool forceDesc = false)
        {
            var description = "";

            if (forceDesc || Random.Shared.Next(3) <= 1)
            {
                var wet = Wetness switch
                {
                    2 => "Appearance.Ass.Wet.Moist",

                    3 => "Appearance.Ass.Wet.Slimy",

                    4 => "Appearance.Ass.Wet.Drooling",

                    _ => Wetness > 4 ? "Appearance.Ass.Wet.SlimeDrooling" : ""
                };

                description += wet.Localized();
            }

            if (forceDesc || Looseness == 0 || Random.Shared.Next(4) == 0 || (Looseness <= 1 && Random.Shared.Next(4) <= 2))
            {
                var tight = Looseness switch
                {
                    0 => "Appearance.Ass.Tight.Virgin",

                    1 => "Appearance.Ass.Tight.Tight",

                    2 => "Appearance.Ass.Tight.Loose",

                    3 => "Appearance.Ass.Tight.Roomy",

                    4 => "Appearance.Ass.Tight.Stretched",

                    _ => "Appearance.Ass.Tight.Gaping"
                };

                description += tight.Localized();
            }

            description += Pick("Appearance.Ass.Noun.Ass", "Appearance.Ass.Noun.Anus", "Appearance.Ass.Noun.Pucker", "Appearance.Ass.Noun.Backdoor", "Appearance.Ass.Noun.Asshole", "Appearance.Ass.Noun.Butthole");

            return description;
        }

        public static string Pick(params string[] keys) => keys[Random.Shared.Next(keys.Length)].Localized();
    }
}