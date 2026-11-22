using MonoCoC.Attributes;
using System;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class SentinelOfApostasyCombatHandlers
    {
        [Register("coc:sentinel_of_apostasy_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:sentinel_of_apostasy_seal_physical")]
        public static void SealPhysical(CombatContext context, CombatAbilityDefinition ability)
        {
            context.AppendLog("Combat.Ability.SentinelOfApostasySealPhysical.Text".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.SentinelPhysicalDisabled, 3, 0, 0, 0);

            context.Enemy.Fatigue += 15;
        }

        [Register("coc:sentinel_of_apostasy_piercing_strike")]
        public static void PiercingStrike(CombatContext context, CombatAbilityDefinition ability)
        {
            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = true, DoFatigue = true
            });

            context.AppendLog("Combat.Ability.SentinelOfApostasyPiercingStrike.Text".Localized());

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.AppendLog("Combat.Ability.SentinelOfApostasyPiercingStrike.Dodged".Localized());
            else if (avoid.Outcome == AvoidOutcome.Block)
                context.AppendLog("Combat.Ability.SentinelOfApostasyPiercingStrike.Blocked".Localized());
            else
            {
                context.AppendLog("Combat.Ability.SentinelOfApostasyPiercingStrike.Hit".Localized());

                context.Player.HealthChange(-(100 + Random.Shared.Next(30)));
            }

            context.Enemy.Fatigue += 10;
        }
    }
}