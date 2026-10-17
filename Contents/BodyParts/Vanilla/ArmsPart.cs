using Hjson;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Internals;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{
    public class ArmsPart : BodyPart
    {
        public override Identifier SlotId => BodyPartSlots.Arms;

        public ClawsPart Claws = BodyPartSlotRegistry.New<ClawsPart>(BodyPartSlots.Claws);

        public string UpdateClaws(Identifier clawType)
        {
            var clawTone = "";

            var oldClawTone = Claws.Tone;

            var skinTone = Body.Get<SkinPart>().Tone;

            switch (clawType.Path)
            {
                case "dragon":
                    clawTone = "steel-gray";

                    break;

                case "salamander":
                    clawTone = "fiery-red";

                    break;

                case "lizard":
                    clawTone = skinTone switch
                    {
                        "red" => "reddish",

                        "green" => "greenish",

                        "white" => "light-gray",

                        "blue" => "bluish",

                        "black" => "dark-gray",

                        "purple" => "purplish",

                        "silver" => "silvery",

                        "pink" => "pink",

                        "orange" => "orangey",

                        "yellow" => "yellowish",

                        "desert-camo" => "pale-yellow",

                        _ => "gray",
                    };

                    break;

                case "coc:imp":
                    clawTone = skinTone;

                    break;
            }

            Claws.Id = clawType;

            Claws.Tone = clawTone;

            return oldClawTone;
        }

        public string Adj() => Id.Path switch
        {
            "harpy" => RandomUtils.Choice(["feathered", "feathery"]),

            "spider" => "chitinous",

            "bee" => "chitinous",

            "salamander" => RandomUtils.Choice(["scaled", "scaly"]),

            "wolf" => "furry",

            "cockatrice" => "scaled and feathered",

            "red_panda" => "fluffy",

            "ferret" => RandomUtils.Choice(["fluffy", "furry"]),

            "cat" => "furry",

            "dog" => "furry",

            "fox" => "furry",

            "dragon" => RandomUtils.Choice(["scaled", "scaly"]),

            "lizard" => RandomUtils.Choice(["scaled", "scaly"]),

            _ => ""
        };

        public string Phrase()
        {
            var adjective = Adj();

            return adjective.Length == 0 ? "arms" : adjective + " arms";
        }

        public override void Restore()
        {
            base.Restore();

            Claws.Restore();
        }

        public override void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            var clawsJson = new JsonObject();

            Claws.Save(clawsJson);

            json["claws"] = clawsJson;
        }

        public override void Load(JsonObject json)
        {
            var saved = json.TryGetValue("id", out var idValue) ? (string)idValue : "";

            Id = Identifier.TryParse(saved, out var parsed) && BodyPartSlotRegistry.Get(SlotId).Types.Has(parsed) ? parsed : BodyPartSlotRegistry.Get(SlotId).DefaultType;

            Claws.Load(json.TryGetValue("claws", out var clawsValue) && clawsValue is JsonObject clawsJson ? clawsJson : []);
        }
    }
}