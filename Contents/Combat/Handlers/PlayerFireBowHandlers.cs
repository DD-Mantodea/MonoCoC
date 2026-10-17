using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4356-4490 fireBowCalc / shootArrow / fireBow
    public static class PlayerFireBowHandlers
    {
        private static readonly Identifier BowMastery = new("coc", "Bow");

        [Register("coc:can_fire_bow")]
        public static bool CanFireBow(CombatContext context, CombatState state) =>

            context.Player.HasKeyItem("Bow") || context.Player.HasKeyItem("Kelt's Bow");

        public static int FireBowChance(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.Blind))
                return 0;

            if (enemy.Id.Path == "worms")
                return 0;

            if (enemy.Id.Path == "pod")
                return 100;

            var chance = player.StandardDodgeFunc(
                enemy,

                player.Speed / 10f + player.Intelligence / 10f + player.MasteryLevel(BowMastery) * 6f - 35f);

            if (player.HasPerk(PerkIds.EagleEye))
                chance *= 1.25f;

            return Math.Clamp((int)chance, 0, 100);
        }

        private static float FireBowDamage(CombatContext context)
        {
            var player = context.Player;

            var mastery = player.MasteryLevel(BowMastery);

            var damage = 20f + player.Strength / 3f + mastery * 25f + player.Speed / 3f;

            if (player.HasPerk(PerkIds.StrongDraw))
                damage += player.Strength / 2f;

            if (damage < 0)
                damage = 0;

            damage *= player.PhysMod();

            if (player.HasKeyItem("Kelt's Bow"))
                damage *= 1.3f;

            return damage;
        }

        // 原版 CombatAbilities.hx:4426 shootArrow
        public static void ShootArrow(CombatContext context, int hitChance, bool fanShot)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Health <= 0)
                return;

            if (player.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Append("Combat.Special.FireBow.Blind".Localized());

                return;
            }

            if (enemy.HasStatusEffect(StatusEffectIds.Sandstorm)
                && context.Random.Next(100) < (player.HasPerk(PerkIds.EagleEye) ? 50 : 90))
            {
                context.Append("Combat.Special.FireBow.Sandstorm".Localized());

                return;
            }

            if (context.Random.Next(100) >= hitChance)
            {
                context.Append("Combat.Special.FireBow.Miss".Localized());

                player.MasteryXP(BowMastery, fanShot ? 1 + context.Random.Next(3) : 5 + context.Random.Next(11));

                return;
            }

            context.Append("Combat.Special.FireBow.Hit".Localized());

            var damage = FireBowDamage(context);

            if (fanShot)
                damage /= 2;

            var miniCrit = false;

            if (enemy.HasStatusEffect(StatusEffectIds.Stunned) && player.HasPerk(PerkIds.KillerInstinct))
            {
                miniCrit = true;

                damage *= 1.3f;
            }

            if (miniCrit)
                context.Append("Combat.Special.FireBow.MiniCrit".Localized());

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Ranged);

            enemy.Lust = Math.Max(0, enemy.Lust - (fanShot ? 5 : 20));
        }

        [Register("coc:special_fire_bow")]
        public static void FireBow(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (player.HasStatusEffect(StatusEffectIds.BowDisabled))
            {
                context.Append("Combat.Special.FireBow.Disabled".Localized());

                return;
            }

            var mastery = player.MasteryLevel(BowMastery);

            context.Append((mastery switch
            {
                0 => "Combat.Special.FireBow.Prep0",

                1 => "Combat.Special.FireBow.Prep1",

                2 => "Combat.Special.FireBow.Prep2",

                3 => "Combat.Special.FireBow.Prep3",

                4 => "Combat.Special.FireBow.Prep4",

                5 => "Combat.Special.FireBow.Prep5",

                _ => "Combat.Special.FireBow.PrepBug",
            }).Localized());

            ShootArrow(context, FireBowChance(context), false);

            player.MasteryXP(BowMastery, 5 + context.Random.Next(11));
        }
    }
}