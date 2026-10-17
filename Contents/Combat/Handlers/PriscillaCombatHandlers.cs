using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PriscillaCombatHandlers
    {
        public static float Multiplier(CombatContext context) =>

            1f + 0.5f * context.Player.NewGamePlusMod();

        [Register("coc:priscilla_charge")]
        public static void Charge(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.ChargeWeapon.StaffChargeSpell".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.ChargeWeapon, 25 * Multiplier(context), 0, 0, 0);
        }

        [Register("coc:priscilla_blind")]
        public static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.Blind.BlindingFlash".Localized());

            if (Random.Shared.Next(Math.Max(1, (int)context.Player.Intelligence / 5)) <= 4)
            {
                context.Player.CreateStatusEffect(StatusEffectIds.Blind, 1 + Random.Shared.Next(3), 0, 0, 0);

                context.Append("Scenes.Priscilla.Blind.BlindedStatus".Localized());
            }
            else
                context.Append("Scenes.Priscilla.Blind.BlinkInTime".Localized());
        }

        [Register("coc:priscilla_whitefire")]
        public static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.Whitefire.WhiteFireSpell".Localized());

            var damage = context.Enemy.Intelligence + Random.Shared.Next(50) * Multiplier(context);

            if (context.Player.IsGoo())
            {
                damage *= 1.5f;

                context.Append("Scenes.Priscilla.Whitefire.SuperEffective".Localized());
            }

            context.MonsterDamageType = DamageTypeIds.Fire;

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:priscilla_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.Arouse.LustDrainSpell".Localized());

            var lust = context.Enemy.Intelligence / 10f

                + context.Player.Libido / 10f

                + Random.Shared.Next(10) * Multiplier(context);

            context.Player.TakeLustDamage(lust);
        }

        [Register("coc:priscilla_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.Heal.HealFocusSpell".Localized());

            var heal = (10 + context.Enemy.Intelligence / 2f

                + Random.Shared.Next(Math.Max(1, (int)context.Enemy.Intelligence / 3))) * Multiplier(context);

            context.Enemy.HealthChange(heal);

            context.Append("Scenes.Priscilla.Heal.WoundsKnitHpPlus".Localized("#00ff00", (int)heal));
        }

        [Register("coc:priscilla_might")]
        public static void Might(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.Might.MightEmpowerSpell".Localized());

            context.Append("Scenes.Priscilla.Might.PowerRush".Localized());

            var boost = 20 * Multiplier(context);

            context.Enemy.CreateStatusEffect(StatusEffectIds.Might, boost, boost, 0, 0);

            context.Enemy.Strength += boost;

            context.Enemy.Toughness += boost;
        }

        [Register("coc:priscilla_slash")]
        public static void Slash(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.Slash.SwordChargeSlash".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.Priscilla.Slash.SlashAvoided".Localized());

                return;
            }

            context.Append("Scenes.Priscilla.Slash.SlashConnects".Localized());

            var damage = context.Enemy.Strength + context.Enemy.WeaponAttack + Random.Shared.Next(40);

            damage = CombatCalculator.ReduceDamage(context, context.Player, damage);

            damage = Math.Max(10, damage);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:priscilla_shield_bash")]
        public static void ShieldBash(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Priscilla.ShieldBash.ShieldCharge".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.Priscilla.ShieldBash.BashAvoided".Localized());

                return;
            }

            context.Append("Scenes.Priscilla.ShieldBash.ShieldHits".Localized());

            if (context.Player.Stun(1, 40))
                context.Append("Scenes.Priscilla.ShieldBash.ConcussionStun".Localized());

            var damage = context.Enemy.Strength + Random.Shared.Next(10);

            damage = CombatCalculator.ReduceDamage(context, context.Player, damage);

            damage = Math.Max(10, damage);

            context.DealDamageToPlayer(damage);
        }
    }
}