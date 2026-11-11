using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;

namespace MonoCoC.UI.Components
{
    public class Bar : Component
    {
        public Bar(SpriteFontBase font)
        {
            Font = font;
        }

        public SpriteFontBase Font;

        public float Value;

        public float Max = 1;

        public float Min;

        public Color FillColor = new(177, 125, 94);

        public Color TrackColor = new(40, 28, 20, 120);

        public Color BorderColor = new(112, 77, 40);

        public Color TextColor = new(59, 44, 29);

        public Color MarkerColor = new(141, 40, 56);

        public int BorderThickness = 1;

        public int TextPadding = 8;

        public string Label = "";

        public string TextFormat = "{0:0} / {1:0}";

        public bool ShowText = true;

        public float Marker = float.NaN;

        public bool ShowMarker => !float.IsNaN(Marker);

        public UIBinding? ValueBinding;

        public UIBinding? MaxBinding;

        public UIBinding? MinBinding;

        public UIBinding? MarkerBinding;

        public override void Update(GameTime gameTime)
        {
            if (ValueBinding?.Resolve(DataContext) is { } value)
                Value = Convert.ToSingle(value);

            if (MaxBinding?.Resolve(DataContext) is { } max)
                Max = Convert.ToSingle(max);

            if (MinBinding?.Resolve(DataContext) is { } min)
                Min = Convert.ToSingle(min);

            if (MarkerBinding?.Resolve(DataContext) is { } marker)
                Marker = Convert.ToSingle(marker);

            base.Update(gameTime);
        }

        public float Ratio
        {
            get
            {
                var range = Max - Min;

                if (range <= 0)
                    return 0;

                return Math.Clamp((Value - Min) / range, 0, 1);
            }
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var bounds = Rectangle;

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var border = Math.Max(0, BorderThickness);

            var inner = new Rectangle(
                bounds.X + border,
                bounds.Y + border,
                Math.Max(0, bounds.Width - border * 2),
                Math.Max(0, bounds.Height - border * 2));

            if (inner.Width > 0 && inner.Height > 0)
            {
                spriteBatch.DrawRectangle(inner, TrackColor * Alpha);

                var fillWidth = (int)MathF.Round(inner.Width * Ratio);

                if (fillWidth > 0)
                    spriteBatch.DrawRectangle(
                        new Rectangle(inner.X, inner.Y, fillWidth, inner.Height),
                        FillColor * Alpha);
            }

            if (border > 0 && BorderColor.A > 0)
                spriteBatch.DrawBorder(bounds, border, BorderColor * Alpha);

            if (ShowMarker && inner.Width > 0)
            {
                var markerWidth = Math.Max(2, border + 1);

                var markerX = inner.X + (int)MathF.Round(inner.Width * Math.Clamp((Marker - Min) / (Max - Min), 0, 1));

                spriteBatch.DrawRectangle(
                    new Rectangle(markerX, inner.Y, markerWidth, inner.Height),
                    MarkerColor * Alpha);
            }

            if (!ShowText || inner.Width <= 0 || inner.Height <= 0)
                return;

            var label = Label;

            var value = FormatValue();

            var labelSize = label.Length == 0 ? Vector2.Zero : Font.MeasureString(label);

            var valueSize = value.Length == 0 ? Vector2.Zero : Font.MeasureString(value);

            var y = bounds.Y + (bounds.Height - labelSize.Y) * 0.5f;

            if (label.Length > 0)
                Font.DrawText(spriteBatch, label, new Vector2(bounds.X + TextPadding, y), TextColor * Alpha);

            if (value.Length > 0)
            {
                var valueX = bounds.Right - TextPadding - valueSize.X;

                Font.DrawText(spriteBatch, value, new Vector2(valueX, bounds.Y + (bounds.Height - valueSize.Y) * 0.5f), TextColor * Alpha);
            }
        }

        private string FormatValue()
        {
            if (string.IsNullOrEmpty(TextFormat))
                return "";

            try
            {
                return string.Format(TextFormat, Value, Max, Min);
            }
            catch (FormatException)
            {
                return $"{Value:0} / {Max:0}";
            }
        }
    }
}
