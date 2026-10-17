using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class CockPart : BodyPart
    {

        public bool HasKnot => KnotMultiplier > 1f;

        public const float KnotMultiplierNoKnot = 1f;

        public float KnotThickness => Thickness * KnotMultiplier;

        public override Identifier SlotId => BodyPartSlots.Cock;

        public int TypeIndex
        {
            get
            {
                // 原版 CockTypesEnum 的枚举顺序，与 BodyPartTypes.CockTypeOrder 一致
                // （旧实现只硬编码了前 13 种，新增 gnoll 等类型会全部落到 0）
                var index = BodyPartTypes.Index(Id);

                return index < 0 ? 0 : index;
            }
        }

        public string PShortDesc = "";

        public string PLongDesc = "";

        public float Length;

        public float Thickness;

        public void Thicken(float amount)
        {
            if (Thickness >= Length * 0.33f && amount > 0)
                return;

            Thickness += amount;
        }

        public float KnotMultiplier = 1;

        public float Pierced;

        public string PiercingShortName = "";

        public string PiercingLongName = "";

        public string Sock = "";

        public string sock { get => Sock; set => Sock = value; }

        public float pierced { get => Pierced; set => Pierced = value; }

        public string pShortDesc { get => PShortDesc; set => PShortDesc = value; }

        public string pLongDesc { get => PLongDesc; set => PLongDesc = value; }

        public bool isPierced => Pierced > 0;

        public bool SheathOn;

        public override void Restore()
        {
            base.Restore();

            Length = 0;

            Thickness = 0;

            KnotMultiplier = 1;

            Pierced = 0;

            PiercingShortName = "";

            PiercingLongName = "";

            Sock = "";

            SheathOn = false;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["length"] = Length;

            json["thickness"] = Thickness;

            json["knotMultiplier"] = KnotMultiplier;

            json["pierced"] = Pierced;

            json["piercingShortName"] = PiercingShortName;

            json["piercingLongName"] = PiercingLongName;

            json["sock"] = Sock;

            json["sheathOn"] = SheathOn;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Length = json.TryGetValue("length", out var length) ? (float)length : 0;

            Thickness = json.TryGetValue("thickness", out var thickness) ? (float)thickness : 0;

            KnotMultiplier = json.TryGetValue("knotMultiplier", out var knot) ? (float)knot : 1;

            Pierced = json.TryGetValue("pierced", out var pierced) ? (float)pierced : 0;

            PiercingShortName = json.TryGetValue("piercingShortName", out var shortName) ? (string)shortName : "";

            PiercingLongName = json.TryGetValue("piercingLongName", out var longName) ? (string)longName : "";

            Sock = json.TryGetValue("sock", out var sock) ? (string)sock : "";

            SheathOn = json.TryGetValue("sheathOn", out var sheathOn) && (bool)sheathOn;
        }

        public bool ChangeType(string typeName)
        {
            var types = BodyPartSlotRegistry.Get(SlotId).Types;

            foreach (var identifier in types.Ids)
                if (string.Equals(types.Get(identifier).GetString("type"), typeName, System.StringComparison.OrdinalIgnoreCase))
                {
                    Id = identifier;

                    return true;
                }

            return false;
        }

        public float GrowCock(float lengthDelta, bool bigCock)        {
            if (lengthDelta == 0)
                return lengthDelta;

            var threshold = 0f;

            if (lengthDelta > 0)
            {
                threshold = 24;

                if (bigCock)
                {
                    lengthDelta *= 1.5f;

                    threshold += 12;
                }

                if (Id.Path != "human")
                    threshold *= 2;

                if (Sock == "scarlet")
                    lengthDelta *= 1.5f;
                else if (Sock == "cobalt")
                    lengthDelta *= 0.5f;

                if (Length > threshold)
                    lengthDelta /= 4;
                else if (Length > threshold / 2)
                    lengthDelta /= 2;
            }
            else
            {
                threshold = 0;

                if (bigCock)
                {
                    lengthDelta *= 0.5f;

                    threshold += 12;
                }

                if (Id.Path != "human")
                    threshold += 12;

                if (Sock == "scarlet")
                    lengthDelta *= 0.5f;
                else if (Sock == "cobalt")
                    lengthDelta *= 1.5f;

                if (Length < threshold)
                    lengthDelta /= 3;
                else if (Length < threshold / 2)
                    lengthDelta /= 2;
            }

            Length += lengthDelta;

            if (Length < 1)
                Length = 1;

            if (Thickness > Length * 0.33f)
                Thickness = Length * 0.33f;

            return lengthDelta;
        }

        public int cockType => TypeIndex;

        public float ThickenCock(float increase, bool ignoreLimits = false)
        {
            var amountGrown = 0f;

            if (increase > 0)
                while (increase > 0)
                {
                    var temp = increase < 1 ? increase : 1f;

                    if (!ignoreLimits)
                    {
                        if (Thickness > 1 && Length < 12) temp /= 4;
                        if (Thickness > 1.5f && Length < 18) temp /= 5;
                        if (Thickness > 2 && Length < 24) temp /= 5;
                        if (Thickness > 3 && Length < 30) temp /= 5;
                        if (Thickness > Length * .15f) temp /= 3;
                        if (Thickness > Length * .20f) temp /= 3;
                        if (Thickness > Length * .30f) temp /= 5;
                        if (Thickness > 4) temp /= 2;
                        if (Thickness > 5) temp /= 2;
                        if (Thickness > 6) temp /= 2;
                        if (Thickness > 7) temp /= 2;
                    }

                    amountGrown += temp;

                    Thickness += temp;

                    increase--;
                }
            else if (increase < 0)
                while (increase < 0)
                {
                    var temp = -1f;

                    if (!ignoreLimits)
                    {
                        if (Thickness <= 1) temp /= 2;
                        if (Thickness < 2 && Length < 10) temp /= 2;
                        if (Thickness < 3 && Length < 18) temp /= 2;
                        if (Thickness < 4 && Length < 24) temp /= 2;
                        if (Thickness <= .5f) temp = 0;
                    }

                    amountGrown += temp;

                    Thickness += temp;

                    increase++;
                }

            return amountGrown;
        }

    }
}