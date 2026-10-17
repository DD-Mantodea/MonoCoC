using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/FetishCultistCombatHandlers.cs（已实现，125 行）
    [Register<FetishCultistEnemy>("coc:fetish_cultist")]
    public class FetishCultistEnemy : Enemy
    {

        public const string SWIMSUIT = "swimsuit";

        public const string PERVY_NUNS_CLOTHING = "pervy nun's clothing";

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 2);

            Tallness = 5 * 12 + 7;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "whip";
        }

    }
}