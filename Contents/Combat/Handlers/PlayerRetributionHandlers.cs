using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5625-5650 / RetributionBuff.hx
    public static class PlayerRetributionHandlers
    {
        private static readonly Identifier[] WeaponMasteries =

        [
            new("coc", "Sword"), new("coc", "Axe"), new("coc", "Spear"), new("coc", "Dagger"),

            new("coc", "Mace"), new("coc", "Staff"), new("coc", "Whip"), new("coc", "Knife"),
        ];

        private static int WeaponMasteryLevel(Player player)
        {
            var max = 0;

            foreach (var mastery in WeaponMasteries)
                max = Math.Max(max, player.MasteryLevel(mastery));

            return max;
        }

        [Register("coc:can_retribution_arc")]
        public static bool CanRetributionArc(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Weapon.IsLarge() && WeaponMasteryLevel(player) >= 2;
        }

        [Register("coc:special_retribution_arc")]
        public static void RetributionArc(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Special.Retribution.Start".Localized());

            // Values[1] 记录施放时的 HP，回合结束据此估算本回合承受的伤害
            player.CreateStatusEffect(StatusEffectIds.RetributionBuff, 0, player.Health, 0, 0, 1);
        }

        [Register("coc:effect_retribution_expire")]
        public static void Expire(CombatContext context, Creature host, StatusEffect status)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.Retribution.Exec".Localized());

            var taken = Math.Max(0f, status.Values[1] - player.Health);

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy) + taken * 2f;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            player.MasteryXP(new Identifier("coc", "Sword"), 10 + context.Random.Next(21));
        }
    }
}