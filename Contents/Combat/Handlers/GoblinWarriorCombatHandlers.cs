using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GoblinWarriorCombatHandlers
    {
        [Register("coc:goblin_warrior_slash")]
        public static void Slash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.GoblinWarrior.Slash.SwordCharge".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = true, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.GoblinWarrior.Slash.SlashAvoided".Localized());

                return;
            }

            context.Append("Scenes.GoblinWarrior.Slash.SlashHit".Localized());

            var damage = enemy.Strength + enemy.WeaponAttack + Random.Shared.Next(40);

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:goblin_warrior_shield_bash")]
        public static void ShieldBash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.GoblinWarrior.ShieldBash.ShieldCharge".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.GoblinWarrior.ShieldBash.ShieldBashAvoided".Localized());

                return;
            }

            context.Append("Scenes.GoblinWarrior.ShieldBash.ShieldBashHit".Localized());

            if (player.Stun(1, 40))
                context.Append("Scenes.GoblinWarrior.ShieldBash.ConcussionStun".Localized());

            var damage = enemy.Strength + Random.Shared.Next(10);

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:goblin_warrior_special")]
        public static void Special(CombatContext context, CombatAbilityDefinition ability)
        {
            if (Random.Shared.Next(2) == 0)
                Slash(context, ability);
            else
                ShieldBash(context, ability);
        }
    }
}