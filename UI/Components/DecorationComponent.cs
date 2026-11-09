using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Styles;

namespace MonoCoC.UI.Components
{
    /// <summary>
    /// Base for non-interactive template decorations. A decoration follows
    /// the nearest Button's state, but owns its own four-state colors.
    /// </summary>
    public abstract class DecorationComponent : Component
    {
        public UIDecorationStyle Style = new();

        public Component? StateSource;

        public DecorationComponent()
        {
            CanHit = false;
        }

        protected (bool Enabled, bool Hovered, bool Pressed) ResolveState()
        {
            var source = StateSource ?? FindStateSource(LayoutData.Parent);

            if (source is null)
                return (Enabled, false, false);

            return (source.Enabled, source.IsHovering, source.IsPressed);
        }

        protected Color PrimaryColor
        {
            get
            {
                var state = ResolveState();

                return Style.PrimaryFor(state.Enabled, state.Hovered, state.Pressed) * Alpha;
            }
        }

        protected Color SecondaryColor
        {
            get
            {
                var state = ResolveState();

                return Style.SecondaryFor(state.Enabled, state.Hovered, state.Pressed) * Alpha;
            }
        }

        protected void Fill(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            if (rectangle.Width > 0 && rectangle.Height > 0 && color != Color.Transparent)
                spriteBatch.Draw(SpriteBatchExt.Pixel, rectangle, color);
        }

        private static Component? FindStateSource(Component? component)
        {
            for (var current = component; current is not null; current = current.LayoutData.Parent)
                if (current is Button)
                    return current;

            return null;
        }
    }
}
