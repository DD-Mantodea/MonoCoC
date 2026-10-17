using Microsoft.Xna.Framework;
using System;

namespace MonoCoC.UI.Common
{
    public static class ScrollBar
    {
        public const int Thickness = 15;

        public const int MinThumb = 24;

        public static Rectangle Track(Rectangle area) => new(area.Right - Thickness, area.Y, Thickness, Math.Max(1, area.Height));

        public static bool Needed(int contentHeight, int viewHeight) => contentHeight > viewHeight && contentHeight > 0;

        public static Rectangle Thumb(Rectangle area, int contentHeight, int viewHeight, int scroll)
        {
            if (!Needed(contentHeight, viewHeight))
                return Rectangle.Empty;

            var track = Track(area);

            var height = Math.Max(MinThumb, (int)(track.Height * (viewHeight / (float)contentHeight)));

            var travel = Math.Max(0, track.Height - height);

            var max = Math.Max(1, contentHeight - viewHeight);

            var offset = (int)(travel * (Math.Clamp(scroll, 0, max) / (float)max));

            return new Rectangle(track.X + 2, track.Y + offset, Math.Max(1, track.Width - 4), height);
        }

        public static bool Contains(Rectangle area, Vector2 position, int contentHeight, int viewHeight) =>

            Needed(contentHeight, viewHeight) && Track(area).Contains(position);

        public static int ScrollFrom(Rectangle area, int contentHeight, int viewHeight, float mouseY)
        {
            if (!Needed(contentHeight, viewHeight))
                return 0;

            var track = Track(area);

            var height = Thumb(area, contentHeight, viewHeight, 0).Height;

            var travel = Math.Max(1, track.Height - height);

            var max = Math.Max(0, contentHeight - viewHeight);

            var t = Math.Clamp((mouseY - track.Y - height / 2f) / travel, 0f, 1f);

            return (int)MathF.Round(t * max);
        }
    }
}