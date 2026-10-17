using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3300-3345 kitsuneChanceCalc / kitsuneTerror
    public static class PlayerTerrorHandlers
    {
        [Register("coc:knows_terror")]
        public static bool KnowsTerror(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.CorruptedNinetails);

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

        [Register("coc:spell_terror")]
        public static void Terror(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.ThroatPunch)
                || player.HasStatusEffect(StatusEffectIds.WebSilence))
            {
                context.Append("Combat.Spell.Terror.Silence".Localized());

                return;
            }

            if (enemy.Id.Path == "pod" || enemy.Intelligence == 0)
            {
                context.Append("Combat.Spell.Terror.NoMind".Localized());

                player.ChangeFatigue(1);

                return;
            }

            context.Append("Combat.Spell.Terror.Intro".Localized());

            var chance = StandardStatComparison(
                player.Intelligence,

                enemy.Intelligence,

                (int)(-30 - 10 * enemy.StatusEffectValue(StatusEffectIds.Fear, 1)));

            if (chance > context.Random.Next(100))
            {
                context.Append("Combat.Spell.Terror.Success".Localized());

                if (enemy.StatusEffectValue(StatusEffectIds.Fear, 1) > 0)
                    enemy.AddStatusValue(StatusEffectIds.Fear, 1, 1);
                else
                    enemy.CreateStatusEffect(StatusEffectIds.Fear, 0, 1, 0, 0);

                enemy.AddStatusValue(StatusEffectIds.Fear, 0, 5);

                enemy.Speed = Math.Max(1, enemy.Speed - 5);
            }
            else
            {
                context.Append("Combat.Spell.Terror.Fail".Localized());

                if (enemy.StatusEffectValue(StatusEffectIds.Fear, 1) >= 4)
                    context.Append("Combat.Spell.Terror.Resistant".Localized());

                context.Append("[pg]");
            }
        }
    }
}