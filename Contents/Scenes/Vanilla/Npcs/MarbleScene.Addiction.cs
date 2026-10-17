using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    public partial class MarbleScene
    {

        public static void DelayBottledMilk()
        {
            if (Player.HasStatusEffect(StatusEffectIds.BottledMilk))
                Player.ChangeStatusValue(StatusEffectIds.BottledMilk, 0, 1 + Random.Shared.Next(6));
            else
                Player.CreateStatusEffect(StatusEffectIds.BottledMilk, 3, 0, 0, 0);
        }

        public static void WithdrawalDelay()
        {
            DelayBottledMilk();

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                Player.RemoveStatusEffect(StatusEffectIds.MarbleWithdrawl);

                Player.ApplyStats(new DynStat(GameStats.Toughness, 5f),

                    new DynStat(GameStats.Intelligence, 5f));
            }
        }

        public void ApplyMarblesMilk()
        {
            Player.SlimeFeed();

            var str = Player.Strength;

            var tou = Player.Toughness;

            if (!Player.HasStatusEffect(StatusEffectIds.MarblesMilk))
            {
                Player.CreateStatusEffect(StatusEffectIds.MarblesMilk, 12, 0, 0, 0);

                Player.ApplyStats(new DynStat(GameStats.Strength, 5f), new DynStat(GameStats.Toughness, 10f));

                Player.ChangeStatusValue(StatusEffectIds.MarblesMilk, 1, Player.Strength - str);

                Player.ChangeStatusValue(StatusEffectIds.MarblesMilk, 2, Player.Toughness - tou);
            }
            else
                Player.ChangeStatusValue(StatusEffectIds.MarblesMilk, 0, 12);

            var duration = Player.StatusEffectValue(StatusEffectIds.MarblesMilk, 0);

            if (duration > 36)
                Player.ChangeStatusValue(StatusEffectIds.MarblesMilk, 0, 36 - duration);

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                Player.RemoveStatusEffect(StatusEffectIds.MarbleWithdrawl);

                Player.ApplyStats(new DynStat(GameStats.Toughness, 5f),

                    new DynStat(GameStats.Intelligence, 5f));
            }
        }

        public void MarbleGenderlessNoFuck()
        {
            OutputText("Scenes.Marblescene.MarbleGenderlessNoFuck.RemembersNoGenitals".Localized());

            OutputText("Scenes.Marblescene.MarbleGenderlessNoFuck.MarbleSpreadsLegs".Localized());

            OutputText("Scenes.Marblescene.MarbleGenderlessNoFuck.MarbleSighsGripTightens".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));
        }

        public void StandardSex(bool newpage = true)
        {
            if (newpage)
                ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                MarbleGenderlessNoFuck();

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Marblescene.StandardSex.LeadsToBed".Localized());

                if (IsFetishGear(Player.ArmorName))
                {
                    OutputText("Scenes.Marblescene.StandardSex.SmirkShowAccess".Localized());

                    if (Player.Cocks.Count > 0)
                    {
                        OutputText(Player.MultiCockDescriptLight());

                        if (Player.Vaginas.Count > 0)
                            OutputText("Scenes.Marblescene.StandardSex.AndYourPrefix".Localized());
                    }

                    if (Player.Vaginas.Count > 0)
                        OutputText(Player.VaginaDescript(0));

                    OutputText("Scenes.Marblescene.StandardSex.NoLingeringClothing".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.StandardSex.SmileRemoveArmor".Localized(
                        Appearance.AllBreastsDescript(Player)));

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Marblescene.StandardSex.HandRunsDown".Localized());
                    else
                        OutputText("Scenes.Marblescene.StandardSex.HandRunsOver".Localized());

                    OutputText("Scenes.Marblescene.StandardSex.OpenLowerClothes".Localized());

                    if (Player.Cocks.Count > 0)
                    {
                        OutputText(Player.MultiCockDescriptLight());

                        if (Player.Vaginas.Count > 0)
                            OutputText("Scenes.Marblescene.StandardSex.AndYourPrefixAgain".Localized());
                    }

                    if (Player.Vaginas.Count > 0)
                        OutputText(Player.VaginaDescript(0));

                    OutputText("Scenes.Marblescene.StandardSex.RemoveRemainingClothes".Localized());
                }

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    OutputText("Scenes.Marblescene.StandardSex.EyesWidenAtEquipment".Localized());

                    OutputText("Scenes.Marblescene.StandardSex.ChooseWhichGenitals".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Maleness".Localized(), () => MarbleSex2Continued(-1));

                    AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), () => MarbleSex2Continued(-2));
                }

                if (Player.Gender == Gender.Male)
                    MarbleSex2Continued(1);

                if (Player.Gender == Gender.Female)
                    MarbleSex2Continued(2);
            }
        }

        public static bool IsFetishGear(string armorName) =>

            armorName == "bondage patient clothes"

            || armorName == "crotch-revealing clothes"

            || armorName == "cute servant's clothes"

            || armorName == "maid's clothes"

            || armorName == "servant's clothes";

        public void MarbleSex2Continued(int genders)
        {
            if (genders < 0)
            {
                genders = -genders;

                ClearOutput();
            }

            if (genders == 1)
            {
                OutputText("Scenes.Marblescene.MarbleSex2Continued.LeansBackInvites".Localized());

                if (Player.Cocks[0].Length < 8)
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.SlideAllTheWay".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.UnableToTakeAll".Localized());

                OutputText("Scenes.Marblescene.MarbleSex2Continued.GaspsArmsClamp".Localized());

                if (Player.Tallness < 60 && Player.Tallness >= 48)
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.HeadLockedBetweenBreasts".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.BodyLockedAgainstHer".Localized());

                OutputText("Scenes.Marblescene.MarbleSex2Continued.GripSpeedsThrusts".Localized());

                if (Player.Cocks[0].Length < 8 && Player.HasKnot(0))
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.KnotSlipsInside".Localized());

                OutputText("Scenes.Marblescene.MarbleSex2Continued.ExplodesDeepWithin".Localized(
                    Player.CockDescript(0)));
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleSex2Continued.GrinsFetchesDildo".Localized(
                    Player.VaginaDescript(0)));

                if (Player.Tallness < 60 && Player.Tallness >= 48)
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.SqueezesHeadIntoBreasts".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.PullsYouAgainstHer".Localized());

                OutputText("Scenes.Marblescene.MarbleSex2Continued.ShuddersRedoublesEfforts".Localized());

                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Marblescene.MarbleSex2Continued.EruptsBetweenBodies".Localized());

                OutputText("Scenes.Marblescene.MarbleSex2Continued.GripRelentsPanting".Localized());

                Player.CuntChange(8, true);
            }

            if (!Player.HasStatusEffect(StatusEffectIds.FuckedMarble))
            {
                OutputText("Scenes.Marblescene.MarbleSex2Continued.BreaksSilenceAfter".Localized());

                Player.CreateStatusEffect(StatusEffectIds.FuckedMarble, 0, 0, 0, 0);
            }
            else
                OutputText("Scenes.Marblescene.MarbleSex2Continued.SighsBigSmile".Localized());

            Player.Orgasm("Generic");

            ReturnToCamp(1);
        }

        public void MarbleMilkSex(bool newpage = true)
        {
            if (newpage)
            {
                ClearOutput();

                DoNext(() => ReturnToCamp(1));
            }

            if (Player.Gender == Gender.Genderless)
            {
                MarbleGenderlessNoFuck();

                return;
            }

            OutputText("Scenes.Marblescene.MarbleMilkSex.MilkFilledNeed".Localized());

            if (IsFetishGear(Player.ArmorName))
                OutputText("Scenes.Marblescene.MarbleMilkSex.SheTakesFirstLook".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleMilkSex.ShePullsArmorOpen".Localized());

            if (Player.CockTotal() > 0)
            {
                if (Player.CockTotal() == Player.CountCocksOfType("Horse") + Player.DogCocks()
                    + Player.CountCocksOfType("Human"))

                    OutputText("Scenes.Marblescene.MarbleMilkSex.MarblePleasedAtSight".Localized(
                        Player.MultiCockDescriptLight()));
                else
                {
                    OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleLongHardLook".Localized());

                    var temp = Player.Cocks.Count;

                    while (temp > 0)
                    {
                        temp--;

                        if (Player.Cocks[temp].TypeIndex > 2)
                        {
                            OutputText(Player.CockDescript(temp));

                            temp = -1;
                        }
                    }

                    OutputText("Scenes.Marblescene.MarbleMilkSex.SheAsksUnsureTone".Localized());
                }

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleTakesMeasure".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleLiftsHerself".Localized());

                if (Player.Cocks[0].Length < 8)
                    OutputText("Scenes.Marblescene.MarbleMilkSex.CockFullyInside".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleBottomsOut".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleRidesShivering".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleSatisfiedAfterMilking".Localized());
            }
            else if (Player.Vaginas.Count > 0)
            {
                if (Player.BiggestBreastSize() > 2)
                    OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleTracesFingers".Localized(
                        Player.VaginaDescript(0),

                        Appearance.BiggestBreastSizeDescript(Player),

                        Appearance.AllBreastsDescript(Player)));
                else
                {
                    OutputText("Scenes.Marblescene.MarbleMilkSex.SheTeasesWithFingers".Localized(
                        Player.VaginaDescript(0),

                        Appearance.BiggestBreastSizeDescript(Player),

                        Appearance.AllBreastsDescript(Player)));

                    if (Player.TotalNipples() > 2)
                        OutputText("Scenes.Marblescene.MarbleMilkSex.OtherWord".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleMilkSex.RestWord".Localized());

                    OutputText("Scenes.Marblescene.MarbleMilkSex.SentencePeriodParagraphBreak".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleMilkSex.AfterFinishingUp".Localized());

                if (Player.BreastRows.Count > 2)
                    OutputText("Scenes.Marblescene.MarbleMilkSex.WithEachOfYour".Localized(
                        Appearance.AllBreastsDescript(Player)));
                else
                    OutputText("Scenes.Marblescene.MarbleMilkSex.WithEachOfBreasts".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleLooksUpTongue".Localized());

                if (Player.HasFuckableNipples())
                {
                    OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleTongueSlipsInside".Localized(
                        Player.NippleDescript(0)));

                    if (Player.BiggestLactation() > 1)
                        OutputText("Scenes.Marblescene.MarbleMilkSex.MarbleKeepsGulping".Localized());
                }
                else if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.Marblescene.MarbleMilkSex.SucklingBringsMilkStream".Localized());

                if (Player.BiggestLactation() <= 1 && !Player.HasFuckableNipples())
                    OutputText("Scenes.Marblescene.MarbleMilkSex.NoMilkOnlyNipple".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.ParagraphBreak".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.MarblePullsBackTalk".Localized());

                if (Player.TotalNipples() > 2)
                    OutputText("Scenes.Marblescene.MarbleMilkSex.NextWord".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleMilkSex.OtherWordAgain".Localized());

                OutputText("Scenes.Marblescene.MarbleMilkSex.StartsExperienceOver".Localized());
            }

            Player.Orgasm("Generic");
        }

        public void EncounterMarbleExploring()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.EncounterMarbleExploring.FarmPathMeeting".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.EncounterMarbleExploring.BreastHugGreeting".Localized());
            else
                OutputText("Scenes.Marblescene.EncounterMarbleExploring.QuickGreetingExchange".Localized());

            if (MarbleAffection < 30)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring.EagerForMilking".Localized());

                OutputText("Scenes.Marblescene.EncounterMarbleExploring.FarmWorkHint".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Marblescene.EncounterMarbleExploring.SuckleOfferQuestion".Localized());

            if (MarbleAddiction < 40)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring.MilkDrinkPrompt".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), DrinkMarbleMilk);

                AddButton(1, "Common.Button.No".Localized(), PlayerRefusesMarbleMilk);
            }
            else
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring.EagerMilkAgreement".Localized());

                DoNext(DrinkMarbleMilk);
            }
        }

        public void DrinkMarbleMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.DrinkMarbleMilk.RoomBedLapInvite".Localized());

            if (Player.Ears.Id != BodyPartTypes.Human)
                OutputText("Scenes.Marblescene.DrinkMarbleMilk.EarScratchClause".Localized());

            OutputText("Scenes.Marblescene.DrinkMarbleMilk.PeriodSpace".Localized());

            OutputText("Scenes.Marblescene.DrinkMarbleMilk.SatedPullBack".Localized());

            MarbleAddiction += 10;

            if (MarbleAddiction >= 50 && AddictionEnabled)
            {
                MarbleAddictionStart(false);

                MarbleAffection += 5;

                ApplyMarblesMilk();

                return;
            }

            MarbleAffection += 5;

            ApplyMarblesMilk();

            Player.HealthChange(10);

            Player.RefillHunger(20);

            Player.ChangeFatigue(-20);

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 20f));

            if (Player.LustPercent > 60)
            {
                MarbleMilkSex(false);

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (MarbleAddiction < 50)
                OutputText("Scenes.Marblescene.DrinkMarbleMilk.MilkEuphoriaExit".Localized());

            ApplyMarblesMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void PlayerRefusesMarbleMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PlayerRefusesMarbleMilk.AnnoyedHarrumph".Localized());

            MarbleAffection -= 5;

            MarbleAddiction -= 5;

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterMarbleExploring2()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.EncounterMarbleExploring2.RoomVisitDecision".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.RoomEntryBreastHug".Localized());

            OutputText("Scenes.Marblescene.EncounterMarbleExploring2.PleasantMealChat".Localized());

            Player.RefillHunger(20, false);

            if (CoC.Instance.Flags.Get(GameFlagIds.MurbleFarmTalkLevels) < 7 && Random.Shared.Next(3) > 0)
            {
                ExtendedMurbelFarmTalkz();

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Marblescene.EncounterMarbleExploring2.WorldDangersTalk".Localized());

            if (MarbleAddiction > 30)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.MilkScentAttraction".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
            }

            if (MarbleAffection < 60)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.InterludeFarewell".Localized());

                MarbleAffection += 3;

                if (Player.Intelligence < 30)
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 4f));
                else if (Player.Intelligence < 40)
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f));
                else if (Player.Intelligence < 60)
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (!Player.HasStatusEffect(StatusEffectIds.FuckedMarble))
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.IntenseLookOffer".Localized());

                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.InvitationPrompt".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                AddButton(0, "Common.Button.Yes".Localized(), AcceptMarblesSexualAdvances);

                AddButton(1, "Common.Button.No".Localized(), TurnDownMarbleSexFirstTime);
            }
            else
            {
                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.BedInvitationOffer".Localized());

                OutputText("Scenes.Marblescene.EncounterMarbleExploring2.InvitationPromptAgain".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                AddButton(0, "Common.Button.Yes".Localized(), AcceptMarblesSexualAdvances);

                AddButton(1, "Common.Button.No".Localized(), TurnDownMarbleSexRepeat);
            }
        }

        public void TurnDownMarbleSexFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.TurnDownMarbleSexFirstTime.RefusalSinksIn".Localized());

            MarbleAffection = 50;

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 4f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TurnDownMarbleSexRepeat()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.TurnDownMarbleSexRepeat.DisappointedBrightening".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AcceptMarblesSexualAdvances()
        {
            StandardSex(true);

            if (!Player.HasStatusEffect(StatusEffectIds.FuckedMarble))
                Player.CreateStatusEffect(StatusEffectIds.FuckedMarble, 0, 0, 0, 0);

            MarbleAffection += 10;

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));
        }

        public void HelpMarble1()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.HelpMarble1.WhitneyHelpSuggestion".Localized());

            if (Player.Strength < 20)
                OutputText("Scenes.Marblescene.HelpMarble1.HeavyCratesStruggle".Localized());
            else if (Player.Strength < 50)
                OutputText("Scenes.Marblescene.HelpMarble1.OutmatchedCrateHauling".Localized());
            else if (Player.Strength < 80)
                OutputText("Scenes.Marblescene.HelpMarble1.MatchedCrateEffort".Localized());
            else
                OutputText("Scenes.Marblescene.HelpMarble1.SurpassingMarbleStrength".Localized());

            OutputText("Scenes.Marblescene.HelpMarble1.MesmerizingHipSway".Localized());

            if (AfterMarbleHelp())
                return;

            OutputText("Scenes.Marblescene.HelpMarble1.DepartureRewardOffer".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, 1f));

            MarbleAffection += (int)Math.Round(Player.Strength / 10f);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            InventoryScene.TakeItem(ConsumableIds.MinotaurMilk, () => ReturnToCamp(1));
        }

        public void HelpMarble2()
        {
            OutputText("Scenes.Marblescene.HelpMarble2.WhitneyFarmEncounter".Localized());

            OutputText("Scenes.Marblescene.HelpMarble2.WeedingJobOffer".Localized());

            if (Player.Speed < 20)
                OutputText("Scenes.Marblescene.HelpMarble2.WeedMunchingLead".Localized());
            else if (Player.Speed < 50)
                OutputText("Scenes.Marblescene.HelpMarble2.WeedingRearGlimpse".Localized());
            else if (Player.Speed < 80)
                OutputText("Scenes.Marblescene.HelpMarble2.SpeedSurprisePout".Localized());
            else
                OutputText("Scenes.Marblescene.HelpMarble2.BreezyWeedHauling".Localized());

            Player.ApplyStats(new DynStat(GameStats.Speed, 1.5f));

            MarbleAffection += (int)Math.Round(Player.Speed / 10f);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            if (AfterMarbleHelp())
                return;

            OutputText("Scenes.Marblescene.HelpMarble2.DepartureRewardOffer".Localized());

            InventoryScene.TakeItem(ConsumableIds.MinotaurMilk, () => ReturnToCamp(1));
        }

        public bool AfterMarbleHelp()
        {
            if (MarbleAddiction >= 40)
            {
                if (Player.Intelligence < 40 && Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Marblescene.AfterMarbleHelp.GrowingMilkThirst".Localized());

                    OutputText("Scenes.Marblescene.AfterMarbleHelp.SatedPullBack".Localized());

                    MarbleAddiction += 10;

                    MarbleAddictionStart(false);

                    return true;
                }

                OutputText("Scenes.Marblescene.AfterMarbleHelp.MilkThoughtDistraction".Localized());
            }

            return false;
        }

        public void MarbleAddictionStart(bool newPage)
        {
            if (newPage)
                ClearOutput();

            OutputText("Scenes.Marblescene.MarbleAddictionStart.BreastScentSleep".Localized());

            OutputText("Scenes.Marblescene.MarbleAddictionStart.SuddenMilkCraving".Localized());

            OutputText("Scenes.Marblescene.MarbleAddictionStart.MilkAddictionNotice".Localized());

            OutputText("Scenes.Marblescene.MarbleAddictionStart.SweetieWellbeingAsk".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), WantMarbleAddiction);

            AddButton(1, "Common.Button.No".Localized(), DoNotWantMarbleAddiction);
        }

        public void WantMarbleAddiction()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.WantMarbleAddiction.WonderMilkPraise".Localized());

            OutputText("Scenes.Marblescene.WantMarbleAddiction.HesitationConfusion".Localized());

            OutputText("Scenes.Marblescene.WantMarbleAddiction.PonderingResponse".Localized());

            MarbleAffection += 5;

            KnowAddiction = 1;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            ApplyMarblesMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void DoNotWantMarbleAddiction()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.DoNotWantMarbleAddiction.AddictionRefusalTears".Localized());

            MarbleAffection += 5;

            KnowAddiction = 2;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            ApplyMarblesMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void AddictedEncounterHappy(bool clearS = true)
        {
            if (clearS)
                ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.MalonVisitedPostAddiction))
            {
                OutputText("Scenes.Marblescene.AddictedEncounterHappy.BarnBottleMeeting".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), PlayerAgreesToDrinkMarbleMilkBottled);

                AddButton(1, "Common.Button.No".Localized(), PlayerRefusesToDrinkBottledMilk);

                Player.CreateStatusEffect(StatusEffectIds.MalonVisitedPostAddiction, 0, 0, 0, 0);

                return;
            }

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Marblescene.AddictedEncounterHappy.BedsideBookVisit".Localized());

                if (Player.Intelligence >= 40)
                {
                    OutputText("Scenes.Marblescene.AddictedEncounterHappy.MilkDrinkPrompt".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), PlayerDrinksMarbleMilk);

                    AddButton(1, "Common.Button.No".Localized(), PlayerDeclinesToDrinkMarbleMilk);
                }
                else
                    PlayerDrinksMarbleMilk();
            }
            else
            {
                OutputText("Scenes.Marblescene.AddictedEncounterHappy.MidChoreHelpOffer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Marble".Localized(), MarbleChoreHelpChooseMarble);

                AddButton(1, "Common.Button.Milk".Localized(), MarbleChoreHelpChooseMilk);

                AddButton(2, "Common.Button.Refuse".Localized(), MarbleChoreRefusal);
            }
        }

        public void PlayerAgreesToDrinkMarbleMilkBottled()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PlayerAgreesToDrinkMarbleMilkBottled.BottleGuzzleRelief".Localized());

            MarbleAddiction += 5;

            if (Player.HasStatusEffect(StatusEffectIds.BottledMilk))
                Player.ChangeStatusValue(StatusEffectIds.BottledMilk, 0, 3 + Random.Shared.Next(6));
            else
                Player.CreateStatusEffect(StatusEffectIds.BottledMilk, 8, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void PlayerRefusesToDrinkBottledMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PlayerRefusesToDrinkBottledMilk.ForcedBottleReturn".Localized());

            MarbleAffection -= 5;

            MarbleAddiction -= 5;

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void PlayerDrinksMarbleMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PlayerDrinksMarbleMilk.LiftedToBreast".Localized());

            MarbleAffection += 5;

            MarbleAddiction += 10;

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 20f),

                new DynStat(GameStats.Corruption, 1f));

            if (Player.LustPercent >= 60)
            {
                OutputText("Scenes.Marblescene.PlayerDrinksMarbleMilk.ParagraphBreak".Localized());

                MarbleMilkSex(false);
            }
            else
                OutputText("Scenes.Marblescene.PlayerDrinksMarbleMilk.ForeheadKissSendoff".Localized());

            ApplyMarblesMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void PlayerDeclinesToDrinkMarbleMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PlayerDeclinesToDrinkMarbleMilk.BarelyEscapeRoom".Localized());

            MarbleAffection -= 5;

            MarbleAddiction -= 5;

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void MarbleChoreHelpChooseMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleChoreHelpChooseMilk.ReliefDrivenChores".Localized());

            OutputText("Scenes.Marblescene.MarbleChoreHelpChooseMilk.ImpressedMilkReward".Localized());

            InventoryScene.TakeItem(ConsumableIds.MinotaurMilk, () => ReturnToCamp(1));

            MarbleAffection -= 5;

            DelayBottledMilk();
        }

        public void MarbleChoreHelpChooseMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleChoreHelpChooseMarble.HelpWithoutMilk".Localized());

            OutputText("Scenes.Marblescene.MarbleChoreHelpChooseMarble.ThankfulBottleHandoff".Localized());

            InventoryScene.TakeItem(ConsumableIds.MinotaurMilk, () => ReturnToCamp(1));

            MarbleAffection += 5;

            DelayBottledMilk();
        }

        public void MarbleChoreRefusal()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleChoreRefusal.AngryWorkRefusal".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            MarbleAffection -= 5;

            MarbleAddiction -= 5;

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void MarbleEncounterAddictedNonWithdrawl()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawl.NonWithdrawalVisit".Localized());

            Player.RefillHunger(30);

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawl.ParagraphBreak".Localized());

            if (MarbleAffection >= 80)
                OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawl.EatingDeepGaze".Localized());

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawl.GratitudeComeBack".Localized());

            MarbleAffection += 10;

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterMarbleAshamedAddiction(bool clearS = true)
        {
            if (clearS)
                ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.MalonVisitedPostAddiction))
            {
                OutputText("Scenes.Marblescene.EncounterMarbleAshamedAddiction.BarnTankFollow".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Comfort".Localized(), AshamedAddictionComfort);

                AddButton(1, "Common.Button.Blame".Localized(), AshamedAddictionBlame);

                Player.CreateStatusEffect(StatusEffectIds.MalonVisitedPostAddiction, 0, 0, 0, 0);

                return;
            }

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleAshamedAddiction.BookResearchVisit".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Resist".Localized(), ResistAddiction);

                AddButton(1, "Common.Button.GiveIn".Localized(), AddictionGiveIn);
            }
            else
            {
                OutputText("Scenes.Marblescene.EncounterMarbleAshamedAddiction.BottleOfferExit".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.DumpIt".Localized(), DumpMarblesMilk);

                AddButton(1, "Common.Button.Refuse".Localized(), RefuseMarblesMilkAddiction);

                AddButton(2, "Common.Button.BegForIt".Localized(), AshamedAddictionBegForIt);
            }
        }

        public void AshamedAddictionBlame()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.AshamedAddictionBlame.BlamingOutburst".Localized());

            MarbleAffection = 0;

            MarbleAddiction -= 15;

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void AshamedAddictionComfort()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.AshamedAddictionComfort.SilentEmbrace".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.AshamedAddictionComfort.RelievedEmbraceWarmth".Localized());
            else
                OutputText("Scenes.Marblescene.AshamedAddictionComfort.ShortBreathsCrying".Localized());

            OutputText("Scenes.Marblescene.AshamedAddictionComfort.WhisperedThanks".Localized());

            MarbleAffection += 10;

            WithdrawalDelay();

            DoNext(() => ReturnToCamp(1));
        }

        public void ResistAddiction()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.ResistAddiction.StrainingTrialHold".Localized());

            MarbleAddiction -= 5;

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void AddictionGiveIn()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.AddictionGiveIn.JigglingBreastDesire".Localized());

            MarbleAffection += 3;

            MarbleAddiction += 10;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ApplyMarblesMilk();

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 20f));

            if (Player.LustPercent >= 60)
            {
                OutputText("Scenes.Marblescene.AddictionGiveIn.ParagraphBreak".Localized());

                MarbleMilkSex(false);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void DumpMarblesMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.DumpMarblesMilk.ExactWordsRepeat".Localized());

            MarbleAffection -= 5;

            MarbleAddiction -= 5;

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void RefuseMarblesMilkAddiction()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.RefuseMarblesMilkAddiction.RefusalWhyAsk".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.RefuseMarblesMilkAddiction.ChestHugClause".Localized());
            else
                OutputText("Scenes.Marblescene.RefuseMarblesMilkAddiction.BigHugClause".Localized());

            OutputText("Scenes.Marblescene.RefuseMarblesMilkAddiction.PraiseEmbraceDump".Localized());

            MarbleAffection += 5;

            DelayBottledMilk();

            DoNext(() => ReturnToCamp(1));
        }

        public void AshamedAddictionBegForIt()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.AshamedAddictionBegForIt.HorrorSnatchBottle".Localized());

            InventoryScene.TakeItem(ConsumableIds.MinotaurMilk, () => ReturnToCamp(1));

            DelayBottledMilk();
        }

        public void MarbleEncounterAddictedNonWithdrawlAshamed()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.AshamedCalmVisit".Localized());

            if (MarbleAffection >= 30)
                OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.WorriedWord".Localized());

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.DawningRealizationClause".Localized());

            if (MarbleAffection >= 30)
                OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.SweetiePrefix".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.WLetter".Localized());

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.WhyHereWithoutMilk".Localized());

            Player.RefillHunger(30, false);

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.ParagraphBreak".Localized());

            if (MarbleAffection >= 80)
                OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.EatingGazeHesitation".Localized());

            OutputText("Scenes.Marblescene.MarbleEncounterAddictedNonWithdrawlAshamed.ThankfulThinkingReturn".Localized());

            MarbleAffection += 10;

            DoNext(() => ReturnToCamp(1));
        }

        public void PostAddictionFarmMornings()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PostAddictionFarmMornings.DailyDoseRush".Localized());

            Player.RefillHunger(20);

            Player.SlimeFeed();

            if (Player.Corruption < 40)
            {
                if (Player.Corruption < 30)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void PostAddictionFarmExplorings()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.DailyRoomVisit".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.EagerRoomWelcome".Localized());

            OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.HappyMealChat".Localized());

            Player.RefillHunger(20, false);

            if (CoC.Instance.Flags.Get(GameFlagIds.MurbleFarmTalkLevels) < 7)
            {
                ExtendedMurbelFarmTalkz();

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.TalkWorldDangers".Localized());

            OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.EntireTimeClause".Localized());

            if (Player.HasPerk(PerkIds.MarbleResistant))
                OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.UncomfortablyWord".Localized());

            OutputText("Scenes.Marblescene.PostAddictionFarmExplorings.MilkScentAwareClause".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ExtendedMurbelFarmTalkz()
        {
            switch (CoC.Instance.Flags.Get(GameFlagIds.MurbleFarmTalkLevels))
            {
                case 0:
                    ExtendedMurbelFarmTalkFirst();

                    break;

                case 1:
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SmallTalkThenAsk".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SighWordResponse".Localized());
                    else if (Player.Corruption < 66)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.PauseBeforeTell".Localized());
                    else
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ChuckleThenPause".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SheGetsMistyEyed".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MarbleDoubleTake".Localized());

                    break;

                case 2:
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.AskLastJobTrouble".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SheFrownsYoungest".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.IdleChatThenPart".Localized());

                    break;

                case 3:
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MarbleNotesMission".Localized());

                    if (Player.Tallness < 60)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.WipeTearsCuteness".Localized());
                    else if (Player.Tallness > 84)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.PerfectMissionChoice".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.LeftNoNobleReason".Localized());

                    if (MarbleAffection >= 60)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.FoundWhatSheSought".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.WhitneyKnocksDoor".Localized());

                    break;

                case 4:
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MarbleOffersOwnTale".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MountainDemonSlayer".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.LeftMountainsSeeking".Localized());

                    if (KnowAddiction == 2 || Player.HasPerk(PerkIds.MarbleResistant))
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SighsAddictionWish".Localized());
                    else if (KnowAddiction == 1 || Player.HasPerk(PerkIds.MarblesMilk))
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.WinksAddictionPity".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ConcludesPastRegrets".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ChangeToWeather".Localized());

                    break;

                case 5:
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.YourTurnLoveLife".Localized());

                    if (Player.HasPerk(PerkIds.HistoryWhore) || Player.HasPerk(PerkIds.HistorySlut))
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ArousingDescriptionShare".Localized());
                    else if (Player.Corruption < 33)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SweetieWillShare".Localized());
                    else if (Player.Corruption < 66)
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SweetieSeesYouCare".Localized());
                    else
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ArousingDescriptionShareAgain".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MissedNursingAnsgar".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ShakesHeadSmiles".Localized());

                    if (Player.HasStatusEffect(StatusEffectIds.ImpGangBang))
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ChuckleLateAdvice".Localized());
                    else
                        OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ThankAdviceStillGuard".Localized());

                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.EndTalkForNow".Localized());

                    break;

                default:
                    ExtendedMurbelFarmTalkDefault();

                    break;
            }

            CoC.Instance.Flags.Set(GameFlagIds.MurbleFarmTalkLevels,

                CoC.Instance.Flags.Get(GameFlagIds.MurbleFarmTalkLevels) + 1);
        }

        public void ExtendedMurbelFarmTalkFirst()
        {
            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MarbleAskYourOrigin".Localized());

            if (Player.Race == "human")
            {
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.OnlyOtherHumanMet".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.HerEyesLightUp".Localized());
                else if (Player.Tallness > 78)
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.AsksHumanHeight".Localized());
            }
            else if (Player.Race == "cow-morph" || Player.Race == "cow-boy")
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.GladToSeeKind".Localized());
            else if (Player.Race == "dog-morph")
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.AskDogMorphOrigin".Localized());
            else if (Player.IsTaur())
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.CentaursAreNomads".Localized());
            else
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.RareRaceAsksLocal".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SighThinkBack".Localized());

            if (Player.Race != "human")
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ExplainOnceHuman".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.TellPortalOrigin".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.TellOfParents".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MealPassesQuietly".Localized());
        }

        public void ExtendedMurbelFarmTalkDefault()
        {
            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.StumbledUponFarm".Localized());

            if (Player.Strength < 90)
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.NoneStrongerThanMe".Localized());
            else
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.OnlyOtherStronger".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SayEnd".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.NoticesDumbstruck".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.StoryOfMyLife".Localized());

            if (!MarbleFollower())
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.MissionStillUntold".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SentencePeriodSayEnd".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.AskIfSheChanged".Localized());

            if (!MarbleFollower())
            {
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.HidesInnerLonely".Localized());

                if (KnowAddiction == 2)
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.WantedLovedNursing".Localized());
                else if (KnowAddiction == 1)
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.FoundTheOne".Localized());
                else
                    OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.NotGivingUpHope".Localized());

                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.SayEndAgain".Localized());
            }
            else
                OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.LonelyUntilMetYou".Localized());

            OutputText("Scenes.Marblescene.ExtendedMurbelFarmTalkz.ThankForSharing".Localized());
        }

        public void PostAddictionFarmHelpings()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PostAddictionFarmHelpings.WhitneySuggestsChores".Localized());

            if (Player.HasPerk(PerkIds.MarbleResistant))
                OutputText("Scenes.Marblescene.PostAddictionFarmHelpings.AssuresNoAddiction".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, Random.Shared.Next(2)),

                new DynStat(GameStats.Speed, Random.Shared.Next(2)));

            InventoryScene.TakeItem(ConsumableIds.MinotaurMilk, () => ReturnToCamp(1));
        }
    }
}