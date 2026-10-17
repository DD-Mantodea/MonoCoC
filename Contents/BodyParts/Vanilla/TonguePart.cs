using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class TonguePart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Tongue;

        public int Pierced;

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["pierced"] = Pierced;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Pierced = json.TryGetValue("pierced", out var pierced) ? (int)(double)pierced : 0;
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => Id.Path == "human" ? "BodyParts.Descript.Tongue.Default".Localized() : "BodyParts.Descript.Tongue.Type".Localized(TypeName);
    }
}