using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    public class Slider : Component
    {
        private readonly Label _value;

        private float _current;

        private bool _dragging;

        public Slider()
        {
            BackgroundColor = Color.Transparent;

            _value = new Label(UIFonts.GetAt(UIFonts.Family("coc:menu"), 18))
            {
                CanHit = false
            };

            _value.Alignment(TextHorizontalAlignment.Right);

            _value.VerticalAlign(TextVerticalAlignment.Center);

            _value.LayoutData.LayoutType = LayoutType.Relative;

            AddChild(_value);
        }

        public float Min { get; set; }

        public float Max { get; set; } = 100f;

        public float Step { get; set; } = 1f;

        public string Unit { get; set; } = "";

        public Color TrackColor { get; set; } = Color.Transparent;

        public Color KnobColor { get; set; } = Color.Transparent;

        public Color TextColor { get; set; } = Color.Transparent;

        public int FontSize { get; set; } = 18;

        public Action<float>? ValueChanged;

        public float Value
        {
            get => _current;

            set => SetValue(value);
        }

        public void SetValue(float value)
        {
            _current = Math.Clamp(value, Min, Max);

            _value.Text(_current.ToString("0") + (Unit.Length > 0 ? " " + Unit : ""));

            ValueChanged?.Invoke(_current);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _value.TextColor = TextColor;

            _value.RichTextLayout.Font = UIFonts.GetAt(UIFonts.Family("coc:menu"), FontSize);

            var trackWidth = Math.Max(1, Width - 80);

            var mouse = UserInput.CurrentMouseState;

            var pressed = mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;

            if (pressed && !_dragging && Rectangle.Contains(mouse.Position))
                _dragging = true;

            if (!pressed)
                _dragging = false;

            if (_dragging)
            {
                var ratio = Math.Clamp((mouse.X - Rectangle.X) / (float)trackWidth, 0f, 1f);

                var value = Min + ratio * (Max - Min);

                SetValue((float)Math.Round(value / Step) * Step);
            }

            _value.LayoutData.Size = new Vector2(76, Height);

            _value.LayoutData.Position = new Vector2(Width - 76, 0);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var trackWidth = Math.Max(1, Width - 80);

            var track = new Rectangle(Rectangle.X, Rectangle.Y + Height / 2 - 4, trackWidth, 8);

            spriteBatch.DrawRectangle(track, TrackColor);

            var ratio = Max > Min ? (_current - Min) / (Max - Min) : 0f;

            var knobX = track.X + (int)(track.Width * ratio);

            spriteBatch.DrawRectangle(new Rectangle(knobX - 6, Rectangle.Y + Height / 2 - 12, 12, 24), KnobColor);
        }
    }
}
