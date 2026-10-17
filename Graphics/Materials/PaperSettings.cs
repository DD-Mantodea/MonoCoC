using Microsoft.Xna.Framework;

namespace MonoCoC.Graphics.Materials
{
    public struct PaperSettings
    {
        public Color CenterColor;

        public Color EdgeColor;

        public float EdgeStrength;

        public float LowNoiseStrength;

        public float FiberStrength;

        public static PaperSettings Default => new()
        {
            CenterColor = new Color(242, 229, 199),
            EdgeColor = new Color(208, 180, 135),

            EdgeStrength = 0.55f,
            LowNoiseStrength = 0.035f,
            FiberStrength = 0.018f
        };

        public static PaperSettings Clean => new()
        {
            CenterColor = new Color(245, 235, 211),
            EdgeColor = new Color(229, 211, 175),

            EdgeStrength = 0.25f,
            LowNoiseStrength = 0.015f,
            FiberStrength = 0.008f
        };

        public static PaperSettings Aged => new()
        {
            CenterColor = new Color(231, 210, 171),
            EdgeColor = new Color(185, 151, 104),

            EdgeStrength = 0.8f,
            LowNoiseStrength = 0.055f,
            FiberStrength = 0.025f
        };
    }
}
