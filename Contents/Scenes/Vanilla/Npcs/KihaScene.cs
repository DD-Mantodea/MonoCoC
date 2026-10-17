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

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<KihaScene>("coc:kiha")]
    public class KihaScene : Scene
    {

        public override void Entry()
        {

        }

        public void KillKiha()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaKilled, 1);

            OutputText("Scenes.Kihascene.KillKiha.ApproachDragonessPreparing".Localized());

            OutputText("Scenes.Kihascene.KillKiha.WordsAreAsIneffective".Localized());

            if (Player.Weapon.IsHolySword() || Player.Weapon.IsBladed())
                OutputText("Scenes.Kihascene.KillKiha.KnewSheWouldFight".Localized());
            else
                if (Player.Weapon.IsKnife())
                    OutputText("Scenes.Kihascene.KillKiha.SheWouldFight".Localized());
                else
                    if (Player.Weapon.IsRanged())
                        OutputText("Scenes.Kihascene.KillKiha.SheWouldFightThird".Localized());
                    else
                        if (Player.Weapon.IsBlunt())
                            OutputText("Scenes.Kihascene.KillKiha.SheWouldFightFourth".Localized());
                        else
                            if (Player.Weapon.IsSpear())
                                OutputText("Scenes.Kihascene.KillKiha.SheWouldFightFifth".Localized());
                            else
                                OutputText("Scenes.Kihascene.KillKiha.SheWouldFightSixth".Localized());

            InventoryScene.TakeItem(ItemIds.KihasGreataxe, CombatScene.CleanupAfterCombat);
        }

        public void EncounterKiha()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaNeedsToReactToHorsecocking) == 1)
            {
                SceneInstance<KihaFollowerScene>.Get().KihaReactsToHorseDicking();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) > 0 && !Player.IsPureEnough(66) && CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) != 1)
            {
                SceneInstance<KihaFollowerScene>.Get().KihaBitchesOutCorruptPCs();

                return ;
            }

            if (Player.IsPureEnough(66) && CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) == 1)
            {
                SceneInstance<KihaFollowerScene>.Get().KihaUnbitchesUncorruptedFolks();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) != 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) == 2 && CoC.Instance.Flags.Get(GameFlagIds.KihaAffection) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.KihaMoveInOffer) == 0)
                {
                    SceneInstance<KihaFollowerScene>.Get().KihaOffersToMoveIn();

                    return ;
                }

                SceneInstance<KihaFollowerScene>.Get().KihaFriendlyGreeting();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetKiha, CoC.Instance.Flags.Get(GameFlagIds.TimesMetKiha) + 1);

            if (Player.IsPureEnough(66) && CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) == 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) >= 3 && (int)Player.Gender > 0)
            {
                SceneInstance<KihaFollowerScene>.Get().KihaSpiderEventIntro();

                return ;
            }

            ShowImage("kiha-intro");

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetKiha) == 1)
            {
                OutputText("Scenes.Kihascene.EncounterKiha.ImposingFigureDropsOut".Localized());

                OutputText("Scenes.Kihascene.EncounterKiha.CommandsDraconicWoman".Localized(
                    Player.Mf("dick-head", "slut-bag")
                ));

                OutputText("Scenes.Kihascene.EncounterKiha.WhatDoYouDo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), MeetKihaAndFight);

                AddButton(1, "Common.Button.AskWhy".Localized(), AskWhy);

                AddButton(2, "Common.Button.BuyPassage".Localized(), OfferToBuyPassageFromKiha);

                AddButton(14, "Common.Button.Leave".Localized(), LeaveWhenMeetingAgressiveKiha);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PcWinLastKihaFight) == 1)
                {
                    OutputText("Scenes.Kihascene.EncounterKiha.PlummetsFromSkySlamming".Localized());

                    OutputText("Scenes.Kihascene.EncounterKiha.ScreamsOutInDefiance".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.PcWinLastKihaFight, 0);

                    var kihaEnemy = EnemyRegistry.Create(new Identifier("coc", "kiha"));

            kihaEnemy.OnDefeated = _ => KihaVictoryIntroduction();

            kihaEnemy.OnWon = _ => KihaLossIntro();

            CombatScene.Start(kihaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KihaToll) == 1 && Random.Shared.Next(2) == 0)
                    {
                        OutputText("Scenes.Kihascene.EncounterKiha.StepsOutFromBehind".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Fight".Localized(), MeetKihaAndFight);

                        AddButton(1, "Common.Button.Pay".Localized(), PayKihaTribute);

                        if (Player.Gems < 200)
                        {
                            OutputText("Scenes.Kihascene.EncounterKiha.CantAffordToPay".Localized());

                            RemoveButton(1);
                        }

                        AddButton(2, "Common.Button.MyIdea".Localized(), TellKihaTributeWasYourIdea);

                        AddButton(14, "Common.Button.Leave".Localized(), LeaveWhenMeetingAgressiveKiha);
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.KihaToll) > 1 && (CoC.Instance.Flags.Get(GameFlagIds.KihaTollDuration) == 1 || (CoC.Instance.Flags.Get(GameFlagIds.KihaTollDuration) == 0 && Random.Shared.Next(2) == 0)))
                        {
                            OutputText("Scenes.Kihascene.EncounterKiha.StepsOutBehindAgain".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Fight".Localized(), MeetKihaAndFight);

                            AddButton(1, "Common.Button.Pay".Localized(), PayKihaTribute);

                            if (Player.Gems < 200)
                            {
                                OutputText("Scenes.Kihascene.EncounterKiha.CantAffordAgain".Localized());

                                RemoveButton(1);
                            }

                            AddButton(2, "Common.Button.MyIdea".Localized(), TellKihaTributeWasYourIdea);

                            AddButton(14, "Common.Button.Leave".Localized(), LeaveWhenMeetingAgressiveKiha);
                        }
                        else
                        {
                            OutputText("Scenes.Kihascene.EncounterKiha.NearbyTreeExplodes".Localized());

                            OutputText("Scenes.Kihascene.EncounterKiha.CommandsAdvancingOnYou".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.KihaToll) == 0)
                            {
                                OutputText("Scenes.Kihascene.EncounterKiha.IfYouHurry".Localized());

                                ClearButtons();

                                AddButton(0, "Common.Button.Fight".Localized(), MeetKihaAndFight);

                                AddButton(1, "Common.Button.AskWhy".Localized(), AskWhy);

                                AddButton(2, "Common.Button.BuyPassage".Localized(), OfferToBuyPassageFromKiha);

                                AddButton(14, "Common.Button.Leave".Localized(), LeaveWhenMeetingAgressiveKiha);
                            }
                            else
                            {
                                OutputText("Scenes.Kihascene.EncounterKiha.ItsAFight".Localized());

                                var kihaEnemy = EnemyRegistry.Create(new Identifier("coc", "kiha"));

            kihaEnemy.OnDefeated = _ => KihaVictoryIntroduction();

            kihaEnemy.OnWon = _ => KihaLossIntro();

            CombatScene.Start(kihaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
                            }
                        }
        }

        public void OfferToBuyPassageFromKiha()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.OfferToBuyPassageFromKiha.JingleYourGemPouch".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaToll, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), MeetKihaAndFight);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveWhenMeetingAgressiveKiha);
        }

        public void LeaveWhenMeetingAgressiveKiha()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.LeaveWhenMeetingAgressiveKiha.NodAndStepBack".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MeetKihaAndFight()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.MeetKihaAndFight.StepCloserAndProclaim".Localized());

            var kihaEnemy = EnemyRegistry.Create(new Identifier("coc", "kiha"));

            kihaEnemy.OnDefeated = _ => KihaVictoryIntroduction();

            kihaEnemy.OnWon = _ => KihaLossIntro();

            CombatScene.Start(kihaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void AskWhy()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.AskWhy.AskWhyShesTrying".Localized());

            OutputText("Scenes.Kihascene.AskWhy.ThinkingYouFoundA".Localized());

            var kihaEnemy = EnemyRegistry.Create(new Identifier("coc", "kiha"));

            kihaEnemy.OnDefeated = _ => KihaVictoryIntroduction();

            kihaEnemy.OnWon = _ => KihaLossIntro();

            CombatScene.Start(kihaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void PayKihaTribute()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.PayKihaTribute.SighAndPayTribute".Localized());

            Player.Gems -= 200;

            CoC.Instance.Flags.Set(GameFlagIds.KihaTollDuration, 3 + Random.Shared.Next(4));

            CoC.Instance.Flags.Set(GameFlagIds.KihaToll, CoC.Instance.Flags.Get(GameFlagIds.KihaToll) + 1);

            DoNext(() => KihaExplore());
        }

        public void KihaExplore(bool clearScreen = true)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaTollDuration, CoC.Instance.Flags.Get(GameFlagIds.KihaTollDuration) - 1);

            float eventRoll = Random.Shared.Next(10);

            Identifier? itype = null;

            if (eventRoll == 0 && CoC.Instance.Flags.Get(GameFlagIds.GottenInquisitorArmor) == 0)
            {
                InquisitorRobesDiscovery();

                return ;
            }

            if (eventRoll < 5)
            {
                OutputText("Scenes.Kihascene.KihaExplore.WanderThroughSwamp".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (eventRoll < 7)
                    itype = ItemIds.Reducto;
                else
                    if (eventRoll < 8)
                        itype = ItemIds.GroPlus;
                    else
                        if (eventRoll < 9)
                            itype = ConsumableIds.Coal;
                        else
                            if (eventRoll < 10)
                                itype = ConsumableIds.ToughSilk;

            OutputText("Scenes.Kihascene.KihaExplore.WhileExploringFindItem".Localized());

            if (itype != null) InventoryScene.TakeItem(itype.Value, () => ReturnToCamp(1));
        }

        public void TellKihaTributeWasYourIdea()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.TellKihaTributeWasYourIdea.AskWhySheChanged".Localized());

            OutputText("Scenes.Kihascene.TellKihaTributeWasYourIdea.DragonGirlYanksAxe".Localized());

            var kihaEnemy = EnemyRegistry.Create(new Identifier("coc", "kiha"));

            kihaEnemy.OnDefeated = _ => KihaVictoryIntroduction();

            kihaEnemy.OnWon = _ => KihaLossIntro();

            CombatScene.Start(kihaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void KihaVictoryIntroduction()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PcWinLastKihaFight, 1);

            OutputText("Scenes.Kihascene.KihaVictoryIntroduction.SwaysBackAndForth".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Kihascene.KihaVictoryIntroduction.WithLustWord".Localized());
            else
                OutputText("Scenes.Kihascene.KihaVictoryIntroduction.WeaklyWord".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryIntroduction.SheTriesToRise".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Kihascene.KihaVictoryIntroduction.ShoutsStruggling".Localized());
            else
                OutputText("Scenes.Kihascene.KihaVictoryIntroduction.MuttersEvenThough".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryIntroduction.WannabeDragonNow".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryIntroduction.CouldForciblyMasturbate".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Masturbate".Localized(), TsundereMasturbationChristmasCarol);

            if (Player.Lust >= 33 && Player.HasVagina())
            {
                OutputText("Scenes.Kihascene.KihaVictoryIntroduction.CouldUseHerTail".Localized());

                AddButton(1, "Common.Button.UseTail".Localized(), KihaVictoryPomfTail);
            }

            if ((Player.HasCock()) && (Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()) != -1 && Player.Lust >= 33))
                {
                    OutputText("Scenes.Kihascene.KihaVictoryIntroduction.CouldFuckHerVagina".Localized());

                    if (Player.CockThatFits2(CombatController.Instance!.Context.Enemy.VaginalCapacity()) != -1 && Player.CockTotal() > 1)
                        OutputText("Scenes.Kihascene.KihaVictoryIntroduction.AndAss".Localized());

                    OutputText("Scenes.Kihascene.KihaVictoryIntroduction.SentencePeriod".Localized());

                    AddButton(2, "Common.Button.Fuckherpussy".Localized(), VictoryDickKiha);
                }

            if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) && Player.Lust >= 33)
                AddButton(3, "Common.Button.Fuckherass".Localized(), AnalRapuzulaKiha);

            if (Player.IsPureEnough(40))
            {
                OutputText("Scenes.Kihascene.KihaVictoryIntroduction.NowShesACaptive".Localized());

                AddButton(4, "Common.Button.Talk".Localized(), RapeKihaWithWORDS);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) > 0)
                AddButton(5, "Scenes.Aiko.YamataLoses.ButtonFinishHer".Localized(), KillKiha);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void KihaLossIntro()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Kihascene.KihaLossIntro.GiveUpOnFighting".Localized());
            else
                OutputText("Scenes.Kihascene.KihaLossIntro.CollapseTooWounded".Localized());

            if (Player.BiggestLactation() >= 2)
                KihaMilkTitHumiliation();
            else
                if (CurrentMonster.LustPercent >= 50)
                {
                    OutputText("Scenes.Kihascene.KihaLossIntro.DragonessSighsGlances".Localized());

                    OutputText("Scenes.Kihascene.KihaLossIntro.LeavingAxeToRest".Localized());

                    if (Player.HasCock())
                        OutputText(Player.MultiCockDescriptLight());

                    if (Player.HasCock() && Player.HasVagina())
                        OutputText("Scenes.Kihascene.KihaLossIntro.AndWord".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript());

                    OutputText("Scenes.Kihascene.KihaLossIntro.PeriodBreak".Localized());

                    if (Player.Gender == Gender.Genderless)
                        KihaGenderlessBeating();
                    else
                        if (Player.Gender == Gender.Male)
                            KihaRapesMen();
                        else
                            if (Player.Gender == Gender.Hermaphrodite)
                                KihaRapesHerms();
                            else
                                if (Player.Gender == Gender.Female)
                                    KihaRapesLittleGirlsISawItOnTheNews();
                }
                else
                {
                    OutputText("Scenes.Kihascene.KihaLossIntro.PullsHerAxeBack".Localized());

                    Player.HealthChange(-1000);

                    CombatScene.CleanupAfterCombat();
                }
        }

        public void KihaMilkTitHumiliation()
        {
            ClearOutput();

            ShowImage("kiha-loss-milking");

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.SlideToGroundToo".Localized());
            else
                OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.SlideToGroundAgain".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.SnortsExpellingBurst".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.AsksLookingDownAt".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.RecoilsAtTheDrops".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.ScalyWomanLeansDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.NoticingStiffening".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.NoticingScentOfYour".Localized());
                else
                    OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.NoticingFlushOfYour".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.CallowlyMilksBreasts".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.EvenHarderTugReleases".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.SmackSound".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.HandCracksAcrossFace".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.BlinkAwayTheTears".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.ThreatensWithSadisticSmile".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.KeepsAtItMilking".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.DeclaresRiflesThrough".Localized());

            OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.TrudgeBackToCamp".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.HowFarYouveFallen".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.HowConfusingAllIs".Localized());
                else
                    OutputText("Scenes.Kihascene.KihaMilkTitHumiliation.DamnThatWasHot".Localized());

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Sensitivity, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void KihaGenderlessBeating()
        {

            OutputText("Scenes.Kihascene.KihaGenderlessBeating.YellsNoticingYour".Localized());

            Player.HealthChange(-1000);

            CombatScene.CleanupAfterCombat();
        }

        public void KihaRapesMen()
        {
            ClearOutput();

            ShowImage("kiha-loss-male");

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            OutputText("Scenes.Kihascene.KihaRapesMen.TryToRiseBut".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.SwiftKickToYour".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.HaunchesClenchAnd".Localized());

            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Kihascene.KihaRapesMen.RemarksOffHandedly".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.CunnyPracticallyDrools".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Kihascene.KihaRapesMen.RestingHeelOfFoot".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.CommandsAsTailPicks".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.DragonGirlLittleShow".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihascene.KihaRapesMen.GushingHoneyPooled".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihascene.KihaRapesMen.RumblingGrowl".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasTailInsteadOfLegs() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard)
                OutputText("Scenes.Kihascene.KihaRapesMen.TailCurlsAround".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.TailInYourMouth".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kihascene.KihaRapesMen.PracticallyLeaping".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.GeyserOfHotSpunk".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Kihascene.KihaRapesMen.UnoccupiedShaftsSpray".Localized());
                else
                    OutputText("Scenes.Kihascene.KihaRapesMen.UnoccupiedShaftSprays".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.PullsSphincterTight".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaChokedOutPc, 1);

            OutputText("Scenes.Kihascene.KihaRapesMen.PurrsForgettingHostile".Localized());

            OutputText("Scenes.Kihascene.KihaRapesMen.WhenYouComeTo".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void KihaRapesHerms()
        {
            ClearOutput();

            ShowImage("kiha-loss-herm");

            OutputText("Scenes.Kihascene.KihaRapesHerms.DragonessRollsHerEyes".Localized());

            OutputText("Scenes.Kihascene.KihaRapesHerms.OpenYourMouthShe".Localized());

            OutputText("Scenes.Kihascene.KihaRapesHerms.HeatOfHerCloseness".Localized());

            if (Player.IsGoo() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Kihascene.KihaRapesHerms.SlidingBackwardSlightly".Localized());
            else
                OutputText("Scenes.Kihascene.KihaRapesHerms.HookingLegUnderOne".Localized());

            OutputText("Scenes.Kihascene.KihaRapesHerms.RepositionsHerselfSo".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Kihascene.KihaRapesHerms.SoLostInThe".Localized());

            var dicked = false;

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)))
            {
                OutputText("Scenes.Kihascene.KihaRapesHerms.TwoOfYouEntwine".Localized());

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.IncubiDraft));

                OutputText("Scenes.Kihascene.KihaRapesHerms.WithoutBreakingStride".Localized());

                dicked = true;
            }

            OutputText("Scenes.Kihascene.KihaRapesHerms.WhenKihaClimaxes".Localized(
                Player.VaginaDescript(0)
            ));

            if (dicked)
                OutputText("Scenes.Kihascene.KihaRapesHerms.StepsBackIntoThe".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 3f));

            CombatScene.CleanupAfterCombat();
        }

        public void TsundereMasturbationChristmasCarol()
        {
            ClearOutput();

            ShowImage("kiha-mutualmasturbation");

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WalkTowardsDraconicWoman".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WantonWord".Localized());
            else
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.ButDispiritedWord".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.SlumpingAgainstBark".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.ArousedGenitalsWord".Localized());
            else
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.DistinctLackOfGenitals".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.PeriodBreak".Localized());

            if (Player.CockTotal() > 1 || Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WhatGoingToDo".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.GetThatAway".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WhatDoYouThink".Localized());
                    else
                        OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WhyComingCloser".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.TodayHoweverDontHave".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.SeeingAsYouDont".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WithoutSayingAWord".Localized());

            if (Player.Tallness < 72)
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.ThoughHiddenBehindHer".Localized());
            else
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.ThoughMuchTallerThan".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.AttemptsToStruggle".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.GirlTriesToSpeak".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.NoticeDragonGirls".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.AgainstHerSoftFlesh".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.BeadOfPreCum".Localized());
                else
                    OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.SmallBeadsForming".Localized());
            }
            else
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.AgainstHardRidges".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.IncreaseThePace".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.NothingYourHotAnd".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.MoistJuicesFlush".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.AccidentallyBrushing".Localized());

                Player.Orgasm("Dick");
            }

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.SighsInReliefTuckered".Localized());

            OutputText("Scenes.Kihascene.TsundereMasturbationChristmasCarol.WhispersBeforeTurning".Localized());

            if (!Player.HasCock())
                Player.ApplyStats(new DynStat(GameStats.Lust, 120f));

            CombatScene.CleanupAfterCombat();
        }

        public void KihaVictoryPomfTail()
        {
            ClearOutput();

            ShowImage("kiha-taildildo");

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.ApproachTheWord".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.LustyWord".Localized());
            else
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.WoundedWord".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.DragonessWickedIdeas".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.GearLandsBehindYou".Localized());

            if (silly)
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.WithAPomf".Localized());
            else
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.SentencePeriod".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.NowNudeSwayBack".Localized());

            if (Player.BiggestBreastSize() <= 4)
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.LooksAwayAndBlushes".Localized());
            else
                if (Player.BiggestBreastSize() <= 12)
                    OutputText("Scenes.Kihascene.KihaVictoryPomfTail.SighsAndSays".Localized());
                else
                    OutputText("Scenes.Kihascene.KihaVictoryPomfTail.LaughsAndTaunts".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.TireOfConstantInsults".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.EyesWellUpWith".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.CumWord".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.SweatSoakedUnderwear".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.ReachBackDownOnce".Localized());

            if (Player.Wetness() <= 3)
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.GrowsWetWord".Localized());
            else
                if (Player.Wetness() <= 4)
                    OutputText("Scenes.Kihascene.KihaVictoryPomfTail.QuicklyGrowsSlimyWord".Localized());
                else
                    OutputText("Scenes.Kihascene.KihaVictoryPomfTail.ImmediatelySoaked".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.PeriodBreak".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.SufficientlyLubricated".Localized());

            Player.CuntChange(1, true, false, true);

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.GodsShesSoWarm".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.LeaningBackAgainstTree".Localized());

            Player.CuntChange(35, true, false, true);

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.MoaningLookAtHer".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.ThatsHerLoss".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Kihascene.KihaVictoryPomfTail.DrippingWord".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Kihascene.KihaVictoryPomfTail.LeakingWord".Localized());
                else
                    if (Player.Wetness() < 5)
                        OutputText("Scenes.Kihascene.KihaVictoryPomfTail.GushingWord".Localized());
                    else
                        OutputText("Scenes.Kihascene.KihaVictoryPomfTail.SprayingWord".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.GirlCumCumsHard".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.MoaningStirsYouFrom".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.DraconicWomanSnaps".Localized());

            OutputText("Scenes.Kihascene.KihaVictoryPomfTail.OnlyResponseDressed".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryDickKiha()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            var y = Player.CockThatFits2(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            ShowImage("kiha-vagfuck");

            OutputText("Scenes.Kihascene.VictoryDickKiha.WalkUpAndPush".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Kihascene.VictoryDickKiha.JustBecauseYouveAroused".Localized());
            else
                OutputText("Scenes.Kihascene.VictoryDickKiha.DontThinkIllEnjoy".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.SayEndBreak".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.CertainlyHasMouth".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Kihascene.VictoryDickKiha.RubbingBackAndForth".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.AnnoyedTwistAppendage".Localized());

            if (Player.LustPercent > 60)
                OutputText("Scenes.Kihascene.VictoryDickKiha.OnlyMakesHarderWord".Localized());
            else
                OutputText("Scenes.Kihascene.VictoryDickKiha.QuicklyMakesHardWord".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.RearMostExtremityYanks".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.PausingForAMoment".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kihascene.VictoryDickKiha.AndVaginaWord".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.ToTheHumidAir".Localized());

            OutputText("Scenes.Kihascene.VictoryDickKiha.AdvanceOnHelplessForm".Localized(
                Player.CockDescript(x)
            ));

            if (y != -1 && Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kihascene.VictoryDickKiha.PerfectDrippingEntrance".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.IsWord".Localized());
                else
                    OutputText("Scenes.Kihascene.VictoryDickKiha.EsAreWord".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.AchingSoMuch".Localized(
                    Player.CockDescript(y)
                ));

                OutputText("Scenes.Kihascene.VictoryDickKiha.ProtestsTryingToRetain".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.WrithesAndPants".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.SighAndSlapFace".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.CutHerSnobbyRanting".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.FromNowhereTailPresses".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaChokedOutPc, 1);

                float z = -1;

                var temp = Player.Cocks.Count;

                while (temp > 0)
                {
                    temp--;

                    if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle") && temp != x && temp != y)
                    {
                        z = temp;

                        break;
                    }
                }

                if (Player.IsNaga() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard || z >= 0)
                {
                    OutputText("Scenes.Kihascene.VictoryDickKiha.SpitefullyYour".Localized());

                    if (z == -1)
                        OutputText("Scenes.Kihascene.VictoryDickKiha.TailWord".Localized());
                    else
                        OutputText("Scenes.Kihascene.VictoryDickKiha.IdleTentacleWord".Localized());

                    OutputText("Scenes.Kihascene.VictoryDickKiha.RespondsInKindBending".Localized());

                    if (z >= 0)
                        OutputText("Scenes.Kihascene.VictoryDickKiha.PlungingHeadOfCock".Localized());

                    OutputText("Scenes.Kihascene.VictoryDickKiha.ParagraphBreak".Localized());
                }

                OutputText("Scenes.Kihascene.VictoryDickKiha.DullyRealizeTwitching".Localized());

                if (Player.IsNaga() || Player.Body.Get<TailPart>().Id == BodyPartTypes.Lizard || z >= 0)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.GaspingMouth".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.TightAnusWithWaves".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.BellyDistendsStuffed".Localized());

                if (Player.CockTotal() > 2)
                {
                    OutputText("Scenes.Kihascene.VictoryDickKiha.EvenCoatHerBelly".Localized());

                    if (Player.CockTotal() > 3)
                        OutputText("Scenes.Kihascene.VictoryDickKiha.PluralEs".Localized());

                    OutputText("Scenes.Kihascene.VictoryDickKiha.SentencePeriod".Localized());
                }

                OutputText("Scenes.Kihascene.VictoryDickKiha.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.SlumpingShaftsContinue".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.SighAndPullBack".Localized());
            }
            else
            {
                OutputText("Scenes.Kihascene.VictoryDickKiha.PerfectDrippingEntranceAgain".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kihascene.VictoryDickKiha.EventuallyAccustomed".Localized(
                    Player.CockDescript(x)
                ));

                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.MoreArousedFeigned".Localized());
                else
                    OutputText("Scenes.Kihascene.VictoryDickKiha.VeryArousedInSpite".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.PickUpThePace".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.AllTooSoonFind".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kihascene.VictoryDickKiha.PushYourselfAllThe".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.ChubbyBulgeAppears".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.EvenAfterThatGouts".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.YouPullOutWord".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Kihascene.VictoryDickKiha.CleanUpGetDressed".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Kihascene.VictoryDickKiha.WipeYourCockOff".Localized());
                    else
                        OutputText("Scenes.Kihascene.VictoryDickKiha.SmearDribblingCock".Localized());

                OutputText("Scenes.Kihascene.VictoryDickKiha.SomehowDoubtShellLearn".Localized());
            }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void KihaRapesLittleGirlsISawItOnTheNews()
        {
            ClearOutput();

            ShowImage("kiha-loss-female");

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.ExclaimsMockingly".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.MoanWeaklyBegging".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.DespitePleasureCrumbling".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.MurmursWhenYouFinally".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.GratefulForForked".Localized());
            else
                if (Player.HasLongTongue())
                    OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.InhumanlyLongTongue".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.ParagraphBreak".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.DespiteEnthusiasticEfforts".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.ShudderInEcstasy".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.UnableUnwillingAnything".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.JuicesSprayIntoFace".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.RespondsInKindGushing".Localized());

            OutputText("Scenes.Kihascene.KihaRapesLittleGirlsISawItOnTheNews.KihaLongGone".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            CombatScene.CleanupAfterCombat();
        }

        public void RapeKihaWithWORDS()
        {
            ClearOutput();

            ShowImage("kiha-forced-talk");

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) == 0)
            {
                OutputText("Scenes.Kihascene.RapeKihaWithWORDS.KneelingMeetDefeated".Localized());

                OutputText("Scenes.Kihascene.RapeKihaWithWORDS.NodHeadAndAsk".Localized());

                OutputText("Scenes.Kihascene.RapeKihaWithWORDS.ChuckleALittle".Localized());

                OutputText("Scenes.Kihascene.RapeKihaWithWORDS.RecoilsAtYourWords".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) == 1)
                {
                    OutputText("Scenes.Kihascene.RapeKihaWithWORDS.OnceAgainCrouchDown".Localized());

                    OutputText("Scenes.Kihascene.RapeKihaWithWORDS.AskIfSheReally".Localized());

                    OutputText("Scenes.Kihascene.RapeKihaWithWORDS.ShruggingSheUnlikely".Localized());

                    OutputText("Scenes.Kihascene.RapeKihaWithWORDS.OnceFinishSheSpeaks".Localized());

                    if (silly && CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) > 0 && false)
                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.FeelsLikeLottie".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) == 2)
                    {
                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.EvenThoughYouveTaken".Localized());

                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.AskHerStory".Localized());

                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.NodAgainToWhich".Localized());

                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.KihaRaves".Localized());

                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.SighSurprisinglyGets".Localized());

                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.DraconicWomanLooksMuch".Localized());
                    }
                    else
                        OutputText("Scenes.Kihascene.RapeKihaWithWORDS.SitNextToKiha".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) < 3)
                CoC.Instance.Flags.Set(GameFlagIds.KihaTalkStage, CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void AnalRapuzulaKiha()
        {
            ClearOutput();

            ShowImage("kiha-buttfuck");

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            var y = Player.CockThatFits2(CombatController.Instance!.Context.Enemy.AnalCapacity());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.WatchDragonessSlump".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.TellHerTurnAround".Localized());

            if (Player.CockArea(x) <= 20)
                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.QuicklyShedYour".Localized());
            else
                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.QuicklyShedYourAgain".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.WordsStingAndResolve".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.ArrogantDragonNowFacing".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.IgnoringDragonBitchMocking".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.BeginToFeelTemperature".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.SmileDevilishlyClenches".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.FingersTouchSkin".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.ThreateningToCrush".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.GoadYouWince".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.NeedWayOut".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.BreaksHerSilence".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.MoveHandsFromHer".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.SinceSheSeems".Localized());
                else
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.SinceSheSeemsAgain".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.ShrieksInShock".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.HeatThatNowEncircles".Localized(
                Appearance.CockNoun(Player.Cocks[x].Id),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.SquirmsFromPenetration".Localized());

            OutputText("Scenes.Kihascene.AnalRapuzulaKiha.MoansOfPainBecome".Localized());

            if (y >= 0)
            {
                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.PervertedIdeaForms".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.YourOtherDick".Localized());
                else
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.AnotherOfYourDicks".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.LineItUp".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.FewMoreThrusts".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.PullOutFinalTime".Localized(
                    Player.CockDescript(y),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.ShuddersInClimax".Localized(
                    Appearance.CockNoun(Player.Cocks[y].Id)
                ));

                if (Player.CockTotal() >= 3)
                {
                    if (Player.CockTotal() == 3)
                        OutputText("Scenes.Kihascene.AnalRapuzulaKiha.RemainingCockTarnishes".Localized());
                    else
                        OutputText("Scenes.Kihascene.AnalRapuzulaKiha.RemainingCocksTarnish".Localized());
                }
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Kihascene.AnalRapuzulaKiha.PulsesWithPleasureSoaking".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.PeriodBreak".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.DragonessSlumpsFalls".Localized());
            }
            else
            {
                if (Player.CockArea(x) <= 20)
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.HandsToHerBreasts".Localized());
                else
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.HandsToBreastsAgain".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.MoansBecomeLouder".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.GiveNipplesOneMore".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.GruntingThrustOneLast".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kihascene.AnalRapuzulaKiha.PulsesWithPleasureBut".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.ContentedSighPull".Localized());

                OutputText("Scenes.Kihascene.AnalRapuzulaKiha.DecidingYouAreDone".Localized());
            }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void InquisitorRobesDiscovery()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.CuttingThroughSwamps".Localized());

            if (Player.Intelligence < 60)
            {
                OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.UnfortunatelyTryAsYou".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.WhileSpinningLocks".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.ChastityYouSay".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.TumblersOfDoorSpin".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.DescendAlertDust".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.IHaveFailed".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.CouldHavePreventedAll".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.ShouldHaveProtectedThem".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.WhenAtLastReckoned".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.DoNotMistakeMe".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.SheMockedMePerhaps".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.FailedToStopDemons".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.ButTheDemons".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.IfYouFoundChamber".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.SpentTheLastOf".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.StandBeforeTheChest".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.DearlySincerelyHope".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.MaraeBless".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.InquisitorZathulSignOff".Localized());

            OutputText("Scenes.Kihascene.InquisitorRobesDiscovery.ReplaceTheScroll".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Retribution".Localized(), RetributionArmorIsCoolShit);

            AddButton(1, "Common.Button.Carnality".Localized(), CarnalityArmorIsCoolShitToo);

            AddButton(4, "Common.Button.Nope".Localized(), NoThankYouSirIDontWantAwesomeArmors);
        }

        public void NoThankYouSirIDontWantAwesomeArmors()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.NoThankYouSirIDontWantAwesomeArmors.UninterestedInReward".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RetributionArmorIsCoolShit()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.RetributionArmorIsCoolShit.ChestClicksAtWord".Localized());

            OutputText("Scenes.Kihascene.RetributionArmorIsCoolShit.TurningToLeaveStartled".Localized());

            OutputText("Scenes.Kihascene.RetributionArmorIsCoolShit.DisplayMakesYouRighteous".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GottenInquisitorArmor, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GottenInquisitorRobes, 1);

            InventoryScene.TakeItem(ArmorIds.I_ROBES, () => ReturnToCamp(1));
        }

        public void CarnalityArmorIsCoolShitToo()
        {
            ClearOutput();

            OutputText("Scenes.Kihascene.CarnalityArmorIsCoolShitToo.ChestClicksAtWord".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Kihascene.CarnalityArmorIsCoolShitToo.StrikesYouAsUnusual".Localized());

            OutputText("Scenes.Kihascene.CarnalityArmorIsCoolShitToo.RedLikeDriedBlood".Localized());

            OutputText("Scenes.Kihascene.CarnalityArmorIsCoolShitToo.TurningToLeaveStartled".Localized());

            OutputText("Scenes.Kihascene.CarnalityArmorIsCoolShitToo.MakesYouFeelBadass".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GottenInquisitorArmor, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GottenInquisitorCorset, 1);

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.InquisitorsCorset), 1);

ReturnToCamp(1);
        }

    }
}