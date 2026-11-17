using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MonoCoC.UI.Common
{
    // 代码构造组件的样式表：数据在 Assets/UI/Components.hjson。
    // 组件只按名字取值；缺键时用这里写的 fallback，并在控制台留一条 warning。
    public static class UIComponentStyles
    {
        private static readonly Dictionary<string, JsonObject> Blocks = [];

        private static bool _loaded;

        public static class ScrollBar
        {
            public static int Thickness => Int("scrollBar", "thickness", 15);

            public static int MinThumb => Int("scrollBar", "minThumb", 24);

            public static Color Track => ColorOf("scrollBar", "track", "#ECE0C4FF");

            public static Color Thumb => ColorOf("scrollBar", "thumb", "#B79A70FF");

            public static int RevealDistance => Int("scrollBar", "revealDistance", 28);

            public static float FadeInSpeed => Float("scrollBar", "fadeInSpeed", 8);

            public static float FadeOutSpeed => Float("scrollBar", "fadeOutSpeed", 4);
        }

        public static class TextBox
        {
            public static Color Text => ColorOf("textBox", "text", "#000000FF");

            public static Color Border => ColorOf("textBox", "border", "#000000FF");

            public static int BorderThickness => Int("textBox", "borderThickness", 1);

            public static Color Cursor => ColorOf("textBox", "cursor", "#000000FF");

            public static Color Selection => ColorOf("textBox", "selection", "#31405172");
        }

        public static class SearchBar
        {
            public static Color Background => ColorOf("searchBar", "background", "#EEE4CDFF");

            public static Color Border => ColorOf("searchBar", "border", "#5C4126FF");

            public static int BorderThickness => Int("searchBar", "borderThickness", 1);

            public static Color Hint => ColorOf("searchBar", "hint", "#7D694EFF");

            public static Color Text => ColorOf("searchBar", "text", "#281E14FF");
        }

        public static class ToolTip
        {
            public static Color Text => ColorOf("toolTip", "text", "#000000FF");

            public static Color Separator => ColorOf("toolTip", "separator", "#000000FF");
        }

        public static class CombatStats
        {
            public static Color HpFill => ColorOf("combatStats", "hpFill", "#B17D5EFF");

            public static Color LustFill => ColorOf("combatStats", "lustFill", "#880101FF");

            public static Color FatigueFill => ColorOf("combatStats", "fatigueFill", "#600000FF");

            public static Color Separator => ColorOf("combatStats", "separator", "#600000FF");
        }

        public static class Slot
        {
            public static Color Background => ColorOf("slot", "background", "#F7EFE0CC");

            public static Color Border => ColorOf("slot", "border", "#B79A70AA");

            public static Color Text => ColorOf("slot", "text", "#35291FFF");

            public static Color SelectedBackground => ColorOf("slot", "selectedBackground", "#E0C9AAFF");

            public static Color SelectedBorder => ColorOf("slot", "selectedBorder", "#9F8156FF");
        }

        public static class NumberField
        {
            public static Color Background => ColorOf("numberField", "background", "#F8F0DCCC");

            public static Color Text => ColorOf("numberField", "text", "#382B22FF");

            public static Color SecondaryText => ColorOf("numberField", "secondaryText", "#5B4A3AFF");
        }

        public static class NumericStepper
        {
            public static Color Selection => ColorOf("numericStepper", "selection", "#B79A7078");
        }

        public static class OptionList
        {
            public static Color Background => ColorOf("optionList", "background", "#F7EFE0CC");

            public static Color Text => ColorOf("optionList", "text", "#35291FFF");

            public static Color SelectedBackground => ColorOf("optionList", "selectedBackground", "#F2E1BEFF");
        }

        public static class Selector
        {
            public static Color Background => ColorOf("selector", "background", "#F5EBDDCC");

            public static Color Text => ColorOf("selector", "text", "#382B22FF");
        }

        public static class Slider
        {
            public static Color Text => ColorOf("slider", "text", "#382B22FF");
        }

        public static class ChoiceWheel
        {
            public static Color Fade => ColorOf("choiceWheel", "fade", "#E6D7BDFF");

            public static Color Lock => ColorOf("choiceWheel", "lock", "#8A857CFF");
        }

        public static class DungeonMap
        {
            public static Color Background => ColorOf("dungeonMap", "background", "#000000BE");
        }

        public static class DungeonMiniMap
        {
            public static Color Background => ColorOf("dungeonMiniMap", "background", "#0A0A0AA0");
        }

        public static class Dropdown
        {
            public static Color HeaderText => ColorOf("dropdown", "headerText", "#35291FFF");

            public static Color ListBackground => ColorOf("dropdown", "listBackground", "#F7EFE0FF");

            public static Color ItemText => ColorOf("dropdown", "itemText", "#35291FFF");

            public static UIStateStyle Header => StateStyle("dropdownHeader");
        }

        public static class SettingsChip
        {
            public static UIStateStyle Style => BuildStateStyle("settingsChip");
        }

        public static class KeyChip
        {
            public static Color Fill => ColorOf("keyChip", "fill", "#D5D8B8FF");

            public static Color FillHover => ColorOf("keyChip", "fillHover", "#DDDFC4FF");

            public static Color Border => ColorOf("keyChip", "border", "#858B62FF");

            public static int BorderThickness => Int("keyChip", "borderThickness", 2);

            public static Color Text => ColorOf("keyChip", "text", "#2F241BFF");

            public static Color EmptyFill => ColorOf("keyChip", "emptyFill", "#00000000");

            public static Color EmptyFillHover => ColorOf("keyChip", "emptyFillHover", "#EFE3CCFF");

            public static Color EmptyBorder => ColorOf("keyChip", "emptyBorder", "#B79A708C");

            public static int EmptyBorderThickness => Int("keyChip", "emptyBorderThickness", 1);

            public static Color EmptyText => ColorOf("keyChip", "emptyText", "#7C6B58CC");
        }

        // 命名状态样式：块内可以直接写 UIStateStyle 字段名（normalText/selectedBackground…），
        // 也可以写 normal/hover/pressed/selected/disabled 子块，键用 background/border/borderThickness/text。
        public static UIStateStyle StateStyle(string name)
        {
            if (StateStyles.TryGetValue(name, out var cached))
                return cached;

            var style = BuildStateStyle(name);

            StateStyles[name] = style;

            return style;
        }

        private static readonly Dictionary<string, UIStateStyle> StateStyles = [];

        private static UIStateStyle BuildStateStyle(string name)
        {
            var style = new UIStateStyle();

            var root = Block(name);

            if (root is null)
                return style;

            foreach (var pair in root)
            {
                if (pair.Value is JsonObject nested)
                {
                    if (pair.Key is not ("normal" or "hover" or "pressed" or "selected" or "disabled"))
                        continue;

                    foreach (var entry in nested)
                        ApplyStateField(style, pair.Key, entry.Key, entry.Value);

                    continue;
                }

                ApplyFlatField(style, pair.Key, pair.Value);
            }

            return style;
        }

        // 显式映射，不用反射：键名大小写一变就静默失配（之前 settingsChip 的填充/描边整块丢失就是这么来的）。
        private static int IntOf(JsonValue value) =>
            int.TryParse(value.ToString(), out var number) ? number : 0;

        private static void ApplyFlatField(UIStateStyle style, string key, JsonValue value)
        {
            switch (key)
            {
                case "normalBackground": style.NormalBackground = ParseColor(value.ToString()); break;
                case "hoverBackground": style.HoverBackground = ParseColor(value.ToString()); break;
                case "pressedBackground": style.PressedBackground = ParseColor(value.ToString()); break;
                case "disabledBackground": style.DisabledBackground = ParseColor(value.ToString()); break;
                case "normalBorder": style.NormalBorder = ParseColor(value.ToString()); break;
                case "hoverBorder": style.HoverBorder = ParseColor(value.ToString()); break;
                case "pressedBorder": style.PressedBorder = ParseColor(value.ToString()); break;
                case "disabledBorder": style.DisabledBorder = ParseColor(value.ToString()); break;
                case "selectedBackground": style.SelectedBackground = ParseColor(value.ToString()); break;
                case "selectedBorder": style.SelectedBorder = ParseColor(value.ToString()); break;
                case "selectedText": style.SelectedText = ParseColor(value.ToString()); break;
                case "normalText": style.NormalText = ParseColor(value.ToString()); break;
                case "hoverText": style.HoverText = ParseColor(value.ToString()); break;
                case "pressedText": style.PressedText = ParseColor(value.ToString()); break;
                case "disabledText": style.DisabledText = ParseColor(value.ToString()); break;
                case "borderThickness": style.BorderThickness = IntOf(value); break;
                case "normalBorderThickness": style.NormalBorderThickness = IntOf(value); break;
                case "hoverBorderThickness": style.HoverBorderThickness = IntOf(value); break;
                case "pressedBorderThickness": style.PressedBorderThickness = IntOf(value); break;
                case "disabledBorderThickness": style.DisabledBorderThickness = IntOf(value); break;
                case "selectedBorderThickness": style.SelectedBorderThickness = IntOf(value); break;
            }
        }

        private static void ApplyStateField(UIStateStyle style, string state, string key, JsonValue value)
        {
            switch (state + "." + key)
            {
                case "normal.background": style.NormalBackground = ParseColor(value.ToString()); break;
                case "normal.border": style.NormalBorder = ParseColor(value.ToString()); break;
                case "normal.borderThickness": style.NormalBorderThickness = IntOf(value); break;
                case "normal.text": style.NormalText = ParseColor(value.ToString()); break;

                case "hover.background": style.HoverBackground = ParseColor(value.ToString()); break;
                case "hover.border": style.HoverBorder = ParseColor(value.ToString()); break;
                case "hover.borderThickness": style.HoverBorderThickness = IntOf(value); break;
                case "hover.text": style.HoverText = ParseColor(value.ToString()); break;

                case "pressed.background": style.PressedBackground = ParseColor(value.ToString()); break;
                case "pressed.border": style.PressedBorder = ParseColor(value.ToString()); break;
                case "pressed.borderThickness": style.PressedBorderThickness = IntOf(value); break;
                case "pressed.text": style.PressedText = ParseColor(value.ToString()); break;

                case "selected.background": style.SelectedBackground = ParseColor(value.ToString()); break;
                case "selected.border": style.SelectedBorder = ParseColor(value.ToString()); break;
                case "selected.borderThickness": style.SelectedBorderThickness = IntOf(value); break;
                case "selected.text": style.SelectedText = ParseColor(value.ToString()); break;

                case "disabled.background": style.DisabledBackground = ParseColor(value.ToString()); break;
                case "disabled.border": style.DisabledBorder = ParseColor(value.ToString()); break;
                case "disabled.borderThickness": style.DisabledBorderThickness = IntOf(value); break;
                case "disabled.text": style.DisabledText = ParseColor(value.ToString()); break;
            }
        }

        // "#RRGGBB" / "#RRGGBBAA"
        public static Color ParseColor(string value)
        {
            value = (value ?? "").Trim().Trim('"');

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

        private static JsonObject? Block(string name)
        {
            EnsureLoaded();

            return Blocks.GetValueOrDefault(name);
        }

        private static string Str(string block, string key, string fallback)
        {
            var root = Block(block);

            if (root is null || !root.TryGetValue(key, out var value) || value is null)
                return fallback;

            return value.ToString().Trim('"');
        }

        private static int Int(string block, string key, int fallback) =>
            int.TryParse(Str(block, key, ""), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? value
                : fallback;

        private static Color ColorOf(string block, string key, string fallback) =>
            ParseColor(Str(block, key, fallback));

        private static float Float(string block, string key, float fallback) =>
            float.TryParse(Str(block, key, ""), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : fallback;

        private static void EnsureLoaded()
        {
            if (_loaded)
                return;

            _loaded = true;

            try
            {
                var path = Path.Combine(Paths.AssetPath("UI"), "GlobalStyles.hjson");

                if (!File.Exists(path))
                {
                    UI.Console.ConsoleOverlay.WriteWarning("[UIComponentStyles] 找不到 " + path);

                    return;
                }

                var root = (JsonObject)MonoCoC.HjsonParser.Parse(File.ReadAllText(path));

                if (!root.TryGetValue("components", out var node) || node is not JsonObject components)
                {
                    UI.Console.ConsoleOverlay.WriteWarning("[UIComponentStyles] GlobalStyles.hjson 缺少 components 节点");

                    return;
                }

                foreach (var pair in components)
                    if (pair.Value is JsonObject block)
                        Blocks[pair.Key] = block;
            }
            catch (Exception exception)
            {
                UI.Console.ConsoleOverlay.WriteWarning("[UIComponentStyles] 读取失败: " + exception.Message);
            }
        }
    }
}
