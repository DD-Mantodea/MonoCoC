using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4215-4223 counterAbility
    public static class PlayerCounterHandlers
    {
        [Register("coc:has_counter_perk")]
        public static bool HasCounterPerk(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.CounterAB);

        [Register("coc:counter_active")]
        public static bool CounterActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.CounterAB);

        [Register("coc:special_counter")]
        public static void Counter(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            player.CreateStatusEffect(StatusEffectIds.CounterAB, 0, 3, 0, 0, 3);

            if (enemy.Distance == CombatDistance.Melee)
            {
                var damage = CombatCalculator.PhysicalDamage(context, player, enemy);

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);
            }
            else
                context.Append("Combat.Special.Counter.Text".Localized());
        }
    }
}