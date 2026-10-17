using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes/scenes/monsters/RhinoBouncer.hx 的三个招式 + 三个抓取技。
    public static class RhinoBouncerCombatHandlers
    {
        // 原版 chargeStart()（RhinoBouncer.hx:25-29）：远程先摆冲锋架势。
        [Register("coc:rhino_bouncer_charge_start")]
        private static void ChargeStart(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.RhinoBouncer.ChargeStart.TheRhinoMorph".Localized());
            context.Append("Combat.RhinoBouncer.ChargeStart.HeCurlsHis".Localized());

            if (context.Enemy is RhinoBouncerEnemy rhino)
                rhino.Charging = true;
        }

        // 原版 chargeGrab()（RhinoBouncer.hx:68-87）：冲锋抓取，按玩家体重决定是否能举起来。
        [Register("coc:rhino_bouncer_charge_grab")]
        private static void ChargeGrab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.ChargeGrab.TheRhinoMorph".Localized());

            var weightRatio = player.WeightRating();

            // 原版 Utils.rand(playerWeightRatio) < str * 1.2（rand 接受 Float）
            if ((float)(Random.Shared.NextDouble() * weightRatio) < enemy.Strength * 1.2f)
            {
                switch (Random.Shared.Next(3))
                {
                    case 0:
                        Headbutt(context);
                        break;
                    case 1:
                        Backbreaker(context);
                        break;
                    default:
                        Piledriver(context);
                        break;
                }
            }
            else
            {
                context.Append("Combat.RhinoBouncer.ChargeGrab.TheMassiveBeast".Localized());
                context.Append("Combat.RhinoBouncer.ChargeGrab.HeSaysPanting".Localized());
            }

            // 原版 MonsterAI.exec 对 ChargingMelee 条目会在 action() 之后 closeDistance(monster)；
            // C# 的 CombatAiInterpreter 没有这一步，所以在招式自身末尾补上。
            CombatRangeData.CloseDistance(enemy);
        }

        // 原版 headbutt()（RhinoBouncer.hx:31-43）
        private static void Headbutt(CombatContext context)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.Headbutt.TheMassiveBeast".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 2);

            player.TakeDamage(damage);

            context.Append("Combat.RhinoBouncer.Headbutt.HeReleasesYou".Localized());

            if (player.Stun(2, 75))
                context.Append("Combat.RhinoBouncer.Headbutt.YouTryTo".Localized());
            else
                context.Append("Combat.RhinoBouncer.Headbutt.ToHisSurprise".Localized());
        }

        // 原版 backbreaker()（RhinoBouncer.hx:45-54）
        private static void Backbreaker(CombatContext context)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.Backbreaker.TheMassiveBeast".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 1.5f);

            player.TakeDamage(damage);

            context.Append("Combat.RhinoBouncer.Backbreaker.HePushesYou".Localized());

            // 原版 player.addStatusEffect(new BackbrokenDebuff(3))（classes/statusEffects/combat/BackbrokenDebuff.hx:7-25）：
            // duration 3，apply 时 buffHost(Spe(-25))，每回合 onCombatRound 里 host.cripple() 并递减。
            // C# 用 coc:backbroken 状态 + 联动 Spe 修饰实现；递减/到期在 RhinoBouncerEnemy.TickStatuses（enemy turn start）。
            var status = player.StatusEffectByType(RhinoBouncerEnemy.BackbrokenStatus);

            if (status == null)
            {
                player.CreateStatusEffect(RhinoBouncerEnemy.BackbrokenStatus, 3);
                CombatCalculator.AddCombatBuff(player, VanillaStats.Speed, -25, "Backbroken",
                    RhinoBouncerEnemy.BackbrokenStatus);
            }
            else
            {
                // 重复命中刷新持续时间（原版 TimedStatusEffect 重新施加的效果）
                status.Values[0] = 3;
            }

            context.Append("Combat.RhinoBouncer.Backbreaker.YourFeelNumb".Localized());
        }

        // 原版 piledriver()（RhinoBouncer.hx:56-66）
        private static void Piledriver(CombatContext context)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.Piledriver.TheMassiveBeast".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength * 2.5f);

            context.Append("Combat.RhinoBouncer.Piledriver.YourHeadCrashes".Localized());

            player.TakeDamage(damage);

            if (player.Health <= 0)
                context.Append("Combat.RhinoBouncer.Piledriver.UnfortunatelyYouCannot".Localized());
        }

        // 原版 dropKick()（RhinoBouncer.hx:89-101）：-25 命中率的飞踢，命中后把自己摔到远距离并眩晕 1 回合。
        [Register("coc:rhino_bouncer_drop_kick")]
        private static void DropKick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.RhinoBouncer.DropKick.TakesADeep".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = true,
                ToHitChance = player.StandardDodgeFunc(enemy, -25)
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Dodge:
                    context.Append("Combat.RhinoBouncer.DropKick.Dodged".Localized());
                    break;
                case AvoidOutcome.Block:
                    context.Append("Combat.RhinoBouncer.DropKick.Blocked".Localized());
                    break;
                case AvoidOutcome.Hit:
                    context.Append("Combat.RhinoBouncer.DropKick.TheMountainOf".Localized());

                    var damage = CombatCalculator.ReduceDamage(context, player,
                        enemy.Strength + enemy.WeaponAttack) * 2;

                    player.TakeDamage(damage);

                    CombatRangeData.MoveMonsterDistant(enemy);
                    break;
            }

            context.Append("Combat.RhinoBouncer.DropKick.TooHeavyAnd".Localized());

            // 原版 this.stun(1, 100, 100, false)
            enemy.Stun(1, 100, 100, false);
        }

        // ── AI 条件：原版 performCombatAction 里 actionChoices.add 的第三个参数 ──

        // chargeStart 条件：distance == Distant
        [Register("coc:rhino_bouncer_at_distant")]
        private static bool AtDistant(CombatContext context, CombatState state) =>
            context.Enemy.Distance == CombatDistance.Distant;

        // chargeGrab 条件：charging
        [Register("coc:rhino_bouncer_charging")]
        private static bool Charging(CombatContext context, CombatState state) =>
            context.Enemy is RhinoBouncerEnemy rhino && rhino.Charging;
    }
}
