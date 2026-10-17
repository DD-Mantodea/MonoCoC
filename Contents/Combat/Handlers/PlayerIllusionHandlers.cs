using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3347-3407 kitsuneIllusionChance / kitsuneIllusion / kitsuneIllusionExec
    public static class PlayerIllusionHandlers
    {
        [Register("coc:knows_illusion")]
        public static bool KnowsIllusion(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.EnlightenedNinetails);

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

        [Register("coc:spell_illusion")]
        public static void Illusion(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.ThroatPunch)
                || player.HasStatusEffect(StatusEffectIds.WebSilence))
            {
                context.Append("Combat.Spell.Illusion.Silence".Localized());

                return;
            }

            if (enemy.Id.Path == "pod" || enemy.Intelligence == 0)
            {
                context.Append("Combat.Spell.Illusion.NoMind".Localized());

                player.ChangeFatigue(1);

                return;
            }

            context.Append("Combat.Spell.Illusion.Intro".Localized());

            var chance = StandardStatComparison(
                player.Intelligence,

                enemy.Intelligence,

                (int)(15 - 10 * enemy.StatusEffectValue(StatusEffectIds.Illusion, 0)));

            if (chance > context.Random.Next(100))
            {
                context.Append("Combat.Spell.Illusion.Success".Localized());

                if (enemy.StatusEffectValue(StatusEffectIds.Illusion, 0) > 0)
                    enemy.AddStatusValue(StatusEffectIds.Illusion, 0, 1);
                else
                    enemy.CreateStatusEffect(StatusEffectIds.Illusion, 1, 0, 0, 0);

                var stacks = enemy.StatusEffectValue(StatusEffectIds.Illusion, 0);

                if (enemy.Speed >= 0)
                    enemy.Speed -= 20 - stacks * 5;

                if (enemy.LustVulnerability > 0)
                    enemy.LustVulnerability += 0.1f;

                if (enemy.Speed < 1)
                    enemy.Speed = 1;
            }
            else
                context.Append("Combat.Spell.Illusion.Fail".Localized());
        }
    }
}