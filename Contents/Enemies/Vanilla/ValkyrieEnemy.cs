using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/ValkyrieCombatHandlers.cs（已实现，168 行）
    [Register<ValkyrieEnemy>("coc:valkyrie")]
    public class ValkyrieEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 1);

            Tallness = 7 * 12;

            Body.Get<SkinPart>().Tone = "light";

            var hair = Body.Get<HairPart>();

            hair.Color = "white";

            hair.Length = 12;

            WeaponName = "spear and shield";

            ArmorName = "bronze plates";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.ValkyrieScene>.Get().WinAgainstValkyrie(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.ValkyrieScene>.Get().LoseToValkyrie();

            return true;
        }
    }
}