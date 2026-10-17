using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PlayerSpellHandlers
    {

        public static float ChargeWeaponCalc(Player player) =>

            (float)Math.Round(10 * player.SpellMod() * player.SpellbladeBonus());

        [Register("coc:spell_charge_weapon")]
        public static void ChargeWeapon(CombatContext context, CombatAbilityDefinition ability)
        {
            var value = ChargeWeaponCalc(context.Player);

            context.Player.CreateStatusEffect(StatusEffectIds.ChargeWeapon, value);

            context.WriteAction("Combat.Spell.ChargeWeapon.Text".Localized());
        }

        [Register("coc:spell_blind")]
        public static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.Blind.Text".Localized());

            if (context.Random.Next(3) != 0)
            {
                var duration = (int)Math.Round(5 * context.Player.SpellMod());

                if (duration < 1)
                    duration = 1;

                context.Enemy.CreateStatusEffect(StatusEffectIds.Blind, duration, 0, 0, 0, duration);

                context.Append("Combat.Spell.Blind.Success".Localized());
            }
            else
                context.Append("Combat.Spell.Blind.Blinked".Localized());
        }

        public static float WhiteFireDamage(CombatContext context, Player player, Creature target)
        {
            var raw = (float)Math.Floor(10 + player.Intelligence / 3f + context.Random.Next((int)(player.Intelligence / 2f)));

            var damage = (float)Math.Floor(raw * player.SpellMod());

            return Math.Max(0, damage * target.FireResistance);
        }

        [Register("coc:spell_whitefire")]
        public static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.Whitefire.Text".Localized());

            var damage = WhiteFireDamage(context, context.Player, context.Enemy);

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            context.Append("Combat.Spell.Whitefire.Damage".Localized(damage));
        }

        public static float ArouseCalc(CombatContext context, Player player, Creature target)
        {
            var baseValue = (int)(player.Intelligence / 5f * player.SpellMod());

            var modified = (int)Math.Max(1, baseValue + (target.Libido + target.Corruption - target.Intelligence * 2) / 5f);

            var between = context.Random.Next(Math.Min(baseValue, modified), Math.Max(baseValue, modified) + 1);

            return Math.Max(0, target.LustVulnerability * between);
        }

        [Register("coc:spell_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.Arouse.Text".Localized());

            if (context.Enemy.LustVulnerability <= 0)
            {
                context.Append("Combat.Spell.Arouse.NoEffect".Localized());

                return;
            }

            var lust = ArouseCalc(context, context.Player, context.Enemy);

            context.WriteAction(context.Enemy.LustRatio() switch
            {
                < 0.3f => "Combat.Spell.Arouse.Squirms".Localized(),

                < 0.6f => "Combat.Spell.Arouse.Staggers".Localized(),

                _ => "Combat.Spell.Arouse.Glazed".Localized()
            });

            context.Enemy.Teased(lust);

            context.Append("Combat.Spell.Arouse.Damage".Localized(lust));
        }

        public static float HealCalc(CombatContext context, Player player) =>

            (float)Math.Floor((player.Level + player.Intelligence / 1.5f + context.Random.Next((int)player.Intelligence)) * player.SpellMod());

        [Register("coc:spell_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.Heal.Text".Localized());

            if (RollBackfire(context))
            {
                BlackMagicBackfire(context, "Combat.Spell.Heal.Backfire");

                return;
            }

            var healed = HealCalc(context, context.Player);

            context.Player.HealthChange(healed);

            context.Append("Combat.Spell.Heal.Healed".Localized(healed));
        }

        public static float MightCalc(Player player) => (float)Math.Round(10 * player.SpellMod());

        [Register("coc:spell_might")]
        public static void Might(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.Might.Text".Localized());

            if (RollBackfire(context))
            {
                BlackMagicBackfire(context, "Combat.Spell.Might.Backfire");

                return;
            }

            var value = MightCalc(context.Player);

            var source = new StatusSource(StatusEffectIds.Might, "Might");

            context.Player.Stats.AddModifier(GameStats.Strength, ModifierOperation.Addition, () => value, source);

            context.Player.Stats.AddModifier(GameStats.Toughness, ModifierOperation.Addition, () => value, source);

            context.Player.CreateStatusEffect(StatusEffectIds.Might, value);

            context.Append("Combat.Spell.Might.Success".Localized(value));
        }

        public static bool RollBackfire(CombatContext context)
        {
            var backfire = context.Player.HasPerk(PerkIds.FocusedMind) ? 15 : 25;

            return context.Random.Next(100) < backfire;
        }

        public static void BlackMagicBackfire(CombatContext context, string messageKey)
        {
            context.Player.Libido *= 1.25f;

            context.Player.Lust = Math.Min(context.Player.MaxLust, context.Player.Lust + 15);

            context.WriteAction(messageKey.Localized());
        }

        public static float GrayMagicMod(Player player)
        {
            if (player.MaxLust <= 0)
                return 1;

            var coefficient = -100f * player.MaxLust * player.MaxLust;

            var mult = player.Lust * player.Lust - player.MaxLust * player.Lust;

            mult *= 1 / coefficient;

            return 1 + (int)(mult * 100) / 100f;
        }

        public static float TkBlastCalc(CombatContext context, Player player)
        {
            var raw = 15 + (player.Intelligence / 2f + context.Random.Next((int)(player.Intelligence / 2f))) *

                player.SpellMod() * GrayMagicMod(player);

            return Math.Max(1, raw * CombatCalculator.DamagePercent(context, context.Enemy) / 100f);
        }

        [Register("coc:spell_tk_blast")]
        public static void TkBlast(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.TKBlast.Text".Localized());

            var damage = TkBlastCalc(context, context.Player);

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Magical);

            context.Append("Combat.Spell.TKBlast.Damage".Localized(damage));

            if (context.Enemy.Stun(0, context.Random.Next((int)context.Player.Intelligence), (int)context.Enemy.Strength))
                context.Append("Combat.Spell.TKBlast.Stun".Localized());
        }

        public static float LeechCalc(Player player) =>

            20 * player.SpellMod() * GrayMagicMod(player) * player.SpellbladeBonus();

        [Register("coc:spell_leech")]
        public static void Leech(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Spell.Leech.Text".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.Leeching, 3, LeechCalc(context.Player), 0, 0);
        }
    }
}