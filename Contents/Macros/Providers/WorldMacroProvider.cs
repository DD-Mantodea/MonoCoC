using MonoCoC.Attributes;
using MonoCoC.Scripting;
using MonoCoC.UI.Components.Views;

namespace MonoCoC.Macros.Providers
{

    [Register<WorldMacroProvider>("coc:world")]
    public class WorldMacroProvider : IMacroProvider
    {
        public bool TryResolve(string name, out ScriptValue value)
        {
            var time = GameView.Instance?.Time;

            var core = Core(name.ToLowerInvariant(), time);

            if (core != null)
            {
                value = core.Value;

                return true;
            }

            var extra = Extra(name, time);

            if (extra != null)
            {
                value = extra.Value;

                return true;
            }

            value = default;

            return false;
        }

        public static ScriptValue? Core(string name, Contents.World.Time? time) => name switch
        {
            "day" => (ScriptValue?)ScriptValue.FromString(time?.DayPart() ?? "day"),

            "timeofday" => (ScriptValue?)ScriptValue.FromString(time?.TimeOfDay() ?? "night"),

            "sun" => (ScriptValue?)ScriptValue.FromString(time?.Sun() ?? "moon"),

            "hour" => (ScriptValue?)ScriptValue.FromInt(time?.Hours ?? 6),

            "daycount" => (ScriptValue?)ScriptValue.FromInt(time?.Days ?? 1),

            "hours" => (ScriptValue?)ScriptValue.FromInt(time?.Hours ?? 6),

            _ => null
        };

        public static ScriptValue? Extra(string name, Contents.World.Time? time) => name switch
        {
            "sun" => PlayerTextMacros.Str(time != null && time.Hours < 21 ? "sun" : "moon"),

            "day" => PlayerTextMacros.Str(time == null ? "day" : time.Hours < 12 ? "morning" : time.Hours < 19 ? "day" : "evening"),

            "timeofday" => PlayerTextMacros.Str(time == null ? "day"

                : time.Hours < 12 ? "morning"

                : time.Hours < 17 ? "afternoon"

                : time.Hours < 21 ? "evening"

                : "night"),

            _ => null
        };
    }
}