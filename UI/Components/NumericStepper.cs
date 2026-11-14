using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;
using MonoCoC.UI.Styles;
using System;

namespace MonoCoC.UI.Components
{
    public class NumericStepper : Component
    {
        private readonly Button _minus;
        private readonly Button _plus;
        private readonly Button _valueButton;
        private readonly TextBox _editor;
        private readonly Label _range;
        private float _value;
        private bool _editing;
        private bool _updating;
        private int _heldDirection;
        private float _holdTimer;
        private float _repeatTimer;
        private bool _longPressStarted;

        public NumericStepper(string fontRole = "coc:menu", int fontSize = 18)
        {
            BackgroundColor = Color.Transparent;

            FontRole = fontRole;
            FontSize = fontSize;

            _range = new Label(UIFonts.GetAt(UIFonts.Family(fontRole), 12))
            {
                CanHit = false,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            _range.Alignment(TextHorizontalAlignment.Right);

            _minus = new Button("", "-", fontRole)
            {
                FontSizeOverride = fontSize,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            _minus.Click(() => ClickStep(-1));

            _plus = new Button("", "+", fontRole)
            {
                FontSizeOverride = fontSize,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            _plus.Click(() => ClickStep(1));

            _valueButton = new Button("", "", fontRole)
            {
                FontSizeOverride = fontSize,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            _valueButton.Click(BeginEdit);

            _editor = new TextBox(UIFonts.GetAt(UIFonts.Family(fontRole), fontSize))
            {
                State = TextBoxState.OnlyNumber,
                ShowBorder = false,
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            _editor.OnTextChanged = text =>
            {
                if (_updating)
                    return;

                if (float.TryParse(text, out var parsed))
                    SetValue(parsed);
            };

            _editor.Cursor.CursorColor = TextColor;

            _editor.Cursor.SelectionColor = new Color(0xB7, 0x9A, 0x70, 120);

            AddChild(_range);
            AddChild(_minus);
            AddChild(_valueButton);
            AddChild(_editor);
            AddChild(_plus);
        }

        public string FontRole { get; set; }

        public int FontSize { get; set; }

        public float Min { get; set; }

        public float Max { get; set; } = 100f;

        public float Step { get; set; } = 1f;

        public float ShiftMultiplier { get; set; } = 10f;

        public string Unit { get; set; } = "";

        public int ButtonWidth { get; set; } = 44;

        public Color MinusColor { get; set; } = Color.Transparent;

        public Color PlusColor { get; set; } = Color.Transparent;

        public Color ValueColor { get; set; } = Color.Transparent;

        public Color TextColor { get; set; } = Color.Transparent;

        public bool ShowRange { get; set; }

        public Color RangeColor { get; set; } = Color.Transparent;

        public int RangeFontSize { get; set; } = 12;

        public UIStateStyle? ButtonStyle { get; set; }

        public UIStateStyle? MinusStyle { get; set; }

        public UIStateStyle? PlusStyle { get; set; }

        public UIStateStyle? ValueStyle { get; set; }

        public Action<float>? ValueChanged;

        public float Value
        {
            get => _value;
            set => SetValue(value);
        }

        private static bool ShiftHeld =>
            UserInput.CurrentKeyState.IsKeyDown(Keys.LeftShift) ||
            UserInput.CurrentKeyState.IsKeyDown(Keys.RightShift);

        public float[]? SnapValues;

        public void SetValue(float value)
        {
            _updating = true;

            if (SnapValues is { Length: > 0 })
            {
                var nearest = SnapValues[0];

                foreach (var candidate in SnapValues)
                    if (Math.Abs(candidate - value) < Math.Abs(nearest - value))
                        nearest = candidate;

                value = nearest;
            }

            _value = Math.Clamp(value, Min, Max);

            _valueButton.SetLabel(_value.ToString("0") + (Unit.Length > 0 ? " " + Unit : ""));

            _valueButton.Label.RichTextLayout.Font = UIFonts.GetAt(UIFonts.Family(FontRole), FontSize);

            _valueButton.FontSizeOverride = FontSize;

            _valueButton.TextColor = TextColor;

            _valueButton.BackgroundColor = ValueColor;

            _valueButton.StateStyle = ValueStyle ?? ButtonStyle;

            _range.Text($"{Min:0} – {Max:0}");
            _range.RichTextLayout.Font = UIFonts.GetAt(UIFonts.Family(FontRole), RangeFontSize);
            _range.TextColor = RangeColor;
            _range.Visible = ShowRange;

            _updating = false;

            ValueChanged?.Invoke(_value);
        }

        private void Add(float delta) => SetValue(_value + delta);

        private void ApplyStep(int direction)
        {
            Add(direction * Step * (ShiftHeld ? ShiftMultiplier : 1));
        }

        private void ClickStep(int direction)
        {
            if (_longPressStarted)
            {
                _longPressStarted = false;
                return;
            }

            ApplyStep(direction);
        }

        private void BeginEdit()
        {
            if (UIManager.Instance.FocusedComponent is TextBox previous && !ReferenceEquals(previous, _editor))
                previous.Active = false;

            _editing = true;

            _editor.Text = _value.ToString("0");

            _editor.Active = true;

            _editor.Cursor.CursorColor = TextColor;

            UIManager.Instance.FocusedComponent = _editor;
        }

        private void UpdateHold(GameTime gameTime)
        {
            var mouse = UserInput.CurrentMouseState;

            if (mouse.LeftButton != ButtonState.Pressed)
            {
                _heldDirection = 0;
                _holdTimer = 0;
                _repeatTimer = 0;

                return;
            }

            var direction = _minus.Rectangle.Contains(mouse.Position)
                ? -1
                : _plus.Rectangle.Contains(mouse.Position) ? 1 : 0;

            if (direction != _heldDirection)
            {
                _heldDirection = direction;
                _holdTimer = 0;
                _repeatTimer = 0;
                _longPressStarted = false;
            }

            if (direction == 0)
                return;

            var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _holdTimer += elapsed;

            if (!_longPressStarted)
            {
                if (_holdTimer < 0.45f)
                    return;

                _longPressStarted = true;
                _repeatTimer = 0;
                ApplyStep(direction);

                return;
            }

            _repeatTimer += elapsed;

            if (_repeatTimer < 0.075f)
                return;

            _repeatTimer = 0;
            ApplyStep(direction);
        }

        private void EndEdit()
        {
            _editor.Active = false;

            _editing = false;

            if (ReferenceEquals(UIManager.Instance.FocusedComponent, _editor))
                UIManager.Instance.FocusedComponent = null;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_editing)
                UpdateHold(gameTime);

            if (!_editing && ShiftHeld)
            {
                if (IsPressed(Keys.OemMinus) || IsPressed(Keys.Subtract))
                    Add(-Step * ShiftMultiplier);

                if (IsPressed(Keys.OemPlus) || IsPressed(Keys.Add))
                    Add(Step * ShiftMultiplier);
            }

            if (_editing && (IsPressed(Keys.Enter) || IsPressed(Keys.Escape)))
            {
                EndEdit();
            }

            if (_editing)
            {
                var mouse = UserInput.CurrentMouseState;

                if (mouse.LeftButton == ButtonState.Pressed && !Rectangle.Contains(mouse.Position))
                    EndEdit();
                else if (UIManager.Instance.FocusedComponent != _editor)
                    EndEdit();
            }

            _minus.Visible = !_editing;
            _plus.Visible = !_editing;
            _valueButton.Visible = !_editing;
            _editor.Visible = _editing;

            _minus.FontSizeOverride = FontSize;
            _plus.FontSizeOverride = FontSize;

            _range.LayoutData.Size = new Vector2(140, 16);
            _range.LayoutData.Position = new Vector2(Width - 140, -18);
            _range.Visible = ShowRange;

            _minus.LayoutData.Size = new Vector2(ButtonWidth, Height);
            _minus.LayoutData.Position = Vector2.Zero;
            _plus.LayoutData.Size = new Vector2(ButtonWidth, Height);
            _plus.LayoutData.Position = new Vector2(Width - ButtonWidth, 0);

            _valueButton.LayoutData.Size = new Vector2(Width - ButtonWidth * 2, Height);
            _valueButton.LayoutData.Position = new Vector2(ButtonWidth, 0);

            _editor.LayoutData.Size = new Vector2(Width - ButtonWidth * 2, Height);
            _editor.LayoutData.Position = new Vector2(ButtonWidth, 0);

            var editorWidth = Math.Max(1, Width - ButtonWidth * 2);
            var textWidth = _editor.Font.MeasureString(_editor.Text).X;
            var leftPadding = Math.Max(5, (int)((editorWidth - textWidth) / 2));

            _editor.LayoutData.Padding = new UIVec4(0, leftPadding, 0, 0);

            _minus.BackgroundColor = MinusColor;
            _plus.BackgroundColor = PlusColor;
            _minus.TextColor = TextColor;
            _plus.TextColor = TextColor;
            _minus.StateStyle = MinusStyle ?? ButtonStyle;
            _plus.StateStyle = PlusStyle ?? ButtonStyle;
            _valueButton.StateStyle = ValueStyle ?? ButtonStyle;
        }

        private static bool IsPressed(Keys key) =>
            UserInput.PreviousKeyState.IsKeyUp(key) && UserInput.CurrentKeyState.IsKeyDown(key);
    }
}
