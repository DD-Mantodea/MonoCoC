using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class RandomSuccubusCombatHandlers
    {
        [Register("coc:random_succubus_grapple")]
        public static void Grapple(CombatContext context, CombatAbilityDefinition ability)
        {
            ((RandomSuccubusEnemy)context.Enemy).Grapple(context);
        }

        [Register("coc:random_succubus_swipe")]
        public static void Swipe(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (RandomSuccubusEnemy)context.Enemy;

            var wingsNone = enemy.Body.Get<BodyParts.Vanilla.WingsPart>().Id == BodyPartTypes.None;

            context.Append("Combat.RandomSuccubus.Swipe.The0S".Localized(
                enemy.ShortName, wingsNone ? "legs tense" : "wings flap"));

            var hitChance = player.StandardDodgeFunc(enemy);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                ToHitChance = hitChance
            });

            if (avoid.AttackFailed)
                return;

            context.Append("Combat.RandomSuccubus.Swipe.ZeroClawsSwing".Localized(
                enemy.Gender == Creatures.Gender.Male ? "His" : "Her"));

            var damage = enemy.Strength + enemy.WeaponAttack;

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            if (damage < player.MaxHealth / 100)
                context.Append("Combat.RandomSuccubus.Swipe.ItDoesnT".Localized());

            player.TakeDamage(damage);
        }

        [Register("coc:random_succubus_lust_magic_attack")]
        public static void LustMagicAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (RandomSuccubusEnemy)context.Enemy;

            context.Append("Combat.RandomSuccubus.LustMagicAttack.TheDemon0".Localized(
                enemy.Gender == Creatures.Gender.Male ? "" : "ess"));

            var lust = (int)((5 + context.Random.Next(6))

                * ((2 * enemy.Intelligence + player.Libido + 2 * player.Corruption) / 200f));

            if (lust > 10)
                context.Append("Combat.RandomSuccubus.LustMagicAttack.ZeroMagicFires".Localized(
                    enemy.Gender == Creatures.Gender.Male ? "His" : "Her"));

            player.TakeLustDamage(lust, true);
        }

        [Register("coc:random_succubus_tail_whip")]
        public static void TailWhip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (RandomSuccubusEnemy)context.Enemy;

            var hitChance = player.StandardDodgeFunc(enemy, 15);

            var damage = (enemy.Strength + enemy.WeaponAttack) / 3f;

            context.Append("Combat.RandomSuccubus.TailWhip.HopingToCatch".Localized(enemy.ShortName));

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                ToHitChance = hitChance
            });

            if (avoid.AttackFailed)
                return;

            context.Append("Combat.RandomSuccubus.TailWhip.TheSpadeSlaps".Localized());

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            var lustDamage = damage / 2;

            player.TakeDamage(damage);

            player.TakeLustDamage(lustDamage, true);
        }

        [Register("coc:random_succubus_heel_kick")]
        public static void HeelKick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = (RandomSuccubusEnemy)context.Enemy;

            var hitChance = player.StandardDodgeFunc(enemy, -20);

            var damage = (enemy.Strength + enemy.WeaponAttack) * 2;

            context.Append("Combat.RandomSuccubus.HeelKick.TwistingHerBody".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                ToHitChance = hitChance
            });

            if (avoid.AttackFailed)
                return;

            context.Append("Combat.RandomSuccubus.HeelKick.WithAStrong".Localized());

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            player.TakeDamage(damage);
        }

        [Register("coc:random_succubus_hl_grapple")]
        public static void HlGrapple(CombatContext context, CombatAbilityDefinition ability)
        {
            ((RandomSuccubusEnemy)context.Enemy).HlGrapple(context);
        }
    }
}