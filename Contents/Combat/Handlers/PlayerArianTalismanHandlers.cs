using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:1277-1320 Arian's Talisman 四种法术
    public static class PlayerArianTalismanHandlers
    {
        [Register("coc:arian_heal_available")]
        public static bool HealAvailable(CombatContext context, CombatState state) =>

            context.Player.KeyItemV1("Arian's Charged Talisman") == 2;

        [Register("coc:arian_immolation_available")]
        public static bool ImmolationAvailable(CombatContext context, CombatState state) =>

            context.Player.KeyItemV1("Arian's Charged Talisman") == 3;

        [Register("coc:arian_lust_available")]
        public static bool LustAvailable(CombatContext context, CombatState state) =>

            context.Player.KeyItemV1("Arian's Charged Talisman") == 4;

        private static void ClearTalisman() =>

            Scenes.SceneInstance<Scenes.Vanilla.Npcs.ArianScene>.Get().ClearTalisman();

        [Register("coc:arian_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Arian.Talisman.Heal.Text".Localized());

            var heal = ((player.Level * 5) + (player.Intelligence / 1.5f)

                + context.Random.Next(Math.Max(1, (int)player.Intelligence))) * player.SpellMod() * 1.5f;

            player.HealthChange(heal);

            ClearTalisman();
        }

        [Register("coc:arian_immolation")]
        public static void Immolation(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Arian.Talisman.Immolation.Text".Localized());

            var damage = 75 + (player.Intelligence / 2f + context.Random.Next(Math.Max(1, (int)player.Intelligence))) * player.SpellMod();

            damage = InfernoModifiers.CalcInfernoMod(context, damage);

            if (enemy is VolcanicGolemEnemy)
            {
                damage *= 0.2f;

                context.Append("Combat.Arian.Talisman.Immolation.Golem".Localized());
            }
            else
            {
                damage = CombatCalculator.GlobalMod(player, damage);

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

                enemy.CreateStatusEffect(StatusEffectIds.OnFire, 0, 0, 0, 0,

                    2 + context.Random.Next(Math.Max(1, (int)(player.Intelligence / 25))));
            }

            ClearTalisman();
        }

        [Register("coc:arian_lust")]
        public static void LustReduction(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Arian.Talisman.Lust.Text".Localized());

            var amount = 30 + context.Random.Next(Math.Max(1, (int)(player.Intelligence / 5))) * player.SpellMod();

            player.Lust = Math.Max(0, player.Lust - amount);

            ClearTalisman();
        }
    }
}