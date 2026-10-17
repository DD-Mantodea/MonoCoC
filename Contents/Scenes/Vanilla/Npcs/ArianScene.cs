using MonoCoC.Contents.World;
using MonoCoC.Registry;
using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<ArianScene>("coc:arian")]
    public class ArianScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Arian);

        public override void Entry()
        {
            ArianCockSize();
        }

        public bool TimeChange()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggCounter) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.ArianEggCounter, CoC.Instance.Flags.Get(GameFlagIds.ArianEggCounter) + 1);

            if (View.Time.Hours > 23)
            {
                if (ArianFollower() && CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.ArianEggEvent, CoC.Instance.Flags.Get(GameFlagIds.ArianEggEvent) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.ArianLessons, 0);

                CoC.Instance.Flags.Set(GameFlagIds.ArianTreatment, 0);
            }

            return false;
        }

        public bool TimeChangeLarge() => false;

        public float ArianCockSize()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) <= 0 || CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 3)
                return 0;
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 1)
                    return 9;
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 2)
                        return 16;
                    else
                        return 36;
        }

        public bool ArianFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ArianFollower) > 0;
        }

        public bool ArianMale()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) <= 0)
                return false;

            var breastCheck = (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0) ? 0 : 1;

            return CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) <= breastCheck;
        }

        public string ArianMF(string boy, string girl)
        {

            return ArianMale() ? boy : girl;
        }

        public int ArianHealth(int arg = 0)
        {

            if (arg != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ArianHealth, CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) + arg);

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.ArianHealth, 100);
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 0)
                        CoC.Instance.Flags.Set(GameFlagIds.ArianHealth, 0);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.ArianHealth);
        }

        public string ArianChestAdjective()
        {

            var buffer = "";

            var temp = Random.Shared.Next(10);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 0)
                return "";
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1)
                {
                    if (temp <= 4)
                        buffer += "small";
                    else
                        if (temp <= 6)
                            buffer += "petite";
                        else
                            if (temp <= 8)
                                buffer += "perky";
                            else
                                buffer += "palm-filling";
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 2)
                    {
                        if (temp <= 3)
                            buffer += "generous";
                        else
                            if (temp <= 5)
                                buffer += "hand-filling";
                            else
                                if (temp <= 7)
                                    buffer += "bouncy";
                                else
                                    buffer += "shapely";
                    }
                    else
                        if (temp <= 3)
                            buffer += "large";
                        else
                            if (temp <= 5)
                                buffer += "voluptuous";
                            else
                                if (temp <= 6)
                                    buffer += "jiggly";
                                else
                                    if (temp <= 7)
                                        buffer += "bra-bursting";
                                    else
                                        if (temp <= 8)
                                            buffer += "bountiful";
                                        else
                                            buffer += "huge";

            return buffer;
        }

        public string ArianChest()
        {

            var buffer = "";

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 0)
                return "chest";

            if (Random.Shared.Next(2) == 0)
                buffer += ArianChestAdjective() + " ";

            var temp = Random.Shared.Next(10);

            if (temp <= 2)
                buffer += "tits";
            else
                if (temp <= 5)
                    buffer += "breasts";
                else
                    if (temp <= 7)
                        buffer += "pillows";
                    else
                        buffer += "boobs";

            return buffer;
        }

        public void ArianSprite()
        {

        }

        public void MeetArian()
        {
            ClearOutput();

            OutputText("Scenes.Arianscene.MeetArian.PassingDarkAlley".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), HelpArianWhenYouMeetHim);

            AddButton(1, "Common.Button.DontHelp".Localized(), () => DontHelpArianWhenYouMeetHim());

            AddButton(2, "Common.Button.NeverHelp".Localized(), () => DontHelpArianWhenYouMeetHim(true));
        }

        public void DontHelpArianWhenYouMeetHim(bool never = false)
        {
            ClearOutput();

            OutputText("Scenes.Arianscene.DontHelpArianWhenYouMeetHim.RisksTooHigh".Localized());

            if (never)
                CoC.Instance.Flags.Set(GameFlagIds.ArianPark, -1);
            else
                CoC.Instance.Flags.Set(GameFlagIds.NotHelpedArianToday, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void HelpArianWhenYouMeetHim()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianPark, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianCapacity, 50);

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ApproachHoodedFigure".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.MasculineVoicePlea".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.OfferShoulderSupport".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryLizans);

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.AuburnSlittedEyes".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.AskAboutTrouble".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ConsiderOptionsDecide".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ArriveAbandonedPark".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ThanksForHelping".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.NoticeCoughing".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.EvasiveHealthProblems".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ForgotIntroduction".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ReassureAboutAides".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.LeaveForNow".Localized());

            OutputText("Scenes.Arianscene.HelpArianWhenYouMeetHim.ParkAddedToMenu".Localized());

            ArianHealth(1);

            ReturnToCamp(1);
        }

        public void VisitThePark()
        {
            ClearOutput();

            ArianSprite();

            ShowImage("arian-park");

            OutputText("Scenes.Arianscene.VisitThePark.SpotSicklyLizan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianPark) == 1)
            {
                OutputText("Scenes.Arianscene.VisitThePark.FeelingPrefix".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Arianscene.VisitThePark.CuriousWord".Localized());
                else
                    OutputText("Scenes.Arianscene.VisitThePark.BoredWord".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.AskHisStory".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.LongStoryBeginning".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.DoAsHeSays".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.ConfessesMage".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.DoubtsMage".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.NotInCovenant".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.AsksHowSo".Localized());

                if (Player.HasSpells())
                    OutputText("Scenes.Arianscene.VisitThePark.NotJustFatigue".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.NewMagicMethod".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.ImpressiveFeatDoubt".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.CoughsThenExplains".Localized());

                OutputText("Scenes.Arianscene.VisitThePark.ExcuseAndLeave".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianPark) == 2)
                {
                    OutputText("Scenes.Arianscene.VisitThePark.RequestContinueStory".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.AcademyOverrun".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.AsksAboutAcademy".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.AcademyDescription".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.NodToContinue".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.DemonsInfiltrated".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.StillListening".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.CouldNotFight".Localized());

                    OutputText("Scenes.Arianscene.VisitThePark.EnoughForNow".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianPark) == 3)
                    {
                        OutputText("Scenes.Arianscene.VisitThePark.AskAboutApprentices".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.ApprenticesAreAides".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.AidesOverlyWorried".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.EscapeIntoDesert".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.MagicalDemonstration".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.NormalWhiteMagic".Localized());

                        if (Player.Corruption > 66)
                            OutputText("Scenes.Arianscene.VisitThePark.ThinksItUseless".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.SentencePeriod".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.ShowsHisTechnique".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.OverdoneItCough".Localized());

                        if (Player.Corruption >= 66)
                            OutputText("Scenes.Arianscene.VisitThePark.HideRealReasons".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.SentencePeriodAgain".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.AsksHelpHome".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.AgreeToWalkHome".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.AtHisDoorstep".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.PromiseToThink".Localized());

                        OutputText("Scenes.Arianscene.VisitThePark.ParkRemovedHouseAdded".Localized());

                        ArianHealth(5);
                    }

            ArianHealth(1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianPark, CoC.Instance.Flags.Get(GameFlagIds.ArianPark) + 1);

            ReturnToCamp(1);
        }

        public void VisitAriansHouse()
        {
            ClearOutput();

            ArianSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 29 || CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) == 1)
                ArianHealth(1);

            if (ArianFollower())
            {
                if (ArianMF("m", "f") == "f")
                    ShowImage("arianfemale-tent");
                else
                    ShowImage("arianmale-tent");

                OutputText("Scenes.Arianscene.VisitAriansHouse.EnterEnchantedTent".Localized());

                var temp = Random.Shared.Next(10);

                if (temp == 0)
                    OutputText("Scenes.Arianscene.VisitAriansHouse.WaitOnCouch".Localized());
                else
                    if (temp == 1)
                        OutputText("Scenes.Arianscene.VisitAriansHouse.ReadingAtTable".Localized());
                    else
                        if (temp <= 2)
                            OutputText("Scenes.Arianscene.VisitAriansHouse.TinkeringOccultGear".Localized());
                        else
                            if (temp <= 4)
                                OutputText("Scenes.Arianscene.VisitAriansHouse.FreshCookingSmell".Localized());
                            else
                                if (temp <= 6)
                                    OutputText("Scenes.Arianscene.VisitAriansHouse.AsleepOnCouch".Localized());
                                else
                                    if (temp <= 8)
                                    {
                                        OutputText("Scenes.Arianscene.VisitAriansHouse.SexSmellNoticed".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                                        {
                                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.MixedFluidsWord".Localized());
                                            else
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.JizzWord".Localized());
                                        }
                                        else
                                            OutputText("Scenes.Arianscene.VisitAriansHouse.FemjizzWord".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.StillHoveringOver".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                                        {
                                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.TwinSlitsWord".Localized());
                                            else
                                            {
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.CockWord".Localized());

                                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                                                    OutputText("Scenes.Arianscene.VisitAriansHouse.SLetter".Localized());
                                            }
                                        }
                                        else
                                            OutputText("Scenes.Arianscene.VisitAriansHouse.PussyWord".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.LongStareBack".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.KnowingSmileReassure".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.StammersPlease".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.CannotWipeSmirk".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.AwkwardSubjectChange".Localized());
                                    }

                ArianHomeMenu();
            }
            else
            {
                if (ArianMF("m", "f") == "f")
                    ShowImage("arianfemale-tent");
                else
                    ShowImage("arianmale-tent");

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianPark) == 4)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ArianPark, CoC.Instance.Flags.Get(GameFlagIds.ArianPark) + 1);

                    OutputText("Scenes.Arianscene.VisitAriansHouse.VisitSicklyMage".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.LaikaOpensDoor".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ClaimAcquaintance".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.FerretClosesDoor".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.AskIfBigDeal".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.BoonStopsLaika".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.GlaresThenComplies".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ProtestCutShort".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Arianscene.VisitAriansHouse.CuriouslyWord".Localized());
                    else
                        if (Player.Corruption < 66)
                            OutputText("Scenes.Arianscene.VisitAriansHouse.CasuallyWord".Localized());
                        else
                            OutputText("Scenes.Arianscene.VisitAriansHouse.IndifferentlyWord".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.WhyThankingYou".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.BoonLeadsUpstairs".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.OpensWoodenDoor".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.CaughtRunningAway".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ArianStayedInside".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.DismissBoon".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.CouldNotResistVisit".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Arianscene.VisitAriansHouse.PullUpChair".Localized());
                    else
                        OutputText("Scenes.Arianscene.VisitAriansHouse.SeatTauricBody".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.PoliteSmallTalk".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ExtraMedicineAfterPark".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.TalkAboutTravels".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ListensLikeChild".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ChatterThenLeave".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.BoonEscortsOut".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.ConfirmToBoon".Localized());

                    OutputText("Scenes.Arianscene.VisitAriansHouse.KeepAnEyeOut".Localized());

                    ReturnToCamp(1);
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianHermChat) == 1)
                    {
                        OutputText("Scenes.Arianscene.VisitAriansHouse.VisitArianConfident".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.LaikaAnswersDoor".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.ThreatensWithDuster".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.BoonTriesCalm".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.AngryAtBrother".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.LaikaStormsOut".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.BoonApologizes".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.AssureFine".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.HeadToAriansRoom".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.VisitAriansHouse.WalkToAriansHouse".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.LaikaGreetsYou".Localized(
                            ArianMF("master", "mistress")
                        ));

                        OutputText("Scenes.Arianscene.VisitAriansHouse.FindWayAlone".Localized(
                            ArianMF("master", "mistress")
                        ));

                        OutputText("Scenes.Arianscene.VisitAriansHouse.AssureBusyFerrets".Localized());
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianHermChat) == 1)
                    {
                        OutputText("Scenes.Arianscene.VisitAriansHouse.SiblingsQuarreling".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.SorryForShock".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.ListenQuarrelEnds".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.SiblingsGetAlong".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.GaspsDeniesChange".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.AskAboutUrges".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.BlushesLayingEggs".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.LaughAtEggs".Localized());

                        OutputText("Scenes.Arianscene.VisitAriansHouse.ChangesSubjectToday".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.ArianHermChat, CoC.Instance.Flags.Get(GameFlagIds.ArianHermChat) + 1);
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 10)
                        {
                            OutputText("Scenes.Arianscene.VisitAriansHouse.FaintCoughInvite".Localized());

                            OutputText("Scenes.Arianscene.VisitAriansHouse.GladYouCame".Localized());

                            OutputText("Scenes.Arianscene.VisitAriansHouse.PullOfferedChair".Localized());

                            OutputText("Scenes.Arianscene.VisitAriansHouse.TalkingIsFine".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 20)
                            {
                                OutputText("Scenes.Arianscene.VisitAriansHouse.GroanComeIn".Localized());

                                OutputText("Scenes.Arianscene.VisitAriansHouse.MotionsToChair".Localized());

                                OutputText("Scenes.Arianscene.VisitAriansHouse.AskHowHeIs".Localized());

                                OutputText("Scenes.Arianscene.VisitAriansHouse.PleasureOfVisit".Localized());
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 30)
                                {
                                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) == 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 2)
                                    {
                                        OutputText("Scenes.Arianscene.VisitAriansHouse.HearFaintMoan".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.SicklyAndHorny".Localized());

                                        ClearButtons();

                                        AddButton(0, "Common.Button.Eavesdrop".Localized(), EavesDropOnArian);

                                        AddButton(1, "Common.Button.Peep".Localized(), PeepOnArian);

                                        AddButton(2, "Common.Button.BargeIn".Localized(), BargeInOnArian);

                                        AddButton(3, "Common.Button.Leave".Localized(), LeaveFappingArian);

                                        return ;
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Arianscene.VisitAriansHouse.SlightPepVoice".Localized());

                                        if (!Player.IsTaur())
                                            OutputText("Scenes.Arianscene.VisitAriansHouse.SitBesideBed".Localized());
                                        else
                                            OutputText("Scenes.Arianscene.VisitAriansHouse.SitSilentlyBeside".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.AsksWhatToday".Localized());
                                    }
                                }
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 50)
                                    {
                                        OutputText("Scenes.Arianscene.VisitAriansHouse.DrawerNoiseInvite".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.WondersWhatWorking".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.AskWhyThanks".Localized());

                                        OutputText("Scenes.Arianscene.VisitAriansHouse.ConsiderWhatNext".Localized());
                                    }
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 75)
                                        {
                                            OutputText("Scenes.Arianscene.VisitAriansHouse.ArianHappyInvite".Localized());

                                            OutputText("Scenes.Arianscene.VisitAriansHouse.DeclineTea".Localized());

                                            if (!Player.IsTaur())
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.TakeOfferedChair".Localized());
                                            else
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.OfferedChairWord".Localized());

                                            OutputText("Scenes.Arianscene.VisitAriansHouse.SentencePeriodFifth".Localized());

                                            OutputText("Scenes.Arianscene.VisitAriansHouse.SetsMugDown".Localized());
                                        }
                                        else
                                        {
                                            OutputText("Scenes.Arianscene.VisitAriansHouse.BrightSmileWelcome".Localized());

                                            OutputText("Scenes.Arianscene.VisitAriansHouse.BreaksHugLeads".Localized());

                                            if (!Player.IsTaur())
                                                OutputText("Scenes.Arianscene.VisitAriansHouse.OffersChairNearby".Localized());

                                            OutputText("Scenes.Arianscene.VisitAriansHouse.TakingAnotherChair".Localized());
                                        }

                    ArianHomeMenu();
                }
            }
        }

        public void ArianHomeMenu()
        {

            var seaShirt = Undergarments.SEA_SHIRT;

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 0 && ArianHealth() >= 10)
                AddButton(0, "Common.Button.Next".Localized(), ArianStoryDialogue1);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 1 && ArianHealth() >= 20)
                    AddButton(0, "Common.Button.Next".Localized(), ArianStoryDialogue2);
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 2 && ArianHealth() >= 30)
                        AddButton(0, "Common.Button.Next".Localized(), ArianDialogue3);
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 3 && ArianHealth() >= 50)
                            AddButton(0, "Common.Button.Next".Localized(), ArianImbue);
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 4 && ArianHealth() >= 75)
                                AddButton(0, "Common.Button.Next".Localized(), ArianPlot4);
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 5 && ArianHealth() >= 100)
                                    AddButton(0, "Common.Button.Next".Localized(), ArianPlot5);
                                else
                                {
                                    if (seaShirt.Save.GetInt("arianDisenchant", 0) != 0 && View.Time.Days > seaShirt.Save.GetInt("arianDisenchantDay", 0) + 1)
                                    {
                                        OutputText("Scenes.Arianscene.ArianHomeMenu.AskAboutShirt".Localized());

                                        OutputText("Scenes.Arianscene.ArianHomeMenu.BreaksBinding".Localized());

                                        OutputText("Scenes.Arianscene.ArianHomeMenu.AppreciatedEffort".Localized());

                                        seaShirt.Save.SetInt("arianDisenchant", 0);

                                        seaShirt.Save.SetInt("bound", 0);

            Player.Inventory.Add(seaShirt, 1);

                                        ArianHomeMenu();

                                        return ;
                                    }

                                    AddButton(0, "Common.Button.Appearance".Localized(), ArianAppearance);

                                    AddButton(1, "Common.Button.Talk".Localized(), TalkToArianChoices);

                                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) >= 2)
                                        AddButton(2, "Common.Button.Sex".Localized(), () => ArianSexMenu());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) >= 1)
                                        AddButton(5, "Common.Button.GiveItem".Localized(), GiveArianAnItem);

                                    if (ArianFollower())
                                        AddRowButton(1, "Affection", ArianAffection);

                                    if (Player.HasKeyItem("Arian's Talisman") || Player.HasKeyItem("Arian's Charged Talisman"))
                                        AddRowButton(1, "Talisman", ImbueTalisman);

                                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) >= 5)
                                        AddRowButton(1, "Treat Corr.", TreatCorruption);

                                    if (ArianFollower() && View.Time.Hours > 20)
                                        AddButton(9, "Common.Button.SleepWith".Localized(), () => SleepWithArian(true));

                                    if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Arian")
                                        AddButton(9, "Common.Button.SleepAlone".Localized(), DontSleepWithArian);

                                    if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !((Areas.Swamp.AlrauneScene.Save.GetInt("questAsked", 0) & Areas.Swamp.AlrauneScene.ASKEDAM) != 0))
                                        AddButton(10, "Common.Button.Vines".Localized(), null );

                                    if (ArianFollower() && Player.HasItem(ItemRegistry.Create(ItemIds.Lethicite)))
                                        AddRowButton(2, "Lethicite", GiveLethicite);

                                    if (Player.UpperGarment == seaShirt && seaShirt.knownBound() && seaShirt.Save.GetInt("arianAsked", 0) == 0)
                                        AddRowButton(2, "Cursed Shirt", AskArianAboutNobleShirt);
                                    else
                                        if (Player.HasItem(seaShirt) && seaShirt.Save.GetInt("bound", 0) != 0 && seaShirt.Save.GetInt("arianDisenchant", 0) == 0)
                                            AddRowButton(2, "Disenchant", AskArianToDisenchant);

                                    if (!ArianFollower())
                                        AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                                    else
                                        AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
                                }
        }

        public void ArianAppearance()
        {
            ClearOutput();

            RegisterMacro("arianbreasts", CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) > 0);

            RegisterMacro("ariancocks", CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0 ? 2 : (ArianCockSize() != 0 ? 1 : 0));

            RegisterMacro("arianvagina", CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0);

            OutputText("Scenes.Arianscene.ArianAppearance.NoLongerSickly".Localized(
                ArianMF("masculine", "feminine")
            ));

            OutputText("Scenes.Arianscene.ArianAppearance.BreastsAndNipples".Localized());

            OutputText("Scenes.Arianscene.ArianAppearance.CockDescription".Localized());

            OutputText("Scenes.Arianscene.ArianAppearance.AnusDescription".Localized());

            VisitAriansHouse();
        }

        public void DontSleepWithArian()
        {
            ClearOutput();

            OutputText("Scenes.Arianscene.DontSleepWithArian.DeclineSleeping".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");

            ArianHomeMenu();
        }

        public void EavesDropOnArian()
        {
            ClearOutput();

            ArianSprite();

            ShowImage("arian-fap");

            OutputText("Scenes.Arianscene.EavesDropOnArian.EarAgainstDoor".Localized());

            OutputText("Scenes.Arianscene.EavesDropOnArian.CursesIllnessDreams".Localized());

            OutputText("Scenes.Arianscene.EavesDropOnArian.ReptileHasCrush".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.BargeIn".Localized(), BargeInOnArian);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveFappingArian);
        }

        public void PeepOnArian()
        {
            ClearOutput();

            ArianSprite();

            ShowImage("arian-fap");

            OutputText("Scenes.Arianscene.PeepOnArian.PeekThroughLock".Localized());

            if (Player.CountCocksOfType("LIZARD") > 0)
                OutputText("Scenes.Arianscene.PeepOnArian.JustLikeYours".Localized());

            OutputText("Scenes.Arianscene.PeepOnArian.StillStrokingOff".Localized());

            OutputText("Scenes.Arianscene.PeepOnArian.CursesIllnessDreams".Localized());

            OutputText("Scenes.Arianscene.PeepOnArian.ReptileHasCrush".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.BargeIn".Localized(), BargeInOnArian);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveFappingArian);
        }

        public void LeaveFappingArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.LeaveFappingArian.GiveHimPrivacy".Localized());

            OutputText("Scenes.Arianscene.LeaveFappingArian.BoonAsksLeaving".Localized());

            OutputText("Scenes.Arianscene.LeaveFappingArian.ArianSeemsBusy".Localized());

            OutputText("Scenes.Arianscene.LeaveFappingArian.BoonOffersCall".Localized());

            OutputText("Scenes.Arianscene.LeaveFappingArian.ReturnLaterFarewell".Localized());

            ReturnToCamp(1);
        }

        public void BargeInOnArian()
        {
            ClearOutput();

            ArianSprite();

            ShowImage("arian-fap");

            OutputText("Scenes.Arianscene.BargeInOnArian.DoorUnlockedSmirk".Localized());

            OutputText("Scenes.Arianscene.BargeInOnArian.HidesUnderCovers".Localized());

            OutputText("Scenes.Arianscene.BargeInOnArian.FlatteredToBeObject".Localized());

            OutputText("Scenes.Arianscene.BargeInOnArian.PeeksFromCovers".Localized());

            if (Player.IsMale())
            {
                OutputText("Scenes.Arianscene.BargeInOnArian.AssumedBothMale".Localized());

                OutputText("Scenes.Arianscene.BargeInOnArian.NotOpposedMale".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.DontMind".Localized(), YouDontMindBeingGayForArian);

                AddButton(1, "Common.Button.LikeGirls".Localized(), YouLikeGirlsNotSickLizardDudes);
            }
            else
                if (Player.IsGenderless())
                {
                    OutputText("Scenes.Arianscene.BargeInOnArian.FidgetsExplaining".Localized());

                    OutputText("Scenes.Arianscene.BargeInOnArian.RevealNotMale".Localized());

                    ArianSexMenu(false);
                }
                else
                    if (Player.IsFemale())
                    {
                        OutputText("Scenes.Arianscene.BargeInOnArian.TrailsOffUnsure".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.WorriedAboutSexuality".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.PreferMen".Localized(), GirlsLikeMaleArian);

                        AddButton(1, "Common.Button.PreferWomen".Localized(), GirlsLikeFemaleArian);
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.BargeInOnArian.TrailsOffQuietly".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.AdorableHopelessVirgin".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.HidesFaceWhisper".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.PullCoversAndFix".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.EyesWidenReveal".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.BreaksHisTrance".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.BothGendersNervous".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.BlurtsOutDenial".Localized());

                        OutputText("Scenes.Arianscene.BargeInOnArian.RubChinThought".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.LikeMale".Localized(), HermsLikeMaleArian);

                        AddButton(1, "Common.Button.LikeFemale".Localized(), HermsLikeFemaleArian);
                    }
        }

        public void GirlsLikeMaleArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.GirlsLikeMaleArian.PreferMenAdorable".Localized());

            GetPenetratedByArianAndHisHitlerMustache();
        }

        public void GirlsLikeFemaleArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.GirlsLikeFemaleArian.PreferGirls".Localized());

            OutputText("Scenes.Arianscene.GirlsLikeFemaleArian.ImagineAsWoman".Localized());

            OutputText("Scenes.Arianscene.GirlsLikeFemaleArian.PotionEffectsStart".Localized());

            OutputText("Scenes.Arianscene.GirlsLikeFemaleArian.NervousHowLook".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, 0);

            CoC.Instance.Flags.Set(GameFlagIds.ArianVagina, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianHermChat, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Facesitting".Localized(), ArianFacesittingFirst);

            AddButton(1, "Common.Button.DildoFun".Localized(), ArianDildoFun);
        }

        public void HermsLikeMaleArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.HermsLikeMaleArian.FineWithGuy".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(50))
                AddButton(0, "Common.Button.Penetrate".Localized(), GiveArianAnal);
            else
                AddButton(0, "Common.Button.Penetrate".Localized(), null);

            AddButton(1, "Common.Button.GetPenetrated".Localized(), GetPenetratedByArianAndHisHitlerMustache);
        }

        public void HermsLikeFemaleArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.GuysSometimesGirls".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.PrefersGirlQuestion".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.ImagineGirlLove".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.WondersHisPlan".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.GetsUpExposed".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.EffectsStartQuickly".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.TransformationContinues".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.NervousSmileAsks".Localized());

            OutputText("Scenes.Arianscene.HermsLikeFemaleArian.HelpHerToBed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianHermChat, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianVagina, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, 0);

            CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PenetrateArian);
        }

        public void YouDontMindBeingGayForArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.YouDontMindBeingGayForArian.CuteMalesFine".Localized());

            OutputText("Scenes.Arianscene.YouDontMindBeingGayForArian.HopelessVirginNod".Localized());

            OutputText("Scenes.Arianscene.YouDontMindBeingGayForArian.FaceHiddenWhisper".Localized());

            OutputText("Scenes.Arianscene.YouDontMindBeingGayForArian.FixThatStrip".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(50))
                AddButton(0, "Common.Button.Penetrate".Localized(), GiveArianAnal);
            else
                AddButton(0, "Common.Button.Penetrate".Localized(), null);

            AddButton(1, "Common.Button.GetPenetrated".Localized(), GetButtWreckedByArian);
        }

        public void YouLikeGirlsNotSickLizardDudes()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.PreferFemales".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.WouldNotMindGirl".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.RaisesBrowsCurious".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.StripsOffRobes".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.EffectsBeginFast".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.TransformationOngoing".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.AsksHowSheLooks".Localized());

            OutputText("Scenes.Arianscene.YouLikeGirlsNotSickLizardDudes.PushHerToBed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianHermChat, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianVagina, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PenetrateArian);
        }

        public void ArianStoryDialogue1()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.AskAboutHisPast".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.ArianNodsShareStories".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.NoncommittalShrug".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMarae) >= 1)
                OutputText("Scenes.Arianscene.ArianStoryDialogue1.MaraeSaidTheyAppeared".Localized());
            else
                OutputText("Scenes.Arianscene.ArianStoryDialogue1.GuessLongTimeAgo".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.AsksIfGuessRight".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.StudyReptilianWizard".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.ArianRevealsHisAge".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.WhyParentsShelteredHim".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.WorldWasMess".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.AdmitHeToldYou".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.BooksOnlyLove".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.OddBookwormWantsOut".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.EnjoyedTeachingStudents".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.EyesLightRecollection".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.SoundsQuiteLonely".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.ThrilledTwoStudents".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.AskWhySpecialAttention".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.IntendedDefeatDemons".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.FrailFromShielding".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.SpellcastingTakesToll".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.PatHisShoulder".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.EscapedTheAcademy".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.NeverSawTheSky".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.ParentsKeptHimInside".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.CheerUpDepressedLizan".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.TriedManyCuresAlready".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.PressForAnyRumor".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.RecallsPossibleCure".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.HelpIfHeBehaves".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.GoingInsaneIndoors".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.UnderstandButMustRest".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.EnjoysYourCompany".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.InsistHeRests".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.TrustsYouAtLast".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.FirmHandshakeDeal".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue1.WishHimGoodSleep".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianSDialogue, CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) + 1);

            ReturnToCamp(1);
        }

        public void ArianStoryDialogue2()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.NoticesHeLooksBetter".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.ThanksThenSighs".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.ConfusingDreamsLately".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.NotBadJustUnusual".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.QuestioningLook".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.DreamStartsStanding".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.UrgeHimToRemember".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.WhatHappensLater".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.RealizesLustfulDream".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.WorriesWhoDreamLover".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.AsksAboutDreamLover".Localized());

            OutputText("Scenes.Arianscene.ArianStoryDialogue2.BegsToForgetIt".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianSDialogue, CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.DropIt".Localized(), ArianStory2DropIt);

            AddButton(1, "Common.Button.Pry".Localized(), ArianStoryPry);
        }

        public void ArianStory2DropIt()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianStory2DropIt.DropsTheSubject".Localized());

            OutputText("Scenes.Arianscene.ArianStory2DropIt.TucksHimselfIn".Localized());

            ReturnToCamp(1);
        }

        public void ArianStoryPry()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianStoryPry.WontLetHimWriggle".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Arianscene.ArianStoryPry.SleepBetterUnburdenedSuffix".Localized());

            OutputText("Scenes.Arianscene.ArianStoryPry.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.ArianStoryPry.AdmitsSomeoneClose".Localized());

            OutputText("Scenes.Arianscene.ArianStoryPry.IntriguingButSilent".Localized());

            ReturnToCamp(1);
        }

        public void ArianDialogue3()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianDialogue3.AsksAboutMagicInterest".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianSDialogue, CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesArianShouldMagicTeach);

            AddButton(1, "Common.Button.No".Localized(), NoArianShouldntMagicTeach);
        }

        public void YesArianShouldMagicTeach()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.YesArianShouldMagicTeach.EagerToLearnSpells".Localized());

            OutputText("Scenes.Arianscene.YesArianShouldMagicTeach.SmirkStillInterested".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ArianMagicLessons);
        }

        public void NoArianShouldntMagicTeach()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.NoArianShouldntMagicTeach.DeclinesForNow".Localized());

            OutputText("Scenes.Arianscene.NoArianShouldntMagicTeach.HappyToHelpYou".Localized());

            ArianHomeMenu();
        }

        public void ArianImbue()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianImbue.ArianHasSurprise".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.WondersWhatItIs".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.WrappedPackageFromDrawer".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.UnwrapTheGift".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.MadeItHimself".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.CallsItBeautiful".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.WearTheNecklace".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.FreezesAtProblem".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.ReliefOfferIngredients".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.TalismanHoldsOneSpell".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.GiftWillBeUseful".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.AsksWhatYouWant".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.SmolderingEyesBurn".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.BlurtsOutDesire".Localized());

            OutputText("Scenes.Arianscene.ArianImbue.PromptSexWithArian".Localized());

            Player.CreateKeyItem("Arian's Talisman", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.ArianSDialogue, CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesPlotSexArian);

            AddButton(1, "Common.Button.No".Localized(), NoPlotSexNauArian);
        }

        public void YesPlotSexArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.YesPlotSexArian.EmbracesAmorousLizan".Localized());

            ArianSexMenu(false);
        }

        public void NoPlotSexNauArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.NoPlotSexNauArian.ApologizeNotInMood".Localized());

            OutputText("Scenes.Arianscene.NoPlotSexNauArian.DisappointedButAccepts".Localized());

            OutputText("Scenes.Arianscene.NoPlotSexNauArian.AgreeNextTime".Localized());

            ArianHomeMenu();
        }

        public void ArianPlot4()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianPlot4.ArianHasGoodNews".Localized());

            OutputText("Scenes.Arianscene.ArianPlot4.AsksWhatNews".Localized());

            OutputText("Scenes.Arianscene.ArianPlot4.CanChannelMagicThrough".Localized());

            OutputText("Scenes.Arianscene.ArianPlot4.UsefulDailyTrick".Localized());

            OutputText("Scenes.Arianscene.ArianPlot4.AnythingForYou".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianSDialogue, CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) + 1);

            ArianHomeMenu();
        }

        public void ArianPlot5()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianPlot5.WantsToAskSomething".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.AsksToLiveTogether".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.ExplainsPortalDuties".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.HeWouldMoveIn".Localized());

            if (CampScene.CompanionsCount() > 1)
                OutputText("Scenes.Arianscene.ArianPlot5.OthersAlreadyLiveThere".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.SayEnd".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.WeighsLivingRequest".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.SpokeWithOthersAlready".Localized());

            OutputText("Scenes.Arianscene.ArianPlot5.PromptFinalDecision".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianSDialogue, CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptArianMovingIntoCamp);

            AddButton(1, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), DenyAriansMoveIn);
        }

        public void AcceptArianMovingIntoCamp()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.AcceptArianMovingIntoCamp.DelightedHeMovesIn".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), InviteArianToCamp);
        }

        public void DenyAriansMoveIn()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.DenyAriansMoveIn.AskTimeToThink".Localized());

            ArianHomeMenu();
        }

        public void TalkToArianChoices()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.TalkToArianChoices.AskToChat".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) > 0)
                AddButton(0, "Common.Button.SexyTalk".Localized(), ArianSexingTalk);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) >= 3)
                AddButton(1, "Common.Button.TeachMagic".Localized(), ArianMagicLessons);

            if (!ArianFollower() && CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) >= 6)
                AddButton(4, "Common.Button.Invite2camp".Localized(), InviteArianToCamp);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) == 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) < 3)
                OutputText("Scenes.Arianscene.TalkToArianChoices.NothingToTalkAbout".Localized());

            AddButton(14, "Common.Button.Back".Localized(), ArianHomeMenu);
        }

        public void ArianMagicLessons()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianMagicLessons.RequestsMagicLesson".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianLessons) >= 4)
            {
                OutputText("Scenes.Arianscene.ArianMagicLessons.RaisesHandToStop".Localized());

                OutputText("Scenes.Arianscene.ArianMagicLessons.AgreesAskTomorrow".Localized());

                ReturnToCamp(1);

                return ;
            }

            OutputText("Scenes.Arianscene.ArianMagicLessons.AgreesWhereToBegin".Localized());

            if (Player.Intelligence < 25)
            {
                OutputText("Scenes.Arianscene.ArianMagicLessons.NewToMagicBasics".Localized());

                OutputText("Scenes.Arianscene.ArianMagicLessons.MagicBasisWillpower".Localized());

                OutputText("Scenes.Arianscene.ArianMagicLessons.TakesMentalNotes".Localized());

                OutputText("Scenes.Arianscene.ArianMagicLessons.AwaitsYourQuestions".Localized());

                OutputText("Scenes.Arianscene.ArianMagicLessons.OffersToRepeat".Localized());

                OutputText("Scenes.Arianscene.ArianMagicLessons.ThankLessonPrefix".Localized());

                if (!ArianFollower())
                    OutputText("Scenes.Arianscene.ArianMagicLessons.CampReturnSuffix".Localized());
                else
                    OutputText("Scenes.Arianscene.ArianMagicLessons.ReturnToDutiesSuffix".Localized());
            }
            else
                if (Player.Intelligence < 50)
                {
                    OutputText("Scenes.Arianscene.ArianMagicLessons.ReadyForAdvancedTopics".Localized());

                    OutputText("Scenes.Arianscene.ArianMagicLessons.SuggestsChanneling".Localized());

                    OutputText("Scenes.Arianscene.ArianMagicLessons.AsksWhatChannelingIs".Localized());

                    OutputText("Scenes.Arianscene.ArianMagicLessons.EnchantingExplained".Localized());

                    OutputText("Scenes.Arianscene.ArianMagicLessons.InterestingSubject".Localized());

                    OutputText("Scenes.Arianscene.ArianMagicLessons.DontOverchargeConduit".Localized());

                    OutputText("Scenes.Arianscene.ArianMagicLessons.TiredAfterLessonPrefix".Localized());

                    if (!ArianFollower())
                        OutputText("Scenes.Arianscene.ArianMagicLessons.BackToCampSuffix".Localized());
                    else
                        OutputText("Scenes.Arianscene.ArianMagicLessons.ExitTentBusinessSuffix".Localized());

                    if (!Player.HasStatusEffect(StatusEffectIds.KnowsCharge))
                    {
                        OutputText("Scenes.Arianscene.ArianMagicLessons.PondersLessonApplying".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.TryItSometime".Localized());

                        Player.CreateStatusEffect(StatusEffectIds.KnowsCharge, 0, 0, 0, 0);
                    }
                }
                else
                    if (Player.Intelligence < 75)
                    {
                        OutputText("Scenes.Arianscene.ArianMagicLessons.ReadyToStartConjuring".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.SuggestsConjuration".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.LecturesOnConjuration".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.QuiteComplexSubject".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.PracticalExampleGiven".Localized());

                        if (!Player.HasStatusEffect(StatusEffectIds.KnowsBlind))
                        {
                            OutputText("Scenes.Arianscene.ArianMagicLessons.CuriousForExample".Localized());

                            OutputText("Scenes.Arianscene.ArianMagicLessons.LiftsClosedFist".Localized());

                            OutputText("Scenes.Arianscene.ArianMagicLessons.UsefulInCombat".Localized());

                            OutputText("Scenes.Arianscene.ArianMagicLessons.PracticeNewSpell".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.KnowsBlind, 0, 0, 0, 0);
                        }
                        else
                        {
                            OutputText("Scenes.Arianscene.ArianMagicLessons.RatherThanBarrier".Localized());

                            OutputText("Scenes.Arianscene.ArianMagicLessons.ExactlyWhatHeMeans".Localized());

                            OutputText("Scenes.Arianscene.ArianMagicLessons.ThanksAndFarewell".Localized());
                        }
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.ArianMagicLessons.ShareYourTheories".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.EyesAlightInterest".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.CouldAlterElements".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.PraisesAlterationIdea".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.ListensToLizan".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.AcademyMagesResearched".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.ConversationInteresting".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.GladToBeUseful".Localized());

                        OutputText("Scenes.Arianscene.ArianMagicLessons.PromiseReturnPrefix".Localized());

                        if (!ArianFollower())
                            OutputText("Scenes.Arianscene.ArianMagicLessons.TrekHomeSuffix".Localized());
                        else
                            OutputText("Scenes.Arianscene.ArianMagicLessons.ExitTentSuffix".Localized());

                        if (!Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                        {
                            OutputText("Scenes.Arianscene.ArianMagicLessons.CombineBothArts".Localized());

                            OutputText("Scenes.Arianscene.ArianMagicLessons.PutIntoPractice".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.KnowsWhitefire, 0, 0, 0, 0);
                        }
                    }

            CoC.Instance.Flags.Set(GameFlagIds.ArianLessons, CoC.Instance.Flags.Get(GameFlagIds.ArianLessons) + 1);

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Intelligence < 75)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Intelligence < 50)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            ReturnToCamp(1);
        }

        public void ArianSexingTalk()
        {
            ClearOutput();

            ArianHealth(1);

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianSexingTalk.AsksAboutSexNow".Localized());

            OutputText("Scenes.Arianscene.ArianSexingTalk.AvoidsTheTopic".Localized());

            OutputText("Scenes.Arianscene.ArianSexingTalk.TakesDeepBreath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) != 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                {
                    OutputText("Scenes.Arianscene.ArianSexingTalk.ResentsOneDick".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.SurprisedPrompt".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.FreakishOneDick".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.FindsHimAdorable".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.SomethingCouldDoThat".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.FineAsHeIs".Localized());
                }
                else
                {
                    OutputText("Scenes.Arianscene.ArianSexingTalk.NoLongerWorries".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.DoubleThePleasure".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.SheepishAgreement".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.DoubleTheWork".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.RemembersCaughtHim".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.NervousLaugh".Localized());
                }

                OutputText("Scenes.Arianscene.ArianSexingTalk.CouldChangeHisSize".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 1)
                    OutputText("Scenes.Arianscene.ArianSexingTalk.NoComplaintsAtAll".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 2)
                        OutputText("Scenes.Arianscene.ArianSexingTalk.BiggerThanHoped".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
                        {
                            OutputText("Scenes.Arianscene.ArianSexingTalk.KeptItForYou".Localized());

                            OutputText("Scenes.Arianscene.ArianSexingTalk.AdmitsItFeelsGood".Localized());
                        }

                OutputText("Scenes.Arianscene.ArianSexingTalk.WillThinkAboutIt".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
            {
                OutputText("Scenes.Arianscene.ArianSexingTalk.ArousedFemaleScent".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.TurningIntoGirl".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.AsksHowHeFeels".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.StrangeNewUrges".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.AsksAboveAllElse".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.CloseMakesHimHot".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.ThenTheEggs".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.AsksAboutEggs".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.LizanEggLaying".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.AsksThinkingOfWhat".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.SaysAboutYou".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.OfferToHelpLay".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.WouldLikeThat".Localized());

                if (ArianFollower())
                    OutputText("Scenes.Arianscene.ArianSexingTalk.PromiseToStopBy".Localized());
                else
                    OutputText("Scenes.Arianscene.ArianSexingTalk.BoonLaikaWouldBerate".Localized());
            }

            OutputText("Scenes.Arianscene.ArianSexingTalk.AskPreferredSexForm".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
            {
                OutputText("Scenes.Arianscene.ArianSexingTalk.ShrugsUndecided".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.NeedsMorePractice".Localized());

                OutputText("Scenes.Arianscene.ArianSexingTalk.NervousNodAgreement".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                {
                    OutputText("Scenes.Arianscene.ArianSexingTalk.RecallsAnalEncounter".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.SurprisedAtPreference".Localized());

                    OutputText("Scenes.Arianscene.ArianSexingTalk.OnlyWithYou".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) <= 100)
                    {
                        OutputText("Scenes.Arianscene.ArianSexingTalk.EnjoysBeingUsed".Localized());

                        OutputText("Scenes.Arianscene.ArianSexingTalk.RareToFindThis".Localized());

                        OutputText("Scenes.Arianscene.ArianSexingTalk.CallsHimselfPervert".Localized());

                        OutputText("Scenes.Arianscene.ArianSexingTalk.NothingSurprisesYou".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.ArianSexingTalk.CantStopThinking".Localized());

                        OutputText("Scenes.Arianscene.ArianSexingTalk.ForwardButWelcome".Localized());

                        OutputText("Scenes.Arianscene.ArianSexingTalk.GlancesAtYourCrotch".Localized());

                        if (!Player.HasCock())
                        {
                            OutputText("Scenes.Arianscene.ArianSexingTalk.SighsAtNoBulge".Localized());

                            OutputText("Scenes.Arianscene.ArianSexingTalk.ConsiderItLater".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Arianscene.ArianSexingTalk.AsksToUseHim".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Yes".Localized(), YesYouButtslutIllFuckYou);

                            AddButton(1, "Common.Button.No".Localized(), GoddamnitNoYouButtSlut);

                            return ;
                        }
                    }

            SexTalkFinish();
        }

        public void YesYouButtslutIllFuckYou()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.YesYouButtslutIllFuckYou.TellHimToStrip".Localized());

            OutputText("Scenes.Arianscene.YesYouButtslutIllFuckYou.StripsOntoTheBed".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GiveArianAnal);
        }

        public void GoddamnitNoYouButtSlut()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.GoddamnitNoYouButtSlut.CantDoThatNow".Localized());

            OutputText("Scenes.Arianscene.GoddamnitNoYouButtSlut.QuietNod".Localized());

            SexTalkFinish(false);
        }

        public void SexTalkFinish(bool newl = false)
        {
            ClearOutput();

            ArianSprite();

            if (!(newl))
                OutputText("Scenes.Arianscene.SexTalkFinish.ParagraphBreak".Localized());

            OutputText("Scenes.Arianscene.SexTalkFinish.PatHeadAndLeave".Localized());

            ReturnToCamp(1);
        }

        public void InviteArianToCamp()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.InviteArianToCamp.AskLizanLeaving".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianConfirmsLeaving".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.YouAgreeReluctantly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) == 0)
                    OutputText("Scenes.Arianscene.InviteArianToCamp.ManWord".Localized());
                else
                    OutputText("Scenes.Arianscene.InviteArianToCamp.HermWord".Localized());
            }
            else
                OutputText("Scenes.Arianscene.InviteArianToCamp.WomanWord".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianPacksToGo".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianBringsOutBags".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianGrabsBothBags".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.SkepticalAboutBags".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianPuzzledByJoke".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.DisbeliefAtSmallBag".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianShrugsAtBags".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.SighAndResign".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianCallsForTwins".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianHugsTheTwins".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.BoonHappyForYou".Localized(
                ArianMF("master", "mistress")
            ));

            OutputText("Scenes.Arianscene.InviteArianToCamp.LaikaFussesOverYou".Localized(
                ArianMF("master", "mistress")
            ));

            OutputText("Scenes.Arianscene.InviteArianToCamp.PupilsNowFullMages".Localized(
                ArianMF("master", "mistress")
            ));

            OutputText("Scenes.Arianscene.InviteArianToCamp.BoonBreaksHugFirst".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.TwinsAskTakeCare".Localized(
                ArianMF("mas", "mis")
            ));

            OutputText("Scenes.Arianscene.InviteArianToCamp.LaikaThreatensScolding".Localized(
                ArianMF("master", "mistress")
            ));

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianBegsNoLaika".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.LaughAtHappyFamily".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.ArianProtestsNotBad".Localized());

            OutputText("Scenes.Arianscene.InviteArianToCamp.FarewellsThenHeadHome".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TakeYerLizardHomePartII);
        }

        public void TakeYerLizardHomePartII()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianNoticesPortal".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ConfirmPortalOrigin".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianPondersPortal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0)
                OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianAdmiresCabin".Localized());
            else
                OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.TentWord".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.AsksToStayHere".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.SayStayIsFine".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianPlansOwnTent".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.NeverSawTent".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianWarnsStepBack".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ObeyDespiteDoubt".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.BagBurstsIntoCloth".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ClaimsMasterMage".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.SurprisedByMagicTent".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianHoldsTarpOpen".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.GaspAtTentInterior".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.KitchenInTheCorner".Localized());

            if (CampScene.FollowersCount() > 1)
                OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.AndOtherCompanions".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianAsksCozy".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.TellVeryCozy".Localized());

            if (!CabinInterior.HasBed)
                OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.JestAboutSleepingHere".Localized());
            else
                OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.SayStart".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.OfferToShareBed".Localized());

            if (!CabinInterior.HasBed)
                OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.JestKeepInMind".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.MaybeDropByNight".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ArianSaysAlwaysWelcome".Localized());

            OutputText("Scenes.Arianscene.TakeYerLizardHomePartII.ExcuseYourselfPolitely".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianFollower, 1);

            ReturnToCamp(1);
        }

        public void ArianSexMenu(bool output = true)
        {
            ClearOutput();

            ArianSprite();

            if (output)
            {
                OutputText("Scenes.Arianscene.ArianSexMenu.AskIfStrongEnough".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 30)
                    OutputText("Scenes.Arianscene.ArianSexMenu.ArianNervousAccepts".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 50)
                        OutputText("Scenes.Arianscene.ArianSexMenu.ArianWantsToLearn".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 75)
                        {
                            OutputText("Scenes.Arianscene.ArianSexMenu.AsksIfJustThanks".Localized());

                            OutputText("Scenes.Arianscene.ArianSexMenu.AssureJustInMood".Localized());

                            OutputText("Scenes.Arianscene.ArianSexMenu.OkayWhatDoing".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 100)
                                OutputText("Scenes.Arianscene.ArianSexMenu.AlwaysEnoughStrength".Localized());
                            else
                                OutputText("Scenes.Arianscene.ArianSexMenu.NoNeedToAsk".Localized());

                OutputText("Scenes.Arianscene.ArianSexMenu.UndressWithLover".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.AnalPitch".Localized(), null);

            AddButton(1, "Common.Button.AnalCatch".Localized(), null);

            AddButton(2, "Common.Button.GetBlown".Localized(), null);

            AddButton(3, "Common.Text.Blow".Localized() + ArianMF("Him", "Her"), null);

            AddButton(4, "Common.Button.FuckVagina".Localized(), null);

            AddButton(5, "Common.Button.GetFucked".Localized(), null);

            AddButton(6, "Common.Button.DildoFun".Localized(), null);

            AddButton(7, "Common.Button.Docking".Localized(), null);

            AddButton(8, "Common.Button.Doublepen".Localized(), null);

            AddButton(9, "Common.Button.Cunnilingus".Localized(), null);

            AddButton(10, "Common.Button.Facesitting".Localized(), null);

            AddButton(11, "Common.Button.Tailriding".Localized(), null);

            if (silly)
                AddButton(12, "Common.Button.Headpat".Localized(), null);

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(50))
                    AddButton(0, "Common.Button.AnalPitch".Localized(), GiveArianAnal);

                AddButton(2, "Common.Button.GetBlown".Localized(), GetBlownByArian);

                if (Player.HasCockThatFits(CoC.Instance.Flags.Get(GameFlagIds.ArianCapacity)) && CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    AddButton(4, "Common.Button.FuckVagina".Localized(), PenetrateArian);

                if (Player.HasCockThatFits(50) && Player.CockThatFits2(50) >= 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    AddButton(8, "Common.Button.Doublepen".Localized(), DoublePenetrateArian);

                if (Player.SmallestCockArea() <= 30 && CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) < 3 && CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                    AddButton(7, "Common.Button.Docking".Localized(), ArianDocking);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                AddButton(1, "Common.Button.AnalCatch".Localized(), GetButtWreckedByArian);

                AddButton(3, "Common.Text.Blow".Localized() + ArianMF("Him", "Her"), SuckAriansDick);
            }

            if (Player.HasVagina())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                    AddButton(5, "Common.Button.GetFucked".Localized(), GetPenetratedByArianAndHisHitlerMustache);

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                {
                    AddButton(9, "Common.Button.Cunnilingus".Localized(), ArianCunnilingus);

                    AddButton(10, "Common.Button.Facesitting".Localized(), ArianFacesitting);

                    AddButton(11, "Common.Button.Tailriding".Localized(), ArianFemaleTailriding);

                    if (Player.HasKeyItem("Dildo") || Player.HasKeyItem("Deluxe Dildo"))
                        AddButton(6, "Common.Button.DildoFun".Localized(), ArianDildoFun);

                    if (silly)
                        AddButton(12, "Common.Button.Headpat".Localized(), ArianHeadpats);
                }
            }

            AddButton(14, "Common.Button.Back".Localized(), ArianHomeMenu);
        }

        public void GiveArianAnal()
        {
            ClearOutput();

            ArianSprite();

            var x = Player.CockThatFits(50);

            ArianHealth(3);

            CoC.Instance.Flags.Set(GameFlagIds.ArianAnalXp, CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) + 10);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) >= 100)
                CoC.Instance.Flags.Set(GameFlagIds.ArianAnalXp, 100);

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            if (ArianMF("m", "f") == "f")
                ShowImage("arianfemale-home-GiveArianAnal");
            else
                ShowImage("arianmale-home-GiveArianAnal");

            if (x == -1)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Arianscene.GiveArianAnal.AskTakeFromBehind".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
            {
                OutputText("Scenes.Arianscene.GiveArianAnal.AsksToBeGentle".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.PromiseAndClimbBed".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) < 1)
                    OutputText("Scenes.Arianscene.GiveArianAnal.AsksIfBadThing".Localized());
                else
                {
                    OutputText("Scenes.Arianscene.GiveArianAnal.BornLikeThisAdmission".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.GirlWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.HermWord".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.NotMakeItSmaller".Localized());
                }

                OutputText("Scenes.Arianscene.GiveArianAnal.AppreciateYourCock".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                {
                    OutputText("Scenes.Arianscene.GiveArianAnal.PuffyDroolingLips".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.AndWord".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                {
                    OutputText("Scenes.Arianscene.GiveArianAnal.ErectingWord".Localized(
                        ArianMF("his", "her")
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.PairOfReptilianPricks".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.ReptilianPrickWord".Localized());
                }

                OutputText("Scenes.Arianscene.GiveArianAnal.SomeoneIsExcited".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.ProtestsButPushesBack".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.AdmitsWantingIt".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.JerkOffForLube".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Arianscene.GiveArianAnal.SurprisedButPleased".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.RelaxesSphincter".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.TremblesInPleasure".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.PraiseTightAndFirm".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.TeaseAboutExpertise".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.TimeFadesWhileThrusting".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Arianscene.GiveArianAnal.BaseOfYourCock".Localized());
                else
                    OutputText("Scenes.Arianscene.GiveArianAnal.BottomOfYourBalls".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.WarnWillCumInside".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.CryOutAndCum".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Arianscene.GiveArianAnal.LoadStillImpressive".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Arianscene.GiveArianAnal.DumpHugeLoad".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.HesitateAtInexperience".Localized(
                            Player.CockDescript(x)
                        ));

                OutputText("Scenes.Arianscene.GiveArianAnal.TriggersMutualOrgasm".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                    OutputText("Scenes.Arianscene.GiveArianAnal.SpewingGobsOfCum".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    OutputText("Scenes.Arianscene.GiveArianAnal.AndWordAgain".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    OutputText("Scenes.Arianscene.GiveArianAnal.WetLegsWithJuice".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.CollapseUtterlySpent".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.AsksIfWasGood".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.WonderfulWithWrySmirk".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.GladYouLikedIt".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.SatedWillConsiderIt".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnal.PatHeadTellRest".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                {
                    OutputText("Scenes.Arianscene.GiveArianAnal.DontBeTooRough".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.StrokeSwayingTail".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.RaisesBehindForAccess".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.SlickFingersWithSpit".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.LizanFemalesExplained".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) < 1)
                    {
                        OutputText("Scenes.Arianscene.GiveArianAnal.MaleBodySensitivity".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.NaturalForLizanKind".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.GiveArianAnal.GirlBodySensitivity".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.GirlNowSensitivity".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.FemalesDontHaveThese".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1)
                            OutputText("Scenes.Arianscene.GiveArianAnal.PerkyWord".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 2)
                                OutputText("Scenes.Arianscene.GiveArianAnal.RoundedWord".Localized());
                            else
                                OutputText("Scenes.Arianscene.GiveArianAnal.PillowyWord".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.MoundsWord".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.GropePresentedBosom".Localized());
                    }

                    OutputText("Scenes.Arianscene.GiveArianAnal.ProbeBetweenLegs".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                    {
                        OutputText("Scenes.Arianscene.GiveArianAnal.AWord".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                            OutputText("Scenes.Arianscene.GiveArianAnal.PairOfDrippingCocks".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.DrippingCockWord".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                            OutputText("Scenes.Arianscene.GiveArianAnal.AndWordThird".Localized());
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.SoppingWetPussy".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.GreetWord".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) == 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0) || (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) == 1 && CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 0))
                        OutputText("Scenes.Arianscene.GiveArianAnal.SLetter".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.AlreadyRaringToGo".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.BegsStopTeasing".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.PlayfulSlapOnAss".Localized());

                    if (ArianFollower())
                        OutputText("Scenes.Arianscene.GiveArianAnal.TentWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.BedchamberWord".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.FingersIntoButt".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.EnjoyFeelingSoFull".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.AskIfEnjoysIt".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) < 1)
                        OutputText("Scenes.Arianscene.GiveArianAnal.BoyPussy".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.NetherPussy".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.PleasurePainHeavenly".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.WhisperIntoEar".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.AssTightensAroundYou".Localized(
                        Player.CockDescript(x)
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                            OutputText("Scenes.Arianscene.GiveArianAnal.CockSpewsLoad".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.TwinCocksSpewLoads".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.OnTheBedsheets".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                            OutputText("Scenes.Arianscene.GiveArianAnal.AndEirWord".Localized());
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.PussyPaintsYourBody".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.MoaningAndShuddering".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.GiveInToClimax".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.BaseOfYourSpine".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.DepthsOfYourBalls".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.BlissedOutLimply".Localized());

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Arianscene.GiveArianAnal.FillInsidesWithCum".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Arianscene.GiveArianAnal.PaintButtWhite".Localized(
                                Player.CockHead(x)
                            ));
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.BuryDeepInside".Localized(
                                Player.CockDescript(x)
                            ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.AdmireHandiwork".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.BestSexEver".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.PetHeadAndWonder".Localized());

                    if (!ArianFollower())
                        OutputText("Scenes.Arianscene.GiveArianAnal.RoomWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianAnal.TentWordAgain".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.SentencePeriod".Localized());
                }
                else
                {
                    OutputText("Scenes.Arianscene.GiveArianAnal.RollsOntoKnees".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.TeaseProudMage".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.BegLikeButtslut".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.NaughtyButtslutSlap".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.BucksAgainstFingers".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.WetGashLeakingJuice".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.AndEirWordAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.ErectCockLeakingPre".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.AmazedHowFarCame".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.NoReasonToDelay".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.GaspsInSilentScream".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.PussySpillsJuices".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.AndWordFourth".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.CockShootsRopes".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.CollapsesOverwhelmed".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.SurprisedItWasReal".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.ScreamsIntoPillow".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                            OutputText("Scenes.Arianscene.GiveArianAnal.StillHardLeakingPlural".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.StillHardLeakingSingular".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                            OutputText("Scenes.Arianscene.GiveArianAnal.CommaAndEirWord".Localized());
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.SquirtingOnYourLegs".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.BegsHarderFaster".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.GiveEmWhatWants".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.PressureBeforeOrgasm".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.BegsSeedInside".Localized(
                        ArianMF("master", "mistress")
                    ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.GiveInToPleasure".Localized());

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Arianscene.GiveArianAnal.SpillLoadInBowels".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Arianscene.GiveArianAnal.DigDeepDeliverLoad".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.PressDeepForLoad".Localized(
                                Player.CockDescript(x)
                            ));

                    OutputText("Scenes.Arianscene.GiveArianAnal.TailHoldsYouStill".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.CuntBlastsLastJet".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        OutputText("Scenes.Arianscene.GiveArianAnal.CockShootsLastRope".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.AssGripsLikeVacuum".Localized());

                    OutputText("Scenes.Arianscene.GiveArianAnal.CollapseHeavingBreath".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianAssChat) == 0)
                    {
                        OutputText("Scenes.Arianscene.GiveArianAnal.DisbeliefLetHerselfGo".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.LosesControlDuringAnal".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.JustCheckingEnjoyment".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.AcceptWhoTheyAre".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.DeclareYouLikeIt".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.ConfessSurprise".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.AllNewToMe".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.NothingToChange".Localized());

                        if (9999 == 9999)
                            OutputText("Scenes.Arianscene.GiveArianAnal.HypocriticalThoughItIs".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.SexyWhenLikeThat".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.NotRightNow".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.GatherClothesAndLeave".Localized());

                        if (!ArianFollower())
                            OutputText("Scenes.Arianscene.GiveArianAnal.ReturnToTelAdre".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.OutOfTentWord".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.ArianAssChat, CoC.Instance.Flags.Get(GameFlagIds.ArianAssChat) + 1);
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.GiveArianAnal.PatAssCallButtslut".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.EnoughForAWhile".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.GreedyLizardMustWait".Localized());

                        OutputText("Scenes.Arianscene.GiveArianAnal.SmileGatherClothes".Localized());

                        if (!ArianFollower())
                            OutputText("Scenes.Arianscene.GiveArianAnal.ReturnTelAdreAgain".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianAnal.OutOfTentAgain".Localized());
                    }
                }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void GetBlownByArian()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            if (ArianMF("m", "f") == "f")
                ShowImage("arianfemale-home-getbj");
            else
                ShowImage("arianmale-home-getbj");

            OutputText("Scenes.Arianscene.GetBlownByArian.PonderWhatToDo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianHasBlown) == 0)
            {
                OutputText("Scenes.Arianscene.GetBlownByArian.LizanAvertsEyes".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
                    OutputText("Scenes.Arianscene.GetBlownByArian.ShaftSlowlyHardens".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.AsksIfReallyCan".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.AdmonishBeCareful".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.ArianNodsEagerly".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.GetBlownByArian.SmilesAndLicksLips".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.AcknowledgesConcern".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.TrustAndSettleBed".Localized());
            }

            OutputText("Scenes.Arianscene.GetBlownByArian.RollsOnTop".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.MouthWarmerThanExpected".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.CoaxesPreOut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianHasBlown) == 0)
            {
                OutputText("Scenes.Arianscene.GetBlownByArian.WondersAtSkill".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.AsksIfVirgin".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.KissesCockheadReply".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.BlurtsAboutSelf".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.NervousMageExplanation".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.AgreesMoreImportant".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.GetBlownByArian.AsksIfPracticing".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.DeniesNeedPractice".Localized());

                OutputText("Scenes.Arianscene.GetBlownByArian.PraisesGreatHead".Localized());
            }

            OutputText("Scenes.Arianscene.GetBlownByArian.ThrustDeepIntoMouth".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.BracesWithMouthful".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.HumpsFaceHard".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.NeverSlowsDown".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.OrgasmRipsThrough".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.SurprisedByFirstJet".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Arianscene.GetBlownByArian.SmallWord".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Arianscene.GetBlownByArian.BigWord".Localized());
                else
                    OutputText("Scenes.Arianscene.GetBlownByArian.HugeWord".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.LetsGoWithSigh".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.LooksFullRemark".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Arianscene.GetBlownByArian.AmazedDidNotPop".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.AsksIfAllRight".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.YawnsToothyMaw".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Arianscene.GetBlownByArian.BellyLikePregnant".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.HelpsBackToBed".Localized());

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Arianscene.GetBlownByArian.GutAudiblySloshing".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.GetBlownByArian.ThanksFallsAsleep".Localized());

            if (!ArianFollower())
                OutputText("Scenes.Arianscene.GetBlownByArian.NotifyLaikaAndBoon".Localized());
            else
                OutputText("Scenes.Arianscene.GetBlownByArian.LeavesArianTent".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.ArianHasBlown, CoC.Instance.Flags.Get(GameFlagIds.ArianHasBlown) + 1);

            ReturnToCamp(1);
        }

        public void PenetrateArian()
        {
            ClearOutput();

            ArianSprite();

            var x = Player.CockThatFits(50);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            ShowImage("arianfemale-home-penetrate");

            OutputText("Scenes.Arianscene.PenetrateArian.AdmiresLizanBody".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                OutputText("Scenes.Arianscene.PenetrateArian.Maleherm".Localized(
                    ArianMF("maleherm.", "dickgirl.")
                ));
            else
                OutputText("Scenes.Arianscene.PenetrateArian.GirlWord".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.SmilesAvertsEyes".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.InsistsAndAsksHow".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.TailWavingLazily".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Arianscene.PenetrateArian.SLetter".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.NervousReadierAsk".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.PromisesToBeGentle".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.LustDropsOnLegs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
            {
                OutputText("Scenes.Arianscene.PenetrateArian.ExposedCockWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.PenetrateArian.SLetterAgain".Localized());

                OutputText("Scenes.Arianscene.PenetrateArian.HardeningAtProspect".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                {
                    OutputText("Scenes.Arianscene.PenetrateArian.BulbousCockWord".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.PenetrateArian.PeekingOutPlural".Localized());
                    else
                        OutputText("Scenes.Arianscene.PenetrateArian.PeekingOutSingular".Localized());

                    OutputText("Scenes.Arianscene.PenetrateArian.AlreadyFullMast".Localized());
                }

            OutputText("Scenes.Arianscene.PenetrateArian.SwallowsAsksWhatNow".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.GripsHipsToConnect".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.ImpalesSelfToSpeed".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.PatsCheekTakeEasy".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 0)
                OutputText("Scenes.Arianscene.PenetrateArian.FemaleWord".Localized());
            else
                OutputText("Scenes.Arianscene.PenetrateArian.HermWord".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.HiltedInsideLizan".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.RubsStretchedLips".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.BestYetToCome".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.GyratesHipsMoaning".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.PenetrateArian.ArianEirPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.PenetrateArian.TwinShaftsWord".Localized());
                else
                    OutputText("Scenes.Arianscene.PenetrateArian.ShaftWord".Localized());

                OutputText("Scenes.Arianscene.PenetrateArian.BobbingOutOfSight".Localized());
            }

            OutputText("Scenes.Arianscene.PenetrateArian.AdjustingToNewGender".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.SmilesDownLicking".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.CaressesSmallBosom".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.GrindsHipsHappy".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.PromisesButHelpless".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.PenetrateArian.EirCockWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.PenetrateArian.TwitchSpurtPlural".Localized());
                else
                    OutputText("Scenes.Arianscene.PenetrateArian.TwitchSpurtSingular".Localized());

                OutputText("Scenes.Arianscene.PenetrateArian.SmallRopeOfPre".Localized());
            }

            OutputText("Scenes.Arianscene.PenetrateArian.ReallyStartPounding".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.DirtyMouthJoke".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.WordsCatchInThroat".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.RudeToCumFirst".Localized(
                ArianMF("maleherm", "lady")
            ));

            OutputText("Scenes.Arianscene.PenetrateArian.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.PenetrateArian.AriansPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.PenetrateArian.CocksThrobPlural".Localized());
                else
                    OutputText("Scenes.Arianscene.PenetrateArian.CockThrobsSingular".Localized());

                OutputText("Scenes.Arianscene.PenetrateArian.SplashesOwnSeed".Localized());
            }

            OutputText("Scenes.Arianscene.PenetrateArian.WallsGripTightly".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.Balls == 0)
                OutputText("Scenes.Arianscene.PenetrateArian.YouWord".Localized());
            else
                OutputText("Scenes.Arianscene.PenetrateArian.YourBallsWord".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.SentencePeriodAgain".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.FinallyKeptPromise".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Arianscene.PenetrateArian.FluidsSpillDown".Localized());
            else
                OutputText("Scenes.Arianscene.PenetrateArian.YLetter".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.UnleashEssenceIntoWomb".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Arianscene.PenetrateArian.PussyClampsHard".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Arianscene.PenetrateArian.StretchWetPussy".Localized());
                else
                    OutputText("Scenes.Arianscene.PenetrateArian.ThreatensToBlowOff".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Arianscene.PenetrateArian.SighsSexFeelsGood".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Arianscene.PenetrateArian.LeanWord".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Arianscene.PenetrateArian.FullWord".Localized());
                else
                    OutputText("Scenes.Arianscene.PenetrateArian.OverfilledWord".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.StayALittleLonger".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.TakesFreeHand".Localized());

            OutputText("Scenes.Arianscene.PenetrateArian.AnnouncesMustGo".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void GetButtWreckedByArian()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                ShowImage("arianfemale-home-getArianAnal");
            else
                ShowImage("arianmale-home-getArianAnal");

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.LicksLipsAsksAnal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.WorriesAboutHurt".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.WillingToTopYou".Localized());
                else
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.PrefersBeingFilled".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.SeductiveNodConfirm".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.AsksHowToProceed".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.SmirksAndWhispers".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.CompliesFallsBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) >= 3)
            {
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.ExposedCockWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.HardenPlural".Localized());
                else
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.HardenSingular".Localized());

                OutputText("Scenes.Arianscene.GetButtWreckedByArian.FullMastWord".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.CockWord".Localized(
                    ArianMF("his", "her")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.PeekOutPlural".Localized());
                else
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.PeekOutSingular".Localized());

                OutputText("Scenes.Arianscene.GetButtWreckedByArian.RisingToFullMast".Localized());
            }

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.SashaysAndStraddles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.PrimaryWord".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.StrokesKnobblyCock".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.WarnsCannotHoldBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.SLetter".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.ThrobbingAgainstHands".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.ShameButMakeDo".Localized());

            Player.ButtChange(ArianCockSize(), true, true, false);

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.ShootsIntoBowels".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
            {
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.OtherShaftSprays".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.BackWord".Localized());
                else
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.TailWord".Localized());

                OutputText("Scenes.Arianscene.GetButtWreckedByArian.SentencePeriod".Localized());
            }

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.StaminaNeedsWork".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.SorryCantBeHelped".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.PantingHandsRoam".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.BendsOverToKiss".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.SlidesBetweenCheeks".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.CaressesTailBase".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) > 66)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.EagerWord".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.ProbesForProstate".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.InstantReactionMoan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.PairOfCocksJump".Localized());
            else
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.SingleCockJumps".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.BackToFullMast".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.MakesUpForQuickness".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.BitesLipNervously".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.RisesAndFalls".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.SlidesPrickThroughAss".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.ClenchesInnerWalls".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.BarelyContainsHimself".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.DrippingWithArousal".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.AndWord".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.CockWordAgain".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.SLetterAgain".Localized());

                OutputText("Scenes.Arianscene.GetButtWreckedByArian.DroolsPrecumOnBelly".Localized());
            }

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.BegsCumTogether".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.GaspsOnCue".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.TwitchPlural".Localized());
            else
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.TwitchSingular".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.GrabsSheetsOrgasm".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.YourButtToo".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.ArianReachesClimax".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.PussyClenchesJuices".Localized());
            else
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.SentencePeriodAgain".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.CryOfGleeOrgasm".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.RopeAfterRope".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.UnattendedPussyLeaks".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.GoesLimpOnBed".Localized());

            OutputText("Scenes.Arianscene.GetButtWreckedByArian.PleasedGrinPraise".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
            {
                OutputText("Scenes.Arianscene.GetButtWreckedByArian.HopesItFeltGood".Localized());

                OutputText("Scenes.Arianscene.GetButtWreckedByArian.PatsLoverAssures".Localized());

                OutputText("Scenes.Arianscene.GetButtWreckedByArian.NuzzlesInAffection".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                {
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.HopesYouFeelGood".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.InsistsButWarns".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.BothWaysFeelGood".Localized());
                }
                else
                {
                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.AsksReturnFavor".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.CallsGreedyButtslut".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.GrinningNervouslyAdmits".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.WantsToBeCatcher".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.ContemplatesTheIdea".Localized());

                    if (Player.HasCock())
                    {
                        Player.Orgasm("Dick");

                        Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

                        Player.SlimeFeed();

                        ClearButtons();

                        if (Player.HasCockThatFits(50))
                            AddButton(0, "Common.Button.Yes".Localized(), GiveArianAnal);
                        else
                            OutputText("Scenes.Arianscene.GetButtWreckedByArian.TooBigToFit".Localized());

                        AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));

                        return ;
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.GetButtWreckedByArian.ChucklesNotEquipped".Localized());

                        OutputText("Scenes.Arianscene.GetButtWreckedByArian.WhimpersDisappointed".Localized());
                    }

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.YawnsAsksToJoin".Localized());

                    OutputText("Scenes.Arianscene.GetButtWreckedByArian.KissOnBrowPromise".Localized());
                }

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            ReturnToCamp(1);
        }

        public void SuckAriansDick()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                ShowImage("arianfemale-home-suckariandick");
            else
                ShowImage("arianmale-home-suckariandick");

            OutputText("Scenes.Arianscene.SuckAriansDick.LickLipsAskTaste".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.OffersReciprocate".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.InsistOnGiving".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.AsksWhatToDo".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.InstructGetOnBed".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.AgreesNervously".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 3)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.SuckAriansDick.TwinCocksHarden".Localized());
                else
                    OutputText("Scenes.Arianscene.SuckAriansDick.SingleCockHarden".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.SuckAriansDick.CockWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.SuckAriansDick.PluralSPrefix".Localized());
                else
                    OutputText("Scenes.Arianscene.SuckAriansDick.ItsEmergeClause".Localized());

                OutputText("Scenes.Arianscene.SuckAriansDick.HiddenFullyErect".Localized());

                OutputText("Scenes.Arianscene.SuckAriansDick.SentencePeriod".Localized());
            }

            OutputText("Scenes.Arianscene.SuckAriansDick.ContemplateHowToBegin".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.UppermostWord".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.MarvelStrangeTexture".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.MoansHumpsHand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.TwinWord".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.ShaftWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.PluralTremblePrefix".Localized());
            else
                OutputText("Scenes.Arianscene.SuckAriansDick.TremblesWord".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.AndThrobClause".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.SLetter".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.PreQuicklyForming".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.SLetterAgain".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.SlideDownBulbousSurface".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.SLetterThird".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.ContinueSlideAskFantasy".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.PantsFeetCurling".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.InnocentSmileTease".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.SLetterFourth".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.TasteSugarySpice".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.ContortsGoingToCum".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.WouldTeaseNoStamina".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.HandsPullSheets".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.TwinCocksThrob".Localized());
            else
                OutputText("Scenes.Arianscene.SuckAriansDick.CockThrobs".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.EruptsIntoMouth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.AndChinClause".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.LegsLockYourBack".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.GuzzleEveryDrop".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.SecondCockSpews".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.PeriodSpace".Localized());

            Player.RefillHunger(15);

            OutputText("Scenes.Arianscene.SuckAriansDick.LickSuckleAfterward".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.SlackJawedPraise".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.ConfessTasteUnusual".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.ReturnTheFavor".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Arianscene.SuckAriansDick.PromiseThinkOfWay".Localized());
            else
                OutputText("Scenes.Arianscene.SuckAriansDick.TeaseCreamyLolly".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.RubsNeckTail".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.BitesLipsTwirlsFingers".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.JokeOnlyReasonComp".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.PatCheekAdorable".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                OutputText("Scenes.Arianscene.SuckAriansDick.TickleCuntFinger".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.KissNoseDepart".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.AwkwardSeeYouSoon".Localized());

            OutputText("Scenes.Arianscene.SuckAriansDick.SmirkShakeAssLeave".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 5)));

            ReturnToCamp(1);
        }

        public void GetPenetratedByArianAndHisHitlerMustache()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                ShowImage("arianfemale-home-getpenetrated");
            else
                ShowImage("arianmale-home-getpenetrated");

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.LookAtSlit".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) < 3)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.WhatItHides".Localized());
            else
            {
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ExposedCockWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetter".Localized());
            }

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.AskBothCocksWorkout".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TwinCocksWord".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.CockWord".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.WorkoutWord".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SwallowsNodsConsent".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.MotionGetUp".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SpreadLegsPrefix".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ExposeVaginaView".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) >= 3)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ExposedWord".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.HiddenWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TwinShaftsFill".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ShaftFills".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.GrowsErectIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TheirWord".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ItsWord".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.BulbousThrobbingGlory".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.BodySeemsWilling".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetterAgain".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ReadyToThrust".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SnapsToSenses".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.KneelsBetweenLegs".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.AlignsTip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.EachOfPrefix".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.PlaceholderSpace".Localized(
                ArianMF("his", "her")
            ));

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ShaftWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetterThird".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.WithVaginaPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.AndAssholePrefix".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.WaitsForConfirm".Localized());

            if (Player.HasLongTail())
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TailAroundWaist".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SmilesBeginsEasing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
            {
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TriesFitBoth".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.OrbsLikeBeads".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.FineJustDifferent".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.FumblesMoreGusto".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.PullUpEmbrace".Localized());

                Player.CuntChange(ArianCockSize(), true, true, false);

                Player.ButtChange(ArianCockSize(), true, true, false);

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.MoansInShock".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.NervousBeginnerThrust".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.MoanSighImpatient".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.DowncastNotUsed".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.GuideHandsAround".Localized());

                if (Player.HasLongTail())
                    OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TightenTailEncourage".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SnugglesBreastToBreast".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.BecauseYoureCute".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SqueakOfShock".Localized());

                Player.CuntChange(ArianCockSize(), true, true, false);

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.GaspHugCloser".Localized());

                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.HardAllTheSame".Localized());
            }

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TapNoseTell".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetterFourth".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.RemainInsideClause".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.AwkwardClumsyThrust".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.BlinksAsksHurt".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ReassureGuideThrough".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.MoansShiversCatchesOn".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.FeelsJustAsGood".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TwinShaftsFeel".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ShaftFeels".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ExquisiteAndPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TheirWordAgain".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ItsWordAgain".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.BumpsMassageEntrance".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetterFifth".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.PraiseQuickStudy".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ClenchesGaspsArches".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.AndAssPrefix".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.StickyWetSeed".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.MoanFillsWithSeed".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.GrowlsSuddenFlip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetterSixth".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SwellGrabsShoulders".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.GroansBraces".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.RedoublesKissThanks".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.CryOfRelief".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) == 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.HimselfWord".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.EyPrefix".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.OverwhelmedBySpray".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.CavityWord".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.CavitiesWord".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SecondHelpingSpunk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.AndAssWord".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ContractingTryingPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.ItsWordThird".Localized());
            else
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.TheirWordThird".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.MilkThePoorLizan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.SLetterSeventh".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.EmbeddedJuicesMat".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.PantsWeakSmile".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.WhyThisSpell".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.DropSubjectLeave".Localized());

            OutputText("Scenes.Arianscene.GetPenetratedByArianAndHisHitlerMustache.FinalKissGoodbye".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void DoublePenetrateArian()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            ShowImage("arianfemale-home-doublepenetrate");

            var x = Player.CockThatFits(CoC.Instance.Flags.Get(GameFlagIds.ArianCapacity));

            var y = Player.CockThatFits2(CoC.Instance.Flags.Get(GameFlagIds.ArianCapacity));

            OutputText("Scenes.Arianscene.DoublePenetrateArian.LizardCocksStir".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.BitesLipFidget".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.RollsFaceDown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) <= 33)
                OutputText("Scenes.Arianscene.DoublePenetrateArian.TightWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) <= 66)
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.LooseWord".Localized());
                else
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.ThrobbingWord".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.AssAndWetVagina".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.SidleSqueezeCheeks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
            {
                OutputText("Scenes.Arianscene.DoublePenetrateArian.FingerPussyLube".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.TightAssWarning".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.AssureGentleBoth".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.PainTurnsMoan".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                {
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.LubeForPoliteness".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.FeelsLikeGirl".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.OfCourseGirl".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.AlignAndPress".Localized());
                }
                else
                {
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.MassagePussyPoliteness".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.ShuddersPushesBack".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.BothHolesAnxious".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.PositionAndPush".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.ProtestFaster".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.BothHolesTenderness".Localized());
                }

            OutputText("Scenes.Arianscene.DoublePenetrateArian.SqueezeAsscheeksAsk".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.SlapPerkyButt".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.PenetratingBothHoles".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Arianscene.DoublePenetrateArian.FeelsSoFull".Localized());

            OutputText("Scenes.Arianscene.DoublePenetrateArian.SnapPaintWombWhite".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Arianscene.DoublePenetrateArian.SurprisedByHerAct".Localized());

            if (Player.CumQ() <= 250)
            {
                OutputText("Scenes.Arianscene.DoublePenetrateArian.CocksExplodeInside".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.DemandsMoreSpent".Localized());
            }
            else
                if (Player.CumQ() <= 1000)
                {
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.LustFloodsInsides".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Arianscene.DoublePenetrateArian.ThemselvesWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.DoublePenetrateArian.YourBallsPrefix".Localized());

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.LoadSheRelishes".Localized(
                        Player.CockDescript(y),

                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.PleadsBellyDistends".Localized());
                }
                else
                {
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.CumForceClamp".Localized(
                        Player.CockDescript(y)
                    ));

                    OutputText("Scenes.Arianscene.DoublePenetrateArian.BellyBulgingEggs".Localized());
                }

            OutputText("Scenes.Arianscene.DoublePenetrateArian.AssGoesSlack".Localized(
                Player.CockDescript(y)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoublePenetrationChat) == 0)
            {
                OutputText("Scenes.Arianscene.DoublePenetrateArian.PatAssRemark".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.EyesSnapOpen".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.LaughNotInControl".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.Apology".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.NoNeedApologize".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.NoClutchDisappointed".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.WhySoDisappointed".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.CoversFaceGirly".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.AskEnjoyedHerself".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.GrinsVeryMuch".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.WantsDoAgain".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.RememberRedressLeave".Localized());

                if (!ArianFollower())
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.BedchambersWord".Localized());
                else
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.TentWord".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.SentencePeriod".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.DoublePenetrateArian.PatAssSurprise".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.LazyOpensRubsBelly".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.FunnyToSeeChange".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.NoTroublePatHead".Localized());

                if (!ArianFollower())
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.BedchambersWordAgain".Localized());
                else
                    OutputText("Scenes.Arianscene.DoublePenetrateArian.TentWordAgain".Localized());

                OutputText("Scenes.Arianscene.DoublePenetrateArian.SentencePeriodAgain".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void ArianDocking()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                ShowImage("arianfemale-home-docking");
            else
                ShowImage("arianmale-home-docking");

            OutputText("Scenes.Arianscene.ArianDocking.EyeSlitAskDock".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.ShuddersSoundsWeird".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.KinkyWantToTry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                OutputText("Scenes.Arianscene.ArianDocking.TwoDicksCuriosity".Localized());
            else
                OutputText("Scenes.Arianscene.ArianDocking.ExtraCockNoDiff".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.AgreesSpreadsLegs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.PairOfPrefix".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.PeckerWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.SLetter".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.PryingEyesClause".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.PushHandsFeelFolds".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.SLetterAgain".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.OfAriansDickPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.SLetterThird".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.BrushFingertip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.SLetterFourth".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.GaugeReaction".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.GroansWontHold".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.CryingShameFinish".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.FinishWhatStarted".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.TightWarmerThanRest".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                OutputText("Scenes.Arianscene.ArianDocking.RubsOwnPecker".Localized());
            else
                OutputText("Scenes.Arianscene.ArianDocking.SandwichedBetweenDicks".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.FrictionSparksPleasure".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.HardToReadDiscomfort".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.ShiftArmsToWaist".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.TwinCocksWord".Localized());
            else
                OutputText("Scenes.Arianscene.ArianDocking.CockWord".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.SheatheWallsGrip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.SLetterFifth".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.MassagesAlmostVagina".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.GaspComeFirst".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Arianscene.ArianDocking.LittleRoomInside".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Arianscene.ArianDocking.LoadPotbellyShallow".Localized());
                else
                    OutputText("Scenes.Arianscene.ArianDocking.EvenGoblinsCantHold".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.SlumpPleadingLook".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.AskWhatsWrong".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.SLetterSixth".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.OutBeforeCummingClause".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.GroansWhiteEruption".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.CummyMastsEmerge".Localized());
            else
                OutputText("Scenes.Arianscene.ArianDocking.CummyMastEmerges".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.FeeblyShootRopes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                OutputText("Scenes.Arianscene.ArianDocking.TheirWord".Localized());
            else
                OutputText("Scenes.Arianscene.ArianDocking.ItsWord".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.AllArianSays".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.PatShoulderWickedGrin".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.GoodButWeird".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.DeepKissUntilNeed".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.WinningGrinWonderWhy".Localized());

            OutputText("Scenes.Arianscene.ArianDocking.OtherThingsInFavor".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            ReturnToCamp(1);
        }

        public void GiveArianAnItem()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.GiveArianAnItem.AskTakeSomething".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) < 1)
            {
                OutputText("Scenes.Arianscene.GiveArianAnItem.RatherNotDrink".Localized());

                OutputText("Scenes.Arianscene.GiveArianAnItem.NodChangeSubject".Localized());

                ArianHomeMenu();

                return ;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianHealth) < 20)
                {
                    OutputText("Scenes.Arianscene.GiveArianAnItem.IsItMedicine".Localized());

                    if (!Player.HasItem(ItemRegistry.Create(ConsumableIds.VitalityTincture)))
                    {
                        OutputText("Scenes.Arianscene.GiveArianAnItem.NoMedicineAdmit".Localized());

                        ArianHomeMenu();

                        return ;
                    }
                }
                else
                    OutputText("Scenes.Arianscene.GiveArianAnItem.WouldntHarmYou".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 0 && ArianHealth() >= 10)
                ArianStoryDialogue1();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 1 && ArianHealth() >= 20)
                    ArianStoryDialogue2();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 2 && ArianHealth() >= 30)
                        ArianDialogue3();
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 3 && ArianHealth() >= 50)
                            ArianImbue();
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 4 && ArianHealth() >= 75)
                                ArianPlot4();
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianSDialogue) == 5 && ArianHealth() >= 100)
                                    ArianPlot5();
                                else
                                {
                                    AddButton(0, ItemRegistry.Create(ConsumableIds.VitalityTincture).Name, ArianVitalityTincture);

                                    AddButton(1, ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft).Name, GiveIncubusDraftToArian);

                                    AddButton(2, ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk).Name, SuccubiMilkForArian);

                                    AddButton(3, ItemRegistry.Create(ConsumableIds.Lactaid).Name, GiveArianLactaid);

                                    AddButton(4, ItemRegistry.Create(ItemIds.Reducto).Name, GiveArianReducto);

                                    AddButton(5, ItemRegistry.Create(ConsumableIds.Reptlum).Name, GiveArianReptilum);

                                    AddButton(14, "Common.Button.Back".Localized(), ArianHomeMenu);
                                }
        }

        public void ArianVitalityTincture()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianVitalityTincture.OfferGiacomoVial".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.ThanksGratefully".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.NoTroubleSleazyPeddler".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.UncorkAndChug".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.SmellsLikeCherries".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.HatesCherries".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.NastyButWorks".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.FeelsBetterThanks".Localized());

            OutputText("Scenes.Arianscene.ArianVitalityTincture.StrokeHeadWelcome".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.VitalityTincture));

            ArianHealth(10);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GiveArianAnItem);
        }

        public void GiveIncubusDraftToArian()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.OfferIncubusDraft".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.BiggerCockOffer".Localized());
            else
                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.OldMalehoodBack".Localized());

            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.HappyToTake".Localized());

            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.HoldOutEagerly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) + 1);

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ChangesStartArousal".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TeaseControlLoss".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.VaginaAndPrefix".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.AssholePeriod".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ThrobsAnotherJet".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.WondersJoinBed".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) < 3)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) + 1);

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ChangesStartAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ShaftEmergesGrows".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TwoShaftsEmerge".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.NewSizeMoansCums".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.FlickCumAskLike".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.PluralBackPrefix".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.BackIntoItsClause".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SlitWord".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 2)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) + 1);

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TucksInImpression".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TightFitStillInside".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.AskUpCloseShow".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.PatHeadComeBack".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ManagesRetractPrefix".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ThemWord".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ItWord".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TwoThirdsInside".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.PantsTipsCantHide".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.PantsTipHangsOut".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TweakExposedTip".Localized(
                            ArianMF("boy-toy", "herm-toy")
                        ));

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.AskBoonForOne".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.FetchOneNowQuestion".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.RefuseBoonFindOut".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SLetter".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SentencePeriodAgain".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        {
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.QuirkBrowCompare".Localized());

                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TapNoseDifference".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SLetterAgain".Localized());

                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.HardeningCantHide".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.AdmitEmbarrassBoon".Localized());

                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.NotSecretlyHappy".Localized());

                            OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SexyGiftPoint".Localized());
                        }

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.KissCheekReconsider".Localized());

                        OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.TeasedEnoughCalmDown".Localized());
                    }
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) > 1 && (!hyper))
            {
                CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) - 1);

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ChangesSweepChest".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ChestNotHeavy".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.CanLiveWithIt".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.HatchedBoyConfused".Localized());

                OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.LaikaUlteriorMotive".Localized());

                ArianSexMenu(false);

                return ;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1 && (!hyper))
                {
                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ChangesSweepAgain".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.MoansAnotherChange".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.VoiceMoreMasculine".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.ArianVagina, 0);

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.MissBeingHalfGirl".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.PrettyThenHandsome".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.AvertsEyesLaying".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ReallySayAfter".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.DidntHurtNice".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ClapShoulderVagina".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.AvertsEyesOfCourse".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.WhatYouThinkLeave".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.CockBeginsHardening".Localized());
                }
                else
                {
                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.GettingTurnedOn".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.ThisIsMyLimit".Localized());

                    OutputText("Scenes.Arianscene.GiveIncubusDraftToArian.SeemsLimitDoElse".Localized());
                }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GiveArianAnItem);
        }

        public void SuccubiMilkForArian()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

            OutputText("Scenes.Arianscene.SuccubiMilkForArian.OfferDemonMilk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                OutputText("Scenes.Arianscene.SuccubiMilkForArian.FurtherWord".Localized());

            OutputText("Scenes.Arianscene.SuccubiMilkForArian.FeminineSideForYou".Localized());

            OutputText("Scenes.Arianscene.SuccubiMilkForArian.BitesLipPassBottle".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ArianVagina, CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) + 1);

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.ThroatyGirlyMoan".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.DragEyesAskOkay".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.GetColdDrink".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.SweepsJugDowns".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.SuccubiMilkForArian.SLetter".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.AlreadySoftened".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
                    OutputText("Scenes.Arianscene.SuccubiMilkForArian.AsFarAsGoes".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.IntoProtectiveSlit".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.AskFeelingOkay".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.NoteLooksFine".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.CoughNotMinding".Localized());

                OutputText("Scenes.Arianscene.SuccubiMilkForArian.RunHandsOverBreasts".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) < 3)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) + 1);

                    OutputText("Scenes.Arianscene.SuccubiMilkForArian.WarmthSettlesMounds".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 2)
                    {
                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.BreastsInflateSoft".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.GropesWeightSize".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.MassageBreastsFeelsGood".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.AskTurnUpNotch".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) >= 3)
                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.ShaftGrowsHard".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.SentencePeriod".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.MoundsInflateOnceMore".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.BustiestLizanSeen".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.HeavyButKeepThem".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.ShaftRisingPoint".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.SentencePeriodAgain".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.CupBosomWilling".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.BlushesWouldLove".Localized());
                    }
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 1 && (!hyper))
                    {
                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.WarmthMoansObvious".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.FeminizerShrinking".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.SLetterAgain".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.NervousGoodRight".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.ThanksPreferGirl".Localized());

                        OutputText("Scenes.Arianscene.SuccubiMilkForArian.ComfortableGirlySide".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 1 && CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) >= 1 && (!hyper))
                        {
                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.WarmthFlowAgain".Localized());

                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.HighFemininitySuggest".Localized());

                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.MeetGazeLevelly".Localized());

                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.InsistsFineReshaping".Localized());

                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.ThinkYoureBeautiful".Localized());

                            OutputText("Scenes.Arianscene.SuccubiMilkForArian.ShowMe".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.ArianDoubleCock, 0);
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 1 && (!hyper))
                            {
                                CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, 0);

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.WarmthFlowThird".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.AgreeTakingCalmly".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.ThinkImPretty".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.PrettyGirlYes".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.AllINeedSmile".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.LookSteadilySure".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.FineReallyInsist".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.ProperQuestionHow".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.CouldDoSomething".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.WarmthSuddenlyStops".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.GrowPenisBack".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.FineUnlessYouWant".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.ThinkAboutItNow".Localized());

                                OutputText("Scenes.Arianscene.SuccubiMilkForArian.CouldGoForSpin".Localized());
                            }

            ArianSexMenu(false);
        }

        public void GiveArianLactaid()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));

            OutputText("Scenes.Arianscene.GiveArianLactaid.LactaidVialHesitate".Localized());

            OutputText("Scenes.Arianscene.GiveArianLactaid.LizansDontLactate".Localized());

            OutputText("Scenes.Arianscene.GiveArianLactaid.WorstFailsHandOver".Localized());

            OutputText("Scenes.Arianscene.GiveArianLactaid.UncorkDownsMilky".Localized());

            OutputText("Scenes.Arianscene.GiveArianLactaid.AwkwardWaitSilence".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 0)
            {
                OutputText("Scenes.Arianscene.GiveArianLactaid.SighsOpensRobes".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.StrangeWonderShrug".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.SorryDidntWork".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), GiveArianAnItem);
            }
            else
            {
                Player.ChangeFatigue(-15);

                Player.HealthChange(Player.MaxHealth * .2f);

                OutputText("Scenes.Arianscene.GiveArianLactaid.PantingFeverish".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.ApproachAskOkay".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.BreastsTicklish".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
                    OutputText("Scenes.Arianscene.GiveArianLactaid.CockGrowsFlush".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.GripsSheetsWatch".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1)
                    OutputText("Scenes.Arianscene.GiveArianLactaid.PerkyWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 2)
                        OutputText("Scenes.Arianscene.GiveArianLactaid.LusciousWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.GiveArianLactaid.PillowyWord".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.BreastsGrowPleads".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.WryCommentMassage".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.GreatItWorked".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.TeaseThenSqueeze".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.MoansCaressWeird".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.DeepSuckleRelease".Localized(
                    ArianMF("his", "hers")
                ));

                OutputText("Scenes.Arianscene.GiveArianLactaid.DrainAllMilk".Localized());

                Player.RefillHunger(10 + (10 * CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts)));

                OutputText("Scenes.Arianscene.GiveArianLactaid.FeltNiceAgain".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.NeedAnotherVial".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) < 3)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) + 1);

                    OutputText("Scenes.Arianscene.GiveArianLactaid.AreTheyBigger".Localized());

                    OutputText("Scenes.Arianscene.GiveArianLactaid.ConfirmBiggerGuess".Localized());
                }

                OutputText("Scenes.Arianscene.GiveArianLactaid.RestSuggestYouToo".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.OkayShowOut".Localized());

                OutputText("Scenes.Arianscene.GiveArianLactaid.LeaveQuietlyPlan".Localized());

                ReturnToCamp(1);
            }
        }

        public void GiveArianReducto()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.GiveArianReducto.HandOverReducto".Localized());

            OutputText("Scenes.Arianscene.GiveArianReducto.WhichPartQuestion".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) > 0)
                AddButton(0, "Common.Button.Breasts".Localized(), UseReductoOnAriansBreasts);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                AddButton(1, "Common.Button.Cock".Localized(), UseReductoOnArianCocks);

            AddButton(2, "Scenes.PlayerAppearance.Label.Asshole".Localized(), UseReductoOnAriansAsshole);

            AddButton(14, "Common.Button.Back".Localized(), () => GiveArianAnItem());
        }

        public void UseReductoOnAriansBreasts()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ItemIds.Reducto);

            OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.PointAtBreastsPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1)
                OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.SmallWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 2)
                    OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.GenerousWord".Localized());
                else
                    OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.PillowyWord".Localized());

            OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.LoseWeightSpareBack".Localized());

            OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.KneadLatherPaste".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) > 1 || (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1 && Random.Shared.Next(6) == 0))
            {
                OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.BreastsSlowlyShrink".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.BetterNowQuestion".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.ArianBreasts, CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) - 1);
            }
            else
            {
                OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.NothingChangingFeel".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnAriansBreasts.FineFindAnother".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GiveArianAnItem);
        }

        public void UseReductoOnArianCocks()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ItemIds.Reducto);

            OutputText("Scenes.Arianscene.UseReductoOnArianCocks.PointCrotchWantSmaller".Localized());

            OutputText("Scenes.Arianscene.UseReductoOnArianCocks.OpensRobesSqueezeTube".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 3)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) - 1);

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.TeasesSlowlyPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.TwinLizardCocks".Localized());
                else
                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.SingleLizardCock".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.FullErectionApplyPaste".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.OversizedDickGroans".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.SLetter".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.SlowlyShrinkClause".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0)
                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.SLetterAgain".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.ManageableSizeRetract".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.PluralBackPrefix".Localized());
                else
                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.BackIntoItsClause".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.TightButFits".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnArianCocks.MoreNaturalComfortable".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 2)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ArianCockSize, CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) - 1);

                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.TeasesSlitCoax".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.PluralFromSheathPrefix".Localized());
                    else
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.FromItsSheathClause".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.AndErectionApplyReducto".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.MereMomentsDiscomfort".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.PluralReturnPrefix".Localized());
                    else
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.ReturnsToItsClause".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.OriginalSizeRetracts".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.ThemWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.ItWord".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.BackIntoGenitalSlit".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnArianCocks.AverageSizedDisappointed".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    {
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.CoaxesTwinPeckers".Localized());

                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.ShortWhileGroans".Localized());

                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.SingleCockRegret".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.CoaxesSnakeDick".Localized());

                        OutputText("Scenes.Arianscene.UseReductoOnArianCocks.NoChangeSpeaks".Localized());
                    }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GiveArianAnItem);
        }

        public void UseReductoOnAriansAsshole()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ItemIds.Reducto);

            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.HandTubeBackAss".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) == 0)
                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.ManWord".Localized());
                else
                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.HermWord".Localized());
            }
            else
                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.WomanWord".Localized());

            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.AllowsSmearCream".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) <= 1)
            {
                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.CantGetTighter".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.PokeProdConcede".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.NoUseChuckle".Localized());

                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.NotWrongDoElse".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
                {
                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.StillTightGoAhead".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.PressTightResists".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.GroansPullOut".Localized());

                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.PromiseTakeCare".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.ArianAnalXp, 1);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.ArianAnalXp, CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) - 33);

                        OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.UsedToFeeling".Localized());

                        OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.TwoFingersStretched".Localized());

                        OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.MoansGettingUsed".Localized());

                        OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.ThanksCatering".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) <= 100)
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.ArianAnalXp, CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) - 33);

                            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.WontFeelAsGood".Localized());

                            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.PatHeadSlut".Localized());

                            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.WholeHandSlide".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                            {
                                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.CockWord".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.PluralJoltPrefix".Localized());
                                else
                                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.JoltFromSheathe".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.AndClauseWord".Localized());
                            }

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 0)
                                    OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.CommaSpace".Localized());

                                OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.PussyWetsStimulation".Localized());
                            }

                            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.SlapButtPumpBalance".Localized());

                            OutputText("Scenes.Arianscene.UseReductoOnAriansAsshole.LessSensitiveStillGood".Localized());
                        }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GiveArianAnItem);
        }

        public void GiveArianReptilum()
        {
            ClearOutput();

            ArianSprite();

            Player.ConsumeItem(ConsumableIds.Reptlum);

            OutputText("Scenes.Arianscene.GiveArianReptilum.FingerVialInstructBlindfold".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.CompliesAsksWhatFor".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.CluckSurpriseSpoiled".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.HopesNotPrank".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.AssureWontPeek".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.TrustMePromise".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.MoreFunPopOpen".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.ProtestsCutShort".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.WaitToFindOut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianFirstReptilum) == 0)
            {
                OutputText("Scenes.Arianscene.GiveArianReptilum.NothingHappensAsk".Localized());

                OutputText("Scenes.Arianscene.GiveArianReptilum.FeelFine".Localized());

                OutputText("Scenes.Arianscene.GiveArianReptilum.WeirdBustQuestion".Localized());
            }
            else
                OutputText("Scenes.Arianscene.GiveArianReptilum.ChuckleAnyTimeNow".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.ShakesHeadAura".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianFirstReptilum, CoC.Instance.Flags.Get(GameFlagIds.ArianFirstReptilum) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianFirstReptilum) == 1)
                OutputText("Scenes.Arianscene.GiveArianReptilum.ConfusedAboutAura".Localized());
            else
                OutputText("Scenes.Arianscene.GiveArianReptilum.ChuckleTryIt".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.LicksCheekRainbows".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.CatsFlexibleButtQuestion".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianFirstReptilum) == 1)
                OutputText("Scenes.Arianscene.GiveArianReptilum.NervousAskAllRight".Localized());
            else
                OutputText("Scenes.Arianscene.GiveArianReptilum.FlexibilityImpressive".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.BendingLizanFalls".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.RatherNotLickButt".Localized());

            if (Player.HasCock())
            {
                OutputText(Player.MultiCockDescriptLight());

                if (Player.HasVagina())
                    OutputText("Scenes.Arianscene.GiveArianReptilum.AndWord".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Arianscene.GiveArianReptilum.VaginaPlaceholder".Localized());

            if (!Player.HasVagina() && !Player.HasCock())
                OutputText("Scenes.Arianscene.GiveArianReptilum.GenderlessCrotchWord".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.LicksYoursPrefix".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Arianscene.GiveArianReptilum.CockBaseToTip".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Arianscene.GiveArianReptilum.PussyBackToClit".Localized());
                else
                    OutputText("Scenes.Arianscene.GiveArianReptilum.SweatyCrotchWord".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.RoleplayOverStroking".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                ShowImage("arianfemale-home-mutualmasturbation");
            else
                ShowImage("arianmale-home-mutualmasturbation");

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) == 1)
            {
                OutputText("Scenes.Arianscene.GiveArianReptilum.QuicklyErectingCock".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.GiveArianReptilum.SLetter".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    OutputText("Scenes.Arianscene.GiveArianReptilum.AndEyPrefix".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                OutputText("Scenes.Arianscene.GiveArianReptilum.SlaveringVaginaWord".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.HighNeedsEncouragement".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.LaughingMoaningPraise".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.DaffyTouchingSelf".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 10)));

            var tfed = false;

            OutputText("Scenes.Arianscene.GiveArianReptilum.GigglingStopsOrgasm".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.GiveArianReptilum.ThickRopesUp".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 0)
                    OutputText("Scenes.Arianscene.GiveArianReptilum.ChestWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 1)
                        OutputText("Scenes.Arianscene.GiveArianReptilum.PerkyBreastsWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianBreasts) == 2)
                            OutputText("Scenes.Arianscene.GiveArianReptilum.SoftBreastsWord".Localized());
                        else
                            OutputText("Scenes.Arianscene.GiveArianReptilum.PillowyBreastsWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                    OutputText("Scenes.Arianscene.GiveArianReptilum.WhileWord".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                OutputText("Scenes.Arianscene.GiveArianReptilum.SoaksBedFemcum".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.SentencePeriodAgain".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.DizzyPraiseGiftedHands".Localized());

            if (Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 0 && CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.GiveArianReptilum.LooksDownFeelsWeird".Localized());

                OutputText("Scenes.Arianscene.GiveArianReptilum.SeeingDoubleJoke".Localized());

                tfed = true;
            }
            else
                OutputText("Scenes.Arianscene.GiveArianReptilum.WideYawnNappy".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.AnticlimacticFinisher".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.DefinitelyInteresting".Localized());

            if (tfed)
            {
                OutputText("Scenes.Arianscene.GiveArianReptilum.SensitiveTouchesSelf".Localized());

                OutputText("Scenes.Arianscene.GiveArianReptilum.TwoWasThatYou".Localized());

                OutputText("Scenes.Arianscene.GiveArianReptilum.AskLikeThem".Localized());

                OutputText("Scenes.Arianscene.GiveArianReptilum.LovesTwoDicks".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.ArianDoubleCock, 1);
            }
            else
                OutputText("Scenes.Arianscene.GiveArianReptilum.ThankWonderfulSurprise".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.ShouldBeThanking".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.ConfusedYawnInterrupts".Localized());

            OutputText("Scenes.Arianscene.GiveArianReptilum.TuckInLeave".Localized());

            ReturnToCamp(1);
        }

        public void TreatCorruption()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.TreatCorruption.AskReduceTaint".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianTreatment) == 1)
            {
                OutputText("Scenes.Arianscene.TreatCorruption.AlreadyTreatedToday".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.ApologizeComeBack".Localized());

                ArianHomeMenu();
            }
            else
            {
                OutputText("Scenes.Arianscene.TreatCorruption.NodsHoldHands".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.TakeHandsWait".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.CloseEyesBreathe".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.CloseEyesInhale".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.StrangeSensationCourses".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.EnoughOpenEyes".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.DontWorryCatchBreath".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.FeelBetterThoughtsClear".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

                Player.ChangeFatigue(20);

                if (Player.Corruption == 0)
                    OutputText("Scenes.Arianscene.TreatCorruption.PurifiedEntirely".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.ThankTreatment".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.OncePerDay".Localized());

                OutputText("Scenes.Arianscene.TreatCorruption.PromiseMoreCareful".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.ArianTreatment, CoC.Instance.Flags.Get(GameFlagIds.ArianTreatment) + 1);

                ReturnToCamp(1);
            }
        }

        public void ImbueTalisman()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ImbueTalisman.AskPlaceSpellClause".Localized());

            if (Player.HasKeyItem("Arian's Talisman"))
                OutputText("Scenes.Arianscene.ImbueTalisman.PlaceNewSpell".Localized());
            else
                OutputText("Scenes.Arianscene.ImbueTalisman.ChangeExistingSpell".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.SentencePeriod".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.FetchesParchment".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.ContemplateChoices".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.DispellingSpellDesc".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.HealingSpellDesc".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.ImmolationSpellDesc".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.LustReductionSpellDesc".Localized());

            OutputText("Scenes.Arianscene.ImbueTalisman.ShieldingSpellDesc".Localized());

            ClearButtons();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.WhiteBook), 1) && Player.HasItem(ItemRegistry.Create(ConsumableIds.BlackBook), 1))
                AddButton(0, "Common.Button.Dispelling".Localized(), () => ArianSpellPlace("Dispelling Spell"));
            else
                AddButton(0, "Common.Button.Dispelling".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SlimyClothoth), 2) && Player.HasItem(ItemRegistry.Create(ConsumableIds.VitalityTincture), 2))
                AddButton(1, "CharCreation.ChooseHistory.Button5".Localized(), () => ArianSpellPlace("Healing Spell"));
            else
                AddButton(1, "CharCreation.ChooseHistory.Button5".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle), 2) && Player.HasItem(ItemRegistry.Create(ConsumableIds.SweetGossamer)))
                AddButton(2, "Common.Button.Immolation".Localized(), () => ArianSpellPlace("Immolation Spell"));
            else
                AddButton(2, "Common.Button.Immolation".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft), 2) && Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft), 1))
                AddButton(3, "Common.Button.LustReduc".Localized(), () => ArianSpellPlace("Lust Reduction Spell"));
            else
                AddButton(3, "Common.Button.LustReduc".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(UseableIds.BeeChitin), 2) && Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk)))
                AddButton(4, "Common.Button.Shielding".Localized(), () => ArianSpellPlace("Shielding Spell"));
            else
                AddButton(4, "Common.Button.Shielding".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), ArianHomeMenu);
        }

        public void ArianSpellPlace(string spell)
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianSpellPlace.AskPlaceInTalisman".Localized(
                spell
            ));

            OutputText("Scenes.Arianscene.ArianSpellPlace.HandIngredientsPlease".Localized());

            OutputText("Scenes.Arianscene.ArianSpellPlace.NabsIngredientsDesk".Localized());

            OutputText("Scenes.Arianscene.ArianSpellPlace.ExpectedThatRemark".Localized());

            OutputText("Scenes.Arianscene.ArianSpellPlace.TalismanInBowl".Localized());

            OutputText("Scenes.Arianscene.ArianSpellPlace.HumWaterGlows".Localized());

            OutputText("Scenes.Arianscene.ArianSpellPlace.AcceptFeelEnergies".Localized());

            OutputText("Scenes.Arianscene.ArianSpellPlace.ImbuedConfirmMessage".Localized(
                spell
            ));

            int selected = 0;

            switch (spell)
            {
                case "Dispelling Spell":
                    selected = 1;

                    Player.ConsumeItem(ConsumableIds.WhiteBook, 2);

                    Player.ConsumeItem(ConsumableIds.BlackBook, 2);

                    break;

                case "Healing Spell":
                    selected = 2;

                    Player.ConsumeItem(ConsumableIds.SlimyClothoth, 2);

                    Player.ConsumeItem(ConsumableIds.VitalityTincture, 2);

                    break;

                case "Immolation Spell":
                    selected = 3;

                    Player.ConsumeItem(ConsumableIds.GoblinAle, 2);

                    Player.ConsumeItem(ConsumableIds.BlackGossamer);

                    break;

                case "Lust Reduction Spell":
                    selected = 4;

                    Player.ConsumeItem(ConsumableIds.LustDraft, 2);

                    Player.ConsumeItem(ConsumableIds.FuckDraft);

                    break;

                case "Shielding Spell":
                    selected = 5;

                    Player.ConsumeItem(UseableIds.BlackChitin, 2);

                    Player.ConsumeItem(ConsumableIds.ToughSilk);

                    break;
            }

            if (Player.HasKeyItem("Arian's Charged Talisman"))
            {
                Player.RemoveKeyItem("Arian's Charged Talisman");

                Player.CreateKeyItem("Arian's Charged Talisman", selected, 0, 0, 0);
            }
            else if (Player.HasKeyItem("Arian's Talisman"))
            {
                Player.RemoveKeyItem("Arian's Talisman");

                Player.CreateKeyItem("Arian's Charged Talisman", selected, 0, 0, 0);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void ClearTalisman()
        {

            Player.RemoveKeyItem("Arian's Charged Talisman");

            Player.CreateKeyItem("Arian's Talisman", 0, 0, 0, 0);
        }

        public void SleepWithArian(bool newl = false)
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Arian");

            OutputText("Scenes.Arianscene.SleepWithArian.RetireCatchShutEye".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.FindLizanStretching".Localized(
                ArianMF("male", "girly")
            ));

            OutputText("Scenes.Arianscene.SleepWithArian.StartledCoversUp".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.AskInvitationStands".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.NodsPutRobesOn".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.PrefersSleepNaked".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.ShareBodyHeat".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.AppreciateViewTease".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.FidgetsWatchesUndress".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.SleepWithArian.SeeExposedCock".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.SleepWithArian.SLetter".Localized());

                OutputText("Scenes.Arianscene.SleepWithArian.StiffeningAtSight".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                OutputText("Scenes.Arianscene.SleepWithArian.ArousedDespiteHiding".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.LetSquirmIndicateBed".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.MotionsHopInFirst".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.SmirkWhatWorried".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.AlwaysSpotForYou".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.ScootsWrapArm".Localized());

            OutputText("Scenes.Arianscene.SleepWithArian.LovesCloseContact".Localized());
// 原版 Achievements.hx:221「My Tent's (not) Better Than Yours」：睡在 Arian 的帐篷里
            AwardAchievement(new Identifier("coc", "general_my_tent_not_better"));

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
            {
                OutputText("Scenes.Arianscene.SleepWithArian.SensationStirsSleep".Localized());

                OutputText("Scenes.Arianscene.SleepWithArian.AwakeOrAsleep".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Listen".Localized(), ListenToLowAnalXPArian);

                AddButton(1, "Scenes.Camp.Sleep".Localized(), DontListenToLowAnalXPArian);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                {
                    OutputText("Scenes.Arianscene.SleepWithArian.WhimperingHipsRubbing".Localized());

                    OutputText("Scenes.Arianscene.SleepWithArian.ListenOrSleep".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Listen".Localized(), ListenToMediumAnalXPArian);

                    AddButton(1, "Scenes.Camp.Sleep".Localized(), DontListenToMediumAnalXPArian);
                }
                else
                {
                    OutputText("Scenes.Arianscene.SleepWithArian.ConfusedWhatDisturbs".Localized());

                    OutputText("Scenes.Arianscene.SleepWithArian.TeaseOrCalm".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Tease".Localized(), TeaseHighAnalXPArian);

                    AddButton(1, "Scenes.Camp.Sleep".Localized(), DontTeaseHighAnalXPArian);
                }
        }

        public void DontListenToLowAnalXPArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.DontListenToLowAnalXPArian.StrokeSideKissSleep".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CampSleep.SleepWrapper);
        }

        public void ListenToLowAnalXPArian()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianMorning, 1);

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.TurnHeadListen".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.InMyAssSure".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.DreamingAnalWonder".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.SausagesInBreads".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.FairDecisionProceed".Localized());
            else
                OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.LackingPenisUseFingers".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.WaitNotReady".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.WeirdLoseMood".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.BakeOvernightGoodnight".Localized());

            OutputText("Scenes.Arianscene.ListenToLowAnalXPArian.DreamedBaker".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CampSleep.SleepWrapper);
        }

        public void DontListenToMediumAnalXPArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.DontListenToMediumAnalXPArian.StrokeKissSleepAgain".Localized());

            CampScene.SleepRecovery(false);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CampSleep.SleepWrapper);
        }

        public void ListenToMediumAnalXPArian()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianMorning, 1);

            OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.TurnHeadListenAgain".Localized());

            OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.NotMyMuffins".Localized());

            OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.MuffinsSquirmingMischief".Localized());

            OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.WhatFlavorQuestion".Localized());

            OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.TellItSurprise".Localized());

            OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.LiftsTailShoveIn".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.HappyToOblige".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.FamiliarFlavorLove".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.BlinkAsleepQuestion".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.CockWord".Localized());
                else
                    OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.FingerWord".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.StillWormingAsleep".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.SheIsTeasing".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.DeservingOfIt".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.PaybackSleepInside".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.LackingPenisFingers".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.FamiliarFlavorAgain".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.BlinkAsleepAgain".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.SheIsTeasingAgain".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.DeservingOfItAgain".Localized());

                OutputText("Scenes.Arianscene.ListenToMediumAnalXPArian.SentencePeriod".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CampSleep.SleepWrapper);
        }

        public void DontTeaseHighAnalXPArian()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.DontTeaseHighAnalXPArian.StrokeKissSleepThird".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CampSleep.SleepWrapper);
        }

        public void TeaseHighAnalXPArian()
        {
            ClearOutput();

            ArianSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                ShowImage("arianfemale-camp-dreamingArian");
            else
                ShowImage("arianmale-camp-dreamingArian");

            CoC.Instance.Flags.Set(GameFlagIds.ArianMorning, 1);

            OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.CheckDreamTopic".Localized());

            OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.DontStuffTurkey".Localized());

            OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.FoodOrSexQuestion".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.IndulgeStrokeShaft".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.ItSoLittleTease".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.NoHesitateWriggle".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.HipsThrustMakeTasty".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.PersevereWhisperEar".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.BigAsMissTurkey".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.BigSquishyBelly".Localized(
                    Player.Mf("man", "herm")
                ));

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.HissesPutSauce".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.HotBotheredFinalThrusts".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.CantCumAsMuch".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.UnableToPullOut".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.NoPartsPity".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.TotalButtslutNoLube".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.HandStuckVacuum".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.GrowCockOrTighten".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.MissedSpotHumping".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.PumpThenBored".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.BagelsNotTasty".Localized());

                OutputText("Scenes.Arianscene.TeaseHighAnalXPArian.TugsDisgruntled".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CampSleep.SleepWrapper);
        }

        public void WakeUpAfterArianSleep()
        {
            ClearOutput();

            ArianSprite();

            if (Player.HasCock())
            {
                if (ArianMF("m", "f") == "f")
                    ShowImage("arianfemale-camp-dreamingArian");
                else
                    ShowImage("arianmale-camp-dreamingArian");

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
                {
                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DreamsGoneMorning".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.OutOfBedStretch".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WideGrinRepeat".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.HeadUnderCovers".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SLetter".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SentencePeriod".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.MasterfulAssIncentive".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ApologeticTeaseOffer".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.InclinedGrinObvious".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SitDownMotionBed".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.CatchOnBareCrotch".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.NuzzlesRubsFace".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ItWord".Localized());
                    else
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ThemWord".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DollopPreDeliberate".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LooksUpGaugeReaction".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.EyesBulgeDrinksDown".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WipesSeedSorry".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DressLeaveChampion".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                    {
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.YawnDreamsChased".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ProtestsStopPoking".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PursedLipsQuestion".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.MixedSignalsWorry".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.RecountNightBehavior".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SleepTalkingSorry".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.MorningWoodHint".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ScratchesChinOpposed".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SeizeEmbraceGuide".Localized(
                            ArianMF("his naughty boy-pussy", "her tail-hole")
                        ));

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.JumpsDoesntResist".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PushFirmInside".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.KnowsWorkMyAss".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ThrustMoanAssFuck".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SeedFillingCream".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.MoreOftenEmbrace".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PullFreeOffBack".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LookForRag".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.TapSneakyNose".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.YelpsPlugsBehind".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.RespectedMagePerverted".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.MakeItUpCloset".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.StillLodgedUrge".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DesperateButtfuckWrap".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.BucksBackEyesOpen".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.JustGivingDistracted".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.HumpsBackPosition".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WouldPounceInstead".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.HarderFasterBegs".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SphincterContractsDeeper".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                        {
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PussySquirtsLowerBody".Localized(
                                ArianMF("his", "her")
                            ));

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.AndClauseWord".Localized());
                            else
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SemicolonSpace".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                            {
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PlaceholderSpace".Localized(
                                    ArianMF("his", "her")
                                ));

                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.TwinShaftsAre".Localized());
                                else
                                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ShaftIs".Localized());

                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WhiteRopesBedsheets".Localized());
                            }

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.NaughtyLittleLizan".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.TeaseSuchButtslut".Localized());

                        if (!Player.HasVagina())
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.AlwaysWantedManTop".Localized());
                        else
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.GetOffGirlDick".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LoveItRegardless".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ThriceMoreHollowCry".Localized());

                        if (Player.CumQ() <= 200)
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.FirstJetPaintInsides".Localized());
                        else
                            if (Player.CumQ() <= 750)
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.StringAfterString".Localized());
                            else
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.EruptionFloodsButtslut".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LovedWakingUpTonight".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SilentPullOut".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.YawnStillSleepy".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LazyLittleLizard".Localized());
                    }

                Player.Orgasm("Dick");
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 33)
                {
                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.AwakenSnuggleScaly".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DidYouEnjoyBed".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ConsiderMoreOften".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.GoodIdeaFishing".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DayWontStartIssue".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WetSlideAgainstYou".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WizardSlurpsGusto".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.OrgasmApproachesLetGo".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.StableArmsRocking".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DressedNiceDayVisit".Localized());

                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WaitNeverMind".Localized());

                    Player.Orgasm("Generic");
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianAnalXp) < 66)
                    {
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.AwakenSnuggleAgain".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DidSleptWell".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SwallowsStrangeDream".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ThingsDoneToMe".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DidntUnderstandWord".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.StrokeSqueezeAss".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                        {
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PlaceholderSpaceAgain".Localized(
                                ArianMF("his", "her")
                            ));

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) >= 3)
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ExposedWord".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.BulbousShaftWord".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SBeginPrefix".Localized());
                            else
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.BeginsWord".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SportingErection".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) < 3)
                            {
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PokingClause".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) == 1)
                                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ItsTipWord".Localized());
                                else
                                    OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.TheirTipsWord".Localized());

                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.OutOfGenitalSlit".Localized());
                            }

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SentencePeriodAgain".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianVagina) > 0)
                                OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.MoistPussyStream".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PantsEyesGlazed".Localized());

                        if (Player.HasVagina())
                        {
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LeanStaresUp".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.RubPositionLegs".Localized(
                                ArianMF("his cock", "hers")
                            ));

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.QuickerRecoverJuices".Localized(
                                ArianMF("engorged member", "flushed folds")
                            ));

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.PressHarderRubbing".Localized(
                                ArianMF("parting your lips around his cock", "giving the trembling lizan the lewdest kiss imaginable")
                            ));

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ShiverGettingClose".Localized(
                                ArianChest()
                            ));

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.WritheThrustWhimper".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LieEnjoyAfterglow".Localized());

                            Player.Orgasm("Vaginal");
                        }
                        else
                            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.StepBackTenderKiss".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.QuietYawnNewDay".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.YawnsLicksLipsGreets".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ProblemKnowingSmile".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.RegrowCockJoke".Localized());

                        if (Player.HasVagina())
                        {
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.SmileBeliesIntentions".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.OhIsThat".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.FingersCreepCloser".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ICanGuess".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ThrustCryCutsOff".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.StrokesTwitchingWild".Localized(
                                ArianMF("cock", "entrance")
                            ));

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.FingersReciprocateFind".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.ClenchesThrashing".Localized());

                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.LieBreathingHeavily".Localized());

                            Player.Orgasm("Vaginal");
                        }
                        else
                        {
                            OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.DontWorryPrettyHead".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                        }

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.FinishDressedAskWhy".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.FiddlesGrowItBack".Localized());

                        OutputText("Scenes.Arianscene.WakeUpAfterArianSleep.UsedToReaming".Localized());
                    }

            CoC.Instance.Flags.Set(GameFlagIds.ArianMorning, 0);

            Go<CampScene>();
        }

        public void ArianEggingEvent()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianEggEvent, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggChat) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ArianEggChat, CoC.Instance.Flags.Get(GameFlagIds.ArianEggChat) + 1);

                OutputText("Scenes.Arianscene.ArianEggingEvent.HearsMoanTent".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.LayingRubsBelly".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.JustBitSick".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.ReliefHealthProblem".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.LizanEggsology".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.OverpopulateQuestion".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.ClutchLowFertility".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.CouldFertilizeEgg".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.AvertsEyesFamily".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.FamilyOutOfQuestion".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.ReassuringCareful".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.BigResponsibility".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.CuriousWhatDoWith".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.EatThemProtein".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.HelpfulDontStrain".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.PuffsChestProud".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.AppreciateColoredEggs".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.WhichColorQuestion".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.ArianEggingEvent.HearsMoanAgain".Localized());

                OutputText("Scenes.Arianscene.ArianEggingEvent.EntersGreetedNaked".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Brown".Localized(), () => PickAnEggArian("brown"));

            AddButton(1, "Common.Button.Purple".Localized(), () => PickAnEggArian("purple"));

            AddButton(2, "Common.Button.Blue".Localized(), () => PickAnEggArian("blue"));

            AddButton(3, "Common.Button.Pink".Localized(), () => PickAnEggArian("pink"));

            AddButton(4, "Common.Button.White".Localized(), () => PickAnEggArian("white"));

            AddButton(5, "Common.Button.Rubber".Localized(), () => PickAnEggArian("rubbery black"));

            AddButton(6, "Common.Button.NoThanks".Localized(), LeaveEggs);
        }

        public void PickAnEggArian(string color = "pink")
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianEggColor, color);

            OutputText("Scenes.Arianscene.PickAnEggArian.AskMakeEgg".Localized(
                color
            ));

            OutputText("Scenes.Arianscene.PickAnEggArian.ClosesEyesFocus".Localized());

            OutputText("Scenes.Arianscene.PickAnEggArian.LightGathersBelly".Localized(
                color
            ));

            OutputText("Scenes.Arianscene.PickAnEggArian.ThankPeckCheek".Localized());

            OutputText("Scenes.Arianscene.PickAnEggArian.NodWaveLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ArianEggCounter, 1);

            ReturnToCamp(1);
        }

        public void LeaveEggs()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.LeaveEggs.NoColoredEggs".Localized());

            ReturnToCamp(1);
        }

        public void ArianLaysEggs()
        {
            ClearOutput();

            ArianSprite();

            string color = CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor);

            CoC.Instance.Flags.Set(GameFlagIds.ArianEggCounter, 0);

            OutputText("Scenes.Arianscene.ArianLaysEggs.TentLaborDiscovery".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                OutputText("Scenes.Arianscene.ArianLaysEggs.HermWord".Localized());
            else
                OutputText("Scenes.Arianscene.ArianLaysEggs.GirlWord".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.SquattingBasinPose".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.SteadyHerSway".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.TheLizanPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                OutputText("Scenes.Arianscene.ArianLaysEggs.HermPlaceholder".Localized(
                    ArianMF("-boy", "")
                ));
            else
                OutputText("Scenes.Arianscene.ArianLaysEggs.FemaleWord".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.LungfulsAirHiss".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.ScoldMissedCall".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.NicerWithCompany".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.ThankfulTakeOver".Localized(
                ArianMF("hermboy", "girl")
            ));

            OutputText("Scenes.Arianscene.ArianLaysEggs.ThroatyMoanPraise".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.CatchFallingEggs".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.QuickFinishPromise".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.BraveGirlWhisper".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.PitchSwitchingLaugh".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.FetchWaterHelp".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.SortingEggsFind".Localized(
                color
            ));

            OutputText("Scenes.Arianscene.ArianLaysEggs.DevouringEggs".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.CoughReminder".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.SheepishEggOffer".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.DeclineOfferDress".Localized(
                color
            ));

            OutputText("Scenes.Arianscene.ArianLaysEggs.GaspsAndCovers".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.BreakfastBelch".Localized());

            OutputText("Scenes.Arianscene.ArianLaysEggs.PocketEggFarewell".Localized());

            object itype = null;

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor) == "brown")
                itype = ConsumableIds.EggLargeBrown;
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor) == "purple")
                    itype = ConsumableIds.EggLargePurple;
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor) == "blue")
                        itype = ConsumableIds.EggLargeBlue;
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor) == "pink")
                            itype = ConsumableIds.EggLargePink;
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor) == "white")
                                itype = ConsumableIds.EggLargeWhite;
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.ArianEggColor) == "rubbery black")
                                    itype = ConsumableIds.EggLargeBlack;

            Player.Inventory.Add(ItemRegistry.Create((Identifier)itype), 1);

            ReturnToCamp(1);
        }

        public void ArianDildoFun()
        {
            ClearOutput();

            ArianSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesArianDildoed) == 0)
            {
                OutputText("Scenes.Arianscene.ArianDildoFun.EyesDownBody".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
                {
                    OutputText("Scenes.Arianscene.ArianDildoFun.PastCockClause".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                        OutputText("Scenes.Arianscene.ArianDildoFun.SLetter".Localized());
                }
                else
                    OutputText("Scenes.Arianscene.ArianDildoFun.Comma".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.PussyYearningSetup".Localized());

                if (Player.HasKeyItem("Dildo"))
                    OutputText("Scenes.Arianscene.ArianDildoFun.GiacomoPurchase".Localized());
                else
                    OutputText("Scenes.Arianscene.ArianDildoFun.TamaniGift".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.AskingWillingTry".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.AvertsEyesShy".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.ShushAndReassure".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.NervousSmile".Localized());
            }
            else
            {
                OutputText("Scenes.Arianscene.ArianDildoFun.PresentToyAgain".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.EmbarrassedFidget".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesArianDildoed, CoC.Instance.Flags.Get(GameFlagIds.TimesArianDildoed) + 1);

            OutputText("Scenes.Arianscene.ArianDildoFun.DildoAsideEmbrace".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.KissLegsSpread".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.ArianDildoFun.CockIgnoredPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.ArianDildoFun.SLetterAgain".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.LowerYPrefix".Localized());
            }
            else
                OutputText("Scenes.Arianscene.ArianDildoFun.CapitalYLetter".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.OneFingerSlide".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.WarmupOverSlap".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.NervousNodSeat".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.BeginnerSuckPraise".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.NippleTeaseShow".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.ClapTakeOver".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.LickToyInspect".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.ArianDildoFun.BrushCockShaft".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.ArianDildoFun.SLetterThird".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.BeforeWord".Localized());
            }

            OutputText("Scenes.Arianscene.ArianDildoFun.OuterLipsTease".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.MoansFeelsGreat".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.TauntProveIt".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.ConfusedThenThrust".Localized(
                ArianMF("", "she-")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianCockSize) > 0)
            {
                OutputText("Scenes.Arianscene.ArianDildoFun.CockWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ArianDoubleCock) > 0)
                    OutputText("Scenes.Arianscene.ArianDildoFun.PluralThrobSuffix".Localized());
                else
                    OutputText("Scenes.Arianscene.ArianDildoFun.ThrobsWord".Localized());

                OutputText("Scenes.Arianscene.ArianDildoFun.WhiteJetEscape".Localized());
            }

            OutputText("Scenes.Arianscene.ArianDildoFun.InstantOrgasm".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.MuffleScreamsPound".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.StuckToyTaunt".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.DazedQuestion".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.HoggingAccusation".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.SpellGrowCopy".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.TiredImpaleSelf".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.TugToyClimax".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.ClenchClimax".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.ToyRevertsCompliment".Localized(
                ArianMF("herm-boy", "girl")
            ));

            OutputText("Scenes.Arianscene.ArianDildoFun.LuckyOneGlomp".Localized(
                ArianMF("herm-boy", "girl")
            ));

            OutputText("Scenes.Arianscene.ArianDildoFun.CleanDildoOrder".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.TongueOutInvite".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.CleanInspectPraise".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.HerTurnToLick".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.GushIntoMouth".Localized());

            OutputText("Scenes.Arianscene.ArianDildoFun.CuddleDriftOff".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void ArianFacesittingFirst()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.AskRideFace".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.WaveOverFace".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.BegToTaste".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.SitOnMouth".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.SheCumsFirst".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.CommandHarderPinch".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.GoodGirlClimax".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.TongueCleansUp".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.LoveExchange".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.SexChangeTalk".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingFirst.SnuggleThenLeave".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianFacesitting()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            OutputText("Scenes.Arianscene.ArianFacesitting.EagerFaceRide".Localized());

            OutputText("Scenes.Arianscene.ArianFacesitting.WaveOverFace".Localized());

            OutputText("Scenes.Arianscene.ArianFacesitting.BegToTaste".Localized());

            OutputText("Scenes.Arianscene.ArianFacesitting.SitOnMouth".Localized());

            OutputText("Scenes.Arianscene.ArianFacesitting.SheCumsFirst".Localized());

            ArianFaceSittingContinued();

            if (OptionStore.Instance.Get("watersports", 0) > 0)
                AddButton(1, "Common.Button.Watersports".Localized(), ArianFacesittingWatersports);
        }

        public void ArianFaceSittingContinued()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianFaceSittingContinued.RhythmNipplePinch".Localized());

            OutputText("Scenes.Arianscene.ArianFaceSittingContinued.GoodGirlClimax".Localized());

            OutputText("Scenes.Arianscene.ArianFaceSittingContinued.TongueCleansUp".Localized());

            OutputText("Scenes.Arianscene.ArianFaceSittingContinued.CleaningTasteTalk".Localized());

            OutputText("Scenes.Arianscene.ArianFaceSittingContinued.BriefSnuggleFarewell".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianFacesittingWatersports()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianFacesittingWatersports.PissPunishment".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingWatersports.SpreadPissRide".Localized());

            OutputText("Scenes.Arianscene.ArianFacesittingWatersports.CleanupOffer".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianCunnilingus()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            OutputText("Scenes.Arianscene.ArianCunnilingus.KissTongueIdea".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingus.DisplayForTongue".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingus.TeaseThenCommand".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Gentle".Localized(), ArianCunnilingusGentle);

            AddButton(1, "Common.Button.Tonguefuck".Localized(), ArianCunnilingusTonguefuck);
        }

        public void ArianCunnilingusGentle()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianCunnilingusGentle.HerOwnApproach".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingusGentle.GSpotSwirl".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingusGentle.HoldFluidsCum".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingusGentle.GlisteningConfession".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianCunnilingusTonguefuck()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianCunnilingusTonguefuck.DoubleTongueDeep".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingusTonguefuck.ReverseBlowjob".Localized());

            Player.CuntChange(6, true, true, false);

            OutputText("Scenes.Arianscene.ArianCunnilingusTonguefuck.MutualOrgasm".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingusTonguefuck.PraiseAndKiss".Localized());

            OutputText("Scenes.Arianscene.ArianCunnilingusTonguefuck.NapAndLeave".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianFemaleTailriding()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.TailUpRequest".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.StraddleTail".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.SuckHerClit".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.TailToEntrance".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.ImpaleOnTail".Localized());

            Player.CuntChange(20, true, true, false);

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.RideToOrgasm".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.FingerHerNeeds".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.HerTailSpasm".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.CoolDownJoke".Localized());

            OutputText("Scenes.Arianscene.ArianFemaleTailriding.PatAndDepart".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianHeadpats()
        {
            ClearOutput();

            ArianSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ArianVirgin, CoC.Instance.Flags.Get(GameFlagIds.ArianVirgin) + 1);

            ArianHealth(3);

            OutputText("Scenes.Arianscene.ArianHeadpats.KneelForPats".Localized());

            OutputText("Scenes.Arianscene.ArianHeadpats.RampUpPats".Localized());

            OutputText("Scenes.Arianscene.ArianHeadpats.FlurryFirstOrgasm".Localized());

            OutputText("Scenes.Arianscene.ArianHeadpats.SuckThenSquirt".Localized());

            OutputText("Scenes.Arianscene.ArianHeadpats.BeggingForMore".Localized());

            OutputText("Scenes.Arianscene.ArianHeadpats.EnoughAndPout".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ArianAffection()
        {

            ClearButtons();

            AddNextButton("Common.Button.Kiss".Localized(), ArianKiss);

            AddNextButton("Common.Button.HoldHands".Localized(), ArianHandholding);

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), ArianCuddle);

            AddButton(14, "Common.Button.Back".Localized(), () => ArianHomeMenu());
        }

        public void ArianHandholding()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianHandholding.CouchHandhold".Localized());

            OutputText("Scenes.Arianscene.ArianHandholding.StayAndSleep".Localized());

            ArianHandholding2();
        }

        public void ArianHandholding2()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianHandholding2.WakeInLap".Localized());

            OutputText("Scenes.Arianscene.ArianHandholding2.ThankAndLeave".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

            Go<CampScene>();
        }

        public void ArianKiss()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianKiss.SurpriseKiss".Localized());

            OutputText("Scenes.Arianscene.ArianKiss.CouchKissMarathon".Localized());

            OutputText("Scenes.Arianscene.ArianKiss.DreamyLikeKissing".Localized());

            ReturnToCamp(1);
        }

        public void ArianCuddle()
        {
            ClearOutput();

            ArianSprite();

            OutputText("Scenes.Arianscene.ArianCuddle.SpoonAndKiss".Localized());

            OutputText("Scenes.Arianscene.ArianCuddle.RestThenGoodbye".Localized());

            ReturnToCamp(1);
        }

        public void GiveLethicite()
        {
            ClearOutput();

            OutputText("Scenes.Arianscene.GiveLethicite.OfferGem".Localized());

            OutputText("Scenes.Arianscene.GiveLethicite.AsksProvenance".Localized());

            OutputText("Scenes.Arianscene.GiveLethicite.DemonFoodWaste".Localized());

            OutputText("Scenes.Arianscene.GiveLethicite.SuggestsStaff".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Staff".Localized(), ReceiveLethiciteStaff);

            AddButton(1, "Common.Button.NoThanks".Localized(), ArianHomeMenu);
        }

        public void ReceiveLethiciteStaff()
        {
            ClearOutput();

            OutputText("Scenes.Arianscene.ReceiveLethiciteStaff.HandOverShards".Localized());

            OutputText("Scenes.Arianscene.ReceiveLethiciteStaff.DistractedApology".Localized());

            OutputText("Scenes.Arianscene.ReceiveLethiciteStaff.StaffReveal".Localized());

            OutputText("Scenes.Arianscene.ReceiveLethiciteStaff.ThankfulExit".Localized());

            Player.DestroyItems(ItemIds.Lethicite, 3);

            Player.DestroyItems(ItemIds.WizardStaff, 1);

            Player.Inventory.Add(ItemRegistry.Create(WeaponIds.LethiciteStaff), 1);

            ReturnToCamp(1);
        }

        public void AskArianAboutNobleShirt()
        {
            ClearOutput();

            RegisterMacro("shirtStage", Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0));

            OutputText("Scenes.Arianscene.AskArianAboutNobleShirt.ShirtWontComeOff".Localized());

            OutputText("Scenes.Arianscene.AskArianAboutNobleShirt.CalmHerFocus".Localized());

            OutputText("Scenes.Arianscene.AskArianAboutNobleShirt.UnknownEnchant".Localized());

            OutputText("Scenes.Arianscene.AskArianAboutNobleShirt.DoubtFindingBooks".Localized());

            Undergarments.SEA_SHIRT.Save.SetInt("arianAsked", 1);

            ReturnToCamp(1);
        }

        public void AskArianToDisenchant()
        {
            ClearOutput();

            RegisterMacro("shirtStage", Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0));

            RegisterMacro("izmaAsked", Undergarments.SEA_SHIRT.Save.GetInt("izmaAsked", 0));

            OutputText("Scenes.Arianscene.AskArianToDisenchant.ExplainBinding".Localized());

            OutputText("Scenes.Arianscene.AskArianToDisenchant.BreakBindingOffer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DoIt".Localized(), ArianDoIt);

            AddButton(1, "Scenes.CorruptedGlade.NeverMind".Localized(), ArianNevermind);
        }

        public void ArianNevermind()
        {

            OutputText("Scenes.Arianscene.ArianNevermind.DeclineSeekExpert".Localized());

            ReturnToCamp(1);
        }

        public void ArianDoIt()
        {

            OutputText("Scenes.Arianscene.ArianDoIt.AcceptFreeService".Localized());

            Player.DestroyItems(Undergarments.SEA_SHIRT.Id, 1);

            Undergarments.SEA_SHIRT.Save.SetInt("arianDisenchant", 1);

            Undergarments.SEA_SHIRT.Save.SetInt("arianDisenchantDay", View.Time.Days);

            ReturnToCamp(1);
        }

    }
}