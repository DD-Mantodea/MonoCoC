using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class WingsPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Wings;

        public string Color = "";

        public string Color2 = "";

        // 原版 bodyParts/Wings.hx:canDye()/hasDyeColor()/applyDye()
        public bool CanDye() => Id == BodyPartTypes.Harpy || Id == BodyPartTypes.FeatheredLarge;

        public bool HasDyeColor(string color) => Color == color;

        public void ApplyDye(string color) => Color = color;

        // 原版 bodyParts/Wings.hx:canOil()/canOil2()：只有龙翼能上油，主色（膜）与第二色（骨）各一套
        public bool CanOil() => Id == BodyPartTypes.DraconicSmall || Id == BodyPartTypes.DraconicLarge;

        public bool CanOil2() => CanOil();

        public bool HasOilColor(string color) => Color == color;

        public bool HasOil2Color(string color) => Color2 == color;

        public void ApplyOil(string color) => Color = color;

        public void ApplyOil2(string color) => Color2 = color;

        public override void Restore()
        {
            base.Restore();

            Color = "";

            Color2 = "";
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["color"] = Color;

            json["color2"] = Color2;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Color = json.TryGetValue("color", out var color) ? (string)color : "";

            Color2 = json.TryGetValue("color2", out var color2) ? (string)color2 : "";
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => Id.Path == "none" ? "BodyParts.Descript.Wings.None".Localized() : "BodyParts.Descript.Wings.Type".Localized(TypeName);
    }
}