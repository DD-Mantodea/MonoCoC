using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<PriscillaEnemy>("coc:priscilla")]
    public class PriscillaEnemy : GoblinEnemy
    {
        protected override void ConfigureBody()
        {
            CreateVagina(false, 4, 2);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 40);

            CreateBreastRow(8);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 30);

            Tallness = 48;

            Body.Get<SkinPart>().Tone = "yellowish-green";

            var hair = Body.Get<HairPart>();

            hair.Color = "dark green";

            hair.Length = 4;

            WeaponName = "primal sword";

            WeaponAttack = 25;

            ArmorName = "bone armor";

            ArmorDefense = 50;

            ShieldBlock = 25;
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
                Scene.OutputText("Scenes.Priscilla.Won.KickedUnconscious".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<PriscillaScene>.Get().GoblinElderBeatYaUp();

            return true;
        }
    }
}