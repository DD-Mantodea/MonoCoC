using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/CorruptedDriderCombatHandlers.cs（已实现，231 行）
    [Register<CorruptedDriderEnemy>("coc:corrupted_drider")]
    public class CorruptedDriderEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(9, 2, BodyPartTypes.Demon);

            CreateVagina(false, 4, 5);

            Tallness = 10 * 12;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Length = 24;

            WeaponName = "claws";

            ArmorName = "carapace";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.CorruptedDriderScene>.Get().DefeatDriderIntro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.CorruptedDrider.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.CorruptedDriderScene>.Get().LoseToDrider();

            return true;
        }
    }
}