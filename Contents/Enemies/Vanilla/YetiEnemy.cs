using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/YetiCombatHandlers.cs（已实现，165 行）
    [Register<YetiEnemy>("coc:yeti")]
    public class YetiEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12, 1.5f, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 2;

            Tallness = 8 * 12;

            Body.Get<SkinPart>().Tone = "light";

            var hair = Body.Get<HairPart>();

            hair.Color = "white";

            hair.Length = 8;

            WeaponName = "fists";

            ArmorName = "thick fur";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.YetiScene>.Get().WinAgainstYeti();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.YetiScene>.Get().LoseToYeti();

            return true;
        }
    }
}