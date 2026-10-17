using Hjson;
using MonoCoC.Assets;
using System;
using System.IO;

namespace MonoCoC.UI.Layout
{
    /// <summary>
    /// Runtime layout values loaded from Assets/UI/Layout.hjson.
    /// Existing static LayoutConstants accessors read from this object.
    /// </summary>
    public sealed class UILayout
    {
        private static UILayout? _current;

        public static UILayout Current => _current ??= Load();

        public int ScreenWidth { get; private set; } = 1000;

        public int ScreenHeight { get; private set; } = 800;

        public int ButtonWidth { get; private set; } = 150;

        public int ButtonHeight { get; private set; } = 40;

        public int ButtonLabelY { get; private set; } = 11;

        public string TextFont { get; private set; } = "pala";

        public int TextSize { get; private set; } = 16;

        public int TextZoneX { get; private set; } = 208;

        public int TextZoneY { get; private set; } = 52;

        public int TextZoneWidth { get; private set; } = 770;

        public int TextZoneHeight { get; private set; } = 602;

        public int TextZonePadding { get; private set; } = 16;

        public int CombatStatsX { get; private set; } = 760;

        public int CombatStatsY { get; private set; } = 52;

        public int CombatStatsWidth { get; private set; } = 230;

        public int CombatStatsHeight { get; private set; } = 190;

        public int CombatTextZoneWidth { get; private set; } = 548;

        public int SlotCount { get; private set; } = 15;

        public int BottomColumns { get; private set; } = 5;

        public int BottomRows { get; private set; } = 3;

        public int BottomX { get; private set; } = 211;

        public int BottomY { get; private set; } = 668;

        public int BottomWidth { get; private set; } = 770;

        public int BottomHeight { get; private set; } = 132;

        public int InputWidth { get; private set; } = 165;

        public int InputHeight { get; private set; } = 25;

        public int InputNameMaxChars { get; private set; } = 16;

        public static UILayout Load()
        {
            var layout = new UILayout();

            var path = ResolvePath();

            if (!File.Exists(path))
                return _current = layout;

            var root = HjsonValue.Parse(File.ReadAllText(path)) as JsonObject;

            if (root is null)
                return _current = layout;

            layout.ScreenWidth = Int(root, "screenWidth", layout.ScreenWidth);
            layout.ScreenHeight = Int(root, "screenHeight", layout.ScreenHeight);
            layout.ButtonWidth = Int(root, "buttonWidth", layout.ButtonWidth);
            layout.ButtonHeight = Int(root, "buttonHeight", layout.ButtonHeight);
            layout.ButtonLabelY = Int(root, "buttonLabelY", layout.ButtonLabelY);
            layout.TextFont = Str(root, "textFont", layout.TextFont);
            layout.TextSize = Int(root, "textSize", layout.TextSize);
            layout.TextZoneX = Int(root, "textZoneX", layout.TextZoneX);
            layout.TextZoneY = Int(root, "textZoneY", layout.TextZoneY);
            layout.TextZoneWidth = Int(root, "textZoneWidth", layout.TextZoneWidth);
            layout.TextZoneHeight = Int(root, "textZoneHeight", layout.TextZoneHeight);
            layout.TextZonePadding = Int(root, "textZonePadding", layout.TextZonePadding);
            layout.CombatStatsX = Int(root, "combatStatsX", layout.CombatStatsX);
            layout.CombatStatsY = Int(root, "combatStatsY", layout.CombatStatsY);
            layout.CombatStatsWidth = Int(root, "combatStatsWidth", layout.CombatStatsWidth);
            layout.CombatStatsHeight = Int(root, "combatStatsHeight", layout.CombatStatsHeight);
            layout.CombatTextZoneWidth = Int(root, "combatTextZoneWidth", layout.CombatTextZoneWidth);
            layout.SlotCount = Int(root, "slotCount", layout.SlotCount);
            layout.BottomColumns = Int(root, "bottomColumns", layout.BottomColumns);
            layout.BottomRows = Int(root, "bottomRows", layout.BottomRows);
            layout.BottomX = Int(root, "bottomX", layout.BottomX);
            layout.BottomY = Int(root, "bottomY", layout.BottomY);
            layout.BottomWidth = Int(root, "bottomWidth", layout.BottomWidth);
            layout.BottomHeight = Int(root, "bottomHeight", layout.BottomHeight);
            layout.InputWidth = Int(root, "inputWidth", layout.InputWidth);
            layout.InputHeight = Int(root, "inputHeight", layout.InputHeight);
            layout.InputNameMaxChars = Int(root, "inputNameMaxChars", layout.InputNameMaxChars);

            return _current = layout;
        }

        private static string Str(JsonObject root, string key, string fallback) =>
            root.TryGetValue(key, out var value) ? value.ToString().Trim('"') : fallback;

        private static int Int(JsonObject root, string key, int fallback) =>
            root.TryGetValue(key, out var value) &&
            int.TryParse(value.ToString(), out var result)
                ? result
                : fallback;

        private static string ResolvePath()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "MonoCoC.csproj")))
                {
                    var source = Path.Combine(directory.FullName, "Assets", "UI", "Layout.hjson");

                    if (File.Exists(source))
                        return source;
                }

                directory = directory.Parent;
            }

            return Path.Combine(Paths.AssetPath("UI"), "Layout.hjson");
        }
    }
}
