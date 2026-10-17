using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class EffectHandlers
    {

        [Register("coc:effect_lust_stones_round")]
        public static void LustStonesRound(CombatContext context, Creature host, StatusEffect status)
        {
            if (host is not Player player)
                return;

            context.Append(player.IsGoo()

                ? "Combat.Effect.LustStones.GooRound".Localized()

                : "Combat.Effect.LustStones.Round".Localized());

            player.TakeLustDamage(status.Values[0] + 4 + player.Sensitivity / 7f);
        }

        [Register("coc:effect_sandstorm_round")]
        public static void SandstormRound(CombatContext context, Creature host, StatusEffect status)
        {
            host.HealthChange(-3);

            context.Write("Combat.Effect.Sandstorm.Round".Localized());

            if (Random.Shared.Next(3) == 0)
                host.CreateStatusEffect(StatusEffectIds.Blind, 1, 0, 0, 0);
        }

        [Register("coc:effect_poison_round")]
        public static void PoisonRound(CombatContext context, Creature host, StatusEffect status)
        {
            host.HealthChange(-(3 + status.Values[1]));

            context.Write("Combat.Effect.Poison.Round".Localized());
        }

        [Register("coc:effect_acid_slap_round")]
        public static void AcidSlapRound(CombatContext context, Creature host, StatusEffect status)
        {
            var damage = 3 + host.MaxHealth * 0.02f;

            host.HealthChange(-damage);

            context.Write("Combat.Effect.AcidSlap.Round".Localized((int)damage));
        }

        [Register("coc:effect_temporary_heat_round")]
        public static void TemporaryHeatRound(CombatContext context, Creature host, StatusEffect status) =>

            host.Lust = Math.Min(host.MaxLust, host.Lust + 2);

        [Register("coc:effect_bleed_round")]
        public static void BleedRound(CombatContext context, Creature host, StatusEffect status)
        {
            var bleed = host.BleedDamage();

            host.HealthChange(-bleed);

            context.Write("Combat.Bleed.Player".Localized(bleed));
        }

        [Register("coc:effect_bleed_expire")]
        public static void BleedExpire(CombatContext context, Creature host, StatusEffect status) =>

            context.Write("Combat.Bleed.PlayerStopped".Localized());

        [Register("coc:effect_scorpion_venom_round")]
        public static void ScorpionVenomRound(CombatContext context, Creature host, StatusEffect status)
        {
            if (!host.HasPerk(PerkIds.Medicine) || context.Random.Next(100) >= 15)
                return;

            context.Write("Combat.Scorpion.Venom.Cleansed".Localized());

            host.RemoveStatusEffect(status.Id);
        }
    }
}