using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<TentacleBeastScene>("coc:tentacle_beast")]
    public class TentacleBeastScene : Scene
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

        public static SaveContent Save => SaveContent(SceneIds.Forest.TentacleBeast);

        public override void Entry()
        {
            Encounter();
        }

        public void Encounter()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless && Random.Shared.Next(3) == 0 && !Player.HasTailInsteadOfLegs() && !Player.IsTaur() && !Player.IsGoo())
            {
                ShowImage("tentaclebeast-sad-monster");

                OutputText("Scenes.TentacleBeast.Encounter.BeastEmerge".Localized());

                if (Player.Corruption > 50 && Player.Corruption <= 75)
                    OutputText("Scenes.TentacleBeast.Encounter.TooLate".Localized());

                if (Player.Corruption > 75)
                    OutputText("Scenes.TentacleBeast.Encounter.WelcomePleasure".Localized());

                if (Player.Corruption < 75)
                    OutputText("Scenes.TentacleBeast.Encounter.ResistMuscle".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.PinsYouDown".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.CreatureSpeaks".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.ConfusedShock".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.BellowsNaughtyBits".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.StateGenderless".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.MonsterContempt".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.OfferFood".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.RoarsNoMouth".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.SlapsAss".Localized());

                OutputText("Scenes.TentacleBeast.Encounter.WalkAway".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, -5f));

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            ShowImage("monster-tentaclebeast");

            OutputText("Scenes.TentacleBeast.Encounter.BeastEmergeAlt".Localized());

            if (Player.Corruption > 50 && Player.Corruption <= 75)
                OutputText("Scenes.TentacleBeast.Encounter.DebateRun".Localized());

            if (Player.Corruption > 75)
                OutputText("Scenes.TentacleBeast.Encounter.WelcomePleasureAlt".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                OutputText("Scenes.TentacleBeast.Encounter.StopsSniffs".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (Player.Corruption > 50)
            {
                OutputText("Scenes.TentacleBeast.Encounter.SubmitOrFight".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), StartTentacleBeastCombat);

                AddButton(1, "Common.Button.Submit".Localized(), TentacleLossRape);

                return ;
            }

                        var tentacleBeast = EnemyRegistry.Create(EnemyIds.TentacleBeast);

            tentacleBeast.OnWon = _ => TentacleLossRape();

            CombatScene.Start(tentacleBeast, () => Go<CampScene>());
        }

        public void StartTentacleBeastCombat()
        {

                        var tentacleBeast = EnemyRegistry.Create(EnemyIds.TentacleBeast);

            tentacleBeast.OnWon = _ => TentacleLossRape();

            CombatScene.Start(tentacleBeast, () => Go<CampScene>(), canFlee: false);
        }

        public void TentacleVictoryRape()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male || Player.Gender == Gender.Hermaphrodite)
            {
                ShowImage("tentaclebeast-win-male");

                OutputText("Scenes.TentacleBeast.TentacleVictoryRape.RushGrabHollow".Localized());

                OutputText("Scenes.TentacleBeast.TentacleVictoryRape.WantDickMilk".Localized());

                OutputText("Scenes.TentacleBeast.TentacleVictoryRape.ImpaleTentacle".Localized());

                OutputText("Scenes.TentacleBeast.TentacleVictoryRape.QuitSquirming".Localized());

                OutputText("Scenes.TentacleBeast.TentacleVictoryRape.CumHowitzer".Localized());

                OutputText("Scenes.TentacleBeast.TentacleVictoryRape.WithdrawLeave".Localized());
            }
            else
            {
                Player.SlimeFeed();

                ShowImage("tentaclebeast-win-female");

                if (Player.Corruption >= 66)
                {
                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.RushGrabPhallic".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.StuffWomb".Localized());

                    Player.CuntChange(20, true, true, false);

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.VaginalMuscles".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.StopStruggling".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.PunchingBagOrgasm".Localized());
                }
                else
                {
                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.RushPushDown".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.SitOnCreature".Localized());

                    Player.CuntChange(20, true, true, false);

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.WombFeeler".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleVictoryRape.BloatedOrgasm".Localized());
                }

                Player.KnockUp(PregnancyIds.TentacleBeastSeed, PregnancyIds.TentacleBeastSeedIncubation);
            }

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void FutaTentacleBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.TentacleBeast.FutaTentacleBadEnd.SwallowedWhole".Localized());

            OutputText("Scenes.TentacleBeast.FutaTentacleBadEnd.TrappedInside".Localized());

            ShowImage("badend-tentaclebeast");

            OutputText("Scenes.TentacleBeast.FutaTentacleBadEnd.MilkedFiercely".Localized());

            OutputText("Scenes.TentacleBeast.FutaTentacleBadEnd.LivingFoodProducer".Localized());

            OutputText("Scenes.TentacleBeast.FutaTentacleBadEnd.FadeInAndOut".Localized());

            OutputText("Scenes.TentacleBeast.FutaTentacleBadEnd.WakedByLight".Localized());

            DoNext(() => FutaTentacleEpilogue());
        }

        public void FutaTentacleEpilogue()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GiacomoMet) > 0)
            {
                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.GiacomoSpeaks".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.FogLifts".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.GiacomoComments".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.CannotSpeak".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.GiacomoJingles".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.GiacomoGiggles".Localized());
            }
            else
            {
                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.VoiceAsksLive".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.VoiceRepliesLive".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.DoctorQueries".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.ManAnswers".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.LoadOntoWagon".Localized(
                     Player.Mf("man", "woman"),

                     Player.Mf("him", "her")
                ));

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.LiftedOntoWagon".Localized());

                OutputText("Scenes.TentacleBeast.FutaTentacleEpilogue.YearOfRecovery".Localized());
            }

            GameOver();
        }

        public void TentacleLossRape()
        {
            ClearOutput();

            Save.SetInt("timesLost", Save.GetInt("timesLost", 0) + 1);

            if (Player.Gender == Gender.Genderless)
                if (Player.IsTaur())
                {
                    CentaurGenderlessRetardation();

                    return ;
                }
                else
                    if (Player.IsNaga())
                    {
                        GenderlessHilarityForNagaKenDolls();

                        return ;
                    }
                    else
                        if (Player.IsGoo())
                        {
                            TentacularGenderGooTimes();

                            return ;
                        }
                        else
                        {
                            ShowImage("tentaclebeast-sad-monster");

                            if (Player.Corruption < 75)
                                OutputText("Scenes.TentacleBeast.TentacleLossRape.ResistMuscleAgain".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.PinsYouDown".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.CreatureSpeaks".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.BellowsNaughtyBits".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.StateGenderless".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.MonsterContempt".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.OfferFood".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.RoarsNoMouth".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.SlapsAss".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.WalkAway".Localized());

                            if (CombatController.InCombat)
                                CombatScene.CleanupAfterCombat();
                            else
                                ReturnToCamp(1);

                            return ;
                        }

            if (Player.CountCocksOfType("Horse") > 0 && Player.Cocks[0].Length > 15 && Player.Cocks[0].Thickness >= 3)
            {
                if (Player.Corruption < 75 && Player.Lust < Player.MaxLust)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.GrabsBeforeEscapeAgain".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.PinsYouThorn".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.HollowTentacle".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.LullCreature".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.BallsRiseDrop".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ParagraphBreak".Localized());

                ShowImage("tentaclebeast-loss-horse");

                OutputText("Scenes.TentacleBeast.TentacleLossRape.BaskInGlow".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.PumpAndThrust".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.Balls".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.Cock".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThrustDeep".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.BallsOverfilled".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.CumOverfilled".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.LastJerk".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f), new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                CombatController.Instance!.Context.Enemy.Health = 0;

                if (Player.Health == 0)
                    Player.Health += 1;

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    ReturnToCamp(1);

                return ;
            }

            if (Player.Lust >= Player.MaxLust)
            {
                var temp= Player.StatusEffectByType(StatusEffectIds.TentacleBadEndCounter);

                if (temp == null)
                    Player.CreateStatusEffect(StatusEffectIds.TentacleBadEndCounter, 0, 0, 0, 0);
                else
                {
                    temp.Values[0] += 1;

                    if (temp.Values[0] >= 3 && Player.Corruption > 50 && Player.Gender == Gender.Hermaphrodite)
                    {
                        FutaTentacleBadEnd();

                        return ;
                    }
                }
            }
            else
                if (Player.IsTaur())
                {
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.TentaclesWrapLegs".Localized());

                    Player.SlimeFeed();

                    OutputText("Scenes.TentacleBeast.TentacleLossRape.TestYourBody".Localized());

                    if (Player.HasCock())
                        if (Player.CockArea(Player.SmallestCockIndex()) <= 50)
                        {
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.ToxinPours".Localized());

                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.TentacleBeast.TentacleLossRape.SurprisedAtCocks".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.ParagraphBreakAgain".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.UnseenTentacle".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.ToxinPoursAgain".Localized());

                            if (Player.Balls > 0)
                                OutputText("Scenes.TentacleBeast.TentacleLossRape.SuckingReaches".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.ParagraphBreakThird".Localized());
                        }

                    ShowImage("tentaclebeast-loss-taur");

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.SquirmingTentacle".Localized(
                             Player.VaginaDescript(0)
                        ));

                        Player.CuntChange(32, true, true, false);

                        if ((Player.HasCock()) && (Player.CockArea(Player.SmallestCockIndex()) <= 50))
                                OutputText("Scenes.TentacleBeast.TentacleLossRape.HumpingNoEffect".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeastBottomsOut".Localized());
                    }

                    if (Player.BiggestBreastSize() >= 1 && Player.LactationQ() <= 0)
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.RovingTentacles".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                        if (Player.BreastRows.Count() == 2)
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.AnusTipsAffix".Localized());
                        else
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.AnusTipsAttach".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.GushOfLiquid".Localized());

                        Player.BoostLactation(1.5f);
                    }

                    if (Player.Anal.Looseness >= 4)
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.InvitingTarget".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.UndulatingLumps".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.MoanInto".Localized());
                    }

                    if (Player.BiggestBreastSize() >= 1 && Player.LactationQ() > 0 && Player.LactationQ() < 1000)
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.RovingTentaclesAgain".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                        Player.BoostLactation(1);
                    }
                    else
                        if (Player.BiggestBreastSize() >= 1 && Player.LactationQ() >= 1000)
                        {
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.RovingTentaclesThird".Localized(
                                 Appearance.AllBreastsDescript(Player)
                            ));

                            if (Player.HasCock())
                            {
                                OutputText("Scenes.TentacleBeast.TentacleLossRape.CockExplodes".Localized());

                                if (Player.CumQ() <= 500)
                                    OutputText("Scenes.TentacleBeast.TentacleLossRape.PushingToEdge".Localized());
                                else
                                    OutputText("Scenes.TentacleBeast.TentacleLossRape.QuicklyOverfilling".Localized());
                            }

                            OutputText("Scenes.TentacleBeast.TentacleLossRape.TentaclesRelease".Localized());

                            Player.Orgasm("Tits");

                            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                            Player.BoostLactation(.5f);

                            CombatController.Instance!.Context.Enemy.Health = 0;

                            if (Player.Health == 0)
                                Player.Health += 1;

                            if (CombatController.InCombat)
                                CombatScene.CleanupAfterCombat();
                            else
                                ReturnToCamp(1);

                            return ;
                        }

                    if (Player.HasCock())
                    {
                        Player.CumMultiplier += .5f;

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.DesiresFulfilled".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.BallsTighten".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.RearUpStrength".Localized());
                    }

                    if (Player.HasVagina())
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.RipplesCoiled".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                    if (Player.HasCock() && Player.CumQ() < 1500 && Player.Anal.Looseness < 4)
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.AnotherTentacle".Localized());

                        Player.ButtChange(40, true, true, false);

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.ViolateUntilBlackout".Localized());

                        Player.Orgasm("Anal");

                        Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

                        if (CombatController.InCombat)
                            CombatScene.CleanupAfterCombat();
                        else
                            ReturnToCamp(2);

                        return ;
                    }

                    if (Player.HasCock() && Player.CumQ() < 1500 && Player.Anal.Looseness >= 0)
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.TentacleSwells".Localized());

                        Player.Orgasm("Anal");

                        Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

                        if (CombatController.InCombat)
                            CombatScene.CleanupAfterCombat();
                        else
                            ReturnToCamp(2);

                        return ;
                    }

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.TentacleRamsAgain".Localized());

                        Player.ButtChange(40, true, true, false);

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.ViolateUntilBlackoutAgain".Localized());

                        Player.Orgasm("Anal");

                        Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

                        if (CombatController.InCombat)
                            CombatScene.CleanupAfterCombat();
                        else
                            ReturnToCamp(2);

                        return ;
                    }

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.PumpMore".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.SackAnd".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.HindLegs".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.EventuallyYou".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.TentacleBeast.TentacleLossRape.BallsFragment".Localized());

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.TurnToLeave".Localized());

                        Player.Orgasm("Dick");

                        Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                        CombatController.Instance!.Context.Enemy.Health = 0;

                        if (Player.Health == 0)
                            Player.Health += 1;

                        if (CombatController.InCombat)
                            CombatScene.CleanupAfterCombat();
                        else
                            ReturnToCamp(1);

                        return ;
                    }

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

                    if (CombatController.InCombat)
                        CombatScene.CleanupAfterCombat();
                    else
                        ReturnToCamp(2);

                    return ;
                }

            if (Player.BiggestLactation() >= 3.5f && (int)Player.Gender > 0)
            {
                Player.SlimeFeed();

                OutputText("Scenes.TentacleBeast.TentacleLossRape.TendrilAroundEars".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, 2f));

                OutputText("Scenes.TentacleBeast.TentacleLossRape.LightHeaded".Localized());

                if (Player.Gender == Gender.Male)
                {
                    OutputText(Player.MultiCockDescriptLight());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeginAgain".Localized());
                    else
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeginsAgain".Localized());
                }
                else
                    if (Player.CockTotal() > 0)
                    {
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.AndCocks".Localized(
                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeginAgain".Localized());
                    }
                    else
                    {
                        OutputText(Player.VaginaDescript(0));

                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeginsAgain".Localized());
                    }

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThrobUrgently".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ShudderAsEncircled".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

                if (Player.Gender == Gender.Female)
                {
                    if (Player.VaginalCapacity() < 30)
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeastSensesExcitement".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeastSensesExcitementAgain".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                    Player.CuntChange(20, true);

                    OutputText("Scenes.TentacleBeast.TentacleLossRape.ParagraphBreakFourth".Localized());
                }

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    if (Player.VaginalCapacity() < 30)
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeastSensesExcitementThird".Localized(
                             Player.VaginaDescript(0),

                             Player.CockDescript(0)
                        ));
                    else
                        OutputText("Scenes.TentacleBeast.TentacleLossRape.BeastSensesExcitementFourth".Localized(
                             Player.VaginaDescript(0),

                             Player.CockDescript(0)
                        ));

                    Player.CuntChange(20, true);

                    OutputText("Scenes.TentacleBeast.TentacleLossRape.ParagraphBreakFifth".Localized());
                }

                OutputText("Scenes.TentacleBeast.TentacleLossRape.SlowlyBecomeAware".Localized(
                     Player.NippleDescript(0),

                     Appearance.AllBreastsDescript(Player)
                ));

                if ((int)Player.Gender >= 2)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.AcheButNotDepleted".Localized(
                         Appearance.AllBreastsDescript(Player),

                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.DribblingMilk".Localized(
                         Player.CockDescript(0),

                         Player.CockDescript(0)
                    ));

                Player.Orgasm("Tits");

                Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Speed, -.5f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                Player.BoostLactation(.5f);

                CombatController.Instance!.Context.Enemy.Health = 0;

                if (Player.Health == 0)
                    Player.Health += 1;

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    ReturnToCamp(1);

                return ;
            }

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
            {
                OutputText("Scenes.TentacleBeast.TentacleLossRape.TooOverwhelmed".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.TentaclesSnake".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThornProtrudes".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.PhallicSlams".Localized());

                Player.CuntChange(Player.VaginalCapacity() * .76f, true, true, false);

                OutputText("Scenes.TentacleBeast.TentacleLossRape.PantingAgape".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.BestialHowling".Localized());

                Player.SlimeFeed();

                Player.Orgasm("Vaginal");

                Player.KnockUp(PregnancyIds.TentacleBeastSeed, PregnancyIds.TentacleBeastSeedIncubation);

                Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    ReturnToCamp(2);

                return ;
            }

            if (Player.Gender == Gender.Male)
            {
                ShowImage("tentaclebeast-loss-male");

                Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 1f));

                if (Player.Corruption < 75)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.GrabsBeforeEscapeAgain".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.PinsYouThornAgain".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.AtBaseOfCocks".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThornInPelvis".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.MemberErect".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.MembersErect".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.StruggleTooLate".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.HollowTentaclePoised".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.HollowTentaclesPoised".Localized(
                         Player.CockTotal()
                    ));
            }

            if (Player.Gender == Gender.Female)
            {
                Player.SlimeFeed();

                ShowImage("tentaclebeast-loss-female");

                Player.ApplyStats(new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 1f));

                if (Player.Corruption < 75)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.GrabsBeforeEscapeThird".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.TentaclesBurst".Localized());

                if (Player.Corruption < 75)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.StruggleConstricted".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.SpreadEagled".Localized());

                if (Player.Corruption < 75)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.ScreamForHelp".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThornyTendrilsPierce".Localized(
                     NumberUtils.Num2Text(Player.BreastRows.Count() + 1)
                ));

                if (Player.Vaginas.Count == 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.Clit".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.Clits".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.HeatAndSwell".Localized());
            }

            if (Player.Gender == Gender.Hermaphrodite)
            {
                Player.SlimeFeed();

                ShowImage("tentaclebeast-loss-herm");

                Player.ApplyStats(new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, 35f), new DynStat(GameStats.Corruption, 2f));

                if (Player.Corruption < 75)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.ResistMuscleAgain".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.PinsYouThornThird".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.Cocks".Localized());
                else
                    OutputText(Player.CockDescript(0));

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThornInPelvisAgain".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.CocksErect".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.MemberErectAgain".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ThornyNipplePierce".Localized(
                     NumberUtils.Num2Text(Player.TotalNipples())
                ));

                Player.GrowTits(1, Player.BreastRows.Count, false, 2);

                OutputText("Scenes.TentacleBeast.TentacleLossRape.StruggleTooLateAgain".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.FuriousCocks".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.FuriousCock".Localized());

                OutputText("Scenes.TentacleBeast.TentacleLossRape.ScreamInShock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.ClampsPubicMound".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleLossRape.ClampsPubicMoundAgain".Localized());
            }

            TentacleRapeContinuation();
        }

        public void TentacleRapeContinuation()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

            if (Player.Gender == Gender.Male)
            {
                ShowImage("tentaclebeast-loss-male-sec-part");

                OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.AnotherTentaclePast".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.SuckingStroking".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.SuckingStrokingAgain".Localized());

                Player.ButtChange(50, true);

                OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.HelplessOverwhelmed".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.PhallusExplodes".Localized());
                else
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.DicksExplode".Localized());

                if (Random.Shared.Next(2) == 0)
                    Player.Orgasm("Anal");
                else
                    Player.Orgasm("Dick");
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    ShowImage("tentaclebeast-loss-female-sec-part");

                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.BeakMawReveals".Localized());

                    if (Player.Vaginas.Count == 1)
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.TongueLikeCock".Localized());
                    else
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.TonguesLikeCocks".Localized(
                             Player.Vaginas.Count,

                             Player.VaginaDescript(0)
                        ));

                    if (Player.Vaginas.Count > 1)
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.PluralSThird".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.CryOutShock".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.Coordination".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.Vaginas.Count > 1)
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.PluralSThird".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.MouthMilking".Localized());

                    if (Player.Vaginas.Count > 1)
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.Clits".Localized());
                    else
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.Clit".Localized());

                    Player.CuntChange(Player.VaginalCapacity() * .76f, true);

                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.BodyBetrays".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.Vaginas.Count > 1)
                        OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.PluralSThird".Localized());

                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.MouthInseminated".Localized());

                    DoNext(() => TentacleRapeContinuationForFemales());

                    Player.SlimeFeed();

                    Player.BoostLactation(.3f);

                    if (Random.Shared.Next(2) == 0)
                        Player.Orgasm("Anal");
                    else
                        Player.Orgasm("Vaginal");

                    return ;
                }
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        ShowImage("tentaclebeast-loss-herm-sec-part");

                        if (Player.CockTotal() == 1)
                        {
                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.SharpTug".Localized(
                                 Player.CockDescript(0),

                                 Player.BreastRows.Count(),

                                 Player.VaginaDescript(0)
                            ));

                            Player.CuntChange(Player.VaginalCapacity() * .76f, true, false, true);

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.FreakishCoordination".Localized(
                                 Player.CockDescript(0),

                                 Player.VaginaDescript(0),

                                 Player.CockDescript(0),

                                 Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.DrunkWithPleasureAgain".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.MilkingStopsAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.WithdrawsAgain".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.SharpTugAgain".Localized(
                                 Player.BreastRows.Count(),

                                 Player.VaginaDescript(0)
                            ));

                            Player.CuntChange(Player.VaginalCapacity() * .76f, true, true, false);

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.FreakishCoordinationAgain".Localized(
                                 Player.VaginaDescript(0),

                                 Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.DrunkWithPleasureAgain".Localized());

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.MilkingStopsAgain".Localized(
                                 Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.TentacleBeast.TentacleRapeContinuation.WithdrawsAgain".Localized());
                        }

                        Player.KnockUp(PregnancyIds.TentacleBeastSeed, PregnancyIds.TentacleBeastSeedIncubation);

                        Player.SlimeFeed();

                        Player.BoostLactation(.3f);

                        Player.Orgasm("Vaginal");

                        Player.Orgasm("Tits", false);
                    }

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void TentacleRapeContinuationForFemales()
        {
            ClearOutput();

            ShowImage("tentaclebeast-loss-female-third-part");

            if (Player.Vaginas.Count == 1)
            {
                OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.SatisfiedDropsYou".Localized());

                if ((Player.HasPerk(PerkIds.BigClit) && Player.GetClitLength() > 2) || Player.GetClitLength() > 3)
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.ClitSwollenAlmost".Localized(
                         NumberUtils.Num2Text((float)Math.Floor(Player.GetClitLength() * 1.75f))
                    ));
                else
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.ClitFourInches".Localized());
            }
            else
                OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.SatisfiedDropsTwoClits".Localized(
                     Player.Vaginas.Count
                ));

            if (Player.Corruption < 75)
                OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.TooLustFume".Localized());
            else
                OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.TooLustContinue".Localized());

            if (Player.GetClitLength() < 7 && Player.GetClitLength() >= 3.5f && Player.HasPerk(PerkIds.BigClit))
            {
                Player.Vaginas[0].ClitLength += .1f + Player.Corruption / 100;

                OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.ClittyDiminishes".Localized(
                     (int)(Player.GetClitLength() * 10) / 10
                ));

                if (Player.GetClitLength() < 5)
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.LikeTinyCock".Localized());

                if (Player.GetClitLength() >= 5 && Player.GetClitLength() < 7)
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.LikeSlickCock".Localized());

                if (Player.GetClitLength() >= 7)
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.LikeBigCock".Localized());
            }
            else
                if (Player.GetClitLength() < 3.5f)
                {
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.ClitReturnsLarger".Localized());

                    Player.Vaginas[0].ClitLength += .2f;
                }
                else
                    OutputText("Scenes.TentacleBeast.TentacleRapeContinuationForFemales.ClitReturnsNormal".Localized());

            if (Player.Vaginas[0].Looseness == 0)
                Player.Vaginas[0].Looseness = 1;

            Player.SlimeFeed();

            Player.KnockUp(PregnancyIds.TentacleBeastSeed, PregnancyIds.TentacleBeastSeedIncubation);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void CentaurGenderlessRetardation()
        {
            ClearOutput();

            ShowImage("tentaclebeast-sad-monster");

            if (CoC.Instance.Flags.Get(GameFlagIds.GenderlessCentaurMadness) == 0 || Player.Balls == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GenderlessCentaurMadness, 1);

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.LegsBoundAgain".Localized());

                if (Player.Corruption < 50 && Player.LustPercent < 70)
                    OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.DreadAgain".Localized());
                else
                    OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.AnticipationAgain".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.StopsAtHaunches".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.YellsWrongBody".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.MouthHangsOpen".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.ShrillsNoBits".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.StammerNoGenitals".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.VoiceSwitches".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.SmacksAss".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.CaughtHorse".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.AshamedAgrees".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.MoreSilence".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.SlapsFace".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.InsultHorse".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.SlamsTentacles".Localized());
            }
            else
            {
                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.LegsBoundAgain".Localized());

                if (Player.Corruption < 50 && Player.LustPercent < 70)
                    OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.DreadAgain".Localized());
                else
                    OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.AnticipationAgain".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.ProbesHaunches".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.EerieVoiceSings".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.SecondTentacleHunts".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.MumblesFrustration".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.GliblyExplains".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.TentaclesCease".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.HowlsRounding".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.QuailSurprised".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.SecondOfAll".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.EyesBulge".Localized());

                OutputText("Scenes.TentacleBeast.CentaurGenderlessRetardation.QEDShouts".Localized());
            }

            Player.HealthChange(-5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void GenderlessHilarityForNagaKenDolls()
        {
            ClearOutput();

            ShowImage("tentaclebeast-sad-monster");

            OutputText("Scenes.TentacleBeast.GenderlessHilarityForNagaKenDolls.TentaclesBind".Localized());

            OutputText("Scenes.TentacleBeast.GenderlessHilarityForNagaKenDolls.VoiceFromUndergrowth".Localized());

            OutputText("Scenes.TentacleBeast.GenderlessHilarityForNagaKenDolls.StunnedNoGenitals".Localized());

            OutputText("Scenes.TentacleBeast.GenderlessHilarityForNagaKenDolls.WordedBadly".Localized());

            OutputText("Scenes.TentacleBeast.GenderlessHilarityForNagaKenDolls.OwnQuestion".Localized());

            OutputText("Scenes.TentacleBeast.GenderlessHilarityForNagaKenDolls.SlamsDownTourists".Localized());

            Player.HealthChange(-15);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void TentacularGenderGooTimes()
        {
            ClearOutput();

            ShowImage("tentaclebeast-sad-monster");

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.GrabbedCompressed".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.HumanVoiceSwears".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.RespondLiquid".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.SubstantialNaughtyBits".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.AwkwardSilence".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.NoAnything".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.ShakeHead".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.WondersTaste".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.LatchesSucks".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.SourApple".Localized());

            OutputText("Scenes.TentacleBeast.TentacularGenderGooTimes.KnockOut".Localized());

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void PumpkinBirth()
        {
            ClearOutput();

            OutputText("Scenes.TentacleBeast.PumpkinBirth.AbdomenCramps".Localized());

            OutputText("Scenes.TentacleBeast.PumpkinBirth.LumpShifts".Localized());

            OutputText("Scenes.TentacleBeast.PumpkinBirth.FluidDrips".Localized());

            OutputText("Scenes.TentacleBeast.PumpkinBirth.ForgetObject".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.TentacleBeast.PumpkinBirth.LazyHandLust".Localized());
            else
            {
                OutputText("Scenes.TentacleBeast.PumpkinBirth.GropeHelplessly".Localized());

                OutputText("Scenes.TentacleBeast.PumpkinBirth.ShiverInNeed".Localized());
            }

            OutputText("Scenes.TentacleBeast.PumpkinBirth.ObjectPopsOut".Localized());

            Player.Orgasm("Vaginal");

            OutputText("Scenes.TentacleBeast.PumpkinBirth.PostOrgasmHigh".Localized());

            OutputText("Scenes.TentacleBeast.PumpkinBirth.ScratchHead".Localized());

            if (Player.Body.Hips < 10)
            {
                Player.Body.Hips += 1;

                OutputText("Scenes.TentacleBeast.PumpkinBirth.ArmorFitsSnug".Localized());
            }

            Player.ClearPregnancy();

            OutputText("Scenes.TentacleBeast.PumpkinBirth.ParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EventParserEscape, 1);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PumpkinSlice), 1);
        }

    }
}