using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<FaerieScene>("coc:faerie")]
    public class FaerieScene : Scene
    {

        internal static void GladesEncounter() => new FaerieScene().GladesScene();

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
            ExecEncounter();
        }

        public void RegisterMacros()
        {

            RegisterMacro("faehair", FaerieHairColor);

            RegisterMacro("faeoutfit", outfits[FaerieOutfit]);

            RegisterMacro("faeisnude", FaerieOutfit == "nude");

            RegisterMacro("faeisstraps", FaerieOutfit == "straps");

            RegisterMacro("faeisleaf", FaerieOutfit == "leaf");

            RegisterMacro("faeispetal", FaerieOutfit == "petal");
        }

        public void RandomizeFaerie()
        {

            var hairChoice = new WeightedRandom<string>()
            .Add("yellow", 5)
            .Add("red", 5)
            .Add("violet", 5)
            .Add("indigo", 5)
            .Add("pink", 5)
            .Add("white", 5)
            .Add("pale blue", 5)
            .Add("lilac", 5)
            .Add("orange", 5)
            .Add("purple", 5)
            .Add("green", 5)
            .Add("black", 5)
            .Add("blue", 5)
            .Add("lavender", 5)
            .Add("fuchsia", 5)
            .Add("saffron", 5);

            var outfitChoice = new WeightedRandom<string>()
            .Add("nude", 3)
            .Add("straps", 1)
            .Add("leaf", 3)
            .Add("petal", 3);

            FaerieHairColor = hairChoice.Choose();

            FaerieOutfit = outfitChoice.Choose();
        }

        public void ExecEncounter()
        {
            ClearOutput();

            RandomizeFaerie();

            RegisterMacros();

            if (silly && Player.HpPercent < 50 && Player.Armor.Id == ArmorIds.KokiriClothes)
            {
                HeyListen();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.CodexEntryFaerie) == 1 && Player.Body.Get<HairPart>().Id == BodyPartTypes.Vine && Player.Body.Get<HairPart>().FlowerColor != "")
            {
                OutputText("Scenes.Faerie.ExecEncounter.TraipsingThroughBrush".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.ShooAway".Localized(), FaerieShooAway);

                AddButton(1, "Common.Button.Nothing".Localized(), FaerieFlowerDoNothing);

                AddButton(2, "Common.Button.Catch".Localized(), FaerieCatch);

                AddButton(4, "Common.Button.NoMore".Localized(), () => DisableFaerieEncounterForGood(true));
            }
            else
            {
                OutputText("Scenes.Faerie.ExecEncounter.FaerieAppears".Localized());

                if (Player.CockTotal() > 0 && (Player.BiggestBreastSize() <= 1 || Random.Shared.Next(2) == 0))
                {
                    OutputText("Scenes.Faerie.ExecEncounter.FaerieExclaims".Localized());

                    if (Random.Shared.Next((int)(Player.Speed / 2)) + Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) > 15)
                    {
                        if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 5)
                        {
                            OutputText("Scenes.Faerie.ExecEncounter.LungeToSnatch".Localized());

                            OutputText("Scenes.Faerie.ExecEncounter.FaeDrewYouIn".Localized());
                        }
                        else
                            if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 10)
                            {
                                OutputText("Scenes.Faerie.ExecEncounter.SnatchFromAir".Localized());

                                OutputText("Scenes.Faerie.ExecEncounter.CaughtBefore".Localized());
                            }
                            else
                                if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 15)
                                {
                                    OutputText("Scenes.Faerie.ExecEncounter.GraspDizzyFae".Localized());

                                    OutputText("Scenes.Faerie.ExecEncounter.SheWantsOff".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Faerie.ExecEncounter.TakeInHand".Localized());

                                    OutputText("Scenes.Faerie.ExecEncounter.DoYouLetHer".Localized());
                                }

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 10 + 2)));

                        Player.UnlockCodexEntry(GameFlagIds.CodexEntryFaerie);

                        SexMenu();

                        return;
                    }

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 10 + 2)));

                    OutputText("Scenes.Faerie.ExecEncounter.LungeMiss".Localized());

                    if (Player.LustPercent >= 90)
                    {
                        OutputText("Scenes.Faerie.ExecEncounter.GroanFrustrated".Localized());

                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Faerie.ExecEncounter.YourDick".Localized());
                        else
                            OutputText("Scenes.Faerie.ExecEncounter.OneOfYourDicks".Localized());

                        OutputText("Scenes.Faerie.ExecEncounter.TooLargeImage".Localized());

                        Player.Orgasm("Dick");
                    }

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntryFaerie);

                    ReturnToCamp(1);

                    return;
                }

                OutputText("Scenes.Faerie.ExecEncounter.FaeCuriousInterest".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryFaerie);

                ClearButtons();

                AddButton(0, "Common.Button.ShooAway".Localized(), FaerieShooAway);

                AddButton(1, "Common.Button.Nothing".Localized(), FaerieDoNothing);

                AddButton(2, "Common.Button.Catch".Localized(), FaerieCatch);

                AddButton(4, "Common.Button.NoMore".Localized(), () => DisableFaerieEncounterForGood(true));
            }
        }

        public bool GladesEnabled()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) < 50 && CoC.Instance.Flags.Get(GameFlagIds.FaerieEncounterDisabled) <= 0;
        }

        public void GladesScene()
        {
            ClearOutput();

            RandomizeFaerie();

            RegisterMacros();

            OutputText("Scenes.Faerie.GladesScene.StrangeScent".Localized());

            OutputText("Scenes.Faerie.GladesScene.TinyFaeAppears".Localized());

            OutputText("Scenes.Faerie.GladesScene.PitchedMoans".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 10 + 2)));

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryFaerie, true, false);

            ClearButtons();

            AddNextButton("Scenes.Faerie.GladesScene.ButtonHelp".Localized(), FaerieHelpHer);

            AddNextButton("Common.Button.Fuck".Localized(), FaerieFuckHer);

            AddNextButton("Common.Button.Leave".Localized(), FaerieLeaveHer);
        }

        public void FaerieFlowerDoNothing()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieFlowerDoNothing.FlowerGreeting".Localized());

            OutputText("Scenes.Faerie.FaerieFlowerDoNothing.ZipsToFace".Localized());

            Player.ChangeFatigue(-5);

            Player.Body.Get<HairPart>().Length += 0.2f;

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieHelpHer()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieHelpHer.ShakeOffHeat".Localized());

            OutputText("Scenes.Faerie.FaerieHelpHer.ShufflesLegs".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieFuckHer()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieFuckHer.WorkedUp".Localized());

            OutputText("Scenes.Faerie.FaerieFuckHer.LikelyTerrified".Localized());

            OutputText("Scenes.Faerie.FaerieFuckHer.ThoughtfulPause".Localized());

            OutputText("Scenes.Faerie.FaerieFuckHer.SuctionAbility".Localized());

            OutputText("Scenes.Faerie.FaerieFuckHer.TuggingShudders".Localized());

            OutputText("Scenes.Faerie.FaerieFuckHer.ConstrictionEases".Localized());

            Player.Orgasm("Dick");

            Player.ChangeFatigue(10);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 4f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieLeaveHer()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieLeaveHer.SafeToAssume".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieCatch()
        {

            if (Player.Speed - (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) * 5) < 50)
            {
                OutputText("Scenes.Faerie.FaerieCatch.LoweredGuard".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Faerie.FaerieCatch.ObliviousFlutters".Localized());

                SexMenu();
            }
        }

        public void SexMenu()
        {

            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), FaerieOral);

            if (Player.HasCock())
                AddNextButton((Player.HasVagina() ? "Male " : "") + "Common.Text.Rape".Localized(), FaerieCaptureHJ);

            if (Player.HasVagina())
                AddNextButton((Player.HasCock() ? "Female " : "") + "Common.Text.Rape".Localized(), FaerieRAEP);

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1 && Player.HasCock() && silly)
                AddNextButton("Scenes.Faerie.SexMenu.ButtonShouldraDick".Localized(), FaerieShouldraDick);

            if (OptionStore.Instance.Get("gore", 0) > 0)
                AddNextButton("Scenes.Faerie.SexMenu.ButtonEat".Localized(), FaerieEatingNotVore);

            AddNextButton("Scenes.Faerie.SexMenu.ButtonFeedHoney".Localized(), FaerieHoneyFeed);

            AddNextButton("Scenes.Faerie.SexMenu.ButtonRelease".Localized(), LetFaerieGo);

            AddNextButton("Scenes.Faerie.SexMenu.ButtonNever".Localized(), () => DisableFaerieEncounterForGood());
        }

        public void FaerieOral()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieOral.ShiftFae".Localized());

            OutputText("Scenes.Faerie.FaerieOral.NotSoBad".Localized());

            OutputText("Scenes.Faerie.FaerieOral.WhimpersCurls".Localized());

            OutputText("Scenes.Faerie.FaerieOral.CarefulNotAsBig".Localized());

            OutputText("Scenes.Faerie.FaerieOral.FlexLips".Localized());

            OutputText("Scenes.Faerie.FaerieOral.FeetPress".Localized());

            OutputText("Scenes.Faerie.FaerieOral.TinyLungsGasps".Localized());

            OutputText("Scenes.Faerie.FaerieOral.KissHerPussy".Localized());

            OutputText("Scenes.Faerie.FaerieOral.DoThisAgain".Localized());

            OutputText("Scenes.Faerie.FaerieOral.CarryToRest".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieRAEP()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.FaerieFemFuck))
                Player.CreateStatusEffect(StatusEffectIds.FaerieFemFuck, 1, 0, 0, 0);
            else
                Player.ChangeStatusValue(StatusEffectIds.FaerieFemFuck, 0, 1);

            OutputText("Scenes.Faerie.FaerieRAEP.LetHerInvestigate".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.FaerieFemFuck, 0) == 1)
                OutputText("Scenes.Faerie.FaerieRAEP.JuicesScreams".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.FaerieFemFuck, 0) <= 5)
                    OutputText("Scenes.Faerie.FaerieRAEP.JuicesMoans".Localized());
                else
                    OutputText("Scenes.Faerie.FaerieRAEP.SquealsRocks".Localized());

            if (Player.IsTaur())
            {
                OutputText("Scenes.Faerie.FaerieRAEP.BopOnHead".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.GetClitLength() >= 3)
                {
                    OutputText("Scenes.Faerie.FaerieRAEP.JerksYourClit".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.Vaginas[0].Wetness <= 2)
                        OutputText("Scenes.Faerie.FaerieRAEP.GigglesMore".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                        if (Player.Vaginas[0].Wetness <= 4)
                            OutputText("Scenes.Faerie.FaerieRAEP.GigglesMoreAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));
                        else
                            OutputText("Scenes.Faerie.FaerieRAEP.GigglesMoreThird".Localized(
                                 Player.VaginaDescript(0)
                            ));
                }
                else
                {
                    OutputText("Scenes.Faerie.FaerieRAEP.RubsHands".Localized());

                    if (Player.Vaginas[0].Wetness <= 2)
                        OutputText("Scenes.Faerie.FaerieRAEP.CumSuddenly".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                        if (Player.Vaginas[0].Wetness <= 4)
                            OutputText("Scenes.Faerie.FaerieRAEP.CumSuddenlyAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));
                        else
                            OutputText("Scenes.Faerie.FaerieRAEP.CumSuddenlyThird".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Faerie.FaerieRAEP.ReleaseLowerArmor".Localized());

                if (Player.StatusEffectValue(StatusEffectIds.FaerieFemFuck, 0) < 4)
                    OutputText("Scenes.Faerie.FaerieRAEP.MoodShifts".Localized());
                else
                    OutputText("Scenes.Faerie.FaerieRAEP.EyesOpenWide".Localized());

                OutputText("Scenes.Faerie.FaerieRAEP.ReleaseFaerie".Localized());

                if (Player.Vaginas[0].Wetness >= 5)
                    OutputText("Scenes.Faerie.FaerieRAEP.ReleasingTinySquirt".Localized());
                else
                    if (Player.Vaginas[0].Wetness >= 4)
                        OutputText("Scenes.Faerie.FaerieRAEP.DribblingJuices".Localized());
                    else
                        if (Player.Vaginas[0].Wetness >= 2)
                            OutputText("Scenes.Faerie.FaerieRAEP.GrowingSlippery".Localized());
                        else
                            OutputText("Scenes.Faerie.FaerieRAEP.MoisteningWithNeed".Localized());

                OutputText("Scenes.Faerie.FaerieRAEP.FromTinyTouches".Localized());

                if (Player.GetClitLength() <= .5f)
                {
                    OutputText("Scenes.Faerie.FaerieRAEP.PullsApartLips".Localized(
                         Player.ClitDescript()
                    ));

                    OutputText("Scenes.Faerie.FaerieRAEP.SquealingBouncing".Localized(
                         Player.VaginaDescript(0)
                    ));
                }
                else
                    if (Player.GetClitLength() <= 1.25f)
                    {
                        OutputText("Scenes.Faerie.FaerieRAEP.WatchesEntranced".Localized());

                        OutputText("Scenes.Faerie.FaerieRAEP.SpreadLegsCareful".Localized(
                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Faerie.FaerieRAEP.OrgasmsSquirting".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));
                    }
                    else
                        if (Player.GetClitLength() <= 4.5f)
                        {
                            OutputText("Scenes.Faerie.FaerieRAEP.EntrancedByGrowth".Localized());

                            OutputText("Scenes.Faerie.FaerieRAEP.GrabsSlipperySurface".Localized(
                                 Player.ClitDescript()
                            ));

                            OutputText("Scenes.Faerie.FaerieRAEP.ImpalesHerself".Localized(
                                 Player.VaginaDescript(0)
                            ));
                        }
                        else
                        {
                            OutputText("Scenes.Faerie.FaerieRAEP.EntrancedBySwollen".Localized());

                            OutputText("Scenes.Faerie.FaerieRAEP.JumpsOntoIt".Localized());

                            OutputText("Scenes.Faerie.FaerieRAEP.ClitJumpsThrows".Localized(
                                 Player.VaginaDescript(0)
                            ));
                        }
            }

            if (Player.StatusEffectValue(StatusEffectIds.FaerieFemFuck, 0) == 1)
            {
                OutputText("Scenes.Faerie.FaerieRAEP.LyingInLoam".Localized());

                OutputText("Scenes.Faerie.FaerieRAEP.FliesOffHungry".Localized());
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.FaerieFemFuck, 0) <= 5)
                {
                    OutputText("Scenes.Faerie.FaerieRAEP.DragsOutOfPussy".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Faerie.FaerieRAEP.SpinsHappy".Localized());

                    OutputText("Scenes.Faerie.FaerieRAEP.FlitsAwayBye".Localized());
                }
                else
                {
                    OutputText("Scenes.Faerie.FaerieRAEP.StumblesOutGiggling".Localized());

                    OutputText("Scenes.Faerie.FaerieRAEP.FlitsUnsteady".Localized());
                }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, .5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieEatingNotVore()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieEatingNotVore.SalivateTaste".Localized());

            OutputText("Scenes.Faerie.FaerieEatingNotVore.YellsHappily".Localized());

            OutputText("Scenes.Faerie.FaerieEatingNotVore.StammersFear".Localized());

            OutputText("Scenes.Faerie.FaerieEatingNotVore.FearStricken".Localized());

            OutputText("Scenes.Faerie.FaerieEatingNotVore.TearFleshStrands".Localized());

            Player.RefillHunger(5, true, false);

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieShooAway()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieShooAway.ShooAway".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieDoNothing()
        {
            ClearOutput();

            if (Player.NippleLength >= 1)
            {
                OutputText("Scenes.Faerie.FaerieDoNothing.LooksYouOver".Localized());

                OutputText("Scenes.Faerie.FaerieDoNothing.NippleWet".Localized());

                if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.Faerie.FaerieDoNothing.MilkSquirt".Localized());

                OutputText("Scenes.Faerie.FaerieDoNothing.FindRhythm".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Faerie.FaerieDoNothing.DiddleYourself".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Faerie.FaerieDoNothing.JerkYourself".Localized());

                OutputText("Scenes.Faerie.FaerieDoNothing.SheShudders".Localized());

                if (!Player.IsGenderless())
                    OutputText("Scenes.Faerie.FaerieDoNothing.JoinHer".Localized());

                OutputText("Scenes.Faerie.FaerieDoNothing.GoesLimp".Localized());

                if (Player.BiggestLactation() > 1.5f)
                    OutputText("Scenes.Faerie.FaerieDoNothing.MilkGout".Localized());

                Player.Orgasm("Tits");

                Player.ApplyStats(new DynStat(GameStats.Libido, -2f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (Player.HasVagina() && Player.GetClitLength() >= 1.0f && Player.GetClitLength() <= 4.5f && Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Faerie.FaerieDoNothing.SmileFlutters".Localized());

                if (Player.Vaginas[0].Wetness >= 4)
                    OutputText("Scenes.Faerie.FaerieDoNothing.DrenchInJuices".Localized());
                else
                    OutputText("Scenes.Faerie.FaerieDoNothing.MaintainComposure".Localized());

                OutputText("Scenes.Faerie.FaerieDoNothing.ComeDown".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (Player.HasVagina() && Player.GetClitLength() > 4.5f)
            {
                OutputText("Scenes.Faerie.FaerieDoNothing.WhispersEar".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Faerie.FaerieDoNothing.SucksNipple".Localized());

            OutputText("Scenes.Faerie.FaerieDoNothing.FrownDress".Localized());

            Player.NippleLength += .25f;

            if (Player.NippleLength > 3 || Player.BiggestBreastSize() <= 2)
            {
                OutputText("Scenes.Faerie.FaerieDoNothing.AppearsTemporary".Localized());

                Player.NippleLength -= .25f;
            }

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void LetFaerieGo()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.LetFaerieGo.ApologizeRelease".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DisableFaerieEncounterForGood(bool alt = false)
        {
            ClearOutput();

            if (alt)
            {
                OutputText("Scenes.Faerie.DisableFaerieEncounterForGood.NeverBotherMe".Localized());

                OutputText("Scenes.Faerie.DisableFaerieEncounterForGood.NeverSeeAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Faerie.DisableFaerieEncounterForGood.ApologizeRelease".Localized());

                OutputText("Scenes.Faerie.DisableFaerieEncounterForGood.MentalNote".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.FaerieEncounterDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieCaptureHJ()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.FaerieFucked))
                Player.ChangeStatusValue(StatusEffectIds.FaerieFucked, 0, 2);
            else
                Player.CreateStatusEffect(StatusEffectIds.FaerieFucked, 2, 0, 0, 0);

            if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 15)
            {
                OutputText("Scenes.Faerie.FaerieCaptureHJ.HoldTightlyScold".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.LooksUpGulps".Localized());
            }

            OutputText("Scenes.Faerie.FaerieCaptureHJ.LetLoose".Localized(
                 Player.ClothedOrNakedLower("the bulge of "),

                 Player.ClothedOrNakedLower("slips under your " + Player.ArmorName, "climbs onto you")
            ));

            if (Player.IsTaur())
            {
                OutputText("Scenes.Faerie.FaerieCaptureHJ.ClimbsOnCock".Localized());

                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.Largest".Localized(
                         Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                OutputText("Scenes.Faerie.FaerieCaptureHJ.CrawlsAbout".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.BodyJerks".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.SmashesOnCocks".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.GlobulesInMouth".Localized());
                else
                    if (Player.CumQ() < 50)
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.SplashesFace".Localized());
                    else
                        if (Player.CumQ() < 200)
                            OutputText("Scenes.Faerie.FaerieCaptureHJ.WashesFace".Localized());
                        else
                            OutputText("Scenes.Faerie.FaerieCaptureHJ.CollidesPropels".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

                if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 10)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.BurpsGigglesAgain".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 15)
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.BurpsLaughs".Localized());
                    else
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.BurpsMasturbates".Localized());

                if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 15)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.LicksFingersRollsAgain".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.TakesOffDrippingAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Faerie.FaerieCaptureHJ.ReachesSwollen".Localized());

                if (Player.HasKnot(0))
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.ClimbsAtopKnot".Localized());
                else
                    if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.ClimbsAtopPrepuce".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                            OutputText("Scenes.Faerie.FaerieCaptureHJ.ClimbsAtopCorrupted".Localized());
                        else
                            if (Player.Cocks[0].Id == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Faerie.FaerieCaptureHJ.ClimbsSquirming".Localized());
                            else
                                OutputText("Scenes.Faerie.FaerieCaptureHJ.ClimbsHardness".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.MusclesClench".Localized(
                     Player.ClothedOrNakedLower("your [armor]", "her mouth")
                ));

                OutputText("Scenes.Faerie.FaerieCaptureHJ.OnlyFewInches".Localized(
                     Player.ClothedOrNakedLower("Too late you realize you should have gotten at least partially undressed. You cum before you can do anything about it, splattering your [armor] with seed and leaving a wet patch on the crotch. You can feel it dripping back onto you and the faerie as more spunk squirts out, soaking the tiny girl in spooge as the wet spot grows", "Good thing your junk is exposed as otherwise you would have ended up jizzing in your pants. You cum, splattering the faerie with seed. This continues until the tiny girl is soaked in spooge")
                ));

                if (Player.CumQ() > 250)
                {
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.CumUncontrollably".Localized(
                         Player.ClothedOrNakedLower("regretting your fertility as your body paints the inside of your [armor] with goopy whiteness.", "painting the ground with goopy whiteness.")
                    ));

                    if (Player.CumQ() > 500)
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.ProofPuddle".Localized());
                    else
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.FallsBackwards".Localized());
                }
                else
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.LetterY".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.WatchGroin".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.RollsOff".Localized());

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.InTheCum".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.GigglingDrunkenly".Localized());

                if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 10)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.BurpsGigglesAgain".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 15)
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.BurpsLaughsAgain".Localized());
                    else
                        OutputText("Scenes.Faerie.FaerieCaptureHJ.BurpsMasturbatesAgain".Localized());

                if (Player.StatusEffectValue(StatusEffectIds.FaerieFucked, 0) < 15)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.LicksFingersRollsAgain".Localized());

                OutputText("Scenes.Faerie.FaerieCaptureHJ.TakesOffDrippingAgain".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

                if (!Player.HasStatusEffect(StatusEffectIds.Jizzpants) && Player.Armor.LongName != "nothing" && Player.Armor.Id != ArmorIds.GooArmor)
                    Player.CreateStatusEffect(StatusEffectIds.Jizzpants, 1, 0, 0, 0);

                if (Player.Armor.Id == ArmorIds.GooArmor)
                    OutputText("Scenes.Faerie.FaerieCaptureHJ.JizzAbsorbed".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieShouldraDick()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.FaerieFucked))
                Player.ChangeStatusValue(StatusEffectIds.FaerieFucked, 0, 2);
            else
                Player.CreateStatusEffect(StatusEffectIds.FaerieFucked, 2, 0, 0, 0);

            OutputText("Scenes.Faerie.FaerieShouldraDick.ShouldraPipesUp".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.ShouldraVoice".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.FaerieInHand".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.DidItNotWork".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.LittleFaeComplains".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.SmallerOne".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.QuestionGoodIdea".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.SecondWave".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.StageIsYours".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.YouGrin".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.LowerOntoCock".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.WaitTimeOut".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.FaceContorted".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.OutlineVisible".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.SpasmingComes".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.TinyFeetPress".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.ChildishChimes".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.DontMindMe".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.WatchNoseChange".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.NarrowThrusts".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.SoakedInJuices".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.NothingHappens".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.YouPanic".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.Rerouting".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.FloodgatesOpen".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.ThatWasAwesome".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.CanYouAgain".Localized());

            OutputText("Scenes.Faerie.FaerieShouldraDick.YouFragment".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieHoneyFeed()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.FaerieHoneyFeed.ParticularIdea".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.ScareyFace".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.DontAnswer".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.WaitIsThatAll".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.CutHerOff".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.RetractDigit".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.PrettyGoodAgain".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.BetterUse".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.FeelFunny".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.SlurringWords".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.WhaddreYouDoing".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.EyesHaze".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.NoooStop".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.ProtestationsEasier".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.LegsJitter".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.BottleClatters".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.Uhhhh".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.SetOnBranch".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.ThankshFriend".Localized());

            OutputText("Scenes.Faerie.FaerieHoneyFeed.MockSalute".Localized());

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BeeHoney));
            else
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney));
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SpecialHoney));

            DoNext(() => ReturnToCamp(1));
        }

        public void HeyListen()
        {
            ClearOutput();

            OutputText("Scenes.Faerie.HeyListen.TrudgingInjured".Localized());

            OutputText("Scenes.Faerie.HeyListen.FaeYelps".Localized());

            OutputText("Scenes.Faerie.HeyListen.FurrowBrow".Localized());

            OutputText("Scenes.Faerie.HeyListen.StaresBlankly".Localized());

            OutputText("Scenes.Faerie.HeyListen.RubForehead".Localized());

            DoNext(() => ReturnToCamp(1));
        }

    }
}
