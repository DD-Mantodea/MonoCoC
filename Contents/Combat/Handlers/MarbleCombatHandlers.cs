using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MarbleCombatHandlers
    {

        [Register("coc:marble_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:marble_special_attack_one")]
        public static void SpecialAttackOne(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoFatigue = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -20)
            });

            if (avoid.AttackFailed)
            {
                AppendDodge(context, avoid, false);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 20 + enemy.WeaponAttack);

            if (damage <= 0)
                context.Append("Combat.Marble.SpecialAttackOne.Deflect".Localized());

            if (damage > 0)
            {
                context.Append("Combat.Marble.SpecialAttackOne.Hit".Localized());

                player.TakeDamage(damage);
            }
        }

        [Register("coc:marble_special_attack_two")]
        public static void SpecialAttackTwo(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoFatigue = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -30)
            });

            if (avoid.AttackFailed)
            {
                AppendDodge(context, avoid, true);

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
                context.Append("Combat.Marble.SpecialAttackTwo.BlindSwing".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 40 + enemy.WeaponAttack);

            damage /= 2;

            if (damage <= 0)
                context.Append("Combat.Marble.SpecialAttackTwo.Deflect".Localized());

            context.Append("Combat.Marble.SpecialAttackTwo.Hit".Localized());

            if (damage > 0)
                player.TakeDamage(damage);
        }

        public static void AppendDodge(CombatContext context, AvoidResult avoid, bool second)
        {
            var key = (second, avoid.DodgeReason) switch
            {
                (false, "Blind") => "Combat.Marble.SpecialAttackOne.DodgeBlind",

                (false, "Evade") => "Combat.Marble.SpecialAttackOne.DodgeEvade",

                (false, "Misdirection") => "Combat.Marble.SpecialAttackOne.DodgeMisdirection",

                (false, "Flexibility") => "Combat.Marble.SpecialAttackOne.DodgeFlexibility",

                (false, _) => "Combat.Marble.SpecialAttackOne.DodgeUnhandled",

                (true, "Blind") => "Combat.Marble.SpecialAttackTwo.DodgeBlind",

                (true, "Evade") => "Combat.Marble.SpecialAttackTwo.DodgeEvade",

                (true, "Misdirection") => "Combat.Marble.SpecialAttackTwo.DodgeMisdirection",

                (true, "Flexibility") => "Combat.Marble.SpecialAttackTwo.DodgeFlexibility",

                _ => "Combat.Marble.SpecialAttackTwo.DodgeUnhandled"
            };

            context.Append(key.Localized());
        }
    }
}