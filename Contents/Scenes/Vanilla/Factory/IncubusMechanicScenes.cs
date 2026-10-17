using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Factory
{
    [Register<IncubusMechanicScenes>("coc:incubus_mechanic")]
    public class IncubusMechanicScenes : Scene
    {

        public const int MECHANIC_NO_SELECTION = 0;

        public const int MECHANIC_PAID = 1;

        public const int MECHANIC_SUCKED = 2;

        public const int MECHANIC_FOUGHT = 3;

        public const int MECHANIC_KILLED = 1;

        public const int MECHANIC_RELEASED = 2;

        public const int MECHANIC_FUCKED = 3;

        public const int MECHANIC_FUCKED_YOU = 4;

        public const int MECHANIC_DEMON_COCK = 0;

        public const int MECHANIC_HORZ_GOG = 1;

        public const int MECHANIC_DOG_COCK = 2;

        public override void Entry()
        {

        }

        public bool MetIncubusMechanicInD1()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryIncubusDefeated) > 0 || CoC.Instance.Flags.Get(GameFlagIds.FactoryIncubusBribed) > 0)
                return true;

            return false;
        }

        public void EncounterIncubusFactory()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryIncubusBribed) > 0)
            {
                OutputText("Scenes.Incubusmechanicscenes.EncounterIncubusFactory.MechanicThumbingThroughMag".Localized());

                AddButton(0, "Common.Button.Fight".Localized(), DoFightIncubus);
            }
            else
            {
                OutputText("Scenes.Incubusmechanicscenes.EncounterIncubusFactory.DemonicMechanicLounges".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), DoFightIncubus);

                AddButton(1, "Common.Button.Talk".Localized(), TalkIncubus);
            }
        }

        public void TalkIncubus()
        {
            ClearOutput();

            if (Player.HasKeyItem("Hentai Comic"))
            {
                OutputText("Scenes.Incubusmechanicscenes.TalkIncubus.IncubusSpeaksWith".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), DoFightIncubus);

                AddButton(1, "Common.Button.Submit".Localized(), DoSubmitIncubus);

                AddButton(2, "Common.Button.Trade".Localized(), DoTradeIncubus);
            }
            else
            {
                OutputText("Scenes.Incubusmechanicscenes.TalkIncubus.IncubusSpeaksWithAgain".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), DoFightIncubus);

                AddButton(1, "Common.Button.Submit".Localized(), DoSubmitIncubus);
            }
        }

        public void DoTradeIncubus()
        {
            ClearOutput();

            OutputText("Scenes.Incubusmechanicscenes.DoTradeIncubus.HandOverHentaiComic".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel) > 0)
                OutputText("Scenes.Incubusmechanicscenes.DoTradeIncubus.SwearYouCanHear".Localized());

            Player.RemoveKeyItem("Hentai Comic");

            CoC.Instance.Flags.Set(GameFlagIds.FactoryIncubusBribed, 1);

        }

        public void DoFightIncubus()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FactoryIncubusDefeated, 1);

            OutputText("Scenes.Incubusmechanicscenes.DoFightIncubus.ShoutWithConviction".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoFightIncubus.UnseenForceClosesDoor".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoFightIncubus.ItsAFight".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "incubus_mechanic")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DoSubmitIncubus()
        {
            ClearOutput();

            OutputText("Scenes.Incubusmechanicscenes.DoSubmitIncubus.IntonesStrangeMechanic".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
            {
                OutputText("Scenes.Incubusmechanicscenes.DoSubmitIncubus.EnterMilkingChamber".Localized());

                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "incubus_mechanic")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

                return ;
            }

            OutputText("Scenes.Incubusmechanicscenes.DoSubmitIncubus.BroughtIntoARoom".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoSubmitIncubus.GesturingTowardsAn".Localized());

            return ;
        }

        public void DefeatIncubus()
        {
            ClearOutput();

            OutputText("Common.Text.YouSmileInSatisfactionAsThe".Localized());

            if (Player.HasCock())
                AddButton(0, "Common.Button.Rape".Localized(), DoRapeIncubus);
            else
                AddButtonDisabled(0, "Rape", "This scene requires you to have a cock.");

            AddButton(1, "Common.Button.ServiceHim".Localized(), DoOralIncubus);

            AddButton(2, "Common.Button.AnalRide".Localized(), DoRideIncubusAnally);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void DoLossIncubus()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (Player.HasFuckableNipples() && Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.MoltenArousalPumps".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.EyesRelaxPureSensation".Localized());

                if (Player.TotalNipples() > 2)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.EachDividesAgainSplitting".Localized());

                if (Player.TotalNipples() > 4)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ContinueDividingWriggling".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ParagraphBreak".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.PleadingMoanEscapes".Localized());

                if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.SplatteringMilkAndPre".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.SplatteringYourTits".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.DemonTensesPullingHead".Localized());

                Player.RefillHunger(100);

                Player.Orgasm("Nipples");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 20f));

                if (CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.IncubusGrumbles".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                {

                }
                else
                    CombatScene.CleanupAfterCombat();

                return ;
            }
            else
            {
                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.MoltenArousalPumpsAgain".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.LowerYourTopExposing".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.LoweringEyesFixating".Localized());

                if (Player.Corruption < 80)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.InADisgustingDisplay".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GropeYourselfNoisily".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.WatchHisPricksPulse".Localized());

                if (Player.Corruption >= 80)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.RealizeTheirSizeAnd".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.EachFatTentacleFlash".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.MakeSureRubEach".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.PairOfThemSlides".Localized());

                if (Player.Corruption < 80)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.WriggleAndWhine".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GrabThickTentacleCock".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.SqueezingLegsAroundThem".Localized());
                else
                {
                    if (Player.Balls > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GrindingAgainstRibbed".Localized());
                    else
                        if (Player.Cocks.Count > 0)
                            OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.SqueezingLegsAroundThick".Localized());

                    if (Player.Gender == Gender.Genderless)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GrindingOnCaptors".Localized());
                }

                OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ParagraphBreakAgain".Localized());

                if (Player.Vaginas.Count > 0 && (Player.Cocks.Count == 0 || Random.Shared.Next(2) != 0))
                {
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.IncubusAtLastDecides".Localized());

                    if (Player.Corruption < 80)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.FeelYourLiftedPulled".Localized());
                    else
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.AsLiftsYouSpread".Localized(
                            Player.AssholeDescript(),

                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GiggleAndTryTo".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 80)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GaspFromPleasure".Localized());
                    else
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.OpenYourMouthWide".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.EnjoyingKnobbedTexture".Localized());

                    Player.CuntChange(Player.VaginalCapacity() * .8f, true);

                    Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.SmirksAsAnotherCock".Localized());

                    if (Player.Corruption < 80)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.SqueezedByTheWrithing".Localized());
                    else
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.RoughlyFuckedByTwo".Localized());

                    if (Player.GetClitLength() > 3)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.NearlyCumTheSpot".Localized());
                    else
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.NearlyCumSpotAgain".Localized());

                    if (Player.Corruption >= 80)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.CumMoreTimesThan".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.PrickInMouthSurges".Localized(
                        Player.VaginaDescript(0)
                    ));

                    Player.RefillHunger(100);

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 25f));

                    if (CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.IncubusGrumblesAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                    {

                    }
                    else
                        CombatScene.CleanupAfterCombat();

                    return ;
                }
                else
                {
                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.IncubusLastDecidesAgain".Localized());

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.BodyOrificesFill".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GroanAroundTheDick".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ParagraphBreakThird".Localized());

                    if (Player.Gender == Gender.Genderless)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.TaintedCumMixed".Localized());

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.CockTentacleIncreases".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.FeelCumPulse".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ParagraphBreakFourth".Localized());

                    if (Player.Cocks.Count > 0)
                    {
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ThoroughlyUsedStimulated".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.BallsWord".Localized());
                        else
                            OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.ProstateWord".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.UnloadsWithEnoughForce".Localized());

                        if (Player.CumQ() < 50)
                            OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.GroanAndOrgasm".Localized());

                        if (Player.CumQ() >= 50 && Player.CumQ() < 400)
                            OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.OrgasmGoesOffLike".Localized());

                        if (Player.CumQ() >= 400)
                            OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.OrgasmOffLikeAgain".Localized());
                    }

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.FeelingSoIntense".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.BlackOutJustAs".Localized());

                    Player.RefillHunger(100);

                    Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                    Player.Orgasm("Anal");

                    if (CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoLossIncubus.IncubusGrumblesThird".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 25f));

                    if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                    {

                    }
                    else
                        CombatScene.CleanupAfterCombat();
                }
            }
        }

        public void DoRideIncubusAnally()
        {
            ClearOutput();

            OutputText("Scenes.Incubusmechanicscenes.DoRideIncubusAnally.EveryDayYouveSpent".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoRideIncubusAnally.FightOverYourEyes".Localized(
                Player.AssholeDescript()
            ));

            OutputText("Scenes.Incubusmechanicscenes.DoRideIncubusAnally.EyesFlashOpen".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoRideIncubusAnally.AllTooSoonGrunts".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoRideIncubusAnally.AwakenMomentsLater".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            Player.SlimeFeed();

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void DoRapeIncubus()
        {
            ClearOutput();

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.FewDeftMotions".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    if (Player.Balls > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.AndWord".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.AndVagina".Localized(
                            Player.VaginaDescript(0)
                        ));
                }
                else
                    if (Player.Balls > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.AndBalls".Localized());
                    else
                        OutputText(Player.MultiCockDescriptLight());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.HavingResolvedToTake".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.DontWasteAnyTime".Localized());

                if (Player.Cocks[0].Length > 10)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.DeepInsideWord".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.InsideWord".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.DemonUnderneathGrunts".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.SwingFreelySmacking".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.ParagraphBreak".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.FeelDrawUpTight".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.WarmHeatBuilds".Localized());

                if (Player.CumQ() > 200)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.GroanAsYouFeel".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.ThrowBackYourHead".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.FewThickSpurtsLater".Localized());

                if (Player.CumQ() >= 50 && Player.CumQ() < 400)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.ThickJetsPumpInto".Localized());

                if (Player.CumQ() >= 400)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.DistendingBulge".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.FallBackTheFluid".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.TurnToGatherYour".Localized());

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                Player.SlimeFeed();

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.FewDeftMotionsAgain".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    if (Player.Balls > 0)
                        OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.AndBallsAgain".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        if (Player.Balls > 0)
                            OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.AndVaginaAgain".Localized(
                                Player.VaginaDescript(0)
                            ));
                }
                else
                    OutputText(Player.VaginaDescript(0));

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.StridingForwardsHunger".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.LooksUpAtYou".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.ThankfulForGesture".Localized());

                if (Player.Vaginas[0].Wetness >= 3)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.InNoTimeFlat".Localized());

                if (Player.Vaginas[0].Wetness < 3 && Player.Vaginas[0].Wetness != 0)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.LubricatedALot".Localized());

                if (Player.Vaginas[0].Wetness == 0)
                    OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.DespiteLightLubrication".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.RelaxingTheMuscles".Localized(
                    Player.Legs()
                ));

                Player.CuntChange(Player.VaginalCapacity() * .8f, true);

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.GaspAndMoanLike".Localized());

                OutputText("Scenes.Incubusmechanicscenes.DoRapeIncubus.UnableToContain".Localized(
                    Player.VaginaDescript(0)
                ));

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void DoOralIncubus()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.LickLipsMoistening".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.LeanForwardsOpening".Localized());

            if (Player.Body.Get<HairPart>().Length > 10)
                OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.BrushingStrandOf".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.ShinyPurplishMonster".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.FeelHisBallsBegin".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.HipsBeginHumping".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.ParagraphBreak".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.PushExhaustedDemon".Localized());

            OutputText("Scenes.Incubusmechanicscenes.DoOralIncubus.AfterRedressingTurn".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void DoLeaveIncubus()
        {

            CombatScene.CleanupAfterCombat();
        }

        public void MeetAtElevator()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.D3MechanicLastGreet) == MECHANIC_NO_SELECTION)
            {
                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.AWord".Localized());

                if (MetIncubusMechanicInD1())
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.FamiliarWord".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.MechanicLoungesGleaming".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryIncubusBribed) > 0)
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.RolledUpMagazineTucked".Localized());

                if (MetIncubusMechanicInD1())
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.SameIncubusYou".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.SportsPairOfStained".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.StiffensUnderAttentions".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.ScowlingAtBeingCaught".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.ParagraphBreak".Localized());

                if (MetIncubusMechanicInD1())
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.OnceMoreYouBumble".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.YouveBumbled".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.IntoAHornetsNest".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.HesOfferingADeal".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.IncredulityPlainOn".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.MechanicDoesntSeem".Localized());

                if (Player.Gems >= 500)
                    AddButton(0, "Common.Button.PayToll".Localized(), PayDaToll);
                else
                    AddButton(0, "Common.Button.PayToll".Localized(), null);

                AddButton(1, "Common.Button.SuckDick".Localized(), SuckIncubusDick);

                AddButton(2, "Common.Button.Fight".Localized(), () => CombatScene.Start(EnemyRegistry.Create(SceneIds.Core.IncubusMechanicScenes), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.D3MechanicLastGreet) == MECHANIC_SUCKED)
                {
                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.AsSoonAsMechanic".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.GrinningDemonOffers".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.AppearsOnlyOptions".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                    AddButton(1, "Common.Button.SuckDick".Localized(), SuckIncubusDick);

                    AddButton(2, "Common.Button.Fight".Localized(), () => CombatScene.Start(EnemyRegistry.Create(SceneIds.Core.IncubusMechanicScenes), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.D3MechanicLastGreet) == MECHANIC_PAID)
                    {
                        OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.QueriesWhileHeReclines".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.VeryAirSeemsThicken".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.MeetAtElevator.LeavePayHimFor".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        if (Player.Gems >= 500)
                            AddButton(0, "Common.Button.PayToll".Localized(), PayDaToll);
                        else
                            AddButton(0, "Common.Button.PayToll".Localized(), null);

                        AddButton(1, "Common.Button.SuckDick".Localized(), SuckIncubusDick);

                        AddButton(2, "Common.Button.Fight".Localized(), () => CombatScene.Start(EnemyRegistry.Create(SceneIds.Core.IncubusMechanicScenes), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false));
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.D3MechanicLastGreet) == MECHANIC_FOUGHT)
                            AddButton(0, "Common.Button.Lift".Localized(), UseLiftPostDefeat);
        }

        public void PayDaToll()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicLastGreet, MECHANIC_PAID);

            Player.Gems -= 500;

            OutputText("Scenes.Incubusmechanicscenes.PayDaToll.UnceremoniouslyFillPouch".Localized());

            OutputText("Scenes.Incubusmechanicscenes.PayDaToll.LamelySaysTheDemon".Localized());

            OutputText("Scenes.Incubusmechanicscenes.PayDaToll.ShakeYourHead".Localized());

            OutputText("Scenes.Incubusmechanicscenes.PayDaToll.MaleSighsYouSmirk".Localized());

            OutputText("Scenes.Incubusmechanicscenes.PayDaToll.PlatformTouchesDown".Localized());

        }

        public void UseLiftPostDefeat()
        {
            ClearOutput();

            OutputText("Scenes.Incubusmechanicscenes.UseLiftPostDefeat.SpendMinuteOrTwo".Localized());

            OutputText("Scenes.Incubusmechanicscenes.UseLiftPostDefeat.TheresARailing".Localized());

            OutputText("Scenes.Incubusmechanicscenes.UseLiftPostDefeat.PlatformTouchesDown".Localized());

        }

        public void SuckIncubusDick()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicLastGreet, MECHANIC_SUCKED);

            if (Player.Gems < 500)
                OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.SpareAGlance".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.ShrugAndSmile".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.StiffeningFurtherAtDeclaration".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.ScowlingYouGoAhead".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.FindYourCrotchBecoming".Localized());

                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.EvenMoreSoaked".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.WetWithFreshLust".Localized());
                    else
                        OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.StickyWithLust".Localized());

                OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.AtHisMasculineScent".Localized());
            }
            else
                if (Player.HasCock())
                    OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.FeelHeadyTurgidWarmth".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.PatchOfTrimmedPubic".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.SoCloseYouCould".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.HandRubsThroughYour".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.SighsSlouchingABit".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.LeanInTowardHim".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.NoMatterManyTimes".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.AfterFewBreathlessMoments".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.GrabHimTheBalls".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.NuzzlingSloppyCock".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.WrenchHeadBackDragging".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.WouldNodIfWerent".Localized());

            Player.RefillHunger(80);

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.BothSagBackSated".Localized());

            OutputText("Scenes.Incubusmechanicscenes.SuckIncubusDick.StumblingOntoElevator".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Corruption, 5f), new DynStat(GameStats.Lust, 100f));

        }

        public void BeatDaMechanic(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicLastGreet, MECHANIC_FOUGHT);

            OutputText("Scenes.Incubusmechanicscenes.BeatDaMechanic.MechanicCollapsesOveralls".Localized());

            if (hpVictory)
                OutputText("Scenes.Incubusmechanicscenes.BeatDaMechanic.FallingOffProneForm".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.BeatDaMechanic.HangingOpenAsReaches".Localized());

            OutputText("Scenes.Incubusmechanicscenes.BeatDaMechanic.SurprisinglyHumanLikeThat".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Kill".Localized(), KillMechanic);

            AddButton(1, "Common.Button.LetGo".Localized(), () => LetMechanicGo(hpVictory));

            if (Player.HasCock())
                AddButton(2, "Common.Button.ButtFuck".Localized(), () => ButtfuckTheMechanic(hpVictory));

            if (Player.HasVagina())
                AddButton(3, "Common.Button.RideCock".Localized(), RideMechanicsCock);
            // 原版 IncubusMechanicScenes.hx:572 — IncubusMechanicScenes.hx:572 — 穿着修女服
            if (Player.Armor.Id == ArmorIds.NaughtyNunsHabit)
                AddButton(4, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), null);
        }

        public void KillMechanic()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicFightResult, MECHANIC_KILLED);

            OutputText("Scenes.Incubusmechanicscenes.KillMechanic.ProvesBlessedlyEasyKill".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void LetMechanicGo(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicFightResult, MECHANIC_RELEASED);

            OutputText("Scenes.Incubusmechanicscenes.LetMechanicGo.FeelingMercifulPoint".Localized());

            OutputText("Scenes.Incubusmechanicscenes.LetMechanicGo.TheIncubusWord".Localized());

            if (hpVictory)
                OutputText("Scenes.Incubusmechanicscenes.LetMechanicGo.SlowlyStrugglesToFeet".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.LetMechanicGo.ReluctantlyStops".Localized());

            OutputText("Scenes.Incubusmechanicscenes.LetMechanicGo.NoddingSkinCracks".Localized());

            OutputText("Scenes.Incubusmechanicscenes.LetMechanicGo.MechanicFliesAway".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void ButtfuckTheMechanic(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicFightResult, MECHANIC_FUCKED);

            var x = Player.CockThatFits(200);

            if (x == -1)
                x = 0;

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.MayHaveDoneHis".Localized());

            if (!hpVictory)
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.ThrobbingHard".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.GentlyBobbing".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.DickButYouveGot".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.OneWord".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.ItWord".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.InHandPumpingSlowly".Localized());

            if (hpVictory)
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.InNeedAnd".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.ButWord".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.HisCockGetsEven".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.GrabbingHoldOfHim".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.LastDitchAttempt".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.SmackHandFalls".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.GrabWithOneHand".Localized());

            if (silly)
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.TabCWithSlot".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.EagerToolWithCorrupted".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.SteadilyPushAtFirst".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.NowSlickenedPucker".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.InstructAsYouSqueeze".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.EnjoyMuscularTightness".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.DemonHasTheAudacity".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.NoHesNotGoing".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.ActuallyFeelYourNearly".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.SwitchingToOneHanded".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.StuttersBetweenShudders".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.OwnPleasureRising".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.DickFloppingDiminishes".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.SpentDemonFinishes".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() <= 250)
            {
                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.YouPullFourth".Localized());

                if (Player.HasKnot(x))
                    OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.PoppingYourKnotFourth".Localized());

                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.OutAfterMeagerLoad".Localized());
            }
            else
                if (Player.CumQ() <= 1000)
                {
                    OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.YouPullAgain".Localized());

                    if (Player.HasKnot(x))
                        OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.PoppingYourKnotAgain".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.OutOfTheCreampied".Localized());
                }
                else
                    if (Player.CumQ() <= 2500)
                    {
                        OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.YouPullThird".Localized());

                        if (Player.HasKnot(x))
                            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.PoppingYourKnotThird".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.OutOfTheWell".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.YouPullFourth".Localized());

                        if (Player.HasKnot(x))
                            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.PoppingYourKnotFourth".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.OutAfterMassiveBelly".Localized());

                        if (Player.CumQ() <= 5000)
                            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.PudgyWord".Localized());
                        else
                            if (Player.CumQ() <= 10000)
                                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.SwollenWord".Localized());
                            else
                                OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.AbsolutelyGravid".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.DistendedWeightOf".Localized());
                    }

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.SlumpsDownInto".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ButtfuckTheMechanic.WipeYourOff".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void RideMechanicsCock()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicFightResult, MECHANIC_FUCKED_YOU);

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.HeDoesHaveQuite".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.AbruptlyDropYourself".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.TailDrapedAcrossFace".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.CasuallyBeginUndress".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.CallOverYourShoulder".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.DemonsVoiceComesBack".Localized());

            if (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties))
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.GigglingYouAnswer".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.LanguidlyStroking".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCock.WhatKindOfDick".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Demon".Localized(), () => RideMechanicsCockII(MECHANIC_DEMON_COCK));

            AddButton(1, "Common.Button.Horse".Localized(), () => RideMechanicsCockII(MECHANIC_HORZ_GOG));

            AddButton(2, "Common.Button.Dog".Localized(), () => RideMechanicsCockII(MECHANIC_DOG_COCK));
        }

        public void RideMechanicsCockII(int cType)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3MechanicCockTypeSelection, cType);

            if (cType == MECHANIC_DEMON_COCK)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GivingSweatyCock".Localized());
            else
                if (cType == MECHANIC_HORZ_GOG)
                {
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GivingSweatyTool".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BreathlesslySays".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SquirmsUnderYou".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FinalWhimperOfRelief".Localized());

                    CombatController.Instance!.Context.Enemy.Cocks[0].Length = 22;

                    CombatController.Instance!.Context.Enemy.Cocks[0].Thickness = 4;

                    CombatController.Instance!.Context.Enemy.Cocks[0].Id = BodyPartTypes.Horse;
                }
                else
                    if (cType == MECHANIC_DOG_COCK)
                    {
                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GivingSweatyToolAgain".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BreathlesslyResponds".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FrownHimLookingOver".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SmilingReachDown".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GiveYourPetEncouraging".Localized());

                        CombatController.Instance!.Context.Enemy.Cocks[0].Length = 12;

                        CombatController.Instance!.Context.Enemy.Cocks[0].Thickness = 2;

                        CombatController.Instance!.Context.Enemy.Cocks[0].KnotMultiplier = 3;

                        CombatController.Instance!.Context.Enemy.Cocks[0].Id = BodyPartTypes.Dog;
                    }

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.DareNotDelay".Localized(
                CombatController.Instance!.Context.Enemy.CockDescriptShort()
            ));

            if (Player.Wetness() <= 2)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FreeFlowingPussyjuice".Localized());
            else
                if (Player.Wetness() <= 3)
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.OhSoJuicyVagina".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.LubricantDripOver".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.StuffTwoFingers".Localized(
                CombatController.Instance!.Context.Enemy.CockDescriptShort()
            ));

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GoSlackYouPress".Localized(
                CombatController.Instance!.Context.Enemy.CockHead()
            ));

            var mCockArea = CombatController.Instance!.Context.Enemy.CockArea(0);

            var pCuntArea = Player.VaginalCapacity();

            var tightFit = false;

            if (pCuntArea >= mCockArea * 1.5f)
            {
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FeelGirthyBonerStart".Localized());

                if (cType == MECHANIC_HORZ_GOG)
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BellyBulgingEquine".Localized());
                else
                    if (cType == MECHANIC_DOG_COCK)
                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.LipsSlurpingNoisily".Localized());

                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SentencePeriod".Localized());
            }
            else
                if (pCuntArea >= (mCockArea * 0.95f))
                {
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FeelGirthyBonerSlowly".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort()
                    ));

                    if (cType == MECHANIC_HORZ_GOG)
                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BellyBulgingEquineAgain".Localized());
                    else
                        if (cType == MECHANIC_DOG_COCK)
                            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.LipsSlurpingNoisilyAgain".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SentencePeriodAgain".Localized());
                }
                else
                    if (pCuntArea >= (mCockArea * 0.5f))
                    {
                        tightFit = true;

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FeelGirthySlowlyAgain".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort()
                        ));

                        if (cType == MECHANIC_HORZ_GOG)
                            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BellyBulgingEquineThird".Localized());
                        else
                            if (cType == MECHANIC_DOG_COCK)
                                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.LipsSlurpingNoisilyThird".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SentencePeriodThird".Localized());
                    }
                    else
                    {
                        tightFit = true;

                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FeelImmenselyGirthy".Localized(
                            CombatController.Instance!.Context.Enemy.CockHead()
                        ));
                    }

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), false, false, false);

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.HoldYourselfAtopHim".Localized(
                CombatController.Instance!.Context.Enemy.CockDescriptShort()
            ));

            if (tightFit)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.WellStretched".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.StuffedWord".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.TwatToSilenceHim".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.MuseOutLoudLift".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryIncubusBribed) > 0)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.NothingButHentaiMags".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SayEnd".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SighsAndNodsYou".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.PussySpasmsWithPleasure".Localized(
                Player.Mf("gasp", "giggle"),

                CombatController.Instance!.Context.Enemy.CockDescriptShort()
            ));

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.DemonGruntsDickSwells".Localized(
                CombatController.Instance!.Context.Enemy.CockDescriptShort()
            ));

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GodsClitSoHard".Localized());

            if (Player.GetClitLength() >= 5)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.GrabHoldOfIt".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.ThumbAtItRubbing".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.OnlyReactionIsGasp".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.AskTheQuestion".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.DickSuddenlyExpands".Localized());

            if (cType == MECHANIC_HORZ_GOG)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.HeadFlaringToSeal".Localized());
            else
                if (cType == MECHANIC_DOG_COCK)
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.KnotPoppingIn".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.JismBeginsToShoot".Localized());

            if (!Player.IsPregnant())
            {
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.EmptyWombDrinksUp".Localized());

                if (cType == MECHANIC_HORZ_GOG)
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.AfterAllBestialHorse".Localized());
                else
                    if (cType == MECHANIC_DOG_COCK)
                        OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.ImmenseKnotDoesnt".Localized());

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);
            }
            else
            {
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BlockedWombDeflects".Localized());

                if (cType != MECHANIC_DOG_COCK)
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.DripOutOverBalls".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.AndStretchYouFurther".Localized());
            }

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.FeelingTriggersClimax".Localized(
                CombatController.Instance!.Context.Enemy.CockDescriptShort()
            ));

            if (Player.Wetness() < 4)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.EvenThoughYouArent".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.PleasureIsSoGreat".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.InseminationWord".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.TrystWord".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.SentencePeriodFourth".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.BothCumWhatFeels".Localized());

            if (cType == MECHANIC_DOG_COCK)
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.WhenTryToRise".Localized());
            else
                OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.WhenRiseItAmazes".Localized(
                    CombatController.Instance!.Context.Enemy.CockHead()
                ));

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.HesADemonMaybe".Localized());

            OutputText("Scenes.Incubusmechanicscenes.RideMechanicsCockII.ShakingHeadClearAway".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void MechanicFuckedYouUp(bool hpVictory, bool pcCameWorms = false)
        {

            if (Player.HasCock() && !Player.HasVagina())
                MaleLossToMechanic(hpVictory);
            else
                ErrybodyelseLossToMechanic(hpVictory);
        }

        public void MaleLossToMechanic(bool hpVictory)
        {
            ClearOutput();

            if (!hpVictory)
            {
                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.YourCockQuiveringWildly".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.PluralS".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.JuttingOutEnticingly".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.LeersSmilingDown".Localized());

                if (Player.BiggestCockLength() < 6)
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.IncubusSighs".Localized());
                else
                    if (Player.BiggestCockLength() < 12)
                        OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.IncubusShrugs".Localized());
                    else
                        if (Player.BiggestCockLength() < 18)
                            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.PleasantlySurprised".Localized());
                        else
                        {
                            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.AsksDisdainfullyPulls".Localized());

                            Player.Cocks[Player.BiggestCockIndex()].Length = 18;

                            Player.Cocks[Player.BiggestCockIndex()].Thickness = 3;
                        }

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.HumpAgainstYourPalm".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.YourWord".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.AWord".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.LengthInsideAnyHole".Localized());
            }
            else
            {
                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.DroppingProneGasp".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.LeeringDownAtYou".Localized());

                if (Player.BiggestCockLength() < 6)
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.IncubusSighsAgain".Localized());
                else
                    if (Player.BiggestCockLength() < 12)
                        OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.IncubusShrugsAgain".Localized());
                    else
                        if (Player.BiggestCockLength() < 18)
                            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.PleasantlySurprisedAgain".Localized());
                        else
                        {
                            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.AsksDisdainfullyAgain".Localized());

                            Player.Cocks[Player.BiggestCockIndex()].Length = 18;

                            Player.Cocks[Player.BiggestCockIndex()].Thickness = 3;
                        }

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.StrokesFinelyGroomed".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.BiologicalNeedOverwhelms".Localized());
            }

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.TaintedMechanicExplains".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.ReachesIntoPocket".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.GathersYourTogether".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.RingForcedDown".Localized());

                var allDemon = false;

                if (Player.CountCocksOfType("DEMON") == Player.Cocks.Count)
                    allDemon = true;

                if (!allDemon)
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.SkinOfTransforming".Localized());

                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.ChucklesEasilyPushing".Localized());
            }
            else
            {
                OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.GrabsYourInHis".Localized());

                if (Player.CountCocksOfType("DEMON") == 0)
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.GleamingRingSliding".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.GleamingRingSlidingAgain".Localized());
            }

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.RatherLetYouPlay".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.ReachesDownToStroke".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.AtThisPointTossed".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanic.HearAndFeelRumbling".Localized());

            DoNext(() => MaleLossToMechanicII());
        }

        public void MaleLossToMechanicII()
        {
            ClearOutput();

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.AgonizinglyPerfectPussy".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.YouGasp".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.LaughingFairVoice".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.WeepingOpenlyFocus".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.SheCumsSplattering".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.YouProtest".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.StopsInHerTracks".Localized());

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicII.VelvetyTunnelWraps".Localized());

            DoNext(() => MaleLossToMechanicIII());
        }

        public void MaleLossToMechanicIII()
        {
            ClearOutput();

            OutputText("Scenes.Incubusmechanicscenes.MaleLossToMechanicIII.ChampionFinallyCums".Localized());

            GameOver();
        }

        public void ErrybodyelseLossToMechanic(bool hpVictory)
        {
            ClearOutput();

            if (!Player.HasVagina() && !hpVictory)
            {
                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.StaggerAboutDrunkenly".Localized());

                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.DemonUncapsBottle".Localized());

                Player.CreateVagina();

                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.ParagraphBreak".Localized());
            }
            else
                if (!Player.HasVagina() && hpVictory)
                {
                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.CollapsingOntoGround".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.AdmitsAsHeKneels".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.MakesYourTingle".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.OverallCladCorruptor".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.WhileDistractedUnhooked".Localized());

                    Player.CreateVagina();

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.GodsYourPussyWet".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.LicksTurnToSucks".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SlippingInsideToThrust".Localized());
                    else
                    {
                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.PinchingTuggingOnAgain".Localized());

                        if (Player.TotalNipples() > Player.BreastRows.Count())
                            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.NippleAfterNipple".Localized());
                        else
                            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.YourNipplesAfterAnother".Localized());
                    }

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.AllThingsMustCome".Localized());

                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.MewlOfDisappointment".Localized());
                }
                else
                    if (Player.HasVagina() && !hpVictory)
                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.StaggerAboutDrunkenlyAgain".Localized());
                    else
                    {
                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.CollapsingOntoGroundAgain".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.AdmitsKneelsAgain".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.MakesYourWetWetter".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.OverallCladCorruptorAgain".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.GodsPussyWetAgain".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.LicksTurnSucksAgain".Localized());

                        if (Player.HasFuckableNipples())
                            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SlippingInsideThrustAgain".Localized());
                        else
                        {
                            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.PinchingTuggingOnAgain".Localized());

                            if (Player.TotalNipples() > Player.BreastRows.Count())
                                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.NippleAfterNippleAgain".Localized());
                            else
                                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.YourNipplesAnotherAgain".Localized());
                        }

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.AllThingsComeAgain".Localized());

                        OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.MewlOfDisappointmentAgain".Localized());
                    }

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.TwistedCreaturePromises".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.LuckilyHeStops".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SighsInWhisperyPleasure".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SourceOfYourLust".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.RCoilsDontSore".Localized());
            else
                if (!Player.IsBiped())
                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.DontEvenNeedSupport".Localized());
                else
                    OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.KneesDontGetSore".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.HowWonderfulYouMove".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SkillfulOralService".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.AffectionatelyTussles".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.PleadRedRimmedEyes".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.ShakingHisHeadSlowly".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.WhatSoThisDelicious".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.DemonMustBeAble".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.AlreadyFeelTheFirst".Localized());

            if (Player.GetClitLength() > 5)
                OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.EvenMakesIt".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.DripsLongStickyStrands".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.HearYourPussyJuices".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.CruellySaysHeSteps".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.DemonAsks".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.OpenMouthIntoAn".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.GoodSays".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.LargeMushroomCrown".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SoonTheThickTool".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.FondlyRunsFingers".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.DemonsNutsSweaty".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.MechanicAnnouncesRuts".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.SuckHarderQuivering".Localized());

            OutputText("Scenes.Incubusmechanicscenes.ErrybodyelseLossToMechanic.YoullAnythingToKeep".Localized());

            GameOver();
        }

    }
}