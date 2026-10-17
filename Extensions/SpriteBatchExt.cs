using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoCoC.Extensions
{
    public static class SpriteBatchExt
    {
        private static Texture2D _pixel;
        public static Texture2D Pixel
        {
            get
            {
                if (_pixel == null)
                {
                    _pixel = new Texture2D(CoC.Instance.GraphicsDevice, 1, 1);

                    _pixel.SetData([Color.White]);
                }

                return _pixel;
            }
        }

        extension(SpriteBatch spriteBatch)
        {
            public void DrawRectangle(Rectangle rect, Color color) => spriteBatch.Draw(Pixel, rect, color);

            public void DrawBorder(Rectangle rect, int thickness, Color color)
            {
                if (rect.Width <= 0 || rect.Height <= 0)
                    return;

                var t = Math.Min(thickness, Math.Min(rect.Width, rect.Height));

                if (t <= 0)
                    return;

                spriteBatch.Draw(Pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), color);

                spriteBatch.Draw(Pixel, new Rectangle(rect.X, rect.Bottom - t, rect.Width, t), color);

                spriteBatch.Draw(Pixel, new Rectangle(rect.X, rect.Y + t, t, rect.Height - t * 2), color);

                spriteBatch.Draw(Pixel, new Rectangle(rect.Right - t, rect.Y + t, t, rect.Height - t * 2), color);
            }

            public void DrawPixel(
                Rectangle destinationRectangle,
                Rectangle? sourceRectangle,
                Color color,
                float rotation,
                Vector2 origin,
                SpriteEffects effects,
                float layerDepth
            )
            {
                spriteBatch.Draw(Pixel, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
            }
        }
    }
}
