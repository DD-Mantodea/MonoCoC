using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BazaarGatekeeperCombatHandlers
    {

        [Register("coc:bazaar_gatekeeper_uber")]
        public static bool IsUber(CombatContext context, CombatState state) =>

            context.Enemy.HasStatusEffect(StatusEffectIds.Uber);

        [Register("coc:bazaar_gatekeeper_not_uber")]
        public static bool IsNotUber(CombatContext context, CombatState state) =>

            !context.Enemy.HasStatusEffect(StatusEffectIds.Uber);

        [Register("coc:bazaar_gatekeeper_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
                return;

            var damage = CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Enemy.WeaponAttack);

            if (damage > 0)
                player.HealthChange(-damage);
        }

        [Register("coc:bazaar_gatekeeper_scimitar_special")]
        public static void ScimitarSpecial(CombatContext context, CombatAbilityDefinition ability)
        {
            if (context.Random.Next(3) == 0)
                ScimitarCrossAttack(context, ability);
            else
                ScimitarAttack(context, ability);
        }

        [Register("coc:bazaar_gatekeeper_scimitar_attack")]
        public static void ScimitarAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.BazaarGatekeeper.Scimitar.Opener".Localized());

            if (context.Enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) > 0)
            {
                context.Append("Combat.BazaarGatekeeper.Scimitar.BlindMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = false
            });

            var key = avoid.Outcome switch
            {
                AvoidOutcome.Parry => "Combat.BazaarGatekeeper.Scimitar.Parry",

                AvoidOutcome.Dodge when avoid.DodgeReason is "Evade" or "Misdirection" => "Combat.BazaarGatekeeper.Scimitar.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.BazaarGatekeeper.Scimitar.DodgeFlexibility",

                AvoidOutcome.Dodge => "Combat.BazaarGatekeeper.Scimitar.DodgeSpeed",

                _ => null
            };

            if (key != null)
            {
                context.Append(key.Localized());

                return;
            }

            context.Append("Combat.BazaarGatekeeper.Scimitar.Hit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Enemy.WeaponAttack + 100f);

            player.HealthChange(-damage);
        }

        [Register("coc:bazaar_gatekeeper_scimitar_cross_attack")]
        public static void ScimitarCrossAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (!context.Enemy.HasStatusEffect(StatusEffectIds.Uber))
            {
                context.WriteAction("Combat.BazaarGatekeeper.Cross.Telegraph".Localized());

                context.Enemy.CreateStatusEffect(StatusEffectIds.Uber);

                return;
            }

            if (player.HasWaited())
                context.WriteAction("Combat.BazaarGatekeeper.Cross.Waited".Localized());
            else if (context.Enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) > 0)
                context.WriteAction("Combat.BazaarGatekeeper.Cross.BlindMiss".Localized());
            else
            {
                context.WriteAction("Combat.BazaarGatekeeper.Cross.Hit".Localized());

                var damage = CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Enemy.WeaponAttack + 250f);

                player.HealthChange(-damage);
            }

            context.Enemy.RemoveStatusEffect(StatusEffectIds.Uber);
        }
    }
}