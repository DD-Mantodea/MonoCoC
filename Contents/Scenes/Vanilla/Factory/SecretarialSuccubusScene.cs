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
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Factory
{
    [Register<SecretarialSuccubusScene>("coc:secretarial_succubus")]
    public class SecretarialSuccubusScene : Scene
    {

        public override void Entry()
        {

        }

        public void EncounterSuccubus()
        {

            OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.StandingByCoffeemaker".Localized());

            if (Player.DemonScore() >= 4 && Player.IsCorruptEnough(75))
            {
                OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.BustyTurnsJiggling".Localized(
                    Player.Mf("stud", "sexy")
                ));

                OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.OmigawshChampion".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.PullsOutFile".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), DoFightSuccubus);

                AddButton(1, "Common.Button.GoDemon".Localized(), GoDemon);

                AddButton(2, "Common.Button.HookUp".Localized(), TalkSuccubusYes);
            }
            else
                if (Player.HumanScore() <= 3)
                {
                    OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.BustyTurnsJigglingAgain".Localized(
                        Player.Mf("stud", "sexy")
                    ));

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.BlowjobOfALifetime".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.LickYourHoneypot".Localized());
                        else
                            OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.NewAdditionShowHow".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.PeriodSayEnd".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.TurnsAwayShowOf".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), DoFightSuccubus);

                    AddButton(1, "Common.Button.ItsMe".Localized(), TalkSuccubusItsMe);

                    AddButton(2, "Common.Button.Leave".Localized(), BackToLobby);
                }
                else
                {
                    OutputText("Scenes.Secretarialsuccubusscene.EncounterSuccubus.BustyTurnsJigglingThird".Localized(
                        Player.Mf("stud", "sexy")
                    ));

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), DoFightSuccubus);

                    AddButton(1, "Common.Button.Talk".Localized(), TalkSuccubus);

                    AddButton(2, "Common.Button.Run".Localized(), BackToLobby);
                }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySuccubus);
        }

        public void BackToLobby()
        {

        }

        public void TalkSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubus.ShouldThankYou".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubus.RunsStockingFoot".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ForWhat".Localized(), TalkSuccubusForWhat);

            AddButton(1, "Common.Button.Yes".Localized(), TalkSuccubusYes);

            AddButton(2, "Common.Button.No".Localized(), TalkSuccubusNo);
        }

        public void TalkSuccubusForWhat()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusForWhat.BemusedForWhat".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sick".Localized(), TalkSuccubusNo);

            AddButton(1, "Common.Button.SoundsFun".Localized(), TalkSuccubusYes);
        }

        public void TalkSuccubusItsMe()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusItsMe.LaughMockingly".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusItsMe.WhirlsFaceMarred".Localized());

            DoNext(() => DoFightSuccubusMocked());
        }

        public void TalkSuccubusYes()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusYes.StepsForwardWraps".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusYes.CatchesOffGuard".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusYes.PulledFreeFrom".Localized());

            DoNext(() => DoBadEndSuccubusPart1());
        }

        public void TalkSuccubusNo()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.TalkSuccubusNo.SheFrowns".Localized());

            DoFightSuccubus();
        }

        public void GoDemon()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.GoDemon.AdvancingSlowly".Localized());

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Secretarialsuccubusscene.GoDemon.BodyBlushesRed".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PalmCaressesCrotch".Localized());

                if (Player.BiggestBreastSize() <= 1)
                {
                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PushedApartFlesh".Localized());

                    Player.CreateCock();

                    Player.Cocks[0].Length = 10;

                    Player.Cocks[0].Thickness = 2;

                    Player.Cocks[0].Id = BodyPartTypes.Demon;

                    Player.Balls = 2;

                    Player.BallSize = 3;
                }
                else
                {
                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GushingWithFluids".Localized());

                    Player.CreateVagina();

                    Player.Vaginas[0].Wetness = 3;
                }

                OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DemonCoos".Localized());
            }

            if (Player.Gender == Gender.Male)
            {
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PulsateStraining".Localized(
                        Player.MultiCockDescript()
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ReachesCuppingUnderside".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GlanceDownSeeing".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.YourBallsAnd".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.BasesOfYourCocks".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SaysAsSheClimbs".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.LiftsHerselfUp".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.HeatWetnessSweeps".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.AsksAsSheRocks".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.MoanPitifully".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.CantRemoveIt".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ResumesFucking".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.LookOfShockPleasure".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SluttyStandsUp".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SheSighsLethicite".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.StandUpDissatisfied".Localized());

                    if (!Player.HasKeyItem("Marae's Lethicite"))
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GatherSomeYourself".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DevourMaraes".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GreedilyGatherLethicite".Localized());

                    if (Player.CountCocksOfType("DEMON") > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GrowingWord".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.NewWord".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DemonCockFewMore".Localized());
                }
                else
                {
                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PulsatesStraining".Localized(
                        Player.MultiCockDescript()
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ReachesCuppingUndersideAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GlanceDownSeeingAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.YourBallsAndAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.BaseOfYourCock".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SaysClimbsAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.LiftsHerselfUpAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.HeatWetnessSweepsAgain".Localized(
                        Player.CockDescript(0)
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.AsksRocksAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.MoanPitifullyAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.CantRemoveItAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ResumesFuckingAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ShockPleasureAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SluttyStandsUpAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SheSighsLethiciteAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.StandUpDissatisfiedAgain".Localized());

                    if (!Player.HasKeyItem("Marae's Lethicite"))
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GatherSomeYourselfAgain".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DevourMaraesAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ButFirstCertainDemoness".Localized());

                    if (Player.CountCocksOfType("DEMON") > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GrowingWordAgain".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.NewWordAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DemonCockMoreAgain".Localized());
                }
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GrowsWetReady".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.WordsMakeBlush".Localized(
                        Player.Legs()
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SwingsLissomeLegs".Localized());

                    if (Player.GetClitLength() >= 2)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PushesFreeNuzzling".Localized(
                            Player.ClitDescript()
                        ));
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.LowersDownRubbing".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.InNoTimeFlat".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.AsksRocksThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.MoanPitifullyThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.CantRemoveItThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.BeautifulPicksUpPace".Localized(
                        Player.VaginaDescript(0),

                        Appearance.AllBreastsDescript(Player)
                    ));

                    if (Player.HasFuckableNipples() && Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GaspTongueSlides".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GaspItCurls".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.FucksLikeThatHours".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PinkFluidSplatters".Localized());

                    if (Player.Vaginas[0].Wetness < 5)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ForFirstTime".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ThrowsBackHead".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.InterruptsDelight".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.BeforeReactGrasps".Localized());
                }
                else
                {
                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.AndGrowWetReady".Localized(
                        Player.MultiCockDescript(),

                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.WordsMakeBlushAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SwingsLissomeLegsAgain".Localized());

                    if (Player.GetClitLength() >= 2)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.PushesFreeNuzzlingAgain".Localized(
                            Player.ClitDescript()
                        ));
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.LowersDownRubbingAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.HeatWetnessWraps".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.AsksRocksFourth".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.MoanPitifullyFourth".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.CantRemoveItFourth".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ResumesFuckingThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.ShockPleasureThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SluttyStandsUpThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.SheSighsLethiciteThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.StandUpDissatisfiedThird".Localized());

                    if (!Player.HasKeyItem("Marae's Lethicite"))
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GatherSomeYourselfThird".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DevourMaraesThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.CertainDemonessAgain".Localized());

                    if (Player.CountCocksOfType("DEMON") > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.GrowingWordThird".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.GoDemon.NewWordThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.DemonCockMoreThird".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.GoDemon.BeforeGetIntoThat".Localized());
                }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Strength, 2f), new DynStat(GameStats.Toughness, 2f), new DynStat(GameStats.Speed, 2f), new DynStat(GameStats.Intelligence, 2f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

            DoNext(() => DoBadEndDemon());
        }

        public void DoBadEndDemon()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndDemon.RapidlyMovedRanks".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndDemon.FullFledgedDemon".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndDemon.RapidlyMovedRanksAgain".Localized());

            GameOver();
        }

        public void DefeatSuccubus()
        {
            ClearOutput();

            OutputText("Common.Text.YouSmileInSatisfactionAsThe2".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), DoRapeSuccubus);

            AddButton(1, "Common.Button.No".Localized(), DoLeaveSuccubus);

            if (Player.HasKeyItem("Deluxe Dildo"))
                AddButton(2, "Common.Button.DildoRape".Localized(), DildoSuccubus);
        }

        public void DoFightSuccubus()
        {

            OutputText("Scenes.Secretarialsuccubusscene.DoFightSuccubus.YellGoingDown".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoFightSuccubus.UnseenForceClosesDoor".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoFightSuccubus.ItsAFight".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FactorySuccubusDefeated, 1);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "secretarial_succubus")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DoFightSuccubusMocked()
        {

            CoC.Instance.Flags.Set(GameFlagIds.FactorySuccubusDefeated, 1);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "secretarial_succubus")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CombatController.Instance!.Context.Enemy.ArmorDefense -= 10;
        }

        public void DoRapeSuccubus()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (Player.Cocks.Count > 0 && (Player.Gender != Gender.Hermaphrodite || Random.Shared.Next(2) != 0))
            {
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PantingStaggers".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.CollapsesOnFloor".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PonderingBestWay".Localized());

                if (Player.Body.Get<HairPart>().Length > CurrentMonster.Body.Get<HairPart>().Length)
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.LengthensWord".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.ShortensWord".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.ShiftingBoneStructures".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.DidDecideToRape".Localized(
                    Player.CockDescript(0),

                    Player.CockDescript(0)
                ));

                if (Player.CockArea(0) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                {
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PleasureShortLived".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.CoosHigherPitched".Localized());

                    Player.Cocks[0].Id = BodyPartTypes.Demon;

                    if (Player.CumQ() < 50)
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.DropletsOfPre".Localized());

                    if (Player.CumQ() >= 50 && Player.CumQ() < 500)
                    {
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.SteadyStreamOfPre".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PracticallyGlowRelief".Localized());

                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.ThrowBackHead".Localized());
                    }

                    if (Player.CumQ() >= 500)
                    {
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.RiverOfPreCum".Localized(
                            Player.CockDescript(0),

                            Player.CockDescript(0)
                        ));

                        if (Player.Balls > 0)
                            OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.TwitchAndShake".Localized());

                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PervertedVersionWave".Localized(
                            Player.CockDescript(0)
                        ));
                    }

                    View.Time.Hours += 2;

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PossessedMalenessFar".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.HoursLaterPullAway".Localized());
                }
                else
                {
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PlungeIntoVelvety".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.LookingDownVeins".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PullBackLettingRing".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.FemaleDoubleReaches".Localized());

                    Player.Cocks[0].Id = BodyPartTypes.Demon;

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.BucksHipsWeakly".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.EachOfNewBumps".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.BallsEmptyLoad".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PeriodSpace".Localized());

                    if (Player.CumQ() >= 50 && Player.CumQ() < 400)
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.InhaleBlackCum".Localized());

                    if (Player.CumQ() >= 400)
                        OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.GaspingPleasureSurprise".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.PantWithExertion".Localized());
                }

                OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.LicksFingersClean".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.FoeStaggersFalls".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.FoeDropsToKnees".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.BuddingClitRises".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Scissor".Localized(), DoScissorSuccubus);

                if (Player.BiggestLactation() > 1)
                {
                    OutputText("Scenes.Secretarialsuccubusscene.DoRapeSuccubus.CouldScissorWithHer".Localized());

                    AddButton(1, "Common.Button.ForceFeed".Localized(), DoForcefeedSuccubus);
                }
                else
                    AddButton(1, "Common.Button.ForceFeed".Localized(), null);
            }
        }

        public void DoScissorSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.DoScissorSuccubus.ShiverWithAnticipation".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.GetClitLength() >= 3)
                OutputText("Scenes.Secretarialsuccubusscene.DoScissorSuccubus.GroanWantonDesire".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoScissorSuccubus.OnlyFewSeconds".Localized(
                Player.VaginaDescript(0),

                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Secretarialsuccubusscene.DoScissorSuccubus.TurnAwayBemused".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void DoForcefeedSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.ChuckleDecide".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            if (Player.BiggestBreastSize() > 6)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.NearlySmothering".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.JiggleBackForth".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.DrinkUpBitch".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TentativelyTakesNipple".Localized());

            if (Player.BiggestLactation() < 2)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TongueCurlsAround".Localized());

            if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() < 3)
            {
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TongueWrapsYourNipple".Localized());

                if (Player.BreastRows.Count() > 2)
                    OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TitsWord".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TitWord".Localized());
            }

            if (Player.BiggestLactation() >= 3 && Player.BiggestLactation() < 4)
            {
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TongueWrapsMilkSwollen".Localized());

                if (Player.BreastRows.Count() > 2)
                    OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.PluralS".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.CantHelpDribble".Localized());
            }

            if (Player.BiggestLactation() >= 4)
            {
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TongueWrapsMilkBloated".Localized());

                if (Player.BreastRows.Count() > 2)
                    OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.PluralSAgain".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.InSympathyDrenching".Localized());
            }

            if (Player.TotalNipples() == 2)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.FirstNippleDrains".Localized());

            if (Player.TotalNipples() > 2 && Player.TotalNipples() < 6)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.FirstDrainsForcibly".Localized(
                    Player.NippleDescript(0),

                    NumberUtils.Num2Text(Player.TotalNipples())
                ));

            if (Player.TotalNipples() >= 6)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.FirstDrainsForce".Localized(
                    Player.NippleDescript(0)
                ));

            if (Player.AverageLactation() * Player.BreastRows.Count() < 6)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.BellyBulgesSlightly".Localized());
            else
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.LooksBloatedPregnant".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.DespiteRelief".Localized(
                Appearance.AllBreastsDescript(Player),

                Player.VaginaDescript(0)
            ));

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.AndFeelHungrier".Localized());
            else
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.FeelsHungrier".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.ShoveCrotchInto".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.Cocks.Count == 0)
            {
                var temp = Random.Shared.Next(3);

                if (Player.Vaginas[0].Wetness >= 5)
                    OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.SoakingGirlCum".Localized());
                else
                    if (Player.Vaginas[0].Wetness <= 2)
                        OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.SlickingFaceGirlish".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.DrenchingDeluge".Localized());
            }
            else
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.UnloadingWaveHot".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.PushHerOver".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.ThickLayerSpunk".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.SentencePeriod".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoForcefeedSuccubus.TurnAwayBemused".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -50f));

            CombatScene.CleanupAfterCombat();
        }

        public void DildoSuccubus()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.DefeatedEyesWarily".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.SpreadsFishnetLegs".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.SeesButtonPoking".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.ShakingHeadBlushing".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.InShameWord".Localized());
            else
                OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.WithLustWord".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.BeginTracingFinger".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.SoddenPlop".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.PickUpTheToy".Localized());

            if (Player.Corruption > 75)
                OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.LongLickTo".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DildoSuccubus.PutItAwayWhen".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((20 + (Player.Libido / 5) + (Player.Corruption / 10)))));

            CombatScene.CleanupAfterCombat();
        }

        public void DoLossSuccubus()
        {
            ClearOutput();

            if (Player.Cocks.Count > 0)
            {
                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.DrivenHalfMad".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.ExhaustedCollapse".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.CroonsPoising".Localized());

                if (Player.Balls > 0)
                {
                    if (Player.BallSize < 6)
                        OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.SultryCaptorLeansLow".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.SultryCaptorMarveling".Localized(
                            Player.BallsDescriptLight()
                        ));
                }
                else
                    if (Player.Vaginas.Count == 0)
                        OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.SultryCaptorAgain".Localized());
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.SultryCaptorThird".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.ParagraphBreak".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.LicksLipsAnticipation".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.WastesNoTime".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.BallsWord".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.AssWord".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.AsSheReleases".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.FreedClenches".Localized());

                if (Player.CumQ() >= 50 && Player.CumQ() < 400)
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.FreedClenchesAgain".Localized());

                if (Player.CumQ() > 400)
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.FreedClenchesThird".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.CroonsMopping".Localized());

                Player.Orgasm("Dick");

            }
            else
            {
                if (Player.Lust >= Player.MaxLust)
                {
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.DrivenHalfMadAgain".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.LastPiecesFallAway".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.BareGroinLastPieces".Localized());
                }
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.WobblingUnsteadily".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.StepsAwayVial".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.SmilesSlipsFinger".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.GaspsOutAs".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.ClitPartsFolds".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.FleshyGrowthErupts".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.InSecondsPlaying".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoLossSuccubus.StandOnShakyLegs".Localized());

                Player.CreateCock();

                Player.Cocks[0].Length = 16;

                Player.Cocks[0].Thickness = 1.5f;

                Player.Orgasm("Vaginal");

            }
        }

        public void DoLeaveSuccubus()
        {

            CombatScene.CleanupAfterCombat();
        }

        public void DoBadEndSuccubusPart1()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.BloodBeginsBurn".Localized());

            if (Player.BiggestLactation() < 1)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.BeginProdding".Localized());

            if (Player.BiggestLactation() >= 1 && Player.BiggestLactation() < 3)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.GetPainfullyHard".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            if (Player.BiggestLactation() >= 3)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.GetHardAsMilk".Localized());

            if (Player.Cocks.Count == 1)
            {
                if (Player.CockArea(0) < 30)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.SwooningBloodLoss".Localized());

                if (Player.CockArea(0) >= 30 && Player.CockArea(0) < 100)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.NearlyBlackingOut".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                if (Player.CockArea(0) >= 100)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.StruggleNotLose".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));
            }

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.LightHeadedness".Localized(
                    Player.MultiCockDescript()
                ));

            if (Player.Vaginas.Count > 0)
            {
                if (Player.GetClitLength() >= 4.5f)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.PoppingFromThighs".Localized());

                if (Player.GetClitLength() > 1 && Player.GetClitLength() < 4.5f)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.WaveOfPleasureErupts".Localized());

                if (Player.Vaginas[0].Wetness >= 3)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.SquishingWetly".Localized(
                        Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.GroaningSoftly".Localized(
                        Player.VaginaDescript(0)
                    ));
            }

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.ParagraphBreak".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.NoLongerCaring".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.PushesButtonWall".Localized());

            if (Player.BreastRows.Count == 1)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.PairOfWord".Localized());
            else
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.NumberOfWord".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.HosesToBreasts".Localized());

            if (Player.Cocks.Count == 0)
            {
                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.VacuumPumpToClit".Localized());
                else
                {
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.AnotherVacuumPump".Localized());

                    Player.CreateCock();
                }
            }
            else
                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.PumpToYourClit".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.PumpManyTimesBigger".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.AtFirstOnlySuction".Localized());

            if (Player.Vaginas.Count == 0)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.SentencePeriod".Localized());
            else
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.EverLargerDildo".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart1.SwoonSolidClick".Localized());

            DoNext(() => DoBadEndSuccubusPart2());
        }

        public void DoBadEndSuccubusPart2()
        {
            ClearOutput();

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.BoundYouGiggles".Localized());

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.SuctionPullsSquirt".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.VacuumPumpCock".Localized());

                if (Player.CumQ() > 300)
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.StrugglingWithAmount".Localized());
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.SuctionDrugsMake".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.NeedleLodged".Localized());
            }

            if (Player.Vaginas.Count > 0)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.ClenchingTightly".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.WorldDisappears".Localized());

            if (SceneInstance<Npcs.MarbleScene>.Get().KnowAddiction == 1)
            {
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.LaterPulledOut".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.SeemsStateOfBliss".Localized());

                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.LookAroundNotAlone".Localized());
            }
            else

                if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                {
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.BriefMomentClarity".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.StateOfBlissAgain".Localized());

                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.NotAloneAgain".Localized());
                }
                else
                    OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.LaterMomentOf".Localized()); // 原版 SecretarialSuccubusScene.hx:714 — AikoScene.hx:1889 / FactoryScene.hx 等：Marble 是跟随者

            OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.InRowsAlongside".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Marble, 2) == 1)
                OutputText("Scenes.Secretarialsuccubusscene.DoBadEndSuccubusPart2.WithMarbleHereToo".Localized());

            GameOver();
        }

    }
}