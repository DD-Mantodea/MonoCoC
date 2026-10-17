using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class ClawsPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Claws;

        public string Tone = "";

        public override void Restore()
        {
            base.Restore();

            Tone = "";
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["tone"] = Tone;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Tone = json.TryGetValue("tone", out var tone) ? (string)tone : "";
        }

        public string ClawsDescript(bool plural = true)
        {
            var tone = Tone == "" ? "BodyParts.Descript.Claws.ToneNone".Localized() : "BodyParts.Descript.Claws.Tone".Localized(Tone);

            var form = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("form");

            var model = form switch
            {
                "dragon" => "Dragon",

                "coc:imp" => "Imp",

                "cat" => "Cat",

                "short" => "Short",

                _ => null
            };

            if (model == null)
                return ("BodyParts.Descript.Claws." + (plural ? "Nails" : "Nail")).Localized();

            return ("BodyParts.Descript.Claws." + model + (plural ? "Plural" : "")).Localized(tone);
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => ClawsDescript();    }
}