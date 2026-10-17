using MonoCoC.Scripting;

namespace MonoCoC.Macros
{

    public interface IMacroProvider
    {

        int Priority => 0;

        bool TryResolve(string name, out ScriptValue value);
    }
}