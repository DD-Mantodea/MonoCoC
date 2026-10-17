using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 Marae 的招式（src/classes/scenes/places/boat/Marae.hx:9-60）。
    public static class MaraeCombatHandlers
    {
        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:marae_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, enemy.NormalAttackAvoid.For(enemy, player));

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        // 原版 smite（Marae.hx:40-43）：挂 Uber，下一回合由 smiteHit 落雷。
        [Register("coc:marae_smite")]
        private static void Smite(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.Marae.Smite.MaraeMouthsA".Localized());

            if (!enemy.HasStatusEffect(VanillaStatusEffects.Uber))
                enemy.CreateStatusEffect(VanillaStatusEffects.Uber, 1);
        }

        // 原版 smiteHit（Marae.hx:45-60）：玩家「等待」过就躲开；否则 100 + str + corAdjustedDown×5。
        [Register("coc:marae_smite_hit")]
        private static void SmiteHit(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            if (context.PlayerWaited)
            {
                context.Append("Combat.Marae.SmiteHit.YouLookUp".Localized());
            }
            else
            {
                context.Append("Combat.Marae.SmiteHit.WithoutWarningThe".Localized());

                if (player.Corruption >= 50)
                    context.Append("Combat.Marae.SmiteHit.TheIntensityOf".Localized());

                var damage = 100 + enemy.Strength + player.CorAdjustedDown() * 5;

                damage = CombatCalculator.ReduceDamage(context, player, damage);

                player.HealthChange(-damage);
            }

            enemy.RemoveStatusEffect(VanillaStatusEffects.Uber);
        }

        // 原版 tentacleAttack（Marae.hx:9-23）：纯闪避判定；命中造成伤害与少量欲望。
        [Register("coc:marae_tentacle_attack")]
        private static void TentacleAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Marae.TentacleAttack.YouSpotBarrage".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.DodgeReason == "Speed")
            {
                context.Append("Combat.Marae.TentacleAttack.AndYouSuccessfully".Localized());

                return;
            }

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Marae.TentacleAttack.AndYouSuccessfully2".Localized());

                return;
            }

            context.Append("Combat.Marae.TentacleAttack.ButYouFail".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player,
                enemy.Strength + 100 + context.Random.Next(50));

            player.HealthChange(-damage);
            player.TakeLustDamage(context.Random.Next(5) + 5);
        }

        // 原版 tentacleRape（Marae.hx:25-37）：命中只结算欲望。
        [Register("coc:marae_tentacle_rape")]
        private static void TentacleRape(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Marae.TentacleRape.YouSpotBarrage".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.DodgeReason == "Speed")
            {
                context.Append("Combat.Marae.TentacleRape.YouManageTo".Localized());

                return;
            }

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Marae.TentacleRape.YouManageTo2".Localized());

                return;
            }

            context.Append("Combat.Marae.TentacleRape.YouAttemptTo".Localized());

            var lust = 20 + context.Random.Next((int)(player.Corruption / 10)) +
                context.Random.Next((int)(player.Sensitivity / 5)) +
                context.Random.Next((int)(player.Libido / 10)) +
                context.Random.Next(10);

            player.TakeLustDamage(lust);
        }
    }
}
