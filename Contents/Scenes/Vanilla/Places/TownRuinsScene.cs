using MonoCoC.Contents.World;
using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places
{
    [Register<TownRuinsScene>("coc:town_ruins")]
    public class TownRuinsScene : Scene
    {

        public static string furnitureFoundToday = "";

        public static double Floor(double value) => Math.Floor(value);

        public override void Entry()
        {
            RackCount();
        }

        public int RackCount()
        {

            var temp = 0;

            if (Player.HasKeyItem("Equipment Rack - Armor"))
                temp += 1;

            if (Player.HasKeyItem("Equipment Rack - Weapons"))
                temp += 1;

            if (Player.HasKeyItem("Equipment Rack - Shields"))
                temp += 1;

            return temp;
        }

        public int TotalDismantled()
        {

            var temp = 0;

            temp += CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHousesDismantled);

            if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsTownhallScavengeProgress) >= 4)
                temp += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsShopScavengeProgress) >= 4)
                temp += 1;

            return temp;
        }

        public bool IsDiscovered()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AmilyVillageAccessible) > 0;
        }

        public void DiscoverAmilyVillage()
        {
            ClearOutput();

            ShowImage("location-townruins-path");

            OutputText("Scenes.Townruins.DiscoverAmilyVillage.RoamsShores".Localized());

            OutputText("Scenes.Townruins.DiscoverAmilyVillage.FollowPrompt".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), ExploreAmilyVillage);

            AddButton(1, "Common.Button.No".Localized(), DontExploreAmilyVillage);
        }

        public void DontExploreAmilyVillage()
        {
            ClearOutput();

            ShowImage("location-townruins-path");

            OutputText("Scenes.Townruins.DontExploreAmilyVillage.TurnsAway".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ExploreAmilyVillage()
        {
            ClearOutput();

            ShowImage("location-townruins");

            OutputText("Scenes.Townruins.ExploreAmilyVillage.FollowsPath".Localized());

            OutputText("Scenes.Townruins.ExploreAmilyVillage.RuinsUnlocked".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void ExploreVillageRuin()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageExplored, CoC.Instance.Flags.Get(GameFlagIds.AmilyVillageExplored) + 1);

            if (RackCount() < 3 && Random.Shared.Next(5) == 0) FindRack(); else Go<CampScene>();
        }

        public void FindRack()
        {

            int rack = 0;

            var rackArray = new List<int>();

            if (!Player.HasKeyItem("Equipment Rack - Armor"))
                rackArray.Add(0);

            if (!Player.HasKeyItem("Equipment Rack - Weapons"))
                rackArray.Add(1);

            if (!Player.HasKeyItem("Equipment Rack - Shields"))
                rackArray.Add(2);

            rack = rackArray[Random.Shared.Next(rackArray.Count)];

            ShowImage("item-rack");

            OutputText("Scenes.Townruins.FindRack.PicksThroughRuins".Localized());

            switch (rack)
            {
                case 0:
                {
                OutputText("Scenes.Townruins.FindRack.ArmorSingular".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Townruins.FindRack.WeaponSingular".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Townruins.FindRack.ShieldSingular".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Townruins.FindRack.UndefinedSingular".Localized());

            OutputText("Scenes.Townruins.FindRack.RackFragment".Localized());

            switch (rack)
            {
                case 0:
                {
                OutputText("Scenes.Townruins.FindRack.ArmorPlural".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Townruins.FindRack.WeaponPlural".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Townruins.FindRack.ShieldPlural".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Townruins.FindRack.UndefinedPlural".Localized());

            OutputText("Scenes.Townruins.FindRack.ChecksRack".Localized());

            switch (rack)
            {
                case 0:
                {
                Player.CreateKeyItem("Equipment Rack - Armor", 0, 0, 0, 0);
                }

                break;

                case 1:
                {
                Player.CreateKeyItem("Equipment Rack - Weapons", 0, 0, 0, 0);
                }

                break;

                case 2:
                {
                Player.CreateKeyItem("Equipment Rack - Shields", 0, 0, 0, 0);
                }

                break;
            }

            if (true )
                OutputText("Scenes.Townruins.FindRack.ReportBugBanner".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ScavengeTownRuinsOption()
        {

            ShowImage("location-townruins");

            if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsScavengeConsidered) == 0)
            {
                OutputText("Scenes.Townruins.ScavengeTownRuinsOption.VillageDesolate".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.TownRuinsScavengeConsidered, 1);
            }
            else
                OutputText("Scenes.Townruins.ScavengeTownRuinsOption.ReturnsAgain".Localized());

            if (TotalDismantled() < 12)
            {
                OutputText("Scenes.Townruins.ScavengeTownRuinsOption.ParagraphBreak".Localized());

                if (TotalDismantled() < 8)
                {
                    OutputText("Scenes.Townruins.ScavengeTownRuinsOption.BurntHouses".Localized());

                    if (TotalDismantled() >= 1)
                        OutputText("Scenes.Townruins.ScavengeTownRuinsOption.StructuresStripped".Localized(
                            NumberUtils.Num2Text(TotalDismantled()),

                            TotalDismantled() == 1 ? "Common.Text.Is".Localized() : "Common.Text.Are".Localized(),

                            TotalDismantled() == 1 ? "Common.Text.ItsFoundation".Localized() : "Common.Text.TheirFoundations".Localized()
                        ));
                }
                else
                {
                    OutputText("Scenes.Townruins.ScavengeTownRuinsOption.MostRuined".Localized());

                    if (TotalDismantled() >= 10)
                        OutputText("Scenes.Townruins.ScavengeTownRuinsOption.WontBeLong".Localized());
                }

                ClearButtons();

                if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHousesDismantled) < 10)
                    AddButton(0, "Common.Text.House".Localized() + (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHousesDismantled) + 1), DismantleHouse);
                else
                    AddButton(0, "Common.Button.House".Localized(), null);

                if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsTownhallScavengeProgress) < 4)
                    AddButton(1, "Common.Button.TownHall".Localized(), DismantleTownHall);
                else
                    AddButton(1, "Common.Button.TownHall".Localized(), null);

                if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsShopScavengeProgress) < 4)
                    AddButton(2, "Common.Button.Shop".Localized(), DismantleShop);
                else
                    AddButton(2, "Common.Button.Shop".Localized(), null);

                SetExitButton("Common.Button.Back".Localized());
            }
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void DismantleHouse()
        {
            ClearOutput();

            ShowImage("location-townruins-house");

            OutputText("Scenes.Townruins.DismantleHouse.StartsWithHouse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHouseFurnitureFound) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHouseScavengeProgress) == 0 && Random.Shared.Next(100) < 80)
                FindFurniture("house");

            DismantleMainText(GameFlagIds.TownRuinsHouseScavengeProgress, "house");
        }

        public void DismantleTownHall()
        {
            ClearOutput();

            ShowImage("location-townruins-townhall");

            OutputText("Scenes.Townruins.DismantleTownHall.TownHallRemains".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsTownhallFurnitureFound) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TownRuinsTownhallScavengeProgress) == 0)
                FindFurniture("town hall");

            DismantleMainText(GameFlagIds.TownRuinsTownhallScavengeProgress, "town hall");
        }

        public void DismantleShop()
        {
            ClearOutput();

            ShowImage("location-townruins-shop");

            OutputText("Scenes.Townruins.DismantleShop.EyesDrawn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TownRuinsTownhallFurnitureFound) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TownRuinsTownhallScavengeProgress) == 0)
                FindFurniture("shop");

            DismantleMainText(GameFlagIds.TownRuinsShopScavengeProgress, "shop");
        }

        public void DismantleMainText(Flag<int> progress, string buildingDesc = "")
        {

            var nails = 0;

            var wood = 0;

            var stones = 0;

            var resourceGainText = new List<string>();

            OutputText("Scenes.Townruins.DismantleMainText.ParagraphBreak".Localized());

            if (!Player.HasKeyItem("Carpenter's Toolbox"))
            {
                OutputText("Scenes.Townruins.DismantleMainText.InspectsInterior".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (CoC.Instance.Flags.Get(progress) == 0)
            {
                OutputText("Scenes.Townruins.DismantleMainText.ChecksFurniture".Localized());

                OutputText("Scenes.Townruins.DismantleMainText.StartWithRoof".Localized());

                if (!(Player.IsTaur() || Player.CanFly()))
                {
                    if (Player.HasKeyItem("Ladder"))
                        OutputText("Scenes.Townruins.DismantleMainText.HaulsLadder".Localized());
                    else
                    {
                        OutputText("Scenes.Townruins.DismantleMainText.GathersPieces".Localized());

                        if (Player.KeyItemV1("Carpenter's Toolbox") >= 24)
                        {
                            OutputText("Scenes.Townruins.DismantleMainText.TakesNails".Localized());

                            Player.AddKeyValue("Carpenter's Toolbox", 1, -24);
                        }
                        else
                            OutputText("Scenes.Townruins.DismantleMainText.ShortOnNails".Localized());

                        OutputText("Scenes.Townruins.DismantleMainText.SecuresLadder".Localized());

                        OutputText("Scenes.Townruins.DismantleMainText.GainsLadder".Localized());

                        Player.ChangeFatigue(50);

                        Player.CreateKeyItem("Ladder", 0, 0, 0, 0);
                    }

                    OutputText("Scenes.Townruins.DismantleMainText.ClimbsLadder".Localized());
                }
                else
                    if (Player.CanFly())
                        OutputText("Scenes.Townruins.DismantleMainText.NothingUnreachable".Localized());
                    else
                        if (Player.IsTaur() || Player.IsDrider())
                            OutputText("Scenes.Townruins.DismantleMainText.TaurAdvantage".Localized());

                OutputText("Scenes.Townruins.DismantleMainText.RoofInReach".Localized());

                if (furnitureFoundToday != "")
                    OutputText("Scenes.Townruins.DismantleMainText.WithNewFind".Localized(
                        furnitureFoundToday
                    ));

                OutputText("Scenes.Townruins.DismantleMainText.ToCampSuffix".Localized());

                nails = (int)(10 + Random.Shared.Next(20) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 4))));

                wood = (int)(10 + Random.Shared.Next(10) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 8))));
            }
            else
                if (CoC.Instance.Flags.Get(progress) == 1)
                {
                    OutputText("Scenes.Townruins.DismantleMainText.RoofRemoved".Localized());

                    OutputText("Scenes.Townruins.DismantleMainText.PicksBricks".Localized());

                    wood = (int)(10 + Random.Shared.Next(10) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 8))));

                    stones = (int)(5 + Random.Shared.Next(5) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 20))));
                }
                else
                    if (CoC.Instance.Flags.Get(progress) == 2)
                    {
                        OutputText("Scenes.Townruins.DismantleMainText.InteriorCleared".Localized((Player.Strength < 80 ? "Scenes.Urtapregs.LubeSpraySideEffects.GreatWord".Localized() : "Common.Text.ABitOf".Localized())));

                        Player.ChangeFatigue(Math.Max(10, 40 - (float)(int)Floor(Player.Strength / 5)));

                        wood = (int)(10 + Random.Shared.Next(10) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 8))));

                        stones = (int)(5 + Random.Shared.Next(5) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 20))));
                    }
                    else
                        if (CoC.Instance.Flags.Get(progress) == 3)
                        {
                            OutputText("Scenes.Townruins.DismantleMainText.NowReduced".Localized(
                                buildingDesc,

                                buildingDesc
                            ));

                            stones = (int)(5 + Random.Shared.Next(5) + (float)(int)Floor(Random.Shared.Next((int)((Player.Strength + Player.Intelligence) / 12))));
                        }

            Player.AddKeyValue("Carpenter's Toolbox", 1, nails);

            CoC.Instance.Flags.Set(GameFlagIds.CampCabinWoodResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) + wood);

            CoC.Instance.Flags.Set(GameFlagIds.CampCabinStoneResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) + stones);

            if (nails > 0)
                resourceGainText.Add(nails + (nails == 1 ? "Common.Text.Nail".Localized() : "Common.Text.Nails".Localized()));

            if (wood > 0)
                resourceGainText.Add(wood + "Common.Text.Wood".Localized());

            if (stones > 0)
                resourceGainText.Add(stones + (stones == 1 ? "Common.Text.Stone".Localized() : "Common.Text.Stones".Localized()));

            if (nails + wood + stones > 0)
                OutputText("Scenes.Townruins.DismantleMainText.ScavengedBanner".Localized(
                    string.Join(", ", resourceGainText)
                ));

            CoC.Instance.Flags.Set(progress, CoC.Instance.Flags.Get(progress) + 1);

            if (CoC.Instance.Flags.Get(progress) == 4)
                OutputText("Scenes.Townruins.DismantleMainText.FocusInterrupted".Localized(
                    buildingDesc
                ));

            if (progress.Id == GameFlagIds.TownRuinsHouseScavengeProgress.Id && CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHousesDismantled) < 10 && CoC.Instance.Flags.Get(progress) >= 4)
            {
                CoC.Instance.Flags.Set(progress, 0);

                CoC.Instance.Flags.Set(GameFlagIds.TownRuinsHousesDismantled, CoC.Instance.Flags.Get(GameFlagIds.TownRuinsHousesDismantled) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.TownRuinsHouseFurnitureFound, 0);
            }

            furnitureFoundToday = "";

            DoNext(() => ReturnToCamp(1));
        }

        public void FindFurniture(string building = "house")
        {

            if (furnitureFoundToday != "")
                return ;

            var choice = new List<int>();

            int select = 0;

            if (!CabinInterior.HasBed && building == "house")
                choice.Add(0);

            if (!CabinInterior.HasNightstand && building == "house")
                choice.Add(1);

            if (!CabinInterior.HasDresser && building == "house")
                choice.Add(2);

            if (!CabinInterior.HasTable)
                choice.Add(3);

            if (!CabinInterior.HasTableChair1 || !CabinInterior.HasTableChair2 || CabinInterior.HasDeskChair)
                choice.Add(4);

            if (!CabinInterior.HasBookshelf)
                choice.Add(5);

            if (!CabinInterior.HasDesk)
                choice.Add(6);

            select = choice[Random.Shared.Next(choice.Count)];

            switch (select)
            {
                case 0:
                {
                OutputText("Scenes.Townruins.FindFurniture.MovesDebris".Localized());

                OutputText("Scenes.Townruins.FindFurniture.RemovesMattress".Localized());

                furnitureFoundToday = "bed";

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureBed, 1);
                }

                break;

                case 1:
                {
                OutputText("Scenes.Townruins.FindFurniture.LooksInterior".Localized());

                OutputText("Scenes.Townruins.FindFurniture.FindsDrawers".Localized());

                furnitureFoundToday = "nightstand";

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureNightstand, 1);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Townruins.FindFurniture.LooksInteriorAgain".Localized());

                OutputText("Scenes.Townruins.FindFurniture.FindsDrawersAgain".Localized());

                furnitureFoundToday = "dresser";

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureDresser, 1);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Townruins.FindFurniture.FindsTable".Localized());

                furnitureFoundToday = "table";

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureTable, 1);
                }

                break;

                case 4:
                {
                OutputText("Scenes.Townruins.FindFurniture.FindsChair".Localized());

                furnitureFoundToday = "chair";

                if (CabinInterior.HasTableChair2)
                    CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureDeskchair, 1);
                else
                    if (CabinInterior.HasTableChair1)
                        CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureChair2, 1);
                    else
                        CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureChair1, 1);
                }

                break;

                case 5:
                {
                OutputText("Scenes.Townruins.FindFurniture.DebrisWall".Localized());

                OutputText("Scenes.Townruins.FindFurniture.ThinksOfIt".Localized());

                furnitureFoundToday = "bookshelf";

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureBookshelf, 1);
                }

                break;

                case 6:
                {
                OutputText("Scenes.Townruins.FindFurniture.MovesParticularly".Localized());

                furnitureFoundToday = "desk";

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinFurnitureDesk, 1);
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Townruins.FindFurniture.SearchThoroughly".Localized());

                furnitureFoundToday = "null";
            }

            if (furnitureFoundToday != "null")
            {
                OutputText("Scenes.Townruins.FindFurniture.ParagraphBreak".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinProgress) >= 10)
                    OutputText("Scenes.Townruins.FindFurniture.GreatAddition".Localized());

                OutputText("Scenes.Townruins.FindFurniture.HaulsOutside".Localized(
                    furnitureFoundToday,

                    building,

                    building
                ));
            }
        }

    }
}