// 原版 classes/scenes/dungeons/Manor.hx（1302 行）：Bone Manor 地牢本体。
// 房间/楼层/移动逻辑照 DungeonAbstractContent.hx + DungeonCore.hx 的 alternative map 模式移植。
// C# 的 DungeonDefinition 只能表达静态 hjson 地牢（工厂/巫师塔那套），装不下 Manor 的 3 层固定图
// + 第 3 层随机虚空，所以本场景自带地图状态（同 WizardTowerMirrorPuzzleScene：Scene + 文本地图）。

using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.OptionSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 命名空间跟 Contents/Dungeons 其他地牢实现走（DungeonScene / WizardTowerRooms / DesertCaveRooms）；
// 不能叫 ...Scenes.Vanilla.Dungeons——那会遮蔽 SaveLoad.cs 里的 Dungeons.DungeonSession。
namespace MonoCoC.Contents.Dungeons
{
    [Register("coc:manor")]
    public class ManorScene : Scene
    {
        // 原版 DungeonRoomConst.hx:3-9 的格子种类
        private const int OpenRoom = 0;
        private const int EmptyRoom = 1;
        private const int LockedRoom = 2;
        private const int VoidRoom = -1;
        private const int StairsUp = 3;
        private const int StairsDown = 4;
        private const int StairsUpDown = 5;
        private const int Trader = 7;

        // 原版 DungeonRoomConst.hx:11-19 的方向位（低 4 位连通、高 4 位加锁）
        private const int DirNorth = 1 << 0;
        private const int DirSouth = 1 << 1;
        private const int DirEast = 1 << 2;
        private const int DirWest = 1 << 3;

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private int _playerLoc;

        public int Floor = 1;
        public int PrevLoc;

        // 原版 DungeonAbstractContent.dungeonRooms：格子索引 → 房间内容
        private readonly Dictionary<int, Action> DungeonRooms = [];

        // 原版 Manor.infinityMaps / InfinityLevel / InfinityFunc（Manor.hx:29-30 / :70）
        private readonly List<(int[] Map, int[] Connectivity)> InfinityMaps = [];
        public int InfinityLevel;
        private Action InfinityFunc;

        // 原版 Manor.Code / Codespell（Manor.hx:206-207，书架谜题）
        public int Code;
        public string Codespell = "";

        private SaveContent Save => SaveContent(new Identifier("coc", "manor"));

        private int ManorProgress => Flags.Get(VanillaFlags.ManorProgress);

        private bool HasProgress(int bit) => (ManorProgress & bit) != 0;

        private void SetProgress(int bit) => Flags.Set(VanillaFlags.ManorProgress, ManorProgress | bit);

        public ManorScene()
        {
            // 原版 Manor.new()（Manor.hx:247-252）
            InfinityFunc = Infinity1;
        }

        public int PlayerLoc
        {
            get => _playerLoc;
            set
            {
                // 原版 DungeonCore.set_playerLoc：prevLoc 记上一格；Manor.tunnels1(:598) 用它判断是不是刚从图书馆下来
                PrevLoc = _playerLoc;
                _playerLoc = value;
            }
        }

        public override void Entry()
        {
            // 原版 Manor 没有 entry()：Dungeons 菜单 / Dullahan 都是直接调 manor.enterDungeon()。
            // 这里让「场景入口」等价于 enterDungeon，registry 里 Go<ManorScene>() 也能用。
            EnterDungeon();
        }

        // 原版 Manor.get_dungeonMap（Manor.hx:32-60）
        public int[] DungeonMap
        {
            get
            {
                var secretLibraryRoom = EmptyRoom;

                if (HasProgress(32))
                    secretLibraryRoom = HasProgress(2048) ? StairsDown : OpenRoom;

                return Floor switch
                {
                    1 =>
                    [
                        secretLibraryRoom, EmptyRoom, StairsUp,
                        OpenRoom, OpenRoom, OpenRoom,
                        EmptyRoom, OpenRoom, EmptyRoom
                    ],
                    2 =>
                    [
                        EmptyRoom, OpenRoom, OpenRoom,
                        OpenRoom, OpenRoom, EmptyRoom,
                        EmptyRoom, StairsDown, EmptyRoom
                    ],
                    0 =>
                    [
                        EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom,
                        EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom,
                        EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom,
                        EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, OpenRoom, EmptyRoom,
                        EmptyRoom, EmptyRoom, EmptyRoom, EmptyRoom, OpenRoom, EmptyRoom,
                        OpenRoom, OpenRoom, OpenRoom, OpenRoom, OpenRoom, StairsUp
                    ],
                    _ => InfinityMaps[InfinityLevel].Map
                };
            }
        }

        // 原版 Manor.get_connectivity（Manor.hx:168-191）
        public int[] Connectivity
        {
            get
            {
                return Floor switch
                {
                    1 =>
                    [
                        DirSouth, DirSouth, DirSouth,
                        DirEast | DirNorth,
                        DirNorth | DirSouth | DirEast | DirWest,
                        DirWest | DirNorth,
                        0, DirNorth, 0
                    ],
                    2 =>
                    [
                        0, DirSouth | DirEast, DirWest,
                        DirEast, DirNorth | DirWest | DirSouth, 0,
                        0, DirNorth, 0
                    ],
                    0 =>
                    [
                        0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, DirSouth, 0,
                        0, 0, 0, 0, DirNorth | DirSouth, 0,
                        DirWest | DirEast, DirWest | DirEast, DirWest | DirEast,
                        DirWest | DirEast, DirWest | DirEast | DirNorth, DirWest
                    ],
                    _ => InfinityMaps[InfinityLevel].Connectivity
                };
            }
        }

        private int MapModulus => (int)Math.Sqrt(DungeonMap.Length);

        // 原版 Manor.initRoomsF1（Manor.hx:80-88）
        public void InitRoomsF1()
        {
            DungeonRooms.Clear();
            DungeonRooms[7] = RoomCourtyard;
            DungeonRooms[4] = MainHall;
            DungeonRooms[1] = DiningRoom;
            DungeonRooms[3] = LibraryRoom;
            DungeonRooms[5] = StairsStart;
            DungeonRooms[0] = LibrarySecret;
            DungeonRooms[2] = Stairs0;
        }

        // 原版 Manor.initRooms（Manor.hx:90-95）
        public void InitRooms()
        {
            DungeonRooms.Clear();
            InitRoomsF1();
        }

        // 原版 Manor.initRoomsF2（Manor.hx:97-103）
        public void InitRoomsF2()
        {
            DungeonRooms.Clear();
            DungeonRooms[7] = Stairs1;
            DungeonRooms[4] = Stairs2;
            DungeonRooms[1] = Stairs3;
            DungeonRooms[2] = Bedroom;
            DungeonRooms[3] = Study;
        }

        // 原版 Manor.initRoomsF0（Manor.hx:105-115）
        public void InitRoomsF0()
        {
            DungeonRooms.Clear();
            DungeonRooms[35] = Tunnels1;
            DungeonRooms[34] = Tunnels2;
            DungeonRooms[33] = Tunnels3;
            DungeonRooms[32] = Tunnels4;
            DungeonRooms[31] = Tunnels5;
            DungeonRooms[30] = TheChamber;
            DungeonRooms[28] = LethiciteTunnel1;
            DungeonRooms[22] = LethiciteTunnel2;
        }

        // 原版 Manor.isInChamber（Manor.hx:117-119）
        public bool IsInChamber() => PlayerLoc == 30 && Floor == 0;

        // 原版 Manor.runFunc（Manor.hx:72-78）→ DungeonAbstractContent.runFunc（:59-68）
        public void RunFunc()
        {
            if (Floor == 3)
            {
                InfinityFunc();

                return;
            }

            ClearOutput();
            SetDungeonButtons();

            // 原版 dungeonRooms.get(playerLoc)() 对空位会直接炸；switchFloor(0, x) 在设 playerLoc
            // 之前先调了一次 runFunc（Manor.hx:129），所以这里用 TryGetValue 防空。
            if (DungeonRooms.TryGetValue(PlayerLoc, out var room))
                room();
        }

        // 原版 Manor.switchFloor（Manor.hx:121-141）
        public void SwitchFloor(int floor, int newLoc, bool runTile = true)
        {
            Floor = floor;

            switch (floor)
            {
                case 1:
                    InitRoomsF1();
                    break;
                case 2:
                    InitRoomsF2();
                    break;
                case 0:
                    InitRoomsF0();
                    RunFunc();
                    break;
                case 3:
                    GenerateInfinityMaps();
                    newLoc = FindEntrance();
                    break;
            }

            PlayerLoc = newLoc;
            RemakeMaps();

            if (runTile)
                RunFunc();
        }

