using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.monsters.PlagueRat 的三个招式（swipeAttack / spit / gnash）。
    public static class PlagueRatCombatHandlers
    {
        // 原版 swipeAttack()：三分支各掷一次闪避，命中则统一结算 str + weaponAttack + 10 的伤害
        [Register("coc:plague_rat_swipe_attack")]
        private static void SwipeAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + 10);

            var hit = false;

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Scenes.Plaguerat.SwipeAttack.ClawLungeLead".Localized());

                    if (Avoid(context, enemy, player).Outcome == AvoidOutcome.Hit)
                    {
                        context.Append("Scenes.Plaguerat.SwipeAttack.ClawRips".Localized());
                        hit = true;
                    }
                    else
                    {
                        context.Append("Scenes.Plaguerat.SwipeAttack.ClawMissSwing".Localized());
                    }

                    break;

                case 1:
                    context.Append("Scenes.Plaguerat.SwipeAttack.TailFlickLead".Localized());

                    if (Avoid(context, enemy, player).Outcome == AvoidOutcome.Hit)
                    {
                        context.Append("Scenes.Plaguerat.SwipeAttack.WhipSting".Localized());
                        hit = true;
                    }
                    else
                    {
                        context.Append("Scenes.Plaguerat.SwipeAttack.ReflexDodge".Localized());
                    }

                    break;

                default:
                    context.Append("Scenes.Plaguerat.SwipeAttack.FerociousRunLead".Localized());

                    if (Avoid(context, enemy, player).Outcome == AvoidOutcome.Hit)
                    {
                        context.Append("Scenes.Plaguerat.SwipeAttack.StrikesLand".Localized());
                        hit = true;
                    }
                    else
                    {
                        context.Append("Scenes.Plaguerat.SwipeAttack.AggressionHandled".Localized());
                    }

                    break;
            }

            if (hit)
                player.TakeDamage(damage);
        }

        // 原版 spit()：只可闪避；龙鳞盾有一半概率直接吸收，否则挂 Poison
        [Register("coc:plague_rat_spit")]
        private static void Spit(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Scenes.Plaguerat.Spit.SpitGlobLead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.Plaguerat.Spit.SpitMissed".Localized());

                return;
            }

            if (player.Shield.Is(VanillaItems.DragonShield) && context.Random.Next(2) == 0)
            {
                context.Append("Scenes.Plaguerat.Spit.DragonShellAbsorb".Localized());

                return;
            }

            context.Append("Scenes.Plaguerat.Spit.SkinIrritation".Localized());

            if (!player.HasStatusEffect(VanillaStatusEffects.Poison))
                player.CreateStatusEffect(VanillaStatusEffects.Poison, 0, 1, 0, 0);
        }

        // 原版 gnash()：撕咬，str + weaponAttack * 1.5，命中后挂 Poison 并流血
        [Register("coc:plague_rat_gnash")]
        private static void Gnash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Scenes.Plaguerat.Gnash.ScreechSprintLead".Localized());

            if (Avoid(context, enemy, player).Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.Plaguerat.Gnash.BiteFailed".Localized());

                return;
            }

            context.Append("Scenes.Plaguerat.Gnash.ZigzagLeapGnaw".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack * 1.5f);

            player.TakeDamage(damage);

            if (!player.HasStatusEffect(VanillaStatusEffects.Poison))
                player.CreateStatusEffect(VanillaStatusEffects.Poison, 0, 1, 0, 0);

            player.Bleed(enemy);
        }

        // 原版三招共用的闪避参数：doDodge + doParry + doBlock
        private static AvoidResult Avoid(CombatContext context, Enemy enemy, Player player) =>
            CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = true,
                DoBlock = true
            });

        // ── AI 条件 ──

        // 原版 spit 的条件：!player.hasStatusEffect(StatusEffects.Poison)
        [Register("coc:plague_rat_can_spit")]
        private static bool CanSpit(CombatContext context, CombatState state) =>
            !context.Player.HasStatusEffect(VanillaStatusEffects.Poison);
    }
}
