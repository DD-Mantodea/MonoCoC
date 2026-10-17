using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MonoCoC.Saving.Legacy
{
    public class SaveLoader
    {
        public static List<string> StrCache = [];

        public static JObject Load(string path)
        {
            var res = (JsonConvert.DeserializeObject(LoadToJson(path)) as JObject)["data"] as JObject;

            var flags = res["flags"] as JObject;

            var items = new List<JProperty>(flags.Properties());

            foreach (var p in items)
            {
                var index = int.Parse(p.Name);

                var value = p.Value;

                flags.Remove(p.Name);

                // 原版 mod 可能带有当前版本未定义的 flag 编号；跳过而不是抛异常
                if (LegacyKFlags.FLAGS.TryGetValue(index, out var legacyFlag))
                    flags.Add(legacyFlag.Name, value);
            }

            return res;
        }

        public static JObject LoadFromJson(string path)
        {
            return JsonConvert.DeserializeObject(File.ReadAllText(path)) as JObject;
        }

        private static string LoadToJson(string path)
        {
            var data = File.ReadAllBytes(path);

            var str = Encoding.UTF8.GetString(data[2..]);

            var res = new HaxeDeserializer(str).Deserialize();

            return JsonConvert.SerializeObject(res);
        }

        public static void SaveCocFile(string path, JObject save)
        {
            var str = ExportToCoc(save.DeepClone() as JObject);

            var data = Encoding.UTF8.GetBytes(str);

            short size = (short)data.Length;

            data = [.. BitConverter.GetBytes(size), .. data];

            File.WriteAllBytes(path, data);
        }

        private static string ExportToCoc(JObject save)
        {
            StrCache.Clear();

            var flags = save["flags"] as JObject;

            var items = new List<JProperty>(flags.Properties());

            foreach (var p in items)
            {
                var index = LegacyKFlags.FLAGS.First(pair => pair.Value.Name == p.Name).Key;

                var value = p.Value;

                flags.Remove(p.Name);

                flags.Add(index.ToString(), value);
            }

            var sb = new StringBuilder();

            sb.Append('o');

            sb.Append(ExportString("data"));

            sb.Append(ExportObject(save));

            sb.Append('g');

            return sb.ToString();
        }

        private static Regex _strRegex = new("([^a-zA-Z0-9.\\-*_!'()~])");

        private static string ExportString(string str)
        {
            var sb = new StringBuilder();

            str = _strRegex.Replace(str, m =>
            {
                return Uri.EscapeDataString(m.Value);
            });

            var strIndex = StrCache.IndexOf(str);

            if (strIndex == -1)
            {
                sb.Append($"y{str.Length}:{str}");

                StrCache.Add(str);
            }
            else sb.Append($"R{strIndex}");

            return sb.ToString();
        }

        private static string ExportValue(JValue value)
        {
            var sb = new StringBuilder();

            switch (value.Value)
            {
                case double d:
                    switch (d)
                    {
                        case double.NaN:
                            sb.Append('k');

                            break;

                        case double.PositiveInfinity:
                            sb.Append('p');

                            break;

                        case double.NegativeInfinity:
                            sb.Append('m');

                            break;

                        default:
                            sb.Append($"d{d}");

                            break;
                    }

                    break;

                case long i:
                    sb.Append(i == 0 ? 'z' : $"i{i}");

                    break;

                case string str:
                    sb.Append(ExportString(str));

                    break;

                case bool b:
                    sb.Append(b ? 't' : 'f');

                    break;

                case null:
                    sb.Append('n');

                    break;
            }

            return sb.ToString();
        }

        private static string ExportObject(JObject obj)
        {
            var sb = new StringBuilder();

            var properties = obj.Properties();

            if (properties.Any(v => v.Name == "HaxeClassName"))
            {
                sb.Append('c');

                sb.Append(ExportString((obj["HaxeClassName"] as JValue).Value as string));

                foreach (var item in properties)
                    if (item.Name != "HaxeClassName")
                        sb.Append(ExportProp(item));
            }
            else
            {
                sb.Append('o');

                foreach (var item in properties)
                    sb.Append(ExportProp(item));
            }

            sb.Append('g');

            return sb.ToString();
        }

        private static string ExportArray(JArray arr)
        {
            var sb = new StringBuilder();

            sb.Append('a');

            foreach (var item in arr)
                switch (item)
                {
                    case JProperty prop:
                        sb.Append(ExportProp(prop));

                        break;

                    case JObject obj:
                        sb.Append(ExportObject(obj));

                        break;

                    case JValue value:
                        sb.Append(ExportValue(value));

                        break;
                }

            sb.Append('h');

            return sb.ToString();
        }

        private static string ExportProp(JProperty prop)
        {
            var sb = new StringBuilder();

            var name = prop.Name;

            sb.Append(ExportString(name));

            var value = prop.Value;

            switch (value)
            {
                case JObject obj:
                    sb.Append(ExportObject(obj));

                    break;

                case JArray array:
                    sb.Append(ExportArray(array));

                    break;

                case JValue val:
                    sb.Append(ExportValue(val));

                    break;
            }

            return sb.ToString();
        }
    }
}