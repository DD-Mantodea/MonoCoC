using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4062-4190 naggaTease
    public static class PlayerNagaTeaseHandlers
    {
        private static readonly Identifier TeaseMastery = new("coc", "Tease");

        [Register("coc:has_naga_lower_body")]
        public static bool HasNagaLowerBody(CombatContext context, CombatState state) =>

            context.Player.Body.Get<LowerBodyPart>().Id.Path == "naga";

        [Register("coc:special_naga_tease")]
        public static void NagaTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.NagaVenom))
            {
                context.Append("Combat.Special.NagaTease.Venomed".Localized());

                return;
            }

            if (enemy.Gender == Gender.Genderless)
            {
                context.Append("Combat.Special.NagaTease.Genderless".Localized());

                return;
            }

            // 原版 Combat.hx fatigueRecovery()
            player.ChangeFatigue(-1);

            if (player.HasPerk(PerkIds.EnlightenedNinetails) || player.HasPerk(PerkIds.CorruptedNinetails))
                player.ChangeFatigue(-(1 + context.Random.Next(3)));

            var mastery = player.MasteryLevel(TeaseMastery);

            var chance = 60f + mastery * 5f;

            if (player.HasPerk(PerkIds.Seduction))
                chance += 10;

            var bimbo = false;

            if (player.HasPerk(PerkIds.BimboBody))
            {
                chance += 10;

                bimbo = true;
            }

            if (player.HasPerk(PerkIds.BroBody))
            {
                chance += 10;

                bimbo = true;
            }

            if (player.HasPerk(PerkIds.FutaForm))
            {
                chance += 10;

                bimbo = true;
            }

            if (player.HasPerk(PerkIds.SensualLover))
                chance += 2;

            var damage = 6f + context.Random.Next(3);

            if (player.HasPerk(PerkIds.SensualLover))
                damage += 2;

            if (player.HasPerk(PerkIds.Seduction))
                damage += 5;

            // 原版 getBonusStat(teaseDamage)：护甲提供的挑逗伤害加成
            damage += player.Stats.Addition(GameStats.TeaseDamage);

            if (bimbo)
                damage += 5;

            damage += player.Level;

            damage += mastery * 2;

            damage += context.Random.Next(7);

            if (enemy.Id.Path == "demons")
                context.Append("Combat.Special.NagaTease.Demons".Localized());
            else if (enemy.Id.Path == "slime")
                context.Append("Combat.Special.NagaTease.Slime".Localized());
            else if (enemy.Gender == Gender.Male)
                context.Append("Combat.Special.NagaTease.Male".Localized());
            else if (enemy.Gender == Gender.Female)
                context.Append("Combat.Special.NagaTease.Female".Localized());
            else
                context.Append("Combat.Special.NagaTease.Herm".Localized());

            if (context.Random.Next(100) <= chance)
            {
                damage *= 0.9f;

                if (player.HasPerk(PerkIds.HistoryWhore))
                    damage *= 1.15f;

                if (player.HasPerk(PerkIds.AscensionSeduction))
                    damage *= 1 + player.Perkv1(PerkIds.AscensionSeduction) * 0.05f;

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + enemy.LustVulnerability * damage);

                player.MasteryXP(TeaseMastery, 1 + context.Random.Next(9));
            }
            else
            {
                context.Append("Combat.Special.NagaTease.Unimpressed".Localized());

                player.MasteryXP(TeaseMastery, 4 + context.Random.Next(33));
            }

            context.Append("[pg]");

            if (enemy is DullahanEnemy)
            {
                context.Append("Combat.Special.NagaTease.DullahanBite".Localized());

                player.HealthChange(-10);
            }
        }
    }
}