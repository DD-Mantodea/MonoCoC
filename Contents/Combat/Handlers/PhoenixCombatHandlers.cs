using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PhoenixCombatHandlers
    {

        [Register("coc:phoenix_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:phoenix_double_slash")]
        public static void DoubleSlash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Phoenix.DoubleSlash.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Parry)
                context.AppendLog("Combat.Phoenix.DoubleSlash.Parry".Localized());

            var blocked = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = false, DoParry = false, DoBlock = true
            }).Outcome == AvoidOutcome.Block;

            if (blocked)
            {
                context.AppendLog("Combat.Phoenix.DoubleSlash.Blocked".Localized());

                return;
            }

            context.AppendLog("Combat.Phoenix.DoubleSlash.Hit".Localized());

            for (var i = 0; i < 2; i++)
                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + context.Random.Next(80)));
        }

        [Register("coc:phoenix_fire_breath")]
        public static void FireBreath(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (!enemy.HasStatusEffect(StatusEffectIds.Uber))
            {
                context.AppendLog("Combat.Phoenix.FireBreath.Charge".Localized());

                enemy.CreateStatusEffect(StatusEffectIds.Uber);

                return;
            }

            if (context.PlayerWaited)
                context.AppendLog("Combat.Phoenix.FireBreath.Dodged".Localized());
            else
            {
                context.AppendLog("Combat.Phoenix.FireBreath.Hit".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + 300 + context.Random.Next(250)));
            }

            enemy.RemoveStatusEffect(StatusEffectIds.Uber);
        }

        [Register("coc:phoenix_lust_bang")]
        public static void LustBang(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.AppendLog("Combat.Phoenix.LustBang.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.AppendLog("Combat.Phoenix.LustBang.Miss".Localized());

                return;
            }

            context.AppendLog("Combat.Phoenix.LustBang.HitLead".Localized());

            if (player.IsGenderless())
                context.AppendLog("Combat.Phoenix.LustBang.Genderless".Localized());

            if (player.HasCock())
                context.AppendLog("Combat.Phoenix.LustBang.Cock".Localized());

            if (player.HasVagina())
                context.AppendLog("Combat.Phoenix.LustBang.Vagina".Localized());

            context.AppendLog("Combat.Phoenix.LustBang.Tail".Localized());

            var lust = 30 + context.Random.Next(30);

            player.Lust = Math.Min(player.MaxLust, player.Lust + lust);
        }
    }
}