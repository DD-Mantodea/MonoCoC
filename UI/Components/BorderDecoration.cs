using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hjson;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class BorderDecoration : DecorationComponent, IFromHjson<BorderDecoration> {
        public int Thickness = 1;

        public int Inset;

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var color = PrimaryColor;

            if (color == Color.Transparent || Thickness <= 0)
                return;

            var bounds = Rectangle;

            bounds.Inflate(-Inset, -Inset);

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            var thickness = System.Math.Min(Thickness, System.Math.Min(bounds.Width, bounds.Height) / 2);

            Fill(spriteBatch, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
            Fill(spriteBatch, new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
            Fill(spriteBatch, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
            Fill(spriteBatch, new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
        }
    
        public static string TypeId => "coc:border";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var border = new BorderDecoration();

            if (node.TryGetValue("thickness", out var thickness))
                border.Thickness = UIParseUtils.Int(thickness);

            if (node.TryGetValue("inset", out var inset))
                border.Inset = UIParseUtils.Int(inset);

            return border;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<BorderDecoration>();
    
}
}
