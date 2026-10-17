using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class SharkManCombatHandlers
    {

        [Register("coc:shark_man_attack")]
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

        [Register("coc:shark_man_thrust_grab")]
        public static void ThrustGrab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.SharkMan.ThrustGrab.InAHuff".Localized());

            if (context.Random.Next(2) == 0 || context.Random.Next(100) < player.Strength * 1.15f)
            {
                context.Append("Combat.SharkMan.ThrustGrab.InMomentsYou".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Grappled))
                    player.CreateStatusEffect(StatusEffectIds.Grappled, 0, 0);
            }
            else
                context.Append("Combat.SharkMan.ThrustGrab.AllHeManages".Localized());
        }

        [Register("coc:shark_man_thrust_fuck")]
        public static void ThrustFuck(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.SharkMan.ThrustFuck.TheLustfulShark".Localized());

            player.TakeLustDamage(context.Random.Next(6) *

                ((player.Sensitivity + player.Corruption + player.Libido) / 100f));
        }

        [Register("coc:shark_man_charge_pt_one")]
        public static void ChargePt1(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.SharkMan.ChargePt1.TheSharkGrowls".Localized());

            if (context.Enemy is SharkManEnemy shark)
                shark.IsCharging = true;
        }

        [Register("coc:shark_man_charge_pt_two")]
        public static void ChargePt2(CombatContext context, CombatAbilityDefinition ability)
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

                player.Stun(1, 10 * (3 + Math.Max(GameSettings.Difficulty, -1)));

                var damage = CombatCalculator.ReduceDamage(context, player,

                    20 + enemy.Strength + enemy.Speed / 2f);

                player.HealthChange(-damage);
            }
            else
                context.Append("Combat.SharkMan.ChargePt2.WithAFocused".Localized());

            if (enemy is SharkManEnemy shark)
                shark.IsCharging = false;
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (!player.HasStatusEffect(StatusEffectIds.Grappled))
                return;

            context.Clear();

            if (context.Random.Next(3) == 0 || context.Random.Next(100) < player.Strength * 1.25f)
            {
                context.Append("Combat.SharkMan.Struggle.InYourTwisting".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Grappled);

                enemy.HealthChange(-(player.Strength / 10f + context.Random.Next(10)));
            }
            else
                context.Append("Combat.SharkMan.Struggle.YouFlailAnd".Localized());
        }
    }
}