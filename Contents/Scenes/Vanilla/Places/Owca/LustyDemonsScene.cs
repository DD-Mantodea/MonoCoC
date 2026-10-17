using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Owca
{
    [Register<LustyDemonsScene>("coc:lusty_demons")]
    public class LustyDemonsScene : Scene
    {

        public float unitAmount = 0;

        public float unitHP = 50;

        public override void Entry()
        {
            MaxHP();
        }

        public float MaxHP()
        {

            return unitAmount * unitHP;
        }

        public void PerformCombatAction()
        {

            unitAmount = (float)Math.Ceiling(CurrentMonster.Health / unitHP);

            float i = 0;

            while (i < unitAmount)
            {
                if (Random.Shared.Next(4) == 0)
                    CombatController.Instance!.Resolver.EnemyLustAttack();
                else
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                OutputText("Scenes.LustyDemons.PerformCombatAction.BlankLine".Localized());

                i += 1;
            }
        }

        public void Defeated(bool hpVictory)
        {

            SceneInstance<OwcaScene>.Get().DefeetVapulasHorde();
        }

        public void Won(bool hpVictory, bool pcCameWorms = false)
        {

            if (pcCameWorms)
            {
                OutputText("Scenes.LustyDemons.Won.DemonsCloseIn".Localized());

                CombatController.Instance!.EndLustLoss();
            }
            else
                SceneInstance<OwcaScene>.Get().LoseOrSubmitToVapula();
        }

        public void Teased(float lustDelta)
        {

            if (lustDelta > 0 && lustDelta < 5)
                OutputText("Scenes.LustyDemons.Teased.LightLustReaction".Localized());

            if (lustDelta >= 5 && lustDelta < 10)
                OutputText("Scenes.LustyDemons.Teased.ModerateLustReaction".Localized());

            if (lustDelta >= 10)
                OutputText("Scenes.LustyDemons.Teased.HeavyLustReaction".Localized());
// 原版 Monster.hx:1543 applyTease()：显示本次挑逗造成的淫欲增量
            OutputText(" <b>(+" + lustDelta.ToString("0.#") + ")</b>");
        }

        public void RunCheck()
        {

            if (CurrentMonster.HasStatusEffect(StatusEffectIds.RunDisabled))
            {
                OutputText("Scenes.LustyDemons.RunCheck.PitWallsBlockRun".Localized());

                CombatScene.CombatMenu();
            }
            else
                RunCheck();
        }

    }
}