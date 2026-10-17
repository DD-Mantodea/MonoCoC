using MonoCoC.Registry;

namespace MonoCoC.Contents.Dungeons
{

    public static class DungeonSession
    {
        public static bool InDungeon { get; set; }

        public static Identifier DungeonId { get; set; }

        public static int FloorIndex { get; set; }

        public static int X { get; set; }

        public static int Y { get; set; }

        public static bool LastCombatLost { get; set; }

        public static void Start(DungeonContext context)
        {
            InDungeon = true;

            Sync(context);
        }

        public static void Sync(DungeonContext context)
        {
            DungeonId = context.Definition.Id;

            FloorIndex = context.FloorIndex;

            X = context.X;

            Y = context.Y;
        }

        public static void End()
        {
            InDungeon = false;

            DungeonId = default;

            FloorIndex = 0;

            X = 0;

            Y = 0;
        }
    }
}