using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4249-4266 parasiteReleaseMusk
    public static class PlayerReleaseScentHandlers
    {
        [Register("coc:has_parasite_musk")]
        public static bool HasParasiteMusk(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.ParasiteMusk);

        [Register("coc:musk_active")]
        public static bool MuskActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.ParasiteSlugMusk);

        [Register("coc:special_release_scent")]
        public static void ReleaseScent(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (player.CockTotal() == 1)
                context.Append("Combat.Special.ReleaseScent.Single".Localized());
            else if (player.CockTotal() > 1)
                context.Append("Combat.Special.ReleaseScent.Multiple".Localized());

            context.Append("Combat.Special.ReleaseScent.Dizzy".Localized());

            player.CreateStatusEffect(StatusEffectIds.ParasiteSlugMusk, 4, 0, 0, 0, 99);

            context.Append("[pg]");
        }

        [Register("coc:effect_parasite_slug_musk_round")]
        public static void MuskRound(CombatContext context, Creature host, StatusEffect status)
        {
            var enemy = context.Enemy;

            if (status.Values[0] > 0)
            {
                context.Append("Combat.Special.ReleaseScent.Round".Localized());

                if (enemy.Lust < 50)
                    context.Append("Combat.Special.ReleaseScent.NotMuch".Localized());

                if (enemy.Lust >= 50 && enemy.Lust < 70)
                    context.Append("Combat.Special.ReleaseScent.Bother".Localized());

                if (enemy.Lust >= 70)
                    context.Append("Combat.Special.ReleaseScent.Aroused".Localized());

                if (host.Lust > 90)
                    context.Append("Combat.Special.ReleaseScent.PlayerAroused".Localized());

                status.Values[0] -= 1;

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 10 * enemy.LustVulnerability);

                host.Lust = Math.Min(host.MaxLust, host.Lust + 5);

                context.Append("[pg]");
            }
            else
            {
                host.RemoveStatusEffect(StatusEffectIds.ParasiteSlugMusk);

                context.Append("Combat.Special.ReleaseScent.Fade".Localized());
            }
        }
    }
}