using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register<FemaleSpiderMorphScene>("coc:female_spider_morph")]
    public class FemaleSpiderMorphScene : Scene
    {

        public int spidersKilled = 0;

        public static SaveContent Save => SaveContent(SceneIds.Swamp.FemaleSpiderMorph);

        public override void Entry()
        {
            FSpiderMorphGreeting();
        }

        public void FSpiderMorphGreeting()
        {
            ClearOutput();

            if (Player.Pregnancy.Event == 2)
            {
                FindASpiderMorphEggSack();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredFemaleSpidermorphs) == 0)
            {
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphGreeting.ExploreIntoTheSwamp".Localized());

                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphGreeting.StrangeNakedFemale".Localized());
            }
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphGreeting.ExploreInTheSwamp".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphGreeting.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightFSpiderMorph);

            AddButton(1, "Common.Button.TryToTalk".Localized(), TalkToFSpiderMorph);

            AddButton(14, "Common.Button.Leave".Localized(), RunFromFSpiderMorph);

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredFemaleSpidermorphs, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredFemaleSpidermorphs) + 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryArachnes);
        }

        public void FightFSpiderMorph()
        {

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.FemaleSpiderMorph), () => Go<CampScene>(), canFlee: false);
        }

        public void RunFromFSpiderMorph()
        {
            ClearOutput();

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Femalespidermorphscene.RunFromFSpiderMorph.TurnAroundAndFlee".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
                CombatScene.Start(EnemyRegistry.Create(EnemyIds.FemaleSpiderMorph), () => Go<CampScene>());
        }

        public void TalkToFSpiderMorph()
        {
            ClearOutput();

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.HoldUpYourHands".Localized());

                OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.TwiddlesHerFingers".Localized());

                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.AfterYouveBothHad".Localized());

                    if (Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.NevermindGenderless".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.FirstSane".Localized());

                    OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.LetHerFuck".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Yes".Localized(), VoluntaryFemaleSpiderMorphRapesYou);

                    AddButton(14, "Common.Button.Leave".Localized(), DeclinedCrazyFemaleSpiderMorphSexFunTimes);
                }
                else
                {
                    OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.AfterBothHadAgain".Localized());

                    Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SweetGossamer), 1);

