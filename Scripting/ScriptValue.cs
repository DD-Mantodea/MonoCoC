using System;
using System.Globalization;

namespace MonoCoC.Scripting
{
    public enum ScriptKind
    {
        Null,

        String,

        Bool,

        Int,

        Float
    }

    public readonly struct ScriptValue
    {
        private readonly string _text;

        private readonly double _number;

        private readonly bool _flag;

        private ScriptValue(ScriptKind kind, string text, double number, bool flag)
        {
            Kind = kind;

            _text = text;

            _number = number;

            _flag = flag;
        }

        public ScriptKind Kind { get; }

        public static ScriptValue Null => new(ScriptKind.Null, null, 0, false);

        public static ScriptValue FromString(string value) => new(ScriptKind.String, value, 0, false);

        public static ScriptValue FromBool(bool value) => new(ScriptKind.Bool, null, 0, value);

        public static ScriptValue FromInt(int value) => new(ScriptKind.Int, null, value, false);

        public static ScriptValue FromFloat(double value) => new(ScriptKind.Float, null, value, false);

        public bool ToBool() => Kind switch
        {
            ScriptKind.Bool => _flag,

            ScriptKind.Int or ScriptKind.Float => _number != 0,

            ScriptKind.String => !string.IsNullOrEmpty(_text),

            _ => false
        };

        public double ToFloat() => Kind switch
        {
            ScriptKind.Int or ScriptKind.Float => _number,

            ScriptKind.Bool => _flag ? 1 : 0,

            ScriptKind.String => double.TryParse(_text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : 0,

            _ => 0
        };

        public string ToText() => Kind switch
        {
            ScriptKind.Bool => _flag ? "true" : "false",

            ScriptKind.Int => ((long)_number).ToString(CultureInfo.InvariantCulture),

            ScriptKind.Float => _number.ToString("0.####", CultureInfo.InvariantCulture),

            ScriptKind.String => _text ?? "",

            _ => ""
        };

        public bool SameAs(ScriptValue other) => Kind switch
        {
            ScriptKind.String => string.Equals(_text ?? "", other.ToText(), StringComparison.Ordinal),

            ScriptKind.Bool => _flag == other.ToBool(),

            ScriptKind.Int or ScriptKind.Float => Math.Abs(_number - other.ToFloat()) < 0.000001,

            _ => other.Kind == ScriptKind.Null
        };
    }
}