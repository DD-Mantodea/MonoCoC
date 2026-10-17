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
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.World;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<MinervaScene>("coc:minerva")]
    public class MinervaScene : Scene, ITimeAware
    {
        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Minerva)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public override void Entry()
        {
            EncounterMinerva();
        }

        public float MinervaVCapacity()
        {

            return 90;
        }

        public float MinervaACapacity()
        {

            return 100;
        }

        public void MinervaSprite()
        {

        }

        public bool MinervaRomanced()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.MinervaLove) == 1;
        }

        public void TryToImpregnateMinerva()
        {

            var children = CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren);

            if (Pregnancy.KnockUpChance(Math.Clamp(30 - children * 2, 10, 30), 80) && children < 100)
                Pregnancy.KnockUpForce(PregnancyIds.PlayerFather, PregnancyIds.MinervaIncubation);
        }

        public void MinervaAppearance()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-appearance");

            OutputText("Scenes.Minervascene.MinervaAppearance.SirenStandsEightFoot".Localized());

            OutputText("Scenes.Minervascene.MinervaAppearance.FeatheredWingsSprout".Localized());

            OutputText("Scenes.Minervascene.MinervaAppearance.FeatherCoveredArms".Localized());

            OutputText("Scenes.Minervascene.MinervaAppearance.WideHipsThinWaist".Localized());

            OutputText("Scenes.Minervascene.MinervaAppearance.LegsRestSexualOrgans".Localized());

            OutputText("Scenes.Minervascene.MinervaAppearance.LargeDemonicFeet".Localized());

            OutputText("Scenes.Minervascene.MinervaAppearance.TubeTopShortShorts".Localized());

            ClearButtons();

            GenericMenu();
        }

        public void EncounterMinerva()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) < 10)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MetMinerva) > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.MetMinerva, CoC.Instance.Flags.Get(GameFlagIds.MetMinerva) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.MetMinerva) == 0)
                    FirstMinervaEncounter();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.MetMinerva) == 3)
                        MinervaThirdPlusEncounter();
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstoryLearned) > 0 && (int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.MinervaLove) == 0 && Random.Shared.Next(2) == 0)
                            MinervaAfterFiveTalksRomancing();
                        else
                            if (Random.Shared.Next(2) == 0)
                                MinervaThirdPlusEncounter();
                            else
                                RepeatEncounterMinerva();
            }
            else
                SceneInstance<MinervaPurificationScene>.Get().VisitTowerPostPurification();
        }

        public void FirstMinervaEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Minervascene.FirstMinervaEncounter.ReturningToMountains".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetSophieCounter) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) == 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) == 0 && true)
                OutputText("Scenes.Minervascene.FirstMinervaEncounter.ThatMatronlyHarpySophie".Localized());
            else
                OutputText("Scenes.Minervascene.FirstMinervaEncounter.SomethingInteresting".Localized());

            OutputText("Scenes.Minervascene.FirstMinervaEncounter.OrGetSomePlunder".Localized());

            if (Player.MinotaurAddicted())
                OutputText("Scenes.Minervascene.FirstMinervaEncounter.MightFindHunkyMinotaur".Localized());

            OutputText("Scenes.Minervascene.FirstMinervaEncounter.TrudgingOnUpMountain".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakePath".Localized(), TakeMinervasPath);

            AddButton(1, "Common.Button.IgnorePath".Localized(), IgnoreMinervasPath);
        }

        public void IgnoreMinervasPath()
        {
            ClearOutput();

            OutputText("Scenes.Minervascene.IgnoreMinervasPath.DecidePlaySafe".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeMinervasPath()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.TakeMinervasPath.NewPathGreatThings".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.CuriosityNewPath".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.EyesWidenInside".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.CloserInspectionSoil".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.LongerYouLookShadow".Localized());

            if (!Player.IsTaur() && !Player.IsDrider())
            {
                OutputText("Scenes.Minervascene.TakeMinervasPath.WingedBeingFlies".Localized());

                OutputText("Scenes.Minervascene.TakeMinervasPath.ThinkItsAHarpy".Localized());

                OutputText("Scenes.Minervascene.TakeMinervasPath.ThrashingLoosensGrip".Localized());
            }
            else
            {
                OutputText("Scenes.Minervascene.TakeMinervasPath.ShadowWingedBeingTackle".Localized());

                OutputText("Scenes.Minervascene.TakeMinervasPath.ThinkItsHarpy".Localized());
            }

            OutputText("Scenes.Minervascene.TakeMinervasPath.AttackedLiftsYouUp".Localized());

            if (Player.Tallness <= 96)
                OutputText("Scenes.Minervascene.TakeMinervasPath.OneHandWord".Localized());
            else
                OutputText("Scenes.Minervascene.TakeMinervasPath.BothHandsWord".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.SwingingOutOverAbyss".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Minervascene.TakeMinervasPath.WhoAreYouDemand".Localized());
            else
                OutputText("Scenes.Minervascene.TakeMinervasPath.CorruptedBeastDemand".Localized());

            if (Player.DemonScore() >= 3)
                OutputText("Scenes.Minervascene.TakeMinervasPath.FilthyDemonDemand".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.CreatureDemands".Localized());

            if (Player.IsGoo())
            {
                OutputText("Scenes.Minervascene.TakeMinervasPath.NutsToThis".Localized());

                if (Player.Corruption >= 75)
                    OutputText("Scenes.Minervascene.TakeMinervasPath.YetYouThink".Localized());

                OutputText("Scenes.Minervascene.TakeMinervasPath.IfSheWantsConflict".Localized());
            }
            else
                if (Player.CanFly())
                    OutputText("Scenes.Minervascene.TakeMinervasPath.SureYouCouldFly".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.HastilyExplainNoIntrude".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.SheAnalyzesYou".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Minervascene.TakeMinervasPath.WiseNotToTrust".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.WithWord".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Minervascene.TakeMinervasPath.YourOwnWord".Localized());
            else
                OutputText("Scenes.Minervascene.TakeMinervasPath.AWord".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.RelievedSigh".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.SheIsQuiteTall".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.UnnamedSharkHarpyOutfit".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.TapsClawedFoot".Localized());

            OutputText("Scenes.Minervascene.TakeMinervasPath.AdmitDifferentName".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MetMinerva, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Follow".Localized(), FollowMinerva);

            AddButton(1, "Common.Button.Dontfollow".Localized(), LeaveMinervasFirstEncounter);
        }

        public void FollowMinerva()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.FollowMinerva.FollowHerInside".Localized());

            ClearButtons();

            GenericMenu();

            RemoveButton(0);

            RemoveButton(2);
        }

        public void LeaveMinervasFirstEncounter()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.LeaveMinervasFirstEncounter.DontRiskHerLair".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RepeatEncounterMinerva()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.RepeatEncounterMinerva.MakeWayUpMountain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMinerva) == 4)
                OutputText("Scenes.Minervascene.RepeatEncounterMinerva.UnlockedOasisTower".Localized());

            OutputText("Scenes.Minervascene.RepeatEncounterMinerva.UponEnteringHome".Localized());

            OutputText("Scenes.Minervascene.RepeatEncounterMinerva.ApproachPondSheTwists".Localized());

            if (!MinervaRomanced())
                OutputText("Scenes.Minervascene.RepeatEncounterMinerva.YouHaveReturned".Localized());
            else
                OutputText("Scenes.Minervascene.RepeatEncounterMinerva.MmmMyLover".Localized());

            GenericMenu();
        }

        public void MinervaThirdPlusEncounter()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.MinervaThirdPlusEncounter.WayBackUpMountain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMinerva) == 4)
                OutputText("Scenes.Minervascene.MinervaThirdPlusEncounter.VisitedTowerEnough".Localized());

            OutputText("Scenes.Minervascene.MinervaThirdPlusEncounter.DontSeeMinerva".Localized());

            if (!MinervaRomanced())
                OutputText("Scenes.Minervascene.MinervaThirdPlusEncounter.GoodToSeeAgain".Localized());
            else
                OutputText("Scenes.Minervascene.MinervaThirdPlusEncounter.DarlingComingToVisit".Localized());

            OutputText("Scenes.Minervascene.MinervaThirdPlusEncounter.StridesTowardYou".Localized());

            ClearButtons();

            GenericMenu();
        }

        public void MinervaAfterFiveTalksRomancing()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.MinervaAfterFiveTalksRomancing.WayBackUpMountain".Localized());

            if (!Player.IsTaur() && !Player.IsDrider())
            {
                OutputText("Scenes.Minervascene.MinervaAfterFiveTalksRomancing.GrabbedAndLifted".Localized());

                OutputText("Scenes.Minervascene.MinervaAfterFiveTalksRomancing.LookingUpGrinning".Localized());

                ClearButtons();

                AddButton(0, "Combat.Ability.IvorySuccubusFlight.Name".Localized(), GoForASkyRideWithMinervaHeartBaring);

                AddButton(1, "Common.Button.Land".Localized(), DontFlyWithWithMeYouCrazyBitch);
            }
            else
            {
                OutputText("Scenes.Minervascene.MinervaAfterFiveTalksRomancing.GrinAsSirenGrabs".Localized());

                OutputText("Scenes.Minervascene.MinervaAfterFiveTalksRomancing.SheLiftsYouUp".Localized());

                OutputText("Scenes.Minervascene.MinervaAfterFiveTalksRomancing.TakesYourHandTour".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), MinervaOpensHerHeartAndDisplaysHerVentricals);
            }
        }

        public void GoForASkyRideWithMinervaHeartBaring()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-fly");

            OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.GrinWithExcitement".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.ClingWord".Localized());
            else
                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.HoldWord".Localized());

            OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.OntoTheSharkHarpy".Localized());

            if (!Player.CanFly())
                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.PleasedYouAccept".Localized());
            else
            {
                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.PleasedYouAccepted".Localized());

                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.SoundOfWingBeats".Localized());

                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.GettingAheadGrin".Localized());
            }

            OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.DiveFasterAndFaster".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.BarelySlimyCohesion".Localized());

            OutputText("Scenes.Minervascene.GoForASkyRideWithMinervaHeartBaring.ThinkItsTheEnd".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MinervaOpensHerHeartAndDisplaysHerVentricals);
        }

        public void MinervaOpensHerHeartAndDisplaysHerVentricals()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.MinervaOpensHerHeartAndDisplaysHerVentricals.HoldsYouStammers".Localized());

            OutputText("Scenes.Minervascene.MinervaOpensHerHeartAndDisplaysHerVentricals.LookAtBlueSkinned".Localized());

            OutputText("Scenes.Minervascene.MinervaOpensHerHeartAndDisplaysHerVentricals.ChuckleApologize".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.KissHer".Localized(), YeahSureMinervaILikeYouAndShit);

            AddButton(1, "Common.Button.Reject".Localized(), RejectMinervasLove);
        }

        public void YeahSureMinervaILikeYouAndShit()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.YeahSureMinervaILikeYouAndShit.KnowYourAnswerKiss".Localized());

            OutputText("Scenes.Minervascene.YeahSureMinervaILikeYouAndShit.ReturnHerGrasp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaLove, 1);

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), MinervaCowgirlSex);

            AddButton(1, "Common.Button.GetFucked".Localized(), MinervaLapSex);

            AddButton(2, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), MinervaCuddling);
        }

        public void RejectMinervasLove()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("impoverlord-loss-female");

            OutputText("Scenes.Minervascene.RejectMinervasLove.StareAwkwardly".Localized());

            if (Player.Corruption < 45)
                OutputText("Scenes.Minervascene.RejectMinervasLove.LookInHerEyes".Localized());
            else
                OutputText("Scenes.Minervascene.RejectMinervasLove.TellSheWantsThis".Localized());

            OutputText("Scenes.Minervascene.RejectMinervasLove.LookAwayConfess".Localized());

            OutputText("Scenes.Minervascene.RejectMinervasLove.HopeFades".Localized());

            OutputText("Scenes.Minervascene.RejectMinervasLove.GetUpStepOut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaLove, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DontFlyWithWithMeYouCrazyBitch()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.DontFlyWithWithMeYouCrazyBitch.LookDownUncertain".Localized());

            OutputText("Scenes.Minervascene.DontFlyWithWithMeYouCrazyBitch.MinervaLooksNervous".Localized());

            ClearButtons();

            GenericMenu();
        }

        public void FightMinerva()
        {
            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Minerva), () => Go<CampScene>(), canFlee: false);

            MinervaSprite();
        }

        public void GenericMenu(bool display = false)
        {

            ClearButtons();

            if (display)
                OutputText("Scenes.Minervascene.GenericMenu.WhatWillItBe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) >= 10)
            {
                SceneInstance<MinervaPurificationScene>.Get().PureMinervaMenu();

                return ;
            }

            AddButton(0, "Common.Button.Appearance".Localized(), MinervaAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), () => MinervaTalkSelect());

            if (Player.Lust >= 33)
                AddButton(2, "Common.Button.Sex".Localized(), () => MinervaSexMenu());
            else
                AddButton(2, "Common.Button.Sex".Localized(), null);

            AddButton(3, "Common.Button.Spar".Localized(), FightMinerva);

            AddButton(5, "Scenes.Faerie.SexMenu.ButtonEat".Localized(), EatSomethingYouCunt);

            AddButton(6, "Scenes.Lumberjack.Button.Drink".Localized(), GetADrinkYouBitch);

            if (MinervaRomanced() && View.Time.Hours >= 20)
                AddButton(9, "Common.Button.SleepWith".Localized(), SleepWithMinerva);
            else
                AddButton(9, "Common.Button.SleepWith".Localized(), null);

            if (Player.HasKeyItem("Rathazul's Purity Potion"))
            AddButton(10, "Common.Button.Purify".Localized(), SceneInstance<MinervaPurificationScene>.Get().PurificationByRathazul);

            if (Player.HasKeyItem("Marae's Seed"))
            AddButton(11, "Common.Button.PlantSeed".Localized(), SceneInstance<MinervaPurificationScene>.Get().PurificationByMarae);

            AddButton(14, "Common.Button.Leave".Localized(), MinervaLeave);
        }

        public void RepeatableMinervaRomanceScene()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.RepeatableMinervaRomanceScene.BackUpForRomance".Localized());

            OutputText("Scenes.Minervascene.RepeatableMinervaRomanceScene.HeadRightForTower".Localized());

            ClearButtons();

            GenericMenu();
        }

        public void MinervaTalkSelect(bool bath = true)
        {

            var choices = new List<Action> { TalkingToMinervaAboutBackstory };

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstoryLearned) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstory) == 0)
                {
                    TalkingToMinervaAboutBackstory();

                    return ;
                }

                choices.Add(() => TalkingToMinervaAboutBackstory());

                choices.Add(() => TalkingToMinervaAboutBackstory());

                choices.Add(() => TalkingToMinervaAboutBackstory());

                choices.Add(() => TalkingToMinervaAboutBackstory());

                choices.Add(() => TalkingToMinervaAboutBackstory());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) == 1)
            {
                SceneInstance<MinervaPurificationScene>.Get().StartPurification();

                return ;
            }

            choices.Add(() => TalkAboutTheSpringWithMinerva());

            if (MinervaRomanced() && CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstoryLearned) == 1)
                choices.Add(() => TalkWithMinervaAboutSharkGirlDaughter());

            if (MinervaRomanced())
                choices.Add(() => TalkToMinervaAboutHerCorruption());

            choices.Add(() => MinervaMotherhood());

            if (bath && MinervaRomanced())
                choices.Add(() => BathTimeTalkWithMinerva());

            choices[Random.Shared.Next(choices.Count)]();
        }

        public void TalkingToMinervaAboutBackstory()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-talk-backstory");

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstory) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstory, 1);

                OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.WantsSitAndTalk".Localized());

                OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.AskAboutHerPast".Localized());

                OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.NotesHerScar".Localized());

                OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ChangeTopicScar".Localized());

                OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.LostInThought".Localized());

                OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.EnoughForNow".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstory) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstory, 2);

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.SitAndTalkAgain".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.TakeSeatContinue".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.StoryAgainPickingUp".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ShuddersLooksDown".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.FoodInlandWorse".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ForceFedThin".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.BitesLipStriped".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.OneDidntLike".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ExhaustedStripedCame".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.GoesQuietGround".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.StruggledGotAway".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.PregnantFromTiger".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.TwelveDaysAfter".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.SmilesAtMemory".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.WhenISawHer".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.SeeingShesBetter".Localized());
                }
                else
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstory, 0);

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.SitAndTalkThird".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.SittingDownAskContinue".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.StoryAgainSmiling".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ReminiscingOpensEyes".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.NoIdeaGrowQuickly".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.SilentThinkingDaughter".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.FortunateThatIDid".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.NoIdeaWhatPut".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.HungryAteWalnuts".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.FlownForHours".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ShelterIfMonster".Localized());

                    OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.ArmAroundShoulder".Localized());

                    if (!MinervaRomanced())
                        OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.GoodFriendToHave".Localized());
                    else
                        OutputText("Scenes.Minervascene.TalkingToMinervaAboutBackstory.WonderfulLover".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstoryLearned, 1);
                }

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkAboutTheSpringWithMinerva()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-talk-spring");

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.TalkSpringSmile".Localized());

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.TheSpringQuestion".Localized());

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.DontKnowMuch".Localized());

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.CouldUseADip".Localized());

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.WaterSplashAround".Localized());

            Player.HasGottenWashed();

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.TryToGrabSiren".Localized());

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.RestTogether".Localized());

            OutputText("Scenes.Minervascene.TalkAboutTheSpringWithMinerva.PromiseVisitSoon".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkWithMinervaAboutSharkGirlDaughter()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-talk-sharkgirl");

            OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.TalkDaughterSmile".Localized());

            OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.RememberHerDaughter".Localized());

            OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.BittersweetSmile".Localized());

            OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.GrowUpQuicklyWorld".Localized());

            OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.NeverLeaveMySide".Localized());

            if (!MinervaRomanced())
                OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.EnoughForNowDaughter".Localized());
            else
            {
                OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.IncidentBetweenUs".Localized());

                OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.ItsMyOwnFault".Localized());

                OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.OnceSheSeducedMe".Localized());

                OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.ThankYouForListening".Localized());

                OutputText("Scenes.Minervascene.TalkWithMinervaAboutSharkGirlDaughter.StayLikeThisDaughter".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToMinervaAboutHerCorruption()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-talk-corruption");

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.JustWantToTalk".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.NoticeSadExpression".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.SomethingInsideMe".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.WideEyesGaze".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstoryLearned) == 1)
                OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.RememberFirstborn".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.DeclaresSourly".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.HandOnShoulderAsk".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.CuddlingUpTight".Localized());

            OutputText("Scenes.Minervascene.TalkToMinervaAboutHerCorruption.StayLikeThisCorruption".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void MinervaMotherhood()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-talk-motherhood");

            OutputText("Scenes.Minervascene.MinervaMotherhood.TalkMotherhoodAstonish".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.RackYourBrain".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.UncomfortableTopic".Localized());

            if (MinervaRomanced())
                OutputText("Scenes.Minervascene.MinervaMotherhood.LoverWord".Localized());
            else
                OutputText("Scenes.Minervascene.MinervaMotherhood.FriendWord".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.NoticeEntranced".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.SavedDamselsGoblins".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minervascene.MinervaMotherhood.QuicklyAnotherStory".Localized());

                if (!MinervaRomanced())
                    OutputText("Scenes.Minervascene.MinervaMotherhood.FriendWordAgain".Localized());
                else
                    OutputText("Scenes.Minervascene.MinervaMotherhood.LoverWordAgain".Localized());

                OutputText("Scenes.Minervascene.MinervaMotherhood.WeaveTaleBeeGirl".Localized());
            }

            if (Player.HasCock())
                OutputText("Scenes.Minervascene.MinervaMotherhood.RollingAnotherStory".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.TaleOverClaps".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Minervascene.MinervaMotherhood.AndThoseGoblins".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Minervascene.MinervaMotherhood.AndThatBeeGirl".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minervascene.MinervaMotherhood.GoblinsAndBeeGirl".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.IfStoriesWereReal".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.ShakeHeadShuffle".Localized());

            if (MinervaRomanced())
                OutputText("Scenes.Minervascene.MinervaMotherhood.HugKissPromise".Localized());
            else
                OutputText("Scenes.Minervascene.MinervaMotherhood.ComfortingSmile".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.ThankYouSoMuch".Localized());

            OutputText("Scenes.Minervascene.MinervaMotherhood.SeeHowLate".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BathTimeTalkWithMinerva()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-talk-bathtime");

            OutputText("Scenes.Minervascene.BathTimeTalkWithMinerva.TalkBathSmile".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), BathTimeWithMinerva);

            AddButton(1, "Common.Button.No".Localized(), NoBathTime);
        }

        public void NoBathTime()
        {
            ClearOutput();

            OutputText("Scenes.Minervascene.NoBathTime.SighsBriefMoment".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MinervaTalkSelect(false));
        }

        public void BathTimeWithMinerva()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-bathtime");

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.HopefulChuckle".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.GrabsHandPulls".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.GaspsLetsGo".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.CatchPeepingHerm".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.SlideArmsAround".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.BothSitInWater".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.NoFireAppears".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.CuddlesSteamingWater".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.HeatFadesTimeOut".Localized());

            OutputText("Scenes.Minervascene.BathTimeWithMinerva.LeaningInKiss".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            Player.ChangeFatigue(-30);

            Player.HasGottenWashed();

            DoNext(() => ReturnToCamp(1));
        }

        public void MinervaSexMenu(bool display = true)
        {
            ClearOutput();

            if (display)
            {
                MinervaSprite();

                OutputText("Scenes.Minervascene.MinervaSexMenu.TellWantSex".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) == 0)
                {
                    OutputText("Scenes.Minervascene.MinervaSexMenu.BlushesLooksYouUp".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Minervascene.MinervaSexMenu.NotInMyVagina".Localized());

                    OutputText("Scenes.Minervascene.MinervaSexMenu.AnythingSpecific".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Minervascene.MinervaSexMenu.WouldYouMindHow".Localized());

                    OutputText("Scenes.Minervascene.MinervaSexMenu.SayEnd".Localized());
                }
                else
                    if (Random.Shared.Next(2) == 0 && Player.HasCock() && Player.CockThatFits(MinervaACapacity()) < 0)
                        OutputText("Scenes.Minervascene.MinervaSexMenu.MonstrousMassStare".Localized());
                    else
                        if (!MinervaRomanced())
                        {
                            OutputText("Scenes.Minervascene.MinervaSexMenu.BlushesLooksUp".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Minervascene.MinervaSexMenu.SinceYouDontMind".Localized());

                            OutputText("Scenes.Minervascene.MinervaSexMenu.SayEndAgain".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Minervascene.MinervaSexMenu.BlushesSmiles".Localized());

                            if (Player.HasCock())
                                OutputText("Scenes.Minervascene.MinervaSexMenu.SinceWereTogether".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Minervascene.MinervaSexMenu.HappyToPound".Localized());

                            OutputText("Scenes.Minervascene.MinervaSexMenu.SayEndThird".Localized());
                        }
            }

            ClearButtons();

            AddButton(0, "Common.Button.Fuckherbutt".Localized(), null);

            AddButton(1, "Common.Button.Fuckcowgirl".Localized(), null);

            AddButton(2, "Common.Button.Restrainfuck".Localized(), null);

            AddButton(5, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), null);

            if (Player.HasCockThatFits(MinervaACapacity()))
                AddButton(0, "Common.Button.Fuckherbutt".Localized(), FuckMinervasAsshole);

            if (Player.HasCockThatFits(MinervaVCapacity()) && MinervaRomanced())
            {
                AddButton(1, "Common.Button.Fuckcowgirl".Localized(), MinervaCowgirlSex);

                AddButton(2, "Common.Button.Restrainfuck".Localized(), FuckMinervaWithHerHandsBehindHerBack);
            }

            AddButton(3, "Common.Button.Takeherdick".Localized(), ChooseVagOrAss);

            AddButton(4, "Common.Button.EatHerOut".Localized(), GoDownOnAHermAndLoveItYouDirtySlutYou);

            if (Player.HasCock())
                AddButton(5, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), LetMinervaSuckYouOff);

            AddButton(14, "Common.Button.Back".Localized(), () => GenericMenu(true));
        }

        public void FuckMinervasAsshole()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-sex-analpitch");

            var x = Player.CockThatFits(MinervaACapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.SmileHandOnThigh".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.SmirkSqueezeAss".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.NotYetReadyTease".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.NoSenseWaiting".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.AndYourVagina".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.ToTheOpenAir".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.BareAssCoax".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.ShaftsWord".Localized());
            else
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.ShaftWord".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.ToErectnessDrag".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.TeasedEnoughDrag".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x)
            ));

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.DrawBackSinkFingers".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.BitesLipPushesBack".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.DrawBackInch".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.IncreasedPaceBrutal".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.CantTakeItUnleash".Localized());

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.KeepGushingSpent".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.UnnaturalOrgasmPours".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.FreakishOrgasmEndless".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.PantingSlumpsForward".Localized());
            else
                if (Player.CumQ() < 5000)
                    OutputText("Scenes.Minervascene.FuckMinervasAsshole.GroaningSwollenBelly".Localized());
                else
                    OutputText("Scenes.Minervascene.FuckMinervasAsshole.GaspingHoldsInJizz".Localized(
                         Player.CockDescript(x)
                    ));

            OutputText("Scenes.Minervascene.FuckMinervasAsshole.AllowHerToHold".Localized());

            if (!MinervaRomanced())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedMinerva) == 0)
                    OutputText("Scenes.Minervascene.FuckMinervasAsshole.SadYoureLeaving".Localized());
                else
                    OutputText("Scenes.Minervascene.FuckMinervasAsshole.DisappointedSigh".Localized());
            }
            else
                OutputText("Scenes.Minervascene.FuckMinervasAsshole.LoveStruckSitsUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesButtfuckedMinerva, CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedMinerva) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void MinervaCowgirlSex()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-sex-cowgirl");

            var x = Player.CockThatFits(MinervaVCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.LookSirenUpDown".Localized());

            if (Player.Tallness <= 60)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.TrepidationSizeDiff".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.AskHerFemaleParts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaCowgirled) == 0)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.LooksUnsureBitesLip".Localized());
            else
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.HandThroughHair".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.RemoveClothesAside".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.GroanVenomCoursing".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.EnjoysYouHelpless".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.EndsTormentEntrance".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.MoanInDelight".Localized());

            if (Player.Tallness < 96)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.SuperiorSize".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.KeepYouPinned".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.MoanShudderPoisons".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.BegForMercy".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.BitingLipPushesDown".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.PleadNeedToCum".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.ShiversSeedBursts".Localized());
            else
                if (Player.CumQ() < 4000)
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.BlissfulReleaseSurges".Localized());
                else
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.IdleThoughtBittenOff".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.NotDoneLetOff".Localized(
                     Player.CockDescript(x)
                ));
            else
            {
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.NotDoneSqueezesDown".Localized());

                OutputText("Scenes.Minervascene.MinervaCowgirlSex.HeavingGaspAsk".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.YourBallsAnd".Localized());

                OutputText("Scenes.Minervascene.MinervaCowgirlSex.BaseStingingJellyfish".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.HasVagina())
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.FewSlitherIntoVagina".Localized());
            }

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.GaspWrithePleading".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.CryOutSecondEruption".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.BellyVisiblyBloated".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.SentencePeriod".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.UndeterredBounces".Localized());

            if (Player.CumQ() >= 500 && Player.CumQ() < 4000)
            {
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.BellyAlmostDoubles".Localized());

                OutputText("Scenes.Minervascene.MinervaCowgirlSex.UndeterredBouncesAgain".Localized());

                if (MinervaRomanced())
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.WithYourChild".Localized());

                OutputText("Scenes.Minervascene.MinervaCowgirlSex.SentencePeriodAgain".Localized());
            }

            if (Player.CumQ() >= 4000)
            {
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.DontKnowIfSafely".Localized());

                OutputText("Scenes.Minervascene.MinervaCowgirlSex.UndeterredBouncesThird".Localized());

                if (!MinervaRomanced())
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.SoBigAlmostPregnant".Localized());
                else
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.CummingSoMuch".Localized());
            }

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.SighingSatisfactionLeans".Localized());
            else
                if (Player.CumQ() < 4000)
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.SighingPantingRubs".Localized());
                else
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.GroaningShudderingInjection".Localized());

            OutputText("Scenes.Minervascene.MinervaCowgirlSex.LetHerHoldYou".Localized());

            if (!MinervaRomanced())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaCowgirled) == 0)
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.SadYoureLeaving".Localized());
                else
                    OutputText("Scenes.Minervascene.MinervaCowgirlSex.DisappointedSigh".Localized());
            }
            else
                OutputText("Scenes.Minervascene.MinervaCowgirlSex.LoveStruckSitsUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaCowgirled, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaCowgirled) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void MinervaLapSex()
        {
            ClearOutput();

            var x = Player.CockThatFits(MinervaACapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            MinervaSprite();

            ShowImage("minerva-sex-lapsex");

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervascene.MinervaLapSex.WhatKindOfFun".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.ApproachSlideArm".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.PushesBackSlidesHands".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.LetsGetClothesOff".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.AboutToAskSquirming".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.HandsSlideGropeAss".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minervascene.MinervaLapSex.HandTrailsToHerm".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervascene.MinervaLapSex.GropeFullChest".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.NoddingDazedDisentangle".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.ShiftHipsTease".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.GroanPullsYouDown".Localized());

            if (Player.Vaginas.Count == 0 && Player.Vaginas[0].Virgin)
                OutputText("Scenes.Minervascene.MinervaLapSex.PrickPlungesCunt".Localized());
            else
                OutputText("Scenes.Minervascene.MinervaLapSex.PrickPlungesPussy".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.BasksBallsDeep".Localized());

            Player.CuntChange(32, true, true, false);

            OutputText("Scenes.Minervascene.MinervaLapSex.GaspTightnessGrips".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.GrinsNibblesEar".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.WetFleshSquishing".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.EightFootGrunts".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.SoGoodSoGood".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.StuffedCoolFluid".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervascene.MinervaLapSex.YourOwnPhallus".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minervascene.MinervaLapSex.PluralSEs".Localized());

                OutputText("Scenes.Minervascene.MinervaLapSex.CummingAlongside".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Minervascene.MinervaLapSex.GushingOverBellies".Localized());
                else
                    if (Player.CumQ() < 4000)
                        OutputText("Scenes.Minervascene.MinervaLapSex.SplatteringSplooge".Localized());
                    else
                        OutputText("Scenes.Minervascene.MinervaLapSex.ExplodingOntoBodies".Localized());
            }

            OutputText("Scenes.Minervascene.MinervaLapSex.SentencePeriod".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.InfertileSeedFills".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.PantingCollapseHeap".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.ReachUpHugSiren".Localized());

            OutputText("Scenes.Minervascene.MinervaLapSex.AllowHerToHold".Localized());

            if (!MinervaRomanced())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) == 0)
                    OutputText("Scenes.Minervascene.MinervaLapSex.SadYoureLeaving".Localized());
                else
                    OutputText("Scenes.Minervascene.MinervaLapSex.DisappointedSigh".Localized());
            }
            else
                OutputText("Scenes.Minervascene.MinervaLapSex.LoveStruckSitsUp".Localized());

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaLapsexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) + 1);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void GoDownOnAHermAndLoveItYouDirtySlutYou()
        {
            ClearOutput();

            MinervaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            ShowImage("minerva-sex-eatout");

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.WhatKindOfFun".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.LickingLipsAnticipation".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.LookUpFromRear".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.GetDownBetweenLegs".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.HeyWhatDoing".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.TastingSoGood".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.HandsDriftToDick".Localized());

            if (Player.Tallness < 96)
                OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.FingersCantFit".Localized());
            else
                OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.FingersBarelyWrap".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.StrokeItSlowly".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.DiveInFullAttention".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic || Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.TongueSlithersDeeper".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.VaginalKissWorks".Localized());

            OutputText("Scenes.Minervascene.GoDownOnAHermAndLoveItYouDirtySlutYou.PullBackTooSoon".Localized());

            ClearButtons();

            AddButton(0, "Scenes.PlayerAppearance.Label.Balls".Localized(), LickMinervasBalls);

            AddButton(1, "Common.Button.NoBalls".Localized(), () => NoBallsMinervaAndContinuation());
        }

        public void LickMinervasBalls()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.LickMinervasBalls.NotEvenCloseTease".Localized());

            NoBallsMinervaAndContinuation(false);
        }

        public void NoBallsMinervaAndContinuation(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.ParagraphBreak".Localized());

            MinervaSprite();

            ShowImage("minerva-sex-eatout2");

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.CarefulMinistrationsShudder".Localized());

            if (!clear)
                OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.RetreatFromBalls".Localized());
            else
                OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.YouWord".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.MoveBackUpSnatch".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.MusicalCryEcstasy".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.BetweenLegsCockPulses".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.LyricalCriesEcho".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.CrescendoLeavesPanting".Localized());

            if (Player.HasLongTongue())
                OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.TongueRetractsQuim".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.SentencePeriod".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.ReleasedFromGrasp".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.ArmsWrapHug".Localized());

            OutputText("Scenes.Minervascene.NoBallsMinervaAndContinuation.LongWhileCuddled".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void LetMinervaSuckYouOff()
        {
            ClearOutput();

            var x = Player.CockThatFits(MinervaVCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            MinervaSprite();

            ShowImage("minerva-sex-blowjob");

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.WhatKindOfFun".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.GrinningDecide".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.UnsureLookRequest".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.LoungeRockSpread".Localized(
                     Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.SitDownSpreadLegs".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.AnxiousKneels".Localized(
                 Player.CockDescript(x)
            ));

            if (!(Player.CockArea(x) < 21))
                if (Player.CockArea(x) < 34)
                    OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.EyesWidenGrows".Localized());
                else
                    if (Player.CockArea(x) < 60)
                        OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.EyesWidenGrowsAgain".Localized());
                    else
                        OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.EyesWidenMouthOpens".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.RelaxingBreathDives".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.HandMovesBetweenLegs".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.HoldKissSlidesTongue".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.LickTeaseBalls".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.SentencePeriod".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.FirmlyCapturedTongue".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.GlossyLipsBobs".Localized());

            if (Player.CockArea(x) >= 50)
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.CockDownThroat".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.SentencePeriodAgain".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.SlurpingLickingPackage".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.TeasingTongueSucking".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.TongueFarAsCan".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.PantingCantHoldOut".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.BallsLurchSwell".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.SentencePeriodThird".Localized());

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.GripHeadBloating".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.FeelOrgasmBurst".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.FeelOrgasmBurstAgain".Localized());
                else
                    if (Player.CumQ() < 4000)
                        OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.FeelOrgasmSuddenlyBurst".Localized());
                    else
                        OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.OrgasmBurstsAgain".Localized(
                             Player.Mf("male", "herm")
                        ));

            OutputText("Scenes.Minervascene.LetMinervaSuckYouOff.RelaxAfterExertion".Localized());

            Player.Orgasm("Dick");

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void MinervaCuddling()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.MinervaCuddling.SatisfiedWithEmbrace".Localized());

            OutputText("Scenes.Minervascene.MinervaCuddling.GazesVibrantBlue".Localized());

            OutputText("Scenes.Minervascene.MinervaCuddling.LayThereCuddling".Localized());

            OutputText("Scenes.Minervascene.MinervaCuddling.LoveStruckSitsUp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MinervaLeave()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.MinervaLeave.CheckingInOnMinerva".Localized((CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0 ? " and your children" : ""), (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0 ? "we were" : "I was")));

            OutputText("Scenes.Minervascene.MinervaLeave.BidFarewell".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckMinervaWithHerHandsBehindHerBack()
        {
            ClearOutput();

            var x = Player.CockThatFits(MinervaVCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            MinervaSprite();

            ShowImage("minerva-sex-restrain");

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.LookLeanTonedHerm".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.PosingGetGoodLook".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.ReachesBackArm".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.ShakingHipsGive".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.NotReadyTeasing".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.HandsHitSpank".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.SoWetPerfectTime".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.SlideInWithEase".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.ContinuePoundingBalls".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.PatOnRump".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.ReadyDevilishGrin".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.GruntingHeatThrust".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.BodyFilledLust".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.NewPaceAnimalistic".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.NeedLustExplosiveEnd".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.StimulatedPotent".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.GroansSeedBursts".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.BlissfulReleaseSurges".Localized());
                else
                {
                    OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.InhumanLoadFills".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.MindlessGurgles".Localized());

                    OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.AmazementWombContains".Localized());

                    OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.LookDownGravid".Localized());
                }

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.LetGoWellFucked".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.AnotherSighLetsGo".Localized());

            OutputText("Scenes.Minervascene.FuckMinervaWithHerHandsBehindHerBack.AmusedGrinPatRump".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Orgasm("Dick");

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void PcGetsEatenOutByMinerva()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.WhatKindOfFun".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.GrinningDecide".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.WantHerTongueWork".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.PullsTongueBack".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.SitBackAgainstTree".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.AndMaleParts".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.SentencePeriod".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.LicksLipsMovesCloser".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.HandLiftsBalls".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.MoveDownToCunt".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.LickLipsStimulation".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.LicksWayUp".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.TongueAroundNuts".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.KissOnClit".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.GrinsSucksHarder".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.LickLipsMainEvent".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.PleasureEasyToRead".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.SlipperySerpentTongue".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.BackArchesScream".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.EvenStillCaught".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.VisionDarkensTunneling".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.GrinsAtWork".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.NotDoneWithYou".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.HandSlidesToClit".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.SensitiveRecentOrgasms".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.RenewedRelentless".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.ParagraphBreak".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.HandyStimulationThrobs".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.MouthClosedOverCunt".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.DeeplyContentSigh".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.NearlyAnHour".Localized());

            if (!MinervaRomanced())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaLezzesOut) == 0)
                    OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.SadYoureLeaving".Localized());
                else
                    OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.DisappointedSigh".Localized());
            }
            else
                OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.LoveStruckSitsUp".Localized());

            OutputText("Scenes.Minervascene.PcGetsEatenOutByMinerva.TurnStartHike".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaLezzesOut, CoC.Instance.Flags.Get(GameFlagIds.MinervaLezzesOut) + 1);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Player.Orgasm("Generic");

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void ChooseVagOrAss()
        {
            ClearOutput();

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minervascene.ChooseVagOrAss.WhichHoleQuestion".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Vagina".Localized(), MinervaLapSex);

                AddButton(1, "Common.Button.Ass".Localized(), GetButtFuckedYouSlut);
            }
            else
                GetButtFuckedYouSlut();
        }

        public void GetButtFuckedYouSlut()
        {
            ClearOutput();

            var x = Player.CockThatFits(MinervaACapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            MinervaSprite();

            ShowImage("minerva-sex-analcatch");

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.WhatKindOfFun".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.ApproachSlideArm".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.PushesBackSlidesHands".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.LetsGetClothesOff".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.AboutToAskSquirming".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.HandsSlideGropeAss".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.HandTrailsVagina".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.GropeFullChest".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.NoddingDazedDisentangle".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.ShiftHipsTease".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.GroanPullsYouDown".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.PrickPlungesBackdoor".Localized());

            Player.ButtChange(32, true);

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.BasksBallsDeep".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.GaspTightnessGrips".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.GrinsNibblesEar".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.WetFleshSquishing".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.EightFootGrunts".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.SoGoodSoGood".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.StuffedCoolFluid".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.YourOwnCunt".Localized());

                if (Player.AverageVaginalWetness() < 4)
                    OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.LeakingFemspunk".Localized());
                else
                    OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.ShootingFemspunk".Localized());
            }

            if (Player.HasVagina() && Player.HasCock())
                OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.WhileWord".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.YourOwnPhallus".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.PluralSEs".Localized());

                OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.CummingWord".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.GushingOverBellies".Localized());
                else
                    if (Player.CumQ() < 4000)
                        OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.SplatteringSplooge".Localized());
                    else
                        OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.ExplodingOntoBodies".Localized());
            }

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.SentencePeriod".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.SeedFillsQuiteFull".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.PantingCollapseHeap".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.ReachUpHugSiren".Localized());

            OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.AllowHerToHold".Localized());

            if (!MinervaRomanced())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) == 0)
                    OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.SadYoureLeaving".Localized());
                else
                    OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.DisappointedSigh".Localized());
            }
            else
                OutputText("Scenes.Minervascene.GetButtFuckedYouSlut.LoveStruckSitsUp".Localized());

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaLapsexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) + 1);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void LoseToMinerva()
        {
            ClearOutput();

            MinervaSprite();

            if (Player.Health < 1)
            {
                OutputText("Scenes.Minervascene.LoseToMinerva.FallBackFinalStrike".Localized());

                OutputText("Scenes.Minervascene.LoseToMinerva.FriendlySmilesCares".Localized());

                Player.HealthChange(Player.MaxHealth / 5);

                InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.PurePeach), () =>
                {
                    CombatScene.CleanupAfterCombat();

                    ReturnToCamp(1);
                });
            }

            if (!(Player.Health < 1))
            {
                OutputText("Scenes.Minervascene.LoseToMinerva.FallBackFinalMove".Localized());

                OutputText("Scenes.Minervascene.LoseToMinerva.SheWillTakeCare".Localized());

                MinervaSexMenu(false);

                AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
            }
        }

        public void BeatUpDatSharpie()
        {
            ClearOutput();

            MinervaSprite();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.Minervascene.BeatUpDatSharpie.FinalBlowHalberd".Localized());

                OutputText("Scenes.Minervascene.BeatUpDatSharpie.GetsUpFreshWeapon".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Minervascene.BeatUpDatSharpie.FinalMoveFinishes".Localized());

                OutputText("Scenes.Minervascene.BeatUpDatSharpie.LookingUpLust".Localized());

                MinervaSexMenu(false);

                AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
            }
        }

        public void EatSomethingYouCunt()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.EatSomethingYouCunt.LookAtHerGarden".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Purpeac".Localized(), GetPurePeach);

            AddButton(1, "Common.Button.CMint".Localized(), GetMint);

            AddButton(14, "Common.Button.Back".Localized(), () => GenericMenu(true));
        }

        public void GetPurePeach()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.GetPurePeach.ExamineStrangeTreats".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.PurePeach), () => ReturnToCamp(1));
        }

        public void GetMint()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.GetMint.ManyStrangeHerbs".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CinnamonMint), 1);

