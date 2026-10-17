using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Plains
{
    [Register<SatyrScene>("coc:satyr")]
    public class SatyrScene : Scene
    {

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
            SatyrEncounter();
        }

        public void SatyrEncounter(int location = 0)
        {
            ClearOutput();

            if (Random.Shared.Next(2) == 0 || Player.Pregnancy.Incubation > 0 || Player.ButtPregnancy.Incubation > 0 || Player.Gender == Gender.Genderless)
            {
                ShowImage("monster-satyr");

                OutputText("Scenes.Satyrscene.SatyrEncounter.AsYouCross".Localized());

                if (location == 0)
                    OutputText("Scenes.Satyrscene.SatyrEncounter.GrassyPlains".Localized());
                else
                    OutputText("Scenes.Satyrscene.SatyrEncounter.SoddenExpanse".Localized());

                OutputText("Scenes.Satyrscene.SatyrEncounter.LewdBellowings".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntrySatyrs);

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.Satyr), () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Satyrscene.SatyrEncounter.YouWander".Localized());

                if (location == 0)
                    OutputText("Scenes.Satyrscene.SatyrEncounter.GrassyPlainsAgain".Localized());
                else
                    OutputText("Scenes.Satyrscene.SatyrEncounter.SoddenExpanseAgain".Localized());

                OutputText("Scenes.Satyrscene.SatyrEncounter.StrangeMusic".Localized());

                if (location == 0)
                {
                    AddButton(0, "Common.Button.Yes".Localized(), () => ConsensualSatyrFuck(0)); AddButton(1, "No", () => ReturnToCamp(1));
                }
                else
                {
                    AddButton(0, "Common.Button.Yes".Localized(), () => ConsensualSatyrFuck(0)); AddButton(1, "No", () => ReturnToCamp(1));
                }
            }
        }

        public void ConsensualSatyrFuck(int loc = 0)
        {
            ClearOutput();

            ShowImage("monster-satyr");

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.SearchSource".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.SittingCircle".Localized());

            if (loc == 0)
                OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.TrampledGrass".Localized());
            else
                OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.DrySolidGround".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.StrangeFigure".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.StomachGrumbles".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.SmilingJubilantly".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.BarelyHear".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.LaughsHeartily".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.BarelyRegister".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.BellowsLaughter".Localized());

            OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.SuddenlyDawns".Localized());

            if (Player.Intelligence > 60 && Player.Lust < Player.MaxLust)
            {
                OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.TrickHim".Localized());

                AddButton(0, "Common.Button.TrickHim".Localized(), TrickZeSatyr);
            }
            else
                AddButton(0, "Common.Button.TrickHim".Localized(), null);

            if (Player.Libido > 60)
            {
                OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.CockLooksYummy".Localized());

                AddButton(2, "Common.Button.SkipForeplay".Localized(), SkipForeplay);
            }
            else
                AddButton(2, "Common.Button.SkipForeplay".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Satyrscene.ConsensualSatyrFuck.OneMoreDrink".Localized());

                AddButton(3, "Common.Button.DrinkSex".Localized(), DrinkAndSex);
            }
            else
                AddButton(3, "Common.Button.DrinkSex".Localized(), null);

            AddButton(1, "Common.Button.KeepDrinking".Localized(), KeepDrinking);

            AddButton(14, "Common.Button.Leave".Localized(), LeavePartySatyr);
        }

        public void KeepDrinking()
        {
            ClearOutput();

            OutputText("Scenes.Satyrscene.KeepDrinking.GrinsEncouragement".Localized());

            OutputText("Scenes.Satyrscene.KeepDrinking.MatchingDrink".Localized());

            OutputText("Scenes.Satyrscene.KeepDrinking.OnlyTooHappy".Localized());

            OutputText("Scenes.Satyrscene.KeepDrinking.EatsDrinks".Localized());

            OutputText("Scenes.Satyrscene.KeepDrinking.CannotResistMelody".Localized());

            OutputText("Scenes.Satyrscene.KeepDrinking.KeepsPlaying".Localized());

            if (Player.HasVagina())
                Player.CuntChange(25, true, true, false);
            else
                Player.ButtChange(25, true, true, false);

            ShowImage("satyr-hangover");

            OutputText("Scenes.Satyrscene.KeepDrinking.WakesHoursLater".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.KeepDrinking.VaginaWord".Localized());
            else
                OutputText("Scenes.Satyrscene.KeepDrinking.AssWord".Localized());

            OutputText("Scenes.Satyrscene.KeepDrinking.MustBeWork".Localized());

            SatyrPreggo();

            DoNext(() => ReturnToCamp(4));
        }

        public void LeavePartySatyr()
        {
            ClearOutput();

            OutputText("Scenes.Satyrscene.LeavePartySatyr.ThanksGenerosity".Localized());

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Satyrscene.LeavePartySatyr.IndignantBleat".Localized());

                var satyr= EnemyRegistry.Create(EnemyIds.Satyr);

                CombatScene.Start(satyr, () => Go<CampScene>());

            var chargeId = EnemyIds.SatyrCharge;

            if (CombatAbilityHandlerRegistry.TryGet(chargeId, out var chargeHandler))
                chargeHandler(CombatController.Instance!.Context, CombatAbilityRegistry.Get(chargeId)!);
            }
            else
            {
                OutputText("Scenes.Satyrscene.LeavePartySatyr.TurnBack".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TrickZeSatyr()
        {
            ClearOutput();

            ShowImage("monster-satyr");

            OutputText("Scenes.Satyrscene.TrickZeSatyr.ComesUpPlan".Localized());

            OutputText("Scenes.Satyrscene.TrickZeSatyr.BlinksSurprise".Localized());

            OutputText("Scenes.Satyrscene.TrickZeSatyr.PretendDrink".Localized());

            OutputText("Scenes.Satyrscene.TrickZeSatyr.StartledQuickly".Localized());

            OutputText("Scenes.Satyrscene.TrickZeSatyr.RepeatsProcess".Localized());

            OutputText("Scenes.Satyrscene.TrickZeSatyr.AnotherSkin".Localized());

            OutputText("Scenes.Satyrscene.TrickZeSatyr.FirstTime".Localized());

            Player.Gems += 10 + Random.Shared.Next(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void SkipForeplay()
        {
            ClearOutput();

            OutputText("Scenes.Satyrscene.SkipForeplay.SmirksCrawls".Localized());

            OutputText("Scenes.Satyrscene.SkipForeplay.LooksSurprised".Localized());

            DoNext(() => WillinglyBoneSatyr());
        }

        public void DrinkAndSex()
        {
            ClearOutput();

            OutputText("Scenes.Satyrscene.DrinkAndSex.GrinsEncouragement".Localized());

            OutputText("Scenes.Satyrscene.DrinkAndSex.TakesEmptySkin".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Satyrscene.DrinkAndSex.CockSlot".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Satyrscene.DrinkAndSex.CommaSpace".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.DrinkAndSex.VaginaSlot".Localized());

            if (Player.HasCock() || Player.HasVagina())
                OutputText("Scenes.Satyrscene.DrinkAndSex.AndWord".Localized());

            OutputText("Scenes.Satyrscene.DrinkAndSex.LooksUpDown".Localized());

            DoNext(() => WillinglyBoneSatyr());
        }

        public void LoseToSatyr()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Satyrscene.LoseToSatyr.PantsMasturbating".Localized());
            else
                OutputText("Scenes.Satyrscene.LoseToSatyr.SteadiesHerself".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.WastesNoTime".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Satyrscene.LoseToSatyr.PinsDownBody".Localized());
            else
                if (Player.IsBiped())
                    OutputText("Scenes.Satyrscene.LoseToSatyr.SpreadsLegs".Localized());
                else
                    OutputText("Scenes.Satyrscene.LoseToSatyr.PinsYouDown".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.ProbesVagina".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.LoseToSatyr.GrinsLustfully".Localized());
            else
                OutputText("Scenes.Satyrscene.LoseToSatyr.LackingTrait".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.SqueezesCheeks".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.WatchInLust".Localized());

            if ((Player.HasVagina() && Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0)) || (!Player.HasVagina() && Player.AnalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0)))
                OutputText("Scenes.Satyrscene.LoseToSatyr.NotEvenBothered".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.SentencePeriod".Localized());

            if (Player.HasVagina())
            {
                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                ShowImage("satyr-sex-vag");
            }
            else
            {
                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                ShowImage("satyr-sex-butt");
            }

            OutputText("Scenes.Satyrscene.LoseToSatyr.ScreamsPainPleasure".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.ShiftsGrip".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.MoansShame".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.LoseToSatyr.WetWord".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Satyrscene.LoseToSatyr.AndWord".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Satyrscene.LoseToSatyr.HardWord".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Satyrscene.LoseToSatyr.ArousedWord".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.BeadsLubricant".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.ThrustsPickUp".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.ClenchesStrangling".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Satyrscene.LoseToSatyr.ThrobsStrings".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.LoseToSatyr.FloodGirlcum".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.BarelyNotices".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.LoseToSatyr.WombWord".Localized());
            else
                OutputText("Scenes.Satyrscene.LoseToSatyr.StomachWord".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.BellySwells".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.PregnantWoman".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.OnceRecover".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Satyrscene.LoseToSatyr.ShamefullyWord".Localized());
            else
                OutputText("Scenes.Satyrscene.LoseToSatyr.ShamelesslyWord".Localized());

            OutputText("Scenes.Satyrscene.LoseToSatyr.GatherRedress".Localized());

            Player.SlimeFeed();

            SatyrPreggo();

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            if (Player.Corruption < 50)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatASatyr()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Satyrscene.DefeatASatyr.CollapsesKnees".Localized());
            else
                OutputText("Scenes.Satyrscene.DefeatASatyr.BeatenAndDazed".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fuckhisbutt".Localized(), null);

            AddButton(1, "Common.Button.RideFace".Localized(), null);

            if (Player.Lust >= 33 && !Player.IsGenderless())
            {
                OutputText("Scenes.Satyrscene.DefeatASatyr.SortOfPayment".Localized());

                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()))
                    AddButton(0, "Common.Button.Fuckhisbutt".Localized(), MalesTakeAdvantageOfSatyrs);

                if (Player.HasVagina())
                {
                    AddButton(1, "Common.Button.RideFace".Localized(), FemaleTakesAdvantageOfSatyr);

                    if (Player.BiggestBreastSize() >= 4 && Player.Armor.Id == ArmorIds.LightMaidenArmor)
                        AddButton(2, "Common.Button.RideFace".Localized(), LustyMaidenPaizuri);
                }
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void FemaleTakesAdvantageOfSatyr()
        {
            ClearOutput();

            ShowImage("monster-satyr");

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.EyesShaft".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.BleatsEyesWide".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.TopplesHim".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.TooHornyFight".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.TongueStrokes".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.YourSack".Localized());
                else
                    OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.BaseOfCocks".Localized());
            }

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.DespiteHint".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.BleatsPain".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.QuiverGyrate".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.MuffledBleat".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.UnexpectedRain".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.BelchingLoad".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.SentencePeriod".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.PervertedFountain".Localized());

            OutputText("Scenes.Satyrscene.FemaleTakesAdvantageOfSatyr.GrindsFace".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void MalesTakeAdvantageOfSatyrs()
        {
            ClearOutput();

            var x= Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.ExaminesConquest".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.NotMouth".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.BulgesSwells".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.IgnoresEfforts".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.FingersSlide".Localized(
                 Player.CockDescript(x)
            ));

            ShowImage("satyr-sex-anally");

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.KnowWhatToDo".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.GroansSurprise".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.EveryTimeEnter".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.GroansDeepLow".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.TriesToMilk".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.DumpsFarMore".Localized());
                else
                    OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.JetAfterJet".Localized());

            OutputText("Scenes.Satyrscene.MalesTakeAdvantageOfSatyrs.SatedMoment".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void WillinglyBoneSatyr()
        {
            ClearOutput();

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.EyesYouUpDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.HardeningMaleness".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.RollsOrbs".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SpreadsLabia".Localized());
            else
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SpreadsCheeks".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.WonderfulLikeThis".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.LicksNetherlips".Localized());
            else
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.LicksAroundHole".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.BleatsExclaims".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SoundsGreat".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.TakesHips".Localized());

            if (Player.HasVagina())
            {
                Player.CuntChange(25, true, true, false);

                ShowImage("satyr-sex-vag");
            }
            else
            {
                Player.ButtChange(25, true, true, false);

                ShowImage("satyr-sex-butt");
            }

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.CriesOutFills".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.RollsOnTop".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.GraspClaw".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.BreaksKissBleats".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.ClenchingButt".Localized());
            else
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SpasmingVagina".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.HoseIntoYou".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.HowlingJoy".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.PussyGushes".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.AndWord".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.CockSpurtsCream".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.HeavingLungfuls".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.TrickleLoad".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.BowelsWord".Localized());
            else
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.WombWord".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SlowShortThrusts".Localized());

            if (!Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.SatyrKids) == 0)
            {
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.StaresBlankly".Localized());

                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.WinksDisquieted".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.MalesWord".Localized());
                else
                    OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.NeutersWord".Localized());

                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.LikeYouLover".Localized());

                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.StaresAtPartner".Localized());
            }

            if (Player.Corruption < 66)
            {
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.InvitationSounds".Localized());

                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SuitYourself".Localized());
            }
            else
            {
                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.ScratchesChin".Localized());

                OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.RubsStomach".Localized());
            }

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.SoftGroan".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.UnsteadyHooves".Localized());

            OutputText("Scenes.Satyrscene.WillinglyBoneSatyr.WatchesHimGo".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            SatyrPreggo();

            DoNext(() => ReturnToCamp(1));
        }

        public void SatyrPreggo()
        {

            if (Player.HasVagina())
                Player.KnockUp(PregnancyIds.Satyr, PregnancyIds.SatyrIncubation);
            else
                Player.StartButtPregnancy(PregnancyIds.Satyr, PregnancyIds.SatyrIncubation, 1);
        }

        public void SatyrBirth(bool vag)
        {

            ShowImage("birth-satyr");

            OutputText("Scenes.Satyrscene.SatyrBirth.SuddenPangs".Localized());

            if (vag && !Player.HasVagina())
            {
                OutputText("Scenes.Satyrscene.SatyrBirth.NewlyGrownVagina".Localized());

                Player.CreateVagina();
            }
            else
                if (vag)
                    OutputText("Scenes.Satyrscene.SatyrBirth.VaginaSlot".Localized());
                else
                    OutputText("Scenes.Satyrscene.SatyrBirth.ButtSlot".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.GrippingWrithing".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.StripsLaysDown".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.DoesntTakeLong".Localized());

            if (vag)
                OutputText("Scenes.Satyrscene.SatyrBirth.VaginaSlotAgain".Localized());
            else
                OutputText("Scenes.Satyrscene.SatyrBirth.AssholeSlot".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.MinisculeHorns".Localized());

            if (vag)
                Player.CuntChange(40, true, false, true);
            else
                Player.ButtChange(40, true, false, true);

            OutputText("Scenes.Satyrscene.SatyrBirth.LongMinutes".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.CuteGoatMan".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.StrokesBeard".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Satyrscene.SatyrBirth.BitDisgusted".Localized());

            OutputText("Scenes.Satyrscene.SatyrBirth.FitfulSleep".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SatyrKids, CoC.Instance.Flags.Get(GameFlagIds.SatyrKids) + 1);

            if (Player.Body.Butt < 10 && Random.Shared.Next(2) == 0)
            {
                Player.Body.Butt += 1;

                OutputText("Scenes.Satyrscene.SatyrBirth.AssFeelsLarger".Localized());
            }
            else
                if (Player.Body.Hips < 10)
                {
                    Player.Body.Hips += 1;

                    OutputText("Scenes.Satyrscene.SatyrBirth.ArmorFitsSnugly".Localized());
                }

            OutputText("Scenes.Satyrscene.SatyrBirth.ParagraphBreak".Localized());
        }

        public void LustyMaidenPaizuri()
        {
            var satyr = EnemyRegistry.Create(SceneIds.Core.SatyrEnemy);

            ClearOutput();

            ClearButtons();

            var cock = satyr.CockDescriptShort(0);

            var head = satyr.CockHead();

            var virgin = Player.HasVagina() && Player.Body.Get<VaginaPart>().Virgin;

            OutputText("Scenes.LustyMaiden.OpenerCockExposed".Localized(cock));

            if (satyr.Lust < 50)
                OutputText("Scenes.LustyMaiden.TierNotReady".Localized(head));
            else if (satyr.Lust < 100)
                OutputText("Scenes.LustyMaiden.TierTurgid".Localized(head));
            else
                OutputText("Scenes.LustyMaiden.TierRaging".Localized());

            OutputText("Scenes.LustyMaiden.Moistness".Localized());

            OutputText("Scenes.LustyMaiden.Chastise".Localized(cock));

            OutputText("Scenes.LustyMaiden.Smile".Localized());

            OutputText((virgin ? "Scenes.LustyMaiden.Virgin" : "Scenes.LustyMaiden.NotVirgin").Localized());

            OutputText("Scenes.LustyMaiden.SqueezeThemOut".Localized());

            OutputText("Scenes.LustyMaiden.Nods".Localized());

            if (satyr.Balls > 0)
                OutputText("Scenes.LustyMaiden.BallsFondle".Localized());
            else if (satyr.ShortName == "anemone")
                OutputText("Scenes.LustyMaiden.AnemoneTaint".Localized());
            else
                OutputText("Scenes.LustyMaiden.Taint".Localized());

            OutputText("Scenes.LustyMaiden.Dive".Localized(cock, head));

            if (satyr.CockArea(0) >= 80)
                OutputText("Scenes.LustyMaiden.ThroatStretch".Localized());

            OutputText("Scenes.LustyMaiden.MouthFeelsGood".Localized());

            OutputText("Scenes.LustyMaiden.UpAndDown".Localized());

            if (Player.HasFuckableNipples() || Player.LactationQ() >= 100)
                OutputText("Scenes.LustyMaiden.NipplesDripping".Localized());

            OutputText("Scenes.LustyMaiden.NipplesSensitive".Localized());

            OutputText((virgin ? "Scenes.LustyMaiden.MaidenheadWord" : "Scenes.LustyMaiden.SloppyGashWord").Localized());

            OutputText("Scenes.LustyMaiden.Hotter".Localized());

            OutputText("Scenes.LustyMaiden.BelowYou".Localized());

            if (satyr.CockArea(0) >= 80)
                OutputText("Scenes.LustyMaiden.MessyCough".Localized());

            OutputText("Scenes.LustyMaiden.TeaseCommand".Localized(satyr.Mf("boy", "girl")));

            OutputText("Scenes.LustyMaiden.GroansAndShakes".Localized());

            if (satyr.Balls > 0)
                OutputText("Scenes.LustyMaiden.BallsPumping".Localized());

            OutputText("Scenes.LustyMaiden.Urethra".Localized());

            OutputText("Scenes.LustyMaiden.HoldOn".Localized());

            OutputText("Scenes.LustyMaiden.Seize".Localized(satyr.Mf("His", "Her"), cock));

            OutputText("Scenes.LustyMaiden.FinalGlow".Localized());

            Player.SlimeFeed();

            var bonus = Flags.Get(GameFlagIds.BikiniArmorBonus) + 2;

            Flags.Set(GameFlagIds.BikiniArmorBonus, Math.Min(bonus, 8));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            if (virgin)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            AddButton(0, "Scenes.Explore.Continue".Localized(), () => ReturnToCamp(1));
        }
    }
}