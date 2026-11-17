using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components.Settings
{
    // 按键位 chip：视觉与 SettingPane.Toggle 的 On/Off 完全一致。
    // 已绑定 = 浅绿填充 + 2px 橄榄绿描边 + 深色字；
    // 空位   = 透明填充 + 1px 半透明浅棕描边 + 灰字；
    // 等待按键 = 外部文案 + 填充态（由 OptionsView 决定文案）。
    public class KeyChip : Component
    {
        public const int ChipWidth = 148;

        public const int ChipHeight = 40;

        private static readonly Color BoundFill = UIComponentStyles.KeyChip.Fill;

        private static readonly Color BoundFillHover = UIComponentStyles.KeyChip.FillHover;

        private static readonly Color BoundBorder = UIComponentStyles.KeyChip.Border;

        private static readonly Color BoundText = UIComponentStyles.KeyChip.Text;

        private static readonly Color EmptyBorder = UIComponentStyles.KeyChip.EmptyBorder;

        private static readonly Color EmptyFillHover = UIComponentStyles.KeyChip.EmptyFillHover;

        private static readonly Color EmptyText = UIComponentStyles.KeyChip.EmptyText;

        private static readonly int BoundBorderThickness = UIComponentStyles.KeyChip.BorderThickness;

        private static readonly int EmptyBorderThickness = UIComponentStyles.KeyChip.EmptyBorderThickness;

        private readonly SpriteFontBase _font;

        private readonly Label _label;

        private string _text = "";

        private bool _filled;

        private Vector2 _measured;

        public Action? Clicked;

        public KeyChip()
        {
            CanHit = true;

            _font = UIFonts.Get("switcher");

            _label = new Label(_font)
            {
                CanHit = false,

                LayoutData = { LayoutType = LayoutType.Relative }
            };

            AddChild(_label);
        }

        public void SetState(string text, bool filled)
        {
            _text = text;

            _filled = filled;

            _label.TextColor = filled ? BoundText : EmptyText;

            _label.Text(text);

            _measured = _font.MeasureString(text);

            Reflow();
        }

        public override void Update(GameTime gameTime)
        {
            Reflow();

            base.Update(gameTime);
        }

        private void Reflow()
        {
            var x = (int)((Width - _measured.X) / 2);

            var y = (int)((Height - _measured.Y) / 2);

            if (_label.LayoutData.Position.X == x && _label.LayoutData.Position.Y == y)
                return;

            _label.LayoutData.Position = new Vector2(x, y);
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (args.Type != InputType.MouseLeftClick)
                return base.OnInput(args);

            Clicked?.Invoke();

            return InputResult.Consumed;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var fill = _filled
                ? IsHovering ? BoundFillHover : BoundFill
                : IsHovering ? EmptyFillHover : Color.Transparent;

            if (fill != Color.Transparent)
                spriteBatch.DrawRectangle(Rectangle, fill * LayoutData.Alpha);

            spriteBatch.DrawBorder(Rectangle, _filled ? BoundBorderThickness : EmptyBorderThickness,
                (_filled ? BoundBorder : EmptyBorder) * LayoutData.Alpha);

            if (IsHovering)
                spriteBatch.DrawBorder(Rectangle, BoundBorderThickness, BoundBorder * (LayoutData.Alpha * 0.5f));
        }
    }
}
