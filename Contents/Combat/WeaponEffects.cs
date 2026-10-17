using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat
{
    // 原版 items/WeaponEffects.hx：武器命中后的附加效果
    public static class WeaponEffects
    {
        private static readonly Identifier DullahanScythe = new("coc", "dullahan_scythe");

        private static readonly Identifier LustDagger = new("coc", "lust_dagger");

        private static readonly Identifier LethicesWhip = new("coc", "lethices_whip");

        private static readonly Identifier SuccubusWhip = new("coc", "succubus_whip");

        private static readonly Identifier Whip = new("coc", "whip");

        private static readonly Identifier GeodeKnuckle = new("coc", "geode_knuckle");

        private static readonly Identifier FinalArgument = new("coc", "final_argument");

        private static readonly Identifier TerraMastery = new("coc", "TerrestrialFire");

        public static void ApplyAfterHit(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var weaponId = player.Weapon.Id;

            if (weaponId == DullahanScythe)
            {
                context.Write("Combat.WeaponEffects.DullahanDrain".Localized());

                player.HealthChange(-(30 + context.Random.Next(10)));
            }
            else if (weaponId == LustDagger)
                LustPoison(context, 5, 10, "poison");
            else if (weaponId == LethicesWhip)
                CorruptedTease(context, 100, 25, 10);
            else if (weaponId == SuccubusWhip)
                CorruptedTease(context, 50, 20, 15);
            else if (weaponId == Whip)
                LustPoison(context, 5, 12, "coiled");
            else if (weaponId == FinalArgument)
            {
                if (player.Stun(0, 150 - (int)player.Strength))
                    context.Write("Combat.WeaponEffects.StrongRecoil".Localized());
            }
            else if (weaponId == GeodeKnuckle)
            {
                var mastery = player.MasteryLevel(TerraMastery);

                var chance = 5 * Math.Max(0, mastery - 3);

                var drain = Math.Max(0, 15 - 5 * (mastery - 3));

                if (player.HasStatusEffect(StatusEffectIds.TFGeodeKnuckle))
                    drain = 0;

                StunAndBleed(context, chance, chance);

                if (drain > 0)
                {
                    player.ChangeFatigue(drain);

                    if (player.Fatigue >= player.MaxFatigue())
                    {
                        context.Write("Combat.WeaponEffects.SummonedFade".Localized());

                        player.SetUnarmed();
                    }
                }
            }
        }

        private static void StunAndBleed(CombatContext context, int chanceStun, int chanceBleed)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.Stun(context.Random.Next(3), chanceStun))
                context.Write("Combat.WeaponEffects.Stun".Localized());

            if (context.Random.Next(100) <= chanceBleed && !enemy.HasStatusEffect(StatusEffectIds.IzmaBleed))
                if (!enemy.Bleed(player, 3))
                    context.Write("Combat.WeaponEffects.BleedNo".Localized());
                else
                    context.Write("Combat.WeaponEffects.Bleed".Localized());
        }

        private static void CorruptedTease(CombatContext context, int chance, int baseTease, int playerCorRatio)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (context.Random.Next(100) > chance || enemy.LustVulnerability <= 0)
                return;

            context.Write(enemy.Plural

                ? "Combat.WeaponEffects.CorruptTeasePlural".Localized()

                : "Combat.WeaponEffects.CorruptTease".Localized());

            enemy.Lust = Math.Min(enemy.MaxLust,

                enemy.Lust + enemy.LustVulnerability * (baseTease + player.Corruption / playerCorRatio));

            if (context.Random.Next(2) == 0)
            {
                context.Write("Combat.WeaponEffects.CorruptThrill".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 1);
            }
        }

        private static void LustPoison(CombatContext context, int baseTease, int playerCorRatio, string type)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.LustVulnerability <= 0)
                return;

            context.Write(type == "coiled"

                ? "Combat.WeaponEffects.Coiled".Localized()

                : "Combat.WeaponEffects.LustPoison".Localized());

            enemy.Lust = Math.Min(enemy.MaxLust,

                enemy.Lust + enemy.LustVulnerability * (baseTease + player.Corruption / playerCorRatio));
        }
    }
}