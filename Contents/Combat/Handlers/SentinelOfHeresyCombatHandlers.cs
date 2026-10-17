using MonoCoC.Attributes;
using System;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class SentinelOfHeresyCombatHandlers
    {
        [Register("coc:sentinel_of_heresy_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:sentinel_of_heresy_seal_tease")]
        public static void SealTease(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SentinelOfHeresySealTease.Text".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.SentinelNoTease, 3, 0, 0, 0);

            context.Enemy.Fatigue += 15;
        }

        [Register("coc:sentinel_of_heresy_martyrdom")]
        public static void Martyrdom(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SentinelOfHeresyMartyrdom.Text".Localized());

            foreach (var ally in context.Enemies)
            {
                if (ally == context.Enemy || ally.Health <= 0)
                    continue;

                var healed = ally.MaxHealth * (1 + Random.Shared.Next(2)) * 0.1f;

                ally.Health = Math.Min(ally.MaxHealth, ally.Health + healed);

                context.Append("Combat.Ability.SentinelOfHeresyMartyrdom.Healed".Localized(ally.ShortName, (int)healed));
            }

            var damaged = 40 + Random.Shared.Next(40);

            context.Enemy.HealthChange(-damaged);

            context.Append("Combat.Ability.SentinelOfHeresyMartyrdom.SelfDamage".Localized(damaged));
        }
    }
}