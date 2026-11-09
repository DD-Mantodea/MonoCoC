using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
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

        private readonly Texture2D _defaultBackground;

        private readonly Label _header;

        private readonly Label _text;

        private readonly Parser _parser = new(new ScriptContext());

        private HintStyle _style = HintStyle.Default;

        private HintView? _customHint;

        private string _headerText;

        private string _bodyText;

        public bool Visible;

        public ToolTipView(Texture2D background, SpriteFontBase headerFont, SpriteFontBase textFont)
        {
            _defaultBackground = background;

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
            if (source is Button { HintView: not null } button)
            {
                _style = button.HintSettings ?? HintStyle.Default;

                _customHint = button.HintView;

                _customHint.Show(source.ToolTipHeader ?? "", source.ToolTipText ?? "", source.Rectangle);

                Visible = true;

                return;
            }

            _customHint = null;

            _style = source is Button { HintSettings: not null } buttonWithStyle
                ? buttonWithStyle.HintSettings
                : HintStyle.Default;

            ApplyStyle();

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

            _customHint = null;

            _bodyText = null;

            _headerText = null;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Visible)
                return;

            if (_customHint is not null)
            {
                _customHint.Draw(spriteBatch, gameTime);

                return;
            }

            _background.Draw(spriteBatch, gameTime);

            DrawBorder(spriteBatch);

            spriteBatch.DrawRectangle(
                _background.Position.Add(_style.Padding, _style.SeparatorY).BuildRect(_style.SeparatorWidth, 1),
                _style.SeparatorColor);

            _header.Draw(spriteBatch, gameTime);

            _text.Draw(spriteBatch, gameTime);
        }

        private void DrawBorder(SpriteBatch spriteBatch)
        {
            if (_style.BorderThickness <= 0 || _style.BorderColor == Color.Transparent)
                return;

            var bounds = _background.Position.BuildRect(_style.Width, _background.Height);

            var thickness = _style.BorderThickness;

            var color = _style.BorderColor * _style.Alpha;

            spriteBatch.DrawRectangle(new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
            spriteBatch.DrawRectangle(new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
            spriteBatch.DrawRectangle(new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
            spriteBatch.DrawRectangle(new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
        }

        private void ApplyStyle()
        {
            var texture = TextureRegistry.Instance.Resolve(_style.BackgroundTexture);

            _background.Texture = texture ?? _defaultBackground;

            _background.Color = _style.BackgroundColor;

            _background.LayoutData.Alpha = _style.Alpha;

            _header.RichTextLayout.Font = UIFonts.Get(_style.HeaderFont);

            _text.RichTextLayout.Font = UIFonts.Get(_style.BodyFont);

            _header.TextColor = _style.HeaderColor;

            _text.TextColor = _style.BodyColor;

            _text.WrapWidth(_style.Width - _style.Padding * 2);
        }

        private void Layout(Component source)
        {
            var bounds = source.Rectangle;

            var contentWidth = _style.Width - _style.Padding * 2;

            var height = Math.Max(
                _text.RichTextLayout.Measure(contentWidth).Y + _style.BodyY + _style.Padding,
                _style.MinHeight);

            var position = HintPlacement.Resolve(_style, bounds, _style.Width, height);

            var x = (int)position.X;

            var y = (int)position.Y;

            _background.Position(x, y).Size(_style.Width, height);

            _header.Position(x + _style.Padding, y + _style.HeaderY)
                .Size(contentWidth, _style.HeaderHeight);

            _text.Position(x + _style.Padding, y + _style.BodyY)
                .Size(contentWidth, height - _style.BodyY - _style.Padding);
        }
    }
}
