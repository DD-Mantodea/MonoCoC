using Microsoft.Xna.Framework;
using System;

namespace MonoCoC.Extensions
{
    public static class IntExt
    {
        extension(int i)
        {
            public int Clamp(int min, int max)
            {
                return Math.Clamp(i, min, max);
            }

            public Color ToColor()
            {
                if (i < 0 || i > 0xFFFFFF)
                    return Color.Transparent;

                byte r = (byte)((i >> 16) & 0xFF);

                byte g = (byte)((i >> 8) & 0xFF);

                byte b = (byte)(i & 0xFF);

                return new(r, g, b);
            }
        }
    }
}
