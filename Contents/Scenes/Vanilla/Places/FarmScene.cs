using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.Farm;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Internals;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places
{
    [Register<FarmScene>("coc:farm")]
    public class FarmScene : Scene
    {

        public static bool UrtaDisabled => OptionStore.Instance.Get("urtaDisabled", 0) > 0;

        public static SaveContent Save => SaveContent(SceneIds.Places.Farm);

        public string debugName => "Farm";

        public string debugHint => "";

        public bool marbleScene => false;

        public static bool easyMode => OptionStore.Instance.Get("easy", 0) > 0;

        public static bool debug => OptionStore.Instance.Get("debug", 0) > 0;

        public override void Entry()
        {
            FarmExploreEncounter();
        }

        public void Reset()
        {

            Save.SetBool("pureWhitneyStables", false);

            Save.SetBool("coc:gnoll", false);

            Save.SetBool("firstEncounterNight", false);
        }

        public bool FarmEnabled()
        {

            return Player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.FarmDisabled) == 0;
        }

        public bool FarmCorrupt()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) > 0;
        }

        public void WhitneySprite()
        {

        }

        public void FarmExploreEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) > 0)

            FarmCorruption.RootScene(); // 农场已被腐化接管：进入腐化农场菜单（原版 FarmCorruption.rootScene）

            if (FarmCorruption.takeoverPrompt() == true)
                return ;

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmDisabled) == 1 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyGone) < 1)
            {
                OutputText("Scenes.Farm.FarmExploreEncounter.MarchesUpMarble".Localized());

                OutputText("Scenes.Farm.FarmExploreEncounter.AfterWhatDidMarble".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmDisabled) == 2 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyGone) < 1)
            {
                OutputText("Scenes.Farm.FarmExploreEncounter.MarchesUpKelt".Localized());

                OutputText("Scenes.Farm.FarmExploreEncounter.AfterWhatDidKelt".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            ShowImage("location-ingnam-farm");

            if (Player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) < 2)
            {
                if (!Player.HasStatusEffect(StatusEffectIds.MetWhitney))
                {
                    if (View.Time.IsDay)
                    {
                        Player.CreateStatusEffect(StatusEffectIds.MetWhitney, 0, 0, 0, 0);

                        OutputText("Scenes.Farm.FarmExploreEncounter.QuaintFarmhouse".Localized(
                            Save.GetBool("firstEncounterNight")
                        ));

                        OutputText("Scenes.Farm.FarmExploreEncounter.FarmerIsAWoman".Localized((noFur ? "dog-like ears and a fluffy tail" : "fur and canine-like features")));
                    }
                    else
                    {
                        OutputText("Scenes.Farm.FarmExploreEncounter.LakesideStrollSpot".Localized());

                        ClearButtons();

                        AddNextButton("Common.Button.Steal".Localized(), NightFarmSteal);

                        AddNextButton("Common.Button.Leave".Localized(), NightFarmLeave);

                        return ;
                    }
                }
                else
                {
                    OutputText("Scenes.Farm.FarmExploreEncounter.StumbleAcrossFarm".Localized((noFur ? "dog-eared" : "anthropomorphic canine")));

                    Player.ChangeStatusValue(StatusEffectIds.MetWhitney, 0, 1);

                    if (Player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) == 2)
                    {
                        OutputText("Scenes.Farm.FarmExploreEncounter.AfterSeveralVisits".Localized());

                        OutputText("Scenes.Farm.FarmExploreEncounter.FarmAddedToPlaces".Localized());
                    }
                }

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CaninePepper), 1);

ReturnToCamp(1);
            }
            else
            {
                ShowImage("location-ingnam-farm");

                Player.ChangeStatusValue(StatusEffectIds.MetWhitney, 0, 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.KeltKilled) >= 1)
                {
                    OutputText("Scenes.Farm.FarmExploreEncounter.StormingUpKilledKelt".Localized());

                    OutputText("Scenes.Farm.FarmExploreEncounter.HandsUpKilledKelt".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.FarmDisabled, 2);

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyFlippedOutOverKelly) == 0)
                {
                    OutputText("Scenes.Farm.FarmExploreEncounter.StormingUpTarnation".Localized());

                    OutputText("Scenes.Farm.FarmExploreEncounter.HoldHandsUpTarnation".Localized());

                    OutputText("Scenes.Farm.FarmExploreEncounter.WhitneyFumes".Localized());

                    OutputText("Scenes.Farm.FarmExploreEncounter.StartsToGrowl".Localized());

                    OutputText("Scenes.Farm.FarmExploreEncounter.SpinsAboutTrotsBack".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.WhitneyFlippedOutOverKelly, 1);

                    FarmMenu(false);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyFlippedOutOverKelly) == 1)
                    {
                        OutputText("Scenes.Farm.FarmExploreEncounter.NotWelcomeOnFarm".Localized());

                        FarmMenu(false);
                    }
                    else
                        FarmMenu();
            }
        }

        public void NightFarmSteal()
        {
            ClearOutput();

            OutputText("Scenes.Farm.NightFarmSteal.ManyPlantsGrowing".Localized());

            OutputText("Scenes.Farm.NightFarmSteal.FightInTheDark".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));

            if (Random.Shared.Next(100) < (50))
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CaninePepper), 1);

