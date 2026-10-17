using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/desert/SandWitch.hx（level 3 沙漠女巫）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/SandWitch.hjson 与 SandWitchCombatHandlers.cs。
    [Register<SandWitchEnemy>("coc:sand_witch")]
    public class SandWitchEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(3f);

            Tallness = 64;

            Body.SetHipsRating("curvy", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "bronzed";

            var hair = Body.Get<HairPart>();

            hair.Color = "sandy-blonde";

            hair.Length = 15;

            WeaponName = "kick";

            ArmorName = "robes";
        }

        // 原版 SandWitch.hx:8 defeated()：先清除石卵状态，再进战败场景
        public override bool Defeated(bool hpVictory)
        {
            var player = CoC.Instance.Player;

            if (player.HasStatusEffect(StatusEffectIds.StoneLust))
                player.RemoveStatusEffect(StatusEffectIds.StoneLust);

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.SandWitchScene>.Get().BeatSandwitch();

            return true;
        }

        // 原版 SandWitch.hx:15 won()
        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Scenes.SandWitch.Won.WitchRetreats".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.SandWitchScene>.Get().SandwitchRape();

            return true;
        }
    }
}