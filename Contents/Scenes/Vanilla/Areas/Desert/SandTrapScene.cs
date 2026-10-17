using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<SandTrapScene>("coc:sand_trap")]
    public class SandTrapScene : Scene
    {

        public static Enemies.Vanilla.SandTrapEnemy? sandTrap =>

            CurrentMonster as Enemies.Vanilla.SandTrapEnemy;

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            EncounterASandTrap();
        }

        public void EncounterASandTrap()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SandtrapLossRepeats) >= 2 && Player.Body.Get<EyesPart>().Id == BodyPartTypes.BlackEyesSandTrap && Player.Body.Get<WingsPart>().Id == BodyPartTypes.GiantDragonfly)
            {
                SandTrapBadEnd();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredSandTraps, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredSandTraps) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredSandTraps) == 1)
            {
                if (Player.Femininity <= 65)
                {
                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.ExplorationsUndulating".Localized());

                    if (Player.Intelligence > 65)
                        OutputText("Scenes.Sandtrapscene.EncounterASandTrap.ScenePutsYou".Localized());

                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.ThinkCouldJustAbout".Localized());
                }
                else
                {
                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.ExplorationsUndulatingAgain".Localized());

                    if (Player.Intelligence > 65)
                        OutputText("Scenes.Sandtrapscene.EncounterASandTrap.ScenePutsYouAgain".Localized());

                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.ThinkCouldReach".Localized());
                }

                ClearButtons();

                AddButton(0, "Scenes.Camp.Save".Localized(), SaveTheSandTarps);

                AddButton(1, "Common.Button.DontSave".Localized(), DontSaveTheTarps);
            }
            else
            {
                OutputText("Scenes.Sandtrapscene.EncounterASandTrap.WalkingAimlessly".Localized());

                if (Player.Speed / 10 + Random.Shared.Next(20) >= 16)
                {
                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.MovingQuicklyLightly".Localized());

                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.GigglesLowersBrow".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Fight".Localized(), StartSandTarpFight);

                    AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.DontMoveQuickly".Localized());

                    OutputText("Scenes.Sandtrapscene.EncounterASandTrap.SaysHuskily".Localized());

                    CombatScene.Start(EnemyRegistry.Create(EnemyIds.SandTrap), () => Go<CampScene>());
                }
            }
        }

        public void SaveTheSandTarps()
        {
            ClearOutput();

            if (Player.Femininity <= 65)
            {
                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.KneelAtEdge".Localized());

                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.WomanDriftingAway".Localized());

                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.RumblingHissing".Localized());

                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.SaysPityingly".Localized());
            }
            else
            {
                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.KneelAtEdgeAgain".Localized());

                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.ManDriftingAway".Localized());

                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.RumblingHissingAgain".Localized());

                OutputText("Scenes.Sandtrapscene.SaveTheSandTarps.SaysPityinglyAgain".Localized());
            }

            var sandTrap = EnemyRegistry.Create(EnemyIds.SandTrap);

            sandTrap.ChangeStatusValue(StatusEffectIds.Level, 1, 2);

            CombatScene.Start(sandTrap, () => Go<CampScene>());
        }

        public void DontSaveTheTarps()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.StepsBackwards".Localized());

            if (Player.Femininity <= 65)
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.WomanWord".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.ManWord".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.PleaseCry".Localized());

            if (Player.Femininity <= 65)
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.SheWord".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.HeWord".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.CriesNeckSinking".Localized());

            if (Player.Femininity <= 65)
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.WomanWordAgain".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.ManWordAgain".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.ThrustsUpwardsPlunges".Localized());

            if (Player.Femininity <= 65)
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.HerWord".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.HimWord".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.DuneStandingOn".Localized());

            if (Player.Femininity <= 65)
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.WomanWordThird".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.ManWordThird".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.SpellInFront".Localized());

            if (Player.Femininity <= 65)
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.HerWordAgain".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.HisWord".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.MidriffExposed".Localized());

            OutputText("Scenes.Sandtrapscene.DontSaveTheTarps.CallsUpGrinning".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), StartSandTarpFight);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void StartSandTarpFight()
        {

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.SandTrap), () => Go<CampScene>(), canFlee: false);
        }

        public void SandtrapmentLoss(bool clear = false)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SandtrapLossRepeats) >= 2 && Player.Body.Get<EyesPart>().Id == BodyPartTypes.BlackEyesSandTrap && Player.Body.Get<WingsPart>().Id == BodyPartTypes.GiantDragonfly)
            {
                LoseLastFightWithSandTrap();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.SandtrapLossRepeats, CoC.Instance.Flags.Get(GameFlagIds.SandtrapLossRepeats) + 1);

            if (!(clear))
                OutputText("Scenes.Sandtrapscene.SandtrapmentLoss.ParagraphBreak".Localized());

            if (sandTrap?.TrapLevel() == 1)
                OutputText("Scenes.Sandtrapscene.SandtrapmentLoss.SunkToBelly".Localized());
            else
                OutputText("Scenes.Sandtrapscene.SandtrapmentLoss.RadiantAndMolten".Localized());

            if (Player.HasCock())
                DoNext(() => DickwieldersLoseToSandTarps());
            else
                if (Player.HasVagina())
                    DoNext(() => ChicksLoseToSandTarp());
                else
                    DoNext(() => GenderlessLoss());
        }

        public void PcBeatsATrap()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SandtrapLossRepeats, 0);

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sandtrapscene.PcBeatsATrap.GroansCollapses".Localized());
            else
                OutputText("Scenes.Sandtrapscene.PcBeatsATrap.SandStopsSinking".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Naga3some".Localized(), null);

            AddButton(1, "Common.Button.Useyourcock".Localized(), null);

            AddButton(2, "Common.Button.RideVaginal".Localized(), null);

            AddButton(3, "Common.Button.Handjob".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Sandtrapscene.PcBeatsATrap.BeforeYouGo".Localized());

                if (Player.IsNaga() && Player.HasStatusEffect(StatusEffectIds.Naga) && !Player.IsGenderless() && Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                    AddButton(0, "Common.Button.Naga3some".Localized(), NagaThreesomeWithSandTrap);

                if (Player.HasCock() && Player.Strength >= 60)
                    AddButton(1, "Common.Button.Useyourcock".Localized(), StickWangInSandgina);

                if (Player.HasVagina())
                    AddButton(2, "Common.Button.RideVaginal".Localized(), RideDatSandTarpLikeIts1999);

                if (!Player.IsGenderless())
                    AddButton(3, "Common.Button.Handjob".Localized(), UseSandTarpsHands);

                if (Player.HasVagina() && Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                    AddButton(4, "Common.Button.BikiniTitfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));

                if (Player.HasMultiTails())
                    AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());
            }

            AddButton(14, "Common.Button.Leave".Localized(), () => CombatScene.CleanupAfterCombat());
        }

        public void DickwieldersLoseToSandTarps()
        {
            ClearOutput();

            ShowImage("sandtrap-desert-male-lose-sex");

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.PurrsPushingFurther".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.ChattersTheTrap".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.CockProngSlides".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.FluttersSpread".Localized());

            if (Player.Cocks[0].Length < 6)
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.LooksDownSurprise".Localized());
            else
                if (Player.Cocks[0].Length < 15)
                    OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.IncrediblyTight".Localized());
                else
                    OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.IncrediblyTightAgain".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.CaressesFaceTenderly".Localized());

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.GaspsAndCoos".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.IgnoredEntirely".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.FinallyReleases".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.ChucklesLowly".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.ArmpitsWord".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.ForelegsWord".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.TryAndTurnHead".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.SelfSameTube".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.PushesHeadDown".Localized());

            if (Player.AnalCapacity() < 40)
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.AlthoughTheOil".Localized());
            else
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.AccommodatingAss".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.ContinuesToMake".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.AndCuntWord".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.FlexingOutMoisture".Localized());

            OutputText("Scenes.Sandtrapscene.DickwieldersLoseToSandTarps.AwakenWearily".Localized());

            CurrentMonster.CreateStatusEffect(StatusEffectIds.Fertilized, 0, 0, 0, 0);

            SandTrapPregChance();

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void ChicksLoseToSandTarp()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.HoldsYourHands".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.SaysGentlyStroking".Localized());

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Fertilized))
                OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.CuteWorkerFallen".Localized());
            else
                OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.CuteWorkerFallenAgain".Localized());

            ShowImage("sandtrap-desert-female-lose-vag-anal-sex");

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.GutturalNoise".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.LipsPressAgainst".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.FluttersSpread".Localized());

            Player.CuntChange(30, true, true, false);

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.NoIdeaWhat".Localized());

            if (Player.VaginalCapacity() < 30)
                OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.SighsWithPleasure".Localized());
            else
                OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.SighsWithPleasureAgain".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.ProngBaseRubs".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.FeelLikeLeaking".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.SoothingShushing".Localized());

            if (Player.AnalCapacity() < 30)
                OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.AlthoughTheOil".Localized());
            else
                OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.AccommodatingAss".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.BeginsToPick".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.ContinuesToMake".Localized());

            OutputText("Scenes.Sandtrapscene.ChicksLoseToSandTarp.AwakenStaggering".Localized());

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Fertilized))
                SandTrapPregChance();

            Player.Orgasm("Vaginal");

            Player.Orgasm("Anal", false);

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void GenderlessLoss()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.HoldsYourHands".Localized());

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.SaysLookingAtYou".Localized());

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Fertilized))
                OutputText("Scenes.Sandtrapscene.GenderlessLoss.NotWorkerOrDrone".Localized());
            else
                OutputText("Scenes.Sandtrapscene.GenderlessLoss.NotWorkerDroneAgain".Localized());

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.HelplessToIgnore".Localized());

            ShowImage("sandtrap-desert-genderless-lose-anal-sex");

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.CockProngSlides".Localized());

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.FluttersSpread".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.SoothingShushing".Localized());

            if (Player.AnalCapacity() < 30)
                OutputText("Scenes.Sandtrapscene.GenderlessLoss.AlthoughTheOil".Localized());
            else
                OutputText("Scenes.Sandtrapscene.GenderlessLoss.AccommodatingAss".Localized());

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.BeginsToPick".Localized());

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.ContinuesToMake".Localized());

            OutputText("Scenes.Sandtrapscene.GenderlessLoss.AwakenWearily".Localized());

            SandTrapPregChance();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void SandTrapPregChance()
        {

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Fertilized))
                Player.StartButtPregnancy(PregnancyIds.SandTrapFertile, PregnancyIds.SandTrapIncubation, 1);
            else
                Player.StartButtPregnancy(PregnancyIds.SandTrap, PregnancyIds.SandTrapIncubation, 1);
        }

        public void BirfSandTarps()
        {

            ShowImage("birth-sandtrap");

            OutputText("Scenes.Sandtrapscene.BirfSandTarps.EyesWidenGout".Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Sandtrapscene.BirfSandTarps.RollEyes".Localized());

                if (!Player.IsTaur() || (Player.Tallness * (5 / 6) < Player.Cocks[Player.BiggestCockIndex()].Length))
                    OutputText("Scenes.Sandtrapscene.BirfSandTarps.OilyHandsDescend".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Sandtrapscene.BirfSandTarps.RollEyesAgain".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (!Player.IsTaur())
                        OutputText("Scenes.Sandtrapscene.BirfSandTarps.OilyHandsPush".Localized());
                }
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText("Scenes.Sandtrapscene.BirfSandTarps.RollEyesThird".Localized());

                        if (!Player.IsTaur())
                            OutputText("Scenes.Sandtrapscene.BirfSandTarps.OilyHandsDescendAgain".Localized());
                    }

            OutputText("Scenes.Sandtrapscene.BirfSandTarps.NearlyCumEgg".Localized());

            OutputText("Scenes.Sandtrapscene.BirfSandTarps.BroughtToSenses".Localized());

            if (Player.Libido < 33)
                OutputText("Scenes.Sandtrapscene.BirfSandTarps.PickYourselfUp".Localized());
            else
                if (Player.LibidoPercent < 66)
                    OutputText("Scenes.Sandtrapscene.BirfSandTarps.SpendMomentEnjoying".Localized());
                else
                    OutputText("Scenes.Sandtrapscene.BirfSandTarps.SmileLazilyLieBack".Localized());

            Player.ButtChange(25, true, true, false);

            OutputText("Scenes.Sandtrapscene.BirfSandTarps.NewlineBreak".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 4f));
        }

        public void UseSandTarpsHands()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BendIntoSand".Localized());

            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.AskSoftly".Localized());

            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.SaysAtLast".Localized());

            if (Player.Gender == Gender.Male)
            {
                ShowImage("sandtrap-desert-male-win-get-hj");

                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.OnePairOfCreature".Localized());

                if (Player.Cocks[0].Length < 6)
                {
                    if (Player.Balls == 0)
                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.OnlyNeedsOneHand".Localized());
                    else
                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.NeedsOneHandAgain".Localized());

                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.SmallSizeDoesnt".Localized());
                }
                else
                    if (Player.Cocks[0].Length < 16)
                    {
                        if (Player.Balls == 0)
                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.GlidesHandUp".Localized());
                        else
                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.GlidesHandUpAgain".Localized());
                    }
                    else
                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.QuicklyComesTo".Localized());

                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.PumpsYouFaster".Localized());

                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyDouble".Localized());
                }
                else
                    if (Player.CockTotal() == 2)
                    {
                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.AsBeginsToPump".Localized(
                             Player.CockDescript(1)
                        ));

                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyDoubleAgain".Localized());
                    }
                    else
                        if (Player.CockTotal() == 3)
                        {
                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BeginsToPumpAgain".Localized(
                                 Player.CockDescript(2)
                            ));

                            if (Player.Balls > 0)
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.GrinsUpShyly".Localized());

                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyTripled".Localized());
                        }
                        else
                            if (Player.CockTotal() == 4)
                            {
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BeginsToPumpThird".Localized(
                                     Player.CockDescript(2),

                                     Player.CockDescript(3)
                                ));

                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyQuadrupled".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BeginsToPumpFourth".Localized(
                                     Player.CockDescript(2),

                                     Player.CockDescript(3)
                                ));

                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyQuadrupledAgain".Localized());
                            }
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    ShowImage("sandtrap-desert-female-win-get-fingering");

                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.OnePairHandsAgain".Localized());

                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.FingersMoveAll".Localized());

                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.FastLearner".Localized());

                    if (Player.Tone < 60)
                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.PicksUpPace".Localized());
                    else
                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.PicksUpPaceAgain".Localized());

                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.GrabBlackHands".Localized());
                }
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        ShowImage("sandtrap-desert-herm-win-get-hj");

                        OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.OnePairHandsThird".Localized(
                             Player.CockDescript(0)
                        ));

                        if (Player.Cocks[0].Length < 6)
                        {
                            if (Player.Balls == 0)
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.NeedsOneHandThird".Localized());
                            else
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.NeedsOneHandFourth".Localized());

                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.SmallSizeDoesntAgain".Localized());
                        }
                        else
                            if (Player.Cocks[0].Length < 16)
                            {
                                if (Player.Balls == 0)
                                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.GlidesHandUpThird".Localized());
                                else
                                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.GlidesHandUpFourth".Localized());
                            }
                            else
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.QuicklyComesToAgain".Localized());

                        if (Player.CockTotal() == 1)
                        {
                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.PumpsYouFasterAgain".Localized());

                            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyDoubleThird".Localized());
                        }
                        else
                            if (Player.CockTotal() == 2)
                            {
                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BeginsToPumpFifth".Localized(
                                     Player.CockDescript(1)
                                ));

                                OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyTriple".Localized());
                            }
                            else
                                if (Player.CockTotal() == 3)
                                {
                                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BeginsToPumpSixth".Localized(
                                         Player.CockDescript(1),

                                         Player.CockDescript(2)
                                    ));

                                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyUnbearably".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.BeginsToPumpSeventh".Localized(
                                         Player.CockDescript(1),

                                         Player.CockDescript(2)
                                    ));

                                    OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.WarmOilyQuadrupledThird".Localized());
                                }
                    }

            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.SighWithSatisfaction".Localized());

            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.ManagesEventually".Localized());

            OutputText("Scenes.Sandtrapscene.UseSandTarpsHands.SayOverShoulder".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void RideDatSandTarpLikeIts1999()
        {
            ClearOutput();

            ShowImage("sandtrap-desert-female-win-vag-ride");

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.BendIntoSand".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.BreathingHeavily".Localized(
                 Appearance.AllBreastsDescript(Player),

                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.AskForIt".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.ManagesInTiny".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.BegForIt".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.CriesOutFluttering".Localized());

            if (Player.VaginalCapacity() < 30)
                OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.ThinLengthPerfect".Localized());
            else
                OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.ThinLengthLost".Localized());

            Player.CuntChange(14, true, true, false);

            ShowImage("sandtrap-win");

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.CantLastLong".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.RotateYourBending".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.ForceTrapOrgasm".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.FlexesInConcert".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.AmplifiedEuphoria".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.WhatSaySlave".Localized());

            OutputText("Scenes.Sandtrapscene.RideDatSandTarpLikeIts1999.TakesCreatureMoments".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void StickWangInSandgina()
        {
            ClearOutput();

            var x = Player.CockThatFits(80);

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("sandtrap-desert-male-win-vag-sex");

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.BendIntoSand".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.TakeMomentToLook".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sandtrapscene.StickWangInSandgina.BallsSwellReadiness".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.AndFillsYouDeep".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.AirOfPurposefulness".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.IgnoreItConcentrating".Localized());

            if (Player.Cocks[x].Length < 8)
                OutputText("Scenes.Sandtrapscene.StickWangInSandgina.MoistCunt".Localized());
            else
                if (Player.Cocks[x].Length < 35)
                    OutputText("Scenes.Sandtrapscene.StickWangInSandgina.MoistSex".Localized());
                else
                {
                    OutputText("Scenes.Sandtrapscene.StickWangInSandgina.MoistSexAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sandtrapscene.StickWangInSandgina.SlapAgainstCreature".Localized());

                    OutputText("Scenes.Sandtrapscene.StickWangInSandgina.CoherenceFliesOut".Localized());
                }

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.TubeLikeOpening".Localized());

            if (silly)
                OutputText("Scenes.Sandtrapscene.StickWangInSandgina.FewerPoutingLips".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.TopHalfHas".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.CreatureSaysHyes".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.ComeToSenses".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sandtrapscene.StickWangInSandgina.BallsAcheAnd".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.VisionSwims".Localized());

            OutputText("Scenes.Sandtrapscene.StickWangInSandgina.SleepyWordsReach".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void NagaThreesomeWithSandTrap()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SlitherDown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SandtrapNaga3some) == 0)
            {
                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SmallCoughAbove".Localized());

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SaysLonelyShared".Localized());

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SuggestCorkscrewing".Localized());
            }
            else
                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.LookUpSomehow".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.HereLoverAllowMe".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.LoosenCoils".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SandtrapFluttering".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.NagaWrappedCoil".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.HissesHuskily".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.KnowsACue".Localized());

            if (Player.HasCock())
            {
                ShowImage("sandtrap-desert-male-win-naga-3some");

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.PressHardening".Localized());

                if (Player.Cocks[0].Length < 14)
                {
                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.GoodFitFor".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SlappingIntoNose".Localized());
                }
                else
                {
                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.BiggerThanWhat".Localized());

                    if (Player.HasPlainSkin())
                        OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.BeadingSweatWord".Localized());

                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.PistoningIntoOpening".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SwingingHeavily".Localized());

                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.PushingLubricated".Localized());
                }

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.InFrontNaga".Localized());

                if (Player.Cocks[0].Length >= 14)
                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.NotSureWhich".Localized());

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.MindlesslyFourArms".Localized());

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.NagaSoonDeep".Localized());

                if (Player.CumQ() > 1000)
                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.LongSensuousBlowjob".Localized());
            }
            else
            {
                ShowImage("sandtrap-desert-female-win-naga-3some");

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.PressMoistening".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Wetness() < 4)
                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.GentleWorship".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.CloseEyesLose".Localized());

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.SimmeringWithPleasure".Localized());

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.NagaSoonDeepAgain".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.EventuallyNagaBegins".Localized());

                if (Player.CumQ() > 1000)
                    OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.LongSensuousTongueBath".Localized(
                         Player.VaginaDescript(0)
                    ));
            }

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.EventuallyTwoRide".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.WhispersReleases".Localized());

            OutputText("Scenes.Sandtrapscene.NagaThreesomeWithSandTrap.DefinitelyThinkDesert".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void SandTrapBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.OnceAgainWandering".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.StartComeTo".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.AndrogyneSaysSoftly".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.StandAtLip".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.HardensThinFrame".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.VaginaMoistensThin".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEnd.InhaleHardTry".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), SandTrapBadEndFight);

            AddButton(1, "Common.Button.Desztiny".Localized(), Desztiny);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveSandTrapBadEnd);
        }

        public void SandTrapBadEndFight()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFight.ShakeYourselfOut".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.SandTrap), () => Go<CampScene>());
        }

        public void LeaveSandTrapBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.LeaveSandTrapBadEnd.BreakFree".Localized());

            OutputText("Scenes.Sandtrapscene.LeaveSandTrapBadEnd.CalmVoiceReaches".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Desztiny()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.Desztiny.RightWhatYour".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandtrapscene.Desztiny.CockStrainsUpwards".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sandtrapscene.Desztiny.FeelButBarelyHear".Localized());

            OutputText("Scenes.Sandtrapscene.Desztiny.IncredibleAsStumble".Localized());

            OutputText("Scenes.Sandtrapscene.Desztiny.SaysGiggling".Localized());

            SandTrapBadEndFinale();
        }

        public void LoseLastFightWithSandTrap()
        {
            ClearOutput();

            OutputText("Scenes.Sandtrapscene.LoseLastFightWithSandTrap.SaysColorHigh".Localized());

            SandTrapBadEndFinale();
        }

        public void SandTrapBadEndFinale()
        {

            ShowImage("sandtrap-desert-badend");

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.LipsMoveSlippery".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.StartWakeGroggily".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.FeelingYourWord".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.CockHardenWord".Localized());
                else
                    OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.VaginaDribbleWord".Localized());
            }

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.YesRememberQuest".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.FlitAroundDesert".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.ExpressionChanges".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.MasturbatingDesperately".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.FingeringDesperately".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.ButOfCourseFresh".Localized());

            OutputText("Scenes.Sandtrapscene.SandTrapBadEndFinale.SpendRestOf".Localized());

            GameOver();
        }

    }
}