        // 原版 Manor.findEntrance（Manor.hx:143-166）
        public int FindEntrance()
        {
            var possibleIndexes = new List<int>();
            var map = DungeonMap;
            var mapModulus = (int)Math.Sqrt(map.Length);

            for (var i = 0; i < map.Length; i++)
            {
                var connections = 0;

                if (i + 1 < map.Length && map[i + 1] == 0)
                    connections += 1;
                if (i - 1 >= 0 && map[i - 1] == 0)
                    connections += 1;
                if (i + mapModulus < map.Length && map[i + mapModulus] == 0)
                    connections += 1;
                if (i - mapModulus < map.Length && map[i - mapModulus] == 0)
                    connections += 1;

                if (map[i] == 0 && (connections == 1 || connections == 4))
                    possibleIndexes.Add(i);
            }

            // 原版没有兜底（候选为空时 Utils.rand(0) 会炸）；C# 返回 0，避免虚空层偶发崩溃。
            return possibleIndexes.Count > 0 ? possibleIndexes[Random.Shared.Next(possibleIndexes.Count)] : 0;
        }

        // 原版 DungeonCore.canMove（DungeonCore.hx:232-242）
        public bool CanMove(int loc, int dest, int direction)
        {
            var map = DungeonMap;
            var connectivity = Connectivity;

            if (loc < 0 || loc >= connectivity.Length || dest < 0 || dest >= map.Length)
                return false;

            var connected = (connectivity[loc] & direction) != 0;
            var unlocked = (connectivity[loc] & (direction << 4)) == 0;
            var walkable = map[dest] is OpenRoom or StairsUp or StairsDown or StairsUpDown or Trader;

            return connected && unlocked && walkable;
        }

        // 原版 DungeonCore.moveAlternative（DungeonCore.hx:244-264）
        public void Move(int direction)
        {
            var modulus = MapModulus;

            PlayerLoc = direction switch
            {
                0 => PlayerLoc - modulus,
                1 => PlayerLoc + modulus,
                2 => PlayerLoc - 1,
                _ => PlayerLoc + 1
            };

            // 原版 cheatTime(1 / 12) = 5 分钟
            View.Time.PassMinutes(5);

            RunFunc();
        }

        private void AddMoveButton(int slot, string label, int direction)
        {
            var modulus = MapModulus;

            var dest = direction switch
            {
                0 => PlayerLoc - modulus,
                1 => PlayerLoc + modulus,
                2 => PlayerLoc - 1,
                _ => PlayerLoc + 1
            };

            var bit = direction switch
            {
                0 => DirNorth,
                1 => DirSouth,
                2 => DirWest,
                _ => DirEast
            };

            if (CanMove(PlayerLoc, dest, bit))
                AddButton(slot, label, () => Move(direction));
            else
                AddButtonDisabled(slot, label, "");
        }

        // 原版 DungeonCore.setDungeonButtons（DungeonCore.hx:183-203）的 alternative 分支
        public void SetDungeonButtons()
        {
            ClearButtons();

            AddMoveButton(6, "North", 0);
            AddMoveButton(11, "South", 1);
            AddMoveButton(10, "West", 2);
            AddMoveButton(12, "East", 3);

            AddButton(13, "Inventory", () => InventoryScene.InventoryMenu(),
                "The inventory allows you to view or use your items.");
            AddButton(14, "Map", ShowMap, "View the map of this dungeon.");
        }

        // 原版 dungeons.setDungeonButtons(northFunction, southFunction, westFunction, eastFunction)：
        // 第 3 层（虚空）的四个方向按钮不再对应地图移动，而是直接调 infinity1..5 的脚本链。
        private void SetInfinityButtons(Action north, Action south, Action west, Action east)
        {
            ClearButtons();

            AddButton(6, "North", north);
            AddButton(11, "South", south);
            AddButton(10, "West", west);
            AddButton(12, "East", east);
            AddButton(13, "Inventory", () => InventoryScene.InventoryMenu(),
                "The inventory allows you to view or use your items.");
            AddButton(14, "Map", ShowMap, "View the map of this dungeon.");
        }

        // 原版 Codex.headerSub（Codex.hx:94-96）：[pg] + Georgia 大号加粗下划线的子标题
        private static void JournalHeader(string text) =>
            OutputText("[pg]<font size=\"24\" face=\"Georgia\"><b><u>" + text + "</u></b></font>\n");

        // 原版 DungeonAbstractContent.setStairButtons（DungeonAbstractContent.hx:85-94）
        public void SetStairButtons(Action upstairs = null, Action downstairs = null)
        {
            if (downstairs != null)
                AddButton(5, "Downstairs", downstairs);

            if (upstairs != null)
                AddButton(downstairs != null ? 0 : 5, "Upstairs", upstairs);
        }

        // 原版 dungeons.remakeMaps()：重绘图形地牢地图。C# 的图形 DungeonMap 只服务 DungeonContext，
        // 本场景用文本地图（ShowMap），这里保持空实现。
        public void RemakeMaps()
        {
        }

        // 原版 DungeonMap.displayMap 的文本版（包含地形/图例/当前位置）
        public void ShowMap()
        {
            ClearOutput();
            ClearButtons();

            var map = DungeonMap;
            var modulus = (int)Math.Sqrt(map.Length);
            var text = new StringBuilder();

            text.Append("<b><u>Old Manor</u></b>\n\n");

            for (var y = 0; y < modulus; y++)
            {
                for (var x = 0; x < modulus; x++)
                {
                    var i = y * modulus + x;

                    if (i >= map.Length)
                        break;

                    text.Append(i == PlayerLoc
                        ? "[@]"
                        : map[i] switch
                        {
                            EmptyRoom => "   ",
                            StairsUp => "[U]",
                            StairsDown => "[D]",
                            StairsUpDown => "[B]",
                            Trader => "[T]",
                            _ => "[ ]"
                        });
                }

                text.Append('\n');
            }

            text.Append("\n<b>Legend</b>\n[@] You are here\n[ ] Room\n[U] Upstairs\n[D] Downstairs\n[B] Up/down\n   Empty\n");

            OutputText(text.ToString());

            AddButton(14, "Back", RunFunc);
        }

        // 原版 Manor.generateInfinityMaps（Manor.hx:62-68）
        public void GenerateInfinityMaps()
        {
            for (var i = 0; i < 5; i++)
                InfinityMaps.Add(GenerateRandomMaze(10, 10, 3));
        }

        // 原版 RandomDungeon.generateRandomMaze（RandomDungeon.hx:33-107）。
        // 用行主序（index = y*width + x）统一 C# 侧索引；density 循环照原版（只弹候选、不改连通位）。
        private static (int[] Map, int[] Connectivity) GenerateRandomMaze(int height, int width, int density = 4, bool stretch = false)
        {
            var distanceBetween = stretch ? 2 : 1;
            var room = new int[height, width];
            var conn = new int[height, width];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    room[y, x] = EmptyRoom;

            var startX = 1 + Random.Shared.Next(width - 2);
            var startY = 1 + Random.Shared.Next(height - 2);

            room[startY, startX] = OpenRoom;

            var frontiers = new List<(int X, int Y, int FromX, int FromY)>
            {
                (startX + 1, startY, startX, startY),
                (startX, startY + 1, startX, startY),
                (startX - 1, startY, startX, startY),
                (startX, startY - 1, startX, startY)
            };

            while (frontiers.Count > 0)
            {
                var index = Random.Shared.Next(frontiers.Count);
                var frontier = frontiers[index];

                frontiers.RemoveAt(index);

                var x = frontier.X;
                var y = frontier.Y;

                if (x < 0 || x >= width || y < 0 || y >= height)
                    continue;

                if (room[y, x] != EmptyRoom)
                    continue;

                room[y, x] = OpenRoom;

                if (x > frontier.FromX)
                {
                    conn[y, x] |= DirWest;
                    conn[frontier.FromY, frontier.FromX] |= DirEast;
                }
                else if (x < frontier.FromX)
                {
                    conn[y, x] |= DirEast;
                    conn[frontier.FromY, frontier.FromX] |= DirWest;
                }

                if (y > frontier.FromY)
                {
                    conn[y, x] |= DirNorth;
                    conn[frontier.FromY, frontier.FromX] |= DirSouth;
                }
                else if (y < frontier.FromY)
                {
                    conn[y, x] |= DirSouth;
                    conn[frontier.FromY, frontier.FromX] |= DirNorth;
                }

                // 原版 RandomDungeon.hx:88-101 的候选计算（原版这个循环不写连通位，照抄）
                var candidates = new List<(int X, int Y, int FromX, int FromY)>
                {
                    (x + distanceBetween, y, x, y),
                    (x, y + distanceBetween, x, y),
                    (x - distanceBetween, y, x, y),
                    (x, y - distanceBetween, x, y)
                };

                candidates.RemoveAll(c => c.X < 0 || c.X >= width || c.Y < 0 || c.Y >= height);

                var connections = candidates.Count(c => room[c.Y, c.X] != EmptyRoom);

                while (connections < density && candidates.Count > 0)
                {
                    candidates.RemoveAt(Random.Shared.Next(candidates.Count));
                    connections += 1;
                }
            }

            var map = new int[height * width];
            var connectivity = new int[height * width];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    map[y * width + x] = room[y, x];
                    connectivity[y * width + x] = conn[y, x];
                }

