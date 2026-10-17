using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MonoCoC.Saving.Legacy
{
    [JsonConverter(typeof(HaxeClassConverter))]
    public class HaxeClass : HaxeDynamicObj
    {
        public string HaxeClassName
        {
            get => Data["HaxeClassName"];

            set => Data["HaxeClassName"] = value;
        }
    }

    public class HaxeClassConverter : JsonConverter<HaxeClass>
    {
        public override HaxeClass? ReadJson(JsonReader reader, Type objectType, HaxeClass? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, HaxeClass? value, JsonSerializer serializer)
        {
            switch (value.Data)
            {
                case Dictionary<object, HaxeObj?> d:
                    JObject.FromObject(d).WriteTo(writer, this);

                    break;

                default:
                    writer.WriteValue(value.Value);

                    break;
            }
        }
    }
}