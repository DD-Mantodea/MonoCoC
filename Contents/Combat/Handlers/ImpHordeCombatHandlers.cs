using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ImpHordeCombatHandlers
    {

        [Register("coc:imp_horde_claw")]
        public static void Claw(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (ImpHordeEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.ImpHordeClaw.Wave".Localized());

            var hits = 2 + Random.Shared.Next(5);

            for (var index = 0; index < hits; index++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false,

                    DoFatigue = false
                });

                if (avoid.Outcome == AvoidOutcome.Dodge)
                    continue;

                context.Append(index % 2 == 0

                    ? "Combat.Ability.ImpHordeClaw.HitA".Localized()

                    : "Combat.Ability.ImpHordeClaw.HitB".Localized());

                var damage = 20 - Random.Shared.Next(Math.Max(1, (int)(player.Toughness / 10)));

                player.HealthChange(-Math.Max(1, damage));
            }

            LegHump(context);
        }

        [Register("coc:imp_horde_bukkake")]
        public static void Bukkake(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (ImpHordeEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.ImpHordeBukkake.Start".Localized());

            var hits = 2 + Random.Shared.Next(5);

            for (var index = 0; index < hits; index++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false,

                    DoFatigue = false
                });

                if (avoid.Outcome != AvoidOutcome.Dodge)
                {
                    context.Append(index % 2 == 0

                        ? "Combat.Ability.ImpHordeBukkake.HitA".Localized()

                        : "Combat.Ability.ImpHordeBukkake.HitB".Localized());

                    player.TakeLustDamage(7 + player.Libido / 40f + player.Corruption / 40f);
                }

                enemy.Lust = Math.Max(0, enemy.Lust - 5);
            }

            LegHump(context);
        }

        [Register("coc:imp_horde_uber")]
        public static void Uber(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (ImpHordeEnemy)context.Enemy;

            if (!enemy.UberCharged)
            {
                context.Append("Combat.Ability.ImpHordeUber.Charging".Localized());

                enemy.UberCharged = true;

                return;
            }

            context.Append("Combat.Ability.ImpHordeUber.Burst".Localized());

            context.Player.TakeLustDamage(50);

            enemy.UberCharged = false;

            LegHump(context);
        }

        public static void LegHump(CombatContext context)
        {
            if (Random.Shared.Next(2) != 0)
                return;

            context.Append("Combat.Ability.ImpHorde.LegHump".Localized());

            context.Player.TakeLustDamage(1);
        }
    }
}