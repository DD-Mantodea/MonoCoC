using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class InfestedChameleonGirlCombatHandlers
    {

        [Register("coc:infested_chameleon_girl_chameleon_bizarre_tease")]
        public static void ChameleonBizarreTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.InfestedChameleonGirl.Tease.Lead".Localized());

            context.Append("Combat.InfestedChameleonGirl.Tease.Release".Localized());

            context.Append(player.Gender switch
            {
                Gender.Hermaphrodite => "Combat.InfestedChameleonGirl.Tease.Herm".Localized(),

                Gender.Male => "Combat.InfestedChameleonGirl.Tease.Male".Localized(),

                Gender.Female => "Combat.InfestedChameleonGirl.Tease.Female".Localized(),

                _ => "Combat.InfestedChameleonGirl.Tease.Genderless".Localized()
            });

            if (player.Corruption < 40)
            {
                context.Append("Combat.InfestedChameleonGirl.Tease.Shiver".Localized());

                return;
            }

            context.Append("Combat.InfestedChameleonGirl.Tease.Aroused".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 30);
        }

        [Register("coc:infested_chameleon_girl_goo_blind")]
        public static void GooBlind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.InfestedChameleonGirl.GooBlind.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.InfestedChameleonGirl.GooBlind.Miss".Localized());

                enemy.ChangeFatigue(10, FatigueType.Physical);

                return;
            }

            context.Append("Combat.InfestedChameleonGirl.GooBlind.Hit".Localized());

            player.CreateStatusEffect(StatusEffectIds.Blind, 1 + context.Random.Next(3));

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength / 2 + enemy.WeaponAttack / 2));

            enemy.ChangeFatigue(10, FatigueType.Physical);
        }

        [Register("coc:infested_chameleon_girl_chameleon_claws")]
        public static void ChameleonClaws(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.ChameleonGirl.Claws.Miss".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage > 0)
            {
                context.Append("Combat.ChameleonGirl.Claws.Hit".Localized());

                player.HealthChange(-damage);
            }
            else
                context.Append("Combat.ChameleonGirl.Claws.Absorbed".Localized());
        }

        [Register("coc:infested_chameleon_girl_roll_kick_claw_what_the_fuck_combo_is_this_shit")]
        public static void RollKickClaw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.ChameleonGirl.RollKick.Dodged".Localized());

                player.HealthChange(-(1 + context.Random.Next(10)));

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack) + 25;

            if (damage > 0)
            {
                context.Append("Combat.ChameleonGirl.RollKick.Hit".Localized());

                player.HealthChange(-damage);
            }
            else
                context.Append("Combat.ChameleonGirl.RollKick.Absorbed".Localized());
        }
    }
}