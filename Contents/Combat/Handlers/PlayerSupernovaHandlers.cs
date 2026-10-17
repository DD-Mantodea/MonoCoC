using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:6617-6663 tfSupernova / tfSupercharge / tfSupernovaExec
    public static class PlayerSupernovaHandlers
    {
        [Register("coc:has_terra_star")]
        public static bool HasTerraStar(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.TFTerraStar);

        [Register("coc:tf_supernova")]
        public static void Supernova(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Terra.Supernova.Start".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.TFSupercharging, 0, 0, 0, 0, 99);
        }

        public static void GatherEnergy(CombatContext context)
        {
            var player = context.Player;

            var charging = player.StatusEffectByType(StatusEffectIds.TFSupercharging);

            if (charging == null)
                return;

            if (charging.Values[0] >= 2)
            {
                Execute(context);

                return;
            }

            context.Append("Combat.Terra.Supernova.Gather".Localized());

            player.AddStatusValue(StatusEffectIds.TFSupercharging, 0, 1);
        }

        private static void Execute(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var cooldown = player.StatusEffectByType(StatusEffectIds.TFTerraStarCooldown);

            cooldown?.Duration = 120;

            player.TerraStarCooldownUntil = Scenes.Scene.View.Time.TotalTime + 120;

            player.AddStatusValue(StatusEffectIds.TFTerraStarCooldown, 1, 1);

            player.AddStatusValue(StatusEffectIds.TFSupercharging, 1, 1);

            context.Append("Combat.Terra.Supernova.Exec1".Localized());

            context.Append("Combat.Terra.Supernova.Exec2".Localized());

            var damage = PlayerTerraFormHandlers.TerraStarDamage(context) * (10 + context.Random.Next(11));

            CombatCalculator.ForEachEnemy(context, target =>
            {
                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

                if (target.Health > 0)
                    context.Append("Combat.Terra.Supernova.Hit".Localized());
            });

            player.HealthChange(-player.Health);

            player.MasteryXP(new MonoCoC.Registry.Identifier("coc", "TerrestrialFire"), 100);
        }
    }
}