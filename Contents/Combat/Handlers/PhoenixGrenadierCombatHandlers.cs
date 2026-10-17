using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{    public static class PhoenixGrenadierCombatHandlers
    {
        [Register("coc:phoenix_grenadier_phoenix_platoon_lustbang")]
        [Register("coc:phoenix_grenadier_phoenix_platoon_lustbang_two")]
        public static void Lustbang(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixGrenadierLustbang.Text".Localized());

            context.Player.TakeLustDamage(23);
        }

        [Register("coc:phoenix_grenadier_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixGrenadierAttack.Text".Localized());

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}