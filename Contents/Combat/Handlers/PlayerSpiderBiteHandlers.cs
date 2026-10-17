using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4290-4352 spiderbiteCalc / spiderbiteChance / spiderBiteAttack
    public static class PlayerSpiderBiteHandlers
    {
        [Register("coc:has_spider_fangs")]
        public static bool HasSpiderFangs(CombatContext context, CombatState state) =>

            context.Player.Body.Get<FacePart>().Id.Path == "spider_fangs";

        // 原版 CombatAbilities.hx:158 standardStatComparison
        private static int StandardStatComparison(float attackerStat, float defenderStat, int chanceBonus = 0)
        {
            const float chanceWhenEqual = 70f;

            var c = MathF.Log(-100f / (chanceWhenEqual - 100f));

            if (defenderStat <= 0)
                defenderStat = 1f;

            var statRatio = MathF.Pow(attackerStat / defenderStat, 1.35f);

            var success = (int)MathF.Round(100f * (1f - MathF.Exp(-c * statRatio)));

            return Math.Clamp(success + chanceBonus, 0, 100);
        }

        [Register("coc:special_spider_bite")]
        public static void SpiderBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasPerk(PerkIds.PoisonImmune))
            {
                context.Append("Combat.Special.SpiderBite.PoisonImmune".Localized());

                return;
            }

            if (enemy.HasPerk(PerkIds.BiteImmune))
            {
                context.Append("Combat.Special.SpiderBite.BiteImmune".Localized());

                player.HealthChange(-5);

                return;
            }

            var chance = enemy.HasStatusEffect(StatusEffectIds.Constricted)

                ? 100

                : StandardStatComparison(player.Speed, enemy.Speed, 10 + (player.HasPerk(PerkIds.NaturalWeapons) ? 20 : 0));

            if (chance <= context.Random.Next(100))
            {
                context.Append("Combat.Special.SpiderBite.Fail".Localized());

                return;
            }

            context.Append(enemy.Plural

                ? "Combat.Special.SpiderBite.HitPlural".Localized()

                : "Combat.Special.SpiderBite.Hit".Localized());

            if (enemy.LustVulnerability == 0)
                context.Append("Combat.Special.SpiderBite.NoEffect".Localized());
            else
            {
                if (enemy.Plural)
                    context.Append("Combat.Special.SpiderBite.FlushPlural".Localized());
                else
                    context.Append((enemy.Gender == Gender.Female

                        ? "Combat.Special.SpiderBite.FlushFemale"

                        : "Combat.Special.SpiderBite.FlushMale").Localized());

                var lustDamage = CombatCalculator.BodyMod(player, 25) * enemy.LustVulnerability;

                if (context.Random.Next(5) == 0)
                    lustDamage *= 2;

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + lustDamage);
            }

            context.Append("[pg]");
        }
    }
}