using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes;
using MonoCoC.ModLoader;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.Dungeons
{
    // 原版 classes/Room.hx（32 行）：命名房间图节点。
    // 与 Manor 的数字地图（dungeonMap/connectivity）是两套系统：房间有名字、四向出口、条件与时间。
    public sealed class Room
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

        // 返回 true = 房间自己摆好了按钮（原版 generateRoomMenu 不再补方向键）；false = 由运行时补方向键。
        public Func<bool>? RoomFunction;
    }

    // 原版 DungeonCore 的房间图部分（DungeonCore.hx:273-322）：
    //   move(roomName) / generateRoomMenu(room) / resumeFromFight() / inRoomedDungeon 状态。
    public static class RoomDungeon
    {
        private static readonly Dictionary<string, Room> Rooms = new(StringComparer.Ordinal);

        // 原版 game.dungeons.inRoomedDungeon / inRoomedDungeonName
        public static bool InRoomedDungeon { get; set; }

        public static string InRoomedDungeonName { get; set; } = "";

        public static string CurrentRoom { get; private set; } = "";

        // 进入房间图地牢：房间表 + 地牢名（原版 enterD3 里设 inRoomedDungeon=true / name="BasiliskCave"）
        public static void Configure(IEnumerable<Room> rooms, string dungeonName)
        {
            Rooms.Clear();

            foreach (var room in rooms)
                Rooms[room.RoomName] = room;

            InRoomedDungeon = true;
            InRoomedDungeonName = dungeonName;
            CurrentRoom = "";

            // 原版 enterD3 里 game.inDungeon = true 的等价物（C# 复用既有 DungeonSession）
            DungeonSession.InDungeon = true;
        }

        // 离开房间图地牢：同时复位房间图状态与 DungeonSession。
        // 原版场景里的 `inDungeon = false; inRoomedDungeon = false;` 统一用这一个入口，不再造第二套。
        public static void Exit()
        {
            InRoomedDungeon = false;
            InRoomedDungeonName = "";
            CurrentRoom = "";
            Rooms.Clear();

            DungeonSession.End();
        }

        // 原版 DungeonCore.move（DungeonCore.hx:298-322）：时间 → 清屏 → 房间函数；房间没接管就补方向键。
        public static void Move(string roomName, float hours = 0f)
        {
            // 原版 cheatTime(timeToPass) 是小时；1/12 小时 = 5 分钟
            if (hours > 0f)
                Scene.View.Time.PassMinutes((int)MathF.Round(hours * 60f));

            Scene.ClearOutput();

            if (!Rooms.TryGetValue(roomName, out var room))
            {
                Scene.OutputText($"Error: Couldn't find the room indexed as: {roomName}");

                return;
            }

            Scene.ClearButtons();
            CurrentRoom = roomName;

            if (room.RoomFunction == null)
            {
                Scene.OutputText($"Error: Room entry function for room indexed as '{roomName}' was not set.");

                return;
            }

            // 原版 move 先 menu() 再调用房间函数；房间函数返回 true 时不再补方向菜单。
            if (!room.RoomFunction())
                GenerateRoomMenu(room);
        }

        // 原版 DungeonCore.resumeFromFight（DungeonCore.hx:267-269）：战斗结束回到当前房间。
        public static void ResumeFromFight() => Move(CurrentRoom);

        private static bool Allows(Func<bool>? condition) => condition == null || condition();

        // 原版 DungeonCore.generateRoomMenu（DungeonCore.hx:273-302）：
        // 只在对应槽位还没有按钮时才补（房间函数可能已经摆了同槽按钮）；Map 按钮不显示（房间图没有地图可画）。
        private static void GenerateRoomMenu(Room room)
        {
            if (Scene.ButtonAt(6) == null)
            {
                if (Allows(room.NorthExitCondition) && room.NorthExit.Length > 0)
                    Scene.AddButton(6, "North", () => Move(room.NorthExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(6, "North", "");
            }

            if (Scene.ButtonAt(11) == null)
            {
                if (Allows(room.SouthExitCondition) && room.SouthExit.Length > 0)
                    Scene.AddButton(11, "South", () => Move(room.SouthExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(11, "South", "");
            }

            if (Scene.ButtonAt(10) == null)
            {
                if (Allows(room.WestExitCondition) && room.WestExit.Length > 0)
                    Scene.AddButton(10, "West", () => Move(room.WestExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(10, "West", "");
            }

            if (Scene.ButtonAt(12) == null)
            {
                if (Allows(room.EastExitCondition) && room.EastExit.Length > 0)
                    Scene.AddButton(12, "East", () => Move(room.EastExit, 1f / 12f));
                else
                    Scene.AddButtonDisabled(12, "East", "");
            }

            if (Scene.ButtonAt(13) == null)
                Scene.AddButton(13, "Inventory", () => MonoCoC.Contents.Scenes.Vanilla.InventoryScene.InventoryMenu());

            // 原版这里还有 addButton(14, "Map", map.displayMap)：房间图地牢没有地图可画（Haxe 的 Map 也是空/无意义）。
            // 按 lead 决定不显示 Map 按钮。
        }
    }

    // 房间图地牢与各场景类之间的接线表（同 DungeonRoomRegistry 的扫描式注册）：
    // B–F 块在自己的文件里写一个 Register("coc:keep_hook_xxx") 的 static void 钩子方法即可自动接上，
    // LethicesKeepScene 不需要被别的块改。未注册时对应按钮置灰并注明出处，便于分批落地。
    public static class KeepHooks
    {
        static KeepHooks() => RegistrationScanner.OnMethods(RegisterMethod);

        private static readonly Dictionary<Identifier, Action> Handlers = [];

        private static readonly Type Signature = typeof(Action);

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
