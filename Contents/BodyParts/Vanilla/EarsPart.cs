using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class EarsPart : BodyPart
    {

        public int Pierced;

        public override Identifier SlotId => BodyPartSlots.Ears;

        public float Value;

        public int EarsPierced;

        public override void Restore()
        {
            base.Restore();

            Value = 0;

            EarsPierced = 0;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["value"] = Value;

            json["earsPierced"] = EarsPierced;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Value = json.TryGetValue("value", out var value) ? (float)value : 0;

            EarsPierced = json.TryGetValue("earsPierced", out var pierced) ? (int)(double)pierced : 0;
        }

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            var type = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("descript");

            if (type == "")
                type = "Human";

            return ("BodyParts.Descript.Ears." + type).Localized();
        }
    }
}