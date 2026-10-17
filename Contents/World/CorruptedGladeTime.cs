using MonoCoC.Contents.Flags;
using System;

namespace MonoCoC.Contents.World
{
    public static class CorruptedGladeTime
    {
        public static void OnHourPassed(int hour)
        {
            var coc = CoC.Instance;

            if (coc == null)
                return;

            var flags = coc.Flags;

            var destroyed = flags.Get(GameFlagIds.CorruptedGladesDestroyed);

            if (destroyed > 1 && destroyed < 100)
            {
                if (flags.Get(GameFlagIds.AmilyDestroyingCorruptedGlades) > 0 && Random.Shared.Next(6) == 0)
                    destroyed += 1;

                if (flags.Get(GameFlagIds.KihaDestroyingCorruptedGlades) > 0 && Random.Shared.Next(4) == 0)
                    destroyed += 1;

                var time = UI.Components.Views.GameView.Instance?.Time;

                if (time != null &&
                    time.Days % (3 + destroyed / 15) == 0 &&

                    hour == 0)

                    destroyed -= 1;
            }

            if (destroyed >= 100)
            {
                if (flags.Get(GameFlagIds.AmilyDestroyingCorruptedGlades) > 0)
                    flags.Set(GameFlagIds.AmilyDestroyingCorruptedGlades, 0);

                if (flags.Get(GameFlagIds.KihaDestroyingCorruptedGlades) > 0)
                    flags.Set(GameFlagIds.KihaDestroyingCorruptedGlades, 0);
            }

            flags.Set(GameFlagIds.CorruptedGladesDestroyed, destroyed);
        }
    }
}