using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoCoC.Graphics.Materials
{
    /// <summary>
    /// Procedural parchment background. The shader owns the paper look;
    /// this class only feeds colors, edge/noise strengths and the rect size.
    /// </summary>
    public sealed class PaperMaterial : Material
    {
        public PaperMaterial(GraphicsDevice graphicsDevice, Effect effect)
            : base(graphicsDevice, effect)
        {
            ListenShader("Paper.fx");
        }

        public PaperSettings Settings { get; set; } = PaperSettings.Default;

        /// <summary>
        /// Draw a procedural parchment background.
        /// Must be called outside an active SpriteBatch.Begin/End block.
        /// </summary>
        public void DrawBackground(SpriteBatch spriteBatch, Rectangle bounds) =>
            Draw(spriteBatch, bounds);

        protected override void ApplyParameters(Rectangle bounds)
        {
            var settings = Settings;
        }

        protected override void OnSpriteBatchBegun(SpriteBatch spriteBatch, Rectangle bounds)
        {
            var viewport = GraphicsDevice.Viewport;
        }
    }
}
