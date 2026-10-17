using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Scripting;
using MonoCoC.UI.Common;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    public class ToolTipView
    {
        public const int Width = 350;

        public const int MinHeight = 239;

        public const int Padding = 15;

        public const int ContentWidth = 316;

        public const int HeaderY = 15;

        public const int HeaderHeight = 25;

        public const int SeparatorY = 40;

        public const int BodyY = 44;

        public const int SeparatorWidth = 320;

        private readonly UIImage _background;

        private readonly Label _header;

        private readonly Label _text;

        private readonly Parser _parser = new(new ScriptContext());

        private string _headerText;

        private string _bodyText;

        public bool Visible;

        public ToolTipView(Texture2D background, SpriteFontBase headerFont, SpriteFontBase textFont)
        {
            _background = new UIImage(background);

            _header = new Label(headerFont)
            {
                TextColor = UIColors.ToolTipText
            };

            _text = new Label(textFont)
            {
                TextColor = UIColors.ToolTipText
            };

            _text.WrapWidth(ContentWidth);

            _parser.Context.Resolver = Scene.ResolveTag;
        }

        public void Show(Component source)
        {
            var header = source.ToolTipHeader ?? "";

            var body = source.ToolTipText ?? "";

            if (body != _bodyText || header != _headerText)
            {
                _bodyText = body;

                _headerText = header;

                _header.Text(header);

                _text.Text(_parser.Parse(body));
            }

            Layout(source);

            Visible = true;
        }

        public void Hide()
        {
            Visible = false;

            _bodyText = null;

            _headerText = null;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Visible)
                return;

            _background.Draw(spriteBatch, gameTime);

            spriteBatch.DrawRectangle(_background.Position.Add(Padding, SeparatorY).BuildRect(SeparatorWidth, 1), Color.Black);

            _header.Draw(spriteBatch, gameTime);

            _text.Draw(spriteBatch, gameTime);
        }

        private void Layout(Component source)
        {
            var bounds = source.Rectangle;

            var height = Math.Max(_text.RichTextLayout.Measure(ContentWidth).Y + 63, MinHeight);

            var x = bounds.X;

            if (x < 0)
                x = 0;
            else if (x + Width > LayoutConstants.ScreenWidth)
                x = LayoutConstants.ScreenWidth - Width;

            var y = bounds.Bottom < LayoutConstants.ScreenHeight / 2 ? bounds.Bottom + 1 : bounds.Y - height;

            _background.Position(x, y).Size(Width, height);

            _header.Position(x + Padding, y + HeaderY).Size(ContentWidth, HeaderHeight);

            _text.Position(x + Padding, y + BodyY).Size(ContentWidth, height - 63);
        }
    }
}