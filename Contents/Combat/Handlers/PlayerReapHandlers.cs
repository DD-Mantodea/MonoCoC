using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5454-5476 reapDull
    public static class PlayerReapHandlers
    {
        private static readonly Identifier DullahanScythe = new("coc", "dullahan_scythe");

        private static readonly Identifier ScytheMastery = new("coc", "Scythe");

        [Register("coc:has_dullahan_scythe")]
        public static bool HasDullahanScythe(CombatContext context, CombatState state) =>

            context.Player.Weapon != null && context.Player.Weapon.Id == DullahanScythe;

        [Register("coc:special_reap")]
        public static void Reap(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.Reap.Intro".Localized());

            player.HealthChange(-(100 + context.Random.Next(150)));

            context.Append("Combat.Special.Reap.Mid".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -15),
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Special.Reap.Miss1".Localized());

                context.Append("Combat.Special.Reap.Miss2".Localized());

                player.HealthChange(-(400 + context.Random.Next(150)));

                player.MasteryXP(ScytheMastery, 10 + context.Random.Next(11));

                return;
            }

            context.Append("Combat.Special.Reap.Hit".Localized());

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 1.5f;

            damage = CombatCalculator.ReduceDamage(context, enemy, damage);

            if (damage * 2.75f > enemy.Health)
                damage *= 2.75f;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            player.MasteryXP(ScytheMastery, 5 + context.Random.Next(11));

            context.Append("[pg]");
        }
    }
}