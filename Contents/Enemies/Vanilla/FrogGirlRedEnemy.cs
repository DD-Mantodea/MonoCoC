using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Scenes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/FrogGirlRedCombatHandlers.cs（已实现，81 行）
    [Register<FrogGirlRedEnemy>("coc:frog_girl_red")]
    public class FrogGirlRedEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 3);

            Tallness = 62;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "dark red";

            hair.Length = 10;

            WeaponName = "tongue";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<Scenes.Vanilla.Areas.Bog.FrogGirlScene>.Get().SuwakoDefeated(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<Scenes.Vanilla.Areas.Bog.FrogGirlScene>.Get().WonHook(hpVictory);

            return true;
        }
    }
}