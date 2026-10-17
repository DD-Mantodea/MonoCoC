using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AikoEnemy>("coc:aiko")]
    public class AikoEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var corrupt = definition.Id.Path.Contains("corrupt", System.StringComparison.OrdinalIgnoreCase);

            CreateVagina(false, 1, 0);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 200);

            CreateBreastRow(5);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 40);

            Tallness = 64;

            Body.Get<SkinPart>().Tone = "light tan";

            var hair = Body.Get<HairPart>();

            hair.Color = "silver-blonde";

            hair.Length = 10;

            Body.Get<TailPart>().Id = BodyPartTypes.Fox;

            Body.Get<EarsPart>().Id = BodyPartTypes.Fox;

            WeaponName = corrupt ? "bill-hook hatchet" : "longbow";

            ArmorName = corrupt ? "ragged, bloodied robes" : "revealing blue and white robes";

            ArmorDefense = 16;
        }

        public override bool Defeated(bool hpVictory)
        {
            if (CoC.Instance.Player.HasStatusEffect(StatusEffectIds.Spar))
                SceneInstance<AikoScene>.Get().SparWithAikoWin();
            else
                SceneInstance<AikoScene>.Get().AikoLosesIntro();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.HasStatusEffect(StatusEffectIds.Spar))
                SceneInstance<AikoScene>.Get().SparWithAikoLose((int)CoC.Instance.Player.LustPercent);
            else
                SceneInstance<AikoScene>.Get().AikoWinsIntro();

            return true;
        }
    }
}