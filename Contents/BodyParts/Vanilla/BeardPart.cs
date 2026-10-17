using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class BeardPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Beard;

        public float Length = 0;

        public int Style;

        public const int Normal = 0;

        public const int Simple = 1;

        public const int Goatee = 2;

        public const int Mountainman = 3;

        public override void Restore()
        {
            base.Restore();

            Length = 0;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["length"] = Length;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Length = json.TryGetValue("length", out var length) ? (float)(double)length : 0;
        }

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            if (Length == 0f)
                return "BodyParts.Descript.Beard.Shaved".Localized();

            var length = Length switch
            {
                < 0.2f => "VeryShort",

                < 0.5f => "Short",

                < 1.5f => "Medium",

                < 3f => "Moderate",

                < 6f => "Long",

                _ => "Full"
            };

            var style = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("style");

            if (style == "")
                style = "Normal";

            var color = ("BodyParts.Color." + (Body?.Get<HairPart>()?.Color ?? "no").PascalKey()).LocalizedOr(Body?.Get<HairPart>()?.Color ?? "no");

            return "BodyParts.Descript.Beard.Format".Localized(("BodyParts.Descript.Beard.Length." + length).Localized(), color, ("BodyParts.Descript.Beard.Style." + style).Localized());
        }    }
}