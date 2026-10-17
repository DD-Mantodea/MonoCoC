using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 BeautifulSwordFight 的招式（src/classes/scenes/camp/BeautifulSwordFight.hx:19-67）。
    public static class BeautifulSwordFightCombatHandlers
    {
        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:beautiful_sword_fight_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 holyLight（BeautifulSwordFight.hx:19-33）：50% 失明；剑等级 >=4 时额外按腐化灼伤。
        [Register("coc:beautiful_sword_fight_holy_light")]
        private static void HolyLight(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.BeautifulSwordFight.HolyLight.TheSwordFlies".Localized());

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.BeautifulSwordFight.HolyLight.YouCanT".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.Blind))
                    player.CreateStatusEffect(VanillaStatusEffects.Blind, 2, 0, 0, 0);
            }
            else
            {
                context.Append("Combat.BeautifulSwordFight.HolyLight.YouBlinkJust".Localized());
            }

            // 原版 flags[BEAUTIFUL_SWORD_LEVEL] >= 4
            if (CoC.Instance.Flags.Get(VanillaFlags.BeautifulSwordLevel) >= 4)
            {
                context.Append("Combat.BeautifulSwordFight.HolyLight.TheVeryLight".Localized());

                var damage = (int)(context.Random.Next(50) * 1 + player.Corruption / 100f);

                damage = (int)CombatCalculator.ReduceDamage(context, player, damage);

                player.HealthChange(-damage);
            }
        }

        // 原版 eviscerate（BeautifulSwordFight.hx:35-50）：无视防御的一击；剑等级 >=5 时附加流血。
        [Register("coc:beautiful_sword_fight_eviscerate")]
        private static void Eviscerate(CombatContext context, CombatAbilityDefinition ability)
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

                if (CoC.Instance.Flags.Get(MonoCoC.Contents.Flags.VanillaFlags.BeautifulSwordLevel) >= 5 &&
                    player.Bleed(enemy))
                {
                    context.Append("Combat.BeautifulSwordFight.Eviscerate.ShortlyThereafterThe".Localized());
                }
            }
            else
            {
                context.Append("Combat.BeautifulSwordFight.Eviscerate.YouManageTo".Localized());
            }
        }

        // 原版 purge（BeautifulSwordFight.hx:52-67）：命中后灼烧并削腐化 4、削欲望 20。
        [Register("coc:beautiful_sword_fight_purge")]
        private static void Purge(CombatContext context, CombatAbilityDefinition ability)
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
            {
                context.Append("Combat.BeautifulSwordFight.Purge.YouManageTo".Localized());
            }
        }
    }
}
