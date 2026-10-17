using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Owca
{
    [Register<FarmersScene>("coc:farmers")]
    public class FarmersScene : Scene
    {

        public override void Entry()
        {
            PerformCombatAction();
        }

        public void PerformCombatAction()
        {

            Player.CreateStatusEffect(StatusEffectIds.Attacks, 4, 0, 0, 0);

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        public void Defeated(bool hpVictory)
        {

            SceneInstance<OwcaScene>.Get().BeatUpOwca();
        }

        public void Won(bool hpVictory, bool pcCameWorms = false)
        {

            SceneInstance<OwcaScene>.Get().LoseToOwca();
        }

    }
}