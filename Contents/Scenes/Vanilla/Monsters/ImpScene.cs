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
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Monsters
{
    [Register<ImpScene>("coc:imp_scene")]
    public class ImpScene : Scene
    {

        public static Enemy? monster => CombatController.Instance?.Context.Enemy;

        public static SaveContent Save => SaveContent(SceneIds.Monsters.Imp);

        public static bool ButtVirgin() => Player.Body.Get<AnalPart>().Looseness == 0;

        public static bool IsLoli() => Player.IsChild() && Player.HasVagina();

        public static bool WillCuntBeStretched(float cockArea) =>

            Player.Vaginas.Count > 0 &&

            Player.Vaginas[0].WillStretch(cockArea, Player.HasPerk(PerkIds.FerasBoonMilkingTwat), Player.VaginalCapacityBonus());

        public static Enemy Enemy => CombatController.Instance!.Context.Enemy;

        public override void Entry()
        {

        }

        public void GenerateImpSexMenu()
        {

            ClearButtons();

            AddButtonDisabled(0, "Male Rape", "");

            AddButtonDisabled(1, "Female Rape", "");

            AddButtonDisabled(2, "NippleFuck", "");

            AddButtonDisabled(3, "Breastfeed", "");

            AddButtonDisabled(4, "Use Condom", "");

            AddButtonDisabled(5, "Oviposit", "");

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephila))
                AddButtonDisabled(7, "Unbirth Him", "");

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Impscene.GenerateImpSexMenu.NeedsNotMet".Localized());

                if (Player.HasCock())

                    if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) == -1)
                        AddButtonDisabled(0, "Male Rape", "");
                    else
                    {
                        AddButton(0, Player.IsTaur() ? "Common.Text.CentaurRape".Localized() : "Common.Button.MaleRape".Localized(),

                            Player.IsTaur() ? CentaurOnImpStart : () => RapeImpWithDick());

                        if (OptionStore.Instance.Get("watersports", 0) > 0 && !Player.IsTaur())
                            AddButton(5, "Common.Button.AssfuckPiss".Localized(), PissDom);
                    }

                if (Player.HasVagina())
                    if (Player.IsTaur())
                        AddButton(1, "Common.Button.GroupVaginal".Localized(), CentaurGirlOnImps);
                    else
                        AddButton(1, "Common.Button.FemaleRape".Localized(), RapeImpWithPussy);

                if (Player.HasFuckableNipples())
                    AddButton(2, "Common.Button.NippleFuck".Localized(), NoogaisNippleRape);

                if (Player.HasVagina() && Player.BiggestBreastSize() >= 4 && false)
                    AddButton(6, "Common.Button.Titfuck".Localized(), null);

                if (Player.CockThatFits(Enemy.AnalCapacity()) != -1 && !Player.IsTaur() && Player.HasItem(ItemRegistry.Create(ItemIds.Condom)))
                    AddButton(4, "Scenes.Goblin.Button.UseCondom".Localized(), () => RapeImpWithDick(true));

                var milkTotal = Player.ItemCount(ConsumableIds.SuccubiMilk) + Player.ItemCount(ConsumableIds.PurifiedSuccubiMilk);

                AddNextButton("Common.Button.Feminize".Localized(), ImpFeminization);
            }
            else
                OutputText("Scenes.Impscene.GenerateImpSexMenu.NotHornyEnoughBold".Localized());

            if (Player.LactationQ() >= 500 || Player.HasStatusEffect(StatusEffectIds.Feeder))
                AddButton(3, "Common.Button.Breastfeed".Localized(), AreImpsLactoseIntolerant);

            if (Player.CanOvipositBee())
                AddButton(5, "Common.Button.Oviposit".Localized(), PutBeeEggsInAnImpYouMonster);

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                AddButton(7, "Common.Button.UnbirthHim".Localized(), ImpFunWithUnbirthing);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), null);

            AddKillPetrifyButtons(12, 13);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void ImpVictory()
        {
            ClearOutput();

            OutputText("Scenes.Impscene.ImpVictory.CollapsesMasturbating".Localized());

            GenerateImpSexMenu();
        }

        public void ImpMultiVictory(bool lustVictory, int impAmount)
        {
            ClearOutput();

            if (impAmount == 1)
            {
                ImpVictory();

                return;
            }

            RegisterMacro("impAmount", impAmount);

            RegisterMacro("lustVictory", lustVictory);

            OutputText("Scenes.Impscene.ImpMultiVictory.ImpsFallVictoryCollapsing".Localized());

            GenerateImpSexMenu();
        }

        public void MultiImpSpitroastLoss(bool lustVictory, int impAmount)
        {
            ClearOutput();

            RegisterMacro("impAmount", impAmount);

            RegisterMacro("lustVictory", lustVictory);

            RegisterMacro("impCock", Enemy.Cocks[0].Length);

            RegisterMacro("stretched", WillCuntBeStretched(Enemy.CockArea(0)));

            RegisterMacro("wasVirgin", Player.HasVagina()

                ? Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin

                : ButtVirgin());

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.PouncesLapseInResistance".Localized());

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.TakeEndDeliriousBegin".Localized());

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.HierarchyAchievedDemonHips".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, true);

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.UnisonSighChastityHole".Localized());

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.GagShaftJamsSaliva".Localized());

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.OnceMoreAssRaped".Localized());

            OutputText("Scenes.Impscene.MultiImpSpitroastLoss.AwakeHoursSoreSpattered".Localized());

            Player.Orgasm("VaginalAnal");

            CombatScene.CleanupAfterCombat();
        }

        public void ImpFeminization()
        {
            ClearOutput();

            OutputText("Scenes.Impscene.ImpFeminization.StareDownThrashedImp".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.SteppingCloserChestConsider".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.WheezesReliefWeightLungs".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.CreamyTransformativeHitsTongue".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.GaggingFadesHeavySwallows".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.LittleSobEmptyBottle".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.WastingNoStripGear".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.HowlsStopGoEyes".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.FedResistanceGreaterStrength".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.WhorishRedNoTrouble".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.ReleasingWristsWideHips".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.WhatSlutTurningOut".Localized());

            OutputText("Scenes.Impscene.ImpFeminization.MuskyScentThickensQuickening".Localized());

            if (Player.CumQ() >= 750)
            {
                OutputText("Scenes.Impscene.ImpFeminization.BallsBlastRopePressure".Localized());

                OutputText("Scenes.Impscene.ImpFeminization.GroansHoldsStomachOvers".Localized());
            }
            else
            {
                OutputText("Scenes.Impscene.ImpFeminization.BallsBlastRopeWomb".Localized());

                OutputText("Scenes.Impscene.ImpFeminization.GroansHoldsStomachThin".Localized());
            }

            OutputText("Scenes.Impscene.ImpFeminization.LeaveLatestDirtSatisfied".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.Orgasm("Dick");

            for (var i = 0; i < 2; i++)
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                    Player.DestroyItems(ConsumableIds.SuccubiMilk, 1);
                else
                    Player.DestroyItems(ConsumableIds.PurifiedSuccubiMilk, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void RapeImpWithDick(bool condomed = false)
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            if (condomed)
            {
                Player.DestroyItems(ItemIds.Condom, 1);

                OutputText("Scenes.Impscene.RapeImpWithDick.UnwrapCondomSlideLatex".Localized(
                    Player.CockDescript(x)));
            }

            ShowImage("monster-imp");

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Impscene.RapeImpWithDick.DemonicSmileInsensibleNeck".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Impscene.RapeImpWithDick.UnclaspRevealPlaceholder".Localized(
                        Player.CockDescript(x)));

                    if (Player.BreastRows.Count > 0 && Player.BreastRows[0].BreastRating > 2)
                        OutputText("Scenes.Impscene.RapeImpWithDick.SmashingBreastsJerkHard".Localized(
                            Player.CockDescript(x)));
                    else
                        OutputText("Scenes.Impscene.RapeImpWithDick.StrokingFullHardnessLanguidly".Localized());
                }

                OutputText("Scenes.Impscene.RapeImpWithDick.NoPressPuckerRam".Localized(
                    Player.CockDescript(x),

                    CombatController.Instance!.Context.Enemy.CockDescriptShort(0)));

                if (Player.Cocks[0].Id == BodyPartTypes.Cat)
                    OutputText("Scenes.Impscene.RapeImpWithDick.TinyClawsSidesBarbs".Localized());

                if (Player.Cocks[0].Length >= 7 && Player.Cocks[0].Length <= 12)
                    OutputText("Scenes.Impscene.RapeImpWithDick.EachThrustDistortsAbdomen".Localized());

                if (Player.Cocks[0].Length > 12)
                    OutputText("Scenes.Impscene.RapeImpWithDick.EachDistortsBodyBulging".Localized());

                OutputText("Scenes.Impscene.RapeImpWithDick.TightConfinesAssOrgasm".Localized());

                if (Player.Balls == 0 && Player.Vaginas.Count > 0)
                    OutputText("Scenes.Impscene.RapeImpWithDick.CumBoilsOutSoaks".Localized(Player.VaginaDescript(0), (condomed ? "Common.Text.InsideYourCondom".Localized() : "Common.Text.DeepIntoTheImpsRectum".Localized())));

                if (Player.Balls == 0 && Player.Vaginas.Count == 0)
                    OutputText("Scenes.Impscene.RapeImpWithDick.CumBoilsOutFlowing".Localized(Player.CockDescript(x), (condomed ? "Common.Text.InsideYourCondom".Localized() : "Common.Text.DeepIntoTheImpsRectum".Localized())));

                if (Player.CumQ() >= 14 && Player.CumQ() <= 30)
                    OutputText("Scenes.Impscene.RapeImpWithDick.OrgasmDragsSlickJism".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.CumQ() > 30 && Player.CumQ() <= 100)
                    OutputText("Scenes.Impscene.RapeImpWithDick.OrgasmLastsJizzDripping".Localized((condomed ? "Common.Text.YourCondom".Localized() : "Common.Text.TheImpsAsshole".Localized()), Player.CockDescript(x)));

                if (Player.CumQ() > 100)
                    OutputText("Scenes.Impscene.RapeImpWithDick.OrgasmGrowsIntenseSpurt".Localized(
                        condomed ? "Your condom swells nearly to the point of bursting, and tiny jets of cum squirt out around your " + Player.CockDescript(x) + "Common.Text.WithEachThrust".Localized() : "The imp begins to look slightly pregnant as you fill him, and tiny jets of cum squirt out around your " + Player.CockDescript(x) + "Common.Text.WithEachThrust".Localized()
                    ));

                OutputText("Scenes.Impscene.RapeImpWithDick.SatisfiedLastPullOff".Localized((condomed ? "Common.Text.Semen".Localized() : "Common.Text.MixedFluids".Localized())));
            }

            if (Player.Cocks.Count >= 2)
            {
                OutputText("Scenes.Impscene.RapeImpWithDick.DemonicSmileGrabAgain".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Impscene.RapeImpWithDick.CasuallyUnclaspRevealCocks".Localized());

                    if (Player.BreastRows.Count > 0 && Player.BreastRows[0].BreastRating > 2)
                        OutputText("Scenes.Impscene.RapeImpWithDick.SmashingBreastsJerkMember".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Impscene.RapeImpWithDick.StrokingMemberFullHardness".Localized());
                }

                OutputText("Scenes.Impscene.RapeImpWithDick.NoForeplayPressPucker".Localized(
                    Player.CockDescript(x),

                    CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

                if (Player.Cocks[0].Length >= 7 && Player.Cocks[0].Length <= 12)
                    OutputText("Scenes.Impscene.RapeImpWithDick.EachDistortsAbdomenAgain".Localized());

                if (Player.Cocks[0].Length > 12 && Player.Cocks[0].Length <= 18)
                    OutputText("Scenes.Impscene.RapeImpWithDick.EachDistortsBodyAgain".Localized());

                OutputText("Scenes.Impscene.RapeImpWithDick.TightConfinesOrgasmAgain".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Impscene.RapeImpWithDick.CumBoilsHeatSpreading".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.Balls == 0 && Player.Vaginas.Count > 0)
                    OutputText("Scenes.Impscene.RapeImpWithDick.CumBoilsSoaksThird".Localized(Player.VaginaDescript(0), (condomed ? "Common.Text.InsideYourCondom".Localized() : "Common.Text.DeepIntoTheImpsRectum".Localized())));

                if (Player.Balls == 0 && Player.Vaginas.Count == 0)
                    OutputText("Scenes.Impscene.RapeImpWithDick.CumBoilsFlowingFourth".Localized(Player.CockDescript(x), (condomed ? "Common.Text.InsideYourCondom".Localized() : "Common.Text.DeepIntoTheImpsRectum".Localized())));

                if (Player.CumQ() >= 14 && Player.CumQ() <= 30)
                    OutputText("Scenes.Impscene.RapeImpWithDick.OrgasmDragsJismAgain".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.CumQ() > 30 && Player.CumQ() <= 100)
                    OutputText("Scenes.Impscene.RapeImpWithDick.OrgasmLastsForeverAgain".Localized((condomed ? "Common.Text.YourCondom".Localized() : "Common.Text.TheImpsAsshole".Localized()), Player.CockDescript(x)));

                if (Player.CumQ() > 100)
                    OutputText("Scenes.Impscene.RapeImpWithDick.OrgasmGrowsIntenseAgain".Localized(
                        condomed ? "Your condom swells nearly to the point of bursting, and tiny jets of cum squirt out around your " + Player.CockDescript(x) + "Common.Text.WithEachThrust".Localized() : "The imp begins to look slightly pregnant as you fill him, and tiny jets of cum squirt out around your " + Player.CockDescript(x) + "Common.Text.WithEachThrust".Localized()
                    ));

                OutputText("Scenes.Impscene.RapeImpWithDick.SatisfiedLastPullAgain".Localized());
            }

            Player.Orgasm("Dick");

            if (!condomed)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void RapeImpWithPussy()
        {
            ClearOutput();

            ShowImage("imp-win-female-fuck");

            Player.SlimeFeed();

            if (Player.VaginalCapacity() < Enemy.CockArea(0) && IsLoli())
            {
                OutputText("Scenes.Impscene.RapeImpWithPussy.ShedSecondThoughtApproach".Localized());

                OutputText("Scenes.Impscene.RapeImpWithPussy.StraddlingStandBulgingCock".Localized());

                OutputText("Scenes.Impscene.RapeImpWithPussy.BreathReliefKneelGrind".Localized());

                OutputText("Scenes.Impscene.RapeImpWithPussy.LoseTrackLanguidlyPump".Localized());
            }
            else
            {
                OutputText("Scenes.Impscene.RapeImpWithPussy.ShedArmorThoughtLooming".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                {
                    OutputText("Scenes.Impscene.RapeImpWithPussy.FrownPushTooLarge".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0),

                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    if (Player.BreastRows.Count > 0 && Player.BreastRows[0].BreastRating > 1)
                        OutputText("Scenes.Impscene.RapeImpWithPussy.HappilyTugErectNipples".Localized());

                    OutputText("Scenes.Impscene.RapeImpWithPussy.LoseTrackPumpAgain".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                        Player.VaginaDescript(0),

                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));
                }
                else
                {
                    OutputText("Scenes.Impscene.RapeImpWithPussy.SinkDownSlowlyDelighting".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                    OutputText("Scenes.Impscene.RapeImpWithPussy.LustDesireSpursBounce".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Impscene.RapeImpWithPussy.TwoLayRecoverSeparate".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);
                }
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void SprocketImp()
        {
            ClearOutput();

            Player.SlimeFeed();

            ShowImage("monster-imp");

            OutputText("Scenes.Impscene.SprocketImp.FallKneesLostThoughts".Localized());

            OutputText("Scenes.Impscene.SprocketImp.SurpriseFluttersUpwardRushes".Localized());

            if (Player.Body.Get<HairPart>().Length > 0)
                OutputText("Scenes.Impscene.SprocketImp.ClawsDigIntoHair".Localized());
            else
                OutputText("Scenes.Impscene.SprocketImp.ClawsDigIntoWrists".Localized());

            OutputText("Scenes.Impscene.SprocketImp.DraggedUpwardSoaringTrees".Localized());

            OutputText("Scenes.Impscene.SprocketImp.PullsRedThrobbingCock".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Human || Player.Cocks[0].Id == BodyPartTypes.Demon || Player.Cocks[0].TypeIndex > 4)
                OutputText("Scenes.Impscene.SprocketImp.CockRubbingTipOwn".Localized());
            else
                if (Player.HasKnot(0))
                    OutputText("Scenes.Impscene.SprocketImp.CockRubbingTipPoint".Localized());
                else
                    if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Impscene.SprocketImp.CockRubbingFlaredHead".Localized());
                    else
                        if (Player.Cocks[0].Id == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Impscene.SprocketImp.HugeGreenCockHead".Localized());

            OutputText("Scenes.Impscene.SprocketImp.SmearingPreJerkingBoth".Localized());

            if (Player.Cocks[0].Id == BodyPartTypes.Human || Player.Cocks[0].Id == BodyPartTypes.Demon || Player.Cocks[0].TypeIndex > 4)
                OutputText("Scenes.Impscene.SprocketImp.ShovesShaftDeeplySlit".Localized());
            else
                if (Player.HasKnot(0))
                    OutputText("Scenes.Impscene.SprocketImp.FindsPointedHeadPlunges".Localized());
                else
                    if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Impscene.SprocketImp.SeatsFlaredHeadPushes".Localized());
                    else
                        if (Player.Cocks[0].Id == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Impscene.SprocketImp.ShovesDickVineLike".Localized());

            OutputText("Scenes.Impscene.SprocketImp.ParagraphBreak".Localized());

            OutputText("Scenes.Impscene.SprocketImp.GripsCockTightlyFucks".Localized());

            if (Player.HasFur())
                OutputText("Scenes.Impscene.SprocketImp.FurryCockSleeve".Localized());
            else
                if (Player.Body.Get<SkinPart>().Tone == "purple" || Player.Body.Get<SkinPart>().Tone == "blue" || Player.Body.Get<SkinPart>().Tone == "shiny black")
                    OutputText("Scenes.Impscene.SprocketImp.DemonicCockSleeve".Localized());
                else
                    OutputText("Scenes.Impscene.SprocketImp.HumanCockSleeve".Localized());

            if (Player.BreastRows.Count > 0 && Player.BiggestBreastSize() > 2)
                OutputText("Scenes.Impscene.SprocketImp.FuckingHardBounceThrust".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.Impscene.SprocketImp.SentencePeriodSpace".Localized());

            OutputText("Scenes.Impscene.SprocketImp.BrieflyCrossesMindPainful".Localized());

            OutputText("Scenes.Impscene.SprocketImp.ThrustsRoughlyMinutesBucking".Localized());

            if (Player.Cocks.Count == 2)
                OutputText("Scenes.Impscene.SprocketImp.OtherCockFindingPleasure".Localized());

            if (Player.Cocks.Count > 2)
                OutputText("Scenes.Impscene.SprocketImp.OtherCocksFindingPleasure".Localized());

            OutputText("Scenes.Impscene.SprocketImp.CopiousSweatRunsShivers".Localized());

            if (Player.Cocks.Count == 2)
                OutputText("Scenes.Impscene.SprocketImp.OtherCumsSameTime".Localized());

            if (Player.Cocks.Count > 2)
                OutputText("Scenes.Impscene.SprocketImp.RestCocksTwitchRelease".Localized());

            if (Player.BiggestLactation() >= 1)
                OutputText("Scenes.Impscene.SprocketImp.MilkBurstsNipplesFace".Localized());

            if (Player.Vaginas.Count > 0)
                OutputText("Scenes.Impscene.SprocketImp.PussyQuiversContractingOrgasm".Localized());

            OutputText("Scenes.Impscene.SprocketImp.SatisfiedSlidesFliesAway".Localized());

            Player.Health -= 10;

            if (Player.Health < 1)
                Player.Health = 1;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CentaurGirlOnImps()
        {
            ClearOutput();

            ShowImage("player-centaur-female");

            OutputText("Scenes.Impscene.CentaurGirlOnImps.StandDefeatedAmusingIdea".Localized());

            OutputText("Scenes.Impscene.CentaurGirlOnImps.GoblinAppearsSwiftKick".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Impscene.CentaurGirlOnImps.ApproachGenitaliaGlistening".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.HandsSlideStrokingEquine".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.LargerSmacksThickFinger".Localized(
                    Player.AssDescript()
                ));

                OutputText("Scenes.Impscene.CentaurGirlOnImps.HeadComesAroundVision".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.LeaderLooksGivesSignal".Localized());

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                OutputText("Scenes.Impscene.CentaurGirlOnImps.ParagraphBreak".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.DisappearsBehindSlapHaunches".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.EdgeClearingLeaderLaughing".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.UnexpectedRunningPausedCocks".Localized(
                    Player.AssDescript()
                ));

                OutputText("Scenes.Impscene.CentaurGirlOnImps.SteadyYourselfRunFaster".Localized());

                if (Player.BiggestBreastSize() > 1)
                    OutputText("Scenes.Impscene.CentaurGirlOnImps.HandsReflexivelyChestMash".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.OwnerDogInsertKnot".Localized());

                if (Player.BiggestBreastSize() > 1)
                {
                    OutputText("Scenes.Impscene.CentaurGirlOnImps.SlidesTorsoGraspsFlailing".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Impscene.CentaurGirlOnImps.FingersSlideInsideNipples".Localized());
                    else
                        OutputText("Scenes.Impscene.CentaurGirlOnImps.FingersGropeGrabNipples".Localized());

                    if (Player.BiggestLactation() >= 1 && Player.LactationQ() < 50)
                        OutputText("Scenes.Impscene.CentaurGirlOnImps.MilkDribblesSquirtsPuddles".Localized());
                    else
                        if (Player.BiggestLactation() >= 1)
                            OutputText("Scenes.Impscene.CentaurGirlOnImps.MilkSpraysMassivePuddles".Localized());

                    OutputText("Scenes.Impscene.CentaurGirlOnImps.ParagraphBreakThird".Localized());
                }

                OutputText("Scenes.Impscene.CentaurGirlOnImps.StopRunningSprayingDirt".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.TrotLeaderSleeve".Localized());

                OutputText("Scenes.Impscene.CentaurGirlOnImps.CollectingThingsLookStumble".Localized());

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

                Player.Orgasm("Vaginal");

                Player.Orgasm("Anal", false);

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
            }

            CombatScene.CleanupAfterCombat();
        }

        public void CentaurOnImpStart()
        {
            ClearOutput();

            OutputText("Scenes.Impscene.CentaurOnImpStart.AsImpCollapsesFront".Localized());

            if (CombatController.Instance!.Context.Enemy.Health != 0)
                OutputText("Scenes.Impscene.CentaurOnImpStart.PantingInExhaustion".Localized());
            else
                OutputText("Scenes.Impscene.CentaurOnImpStart.MasturbatingFuriously".Localized());

            OutputText("Scenes.Impscene.CentaurOnImpStart.AdvancePoorCreatureEyes".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Impscene.CentaurOnImpStart.LickLipsSlightlyApproach".Localized());
            else
                OutputText("Scenes.Impscene.CentaurOnImpStart.LickLipsObscenelyApproach".Localized());

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x >= 0 && !Player.HasVagina())
                CentaurOnImpMale();
            else
                if (Player.HasVagina() && x < 0)
                    CentaurOnImpFemale();
                else
                {
                    OutputText("Scenes.Impscene.CentaurOnImpStart.FocusMalenessGirlParts".Localized());

                    ClearButtons();

                    AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), () => CentaurOnImpMale(true));

                    AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), () => CentaurOnImpFemale(true));
                }
        }

        public void CentaurOnImpMale(bool vape = false)
        {
            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            if (vape)
                ClearOutput();

            if (Player.Gender == Gender.Hermaphrodite)
                ShowImage("player-centaur-herm");
            else
                ShowImage("player-centaur-male");

            OutputText("Scenes.Impscene.CentaurOnImpMale.ShadowFallsHintFear".Localized());

            if (Player.CockTotal() > 1 && Player.Cocks[Player.BiggestCockIndex()].Length >= 24)
            {
                OutputText("Scenes.Impscene.CentaurOnImpMale.ShadowFallsLooksFear".Localized(
                    Player.CockDescript(Player.BiggestCockIndex())
                ));

                if (Player.Corruption < 50)
                {
                    OutputText("Scenes.Impscene.CentaurOnImpMale.SlowlyUndulateAgainstMouth".Localized(
                        Player.CockDescript(Player.BiggestCockIndex())
                    ));

                    CentaurOnImpResults(3);

                    OutputText("Scenes.Impscene.CentaurOnImpMale.SoundsBeneathIntenseNote".Localized(
                        Player.CockDescript(Player.BiggestCockIndex())
                    ));

                    CentaurOnImpResults(2);

                    Player.Orgasm("Dick");

                    CombatScene.CleanupAfterCombat();

                    return;
                }
                else
                    if (Player.CockTotal() == 2)
                    {
                        OutputText("Scenes.Impscene.CentaurOnImpMale.EvilSmileWaitLips".Localized(
                            Player.CockDescript(Player.SmallestCockIndex()),

                            Player.CockDescript(Player.SmallestCockIndex()),

                            Player.CockDescript(Player.SmallestCockIndex())
                        ));

                        OutputText("Scenes.Impscene.CentaurOnImpMale.ImpFocusedTentaclesAir".Localized(
                            Player.CockDescript(Player.BiggestCockIndex()),

                            CombatController.Instance!.Context.Enemy.AssholeDescript(),

                            CombatController.Instance!.Context.Enemy.AssholeDescript(),

                            Player.CockDescript(Player.SmallestCockIndex())
                        ));

                        OutputText("Scenes.Impscene.CentaurOnImpMale.NotLongStrugglesSlowing".Localized());

                        if (Player.Corruption < 80)
                        {
                            OutputText("Scenes.Impscene.CentaurOnImpMale.FeelingMercifulFlipRock".Localized(
                                Player.CockDescript(Player.BiggestCockIndex()),

                                CombatController.Instance!.Context.Enemy.AssholeDescript()
                            ));

                            CentaurOnImpResults(1);

                            Player.Orgasm("Dick");

                            CombatScene.CleanupAfterCombat();

                            return;
                        }
                        else
                        {
                            OutputText("Scenes.Impscene.CentaurOnImpMale.GroanPleasureSlideDeeper".Localized(
                                Player.CockDescript(Player.BiggestCockIndex())
                            ));

                            if (Player.Balls > 0)
                                OutputText("Scenes.Impscene.CentaurOnImpMale.BallsParagraphBreak".Localized());
                            else
                                OutputText("Scenes.Impscene.CentaurOnImpMale.GroinParagraphBreak".Localized());

                            CentaurOnImpResults(3);

                            OutputText("Scenes.Impscene.CentaurOnImpMale.GutturalMoanPassedOut".Localized());

                            if (Player.HasFuckableNipples())
                                OutputText("Scenes.Impscene.CentaurOnImpMale.ShovingFingersDeepNipples".Localized());
                            else
                                OutputText("Scenes.Impscene.CentaurOnImpMale.FierceTugOnNipples".Localized());

                            OutputText("Scenes.Impscene.CentaurOnImpMale.BeginCumDeepStomach".Localized(
                                CombatController.Instance!.Context.Enemy.AssholeDescript()
                            ));

                            if (Player.CumQ() > 250)
                                OutputText("Scenes.Impscene.CentaurOnImpMale.BellyDistendingFeelSome".Localized());

                            OutputText("Scenes.Impscene.CentaurOnImpMale.LastGruntExtractingTentacles".Localized(
                                Player.CockDescript(Player.SmallestCockIndex())
                            ));

                            Player.Orgasm("Dick");

                            CombatScene.CleanupAfterCombat();

                            return;
                        }
                    }
                    else
                    {
                        OutputText("Scenes.Impscene.CentaurOnImpMale.EvilSmileTouchTentacle".Localized(
                            Player.CockDescript(Player.BiggestCockIndex())
                        ));

                        OutputText("Scenes.Impscene.CentaurOnImpMale.LickingLipsAnticipationPush".Localized(
                            Player.CockDescript(Player.BiggestCockIndex()),

                            CombatController.Instance!.Context.Enemy.AssholeDescript(),

                            CombatController.Instance!.Context.Enemy.AssholeDescript(),

                            Player.CockDescript(Player.BiggestCockIndex())
                        ));

                        if (Player.BiggestBreastSize() >= 0)
                        {
                            OutputText("Scenes.Impscene.CentaurOnImpMale.OneHandReleasesPlaying".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));

                            if (Player.HasFuckableNipples())
                                OutputText("Scenes.Impscene.CentaurOnImpMale.AndFingeringNipples".Localized());

                            OutputText("Scenes.Impscene.CentaurOnImpMale.DroolSlightlyAbsolutePleasure".Localized());
                        }

                        OutputText("Scenes.Impscene.CentaurOnImpMale.NoisesLessenSloppySounds".Localized());

                        CentaurOnImpResults(1);

                        Player.Orgasm("Dick");

                        CombatScene.CleanupAfterCombat();

                        return;
                    }
            }

            if (Player.CockArea(x) <= 15)
            {
                OutputText("Scenes.Impscene.CentaurOnImpMale.ReliefWashesIntenseLust".Localized(
                    CombatController.Instance!.Context.Enemy.AssholeDescript()
                ));

                if (Player.Corruption < 50)
                    OutputText("Scenes.Impscene.CentaurOnImpMale.SlowlyRubBetweenCheeks".Localized(
                        Player.CockDescript(x),

                        CombatController.Instance!.Context.Enemy.AssholeDescript(),

                        Player.CockDescript(x),

                        CombatController.Instance!.Context.Enemy.AssholeDescript(),

                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Impscene.CentaurOnImpMale.PositionAgainstDryAnus".Localized(
                        Player.CockDescript(x)
                    ));

                CentaurOnImpResults(1);

                Player.Orgasm("Dick");

                CombatScene.CleanupAfterCombat();

                return;
            }
            else
                if (Player.Corruption < 50)
                {
                    OutputText("Scenes.Impscene.CentaurOnImpMale.ImpEyesApprehensionFlee".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    if (Player.Cocks[0].Thickness > 3)
                    {
                        OutputText("Scenes.Impscene.CentaurOnImpMale.NotLongTongueUrethra".Localized());

                        if (Player.Balls > 0)
                            OutputText(Player.BallsDescriptLight());
                        else
                            OutputText("Scenes.Impscene.CentaurOnImpMale.Prostate".Localized());

                        OutputText("Scenes.Impscene.CentaurOnImpMale.FeelForeignInvaderWiggling".Localized());
                    }

                    OutputText("Scenes.Impscene.CentaurOnImpMale.CannotTakeHoovesScraping".Localized(
                        Player.CockDescript(x)
                    ));

                    CentaurOnImpResults(2);

                    Player.Orgasm("Dick");

                    CombatScene.CleanupAfterCombat();

                    return;
                }
                else
                {
                    OutputText("Scenes.Impscene.CentaurOnImpMale.ImpEyesApprehensionTurn".Localized(
                        Player.CockDescript(x),

                        CombatController.Instance!.Context.Enemy.AssholeDescript(),

                        Player.CockDescript(x)
                    ));

                    CentaurOnImpResults(1);

                    Player.Orgasm("Dick");

                    CombatScene.CleanupAfterCombat();

                    return;
                }

        }

        public void CentaurOnImpFemale(bool vape = false)
        {
            ClearOutput();

            Player.SlimeFeed();

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

            ShowImage("player-centaur-herm");

            OutputText("Scenes.Impscene.CentaurOnImpFemale.ImpLaysHandsStroke".Localized(
                CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                Player.VaginaDescript(0)
            ));

            if (Player.Vaginas[0].Looseness <= 3)
            {
                OutputText("Scenes.Impscene.CentaurOnImpFemale.LasciviousGrinHopsForward".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Virgin)
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.CryVirginalPussyTorn".Localized());
                else
                {
                    if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.GroansDelightTightnessShoves".Localized());

                    if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) && Player.VaginalCapacity() <= CombatController.Instance!.Context.Enemy.CockArea(0) * 1.25f)
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.PleasedSoundSlidesDeeply".Localized(
                            Player.VaginaDescript(0)
                        ));

                    if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) * 1.25f)
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.DickSlidesEasilySlopping".Localized());
                }

                OutputText("Scenes.Impscene.CentaurOnImpFemale.BottomsOutTrottingForward".Localized(
                    CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0),

                    CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                Player.Orgasm("Vaginal");

                CombatScene.CleanupAfterCombat();

                return;
            }
            else
            {
                OutputText("Scenes.Impscene.CentaurOnImpFemale.LasciviousHopsForwardAgain".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Corruption >= 50)
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.TakeComfortInsideSoon".Localized());

                OutputText("Scenes.Impscene.CentaurOnImpFemale.ImpSharedStrangeHard".Localized(
                    Player.VaginaDescript(0),

                    CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                    Player.VaginaDescript(0)
                ));

                if (Player.Corruption < 80)
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.TearsPourBleedingSlightly".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.HangTreeAfraidPain".Localized());
                else
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.HangTreeGruntingDelight".Localized());

                if (Player.Corruption >= 80)
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.HowlPainBuckingIntensify".Localized());

                if (Player.Corruption >= 50 && Player.BiggestBreastSize() >= 2)
                {
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.ReleaseTreeBeginPlaying".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.ShovingFingersIntoNipple".Localized());
                    else
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.SentencePeriodSpace".Localized());
                }

                OutputText("Scenes.Impscene.CentaurOnImpFemale.PushingDeeperCryFirst".Localized(
                    CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

                if (Player.AnalCapacity() >= 35)
                {
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.RealizesCannotFurtherAsshole".Localized());

                    if (Player.Anal.Looseness < 4 && Player.Corruption < 80)
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.HowlPainThroatRaw".Localized());
                    else
                        OutputText("Scenes.Impscene.CentaurOnImpFemale.HowlPerverseThroatRaw".Localized());
                }

                OutputText("Scenes.Impscene.CentaurOnImpFemale.ReliefSpermFillingWomb".Localized());

                if (Player.Corruption < 80)
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.PromiseYourselfNotAgain".Localized());
                else
                    OutputText("Scenes.Impscene.CentaurOnImpFemale.FindJuicesDrippingAnticipation".Localized());

                Player.Orgasm("Vaginal");

                CombatScene.CleanupAfterCombat();

                return;
            }
        }

        public void CentaurOnImpResults(float iNum)
        {

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            if (iNum == 1)
            {
                if (Player.CumQ() >= 250)
                {
                    if (!Player.HasKnot(x))
                        OutputText("Scenes.Impscene.CentaurOnImpResults.AmountAbusedNoSign".Localized(
                            CombatController.Instance!.Context.Enemy.AssholeDescript()
                        ));
                    else
                        OutputText("Scenes.Impscene.CentaurOnImpResults.AbusedFullBrimKnot".Localized(
                            CombatController.Instance!.Context.Enemy.AssholeDescript()
                        ));

                    OutputText("Scenes.Impscene.CentaurOnImpResults.FinallyEmergesTorrentDistended".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Impscene.CentaurOnImpResults.OtherCockDrenchesSecretions".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Impscene.CentaurOnImpResults.OtherCocksReleaseGlazed".Localized());

                    OutputText("Scenes.Impscene.CentaurOnImpResults.LeavePantingLappingPool".Localized());
                }
                else
                {
                    OutputText("Scenes.Impscene.CentaurOnImpResults.LastThrustReceptacleSpinning".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Impscene.CentaurOnImpResults.OtherDrenchesSecretionsAgain".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Impscene.CentaurOnImpResults.OtherCocksGlazedAgain".Localized());

                    OutputText("Scenes.Impscene.CentaurOnImpResults.LeavePantingDrapedBoulder".Localized());
                }

                return;
            }

            if (iNum == 2)
            {
                if (Player.CumQ() >= 250)
                {
                    OutputText("Scenes.Impscene.CentaurOnImpResults.EyesWidenPouringGobs".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Impscene.CentaurOnImpResults.NoMatterBellyDistends".Localized());
                }

                OutputText("Scenes.Impscene.CentaurOnImpResults.SatedTrotLickingLips".Localized());

                return;
            }

            if (iNum == 3)
            {
                if (Player.BiggestBreastSize() >= 2)
                {
                    OutputText("Scenes.Impscene.CentaurOnImpResults.IntensifyReachMassaging".Localized());

                    if (Player.HasFuckableNipples())
                    {
                        if (Player.BiggestLactation() >= 1)
                            OutputText("Scenes.Impscene.CentaurOnImpResults.MilkStreamsFilledCum".Localized());
                        else
                            OutputText("Scenes.Impscene.CentaurOnImpResults.FingersSlideNipplesStroke".Localized());
                    }
                    else
                        if (Player.BiggestLactation() > 0)
                            if (Player.BiggestLactation() <= 1)
                                OutputText("Scenes.Impscene.CentaurOnImpResults.BeadsMilkDripSpurt".Localized());
                            else
                                OutputText("Scenes.Impscene.CentaurOnImpResults.MilkPoursStreamsBody".Localized());
                }

                return;
            }
        }

        public void AreImpsLactoseIntolerant()
        {
            ClearOutput();

            ShowImage("monster-imp");

            OutputText("Scenes.Impscene.AreImpsLactoseIntolerant.MasturbatingBaringSwinging".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Impscene.AreImpsLactoseIntolerant.SitGrabHairSqueals".Localized());

            OutputText("Scenes.Impscene.AreImpsLactoseIntolerant.AfterTakesHandAgainst".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Impscene.AreImpsLactoseIntolerant.FeelGulpsSlowRelief".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f), new DynStat(GameStats.Lust, -50f));

            Player.Milked();

            CombatScene.CleanupAfterCombat();
        }

        public void ImpGangGetsWhooped()
        {
            ClearOutput();

            OutputText("Scenes.Impscene.ImpGangGetsWhooped.ImpsDefeatedBodiesGems".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Sleeping = true;

        }

        public void ImpGangabangaEXPLOSIONS(bool loss = false)
        {
            ClearOutput();

            Player.SlimeFeed();

            var titsOrgasm = false;

            CurrentMonster = EnemyRegistry.Create(new Identifier("coc", "imp_gang"));

            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreak".Localized());

            ShowImage("event-impGang");

            if (!loss)
                OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SleepUneasilyWakesBold".Localized());

            if (SceneInstance<ImpGangBangScene>.Get().SleepingInside())
                OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LookDoorForgotLock".Localized());

            if (!loss && RandomUtils.Chance(50 + (Player.Intelligence + Player.Speed) / 4))
            {
                OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsStandMeterTo".Localized(
                    CombatController.Instance!.Context.Enemy.CockDescript(0),

                    CombatController.Instance!.Context.Enemy.CockDescript(0)
                ));

                OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.QuicklyGetReadyWeapon".Localized());

                var impEnemy = EnemyRegistry.Create(new Identifier("coc", "imp"));

                var gangEnemy = EnemyRegistry.Create(new Identifier("coc", "imp_gang"));

                impEnemy.OnDefeated = _ => ImpGangGetsWhooped();

                impEnemy.OnWon = _ => ImpGangabangaEXPLOSIONS(true);

                gangEnemy.OnDefeated = _ => ImpGangGetsWhooped();

                gangEnemy.OnWon = _ => ImpGangabangaEXPLOSIONS(true);

                CombatScene.StartMultiple([impEnemy, gangEnemy], () => ReturnToCamp(1));

                impEnemy.ScaleToLevel(10);

                return;
            }

            if (Player.IsTaur())
            {
                if (Random.Shared.Next(2) == 0 && (Player.CockTotal() == 0 || Player.Gender == Gender.Hermaphrodite))
                {
                    if (!Player.HasStatusEffect(StatusEffectIds.ImpGangBang))
                    {
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsStandMeterAgain".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));

                        Player.CreateStatusEffect(StatusEffectIds.ImpGangBang, 0, 0, 0, 0);

                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsLeapSweatyClinging".Localized());

                        if (!Player.Weapon.IsAttached())
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AnotherKicksWeaponReach".Localized());

                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MonsterSurgesGrapplesHump".Localized(
                            CombatController.Instance!.Context.Enemy.ShortName,

                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));

                        if (Player.CockTotal() > 0)
                            OutputText(Player.CockDescript(0));
                        else
                            OutputText(Player.NippleDescript(0));

                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TwistingPinchingYelpMounts".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));
                    }

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HordeDragsHorseBody".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    if (Player.BiggestBreastSize() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ArmsTiedStandOut".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ThemselvesOutstretchedChest".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FaceFlushesTwistsHair".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SwarmsBodyStrokingSquirm".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.YelpShockSlapAss".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TongueSlidesSackDrool".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ShakeHipsDemonicTongue".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ShudderingSighHeatSpreads".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakAgain".Localized());

                    if (Player.BiggestBreastSize() > 1)
                    {
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HandsSlideAttentionMob".Localized(
                            Appearance.AllBreastsDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.Corruption < 50)
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CantTonguesLappingNipple".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TonguesSqueezingTuggingMatch".Localized(
                                Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TinyNeedlePierceVenom".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                        Player.GrowTits(2, Player.BreastRows.Count, false, 1);

                        Player.BoostLactation(.3f);

                        titsOrgasm = true;
                    }

                    ShowImage("gangbang-loss-centaur");

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DimlyThroughImpSteps".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse),

                        CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.YelpTwistEscapeHeat".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SlowlyRubsShaftCheeks".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse)
                    ));

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpGrabsBreasts".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HotDribblesOntoTongue".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EitherSwallowDemonTongue".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SwallowGlobSuckTongue".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigWalksPinnedBody".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OtherImpsAchingBreasts".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpRubsHandsFlanks".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse),

                        Player.VaginaDescript(0),

                        Appearance.CockNoun(BodyPartTypes.Horse)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ScreamForHelp".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanWithLust".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.InhumanlyHotHeadStretches".Localized());

                    if (Player.BiggestBreastSize() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsBreastsSuckHarder".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SquirmTwistAgainstHolders".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HelplessTerrorWorksUp".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PantingMonstrousPushesAside".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpace".Localized());

                    Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(1), true);

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FeelRidgeVeinPulling".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EveryPulseMakesTwitch".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WhimperSpasmingHotShaft".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanPleasureRotatingHips".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.InStuffedSneersFlexes".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RollChestSquirm".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EyesRollBackSquirm".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodParagraphBreak".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FinallyPullsRidgePulling".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BouncesAsBulgePasses".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanMushroomReachesEntrance".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TryBuckBreakFree".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanEcstasyPushesDeep".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse),

                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CorruptCockBottomsWomb".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SlowShakesBreastsCling".Localized(
                            Appearance.AllBreastsDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player),

                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigGrindsCockRidges".Localized(
                        Player.VaginaDescript(0),

                        Appearance.CockNoun(BodyPartTypes.Horse)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OverCocksWhereIt".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DripsOntoGround".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PainHugeStretchesOverwhelming".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PainPleasureBlendOne".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OtherImpsJerkImpales".Localized());

                    if (Player.BiggestBreastSize() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsBloatedDrinksFill".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SwellPainfullyPumpsBlood".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigImpTongueFlicks".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DrawsTongueWrapsCock".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.GaspTimeThrusts".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RapedMilkedLikeCow".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CorruptedLandNeverTaken".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanRiseTowardsOrgasm".Localized());

                    if (Player.BiggestBreastSize() > 3)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.UddersShakeBackForth".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SomePartFeelShame".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanShamelesslyMilkedMoos".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakFifth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterPoundsDeeper".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(1),

                        CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ScreamBloatsCorruptedJism".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse),

                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ThoughtDemonJismEdge".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.NearlyBlackOrgasmBlasts".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OrgasmsBreastsPumpMilk".Localized());

                    if (Player.Corruption >= 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LapEagerlyCumDrink".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakSixth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpJismRainsSpasming".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AndBouncingAgain".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MonsterNoMasturbateClaw".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MilkSpurtsNippleAnd".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FluidsFilledUnconsciousness".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WakeLaterTwitchingDark".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LastThoughtHideCamp".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LastMutatedMasterImp".Localized());

                    if (titsOrgasm)
                        Player.Orgasm("Tits");
                    else
                        Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 3f));

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);
                }
                else
                {
                    if (Player.HasStatusEffect(StatusEffectIds.ImpGangBang))
                        if (Player.Corruption < 50)
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CantTellNotRacist".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AboutTimeShowedRocks".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpTacklesGrabbingArms".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SwingDeterminedNotTake".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TwistStruggleInGrips".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.KickHoovesSmashChest".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HumanTorsoDraggedLog".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.UnboundBounceLog".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SpreadArmsTieLog".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FaceFlushesTurnAttentions".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AngerFlushesTurnAttentions".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakSeventh".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpClimbsFeetShoulders".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpMountsSlapsBetween".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                            Appearance.AllBreastsDescript(Player)
                        ));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpDucksGrabsCock".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TwoImpsSeizeCocks".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FightLegsBuckOff".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WritheGraspClawsTeeth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakEighth".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpHandlesSoftFlesh".Localized(
                            Appearance.BiggestBreastSizeDescript(Player),

                            Appearance.AllBreastsDescript(Player)
                        ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WhimperMistreatedFleshStings".Localized(
                            CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SuckleEagerlyMuskyBalls".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakNinth".Localized());

                    ShowImage("gangbang-loss-centaur");

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LoudPullsBallsOut".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                        OptionStore.Instance.Get("furry", 0) <= 0 ? "Common.Text.FurryDogEars".Localized() : "Common.Text.AFaceAsMuchDogAs".Localized(),

                        OptionStore.Instance.Get("furry", 0) <= 0 ? " covering his lower legs" : "",

                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OneSoreCallousedHand".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TailAndYanksBrutally".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.UntilShriekPlungesMouth".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterWalksBackHips".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceAgain".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PreDripsTipBase".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigImpCumStings".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceThird".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TryPullCockHead".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ScentMuskDrivesWild".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTenth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PointedTipPlungesSplitting".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WideOpenAroundCock".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriod".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpFuckingPaintsTits".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakEleventh".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigFucksClenchingHips".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EntranceOfWomb".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DepthsOfBowels".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AsKnotSlamsAgainst".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EachHardPoundsLog".Localized());

                    if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                        Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterFucksDogKnot".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SlappingAssFewThrusts".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RoughShakesStainedBreasts".Localized(
                            Appearance.AllBreastsDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RapedAgainRoastPig".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CorruptedLandTakenAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwelfth".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.GurgleHelplesslyThickWads".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EagerlyChugWadsThroat".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CumSwellingSlamsDeep".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight(),

                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SpasmsSteamingPoleNext".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ShootsAcrossGroundMouths".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakThirteenth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AnotherImpSpasmsThroat".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HugeStretchesHammers".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HowlAroundBloatedKnot".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterBloatedStretchesEntrance".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigAgainCumsWaves".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WombWord".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.GutsWord".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PumpsMoreJismBalls".Localized());

                    if (Player.BiggestBreastSize() > 2)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpCumsLoadLost".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterIsntChurningKnot".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AsContinuesBothEnds".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LegsPulsesWorkUp".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PassTidalWaveSpews".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WakeLaterTremblingBurbles".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BruisedTitsPressedWood".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BodyRestsCoolingPool".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HowLongLast".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WhyBotherQuestScratched".Localized());

                    Player.Orgasm("VaginalAnal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 3f));

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);

                    if (Player.HasVagina())
                    {
                        if (Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(2), true))
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakFourteenth".Localized());
                    }
                    else
                        if (Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(2), true))
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakFifteenth".Localized());
                }
            }
            else
                if (Random.Shared.Next(2) == 0 && (Player.CockTotal() == 0 || Player.Gender == Gender.Hermaphrodite))
                {
                    if (!Player.HasStatusEffect(StatusEffectIds.ImpGangBang))
                    {
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsStandMeterThird".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));

                        Player.CreateStatusEffect(StatusEffectIds.ImpGangBang, 0, 0, 0, 0);
                    }

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsLeapClingingAgain".Localized());

                    if (!Player.Weapon.IsAttached())
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.KicksWeaponAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MonsterSurgesHumpAgain".Localized(
                        CombatController.Instance!.Context.Enemy.ShortName,

                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText(Player.CockDescript(0));
                    else
                        OutputText(Player.NippleDescript(0));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TwistingPinchingMountsAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DraggingGroundNumbers".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsSurroundPreCum".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OthersStandJackOff".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpRubsCockHead".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakSixteenth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SnickersLewdlyNipplesHarden".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RenewBreakFreeLaugh".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BuckHipsTryingCapture".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BeforeShovedAsideBiggest".Localized());

                    ShowImage("gangbang-loss-human");

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FourFeetBroaderHealthier".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AndCocks".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HisSizeFist".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight(),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.YelpHipsEscapeHeat".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.GrabsHipsShaftDown".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ScreamForHelpAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanWithLustAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.InhumanlyHotStretchesAgain".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HelplessTerrorInchesUp".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PantingMonstrousAsideAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceFourth".Localized());

                    Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(1), true);

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakSeventeenth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FeelRidgePullingAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EveryMakesTwitchAgain".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WhimperSpasmingShaftAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanPleasureHipsAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.InStuffedSneersBounces".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.BiggestBreastSize() >= 3)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RollChestSquirmAgain".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.EyesRollSquirmAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceFifth".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SlapsAgainstDistendedBelly".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakEighteenth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FinallyPullsPullingAgain".Localized(
                        Appearance.CockNoun(BodyPartTypes.Horse)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BouncesBulgePassesUnder".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanReachesEntranceAgain".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TryHipsBackFighting".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanPushesDeepAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse),

                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CorruptBottomsWombAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigGrindsRidgesAgain".Localized(
                        Player.VaginaDescript(0),

                        Appearance.CockNoun(BodyPartTypes.Horse)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PainStretchesOverwhelmingAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PainPleasureBlendAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakNineteenth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.OtherImpsImpalesAgain".Localized());

                    if (Player.BiggestBreastSize() >= 3)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BounceJiggleBackForth".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SwellPumpsBloodAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigImpLashesOut".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DrawsWrapsCockAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TongueFlicksClit".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.GaspTimeThrustsAgain".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RapedForcedInhumanCock".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CorruptedLandTakenThird".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MoanRiseOrgasmAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterPoundsHardGrunt".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(1)
                    ));

                    if (Player.BiggestBreastSize() >= 3)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AndAgain".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterPoundsSeesSwell".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ScreamBloatsJismAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse),

                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ThoughtDemonEdgeAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Horse)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.NearlyBlackBlastsAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpRainsSpasmingAgain".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AndCumDrippingCock".Localized());

                    if (Player.BiggestBreastSize() >= 3)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AndBouncingAgain".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MonsterNoClawAgain".Localized());

                    if (Player.BiggestBreastSize() >= 3 && Player.BiggestLactation() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MilkSpurtsNippleAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FluidsUnconsciousnessAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WakeLaterDarkAgain".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0)
                    ));

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LastThoughtCampAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LastMutatedImpAgain".Localized());

                    Player.Orgasm("VaginalAnal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 3f));

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);
                }
                else
                {
                    if (Player.HasStatusEffect(StatusEffectIds.ImpGangBang))
                        if (Player.Corruption < 50)
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CantImpsLookAlike".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AboutShowedRocksAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MobImpsSwarmsDragging".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PunchKickWildlyDetermined".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TwistStruggleGrips".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PullFallenLog".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CoilsTwistingAroundLog".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LegsWideAgainstLog".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MonsterMakesWorkArmor".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpMountsChestSlaps".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                            Appearance.AllBreastsDescript(Player)
                        ));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CockSeizedLicksTip".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TwoImpsCocksLicking".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FightArmsShakeChest".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WritheGraspSpreadOpen".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodBreakAgain".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                    {
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HandsSlidePullingNipples".Localized(
                            Appearance.AllBreastsDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.Corruption < 50)
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CantEscapeTongueLapping".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TongueSqueezingFloodsLust".Localized());

                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.TinyNeedleNippleVenom".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                        if (Player.HasFuckableNipples())
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpTaintLikeShape".Localized());

                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwentieth".Localized());

                        Player.GrowTits(2, Player.BreastRows.Count, false, 1);

                        Player.BoostLactation(.5f);
                    }

                    ShowImage("gangbang-loss-human");

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterStepsUp".Localized());

                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AlongsideYourTautTail".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BetweenYourLegs".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.LeeringTrappedBody".Localized(
                        OptionStore.Instance.Get("furry", 0) <= 0 ? "Common.Text.FurryDogEars".Localized() : "Common.Text.AFaceAsMuchDogAs".Localized(),

                        OptionStore.Instance.Get("furry", 0) <= 0 ? " covering his lower legs" : "",

                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText(Player.MultiCockDescriptLight());
                    else
                        OutputText(Player.VaginaDescript(0));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceSixth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HeatStingsLapCooling".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ThrustsBrutallyShovingHead".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(2)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceSeventh".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ScreechForcesWolfCock".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterPainfullyStretchesWider".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HugeDickBumpsEntrance".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodBreakThird".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BigReachesTitRider".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MouthGapesFullCock".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterGrabsPistoningSteaming".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SentencePeriodSpaceEighth".Localized());

                    if (Player.BiggestBreastSize() > 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.RoughShakesClingAgain".Localized(
                            Appearance.AllBreastsDescript(Player),

                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpGrabsHairJaw".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.NotCaringBeyondEndurance".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpaledRoastPigAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.CorruptedLandTakenFourth".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwentyFirst".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AcheStartsBaseTits".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SmallerSlamsThroatSlapping".Localized(
                        CombatController.Instance!.Context.Enemy.BallsDescriptLight(),

                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.SpasmsSteamingNextAgain".Localized());

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ShootsAcrossBellyMouths".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwentySecond".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                    {
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpsBloatedRiderMilks".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.HasFuckableNipples())
                        {
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpGrabsFoldsLabia".Localized(
                                Appearance.AllBreastsDescript(Player),

                                Appearance.BiggestBreastSizeDescript(Player)
                            ));

                            if (Player.BiggestBreastSize() <= 5)
                                OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FeelBulbousSqueezeDeeper".Localized());
                            else
                                OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.FeelUnnaturallyErectionSpear".Localized(
                                    Appearance.BiggestBreastSizeDescript(Player),

                                    Appearance.BiggestBreastSizeDescript(Player)
                                ));

                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.BackBeginsFuckingTit".Localized(
                                Appearance.BiggestBreastSizeDescript(Player)
                            ));

                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwentyThird".Localized());
                        }
                    }

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpCockSpasmsRams".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.StretchesHammersAgain".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HowlBloatedKnotAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterBloatedEntranceAgain".Localized(
                            Appearance.CockNoun(BodyPartTypes.Dog)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpHowlsCumsAgain".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WombWordAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.GutsWordAgain".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PumpsMoreBallsAgain".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ImpCumsPaintsUdders".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MasterIsntKnotAgain".Localized(
                        Appearance.CockNoun(BodyPartTypes.Dog)
                    ));

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AsContinuesEndsAgain".Localized());

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText(Player.AssholeDescript());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ToesPulsesWorkUp".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.PassAnotherLoadThroat".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WakeLaterBurblesAgain".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.MilkFilledUddersSwollen".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.QuiveringOrgasmicTwitchSkin".Localized());

                    if (Player.BiggestBreastSize() > 3)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.AndMassiveTits".Localized());

                    OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.DrumTightButtonOutie".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.HowLongLastAgain".Localized());
                    else
                        OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.WhyBotherScratchedAgain".Localized());

                    Player.Orgasm("VaginalAnal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Corruption, 3f));

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);

                    if (Player.HasVagina())
                    {
                        if (Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(2), true))
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwentyFourth".Localized());
                    }
                    else
                        if (Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(2), true))
                            OutputText("Scenes.Impscene.ImpGangabangaEXPLOSIONS.ParagraphBreakTwentyFifth".Localized());
                }

            if (!loss)

                CoC.Instance.Flags.Set(GameFlagIds.HasBeenImpSleepRaped, true);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(PlayerMenu);
        }

        public void ImpRapesYou(bool hpVictory)
        {
            ClearOutput();

            if ((Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties)) && !Player.IsTaur() && Player.HasVagina())
            {
                ShowImage("imp-loss-female-fuck");

                OutputText("Scenes.Impscene.ImpRapesYou.SinkGroundAssumingPosition".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                OutputText("Scenes.Impscene.ImpRapesYou.AnticipationInvoluntarilyKnees".Localized(
                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Impscene.ImpRapesYou.ImpPushesRammingHilt".Localized());

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                OutputText("Scenes.Impscene.ImpRapesYou.ParagraphBreak".Localized());

                OutputText("Scenes.Impscene.ImpRapesYou.RoughFuckingAirHeaded".Localized());

                OutputText("Scenes.Impscene.ImpRapesYou.SighPullsFlopDown".Localized());

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                CombatScene.CleanupAfterCombat();

                return;
            }

            if (hpVictory)
                ImpRapesYouHpLoss();
            else
            {
                if ((Player.Cocks.Count >= 1 && Random.Shared.Next(2) == 0) && (Player.Cocks[0].Thickness >= 4))
                {
                    SprocketImp();

                    return;
                }

                if (Player.Gender == Gender.Female || Player.Gender == Gender.Hermaphrodite)
                {
                    Player.SlimeFeed();

                    ShowImage("imp-loss-female-fuck");

                    OutputText("Scenes.Impscene.ImpRapesYou.SinkGroundLustWicked".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Impscene.ImpRapesYou.SmilesPressesLustDriven".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                    {
                        if (Player.Vaginas[0].Virgin)
                            OutputText("Scenes.Impscene.ImpRapesYou.PlungesHardBreakingHymen".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpRapesYou.PushesAgainstLittlePussy".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.CockSinksInchesClaiming".Localized(
                            Player.VaginaDescript(0)
                        ));

                        Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);
                    }
                    else
                    {
                        OutputText("Scenes.Impscene.ImpRapesYou.PlungesViolentlyRammingHilt".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));

                        Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);
                    }

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Impscene.ImpRapesYou.RoughFuckingTimePasses".Localized(
                            Player.VaginaDescript(0)
                        ));

                    if (Player.Gender == Gender.Female)
                        OutputText("Scenes.Impscene.ImpRapesYou.RoughFuckingMoanLoudly".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Impscene.ImpRapesYou.ImpExplodesPumpingLoads".Localized(
                        CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                    ));

                    OutputText("Scenes.Impscene.ImpRapesYou.SighPullsFlopBack".Localized());

                    if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation <= 216)
                        OutputText("Scenes.Impscene.ImpRapesYou.WonderWomb".Localized());
                    else
                        if (Player.InHeat)
                            OutputText("Scenes.Impscene.ImpRapesYou.FindYourselfHopingPregnant".Localized());
                        else
                            if (Player.Pregnancy.Incubation <= 0)
                                if (Player.Corruption > 75)
                                    OutputText("Scenes.Impscene.ImpRapesYou.AppreciativeMoanBuryFingers".Localized());
                                else
                                    OutputText("Scenes.Impscene.ImpRapesYou.HopeDontLoseConsciousness".Localized());

                    Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 1f), new DynStat(GameStats.Corruption, 1f));

                    Player.Orgasm("Vaginal");
                }

                if (Player.Gender == Gender.Genderless || Player.Gender == Gender.Male)
                {
                    ShowImage("monster-imp");

                    if (Player.Gender == Gender.Male && Random.Shared.Next(2) == 0)
                    {
                        OutputText("Scenes.Impscene.ImpRapesYou.EyesGlazeLustMagic".Localized());

                        if (Player.IsTaur())
                            OutputText("Scenes.Impscene.ImpRapesYou.HocksKneesHurtingPressure".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpRapesYou.KneesPullMindlesslyStroking".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.EyesGlanceMassiveBulge".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.DropsLoinclothInchGrabs".Localized());

                        if (Player.IsTaur())
                            OutputText("Scenes.Impscene.ImpRapesYou.DroppingGroundTremblesRhythm".Localized());
                        else
                            OutputText("Scenes.Impscene.ImpRapesYou.AllFoursBounces".Localized());

                        if (Player.BallSize >= 5)
                            OutputText("Scenes.Impscene.ImpRapesYou.HugeBallsSwingHeavily".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.LightHeadedAirGrips".Localized());

                        Player.RefillHunger(40);

                        OutputText("Scenes.Impscene.ImpRapesYou.SlowlyWithdrawsPumpingThroat".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.FallGroundGaspingExhausted".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 2f));

                        CombatScene.CleanupAfterCombat();

                        Player.SlimeFeed();

                        Player.Orgasm("Lips", false);

                        return;
                    }
                    else
                    {
                        Player.SlimeFeed();

                        OutputText("Scenes.Impscene.ImpRapesYou.SinkGroundLustCircles".Localized());

                        if (!Player.IsTaur())
                            OutputText("Scenes.Impscene.ImpRapesYou.AsYouAreDisrobed".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.ImpPressesMassivePenis".Localized());

                        OutputText("Scenes.Impscene.ImpRapesYou.ImpPushesFucksHard".Localized(
                            CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));

                        Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                        Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 1f), new DynStat(GameStats.Corruption, 1f));

                        if (Player.Sensitivity > 40)
                        {
                            OutputText("Scenes.Impscene.ImpRapesYou.ManageFilledHotCum".Localized());

                            if (Player.Gender == Gender.Male)
                                OutputText("Scenes.Impscene.ImpRapesYou.JizzGroundHugeSquirts".Localized());

                            Player.Orgasm("Anal");

                            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
                        }

                        OutputText("Scenes.Impscene.ImpRapesYou.DropGroundSpilling".Localized());
                    }
                }
            }

            CombatScene.CleanupAfterCombat();
        }

        public void NoogaisNippleRape()
        {
            ClearOutput();

            ShowImage("monster-imp");

            OutputText("Scenes.Impscene.NoogaisNippleRape.SlowlyWalkHipsSwaying".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.SheddingClothesPushStraddle".Localized());

            if (Player.HasCock())
                OutputText(Player.CockDescript(0));

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Impscene.NoogaisNippleRape.AndAgain".Localized());

            if (Player.HasVagina())
                OutputText(Player.VaginaDescript(0));
            else
                if (Player.Gender == Gender.Genderless)
                    OutputText(Player.AssholeDescript());

            OutputText("Scenes.Impscene.NoogaisNippleRape.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.GentlyInsertSingleDigit".Localized());

            if (Player.LactationQ() >= 1000)
                OutputText("Scenes.Impscene.NoogaisNippleRape.UnleashingTorrentThickMilk".Localized());
            else
                if (Player.LactationQ() >= 50 && Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Impscene.NoogaisNippleRape.ReleasingSteadyWarmMilk".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.LustInducedOntoLap".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Impscene.NoogaisNippleRape.RockHardCock".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Impscene.NoogaisNippleRape.AndAgain".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Impscene.NoogaisNippleRape.DrippingWetPussy".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText(Player.AssholeDescript());

            OutputText("Scenes.Impscene.NoogaisNippleRape.TeasingLewdRollsBack".Localized());

            if (silly)
                OutputText("Scenes.Impscene.NoogaisNippleRape.ImpSickenedButCurious".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.ParagraphBreak".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.ContinueFingerNippleAroused".Localized());

            if (Player.Corruption >= 66)
            {
                OutputText("Scenes.Impscene.NoogaisNippleRape.WhisperHuskilySoppingHole".Localized());

                if (Player.BiggestLactation() < 1)
                    OutputText("Scenes.Impscene.NoogaisNippleRape.Wet".Localized());
                else
                    OutputText("Scenes.Impscene.NoogaisNippleRape.Milky".Localized());

                OutputText("Scenes.Impscene.NoogaisNippleRape.BuckingHipsUpwardsDesperation".Localized());
            }

            OutputText("Scenes.Impscene.NoogaisNippleRape.DecidingSufferedGuideNipple".Localized());

            if (Player.BiggestBreastSize() <= 4)
                OutputText("Scenes.Impscene.NoogaisNippleRape.BottomingHalfwayImmenseDick".Localized());
            else
                OutputText("Scenes.Impscene.NoogaisNippleRape.OnlyStoppingBumpsBallsack".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.ShudderEcstasyRiseGirth".Localized());

            if (Player.BiggestBreastSize() <= 4)
                OutputText("Scenes.Impscene.NoogaisNippleRape.FeelSwollenBottomOut".Localized());
            else
                OutputText("Scenes.Impscene.NoogaisNippleRape.SwollenFilledBallsEngulfed".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.TimingThrustsMovements".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Impscene.NoogaisNippleRape.PumpingCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Impscene.NoogaisNippleRape.FingeringHungryBabyTunnel".Localized());
                else
                    OutputText("Scenes.Impscene.NoogaisNippleRape.FingeringTinglingAnus".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.SentencePeriodBreakAgain".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.EventuallyRhythmSporadicClimax".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Impscene.NoogaisNippleRape.AWord".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Impscene.NoogaisNippleRape.Jet".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Impscene.NoogaisNippleRape.Geyser".Localized());
                    else
                        OutputText("Scenes.Impscene.NoogaisNippleRape.Volcano".Localized());

                OutputText("Scenes.Impscene.NoogaisNippleRape.CumSpraysCockSplatters".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Impscene.NoogaisNippleRape.While".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Impscene.NoogaisNippleRape.PussyJuicesSpurtTwitches".Localized(
                    Player.VaginaDescript(0)
                ));

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Impscene.NoogaisNippleRape.AssholeClenchesTightFinger".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.SentencePeriodBreakThird".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.CollapseHeavilyImpalingBreast".Localized());

            if (Player.BiggestLactation() >= 1 && Player.LactationQ() < 40)
                OutputText("Scenes.Impscene.NoogaisNippleRape.Milk".Localized());

            OutputText("Scenes.Impscene.NoogaisNippleRape.FemSpunkCumLeaks".Localized());

            if (Player.Corruption > 60)
                OutputText("Scenes.Impscene.NoogaisNippleRape.ThrustDigitsScoopingJizz".Localized());
            else
                OutputText("Scenes.Impscene.NoogaisNippleRape.QuicklyDressedLeaveSlumbering".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void PutBeeEggsInAnImpYouMonster()
        {
            ClearOutput();

            ShowImage("monster-imp");

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.MasturbatingTwitchAbdomen".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.TooBusyAirHooking".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.GrinStraddlingAssResting".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.HeGaspsGoingOn".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.DeignNotAbdomenCurling".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.WhimpersNoBiteLip".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.ShudderTandemHoneySmears".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.LayFirstFaceDown".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.ImpPantsBreathAdjusting".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.GroansStopThirdEgg".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.RippledGooEdges".Localized());
            else
                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.TremblingCoils".Localized());
                else
                    OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.StraddlingKnees".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.TurnImpPrivateIncubator".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.AfterEggsRunRoom".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.EyesAlmostRolledBack".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.EnoughStimulateOvipositor".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.MoansMoreMoreOblige".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.TinyClawsGrabYour".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.FirstRowOf".Localized());

                OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.SqueezingTitsFuck".Localized());

                if (Player.LactationQ() >= 500)
                    OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.RivuletsMilkRunDown".Localized());
            }
            else
                if (Player.HasCock())
                    OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.RiseSwollenBellyPresses".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.TinyClawedScrabbleFlails".Localized());
                    else
                        OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.FeelFirmAssholeTail".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.GroanClimaxingGoutSeed".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.PantHeavilyPullOut".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.HearSoundsLikeGiggle".Localized());

            OutputText("Scenes.Impscene.PutBeeEggsInAnImpYouMonster.BlinkStandChalkingOff".Localized());

            Player.Orgasm("Ovi");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.DumpEggs();

            CombatScene.CleanupAfterCombat();
        }

        public void ImpFunWithUnbirthing()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Impscene.ImpFunWithUnbirthing.ClapHandsChildrenSwarm".Localized(
                Player.VaginaDescript(0)
            ));

            ShowImage("imp-win-female-fuck");

            OutputText("Scenes.Impscene.ImpFunWithUnbirthing.AlreadyForgottenDevouredPussy".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            Player.Orgasm("Vaginal");

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -2);

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 2f));

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 24);

            CombatScene.CleanupAfterCombat();
        }

        public void PissDom()
        {
            ClearOutput();

            OutputText("Scenes.Impscene.PissDom.ImpDefeatedSpreadOut".Localized());

            OutputText("Scenes.Impscene.PissDom.ThwhackSwiftKickTumbling".Localized());

            OutputText("Scenes.Impscene.PissDom.SwingHeadBackPiss".Localized());

            OutputText("Scenes.Impscene.PissDom.DemonCockTwitchesUnsatisfied".Localized());

            OutputText("Scenes.Impscene.PissDom.ContinueButtfuckLanguidPace".Localized());

            OutputText("Scenes.Impscene.PissDom.SensesOverwhelmOrgasmArrives".Localized((Player.Balls > 0 ? "Common.Text.Ballsack".Localized() : "Common.Text.Prostate".Localized())));

            OutputText("Scenes.Impscene.PissDom.RedressLeaveNearlyUnconscious".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            CombatScene.CleanupAfterCombat();
        }

        public bool ImpPetrifyable()
        {

            return Player.IsBasilisk() && CoC.Instance.Flags.Get(GameFlagIds.CampWallProgress) >= 100;
        }

        public bool AddKillPetrifyButtons(float killButtonPos, float petriButtonPos)
        {

            var buttonCount = 0;

            if (CombatController.Instance!.Context.Enemy.ShortName != "imp overlord")
                return false;

            if (float.IsNaN(killButtonPos))
                killButtonPos = 0;

            if (killButtonPos >= 0 && CombatController.Instance!.Context.Enemy.Health < 1)
            {
                AddButton((int)killButtonPos, "Common.Button.KillHim".Localized(), KillImp);

                buttonCount += 1;
            }

            if (float.IsNaN(petriButtonPos))
                petriButtonPos = killButtonPos + 1;

            if (ImpPetrifyable())
            {
                AddButton((int)petriButtonPos, "Common.Button.PetrifyHim".Localized(), PetrifyImp);

                buttonCount += 1;
            }

            return buttonCount > 0;
        }

        public void PetrifyImp()
        {

            var winText = "";

            var winText2 = "";

            var skullText = "";

            var benoitE = 0;

            ClearOutput();

            ShowImage("monster-imp");

            if (Enemy.Health >= 1 && (50 + 50 * (1 - (Enemy.MaxHealth <= 0 ? 1f : Enemy.Health / Enemy.MaxHealth))) < Random.Shared.Next(100))
            {
                OutputText("Scenes.Impscene.PetrifyImp.DecideTeachGrabThroat".Localized());

                OutputText("Scenes.Impscene.PetrifyImp.PerhapsBeatenDown".Localized((monster.HPRatio() < 1 ? " more" : "")));

                CombatScene.CleanupAfterCombat();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ImpsPetrified) <= 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ImpsPetrified, 0);

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                {
                    winText = "the beaten and bruised creature cowering teary-eyed,";

                    winText2 = "";
                }
                else
                {
                    winText = "the pathetically masturbating creature too focused on his blighted cock,";

                    winText2 = "though his hand remains on his prick,";
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.CampWallSkulls) > 0)
                    skullText = "heedless of the clear warning your wall presents.";
                else
                    skullText = "no matter how many you beat.";

                OutputText("Scenes.Impscene.PetrifyImp.LookImpFeelDisgusted".Localized(
                    winText,

                    skullText
                ));

                OutputText("Scenes.Impscene.PetrifyImp.ThinkExplorationsHighMountains".Localized(
                    benoitE
                ));

                OutputText("Scenes.Impscene.PetrifyImp.BlinkSlowlyYourselfScruff".Localized(
                    winText2
                ));

                OutputText("Scenes.Impscene.PetrifyImp.BasilisksCompulsionGaze".Localized());

                OutputText("Scenes.Impscene.PetrifyImp.SeeStartGlassOver".Localized());

                OutputText("Scenes.Impscene.PetrifyImp.ShutLinkDazedExpression".Localized());

                OutputText("Scenes.Impscene.PetrifyImp.SureTransfiguredReachTouch".Localized());
            }
            else
            {
                OutputText("Scenes.Impscene.PetrifyImp.DecideTeachThroatAgain".Localized());

                OutputText("Scenes.Impscene.PetrifyImp.BlinkLetReachPower".Localized());

                OutputText("Scenes.Impscene.PetrifyImp.NoStrugglesAgainstStone".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.ImpsPetrified, CoC.Instance.Flags.Get(GameFlagIds.ImpsPetrified) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.BasiliskResistanceTracker, CoC.Instance.Flags.Get(GameFlagIds.BasiliskResistanceTracker) + 10);

            ClearButtons();

            AddButton(0, "Common.Button.TakeStatue".Localized(), () =>
            {
                CombatScene.CleanupAfterCombat();

                TakeStatue();
            });

            AddButton(1, "Common.Button.Leave".Localized(), () => CombatScene.CleanupAfterCombat());
        }

        public void TakeStatue()
        {
            ClearOutput();

            var timeToReturn = 1;

            if (Player.Strength < 90)
                timeToReturn += 1;

            if (Player.Strength < 70)
                timeToReturn += 1;

            if (Player.Strength < 40)
                timeToReturn += 1;

            var timeText = "";

            timeText = timeToReturn > 1 ? NumberUtils.Num2Text(timeToReturn) + "Common.Text.Hours".Localized() : "hour";

            ShowImage("event-impStatue");

            if (CoC.Instance.Flags.Get(GameFlagIds.CampWallStatues) <= 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.CampWallStatues, 0);

                var __sw0_1 = timeToReturn;

                if (__sw0_1 == 1)
                    OutputText("Scenes.Impscene.TakeStatue.HeftStatueShoulderLight".Localized());

                if (__sw0_1 == 2)
                    OutputText("Scenes.Impscene.TakeStatue.HeftStatueBackBulk".Localized());

                if (__sw0_1 == 3 || __sw0_1 == 4)
                    OutputText("Scenes.Impscene.TakeStatue.TryLiftUnableHuff".Localized());
            }
            else
                OutputText("Scenes.Impscene.TakeStatue.InTimeTakesCarry".Localized(
                    timeText
                ));

            CoC.Instance.Flags.Set(GameFlagIds.CampWallStatues, CoC.Instance.Flags.Get(GameFlagIds.CampWallStatues) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.CampWallStatues) == 1)

                OutputText("Common.Text.NNBYouNowHave".Localized());
            else

                OutputText("\n\n<b>You now have " + NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.CampWallStatues)) + "Common.Text.MarbleImpStatuesAtCampB".Localized());

            if (timeToReturn >= 3)
                Player.ChangeFatigue(10);

            if (timeToReturn >= 4)
                Player.ChangeFatigue(10);

            ReturnToCamp(timeToReturn);
        }

        public void KillImp()
        {
            ClearOutput();

            RegisterMacro("impAmount", CombatController.Instance!.Context.Enemies.Count);

            ShowImage("item-impSkull");

            CoC.Instance.Flags.Set(GameFlagIds.ImpsKilled, CoC.Instance.Flags.Get(GameFlagIds.ImpsKilled) + 1);

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Impscene.KillImp.PlungeRadiantBladeIncinerating".Localized());
            else
                if (Player.Weapon.Id == WeaponIds.DullScimitar)
                    OutputText("Scenes.Impscene.KillImp.RaiseCursedScytheReap".Localized());
                else
                    if (Player.Weapon.IsFirearm())
                        OutputText("Scenes.Impscene.KillImp.GrabImpHornBarrel".Localized());
                    else
                        if (Player.Weapon.IsStaff() && Player.Weapon.IsChanneling(Player))
                            OutputText("Scenes.Impscene.KillImp.ChantBasicSoftGlow".Localized());
                        else
                            if (Player.Weapon.IsSpear() || Player.Weapon.Id == ItemIds.MidnightRapier || Player.Weapon.Id == ItemIds.RaphaelsRapier)
                                OutputText("Scenes.Impscene.KillImp.LungeForwardEasilyPiercing".Localized());
                            else
                                if (Player.Weapon.IsAxe())
                                    OutputText("Scenes.Impscene.KillImp.StompHeadAgainstGround".Localized());
                                else
                                    if (Player.Weapon.IsBladed())
                                        OutputText("Scenes.Impscene.KillImp.SwingHeavilySeveringHead".Localized());
                                    else
                                        OutputText("Scenes.Impscene.KillImp.QuickWorkImpDragging".Localized());

            OutputText("Scenes.Impscene.KillImp.CretinScramblesFleeSuffers".Localized());

            if (Player.Corruption < 25)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -0.5f));

            Player.UpgradeDeusVult();

            ClearButtons();

            AddButton(0, "Common.Button.TakeSkull".Localized(), TakeSkull);

            AddButton(1, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void TakeSkull()
        {
            ClearOutput();

            Player.Inventory.Add(ItemRegistry.Create(UseableIds.ImpSkull), 1);

            CombatScene.CleanupAfterCombat();
        }

        public void LoliVirginRape()
        {
            ClearOutput();

            OutputText("Scenes.Impscene.LoliVirginRape.FallGroundSoreBody".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.LittleChucklesRareTreat".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.WriggleWritheVainAttempt".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.DemonRearsHeadSlit".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.AggressiveFilledLungePierce".Localized());

            Player.CuntChange(10, true, true);

            OutputText("Scenes.Impscene.LoliVirginRape.SharpBurningForefrontMind".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.RapistYesChildrenGift".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.ImpNotGoingAnywhere".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.LackOxygenOverwhelmingPain".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.HoursLaterSitUp".Localized());

            OutputText("Scenes.Impscene.LoliVirginRape.ShakeThoughtsGatheringThings".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

            CombatScene.CleanupAfterCombat();
        }

        public void ImpRapesYouHpLoss()
        {
            ClearOutput();

            RegisterMacro("noob", Player.Level == 1);

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.CantLetFallHere".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.WorldWobblesSwaysStruggle".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.IsntHowQuestGoing".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.DoesntHandsSlipUnder".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.OneFinalComeHard".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.ImpVisionClears".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.MoreYouTryThrash".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.SomethingFirmTellYou".Localized((Player.HasCock() && Player.Cocks[0].cockType == CockTypesEnum.DEMON ? "Common.Text.EvenMorePerverseThanYourOwn".Localized() : "Common.Text.ATrulyInhumanThing".Localized()), (Player.HasCock() && Player.Cocks[0].cockType == CockTypesEnum.DEMON ? "Common.Text.Its".Localized() : "Common.Text.TheStrange".Localized())));

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.ThenSlipsDeliberateSlowness".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.NostrilsFlareBrushesAgainst".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.DontEvenChanceBlink".Localized());

            if (Player.HasVagina())
            {
                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);
            }
            else
                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.EveryTaintedBurnsWalls".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.EntireShrivelBottomsOut".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.ToDismayEncourageMore".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.NoUsePinsClose".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.SitsLikeSecondSkin".Localized());

            OutputText("Scenes.Impscene.ImpRapesYouHpLoss.LastThingRushWings".Localized());

            Player.Orgasm("VaginalAnal");

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
        }

    }
}
