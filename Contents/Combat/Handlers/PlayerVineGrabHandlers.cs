using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:2073 / :4189 / :6685-6760
    public static class PlayerVineGrabHandlers
    {
        // 原版 combatAbilities.hx:158 standardStatComparison
        private static int StandardStatComparison(float attackerStat, float defenderStat, int chanceBonus = 0)
        {
            const float chanceWhenEqual = 70f;

            var c = MathF.Log(-100f / (chanceWhenEqual - 100f));

            if (defenderStat <= 0)
                defenderStat = 1f;

            var statRatio = MathF.Pow(attackerStat / defenderStat, 1.35f);

            var success = (int)MathF.Round(100f * (1f - MathF.Exp(-c * statRatio)));

            return Math.Clamp(success + chanceBonus, 0, 100);
        }

        private static int GrabChance(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasPerk(PerkIds.Juggernaut))
                return 0;

            return StandardStatComparison(player.Speed, enemy.Speed, -30);
        }

        [Register("coc:vine_grab_ready")]
        public static bool VineGrabReady(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Armor != null

                && player.Armor.Id.Path == "vine_armor"

                && player.Armor.Stage > 4

                && !context.Enemy.HasStatusEffect(StatusEffectIds.Constricted);
        }

        [Register("coc:enemy_constricted")]
        public static bool EnemyConstricted(CombatContext context, CombatState state) =>

            context.Enemy.HasStatusEffect(StatusEffectIds.Constricted);

        [Register("coc:vine_grab")]
        public static void VineGrab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Plural)
            {
                context.Append("Combat.VineGrab.TooMany".Localized());

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Concentration))
            {
                context.Append("Combat.VineGrab.Concentration".Localized());

                return;
            }

            if (enemy is VolcanicGolemEnemy)
            {
                if (enemy.HasStatusEffect(StatusEffectIds.VolcanicFistProblem))
                {
                    context.Append("Combat.VineGrab.GolemWrap".Localized());

                    if (player.Strength > 80 && !enemy.HasStatusEffect(StatusEffectIds.VolcanicFrenzy))
                    {
                        context.Append("Combat.VineGrab.GolemRip".Localized());

                        enemy.CreateStatusEffect(StatusEffectIds.VolcanicWeapRed, 3, 0, 0, 0);

                        enemy.WeaponAttack -= 40;

                        if (enemy.WeaponAttack <= 0)
                            enemy.WeaponAttack = 10;
                    }
                    else
                        context.Append("Combat.VineGrab.GolemTooWeak".Localized());
                }
                else
                    context.Append("Combat.VineGrab.GolemSwat".Localized());

                return;
            }

            if (context.Random.Next(100) >= GrabChance(context))
            {
                context.Append("Combat.VineGrab.GrabFail".Localized());

                return;
            }

            context.Append("Combat.VineGrab.Grab".Localized());

            enemy.CreateStatusEffect(StatusEffectIds.Constricted, 1, 0, 0, 0);

            enemy.AddStatusValue(StatusEffectIds.Constricted, 0, context.Random.Next(3));

            CombatRangeData.CloseDistance(enemy);

            enemy.Bleed(player, 1, 0.5f);
        }

        [Register("coc:vine_constrict")]
        public static void VineConstrict(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.Fatigue + player.PhysicalCost(10) > player.MaxFatigue())
            {
                context.Append("Combat.VineGrab.ConstrictTired".Localized());

                return;
            }

            player.ChangeFatigue(10);

            context.Append("Combat.VineGrab.ConstrictText".Localized());

            var damage = enemy.MaxHealth * (0.10f + context.Random.Next(15) / 100f);

            enemy.HealthChange(-damage);

            enemy.HandleDamaged(context, damage, CombatDamageKind.Melee);

            enemy.Bleed(player, 1, 1.5f);

            if (enemy.Health < 1)
                context.Append("Combat.VineGrab.ConstrictFaint".Localized());
        }

        [Register("coc:vine_release")]
        public static void VineRelease(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.VineGrab.LetGo".Localized());

            enemy.RemoveStatusEffect(StatusEffectIds.Constricted);
        }
    }
}