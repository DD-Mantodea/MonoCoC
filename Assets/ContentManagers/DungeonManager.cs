using Hjson;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoCoC.Assets.ContentManagers
{
    // 地牢数据：Assets/Data/Dungeons/*.hjson
    //
    // {
    //   id: "coc:deep_cave"
    //   nameKey: "Dungeons.DeepCave.Name"
    //   start: "entrance"
    //   floors: [
    //     {
    //       map: [ "#####", "#abc#", ... ]      // 每行等宽
    //       symbols: { a: "entrance", b: "tunnel" }
    //       rooms: { entrance: { handler: "coc:deep_cave_entrance" } }
    //       links: [ { from: "hall", to: "zetaz", dir: "north", lockedBy: "coc:zetaz_door_unlocked" } ]
    //     }
    //   ]
    // }
    public class DungeonManager : ContentManager<DungeonDefinition>
    {
        public override string Folder => "Data/Dungeons";

        public override string Pattern => "*.hjson";

        public static DungeonDefinition? Get(Identifier id) =>

            ManagerInstance<DungeonManager>.Instance.Dict.TryGetValue(id.ToString(), out var definition)

                ? definition

                : null;

        public override void FromText(string path, string text, IDictionary<string, DungeonDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var idText = Str(root, "id");

            if (!Identifier.TryParse(idText, out var identifier))
                throw new FormatException($"Invalid dungeon id '{idText}' in '{path}'");

            var definition = new DungeonDefinition
            {
                Id = identifier,

                NameKey = Str(root, "nameKey"),

                Start = Str(root, "start"),

                OnDefeat = ParseOptionalIdentifier(Str(root, "onDefeat"))
            };

            if (root.TryGetValue("floors", out var floorsValue) && floorsValue is JsonArray floors)
            {
                var index = 0;

                foreach (var floor in floors)
                {
                    if (floor is JsonObject floorJson)
                        definition.Floors.Add(ParseFloor(floorJson, path, index));

                    index++;
                }
            }
            else
                definition.Floors.Add(ParseFloor(root, path, 0));

            Validate(definition, path);

            entries[identifier.ToString()] = definition;
        }

        private static DungeonFloorDefinition ParseFloor(JsonObject root, string path, int floorIndex)
        {
            var floor = new DungeonFloorDefinition();

            if (root.TryGetValue("map", out var mapValue) && mapValue is JsonArray map)
                floor.Map = map.Select(row => row.ToString().Trim('"')).ToArray();

            if (floor.Map.Length == 0)
                throw new FormatException($"地牢 {path} 第 {floorIndex} 层没有 map");

            if (root.TryGetValue("symbols", out var symbolsValue) && symbolsValue is JsonObject symbols)
                foreach (var pair in symbols)
                {
                    if (pair.Key.Length != 1)
                        throw new FormatException($"地牢 {path} 第 {floorIndex} 层 symbols 的键必须是单个字符：'{pair.Key}'");

                    floor.Symbols[pair.Key[0]] = Unquote(pair.Value?.ToString() ?? "");
                }

            if (root.TryGetValue("rooms", out var roomsValue) && roomsValue is JsonObject rooms)
                foreach (var pair in rooms)
                {
                    if (pair.Value is not JsonObject roomJson)
                        continue;

                    var room = new DungeonRoomDefinition
                    {
                        Id = pair.Key,

                        NameKey = Str(roomJson, "nameKey"),

                        Icon = Str(roomJson, "icon"),

                        IconSmall = Str(roomJson, "iconSmall"),

                        IconLarge = Str(roomJson, "iconLarge"),

                        StairsTo = Str(roomJson, "stairsTo")
                    };

                    if (Identifier.TryParse(Str(roomJson, "handler"), out var handler))
                        room.Handler = handler;

                    floor.Rooms[pair.Key] = room;
                }

            if (root.TryGetValue("links", out var linksValue) && linksValue is JsonArray links)
                foreach (var entry in links)
                {
                    if (entry is not JsonObject linkJson)
                        continue;

                    var direction = Str(linkJson, "dir", "north").ToLowerInvariant() switch
                    {
                        "south" => DungeonDirection.South,

                        "east" => DungeonDirection.East,

                        "west" => DungeonDirection.West,

                        _ => DungeonDirection.North
                    };

                    var link = new DungeonLink
                    {
                        From = Str(linkJson, "from"),

                        To = Str(linkJson, "to"),

                        Direction = direction
                    };

                    if (Identifier.TryParse(Str(linkJson, "lockedBy"), out var condition))
                        link.LockedBy = condition;

                    floor.Links.Add(link);
                }

            return floor;
        }

        // 加载期校验：字符画歪一格、房间写错字符、孤立房间都在这里报出来。
        private static void Validate(DungeonDefinition definition, string path)
        {
            for (var index = 0; index < definition.Floors.Count; index++)
            {
                var floor = definition.Floors[index];

                var width = floor.Width;

                for (var y = 0; y < floor.Height; y++)
                {
                    if (floor.Map[y].Length != width)
                        throw new FormatException(
                            $"地牢 {path} 第 {index} 层第 {y + 1} 行宽度 {floor.Map[y].Length}，与第 1 行 {width} 不一致");

                    foreach (var c in floor.Map[y])
                    {
                        if (c == '#' || c == '.') continue;

                        if (!floor.Symbols.ContainsKey(c))
                            throw new FormatException(
                                $"地牢 {path} 第 {index} 层第 {y + 1} 行出现未登记字符 '{c}'");
                    }
                }

                // 每个符号必须对应 rooms 里的房间，且每个房间只能出现一次
                var seen = new Dictionary<string, int>();

                foreach (var (symbol, roomId) in floor.Symbols)
                {
                    if (!floor.Rooms.ContainsKey(roomId))
                        throw new FormatException($"地牢 {path} 第 {index} 层符号 '{symbol}' 指向不存在的房间 '{roomId}'");

                    var count = floor.Map.Sum(row => row.Count(c => c == symbol));

                    if (count != 1)
                        throw new FormatException(
                            $"地牢 {path} 第 {index} 层房间 '{roomId}' 在地图上出现 {count} 次（必须恰好 1 次）");

                    seen[roomId] = count;
                }

                foreach (var roomId in floor.Rooms.Keys)
                    if (!seen.ContainsKey(roomId))
                        throw new FormatException($"地牢 {path} 第 {index} 层房间 '{roomId}' 没有放到地图上");

                foreach (var link in floor.Links)
                    if (!floor.Rooms.ContainsKey(link.From) || !floor.Rooms.ContainsKey(link.To))
                        throw new FormatException($"地牢 {path} 第 {index} 层 link 引用了不存在的房间：{link.From} → {link.To}");
            }

            if (string.IsNullOrEmpty(definition.Start))
                throw new FormatException($"地牢 {path} 没有 start");

            if (!definition.Floors[0].Rooms.ContainsKey(definition.Start))
                throw new FormatException($"地牢 {path} 的 start '{definition.Start}' 不在第 0 层 rooms 里");

            // 从 start 出发的可达性（墙阻挡；锁视为可通过，锁是运行期条件）
            var floor0 = definition.Floors[0];

            var start = floor0.Find(definition.Start);

            var visited = new HashSet<(int, int)> { start };

            var queue = new Queue<(int X, int Y)>();

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                foreach (var (dx, dy) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) })
                {
                    var nx = x + dx;

                    var ny = y + dy;

                    if (!floor0.Walkable(nx, ny) || !visited.Add((nx, ny)))
                        continue;

                    queue.Enqueue((nx, ny));
                }
            }

            foreach (var roomId in floor0.Rooms.Keys)
            {
                var (x, y) = floor0.Find(roomId);

                if (!visited.Contains((x, y)))
                    throw new FormatException($"地牢 {path} 的房间 '{roomId}' 从 start 走不到");
            }
        }

        private static string Unquote(string text) =>

            text.Length > 1 && text[0] == '"' && text[^1] == '"' ? text[1..^1] : text;

        private static Identifier ParseOptionalIdentifier(string text) =>

            Identifier.TryParse(text, out var id) ? id : default;

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            return Unquote(value.ToString());
        }
    }
}