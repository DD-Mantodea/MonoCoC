using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/GoblinElderCombatHandlers.cs（已实现，77 行）
    [Register<GoblinElderEnemy>("coc:goblin_elder")]
    public class GoblinElderEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 2);

            Tallness = 48;

            Body.Get<SkinPart>().Tone = "yellowish-green";

            var hair = Body.Get<HairPart>();

            hair.Color = "dark green";

            hair.Length = 4;

            WeaponName = "primal sword";

            ArmorName = "bone armor";

            WeaponAttack = 25;

            ArmorDefense = 50;

            ShieldBlock = 20;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<PriscillaScene>.Get().GoblinElderRapeIntro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.IsGenderless())
            {
                Scene.OutputText("Scenes.GoblinElder.Won.KickedUnconscious".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<PriscillaScene>.Get().GoblinElderBeatYaUp();

            return true;
        }
    }
}