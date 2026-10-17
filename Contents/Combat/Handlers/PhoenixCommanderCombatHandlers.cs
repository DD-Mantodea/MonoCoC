using MonoCoC.Attributes;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.Handlers
{    public static class PhoenixCommanderCombatHandlers
    {
        [Register("coc:phoenix_commander_order_grenadier")]
        public static void OrderGrenadier(CombatContext context, CombatAbilityDefinition ability) =>

            context.Append("Combat.Ability.PhoenixCommanderOrderGrenadier.Text".Localized());

        [Register("coc:phoenix_commander_order_pyro")]
        public static void OrderPyro(CombatContext context, CombatAbilityDefinition ability) =>

            context.Append("Combat.Ability.PhoenixCommanderOrderPyro.Text".Localized());

        [Register("coc:phoenix_commander_order_sapper")]
        public static void OrderSapper(CombatContext context, CombatAbilityDefinition ability) =>

            context.Append("Combat.Ability.PhoenixCommanderOrderSapper.Text".Localized());

        [Register("coc:phoenix_commander_protect")]
        public static void Protect(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixCommanderProtect.Text".Localized());

            context.Enemy.ArmorDefense = 70;

            ((Enemies.Vanilla.PhoenixCommanderEnemy)context.Enemy).ShieldWall = true;
        }

        [Register("coc:phoenix_commander_inspire")]
        public static void Inspire(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixCommanderInspire.Text".Localized());

            context.Enemy.Fatigue += 15;

            foreach (var ally in context.Enemies.Where(e => e.Health > 0))
            {
                ally.Fatigue = Math.Max(0, ally.Fatigue - 15);

                if (ally.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Stunned) && Random.Shared.Next(2) == 0)
                    ally.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Stunned);

                if (ally.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Fear) && Random.Shared.Next(2) == 0)
                    ally.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Fear);

                if (ally.HasStatusEffect(Datas.Vanilla.StatusEffectIds.Whispered) && Random.Shared.Next(2) == 0)
                    ally.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.Whispered);

                ally.Health = Math.Min(ally.MaxHealth, ally.Health + ally.MaxHealth * 0.15f);
            }
        }

        [Register("coc:phoenix_commander_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixCommanderAttack.Text".Localized());

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}