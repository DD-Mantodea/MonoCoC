using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.UI.Components
{
    public sealed class SymbolDecoration : DecorationComponent
    {
        public string Text = "";

        public string TextureId = "";

        public string FontRole = "button";

        public int SymbolSize;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var color = PrimaryColor;

            var texture = TextureRegistry.Instance.Resolve(TextureId);

            if (texture is not null)
            {
                var size = SymbolSize > 0 ? SymbolSize : Math.Min(Width, Height);

                var rectangle = new Rectangle(
                    Rectangle.Center.X - size / 2,
                    Rectangle.Center.Y - size / 2,
                    size,
                    size);

                spriteBatch.Draw(texture, rectangle, color);

                return;
            }

            if (Text.Length == 0)
                return;

            var font = UIFonts.Get(FontRole);

            var textSize = font.MeasureString(Text);

            font.DrawText(
                spriteBatch,
                Text,
                new Vector2(
                    Rectangle.Center.X - textSize.X * 0.5f,
                    Rectangle.Center.Y - textSize.Y * 0.5f),
                color);
        }
    }
}
