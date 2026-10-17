using Hjson;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.StatusEffects
{
    public class StatusEffect
    {
        public Identifier Id;

        public float[] Values = new float[4];

        public float Duration;

        public JsonObject Data = [];

        public EffectDefinition? Definition => EffectManager.Get(Id);

        public virtual void Save(JsonObject json)
        {
            json["id"] = Id.ToString();

            var values = new JsonArray();

            foreach (var value in Values)
                values.Add(value);

            json["values"] = values;

            json["duration"] = Duration;

              json["data"] = Data;
        }

        public virtual void Load(JsonObject json)
        {
            Id = json.TryGetValue("id", out var idValue) && Identifier.TryParse((string)idValue, out var parsed) ? parsed : Id;

            Values = [0f, 0f, 0f, 0f];

            if (json.TryGetValue("values", out var valuesValue) && valuesValue is JsonArray values)
                for (var index = 0; index < Math.Min(Values.Length, values.Count); index++)
                    Values[index] = (float)values[index];

            Duration = json.TryGetValue("duration", out var duration) ? (float)duration : 0;

              Data = json.TryGetValue("data", out var data) && data is JsonObject dataObject ? dataObject : [];
        }
    }
}