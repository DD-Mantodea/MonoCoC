using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Dungeons
{

    public class DeepCaveFungusScene : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            OutputText("Scenes.DeepCave.RoomFungusCavern.FungusCavernHeading".Localized());

            OutputText("Scenes.DeepCave.RoomFungusCavern.CavernIsHuge".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetazFungusRoomDefeated) == 0)
            {
                OutputText("Scenes.DeepCave.RoomFungusCavern.StrangeSweetSmell".Localized());

                AddButton(0, "Common.Button.GetSword".Localized(), GetSwordAndGetTrapped);

                if (Player.CanFly())
                    AddButton(1, "Common.Button.FlyToSword".Localized(), FlyToSwordAndGetTrapped);
            }
            else
                OutputText("Scenes.DeepCave.RoomFungusCavern.FamiliarSweetSmell".Localized());
        }

        public void GetSwordAndGetTrapped()
        {
            ClearOutput();

            OutputText("Scenes.DeepCave.GetSwordAndGetTrapped.StumbleNearlyFall".Localized());

            if (Player.Speed < 50)
                OutputText("Scenes.DeepCave.GetSwordAndGetTrapped.WalkToCorpse".Localized(
                     Player.AssDescript()
                ));
            else
                OutputText("Scenes.DeepCave.GetSwordAndGetTrapped.FallFlat".Localized(Player.AssDescript()));

            GetTrappedContinuation();
        }

        public void FlyToSwordAndGetTrapped()
        {
            ClearOutput();

            OutputText("Scenes.DeepCave.FlyToSwordAndGetTrapped.FlyToCorpse".Localized());

            GetTrappedContinuation();
        }

        public void GetTrappedContinuation()
        {

            OutputText("Scenes.DeepCave.GetTrappedContinuation.LoudSlap".Localized());

            OutputText("Scenes.DeepCave.GetTrappedContinuation.TentaclesSpringUp".Localized());

            OutputText("Scenes.DeepCave.GetTrappedContinuation.SweetAromaStronger".Localized());

            Player.CreateStatusEffect(new Identifier("coc", "immobilized"), 1, 0, 0, 0);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.EncapsulationPod), () => Go<CampScene>());
        }

        public void EncapsulationVictory()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health <= 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ZetazFungusRoomDefeated, CoC.Instance.Flags.Get(GameFlagIds.ZetazFungusRoomDefeated) + 1);

                OutputText("Scenes.DeepCave.EncapsulationVictory.PodWallBursts".Localized());

                OutputText("Scenes.DeepCave.EncapsulationVictory.RapierStillLies".Localized());
            }

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToThisShitPartII()
        {
            ClearOutput();

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.SheathWord".Localized());

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.HeadClearsMilkEscape".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.ThickSlimeOverLips".Localized(
                     Player.SMultiCockDesc()
                ));

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.TwitchesAnalOnly".Localized());

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.IsntRapeAnymore".Localized());

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.LogicAndRestDont".Localized());

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.ComesToHalt".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

            if (!Player.HasVagina())
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.SuchStimulation".Localized());
            else
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.SuchStimulation".Localized());

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.FluidsGurgle".Localized());

                if (Player.BiggestLactation() > 1)
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.CockBloatsWithBlood".Localized(Player.SMultiCockDesc()));

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.SqueezeInflate".Localized());
            }

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.MilkEscape".Localized(Player.AllBreastsDescript()));

                if (Player.CumQ() > 800)
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.TentaclesSqueezeRotate".Localized());
            }

            if (Player.BiggestLactation() > 1)
            {
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.WhileFuckingContinues".Localized());

                if (Player.BiggestLactation() < 2)
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.BaseWord".Localized());
                else
                    if (Player.BiggestLactation() < 3)
                        OutputText("Scenes.DeepCave.LoseToThisShitPartII.TwitchesDoublePen".Localized());
                    else
                        OutputText("Scenes.DeepCave.LoseToThisShitPartII.WalkToSkeleton".Localized());
            }

            OutputText("Scenes.DeepCave.LoseToThisShitPartII.ParagraphBreak".Localized());

            if (Player.CumQ() > 3500)
            {
                OutputText("Scenes.DeepCave.LoseToThisShitPartII.SteadyRhythm".Localized());

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.PodFeedsOnSemen".Localized(
                     Player.SMultiCockDesc()
                ));

                CombatController.Instance!.Context.Enemy.Lust = 100;

                Player.Orgasm("Dick");
            }
            else
                if (Player.LactationQ() > 3500 || (Player.LactationQ() + Player.CumQ() > 4500))
                {
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.OrgasmVaginalTentacles".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.PodFeedsOnSemen".Localized());

                    CombatController.Instance!.Context.Enemy.Lust = 100;

                    Player.Orgasm("Tits");
                }
                else
                    if (Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.DeepCave.LoseToThisShitPartII.BallsWord".Localized());

                        OutputText("Scenes.DeepCave.LoseToThisShitPartII.AndMilkWord".Localized());

                        OutputText("Scenes.DeepCave.LoseToThisShitPartII.PluralS".Localized());

                        CombatController.Instance!.Context.Enemy.Lust = 100;

                        CombatController.Instance!.Context.Enemy.Experience = 1;

                        Player.Orgasm("Anal");
                    }

            if (CombatController.Instance!.Context.Enemy.Lust != 100)
            {
                CoC.Instance.Flags.Set(GameFlagIds.ZetazFungusRoomDefeated, CoC.Instance.Flags.Get(GameFlagIds.ZetazFungusRoomDefeated) + 1);

                CombatScene.CleanupAfterCombat();

                return ;
            }

            if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
            {
                if (Player.Gender == Gender.Male)
                    ShowImage("badend-pod-male");
                else
                    ShowImage("badend-pod-herm");

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.OrgasmDragsOn".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.WaveOfNeed".Localized());
                else
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.ColoringMoreWhite".Localized());

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.BeforePassOut".Localized());

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.OrgasmAnalTentacle".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.FloatingInBliss".Localized());
                else
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.ColoringMoreWhite".Localized());

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.OrgasmSquirtsDrips".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.DeepCave.LoseToThisShitPartII.ColoringMoreWhite".Localized());

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.VineStalksConstrict".Localized());

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.TentacleIntoMouth".Localized());

                GameOver();
            }
            else
            {
                ShowImage("badend-pod-female");

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.BallsWord".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.HeadClearsCumEscape".Localized(Player.SMultiCockDesc()));

                OutputText("Scenes.DeepCave.LoseToThisShitPartII.NipplesSquirt".Localized());

                GameOver();
            }
        }

    }
}