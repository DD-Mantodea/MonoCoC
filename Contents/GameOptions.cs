using MonoCoC.Configuration;
using System.Linq;

namespace MonoCoC.Contents
{
    public static class GameOptions
    {
        public static bool Silly => OptionStore.Instance.Get("coc:silly", 0) > 0;

        public static bool Realistic => OptionStore.Instance.Get("coc:realistic", 0) > 0;

        public static bool Survival => OptionStore.Instance.Get("coc:survival", 0) > 0;

        public static bool Metric => OptionStore.Instance.Get("coc:metric", 0) > 0;

        public static bool Hyper => OptionStore.Instance.Get("coc:hyper", 0) > 0;
    }

    public static class PlayerConditions
    {
        public static bool NoFur => !CoC.Instance.Player.HasFur();

        public static bool HasKnot =>
            CoC.Instance.Player.Cocks.Count > 0 &&
            CoC.Instance.Player.Cocks.Any(c => c.KnotMultiplier > 1f);
    }
}
