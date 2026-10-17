using MonoCoC.Contents.Creatures;
using MonoCoC.Scripting;

namespace MonoCoC.Macros
{
    internal static class PlayerTextMacros
    {
        public static ScriptValue? Str(string value) => ScriptValue.FromString(value);

        public static string CockOrClit(Player player)
        {
            if (player.HasCock())
                return player.CockDescript(0);

            return player.HasVagina() ? player.ClitDescript() : "";
        }

        public static string WalkWord(Player player, bool progressive)
        {
            if (player.HasTailInsteadOfLegs())
                return progressive ? "slithering" : "slither";

            if (player.IsGoo())
                return progressive ? "sliding" : "slide";

            if (player.IsDrider())
                return progressive ? "skittering" : "skitter";

            return progressive ? "walking" : "walk";
        }
    }
}