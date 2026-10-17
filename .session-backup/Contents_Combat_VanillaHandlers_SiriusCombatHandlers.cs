using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 Sirius 的招式（src/classes/scenes/quests/urtaQuest/Sirius.hx:8-75）。
    public static class SiriusCombatHandlers
    {
        // 原版 eAttack（Sirius.hx:8-11）：先出专属前言，再走基底 eAttack；
        // 被闪开时走 outputPlayerDodged 的专属措辞（Sirius.hx:13-15）。
        [Register("coc:sirius_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Sirius.EAttack.SiriusReadiesHis".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Sirius.OutputPlayerDodged.WithYourTrained".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            // SiriusEnemy.OutputAttack 覆写：伤害 > 0 时用专属措辞
            enemy.OutputAttack(context, damage);
        }

        // 原版 manNagaTease（Sirius.hx:34-50）：凝视催眠，失明时伤害更低。
        // 注意原版先掷 rand(10) 出「挣脱」台词，但后面仍然结算凝视（没有 return），照抄。
        [Register("coc:sirius_man_naga_tease")]
        private static void ManNagaTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Sirius.ManNagaTease.TheSnakeMan".Localized());

            if (context.Random.Next(10) == 0)
                context.Append("Combat.Sirius.ManNagaTease.YouBlinkAnd".Localized());

            float lust;

            if (enemy.HasStatusEffect(VanillaStatusEffects.Blind))
            {
                context.Append("Combat.Sirius.ManNagaTease.ThoughYourVision".Localized());

                lust = 5 + player.Libido / 10f - player.Intelligence / 20f;
            }
            else
            {
                context.Append("Combat.Sirius.ManNagaTease.ThosePoolsOf".Localized());

                lust = 10 + player.Libido / 7f - player.Intelligence / 20f;
            }

            player.Lust = Math.Clamp(player.Lust + lust, 0, player.MaxLust);
        }

        // 原版 nagaSpitAttack（Sirius.hx:52-63）：命中则玩家失明 3 回合。
        [Register("coc:sirius_naga_spit_attack")]
        private static void NagaSpitAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Sirius.NagaSpitAttack.HissingLoudlySirius".Localized());

            if (enemy.Speed / 20f + context.Random.Next(20) + 1 > player.Speed / 20f + 10)
            {
                context.Append("Combat.Sirius.NagaSpitAttack.TheVileSpray".Localized());

                if (!player.HasStatusEffect(VanillaStatusEffects.Blind))
                    player.CreateStatusEffect(VanillaStatusEffects.Blind, 3, 0, 0, 0);
            }
            else
            {
                context.Append("Combat.Sirius.NagaSpitAttack.YouQuicklyLean".Localized());
            }
        }

        // 原版 poisonBite（Sirius.hx:65-75）：先掷闪避，但原版闪避分支没有 return，
        // 所以无论是否闪开都会接着输出命中文本并造成 40 + rand(20) 伤害 —— 照原版行为移植。
        [Register("coc:sirius_poison_bite")]
        private static void PoisonBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Sirius.PoisonBite.WithALoud".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
                context.Append("Combat.Sirius.PoisonBite.YouDodgeJust".Localized());

            context.Append("Combat.Sirius.PoisonBite.TheSnakeMan".Localized());

            player.HealthChange(-(40 + context.Random.Next(20)));
        }
    }
}
