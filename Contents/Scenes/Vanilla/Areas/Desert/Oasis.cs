using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<Oasis>("coc:oasis")]
    public class Oasis : Scene
    {

        public override void Entry()
        {
            OasisEncounter();
        }

        public void OasisEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Oasis.OasisEncounter.WandersDesert".Localized());

            OutputText("Scenes.Oasis.OasisEncounter.SittingShade".Localized());

            OutputText("Scenes.Oasis.OasisEncounter.GroupDescribed".Localized());

            OutputText("Scenes.Oasis.OasisEncounter.NoticesLate".Localized());

            OutputText("Scenes.Oasis.OasisEncounter.WhatDoBanner".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), OasisTalk);

            AddButton(1, "Common.Button.Fight".Localized(), ChooseToFight);

            AddButton(14, "Common.Button.Leave".Localized(), OasisRunAway);
        }

        public void ChooseToFight()
        {

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.DemonPack), () => Go<CampScene>(), canFlee: false);
        }

        public void OasisRunAway()
        {
            ClearOutput();

            if (Player.Speed > 15 && Player.Speed / 2 > Random.Shared.Next(40))
            {
                OutputText("Scenes.Oasis.OasisRunAway.BoltsOut".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Oasis.OasisRunAway.ScramblesAway".Localized((noFur ? "ears" : "features")));

                        CombatScene.Start(EnemyRegistry.Create(EnemyIds.DemonPack), () => Go<CampScene>());
            }
        }

        public void OasisTalk()
        {
            ClearOutput();

            OutputText("Scenes.Oasis.OasisTalk.RisesCautiously".Localized());

            OutputText("Scenes.Oasis.OasisTalk.DesertConversation".Localized());

            OutputText("Scenes.Oasis.OasisTalk.StayOrLeaveBanner".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), OasisTalkAccept);

            AddButton(14, "Common.Button.Leave".Localized(), OasisTalkDecline);
        }

        public void OasisTalkDecline()
        {
            ClearOutput();

            OutputText("Scenes.Oasis.OasisTalkDecline.ConsidersDecline".Localized());

            OutputText("Scenes.Oasis.OasisTalkDecline.CirclesMenacingly".Localized());

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.DemonPack), () => Go<CampScene>());
        }

        public void OasisTalkAccept()
        {
            ClearOutput();

            OutputText("Scenes.Oasis.OasisTalkAccept.LeaderSmiles".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Oasis.OasisTalkAccept.LegsBuckle".Localized());
            else
                OutputText("Scenes.Oasis.OasisTalkAccept.LegsFallOpen".Localized());

            OutputText("Scenes.Oasis.OasisTalkAccept.SilenceBroken".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.VoluntaryDemonpack))
                Player.CreateStatusEffect(StatusEffectIds.VoluntaryDemonpack, 0, 0, 0, 0);
            else
                Player.ChangeStatusValue(StatusEffectIds.VoluntaryDemonpack, 0, 1);

            DoNext(() => OasisSexing());
        }

        public void OasisSexing()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Oasis.OasisSexing.ClawedHandsGrasp".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Oasis.OasisSexing.EachOfWord".Localized());

                OutputText("Scenes.Oasis.OasisSexing.SlidesUp".Localized());

                if (Player.Cocks[0].Length > 25)
                    OutputText("Scenes.Oasis.OasisSexing.SlidesInto".Localized(
                         Player.CockDescript(0)
                    ));
                else
                {
                    OutputText("Scenes.Oasis.OasisSexing.YoungerDemon".Localized());

                    if (Player.Cocks[0].Length > 17)
                        OutputText("Scenes.Oasis.OasisSexing.HardlyInside".Localized());
                    else
                        if (Player.Cocks[0].Length > 12)
                            OutputText("Scenes.Oasis.OasisSexing.HalfwayDown".Localized());
                        else
                            if (Player.Cocks[0].Length > 6)
                                OutputText("Scenes.Oasis.OasisSexing.AlmostAllInside".Localized());
                            else
                                if (Player.Cocks[0].Length <= 6)
                                    OutputText("Scenes.Oasis.OasisSexing.LipsTouchBase".Localized());

                    OutputText("Scenes.Oasis.OasisSexing.SlidesHerself".Localized());

                    if (Player.Cocks[0].Length > 6)
                    {
                        OutputText("Scenes.Oasis.OasisSexing.RealizesPussy".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
                    }

                    OutputText("Scenes.Oasis.OasisSexing.CockWhoreCums".Localized());
                }

                OutputText("Scenes.Oasis.OasisSexing.ShakingPumping".Localized());

                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Oasis.OasisSexing.LiesUnder".Localized());

                    if (Player.Cocks.Count == 2)
                        OutputText("Scenes.Oasis.OasisSexing.SingularIs".Localized(
                             Player.CockDescript(1)
                        ));
                    else
                        OutputText("Scenes.Oasis.OasisSexing.CocksAre".Localized());

                    OutputText("Scenes.Oasis.OasisSexing.OtherHands".Localized());
                }

                OutputText("Scenes.Oasis.OasisSexing.ParagraphBreak".Localized());
            }

            if (Player.Vaginas.Count > 0)
            {
                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Oasis.OasisSexing.TinyTrip".Localized());
                else
                    OutputText("Scenes.Oasis.OasisSexing.TussleForPlace".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.Vaginas[0].Looseness >= 5)
                    OutputText("Scenes.Oasis.OasisSexing.HoweverAllOfThis".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));
                else
                {
                    OutputText("Scenes.Oasis.OasisSexing.OneWins".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    Player.CuntChange(40, true);

                    OutputText("Scenes.Oasis.OasisSexing.TwoCocksPump".Localized(
                         Player.VaginaDescript(0)
                    ));
                }

                OutputText("Scenes.Oasis.OasisSexing.ImmenseFullness".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Oasis.OasisSexing.ParagraphBreakTwo".Localized());

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation, 90);
            }

            if (Player.Gender == Gender.Hermaphrodite)
                ShowImage("demonmob-surround-herm");
            else
                if (Player.Gender == Gender.Female)
                    ShowImage("demonmob-surround-female");
                else
                    ShowImage("demonmob-surround-male");

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Oasis.OasisSexing.InterestedInEvery".Localized());

            OutputText("Scenes.Oasis.OasisSexing.HandsAndDicks".Localized());

            if (Player.Vaginas.Count > 0)
                OutputText("Scenes.Oasis.OasisSexing.CocksInBoth".Localized());

            OutputText("Scenes.Oasis.OasisSexing.CockFucks".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Oasis.OasisSexing.DemonsWrapped".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 1f));

            if ((Player.BreastRows.Count > 0) && (Player.BiggestBreastSize() > 1))
                    OutputText("Scenes.Oasis.OasisSexing.DemonicHands".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

            OutputText("Scenes.Oasis.OasisSexing.MouthTaken".Localized());

            if (Player.Pregnancy.Incubation == 0 && (int)Player.Gender > 1)
            {
                OutputText("Scenes.Oasis.OasisSexing.ParagraphBreakThree".Localized());

                OutputText("Scenes.Oasis.OasisSexing.KeepsMentalCheck".Localized(
                     Player.VaginaDescript(0)
                ));
            }

            if ((CombatController.Instance!.Context.Enemy.Health < 1 || CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust) && CombatController.InCombat)
            {
                OutputText("Scenes.Oasis.OasisSexing.FucksUntilNone".Localized());

                CombatScene.CleanupAfterCombat();

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1.5f));

                return ;
            }
            else
                if ((Player.Health < 1 || Player.Lust >= Player.MaxLust) && CombatController.InCombat)
                {
                    if (SceneInstance<AntsScene>.Get().PlayingHero)
                    {
                        OutputText("Scenes.Oasis.OasisSexing.SoughtToSave".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.AntsPcFailedPhylla, 1);

                        SceneInstance<AntsScene>.Get().PlayingHero = false;
                    }

                    OutputText("Scenes.Oasis.OasisSexing.FuckLikeAnimals".Localized());

                    CombatScene.CleanupAfterCombat();

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Corruption, 3f));

                    return ;
                }

            if (Player.StatusEffectValue(StatusEffectIds.VoluntaryDemonpack, 0) >= 6 && Player.HasVagina())
            {
                OasisBadEnd();

                return ;
            }

            OutputText("Scenes.Oasis.OasisSexing.FuckForHours".Localized());

            Player.Orgasm("All");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Sensitivity, .5f), new DynStat(GameStats.Corruption, 4f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PlayerMenu();
        }

        public void OasisBadEnd()
        {
            ClearOutput();

            ShowImage("demonmob-end");

            OutputText("Scenes.Oasis.OasisBadEnd.FuckForHoursAgain".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.PassingOut".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Oasis.OasisBadEnd.FeetShackled".Localized());
            else
                OutputText("Scenes.Oasis.OasisBadEnd.LegsShackled".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.ArmsTied".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.LeaderSpeaks".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.YellsIndignation".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.SmirkWidens".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                if (Player.IsTaur())
                    OutputText("Scenes.Oasis.OasisBadEnd.LeaderSmirks".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Oasis.OasisBadEnd.SmirkWidensMore".Localized(
                         Player.CockDescript(0)
                    ));

            OutputText("Scenes.Oasis.OasisBadEnd.StaresDread".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.LeaderDeepens".Localized());

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Oasis.OasisBadEnd.ReachesDown".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.TugsChain".Localized());

            OutputText("Scenes.Oasis.OasisBadEnd.FlushesRed".Localized());

            DoNext(() => OasisBadEndEpilogue());
        }

        public void OasisBadEndEpilogue()
        {
            ClearOutput();

            ShowImage("badend-demonmob");

            OutputText("Scenes.Oasis.OasisBadEndEpilogue.AfterOneYear".Localized());

            if ((int)Player.Gender <= 1)
                OutputText("Scenes.Oasis.OasisBadEndEpilogue.FermentedMilk".Localized());

            OutputText("Scenes.Oasis.OasisBadEndEpilogue.EllipsisBreak".Localized());

            OutputText("Scenes.Oasis.OasisBadEndEpilogue.YearGoneBy".Localized());

            GameOver();
        }

    }
}