using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.UI.Components
{
    public sealed class DividerDecoration : DecorationComponent
    {
        public bool Vertical;

        public bool Double;

        public bool Ornament = true;

        public string OrnamentText = "✦";

        public string FontRole = "button";

        public int Thickness = 1;

        public int Gap = 2;

        public int OrnamentPadding = 6;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var color = PrimaryColor;

            if (color == Color.Transparent || Thickness <= 0)
                return;

            var font = UIFonts.Get(FontRole);
            var drawOrnament = Ornament && OrnamentText.Length > 0;
            var bounds = drawOrnament ? font.TextBounds(OrnamentText, Vector2.One) : default;
            var textPosition = drawOrnament
                ? new Vector2(
                    Rectangle.Center.X - (bounds.X + bounds.X2) * 0.5f,
                    Rectangle.Center.Y - (bounds.Y + bounds.Y2) * 0.5f)
                : Vector2.Zero;

            var inkLeft = (int)MathF.Round(textPosition.X + bounds.X);
            var inkRight = (int)MathF.Round(textPosition.X + bounds.X2);
            var inkTop = (int)MathF.Round(textPosition.Y + bounds.Y);
            var inkBottom = (int)MathF.Round(textPosition.Y + bounds.Y2);

            if (Vertical)
            {
                var center = Rectangle.X + (Rectangle.Width - Thickness) / 2;

                var topEnd = drawOrnament ? inkTop - OrnamentPadding : Rectangle.Center.Y;
                var bottomStart = drawOrnament ? inkBottom + OrnamentPadding : Rectangle.Center.Y;

                if (drawOrnament)
                {
                    DrawVerticalSegment(spriteBatch, center, Rectangle.Y, topEnd, color);

                    if (Double)
                        DrawVerticalSegment(spriteBatch, center + Gap + Thickness, Rectangle.Y, topEnd, SecondaryOrPrimary);

                    DrawVerticalSegment(spriteBatch, center, bottomStart, Rectangle.Bottom, color);

                    if (Double)
                        DrawVerticalSegment(spriteBatch, center + Gap + Thickness, bottomStart, Rectangle.Bottom, SecondaryOrPrimary);
                }
                else
                {
                    DrawVerticalSegment(spriteBatch, center, Rectangle.Y, Rectangle.Bottom, color);

                    if (Double)
                        DrawVerticalSegment(spriteBatch, center + Gap + Thickness, Rectangle.Y, Rectangle.Bottom, SecondaryOrPrimary);
                }
            }
            else
            {
                var center = Rectangle.Y + (Rectangle.Height - Thickness) / 2;

                var leftEnd = drawOrnament ? inkLeft - OrnamentPadding - 1 : Rectangle.Center.X;
                var rightStart = drawOrnament ? inkRight + OrnamentPadding - 1 : Rectangle.Center.X;

                if (drawOrnament)
                {
                    DrawHorizontalSegment(spriteBatch, center, Rectangle.X, leftEnd, color);

                    if (Double)
                        DrawHorizontalSegment(spriteBatch, center + Gap + Thickness, Rectangle.X, leftEnd, SecondaryOrPrimary);

                    DrawHorizontalSegment(spriteBatch, center, rightStart, Rectangle.Right, color);

                    if (Double)
                        DrawHorizontalSegment(spriteBatch, center + Gap + Thickness, rightStart, Rectangle.Right, SecondaryOrPrimary);
                }
                else
                {
                    DrawHorizontalSegment(spriteBatch, center, Rectangle.X, Rectangle.Right, color);

                    if (Double)
                        DrawHorizontalSegment(spriteBatch, center + Gap + Thickness, Rectangle.X, Rectangle.Right, SecondaryOrPrimary);
                }
            }

            if (!drawOrnament)
                return;

            font.DrawText(spriteBatch, OrnamentText, textPosition, PrimaryColor);
        }

        private Color SecondaryOrPrimary =>
            SecondaryColor == Color.Transparent ? PrimaryColor : SecondaryColor;

        private void DrawHorizontalSegment(SpriteBatch spriteBatch, int y, int startX, int endX, Color color)
        {
            if (endX <= startX)
                return;

            Fill(spriteBatch, new Rectangle(startX, y, endX - startX, Thickness), color);
        }

        private void DrawVerticalSegment(SpriteBatch spriteBatch, int x, int startY, int endY, Color color)
        {
            if (endY <= startY)
                return;

            Fill(spriteBatch, new Rectangle(x, startY, Thickness, endY - startY), color);
        }
    }
}
