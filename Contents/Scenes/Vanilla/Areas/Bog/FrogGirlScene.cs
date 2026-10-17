using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<FrogGirlScene>("coc:frog_girl")]
    public class FrogGirlScene : Scene
    {

        public int taughtLesson = 0;

        public bool metSuwako = false;

        public int lastEncounter = 0;

        public int analMad = 0;

        public int angryLosses = 0;

        public bool fought = false;

        public int eggCount = 0;

        public bool submissive = false;

        public int shownKids = 0;

        public bool pattyCake = false;

        internal void WonHook(bool hpVictory)
        {
            if (Save.GetInt("analMad", 0) != 0) SuwakoAnalWon();
            else SuwakoWon(hpVictory);
        }

        public FrogGirlEncounter LastEncounter
        {
            get => Save.GetEnum("lastEncounter", FrogGirlEncounter.NoSex);

            set => Save.SetEnum("lastEncounter", value);
        }

        public static SaveContent Save => SaveContent(SceneIds.Bog.FrogGirl);

        public string debugName => "Frog-Girl";

        public string debugHint => "";

        public override void Entry()
        {
            ExecEncounter();
        }

        public void Reset()
        {

            Save.SetInt("taughtLesson", 0);

            Save.SetBool("metSuwako", false);

            LastEncounter = 0;

            Save.SetInt("analMad", 0);

            Save.SetInt("angryLosses", 0);

            Save.SetBool("fought", false);

            Save.SetInt("eggCount", 0);

            Save.SetBool("submissive", false);

            Save.SetInt("shownKids", 0);

            Save.SetBool("pattyCake", false);
        }

        public void ExecEncounter()
        {

            if (RandomUtils.Chance((!Player.IsPregnant() || Save.GetInt("shownKids", 0) != 0) ? 33 : 100) && Player.ButtPregnancy.Incubation == 0 && Save.GetInt("taughtLesson", 0) < 2)
                FindTheFrogGirl();
            else
            {
                var __sw0_1 = true;

                if (__sw0_1 == !Save.GetBool("metSuwako"))
                {
                    Suwako();

                    return ;
                }

                if (__sw0_1 == (Save.GetInt("eggCount", 0) >= 15 && !Save.GetBool("fought")))
                {
                    SubmissiveSuwako();

                    return ;
                }

                if (__sw0_1 == (Save.GetInt("analMad", 0) >= 3))
                {
                    SuwakoAngry();

                    return ;
                }

                if (true )
                    SuwakoRepeat();
            }
        }

        public void FindTheFrogGirl()
        {
            ClearOutput();

            ShowImage("froggirl");

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredFrog) == 0)
            {
                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.BeginTrudgeThroughBog".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.BecomeAwareOfA".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.WideRootsOfTrees".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.MoveCloserClimbingUp".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.LimeGreenSkinAnd".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.PursesLipsBlowingOut".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.SmilesThreeFootTongue".Localized());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.ExploringBogBecomeAware".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.WideRootsAgain".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.MoveCloserClimbingAgain".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.PursesLipsBlowingAgain".Localized());

                OutputText("Scenes.Froggirlscene.FindTheFrogGirl.SmilesTongueAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredFrog, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredFrog) + 1);

            OutputText("Scenes.Froggirlscene.FindTheFrogGirl.FollowTheMusic".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Follow".Localized(), FollowDatFrog);

            AddButton(1, "Common.Button.Resist".Localized(), ResistDatFrog);
        }

        public void FollowDatFrog()
        {
            ClearOutput();

            ShowImage("froggirl-anal");

            OutputText("Scenes.Froggirlscene.FollowDatFrog.SplashYourWayUp".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.SkinIsSlimy".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.RoundBellyRubsAgainst".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Froggirlscene.FollowDatFrog.EachCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Froggirlscene.FollowDatFrog.YourVagina".Localized());
                else
                    OutputText("Scenes.Froggirlscene.FollowDatFrog.BareSensitiveGroin".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.RaisesFingertips".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.ArmsDrapeOverLip".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.SaysPattingPregnant".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.WordsRaiseRedFlags".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.CoosSlickHandsSplay".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.PullsAwayFromKiss".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.LayBackShuddering".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.AsksAsWorldDissolves".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.LostInSensationPant".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.CrossAndUncrossEyes".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.MoansBearingDown".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Froggirlscene.FollowDatFrog.GiggleDrunkenly".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.HissesThroughGritted".Localized());

            OutputText("Scenes.Froggirlscene.FollowDatFrog.GiggleAgainLoving".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GetFrogButtFilled);
        }

        public void GetFrogButtFilled()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.GetFrogButtFilled.WakeAnHourLater".Localized());

            OutputText("Scenes.Froggirlscene.GetFrogButtFilled.EggsOhHellThe".Localized());

            OutputText("Scenes.Froggirlscene.GetFrogButtFilled.WrappingArmsUnderBelly".Localized());

            OutputText("Scenes.Froggirlscene.GetFrogButtFilled.DressWithDifficulty".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            Player.StartButtPregnancy(PregnancyIds.FrogGirl, PregnancyIds.FrogGirlIncubation, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void ResistDatFrog()
        {
            ClearOutput();

            ShowImage("froggirl");

            OutputText("Scenes.Froggirlscene.ResistDatFrog.ShakeHeadAndDrop".Localized());

            OutputText("Scenes.Froggirlscene.ResistDatFrog.RealizingShakenOff".Localized());

            OutputText("Scenes.Froggirlscene.ResistDatFrog.WeighedDownByBelly".Localized());

            OutputText("Scenes.Froggirlscene.ResistDatFrog.StandAtLipOf".Localized());

            OutputText("Scenes.Froggirlscene.ResistDatFrog.SaysHoldingUpWide".Localized());

            OutputText("Scenes.Froggirlscene.ResistDatFrog.WrinklesNoselessFace".Localized());

            OutputText("Scenes.Froggirlscene.ResistDatFrog.AdmitABitCurious".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Question".Localized(), QuestDatFrogGirl);

            if (Save.GetInt("taughtLesson", 0) != 0)
            {
                if (Player.Weapon.IsWhip())
                    AddNextButton("Common.Button.Lash".Localized(), Lash);
                else
                    AddNextButton("Common.Button.Kill".Localized(), Kill);

                AddNextButton("Common.Button.Scare".Localized(), Scare);
            }
            else
                AddButton(1, "Common.Button.TeachLesson".Localized(), TeachDatFrogALesson);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveFrogBe);
        }

        public void LeaveFrogBe()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.LeaveFrogBe.ShrugAndLeavePouting".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void QuestDatFrogGirl()
        {
            ClearOutput();

            ShowImage("froggirl");

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.QuestionThePouting".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.BubblesShortlyHalfSunk".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.RaiseEyebrowRequestingMore".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.PullsHerselfHigher".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.GestureForMoreInfo".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.SaysIdlySplashing".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.ShiftsEyesSideways".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.BeforeReactSheAdds".Localized());

            OutputText("Scenes.Froggirlscene.QuestDatFrogGirl.WonderWhatWouldBe".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Vaginally".Localized(), SuperBonusFrogEggsInYerCooch);

            AddButton(1, "Common.Button.Carry".Localized(), CarryBeeGirlsEggsVoluntarilyYouButtSlut);

            AddButton(14, "Common.Button.Leave".Localized(), () => ContinueOnYourWay());
        }

        public void ContinueOnYourWay()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.ContinueOnYourWay.NodInSympathyPatting".Localized());

            OutputText("Scenes.Froggirlscene.ContinueOnYourWay.LeaveDownTheTerrace".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CarryBeeGirlsEggsVoluntarilyYouButtSlut()
        {
            ClearOutput();

            ShowImage("froggirl-anal");

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.FeelingSympathy".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.SaysLungingForward".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.SlimeOnArmsTingles".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.RoundBellyRubsAgainst".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.EachCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.YourVagina".Localized());
                else
                    OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.BareSensitiveGroin".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.RaisesFingertips".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.ArmsDrapeOverLip".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.SaysMassagingPregnant".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.SlickHandsSplay".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.PullsAwayFromKiss".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.LayBackShuddering".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.AsksAsWorldDissolves".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.LostInSensationPant".Localized());

            Player.ButtChange(20, true, true, false);

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.CrossAndUncross".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.MoansBearingDown".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.GiggleDrunkenly".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.HissesThroughGritted".Localized());

            OutputText("Scenes.Froggirlscene.CarryBeeGirlsEggsVoluntarilyYouButtSlut.GiggleAgainLoving".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), VoluntarilyGetEggedEpilogue);
        }

        public void VoluntarilyGetEggedEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.VoluntarilyGetEggedEpilogue.WakeAnHourLater".Localized());

            OutputText("Scenes.Froggirlscene.VoluntarilyGetEggedEpilogue.EggsOhHellThe".Localized());

            OutputText("Scenes.Froggirlscene.VoluntarilyGetEggedEpilogue.WrappingArmsUnderBelly".Localized());

            OutputText("Scenes.Froggirlscene.VoluntarilyGetEggedEpilogue.DonClothesWithDifficulty".Localized());

            Player.StartButtPregnancy(PregnancyIds.FrogGirl, PregnancyIds.FrogGirlIncubation, 1);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TeachDatFrogALesson()
        {
            ClearOutput();

            ShowImage("froggirl");

            if (Player.Intelligence < 20)
            {
                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.FrogBitchNeedsLesson".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.SaySoHopDown".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.HandTinglesBut".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.StaggerBackClutchingHead".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.GlowersAtYou".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.NoActualPainTo".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.OpenYourEyesAgain".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.ColorsLightsErupt".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), LessonFollowup);
            }
            else
            {
                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.ByNoMeansA".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.PleadsButIgnoreHer".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.GrabHerByWrist".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.BlinkingTheFrogAsks".Localized());

                OutputText("Scenes.Froggirlscene.TeachDatFrogALesson.CantRelyOnAssaulting".Localized());

                Save.SetInt("taughtLesson", 1);

                ClearButtons();

                if (Player.Weapon.IsWhip())
                    AddNextButton("Common.Button.Lash".Localized(), Lash);
                else
                    AddNextButton("Common.Button.Kill".Localized(), Kill);

                AddNextButton("Common.Button.Scare".Localized(), Scare);

                AddButton(14, "Common.Button.Leave".Localized(), () => LessonLeave());
            }
        }

        public void LessonFollowup()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.LessonFollowup.WakeTwoHoursLater".Localized());

            Player.ApplyStats(new DynStat(GameStats.Toughness, -1f));

            DoNext(() => ReturnToCamp(2));
        }

        public void Kill()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Kill.UnfortunateEffectsOf".Localized());

            if (Player.Weapon.IsRanged())
                OutputText("Scenes.Froggirlscene.Kill.TakeAimFindingIt".Localized());
            else
                OutputText("Scenes.Froggirlscene.Kill.CloseDistanceAndSwing".Localized());

            OutputText("Scenes.Froggirlscene.Kill.NothingButBloatedAmphibian".Localized());

            Save.SetInt("taughtLesson", 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void Lash()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Lash.SeeingYouCantEnjoy".Localized());

            OutputText("Scenes.Froggirlscene.Lash.PleaseSorryForTricking".Localized());

            OutputText("Scenes.Froggirlscene.Lash.LashAgainHearing".Localized());

            OutputText("Scenes.Froggirlscene.Lash.AfterAnotherScream".Localized());

            OutputText("Scenes.Froggirlscene.Lash.SeemsSufficient".Localized());

            Save.SetInt("taughtLesson", 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void Scare()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Scare.DeterminedToMake".Localized());

            Save.SetInt("taughtLesson", 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void LessonLeave()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.LessonLeave.LittleYouCanDo".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public bool UpdateAnalPregnancy()
        {

            if (Player.ButtPregnancy.Incubation == 8)
            {
                OutputText("Common.Text.PgYourGutChurnsAndWith".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.SlimeFeelsWarm".Localized());
                else
                    if (Player.Balls > 0)
                        OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.HallucinogenicSlime".Localized());
                    else
                        if (Player.HasCock())
                            OutputText(" Splashing against the underside of your " + Player.MultiCockDescriptLight() + "Common.Text.TheSlimeLeavesAWarmOozy".Localized());
                        else
                            OutputText("Common.Text.YourAssholeBeginsTwitchingAchingFor".Localized());

                OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.SeatedInOwnSlime".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

                return true;
            }

            return false;
        }

        public void AnalBirth()
        {

            ShowImage("birth-froggirl-anal");

            OutputText("Common.Text.PgBOhNoBPg".Localized());

            OutputText("Common.Text.NNwaterYouNeedToBe".Localized());

            OutputText("Common.Text.NNslimePoolsBeneathYouRunning".Localized());

            OutputText("Common.Text.NNtheFirstEggRollsDown".Localized());

            Player.ButtChange(80, true, true, false);

            OutputText("Common.Text.NNnaturePushesOnwardThoughAnd".Localized());

            OutputText("Common.Text.NNBLaterYouWake".Localized());

            OutputText("Common.Text.NNtheTadgirlsSplashEachOther".Localized());

            OutputText("Common.Text.NNyouNodToYourselfHappy".Localized());

            Player.Body.Hips += 1;

            Player.Anal.Wetness += 1;

            if (Player.Anal.Wetness > 5)
                Player.Anal.Wetness = 5;

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));
        }

        public void SuperBonusFrogEggsInYerCooch()
        {
            ClearOutput();

            ShowImage("froggirl-vag");

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.AsksFrogGirlIncredulously".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.RepeatThatYoudLike".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.MuttersFurrowingBrow".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.LeansInAgainstYou".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.SmallOfYourBack".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.HearFrogGirlCoo".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.ThickerSlimeAdheres".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.CouldThinkClearly".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.HookedOverTail".Localized());
            else
                OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.ClaspedAgainstAss".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.SmearingMoreAndMore".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.BrainTurnsHappy".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.NotSureWhenThe".Localized());

            OutputText("Scenes.Froggirlscene.SuperBonusFrogEggsInYerCooch.AwarenessTrickles".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            Player.KnockUp(PregnancyIds.FrogGirl, PregnancyIds.FrogGirlIncubation, 1, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public bool UpdateVaginalPregnancy()
        {

            var displayedUpdate = false;

            if (Player.Pregnancy.Incubation == 8)
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.GutChurnsSquelch".Localized());

                    OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.RealizeThat".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.SlimeFeelsWarm".Localized());
                    else
                        if (Player.Balls > 0)
                            OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.HallucinogenicSlime".Localized());
                        else
                            OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.BeginsTwitchingAching".Localized());

                    OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.SeatedInOwnSlime".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

                    displayedUpdate = true;
                }
                else
                {
                    OutputText("Scenes.Froggirlscene.UpdateVaginalPregnancy.GutChurnsButAfter".Localized());

                    displayedUpdate = true;
                }

            return displayedUpdate;
        }

        public void VaginalBirth()
        {

            ShowImage("birth-froggirl-vag");

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Froggirlscene.VaginalBirth.TerriblePressureIn".Localized());

                Player.CreateVagina();
            }
            else
                OutputText("Scenes.Froggirlscene.VaginalBirth.ShudderAsYouFeel".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SwollenBellyChurns".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.HustleToTheBanks".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.BeforePainBlossoms".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SlimeBeginsToNumb".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SaysSlurpingTongueBack".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.AbleToThankHer".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.MuttersLeaningAcross".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SwallowReflexively".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.FrogGirlWithSmile".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.LackingMuscleControl".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.ForestTurnsUpsideDown".Localized());

            Player.CuntChange(80, true, true, false);

            OutputText("Scenes.Froggirlscene.VaginalBirth.LoseCountOfSquishy".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.DontRememberPassing".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.QueriesTheVoice".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SheSays".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.NymphsInWaterAll".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SheSaysAgain".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.ChantInUnison".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SmileWeaklyWondering".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.FrogGirlComposed".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.ToYouSheWinks".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.SplashSheDivesBeneath".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.ShakeHeadWearily".Localized());

            OutputText("Scenes.Froggirlscene.VaginalBirth.WinceYouHeadBack".Localized());

            if (Player.Vaginas[0].Wetness < 5)
            {
                Player.Vaginas[0].Wetness += 1;

                OutputText("Scenes.Froggirlscene.VaginalBirth.RapidlyBecomesClear".Localized());
            }

            if (Player.Body.Hips < 25)
            {
                Player.Body.Hips += 1;

                OutputText("Scenes.Froggirlscene.VaginalBirth.ExtraThicknessTo".Localized());
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KnockUpSuwako()
        {

            if (Player.KnockUpChance(20, 100))
            {
                Player.KnockUp(PregnancyIds.PlayerFather, 96);

                Save.SetInt("analMad", Math.Max(Save.GetInt("analMad", 0) - 1, 0));
            }
        }

        public void Suwako()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Suwako.TrekThroughBleakBog".Localized());

            OutputText("Scenes.Froggirlscene.Suwako.SurroundingsGrowDarker".Localized());

            ClearButtons();

            AddNextButton("Combat.Button.Approach".Localized(), Approach);

            AddNextButton("Common.Button.ForgetIt".Localized(), Forget);
        }

        public void Forget()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Forget.ShakeHeadChoosingTo".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Approach()
        {
            ClearOutput();

            Save.SetBool("metSuwako", true);

            OutputText("Scenes.Froggirlscene.Approach.ApproachBeautifulWater".Localized());

            if (Player.Speed >= 60)
            {
                OutputText("Scenes.Froggirlscene.Approach.WhenYouDoSplash".Localized());

                OutputText("Scenes.Froggirlscene.Approach.SurpriseSlimyTendril".Localized());

                OutputText("Scenes.Froggirlscene.Approach.BasedOnCreatures".Localized());

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.FrogGirl), () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.Approach.LongAppendageGrab".Localized());

                if (Player.Strength >= 60)
                {
                    OutputText("Scenes.Froggirlscene.Approach.MusteringAllPower".Localized());

                    OutputText("Scenes.Froggirlscene.Approach.GlaresDaggers".Localized());

                    CombatScene.Start(EnemyRegistry.Create(EnemyIds.FrogGirl), () => Go<CampScene>());
                }
                else
                {
                    OutputText("Scenes.Froggirlscene.Approach.ColdFearGripsHeart".Localized());

                    OutputText("Scenes.Froggirlscene.Approach.SightOfBlackHair".Localized((Player.HasGills() ? "" : " The need for air proves too much for you to endure any longer. A torrent of water floods straight into your lungs as you desperately gasp for oxygen. Your vision quickly grows darker and darker until you can't feel anything at all...")));

                    if (Player.HasGills())
                        DoNext(() => GillEscape());
                    else
                        DoNext(() => AutoRape());
                }
            }
        }

        public void GillEscape()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.GillEscape.AbleToBreatheIn".Localized());

            OutputText("Scenes.Froggirlscene.GillEscape.SeeHerGlaring".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.FrogGirl), () => Go<CampScene>());
        }

        public void AutoRape()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Froggirlscene.AutoRape.AwakenBodyLeaning".Localized());

                OutputText("Scenes.Froggirlscene.AutoRape.MustHaveNoticedPoking".Localized());

                if (Player.BiggestCockLength() >= 10)
                    OutputText("Scenes.Froggirlscene.AutoRape.BuckHipsInstinctively".Localized());
                else
                    OutputText("Scenes.Froggirlscene.AutoRape.ShiversInDelight".Localized());

                OutputText("Scenes.Froggirlscene.AutoRape.HeatedSighEscapes".Localized());

                OutputText("Scenes.Froggirlscene.AutoRape.MustHaveRealized".Localized());

                OutputText("Scenes.Froggirlscene.AutoRape.PullsYourSpentDick".Localized());

                OutputText("Scenes.Froggirlscene.AutoRape.HerGoneBackOn".Localized());

                KnockUpSuwako();

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.Froggirlscene.AutoRape.AwakenBodySafely".Localized());

                OutputText("Scenes.Froggirlscene.AutoRape.TemptingReturnCamp".Localized());

                Player.HealthChange(-150);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoRepeat()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoRepeat.WanderThroughBog".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoRepeat.ObviousFromHow".Localized(
                new[] { "is unhappy to see you. Perhaps she is annoyed how you bested her last time", "[if (hascock) {believes she'll easily have her way with you this time too|isn't glad to see you. If anything, she appears a little disgusted}]", "looks oddly pleased to see you", "isn't very happy to meet you. In fact, you'd say she looks rather furious" }[((int)LastEncounter)]
            ));

            OutputText("Scenes.Froggirlscene.SuwakoRepeat.SupposeNoChoice".Localized());

            LastEncounter = 0;

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.FrogGirl), () => Go<CampScene>());
        }

        public void SuwakoWon(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoWon.CollapsingToThe".Localized((hpVictory ? "Common.Text.PainCoursingThroughYourBody".Localized() : "Common.Text.HeatInYourLoins".Localized())));

            if (Player.HasCock())
            {
                OutputText("Scenes.Froggirlscene.SuwakoWon.GoldenEyesSparkle".Localized());

                if (Player.BiggestCockLength() >= 10)
                    OutputText("Scenes.Froggirlscene.SuwakoWon.QuicklyArrivesAtFrog".Localized());
                else
                    OutputText("Scenes.Froggirlscene.SuwakoWon.SwallowedUpBy".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoWon.GrabsYourHands".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoWon.ContinuouslyRocksHips".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoWon.TakeMomentToRecover".Localized());

                Player.Orgasm("Dick");

                KnockUpSuwako();
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoWon.SmirkingFaceContorts".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoWon.AwakenSometimeLater".Localized());

                Player.HealthChange(-150);
            }

            LastEncounter = FrogGirlEncounter.Raped;

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoDefeated(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoDefeated.FrogGirlCollapses".Localized((hpVictory ? "Common.Text.HerWoundsPreventingHerFromResisting".Localized() : "Common.Text.WithAShamefulBlushOnHer".Localized())));

            ClearButtons();

            AddNextButton("Common.Button.Vaginal".Localized(), SuwakoVaginal);

            AddNextButton("Common.Button.Anal".Localized(), SuwakoAnal);

            AddNextButton("Common.Button.Tongue".Localized(), SuwakoTongue);

            AddButton(14, "Common.Button.Leave".Localized(), () => SuwakoSexLeave());
        }

        public void SuwakoSexLeave()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoSexLeave.BackAwayFromFrog".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoVaginal()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoVaginal.ApproachTheFallenFrog".Localized((CurrentMonster!.Health < 1 ? "Common.Text.StaresAtYouWearily".Localized() : "Common.Text.EyesYourDickHungrily".Localized())));

            OutputText("Scenes.Froggirlscene.SuwakoVaginal.MoansRatherCutely".Localized());

            if (Player.BiggestCockLength() >= 10)
                OutputText("Scenes.Froggirlscene.SuwakoVaginal.SquishyCervixMeets".Localized());
            else
                OutputText("Scenes.Froggirlscene.SuwakoVaginal.PussyEnvelopsLength".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoVaginal.RaisesHipsToMeet".Localized((Player.HasKnot(Player.BiggestCockIndex()) && Player.BiggestCockLength() < 10 ? "You batter your [knotword] against her labia, easily slipping past and letting it swell up to lock yourself to her for ensured insemination. " : "")));

            OutputText("Scenes.Froggirlscene.SuwakoVaginal.PullSpentDickFree".Localized());

            Player.Orgasm("Dick");

            KnockUpSuwako();

            LastEncounter = FrogGirlEncounter.Vag;

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoAnal()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAnal.TossingThemAside".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? " with desire" : "")));

            ClearButtons();

            AddNextButton("Scenes.Aiko.AikoRapeSex.ButtonFuckVag".Localized(), AssToVag);

            AddNextButton("Common.Button.Anal".Localized(), SuwakoAnal2);
        }

        public void AssToVag()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.AssToVag.MomentYouTouchWomanhood".Localized());

            if (Player.BiggestCockLength() >= 10)
                OutputText("Scenes.Froggirlscene.AssToVag.LetsOutA".Localized());
            else
                OutputText("Scenes.Froggirlscene.AssToVag.GaspsInDelight".Localized());

            OutputText("Scenes.Froggirlscene.AssToVag.InsidesClenchDown".Localized());

            OutputText("Scenes.Froggirlscene.AssToVag.GroanYouFeelThe".Localized());

            OutputText("Scenes.Froggirlscene.AssToVag.HerGoneHesitates".Localized());

            Player.Orgasm("Dick");

            KnockUpSuwako();

            LastEncounter = FrogGirlEncounter.Vag;

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoAnal2()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAnal2.SpreadAmphibiousGirls".Localized((analMad > 1 ? " again" : "")));

            if (Player.BiggestCockLength() >= 10)
                OutputText("Scenes.Froggirlscene.SuwakoAnal2.LetsOutA".Localized());
            else
                OutputText("Scenes.Froggirlscene.SuwakoAnal2.LetsOutAAgain".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAnal2.HasLongSince".Localized((Player.HasKnot(Player.BiggestCockIndex()) && Player.BiggestCockLength() < 10 ? " You batter your [knotword] against her asshole, slipping past and letting it swell up to lock yourself in her." : "")));

            OutputText("Scenes.Froggirlscene.SuwakoAnal2.FinallyDoneEmptying".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAnal2.AfterSheDivesBack".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Save.SetInt("analMad", Save.GetInt("analMad", 0) + 1);

            LastEncounter = FrogGirlEncounter.NonVag;

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoTongue()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Froggirlscene.SuwakoTongue.ApproachTheFallenFrog".Localized((CurrentMonster!.Health < 1 ? "Common.Text.LooksLessWorriedAtTheSight".Localized() : "Common.Text.WasTooLostInAHaze".Localized())));

                OutputText("Scenes.Froggirlscene.SuwakoTongue.TightensHerTongue".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoTongue.LoudlyPoppingFree".Localized());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoTongue.AsYouApproachFallen".Localized((CurrentMonster!.Health < 1 ? "Common.Text.Energy".Localized() : "Common.Text.Will".Localized())));

                OutputText("Scenes.Froggirlscene.SuwakoTongue.SensationOfSlimyOrgan".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoTongue.MoanInConstantPleasure".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoTongue.TongueGrindsAgainst".Localized());

                Save.SetInt("analMad", Save.GetInt("analMad", 0) + 1);
            }

            Player.Orgasm("Generic");

            LastEncounter = FrogGirlEncounter.NonVag;

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoAnalWon()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAnalWon.FallToMuddy".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAnalWon.CantHelpButWince".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAnalWon.TongueProdsProstate".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAnalWon.QuicklyMovesA".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAnalWon.BringsAttractiveFace".Localized());

            Player.Orgasm("Dick");

            KnockUpSuwako();

            LastEncounter = FrogGirlEncounter.Raped;

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoAngry()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAngry.ExploringTheBog".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAngry.DontLookAllThat".Localized());

            CombatScene.StartMultiple(
                new[]
                {
                    EnemyRegistry.Create(EnemyIds.FrogGirl),

                    EnemyRegistry.Create(EnemyIds.FrogGirlPink),

                    EnemyRegistry.Create(EnemyIds.FrogGirlRed)
                },

                SuwakoAngryDefeated,

                false);
        }

        public void SuwakoAngryWon()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                if (Save.GetInt("angryLosses", 0) >= 4)
                {
                    SuwakoBadEnd();

                    return ;
                }

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.TrioFromYourBody".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.ThickTonguePunches".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.RedGirlAllFours".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.DonePouringAll".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.ThenFlippedOntoYour".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.ZealouslySlidesInside".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.InsidesTightlySqueeze".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.TrioTakeLastLook".Localized());

                KnockUpSuwako();
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.FallToGroundAs".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.BegrudginglyUseYour".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.FrogGirlOnFace".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.BodySeizesUp".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.LyingInThe".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoAngryWon.AwakenWithTheThree".Localized());
            }

            Player.Orgasm("Generic");

            Save.SetInt("angryLosses", Save.GetInt("angryLosses", 0) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.ThreeGirlsObviously".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.RibbitSoftly".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.CarelesslyTossedAside".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.SeemsToHaveSeries".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.CountlessFrogGirls".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.ManhoodGrabbed".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd.LittleGreenGirlComes".Localized());

            DoNext(() => SuwakoBadEnd2());
        }

        public void SuwakoBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd2.PleasureCoursingThrough".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd2.ShakilyRemovesManhood".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd2.NervouslyApproach".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd2.PullingAwayFromGirl".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd2.AnotherLoadYanked".Localized());

            DoNext(() => SuwakoBadEnd3());
        }

        public void SuwakoBadEnd3()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.HowLongBeenHere".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.ThatsTheThought".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.InsidesClench".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.FeelTheGirlWho".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.NowAloneWithYou".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.TenseUpFlooding".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.NoDoubtTheyIntend".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoBadEnd3.RestOfYourDays".Localized());

            GameOver();
        }

        public void SuwakoAngryDefeated()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAngryDefeated.ThreeGirlsCollapse".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? "Common.Text.EachFuriouslyRubbingTheirSlits".Localized() : "Common.Text.ClearlyInTooMuchPainTo".Localized())));

            ClearButtons();

            AddNextButton("Common.Button.Anal".Localized(), AbsolutelyFuriousAnal);

            AddNextButton("Common.Button.Vaginal".Localized(), SuwakoVaginal);

            AddNextButton("Common.Button.Service".Localized(), SuwakoService);

            AddButton(14, "Common.Button.Leave".Localized(), () => SuwakoAngryLeave());
        }

        public void SuwakoAngryLeave()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAngryLeave.SimplyLeaveTheGirl".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? "Common.Text.DesperatelyTryingToRelieveTheHeat".Localized() : "Common.Text.LyingFaceDownInTheMud".Localized())));

            CombatScene.CleanupAfterCombat();
        }

        public void SuwakoService()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Froggirlscene.SuwakoService.NoticeHowTheThree".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? "hungrily " : "")));

                OutputText("Scenes.Froggirlscene.SuwakoService.GreenGirlCoils".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoService.InstantOtherTwo".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoService.PinkAndRedLeave".Localized());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoService.NoteHowTheThree".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoService.RedGirlSeemsTo".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoService.ConstantlyShake".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoService.ThroughBlurryVision".Localized());
            }

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void AbsolutelyFuriousAnal()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.AbsolutelyFuriousAnal.AsideBeforeApproach".Localized());

            OutputText("Scenes.Froggirlscene.AbsolutelyFuriousAnal.WhyNotStartGreen".Localized());

            OutputText("Scenes.Froggirlscene.AbsolutelyFuriousAnal.PinkGirlCrawls".Localized());

            OutputText("Scenes.Froggirlscene.AbsolutelyFuriousAnal.CockFeelsAsIf".Localized());

            OutputText("Scenes.Froggirlscene.AbsolutelyFuriousAnal.SpermDesperately".Localized());

            OutputText("Scenes.Froggirlscene.AbsolutelyFuriousAnal.PinkAndGreenGirls".Localized());

            Player.Orgasm("Dick");

            Save.SetInt("analMad", Save.GetInt("analMad", 0) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void SubmissiveSuwako()
        {
            ClearOutput();

            RegisterMacro("frogpreg", Player.IsPregnant());

            if (!Save.GetBool("submissive"))
            {
                OutputText("Scenes.Froggirlscene.SubmissiveSuwako.OnceAgainVenturing".Localized());

                OutputText("Scenes.Froggirlscene.SubmissiveSuwako.CaughtOffGuard".Localized());

                OutputText("Scenes.Froggirlscene.SubmissiveSuwako.BlatantAttemptToEntice".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Talk".Localized(), SuwakoTalk);

                AddNextButton("Common.Button.FuckHer".Localized(), SuwakoYesSub);

                AddNextButton("Common.Button.Attack".Localized(), SuwakAttack);

                AddNextButton("Common.Button.Leave".Localized(), SuwakoNoSub);
            }
            else
                if (!Player.HasCock())
                {
                    OutputText("Scenes.Froggirlscene.SubmissiveSuwako.ArriveBackAtThe".Localized());

                    OutputText("Scenes.Froggirlscene.SubmissiveSuwako.JumpsBackIntoWater".Localized());

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (Save.GetInt("shownKids", 0) == 0 && Save.GetInt("eggCount", 0) >= 35)
                    {
                        OutputText("Scenes.Froggirlscene.SubmissiveSuwako.TraversingDreadfulBog".Localized());

                        OutputText("Scenes.Froggirlscene.SubmissiveSuwako.AmphibiousGirlSmiles".Localized());

                        OutputText("Scenes.Froggirlscene.SubmissiveSuwako.SeeingLittleGirlWont".Localized());

                        OutputText("Scenes.Froggirlscene.SubmissiveSuwako.LoverSweetlyRibbits".Localized());

                        OutputText("Scenes.Froggirlscene.SubmissiveSuwako.MotherAndChildNow".Localized());

                        Save.SetInt("shownKids", 1);

                        DoNext(() => ReturnToCamp(1));
                    }
                    else
                        if (Save.GetInt("eggCount", 0) >= 50 && Random.Shared.Next(100) < (10))
                            Foursome();
                        else
                        {
                            if (Save.GetInt("shownKids", 0) != 0)
                            {
                                OutputText("Scenes.Froggirlscene.SubmissiveSuwako.ArriveBackAtLarge".Localized());

                                OutputText("Scenes.Froggirlscene.SubmissiveSuwako.TakesYourHand".Localized());
                            }
                            else
                                OutputText("Scenes.Froggirlscene.SubmissiveSuwako.CarefullyTraverse".Localized((LastEncounter == FrogGirlEncounter.NonVag ? "Common.Text.Pout".Localized() : "Common.Text.RatherFondSmile".Localized()), (Save.GetInt("analMad", 0) >= 3 ? "Common.Text.AndMeeklyPresentsHerPlumpBackside".Localized() : "Common.Text.LyingOnHerBackBeforeSpreading".Localized())));

                            ClearButtons();

                            AddButton(0, "Common.Button.Appearance".Localized(), SuwakoAppearance);

                            if (Save.GetInt("shownKids", 0) != 0)
                            {
                                AddButton(1, "Common.Button.Pat".Localized(), SuwakoPat);

                                AddButton(5, "Common.Button.Children".Localized(), SuwakoChildren);

                                AddButton(6, "Common.Button.Pregnancy".Localized(), SuwakoPregnancy);
                            }

                            AddNextButton("Common.Button.Sex".Localized(), SuwakoSubSex);

                            AddButton(14, "Common.Button.Leave".Localized(), () => SuwakoSubLeave());
                        }
        }

        public void SuwakoNoSub()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoNoSub.TurningToLeaveHear".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoNoSub.SupposeSheIsDisappointed".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoTalk()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoTalk.ClearingThroatMakeAn".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoTalk.ClearTheTwoOf".Localized());

            AddButtonDisabled(0, "Talk", "");

        }

        public void SuwakoYesSub()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoYesSub.BehindTheFrogGirl".Localized());

            if (Player.BiggestCockLength() >= 10)
                OutputText("Scenes.Froggirlscene.SuwakoYesSub.PushDeeperIntoThe".Localized());
            else
                OutputText("Scenes.Froggirlscene.SuwakoYesSub.WallsWrapAround".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoYesSub.WarmPleasureRunning".Localized((Player.HasKnot() && Player.BiggestCockLength() < 10 ? " Your knot batters against the frog-girl's entrance a few times, making you believe it won't fit. But you persist, causing her to meekly whimper when it audibly pops past her folds to lock you inside." : "")));

            OutputText("Scenes.Froggirlscene.SuwakoYesSub.PullSpentCockFrom".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoYesSub.NowAloneStareOut".Localized());

            Player.Orgasm("Dick");

            KnockUpSuwako();

            Save.SetBool("submissive", true);

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakAttack()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakAttack.HerRaisedRear".Localized());

            OutputText("Scenes.Froggirlscene.SuwakAttack.TwoGoldenEyes".Localized());

            Save.SetBool("fought", true);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.FrogGirl), () => Go<CampScene>());
        }

        public void SuwakoAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoAppearance.StandsAtAround".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAppearance.CompletelyNaked".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoAppearance.BetweenTonedLegs".Localized());

            DoNext(() => SubmissiveSuwako());
        }

        public void SuwakoPat()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoPat.GentlyPatHerHead".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoPat.FinallyPullHand".Localized());

            DoNext(() => SubmissiveSuwako());
        }

        public void SuwakoSubSex()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoSubSex.StareAtTheGirl".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Vaginal".Localized(), SuwakoSubFuck);

            AddNextButton("Common.Button.Anal".Localized(), SuwakoSubAnal);

            AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), SuwakoSubOral);

            AddButton(14, "Common.Button.Back".Localized(), () => SubmissiveSuwako());
        }

        public void SuwakoSubOral()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoSubOral.GetComfortableFinding".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoSubOral.ForASecondThink".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Froggirlscene.SuwakoSubOral.SecondMemberNotLeft".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Froggirlscene.SuwakoSubOral.UsesHerOther".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoSubOral.CantStopYourEntire".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoSubOral.LoudlyPopsMember".Localized());

            Player.Orgasm("Dick");

            KnockUpSuwako();

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoSubFuck()
        {
            ClearOutput();

            if (Save.GetInt("shownKids", 0) != 0)
            {
                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.CasuallyPuttingHand".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.LiesDownOn".Localized());

                if (Player.BiggestCockLength() >= 10)
                    OutputText("Scenes.Froggirlscene.SuwakoSubFuck.MoansInDelight".Localized());
                else
                    OutputText("Scenes.Froggirlscene.SuwakoSubFuck.LetsOutA".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.CantHelpGroaning".Localized((Player.HasKnot(Player.BiggestCockIndex()) && Player.BiggestCockLength() < 10 ? " Instincts compel you to tie with your partner, your knot loudly popping inside the amphibious girl. She meekly whimpers in pain at the feeling of you getting stuck inside her as her pussy is stretched to its limits." : "")));

                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.VirileSeedRushes".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.CompletelySpentPull".Localized());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.TakeABriefMoment".Localized((analMad >= 3 ? "Common.Text.PresentingHerJuicyBacksideTo".Localized() : "Common.Text.SpreadingHerLegsFor".Localized()), (analMad >= 3 ? "While the fact she is already anticipating you fucking her ass like you usually do tempts you to meet her expectations, you feel like using the proper hole today. When you get the girl to lie on her back, she offers you an obviously confused sounding ribbit." : "")));

                if (Player.BiggestCockLength() >= 10)
                    OutputText("Scenes.Froggirlscene.SuwakoSubFuck.PushingApartInnerFolds".Localized());
                else
                    OutputText("Scenes.Froggirlscene.SuwakoSubFuck.PushingInsideTheGirl".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.PlumpTitsJiggle".Localized((Player.HasKnot(Player.BiggestCockIndex()) && Player.BiggestCockLength() < 10 ? "Your knot batters up against the frog-girl's wet pussy lips, making her whimper when it finally pops into her. " : "")));

                OutputText("Scenes.Froggirlscene.SuwakoSubFuck.FinallySpentYou".Localized());
            }

            Player.Orgasm("Dick");

            KnockUpSuwako();

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoSubAnal()
        {
            ClearOutput();

            if (Save.GetInt("shownKids", 0) != 0)
            {
                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.StareAtTheFrog".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.HaveTheGirlGet".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.PushingPastReluctant".Localized((analMad >= 3 ? "Common.Text.DespiteHerAnusBeingUsedTo".Localized() : "Common.Text.HerAnusViolentlyContractsAroundYour".Localized())));

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.PressYourDickInto".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.OnlyAMomentMore".Localized());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.EnjoyHowTheFrog".Localized((analMad >= 3 ? "Common.Text.PresentingHerLusciousBacksideTo".Localized() : "Common.Text.SpreadingHerLovelyLegsOpenFor".Localized()), (analMad == 0 ? "move her onto all fours and position yourself directly behind her. She ribbits excitedly, wiggling her rounded ass in ignorance of your intentions to soon conquer her [b:other] hole" : (analMad >= 3 ? "Common.Text.PositionYourselfBehindHerSheRibbits".Localized() : "Common.Text.HaveTheGirlGetOnAll".Localized()))));

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.HeartLoudlyPounding".Localized((analMad == 0 ? "Common.Text.SurprisedCroakLookingOverHerShoulders".Localized() : "Common.Text.MeekCroakLoweringHerHeadFurther".Localized())));

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.RectumSwallowing".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal.VigorouslyThrusting".Localized());
            }

            ClearButtons();

            AddNextButton("Common.Button.InHerAss".Localized(), () => SuwakoSubAnal2(true));

            AddNextButton("Common.Button.InHerPussy".Localized(), () => SuwakoSubAnal2(false));
        }

        public void SuwakoSubAnal2(bool ass)
        {
            ClearOutput();

            if (Save.GetInt("shownKids", 0) != 0)
            {
                if (ass)
                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.BestialGrowlSlam".Localized());
                else
                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.ShudderingPullFromHer".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.FinallyFinishEnergetic".Localized());
            }
            else
                if (ass)
                {
                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.RoughlyThrustDeep".Localized());

                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.CanOnlyTremble".Localized());
                }
                else
                {
                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.GiveWhatSheWants".Localized());

                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.TremblesInThe".Localized());

                    OutputText("Scenes.Froggirlscene.SuwakoSubAnal2.HearHerLettingOut".Localized());
                }

            Player.Orgasm("Dick");

            Save.SetInt("analMad", Save.GetInt("analMad", 0) + 1);

            LastEncounter = FrogGirlEncounter.NonVag;

            ClearButtons();

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), SuwakoSubAnalCuddle);

            AddNextButton("Common.Button.Leave".Localized(), SuwakoSubAnalLeave);
        }

        public void SuwakoSubAnalCuddle()
        {
            ClearOutput();

            if (Save.GetInt("shownKids", 0) != 0)
            {
                OutputText("Scenes.Froggirlscene.SuwakoSubAnalCuddle.LyingNextToShivering".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnalCuddle.BeforeYouGetUp".Localized());
            }
            else
            {
                OutputText("Scenes.Froggirlscene.SuwakoSubAnalCuddle.RatherThanLeave".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoSubAnalCuddle.TellByTheFaint".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoSubAnalLeave()
        {
            ClearOutput();

            if (Save.GetInt("shownKids", 0) != 0)
                OutputText("Scenes.Froggirlscene.SuwakoSubAnalLeave.SimplyGetUpSatisfied".Localized());
            else
                OutputText("Scenes.Froggirlscene.SuwakoSubAnalLeave.SatisfiedNowThat".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoPregnancy()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoPregnancy.DirectHeavilyPregnant".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoPregnancy.SlowlyMoveHand".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoPregnancy.LookBackIntoHer".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoPregnancy.CutelyRibbitsSomething".Localized());

            DoNext(() => SubmissiveSuwako());
        }

        public void SuwakoChildren()
        {
            ClearOutput();

            if (Save.GetInt("shownKids", 0) < 2)
            {
                OutputText("Scenes.Froggirlscene.SuwakoChildren.LookToTheGirl".Localized());

                OutputText("Scenes.Froggirlscene.SuwakoChildren.SeeAHandfulOf".Localized());

                Save.SetInt("shownKids", 2);
            }
            else
                OutputText("Scenes.Froggirlscene.SuwakoChildren.GentlyCaressing".Localized());

            OutputText("Scenes.Froggirlscene.SuwakoChildren.CurrentlyHaveChildren".Localized(
                 NumberUtils.Num2Text(Save.GetInt("eggCount", 0))
            ));

            ClearButtons();

            AddNextButton("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.ButtonPlay".Localized(), KidsPlay);

            AddNextButton("Common.Button.Lewd".Localized(), KidsLewd);

            AddButton(14, "Common.Button.Leave".Localized(), () => KidsLeave());
        }

        public void KidsPlay()
        {
            ClearOutput();

            switch (Random.Shared.Next(4))
            {
                case 0:
                {
                OutputText("Scenes.Froggirlscene.KidsPlay.ObserveLittleGirls".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.SpendSomeTimeWith".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.TheirLittleCastles".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Froggirlscene.KidsPlay.WatchYourAdorable".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.LeisurelyLittleGirls".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.SwimmingWithChildren".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.EmbraceYourDaughters".Localized());
                }

                break;

                case 2:
                {
                if (Save.GetBool("pattyCake"))
                {
                    OutputText("Scenes.Froggirlscene.KidsPlay.OneDaughterApproaches".Localized());

                    OutputText("Scenes.Froggirlscene.KidsPlay.VastSmileOn".Localized());

                    OutputText("Scenes.Froggirlscene.KidsPlay.SoonNeedToGo".Localized());

                    OutputText("Scenes.Froggirlscene.KidsPlay.SlowlyPatHerHead".Localized());
                }
                else
                {
                    OutputText("Scenes.Froggirlscene.KidsPlay.ApproachOneDaughter".Localized());

                    OutputText("Scenes.Froggirlscene.KidsPlay.SoonOtherGirls".Localized());

                    OutputText("Scenes.Froggirlscene.KidsPlay.DaughterFirstTeaching".Localized());

                    Save.SetBool("pattyCake", true);
                }
                }

                break;

                case 3:
                {
                OutputText("Scenes.Froggirlscene.KidsPlay.TakingASeat".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.StillPattingHerHead".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.PerhapsSmallAmount".Localized());

                OutputText("Scenes.Froggirlscene.KidsPlay.PlaySomeMore".Localized());
                }

                break;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void KidsLewd()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.KidsLewd.WatchingNakedGirl".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fuck".Localized(), KidsFuck);

            AddNextButton("Common.Button.Tadpole".Localized(), Tadpole);

            AddNextButton("Common.Button.Cunnilingus".Localized(), Cunny);

            AddButton(14, "Common.Button.Back".Localized(), () => SuwakoChildren());
        }

        public void KidsFuck()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.KidsFuck.AsideBeforeWading".Localized());

            OutputText("Scenes.Froggirlscene.KidsFuck.TightlyShutSlit".Localized());

            OutputText("Scenes.Froggirlscene.KidsFuck.GrabDaughtersSupple".Localized());

            OutputText("Scenes.Froggirlscene.KidsFuck.LittleGirlBounces".Localized());

            OutputText("Scenes.Froggirlscene.KidsFuck.IfNotForCunny".Localized());

            OutputText("Scenes.Froggirlscene.KidsFuck.PullFlaccidDick".Localized());

            OutputText("Scenes.Froggirlscene.KidsFuck.WhenGirlIsCalm".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddNextButton("Common.Button.Orgy".Localized(), KidsOrgy);

            AddNextButton("Common.Button.Flee".Localized(), KidsFlee);
        }

        public void KidsOrgy()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.KidsOrgy.SeeingDaughtersAre".Localized());

            OutputText("Scenes.Froggirlscene.KidsOrgy.MeeklyCriesOut".Localized());

            OutputText("Scenes.Froggirlscene.KidsOrgy.PullDaughterOffCock".Localized());

            OutputText("Scenes.Froggirlscene.KidsOrgy.OtherTwoHaving".Localized());

            OutputText("Scenes.Froggirlscene.KidsOrgy.LookSeeYouStill".Localized());

            Player.Orgasm("Dick");

            DoNext(() => KidsOrgy2());
        }

        public void KidsOrgy2()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.KidsOrgy2.AwakenOnMuddyShore".Localized());

            OutputText("Scenes.Froggirlscene.KidsOrgy2.CautiousNotToWake".Localized());

            View.Time.PassTime(4);

            Player.ChangeFatigue(20);

            DoNext(() => ReturnToCamp(1));
        }

        public void KidsFlee()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.KidsFlee.InImminentDanger".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Tadpole()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Tadpole.AsideBeforeWadeInto".Localized());

            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), TadpOral);

            AddNextButton("Common.Button.Finger".Localized(), TadpoleFinger);
        }

        public void TadpOral()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.TadpOral.HeartRaceInChest".Localized());

            OutputText("Scenes.Froggirlscene.TadpOral.SoftMoanEscapes".Localized());

            OutputText("Scenes.Froggirlscene.TadpOral.LittleGirlPushes".Localized());

            OutputText("Scenes.Froggirlscene.TadpOral.OddlySatisfiedReturn".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void TadpoleFinger()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.TadpoleFinger.ContinueCaressingTadpole".Localized());

            OutputText("Scenes.Froggirlscene.TadpoleFinger.DisappointingGingerlyProd".Localized());

            OutputText("Scenes.Froggirlscene.TadpoleFinger.ContinuesToTwistAbout".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Cunny()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Cunny.ApproachOneAdorable".Localized());

            OutputText("Scenes.Froggirlscene.Cunny.MomentYouMoveHead".Localized());

            OutputText("Scenes.Froggirlscene.Cunny.KeepLappingUp".Localized());

            OutputText("Scenes.Froggirlscene.Cunny.HearSomeoneMutter".Localized());

            OutputText("Scenes.Froggirlscene.Cunny.AnotherCuteGirl".Localized());

            OutputText("Scenes.Froggirlscene.Cunny.VisionBeginsToBlur".Localized());

            DoNext(() => Cunny2());
        }

        public void Cunny2()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Cunny2.AwakenFewHoursLater".Localized());

            OutputText("Scenes.Froggirlscene.Cunny2.CantUnderstandThem".Localized());

            OutputText("Scenes.Froggirlscene.Cunny2.TakeMomentToRub".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void KidsLeave()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.KidsLeave.SmallBitOf".Localized());

            OutputText("Scenes.Froggirlscene.KidsLeave.TurnToLeaveYou".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SuwakoSubLeave()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.SuwakoSubLeave.SeeGirlSadly".Localized((shownKids != 0 ? " stand up and" : "")));

            DoNext(() => ReturnToCamp(1));
        }

        public void Foursome()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.Foursome.AgainTraversingHumid".Localized());

            OutputText("Scenes.Froggirlscene.Foursome.DoesntTakeLong".Localized());

            OutputText("Scenes.Froggirlscene.Foursome.UncertaintyEncroaches".Localized());

            OutputText("Scenes.Froggirlscene.Foursome.PinkAndRedAmphibious".Localized());

            OutputText("Scenes.Froggirlscene.Foursome.FromTheWayThey".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fuck".Localized(), FoursomeFuck);

            AddNextButton("Common.Button.Nah".Localized(), FoursomeNo);
        }

        public void FoursomeFuck()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.FoursomeFuck.ApproachTheSubmissive".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.LoudRibbitsOfDelight".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.FillYourCurrentLover".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.SlamYourRight".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.FinallySpentYou".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.GrabbingAmpleHips".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.PlacesYourHand".Localized());

            OutputText("Scenes.Froggirlscene.FoursomeFuck.SenseOfPride".Localized());

            Player.Orgasm("Dick");

            KnockUpSuwako();

            DoNext(() => ReturnToCamp(1));
        }

        public void FoursomeNo()
        {
            ClearOutput();

            OutputText("Scenes.Froggirlscene.FoursomeNo.NotInterestedIn".Localized());

            DoNext(() => ReturnToCamp(1));
        }

    }
}