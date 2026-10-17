using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ShouldraCombatHandlers
    {

        [Register("coc:shouldra_attack")]
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

                DoBlock = false,

                DoFatigue = false
            });

            if (avoid.AttackFailed)
            {
                AppendDodge(context, avoid);

                return;
            }

            var damage = 0f;

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Combat.Shouldra.Shouldrattack.DuckingInClose".Localized());

                    break;

                case 1:
                    context.Append("Combat.Shouldra.Shouldrattack.TheGirlFeints".Localized());

                    break;

                default:
                    context.Append("Combat.Shouldra.Shouldrattack.YouMomentarilyDrop".Localized());

                    break;
            }

            player.TakeDamage(damage);

            if (damage > 0 && player.Armor.Id == ArmorIds.BondageStraps)
            {
                var who = enemy.Article + enemy.ShortName;

                context.Append("Combat.Shouldra.Attack.BondageStraps".Localized(
                    char.ToUpperInvariant(who[0]) + who[1..]));

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5 * enemy.LustVulnerability);
            }
        }

        [Register("coc:shouldra_lust_attack")]
        public static void LustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (context.Random.Next(2) == 0)
            {

                var butt = enemy is ShouldraEnemy { GhostLoli: true }

                    ? "Combat.Shouldra.LustAttack.TinyButt".Localized()

                    : "Combat.Shouldra.LustAttack.FirmButt".Localized();

                context.Append("Combat.Shouldra.ShouldraLustAttack.TheGirlSpins".Localized(butt));
            }
            else
                context.Append("Combat.Shouldra.ShouldraLustAttack.TheGirlS".Localized());

            player.TakeLustDamage(8 + player.Libido / 10f, true);
        }

        [Register("coc:shouldra_magic_lazers")]
        public static void MagicLazers(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Shouldra.ShouldraMagicLazers.FallingBackA".Localized());

            player.TakeDamage(16 + enemy.Level + context.Random.Next(10));
        }

        public static void AppendDodge(CombatContext context, AvoidResult avoid)
        {
            var key = avoid.Outcome switch
            {
                AvoidOutcome.Parry => "Combat.Shouldra.Attack.Parry",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.Shouldra.Attack.DodgeMisdirection",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.Shouldra.Attack.DodgeFlexibility",

                AvoidOutcome.Dodge when avoid.DodgeReason == "Blind" => "Combat.Shouldra.Attack.DodgeBlind",

                AvoidOutcome.Dodge => "Combat.Shouldra.Attack.DodgeUnhandled",

                _ => "Combat.Shouldra.Attack.DodgeUnhandled"
            };

            context.Append(key.Localized());
        }
    }
}