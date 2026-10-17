using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    [Register<LumberjackEnemy>("coc:lumberjack")]
    public class LumberjackEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12f, 1.75f, new MonoCoC.Registry.Identifier("coc", "demon"));

            Balls = 2;

            BallSize = 2;

            CreateBreastRow(0f);
        }
    }
}