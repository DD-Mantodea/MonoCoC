using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class TailPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Tail;

        public float Venom;

        public float Recharge = 5;

        public override void Restore()
        {
            base.Restore();

            Venom = 0;

            Recharge = 5;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["venom"] = Venom;

            json["recharge"] = Recharge;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Venom = json.TryGetValue("venom", out var venom) ? (float)(double)venom : 0;

            Recharge = json.TryGetValue("recharge", out var recharge) ? (float)(double)recharge : 5;
        }

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            if (Id.Path == "none")
                return "";

            if (BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetBool("kitsune") && Venom >= 1f)
            {
                var key = Venom switch
                {
                    <= 1f => "One",

                    <= 2f => "Two",

                    <= 3f => "Three",

                    <= 4f => "Four",

                    <= 5f => "Five",

                    _ => "Many"
                };

                return ("BodyParts.Descript.Tail.Kitsune." + key).Localized();
            }

            return "BodyParts.Descript.Tail.Type".Localized(TypeName);
        }    }
}