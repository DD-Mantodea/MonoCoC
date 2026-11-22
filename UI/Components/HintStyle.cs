using Microsoft.Xna.Framework;
using MonoCoC.UI.Common;

namespace MonoCoC.UI.Components
{
    /// <summary>
    /// Per-button hint menu configuration.
    /// </summary>
    public class HintStyle
    {
        public int Width = 350;

        public int MinHeight = 239;

        public int Padding = 15;

        public int HeaderY = 15;

        public int HeaderHeight = 25;

        public int SeparatorY = 40;

        public int BodyY = 44;

        public int SeparatorWidth = 320;

        public string Direction = "auto";

        public int Distance;

        public int OffsetX;

        public int OffsetY;

        public string HeaderFont = "coc:tooltipHeader";

        public string BodyFont = "coc:tooltipBody";

        public string BackgroundTexture = "";

        public Color BackgroundColor = Color.White;

        public float Alpha = 1f;

        public Color BorderColor = Color.Transparent;

        public int BorderThickness;

        public Color HeaderColor = new(0x00, 0x00, 0x00);

        public Color BodyColor = new(0x00, 0x00, 0x00);

        public Color SeparatorColor = new(0x00, 0x00, 0x00);

        public static HintStyle Default { get; } = new();
    }
}
