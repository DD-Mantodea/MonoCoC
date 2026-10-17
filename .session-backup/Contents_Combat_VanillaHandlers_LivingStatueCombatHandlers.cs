using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.LivingStatue 的招式（LivingStatue.hx:81-189）。
    // 它的 performCombatAction 是手写的（不走 MonsterAI），选招在 LivingStatueEnemy.ChooseCombatAction。
    public static class LivingStatueCombatHandlers
    {
        // 原版 concussiveBlow()（LivingStatue.hx:81-96）：
        //   player.stun(2, 33, 100, false) 决定是否眩晕；伤害 = 100 * ((inte / player.inte) / 4)，
        //   注释写明是「light magic-type damage」，不经护甲减免（直接 takeDamage）。
        [Register("coc:living_statue_concussive_blow")]
        private static void ConcussiveBlow(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.ConcussiveBlow.TheGiantRaises".Localized());

            if (player.Stun(2, 33, 100, false))
                context.Append("Combat.LivingStatue.ConcussiveBlow.TheVibrationsLeave".Localized());
            else
                context.Append("Combat.LivingStatue.ConcussiveBlow.YouShakeOff".Localized());

            // 原版除法在玩家智力为 0 时会得到 Infinity，这里夹住除数避免除零
            var damage = 100f * (enemy.Intelligence / Math.Max(1f, player.Intelligence) / 4f);

            context.DealDamageToPlayer(damage);
        }

        // 原版 dirtKick()（LivingStatue.hx:98-109）：
        //   1/2 概率致盲（已经瞎了就只出「闭眼」那句），持续 2 回合。
        [Register("coc:living_statue_dirt_kick")]
        private static void DirtKick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.LivingStatue.DirtKick.TheAnimatedSculpture".Localized());

            if (context.Random.Next(2) == 0 && !player.HasStatusEffect(VanillaStatusEffects.Blind))
            {
                player.CreateStatusEffect(VanillaStatusEffects.Blind, 2, 0, 0, 0);

                context.Append("Combat.LivingStatue.DirtKick.YouAreBlinded".Localized());
            }
            else
            {
                context.Append("Combat.LivingStatue.DirtKick.YouCloseYour".Localized());
            }
        }

        // 原版 backhand()（LivingStatue.hx:111-131）：
        //   伤害 = player.reduceDamage(str + weaponAttack)；没伤害或闪开 → 躲开文案；
        //   命中 → 把距离打成 Distant（玩家得跑回来），并 takeDamage。
        [Register("coc:living_statue_backhand")]
        private static void Backhand(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.Backhand.TheMarbleGolem".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0)
            {
                context.Append("Combat.LivingStatue.Backhand.YouSlideUnderneath".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.Backhand.YouSlideUnderneath".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.Backhand.ItChitsYou".Localized());

            // 原版 distance = Distant（LivingStatue.hx:126）
            enemy.Distance = CombatDistance.Distant;

            context.DealDamageToPlayer(damage);
        }

        // 原版 overhandSmash()（LivingStatue.hx:133-149）：
        //   高伤害（175 + reduceDamage）、低命中：先 25% 概率挥空，再走闪避判定。
        [Register("coc:living_statue_overhand_smash")]
        private static void OverhandSmash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.OverhandSmash.RaisingItsHammer".Localized());

            var damage = 175f + CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0 || context.Random.Next(100) < 25)
            {
                context.Append("Combat.LivingStatue.OverhandSmash.YouReAble".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.OverhandSmash.YouReAble".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.OverhandSmash.TheConcussiveStrike".Localized());

            context.DealDamageToPlayer(damage);
        }

        // 原版 disarm()（LivingStatue.hx:151-170）：
        //   闪避判定用 player.standardDodgeFunc(this, -30)（比平时更容易命中）；
        //   命中则没收玩家武器，并给自己（雕像）挂 Disarmed 状态做「已用过」标记。
        [Register("coc:living_statue_disarm")]
        private static void Disarm(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.Disarm.TheAnimatedStatue".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false,
                ToHitChance = player.StandardDodgeFunc(enemy, -30)
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.Disarm.YouManageTo".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.Disarm.YourEquipmentFlies".Localized());

            player.Disarm(player.Weapon);

            // 原版 this.createStatusEffect(StatusEffects.Disarmed, 0, 0, 0, 0)（LivingStatue.hx:166）：
            // 同一个 id 挂在雕像身上当标记，不参与玩家侧的缴械判定。
            enemy.CreateStatusEffect(VanillaStatusEffects.Disarmed);
        }

        // 原版 cycloneStrike()（LivingStatue.hx:172-189）：
        //   伤害 = (175 + reduceDamage) / (rand(3) + 2)，即 2~4 等分；闪避参数与其它招式一致。
        [Register("coc:living_statue_cyclone_strike")]
        private static void CycloneStrike(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.CycloneStrike.TwistingBackThe".Localized());

            var damage = (175f + CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack)) /
                (context.Random.Next(3) + 2);

            if (damage <= 0)
            {
                context.Append("Combat.LivingStatue.CycloneStrike.ByTheGrace".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,
                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.CycloneStrike.ByTheGrace".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.CycloneStrike.YouReSquarely".Localized());

            context.DealDamageToPlayer(damage);
        }
    }
}
