using MonoCoC.Contents.Combat;
using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/desert/OasisShaman.hx（level 6 绿洲恶魔萨满）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/OasisShaman.hjson 与 OasisShamanCombatHandlers.cs。
    // 原版 OasisShaman.hx:47-58：被美人鱼之歌魅惑时的三个阶段反应
    // 施加 Chanted 的 mermaidChant() 招式已在 MermaidChantCombatHandlers 中移植（CombatAbilities.hx:3769）。
    // 该分支随护盾/魅惑状态生效而自动触发。
    [Register<OasisShamanEnemy>("coc:oasis_shaman")]
    public class OasisShamanEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(8f, 2f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 4;

            CreateBreastRow(2f);

            Tallness = 72;

            Body.SetHipsRating("curvy", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "greyish-tan";

            var hair = Body.Get<HairPart>();

            hair.Color = "sandy-blonde";

            hair.Length = 15;

            WeaponName = "claws";

            ArmorName = "skin";
        }

        /// <summary>原版 OasisShaman.hx:51 heardChant —— 是否已经对歌声做过惊讶反应。</summary>
        public bool HeardChant;

        // 原版 OasisShaman.hx:47-58：被 Chanted 魅惑时的三段反应
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (!HasStatusEffect(StatusEffectIds.Chanted))
                return false;

            // 速度被压到 1 以下：直接发呆，不行动
            if (Speed <= 1)
            {
                context.Append("Scenes.Oasisshaman.PerformCombatAction.DazedFloatingDemon".Localized());

                return true;
            }

            if (!HeardChant)
            {
                context.Append("Scenes.Oasisshaman.PerformCombatAction.ChantSurpriseSquirm".Localized());

                context.Append("Scenes.Oasisshaman.PerformCombatAction.VoiceHowDoingThat".Localized());

                HeardChant = true;

                return true;
            }

            context.Append("Scenes.Oasisshaman.PerformCombatAction.BeautyCloseYetFar".Localized());

            return false;
        }

        // 原版 OasisShaman.hx:12 defeated() -> winCombatOasisShaman(hpVictory)
        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.OasisShamanScene>.Get().WinCombatOasisShaman(hpVictory);

            return true;
        }

        // 原版 OasisShaman.hx:18 won() -> lostToOasisShaman()
        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.OasisShamanScene>.Get().LostToOasisShaman();

            return true;
        }
    }
}