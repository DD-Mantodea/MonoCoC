using System;
using MonoCoC.Extensions;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class LumberjackCombatHandlers
    {

        [Register("coc:lumberjack_chop")]
        public static void Chop(CombatContext context, CombatAbilityDefinition ability)
        {
            var variant = context.Random.Next(2);

            context.WriteAction((variant == 0

                ? "Combat.Lumberjack.Chop1"

                : "Combat.Lumberjack.Chop2").Localized());

            var result = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (result.Outcome == AvoidOutcome.Dodge)
            {
                context.WriteAction((variant == 0

                    ? "Combat.Lumberjack.Chop1Miss"

                    : "Combat.Lumberjack.Chop2Miss").Localized());

                return;
            }

            if (result.Outcome == AvoidOutcome.Block)
            {
                context.WriteAction((variant == 0

                    ? "Combat.Lumberjack.Chop1Block"

                    : "Combat.Lumberjack.Chop2Block").Localized());

                return;
            }

            var damage = CombatCalculator.PhysicalDamage(context, context.Enemy, context.Player, 5);

            context.Player.HealthChange(-damage);

            context.WriteAction((variant == 0

                ? "Combat.Lumberjack.Chop1Hit"

                : "Combat.Lumberjack.Chop2Hit").Localized());
        }

        [Register("coc:lumberjack_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Lumberjack.Arouse".Localized());

            var lust = 10 + context.Random.Next(6);

            context.Player.Lust = Math.Min(context.Player.MaxLust, context.Player.Lust + lust);

            context.WriteAction("Combat.Lumberjack.ArouseLust".Localized(lust));
        }
    }
}