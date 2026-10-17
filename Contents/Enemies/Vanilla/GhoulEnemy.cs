using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/desert/Ghoul.hx：
    // 初见是土狼，首次被击中后现出幽灵真身；幽灵形态不会接受挑逗（lustVuln = 0）。
    // 招式与 AI 见 Assets/Data/Combat/Ai/Ghoul.hjson 与 GhoulCombatHandlers.cs。
    [Register<GhoulEnemy>("coc:ghoul")]
    public class GhoulEnemy : Enemy
    {
        /// <summary>原版 Ghoul.hx:20 ghoulReveal —— 是否已揭穿真身（用状态承载以便存档与 AI 条件）。</summary>
        public bool GhoulReveal
        {
            get => HasStatusEffect(StatusEffectIds.GhoulReveal);

            set
            {
                if (value)
                    CreateStatusEffect(StatusEffectIds.GhoulReveal, 0, 0, 0, 0);
                else
                    RemoveStatusEffect(StatusEffectIds.GhoulReveal);
            }
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            LustVulnerability = 0f;

            WeaponName = "claws";

            WeaponAttack = Random.Shared.Next(4) + 2;

            ArmorName = "ethereal hide";

            ArmorDefense = Random.Shared.Next(5) + 2;

            Gems = Random.Shared.Next(25) + 5;

            Body.Get<SkinPart>().Tone = "ghostly";
        }

        // 原版 Ghoul.hx:29 react(WhenAttacked)：第一次被击中时揭穿真身
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.WhenAttacked && !GhoulReveal)
            {
                context.Append("Scenes.Ghoul.React.IllusionDispelled".Localized());

                context.Append("Scenes.Ghoul.React.IllusionWoreOff".Localized());

                GhoulReveal = true;
            }

            return true;
        }

        // 原版 Ghoul.hx:108 defeated()：化为沙尘消散，走默认战后清理
        public override bool Defeated(bool hpVictory)
        {
            Scenes.Scene.OutputText("Scenes.Ghoul.Defeated.ScreechVanishes".Localized());

            GhoulReveal = false;

            return false;
        }

        // 原版 Ghoul.hx:115 won()
        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Scenes.Ghoul.Won.DisgustedVanishes".Localized());

                GhoulReveal = false;

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Desert.GhoulScene>.Get().GhoulWon();

            GhoulReveal = false;

            return true;
        }
    }
}