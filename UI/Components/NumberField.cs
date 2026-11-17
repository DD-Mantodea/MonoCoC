using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Globalization;

namespace MonoCoC.UI.Components
{
    public class NumberField : Component
    {
        private readonly Button _minus;

        private readonly Button _plus;

        private readonly TextBox _box;

        private readonly Label _unit;

        private float _value;

        private bool _updating;

        public NumberField()
        {
            _minus = new Button("", "-", "coc:menu")
            {
                BackgroundColor = Color.Transparent,
                TextColor = UIComponentStyles.NumberField.Text
            };

            _minus.Click(() => Add(-Step));

            _minus.LayoutData.LayoutType = LayoutType.Relative;

            _plus = new Button("", "+", "coc:menu")
            {
                BackgroundColor = Color.Transparent,
                TextColor = UIComponentStyles.NumberField.Text
            };

            _plus.Click(() => Add(Step));

            _plus.LayoutData.LayoutType = LayoutType.Relative;

            _box = new TextBox(UIFonts.GetAt(UIFonts.Family("coc:input"), 20))
            {
                TextColor = UIComponentStyles.NumberField.Text,
                ShowBorder = false,
                State = TextBoxState.OnlyNumber,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            _box.OnTextChanged = text =>
            {
                if (_updating)
                    return;

                if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                {
                    SetValue(value);

                    return;
                }

                SetValue(_value);
            };

            _unit = new Label(UIFonts.GetAt(UIFonts.Family("coc:menu"), 18))
            {
                TextColor = UIComponentStyles.NumberField.SecondaryText,
                CanHit = false
            };

            _unit.Alignment(TextHorizontalAlignment.Center);

            _unit.VerticalAlign(TextVerticalAlignment.Center);

            _unit.LayoutData.LayoutType = LayoutType.Relative;

            AddChild(_minus);

            AddChild(_box);

            AddChild(_unit);

            AddChild(_plus);

        }



        public float Min { get; set; }

        public float Max { get; set; } = 100f;

        public float Step { get; set; } = 1f;

        public string Unit { get; set; } = "";

        public Action<float>? ValueChanged;

        public float Value
        {
            get => _value;

            set => SetValue(value);
        }

        public void SetValue(float value)
        {
            _updating = true;

            _value = Math.Clamp(value, Min, Max);

            _box.Text = _value.ToString("0", CultureInfo.InvariantCulture);

            _unit.Text(Unit);

            _updating = false;

            ValueChanged?.Invoke(_value);
        }

        private void Add(float delta) => SetValue(_value + delta);

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var buttonWidth = Math.Min(36, Math.Max(24, Width / 7));

            var unitWidth = Unit.Length > 0 ? 36 : 0;

            _minus.LayoutData.Size = new Vector2(buttonWidth, Height);

            _minus.LayoutData.Position = new Vector2(0, 0);

            _plus.LayoutData.Size = new Vector2(buttonWidth, Height);

            _plus.LayoutData.Position = new Vector2(Width - buttonWidth, 0);

            _box.LayoutData.Size = new Vector2(Width - buttonWidth * 2 - unitWidth, Height);

            _box.LayoutData.Position = new Vector2(buttonWidth, 0);

            _unit.LayoutData.Size = new Vector2(unitWidth, Height);

            _unit.LayoutData.Position = new Vector2(Width - buttonWidth - unitWidth, 0);

            _box.Font = UIFonts.GetAt(UIFonts.Family("coc:input"), Math.Clamp((int)(Height * 0.5f), 16, 24));
        }
    }
}
