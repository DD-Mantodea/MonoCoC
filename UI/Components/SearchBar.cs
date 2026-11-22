using Hjson;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Runtime.CompilerServices;

namespace MonoCoC.UI.Components
{
    public class SearchBar : Container, IFromHjson<SearchBar>
    {
        private readonly Label _hint;

        private readonly TextBox _box;

        public SearchBar(Action<string> onChanged, string hint)
        {
            BackgroundColor = UIParseUtils.ColorOf("#EEE4CDFF");

            CanHit = true;

            LayoutData.Size = new Vector2(290, 40);

            LayoutData.Position = new Vector2(975, 20);

            var font = UIFonts.Get("search");

            _box = new TextBox(font)
                .Size(274, 28)
                .Position(8, 6)
                .LayoutType(LayoutType.Relative)
                .Of(this);

            _box.TextColor = UIParseUtils.ColorOf("#281E14FF");

            _box.ShowBorder = false;

            _box.Cursor = new Cursor(UIParseUtils.ColorOf("#281E14FF"), UIParseUtils.ColorOf("#31405172"))
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

            _hint.TextColor = UIParseUtils.ColorOf("#7D694EFF");

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

            spriteBatch.DrawBorder(Rectangle, 1, UIParseUtils.ColorOf("#5C4126FF"));
        }
        public static string TypeId => "coc:search_bar";

        public static Component FromHjson(JsonObject node, UIBuildContext context) =>
            new SearchBar(_ => { }, node.Str("hint"));

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<SearchBar>();
    }
}
