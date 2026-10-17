using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<GoblinWarriorEnemy>("coc:goblin_warrior")]
    public class GoblinWarriorEnemy : GoblinEnemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 1);

            CreateBreastRow(5);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 47;

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();

            hair.Color = "red";

            hair.Length = 4;

            WeaponName = "sword and shield";

            WeaponAttack = 14;

            ArmorName = "platemail";

            ArmorDefense = 30;

            ShieldBlock = 10;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<GoblinWarriorScene>.Get().GoblinWarriorRapeIntro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.IsGenderless())
            {
                Scene.OutputText("Scenes.Goblinwarriorscene.Won.KickedUnconscious".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<GoblinWarriorScene>.Get().GobboWarriorBeatYaUp();

            return true;
        }
    }
}