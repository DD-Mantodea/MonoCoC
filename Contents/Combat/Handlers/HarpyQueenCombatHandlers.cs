using MonoCoC.Attributes;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class HarpyQueenCombatHandlers
    {
        [Register("coc:harpy_queen_eldritch_ropes")]
        public static void EldritchRopes(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.HarpyQueenEldritchRopes.Text".Localized());

            context.Player.HealthChange(-(25 + Random.Shared.Next(10)));
        }

        [Register("coc:harpy_queen_lust_spike_attack")]
        public static void LustSpikeAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.HarpyQueenLustSpikeAttack.Text".Localized());

            context.Player.TakeLustDamage(40);
        }

        [Register("coc:harpy_queen_wind_slam_attack")]
        public static void WindSlamAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.HarpyQueenWindSlamAttack.Text".Localized());

            context.Player.HealthChange(-(100 + Random.Shared.Next(50)));
        }
    }
}