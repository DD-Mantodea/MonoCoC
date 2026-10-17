using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BeautifulSwordFightCombatHandlers
    {

        [Register("coc:beautiful_sword_fight_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

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
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:beautiful_sword_fight_holy_light")]
        public static void HolyLight(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.BeautifulSwordFight.HolyLight.TheSwordFlies".Localized());

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.BeautifulSwordFight.HolyLight.YouCanT".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Blind))
                    player.CreateStatusEffect(StatusEffectIds.Blind, 2, 0, 0, 0);
            }
            else
                context.Append("Combat.BeautifulSwordFight.HolyLight.YouBlinkJust".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BeautifulSwordLevel) >= 4)
            {
                context.Append("Combat.BeautifulSwordFight.HolyLight.TheVeryLight".Localized());

                var damage = (int)(context.Random.Next(50) * 1 + player.Corruption / 100f);

                damage = (int)CombatCalculator.ReduceDamage(context, player, damage);

                player.HealthChange(-damage);
            }
        }

        [Register("coc:beautiful_sword_fight_eviscerate")]
        public static void Eviscerate(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.BeautifulSwordFight.Eviscerate.TheSwordTakes".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Combat.BeautifulSwordFight.Eviscerate.TheSliceStrikes".Localized());

                var damage = (int)(context.Random.Next(50) + enemy.WeaponAttack + player.Corruption / 20f);

                player.HealthChange(-damage);

                if (CoC.Instance.Flags.Get(GameFlagIds.BeautifulSwordLevel) >= 5 &&
                    player.Bleed(enemy))

                    context.Append("Combat.BeautifulSwordFight.Eviscerate.ShortlyThereafterThe".Localized());
            }
            else
                context.Append("Combat.BeautifulSwordFight.Eviscerate.YouManageTo".Localized());
        }

        [Register("coc:beautiful_sword_fight_purge")]
        public static void Purge(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.BeautifulSwordFight.Purge.TheSwordPositions".Localized());

            context.Append("Combat.BeautifulSwordFight.Purge.HeHoldsThe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Combat.BeautifulSwordFight.Purge.TheBeamStrikes".Localized());

                var damage = (int)(100 + context.Random.Next(3) * (player.Corruption / 100f));

                damage = (int)CombatCalculator.ReduceDamage(context, player, damage);

                player.HealthChange(-damage);

                context.Append("Combat.BeautifulSwordFight.Purge.YouFeelYour".Localized());

                player.Corruption = Math.Max(0, player.Corruption - 4);

                player.Lust = Math.Max(0, player.Lust - 20);
            }
            else
                context.Append("Combat.BeautifulSwordFight.Purge.YouManageTo".Localized());
        }
    }
}