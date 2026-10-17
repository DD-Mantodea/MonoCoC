using Newtonsoft.Json;
using System;

namespace MonoCoC.Saving.Legacy
{
    [JsonConverter(typeof(HaxeObjConverter))]
    public class HaxeObj
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object? Value = null;

        public static implicit operator string(HaxeObj o) => o.Value as string ?? "";

        public static implicit operator HaxeObj(string s) => new() { Value = s };

        public static implicit operator HaxeObj(bool b) => new() { Value = b };

        public static implicit operator int(HaxeObj o) => o.Value is int i ? i : 0;

        public static implicit operator HaxeObj(int i) => new() { Value = i };

        public static implicit operator HaxeObj(float f) => new() { Value = f };

        public static implicit operator HaxeObj(double d) => new() { Value = d };

        public static implicit operator HaxeObj(byte[] bytes) => new() { Value = bytes };
    }

    public class HaxeObjConverter : JsonConverter<HaxeObj>
    {
        public override HaxeObj? ReadJson(JsonReader reader, Type objectType, HaxeObj? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, HaxeObj? value, JsonSerializer serializer)
        {
            switch (value.Value)
            {
                case double d:
                    writer.WriteValue(d);

                    break;

                case int i:
                    writer.WriteValue(i);

                    break;

                case string s:
                    writer.WriteValue(s);

                    break;

                default:
                    writer.WriteValue(value.Value);

                    break;
            }
        }
    }
}