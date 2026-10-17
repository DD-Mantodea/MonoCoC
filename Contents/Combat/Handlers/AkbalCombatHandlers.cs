using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class AkbalCombatHandlers
    {

        public static bool IsFierce(CombatContext context) => context.Enemy.Id.Path == "akbal_fierce";

        [Register("coc:akbal_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Akbal.Attack.Miss".Localized());

                return;
            }

            var heavy = context.Random.Next(2) != 0;

            var damage = context.Enemy.Strength + (heavy ? 25f : 0f);

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            if (damage <= 0)
            {
                context.WriteAction(heavy ? "Combat.Akbal.Attack.HeavyBlocked".Localized() : "Combat.Akbal.Attack.Blocked".Localized());

                return;
            }

            player.HealthChange(-damage);

            context.WriteAction(heavy ? "Combat.Akbal.Attack.Heavy".Localized() : "Combat.Akbal.Attack.Normal".Localized());
        }

        [Register("coc:akbal_special")]
        public static void Special(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var fierce = IsFierce(context);

            if (context.Random.Next(2) == 0 && player.Speed > 20)
            {
                context.WriteAction("Combat.Akbal.Special.Speed".Localized());

                player.CreateStatusEffect(StatusEffectIds.AkbalSpeed);

                return;
            }

            context.WriteAction("Combat.Akbal.Special.Fire".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Akbal.Special.FireDodged".Localized());

                if (fierce)
                {
                    context.WriteAction("Combat.Akbal.Special.FlamePenalty".Localized());

                    if (player.HasStatusEffect(StatusEffectIds.AkbalFlameDebuff))
                        player.ChangeStatusValue(StatusEffectIds.AkbalFlameDebuff, 0, 1);
                    else
                        player.CreateStatusEffect(StatusEffectIds.AkbalFlameDebuff);
                }

                return;
            }

            var damage = fierce ? 80f + context.Random.Next(20) : 40f;

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            player.HealthChange(-damage);

            context.Append("Combat.Akbal.Special.FireDamage".Localized(damage));
        }

        [Register("coc:akbal_lust_attack")]
        public static void LustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var fierce = IsFierce(context);

            var first = !player.HasStatusEffect(StatusEffectIds.Whispered)

                && !player.HasStatusEffect(StatusEffectIds.TrueWhispered);

            if (first)
            {
                context.WriteAction(fierce ? "Combat.Akbal.Lust.FirstFierce".Localized() : "Combat.Akbal.Lust.First".Localized());

                var lust = 7f + (100f - player.Intelligence) / 10f;

                if (fierce) lust *= 2;

                player.Lust = Math.Min(player.MaxLust, player.Lust + lust);

                player.CreateStatusEffect(fierce ? StatusEffectIds.TrueWhispered : StatusEffectIds.Whispered);

                return;
            }

            context.WriteAction(fierce ? "Combat.Akbal.Lust.RepeatFierce".Localized() : "Combat.Akbal.Lust.Repeat".Localized());

            var ongoing = 12f + (100f - player.Intelligence) / 10f;

            if (fierce) ongoing *= 2;

            player.Lust = Math.Min(player.MaxLust, player.Lust + ongoing);

            if (!player.HasStatusEffect(StatusEffectIds.Whispered))
                player.CreateStatusEffect(StatusEffectIds.Whispered);
        }

        [Register("coc:akbal_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            if (enemy.HealthRatio() >= 1f)
                context.WriteAction("Combat.Akbal.Heal.Full".Localized());
            else
                context.WriteAction("Combat.Akbal.Heal.Wounded".Localized());

            enemy.HealthChange(enemy.MaxHealth * 0.12f);

            if (!IsFierce(context))
                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 10);
        }
    }
}