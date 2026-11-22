using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class SatyrCombatHandlers
    {

        [Register("coc:satyr_attack")]
        public static void SatyrAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Satyr.Attack.Lead".Localized());

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.AppendLog("Combat.Satyr.Attack.Blind".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.AppendLog("Combat.Satyr.Attack.Miss".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage > 0)
            {
                context.AppendLog("Combat.Satyr.Attack.Hit".Localized());

                player.HealthChange(-damage);
            }
            else
                context.AppendLog("Combat.Satyr.Attack.Blocked".Localized());
        }

        [Register("coc:satyr_bate")]
        public static void SatyrBate(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Satyr.Bate.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + player.Libido / 5 + 4);

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);
        }

        [Register("coc:satyr_charge")]
        public static void SatyrCharge(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Satyr.Charge.Lead".Localized());

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.AppendLog("Combat.Satyr.Charge.Blind".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            var dodgeKey = avoid.Outcome switch
            {
                AvoidOutcome.Dodge when avoid.DodgeReason == "Evade" => "Combat.Satyr.Charge.DodgeEvade",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.Satyr.Charge.DodgeFlexibility",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.Satyr.Charge.DodgeMisdirection",

                AvoidOutcome.Dodge => "Combat.Satyr.Charge.DodgeSpeed",

                _ => null
            };

            if (dodgeKey != null)
            {
                context.AppendLog(dodgeKey.Localized());

                enemy.HealthChange(-5);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage > 0)
            {
                context.AppendLog("Combat.Satyr.Charge.Hit".Localized());

                if (player.Stun(0, 33))
                    context.AppendLog("Combat.Satyr.Charge.Stunned".Localized());

                player.HealthChange(-damage);
            }
            else
                context.AppendLog("Combat.Satyr.Charge.Deflected".Localized());
        }

        [Register("coc:satyr_bottle_chug")]
        public static void BottleChug(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Satyr.BottleChug.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + player.Libido / 5);

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);
        }

        [Register("coc:satyr_high_lust_chug_rape")]
        public static void HighLustChugRape(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Satyr.HighLust.Lead".Localized());

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.AppendLog("Combat.Satyr.HighLust.Blind".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.AppendLog("Combat.Satyr.HighLust.Dodged".Localized());

                return;
            }

            context.AppendLog("Combat.Satyr.HighLust.Hit".Localized());

            enemy.Lust = Math.Max(0, enemy.Lust - 50);

            player.Lust = Math.Min(player.MaxLust, player.Lust + player.Sensitivity / 5 + 20);
        }
    }
}