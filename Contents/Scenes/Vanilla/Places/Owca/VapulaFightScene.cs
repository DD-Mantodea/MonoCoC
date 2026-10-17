using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Owca
{
    [Register<VapulaFightScene>("coc:vapula_fight")]
    public class VapulaFightScene : Scene
    {

        public override void Entry()
        {
            PerformCombatAction();
        }

        public void PerformCombatAction()
        {

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        public void Defeated(bool hpVictory)
        {

            SceneInstance<OwcaScene>.Get().DefeetVapulasHorde();
        }

        public void Won(bool hpVictory, bool pcCameWorms = false)
        {

            if (pcCameWorms)
            {
                OutputText("Scenes.VapulaFight.Won.DemonsCloseIn".Localized());

                CombatController.Instance!.EndLustLoss();
            }
            else
                SceneInstance<OwcaScene>.Get().LoseOrSubmitToVapula();
        }

        public void Teased(float lustDelta)
        {

            if (lustDelta > 0 && lustDelta < 5)
                OutputText("Scenes.VapulaFight.Teased.LightLustReaction".Localized());

            if (lustDelta >= 5 && lustDelta < 10)
                OutputText("Scenes.VapulaFight.Teased.ModerateLustReaction".Localized());

            if (lustDelta >= 10)
                OutputText("Scenes.VapulaFight.Teased.HeavyLustReaction".Localized());
// 原版 Monster.hx:1543 applyTease()：显示本次挑逗造成的淫欲增量
            OutputText(" <b>(+" + lustDelta.ToString("0.#") + ")</b>");
        }

        public void RunCheck()
        {

            if (CurrentMonster.HasStatusEffect(StatusEffectIds.RunDisabled))
            {
                OutputText("Scenes.VapulaFight.RunCheck.PitWallsBlockRun".Localized());

                CombatScene.CombatMenu();
            }
            else
                RunCheck();
        }

    }
}