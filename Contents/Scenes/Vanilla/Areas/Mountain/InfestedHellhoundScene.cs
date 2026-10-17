using MonoCoC.Attributes;

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

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<InfestedHellhoundScene>("coc:infested_hellhound")]
    public class InfestedHellhoundScene : Scene
    {

        public override void Entry()
        {
            InfestedHellhoundEncounter();
        }

        public void InfestedHellhoundEncounter()
        {
            ClearOutput();

            ShowImage("monster-infestedhellhound");

            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundEncounter.LowSnarl".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundEncounter.JelloBodyJiggle".Localized());
            else
                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundEncounter.RattlesTeeth".Localized());

            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundEncounter.PeekingFearfully".Localized());

            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundEncounter.KnowsThreat".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryHellhounds);

            var hound = EnemyRegistry.Create(EnemyIds.InfestedHellhound);

            hound.OnDefeated = hpVictory =>
            {
                if (hpVictory)
                    CombatScene.CleanupAfterCombat();
                else
                    SceneInstance<HellHoundScene>.Get().HellHoundGetsRaped();
            };

            hound.OnWon = hpVictory => InfestedHellhoundLossRape();

            CombatScene.Start(hound, () => Go<CampScene>());
        }

        public void InfestedHellhoundLossRape()
        {
            ClearOutput();

            if (Player.Cocks.Count > 0 && Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                if (Player.Lust >= Player.MaxLust)
                {
                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.NoShame".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.Revulsion".Localized());
                    else
                        if (Player.Corruption < 66)
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BetterSense".Localized());
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.PerverseAnticipation".Localized());

                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.RollOntoBack".Localized());
                }
                else
                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.TooWounded".Localized(
                         Player.AssDescript()
                    ));

                ShowImage("hellhound-wormix");

                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BeastSniffs".Localized(
                     Player.CockHead()
                ));

                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.WormsOvercome".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                Player.CumMultiplier += .5f;

                CombatScene.CleanupAfterCombat();
            }
            else
                if (!Player.HasStatusEffect(StatusEffectIds.Infested) && Player.BiggestCockArea() >= 40 && Player.HasCock())
                {
                    if (Player.Lust >= Player.MaxLust)
                    {
                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.NoShameAgain".Localized());

                        if (Player.Corruption < 33)
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.RevulsionAgain".Localized());
                        else
                            if (Player.Corruption < 66)
                                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BetterSenseAgain".Localized());
                            else
                                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.PerverseAnticipationAgain".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.RollOntoBackAgain".Localized());
                    }
                    else
                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.TooWoundedAgain".Localized(
                             Player.AssDescript()
                        ));

                    ShowImage("hellhound-infestation");

                    if (Player.Cocks.Count > 1)
                    {
                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HoundBlocks".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.EachFeelsGood".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.EveryOneHardens".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.StopsLicking".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.ShovesInfected".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.YourWord".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.ShiversCum".Localized());
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BodyFull".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.StrangeFreakishness".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.DogBacksAway".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HoundBlocksAgain".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.YourFeelsBetter".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HardensStimulation".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.StopsLickingAgain".Localized(
                             Player.CockHead()
                        ));

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.ShovesInfectedAgain".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.YourWordAgain".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.ShiversCumAgain".Localized());
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BodyFullAgain".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.StrangeFreakishnessAgain".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.DogBacksAwayAgain".Localized());
                    }

                    Player.CreateStatusEffect(StatusEffectIds.Infested, 0, 0, 0, 0);

                    Player.Orgasm("Generic");

                    if (Player.HasPerk(PerkIds.ParasiteMusk))
                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.WormsDestroyed".Localized());

                    Player.RemovePerk(PerkIds.ParasiteMusk);

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugReproduction);

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                    Player.CumMultiplier += .2f;

                    if (CoC.Instance.Flags.Get(GameFlagIds.EverInfested) == 0)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.EverInfested, 1);

                        if (Player.Corruption < 25)
                            Player.Corruption = 25;
                    }

                    CombatScene.CleanupAfterCombat();
                }
                else
                    if (Player.HasVagina() && Player.BiggestCockArea() < 40 && !Player.HasTailInsteadOfLegs())
                    {
                        if (Player.Lust >= Player.MaxLust)
                        {
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.NoShameThird".Localized());

                            if (Player.Corruption < 33)
                                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.RevulsionThird".Localized());
                            else
                                if (Player.Corruption < 66)
                                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BetterSenseThird".Localized());
                                else
                                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.PerverseAnticipationThird".Localized());

                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.DropToAllFours".Localized());
                        }
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.TooWoundedThird".Localized(
                                 Player.AssDescript(),

                                 Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HoundRises".Localized());

                        if (Player.Corruption < 66)
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.Begs".Localized());
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.StartsBegging".Localized());

                        ShowImage("hellhound-loss-vag");

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.DogDoubles".Localized());

                        Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                        Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false);

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HeatRadiates".Localized(
                             Player.AssDescript()
                        ));

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.OhGodsNo".Localized(
                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.SoWrongSoHot".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.UnableToSupport".Localized());

                        if (Player.HasStatusEffect(StatusEffectIds.WormPlugged))
                            Player.ChangeStatusValue(StatusEffectIds.WormPlugged, 0, 1 + Random.Shared.Next(5));
                        else
                            Player.CreateStatusEffect(StatusEffectIds.WormPlugged, 1 + Random.Shared.Next(5), 0, 0, 0);

                        Player.KnockUp(PregnancyIds.WormStuffed, (int)(100 + Player.StatusEffectValue(StatusEffectIds.WormPlugged, 0)), 100, 1);

                        Player.Orgasm("VaginalAndAnal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 1f));

                        CombatScene.CleanupAfterCombat();
                    }
                    else
                    {
                        if (Player.Lust >= Player.MaxLust)
                        {
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.NoShameFourth".Localized());

                            if (Player.Corruption < 33)
                                OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.RevulsionFourth".Localized());
                            else
                                if (Player.Corruption < 66)
                                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BetterSenseFourth".Localized());
                                else
                                    OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.PerverseAnticipationFourth".Localized());

                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.DropToElbows".Localized());
                        }
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.TooWoundedFourth".Localized(
                                 Player.AssDescript()
                            ));

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HoundRisesAgain".Localized());

                        if (Player.Corruption < 80)
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.BegsAgain".Localized());
                        else
                            OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.StartsBeggingAgain".Localized());

                        ShowImage("hellhound-loss-butt");

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.DogPenetrates".Localized());

                        Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.HeatThick".Localized(
                             Player.AssDescript()
                        ));

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.OhGodsNoAgain".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.WrongButSoHot".Localized());

                        OutputText("Scenes.Infestedhellhoundscene.InfestedHellhoundLossRape.UnableToSupportAgain".Localized());

                        Player.Orgasm("Anal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 1f));

                        CombatScene.CleanupAfterCombat();
                    }
        }

    }
}