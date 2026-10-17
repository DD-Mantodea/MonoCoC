using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp
{
    [Register<CorruptedDriderScene>("coc:corrupted_drider")]
    public class CorruptedDriderScene : Scene
    {

        public override void Entry()
        {
            DriderEncounter();
        }

        public void DriderEncounter()
        {
            ClearOutput();

            var drider = EnemyRegistry.Create(EnemyIds.CorruptedDrider);

            ShowImage("monster-corrupteddrider");

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredDrider) == 0)
            {
                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.SwampAtTwilight".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.DriderAppears".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.LovelyPrey".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.RunOrFight".Localized());
            }
            else
            {
                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.SpiderTerritory".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.StalkerRevealsItself".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.DemandAmbusher".Localized());

                if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                    OutputText("Scenes.Corrupteddriderscene.DriderEncounter.TailWord".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.DriderEncounter.HairWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.TugPullsBack".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.CleverLittlePet".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderEncounter.SadisticDisplay".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEncounteredDrider, CoC.Instance.Flags.Get(GameFlagIds.TimesEncounteredDrider) + 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryArachnes);

            CombatScene.Start(drider, () => Go<CampScene>());
        }

        public void DefeatDriderIntro()
        {
            ClearOutput();

            OutputText("Scenes.Corrupteddriderscene.DefeatDriderIntro.LegsCollapse".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.DefeatDriderIntro.TriesToFight".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.DefeatDriderIntro.MasturbatesInstead".Localized());

            OutputText("Scenes.Corrupteddriderscene.DefeatDriderIntro.DefeatedHeap".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ButtFuck".Localized(), null);

            AddButton(1, "Common.Button.FuckPussy".Localized(), null);

            AddButton(2, "Common.Button.BondageFuck".Localized(), null);

            AddButton(3, "Common.Button.Fuckspinner".Localized(), null);

            AddButton(4, "Common.Button.RideCock".Localized(), null);

            AddButton(5, "Common.Button.RideOvi".Localized(), null);

            AddButton(6, "Common.Button.Rideovianal".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()))
                    AddButton(0, "Common.Button.ButtFuck".Localized(), ButtFuckADriderOhBaby);

                if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                {
                    AddButton(1, "Common.Button.FuckPussy".Localized(), WinDriderPCDickInSpiderCunt);

                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.SpiderAbdomen || Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk)))
                        AddButton(2, "Common.Button.BondageFuck".Localized(), DriderVagSmartFuck);
                }

                if (Player.HasCockThatFits(12))
                    AddButton(3, "Common.Button.Fuckspinner".Localized(), VictoryVSDriderStickDickInSpinneret);

                if (Player.HasVagina())
                {
                    AddButton(4, "Common.Button.RideCock".Localized(), WinVSDriderTakeItsCockInCunt);

                    AddButton(5, "Common.Button.RideOvi".Localized(), VictoryVSDriderRideOviVaginal);

                    if (Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                        AddButton(7, "Common.Button.RideCock".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));
                }

                AddButton(6, "Common.Button.Rideovianal".Localized(), VictoryVSDriderRideOviAnal);
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void LoseToDrider()
        {

            var select = new List<int>();

            if (Player.HasCock())
                select.Add(0);

            if (Player.HasVagina())
                select.Add(1);

            if (Player.Gender == Gender.Genderless || ((int)Player.Gender > 0 && Random.Shared.Next(3) == 0))
                select.Add(2);

            float choice = select[Random.Shared.Next(select.Count)];

            if (choice == 2)
                LoseVSDriderGetAnalledOvi();
            else
                if (choice == 1)
                    LoseVSDriderAndVaginallyImpreggleOvi();
                else
                    LoseVSDriderAndGetFuckedANDAnalImpreggegity();
        }

        public void WinDriderPCDickInSpiderCunt()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.EagerToPlumb".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SheLooksAgain".Localized());

                if (Player.Tallness > 72)
                    OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.UpWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.LustfulEyesWide".Localized());
            }
            else
            {
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SheLooksAgain".Localized());

                if (Player.Tallness > 72)
                    OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.UpWordAgain".Localized());

                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.DisgustedLook".Localized());

                if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                    OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ChainsJingle".Localized());

                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.LowerLipQuivers".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SaunterOver".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SighsAtTouch".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.LewdMoans".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.LuridMoans".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.PleasureAndHer".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ChainPierced".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.NipplesHardAsArmor".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.PiercedWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.GropesBreasts".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.FondlingFourTits".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.HalfInchAtTime".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.BodyInstincts".Localized(
                 Player.CockHead(x)
            ));

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ClinkingChains".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SwollenClitProds".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.YourExtraPenis".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ExtraPenisesFanOut".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ExtraPenisSticksOut".Localized());
            }
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.CockBulgesInside".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ArachnidHussy".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.InvigoratedDesire".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.HeavingMightily".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.PiercedWordAgain".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.WhiteSkinned".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.BreastsMash".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.BackArches".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.FillsMassiveWomb".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.DistractedByTension".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.HumsIntoCollarbone".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.WiggleFutilely".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.CocksHerHead".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ShesRight".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ReadingYourThoughts".Localized());

            if (Player.Corruption > 80 && Player.Libido > 50)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.OfCourseItsTruth".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.NoThatsNotRight".Localized(
                     Player.CockDescript(x),

                     Player.Mf("stud", "slut")
                ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.DriderCroons".Localized());

            if (Player.Corruption > 80 && Player.Libido > 50)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.GrumbleDenial".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.MumbleDenial".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.LeakingPreCum".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ExgartuanMumbles".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SheAdds".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.HardToThink".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Corruption > 80 && Player.Libido > 50)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.VenomParalyzing".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SomethingWrong".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.PausesToGaze".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.NodsAgainstBreasts".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.DickGettingHarder".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.MoanAndPreCum".Localized(
                 Player.CockDescript(x)
            ));

            ShowImage("cDrider-sex-male");

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ChitinousBitch".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.PlaintiveWhining".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.GigglesAndKisses".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.BlushingFace".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.HeadHangsLimply".Localized());

            if (Player.Corruption > 80 && Player.Libido > 50)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.BadIdeaNotBind".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.BadIdea".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ControllingRebelliousDick".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.MoansLouder".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.MewlAffirmation".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ShuddersHeadToToes".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.FirstSpurt".Localized());

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.PackedItFull".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.LegsSkitterTremble".Localized());

            if (Player.CumQ() > 1000)
            {
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.CumDribblesOut".Localized());

                if (Player.CumQ() > 2000)
                    OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.FlowThickens".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ParagraphBreakThird".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.OrgasmEventuallyEnds".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.ArachnidRollsOver".Localized());

            if (Player.CumQ() <= 1000)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SplayedAcrossGround".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.SoakingInSpooge".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt.UnconsciousContractions".Localized());

            DoNext(() => WinDriderPCDickInSpiderCunt2());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));
        }

        public void WinDriderPCDickInSpiderCunt2()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.SomeTimeLater".Localized());

            ShowImage("cDrider-win");

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.PistoningViolently".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.LowGrowl".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.EvenLater".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.CollapseIntoDirt".Localized());

            if (Player.Corruption > 80 && Player.Libido > 50)
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.StrengthReturns".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.RememberYourself".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinDriderPCDickInSpiderCunt2.SnuggleAgainst".Localized());

            CombatController.Instance!.Context.Enemy.Lust = 98;

            CombatController.Instance!.Context.Enemy.Health = 2;

            Player.Lust = Player.MaxLust;

            CoC.Instance.Flags.Set(GameFlagIds.CombatBonusXpValue, CombatController.Instance!.Context.Enemy.Experience);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));
        }

        public void WinVSDriderTakeItsCockInCunt()
        {
            ClearOutput();

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.EyesLowerToShaft".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.AvoidingSpindlyLegs".Localized(
                 Player.AssDescript(),

                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.HoldingTopPortion".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.ShaftBeginsToGrow".Localized());

            ShowImage("cDrider-win-vag");

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.MoanAsSheEnters".Localized());

            if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.BottomsOut".Localized());
            else
                if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0) * 2)
                    OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.FitsTightly".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.BarelyHalfway".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.InsideOfYou".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.LeanDownStraddling".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.PiercedNipplesStud".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.MoistBud".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.WithYourTongue".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.DemonicLengthWraps".Localized());

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.FreeHandMassage".Localized(
                 Player.AssDescript(),

                 Player.VaginaDescript(0),

                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Corrupteddriderscene.WinVSDriderTakeItsCockInCunt.ComingToClimax".Localized());

            Player.KnockUp(PregnancyIds.DriderEggs, PregnancyIds.DriderIncubation, 151);

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryVSDriderRideOviVaginal()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.EyeingTheLump".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.HateWord".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.LustWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.PouringFromFourEyes".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.PerfectViewOfEndowments".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.LookOfHate".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.HateFades".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.AndEachCockTwitch".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.TwitchesWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.AtTheThought".Localized());
            }
            else
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.LustDeepens".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.EachCockTwitchAgain".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.TwitchesWordAgain".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.InAnticipation".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.SitOnAbdomen".Localized());

            ShowImage("cDrider-loss-vag");

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.ShiftingBack".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.PiercedWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.GropesOwnTits".Localized());

            Player.CuntChange(30, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.GaspArchingBack".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.HalfwayDown".Localized());

            Player.CuntChange(50, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.MaulingHerTits".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.LookBackIrritation".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.ShudderingClimax".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.MomentOfPain".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.CocksRetaliate".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.PluralS".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.AutomaticallyDumping".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.ScoresOf".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.Unending".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.JizzRopesOnGroin".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.GutturalSounds".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.VocalizingAnimal".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.AftershocksLimp".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviVaginal.WatchHerForTime".Localized());

            Player.KnockUp(PregnancyIds.DriderEggs, PregnancyIds.DriderIncubation, 151);

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryVSDriderRideOviAnal()
        {
            ClearOutput();

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.EyeingTheLump".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.HateWord".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.LustWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PouringFromFourEyes".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PerfectViewOfEndowments".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.LookOfHate".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.HateFades".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.AndEachCockTwitch".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.TwitchesWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.AtTheThought".Localized());
            }
            else
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.LustDeepens".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.EachCockTwitchAgain".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.TwitchesWordAgain".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.InAnticipation".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.LeanOverAndGrab".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.ClimbingOverBody".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.OvipositorLurches".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.TubeProdsForward".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.EachSegmentLarger".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.WarmOrganProstate".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.DropWord".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.StreamWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PreCumDownLength".Localized());
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.VaginaQuivers".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Wetness() >= 3)
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.AndDrips".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.SympatheticClitPeeks".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.NotDeniedAnalPleasure".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PluralS".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.BetweenYou".Localized());

            if (Player.HasVagina() && Player.GetClitLength() >= 4)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PressingChickStick".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.SentencePeriod".Localized());

            Player.ButtChange(50, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.ParagraphBreak".Localized());

            ShowImage("cDrider-loss-butt");

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.GlanceBackAtWoman".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.AnalPregnancySlut".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.ExoskeletonDripping".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.FinalHardShove".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.CrushingProstate".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PeriodSpace".Localized());

            if (!Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.GaspDribbleGirlGoo".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.GaspSquirtCum".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.WrithingHappily".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.CocksDribbleGoo".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.VaginaDribblesSlime".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.FingersTweakNipples".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.OvipositorDilates".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.LargeEggPushes".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.AndYourProstate".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.TravelsDeepInside".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.MilkYourProstate".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PeriodBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.StreamOfSlime".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.Simultaneously".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.StormOfSemen".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.PluralSAgain".Localized());
                }
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.SiegeOfSpasms".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.AndWord".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.SquirtsOfSpooge".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.LiftedOffCarapace".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderRideOviAnal.FinishGettingDressed".Localized());

            Player.StartButtPregnancy(PregnancyIds.DriderEggs, PregnancyIds.DriderIncubation - 200, 10);

            Player.SlimeFeed();

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void VictoryVSDriderStickDickInSpinneret()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.QuicklyShedding".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.YourWord".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.OneOfYour".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.SubParCock".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.PluralS".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.SheTaunts".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.OutragedAtSlight".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.LittleTwist".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.AnySatisfaction".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.AnnoyedReachBack".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.GraspNippleChains".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.GraspNipples".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.TugRoughly".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.FemSpunkLeaking".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.MercifulWhim".Localized());

            if (Player.BiggestBreastSize() >= 3)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.SemiStickyStuff".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.SlipsUnconscious".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.AsGentlyAsCan".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.GigglingBallsSlap".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.PeriodBreak".Localized());

            ShowImage("cDrider-win-oral");

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.TongueLicks".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() < 50)
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.KeepsInRhythm".Localized());
            else
                if (Player.CumQ() < 350)
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.SputtersABit".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.HardPressedSwallow".Localized());
                    else
                        OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.ThroatBulges".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.YourOther".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.CocksGive".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.CockGives".Localized());

                OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.PictureInPicture".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.VictoryVSDriderStickDickInSpinneret.LustSated".Localized(
                 Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void LoseVSDriderGetAnalledOvi()
        {
            ClearOutput();

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.FallToGround".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.MakeItHarder".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.GruntTooExhausted".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.RelaxDiscomfort".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.CalmRacingThoughts".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.AsSheEnters".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.InchByInch".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.SheGasps".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript(0));
                else
                    OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.TaintWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.SlappingAgainstCunt".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.AfterHoursOfPlunging".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.VigorRenewed".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.DriderPants".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.NoticeOvipositor".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.GentleMovement".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.HereTheyCome".Localized());

            ShowImage("cDrider-loss-butt");

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.OneByOneEggs".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.AbusedWord".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.HungryWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.EggsSqueezingAlong".Localized(
                 Player.AssDescript()
            ));

            if (Player.Tallness < 66)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.InFrontOfYou".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.OnYourFace".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.PussyJuicesSquirt".Localized());

            if (Player.Tallness < 66)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.SlidingDownCheeks".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.ChestTrickling".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.PullsOutSlowly".Localized());

            Player.ButtChange(50, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.GiggleRemovesHerself".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.EllipsisBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderGetAnalledOvi.FewHoursLater".Localized());

            Player.StartButtPregnancy(PregnancyIds.DriderEggs, PregnancyIds.DriderIncubation - 200, 10);

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 800f), new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void LoseVSDriderAndVaginallyImpreggleOvi()
        {
            ClearOutput();

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.DriderIgnoresYou".Localized());

            if (Player.IsBiped() || Player.IsGoo())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.DownOntoBack".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.OverOntoSide".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.RegardlessOfSlenderness".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.PullFeetApart".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ScrapeAtScales".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ClimbingAtopYou".Localized());

            if (!Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.CoatingYour".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.RearWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.PinningProne".Localized());
            }
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.SprayWebbingTail".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.EerieSpeed".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.WristsWord".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ForelegsWord".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.PinnedSpreadEagled".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.MakesToBite".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.LacingWebbingHair".Localized());
            else
            {
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ReachBehindHead".Localized());

                if (Player.BiggestBreastSize() > 4)
                    OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.PainThenPop".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.TiesArmsBehind".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ChucklesAgain".Localized());

            if (Player.HasCock() && CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ChainsBrushOver".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.FangsTrailBelly".Localized());

            if (Player.BiggestBreastSize() > 3)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.VenomDribbles".Localized(
                     Appearance.BiggestBreastSizeDescript(Player)
                ));

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.DrawingEverCloser".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.BloodSwollenProtuberance".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.SlowDripNoPurpose".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.MoreYouDrink".Localized());

            ShowImage("cDrider-win-vag");

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.EventuallyEnough".Localized(
                 Player.VaginaDescript()
            ));

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.VileAppendageTwists".Localized());

            Player.CuntChange(50, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.BodyInTheWay".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.CockErupted".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.CockSpurtsPreCum".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.ShameInOrgasm".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOvi.GrindsPrickMouth".Localized());

            DoNext(() => LoseVSDriderAndVaginallyImpreggleOviEnding());
        }

        public void LoseVSDriderAndVaginallyImpreggleOviEnding()
        {
            ClearOutput();

            ShowImage("cDrider-loss-vag");

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.NotWaitingForAnswer".Localized(
                 Player.VaginaDescript()
            ));

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.PressesYouToCrotch".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.CuttingHeadLoose".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.ThroatStretched".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.AnotherEggShoves".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.BeforeYouProtest".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.DontMindThough".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.YouLikeIt".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.DeepGaspOfBreath".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.LandAgainstGround".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.StrainYourNeck".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.NearlySpitUp".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.CockFeelsThinner".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.CumExplodes".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.StepsOverYou".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndVaginallyImpreggleOviEnding.BindingsSlackening".Localized());

            Player.KnockUp(PregnancyIds.DriderEggs, PregnancyIds.DriderIncubation, 151);

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 100f), new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void LoseVSDriderAndGetFuckedANDAnalImpreggegity()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.UnableToResist".Localized());
            else
            {
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.SlumpDown".Localized());

                if (Player.LustPercent < 50)
                    OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.SoftWord".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.HardWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.MemberWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.PluralS".Localized());

                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.ToTheAir".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.StrengthBeyond".Localized());

            if (CombatController.Instance!.Context.Enemy.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.CrossedChainsDig".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.WarmthOfCock".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.LinesOfWebbing".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.HairedDominatrix".Localized());

            ShowImage("cDrider-sex-male");

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.LookAtHerFear".Localized(
                 Player.CockHead(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.SoEagerSperm".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.WaitEggs".Localized());

            Player.ButtChange(8, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.AnalViolation".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.StrangeOrganDeeper".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.DeepEnoughProstate".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.AnotherInch".Localized());

            Player.ButtChange(50, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.EggTravelsUp".Localized(
                 Player.SMultiCockDesc()
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.AndBelly".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.ConstantPressure".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.LoseTrackOfEggs".Localized());

            OutputText("Scenes.Corrupteddriderscene.LoseVSDriderAndGetFuckedANDAnalImpreggegity.GoodMotherSpeech".Localized());

            Player.SlimeFeed();

            Player.StartButtPregnancy(PregnancyIds.DriderEggs, PregnancyIds.DriderIncubation - 200, 10);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY()
        {

            ShowImage("birth-cDrider-butt");

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.GutRumbles".Localized());

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.OddlySoothingBirth".Localized());

            Player.ButtChange(40, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.ShiftPosition".Localized());

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.ClenchingRelaxing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.CockStaysLimp".Localized(
                     Player.SMultiCockDesc()
                ));

            if (Player.HasVagina())
            {
                OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.PussyClenchesAnd".Localized());

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.DripsWord".Localized());
                else
                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.DribblesWord".Localized());
                    else
                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.TinySquirt".Localized());
                        else
                            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.RiverOfLadySpunk".Localized());

                OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.IntoTheDirt".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.ComeDownFromHigh".Localized());

            OutputText("Scenes.Corrupteddriderscene.BirthSpiderEggsFromAnusITSBLEEDINGYAYYYYY.NewbornChildren".Localized());

            Player.Orgasm("Anal");
        }

        public void DriderPregVagBirth()
        {

            if (!Player.HasVagina())
            {
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.TerriblePressure".Localized());

                Player.CreateVagina();
            }

            ShowImage("birth-cDrider-vag");

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.ShudderViolently".Localized());

            Player.CuntChange(40, true, true, false);

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.ParagraphBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.AfterHoursOfEggs".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.NoMoreEggs".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.LookingBetweenLegs".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.PantingReverie".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.EggRollsAway".Localized());

            if (Player.BiggestBreastSize() >= 2)
            {
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.SuckleAtBreast".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.FollowingSisterLead".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.FollowingHerSisterLead".Localized());

                if (Player.LactationQ() >= 3000)
                    OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.ClumsyChild".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.ChildrenExplore".Localized());

            if (Player.Body.Get<EyesPart>().Id != BodyPartTypes.FourSpiderEyes || Player.Body.Get<EyesPart>().Id != BodyPartTypes.Spider)
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.FlopsForward".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.BabyDridersCrawlTail".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.BabyDridersSitLegs".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.ParagraphBreakThird".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.TwoGirlsStandOff".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.LieBackExhausted".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.AndRagingHardOn".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.AfterRemembering".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderPregVagBirth.SpiderWebMessage".Localized());

            Player.Orgasm("Vaginal");
        }

        public void ButtFuckADriderOhBaby()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            ShowImage("cDrider-win");

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.LeapIntoAction".Localized());

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.CalmedDown".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.DriderChides".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.PleasedToSee".Localized());

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.DriderCums".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.HasVagina())
            {
                OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.UnusedCuntSpurts".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.AndWord".Localized());
            }
            else
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.AsWord".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.YourSpareCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.CoatsWord".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.SCoatWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.HerHumanBack".Localized());
            }

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.PeriodBreak".Localized());

            OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.FinallyDone".Localized());

            if (silly)
                OutputText("Scenes.Corrupteddriderscene.ButtFuckADriderOhBaby.ButtSexIsAwesome".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void DriderVagSmartFuck()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            ShowImage("cDrider-win-collapsedSpidergirl");

            OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.KeepYourDistance".Localized(
                 y
            ));

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.PartedAndGlistening".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.StuffedWithFingers".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.NeedToBury".Localized());

            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.SpiderAbdomen)
            {
                OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.PlanForms".Localized());

                if (Player.IsDrider())
                    OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.ScuttleWord".Localized());
                else
                    OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.SaunterWord".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.OverToCollapsed".Localized());

                DriderTiedUpPartTwo(false);
            }
            else
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk)))
                {
                    OutputText("Scenes.Corrupteddriderscene.DriderVagSmartFuck.RememberingSilk".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), () => DriderTiedUpPartTwo(true)); AddButton(1, "No", () => DontSilkRapeDrider());
                }
        }

        public void DontSilkRapeDrider()
        {
            ClearOutput();

            OutputText("Scenes.Corrupteddriderscene.DontSilkRapeDrider.DecidingAgainstWaste".Localized());

            DoNext(() => DefeatDriderIntro());
        }

        public void DriderTiedUpPartTwo(bool useItem)
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            if (useItem)
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.ToughSilk));

                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.SmirkingAdvance".Localized(
                     y
                ));
            }

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.DartingForwards".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.AwayFromSnatch".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.StrugglesUseless".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.MovingBehindBack".Localized());

            if (!useItem)
                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.AwkwardlyPosition".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.ReachIntoInventory".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.ReleasingBoundForm".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.SeizingAdvantage".Localized());

            ShowImage("cDrider-win");

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.FirmBehindBumps".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.ThoughtIsEnticing".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.InhalesSharply".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.WhisperIntoEar".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.EnoughsEnough".Localized());

            if (!useItem)
                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.WithdrawHand".Localized());
            else
                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartTwo.GrabRemainderSilk".Localized());

            DoNext(() => DriderTiedUpPartThree());
        }

        public void DriderTiedUpPartThree()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.CompletelyOblivious".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.GruntPullMaterial".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.GagSecure".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.FreeOfArmor".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.SpindlyLimbsGrasp".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.NiceTry".Localized());

            ShowImage("cDrider-sex-tiedUp");

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.LeaningOverHer".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.TongueCircles".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.UnableToHoldBack".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.CalmYourself".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.HangThere".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.WaitingPerfectMoment".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.HandsKeepRoaming".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.HandRubbingButton".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.SheFinallyClimaxes".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.MoanAsSheWrithes".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.DrivenOnBySight".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.PantInTime".Localized());

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.YouFeelYourCock".Localized(
                 y
            ));

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartThree.FinallyFinished".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => DriderTiedUpPartFour());
        }

        public void DriderTiedUpPartFour()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            ShowImage("cDrider-sex-tiedUp-end");

            OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.GrabbingByShoulders".Localized());

            if (Player.Corruption >= 70)
            {
                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.ChuckleToYourself".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.RoughlyTearingSilk".Localized());

                OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.WithEveryThrustHer".Localized(
                     y
                ));
            }
            else
                if (Player.Corruption > 30)
                {
                    OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.CheckOnLover".Localized());

                    OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.RubHerStomach".Localized());
                }
                else
                {
                    OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.MakeSureComfortable".Localized());

                    OutputText("Scenes.Corrupteddriderscene.DriderTiedUpPartFour.NewlyFreedHands".Localized());
                }

            CombatScene.CleanupAfterCombat();
        }

    }
}