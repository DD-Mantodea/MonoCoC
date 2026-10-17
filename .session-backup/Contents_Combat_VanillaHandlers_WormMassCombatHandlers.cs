using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 WormMass 的两个招式（src/classes/scenes/areas/mountain/WormMass.hx:10-60）。
    public static class WormMassCombatHandlers
    {
        // 原版 StatusEffects.InfestAttempted：C# 的 VanillaStatusEffects 里没有这条，按原版 key 直接建。
        private static readonly Identifier InfestAttempted = new("coc", "infest_attempted");

        // 原版 wormAttack（WormMass.hx:10-29）：闪避成功只出「乱挥」；命中伤害不足 1 时按 1 点算。
        [Register("coc:worm_mass_worm_attack")]
        private static void WormAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.WormMass.WormAttack.TheWormColony".Localized());

                return;
            }

            context.Append("Combat.WormMass.WormAttack.TheWormColony2".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0)
                damage = 1;

            player.HealthChange(-damage);
        }

        // 原版 wormsEntice（WormMass.hx:31-60）：50% 失败；成功且玩家高欲望时，
        // 已中过 InfestAttempted 会直接走 wormsScene.infest1()，否则挂 InfestAttempted。
        [Register("coc:worm_mass_worms_entice")]
        private static void WormsEntice(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (context.Random.Next(2) == 0)
            {
                context.Append((player.Lust100 < 50
                    ? "Combat.WormMass.WormsEntice.TheWormColony"
                    : "Combat.WormMass.WormsEntice.TheWormColony2").Localized());

                return;
            }

            if (player.Lust100 < 50)
            {
                context.Append("Combat.WormMass.WormsEntice.TheWormColony3".Localized());

                player.TakeLustDamage(10 + player.Libido / 20f + player.Corruption / 20f);

                return;
            }

            context.Append("Combat.WormMass.WormsEntice.TheWormColony4".Localized());

            if (player.HasStatusEffect(InfestAttempted))
            {
                context.Append("Combat.WormMass.WormsEntice.StruggleAsYou".Localized());

                SceneInstance<WormsScene>.Get().Infest1();

                return;
            }

            context.Append("Combat.WormMass.WormsEntice.YouStruggleWildly".Localized());

            player.CreateStatusEffect(InfestAttempted);
        }
    }
}
