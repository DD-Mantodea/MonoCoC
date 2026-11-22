using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Runtime.CompilerServices;

using Hjson;

namespace MonoCoC.UI.Components
{
    public class Slider : Component, IFromHjson<Slider>
    {
        public float Min;

        public float Max = 1;

        public float Step = 1;

        public Color TrackColor = UIParseUtils.ColorOf("#B79A70FF");

        public Color FillColor = UIParseUtils.ColorOf("#6D5436FF");

        public Color KnobColor = UIParseUtils.ColorOf("#35291FFF");

        public Color BorderColor = UIParseUtils.ColorOf("#9F8156FF");

        public int BorderThickness = 1;

        public int KnobWidth = 16;

        public int KnobHeight = 24;

        public bool ShowBorder = true;

        public Action<float>? ValueChanged;

        public bool Dragging { get; private set; }

        private float _value;

        public float Value
        {
            get => _value;

            set => SetValue(value, true);
        }

        public Slider()
        {
            CanHit = true;

            Scissor = false;
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = UserInput.GetMousePos();

            var hovered = Rectangle.Contains(mouse);

            var held = UserInput.CurrentMouseState.MouseLeft();

            MouseCursor = hovered || Dragging ? MouseCursor.Hand : MouseCursor.Arrow;

            if (UserInput.MouseLeftPressed && hovered)
            {
                Dragging = true;

                SetFromMouse((int)mouse.X);
            }

            if (Dragging && held)
                SetFromMouse((int)mouse.X);

            if (!held)
                Dragging = false;

            base.Update(gameTime);
        }

        private void SetFromMouse(int x)
        {
            var width = Math.Max(1, Width - KnobWidth);

            var ratio = Math.Clamp((x - Rectangle.X - KnobWidth / 2f) / width, 0f, 1f);

            SetValue(Min + ratio * (Max - Min), true);
        }

        private void SetValue(float value, bool notify)
        {
            var clamped = Math.Clamp(value, Min, Max);

            if (Step > 0)
                clamped = MathF.Round((clamped - Min) / Step) * Step + Min;

            clamped = Math.Clamp(clamped, Min, Max);

            if (Math.Abs(clamped - _value) < 0.0001f)
                return;

            _value = clamped;

            if (notify)
                ValueChanged?.Invoke(_value);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var bounds = Rectangle;

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var border = Math.Max(0, BorderThickness);

            var trackHeight = Math.Max(4, bounds.Height - KnobHeight);

            var trackY = bounds.Y + (bounds.Height - trackHeight) / 2;

            var track = new Rectangle(
                bounds.X + KnobWidth / 2,
                trackY,
                Math.Max(1, bounds.Width - KnobWidth),
                trackHeight);

            spriteBatch.DrawRectangle(track, TrackColor * Alpha);

            var ratio = Max == Min ? 0 : Math.Clamp((_value - Min) / (Max - Min), 0, 1);

            var fillWidth = (int)MathF.Round(track.Width * ratio);

            if (fillWidth > 0)
                spriteBatch.DrawRectangle(new Rectangle(track.X, track.Y, fillWidth, track.Height), FillColor * Alpha);

            if (ShowBorder && border > 0 && BorderColor.A > 0)
                spriteBatch.DrawBorder(bounds, border, BorderColor * Alpha);

            var knobX = bounds.X + (int)MathF.Round((bounds.Width - KnobWidth) * ratio);

            spriteBatch.DrawRectangle(
                new Rectangle(knobX, bounds.Y, KnobWidth, bounds.Height),
                KnobColor * Alpha);
        }

        public static string TypeId => "coc:slider";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var slider = new Slider
            {
                Min = node.Float("min", 0),
                Max = node.Float("max", 1),
                Step = node.Float("step", 1),
                KnobWidth = node.Int("knobWidth", 16),
                KnobHeight = node.Int("knobHeight", 24),
                ShowBorder = node.Bool("showBorder", true),
                BorderThickness = node.Int("borderThickness", 1),
                TrackColor = UIParseUtils.ColorOf(node.Str("trackColor", "#B79A70FF")),
                FillColor = UIParseUtils.ColorOf(node.Str("fillColor", "#6D5436FF")),
                KnobColor = UIParseUtils.ColorOf(node.Str("knobColor", "#35291FFF")),
                BorderColor = UIParseUtils.ColorOf(node.Str("borderColor", "#9F8156FF"))
            };

            slider.Value = node.Float("value", slider.Min);

            return slider;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<Slider>();
    }
}
