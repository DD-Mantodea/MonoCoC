using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:6874-6905 rend
    public static class PlayerRendHandlers
    {
        private static readonly Identifier AxeMastery = new("coc", "Axe");

        [Register("coc:can_rend")]
        public static bool CanRend(CombatContext context, CombatState state)
        {
            var weapon = context.Player.Weapon;

            return weapon.IsAxe() && context.Player.MasteryLevel(AxeMastery) >= 2;
        }

        [Register("coc:special_rend")]
        public static void Rend(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.Rend.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = true, DoBlock = true,

                ToHitChance = player.StandardDodgeFunc(enemy),
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Special.Rend.Dodge".Localized());

                return;
            }

            if (avoid.Outcome is AvoidOutcome.Parry or AvoidOutcome.Block)
            {
                context.Append("Combat.Special.Rend.Block".Localized());

                return;
            }

            context.Append("Combat.Special.Rend.Hit".Localized());

            if (enemy.Bleed(player, 3, 0.5f))
                context.Append("Combat.Special.Rend.Bleed".Localized());

            context.Append("Combat.Special.Rend.Armor".Localized());

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 1.5f;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            enemy.ArmorDefense = Math.Max(0, enemy.ArmorDefense - player.MasteryLevel(AxeMastery));

            player.MasteryXP(AxeMastery, 10 + context.Random.Next(21));
        }
    }
}