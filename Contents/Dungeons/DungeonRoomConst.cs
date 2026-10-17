namespace MonoCoC.Contents.Dungeons
{
    public static class DungeonRoomConst
    {
        public const int OpenRoom = 0;

        public const int Empty = 1;

        public const int LockedRoom = 2;

        public const int Void = -1;

        public const int StairsUp = 3;

        public const int StairsDown = 4;

        public const int StairsUpDown = 5;

        public const int Npc = 6;

        public const int Trader = 7;

        public const int N = 1 << 0;

        public const int S = 1 << 1;

        public const int E = 1 << 2;

        public const int W = 1 << 3;

        public const int LN = 1 << 4;

        public const int LS = 1 << 5;

        public const int LE = 1 << 6;

        public const int LW = 1 << 7;

        public static readonly int[] Walkable = [OpenRoom, StairsUp, StairsDown, StairsUpDown, Trader];

        public static readonly int[] Connectable = [OpenRoom, StairsUp, StairsDown, StairsUpDown, Trader, LockedRoom];

        public static int FromString(string str)
        {
            var result = 0;

            var previous = "";

            foreach (var raw in str)
            {
                var c = char.ToUpperInvariant(raw);

                if (previous == "L")
                {
                    previous = "";

                    switch (c)
                    {
                        case 'N': result |= LN; continue;
                        case 'S': result |= LS; continue;
                        case 'E': result |= LE; continue;
                        case 'W': result |= LW; continue;
                        default: return Void;
                    }
                }

                switch (c)
                {
                    case 'L': previous = "L"; break;
                    case 'N': result |= N; break;
                    case 'S': result |= S; break;
                    case 'E': result |= E; break;
                    case 'W': result |= W; break;
                    default: return Void;
                }
            }

            return result;
        }
    }
}