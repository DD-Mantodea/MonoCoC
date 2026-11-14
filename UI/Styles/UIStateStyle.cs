using Microsoft.Xna.Framework;

namespace MonoCoC.UI.Styles
{
    /// <summary>
    /// Optional Normal/Hover/Pressed/Disabled visual state colors for a
    /// Component. Border is drawn after children so it stays visible above
    /// material overlays and labels.
    /// </summary>
    public readonly record struct BorderSides(int Left, int Top, int Right, int Bottom)
    {
        public static BorderSides Uniform(int thickness) => new(thickness, thickness, thickness, thickness);
    }

    public class UIStateStyle
    {
        public Color NormalBackground = Color.Transparent;

        public Color HoverBackground = Color.Transparent;

        public Color PressedBackground = Color.Transparent;

        public Color DisabledBackground = Color.Transparent;

        public Color NormalBorder = Color.Transparent;

        public Color SelectedBackground = Color.Transparent;

        public Color SelectedBorder = Color.Transparent;

        public Color? SelectedText;

        public Color HoverBorder = Color.Transparent;

        public Color PressedBorder = Color.Transparent;

        public Color DisabledBorder = Color.Transparent;

        public Color? NormalText;

        public Color? HoverText;

        public Color? PressedText;

        public Color? DisabledText;

        public int NormalBorderThickness = 1;

        public int HoverBorderThickness = 1;

        public int PressedBorderThickness = 1;

        public int DisabledBorderThickness = 1;

        public int SelectedBorderThickness = 1;

        public int BorderThickness = 1;

        public BorderSides? NormalBorderSides;
        public BorderSides? HoverBorderSides;
        public BorderSides? PressedBorderSides;
        public BorderSides? DisabledBorderSides;
        public BorderSides? SelectedBorderSides;

        public Color BackgroundFor(bool enabled, bool hovered, bool pressed, bool selected = false) =>
            selected ? SelectedBackground : !enabled ? DisabledBackground : pressed ? PressedBackground : hovered ? HoverBackground : NormalBackground;

        public Color BorderFor(bool enabled, bool hovered, bool pressed, bool selected = false) =>
            selected ? SelectedBorder : !enabled ? DisabledBorder : pressed ? PressedBorder : hovered ? HoverBorder : NormalBorder;

        public int ThicknessFor(bool enabled, bool hovered, bool pressed, bool selected = false) =>
            selected ? SelectedBorderThickness : !enabled ? DisabledBorderThickness : pressed ? PressedBorderThickness : hovered ? HoverBorderThickness : NormalBorderThickness;

        public BorderSides SidesFor(bool enabled, bool hovered, bool pressed, bool selected = false)
        {
            var sides = selected ? SelectedBorderSides :
                !enabled ? DisabledBorderSides :
                pressed ? PressedBorderSides :
                hovered ? HoverBorderSides : NormalBorderSides;

            return sides ?? BorderSides.Uniform(ThicknessFor(enabled, hovered, pressed, selected));
        }

        public Color? TextFor(bool enabled, bool hovered, bool pressed, bool selected = false) =>
            selected ? SelectedText : !enabled ? DisabledText : pressed ? PressedText : hovered ? HoverText : NormalText;
    }
}
