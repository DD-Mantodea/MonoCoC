using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{

    public partial class MarbleScene
    {

        public bool HighLibidoTryst;

        public int _checkedMarbleMilk;

        public static Random Rng => Random.Shared;

        public static bool AddictionEnabled => Configuration.OptionStore.Instance.Get("addiction", 1) > 0;

        public bool MarbleFollower() => Player.HasStatusEffect(StatusEffectIds.CampMarble);

        public int MarbleAffection
        {
            get => (int)Player.StatusEffectValue(StatusEffectIds.Marble, 0);

            set
            {
                EnsureMarbleStatus();

                Player.StatusEffectByType(StatusEffectIds.Marble)!.Values[0] = Math.Clamp(value, 0, 100);
            }
        }

        public int MarbleAddiction
        {
            get => (int)Player.StatusEffectValue(StatusEffectIds.Marble, 1);

            set
            {
                EnsureMarbleStatus();

                Player.StatusEffectByType(StatusEffectIds.Marble)!.Values[1] = Math.Clamp(value, 0, 100);
            }
        }

        public int KnowAddiction
        {
            get => (int)Player.StatusEffectValue(StatusEffectIds.Marble, 2);

            set
            {
                EnsureMarbleStatus();

                Player.StatusEffectByType(StatusEffectIds.Marble)!.Values[2] = value;
            }
        }

        public int knowAddiction => KnowAddiction;

        public static int MarblePregnancyRawType => Flags.Get(GameFlagIds.MarblePregnancyType);

        public static int MarblePregnancyType => MarblePregnancyRawType & 0xFFFF;

        public static int MarblePregnancyIncubation => Flags.Get(GameFlagIds.MarblePregnancyIncubation);

        public static void EnsureMarbleStatus()
        {
            if (!Player.HasStatusEffect(StatusEffectIds.Marble))
                Player.CreateStatusEffect(StatusEffectIds.Marble, 0, 0, 0, 40);
        }

        public static bool AmilyFollower()
        {
            if (Flags.Get(GameFlagIds.AmilyFollower) == 0)
                return false;

            if (Flags.Get(GameFlagIds.FollowerAtFarmAmily) != 0)
                return false;

            var visitingUrta = Flags.Get(GameFlagIds.AmilyVisitingUrta);

            return visitingUrta != 1 && visitingUrta != 2;
        }

        public static bool IsabellaAtCamp() =>

            Flags.Get(GameFlagIds.IsabellaFollowerAccepted) > 0

            && Flags.Get(GameFlagIds.FollowerAtFarmIsabella) == 0;

        public static bool MarblePregnancyIsPregnant => MarblePregnancyType != 0;

        public static void MarblePregnancyAdvance()
        {
            var next = MarblePregnancyIncubation - 1;

            if (next >= 0)
                Flags.Set(GameFlagIds.MarblePregnancyIncubation, next);
        }

        public static void MarblePregnancyKnockUpForce()
        {
            Flags.Set(GameFlagIds.MarblePregnancyType, 0);

            Flags.Set(GameFlagIds.MarblePregnancyIncubation, 0);
        }

        public void MarbleSprite(bool nude = false)
        {
        }

        public bool MarbleAtCamp() =>

            MarbleFollower()

            && Flags.Get(GameFlagIds.MarbleRathazulCounter2) <= 0

            && Flags.Get(GameFlagIds.FollowerAtFarmMarble) == 0;

        public string MarbleBreastSize()
        {
            if (Flags.Get(GameFlagIds.MarblePurified) > 0)
                return Flags.Get(GameFlagIds.MarbleBreastSize) switch
                {
                    0 => "DD cup",

                    1 => "G cup",

                    2 => "HH cup",

                    _ => "J cup"
                };

            return Player.StatusEffectValue(StatusEffectIds.Marble, 3) > 30 ? "HH cup" : "G cup";
        }

        public bool TimeChange()
        {
            var needNext = false;

            _checkedMarbleMilk = 0;

            MarblePregnancyAdvance();

            if (MarbleFollower())
            {
                if (Flags.Get(GameFlagIds.MarbleRathazulCounter1) > 0)
                {
                    Flags.Set(GameFlagIds.MarbleRathazulCounter1, Flags.Get(GameFlagIds.MarbleRathazulCounter1) - 1);

                    if (Flags.Get(GameFlagIds.MarbleRathazulCounter1) <= 1)
                        Flags.Set(GameFlagIds.MarbleRathazulCounter1, 1);
                }

                if ((Flags.Get(GameFlagIds.MarblePurificationStage) == 0) && (Flags.Get(GameFlagIds.MarbleCountupToPurifying) < 200))
                        Flags.Set(GameFlagIds.MarbleCountupToPurifying, Flags.Get(GameFlagIds.MarbleCountupToPurifying) + 1);

                if (Flags.Get(GameFlagIds.MarbleRathazulCounter2) > 0)
                {
                    Flags.Set(GameFlagIds.MarbleRathazulCounter2, Flags.Get(GameFlagIds.MarbleRathazulCounter2) - 1);

                    if (Flags.Get(GameFlagIds.MarbleRathazulCounter2) <= 1)
                        Flags.Set(GameFlagIds.MarbleRathazulCounter2, 1);
                }

                if (Flags.Get(GameFlagIds.MarblePurificationStage) >= 5)
                {
                    Flags.Set(GameFlagIds.MarbleTimeSinceNursedInHours,

                        Flags.Get(GameFlagIds.MarbleTimeSinceNursedInHours) + 1);

                    if (Flags.Get(GameFlagIds.MarbleTimeSinceNursedInHours) > 1000)
                        Flags.Set(GameFlagIds.MarbleTimeSinceNursedInHours, 1000);
                }

                if (Flags.Get(GameFlagIds.MarbleWarnedAboutCorruption) == 1 && Player.IsPureEnough(50))
                    Flags.Set(GameFlagIds.MarbleWarnedAboutCorruption, 0);

                if (Flags.Get(GameFlagIds.MarbleTimeSinceNursedInHours) < 100)
                    Flags.Set(GameFlagIds.MarbleTimeSinceNursedInHours,

                        Flags.Get(GameFlagIds.MarbleTimeSinceNursedInHours) + 1);

                if (Flags.Get(GameFlagIds.MarbleLust) < -100)
                    Flags.Set(GameFlagIds.MarbleLust, -100);

                if (Rng.Next(2) == 0)
                    Flags.Set(GameFlagIds.MarbleLust, Flags.Get(GameFlagIds.MarbleLust) + 1);

                if (Player.StatusEffectValue(StatusEffectIds.Marble, 3) > 50)
                    Flags.Set(GameFlagIds.MarbleLust, Flags.Get(GameFlagIds.MarbleLust) + .3f);

                if (Player.StatusEffectValue(StatusEffectIds.Marble, 3) > 70)
                    Flags.Set(GameFlagIds.MarbleLust, Flags.Get(GameFlagIds.MarbleLust) + .3f);

                if (Flags.Get(GameFlagIds.MarbleNurseryConstruction) > 0
                    && Flags.Get(GameFlagIds.MarbleNurseryConstruction) < 100

                    && MarbleAtCamp())
                {
                    Flags.Set(GameFlagIds.MarbleNurseryConstruction,

                        Flags.Get(GameFlagIds.MarbleNurseryConstruction) + 1);

                    if (Flags.Get(GameFlagIds.MarbleNurseryConstruction) >= 100)
                    {
                        MarbleSprite();

                        OutputText("Scenes.Marblescene.TimeChange.NurseryBuiltNotice".Localized());

                        needNext = true;

                        Flags.Set(GameFlagIds.MarbleNurseryConstruction, 100);
                    }
                }
                else if ((!Player.HasStatusEffect(StatusEffectIds.MarbleHasItem) && MarbleAtCamp()) && (!Player.HasStatusEffect(StatusEffectIds.MarbleItemCooldown) && Rng.Next(10) == 0))
                    {
                        MarbleSprite();

                        OutputText("Scenes.Marblescene.TimeChange.CampNoteFromMarble".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.MarbleItemCooldown, 24 + Rng.Next(24), 0, 0, 0);

                        Player.CreateStatusEffect(StatusEffectIds.MarbleHasItem, Rng.Next(10), 0, 0, 0);

                        needNext = true;
                    }
            }

            if (Player.HasStatusEffect(StatusEffectIds.MarbleItemCooldown))
            {
                Player.ChangeStatusValue(StatusEffectIds.MarbleItemCooldown, 0, -1);

                if (Player.StatusEffectValue(StatusEffectIds.MarbleItemCooldown, 0) < 1)
                    Player.RemoveStatusEffect(StatusEffectIds.MarbleItemCooldown);
            }

            if (!Player.HasStatusEffect(StatusEffectIds.Infested))
                Flags.Set(GameFlagIds.MarbleGrossedOutBecauseWormInfestation, 0);

            if (Player.HasStatusEffect(StatusEffectIds.MarblesMilk) && !Player.HasPerk(PerkIds.MarblesMilk))
            {
                Player.ChangeStatusValue(StatusEffectIds.MarblesMilk, 0, -1);

                if (Player.StatusEffectValue(StatusEffectIds.MarblesMilk, 0) <= 0)
                {
                    needNext = true;

                    Player.ApplyStats(
                        new DynStat(GameStats.Strength, -Player.StatusEffectValue(StatusEffectIds.MarblesMilk, 1)),

                        new DynStat(GameStats.Toughness, -Player.StatusEffectValue(StatusEffectIds.MarblesMilk, 2)));

                    Player.RemoveStatusEffect(StatusEffectIds.MarblesMilk);

                    if (MarbleAddiction <= 10)
                        OutputText("Scenes.Marblescene.TimeChange.EuphoriaFadeRealization".Localized());
                    else if (MarbleAddiction <= 30)
                        OutputText("Scenes.Marblescene.TimeChange.MildEuphoriaLoss".Localized());
                    else if (MarbleAddiction <= 50)
                        OutputText("Scenes.Marblescene.TimeChange.ShiverAndSuckleUrge".Localized());

                    if (KnowAddiction > 0)
                        if (Player.HasStatusEffect(StatusEffectIds.BottledMilk))
                            OutputText("Scenes.Marblescene.TimeChange.TremblingHandsWithdrawal".Localized());
                        else
                        {
                            if (MarbleAddiction <= 90)
                                OutputText("Scenes.Marblescene.TimeChange.DesperateTremblingNeed".Localized());
                            else
                                OutputText("Scenes.Marblescene.TimeChange.CravingEscapeUrge".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.MarbleWithdrawl, 0, 0, 0, 0);

                            Player.ApplyStats(
                                new DynStat(GameStats.Toughness, -5f),

                                new DynStat(GameStats.Intelligence, -5f));
                        }
                }
            }

            if ((KnowAddiction > 0
                && !Player.HasPerk(PerkIds.MarbleResistant)

                && !Player.HasPerk(PerkIds.MarblesMilk)

                && MarbleAddiction > 25

                && AddictionEnabled) && ((!Player.HasStatusEffect(StatusEffectIds.MarblesMilk)

                    && !Player.HasStatusEffect(StatusEffectIds.BottledMilk)) && (!Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))))
                    {
                        OutputText("Scenes.Marblescene.TimeChange.OverwhelmedMilkDesire".Localized());

                        needNext = true;

                        Player.CreateStatusEffect(StatusEffectIds.MarbleWithdrawl, 0, 0, 0, 0);

                        Player.ApplyStats(
                            new DynStat(GameStats.Toughness, -5f),

                            new DynStat(GameStats.Intelligence, -5f));
                    }

            if (KnowAddiction <= 0 && MarbleAddiction > 60)
                MarbleAddiction = 60;

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                if (MarbleAddiction <= 25)
                {
                    Player.RemoveStatusEffect(StatusEffectIds.MarbleWithdrawl);

                    Player.ApplyStats(
                        new DynStat(GameStats.Toughness, 5f),

                        new DynStat(GameStats.Intelligence, 5f));

                    OutputText("Scenes.Marblescene.TimeChange.WithdrawalSymptomsGone".Localized());

                    needNext = true;
                }
                else if (Player.HasStatusEffect(StatusEffectIds.BottledMilk))
                {
                    OutputText("Scenes.Marblescene.TimeChange.WithdrawalSymptomsGoneAgain".Localized());

                    needNext = true;

                    Player.RemoveStatusEffect(StatusEffectIds.MarbleWithdrawl);

                    Player.ApplyStats(
                        new DynStat(GameStats.Toughness, 5f),

                        new DynStat(GameStats.Intelligence, 5f));
                }

            if (Player.HasStatusEffect(StatusEffectIds.BottledMilk))
            {
                Player.ChangeStatusValue(StatusEffectIds.BottledMilk, 0, -1);

                if (Player.StatusEffectValue(StatusEffectIds.BottledMilk, 0) <= 0)
                    Player.RemoveStatusEffect(StatusEffectIds.BottledMilk);
            }

            if (View.Time.Hours == 0)
                Flags.Set(GameFlagIds.MarblePlayedWithKidsToday, 0);

            return needNext;
        }

        public bool TimeChangeLarge()
        {
            if (MarblePregnancyIsPregnant && MarblePregnancyIncubation == 0 && MarbleAtCamp())
            {

                MarblePoopsBaybees();

                MarblePregnancyKnockUpForce();

                return false;
            }

            if (KnowAddiction > 0
                && MarbleAddiction >= 100

                && !Player.HasPerk(PerkIds.MarblesMilk)

                && !Player.HasPerk(PerkIds.MarbleResistant)

                && View.Time.Hours == 6)
            {
                MarbleSprite();

                OutputText("Scenes.Marblescene.TimeChangeLarge.ChillingClarityAwakening".Localized());

                OutputText("Scenes.Marblescene.TimeChangeLarge.MarbleSmilesInRoom".Localized());

                Player.CreatePerk(PerkIds.MarblesMilk, 0, 0, 0, 0);

                if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
                {
                    Player.RemoveStatusEffect(StatusEffectIds.MarbleWithdrawl);

                    Player.ApplyStats(
                        new DynStat(GameStats.Toughness, 5f),

                        new DynStat(GameStats.Intelligence, 5f));
                }

                if (Player.HasStatusEffect(StatusEffectIds.MarblesMilk))
                    Player.RemoveStatusEffect(StatusEffectIds.MarblesMilk);
                else
                    Player.ApplyStats(
                        new DynStat(GameStats.Strength, 5f),

                        new DynStat(GameStats.Toughness, 10f));

                if (KnowAddiction == 1)
                {
                    if (MarbleAffection < 49)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.PulledIntoHerChest".Localized());

                        DoNext(() => MarbleBadEndFollowup());

                        return false;
                    }

                    if (MarbleAffection >= 50 && MarbleAffection < 79)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.LeaveThingsAsIs".Localized());

                    if (MarbleAffection >= 80)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.LikingHerForHer".Localized());

                        MarbleAddictionSex(false);

                        OutputText("Scenes.Marblescene.TimeChangeLarge.ParagraphBreak".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.CampMarble, 0, 0, 0, 0);

                        Flags.Set(GameFlagIds.FollowerAtFarmMarble, 0);

                        if (IsabellaAtCamp())
                            Flags.Set(GameFlagIds.IsabellaMurbleBleh, 1);

                        Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);

                        Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

                        if (Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1
                            && Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0)

                            Flags.Set(GameFlagIds.IzmaMarbleFreakoutStatus, 1);
                    }
                }
                else
                {
                    if (MarbleAffection < 30)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ChestClutchApology".Localized());

                        DoNext(() => MarbleBadEndFollowup());

                        return false;
                    }

                    if (MarbleAffection < 80)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.DailyMilkArrangement".Localized());
                    else
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ForgivenessAndSpecialAsk".Localized());

                        MarbleAddictionSex(false);

                        OutputText("Scenes.Marblescene.TimeChangeLarge.ParagraphBreakAgain".Localized());

                        SetMarbleMovedToCamp();
                    }
                }

                OutputText("Scenes.Marblescene.TimeChangeLarge.MilkPerkGained".Localized());

                PlayerMenu();

                return false;
            }

            if (Player.HasStatusEffect(StatusEffectIds.Marble) && View.Time.Hours == 6)
                MarbleAddiction -= 1;

            if (KnowAddiction > 0
                && MarbleAddiction < 25

                && !Player.HasPerk(PerkIds.MarblesMilk)

                && !Player.HasPerk(PerkIds.MarbleResistant)

                && View.Time.Hours == 6)
            {
                MarbleSprite();

                OutputText("Scenes.Marblescene.TimeChangeLarge.StrangelyRestedAwakening".Localized());

                OutputText("Scenes.Marblescene.TimeChangeLarge.StartledMarbleGreeting".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

                Player.CreatePerk(PerkIds.MarbleResistant, 0, 0, 0, 0);

                if (KnowAddiction == 1)
                {
                    if (MarbleAffection < 30)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.SheStormsOutLeaving".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);
                    }

                    if (MarbleAffection >= 30 && MarbleAffection < 90)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.CalmingDownTogether".Localized());

                    if (MarbleAffection >= 90)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.StaysAtCampOffer".Localized());

                        OutputText("Scenes.Marblescene.TimeChangeLarge.LoversMenuNotice".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.CampMarble, 0, 0, 0, 0);

                        Flags.Set(GameFlagIds.FollowerAtFarmMarble, 0);

                        if (IsabellaAtCamp())
                            Flags.Set(GameFlagIds.IsabellaMurbleBleh, 1);

                        Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

                        if (Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1
                            && Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0)

                            Flags.Set(GameFlagIds.IzmaMarbleFreakoutStatus, 1);

                        Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);
                    }
                }
                else
                {
                    if (MarbleAffection < 30)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ColdStareAtNews".Localized());

                        OutputText("Scenes.Marblescene.TimeChangeLarge.TornEncyclopediaPage".Localized());

                        OutputText("Scenes.Marblescene.TimeChangeLarge.CrudeHandwrittenThreat".Localized());

                        Player.UnlockCodexEntry(GameFlagIds.CodexEntryLabovines);

                        Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);
                    }

                    if (MarbleAffection >= 30 && MarbleAffection < 70)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ReassuredGoodFriend".Localized());

                    if (MarbleAffection >= 70)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.TearsAndTightHug".Localized());

                        MarbleAddictionSex(false);

                        OutputText("Scenes.Marblescene.TimeChangeLarge.ParagraphBreakThird".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.CampMarble, 0, 0, 0, 0);

                        Flags.Set(GameFlagIds.FollowerAtFarmMarble, 0);

                        if (IsabellaAtCamp())
                            Flags.Set(GameFlagIds.IsabellaMurbleBleh, 1);

                        Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

                        if (Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1
                            && Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0)

                            Flags.Set(GameFlagIds.IzmaMarbleFreakoutStatus, 1);

                        Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);
                    }
                }

                OutputText("Scenes.Marblescene.TimeChangeLarge.ResistancePerkGained".Localized());

                PlayerMenu();

                return false;
            }

            if (_checkedMarbleMilk++ == 0 && View.Time.Hours == 6 && Player.HasPerk(PerkIds.MarblesMilk))
            {
                if (MarbleAtCamp())
                    PostAddictionCampMornings(false);
                else
                    if (!Player.HasStatusEffect(StatusEffectIds.NoMoreMarble))
                        PostAddictionFarmMornings();

                PlayerMenu();

                return false;
            }

            return false;
        }

        public void SetMarbleMovedToCamp()
        {
            Player.CreateStatusEffect(StatusEffectIds.CampMarble, 0, 0, 0, 0);

            Flags.Set(GameFlagIds.FollowerAtFarmMarble, 0);

            if (IsabellaAtCamp())
                Flags.Set(GameFlagIds.IsabellaMurbleBleh, 1);

            Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);

            Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

            if (Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1
                && Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0)

                Flags.Set(GameFlagIds.IzmaMarbleFreakoutStatus, 1);
        }

        public void EncounterMarbleInitially()
        {
            MarbleSprite();

            Player.CreateStatusEffect(StatusEffectIds.Marble, 0, 0, 0, 40);

            OutputText("Scenes.Marblescene.EncounterMarbleInitially.SouthernBelleSpotted".Localized((noFur ? "" : " furry")));

            OutputText("Scenes.Marblescene.EncounterMarbleInitially.WhitneyExplainsSoreness".Localized());

            OutputText("Scenes.Marblescene.EncounterMarbleInitially.SearchingBarnRooms".Localized((noFur ? "just above her knees" : "at least mid-way down her thighs")));

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryLabovines, false, true);

            if (Player.Tallness < 60)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleInitially.CuteMilkInvitation".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHer".Localized(), MarblePicksYouUpInitially);

                AddButton(1, "Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), ResistMarbleInitially);

                return;
            }

            OutputText("Scenes.Marblescene.EncounterMarbleInitially.NameExchange".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Caress".Localized(), CaressMarble, "Give her a gentle massage.");

            AddButton(1, "Common.Button.Suckle".Localized(), SuckleMarble, "Lighten her load by drinking some milk.");

            AddButton(2, "Common.Button.Rape".Localized(), () => RapeDAHMARBLEZ(), "Take what you want from her.");

            AddButton(14, "Common.Button.Leave".Localized(), TurnOffMarbleForever);
        }

        public void TurnOffMarbleForever()
        {
            ClearOutput();

            Flags.Set(GameFlagIds.MarbleWarning, 1);

            OutputText("Scenes.Marblescene.TurnOffMarbleForever.ConsideringHerBreasts".Localized());

            ReturnToCamp(1);
        }

        public void MarbleWarningStateMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleWarningStateMeeting.FieldEncounterWalk".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Apologize".Localized(), ApologizeToWalkingTitsIMEANMARBLE);

            AddButton(1, "Common.Button.PickFight".Localized(), PickAFight);

            AddButton(2, "Common.Button.Leave4ever".Localized(), LeaveNonFriendsMarble4EVERRRR);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveNonFriendsMarble);
        }

        public void LeaveNonFriendsMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.LeaveNonFriendsMarble.InsincerePoliteExit".Localized());

            ReturnToCamp(1);
        }

        public void LeaveNonFriendsMarble4EVERRRR()
        {
            ClearOutput();

            Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);

            Flags.Set(GameFlagIds.MarbleWarning, 2);

            OutputText("Scenes.Marblescene.LeaveNonFriendsMarble4EVERRRR.BlankLookRefusal".Localized());

            ReturnToCamp(1);
        }

        public void ApologizeToWalkingTitsIMEANMARBLE()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.SeekingReconciliation".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.DucksIntoGrainField".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.ChasingThroughCrops".Localized());

            if (Player.Intelligence < 15)
                OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.CaughtWithHandsUp".Localized());
            else if (Player.Intelligence < 40)
                OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.WaitingHandsRaised".Localized());
            else
                OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.EasyCatchOffGuard".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.PulledOntoHerLap".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.OffersBountifulBreasts".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.ExpectedNursingOffer".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.AskedToSwitchBreast".Localized());

            Player.RefillHunger(20);

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.FinallyFedPullBack".Localized());

            MarbleAffection = 5;

            MarbleAddiction += 10;

            Flags.Set(GameFlagIds.MarbleWarning, 0);

            ApplyMarblesMilk();

            Player.ApplyStats(
                new DynStat(GameStats.Libido, .2f),

                new DynStat(GameStats.Lust, 5 + Player.Libido / 10f));

            Player.HealthChange(100);

            Player.ChangeFatigue(-50);

            ReturnToCamp(1);
        }

        public void PickAFight()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PickAFight.VoicingDispleasure".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), StayForFights);

            AddButton(1, "Common.Button.FuckThat".Localized(), GetOutOfDodge);
        }

        public void StayForFights()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.StayForFights.ArmsCrossedDefiance".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "marble")), () => ReturnToCamp(1));
        }

        public void GetOutOfDodge()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.GetOutOfDodge.TruthOverTalkingHammers".Localized());

            Flags.Set(GameFlagIds.MarbleWarning, 3);

            ReturnToCamp(1);
        }

        public void ResistMarbleInitially()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.ResistMarbleInitially.ResistanceCausesApology".Localized());

            OutputText("Scenes.Marblescene.ResistMarbleInitially.NameIntroduction".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Caress".Localized(), CaressMarble, "Give her a gentle massage.");

            AddButton(1, "Common.Button.Suckle".Localized(), SuckleMarble, "Lighten her load by drinking some milk.");

            AddButton(2, "Common.Button.Rape".Localized(), () => RapeDAHMARBLEZ(), "Take what you want from her.");

            AddButton(14, "Common.Button.Leave".Localized(), TurnOffMarbleForever);
        }

        public void MarblePicksYouUpInitially()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.LiftedOntoHerLap".Localized());

            Player.RefillHunger(30);

            OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.MilkedEnoughLeaning".Localized());

            if (Player.Body.Get<BodyParts.Vanilla.EarsPart>().Id != BodyPartTypes.Human)
                OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.EarScratchGesture".Localized());
            else
                OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.HeadCaressGesture".Localized());

            OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.ThankfulAsksForName".Localized());

            MarbleAffection += 30;

            MarbleAddiction += 10;

            ApplyMarblesMilk();

            ReturnToCamp(1);
        }

        public void CaressMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.CaressMarble.OfferGentleCaress".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            MarbleAffection += 5;

            ReturnToCamp(1);
        }

        public void SuckleMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.SuckleMarble.SuggestSucklingHelp".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.SuckleMarble.ChairToReachBreasts".Localized());

            OutputText("Scenes.Marblescene.SuckleMarble.LeaningToSuckle".Localized());

            Player.RefillHunger(30);

            MarbleAffection += 15;

            MarbleAddiction += 10;

            ApplyMarblesMilk();

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ReturnToCamp(1);
        }

        public void RapeMarbleInHerRoom()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.DraggedFromAudience".Localized());

            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.RoughBreastSqueeze".Localized());

            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SlapDodgedNippleTwist".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.ChuckleFreeingCocks".Localized());

                if (Player.Cocks[0].Thickness < 3)
                {
                    if (Player.Cocks[0].Length <= 8)
                        OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.ForcingDeepInsertion".Localized());
                    else
                        OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.HiltDeepInsertion".Localized());

                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.GruntingBreastPlay".Localized());

                    if (Player.Corruption >= 33)
                        MarbleRapeCorruptFantasy();

                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.TauntingInsideRelease".Localized());

                    Player.Orgasm("Dick");
                }
                else
                {
                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.DifficultGirthInsertion".Localized(
                        Player.CockDescript(0)));

                    if (Player.Corruption >= 33)
                        MarbleRapeCorruptFantasy();

                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.TauntingOutsideRelease".Localized());

                    Player.Orgasm("Dick");
                }
            }
            else if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SearchingForDildo".Localized());

                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SharedDildoSetup".Localized(
                    Player.VaginaDescript(0)));

                if (Player.Corruption >= 33)
                    MarbleRapeCorruptFantasy();

                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.TauntingDildoRelease".Localized());

                Player.Orgasm("Vaginal");
            }
            else
            {
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.GenitalLackWorkaround".Localized());

                if (Player.IsCorruptEnough(33))
                    MarbleRapeCorruptFantasy();

                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.FullFistInsertion".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, -20f));
            }

            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SatisfiedHammerClaim".Localized());

            if (Flags.Get(GameFlagIds.FarmCorruptionStarted) == 0)
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.AngryWhitneyConfronts".Localized());

            Flags.Set(GameFlagIds.FarmDisabled, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void ForceFeedMarble()
        {
            ClearOutput();

            if (Player.HasPerk(PerkIds.Feeder))
                OutputText("Scenes.Marblescene.ForceFeedMarble.BetSheLovesMilking".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.NoticingFarmCrowd".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.BaringChestToCrowd".Localized());

            if (Player.BiggestBreastSize() <= 2)
                OutputText("Scenes.Marblescene.ForceFeedMarble.PressingLipsToNipple".Localized());
            else
                OutputText("Scenes.Marblescene.ForceFeedMarble.LiftingNippleToLips".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.TightlyClosedRefusal".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.TeasingHerToDrink".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.AngryGaspOpening".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.CommandingHerToDrink".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.SqueezeMakesHerGulp".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.OfferingHelpWithMess".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.ExposingBothBreasts".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.SqueezeWithoutMess".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.InevitableMessAccepted".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.PlayfulMilkingHalt".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.CowGirlBlueBalls".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.ThoroughHandMilking".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.RelaxedAwarenessReturn".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.ApologyCuppingTits".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.CrowdDissolvesWarily".Localized());

            Player.CreateStatusEffect(StatusEffectIds.NoMoreMarble, 0, 0, 0, 0);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        public partial void RapeDAHMARBLEZ();

        public partial void MarbleRapeCorruptFantasy();

        public partial void MarbleBadEndFollowup();

        public partial void MarblePoopsBaybees();

    }
}