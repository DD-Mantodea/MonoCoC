using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class UIImage(Texture2D texture) : Component, IUIStatefulPart
    {
        private sealed record State(Texture2D? Texture, Color Color, bool Visible);

        public Texture2D Texture = texture;

        public Color Color = Color.White;

        public float Rotation;

        public object? CaptureState() => new State(Texture, Color, Visible);

        public void RestoreState(object value)
        {
            if (value is not State state)
                return;

            Texture = state.Texture;

            Color = state.Color;

            Visible = state.Visible;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Texture == null)
                return;

            var destination = Position.BuildRect(Width, Height);

            if (Rotation == 0)
            {
                spriteBatch.Draw(Texture, destination, Color * Alpha);

                return;
            }

            var center = new Vector2(destination.Center.X, destination.Center.Y);

            var scale = new Vector2((float)Width / Texture.Width, (float)Height / Texture.Height);

            spriteBatch.Draw(Texture, center, null, Color * Alpha, Rotation,

                new Vector2(Texture.Width / 2f, Texture.Height / 2f), scale, SpriteEffects.None, 0f);
        }
    }
}