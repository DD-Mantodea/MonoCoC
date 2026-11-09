using Microsoft.Xna.Framework;

namespace MonoCoC.UI.Styles
{
    /// <summary>
    /// Optional Normal/Hover/Pressed/Disabled visual state colors for a
    /// Component. Border is drawn after children so it stays visible above
    /// material overlays and labels.
    /// </summary>
    public sealed class UIStateStyle
    {
        public Color NormalBackground = Color.Transparent;

        public Color HoverBackground = Color.Transparent;

        public Color PressedBackground = Color.Transparent;

        public Color DisabledBackground = Color.Transparent;

        public Color NormalBorder = Color.Transparent;

        public Color HoverBorder = Color.Transparent;

        public Color PressedBorder = Color.Transparent;

        public Color DisabledBorder = Color.Transparent;

        public Color? NormalText;

        public Color? HoverText;

        public Color? PressedText;

        public Color? DisabledText;

        public int BorderThickness = 1;

        public Color BackgroundFor(bool enabled, bool hovered, bool pressed) =>
            !enabled ? DisabledBackground : pressed ? PressedBackground : hovered ? HoverBackground : NormalBackground;

        public Color BorderFor(bool enabled, bool hovered, bool pressed) =>
            !enabled ? DisabledBorder : pressed ? PressedBorder : hovered ? HoverBorder : NormalBorder;

        public Color? TextFor(bool enabled, bool hovered, bool pressed) =>
            !enabled ? DisabledText : pressed ? PressedText : hovered ? HoverText : NormalText;
    }
}
