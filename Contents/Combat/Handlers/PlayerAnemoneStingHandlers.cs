using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3686-3730 anemoneCalc / anemoneChance / anemoneSting
    public static class PlayerAnemoneStingHandlers
    {
        [Register("coc:has_anemone_hair")]
        public static bool HasAnemoneHair(CombatContext context, CombatState state) =>

            context.Player.Body.Get<HairPart>().Id.Path == "anemone";

        [Register("coc:special_anemone_sting")]
        public static void AnemoneSting(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -20 + player.Body.Get<HairPart>().Length),
            });

            if (avoid.AttackFailed)
            {
                context.Append(enemy.Plural

                    ? "Combat.Special.AnemoneSting.MissPlural".Localized()

                    : "Combat.Special.AnemoneSting.Miss".Localized());

                return;
            }

            context.Append("Combat.Special.AnemoneSting.Intro".Localized());

            context.Append("Combat.Special.AnemoneSting.Swipes".Localized());

            context.Append(enemy.Plural

                ? "Combat.Special.AnemoneSting.VenomPlural".Localized()

                : "Combat.Special.AnemoneSting.Venom".Localized());

            var hairLength = player.Body.Get<HairPart>().Length;

            var charges = (int)Math.Min(hairLength / 12f, 3);

            if (charges < 0)
                charges = 0;

            var extraMax = Math.Min(charges * 2, 4);

            var temp = charges + (extraMax > 0 ? context.Random.Next(extraMax) : 0);

            var damage = 5 * temp + (6 * temp > 0 ? context.Random.Next(6 * temp) : 0) + player.Level * 1.5f;

            if (hairLength >= 12)
                damage += context.Random.Next(1, 3);

            if (hairLength >= 24)
                damage += context.Random.Next(1, 3);

            if (hairLength >= 36)
                damage += 1;

            damage *= enemy.LustVulnerability;

            damage = CombatCalculator.BodyMod(player, damage);

            enemy.Speed -= damage / 2;

            enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + damage);
        }
    }
}