ReturnToCamp(1);
                }
            }
            else
            {
                OutputText("Scenes.Femalespidermorphscene.TalkToFSpiderMorph.HoldYourHandsUp".Localized());

                var femaleSpiderMorph = EnemyRegistry.Create(EnemyIds.FemaleSpiderMorph);

                CombatScene.Start(femaleSpiderMorph, () => Go<CampScene>());
            }
        }

        public void VoluntaryFemaleSpiderMorphRapesYou()
        {

            CurrentMonster = EnemyRegistry.Create(EnemyIds.FemaleSpiderMorph);

            LoseToFemaleSpiderMorph();
        }

        public void DeclinedCrazyFemaleSpiderMorphSexFunTimes()
        {
            ClearOutput();

            OutputText("Scenes.Femalespidermorphscene.DeclinedCrazyFemaleSpiderMorphSexFunTimes.TellLustySpider".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DefeatFemale()
        {
            ClearOutput();

            if (!CombatController.InCombat)
            {
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.AgreeToHaveSex".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.NervouslyWord".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.PlayfullyWord".Localized());
                    else
                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.SeductivelyWord".Localized());

                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.UndressTossingAside".Localized());

                if (Player.HasCock())
                {
                    var multi = Player.CockTotal() > 1;

                    var s = multi ? "s" : "";

                    var your = multi ? " Each of your" : " Your";
                }
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.MeanwhileYour".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.Wetness() < 4)
                            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.MoistensWord".Localized());
                        else
                            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.DripsWord".Localized());

                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.LabiaEngorge".Localized());
                    }

                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ParagraphBreak".Localized());

                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ArachnidWomanTurnsAround".Localized());

                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.CoosWithAHungry".Localized());

                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ArachnidGirlSmirks".Localized());
            }
            else
                if (Player.Health < 1)
                {
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.DropToGroundUtterly".Localized());

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.WatchANumberOf".Localized());

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.VictoriousSpiderMuses".Localized());

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.KnowingShePlans".Localized());
                }
                else
                {
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.DropToGroundAnd".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.HasCock())
                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.FlopAboutLewdly".Localized(
                             Player.SMultiCockDesc()
                        ));

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.SpiderGirlLooksDown".Localized());

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ProtrusionsBulgeUp".Localized());

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ApproachesAndDelicately".Localized());

                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.WonderWhatSheHas".Localized());
                }

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.LicksHerLips".Localized());

            if (Player.BiggestLactation() >= 1)
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.EachDraughtOfMilk".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.EachFruitlessSuckle".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.PeriodBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.WriggleUnderneath".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.MoistWord".Localized());
            else
                if (Player.Wetness() < 5)
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.DroolingWord".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.GushingWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.NetherLipsStew".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.WithASmugGrin".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.PullsBackAnd".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.OtherSideOfChest".Localized());
            else
                if (Player.BreastRows.Count() == 2)
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.YourOtherBreast".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.AnotherOfManyBreasts".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.BodyIsThrumming".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.GushWord".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.TinySquirtWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.GirlCumSplattersOut".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.MonsterWomanLaysHand".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ChitinousKneesSink".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.RubsFingerBetween".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.EmphasizeHerPoint".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.ArachnidSexTastesSweet".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.SnakeLikeLength".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Femalespidermorphscene.DefeatFemale.MassiveDemonicLength".Localized());
                else
                    if (Player.HasLongTongue())
                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.InhumanLengthInto".Localized());
                    else
                        OutputText("Scenes.Femalespidermorphscene.DefeatFemale.AllOfYourLength".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.HardNubOfClit".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.MistressRocksHerHips".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.FangsStabDeep".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.FingertipIn".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.CryAndBegRelease".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.EachLoveBiteHurts".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.FangsSheathThemselves".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.EruptsLikeFountain".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.DefeatFemale.PuddlesJuiceOnGround".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.EachClimaxTriggers".Localized());

            OutputText("Scenes.Femalespidermorphscene.DefeatFemale.DreamlessSleepBody".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f));

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void SpiderMorphFemaleRidesACawk()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            if (!CombatController.InCombat)
            {
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ShuckYourAndToss".Localized());

                if (Player.LustPercent < 70)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.TwitchBeginToStiffen".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.TwitchAlreadyHard".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.StridingForwardClose".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ShrugAndStepBack".Localized());
            }
            else
                if (Player.Health < 1)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.CollapseIntoSoftMoss".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.CollapseOnMossAgain".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.SinkingSlightlyInto".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.GossamerStrandHits".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.OnceItFinishes".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.DecreesThe".Localized());

            if (CombatController.InCombat)
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.VictoriousArachnidWord".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.DangerousGleamIn".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ParagraphBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.InhumanBrunetteTurns".Localized());

            if (Player.LustPercent >= 80)
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ThrustUpAsIf".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.GrowHarderEntranced".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.GigglesBeforeSquirting".Localized(
                 Player.CockDescript(x)
            ));

            if (!Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.BaseWord".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.SheathWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.TightRingThatHolds".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.SheMakesWord".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ASimilarRing".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.SimilarRings".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ForYourOtherPenis".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PluralEs".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.AsWellMakingSure".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.WinceFromOnset".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.WouldntWantMeTo".Localized());

            if (Player.SpiderScore() < 4)
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.BeastsSeedWord".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ItinerateSpiderSeed".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.WouldWeAsksCaptor".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PushesPastOtherDick".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PluralS".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.AndWord".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.CaressesBoundCock".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.TeasesSpiderWoman".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.GroanAtTreatment".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.BeginsToLower".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.DraggingOverYourOwn".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.HoveringAboveChest".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.SpiderMaidenTire".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ProclaimsArachnidLover".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.MeanwhileExtraPenis".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PluralEsAgain".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.DroolWord".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PluralSAgain".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PreCumBut".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.IsWord".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.AreWord".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.CompletelyIgnored".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.DickSuddenlyGrabbed".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.TorturedGroanOnly".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.CumAgainJustLike".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.EntireBodyLocksUp".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.RollsOffOfYou".Localized());

            if (Player.CumQ() < 25)
                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.BaseballWord".Localized());
            else
                if (Player.CumQ() < 100)
                    OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.GrapefruitWord".Localized());
                else
                    if (Player.CumQ() < 400)
                        OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.BasketballWord".Localized());
                    else
                        if (Player.CumQ() < 700)
                            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.WatermelonDraggingDown".Localized());
                        else
                            if (Player.CumQ() < 1500)
                                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.ExerciseBallSmothering".Localized());
                            else
                                OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PersonCompletelyCovering".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.PeriodBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderMorphFemaleRidesACawk.FinallyAtPeace".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f));

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void FemaleSpiderMorphTooBigWebRape()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            if (!CombatController.InCombat)
            {
                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.HastilyRemoveAndToss".Localized());

                if (Player.LustPercent < 70)
                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.ExpandingWord".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.HardWord".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.ExpressionOfDisbelief".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.ArentYouWhyDont".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.LayDownQuiteConfident".Localized(
                     Player.SMultiCockDesc(),

                     Player.CockHead()
                ));

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.OverSphericalOrgan".Localized(
                     Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.IgnoringSmallerMembers".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SheAsks".Localized());
            }
            else
            {
                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.DropToGround".Localized());

                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.HandsIntoYourGroin".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.GroanInObviousPain".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.AdvancesGiggling".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SheMuses".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.BeforeYouCanReact".Localized());

                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SpiderGirlAsks".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.StartToProtest".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SilkStrandsStart".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.StopsAndAnnounces".Localized());

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.MistressDoesntDelay".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.PullingOutSlaps".Localized());

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.AlreadyShiveringPanting".Localized());

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.CommandsWhileShe".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.FasterYouGo".Localized());

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SplashOfCandySyrup".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.BodyCompletelyEmpty".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.BallsCompletelyEmptied".Localized());

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.PeriodBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SuddenlyFocusOfAttention".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.AppleSizedLump".Localized());
            else
                if (Player.CumQ() < 300)
                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.GrapefruitSizedBubble".Localized());
                else
                    if (Player.CumQ() < 700)
                        OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.BasketballSizedBalloon".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.WatermelonSizedBalloon".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.ExerciseBallSizedMass".Localized());
                            else
                            {
                                OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.PersonSizedMass".Localized());

                                if (Player.CumQ() >= 4000)
                                    OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.ItPopsTooWeak".Localized());
                            }

            OutputText("Scenes.Femalespidermorphscene.FemaleSpiderMorphTooBigWebRape.SighAndFallInto".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f));

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void LoseToFemaleSpiderMorph()
        {

            if (Player.HasCock())
            {
                if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.Body.Get<VaginaPart>().Capacity()) != -1)
                    FemaleSpiderMorphTooBigWebRape();
                else
                    SpiderMorphFemaleRidesACawk();
            }
            else
                if (Player.HasVagina())
                    DefeatFemale();
                else
                {
                    OutputText("Scenes.Femalespidermorphscene.LoseToFemaleSpiderMorph.SpiderGirlKnocksOut".Localized());

                    CombatScene.CleanupAfterCombat();
                }
        }

        public void DefeatASpiderBitch()
        {
            ClearOutput();

            OutputText("Scenes.Femalespidermorphscene.DefeatASpiderBitch.SpiderGirlDrops".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Femalespidermorphscene.DefeatASpiderBitch.ThrustingCarapaceCovered".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.DefeatASpiderBitch.TooWoundedToFight".Localized());

            ClearButtons();

            if (Player.Lust >= 33)
                OutputText("Scenes.Femalespidermorphscene.DefeatASpiderBitch.WhatDoToHer".Localized());

            AddButton(0, "Common.Button.FuckAss".Localized(), EvilSpiderGirlVictoryAnal);

            AddButton(1, "Common.Button.FuckPussy".Localized(), FSpiderMorphRapeDude);

            AddButton(2, "Common.Button.Scissor".Localized(), FSpiderMorphRape);

            if (OptionStore.Instance.Get("gore", 0) > 0)
                AddNextButton("Common.Button.Amputate".Localized(), SpiderAmputation);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void FSpiderMorphRape()
        {
            ClearOutput();

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.LaughAndPush".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.EvenAsFingersContinue".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SlapHerHandsAway".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.PussyDarkLikeHer".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.UntilShesPanting".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.PeriodBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.HoneyedOnyx".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.LiftHerBreast".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SeveralRepetitionsSheGives".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.LieDownBeside".Localized(
                 Player.Legs()
            ));

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.StimulationOfTwitching".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.ThreadYourselfBack".Localized());

            if (Player.GetClitLength() < 4)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.BumpingGlidingAgainst".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SpearingForwardToPenetrate".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.PaleBeautyMoans".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SquishesAndSchlicks".Localized());

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SeizedByVelvetEmbrace".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.DragsOverVelvetLips".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.ParagraphBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.RollAndBuckHips".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SpewsWastedSeed".Localized());

                if (Player.CumQ() >= 400)
                {
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.EvenLeavingA".Localized());

                    if (Player.CumQ() >= 400 && Player.CumQ() < 1000)
                        OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SmallWord".Localized());
                    else
                        if (Player.CumQ() >= 2000)
                            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.HugeWord".Localized());

                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.PuddleOnTheGround".Localized());
                }

                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.PeriodSpace".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.SharesAMoment".Localized());

            if (Player.CumQ() < 400)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.HerOwnWord".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.TheMixedWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.PuddleOfGirlCum".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.DoesntTakeMoreThan".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.MomentOfPity".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.FeelALittleGuilty".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.LeaveHerThereWith".Localized());

            Player.Orgasm("Vaginal");

            if (Player.CanDepositEel())
            {
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRape.EelParasiteSlipped".Localized());

                Player.DepositEel();
            }

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void FSpiderMorphRapeDude()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = 0;

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.PushSpiderGirlOnto".Localized());

            if (CurrentMonster.LustPercent <= 99)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.RelaxesBitTheSmell".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.SalivatesBothHoles".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ItDoesntMatterYoure".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.SpreadsHerLegs".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.YouAdvanceWord".Localized());

            if (Player.LustPercent < 70)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ThickeningWord".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.PulsatingWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ReadyToPenetrate".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.DefeatedArachnidTrembles".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ThatWasUnexpected".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.WhileYourOther".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.CockFlopsUselessly".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.CocksFlopUselessly".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ExtraStimulationAllIt".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.TeasingEagerTwat".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.PunishingWorthlessTwat".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.CummingEarlySpastic".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.YouBottomOut".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.SlapYourBallsInto".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.YourTwitchingWord".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.WhileTheWord".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.RestSpasmPointlessly".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.OtherSpasmsPointlessly".Localized());
            }
            else
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.HappilyAsItPrepares".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.JismBoilsOut".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.EachSpurtFeelsBigger".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.TellFromTheSmall".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ItsLeakingOutAround".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ParagraphBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ExpectingMother".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.DistendedWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.BellyLookOfDisbelief".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.PullingOutWord".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.WipeAwayHerTears".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.ShrugAndSighWiping".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.SlapHerAcrossFace".Localized());

            OutputText("Scenes.Femalespidermorphscene.FSpiderMorphRapeDude.DressedHeadBack".Localized());

            Player.Orgasm("Dick");

            Player.Pregnancy.Type = Contents.Pregnancy.PregnancyIds.PlayerFather;

            Player.Pregnancy.Incubation = Contents.Pregnancy.PregnancyIds.SpiderIncubation - 200;

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void EvilSpiderGirlVictoryAnal()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x == -1)
                x = 0;

            var y = Player.CockThatFits2(CombatController.Instance!.Context.Enemy.AnalCapacity());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.PeelOffYour".Localized());

            if (CurrentMonster.LustPercent >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.MoaningPlaintively".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.GroaningPainfully".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.SighingGrabNarrowFrame".Localized());

            if (CurrentMonster.LustPercent >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.MewlsWithDisappointment".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.MuchBetterPosition".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.LooksBackFearfully".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.CircleAFingerAlong".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.OfCourseYouWill".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.WhileYourSlipsInto".Localized(
                     Player.CockDescript(y)
                ));
            else
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.TightRingOfSphincter".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.CarapaceFingertipsDig".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.AtLastGivesUp".Localized());

            if (y >= 0)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.StillTighterThan".Localized(
                     Player.CockDescript(y)
                ));

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.PeriodBreak".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.VictimBitesHerLip".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.SkankCumsHard".Localized());

            if (y == -1)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.SplatteringFragrantSpunk".Localized());
            else
            {
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.SplatteringYour".Localized(
                     Player.CockDescript(y)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.AndBalls".Localized());

                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.WithCopiousLadySpunk".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.WaysFromClimax".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.WhiteGooSpurting".Localized());

            if (y != -1)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.AndCumDrizzlingSlit".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.WatchHerCheeksJiggle".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.FuckingHerLikeThis".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.BallsSquirmingUnloading".Localized());
            else
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.ProstateUnloadingWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.PumpInjectionsOfCream".Localized());

            if (y != -1)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.AndPussy".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.FillingWithSeed".Localized());

            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.IncrediblySheCan".Localized());

                if (Player.CumQ() < 750)
                    OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.SlightlyWord".Localized());
                else
                    if (Player.CumQ() >= 1200)
                        OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.MassivelyTurningObscene".Localized());
            }

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.PeriodBreakAgain".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.PopOutStepBack".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.TrickleWord".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.GushWord".Localized());
                else
                    OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.RiverWord".Localized());

            OutputText("Scenes.Femalespidermorphscene.EvilSpiderGirlVictoryAnal.SeedRushesOut".Localized());

            if (y != 1)
            {
                Player.Pregnancy.Type = Contents.Pregnancy.PregnancyIds.PlayerFather;

            Player.Pregnancy.Incubation = Contents.Pregnancy.PregnancyIds.SpiderIncubation - 200;
            }

            Player.Orgasm("Dick");

            if (!CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void FindASpiderMorphEggSack()
        {
            ClearOutput();

            OutputText("Scenes.Femalespidermorphscene.FindASpiderMorphEggSack.StumbleUponWebbed".Localized());

            OutputText("Scenes.Femalespidermorphscene.FindASpiderMorphEggSack.LeftScratchingYourHead".Localized());

            Player.Pregnancy.Type = default;

            Player.Pregnancy.Incubation = 0;

            DoNext(() => ReturnToCamp(1));
        }

        public void SpiderAmputation()
        {
            ClearOutput();

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.CommandSpiderKin".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.HandsTryingToGrasp".Localized());

            if (Player.Weapon.IsAxe() || Player.HasKeyItem("Carpenter's Toolbox"))
            {
                OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.WhatCanOnlyAssume".Localized((Player.Weapon.IsAxe() ? "weapon" : "wood-cutting axe")));

                OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.HeavesAndWretches".Localized(
                     CombatController.Instance!.Context.Enemy.Mf("boy", "girl")
                ));
            }
            else
            {
                OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.MullingHowProceed".Localized());

                OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.WincesAndBreathes".Localized(
                     CombatController.Instance!.Context.Enemy.Mf("boy", "girl")
                ));
            }

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.LikelyDeterredThis".Localized((!(Player.Weapon.IsAxe() || Player.HasKeyItem("Carpenter's Toolbox")) ? "once more " : "")));

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.BloodCurdlingShrieks".Localized((!(Player.Weapon.IsAxe() || Player.HasKeyItem("Carpenter's Toolbox")) ? "If that weren't so much effort, you'd take the other leg too. " : "")));
            // 原版 FemaleSpiderMorphScene.hx:871：Shouldra 是跟随者且开启 silly 时会插一句吐槽
            if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra() && silly)
                OutputText("Scenes.Femalespidermorphscene.SpiderAmputation.DisembodiedVoice".Localized());

            Save.SetInt("spidersKilled", Save.GetInt("spidersKilled", 0) + 1);

            ClearButtons();

            AddNextButton("Common.Button.Beat".Localized(), SpiderAmputationBeat);

            AddNextButton("Common.Button.Leave".Localized(), SpiderAmputationLeave);
        }

        public void SpiderAmputationBeat()
        {
            ClearOutput();

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputationBeat.NeedNotLetProcess".Localized());

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputationBeat.AfterReasonableStretches".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void SpiderAmputationLeave()
        {

            OutputText("Scenes.Femalespidermorphscene.SpiderAmputationLeave.PerhapsThisLevel".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }
}