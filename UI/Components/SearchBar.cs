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

        private readonly TextBox _box;

        public SearchBar(Action<string> onChanged, string hint)
        {
            BackgroundColor = UIComponentStyles.SearchBar.Background;

            CanHit = true;

            LayoutData.Size = new Vector2(290, 40);

            LayoutData.Position = new Vector2(975, 20);

            var font = UIFonts.Get("search");

            _box = new TextBox(font)
                .Size(274, 28)
                .Position(8, 6)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _box.TextColor = UIComponentStyles.SearchBar.Text;

            _box.ShowBorder = false;

            _box.Cursor = new Cursor(UIComponentStyles.SearchBar.Text, UIComponentStyles.TextBox.Selection)
            {
                TextBox = _box
            };

            // 用字形实际高度居中，而不是 LineHeight（后者包含 descender 空白，视觉会偏上）。
            var hintHeight = (int)MathF.Ceiling(font.MeasureString(hint).Y);

            _hint = new Label(font)
                .Text(hint)
                .Position(13, (40 - hintHeight) / 2)
                .CanHit(false)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _hint.TextColor = UIComponentStyles.SearchBar.Hint;

            _box.OnTextChanged = text =>
            {
                _hint.Visible = string.IsNullOrEmpty(text) && !_box.Active;

                onChanged(text);
            };
        }

        public override void Update(GameTime gameTime)
        {
            // 聚焦时隐藏占位提示（即使文本还是空的）。
            _hint.Visible = !_box.Active && string.IsNullOrEmpty(_box.Text);

            base.Update(gameTime);
        }
        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            spriteBatch.DrawBorder(Rectangle, UIComponentStyles.SearchBar.BorderThickness, UIComponentStyles.SearchBar.Border);
        }
    }
}
