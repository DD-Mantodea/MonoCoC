using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Saving.Legacy
{
    [JsonConverter(typeof(HaxeArrayConverter))]
    public class HaxeArray : HaxeObj
    {
        [JsonIgnore]
        public List<HaxeObj?> Data
        {
            get
            {
                Value ??= new List<HaxeObj?>();

                return (List<HaxeObj?>)Value;
            }
        }

        public HaxeObj? this[int index]
        {
            get => Data[index];

            set => Data[index] = value;
        }

        [JsonIgnore]
        public int Count => Data.Count;

        public static implicit operator HaxeArray(List<HaxeObj> list) => new() { Value = list.Cast<HaxeObj?>().ToList() };
    }

    public class HaxeArrayConverter : JsonConverter<HaxeArray>
    {
        public override HaxeArray? ReadJson(JsonReader reader, Type objectType, HaxeArray? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, HaxeArray? value, JsonSerializer serializer)
        {
            switch (value.Data)
            {
                case List<HaxeObj?> l:
                    JArray.FromObject(l).WriteTo(writer, this);

                    break;

                default:
                    writer.WriteValue(value.Value);

                    break;
            }
        }
    }
}