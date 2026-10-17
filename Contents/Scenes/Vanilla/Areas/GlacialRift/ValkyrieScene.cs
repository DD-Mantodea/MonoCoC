using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift
{
    [Register<ValkyrieScene>("coc:valkyrie")]
    public class ValkyrieScene : Scene
    {

        public override void Entry()
        {

        }

        public void LoseToValkyrie()
        {
            ClearOutput();

            OutputText("Scenes.Valkyriescene.LoseToValkyrie.UnableResist".Localized());

            if (Player.Health <= 0)
                OutputText("Scenes.Valkyriescene.LoseToValkyrie.HerOnslaught".Localized());
            else
                OutputText("Scenes.Valkyriescene.LoseToValkyrie.RisingLust".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrie.CollapsesKnees".Localized());

            if (CurrentMonster.LustPercent >= 70)
                LoseToValkyrieForcedOral();
            else
            {
                OutputText("Scenes.Valkyriescene.LoseToValkyrie.ContemptuousKick".Localized());

                OutputText("Scenes.Valkyriescene.LoseToValkyrie.UtterlyFreezing".Localized());

                if ((100 * Player.Strength / Player.GetMaxStats("str")) > 50)
                    Player.ApplyStats(new DynStat(GameStats.Strength, -1f));

                Player.ApplyStats(new DynStat(GameStats.Strength, -1f));

                if ((100 * Player.Speed / Player.GetMaxStats("spe")) > 50)
                    Player.ApplyStats(new DynStat(GameStats.Speed, -1f));

                Player.ApplyStats(new DynStat(GameStats.Speed, -1f));

                CombatScene.CleanupAfterCombat();
            }
        }

        public void LoseToValkyrieForcedOral()
        {

            var gemPenalty = (Player.Gems / 100) + 10 + Random.Shared.Next(20);

            if (gemPenalty > 100)
                gemPenalty = 100;

            if (gemPenalty > Player.Gems)
                gemPenalty = Player.Gems;

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.StandsOver".Localized(
                 Player.Mf("bastard", "bitch")
            ));

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.TooWord".Localized());

            if (Player.Health <= 0)
                OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.BeatenWord".Localized());
            else
                OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.LustyWord".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.NoFight".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.TentativeLick".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.FaceCoated".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.RewardedScream".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.LaughsStraightens".Localized());

            OutputText("Scenes.Valkyriescene.LoseToValkyrieForcedOral.ReachesDown".Localized(
                 gemPenalty
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            Player.Gems -= gemPenalty;

            CombatScene.CleanupAfterCombat();

            DoNext(() => ReturnToCamp(1));
        }

        public void WinAgainstValkyrie(bool hpVictory = true)
        {
            ClearOutput();

            if (hpVictory)
                OutputText("Scenes.Valkyriescene.WinAgainstValkyrie.BeatenBloodied".Localized());
            else
                OutputText("Scenes.Valkyriescene.WinAgainstValkyrie.ArousalOverflow".Localized());

            OutputText("Scenes.Valkyriescene.WinAgainstValkyrie.ConcedesWeakly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.AerialFuck".Localized(), null);

            AddButton(1, "Common.Button.AnalFuck".Localized(), null);

            AddButton(2, "Common.Button.GetLicked".Localized(), null);

            AddButton(3, "Common.Button.Scissor".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                {
                    AddButton(0, "Common.Button.AerialFuck".Localized(), ValkyrieAerialFuck);

                    AddButton(1, "Common.Button.AnalFuck".Localized(), ValkyrieAnalFuck);
                }

                if (Player.HasVagina())
                {
                    AddButton(2, "Common.Button.GetLicked".Localized(), ValkyrieLicksYou);

                    AddButton(3, "Common.Button.Scissor".Localized(), ValkyrieScissor);
                }
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void ValkyrieAerialFuck()
        {
            ClearOutput();

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.OverDefeated".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.SheHesitates".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.RealizesFloat".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.CommandsFlight".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.TakesMuch".Localized());

            if (Player.CockArea(0) >= 16)
                OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.NoMore".Localized());
            else
                OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.NoMoreToTake".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.ReturnsFavor".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.MusclesContract".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.ScreamsHalf".Localized());

            Player.Orgasm("Generic");

            if (Player.CanFly())
                OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.BeatsWings".Localized());
            else
                OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.ClutchesValkyrie".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.SpreadsWings".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAerialFuck.ReelingOrgasm".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void ValkyrieAnalFuck()
        {
            ClearOutput();

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.GrabsDefeated".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.GrabsHips".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.EagerSurprise".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.SoEager".Localized(
                 Player.CockDescript()
            ));

            if (Player.CockArea(0) >= 16)
                OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.BottomsOut".Localized());
            else
                OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.HiltsValkyrie".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.ScreamsPleasure".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.SmilesMoans".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.TightPartner".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieAnalFuck.WithdrawsPop".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void ValkyrieLicksYou()
        {
            ClearOutput();

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.TakesAdvantage".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.SurprisingTongue".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.SlipsTongue".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.GrindsSlit".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.LittleGasp".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.CannotResist".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.DickUnloads".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.ParagraphBreak".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieLicksYou.UtterlySatisfied".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void ValkyrieScissor()
        {
            ClearOutput();

            OutputText("Scenes.Valkyriescene.ValkyrieScissor.PushesGently".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieScissor.LittleGrin".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieScissor.ReachesOver".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieScissor.SpendsMinutes".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieScissor.CupsCheeks".Localized());

            OutputText("Scenes.Valkyriescene.ValkyrieScissor.ExhaustedPlay".Localized());

            Player.Orgasm("Vaginal");

            if (Player.CanDepositEel())
            {
                OutputText("Scenes.Valkyriescene.ValkyrieScissor.EelParasiteBanner".Localized());

                Player.DepositEel();
            }

            CombatScene.CleanupAfterCombat();
        }

    }
}