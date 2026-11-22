using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hjson;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class CornerDecoration : DecorationComponent, IFromHjson<CornerDecoration> {
        public int Length = 14;

        public int Thickness = 2;

        public int Inset;

        public int Gap = 2;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var bounds = Rectangle;
            bounds.Inflate(-Inset, -Inset);

            if (bounds.Width <= 0 || bounds.Height <= 0 || Length <= 0 || Thickness <= 0)
                return;

            var length = System.Math.Min(Length, System.Math.Min(bounds.Width, bounds.Height) / 2);

            DrawCorner(spriteBatch, bounds, length, Thickness, PrimaryColor, true, true);
            DrawCorner(spriteBatch, bounds, length, Thickness, PrimaryColor, false, true);
            DrawCorner(spriteBatch, bounds, length, Thickness, PrimaryColor, true, false);
            DrawCorner(spriteBatch, bounds, length, Thickness, PrimaryColor, false, false);

            var innerInset = Thickness + Gap;
            var inner = new Rectangle(
                bounds.X + innerInset,
                bounds.Y + innerInset,
                bounds.Width - innerInset * 2,
                bounds.Height - innerInset * 2);

            if (inner.Width <= 0 || inner.Height <= 0)
                return;

            var innerLength = System.Math.Min(length - innerInset, System.Math.Min(inner.Width, inner.Height) / 2);

            DrawCorner(spriteBatch, inner, innerLength, Thickness, SecondaryColor, true, true);
            DrawCorner(spriteBatch, inner, innerLength, Thickness, SecondaryColor, false, true);
            DrawCorner(spriteBatch, inner, innerLength, Thickness, SecondaryColor, true, false);
            DrawCorner(spriteBatch, inner, innerLength, Thickness, SecondaryColor, false, false);
        }

        private void DrawCorner(
            SpriteBatch spriteBatch,
            Rectangle bounds,
            int length,
            int thickness,
            Color color,
            bool left,
            bool top)
        {
            if (length <= 0 || color == Color.Transparent)
                return;

            var x = left ? bounds.X : bounds.Right - length;
            var y = top ? bounds.Y : bounds.Bottom - thickness;

            Fill(spriteBatch, new Rectangle(x, y, length, thickness), color);

            var verticalX = left ? bounds.X : bounds.Right - thickness;
            var verticalY = top ? bounds.Y : bounds.Bottom - length;

            Fill(spriteBatch, new Rectangle(verticalX, verticalY, thickness, length), color);
        }
    
        public static string TypeId => "coc:corners";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var corners = new CornerDecoration();

            if (node.TryGetValue("length", out var length))
                corners.Length = UIParseUtils.Int(length);

            if (node.TryGetValue("thickness", out var thickness))
                corners.Thickness = UIParseUtils.Int(thickness);

            if (node.TryGetValue("inset", out var inset))
                corners.Inset = UIParseUtils.Int(inset);

            if (node.TryGetValue("gap", out var gap))
                corners.Gap = UIParseUtils.Int(gap);

            return corners;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<CornerDecoration>();
    
}
}
