using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ZetazCombatHandlers
    {

        [Register("coc:zetaz_potion")]
        public static void Potion(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (ZetazEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.ZetazPotion.Throw".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoFatigue = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Ability.ZetazPotion.Dodged".Localized());

                return;
            }

            context.Append("Combat.Ability.ZetazPotion.Coated".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.TemporaryHeat))
                player.CreateStatusEffect(StatusEffectIds.TemporaryHeat, 0, 1, 0, 0);
        }

        [Register("coc:zetaz_gust")]
        public static void Gust(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.ZetazGust.Text".Localized());

            if (player.Toughness <= 60)
                player.HealthChange(-(1 + Random.Shared.Next(6)));

            player.CreateStatusEffect(StatusEffectIds.Blind, 1, 0, 0, 0);
        }

        [Register("coc:zetaz_giga_arouse")]
        public static void GigaArouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.ZetazGigaArouse.Text".Localized());

            player.TakeLustDamage(15 + Random.Shared.Next(Math.Max(1, (int)(player.Libido / 10))) + player.Corruption / 10f);
        }

        [Register("coc:zetaz_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:zetaz_drain")]
        public static void Drain(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (ZetazEnemy)context.Enemy;

            context.Append("Combat.Ability.ZetazDrain.Text".Localized());

            enemy.Health += enemy.MaxHealth * 0.25f;

            enemy.Lust = Math.Max(0, enemy.Lust - 20);

            context.Player.TakeLustDamage(2);
        }
    }
}