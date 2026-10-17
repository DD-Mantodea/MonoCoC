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
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<PriscillaScene>("coc:priscilla")]
    public class PriscillaScene : Scene
    {

        public override void Entry()
        {
            GoblinElderEncounter();
        }

        public void GoblinElderEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredGoblinElder) == 0)
            {
                OutputText("Scenes.Priscillascene.GoblinElderEncounter.StrollImposingGoblin".Localized());

                if (Player.HasCock())
                {
                    if (Player.IsPureEnough(60))
                        OutputText("Scenes.Priscillascene.GoblinElderEncounter.LikeToFuckMe".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderEncounter.DontFuckMeCorrupt".Localized());
                }
                else
                    OutputText("Scenes.Priscillascene.GoblinElderEncounter.NothingToKnockUp".Localized());
            }
            else
                if (Player.HasVagina() && !Player.HasCock() && (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) == 1 || CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) == 2))
                    Startffaction();
                else
                {
                    OutputText("Scenes.Priscillascene.GoblinElderEncounter.FamiliarSightPowerfulGoblin".Localized((CoC.Instance.Flags.Get(GameFlagIds.GoblinElderTalkCounter) > 0 ? "Priscilla" : "the goblin elder")));

                    if (Player.HasCock())
                    {
                        if (Player.IsPureEnough(60))
                            OutputText("Scenes.Priscillascene.GoblinElderEncounter.HelloAgain".Localized((Player.HasCock() ? ", stud. Ready to knock me up with your cum? Or are you here to duel?" : ",")));
                        else
                            OutputText("Scenes.Priscillascene.GoblinElderEncounter.TooCorruptAgain".Localized());
                    }
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderEncounter.KnockUpChallenge".Localized());
                }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredGoblinElder, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredGoblinElder) + 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryGoblins);

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), StartFight);

            if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderTalkCounter) < 2)
                AddButton(1, "Common.Button.Talk".Localized(), TalkToGoblinElder);

            if (Player.HasVagina() && Player.Lust >= 33 && Player.IsPureEnough(60))
            {
                CurrentMonster = EnemyRegistry.Create(EnemyIds.Priscilla);

                AddButton(2, "Common.Button.SexAnyway".Localized(), Startffaction);

                if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) == 3 || Player.HasCock())
                    AddButton(2, "Common.Button.Sex".Localized(), Startforeplay);
            }

            if (Player.HasCock() && Player.Lust >= 33 && Player.IsPureEnough(60))
            {
                CurrentMonster = EnemyRegistry.Create(EnemyIds.Priscilla);

                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                    AddButton(2, "Common.Button.Sex".Localized(), Startforeplay);
                else
                    OutputText("Scenes.Priscillascene.GoblinElderEncounter.NoWayCockFits".Localized());
            }

            SetExitButton("Common.Button.Back".Localized());
        }

        public void StartFight()
        {

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Priscilla), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);
        }

        public void GoblinElderBeatYaUp()
        {
            ClearOutput();

            int x = 0;

            if (Player.LustPercent <= 99)
                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.CollapseBeatenRestrain".Localized());

            if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()) && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
            {
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

                if (x < 0)
                    x = Player.SmallestCockIndex();

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SoArousedHornyModesty".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.WatchesImpassivelyNeed".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ThatThingWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ThoseThingsWord".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.LookMightyTastySample".Localized(
                    Player.Mf("stud", "sexy")
                ));

                if (Player.Tallness >= 60)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.BigWord".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.EnoughJizzToPay".Localized(
                    Player.Mf("strapping young man", "sexy breeder")
                ));

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.DancingFingersPirouette".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.GentleCaressSack".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.TipsSoakedMound".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.TipsSensitiveTaint".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ExplainsWontHelpAnswer".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.AnswerNoTrailsOff".Localized());

                if (Player.Femininity < 40)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.AlmostWord".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.GirlishMoanNeedThrumming".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ViridianSkinCaresses".Localized());

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.TheSheathWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.YourBaseWord".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.BriefMomentVanishes".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ResponseImmediateEnthusiastic".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.MusesSquirtingAlready".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PluralS".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.WildNeedSubmitLick".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ToesSmearPrecum".Localized(
                    Player.CockHead(x),

                    NumberUtils.Num2Text(Player.Cocks[x].Length),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ShortStackedMovesLegs".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SayStart".Localized());

                if (silly)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.StopCriminalScum".Localized());
                else
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.HoldYourHorses".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.GlancesUrgesComeOn".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.InYourBallsWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.InsideYouWord".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PayFineLetGo".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.HandSqueezesSack".Localized());
                else
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.HandsSqueezeButt".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ImpalesHerselfOrgasm".Localized());

                if (Player.CumQ() >= 750)
                {
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.FertilityCantKeepUp".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SwollenPregnantDome".Localized());
                }

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PassionSpendsClimax".Localized());

                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.StandingWhiteGooWink".Localized());
            }
            else
                if (Player.BiggestCockArea() > CombatController.Instance!.Context.Enemy.VaginalCapacity() && Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
                {
                    x = Player.BiggestCockIndex();

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.TearThroughArmorRush".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SighsGrumblesTreeTrunk".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.WatchTiradeConfused".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PuffyCurtainsDrapeDick".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.BiggestCockLength() > Player.Tallness / 2)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.LongPhallusBlocksView".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.NoneTooTenderSwat".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.BallsWord".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.CockWord".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.GoblinGruntsStopSquirming".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.HesitantlyObeyStartled".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ViridianTartLivestock".Localized());

                    if (Player.BiggestCockLength() < Player.Tallness / 2)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SittingOnFaceMaleness".Localized(
                            Player.CockHead(x)
                        ));
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SittingGroundAboveYou".Localized(
                            Player.CockHead(x)
                        ));

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SoftFeetClaspDick".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.LubricatedVaginalDrippings".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SpurtDollopsPreCum".Localized());
                    else
                        if (Player.CumQ() >= 250)
                            OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.DroolSteadyFlow".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.WontBeLongElder".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.GruntProtestMuffled".Localized());

                    if (Player.BiggestCockLength() >= Player.Tallness / 2)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PulsatingIronHardCock".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.CaptorsPillowyAsscheeks".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PullsHardFeetSlurps".Localized(
                        Player.CockHead(x),

                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() <= 10)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.DribblesWord".Localized());
                    else
                        if (Player.CumQ() <= 100)
                            OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SquirtsWord".Localized());
                        else
                            if (Player.CumQ() <= 250)
                                OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ThickWadsWord".Localized());
                            else
                                if (Player.CumQ() <= 1750)
                                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.WavesGooeySpoogeWord".Localized());
                                else
                                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.RiversOfCumWord".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.IntoGoblinsBox".Localized());

                    if (Player.CumQ() >= 2500)
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.BellyRoundsUp".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.GreenGirlLastSqueezes".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.DefeatedByNeedsStrip".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.BeginMasturbatingViolently".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PistonNippleMasturbate".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SqueezeTautNipple".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ReverieInterruptedImpacts".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ConfusionStareStraddles".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.ProtestDropsOnFace".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.TauntsLickFasterSlut".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.IrritatedSlapSkin".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.MomentaryPainVertigo".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.SuckClitSlideButt".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PantingIntoGreenCunt".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PinprickEngorgedNipple".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.EyesRollViridian".Localized());

                    OutputText("Scenes.Priscillascene.GoblinElderBeatYaUp.PassOutPuddleJuices".Localized());
                }

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void GoblinElderRapeIntro()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.GoblinElderRapeIntro.ElderFallsTittiesArmor".Localized());

            if (Player.Lust < 33)
                OutputText("Scenes.Priscillascene.GoblinElderRapeIntro.NotHornyEnoughRape".Localized());

            AddButton(0, "Scenes.Goblin.Button.DickFuck".Localized(), null);

            AddButton(1, "Scenes.Goblin.Button.DickTooBig".Localized(), null);

            AddButton(2, "Scenes.Goblin.Button.CorruptDick".Localized(), null);

            AddButton(3, "Scenes.Goblin.Button.DickInAss".Localized(), null);

            AddButton(4, "Scenes.Goblin.Button.JogFuck".Localized(), null);

            AddButton(5, "Common.Button.Breastfeed".Localized(), null);

            AddButton(6, "Scenes.Goblin.Button.UseCondom".Localized(), null);

            AddButton(7, "Scenes.Goblin.Button.Pussies".Localized(), null);

            AddButton(8, "Common.Button.LayEggs".Localized(), null);

            if (Player.HasVagina() && Player.Lust >= 33)
                AddButton(7, "Scenes.Goblin.Button.Pussies".Localized(), GobboGetsRapedFem);

            if (Player.HasCock() && Player.Lust >= 33)
            {
                if (Player.CockArea(Player.BiggestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity() && Player.IsCorruptEnough(80) || Player.HasPerk(PerkIds.Sadist))
                    AddButton(2, "Scenes.Goblin.Button.CorruptDick".Localized(), RapeAGoblinCorruptTooBig);

                if (Player.CockArea(Player.BiggestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                    AddButton(1, "Scenes.Goblin.Button.DickTooBig".Localized(), ManRapesGoblinTooBig);

                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                {
                    AddButton(0, "Scenes.Goblin.Button.DickFuck".Localized(), GatsGoblinBoners);

                    AddButton(4, "Scenes.Goblin.Button.JogFuck".Localized(), GobboGetsRapedMaleFits);
                }

                if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) >= 0 && (Player.IsCorruptEnough(70) || Player.HasPerk(PerkIds.Sadist)))
                    AddButton(3, "Scenes.Goblin.Button.DickInAss".Localized(), GobboButtSecks);

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.SpiderAbdomen && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                    AddButton(6, "Common.Button.WebCondom".Localized(), () => GoblinCondomed(0));
                else
                    if (Player.HasItem(ItemRegistry.Create(ItemIds.Condom)) && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                        AddButton(6, "Scenes.Goblin.Button.UseCondom".Localized(), () => GoblinCondomed(1));
            }

            if (Player.CanOvipositSpider())
                AddButton(8, "Common.Button.LayEggs".Localized(), LaySomeDriderEggsInGobboTwat);

            if (Player.LactationQ() >= 500 || Player.HasStatusEffect(StatusEffectIds.Feeder))
                AddButton(5, "Common.Button.Breastfeed".Localized(), GiveGoblinAMilkMustache);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void GiveGoblinAMilkMustache()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.GiveGoblinAMilkMustache.WalkUpGentlyTell".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Priscillascene.GiveGoblinAMilkMustache.StrugglesGripNeedyPussy".Localized(
                Player.Mf("slut", "stud"),

                Player.NippleDescript(0)
            ));

            OutputText("Scenes.Priscillascene.GiveGoblinAMilkMustache.FallsAsleepArms".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f), new DynStat(GameStats.Lust, -50f));

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void GobboButtSecks()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            ShowImage("goblinelder-win-male-buttsex");

            OutputText("Scenes.Priscillascene.GobboButtSecks.EasilyDefeatAnotherGoblin".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.BegsStudDemands".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.InsultedWhoVictorious".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.RemoveArmorTearClothing".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.GobboButtSecks.SneerCrownPress".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.GobboButtSecks.NotThereLipsTrembling".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.SayNothingTremblingWoman".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.SlamGoblinWhoreImpaling".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.GobboButtSecks.SquirmsBeatsFists".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.GobboButtSecks.NotWhatIWanted".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText(Player.SackDescript());
            else
                OutputText(Player.HipDescript());

            OutputText("Scenes.Priscillascene.GobboButtSecks.SmackingPlumpRump".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.TightConfinesConvulses".Localized());

            OutputText("Scenes.Priscillascene.GobboButtSecks.ArmorGlanceHands".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void GobboGetsRapedFem()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                ShowImage("goblinelder-win-female-taur-rapedfem");

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.HoldingTearStrip".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.BlushesFlowThickens".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ThirdBottleGivenUp".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.FlipCreatureCrux".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SquealsDelightOrPain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.DropHerForReal".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.CantSeeStrugglingStops".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.EffortsRewardedCum".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.CasuallyDressIgnoringPants".Localized());

                CombatScene.CleanupAfterCombat();

                Player.Orgasm("Vaginal");
            }
            else
                if (Player.IsNaga())
                {
                    ShowImage("goblinelder-win-female-naga-rapedfem");

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SlitherOverHelpless".Localized());

                    if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.BareFangsCuriosityGone".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SmileFearDisappears".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ParagraphBreak".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.WrapTightlyPrey".Localized());

                    if (Player.Corruption > 60)
                    {
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.HissIncoherentQuiver".Localized());

                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SpreadOpenMouthFiddle".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.AskIfSorryPoison".Localized());

                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.OpenMouthPreventClosing".Localized());

                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.PityNotFarEnough".Localized());
                    }

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.EffectsPurplishDesperate".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SlideTailTweakClit".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SmoothMotionTailInside".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.PullOutGush".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.PullOutExamineCentimeter".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ShruggingDumpContents".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.EyesFlyOpenFalse".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.MuffledGotItBoss".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.OrgasmRepeatedlyNotTiring".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ThoroughlyConfusedDecideLeave".Localized());

                    CombatScene.CleanupAfterCombat();

                    Player.Orgasm("Generic");
                }
                else
                {
                    ShowImage("goblinelder-win-female-rapedfem");

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SittingOnKnee".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.BlushesFlowThickensAgain".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ThirdBottleGrind".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Vaginas[0].Wetness < 2)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.GrowsPuffyMoist".Localized());
                    else
                        if (Player.Vaginas[0].Wetness < 4)
                            OutputText("Scenes.Priscillascene.GobboGetsRapedFem.DripsFeminineMoisture".Localized());
                        else
                            if (Player.Vaginas[0].Wetness < 5)
                                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SoakYourThighs".Localized());
                            else
                                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.DroolsNeedPuddling".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ArousalCanWaitPunished".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ThwackHandSmacksAss".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ScoldSlapHarder".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SquealsDelightPain".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.PullLegShoveFace".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.CumOnDruggedGreen".Localized());

                    if (Player.Vaginas[0].Wetness > 4)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SplatteringWithFluids".Localized());
                    else
                        if (Player.Vaginas[0].Wetness > 3)
                            OutputText("Scenes.Priscillascene.GobboGetsRapedFem.CoatingFaceProof".Localized());
                        else
                            if (Player.Vaginas[0].Wetness > 2)
                                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.StickyNetherJuices".Localized());
                            else
                                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.TasteOfPussyTongue".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.PeriodSpace".Localized());

                    if (Player.BiggestLactation() >= 3.5f)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.MilkExplodesSoaking".Localized());
                    else
                        if (Player.BiggestLactation() > 2)
                            OutputText("Scenes.Priscillascene.GobboGetsRapedFem.TwinStreamsMilk".Localized());
                        else
                            if (Player.BiggestLactation() >= 1)
                                OutputText("Scenes.Priscillascene.GobboGetsRapedFem.MilkDribblesHair".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.StrangeDopeySmile".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.DressIgnoringMoans".Localized());

                    if (Player.Corruption > 50)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SmirkWord".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.SighWord".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedFem.AndSaunterOffFeeling".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.BitGuiltyOverdoing".Localized());
                    else
                        OutputText("Scenes.Priscillascene.GobboGetsRapedFem.ThoroughlySatisfiedRevenge".Localized());

                    CombatScene.CleanupAfterCombat();

                    Player.Orgasm("Vaginal");
                }
        }

        public void RapeAGoblinCorruptTooBig()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            ShowImage("goblinelder-win-male-corruptedfuck");

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.BeginRemoveArmorHelpless".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.IsWord".Localized());
            else
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.AreAllWord".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.MoreAwareStoopStrip".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.YourCocksRest".Localized());
            else
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.CockRestOnTop".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.Cocks[0].Length < 20)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.LongAsEntireTorso".Localized());
            else
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.BiggerThanSheIs".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.WickedIdea".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.SeatLegsWide".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                Player.CockDescript(x),

                CombatController.Instance!.Context.Enemy.VaginaDescript(),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.BothDrippingSweat".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.RelaxesNoLongerPressing".Localized(
                Player.CockDescript(x),

                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.RepositionFucktoyFacingAway".Localized(
                Player.CockDescript(x),

                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.TipPopsIn".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.RestFollowsHead".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.StopShaftSinksIn".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.DontMoreEnters".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.PleaseFillsCapacity".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.AndBeyond".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.FuckMe".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.RamHomeDickJockey".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[0].Length < 20)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.MassiveBulgeMidsection".Localized());
            else
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.StretchVisibleBulge".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.DrivesLustHigherWithdraw".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.HeadRestingBetweenBreasts".Localized());

                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.AsYourNipple".Localized());

                    if (Player.BiggestLactation() < 2)
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.DripMilkyTears".Localized());

                    if (Player.BiggestLactation() < 3)
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.WeepStreamsOfMilk".Localized());
                    else
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.PlasterGoutsFluid".Localized());
                }
            }

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.HairMattedWith".Localized());

            if (Player.BiggestLactation() >= 1)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.MilkAndWord".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.SweatOfBothGurgle".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.AlrightWhoreBabiesCum".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.WhileYourCock".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Wetness <= 3)
                    OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.JuicesWord".Localized());
                else
                    if (Player.Vaginas[0].Wetness <= 4)
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.FloodsWord".Localized());
                    else
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.ExplodesWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.CocksDrenchHer".Localized());
            }
            else
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.WhileYourOther".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.DrenchHerWord".Localized());
                    else
                        OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.DrenchesHerWord".Localized());
                }

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.TightNaturalRing".Localized());

            if (Player.Balls >= 2)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.BallsTryingButH".Localized());
            else
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.HWord".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.SpasmingClampingDown".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript(),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.MomentPassesUnsatisfied".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.SeemsLikeHoursScrew".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.TryAgainSitting".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.PressedFirmlySeeCum".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.RemainingCocksGeysers".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.RemainingPenisGeysers".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.SameTightnessPrevented".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.FinallyBlowLastWad".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.StandPrepareToLeave".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.DaughtersBetterFuck".Localized());

            OutputText("Scenes.Priscillascene.RapeAGoblinCorruptTooBig.SheAbsolutelyWill".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void ManRapesGoblinTooBig()
        {
            ClearOutput();

            ShowImage("goblinelder-win-male-corruptedbj");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.WhipOutStiffeningMaleness".Localized());

            if (Player.CockArea(x) < 80)
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.NearlyWord".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.AbsurdSizeDinnerPlates".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.YouWord".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.GuessMayAsWell".Localized());
            else
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.GrinHappyToStretch".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.TheWord".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.FlareWord".Localized());
            else
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.HeadWord".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.PulsesInExcitement".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.FlareWordAgain".Localized());
            else
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.CrownWord".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.StretchNoMore".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.DisgruntledPullOutTorso".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasKnot(0))
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.HookingHeelsBehindKnot".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.PeriodBreak".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.GoodAtGrinding".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.HermWord".Localized());
            else
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.ManWord".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.ForSureDrippingExcitement".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.NeverThinkSoEnjoyable".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 75)
                OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.SplatteringFewMoreTimes".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.PaintingThickLayer".Localized());
                else
                    OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.SoakingThickPuddle".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.GreenSlutHandlesScoop".Localized());

            OutputText("Scenes.Priscillascene.ManRapesGoblinTooBig.ShakeHeadLeaveDrained".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void GobboGetsRapedMaleFits()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.BiggestCockIndex();

            ShowImage("goblinelder-win-male-getridden");

            if (Player.CockArea(x) > CombatController.Instance!.Context.Enemy.VaginalCapacity() * .8f)
            {
                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.DefeatedCrossesArms".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.StartWalkingGravity".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.RubbingPainfullyNubs".Localized());
                else
                    if (Player.HasKnot(0))
                        OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.StretchingPainfullyKnot".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                            OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.FeelingPainfullyTight".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.EveryStepSlidesDown".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText(Appearance.AllBreastsDescript(Player));
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.TorsoWord".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.SupportHerselfSteadyPulse".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PantsHappilySoFull".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.AllEncouragementBreakRun".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.TitsWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.SkinWord".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.HangsOnClenchingSmashing".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.FuckStudBounceMe".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.FamiliarTightnessComingOrgasm".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.BallsTightenSeed".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.HowlPowerfulOrgasm".Localized());

                if (Player.CumQ() >= 100)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.EnhancedBodyStuffsCream".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.OrgasmPotentPregnantDome".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if (Player.CumQ() >= 250)
                        OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.OrgasmPotentHeavilyPregnant".Localized(
                            Player.CockDescript(x)
                        ));

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ParagraphBreak".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PantStopSetDown".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PoursOutRiverWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.LeaksWord".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.GapingTwatBlowsKiss".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ChucklingWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ShudderingWord".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.BackToCampSatisfied".Localized());
            }
            else
            {
                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PickUpDefeatedAgain".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.StartWalkingMovementsGravity".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.RubbingPerfectlyNubs".Localized());
                else
                    if (Player.HasKnot(0))
                        OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.StretchingTightlyKnot".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                            OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.FeelingAbsolutelyPerfect".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.EveryStepSlides".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText(Appearance.AllBreastsDescript(Player));
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.TorsoWordAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.SteadyPulseAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PantsCheersYouOn".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.EncouragementRun".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.TitsWordAgain".Localized());
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.SkinWordAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ClenchingSmashing".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.FuckStudBounceAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.TightnessOrgasmAgain".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.BallsTightenSeedAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.HowlPowerfulOrgasmAgain".Localized());

                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.StuffsCreamAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText(Player.BallsDescriptLight());
                    else
                        OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.LegsWord".Localized());

                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.SentencePeriod".Localized());
                }

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PotentOrgasmHeavy".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PantThenSetDown".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.PoursRiverAgain".Localized());
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.LeaksWordAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.GapingTwatKiss".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ChucklingWordAgain".Localized());
                else
                    OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.ShudderingWordAgain".Localized());

                OutputText("Scenes.Priscillascene.GobboGetsRapedMaleFits.CampSatisfiedAgain".Localized());
            }

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void GoblinCondomed(float mode = 0)
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            ShowImage("goblinelder-win-male-goblincondomed");

            OutputText("Scenes.Priscillascene.GoblinCondomed.DefeatedKneesGiveOut".Localized());

            OutputText("Scenes.Priscillascene.GoblinCondomed.ImpatientlyHooksToes".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Priscillascene.GoblinCondomed.YourThrobbingSackWord".Localized());
            else
                OutputText("Scenes.Priscillascene.GoblinCondomed.BaseOfShaftWord".Localized());

            OutputText("Scenes.Priscillascene.GoblinCondomed.DigitsCurlMember".Localized());

            OutputText("Scenes.Priscillascene.GoblinCondomed.WideningLakeHoney".Localized());

            if (mode == 0)
                OutputText("Scenes.Priscillascene.GoblinCondomed.WorkSpinneretsSheath".Localized(
                    Player.CockDescript(x)
                ));
            else
            {
                Player.DestroyItems(ItemIds.Condom, 1);

                OutputText("Scenes.Priscillascene.GoblinCondomed.TearPacketLatex".Localized(
                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Priscillascene.GoblinCondomed.GiveWhatAsked".Localized());

            OutputText("Scenes.Priscillascene.GoblinCondomed.BendLegsApart".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.GoblinCondomed.MouthyBitchForgotRemind".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Priscillascene.GoblinCondomed.SquirmingSweatAndMud".Localized());

            if (mode == 0)
                OutputText("Scenes.Priscillascene.GoblinCondomed.InformSpiderSilkSticky".Localized());
            else
                OutputText("Scenes.Priscillascene.GoblinCondomed.InformLatexPreventsPregnancy".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void GatsGoblinBoners()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("goblinelder-win-male-goblinboners");

            if (CombatController.InCombat)
            {
                OutputText("Scenes.Priscillascene.GatsGoblinBoners.GoblinStrewnOnStomach".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.LuckilyIntentionChanging".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.BeginFondleYourCock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralS".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.WalkTowardsUnsuspecting".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSAgain".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.PeriodBreak".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.WhaddyaDoingBack".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSThird".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.AndSlapWord".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.ItWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.ThemWord".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.AgainstBareFleshAss".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.YourDickInsidePanting".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Priscillascene.GatsGoblinBoners.BothDicksInsidePanting".Localized());
                    else
                    {
                        OutputText("Scenes.Priscillascene.GatsGoblinBoners.TwoDicksInsidePanting".Localized());

                        if (Player.CockTotal() >= 4)
                            OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSFourth".Localized());

                        OutputText("Scenes.Priscillascene.GatsGoblinBoners.RemainingSandwichedAsscheeks".Localized());
                    }

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.PeriodBreakAgain".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.RoughlyPoundGoblinGirl".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.EventuallyFeelClimaxFaster".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSFifth".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.SlowlyReleaseCoverBack".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.PickYourselfUpJerking".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Priscillascene.GatsGoblinBoners.TellFuckEarsPerk".Localized(
                    Player.ClothedOrNakedLower("You open up your [armorname]" + ((Player.Armor.LongName != "nothing" && Player.LowerGarment.Name != "nothing") ? " and [lowergarment]" : "") + ", revealing your [cocks].", "")
                ));

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.FondleCockAgain".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSSixth".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.WalkTowardsGoblinGirl".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSSeventh".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.PeriodBreakThird".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.YelpsFuckMeAlready".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSEighth".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.AndSlapWordAgain".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.ItWordAgain".Localized());
                else
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.ThemWordAgain".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.BareFleshAssAgain".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.DickInsidePanting".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Priscillascene.GatsGoblinBoners.BothDicksPanting".Localized());
                    else
                    {
                        OutputText("Scenes.Priscillascene.GatsGoblinBoners.TwoDicksPanting".Localized());

                        if (Player.CockTotal() >= 4)
                            OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSNinth".Localized());

                        OutputText("Scenes.Priscillascene.GatsGoblinBoners.SandwichedCheeksAgain".Localized());
                    }

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.PeriodBreakFourth".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.PoundAgainstGoblinElder".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.FeelClimaxFaster".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Priscillascene.GatsGoblinBoners.PluralSTenth".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.ReleaseCoverBack".Localized());

                OutputText("Scenes.Priscillascene.GatsGoblinBoners.UpJerkingAgain".Localized(
                    Player.CockDescript(x)
                ));
            }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LaySomeDriderEggsInGobboTwat()
        {
            ClearOutput();

            ShowImage("goblinelder-win-drider-egged");

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.PitifulStrugglingGreenCreature".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.HardenWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.MoistenWord".Localized());
                else
                    OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.TwitchWord".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.SlightlyGrowingWeightSpider".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.QuicklyUndressSkitterOver".Localized());

            if (Player.Femininity > 50)
                OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.BitchIdeaOfFun".Localized());
            else
                OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.BastardIdeaOfFun".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.LashesFeetNoStrength".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.SquirmsLemmeGo".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.PlaythingFallenSilent".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.NervousToneConceal".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.StifledGaspsLivingSleeve".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.BottomOutCumSleeve".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.SlowPathEggTube".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.OrgasmJustStrong".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.AndYWord".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.YWord".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.OurVaginaClenching".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.NextEggRollsForward".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.PinnedFillUp".Localized());

            OutputText("Scenes.Priscillascene.LaySomeDriderEggsInGobboTwat.LayingDownInShade".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void TalkToGoblinElder()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.TalkToGoblinElder.AskIfMindTalking".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderTalkCounter) == 0)
            {
                OutputText("Scenes.Priscillascene.TalkToGoblinElder.WhatIsSheUnusual".Localized());

                OutputText("Scenes.Priscillascene.TalkToGoblinElder.OfCoursePriscillaTraveled".Localized());

                OutputText("Scenes.Priscillascene.TalkToGoblinElder.WhatAreCurrentGoals".Localized());

                OutputText("Scenes.Priscillascene.TalkToGoblinElder.SeekingNotCorrupt".Localized((CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0 ? ". I did hear the news that you have defeated Lethice. You're my savior and I'll be forever grateful at you for causing a major blow against the demons" : "")));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderTalkCounter) == 1)
                {
                    OutputText("Scenes.Priscillascene.TalkToGoblinElder.WhereOriginallyCameFrom".Localized());

                    OutputText("Scenes.Priscillascene.TalkToGoblinElder.OldGoblinCityTechnological".Localized());
                }

            OutputText("Scenes.Priscillascene.TalkToGoblinElder.ThankThenWaveOff".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GoblinElderTalkCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderTalkCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void Startffaction()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) == 0)
            {
                OutputText("Scenes.Priscillascene.Startffaction.SmileSeductivelyAskCompany".Localized());

                OutputText("Scenes.Priscillascene.Startffaction.DontFuckForFun".Localized());

                OutputText("Scenes.Priscillascene.Startffaction.StartProtestWalkedAway".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), () => ReturnToCamp(1));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) == 1)
                {
                    OutputText("Scenes.Priscillascene.Startffaction.WalkExploringSeePriscilla".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Check".Localized(), Checkonher);

                    AddButton(1, "Common.Button.Leave".Localized(), Leaveher);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) == 2)
                    {
                        OutputText("Scenes.Priscillascene.Startffaction.HeyYouAwkwardly".Localized());

                        OutputText("Scenes.Priscillascene.Startffaction.FriendSinceDemons".Localized());

                        DoNext(() => Scissorscene());
                    }
        }

        public void Checkonher()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Checkonher.OutOfHidingThroat".Localized());

            OutputText("Scenes.Priscillascene.Checkonher.HowRudeBestIntentions".Localized());

            OutputText("Scenes.Priscillascene.Checkonher.ThenAgainLooksAwful".Localized());

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney), 1) || Player.HasItem(ItemRegistry.Create(UseableIds.PurePearl), 1) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurityPhilter), 1))
                OutputText("Scenes.Priscillascene.Checkonher.HappenToHaveItem".Localized());

            ClearButtons();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney), 1) || Player.HasItem(ItemRegistry.Create(UseableIds.PurePearl), 1) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurityPhilter), 1))
                AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), Helpher);
            else
                AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), null);

            AddButton(1, "Combat.Ability.SuccubusSeduceAttack.Name".Localized(), Seduceher);

            AddButton(2, "Common.Button.Rape".Localized(), GobboGetsRapedFem);

            AddButton(14, "Common.Button.Leave".Localized(), Leaveher2);
        }

        public void Helpher()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.GoblinElderRelationCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) + 1);

            OutputText("Scenes.Priscillascene.Helpher.TakeOutVial".Localized());

            OutputText("Scenes.Priscillascene.Helpher.WhispersNeedCharity".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Leaveher()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.LeaveHer.BigGirlNotFirst".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Leaveher2()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Leaveher2.NoBackwardsGlance".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Seduceher()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Seduceher.NotGoingAnywhereStrip".Localized());

            OutputText("Scenes.Priscillascene.Seduceher.StopMoansReachWeapon".Localized());

            OutputText("Scenes.Priscillascene.Seduceher.StopPlayingTurnAround".Localized(
                Player.AssDescript(),

                Player.AssDescript(),

                Player.AssDescript()
            ));

            OutputText("Scenes.Priscillascene.Seduceher.CryArchesBackCums".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Scissorscene()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.GoblinElderRelationCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderRelationCounter) + 1);

            OutputText("Scenes.Priscillascene.Scissorscene.EagerlyRummagesThrowsVial".Localized());

            OutputText("Scenes.Priscillascene.Scissorscene.AlrightReadySmile".Localized());

            OutputText("Scenes.Priscillascene.Scissorscene.FeltLikeElectricity".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Scissorscene.CockFlopsNoisilyPre".Localized());

            OutputText("Scenes.Priscillascene.Scissorscene.DesperationGrabsHandJerks".Localized());

            OutputText("Scenes.Priscillascene.Scissorscene.GruntsGonnaCumBreaks".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Scissorscene.AndYourCockJerking".Localized());

            OutputText("Scenes.Priscillascene.Scissorscene.ThrustLastTime".Localized());

            OutputText("Scenes.Priscillascene.Scissorscene.AlmostGoodAsCock".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Startforeplay()
        {
            ClearOutput();

            if (Player.HasCock() && Player.HasVagina())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderForeplayCounter) == 0)
                {
                    OutputText("Scenes.Priscillascene.Startforeplay.PoutsThoughtUnderstood".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredGoblinElder) <= 2)
                        OutputText("Scenes.Priscillascene.Startforeplay.DicksLikeYoursScarce".Localized());
                    else
                        OutputText("Scenes.Priscillascene.Startforeplay.SwallowingAssNoHelp".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.CausesHerToStop".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GoblinElderForeplayCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderForeplayCounter) + 1);
                }
                else
                {
                    OutputText("Scenes.Priscillascene.Startforeplay.TellHerLezOut".Localized());

                    if (Player.Tallness >= 60)
                        OutputText("Scenes.Priscillascene.Startforeplay.KneelScoopForKiss".Localized());
                    else
                        OutputText("Scenes.Priscillascene.Startforeplay.PullPriscillaIntoKiss".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.SourTongueOneHand".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.YesFingerMyAss".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.DefinitelyVagToday".Localized());

                    if (Player.CumQ() >= 750)
                        OutputText("Scenes.Priscillascene.Startforeplay.KneesStartLapping".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.HandsOnSoftenedCock".Localized(
                        Player.CockDescript(0)
                    ));
                }

                ClearButtons();

                AddButton(0, "Common.Button.Scissor".Localized(), Scissorscene);

                AddButton(1, "Common.Button.Some".Localized(), Unleashthekraken);

                AddButton(2, "Common.Button.Wrestle".Localized(), Wrestle);

                AddButton(3, "Common.Button.PriDom".Localized(), Priscilladom);
            }

            if (Player.HasCock())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderForeplayCounter) == 0)
                {
                    OutputText("Scenes.Priscillascene.Startforeplay.TellHerFuckGrin".Localized(
                        Player.CockDescript(0),

                        NumberUtils.Num2Text(Player.Cocks[0].Length)
                    ));

                    if (Player.Tallness >= 60)
                        OutputText("Scenes.Priscillascene.Startforeplay.KneelScoopKissFirst".Localized());
                    else
                        OutputText("Scenes.Priscillascene.Startforeplay.PullPriscillaKissFirst".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.HalfwayExpectingSwoon".Localized(
                        Player.CockDescript(0)
                    ));

                    OutputText("Scenes.Priscillascene.Startforeplay.NaughtyBoyPurrs".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GoblinElderForeplayCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderForeplayCounter) + 1);
                }
                else
                {
                    OutputText("Scenes.Priscillascene.Startforeplay.ThickCreamyLoadSilk".Localized(
                        Player.CockDescript(0)
                    ));

                    OutputText("Scenes.Priscillascene.Startforeplay.PickUpGently".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Toughness, -1f));

                    OutputText("Scenes.Priscillascene.Startforeplay.StudNoMoreGames".Localized());
                }

                ClearButtons();

                AddButton(0, "Common.Button.Pussy".Localized(), Pussyfuck);

                AddButton(1, "Common.Button.Ass".Localized(), Assfuck);

                AddButton(2, "Common.Button.Mouth".Localized(), Mouthfuck);
            }

            if (Player.HasVagina())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderForeplayCounter) == 0)
                {
                    OutputText("Scenes.Priscillascene.Startforeplay.TellHerSomeCunt".Localized());

                    if (Player.Tallness >= 60)
                        OutputText("Scenes.Priscillascene.Startforeplay.ScoopForKissAgain".Localized());
                    else
                        OutputText("Scenes.Priscillascene.Startforeplay.PriscillaKissAgain".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.SourTastingTongueAgain".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.YouBitchFrenchKiss".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.BabeLetsGetDown".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.MentionObviousLipsNethers".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.SoHowHandleThis".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GoblinElderForeplayCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderForeplayCounter) + 1);
                }
                else
                {
                    OutputText("Scenes.Priscillascene.Startforeplay.TellHerForHer".Localized());

                    if (Player.Tallness >= 60)
                        OutputText("Scenes.Priscillascene.Startforeplay.ScoopForKissThird".Localized());
                    else
                        OutputText("Scenes.Priscillascene.Startforeplay.PriscillaKissThird".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.ThrustsTongueIntoMouth".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.YesSpankMeHarder".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.TemptingLayOnBack".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderWrestleCounter) != 0)
                        OutputText("Scenes.Priscillascene.Startforeplay.ThisTimeWord".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.KnowTheToll".Localized());

                    OutputText("Scenes.Priscillascene.Startforeplay.HowSpendTimeToday".Localized());
                }

                ClearButtons();

                AddButton(0, "Common.Button.Scissor".Localized(), Scissorscene);

                AddButton(1, "Common.Button.Some".Localized(), Unleashthekraken);

                AddButton(2, "Common.Button.Wrestle".Localized(), Wrestle);

                AddButton(3, "Common.Button.PriDom".Localized(), Priscilladom);
            }
        }

        public void Unleashthekraken()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Unleashthekraken.GoesToArmorFleshlight".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.ShoutedAttackRushedBeast".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.TentacleSlides".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderBeastCounter) == 0)
                OutputText("Scenes.Priscillascene.Unleashthekraken.AskIfBeastTrusted".Localized());
            else
                OutputText("Scenes.Priscillascene.Unleashthekraken.AskHowLearnedCall".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.BothBecomingBoundBounce".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderBeastCounter) == 0)
            {
                OutputText("Scenes.Priscillascene.Unleashthekraken.GetReadyToRock".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GoblinElderBeastCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderBeastCounter) + 1);
            }
            else
                OutputText("Scenes.Priscillascene.Unleashthekraken.HereItComesMania".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.SuppressScreamDeepCaverns".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Unleashthekraken.InstantlyErectPainExciting".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.YesYesGoingWild".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Unleashthekraken.StartsCummingEverywhere".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.MeNextMeNext".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.RightThereYesYes".Localized());

            OutputText("Scenes.Priscillascene.Unleashthekraken.AwakeHourLaterGone".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Wrestle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GoblinElderWrestleCounter) == 0)
                OutputText("Scenes.Priscillascene.Wrestle.GrabStickCreateCircle".Localized());
            else
            {
                OutputText("Scenes.Priscillascene.Wrestle.CircleAgainWrestles".Localized());

                OutputText("Scenes.Priscillascene.Wrestle.SureBabeSetCircle".Localized());
            }

            DoNext(() => Wrestle2());
        }

        public void Wrestle2()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.GoblinElderWrestleCounter, CoC.Instance.Flags.Get(GameFlagIds.GoblinElderWrestleCounter) + 1);

            OutputText("Scenes.Priscillascene.Wrestle2.ArmLockPushingPulling".Localized());

            OutputText("Scenes.Priscillascene.Wrestle2.RollOffHeadbutt".Localized());

            OutputText("Scenes.Priscillascene.Wrestle2.ChargedAgainRolledUnder".Localized());

            if ((Player.Strength + Player.Toughness + Player.Speed + Player.Intelligence / 2 + Random.Shared.Next(50)) > 230)
            {
                OutputText("Scenes.Priscillascene.Wrestle2.ButYouAreFaster".Localized());

                OutputText("Scenes.Priscillascene.Wrestle2.PushingBackHeldFirm".Localized());

                OutputText("Scenes.Priscillascene.Wrestle2.ShutUpLaughedWaited".Localized());

                DoNext(() => Dompriscilla());
            }
            else
            {
                OutputText("Scenes.Priscillascene.Wrestle2.TryGrabTooSlow".Localized());

                OutputText("Scenes.Priscillascene.Wrestle2.ExpectedBetter".Localized());

                DoNext(() => Priscilladom());
            }
        }

        public void Dompriscilla()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Dompriscilla.DemureBlowKiss".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Dompriscilla.StartPullingGettingHard".Localized());

            OutputText("Scenes.Priscillascene.Dompriscilla.HandRubsButtSides".Localized());

            OutputText("Scenes.Priscillascene.Dompriscilla.MmmmDiningOnSnatch".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Dompriscilla.PushCockInFace".Localized());

            OutputText("Scenes.Priscillascene.Dompriscilla.ChuckleGrabThingsAnnoyed".Localized());

            OutputText("Scenes.Priscillascene.Dompriscilla.TakeYouDownNext".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Priscilladom()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Priscilladom.OrderedToLay".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Priscilladom.CockTwirlsAchingContact".Localized());

            OutputText("Scenes.Priscillascene.Priscilladom.LoveBeingBottomBitch".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Priscillascene.Priscilladom.CockFullyErectAsshole".Localized());

            OutputText("Scenes.Priscillascene.Priscilladom.YesMaraeRidingFace".Localized());

            OutputText("Scenes.Priscillascene.Priscilladom.GetsUpNoGlance".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Pussyfuck()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Pussyfuck.SitUpBeckon".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.InstantAllOverGlans".Localized(
                Player.CockDescript(0)
            ));

            OutputText("Scenes.Priscillascene.Pussyfuck.WetSeeForYourself".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.FallsOnSpear".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.YesFuckMePregnant".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.BouncesMoveHandClitoris".Localized(
                Player.CockDescript(0),

                Player.CockDescript(0)
            ));

            OutputText("Scenes.Priscillascene.Pussyfuck.MovingFasterForceFurther".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.AhYesBreedMe".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Priscillascene.Pussyfuck.NippleSucklingStomachInflate".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.HoldsUntilFlowStops".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.MmmmStoodUpArmor".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Priscillascene.Pussyfuck.AmazingBreederBaby".Localized());
            else
                OutputText("Scenes.Priscillascene.Pussyfuck.StaySafeMoreCream".Localized());

            OutputText("Scenes.Priscillascene.Pussyfuck.DeviousLookGulp".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void Assfuck()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Assfuck.HuffsButtFunRummages".Localized());

            OutputText("Scenes.Priscillascene.Assfuck.AllFoursSwaying".Localized());

            OutputText("Scenes.Priscillascene.Assfuck.FingersSoColdTaboo".Localized());

            OutputText("Scenes.Priscillascene.Assfuck.WaitWaitDazedGaping".Localized(
                NumberUtils.Num2Text(Player.Cocks[0].Length)
            ));

            if (Player.Cocks[0].Length > 14)
                OutputText("Scenes.Priscillascene.Assfuck.SlammingIntoMeatDelight".Localized());

            OutputText("Scenes.Priscillascene.Assfuck.LargeMoanThrobInside".Localized());

            OutputText("Scenes.Priscillascene.Assfuck.ShriekingCryBabyBatter".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Priscillascene.Assfuck.PureWhiteFlowJism".Localized());
            else
                OutputText("Scenes.Priscillascene.Assfuck.ThinTrailManMilk".Localized());

            OutputText("Scenes.Priscillascene.Assfuck.ForHerCreditConscious".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void Mouthfuck()
        {
            ClearOutput();

            OutputText("Scenes.Priscillascene.Mouthfuck.SitUpComeBeckon".Localized());

            OutputText("Scenes.Priscillascene.Mouthfuck.GraspingBallsBase".Localized());

            if (Player.Cocks[0].Length > 14)
                OutputText("Scenes.Priscillascene.Mouthfuck.SoMuchMeatMine".Localized());
            else
                OutputText("Scenes.Priscillascene.Mouthfuck.GoingToBeTreat".Localized());

            OutputText("Scenes.Priscillascene.Mouthfuck.WhisperedGraspedCockLips".Localized(
                Player.CockDescript(0),

                Player.CockDescript(0)
            ));

            OutputText("Scenes.Priscillascene.Mouthfuck.MoanLoversEyes".Localized(
                NumberUtils.Num2Text(Player.Cocks[0].Length)
            ));

            OutputText("Scenes.Priscillascene.Mouthfuck.AllMineJealous".Localized());

            OutputText("Scenes.Priscillascene.Mouthfuck.TitsJiggleSlams".Localized());

            OutputText("Scenes.Priscillascene.Mouthfuck.LostControlGivingIn".Localized());

            OutputText("Scenes.Priscillascene.Mouthfuck.HandsOnBallsSlippery".Localized());

            OutputText("Scenes.Priscillascene.Mouthfuck.HourLaterDazed".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

    }
}