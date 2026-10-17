using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ValkyrieCombatHandlers
    {

        [Register("coc:valkyrie_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:valkyrie_spear_attack")]
        public static void SpearAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Valkyrie.SpearAttack.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = false
            });

            var dodgeKey = avoid.Outcome switch
            {
                AvoidOutcome.Parry => "Combat.Valkyrie.SpearAttack.Parry",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Evade" => "Combat.Valkyrie.SpearAttack.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.Valkyrie.SpearAttack.DodgeFlexibility",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.Valkyrie.SpearAttack.DodgeMisdirection",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Blind" => "Combat.Valkyrie.SpearAttack.DodgeBlind",

                AvoidOutcome.Dodge => "Combat.Valkyrie.SpearAttack.DodgeSpeed",

                _ => null
            };

            if (dodgeKey != null)
            {
                context.Append(dodgeKey.Localized());

                return;
            }

            context.Append("Combat.Valkyrie.SpearAttack.Hit".Localized());

            var strikes = 1 + context.Random.Next(3);

            var damage = 0f;

            for (var i = 0; i < strikes; i++)
                damage += CombatCalculator.ReduceDamage(context, player, enemy.Strength + context.Random.Next(50));

            player.HealthChange(-damage);
        }

        [Register("coc:valkyrie_shield_bash")]
        public static void ShieldBash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Valkyrie.ShieldBash.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            var dodgeKey = avoid.Outcome switch
            {
                AvoidOutcome.Dodge when avoid.DodgeReason == "Evade" => "Combat.Valkyrie.ShieldBash.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.Valkyrie.ShieldBash.DodgeFlexibility",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.Valkyrie.ShieldBash.DodgeMisdirection",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Blind" => "Combat.Valkyrie.ShieldBash.DodgeBlind",

                AvoidOutcome.Dodge => "Combat.Valkyrie.ShieldBash.DodgeSpeed",

                _ => null
            };

            if (dodgeKey != null)
            {
                context.Append(dodgeKey.Localized());

                return;
            }

            context.Append("Combat.Valkyrie.ShieldBash.HitLead".Localized());

            context.Append(player.Stun(0, 33)

                ? "Combat.Valkyrie.ShieldBash.Stunned".Localized()

                : "Combat.Valkyrie.ShieldBash.NotStunned".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 50 + context.Random.Next(50));

            player.HealthChange(-damage);
        }

        [Register("coc:valkyrie_aerial_rave")]
        public static void AerialRave(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (context.Random.Next(2) == 0 || player.CanFly())
            {
                SpearAttack(context, ability);

                return;
            }

            context.Append("Combat.Valkyrie.AerialRave.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            var dodgeKey = avoid.Outcome switch
            {
                AvoidOutcome.Dodge when avoid.DodgeReason == "Evade" => "Combat.Valkyrie.AerialRave.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.Valkyrie.AerialRave.DodgeFlexibility",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.Valkyrie.AerialRave.DodgeMisdirection",

                AvoidOutcome.Dodge => "Combat.Valkyrie.AerialRave.DodgeSpeed",

                _ => null
            };

            if (dodgeKey != null)
            {
                context.Append(dodgeKey.Localized());

                return;
            }

            context.Append("Combat.Valkyrie.AerialRave.Hit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 200 + context.Random.Next(100));

            player.HealthChange(-damage);
        }
    }
}