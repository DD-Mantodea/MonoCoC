using Hjson;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Npcs
{
    public class NpcState
    {
        public bool Met;

        public bool Hostile;

        public bool Follower;

        public int Affection;

        public HashSet<Identifier> Flags = [];

        public Identifier PregnancyType;

        public float PregnancyIncubation;

        public int PregnancyNotice;

        public Identifier ButtPregnancyType;

        public float ButtPregnancyIncubation;

        public int ButtPregnancyNotice;

        public bool IsPregnant => PregnancyIncubation > 0;

        public bool IsButtPregnant => ButtPregnancyIncubation > 0;

        public virtual void Reset()
        {
            Met = false;

            Hostile = false;

            Follower = false;

            Affection = 0;

            Flags.Clear();

            PregnancyType = default;

            PregnancyIncubation = 0;

            ButtPregnancyType = default;

            ButtPregnancyIncubation = 0;
        }

        public virtual void Save(JsonObject json)
        {
            json["met"] = Met;

            json["hostile"] = Hostile;

            json["follower"] = Follower;

            json["affection"] = Affection;

            var flags = new JsonArray();

            foreach (var flag in Flags)
                flags.Add(flag.ToString());

            json["flags"] = flags;

            json["pregnancyType"] = PregnancyType.ToString();

            json["pregnancyIncubation"] = PregnancyIncubation;

            json["buttPregnancyType"] = ButtPregnancyType.ToString();

            json["buttPregnancyIncubation"] = ButtPregnancyIncubation;
        }

        public virtual void Load(JsonObject json)
        {
            Met = json.TryGetValue("met", out var met) && (bool)met;

            Hostile = json.TryGetValue("hostile", out var hostile) && (bool)hostile;

            Follower = json.TryGetValue("follower", out var follower) && (bool)follower;

            Affection = json.TryGetValue("affection", out var affection) ? (int)affection : 0;

            Flags.Clear();

            if (json.TryGetValue("flags", out var value) && value is JsonArray flags)
                foreach (var flag in flags)
                    if (Identifier.TryParse((string)flag, out var id))
                        Flags.Add(id);

            PregnancyType = json.TryGetValue("pregnancyType", out var pregnancyType)

                            && Identifier.TryParse((string)pregnancyType, out var parsedType)

                ? parsedType

                : default;

            PregnancyIncubation = json.TryGetValue("pregnancyIncubation", out var incubation) ? (float)incubation : 0f;

            ButtPregnancyType = json.TryGetValue("buttPregnancyType", out var buttType)

                                && Identifier.TryParse((string)buttType, out var parsedButtType)

                ? parsedButtType

                : default;

            ButtPregnancyIncubation = json.TryGetValue("buttPregnancyIncubation", out var buttIncubation)

                ? (float)buttIncubation

                : 0f;
        }
    }
}