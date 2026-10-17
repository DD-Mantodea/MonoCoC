using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class PhoenixSapperCombatHandlers
    {
        [Register("coc:phoenix_sapper_armor_rend")]
        [Register("coc:phoenix_sapper_armor_rend_two")]
        public static void ArmorRend(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (PhoenixSapperEnemy)context.Enemy;

            var player = context.Player;

            enemy.Fatigue += 15;

            context.Append("Combat.Ability.PhoenixSapperArmorRend.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = true, DoBlock = true, DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Ability.PhoenixSapperArmorRend.Dodged".Localized());

                return;
            }

            if (avoid.Outcome is AvoidOutcome.Block or AvoidOutcome.Parry)
            {
                context.Append("Combat.Ability.PhoenixSapperArmorRend.Deflected".Localized());

                return;
            }

            context.Append("Combat.Ability.PhoenixSapperArmorRend.Hit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, Random.Shared.Next(25) + enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            if (player.HasStatusEffect(StatusEffectIds.ArmorRent))
                player.ChangeStatusValue(StatusEffectIds.ArmorRent, 0, 5);
            else
                player.CreateStatusEffect(StatusEffectIds.ArmorRent, 5, 0, 0, 0);
        }

        [Register("coc:phoenix_sapper_flash_bang")]
        [Register("coc:phoenix_sapper_flash_bang_two")]
        public static void FlashBang(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (PhoenixSapperEnemy)context.Enemy;

            enemy.Fatigue += 15;

            context.Append("Combat.Ability.PhoenixSapperFlashBang.Text".Localized());

            if (Random.Shared.Next(3) != 0)
                context.Player.CreateStatusEffect(StatusEffectIds.Blind, 2 + Random.Shared.Next(3), 0, 0, 0);
            else
                context.Append("Combat.Ability.PhoenixSapperFlashBang.Covered".Localized());
        }

        [Register("coc:phoenix_sapper_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.PhoenixSapperAttack.Text".Localized());

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}