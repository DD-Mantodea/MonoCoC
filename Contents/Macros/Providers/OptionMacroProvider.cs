using MonoCoC.Attributes;
using MonoCoC.Configuration;
using MonoCoC.Scripting;

namespace MonoCoC.Macros.Providers
{

    [Register<OptionMacroProvider>("coc:option")]
    public class OptionMacroProvider : IMacroProvider
    {
        public bool TryResolve(string name, out ScriptValue value)
        {
            var resolved = Resolve(name.ToLowerInvariant());

            if (resolved != null)
            {
                value = resolved.Value;

                return true;
            }

            value = default;

            return false;
        }

        public static ScriptValue? Resolve(string name) => name switch
        {
            "metric" => (ScriptValue?)ScriptValue.FromBool(GameSettings.Metric),

            "silly" => (ScriptValue?)ScriptValue.FromBool(OptionStore.Instance.Get("silly", 0) > 0),

            "guro" => (ScriptValue?)ScriptValue.FromBool(OptionStore.Instance.Get("gore", 0) > 0),

            "furry" => (ScriptValue?)ScriptValue.FromBool(OptionStore.Instance.Get("furry", 0) > 0),

            "nofur" => (ScriptValue?)ScriptValue.FromBool(OptionStore.Instance.Get("furry", 0) <= 0),

            _ => null
        };
    }
}