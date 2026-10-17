using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/camp/ScarredBlade.hx（level 17 被诅咒的活剑）。
    // 非生物：无性别、无球、不会接受挑逗（lustVuln = 0）。
    [Register<ScarredBladeEnemy>("coc:scarred_blade")]
    public class ScarredBladeEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(1f);

            Balls = 0;

            BallSize = 0;

            Tallness = 36;

            Body.Get<SkinPart>().Tone = "metallic";

            WeaponName = "scarred blade";

            ArmorName = "lethicite";

            LustVulnerability = 0f;
        }

        // 原版 ScarredBlade.hx:6 won() -> sheilaScene.badEndScarredBlade()
        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.SheilaScene>.Get().BadEndScarredBlade();

            return true;
        }

        // 原版 ScarredBlade.hx:10 defeated() -> sheilaScene.breakScarredBlade()
        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Npcs.SheilaScene>.Get().BreakScarredBlade();

            return true;
        }
    }
}