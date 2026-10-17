using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 Kelt 的招式（src/classes/scenes/places/farm/Kelt.hx:8-107）。
    public static class KeltCombatHandlers
    {
        // 原版 keltTramplesJoo（Kelt.hx:9-33）：冲锋践踏，只允许闪避（无招架/格挡）。
        // 伤害 = reduceDamage(str + weaponAttack)；被挡下时 Kelt 自己 +5 欲望。
        [Register("coc:kelt_tramples_joo")]
        private static void TramplesJoo(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 keltShootBow（Kelt.hx:35-62）：每用一次把 bowCooldown 重置为 3。
        // 伤害 = reduceDamage(20 + str/3 + 100 + spe/3)。
        [Register("coc:kelt_shoot_bow")]
        private static void ShootBow(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 dayDreamKelly（Kelt.hx:87-94）：自慰式「攻击」，自己 +5 欲望。
        [Register("coc:kelt_day_dream_kelly")]
        private static void DayDreamKelly(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append((context.Random.Next(2) == 0
                ? "Combat.Kelt.DayDreamKelly.KeltPausesMid"
                : "Combat.Kelt.DayDreamKelly.FlaringHisNostrils").Localized());

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5);
        }

        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:kelt_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 KellyuraAttack（Kelt.hx:65-83）：rand(3) 三段，第三段再按玩家 lust100 分支。
        [Register("coc:kelt_kellyura_attack")]
        private static void KellyuraAttack(CombatContext context, CombatAbilityDefinition ability)
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
                    context.Append((player.Lust100 >= 80
                        ? "Combat.Kelt.KellyuraAttack.YourHandMoves"
                        : "Combat.Kelt.KellyuraAttack.YourHandsTwitch").Localized());

                    break;
            }

            player.TakeLustDamage(player.Libido / 5f + context.Random.Next(10));
        }
    }
}
