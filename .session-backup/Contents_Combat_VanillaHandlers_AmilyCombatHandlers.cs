using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.npcs.Amily 的招式（Amily.hx:13 / :28 / :49）。
    public static class AmilyCombatHandlers
    {
        // 原版 AmilyVenomDebuff（classes/statusEffects/combat/AmilyVenomDebuff.hx）：
        // 每次施加 -2-rand(5) 力量与 -2-rand(5) 速度，可叠加，战斗结束还原。
        // C# 没有这个状态定义，这里用「一个来源 + 累计值」的修饰层实现同样效果，
        // 并通过 CombatCalculator 的联动表在脱战时撤销（同一来源同时挂两条属性）。
        private const string VenomSource = "AmilyVenom";

        // 原版 eAttack()：普攻
        [Register("coc:amily_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();

        // 原版 doubleAttack()（Amily.hx:13）：createStatusEffect(Attacks, 2) 后 eAttack()。
        // C# 的 EnemyBasicAttack 不读 Attacks 状态（见 PhoenixPlatoonCombatHandlers 的相同处理），
        // 所以这里显式打两次。
        [Register("coc:amily_double_attack")]
        private static void DoubleAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            for (var i = 0; i < 2; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // 原版 amilyDartGo()（Amily.hx:28）：假动作 + 毒镖；命中后施加 AmilyVenom，
        // 若玩家力量与速度都被削到 ≤ 2 则直接打 8999 点「处决」伤害。
        [Register("coc:amily_dart_go")]
        private static void DartGo(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Amily.AmilyDartGo.AmilyDashesAt".Localized());

                return;
            }

            context.Append("Combat.Amily.AmilyDartGo.AmilyDashesAt2".Localized());

            ApplyVenom(context);

            // 原版 player.spe <= 2 && player.str <= 2 → takeDamage(8999)
            if (player.Speed <= 2 && player.Strength <= 2)
            {
                context.Append("Combat.Amily.AmilyDartGo.YouVeBecome".Localized());

                player.TakeDamage(8999);
            }
        }

        // 原版 amilyConcentration()（Amily.hx:49）：进入专注（下一击必闪，挑逗可打断）
        [Register("coc:amily_concentration")]
        private static void Concentration(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Amily.AmilyConcentration.AmilyTakesA".Localized());

            context.Enemy.CreateStatusEffect(VanillaStatusEffects.Concentration, 0, 0, 0, 0);
        }

        // AI 条件（原版 MonsterAI.add 的 when）：!hasStatusEffect(StatusEffects.Concentration)
        [Register("coc:amily_can_concentrate")]
        private static bool CanConcentrate(CombatContext context, CombatState state) =>
            !context.Enemy.HasStatusEffect(VanillaStatusEffects.Concentration);

        // 原版 AmilyVenomDebuff.apply：buffHost(Str(-2 - rand(5)), Spe(-2 - rand(5)))；
        // increase() 再叠加一次。用累计值重写同一来源的修饰层。
        private static void ApplyVenom(CombatContext context)
        {
            var player = context.Player;

            var strengthLoss = CurrentLoss(player, VanillaStats.Strength) + 2 + context.Random.Next(5);
            var speedLoss = CurrentLoss(player, VanillaStats.Speed) + 2 + context.Random.Next(5);

            var source = VenomStatusSource();

            player.Stats.RemoveFrom(source);

            player.Stats.AddModifier(VanillaStats.Strength, ModifierOperation.Addition, () => -strengthLoss, source);
            player.Stats.AddModifier(VanillaStats.Speed, ModifierOperation.Addition, () => -speedLoss, source);

            // CombatCalculator 的脱战清理表是内部字典，只能通过这个入口登记：
            // 传 0 不会改变数值，但会把「AmilyVenom 这一来源」记进联动表，
            // 战斗结束时 ClearLinkedBuffs 会把上面两条修饰一起撤掉。
            CombatCalculator.AddCombatBuff(player, VanillaStats.Strength, 0, VenomSource, VenomStatus());
        }

        // 当前这一来源已经削掉多少（取绝对值）
        private static float CurrentLoss(Creature creature, Identifier stat)
        {
            var source = VenomStatusSource();

            return -creature.Stats.From(source).Where(modifier => modifier.Stat.Id == stat).Sum(modifier => modifier.Resolve());
        }

        // 与 CombatCalculator.AddCombatBuff 内部生成的 id 保持完全一致
        private static StatusSource VenomStatusSource() =>
            new(new Identifier("coc", "combat_buff_" + VenomSource.ToLowerInvariant().Replace(' ', '_')), VenomSource);

        // 原版状态 id（Amily Venom）；C# 的 VanillaStatusEffects 是共享文件，这里只做联动键用
        private static Identifier VenomStatus() => new("coc", "amily_venom");
    }
}
