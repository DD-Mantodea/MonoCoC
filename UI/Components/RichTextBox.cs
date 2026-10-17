using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class RichTextBox(SpriteFontBase font) : Component
    {
        public RichTextLayout RichTextLayout = new() { Font = font };

        public TextHorizontalAlignment HorizontalAlignment = TextHorizontalAlignment.Left;

        public Color TextColor = Color.Black;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            RichTextLayout.Draw(spriteBatch, Position, TextColor * Alpha, 0, Vector2.Zero, null, 0, HorizontalAlignment);

            base.DrawSelf(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public RichTextBox Text(string text)
        {
            RichTextLayout.Text = text;

            return this;
        }

        public RichTextBox WrapWidth(int width)
        {
            RichTextLayout.Width = width;

            return this;
        }

        public RichTextBox LineSpacing(int spacing)
        {
            RichTextLayout.VerticalSpacing = spacing;

            return this;
        }

        public RichTextBox Alignment(TextHorizontalAlignment alignment)
        {
            HorizontalAlignment = alignment;

            return this;
        }
    }
}