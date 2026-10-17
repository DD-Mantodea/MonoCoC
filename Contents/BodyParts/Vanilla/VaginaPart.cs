using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class VaginaPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Vagina;

        public float Wetness = 1;

        public int Looseness;

        public bool Virgin = true;

        public float RecoveryProgress;

        public float ClitLength = 0.5f;

        public int ClitPierced;

        public int clitPierced { get => ClitPierced; set => ClitPierced = value; }

        public string clitPShort { get => ClitPShort; set => ClitPShort = value; }

        public string clitPLong { get => ClitPLong; set => ClitPLong = value; }

        public int LabiaPierced;

        public string ClitPShort = "";

        public string ClitPLong = "";

        public string LabiaPShort = "";

        public string LabiaPLong = "";

        public float Capacity(float bonus = 0) => (bonus + 8 * Looseness * Looseness) * (1 + Wetness / 10f);

        public void ResetRecoveryProgress() => RecoveryProgress = 0;

        public bool Stretch(float cArea, bool hasFeraMilkingTwat = false, float bonus = 0)
        {
            var stretched = false;

            if (!hasFeraMilkingTwat || Looseness <= 1)
                if (cArea >= Capacity(bonus))
                {
                    Looseness += 1;

                    stretched = true;
                }
                else if (cArea >= 0.9f * Capacity(bonus) && Random.Shared.Next(2) == 0)
                {
                    Looseness += 1;

                    stretched = true;
                }
                else if (cArea >= 0.75f * Capacity(bonus) && Random.Shared.Next(4) == 0)
                {
                    Looseness += 1;

                    stretched = true;
                }

            if (Looseness > 5)
                Looseness = 5;

            if (hasFeraMilkingTwat && Looseness > 1)
                Looseness = 1;

            if (Virgin)
                Virgin = false;

            return stretched;
        }

        public bool WillStretch(float cArea, bool hasFeraMilkingTwat = false, float bonus = 0)
        {
            if (hasFeraMilkingTwat || Looseness > 1)
                return false;

            if (cArea >= Capacity(bonus))
                return true;

            if (cArea >= 0.9f * Capacity(bonus) && Random.Shared.Next(2) == 0)
                return true;

            if (cArea >= 0.75f * Capacity(bonus) && Random.Shared.Next(4) == 0)
                return true;

            return false;
        }

        public override void Restore()
        {
            base.Restore();

            Wetness = 1;

            Looseness = 0;

            Virgin = true;

            RecoveryProgress = 0;

            ClitLength = 0.5f;

            ClitPierced = 0;

            LabiaPierced = 0;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["wetness"] = Wetness;

            json["looseness"] = Looseness;

            json["virgin"] = Virgin;

            json["recoveryProgress"] = RecoveryProgress;

            json["clitLength"] = ClitLength;

            json["clitPierced"] = ClitPierced;

            json["labiaPierced"] = LabiaPierced;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Wetness = json.TryGetValue("wetness", out var wetness) ? (float)wetness : 1;

            Looseness = json.TryGetValue("looseness", out var looseness) ? (int)looseness : 0;

            Virgin = json.TryGetValue("virgin", out var virgin) && (bool)virgin;

            RecoveryProgress = json.TryGetValue("recoveryProgress", out var recovery) ? (float)recovery : 0;

            ClitLength = json.TryGetValue("clitLength", out var clit) ? (float)clit : 0.5f;

            ClitPierced = json.TryGetValue("clitPierced", out var clitPierced) ? (int)(double)clitPierced : 0;

            LabiaPierced = json.TryGetValue("labiaPierced", out var labiaPierced) ? (int)(double)labiaPierced : 0;
        }
    }
}