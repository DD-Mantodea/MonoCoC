using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3504-3571 paralyzingStareChance / paralyzingStare
    public static class PlayerParalyzingStareHandlers
    {
        [Register("coc:can_use_stare")]
        public static bool CanUseStare(CombatContext context, CombatState state) =>

            context.Player.CanUseStare();

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

        [Register("coc:spell_paralyzing_stare")]
        public static void ParalyzingStare(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.ThroatPunch)
                || player.HasStatusEffect(StatusEffectIds.WebSilence))
            {
                context.Append("Combat.Spell.ParalyzingStare.Silence".Localized());

                return;
            }

            if (enemy is EncapsulationPodEnemy || enemy.Intelligence == 0)
            {
                context.Append("Combat.Spell.ParalyzingStare.NoMind".Localized());

                player.ChangeFatigue(1);

                return;
            }

            if (enemy is TentacleBeastEnemy)
            {
                context.Append("Combat.Spell.ParalyzingStare.NoEyes".Localized());

                player.ChangeFatigue(1);

                return;
            }

            if (enemy.HasPerk(PerkIds.BasiliskResistance))
            {
                context.Append("Combat.Spell.ParalyzingStare.Immune".Localized());

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Shell))
            {
                context.Append("Combat.Spell.ParalyzingStare.Shell".Localized());

                return;
            }

            context.Append("Combat.Spell.ParalyzingStare.Intro".Localized());

            var training = Math.Clamp(CoC.Instance.Flags.Get(GameFlagIds.BasiliskResistanceTracker) / 100f, 0f, 1f);

            var modifier = (int)(-30 * (1 - training));

            if (player.HasPerk(PerkIds.NaturalWeapons))
                modifier += 20;

            var chance = StandardStatComparison(player.Intelligence, enemy.Intelligence, modifier);

            if (chance > context.Random.Next(100))
            {
                var magnitude = 16 + training * 8;

                context.Append("Combat.Spell.ParalyzingStare.Success".Localized());

                CombatCalculator.AddCombatBuff(enemy, GameStats.Speed, -magnitude, "BasiliskSlow", StatusEffectIds.BasiliskSlow);

                enemy.CreateStatusEffect(StatusEffectIds.BasiliskCompulsion, magnitude * 0.75f, 0, 0, 0);

                CoC.Instance.Flags.Set(GameFlagIds.BasiliskResistanceTracker,

                    CoC.Instance.Flags.Get(GameFlagIds.BasiliskResistanceTracker) + 4);

                context.Append("Combat.Spell.ParalyzingStare.SpeedLoss".Localized((int)Math.Round(magnitude)));
            }
            else
            {
                context.Append("Combat.Spell.ParalyzingStare.Fail".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.BasiliskResistanceTracker,

                    CoC.Instance.Flags.Get(GameFlagIds.BasiliskResistanceTracker) + 2);
            }
        }
    }
}