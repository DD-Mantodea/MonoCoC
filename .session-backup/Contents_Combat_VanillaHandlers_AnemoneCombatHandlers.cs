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
    // 原版 classes.scenes.npcs.Anemone 的招式（Anemone.hx:10 覆写 eAttack）。
    public static class AnemoneCombatHandlers
    {
        // 原版 eAttack()（Anemone.hx:10）：
        // CombatAttackBuilder().canBlock().canDodge().canCounter().canParry()，
        // 命中后 1 点伤害 + applyVenom(rand(4 + sens / 20) + 1)。
        [Register("coc:anemone_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Anemone.EAttack.GigglingPlayfullyThe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = true,
                DoBlock = true,
                DoCounter = true
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Dodge:
                    context.WriteAction("Combat.Resolve.EnemyMiss".Localized(enemy.ShortName));

                    return;
                case AvoidOutcome.Parry:
                    context.WriteAction("Combat.Resolve.Parry".Localized(enemy.ShortName));

                    return;
                case AvoidOutcome.Block:
                    context.WriteAction("Combat.Resolve.Block".Localized(enemy.ShortName));

                    return;
                case AvoidOutcome.Counter:
                    context.WriteAction("Combat.Resolve.Counter".Localized(enemy.ShortName));

                    return;
            }

            context.Append("Combat.Anemone.EAttack.YouJinkAnd".Localized());

            player.TakeDamage(1);

            AnemoneEnemy.ApplyVenom(context, context.Random.Next(4 + (int)(player.Sensitivity / 20)) + 1);
        }
    }
}
