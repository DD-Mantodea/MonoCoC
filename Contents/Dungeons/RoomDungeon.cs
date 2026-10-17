using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Dungeons
{

    public class Room
    {
        public string RoomName = "";

        public string RoomDisplayName = "";

        public string NorthExit = "";

        public Func<bool>? NorthExitCondition;

        public int NorthExitTime;

        public string EastExit = "";

        public Func<bool>? EastExitCondition;

        public int EastExitTime;

        public string SouthExit = "";

        public Func<bool>? SouthExitCondition;

        public int SouthExitTime;

        public string WestExit = "";

        public Func<bool>? WestExitCondition;

        public int WestExitTime;

        public Func<bool>? RoomFunction;
    }

    public static class RoomDungeon
    {
        public static readonly Dictionary<string, Room> Rooms = new(StringComparer.Ordinal);

        public static bool InRoomedDungeon { get; set; }

        public static string InRoomedDungeonName { get; set; } = "";

        public static string CurrentRoom { get; set; } = "";

        public static void Configure(IEnumerable<Room> rooms, string dungeonName)
        {
            Rooms.Clear();

            foreach (var room in rooms)
                Rooms[room.RoomName] = room;

            InRoomedDungeon = true;

            InRoomedDungeonName = dungeonName;

            CurrentRoom = "";

            DungeonSession.InDungeon = true;
        }

        public static void Exit()
        {
            InRoomedDungeon = false;

            InRoomedDungeonName = "";

            CurrentRoom = "";

            Rooms.Clear();

            DungeonSession.End();
        }

        public static void Move(string roomName, float hours = 0f)
        {

            if (hours > 0f)
                Scene.View.Time.PassMinutes((int)MathF.Round(hours * 60f));

            Scene.ClearOutput();

            if (!Rooms.TryGetValue(roomName, out var room))
            {
                Scene.OutputText("Scenes.Dungeoncore.Move.RoomNotFoundError".Localized(roomName));

                return;
            }

            Scene.ClearButtons();

            CurrentRoom = roomName;

            if (room.RoomFunction == null)
            {
                Scene.OutputText("Scenes.Dungeoncore.Move.RoomFunctionMissing".Localized(roomName));

                return;
            }

            if (!room.RoomFunction())
                GenerateRoomMenu(room);
        }

        public static void ResumeFromFight() => Move(CurrentRoom);

        public static bool Allows(Func<bool>? condition) => condition == null || condition();

        public static void GenerateRoomMenu(Room room)
        {
            if (Scene.ButtonAt(6) == null)
                if (Allows(room.NorthExitCondition) && room.NorthExit.Length > 0)
                    Scene.AddButton(6, "Common.Button.North".Localized(), () => Move(room.NorthExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(6, "North", "");

            if (Scene.ButtonAt(11) == null)
                if (Allows(room.SouthExitCondition) && room.SouthExit.Length > 0)
                    Scene.AddButton(11, "Common.Button.South".Localized(), () => Move(room.SouthExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(11, "South", "");

            if (Scene.ButtonAt(10) == null)
                if (Allows(room.WestExitCondition) && room.WestExit.Length > 0)
                    Scene.AddButton(10, "Common.Button.West".Localized(), () => Move(room.WestExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(10, "West", "");

            if (Scene.ButtonAt(12) == null)
                if (Allows(room.EastExitCondition) && room.EastExit.Length > 0)
                    Scene.AddButton(12, "Common.Button.East".Localized(), () => Move(room.EastExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(12, "East", "");

            if (Scene.ButtonAt(13) == null)
                Scene.AddButton(13, "Common.Button.Inventory".Localized(), () => Scenes.Vanilla.InventoryScene.InventoryMenu());

        }
    }

    public static class KeepHooks
    {
        static KeepHooks() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Action> Handlers = [];

        public static readonly Type Signature = typeof(Action);

        public static Action? Get(string id) =>

            Handlers.TryGetValue(Identifier.Parse(id), out var handler) ? handler : null;

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(void))
                return false;

            if (method.GetParameters().Length != 0)
                return false;

            var handler = (Action)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Handlers[Identifier.Parse(attribute.Id)] = handler;

            return true;
        }
    }
}