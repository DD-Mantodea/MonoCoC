using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    // 原版 Hellmouth.hx：撕咬 / 火焰吐息 / 召唤小鬼 / 火焰撕咬（hellmaw）四招，均已实现。
    public static class HellmouthCombatHandlers
    {

        [Register("coc:hellmouth_bite")]
        public static void HellmouthBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Hellmouth.Bite.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Hellmouth.Bite.Miss".Localized());

                return;
            }

            context.Append("Combat.Hellmouth.Bite.Hit".Localized());

            if (player.Bleed(context.Enemy, 2, 2))
                context.Append("Combat.Hellmouth.Bite.Bleed".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength / 1.25f + enemy.Level));
        }

        [Register("coc:hellmouth_fire_breath")]
        public static void FireBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Hellmouth.FireBreath.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Hellmouth.FireBreath.Miss".Localized());

                return;
            }

            context.Append("Combat.Hellmouth.FireBreath.Hit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Intelligence * 1.30f + enemy.Level) - 40;

            player.HealthChange(-Math.Max(0, damage));
        }

        [Register("coc:hellmouth_summon")]
        public static void HellmouthSummon(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hellmouth.Summon.Lead".Localized());

            var imp = EnemyRegistry.Create(EnemyIds.Imp);

            imp.ScaleToLevel(Random.Shared.Next(3) + 5);

            context.AddEnemy(imp);
        }

        // 原版 Hellmouth.hx:47 hellmaw()：火焰撕咬（命中判定的闪避修正 -15）
        [Register("coc:hellmouth_hellmaw")]
        public static void Hellmaw(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Hellmouth.Hellmaw.MawLunge".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true,
                // 原版 :56 toHitChance: player.standardDodgeFunc(this, -15)
                ToHitChance = Math.Max(0f, context.Player.StandardDodgeFunc(context.Enemy) - 15f)
            });

            if (avoid.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Scenes.Hellmouth.Hellmaw.MawBiteFail".Localized());

                // 原版 :59-60：monsterDamageType = DAMAGE_FIRE，且伤害经 reduceDamage(..., 85, false, true)
                var damage = CombatCalculator.ReduceDamage(context, context.Player,

                    context.Enemy.Strength * 1.2f + context.Enemy.Level);

                context.Player.TakeDamage(damage);
            }
            else
                context.Append("Common.Text.YouManageToDashToThe".Localized());
        }
    }
}