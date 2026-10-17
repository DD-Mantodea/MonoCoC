using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    /// <summary>
    /// 原版 Divine Wind 法术（CombatAbilities.hx:1253-1266）与 DivineWindBuff
    /// （statusEffects/combat/DivineWindBuff.hx:14-40）。
    /// </summary>
    public static class DivineWindHandlers
    {
        public static readonly Identifier DivineWind = new("coc", "divine_wind");

        /// <summary>原版 divineWind()：施放后给玩家挂 4 回合的治愈风。</summary>
        [Register("coc:spell_divine_wind")]
        public static void DivineWindCast(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.DivineWind.Text".Localized());

            context.Player.CreateStatusEffect(DivineWind, 0, 0, 0, 0, 4);
        }

        /// <summary>
        /// 原版 DivineWindBuff.onCombatRound()：
        /// heal = round((host.level + host.inte/1.5 + rand(host.inte)) * host.spellMod()) * 0.6
        /// 敌方 50% 概率被治愈，玩家 75% 概率被治愈。
        /// </summary>
        [Register("coc:effect_divine_wind_round")]
        public static void Round(CombatContext context, Creature host, StatusEffect status)
        {
            var heal = MathF.Round(
                (host.Level + host.Intelligence / 1.5f + context.Random.Next(Math.Max(1, (int)host.Intelligence)))

                * context.Player.SpellMod()) * 0.6f;

            if (context.Random.Next(4) < 2)
            {
                context.Append("Combat.Effect.DivineWind.HealsFoe".Localized(context.Enemy.ShortName));

                context.Enemy.HealthChange(heal);
            }

            if (context.Random.Next(4) < 3)
            {
                host.HealthChange(heal);

                context.Append("Combat.Effect.DivineWind.HealsYou".Localized());
            }
        }

        /// <summary>原版 DivineWindBuff 的 removeString。</summary>
        [Register("coc:effect_divine_wind_expire")]
        public static void Expire(CombatContext context, Creature host, StatusEffect status) =>

            context.Append("Combat.Effect.DivineWind.Fades".Localized());
    }
}