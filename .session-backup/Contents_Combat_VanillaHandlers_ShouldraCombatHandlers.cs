using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.npcs.Shouldra 的三个招式（Shouldra.hx:28 / :59 / :69）。
    public static class ShouldraCombatHandlers
    {
        // 原版 shouldrattack()（Shouldra.hx:28）：可闪避、可招架。
        // 原版 customOutput 七分支按闪避原因分派。
        //
        // 注意：原版 Shouldra.hx:29 起 `var damage:Float = 0;` 之后从未赋值，
        // 所以 player.takeDamage(damage) 恒为 0；damage > 0 的 BONSTRP 加欲分支永远进不去。
        // 这里照抄原版语义，不擅自补伤害公式。
        [Register("coc:shouldra_attack")]
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
                DoBlock = false,
                DoFatigue = false
            });

            if (avoid.AttackFailed)
            {
                AppendDodge(context, avoid);

                return;
            }

            var damage = 0f;

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Combat.Shouldra.Shouldrattack.DuckingInClose".Localized());
                    break;
                case 1:
                    context.Append("Combat.Shouldra.Shouldrattack.TheGirlFeints".Localized());
                    break;
                default:
                    context.Append("Combat.Shouldra.Shouldrattack.YouMomentarilyDrop".Localized());
                    break;
            }

            player.TakeDamage(damage);

            if (damage > 0 && player.Armor.Id == VanillaArmors.BondageStraps)
            {
                var who = enemy.Article + enemy.ShortName;

                context.Append("Combat.Shouldra.Attack.BondageStraps".Localized()
                    char.ToUpperInvariant(who[0]) + who[1..]);

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 5 * enemy.LustVulnerability);
            }
        }

        // 原版 shouldraLustAttack()（Shouldra.hx:59）：两种撩拨台词，伤害 8 + libido / 10。
        [Register("coc:shouldra_lust_attack")]
        private static void LustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            if (context.Random.Next(2) == 0)
            {
                // 原版 (ghostLoli() ? "tiny, girlish" : "firm and moderately large")
                var butt = enemy is ShouldraEnemy { GhostLoli: true }
                    ? "Combat.Shouldra.LustAttack.TinyButt".Localized()
                    : "Combat.Shouldra.LustAttack.FirmButt".Localized();

                context.Append("Combat.Shouldra.ShouldraLustAttack.TheGirlSpins".Localized(butt));
            }
            else
            {
                context.Append("Combat.Shouldra.ShouldraLustAttack.TheGirlS".Localized());
            }

            player.TakeLustDamage(8 + player.Libido / 10f, true);
        }

        // 原版 shouldraMagicLazers()（Shouldra.hx:69）：16 + level + rand(10) 点直接伤害。
        [Register("coc:shouldra_magic_lazers")]
        private static void MagicLazers(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Shouldra.ShouldraMagicLazers.FallingBackA".Localized());

            player.TakeDamage(16 + enemy.Level + context.Random.Next(10));
        }

        // 原版 customOutput（Shouldra.hx:32）：
        // [BLIND]/[SPEED]/[EVADE]/[UNHANDLED] 同一句，[MISDIRECTION]/[FLEXIBILITY]/[PARRY] 各自不同。
        private static void AppendDodge(CombatContext context, AvoidResult avoid)
        {
            var key = avoid.Outcome switch
            {
                AvoidOutcome.Parry => "Combat.Shouldra.Attack.Parry",
                AvoidOutcome.Dodge when avoid.DodgeReason == "Misdirection" => "Combat.Shouldra.Attack.DodgeMisdirection",
                AvoidOutcome.Dodge when avoid.DodgeReason == "Flexibility" => "Combat.Shouldra.Attack.DodgeFlexibility",
                AvoidOutcome.Dodge when avoid.DodgeReason == "Blind" => "Combat.Shouldra.Attack.DodgeBlind",
                AvoidOutcome.Dodge => "Combat.Shouldra.Attack.DodgeUnhandled",
                _ => "Combat.Shouldra.Attack.DodgeUnhandled"
            };

            context.Append(key.Localized());
        }
    }
}
