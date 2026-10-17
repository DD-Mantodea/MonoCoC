using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    // 大地图：可缩放/平移的瓦片地图（原版 DungeonMap 的 alternative 模式）。
    // 瓦片/连接线绘制在 DungeonMapDrawing 里，和小地图共用。
    public class DungeonMapPanel : Container
    {
        private const int TileSize = 40;

        private const float MinZoom = 0.2f;

        private const float MaxZoom = 5f;

        private DungeonContext? _context;

        public float ZoomScale = 1f;

        public Vector2 CameraPosition;

        private bool _dragging;

        private Vector2 _lastMouse;

        public DungeonMapPanel()
        {
            BackgroundColor = new Color(0, 0, 0, 190);

            Visible = false;

            CanHit = false;
        }

        public void Show(DungeonContext context)
        {
            _context = context;

            LayoutData.Size.X = 720;

            LayoutData.Size.Y = 560;

            LayoutData.Position.X = (LayoutConstants.ScreenWidth - Width) / 2;

            LayoutData.Position.Y = (LayoutConstants.ScreenHeight - Height) / 2;

            var floor = context.Floor;

            var mapWidth = floor.Width * TileSize;

            var mapHeight = floor.Height * TileSize;

            ZoomScale = MathF.Min(1f, MathF.Min((Width - 40f) / mapWidth, (Height - 40f) / mapHeight));

            ZoomScale = MathF.Max(MinZoom, ZoomScale);

            CameraPosition = Vector2.Zero;

            _dragging = false;

            Visible = true;
        }

        public void Hide()
        {
            Visible = false;

            _dragging = false;

            _context = null;
        }

        public void Zoom(float factor)
        {
            ZoomScale = Math.Clamp(MathF.Round(ZoomScale * factor, 4), MinZoom, MaxZoom);

            ClampCamera();
        }

        public void Pan(Vector2 screenDelta)
        {
            CameraPosition += screenDelta / ZoomScale;

            ClampCamera();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Visible || _context == null)
                return;

            var mouse = UserInput.GetMousePos();

            var hover = Rectangle.Contains(mouse.ToPoint());

            if (hover)
            {
                var wheel = UserInput.GetDeltaWheelValue();

                if (wheel > 0)
                    Zoom(1.25f);
                else if (wheel < 0)
                    Zoom(0.8f);
            }

            if (hover && UserInput.MouseLeftPressed)
            {
                _dragging = true;

                _lastMouse = mouse;
            }

            if (!UserInput.CurrentMouseState.MouseLeft())
                _dragging = false;

            if (_dragging)
            {
                Pan(mouse - _lastMouse);

                _lastMouse = mouse;
            }
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            if (_context == null || !Visible)
                return;

            var tile = TileSize * ZoomScale;

            var mapWidth = _context.Floor.Width * tile;

            var mapHeight = _context.Floor.Height * tile;

            var originX = Position.X + (Width - mapWidth) / 2 + CameraPosition.X * ZoomScale;

            var originY = Position.Y + (Height - mapHeight) / 2 + CameraPosition.Y * ZoomScale;

            spriteBatch.End();

            UIManager.Instance.ScissorStack.Push(Rectangle);

            spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);

            try
            {
                DungeonMapDrawing.Draw(spriteBatch, _context, Rectangle, originX, originY, tile, Color.White * Alpha, large: true);
            }
            finally
            {
                spriteBatch.End();

                UIManager.Instance.ScissorStack.Pop();

                spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);
            }
        }

        private void ClampCamera()
        {
            if (_context == null)
                return;

            var floor = _context.Floor;

            var mapWidth = floor.Width * TileSize * ZoomScale;

            var mapHeight = floor.Height * TileSize * ZoomScale;

            var limitX = MathF.Abs(Width - mapWidth) / 2 / ZoomScale;

            var limitY = MathF.Abs(Height - mapHeight) / 2 / ZoomScale;

            CameraPosition.X = Math.Clamp(CameraPosition.X, -limitX, limitX);

            CameraPosition.Y = Math.Clamp(CameraPosition.Y, -limitY, limitY);
        }
    }
}