using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class LowerBodyPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.LowerBody;

        public int LegCount = 2;

        public bool Incorporeal = false;

        public override void Restore()
        {
            base.Restore();

            LegCount = 2;

            Incorporeal = false;
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["legCount"] = LegCount;

            json["incorporeal"] = Incorporeal;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            LegCount = json.TryGetValue("legCount", out var legCount) ? (int)(double)legCount : 2;

            Incorporeal = json.TryGetValue("incorporeal", out var incorporeal) ? (bool)incorporeal : false;
        }

        public string Skin() => Id.Path switch
        {
            "red_panda" or "wolf" or "ferret" or "fox" or "kangaroo" or "echidna" or "bunny"

                or "pony" or "cloven_hoofed" or "dog" or "cat" or "raccoon" => "fur",

            "chitinous_spider_legs" or "drider" or "bee" => "chitin",

            "harpy" or "cockatrice" => "feathers",

            "hoofed" => "hide",

            "coc:naga" or "salamander" or "dragon" or "lizard" or "mermaid" => "scales",

            "root_legs" => "bark",

            "goo" => "goo",

            _ => "skin"
        };

        public string Legs()
        {
            var legs = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("legs");

            if (legs == "")
                legs = "Default";

            return ("BodyParts.Descript.LowerBody." + legs).Localized();
        }

        public string Leg()
        {
            var legs = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("legs");

            if (legs == "")
                legs = "Default";

            return ("BodyParts.Descript.LowerBody.Leg." + legs).Localized();
        }

        public string Feet()
        {
            var legs = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("legs");

            if (legs == "")
                legs = "Default";

            return ("BodyParts.Descript.LowerBody.Feet." + legs).Localized();
        }

        public string Foot()
        {
            var legs = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id).GetString("legs");

            if (legs == "")
                legs = "Default";

            return ("BodyParts.Descript.LowerBody.Foot." + legs).Localized();
        }

        public override string Descript(bool noAdj = false, bool noTone = false) => Legs();

    }
}