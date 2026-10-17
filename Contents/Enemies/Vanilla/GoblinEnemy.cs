using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<GoblinEnemy>("coc:goblin")]
    public class GoblinEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            ConfigureBody();
        }

        protected virtual void ConfigureBody()
        {
            CreateVagina(false, 4f, 1);

            CreateBreastRow(4.5f);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);
        }

        // 原版 Goblin.hx:174 defeated() -> goblinScene.gobboRapeIntro()（C# 已移植为 GoblinScene.Victory）
        public override bool Defeated(bool hpVictory)
        {
            Scenes.Vanilla.Areas.Forest.GoblinScene.Victory();

            return true;
        }
    }
}