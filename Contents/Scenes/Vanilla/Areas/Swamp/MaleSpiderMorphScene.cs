using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register<MaleSpiderMorphScene>("coc:male_spider_morph")]
    public class MaleSpiderMorphScene : Scene
    {

        public override void Entry()
        {
            GreetMaleSpiderMorph();
        }

        public void GreetMaleSpiderMorph()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.GreetMaleSpiderMorph.DropsFromTree".Localized());

            if (Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Malespidermorphscene.GreetMaleSpiderMorph.FlipsDown".Localized());

                CombatScene.Start(EnemyRegistry.Create(EnemyIds.MaleSpiderMorph), () => Go<CampScene>());
            }
            else
            {
                OutputText("Scenes.Malespidermorphscene.GreetMaleSpiderMorph.BreaksSmile".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightSpiderBoy);

                AddButton(1, "Common.Button.Talk".Localized(), TalkToSpiderBoy);

                SetExitButton("Common.Button.Back".Localized());
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryArachnes);
        }

        public void FightSpiderBoy()
        {

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.MaleSpiderMorph), () => Go<CampScene>(), canFlee: false);
        }

        public void TalkToSpiderBoy()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.GrinsWider".Localized());

            Player.RefillHunger(20, false);

            OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.TellsOfPeople".Localized());

            OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.TalksOwnPeople".Localized());

            OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.ArachnidComments".Localized());

            if (Random.Shared.Next(6) != 0)
            {
                OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.EatWebbing".Localized());

                OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.PinkFibers".Localized());

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SweetGossamer), 1);

ReturnToCamp(1);
            }
            else
            {
                OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.WebbingTough".Localized());

                OutputText("Scenes.Malespidermorphscene.TalkToSpiderBoy.WhiteFibers".Localized());

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.ToughSilk), 1);

