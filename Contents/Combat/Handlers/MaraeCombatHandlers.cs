using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MaraeCombatHandlers
    {

        [Register("coc:marae_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
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

        [Register("coc:marae_smite")]
        public static void Smite(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.Marae.Smite.MaraeMouthsA".Localized());

            if (!enemy.HasStatusEffect(StatusEffectIds.Uber))
                enemy.CreateStatusEffect(StatusEffectIds.Uber, 1);
        }

        [Register("coc:marae_smite_hit")]
        public static void SmiteHit(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (context.PlayerWaited)
                context.Append("Combat.Marae.SmiteHit.YouLookUp".Localized());
            else
            {
                context.Append("Combat.Marae.SmiteHit.WithoutWarningThe".Localized());

                if (player.Corruption >= 50)
                    context.Append("Combat.Marae.SmiteHit.TheIntensityOf".Localized());

                var damage = 100 + enemy.Strength + player.CorAdjustedDown() * 5;

                damage = CombatCalculator.ReduceDamage(context, player, damage);

                player.HealthChange(-damage);
            }

            enemy.RemoveStatusEffect(StatusEffectIds.Uber);
        }

        [Register("coc:marae_tentacle_attack")]
        public static void TentacleAttack(CombatContext context, CombatAbilityDefinition ability)
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

        [Register("coc:marae_tentacle_rape")]
        public static void TentacleRape(CombatContext context, CombatAbilityDefinition ability)
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