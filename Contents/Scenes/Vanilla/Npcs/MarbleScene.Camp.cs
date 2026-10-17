using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{

    public partial class MarbleScene
    {

        public static bool TelAdreAllowed() => Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1;

        public static bool FollowerRathazul() =>

            SceneInstance<RathazulScene>.Get().FollowerRathazul();

        public void PostAddictionCampMornings(bool extra = true)
        {

            OutputText("Scenes.Marblescene.PostAddictionCampMornings.WakesToFreshMilk".Localized());

            if (Player.Corruption < 40)
            {
                if (Player.Corruption < 30)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            }

            if (Player.Libido < 40)
                Player.ApplyStats(new DynStat(GameStats.Libido, 0.1f));

            Player.RefillHunger(20f);

            Player.SlimeFeed();

            if (!extra)
                return;

            if (Player.ItemCount(ConsumableIds.MinotaurMilk) < 5)
            {
                OutputText("Scenes.Marblescene.PostAddictionCampMornings.HandedMilkBottle".Localized());

                if (Player.HasPerk(PerkIds.MarbleResistant))
                    OutputText("Scenes.Marblescene.PostAddictionCampMornings.AssuresSafeBottle".Localized());

                InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.MinotaurMilk),

                    () => ReturnToCamp(1));
            }
        }

        public void MarbleAddictionSex(bool newpage = true)
        {

            if (newpage)
                ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                MarbleGenderlessNoFuck();

                if (newpage)
                    ReturnToCamp(1);

                return;
            }

            OutputText("Scenes.Marblescene.MarbleAddictionSex.RockTogetherUndressing".Localized());

            if (Player.Tallness < 66)
                OutputText("Scenes.Marblescene.MarbleAddictionSex.MarbleBendsAndKisses".Localized());
            else if (Player.Tallness < 78)
                OutputText("Scenes.Marblescene.MarbleAddictionSex.MarbleLowersHeadKiss".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleAddictionSex.MarbleGrabsYourHead".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Marblescene.MarbleAddictionSex.NeedAlmostPalpable".Localized());

                if (Player.Tallness >= 48 && Player.Tallness <= 60)
                    OutputText("Scenes.Marblescene.MarbleAddictionSex.HeadClampedBreasts".Localized());

                OutputText("Scenes.Marblescene.MarbleAddictionSex.WarmFoldsAroundCock".Localized());
            }
            else if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Marblescene.MarbleAddictionSex.NeedPalpableMarbleGiggles".Localized(
                    Player.VaginaDescript(0)));

                Player.CuntChange(8, true);

                OutputText("Scenes.Marblescene.MarbleAddictionSex.TwoShudderAndBask".Localized());

                if (Player.Tallness >= 48 && Player.Tallness <= 60)
                    OutputText("Scenes.Marblescene.MarbleAddictionSex.HeadBetweenBreasts".Localized());

                OutputText("Scenes.Marblescene.MarbleAddictionSex.ThinkMarbleWonderful".Localized());

                OutputText("Scenes.Marblescene.MarbleAddictionSex.ParagraphBreak".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleAddictionSex.LongAfterLovemaking".Localized());

            if (Player.Tallness < 66)
                OutputText("Scenes.Marblescene.MarbleAddictionSex.SetsYouDown".Localized());

            OutputText("Scenes.Marblescene.MarbleAddictionSex.PushesArmsLengthAsks".Localized());

            Player.Orgasm("Generic");

            if (newpage)
                ReturnToCamp(1);
        }

        public void MarbleCampFuckFUCKFUCKFUCK()
        {

            MarbleSprite(true);

            ClearOutput();

            OutputText("Scenes.Camp.NotYet".Localized());

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), InteractWithMarbleAtCamp);
        }

        public void InteractWithMarbleAtCamp()
        {
            ClearOutput();

            MarbleSprite();

            Action? playtime = null;

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) != 2)
                OutputText("Scenes.Marblescene.InteractWithMarbleAtCamp.HowInteractWithMarble".Localized());
            else
                OutputText("Scenes.Marblescene.InteractWithMarbleAtCamp.AlchemistPurityQuestion".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 0)
                playtime = MarbleKidsPlaytime;

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), MarbleAppearance, "Take a closer look at the bovine girl.");

            AddButton(1, "Common.Button.Talk".Localized(), TalkWithMarbleAtCamp);

            AddButton(2, "Common.Button.Sex".Localized(), MarbleSexMenu);

            AddButton(5, "Common.Button.Children".Localized(), playtime);

            AddRowButton(1, "Give Item", GiveItem)?.DisableIf(!CanGiveItem(), "You have nothing to give her.");

            AddRowButton(1, "Get Present", MarbleGathered)?.DisableIf(
                !Player.HasStatusEffect(StatusEffectIds.MarbleHasItem),

                "Marble might find something for you again at a later time.");

            AddRowButton(1, "Get Milk", GotMilk)?.DisableIf(
                Player.ItemCount(ConsumableIds.MinotaurMilk) >= 5,

                "Marble can't give you any more milk right now.");

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Marble")
                AddButton(9, "Common.Button.SleepWith".Localized(), MarbleSleepToggle);
            else
                AddButton(9, "Common.Button.SleepAlone".Localized(), MarbleSleepToggle);

            AddButton(10, "Common.Button.BreakUp".Localized(), BreakUpWithMarble);

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) == 3 &&
                CoC.Instance.Flags.Get(GameFlagIds.MarbleRathazulCounter1) == 0)

                AddRowButton(2, "Go",

                    () => SceneInstance<MarblePurification>.Get()
                        .MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether());

            AddButton(14, "Common.Button.Back".Localized(), () => FollowersScene.Open(FollowerIds.Roles.Lover));
        }

        public void MarbleSexMenu()
        {
            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonRelease".Localized(), MarbleSexExpanded)?.DisableIf(Player.Lust < 33,

                "You're not horny enough to have sex.");

            if ((CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) < 5 &&
                 CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) >= 20) ||

                (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5 &&

                 CoC.Instance.Flags.Get(GameFlagIds.MarbleTimeSinceNursedInHours) >= 4))
            {
                OutputText("Scenes.Marblescene.MarbleSexMenu.MarbleFidgetsUncomfortably".Localized());

                AddNextButton("Common.Button.Milking".Localized(), MilkMarble);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5)

                AddNextButton("Common.Button.Nurse".Localized(), () => SceneInstance<MarblePurification>.Get().NursingFromPureMarble());

            SetExitButton("Common.Button.Back".Localized(), InteractWithMarbleAtCamp);
        }

        public void MarbleSleepToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Marble")
            {
                OutputText("Scenes.Marblescene.MarbleSleepToggle.MarbleSaysLovely".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Marble");
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleSleepToggle.MarbleFoldsArmsSighs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), InteractWithMarbleAtCamp);
        }

        public void MarbleKidsPlaytime()
        {
            RegisterMacro("marbleMultiKids", CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1);

            RegisterMacro("marbleSoleBoy", CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 1);

            ClearOutput();

            var choices = new List<int> { 1 };

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
            {
                choices.Add(2);

                choices.Add(3);

                choices.Add(4);

                choices.Add(5);
            }

            if (FollowerRathazul())
                choices.Add(7);

            choices.Add(8);

            var select = choices[Random.Shared.Next(choices.Count)];

            if (select == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1 &&
                    CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 1)
                {
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.SonClopsIntoArms".Localized(
                        Player.Mf("Daddy, daddy", "Mommy, mommy")));

                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.GameEndsBovineVictory".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.DaughterClopsIntoArms".Localized(
                        Player.Mf("Daddy, daddy", "Mommy, mommy")));

                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.GameEndsBovineTriumph".Localized());
                }

                Player.ChangeFatigue(10);
            }

            if (select == 2)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.KidsGreetAtNursery".Localized(
                    Player.Mf("Dad", "Mom")));

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.TaleOfExploits".Localized());

                if (Player.Corruption < 66)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.SkipsExplicitParts".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.StoryEndsFarewell".Localized());

                Player.ChangeFatigue(10);
            }

            if (select == 3)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleAsksWhitneyVisit".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.QuickTravelToWhitney".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.JoinHerWithKids".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.HourNearlyUpGoodbye".Localized());

                Player.ChangeFatigue(10);
            }

            if (select == 4)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleSitsOnLogs".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.BrokenStructureCry".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleTeachesBuilding".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 2)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.BothWord".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.HalfOfYourChildren".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.SomeKnackForIt".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarblePatienceLessons".Localized());
            }

            if (select == 5)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleWearySmile".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.YouSmileAndSuggest".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.CanManageProtest".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.YouSilenceHerExplain".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.ThankfulForRest".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.YouCallTheBrood".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.EagerKidQuestion".Localized(
                    Player.Mf("Daddy", "Mommy")));

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.EnergeticGrabbingKids".Localized());

                if (Player.Intelligence < 30)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.GatherReturnToCamp".Localized());
                else if (Player.Intelligence < 70)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.AwareOfSpiceHerbs".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.KnowsSpicesByHeart".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.TiredKidsAfterWork".Localized(
                    Player.Mf("Daddy", "Mommy")));

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleWaitsRested".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.LiftsYouBigHug".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.StrongWarmEmbrace".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.LoveYouThanks".Localized());

                Player.ChangeFatigue(15);

                InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.W_FRUIT),

                    () => ReturnToCamp(1));

                return;
            }

            if (select == 7)
                KidsHaveRatPlaytime();

            if (select == 8)
                KidsPlayingFarmers();

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePlayedWithKidsToday) > 0)
                Player.ApplyStats(new DynStat(GameStats.Lust, -10f));
            else
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f),

                    new DynStat(GameStats.Lust, -10f),

                    new DynStat(GameStats.Corruption, -1f));

            ReturnToCamp(1);
        }

        public void KidsPlayingFarmers()
        {
            OutputText("Scenes.Marblescene.KidsPlayingFarmers.KidRunsUpEagerly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.KidsPlayingFarmers.KidsUndecidedRole".Localized());
            else
                OutputText("Scenes.Marblescene.KidsPlayingFarmers.WithYourLittleKid".Localized());

            OutputText("Scenes.Marblescene.KidsPlayingFarmers.FarmOwner".Localized());

            if (TelAdreAllowed() && Random.Shared.Next(3) == 0)
                MarbleKidsAtTelAdre();

            OutputText("Scenes.Marblescene.KidsPlayingFarmers.MarbleJoinsRoleplay".Localized());

            OutputText("Scenes.Marblescene.KidsPlayingFarmers.ContinuePlayingUntilTired".Localized());
        }

        public void MarbleKidsAtTelAdre()
        {
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.AppleDiscussionInterrupted".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.AssureShortErrand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.StayWithMommyRefusal".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.KidsPlayAloneRefused".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.HesitantThenConvinced".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.KidsAccustomOutside".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 1)
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.SonUsedToOutside".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.DaughterOutsideWorld".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.AvoidPerverseTownParts".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.QuickTravelTelAdre".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.CuriousKidAsksAll".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.TooExcitedStayPut".Localized());
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 0)
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.TirelessGirlRunning".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.TirelessBoyRunning".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.StaysWithinYourRange".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.IngredientsThenShops".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.KidsEvenLivelier".Localized());
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 0)
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.GirlBeamsAtShop".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.BoyJoyAtShop".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.GrowingExcitement".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.EnthusiasmWithEveryVisit".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0)
            {
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.ClearsMisunderstanding".Localized());

                MarbleKidsAtTelAdreEnding();
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.RecognizeEdrynSurprised".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.EdrynAsksWhoKids".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) > 0)
                MarbleKidsMeetEdrynFuckbuddy();
            else
                MarbleKidsMeetEdryn();
        }

        public void MarbleKidsMeetEdrynFuckbuddy()
        {
            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.HerEyesGoWide".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.ChildrenStareDisbelief".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 0)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.DaughterHardToBelieve".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.SonDisbeliefNervous".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.MommyMarble".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 999 && Silly)
                TooManyCows();

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.BlurtedCongrats".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.EdrynAsksCare".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.RefuseOfferExplainCamp".Localized());

            if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.NoInterestTown".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 39)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.TooManyChildrenStrain".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.SentencePeriod".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.ComesCloserAdvice".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.GettingLateLeaves".Localized());

            MarbleKidsAtTelAdreEnding();
        }

        public void MarbleKidsMeetEdryn()
        {
            if (Player.IsChild())
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DisbeliefEars".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ChildrenStatement".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 0)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DaughterStatement".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.SonStatement".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.YoungFatherSurprise".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1)
                {
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynLooksCloser".Localized());

                    if (Player.CowScore() < 5)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DifferentSpeciesPrefix".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.YLetter".Localized());

                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.FamilialResemblance".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynLittleKidsLook".Localized());

                    if (Player.CowScore() < 5)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.OtherSpeciesPrefix".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.TLetter".Localized());

                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ResembleFather".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 999 && Silly)
                    TooManyCows();

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 39)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EyesWideAwe".Localized());
                else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 9)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.SilenceAmazed".Localized());
                else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 3)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BigFamilyRemark".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.AsksSoloCare".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BriefRunDown".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.KidsPraiseHer".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.KidsPraiseYou".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynRelievedChange".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.FirstVisitPrefix".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.IntroduceChild".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 999 && Silly)
                    TooManyCows();

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.CongratsFather".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 39)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.HugeFamilyShock".Localized());
                else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 9)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.HugeFamilySurprise".Localized());
                else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 3)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BusyRemark".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.YourPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.KidsGreetJoy".Localized());
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 0)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DaughterGreetJoy".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.SonGreetJoy".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.CentaurLikeDaddy".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynSmilesAsks".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ChildRidesEdryn".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ChildrenRideEdryn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 4)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.TooManyTakeTurns".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) < 9 && Player.IsTaur())
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.OthersRideYourBack".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.TelAdreSuggestion".Localized());

            if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.RefuseCorruptTown".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 39)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.RefuseStrain".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.CampIsSafe".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BidsFarewell".Localized());
        }

        public void TooManyCows()
        {
            OutputText("Scenes.Marblescene.TooManyCows.GargantuanFamily".Localized());

            MarbleKidsAtTelAdreEnding();
        }

        public void MarbleKidsAtTelAdreEnding()
        {
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.ReturnToCamp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CampWallGate) > 0)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.CampGate".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.TheEdgeOfThe".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.LiftedEmbracedKiss".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.RunsHugsKiss".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.KidsJoinHug".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 4)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.KidsSqueezeHug".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.SharedTimeTogether".Localized());
        }

        public void KidsHaveRatPlaytime()
        {
            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.LeisureWithKids".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.GamesReachRathazul".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.WithoutLookingAwayFrom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.ChildrenMoreCuriousThey".Localized());
            else
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.ChildAsksHim".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.AlchemyExplained".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.RathazulExperience".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.KidsUnderstandNothingOf".Localized());
            else
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.ChildStaysCurious".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.LessonFinished".Localized());
        }

        public void BreakUpWithMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.BreakUpWithMarble.ConfirmBreakup".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), DefinitelyBreakUpWithWithMarble);

            AddButton(1, "Common.Button.No".Localized(), InteractWithMarbleAtCamp);
        }

        public void DefinitelyBreakUpWithWithMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.ApproachForSpace".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.SeeYouForMilk".Localized());
            else
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.SeeYouLater".Localized());

            OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.CollectsThings".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1)
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.HerChildSuffix".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.HerChildrenSuffix".Localized());

            OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.LeavesCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BrokeUpWithMarble, 1);

            Player.RemoveStatusEffect(StatusEffectIds.CampMarble);

            Player.RemoveStatusEffect(StatusEffectIds.NoMoreMarble);

            PlayerMenu();
        }

        public void TalkWithMarbleAtCamp()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.MarbleSpecials))
                Player.CreateStatusEffect(StatusEffectIds.MarbleSpecials, 0, 0, 0, 0);

            OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.CallOverToChat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMarae) > 0 &&
                CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 1)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.VisitGoddessMarae".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 1);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.FactoryFound) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 2)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FoundDemonFactory".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 2);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 3)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FactoryHorrors".Localized());

                if (Player.HasPerk(PerkIds.OmnibusGift))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LessonGiftDemons".Localized());
                else
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ProbablyForBest".Localized());

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FactoryShutdownTale".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 3);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.MaraeQuestComplete) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1 &&

                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 4)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.HappyHelpedMarae".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 4);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.MaraeQuestComplete) >= 1 &&
                     CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2 &&

                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 4)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.MarbleShakenStory".Localized());

                if (Player.HasPerk(PerkIds.MaraesGiftFertility))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethicitePregnancyConcern".Localized());
                else if (Player.HasPerk(PerkIds.MaraesGiftStud))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethiciteBreedQuestion".Localized());

                if (Player.LustPercent < 50)
                    Player.ApplyStats(new DynStat(GameStats.Lust, 35f));

                if (!Player.HasPerk(PerkIds.MaraesGiftFertility) &&
                    !Player.HasPerk(PerkIds.MaraesGiftStud))

                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.RanAwayTale".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 4);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 5)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ForestCaveTale".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 5);
            }
            else if (TelAdreAllowed() && CoC.Instance.Flags.Get(GameFlagIds.MarbleTeladreStory) == 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.TelAdreTale".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleTeladreStory, 1);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.DefeatedZetaz) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 6)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.OldFoeCave".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 6);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 7)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethiceStronghold".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 7);
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MarbleCamptalkLevel) < 8)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethiceDefeated".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleCamptalkLevel, 8);
            }
            else
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.NoNewStories".Localized());

            OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ParagraphBreak".Localized());

            OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.NextStepAdvice".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMarae) <= 0)
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FarmLakeAdvice".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.FactoryFound) <= 0)
            {
                if (Player.Level < 3)
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.NotReadyForMountains".Localized());
                else
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ReadyForFactory".Localized());
            }
            else if (CoC.Instance.Flags.Get(GameFlagIds.FactoryFound) >= 1 &&
                     CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FinishFactory".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) >= 1 &&
                     CoC.Instance.Flags.Get(GameFlagIds.MaraeQuestComplete) <= 0 &&

                     CoC.Instance.Flags.Get(GameFlagIds.MetMaraeCorrupted) <= 0 &&

                     CoC.Instance.Flags.Get(GameFlagIds.CorruptedMaraeKilled) <= 0)

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ReturnToMarae".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) == 0 || !TelAdreAllowed())
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ExploreWorld".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) == 0 && TelAdreAllowed())
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.DeepWoodsSecrets".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) > 0 && !TelAdreAllowed())
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.PatrolUniforms".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.DefeatedZetaz) == 0)

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ExploreCave".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.DefeatedZetaz) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) == 0)

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.HighMountains".Localized());
            else if (CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) > 0 &&
                     CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) == 0)

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.StopLethice".Localized());
            else
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.MainStoryDone".Localized());

            InteractWithMarbleAtCamp();
        }

        public void MarbleSexExpanded()
        {
            MarbleCampFuckFUCKFUCKFUCK();
        }

        public void GotMilk()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.GotMilk.AskMilkBottle".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.MinotaurMilk),

                InteractWithMarbleAtCamp);
        }

        public void MarbleGathered()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleGathered.AskSupplies".Localized());

            var itemStage = Player.StatusEffectValue(StatusEffectIds.MarbleHasItem, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) != 1 && itemStage <= 4)
                InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.VitalityTincture),

                    InteractWithMarbleAtCamp);
            else if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) != 1 && itemStage <= 7)

                InventoryScene.TakeItem(ItemRegistry.Create(ArmorIds.LeatherArmor),

                    InteractWithMarbleAtCamp);
            else
                InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.LaBova),

                    InteractWithMarbleAtCamp);

            Player.RemoveStatusEffect(StatusEffectIds.MarbleHasItem);
        }

        public bool CanGiveItem()
        {
            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir), 1))
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) == 0)
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft), 1))
                    return true;
            }
            else
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink), 1))
                    return true;

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargePink), 1))
                    return true;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedLaBova), 1) &&
                CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) == 1)

                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5)
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 1))
                    return true;

                if (Player.HasItem(ItemRegistry.Create(ItemIds.Reducto), 1))
                    return true;

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid), 1))
                    return true;
            }

            return false;
        }

        public void GiveItem()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.GiveItem.ChooseItemGift".Localized());

            ClearButtons();

            AddButton(1, ItemRegistry.Create(ConsumableIds.OviElixir).Name, MarbleOvulatesLikeMadDawg)

                ?.DisableIf(!Player.HasItem(ConsumableIds.OviElixir), "You don't have that item.");

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) == 0)
                AddButton(2, ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft).Name, MarbleDigsDraftsYo)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.PurifiedIncubiDraft), "You don't have that item.");
            else
            {
                AddButton(3, ItemRegistry.Create(ConsumableIds.EggPink).Name, MarblepinkEgg)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.EggPink), "You don't have that item.");

                AddButton(4, ItemRegistry.Create(ConsumableIds.EggLargePink).Name, MarbleLPinkEgg)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.EggLargePink), "You don't have that item.");
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) < 2)
                AddButton(5, ItemRegistry.Create(ConsumableIds.LaBova).Name, GiveMurbleProBova)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.LaBova), "You don't have that item.");

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) == 1)
                AddButton(6, ItemRegistry.Create(ConsumableIds.PurifiedLaBova).Name,

                    SceneInstance<MarblePurification>.Get().GiveMarblePureQuestLabova)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.PurifiedLaBova), "You don't have that item.");

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5)
            {
                AddButton(7, ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk).Name,

                    SceneInstance<MarblePurification>.Get().PurifiedSuccubusMilkForPureMarbles)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.PurifiedSuccubiMilk), "You don't have that item.");

                AddButton(8, ItemRegistry.Create(ItemIds.Reducto).Name,

                    SceneInstance<MarblePurification>.Get().PureMurbleUsesReducto)

                    ?.DisableIf(!Player.HasItem(ItemIds.Reducto), "You don't have that item.");

                AddButton(0, ItemRegistry.Create(ConsumableIds.Lactaid).Name,

                    SceneInstance<MarblePurification>.Get().LactaidForPureMurble)

                    ?.DisableIf(!Player.HasItem(ConsumableIds.Lactaid), "You don't have that item.");
            }
            else if (Player.HasPerk(PerkIds.MarblesMilk))
                AddButton(0, ItemRegistry.Create(ConsumableIds.Lactaid).Name, GiveMarbleLactaid);
            else

                AddButtonDisabled(0, ItemRegistry.Create(ConsumableIds.Lactaid).Name);

            AddButton(14, "Common.Button.Back".Localized(), InteractWithMarbleAtCamp);
        }

        public void GiveMarbleLactaid()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));

            OutputText("Scenes.Marblescene.GiveMarbleLactaid.ShowLactaidBottle".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.GiveMarbleLactaid.LiftsToChest".Localized());
            else if (Player.Tallness > 72)
                OutputText("Scenes.Marblescene.GiveMarbleLactaid.DrinkNowPrompt".Localized());
            else
                OutputText("Scenes.Marblescene.GiveMarbleLactaid.QuickLungeDrink".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleLactaid.TastierMilk".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, 1f),

                new DynStat(GameStats.Toughness, 1f),

                new DynStat(GameStats.Intelligence, 1f));

            ReturnToCamp(1);
        }

        public static bool IsFetishGear() =>

            Player.ArmorName == "bondage patient clothes" ||

            Player.ArmorName == "crotch-revealing clothes" ||

            Player.ArmorName == "cute servant's clothes" ||

            Player.ArmorName == "maid's clothes" ||

            Player.ArmorName == "servant's clothes";
    }
}
