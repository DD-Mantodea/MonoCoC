using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{    public static class PhoenixPyroCombatHandlers
    {
        [Register("coc:phoenix_pyro_phoenix_platoon_fire_breath")]
        [Register("coc:phoenix_pyro_phoenix_platoon_fire_breath_two")]
        public static void FireBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixPyroFireBreath.Text".Localized());

            var damage = 60 + Random.Shared.Next(50);

            if (context.Player.Shield != null && context.Player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
            {
                context.Append("Combat.Ability.PhoenixPyroFireBreath.Shield".Localized());

                return;
            }

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:phoenix_pyro_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixPyroAttack.Text".Localized());

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}