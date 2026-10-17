using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<MinotaurMobScene>("coc:minotaur_mob")]
    public class MinotaurMobScene : Scene
    {

        public override void Entry()
        {
            MeetMinotaurSons();
        }

        public void MeetMinotaurSons()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredMinotaurMob, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredMinotaurMob) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredMinotaurMob) == 1)
            {
                ShowImage("monster-minomob");

                if (!Player.MinotaurAddicted())
                {
                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ExploreHillyMountains".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ThreeMinotaursHalfCircle".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ReadyWeaponFightingPose".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.AffectionsMoreThanOught".Localized());
                    else
                        if (Player.Corruption < 66)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.IndulgedGloriousFewTimes".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.NeverTurningFuckBeasts".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.BrutesNoPreyAdrenaline".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.MomDoesntRecognizeBiggest".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.NextLargestSmirksHorns".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FurPatternsAboutIt".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 3)
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.SomeOfWord".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.MatchCoatsProgenyGrown".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.CantHelpLaughPride".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), FightOTaurs);

                    AddButton(1, "Common.Button.Submit".Localized(), SubmitToMinotaurMob);

                    AddButton(2, "Common.Button.Negotiate".Localized(), Negotiate);

                    AddButton(3, "Common.Button.Run".Localized(), RunFromMinotaurs);
                }
                else
                {
                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ExploreHillyMountainsAgain".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ThreeMinotaursCircleAgain".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.StartReadyWobblingClose".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.MomDoesntRecognize".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.NextLargestSmirksAgain".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.GapeDesireSatiateReplaced".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.LookThinkAboutHowever".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 3)
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.SomeOfWordAgain".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ThoseOffspringGrownUp".Localized());

                    OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.JudgingStiffeningLoincloths".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.WouldntFuckOffspringCum".Localized());
                    else
                        if (Player.Corruption < 66)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.NotSureHowFeel".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.WhoCaresOffspringAppetizing".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), FightOTaurs);

                    AddButton(1, "Common.Button.Submit".Localized(), SubmitToMinotaurMob);

                    AddButton(3, "Common.Button.Run".Localized(), RunFromMinotaurs);
                }
            }
            else
            {
                ShowImage("monster-minomob");

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 20)
                {
                    if (!Player.MinotaurAddicted())
                    {
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.OffspringFaceToFace".Localized(
                             NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings))
                        ));

                        if (Player.Wetness() <= 2)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.TrickleWetnessRunsDown".Localized());
                        else
                            if (Player.Wetness() <= 4)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.SteadyFlowJuicesSoaks".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FloodWetnessSoaksArmor".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.CouldBeBitDifficult".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.LargestFoldsArmsChest".Localized());

                        if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 216 && Player.Pregnancy.Type == PregnancyIds.Minotaur)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.PregnantAnotherBrotherMore".Localized());
                        else
                            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 216)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.HuhCouldntLegsClosed".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.HeyMomAnotherWhack".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ParagraphBreak".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.NahBackFinishAddicted".Localized());

                        if (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 216)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.SoonToBeWord".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.KnockedUpPussTaunts".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.RuntLurksStrokingWhining".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.WhatDoYouDo".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.OffspringFaceSoon".Localized(
                             NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings))
                        ));

                        if (Player.Wetness() <= 2)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.TrickleWetnessDownAgain".Localized());
                        else
                            if (Player.Wetness() <= 4)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.SteadyJuicesSoaksAgain".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FloodWetnessArmorAgain".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.AbsentmindedlyTweakNipple".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.LargestFoldsChestAgain".Localized());

                        if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 216 && Player.Pregnancy.Type == PregnancyIds.Minotaur)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.PregnantAnotherBrotherFix".Localized());
                        else
                            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 216)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.HuhKnockedUpSlut".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.HeyMomAnotherKnocking".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ParagraphBreakAgain".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.TauntsLargestBrotherSlander".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.RuntMinitaurLurksWhining".Localized());

                        if (Player.MinotaurNeed())
                        {
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.MindFoggingIntenseNeed".Localized());

                            DoNext(() => SubmitToMinotaurMob());

                            return ;
                        }

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.MindFoggingScentWithdrawal".Localized());
                    }

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), FightOTaurs);

                    AddButton(1, "Common.Button.Submit".Localized(), SubmitToMinotaurMob);

                    AddButton(2, "Common.Button.Run".Localized(), RunFromMinotaurs);
                }
                else
                {
                    if (!Player.MinotaurAddicted())
                    {
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.WhileExploringSonsTerritory".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.BemusedSnortRousesDream".Localized());

                        if (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 216)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ComeBackAddTribe".Localized());
                        else
                            if (Player.Pregnancy.Type == PregnancyIds.Minotaur)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ShowOffLilBro".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ComeBackRealStuds".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.CheeksColorLewdSuggestions".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.TauntsSecondFamiliarVoice".Localized(
                             NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings))
                        ));

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.GodsYoureGettingWet".Localized());

                        if (Player.Wetness() >= 5)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FeelJuicesRunningUnimpeded".Localized());
                        else
                            if (Player.Wetness() >= 3)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FeelJuicesDribblingPrimal".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FeelArmorClingingSopping".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ManyLoinclothsPreCum".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.NoWord".Localized());

                        if (Player.Corruption < 33)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.CantThinkWayTribe".Localized());
                        else
                            if (Player.Corruption < 66)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ShouldntThinkWaySpawn".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ShouldntThinkWayBeasts".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.WhatDoYouAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.WhileExploringTerritoryAgain".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.BemusedSnortRousesDaydream".Localized());

                        if (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 216)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ComeBabySpunkStuffed".Localized());
                        else
                            if (Player.Pregnancy.Type == PregnancyIds.Minotaur)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ComeBackSpunkMinitaur".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ComeStudsDontFret".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.CheeksColorSuggestionsAgain".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.SecondFamiliarWastingTime".Localized(
                             NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings))
                        ));

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.GodsYoureGettingAgain".Localized());

                        if (Player.Wetness() >= 5)
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FeelRunningUnimpededAgain".Localized());
                        else
                            if (Player.Wetness() >= 3)
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FeelDribblingPrimalAgain".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.FeelClingingSoppingAgain".Localized());

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.ManyLoinclothsLikewiseStained".Localized());

                        if (Player.MinotaurNeed())
                        {
                            OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.PerfectNeedItBad".Localized());

                            DoNext(() => SubmitToMinotaurMob());

                            return ;
                        }

                        OutputText("Scenes.Minotaurmobscene.MeetMinotaurSons.GloriousManyHornyBoys".Localized());
                    }

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), FightOTaurs);

                    AddButton(1, "Common.Button.Submit".Localized(), SubmitToMinotaurMob);
                }
            }
        }

        public void Negotiate()
        {
            ClearOutput();

            ShowImage("minomob-negotiate");

            OutputText("Scenes.Minotaurmobscene.Negotiate.CalmDownWarnPushover".Localized());

            OutputText("Scenes.Minotaurmobscene.Negotiate.SecondLargestChillOut".Localized());

            OutputText("Scenes.Minotaurmobscene.Negotiate.BurliestBrotherFuckSuck".Localized());

            OutputText("Scenes.Minotaurmobscene.Negotiate.MeekMinitaurSpeaksLast".Localized());

            OutputText("Scenes.Minotaurmobscene.Negotiate.OnlyInterestedOneThing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightOTaurs);

            AddButton(1, "Common.Button.Submit".Localized(), SubmitToMinotaurMob);

            AddButton(2, "Common.Button.Run".Localized(), RunFromMinotaurs);
        }

        public void RunFromMinotaurs()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.RunFromMinotaurs.TurnTailTryRun".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Minotaurmobscene.RunFromMinotaurs.LeapingSmallCliffsFlight".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.RunFromMinotaurs.FleeingDownMountainside".Localized());

            OutputText("Scenes.Minotaurmobscene.RunFromMinotaurs.PeriodSpace".Localized());

            if ((Player.CanFly() && Player.Speed > Random.Shared.Next(40)) || (!Player.CanFly() && Player.Speed > Random.Shared.Next(60)))
            {
                OutputText("Scenes.Minotaurmobscene.RunFromMinotaurs.FurryArmNearlyCatches".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                if (Player.Gender == Gender.Hermaphrodite)
                    ShowImage("minomob-falling-herm");
                else
                    if (Player.Gender == Gender.Female)
                        ShowImage("minomob-falling-female");
                    else
                        ShowImage("minomob-falling-male");

                OutputText("Scenes.Minotaurmobscene.RunFromMinotaurs.FurryArmCatchesYanks".Localized());

                FightOTaurs();
            }
        }

        public void FightOTaurs()
        {

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.MinotaurMob), () => Go<CampScene>(), canFlee: false);
        }

        public void SubmitToMinotaurMob()
        {

            Player.Lust = Player.MaxLust;

            Player.ApplyStats(new DynStat(GameStats.Lust, 1f));

            MinotaurDeFeet();
        }

        public void MinotaurDeFeet()
        {
            ClearOutput();

            if (Player.Gender == Gender.Hermaphrodite)
                ShowImage("minomob-falling-herm");
            else
                if (Player.Gender == Gender.Female)
                    ShowImage("minomob-falling-female");
                else
                    ShowImage("minomob-falling-male");

            if (!Player.HasVagina())
            {
                OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.SlumpDefeatMissingFemininity".Localized());

                CombatScene.CleanupAfterCombat();

                return ;
            }

            if (Player.Health < 1)
                OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.CompletelyDefeatedBruised".Localized());
            else
            {
                OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.TooTurnedOnFight".Localized());

                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.BeginsToDripFrom".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.LeaksFromYour".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.RollsOutWaveFrom".Localized());

                OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.ZeroSentencePeriod".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.HardensDanglesAboveGround".Localized(
                         Player.SMultiCockDesc()
                    ));

                OutputText("Scenes.Minotaurmobscene.MinotaurDeFeet.RemoveLoinclothsSmears".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 25 && Player.MinotaurNeed())
                MinotaurGangBadEnd();
            else
                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 216)
                {
                    if (Player.Pregnancy.Incubation > 120)
                        AnalSpearSemiPregMinotaurGangbang();
                    else
                        LoseToMinoMobVeryPregnant();
                }
                else
                    NonAddictMinotaurGangBang();
        }

        public void NonAddictMinotaurGangBang()
        {

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.StepCloserHoovesDust".Localized());

            if (Player.IsTaur())
            {
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.SuddenPushRollsFlank".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.AssholeWord".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.TailHoleWord".Localized());
            }
            else
                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.SuddenYankStretchesTail".Localized());
                else
                    if (Player.IsGoo())
                    {
                        OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.SuddenPushRollsSide".Localized());

                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.AssholeWordAgain".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.TailholeWordAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.PowerfulYankLiftsLeg".Localized());

                        if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.AssholeWordThird".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.TailholeWordThird".Localized());
                    }

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.CruelFlaredTipBatters".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.PressureSubsidesNotPulling".Localized(
                 Player.AssholeDescript()
            ));

            Player.ButtChange(60, true, true, true);

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 10)
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.AssembledBeastsSnicker".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.MinitaurGigglesMewling".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.GurgleDualDicksDeeper".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.InchByInchBulging".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.BovineMastersSawMembers".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.GushesEverywherePuddling".Localized());
            else
                if (Player.Wetness() >= 3)
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.DroolsThickStreamGround".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.DripsLubricantBlotches".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.BouncingLikeChildsBall".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.MouthGrabsHoldOf".Localized());

            if (Player.Horns.Id.Path != "none")
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HornsWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HeadWord".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.RoughlyBuriesCockHilt".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.ExpandingPenultimateFlares".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.CouldntBeHappier".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.BulgesSqueezingPastTongue".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.OnceFreeWipesMess".Localized());

            ShowImage("minomob-gangbang");

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.RuntNotTodayLifts".Localized());

            if (Player.Tallness <= 70)
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HisChestWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HisShoulderWord".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.LaughsCrestfallenBackdoor".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.RockSolidMassSoftens".Localized());

            Player.CuntChange(60, true, false, true);

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.PartFleshWithEase".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.FuckMomPussyGreat".Localized());

            if (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 216)
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HopeDontAnotherBrother".Localized());
            else
                if (Player.Pregnancy.Type == PregnancyIds.Minotaur)
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HopeDontSayingHi".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HopeBackRealBaby".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.FeelComingReadyMa".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HugeBloatedMassStretches".Localized());

            Player.CuntChange(60, true, false, true);

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HeadFlaresSquirtsWomb".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.ExplodesAgainstBelly".Localized(
                     Player.SMultiCockDesc()
                ));

                if (Player.CumQ() < 25)
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.FewSpotsOfCum".Localized());
                else
                    if (Player.CumQ() < 50)
                        OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.SteadyTrickleOfCum".Localized());
                    else
                        if (Player.CumQ() < 100)
                            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.SteadyStreamOfCum".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.ConstantFlowSoaksFur".Localized());
            }

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.CompletelyImpaledDrugged".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.DistractedHighPulledOut".Localized());

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.MinitaurStaysRubbingLips".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) <= 3)
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.FirstDoseReadyMore".Localized());
            else
            {
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.HasntHadChanceYet".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) <= 5)
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.OpenMouthWideShiny".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.OpenMouthShinyAgain".Localized());
            }

            OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.BlackOutWakeBottle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.MinotaurCum);

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation, 75);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -.5f), new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            Player.MinoCumAddiction(20);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
            {
                OutputText("Scenes.Minotaurmobscene.NonAddictMinotaurGangBang.ParagraphBreak".Localized());

                Player.Sleeping = true;

                InventoryScene.TakeItem(ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.BonusItemAfterCombatId))), () => ReturnToCamp(1));
            }
        }

        public void LoseToMinoMobVeryPregnant()
        {

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.LargestStudAboutPop".Localized());

            if (Player.Pregnancy.Type != PregnancyIds.Minotaur)
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.DoesntKeepTwatClosed".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FeelBrotherNotLong".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.WombFullOtherHoles".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FrownInWord".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.IrritationAddressedInsultingly".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.AnnoyanceTheirFamiliarity".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.IrritationLosingControl".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.BuildingAnticipationLoins".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.SmearsFlareForeheadGoo".Localized());

            if (Player.Pregnancy.Type != PregnancyIds.Minotaur)
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.IsntWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.MayNotBeWord".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FatherOfChildWomb".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FuzzCoveredBrushChin".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.PreLeakingCocksReminds".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.PainRadiatesStrongImpact".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.OgleDicksHoveringMinitaur".Localized(
                 NumberUtils.Num2Text((CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) - 1))
            ));

            if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic || Player.Body.Get<TailPart>().Id == BodyPartTypes.Cow || Player.Body.Get<TailPart>().Id == BodyPartTypes.Shark || Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard || Player.Body.Get<TailPart>().Id == BodyPartTypes.Kangaroo)
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.EvenTailReachesCurl".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.PredictsSecondLargestIt".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.PropheticWordsFlareEsophagus".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.GoodBitchProclaimsSpurts".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.DomineeringSecondLineSlaps".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FinishSwallowingSighHappily".Localized());

            ShowImage("minomob-pregfuck");

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.GodsDemonsSuchSlut".Localized());

            if (Player.Pregnancy.Type == PregnancyIds.Minotaur)
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.BabyProbablyMineToo".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.BabyIsntRightSpecies".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.HowDoesMakeFeel".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.GoodComesOutMpppp".Localized());

            if (Player.Horns.Id.Path != "none")
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.HornsWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.HairWord".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.PullsBackTongueUnderside".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FranticFaceHumpingEruption".Localized());

            OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.NiceTightThroatWhore".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 8)
            {
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.ItsMinitaursNotHuge".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.MinitaurMoansCumNow".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.LookUpTongueSheath".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.FinishedFemininePullsBack".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 3)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 4)
                        OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.SorryBroMomDone".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.SorryBrosMomDone".Localized());
                }
                else
                    OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.DamnNicePregnantSlut".Localized());
            }
            else
            {
                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.BiggestYetWhichSon".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.SighLetAnchorPlace".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.ReachingGrabSweatySpheres".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.MomentLaterTestesQuiver".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.WellEndowedShowersSeed".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.NextBeastApproachesNarcotics".Localized());

                if (Player.Pregnancy.Type == PregnancyIds.Minotaur)
                    OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.WouldntWantTribeInjured".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.WouldntRuinFavoriteToy".Localized());

                OutputText("Scenes.Minotaurmobscene.LoseToMinoMobVeryPregnant.LookSmilingPassesOut".Localized());
            }

            Player.Orgasm("Lips");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, .5f), new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            Player.MinoCumAddiction(15);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
            {
                Player.Sleeping = true;

                ReturnToCamp(1);
            }
        }

        public void AnalSpearSemiPregMinotaurGangbang()
        {

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.QuipsSecondLittlePregnant".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.BiggestBruteSlamsGropes".Localized());

            if (Player.BiggestLactation() >= 1.5f)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.ExpressingDropsOfMilk".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.MoaningSquirmingCrudeCaresses".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.TurgidLengthProddingTummy".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.NervousVoiceInterruptsGroping".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 20)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.MakeTribeEvenBigger".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.MoreBrothersWord".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.DontTinyMinitaurFlinches".Localized());

            if (Player.Pregnancy.Type == PregnancyIds.Minotaur)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.OurLittleBrotherWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.HerBabyMakerWord".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.PeriodSayEnd".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.GrabsJawOpensInserting".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.WonderfulSaltySweetTaste".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.FuckThroatTightPro".Localized());

            if (!Player.MinotaurAddicted())
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.GetMomHigh".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.GottaGetMomFix".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.TrySuccubusBlowjobBitch".Localized());

            ShowImage("minomob-pregfuck-semi");

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.SureThingDeepVoiced".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.HaunchesWord".Localized());
            else
                OutputText(Player.AssDescript());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.SkywardBackBowingTip".Localized());

            Player.ButtChange(60, true, true, false);

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.VoiceLikeAvalancheLaughs".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.MoreHorseCocksJokes".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.HigherPitchedWhinesNice".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.SnarlingGrowlShutUp".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.HummingSonsCockTaunts".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.AdaptSituationEnjoyReturn".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.SquirtWord".Localized());
            else
                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.SlimeWord".Localized());
                else
                    if (Player.Wetness() >= 3)
                        OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.LeakWord".Localized());
                    else
                        if (Player.Wetness() >= 2)
                            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.TrailDribblesOfWord".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.DripWord".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.GirlJuiceEverywhere".Localized());

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.ProbingHandsNippleFingers".Localized());

                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.WowMinitaurCouldUse".Localized());

                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.GlanceSmallestFurtively".Localized());

                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.MoansLouderAmazingMom".Localized());

                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.UnfortunatelyNotOnlyHears".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 7)
                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.ShakesHeadSavingTrouble".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.NodsMemberPullsRunt".Localized());

                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.ParagraphBreak".Localized());
            }
            else
                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.WetClaspNippleTrickle".Localized());

                    if (Player.BreastRows.Count() == 2)
                        OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.YourWord".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.AnWord".Localized());

                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.OtherBreastFeminineHand".Localized());
                }
                else
                {
                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.WetClaspNipplePuffy".Localized());

                    if (Player.BreastRows.Count() == 2)
                        OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.YourWordAgain".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.AnWordAgain".Localized());

                    OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.OtherBreastFeminineAgain".Localized());
                }

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.BouncedToFroFucked".Localized());

            if (Player.BiggestBreastSize() >= 3 && Player.BiggestLactation() >= 1)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.PregnantMilkyTitsSwaying".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.HardAchingNipplesSucked".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.PotentPreCumDribbling".Localized());

            if (Player.Wetness() <= 3)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.EvenNotUsuallyWet".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.FeelsGoodShakeSpasm".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.AssholeLockingBiggerDick".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.BothPolesSlideOut".Localized());

            if (!Player.HasFuckableNipples() && Player.BiggestLactation() >= 1)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.MinitaursLipsDisentangle".Localized());

            OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.HandsGentlyLowerGround".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 5)
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.PositionedAssInAir".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.AnalSpearSemiPregMinotaurGangbang.StepBackAdmireHandiwork".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, .5f), new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            Player.MinoCumAddiction(15);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
            {
                Player.Sleeping = true;

                ReturnToCamp(1);
            }
        }

        public void VictoryMinotaurGang()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                ShowImage("minomob-fallen-hp");

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGang.LastMinotaurSinksKnees".Localized());
            }
            else
            {
                ShowImage("minomob-fallen-lust");

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGang.LastMinotaurKneesAgain".Localized());
            }

            if (Player.Lust < 33)
                Player.ApplyStats(new DynStat(GameStats.Lust, 33f));
            else
                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGang.BodyBurningBuzzingLust".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Titfuck".Localized(), null);

            AddButton(2, "Common.Button.NippleFuck".Localized(), null);

            AddButton(4, "Common.Button.GetSucked".Localized(), null);

            AddButton(0, "Common.Button.Gangbang".Localized(), VictoryAllThePenetrationsMinotaurGangBang);

            if (Player.BiggestBreastSize() >= 6)
                AddButton(1, "Common.Button.Titfuck".Localized(), VictoryMinotaurGangTitFuck);

            if (Player.HasFuckableNipples())
                AddButton(2, "Common.Button.NippleFuck".Localized(), VictoryBJNippleFuckMinotaurGang);

            AddButton(3, "Common.Button.GetLicked".Localized(), () => ForceMinitaurToGiveOral(0));

            if (Player.HasCock())
                AddButton(4, "Common.Button.GetSucked".Localized(), () => ForceMinitaurToGiveOral(1));

            AddButton(5, "Common.Button.Discipline".Localized(), DisciplineEldestMinotaurSon);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void VictoryMinotaurGangTitFuck()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.CooAdvanceHipsSwaying".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.RumblesDefeatedReleaseSlumps".Localized());

            if (Player.MinotaurAddicted())
                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.OfCourseHelpsPleasing".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.FantasticBoysComeHere".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LowerYourselfDown".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.ToYourKneesWord".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.OnYourForelegsWord".Localized());
                else
                    if (Player.IsGoo())
                        OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.IntoGooeyUndercarriage".Localized());
                    else
                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.IntoCoilsOfTail".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.IntoErrorUnclassifiedBody".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.OpenArmorExposeApproach".Localized(
                 Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.FirstTwoArmsReach".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.OhYesNaughtyPunished".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GigglePunishCryingForgiveness".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.PatientlyHoldStillDefeated".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.PressHandsMashFlesh".Localized(
                 Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.RewardPatientStudsTempo".Localized());

            ShowImage("minomob-won-titfuck");

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LeaningWrapLipsFlare".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.IsntEvenTimeRecover".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.SwayOutOfMind".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 10)
            {
                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.SoMilkHeadSpin".Localized(
                     Appearance.BiggestBreastSizeDescript(Player)
                ));

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GoodBoysMommyCum".Localized());

                if (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties))
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LikeWord".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GettingSoooWastedCommand".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.RuntWhiningSorryMom".Localized());

                if (Player.Horns.Id.Path != "none")
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.HornsWord".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.HairWord".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.PullsDownRammingFlare".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.NarcoticSludgeThickens".Localized());

                if (Player.MinotaurAddicted())
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.HateSeeGoMad".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.SoBetterLearnNeed".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.SayEndParagraph".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.NodDumblySideBottle".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.AwakenCampWoozyNaked".Localized());
            }
            else
            {
                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.TheresOnlyWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 3)
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.OneMinotaurWord".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.MinotaursCountWord".Localized(
                         NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) - 2)
                    ));

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LeftToMilkPunishing".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 3)
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GrabMinitaurDragTits".Localized());
                else
                {
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GrabMinitaurAnd".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 4)
                        OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.RemainingBrotherWord".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.OneRemainingBrothersWord".Localized());

                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.PullingHardBuryingMembers".Localized());
                }

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GoodBoysMommyAgain".Localized());

                if (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties))
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LikeWordAgain".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GettingWastedCommandAgain".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.MinitaurWobblesUnuse".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 3)
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GlazeSpurtsAlongOpening".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.PreviouslySatedBottlePlaces".Localized());

                if (!Player.MinotaurAddicted())
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.MaybeGetMomHooked".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.NiceLaterLoseControl".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.SayEndWord".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.GatherMouthfulsStumblingFeet".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 5)
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LetGetDressedPleased".Localized());
                else
                {
                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.BeforeFarStrongArm".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 5)
                    {
                        OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.AndHisBrotherWord".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 6)
                            OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.PluralS".Localized());
                    }

                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.ActPassesInBlur".Localized());

                    if (!Player.IsGoo())
                        OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.LookLikeWhiteGirl".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.RGelatinousBodyFilled".Localized());

                    OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.ManageLeaveNearlyForget".Localized());
                }
            }

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.MinotaurCum);

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, .5f), new DynStat(GameStats.Corruption, .5f));

            Player.SlimeFeed();

            Player.MinoCumAddiction(15);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
            {
                OutputText("Scenes.Minotaurmobscene.VictoryMinotaurGangTitFuck.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.BonusItemAfterCombatId))), () => ReturnToCamp(1));
            }
        }

        public void VictoryAllThePenetrationsMinotaurGangBang()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.DecidingSameStripInstruct".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.NearlyInstantlyTooAroused".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SluggishlyWorkGetNude".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SaunterClosestAdmireHardness".Localized());

            ShowImage("minomob-gangbang");

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.ProdsVulvaSmearingLips".Localized());

            if (Player.VaginalCapacity() >= 60)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SlipsInWithEase".Localized(
                     Player.VaginaDescript(0)
                ));
            else
                if (Player.VaginalCapacity() >= 40)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SlipsInEverSlowly".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.StretchesPainfulForce".Localized(
                         Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.PulsatesWetlyHeartbeat".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.ArchWiggleWhichSon".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.WithinSecondsCrowdFormed".Localized(
                 Player.ButtDescript()
            ));

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.FinallyHitsAnusPushes".Localized());

            if (Player.AnalCapacity() < 40)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SoTightPushRelease".Localized());
            else
                if (Player.AnalCapacity() < 60)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SlowlyWidensToAccept".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.DevoursCockEasily".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.BallsRestAgainstTaint".Localized());

            Player.ButtChange(60, true, true, false);

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.RockHipsHelpMom".Localized());

            if (Player.HasFuckableNipples() && CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 4)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.TheresStillCouldDo".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.ParagraphBreak".Localized());

            if (Player.HasFuckableNipples() && CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 4)
            {
                if (Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.MilkyLubeWord".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.LubricantWord".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.DripsFromNipplesSympathy".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 4)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.StudAssemblesNextTo".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 5 && Player.BreastRows.Count() == 2)
                        OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.TwoStudsAssembleAround".Localized());
                    else
                    {
                        var dicks = Player.BreastRows.Count();

                        if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) - 3 < dicks)
                            dicks = CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) - 3;

                        OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.StudsAssembleAroundCount".Localized(
                             NumberUtils.Num2Text(dicks)
                        ));
                    }

                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.WatchingTitsSwayMotion".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 4)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.HeExcitedlyFingers".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.TheyExcitedlyFinger".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.TitsSlippingDigitsDepths".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 4)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.LicksCleanSpearing".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.TheyCleanSpearingNipples".Localized());
            }

            if (Player.HasCock())
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.HangsLimplyBelliesDribbling".Localized(
                     Player.SMultiCockDesc()
                ));

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.MoreLikeCompletelyImpaled".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.CringeIncestuousOrgyRising".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.BarelyHangSanityDrug".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.HumPleasureCompletePenetration".Localized());

            if (Player.HasFuckableNipples() && CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 4)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.NipplesSquirt".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.AssSqueezesDownOverwhelmed".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.BounceLikeChildsMerc".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.AssGetsOffFirst".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.FeelFourBallsShrinking".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.FlaringWideThroatCum".Localized());

            if (Player.HasFuckableNipples() && CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 4)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) > 4)
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.MeanwhileNipplesSimilarFeeling".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.MeanwhileNippleSimilarFeeling".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.BloatingBreastsBraSize".Localized());
            }

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.AllPullOutOnce".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 20)
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.GangWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.TribeWord".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.SmirksStruggleRightYourself".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.WobbleMakeItUp".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryAllThePenetrationsMinotaurGangBang.AfterDressedScoopCum".Localized());

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation, 75);

            Player.Orgasm("Anal");

            Player.Orgasm("Vaginal", false);

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -.5f), new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            Player.MinoCumAddiction(20);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void ForceMinitaurToGiveOral(float choice = 0)
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.HastilyRemoveLowerPortions".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.AndWord".Localized());

            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.ToTheAirDefeated".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 20)
                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.MobWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.TribeWord".Localized());

            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.OfMinotaursPerkUp".Localized());

            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.TinyFeminineHopefullyCrush".Localized());

            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.PatheticHopelesslyAddicted".Localized());

            if (choice == 0)
            {
                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.StartLickingMommaPussy".Localized());

                if (Player.IsTaur())
                {
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.TurnWord".Localized());

                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.AndLiftYourTail".Localized());

                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.ExposeGlisteningPinkness".Localized());
                }
                else
                {
                    if (Player.Balls > 0)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.LiftYourBallsAnd".Localized());

                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SpreadWideWithFingers".Localized());
                }

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.ClitPeeksOutHood".Localized());

                if (Player.GetClitLength() >= 8)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.UntilHugeSizeRevealed".Localized());
                else
                    if (Player.GetClitLength() >= 3)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.UntilLikeLengthRevealed".Localized());
                    else
                        if (Player.GetClitLength() >= 1)
                            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.UntilLargeWomanlyOrgan".Localized());
                        else
                            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.RevealingCuteButtonSelf".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SentencePeriod".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.GirlyMonsterCrawlsForward".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.DisappearingUnderHindLegs".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.LookingUpLonginglyLeans".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.OpensMouthHesitantlyTongue".Localized());

                ShowImage("minomob-won-pussysucker");

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.ExtendingDefensivelySlides".Localized());

                if (Player.GetClitLength() < 3)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.EvenSuckingClitLength".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.EvenStrokingClitHand".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.BeginUndulateGrinding".Localized());

                if (Player.GetClitLength() >= 6)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.JacksClitLikeCock".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SucksLicksPussyClit".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.CumHardBoyMuzzle".Localized());

                if (Player.Wetness() >= 5)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.WaveSquirtingDripsChin".Localized());
                else
                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.StreamDribblingDripsChin".Localized());
                    else
                        if (Player.Wetness() >= 3)
                            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.DribblingFluidDownChin".Localized());
                        else
                            if (Player.Wetness() >= 2)
                                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SomeFluidsEscapeMouth".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.AllMoistnessCollectedTongue".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.QuiveringCuntMilkTongue".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SaltyCumDribblesBack".Localized());
            }
            else
            {
                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.StartSuckingMommyCock".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.FlexCocksSlapBelly".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.StrokeCocksOneHand".Localized());

                if (Player.LustPercent < 50)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.GrowsHard".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.GetsEvenHarder".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.BeforeEyesLicksLips".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.GirlyMonsterForwardAgain".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.DisappearingHindLegsAgain".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.LookingUpLonginglyCock".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.BigBrownMeetTongue".Localized());

                ShowImage("minomob-won-cocksucker");

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SerpentLikeTongueSnakes".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.AroundSackBeforeCurling".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.AroundBaseCirclesUp".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.AWord".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.TheWord".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.ShaftStoppingFrenulum".Localized(
                     Player.CockHead()
                ));

                if (!Player.IsTaur())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.BreakEyeContactOr".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SlowDownInSlightest".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.HummingSlipsDigitVagina".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.CirclingWigglingTongueLength".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.WhileHeStrokes".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.OneOfIgnoredPenises".Localized());
                    else
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.OnIgnoredPenis".Localized());
                }

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SentencePeriodAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.BeginCumSonsMouth".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.YourJismWord".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SquirtsRepeatedlyTongue".Localized());
                else
                    if (Player.CumQ() <= 250)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SquirtsOverAndBulging".Localized());
                    else
                        if (Player.CumQ() <= 1000)
                            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SpurtsFillingFirstBurst".Localized());
                        else
                        {
                            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.BlastsDribblesCorners".Localized());

                            if (Player.CumQ() >= 2000)
                                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.SpunkRunsFromNostrils".Localized());
                        }

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.PullsBackBeforeFinish".Localized());

                if (Player.HasVagina())
                    if (Player.Wetness() >= 5)
                        OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.CunnySquirtsOntoChest".Localized());
                    else
                        if (Player.Wetness() >= 3)
                            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.CunnyDripsEverywhereMakes".Localized());

                OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.ActuallyLooksSpoogeNose".Localized());
            }

            OutputText("Scenes.Minotaurmobscene.ForceMinitaurToGiveOral.FinishedPushBackGround".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void VictoryBJNippleFuckMinotaurGang()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.PullDownShakeChest".Localized());

            if (Player.TotalNipples() == 2)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.EachWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.TwoWord".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.OfLeakyNippleCunts".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.SecondLargestDaresAsk".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.ExplainWiggleRebelliousFools".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.BiggestShouldersAsideSnarls".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) == 3)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.MinitaurMuchSmallerChagrin".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.RestOfKinChagrin".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.PairRavenousEyesToy".Localized());

            ShowImage("minomob-nipplefuck");

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.SmileAndWord".Localized());

            if (Player.Tallness >= 72)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.DropDownWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.ShiftPositionWord".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.GetNipplesBetterAngle".Localized(
                 Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.SensationHotPricksStuffing".Localized());

            if (Player.BiggestBreastSize() <= 2)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.FewInchesWord".Localized());
            else
                if (Player.BiggestBreastSize() <= 4)
                    OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.InchesWord".Localized());
                else
                    if (Player.BiggestBreastSize() <= 8)
                        OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.MostOfFootWord".Localized());
                    else
                        if (Player.BiggestBreastSize() <= 14)
                            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.OverFootWord".Localized());
                        else
                            if (Player.BiggestBreastSize() <= 30)
                                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.NearlyTwoFeetWord".Localized());
                            else
                                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.WholeLengthWord".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.EquineDisappearFascination".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.MoanSlutCaressTestes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 7)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.ChasedJeersRemainderBrood".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.ShylyScratchesHeadLifts".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.GirlishMinitaurDivesMaw".Localized());

            if (Player.HasCock())
            {
                if (Player.HasVagina())
                    OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.AndVaginaDrool".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.DroolsWord".Localized());

                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.LamelyInsideClothingForgotten".Localized());
            }

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.FourBallsWritheContract".Localized());

            if (Player.BiggestLactation() >= 3)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.HoseSpraysMilkRods".Localized());
            else
                if (Player.BiggestLactation() >= 1.5f)
                    OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.DribbleMilkAroundRods".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.SentencePeriod".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.TwoCummingPullOut".Localized());

            if (!Player.MinotaurAddicted())
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.MealWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.NotherFixWord".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.SentencePeriodAgain".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.StandUpStretchCum".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) < 5)
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.RelaxFanLustyLooks".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.StareGloomilyLuckyThree".Localized());

            OutputText("Scenes.Minotaurmobscene.VictoryBJNippleFuckMinotaurGang.RedressSensuallyShowDare".Localized());

            Player.Orgasm("Nipples");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 1f));

            Player.MinoCumAddiction(10);

            Player.SlimeFeed();

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void MinotaurGangBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.SlumpingKneesCrowdDozens".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.GigglingSceneAboutHappen".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.AnswerChorusBoomingLaughs".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.DroppedPerchPillowsFlopping".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.PlacePleasantGirlsBare".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.RuntForcesThroughPouts".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.GiggleLazingSuckDozens".Localized());

            ShowImage("minotaur-gangbadend");

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.NoddingLickGrippingShaft".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.CuppingShudderingBallSack".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEnd.GiveSqueezeJerkBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 15f));

            DoNext(() => MinotaurGangBadEndPart2A());
        }

        public void MinotaurGangBadEndPart2A()
        {
            ClearOutput();

            ShowImage("minotaur-gangbadend-part2a");

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2A.ShakingNeedOwnRise".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2A.OverWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2A.BackSnakeLikeWord".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2A.PresentNeedyOpeningSmallest".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2A.MinitaurLooksDownThankful".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2A.AlreadyPleasantBuzzThrow".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            DoNext(() => MinotaurGangBadEndPart2B());
        }

        public void MinotaurGangBadEndPart2B()
        {
            ClearOutput();

            ShowImage("minotaur-gangbadend-part2b");

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.FeelLikeFloatingClouds".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.SlideNippleCuntsFinger".Localized());
            else
                if (Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.TugPullDrippyNipples".Localized());
                else
                    OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.TugAchyNipples".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.MoaningLikeWhoreGasp".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.RhythmicContractionsSetOff".Localized());

            if (Player.Pregnancy.Incubation == 0)
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.InsideEmptyWombWord".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.AgainstPluggedPregnantWomb".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.ReleasingSoSquirtEscapes".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart2B.SonSighsSlumpsSlipping".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -10f), new DynStat(GameStats.Libido, 10f), new DynStat(GameStats.Sensitivity, 10f));

            DoNext(() => MinotaurGangBadEndPart3A());
        }

        public void MinotaurGangBadEndPart3A()
        {
            ClearOutput();

            ShowImage("badend-minomob-I");

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3A.SomeTimeLaterWord".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3A.ParagraphBoldEnd".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3A.BlinkEyesAdjustDaylight".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3A.TodayLeadingAllStocks".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3A.StoppedPlaceHeadArms".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3A.HushSettlesOglingRumps".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 35f), new DynStat(GameStats.Sensitivity, 15f));

            DoNext(() => MinotaurGangBadEndPart3B());
        }

        public void MinotaurGangBadEndPart3B()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.AsBiggestPotentCrowding".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.BickeringBehindFistsStriking".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.SureSquirterBeforeDozen".Localized());
            else
                OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.SurePlentyBeforeDrizzling".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.QuimSoakedCockOut".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.PurrThickThroatStretching".Localized());

            ShowImage("badend-minomob-II");

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.ChemicalsWickBloodstream".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.CutBackOffBigger".Localized());

            OutputText("Scenes.Minotaurmobscene.MinotaurGangBadEndPart3B.PulledReplacedSloppySeconds".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 100f), new DynStat(GameStats.Sensitivity, 100f));

            GameOver();
        }

        public void DisciplineEldestMinotaurSon()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.WaywardSonsDropMacho".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.AskOldestCrossedArms".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.SnortedFacadeBraveryOver".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.DemandExplainOldestExample".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.LaughsMotherSetExample".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.DissatisfiedSmackTasteMouth".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.AfterRemoveLowerPortion".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.CooNotGettingPussy".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.SaidHintOfHonesty".Localized());

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.PointOutWreckPussy".Localized(
                 Player.VaginaDescript(0)
            ));

            ShowImage("minomob-discipline-eldest");

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.BurlyClearlyUnaccustomed".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.EverythingElseFoggyHaze".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Minotaurmobscene.DisciplineEldestMinotaurSon.FinishedPushOnBack".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

    }
}