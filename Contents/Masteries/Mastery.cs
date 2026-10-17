using Hjson;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Masteries
{
    public class Mastery
    {
        public Identifier Id;

        public int Level;

        public int XP;

        public bool Permed;

        public void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            json["level"] = Level;

            json["xp"] = XP;

            json["permed"] = Permed;
        }

        public void Load(JsonObject json)
        {
            Id = json.TryGetValue("id", out var idValue) && Identifier.TryParse((string)idValue, out var parsed) ? parsed : Id;

            Level = json.TryGetValue("level", out var level) ? (int)level : 0;

            XP = json.TryGetValue("xp", out var xp) ? (int)xp : 0;

            Permed = json.TryGetValue("permed", out var permed) && (bool)permed;
        }
    }
}