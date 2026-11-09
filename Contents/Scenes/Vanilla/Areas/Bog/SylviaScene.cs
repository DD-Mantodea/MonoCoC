using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<SylviaScene>("coc:sylvia")]
    public class SylviaScene : Scene
    {

        public static double Round(double value) => Math.Round(value);

        public static SaveContent Save => SaveContent(SceneIds.Bog.Sylvia);

        public string debugName => "Sylvia";

        public string debugHint => "";

        public int sylviaProg => Save.GetInt("sylviaProgress", 0);

        public int SylviaProg => sylviaProg;

        public int sylviaGetAff => Save.GetInt("sylviaAffection", 0);

        public int sylviaGetDom => Save.GetInt("sylviaDominance", 0);

        public SylviaScene saveContent => this;

        public SaveContent SaveData => Save;

        public bool unlockedOyakodon => Save.GetBool("unlockedOyakodon");

        public override void Entry()
        {
            ExecEncounter();
        }

        public void Reset()
        {

            Save.SetInt("sylviaProgress", 0);

            Save.SetInt("sylviaAffection", 0);

            Save.SetInt("sylviaDominance", 0);

            Save.SetInt("sylviaStalking", 0);

            Save.SetInt("sylviaFertile", 0);

            Save.SetInt("sylviaCapstoneCounter", 0);

            Save.SetInt("sylviaClothes", 0);

            Save.SetInt("timeSinceVisit", 0);

            Save.SetFloat("encounterDisabled", 0);

            Save.SetBool("talkedMoths", false);

            Save.SetBool("sylviaGiftedDress", false);

            Save.SetBool("unlockedOyakodon", false);
        }

        public void ExecEncounter()
        {

            if (sylviaProg == 0)
                SylviaFirstMeeting();
            else
                if (sylviaProg == 1)
                    SylviaSecondMeeting();
                else
                    if (sylviaProg == 2 && sylviaGetAff > 50)
                        SylviaStalking();
                    else
                        if (sylviaProg == 3 && 4 > Save.GetInt("sylviaStalking", 0) && Save.GetInt("sylviaStalking", 0) > 0)
                            SylviaStalkAgain();
                        else
                            if (sylviaProg == 4 && Save.GetInt("sylviaCapstoneCounter", 0) == 3)
                            {
                                if (sylviaGetDom < 50) SylviaSubCapstone(); else SylviaDomCapstone();
                            }
                            else
                                SylviaMenu(true);
        }

        public void SylviaDom(int change = 0)
        {

            if (Save.GetInt("sylviaCapstoneCounter", 0) > 0)
                return ;

            Save.SetInt("sylviaDominance", Save.GetInt("sylviaDominance", 0) + change);

            if (Save.GetInt("sylviaDominance", 0) > 100)
                Save.SetInt("sylviaDominance", 100);

            if (Save.GetInt("sylviaDominance", 0) < 0)
                Save.SetInt("sylviaDominance", 0);
        }

        public void SylviaAff(int change = 0)
        {

            Save.SetInt("sylviaAffection", Save.GetInt("sylviaAffection", 0) + change);

            if (Save.GetInt("sylviaAffection", 0) > 100)
                Save.SetInt("sylviaAffection", 100);
        }

        public void SylviaKnockupAttempt(bool dick = true)
        {

            var femBonus = 0;

            if (Player.HasVagina())
                femBonus = (int)(Player.Vaginas[0].Wetness * 5 + Player.Fertility * 100);

            if ((dick && !Player.HasCock()) || (!dick && !Player.HasVagina()))
                OutputText("Scenes.Sylviascene.SylviaKnockupAttempt.PregnancyCheckMismatch".Localized());

            if (Player.KnockUpChance(40 + (dick ? 0 : femBonus), 90) && 0 < 1 && Save.GetInt("sylviaFertile", 0) > 0)
                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.SylviaIncubation);
        }

        public void SylviaFirstMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.ExploreDeeperBog".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.HalfHourNothingHappened".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.BrokenFromReverieFoot".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.PressOnThroughDark".Localized((View.Time.Hours < 20 ? "Common.Text.Sun".Localized() : "Common.Text.Moon".Localized())));

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.AfterHourWildlifeBegins".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.TurnAroundTrudgeOpposite".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting.MoveFasterLeavesBuffet".Localized());

            DoNext(() => SylviaFirstMeeting2());
        }

        public void SylviaFirstMeeting2()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.EerieSilenceBrokenSoft".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.ReadyWeaponBuzzing".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.BuzzingDiesHalfCircle".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.WhirlAroundStalker".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.SoBright".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.PaleEtherealCreaturePinning".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.LetOutBreathScent".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.FlashClarityBreakTrance".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.MurmursPleaseLust".Localized());

            OutputText("Scenes.Sylviascene.SylviaFirstMeeting2.SlaveringMothGirlApproaching".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fight".Localized(), SylviaFight);

            AddNextButton("Common.Button.Submit".Localized(), () => SylviaDefeat(true));
        }

        public void SylviaFight()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaFight.ShiftIntoFightingStance".Localized());

            OutputText("Scenes.Sylviascene.SylviaFight.TrailsOffExcitedSmirk".Localized());

            OutputText("Scenes.Sylviascene.SylviaFight.QuickerThanExpectWings".Localized());

            var monster = new SylviaEnemy
            {
                OnDefeated = _ => SylviaVictory(),

                OnWon = _ => SylviaDefeat()
            };

            CombatScene.Start(monster, () => Go<CampScene>());
        }

        public void SylviaVictory()
        {
            ClearOutput();

            if (sylviaProg == 0)
            {
                OutputText("Scenes.Sylviascene.SylviaVictory.SinksToKneesWings".Localized((CurrentMonster!.Health < 1 ? "Common.Text.ItSeemsYourLastBlowWas".Localized() : "Common.Text.SheShuddersWithArousalClearlyOverwhelmed".Localized())));

                OutputText("Scenes.Sylviascene.SylviaVictory.NoCantPlease".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Sex".Localized(), SylviaVictorySex);

                AddNextButton("Common.Button.Leave".Localized(), SylviaIntroLeave);

                AddNextButton("Common.Button.Kill".Localized(), SylviaKill);
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaVictory.SinksToKneesAgain".Localized((CurrentMonster!.Health < 1 ? "Common.Text.ItSeemsYourLastBlowWas".Localized() : "Common.Text.SheShuddersWithArousalClearlyOverwhelmed".Localized())));

                OutputText("Scenes.Sylviascene.SylviaVictory.WillYouGiveHer".Localized());

                SylviaDom(-10);

                ClearButtons();

                AddNextButton("Common.Button.Sex".Localized(), SylviaVictorySex);

                AddButton(1, "Common.Button.Leave".Localized(), () => SylviaSparLeave());
            }
        }

        public void SylviaVictorySex()
        {
            ClearOutput();

            if (sylviaProg == 0)
                OutputText("Scenes.Sylviascene.SylviaVictorySex.DespondentSitsTearingUp".Localized((Player.Corruption > 50 ? "Common.Text.QuiteEagerAt".Localized() : "Common.Text.NotQuiteSoOpposedTo".Localized())));
            else
                OutputText("Scenes.Sylviascene.SylviaVictorySex.UpsetAboutLosingCheer".Localized());

            OutputText("Scenes.Sylviascene.SylviaVictorySex.PutWeaponStepForward".Localized((sylviaProg == 0 ? "Common.Text.SheLooksConfusedForAMoment".Localized() : "Common.Text.SheLooksExtremelyExcitedAtYour".Localized()), (sylviaProg == 0 ? "Common.Text.LikeThisShesAlmostCuteIf".Localized() : "Common.Text.ShesSoCuteLikeThisThat".Localized() + Player.Mf("man and moth.", "together."))));

            OutputText("Scenes.Sylviascene.SylviaVictorySex.LipsContactMeltLimbs".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Sylviascene.SylviaVictorySex.MumblesNeedYouInside".Localized());

                OutputText("Scenes.Sylviascene.SylviaVictorySex.BlissfulSlickTunnel".Localized());

                OutputText("Scenes.Sylviascene.SylviaVictorySex.InactionDoesntLastLong".Localized());

                OutputText("Scenes.Sylviascene.SylviaVictorySex.UrgedPickUpPace".Localized());

                Player.Orgasm("Dick");

                SylviaKnockupAttempt();
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaVictorySex.MumblesTouchMe".Localized());

                OutputText("Scenes.Sylviascene.SylviaVictorySex.NotWantingOutdoneGroping".Localized());

                OutputText("Scenes.Sylviascene.SylviaVictorySex.InactionImpatientGrind".Localized());

                OutputText("Scenes.Sylviascene.SylviaVictorySex.BothPickUpPace".Localized());

                Player.Orgasm("Vaginal");

                SylviaKnockupAttempt(false);
            }

            OutputText("Scenes.Sylviascene.SylviaVictorySex.SlumpOverSpentArms".Localized());

            OutputText("Scenes.Sylviascene.SylviaVictorySex.ThankYouLoveYou".Localized());

            if (sylviaProg < 1)
            {
                OutputText("Scenes.Sylviascene.SylviaVictorySex.NotTimeProcessConfession".Localized());

                Save.SetInt("sylviaAffection", 10);

                Save.SetInt("sylviaDominance", 50);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaVictorySex.HaveToGoCollect".Localized());

                SylviaAff(5);

                CombatScene.CleanupAfterCombat();
            }
        }

        public void SylviaDefeat(bool submit = false)
        {
            ClearOutput();

            if (sylviaProg == 0)
                OutputText("Scenes.Sylviascene.SylviaDefeat.ProvesTooMuchUpright".Localized());
            else
                OutputText("Scenes.Sylviascene.SylviaDefeat.ProvesTooUprightAgain".Localized());

            OutputText("Scenes.Sylviascene.SylviaDefeat.LustyFaceToFace".Localized());

            OutputText("Scenes.Sylviascene.SylviaDefeat.YourePerfect".Localized());

            OutputText("Scenes.Sylviascene.SylviaDefeat.HeadFeelsLightWaver".Localized());

            OutputText("Scenes.Sylviascene.SylviaDefeat.AndNowYoureMine".Localized());

            OutputText("Scenes.Sylviascene.SylviaDefeat.StarkNakedDoesntMatter".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Sylviascene.SylviaDefeat.PainfullyLongWaitSlips".Localized());

                OutputText("Scenes.Sylviascene.SylviaDefeat.SeemingPacePicksUp".Localized());

                OutputText("Scenes.Sylviascene.SylviaDefeat.FirstRopeSlamsDown".Localized());

                Player.Orgasm("Dick");

                SylviaKnockupAttempt();
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Sylviascene.SylviaDefeat.PainfullyLongWaitFingers".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDefeat.WolfishGrinReverie".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDefeat.TakesTimeGrindingPrecision".Localized());

                    Player.Orgasm("Vaginal");

                    SylviaKnockupAttempt(false);
                }
                else
                {
                    if (silly)
                        OutputText("Scenes.Sylviascene.SylviaDefeat.JamsFingerCumFruit".Localized());
                    else
                        OutputText("Scenes.Sylviascene.SylviaDefeat.SeemsDisappointedMorePlay".Localized());

                    Player.Orgasm("Anal");
                }

            OutputText("Scenes.Sylviascene.SylviaDefeat.ComeDownFogClears".Localized((sylviaProg == 0 ? "Common.Text.YoureSureThatWasntAnInvention".Localized() : "Common.Text.TheseWordsAreSomehowWonderfullyComforting".Localized())));

            OutputText("Scenes.Sylviascene.SylviaDefeat.SeveralMinutesStroking".Localized());

            if (sylviaProg == 0)
                OutputText("Scenes.Sylviascene.SylviaDefeat.SeeYouSoon".Localized());
            else
                OutputText("Scenes.Sylviascene.SylviaDefeat.ILoveYouName".Localized());

            if (sylviaProg < 1)
            {
                OutputText("Scenes.Sylviascene.SylviaDefeat.BarelyRegisterSleep".Localized());

                Save.SetInt("sylviaAffection", 20);

                Save.SetInt("sylviaDominance", 60);

                if (!submit)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => SylviaWakeUp());
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaDefeat.UrgeSleepHelpsUp".Localized());

                SylviaDom(10);

                SylviaAff(5);

                CombatScene.CleanupAfterCombat();
            }
        }

        public void SylviaIntroLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaIntroLeave.HavingNoneOfIt".Localized());

            OutputText("Scenes.Sylviascene.SylviaIntroLeave.LooksCompletelyCrushed".Localized());

            OutputText("Scenes.Sylviascene.SylviaIntroLeave.SureNotComingExhaustion".Localized());

            Save.SetInt("sylviaAffection", 0);

            Save.SetInt("sylviaDominance", 40);

            CombatScene.CleanupAfterCombat();
        }

        public void SylviaWakeUp()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaWakeUp.JoltAwakePanic".Localized());

            OutputText("Scenes.Sylviascene.SylviaWakeUp.UnfamiliarNoIdeaHow".Localized());

            OutputText("Scenes.Sylviascene.SylviaWakeUp.StrugglingRememberFlashesWhite".Localized());

            Save.SetInt("sylviaProgress", 1);

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void SylviaKill()
        {
            ClearOutput();

            if (silly)
            {
                OutputText("Scenes.Sylviascene.SylviaKill.KnowHowDealWhores".Localized());

                OutputText("Scenes.Sylviascene.SylviaKill.AnythingButThat".Localized());

                OutputText("Scenes.Sylviascene.SylviaKill.PetulantWailsMarchOver".Localized((Player.HasTailInsteadOfLegs() ? "Common.Text.Tail".Localized() : "Common.Text.Knee".Localized()), (OptionStore.Instance.Get("gore", 0) > 0 ? "Common.Text.BloodyPulp".Localized() : "Common.Text.BatteredMass".Localized())));

                OutputText("Scenes.Sylviascene.SylviaKill.StepBackAdmireSquashed".Localized());
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaKill.NeedsDealtWithPermanently".Localized());

                if (Player.Weapon.IsAxe() || Player.Weapon.IsScythe())
                    OutputText("Scenes.Sylviascene.SylviaKill.WalkUpPullWeapon".Localized());
                else
                    if (Player.Weapon.IsKnife())
                        OutputText("Scenes.Sylviascene.SylviaKill.SlowlyAdvanceFallenMoth".Localized());
                    else
                        if (Player.Weapon.IsBladed())
                            OutputText("Scenes.Sylviascene.SylviaKill.MarchSwordTipFixed".Localized());
                        else
                            if (Player.Weapon.IsBlunt())
                                OutputText("Scenes.Sylviascene.SylviaKill.WalkingOverDoesntUnderstand".Localized());
                            else
                                if (Player.Weapon.IsSpear())
                                    OutputText("Scenes.Sylviascene.SylviaKill.SetSpearCharge".Localized());
                                else
                                    if (Player.Weapon.IsStaff())
                                        OutputText("Scenes.Sylviascene.SylviaKill.NotGiveUpTactical".Localized());
                                    else
                                        if (Player.Weapon.IsFirearm())
                                            OutputText("Scenes.Sylviascene.SylviaKill.ApproachWarilyReadyFire".Localized());
                                        else
                                            OutputText("Scenes.Sylviascene.SylviaKill.StridePlantFirstBlow".Localized());

                OutputText("Scenes.Sylviascene.SylviaKill.BloodPoursShockedMouth".Localized());

                OutputText("Scenes.Sylviascene.SylviaKill.JustWantedBright".Localized());

                OutputText("Scenes.Sylviascene.SylviaKill.StopsMakingSoundLifeless".Localized());
            }

            if (!Player.HasPerk(PerkIds.HistoryDEUSVULT))
                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Save.SetInt("sylviaProgress", -1);

            CombatScene.CleanupAfterCombat();
        }

        public void SylviaSecondMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.SomethingStillBothersMemory".Localized());

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.NothingEventfulThickerMire".Localized());

            if (sylviaGetAff > 0)
                OutputText("Scenes.Sylviascene.SylviaSecondMeeting.GoodToSeeAgain".Localized());
            else
                OutputText("Scenes.Sylviascene.SylviaSecondMeeting.YouCameBack".Localized());

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.StumbleForwardFaceAssailant".Localized((sylviaGetAff > 0 ? "Common.Text.Sultry".Localized() : "Appearance.Balls.Lusty.Desperate".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.HmmRespondsWhisperName".Localized());

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.ContentConversationPressOn".Localized());

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.RepliesSoftlyTooBright".Localized());

            OutputText("Scenes.Sylviascene.SylviaSecondMeeting.AnswerIsntEnoughContinues".Localized());

            Save.SetInt("sylviaProgress", 2);

            ClearButtons();

            AddNextButton("Common.Button.AcceptEagerly".Localized(), () => SylviaSecondAccept());

            AddNextButton("Common.Button.AcceptWarily".Localized(), () => SylviaSecondAccept(1));

            AddNextButton("Common.Button.AcceptMeekly".Localized(), () => SylviaSecondAccept(2));

            AddNextButton("Common.Button.Reject".Localized(), SylviaSecondReject);
        }

        public void SylviaSecondAccept(int choice = 0)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Sylviascene.SylviaSecondAccept.BehaviorUnusualInterested".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.BeforeReachesFaltersPheromones".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.FewMomentsFerventlyEmbracing".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.HeadCheerNextEncounter".Localized());

                SylviaAff(10);
                }

                break;

                case 1:
                {
                OutputText("Scenes.Sylviascene.SylviaSecondAccept.NotCompletelyIntrigued".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.StopsShortReactionRealization".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.StillCatchHintPheromones".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.WaitingForYou".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.HeadBackContentSmile".Localized());

                SylviaDom(-10);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Sylviascene.SylviaSecondAccept.RushesForwardSweepsHug".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.ThereThereCalmedDown".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.WhatSaidTrueClearer".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.EnoughComeBackSoon".Localized());

                OutputText("Scenes.Sylviascene.SylviaSecondAccept.HeadBackFeelsWatched".Localized());

                SylviaDom(10);
                }

                break;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSecondReject()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSecondReject.CantAgreeDangerous".Localized());

            OutputText("Scenes.Sylviascene.SylviaSecondReject.LooksStabbedHeart".Localized((Player.Corruption < 40 ? "Common.Text.TryToExplainToHerThat".Localized() : "Common.Text.ExplainToHerThatSheShouldnt".Localized())));

            Save.SetInt("sylviaProgress", -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaStalking()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaStalking.WanderingBogAnotherAttempt".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking.FewTenseMinutesHear".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking.CautiousStepForwardAsking".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking.TurnFrogGirlFleeing".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking.ContinueTrekOppressiveSilence".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking.DontThinkDriderSource".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking.MysteriousSaviorZoomsOff".Localized());

            Save.SetInt("sylviaProgress", 3);

            DoNext(() => SylviaStalking2());
        }

        public void SylviaStalking2()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaStalking2.ReturnHomeInterruptedPhantom".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking2.StepsForwardHugNotice".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalking2.ArentSureSpecificAnswer".Localized((Player.Tallness > 70 ? "Common.Text.YourChest".Localized() : "Common.Text.YouIntoHerBosom".Localized()), (Player.Tallness > 70 ? "Common.Text.Down".Localized() : "Common.Text.Up".Localized())));

            OutputText("Scenes.Sylviascene.SylviaStalking2.ExpressionShiftsNonchalance".Localized());

            ClearButtons();

            AddNextButton("Common.Button.ItsFine".Localized(), SylviaStalkAgree);

            AddNextButton("Common.Button.NoWay".Localized(), SylviaStalkDecline);
        }

        public void SylviaStalkAgree()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaStalkAgree.MethodsUnusualBestInterests".Localized((Player.Tallness > 70 ? "Common.Text.Chest".Localized() : "Common.Text.Hair".Localized())));

            OutputText("Scenes.Sylviascene.SylviaStalkAgree.LaughingGetBetterLook".Localized((Player.Tallness > 70 ? "Common.Text.TiltHerChinUp".Localized() : "Common.Text.PullYourHeadBack".Localized()), (Player.IsDrider() ? " Not that there's anything wrong with being a spider, of course." : "")));

            OutputText("Scenes.Sylviascene.SylviaStalkAgree.LaughAgainNervouslySubject".Localized());

            Save.SetInt("sylviaStalking", 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaStalkDecline()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaStalkDecline.MayInterestsCrossesLine".Localized());

            OutputText("Scenes.Sylviascene.SylviaStalkDecline.MayBeWrongReaction".Localized());

            Save.SetInt("sylviaStalking", -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaStalkAgain()
        {
            ClearOutput();

            switch (Save.GetInt("sylviaStalking", 0))
            {
                case 1:
                {
                OutputText("Scenes.Sylviascene.SylviaStalkAgain.MiserableHourTrudgingMud".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.WantingInvestigateInspectTrees".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.PermissionToFollow".Localized());

                Save.SetInt("sylviaStalking", Save.GetInt("sylviaStalking", 0) + 1);

                Save.SetFloat("encounterDisabled", View.Time.TotalTime);

                }

                break;

                case 2:
                {
                OutputText("Scenes.Sylviascene.SylviaStalkAgain.FamiliarSolitudeStrikes".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.ApproachSlowlyCautiously".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.FirstWordShootsOff".Localized());

                Save.SetInt("sylviaStalking", Save.GetInt("sylviaStalking", 0) + 1);

                Save.SetFloat("encounterDisabled", View.Time.TotalTime);

                }

                break;

                case 3:
                {
                OutputText("Scenes.Sylviascene.SylviaStalkAgain.OnceAgainDeathlyQuiet".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.ExasperationStartCallingOut".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.AnnoyedAbsentmindednessRoot".Localized((Player.Body.Get<BodyParts.Vanilla.WingsPart>().Id != BodyPartTypes.None ? ", but your wings remain tangled up, preventing you from flying away" : "")));

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.DriderCantDealWith".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.ShiverSurroundedTormentors".Localized((Player.IsDrider() ? "It seems your shared features will do nothing to dissuade them. " : "")));

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.AllLookingSameDirection".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.FliesForSomeDistance".Localized((Player.Tallness > 70 ? "Common.Text.IntoYourChest".Localized() : "Common.Text.IntoYourHair".Localized())));

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.FirstToBreakSilence".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.RubbingBackThankAssistance".Localized());

                Save.SetInt("sylviaStalking", Save.GetInt("sylviaStalking", 0) + 1);

                DoNext(() => ReturnToCamp(1));
                }

                break;
            }

            if (true )
            {
                OutputText("Scenes.Sylviascene.SylviaStalkAgain.FuckedUpReportBug".Localized());

                OutputText("Scenes.Sylviascene.SylviaStalkAgain.StalkingValue".Localized(
                    Save.GetInt("sylviaStalking", 0)
                ));

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void SylviaHomeStalking()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.DayGrowsSurveyCamp".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.DontQuiteKnowWhy".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.CampWallProgress) > 0 ? "your wall should do a good job keeping out any undesirables" : (CampScene.CampGuarded() ? "Common.Text.YouDoHaveSomeoneOnWatch".Localized() : "Common.Text.YouFeelConfidentEnoughInYour".Localized())
            ));

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.StillNothingHerePeculiar".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.TurnBackFlickerWhite".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.SeeHoveringEdgeCamp".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.SuddenSmileBoltsOff".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeStalking.WerentAwareKnewWhere".Localized());

            Save.SetInt("sylviaStalking", 5);

            DoNext(PlayerMenu);
        }

        public void SylviaCaveFirst()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaCaveFirst.DecideVisitHerHome".Localized());

            OutputText("Scenes.Sylviascene.SylviaCaveFirst.DontSylviaSomethingCreaks".Localized());

            OutputText("Scenes.Sylviascene.SylviaCaveFirst.BySuddenDeclarationExplain".Localized((Player.Corruption > 50 ? "Common.Text.Excited".Localized() : "Common.Text.Surprised".Localized()), (!Player.HasCock() ? ", as you're not even sure how that would be physically possible," : "")));

            if (!Player.HasCock())
                OutputText("Scenes.Sylviascene.SylviaCaveFirst.StillBitConfusedMate".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Accept".Localized(), SylviaAttunement);

            AddNextButton("Scenes.Lumberjack.Button.Decline".Localized(), DeclineAttunement);
        }

        public void SylviaAttunement()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaAttunement.TellHappyToFather".Localized((Player.Tallness > 70 ? "Common.Text.YouWrapYourArmsAroundThe".Localized() : "Common.Text.YoureABitUncomfortableWithHow".Localized()), (sylviaGetDom < 50 ? "Common.Text.WithMildAnnoyance".Localized() : "Common.Text.WithGreatEffort".Localized())));

            OutputText("Scenes.Sylviascene.SylviaAttunement.AboutAskSwoopsIn".Localized());

            OutputText("Scenes.Sylviascene.SylviaAttunement.NormallySurprisedTongueCrawls".Localized());

            OutputText("Scenes.Sylviascene.SylviaAttunement.SurroundedByNothing".Localized());

            OutputText("Scenes.Sylviascene.SylviaAttunement.FloatThereForTime".Localized());

            OutputText("Scenes.Sylviascene.SylviaAttunement.TorrentOfInformationFigure".Localized());

            OutputText("Scenes.Sylviascene.SylviaAttunement.JoltAwakeHeartPalpitating".Localized());

            OutputText("Scenes.Sylviascene.SylviaAttunement.DontWantThinkAttunement".Localized());

            Save.SetInt("sylviaFertile", 1);

            ClearButtons();

            AddNextButton("Common.Button.MatingPress".Localized(), SylviaMatingPress);

            AddNextButton("Common.Button.ReverseCowgirl".Localized(), SylviaReverseCowgirl);

            AddNextButton("Common.Button.Fluffjob".Localized(), SylviaFluffjob);

            AddNextButton("Common.Button.ProbTonguing".Localized(), SylviaProbTongue);

            AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), SylviaTribbing);

            AddButton(14, "Common.Button.Leave".Localized(), () => SylviaAttunementLeave());
        }

        public void SylviaAttunementLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaAttunementLeave.TellTooTired".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineAttunement()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.DeclineAttunement.TellDontWantThrough".Localized((sylviaGetDom < 50 ? "Common.Text.StaunchlyAffirm".Localized() : "Common.Text.HesitantlyPutForth".Localized())));

            OutputText("Scenes.Sylviascene.DeclineAttunement.SilentChangeMindHere".Localized((Player.Corruption > 50 ? "Common.Text.Annoyed".Localized() : "Common.Text.Guilty".Localized())));

            Save.SetInt("sylviaFertile", -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresBirth()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.DoloresBirth.FeelingShouldBeWith".Localized());

            OutputText("Scenes.Sylviascene.DoloresBirth.OhGladYoureHere".Localized());

            OutputText("Scenes.Sylviascene.DoloresBirth.AssistPositionHoldHand".Localized());

            OutputText("Scenes.Sylviascene.DoloresBirth.FinallyDaughterIntoArms".Localized());

            OutputText("Scenes.Sylviascene.DoloresBirth.TwoOfThemTogether".Localized());

            OutputText("Scenes.Sylviascene.DoloresBirth.NameMothMothersTradition".Localized());

            Save.SetInt("doloresProgress", 1);

            Player.Pregnancy.Type = new Identifier("coc", "none"); Player.Pregnancy.Incubation = 0;

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSubCapstone()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.SittingBedHandsEmpty".Localized((Save.GetBool("sylviaGiftedDress", false) ? "Common.Text.ShesAlsoWearingTheDressYou".Localized() : "Common.Text.IsSomethingGoingOn".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.CallOutLooksUp".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.WalkSitDownConcerned".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.SnifflesSappyRightNow".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.NoWarningLurchesRestrains".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.QuietVoiceBarelyWhisper".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.HoverUncertainlyTakeThem".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapstone.SmilesCheerfullyNoSerious".Localized());

            Save.SetInt("sylviaProgress", 5);

            ClearButtons();

            AddNextButton("Common.Button.Reciprocate".Localized(), SylviaSubCapReciprocate);

            AddNextButton("Common.Button.Comfort".Localized(), SylviaSubCapComfort);

            AddNextButton("Common.Button.Reject".Localized(), SylviaSubCapReject);
        }

        public void SylviaSubCapReciprocate()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.RatherThanWordsKiss".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.ForcedPartByAir".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.ArmsTightenDeathGrip".Localized((Player.Tallness > 65 ? "Common.Text.Chest".Localized() : "Common.Text.Hair".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.PullsHeadBackThanks".Localized((Player.Tallness > 65 ? "Common.Text.Chest".Localized() : "Common.Text.Hair".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.HandOnBackHead".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.GoEventuallyDread".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.WinkingGoSoonPlace".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.PleaseComeBackSoon".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReciprocate.OneLastKissWay".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSubCapComfort()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.ObviouslyDevotedButDont".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.LookingExpectantlyWrappingArms".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.NotEasiestPerson".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.NotSureSheWarmer".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.SorryAboutTheatricsFine".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.FineWithHopeEnough".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapComfort.ThinkYouContentExit".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSubCapReject()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.LookEyesDontLove".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.EyesWiderHandsShake".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.SobbingTearsAtHer".Localized((Save.GetBool("sylviaGiftedDress", false) ? "Common.Text.HerDress".Localized() : "Common.Text.ThePatchOfFurAtHer".Localized()), (Save.GetBool("sylviaGiftedDress", false) ? "Common.Text.LongStrips".Localized() : "Common.Text.SmallChunks".Localized()), (Player.Corruption < 20 ? "Common.Text.ASmallPangOfGuilt".Localized() : "Common.Text.Indifference".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.EyesTurnHollowMeet".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.GetUpReachesOut".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.WhispersLoveQuieterEach".Localized((Player.Tallness > 70 ? "Common.Text.Chest".Localized() : "Common.Text.Hair".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.NotFareStickingAround".Localized((Player.Corruption < 50 ? "Common.Text.YouDidntMeanToHurtHer".Localized() : "Common.Text.YouDontCareAllTheMuch".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.FaceSnapsRevealing".Localized());

            OutputText("Scenes.Sylviascene.SylviaSubCapReject.ShiverLeaveBehind".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaDomCapstone()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.SittingBedHandsEmpty".Localized((Save.GetBool("sylviaGiftedDress", false) ? "Common.Text.ShesAlsoWearingTheDressYou".Localized() : "Common.Text.IsSomethingGoingOn".Localized())));

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.WalkUpSomethingOff".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.ILoveYou".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.LikeToReplyWrong".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.RealizeEmotionUnbridledDesire".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.ILoveYouAgain".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.BreastsPressFingersCrawling".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.ILoveYouThird".Localized());

            OutputText("Scenes.Sylviascene.SylviaDomCapstone.DoResistTooLate".Localized());

            Save.SetInt("sylviaProgress", 5);

            ClearButtons();

            AddNextButton("Common.Button.Resist".Localized(), SylviaResistBadEnd);

            AddNextButton("Common.Button.GiveIn".Localized(), SylviaBadEnd);
        }

        public void SylviaResistBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.SnapOutHazeShaking".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.FinishSheDoesNothing".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.StunnedSomethingDifferentEyes".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.SeveralMinutesHuggingBack".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.WhyDontGetDrink".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.FloatsBackRoomCup".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.AlwaysBeMine".Localized());

            OutputText("Scenes.Sylviascene.SylviaResistBadEnd.HelpsYouSendsWay".Localized());

            Player.ChangeFatigue(100);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaBadEnd.CouldntPossiblySummonWill".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd.WingsBeatAirSmell".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd.FamiliarPheromonesWithstand".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd.TakesArmLeadsChair".Localized((Player.IsChild() ? "Common.Text.LikeAParentWould".Localized() : "Common.Text.LikeYoureAChild".Localized())));

            OutputText("Scenes.Sylviascene.SylviaBadEnd.WingsSpreadBlockingAirflow".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd.WarmCompressingTorso".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd.EyesRollBackSubmit".Localized());

            DoNext(() => SylviaBadEnd2());
        }

        public void SylviaBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.NotQuiteOblivionSpace".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.BodyAndSoulDevoted".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.SometimesWakeUpBookshelf".Localized((Save.GetInt("doloresProgress", 0) > 0 ? "Common.Text.YouEvenGetToSeeYour".Localized() : "Common.Text.AndSometimesInYourDreamsYou".Localized())));

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.OtherTimesBetterThings".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.YearsGoLittleChanges".Localized((Save.GetInt("doloresProgress", 0) > 0 ? "Common.Button.Dolores".Localized() : "Npcs.Sylvia.Name".Localized()), (FollowerRegistry.All.Count != 0 ? " Will your companions be okay?" : ""), (Player.HasChildren() ? " Your children?" : ""), (Player.Corruption < 50 ? "Common.Text.WhatAboutIngnam".Localized() : "Common.Text.WhatAboutYourGreatAmbitions".Localized())));

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.EverythingOkayAlwaysThere".Localized());

            OutputText("Scenes.Sylviascene.SylviaBadEnd2.BoldILoveYou".Localized());

            GameOver();
        }

        public void SylviaMenu(bool intro = false)
        {
            ClearOutput();

            if (Player.Pregnancy.Event == 1)
            {
                OutputText("Scenes.Sylviascene.SylviaMenu.WalkUpEyesFlash".Localized());

                OutputText("Scenes.Sylviascene.SylviaMenu.StopsMomentWonderfulThing".Localized());

                OutputText("Scenes.Sylviascene.SylviaMenu.NuzzlesLongerThanComfortable".Localized());
            }
            else
                if (Save.GetInt("timeSinceVisit", 0) > 504)
                {
                    OutputText("Scenes.Sylviascene.SylviaMenu.CheckUsualHaunts".Localized());

                    OutputText("Scenes.Sylviascene.SylviaMenu.WonderingBuzzingOnlyWarning".Localized());

                    OutputText("Scenes.Sylviascene.SylviaMenu.ShudderingBreathAgainstBack".Localized((sylviaGetDom < 50 ? "Common.Text.AllowingYouToTurnAround".Localized() : "Common.Text.AndThenSpinsYouAround".Localized())));

                    OutputText("Scenes.Sylviascene.SylviaMenu.EyesWideManicTrembles".Localized());

                    OutputText("Scenes.Sylviascene.SylviaMenu.DontLeaveLikeThat".Localized());
                }
                else
                    if (sylviaProg < 4)
                    {
                        if (intro)
                        {
                            if (sylviaGetAff < 50)
                                OutputText("Scenes.Sylviascene.SylviaMenu.MothIntriguesDeeperBog".Localized());
                            else
                                OutputText("Scenes.Sylviascene.SylviaMenu.DecideVisitPathFamiliar".Localized());

                            OutputText("Scenes.Sylviascene.SylviaMenu.SureEnoughKeenBuzzing".Localized());
                        }
                        else
                            OutputText("Scenes.Sylviascene.SylviaMenu.WasThereAnythingElse".Localized());
                    }
                    else
                        if (intro)
                        {
                            var choices = new List<int> { 1, 2, 3, 4, 5 };

                            if (Save.GetInt("sylviaClothes", 0) > 1)
                                choices.Add(6);

                            if (Save.GetBool("sylviaGiftedDress"))
                                choices.Add(7);

                            switch (choices[Random.Shared.Next(choices.Count)])
                            {
                                case 1:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.SittingBedReadingBook".Localized());
                                }

                                break;

                                case 2:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.LookCantFindHer".Localized());

                                OutputText("Scenes.Sylviascene.SylviaMenu.WereYouLookingMe".Localized());
                                }

                                break;

                                case 3:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.SittingAtTableTroubled".Localized());
                                }

                                break;

                                case 4:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.DryingHerselfWithTowel".Localized());

                                OutputText("Scenes.Sylviascene.SylviaMenu.BeWithYouMinute".Localized());
                                }

                                break;

                                case 5:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.SittingTableNibblingFruit".Localized());
                                }

                                break;

                                case 6:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.HearGigglingBeforeSee".Localized());

                                OutputText("Scenes.Sylviascene.SylviaMenu.LooksMortifiedNothing".Localized());
                                }

                                break;

                                case 7:
                                {
                                OutputText("Scenes.Sylviascene.SylviaMenu.SmoothingOutWhiteDress".Localized());

                                OutputText("Scenes.Sylviascene.SylviaMenu.JustThinkingOfYou".Localized());
                                }

                                break;
                            }

                            if (true )
                                OutputText("Scenes.Sylviascene.SylviaMenu.BugOccurredReportThread".Localized());
                        }
                        else
                            OutputText("Scenes.Sylviascene.SylviaMenu.WasThereAnythingAgain".Localized());

            Save.SetInt("timeSinceVisit", 0);

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), SylviaAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), SylviaTalkMenu);

            AddButton(2, "Common.Button.Sex".Localized(), SylviaSexMenu);

            AddButton(3, "Common.Button.Spar".Localized(), SylviaSpar);

            AddButton(5, "Common.Button.Kiss".Localized(), SylviaKiss);

            if (Save.GetInt("sylviaClothes", 0) > 0)
                AddButton(6, "Common.Button.Clothing".Localized(), GiveSylviaClothes);

            if (sylviaProg >= 4)
            {
                AddRowButton(1, "Cuddle", SylviaCuddle);

                if (sylviaGetDom >= 75)
                    AddRowButton(1, "Massage", SylviaMassage);

                if (sylviaGetDom < 25)
                    AddRowButton(1, "Meal", SylviaMeal);
            }

            if (Save.GetInt("sylviaFertile", 0) == -1)
                AddButton(10, "Common.Button.Attunement".Localized(), SylviaAttunement);

            if (sylviaProg == 4 && (sylviaGetDom == 0 || sylviaGetDom == 100) && Save.GetInt("sylviaCapstoneCounter", 0) < 3)
                AddRowButton(2, "Spend Time", SylviaSpendTime);

            if (Player.HasKeyItem("Sylvia's Dress"))
                AddRowButton(2, "Gift Dress", GiftDress);

            if (sylviaProg < 4)
                AddButton(14, "Common.Button.Leave".Localized(), () => SylviaBogLeave());
            else
                SetExitButton("Common.Button.Back".Localized(), () => SylviaMenu());
        }

        public void SylviaBogLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaBogLeave.WerentLookingToAnything".Localized());

            OutputText("Scenes.Sylviascene.SylviaBogLeave.SmileFallsStaresBlankly".Localized());

            OutputText("Scenes.Sylviascene.SylviaBogLeave.HeadBackUneasiness".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaAppearance.VoluptuousMothGirlTall".Localized((metric ? "172 cm" : "5'8''")));

            if (Player.Pregnancy.Event > 2)
                OutputText("Scenes.Sylviascene.SylviaAppearance.BellyCloseToPopping".Localized());
            else
                if (Player.Pregnancy.Event > 0)
                    OutputText("Scenes.Sylviascene.SylviaAppearance.SlightSwellingEvidenceLove".Localized());

            DoNext(() => SylviaMenu());
        }

        public void SylviaTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaTalkMenu.WantTalkAbout".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Wings".Localized(), SylviaWingTalk);

            AddNextButton("Common.Button.Alone".Localized(), SylviaAloneTalk);

            AddNextButton("Common.Button.Family".Localized(), SylviaFamilyTalk);

            if (sylviaGetAff >= 50)
            {
                AddNextButton("Common.Button.Moths".Localized(), SylviaMothTalk);

                AddNextButton("Areas.Bog.Name".Localized(), SylviaBogTalk);
            }

            if (Save.GetInt("sylviaAffection", 0) > 99 && sylviaProg == 3)
                AddNextButton("Common.Button.Home".Localized(), SylviaHomeTalk);

            if (Save.GetInt("doloresProgress", 0) > 2)
                AddNextButton("Common.Button.Dolores".Localized(), SylviaDoloresTalk);

            if (!Save.GetBool("unlockedOyakodon") && 0 > 0)
                AddNextButton("Common.Button.DoloresSex".Localized(), SylviaThreesomeTalk);

            AddButton(14, "Common.Button.Back".Localized(), () => SylviaMenu());
        }

        public void SylviaWingTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaWingTalk.NormalConversationWings".Localized());

            OutputText("Scenes.Sylviascene.SylviaWingTalk.PlaceholderOnly".Localized((sylviaGetDom < 50 ? "Common.Text.SheBlushesAndRespondsSayWell".Localized() : "Common.Text.SayInterestedInMyBodyAre".Localized())));

            OutputText("Scenes.Sylviascene.SylviaWingTalk.BeautifulStretchingEither".Localized());

            OutputText("Scenes.Sylviascene.SylviaWingTalk.DraggingEyesFromDerriere".Localized());

            DoNext(() => SylviaTalkMenu());
        }

        public void SylviaAloneTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaAloneTalk.AskHowSheSurvives".Localized());

            OutputText("Scenes.Sylviascene.SylviaAloneTalk.MothsDontEatMuch".Localized());

            OutputText("Scenes.Sylviascene.SylviaAloneTalk.ProboscisTongueSnakesOut".Localized());

            OutputText("Scenes.Sylviascene.SylviaAloneTalk.PlaceholderAgain".Localized((sylviaGetDom < 50 ? "Common.Text.SayYouDontNeedToWorry".Localized() : "Common.Text.SayWorriedAboutMeHmmShe".Localized())));

            if (Save.GetInt("sylviaClothes", 0) == 0)
            {
                OutputText("Scenes.Sylviascene.SylviaAloneTalk.CuriositySatisfiedTakeLeave".Localized((sylviaGetDom < 50 ? "" : "entirely ")));

                OutputText("Scenes.Sylviascene.SylviaAloneTalk.IntonesSoftlyLonely".Localized());

                OutputText("Scenes.Sylviascene.SylviaAloneTalk.YourClothesNotObjectionable".Localized((Player.Tallness > 80 ? "Common.Text.TheyClearlyWouldntFitHer".Localized() : "Common.Text.YouveNeverSeenHerWearAny".Localized())));

                OutputText("Scenes.Sylviascene.SylviaAloneTalk.SomethingSmellsLikeYou".Localized());

                OutputText("Scenes.Sylviascene.SylviaAloneTalk.WhenPutsLikeThat".Localized());

                Save.SetInt("sylviaClothes", Save.GetInt("sylviaClothes", 0) + 1);
            }

            DoNext(() => SylviaTalkMenu());
        }

        public void SylviaFamilyTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaFamilyTalk.AskIfSheFamily".Localized());

            OutputText("Scenes.Sylviascene.SylviaFamilyTalk.MotherRaisedUnsure".Localized());

            OutputText("Scenes.Sylviascene.SylviaFamilyTalk.DiedAfterDemonsCame".Localized());

            OutputText("Scenes.Sylviascene.SylviaFamilyTalk.HeartfeltMomentMoveOn".Localized());

            DoNext(() => SylviaTalkMenu());
        }

        public void GiveSylviaClothes()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.GiveSylviaClothes.WhatToGive".Localized());

            ClearButtons();

            var foundItem = false;

            foreach (var x in Enumerable.Range(0, 10))
                if (Player.Inventory.Slots[x].Quantity > 0 && ((false  && !false ) || false ))
                {
                    AddButton(x, Player.Inventory.Slots[x].InvLabel, () => SylviaClothingTalk(x));

                    foundItem = true;
                }

            if (!foundItem)
                OutputText("Scenes.Sylviascene.GiveSylviaClothes.NoAppropriateItems".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => SylviaMenu());
        }

        public void SylviaClothingTalk(int slot)
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaClothingTalk.RequestedSomething".Localized());

            Player.Inventory.Slots[slot].RemoveOneItem();

            ClearButtons();

            AddNextButton("Common.Button.Playfully".Localized(), () => SylviaClothingTalk2(1));

            AddNextButton("Common.Button.Straightforward".Localized(), () => SylviaClothingTalk2(2));
        }

        public void SylviaClothingTalk2(int choice)
        {

            if (choice == 1)
            {
                OutputText("Scenes.Sylviascene.SylviaClothingTalk2.DeviousIdeaBehindBack".Localized());

                OutputText("Scenes.Sylviascene.SylviaClothingTalk2.GetsFrustratedCrossingArms".Localized((Player.Corruption > 50 ? "Common.Text.ThrillsYouButYouKnowYou".Localized() : "Common.Text.MakesYouFeelABitGuilty".Localized())));

                SylviaDom(-5);
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaClothingTalk2.NervousPullOutGift".Localized());

                OutputText("Scenes.Sylviascene.SylviaClothingTalk2.DoneWordlesslyFloatsHugs".Localized());

                SylviaDom(5);
            }

            OutputText("Scenes.Sylviascene.SylviaClothingTalk2.PleasedWithGiftExamines".Localized());

            OutputText("Scenes.Sylviascene.SylviaClothingTalk2.ConversationWindsDownLeave".Localized());

            SylviaAff(15);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaMothTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMothTalk.StrikesNeverSeenMoth".Localized((Save.GetInt("doloresProgress", 0) > 0 ? "Common.Text.BesidesDoloresObviously".Localized() : "Common.Text.NotEvenOnce".Localized())));

            OutputText("Scenes.Sylviascene.SylviaMothTalk.FingerToChinAskance".Localized());

            OutputText("Scenes.Sylviascene.SylviaMothTalk.LetsWordLingerDowncast".Localized((Save.GetInt("doloresProgress", 0) > 0 ? "We" : "I"), (Save.GetInt("doloresProgress", 0) > 0 ? "s" : "")));

            if (Save.GetBool("talkedMoths"))
            {
                OutputText("Scenes.Sylviascene.SylviaMothTalk.SmallQuiverComposesHerself".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalk.DoYouDifferentTopic".Localized());

                DoNext(() => SylviaTalkMenu());
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaMothTalk.ConfidentFaceTries".Localized());

                Save.SetBool("talkedMoths", true);

                ClearButtons();

                AddNextButton("Common.Button.Protect".Localized(), () => SylviaMothTalkAnswer(0));

                AddNextButton("Common.Button.Respect".Localized(), () => SylviaMothTalkAnswer(1));

                AddNextButton("Common.Button.Hug".Localized(), () => SylviaMothTalkAnswer(2));

                AddNextButton("Common.Button.Nothing".Localized(), () => SylviaMothTalkAnswer(3));
            }
        }

        public void SylviaMothTalkAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.SightDowncastTooMuch".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.EachWordFaceMorphs".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.SnifflesNeverForget".Localized());

                SylviaDom(-10);
                }

                break;

                case 1:
                {
                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.ThinkingStartTellImpressed".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.FinishedLetsSilenceHang".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.FluttersCloserArmsSlipping".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.RemainsHuffingEar".Localized());

                SylviaDom(10);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.CantStandSweepUp".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.FewMinutesWhispersThank".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.WasThereAnythingTalk".Localized());

                SylviaAff(10);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.LetConversationDissipate".Localized());

                OutputText("Scenes.Sylviascene.SylviaMothTalkAnswer.PerksUpUncannySmile".Localized());
                }

                break;
            }

            DoNext(() => SylviaTalkMenu());
        }

        public void SylviaBogTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaBogTalk.AskHowLikesHere".Localized());

            OutputText("Scenes.Sylviascene.SylviaBogTalk.LooksQuizzicallyThinks".Localized());

            OutputText("Scenes.Sylviascene.SylviaBogTalk.PausesBeforeContinuing".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Invite".Localized(), () => SylviaBogTalkAnswer(true));

            AddNextButton("Scenes.Sandwitchscene.SammitchBirthsDriders.CuriousWord".Localized(), () => SylviaBogTalkAnswer(false));
        }

        public void SylviaBogTalkAnswer(bool answer)
        {
            ClearOutput();

            if (answer)
            {
                OutputText("Scenes.Sylviascene.SylviaBogTalkAnswer.MakeOfferFaceSlackens".Localized());

                OutputText("Scenes.Sylviascene.SylviaBogTalkAnswer.EyesGrowIntensePools".Localized());

                OutputText("Scenes.Sylviascene.SylviaBogTalkAnswer.NoSaysDefinitively".Localized());

                OutputText("Scenes.Sylviascene.SylviaBogTalkAnswer.LetsPointTrailSatisfied".Localized());
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaBogTalkAnswer.NoncommittalPassingThought".Localized());

                OutputText("Scenes.Sylviascene.SylviaBogTalkAnswer.WillThatBeAll".Localized());
            }

            DoNext(() => SylviaTalkMenu());
        }

        public void SylviaHomeTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaHomeTalk.KnownWhileMeetingsOutside".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk.StuttersNervous".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk.TellLikeVisitWhimper".Localized((Player.Tallness > 65 ? "Common.Text.Chest".Localized() : "Common.Text.Head".Localized())));

            DoNext(() => SylviaHomeTalk2());
        }

        public void SylviaHomeTalk2()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaHomeTalk2.SideBySideHums".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk2.SurprisinglyWarmImpressed".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk2.AssureShellGreatBlushes".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk2.CorridorBackMainRoom".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk2.PlowsIntoHugTightly".Localized((Save.GetInt("sylviaClothes", 0) > 1 ? "Common.Text.FromThisAngleYouCanNow".Localized() : "Common.Text.PgAsYouReenterTheRoom".Localized()), (Player.Corruption > 50 ? "Common.Text.ExcitedBy".Localized() : "Common.Text.UncertainAbout".Localized())));

            ClearButtons();

            AddNextButton("Common.Button.ThereIs".Localized(), () => SylviaHomeTalk3());

            AddNextButton("Common.Button.NotSure".Localized(), () => SylviaHomeTalk3(1));
        }

        public void SylviaHomeTalk3(int choice = 0)
        {
            ClearOutput();

            if (choice == 0)
            {
                OutputText("Scenes.Sylviascene.SylviaHomeTalk3.ExplainDutiesCantAbandon".Localized());

                SylviaDom(-5);
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaHomeTalk3.StruckByAlluringProposal".Localized());

                OutputText("Scenes.Sylviascene.SylviaHomeTalk3.DevilishSmileChillingWords".Localized());

                SylviaDom(5);
            }

            OutputText("Scenes.Sylviascene.SylviaHomeTalk3.ExitingCaveMemorizeLandmarks".Localized());

            OutputText("Scenes.Sylviascene.SylviaHomeTalk3.MothCaveAddedPlaces".Localized());

            Save.SetInt("sylviaProgress", 4);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaDoloresTalk()
        {
            ClearOutput();

            if (Save.GetInt("doloresProgress", 0) < 2)
            {
                OutputText("Scenes.Sylviascene.SylviaDoloresTalk.RespondsAlmostBeforeWords".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalk.AsksExplainMore".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalk.GigglesEyesSoBig".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalk.FinishesSmellsLikeYou".Localized());
            }
            else
                if (Save.GetInt("doloresProgress", 0) < 9)
                {
                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.AskHowSheFeels".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.SaysQuiteSimplyLove".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.SupposeBetterAnswerWorry".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.WavesHandShesFine".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.SmileGrowsEvenWider".Localized());
                }
                else
                {
                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.WellWayOwnPerson".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.SaysLifeChangingUnexpected".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.LooksUpBigEyes".Localized());

                    OutputText("Scenes.Sylviascene.SylviaDoloresTalk.SpendsFewMomentsContemplative".Localized());
                }

            OutputText("Scenes.Sylviascene.SylviaDoloresTalk.LetsOutWistfulSigh".Localized());

            ClearButtons();

            AddNextButton("Scenes.Options.Option.MoreKids".Localized(), () => SylviaDoloresTalkAnswer(0));

            AddNextButton("Common.Button.Jealousy".Localized(), () => SylviaDoloresTalkAnswer(1));

            AddNextButton("Common.Button.You".Localized(), () => SylviaDoloresTalkAnswer(2));

            AddNextButton("CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), SylviaTalkMenu);
        }

        public void SylviaDoloresTalkAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.SeemedHappyWantAnother".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.SmilesEyesDowncast".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.SighsBitPutOut".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.AnswersExpressionGrows".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.ThighsGrindIdeas".Localized());

                AddButton(0, "Scenes.Options.Option.MoreKids".Localized(), null);
                }

                break;

                case 1:
                {
                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.NoAroundLessAttention".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.SaysNo".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.AboutAskBurningEyes".Localized());

                AddButton(1, "Common.Button.Jealousy".Localized(), null);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.AskIfFatherWants".Localized((SceneInstance<Places.MothCave.DoloresScene>.Get().doloresTimesLeft > 0 ? "Common.Text.FlickerForJustAMoment".Localized() : "Common.Text.Crinkle".Localized())));

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.NotJustGiftWonderful".Localized());

                OutputText("Scenes.Sylviascene.SylviaDoloresTalkAnswer.FourHandsTakeUp".Localized());

                AddButton(2, "Common.Button.You".Localized(), null);
                }

                break;
            }
        }

        public void SylviaMassage()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMassage.StressDoneNumber".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage.ApprehensionMassageSoundsGood".Localized((Player.Pregnancy.Event > 2 ? ", and the small bump pressing against you reminds you of the third participant in this embrace" : "")));

            OutputText("Scenes.Sylviascene.SylviaMassage.AcceptingTripInsensateState".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage.ProcuresOilSpreading".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage.GroanWorksMagicKneads".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage.FeelLikeDrowningPressure".Localized());

            DoNext(() => SylviaMassage2());
        }

        public void SylviaMassage2()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMassage2.InvoluntaryGroanSetsOff".Localized((Player.HasCock() ? "Common.Text.GrabsHoldOfTheRagingErection".Localized() : "Common.Text.CupsYourCrotchAlertingYouTo".Localized()), (Player.HasCock() ? "Scenes.DesertCave.Lionpaws.PrickWord".Localized() : "Common.Text.Lips".Localized())));

            if (Player.Cocks.Count > 2)
                OutputText("Scenes.Sylviascene.SylviaMassage2.FaceDownAwkward".Localized());
            else
                OutputText("Scenes.Sylviascene.SylviaMassage2.FaceDownAwkwardAgain".Localized((Player.HasCock() ? "Common.Text.ButSylviaDoesntSeemToMind".Localized() : "Common.Text.SoSylviaSpreadsYourLegs".Localized()), (Player.HasCock() ? "Common.Text.Milks".Localized() : "Common.Text.Schlicks".Localized()), (Player.HasCock() ? "Common.Text.IfHasballsOneFormsARing".Localized() : "Common.Text.OneGetsAFirmHoldOn".Localized()), (Player.HasCock() ? "Common.Text.Dick".Localized() : "Common.Text.Clit".Localized())));

            OutputText("Scenes.Sylviascene.SylviaMassage2.LuxuriousTreatmentAltersPace".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage2.LusciousLuridVoiceEchoes".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage2.EruptStartGasps".Localized((Player.HasCock() ? "Common.Text.SprayingYourSeed".Localized() : "Common.Text.Squirting".Localized()), (Player.HasCock() ? "Common.Text.RopeAfterRopeOfCum".Localized() : "Common.Text.SpurtAfterSpurtOfFemcum".Localized()), (Player.CumQ() > 500 ? "Common.Text.ProducingItFarInExcessOf".Localized() : "Common.Text.AbsolutelyCoatingTheMothsBed".Localized())));

            Player.HasCock();

            Player.HasCock();

            DoNext(() => SylviaMassage3());
        }

        public void SylviaMassage3()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMassage3.NoIdeaLongState".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage3.SomeLaterAwakenTucked".Localized());

            OutputText("Scenes.Sylviascene.SylviaMassage3.SmugGrinNeverWavering".Localized((Player.Pregnancy.Event > 2 ? ", given that I've been a bit... 'burdened' as of late" : "")));

            SylviaDom(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaMeal()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMeal.TalkingNothingRealize".Localized());

            OutputText("Scenes.Sylviascene.SylviaMeal.GleefulLookMeltsHeart".Localized());

            OutputText("Scenes.Sylviascene.SylviaMeal.SitDownStartReading".Localized());

            OutputText("Scenes.Sylviascene.SylviaMeal.LaidOutTableHewn".Localized());

            OutputText("Scenes.Sylviascene.SylviaMeal.UnableToContainSteers".Localized());

            OutputText("Scenes.Sylviascene.SylviaMeal.ContentsWithSmallMelon".Localized());

            OutputText("Scenes.Sylviascene.SylviaMeal.SupremelySatisfiedAskWhat".Localized());

            SylviaDom(-5);

            Player.RefillHunger(50);

            ClearButtons();

            AddNextButton("Common.Button.YesPlease".Localized(), SyvliaMealSex);

            AddNextButton("Common.Button.YoureFull".Localized(), SylviaMealLeave);
        }

        public void SyvliaMealSex()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SyvliaMealSex.NodApprovalLaunchesTable".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Sylviascene.SyvliaMealSex.TakesIntoHandsRubbing".Localized((!Player.IsNaked() ? "The moth-girl eventually manages to get your [armor] out of the way, allowing your [cock] to flop free. " : ""), (!Player.IsNaked() ? "Common.Text.It".Localized() : "Common.Text.YourCock".Localized())));

                OutputText("Scenes.Sylviascene.SyvliaMealSex.MoaningNameBetterMouth".Localized((Player.Pregnancy.Event > 2 ? ", her plump belly pressing against you as she pulls you tight" : "")));

                OutputText("Scenes.Sylviascene.SyvliaMealSex.SurgePassionLiftUp".Localized((Player.Pregnancy.Event > 2 ? "Common.Text.GentlyLowerHerSwollenBodyOnto".Localized() : "Common.Text.PinHerToTheTable".Localized())));

                OutputText("Scenes.Sylviascene.SyvliaMealSex.NearingLimitHoldBack".Localized());

                Player.Orgasm("Dick");

                SylviaKnockupAttempt();
            }
            else
            {
                OutputText("Scenes.Sylviascene.SyvliaMealSex.TakesHandOntoChest".Localized((!Player.IsNaked() ? "The moth-girl eventually manages to get your [armor] out of the way, allowing your [vagina] to breath. " : "")));

                OutputText("Scenes.Sylviascene.SyvliaMealSex.MoaningBetterMouthAgain".Localized((Player.Pregnancy.Event > 2 ? ", her plump belly pressing against you as she pulls you tight" : "")));

                OutputText("Scenes.Sylviascene.SyvliaMealSex.SurgePassionLiftAgain".Localized((Player.Pregnancy.Event > 2 ? "Common.Text.GentlyLowerHerSwollenBodyOnto".Localized() : "Common.Text.PinHerToTheTable".Localized())));

                OutputText("Scenes.Sylviascene.SyvliaMealSex.NearingLimitBackAgain".Localized());

                Player.Orgasm("Vaginal");

                SylviaKnockupAttempt(false);
            }

            OutputText("Scenes.Sylviascene.SyvliaMealSex.PullSylviaSlumpChair".Localized((Player.Corruption > 50 ? "Common.Text.NotReadyToGiveUpHer".Localized() : "Common.Text.WorriedAboutTheStructuralIntegrityOf".Localized()), (Player.Pregnancy.Event > 2 ? "She tenderly rests on hand on her stomach, a motherly smile gracing her face. " : "")));

            OutputText("Scenes.Sylviascene.SyvliaMealSex.PickUpNotAwaken".Localized((Player.Pregnancy.Event > 2 ? ", although slightly increased by the additional passenger," : "")));

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaMealLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMealLeave.ThankCantEatMore".Localized());

            OutputText("Scenes.Sylviascene.SylviaMealLeave.InitialSurpriseQuickRecover".Localized());

            OutputText("Scenes.Sylviascene.SylviaMealLeave.ExpressGratitudeFlutters".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaCuddle()
        {
            ClearOutput();

            // 原版 SylviaScene.hx:1265：有小屋床时是「your own bed」，否则「your bedroll」
            OutputText("Scenes.Sylviascene.SylviaCuddle.JustLikeToCuddle".Localized(
                Player.Pregnancy.Event > 2 ? "Common.Text.HerBabyWeightClearlyHamperingHer".Localized() : "Common.Text.WithALittleFlourish".Localized(),

                CampScene.HasCabinBed ? "Common.Text.YourOwnBed".Localized() : "Common.Text.YourBedroll".Localized()));

            OutputText("Scenes.Sylviascene.SylviaCuddle.ManageToSettleIn".Localized((Player.Tallness > 108 ? "Common.Text.ThoughItsABitCramped".Localized() : "Common.Text.SideBySide".Localized()), (sylviaGetDom < 50 ? "Common.Text.YouNaturallyAreTheBigSpoon".Localized() : "Common.Text.SylviaNaturallyIsTheBigSpoon".Localized()), (Player.Pregnancy.Event > 2 ? (sylviaGetDom < 50 ? "Common.Text.YouSlipAnArmAroundHer".Localized() : "Common.Text.YouFeelHerBellyPressingAgainst".Localized()) + ", comforted by the presence of your child." : "")));

            OutputText("Scenes.Sylviascene.SylviaCuddle.MomentEverythingRight".Localized());

            OutputText("Scenes.Sylviascene.SylviaCuddle.BlissComfortablyClose".Localized((sylviaGetDom < 50 ? "Common.Text.TurningHerHeadTowardsYou".Localized() : "Common.Text.MovingHerMouthUpToYour".Localized())));

            OutputText("Scenes.Sylviascene.SylviaCuddle.WellHaveToRespond".Localized((sylviaGetDom < 50 ? "Common.Text.SomewhatBegrudginglyExplain".Localized() : "Common.Text.NervouslyStammerOut".Localized())));

            OutputText("Scenes.Sylviascene.SylviaCuddle.WaitsMomentBeforeContinuing".Localized());

            if (sylviaGetDom < 25)
                OutputText("Scenes.Sylviascene.SylviaCuddle.MakeMeFeelWhole".Localized());
            else
                if (sylviaGetDom < 75)
                    OutputText("Scenes.Sylviascene.SylviaCuddle.JustLoveNothingChange".Localized());
                else
                    OutputText("Scenes.Sylviascene.SylviaCuddle.YoureMineAllMine".Localized());

            OutputText("Scenes.Sylviascene.SylviaCuddle.QuietWordsCalmHeart".Localized());

            OutputText("Scenes.Sylviascene.SylviaCuddle.MindAtEaseRelax".Localized((sylviaGetDom < 50 ? "Common.Text.YouEncircleTheMothGirlWith".Localized() : "Common.Text.TheMothGirlEncirclesYouWith".Localized())));

            OutputText("Scenes.Sylviascene.SylviaCuddle.WakeRefreshedRiseStretch".Localized((View.Time.Hours > 18 ? "Common.Text.TheNextMorning".Localized() : "Common.Text.AFewHoursLater".Localized())));

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void SylviaThreesomeTalk()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaThreesomeTalk.TryThinkHowApproach".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeTalk.MildlyConfusedLook".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeTalk.QuiteUnexpectedClarification".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeTalk.SmirkingDevilishlyBesides".Localized());

            Save.SetBool("unlockedOyakodon", true);

            ClearButtons();

            AddNextButton("Common.Button.Assure".Localized(), SylviaThreesomeAssure);

            AddNextButton("Common.Button.BrushOff".Localized(), SylviaThreesomeDeflect);
        }

        public void SylviaThreesomeAssure()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaThreesomeAssure.PutAroundDrawIn".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeAssure.ThankYouDearSuppose".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeAssure.StayInEmbraceWarmth".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeAssure.WasThereAnythingElse".Localized());

            DoNext(() => SylviaTalkMenu());
        }

        public void SylviaThreesomeDeflect()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaThreesomeDeflect.AwkwardlySlipAwayPat".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeDeflect.ILovePatBack".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeDeflect.TakesTimeBreakOff".Localized());

            OutputText("Scenes.Sylviascene.SylviaThreesomeDeflect.SettlesThatSuppose".Localized());

            DoNext(() => SylviaTalkMenu());
        }

        public void SylviaKiss()
        {
            ClearOutput();

            if (sylviaGetDom < 25)
            {
                OutputText("Scenes.Sylviascene.SylviaKiss.SweepOffFeetKiss".Localized((Player.Pregnancy.Event > 2 ? ", letting you feel her burgeoning baby-bump" : "")));

                OutputText("Scenes.Sylviascene.SylviaKiss.WarmedByWordsAnother".Localized((Player.Tallness > 60 ? " [chest]" : "s")));
            }
            else
                if (sylviaGetDom < 50)
                {
                    OutputText("Scenes.Sylviascene.SylviaKiss.AskingKissBlushesNods".Localized());

                    OutputText("Scenes.Sylviascene.SylviaKiss.LipsContactHeartPounding".Localized((Player.Pregnancy.Event > 2 ? "her body the picture of maternity, " : "")));
                }
                else
                    if (sylviaGetDom < 75)
                    {
                        OutputText("Scenes.Sylviascene.SylviaKiss.AskKissSmugGrin".Localized((Player.Pregnancy.Event > 2 ? ", her maternal glow quite noticeable" : "")));

                        OutputText("Scenes.Sylviascene.SylviaKiss.SoftLipsIndescribableTongue".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sylviascene.SylviaKiss.HesitantlyAskSwoopsIn".Localized((Player.Pregnancy.Event > 2 ? " against her swollen belly" : "")));

                        OutputText("Scenes.Sylviascene.SylviaKiss.TakesHeadGuidesLips".Localized((Player.Pregnancy.Event > 2 ? ", but the reassuring presence of her puffy stomach makes you happy to be so close to the Mother of your child" : "")));

                        OutputText("Scenes.Sylviascene.SylviaKiss.ComeToSensesGroggily".Localized());
                    }

            OutputText("Scenes.Sylviascene.SylviaKiss.SatisfiedWaveGoodbye".Localized());

            SylviaAff(10);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSpar()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSpar.LikeToSpar".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpar.SubduedAssumesStance".Localized((sylviaProg >= 4 ? "Common.Text.TheMothGirlLeadsYouOut".Localized() : "Common.Text.TheMothGirlLeadsYouTo".Localized())));

            var monster = new SylviaEnemy
            {
                OnDefeated = _ => SylviaVictory(),

                OnWon = _ => SylviaDefeat()
            };

            CombatScene.Start(monster, () => Go<CampScene>());
        }

        public void SylviaSparLeave()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSparLeave.OnlyInterestedHelpFeet".Localized());

            OutputText("Scenes.Sylviascene.SylviaSparLeave.IsThatSo".Localized((sylviaGetDom < 50 ? "Common.Text.ADisappointedLookOnHerFace".Localized() : "Common.Text.AFrustratedLookOnHerFace".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSparLeave.PartAwkwardlyHeadsOff".Localized((sylviaProg >= 4 ? "Common.Text.PartsUnknown".Localized() : "Common.Text.HerHome".Localized())));

            CombatScene.CleanupAfterCombat();
        }

        public void SylviaSexMenu()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSexMenu.YouSaunterUpTo".Localized());

            ClearButtons();

            AddNextButton("Common.Button.MatingPress".Localized(), SylviaMatingPress);

            AddNextButton("Common.Button.ReverseCowgirl".Localized(), SylviaReverseCowgirl);

            AddNextButton("Common.Button.Fluffjob".Localized(), SylviaFluffjob);

            AddNextButton("Common.Button.ProbTonguing".Localized(), SylviaProbTongue);

            AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), SylviaTribbing);

            if (sylviaGetAff >= 50)
                AddNextButton("Common.Button.Tease".Localized(), SylviaTeasing);

            if (Save.GetBool("unlockedOyakodon"))
                AddNextButton("Common.Button.Threesome".Localized(), SylviaOyakodon);

            AddButton(14, "Common.Button.Back".Localized(), () => SylviaMenu());
        }

        public void SylviaMatingPress()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaMatingPress.FirmLookTakeYou".Localized());

            OutputText("Scenes.Sylviascene.SylviaMatingPress.SoYoudLikeTop".Localized());

            OutputText("Scenes.Sylviascene.SylviaMatingPress.StepForwardPullTight".Localized((Player.Pregnancy.Event > 2 ? "pregnant" : "")));

            OutputText("Scenes.Sylviascene.SylviaMatingPress.AllArmsStripArmor".Localized((!Player.IsNaked() ? "You don't help the process along much with your constant attempts at fondling the impatient moth, but eventually, she manages to get you completely nude. " : ""), (sylviaProg >= 4 ? "Common.Text.HerBed".Localized() : "Common.Text.TheGround".Localized()), (Player.Pregnancy.Event > 2 ? ", careful not to be too rough" : ""), (Player.Pregnancy.Event > 2 ? "Common.Text.Fuck".Localized() : "Common.Text.Breed".Localized())));

            OutputText("Scenes.Sylviascene.SylviaMatingPress.NeedHerThighsUp".Localized());

            OutputText("Scenes.Sylviascene.SylviaMatingPress.PullOutSlowlyMoan".Localized((sylviaProg >= 4 ? "Common.Text.Cave".Localized() : "Common.Text.Swamp".Localized())));

            OutputText("Scenes.Sylviascene.SylviaMatingPress.ThrustLikeMadman".Localized());

            OutputText("Scenes.Sylviascene.SylviaMatingPress.NearingLimitLipsLock".Localized((Player.Pregnancy.Event > 2 ? "bloated belly" : "")));

            OutputText("Scenes.Sylviascene.SylviaMatingPress.PowerfulGroanSlam".Localized((Player.Body.Get<BodyParts.Vanilla.HairPart>().Length > 0 ? "Common.Text.RuffleYourHair".Localized() : "Common.Text.RubYourHead".Localized())));

            OutputText("Scenes.Sylviascene.SylviaMatingPress.CantMoveCollapse".Localized((Player.Pregnancy.Event > 2 ? "Common.Text.SwollenStomach".Localized() : "Common.Text.LeakingNethers".Localized())));

            SylviaAff(10);

            SylviaDom(-5);

            Player.Orgasm("Dick");

            SylviaKnockupAttempt();

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaReverseCowgirl()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.ConsideringWhatSaySidles".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.SultryPurrsTakeCare".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.PushesStumbleOntoAss".Localized((Player.Pregnancy.Event > 2 ? "Common.Text.Gravid".Localized() : "Common.Text.Stunning".Localized())));

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.ApprehensionSittingOnCock".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.FeelTwoArms".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.LetsNotAheadOurselves".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.MollifiedPromiseRelease".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.IncredibleChemicals".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.RidesHardSmacking".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.NotSureHowMore".Localized());

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.BlowLoadDutiful".Localized((Player.CumQ() > 500 ? "Common.Text.OverflowsSylviasCanalSplatteringTheGround".Localized() : "Common.Text.FloodsSylviasCanalLeakingOutA".Localized())));

            OutputText("Scenes.Sylviascene.SylviaReverseCowgirl.ByTimeStoppedExhausted".Localized());

            SylviaAff(10);

            SylviaDom(5);

            Player.Orgasm("Dick");

            SylviaKnockupAttempt();

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaFluffjob()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaFluffjob.AskServiceWithHands".Localized());

            OutputText("Scenes.Sylviascene.SylviaFluffjob.RespondsKneelsInFront".Localized((Player.Pregnancy.Event > 2 ? "Common.Text.SayAndThatSoundsLikeIt".Localized() : "Common.Text.SayEspeciallyWhenYoureSoDelicious".Localized()), (!Player.IsNaked() ? "until your [cock] pops free" : "")));

            OutputText("Scenes.Sylviascene.SylviaFluffjob.TwelveFingersPumping".Localized());

            OutputText("Scenes.Sylviascene.SylviaFluffjob.DecidesTimeGetIt".Localized((Player.ThickestCockThickness() > 4 ? "Common.Text.ButSheStrugglesToFitHer".Localized() : "Common.Text.AndHerFingersWrapAroundYou".Localized())));

            OutputText("Scenes.Sylviascene.SylviaFluffjob.NiceWetPullsBack".Localized());

            OutputText("Scenes.Sylviascene.SylviaFluffjob.SoftLaughEnjoyingOurselves".Localized());

            OutputText("Scenes.Sylviascene.SylviaFluffjob.TrueToWordSkillful".Localized());

            OutputText("Scenes.Sylviascene.SylviaFluffjob.CantBackEruptingFur".Localized());

            OutputText("Scenes.Sylviascene.SylviaFluffjob.RecoverFurMatted".Localized((sylviaGetDom < 50 ? "Common.Text.LightlyTeaseHer".Localized() : "Common.Text.SheepishlyApologize".Localized())));

            SylviaAff(5);

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaProbTongue()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaProbTongue.IndecentIdeaLongTongue".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.CompliesWithoutFussSituates".Localized((sylviaProg >= 4 ? "Common.Text.HerBed".Localized() : "Common.Text.AConvenientRock".Localized()), (!Player.IsNaked() ? ", telling her to undress you" : "")));

            OutputText("Scenes.Sylviascene.SylviaProbTongue.GetsThighsSlowsDown".Localized((Player.IsNaked() ? "Having reached bare [skindesc]," : "")));

            OutputText("Scenes.Sylviascene.SylviaProbTongue.HadEnoughTeasingLegs".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.LetOutMoanWork".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.ChancingGlanceTopHead".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.TakeItSlowlyFingers".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.NotOverlyDeterredSenseless".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.DamBreaksErrantClit".Localized());

            OutputText("Scenes.Sylviascene.SylviaProbTongue.RecoveringTiringSessionKiss".Localized());

            SylviaAff(10);

            SylviaDom(-5);

            Player.Orgasm("Vaginal");

            SylviaKnockupAttempt(false);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaTribbing()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaTribbing.SimplyStaresRepeatRequest".Localized((sylviaProg >= 4 ? "Common.Text.TheWall".Localized() : "Common.Text.ATree".Localized())));

            OutputText("Scenes.Sylviascene.SylviaTribbing.NotNextMoveContent".Localized());

            OutputText("Scenes.Sylviascene.SylviaTribbing.WithoutWarningGraspsSwings".Localized((sylviaProg >= 4 ? "Common.Text.HerBed".Localized() : "Common.Text.ANearbyPatchOfGrass".Localized()), (!Player.IsNaked() ? "It's not long before they've completely stripped you of your [armor], your now exposed [skindesc] shivering." : "")));

            OutputText("Scenes.Sylviascene.SylviaTribbing.ThenTheyStopPheromones".Localized());

            OutputText("Scenes.Sylviascene.SylviaTribbing.WillYouBeg".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), () => SylviaTribbing2(1));

            AddNextButton("Common.Button.No".Localized(), () => SylviaTribbing2(2));
        }

        public void SylviaTribbing2(int choice)
        {
            ClearOutput();

            if (choice == 1)
            {
                OutputText("Scenes.Sylviascene.SylviaTribbing2.TooShakyBreathEntreat".Localized());

                OutputText("Scenes.Sylviascene.SylviaTribbing2.DragsFingerFromNavel".Localized());
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaTribbing2.WontGiveSatisfactionPredatory".Localized());

                OutputText("Scenes.Sylviascene.SylviaTribbing2.DragsFingerScratching".Localized());
            }

            OutputText("Scenes.Sylviascene.SylviaTribbing2.HumiliationWorthItTalented".Localized());

            OutputText("Scenes.Sylviascene.SylviaTribbing2.JustAsGoodSlows".Localized());

            OutputText("Scenes.Sylviascene.SylviaTribbing2.WillingGiveHipsShift".Localized());

            OutputText("Scenes.Sylviascene.SylviaTribbing2.LongTimeComeDown".Localized((sylviaProg >= 4 ? "Common.Text.Cave".Localized() : "Common.Text.Bog".Localized())));

            OutputText("Scenes.Sylviascene.SylviaTribbing2.StrainedMindRecuperates".Localized());

            SylviaAff(10);

            SylviaDom(5);

            Player.Orgasm("Vaginal");

            SylviaKnockupAttempt(false);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaOyakodon()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaOyakodon.TellParticularShowPat".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.HmmSoundsFunNever".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.PlantsBriefTakesHold".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.WalkSideDoloresRoom".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.BeforeRespondSylviaSwoops".Localized(
                Player.Mf("Hello Mother, Father", "Good [day] to you both")
            ));

            OutputText("Scenes.Sylviascene.SylviaOyakodon.MMotherWhatDoing".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.SmirksGivingPleasureOwed".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.CantFormulateBeckonsOver".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.SoDistractedDoesntReact".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.NNoICant".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.OlderMothFingerLips".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.WhateverCaseNudeFramed".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodon.ApproachDoloresLooksDown".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            if (Player.IsHerm())
            {
                ClearButtons();

                AddNextButton("Common.Button.UseCock".Localized(), () => SylviaOyakodon2(true));

                AddNextButton("Common.Button.UsePussy".Localized(), () => SylviaOyakodon2(false));
            }
            else
                SylviaOyakodon2(Player.HasCock());
        }

        public void SylviaOyakodon2(bool cock)
        {
            ClearOutput();

            if (cock)
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodon2.PreparedLeadToBed".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.ANnnNotLike".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.TrainOfThoughtBroken".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.QuiteEnjoyingTwitchingPrick".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.SlipsIntoMouthEnraptured".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.MakeNo".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.CaringMotherMakesComfortable".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.InsertionHeavenlyForeplay".Localized((Player.LongestCockLength() <= 8 ? "Common.Text.AllTheWay".Localized() : "Common.Text.FairlyDeep".Localized())));

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.MomentToBreatheAdjust".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.SeeingMovementsKissNeck".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.ContinueFondlingTongue".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.LastHandsSlipDown".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.StartToOrgasmLatches".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.FinallySpentFreesMember".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.DespiteDrainedStillRaring".Localized());

                Player.Orgasm("Cock");
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodon2.PressAgainstFrontBack".Localized(
                    Player.Mf("parents", "mothers")
                ));

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.StepBackAlmostCollapses".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.StartsButDoesntKnow".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.GentlySootheLeadBed".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.WantPushBackFirmly".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.PantsOutPoorDaughter".Localized(
                    Player.Mf("D-Dad", "M-Mom")
                ));

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.LegsTangleCouldnt".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.TwoHandsFindHips".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.CausesGaspNeedTo".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.SlickAppendagePlaysFlits".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.LashingTongueSkinSteal".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.ShiveringAirThickMuggy".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.MusclesClenchGoLimp".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodon2.DespiteDrainedRaringAgain".Localized());

                Player.Orgasm("Vaginal");
            }

            ClearButtons();

            AddNextButton("Common.Button.Continue".Localized(), () => SylviaOyakodonContinue(cock));

            AddNextButton("Common.Button.Finished".Localized(), () => SylviaOyakodonFinished(false));
        }

        public void SylviaOyakodonContinue(bool cock)
        {
            ClearOutput();

            if (cock)
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.ReturnSmileSomethingElse".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.YelpsLookingAroundWildly".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.AhemExhaustingLeaveBed".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.InformNotEveryoneSatisfied".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.HandsToShouldersSuppress".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.UUhmMotherI".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.CutsOffShowYou".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.SoftSighBeGentle".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.ContinuesInstructingLittle".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.WithoutFurtherLeadsComfortable".Localized());
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.GiveSlightNodIntentions".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.UmGratefulForThat".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.KissStrugglesSuccumbing".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.TakeHandPullUpright".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue.BedDoesntHaveSpace".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            SylviaOyakodonContinue2(cock);
        }

        public void SylviaOyakodonContinue2(bool cock)
        {
            ClearOutput();

            if (cock)
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.OlderFirstActionHips".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.OneGroundSlapsRump".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ConfirmedPresenceSlidesUp".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.EverythingPlacePushIn".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ElderSwiftlyFollowsLicks".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ContinueRutRippleEffect".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.UnableShakeThought".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.SightClimaxingDaughterMuch".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ThreeStayTimelessMoment".Localized());

                Player.Orgasm("Cock");
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ScarcelyWordCatchesOn".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.DoloresBlushesDoesntAway".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.MidstEnjoyingAromaEep".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.FirstHesitantLickShudder".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.NowSettledReturnAttentions".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ThreeNowLinkedWheel".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.SurprisesMostPassionately".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.OddThumpingBreaksConcentration".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.SecondHighApproachingStave".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.LegsClampLikeVice".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonContinue2.ClimaxSweepsConnection".Localized());

                Player.Orgasm("Vaginal");
            }

            SylviaOyakodonFinished(true, cock);
        }

        public void SylviaOyakodonFinished(bool continued, bool cock = true)
        {
            ClearOutput();

            if (continued && cock)
            {
                OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.ComeDownMeetEyes".Localized());

                OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.SeveralMinutesBeforeSee".Localized((sylviaGetDom > 50 ? "Common.Text.YouFinallyPullBackAndWhen".Localized() : "Common.Text.SylviaFinallyLetsYouGoAnd".Localized())));
            }
            else
                if (continued)
                {
                    OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.SoftWarmPillowComforts".Localized());

                    OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.RegainFacultiesFaintSucking".Localized());

                    OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.PerhapsTooMuch".Localized());
                }
                else
                    OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.UnfortunatelyCantWhatWants".Localized());

            OutputText("Scenes.Sylviascene.SylviaOyakodonFinished.HelpsSlideTuckIn".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaTeasing()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaTeasing.RatherFirstMoveStand".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.DriftCloserSmileLightly".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.ThenStepBack".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.TurningAroundGladNice".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.EyesBottomlessPitsFar".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.DontHaveTimeSoon".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.BoldName".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.RespondDoesntGetThrough".Localized((sylviaProg >= 4 ? "Common.Text.HerBed".Localized() : "Common.Text.YourBackAIfMetricMeter".Localized())));

            OutputText("Scenes.Sylviascene.SylviaTeasing.BoldNameAgain".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.ShiftTorsoArmsPin".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.LeansInCloseWhiff".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.ShiftsHipsLower".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.WedgeLegsUnder".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.LieThereBeneathForce".Localized((sylviaProg >= 4 ? "Common.Text.BreakTheBed".Localized() : "Common.Text.ForceYouIntoTheDirt".Localized())));

            OutputText("Scenes.Sylviascene.SylviaTeasing.FinalExpressionLove".Localized());

            OutputText("Scenes.Sylviascene.SylviaTeasing.WhiteSpotsFadeTrembling".Localized());

            Player.Orgasm("Generic");

            DoNext(() => SylviaTeasing2());
        }

        public void SylviaTeasing2()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaTeasing2.WakeAloneBelongingsBeside".Localized());

            SylviaAff(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSpendTime()
        {
            ClearOutput();

            Save.SetInt("sylviaCapstoneCounter", Save.GetInt("sylviaCapstoneCounter", 0) + 1);

            if (sylviaGetDom < 50)
            {
                OutputText("Scenes.Sylviascene.SylviaSpendTime.TellSimplyHere".Localized());

                switch (Save.GetInt("sylviaCapstoneCounter", 0))
                {
                    case 1:
                    {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.BlinksSurprisedUmSuppose".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ReaffirmJoinOnBed".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.WhatBeenUpTo".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.NotMostNovelRegale".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.AfterAboutTakeOff".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.GrowCloserBold".Localized());

                    DoNext(() => ReturnToCamp(1));
                    }

                    break;

                    case 2:
                    {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.SmilesFaintlyBlushingOkay".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.OftenFeelPeaceDifferent".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ShakenOutReverieChair".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ConversationMovesFutureChipper".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.Savior".Localized(), () => SylviaSpendTime2Answer(0));

                    AddNextButton("Common.Button.Ingnam".Localized(), () => SylviaSpendTime2Answer(1));

                    AddNextButton("Common.Button.Kingdom".Localized(), () => SylviaSpendTime2Answer(2));

                    AddNextButton("Common.Button.WithYou".Localized(), () => SylviaSpendTime2Answer(3));
                    }

                    break;

                    case 3:
                    {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.SmilesAsUsualDifferent".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ConversationsFocusedOnYou".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ContinueLineOfQuestioning".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.KeepGoing".Localized(), SylviaSpendTimeKeepGoing);

                    AddNextButton("Common.Button.DontCare".Localized(), SylviaSpendTimeDontCare);
                    }

                    break;
                }

                if (true )
                {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.BrokenReportThread".Localized());

                    OutputText(Save.GetInt("sylviaCapstoneCounter", 0).ToString());

                    DoNext(() => ReturnToCamp(1));
                }
            }
            else
            {
                OutputText("Scenes.Sylviascene.SylviaSpendTime.TentativelySpendTimeTogether".Localized());

                switch (Save.GetInt("sylviaCapstoneCounter", 0))
                {
                    case 1:
                    {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.EyesWidenTadSurprised".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.PatsOnCheekLovely".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.StartsRightAwayQuestions".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.WantToKnowEverything".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.SlipAfterHourExcuse".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.GrowingWeakerPheromones".Localized());

                    DoNext(() => ReturnToCamp(1));
                    }

                    break;

                    case 2:
                    {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.GrinsLeeringHungryLioness".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.BriefImpulseFleeSubsides".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.NotInterestedUnseemly".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.AlightsOnFutureGroggy".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.HeadLeansAcrossTable".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.LickingLipsForNow".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.SurprisedLettingLeaveSoon".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.GrowingWeakerPheromonesAgain".Localized());

                    DoNext(() => ReturnToCamp(4));
                    }

                    break;

                    case 3:
                    {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.InexplicableSenseOfDread".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ReducedFacultiesSpitOut".Localized((Player.IsChild() ? "Common.Text.Boy".Localized() : "Common.Text.Man".Localized())));

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.KissesCantFeelLips".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.WouldItBadLive".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.RealizeDontKnow".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.OhGodsMisheard".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.KeepYourVoiceDown".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.ShesRightNeedCalm".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.DoNeedLieDown".Localized());

                    OutputText("Scenes.Sylviascene.SylviaSpendTime.YouDoYou".Localized());

                    View.Time.PassMinutes((int)Round((72) * 60));

                    DoNext(() => SylviaSpendTimeMindbreak());
                    }

                    break;
                }

                if (true )
                {
                    OutputText("Scenes.Sylviascene.SylviaSpendTime.BrokenReportThreadAgain".Localized());

                    OutputText(Save.GetInt("sylviaCapstoneCounter", 0).ToString());

                    DoNext(() => ReturnToCamp(1));
                }
            }
        }

        public void SylviaSpendTime2Answer(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.FightingBackDemonsPurging".Localized());

                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.SaysSurprisedSeriousness".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.BackHomeGrewUp".Localized());

                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.EyesDowncastWonderfulGoal".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.SeatOfPowerRuling".Localized());

                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.SaysSmirkAmbition".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.RightByHerSide".Localized());

                OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.ClearlyStunnedDirectBlush".Localized());
                }

                break;
            }

            OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.TalkSeveralMoreHours".Localized((choice == 4 ? "Common.Text.TellHerExactlyWhat".Localized() : "Common.Text.GiveHerAnAnswer".Localized())));

            OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.WontKeepYouLong".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.AssureWontQuickKiss".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTime2Answer.GrowCloserBold".Localized());

            DoNext(() => ReturnToCamp(4));
        }

        public void SylviaSpendTimeKeepGoing()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.CantBeCaseResolve".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.SometimesIRead".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.EmboldenedDontLetUp".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.LoveClassicalRomances".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.AmbushTacticsEffective".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.NotComplexRoughSketch".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.AndSoWhiledAway".Localized(
                View.Time.Hours < 12 ? "morning" : (View.Time.Hours < 19 ? "Common.Text.Afternoon".Localized() : "Common.Text.Evening".Localized())
            ));

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.ReallyDoLoveThanks".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.DeepPassionateKissBroken".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeKeepGoing.NewLevelOfIntimacy".Localized());

            ReturnToCamp(1);
        }

        public void SylviaSpendTimeDontCare()
        {

            OutputText("Scenes.Sylviascene.SylviaSpendTimeDontCare.ShrugMoveOnTopic".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeDontCare.ThankButGoNow".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeDontCare.CloseToNewDevelopment".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SylviaSpendTimeMindbreak()
        {
            ClearOutput();

            View.Time.PassMinutes((int)Round((24) * 60));

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak.TentativelySpendTimeTogether".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak.UnderstandComeOverHere".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak.WantNotSureLegs".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak.ThereThereGotYou".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak.LeadsToTableChatting".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak.TooDizzyBackDown".Localized());

            DoNext(() => SylviaSpendTimeMindbreak2());
        }

        public void SylviaSpendTimeMindbreak2()
        {
            ClearOutput();

            View.Time.PassMinutes((int)Round((24) * 60));

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.TentativelySpendTimeTogether".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.DearComeToMe".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.WrapsEnvelopingArmsSafe".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.FrownsAnyNeed".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.NotSureAnythingAway".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.SenseBehindTurnAround".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.AreYouAllRight".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.NotShakesHeadChuckles".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.OkayYouGoNow".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.NotSureWhatMeans".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.EverythingOkayShallWell".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.StartToWalkWobbly".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.YoullBeBack".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.ThinkOfHomeHard".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.NeverReallyLeft".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.ThereStumbleBedCollapse".Localized());

            OutputText("Scenes.Sylviascene.SylviaSpendTimeMindbreak2.ILoveYou".Localized());

            ; 
        }

        public void GiftDress()
        {
            ClearOutput();

            OutputText("Scenes.Sylviascene.GiftDress.DressFromUndeadSeamstress".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.WhyThankUnfurlsDrops".Localized((Save.GetInt("sylviaClothes", 0) > 1 ? "Common.Text.AhAnotherTreat".Localized() : "Common.Text.OhForMe".Localized())));

            OutputText("Scenes.Sylviascene.GiftDress.TearsSpillingHersSmell".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.HappyLikesStrongerReaction".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.WistfulSilkSacred".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.TrailsOffSlipsAway".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.RadiantSwathedWhirlwind".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.AsksNervousWhatThink".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.LipsLockSayMore".Localized());

            OutputText("Scenes.Sylviascene.GiftDress.TimePartFondFarewell".Localized());

            Save.SetBool("sylviaGiftedDress", true);

            Player.RemoveKeyItem("Sylvia's Dress");

            DoNext(() => ReturnToCamp(1));
        }

    }
}