ReturnToCamp(1);
            }
        }

        public void DefeatSpiderBoy()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.DefeatSpiderBoy.CollapsesKnees".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Malespidermorphscene.DefeatSpiderBoy.MasturbatesFuriously".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.DefeatSpiderBoy.WobblesUpright".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Mount".Localized(), null);

            AddButton(1, "Common.Button.Fuckhisbutt".Localized(), null);

            AddButton(2, "Common.Button.Frot".Localized(), null);

            if (!Player.IsGenderless() && Player.Lust >= 33)
            {
                OutputText("Scenes.Malespidermorphscene.DefeatSpiderBoy.WhatDoYouDo".Localized());

                if (Player.HasVagina())
                {
                    AddButton(0, "Common.Button.Mount".Localized(), VictoryCowgirlRidingOnSpiderBoi);

                    if (Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                        AddButton(3, "Common.Button.BikiniTitfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));
                }

                if (Player.HasCock())
                {
                    if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) != -1)
                        AddButton(1, "Common.Button.Fuckhisbutt".Localized(), VictoryButtFuck);

                    if (Player.BiggestCockArea() > CombatController.Instance!.Context.Enemy.AnalCapacity())
                        AddButton(2, "Common.Button.Frot".Localized(), VictoryFrotTheSpoidah);
                }
            }

            if (OptionStore.Instance.Get("gore", 0) > 0)
                AddNextButton("Common.Button.Amputate".Localized(), null);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void LoseToMaleSpiderMorph()
        {
            ClearOutput();

            var choices = new List<int>();

            if (Player.HasCock())
                choices.Add(0);

            if (Player.HasVagina())
                choices.Add(1);

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Malespidermorphscene.LoseToMaleSpiderMorph.TearsArmor".Localized());

                Player.Health = 1;

                CombatScene.CleanupAfterCombat();

                return ;
            }

            float select = choices[Random.Shared.Next(choices.Count)];

            if (select == 0)
                SpiderBoyLossMaleButtfuck();
            else
                LoseToSpiderBoyVagFucked();
        }

        public void VictoryButtFuck()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.TurnsSpiderAround".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.DistractedFondling".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.TooBusyUpright".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.WideSmileGrab".Localized(
                 Player.SMultiCockDesc()
            ));

            if (Player.LustPercent < 70)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.HardensFullSize".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.BobsHappily".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.EagerForHole".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.RealizesIntent".Localized());

            if (Random.Shared.Next(3) == 0)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.NeverHadAnything".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.DontWantAnything".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.SayEndBreak".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.CaressesBalls".Localized());

            if (CurrentMonster.LustPercent <= 99)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.AfterHeTouches".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.WhileHeFondles".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.LinesItUp".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.GentlePressure".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x),

                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.OtherPenis".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.RestingSmallBack".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.SplaysOverButt".Localized());
            }

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.ParagraphBreak".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.CriesOut".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.ChucklesAdmission".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.SquirmsUnder".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.WhispersFeelsGood".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.OwnSuffix".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.BriefInstant".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.MostOfIt".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.GoesWild".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.LeftoverDick".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.LeftoverDicks".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.BellyIs".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.HeavyAnd".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.SlightlyWord".Localized());

                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.DistendedFinish".Localized());
            }

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.PushesBoyOff".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.TricklesWord".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.StreamsWord".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.RiversWord".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryButtFuck.SemenSliding".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryFrotTheSpoidah()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.PushesThe".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.MasturbatingWord".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.InjuredWord".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.OntoHisBack".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.DropsSquarely".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.DropsLargest".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.AtopMiniature".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.PantsSurprise".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.GaspsSlowly".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.SwaysHips".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.TrappedBelow".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.HumpsForward".Localized(
                 Player.CockHead()
            ));

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.LooksUpTwinkle".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.UnquenchableDesire".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.NoDecency".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.SpasmPleasure".Localized());

            if (Player.CumQ() >= 750)
            {
                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.AfterYouFinish".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.HugeWord".Localized());

                OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.PuddleUnderHead".Localized());
            }

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.ParagraphBreak".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryFrotTheSpoidah.StretchesContentment".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryCowgirlRidingOnSpiderBoi()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.RemovesArmor".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.StayingUpright".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.DrowningPleasure".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.TooIrritating".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.DrippingDick".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.StiffeningDick".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.ExploresBody".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.StiffPreCum".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.CockHoodFlesh".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.ParagraphBreak".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.LooksUpWide".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.LiftingBalls".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.AtopFoldedSkin".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.PinchesNipple".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.WhinesComing".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.IdeaComes".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.WithoutWaiting".Localized(
                 Player.VaginaDescript(0)
            ));

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.PinchWord".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.FingerWord".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.StrugglingNipples".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.LetsHimLoose".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.OrgasmArrives".Localized());

            OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.RegainsComposure".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.CleansUp".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.WipesWorst".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.VictoryCowgirlRidingOnSpiderBoi.IgnoresThick".Localized());

            Player.SlimeFeed();

            Player.KnockUp(PregnancyIds.Spider, PregnancyIds.SpiderIncubation, 151);

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void SpiderBoyLossMaleButtfuck()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.YouCollapse".Localized());

            if (Player.Health < 1)
            {
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SemiConsciousHeap".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.InFear".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.WithWorry".Localized());
                    else
                        OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.InAnticipation".Localized());

                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.AtMaleSpider".Localized());
            }
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.OverwhelmedDesire".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.LooksDownWith".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.ForeplayFinished".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.HindquartersSlightly".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.UpOutOfWay".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SurprisingStrength".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SilentlyMouth".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.LittleNod".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.OnlySqueeze".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.BouncesBelly".Localized(
                     Player.SMultiCockDesc()
                ));
            else
            {
                if (Player.LustPercent < 50)
                    OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SlowlyFills".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.FillsCompletely".Localized());

                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.TinglesRadiating".Localized());
            }

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.PenetrationIs".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.MercifullyEasy".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.QuiteEasy".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.FirmHand".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.PurpleEyedMan".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.NotLeftEmpty".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.ARibbon".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.RibbonsWord".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.ThickCumDrizzle".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SLetter".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.LeaksOverBelly".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.ButtSlutComment".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.BlushUtterly".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.BlushHumiliated".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.BlushArousal".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.PumpsSlowly".Localized());

            if (Player.Anal.Looseness >= 3)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.OkaySuffix".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.GreatWord".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.CockSleeve".Localized());

            if (Player.Anal.Looseness >= 3)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.WishLessLoose".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.LieThereTakeIt".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SighGoodSpot".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.CrossesEyes".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SemenContinues".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.SemenTrailsOff".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.DrowningBliss".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.MumblesFuckMe".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.GroansLittleSlut".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.BouncesLeaking".Localized(
                     Player.SMultiCockDesc()
                ));
            else
                OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.ExplodesAgain".Localized(
                     Player.SMultiCockDesc()
                ));

            OutputText("Scenes.Malespidermorphscene.SpiderBoyLossMaleButtfuck.EyelidsDroop".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToSpiderBoyVagFucked()
        {
            ClearOutput();

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.AsYour".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.WoundsOverwhelm".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.LustOverwhelms".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.LegsGiveOut".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.CaressesRecklessly".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.TooInjured".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.PouncesFast".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.PlungesTongue".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.CirclesTongue".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.PeriodBreak".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.ArchesBack".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.InjuredOralAttentions".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.TriedToFight".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.HandlesBody".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.HalfStifled".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.MoansPleasure".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.EyesSeem".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.SoakedWord".Localized());
            else
                if (Player.Wetness() >= 2)
                    OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.WetWord".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.MoistWord".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.MonsTeasing".Localized());

            if (Player.GetClitLength() >= 3)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.FrottingAgainst".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.GrindingUnderside".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.RocksEncouragingly".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.KissesNipples".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.PoursLube".Localized());
            else
                if (Player.Wetness() >= 3)
                    OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.DroolsStream".Localized());
                else
                    if (Player.Wetness() >= 2)
                        OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.DribblesLube".Localized());
                    else
                        OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.GetsSoaked".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.YourSlot".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.DripJuices".Localized());
            else
                if (Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.BeadMilk".Localized());
                else
                    OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.FullyEngorge".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.ParagraphBreak".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.FullAndHard".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.AndNipples".Localized());

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.BouncesOnNipples".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.PulsatesBeat".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.GurglesHappily".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.KeepsLips".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false, true);

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.RipplingCock".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.FucksLikeAnimal".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.TwinNeedles".Localized());

            OutputText("Scenes.Malespidermorphscene.LoseToSpiderBoyVagFucked.SighsClosesEyes".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.KnockUp(PregnancyIds.Spider, PregnancyIds.SpiderIncubation);

            CombatScene.CleanupAfterCombat();
        }

        public void SpiderPregVagBirth()
        {

            OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.ParagraphBreak".Localized());

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.TerriblePressure".Localized());

                Player.CreateVagina();
            }

            OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.ShuddersGrowling".Localized(
                 Player.VaginaDescript(0),

                 Player.VaginaDescript(0)
            ));

            if (Player.Gender != Gender.Female)
                OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.SpraysCum".Localized());
            else
                OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.SpraysJuices".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.LostCount".Localized());

            View.Time.Hours += 2;

            OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.AwakensSticky".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.RagingHardOn".Localized());

            OutputText("Scenes.Malespidermorphscene.SpiderPregVagBirth.SlimyPool".Localized());

            Player.Orgasm("Vaginal");
        }

    }
}