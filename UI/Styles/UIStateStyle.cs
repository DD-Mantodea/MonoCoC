using Microsoft.Xna.Framework;
using FontStashSharp.RichText;
using MonoCoC.UI.Components;
using System;
using MonoCoC.UI.Framework;
using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;

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

        public Color? NormalLabelColor;

        public Color? HoverLabelColor;

        public Color? PressedLabelColor;

        public Color? DisabledLabelColor;

        public Color? SelectedLabelColor;

        public Color? NormalValueColor;

        public Color? HoverValueColor;

        public Color? PressedValueColor;

        public Color? DisabledValueColor;

        public Color? SelectedValueColor;

        public string FontRole = "coc:menu";

        public int FontSize;

        public string LabelFontRole = "coc:menuBold";

        public int LabelFontSize;

        public int Height;

        public TextVerticalAlignment TextVerticalAlign = TextVerticalAlignment.Center;

        public int TextOffsetY;

        public UIVec4 Padding = 0;

        public UIVec4 Margin = 0;

        public int Spacing;

        public int Columns = 2;

        public int ColumnGap = 36;

        public int RowGap;

        public int? Width;

        public int? HeightValue;

        public int? MinWidth;

        public int? MinHeight;

        public int? MaxWidth;

        public int? MaxHeight;

        public int TextOffsetX;

        public int? LineSpacing;

        public int? WrapWidth;

        public TextHorizontalAlignment? TextHorizontalAlign;

        public bool? Visible;

        public JsonObject? Source;

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

        public Color? LabelFor(bool enabled, bool hovered, bool pressed, bool selected = false) =>
            selected ? SelectedLabelColor : !enabled ? DisabledLabelColor : pressed ? PressedLabelColor : hovered ? HoverLabelColor : NormalLabelColor;

        public Color? ValueFor(bool enabled, bool hovered, bool pressed, bool selected = false) =>
            selected ? SelectedValueColor : !enabled ? DisabledValueColor : pressed ? PressedValueColor : hovered ? HoverValueColor : NormalValueColor;

        public static UIStateStyle Parse(JsonObject? node)
        {
            var style = new UIStateStyle();

            if (node is null)
                return style;

            style.Source = node;

            style.Width = OptionalInt(node, "width");

            style.HeightValue = OptionalInt(node, "height");

            style.MinWidth = OptionalInt(node, "minWidth");

            style.MinHeight = OptionalInt(node, "minHeight");

            style.MaxWidth = OptionalInt(node, "maxWidth");

            style.MaxHeight = OptionalInt(node, "maxHeight");

            style.Spacing = node.Int("spacing", 0);

            style.Columns = Math.Max(1, node.Int("columns", style.Columns));

            style.ColumnGap = node.Int("columnGap", style.ColumnGap);

            style.RowGap = node.Int("rowGap", style.RowGap);

            style.TextOffsetX = node.Int("textOffsetX", 0);

            style.LineSpacing = OptionalInt(node, "lineSpacing");

            style.WrapWidth = OptionalInt(node, "wrapWidth");

            style.Visible = node.TryGetValue("visible", out var visibleValue)
                ? visibleValue.ToString().Trim('"').Equals("true", StringComparison.OrdinalIgnoreCase)
                : null;

            style.TextHorizontalAlign = node.Str("textHorizontalAlign", node.Str("textHAlign")).ToLowerInvariant() switch
            {
                "center" => TextHorizontalAlignment.Center,
                "right" => TextHorizontalAlignment.Right,
                "left" => TextHorizontalAlignment.Left,
                _ => null
            };

            if (node.TryGetValue("margin", out var margin))
                style.Margin = UIParseUtils.ParsePadding(margin);

            style.FontRole = node.Str("font", style.FontRole);

            style.FontSize = node.Int("fontSize", style.FontSize);

            style.LabelFontRole = node.Str("labelFont", style.LabelFontRole);

            style.LabelFontSize = node.Int("labelFontSize", style.LabelFontSize);

            style.Height = node.Int("height", style.Height);

            style.TextOffsetY = node.Int("textOffsetY", style.TextOffsetY);

            if (node.TryGetValue("padding", out var padding))
                style.Padding = UIParseUtils.ParsePadding(padding);

            style.TextVerticalAlign = node.Str("textVerticalAlign", "center").ToLowerInvariant() switch
            {
                "top" => TextVerticalAlignment.Top,
                "bottom" => TextVerticalAlignment.Bottom,
                _ => TextVerticalAlignment.Center
            };

            if (node.TryGetValue("states", out var statesValue) && statesValue is JsonObject states)
            {
                style = ParseStates(states, node.Int("borderThickness", 0));

                style.FontRole = node.Str("font", style.FontRole);

                style.FontSize = node.Int("fontSize", style.FontSize);

                style.Height = node.Int("height", style.Height);
            }
            else
            {
                style.BorderThickness = node.Int("borderThickness", 0);

                style.NormalBorderThickness = style.BorderThickness;
                style.HoverBorderThickness = style.BorderThickness;
                style.PressedBorderThickness = style.BorderThickness;
                style.DisabledBorderThickness = style.BorderThickness;
                style.SelectedBorderThickness = style.BorderThickness;

                if (node.TryGetValue("borderThickness", out var thickness) &&
                    Component.ReadSides(thickness) is { } sides)
                {
                    style.NormalBorderSides = sides;
                    style.HoverBorderSides = sides;
                    style.PressedBorderSides = sides;
                    style.DisabledBorderSides = sides;
                    style.SelectedBorderSides = sides;
                }

                style.NormalBackground = ColorOf(node, "backgroundColor") ?? style.NormalBackground;
                style.HoverBackground = ColorOf(node, "hoverBackgroundColor") ?? style.HoverBackground;
                style.SelectedBackground = ColorOf(node, "selectedBackgroundColor") ?? style.SelectedBackground;
                style.NormalBorder = ColorOf(node, "borderColor") ?? style.NormalBorder;
                style.HoverBorder = ColorOf(node, "hoverBorderColor") ?? style.HoverBorder;
                style.SelectedBorder = ColorOf(node, "selectedBorderColor") ?? style.SelectedBorder;
                style.NormalLabelColor = ColorOf(node, "labelColor");
                style.HoverLabelColor = ColorOf(node, "hoverLabelColor");
                style.SelectedLabelColor = ColorOf(node, "selectedLabelColor");
                style.NormalValueColor = ColorOf(node, "valueColor");
                style.HoverValueColor = ColorOf(node, "hoverValueColor");
                style.SelectedValueColor = ColorOf(node, "selectedValueColor");
            }

            if (node.TryGetValue("borderColor", out _))
            {
                var border = UIParseUtils.ColorOf(node.Str("borderColor"));

                style.NormalBorder = border;

                if (style.HoverBorder == Color.Transparent)
                    style.HoverBorder = border;

                if (style.PressedBorder == Color.Transparent)
                    style.PressedBorder = border;

                if (style.DisabledBorder == Color.Transparent)
                    style.DisabledBorder = border;
            }

            return style;
        }

        public static UIStateStyle ParseStates(JsonObject states, int thickness)
        {
            var style = new UIStateStyle
            {
                BorderThickness = thickness,
                NormalBorderThickness = thickness,
                HoverBorderThickness = thickness,
                PressedBorderThickness = thickness,
                DisabledBorderThickness = thickness,
                SelectedBorderThickness = thickness
            };

            Component.ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
            Component.ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
            Component.ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
            Component.ApplyState(states, "disabled", out style.DisabledBackground, out style.DisabledBorder, out style.DisabledText);
            Component.ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);

            Component.ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
            Component.ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
            Component.ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
            Component.ApplyStateThickness(states, "disabled", ref style.DisabledBorderThickness);
            Component.ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

            style.NormalBorderSides = Component.StateSides(states, "normal");
            style.HoverBorderSides = Component.StateSides(states, "hover");
            style.PressedBorderSides = Component.StateSides(states, "pressed");
            style.DisabledBorderSides = Component.StateSides(states, "disabled");
            style.SelectedBorderSides = Component.StateSides(states, "selected");

            style.NormalLabelColor = LabelColor(states, "normal") ?? style.NormalText;
            style.HoverLabelColor = LabelColor(states, "hover") ?? style.NormalLabelColor;
            style.PressedLabelColor = LabelColor(states, "pressed") ?? style.NormalLabelColor;
            style.DisabledLabelColor = LabelColor(states, "disabled") ?? style.NormalLabelColor;
            style.SelectedLabelColor = LabelColor(states, "selected") ?? style.NormalLabelColor;

            style.NormalValueColor = ValueColor(states, "normal") ?? style.NormalText;
            style.HoverValueColor = ValueColor(states, "hover") ?? style.NormalValueColor;
            style.PressedValueColor = ValueColor(states, "pressed") ?? style.NormalValueColor;
            style.DisabledValueColor = ValueColor(states, "disabled") ?? style.NormalValueColor;
            style.SelectedValueColor = ValueColor(states, "selected") ?? style.NormalValueColor;

            Component.InheritStateFallbacks(states, style);

            return style;
        }

        private static Color? LabelColor(JsonObject states, string name) => StateColor(states, name, "labelColor");

        private static Color? ValueColor(JsonObject states, string name) => StateColor(states, name, "valueColor");

        private static Color? StateColor(JsonObject states, string name, string key) =>
            states.TryGetValue(name, out var value) && value is JsonObject state && state.TryGetValue(key, out _)
                ? UIParseUtils.ColorOf(state.Str(key))
                : null;

        private static Color? ColorOf(JsonObject node, string key) =>
            node.TryGetValue(key, out _) ? UIParseUtils.ColorOf(node.Str(key)) : null;

        private static int? OptionalInt(JsonObject node, string key) =>
            node.TryGetValue(key, out var value) ? UIParseUtils.Int(value) : null;
    }
}
