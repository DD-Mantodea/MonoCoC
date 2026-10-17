using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class YetiCombatHandlers
    {

        [Register("coc:yeti_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:yeti_claw_and_punch")]
        public static void ClawAndPunch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Yeti.ClawAndPunch.Miss".Localized());

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) > 0)
            {
                var selfDamage = 30 + context.Random.Next(50);

                enemy.HealthChange(-selfDamage);

                context.Append("Combat.Yeti.ClawAndPunch.BlindWall".Localized(selfDamage));

                if (context.Random.Next(2) == 0)
                {
                    enemy.CreateStatusEffect(StatusEffectIds.Stunned, duration: 2);

                    context.Append("Combat.Yeti.ClawAndPunch.SelfStun".Localized());
                }

                return;
            }

            context.Append("Combat.Yeti.ClawAndPunch.Hit".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + 25 + context.Random.Next(50)));
        }

        [Register("coc:yeti_tackle_tumble")]
        public static void TackleTumble(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                var selfDamage = 30 + context.Random.Next(50);

                enemy.HealthChange(-selfDamage);

                context.Append("Combat.Yeti.TackleTumble.Miss".Localized(selfDamage));

                return;
            }

            context.Append("Combat.Yeti.TackleTumble.Hit".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + 50 + context.Random.Next(150)));
        }

        [Register("coc:yeti_snowball")]
        public static void Snowball(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Yeti.Snowball.Miss".Localized());

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) > 0)
            {
                context.Append("Combat.Yeti.Snowball.BlindMiss".Localized());

                return;
            }

            context.Append("Combat.Yeti.Snowball.Hit".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength / 2 + context.Random.Next(20)));

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -10, "Yeti");
        }

        [Register("coc:yeti_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var threshold = Math.Max(1, (int)(player.Libido + player.Corruption));

            if (context.Random.Next(threshold) >= 30 && context.Random.Next(3) > 0)
            {
                context.Append("Combat.Yeti.Tease.Success".Localized());

                var lust = 20 + context.Random.Next(10);

                player.Lust = Math.Min(player.MaxLust, player.Lust + lust);
            }
            else
                context.Append("Combat.Yeti.Tease.Fail".Localized());
        }
    }
}