            return (map, connectivity);
        }
        // Haxe Reset()   outputText 0 
        public void Reset()
        {

            Save.SetInt("wineDrunk", 0);
            Save.SetInt("lethiciteTaken", 0);
            Save.SetBool("booksTaken", false);
        }

        // Haxe TestResolve()   outputText 8 
        public void TestResolve(bool dungeon = false)
        {

            var virtue = false;
            if (Random.Shared.Next((int)(10)) + (CoC.Instance.Flags.Get(VanillaFlags.ShouldraFollowerState) == 1 ? 1 : 0) + (Player.HasPerk(VanillaPerks.Revelation) ? 1 : 0) >= 6)
            {
                virtue = true;
            }
            float buffType = Random.Shared.Next((int)(3));
            var __sw0_0 = buffType;
            if (__sw0_0 == 0)
            {
                if (virtue)
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 1, .9f, 5, 0);
                    OutputText("Scenes.Manor.TestResolve.StandOverAllChallenges".Localized());
                }
                else
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 2, 1.1f, 10, 0);
                    OutputText("Scenes.Manor.TestResolve.RememberThePain".Localized());
                }
            }
            if (__sw0_0 == 1)
            {
                if (virtue)
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 3, 10, 20, 0);
                    OutputText("Scenes.Manor.TestResolve.MomentOfUtterTerror".Localized());
                }
                else
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 4, 10, 20, 0);
                    OutputText("Scenes.Manor.TestResolve.AreYouDead".Localized());
                }
            }
            if (__sw0_0 == 2)
            {
                if (virtue)
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 5, 1.15f, 1.15f, 0);
                    OutputText("Scenes.Manor.TestResolve.VastTerrorsMotivate".Localized());
                }
                else
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 6, 0.85f, 0.85f, 0);
                    OutputText("Scenes.Manor.TestResolve.WeakApatheticVictory".Localized());
                }
            }
            if (__sw0_0 == 3)
            {
                if (virtue)
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 7, 0.85f, 0, 0);
                    OutputText("Scenes.Manor.TestResolve.AnotherTestOfConstitution".Localized());
                }
                else
                {
                    Player.CreateStatusEffect(VanillaStatusEffects.Resolve, 8, 0, 0, 0);
                    OutputText("Scenes.Manor.TestResolve.FacedWithTerrors".Localized());
                }
            }
            if (dungeon)
            {
                // 原版 Manor.hx:301-303 doNext(courtierFight)
                DoNext(CourtierFight);
            }
        }

        // Haxe EnterDungeon()   outputText 4 
        public void EnterDungeon()
        {
            ClearOutput();

            OutputText("Scenes.Manor.EnterDungeon.WalkUpDecrepitRoad".Localized());
            OutputText("Scenes.Manor.EnterDungeon.AreaStandsSilent".Localized());
            OutputText("Scenes.Manor.EnterDungeon.EnterManorOrLeave".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.FoundManor) < 1)
            {
                OutputText("Scenes.Manor.EnterDungeon.ManorNowAccessible".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.FoundManor, 1);
            }
            ClearButtons();
            AddButton(0, "Enter", EnterManor);
            AddButton(1, "Leave", ExitDungeon);
        }

        // Haxe EnterManor()   outputText 0 
        public void EnterManor()
        {

            ClearButtons();
DungeonSession.InDungeon = true;

            // 原版 dungeons.startAlternative(this, 7, "Old Manor")：设定当前地牢/初始格并初始化房间
            InitRooms();
            PlayerLoc = 7;
            SwitchFloor(1, 7);
            RunFunc();
        }

        // Haxe EnterVoid()   outputText 0 
        public void EnterVoid()
        {

DungeonSession.InDungeon = true;

            // 原版 dungeons.startAlternative(this, 7, "Old Manor")
            InitRooms();
            PlayerLoc = 7;
            InfinityLevel = 3;
            InfinityFunc = Infinity4;
            SwitchFloor(3, 0);
            RunFunc();
        }

        // Haxe RoomCourtyard()   outputText 6 
        public void RoomCourtyard()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) < 1)
            {
                OutputText("Scenes.Manor.RoomCourtyard.ApproachRottenDoor".Localized());
                OutputText("Scenes.Manor.RoomCourtyard.TremorCausesBrief".Localized());
                OutputText("Scenes.Manor.RoomCourtyard.DoorViolentlyDestroyed".Localized());
                OutputText("Scenes.Manor.RoomCourtyard.EnterCombatStance".Localized());
                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "bone_general")), RunFunc);
            }
            else
            {
                OutputText("Scenes.Manor.RoomCourtyard.StandAtDecrepitCourtyard".Localized());
                OutputText("Scenes.Manor.RoomCourtyard.CrowsGatherDeadTrees".Localized());
                AddButton(11, "Leave", ExitDungeon);
            }
        }

        // Haxe LoseToGeneral()   outputText 3 
        public void LoseToGeneral()
        {
            ClearOutput();

            CombatScene.CleanupAfterCombat();
            OutputText("Scenes.Manor.LoseToGeneral.CollapseTooWeak".Localized());
            OutputText("Scenes.Manor.LoseToGeneral.ShamblesTowardsYou".Localized());
            OutputText("Scenes.Manor.LoseToGeneral.DeadMoreBlood".Localized());
            GameOver();
        }

        // Haxe DefeatGeneral()   outputText 2 
        public void DefeatGeneral()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 0));
            CombatScene.CleanupAfterCombat();
            OutputText("Scenes.Manor.DefeatGeneral.FinalStrikeMammoth".Localized());
            OutputText("Scenes.Manor.DefeatGeneral.PressForwardInto".Localized());
            DoNext(RunFunc);
        }

        // Haxe DefeatCourtier()   outputText 6 
        public void DefeatCourtier()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 2));
            OutputText("Scenes.Manor.DefeatCourtier.WeakenedSkeletonAttempts".Localized());
            if ((Player.StatusEffectValue(VanillaStatusEffects.Resolve, 0) % 2) == 0)
            {
                OutputText("Scenes.Manor.DefeatCourtier.HazeOfTerror".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.DefeatCourtier.EnergizingVirtuousThoughts".Localized());
            }
            OutputText("Scenes.Manor.DefeatCourtier.RemindedOfThe".Localized());
            OutputText("Scenes.Manor.DefeatCourtier.BriefMomentWonder".Localized());
            OutputText("Scenes.Manor.DefeatCourtier.NewMagicalAbilityGained".Localized());
            CombatScene.CleanupAfterCombat();
            DoNext(RunFunc);
        }

        // Haxe LoseToCourtier()   outputText 8 
        public void LoseToCourtier()
        {
            ClearOutput();

            CombatScene.CleanupAfterCombat();
            OutputText("Scenes.Manor.LoseToCourtier.FallOverwhelmedBy".Localized());
            if (Player.Health < 1)
            {
                OutputText("Scenes.Manor.LoseToCourtier.YourWoundsWord".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.LoseToCourtier.LustRagingThrough".Localized());
            }
            OutputText("Scenes.Manor.LoseToCourtier.UndeadWizardTooMuch".Localized());
            OutputText("Scenes.Manor.LoseToCourtier.WakeUpDazedBound".Localized());
            OutputText("Scenes.Manor.LoseToCourtier.ApproachesYouSilent".Localized());
            if (Player.Intelligence < 80)
            {
                OutputText("Scenes.Manor.LoseToCourtier.FewMomentsOfIncredible".Localized());
                DoNext(LoseToCourtierSkeletonized);
            }
            else
            {
                OutputText("Scenes.Manor.LoseToCourtier.MomentsIncrediblePain".Localized());
                DoNext(LoseToCourtierEphraimd);
            }
        }

        // Haxe LoseToCourtierEphraimd()   outputText 3 
        public void LoseToCourtierEphraimd()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LoseToCourtierEphraimd.WakeUpOnBed".Localized());
            OutputText("Scenes.Manor.LoseToCourtierEphraimd.LookAtYourself".Localized());
            OutputText("Scenes.Manor.LoseToCourtierEphraimd.EphraimYouAreEphraim".Localized());
            GameOver();
        }

        // Haxe LoseToCourtierSkeletonized()   outputText 3 
        public void LoseToCourtierSkeletonized()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LoseToCourtierSkeletonized.RegainSensesAgain".Localized());
            OutputText("Scenes.Manor.LoseToCourtierSkeletonized.BriefStateOf".Localized());
            OutputText("Scenes.Manor.LoseToCourtierSkeletonized.AnotherLifeWasted".Localized());
            GameOver();
        }

        // Haxe LoseToJester()   outputText 5 
        public void LoseToJester()
        {
            ClearOutput();

            CombatScene.CleanupAfterCombat();
            OutputText("Scenes.Manor.LoseToJester.FallOverwhelmedBy".Localized());
            if (Player.Health < 1)
            {
                OutputText("Scenes.Manor.LoseToJester.YourWoundsWord".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.LoseToJester.LustRagingThrough".Localized());
            }
            OutputText("Scenes.Manor.LoseToJester.JesterJumpsOnTable".Localized());
            OutputText("Scenes.Manor.LoseToJester.UncannyStrengthSlices".Localized());
            DoNext(LoseToJesterDullahand);
        }

        // Haxe DefeatJester()   outputText 2 
        public void DefeatJester()
        {
            ClearOutput();

            OutputText("Scenes.Manor.DefeatJester.NoticingImminentDefeat".Localized());
            OutputText("Scenes.Manor.DefeatJester.EvidenceSkeleton".Localized());
            CombatScene.CleanupAfterCombat();
            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 3));
            DoNext(RunFunc);
        }

        // Haxe LoseToJesterDullahand()   outputText 7 
        public void LoseToJesterDullahand()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LoseToJesterDullahand.WakeUpScreaming".Localized());
            OutputText("Scenes.Manor.LoseToJesterDullahand.SomethingMovesInto".Localized());
            OutputText("Scenes.Manor.LoseToJesterDullahand.HissesForMoment".Localized());
            OutputText("Scenes.Manor.LoseToJesterDullahand.NearlyBlindedBy".Localized());
            OutputText("Scenes.Manor.LoseToJesterDullahand.AttemptMoveAgain".Localized());
            OutputText("Scenes.Manor.LoseToJesterDullahand.BrieflyOverwhelmHorror".Localized());
            OutputText("Scenes.Manor.LoseToJesterDullahand.SoonTurnedIntoA".Localized());
            GameOver();
        }

        // Haxe MainHall()   outputText 2 
        public void MainHall()
        {
            ClearOutput();

            OutputText("Scenes.Manor.MainHall.StandAtSpaciousMain".Localized());
            OutputText("Scenes.Manor.MainHall.TatteredRegalRug".Localized());
        }

        // Haxe DiningRoom()   outputText 12 
        public void DiningRoom()
        {
            ClearOutput();

            OutputText("Scenes.Manor.DiningRoom.SpaciousDining".Localized());
            if (((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 8) != 0) && !((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 16) != 0))
            {
                OutputText("Scenes.Manor.DiningRoom.SeeDarkCurvedDagger".Localized());
                AddButton(1, "Take Dagger", () => InventoryScene.TakeItem(Identifier.Parse("coc:cursed_dagger"), TookDagger, RunFunc));
            }
            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 4) != 0) || ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 8) != 0) || ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 64) != 0))
            {
                var __sw0_1 = Save.GetInt("wineDrunk", 0);
                if (__sw0_1 == 0)
                {
                    OutputText("Scenes.Manor.DiningRoom.WineRackAtBack".Localized());
                }
                if (__sw0_1 == 1)
                {
                    OutputText("Scenes.Manor.DiningRoom.BottleOpenedBeforeSits".Localized());
                }
                if (__sw0_1 == 2)
                {
                    OutputText("Scenes.Manor.DiningRoom.BottleOpenedBefore".Localized());
                }
                if (__sw0_1 == 3)
                {
                    OutputText("Scenes.Manor.DiningRoom.BottleOnDiningTable".Localized().Replace("[if (silly) { It's taunting you. It thinks it can win this fight. Go prove it wrong! Kill it.}]", silly ? " It's taunting you. It thinks it can win this fight. Go prove it wrong! Kill it." : ""));
                }
                if (__sw0_1 == 4)
                {
                    OutputText("Scenes.Manor.DiningRoom.LastWineBottleNow".Localized());
                }
                if (Save.GetInt("wineDrunk", 0) >= 4)
                    AddButtonDisabled(0, "Drink Wine", "The bottle is empty.");
                else
                    AddButton(0, "Drink Wine", DrinkWine, "Drink some of the wine.");
            }
            else
            {
                OutputText("Scenes.Manor.DiningRoom.ExploreDiningRoom".Localized());
                OutputText("Scenes.Manor.DiningRoom.AgainHearNoise".Localized());
                OutputText("Scenes.Manor.DiningRoom.MoveAroundTrying".Localized());
                if (Player.Speed > 70)
                {
                    OutputText("Scenes.Manor.DiningRoom.ManageToTurnAround".Localized());
                }
                else
                {
                    OutputText("Scenes.Manor.DiningRoom.FailToTurn".Localized(Player.HasPerk(VanillaPerks.BleedImmune) ? "bleeding and " : ""));
                    // 原版 Manor.hx:505-507：player.reduceDamage(45 + rand(30), new BoneJester()) + player.bleed(...)。
                    // C# 的减伤只在 CombatCalculator.ReduceDamage(context, ...) 里（需要 CombatContext），
                    // 这场偷袭发生在离场对话里没有 context，所以按原始伤害结算；差异已登记在报告。
                    var damage = 45 + Random.Shared.Next(30);
                    Player.HealthChange(-damage);
                    Player.Bleed(EnemyRegistry.Create(new Identifier("coc", "bone_jester")));
                }
                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "bone_jester")), RunFunc);
            }
        }

        // Haxe TookDagger()   outputText 0 
        public void TookDagger()
        {

            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 4));
            RunFunc();
        }

        // Haxe DrinkWine()   outputText 10 
        public void DrinkWine()
        {
            ClearOutput();

            var __sw0_0 = 1 + Save.GetInt("wineDrunk", 0);
            if (__sw0_0 == 1)
            {
                OutputText("Scenes.Manor.DrinkWine.UncorkAndSmell".Localized());
                OutputText("Scenes.Manor.DrinkWine.TastesGreat".Localized());
            }
            if (__sw0_0 == 2)
            {
                OutputText("Scenes.Manor.DrinkWine.DespiteBetterJudgment".Localized());
                OutputText("Scenes.Manor.DrinkWine.DelightfulAroma".Localized(silly ? "Does this mean you're an alcoholic?" : ""));
            }
            if (__sw0_0 == 3)
            {
                OutputText("Scenes.Manor.DrinkWine.PouringMoreWine".Localized());
                OutputText("Scenes.Manor.DrinkWine.FleetingMomentOfLucidity".Localized());
                Player.Lust = 0;
            }
            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Manor.DrinkWine.StareAtBottleIntensely".Localized());
                OutputText("Scenes.Manor.DrinkWine.TearsWellUp".Localized());
                OutputText("Scenes.Manor.DrinkWine.DamnThisHorridPlace".Localized());
                Player.Lust = 0;
            }
            if (true /* default */)
            {
                OutputText("Scenes.Manor.DrinkWine.BottleIsEmpty".Localized().Replace("[if (silly) { As are you.}]", silly ? " As are you." : ""));
            }
            Save.SetInt("wineDrunk", Save.GetInt("wineDrunk", 0) + 1);
            if (Save.GetInt("wineDrunk", 0) < 5)
            {
                Player.HealthChange(Player.MaxHealth / 10);
                Player.ApplyStats(new DynStat(VanillaStats.Strength, -1f), new DynStat(VanillaStats.Corruption, 2f), new DynStat(VanillaStats.Toughness, -1f), new DynStat(VanillaStats.Intelligence, -1f), new DynStat(VanillaStats.Speed, -1f));
            }
            DoNext(RunFunc);
        }

        // Haxe LibraryRoom()   outputText 2 
        public void LibraryRoom()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LibraryRoom.WellStockedLibrary".Localized());
            OutputText("Scenes.Manor.LibraryRoom.NorthernBookshelfIs".Localized());
            AddButton(0, "North Shelf", NorthBookshelf);
            if (!Save.GetBool("booksTaken"))

                AddButton(1, "Take Books", TakeBooks, "You might be able to find some use for these.");
        }

        // Haxe TakeBooks()   outputText 3 
        public void TakeBooks()
        {
            ClearOutput();

            Save.SetBool("booksTaken", true);
            OutputText("Scenes.Manor.TakeBooks.NotSureExactlyWhere".Localized());
            OutputText("Scenes.Manor.TakeBooks.CollectTomesGently".Localized().Replace("[if (str < 40) {is a bit much, but you'll make do|doesn't bother you [if (str < 70) {much|at all}]}]", Player.Strength < 40 ? "is a bit much, but you'll make do" : Player.Strength < 70 ? "doesn't bother you much" : "doesn't bother you at all"));
            OutputText("Scenes.Manor.TakeBooks.OldManorBooks".Localized());
            Player.CreateKeyItem("Old Manor Books");
            DoNext(RunFunc);
        }

        // Haxe LibrarySecret()   outputText 4 
        public void LibrarySecret()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LibrarySecret.InsideSecretRoom".Localized());
            if ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 2048) != 0)
            {
                OutputText("Scenes.Manor.LibrarySecret.SeePassageToThe".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.LibrarySecret.NorthernWallThers".Localized());
            }
            OutputText("Scenes.Manor.LibrarySecret.AlsoNoticeScatteredPages".Localized());
            if ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 2048) != 0)
            {
                SetStairButtons(null, () => SwitchFloor(0, 35));
            }
            AddButton(0, "Pages", () => FloorPages());
            // 原版 .hideIf(!player.hasKeyItem("Family Talisman") || progress & 2048)

            if (Player.HasKeyItem("Family Talisman") && ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 2048) == 0))

                AddButton(1, "Insert Talisman", OpenStairs, "Insert the talisman you found into the slot.");
        }

        // Haxe OpenStairs()   outputText 2 
        public void OpenStairs()
        {
            ClearOutput();

            OutputText("Scenes.Manor.OpenStairs.PressTheTalisman".Localized());
            OutputText("Scenes.Manor.OpenStairs.PeerIntoGap".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 11));
            RemakeMaps();
            DoNext(RunFunc);
        }

        // Haxe Tunnels1()   outputText 5 
        public void Tunnels1()
        {
            ClearOutput();

            if (PrevLoc == 0)
            {
                if ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0)
                {
                    OutputText("Scenes.Manor.Tunnels1.MoveThroughGateway".Localized());
                }
                else
                {
                    OutputText("Scenes.Manor.Tunnels1.GoDownCarefully".Localized());
                    OutputText("Scenes.Manor.Tunnels1.StandAtBeginning".Localized());
                    OutputText("Scenes.Manor.Tunnels1.AirFeelsCool".Localized(Player.HasPerk(VanillaPerks.HistoryAlchemist) ? " You inhale more deeply and try to identify the source of the smell. Embalming fluids, you think to yourself." : " Whatever the source is, its purpose can only be grim."));
                }
            }
            else
            {
                OutputText("Scenes.Manor.Tunnels1.ReachEndOfThe".Localized());
            }
            SetStairButtons(() => SwitchFloor(1, 0));
        }

        // Haxe Tunnels2()   outputText 10 
        public void Tunnels2()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Tunnels2.MoveThroughTunnelSlowly".Localized());
            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 64) != 0))
            {
                OutputText("Scenes.Manor.Tunnels2.SuddenlyHearSound".Localized());
                OutputText("Scenes.Manor.Tunnels2.OfRattlingBones".Localized());
                OutputText("Scenes.Manor.Tunnels2.OfEldritchChanting".Localized());
                OutputText("Scenes.Manor.Tunnels2.MoveForwardFewFeet".Localized());
                OutputText("Scenes.Manor.Tunnels2.PrepareToFight".Localized());
                if ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 8) != 0)
                {
                    OutputText("Scenes.Manor.Tunnels2.IntuitionLeadsYou".Localized());
                }
                else
                {
                    OutputText("Scenes.Manor.Tunnels2.IntuitionLeadsYouAgain".Localized());
                }
                OutputText("Scenes.Manor.Tunnels2.NoWayAroundIt".Localized());
                var desc = "Before you stands a veritable court of reanimated skeletons. The Bone Guardian stands in front, protecting the rest with its massive armored body. The Bone Jester moves in and out of sight quickly, attempting to lose your focus. Standing behind all of them is the Bone Sorcerer, preparing arcane spells to weaken you so that his companions can finish you off.";
                // 原版 Manor.hx:630 startCombatMultiple(BoneGeneral, BoneJester, BoneCourtier, null, defeatCourt, loseToCourt, defeatCourt, loseToCourt, desc)
                var general = EnemyRegistry.Create(new Identifier("coc", "bone_general"));
                var jester = EnemyRegistry.Create(new Identifier("coc", "bone_jester"));
                var courtier = EnemyRegistry.Create(new Identifier("coc", "bone_courtier"));

                general.DescriptionOverride = desc;
                general.OnDefeated = _ => DefeatCourt();
                general.OnWon = _ => LoseToCourt();

                CombatScene.StartMultiple([general, jester, courtier], RunFunc);
            }
            else
            {
                if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 8) != 0) && !((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 16) != 0))
                {
                    OutputText("Scenes.Manor.Tunnels2.SeeDarkCurvedDagger".Localized());
                    AddButton(1, "Take Dagger", () => InventoryScene.TakeItem(Identifier.Parse("coc:cursed_dagger"), TookDagger, RunFunc));
                }
            }
        }

        // Haxe LoseToCourt()   outputText 5 
        public void LoseToCourt()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LoseToCourt.StaggerBackwards".Localized());
            OutputText("Scenes.Manor.LoseToCourt.CoughBloodCrippled".Localized());
            OutputText("Scenes.Manor.LoseToCourt.SlipAndFall".Localized());
            OutputText("Scenes.Manor.LoseToCourt.LifeLeavesYou".Localized());
            OutputText("Scenes.Manor.LoseToCourt.MoreDustMoreAshes".Localized());
            GameOver();
            CombatScene.CleanupAfterCombat();
        }

        // Haxe DefeatCourt()   outputText 4 
        public void DefeatCourt()
        {
            ClearOutput();

            OutputText("Scenes.Manor.DefeatCourt.ColossalArmoredStumbles".Localized());
            OutputText("Scenes.Manor.DefeatCourt.AfterDefeatingArmored".Localized());
            OutputText("Scenes.Manor.DefeatCourt.StandAndLook".Localized());
            OutputText("Scenes.Manor.DefeatCourt.PantExhaustedLook".Localized());
            CombatScene.CleanupAfterCombat();
            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 6));
            DoNext(RunFunc);
        }

        // Haxe LethiciteTunnel1()   outputText 2 
        public void LethiciteTunnel1()
        {
            ClearOutput();

            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 512) != 0))
            {
                OutputText("Scenes.Manor.LethiciteTunnel1.AtEndOfAdjacent".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.LethiciteTunnel1.CrystalDestroyedTunnel".Localized());
            }
        }

        // Haxe LethiciteTunnel2()   outputText 4 
        public void LethiciteTunnel2()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LethiciteTunnel2.EnterChamberAtThe".Localized());
            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 512) != 0) && !((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0))
            {
                OutputText("Scenes.Manor.LethiciteTunnel2.CenterStands".Localized());
                AddButton(0, "Destroy", () => CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "lethicite_crystal")), RunFunc), "Attempt to destroy the crystal. This lethicite is probably powering some kind of spell, but destroying raw lethicite is nigh-impossible by conventional means.");
            }
            else
            {
                OutputText("Scenes.Manor.LethiciteTunnel2.CenterStandsAgain".Localized());
                if ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0 && !((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 512) != 0))
                {
                    OutputText("Scenes.Manor.LethiciteTunnel2.PerhapsBoreSomeType".Localized());
                }

            }
        }

        // Haxe LoseToCrystal()   outputText 7 
        public void LoseToCrystal()
        {
            ClearOutput();

            CombatScene.CleanupAfterCombat();
            OutputText("Scenes.Manor.LoseToCrystal.FallDownTooAroused".Localized());
            OutputText("Scenes.Manor.LoseToCrystal.ImmediatelyAttemptMasturbate".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Manor.LoseToCrystal.EjaculatingThickStrands".Localized());
            }
            if (Player.HasVagina())
            {
                OutputText("Scenes.Manor.LoseToCrystal.SquirtingUncannyAmounts".Localized());
            }
            OutputText("Scenes.Manor.LoseToCrystal.BeforeYouCanEnjoy".Localized());
            OutputText("Scenes.Manor.LoseToCrystal.DespiteMindCompletely".Localized());
            OutputText("Scenes.Manor.LoseToCrystal.StandUpTrembling".Localized());
            DoNext(LoseToCrystal2);
        }

        // Haxe LoseToCrystal2()   outputText 3 
        public void LoseToCrystal2()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(VanillaStats.Lust, 999f), new DynStat(VanillaStats.Corruption, 999f));
            OutputText("Scenes.Manor.LoseToCrystal2.WhileUnconsciousTowering".Localized());
            OutputText("Scenes.Manor.LoseToCrystal2.HissesStandsUpAttaches".Localized());
            OutputText("Scenes.Manor.LoseToCrystal2.LeavesShortlyAfterwards".Localized());
            GameOver();
        }

        // Haxe DestroyCrystal()   outputText 3 
        public void DestroyCrystal()
        {
            ClearOutput();

            OutputText("Scenes.Manor.DestroyCrystal.CrystalBeginsToRespond".Localized());
            OutputText("Scenes.Manor.DestroyCrystal.BeginsVibratingSoftly".Localized());
            OutputText("Scenes.Manor.DestroyCrystal.WhenYouHitGround".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 9));
            CombatScene.CleanupAfterCombat();
            DoNext(RunFunc);
        }

        // Haxe Tunnels3()   outputText 2 
        public void Tunnels3()
        {
            ClearOutput();

            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0))
            {
                OutputText("Scenes.Manor.Tunnels3.WantedToFreeMyself".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.Tunnels3.DespiteNecromancerDefeat".Localized());
            }
        }

        // Haxe Tunnels4()   outputText 2 
        public void Tunnels4()
        {
            ClearOutput();

            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0))
            {
                OutputText("Scenes.Manor.Tunnels4.WorkersEvelyn".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.Tunnels4.DespiteNecromancerDefeat".Localized());
            }
        }

        // Haxe Tunnels5()   outputText 2 
        public void Tunnels5()
        {
            ClearOutput();

            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0))
            {
                OutputText("Scenes.Manor.Tunnels5.RegardlessOfEthics".Localized());
            }
            else
            {
                OutputText("Scenes.Manor.Tunnels5.DespiteNecromancerDefeat".Localized());
            }
        }

        // Haxe TheChamber()   outputText 6 
        public void TheChamber()
        {
            ClearOutput();

            var necroDesc = "Before you stands a towering monstrosity, a being of nightmares. The Necromancer, covered in a red cloak, breathes slowly, sure of his impending victory. Around him stand several piles of bones from which skeletons are assembled to attack you.";
            if ((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 512) != 0)
            {
                necroDesc += "\n\nWith the lethicite crystal destroyed, his summoning ability is weakened.\n\n";
            }
            OutputText("Scenes.Manor.TheChamber.ApproachMassiveChamber".Localized());
            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 128) != 0))
            {
                OutputText("Scenes.Manor.TheChamber.LaidOverTorchAltar".Localized());
                OutputText("Scenes.Manor.TheChamber.TurnsToFaceYou".Localized());
                OutputText("Scenes.Manor.TheChamber.FigureLiftsArm".Localized());
                // 原版 Manor.hx:765 startCombatMultiple(Necromancer, SkeletonHorde, null, null, defeatNecro, loseToNecro, defeatNecro, loseToNecro, necroDesc)
                var necromancer = EnemyRegistry.Create(new Identifier("coc", "necromancer"));
                var horde = EnemyRegistry.Create(new Identifier("coc", "skeleton_horde"));

                necromancer.DescriptionOverride = necroDesc;

                CombatScene.StartMultiple([necromancer, horde], RunFunc);
            }
            else
            {
                if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 256) != 0))
                {
                    OutputText("Scenes.Manor.TheChamber.SeeDarkCrystal".Localized());
                    AddButton(1, "Take Crystal", () => InventoryScene.TakeItem(VanillaUseables.AbyssalShard, TookCrystal, RunFunc));
                }
                OutputText("Scenes.Manor.TheChamber.NearbyTableYouSee".Localized());
                AddButton(0, "Read Pages", () => TablePages());
            }
        }

        // Haxe TablePages()   outputText 1 
        public void TablePages(int currentlyRead = -1)
        {

            if (!((CoC.Instance.Flags.Get(VanillaFlags.CodexEphraimJournal) & 4) != 0))
            {
                CoC.Instance.Flags.Set(VanillaFlags.CodexEphraimJournal, CoC.Instance.Flags.Get(VanillaFlags.CodexEphraimJournal) + 4);
                OutputText("Scenes.Manor.TablePages.NewCodexEntryManor".Localized());
            }
            ClearButtons();
            AddButton(0, "Journal IX", () => Journal(8, TablePages));
            AddButton(1, "Journal X", () => Journal(9, TablePages));
            AddButton(14, "Back", RunFunc);
        }

        // Haxe DefeatNecro()   outputText 4 
        public void DefeatNecro()
        {
            ClearOutput();

            OutputText("Scenes.Manor.DefeatNecro.ToweringNecromancerStumbles".Localized());
            OutputText("Scenes.Manor.DefeatNecro.MoveForwardToStrike".Localized());
            OutputText("Scenes.Manor.DefeatNecro.FewMomentsOfAgony".Localized());
            OutputText("Scenes.Manor.DefeatNecro.HaveDefeatedNecromancer".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 7));
            Player.UpgradeBeautifulSword();
            CombatScene.CleanupAfterCombat();
            DoNext(RunFunc);
        }

        // Haxe LoseToNecro()   outputText 4 
        public void LoseToNecro()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LoseToNecro.FallOverwhelmedByNecromancer".Localized());
            OutputText("Scenes.Manor.LoseToNecro.NecromancerApproaches".Localized());
            if (Player.Intelligence < 80)
            {
                OutputText("Scenes.Manor.LoseToNecro.FewMomentsOfIncredible".Localized());
                DoNext(LoseToCourtierSkeletonized);
            }
            else
            {
                OutputText("Scenes.Manor.LoseToNecro.MomentsIncrediblePain".Localized());
                DoNext(LoseToCourtierEphraimd);
            }
        }

        // Haxe TookCrystal()   outputText 0 
        public void TookCrystal()
        {

            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 8));
            RunFunc();
        }

        // Haxe UseCrystal()   outputText 5 
        public bool UseCrystal()
        {
            ClearOutput();

            if (IsInChamber() && (View.Time.Hours >= 19 || View.Time.Hours == 0))
            {
                // 原版 inventory.callNext = runFunc：C# 的 useCrystal 由 AbyssalShardBehavior 接到 RunFunc
                OutputText("Scenes.Manor.UseCrystal.LiftCrystalAndPeer".Localized());
                OutputText("Scenes.Manor.UseCrystal.WidenYourEyes".Localized());
                OutputText("Scenes.Manor.UseCrystal.RoomTurnsPitchBlack".Localized());
                SwitchFloor(3, 0, false);
                return true;
            }
            else
            {
                OutputText("Scenes.Manor.UseCrystal.LiftCrystalAgain".Localized());
                OutputText("Scenes.Manor.UseCrystal.HardToTearAway".Localized());
                return false;
            }
        }

        // Haxe Infinity1()   outputText 2 
        public void Infinity1()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Infinity1.InADarkExpanse".Localized());
            OutputText("Scenes.Manor.Infinity1.AbyssWelcomesYou".Localized());
            InfinityLevel = 0;
            RemakeMaps();
            SetInfinityButtons(Infinity1, Infinity2, Infinity1, Infinity1);
        }

        // Haxe Infinity2()   outputText 3 
        public void Infinity2()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Infinity2.HowLongHere".Localized());
            OutputText("Scenes.Manor.Infinity2.WhenConfrontedWithReality".Localized());
            OutputText("Scenes.Manor.Infinity2.ChaseTheSettingSun".Localized());
            InfinityLevel = 1;
            InfinityFunc = Infinity2;
            RemakeMaps();
            PlayerLoc = FindEntrance();
            SetInfinityButtons(Infinity1, Infinity2, Infinity3, Infinity1);
        }

        // Haxe Infinity3()   outputText 2 
        public void Infinity3()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Infinity3.WhatAreYou".Localized());
            OutputText("Scenes.Manor.Infinity3.TurnBackSeeInfinity".Localized());
            InfinityLevel = 2;
            InfinityFunc = Infinity3;
            RemakeMaps();
            PlayerLoc = FindEntrance();
            SetInfinityButtons(Infinity2, Infinity1, Infinity2, Infinity4);
        }

        // Haxe Infinity4()   outputText 3 
        public void Infinity4()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Infinity4.NothingButAPart".Localized());
            OutputText("Scenes.Manor.Infinity4.AllReturnInTime".Localized());
            OutputText("Scenes.Manor.Infinity4.FeelAsIfYoure".Localized());
            InfinityLevel = 3;
            InfinityFunc = Infinity4;
            RemakeMaps();
            PlayerLoc = FindEntrance();
            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 1024) != 0))
            {
                CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 10));
            }
            SetInfinityButtons(Infinity5, Infinity2, Infinity2, Infinity1);
            AddButton(0, "The Camp", ReturnCamp);
        }

        // Haxe ReturnCamp()   outputText 0 
        public void ReturnCamp()
        {


            DungeonSession.End();

            // 原版 game.dungeons.usingAlternative = false（C# 该状态在 DungeonSession 里，退出时已 End）
            Camp.ReturnToCamp(1);
        }

        // Haxe Infinity5()   outputText 3 
        public void Infinity5()
        {
            ClearOutput();

            InfinityLevel = 4;
            InfinityFunc = Infinity5;
            RemakeMaps();
            PlayerLoc = FindEntrance();
            OutputText("Scenes.Manor.Infinity5.AtSomeKindOf".Localized());
            OutputText("Scenes.Manor.Infinity5.CreatureTurnsToFace".Localized());
            OutputText("Scenes.Manor.Infinity5.DoNotKnowWhere".Localized());
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "nameless_horror")), RunFunc);
        }

        // Haxe Banish()   outputText 7 
        public void Banish(bool defeatedHorror = false)
        {
            ClearOutput();

            if (Player.StatusEffectValue(VanillaStatusEffects.TFSupercharging, 1) > 0
                && !CoC.Instance.UnlockedAchievements.Contains(VanillaAchievements.NightSun.Id))
            {
                // 原版 Manor.hx:902-905：先解锁 Night Sun 成就，再重跑 banish
                AwardAchievement(VanillaAchievements.NightSun);
                DoNext(() => Banish());
                return;
            }
            OutputText("Scenes.Manor.Banish.CreatureShrieksLoudly".Localized());
            OutputText("Scenes.Manor.Banish.PointsOneOfTentacles".Localized());
            OutputText("Scenes.Manor.Banish.AttackedByAnemone".Localized());
            OutputText("Scenes.Manor.Banish.FromShardOfOne".Localized());
            OutputText("Scenes.Manor.Banish.GrowsToTakeShapes".Localized());
            OutputText("Scenes.Manor.Banish.UniverseZoomsIn".Localized());
            OutputText("Scenes.Manor.Banish.PerkGainedRevelation".Localized());
            if (!Player.HasPerk(VanillaPerks.Revelation))
            {
                Player.CreatePerk(VanillaPerks.Revelation, 0, 0, 0, 0);
            }
            if (defeatedHorror)
            {
                // 原版 Manor.hx:917-919：awardAchievement("Lord Shoggoth Postulate", ..., true, true)
                AwardAchievement(VanillaAchievements.LordBritishPostulate, true, true, true);
            }
            CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & ~1024);

            // 原版 banish() 走 combat.cleanupAfterCombat()（Manor.hx:920），其中含 clearStatuses()；
            // C# 的 CleanupAfterCombat 只做脱战状态与回营地，所以在这里补战斗状态清理。
            Player.ClearCombatStatuses();

            CombatScene.CleanupAfterCombat();
            SwitchFloor(1, 7, false);
            DoNext(RunFunc);
        }

        // Haxe LoseToNamelessHorror()   outputText 3 
        public void LoseToNamelessHorror()
        {
            ClearOutput();

            OutputText("Scenes.Manor.LoseToNamelessHorror.FallDefeatedByHorror".Localized());
            OutputText("Scenes.Manor.LoseToNamelessHorror.StareUponItsFace".Localized());
            OutputText("Scenes.Manor.LoseToNamelessHorror.NoHope".Localized());
            CombatScene.CleanupAfterCombat();
            GameOver();
        }

        // Haxe FloorPages()   outputText 1 
        public void FloorPages(int currentlyRead = -1)
        {

            if (!((CoC.Instance.Flags.Get(VanillaFlags.CodexEphraimJournal) & 2) != 0))
            {
                CoC.Instance.Flags.Set(VanillaFlags.CodexEphraimJournal, CoC.Instance.Flags.Get(VanillaFlags.CodexEphraimJournal) + 2);
                OutputText("Scenes.Manor.FloorPages.NewCodexEntryManor".Localized());
            }
            ClearButtons();
            AddButton(0, "Journal VI", () => Journal(5, FloorPages));
            AddButton(1, "Journal VII", () => Journal(6, FloorPages));
            AddButton(2, "Journal VIII", () => Journal(7, FloorPages));
            AddButton(14, "Back", RunFunc);
        }

        // Haxe NorthBookshelf()   outputText 3 
        public void NorthBookshelf()
        {
            ClearOutput();

            if (Code == 7 && !((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 32) != 0))
            {
                OutputText("Scenes.Manor.NorthBookshelf.PlacingLastBookBack".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 5));
                RemakeMaps();
            }
            if (!((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 32) != 0) && Codespell != "")
            {
                OutputText("Scenes.Manor.NorthBookshelf.CodespellWord".Localized()
                    Codespell
                );
            }
            ClearButtons();
            OutputText("Scenes.Manor.NorthBookshelf.ApproachOddBookshelf".Localized());
            AddButton(0, "Aurora Consu.", () => Keyword(0));
            AddButton(1, "Ershaw Shards", () => Keyword(1));
            AddButton(2, "Galdrabohk", () => Keyword(2));
            AddButton(3, "Heptarchia M.", () => Keyword(3));
            AddButton(4, "Incisis Aureus", () => Keyword(4));
            AddButton(5, "Liber Ivonis", () => Keyword(5));
            AddButton(6, "Musaeum Herm.", () => Keyword(6));
            AddButton(7, "Pneki Acc.", () => Keyword(7));
            AddButton(8, "Qabalum Max.", () => Keyword(8));
            AddButton(9, "Revelations of G.", () => Keyword(9));
            AddButton(14, "Back", RunFunc);
        }

        // Haxe Keyword()   outputText 32 
        public void Keyword(int letter = 0)
        {
            ClearOutput();

            var __sw0_0 = letter;
            if (__sw0_0 == 0)
            {
                OutputText("Scenes.Manor.Keyword.OpenAuroraConsurgens".Localized(Player.HasPerk(VanillaPerks.HistoryAlchemist) ? " The amount of glaring mistakes makes you cringe." : " You fail to understand most of it."));
                if (Code == 4)
                {
                    Code += 1;
                    Codespell += " A";
                    OutputText("Scenes.Manor.Keyword.PushBookIntoSlot".Localized());
                }
                else
                {
                    Code = 0;
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotAgain".Localized());
                    }
                    Codespell = "";
                }
            }
            if (__sw0_0 == 1)
            {
                OutputText("Scenes.Manor.Keyword.OpenErshawShards".Localized());
                if (Code == 0)
                {
                    Code += 1;
                    Codespell += "E";
                    OutputText("Scenes.Manor.Keyword.PushBookSlotThird".Localized());
                }
                else
                {
                    Code = 0;
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotFourth".Localized());
                    }
                    Codespell = "";
                }
            }
            if (__sw0_0 == 2)
            {
                OutputText("Scenes.Manor.Keyword.OpenGalbrabohk".Localized());
                if (!Player.HasStatusEffect(VanillaStatusEffects.KnowsWhitefire) && Player.Intelligence > 60)
                {
                    OutputText("Scenes.Manor.Keyword.OneChapterUsefulReadable".Localized());
                    Player.CreateStatusEffect(VanillaStatusEffects.KnowsWhitefire, 0, 0, 0, 0);
                }
                if (Codespell.Length > 0)
                {
                    OutputText("Scenes.Manor.Keyword.PushBookSlotFifth".Localized());
                }
                Codespell = "";
                Code = 0;
            }
            if (__sw0_0 == 3)
            {
                OutputText("Scenes.Manor.Keyword.OpenHeptarchiaMystica".Localized());
                if (Player.Intelligence >= 100 && !Player.HasPerk(VanillaPerks.MysticLearnings))
                {
                    OutputText("Scenes.Manor.Keyword.DespiteIncredibleComplexity".Localized());
                    OutputText("Scenes.Manor.Keyword.PerkAcquiredMysticLearnings".Localized());
                    Player.CreatePerk(VanillaPerks.MysticLearnings, 0, 0, 0, 0);
                }
                else
                {
                    if (Player.Intelligence < 100)
                    {
                        OutputText("Scenes.Manor.Keyword.AlthoughCouldGatherInfo".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Manor.Keyword.AlreadyAcquiredAll".Localized());
                    }
                }
                if (Code == 2)
                {
                    Code += 1;
                    Codespell += " H";
                    OutputText("Scenes.Manor.Keyword.PushBookSlotSixth".Localized());
                }
                else
                {
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotSeventh".Localized());
                    }
                    Codespell = "";
                    Code = 0;
                }
            }
            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Manor.Keyword.OpenIncisisAureus".Localized());
                if (Code == 5)
                {
                    Code += 1;
                    Codespell += " I";
                    OutputText("Scenes.Manor.Keyword.PushBookSlotEighth".Localized());
                }
                else
                {
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotNinth".Localized());
                    }
                    Codespell = "";
                    Code = 0;
                }
            }
            if (__sw0_0 == 5)
            {
                OutputText("Scenes.Manor.Keyword.OpenLiberIvonis".Localized());
                if (Codespell.Length > 0)
                {
                    OutputText("Scenes.Manor.Keyword.PushBookSlotTenth".Localized());
                }
                Code = 0;
                Codespell = "";
            }
            if (__sw0_0 == 6)
            {
                OutputText("Scenes.Manor.Keyword.OpenMusaeumHermeticum".Localized());
                if (Code == 6)
                {
                    Code += 1;
                    Codespell += " M";
                    OutputText("Scenes.Manor.Keyword.PushBookSlotEleventh".Localized());
                }
                else
                {
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotTwelfth".Localized());
                    }
                    Codespell = "";
                    Code = 0;
                }
            }
            if (__sw0_0 == 7)
            {
                OutputText("Scenes.Manor.Keyword.OpenPnekiAccounts".Localized());
                if (Code == 1)
                {
                    Code += 1;
                    Codespell += " P";
                    OutputText("Scenes.Manor.Keyword.PushBookSlotThirteenth".Localized());
                }
                else
                {
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotFourteenth".Localized());
                    }
                    Codespell = "";
                    Code = 0;
                }
            }
            if (__sw0_0 == 8)
            {
                OutputText("Scenes.Manor.Keyword.OpenQabalumMaximus".Localized());
                if (Codespell.Length > 0)
                {
                    OutputText("Scenes.Manor.Keyword.PushBookSlotFifteenth".Localized());
                }
                Code = 0;
                Codespell = "";
            }
            if (__sw0_0 == 9)
            {
                OutputText("Scenes.Manor.Keyword.OpenRevelationsOfGlaaki".Localized());
                if (Code == 3)
                {
                    Code += 1;
                    Codespell += " R";
                    OutputText("Scenes.Manor.Keyword.PushBookSlotSixteenth".Localized());
                }
                else
                {
                    if (Codespell.Length > 0)
                    {
                        OutputText("Scenes.Manor.Keyword.PushBookSlotSeventeenth".Localized());
                    }
                    Codespell = "";
                    Code = 0;
                }
            }
            NorthBookshelf();
        }

        // Haxe StairsStart()   outputText 2 
        public void StairsStart()
        {
            ClearOutput();

            OutputText("Scenes.Manor.StairsStart.CorridorTo".Localized());
            OutputText("Scenes.Manor.StairsStart.DespiteBlockage".Localized());
        }

        // Haxe Stairs0()   outputText 1 
        public void Stairs0()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Stairs0.AtBaseOf".Localized());
            SetStairButtons(() => SwitchFloor(2, 7));
        }

        // Haxe Stairs1()   outputText 1 
        public void Stairs1()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Stairs1.SecondFloor".Localized());
            SetStairButtons(null, () => SwitchFloor(1, 2));
        }

        // Haxe Stairs2()   outputText 2 
        public void Stairs2()
        {
            ClearOutput();

            if (((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 2) != 0) && !((CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) & 4) != 0))
            {
                OutputText("Scenes.Manor.Stairs2.FeelOddApproaching".Localized());
                DoNext(CourtierEncounter);
                return ;
            }
            OutputText("Scenes.Manor.Stairs2.SecondFloor".Localized());
        }

        // Haxe CourtierEncounter()   outputText 8 
        public void CourtierEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Manor.CourtierEncounter.ImagesOfIngnam".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KihaAffection) > 70)
            {
                OutputText("Scenes.Manor.CourtierEncounter.SeeKiha".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.HelAffection) >= 70)
            {
                OutputText("Scenes.Manor.CourtierEncounter.SeeHelia".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.AmilyAffection) >= 40)
            {
                OutputText("Scenes.Manor.CourtierEncounter.SeeAmily".Localized());
            }
            if ((int)Player.StatusEffectValue(VanillaStatusEffects.Marble, 0) >= 80)
            {
                OutputText("Scenes.Manor.CourtierEncounter.SeeUnbeatableArmy".Localized());
            }
            OutputText("Scenes.Manor.CourtierEncounter.TheyMockYou".Localized());
            OutputText("Scenes.Manor.CourtierEncounter.ImagesNearlyCripple".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.ShouldraFollowerState) == 1)
            {
                OutputText("Scenes.Manor.CourtierEncounter.ShouldraAppears".Localized());
            }
            AddButton(0, "Next", () => TestResolve(true));
        }

        // Haxe CourtierFight()   outputText 1 
        public void CourtierFight()
        {
            ClearOutput();

            OutputText("Scenes.Manor.CourtierFight.StandUpTemporarilyFree".Localized());
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "bone_courtier")), RunFunc);
        }

        // Haxe Study()   outputText 2 
        public void Study()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Study.SomeKindOf".Localized());
            OutputText("Scenes.Manor.Study.AlthoughManyBooks".Localized());
            AddButton(0, "Desk", () => DeskStudy());
            AddButton(1, "Books", ReadStudyBooks);
        }

        // Haxe DeskStudy()   outputText 1 
        public void DeskStudy(int currentlyRead = -1)
        {

            if (!((CoC.Instance.Flags.Get(VanillaFlags.CodexEphraimJournal) & 1) != 0))
            {
                CoC.Instance.Flags.Set(VanillaFlags.CodexEphraimJournal, CoC.Instance.Flags.Get(VanillaFlags.CodexEphraimJournal) | (1 << 0));
                OutputText("Scenes.Manor.DeskStudy.NewCodexEntryManor".Localized());
            }
            ClearButtons();
            AddButton(0, "Journal I", () => Journal(0, DeskStudy));
            AddButton(1, "Journal II", () => Journal(1, DeskStudy));
            AddButton(2, "Journal III", () => Journal(2, DeskStudy));
            AddButton(3, "Journal IV", () => Journal(3, DeskStudy));
            AddButton(4, "Journal V", () => Journal(4, DeskStudy));
            AddButton(14, "Back", RunFunc);
        }

        // Haxe ReadStudyBooks()   outputText 1 
        public void ReadStudyBooks()
        {
            ClearOutput();

            OutputText("Scenes.Manor.ReadStudyBooks.ReadSomePages".Localized());
            DoNext(RunFunc);
        }

        // 原版 MarbleScene.hx 的 journal(section:Int = 0, callback:Int -> Void)（Manor.hx:1206）：
        // C# 可选参数必须排最后，所以调用点显式传 section。
        public void Journal(int section, Action<int> callback)
        {
            ClearOutput();

            var __sw0_0 = section;
            if (__sw0_0 == 0)
            {
                JournalHeader("<b>I</b>");
                OutputText("Scenes.Manor.Journal.IHaveArrived".Localized());
                OutputText("Scenes.Manor.Journal.WritingOnGrimTopics".Localized());
                OutputText("Scenes.Manor.Journal.ManorStillHabitable".Localized());
            }
            if (__sw0_0 == 1)
            {
                JournalHeader("<b>II</b>");
                OutputText("Scenes.Manor.Journal.SeveralDaysHardWork".Localized());
            }
            if (__sw0_0 == 2)
            {
                JournalHeader("<b>III</b>");
                OutputText("Scenes.Manor.Journal.MostOfManor".Localized());
                OutputText("Scenes.Manor.Journal.HauntedByNightmares".Localized());
                OutputText("Scenes.Manor.Journal.DreamsDistressMe".Localized());
            }
            if (__sw0_0 == 3)
            {
                JournalHeader("<b>IV</b>");
                OutputText("Scenes.Manor.Journal.InsistentGnawing".Localized());
                OutputText("Scenes.Manor.Journal.ExcavationsThroughTunnels".Localized());
                OutputText("Scenes.Manor.Journal.TheyContinuedWork".Localized());
            }
            if (__sw0_0 == 4)
            {
                JournalHeader("<b>V</b>");
                OutputText("Scenes.Manor.Journal.WorkersUncoveredRoom".Localized());
                OutputText("Scenes.Manor.Journal.TakenOverByDesire".Localized());
            }
            if (__sw0_0 == 5)
            {
                JournalHeader("<b>VI</b>");
                OutputText("Scenes.Manor.Journal.DraggedTheCorpse".Localized());
            }
            if (__sw0_0 == 6)
            {
                JournalHeader("<b>VII</b>");
                OutputText("Scenes.Manor.Journal.ContinuedFailures".Localized());
                OutputText("Scenes.Manor.Journal.TheyArrivedInHiding".Localized());
                OutputText("Scenes.Manor.Journal.SharedSeveralVolumes".Localized());
                OutputText("Scenes.Manor.Journal.ProcessOverwhelmingSuccess".Localized());
            }
            if (__sw0_0 == 7)
            {
                JournalHeader("<b>VIII</b>");
                OutputText("Scenes.Manor.Journal.LuredTheInnocentGirl".Localized());
                OutputText("Scenes.Manor.Journal.UsingHerOwnSoul".Localized());
                OutputText("Scenes.Manor.Journal.UsingTheirSouls".Localized());
            }
            if (__sw0_0 == 8)
            {
                JournalHeader("<b>IX</b>");
                OutputText("Scenes.Manor.Journal.AfterSeveralYears".Localized());
                OutputText("Scenes.Manor.Journal.InMyDreams".Localized());
            }
            if (__sw0_0 == 9)
            {
                JournalHeader("<b>X</b>");
                OutputText("Scenes.Manor.Journal.SpheresAreAligned".Localized());
                OutputText("Scenes.Manor.Journal.SoonThisWorldWill".Localized());
            }
            callback(section);
        }

        // Haxe Stairs3()   outputText 1 
        public void Stairs3()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Stairs3.SecondFloor".Localized());
        }

        // Haxe Bedroom()   outputText 2 
        public void Bedroom()
        {
            ClearOutput();

            OutputText("Scenes.Manor.Bedroom.InsideABedroom".Localized());
            OutputText("Scenes.Manor.Bedroom.StrangeCirclesRunes".Localized());
            AddButton(0, "Check Bed", CheckBed);
        }

        // Haxe CheckBed()   outputText 5 
        public void CheckBed()
        {
            ClearOutput();

            OutputText("Scenes.Manor.CheckBed.ApproachBedAndCheck".Localized(Player.Intelligence > 70 ? " Judging by the spread and the hole, you'd wager someone fired a flintlock upwards... suicide?" : " You can't puzzle what happened here."));
            if (!Player.HasKeyItem("Family Talisman"))
            {
                OutputText("Scenes.Manor.CheckBed.SearchAroundTheBed".Localized());
                OutputText("Scenes.Manor.CheckBed.KeyItemFamilyTalisman".Localized());
                Player.CreateKeyItem("Family Talisman", 0, 0, 0, 0);
                OutputText("Scenes.Manor.CheckBed.HearSomethingMoving".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.ManorProgress, CoC.Instance.Flags.Get(VanillaFlags.ManorProgress) | (1 << 1));
            }
            else
            {
                OutputText("Scenes.Manor.CheckBed.NothingElseToFind".Localized());
            }
            DoNext(RunFunc);
        }

        // Haxe ExitDungeon()   outputText 1 
        public void ExitDungeon()
        {


            DungeonSession.End();

            OutputText("Scenes.Manor.ExitDungeon.LeaveAccursedManor".Localized());
            Camp.ReturnToCamp(1);
        }
    }
}

