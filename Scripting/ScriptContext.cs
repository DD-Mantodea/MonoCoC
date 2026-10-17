using System;
using System.Collections.Generic;

namespace MonoCoC.Scripting
{
    public class ScriptContext
    {
        public string FontFamily = "pala";

        public string BoldFont = "palab";

        public string ItalicFont = "palai";

        public string BoldItalicFont = "palabi";

        public int FontSize = 20;

        private readonly Dictionary<string, ScriptValue> _tags = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyDictionary<string, ScriptValue> Tags => _tags;

        public ScriptContext Set(string name, ScriptValue value)
        {
            _tags[name] = value;

            return this;
        }

        public ScriptContext Set(string name, string value) => Set(name, ScriptValue.FromString(value));

        public ScriptContext Set(string name, bool value) => Set(name, ScriptValue.FromBool(value));

        public ScriptContext Set(string name, int value) => Set(name, ScriptValue.FromInt(value));

        public ScriptContext Set(string name, double value) => Set(name, ScriptValue.FromFloat(value));

        public Func<string, ScriptValue?> Resolver;

        public bool TryGet(string name, out ScriptValue value)
        {
            if (_tags.TryGetValue(name, out value))
                return true;

            var resolved = Resolver?.Invoke(name);

            if (resolved.HasValue)
            {
                value = resolved.Value;

                return true;
            }

            value = default;

            return false;
        }

        public void Clear() => _tags.Clear();
    }
}