using Hjson;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MonoCoC.Contents.Flags
{
    public class GameFlags
    {
        public static readonly Dictionary<Identifier, FlagType> Types = [];

        public readonly Dictionary<Identifier, object> _values = [];

        public IReadOnlyDictionary<Identifier, object> Values => _values;

        public int Count => _values.Count;

        public static void Register(Identifier id, FlagType type)
        {
            if (Types.TryGetValue(id, out var existing))
            {
                if (existing != type)
                    throw new InvalidOperationException($"Flag '{id}' registered as {existing} and {type}");

                return;
            }

            Types[id] = type;
        }

        public static FlagType TypeOf<T>() => typeof(T) == typeof(int) ? FlagType.Int

            : typeof(T) == typeof(float) ? FlagType.Float

            : typeof(T) == typeof(bool) ? FlagType.Bool

            : typeof(T) == typeof(string) ? FlagType.String

            : throw new NotSupportedException($"Flag type '{typeof(T).Name}' is not supported");

        public int Get(Flag<int> flag)
        {
            Ensure(flag.Id, FlagType.Int);

            return _values.TryGetValue(flag.Id, out var value) ? ToInt(value) : 0;
        }

        public float Get(Flag<float> flag)
        {
            Ensure(flag.Id, FlagType.Float);

            return _values.TryGetValue(flag.Id, out var value) ? ToFloat(value) : 0f;
        }

        public bool Get(Flag<bool> flag)
        {
            Ensure(flag.Id, FlagType.Bool);

            return _values.TryGetValue(flag.Id, out var value) && value is bool result && result;
        }

        public string Get(Flag<string> flag)
        {
            Ensure(flag.Id, FlagType.String);

            return _values.TryGetValue(flag.Id, out var value) && value is string result ? result : "";
        }

        public void Set(Flag<int> flag, int value)
        {
            Ensure(flag.Id, FlagType.Int);

            if (value == 0)
            {
                _values.Remove(flag.Id);

                return;
            }

            _values[flag.Id] = value;
        }

        public void Set(Flag<float> flag, float value)
        {
            Ensure(flag.Id, FlagType.Float);

            if (value == 0f)
            {
                _values.Remove(flag.Id);

                return;
            }

            _values[flag.Id] = value;
        }

        public void Set(Flag<bool> flag, bool value)
        {
            Ensure(flag.Id, FlagType.Bool);

            _values[flag.Id] = value;
        }

        public void Set(Flag<string> flag, string value)
        {
            Ensure(flag.Id, FlagType.String);

            if (string.IsNullOrEmpty(value))
            {
                _values.Remove(flag.Id);

                return;
            }

            _values[flag.Id] = value;
        }

        public int Add(Flag<int> flag, int delta)
        {
            var value = Get(flag) + delta;

            Set(flag, value);

            return value;
        }

        public float Add(Flag<float> flag, float delta)
        {
            var value = Get(flag) + delta;

            Set(flag, value);

            return value;
        }

        public void SetRaw(Identifier id, object value)
        {
            if (value == null)
            {
                _values.Remove(id);

                return;
            }

            if (Types.TryGetValue(id, out var type))
                value = type switch
                {
                    FlagType.Int => Convert.ToInt32(value),

                    FlagType.Float => Convert.ToSingle(value),

                    FlagType.Bool => Convert.ToBoolean(value),

                    FlagType.String => Convert.ToString(value),

                    _ => value
                };

            if (IsDefault(value))
            {
                _values.Remove(id);

                return;
            }

            _values[id] = value;
        }

        public object GetRaw(Identifier id) => _values.TryGetValue(id, out var value) ? value : null;

        public bool Has<T>(Flag<T> flag) => _values.ContainsKey(flag.Id);

        public void Remove<T>(Flag<T> flag) => _values.Remove(flag.Id);

        public void Clear() => _values.Clear();

        public JsonObject Save()
        {
            var json = new JsonObject();

            foreach (var pair in _values)
                json[pair.Key.ToString()] = pair.Value switch
                {
                    int integer => (JsonValue)integer,

                    float single => (JsonValue)single,

                    bool boolean => (JsonValue)boolean,

                    string text => (JsonValue)text,

                    _ => (JsonValue)""
                };

            return json;
        }

        public void Load(JsonObject json)
        {
            RuntimeHelpers.RunClassConstructor(typeof(GameFlagIds).TypeHandle);

            _values.Clear();

            foreach (var pair in json)
            {
                var id = Identifier.Parse(pair.Key);

                if (!Types.TryGetValue(id, out var type))
                    continue;

                switch (type)
                {
                    case FlagType.Int:
                        if (pair.Value.JsonType == JsonType.Number)
                            SetRaw(id, (int)pair.Value);

                        break;

                    case FlagType.Float:
                        if (pair.Value.JsonType == JsonType.Number)
                            SetRaw(id, (float)pair.Value);

                        break;

                    case FlagType.Bool:
                        if (pair.Value.JsonType == JsonType.Boolean)
                            SetRaw(id, (bool)pair.Value);

                        break;

                    case FlagType.String:
                        if (pair.Value.JsonType == JsonType.String)
                            SetRaw(id, (string)pair.Value);

                        break;
                }
            }
        }

        public static void Ensure(Identifier id, FlagType expected)
        {
            if (!Types.TryGetValue(id, out var type))
                throw new InvalidOperationException($"Flag '{id}' is not registered");

            if (type != expected)
                throw new InvalidOperationException($"Flag '{id}' is {type}, used as {expected}");
        }

        public static bool IsDefault(object value) => value switch
        {
            int integer => integer == 0,

            float single => single == 0f,

            string text => text.Length == 0,

            _ => false
        };

        public static int ToInt(object value) => value switch
        {
            int integer => integer,

            double number => (int)number,

            _ => 0
        };

        public static float ToFloat(object value) => value switch
        {
            float single => single,

            double number => (float)number,

            _ => 0f
        };
    }
}