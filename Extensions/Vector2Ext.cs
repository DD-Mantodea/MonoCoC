using Microsoft.Xna.Framework;

namespace MonoCoC.Extensions
{
    public static class Vector2Ext
    {
        extension(Vector2 vec)
        {
            public Rectangle BuildRect(int width, int height) => new((int)vec.X, (int)vec.Y, width, height);

            public Vector2 Add(int width, int height) => vec + new Vector2(width, height);
        }
    }
}
