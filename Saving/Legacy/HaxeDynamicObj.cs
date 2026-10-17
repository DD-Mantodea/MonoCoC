using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Saving.Legacy
{
    [JsonConverter(typeof(HaxeDynamicObjConverter))]
    public class HaxeDynamicObj : HaxeObj
    {
        [JsonIgnore]
        public Dictionary<object, HaxeObj?> Data
        {
            get
            {
                Value ??= new Dictionary<object, HaxeObj?>();

                return (Dictionary<object, HaxeObj?>)Value;
            }
        }

        public HaxeObj? this[object index]
        {
            get => Data[index];

            set => Data[index] = value;
        }

        [JsonIgnore]
        public int Count => Data.Count;

        public static implicit operator HaxeDynamicObj(Dictionary<object, HaxeObj> dict) => new() { Value = dict };

        public static implicit operator HaxeDynamicObj(Dictionary<string, HaxeObj> dict) => new() { Value = dict.Select(kp => new KeyValuePair<object, HaxeObj>(kp.Key, kp.Value)).ToDictionary() };

        public static implicit operator HaxeDynamicObj(Dictionary<int, HaxeObj> dict) => new() { Value = dict.Select(kp => new KeyValuePair<object, HaxeObj>(kp.Key, kp.Value)).ToDictionary() };

        public static implicit operator HaxeDynamicObj(Dictionary<HaxeObj, HaxeObj> dict) => new() { Value = dict.Select(kp => new KeyValuePair<object, HaxeObj>(kp.Key, kp.Value)).ToDictionary() };
    }

    public class HaxeDynamicObjConverter : JsonConverter<HaxeDynamicObj>
    {
        public override HaxeDynamicObj? ReadJson(JsonReader reader, Type objectType, HaxeDynamicObj? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, HaxeDynamicObj? value, JsonSerializer serializer)
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