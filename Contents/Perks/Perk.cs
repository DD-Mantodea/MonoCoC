using Hjson;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Perks
{
    public class Perk
    {
        public Identifier Id;

        public float[] Values = new float[4];

        public void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            var values = new JsonArray();

            foreach (var value in Values)
                values.Add(value);

            json["values"] = values;
        }

        public void Load(JsonObject json)
        {
            Id = json.TryGetValue("id", out var idValue) && Identifier.TryParse((string)idValue, out var parsed) ? parsed : Id;

            Values = [0f, 0f, 0f, 0f];

            if (json.TryGetValue("values", out var valuesValue) && valuesValue is JsonArray values)
                for (var index = 0; index < Math.Min(Values.Length, values.Count); index++)
                    Values[index] = (float)values[index];
        }
    }
}