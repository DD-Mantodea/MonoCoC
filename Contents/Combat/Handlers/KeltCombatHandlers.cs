using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class KeltCombatHandlers
    {

        [Register("coc:kelt_tramples_joo")]
        public static void TramplesJoo(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Kelt.KeltTramplesJoo.BeforeYouKnow".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Kelt.KeltTramplesJoo.YouRollOut".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0)
            {
                context.Append("Combat.Kelt.KeltTramplesJoo.IncrediblyYouBrace".Localized());

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);
            }
            else
            {
                context.Append("Combat.Kelt.KeltTramplesJoo.YouCanT".Localized());

                player.HealthChange(-damage);
            }
        }

        [Register("coc:kelt_shoot_bow")]
        public static void ShootBow(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy is KeltEnemy kelt)
                kelt.SetBowCooldown(3);

            context.Append("Combat.Kelt.KeltShootBow.KeltKnocksAnd".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Kelt.KeltShootBow.YouManageTo".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player,

                20 + enemy.Strength / 3f + 100 + enemy.Speed / 3f);

            if (damage < 0)
                damage = 0;

            if (damage == 0)
            {
                context.Append("Combat.Kelt.KeltShootBow.YouDeflectThe".Localized());

                return;
            }

            context.Append("Combat.Kelt.KeltShootBow.TheArrowBites".Localized());

            player.HealthChange(-damage);
        }

        [Register("coc:kelt_day_dream_kelly")]
        public static void DayDreamKelly(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append((context.Random.Next(2) == 0

                ? "Combat.Kelt.DayDreamKelly.KeltPausesMid"

                : "Combat.Kelt.DayDreamKelly.FlaringHisNostrils").Localized());

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);
        }

        [Register("coc:kelt_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
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

        [Register("coc:kelt_kellyura_attack")]
        public static void KellyuraAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Combat.Kelt.KellyuraAttack.KeltFlashesHis".Localized());

                    break;

                case 2:
                    context.Append("Combat.Kelt.KellyuraAttack.GrinningKeltRuns".Localized());

                    break;

                default:
                    context.Append("Combat.Kelt.KellyuraAttack.KeltSnarls".Localized());

                    context.Append((player.LustPercent >= 80

                        ? "Combat.Kelt.KellyuraAttack.YourHandMoves"

                        : "Combat.Kelt.KellyuraAttack.YourHandsTwitch").Localized());

                    break;
            }

            player.TakeLustDamage(player.Libido / 5f + context.Random.Next(10));
        }
    }
}