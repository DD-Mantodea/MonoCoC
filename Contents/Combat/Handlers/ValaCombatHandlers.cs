using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ValaCombatHandlers
    {

        [Register("coc:vala_special_one")]
        public static void Special1(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (ValaEnemy)context.Enemy;

            var player = context.Player;

            var ratio = enemy.MaxHealth <= 0 ? 0 : enemy.Health / enemy.MaxHealth;

            if (ratio > 0.7f)
            {
                context.Append("Combat.Ability.ValaSpecial1.Light".Localized());

                player.TakeLustDamage(5 + player.Libido / 20f);
            }
            else if (ratio > 0.4f)
            {
                context.Append("Combat.Ability.ValaSpecial1.Medium".Localized());

                player.TakeLustDamage(10 + player.Libido / 20f);
            }
            else
            {
                context.Append("Combat.Ability.ValaSpecial1.Heavy".Localized());

                player.TakeLustDamage(30 + player.Libido / 10f);
            }
        }

        [Register("coc:vala_special_two")]
        public static void Special2(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.ValaSpecial2.Text".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.Milk))
                player.CreateStatusEffect(StatusEffectIds.Milk, 5, 0, 0, 0);

            player.TakeLustDamage(5 + player.Libido / 20f);
        }

        [Register("coc:vala_masturbate")]
        public static void Masturbate(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.ValaMasturbate.Text".Localized());

            player.TakeLustDamage(4 + player.Corruption / 10f);
        }
    }
}