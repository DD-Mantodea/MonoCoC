using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class NeckPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Neck;

        public float Len = 2;

        public bool Pos = false;

        public string Color = "";

        // 原版 bodyParts/Neck.hx:canDye()：只有鸡蛇脖子能染色
        public bool CanDye() => Id == BodyPartTypes.Cockatrice;

        public bool HasDyeColor(string color) => Color == color;

        public void ApplyDye(string color) => Color = color;

        public override void Restore()
        {
            base.Restore();

            Len = 2;

            Pos = false;

            Color = "";
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["len"] = Len;

            json["pos"] = Pos;

            json["color"] = Color;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Len = json.TryGetValue("len", out var len) ? (float)(double)len : 2;

            Pos = json.TryGetValue("pos", out var pos) ? (bool)pos : false;

            Color = json.TryGetValue("color", out var color) ? (string)color : "";
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => "BodyParts.Descript.Neck.Type".Localized(TypeName);
    }
}