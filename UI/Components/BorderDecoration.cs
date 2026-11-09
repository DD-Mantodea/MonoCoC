using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoCoC.UI.Components
{
    public sealed class BorderDecoration : DecorationComponent
    {
        public int Thickness = 1;

        public int Inset;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var color = PrimaryColor;

            if (color == Color.Transparent || Thickness <= 0)
                return;

            var bounds = Rectangle;

            bounds.Inflate(-Inset, -Inset);

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var thickness = System.Math.Min(Thickness, System.Math.Min(bounds.Width, bounds.Height) / 2);

            Fill(spriteBatch, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
            Fill(spriteBatch, new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
            Fill(spriteBatch, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
            Fill(spriteBatch, new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
        }
    }
}
