using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 classes/scenes/areas/mountain/NephilaSlime.hx 的三个招式
    // 文本键沿用既有 Scenes.Nephilaslimescene.*（若缺失则退化为英文常量，见各分支注释）
    public static class NephilaSlimeCombatHandlers
    {
        [Register("coc:nephila_slime_combo")]
        public static void Combo(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.Nephilaslimescene.Combo.Sidestep".Localized());

                player.HealthChange(-(1 + context.Random.Next(10)));

                return;
            }

            context.Append("Scenes.Nephilaslimescene.Combo.Caught".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack) + 25;

            if (damage > 0)
                player.HealthChange(-damage);
        }

        [Register("coc:nephila_slime_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.Nephilaslimescene.Tease.Lead".Localized());

            if (player.Corruption < 40)
            {
                context.Append("Scenes.Nephilaslimescene.Tease.Unnerving".Localized());

                return;
            }

            context.Append("Scenes.Nephilaslimescene.Tease.Aroused".Localized());

            player.TakeLustDamage(30 * player.LustPercent / 100f, true);
        }

        [Register("coc:nephila_slime_goo_blind")]
        public static void GooBlind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Nephilaslimescene.GooBlind.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                // 原版 NephilaSlime.hx:37：playerAvoidDamage 的 customOutput 按闪避原因分 7 种文案。
                // 本次调用 DoBlock=false，所以原版数组里的 [BLOCK] 分支不可达（原版同样为 false）。
                var dodgeKey = avoid.DodgeReason switch
                {
                    "Blind" => "Scenes.Nephilaslimescene.GooBlind.DodgedBlind",

                    "Speed" => "Scenes.Nephilaslimescene.GooBlind.DodgedSpeed",

                    "Evade" => "Scenes.Nephilaslimescene.GooBlind.DodgedEvade",

                    "Misdirection" => "Scenes.Nephilaslimescene.GooBlind.DodgedMisdirection",

                    "Flexibility" => "Scenes.Nephilaslimescene.GooBlind.DodgedFlexibility",

                    _ => "Scenes.Nephilaslimescene.GooBlind.Dodged",
                };

                context.Append(dodgeKey.Localized());

                return;
            }

            context.Append("Scenes.Nephilaslimescene.GooBlind.Hit".Localized());

            player.CreateStatusEffect(StatusEffectIds.Blind, 1 + context.Random.Next(3), 0, 0, 0);

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength / 2f + enemy.WeaponAttack / 2f);

            player.HealthChange(-damage);
        }
    }
}