using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3594-3683 broodBiteCalc / unleashBrood
    public static class PlayerUnleashBroodHandlers
    {
        [Register("coc:has_nephila_brood")]
        public static bool HasNephilaBrood(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.NephilaArchQueen);

        [Register("coc:special_unleash_brood")]
        public static void UnleashBrood(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy is EncapsulationPodEnemy)
            {
                context.Append("Combat.Special.UnleashBrood.Pod".Localized());

                return;
            }

            if (enemy is TentacleBeastEnemy)
            {
                context.Append("Combat.Special.UnleashBrood.TentacleBeast".Localized());

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Shell))
            {
                context.Append("Combat.Special.UnleashBrood.Shell".Localized());

                return;
            }

            context.Append("Combat.Special.UnleashBrood.Intro".Localized());

            var damage = 10 + (player.Toughness / 3f + context.Random.Next((int)Math.Max(1, player.Intelligence / 2))) * player.SpellMod();

            if (enemy.Speed < 1)
                damage *= enemy.Plural ? 2.5f : 5.0f;

            if (damage > 0)
            {
                damage *= player.PhysMod();

                damage = CombatCalculator.GlobalMod(player, damage);
            }

            var reduced = CombatCalculator.ReduceDamage(context, enemy, damage);

            if (reduced <= 0)
            {
                reduced = 0;

                context.Append("Combat.Special.UnleashBrood.Blocked".Localized());
            }
            else if (reduced < 10)
                context.Append("Combat.Special.UnleashBrood.Low".Localized());
            else if (reduced < 20)
                context.Append("Combat.Special.UnleashBrood.Medium".Localized());
            else if (reduced < 30)
                context.Append("Combat.Special.UnleashBrood.High".Localized());
            else
                context.Append("Combat.Special.UnleashBrood.Mutilate".Localized());

            CombatCalculator.DamageEnemy(context, reduced, CombatDamageKind.Ranged);

            if (enemy.Bleed(player))
                context.Append(enemy.Plural

                    ? "Combat.Special.UnleashBrood.BleedPlural".Localized()

                    : "Combat.Special.UnleashBrood.Bleed".Localized());
            else if (enemy is LivingStatueEnemy)
                context.Append("Combat.Special.UnleashBrood.NoBleedStatue".Localized());

            if (!enemy.HasStatusEffect(StatusEffectIds.NephilaCompulsion))
            {
                context.Append("Combat.Special.UnleashBrood.First".Localized());

                enemy.CreateStatusEffect(StatusEffectIds.NephilaCompulsion, 0, 0, 0, 0);
            }
            else if (enemy.Speed <= 1)
                context.Append(enemy.Plural

                    ? "Combat.Special.UnleashBrood.FinalPlural".Localized()

                    : "Combat.Special.UnleashBrood.Final".Localized());
            else
            {
                context.Append(enemy.Plural

                    ? "Combat.Special.UnleashBrood.IntermediatePlural".Localized()

                    : "Combat.Special.UnleashBrood.Intermediate".Localized());

                enemy.Speed -= 30;
            }
        }
    }
}