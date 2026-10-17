using Hjson;
using MonoCoC.Extensions;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class SkinPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Skin;

        public string Tone = "albino";

        public string Adj = "";

        public string FurColor = "no";

        public string DescKey = "Skin.Desc.skin";

        public string Desc => DescKey.Localized();

        protected override void OnTypeChanged()
        {
            if (BodyPartSlotRegistry.Registry.Has(SlotId))
                DescKey = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).DescKey;
        }

        public override void Restore()
        {
            base.Restore();

            Tone = "albino";

            Adj = "";

            FurColor = "no";

            DescKey = "Skin.Desc.skin";
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["tone"] = Tone;

            json["adj"] = Adj;

            json["furColor"] = FurColor;

            json["descKey"] = DescKey;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Tone = json.TryGetValue("tone", out var tone) ? (string)tone : "albino";

            Adj = json.TryGetValue("adj", out var adj) ? (string)adj : "";

            FurColor = json.TryGetValue("furColor", out var furColor) ? (string)furColor : "no";

            DescKey = json.TryGetValue("descKey", out var descKey) ? (string)descKey : DescKey;
        }

        public new BodyPartType Type => BodyPartSlotRegistry.Get(SlotId).Types.Get(Id);

        public string Kind => Type.GetString("skinKind");

        public bool HasFur => Type.GetBool("fur");

        public bool HasWool => Type.GetBool("wool");

        public bool HasFeathers => Type.GetBool("feathers");

        public bool HasScales => Type.GetBool("scales");

        public bool IsFurry => Type.GetBool("furry");

        public bool IsFluffy => Type.GetBool("fluffy");

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            var noun = IsFluffy ? "BodyParts.Descript.Skin.Fluffy".Localized() : Desc;

            var tone = ("BodyParts.Color." + Tone.PascalKey()).LocalizedOr(Tone);

            if (!noAdj && Adj != "" && !noTone && Tone != "rough gray")
                return "BodyParts.Descript.Skin.WithAdj".Localized(("BodyParts.Adj." + Adj.PascalKey()).LocalizedOr(Adj), tone, noun);

            if (!noTone)
                return "BodyParts.Descript.Skin.Plain".Localized(tone, noun);

            return noun;
        }

        public string SkinFurScales()
        {
            var color = IsFluffy ? FurColor : Tone;

            var tone = ("BodyParts.Color." + color.PascalKey()).LocalizedOr(color);

            if (Adj != "")
                return "BodyParts.Descript.SkinFurScales.WithAdj".Localized(("BodyParts.Adj." + Adj.PascalKey()).LocalizedOr(Adj), tone, Desc);

            return "BodyParts.Descript.SkinFurScales.Plain".Localized(tone, Desc);
        }    }
}