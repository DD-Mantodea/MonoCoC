using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Dungeons
{

    [Register<LethicesKeepScene>("coc:lethices_keep")]
    public class LethicesKeepScene : Scene
    {

        public const int EggBlack = 1 << 0;

        public const int EggBlue = 1 << 1;

        public const int EggWhite = 1 << 2;

        public const int EggPink = 1 << 3;

        public const int EggBrown = 1 << 4;

        public const int EggPurple = 1 << 5;

        public override void Entry() => DiscoverD3();

        public void DiscoverD3()
        {
            Flags.Set(GameFlagIds.D3Discovered, 1);

            ClearOutput();

            ShowImage("stronghold-entrance");

            OutputText("Scenes.Lethiceskeep.DiscoverD3.DuringYourExplorationYou".Localized());

            OutputText("Scenes.Lethiceskeep.DiscoverD3.YouVeDiscoveredA".Localized());

            OutputText("Scenes.Lethiceskeep.DiscoverD3.DoYouStepInside".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Enter".Localized(), EnterD3);

            AddButton(1, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void EnterD3()
        {
            RoomDungeon.Configure(BuildRooms(), "BasiliskCave");

            RoomDungeon.Move("entrance");
        }

        public void ExitD3()
        {
            RoomDungeon.Exit();

            ReturnToCamp(1);
        }

        public static Room[] BuildRooms()
        {
            var rooms = new Func<Room>[]
            {
                () => new Room { RoomName = "entrance", EastExit = "tunnel1", RoomFunction = () => Instance.EntranceRoomFunc() },

                () => new Room { RoomName = "tunnel1", NorthExit = "antechamber", WestExit = "entrance", RoomFunction = () => Instance.Tunnel1RoomFunc() },

                () => new Room { RoomName = "antechamber", NorthExit = "magpiehalls", EastExit = "roomofmirrors", SouthExit = "tunnel1", RoomFunction = () => Instance.AntechamberRoomFunc() },

                () => new Room { RoomName = "roomofmirrors", WestExit = "antechamber", RoomFunction = () => Instance.RoomofmirrorsRoomFunc() },

                () => new Room { RoomName = "magpiehalls", NorthExit = "tunnel2", SouthExit = "antechamber", RoomFunction = () => Instance.MagpiehallsRoomFunc() },

                () => new Room { RoomName = "magpiehalln", NorthExit = "tunnel2", SouthExit = "antechamber", RoomFunction = () => Instance.MagpiehallnRoomFunc() },

                () => new Room { RoomName = "tunnel2", EastExit = "edgeofkeep", SouthExit = "magpiehalln", RoomFunction = () => Instance.Tunnel2RoomFunc() },

                () => new Room { RoomName = "edgeofkeep", NorthExit = "northentry", WestExit = "tunnel2", RoomFunction = () => Instance.EdgeofkeepRoomFunc() },

                () => new Room { RoomName = "northentry", NorthExit = "southcourtyard", SouthExit = "edgeofkeep", RoomFunction = () => Instance.NorthentryRoomFunc() },

                () => new Room { RoomName = "southcourtyard", SouthExit = "northentry", EastExit = "southeastcourtyard", WestExit = "southwestcourtyard", RoomFunction = () => Instance.SouthcourtyardRoomFunc() },

                () => new Room { RoomName = "southwestcourtyard", EastExit = "southcourtyard", NorthExit = "southwestwalk", RoomFunction = () => Instance.SouthwestcourtyardRoomFunc() },

                () => new Room { RoomName = "southwestwalk", NorthExit = "westwalk", SouthExit = "southwestcourtyard", RoomFunction = () => Instance.SouthwestwalkRoomFunc() },

                () => new Room { RoomName = "westwalk", NorthExit = "northwestwalk", EastExit = "courtyardsquare", SouthExit = "southwestwalk", RoomFunction = () => Instance.WestwalkRoomFunc() },

                () => new Room { RoomName = "northwestwalk", NorthExit = "northwestcourtyard", SouthExit = "westwalk", RoomFunction = () => Instance.NorthwestwalkRoomFunc() },

                () => new Room { RoomName = "northwestcourtyard", EastExit = "northcourtyard", SouthExit = "northwestwalk", RoomFunction = () => Instance.NorthwestcourtyardRoomFunc() },

                () => new Room
                {
                    RoomName = "northcourtyard",

                    EastExit = "northeastcourtyard",

                    WestExit = "northwestcourtyard",

                    NorthExit = "throneroom",

                    NorthExitCondition = () => Instance.UnlockedThroneRoom(),

                    RoomFunction = () => Instance.NorthcourtyardRoomFunc()
                },

                () => new Room { RoomName = "northeastcourtyard", SouthExit = "northeastwalk", WestExit = "northcourtyard", RoomFunction = () => Instance.NortheastcourtyardRoomFunc() },

                () => new Room { RoomName = "northeastwalk", NorthExit = "northeastcourtyard", SouthExit = "eastwalk", RoomFunction = () => Instance.NortheastwalkRoomFunc() },

                () => new Room { RoomName = "eastwalk", NorthExit = "northeastwalk", SouthExit = "southeastwalk", WestExit = "courtyardsquare", RoomFunction = () => Instance.EastwalkRoomFunc() },

                () => new Room { RoomName = "southeastwalk", NorthExit = "eastwalk", SouthExit = "southeastcourtyard", RoomFunction = () => Instance.SoutheastwalkRoomFunc() },

                () => new Room { RoomName = "southeastcourtyard", NorthExit = "southeastwalk", SouthExit = "greatlift", WestExit = "southcourtyard", RoomFunction = () => Instance.SoutheastcourtyardRoomFunc() },

                () => new Room { RoomName = "courtyardsquare", EastExit = "eastwalk", WestExit = "westwalk", RoomFunction = () => Instance.CourtyardsquareRoomFunc() },

                () => new Room { RoomName = "greatlift", NorthExit = "southeastcourtyard", RoomFunction = () => Instance.GreatliftRoomFunc() },

                () => new Room { RoomName = "throneroom", SouthExit = "northcourtyard", RoomFunction = () => Instance.ThroneRoomFunc() }
            };

            var list = new Room[rooms.Length];

            for (var i = 0; i < rooms.Length; i++)
                list[i] = rooms[i]();

            return list;
        }

        public static LethicesKeepScene Instance => SceneInstance<LethicesKeepScene>.Get();

        public bool EntranceRoomFunc()
        {
            ShowImage("stronghold-Entrance-room");

            OutputText("Scenes.Lethiceskeep.EntranceRoomFunc.EntranceRoom".Localized());

            OutputText("Scenes.Lethiceskeep.EntranceRoomFunc.TheInsideOfThis".Localized());

            ClearButtons();

            AddButton(10, "Common.Button.Leave".Localized(), ExitD3);

            return false;
        }

        public bool Tunnel1RoomFunc()
        {
            ShowImage("stronghold-tunnel-1");

            OutputText("Scenes.Lethiceskeep.Tunnel1RoomFunc.Tunnel".Localized());

            OutputText("Scenes.Lethiceskeep.Tunnel1RoomFunc.WindingBackAndForth".Localized());

            return false;
        }

        public bool AntechamberRoomFunc()
        {
            ShowImage("stronghold-Antechamber");

            OutputText("Scenes.Lethiceskeep.AntechamberRoomFunc.Antechamber".Localized());

            OutputText("Scenes.Lethiceskeep.AntechamberRoomFunc.YouAreStandingIn".Localized());

            OutputText("Scenes.Lethiceskeep.AntechamberRoomFunc.AheadIsALarge".Localized());

            return false;
        }

        public bool RoomofmirrorsRoomFunc()
        {
            ShowImage("stronghold-Mirrors-room");

            OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.RoomOfMirrors".Localized());

            OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.TheMetalDoorOpens".Localized());

            if (Flags.Get(GameFlagIds.D3MirrorsShattered) == 1)
                OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.NowShattered".Localized());

            OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.OrnateStandingMirrorDominates".Localized());

            if (Player.HasKeyItem("Laybans"))
                OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.ThePlaceFeelsHollow".Localized());
            else
            {
                OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.NearTheBackNext".Localized());

                if (Flags.Get(GameFlagIds.D3EnteredMagpiehall) == 1)
                    OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.YourSpiritsRiseThey".Localized());

                if (Player.Intelligence >= 70 || Player.Sensitivity >= 70)
                    OutputText("Scenes.Lethiceskeep.RoomofmirrorsRoomFunc.DisquietEdgesDownYour".Localized());

                AddHookButton(2, "Glasses", "coc:keep_hook_glasses",

                    "Block F 未落地：原版 LethicesKeep.hx:279 doppelganger.getDemGlasses");
            }

            return false;
        }

        public bool MagpiehallsRoomFunc()
        {
            ShowImage(Flags.Get(GameFlagIds.D3JeanClaudeDefeated) == 0 ? "stronghold-basilisks" : "stronghold-Magpie-hall");

            OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.MagpieHall".Localized());

            if (Flags.Get(GameFlagIds.D3JeanClaudeDefeated) == 0)
            {
                if (Flags.Get(GameFlagIds.D3EnteredMagpiehall) == 0)
                {
                    Flags.Set(GameFlagIds.D3EnteredMagpiehall, 1);

                    OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.YouCreepThroughThe".Localized());

                    OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.ManyFeetBelowThe".Localized());

                    if (Flags.Get(GameFlagIds.BenoitAffection) == 100)
                        OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.ThisCanOnlyBe".Localized(
                            SceneInstance<BenoitScene>.Get().BenoitMF("Benoit", "Benoite")));

                    OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.YouGetTheFright".Localized());

                    OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.YourHeartbeatQuickensAs".Localized());

                    if (Player.CanFly())
                        OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.EvenIfYouTried".Localized());

                    OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.WouldnTYouYou".Localized());

                    if (Player.HasKeyItem("Laybans"))
                        OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.YouTakeTheLaybans".Localized());
                }
                else
                {
                    OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.AgainYouCreepUp".Localized());

                    if (!Player.HasKeyItem("Laybans"))
                        OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.AreYouGoingTo".Localized());
                    else
                        OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.YouTakeTheLaybans.Two".Localized());
                }

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryBasilisks);

                ClearButtons();

                AddHookButton(0, "Go!", "coc:keep_hook_jeanclaude_south",

                    "Block E 未落地：原版 LethicesKeep.hx:318 jeanClaude.gogoFuckTheseBasilisks");

                AddButton(1, "Common.Button.FallBack".Localized(), FallbackFromMagpieHallS);

                return true;
            }

            OutputText("Scenes.Lethiceskeep.MagpiehallsRoomFunc.YouAreBackIn".Localized());

            if (EggsAvailable() > 0)
                AddButton(2, "Common.Button.Eggs".Localized(), GoToEggPile);

            return false;
        }

        public int EggsAvailable()
        {
            var flagNum = Flags.Get(GameFlagIds.D3EggsAvailable);

            var eggs = 0;

            if ((flagNum & EggBlack) == 0)
                eggs += 1;

            if ((flagNum & EggBlue) == 0)
                eggs += 1;

            if ((flagNum & EggWhite) == 0)
                eggs += 1;

            if ((flagNum & EggPink) == 0)
                eggs += 1;

            if ((flagNum & EggBrown) == 0)
                eggs += 1;

            if ((flagNum & EggPurple) == 0)
                eggs += 1;

            return eggs;
        }

        public void GoToEggPile()
        {
            ClearOutput();

            ShowImage("item-egg-pile");

            OutputText("Scenes.Lethiceskeep.GoToEggPile.YouHeadDownThe".Localized());

            ClearButtons();

            var flagNum = Flags.Get(GameFlagIds.D3EggsAvailable);

            if ((flagNum & EggBlack) == 0)
                AddButton(0, "Common.Button.Black".Localized(), () => TakeEgg(EggBlack));

            if ((flagNum & EggBlue) == 0)
                AddButton(1, "Common.Button.Blue".Localized(), () => TakeEgg(EggBlue));

            if ((flagNum & EggWhite) == 0)
                AddButton(2, "Common.Button.White".Localized(), () => TakeEgg(EggWhite));

            if ((flagNum & EggPink) == 0)
                AddButton(3, "Common.Button.Pink".Localized(), () => TakeEgg(EggPink));

            if ((flagNum & EggBrown) == 0)
                AddButton(4, "Common.Button.Brown".Localized(), () => TakeEgg(EggBrown));

            if ((flagNum & EggPurple) == 0)
                AddButton(5, "Common.Button.Purple".Localized(), () => TakeEgg(EggPurple));

            AddButton(14, "Common.Button.Back".Localized(), RoomDungeon.ResumeFromFight);
        }

        public void TakeEgg(int eggMask)
        {
            var eggId = eggMask switch
            {
                EggBlack => ConsumableIds.EggLargeBlack,

                EggBlue => ConsumableIds.EggLargeBlue,

                EggWhite => ConsumableIds.EggLargeWhite,

                EggPink => ConsumableIds.EggLargePink,

                EggBrown => ConsumableIds.EggLargeBrown,

                _ => ConsumableIds.EggLargePurple
            };

            ShowImage(eggMask switch
            {
                EggBlack => "item-egg-black",

                EggBlue => "item-egg-blue",

                EggWhite => "item-egg-white",

                EggPink => "item-egg-pink",

                EggBrown => "item-egg-brown",

                _ => "item-egg-purple"
            });

            ClearOutput();

            var item = ItemRegistry.Create(eggId);

            OutputText("Scenes.Lethiceskeep.TakeEgg.YouPluckOut".Localized(item.LongName));

            Flags.Set(GameFlagIds.D3EggsAvailable, Flags.Get(GameFlagIds.D3EggsAvailable) + eggMask);

            InventoryScene.TakeItem(eggId, PlayerMenu);
        }

        public void FallbackFromMagpieHallS()
        {
            ClearOutput();

            ShowImage("stronghold-Antechamber");

            OutputText("Scenes.Lethiceskeep.FallbackFromMagpieHallS.NoThereHasTo".Localized());

            if (!Player.HasKeyItem("Laybans") && Player.Intelligence >= 50)
                OutputText("Scenes.Lethiceskeep.FallbackFromMagpieHallS.SurelyTheDemonsThemselves".Localized());

            OutputText("Scenes.Lethiceskeep.FallbackFromMagpieHallS.YouHeadBackThrough".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Next".Localized(), () => RoomDungeon.Move("antechamber"));
        }

        public bool MagpiehallnRoomFunc()
        {
            ShowImage(Flags.Get(GameFlagIds.D3JeanClaudeDefeated) == 0 ? "stronghold-Magpie-locked" : "stronghold-Magpie-hall");

            OutputText("Scenes.Lethiceskeep.MagpiehallnRoomFunc.MagpieHall".Localized());

            if (Flags.Get(GameFlagIds.D3JeanClaudeDefeated) == 0)
            {
                OutputText("Scenes.Lethiceskeep.MagpiehallnRoomFunc.YouFindYourselfBack".Localized());

                ClearButtons();

                AddHookButton(0, "Go!", "coc:keep_hook_jeanclaude_north",

                    "Block E 未落地：原版 LethicesKeep.hx:455 jeanClaude.gogoFuckTheseBasilisksNorth");

                AddButton(1, "Common.Button.Stronghold".Localized(), () => RoomDungeon.Move("tunnel2"));

                return true;
            }

            OutputText("Scenes.Lethiceskeep.MagpiehallnRoomFunc.YouAreBackIn".Localized());

            if (EggsAvailable() > 0)
                AddButton(2, "Common.Button.Eggs".Localized(), GoToEggPile);

            return false;
        }

        public bool Tunnel2RoomFunc()
        {
            RoomDungeon.InRoomedDungeonName = "BasiliskCave";

            ShowImage("stronghold-tunnel-2");

            OutputText("Scenes.Lethiceskeep.Tunnel2RoomFunc.Tunnel".Localized());

            OutputText("Scenes.Lethiceskeep.Tunnel2RoomFunc.LightTricklesInFrom".Localized());

            return false;
        }

        public bool EdgeofkeepRoomFunc()
        {
            RoomDungeon.InRoomedDungeonName = "LethicesKeep";

            ShowImage("stronghold-Edge-of-Keep");

            OutputText("Scenes.Lethiceskeep.EdgeofkeepRoomFunc.EdgeOfKeep".Localized());

            OutputText("Scenes.Lethiceskeep.EdgeofkeepRoomFunc.StandingOnTheEdge".Localized());

            return false;
        }

        public bool NorthentryRoomFunc()
        {
            ShowImage("stronghold-North-entry");

            OutputText("Scenes.Lethiceskeep.NorthentryRoomFunc.NorthEntry".Localized());

            OutputText("Scenes.Lethiceskeep.NorthentryRoomFunc.YouNowStandIn".Localized());

            return false;
        }

        public bool SouthcourtyardRoomFunc()
        {
            ShowImage("stronghold-courtyard-South");

            OutputText("Scenes.Lethiceskeep.SouthcourtyardRoomFunc.SouthCourtyard".Localized());

            OutputText("Scenes.Lethiceskeep.SouthcourtyardRoomFunc.LethiceSCourtyardIs".Localized());

            return false;
        }

        public bool SouthwestcourtyardRoomFunc()
        {
            ShowImage("stronghold-courtyard-SW");

            OutputText("Scenes.Lethiceskeep.SouthwestcourtyardRoomFunc.SouthwestCourtyard".Localized());

            OutputText("Scenes.Lethiceskeep.SouthwestcourtyardRoomFunc.SomeOfTheNearby".Localized());

            return false;
        }

        public bool SouthwestwalkRoomFunc()
        {
            ShowImage(Flags.Get(GameFlagIds.D3GardenerDefeated) == 0 ? "sGardener-encounter" : "stronghold-walk-SW");

            OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.SouthwestWalk".Localized());

            OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.TheBushesSurroundingThe".Localized());

            if (Flags.Get(GameFlagIds.D3GardenerDefeated) == 0)
            {
                OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.TheSlickFoliageParts".Localized());

                OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.YouShakeYourHead".Localized());

                if (Player.Lust <= 75)
                    OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.Text".Localized());
                else
                    OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.YouGropeYourselfAbsentmindedly".Localized());

                OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.TheCorruptTemptressSays".Localized());

                OutputText("Scenes.Lethiceskeep.SouthwestwalkRoomFunc.SheSmilesEncouragingly".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), () => CombatScene.Start(
                    EnemyRegistry.Create(new Identifier("coc", "succubus_gardener")),

                    RoomDungeon.ResumeFromFight));

                AddHookButton(1, "Surrender", "coc:keep_hook_gardener_surrender",

                    "Block E 未落地：原版 LethicesKeep.hx:524 succubusGardener.surrenderToTheGardener");

                return true;
            }

            return false;
        }

        public bool WestwalkRoomFunc()
        {
            ShowImage("stronghold-walk-West");

            OutputText("Scenes.Lethiceskeep.WestwalkRoomFunc.WestWalk".Localized());

            OutputText("Scenes.Lethiceskeep.WestwalkRoomFunc.PollenClingsToYou".Localized());

            if (Flags.Get(GameFlagIds.D3StatueDefeated) == 0)
                OutputText("Scenes.Lethiceskeep.WestwalkRoomFunc.SeeAMassiveStatue".Localized());
            else
                OutputText("Scenes.Lethiceskeep.WestwalkRoomFunc.AMoundOfRubble".Localized());

            OutputText("Scenes.Lethiceskeep.WestwalkRoomFunc.TheWarmSandstoneBricks".Localized());

            return false;
        }

        public bool NorthwestwalkRoomFunc()
        {
            ShowImage("stronghold-walk-NW");

            OutputText("Scenes.Lethiceskeep.NorthwestwalkRoomFunc.NorthwestWalk".Localized());

            OutputText("Scenes.Lethiceskeep.NorthwestwalkRoomFunc.ANarrowPathSplits".Localized());

            OutputText("Scenes.Lethiceskeep.NorthwestwalkRoomFunc.YouSmirkAtYour".Localized());

            OutputText("Scenes.Lethiceskeep.NorthwestwalkRoomFunc.TheCourtyardItselfContinues".Localized());

            return false;
        }

        public bool NorthwestcourtyardRoomFunc()
        {
            ShowImage("stronghold-courtyard-NW");

            OutputText("Scenes.Lethiceskeep.NorthwestcourtyardRoomFunc.NorthwestCourtyard".Localized());

            OutputText("Scenes.Lethiceskeep.NorthwestcourtyardRoomFunc.TheCourtyardComesTo".Localized());

            return false;
        }

        public bool NorthcourtyardRoomFunc()
        {
            ShowImage("stronghold-courtyard-North");

            OutputText("Scenes.Lethiceskeep.NorthcourtyardRoomFunc.NorthCourtyard".Localized());

            OutputText("Scenes.Lethiceskeep.NorthcourtyardRoomFunc.YouStandBeforeWhat".Localized());

            if (UnlockedThroneRoom())
                OutputText("Scenes.Lethiceskeep.NorthcourtyardRoomFunc.TheSealAppearsTo".Localized());
            else
                OutputText("Scenes.Lethiceskeep.NorthcourtyardRoomFunc.IfOnlyTheDoor".Localized());

            return false;
        }

        public bool NortheastcourtyardRoomFunc()
        {
            ShowImage("stronghold-courtyard-NE");

            OutputText("Scenes.Lethiceskeep.NortheastcourtyardRoomFunc.NortheastCourtyard".Localized());

            OutputText("Scenes.Lethiceskeep.NortheastcourtyardRoomFunc.ThisParticularCornerOf".Localized());

            if (Flags.Get(GameFlagIds.D3CentaurDefeated) == 0)
            {
                ClearButtons();

                if (!InvokeHook("coc:keep_hook_hermcentaur"))
                    AddButtonDisabled(0, "Next", "Block D 未落地：原版 LethicesKeep.hx:576 hermCentaur.encounterThePony");

                return true;
            }

            return false;
        }

        public bool NortheastwalkRoomFunc()
        {
            ShowImage("stronghold-walk-NE");

            OutputText("Scenes.Lethiceskeep.NortheastwalkRoomFunc.NortheastWalk".Localized());

            OutputText("Scenes.Lethiceskeep.NortheastwalkRoomFunc.TheAirIsPleasant".Localized());

            return false;
        }

        public bool EastwalkRoomFunc()
        {
            ShowImage("stronghold-walk-East");

            OutputText("Scenes.Lethiceskeep.EastwalkRoomFunc.EastWalk".Localized());

            OutputText("Scenes.Lethiceskeep.EastwalkRoomFunc.TheSmoothAlmostFlawlessly".Localized());

            if (Flags.Get(GameFlagIds.D3StatueDefeated) == 0)
                OutputText("Scenes.Lethiceskeep.EastwalkRoomFunc.AMassiveStatueWith".Localized());
            else
                OutputText("Scenes.Lethiceskeep.EastwalkRoomFunc.AMoundOfRubble".Localized());

            return false;
        }

        public bool SoutheastwalkRoomFunc()
        {
            ShowImage("stronghold-walk-SE");

            OutputText("Scenes.Lethiceskeep.SoutheastwalkRoomFunc.SoutheastWalk".Localized());

            OutputText("Scenes.Lethiceskeep.SoutheastwalkRoomFunc.SwarmsOfButterfliesCongregate".Localized());

            return false;
        }

        public bool SoutheastcourtyardRoomFunc()
        {
            ShowImage("stronghold-courtyard-SE");

            OutputText("Scenes.Lethiceskeep.SoutheastcourtyardRoomFunc.SoutheastCourtyard".Localized());

            OutputText("Scenes.Lethiceskeep.SoutheastcourtyardRoomFunc.WalkingAlongTheSandstone".Localized());

            return false;
        }

        public bool CourtyardsquareRoomFunc()
        {
            ShowImage("stronghold-courtyard-square");

            OutputText("Scenes.Lethiceskeep.CourtyardsquareRoomFunc.CourtyardSquare".Localized());

            OutputText("Scenes.Lethiceskeep.CourtyardsquareRoomFunc.ACircleOfPolished".Localized());

            if (Flags.Get(GameFlagIds.D3StatueDefeated) == 0)
            {
                OutputText("Scenes.Lethiceskeep.CourtyardsquareRoomFunc.WaitWhatSThat".Localized());

                ClearButtons();

                if (!InvokeHook("coc:keep_hook_livingstatue"))
                    AddButtonDisabled(0, "Next", "Block C 未落地：原版 LethicesKeep.hx:624 livingStatue.encounter");

                return true;
            }

            OutputText("Scenes.Lethiceskeep.CourtyardsquareRoomFunc.TwoDisembodiedMarbleFeet".Localized());

            return false;
        }

        public bool GreatliftRoomFunc()
        {
            ShowImage("stronghold-lift");

            OutputText("Scenes.Lethiceskeep.GreatliftRoomFunc.GreatLift".Localized());

            OutputText("Scenes.Lethiceskeep.GreatliftRoomFunc.IntricateStoneworkSupportsThis".Localized());

            SceneInstance<IncubusMechanicScenes>.Get().MeetAtElevator();

            return false;
        }

        public bool ThroneRoomFunc()
        {
            if (Flags.Get(GameFlagIds.DriderincubusDefeated) == 0)
            {
                ClearButtons();

                if (!InvokeHook("coc:keep_hook_drider"))
                    AddButtonDisabled(0, "Next", "Block C 未落地：原版 LethicesKeep.hx:641 driderIncubus.encounterDriderIncbutt");

                return true;
            }

            if (Flags.Get(GameFlagIds.MinotaurkingDefeated) == 0)
            {
                ClearButtons();

                if (!InvokeHook("coc:keep_hook_minotaurking"))
                    AddButtonDisabled(0, "Next", "Block D 未落地：原版 LethicesKeep.hx:645 minotaurKing.encounterMinotaurKing");

                return true;
            }

            if (Flags.Get(GameFlagIds.LethiceDefeated) == 0)
            {
                ClearButtons();

                if (!InvokeHook("coc:keep_hook_lethice"))
                    AddButtonDisabled(0, "Next", "Block B 未落地：原版 LethicesKeep.hx:649 lethice.encounterLethice");

                return true;
            }

            ShowImage("stronghold-Throne-room");

            OutputText("Scenes.Lethiceskeep.ThroneRoomFunc.ThroneRoom".Localized());

            OutputText("Scenes.Lethiceskeep.ThroneRoomFunc.TheThroneRoomIs".Localized());

            return false;
        }

        public bool UnlockedThroneRoom() =>

            Flags.Get(GameFlagIds.D3GardenerDefeated) > 0

            && Flags.Get(GameFlagIds.D3CentaurDefeated) > 0

            && Flags.Get(GameFlagIds.D3StatueDefeated) > 0;

        public static bool InvokeHook(string hookId)
        {
            var hook = KeepHooks.Get(hookId);

            if (hook == null)
                return false;

            hook();

            return true;
        }

        public void AddHookButton(int slot, string label, string hookId, string seamHint)
        {
            var hook = KeepHooks.Get(hookId);

            if (hook != null)
                AddButton(slot, label, hook);
            else
                AddButtonDisabled(slot, label, seamHint);
        }
    }
}