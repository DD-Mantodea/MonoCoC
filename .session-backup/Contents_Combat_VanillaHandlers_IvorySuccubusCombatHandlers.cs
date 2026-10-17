using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes/scenes/monsters/IvorySuccubus.hx 的九个招式（land 在敌人类里）。
    public static class IvorySuccubusCombatHandlers
    {
        // 原版 lash()（IvorySuccubus.hx:10-36）：三段鞭击台词，命中/格挡/闪避各有专属措辞。
        [Register("coc:ivory_succubus_lash")]
        private static void Lash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            var version = context.Random.Next(3);

            context.Append($"Combat.IvorySuccubus.Lash.Lead{version}".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = true,
                ToHitChance = player.StandardDodgeFunc(enemy, 10)
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Block:
                    context.Append($"Combat.IvorySuccubus.Lash.Block{version}".Localized());
                    break;

                case AvoidOutcome.Dodge:
                    context.Append($"Combat.IvorySuccubus.Lash.Avoid{version}".Localized());
                    break;

                case AvoidOutcome.Hit:
                    context.Append($"Combat.IvorySuccubus.Lash.Hit{version}".Localized());

                    // 原版：player.reduceDamage(str + weaponAttack + Utils.rand(20), this)
                    var damage = CombatCalculator.ReduceDamage(context, player,
                        enemy.Strength + enemy.WeaponAttack + context.Random.Next(20));

                    player.TakeDamage(damage);
                    break;
            }
        }

        // 原版 tease()（IvorySuccubus.hx:93-100）：三选一台词 + 12 + rand(20) 欲望。
        [Register("coc:ivory_succubus_tease")]
        private static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append($"Combat.IvorySuccubus.Tease.Text{context.Random.Next(3)}".Localized());

            context.Player.TakeLustDamage(12 + context.Random.Next(20));
        }

        // 原版 flight()（IvorySuccubus.hx:38-43）
        [Register("coc:ivory_succubus_flight")]
        private static void Flight(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Flight.HerWingsExtend".Localized());

            var enemy = (IvorySuccubusEnemy)context.Enemy;

            enemy.FlightCooldown = 7;
            enemy.CreateStatusEffect(IvorySuccubusEnemy.PermaFly, 0, 0, 0, 0);

            CombatRangeData.MoveDistantSafe(enemy);
        }

        // 原版 blind()（IvorySuccubus.hx:51-59）：33% 被玩家眨眼躲过，否则失明 2+rand(3) 回合。
        [Register("coc:ivory_succubus_blind")]
        private static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Blind.ShePresentsHer".Localized());

            if (context.Random.Next(100) < 33)
            {
                context.Append("Combat.IvorySuccubus.Blind.FortunatelyYouBlinked".Localized());

                return;
            }

            context.Append("Combat.IvorySuccubus.Blind.YouShutYour".Localized());

            context.Player.CreateStatusEffect(VanillaStatusEffects.Blind,
                2 + context.Random.Next(3), 0, 0, 0);
        }

        // 原版 whitefire()（IvorySuccubus.hx:61-65）：火焰伤害 2.5*inte + rand(50)。
        [Register("coc:ivory_succubus_whitefire")]
        private static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Whitefire.WispsOfFire".Localized());

            var damage = 2.5f * context.Enemy.Intelligence + context.Random.Next(50);

            context.MonsterDamageType = Identifier.Parse("coc:fire");
            context.DealDamageToPlayer(damage);
        }

        // 原版 charge()（IvorySuccubus.hx:67-70）
        [Register("coc:ivory_succubus_charge")]
        private static void Charge(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Charge.TheDemonTwirls".Localized());

            context.Enemy.CreateStatusEffect(VanillaStatusEffects.ChargeWeapon, 25, 0, 0, 0);
        }

        // 原版 arouse()（IvorySuccubus.hx:72-75）
        [Register("coc:ivory_succubus_arouse")]
        private static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Arouse.TheDemonessPerforms".Localized());

            context.Player.TakeLustDamage(context.Enemy.Intelligence / 4f + context.Random.Next(15));
        }

        // 原版 attract()（IvorySuccubus.hx:77-84）：二选一台词 + 挂 Attracted(.75)。
        [Register("coc:ivory_succubus_attract")]
        private static void Attract(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append($"Combat.IvorySuccubus.Attract.Text{context.Random.Next(2)}".Localized());
            context.Append("Combat.IvorySuccubus.Attract.Text".Localized());

            // 原版 player.createStatusEffect(StatusEffects.Attracted, .75, 0, 0, 0)：
            // 效果是「欲望抗性降低 25%」，C# 没有欲望抗性系统（Player 没有 lust resistance 字段），
            // 这里只挂标记，等抗性系统接入后再让 .75 生效。
            context.Player.CreateStatusEffect(IvorySuccubusEnemy.Attracted, .75f, 0, 0, 0);
        }

        // 原版 lockUp()（IvorySuccubus.hx:86-91）：伤害 inte/4 + rand(10)、+15 物理疲劳、尝试眩晕。
        [Register("coc:ivory_succubus_lock_up")]
        private static void LockUp(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.IvorySuccubus.LockUp.TheDemonessS".Localized());

            var damage = context.Enemy.Intelligence / 4f + context.Random.Next(10);

            player.TakeDamage(damage);
            player.ChangeFatigue(15, FatigueType.Physical);
            player.Stun();
        }
    }
}
