using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 RiddleDemon 的招式（src/classes/scenes/places/mothCave/RiddleDemon.hx:28-65）。
    public static class RiddleDemonCombatHandlers
    {
        // 原版 slash（RiddleDemon.hx:28-49）：
        //   CombatAttackBuilder().canDodge().canBlock().canParry()，闪避/招架/格挡各有措辞；
        //   命中后挂 Poison(2) 并造成 reduceDamage(str + weaponAttack + rand(20))。
        [Register("coc:riddle_demon_slash")]
        private static void Slash(CombatContext context, CombatAbilityDefinition ability)
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

            if (!player.HasStatusEffect(VanillaStatusEffects.Poison))
                player.CreateStatusEffect(VanillaStatusEffects.Poison, 0, 2, 0, 0);

            var damage = CombatCalculator.ReduceDamage(context, player,
                enemy.Strength + enemy.WeaponAttack + context.Random.Next(20));

            player.HealthChange(-damage);
        }

        // 原版 feint（RiddleDemon.hx:51-59）：
        //   rand(100) < player.inte100 - 10 时看穿；否则伤害 reduceDamage(str + weaponAttack + rand(10), this, 10)。
        // 第三个参数是 armorIgnore=10：C# 的 ReduceDamage 没有该参数，临时削 10 点护甲再还原。
        [Register("coc:riddle_demon_feint")]
        private static void Feint(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.RiddleDemon.Feint.TheSlyDemon".Localized());

            // 原版 player.inte100 = 100 * inte / maxStats('inte')；普通玩家 maxStats('inte')=100。
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

        // 原版 whisper（RiddleDemon.hx:61-65）：挂 Whispered，冷却设为 4。
        [Register("coc:riddle_demon_whisper")]
        private static void Whisper(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.RiddleDemon.Whisper.TheDemonDashes".Localized());

            if (!player.HasStatusEffect(VanillaStatusEffects.Whispered))
                player.CreateStatusEffect(VanillaStatusEffects.Whispered);

            if (context.Enemy is RiddleDemonEnemy demon)
                demon.SetWhisperCooldown(4);
        }
    }
}
