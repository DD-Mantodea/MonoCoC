using Hjson;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Scenes
{

    public class SaveContent(JsonObject json)
    {
        public JsonObject Json { get; } = json;

        public virtual bool GetBool(string key, bool fallback = false) =>

            Json.TryGetValue(key, out var value) && value is not null && (bool)value;

        public virtual void SetBool(string key, bool value) => Json[key] = value;

        public virtual int GetInt(string key, int fallback = 0) =>

            Json.TryGetValue(key, out var value) && value is not null ? (int)value : fallback;

        public virtual void SetInt(string key, int value) => Json[key] = value;

        public virtual List<int> GetIntList(string key)
        {
            if (!Json.TryGetValue(key, out var value) || value is not JsonArray array)
                return [];

            var result = new List<int>();

            foreach (var item in array)
                result.Add((int)item);

            return result;
        }

        public virtual void SetIntList(string key, IEnumerable<int> values)
        {
            var array = new JsonArray();

            foreach (var value in values)
                array.Add(value);

            Json[key] = array;
        }

        public virtual float GetFloat(string key, float fallback = 0f) =>

            Json.TryGetValue(key, out var value) && value is not null ? (float)(double)value : fallback;

        public virtual void SetFloat(string key, float value) => Json[key] = value;

        public virtual string GetString(string key, string fallback = "") =>

            Json.TryGetValue(key, out var value) && value is not null ? (string)value : fallback;

        public virtual void SetString(string key, string value) => Json[key] = value;

        public T GetEnum<T>(string key, T fallback = default) where T : struct, Enum =>

            (T)Enum.ToObject(typeof(T), GetInt(key, Convert.ToInt32(fallback)));

        public void SetEnum<T>(string key, T value) where T : struct, Enum =>

            Json[key] = Convert.ToInt32(value);

        public virtual void Clear()
        {
            foreach (var key in Json.Keys.ToList())
                Json.Remove(key);
        }
    }

    public static class SceneSaveRegistry
    {
        public static readonly Dictionary<Identifier, SaveContent> Contents = [];

        public static SaveContent For(Identifier sceneId)
        {
            if (Contents.TryGetValue(sceneId, out var existing))
                return existing;

            var content = new SaveContent([]);

            Contents[sceneId] = content;

            return content;
        }

        public static JsonObject Save()
        {
            var root = new JsonObject();

            foreach (var (id, content) in Contents)
                root[id.ToString()] = content.Json;

            return root;
        }

        public static void Load(JsonObject json)
        {
            Contents.Clear();

            if (json == null)
                return;

            foreach (var pair in json)
            {
                if (!Identifier.TryParse(pair.Key, out var id))
                    continue;

                if (pair.Value is JsonObject value)
                    Contents[id] = new SaveContent(value);
            }
        }

        public static void Clear() => Contents.Clear();
    }
}