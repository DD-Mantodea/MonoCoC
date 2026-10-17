using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class FacePart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Face;

        public int LipPierced;

        public int EyebrowPierced;

        public int NosePierced;

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["lipPierced"] = LipPierced;

            json["eyebrowPierced"] = EyebrowPierced;

            json["nosePierced"] = NosePierced;

        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            LipPierced = json.TryGetValue("lipPierced", out var lip) ? (int)(double)lip : 0;

            EyebrowPierced = json.TryGetValue("eyebrowPierced", out var eyebrow) ? (int)(double)eyebrow : 0;

            NosePierced = json.TryGetValue("nosePierced", out var nose) ? (int)(double)nose : 0;

        }

        public override string Descript(bool noAdj = false, bool noTone = false)
        {
            if (Id.Path == "human")
                return "BodyParts.Descript.Face.Face".Localized();

            var type = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id);

            var nounKind = type.GetString("noun");

            if (nounKind == "")
                nounKind = type.GetBool("muzzle") ? "All" : "Face";

            var noun = nounKind switch
            {
                "All" => "BodyParts.Descript.Face.Noun." + (Random.Shared.Next(3) switch
                {
                    0 => "Muzzle",

                    1 => "Snout",

                    _ => "Face"
                }),

                "MuzzleFace" => Random.Shared.Next(2) == 0 ? "BodyParts.Descript.Face.Noun.Muzzle" : "BodyParts.Descript.Face.Noun.Face",

                "SnoutFace" => Random.Shared.Next(4) == 0 ? "BodyParts.Descript.Face.Noun.Snout" : "BodyParts.Descript.Face.Noun.Face",

                _ => "BodyParts.Descript.Face.Noun.Face"
            };

            var adj = type.GetString("adj");

            if (adj != "" && Random.Shared.Next(3) == 0)
                return ("BodyParts.Descript.Face.Adj." + adj.PascalKey()).Localized() + noun.Localized();

            return noun.Localized();
        }
    }
}