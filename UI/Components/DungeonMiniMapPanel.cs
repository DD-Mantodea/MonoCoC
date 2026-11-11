using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    // 鍘熺増 MinimapView.minidungeonMap锛?2px 鐡︾墖锛堝師鐗?TILE_WIDTH * 0.8锛夛紝
    // 浠ョ帺瀹朵负涓績瑁佸垏锛屽簳鏉跨敤 minimapBackground銆?
    public class DungeonMiniMapPanel : Component
    {
        private const int TileSize = 32;

        private DungeonContext? _context;

        private Texture2D? _panel;

        public DungeonMiniMapPanel()
        {
            BackgroundColor = new Color(10, 10, 10, 160);

            Visible = false;

            CanHit = false;
        }

        public void Show(DungeonContext context)
        {
            _context = context;

            Visible = true;
        }

        public void Hide()
        {
            _context = null;

            Visible = false;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            if (_context == null || !Visible)
                return;

            var textures = ManagerInstance<TextureManager>.Instance;

            _panel ??= textures["minimapBackground"];

            if (_panel != null)
                spriteBatch.Draw(_panel, Position.BuildRect(Width, Height), Color.White * Alpha);

            // 浠ョ帺瀹朵负涓績
            var originX = Position.X + Width / 2f - TileSize / 2f - _context.X * TileSize;

            var originY = Position.Y + Height / 2f - TileSize / 2f - _context.Y * TileSize;

            spriteBatch.End();

            UIManager.Instance.ScissorStack.Push(Rectangle);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);

            try
            {
                DungeonMapDrawing.Draw(spriteBatch, _context, Rectangle, originX, originY, TileSize, Color.White * Alpha, large: false);
            }
            finally
            {
                spriteBatch.End();

                UIManager.Instance.ScissorStack.Pop();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);
            }
        }
    }
}
