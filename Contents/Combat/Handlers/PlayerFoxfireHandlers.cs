using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3223-3298
    public static class PlayerFoxfireHandlers
    {
        [Register("coc:has_enlightened_ninetails")]
        public static bool HasEnlightenedNinetails(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.EnlightenedNinetails);

        [Register("coc:has_corrupted_ninetails")]
        public static bool HasCorruptedNinetails(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.CorruptedNinetails);

        private static float BaseDamage(CombatContext context)
        {
            var player = context.Player;

            var damage = 10 + (player.Intelligence / 3f + context.Random.Next((int)Math.Max(1, player.Intelligence / 2))) * player.SpellMod();

            damage = InfernoModifiers.CalcInfernoMod(context, damage);

            damage = CombatCalculator.GlobalMod(player, damage);

            return damage;
        }

        private static bool Silenced(CombatContext context)
        {
            var player = context.Player;

            return player.HasStatusEffect(StatusEffectIds.ThroatPunch)

                || player.HasStatusEffect(StatusEffectIds.WebSilence);
        }

        [Register("coc:spell_foxfire")]
        public static void Foxfire(CombatContext context, CombatAbilityDefinition ability)
        {
            if (Silenced(context))
            {
                context.Append("Combat.Spell.Foxfire.Silence".Localized());

                return;
            }

            var enemy = context.Enemy;

            context.Append("Combat.Spell.Foxfire.Cast".Localized());

            var damage = BaseDamage(context);

            damage *= enemy.Corruption switch
            {
                <= 10 => 0.66f,

                <= 25 => 0.80f,

                <= 50 => 1.00f,

                <= 66 => 1.20f,

                _ => 1.30f,
            };

            damage *= enemy.FireResistance;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            InfernoModifiers.BoostInferno(context);

            if (enemy.Lust >= 1)
            {
                var heal = damage * (enemy.Lust / enemy.MaxLust) * 0.6f;

                context.Append("Combat.Spell.Foxfire.Heal".Localized());

                context.Player.HealthChange(heal);
            }
        }

        [Register("coc:spell_corrupted_foxfire")]
        public static void CorruptedFoxfire(CombatContext context, CombatAbilityDefinition ability)
        {
            if (Silenced(context))
            {
                context.Append("Combat.Spell.Foxfire.Silence".Localized());

                return;
            }

            var enemy = context.Enemy;

            context.Append("Combat.Spell.CorruptedFoxfire.Cast".Localized());

            var damage = BaseDamage(context);

            damage *= enemy.Corruption switch
            {
                >= 66 => 0.66f,

                >= 50 => 0.80f,

                >= 25 => 1.00f,

                >= 10 => 1.20f,

                _ => 1.30f,
            };

            damage *= 1 + enemy.Lust / enemy.MaxLust * 0.5f;

            damage *= enemy.FireResistance;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            InfernoModifiers.BoostInferno(context);
        }
    }
}