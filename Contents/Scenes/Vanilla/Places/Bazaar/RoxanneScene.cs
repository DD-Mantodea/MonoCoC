using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<RoxanneScene>("coc:roxanne")]
    public class RoxanneScene : Scene
    {

        public override void Entry()
        {

        }

        public void RoxanneAppearance()
        {

            if (View.Time.Hours > 12 && View.Time.Hours < 19)
                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneMet) == 0)
                    OutputText("Scenes.Roxanne.RoxanneAppearance.TableWithA".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneAppearance.RoxanneUsualCrew".Localized());
        }

        public void Roxanne1stApproach()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.Roxanne1stApproach.HesitantlyApproach".Localized((noFur ? " as they cover everywhere but her fce and torso" : ""), (noFur ? "is largely humanoid, it lacks any ears and" : "has a lizard-like shape,")));

            OutputText("Scenes.Roxanne.Roxanne1stApproach.AlphaLizanGlances".Localized(
                 Player.Mf("buddy", "girly")
            ));

            OutputText("Scenes.Roxanne.Roxanne1stApproach.DoYouApproach".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryLizans);

            AddButton(0, "Common.Button.Yes".Localized(), RoxanneChooseApproachOrRepeat);

            AddButton(1, "Common.Button.No".Localized(), () => Go<BazaarScene>());
        }

        public void RoxanneChooseApproachOrRepeat()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneMet) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.RoxanneMet, CoC.Instance.Flags.Get(GameFlagIds.RoxanneMet) + 1);

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.WalkUpTakeSeat".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.EyesSlideUpDown".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.ExclaimsPoisontail".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.SaysDontLook".Localized());

                if (Player.Corruption > 66)
                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.YeahGotCorruption".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.NeedToBlowOff".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.CanGetWord".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.PointOutClearly".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.WouldChampionReallyDo".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.SentencePeriod".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.ParagraphBreak".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.PiraticalLizanSlides".Localized());

                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.DoYouEngageHer".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) + CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) <= 0)
                {
                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.DidYouComeBack".Localized());

                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.DoYouEngageCaptain".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 2 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) >= 200)
                    {
                        OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.BeenAWhile".Localized());

                        OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.LooksLikeSheWants".Localized());

                        if (Player.AnalCapacity() >= 100)
                            OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.KeepYouStretched".Localized());
                        else
                            OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.ProbablyGoingToStretch".Localized());

                        OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.DoYouEnterContest".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 2 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) < 200)
                        {
                            OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.MissedMyTouch".Localized());

                            OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.DoYouDrinkAgain".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 1 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) < 200)
                            {
                                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.GlaresAtYou".Localized());

                                OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.SlidesEmptyMug".Localized());
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 1 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) >= 200)
                                {
                                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.WincesWhenComeBack".Localized());

                                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.DoYouAcceptOffer".Localized());
                                }
                                else
                                    OutputText("Scenes.Roxanne.RoxanneChooseApproachOrRepeat.IfYoureReading".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RoxanneDrinkingContestLoseOnPurpose, 0);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryLizans);

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), RoxanneDrinkingContest);

            AddButton(1, "Common.Button.No".Localized(), RoxanneDrinkingContestNo);

            AddButton(2, "Common.Button.Lose".Localized(), RoxanneDrinkingContestLoseDeliberately);
        }

        public void RoxanneDrinkingContestNo()
        {

            if (View.Time.Hours == 19 || View.Time.Hours == 20)
            {
                CoC.Instance.Flags.Set(GameFlagIds.CountdownToNightRape, CoC.Instance.Flags.Get(GameFlagIds.CountdownToNightRape) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.CountdownToNightRape) % 4 == 0 && Player.Gender == Gender.Male)
                {
                    SceneInstance<BazaarScene>.Get().NightBazaarButtfuck();

                    return ;
                }
            }

            SceneInstance<BazaarScene>.Get().EnterTheBazaarAndMenu();
        }

        public void RoxanneDrinkingContestLoseDeliberately()
        {

            CoC.Instance.Flags.Set(GameFlagIds.RoxanneDrinkingContestLoseOnPurpose, 1);

            RoxanneDrinkingContest();
        }

        public void RoxanneDrinkingContest()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.RoxanneWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) >= 200)
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.StumblesHugeManhood".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.SauntersOver".Localized());

            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.DemonicDeerTaur".Localized());

            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.TakeMugAndHand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) + CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) <= 0 || CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 2)
            {
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.SmirksAtYou".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 2)
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.AfterLastHumiliation".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.AgainstRenownedFoe".Localized());
            }
            else
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.LeersAtRoxanne".Localized());

            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.YouWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) + CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) < 3)
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.HesitantlySniff".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.SmileLickLips".Localized());

            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.SlamBeerBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) + CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) <= 0)
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.ShoutsTriumphantly".Localized(
                     Player.Mf("brother", "sister")
                ));
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) == 0)
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.TeasesLizanPirate".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) <= 0)
                        OutputText("Scenes.Roxanne.RoxanneDrinkingContest.FeignedAir".Localized((noFur ? "lips" : "draconian muzzle")));
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLastWinner) == 1)
                            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.DeterminedLizanSwishes".Localized());
                        else
                            OutputText("Scenes.Roxanne.RoxanneDrinkingContest.TauntsRoxanne".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLoseOnPurpose) == 0)
            {
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.TaurAtTap".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) >= 200)
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.MassiveSeamRipping".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.HardToHideBulge".Localized());

                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.MoreThanEyeful".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
            }
            else
                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.TaurAtTapAgain".Localized());

            float score = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLoseOnPurpose) == 0)
            {
                score = (Player.Tallness * ((Player.Thickness + 100) / 200) * (Player.Toughness / 100)) + CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestBonusScore);

                if (Player.HasPerk(PerkIds.SatyrSexuality))
                    score += 10;

                if (Player.HasPerk(PerkIds.Lustserker))
                    score += 10;

                if (Player.HasPerk(PerkIds.Dragonfire))
                    score += 10;

                if (Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
                    score += 10;

                if (Player.HasPerk(PerkIds.Medicine))
                    score += 5;

                if (Player.HasPerk(PerkIds.Resolute))
                    score += 5;
            }

            if (score < (45 + Random.Shared.Next(20)))
            {
                CoC.Instance.Flags.Set(GameFlagIds.RoxanneDriningContestLost, CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.RoxanneDrinkingContestLastWinner, 2);

                CoC.Instance.Flags.Set(GameFlagIds.RoxanneDrinkingContestBonusScore, CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestBonusScore) + 10);

                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.GigglingNearlyTripping".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestLoseOnPurpose) == 0)
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.NoooooCutOff".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneDrinkingContest.YessssSoDrunk".Localized());

                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.ScaledHandSlaps".Localized());

                if (Player.Body.Butt > 12 && Player.Tone <= 50 && CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestLost) > 1 && Random.Shared.Next(2) == 0)
                    DoNext(() => BigBootyRoxanneContestLoss());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) >= 200)
                        DoNext(() => RoxanneFucksYourAssOHGODITSHUGE());
                    else
                        DoNext(() => RoxanneReamsYouNormal());
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.RoxanneDriningContestWon, CoC.Instance.Flags.Get(GameFlagIds.RoxanneDriningContestWon) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.RoxanneDrinkingContestLastWinner, 1);

                CoC.Instance.Flags.Set(GameFlagIds.RoxanneDrinkingContestBonusScore, CoC.Instance.Flags.Get(GameFlagIds.RoxanneDrinkingContestBonusScore) + 4);

                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.LaughingUproariously".Localized());

                OutputText("Scenes.Roxanne.RoxanneDrinkingContest.OtherLizansLooking".Localized());

                ClearButtons();

                if (Player.HasVagina())
                    AddButton(0, "Common.Button.Cunnilingus".Localized(), RoxanneCunnilingus);
                else
                    AddButton(0, "Common.Button.Cunnilingus".Localized(), null);

                if (Player.HasCock())
                    AddButton(1, "Common.Button.Fellatio".Localized(), RoxanneGivesABlowjob);
                else
                    AddButton(1, "Common.Button.Fellatio".Localized(), null);

                AddButton(2, "Scenes.Dullahan.AskDullSex.ButtonRimming".Localized(), RoxanneRimjob);
            }
        }

        public void RoxanneGivesABlowjob()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.OpenLowerPortion".Localized());

            if (Player.LustPercent < 70)
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.StiffensErect".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.AlreadyHardCock".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.GigglesDrunkenly".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.SmoothScaledHand".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.KnotWord".Localized());
            else
                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.SheathWord".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.BaseWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.SqueezingSoftly".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.WarmLongNailed".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.LizanGlancesBack".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.SlumpBackChair".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockArea(x) < 60)
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.EntireLengthOf".Localized(
                     Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.AsMuchOverSized".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.ConstrictingSnakeLike".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.StoppingShortFemale".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.PeriodBreak".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.SlurpingAndPumping".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.DemonsStartApproach".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.SmackTailSlaps".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.WigglingFlexibleTongue".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.HugeTorrentDrenches".Localized());
            else
                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.HugeSpurtSoaks".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.BigSpurtSplatters".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.WithCumSoftOral".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.PiratePullsBack".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.AndBodyWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneGivesABlowjob.LaughsStruggling".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RoxanneCunnilingus()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.ShimmyOutOf".Localized((noFur ? "" : " scaled"), (noFur ? "skin" : "scales")));

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.PinkLengthOfPoisontail".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.JuiceDribblingWord".Localized());
            else
                if (Player.Wetness() >= 2)
                    OutputText("Scenes.Roxanne.RoxanneCunnilingus.JuicyWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.EntranceOfLoins".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.FaceBlushingRed".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.DrunkenLizanPuts".Localized());

            if (Player.GetClitLength() < 16)
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.DwarfsWord".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.EnvelopsWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.TheWord".Localized());

            if (Player.GetClitLength() < 1)
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.LittlePleasureBuzzer".Localized());
            else
                if (Player.GetClitLength() < 3)
                    OutputText("Scenes.Roxanne.RoxanneCunnilingus.SwollenLoveButton".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneCunnilingus.CockLikeLadyPart".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.SlobberingOverIt".Localized());

            if (Player.GetClitLength() >= 3)
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.CurlsTightly".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.LetsSpitSloppy".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.ParagraphBreak".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.HelplesslyTwitching".Localized());

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.PullsHandsOff".Localized());

                if (Player.BiggestLactation() < 1.5f)
                    OutputText("Scenes.Roxanne.RoxanneCunnilingus.ExpertlyAsOneBelow".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneCunnilingus.SquirtMilkAround".Localized());

                OutputText("Scenes.Roxanne.RoxanneCunnilingus.PeriodSpace".Localized());
            }

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.UnableToDam".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.AndSquirt".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.TightensHerGrip".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Roxanne.RoxanneCunnilingus.BouncesOnHerHead".Localized(
                     Player.SMultiCockDesc()
                ));

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.SlouchDownPleasureDrunk".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.CockyDrunkenLizan".Localized());

            OutputText("Scenes.Roxanne.RoxanneCunnilingus.SmirkBackSlowly".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RoxanneRimjob()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.RoxanneRimjob.ShimmyOutOf".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Roxanne.RoxanneRimjob.BreastsCushioning".Localized());

            OutputText("Scenes.Roxanne.RoxanneRimjob.PoisontailEyeballs".Localized(
                 Player.AssholeDescript()
            ));

            OutputText("Scenes.Roxanne.RoxanneRimjob.GroansGivesYour".Localized());

            if (Player.HasCock())
            {
                if (Player.LustPercent < 70)
                    OutputText("Scenes.Roxanne.RoxanneRimjob.HardensWord".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneRimjob.PulsesWithNeed".Localized());

                OutputText("Scenes.Roxanne.RoxanneRimjob.BeadOfPreCum".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Roxanne.RoxanneRimjob.NeglectedBoxGetting".Localized());

            OutputText("Scenes.Roxanne.RoxanneRimjob.CompletelyFocusedAnal".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Roxanne.RoxanneRimjob.SurprisinglyOneOf".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Roxanne.RoxanneRimjob.OneOfWord".Localized());

                OutputText("Scenes.Roxanne.RoxanneRimjob.YourMemberWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Roxanne.RoxanneRimjob.PluralS".Localized());

                OutputText("Scenes.Roxanne.RoxanneRimjob.MilkingShaftLikeCow".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Roxanne.RoxanneRimjob.SurprisinglyOneOfAgain".Localized());

            OutputText("Scenes.Roxanne.RoxanneRimjob.LongWondrousTongue".Localized());

            OutputText("Scenes.Roxanne.RoxanneRimjob.OrgasmHitsPowerfully".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Roxanne.RoxanneRimjob.QuiverCumOnFingers".Localized());

                if (!(Player.CumQ() < 500))
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Roxanne.RoxanneRimjob.NicelySizedPool".Localized());
                    else
                        if (Player.CumQ() < 1500)
                            OutputText("Scenes.Roxanne.RoxanneRimjob.LargePoolBefore".Localized());
                        else
                            OutputText("Scenes.Roxanne.RoxanneRimjob.MassivePoolBefore".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Roxanne.RoxanneRimjob.QuiverCumOnHer".Localized());

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Roxanne.RoxanneRimjob.SoakingGushingOrgasms".Localized());
                    else
                        OutputText("Scenes.Roxanne.RoxanneRimjob.SlimingCopiousGirlCum".Localized());

                    OutputText("Scenes.Roxanne.RoxanneRimjob.TeasesYourWith".Localized());
                }

            OutputText("Scenes.Roxanne.RoxanneRimjob.ParagraphBreak".Localized());

            OutputText("Scenes.Roxanne.RoxanneRimjob.CompletelyFinished".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RoxanneReamsYouNormal()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.FootInterposes".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.GiggleUnderneath".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.SlowSteadyPush".Localized());

            Player.ButtChange((int)Math.Floor((double)(30 + (CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) / 4))), true, true, false);

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ParagraphBreak".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.TooIntoxicated".Localized((noFur ? " almost" : "")));

            if (Player.Balls > 0)
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.AgainstYourOwnOrbs".Localized(
                     NumberUtils.Num2Text(Player.Balls + 2)
                ));
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.AgainstYourSweatyOrbs".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.AgainstYourTaint".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.PeriodBreak".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.NothingToDoBut".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.TailHoleWord".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.AnusWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.BeginsToFeelGood".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.VeryGoodWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.BumpsAndGrinds".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.HateCeraphCurse".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ShakeYourHeadNo".Localized());

            if (!Player.HasPerk(PerkIds.Masochist))
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.VoluntarilyWorking".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.MoaningWithDelight".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.BreakingUnderOnslaught".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ConstantPressure".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.BouncesAgainstBelly".Localized());
                else
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.TheyBounceAgainst".Localized());

                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.StickyDroplets".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.DelightAndShame".Localized());

                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ParagraphBreakAgain".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ConstantPressureAgain".Localized());

                    if (Player.Wetness() < 2)
                        OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.SlimingReadyVulva".Localized());
                    else
                        if (Player.Wetness() <= 3)
                            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.SoakingReadyVulva".Localized());
                        else
                            if (Player.Wetness() <= 4)
                                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.WhichDripFromVulva".Localized());
                            else
                                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.WhichLeakSteadyStream".Localized());

                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ParagraphBreakThird".Localized());
                }

            if (Player.Tallness <= 50)
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ShiftsForwardTits".Localized((noFur ? "" : " the smooth scales of")));
            else
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.ShiftsForwardHeaving".Localized((noFur ? "" : " the smooth scales of")));

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.DoublesHerPace".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.InsidesFrothChurn".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.FinishingObsceneOrgasm".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.TugsBackButEven".Localized());

            if (Player.HasCock())
                if (Player.CumQ() >= 1500)
                {
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.TorrentialOutpouring".Localized());

                    if (Player.CumQ() <= 2500)
                        OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.PuddleWord".Localized());
                    else
                        OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.LakeWord".Localized());

                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.BelowTheTable".Localized());
                }
                else
                    OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.OutpouringDrools".Localized());

            if (Player.BiggestLactation() >= 2)
                OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.BurstsOfMilk".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.AsItWindsDown".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.LaterBreak".Localized());

            OutputText("Scenes.Roxanne.RoxanneReamsYouNormal.WakeInLizans".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

            ApplyHangover();

            ResetRoxanneSexTimeCounter();

            DoNext(() => ReturnToCamp(4));
        }

        public void RoxanneFucksYourAssOHGODITSHUGE()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.GoshSoStrong".Localized());

            if (Player.Tallness >= 80)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SmallWord".Localized());
            else
                if (Player.Tallness < 50)
                    OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.LargeWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ShemaleForSupport".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.NoIdeaOfThe".Localized(
                 Player.Mf("boy", "slut")
            ));

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.NudeNewhalfPounces".Localized(
                 Player.Mf("my dear", "sexy girl")
            ));

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.OhGodsShesRight".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SheLiftsYourTail".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SheSpreadsYourCheeks".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ToExamineYour".Localized());

            if (Player.AnalCapacity() < 150)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.DontThinkYouCan".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SoStretchy".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ParagraphBreak".Localized());

            if (Player.AnalCapacity() < 150)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ColdSlimeRubbed".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ShiverAsSheRuns".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.LookBackOverShoulder".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.AllTracesHesitation".Localized());

            if (Player.AnalCapacity() < 150)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ThanksToOintment".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ThanksToYourExperience".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ShesSoBig".Localized());

            if (Player.AnalCapacity() >= 150)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SpitWord".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.OintmentWord".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SlickedAssAccept".Localized());

            if (Player.HasPlainSkin())
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.LeavingRedLines".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.HipsVisiblySpread".Localized());

            Player.ButtChange((int)Math.Floor((double)(30 + (CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) / 4))), true, true, false);

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.StopsAndPants".Localized());

            if (Player.HasScales())
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.RoughlyScratchedX".Localized());
            else
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.IrritatedRedX".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.WhimperFromPain".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.PressureInsideKicks".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.BreatheInShallow".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.OwnWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript());
                else
                    OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.NowNarrowTaint".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.ChurningAndSwelling".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.CommentsSlappingCheeks".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.WailsBanshee".Localized());

            if (Player.HasCock() && Player.CumQ() >= 1000)
                OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.DroolingIntoPuddle".Localized());
            else
                if (Player.HasVagina() && Player.Wetness() >= 4)
                    OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.DroolingIntoPuddy".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.SentencePeriod".Localized());

            OutputText("Scenes.Roxanne.RoxanneFucksYourAssOHGODITSHUGE.YoullWakeAndHead".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

            ApplyHangover();

            ResetRoxanneSexTimeCounter();

            DoNext(() => ReturnToCamp(4));
        }

        public void ApplyHangover()
        {

            if (Player.HasStatusEffect(StatusEffectIds.Hangover))
                Player.ChangeStatusValue(StatusEffectIds.Hangover, 1, 8);
            else
            {
                Player.CreateStatusEffect(StatusEffectIds.Hangover, 8, 0, 0, 0);

                var temp = 5;

                while (temp > 0)
                {
                    temp--;

                    if (Player.Strength >= 2)
                    {
                        Player.Strength--;

                        Player.ChangeStatusValue(StatusEffectIds.Hangover, 1, 1);
                    }
                }

                temp = 10;

                while (temp > 0)
                {
                    temp--;

                    if (Player.Speed >= 2)
                    {
                        Player.Speed--;

                        Player.ChangeStatusValue(StatusEffectIds.Hangover, 2, 1);
                    }
                }

                temp = 15;

                while (temp > 0)
                {
                    temp--;

                    if (Player.Intelligence >= 2)
                    {
                        Player.Intelligence--;

                        Player.ChangeStatusValue(StatusEffectIds.Hangover, 4, 1);
                    }
                }
            }

        }

        public void BigBootyRoxanneContestLoss()
        {
            ClearOutput();

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.GodsHeadSwimming".Localized(
                 Player.Mf("chuckle", "giggle")
            ));

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.WoodenPlanksRattle".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.ReflexivelyGrows".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.NextMomentStumbling".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.MumblesSqueezing".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.IntoxicatedShemaleMassages".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.DropsOfLizanLove".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.AfterAllThatAlcohol".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.GleefullySlaps".Localized());

            Player.ButtChange((int)Math.Floor((double)(30 + (CoC.Instance.Flags.Get(GameFlagIds.RoxanneTimeWithoutSex) / 4))), true, true, false);

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.TwitchesHalfHard".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.GetsSlicker".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.RoxanneGrunts".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.NoPussyTonight".Localized());
            else
                OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.AnalCumDumpster".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.WhimperAsThrusts".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.SighingWithRelief".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.DrunkenScaledDickGirl".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.OpenlyMoaning".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.MoansAndThrusts".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.StrengthIsGone".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.LaterBreak".Localized());

            OutputText("Scenes.Roxanne.BigBootyRoxanneContestLoss.WakeInLizans".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f));

            ApplyHangover();

            ResetRoxanneSexTimeCounter();

            DoNext(() => ReturnToCamp(4));
        }

        public void ResetRoxanneSexTimeCounter()
        {

            CoC.Instance.Flags.Set(GameFlagIds.RoxanneTimeWithoutSex, 1);
        }

    }
}