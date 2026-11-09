using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<SandWitchScene>("coc:sand_witch")]
    public class SandWitchScene : Scene
    {

        public override void Entry()
        {
            Encounter();
        }

        public void Encounter()
        {
            ClearOutput();

            if (!Player.IsChild())
                OutputText("Scenes.Sandwitchscene.Encounter.StrangeWomanAppears".Localized());
            else
                OutputText("Scenes.Sandwitchscene.Encounter.StrangeWomanAppearsAgain".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySandwitches);

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && Player.CockArea(0) > 100 && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0)
            {
                OutputText("Scenes.Sandwitchscene.Encounter.CoveringHitsLowerHalf".Localized());

                OutputText("Scenes.Sandwitchscene.Encounter.TheSandwitch".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Sandwitchscene.Encounter.AndBothTurnCrimson".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.Encounter.TurnsCrimson".Localized());

                OutputText("Scenes.Sandwitchscene.Encounter.YankYourBackInto".Localized());

                var enemy = EnemyRegistry.Create(EnemyIds.SandWitch);

                enemy.OnDefeated = hpVictory => BeatSandwitch();

                enemy.OnWon = hpVictory => SandwitchRape();

                CombatScene.Start(enemy, () => Go<CampScene>());
            }
            else
            {
                AddButton(0, "Common.Button.Yes".Localized(), () => AllowSandWitchMagic()); AddButton(1, "No", () => RefuseSandWitchMagic());
            }
        }

        public void AllowSandWitchMagic()
        {
            ClearOutput();

            if (Player.Body.Get<HairPart>().Color == "sandy blonde")
            {
                OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.SmilesWickedlyIntones".Localized());

                if (Player.BreastRows.Count == 0 || Player.BiggestBreastSize() == 0)
                {
                    OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.GrowRoundedPair".Localized());

                    if (Player.BreastRows.Count == 0)
                        Player.CreateBreastRow();

                    Player.BreastRows[0].Breasts = 2;

                    Player.BreastRows[0].BreastRating = 3;

                    if (Player.BreastRows[0].NipplesPerBreast < 1)
                        Player.BreastRows[0].NipplesPerBreast = 1;

                    Player.Orgasm("Tits", false);

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 1f));
                }

                if (Player.BiggestBreastSize() >= 1 && Player.BiggestBreastSize() <= 2)
                {
                    OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.BreastsBalloonOutwards".Localized());

                    Player.BreastRows[0].BreastRating = 3;

                    Player.Orgasm("Tits", false);

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 1f));
                }

                if (Player.BreastRows[0].NipplesPerBreast < 1)
                {
                    OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.TwoDarkSpotsAppear".Localized());

                    Player.BreastRows[0].NipplesPerBreast = 1;

                    Player.Orgasm("Tits", false);

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 1f));
                }

                if (Player.BiggestLactation() > 0)
                {
                    OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.StrongPressureBuilds".Localized());

                    if (Player.BiggestLactation() < 2)
                        OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.PowerfulJetsOfMilk".Localized());

                    if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() <= 2.6f)
                        OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.EruptionsOfMilkSquirt".Localized());

                    if (Player.BiggestLactation() > 2.6f && Player.BiggestLactation() < 3)
                        OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.ThickHosesOfMilk".Localized());

                    if (Player.BiggestLactation() >= 3)
                        OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.DropToYourKnees".Localized());
                    else
                    {
                        Player.BoostLactation(.75f);

                        OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.BreastsFeelFullerRiper".Localized());
                    }

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, 15f));

                    Player.Orgasm("Tits", false);
                }

                if (Player.BiggestLactation() == 0)
                {
                    OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.PleasurableReleaseErupts".Localized());

                    Player.BoostLactation(1);

                    Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 10f));

                    Player.Orgasm("Tits", false);
                }

                OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.SmilesAndThanks".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SandwitchServiced) == 0)
                    OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.WonderAtHerRobes".Localized());
                else
                    if (Player.Corruption <= 33)
                        OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.GladToAvoidServicing".Localized());
                    else
                        if (Player.Corruption <= 66)
                            OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.WonderIfYouShouldve".Localized());
                        else
                            OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.WishHadSaidNo".Localized());
            }
            else
            {
                OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.SmilesWickedlyIntonesAgain".Localized());

                OutputText("Scenes.Sandwitchscene.AllowSandWitchMagic.FeelATingling".Localized());

                Player.Body.Get<HairPart>().Color = "sandy blonde";
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void RefuseSandWitchMagic()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.RefuseSandWitchMagic.InarticulateScreamOfRage".Localized());

            var enemy = EnemyRegistry.Create(EnemyIds.SandWitch);

            enemy.OnDefeated = hpVictory => BeatSandwitch();

            enemy.OnWon = hpVictory => SandwitchRape();

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void SandwitchRape()
        {
            ClearOutput();

            Player.ClearStatuses();

            if (Player.Lust >= Player.MaxLust)
            {
                var titsOrgasm = false;

                if (Player.BiggestBreastSize() >= 9 && Player.BiggestLactation() >= 3 && !Player.IsPureEnough(25))
                {
                    OutputText("Scenes.Sandwitchscene.SandwitchRape.OverwhelmedByPleasure".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.WhimperDesperateToCum".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.JustBeforeYouCum".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.FamiliarPressureBuilds".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.BlackOutParagraph".Localized());

                    SandWitchBadEnd();

                    return ;
                }

                OutputText("Scenes.Sandwitchscene.SandwitchRape.WobblyLegsGiveOut".Localized());

                if (Player.Body.Get<HairPart>().Color.IndexOf("sandy blonde") != -1)
                {
                    OutputText("Scenes.Sandwitchscene.SandwitchRape.SandWitchSmilesWickedly".Localized());

                    if (Player.BreastRows.Count == 0 || Player.BiggestBreastSize() == 0)
                    {
                        OutputText("Scenes.Sandwitchscene.SandwitchRape.GrowRoundedPair".Localized());

                        if (Player.BreastRows.Count == 0)
                            Player.CreateBreastRow();

                        Player.BreastRows[0].Breasts = 2;

                        Player.BreastRows[0].BreastRating = 3;

                        if (Player.BreastRows[0].NipplesPerBreast < 1)
                            Player.BreastRows[0].NipplesPerBreast = 1;

                        Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 1f));
                    }

                    if (Player.BiggestBreastSize() >= 1 && Player.BiggestBreastSize() < 3)
                    {
                        OutputText("Scenes.Sandwitchscene.SandwitchRape.BreastsBalloonOutwards".Localized());

                        Player.BreastRows[0].BreastRating = 3;

                        Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 1f));
                    }

                    if (Player.AverageNipplesPerBreast() < 1)
                    {
                        OutputText("Scenes.Sandwitchscene.SandwitchRape.DarkSpotAppearsEach".Localized());

                        var temp = Player.BreastRows.Count;

                        while (temp > 0)
                        {
                            temp--;

                            if (Player.BreastRows[0].NipplesPerBreast < 1)
                                Player.NippleLength = .2f;

                            Player.BreastRows[0].NipplesPerBreast = 1;
                        }

                        Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 1f));
                    }

                    if (Player.BiggestLactation() > 0)
                    {
                        OutputText("Scenes.Sandwitchscene.SandwitchRape.StrongPressureBuilds".Localized());

                        if (Player.BiggestLactation() < 2)
                            OutputText("Scenes.Sandwitchscene.SandwitchRape.PowerfulJetsOfMilk".Localized());

                        if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() <= 2.6f)
                            OutputText("Scenes.Sandwitchscene.SandwitchRape.EruptionsOfMilkSquirt".Localized());

                        if (Player.BiggestLactation() > 2.6f && Player.BiggestLactation() < 3)
                            OutputText("Scenes.Sandwitchscene.SandwitchRape.ThickHosesOfMilk".Localized());

                        if (Player.BiggestLactation() >= 3)
                            OutputText("Scenes.Sandwitchscene.SandwitchRape.DropToYourKnees".Localized());
                        else
                        {
                            Player.BoostLactation(.7f);

                            OutputText("Scenes.Sandwitchscene.SandwitchRape.BreastsFeelFullerRiper".Localized());
                        }

                        Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, 15f));

                        titsOrgasm = true;
                    }

                    if (Player.BiggestLactation() == 0)
                    {
                        OutputText("Scenes.Sandwitchscene.SandwitchRape.PleasurableReleaseErupts".Localized());

                        Player.BoostLactation(1);

                        Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 10f));
                    }

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.WitchWhispersIntoEar".Localized());
                }
                else
                {
                    OutputText("Scenes.Sandwitchscene.SandwitchRape.SandSmilesWickedlyAgain".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.FeelATingling".Localized());

                    Player.Body.Get<HairPart>().Color = "sandy blonde";

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.WitchWhispersEarAgain".Localized());
                }

                OutputText("Scenes.Sandwitchscene.SandwitchRape.HearSoftImpactOf".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchRape.TurnsOverAndDrops".Localized());

                if (titsOrgasm)
                    Player.Orgasm("Tits");
                else
                    Player.Orgasm("Generic");

                Player.SlimeFeed();

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 5f));

                CoC.Instance.Flags.Set(GameFlagIds.SandwitchServiced, CoC.Instance.Flags.Get(GameFlagIds.SandwitchServiced) + 1);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                if (Player.BiggestBreastSize() >= 9 && Player.BiggestLactation() >= 3 && !Player.IsPureEnough(25))
                {
                    OutputText("Scenes.Sandwitchscene.SandwitchRape.StaggerFallToOne".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.VisionWaversWithExhaustion".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.LastThingYouHear".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRape.DreamOfWalkingProudly".Localized());

                    SandWitchBadEnd();

                    return ;
                }

                OutputText("Scenes.Sandwitchscene.SandwitchRape.FallDefeatedByThe".Localized());

                if (Player.IsChild())
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ChildLossesSandwitch, CoC.Instance.Flags.Get(GameFlagIds.ChildLossesSandwitch) + 1);

                    if (CoC.Instance.Flags.Get(GameFlagIds.ChildLossesSandwitch) >= 5)
                    {
                        ChildBadEnd();

                        return ;
                    }
                }

                CombatScene.CleanupAfterCombat();
            }
        }

        public void SandWitchBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandWitchBadEnd.AwakenInCandlelitStone".Localized());

            OutputText("Scenes.Sandwitchscene.SandWitchBadEnd.FamiliarVoiceSays".Localized());

            OutputText("Scenes.Sandwitchscene.SandWitchBadEnd.WitchLeansTowardYou".Localized());

            OutputText("Scenes.Sandwitchscene.SandWitchBadEnd.HardlyHaveTimeWonder".Localized());

            DoNext(() => SandWitchBadEndPartTwo());
        }

        public void SandWitchBadEndPartTwo()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartTwo.AudibleGurglingNoise".Localized());

            OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartTwo.ForAMomentNothing".Localized());

            DoNext(() => SandWitchBadEndPartThree());
        }

        public void SandWitchBadEndPartThree()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.SomethingBreaksWithin".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
            {
                OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.NoSoonerSpokenBefore".Localized());

                OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.TryingToFigureOut".Localized());

                OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.LastThingYouHear".Localized());
            }
            else
            {
                OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.NoSoonerSpokenThan".Localized());

                OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.DuskyWitchCatches".Localized());

                OutputText("Scenes.Sandwitchscene.SandWitchBadEndPartThree.AsYouHate".Localized());
            }

            GameOver();
        }

        public void SandwitchRaped()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                DoNext(() => ReturnToCamp(1));

                SandwitchCentaurBoning();

                CombatScene.CleanupAfterCombat();

                return ;
            }

            OutputText("Scenes.Sandwitchscene.SandwitchRaped.RipHerSand".Localized());

            if (Player.Gender == Gender.Male || Player.Gender == Gender.Hermaphrodite)
            {
                if (Player.IsPureEnough(50))
                {
                    OutputText("Scenes.Sandwitchscene.SandwitchRaped.SmileAtSandWitch".Localized());

                    OutputText("Scenes.Sandwitchscene.SandwitchRaped.AttentionWandersDown".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Breasts".Localized(), SandwitchBewbs);

                    AddButton(1, "Common.Button.Sex".Localized(), (Player.IsChild() && Player.BiggestCockLength() <= 7 ? SandwitchSexChild : SandwitchSex));
                }
                else
                {
                    if (Player.CockTotal() == 1)
                        if (Player.HasKnot())
                            KnotSandwitch();
                        else
                        {
                            ShowImage("sandwitch-male-win-hardsex");

                            OutputText("Scenes.Sandwitchscene.SandwitchRaped.PlaceInTheWet".Localized(
                                 NumberUtils.Num2Text(Player.Cocks.Count + 1)
                            ));

                            OutputText("Scenes.Sandwitchscene.SandwitchRaped.ThrustingDeepHitBottom".Localized(
                                 Player.CockDescript(0)
                            ));

                            OutputText("Scenes.Sandwitchscene.SandwitchRaped.SatisfiedGrowlOfLust".Localized());

                            OutputText("Scenes.Sandwitchscene.SandwitchRaped.BestialCryOfLust".Localized());

                            OutputText("Scenes.Sandwitchscene.SandwitchRaped.ScreamsInSavage".Localized());

                            OutputText("Scenes.Sandwitchscene.SandwitchRaped.FinallyFinishedPullOut".Localized(
                                 Player.CockDescript(0)
                            ));

                            Player.Orgasm("Dick");
                        }

                    if (Player.CockTotal() > 1)
                    {
                        Player.Orgasm("Dick");

                        if (Random.Shared.Next(2) == 0)
                        {
                            RapeSandwitchMultis();

                            return ;
                        }

                        OutputText("Scenes.Sandwitchscene.SandwitchRaped.UsingTheCorruption".Localized(
                             NumberUtils.Num2Text(Player.Cocks.Count + 1)
                        ));

                        OutputText("Scenes.Sandwitchscene.SandwitchRaped.ThrustingDeepAgain".Localized(
                             Player.MultiCockDescriptLight()
                        ));

                        OutputText("Scenes.Sandwitchscene.SandwitchRaped.SatisfiedGrowlAgain".Localized());

                        OutputText("Scenes.Sandwitchscene.SandwitchRaped.BestialCryAgain".Localized());

                        OutputText("Scenes.Sandwitchscene.SandwitchRaped.ScreamsInSavageAgain".Localized());

                        OutputText("Scenes.Sandwitchscene.SandwitchRaped.FinallyFinishedAgain".Localized(
                             Player.MultiCockDescriptLight()
                        ));
                    }

                    CombatScene.CleanupAfterCombat();
                }
            }
            else
            {
                OutputText("Scenes.Sandwitchscene.SandwitchRaped.SmileSandWitchAgain".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchRaped.AttentionWandersAgain".Localized());

                SandwitchBewbs();
            }
        }

        public void SandwitchBewbs()
        {
            ClearOutput();

            ShowImage("sandwitch-male-win-kiss-suckherbreast");

            OutputText("Scenes.Sandwitchscene.SandwitchBewbs.MoveAtopDefeated".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBewbs.WarmthFillsBelly".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBewbs.BurpAndFlushScarlet".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBewbs.BreaksKissAndSmiles".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBewbs.WakeUpInDesert".Localized());

            Player.SlimeFeed();

            Player.RefillHunger(35);

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            CombatScene.CleanupAfterCombat();

            OutputText("Scenes.Sandwitchscene.SandwitchBewbs.ParagraphBreak".Localized());
        }

        public void SandwitchSex()
        {
            ClearOutput();

            ShowImage("sandwitch-male-win-vagsex");

            OutputText("Scenes.Sandwitchscene.SandwitchSex.CharmingDisarmingSmile".Localized());

            if (Player.CockTotal() > 1)
                OutputText(NumberUtils.Num2Text(Player.CockTotal()));
            else
                OutputText("Scenes.Sandwitchscene.SandwitchSex.TwoWord".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSex.OfSandWitchAppears".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSex.CriesOutHer".Localized(
                 Player.CockDescript(0)
            ));

            OutputText("Scenes.Sandwitchscene.SandwitchSex.RestAgainstHerChest".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSex.KissTheSandWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSex.FinallyGetSerious".Localized());

            if (Player.Balls >= 2)
                OutputText("Scenes.Sandwitchscene.SandwitchSex.BallsAcheAndYour".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSex.ThrobsWithNeed".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSex.SatisfiedSmirkYouLay".Localized(
                 Player.CockDescript(0)
            ));

            OutputText("Scenes.Sandwitchscene.SandwitchSex.WithStartYouWake".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void SandwitchSexChild()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.SandWitchAppearsFrightened".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.CriesOutHer".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.RestAgainstHerChest".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.KissTheSandWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.GrowingAccustomedTo".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.SmirkSheFinallyUnhooks".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.SaysTrailingOff".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.WithStartYouWake".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.RoundTwoDarling".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.OhNoSurelyDoesnt".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSexChild.AlasOlderWitchFar".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void SandwitchCentaurBoning()
        {

            Player.Orgasm("Generic");

            OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.SandWitchPantingBefore".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.AndTornManyPlaces".Localized());
            else
                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.FingersSqueezingNipples".Localized());

            if (Player.BiggestBreastSize() > 3)
            {
                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.ApproachHerEyesDrift".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.UnableToStopHerself".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.SensuallyLicksLips".Localized());
            }

            if (Player.BiggestLactation() > 1)
                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.EyesWidenSheSees".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.ParagraphBreak".Localized());

            if (Player.Corruption < 70)
            {
                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.SmileAsYouApproach".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.YourNipplesBecomeStiff".Localized());

                if (Player.Cocks.Count > 0 && !Player.HasVagina())
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.AndYourCockHardens".Localized());

                if (Player.HasVagina() && Player.Cocks.Count == 0)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.AndBeginsToDrip".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.HasVagina() && Player.Cocks.Count > 0)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.AndYourHardensAs".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.HerSuffix".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.SeemsHesitantAtFirst".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.EyesFixedOnYour".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.PeriodParagraphBreak".Localized());

                if (SWCentaurMore(3))
                    return ;

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.DelicateHandsReachAround".Localized());

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.FondleYourAnd".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.TeaseYourAndFeel".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.WithStartYouRealize".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.LoseTrackOfTime".Localized());

                if ((Player.BiggestBreastSize() >= 1) && (Player.BiggestLactation() >= 1))
                    {
                        OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.YourOwnAreLeaking".Localized());

                        if (Player.BiggestLactation() < 3)
                            OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.SmallStreamsRunningOut".Localized());
                        else
                            OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.TorrentsGushingOut".Localized());

                        OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.UnderHerExpertHands".Localized());
                    }

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.SomehowFeelHerArousal".Localized());

                if (SWCentaurMore(1))
                    return ;
            }
            else
            {
                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.WhenSeesTheLook".Localized());

                if (Player.Strength >= 50)
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.EasilyTearThemOff".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.DragBitUntil".Localized());

                OutputText("Scenes.Sandwitchscene.SandwitchCentaurBoning.UnderneathRobesBeautiful".Localized());

                if (SWCentaurMore(2))
                    return ;
            }
        }

        public bool SWCentaurMore(float argument)
        {

            if (argument == 1)
            {
                OutputText("Scenes.Sandwitchscene.SWCentaurMore.RealizeTooLate".Localized());

                Player.Orgasm("Generic");

                if (Player.BiggestBreastSize() == 0)
                {
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.GrowRoundedPair".Localized());

                    if (Player.BreastRows.Count == 0)
                        Player.CreateBreastRow();

                    Player.BreastRows[0].Breasts = 2;

                    Player.BreastRows[0].BreastRating = 3;

                    if (Player.BreastRows[0].NipplesPerBreast < 1)
                        Player.BreastRows[0].NipplesPerBreast = 1;

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 1f));

                    return false;
                }

                if (Player.BiggestBreastSize() < 3)
                {
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.BreastsBalloonOutwards".Localized());

                    Player.BreastRows[0].BreastRating = 3;

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 1f));

                    return false;
                }

                if (Player.BiggestLactation() == 0)
                {
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.BreastsNowLactate".Localized());

                    Player.BoostLactation(1);

                    Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 10f));

                    return false;
                }

                if (Player.BiggestLactation() < 3)
                {
                    Player.BoostLactation(.7f);

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.BreastsFeelFullerMilkier".Localized());

                    return false;
                }

                Player.ApplyStats(new DynStat(GameStats.Libido, 4f));

                OutputText("Scenes.Sandwitchscene.SWCentaurMore.LibidoBoostedSignificantly".Localized());

                return false;
            }

            if (argument == 2)
            {
                if (Player.CockTotal() > 0 || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                {
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.FinishedGamesPreCum".Localized());

                    if (Player.Cocks[0].Thickness >= 3)
                        if (Player.HasKnot(0))
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.SeeingItsSheerSize".Localized());
                        else
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.SheerSizeCausesHer".Localized());

                    ShowImage("sandwitch-male-taur-win-hardsex");

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.GrabWitchAndKnock".Localized());

                    if (Player.CockTotal() == 1 && Player.CountCocksOfType("TENTACLE") == 0)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.SingleThrustYouPush".Localized());

                    if (Player.CockTotal() == 1 && Player.CountCocksOfType("TENTACLE") == 1)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.CaressesHerAnusTeasingly".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.AfterAligningToTwin".Localized());

                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.LiningUpTwoCocks".Localized());

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.ElicitingAScream".Localized());

                    if (Player.Cocks[0].Thickness >= 3)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.HardToBelieveJust".Localized());

                    if (Player.CockTotal() >= 2)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.FeelingOfTouchingEach".Localized());

                    if (Player.Cocks[0].Length >= 12)
                    {
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.BodySurrendersMore".Localized());
                        else
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.BodySurrendersAgain".Localized());

                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.MouthOpensSoundless".Localized());
                    }

                    if (Player.CountCocksOfType("TENTACLE") == 1 && Player.CockTotal() == 1)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.BodySurrendersThird".Localized());

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.FeelsHaveBeen".Localized());

                    if (Player.Cocks[0].Length >= 12 || Player.Cocks[0].Thickness >= 3)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.CriesLongSinceDied".Localized());
                    else
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.AndCriesLongSince".Localized());

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.CannotHoldOffOrgasm".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.SLetter".Localized());

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.CommaSpace".Localized());

                    if (Player.CumQ() >= 250)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.GushingMassiveAmounts".Localized());
                    else
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.PushingYourSperm".Localized());

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.DeepIntoHer".Localized());

                    if (Player.CockTotal() == 1 && Player.CountCocksOfType("TENTACLE") == 1)
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.AnusWord".Localized());
                    else
                        if (Player.Cocks[0].Length >= 12 || Player.Cocks[0].Thickness >= 3)
                        {
                            if (Player.CockTotal() == 1)
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.CuntWord".Localized());

                            if (Player.CockTotal() >= 2)
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.CuntsWord".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.WombWord".Localized());

                            if (Player.CockTotal() >= 3)
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.AndBowels".Localized());
                        }

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.SatisfiedGroanYouPull".Localized());
                }
                else
                {
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.FinishedGamesPreCum".Localized());

                    ShowImage("sandwitch-taur-win-get-cunning-fingered");

                    if (Player.HasVagina())
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.AndDrippingWithDesire".Localized(
                             Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.PushWitchUnceremoniously".Localized());

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.FingersHerselfHarder".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.VaginalCapacity() < 10)
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.OneOfHerFingers".Localized());
                        else
                            if (Player.VaginalCapacity() < 20)
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.SeveralOfHerFingers".Localized());
                            else
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.AFistAgain".Localized());

                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.PushingDeepIntoYour".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.AnalCapacity() < 10)
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.HerTongue".Localized());
                        else
                            if (Player.AnalCapacity() < 20)
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.TwoOfHerFingers".Localized());
                            else
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.AFistAgain".Localized());

                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.PenetratesYour".Localized());

                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.SentencePeriod".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.FingersHerselfHarderAgain".Localized());

                        if (Player.AnalCapacity() < 10)
                            OutputText("Scenes.Sandwitchscene.SWCentaurMore.SensationsAreIntense".Localized());
                        else
                            if (Player.AnalCapacity() < 20)
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.UnsatisfiedWithBuffetYou".Localized());
                            else
                                OutputText("Scenes.Sandwitchscene.SWCentaurMore.UnsatisfiedWithBuffetThe".Localized());

                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.MumblingFinishes".Localized());

                        OutputText("Scenes.Sandwitchscene.SWCentaurMore.ParagraphBreak".Localized());

                        SWCentaurMore(1);

                        DoNext(() => ReturnToCamp(2));

                        return true;
                    }
                }

                return false;
            }

            if (argument == 3)
            {
                OutputText("Scenes.Sandwitchscene.SWCentaurMore.AsBeginsSeemLess".Localized());

                if (Player.Strength > 50)
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.PickUpAndToss".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.SWCentaurMore.OfferHerYourHand".Localized());

                OutputText("Scenes.Sandwitchscene.SWCentaurMore.StrongHoovesCarryYou".Localized());
            }

            return false;
        }

        public void KnotSandwitch()
        {
            ClearOutput();

            ShowImage("sandwitch-male-win-dogcock-vagsex");

            OutputText("Scenes.Sandwitchscene.KnotSandwitch.PressTheTip".Localized());

            OutputText("Scenes.Sandwitchscene.KnotSandwitch.AsYourStirsWithin".Localized());

            OutputText("Scenes.Sandwitchscene.KnotSandwitch.BeginToBuck".Localized());

            OutputText("Scenes.Sandwitchscene.KnotSandwitch.ThenBeginsToGet".Localized());

            OutputText("Scenes.Sandwitchscene.KnotSandwitch.GruntAndPushHard".Localized());

            if (Player.CumQ() < 25)
                OutputText("Scenes.Sandwitchscene.KnotSandwitch.CumSlicksYourDick".Localized());

            if (Player.CumQ() >= 25 && Player.CumQ() < 100)
                OutputText("Scenes.Sandwitchscene.KnotSandwitch.CumExpandsHerPussy".Localized());

            if (Player.CumQ() >= 100 && Player.CumQ() < 500)
                OutputText("Scenes.Sandwitchscene.KnotSandwitch.CumBeginsToPump".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Sandwitchscene.KnotSandwitch.CumSpurtsIntoHer".Localized());

            Player.Orgasm("Dick");
        }

        public void RapeSandwitchMultis()
        {
            ClearOutput();

            ShowImage("sandwitch-male-win-multidick-vagsex");

            OutputText("Scenes.Sandwitchscene.RapeSandwitchMultis.CorruptionOfThisRealm".Localized());

            OutputText("Scenes.Sandwitchscene.RapeSandwitchMultis.GrinAtTheHumiliated".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void BeatSandwitch()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Sandwitchscene.BeatSandwitch.SmileInSatisfaction".Localized());
            else
                OutputText("Scenes.Sandwitchscene.BeatSandwitch.SmileInSatisfactionAgain".Localized());

            if (Player.IsChild())
                CoC.Instance.Flags.Set(GameFlagIds.ChildLossesSandwitch, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Rape".Localized(), null);

            AddButton(1, "Common.Button.DildoRape".Localized(), null);

            AddButton(2, "Common.Button.Taunt".Localized(), null);

            AddButton(3, "Common.Button.LayEggs".Localized(), null);

            AddButton(4, "Common.Button.UseShouldra".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Sandwitchscene.BeatSandwitch.SadlyRealizeYourNeeds".Localized());

                OutputText("Scenes.Sandwitchscene.BeatSandwitch.JustTauntingTeasingAnd".Localized());

                AddButton(0, "Common.Button.Rape".Localized(), SandwitchRaped);

                if (Player.HasKeyItem("Deluxe Dildo"))
                    AddButton(1, "Common.Button.DildoRape".Localized(), SandwitchGetsDildoed);

                AddButton(2, "Common.Button.TauntHer".Localized(), ((Player.IsChild() && Player.Tallness < 55) ? SandwitchTauntChild : SandwitchSpanking));

                if (!Player.IsGenderless() && Player.CanOviposit())
                    AddButton(3, "Common.Button.LayEggs".Localized(), OvipositSandWitches);
                // 原版 SandWitchScene.hx:846：Shouldra 是跟随者且 PC 非无性别时可用幽灵助手
                if (SceneInstance<Npcs.ShouldraFollowerScene>.Get().FollowerShouldra()
                    && !Player.IsGenderless())

                    AddButton(4, "Common.Button.UseShouldra".Localized(), () => SceneInstance<Npcs.ShouldraFollowerScene>.Get().SandWitchGetsGhostly());

                if (silly)
                    AddButton(5, "Common.Button.UseI".Localized(), MissingoSex);

                AddButton(6, "Common.Button.Titfuck".Localized(), SandwitchTitFuck);
            }

            if (Player.AbilityAvailable(new[] { "Arouse" }, ignoreLust: true, ignoreFatigue: true))
                AddNextButton("Combat.Ability.NamelessHorrorOverwhelm.Name".Localized(), SandwitchOverwhelm);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => CombatScene.CleanupAfterCombat());
        }

        public void MissingoSex()
        {
            ClearOutput();

            ShowImage("sandwitch-silly-missingosex");

            OutputText("Scenes.Sandwitchscene.MissingoSex.StrideProudlyTowards".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex.ThisBitchNeedsDick".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex.WantToEmbarrassHer".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex.GarbledItsTime".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex.BeginsToPanic".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex.DontReportThisAs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.NXt".Localized(), MissingoSex2);
        }

        public void MissingoSex2()
        {
            ClearOutput();

            ShowImage("sandwitch-silly-missingosex");

            OutputText("Scenes.Sandwitchscene.MissingoSex2.NaturallyYouHonestly".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex2.CantBelieveYou".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex2.StillSheSaysOk".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.NXt".Localized(), MissingoSex3);
        }

        public void MissingoSex3()
        {
            ClearOutput();

            ShowImage("sandwitch-silly-missingosex");

            OutputText("Scenes.Sandwitchscene.MissingoSex3.AtReallyYouI".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex3.AToBelieveYou".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex3.ISheSays".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Xt".Localized(), MissingoSex4);
        }

        public void MissingoSex4()
        {
            ClearOutput();

            ShowImage("sandwitch-silly-missingosex");

            OutputText("Scenes.Sandwitchscene.MissingoSex4.MoanOne".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex4.MoanTwo".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex4.MoanThree".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckThis".Localized(), MissingoSex5);
        }

        public void MissingoSex5()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.MissingoSex5.YeahNoThisShit".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex5.SighDefeatedSigh".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex5.FlipUpThePlastic".Localized());

            ShowImage("coc-title");

            OutputText("Scenes.Sandwitchscene.MissingoSex5.SystemPowersUp".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex5.OhGoddammitRemember".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Restore".Localized(), MissingoSex6);
        }

        public void MissingoSex6()
        {
            ClearOutput();

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();

            OutputText("Scenes.Sandwitchscene.MissingoSex6.SuccessfullyRestoreGame".Localized());

            OutputText("Scenes.Sandwitchscene.MissingoSex6.LikeFuckingChristmas".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SandwitchTitFuck()
        {
            ClearOutput();

            RegisterMacro("hasPrehensileTail", Player.HasPrehensileTail());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.PushingWitchOntoSide".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.HissesDiscontent".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.FollowingYourInstincts".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.IsntForYouShe".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.SimpleFashionSpill".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.SitYourHaunches".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.MoveInCloseGently".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.CatchesHerVoiceIn".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.AtFirstYouLay".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.HipsStartToRock".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTitFuck.BouncingTitflesh".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void SandwitchGetsDildoed()
        {
            ClearOutput();

            ShowImage("sandwitch-u-win-dildoher");

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.PullOutTamanisDildo".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.PushingHerLegsApart".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.CriesDefeatedWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.AfterGivingHerChance".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.FeelTheToySlowly".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.ReleasingOversizedTits".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.WiggleTheToyAbout".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.LickItClean".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchGetsDildoed.HeadBackTowardsCamp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((20 + Player.Libido / 5 + Player.Corruption / 10))));

            CombatScene.CleanupAfterCombat();
        }

        public void OvipositSandWitches()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.GlanceDownAtThe".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.BruisedAndBattered".Localized());
            else
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.LustyWord".Localized());

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.FeelTheFamiliar".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.FrightAsSheClamors".Localized());
            else
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.UneasyCuriosityToSee".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.RollingEyesOfferA".Localized());

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.SlowsToA".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.HardenedWord".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.AndWord".Localized());

            if (Player.HasVagina())
                OutputText(Player.ClitDescript());

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.ForHerViewing".Localized());

            if (Player.Cocks.Count > 1)
                OutputText(NumberUtils.Num2Text(Player.CockTotal() + 1));
            else
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.TwoWord".Localized());

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.SexedFormAllowingSun".Localized());

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.DrawingCloserToProne".Localized());

            if (Player.CanOvipositBee())
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.BeeMorphs".Localized());
            else
                OutputText("Scenes.Sandwitchscene.OvipositSandWitches.Driders".Localized());

            OutputText("Scenes.Sandwitchscene.OvipositSandWitches.CanDoToPartners".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ForeplaySt".Localized(), EggwitchForeplay);

            AddButton(1, "Common.Button.GetFucking".Localized(), GetToFuckingWithZeEggsInWitch);
        }

        public void EggwitchForeplay()
        {
            ClearOutput();

            ShowImage("sandwitch-bee-drider-give-foreplay");

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.KnowingHowWitchFeels".Localized());

            if (Player.Cocks.Count > 1)
                OutputText(NumberUtils.Num2Text(Player.CockTotal() + 1));
            else
                OutputText("Scenes.Sandwitchscene.EggwitchForeplay.TwoWord".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.ClitsAllHard".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.GrinningInDelight".Localized());

            if (Player.CockTotal() > 1 || Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Sandwitchscene.EggwitchForeplay.EsLetter".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.DescendOnLoveButtons".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sandwitchscene.EggwitchForeplay.SLetter".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.InStepYourTongue".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.WarmedThisSorceressUp".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Sandwitchscene.EggwitchForeplay.EnoughWord".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.GivingWayOfBreeding".Localized());

            if (Player.CanOvipositBee())
                OutputText("Scenes.Sandwitchscene.EggwitchForeplay.SittingWord".Localized());
            else
                OutputText("Scenes.Sandwitchscene.EggwitchForeplay.LayingWord".Localized());

            OutputText("Scenes.Sandwitchscene.EggwitchForeplay.AgainstYourMate".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, -.35f));

            DoNext(() => LaySomeEggsInThatWitchFinally());
        }

        public void GetToFuckingWithZeEggsInWitch()
        {
            ClearOutput();

            ShowImage("sandwitch-spread-her-leg");

            if (Player.Corruption < 66)
            {
                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.UnderstandHerReservation".Localized());

                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.ReluctantlySpreadsLegs".Localized());
            }
            else
            {
                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.SightSandWitch".Localized());

                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.LaboredBreathingBecomes".Localized());

                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.WhatWouldThisServe".Localized());

                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.TriesToProtest".Localized());

                OutputText("Scenes.Sandwitchscene.GetToFuckingWithZeEggsInWitch.PleaseTakeMe".Localized());
            }

            DoNext(() => LaySomeEggsInThatWitchFinally());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
        }

        public void LaySomeEggsInThatWitchFinally()
        {
            ClearOutput();

            var cunt = (Random.Shared.Next(2) == 0);

            if (Player.CanOvipositBee())
                ShowImage("sandwitch-u-lay-beeegg");
            else
                ShowImage("sandwitch-u-lay-drideregg");

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.BeforeLongIntenseNeed".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.FoldsOfHerVulva".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.RingOfHerAnus".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.LikeOutAFever".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.SexWord".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AssWord".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.TheCommaWord".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.RibbedContoursOfPussy".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.TightFitOfInnards".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.EnrapturingEggTube".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.LoveHyphen".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AnalWord".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.CanalForDelivery".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.DescentIntoHerBeing".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.OpeningOfLoversCervix".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.DeepestRecessesOfSphincter".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.JoiningOfOviductTo".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.WombWord".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AnusWord".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.NowCompletePrepare".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AtTheWaitingLips".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.ThroughHerRectalWalls".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.MeticulousPushForerunner".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.SorceressCriesOut".Localized());

            if (cunt)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.IntoHerWaitingWomb".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AsFarCanSafely".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.NestledAndSound".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.WhimpersCaressingYour".Localized());

            if (Player.CanOvipositBee() && !Player.IsTaur())
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.TorsoWord".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.PelvisWord".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.ToSpurYouOn".Localized());

            if (Player.CockTotal() > 1 || (int)Player.Gender > 2)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.EsLetter".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.OnlyPointOfDissatisfaction".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.TakeHoldAndBring".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.BringYour".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.ToFaceCuteStare".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.ForFeelsLikeEternity".Localized());

            if (Player.CockTotal() > 1 || Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.EsLetterAgain".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.SlowlyPumpHerFull".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AndWord".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.YourVagina".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.AtLongLastEfforts".Localized());

            var facial = Random.Shared.Next(2);

            if (facial == 0)
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.FacialFeaturesWithProof".Localized());
            else
                if (facial == 1)
                    OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.InteriorOfHerMouth".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.FinallyAchievedRelease".Localized());

            if (Player.CanOvipositBee())
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.HoneyLike".Localized());
            else
                OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.GoopyWord".Localized());

            OutputText("Scenes.Sandwitchscene.LaySomeEggsInThatWitchFinally.LubricantDroolingOut".Localized());

            if (Player.FertilizedEggs() > 0)
                if (Player.CanOvipositBee())
                {
                    Player.Pregnancy.Type = Pregnancy.PregnancyIds.BeeEggs;

                    Player.Pregnancy.Incubation = 192;
                }
                else
                {
                    Player.Pregnancy.Type = Pregnancy.PregnancyIds.DriderEggs;

                    Player.Pregnancy.Incubation = 192;
                }

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void SammitchBirthsDriders()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.StruggleWithTheLoose".Localized());

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.UntilAnotherHowlReaches".Localized());

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.TryingBestToLocate".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.IntendingToHelp".Localized());
            else
                OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.CuriousWord".Localized());

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.SlideDownTheDune".Localized());

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.WearySorceressAdmits".Localized());

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.JokesWeaklyBeforeLurching".Localized());

            OutputText("Scenes.Sandwitchscene.SammitchBirthsDriders.CanGuessThatWithout".Localized());

            if (Player.IsPureEnough(60))
                HelpZeWithBirfBabies();
            else
                ReluctantlyHelpZeWitch();
        }

        public void HelpZeWithBirfBabies()
        {

            OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.FilledWithPride".Localized());

            if (!Player.IsDrider())
                OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.BlurtsNoticingThe".Localized());

            if (Player.IsTaur() || Player.IsDrider())
            {
                OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.CriesOutDespairing".Localized());

                OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.CallsOutFromBehind".Localized());

                if (Player.IsDrider())
                    OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.SkitterWord".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.GallopWord".Localized());

                OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.AcrossDryDesertSea".Localized());
            }
            else
                OutputText("Scenes.Sandwitchscene.HelpZeWithBirfBabies.EasingHandUnderArmpit".Localized());

            DoNext(() => SandwitchBirthsYourMonstrosities());
        }

        public void ReluctantlyHelpZeWitch()
        {

            OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.GreatTheThings".Localized());

            if (!Player.IsDrider())
                OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.BlurtsNoticingThe".Localized());

            if (Player.IsDrider() || Player.IsTaur())
            {
                OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.CriesOutDespairing".Localized());

                OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.CallsOutClearlyTrying".Localized());

                if (Player.IsDrider())
                    OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.SkitterWord".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.GallopWord".Localized());

                OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.AcrossDryDesertSea".Localized());
            }
            else
                OutputText("Scenes.Sandwitchscene.ReluctantlyHelpZeWitch.EasingHandUnderArmpit".Localized());

            DoNext(() => SandwitchBirthsYourMonstrosities());
        }

        public void SandwitchBirthsYourMonstrosities()
        {
            ClearOutput();

            ShowImage("sandwitch-give-birth-drider");

            OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.AfterSeemsLikeForever".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.RelievedToHaveCarried".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.TurnToTheSpellcaster".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.ArentRulingAnythingOut".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.WithAQuickKiss".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.WhisperInHerEar".Localized());

            if (Player.Corruption >= 60)
                OutputText("Scenes.Sandwitchscene.SandwitchBirthsYourMonstrosities.UseYourCommonSense".Localized());

            Player.Pregnancy.Type = default;

            Player.Pregnancy.Incubation = 0;

            DoNext(() => ReturnToCamp(1));
        }

        public void WitchBirfsSomeBees()
        {
            ClearOutput();

            ShowImage("sandwitch-give-birth-bee");

            OutputText("Scenes.Sandwitchscene.WitchBirfsSomeBees.TrudgeThroughTheWindy".Localized());

            OutputText("Scenes.Sandwitchscene.WitchBirfsSomeBees.RushingTowardsCrescendo".Localized());

            if (silly)
                OutputText("Scenes.Sandwitchscene.WitchBirfsSomeBees.EchoingTinyLittleCries".Localized());

            OutputText("Scenes.Sandwitchscene.WitchBirfsSomeBees.LikelyJustWaitingFor".Localized(
                 Player.Mf("handsome", "beautiful")
            ));

            OutputText("Scenes.Sandwitchscene.WitchBirfsSomeBees.ContentThingsTurned".Localized());

            Player.Pregnancy.Type = default;

            Player.Pregnancy.Incubation = 0;

            DoNext(() => ReturnToCamp(1));
        }

        public void SandwitchTauntChild()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.FeelingEuphoricAfterVictory".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.GrabTheHelplessWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.WitchProtestsOnce".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.MilkHerEffortlessly".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.ActuallyKindOfFun".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.TooLateSeeWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.SeeminglyRecoveredTowering".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.SnapsWithMagicCrackling".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.SmirkingSorceressLaughs".Localized((silly ? "[say: I'll beat your ass so hard you can't sit down for a week, [boy]!]" : "")));

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.ThisSeemsLastForever".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.HavingWorkedHerFrustrations".Localized((silly ? "But didn't anyone ever tell her not to cry over spilt milk?" : "")));

            OutputText("Scenes.Sandwitchscene.SandwitchTauntChild.PickingUpHerRobe".Localized());

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void ChildBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.ChildBadEnd.StaggerFallToOne".Localized());

            OutputText("Scenes.Sandwitchscene.ChildBadEnd.VisionWaversWithExhaustion".Localized());

            OutputText("Scenes.Sandwitchscene.ChildBadEnd.LastThingYouHear".Localized());

            OutputText("Scenes.Sandwitchscene.ChildBadEnd.WhenYouComeTo".Localized());

            DoNext(() => ChildBadEnd2());
        }

        public void ChildBadEnd2()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.ChildBadEnd2.SkinDeepBrownHair".Localized());

            OutputText("Scenes.Sandwitchscene.ChildBadEnd2.GazeOutIntoWastes".Localized());

            GameOver();
        }

        public void SandwitchSpanking()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.FeelingEuphoricAfterVictory".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.CantEvenStandUp".Localized());
            else
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.CantEvenStopMasturbating".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.KickHerHandAway".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.ButStillMasturbating".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.TimeForSomeFun".Localized());

            ShowImage("sandwitch-get-spank");

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.GrabTheHelplessWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.WitchProtestsOnce".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.ActuallyKindOfFun".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.ThoughYouFeelKind".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Sandwitchscene.SandwitchSpanking.ThoughYouArentSure".Localized());
                else
                    OutputText("Scenes.Sandwitchscene.SandwitchSpanking.ThoughUsedToFind".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.TweakAndTease".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.TooLateSeeWitch".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.SnapsWithMagicCrackling".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.WorseStillUnprotected".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Sandwitchscene.SandwitchSpanking.WorseStillPressedUp".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.BristleAtHelplessness".Localized());

            if (Player.HasPerk(PerkIds.FireLord) || Player.HasPerk(PerkIds.Hellfire) || Player.HasPerk(PerkIds.Dragonfire))
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.BreathingOutFlame".Localized());
            else
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.NothingYouCanDo".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.FingersTickleBottom".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.SmirkingSorceressLaughs".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.ThisSeemsLastForever".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.SoundsAlmostFanatical".Localized());
            else
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.WhatHellIsShe".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.PickingUpThePieces".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.AndWord".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.AllOfThemVibrating".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Sandwitchscene.SandwitchSpanking.AndWordAgain".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchSpanking.BruisedAndOrgasmed".Localized());

            CombatController.Instance!.Context.Enemy.Lust = 98;

            CombatController.Instance!.Context.Enemy.Health = 2;

            Player.Lust = Player.MaxLust;

            CoC.Instance.Flags.Set(GameFlagIds.CombatBonusXpValue, CombatController.Instance!.Context.Enemy.Experience);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));
        }

        public void SandwitchOverwhelm()
        {
            ClearOutput();

            OutputText("Scenes.Sandwitchscene.SandwitchOverwhelm.AppearsToBeEnjoying".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchOverwhelm.BlissfulScream".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchOverwhelm.SheBegsWitchBegins".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchOverwhelm.ExclaimsWeakAndLimp".Localized());

            OutputText("Scenes.Sandwitchscene.SandwitchOverwhelm.LastClimaxAppearsTo".Localized());

            CombatScene.CleanupAfterCombat();
        }

    }
}