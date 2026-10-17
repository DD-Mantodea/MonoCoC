using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class HairPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Hair;

        public string Color = "no";

        public string Adj = "";

        public string FlowerColor = "";

        public float Length;

        public override void Restore()
        {
            base.Restore();

            Color = "no";

            Adj = "";

            FlowerColor = "";

            Length = 0;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["color"] = Color;

            json["adj"] = Adj;

            json["flowerColor"] = FlowerColor;

            json["length"] = Length;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Color = json.TryGetValue("color", out var color) ? (string)color : "no";

            Adj = json.TryGetValue("adj", out var adj) ? (string)adj : "";

            FlowerColor = json.TryGetValue("flowerColor", out var flowerColor) ? (string)flowerColor : "";

            Length = json.TryGetValue("length", out var length) ? (float)(double)length : 0;
        }

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            if (Length == 0f)
                return "BodyParts.Descript.Hair.Bald".Localized();

            var length = Length switch
            {
                < 1f => "VeryShort",

                < 3f => "Short",

                < 6f => "Shaggy",

                < 10f => "Moderate",

                < 16f => "Long",

                < 26f => "VeryLong",

                < 40f => "AssLength",

                _ => "Floor"
            };

            var type = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("descript");

            if (type == "")
                type = "Default";

            return ("BodyParts.Descript.Hair.Length." + length).Localized() + ("BodyParts.Descript.Hair.Type." + type).Localized(("BodyParts.Color." + Color.PascalKey()).LocalizedOr(Color));
        }

        public string HairShort()
        {
            var type = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("descript");

            if (type == "")
                type = "Default";

            return ("BodyParts.Descript.Hair.Short." + type).Localized();
        }

        public string HairOrFur()
        {
            var skin = Body?.Get<SkinPart>();

            return skin != null && skin.HasFur ? "BodyParts.Descript.Hair.Short.Fur".Localized() : "BodyParts.Descript.Hair.Short.Hair".Localized();
        }

        public string HairOrFurColor()
        {
            var skin = Body?.Get<SkinPart>();

            return skin != null && skin.IsFluffy ? ("BodyParts.Color." + skin.FurColor.PascalKey()).LocalizedOr(skin.FurColor) : ("BodyParts.Color." + Color.PascalKey()).LocalizedOr(Color);
        }

    }
}