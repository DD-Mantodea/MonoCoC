using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    /// <summary>
    /// 原版 Wither 法术（CombatAbilities.hx:2558-2575）与 WitheringDebuff 状态
    /// （statusEffects/combat/WitheringDebuff.hx）。
    /// 原版该 debuff 只有文案、原版也未含治疗反噬数值，这里保持同样行为，不臆造机制。
    /// </summary>
    public static class WitherHandlers
    {
        public static readonly Identifier WitheringDebuff = new("coc", "withering_debuff");

        // 原版 CombatAbilities.hx:158 standardStatComparison
        private static int StandardStatComparison(float attackerStat, float defenderStat, int chanceBonus = 0)
        {
            const float chanceWhenEqual = 70f;

            var c = MathF.Log(-100f / (chanceWhenEqual - 100f));

            if (defenderStat <= 0) defenderStat = 1f;

            var statRatio = MathF.Pow(attackerStat / defenderStat, 1.35f);

            var success = (int)MathF.Round(100f * (1f - MathF.Exp(-c * statRatio)));

            return Math.Clamp(success + chanceBonus, 0, 100);
        }

        [Register("coc:spell_wither")]
        public static void Wither(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.WriteAction("Combat.Spell.Wither.Text".Localized());

            // 原版 witherChance(): standardStatComparison(player.inte, (monster.inte + monster.tou)/2, -10)
            var chance = StandardStatComparison(
                player.Intelligence,

                (enemy.Intelligence + enemy.Toughness) / 2f,

                -10);

            if (context.Random.Next(100) <= chance)
            {
                context.Append("Combat.Spell.Wither.Hits".Localized());

                // 原版 new WitheringDebuff(Std.int(1 + Math.fround(player.spellMod() * 2)))
                var duration = MathF.Round(1f + player.SpellMod() * 2f);

                enemy.CreateStatusEffect(WitheringDebuff, 0, 0, 0, 0, duration);
            }
            else
                context.Append("Combat.Spell.Wither.Resists".Localized());
        }

        /// <summary>原版 WitheringDebuff.onAttach 的 updateString。</summary>
        [Register("coc:effect_withering_round")]
        public static void Round(CombatContext context, Creature host, StatusEffect status) =>

            context.Append("Combat.Effect.Withering.StillWithered".Localized(host.ShortName));

        /// <summary>原版 WitheringDebuff.onAttach 的 removeString。</summary>
        [Register("coc:effect_withering_expire")]
        public static void Expire(CombatContext context, Creature host, StatusEffect status) =>

            context.Append("Combat.Effect.Withering.NoLonger".Localized(host.ShortName));
    }
}