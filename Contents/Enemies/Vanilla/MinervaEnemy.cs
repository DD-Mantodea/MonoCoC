using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/MinervaCombatHandlers.cs（已实现，243 行）
    [Register<MinervaEnemy>("coc:minerva")]
    public class MinervaEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(16, 3);

            CreateCock(16, 3);

            Balls = 2;

            BallSize = 3;

            CreateVagina(false, 3, 2);

            Tallness = 8 * 12 + 4;

            Body.Get<SkinPart>().Tone = "blue";

            var hair = Body.Get<HairPart>();

            hair.Color = "red";

            hair.Length = 25;

            WeaponName = "halberd";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.MinervaScene>.Get().BeatUpDatSharpie();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.MinervaScene>.Get().LoseToMinerva();

            return true;
        }
    }
}