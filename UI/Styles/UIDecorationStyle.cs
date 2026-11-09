using Microsoft.Xna.Framework;

namespace MonoCoC.UI.Styles
{
    public sealed class UIDecorationStyle
    {
        public Color Normal = Color.Transparent;

        public Color Hover = Color.Transparent;

        public Color Pressed = Color.Transparent;

        public Color Disabled = Color.Transparent;

        public Color NormalSecondary = Color.Transparent;

        public Color HoverSecondary = Color.Transparent;

        public Color PressedSecondary = Color.Transparent;

        public Color DisabledSecondary = Color.Transparent;

        public Color PrimaryFor(bool enabled, bool hovered, bool pressed) =>
            !enabled ? Disabled : pressed ? Pressed : hovered ? Hover : Normal;

        public Color SecondaryFor(bool enabled, bool hovered, bool pressed) =>
            !enabled ? DisabledSecondary : pressed ? PressedSecondary : hovered ? HoverSecondary : NormalSecondary;
    }
}
