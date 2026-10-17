using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Internals;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    public partial class MarbleScene
    {

        public const int MarbleIncubation = 368;

        public static bool MarbleKnockUpChance(int floor = 20, int ceiling = 60)
        {
            if (MarblePregType != 0)
                return false;

            return RandomUtils.Chance(Math.Clamp(floor + Player.VirilityQ(), floor, ceiling));
        }

        public static void MarbleKnockUp(int type, int incubation)
        {
            if (MarblePregType != 0)
                return;

            var notice = MarblePregRawType & PregnancyNoticeMask;

            CoC.Instance.Flags.Set(GameFlagIds.MarblePregnancyType, notice + type);

            CoC.Instance.Flags.Set(GameFlagIds.MarblePregnancyIncubation, incubation);
        }

        public bool PcPregWithMarblesKids()
        {

            return Player.Pregnancy.Type == PregnancyIds.Marble && Player.Pregnancy.Incubation <= 280;
        }

        public bool MarblePregWithPCKids()
        {

            return MarblePregType == PregnancyPlayerType && MarblePregIncubation <= 280;
        }

        public void MarbleCuddlin()
        {

            OutputText("Scenes.Marblescene.MarbleCuddlin.MarbleReachesOver".Localized());

            if (!PcPregWithMarblesKids() && !MarblePregWithPCKids())
            {
                if (Player.Tallness <= 78)
                    OutputText("Scenes.Marblescene.MarbleCuddlin.HeadOntoHerChest".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleCuddlin.ArmsWrappedAroundYou".Localized());
            }
            else
                if (MarblePregWithPCKids() && !PcPregWithMarblesKids())
                    OutputText("Scenes.Marblescene.MarbleCuddlin.EarToSwollenBelly".Localized());
                else
                    if (PcPregWithMarblesKids())
                        OutputText("Scenes.Marblescene.MarbleCuddlin.PetsYourBelly".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleCuddlin.HandsOnEachBelly".Localized());
        }

        public void AtNightAskMarbleForSomeSexMaybe()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.TurnToFaceMarble".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 0)
                OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.AnnoyedBackOfHead".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 20)
                {
                    OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.FoolAroundRefused".Localized());

                    MarbleCuddlin();
                }
                else
                {
                    OutputText("Scenes.Marblescene.AtNightAskMarbleForSomeSexMaybe.FoolAroundAccepted".Localized());

                    MarbleNightSexIntro(false);

                    return ;
                }

            DoNext(PlayerMenu);
        }

        public void MarbleNightLovemaking()
        {

            OutputText("Scenes.Marblescene.MarbleNightLovemaking.HintAtIntimacy".Localized());

            if (MarblePregWithPCKids())
            {
                OutputText("Scenes.Marblescene.MarbleNightLovemaking.PatsPregnantBelly".Localized());

                OutputText("Scenes.Marblescene.MarbleNightLovemaking.CradleHerBelly".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightLovemaking.StillHasNeeds".Localized());

                OutputText("Scenes.Marblescene.MarbleNightLovemaking.NoPentUpNeeds".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleNightLovemaking.UsherHerInside".Localized());

            var fitting = Player.HasCock() && Player.CockThatFits(MarbleCuntCapacity()) < 0 ? "fitting " : "";

            AddButton(0, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), MarbleNightMassage, "Help the cow unwind and get off with rubbing and kissing.");

            if (!Player.HasCock() || fitting != "")
                AddButtonDisabled(1, "Marble Rides", "Requires a " + fitting + "cock.", "Marble Rides");
            else
                AddButton(1, "Common.Button.MarbleRides".Localized(), MarbleNightRiding, "Let her dominantly take charge while you enjoy yourself.");

            AddButton(2, "Common.Button.Sex".Localized(), () => MarbleNightSexIntro());
        }

        public void MarbleNightRefusal()
        {

            OutputText("Scenes.Marblescene.MarbleNightRefusal.DeclineHerOffer".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRefusal.SettleBesideHer".Localized());

            DoNext(PlayerMenu);
        }

        public void MarbleNightSexIntro(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.ParagraphBreak".Localized());

            MarbleSprite(true);

            int x = Player.CockThatFits(MarbleCuntCapacity());

            if (x < 0)
                x = 0;
            else
                if (Player.Gender == Gender.Male && Player.CockArea(x) > MarbleCuntCapacity())
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.WishForIntimacy".Localized(
                        Player.CockDescript(x)
                    ));

                    DoNext(PlayerMenu);
                }

            OutputText("Scenes.Marblescene.MarbleNightSexIntro.WhisperInEar".Localized());

            if (Player.Tallness < 60)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.RollOntoBack".Localized());

                OutputText("Scenes.Marblescene.MarbleNightSexIntro.HeadBetweenBreasts".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));

                if (Player.CockTotal() > 0)
                {
                    OutputText(Player.MultiCockDescriptLight());

                    if (Player.HasVagina())
                        OutputText("Scenes.Marblescene.MarbleNightSexIntro.AndYourWord".Localized());
                }

                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript());

                OutputText("Scenes.Marblescene.MarbleNightSexIntro.CaressAndWhisper".Localized());

                OutputText("Scenes.Marblescene.MarbleNightSexIntro.UndressedTurnOver".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightSexIntro.HandInsideClothes".Localized());

                if (Player.CockTotal() > 0)
                {
                    OutputText(Player.MultiCockDescriptLight());

                    if (Player.HasVagina())
                        OutputText("Scenes.Marblescene.MarbleNightSexIntro.AndYourWordAgain".Localized());
                }

                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript(0));

                OutputText("Scenes.Marblescene.MarbleNightSexIntro.SlipHandInside".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.RubHerThenLower".Localized(
                        MarbleCock()
                    ));

                OutputText("Scenes.Marblescene.MarbleNightSexIntro.FingersDownWomanhood".Localized());
            }

            if (Player.Gender == Gender.Hermaphrodite)
            {
                if (Player.CockArea(x) <= MarbleCuntCapacity())
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.AsksWhichPart".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Dick".Localized(), MarbleNightSexDudes);

                    AddButton(1, "Common.Button.Vagina".Localized(), MarbleNightSexChicks);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexIntro.TooBigForHer".Localized(
                        Player.CockDescript(x),

                        Player.VaginaDescript(0)
                    ));

                    DoNext(() => MarbleNightSexChicks());
                }
            }
            else
                if (Player.Gender == Gender.Male)
                    DoNext(() => MarbleNightSexDudes());
                else
                    DoNext(() => MarbleNightSexChicks());
        }

        public void MarbleNightMassage()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightMassage.PushHerOntoBed".Localized());

            OutputText("Scenes.Marblescene.MarbleNightMassage.HelpHerRelax".Localized());

            OutputText("Scenes.Marblescene.MarbleNightMassage.PullTopOff".Localized());

            OutputText("Scenes.Marblescene.MarbleNightMassage.MassageNotKiss".Localized());

            OutputText("Scenes.Marblescene.MarbleNightMassage.HiddenUnderFat".Localized());

            DoNext(() => MarbleNightMassageEnd());
        }

        public void MarbleNightMassageEnd()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightMassageEnd.SuddenDeepKiss".Localized());

            OutputText("Scenes.Marblescene.MarbleNightMassageEnd.FingersToNethers".Localized());

            OutputText("Scenes.Marblescene.MarbleNightMassageEnd.ReleaseFromGrip".Localized((CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0?" and some white spots on her cock":"")));

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, 2f));

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 10);

            DoNext(PlayerMenu);
        }

        public void MarbleNightRiding()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightRiding.StripShowRequest".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.WontHaveToMove".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.SheWasntLying".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.GaspPainPleasure".Localized(
                MarbleCuntCapacity()
            ));

            OutputText("Scenes.Marblescene.MarbleNightRiding.GrindSlowsDown".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.PressOnNavel".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.ArchesAndBounces".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.GrabsYourShoulders".Localized(
                MarbleCuntCapacity()
            ));

            OutputText("Scenes.Marblescene.MarbleNightRiding.SlamsDownDeep".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRiding.PinchAndGiggle".Localized());

            Player.Orgasm("Dick");

            MarblePreggoChance();

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 10);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (Player.Libido >= 50)
                DoNext(() => MarbleNightRidingCont());
            else
                DoNext(() => MarbleNightEnd());
        }

        public void MarbleNightRidingCont()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.SlowHipMotion".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.CaughtOffGuard".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.LovesHowYouNurse".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.MuffledResponse".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.SighsAndStrokes".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.LoudSquelching".Localized(
                MarbleCuntCapacity()
            ));

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.DivineMovements".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.TeatPulledOut".Localized((CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) > 0? "The vigorous motion of your lover makes her other three nipples flick across your face while you drink. " : "")));

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.SuckTeatDeep".Localized((CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) > 0? "all four of her nipples plus " : "")));

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.PantsAndMoans".Localized());

            OutputText("Scenes.Marblescene.MarbleNightRidingCont.RockAndShudder".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 0);

            HighLibidoTryst = true;

            DoNext(PlayerMenu);
        }

        public void MarbleNightLustEnd()
        {
            ClearOutput();

            HighLibidoTryst = false;

            OutputText("Scenes.Marblescene.MarbleNightLustEnd.SoreButContent".Localized());

            OutputText("Scenes.Marblescene.MarbleNightLustEnd.GoodMorningKiss".Localized());

            DoNext(PlayerMenu);
        }

        public void MarbleNightEnd()
        {
            ClearOutput();

            if (!MarblePregWithPCKids() && !PcPregWithMarblesKids())
            {
                OutputText("Scenes.Marblescene.MarbleNightEnd.RestAndRecover".Localized());

                OutputText("Scenes.Marblescene.MarbleNightEnd.NodInSilence".Localized());

                OutputText("Scenes.Marblescene.MarbleNightEnd.PlayfulWink".Localized());

                OutputText("Scenes.Marblescene.MarbleNightEnd.CurlUpTogether".Localized());

                OutputText("Scenes.Marblescene.MarbleNightEnd.DriftIntoSleep".Localized(
                    MarbleCuntCapacity()
                ));
            }
            else
                if (MarblePregWithPCKids())
                {
                    OutputText("Scenes.Marblescene.MarbleNightEnd.RubsSwollenBelly".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightEnd.CannotBeUpset".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightEnd.CheckTheCalf".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightEnd.PullUpPillows".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightEnd.ListensToBaby".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightEnd.AsksWhatSheHears".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightEnd.RelaxedHeartbeat".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightEnd.PullUpBlankets".Localized());
                }

            DoNext(PlayerMenu);
        }

        public void MarbleNightSexDudes()
        {
            ClearOutput();

            int x = Player.CockThatFits(MarbleCuntCapacity());

            if (x < 0)
                x = 0;

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 0);

            OutputText("Scenes.Marblescene.MarbleNightSexDudes.SlideTipInside".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length > 8)
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.EndOfTheLine".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.FullyEnveloped".Localized());

            OutputText("Scenes.Marblescene.MarbleNightSexDudes.EyesLocked".Localized());

            if (Player.Corruption + Player.Libido < Player.StatusEffectValue(StatusEffectIds.Marble, 4) + 10)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.SheTakesControl".Localized());

                if (Player.Tallness < 66)
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.BackIntoBosom".Localized());
                else
                    if (Player.Tallness < 84)
                        OutputText("Scenes.Marblescene.MarbleNightSexDudes.AgainstHerPillows".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    else
                        if (Player.BiggestBreastSize() >= 4)
                            OutputText("Scenes.Marblescene.MarbleNightSexDudes.LandsOnHerFace".Localized(
                                Appearance.BiggestBreastSizeDescript(Player)
                            ));
                        else
                            OutputText("Scenes.Marblescene.MarbleNightSexDudes.FlattensHerBreasts".Localized());

                OutputText("Scenes.Marblescene.MarbleNightSexDudes.GrinsAtHerFrown".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.DeepFastThrusts".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.ToysWithNipples".Localized(
                        Appearance.BiggestBreastSizeDescript(Player),

                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.TooMuchToBear".Localized(
                        Player.CockDescript(x)
                    ));
            }

            OutputText("Scenes.Marblescene.MarbleNightSexDudes.SheClenchesTight".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.CriesOutInJoy".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.FilledToBursting".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.OfCourseWord".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.RemainingCocks".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleNightSexDudes.OtherToolWord".Localized());

                OutputText("Scenes.Marblescene.MarbleNightSexDudes.UnleashesOwnLoad".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                OutputText("Scenes.Marblescene.MarbleNightSexDudes.TrappedBetweenBodies".Localized(
                    MarbleCock()
                ));

            MarblePreggoChance();

            MarbleSexFinish();
        }

        public void MarbleNightSexChicks()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) == 0)
            {
                OutputText("Scenes.Marblescene.MarbleNightSexChicks.LiftOntoDoubleDildo".Localized(
                    Player.VaginaDescript(0)
                ));

                Player.CuntChange(14, true);

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.AdjustToShaftFeeling".Localized());

                if (Player.Corruption + Player.Libido < Player.StatusEffectValue(StatusEffectIds.Marble, 4) + 10)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GrinAndLiftHips".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.SlowDownOffer".Localized());

                    if (Player.BiggestBreastSize() >= 4)
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.HandsOffPaceYourself".Localized(
                            Appearance.BiggestBreastSizeDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.TeaseChestBegin".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.BounceMoanOnDildo".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GaspAtBreastPlay".Localized());

                    if (Player.BiggestBreastSize() >= 4)
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.FlickChestInRhythm".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.GripHandsToChest".Localized(
                            MarbleNips()
                        ));

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.RapidPistonNearEdge".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.MutualOrgasmClench".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.YourLoadOnChest".Localized());

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.ShudderThenCollapse".Localized());
            }
            else
            {
                Player.SlimeFeed();

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.GuideTipAndLower".Localized(
                    Player.VaginaDescript(0),

                    MarbleCock(),

                    Player.VaginaDescript(0),

                    MarbleCock()
                ));

                if (Player.VaginalCapacity() > CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) * CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness))
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.HiltedShudderPraise".Localized());
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.BottomOutSadAsk".Localized(
                        MarbleCock()
                    ));

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GrinFitExtraTease".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.AdjustToHerWithin".Localized());

                if (Player.Corruption + Player.Libido < Player.StatusEffectValue(StatusEffectIds.Marble, 4) + 10)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GrinLiftOnMember".Localized(
                        Player.VaginaDescript(0),

                        MarbleCock()
                    ));

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.BreathCatchOffer".Localized());

                    if (Player.BiggestBreastSize() >= 4)
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.LetYouSetPace".Localized(
                            Appearance.BiggestBreastSizeDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.TeaseNippleBegin".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.MoanAndRideHer".Localized(
                        MarbleCock()
                    ));

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.GaspThenBreastPlay".Localized());

                    if (Player.BiggestBreastSize() >= 4)
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.ToyAtMatchingPace".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.PullHandsToChest".Localized(
                            MarbleNips()
                        ));

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.PistonHerToEdge".Localized(
                        MarbleCock()
                    ));
                }

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.SeedFillingOrgasm".Localized(
                    Player.VaginaDescript(0),

                    MarbleCock()
                ));

                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.SameTimePrefix".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Marblescene.MarbleNightSexChicks.EachOfPrefix".Localized());

                    OutputText("Scenes.Marblescene.MarbleNightSexChicks.LoadSpraysOntoChest".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleNightSexChicks.CollapseAfterShudder".Localized());

                Player.KnockUp(PregnancyIds.Marble, MarbleIncubation, 150);

                Player.CuntChange(CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) * CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength), true);
            }

            MarbleSexFinish();
        }

        public void MarblePreggoChance()
        {

            if (MarbleKnockUpChance() && (Player.HasPerk(PerkIds.MarblesMilk) || CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5))

                MarbleKnockUp(PregnancyPlayerType, 720);
        }

        public void MarbleSexFinish()
        {

            OutputText("Scenes.Marblescene.MarbleSexFinish.RollAsideToSleep".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
        }

        public void MarbleNomNoms()
        {
            ClearOutput();

            MarbleSprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
            {
                Player.SlimeFeed();

                if (Player.Corruption < 30)
                    OutputText("Scenes.Marblescene.MarbleNomNoms.ReluctantAgree".Localized(
                        MarbleCock()
                    ));
                else
                    OutputText("Scenes.Marblescene.MarbleNomNoms.TastySuckAppeal".Localized(
                        MarbleCock()
                    ));

                OutputText("Scenes.Marblescene.MarbleNomNoms.SkirtUpErectReveal".Localized(
                    MarbleCock()
                ));

                if (Player.HasStatusEffect(StatusEffectIds.SlimeCraving) || Player.Libido + Player.Corruption - Player.Intelligence > 35)
                {
                    OutputText("Scenes.Marblescene.MarbleNomNoms.MuskyHeadFog".Localized(
                        MarbleCock()
                    ));

                    OutputText("Scenes.Marblescene.MarbleNomNoms.SighAboveHandPlaced".Localized(
                        MarbleCock()
                    ));

                    OutputText("Scenes.Marblescene.MarbleNomNoms.SaltyGaspSwallow".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -0.2f), new DynStat(GameStats.Libido, -0.2f));
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleNomNoms.SteadyYourThoughts".Localized(
                        MarbleCock()
                    ));

                    OutputText("Scenes.Marblescene.MarbleNomNoms.RealizeItWasMarble".Localized(
                        MarbleCock()
                    ));
                }
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleNomNoms.SkirtUpWetView".Localized());

                OutputText("Scenes.Marblescene.MarbleNomNoms.HoneyTasteSpur".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleNomNoms.MarbleThanksInvite".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Random.Shared.Next(10) + Player.Libido / 10)));

            DoNext(PlayerMenu);
        }

        public void MarbleDigsDraftsYo()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

            OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.HandBottleSheStudies".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) > 60)
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.SmileCorruptionOffer".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.UncertainDemonRefusal".Localized());

            OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.DropPotionGrabCrotch".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) > 50)
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.BigWord".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.NervousWord".Localized());

            OutputText("Scenes.Marblescene.MarbleDigsDraftsYo.SmileHermNotice".Localized());

            DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.MarbleDickType, 1);

            CoC.Instance.Flags.Set(GameFlagIds.MarbleDickLength, 7);

            CoC.Instance.Flags.Set(GameFlagIds.MarbleDickThickness, 2);
        }

        public void MarblepinkEgg()
        {

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggPink));

            MarblePEggEffects();
        }

        public void MarbleLPinkEgg()
        {

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePink));

            MarblePEggEffects();
        }

        public void MarblePEggEffects()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarblePEggEffects.HandPinkEgg".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) > 50)
                OutputText("Scenes.Marblescene.MarblePEggEffects.CockTiredQuestion".Localized());
            else
                OutputText("Scenes.Marblescene.MarblePEggEffects.KeepCockQuestion".Localized());

            OutputText("Scenes.Marblescene.MarblePEggEffects.WatchAbsorbRestore".Localized(
                MarbleCock()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.MarbleDickType, 0);

            CoC.Instance.Flags.Set(GameFlagIds.MarbleDickLength, 0);

            CoC.Instance.Flags.Set(GameFlagIds.MarbleDickThickness, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void MarbleOvulatesLikeMadDawg()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) < 100 && Player.Pregnancy.Type == PregnancyIds.PlayerFather)
            {
                OutputText("Scenes.Marblescene.MarbleOvulatesLikeMadDawg.RefuseElixirNursery".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (Player.IsPregnant())
                {
                    OutputText("Scenes.Marblescene.MarbleOvulatesLikeMadDawg.BellyRumbleAdvance".Localized());

                    var newIncubation = Player.Pregnancy.Incubation - (40 + Random.Shared.Next(30));

                    if (newIncubation <= 0)
                        newIncubation = 1;

                    Player.KnockUp(Player.Pregnancy.Type, newIncubation, 100, 1);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleOvulatesLikeMadDawg.BellyFullEggsSoon".Localized());

                    Player.KnockUp(PregnancyIds.OvielixirEggs, PregnancyIds.OvielixirEggsIncubation + 46, 100, 1);
                }

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir));

            DoNext(() => ReturnToCamp(1));
        }

        public float MarbleCuntCapacity()
        {

            float size = 36;

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) >= 1)
                size += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) >= 2)
                size += 20;

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 0)
                size += 14;

            return size;
        }

        public void GiveMurbleProBova()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) == 0)
            {
                OutputText("Scenes.Marblescene.GiveMurbleProBova.HandProBovaFirst".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), GiveMarbleTheProBovas4Sho);

                AddButton(1, "Common.Button.No".Localized(), GiveItem);
            }
            else
            {
                OutputText("Scenes.Marblescene.GiveMurbleProBova.SecondProBovaBottle".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), GiveMarbleTheProBovas4Sho);

                AddButton(1, "Common.Button.No".Localized(), GiveItem);
            }
        }

        public void GiveMarbleTheProBovas4Sho()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LaBova));

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) == 0)
            {
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.NodDownAndGasp".Localized());

                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.TestNewNipples".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleBovaLevel, 1);

                MarbleAffection += 4;

                MarbleAddiction += 4;
            }
            else
            {
                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.DeepBreathLoveVow".Localized());

                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.ItchyFurGrowsTaller".Localized());

                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.PainHandsOverFace".Localized());

                OutputText("Scenes.Marblescene.GiveMarbleTheProBovas4Sho.ChangesStopGasp".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleBovaLevel, 2);

                MarbleSprite();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public string MarbleNip()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) > 0)
                return "quad-nipple";
            else
                return "nipple";
        }

        public void MarbleAppearance()
        {
            ClearOutput();

            ShowImage("monster-marble");

            OutputText("Scenes.Marblescene.MarbleAppearance.MarbleIsAPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) == 0)
                OutputText("Scenes.Marblescene.MarbleAppearance.HeightToken".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) == 1)
                    OutputText("Scenes.Marblescene.MarbleAppearance.HeightTallToken".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleAppearance.HeightTallestToken".Localized());

            OutputText("Scenes.Marblescene.MarbleAppearance.TallCowGirlDescription".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) <= 1)
                OutputText("Scenes.Marblescene.MarbleAppearance.NormalHumanFace".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleAppearance.BovineHumanFace".Localized());

            OutputText("Scenes.Marblescene.MarbleAppearance.BrownHairCowEars".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 20)
                OutputText("Scenes.Marblescene.MarbleAppearance.SmallHornPair".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 50)
                    OutputText("Scenes.Marblescene.MarbleAppearance.MediumHornPair".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleAppearance.LargeHornPair".Localized());

            OutputText("Scenes.Marblescene.MarbleAppearance.WideThighsAndRear".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) <= 1)
                OutputText("Scenes.Marblescene.MarbleAppearance.HumanThighFurPrefix".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleAppearance.OddJointedHoovesClause".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 15)
                OutputText("Scenes.Marblescene.MarbleAppearance.PillowyBreastPair".Localized(
                    MarbleNip(),

                    MarbleBreastSize()
                ));
            else
                if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 25)
                    OutputText("Scenes.Marblescene.MarbleAppearance.LargeBreastPair".Localized(
                        MarbleNip(),

                        MarbleBreastSize()
                    ));
                else
                    if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 35)
                        OutputText("Scenes.Marblescene.MarbleAppearance.BasketballBreastPair".Localized(
                            MarbleNip(),

                            MarbleBreastSize()
                        ));
                    else
                        OutputText("Scenes.Marblescene.MarbleAppearance.BasketballBreastPairLarger".Localized(
                            MarbleNip(),

                            MarbleBreastSize()
                        ));

            switch (Player.Pregnancy.Event)
            {
                case 2:
                {
                OutputText("Scenes.Marblescene.MarbleAppearance.GainedSomeWeight".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellySlightSwelling".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellyObviousPregnancy".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellyVeryPregnant".Localized());
                }

                break;

                case 6:
                {
                OutputText("Scenes.Marblescene.MarbleAppearance.BellyExtremeQuiver".Localized());
                }

                break;
            }

            if ((CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) < 5 && CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) >= 20) || (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.MarbleTimeSinceNursedInHours) >= 4))
                OutputText("Scenes.Marblescene.MarbleAppearance.FidgetNeedsMilking".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                OutputText("Scenes.Marblescene.MarbleAppearance.GrownSinceArrival".Localized(
                    MarbleCock(),

                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength)),

                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness))
                ));

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 50)
                OutputText("Scenes.Marblescene.MarbleAppearance.PussySmallClit".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) <= 75)
                    OutputText("Scenes.Marblescene.MarbleAppearance.CuntMediumClit".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleAppearance.FuckHoleClitParted".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.MarbleSpecials, 0) == 0)
                OutputText("Scenes.Marblescene.MarbleAppearance.VirginBumHole".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleAppearance.BumHoleBetweenCheeks".Localized());

            DoNext(() => InteractWithMarbleAtCamp());
        }

        public void GiveMarbleTailjobRelease()
        {
            ClearOutput();

            MarbleSprite(true);

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.NotInMoodAlternative".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.GrinTailUpLeg".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.TailBetweenThighs".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.MoistureSufficient".Localized(
                NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength))
            ));

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.HerHandsJoinIn".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.CoilsOverErection".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.QuickClimaxAfterward".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.WarmTailStroking".Localized());

            if (Player.IsCorruptEnough(70))
            {
                OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.ProwessMarkingProof".Localized());

                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake || Player.HasLongTongue())
                    OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.TasteBothSecretions".Localized());

                if (Player.HasLongTongue())
                    OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.ShowForMarble".Localized());

                OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.TeaseAgainQuestion".Localized());

                OutputText("Scenes.Marblescene.GiveMarbleTailjobRelease.RepeatWouldBeNice".Localized());

                Player.SlimeFeed();
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 0);

            DoNext(PlayerMenu);
        }

        public void MilkMarble()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleMilkedBefore) < 1)
            {
                OutputText("Scenes.Marblescene.MilkMarble.MilkBrightEyeGreeting".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.HarmlessStallVisit".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.HandHoldWarpFarm".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.LoverShowsStall".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.AmorousGuideFurnitureTour".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.CuriousAskProgress".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.ApologizeSuggestMilking".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.PerkUpPullCups".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) < 1)
                    OutputText("Scenes.Marblescene.MilkMarble.TwoWord".Localized());
                else
                    OutputText("Scenes.Marblescene.MilkMarble.EightWord".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.AttachSuctionCups".Localized(
                    MarbleNips()
                ));

                OutputText("Scenes.Marblescene.MilkMarble.AsksWhatsInMind".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleMilkedBefore, 1);
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarble.EscortToStallOffer".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.MarblePleasedAgrees".Localized());

                OutputText("Scenes.Marblescene.MilkMarble.HookedUpSheAsks".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) >= 5)
                CoC.Instance.Flags.Set(GameFlagIds.MarbleTimeSinceNursedInHours, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Cunnilingus".Localized(), MilkMarbleCunnilingling);

            if (Player.HasCockThatFits(MarbleCuntCapacity()) && (Player.IsBiped() || Player.HasTailInsteadOfLegs() || Player.IsGoo()))
                AddButton(1, "Common.Button.FuckHer".Localized(), MilkMarbleFuckDatCowPussy);

            if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0 && (Player.IsBiped() || Player.IsGoo()))
                AddButton(2, "Scenes.Tamani.TamaniMaleRepeatEncounter.ButtonTakeHer".Localized(), MilkMarbleTakeHerDick);

            AddButton(3, "Common.Button.HerOnBar".Localized(), MilkMarbleOnTheBar);

            AddButton(14, "Common.Button.Leave".Localized(), () => MilkMarbleNoMilking());
        }

        public void MilkMarbleCunnilingling()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.SuggestLickingWhileMilked".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.HugeSmileAgreement".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.ChairSeatLegsSpread".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBovaLevel) < 1)
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.SemiHumanWord".Localized());
            else
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.InhumanWord".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LegsClearView".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.GetShowOnRoad".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.KneesCrawlForward".Localized());
            else
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BodyLowInchForward".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.AnimalStalkingApproach".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.HoofLegArmRest".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BowStrandsGift".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.DeserveItReply".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.UnwrapBowPresent".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.TryItNowInvite".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.NoSecondAsking".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.HipsJerkTugging".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LongDrawnOutCry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.FirstFlickSqueeze".Localized(
                    MarbleCock()
                ));
            else
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.FirstFlickChairGrip".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LaughSuggestPause".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BiteMeNowDemand".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.CannotRefuseRequest".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.BovineGratefulCry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.SpurtOverLap".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.MilkersAmplifyPleasure".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.AskWasItGood".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.LovelyThanksNotice".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.RetieBowForLater".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleCunnilingling.AwkwardPeckFarewell".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 10);

            var pLust = (int)(20 + ((Player.Libido + Player.Corruption) / 10));

            Player.ApplyStats(new DynStat(GameStats.Lust, pLust));

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            DoNext(() => ReturnToCamp(2));
        }

        public void MilkMarbleFuckDatCowPussy()
        {
            ClearOutput();

            var x = Player.CockThatFits(MarbleCuntCapacity());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.WonderAloudIntercourse".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.TemptingOfferInspect".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.FlourishArmorReveal".Localized(
                Player.CockDescript(x)
            ));

            if ((Player.HasPerk(PerkIds.BulgeArmor) || Player.ModArmorName == "backless female teacher's clothes" || Player.ModArmorName == "bridle bit and saddle set" || Player.ModArmorName == "headdress, necklaces, and many body-chains" || Player.ModArmorName == "bondage patient clothes" || Player.ModArmorName == "crotch-revealing clothes" || Player.ModArmorName == "cute servant's clothes" || Player.ModArmorName == "maid's clothes" || Player.ModArmorName == "servant's clothes") && Player.HasCock())
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.AppreciateShowCloser".Localized());
            else
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.WillWorkInspect".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.StepForwardShowGoods".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SquatCarefulLookover".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SeriousApprovalVerdict".Localized());

            if (Player.Tallness < 68)
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.DeepHugLaughter".Localized(
                    Player.CockDescript(x)
                ));
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.ReassureDuringHysterics".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SettleDownReturnHug".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.BreakAwayToBench".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.HuskyVoiceAskReady".Localized());
            }

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.GrinAlwaysReady".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.WordsBarelyEngulf".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.HandsPressStomachRock".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MilkSpurtEachThrust".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.StopsMovingWorried".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
            {
                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SitUpMilkingTakeover".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MachineFailsHandDrain".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.FlowSlowsToTrickle".Localized());

                Player.Orgasm("Generic");

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 15);

                Player.ApplyStats(new DynStat(GameStats.Libido, .3f));

                if (Player.Corruption < Player.StatusEffectValue(StatusEffectIds.Marble, 4))
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                Player.ApplyStats(new DynStat(GameStats.Toughness, .1f));

                Player.ApplyStats(new DynStat(GameStats.Strength, .1f));
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.Marble, 4) > 30)
                {
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.TaintedFluidCraving".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SpeakingThenQuiet".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.RubbingNotEnough".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MutualOrgasmCry".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SouthernVoiceInterrupt".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.SurprisedStareTense".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MetalSlamWarning".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.LaughReattachCups".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 5);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.AssureBucketContinue".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.RollSoftFleshMoans".Localized(
                        MarbleNips()
                    ));

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.GaspOwnHandMilking".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.MinutesToOrgasmPeak".Localized());

                    OutputText("Scenes.Marblescene.MilkMarbleFuckDatCowPussy.TripleClimaxHour".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 10);
                }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            MarblePreggoChance();

            DoNext(() => ReturnToCamp(2));
        }

        public void MilkMarbleTakeHerDick()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.AskPenetrateWhileMilked".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.ThoughtfulTailSwish".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.FrownAtSillyLook".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.SuggestRailingLean".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.RatherLookEyes".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.SilenceStripQuickly".Localized());

            if (Player.Tallness < 56)
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.GrabBoxClause".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.LeanRailingDisplay".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.LickLipsNervousTail".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.StripSkirtTakePosition".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.RetortNotSuggested".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.SlowEntryInchStep".Localized());

            if (Player.VaginalCapacity() < CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) * CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness))
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.WaitAdjustEachInch".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.FinalInsertTailUp".Localized());

            Player.CuntChange(8, true, true, false);

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.StrangeFeelingThrust".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.PlayBacksideMassage".Localized());

            if (Player.Body.Butt <= 4 && !Player.IsGoo())
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.TautWord".Localized());
            else
                OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.JigglingWord".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.FleshLengthPushPull".Localized(
                NumberUtils.Num2Text((float)(CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength)))
            ));

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.WildFloodingSeed".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleTakeHerDick.HourLaterCalmDown".Localized());

            Player.Orgasm("Vaginal");

            Player.Orgasm("Vaginal");

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 10);

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f));

            Player.KnockUp(PregnancyIds.Marble, MarbleIncubation, 150);

            DoNext(() => ReturnToCamp(2));
        }

        public void MilkMarbleOnTheBar()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.SuggestBarPlay".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.ConsiderProposition".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.AssureWorthwhile".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.SeriousCounteroffer".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.WonderingTurnAround".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.GetMeReady".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.LiftSkirtExpose".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.PleasedMoanTailLift".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.TeaseEntranceFinger".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.PutFingerInside".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.PushIndexFinger".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.OfferTailHelp".Localized());

            if (Player.CockThatFits(MarbleCuntCapacity()) > 0)
                OutputText("Scenes.Marblescene.MilkMarbleOnTheBar.ContemplateTailGrab".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FollowTail".Localized(), MilkMarbleBarFollowTail);

            AddButton(1, "Common.Button.Cunnilingus".Localized(), MilkMarbleBarCunnilingling);

            if (Player.HasCockThatFits(MarbleCuntCapacity()))
                AddButton(2, "Common.Button.PullTail".Localized(), MilkMarbleBarPullTail);

            AddButton(3, "Common.Button.Leave".Localized(), MilkMarbleLeaveAfterBar);
        }

        public void MilkMarbleBarFollowTail()
        {
            ClearOutput();

            var x = Player.CockThatFits(MarbleCuntCapacity());

            var y = Player.CockThatFits2(MarbleCuntCapacity());

            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.AskTailDestination".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.LowChuckleWait".Localized());

            if (x >= 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TailSearchesCrotch".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TauntFoundIt".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.ObviousCannotHide".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.FunLooking".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.SlowEnjoyableEntry".Localized());

                if (y >= 0)
                    OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.ComfortableCheekSqueeze".Localized(
                        Player.CockDescript(y)
                    ));

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.GuideStringTightens".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.ThreefoldSensation".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TailPullsHandOut".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.AskFoundRightSpot".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.WhatGaveItAway".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.JustHunch".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.AdjustToyGrip".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.StuckNeedHand".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.TailReleasesPressure".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.BeyondPlaything".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.FreelyBuildPleasure".Localized());
            }

            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.BarelyNoticeCall".Localized());

            if (x > 0)
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.RootGripRelaxes".Localized());
            else
                OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.DeathGripRelease".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarFollowTail.SternLookTurnaround".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 10);

            if (x >= 0)
            {
                Player.Orgasm("Generic");

                MarblePreggoChance();
            }
            else
            {
                var pLust = (int)(10 + Player.Libido / 10);

                Player.ApplyStats(new DynStat(GameStats.Lust, pLust));
            }

            DoNext(() => ReturnToCamp(2));
        }

        public void MilkMarbleBarCunnilingling()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.AskReleaseWrist".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.WetConsentGranted".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.KneelBehindScent".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.GripTailRoot".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.BemusedResponse".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.RingGripTailSlide".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.SurprisedMoan".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.CarefulDabHarvest".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.TailAroundHead".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.RidingOutOrgasm".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.NuzzleSoftBum".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.WillingAfterAll".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.AssureFullEffort".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.TailFlicksOut".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarCunnilingling.PauseTeasing".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 15);

            var pLust = (int)(10 + Player.Libido / 10);

            Player.ApplyStats(new DynStat(GameStats.Lust, pLust));

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            View.Time.Hours += 1;

            DoNext(() => ReturnToCamp(1));
        }

        public void MilkMarbleBarPullTail()
        {
            ClearOutput();

            var x = Player.CockThatFits(MarbleCuntCapacity());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.GrabTailTakeCharge".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.PainOutburst".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.IgnoreProtestsSmack".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.AngryHoofStomp".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TakeChargeWarning".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.StraddleFaceShake".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.MemberCheekRubbing".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.EnjoyingControl".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) == 0)
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SuffocatingHipDrop".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TooRoughFaceGentle".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.GentleSettleBreathing".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LickDemand".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LickApproved".Localized());

                if (Player.Body.Get<TonguePart>().Id.Path != "human")
                    OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LongTongueFill".Localized());
                else
                    OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TongueDeepWriggle".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.ForceMouthCockShove".Localized(
                    NumberUtils.Num2Text((float)(CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength)))
                ));

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TooRoughMouthGentle".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.GentleThrustAdjustment".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SuckDemand".Localized());

                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.LipsWrappedApproval".Localized());
            }

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.HipsLiftedInstruction".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SootheHurtFur".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.ApologyExperiment".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.TongueSweatyFur".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.DidYouLikeIt".Localized());

            if (Silly)
                OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.BrushTeethTail".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.NodsFeelingBetter".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.AskHerPlan".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.RepositionOverWaist".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.AlreadyDecided".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.HipShakeSpin".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.SuddenEntrancePush".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.QuiveringQuestion".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.UnprecedentedAggression".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.UproariousRiding".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.ExhaustedGasps".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleBarPullTail.BosomSnuggle".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 15);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            MarblePreggoChance();

            DoNext(() => ReturnToCamp(2));
        }

        public void MilkMarbleLeaveAfterBar()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleLeaveAfterBar.SilentFingerOrgasm".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleLeaveAfterBar.MildDisappointment".Localized());

            var pLust = (int)(5 + Player.Libido / 20);

            Player.ApplyStats(new DynStat(GameStats.Lust, pLust));

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) + 5);

            DoNext(() => ReturnToCamp(2));
        }

        public void MilkMarbleNoMilking()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MilkMarbleNoMilking.DeclinePolitely".Localized());

            OutputText("Scenes.Marblescene.MilkMarbleNoMilking.FarewellChairSeat".Localized());

            DoNext(() => ReturnToCamp(2));
        }
    }
}