using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Dungeons
{
    public class RandomMazeResult
    {
        public int[] DungeonMap = [];

        public int[] Connectivity = [];
    }

    public static class RandomDungeon
    {
        private class Cell
        {
            public int Room = 1;

            public int Connections;
        }

        private class Frontier
        {
            public int X;

            public int Y;

            public int FromX;

            public int FromY;
        }

        public static RandomMazeResult GenerateRandomMaze(int height, int width, int density = 4, bool stretch = false)
        {
            var distanceBetween = stretch ? 2 : 1;

            var map = new Cell[width, height];

            for (var ix = 0; ix < width; ix++)
                for (var iy = 0; iy < height; iy++)
                    map[ix, iy] = new Cell();

            var x = 1 + Random.Shared.Next(Math.Max(1, width - 2));

            var y = 1 + Random.Shared.Next(Math.Max(1, height - 2));

            map[x, y].Room = 0;

            var frontiers = new List<Frontier>
            {
                new() { X = x + 1, Y = y, FromX = x, FromY = y },

                new() { X = x, Y = y + 1, FromX = x, FromY = y },

                new() { X = x - 1, Y = y, FromX = x, FromY = y },

                new() { X = x, Y = y - 1, FromX = x, FromY = y },
            };

            while (frontiers.Count > 0)
            {
                var frontier = frontiers[Random.Shared.Next(frontiers.Count)];

                frontiers.Remove(frontier);

                x = frontier.X;

                y = frontier.Y;

                var fromX = frontier.FromX;

                var fromY = frontier.FromY;

                var current = map[x, y];

                var fromRoom = map[fromX, fromY];

                if (current.Room != 1)
                    continue;

                current.Room = 0;

                if (x > fromX)
                {
                    current.Connections |= DungeonRoomConst.W;

                    fromRoom.Connections |= DungeonRoomConst.E;
                }
                else if (x < fromX)
                {
                    current.Connections |= DungeonRoomConst.E;

                    fromRoom.Connections |= DungeonRoomConst.W;
                }

                if (y > fromY)
                {
                    current.Connections |= DungeonRoomConst.N;

                    fromRoom.Connections |= DungeonRoomConst.S;
                }
                else if (y < fromY)
                {
                    current.Connections |= DungeonRoomConst.S;

                    fromRoom.Connections |= DungeonRoomConst.N;
                }

                var possibleConnections = new List<Frontier>
                {
                    new() { X = x + distanceBetween, Y = y, FromX = x, FromY = y },

                    new() { X = x, Y = y + distanceBetween, FromX = x, FromY = y },

                    new() { X = x - distanceBetween, Y = y, FromX = x, FromY = y },

                    new() { X = x, Y = y - distanceBetween, FromX = x, FromY = y },
                };

                possibleConnections.RemoveAll(it => it.X < 0 || it.X >= width || it.Y < 0 || it.Y >= height);

                var connections = possibleConnections.FindAll(it => map[it.X, it.Y].Room != 1).Count;

                while (connections < density && possibleConnections.Count > 0)
                {
                    possibleConnections.RemoveAt(Random.Shared.Next(possibleConnections.Count));

                    connections += 1;
                }
            }

            var result = new RandomMazeResult
            {
                DungeonMap = new int[width * height],

                Connectivity = new int[width * height],
            };

            var index = 0;

            for (var ix = 0; ix < width; ix++)
                for (var iy = 0; iy < height; iy++)
                {
                    result.DungeonMap[index] = map[ix, iy].Room;

                    result.Connectivity[index] = map[ix, iy].Connections;

                    index++;
                }

            return result;
        }

        public static int FindEntrance(int[] dungeonMap)
        {
            var possibleIndexes = new List<int>();

            var mapModulus = (int)Math.Sqrt(dungeonMap.Length);

            for (var i = 0; i < dungeonMap.Length; i++)
            {
                if (dungeonMap[i] != 0)
                    continue;

                var x = i % mapModulus;

                var y = i / mapModulus;

                var n = x + (y - 1) * mapModulus;

                var s = x + (y + 1) * mapModulus;

                var e = x + 1 + y * mapModulus;

                var w = x - 1 + y * mapModulus;

                var connections = 0;

                foreach (var candidate in new[] { n, s, e, w })
                    if (candidate >= 0 && candidate < dungeonMap.Length && dungeonMap[candidate] == 0)
                        connections += 1;

                if (connections is 1 or 4)
                    possibleIndexes.Add(i);
            }

            if (possibleIndexes.Count == 0)
                return 0;

            return possibleIndexes[Random.Shared.Next(possibleIndexes.Count)];
        }
    }
}