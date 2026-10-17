using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class HornsPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Horns;

        public float Value = 0;

        public override void Restore()
        {
            base.Restore();

            Value = 0;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["value"] = Value;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Value = json.TryGetValue("value", out var value) ? (float)(double)value : 0;
        }

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            if (Id.Path == "none")
                return "";

            if (Id.Path == "wooden" && Value == 1f)
                return "BodyParts.Descript.Horns.Antlers".Localized();

            return "BodyParts.Descript.Horns.Type".Localized(TypeName);
        }    }
}