using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.npcs.Marble 的三个招式（eAttack / Marble.hx:8 / :35）。
    public static class MarbleCombatHandlers
    {
        // 原版 eAttack()：普攻，交给通用解析器（str + weaponAttack、normalAttackAvoid、通用战报）
        [Register("coc:marble_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();

        // 原版 marbleSpecialAttackOne()（Marble.hx:8）：
        // 双手过顶重砸，toHitBonus -20（更容易被闪避），命中伤害 str + 20 + weaponAttack。
        [Register("coc:marble_special_attack_one")]
        private static void SpecialAttackOne(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false,
                DoFatigue = false,
                ToHitChance = player.StandardDodgeFunc(enemy, -20)
            });

            if (avoid.AttackFailed)
            {
                AppendDodge(context, avoid, false);

                return;
            }

            // 原版 reduceDamage(str + 20 + weaponAttack, this, 100)：第三个参数是破甲，
            // C# 的 ReduceDamage 没有破甲项，先按普通减伤结算。
            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 20 + enemy.WeaponAttack);

            if (damage <= 0)
                context.Append("Combat.Marble.SpecialAttackOne.Deflect".Localized());

            if (damage > 0)
            {
                context.Append("Combat.Marble.SpecialAttackOne.Hit".Localized());

                player.TakeDamage(damage);
            }
        }

        // 原版 marbleSpecialAttackTwo()（Marble.hx:35）：
        // 横扫，toHitBonus -30，命中伤害 (str + 40 + weaponAttack) / 2。
        // 原版把「Marble easily hits you...」写在 damage 判定之外，这里照抄（0 伤害时也会出这句）。
        [Register("coc:marble_special_attack_two")]
        private static void SpecialAttackTwo(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false,
                DoFatigue = false,
                ToHitChance = player.StandardDodgeFunc(enemy, -30)
            });

            if (avoid.AttackFailed)
            {
                AppendDodge(context, avoid, true);

                return;
            }

            if (enemy.HasStatusEffect(VanillaStatusEffects.Blind))
                context.Append("Combat.Marble.SpecialAttackTwo.BlindSwing".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 40 + enemy.WeaponAttack);

            damage /= 2;

            if (damage <= 0)
                context.Append("Combat.Marble.SpecialAttackTwo.Deflect".Localized());

            context.Append("Combat.Marble.SpecialAttackTwo.Hit".Localized());

            if (damage > 0)
                player.TakeDamage(damage);
        }

        // 原版 customOutput 表按闪避原因分派：[BLIND] / [SPEED] / [EVADE] / [MISDIRECTION] / [FLEXIBILITY] / [UNHANDLED]。
        // 招式二原版只覆盖 [EVADE]，其余原因走默认文案（这里落到各自的 Dodge* 兜底 key）。
        private static void AppendDodge(CombatContext context, AvoidResult avoid, bool second)
        {
            var key = (second, avoid.DodgeReason) switch
            {
                (false, "Blind") => "Combat.Marble.SpecialAttackOne.DodgeBlind",
                (false, "Evade") => "Combat.Marble.SpecialAttackOne.DodgeEvade",
                (false, "Misdirection") => "Combat.Marble.SpecialAttackOne.DodgeMisdirection",
                (false, "Flexibility") => "Combat.Marble.SpecialAttackOne.DodgeFlexibility",
                (false, _) => "Combat.Marble.SpecialAttackOne.DodgeUnhandled",
                (true, "Blind") => "Combat.Marble.SpecialAttackTwo.DodgeBlind",
                (true, "Evade") => "Combat.Marble.SpecialAttackTwo.DodgeEvade",
                (true, "Misdirection") => "Combat.Marble.SpecialAttackTwo.DodgeMisdirection",
                (true, "Flexibility") => "Combat.Marble.SpecialAttackTwo.DodgeFlexibility",
                _ => "Combat.Marble.SpecialAttackTwo.DodgeUnhandled"
            };

            context.Append(key.Localized());
        }
    }
}
