using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<ValeriaScene>("coc:valeria")]
    public class ValeriaScene : Scene
    {

        public const int GOOSTUFFED_EFFECT_MIN_DURATION = 10;

        public const int GOOSTUFFED_EFFECT_MAX_DURATION_DELTA = 300;

        public const int GOOSTUFFED_PREGNANCY_DURATION = int.MaxValue;

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Valeria);

        public override void Entry()
        {
            ValeriaFluidsEnabled();
        }

        public bool ValeriaFluidsEnabled()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel) != 0 || OptionStore.Instance.Get("hardcore", 0) > 0 || realistic;
        }

        public bool ValeriaFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1 || Player.Armor.Id == ArmorIds.GooArmor;
        }

        public void ValeriaFollowerMenu()
        {
            ClearOutput();

            if (Player.ArmorName == "goo armor")
            {
                OutputText("Scenes.Valeria.ValeriaFollowerMenu.TellValeriaYoud".Localized());

                OutputText("Scenes.Valeria.ValeriaFollowerMenu.SurePartnerSoundsNice".Localized());

                OutputText("Scenes.Valeria.ValeriaFollowerMenu.GoogirlSlidesHerselfOff".Localized());

                OutputText("Scenes.Valeria.ValeriaFollowerMenu.WantAnything".Localized());
            }
            else
            {
                OutputText("Scenes.Valeria.ValeriaFollowerMenu.WalkOverToValeria".Localized());

                OutputText("Scenes.Valeria.ValeriaFollowerMenu.HeyTherePartner".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), ValeriaAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), TalkWithValeria);

            AddButton(2, "Common.Button.Sex".Localized(), () => FollowersValeriaSex());

            AddButton(3, "Common.Button.Spar".Localized(), ValeriaSpar);

            if (Player.ArmorName != "goo armor")
                AddButton(5, "Common.Button.Take".Localized(), TakeValeria);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void ValeriaCampLines()
        {

            if (Player.Armor.Id == ArmorIds.GooArmor)
                return ;

            if (View.Time.Hours < 21)
                OutputText("Scenes.Valeria.ValeriaCampLines.GooLazingAboutCamp".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaCampLines.AMoundOfSapphire".Localized(
                    RandomUtils.Choice(["A mound of sapphire-blue goo sits calmly in your camp.", "There is a mass of deep blue slime on the ground. You identify it to be Valeria, sleeping."])
                ));

            OutputText("Scenes.Valeria.ValeriaCampLines.ParagraphBreak".Localized());
        }

        public void FeedValeria(float amnt)
        {

            if (ValeriaFollower())
            {
                CoC.Instance.Flags.Set(GameFlagIds.ValeriaFluids, CoC.Instance.Flags.Get(GameFlagIds.ValeriaFluids) + (int)Math.Round((double)(amnt)));

                if (CoC.Instance.Flags.Get(GameFlagIds.ValeriaFluids) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.ValeriaFluids, 100);
            }
        }

        public void ValeriaAppearance()
        {
            ClearOutput();

            ShowImage("monster-gooarmor");

            OutputText("Scenes.Valeria.ValeriaAppearance.SixFootTallGirl".Localized());

            OutputText("Scenes.Valeria.ValeriaAppearance.PairCCupBreasts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.VelariaFuta) == 1)
                OutputText("Scenes.Valeria.ValeriaAppearance.WhenYouDesireIt".Localized());

            OutputText("Scenes.Valeria.ValeriaAppearance.EasilyStretchableCunt".Localized());

            OutputText("Scenes.Valeria.ValeriaAppearance.DoesntBotherAnus".Localized());

            DoNext(() => ValeriaFollowerMenu());
        }

        public void ValeriaSpar()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.ValeriaSpar.AskIfSheWould".Localized());

            OutputText("Scenes.Valeria.ValeriaSpar.ThatdBeGreatPartner".Localized());

            OutputText("Scenes.Valeria.ValeriaSpar.TakeValeriaOutTo".Localized());

            var enemy = EnemyRegistry.Create(new Identifier("coc", "goo_armor"));

            enemy.OnDefeated = _ => PcWinsValeriaSpar();

            enemy.OnWon = _ => PcWinsValeriaSparDefeat();

            CombatScene.Start(enemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CurrentMonster.CreateStatusEffect(StatusEffectIds.Spar, 0, 0, 0, 0);

            CombatController.Instance!.Context.Enemy.Gems = 0;
        }

        public void PcWinsValeriaSpar()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.PcWinsValeriaSpar.GruntsNearlyLosing".Localized());

            OutputText("Scenes.Valeria.PcWinsValeriaSpar.OfferHelpHerUp".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void PcWinsValeriaSparDefeat(bool offCamp = false)
        {
            ClearOutput();

            if (!offCamp)
            {
                OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.YouCollapse".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.InTooMuchPain".Localized());
                else
                    OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.TooTurnedOn".Localized());

                OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.ToFightAnyLonger".Localized());
            }

            OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.AsksLikeAKnight".Localized());

            OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.NodEmphatically".Localized());

            OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.SighsShakingHerHead".Localized());

            if ((Player.Health < 1 || Player.Gender == Gender.Genderless) && !offCamp)
            {
                OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.OfferingHandUp".Localized());

                OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.CmonLetsGetBack".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Valeria.PcWinsValeriaSparDefeat.SinceYoureSoEager".Localized());

                DoNext(() => ValeriaSexDominated());
            }
        }

        public void FollowersValeriaSex(bool display = true)
        {
            ClearOutput();

            if (display)
            {
                OutputText("Scenes.Valeria.FollowersValeriaSex.LustyGrinYouAsk".Localized());

                if ((int)Player.Gender > 0)
                    OutputText("Scenes.Valeria.FollowersValeriaSex.ShesInterestedALittle".Localized());
                else
                    OutputText("Scenes.Valeria.FollowersValeriaSex.WouldntMindHelpingYou".Localized());

                OutputText("Scenes.Valeria.FollowersValeriaSex.SentencePeriod".Localized());

                OutputText("Scenes.Valeria.FollowersValeriaSex.CouldBeArranged".Localized());
            }

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.Penetrateher".Localized(), PenetrateValeria);

            AddButton(1, "Common.Button.GetFucked".Localized(), ValeriaGetFucked);

            AddButton(2, "Common.Button.Gooflation".Localized(), () => GooFlation());

            if ((int)Player.Gender > 0)
                AddButton(3, "Common.Button.Getdominated".Localized(), () => ValeriaSexDominated());

            if (CoC.Instance.Flags.Get(GameFlagIds.VelariaFuta) == 1)
                AddButton(4, "Common.Button.LoseDick".Localized(), ValeriaDickToggle);
            else
                AddButton(4, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGrowDick".Localized(), ValeriaDickToggle);

            AddButton(14, "Common.Button.Back".Localized(), ValeriaFollowerMenu);
        }

        public void ValeriaDickToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.VelariaFuta) == 1)
            {
                OutputText("Scenes.Valeria.ValeriaDickToggle.BeforeAnythingYouAsk".Localized());

                OutputText("Scenes.Valeria.ValeriaDickToggle.SheGroans".Localized());

                OutputText("Scenes.Valeria.ValeriaDickToggle.GratifiedToSeeHer".Localized());

                OutputText("Scenes.Valeria.ValeriaDickToggle.AllBetterNowPartner".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.VelariaFuta, 0);
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.VelariaFuta, 1);

                OutputText("Scenes.Valeria.ValeriaDickToggle.StrugglingToFind".Localized());

                if (!Player.HasCock() || (Player.Gender == Gender.Hermaphrodite && Player.BreastRows.Average(r => r.BreastRating) >= 2))
                {
                    OutputText("Scenes.Valeria.ValeriaDickToggle.SheCocksAnEyebrow".Localized());

                    OutputText("Scenes.Valeria.ValeriaDickToggle.WatchAGrunt".Localized());

                    OutputText("Scenes.Valeria.ValeriaDickToggle.GoingToBeDifferent".Localized());
                }
                else
                {
                    OutputText("Scenes.Valeria.ValeriaDickToggle.ValeriaCocksEyebrow".Localized());

                    OutputText("Scenes.Valeria.ValeriaDickToggle.ScowlAtHer".Localized());

                    OutputText("Scenes.Valeria.ValeriaDickToggle.GiantPileOfGoop".Localized());

                    OutputText("Scenes.Valeria.ValeriaDickToggle.WatchGruntAgain".Localized());

                    OutputText("Scenes.Valeria.ValeriaDickToggle.GoingDifferentAgain".Localized());
                }
            }

            DoNext(() => ValeriaFollowerMenu());
        }

        public void ValeriaGetFucked()
        {
            ClearOutput();

            ShowImage("valeria-sex-getfucked");

            OutputText("Scenes.Valeria.ValeriaGetFucked.YouDisrobeAnd".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Valeria.ValeriaGetFucked.StrokingSlickLips".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGetFucked.PosingSeductivelyAsk".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.GrinsDoAsShe".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Valeria.ValeriaGetFucked.AndSpreadingLegs".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.AsValeria".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.VelariaFuta) == 0)
                OutputText("Scenes.Valeria.ValeriaGetFucked.FormsThickGooeyCock".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGetFucked.GrabsThickGooeyCock".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.LoomsOverYou".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.KneelsOverShoulders".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.StartToRemindHer".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.LetsOutLongLow".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.TipGoopyCock".Localized());

            if (Player.BiggestBreastSize() > 3)
                OutputText("Scenes.Valeria.ValeriaGetFucked.BumpingOverYourAnd".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.ComingToStopOver".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valeria.ValeriaGetFucked.GooeyAssDevouring".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Valeria.ValeriaGetFucked.AndWord".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Valeria.ValeriaGetFucked.LettingBitsGooBody".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Valeria.ValeriaGetFucked.PokingShaftOfCock".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.SentencePeriod".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.RocksBackYourLap".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valeria.ValeriaGetFucked.RhythmicallyFlexesAss".Localized());

            OutputText("Scenes.Valeria.ValeriaGetFucked.SentencePeriodAgain".Localized());

            if (!Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Valeria.ValeriaGetFucked.GivesLittleGrin".Localized());

                OutputText("Scenes.Valeria.ValeriaGetFucked.SitsInYourLap".Localized());

                OutputText("Scenes.Valeria.ValeriaGetFucked.GrinningWickedlyShifts".Localized());

                if (!Player.HasFuckableNipples())
                    OutputText("Scenes.Valeria.ValeriaGetFucked.SwirlsHerHeels".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGetFucked.GooGirlStillGrinning".Localized());

                Player.CuntChange(10, true, true, false);

                OutputText("Scenes.Valeria.ValeriaGetFucked.UnderTripleAttacks".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Valeria.ValeriaGetFucked.PluralS".Localized());

                OutputText("Scenes.Valeria.ValeriaGetFucked.DeeperInsideThanEver".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Valeria.ValeriaGetFucked.AndChestWord".Localized());

                OutputText("Scenes.Valeria.ValeriaGetFucked.SlimeOverflowsInside".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Valeria.ValeriaGetFucked.PluralSAgain".Localized());

                OutputText("Scenes.Valeria.ValeriaGetFucked.LiterallyPopInside".Localized());
            }
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Valeria.ValeriaGetFucked.CockStuffedIntoValeria".Localized());

                    OutputText("Scenes.Valeria.ValeriaGetFucked.ResignedLetGooGirl".Localized());

                    OutputText("Scenes.Valeria.ValeriaGetFucked.JustAsYoureGetting".Localized());

                    if (Player.HasVagina())
                    {
                        Player.CuntChange(10, true, true, false);

                        OutputText("Scenes.Valeria.ValeriaGetFucked.DoubleAttacksPleasure".Localized());
                    }

                    OutputText("Scenes.Valeria.ValeriaGetFucked.SentencePeriodThird".Localized());

                    Player.ButtChange(10, true, true, false);

                    OutputText("Scenes.Valeria.ValeriaGetFucked.BeginsBuckHerHips".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Valeria.ValeriaGetFucked.PluralSThird".Localized());

                    OutputText("Scenes.Valeria.ValeriaGetFucked.FuckingYourWithOne".Localized());

                    OutputText("Scenes.Valeria.ValeriaGetFucked.CumScreamingAsUnload".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Valeria.ValeriaGetFucked.PluralSFourth".Localized());

                    OutputText("Scenes.Valeria.ValeriaGetFucked.PoppingAsSheClimaxes".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Valeria.ValeriaGetFucked.AndVagWord".Localized());

                    OutputText("Scenes.Valeria.ValeriaGetFucked.WithWarmStickyGoop".Localized());
                }
                else
                    GooFlation(false);

            OutputText("Scenes.Valeria.ValeriaGetFucked.CollapseGoopFlowing".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Valeria.ValeriaGetFucked.PattingBellyFullOf".Localized());

                FeedValeria((float)Math.Sqrt(Player.CumQ()) + 5 + (Player.AverageVaginalWetness() * 5));
            }

            OutputText("Scenes.Valeria.ValeriaGetFucked.LoomsOverSheSays".Localized());

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.HealthChange(25 + (Player.NewGamePlusMod() * 15));

            DoNext(() => ReturnToCamp(1));
        }

        public void GooFlation(bool clearText = true)
        {
            ClearOutput();

            if (clearText)
            {
                OutputText("Scenes.Valeria.GooFlation.GrinsReachingUp".Localized());

                OutputText("Scenes.Valeria.GooFlation.BiteLipAsYou".Localized());
            }
            else
                OutputText("Scenes.Valeria.GooFlation.WigglesGooeyAss".Localized());

            ShowImage("valeria-sex-gooflation");

            OutputText("Scenes.Valeria.GooFlation.StillCurledUp".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Valeria.GooFlation.OneLonelyWord".Localized());

            OutputText("Scenes.Valeria.GooFlation.HoleYouGulp".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Valeria.GooFlation.GruntAndGasp".Localized());

            OutputText("Scenes.Valeria.GooFlation.GutBecomesConcave".Localized());

            Player.RefillHunger(50);

            OutputText("Scenes.Valeria.GooFlation.ShoutPokingStomach".Localized());

            OutputText("Scenes.Valeria.GooFlation.HearLaughFromInside".Localized());

            if (clearText)
            {
                OutputText("Scenes.Valeria.GooFlation.CollapseGoopFlowing".Localized());

                if ((int)Player.Gender > 0)
                    OutputText("Scenes.Valeria.GooFlation.PattingBellyFullOf".Localized());

                OutputText("Scenes.Valeria.GooFlation.LoomsOverSheSays".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

                Player.HealthChange(25 + (Player.NewGamePlusMod() * 15));

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void PenetrateValeria()
        {
            ClearOutput();

            ShowImage("valeria-sex-penetration");

            OutputText("Scenes.Valeria.PenetrateValeria.SaysAsYouDisrobe".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.GaspGooeySkin".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.BeforeTooComfortable".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.SinkFingersIntoGoo".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.SeemsToSenseComing".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.LaughsSpreadingLegs".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.EagerlyObligeHer".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.LaughsCryOut".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.TryToStruggleInside".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.FeelGooConstricting".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.SuddenSurgeOfStrength".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.RelievedExhaustedSigh".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.LaughsSheAdds".Localized());

            OutputText("Scenes.Valeria.PenetrateValeria.RunHandAlongHer".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            Player.HealthChange(25 + (Player.NewGamePlusMod() * 15));

            FeedValeria((float)Math.Sqrt(Player.CumQ()) + 5);

            DoNext(() => ReturnToCamp(1));
        }

        public void ValeriaSexDominated(bool offCamp = false)
        {
            ClearOutput();

            ShowImage("valeria-sex-domination");

            OutputText("Scenes.Valeria.ValeriaSexDominated.MakingShowOfHesitant".Localized(
                Player.Mf("handsome", "cutie")
            ));

            OutputText("Scenes.Valeria.ValeriaSexDominated.SexualSwingOfHer".Localized());

            OutputText("Scenes.Valeria.ValeriaSexDominated.LieBackAndSubmit".Localized());

            if (Player.HasVagina() && (!Player.HasCock() || Random.Shared.Next(2) == 0))
            {
                OutputText("Scenes.Valeria.ValeriaSexDominated.BeginsUseHerGoo".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.HaveOnlyAMoment".Localized());

                Player.CuntChange(Player.VaginalCapacity(), true, true, false);

                OutputText("Scenes.Valeria.ValeriaSexDominated.ArmorGooAsksEvilly".Localized());

                if (Player.BiggestBreastSize() > 1)
                {
                    OutputText("Scenes.Valeria.ValeriaSexDominated.HammersYourCunny".Localized());

                    if (Player.LactationQ() > 0)
                        OutputText("Scenes.Valeria.ValeriaSexDominated.ToHerDelightSpray".Localized());
                }

                OutputText("Scenes.Valeria.ValeriaSexDominated.ContinuesToPoundCunt".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.Valeria.ValeriaSexDominated.BigWord".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaSexDominated.LittleWord".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.ReadyToCumWithdraws".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.GulpNervously".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.CanOnlyGritTeeth".Localized());

                if (Player.VaginalCapacity() > 80 || Player.AnalCapacity() > 80)
                    OutputText("Scenes.Valeria.ValeriaSexDominated.AndYourReceptibility".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.HuggingYourBack".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.DemandsCocksUtterly".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.WhimperedMoan".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.ValeriaGrunts".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.CumCommandFem".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.SaysPattingFullBelly".Localized());

                if (Player.LactationQ() > 0)
                    OutputText("Scenes.Valeria.ValeriaSexDominated.AndMilkWord".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.SwirlingInsideSheAdds".Localized());
            }
            else
            {
                OutputText("Scenes.Valeria.ValeriaSexDominated.BeginsUseGooAgain".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.GaspWithPleasure".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.HelplessCanOnlySubmit".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.Valeria.ValeriaSexDominated.BigWordAgain".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaSexDominated.LittleWordAgain".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.ReadyWellGoOn".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.CumRopesThickWhite".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.BeforeThinkClearly".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.TastyTreatGetAway".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.EyesGoWide".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.GooGirlRidesHard".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.CooesLoomingOverYou".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.PleadWithValeriaTo".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.CommandsUtterlyStill".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.BegYouWant".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.ThrowsHerHeadBack".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.GoodLetAllOut".Localized());

                OutputText("Scenes.Valeria.ValeriaSexDominated.SaysPattingBellyAgain".Localized());
            }

            Player.HealthChange(25 + (Player.NewGamePlusMod() * 15));

            FeedValeria((float)Math.Sqrt(Player.CumQ()) + 5 + (Player.AverageVaginalWetness() * 5));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (offCamp || !CombatController.InCombat)
                DoNext(() => ReturnToCamp(1));
            else
                CombatScene.CleanupAfterCombat();
        }

        public void TalkWithValeria()
        {
            ClearOutput();

            ShowImage("valeria-talk");

            OutputText("Scenes.Valeria.TalkWithValeria.AskIfSheWouldnt".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.TeasesSittingBeside".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.RollYourEyesAfter".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.LaughsGirlishlyWaving".Localized());

            if (Player.Race != "human")
                OutputText("Scenes.Valeria.TalkWithValeria.WereWord".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.IJustIDunno".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.ChangedQuestion".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.SaysQuietlyLookingAway".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.QuicklyRemark".Localized());

            OutputText("Scenes.Valeria.TalkWithValeria.ChucklesWrylyWhispers".Localized());

            ClearButtons();

            if ((int)Player.Gender > 0)
                AddButton(0, "Common.Button.Flirt".Localized(), FlirtWithValeria);

            AddButton(1, "Common.Button.Accept".Localized(), AcceptValeriasNeeds);

            AddButton(2, "Common.Button.Gross".Localized(), DeclineValeriasNeeds);
        }

        public void FlirtWithValeria()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.FlirtWithValeria.SlipAnArmAround".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valeria.FlirtWithValeria.BrushingAlongLength".Localized());
            else
                OutputText("Scenes.Valeria.FlirtWithValeria.BrushingWetWarmKnuckle".Localized());

            OutputText("Scenes.Valeria.FlirtWithValeria.SentencePeriod".Localized());

            OutputText("Scenes.Valeria.FlirtWithValeria.MurmursIntoYourNeck".Localized());

            FollowersValeriaSex(false);
        }

        public void AcceptValeriasNeeds()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.AcceptValeriasNeeds.SmileAndTellValeria".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.SlimeCraving))
                OutputText("Scenes.Valeria.AcceptValeriasNeeds.HaveTheSameOnes".Localized());

            OutputText("Scenes.Valeria.AcceptValeriasNeeds.AsLongTheyDont".Localized());

            OutputText("Scenes.Valeria.AcceptValeriasNeeds.SaysLittleWink".Localized());

            OutputText("Scenes.Valeria.AcceptValeriasNeeds.RuffleHerGooeyHair".Localized());

            DoNext(() => ValeriaFollowerMenu());
        }

        public void DeclineValeriasNeeds()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.DeclineValeriasNeeds.GrimaceAndPushGoo".Localized());

            OutputText("Scenes.Valeria.DeclineValeriasNeeds.SaysWithAHuff".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeValeria()
        {
            ClearOutput();

            if (Player.Armor.Id == ArmorIds.VineArmorBehavior)
            {
                OutputText("Scenes.Valeria.TakeValeria.TheSapphireOozeSays".Localized());

                OutputText("Scenes.Valeria.TakeValeria.AllCanReallyDo".Localized());

                DoNext(PlayerMenu);

                return ;
            }

            ShowImage("valeria-take");

            var item = Player.SetArmor((Armor)ItemRegistry.Create(ArmorIds.GooArmor));

            if (item == null)
                DoNext(PlayerMenu);
            else
                InventoryScene.TakeItem(item, () => Go<CampScene>());
        }

        public void ValeriaAndGooThreeStuff()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.CautiouslyApproachWith".Localized());

            var list = new List<string> { "[hips]" };

            if (Player.Balls > 0)
                list.Add("[balls]");

            if (Player.Cocks.Count > 0)
                list.Add("[multiCockDescriptLight]");

            if (Player.HasVagina())
                list.Add("[vagina]");

            list.Add("[asshole]");

            list.Add("[nipples]");

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.LiquidSoftCaresses".Localized(
                string.Join(", ", list)
            ));

            if (Player.HasCock())
            {
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SighAsStandsFull".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.PluralS".Localized());

                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.BarelyThickerCondom".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.EsSuffix".Localized());

                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.AroundWord".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.ItAsItDistends".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.ThemAsTheyDistend".Localized());

                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.IncreasinglyLiquescent".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.PluralSAgain".Localized());

                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SapphireSlimeFalls".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.PluralSThird".Localized());

                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SentencePeriod".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SighYourAsTenderly".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SighAsYourTenderly".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.GaitTurnsStuttery".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) == 0)
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.FuckHungryPenetrator".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.JustLikeLastTime".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SurprisesContorting".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SplashIntoCushiony".Localized(
                ((GooGirlEnemy)(CurrentMonster)).GooColor(),

                ((GooGirlEnemy)(CurrentMonster)).GooColor4()
            ));

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.NoMatterYouTry".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.TheBluishWord".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.JuicesSlowlyMeld".Localized());

            if (Player.HasCock() || Player.HasVagina())
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.GenitalsWord".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.AssholeWord".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.SinfulTightness".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.DistortionBecomesVisible".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.YouWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) == 0)
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.DemandSheReleaseYou".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.PlayfullyComment".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.TroublesomeArmorShakes".Localized());

            OutputText("Scenes.Valeria.ValeriaAndGooThreeStuff.GonnaGiveMySister".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ValeriaGooRapeII);
        }

        public void ValeriaGooRapeII()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.ValeriaGooRapeII.LumpThatBumpedYou".Localized());

            if (Player.BiggestBreastSize() >= 3)
                OutputText("Scenes.Valeria.ValeriaGooRapeII.ValleyOfYourCleavage".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGooRapeII.YourChestWord".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.OnceThisTime".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.GetYourAnswerIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) == 0)
                OutputText("Scenes.Valeria.ValeriaGooRapeII.UnexpectedWord".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGooRapeII.ButExpectedWord".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.PressureAtYour".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) == 0)
                OutputText("Scenes.Valeria.ValeriaGooRapeII.HoldTightAnyway".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGooRapeII.HoldTightKeepingUp".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.SpinningFaster".Localized());

            Player.ButtChange(Player.AnalCapacity() * .75f, true, true, false);

            if (!Player.IsPregnant() && Player.HasVagina())
            {
                if (silly)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.BillyMaysAnnounces".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.ShortlyAfterBlob".Localized());

                Player.CuntChange(Player.VaginalCapacity() * .75f, true, true, false);

                OutputText("Scenes.Valeria.ValeriaGooRapeII.CeruleanShaftBatters".Localized());
            }

            if (Player.HasFuckableNipples() && Player.BiggestBreastSize() >= 5)
            {
                OutputText("Scenes.Valeria.ValeriaGooRapeII.RoughlySqueezedGroped".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.AsYourSnatch".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.AsTheRandiestSnatch".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.TakesThisAsInvitation".Localized(
                    NumberUtils.Num2Text(Player.TotalNipples())
                ));
            }

            if (Player.HasCock())
            {
                OutputText("Scenes.Valeria.ValeriaGooRapeII.VoiceCallsOutOnce".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.ThisLittleGuy".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.TheseLittleGuys".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.DidYou".Localized());

                if (Player.BiggestCockArea() >= 50)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.MaybeBigGuyBit".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.SavingTheBest".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.SleeveOfSquishyEcstasy".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralS".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.InPlaceSomethingWarmer".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSAgain".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.PushesAtYourHead".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSThird".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.CirclingYourUrethra".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSFourth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.InadvertentlyClench".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSFifth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.FeelWarmWetFluid".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSSixth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.ToYourMiddle".Localized());
            }

            OutputText("Scenes.Valeria.ValeriaGooRapeII.GigglingSapphireSlut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) > 0)
                OutputText("Scenes.Valeria.ValeriaGooRapeII.GrinInEagerAnticipation".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGooRapeII.AtFirstDontRealize".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.PumpingIntoYourBowels".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Valeria.ValeriaGooRapeII.YeGodsIsGood".Localized());

                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.LeakingWord".Localized());
                else
                    if (Player.Wetness() <= 3)
                        OutputText("Scenes.Valeria.ValeriaGooRapeII.DrippingWord".Localized());
                    else
                        OutputText("Scenes.Valeria.ValeriaGooRapeII.SquirtingWord".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.GirlcumInFeverish".Localized());
            }

            if (Player.HasFuckableNipples() && Player.BiggestBreastSize() >= 5)
            {
                OutputText("Scenes.Valeria.ValeriaGooRapeII.BreastsExperienceSimilar".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.TopRowOfBreasts".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.BreastsWord".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.PullingAwayGaining".Localized());
            }

            if (Player.HasCock())
            {
                OutputText("Scenes.Valeria.ValeriaGooRapeII.SealedCockPluggingTube".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSSeventh".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.InEachCock".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.DoNotWord".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.DoesNotWord".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.DifferFromOther".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.TheyStartWord".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.ItStartsWord".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.ThickeningYourCumvein".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSEighth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.AsWideAsWord".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.TheyWord".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.ItWord".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.GoWithoutPain".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSNinth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSTenth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.TheSleeveWord".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSEleventh".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.SqueezeWord".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSTwelfth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.DownOnYourLength".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSThirteenth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.HarderSlideAround".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.PluralSFourteenth".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.SpurtingBlissThatCant".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.SomethingExpanding".Localized());
                else
                    OutputText("Scenes.Valeria.ValeriaGooRapeII.YourExpandingYouCum".Localized());

                OutputText("Scenes.Valeria.ValeriaGooRapeII.SentencePeriod".Localized());
            }

            OutputText("Scenes.Valeria.ValeriaGooRapeII.WhimperingInEcstasy".Localized());

            if (Player.BiggestBreastSize() >= 5 && Player.HasFuckableNipples())
                OutputText("Scenes.Valeria.ValeriaGooRapeII.PausingHeftySwelling".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.MoaningAndGroaning".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.ShakeAndSquirm".Localized());

            if (Player.BiggestBreastSize() >= 5 && Player.HasFuckableNipples())
            {
                var x = 0;

                while (x < Player.BreastRows.Count)
                {
                    Player.BreastRows[x].BreastRating += 3 + Random.Shared.Next(3);

                    x += 1;
                }

                OutputText("Scenes.Valeria.ValeriaGooRapeII.TitsGrownMuchLarger".Localized());

                Player.Orgasm("Tits", false);
            }

            if (Player.HasCock() && Player.Balls > 0)
            {
                Player.BallSize += 3 + Random.Shared.Next(2);

                OutputText("Scenes.Valeria.ValeriaGooRapeII.BallsPositivelySwollen".Localized());
            }

            OutputText("Scenes.Valeria.ValeriaGooRapeII.BellyStuffedInto".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Valeria.ValeriaGooRapeII.SoGravidFromBoth".Localized());
            else
                OutputText("Scenes.Valeria.ValeriaGooRapeII.SentencePeriodAgain".Localized());

            OutputText("Scenes.Valeria.ValeriaGooRapeII.StaggerUpOnLegs".Localized());

            Player.RefillHunger(100);

            CoC.Instance.Flags.Set(GameFlagIds.TimesValeriaGooThreesomed, CoC.Instance.Flags.Get(GameFlagIds.TimesValeriaGooThreesomed) + 1);

            Player.Orgasm("Anal");

            Player.CreateStatusEffect(StatusEffectIds.GooStuffed, GOOSTUFFED_EFFECT_MIN_DURATION + Random.Shared.Next(GOOSTUFFED_EFFECT_MAX_DURATION_DELTA), 0, 0, 0);

            Player.StartButtPregnancy(PregnancyIds.GooStuffed, GOOSTUFFED_PREGNANCY_DURATION);

            if (Player.HasVagina())
            {
                Player.ChangeStatusValue(StatusEffectIds.GooStuffed, 3, 1);

                Player.KnockUp(PregnancyIds.GooStuffed, GOOSTUFFED_PREGNANCY_DURATION, 100, 1);
            }

            if (Player.HasCock())
                if (Player.Balls > 0)
                    Player.ChangeStatusValue(StatusEffectIds.GooStuffed, 2, 2);
                else
                    Player.ChangeStatusValue(StatusEffectIds.GooStuffed, 2, 1);

            if (Player.BiggestBreastSize() >= 5 && Player.HasFuckableNipples())
                Player.ChangeStatusValue(StatusEffectIds.GooStuffed, 4, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void BirthOutDatGooSlut()
        {

            OutputText("Scenes.Valeria.BirthOutDatGooSlut.SomethingOddHappens".Localized());

            OutputText("Scenes.Valeria.BirthOutDatGooSlut.SuddenViolentLurch".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.GooStuffed, 4) > 0)
                OutputText("Scenes.Valeria.BirthOutDatGooSlut.TitsWobblingAround".Localized());

            OutputText("Scenes.Valeria.BirthOutDatGooSlut.GetYourAnswerWhen".Localized());

            if (Player.HasFur())
                OutputText("Scenes.Valeria.BirthOutDatGooSlut.FurryWord".Localized());

            OutputText("Scenes.Valeria.BirthOutDatGooSlut.BitchInHeatGods".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.GooStuffed, 4) > 0)
            {
                OutputText("Scenes.Valeria.BirthOutDatGooSlut.ParagraphBreak".Localized());

                if (!Player.HasFuckableNipples())
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.StiffeningUpWord".Localized());
                else
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.OpeningUpWord".Localized());

                OutputText("Scenes.Valeria.BirthOutDatGooSlut.SuddenlyDisgorgeRivers".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.AndNextRowDown".Localized());

                if (Player.BreastRows.Count > 2)
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.AndSoOnWord".Localized());

                OutputText("Scenes.Valeria.BirthOutDatGooSlut.WorkingOutEveryDrop".Localized());
            }

            if (Player.HasCock() && Player.StatusEffectValue(StatusEffectIds.GooStuffed, 1) > 0)
            {
                OutputText("Scenes.Valeria.BirthOutDatGooSlut.FillsToTwitching".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.BallClenchingWord".Localized());
                else
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.BodyClenchingWord".Localized());

                OutputText("Scenes.Valeria.BirthOutDatGooSlut.QuiversOfBeatific".Localized());

                if (Player.StatusEffectValue(StatusEffectIds.GooStuffed, 1) >= 2)
                    OutputText("Scenes.Valeria.BirthOutDatGooSlut.BallsKeepEnhancedSize".Localized());
            }

            if (Player.HasVagina() && Player.StatusEffectValue(StatusEffectIds.GooStuffed, 2) > 0)
                OutputText("Scenes.Valeria.BirthOutDatGooSlut.BirthingGeyserOfSlime".Localized());

            OutputText("Scenes.Valeria.BirthOutDatGooSlut.PantToCatchBreath".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.GooStuffed);

            Player.ClearPregnancy();

            Player.ButtPregnancy = new PregnancyState();

            DoNext(PlayerMenu);
        }

        public void IsabellaAndValeriaSpar()
        {
            ClearOutput();

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.MakeYourWayBack".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.GroansNoddingToGooey".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.HuffsCrossingArms".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.WhatsTheFuckingPoint".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.SnapsScowling".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.AtLeastUsingSword".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.NeinIsabellaVillNot".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.ShouldersSlump".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.StartsGrabbingShoulder".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.BeamsHandsTheCow".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.IsabellaSwingsGreatsword".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.WatchAsTheArmor".Localized((Player.Intelligence > 80 ? "surprise" : "horror")));

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.AnswersHeadReappearing".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.IJaIchVill".Localized());

            OutputText("Scenes.Valeria.IsabellaAndValeriaSpar.ChucklesAsReabsorbs".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IsabellaValeriaSparred, 1);

            DoNext(PlayerMenu);
        }

        public void ValeriaAtRift()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ValeriaFoundInGlacialRift, 1);

            OutputText("Common.Text.AsYouMakeYourWayAcross".Localized());

            OutputText("Scenes.GlacialRift.ValeriaAtRift.TerritoryShout".Localized());

            AddButton(0, "Common.Button.Fight".Localized(), () =>
            {
                var enemy = EnemyRegistry.Create(EnemyIds.GooArmor);

                enemy.OnDefeated = _ => PcWinsValeriaSpar();

                enemy.OnWon = _ => PcWinsValeriaSparDefeat(true);

                CombatScene.Start(enemy, () => Go<CampScene>());
            });

            AddButton(1, "Common.Button.Submit".Localized(), () => PcWinsValeriaSparDefeat(true));
        }
    }
}