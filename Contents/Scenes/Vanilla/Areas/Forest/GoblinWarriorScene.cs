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
using MonoCoC.Configuration;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<GoblinWarriorScene>("coc:goblin_warrior")]
    public class GoblinWarriorScene : Scene
    {

        public override void Entry()
        {
            GoblinWarriorEncounter();
        }

        public void GoblinWarriorEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredGoblinWarrior) == 0)
            {
                OutputText("Scenes.Goblinwarriorscene.GoblinWarriorEncounter.GoblinJumpsOut".Localized());

                OutputText("Scenes.Goblinwarriorscene.GoblinWarriorEncounter.BeatJustToFuck".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinwarriorscene.GoblinWarriorEncounter.ArmoredGoblinCatches".Localized());

                OutputText("Scenes.Goblinwarriorscene.GoblinWarriorEncounter.BeatToFuckAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredGoblinWarrior, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredGoblinWarrior) + 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryGoblins);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.GoblinWarrior), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void GobboWarriorBeatYaUp()
        {
            ClearOutput();

            if (Player.IsShota() && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))

                return ;

            int x = 0;

            if (Player.LustPercent <= 99)
                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.CollapseUtterlyBeaten".Localized());

            if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()) && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
            {
                x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

                if (x < 0)
                    x = Player.SmallestCockIndex();

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TearThroughYourArmor".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.WatchesImpassivelyFor".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ThatThing".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ThoseThings".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.LookMightyTasty".Localized(
                    Player.Mf("stud", "sexy")
                ));

                if (Player.Tallness >= 60)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.BigWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.EnoughJizzToPay".Localized(
                    Player.Mf("strapping young man", "sexy breeder")
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.DancingFingersPirouette".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.GentleCaressBefore".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TipsGivingSoakedMound".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TaintTipsGiving".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SheExplains".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TryToAnswerNo".Localized());

                if (Player.Femininity < 40)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.AlmostWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.GirlishMoanNeedThrumming".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ViridianSkinCaresses".Localized());

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TheSheathWord".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.YourBaseWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.BriefMomentVanishes".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ResponseAsImmediateAs".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.MusesEmeraldSlut".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PluralS".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.WildWithNeed".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ToesSmearPrecum".Localized(
                    Player.CockHead(x),

                    NumberUtils.Num2Text(Player.Cocks[x].Length),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ShortStackedWomanMoves".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SayStart".Localized());

                if (silly)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.StopRightThereScum".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.HoldYourHorses".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.EmeraldWarriorGlances".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.InYourBalls".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.InsideYou".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PayYourFineMight".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.HandSqueezesEncouragingly".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.HandsSqueezeEncouragingly".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.GoblinImpalesHerself".Localized());

                if (Player.CumQ() >= 750)
                {
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SupernaturalFertilityCan".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SwellingRoundedPregnant".Localized());
                }

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PassionSpendsItself".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.StandingUpWithGoo".Localized());
            }
            else
                if (Player.BiggestCockArea() > CombatController.Instance!.Context.Enemy.VaginalCapacity() && Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
                {
                    x = Player.BiggestCockIndex();

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TearThroughArmorAgain".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.WarriorSighsGrumbles".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.WatchTiradeConfused".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.EmeraldCurtainsDrape".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.BiggestCockLength() > Player.Tallness / 2)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.LongPhallusBlocks".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.NoneTooTenderSwat".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.BallsWord".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.CockWord".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.GoblinGrunts".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.HesitantlyObey".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SaysViridianTart".Localized());

                    if (Player.BiggestCockLength() < Player.Tallness / 2)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SittingOnYourFace".Localized(
                            Player.CockHead(x)
                        ));
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SittingOnGroundAbove".Localized(
                            Player.CockHead(x)
                        ));

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SoftFeetClaspDick".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.LubricatedByThe".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SpurtHugeDollops".Localized());
                    else
                        if (Player.CumQ() >= 250)
                            OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.DroolSteadyFlow".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.WontBeLongNow".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TryToGruntProtest".Localized());

                    if (Player.BiggestCockLength() >= Player.Tallness / 2)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PulsatingIronHardCock".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.CaptorsPillowyAsscheeks".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.GoblinPullsHard".Localized(
                        Player.CockHead(x),

                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() <= 10)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.DribblesWord".Localized());
                    else
                        if (Player.CumQ() <= 100)
                            OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SquirtsWord".Localized());
                        else
                            if (Player.CumQ() <= 250)
                                OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ThickWadsWord".Localized());
                            else
                                if (Player.CumQ() <= 1750)
                                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.WavesOfGooeySpooge".Localized());
                                else
                                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.RiversOfCum".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.IntoGoblinsBox".Localized());

                    if (Player.CumQ() >= 2500)
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.BellyRoundsUpNicely".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.GreenGirlGivesYour".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.DefeatedByOwnNeeds".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.MasturbatingUnabashedly".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PistonInAndOut".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TautNippleTwisting".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.ReverieInterrupted".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.LookingOnInConfusion".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.BeforeMutterProtest".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.TauntsFaceFuckingBox".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.IrritatedSlapCracks".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.MomentaryPainIn".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.SuckOnHerClit".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PantingIntoGreen".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PinprickOnTender".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.EyesRollBack".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboWarriorBeatYaUp.PassingOutInPuddle".Localized());
                }

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void GoblinWarriorRapeIntro()
        {
            ClearOutput();

            OutputText("Scenes.Goblinwarriorscene.GoblinWarriorRapeIntro.WarriorFallsToFeet".Localized());

            if (Player.Lust < 33)
                OutputText("Scenes.Goblinwarriorscene.GoblinWarriorRapeIntro.ArentHornyEnough".Localized());

            AddButton(0, "Scenes.Goblin.Button.DickFuck".Localized(), null);

            AddButton(1, "Scenes.Goblin.Button.DickTooBig".Localized(), null);

            AddButton(2, "Scenes.Goblin.Button.CorruptDick".Localized(), null);

            AddButton(3, "Scenes.Goblin.Button.DickInAss".Localized(), null);

            AddButton(4, "Scenes.Goblin.Button.JogFuck".Localized(), null);

            AddButton(5, "Common.Button.Breastfeed".Localized(), null);

            AddButton(6, "Scenes.Goblin.Button.UseCondom".Localized(), null);

            AddButton(7, "Scenes.Goblin.Button.Pussies".Localized(), null);

            AddButton(8, "Common.Button.LayEggs".Localized(), null);

            if (OptionStore.Instance.Get("gore", 0) > 0)
                AddButton(10, "Scenes.Goblin.Button.WombFuck".Localized(), null);

            if (Player.HasVagina() && Player.Lust >= 33)
                if (Player.IsChild())
                    AddButton(7, "Scenes.Goblin.Button.Pussies".Localized(), GoblinScene.GoblinLoliScene);
                else
                    AddButton(7, "Scenes.Goblin.Button.Pussies".Localized(), GobboGetsRapedFem);

            if (Player.HasCock() && Player.Lust >= 33)
            {
                if (Player.CockArea(Player.BiggestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity() && (Player.IsCorruptEnough(80) || Player.HasPerk(PerkIds.Sadist)))
                    AddButton(2, "Scenes.Goblin.Button.CorruptDick".Localized(), RapeAGoblinCorruptTooBig);

                if (Player.CockArea(Player.BiggestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
                    AddButton(1, "Scenes.Goblin.Button.DickTooBig".Localized(), ManRapesGoblinTooBig);

                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                {
                    AddButton(0, "Scenes.Goblin.Button.DickFuck".Localized(), GatsGoblinBoners);

                    AddButton(4, "Scenes.Goblin.Button.JogFuck".Localized(), GobboGetsRapedMaleFits);

                    if (OptionStore.Instance.Get("gore", 0) > 0)
                        AddButton(10, "Scenes.Goblin.Button.WombFuck".Localized(), GoblinScene.GobboWombFuck);
                }

                if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) >= 0 && (Player.IsCorruptEnough(70) || Player.HasPerk(PerkIds.Sadist)))
                    AddButton(3, "Scenes.Goblin.Button.DickInAss".Localized(), GobboButtSecks);

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.SpiderAbdomen && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                    AddButton(6, "Common.Button.WebCondom".Localized(), () => GoblinCondomed(0));
                else
                    if (Player.HasItem(ItemRegistry.Create(ItemIds.Condom)) && Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                        AddButton(6, "Scenes.Goblin.Button.UseCondom".Localized(), () => GoblinCondomed(1));

                if (OptionStore.Instance.Get("watersports", 0) > 0 && Player.CockThatFits(60) >= 0)
                    AddButton(9, "Scenes.Goblin.Button.FacefuckPiss".Localized(), GoblinScene.FacefuckPiss);
            }

            if (Player.CanOvipositSpider())
                AddButton(8, "Common.Button.LayEggs".Localized(), LaySomeDriderEggsInGobboTwat);

            if (Player.LactationQ() >= 500 || Player.HasStatusEffect(StatusEffectIds.Feeder))
                AddButton(5, "Common.Button.Breastfeed".Localized(), GiveGoblinAMilkMustache);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void GiveGoblinAMilkMustache()
        {
            ClearOutput();

            OutputText("Scenes.Goblinwarriorscene.GiveGoblinAMilkMustache.SlowlyWalkUpTo".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Goblinwarriorscene.GiveGoblinAMilkMustache.StrugglesAgainstGrip".Localized(
                Player.Mf("slut", "stud"),

                Player.NippleDescript(0)
            ));

            OutputText("Scenes.Goblinwarriorscene.GiveGoblinAMilkMustache.AfterAWhileFeel".Localized());

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

            ShowImage("goblin-win-male-buttsex");

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.EasilyDefeatAnother".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.BegsThoughDefeated".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.InsultedWhoEmerged".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.QuicklyRemovingYour".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.WithASneer".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.BegsLipsTrembling".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.SayNothingToTrembling".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.WithoutReservationsSlam".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.ProtestingSquirmsMore".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.CriesIfExpandingHer".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText(Player.SackDescript());
            else
                OutputText(Player.HipDescript());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.SmackingPlumpRump".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.TightConfinesOfGoblin".Localized());

            OutputText("Scenes.Goblinwarriorscene.GobboButtSecks.PickUpYourAnd".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");
        }

        public void GobboGetsRapedFem()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                ShowImage("goblin-win-female-taur-rapedfem");

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.PickUpGoblinHolding".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.BlushesRedFlow".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ByThirdBottleShes".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.FlipCreatureBack".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SquealsThoughDelight".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.DropHerForReal".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.CantSeeWhatShe".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.EffortsRewarded".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.CasuallyDressIgnoring".Localized());

                CombatScene.CleanupAfterCombat();

                Player.Orgasm("Vaginal");
            }
            else
                if (Player.IsNaga())
                {
                    ShowImage("goblin-win-female-naga-rapedfem");

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SlitherOverToHelpless".Localized());

                    if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.BareYourFangs".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SmileAtHerAnd".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ParagraphBreak".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.WrapYourselfTightly".Localized());

                    if (Player.Corruption > 60)
                    {
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.HissSomethingIncoherent".Localized());

                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.FingersSpreadOpen".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.AskHerIfShes".Localized());

                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.OpeningFingersOpenHer".Localized());

                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.PityForCreature".Localized());
                    }

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.EffectsDontTakeLong".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SlideYourTailUp".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SmoothMotionTail".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.NoWarningPullOut".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.PullItOutExamine".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ShruggingDumpContents".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ProvenVeryFalse".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.HearMuffledYell".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.OrgasmRepeatedly".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ThoroughlyConfused".Localized());

                    CombatScene.CleanupAfterCombat();

                    Player.Orgasm("Vaginal");
                }
                else
                {
                    ShowImage("goblin-win-female-rapedfem");

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.PickUpGoblinSitting".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.BlushesRedFlowAgain".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ByThirdBottleAgain".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Vaginas[0].Wetness < 2)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.GrowsPuffyAndMoist".Localized());
                    else
                        if (Player.Vaginas[0].Wetness < 4)
                            OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.DripsWithMoisture".Localized());
                        else
                            if (Player.Vaginas[0].Wetness < 5)
                                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SlowlySoakThighs".Localized());
                            else
                                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.DroolsWithNeed".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ArousalCanWait".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ThwackHandSmacks".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ScoldSlappingHarder".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SquealsThoughDelightAgain".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.PullHerOffLeg".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.CumOnDruggedGreen".Localized());

                    if (Player.Vaginas[0].Wetness > 4)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SplatteringWithFluids".Localized());
                    else
                        if (Player.Vaginas[0].Wetness > 3)
                            OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.CoatingFaceWithProof".Localized());
                        else
                            if (Player.Vaginas[0].Wetness > 2)
                                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.LeavingStickyWith".Localized());
                            else
                                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.LeavingTasteOfPussy".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SentencePeriodSpace".Localized());

                    if (Player.BiggestLactation() >= 3.5f)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.MilkExplodesFromNipples".Localized());
                    else
                        if (Player.BiggestLactation() > 2)
                            OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.TwinStreamsOfMilk".Localized());
                        else
                            if (Player.BiggestLactation() >= 1)
                                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.MilkDribblesFalling".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.StrangeDopeyFace".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.CasuallyDressIgnoringAgain".Localized());

                    if (Player.Corruption > 50)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SmirkWord".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.SighWord".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.AndSaunterOffFeeling".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.BitGuiltyOverdoing".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedFem.ThoroughlySatisfied".Localized());

                    CombatScene.CleanupAfterCombat();

                    Player.Orgasm("Vaginal");
                }
        }

        public void RapeAGoblinCorruptTooBig()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            ShowImage("goblin-win-male-corruptedfuck");

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.BeginToRemoveYour".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.IsWord".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.AreAllWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.FarMoreAware".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.CockComesToRest".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.YourComesToRest".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.Cocks[0].Length < 20)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.LongAsHerTorso".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.BiggerThanSheIs".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.GivesWickedIdea".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SeatLegsWide".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                Player.CockDescript(x),

                CombatController.Instance!.Context.Enemy.VaginaDescript(),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.BothDrippingWithSweat".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.GoblinRelaxesALittle".Localized(
                Player.CockDescript(x),

                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.RepositionYourFucktoy".Localized(
                Player.CockDescript(x),

                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SaysWeaklyTipOf".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.PleadsRestOfThe".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.BegsShaftStartsTo".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.MoreEntersSmallBody".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SheFillsToCapacity".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.AndBeyond".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.FuckMeSay".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.RamHomeTheRest".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[0].Length < 20)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.FeelMassiveBulge".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.StretchHerSoMuch".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.DrivesYourLustEven".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript()
            ));

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.HeadRestingBetween".Localized());

                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.AsYourNippleWord".Localized());

                    if (Player.BiggestLactation() < 2)
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.DripMilkyTears".Localized());

                    if (Player.BiggestLactation() < 3)
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.WeepStreamsOfMilk".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.PlasterWithGouts".Localized());
                }
            }

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.HairIsMattedWith".Localized());

            if (Player.BiggestLactation() >= 1)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.MilkAndWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SweatOfBoth".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.YouYellHowever".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.WhileYourVaginaWord".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Wetness <= 3)
                    OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.JuicesWord".Localized());
                else
                    if (Player.Vaginas[0].Wetness <= 4)
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.FloodsWord".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.ExplodesWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.RestOfYourDrench".Localized());
            }
            else
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.WhileYourOtherCock".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.DrenchHerWord".Localized());
                    else
                        OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.DrenchesHerWord".Localized());
                }

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.BitchTooTight".Localized());

            if (Player.Balls >= 2)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.AreTryingButH".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.HLetter".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.ErSpasmingClamping".Localized(
                CombatController.Instance!.Context.Enemy.VaginaDescript(),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.MomentPasses".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SeemsLikeHours".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.ShoutPullingTwo".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.PressedSoFirmly".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.RemainingBlastGeysers".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.RemainingPenisBlasts".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SameTightnessPrevented".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.FinallyBlowLastWad".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.StandPrepareLeave".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.YouSay".Localized());

            OutputText("Scenes.Goblinwarriorscene.RapeAGoblinCorruptTooBig.SheAbsolutelyWill".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void ManRapesGoblinTooBig()
        {
            ClearOutput();

            ShowImage("goblin-win-male-corruptedbj");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.WhipOutStiffening".Localized());

            if (Player.CockArea(x) < 80)
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.NearlyWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.AbsurdSizeToVictim".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.YouWord".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.ShrugAndGuess".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.GrinHappyToTry".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.TheWord".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.FlareWord".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.HeadWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.VisiblyPulsesIn".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.FlareWordAgain".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.CrownWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.AfterMomentOrTwo".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.DisgruntledTeasePull".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasKnot(0))
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.HookingTheHeels".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.MuseAsSheGrinds".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.HermWord".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.ManWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.ThatsForSureStart".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.YoudNeverThink".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 75)
                OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.SplatteringFewMore".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.PaintingThickLayer".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.SoakingAndLeaving".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.GreenSlutSeemsHandle".Localized());

            OutputText("Scenes.Goblinwarriorscene.ManRapesGoblinTooBig.ShakeHeadAndLeave".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void GobboGetsRapedMaleFits()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.BiggestCockIndex();

            ShowImage("goblin-win-male-getridden");

            if (Player.CockArea(x) > CombatController.Instance!.Context.Enemy.VaginalCapacity() * .8f)
            {
                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PickUpDefeatedGoblin".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.StartWalkingMovements".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.RubbingPainfully".Localized());
                else
                    if (Player.HasKnot(0))
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.StretchingPainfully".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                            OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.FeelingPainfullyTight".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.EveryStepShe".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText(Appearance.AllBreastsDescript(Player));
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.TorsoWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SupportHerselfSteady".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PantsHappilyTongue".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.AllTheEncouragement".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.TitsWord".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SkinWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.HangsOnClenching".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SheMoans".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.FamiliarTightness".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.YourTightenReleasing".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.HowlOfPowerfulOrgasm".Localized());

                if (Player.CumQ() >= 100)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.EnhancedBodyStuffs".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.OrgasmSoPotent".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if (Player.CumQ() >= 250)
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.OrgasmSoPotentAgain".Localized(
                            Player.CockDescript(x)
                        ));

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.ParagraphBreak".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PantAndStopPulling".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PoursOutInRiver".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.LeaksWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.FromGapingTwatShe".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.ChucklingWord".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.ShudderingWord".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.MakeYourWayBack".Localized());
            }
            else
            {
                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PickUpDefeatedAgain".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.StartWalkingMovementsAgain".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.RubbingPerfectly".Localized());
                else
                    if (Player.HasKnot(0))
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.StretchingTightly".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse || Player.Cocks[0].TypeIndex > 3)
                            OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.FeelingAbsolutelyPerfect".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.EveryStepYou".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText(Appearance.AllBreastsDescript(Player));
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.TorsoWordAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SupportHerselfSteadyAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PantsHappilyTongueAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.AllTheEncouragementAgain".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.TitsWordAgain".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SkinWordAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.HangsOnClenchingAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SheMoansAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.FamiliarTightnessAgain".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.YourTightenReleasingAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.HowlOfOrgasmAgain".Localized());

                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.EnhancedBodyStuffsAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText(Player.BallsDescriptLight());
                    else
                        OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.LegsWord".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.SentencePeriod".Localized());
                }

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.OrgasmSoPotentThird".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PantStopPullingAgain".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.PoursOutRiverAgain".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.LeaksWordAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.FromGapingTwatAgain".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.ChucklingWordAgain".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.ShudderingWordAgain".Localized());

                OutputText("Scenes.Goblinwarriorscene.GobboGetsRapedMaleFits.MakeWayBackAgain".Localized());
            }

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Generic");
        }

        public void GoblinCondomed(float mode = 0)
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            ShowImage("goblin-win-male-goblincondomed");

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.DefeatedGoblinGirlsKnees".Localized());

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.ImpatientlyThrustsLegs".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.YourThrobbingSack".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.BaseOfYourShaft".Localized());

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.DigitsCurlAround".Localized());

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.WideningLakeOf".Localized());

            if (mode == 0)
                OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.WorkYourSpinnerets".Localized(
                    Player.CockDescript(x)
                ));
            else
            {
                Player.DestroyItems(ItemIds.Condom, 1);

                OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.TearOpenPacket".Localized(
                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.TimeToGiveGoblin".Localized());

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.HoldingWrigglingFeet".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.MouthyBitchForgot".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.SquirmingRightSideUp".Localized());

            if (mode == 0)
                OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.InformHerSpider".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.GoblinCondomed.InformHerLatex".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void GatsGoblinBoners()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("goblin-win-male-goblinboners");

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.GoblinLiesStrewn".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.LuckilyHaveEvery".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.BeginToFondleCock".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PluralS".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.WalkTowardsUnsuspecting".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PluralSAgain".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.YelpsMoreSurprised".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PluralSThird".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.AndSlapWord".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.ItWord".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.ThemWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.AgainstBareFlesh".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.DickInsidePanting".Localized());
            else
                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.BothDicksInside".Localized());
                else
                {
                    OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.TwoDicksInside".Localized());

                    if (Player.CockTotal() >= 4)
                        OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PluralSFourth".Localized());

                    OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.RemainingSandwiched".Localized());
                }

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PeriodParagraphBreakAgain".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.RoughlyPound".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.EventuallyBeginToFeel".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PluralSFifth".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.SlowlyReleaseYourself".Localized());

            OutputText("Scenes.Goblinwarriorscene.GatsGoblinBoners.PickYourselfBackUp".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LaySomeDriderEggsInGobboTwat()
        {
            ClearOutput();

            ShowImage("goblin-win-drider-egged");

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.PitifulStruggling".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.HardenWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.MoistenWord".Localized());
                else
                    OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.TwitchWord".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.SlightlyCantHelp".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.UndressAndSkitterOver".Localized());

            if (Player.Femininity > 50)
                OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.HaventGotAnyBitch".Localized());
            else
                OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.HaventGotAnyBastard".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.LashesOutWithFeet".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.SquirmsAndYouSlip".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.PlaythingFallenSilent".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.NervousToneNothing".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.StifledGaspsEscape".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.FinallyBottomOut".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.FeelItsSlowPath".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.OwnOrgasmJustAs".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.AndYFragment".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.YFragment".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.OurClenchingInTime".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.NextEggRollsForward".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.KeepHerPinned".Localized());

            OutputText("Scenes.Goblinwarriorscene.LaySomeDriderEggsInGobboTwat.LayingHerInShade".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

    }
}