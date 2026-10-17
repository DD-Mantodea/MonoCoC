using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCoC.Contents.Dungeons
{
	[Register<ManorScene>("coc:manor")]
	public class ManorScene : Scene
	{

		public const int OpenRoom = 0;

		public const int EmptyRoom = 1;

		public const int LockedRoom = 2;

		public const int VoidRoom = -1;

		public const int StairsUp = 3;

		public const int StairsDown = 4;

		public const int StairsUpDown = 5;

		public const int Trader = 7;

		public const int DirNorth = 1 << 0;

		public const int DirSouth = 1 << 1;

		public const int DirEast = 1 << 2;

		public const int DirWest = 1 << 3;

		public int _playerLoc;

		public int Floor = 1;

		public int PrevLoc;

		public readonly Dictionary<int, Action> DungeonRooms = [];

		public readonly List<(int[] Map, int[] Connectivity)> InfinityMaps = [];

		public int InfinityLevel;

		public Action InfinityFunc;

		public int Code;

		public string Codespell = "";

		public SaveContent Save => SaveContent(new Identifier("coc", "manor"));

		public int ManorProgress => Flags.Get(GameFlagIds.ManorProgress);

		public bool HasProgress(int bit) => (ManorProgress & bit) != 0;

		public void SetProgress(int bit) => Flags.Set(GameFlagIds.ManorProgress, ManorProgress | bit);

		public ManorScene()
		{

			InfinityFunc = Infinity1;
		}

		public int PlayerLoc
		{
			get => _playerLoc;

			set
			{

				PrevLoc = _playerLoc;

				_playerLoc = value;
			}
		}

		public override void Entry()
		{

			EnterDungeon();
		}

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

		public int MapModulus => (int)Math.Sqrt(DungeonMap.Length);

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

		public void InitRooms()
		{
			DungeonRooms.Clear();

			InitRoomsF1();
		}

		public void InitRoomsF2()
		{
			DungeonRooms.Clear();

			DungeonRooms[7] = Stairs1;

			DungeonRooms[4] = Stairs2;

			DungeonRooms[1] = Stairs3;

			DungeonRooms[2] = Bedroom;

			DungeonRooms[3] = Study;
		}

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

		public bool IsInChamber() => PlayerLoc == 30 && Floor == 0;

		public void RunFunc()
		{
			if (Floor == 3)
			{
				InfinityFunc();

				return;
			}

			ClearOutput();

			SetDungeonButtons();

			if (DungeonRooms.TryGetValue(PlayerLoc, out var room))
				room();
		}

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

			return possibleIndexes.Count > 0 ? possibleIndexes[Random.Shared.Next(possibleIndexes.Count)] : 0;
		}

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

			View.Time.PassMinutes(5);

			RunFunc();
		}

		public void AddMoveButton(int slot, string label, int direction)
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

		public void SetDungeonButtons()
		{
			ClearButtons();

			AddMoveButton(6, "North", 0);

			AddMoveButton(11, "South", 1);

			AddMoveButton(10, "West", 2);

			AddMoveButton(12, "East", 3);

			AddButton(13, "Common.Button.Inventory".Localized(), () => InventoryScene.InventoryMenu(),

				"The inventory allows you to view or use your items.");

			AddButton(14, "Common.Button.Map".Localized(), ShowMap, "View the map of this dungeon.");
		}

		public void SetInfinityButtons(Action north, Action south, Action west, Action east)
		{
			ClearButtons();

			AddButton(6, "Common.Button.North".Localized(), north);

			AddButton(11, "Common.Button.South".Localized(), south);

			AddButton(10, "Common.Button.West".Localized(), west);

			AddButton(12, "Common.Button.East".Localized(), east);

			AddButton(13, "Common.Button.Inventory".Localized(), () => InventoryScene.InventoryMenu(),

				"The inventory allows you to view or use your items.");

			AddButton(14, "Common.Button.Map".Localized(), ShowMap, "View the map of this dungeon.");
		}

		public static void JournalHeader(string text) =>

			OutputText("[pg]<font size=\"24\" face=\"Georgia\"><b><u>" + text + "</u></b></font>\n");

		public void SetStairButtons(Action upstairs = null, Action downstairs = null)
		{
			if (downstairs != null)
				AddButton(5, "Common.Button.Downstairs".Localized(), downstairs);

			if (upstairs != null)
				AddButton(downstairs != null ? 0 : 5, "Common.Button.Upstairs".Localized(), upstairs);
		}

		public void RemakeMaps()
		{
		}

		public void ShowMap()
		{
			ClearOutput();

			ClearButtons();

			var map = DungeonMap;

			var modulus = (int)Math.Sqrt(map.Length);

			var text = new StringBuilder();

			text.Append("Common.Text.BUOldManorUB".Localized());

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

			text.Append("Common.Text.NBLegendBNYou".Localized());

			OutputText(text.ToString());

			AddButton(14, "Common.Button.Back".Localized(), RunFunc);
		}

		public void GenerateInfinityMaps()
		{
			for (var i = 0; i < 5; i++)
				InfinityMaps.Add(GenerateRandomMaze(10, 10, 3));
		}

		public static (int[] Map, int[] Connectivity) GenerateRandomMaze(int height, int width, int density = 4, bool stretch = false)
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

				var (X, Y, FromX, FromY) = frontiers[index];

				frontiers.RemoveAt(index);

				var x = X;

				var y = Y;

				if (x < 0 || x >= width || y < 0 || y >= height)
					continue;

				if (room[y, x] != EmptyRoom)
					continue;

				room[y, x] = OpenRoom;

				if (x > FromX)
				{
					conn[y, x] |= DirWest;

					conn[FromY, FromX] |= DirEast;
				}
				else if (x < FromX)
				{
					conn[y, x] |= DirEast;

					conn[FromY, FromX] |= DirWest;
				}

				if (y > FromY)
				{
					conn[y, x] |= DirNorth;

					conn[FromY, FromX] |= DirSouth;
				}
				else if (y < FromY)
				{
					conn[y, x] |= DirSouth;

					conn[FromY, FromX] |= DirNorth;
				}

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

		public void Reset()
		{

			Save.SetInt("wineDrunk", 0);

			Save.SetInt("lethiciteTaken", 0);

			Save.SetBool("booksTaken", false);
		}

		public void TestResolve(bool dungeon = false)
		{

			var virtue = false;

			if (Random.Shared.Next(10) + (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1 ? 1 : 0) + (Player.HasPerk(PerkIds.Revelation) ? 1 : 0) >= 6)
				virtue = true;

			float buffType = Random.Shared.Next(3);

			switch (buffType)
			{
				case 0:
					{
						if (virtue)
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 1, .9f, 5, 0);

							OutputText("Scenes.Manor.TestResolve.StandOverAllChallenges".Localized());
						}
						else
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 2, 1.1f, 10, 0);

							OutputText("Scenes.Manor.TestResolve.RememberThePain".Localized());
						}
					}

					break;

				case 1:
					{
						if (virtue)
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 3, 10, 20, 0);

							OutputText("Scenes.Manor.TestResolve.MomentOfUtterTerror".Localized());
						}
						else
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 4, 10, 20, 0);

							OutputText("Scenes.Manor.TestResolve.AreYouDead".Localized());
						}
					}

					break;

				case 2:
					{
						if (virtue)
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 5, 1.15f, 1.15f, 0);

							OutputText("Scenes.Manor.TestResolve.VastTerrorsMotivate".Localized());
						}
						else
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 6, 0.85f, 0.85f, 0);

							OutputText("Scenes.Manor.TestResolve.WeakApatheticVictory".Localized());
						}
					}

					break;

				case 3:
					{
						if (virtue)
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 7, 0.85f, 0, 0);

							OutputText("Scenes.Manor.TestResolve.AnotherTestOfConstitution".Localized());
						}
						else
						{
							Player.CreateStatusEffect(StatusEffectIds.Resolve, 8, 0, 0, 0);

							OutputText("Scenes.Manor.TestResolve.FacedWithTerrors".Localized());
						}
					}

					break;
			}

			if (dungeon)

				DoNext(CourtierFight);
		}

		public void EnterDungeon()
		{
			ClearOutput();

			OutputText("Scenes.Manor.EnterDungeon.WalkUpDecrepitRoad".Localized());

			OutputText("Scenes.Manor.EnterDungeon.AreaStandsSilent".Localized());

			OutputText("Scenes.Manor.EnterDungeon.EnterManorOrLeave".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.FoundManor) < 1)
			{
				OutputText("Scenes.Manor.EnterDungeon.ManorNowAccessible".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.FoundManor, 1);
			}

			ClearButtons();

			AddButton(0, "Common.Button.Enter".Localized(), EnterManor);

			AddButton(1, "Common.Button.Leave".Localized(), ExitDungeon);
		}

		public void EnterManor()
		{

			ClearButtons();

			DungeonSession.InDungeon = true;

			InitRooms();

			PlayerLoc = 7;

			SwitchFloor(1, 7);

			RunFunc();
		}

		public void EnterVoid()
		{

			DungeonSession.InDungeon = true;

			InitRooms();

			PlayerLoc = 7;

			InfinityLevel = 3;

			InfinityFunc = Infinity4;

			SwitchFloor(3, 0);

			RunFunc();
		}

		public void RoomCourtyard()
		{
			ClearOutput();

			if (CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) < 1)
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

				AddButton(11, "Common.Button.Leave".Localized(), ExitDungeon);
			}
		}

		public void LoseToGeneral()
		{
			ClearOutput();

			CombatScene.CleanupAfterCombat();

			OutputText("Scenes.Manor.LoseToGeneral.CollapseTooWeak".Localized());

			OutputText("Scenes.Manor.LoseToGeneral.ShamblesTowardsYou".Localized());

			OutputText("Scenes.Manor.LoseToGeneral.DeadMoreBlood".Localized());

			GameOver();
		}

		public void DefeatGeneral()
		{
			ClearOutput();

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 0));

			CombatScene.CleanupAfterCombat();

			OutputText("Scenes.Manor.DefeatGeneral.FinalStrikeMammoth".Localized());

			OutputText("Scenes.Manor.DefeatGeneral.PressForwardInto".Localized());

			DoNext(RunFunc);
		}

		public void DefeatCourtier()
		{
			ClearOutput();

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 2));

			OutputText("Scenes.Manor.DefeatCourtier.WeakenedSkeletonAttempts".Localized());

			if ((Player.StatusEffectValue(StatusEffectIds.Resolve, 0) % 2) == 0)
				OutputText("Scenes.Manor.DefeatCourtier.HazeOfTerror".Localized());
			else
				OutputText("Scenes.Manor.DefeatCourtier.EnergizingVirtuousThoughts".Localized());

			OutputText("Scenes.Manor.DefeatCourtier.RemindedOfThe".Localized());

			OutputText("Scenes.Manor.DefeatCourtier.BriefMomentWonder".Localized());

			OutputText("Scenes.Manor.DefeatCourtier.NewMagicalAbilityGained".Localized());

			CombatScene.CleanupAfterCombat();

			DoNext(RunFunc);
		}

		public void LoseToCourtier()
		{
			ClearOutput();

			CombatScene.CleanupAfterCombat();

			OutputText("Scenes.Manor.LoseToCourtier.FallOverwhelmedBy".Localized());

			if (Player.Health < 1)
				OutputText("Scenes.Manor.LoseToCourtier.YourWoundsWord".Localized());
			else
				OutputText("Scenes.Manor.LoseToCourtier.LustRagingThrough".Localized());

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

		public void LoseToCourtierEphraimd()
		{
			ClearOutput();

			OutputText("Scenes.Manor.LoseToCourtierEphraimd.WakeUpOnBed".Localized());

			OutputText("Scenes.Manor.LoseToCourtierEphraimd.LookAtYourself".Localized());

			OutputText("Scenes.Manor.LoseToCourtierEphraimd.EphraimYouAreEphraim".Localized());

			GameOver();
		}

		public void LoseToCourtierSkeletonized()
		{
			ClearOutput();

			OutputText("Scenes.Manor.LoseToCourtierSkeletonized.RegainSensesAgain".Localized());

			OutputText("Scenes.Manor.LoseToCourtierSkeletonized.BriefStateOf".Localized());

			OutputText("Scenes.Manor.LoseToCourtierSkeletonized.AnotherLifeWasted".Localized());

			GameOver();
		}

		public void LoseToJester()
		{
			ClearOutput();

			CombatScene.CleanupAfterCombat();

			OutputText("Scenes.Manor.LoseToJester.FallOverwhelmedBy".Localized());

			if (Player.Health < 1)
				OutputText("Scenes.Manor.LoseToJester.YourWoundsWord".Localized());
			else
				OutputText("Scenes.Manor.LoseToJester.LustRagingThrough".Localized());

			OutputText("Scenes.Manor.LoseToJester.JesterJumpsOnTable".Localized());

			OutputText("Scenes.Manor.LoseToJester.UncannyStrengthSlices".Localized());

			DoNext(LoseToJesterDullahand);
		}

		public void DefeatJester()
		{
			ClearOutput();

			OutputText("Scenes.Manor.DefeatJester.NoticingImminentDefeat".Localized());

			OutputText("Scenes.Manor.DefeatJester.EvidenceSkeleton".Localized());

			CombatScene.CleanupAfterCombat();

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 3));

			DoNext(RunFunc);
		}

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

		public void MainHall()
		{
			ClearOutput();

			OutputText("Scenes.Manor.MainHall.StandAtSpaciousMain".Localized());

			OutputText("Scenes.Manor.MainHall.TatteredRegalRug".Localized());
		}

		public void DiningRoom()
		{
			ClearOutput();

			OutputText("Scenes.Manor.DiningRoom.SpaciousDining".Localized());

			if (((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 8) != 0) && !((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 16) != 0))
			{
				OutputText("Scenes.Manor.DiningRoom.SeeDarkCurvedDagger".Localized());

				AddButton(1, "Common.Button.TakeDagger".Localized(), () => InventoryScene.TakeItem(ItemIds.CursedDagger, TookDagger, RunFunc));
			}

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 4) != 0) || ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 8) != 0) || ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 64) != 0))
			{
				switch (Save.GetInt("wineDrunk", 0))
				{
					case 0:
						{
							OutputText("Scenes.Manor.DiningRoom.WineRackAtBack".Localized());
						}

						break;

					case 1:
						{
							OutputText("Scenes.Manor.DiningRoom.BottleOpenedBeforeSits".Localized());
						}

						break;

					case 2:
						{
							OutputText("Scenes.Manor.DiningRoom.BottleOpenedBefore".Localized());
						}

						break;

					case 3:
						{
							OutputText("Scenes.Manor.DiningRoom.BottleOnDiningTable".Localized().Replace("[if (silly) { It's taunting you. It thinks it can win this fight. Go prove it wrong! Kill it.}]", silly ? " It's taunting you. It thinks it can win this fight. Go prove it wrong! Kill it." : ""));
						}

						break;

					case 4:
						{
							OutputText("Scenes.Manor.DiningRoom.LastWineBottleNow".Localized());
						}

						break;
				}

				if (Save.GetInt("wineDrunk", 0) >= 4)
					AddButtonDisabled(0, "Drink Wine", "The bottle is empty.");
				else
					AddButton(0, "Common.Button.DrinkWine".Localized(), DrinkWine, "Drink some of the wine.");
			}
			else
			{
				OutputText("Scenes.Manor.DiningRoom.ExploreDiningRoom".Localized());

				OutputText("Scenes.Manor.DiningRoom.AgainHearNoise".Localized());

				OutputText("Scenes.Manor.DiningRoom.MoveAroundTrying".Localized());

				if (Player.Speed > 70)
					OutputText("Scenes.Manor.DiningRoom.ManageToTurnAround".Localized());
				else
				{
					OutputText("Scenes.Manor.DiningRoom.FailToTurn".Localized(Player.HasPerk(PerkIds.BleedImmune) ? "bleeding and " : ""));

					var damage = 45 + Random.Shared.Next(30);

					Player.HealthChange(-damage);

					Player.Bleed(EnemyRegistry.Create(new Identifier("coc", "bone_jester")));
				}

				CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "bone_jester")), RunFunc);
			}
		}

		public void TookDagger()
		{

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 4));

			RunFunc();
		}

		public void DrinkWine()
		{
			ClearOutput();

			switch (1 + Save.GetInt("wineDrunk", 0))
			{
				case 1:
					{
						OutputText("Scenes.Manor.DrinkWine.UncorkAndSmell".Localized());

						OutputText("Scenes.Manor.DrinkWine.TastesGreat".Localized());
					}

					break;

				case 2:
					{
						OutputText("Scenes.Manor.DrinkWine.DespiteBetterJudgment".Localized());

						OutputText("Scenes.Manor.DrinkWine.DelightfulAroma".Localized(silly ? "Does this mean you're an alcoholic?" : ""));
					}

					break;

				case 3:
					{
						OutputText("Scenes.Manor.DrinkWine.PouringMoreWine".Localized());

						OutputText("Scenes.Manor.DrinkWine.FleetingMomentOfLucidity".Localized());

						Player.Lust = 0;
					}

					break;

				case 4:
					{
						OutputText("Scenes.Manor.DrinkWine.StareAtBottleIntensely".Localized());

						OutputText("Scenes.Manor.DrinkWine.TearsWellUp".Localized());

						OutputText("Scenes.Manor.DrinkWine.DamnThisHorridPlace".Localized());

						Player.Lust = 0;
					}

					break;

				default:
					{
						OutputText("Scenes.Manor.DrinkWine.BottleIsEmpty".Localized().Replace("[if (silly) { As are you.}]", silly ? " As are you." : ""));
					}

					break;
			}

			Save.SetInt("wineDrunk", Save.GetInt("wineDrunk", 0) + 1);

			if (Save.GetInt("wineDrunk", 0) < 5)
			{
				Player.HealthChange(Player.MaxHealth / 10);

				Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Corruption, 2f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Speed, -1f));
			}

			DoNext(RunFunc);
		}

		public void LibraryRoom()
		{
			ClearOutput();

			OutputText("Scenes.Manor.LibraryRoom.WellStockedLibrary".Localized());

			OutputText("Scenes.Manor.LibraryRoom.NorthernBookshelfIs".Localized());

			AddButton(0, "Common.Button.NorthShelf".Localized(), NorthBookshelf);

			if (!Save.GetBool("booksTaken"))

				AddButton(1, "Common.Button.TakeBooks".Localized(), TakeBooks, "You might be able to find some use for these.");
		}

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

		public void LibrarySecret()
		{
			ClearOutput();

			OutputText("Scenes.Manor.LibrarySecret.InsideSecretRoom".Localized());

			if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 2048) != 0)
				OutputText("Scenes.Manor.LibrarySecret.SeePassageToThe".Localized());
			else
				OutputText("Scenes.Manor.LibrarySecret.NorthernWallThers".Localized());

			OutputText("Scenes.Manor.LibrarySecret.AlsoNoticeScatteredPages".Localized());

			if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 2048) != 0)
				SetStairButtons(null, () => SwitchFloor(0, 35));

			AddButton(0, "Common.Button.Pages".Localized(), () => FloorPages());

			if (Player.HasKeyItem("Family Talisman") && ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 2048) == 0))

				AddButton(1, "Common.Button.InsertTalisman".Localized(), OpenStairs, "Insert the talisman you found into the slot.");
		}

		public void OpenStairs()
		{
			ClearOutput();

			OutputText("Scenes.Manor.OpenStairs.PressTheTalisman".Localized());

			OutputText("Scenes.Manor.OpenStairs.PeerIntoGap".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 11));

			RemakeMaps();

			DoNext(RunFunc);
		}

		public void Tunnels1()
		{
			ClearOutput();

			if (PrevLoc == 0)
			{
				if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0)
					OutputText("Scenes.Manor.Tunnels1.MoveThroughGateway".Localized());
				else
				{
					OutputText("Scenes.Manor.Tunnels1.GoDownCarefully".Localized());

					OutputText("Scenes.Manor.Tunnels1.StandAtBeginning".Localized());

					OutputText("Scenes.Manor.Tunnels1.AirFeelsCool".Localized(Player.HasPerk(PerkIds.HistoryAlchemist) ? " You inhale more deeply and try to identify the source of the smell. Embalming fluids, you think to yourself." : " Whatever the source is, its purpose can only be grim."));
				}
			}
			else
				OutputText("Scenes.Manor.Tunnels1.ReachEndOfThe".Localized());

			SetStairButtons(() => SwitchFloor(1, 0));
		}

		public void Tunnels2()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Tunnels2.MoveThroughTunnelSlowly".Localized());

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 64) != 0))
			{
				OutputText("Scenes.Manor.Tunnels2.SuddenlyHearSound".Localized());

				OutputText("Scenes.Manor.Tunnels2.OfRattlingBones".Localized());

				OutputText("Scenes.Manor.Tunnels2.OfEldritchChanting".Localized());

				OutputText("Scenes.Manor.Tunnels2.MoveForwardFewFeet".Localized());

				OutputText("Scenes.Manor.Tunnels2.PrepareToFight".Localized());

				if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 8) != 0)
					OutputText("Scenes.Manor.Tunnels2.IntuitionLeadsYou".Localized());
				else
					OutputText("Scenes.Manor.Tunnels2.IntuitionLeadsYouAgain".Localized());

				OutputText("Scenes.Manor.Tunnels2.NoWayAroundIt".Localized());

				var desc = "Before you stands a veritable court of reanimated skeletons. The Bone Guardian stands in front, protecting the rest with its massive armored body. The Bone Jester moves in and out of sight quickly, attempting to lose your focus. Standing behind all of them is the Bone Sorcerer, preparing arcane spells to weaken you so that his companions can finish you off.";

				var general = EnemyRegistry.Create(new Identifier("coc", "bone_general"));

				var jester = EnemyRegistry.Create(new Identifier("coc", "bone_jester"));

				var courtier = EnemyRegistry.Create(new Identifier("coc", "bone_courtier"));

				general.DescriptionOverride = desc;

				general.OnDefeated = _ => DefeatCourt();

				general.OnWon = _ => LoseToCourt();

				CombatScene.StartMultiple([general, jester, courtier], RunFunc);
			}
			else
				if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 8) != 0) && !((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 16) != 0))
				{
					OutputText("Scenes.Manor.Tunnels2.SeeDarkCurvedDagger".Localized());

					AddButton(1, "Common.Button.TakeDagger".Localized(), () => InventoryScene.TakeItem(ItemIds.CursedDagger, TookDagger, RunFunc));
				}
		}

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

		public void DefeatCourt()
		{
			ClearOutput();

			OutputText("Scenes.Manor.DefeatCourt.ColossalArmoredStumbles".Localized());

			OutputText("Scenes.Manor.DefeatCourt.AfterDefeatingArmored".Localized());

			OutputText("Scenes.Manor.DefeatCourt.StandAndLook".Localized());

			OutputText("Scenes.Manor.DefeatCourt.PantExhaustedLook".Localized());

			CombatScene.CleanupAfterCombat();

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 6));

			DoNext(RunFunc);
		}

		public void LethiciteTunnel1()
		{
			ClearOutput();

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 512) != 0))
				OutputText("Scenes.Manor.LethiciteTunnel1.AtEndOfAdjacent".Localized());
			else
				OutputText("Scenes.Manor.LethiciteTunnel1.CrystalDestroyedTunnel".Localized());
		}

		public void LethiciteTunnel2()
		{
			ClearOutput();

			OutputText("Scenes.Manor.LethiciteTunnel2.EnterChamberAtThe".Localized());

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 512) != 0) && !((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0))
			{
				OutputText("Scenes.Manor.LethiciteTunnel2.CenterStands".Localized());

				AddButton(0, "Common.Button.Destroy".Localized(), () => CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "lethicite_crystal")), RunFunc), "Attempt to destroy the crystal. This lethicite is probably powering some kind of spell, but destroying raw lethicite is nigh-impossible by conventional means.");
			}
			else
			{
				OutputText("Scenes.Manor.LethiciteTunnel2.CenterStandsAgain".Localized());

				if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0 && !((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 512) != 0))
					OutputText("Scenes.Manor.LethiciteTunnel2.PerhapsBoreSomeType".Localized());

			}
		}

		public void LoseToCrystal()
		{
			ClearOutput();

			CombatScene.CleanupAfterCombat();

			OutputText("Scenes.Manor.LoseToCrystal.FallDownTooAroused".Localized());

			OutputText("Scenes.Manor.LoseToCrystal.ImmediatelyAttemptMasturbate".Localized());

			if (Player.HasCock())
				OutputText("Scenes.Manor.LoseToCrystal.EjaculatingThickStrands".Localized());

			if (Player.HasVagina())
				OutputText("Scenes.Manor.LoseToCrystal.SquirtingUncannyAmounts".Localized());

			OutputText("Scenes.Manor.LoseToCrystal.BeforeYouCanEnjoy".Localized());

			OutputText("Scenes.Manor.LoseToCrystal.DespiteMindCompletely".Localized());

			OutputText("Scenes.Manor.LoseToCrystal.StandUpTrembling".Localized());

			DoNext(LoseToCrystal2);
		}

		public void LoseToCrystal2()
		{
			ClearOutput();

			Player.ApplyStats(new DynStat(GameStats.Lust, 999f), new DynStat(GameStats.Corruption, 999f));

			OutputText("Scenes.Manor.LoseToCrystal2.WhileUnconsciousTowering".Localized());

			OutputText("Scenes.Manor.LoseToCrystal2.HissesStandsUpAttaches".Localized());

			OutputText("Scenes.Manor.LoseToCrystal2.LeavesShortlyAfterwards".Localized());

			GameOver();
		}

		public void DestroyCrystal()
		{
			ClearOutput();

			OutputText("Scenes.Manor.DestroyCrystal.CrystalBeginsToRespond".Localized());

			OutputText("Scenes.Manor.DestroyCrystal.BeginsVibratingSoftly".Localized());

			OutputText("Scenes.Manor.DestroyCrystal.WhenYouHitGround".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 9));

			CombatScene.CleanupAfterCombat();

			DoNext(RunFunc);
		}

		public void Tunnels3()
		{
			ClearOutput();

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0))
				OutputText("Scenes.Manor.Tunnels3.WantedToFreeMyself".Localized());
			else
				OutputText("Scenes.Manor.Tunnels3.DespiteNecromancerDefeat".Localized());
		}

		public void Tunnels4()
		{
			ClearOutput();

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0))
				OutputText("Scenes.Manor.Tunnels4.WorkersEvelyn".Localized());
			else
				OutputText("Scenes.Manor.Tunnels4.DespiteNecromancerDefeat".Localized());
		}

		public void Tunnels5()
		{
			ClearOutput();

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0))
				OutputText("Scenes.Manor.Tunnels5.RegardlessOfEthics".Localized());
			else
				OutputText("Scenes.Manor.Tunnels5.DespiteNecromancerDefeat".Localized());
		}

		public void TheChamber()
		{
			ClearOutput();

			var necroDesc = "Before you stands a towering monstrosity, a being of nightmares. The Necromancer, covered in a red cloak, breathes slowly, sure of his impending victory. Around him stand several piles of bones from which skeletons are assembled to attack you.";

			if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 512) != 0)
				necroDesc += "\n\nWith the lethicite crystal destroyed, his summoning ability is weakened.\n\n";

			OutputText("Scenes.Manor.TheChamber.ApproachMassiveChamber".Localized());

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0))
			{
				OutputText("Scenes.Manor.TheChamber.LaidOverTorchAltar".Localized());

				OutputText("Scenes.Manor.TheChamber.TurnsToFaceYou".Localized());

				OutputText("Scenes.Manor.TheChamber.FigureLiftsArm".Localized());

				var necromancer = EnemyRegistry.Create(new Identifier("coc", "necromancer"));

				var horde = EnemyRegistry.Create(new Identifier("coc", "skeleton_horde"));

				necromancer.DescriptionOverride = necroDesc;

				CombatScene.StartMultiple([necromancer, horde], RunFunc);
			}
			else
			{
				if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 256) != 0))
				{
					OutputText("Scenes.Manor.TheChamber.SeeDarkCrystal".Localized());

					AddButton(1, "Common.Button.TakeCrystal".Localized(), () => InventoryScene.TakeItem(UseableIds.AbyssalShard, TookCrystal, RunFunc));
				}

				OutputText("Scenes.Manor.TheChamber.NearbyTableYouSee".Localized());

				AddButton(0, "Common.Button.ReadPages".Localized(), () => TablePages());
			}
		}

		public void TablePages(int currentlyRead = -1)
		{

			if (!((CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) & 4) != 0))
			{
				CoC.Instance.Flags.Set(GameFlagIds.CodexEphraimJournal, CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) + 4);

				OutputText("Scenes.Manor.TablePages.NewCodexEntryManor".Localized());
			}

			ClearButtons();

			AddButton(0, "Common.Button.JournalIx".Localized(), () => Journal(8, TablePages));

			AddButton(1, "Common.Button.JournalX".Localized(), () => Journal(9, TablePages));

			AddButton(14, "Common.Button.Back".Localized(), RunFunc);
		}

		public void DefeatNecro()
		{
			ClearOutput();

			OutputText("Scenes.Manor.DefeatNecro.ToweringNecromancerStumbles".Localized());

			OutputText("Scenes.Manor.DefeatNecro.MoveForwardToStrike".Localized());

			OutputText("Scenes.Manor.DefeatNecro.FewMomentsOfAgony".Localized());

			OutputText("Scenes.Manor.DefeatNecro.HaveDefeatedNecromancer".Localized());

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 7));

			Player.UpgradeBeautifulSword();

			CombatScene.CleanupAfterCombat();

			DoNext(RunFunc);
		}

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

		public void TookCrystal()
		{

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 8));

			RunFunc();
		}

		public bool UseCrystal()
		{
			ClearOutput();

			if (IsInChamber() && (View.Time.Hours >= 19 || View.Time.Hours == 0))
			{

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

		public void Infinity1()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Infinity1.InADarkExpanse".Localized());

			OutputText("Scenes.Manor.Infinity1.AbyssWelcomesYou".Localized());

			InfinityLevel = 0;

			RemakeMaps();

			SetInfinityButtons(Infinity1, Infinity2, Infinity1, Infinity1);
		}

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

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 1024) != 0))
				CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 10));

			SetInfinityButtons(Infinity5, Infinity2, Infinity2, Infinity1);

			AddButton(0, "Common.Button.TheCamp".Localized(), ReturnCamp);
		}

		public void ReturnCamp()
		{

			DungeonSession.End();

			ReturnToCamp(1);
		}

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

		public void Banish(bool defeatedHorror = false)
		{
			ClearOutput();

			if (Player.StatusEffectValue(StatusEffectIds.TFSupercharging, 1) > 0
				&& !Assets.ManagerInstance<Assets.ContentManagers.AchievementManager>.Instance.IsUnlocked(AchievementIds.NightSun))
			{

				AwardAchievement(AchievementIds.NightSun);

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

			if (!Player.HasPerk(PerkIds.Revelation))
				Player.CreatePerk(PerkIds.Revelation, 0, 0, 0, 0);

			if (defeatedHorror)

				AwardAchievement(AchievementIds.LordBritishPostulate, true, true, true);

			CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & ~1024);

			Player.ClearCombatStatuses();

			CombatScene.CleanupAfterCombat();

			SwitchFloor(1, 7, false);

			DoNext(RunFunc);
		}

		public void LoseToNamelessHorror()
		{
			ClearOutput();

			OutputText("Scenes.Manor.LoseToNamelessHorror.FallDefeatedByHorror".Localized());

			OutputText("Scenes.Manor.LoseToNamelessHorror.StareUponItsFace".Localized());

			OutputText("Scenes.Manor.LoseToNamelessHorror.NoHope".Localized());

			CombatScene.CleanupAfterCombat();

			GameOver();
		}

		public void FloorPages(int currentlyRead = -1)
		{

			if (!((CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) & 2) != 0))
			{
				CoC.Instance.Flags.Set(GameFlagIds.CodexEphraimJournal, CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) + 2);

				OutputText("Scenes.Manor.FloorPages.NewCodexEntryManor".Localized());
			}

			ClearButtons();

			AddButton(0, "Common.Button.JournalVi".Localized(), () => Journal(5, FloorPages));

			AddButton(1, "Common.Button.JournalVii".Localized(), () => Journal(6, FloorPages));

			AddButton(2, "Common.Button.JournalViii".Localized(), () => Journal(7, FloorPages));

			AddButton(14, "Common.Button.Back".Localized(), RunFunc);
		}

		public void NorthBookshelf()
		{
			ClearOutput();

			if (Code == 7 && !((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 32) != 0))
			{
				OutputText("Scenes.Manor.NorthBookshelf.PlacingLastBookBack".Localized());

				CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 5));

				RemakeMaps();
			}

			if (!((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 32) != 0) && Codespell != "")
				OutputText("Scenes.Manor.NorthBookshelf.CodespellWord".Localized(
					Codespell
				));

			ClearButtons();

			OutputText("Scenes.Manor.NorthBookshelf.ApproachOddBookshelf".Localized());

			AddButton(0, "Common.Button.AuroraConsu".Localized(), () => Keyword(0));

			AddButton(1, "Common.Button.ErshawShards".Localized(), () => Keyword(1));

			AddButton(2, "Common.Button.Galdrabohk".Localized(), () => Keyword(2));

			AddButton(3, "Common.Button.HeptarchiaM".Localized(), () => Keyword(3));

			AddButton(4, "Common.Button.IncisisAureus".Localized(), () => Keyword(4));

			AddButton(5, "Common.Button.LiberIvonis".Localized(), () => Keyword(5));

			AddButton(6, "Common.Button.MusaeumHerm".Localized(), () => Keyword(6));

			AddButton(7, "Common.Button.PnekiAcc".Localized(), () => Keyword(7));

			AddButton(8, "Common.Button.QabalumMax".Localized(), () => Keyword(8));

			AddButton(9, "Common.Button.RevelationsOfG".Localized(), () => Keyword(9));

			AddButton(14, "Common.Button.Back".Localized(), RunFunc);
		}

		public void Keyword(int letter = 0)
		{
			ClearOutput();

			switch (letter)
			{
				case 0:
					{
						OutputText("Scenes.Manor.Keyword.OpenAuroraConsurgens".Localized(Player.HasPerk(PerkIds.HistoryAlchemist) ? " The amount of glaring mistakes makes you cringe." : " You fail to understand most of it."));

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
								OutputText("Scenes.Manor.Keyword.PushBookSlotAgain".Localized());

							Codespell = "";
						}
					}

					break;

				case 1:
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
								OutputText("Scenes.Manor.Keyword.PushBookSlotFourth".Localized());

							Codespell = "";
						}
					}

					break;

				case 2:
					{
						OutputText("Scenes.Manor.Keyword.OpenGalbrabohk".Localized());

						if (!Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire) && Player.Intelligence > 60)
						{
							OutputText("Scenes.Manor.Keyword.OneChapterUsefulReadable".Localized());

							Player.CreateStatusEffect(StatusEffectIds.KnowsWhitefire, 0, 0, 0, 0);
						}

						if (Codespell.Length > 0)
							OutputText("Scenes.Manor.Keyword.PushBookSlotFifth".Localized());

						Codespell = "";

						Code = 0;
					}

					break;

				case 3:
					{
						OutputText("Scenes.Manor.Keyword.OpenHeptarchiaMystica".Localized());

						if (Player.Intelligence >= 100 && !Player.HasPerk(PerkIds.MysticLearnings))
						{
							OutputText("Scenes.Manor.Keyword.DespiteIncredibleComplexity".Localized());

							OutputText("Scenes.Manor.Keyword.PerkAcquiredMysticLearnings".Localized());

							Player.CreatePerk(PerkIds.MysticLearnings, 0, 0, 0, 0);
						}
						else
							if (Player.Intelligence < 100)
								OutputText("Scenes.Manor.Keyword.AlthoughCouldGatherInfo".Localized());
							else
								OutputText("Scenes.Manor.Keyword.AlreadyAcquiredAll".Localized());

						if (Code == 2)
						{
							Code += 1;

							Codespell += " H";

							OutputText("Scenes.Manor.Keyword.PushBookSlotSixth".Localized());
						}
						else
						{
							if (Codespell.Length > 0)
								OutputText("Scenes.Manor.Keyword.PushBookSlotSeventh".Localized());

							Codespell = "";

							Code = 0;
						}
					}

					break;

				case 4:
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
								OutputText("Scenes.Manor.Keyword.PushBookSlotNinth".Localized());

							Codespell = "";

							Code = 0;
						}
					}

					break;

				case 5:
					{
						OutputText("Scenes.Manor.Keyword.OpenLiberIvonis".Localized());

						if (Codespell.Length > 0)
							OutputText("Scenes.Manor.Keyword.PushBookSlotTenth".Localized());

						Code = 0;

						Codespell = "";
					}

					break;

				case 6:
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
								OutputText("Scenes.Manor.Keyword.PushBookSlotTwelfth".Localized());

							Codespell = "";

							Code = 0;
						}
					}

					break;

				case 7:
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
								OutputText("Scenes.Manor.Keyword.PushBookSlotFourteenth".Localized());

							Codespell = "";

							Code = 0;
						}
					}

					break;

				case 8:
					{
						OutputText("Scenes.Manor.Keyword.OpenQabalumMaximus".Localized());

						if (Codespell.Length > 0)
							OutputText("Scenes.Manor.Keyword.PushBookSlotFifteenth".Localized());

						Code = 0;

						Codespell = "";
					}

					break;

				case 9:
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
								OutputText("Scenes.Manor.Keyword.PushBookSlotSeventeenth".Localized());

							Codespell = "";

							Code = 0;
						}
					}

					break;
			}

			DoNext(() => NorthBookshelf());
		}

		public void StairsStart()
		{
			ClearOutput();

			OutputText("Scenes.Manor.StairsStart.CorridorTo".Localized());

			OutputText("Scenes.Manor.StairsStart.DespiteBlockage".Localized());
		}

		public void Stairs0()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Stairs0.AtBaseOf".Localized());

			SetStairButtons(() => SwitchFloor(2, 7));
		}

		public void Stairs1()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Stairs1.SecondFloor".Localized());

			SetStairButtons(null, () => SwitchFloor(1, 2));
		}

		public void Stairs2()
		{
			ClearOutput();

			if (((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 2) != 0) && !((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 4) != 0))
			{
				OutputText("Scenes.Manor.Stairs2.FeelOddApproaching".Localized());

				DoNext(CourtierEncounter);

				return;
			}

			OutputText("Scenes.Manor.Stairs2.SecondFloor".Localized());
		}

		public void CourtierEncounter()
		{
			ClearOutput();

			OutputText("Scenes.Manor.CourtierEncounter.ImagesOfIngnam".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffection) > 70)
				OutputText("Scenes.Manor.CourtierEncounter.SeeKiha".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.HelAffection) >= 70)
				OutputText("Scenes.Manor.CourtierEncounter.SeeHelia".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) >= 40)
				OutputText("Scenes.Manor.CourtierEncounter.SeeAmily".Localized());

			if ((int)Player.StatusEffectValue(StatusEffectIds.Marble, 0) >= 80)
				OutputText("Scenes.Manor.CourtierEncounter.SeeUnbeatableArmy".Localized());

			OutputText("Scenes.Manor.CourtierEncounter.TheyMockYou".Localized());

			OutputText("Scenes.Manor.CourtierEncounter.ImagesNearlyCripple".Localized());

			if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1)
				OutputText("Scenes.Manor.CourtierEncounter.ShouldraAppears".Localized());

			AddButton(0, "Common.Button.Next".Localized(), () => TestResolve(true));
		}

		public void CourtierFight()
		{
			ClearOutput();

			OutputText("Scenes.Manor.CourtierFight.StandUpTemporarilyFree".Localized());

			CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "bone_courtier")), RunFunc);
		}

		public void Study()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Study.SomeKindOf".Localized());

			OutputText("Scenes.Manor.Study.AlthoughManyBooks".Localized());

			AddButton(0, "Cabin.Furniture.Desk.Name".Localized(), () => DeskStudy());

			AddButton(1, "Common.Button.Books".Localized(), ReadStudyBooks);
		}

		public void DeskStudy(int currentlyRead = -1)
		{

			if (!((CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) & 1) != 0))
			{
				CoC.Instance.Flags.Set(GameFlagIds.CodexEphraimJournal, CoC.Instance.Flags.Get(GameFlagIds.CodexEphraimJournal) | (1 << 0));

				OutputText("Scenes.Manor.DeskStudy.NewCodexEntryManor".Localized());
			}

			ClearButtons();

			AddButton(0, "Common.Button.JournalI".Localized(), () => Journal(0, DeskStudy));

			AddButton(1, "Common.Button.JournalIi".Localized(), () => Journal(1, DeskStudy));

			AddButton(2, "Common.Button.JournalIii".Localized(), () => Journal(2, DeskStudy));

			AddButton(3, "Common.Button.JournalIv".Localized(), () => Journal(3, DeskStudy));

			AddButton(4, "Common.Button.JournalV".Localized(), () => Journal(4, DeskStudy));

			AddButton(14, "Common.Button.Back".Localized(), RunFunc);
		}

		public void ReadStudyBooks()
		{
			ClearOutput();

			OutputText("Scenes.Manor.ReadStudyBooks.ReadSomePages".Localized());

			DoNext(RunFunc);
		}

		public void Journal(int section, Action<int> callback)
		{
			ClearOutput();

			switch (section)
			{
				case 0:
					{
						JournalHeader("<b>I</b>");

						OutputText("Scenes.Manor.Journal.IHaveArrived".Localized());

						OutputText("Scenes.Manor.Journal.WritingOnGrimTopics".Localized());

						OutputText("Scenes.Manor.Journal.ManorStillHabitable".Localized());
					}

					break;

				case 1:
					{
						JournalHeader("<b>II</b>");

						OutputText("Scenes.Manor.Journal.SeveralDaysHardWork".Localized());
					}

					break;

				case 2:
					{
						JournalHeader("<b>III</b>");

						OutputText("Scenes.Manor.Journal.MostOfManor".Localized());

						OutputText("Scenes.Manor.Journal.HauntedByNightmares".Localized());

						OutputText("Scenes.Manor.Journal.DreamsDistressMe".Localized());
					}

					break;

				case 3:
					{
						JournalHeader("<b>IV</b>");

						OutputText("Scenes.Manor.Journal.InsistentGnawing".Localized());

						OutputText("Scenes.Manor.Journal.ExcavationsThroughTunnels".Localized());

						OutputText("Scenes.Manor.Journal.TheyContinuedWork".Localized());
					}

					break;

				case 4:
					{
						JournalHeader("<b>V</b>");

						OutputText("Scenes.Manor.Journal.WorkersUncoveredRoom".Localized());

						OutputText("Scenes.Manor.Journal.TakenOverByDesire".Localized());
					}

					break;

				case 5:
					{
						JournalHeader("<b>VI</b>");

						OutputText("Scenes.Manor.Journal.DraggedTheCorpse".Localized());
					}

					break;

				case 6:
					{
						JournalHeader("<b>VII</b>");

						OutputText("Scenes.Manor.Journal.ContinuedFailures".Localized());

						OutputText("Scenes.Manor.Journal.TheyArrivedInHiding".Localized());

						OutputText("Scenes.Manor.Journal.SharedSeveralVolumes".Localized());

						OutputText("Scenes.Manor.Journal.ProcessOverwhelmingSuccess".Localized());
					}

					break;

				case 7:
					{
						JournalHeader("<b>VIII</b>");

						OutputText("Scenes.Manor.Journal.LuredTheInnocentGirl".Localized());

						OutputText("Scenes.Manor.Journal.UsingHerOwnSoul".Localized());

						OutputText("Scenes.Manor.Journal.UsingTheirSouls".Localized());
					}

					break;

				case 8:
					{
						JournalHeader("<b>IX</b>");

						OutputText("Scenes.Manor.Journal.AfterSeveralYears".Localized());

						OutputText("Scenes.Manor.Journal.InMyDreams".Localized());
					}

					break;

				case 9:
					{
						JournalHeader("<b>X</b>");

						OutputText("Scenes.Manor.Journal.SpheresAreAligned".Localized());

						OutputText("Scenes.Manor.Journal.SoonThisWorldWill".Localized());
					}

					break;
			}

			callback(section);
		}

		public void Stairs3()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Stairs3.SecondFloor".Localized());
		}

		public void Bedroom()
		{
			ClearOutput();

			OutputText("Scenes.Manor.Bedroom.InsideABedroom".Localized());

			OutputText("Scenes.Manor.Bedroom.StrangeCirclesRunes".Localized());

			AddButton(0, "Common.Button.CheckBed".Localized(), CheckBed);
		}

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

				CoC.Instance.Flags.Set(GameFlagIds.ManorProgress, CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) | (1 << 1));
			}
			else
				OutputText("Scenes.Manor.CheckBed.NothingElseToFind".Localized());

			DoNext(RunFunc);
		}

		public void ExitDungeon()
		{

			DungeonSession.End();

			OutputText("Scenes.Manor.ExitDungeon.LeaveAccursedManor".Localized());

			ReturnToCamp(1);
		}
	}
}