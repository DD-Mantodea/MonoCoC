using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class UnderBodyPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.UnderBody;

        public SkinPart Skin = BodyPartSlotRegistry.New<SkinPart>(BodyPartSlots.Skin);

        public override void Restore()
        {
            base.Restore();

            Skin.Restore();
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            var skinJson = new JsonObject();

            Skin.Save(skinJson);

            json["skin"] = skinJson;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Skin.Load(json.TryGetValue("skin", out var skinValue) && skinValue is JsonObject skinJson ? skinJson : []);
        }
    }
}