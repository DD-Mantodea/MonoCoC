using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class GradientPanel : Component
    {
        public Color FromColor = Color.Transparent;

        public Color ToColor = Color.Transparent;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Width <= 0 || Height <= 0)
                return;

            var rectangle = Rectangle;

            for (var y = 0; y < Height; y++)
            {
                var color = Color.Lerp(FromColor, ToColor, Height <= 1 ? 0 : y / (float)(Height - 1));

                spriteBatch.DrawRectangle(
                    new Rectangle(rectangle.X, rectangle.Y + y, rectangle.Width, 1),
                    color * LayoutData.Alpha);
            }
        }
    }
}
