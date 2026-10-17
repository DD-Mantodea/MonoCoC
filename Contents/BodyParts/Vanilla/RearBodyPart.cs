using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class RearBodyPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.RearBody;

        public string Color = "";

        // 原版 bodyParts/RearBody.hx:canDye()：只有龙鬃能染色
        public bool CanDye() => Id == BodyPartTypes.DraconicMane;

        public bool HasDyeColor(string color) => Color == color;

        public void ApplyDye(string color) => Color = color;

        public override void Restore()
        {
            base.Restore();

            Color = "";
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["color"] = Color;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Color = json.TryGetValue("color", out var color) ? (string)color : "";
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => Id.Path == "none" ? "" : TypeName;    }
}