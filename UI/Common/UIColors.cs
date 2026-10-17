using Microsoft.Xna.Framework;

namespace MonoCoC.UI.Common
{
    public static class UIColors
    {
        public static readonly Color ScrollTrack = new Color(90, 62, 32) * 0.12f;

        public static readonly Color ScrollTrackBorder = new Color(90, 62, 32) * 0.35f;

        public static readonly Color ScrollThumb = new Color(122, 88, 48) * 0.75f;

        public static readonly Color ScrollThumbBorder = new Color(58, 40, 20) * 0.85f;

        public static readonly Color ScrollThumbHighlight = new Color(255, 240, 210) * 0.5f;

        public static readonly Color InputBox = Color.White;

        public static readonly Color InputBorder = Color.Black;

        public static readonly Color InputText = Color.Black;

        public static readonly Color InputCaret = Color.Black;

        public static readonly Color InputSelection = new Color(110, 140, 180) * 0.45f;

        public static readonly Color SearchBackground = new Color(238, 228, 205);

        public static readonly Color SearchBorder = new Color(92, 65, 38);

        public static readonly Color SearchHint = new Color(125, 105, 78);

        public static readonly Color SearchText = new Color(40, 30, 20);

        public static readonly Color ButtonTextDisabled = new(90, 80, 70);

        public static readonly Color ToolTipText = Color.Black;

        public static readonly Color ToolTipBorder = Color.Black;
    }
}