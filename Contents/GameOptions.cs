using System.Linq;

namespace MonoCoC.Contents
{
    public static class PlayerConditions
    {
        public static bool NoFur => !CoC.Instance.Player.HasFur();

        public static bool HasKnot =>
            CoC.Instance.Player.Cocks.Count > 0 &&
            CoC.Instance.Player.Cocks.Any(c => c.KnotMultiplier > 1f);
    }
}
