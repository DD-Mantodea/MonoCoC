using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/GnollCombatHandlers.cs（已实现，92 行）
    [Register<GnollEnemy>("coc:gnoll")]
    public class GnollEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 4, 3);

            Tallness = 6 * 12;

            Body.Get<SkinPart>().Tone = "tawny";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 22;

            WeaponName = "twisted club";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Plains.GnollScene>.Get().DefeatHyena();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Gnoll.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Plains.GnollScene>.Get().GetRapedByGnoll();

            return true;
        }
    }
}