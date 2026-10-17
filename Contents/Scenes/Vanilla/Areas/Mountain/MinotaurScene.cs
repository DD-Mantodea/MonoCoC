using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
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
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<MinotaurScene>("coc:minotaur")]
    public class MinotaurScene : Scene
    {

        public override void Entry()
        {
            MinotaurAddicted();
        }

        public bool MinotaurAddicted()
        {

            return Player.MinotaurAddicted();
        }

        public bool MinotaurNeed()
        {

            return Player.MinotaurNeed();
        }

        public void MinoVictoryRapeChoices()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 1)
                OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SmilingVanquishedFoeHunger".Localized());
            else
                if (Player.Lust >= 33 && !Player.IsGenderless())
                {
                    if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                        OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SmileDropsFoursMasturbates".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SmileCollapsesUnableContinue".Localized());
                }
                else
                    if (Player.HasStatusEffect(StatusEffectIds.Feeder))
                    {
                        if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                            OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SmileCollapsesContinueAgain".Localized());
                        else
                            OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SmileCollapsesContinueThird".Localized());
                    }
                    else
                        if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                            OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SmileDropsMasturbatesAgain".Localized());
                        else
                            OutputText("Scenes.Minotaurscene.MinoVictoryRapeChoices.SatisfactionCollapsesUnable".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.UseCock".Localized(), null);

            AddButton(1, "Common.Button.UseVagina".Localized(), null);

            AddButton(2, "Common.Button.UseBoth".Localized(), null);

            AddButton(3, "Common.Button.Tentacledick".Localized(), null);

            AddButton(4, "Common.Button.Urethrafuck".Localized(), null);

            AddButton(5, "Common.Button.Makehimsuck".Localized(), null);

            AddButton(6, "Common.Button.Minomutual".Localized(), null);

            AddButton(7, "Common.Button.GetFilled".Localized(), null);

            AddButton(8, "Common.Button.TitfuckHim".Localized(), null);

            AddButton(9, "Common.Button.LayEggs".Localized(), null);

            AddButton(10, "Common.Button.Breastfeed".Localized(), null);

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (Player.Lust >= 33)
            {
                if (x >= 0)
                {
                    AddButton(0, "Common.Button.UseCock".Localized(), BumRapeaMinotaur);

                    AddButton(6, "Common.Button.Minomutual".Localized(), MinoMutual);

                    if (Player.CockTotal() > 1 && Player.CountCocksOfType("TENTACLE") > 0)
                        AddButton(3, "Common.Button.Tentacledick".Localized(), RapeMinotaurTentacles);
                }

                if (Player.HasCock())
                {
                    var counter = 0;

                    while (counter < Player.CockTotal())
                    {
                        if (Player.Cocks[counter].Thickness <= 4 && Player.Cocks[counter].Length >= 14)
                        {
                            AddButton(4, "Common.Button.Urethrafuck".Localized(), MinoUrethralPen);

                            break;
                        }

                        counter += 1;
                    }

                    if (Player.HasCockThatFits(80))
                        AddButton(5, "Common.Button.Makehimsuck".Localized(), MinotaurBlowjob);
                }

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.UseVagina".Localized(), GirlRapeAMinotaur);

                if (Player.IsHerm() && x >= 0 && !Player.IsTaur())
                    AddButton(2, "Common.Button.UseBoth".Localized(), MinotaurGetsRapedByHerms);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 1 || Player.HasPerk(PerkIds.MinotaurCumAddict))
            {
                AddButton(7, "Common.Button.GetFilled".Localized(), TakeMinoCumDirectly);

                if (Player.BiggestBreastSize() >= 5 && !Player.HasTailInsteadOfLegs())
                    AddButton(8, "Common.Button.TitfuckHim".Localized(), MinoGetsTitFucked);
                else
                    if (Player.IsNaga())
                        AddButton(8, "Common.Button.Prostatemilk".Localized(), MinoGetsTitFucked);
            }

            if (Player.CanOvipositSpider() || (Player.CanOvipositBee() && !Player.IsGenderless()))
                AddButton(9, "Common.Button.LayEggs".Localized(), LayEggsInAMinotaurSpiderLike);

            if (Player.HasPerk(PerkIds.Feeder) || Player.LactationQ() >= 1000)
                AddButton(10, "Common.Button.Breastfeed".Localized(), MinotaurDrinksMilkNewsAtEleven);

            if (Player.HasVagina() && Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                AddButton(11, "Common.Button.BikiniTitfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                AddButton(12, "Common.Button.UnbirthHim".Localized(), MinotaurFunWithUnbirthing);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void RapeMinotaurTentacles()
        {
            ClearOutput();

            if (Player.Cocks.Count > 1 && Player.CountCocksOfType("TENTACLE") == 1)
            {
                RapeMinotaurTentacle();

                return ;
            }

            MinoRapeIntro();

            int x = 0;

            int counter = 0;

            while (counter < Player.CockTotal())
            {
                if (Player.Cocks[counter].Id == new Identifier("coc", "tentacle"))
                {
                    x = counter;

                    break;
                }

                counter += 1;
            }

            var mismatched = false;

            int temp2 = 0;

            if (CombatController.Instance!.Context.Enemy.Health <= 0)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.MinotaurCollapsesDefeatedTired".Localized());
            else
            {
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.MinotaurDropsHisArm".Localized());

                if (CombatController.Instance!.Context.Enemy.WeaponName != "axe")
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.LettingAxeClatterFloor".Localized());

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.TransfixedCockFrontFace".Localized(
                     Player.MultiCockDescript()
                ));
            }

            OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.GrabArmNoRetaliation".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.CryPettingSmoothingHorns".Localized(
                     Player.MultiCockDescript()
                ));

            if (Player.Corruption >= 25 && Player.Corruption < 50)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.SmoothlyGuideCockTailhole".Localized(
                     Player.MultiCockDescript()
                ));

            if (Player.Corruption >= 50 && Player.Corruption < 80)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.RoughlyGrabSlammingClose".Localized(
                     Player.MultiCockDescript()
                ));

            if (Player.Corruption >= 80)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.GrinHornsRideBeast".Localized(
                     Player.MultiCockDescript()
                ));

            ShowImage("minotaur-win-tentacle");

            if (Player.CockTotal() != Player.CountCocksOfType("TENTACLE"))
            {
                mismatched = true;

                var temp = Player.Cocks.Count;

                temp2 = -1;

                while (temp > 0 && temp2 == -1)
                {
                    temp -= 1;

                    if (Player.Cocks[temp].Id != new Identifier("coc", "tentacle"))
                        temp2 = temp;
                }

                if (temp2 == -1)
                    temp2 = 0;

                temp = Player.Cocks.Count;

                while (temp > 0)
                {
                    temp -= 1;

                    if (Player.Cocks[temp].Length > Player.Cocks[temp2].Length && Player.Cocks[temp].Id != new Identifier("coc", "tentacle"))
                        temp2 = temp;
                }

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.CockSlamsTailholeSpreading".Localized(
                     Player.CockDescript(temp2)
                ));
            }
            else
            {
                temp2 = 0;

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.PrehensileCoilsNearTailhole".Localized(
                     NumberUtils.Num2Text((int)Math.Floor(Player.Cocks[0].Length))
                ));
            }

            if (Player.Vaginas.Count >= 1)
            {
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.CockEmbeddedTwistPositioning".Localized(
                     Player.CockDescript(temp2),

                     Player.VaginaDescript(0)
                ));

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false, true);
            }

            if (Player.Vaginas.Count > 1 && !mismatched && Player.Cocks.Count > 2)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.TentacleCocksWritheThemselves".Localized(
                     Player.VaginaDescript(1)
                ));

            if (Player.CountCocksOfType("TENTACLE") == 2 || Player.CountCocksOfType("TENTACLE") == 3)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.NextTentacleSaddenedTurns".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 3)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.RemainingDicksWaveBalls".Localized());

            OutputText("Scenes.Minotaurscene.RapeMinotaurTentacles.TentaclesThrobPulseSwelling".Localized());

            Player.Orgasm("Dick", true, Player.CountCocksOfType("TENTACLE"));

            CombatScene.CleanupAfterCombat();
        }

        public void RapeMinotaurTentacle()
        {
            ClearOutput();

            MinoRapeIntro();

            int x = 0;

            int counter = 0;

            while (counter < Player.CockTotal())
            {
                if (Player.Cocks[counter].Id == new Identifier("coc", "tentacle"))
                {
                    x = counter;

                    break;
                }

                counter += 1;
            }

            OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.StandDefeatedBackForth".Localized());

            ShowImage("minotaur-win-tentacle");

            OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.ComeBehindAroundMember".Localized());

            OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.BallsBeginBaseTightly".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                 Player.CockDescript(x),

                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            var temp = 0;

            if (Player.CountCocksOfType("Horse") == 1)
            {
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.NotEnoughPullAnus".Localized());

                temp = 1;
            }

            if (Player.CountKnottedCocks() == 1 && temp != 1)
            {
                temp = 1;

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.BitchKnotterPullAnus".Localized());
            }

            if (Player.Cocks.Count == 2 && temp != 1)
            {
                temp = 1;

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.AvailYourselfRearEnd".Localized());
            }

            if (Player.Cocks.Count > 2 && temp != 1)
            {
                temp = 1;

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.NotDicksLargeHole".Localized(
                     Player.MultiCockDescript()
                ));
            }

            if (Player.GetClitLength() > 6)
                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.EagerMoreClitFuck".Localized());

            if (Player.Vaginas.Count > 0)
            {
                temp = 2;

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.YoureWomanActionSlide".Localized(
                     Player.VaginaDescript(0),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.EventuallyMomentBoilingHeat".Localized(
                     Player.MultiCockDescript()
                ));

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.CockMessWetSuck".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.CumQ() >= 50 && Player.CumQ() < 400)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.CockMessFreeDrips".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.CumQ() >= 400)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.CockMessRiverCum".Localized(
                         Player.VaginaDescript(0)
                    ));
            }
            else
            {
                temp = 2;

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.FeelingLittleFrontOffer".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                    Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

                OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.EventuallyMomentHeatAgain".Localized(
                     Player.MultiCockDescript()
                ));

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.CockAssWetSucking".Localized());

                if (Player.CumQ() >= 50 && Player.CumQ() < 400)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.CockAssMatFur".Localized());

                if (Player.CumQ() >= 400)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.CockBlastedForceEjaculation".Localized());
            }

            if (temp < 2)
            {
                if (Player.CumQ() < 25)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.MomentPositionAccordingly".Localized());

                if (Player.CumQ() >= 25 && Player.CumQ() < 250)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.MomentAccordinglyAgain".Localized());

                if (Player.CumQ() >= 250 && Player.CumQ() < 500)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.MomentAccordinglyThird".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Minotaurscene.RapeMinotaurTentacle.MomentAccordinglyFourth".Localized());
            }

            CombatScene.CleanupAfterCombat();
        }

        public void AddictNagaOnMinotaur()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.SlitherFallenTasteSemen".Localized());
            else
                OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.SlitherFallenSemenAgain".Localized());

            OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.GooeyPrecumHorseCock".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.FuriouslyStrokeCocksMuffled".Localized());
            else
                OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.PartFoldsInsideMasturbating".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.LungsPainFinishQuicker".Localized());

            ShowImage("minotaur-win-naga");

            OutputText("Scenes.Minotaurscene.AddictNagaOnMinotaur.WipeCumFaceDrenched".Localized());

            MinoCumAddiction(10);

            CombatScene.CleanupAfterCombat();
        }

        public void MinoUrethralPen()
        {

            MinoRapeIntro();

            int counter = 0;

            int x = 0;

            while (counter < Player.CockTotal())
            {
                if (Player.Cocks[counter].Thickness <= 4 && Player.Cocks[counter].Length >= 14)
                {
                    x = counter;

                    break;
                }

                counter += 1;
            }

            ShowImage("minotaur-win-male");

            if (Player.StatusEffectValue(StatusEffectIds.Infested, 0) == 5)
            {
                OutputText("Scenes.Minotaurscene.MinoUrethralPen.GrinningMaliciouslyOntoBack".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.BruteBreaksDazeHorror".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.WrithesMoansPanickingSaw".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.ContactCumParasitesOverdrive".Localized());

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.ShiversEyesScrotumBegins".Localized());

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.GroaningSuccumbsMinistrations".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Minotaurscene.MinoUrethralPen.ObscenelyWrigglingSackBounces".Localized());
                else
                    if (Player.CumQ() < 5000)
                        OutputText("Scenes.Minotaurscene.MinoUrethralPen.HugelyBloatedDrapeAsshole".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.MinoUrethralPen.BeastBloatedLiftLegs".Localized());

                if (Player.CumQ() < 5000)
                    OutputText("Scenes.Minotaurscene.MinoUrethralPen.BeastCumsInfestedJizz".Localized(
                         Player.CockDescript(x)
                    ));
                else
                {
                    OutputText("Scenes.Minotaurscene.MinoUrethralPen.BeastRoarsNutsChurn".Localized());

                    OutputText("Scenes.Minotaurscene.MinoUrethralPen.WalkPullAgainstWall".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Minotaurscene.MinoUrethralPen.OvercomeLustFallsGround".Localized());

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.HandCoveredPrickSmear".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.GruntsSlowlyMoansStretching".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.SurveyResultsPleasedProud".Localized());

                OutputText("Scenes.Minotaurscene.MinoUrethralPen.KnowWontStruggleManaged".Localized());
            }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void MinoRapeIntro()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health <= 0)
            {
                OutputText("Scenes.Minotaurscene.MinoRapeIntro.RollConsciousAssKnees".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Minotaurscene.MinoRapeIntro.GlancingBackTwitchesLays".Localized(
                         CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Minotaurscene.MinoRapeIntro.GlancingBackWontRelieved".Localized(
                         CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));
            }
            else
            {
                OutputText("Scenes.Minotaurscene.MinoRapeIntro.RipLoinclothSlappingAss".Localized(
                     CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                ));

                if (Player.TotalNipples() > 1)
                    OutputText("Scenes.Minotaurscene.MinoRapeIntro.AndNipplesWord".Localized());

                OutputText("Scenes.Minotaurscene.MinoRapeIntro.SentencePeriod".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Minotaurscene.MinoRapeIntro.GlancingTwitchesLaysAgain".Localized(
                         CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Minotaurscene.MinoRapeIntro.GlancingBackDoubtsRelieve".Localized(
                         CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                    ));
            }
        }

        public void BumRapeaMinotaur()
        {

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            MinoRapeIntro();

            ShowImage("minotaur-win-male");

            OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.DroopYour".Localized());

            if (Player.CockTotal() == 1)
                OutputText(Player.CockDescript(x));
            else
                OutputText(Player.MultiCockDescriptLight());

            OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.AcrossHisUpraisedBackside".Localized());

            if (Player.CumQ() < 25)
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.DribblingSlickPrecumFur".Localized());

            if (Player.CumQ() >= 25 && Player.CumQ() < 250)
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.LeakingRopesMuscularAss".Localized());

            if (Player.CumQ() >= 250 && Player.CumQ() < 500)
            {
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.DrizzlingRopesCoatTailhole".Localized());

                CombatController.Instance!.Context.Enemy.Body.Get<AnalPart>().Wetness += 1;
            }

            if (Player.CumQ() > 500)
            {
                CombatController.Instance!.Context.Enemy.Body.Get<AnalPart>().Wetness += 2;

                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.HosingConstantFlowPrecum".Localized());
            }

            OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.GentlySqueezeBallsAlign".Localized());

            OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.ParagraphBreak".Localized());

            OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.LittleWarningSphincterRelax".Localized(
                 NumberUtils.Num2Text(Player.Cocks[0].Length),

                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.AtLastClimaxJackhammer".Localized());

            if (Player.CumQ() <= 25)
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.LongStrokesJizzSpurting".Localized(
                     Player.CockDescript(x)
                ));

            if (Player.CumQ() > 25 && Player.CumQ() <= 250)
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.LongStrokesThickGlobs".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

            if (Player.CumQ() > 250 && Player.CumQ() <= 500)
            {
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.LongStrokesGroaningPressure".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                ));

                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.EventuallyPullVictimFalls".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));
            }

            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.LongStrokesBodyTrembling".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     CombatController.Instance!.Context.Enemy.BallsDescriptLight(),

                     Player.CockDescript(x),

                     CombatController.Instance!.Context.Enemy.BallsDescriptLight()
                ));

                OutputText("Scenes.Minotaurscene.BumRapeaMinotaur.EcstaticSighPassesOut".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(15 + Player.Libido / 7)));
            }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void GirlRapeAMinotaur()
        {

            MinoRapeIntro();

            Player.SlimeFeed();

            MinoCumAddiction(5);

            OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.QuickShoveBackExposing".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                 Player.VaginaDescript(0)
            ));

            ShowImage("minotaur-win-female");

            if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.EvaporatesYourselfAching".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

            if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) && Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0) * 1.5f)
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.RestraintEvaporatesAchingAgain".Localized(
                     Player.VaginaDescript(0)
                ));

            if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) * 1.5f)
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.RestraintEvaporatesAchingThird".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.AtLastHandsGrab".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.ForcefullyGrindingVagina".Localized());
            else
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.HipsForcefullyGrindingDown".Localized());

            if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.EveryPressesTightlyWalls".Localized(
                     CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

            if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) && Player.VaginalCapacity() * 1.5f < CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.EveryStrokeDepositsPreseed".Localized(
                     CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

            if (Player.VaginalCapacity() * 1.5f >= CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.EverySlidesBuildPuddle".Localized(
                     CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.PicksPaceHeartbeatThudding".Localized(
                 Player.VaginaDescript(0),

                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.PassesContentedBallsShrunk".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Minotaurscene.GirlRapeAMinotaur.MixedPleasurePainNumbness".Localized(
                     Player.VaginaDescript(0)
                ));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Sensitivity, 1.5f), new DynStat(GameStats.Corruption, 1f));

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);

            CombatScene.CleanupAfterCombat();
        }

        public void MinotaurGetsRapedByHerms()
        {

            MinoRapeIntro();

            MinoCumAddiction(5);

            Player.SlimeFeed();

            ShowImage("minotaur-win-herm");

            OutputText("Scenes.Minotaurscene.MinotaurGetsRapedByHerms.LittleCareHandsPulling".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Minotaurscene.MinotaurGetsRapedByHerms.TwistedGrinPushForward".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                 Player.VaginaDescript(0)
            ));

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            if (Player.Vaginas[0].Looseness < 3)
                OutputText("Scenes.Minotaurscene.MinotaurGetsRapedByHerms.SmallSuppressedDimlyRegisters".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinotaurGetsRapedByHerms.IntenseFeelingsPowerfulDrug".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Minotaurscene.MinotaurGetsRapedByHerms.CockQuiversHeartSqueezing".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Minotaurscene.MinotaurGetsRapedByHerms.OrgasmsDragBellyDistended".Localized());

            Player.Orgasm("Vaginal");

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Sensitivity, 1.5f), new DynStat(GameStats.Corruption, 1f));

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);

            CombatScene.CleanupAfterCombat();
        }

        public void MinotaurFunWithUnbirthing()
        {
            ClearOutput();

            Player.SlimeFeed();

            MinoCumAddiction(5);

            OutputText("Scenes.Minotaurscene.MinotaurFunWithUnbirthing.LoomDefeatedFamiliarAche".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                 Player.VaginaDescript(0)
            ));

            ShowImage("minotaur-win-female");

            OutputText("Scenes.Minotaurscene.MinotaurFunWithUnbirthing.ComeMommyEvaporatesHorns".Localized(
                 Player.VaginaDescript(0),

                 Player.AssDescript()
            ));

            OutputText("Scenes.Minotaurscene.MinotaurFunWithUnbirthing.ParasitesResumeInsideWomb".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            Player.Orgasm("Vaginal");

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -2);

            Player.ApplyStats(new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 2f));

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);

            CombatScene.CleanupAfterCombat();
        }

        public void MinoPheromones()
        {

            OutputText("Scenes.Minotaurscene.MinoPheromones.SmilesLiftsThickRopes".Localized());

            if (Random.Shared.Next(3) == 0)
            {
                OutputText("Scenes.Minotaurscene.MinoPheromones.SlappingFaceWipeSlick".Localized());

                if (Player.LustPercent > 75)
                {
                    OutputText("Scenes.Minotaurscene.MinoPheromones.SwallowingMouthWithoutThinking".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));
                }
                else
                {
                    OutputText("Scenes.Minotaurscene.MinoPheromones.HeartBeatLicksResidue".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));
                }
            }
            else
                OutputText("Scenes.Minotaurscene.MinoPheromones.RightPastYourHead".Localized());

            OutputText("Scenes.Minotaurscene.MinoPheromones.AnimalisticScentMuskyAroma".Localized());

            Player.TakeLustDamage(10 + Player.Libido / 20);

            if (Player.HasPerk(PerkIds.MinotaurCumAddict) || CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 2)
            {
                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Minotaurscene.MinoPheromones.ShiverNeedFaceLoincloth".Localized());
                else
                    OutputText("Scenes.Minotaurscene.MinoPheromones.GroanLipsCravingTaste".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5 + Random.Shared.Next(5)));
            }

            if (Player.Lust >= Player.MaxLust)
                CombatController.Instance!.EndLustLoss();
            else
                CombatScene.CombatMenu();
        }

        public void GetRapedByMinotaur(bool autoRape = false)
        {
            ClearOutput();

            Player.SlimeFeed();

            var difficultyCheck = 17 + 2 * GameSettings.Difficulty;

            var levelCheck = 12 + 3 * GameSettings.Difficulty;

            if (!autoRape)
            {
                if (Random.Shared.Next(2) == 0 && Player.Body.Butt >= 15 && Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.BiggestCockArea() && Player.Tone < 60)
                {
                    GetMinoHawtDawged();

                    return ;
                }

                if (Random.Shared.Next(2) == 0 && !Player.IsTaur())
                {
                    GetOralRapedByMinotaur();

                    return ;
                }
            }
            else
            {
                ShowImage("encounter-minotaur");

                OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.WindingPathCaveOpening".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 5)));

                if (Random.Shared.Next(30) + Player.Intelligence / 5 > difficultyCheck)
                {
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SpotShadowMinotaurLumbering".Localized());

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntryMinotaurs);

                    CombatScene.Start(EnemyRegistry.Create(EnemyIds.Minotaur), () => Go<CampScene>());

                    return ;
                }

                OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.GrabbedArmsMassiveHand".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.AgainstYourButtocks".Localized());
                else
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.IntoSmallBack".Localized());

                OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.GrowsLargerSmearingPrecum".Localized());

                if (Random.Shared.Next(20) + Player.Strength / 3 > difficultyCheck || Random.Shared.Next(20) + Player.Speed / 5 > difficultyCheck || Player.Level >= levelCheck)
                {
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.TwistAroundSquirmFree".Localized());

                    CombatScene.Start(EnemyRegistry.Create(EnemyIds.Minotaur), () => Go<CampScene>());

                    return ;
                }
            }

            if (Player.Vaginas.Count > 0)
            {
                if (Player.Libido + Random.Shared.Next(15) > 50)
                {
                    ShowImage("minotaur-loss-butt-female");

                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.RoughlyGrabsPressAsshole".Localized());

                    Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                    MinoCumAddiction(15);

                    if (Player.IsTaur())
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.PositionsBehindLegsImpales".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.LiftsAirHeftingWeight".Localized());

                    if (Player.BiggestBreastSize() >= 1 && Player.MostBreastsPerRow() > 1 && Player.BreastRows.Count > 0)
                    {
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.TitsViolentlySqueezing".Localized());

                        if (Player.BiggestLactation() > 1.5f)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.GruntApprovalMilkSquirts".Localized());
                    }

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.BendsGrabsCrushingGrip".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.BendsForwardOneCock".Localized());

                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.FinallyReadyThrustsShorter".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -.5f), new DynStat(GameStats.Corruption, 1f));

                    if (Player.CockTotal() > 0 && (Player.Sensitivity + Random.Shared.Next(40) > 50))
                    {
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.YouOrgasmThird".Localized());

                        if (Player.CumQ() < 25)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SpurtingSeedBrutalRape".Localized());

                        if (Player.CumQ() >= 25 && Player.CumQ() < 250)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SquirtingRopesCumCave".Localized());

                        if (Player.CumQ() >= 250 && Player.CumQ() < 500)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.EruptingRopesThickBursts".Localized());

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.EruptingThickSeedForever".Localized());

                        Player.Orgasm("Anal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));
                    }

                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.RelaxesShovesColdGround".Localized());
                }
                else
                {
                    if (Player.DeerScore() >= 4)
                        ShowImage("minotaur-loss-vag-deer");
                    else
                        if (Player.CowScore() >= 4)
                            ShowImage("minotaur-loss-vag-cow");
                        else
                            if (Player.CatScore() >= 4)
                                ShowImage("minotaur-loss-vag-cat");
                            else
                                ShowImage("minotaur-loss-vag");

                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.RoughlyGrabsFlaredHead".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.AverageVaginalWetness() < 2)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.ArentWetFearDamage".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.ShamefullyWetKneesBuckling".Localized());

                    MinoCumAddiction(5);

                    if (Player.IsTaur())
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.PositionsLegsImpalesAgain".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.LiftsAirWeightAgain".Localized());

                    Player.CuntChange((24 * 3), true, false, true);

                    if (Player.BiggestBreastSize() >= 1 && Player.MostBreastsPerRow() > 1 && Player.BreastRows.Count > 0)
                    {
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.ManhandlesTitsSqueezingAgain".Localized());

                        if (Player.BiggestLactation() > 1.5f)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.GruntApprovalSquirtsAgain".Localized());
                    }

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.BendsGrabsGripAgain".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.BendsOneCockAgain".Localized());

                    if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.ClenchRibbedCockRavages".Localized());

                    if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) && Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0) * 1.5f)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.FeelMonstrousLoveCanal".Localized());

                    if (Player.VaginalCapacity() >= CombatController.Instance!.Context.Enemy.CockArea(0) * 1.5f)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.FeelMonstrousFuckHole".Localized());

                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.FinallyThrustsShorterAgain".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -.5f), new DynStat(GameStats.Corruption, 1f));

                    if (Player.AverageVaginalWetness() >= 2)
                    {
                        if (Player.AverageVaginalWetness() < 4)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SquirmQuiverRoughUsage".Localized());

                        if (Player.AverageVaginalWetness() == 4)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SquirmQuiverOrgasmingSoaking".Localized(
                                 Player.VaginaDescript(0)
                            ));

                        if (Player.AverageVaginalWetness() == 5)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.MassiveSplatteringGirlcum".Localized());

                        Player.Orgasm("Vaginal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));
                    }
                    else
                    {
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.GaspPainRubbedRaw".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Toughness, -.5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, -5f), new DynStat(GameStats.Corruption, 1f));
                    }

                    Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);

                    if (Player.CockTotal() > 0 && (Player.Sensitivity + Random.Shared.Next(40) > 50))
                    {
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.YouOrgasmAgain".Localized());

                        if (Player.CumQ() < 25)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SpurtingSeedRapeAgain".Localized());

                        if (Player.CumQ() >= 25 && Player.CumQ() < 250)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SquirtingRopesCaveAgain".Localized());

                        if (Player.CumQ() >= 250 && Player.CumQ() < 500)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.EruptingRopesBurstsAgain".Localized());

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.EruptingSeedForeverAgain".Localized());

                        Player.Orgasm("Anal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));
                    }

                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.RelaxesShovesGroundAgain".Localized());
                }
            }
            else
            {
                if (Player.IsTaur())
                    ShowImage("minotaur-loss-butt-centaur");
                else
                    if (Player.CowScore() >= 4)
                        ShowImage("minotaur-loss-butt-cow");
                    else
                        if (Player.DogScore() >= 4)
                            ShowImage("minotaur-loss-butt-dog");
                        else
                            if (Player.WolfScore() >= 4)
                                ShowImage("minotaur-loss-butt-wolf");
                            else
                                if (Player.GoblinScore() >= 4)
                                    ShowImage("minotaur-loss-butt-goblin");
                                else
                                    ShowImage("minotaur-loss-butt");

                OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.RoughlyGrabsAssholeAgain".Localized());

                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                MinoCumAddiction(15);

                if (Player.IsTaur())
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.PositionsLegsImpalesThird".Localized());
                else
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.LiftsAirWeightThird".Localized());

                if (Player.BiggestBreastSize() >= 1 && Player.MostBreastsPerRow() > 1 && Player.BreastRows.Count > 0)
                {
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.ManhandlesTitsSqueezingThird".Localized());

                    if (Player.BiggestLactation() > 1.5f)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.GruntApprovalSquirtsThird".Localized());
                }

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.BendsGrabsGripThird".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.BendsOneCockThird".Localized());

                OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.FinallyThrustsShorterThird".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -.5f), new DynStat(GameStats.Corruption, 1f));

                if (Player.CockTotal() > 0 && (Player.Sensitivity + Random.Shared.Next(40) > 50))
                {
                    OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.YouOrgasmThird".Localized());

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SpurtingSeedRapeThird".Localized());

                    if (Player.CumQ() >= 25 && Player.CumQ() < 250)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.SquirtingRopesCaveThird".Localized());

                    if (Player.CumQ() >= 250 && Player.CumQ() < 500)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.EruptingRopesBurstsThird".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.EruptingSeedForeverThird".Localized());

                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));
                }

                OutputText("Scenes.Minotaurscene.GetRapedByMinotaur.RelaxesShovesGroundThird".Localized());
            }

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void GetOralRapedByMinotaur()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.HugeGrabsFlipsOver".Localized());

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.TrapsSlamsBackWall".Localized(
                 Player.Legs(),

                 Player.Legs()
            ));

            if (Player.HasBreasts())
                OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.GrabsCockMaulingCalloused".Localized(
                     Appearance.BiggestBreastSizeDescript(Player)
                ));

            if (Player.CockTotal() >= 1)
            {
                OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.YourCocksSlap".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.PluralS".Localized());

                OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.PainfullyDragsAlongWall".Localized());
            }

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.AllSeeTipGrinding".Localized());

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.ReachesGrabsYankingOpen".Localized());

            ShowImage("minotaur-loss-oral");

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.WastingNoTimeThe".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.HearPopForcesFull".Localized());

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.ArchBackWallWeight".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            if (Player.HasBreasts())
                OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.PainfullyGrindingWiryFur".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.FeelHotBreathBalls".Localized());

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.SnortsJerksTwistHands".Localized());

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.NoYanksLegsLedge".Localized());

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.BellowsSlamsPastLips".Localized(
                 CombatController.Instance!.Context.Enemy.BallsDescriptLight(),

                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            Player.RefillHunger(50, false);

            OutputText("Scenes.Minotaurscene.GetOralRapedByMinotaur.GruntsTimesLegPulls".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            Player.Orgasm("Lips");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            MinoCumAddiction(10);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void MinoGetsTitFucked()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (Player.IsNaga())
            {
                AddictNagaOnMinotaur();

                return ;
            }

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.HelplessMinotaurLaysDirt".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.DefeatedWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.MasturbatingUncontrollablyWord".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.HesYoursHorseCock".Localized(
                 Appearance.AllBreastsDescript(Player)
            ));

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.PushHandsAwayAnd".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.YWord".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.OuLeanBreastFlesh".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.CockGrowsMoreAroused".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.BodyAchesArousalWhile".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.GrowsHotterBeggingTaken".Localized(
                     Player.VaginaDescript(0)
                ));
            else
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.AssholePuckersMonstrousBeast".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.ParagraphBreak".Localized());

            ShowImage("minotaur-win-titfuck");

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.UnderstandingFrustrated".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.CuntWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.AssWord".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.ProveWrongReleaseFace".Localized(
                 Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.BeginBreastsStrokingLength".Localized(
                 Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.BallsThrowsHeadBack".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.SqueezePressHeadFlare".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.RelaxesLastPassesOut".Localized());

            OutputText("Scenes.Minotaurscene.MinoGetsTitFucked.WakeHourUnsureCamp".Localized());

            MinoCumAddiction(10);

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, .5f), new DynStat(GameStats.Corruption, .7f));

            CombatScene.CleanupAfterCombat();
        }

        public void TakeMinoCumDirectly()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.VictoryDeeplyShivering".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.BeastManShaftRisen".Localized());
            else
                OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.BeastLegsSplayedMasturbating".Localized());

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.BlushingAnticipationReadying".Localized());

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.ShedArmorWaistTrembling".Localized());

            if (Player.AnalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.PushCopiousLubeDrug".Localized());
            else
                OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.MusclesInstantlyContactPre".Localized());

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.EyesOpenDoesntStruggle".Localized());

            ShowImage("minotaur-loss-butt");

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.BottomSighingHotFluids".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.ChestWord".Localized());
            else
                OutputText(Player.BreastDescript(0));

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.HungryBlissBouncingAtop".Localized());

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.FloodHeatedPumpsOnce".Localized());

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.ZoneHandsRoamingOrgiastic".Localized());

            OutputText("Scenes.Minotaurscene.TakeMinoCumDirectly.HourLaterMudReeks".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, -80f), new DynStat(GameStats.Corruption, 1f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            if (Player.SensitivityPercent < 80)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (Player.SensitivityPercent < 60)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (Player.SensitivityPercent < 40)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (Player.SensitivityPercent < 20)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

            MinoCumAddiction(15);

            CombatScene.CleanupAfterCombat();
        }

        public void MinoCumAddiction(float raw = 10)
        {

            Player.MinoCumAddiction(raw);
        }

        public bool MinoCumUpdate()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) == 0)
                return false;

            var output = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 0 && CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) >= 50 && OptionStore.Instance.Get("addiction", 1) > 0)
            {
                OutputText("Scenes.Minotaurscene.MinoCumUpdate.ArousedThoughtMinotaurs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionState, 1);

                output = true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) < 50)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 0)
                {
                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.FirstTimeHeadClear".Localized());

                    if (Player.Corruption > 60)
                        OutputText("Scenes.Minotaurscene.MinoCumUpdate.MissTinglingApathyFuzziness".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.MinoCumUpdate.ResolveBeastsWideBerth".Localized());

                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.BoldEndParagraph".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionState, 0);

                    output = true;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumResistanceTracker) >= 150 && !Player.HasPerk(PerkIds.MinotaurCumAddict) && !Player.HasPerk(PerkIds.MinotaurCumResistance))
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionTracker, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionState, 0);

                    Player.CreatePerk(PerkIds.MinotaurCumResistance, 0, 0, 0, 0);

                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.FinallyLearnedAvoidAddicted".Localized());

                    output = true;
                }
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 1 && CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) < 24)
                {
                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.SighHappilyCampNeeded".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionState, 1);

                    output = true;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) >= 24 && CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionState, 2);

                    output = true;

                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurSonsCumRepeatCooldown, 12);

                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.ShiverFeelingGetMore".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) >= 48 && CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 2)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionState, 3);

                    output = true;

                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurSonsCumRepeatCooldown, 12);

                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.SteadyGrowsGrabHead".Localized());
                }

                if ((CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) >= 100 && !Player.HasPerk(PerkIds.MinotaurCumResistance)) && (!(Player.HasPerk(PerkIds.MinotaurCumAddict))))
                    {
                        Player.CreatePerk(PerkIds.MinotaurCumAddict, 0, 0, 0, 0);

                        OutputText("Scenes.Minotaurscene.MinoCumUpdate.YouMinotaurCumAddict".Localized());

                        output = true;
                    }

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3 && CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) >= 48)
                    Player.ApplyStats(new DynStat(GameStats.Lust, 2f));

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 2 && CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) >= 24)
                    Player.ApplyStats(new DynStat(GameStats.Lust, 1f));

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionTracker, 100);
            }

            if ((CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) >= 48 && !Player.HasPerk(PerkIds.MinotaurCumAddict)) && (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) >= 0.15f))
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumResistanceTracker, CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumResistanceTracker) + 0.15f);

                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumAddictionTracker, CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) - 0.15f);
                }

            CoC.Instance.Flags.Set(GameFlagIds.TimeSinceLastConsumedMinotaurCum, CoC.Instance.Flags.Get(GameFlagIds.TimeSinceLastConsumedMinotaurCum) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumReallyAddictedState) > 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumReallyAddictedState, CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumReallyAddictedState) - 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumReallyAddictedState) == 0)
                {
                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.PainNoLongerIndistinguishable".Localized());

                    OutputText("Scenes.Minotaurscene.MinoCumUpdate.BoldEndParagraphAgain".Localized());

                    output = true;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumReallyAddictedState) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumReallyAddictedState, 0);
            }

            return output;
        }

        public void MinoAddictionFuck()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.MinoAddictionFuck.ExploringMountainsMuskyScent".Localized());

            ShowImage("monster-minotaur");

            OutputText("Scenes.Minotaurscene.MinoAddictionFuck.TossesAsideShaftLoincloth".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 3000f));

            DoNext(() => GetRapedByMinotaur());
        }

        public void MinoAddictionBadEndEncounter()
        {
            ClearOutput();

            ShowImage("area-mountain");

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEndEncounter.ExploringMountainsStrongWhiff".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) == 3 && Player.Intelligence < 60)
            {
                OutputText("Scenes.Minotaurscene.MinoAddictionBadEndEncounter.NeedSoGreatFollow".Localized());

                DoNext(() => MinoAddictionBadEnd2());
            }
            else
            {
                if (Player.Intelligence > 40)
                    OutputText("Scenes.Minotaurscene.MinoAddictionBadEndEncounter.TinyWarnsGetAway".Localized());

                OutputText("Scenes.Minotaurscene.MinoAddictionBadEndEncounter.DoFollowLikeAddict".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => MinoAddictionBadEnd2()); AddButton(1, "No", () => ReturnToCamp(1));
            }
        }

        public void MinoAddictionBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.GrinStumbleMountainInhaling".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.WetWord".Localized());

                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.AndHardWord".Localized());
            }
            else
                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.HardWord".Localized());
                else
                    OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.ShiverWord".Localized());

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.WithPupilsEyesGlazed".Localized());

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.WholeMinotaursCaveOpenings".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
                OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.ThirdBeastSuspendedAnkles".Localized());

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.GlancesPredatorySmile".Localized());

            ShowImage("minotaur-loss-oral");

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.WordsMusicWallowDirt".Localized());

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.NeedGrabsPullingUpward".Localized());

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.MasterPullsCollarDragging".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Minotaurscene.MinoAddictionBadEnd2.GruntsSecondTimeFlood".Localized());

            DoNext(() => MinoCumAddictBadEnd3());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -20f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 15f), new DynStat(GameStats.Lust, 50f), new DynStat(GameStats.Corruption, 10f));
        }

        public void MinoCumAddictBadEnd3()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.MinoCumAddictBadEnd3.DaysWeeksRememberedHaze".Localized());

            OutputText("Scenes.Minotaurscene.MinoCumAddictBadEnd3.NoLongerVillageCum".Localized());

            ShowImage("badend-minotaur");

            GameOver();

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 30f), new DynStat(GameStats.Lust, 100f), new DynStat(GameStats.Corruption, 20f));
        }

        public void MinotaurDrinksMilkNewsAtEleven()
        {
            ClearOutput();

            ShowImage("minotaur-win-breastfeeding");

            OutputText("Scenes.Minotaurscene.MinotaurDrinksMilkNewsAtEleven.KneelExhaustedHeadBreasts".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurDrinksMilkNewsAtEleven.PopNippleLeaveEnjoy".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f), new DynStat(GameStats.Lust, -50f));

            Player.Orgasm("Tits", false);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void MinotaurBlowjob()
        {
            ClearOutput();

            int x = Player.CockThatFits(80);

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.StepTowardsMinotaur".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.PeelingAwayArmorAnd".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.RevealingCocksBullArousal".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.TakingBullByHorns".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.PinningHornsCrookForelegs".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.GuideMouthMemberAutopilot".Localized(
                 Appearance.CockNoun(Player.Cocks[x].Id)
            ));

            ShowImage("minotaur-cocksucker");

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.GrimaceAwkwardRegainConfidence".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.HisOwnBurningLusts".Localized());
            else
                if (Player.HasPerk(PerkIds.ArousingAura))
                    OutputText("Scenes.Minotaurscene.MinotaurBlowjob.PowerOfYourCorruption".Localized());
                else
                    OutputText("Scenes.Minotaurscene.MinotaurBlowjob.SaltyTasteOfPrecum".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.LeavingHungryMore".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.RealizationAttemptsAwayWeak".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.HandsHornsBackHead".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.HipsBeginFaceDaze".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.ObligeBullFaceBobbing".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.YourHandWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.MinotaurBlowjob.AForehoofWord".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.QuicklyPutHimselfKnocking".Localized());

            OutputText("Scenes.Minotaurscene.MinotaurBlowjob.PullingFreeAbusedFalls".Localized());

            Player.Orgasm("Lips");

            CombatScene.CleanupAfterCombat();
        }

        public void LayEggsInAMinotaurSpiderLike()
        {
            ClearOutput();

            if (Player.CanOvipositBee())
            {
                LayBeeEggsInCowbutt();

                return ;
            }

            ShowImage("minotaur-win-male");

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.TurnsTriesStickyWebbing".Localized());

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.HelplessTiedWideEyes".Localized());

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.GatheringSilkLimbsOvipositor".Localized());

            if (!silly)
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.HisBovineRosebud".Localized());
            else
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.EntranceToOkCorral".Localized());

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.LiftAbdomenForwardEggs".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.ParagraphBreak".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.ForcingCockThighsFrot".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.GraspingBullAgainstPussy".Localized());
            }

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.ParagraphBreakAgain".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.OrgasmWashesSprayCum".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.SpatteringWord".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.DrenchingWord".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.NearlyDrowningWord".Localized());

                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.YourHelplessPrey".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.FemcumSpraysAcrossCock".Localized());

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.OvipositorPulsesEggAfter".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.CombinationScentFluidsAnd".Localized());

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.SensationEggsPastSphincter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 1)
            {
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.SmellSecondTimeFlare".Localized());

                MinoCumAddiction(3);
            }

            if (Player.Corruption < 50)
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.CarefullyLimbsSnoutLook".Localized());
            else
                OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.ThoughtDenizensWebbing".Localized());

            OutputText("Scenes.Minotaurscene.LayEggsInAMinotaurSpiderLike.WellAgainTime".Localized());

            Player.DumpEggs();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LayBeeEggsInCowbutt()
        {
            ClearOutput();

            ShowImage("minotaur-win-male");

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.LookingDefeatedMinotaur".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.StrokingFlaredCockDesperately".Localized());

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.ApiaryAppendageLeeringGrin".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.WrappingTailLiftFlanks".Localized());
            else
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.StraddlingLegsSlideForward".Localized());

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.WhileAimingOvipositor".Localized());

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.EggPhallusConfusedMoan".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.HesitateSurgePityInstincts".Localized());
            else
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.GrinningHornsHaulBack".Localized());

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.BeginThrustGruntingMoo".Localized());

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.BodyLongLayClimax".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.ThrustYourCock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.PluralS".Localized());

                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.ThroughAirFasterDrenching".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.BacksideWithYourCum".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.BackRopesDrippingHorns".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.BackCumCoversFloor".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.RubPussyMuscularAss".Localized());

            OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.FinallyPulsesAbdomenEggs".Localized());

            if (silly)
                OutputText("Scenes.Minotaurscene.LayBeeEggsInCowbutt.LookingMassEggsWhisper".Localized());

            Player.DumpEggs();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void GetMinoHawtDawged()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.CollapseKneesRacingCrotch".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.CockBiggestStrokeArmor".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.OtherHandRacesTo".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.VaginaDrippingArousalArmor".Localized());
            }
            else
                if (CombatController.Instance!.Context.Enemy.WeaponName != "axe")
                    OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.LastGreatLittleOff".Localized());
                else
                    OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.LastGreatUpsideHead".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.FootstepsBarelyRegister".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.EnormousSpewsLoadPre".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.ParagraphBreak".Localized());

                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.LinesCockBodyBetrays".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.FortunatelyUnfortunatelyCrown".Localized(
                     Player.VaginaDescript(0)
                ));
            }

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.HearSlurpingCrossingLips".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.BiteBackCarnalDelight".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.MuzzleDigsBulbousAss".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.FrustratedMoanTryWiggle".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.BroadLicksLapsButthole".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.TwitchingBeneathFloorPre".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.AlsoFeel".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.VaginaClenchingUnclenchingHips".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.BodyStartsAttentionSlick".Localized());

            ShowImage("minotaur-loss-butt");

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.SpittlingLubedFatButt".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.OnlyFeelBelowTailbone".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.ThisTimeHeadPops".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.AgainStrongHipsSurge".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.BouncingThrustsUsesAss".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.LowWhineCrotchHelplessly".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.HeadPushesFountainingSpunk".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.YourTailWord".Localized());

            OutputText("Scenes.Minotaurscene.GetMinoHawtDawged.YourBackGushesSeed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15 + Random.Shared.Next((int)(Player.Libido / 2))));

            Player.SlimeFeed();

            MinoCumAddiction(5);

            CombatScene.CleanupAfterCombat();
        }

        public void MinoMutual()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinoMutual) <= 0)
            {
                OutputText("Scenes.Minotaurscene.MinoMutual.EveryBulgesHorseCock".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.EvenThoughRapeBejeezus".Localized());
            }
            else
            {
                OutputText("Scenes.Minotaurscene.MinoMutual.ShakeHeadBringHimself".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.GrabHornyHornsGuide".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.WhenEyesIgniteHope".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.ReceivingGruntShakingHands".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinoMutual, CoC.Instance.Flags.Get(GameFlagIds.TimesMinoMutual) + 1);

            OutputText("Scenes.Minotaurscene.MinoMutual.EachSwingsErectLeaking".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutual.HotBathesFaceGroin".Localized());

            ShowImage("minotaur-cocksucker");

            OutputText("Scenes.Minotaurscene.MinoMutual.WhenLipsSurfaceTongue".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minotaurscene.MinoMutual.WetLipsKissTender".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutual.KnowingRightSucksHead".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutual.LooksLustEyesRutting".Localized());

            if (Player.Cocks[x].Length <= 10)
            {
                OutputText("Scenes.Minotaurscene.MinoMutual.SlowlyPullsSoftLips".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.VoiceComesGaspsHum".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.BeginsBobSlidesLoud".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.SaysMmmmDickIncreases".Localized());
            }
            else
                if (Player.Cocks[x].Length > 10 && Player.Cocks[x].Length <= 20)
                {
                    OutputText("Scenes.Minotaurscene.MinoMutual.ExaggeratedSlownessCockHot".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.VoiceComesDeepGroans".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.TrueMasculinePerseveresGulp".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.LookingKnowingDeepBreath".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.AsTwistsHeadSpasm".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.BeginsBobLengthThroat".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.GroansMmmmmmEquineCock".Localized());
                }
                else
                    if (Player.Cocks[x].Length > 20)
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutual.SlowlyPullsMouthCheeks".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.DroolDripsLipsShoves".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.GetsSupremeConcentration".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.SmileGracesDrippingGulp".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.DisheartenedShouldersSlump".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.GroansMmmmmmEquineDick".Localized());
                    }

            OutputText("Scenes.Minotaurscene.MinoMutual.StillStrokingFallsLips".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Human)
            {
                OutputText("Scenes.Minotaurscene.MinoMutual.TakesRubsFaceSmearing".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.StretchesThrustingCrevice".Localized());
            }
            else
                if (Player.HasKnot(x))
                {
                    OutputText("Scenes.Minotaurscene.MinoMutual.WrapsCuriousSwollenKnot".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.ThrustCreviceSpitSlick".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.StrongGraspsSuckingKnot".Localized());
                }
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutual.HoldsCockEncirclingBase".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.FaceNotSureAfraid".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.BeginsAcrossSegmentsSpit".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.IsntKissCockHead".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutual.ExaggeratedSlownessLicksBelly".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.OnceSatisfiedPoolingTongue".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.GiveGulpsPrizeLicks".Localized());
                    }

            if (Player.Balls > 0)
            {
                if (Player.BallSize == 1)
                {
                    OutputText("Scenes.Minotaurscene.MinoMutual.BigCupsMassagingLicks".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.NextBuriedSacSuckling".Localized());
                }
                else
                    if (Player.BallSize > 1 && Player.BallSize <= 4)
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutual.BigSlidesBallsMassaging".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.NextBuriedSucklingAgain".Localized());
                    }
                    else
                        if (Player.BallSize > 4)
                        {
                            OutputText("Scenes.Minotaurscene.MinoMutual.ReachingSlidesHandBalls".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutual.SlowlyGetsKissesLicks".Localized());
                        }

                OutputText("Scenes.Minotaurscene.MinoMutual.SmilesBallsBuryingNose".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.SaysEyesLiddedDrinks".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.BeginsInhalingAmbrosia".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutual.TongueRushesFlatBalls".Localized());
            }

            OutputText("Scenes.Minotaurscene.MinoMutual.OpensWideViolentlySucking".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minotaurscene.MinoMutual.EveryBallsLoudlyChin".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutual.KnowingHomeFuckWilling".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutual.GrowlShakesLipsCock".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutual.ObeysAdoringSticksTongue".Localized());

            if (Player.CumQ() <= 25)
            {
                OutputText("Scenes.Minotaurscene.MinoMutual.ShoveCockSpurtsRope".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minotaurscene.MinoMutual.GruntTriumphSucksSpurting".Localized());
                else
                    OutputText("Scenes.Minotaurscene.MinoMutual.ReleasesGruntSucksSpurting".Localized());
            }
            else
                if (Player.CumQ() > 25 && Player.CumQ() <= 250)
                {
                    OutputText("Scenes.Minotaurscene.MinoMutual.ShoveCockTongueErupts".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutual.HeadGoesLeavesBody".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Minotaurscene.MinoMutual.EuphoriaInducingMilkStreams".Localized());
                    else
                        OutputText("Scenes.Minotaurscene.MinoMutual.AsEuphoriaManMilk".Localized());
                }
                else
                    if (Player.CumQ() > 250 && Player.CumQ() < 500)
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutual.ShoveCockTongueBlasts".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutual.HeadGoesBlastsOut".Localized());

                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Minotaurscene.MinoMutual.GiganticFloodBatterFires".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutual.GripLoosensFillMouth".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutual.RepeatsProcessTimesSavoring".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Minotaurscene.MinoMutual.GiganticFloodFiresAgain".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutual.GripLoosensCumMouth".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutual.RepeatsProcessSavoringAgain".Localized());
                        }
                    }
                    else
                        if (Player.CumQ() >= 500)
                        {
                            OutputText("Scenes.Minotaurscene.MinoMutual.ShoveCockTongueExplodes".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutual.BodyWrackedEssenceShoots".Localized());

                            if (Player.Balls > 0)
                            {
                                OutputText("Scenes.Minotaurscene.MinoMutual.TsunamiManMilkArcs".Localized());

                                OutputText("Scenes.Minotaurscene.MinoMutual.CumForcesHitFace".Localized());

                                OutputText("Scenes.Minotaurscene.MinoMutual.ThirstyLipsHeadDesperately".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Minotaurscene.MinoMutual.TsunamiBabyBatterArcs".Localized());

                                OutputText("Scenes.Minotaurscene.MinoMutual.GripLoosensMouthThird".Localized());

                                OutputText("Scenes.Minotaurscene.MinoMutual.RepeatsProcessSavoringThird".Localized());
                            }
                        }

            Player.Orgasm("Dick");

            MinoMutualPt2(x);
        }

        public void MinoMutualPt2(int x)
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.ChestRisesSplendidExhaustion".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.KindSadNaturalBovine".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.LookRuttingCockPulses".Localized());

            ShowImage("minotaur-loss-oral");

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.TowerHorseReachesFace".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.WithoutWarningLockLips".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.SquirmsStrainsGirlyPitch".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.OnceFillHardAgainst".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.ListenRisesStrainedGroans".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.KeepingOnEdgeWhines".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.WhenLooksAweFear".Localized());

            if (Player.Cocks[x].Length < 15)
            {
                OutputText("Scenes.Minotaurscene.MinoMutualPt2.LookingBiggerDoesntMatter".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutualPt2.RidgesInterestingSpeedBumps".Localized());
            }
            else
                if (Player.Cocks[x].Length >= 15)
                {
                    OutputText("Scenes.Minotaurscene.MinoMutualPt2.LookingCockBigger".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutualPt2.RidgesFeelAgainstShaft".Localized());
                }

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.KeepItClenchTremble".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.ReachingUsePressDick".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.SoonPoundingAgainstHumping".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.SuddenlyStopFrustratedGrowl".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.EquinePenisFlaredHead".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.FreeHandCumSlit".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.FailsHipsLegsBending".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.BeforeDecideTremblingMoo".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.FromReactionEnjoyingStakes".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.GrindsAgainstLipInsatiable".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.ForceFourthBreaksCork".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.PluckFingerHoleLicking".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.DipIndexCumslitSeconds".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.PenetrateSecondFingerGasps".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.VoiceGainsThirdFinger".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt2.BeforeStartHowThumb".Localized());

            MinoMutualPt3(x);
        }

        public void MinoMutualPt3(int x)
        {
            ClearOutput();

            ShowImage("minotaur-win-male");

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.PryGripTugHand".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.GrinTakeLineUp".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.TakeCockWidenedHole".Localized());

            if (Player.Cocks[x].Length <= 7)
                OutputText("Scenes.Minotaurscene.MinoMutualPt3.GruntsSuddenlyTightHole".Localized());
            else
                if (Player.Cocks[x].Length > 7 && Player.Cocks[x].Length <= 11)
                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.GroansNotPainStretch".Localized());
                else
                    if (Player.Cocks[x].Length > 11)
                        OutputText("Scenes.Minotaurscene.MinoMutualPt3.FaceContortsGrowlUrethra".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.FuckBigHardShoving".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.WipeLeftCumThigh".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.WorkBreakRiseFall".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.SmirkingMischievouslyLustDraft".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.FewMomentsFluidBubble".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.AsUnaskedColourLighter".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.GrinningPressLipsFlare".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Minotaurscene.MinoMutualPt3.InstantlyFullMixtureAltered".Localized());
            else
                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.InstantlyFullBegsFilled".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.DrinkCocktailHeadyJoy".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.GaspsBreathGrabCock".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.PressCockEntranceAnal".Localized());

            if (Player.Cocks[x].Length < 10)
            {
                OutputText("Scenes.Minotaurscene.MinoMutualPt3.BullmanWincesBlissfullyTight".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutualPt3.DontGiveMindPump".Localized());
            }
            else
                if (Player.Cocks[x].Length >= 10 && Player.Cocks[x].Length <= 24)
                {
                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.BullmanPainedHipsForcing".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.NowManagedShapePist".Localized());
                }
                else
                    if (Player.Cocks[x].Length > 24)
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutualPt3.EntireTensesHeadUnbearably".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutualPt3.StayEyesSqueezeShut".Localized());
                    }

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.HipsCreateSpreadAss".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.MountingRuthlessThrustsWhirl".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.DecideGoGrabHuge".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.ScreamsSensationsDick".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.DesiresWellTuggingNipple".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.GivingEverythingTellBoth".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.BovineReleasesMooVibrates".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.HappyShiversFuckPieces".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minotaurscene.MinoMutualPt3.FeelBallsGoesDark".Localized());

            if (Player.CumQ() <= 25)
            {
                OutputText("Scenes.Minotaurscene.MinoMutualPt3.ShiverSpurtsGroaningBull".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutualPt3.LookingBaredMuscleStands".Localized());

                OutputText("Scenes.Minotaurscene.MinoMutualPt3.VoiceRisesAssJism".Localized());
            }
            else
                if (Player.CumQ() > 25 && Player.CumQ() <= 250)
                {
                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.ShiverPumpsPearlySeed".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.LookingBaredTeethMuscle".Localized());

                    OutputText("Scenes.Minotaurscene.MinoMutualPt3.VoicesRisesFleshyApplause".Localized());
                }
                else
                    if (Player.CumQ() > 250 && Player.CumQ() < 500)
                    {
                        OutputText("Scenes.Minotaurscene.MinoMutualPt3.FuckGushingClenchingHole".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutualPt3.LookingTeethBaredEcstasy".Localized());

                        OutputText("Scenes.Minotaurscene.MinoMutualPt3.VoiceRisesDecimateRear".Localized());
                    }
                    else
                        if (Player.CumQ() >= 500)
                        {
                            OutputText("Scenes.Minotaurscene.MinoMutualPt3.SlamBlissFuckHole".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutualPt3.LookingSeeTeethBared".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutualPt3.VoiceRisesMuscledAss".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutualPt3.FeelingExhaustedShiningFace".Localized());

                            OutputText("Scenes.Minotaurscene.MinoMutualPt3.SpentDrippingLimbsFall".Localized());
                        }

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.NotLegsBackOpen".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.OpenMouthRumbleAccepts".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.OrderObeysOpeningMouth".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.LeanForwardWetTongue".Localized());

            OutputText("Scenes.Minotaurscene.MinoMutualPt3.ByDressedSoftlyLeave".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void JoinBeingAMinoCumSlut()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.ProspectHugeTooMuch".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.CowHandsLowLedge".Localized());

            if (Player.Tallness <= 96)
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.DespiteImmenseHeight".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AsLookAreaSilent".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.GruntsFinallyTurnsBack".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AllContinuesGruntingThrusting".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.IgnoringYourCock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.PluralS".Localized());

                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.Entirely".Localized());
            }

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.SlippingFingerIntoMoistening".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.PussyWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AssholeWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.LowSecondBustyBovine".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.SwirlsAroundClitoris".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.PressesAgainstPerineum".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.SentencePeriod".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BroadShoulderedOntoKnees".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WithoutWastingLeansDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.OnceAgainManhoodCompletely".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.DivesTongueFirstWet".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.QuimWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BackDoorWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.MovementSuppressPervertedMoan".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.SoddenBoxWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.MoistButtholeWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.RemainingFearsSexShow".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.TongueLavishesHomageCrotch".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BullThrustsFinalBellow".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.GirlGibbersRigidCock".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.RoughHandsDrawCloser".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.TauntsEagerSlutTurgid".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WithinKnockedManlyBeast".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WithinYourBowels".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.GruntGoodCockBelly".Localized());

            if (Player.HasVagina())
                Player.CuntChange(36, true, true, false);
            else
                Player.ButtChange(36, true, true, false);

            ShowImage("minotaur-cumslut");

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AhhYeahGood".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.CowPussyWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AssWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.RightGroansBombasticCock".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.GSpotWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.ProstateWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WideFlareStrokesThrough".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.VaginalWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AnalWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WallsContainedPleasureBreasts".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.GiggleSideBackUp".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.PartMindQuicklySilenced".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.SerpentineWord".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.DemonicWord".Localized());
                else
                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                        OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.DraconicWord".Localized());
                    else
                        if (Player.HasLongTongue())
                            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.InhumanlyLongWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.TongueInsideLickingRemnants".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.ForNoCreamPie".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WombWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BackDoorWordAgain".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.IncreasesLimbsTinglingProspect".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.StartsFingersSeetheGrass".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.VaginaWord".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AssholeWordAgain".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.LikeChristmasAwareMovement".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.MusclesLetsLooseRoar".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WombWordAgain".Localized());
            else
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BowelsWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.MomentHitsLightningBolt".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.ForAddledMindCocks".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.NewWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.WombWithDivineDick".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.TakesMinutesComeHigh".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.GingerlySitBodyQuaking".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AsPossessTelepathyLean".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.UppermostWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BreastsAroundFacesMeet".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.TwoBreastsBarelyLeaving".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.BullGroansThirdOrgasm".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.SadlyRelinquishBackWatch".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.NowSatisfiedPreciousSpunk".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.EllipsisWord".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.AwakenHoursNowhereSeen".Localized());

            OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.QuicklyDressCampGoblin".Localized());

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, 1f));
            // 原版 MinotaurScene.hx:1898 — CeraphScene.hx：已解锁 Ceraph 的露出剧情
            if (SceneInstance<Npcs.CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Minotaurscene.JoinBeingAMinoCumSlut.ThrillRunsThoughClimax".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
            }

            Player.MinoCumAddiction(10);

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);

            if (Player.HasStatusEffect(StatusEffectIds.Feeder))
            {
                Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
            }

            View.Time.Hours += 1;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.MinotaurCum), 1);

