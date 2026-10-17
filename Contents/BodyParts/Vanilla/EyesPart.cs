using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class EyesPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Eyes;

        public int Count = 2;

        public override void Restore()
        {
            base.Restore();

            Count = 2;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["count"] = Count;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Count = json.TryGetValue("count", out var count) ? (int)(double)count : 2;
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => Id.Path == "human" ? "BodyParts.Descript.Eyes.Default".Localized() : "BodyParts.Descript.Eyes.Type".Localized(TypeName);
    }
}