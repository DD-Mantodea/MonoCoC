using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Dungeons
{
    public enum DungeonDirection
    {
        North,

        South,

        East,

        West
    }

    public class DungeonRoomDefinition
    {
        public string Id = "";

        public Identifier Handler;

        public string NameKey = "";

        public string Icon = "";

        public string IconSmall = "";

        public string IconLarge = "";

        public string StairsTo = "";
    }

    public class DungeonLink
    {
        public string From = "";

        public string To = "";

        public DungeonDirection Direction;

        public Identifier LockedBy;
    }

    public class DungeonFloorDefinition
    {

        public string[] Map = [];

        public Dictionary<char, string> Symbols = [];

        public Dictionary<string, DungeonRoomDefinition> Rooms = [];

        public List<DungeonLink> Links = [];

        public int Width => Map.Length == 0 ? 0 : Map[0].Length;

        public int Height => Map.Length;

        public char CharAt(int x, int y) =>

            x >= 0 && x < Width && y >= 0 && y < Height ? Map[y][x] : '#';

        public bool Walkable(int x, int y) => CharAt(x, y) != '#';

        public string RoomAt(int x, int y) =>

            Symbols.TryGetValue(CharAt(x, y), out var room) ? room : "";

        public (int X, int Y) Find(string roomId)
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    if (RoomAt(x, y) == roomId)
                        return (x, y);

            return (-1, -1);
        }

        public DungeonLink? LinkFor(string from, string to) =>

            Links.Find(link => link.From == from && link.To == to);
    }

    public class DungeonDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey = "";

        public string Start = "";

        public Identifier OnDefeat;

        public List<DungeonFloorDefinition> Floors = [];
    }
}