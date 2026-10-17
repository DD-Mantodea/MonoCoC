using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<KitsuneEnemy>("coc:kitsune")]
    public class KitsuneEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var id = definition.Id.Path;

            var color = id.Contains("black", System.StringComparison.OrdinalIgnoreCase) ? "black"

                : id.Contains("red", System.StringComparison.OrdinalIgnoreCase) ? "red" : "blonde";

            if (color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) == 1)
            {
                CreateCock(14, 2, BodyPartTypes.Human);

                Balls = 2;

                BallSize = 4;

                CumMultiplier = 1.5f;
            }

            CreateVagina(false, 4, 0);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 20);

            CreateBreastRow(5);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 20);

            Tallness = 66;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Color = color;

            hair.Length = 16;

            Body.Get<TailPart>().Id = BodyPartTypes.Fox;

            WeaponName = "claws";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<KitsuneScene>.Get().DefeatTheKitsunes();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                Scene.OutputText("Combat.Kitsune.CameWorms".Localized());

                CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<KitsuneScene>.Get().LoseToKitsunes();

            return true;
        }
    }
}