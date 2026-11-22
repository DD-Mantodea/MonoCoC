using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Extensions;
using MonoCoC.UI.Components;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;
using System;
using System.Globalization;

namespace MonoCoC.UI.Data
{
    public static class UIParseUtils
    {
        public static UIVec4 ParsePadding(JsonValue value)
        {
            if (value is JsonArray array && array.Count >= 4)
                return new UIVec4(Int(array[0]), Int(array[1]), Int(array[2]), Int(array[3]));

            if (value is JsonObject obj)
                return new UIVec4(
                    Int(obj.Str("top")),
                    Int(obj.Str("left")),
                    Int(obj.Str("right")),
                    Int(obj.Str("bottom")));

            return new UIVec4(Int(value));
        }

        public static AlignHorizontal ParseHorizontalAlign(string value) =>
            value.Trim().ToLowerInvariant() switch
            {
                "center" => AlignHorizontal.Center,
                "right" => AlignHorizontal.Right,
                "stretch" => AlignHorizontal.Stretch,
                "left" => AlignHorizontal.Left,
                _ => ReportInvalidHorizontal(value)
            };

        public static AlignVertical ParseVerticalAlign(string value) =>
            value.Trim().ToLowerInvariant() switch
            {
                "center" => AlignVertical.Center,
                "bottom" => AlignVertical.Bottom,
                "stretch" => AlignVertical.Stretch,
                "top" => AlignVertical.Top,
                _ => ReportInvalidVertical(value)
            };

        public static AlignHorizontal ReportInvalidHorizontal(string value)
        {
            ReportAlignmentError("horizontalAlign", value);

            return AlignHorizontal.None;
        }

        public static AlignVertical ReportInvalidVertical(string value)
        {
            ReportAlignmentError("verticalAlign", value);

            return AlignVertical.None;
        }

        public static void ReportAlignmentError(string field, string value) =>
            MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                $"[UI] Invalid {field} '{value}', using default.");

        public static TextHorizontalAlignment ReportInvalidTextHorizontal(string value)
        {
            ReportAlignmentError("textHorizontalAlign", value);

            return TextHorizontalAlignment.Left;
        }

        public static TextVerticalAlignment ReportInvalidTextVertical(string value)
        {
            ReportAlignmentError("textVerticalAlign", value);

            return TextVerticalAlignment.Top;
        }

        public static LayoutType ParseLayoutType(string value) =>
            string.Equals(value, "relative", StringComparison.OrdinalIgnoreCase)
                ? LayoutType.Relative
                : LayoutType.Absolute;

        public static SizeType ParseSizeType(string value) =>
            string.Equals(value, "fill", StringComparison.OrdinalIgnoreCase)
                ? SizeType.Fill
                : SizeType.Common;

        public static CommonDataType ParseAlphaType(string value) =>
            string.Equals(value, "inherit", StringComparison.OrdinalIgnoreCase)
                ? CommonDataType.Inherit
                : CommonDataType.Independent;

        public static int Int(JsonValue value) =>
            int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : 0;

        public static float Float(JsonValue value) =>
            float.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : 0f;

        public static Color ColorOf(string value)
        {
            value = value.Trim();

            if (value.Length == 0 || value[0] != '#')
                return Color.Transparent;

            var hex = value[1..];

            if (hex.Length == 6)
                hex += "FF";

            if (hex.Length != 8 ||
                !uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgba))
                return Color.Transparent;

            return new Color(
                (byte)((rgba >> 24) & 0xFF),
                (byte)((rgba >> 16) & 0xFF),
                (byte)((rgba >> 8) & 0xFF),
                (byte)(rgba & 0xFF));
        }

        public static UIBinding? ReadBinding(JsonObject node, string name)
        {
            if (!node.TryGetValue(name, out var value) || value is not JsonObject binding)
                return null;

            if (!binding.TryGetValue("bind", out var path))
                return null;

            return new UIBinding(path.ToString().Trim('"'));
        }

        public static string EventId(JsonObject node, string primary, string? fallback = null)
        {
            var command = node.Str(primary);

            if (command.Length == 0 && fallback is not null)
                command = node.Str(fallback);

            return command;
        }
    }
}
