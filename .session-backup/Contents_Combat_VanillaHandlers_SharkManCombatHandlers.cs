using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 SharkMan 的招式（src/classes/scenes/places/swim/SharkMan.hx:24-104）。
    public static class SharkManCombatHandlers
    {
        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:shark_man_attack")]
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

        // 原版 thrustGrab（SharkMan.hx:39-48）：1/2 或 str×1.15% 抓住玩家。
        [Register("coc:shark_man_thrust_grab")]
        private static void ThrustGrab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.SharkMan.ThrustGrab.InAHuff".Localized());

            if (context.Random.Next(2) == 0 || context.Random.Next(100) < player.Strength * 1.15f)
            {
                context.Append("Combat.SharkMan.ThrustGrab.InMomentsYou".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.Grappled))
                    player.CreateStatusEffect(VanillaStatusEffects.Grappled, 0, 0);
            }
            else
            {
                context.Append("Combat.SharkMan.ThrustGrab.AllHeManages".Localized());
            }
        }

        // 原版 thrustFuck（SharkMan.hx:50-54）：被抓住时持续磨蹭。
        [Register("coc:shark_man_thrust_fuck")]
        private static void ThrustFuck(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.SharkMan.ThrustFuck.TheLustfulShark".Localized());

            player.TakeLustDamage(context.Random.Next(6) *
                ((player.Sensitivity + player.Corruption + player.Libido) / 100f));
        }

        // 原版 chargePt1（SharkMan.hx:56-59）：开始蓄力。
        [Register("coc:shark_man_charge_pt1")]
        private static void ChargePt1(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.SharkMan.ChargePt1.TheSharkGrowls".Localized());

            if (context.Enemy is SharkManEnemy shark)
                shark.IsCharging = true;
        }

        // 原版 chargePt2（SharkMan.hx:61-73）：玩家「等待」过或闪避成功则冲空；
        // 命中时击晕并造成 reduceDamage(20 + str + spe/2)。
        [Register("coc:shark_man_charge_pt2")]
        private static void ChargePt2(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var didHit = !context.PlayerWaited &&
                CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy,
                    Defender = player,
                    DoDodge = true,
                    DoParry = false,
                    DoBlock = false
                }).Outcome == AvoidOutcome.Hit;

            if (didHit)
            {
                context.Append("Combat.SharkMan.ChargePt2.YouReImpacted".Localized());

                player.Stun(1, (int)(10 * (3 + Math.Max(CoC.Instance.Difficulty, -1))));

                var damage = CombatCalculator.ReduceDamage(context, player,
                    20 + enemy.Strength + enemy.Speed / 2f);

                player.HealthChange(-damage);
            }
            else
            {
                context.Append("Combat.SharkMan.ChargePt2.WithAFocused".Localized());
            }

            if (enemy is SharkManEnemy shark)
                shark.IsCharging = false;
        }

        // 原版 struggle（SharkMan.hx:24-36）：1/3 或 str×1.25% 成功脱身并反伤鲨鱼人。
        public static void Struggle(CombatContext context)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            if (!player.HasStatusEffect(VanillaStatusEffects.Grappled))
                return;

            context.Clear();

            if (context.Random.Next(3) == 0 || context.Random.Next(100) < player.Strength * 1.25f)
            {
                context.Append("Combat.SharkMan.Struggle.InYourTwisting".Localized());

                player.RemoveStatusEffect(VanillaStatusEffects.Grappled);

                enemy.HealthChange(-(player.Strength / 10f + context.Random.Next(10)));
            }
            else
            {
                context.Append("Combat.SharkMan.Struggle.YouFlailAnd".Localized());
            }
        }
    }
}
