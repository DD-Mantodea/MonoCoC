using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5654-5664 endlessFlurry
    public static class PlayerEndlessFlurryHandlers
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

        [Register("coc:can_endless_flurry")]
        public static bool CanEndlessFlurry(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Weapon.IsOneHandedMelee() && WeaponMasteryLevel(player) >= 2;
        }

        [Register("coc:special_endless_flurry")]
        public static void EndlessFlurry(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.EndlessFlurry.Intro".Localized());

            var state = CombatController.Instance!.State;

            var hits = state.FlurryAmount;

            if (hits == 1 && player.HasStatusEffect(StatusEffectIds.ClawStance))
                hits = 2;

            for (var i = 0; i < hits; i++)
            {
                if (enemy.Health <= 0)
                    break;

                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = player, Defender = enemy,

                    DoDodge = true, DoParry = true, DoBlock = true,
                });

                if (avoid.AttackFailed)
                {
                    context.Append("Combat.Special.EndlessFlurry.Miss".Localized());

                    continue;
                }

                var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 0.5f;

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

                context.Append("Combat.Special.EndlessFlurry.Hit".Localized((int)damage));
            }

            if (state.FlurryAmount < 5)
                state.FlurryAmount += 1;
        }
    }
}