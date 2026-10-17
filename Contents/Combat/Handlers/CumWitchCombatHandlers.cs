using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class CumWitchCombatHandlers
    {
        [Register("coc:cum_witch_bukkake_attack")]
        public static void Bukkake(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (CumWitchEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.CumWitchBukkake.Text".Localized());

            var hits = 2 + Random.Shared.Next(5);

            var damage = 0;

            for (var index = 0; index < hits; index++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy, Defender = player,

                    DoDodge = true, DoParry = false, DoBlock = false, DoFatigue = false
                });

                if (avoid.Outcome == AvoidOutcome.Dodge)
                    continue;

                if (player.Shield != null && player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
                    continue;

                damage += 4;

                if (Random.Shared.Next(5) == 4)
                    damage += 6;
            }

            player.TakeLustDamage(damage);
        }

        [Register("coc:cum_witch_cum_magic_attack")]
        public static void CumMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.CumWitchCumMagic.Text".Localized());

            context.Player.TakeLustDamage(5 + context.Player.Libido / 12f);

            context.Player.HoursSinceCum += 100;
        }

        [Register("coc:cum_witch_cum_hunger_attack")]
        public static void CumHunger(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.CumWitchCumHunger.Text".Localized());

            if (context.Player.IsPregnant())
            {
                context.Append("Combat.Ability.CumWitchCumHunger.Immune".Localized());

                return;
            }

            context.Player.TakeLustDamage(5 + context.Player.Libido / 12f);
        }

        [Register("coc:cum_witch_gender_confusion_attack")]
        public static void GenderConfusion(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.CumWitchGenderConfusion.Text".Localized());

            if (player.Intelligence / 5 + Random.Shared.Next(20) + player.Level / 2 < 18)
            {
                context.Append("Combat.Ability.CumWitchGenderConfusion.Stunned".Localized());

                player.CreateStatusEffect(StatusEffectIds.Confusion);
            }
            else
                context.Append("Combat.Ability.CumWitchGenderConfusion.Resisted".Localized());
        }

        [Register("coc:cum_witch_shell_defense")]
        public static void ShellDefense(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.CumWitchShell.Text".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.Shell, 3, 0, 0, 0);

            context.Enemy.Fatigue += 20;
        }

        [Register("coc:cum_witch_cocknosis_attack")]
        public static void Cocknosis(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Ability.CumWitchCocknosis.Text".Localized());

            if (player.Intelligence / 20 + Random.Shared.Next(20) >= 13)
            {
                context.Append("Combat.Ability.CumWitchCocknosis.Resisted".Localized());

                return;
            }

            context.Append("Combat.Ability.CumWitchCocknosis.Hypnotized".Localized());

            player.TakeLustDamage(20);
        }

        [Register("coc:cum_witch_sand_witch_cunt_heals")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.CumWitchHeal.Text".Localized());

            context.Enemy.Health = Math.Min(context.Enemy.MaxHealth, context.Enemy.Health + context.Enemy.MaxHealth * 0.33f);

            context.Enemy.Fatigue += 10;
        }
    }
}