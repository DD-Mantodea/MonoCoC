using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class FetishZealotCombatHandlers
    {

        [Register("coc:fetish_zealot_zealot_special_one")]
        public static void ZealotSpecial1(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var key = enemy.ArmorName switch
            {
                OutfitIds.ReligiousClothes => "Combat.FetishZealot.Special1.Religious",

                OutfitIds.LeatherClothes => "Combat.FetishZealot.Special1.Leather",

                OutfitIds.StudentsClothes => "Combat.FetishZealot.Special1.Students",

                _ => "Combat.FetishZealot.Special1.Default"
            };

            context.Append(key.Localized());

            player.Lust = Math.Min(player.MaxLust,

                player.Lust + 7 + context.Random.Next(Math.Max(1, (int)(player.Libido / 20 + player.Corruption / 20))));
        }

        [Register("coc:fetish_zealot_zealot_special_two")]
        public static void ZealotSpecial2(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.FetishZealot.Special2.Lead".Localized());

            var amount = enemy.Lust / 2;

            player.Lust = Math.Min(player.MaxLust, player.Lust + amount);

            enemy.Lust = Math.Max(0, enemy.Lust - amount);
        }

        [Register("coc:fetish_zealot_attack")]
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

            if (damage > 0)
            {
                context.Append("Combat.FetishZealot.PostAttack.Lead".Localized());

                context.Append(player.Intelligence > 50

                    ? "Combat.FetishZealot.PostAttack.Smart".Localized()

                    : "Combat.FetishZealot.PostAttack.Plain".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + player.Libido / 20 + 5);
            }
        }
    }
}