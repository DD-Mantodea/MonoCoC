using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Scenes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/FrogGirlCombatHandlers.cs（已实现，86 行）
    [Register<FrogGirlEnemy>("coc:frog_girl")]
    public class FrogGirlEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 3);

            Tallness = 62;

            Body.Get<SkinPart>().Tone = "light green";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 10;

            WeaponName = "powerful tongue";

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