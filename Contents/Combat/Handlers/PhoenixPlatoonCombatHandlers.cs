using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PhoenixPlatoonCombatHandlers
    {

        [Register("coc:phoenix_platoon_rush")]
        public static void Rush(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.PhoenixPlatoon.PhoenixPlatoonRush.YouFallBack".Localized());

            var attacks = 2 + context.Random.Next(3);

            for (var i = 0; i < attacks; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:phoenix_platoon_fire_breath")]
        public static void FireBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.PhoenixPlatoon.PhoenixPlatoonFireBreath.SuddenlyTheShield".Localized());

            var damage = 100 + context.Random.Next(50);

            context.Player.TakeDamage(damage);
        }

        [Register("coc:phoenix_platoon_lustbang")]
        public static void Lustbang(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.PhoenixPlatoon.PhoenixPlatoonLustbang.OneOfThe".Localized());

            context.Player.TakeLustDamage(40, true);
        }
    }
}