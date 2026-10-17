using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class RiddleDemonCombatHandlers
    {

        [Register("coc:riddle_demon_slash")]
        public static void Slash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RiddleDemon.Slash.TheDemonBears".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                if (avoid.Outcome == AvoidOutcome.Counter)
                    return;

                if (avoid.Outcome is AvoidOutcome.Block or AvoidOutcome.Parry)
                    context.Append("Combat.RiddleDemon.Slash.TurnAside".Localized());
                else if (avoid.Outcome == AvoidOutcome.Dodge && avoid.DodgeReason == "Blind")
                    context.Append("Combat.RiddleDemon.Slash.AvoidBlind".Localized());
                else
                    context.Append("Combat.RiddleDemon.Slash.AvoidDodge".Localized());

                return;
            }

            context.Append("Combat.RiddleDemon.Slash.AndBeforeYou".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.Poison))
                player.CreateStatusEffect(StatusEffectIds.Poison, 0, 2, 0, 0);

            var damage = CombatCalculator.ReduceDamage(context, player,

                enemy.Strength + enemy.WeaponAttack + context.Random.Next(20));

            player.HealthChange(-damage);
        }

        [Register("coc:riddle_demon_feint")]
        public static void Feint(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.RiddleDemon.Feint.TheSlyDemon".Localized());

            var insight = Math.Min(100f, player.Intelligence) - 10;

            if (context.Random.Next(100) < insight)
            {
                context.Append("Combat.RiddleDemon.Feint.ButYouRe".Localized());

                return;
            }

            context.Append("Combat.RiddleDemon.Feint.AllowingHimTo".Localized());

            var armor = player.ArmorDefense;

            player.ArmorDefense = Math.Max(0, armor - 10);

            var damage = CombatCalculator.ReduceDamage(context, player,

                enemy.Strength + enemy.WeaponAttack + context.Random.Next(10));

            player.ArmorDefense = armor;

            player.HealthChange(-damage);
        }

        [Register("coc:riddle_demon_whisper")]
        public static void Whisper(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.RiddleDemon.Whisper.TheDemonDashes".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.Whispered))
                player.CreateStatusEffect(StatusEffectIds.Whispered);

            if (context.Enemy is RiddleDemonEnemy demon)
                demon.SetWhisperCooldown(4);
        }
    }
}