using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<AkbalScene>("coc:akbal")]
    public class AkbalScene : Scene
    {

        public const int AKBAL_EVENT_GOBLIN = (int)AkbalEvent.Goblin;

        public const int AKBAL_EVENT_ALICE = (int)AkbalEvent.Alice;

        public const int AKBAL_EVENT_KITSUNE1 = (int)AkbalEvent.Kitsune1;

        public const int AKBAL_EVENT_KITSUNE2 = (int)AkbalEvent.Kitsune2;

        public const int AKBAL_QUEST_BADEND = (int)AkbalEvent.QuestBadEnd;

        public const int AKBAL_QUEST_DONE = (int)AkbalEvent.QuestDone;

        internal static void AkbalQuestWinHook() => new AkbalScene().AkbalQuestConclusionWin();

        internal static void AkbalQuestLoseHook() => new AkbalScene().AkbalQuestConclusionLose();

        public void StartAkbalUnsealedFight()
        {
            var akbal = EnemyRegistry.Create(EnemyIds.AkbalUnsealed);

            akbal.OnDefeated = (hpVictory) => AkbalQuestConclusionWin();

            akbal.OnWon = (hpVictory) => AkbalQuestConclusionLose();

            CombatScene.Start(akbal, () => Go<CampScene>());
        }

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
        {
            ["nude"] = "bare, exposed body",
            ["straps"] = "solid black stockings and leather chest-wraps",
            ["leaf"] = "leafy wrappings",
            ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
        };

        public static SaveContent Save => SaveContent(SceneIds.Forest.Akbal);

        public override void Entry()
        {
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            RegisterMacro("timesraped", Save.GetInt("timesRaped", 0));

            RegisterMacro("timessubmitted", CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter));

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) > 0)
            {
                var eventsRemaining = new List<int>();

                if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & AKBAL_EVENT_GOBLIN) == 0)
                    eventsRemaining.Add(0);

                if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & AKBAL_EVENT_ALICE) == 0)
                    eventsRemaining.Add(1);

                if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & AKBAL_EVENT_KITSUNE1) == 0)
                    eventsRemaining.Add(2);

                if (eventsRemaining.Count > 0)
                    switch (eventsRemaining[Random.Shared.Next(eventsRemaining.Count)])
                    {
                        case 0:
                            {
                                AkbalEventGoblin();
                            }

                            break;

                        case 1:
                            {
                                AkbalEventAlice();
                            }

                            break;

                        case 2:
                            {
                                AkbalEventKitsune();
                            }

                            break;
                    }
                else
                    AkbalQuestConclusionPrompt();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalBitchQ) >= 2)
            {
                AkbitchEncounter();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionState) == 2)
            {
                RepeatAkbalPostSubmission();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionState) == 1)
            {
                AckbalRepeatAfterWin();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionState) == -1)
            {
                AckbalRepeatAfterLoss();

                return;
            }

            AkbalFirstEncounter();
        }

        public void AkbalDefeated(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalSubmissionState, 1);

            ClearButtons();

            OutputText("Scenes.Akbal.AkbalDefeated.FallsYetGrowl".Localized());

            OutputText("Scenes.Akbal.AkbalDefeated.WalkAroundForm".Localized());

            AddButton(0, "Scenes.Akbal.AkbalDefeated.ButtonButtFuck".Localized(), RapeAkbal);

            AddButton(1, "Common.Button.TakeVaginally".Localized(), GirlsRapeAkbal);

            AddButton(2, "Scenes.Akbal.AkbalDefeated.ButtonForceLick".Localized(), RapeAkbalForcedFemaleOral);

            if (Player.Armor?.Id == ArmorIds.NaughtyNunsHabit)
                AddButton(4, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), () => Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.CockWorship());

            AddButton(14, "Common.Button.Leave".Localized(), () => CombatScene.CleanupAfterCombat());
        }

        public void AkbalWon(bool hpVictory, bool pcCameWorms = false)
        {

            CoC.Instance.Flags.Set(GameFlagIds.AkbalSubmissionState, -1);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalBitchQ, 0);

            Save.SetInt("timesRaped", Save.GetInt("timesRaped", 0) + 1);

            if (pcCameWorms)
                OutputText("Scenes.Akbal.AkbalWon.FoeNotDisgusted".Localized());

            if (hpVictory)
                HealthLoss();
            else
                LoseToAckballllllz();
        }

        public void AkbalFirstEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalFirstEncounter.TwigSnaps".Localized());

            OutputText("Scenes.Akbal.AkbalFirstEncounter.BlinksReleasesTrance".Localized());

            OutputText("Scenes.Akbal.AkbalFirstEncounter.CirclesYouVoice".Localized());

            OutputText("Scenes.Akbal.AkbalFirstEncounter.IAmAkbal".Localized());

            OutputText("Scenes.Akbal.AkbalFirstEncounter.AuraNotGodlike".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), SuperAkbalioTalk);

            AddButton(1, "Common.Button.Fight".Localized(), StartuAkabalFightomon);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void RapeAkbalForcedFemaleOral()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalBitchQ, CoC.Instance.Flags.Get(GameFlagIds.AkbalBitchQ) + 1);

            if (Player.HasTailInsteadOfLegs())
            {
                ShowImage("akbal-deepwoods-naga-forcedfemaleoral");

                OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.SlitherAround".Localized());

                OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.UncoilBump".Localized());

                OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.EyesWidenBarbs".Localized());

                OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.ScentQuivers".Localized());

                OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.BeginsLiftUp".Localized());
            }
            else
                if (Player.IsTaur())
                {
                    ShowImage("akbal-deepwoods-taur-forcedfemaleoral");

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.GrabScruffGut".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.SnarlsShoveBack".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.HeadOfMassive".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.WalkAroundIrritated".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.MuffledYelp".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.DrillsTongue".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.MakeYouCum".Localized());
                }
                else
                {
                    ShowImage("akbal-deepwoods-forcedfemaleoral");

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.GrabScruffGutAgain".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.GruntsSmash".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.EyesWidenBarbsAgain".Localized());

                    OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.ScentQuiversAgain".Localized());
                }

            OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.SadisticLaughRide".Localized());

            OutputText("Scenes.Akbal.RapeAkbalForcedFemaleOral.SnarlsLeave".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void RapeAkbal()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalBitchQ, CoC.Instance.Flags.Get(GameFlagIds.AkbalBitchQ) + 1);

            var primary = Player.CockThatFits(50);

            if (primary < 0)
                primary = 0;

            if (Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Akbal.RapeAkbal.SlitherAround".Localized());

                Player.Orgasm("Dick");

                ShowImage("akbal-deepwoods-male-naga-rapeakbal");

                if (Player.Cocks[primary].Length <= 7)
                    OutputText("Scenes.Akbal.RapeAkbal.UncoilBump".Localized(
                         Player.CockDescript(primary),

                         Player.CockDescript(primary),

                         Player.CockDescript(primary)
                    ));
                else
                    if (Player.Cocks[primary].Length <= 12)
                    {
                        OutputText("Scenes.Akbal.RapeAkbal.UncoilBumpAgain".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.Akbal.RapeAkbal.ShoveTightPucker".Localized(
                             Player.CockDescript(primary),

                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.Akbal.RapeAkbal.DozenSlowThrusts".Localized(
                             Player.CockDescript(primary)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Akbal.RapeAkbal.UncoilBumpThird".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.Akbal.RapeAkbal.SingleFinger".Localized());

                        OutputText("Scenes.Akbal.RapeAkbal.TwistTailWhimper".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.Akbal.RapeAkbal.HoursResistanceAgain".Localized());
                    }

                OutputText("Scenes.Akbal.RapeAkbal.RapeTightHole".Localized());

                if (Player.CumQ() > 1000)
                {
                    OutputText("Scenes.Akbal.RapeAkbal.CrushingTightness".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.CockContinuesPump".Localized());
                }
                else
                    OutputText("Scenes.Akbal.RapeAkbal.CrushingTightnessAgain".Localized());

                OutputText("Scenes.Akbal.RapeAkbal.StandGatherArmorAgain".Localized());

                OutputText("Scenes.Akbal.RapeAkbal.LeaveSnarls".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                CombatScene.CleanupAfterCombat();

                return;
            }
            else
                if (Player.IsTaur())
                {
                    OutputText("Scenes.Akbal.RapeAkbal.GrabScruffGut".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.WhosGonnaSubmit".Localized());

                    Player.Orgasm("Dick");

                    ShowImage("akbal-deepwoods-male-taur-rapeakbal");

                    if (Player.Cocks[primary].Length <= 7)
                    {
                        OutputText("Scenes.Akbal.RapeAkbal.SnarlsPitifully".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.Akbal.RapeAkbal.RearUpHindLegsThird".Localized(
                             Player.CockDescript(primary)
                        ));

                        OutputText("Scenes.Akbal.RapeAkbal.TightHoleClamps".Localized(
                             Player.CockDescript(primary),

                             Player.CockDescript(primary)
                        ));
                    }
                    else
                        if (Player.Cocks[primary].Length <= 12)
                        {
                            OutputText("Scenes.Akbal.RapeAkbal.SnarlsPitifullyAgain".Localized(
                                 Player.CockDescript(primary)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.RearUpHindLegsThird".Localized(
                                 Player.CockDescript(primary)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.ShoveForwardPucker".Localized(
                                 Player.CockDescript(primary),

                                 Player.CockDescript(primary)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.DozenSlowThrustsAgain".Localized(
                                 Player.CockDescript(primary)
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Akbal.RapeAkbal.SnarlsPitifullyThird".Localized(
                                 Player.CockDescript(primary),

                                 Player.CockDescript(primary)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.RearUpHindLegsThird".Localized(
                                 Player.CockDescript(primary)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.DemonWhinesDigs".Localized(
                                 Player.CockDescript(primary)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.GrinTakeWhile".Localized());

                            OutputText("Scenes.Akbal.RapeAkbal.HoursResistanceAgain".Localized());
                        }

                    OutputText("Scenes.Akbal.RapeAkbal.RapeTightHoleAgain".Localized());

                    if (Player.CumQ() > 1000)
                    {
                        OutputText("Scenes.Akbal.RapeAkbal.CrushingTightnessThird".Localized());

                        OutputText("Scenes.Akbal.RapeAkbal.ContinueSlideOrgan".Localized());

                        OutputText("Scenes.Akbal.RapeAkbal.OrgasmSubsidesPullOut".Localized());
                    }
                    else
                        OutputText("Scenes.Akbal.RapeAkbal.CrushingTightnessFourth".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.StandGatherArmorAgain".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.LeaveSnarlsAgain".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                    CombatScene.CleanupAfterCombat();

                    return;
                }
                else
                {
                    OutputText("Scenes.Akbal.RapeAkbal.GrabScruffGutAgain".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.WhosGonnaSubmitAgain".Localized());

                    Player.Orgasm("Dick");

                    ShowImage("akbal-deepwoods-male-rapeakbal");

                    OutputText("Scenes.Akbal.RapeAkbal.GruntsSmash".Localized());

                    if (Player.CockArea(0) < 13)
                        OutputText("Scenes.Akbal.RapeAkbal.FirstPokeFinger".Localized(
                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));
                    else
                        if (Player.CockArea(0) < 25)
                        {
                            OutputText("Scenes.Akbal.RapeAkbal.LightTapConstrict".Localized(
                                 Player.CockDescript(0)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.DozenSlowThrustsThird".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Akbal.RapeAkbal.WontFit".Localized());

                            OutputText("Scenes.Akbal.RapeAkbal.TwistTailWhimperAgain".Localized(
                                 Player.CockDescript(0)
                            ));

                            OutputText("Scenes.Akbal.RapeAkbal.HoursResistanceThird".Localized());
                        }

                    OutputText("Scenes.Akbal.RapeAkbal.RapeTightHoleThird".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.CrushingTightnessFifth".Localized());

                    if (Player.CumQ() > 1000)
                    {
                        OutputText("Scenes.Akbal.RapeAkbal.ContinueSlideOrganAgain".Localized());

                        OutputText("Scenes.Akbal.RapeAkbal.OrgasmSubsidesPullOutAgain".Localized());
                    }
                    else
                        OutputText("Scenes.Akbal.RapeAkbal.BodyGoesLimp".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.StandGatherArmorThird".Localized());

                    OutputText("Scenes.Akbal.RapeAkbal.LeaveSnarlsThird".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                    CombatScene.CleanupAfterCombat();
                }
        }

        public void GirlsRapeAkbal()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalBitchQ, CoC.Instance.Flags.Get(GameFlagIds.AkbalBitchQ) + 1);

            OutputText("Scenes.Akbal.GirlsRapeAkbal.SmirkQuietly".Localized());

            ShowImage("akbal-deepwoods-female-bindakbal");

            if (Player.IsTaur())
            {
                OutputText("Scenes.Akbal.GirlsRapeAkbal.SeeTheTrouble".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbal.BeforeRecoverBind".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbal.LegsBoundExamine".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbal.VinesHoldSlowly".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.GirlsRapeAkbal.HangingTwitching".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbal.SmirkHelpless".Localized());
            }
            else
                if (Player.VaginalCapacity() < 37.5f)
                {
                    OutputText("Scenes.Akbal.GirlsRapeAkbal.RollingOntoBack".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.GrabTail".Localized());

                    if (Player.Strength > 60)
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.HaulingLikeKittenAgain".Localized());
                    else
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.DraggingToTreeAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.ForcingLeanBackAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.GrippingTailScruff".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.CurlTailTip".Localized());
                    else
                        if (Player.IsGoo())
                            OutputText("Scenes.Akbal.GirlsRapeAkbal.ExtendLiquidBody".Localized());
                        else
                            OutputText("Scenes.Akbal.GirlsRapeAkbal.LiftLegFoot".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.StuffOwnTail".Localized());
                }
                else
                {
                    OutputText("Scenes.Akbal.GirlsRapeAkbal.GrinFlipOver".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.RememberCats".Localized());

                    if (Player.HasPerk(PerkIds.Flexibility))
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.OwnExperience".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.FigureGoodIdea".Localized());

                    if (Player.Strength > 60)
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.HaulingLikeKittenAgain".Localized());
                    else
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.DraggingToTreeAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.ForcingLeanBackAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.DazedLittleResistance".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.MomentPityStroke".Localized());
                    else
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.SmirkGrabSack".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.FinallyJudgeDone".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Akbal.GirlsRapeAkbal.RearUpTailSupport".Localized());
                    else
                        if (Player.IsGoo())
                            OutputText("Scenes.Akbal.GirlsRapeAkbal.ReachLiquidCoating".Localized());
                        else
                            OutputText("Scenes.Akbal.GirlsRapeAkbal.StandOverSpreadLegs".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbal.JudgingShudder".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            DoNext(() => GirlsRapeAkbalPart2());
        }

        public void GirlsRapeAkbalPart2()
        {
            ClearOutput();

            ShowImage("akbal-deepwoods-female-taur-bindakbal");

            if (Player.IsTaur())
            {
                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.MoanDeepShaft".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SwingingBackForth".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.DemonGroansClench".Localized());

                if (Player.Corruption > 33)
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.IfNotSoEager".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SettleIntoRhythm".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.ContinueHour".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.AkbalJustDesperate".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.CockSlapsStomach".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.BuriesDeepBalls".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SwayingBackToMeet".Localized());

                    if (Player.BallSize >= 12)
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.DwarfingHim".Localized());
                }

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.CoaxingMoans".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.JaguarFinishesFirst".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.TwitchLetLoose".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart2.PeriodBreakAgain".Localized());

                OutputText("Scenes.Akbal.GirlsRapeAkbalPart.BodyTremblesSags".Localized());

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 101);

                Player.CuntChange(37.5f, true, true, false);

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            }
            else
                if (Player.VaginalCapacity() < 37.5f)
                {
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SilkyFurSlick".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.UpDownFeline".Localized());
                    else
                        if (Player.IsGoo())
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SquirmWritheBase".Localized());
                        else
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.PressShaftFirmer".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SilentPayment".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.TimeStretches".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.StrokeInTime".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.FlexibleTailPokes".Localized(
                         Player.Mf("Ruler of Beasts", "Queen of Beasts")
                    ));

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SquirmingTailTooMuch".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.StrokingRoughly".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.MoaningLipsClench".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.OffersBounty".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.AddedHeatWetness".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.ChestHeaves".Localized());

                    if (Player.CumQ() > 500)
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.WaistCoated".Localized());
                    else
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SitsDazed".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SagsAgainstTrunk".Localized());

                    Player.Orgasm("Vaginal");
                }
                else
                {
                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.GroanShaftStretches".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.LiftOffHeadSwim".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.FlopFishHalf".Localized());
                    else
                        if (Player.IsGoo())
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.EncaseHips".Localized());
                        else
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.LockLegsTorso".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.UseGroinRoughly".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.WrapStrainingShaft".Localized());
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.WrapEachMalehood".Localized());
                        else
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.StrokeTickleAss".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart2.PeriodBreakAgain".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.NotSureHowLong".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.TwitchesReadyBlow".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.DoesntDisappoint".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SettingOwnMalehood".Localized());
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SettingEachMalenesses".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.FaceTwistedSnarl".Localized());

                    OutputText("Scenes.Akbal.GirlsRapeAkbalPart.SlowlyComeDown".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 101);
                }

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToAckballllllz()
        {
            ClearOutput();

            if (!Player.IsTaur())
            {
                ShowImage("akbal-deepwoods-losslust-analed");

                OutputText("Scenes.Akbal.LoseToAckballllllz.FallToKneesMasturbate".Localized());

                OutputText("Scenes.Akbal.LoseToAckballllllz.WhoreInHeatLick".Localized());

                OutputText("Scenes.Akbal.LoseToAckballllllz.DefianceRepaid".Localized());
            }
            else
                if (Player.IsTaur())
                {
                    ShowImage("akbal-deepwoods-losslust-taur-analed");

                    OutputText("Scenes.Akbal.LoseToAckballllllz.StumbleDrunkenPony".Localized());

                    OutputText("Scenes.Akbal.LoseToAckballllllz.LongCockSwings".Localized());

                    OutputText("Scenes.Akbal.LoseToAckballllllz.DefianceRepaidAgain".Localized());

                    if (Player.Anal.Looseness < 3)
                    {
                        OutputText("Scenes.Akbal.LoseToAckballllllz.MountsPushBarbs".Localized());

                        OutputText("Scenes.Akbal.LoseToAckballllllz.ShovesStrain".Localized());

                        OutputText("Scenes.Akbal.LoseToAckballllllz.HoursSlowForced".Localized());
                    }
                    else
                        if (Player.Anal.Looseness < 5)
                            OutputText("Scenes.Akbal.LoseToAckballllllz.MountsPushBarbsAgain".Localized());
                        else
                        {
                            OutputText("Scenes.Akbal.LoseToAckballllllz.MountsPushBarbsThird".Localized());

                            OutputText("Scenes.Akbal.LoseToAckballllllz.SnarlsSlams".Localized());
                        }

                    Player.ButtChange(37.5f, true);

                    OutputText("Scenes.Akbal.LoseToAckballllllz.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Akbal.LoseToAckballllllz.EntireLengthHums".Localized());

                    OutputText("Scenes.Akbal.LoseToAckballllllz.RoarsClimax".Localized());

                    OutputText("Scenes.Akbal.LoseToAckballllllz.HoursHisToy".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)), new DynStat(GameStats.Corruption, 5 + Random.Shared.Next(10)));

                    return;
                }

            if (Player.Anal.Looseness < 3)
            {
                OutputText("Scenes.Akbal.LoseToAckballllllz.MountsPushBarbsFourth".Localized());

                OutputText("Scenes.Akbal.LoseToAckballllllz.StrainShoving".Localized());

                OutputText("Scenes.Akbal.LoseToAckballllllz.HoursLongCatDick".Localized());
            }
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Akbal.LoseToAckballllllz.MountsPushBarbsFifth".Localized());
                else
                    OutputText("Scenes.Akbal.LoseToAckballllllz.MountsPushBarbsSixth".Localized());

            Player.ButtChange(37.5f, true);

            OutputText("Scenes.Akbal.LoseToAckballllllz.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Akbal.LoseToAckballllllz.EmbeddedCockHums".Localized());

            OutputText("Scenes.Akbal.LoseToAckballllllz.HoursHisToyAgain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)), new DynStat(GameStats.Corruption, 5 + Random.Shared.Next(10)));

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void HealthLoss()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.HealthLoss.BreathBurnsRagged".Localized());

            OutputText("Scenes.Akbal.HealthLoss.NeedToMove".Localized());

            OutputText("Scenes.Akbal.HealthLoss.StopDeadTrack".Localized());

            OutputText("Scenes.Akbal.HealthLoss.VoiceRattlesFool".Localized());

            OutputText("Scenes.Akbal.HealthLoss.EasyGraceSteps".Localized());

            OutputText("Scenes.Akbal.HealthLoss.RememberThat".Localized());

            OutputText("Scenes.Akbal.HealthLoss.WhispersFallSilent".Localized());

            OutputText("Scenes.Akbal.HealthLoss.MadeAllTheClear".Localized());

            if (Player.IsNakedLower())
                OutputText("Scenes.Akbal.HealthLoss.TailNudgesSlime".Localized());
            else
            {
                OutputText("Scenes.Akbal.HealthLoss.ClimbsUponBack".Localized());

                OutputText("Scenes.Akbal.HealthLoss.SnarlsStrip".Localized());

                OutputText("Scenes.Akbal.HealthLoss.ObeyingEasier".Localized());

                OutputText("Scenes.Akbal.HealthLoss.NotFastEnough".Localized());
            }

            OutputText("Scenes.Akbal.HealthLoss.HesGoneBreath".Localized());

            OutputText("Scenes.Akbal.HealthLoss.AirStills".Localized());

            OutputText("Scenes.Akbal.HealthLoss.NotEvenMadeIt".Localized());

            OutputText("Scenes.Akbal.HealthLoss.StrugglesCease".Localized());

            OutputText("Scenes.Akbal.HealthLoss.KnowWhatHappens".Localized());

            OutputText("Scenes.Akbal.HealthLoss.FeelItGrowing".Localized());

            OutputText("Scenes.Akbal.HealthLoss.BarbsGraze".Localized());

            OutputText("Scenes.Akbal.HealthLoss.NoAnswer".Localized());

            OutputText("Scenes.Akbal.HealthLoss.WontFitTear".Localized());

            Player.ButtChange(37.5f, true);

            OutputText("Scenes.Akbal.HealthLoss.WetWhimpers".Localized());

            OutputText("Scenes.Akbal.HealthLoss.EveryInchBurns".Localized());

            OutputText("Scenes.Akbal.HealthLoss.HisToyLocked".Localized());

            OutputText("Scenes.Akbal.HealthLoss.EachThrust".Localized());

            OutputText("Scenes.Akbal.HealthLoss.SquirmLimply".Localized());

            OutputText("Scenes.Akbal.HealthLoss.DespiteHopes".Localized());

        }

        public void SuperAkbalioTalk()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.SuperAkbalioTalk.WhatDoYouMean".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), StartuAkabalFightomon);

            AddButton(1, "Common.Button.Submit".Localized(), AkbalSubmit);
        }

        public void RepeatAkbalPostSubmission()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.RepeatAkbalPostSubmission.PurringHeard".Localized());

            OutputText("Scenes.Akbal.RepeatAkbalPostSubmission.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Submit".Localized(), AkbalSubmit);

            AddButton(1, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), AkbalDeny);

            AddButton(4, "Common.Button.Fight".Localized(), StartuAkabalFightomon);
        }

        public void AkbalDeny()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalDeny.ShakeHeadRub".Localized());

            OutputText("Scenes.Akbal.AkbalDeny.NewToyLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AckbalRepeatAfterWin()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AckbalRepeatAfterWin.SnarlLookUp".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Akbal.AckbalRepeatAfterWin.FourLeggedLeap".Localized());
            else
                OutputText("Scenes.Akbal.AckbalRepeatAfterWin.DodgingRoll".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), StartuAkabalFightomon);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void AckbalRepeatAfterLoss()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AckbalRepeatAfterLoss.ChorusLaughter".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Submit".Localized(), AkbalSubmit);

            AddButton(1, "Common.Button.Fight".Localized(), StartuAkabalFightomon);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void StartuAkabalFightomon()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.StartuAkabalFightomon.ReadyWeaponBattle".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Akbal), () => Go<CampScene>());

            CoC.Instance.Flags.Set(GameFlagIds.PlayerResistedAkbal, CoC.Instance.Flags.Get(GameFlagIds.PlayerResistedAkbal) + 1);
        }

        public void AkbalSubmit()
        {
            ClearOutput();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalSubmissionCounter, CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalSubmissionState, 2);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalBitchQ, -1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 5 && CoC.Instance.Flags.Get(GameFlagIds.PlayerResistedAkbal) < 2 && Player.Body.Butt >= 13 && Player.Tone < 80)
            {
                AkbalBigButtSubmit();

                return;
            }

            if (Player.HasTailInsteadOfLegs())
            {
                ShowImage("akbal-deepwoods-naga-sumbitanal");

                OutputText("Scenes.Akbal.AkbalSubmit.NodAfterThinking".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.PushesFaceFirst".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.ShovesFaceFlinc".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.WorksTongue".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.SuddenWarmth".Localized());

                if (Player.Anal.Looseness < 3)
                {
                    OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundFourth".Localized());

                    OutputText("Scenes.Akbal.AkbalSubmit.FeelingStretched".Localized());
                }
                else
                    if (Player.Anal.Looseness < 5)
                    {
                        OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundAgain".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmit.TitanicMemberStretches".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundThird".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmit.WeirdSpitHeats".Localized());
                    }

                Player.ButtChange(37.5f, true);

                OutputText("Scenes.Akbal.AkbalSubmit.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.WorksHipsFast".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.ReleasesGrowlFeline".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.LastEruptionPullOut".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.NoLongerMinds".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 4 + Random.Shared.Next(8)));

                Player.ApplyStats(new DynStat(GameStats.Speed, 1 + Random.Shared.Next(2)));

                Player.Orgasm("Anal");

                if (Player.Body.Butt < 8)
                {
                    OutputText("Scenes.Akbal.AkbalSubmit.SleepAssPlumpsThird".Localized());

                    Player.Body.Butt += 1;
                }

                Player.CreateStatusEffect(StatusEffectIds.PostAkbalSubmission, 0, 0, 0, 0);

                Player.Sleeping = true;

                ReturnToCamp(1);

                return;
            }

            if (Player.IsTaur())
            {
                ShowImage("akbal-deepwoods-taur-sumbitanal");

                OutputText("Scenes.Akbal.AkbalSubmit.NodAfterThinkingAgain".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.QuicklyMountsPushes".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.LookingBackBends".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbalSubmit.SlurpsDownVagina".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Akbal.AkbalSubmit.SlurpsDownSack".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.OralFinishedWarmth".Localized());

                if (Player.Anal.Looseness < 3)
                {
                    OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundFourth".Localized());

                    OutputText("Scenes.Akbal.AkbalSubmit.FeelingStretchedAgain".Localized());
                }
                else
                    if (Player.Anal.Looseness < 5)
                    {
                        OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundFifth".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmit.TitanicMemberStretchesAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundSixth".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmit.WeirdSpitHeatsAgain".Localized());
                    }

                Player.ButtChange(37.5f, true);

                OutputText("Scenes.Akbal.AkbalSubmit.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.WorksHipsFastAgain".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.NotSlowingDown".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.LastEruptionPullOutAgain".Localized());

                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Akbal.AkbalSubmit.ToJoinYourOwn".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.PeriodBreak".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.CloseEyesSleep".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 4 + Random.Shared.Next(8)));

                Player.ApplyStats(new DynStat(GameStats.Speed, 1 + Random.Shared.Next(2)));

                Player.Orgasm("Anal");

                if (Player.Body.Butt < 8)
                {
                    OutputText("Scenes.Akbal.AkbalSubmit.SleepAssPlumpsThird".Localized());

                    Player.Body.Butt += 1;
                }

                Player.CreateStatusEffect(StatusEffectIds.PostAkbalSubmission, 0, 0, 0, 0);

                Player.Sleeping = true;

                ReturnToCamp(1);

                return;
            }

            ShowImage("akbal-deepwoods-sumbitanal");

            OutputText("Scenes.Akbal.AkbalSubmit.NodAfterMinute".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.PushesFaceFirstAgain".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.YanksUpturnedBottom".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Akbal.AkbalSubmit.SlurpsDownVaginaAgain".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Akbal.AkbalSubmit.SlurpsDownSackAgain".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.OralEndsWarmth".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.PokingAroundSeventh".Localized());

            if (Player.Anal.Looseness < 3)
            {
                OutputText("Scenes.Akbal.AkbalSubmit.GritTeethExpecting".Localized());

                Player.ButtChange(37.5f, true);

                OutputText("Scenes.Akbal.AkbalSubmit.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Akbal.AkbalSubmit.BeingStretchedShudders".Localized());
            }
            else
                if (Player.Anal.Looseness < 5)
                {
                    OutputText("Scenes.Akbal.AkbalSubmit.WeirdSalivaEases".Localized());

                    Player.ButtChange(37.5f, true);

                    OutputText("Scenes.Akbal.AkbalSubmit.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Akbal.AkbalSubmit.TitanicStretchingGroan".Localized());
                }
                else
                {
                    OutputText("Scenes.Akbal.AkbalSubmit.BeginsPenetrateGroan".Localized());

                    OutputText("Scenes.Akbal.AkbalSubmit.WeirdSpitHeatsThird".Localized());
                }

            OutputText("Scenes.Akbal.AkbalSubmit.WorksHipsFastThird".Localized());

            if (Player.HasVagina() || Player.Cocks.Count > 0)
                OutputText("Scenes.Akbal.AkbalSubmit.InPoolOfYourOwn".Localized());
            else
                OutputText("Scenes.Akbal.AkbalSubmit.InSexualBliss".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.PeriodBreakAgain".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.ReleasesGrowlFelineAgain".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.SensationSettles".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.FinalEruptionPullOut".Localized());

            OutputText("Scenes.Akbal.AkbalSubmit.NoLongerMindsAgain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 4 + Random.Shared.Next(8)));

            Player.ApplyStats(new DynStat(GameStats.Speed, 1 + Random.Shared.Next(2)));

            Player.Orgasm("Anal");

            if (Player.Body.Butt < 8)
            {
                OutputText("Scenes.Akbal.AkbalSubmit.SleepAssPlumpsThird".Localized());

                Player.Body.Butt += 1;
            }

            Player.CreateStatusEffect(StatusEffectIds.PostAkbalSubmission, 0, 0, 0, 0);

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void AkbalSubmissionFollowup()
        {
            ClearOutput();

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) < 4)
                OutputText("Scenes.Akbal.AkbalSubmissionFollowup.AwakeDangerousThird".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PlayerResistedAkbal) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) >= 8 && Player.IsCorruptEnough(80))
                {
                    if (!Player.HasPerk(PerkIds.FireLord))
                    {
                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.OpensEyesAkbal".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.LoyalPet".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.NotBurning".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.DemonicPresenceLungs".Localized());

                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.NowBreatheFire".Localized());

                        Player.CreatePerk(PerkIds.FireLord, 0, 0, 0, 0);
                    }
                    else
                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.AwakeDangerousThird".Localized());
                }
                else
                    if (!Player.HasPerk(PerkIds.Whispered) && Player.IsCorruptEnough(40))
                    {
                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.AwakeChorus".Localized());

                        if (!Player.HasPerk(PerkIds.Whispered))
                        {
                            OutputText("Scenes.Akbal.AkbalSubmissionFollowup.NowWhispered".Localized());

                            Player.CreatePerk(PerkIds.Whispered, 0, 0, 0, 0);
                        }
                    }
                    else
                        OutputText("Scenes.Akbal.AkbalSubmissionFollowup.AwakeDangerousThird".Localized());

            if (Player.HasPerk(PerkIds.Whispered) && Player.HasPerk(PerkIds.FireLord))
                AwardAchievement(AchievementIds.AllahuAkbal, true, true);

            PlayerMenu();
        }

        public void AkbalBigButtSubmit()
        {
            ClearOutput();

            ShowImage("akbal-deepwoods-bigbuttanaled");

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.SmilingAnticipation".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.DeepVoiceResounds".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.PsychicSnarlApproval".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.AndBalls".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.AndVagina".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.CantHelpSquirm".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.AssInAir".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.BobbingBeneath".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.VulvaBiggerPuffier".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.TongueGoesDeep".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.SeesLegsShaking".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.GentlePokes".Localized());

            Player.ButtChange(37.5f, true, true, false);

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.EyesCross".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.CockDrooling".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.VaginaDripping".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.BlessedToolDeparts".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.FreelyUnleashing".Localized());

                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.PuddlesAroundYou".Localized());

                    if (Player.CumQ() >= 1000)
                        if (Player.CumQ() < 3000)
                            OutputText("Scenes.Akbal.AkbalBigButtSubmit.IntoSmallLake".Localized());
                        else
                            OutputText("Scenes.Akbal.AkbalBigButtSubmit.IntoSlimyMorass".Localized());
                }
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.Comma".Localized());

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Akbal.AkbalBigButtSubmit.Squirting".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbalBigButtSubmit.Leaking".Localized());

                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.IntoDirtStinks".Localized());
                }

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.Period".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.GroansDoesNotRelease".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.FirstJetSeed".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.FreshWaveSlops".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.CumAndCum".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.SatedLiftedUp".Localized());

            if (Player.HasCock() && Player.CumQ() >= 1000)
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.Dry".Localized());

            OutputText("Scenes.Akbal.AkbalBigButtSubmit.ComfortablePlaceDocks".Localized());

            if (Player.Body.Butt < 20)
            {
                OutputText("Scenes.Akbal.AkbalBigButtSubmit.TinglingButtBulges".Localized());

                Player.Body.Butt += 1;
            }
            else
                if (Player.Tone > 30)
                {
                    OutputText("Scenes.Akbal.AkbalBigButtSubmit.TinglingButtJiggles".Localized());

                    Player.ModTone(30, 5);
                }

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Player.SlimeFeed();

            Player.CreateStatusEffect(StatusEffectIds.PostAkbalSubmission, 0, 0, 0, 0);

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void AkbitchEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbitchEncounter.SoftSlurpingHeard".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AkbalBitchQ, 1);

            if ((!Player.IsCorruptEnough(40) && !Player.HasPerk(PerkIds.Pervert) && !Player.HasPerk(PerkIds.Sadist)) || Player.Lust < 33)
                AkbitchNoThnx(false);
            else
            {
                OutputText("Scenes.Akbal.AkbitchEncounter.TakeAdvantageAgain".Localized());

                ClearButtons();

                AddButton(1, "Common.Button.No".Localized(), () => AkbitchNoThnx());

                AddButton(0, "Common.Button.Yes".Localized(), TakeAdvantageOfAkbitch);
            }
        }

        public void AkbitchNoThnx(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Akbal.AkbitchNoThnx.ParagraphBreak".Localized());

            OutputText("Scenes.Akbal.AkbitchNoThnx.TurnBackLeaves".Localized());

            if (Player.Lust < 33)
                OutputText("Scenes.Akbal.AkbitchNoThnx.NotArousedAnyway".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeAdvantageOfAkbitch()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.CreepBehindTrees".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.WalkSilentFeet".Localized());
            else
                OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.SilentlySlither".Localized());

            OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.InRangeVine".Localized());

            if (Player.Strength < 50)
            {
                OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.GoutGreenFire".Localized());

                var akbal = EnemyRegistry.Create((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) > 0 ? EnemyIds.AkbalFierce : EnemyIds.Akbal));

                CombatScene.Start(akbal, () => Go<CampScene>());

                return;
            }

            OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.GoutGreenFireAgain".Localized());

            if (Player.Intelligence < 60)
            {
                OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.StopsStrugglingShout".Localized());

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.Akbal), () => Go<CampScene>());

                return;
            }

            OutputText("Scenes.Akbal.TakeAdvantageOfAkbitch.StopsStrugglingShoutAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AkbalTimesBitched, CoC.Instance.Flags.Get(GameFlagIds.AkbalTimesBitched) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.FuckHim".Localized(), null);

            AddButton(1, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonStrFuck".Localized(), null);

            AddButton(6, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonStrRide".Localized(), null);

            AddButton(2, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonSpdFuck".Localized(), null);

            AddButton(7, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonSpdRide".Localized(), null);

            AddButton(3, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonTouFuck".Localized(), null);

            AddButton(8, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonTouRide".Localized(), null);

            if (Player.HasCock())
                AddButton(0, "Common.Button.FuckHim".Localized(), ButtFuckbuttFuckbuttFuckAkbal);

            AddButton(5, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonRideHim".Localized(), TopAkbitchFromDaBottom);

            if (Player.Strength >= 70)
            {
                if (Player.HasCock())
                    AddButton(1, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonStrFuck".Localized(), () => AkbitchHighStrengthVariant(false));

                AddButton(6, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonStrRide".Localized(), () => AkbitchHighStrengthVariant(true));
            }

            if (Player.Speed >= 70)
            {
                if (Player.HasCock())
                    AddButton(2, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonSpdFuck".Localized(), () => AkbalBitchSpeed(false));

                AddButton(7, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonSpdRide".Localized(), () => AkbalBitchSpeed(true));
            }

            if (Player.Toughness >= 70)
            {
                if (Player.HasCock())
                    AddButton(3, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonTouFuck".Localized(), () => AkbitchToughness(false));

                AddButton(8, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonTouRide".Localized(), () => AkbitchToughness(true));
            }
        }

        public void ButtFuckbuttFuckbuttFuckAkbal()
        {
            ClearOutput();

            ShowImage("akbal-deepwoods-male-buttfuck");

            OutputText("Scenes.Akbal.ButtFuckAkbal.GrinTugCollar".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalTimesBitched) == 1)
                OutputText("Scenes.Akbal.ButtFuckAkbal.SmileSpreadsWider".Localized());
            else
                OutputText("Scenes.Akbal.ButtFuckAkbal.LetterH".Localized());

            OutputText("Scenes.Akbal.ButtFuckAkbal.LiftsTailView".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Akbal.ButtFuckAkbal.KneelSinkDepths".Localized());
            else
            {
                OutputText("Scenes.Akbal.ButtFuckAkbal.KneelChooseBiggest".Localized());

                OutputText("Scenes.Akbal.ButtFuckAkbal.LiftIntoCockPosition".Localized());
            }

            var x = Player.BiggestCockIndex();

            if (Player.Cocks[x].Length < 7)
                OutputText("Scenes.Akbal.ButtFuckAkbal.CroonsWondering".Localized());
            else
                if (Player.Cocks[x].Length <= 12)
                    OutputText("Scenes.Akbal.ButtFuckAkbal.GroansDeepLube".Localized());
                else
                    OutputText("Scenes.Akbal.ButtFuckAkbal.HowlsCringesStuff".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FuckAkbitchsButt);
        }

        public void FuckAkbitchsButt()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            ShowImage("akbal-deepwoods-male-buttfuck2");

            OutputText("Scenes.Akbal.FuckAkbitchsButt.VacuumTight".Localized());

            if (Player.Corruption < 90)
                OutputText("Scenes.Akbal.FuckAkbitchsButt.HoldCollarRideOut".Localized());
            else
                OutputText("Scenes.Akbal.FuckAkbitchsButt.PoundQuiveringRectum".Localized());

            OutputText("Scenes.Akbal.FuckAkbitchsButt.FamiliarPressure".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Akbal.FuckAkbitchsButt.MeetNewBitch".Localized());
            else
                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Akbal.FuckAkbitchsButt.ShaftSplitsTwo".Localized());
                else
                    if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                        OutputText("Scenes.Akbal.FuckAkbitchsButt.GoesWildHowling".Localized());
                    else
                        if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                            OutputText("Scenes.Akbal.FuckAkbitchsButt.ParticularlyFond".Localized());
                        else
                            OutputText("Scenes.Akbal.FuckAkbitchsButt.PullCollarBlast".Localized());

            OutputText("Scenes.Akbal.FuckAkbitchsButt.LookBackRegains".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TopAkbitchFromDaBottom()
        {
            ClearOutput();

            ShowImage("akbal-deepwoods-male-akbalonback");

            OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.GrinTugCollar".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalTimesBitched) == 1)
                OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.SmileSpreadsWider".Localized());
            else
                OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.LetterH".Localized());

            OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.LiftsTailView".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.LayDownGrass".Localized());
            else
            {
                OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.LayDownCollar".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.WordClit".Localized());
                else
                    OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.WordProstate".Localized());

                OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.CringeNewToy".Localized());
            }

            OutputText("Scenes.Akbal.TopAkbitchFromDaBottom.BoundLegsPull".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TopAkbitchFromBottomDuex);
        }

        public void TopAkbitchFromBottomDuex()
        {
            ClearOutput();

            var analOrgasm = false;

            ShowImage("akbal-deepwoods-male-akbalonback2");

            if ((!Player.HasVagina() && Player.Anal.Looseness < 2) || (Player.HasVagina() && Player.Vaginas[0].Looseness <= 2))
                OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.BeginImpalePole".Localized());
            else
                if ((!Player.HasVagina() && Player.Anal.Looseness < 4) || (Player.HasVagina() && Player.Vaginas[0].Looseness < 4))
                    OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.ImpaleYourselfMoan".Localized());
                else
                    OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.SlideDownRollback".Localized());

            if (Player.HasVagina())
                Player.CuntChange(37.5f, true, true, false);
            else
                Player.ButtChange(37.5f, true, true, false);

            if (Player.HasCock())
            {
                OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.WrapHandCock".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.WordBoth".Localized());

                OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.WordYourSexOrgan".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.ClimaxAtOnce".Localized());
                else
                    OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.WordClimaxes".Localized());

                OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.SpasmsAround".Localized());

                analOrgasm = true;
            }
            else
                OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.EveryNerveExplodes".Localized());

            OutputText("Scenes.Akbal.TopAkbitchFromBottomDuex.LookBackRegains".Localized());

            Player.Orgasm(analOrgasm ? "Common.Button.Anal".Localized() : "Common.Button.Vaginal".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            if (Player.HasVagina())
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 101);

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbitchHighStrengthVariant(bool ride = false)
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.WickedGrinRipArmor".Localized());

            if (Player.HasCock() && !ride)
            {
                ShowImage("akbal-deepwoods-male-highstrength");

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.LicksFaceLost".Localized());

                var x = Player.BiggestCockIndex();

                if (Player.Cocks[x].Length < 7)
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.PurrsShoveHead".Localized());
                else
                    if (Player.Cocks[x].Length <= 12)
                        OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.GroansShoveHead".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.RoarsShoved".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.BodyConvulsionsJet".Localized());

                if (Player.Cocks[x].Length > 12)
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.WordAnother".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.WordA".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.TitanicOrgasm".Localized());

                if (Player.Corruption < 90)
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.CantHelpSmile".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.FlexMuscles".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.ShuddersOrgasmEnds".Localized());

                if (Player.HasKnot(x))
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.AllStrengthSlam".Localized());
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.ThrustsDoubleForce".Localized());
                    else
                        if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.FeelPressureBuilding".Localized());
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.SlamToHilt".Localized());
                            else
                                if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
                                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.SlamFullLength".Localized());
                                else
                                    if (Player.Cocks[x].Id == BodyPartTypes.Lizard)
                                        OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.SmashHipsUpturned".Localized());
                                    else
                                        OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.RamUpturned".Localized());

                if (Player.Corruption < 90)
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.StandAllowingRelax".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.StandLegsFlop".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                ShowImage("akbal-deepwoods-female-highstrength");

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.LicksFaceLostAgain".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.BeginsTonguing".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.NetherLips".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.Entrance".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.HotSalivaFlinch".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.ShoveOntoBack".Localized());

                if ((!Player.HasVagina() && Player.Anal.Looseness < 2) || (Player.HasVagina() && Player.Vaginas[0].Looseness <= 2))
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.BeginsEnvelop".Localized());

                if ((!Player.HasVagina() && Player.Anal.Looseness < 4) || (Player.HasVagina() && Player.Vaginas[0].Looseness < 4))
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.BeginsEnvelopAgain".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.GiantErectionTouches".Localized());

                if (Player.HasVagina())
                    Player.CuntChange(37.5f, true, true, false);
                else
                    Player.ButtChange(37.5f, true, true, false);

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.HandsOnCalves".Localized());

                OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.EmbeddedDickJumps".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.VaginaExplodesJerks".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.BodyConvulseLightning".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.AkbitchHighStrengthVariant.SwellsExplodesCream".Localized());

                Player.Orgasm("VaginalAnal");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

                if (Player.HasVagina())
                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 101);

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void AkbalBitchSpeed(bool ride = false)
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalBitchSpeed.GroansFaceFirst".Localized());

            OutputText("Scenes.Akbal.AkbalBitchSpeed.ChorusCroonsMaster".Localized());

            var x = Player.BiggestCockIndex();

            if (Player.HasCock() && !ride)
            {
                Player.Orgasm("Dick");

                ShowImage("akbal-deepwoods-male-highspeed");

                OutputText("Scenes.Akbal.AkbalBitchSpeed.PullTailFaceDown".Localized());

                if (Player.Cocks[x].Length < 7)
                {
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.HandsAtHaunches".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Akbal.AkbalBitchSpeed.DripsGobsLower".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbalBitchSpeed.DripsGobsSack".Localized());

                    OutputText("Scenes.Akbal.AkbalBitchSpeed.ClenchedTeethSpur".Localized());
                }
                else
                    if (Player.Cocks[x].Length <= 12)
                        OutputText("Scenes.Akbal.AkbalBitchSpeed.PushesBackWiggle".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbalBitchSpeed.SurprisedFit".Localized());

                OutputText("Scenes.Akbal.AkbalBitchSpeed.PushingDownBack".Localized());

                OutputText("Scenes.Akbal.AkbalBitchSpeed.LiesShivering".Localized());
            }
            else
            {
                Player.Orgasm("VaginalAnal");

                ShowImage("akbal-deepwoods-female-highspeed");

                OutputText("Scenes.Akbal.AkbalBitchSpeed.GrabLegRoll".Localized());

                OutputText("Scenes.Akbal.AkbalBitchSpeed.SquatAboveFace".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.WordClit".Localized());
                else
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.WordProstate".Localized());

                OutputText("Scenes.Akbal.AkbalBitchSpeed.LongTongueShiver".Localized());

                if ((!Player.HasVagina() && Player.Anal.Looseness < 2) || (Player.HasVagina() && Player.Vaginas[0].Looseness <= 2))
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.MushroomHeadMonstrous".Localized());
                else
                    if ((!Player.HasVagina() && Player.Anal.Looseness < 4) || (Player.HasVagina() && Player.Vaginas[0].Looseness < 4))
                    {
                        OutputText("Scenes.Akbal.AkbalBitchSpeed.MushroomHeadBarbed".Localized());

                        OutputText("Scenes.Akbal.AkbalBitchSpeed.PlantFeetRide".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Akbal.AkbalBitchSpeed.MushroomHeadGorgeous".Localized());

                        if (Player.HasVagina())
                            OutputText("Scenes.Akbal.AkbalBitchSpeed.WordVagina".Localized());
                        else
                            OutputText("Scenes.Akbal.AkbalBitchSpeed.WordAss".Localized());

                        OutputText("Scenes.Akbal.AkbalBitchSpeed.TouchedFluidSoaked".Localized());

                        OutputText("Scenes.Akbal.AkbalBitchSpeed.KnowWhatToDo".Localized());
                    }

                if (Player.HasVagina())
                    Player.CuntChange(37.5f, true, true, false);
                else
                    Player.ButtChange(37.5f, true, true, false);

                OutputText("Scenes.Akbal.AkbalBitchSpeed.FromReactionKnow".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.CantHelpGrip".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.ReleasesCreamyFlood".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Akbal.AkbalBitchSpeed.BodyConvulseCall".Localized());

                OutputText("Scenes.Akbal.AkbalBitchSpeed.BlissfullySore".Localized());

                if (Player.HasVagina())
                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 101);
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbitchToughness(bool ride = false)
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbitchToughness.LookDownHandiwork".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.SoundHitsEars".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Akbal.AkbitchToughness.TwitchesSpillingPre".Localized());
            else
                OutputText("Scenes.Akbal.AkbitchToughness.LetterY".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.CantHelpRub".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.InstructStopGrowling".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.ScreamsLookDown".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.TellShutUp".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.CastingVeneerAside".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.GoodBoyRub".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.WillPayForThis".Localized());

            OutputText("Scenes.Akbal.AkbitchToughness.HowDareBitchSpeak".Localized());

            if (Player.HasCock() && !ride)
            {
                Player.Orgasm("Dick");

                ShowImage("akbal-deepwoods-male-hightoughness");

                OutputText("Scenes.Akbal.AkbitchToughness.TellPutAssUp".Localized());

                var x = Player.BiggestCockIndex();

                if (Player.Cocks[x].Length < 7)
                    OutputText("Scenes.Akbal.AkbitchToughness.PushIntoJaguarBarely".Localized());
                else
                    if (Player.Cocks[x].Length <= 12)
                        OutputText("Scenes.Akbal.AkbitchToughness.PushIntoJaguarFlinch".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbitchToughness.PushIntoJaguar".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.NowComfortableIncrease".Localized());

                if (Player.Cocks[x].Length > 12)
                    OutputText("Scenes.Akbal.AkbitchToughness.WordAnother".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchToughness.WordA".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.BlastOfCorruptedSeed".Localized());

                if (Player.Corruption < 90)
                    OutputText("Scenes.Akbal.AkbitchToughness.ConvulsingHoleAround".Localized());
                else
                {
                    OutputText("Scenes.Akbal.AkbitchToughness.GrabHipsPound".Localized());

                    OutputText("Scenes.Akbal.AkbitchToughness.ClawsGroundRoaring".Localized());
                }

                if (Player.HasKnot(x))
                    OutputText("Scenes.Akbal.AkbitchToughness.HissesForceKnot".Localized());
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Akbal.AkbitchToughness.TriesKeepQuiet".Localized());
                    else
                        if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Akbal.AkbitchToughness.NothingButRoaring".Localized());
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                                OutputText("Scenes.Akbal.AkbitchToughness.FeelCockJump".Localized());
                            else
                                if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
                                    OutputText("Scenes.Akbal.AkbitchToughness.PoundMuscularAss".Localized());
                                else
                                    if (Player.Cocks[x].Id == BodyPartTypes.Lizard)
                                        OutputText("Scenes.Akbal.AkbitchToughness.InsidesQuiverTime".Localized());
                                    else
                                        OutputText("Scenes.Akbal.AkbitchToughness.PaintsAnalWhite".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.RiseGoesLimp".Localized());
            }
            else
            {
                Player.Orgasm("VaginalAnal");

                ShowImage("akbal-deepwoods-female-highspeed");

                OutputText("Scenes.Akbal.AkbitchToughness.FlipDemonOver".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.ObedientBitchFollows".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Akbal.AkbitchToughness.ClitFragment".Localized());
                else
                    OutputText("Scenes.Akbal.AkbitchToughness.WordProstate".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.LongTongueConvulses".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.ShoveDemonAway".Localized());

                if ((!Player.HasVagina() && Player.Anal.Looseness < 2) || (Player.HasVagina() && Player.Vaginas[0].Looseness <= 2))
                    OutputText("Scenes.Akbal.AkbitchToughness.TouchesMushroomHead".Localized());
                else
                    if ((!Player.HasVagina() && Player.Anal.Looseness < 4) || (Player.HasVagina() && Player.Vaginas[0].Looseness < 4))
                        OutputText("Scenes.Akbal.AkbitchToughness.TouchesMushroomHeadAgain".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbitchToughness.TouchesWetHead".Localized());

                if (Player.HasVagina())
                    Player.CuntChange(37.5f, true, true, false);
                else
                    Player.ButtChange(37.5f, true, true, false);

                OutputText("Scenes.Akbal.AkbitchToughness.BeginsSlideUp".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.ShrillCryCum".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Akbal.AkbitchToughness.EruptsSameTime".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.SubsidesGrind".Localized());

                OutputText("Scenes.Akbal.AkbitchToughness.WithoutGlanceGather".Localized());
            }

            if (Player.HasVagina())
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 101);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalQuestFight(int event_)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, event_);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 1);

            switch (event_)
            {
                case 0:
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.MetAliandra, 1);

                        OutputText("Scenes.Akbal.AkbalQuestFight.ClearShrubs".Localized());

                        OutputText("Scenes.Akbal.AkbalQuestFight.GoblinAsksFriend".Localized());

                        OutputText("Scenes.Akbal.AkbalQuestFight.GroggilyGetsUp".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Akbal.AkbalQuestFight.PushThroughShrubbery".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Akbal.AkbalQuestFight.JumpForthTrees".Localized());
                    }

                    break;
            }

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Akbal), () => Go<CampScene>());
        }

        public void AkbalEventGoblin()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) | AKBAL_EVENT_GOBLIN);

            OutputText("Scenes.Akbal.AkbalEventGoblin.ChatteringNoise".Localized());

            DoNext(() => AkbalEventGoblin2());
        }

        public void AkbalEventGoblin2()
        {

            OutputText("Scenes.Akbal.AkbalEventGoblin.SiftThroughFoliage".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblin.AudaciousFlair".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblin.GroansLookingForward".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblin.TrueToWordMushrooms".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblin.PausesFewMinutes".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Attack".Localized(), () => AkbalQuestFight(0));

            AddButton(1, "Scenes.Akbal.AkbalEventGoblin.ButtonMindControl".Localized(), AkbalEventGoblinPossess);

            AddButton(2, "Common.Button.Leave".Localized(), AkbalEventGoblinLeave);
        }

        public void AkbalEventGoblinPossess()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 4);
            // 原版 AkbalScene.hx:1738：该分支的触发条件
            if (SceneInstance<Npcs.ShouldraFollowerScene>.Get().FollowerShouldra())
            {
                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.HeyChamp".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.PerksLivingInside".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.SheShallStrange".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.EyesShiftYellow".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.PotionRipeReady".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.EyesWidenBubbling".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.AlchemistWaitStop".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.AngryLittlePuss".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.WorkedThatsAll".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AliandraDead, 1);
            }
            else
            {
                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.ReflectingGhostlyVisage".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.HearResonating".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.FutililtyNotOkay".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.SteelResolvePressOn".Localized());

                if (Player.HasPerk(PerkIds.HistoryAlchemist))
                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.DecipheringIngredients".Localized());
                else
                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.TossingIngredients".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.ImprovisedConcoction".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.ShoveCorkedFlask".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.NotYourProblem".Localized());

                OutputText("Scenes.Akbal.AkbalEventGoblinPossess.FlaskBubblesViolently".Localized());

                if (Player.IsPureEnough(33))
                {
                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.ChannelMentalControl".Localized());

                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.ExhaustionControlling".Localized());

                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.CrawlQuietlyFoliage".Localized());

                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.GetBackUpCoastClear".Localized());
                }
                else
                {
                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.VacateGoblinSwiftly".Localized());

                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.FarTooLate".Localized());

                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.AliandraStiflesGiggle".Localized());

                    OutputText("Scenes.Akbal.AkbalEventGoblinPossess.SetOffHome".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AliandraDead, 1);
                }
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventGoblinLeave()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalEventGoblinLeave.CouldDisruptRather".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLeave.EasyToLearn".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLeave.SmirkAtSight".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventGoblinWin()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 1);

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.AdversarySinks".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.ThrashedJaguar".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.NeverthelessGoal".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.RaisesHandsDefensively".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.PickPotionEye".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.AlchemistChimesIn".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinWin.ShareYourName".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventGoblinLose()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.CrumblingSlump".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.SighsHadItComing".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.BottlesConcoction".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.RunsOffObjective".Localized());

            DoNext(() => AkbalEventGoblinLose2());
        }

        public void AkbalEventGoblinLose2()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.SorenessFades".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.HelloSleepyHead".Localized());

            OutputText("Scenes.Akbal.AkbalEventGoblinLose.ExplainsHealingPotion".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public string AliceTerm()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) > 0 ? "Common.Text.Alice".Localized() : "Common.Text.LittleDemon".Localized();
        }

        public void AkbalEventAlice()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) | AKBAL_EVENT_ALICE);

            OutputText("Scenes.Akbal.AkbalEventAlice.FamiliarScentGlade".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) >= 99)
                OutputText("Scenes.Akbal.AkbalEventAlice.QuiteShock".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) >= 50)
                    OutputText("Scenes.Akbal.AkbalEventAlice.FewThatRemain".Localized());
                else
                    OutputText("Scenes.Akbal.AkbalEventAlice.AllTooCommon".Localized());

            OutputText("Scenes.Akbal.AkbalEventAlice.SetOffTowardSource".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) > 0)
                OutputText("Scenes.Akbal.AkbalEventAlice.AnAliceDemon".Localized());
            else
                OutputText("Scenes.Akbal.AkbalEventAlice.SmallChildHorns".Localized());

            OutputText("Scenes.Akbal.AkbalEventAlice.HoldingBottleMilking".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Attack".Localized(), () => AkbalQuestFight(1));

            AddButton(1, "Scenes.Akbal.AkbalEventAlice.ButtonBreakBottle".Localized(), AkbalEventAliceShoot);

            AddButton(2, "Common.Button.Leave".Localized(), AkbalEventAliceLeave);
        }

        public void AkbalEventAliceShoot()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 3);

            if (Player.Weapon.IsRanged() && Player.HasPerk(PerkIds.Scattering))
            {
                OutputText("Scenes.Akbal.AkbalEventAliceShoot.TakeAimWeapon".Localized());

                OutputText("Scenes.Akbal.AkbalEventAliceShoot.GlassShatters".Localized(
                     AliceTerm(),

                     AliceTerm()
                ));

                if (Player.IsPureEnough(33))
                {
                    ClearButtons();

                    AddButton(0, "Help " + (CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) > 0 ? "Common.Text.Alice".Localized() : "Common.Text.Child".Localized()), AkbalEventAliceShootHelp);

                    AddButton(1, "Common.Button.Leave".Localized(), AkbalEventAliceShootLeave);
                }
                else
                {
                    OutputText("Scenes.Akbal.AkbalEventAliceShoot.AnotherThwarting".Localized());

                    DoNext(() => ReturnToCamp(1));
                }
            }
            else
            {
                OutputText("Scenes.Akbal.AkbalEventAliceShoot.TakeAimFocusing".Localized(
                     AliceTerm()
                ));

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void AkbalEventAliceShootHelp()
        {

            var healSpells = new List<string> { "Heal", "Divine Wind", "Arian's Talisman - Heal" };

            var hasHeal = Player.AbilityAvailable(healSpells, ignoreLust: true, ignoreFatigue: true);

            OutputText("Scenes.Akbal.AkbalEventAliceShootHelp.WaitOutWhile".Localized());

            OutputText("Scenes.Akbal.AkbalEventAliceShootHelp.LoAndBehold".Localized(
                 AliceTerm()
            ));

            if (hasHeal)
                OutputText("Scenes.Akbal.AkbalEventAliceShootHelp.DrawArcaneSoothe".Localized());
            else
                OutputText("Scenes.Akbal.AkbalEventAliceShootHelp.PullOutSupplies".Localized());

            OutputText("Scenes.Akbal.AkbalEventAliceShootHelp.WontFixFingers".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventAliceShootLeave()
        {

            OutputText("Scenes.Akbal.AkbalEventAliceShootLeave.AnotherThwarting".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventAliceWin()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 1);

            OutputText("Scenes.Akbal.AkbalEventAliceWin.StumblesIntoGrass".Localized(
                 AliceTerm()
            ));

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventAliceLose()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalEventAliceLose.PinsYouDown".Localized(
                 AliceTerm()
            ));

            OutputText("Scenes.Akbal.AkbalEventAliceLose.RunsOffBottle".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AkbalEventAliceLeave()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalEventAliceLeave.ContentToLetPlay".Localized(
                 AliceTerm(),

                 AliceTerm(),

                 AliceTerm()
            ));

            OutputText("Scenes.Akbal.AkbalEventAliceLeave.RewardsLittleDemon".Localized(
                 AliceTerm()
            ));

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventKitsune()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) | AKBAL_EVENT_KITSUNE1);

            var fearSpells = new List<string> { "Whisper", "Terror", "Illusion" };

            var hasFear = Player.AbilityAvailable(fearSpells, ignoreLust: true, ignoreFatigue: true);

            OutputText("Scenes.Akbal.AkbalEventKitsune.TrudgingFoliage".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Attack".Localized(), () => AkbalQuestFight(2));

            AddButton(1, "Scenes.Akbal.AkbalEventKitsune.ButtonTerror".Localized(), AkbalEventKitsuneTerror);

            AddButton(2, "Common.Button.Leave".Localized(), AkbalEventKitsuneLeave);
        }

        public void AkbalEventKitsuneTerror()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 3);

            OutputText("Scenes.Akbal.AkbalEventKitsuneTerror.StaringContestJaguar".Localized());

            OutputText("Scenes.Akbal.AkbalEventKitsuneTerror.ChannelMagicAssault".Localized());

            OutputText("Scenes.Akbal.AkbalEventKitsuneTerror.RoarsHissesJumps".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventKitsuneLeave()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalEventKitsuneLeave.ContinuesStaringContest".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventKitsuneWin()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 1);

            OutputText("Scenes.Akbal.AkbalEventKitsuneWin.CollapsesOnslaught".Localized());

            OutputText("Scenes.Akbal.AkbalEventKitsuneWin.WalkOverKitsune".Localized());

            OutputText("Scenes.Akbal.AkbalEventKitsuneWin.RedHairedWalksOff".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalEventKitsuneLose()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalEventKitsuneLose.SlumpOverDefeat".Localized());

            OutputText("Scenes.Akbal.AkbalEventKitsuneLose.ContinuesStaringContest".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AkbalQuestConclusionPrompt()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestConclusionPrompt.SuddenPremonition".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Continue".Localized(), AkbalQuestConclusion);

            AddButton(1, "Common.Button.Leave".Localized(), AkbalQuestConclusionDelay);
        }

        public void AkbalQuestConclusionDelay()
        {

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalQuestConclusionDelay.AvoidForFewDays".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AkbalQuestConclusion()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestConclusion.WanderDenseForest".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) > 1)
                OutputText("Scenes.Akbal.AkbalQuestConclusion.EagerAnotherChance".Localized());
            else
                OutputText("Scenes.Akbal.AkbalQuestConclusion.SensesPerked".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.SuspicionStenchBlood".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.FeminineVoiceChill".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.FeraPassesWalks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) > 1)
                OutputText("Scenes.Akbal.AkbalQuestConclusion.TakesNoticeGlares".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.GoddessSmirks".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.FeraTrueToWord".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Watch".Localized(), () => AkbalQuestConclusion2(false));

            AddButton(1, "Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), () => AkbalQuestConclusion2(true));
        }

        public void AkbalQuestConclusion2(bool resist = false)
        {
            ClearOutput();

            if (resist)
            {
                OutputText("Scenes.Akbal.AkbalQuestConclusion.NotOnYourWatch".Localized());

                if (Player.Weapon.IsUnarmed())
                    OutputText("Scenes.Akbal.AkbalQuestConclusion.ReadyFightingStance".Localized());
                else
                    OutputText("Scenes.Akbal.AkbalQuestConclusion.ReadyWeapon".Localized());

                OutputText("Scenes.Akbal.AkbalQuestConclusion.GrowlsLowlyPrepared".Localized());

                OutputText("Scenes.Akbal.AkbalQuestConclusion.CarefreeHmf".Localized());

                OutputText("Scenes.Akbal.AkbalQuestConclusion.RootsSprout".Localized());

                if (Player.IsGoo())
                    OutputText("Scenes.Akbal.AkbalQuestConclusion.AmorphousBound".Localized());

                OutputText("Scenes.Akbal.AkbalQuestConclusion.FirmlyInPlace".Localized());

                if (Player.Weapon.IsFirearm())
                    OutputText("Scenes.Akbal.AkbalQuestConclusion.OpenFireRegardless".Localized());
                else
                    if (Player.Weapon.IsChanneling(Player))
                        OutputText("Scenes.Akbal.AkbalQuestConclusion.IncinerateBindings".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbalQuestConclusion.BreakThroughBindings".Localized());

                OutputText("Scenes.Akbal.AkbalQuestConclusion.LongSharpRoots".Localized());

                if (Player.IsGoo())
                    OutputText("Scenes.Akbal.AkbalQuestConclusion.SnakeTowardCore".Localized());
                else
                    OutputText("Scenes.Akbal.AkbalQuestConclusion.ProdFlesh".Localized());

                OutputText("Scenes.Akbal.AkbalQuestConclusion.NoWayAround".Localized());
            }
            else
                OutputText("Scenes.Akbal.AkbalQuestConclusion.TrulyHappening".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.WhilePonder".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.RubyRedLips".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.SighsVeryWell".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.StandsUpStraight".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.LoveHealthyBlood".Localized());

            if (resist)
                OutputText("Scenes.Akbal.AkbalQuestConclusion.GoodToSeeNotOnly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => AkbalQuestConclusion3(resist));
        }

        public void AkbalQuestConclusion3(bool resisted = false)
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestConclusion.WithdrawsClaws".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.HulkingForcesUp".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.EnjoyingAwe".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.SmilesAdmires".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.LowersToKnees".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.SmugGlee".Localized());

            if (resisted)
                OutputText("Scenes.Akbal.AkbalQuestConclusion.RootsRetreat".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusion.MyPleasureGoddess".Localized());

            DoNext(() => AkbalQuestConclusion4());
        }

        public void AkbalQuestConclusion4()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) > 1)
                OutputText("Scenes.Akbal.AkbalQuestConclusion.IncrediblePleasure".Localized());
            else
                OutputText("Scenes.Akbal.AkbalQuestConclusion.SoTerriblySorry".Localized());

            if (Player.Tallness >= 96)
                OutputText("Scenes.Akbal.AkbalQuestConclusion.SeldomMeasureUp".Localized());
            else
                OutputText("Scenes.Akbal.AkbalQuestConclusion.StatureLookingUp".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), AkbalQuestConclusionFight);

            AddButton(1, "Common.Button.Question".Localized(), AkbalQuestConclusionWho);

            AddButton(2, "Common.Button.Taunt".Localized(), AkbalQuestConclusionTaunt);
        }

        public void AkbalQuestConclusionFight()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestConclusionFight.EnoughDawdling".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionFight.WitnessMyTrueForm".Localized());

            StartAkbalUnsealedFight();
        }

        public void AkbalQuestConclusionWho()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.TrulyLargeBluffing".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.HaughtyWhoAmI".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.OnceHuman".Localized(
                 Player.StartingRace == "human" ? " just as you " + (Player.Race == "human" ? "Common.Text.Are".Localized() : "Scenes.Akbal.AkbalQuestConclusionWho.WordWere".Localized()) : ""
            ));

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.DemonQueenPerhaps".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.ManiacalLaughDistill".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.MonsterMoreLike".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.SneersArrogant".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.DontMisunderstand".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.GreatPowerfulMage".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.LaughHysterically".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWho.ThenLetUsBegin".Localized());

            StartAkbalUnsealedFight();
        }

        public void AkbalQuestConclusionTaunt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalFuckery, CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) + 1);

            OutputText("Scenes.Akbal.AkbalQuestConclusionTaunt.EverythingLeadUpTo".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionTaunt.WatchYourTongue".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionTaunt.BigWordsSmallWorm".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionTaunt.FlinchingBoltsFist".Localized());

            StartAkbalUnsealedFight();
        }

        public void AkbalQuestConclusionLose()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) | AKBAL_QUEST_BADEND);

            CoC.Instance.Flags.Set(GameFlagIds.AkbalDayDone, View.Time.Days);

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.AllTooMuch".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.HavingFunWithYou".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.GoddessDropsBack".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.GreatDemonDescends".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.RoughDemonicHands".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.PullsTongueOut".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.PantRecoverBreath".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.ClawedHandsHips".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.ThisIsWhatYouAre".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionLose.GodlyCockThrusts".Localized());

            GameOver();
        }

        public void AkbalQuestConclusionWin()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.EffortForNaught".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.PoorAkky".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.GrabsByHair".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.WatchMesmerized".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.ComeGetReward".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.WhatChoicePower".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.PerkTerrestrialFire".Localized());

            Player.CreatePerkIfNotHasPerk(PerkIds.TerrestrialFire, 0, 0, 0, 0);

            Player.AddMastery(new Identifier("coc", "terrestrial_fire"), 75);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryTerrestrialFire);

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.PowerDrainedRelieved".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.ClappingBreaksTrance".Localized());

            OutputText("Scenes.Akbal.AkbalQuestConclusionWin.ExamineDefeatedBody".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Kill".Localized(), AkbalQuestRewardKill);

            AddButton(1, "Common.Button.Curse".Localized(), AkbalQuestRewardKitty);
        }

        public void AkbalQuestRewardKill()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestRewardKill.WhoAreYouKidding".Localized());

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Akbal.AkbalQuestRewardKill.MoveShimmeringSword".Localized());
            else
                if (Player.Weapon.IsScythe())
                    OutputText("Scenes.Akbal.AkbalQuestRewardKill.BringScytheOver".Localized());
                else
                    if (Player.Weapon.IsFirearm())
                        OutputText("Scenes.Akbal.AkbalQuestRewardKill.AimAtHead".Localized());
                    else
                        if (Player.Weapon.IsBlunt())
                            OutputText("Scenes.Akbal.AkbalQuestRewardKill.RaiseHighLeap".Localized());
                        else
                            if (Player.Weapon.IsKnife())
                                OutputText("Scenes.Akbal.AkbalQuestRewardKill.WalkUpSlideWeapon".Localized());
                            else
                                if (Player.Weapon.IsBladed())
                                    OutputText("Scenes.Akbal.AkbalQuestRewardKill.MoveOverWeapon".Localized());
                                else
                                    OutputText("Scenes.Akbal.AkbalQuestRewardKill.ApproachFallenGod".Localized());

            Player.UpgradeBeautifulSword();

            OutputText("Scenes.Akbal.AkbalQuestRewardKill.ThisIsHisEnd".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalFuckery) >= 4)
            {
                OutputText("Scenes.Akbal.AkbalQuestRewardKill.GoddessGazesSlain".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => AkbalQuestRewardKillYes()); AddButton(1, "No", () => AkbalQuestRewardKillNo());
            }
            else
                DoNext(() => AkbalQuestEnd());
        }

        public void AkbalQuestRewardKillYes()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestRewardKillYes.NodAgreementClap".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKillYes.LeaningNailsPierce".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKillYes.CarvesThroughSkin".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKillYes.LicksGoreHands".Localized());

            Player.Inventory.Add(ItemRegistry.Create(UseableIds.AkbalPelt), 1);

            AkbalQuestEnd();
        }

        public void AkbalQuestRewardKillNo()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestRewardKillNo.ShakeHeadForgotten".Localized());

            AkbalQuestEnd(false);
        }

        public void AkbalQuestRewardKitty()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.SmileTurnToFera".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.GleefulLaugh".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.ScreamsNo".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.SaysNothingSmiling".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.ShieldEyesDisplay".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.NoMoreThreat".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKitty.WishAdoptCat".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => AkbalQuestRewardKittyYes()); AddButton(1, "No", () => AkbalQuestRewardKittyNo());
        }

        public void AkbalQuestRewardKittyYes()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestRewardKittyYes.WhatNameKitty".Localized());

            GenericNamePrompt();

            AddButton(0, "Common.Button.Confirm".Localized(), AkbalQuestRewardKittyName);
        }

        public void AkbalQuestRewardKittyName()
        {
            ClearOutput();

            var input = FinishInput().Trim();

            if (input == "")
            {
                OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.MustSelectName".Localized());

                GenericNamePrompt();

                AddButton(0, "Common.Button.Confirm".Localized(), AkbalQuestRewardKittyName);

                return;
            }

            var snugglePuss = new List<string> { "snuggl", "fuzz", "fluff", "baby", "kitty", "kitten", "mitten" };

            CoC.Instance.Flags.Set(GameFlagIds.AkkyName, input);

            input = input.ToLower();

            switch (input)
            {
                case "akky":
                    {
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.AkkyFineName".Localized());
                    }

                    break;

                case "akbal":
                    {
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.PoeticJusticeKeep".Localized());
                    }

                    break;

                case "fera":
                    {
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.GoddessSmirksShiver".Localized());

                        DoNext(() => AkbalQuestRewardKittyYes());

                        return;
                    }

                case "tama":
                    {
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.ExcellentPetStation".Localized());
                    }

                    break;

                case "alice":
                    {
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.YouAreWhatYouEat".Localized());

                        if (new[] { "dick", "richard", "pussy", "vagina", "cunt", "cock", "penis", "gash", "ass", "wang", "cum", "spooge", "kitty", "cunny" }.Contains(Player.ShortName.ToLower()))
                            OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.JustLikeYourself".Localized());
                    }

                    break;
            }

            if (true)
                if (snugglePuss.Any(it => input.StartsWith(it)))
                    OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.CheekyGrinName".Localized());
                else
                    if (input == "nig" || input.IndexOf("nigg") == 0)
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.OnceEnemyNowPet".Localized());
                    else
                        OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.WillDoJustFine".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKittyName.MeowsSound".Localized());

            DoNext(() => AkbalQuestEnd());
        }

        public void AkbalQuestRewardKittyNo()
        {
            ClearOutput();

            OutputText("Scenes.Akbal.AkbalQuestRewardKittyNo.RatherNot".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKittyNo.WhimsicallyImps".Localized());

            OutputText("Scenes.Akbal.AkbalQuestRewardKittyNo.LittleAkkyRunsOff".Localized());

            Save.SetBool("strayCat", true);

            DoNext(() => AkbalQuestEnd());
        }

        public void AkbalQuestEnd(bool clear = true)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) | AKBAL_QUEST_DONE);

            OutputText("Scenes.Akbal.AkbalQuestEnd.StretchesLimbs".Localized());

            OutputText("Scenes.Akbal.AkbalQuestEnd.WandersOff".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }

    [Flags]
    public enum AkbalEvent
    {
        None = 0,

        Goblin = 1 << 1,

        Alice = 1 << 2,

        Kitsune1 = 1 << 3,

        Kitsune2 = 1 << 4,

        QuestBadEnd = 1 << 5,

        QuestDone = 1 << 6,
    }
}