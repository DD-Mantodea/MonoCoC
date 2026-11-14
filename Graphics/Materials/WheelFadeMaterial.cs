using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Shaders;
using System;

namespace MonoCoC.Graphics.Materials
{
    public class WheelFadeMaterial : Material
    {
        public Color FadeColor = new Color(0xE6, 0xD7, 0xBD, 255);

        public float FadeTop = 0.2f;

        public float FadeBottom = 0.2f;

        public WheelFadeMaterial(GraphicsDevice graphicsDevice, Effect effect) : base(graphicsDevice, effect)
        {
            ListenShader("WheelFade.fx");
        }

        protected override void ApplyParameters(Rectangle bounds)
        {
            Parameter("FadeColor")?.SetValue(FadeColor.ToVector4());
            Parameter("FadeTop")?.SetValue(FadeTop);
            Parameter("FadeBottom")?.SetValue(FadeBottom);
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle bounds, Color? tint = null)
        {
            ApplyParameters(bounds);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                Effect);

            spriteBatch.Draw(SpriteBatchExt.Pixel, bounds, tint ?? Color.White);

            spriteBatch.End();
        }
    }
}
