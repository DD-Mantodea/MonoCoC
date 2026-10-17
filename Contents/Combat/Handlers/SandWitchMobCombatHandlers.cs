using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class SandWitchMobCombatHandlers
    {
        [Register("coc:sand_witch_mob_sand_storm_attack")]
        public static void SandStormAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SandWitchMobSandStorm.Text".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.Sandstorm);

            context.Player.CreateStatusEffect(StatusEffectIds.Sandstorm);
        }

        [Register("coc:sand_witch_mob_drank_some_mialk")]
        public static void DrankSomeMialk(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SandWitchMobDrankMilk.Text".Localized());

            context.Enemy.Health += 30;

            context.Player.TakeLustDamage(4 + context.Player.Libido / 10f);
        }

        [Register("coc:sand_witch_mob_sandstones_are_cool")]
        public static void SandstonesAreCool(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (SandWitchMobEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.SandWitchMobSandstones.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false, DoFatigue = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Ability.SandWitchMobSandstones.Missed".Localized());

                enemy.RemoveStatusEffect(StatusEffectIds.Sandstorm);

            player.RemoveStatusEffect(StatusEffectIds.Sandstorm);

                player.RemoveStatusEffect(StatusEffectIds.Sandstorm);

                return;
            }

            var bonus = 0;

            if (player.HasCock()) bonus++;
            if (player.HasVagina()) bonus++;
            if (player.Balls > 0) bonus++;
            bonus += Math.Max(0, player.BreastRows.Count - 1);

            player.CreateStatusEffect(StatusEffectIds.LustStones, bonus, 0, 0, 0);

            player.TakeLustDamage(bonus * 2 + 5 + player.Sensitivity / 7f);

            enemy.RemoveStatusEffect(StatusEffectIds.Sandstorm);

            player.RemoveStatusEffect(StatusEffectIds.Sandstorm);
        }

        [Register("coc:sand_witch_mob_gangrush")]
        public static void Gangrush(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = (SandWitchMobEnemy)context.Enemy;

            var player = context.Player;

            context.Append("Combat.Ability.SandWitchMobGangrush.Text".Localized());

            var hits = 2 + Random.Shared.Next(3);

            for (var index = 0; index < hits; index++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy, Defender = player,

                    DoDodge = true, DoParry = false, DoBlock = false, DoFatigue = false
                });

                if (avoid.Outcome != AvoidOutcome.Dodge)
                    player.HealthChange(-(5 + Random.Shared.Next(6)));
            }
        }
    }
}