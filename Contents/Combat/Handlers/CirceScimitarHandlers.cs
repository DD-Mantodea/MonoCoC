using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    /// <summary>
    /// 原版 Circe's Summoned Scimitars：
    /// 条目 CombatAbilities.hx:1221-1237，施法 :5512-5522，斩击 :5687-5745，
    /// 回合末执行见 statusEffects/combat/CirceScimitarBuff.hx。
    /// C# 把「剑数状态 + 回合末执行」合并为一条带 onRound 的状态。
    /// </summary>
    public static class CirceScimitarHandlers
    {
        public static readonly Identifier Scimitar = new("coc", "circe_summoned_scimitar");

        private const int MaxScimitars = 3;

        /// <summary>原版 summonSword()：召唤一把剑（最多 3 把）。</summary>
        [Register("coc:spell_summon_scimitar")]
        public static void Summon(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var existing = player.StatusEffectByType(Scimitar);

            var count = existing == null ? 0 : (int)existing.Values[0];

            // 原版 disabledWhen：剑数 >= 3 时不可再召唤
            if (count >= MaxScimitars)
            {
                context.WriteAction("Combat.Spell.SummonScimitar.TooMany".Localized());

                return;
            }

            context.WriteAction("Combat.Spell.SummonScimitar.Text".Localized());

            context.Append("Combat.Spell.SummonScimitar.Alive".Localized());

            if (existing != null) existing.Values[0] += 1;
            else player.CreateStatusEffect(Scimitar, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// 原版 summonedSwordExec()：每把剑各攻击一次。
        /// 命中率 75 - 敌人闪避；伤害 rand(50,80) * (1 + spellMod/3)，再经目标减伤。
        /// </summary>
        [Register("coc:effect_circe_scimitar_round")]
        public static void Round(CombatContext context, Creature host, StatusEffect status)
        {
            var enemy = context.Enemy;

            var swords = (int)status.Values[0];

            for (var i = 0; i < swords; i++)
            {
                var chance = 75f - enemy.GetEvasionChance();

                if (context.Random.Next(100) >= chance)
                {
                    context.Append("Combat.Spell.SummonScimitar.Misses".Localized(enemy.ShortName));

                    continue;
                }

                var raw = context.Random.Next(50, 81) * (1f + context.Player.SpellMod() / 3f);

                var damage = CombatCalculator.ReduceDamage(context, enemy, (int)raw);

                enemy.HealthChange(-damage);

                context.Append("Combat.Spell.SummonScimitar.Slash".Localized(enemy.ShortName));
            }
        }
    }
}