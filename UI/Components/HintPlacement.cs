using Microsoft.Xna.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    public static class HintPlacement
    {
        /// <summary>
        /// Resolves the hint rectangle from the button rectangle.
        /// direction: top / bottom / left / right / auto.
        /// offsetX / offsetY shift the selected side in screen axes;
        /// distance is the gap between the button and the hint.
        /// </summary>
        public static Vector2 Resolve(HintStyle style, Rectangle anchor, int width, int height)
        {
            var direction = string.IsNullOrWhiteSpace(style.Direction)
                ? "auto"
                : style.Direction.Trim().ToLowerInvariant();

            if (direction == "auto")
                direction = anchor.Bottom < LayoutConstants.ScreenHeight / 2 ? "bottom" : "top";

            var distance = style.Distance;

            var x = direction switch
            {
                "top" or "bottom" => anchor.X,
                "left" => anchor.X - width - distance,
                "right" => anchor.Right + distance,
                _ => anchor.X
            };

            var y = direction switch
            {
                "top" => anchor.Y - height - distance,
                "bottom" => anchor.Bottom + distance,
                "left" or "right" => anchor.Y,
                _ => anchor.Bottom + distance
            };

            x += style.OffsetX;

            y += style.OffsetY;

            x = Math.Clamp(x, 0, Math.Max(0, LayoutConstants.ScreenWidth - width));

            y = Math.Clamp(y, 0, Math.Max(0, LayoutConstants.ScreenHeight - height));

            return new Vector2(x, y);
        }
    }
}