ReturnToCamp(1);
        }

        public void GetADrinkYouBitch()
        {
            ClearOutput();

            MinervaSprite();

            OutputText("Scenes.Minervascene.GetADrinkYouBitch.AskToDrinkSpring".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), DrinkDirectly);

            AddButton(1, "Common.Button.Bottle".Localized(), GetBottle);

            AddButton(14, "Common.Button.Back".Localized(), () => GenericMenu(true));
        }

        public void DrinkDirectly()
        {
            ClearOutput();

            OutputText("Scenes.Minervascene.DrinkDirectly.KneelDipHands".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -25f), new DynStat(GameStats.Corruption, -.5f));

            Player.RefillHunger(15);

            if (Player.Corruption > 50)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            if (Player.Corruption > 75)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GetBottle()
        {
            ClearOutput();

            OutputText("Scenes.Minervascene.GetBottle.FillWaterskin".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SpringWater), 1);

ReturnToCamp(1);
        }

        public void SleepWithMinerva()
        {
            ClearOutput();

            MinervaSprite();

            ShowImage("minerva-sleepwith");

            OutputText("Scenes.Minervascene.SleepWithMinerva.HugWantToSleep".Localized());

            OutputText("Scenes.Minervascene.SleepWithMinerva.GuidesComfortableSpot".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Minervascene.SleepWithMinerva.GetPlatebodyOff".Localized());
            else
                OutputText("Scenes.Minervascene.SleepWithMinerva.GetArmorOff".Localized());

            OutputText("Scenes.Minervascene.SleepWithMinerva.HeadOnBreasts".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Minervascene.SleepWithMinerva.ValeriaEnvelopesBoth".Localized());

            OutputText("Scenes.Minervascene.SleepWithMinerva.SweetDreams".Localized());

            DoNext(() => SleepWithMinervaProcess());
        }

        public void SleepWithMinervaII()
        {
            ClearOutput();

            ShowImage("minerva-sleepwith2");

            OutputText("Scenes.Minervascene.SleepWithMinervaII.TenHoursPass".Localized());

            OutputText("Scenes.Minervascene.SleepWithMinervaII.WakeRefreshed".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Minervascene.SleepWithMinervaII.ValeriaEncasesOnceMore".Localized());
            else
                OutputText("Scenes.Minervascene.SleepWithMinervaII.GetReDressed".Localized());

            OutputText("Scenes.Minervascene.SleepWithMinervaII.LeaveTheTower".Localized());

            AwardAchievement(AchievementIds.Getaway);

            SleepWithMinervaHeal();

            DoNext(() => ReturnToCamp(1));
        }

        public void SleepWithMinervaProcess()
        {
            ClearOutput();

            Player.Sleeping = true;

            View.Time.PassTime(10);

            SleepWithMinervaII();
        }

        public void SleepWithMinervaHeal()
        {

            CampScene.SleepRecovery(true);
        }

        public bool TimeChange()
        {
            var needNext = false;

            Pregnancy.PregnancyAdvance();

            if (Pregnancy.IsPregnant && Pregnancy.Incubation == 0)
            {
                SceneInstance<MinervaPurificationScene>.Get().MinervaGivesBirth();

                needNext = true;
            }

            return needNext;
        }

        public bool TimeChangeLarge()
        {
            var progress = CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress);

            if (progress > 0 && progress < 9 && progress != 4)
                CoC.Instance.Flags.Set(GameFlagIds.MinervaCorruptionProgress, progress + 1);

            return false;
        }
    }
}