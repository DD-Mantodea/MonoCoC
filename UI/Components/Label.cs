using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.UI.Framework;
using System;
using System.Linq;

namespace MonoCoC.UI.Components
{
    public enum TextVerticalAlignment
    {
        Top,

        Center,

        Bottom
    }

    public class Label : Component
    {
        public RichTextLayout RichTextLayout;

        public TextHorizontalAlignment HorizontalAlignment = TextHorizontalAlignment.Left;

        public TextVerticalAlignment VerticalAlignment = TextVerticalAlignment.Top;

        public bool AutoSize;

        public Color TextColor = Color.Black;

        private string _measuredText = "";

        private int _measuredWidth = -1;

        private Vector2 _textSize;

        private float _naturalWidth;

        public Label(SpriteFontBase font)
        {
            RichTextLayout = new RichTextLayout { Font = font };

            BackgroundColor = Color.Transparent;
        }

        public override void Update(GameTime gameTime)
        {
            RefreshMeasurement();

            base.Update(gameTime);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            RefreshMeasurement();

            var offset = VerticalAlignment switch
            {
                TextVerticalAlignment.Center => new Vector2(0, (Height - _textSize.Y) * 0.5f),
                TextVerticalAlignment.Bottom => new Vector2(0, Height - _textSize.Y),
                _ => Vector2.Zero
            };

            base.DrawSelf(spriteBatch, gameTime);

            if (Width > 0 && HorizontalAlignment is TextHorizontalAlignment.Center or TextHorizontalAlignment.Right)
            {
                DrawAlignedLines(spriteBatch, Position.Y + offset.Y);

                return;
            }

            if (Width <= 0)
            {
                RichTextLayout.Draw(
                    spriteBatch,
                    Position + offset,
                    TextColor * Alpha,
                    0,
                    Vector2.Zero,
                    null,
                    0,
                    HorizontalAlignment);

                return;
            }

            RichTextLayout.Draw(
                spriteBatch,
                Position + offset,
                TextColor * Alpha,
                0,
                Vector2.Zero,
                null,
                0,
                TextHorizontalAlignment.Left);
        }

        private void DrawAlignedLines(SpriteBatch spriteBatch, float y)
        {
            var font = RichTextLayout.Font;

            foreach (var line in RichTextLayout.Lines)
            {
                var text = string.Concat(line.Chunks.OfType<TextChunk>().Select(chunk => chunk.Text));

                var width = line.Size.X;

                var x = HorizontalAlignment == TextHorizontalAlignment.Center
                    ? Position.X + (Width - width) * 0.5f
                    : Position.X + Width - width;

                font.DrawText(spriteBatch, text, new Vector2(x, y), TextColor * Alpha);

                y += line.Size.Y + RichTextLayout.VerticalSpacing;
            }
        }

        private void RefreshMeasurement()
        {
            var text = RichTextLayout.Text ?? "";

            var wrapWidth = RichTextLayout.Width ?? 0;

            if (text == _measuredText && wrapWidth == _measuredWidth)
                return;

            _measuredText = text;

            _measuredWidth = wrapWidth;

            var measured = RichTextLayout.Measure(wrapWidth > 0 ? wrapWidth : 100000);

            _textSize = new Vector2(measured.X, measured.Y);

            _naturalWidth = measured.X;

            foreach (var line in RichTextLayout.Lines)
                _naturalWidth = Math.Max(_naturalWidth, line.Size.X);

            if (!AutoSize)
                return;

            LayoutData.Size = _textSize;

            if (wrapWidth <= 0)
                RichTextLayout.Width = (int)Math.Ceiling(_textSize.X);
        }

        public Label Text(string text)
        {
            RichTextLayout.Text = text;

            return this;
        }

        public Label WrapWidth(int width)
        {
            RichTextLayout.Width = width;

            return this;
        }

        public Label LineSpacing(int spacing)
        {
            RichTextLayout.VerticalSpacing = spacing;

            return this;
        }

        public Label Alignment(TextHorizontalAlignment alignment)
        {
            HorizontalAlignment = alignment;

            return this;
        }

        public Label VerticalAlign(TextVerticalAlignment alignment)
        {
            VerticalAlignment = alignment;

            return this;
        }
    }
}
