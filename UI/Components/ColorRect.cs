using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class ColorRect(Color color) : Component
    {
        private static Texture2D? _pixel;

        public Color Color = color;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(CoC.Instance.GraphicsDevice, 1, 1);

                _pixel.SetData([Color.White]);
            }

            spriteBatch.Draw(_pixel, Position.BuildRect(Width, Height), Color * Alpha);
        }
    }
}