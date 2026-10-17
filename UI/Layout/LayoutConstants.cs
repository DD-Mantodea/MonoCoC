namespace MonoCoC.UI.Layout
{
    /// <summary>
    /// Stable accessors for layout values. Values live in
    /// Assets/UI/Layout.hjson and are loaded by UILayout.
    /// </summary>
    public static class LayoutConstants
    {
        public static int ScreenWidth => UILayout.Current.ScreenWidth;

        public static int ScreenHeight => UILayout.Current.ScreenHeight;

        public static int ButtonWidth => UILayout.Current.ButtonWidth;

        public static int ButtonHeight => UILayout.Current.ButtonHeight;

        public static int ButtonLabelX => ButtonWidth / 2;

        public static int ButtonLabelY => UILayout.Current.ButtonLabelY;

        public static int TextZoneX => UILayout.Current.TextZoneX;

        public static int TextZoneY => UILayout.Current.TextZoneY;

        public static int TextZoneWidth => UILayout.Current.TextZoneWidth;

        public static int TextZoneHeight => UILayout.Current.TextZoneHeight;

        public static int TextZonePadding => UILayout.Current.TextZonePadding;

        public static int CombatStatsX => UILayout.Current.CombatStatsX;

        public static int CombatStatsY => UILayout.Current.CombatStatsY;

        public static int CombatStatsWidth => UILayout.Current.CombatStatsWidth;

        public static int CombatStatsHeight => UILayout.Current.CombatStatsHeight;

        public static int CombatTextZoneWidth => UILayout.Current.CombatTextZoneWidth;

        public static int SlotCount => UILayout.Current.SlotCount;

        public static int BottomColumns => UILayout.Current.BottomColumns;

        public static int BottomRows => UILayout.Current.BottomRows;

        public static int BottomX => UILayout.Current.BottomX;

        public static int BottomY => UILayout.Current.BottomY;

        public static int BottomWidth => UILayout.Current.BottomWidth;

        public static int BottomHeight => UILayout.Current.BottomHeight;

        public static int InputWidth => UILayout.Current.InputWidth;

        public static int InputHeight => UILayout.Current.InputHeight;

        public static int InputNameMaxChars => UILayout.Current.InputNameMaxChars;
    }
}
