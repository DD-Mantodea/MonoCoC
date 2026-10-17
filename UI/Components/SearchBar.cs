using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    public class SearchBar : Container
    {
        private readonly Label _hint;

        public SearchBar(Action<string> onChanged, string hint)
        {
            BackgroundColor = UIColors.SearchBackground;

            CanHit = true;

            LayoutData.Size = new Vector2(280, 30);

            LayoutData.Position = new Vector2(675, 8);

            var font = UIFonts.Get("search");

            var box = new TextBox(font)
                .Size(264, 22)
                .Position(8, 4)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            box.TextColor = UIColors.SearchText;

            box.ShowBorder = false;

            box.Cursor = new Cursor(UIColors.SearchText, UIColors.InputSelection)
            {
                TextBox = box
            };

            _hint = new Label(font)
                .Text(hint)
                .Position(13, 17 - font.LineHeight / 2)
                .CanHit(false)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _hint.TextColor = UIColors.SearchHint;

            box.OnTextChanged = text =>
            {
                _hint.Visible = text.Length == 0;

                onChanged(text);
            };
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            spriteBatch.DrawBorder(Rectangle, 1, UIColors.SearchBorder);
        }
    }
}
