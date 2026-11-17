using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.UI.Data;
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

        public UIBinding? TextBinding;

        public Label BindText(UIBinding binding)
        {
            TextBinding = binding;

            return this;
        }

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
            if (TextBinding is not null)
            {
                var value = TextBinding.Resolve(DataContext);

                if (value is not null)
                    Text(value.ToString() ?? "");
            }

            base.Update(gameTime);
        }

        public override void PerformLayout(GameTime gameTime)
        {
            RefreshMeasurement();

            base.PerformLayout(gameTime);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
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

            // 只在文本/换行宽度变化时重新测量；AutoSize 的应用必须每帧执行，
            // 否则先测量、后把 AutoSize 置 true 的标签永远拿不到尺寸（高度 0 会压扁所在网格行）。
            if (text != _measuredText || wrapWidth != _measuredWidth)
            {
                _measuredText = text;

                _measuredWidth = wrapWidth;

                var measured = RichTextLayout.Measure(wrapWidth > 0 ? wrapWidth : 100000);

                _textSize = new Vector2(measured.X, measured.Y);

                _naturalWidth = measured.X;

                foreach (var line in RichTextLayout.Lines)
                    _naturalWidth = Math.Max(_naturalWidth, line.Size.X);
            }

            if (!AutoSize)
                return;

            LayoutData.Size = _textSize;

            if (wrapWidth <= 0)
                RichTextLayout.Width = (int)Math.Ceiling(_textSize.X);
        }

        public Label Text(string text)
        {
            RichTextLayout.Text = text;

            // 文本一改就立刻重新测量：否则同一帧里读 Width/Height 或用旧尺寸居中的地方
            // 会先按旧文本画一次，下一帧才纠正 —— 视觉上就是“点一下文字跳一下”。
            RefreshMeasurement();

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