ReturnToCamp(1);
            }
            else
                InventoryScene.TakeItem(RandomUtils.Choice(ItemIds.LargeCaninePepper, ItemIds.DoubleCaninePepper, ItemIds.BlackCaninePepper), () => ReturnToCamp(1));
        }

        public void NightFarmLeave()
        {
            ClearOutput();

            OutputText("Scenes.Farm.NightFarmLeave.ShakeYourHead".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FarmMenu(bool intro = true)
        {
            ClearOutput();

            if (intro)
                if (View.Time.IsDay)
                    OutputText("Scenes.Farm.FarmMenu.ManyPlotsCrops".Localized());
                else
                    OutputText("Scenes.Farm.FarmMenu.AlthoughSunAbsent".Localized());

            ClearButtons();

            if (Player.HasStatusEffect(StatusEffectIds.Kelt) && !Player.HasStatusEffect(StatusEffectIds.KeltOff) && CoC.Instance.Flags.Get(GameFlagIds.KeltKilled) <= 0)
                if (CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) >= 4)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyFlippedOutOverKelly) == 1)
                        AddButton(0, "Common.Button.Kelly".Localized(), () => { });
                    else
                        AddButton(3, "Common.Button.Kelly".Localized(), () => { });
                }
                else
                    AddButton(3, "Common.Button.Kelt".Localized(), () => { });

            if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyFlippedOutOverKelly) != 1 || CoC.Instance.Flags.Get(GameFlagIds.WhitneyGone) > 0)
            {
                AddButton(0, "Common.Button.Explore".Localized(), ExploreFarm);

                AddButton(1, "Common.Button.Talk".Localized(), TalkWhitney);

                AddButton(2, "Common.Button.Work".Localized(), WorkFarm);

                if (Player.HasStatusEffect(StatusEffectIds.Marble))
                    AddButton(4, "Common.Button.Marble".Localized(), MeetMarble);

                if (CoC.Instance.Flags.Get(GameFlagIds.FarmStables) > 0)
                    AddButton(5, "Common.Button.Stables".Localized(), StablesMenu);

                if (Player.HasKeyItem("Breast Milker - Installed At Whitney's Farm"))
                    AddButton(6, "Common.Button.GetMilked".Localized(), GetMilked);

                if (Player.HasKeyItem("Cock Milker - Installed At Whitney's Farm") && Player.CockTotal() > 0)
                    AddButton(7, "Common.Button.MilkCock".Localized(), CockPumping);
            }

            SetExitButton("Common.Button.Back".Localized());
        }

        public void WhitneyMilkerHookup(bool breast = true)
        {
            ClearOutput();

            OutputText("Scenes.Farm.WhitneyMilkerHookup.TakesGearBack".Localized());

            OutputText("Scenes.Farm.WhitneyMilkerHookup.ShrugHeadBack".Localized());

            if (breast)
            {
                Player.CreateKeyItem("Breast Milker - Installed At Whitney's Farm", 0, 0, 0, 0);

                Player.RemoveKeyItem("Breast Milker");
            }
            else
            {
                Player.CreateKeyItem("Cock Milker - Installed At Whitney's Farm", 0, 0, 0, 0);

                Player.RemoveKeyItem("Cock Milker");
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void WhitneyMilkerRefusal()
        {
            ClearOutput();

            OutputText("Scenes.Farm.WhitneyMilkerRefusal.WhitneyShrugs".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkWhitney()
        {
            ClearOutput();

            if (!Player.HasKeyItem("Fake Mare") && Player.IsTaur())
            {
                CentaurToysHoooooo();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MurbleFarmTalkLevels) > 0 && Player.HasStatusEffect(StatusEffectIds.Kelt) && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkMurbleAndKelt) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyTalkMurbleAndKelt, 1);

                OutputText("Scenes.Farm.TalkWhitney.WhitneyUnderTreeBook".Localized());

                OutputText("Scenes.Farm.TalkWhitney.NoticeNoseyingAround".Localized());

                OutputText("Scenes.Farm.TalkWhitney.ThoseTwoHardWorkers".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            // 原版 Farm.hx:293 — 已进过 Tel'Adre、尚未聊过、且已与 Murble/Kelt 谈过
            if (SceneInstance<TelAdre.TelAdre>.Get().IsDiscovered() && SceneInstance<TelAdre.TelAdre>.Get().IsAllowedInto() && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkTeladre) == 0 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkMurbleAndKelt) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyTalkTeladre, 1);

                OutputText("Scenes.Farm.TalkWhitney.DogWomanHandMilking".Localized());

                OutputText("Scenes.Farm.TalkWhitney.StrangerHowYouDoin".Localized(UrtaDisabled

                    ? ""

                    : "Scenes.Farm.TalkWhitney.UrtaStillAround".Localized()));

                OutputText("Scenes.Farm.TalkWhitney.AskWhySheLeft".Localized());

                OutputText("Scenes.Farm.TalkWhitney.HadMyReasons".Localized(UrtaDisabled

                    ? ""

                    : "Scenes.Farm.TalkWhitney.AsForUrta".Localized()));

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredHighMountain) > 0 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkHighMountain) == 0 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkTeladre) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyTalkHighMountain, 1);

                OutputText("Scenes.Farm.TalkWhitney.OutsideMilkingShedLabels".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.BreastsMilked))
                    OutputText("Scenes.Farm.TalkWhitney.LabelsWithYourName".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) != -1)
                    OutputText("Scenes.Farm.TalkWhitney.ClusterLabeledJojo".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) == 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) == -1)
                        OutputText("Scenes.Farm.TalkWhitney.ClusterLabeledIsabella".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.JojoCockMilkingCounter) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) == -1)
                            OutputText("Scenes.Farm.TalkWhitney.ClustersJojoAndIsabella".Localized());

                OutputText("Scenes.Farm.TalkWhitney.WhoSheSellsTo".Localized());

                OutputText("Scenes.Farm.TalkWhitney.CentaursAndGoblins".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.BreastsMilked))
                    OutputText("Scenes.Farm.TalkWhitney.ChargeTopGem".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.CockPumped))
                {
                    if (!Player.HasStatusEffect(StatusEffectIds.BreastsMilked))
                        OutputText("Scenes.Farm.TalkWhitney.ParagraphBreak".Localized());

                    OutputText("Scenes.Farm.TalkWhitney.SmallerBottles".Localized());
                }

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkDungeon) == 0 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkHighMountain) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyTalkDungeon, 1);

                OutputText("Scenes.Farm.TalkWhitney.NotAroundBuildings".Localized());

                OutputText("Scenes.Farm.TalkWhitney.WhatsUpHaltingly".Localized());

                OutputText("Scenes.Farm.TalkWhitney.EasyToBelieve".Localized());

                OutputText("Scenes.Farm.TalkWhitney.KnewGuyLikeYou".Localized());

                OutputText("Scenes.Farm.TalkWhitney.StopsForLongWhile".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkDungeonFollowup) == 0 && CoC.Instance.Flags.Get(GameFlagIds.WhitneyTalkDungeon) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyTalkDungeonFollowup, 1);

                OutputText("Scenes.Farm.TalkWhitney.HardAtWorkPepper".Localized());

                OutputText("Scenes.Farm.TalkWhitney.AlmostDoneHereDay".Localized());

                OutputText("Scenes.Farm.TalkWhitney.MightComeAcrossBad".Localized());

                OutputText("Scenes.Farm.TalkWhitney.ColorDisappeared".Localized(UrtaDisabled

                    ? "some others"

                    : "Urta"));

                OutputText("Scenes.Farm.TalkWhitney.GoodbyeWithHug".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            var temp = Random.Shared.Next(6);

            if (temp == 0)
                OutputText("Scenes.Farm.TalkWhitney.FindFarmerGirl".Localized());

            if (temp == 1)
                OutputText("Scenes.Farm.TalkWhitney.PatrollingWithScythe".Localized());

            if (temp == 2)
                OutputText("Scenes.Farm.TalkWhitney.BentOverPepperFields".Localized((noFur ? "face" : "fur and muzzle")));

            if (temp == 3)
                OutputText("Scenes.Farm.TalkWhitney.BehindBarnForge".Localized());

            if (temp == 4)
                OutputText("Scenes.Farm.TalkWhitney.RoundingUpCows".Localized());

            if (temp == 5)
                OutputText("Scenes.Farm.TalkWhitney.LeaningBackHat".Localized());

            if (Random.Shared.Next(4) == 0 && !Player.HasKeyItem("Breast Milker - Installed At Whitney's Farm"))
                if (Player.HasKeyItem("Breast Milker"))
                {
                    OutputText("Scenes.Farm.TalkWhitney.DemonMachineBreast".Localized());

                    OutputText("Scenes.Farm.TalkWhitney.LiberatedFromFactoryBreast".Localized());

                    OutputText("Scenes.Farm.TalkWhitney.ExplainsPumpSystemBreast".Localized());

                    OutputText("Scenes.Farm.TalkWhitney.GiveBreastMilkerQuestion".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), () => WhitneyMilkerHookup());

                    AddButton(1, "Common.Button.No".Localized(), WhitneyMilkerRefusal);

                    return ;
                }
                else
                    if (Player.BiggestLactation() >= 2)
                    {
                        OutputText("Scenes.Farm.TalkWhitney.BemusedLookChat".Localized());

                        OutputText("Scenes.Farm.TalkWhitney.GesturingAtWetness".Localized());

                        if (Player.Corruption < 33)
                            OutputText("Scenes.Farm.TalkWhitney.BlushWithShame".Localized());
                        else
                            if (Player.Corruption <= 66)
                                OutputText("Scenes.Farm.TalkWhitney.FlushWithExhibitionism".Localized());
                            else
                                OutputText("Scenes.Farm.TalkWhitney.FlushHotlyArchBack".Localized());

                        OutputText("Scenes.Farm.TalkWhitney.RunnersOfMilk".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                        OutputText("Scenes.Farm.TalkWhitney.GiggleStifled".Localized());

                        OutputText("Scenes.Farm.TalkWhitney.TooGoodTrue".Localized());

                        if (Player.Gems >= 250)
                        {
                            OutputText("Scenes.Farm.TalkWhitney.PurchaseBreastMilkerQuestion".Localized());

                            AddButton(0, "Common.Button.Yes".Localized(), BreastMilkerPurchase);

                            AddButton(1, "Common.Button.No".Localized(), BreastMilkerNoPurchase);
                        }
                        else
                        {
                            OutputText("Scenes.Farm.TalkWhitney.NotEnoughMoneyMilker".Localized());

                            DoNext(() => ReturnToCamp(1));
                        }

                        return ;
                    }

            if (Random.Shared.Next(4) == 0 && !Player.HasKeyItem("Cock Milker - Installed At Whitney's Farm") && Player.HasKeyItem("Cock Milker"))
            {
                OutputText("Scenes.Farm.TalkWhitney.DemonMachineCock".Localized());

                OutputText("Scenes.Farm.TalkWhitney.LiberatedFromFactoryCock".Localized());

                OutputText("Scenes.Farm.TalkWhitney.ExplainsPumpSystemCock".Localized());

                OutputText("Scenes.Farm.TalkWhitney.GiveCockMilkerQuestion".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => WhitneyMilkerHookup(false));

                AddButton(1, "Common.Button.No".Localized(), WhitneyMilkerRefusal);

                return ;
            }

            OutputText("Scenes.Farm.TalkWhitney.TellOfRecentTrials".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Farm.TalkWhitney.PartsSheWouldHear".Localized());

            OutputText("Scenes.Farm.TalkWhitney.ListensAttentively".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) < 2)
                OutputText("Scenes.Farm.TalkWhitney.FarmDoingWell".Localized());
            else
                OutputText("Scenes.Farm.TalkWhitney.FarmDoingPoorly".Localized());

            OutputText("Scenes.Farm.TalkWhitney.FeelsGoodToTalk".Localized());

            if (Player.Intelligence < 15)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Intelligence < 30)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            Player.ApplyStats(new DynStat(GameStats.Lust, -5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void BreastMilkerPurchase()
        {

            OutputText("Scenes.Farm.BreastMilkerPurchase.TakesGemsAndLeaves".Localized());

            DoNext(() => ReturnToCamp(1));

            Player.CreateKeyItem("Breast Milker - Installed At Whitney's Farm", 0, 0, 0, 0);

            Player.Gems -= 250;

        }

        public void BreastMilkerNoPurchase()
        {

            DoNext(() => ReturnToCamp(1));

            OutputText("Scenes.Farm.BreastMilkerNoPurchase.ShrugsAndChats".Localized());

            if (Player.Intelligence < 15)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Intelligence < 30)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));
        }

        public void WorkFarm()
        {
            ClearOutput();

            float marbling = 0;

            if (!Player.HasStatusEffect(StatusEffectIds.NoMoreMarble) && Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                if (SceneInstance<MarbleScene>.Get().KnowAddiction == 1)
                    SceneInstance<MarbleScene>.Get().AddictedEncounterHappy();
                else
                    SceneInstance<MarbleScene>.Get().EncounterMarbleAshamedAddiction();

                return ;
            }

            if (Random.Shared.Next(3) == 0 && !Player.HasStatusEffect(StatusEffectIds.NoMoreMarble) && Player.HasStatusEffect(StatusEffectIds.Marble))
            {
                if (Player.HasStatusEffect(StatusEffectIds.MarbleRapeAttempted) || CoC.Instance.Flags.Get(GameFlagIds.MarbleWarning) == 3)
                {
                    SceneInstance<MarbleScene>.Get().MarbleAfterRapeBattle();

                    Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleWarning) == 1)
                {
                    SceneInstance<MarbleScene>.Get().MarbleWarningStateMeeting();

                    return ;
                }

                if (Player.HasStatusEffect(StatusEffectIds.Marble))
                    if (SceneInstance<MarbleScene>.Get().KnowAddiction == 0)
                    {
                        marbling = Random.Shared.Next(2);

                        if (marbling == 0)
                            SceneInstance<MarbleScene>.Get().HelpMarble1();

                        if (marbling == 1)
                            SceneInstance<MarbleScene>.Get().HelpMarble2();

                        return ;
                    }
                    else
                    {
                        if (Player.HasPerk(PerkIds.MarbleResistant))
                        {
                            SceneInstance<MarbleScene>.Get().PostAddictionFarmHelpings();

                            return ;
                        }

                        if (SceneInstance<MarbleScene>.Get().KnowAddiction == 1)
                        {
                            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                                marbling = 0;
                            else
                                marbling = 1;

                            if (marbling == 0)
                                SceneInstance<MarbleScene>.Get().AddictedEncounterHappy();
                            else
                                SceneInstance<MarbleScene>.Get().MarbleEncounterAddictedNonWithdrawl();

                            return ;
                        }
                        else
                        {
                            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                                marbling = 0;
                            else
                                marbling = 1;

                            if (marbling == 0)
                                SceneInstance<MarbleScene>.Get().EncounterMarbleAshamedAddiction();
                            else
                                SceneInstance<MarbleScene>.Get().MarbleEncounterAddictedNonWithdrawlAshamed();

                            return ;
                        }
                    }
            }

            if (Random.Shared.Next(4) == 0)
            {
                OutputText("Scenes.Farm.WorkFarm.GettingScythe".Localized());

                OutputText("Scenes.Farm.WorkFarm.SmellHitsYou".Localized());

                OutputText("Scenes.Farm.WorkFarm.OpeningEmptyStall".Localized());

                OutputText("Scenes.Farm.WorkFarm.SteelYourself".Localized());

                if (new[] { BodyPartTypes.Dog, BodyPartTypes.Fox, BodyPartTypes.Cat, BodyPartTypes.Catgirl }.Contains(Player.Body.Get<FacePart>().Id))
                    OutputText("Scenes.Farm.WorkFarm.SensitiveWord".Localized());

                OutputText("Scenes.Farm.WorkFarm.NoseSetToWork".Localized());

                if (Player.CowScore() + Player.MinoScore() > 0)
                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.CowScore() + Player.MinoScore())));

                OutputText("Scenes.Farm.WorkFarm.HourLaterExitBarn".Localized());

                OutputText("Scenes.Farm.WorkFarm.HeresYourPayment".Localized());

                OutputText("Scenes.Farm.WorkFarm.PromiseToComeBack".Localized());

                if ((100 * Player.Strength / Player.GetMaxStats("str")) <= 25 || Random.Shared.Next(2) == 0)
                    Player.ApplyStats(new DynStat(GameStats.Strength, 1f));

                if ((100 * Player.Toughness / Player.GetMaxStats("tou")) <= 25 || Random.Shared.Next(2) == 0)
                    Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));

                Player.Gems += 5;

                Player.ChangeFatigue(20);

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Farm.WorkFarm.AskCouldUseHelp".Localized());

            OutputText("Scenes.Farm.WorkFarm.BorrowBasket".Localized());

            if ((100 * Player.Speed / Player.GetMaxStats("spe")) <= 25 || Random.Shared.Next(2) == 0)
                Player.ApplyStats(new DynStat(GameStats.Speed, 1f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) <= 25 || Random.Shared.Next(2) == 0)
                Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));

            Player.ChangeFatigue(20);

            float pepper = Random.Shared.Next(4);

            Identifier? itype = null;

            if (pepper <= 2)
                itype = ConsumableIds.CaninePepper;
            else
                itype = RandomUtils.Choice(

                ConsumableIds.LargeCaninePepper,

                ConsumableIds.DoubleCaninePepper,

                ConsumableIds.BlackCaninePepper,

                ConsumableIds.KnottyCaninePepper,

                ConsumableIds.BulbyCaninePepper,

                ConsumableIds.WOLF_PP
            );

            if (itype != null) InventoryScene.TakeItem(itype.Value, () => ReturnToCamp(1));
        }

        public void MeetMarble()
        {

            float marbling = 0;

            if (SceneInstance<MarbleScene>.Get().KnowAddiction == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MurbleFarmTalkLevels) >= 7)
                    SceneInstance<MarbleScene>.Get().EncounterMarbleExploring();
                else
                    SceneInstance<MarbleScene>.Get().EncounterMarbleExploring2();
            }
            else
            {
                if (Player.HasPerk(PerkIds.MarbleResistant))
                {
                    SceneInstance<MarbleScene>.Get().PostAddictionFarmExplorings();

                    return ;
                }

                if (SceneInstance<MarbleScene>.Get().KnowAddiction == 1)
                {
                    if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                        marbling = 0;
                    else
                        marbling = 1;

                    if (marbling == 0)
                        SceneInstance<MarbleScene>.Get().AddictedEncounterHappy();
                    else
                        SceneInstance<MarbleScene>.Get().MarbleEncounterAddictedNonWithdrawl();
                }
                else
                {
                    if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                        marbling = 0;
                    else
                        marbling = 1;

                    if (marbling == 0)
                        SceneInstance<MarbleScene>.Get().EncounterMarbleAshamedAddiction();
                    else
                        SceneInstance<MarbleScene>.Get().MarbleEncounterAddictedNonWithdrawlAshamed();
                }
            }
        }

        public void ExploreFarm()
        {
            ClearOutput();

            float explore = 0;

            if (Player.HasStatusEffect(StatusEffectIds.MarbleRapeAttempted) && !Player.HasStatusEffect(StatusEffectIds.NoMoreMarble))
            {
                SceneInstance<MarbleScene>.Get().MarbleAfterRapeBattle();

                Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);

                return ;
            }
            // 原版 Farm.hx:691 — 找胡萝卜（Nieve 剧情 1-3 阶段）
            if (SceneInstance<Seasonal.NieveScene>.Get().NieveAvailable() && CoC.Instance.Flags.Get(GameFlagIds.NieveStage) > 0 && CoC.Instance.Flags.Get(GameFlagIds.NieveStage) < 4 && !Player.HasKeyItem("Carrot"))
                return ;

            if (Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.FoundIsabellaAtFarmToday) == 0 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkedYet) < 0 && false && CoC.Instance.Flags.Get(GameFlagIds.IsabellaMilkCooldown) == 0 && Random.Shared.Next(2) == 0)

                return ;

            if (!Player.HasStatusEffect(StatusEffectIds.Marble) && !Player.HasStatusEffect(StatusEffectIds.NoMoreMarble))
            {
                DoNext(() => ReturnToCamp(1));

                SceneInstance<MarbleScene>.Get().EncounterMarbleInitially();

                return ;
            }

            if (Random.Shared.Next(2) == 0 && !Player.HasStatusEffect(StatusEffectIds.Kelt) && !Player.HasStatusEffect(StatusEffectIds.KeltOff))
            {
                DoNext(() => ReturnToCamp(1));

                SceneInstance<KeltScene>.Get().KeltEncounter();

                return ;
            }

            if (Random.Shared.Next(3) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmStables) == 0 && Player.HasStatusEffect(StatusEffectIds.Kelt))
            {
                FindStables();

                return ;
            }

            if (!Player.HasStatusEffect(StatusEffectIds.NoMoreMarble) && Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                if (SceneInstance<MarbleScene>.Get().KnowAddiction == 1)
                    SceneInstance<MarbleScene>.Get().AddictedEncounterHappy();
                else
                    SceneInstance<MarbleScene>.Get().EncounterMarbleAshamedAddiction();

                return ;
            }

            explore = Random.Shared.Next(3);

            if (explore == 0)
            {
                if (View.Time.IsDay)
                {
                    OutputText("Scenes.Farm.ExploreFarm.RunAroundFarm".Localized());

                    if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 30)
                    {
                        Player.ApplyStats(new DynStat(GameStats.Speed, 2f));

                        OutputText("Scenes.Farm.ExploreFarm.WhitneyOutpaces".Localized());
                    }
                    else
                        if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 50)
                        {
                            Player.ApplyStats(new DynStat(GameStats.Speed, 1f));

                            OutputText("Scenes.Farm.ExploreFarm.WhitneyStillFaster".Localized());
                        }
                        else
                            if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 70)
                            {
                                Player.ApplyStats(new DynStat(GameStats.Speed, 0.75f));

                                OutputText("Scenes.Farm.ExploreFarm.EvenlyMatched".Localized());
                            }
                            else
                            {
                                Player.ApplyStats(new DynStat(GameStats.Speed, 0.5f));

                                OutputText("Scenes.Farm.ExploreFarm.WhitneyFallsBehind".Localized());
                            }

                    OutputText("Scenes.Farm.ExploreFarm.LieBackPanting".Localized());
                }
                else
                {
                    OutputText("Scenes.Farm.ExploreFarm.EmbracingOpenAir".Localized(
                        Player.HasStatusEffect(StatusEffectIds.Kelt) && CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) == 0 && CoC.Instance.Flags.Get(GameFlagIds.KeltKilled) == 0
                    ));

                    Player.ApplyStats(new DynStat(GameStats.Speed, .1f));
                }

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (explore == 1)
            {
                if (View.Time.IsDay)
                    OutputText("Scenes.Farm.ExploreFarm.WanderAtopRiseDay".Localized());
                else
                    OutputText("Scenes.Farm.ExploreFarm.WanderAtopRiseNight".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, -Random.Shared.Next(3)));

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (explore == 2)
            {
                if (View.Time.IsDay)
                {
                    OutputText("Scenes.Farm.ExploreFarm.CowPasture".Localized());

                    if (Player.CountCocksOfType("Horse") > 0)
                    {
                        OutputText("Scenes.Farm.ExploreFarm.UnbiddenNotion".Localized());

                        if (Player.Corruption > 60)
                            OutputText("Scenes.Farm.ExploreFarm.ItMakesYouSmile".Localized());

                        if (Player.Corruption < 30)
                            OutputText("Scenes.Farm.ExploreFarm.ItDisgustsYou".Localized());
                        else
                            if (Player.Corruption <= 60)
                                OutputText("Scenes.Farm.ExploreFarm.NotSureHowFeel".Localized());

                        OutputText("Scenes.Farm.ExploreFarm.WouldGetPitchfork".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                    }
                    else
                        if (Player.InHeat)
                        {
                            OutputText("Scenes.Farm.ExploreFarm.MaybeWantsABull".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                        }
                        else
                            if (Player.BiggestLactation() >= 2)
                            {
                                OutputText("Scenes.Farm.ExploreFarm.MaybeSheWantsMilked".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, 3f));
                            }

                    OutputText("Scenes.Farm.ExploreFarm.ClearYourThoughts".Localized());
                }
                else
                    OutputText("Scenes.Farm.ExploreFarm.SleepingCows".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (explore != 3)
            {
                OutputText("Scenes.Farm.ExploreFarm.NothingEntertaining".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void GetMilked()
        {
            ClearOutput();

            OutputText("Scenes.Farm.GetMilked.BarnLoomsTall".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.BreastsMilked))
            {
                if (Player.Corruption < 50)
                    OutputText("Scenes.Farm.GetMilked.ShiverNervously".Localized());
                else
                    OutputText("Scenes.Farm.GetMilked.SmileEagerly".Localized());

                OutputText("Scenes.Farm.GetMilked.BarnFilledSmells".Localized());
            }
            else
            {
                OutputText("Scenes.Farm.GetMilked.WalkOverToBarn".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.LactationReduction) && Player.Cocks.Count == 0)
                    OutputText("Scenes.Farm.GetMilked.NipplesEngorged".Localized());
                else
                    if (Player.Cocks.Count > 0)
                    {
                        OutputText("Scenes.Farm.GetMilked.CocksErect".Localized());

                        if (Player.Cocks.Count > 1)
                            OutputText("Scenes.Farm.GetMilked.PluralSErect".Localized());

                        OutputText("Scenes.Farm.GetMilked.AndThrob".Localized());

                        if (Player.Cocks.Count == 1)
                            OutputText("Scenes.Farm.GetMilked.PluralSThrob".Localized());

                        OutputText("Scenes.Farm.GetMilked.WithDesire".Localized());
                    }

                if (Player.HasStatusEffect(StatusEffectIds.LactationReduction) && Player.CockTotal() > 0)
                {
                    OutputText("Scenes.Farm.GetMilked.NipplesAndCocksGrow".Localized());

                    OutputText("Scenes.Farm.GetMilked.HardAndReadyOf".Localized());

                    OutputText("Scenes.Farm.GetMilked.TheirWord".Localized());

                    OutputText("Scenes.Farm.GetMilked.OwnVolition".Localized());
                }

                OutputText("Scenes.Farm.GetMilked.DoorsPartEasily".Localized());
            }

            if (!Player.HasStatusEffect(StatusEffectIds.BreastsMilked))
                OutputText("Scenes.Farm.GetMilked.HarnessHangsLimply".Localized());
            else
                OutputText("Scenes.Farm.GetMilked.AttachHarnesses".Localized());

            float application = Random.Shared.Next(3);

            if (Player.NippleLength == 3 && Random.Shared.Next(2) == 0)
                application = 3;

            if (!Player.HasStatusEffect(StatusEffectIds.BreastsMilked) || application == 0)
            {
                if (!Player.HasStatusEffect(StatusEffectIds.BreastsMilked))
                    Player.CreateStatusEffect(StatusEffectIds.BreastsMilked, 0, 0, 0, 0);

                OutputText("Scenes.Farm.GetMilked.GrabSuctionCups".Localized());

                if (Player.NippleLength <= 1.5f)
                    OutputText("Scenes.Farm.GetMilked.PullingNipplesIn".Localized());
                else
                    OutputText("Scenes.Farm.GetMilked.StrugglingToFit".Localized());

                OutputText("Scenes.Farm.GetMilked.MechanicalLurching".Localized(
                    (int)(Player.NippleLength * 1.5f * 10) / 10,

                    Appearance.AllBreastsDescript(Player)
                ));
            }
            else
                if (application == 1)
                {
                    OutputText("Scenes.Farm.GetMilked.StretchDownGrab".Localized());

                    if (Player.NippleLength >= 3)
                        OutputText("Scenes.Farm.GetMilked.NearlyBurstTubes".Localized());

                    OutputText("Scenes.Farm.GetMilked.SensitiveFleshPressure".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                }
                else
                    if (application == 2)
                        OutputText("Scenes.Farm.GetMilked.DespiteTightnessCups".Localized(
                            Player.NippleDescript(0)
                        ));

            if (application == 3)
            {
                OutputText("Scenes.Farm.GetMilked.InSpiteTightnessHuge".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
            }

            float milksplosion = Random.Shared.Next(3);

            if (Player.LactationQ() < 50)
            {
                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

                if (milksplosion == 0)
                {
                    OutputText("Scenes.Farm.GetMilked.FewDropsBud".Localized());

                    if (Player.BreastRows.Count() == 2)
                        OutputText("Scenes.Farm.GetMilked.PairWord".Localized());
                    else
                        if (Player.BreastRows.Count() == 4)
                            OutputText("Scenes.Farm.GetMilked.QuartetWord".Localized());
                        else
                            OutputText("Scenes.Farm.GetMilked.GroupWord".Localized());

                    OutputText("Scenes.Farm.GetMilked.InternalFloodgates".Localized());
                }
                else
                    if (milksplosion == 1)
                        OutputText("Scenes.Farm.GetMilked.TinySpurtErupts".Localized(
                            Player.NippleDescript(0),

                            Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        if (milksplosion == 2)
                            OutputText("Scenes.Farm.GetMilked.TipsSwell".Localized());
            }
            else
                if (Player.LactationQ() < 250)
                {
                    Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

                    if (milksplosion == 0)
                        OutputText("Scenes.Farm.GetMilked.DropsRollDown".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    if (milksplosion == 1)
                        OutputText("Scenes.Farm.GetMilked.TightStreamErupts".Localized());

                    if (milksplosion == 2)
                    {
                        OutputText("Scenes.Farm.GetMilked.FatDropsPour".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                        if (Player.Cocks.Count == 0)
                        {
                            if (Player.HasVagina())
                                OutputText("Scenes.Farm.GetMilked.WetWord".Localized());
                            else
                                OutputText("Scenes.Farm.GetMilked.HornyWord".Localized());
                        }
                        else
                        {
                            if (Player.HasVagina())
                                OutputText("Scenes.Farm.GetMilked.WetAndWord".Localized());

                            OutputText("Scenes.Farm.GetMilked.HardWord".Localized());
                        }

                        OutputText("Scenes.Farm.GetMilked.FromTheSensation".Localized());
                    }
                }
                else
                    if (Player.LactationQ() < 750)
                    {
                        Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

                        if (milksplosion == 0)
                            OutputText("Scenes.Farm.GetMilked.EruptionFloodsTubes".Localized());

                        if (milksplosion == 1)
                        {
                            OutputText("Scenes.Farm.GetMilked.SwellLikeBalloons".Localized());

                            if (Player.Cocks.Count == 0)
                            {
                                if (Player.HasVagina())
                                    OutputText("Scenes.Farm.GetMilked.WetWordAgain".Localized());
                                else
                                    OutputText("Scenes.Farm.GetMilked.HornyWordAgain".Localized());
                            }
                            else
                            {
                                if (Player.HasVagina())
                                    OutputText("Scenes.Farm.GetMilked.WetAndWordAgain".Localized());

                                OutputText("Scenes.Farm.GetMilked.HardWordAgain".Localized());
                            }

                            OutputText("Scenes.Farm.GetMilked.WithExcitement".Localized());
                        }

                        if (milksplosion == 2)
                            OutputText("Scenes.Farm.GetMilked.MilkFloodsCups".Localized());
                    }
                    else
                    {
                        Player.ApplyStats(new DynStat(GameStats.Lust, 60f));

                        milksplosion = Random.Shared.Next(2);

                        if (milksplosion == 0)
                        {
                            OutputText("Scenes.Farm.GetMilked.TwitchAndPulse".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));

                            if (Player.HasVagina())
                                OutputText("Scenes.Farm.GetMilked.CreamYourself".Localized());
                            else
                                if (Player.HasCock())
                                    OutputText("Scenes.Farm.GetMilked.JizzYourself".Localized());
                                else
                                    OutputText("Scenes.Farm.GetMilked.OrgasmWord".Localized());

                            OutputText("Scenes.Farm.GetMilked.FromTheAttentions".Localized());
                        }
                        else
                            OutputText("Scenes.Farm.GetMilked.BodyLetsDownMilk".Localized());
                    }

            int liters = 0;

            int payout = 0;

            int cap = 500;

            if (easyMode)
                cap *= 2;

            if (debug)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyGemsPaidThisWeek, 0);

                cap = 9999;
            }

            liters = (int)(Player.LactationQ() * (Random.Shared.Next(10) + 90) / 100 / 1000);

            if (liters < 0)
                liters = 1337;

            payout = liters * 2 * 4;

            OutputText("Scenes.Farm.GetMilked.MachineryDisplaysLiters".Localized(
                liters
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) >= cap)
            {
                OutputText("Scenes.Farm.GetMilked.MoreThanWhitneyPay".Localized());

                payout = 0;
            }

            if (payout > 0)
            {
                if (payout + CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) > cap)
                    payout = cap - CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek);

                CoC.Instance.Flags.Set(GameFlagIds.WhitneyGemsPaidThisWeek, CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) + payout);

                OutputText("Scenes.Farm.GetMilked.DispensesGems".Localized(NumberUtils.Num2Text(payout), (payout == 1 ? "" : "s")));

                if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) >= cap)
                    OutputText("Scenes.Farm.GetMilked.CantAffordMore".Localized());

                Player.Gems += payout;
            }
            else
                OutputText("Scenes.Farm.GetMilked.SentencePeriod".Localized());

            if (liters > 2)
            {
                OutputText("Scenes.Farm.GetMilked.WoozyLightheaded".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                if (Player.HasStatusEffect(StatusEffectIds.Feeder))
                {
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

                    if (liters > 5)
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

                    if (liters > 10)
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

                    if (liters > 20)
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));
                }
                else
                {
                    if (liters / 2 > 10)
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -10f));
                    else
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -liters / 2));

                    if (liters > 30)
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f));
                }

                if (Player.Intelligence < 10)
                {
                    DoNext(() => CowBadEnd1());

                    return ;
                }
                else
                    if (Player.Intelligence < 15)
                        OutputText("Scenes.Farm.GetMilked.ContentedMoo".Localized());
                    else
                        if (Player.Intelligence < 25)
                            OutputText("Scenes.Farm.GetMilked.QuietlyMoo".Localized());
            }

            OutputText("Scenes.Farm.GetMilked.ParagraphBreak".Localized());

            if (Player.LustPercent < 75)
                OutputText("Scenes.Farm.GetMilked.SoreVeryHungry".Localized());
            else
            {
                OutputText("Scenes.Farm.GetMilked.OverwhelmedDesire".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                if (FarmCorruption.whitneyCorruption() < 90)
                    OutputText("Scenes.Farm.GetMilked.DisapprovingLook".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
            }

            Player.BoostLactation(.05f);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            if (Player.StatusEffectValue(StatusEffectIds.LactationEndurance, 0) < 1.5f)
                Player.ChangeStatusValue(StatusEffectIds.LactationEndurance, 0, .05f);

            Player.ChangeStatusValue(StatusEffectIds.LactationEndurance, 0, .05f);

            Player.CreateStatusEffect(StatusEffectIds.Milked, 8, 0, 0, 0);

            Player.Orgasm("Tits", false);

            DoNext(() => ReturnToCamp(1));
        }

        public void CockPumping()
        {
            ClearOutput();

            var cumQ = Player.CumQ() * (Random.Shared.Next(10) + 90) / 100;

            if (!Player.HasStatusEffect(StatusEffectIds.CockPumped))
            {
                OutputText("Scenes.Farm.CockPumping.BarnLoomsTall".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Farm.CockPumping.ShiverNervously".Localized());
                else
                    OutputText("Scenes.Farm.CockPumping.SmileEagerly".Localized());

                OutputText("Scenes.Farm.CockPumping.BarnFilledSmells".Localized());
            }
            else
            {
                OutputText("Scenes.Farm.CockPumping.WalkOverToBarn".Localized());

                OutputText("Scenes.Farm.CockPumping.CocksErect".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.PluralSErect".Localized());

                OutputText("Scenes.Farm.CockPumping.AndThrob".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.PluralSThrob".Localized());

                OutputText("Scenes.Farm.CockPumping.WithBuildingDesire".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.ItWord".Localized());
                else
                    OutputText("Scenes.Farm.CockPumping.TheyWord".Localized());

                OutputText("Scenes.Farm.CockPumping.KnowWord".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.PluralSKnow".Localized());

                OutputText("Scenes.Farm.CockPumping.AllTooWell".Localized());
            }

            if (!Player.HasStatusEffect(StatusEffectIds.CockPumped))
            {
                OutputText("Scenes.Farm.CockPumping.NoteTackedToDoor".Localized());

                OutputText("Scenes.Farm.CockPumping.MachineRiggedUp".Localized());

                OutputText("Scenes.Farm.CockPumping.HarnessHangsLimply".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.SingleHoseWriggles".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Farm.CockPumping.PairOfHoses".Localized());
                    else
                        OutputText("Scenes.Farm.CockPumping.GroupOfHoses".Localized());

                Player.CreateStatusEffect(StatusEffectIds.CockPumped, 1, 0, 0, 0);
            }
            else
            {
                OutputText("Scenes.Farm.CockPumping.AttachHarnesses".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.HoseWrigglesOut".Localized());
                else
                    OutputText("Scenes.Farm.CockPumping.HosesWriggleOut".Localized());

                Player.ChangeStatusValue(StatusEffectIds.CockPumped, 0, 1);
            }

            if (Player.CockArea(0) < 20)
            {
                if (Random.Shared.Next(2) == 0)
                {
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Farm.CockPumping.HoseTipSingle".Localized());
                    else
                        OutputText("Scenes.Farm.CockPumping.HoseTipsMultiple".Localized());
                }
                else
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Farm.CockPumping.HoseOpeningSingle".Localized(
                            Player.CockDescript(0)
                        ));
                    else
                        OutputText("Scenes.Farm.CockPumping.HoseOpeningsMultiple".Localized(
                            Player.MultiCockDescript()
                        ));
            }
            else
                if (Player.CockArea(0) < 70)
                {
                    if (Random.Shared.Next(2) == 0)
                    {
                        if (Player.CockTotal() == 1)
                        {
                            OutputText("Scenes.Farm.CockPumping.SnakeReceptacleYawns".Localized(
                                Player.CockHead()
                            ));

                            if (Player.HasKnot(0))
                                OutputText("Scenes.Farm.CockPumping.PastYourKnot".Localized());
                            else
                                OutputText("Scenes.Farm.CockPumping.ToYourBasePlural".Localized());

                            OutputText("Scenes.Farm.CockPumping.MassagingProtrusions".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Farm.CockPumping.ReceptaclesYawn".Localized(
                                Player.CockHead()
                            ));

                            OutputText("Scenes.Farm.CockPumping.ToYourBasePlural".Localized());

                            OutputText("Scenes.Farm.CockPumping.MassageProtrusions".Localized());
                        }
                    }
                    else
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Farm.CockPumping.MilkerOpeningSingle".Localized(
                                Player.CockDescript(0),

                                Player.CockHead()
                            ));
                        else
                            OutputText("Scenes.Farm.CockPumping.MilkerOpeningsMultiple".Localized(
                                Player.MultiCockDescriptLight(),

                                Player.CockHead()
                            ));
                }
                else
                    if (Random.Shared.Next(2) == 0)
                    {
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Farm.CockPumping.HoseHoleSpreads".Localized(
                                Player.CockDescript(0),

                                Player.CockHead()
                            ));
                        else
                            OutputText("Scenes.Farm.CockPumping.HosesHolesSpread".Localized(
                                Player.CockHead(),

                                Player.MultiCockDescript()
                            ));
                    }
                    else
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Farm.CockPumping.MilkerOpeningDilates".Localized(
                                Player.CockHead()
                            ));
                        else
                            OutputText("Scenes.Farm.CockPumping.MilkerOpeningsDilate".Localized(
                                Player.MultiCockDescriptLight()
                            ));

            if (Random.Shared.Next(2) == 0)
                OutputText("Scenes.Farm.CockPumping.SquirmAgainstRestraints".Localized());
            else
                OutputText("Scenes.Farm.CockPumping.MoanWithLust".Localized());

            if (Player.HasKeyItem("Cock Milker: Anal Attachment"))
            {
                Player.Orgasm("Anal");

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.AnalProbeSingle".Localized());
                else
                    OutputText("Scenes.Farm.CockPumping.AnalProbePlural".Localized());
            }
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.KeptOnEdgeSingle".Localized());
                else
                    OutputText("Scenes.Farm.CockPumping.KeptOnEdgePlural".Localized());

            if (Player.CumQ() >= 50 && Player.Fatigue >= Player.MaxFatigue() && CoC.Instance.Flags.Get(GameFlagIds.UsedMilkerToday) > 0)
            {
                if (cumQ < 1000)
                {
                    OutputText("Scenes.Farm.CockPumping.OrgasmRollsSingle".Localized());

                    if (cumQ > 500)
                        OutputText("Scenes.Farm.CockPumping.ThickWord".Localized());

                    OutputText("Scenes.Farm.CockPumping.SpurtsOfSpunk".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Farm.CockPumping.PluralSCocks".Localized());

                    OutputText("Scenes.Farm.CockPumping.SentencePeriod".Localized());

                    OutputText("Scenes.Farm.CockPumping.LoudDingSounds".Localized());

                    if (cumQ > 100)
                        OutputText("Scenes.Farm.CockPumping.ClatterOfGems".Localized());

                    OutputText("Scenes.Farm.CockPumping.AsMachineMeasures".Localized(
                        Player.MultiCockDescriptLight()
                    ));

                    if (Player.BiggestCockArea() > 70)
                    {
                        OutputText("Scenes.Farm.CockPumping.TheEndWord".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Farm.CockPumping.PluralSTheEnd".Localized());

                        OutputText("Scenes.Farm.CockPumping.WrappedSoSnugly".Localized());

                        OutputText("Scenes.Farm.CockPumping.PluralSMembers".Localized());

                        OutputText("Scenes.Farm.CockPumping.AndWord".Localized());
                    }

                    OutputText("Scenes.Farm.CockPumping.HowTiredYouAre".Localized());
                }
                else
                {
                    OutputText("Scenes.Farm.CockPumping.EruptionOfJism".Localized(
                        Player.MultiCockDescriptLight()
                    ));

                    OutputText("Scenes.Farm.CockPumping.JismSplattersFloor".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Farm.CockPumping.PluralSTubes".Localized());

                    OutputText("Scenes.Farm.CockPumping.WrigglingBackIntoPlace".Localized());

                    OutputText("Scenes.Farm.CockPumping.LoudDingAndGems".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Farm.CockPumping.PluralSTubesAgain".Localized());

                    OutputText("Scenes.Farm.CockPumping.TryToPullMilker".Localized());

                    if (Player.BiggestCockArea() > 70)
                    {
                        OutputText("Scenes.Farm.CockPumping.AndTheEnd".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Farm.CockPumping.PluralSEnd".Localized());

                        OutputText("Scenes.Farm.CockPumping.WrapTooSnugly".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Farm.CockPumping.PluralSSwollenMembers".Localized());
                    }

                    OutputText("Scenes.Farm.CockPumping.EveryTimeGoodGrip".Localized(
                        Player.SMultiCockDesc()
                    ));
                }

                DoNext(() => MilkerBadEnd1());

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.UsedMilkerToday, CoC.Instance.Flags.Get(GameFlagIds.UsedMilkerToday) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.PcCockMilkedCounter, CoC.Instance.Flags.Get(GameFlagIds.PcCockMilkedCounter) + 1);

            if (cumQ < 10)
            {
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.CockPumping.OrgasmRollsTiny".Localized(
                        Player.CockHead()
                    ));
                else
                    OutputText("Scenes.Farm.CockPumping.OrgasmRollsTinyPlural".Localized(
                        Player.CockHead()
                    ));

                Player.ChangeFatigue(5);
            }
            else
                if (cumQ < 100)
                {
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Farm.CockPumping.OrgasmRollsThick".Localized(
                            Player.CockHead()
                        ));
                    else
                        OutputText("Scenes.Farm.CockPumping.OrgasmRollsThickPlural".Localized(
                            Player.CockHead()
                        ));

                    Player.ChangeFatigue(10);
                }
                else
                    if (cumQ < 333)
                    {
                        if (Player.CockTotal() == 1)
                        {
                            OutputText("Scenes.Farm.CockPumping.OrgasmOceanWave".Localized());

                            if (Player.Balls > 0)
                                OutputText("Scenes.Farm.CockPumping.BloatedBallsAre".Localized());
                            else
                                OutputText("Scenes.Farm.CockPumping.SensitiveCockIs".Localized());

                            OutputText("Scenes.Farm.CockPumping.FinallyRelievedOf".Localized());

                            if (Player.Balls > 0)
                                OutputText("Scenes.Farm.CockPumping.TheirWord".Localized());
                            else
                                OutputText("Scenes.Farm.CockPumping.ItsWord".Localized());

                            OutputText("Scenes.Farm.CockPumping.PentUpSeed".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Farm.CockPumping.OrgasmOceanWavePlural".Localized());

                            if (Player.Balls > 0)
                                OutputText("Scenes.Farm.CockPumping.BloatedBallsArePlural".Localized());
                            else
                                OutputText("Scenes.Farm.CockPumping.SensitiveCocksAre".Localized());

                            OutputText("Scenes.Farm.CockPumping.FinallyRelievedOfPlural".Localized());
                        }

                        Player.ChangeFatigue(15);
                    }
                    else
                        if (cumQ < 1000)
                        {
                            if (Player.CockTotal() == 1)
                                OutputText("Scenes.Farm.CockPumping.ForceOfOrgasm".Localized());
                            else
                                OutputText("Scenes.Farm.CockPumping.ForceOfOrgasmPlural".Localized());

                            Player.ChangeFatigue(20);
                        }
                        else
                        {
                            if (Player.CockTotal() == 1)
                            {
                                OutputText("Scenes.Farm.CockPumping.EruptionOfJismMassive".Localized(
                                    Player.CockDescript(0)
                                ));

                                OutputText("Scenes.Farm.CockPumping.HurriedFootsteps".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Farm.CockPumping.JismEruptionPlural".Localized(
                                    Player.MultiCockDescriptLight()
                                ));

                                OutputText("Scenes.Farm.CockPumping.HurriedFootstepsPlural".Localized());
                            }

                            Player.ChangeFatigue(40);
                        }

            OutputText("Scenes.Farm.CockPumping.TheresA".Localized());

            if (Player.CumQ() < 20)
                OutputText("Scenes.Farm.CockPumping.BarelyNoticeable".Localized());
            else
                OutputText("Scenes.Farm.CockPumping.ThickWordAgain".Localized());

            OutputText("Scenes.Farm.CockPumping.TrailOfCum".Localized());

            int payout = 0;

            int cap = 500;

            if (easyMode)
                cap *= 2;

            if (debug)
            {
                CoC.Instance.Flags.Set(GameFlagIds.WhitneyGemsPaidThisWeek, 0);

                cap = 9999;
            }

            cumQ = (int)(cumQ);

            if (cumQ > 100)
                OutputText("Scenes.Farm.CockPumping.LitersLs".Localized(
                    cumQ / 1000
                ));
            else
                OutputText("Scenes.Farm.CockPumping.MillilitersMls".Localized(
                    cumQ
                ));

            if (cumQ > 100)
                payout = 2 + (int)(cumQ / 200) * 2;

            if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) >= cap)
            {
                payout = 0;

                OutputText("Scenes.Farm.CockPumping.FundsExhausted".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) + payout >= cap)
                {
                    payout = cap - CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek);

                    OutputText("Scenes.Farm.CockPumping.NotEnoughForFull".Localized());
                }

            if (payout > 0)
            {
                if (Player.CumQ() < 1000)
                    Player.CumMultiplier = 1;

                if (payout == 1)
                    OutputText("Scenes.Farm.CockPumping.GemRolls".Localized(
                        NumberUtils.Num2Text(payout)
                    ));
                else
                    OutputText("Scenes.Farm.CockPumping.GemsRoll".Localized(
                        NumberUtils.Num2Text(payout)
                    ));

                OutputText("Scenes.Farm.CockPumping.OutIntoCollectionPlate".Localized());

                Player.Gems += payout;

                CoC.Instance.Flags.Set(GameFlagIds.WhitneyGemsPaidThisWeek, CoC.Instance.Flags.Get(GameFlagIds.WhitneyGemsPaidThisWeek) + payout);

            }

            if (Player.CountCockSocks("gilded") > 0)
            {
                var gems = Random.Shared.Next(2) + Player.CountCockSocks("gilded");

                if (Player.CumQ() > 1000)
                    gems = (int)(gems * 1.5f);

                if (payout > 0)
                    OutputText("Scenes.Farm.CockPumping.AsYouTakePayment".Localized());
                else
                    OutputText("Scenes.Farm.CockPumping.BoldYouStart".Localized());

                OutputText("Scenes.Farm.CockPumping.OuSeeSparklingGems".Localized(
                    gems
                ));

                Player.Gems += gems;
            }
            else
                OutputText("Scenes.Farm.CockPumping.YouGoWord".Localized());

            OutputText("Scenes.Farm.CockPumping.OnYourWayWhistling".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void CowBadEnd1()
        {

            OutputText("Scenes.Farm.CowBadEnd1.MooWithDelight".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Farm.CowBadEnd1.StopTryingToRemember".Localized());

            OutputText("Scenes.Farm.CowBadEnd1.FindingSomeoneToFuck".Localized());

            DoNext(() => CowBadEnd2());
        }

        public void CowBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Farm.CowBadEnd2.SloshBackIntoBarn".Localized());

            OutputText("Scenes.Farm.CowBadEnd2.MooHappily".Localized());

            OutputText("Scenes.Farm.CowBadEnd2.CountryGirlPats".Localized((noFur ? "" : " anthropomorphic")));

            OutputText("Scenes.Farm.CowBadEnd2.NodHappily".Localized());

            GameOver();
        }

        public void MilkerBadEnd1()
        {
            ClearOutput();

            var cumQ = Player.CumQ();

            OutputText("Scenes.Farm.MilkerBadEnd1.RollOntoYourBack".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Farm.MilkerBadEnd1.InYourBalls".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.AsBeforeMachineWorks".Localized());

            if (cumQ < 100)
                OutputText("Scenes.Farm.MilkerBadEnd1.TrickleWord".Localized());
            else
                if (cumQ < 600)
                    OutputText("Scenes.Farm.MilkerBadEnd1.StreamWord".Localized());
                else
                    OutputText("Scenes.Farm.MilkerBadEnd1.TorrentWord".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.OfJism".Localized());

            if (cumQ > 1000)
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.FleshyValveFlap".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Farm.MilkerBadEnd1.PluralSValves".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.EjaculationPressure".Localized());
            }

            OutputText("Scenes.Farm.MilkerBadEnd1.OrgasmWindsDown".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.AnotherDing".Localized());

            if (cumQ > 100)
                OutputText("Scenes.Farm.MilkerBadEnd1.AnotherClatterOfGems".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.ReachesYourEars".Localized());

            var tubes = false;

            if (Player.HasVagina() || Player.HasFuckableNipples() || Player.BiggestLactation() > 1)
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.PluralSTubes".Localized());

                tubes = true;
            }

            OutputText("Scenes.Farm.MilkerBadEnd1.SnakingOutOfWall".Localized());

            if (!tubes)
                OutputText("Scenes.Farm.MilkerBadEnd1.ItWord".Localized());
            else
                OutputText("Scenes.Farm.MilkerBadEnd1.OneOfThem".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.InsertsRodIntoMouth".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.AnotherTubeVagina".Localized(
                    Player.VaginaDescript(0)
                ));

                Player.CuntChange(15, true);

                if (Player.GetClitLength() > 3)
                    OutputText("Scenes.Farm.MilkerBadEnd1.SmallerTubeOverClit".Localized());
            }

            if (Player.BiggestLactation() >= 1 && !Player.HasFuckableNipples())
                OutputText("Scenes.Farm.MilkerBadEnd1.CupTubesOnNipples".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Farm.MilkerBadEnd1.MoreTubesIntoNipples".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));

            OutputText("Scenes.Farm.MilkerBadEnd1.BecomesEnormouslyHard".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Farm.MilkerBadEnd1.UnderThisNewDrug".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Farm.MilkerBadEnd1.SackStretches".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.AnotherOrgasmPasses".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Farm.MilkerBadEnd1.PluralSCocks".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.ThanksToTheDrug".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Farm.MilkerBadEnd1.NewVolumeOfTesticles".Localized());

            if (cumQ > 1000)
                OutputText("Scenes.Farm.MilkerBadEnd1.SprayingOverflowValve".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.SentencePeriod".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.CuntSpasmsAround".Localized());

                if (Player.GetClitLength() > 3)
                    OutputText("Scenes.Farm.MilkerBadEnd1.ClitorisTwitches".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.SentencePeriodAgain".Localized());
            }

            if (Player.BiggestBreastSize() > 1)
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.MilkSpraysOut".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Farm.MilkerBadEnd1.RingsRaiseSeal".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.SuctionWhisksAway".Localized());
            }

            OutputText("Scenes.Farm.MilkerBadEnd1.MinuteAfterRelease".Localized());

            if (Player.BiggestLactation() > 1)
                OutputText("Scenes.Farm.MilkerBadEnd1.DrugTastesOfMilk".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.AnotherChorusSounds".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.ForSeveralHours".Localized());

            if (Player.Corruption > 90)
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.HundredsOfOrgasms".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Farm.MilkerBadEnd1.AndWord".Localized(
                        Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Farm.MilkerBadEnd1.ToEdgeAndBack".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Farm.MilkerBadEnd1.PluralSShafts".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.DemonMachineCantStop".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Farm.MilkerBadEnd1.PluralSShaftsAgain".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.AsOrgasmLetsFly".Localized());
            }

            OutputText("Scenes.Farm.MilkerBadEnd1.WhitneyWalksIn".Localized());

            OutputText("Scenes.Farm.MilkerBadEnd1.RancherSaysFuck".Localized());

            if (Player.Corruption < 90)
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.BarelyTakeNotice".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Farm.MilkerBadEnd1.PenisSuffixEs".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.PushingMassiveLoads".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, 20f), new DynStat(GameStats.Sensitivity, 10f), new DynStat(GameStats.Lust, 80f), new DynStat(GameStats.Corruption, 20f));
            }
            else
            {
                OutputText("Scenes.Farm.MilkerBadEnd1.StillFilledResidual".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Farm.MilkerBadEnd1.GiganticPenisBobbing".Localized());
                else
                    OutputText("Scenes.Farm.MilkerBadEnd1.MultiplePenisesBobbing".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.JustInTimeBacks".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Farm.MilkerBadEnd1.DickWord".Localized());
                else
                    OutputText("Scenes.Farm.MilkerBadEnd1.SetOfDicks".Localized());

                OutputText("Scenes.Farm.MilkerBadEnd1.LikeYoursWhyWorry".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, 20f), new DynStat(GameStats.Sensitivity, 10f), new DynStat(GameStats.Lust, 80f), new DynStat(GameStats.Corruption, 100f));
            }

            GameOver();
        }

        public void CentaurToysHoooooo()
        {
            ClearOutput();

            OutputText("Scenes.Farm.CentaurToysHoooooo.FindWhitneyScratching".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.NotMuchHeyNow".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Farm.CentaurToysHoooooo.ShiftAwkwardly".Localized());
            else
                OutputText("Scenes.Farm.CentaurToysHoooooo.StrutALittle".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.ParagraphBreak".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.CentaursRanOff".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.YouShrugSure".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.PersonalBelongings".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.LoadThemUp".Localized());

            OutputText("Scenes.Farm.CentaurToysHoooooo.KeyItemsGained".Localized());

            Player.CreateKeyItem("Fake Mare", 0, 0, 0, 0);

            Player.CreateKeyItem("Centaur Pole", 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public bool GrazingTime()
        {

            var grazingStart = 6;

            var grazingEnd = 11;

            return View.Time.Hours >= grazingStart && View.Time.Hours < grazingEnd;
        }

        public void FindStables()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FarmStables, 1);

            OutputText("Scenes.Farm.FindStables.SpotStables".Localized());

            OutputText("Scenes.Farm.FindStables.StablesSmellMusk".Localized((GrazingTime() ? "just cleanin' the stalls. Horses are out gettin' some fresh air and grass." : "just tendin' the horses. It's important to brush and clean them, and it's a good opportunity to make sure they haven't gone and injured themselves somehow.")));

            OutputText("Scenes.Farm.FindStables.ExplainWandering".Localized());

            OutputText("Scenes.Farm.FindStables.FairEnoughHorses".Localized());

            OutputText("Scenes.Farm.FindStables.LetMeKnowSometime".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void StablesMenu()
        {
            ClearOutput();

            var dildo = !Player.IsTaur() && Player.HasVagina() && (Player.HasKeyItem("Dildo") || Player.HasKeyItem("Deluxe Dildo") || Player.HasKeyItem("Demonic Strap-On"));

            OutputText("Scenes.Farm.StablesMenu.AloneWithHorses".Localized());

            OutputText("Scenes.Farm.StablesMenu.SentimentFadesQuickly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Stallion".Localized(), StablesStallion);

            AddButton(1, "Common.Button.Mare".Localized(), StablesMare);

            AddButton(14, "Common.Button.Back".Localized(), () => StablesLeave());
        }

        public void StablesLeave()
        {
            ClearOutput();

            OutputText("Scenes.Farm.StablesLeave.NothingOfCourse".Localized());

            if (FarmCorrupt())
                FarmCorruption.RootScene();
            else
                DoNext(() => FarmMenu());
        }

        public void StablesStallion()
        {
            ClearOutput();

            OutputText("Scenes.Farm.StablesStallion.EyeUpAStallion".Localized());

            OutputText("Scenes.Farm.StablesStallion.StallionStartled".Localized((metric ? "almost half a meter" : "at least a foot and a half")));

            OutputText("Scenes.Farm.StablesStallion.LickTheTip".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Lick".Localized(), StablesStallionLick);

            AddButton(1, "Common.Button.Throatfuck".Localized(), StablesStallionThroatfuck);

            AddButton(2, "Common.Button.BendOver".Localized(), StablesStallionBendOver);

            AddButton(3, "Common.Button.Sodomize".Localized(), StablesStallionSodomize);
        }

        public void StablesStallionLick()
        {
            ClearOutput();

            OutputText("Scenes.Farm.StablesStallionLick.RelishingTheTaste".Localized());

            OutputText("Scenes.Farm.StablesStallionLick.MoanOfExcitement".Localized());

            OutputText("Scenes.Farm.StablesStallionLick.SomeTimePasses".Localized());

            if (Player.Fatigue >= Player.MaxFatigue() * .75f)
                StablesScandal();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) <= 0)
                    OutputText("Scenes.Farm.StablesStallionLick.GoodThingWhitneyDidnt".Localized());

            OutputText("Scenes.Farm.StablesStallionLick.CleanYourselfOff".Localized());

            Player.Orgasm("Lips", false);

            Player.RefillHunger(20);

            DoNext(() => ReturnToCamp(1));
        }

        public void StablesStallionThroatfuck()
        {
            ClearOutput();

            OutputText("Scenes.Farm.StablesStallionThroatfuck.HardestPartBegins".Localized());

            OutputText("Scenes.Farm.StablesStallionThroatfuck.ShrugOffDecency".Localized());

            OutputText("Scenes.Farm.StablesStallionThroatfuck.FocusYourEfforts".Localized());

            OutputText("Scenes.Farm.StablesStallionThroatfuck.MiracleOfBiology".Localized());

            if (Player.Fatigue >= Player.MaxFatigue() * .75f)
                StablesScandal();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) <= 0)
                    OutputText("Scenes.Farm.StablesStallionThroatfuck.GoodThingWhitneyDidnt".Localized());

            OutputText("Scenes.Farm.StablesStallionThroatfuck.AffectionatePetting".Localized());

            Player.Orgasm("Lips", false);

            Player.RefillHunger(30);

            DoNext(() => ReturnToCamp(1));
        }

        public void StablesStallionBendOver()
        {
            ClearOutput();

            OutputText("Scenes.Farm.StablesStallionBendOver.CeaseYourMinistrations".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.LeanForwardNuzzling".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Farm.StablesStallionBendOver.LiftYourselfUpright".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Farm.StablesStallionBendOver.SlideBackUpright".Localized());
                else
                    if (Player.IsTaur())
                        OutputText("Scenes.Farm.StablesStallionBendOver.HoovesUnderYourself".Localized());
                    else
                        OutputText("Scenes.Farm.StablesStallionBendOver.ScrambleToYourFeet".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.CautiouslyPeeringBack".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.SightOfReadyMale".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.TurnAroundBrace".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Farm.StablesStallionBendOver.TwistBodyJustSo".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Farm.StablesStallionBendOver.SpreadRearLegs".Localized());
                else
                    OutputText("Scenes.Farm.StablesStallionBendOver.SpreadLegsBendOver".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.OfferAccessToSex".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.BeastGruntsFinds".Localized());

            Player.CuntChange(40, true, true);

            OutputText("Scenes.Farm.StablesStallionBendOver.ItIsNotGentle".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.WoundedStrangledNoise".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.SkinDampensSweat".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.ForcefulJab".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Farm.StablesStallionBendOver.ShakingCoils".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Farm.StablesStallionBendOver.ToTheFloor".Localized());
                else
                    OutputText("Scenes.Farm.StablesStallionBendOver.InsidesOfPartedLegs".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.PainDrownedByPleasure".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.PantingPushBack".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.MumblingEncouragements".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.MatesFlareExpands".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.HorseGetYouPregnant".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.SatisfiedSnortDismounts".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.ShuddersSubside".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Farm.StablesStallionBendOver.TeeteringOnCoils".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Farm.StablesStallionBendOver.TremblingWord".Localized());
                else
                    OutputText("Scenes.Farm.StablesStallionBendOver.TeeteringOnLegs".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.NeverMindGooeyMess".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.SomeTimePasses".Localized());

            if (Player.Fatigue >= Player.MaxFatigue() * .75f)
                StablesScandal();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) <= 0)
                    OutputText("Scenes.Farm.StablesStallionBendOver.GoodThingWhitneyDidnt".Localized());

            OutputText("Scenes.Farm.StablesStallionBendOver.CleanUpAndRedress".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void StablesStallionSodomize()
        {
            ClearOutput();

            OutputText("Scenes.Farm.StablesStallionSodomize.HoweverIntoxicating".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.MeetEquineBackside".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.ForcingOutAWhine".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.SuddenEmbrace".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.ExperienceHeightened".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.OneTooManyProds".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.StallionSquirms".Localized());

            OutputText("Scenes.Farm.StablesStallionSodomize.TakeMomentToCompose".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void StablesMare()
        {
            ClearOutput();

            var strapon = Player.HasKeyItem("Demonic Strap-On");

            OutputText("Scenes.Farm.StablesMare.EyeUpAMare".Localized());

            OutputText("Scenes.Farm.StablesMare.PettingShortCoat".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Farm.StablesMare.NoNeedToDawdle".Localized());
            else
                if (strapon)
                    OutputText("Scenes.Farm.StablesMare.NoNeedStrapOn".Localized());
                else
                    OutputText("Scenes.Farm.StablesMare.FigureOutBestWay".Localized());

            OutputText("Scenes.Farm.StablesMare.MuskyScent".Localized());

            OutputText("Scenes.Farm.StablesMare.LiningUpThrust".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Farm.StablesMare.SoftGentleFlexible".Localized());
            else
                OutputText("Scenes.Farm.StablesMare.WellLubricatedFlexible".Localized());

            OutputText("Scenes.Farm.StablesMare.MarePartnerShudders".Localized());

            OutputText("Scenes.Farm.StablesMare.PlantYourselfFirmly".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Farm.StablesMare.MilkingItToBest".Localized(
                    Player.LowMedHighCum("squirts", "ounces", Player.LowMedHighCum("pints", "quarts", "gallons", 1500, 5000), 100, 500)
                ));
            else
                OutputText("Scenes.Farm.StablesMare.ToyTryingToMilk".Localized());

            OutputText("Scenes.Farm.StablesMare.StumbleBackBreath".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Farm.StablesMare.WantHorsesBred".Localized());
            else
                OutputText("Scenes.Farm.StablesMare.AppreciateSpendingTime".Localized());

            if (Player.Fatigue >= Player.MaxFatigue() * .75f)
                StablesScandal();

            OutputText("Scenes.Farm.StablesMare.DonArmorAndPat".Localized());

            Player.Orgasm(Player.HasCock() ? "Dick" : "Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void StablesScandal()
        {

            OutputText("Scenes.Farm.StablesScandal.ThoughPleasedOvertaken".Localized());

            OutputText("Scenes.Farm.StablesScandal.SerenityBroken".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) != 0)
            {
                if (FarmCorruption.whitneyCorrupt())
                {
                    OutputText("Scenes.Farm.StablesScandal.DidntTellMeHorses".Localized());

                    OutputText("Scenes.Farm.StablesScandal.BitchWalksBackOut".Localized());
                }
                else
                    if (Save.GetBool("pureWhitneyStables"))
                        OutputText("Scenes.Farm.StablesScandal.GlanceTowardWhitney".Localized());
                    else
                    {
                        OutputText("Scenes.Farm.StablesScandal.WhitneyGroans".Localized());

                        OutputText("Scenes.Farm.StablesScandal.InYourDefense".Localized());

                        OutputText("Scenes.Farm.StablesScandal.DogWomanHeavesSigh".Localized());

                        Save.SetBool("pureWhitneyStables", true);
                    }
            }
            else
            {
                OutputText("Scenes.Farm.StablesScandal.WhatTheHellDoing".Localized());

                OutputText("Scenes.Farm.StablesScandal.StillGotObviousMess".Localized());

                OutputText("Scenes.Farm.StablesScandal.FaceScrunchesUp".Localized());
            }

            OutputText("Scenes.Farm.StablesScandal.ParagraphBreak".Localized());
        }

        public void WhitneyFightsGnoll()
        {
            ClearOutput();

            OutputText("Scenes.Farm.WhitneyFightsGnoll.LoudNeighing".Localized());

            OutputText("Scenes.Farm.WhitneyFightsGnoll.WagonCarryingProduce".Localized());

            OutputText("Scenes.Farm.WhitneyFightsGnoll.FocusedLookEases".Localized());

            OutputText("Scenes.Farm.WhitneyFightsGnoll.NotTerriblySurprising".Localized());

            OutputText("Scenes.Farm.WhitneyFightsGnoll.TravelingTradesmen".Localized());

            OutputText("Scenes.Farm.WhitneyFightsGnoll.RoughPlaceToTravel".Localized((SceneInstance<MarbleScene>.Get().MarbleFollower() ? " Granted, you did take Marble away, but it's not as though you wouldn't understand if Whitney wanted Marble to tag along for a bit more of the day to travel." : ""), (CoC.Instance.Flags.Get(GameFlagIds.KeltKilled) == 0 && CoC.Instance.Flags.Get(GameFlagIds.KeltBreakLevel) < 4 ? " Kelt could surely help, both for his stature granting a good view of the surroundings, and his evident skill with a bow." : "")));

            OutputText("Scenes.Farm.WhitneyFightsGnoll.CouldntLeaveFarm".Localized());

            OutputText("Scenes.Farm.WhitneyFightsGnoll.SetsOffOnceMore".Localized());

            Save.SetBool("coc:gnoll", true);

            DoNext(() => ReturnToCamp(1));
        }

    }
}