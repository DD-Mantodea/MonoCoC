using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Scripting;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Components;
using System;

namespace MonoCoC.UI.Components
{
    /// <summary>
    /// A template-built hint. The HJSON control template supplies the
    /// decorations and named "header"/"body" label parts; this class only
    /// binds text and positions the visual tree.
    /// </summary>
    public class HintView
    {
        private readonly Component _root;

        private readonly Label? _header;

        private readonly Label? _body;

        private readonly HintStyle _style;

        private readonly bool _autoLayout;

        private readonly Parser _parser = new(new ScriptContext());

        private string _headerText = "";

        private string _bodyText = "";

        public Component Root => _root;

        public Label? Header => _header;

        public Label? Body => _body;

        public HintView(Component root, HintStyle style)
            : this(root, style, false)
        {
        }

        public HintView(Component root, HintStyle style, bool autoLayout)
        {
            _root = root;

            _style = style;

            _autoLayout = autoLayout;

            _header = FindPart(root, "header") as Label;

            _body = FindPart(root, "body") as Label;

            _parser.Context.Resolver = Scene.ResolveTag;
        }

        public static HintView CreateDefault(HintStyle style)
        {
            var root = new Container
            {
                CanHit = false,
                BackgroundColor = style.BackgroundColor * style.Alpha
            };

            var header = new Label(UIFonts.Get(style.HeaderFont))
            {
                PartName = "header",
                CanHit = false
            };

            var body = new Label(UIFonts.Get(style.BodyFont))
            {
                PartName = "body",
                CanHit = false
            };

            header.Of(root);

            body.Of(root);

            return new HintView(root, style, true);
        }

        public void Show(string header, string body, Rectangle anchor)
        {
            _header?.TextColor = _style.HeaderColor;

            _body?.TextColor = _style.BodyColor;

            if (header != _headerText)
            {
                _headerText = header;

                _header?.Text(header);
            }

            if (body != _bodyText)
            {
                _bodyText = body;

                _body?.Text(_parser.Parse(body));
            }

            var contentWidth = Math.Max(1, _style.Width - _style.Padding * 2);

            var bodyHeight = _body?.RichTextLayout.Measure(contentWidth).Y ?? 0;

            var height = Math.Max(_style.MinHeight, (int)bodyHeight + _style.BodyY + _style.Padding);

            _root.LayoutData.LayoutType = LayoutType.Absolute;

            _root.LayoutData.Size = new Vector2(_style.Width, height);

            _root.LayoutData.Position = HintPlacement.Resolve(_style, anchor, _style.Width, height);

            if (!_autoLayout)
                return;

            if (_header is not null)
            {
                _header.LayoutData.Position = new Vector2(_style.Padding, _style.HeaderY);

                _header.LayoutData.Size = new Vector2(contentWidth, _style.HeaderHeight);
            }

            if (_body is not null)
            {
                _body.LayoutData.Position = new Vector2(_style.Padding, _style.BodyY);

                _body.LayoutData.Size = new Vector2(contentWidth, height - _style.BodyY - _style.Padding);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime) =>
            _root.Draw(spriteBatch, gameTime);

        private static Component? FindPart(Component component, string part)
        {
            if (string.Equals(component.PartName, part, StringComparison.OrdinalIgnoreCase))
                return component;

            foreach (var child in component.LayoutData.Children)
            {
                var result = FindPart(child, part);

                if (result is not null)
                    return result;
            }

            return null;
        }
    }
}