ReturnToCamp(1);
        }

        public void WatchAMinoCumSlut()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurscene.WatchAMinoCumSlut.DecidingNotNookWatch".Localized());

            OutputText("Scenes.Minotaurscene.WatchAMinoCumSlut.BellowsGrabsCushionyAss".Localized());

            ShowImage("minotaur-cumslut");

            OutputText("Scenes.Minotaurscene.WatchAMinoCumSlut.HeartyPlungesFuckHole".Localized());

            OutputText("Scenes.Minotaurscene.WatchAMinoCumSlut.PairSettlesGruntsGroans".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WatchMinoCumSlutII);
        }

        public void WatchMinoCumSlutII()
        {
            ClearOutput();

            ShowImage("minotaur-cumslut");

            OutputText("Scenes.Minotaurscene.WatchMinoCumSlutII.GoItObliviousIntensity".Localized());

            OutputText("Scenes.Minotaurscene.WatchMinoCumSlutII.LookCoveredCreaturesExhausted".Localized());

            OutputText("Scenes.Minotaurscene.WatchMinoCumSlutII.OnlyFacesLedgesRidges".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ContinueMinoVoyeurism()
        {
            ClearOutput();

            ShowImage("minotaur-cumslut");

            OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.GoItObliviousIntensity".Localized());

            OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.LookCoveredCreaturesLaying".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.InDisgust".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.InConfusion".Localized());
                else
                    OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.Reluctantly".Localized());

            OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.OnlyNoticeFacesLedges".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.MinoPlusCowgirl, 0) == 0)
                OutputText("Scenes.Minotaurscene.ContinueMinoVoyeurism.ApparentlyNotLocalsEnjoy".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20 + Player.MinoScore() + Player.CowScore())));

            DoNext(() => ReturnToCamp(1));
        }

    }
}