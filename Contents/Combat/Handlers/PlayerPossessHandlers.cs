using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3105-3143 possessCalc / possessChance / possess
    public static class PlayerPossessHandlers
    {
        [Register("coc:knows_possess")]
        public static bool KnowsPossess(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.Incorporeality);

        // 原版 CombatAbilities.hx:158 standardStatComparison
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

        private static int PossessChance(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if ((!enemy.HasCock() && !enemy.HasVagina())
                || enemy.LustVulnerability == 0

                || enemy.Intelligence == 0

                || enemy.Intelligence > 100

                || player.Intelligence < enemy.Intelligence)

                return 0;

            return StandardStatComparison(player.Intelligence, enemy.Intelligence, -10);
        }

        [Register("coc:spell_possess")]
        public static void Possess(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Id.Path == "plain_girl" || enemy.HasPerk(PerkIds.Incorporeality))
            {
                context.Append("Combat.Spell.Possess.Ghost".Localized());

                return;
            }

            if (enemy is DullahanEnemy || enemy is DullahanHorseEnemy)
            {
                context.Append("Combat.Spell.Possess.Dullahan".Localized());

                return;
            }

            if (enemy is LivingStatueEnemy || enemy is VolcanicGolemEnemy || enemy is TrainingDummyEnemy)
            {
                context.Append("Combat.Spell.Possess.Nothing".Localized());

                return;
            }

            if ((!enemy.HasCock() && !enemy.HasVagina())
                || enemy.LustVulnerability == 0

                || enemy.Intelligence == 0

                || enemy.Intelligence > 100)
            {
                context.Append(enemy.Intelligence > 100

                    ? "Combat.Spell.Possess.TooSmart".Localized()

                    : "Combat.Spell.Possess.Incompatible".Localized());

                return;
            }

            if (PossessChance(context) > context.Random.Next(100))
            {
                context.Append("Combat.Spell.Possess.Success".Localized());

                var damage = (player.Intelligence / 5f + context.Random.Next(Math.Max(1, (int)player.Level)) + player.Level)

                    * enemy.LustVulnerability;

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + damage);
            }
            else
                context.Append("Combat.Spell.Possess.Fail".Localized());
        }
    }
}