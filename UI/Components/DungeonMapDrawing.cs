using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components
{
    // 大地图和小地图共用的绘制：瓦片、房间图标、连接线、玩家高亮。
    // 瓦片尺寸与原点由调用方决定，viewport 用于裁剪。
    public static class DungeonMapDrawing
    {
        private static readonly Dictionary<string, string> IconByRoomTag = new()
        {
            ["up"] = "iconUp",
            ["down"] = "iconDown",
            ["updown"] = "iconUpDown",
            ["npc"] = "iconNpc",
            ["trader"] = "iconTrader",
            ["return"] = "iconReturn"
        };

        // 大地图占位图标（Assets/Textures/DungeonLarge）。
        // 以后换成正式美术时，只改数据里的 iconLarge，或替换这些 png。
        private static readonly Dictionary<string, string> LargeIconByRoomTag = new()
        {
            ["room"] = "DungeonLarge/Room",
            ["entrance"] = "DungeonLarge/Entrance",
            ["return"] = "DungeonLarge/Entrance",
            ["up"] = "DungeonLarge/StairsUp",
            ["down"] = "DungeonLarge/StairsDown",
            ["updown"] = "DungeonLarge/StairsUpDown",
            ["npc"] = "DungeonLarge/Npc",
            ["trader"] = "DungeonLarge/Trader",
            ["locked"] = "DungeonLarge/Locked"
        };

        private static Texture2D? _background;

        private static Texture2D? _backgroundPlayer;

        private static Texture2D? _connectVertical;

        private static Texture2D? _connectHorizontal;

        private static Texture2D? _lockedVertical;

        private static Texture2D? _lockedHorizontal;

        // 大地图版本（128px 宽/高，避免小图拉伸模糊）
        private static Texture2D? _largeConnectVertical;

        private static Texture2D? _largeConnectHorizontal;

        private static Texture2D? _largeLockedVertical;

        private static Texture2D? _largeLockedHorizontal;

        public static void Draw(
            SpriteBatch spriteBatch,

            DungeonContext context,

            Rectangle viewport,

            float originX,

            float originY,

            float tile,

            Color tint,

            bool large)
        {
            var textures = ManagerInstance<TextureManager>.Instance;

            _background ??= textures["iconBackground"];

            _backgroundPlayer ??= textures["iconBackgroundPlayer"];

            _connectVertical ??= textures["minimapConnectVertical"];

            _connectHorizontal ??= textures["minimapConnectHorizontal"];

            _lockedVertical ??= textures["minimapLockedVertical"];

            _lockedHorizontal ??= textures["minimapLockedHorizontal"];

            _largeConnectVertical ??= textures["DungeonLarge/ConnectVertical"];

            _largeConnectHorizontal ??= textures["DungeonLarge/ConnectHorizontal"];

            _largeLockedVertical ??= textures["DungeonLarge/LockedVertical"];

            _largeLockedHorizontal ??= textures["DungeonLarge/LockedHorizontal"];

            var floor = context.Floor;

            for (var y = 0; y < floor.Height; y++)
                for (var x = 0; x < floor.Width; x++)
                {
                    var cell = floor.CharAt(x, y);

                    if (cell == '#')
                        continue;

                    var cellX = originX + x * tile;

                    var cellY = originY + y * tile;

                    if (cellX > viewport.Right || cellY > viewport.Bottom
                        || cellX + tile < viewport.Left || cellY + tile < viewport.Top)

                        continue;

                    var isPlayer = x == context.X && y == context.Y;

                    // 底色铺满整格，图标再留边
                    var full = new Rectangle((int)cellX, (int)cellY, Math.Max(1, (int)tile), Math.Max(1, (int)tile));

                    var gap = large ? 0.12f : 0.05f;

                    var rect = new Rectangle(
                        (int)(cellX + tile * gap),

                        (int)(cellY + tile * gap),

                        Math.Max(1, (int)(tile * (1 - gap * 2))),

                        Math.Max(1, (int)(tile * (1 - gap * 2))));

                    if (cell != '.')
                    {
                        var background = isPlayer ? _backgroundPlayer : _background;

                        Texture2D? icon = null;

                        if (floor.Rooms.TryGetValue(floor.RoomAt(x, y), out var room))
                        {
                            var iconKey = ResolveIconKey(room, large);

                            if (iconKey.Length > 0)
                                icon = textures[iconKey];
                        }

                        if (large)
                        {
                            // 大地图：只画专属图标，不再叠小地图底图（否则会露出它的黑框）
                            if (icon != null)
                                spriteBatch.Draw(icon, rect, tint);
                        }
                        else
                        {
                            // 小地图：底图铺满，图标再叠
                            if (background != null)
                                spriteBatch.Draw(background, full, tint);

                            if (icon != null)
                                spriteBatch.Draw(icon, rect, tint);
                        }
                    }

                    // 原版用 mmBackgroundPlayer 表示玩家；小地图再加细边，大地图不加描边
                    if (isPlayer && !large)
                        spriteBatch.DrawBorder(rect, Math.Max(1, (int)(tile * 0.05f)), Color.Gold * tint);
                }

            // 通道/锁在所有房间之后统一画，保证盖在房间之上（原版 addConnections 的顺序）
            for (var y = 0; y < floor.Height; y++)
                for (var x = 0; x < floor.Width; x++)
                {
                    var cell = floor.CharAt(x, y);

                    if (cell == '#')
                        continue;

                    var cellX = originX + x * tile;

                    var cellY = originY + y * tile;

                    if (cellX > viewport.Right || cellY > viewport.Bottom
                        || cellX + tile < viewport.Left || cellY + tile < viewport.Top)

                        continue;

                    DrawConnections(spriteBatch, context, x, y, cellX, cellY, tile, tint, large);
                }
        }

        // iconLarge 给大地图，iconSmall 给小地图；都没写就用 icon。
        // 值可以是短标签，也可以直接是 Textures 的 key。
        private static string ResolveIconKey(DungeonRoomDefinition room, bool large)
        {
            var raw = large ? room.IconLarge : room.IconSmall;

            if (raw.Length == 0)
                raw = room.Icon;

            if (raw.Length == 0)
                return "";

            var map = large ? LargeIconByRoomTag : IconByRoomTag;

            return map.TryGetValue(raw, out var mapped) ? mapped : raw;
        }

        private static void DrawConnections(
            SpriteBatch spriteBatch, DungeonContext context, int x, int y, float cellX, float cellY, float tile, Color tint, bool large)
        {
            if (large)
            {
                // 大地图：只画北/西两条边（每条相邻关系只画一次），并且画在格子间隙里
                var gap = tile * 0.12f;

                var centerX = cellX + tile / 2f;

                var centerY = cellY + tile / 2f;

                // 北
                DrawLargeEdge(spriteBatch, context, x, y, x, y - 1,

                    centerX, cellY, gap, tile, vertical: true, tint);

                // 西
                DrawLargeEdge(spriteBatch, context, x, y, x - 1, y,

                    cellX, centerY, gap, tile, vertical: false, tint);

                return;
            }

            var thickness = Math.Max(1, (int)(tile * 0.1f));

            var length = Math.Max(1, (int)(tile * 0.25f));

            var half = (int)(tile / 2) - thickness / 2;

            DrawConnection(spriteBatch, context, x, y, x, y - 1,

                (int)(cellX + half), (int)cellY, thickness, length, vertical: true, tint);

            DrawConnection(spriteBatch, context, x, y, x, y + 1,

                (int)(cellX + half), (int)(cellY + tile - length), thickness, length, vertical: true, tint);

            DrawConnection(spriteBatch, context, x, y, x - 1, y,

                (int)cellX, (int)(cellY + half), length, thickness, vertical: false, tint);

            DrawConnection(spriteBatch, context, x, y, x + 1, y,

                (int)(cellX + tile - length), (int)(cellY + half), length, thickness, vertical: false, tint);
        }

        // 大地图边：按格子的 12% 间隙计算尺寸，避免拉伸变形
        private static void DrawLargeEdge(
            SpriteBatch spriteBatch,

            DungeonContext context,

            int fromX, int fromY,

            int toX, int toY,

            float boundaryX, float boundaryY,

            float gap, float tile,

            bool vertical,

            Color tint)
        {
            var floor = context.Floor;

            if (!floor.Walkable(toX, toY))
                return;

            var from = floor.RoomAt(fromX, fromY);

            var to = floor.RoomAt(toX, toY);

            if (from.Length == 0 || to.Length == 0)
                return;

            var link = floor.LinkFor(from, to);

            var locked = link != null && link.LockedBy.Path.Length > 0

                         && !DungeonConditionRegistry.Evaluate(link.LockedBy, context);

            // 通道/锁按贴图原始比例（32×128 = 1:4）画，长度直接取两倍间隙，
            // 不再把长条压缩成短条。
            var length = gap * 4;      // 0.48 格

            var thickness = length / 4; // 保持 1:4

            if (vertical)
            {
                var texture = locked ? _largeLockedVertical : _largeConnectVertical;

                if (texture != null)
                    spriteBatch.Draw(texture, new Rectangle(
                        (int)(boundaryX - thickness / 2), (int)(boundaryY - length / 2),

                        (int)thickness, (int)length), tint);

                return;
            }

            var textureH = locked ? _largeLockedHorizontal : _largeConnectHorizontal;

            if (textureH != null)
                spriteBatch.Draw(textureH, new Rectangle(
                    (int)(boundaryX - length / 2), (int)(boundaryY - thickness / 2),

                    (int)length, (int)thickness), tint);
        }

        // 小地图的连接线（大地图走 DrawLargeEdge）
        private static void DrawConnection(
            SpriteBatch spriteBatch,

            DungeonContext context,

            int fromX, int fromY,

            int toX, int toY,

            int x, int y, int width, int height,

            bool vertical,

            Color tint)
        {
            var floor = context.Floor;

            if (!floor.Walkable(toX, toY))
                return;

            var from = floor.RoomAt(fromX, fromY);

            var to = floor.RoomAt(toX, toY);

            var locked = false;

            if (from.Length > 0 && to.Length > 0)
            {
                var link = floor.LinkFor(from, to);

                if (link != null && link.LockedBy.Path.Length > 0)
                    locked = !DungeonConditionRegistry.Evaluate(link.LockedBy, context);
            }

            // 锁就是同方向的通道换一张贴图：竖通道→竖锁，横通道→横锁
            var texture = locked

                ? vertical ? _lockedVertical : _lockedHorizontal

                : vertical ? _connectVertical : _connectHorizontal;

            if (texture != null)
                spriteBatch.Draw(texture, new Rectangle(x, y, width, height), tint);
        }
    }
}