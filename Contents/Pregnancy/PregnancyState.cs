using Hjson;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Pregnancy
{
    public class PregnancyState
    {
        public bool allowHerm { get; set; }

        public Identifier Type = new("coc", "none");

        public int Incubation;

        public int Event;

        public int Count;

        public Identifier EggType = new("coc", "none");

        public int Notice;

        public bool IsActive => Type.Path != "none" && Incubation > 0;

        public virtual void Save(JsonObject json)
        {
            json["type"] = Type.ToString();

            json["incubation"] = Incubation;

            json["count"] = Count;

            json["event"] = Event;

            json["eggType"] = EggType.ToString();
        }

        public virtual void Load(JsonObject json)
        {
            Type = json.TryGetValue("type", out var typeValue) && Identifier.TryParse((string)typeValue, out var parsed) ? parsed : new Identifier("coc", "none");

            Incubation = json.TryGetValue("incubation", out var incubation) ? (int)(double)incubation : 0;

            Count = json.TryGetValue("count", out var count) ? (int)(double)count : 0;

            EggType = json.TryGetValue("eggType", out var eggValue) && Identifier.TryParse((string)eggValue, out var egg) ? egg : new Identifier("coc", "none");
        }
    }
}