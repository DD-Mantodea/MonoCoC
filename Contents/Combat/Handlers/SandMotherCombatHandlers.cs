using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class SandMotherCombatHandlers
    {
        [Register("coc:sand_mother_earth_shield")]
        public static void EarthShield(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SandMotherEarthShield.Text".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.Earthshield);
        }

        [Register("coc:sand_mother_giga_fire")]
        public static void GigaFire(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (SandMotherEnemy)context.Enemy;

            context.Append("Combat.Ability.SandMotherGigaFire.Text".Localized());

            enemy.PreparingGigaFire = true;

            enemy.GigaHealth = enemy.Health;
        }

        [Register("coc:sand_mother_giga_fire_two")]
        public static void GigaFire2(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (SandMotherEnemy)context.Enemy;

            var interrupted = enemy.Health < enemy.GigaHealth;

            context.Append((interrupted

                ? "Combat.Ability.SandMotherGigaFire2.Interrupted"

                : "Combat.Ability.SandMotherGigaFire2.Unleashed").Localized());

            var damage = 40 + Random.Shared.Next(11);

            if (!interrupted)
                damage *= 3;

            enemy.PreparingGigaFire = false;

            enemy.GigaHealth = 0;

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:sand_mother_telekinesis")]
        public static void Telekinesis(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.SandMotherTelekinesis.Text".Localized());

            var sizeMod = 100f + player.Tallness * (player.Thickness / 100f + 0.5f);

            player.HealthChange(-(20f * sizeMod / 170f));
        }

        [Register("coc:sand_mother_eat_a_lightning_bolt")]
        [Register("coc:sand_mother_eat_alightning_bolt")]
        public static void Lightning(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SandMotherLightning.Text".Localized());

            context.Player.HealthChange(-(30 + Random.Shared.Next(10)));
        }

        [Register("coc:sand_mother_get_whispered")]
        public static void Whispered(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SandMotherWhispered.Text".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.Whispered);

            context.Player.TakeLustDamage(15);
        }
    }
}