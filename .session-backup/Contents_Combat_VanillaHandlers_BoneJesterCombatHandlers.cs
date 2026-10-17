using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/manor/BoneJester.hx 的招式。
    // 文本 key 来自 Tools/PortKit/out/BoneJester.texts.json（落在 Monsters/BoneJester.hjson）。
    public static class BoneJesterCombatHandlers
    {
        // 原版 eAttack：普通攻击（Monster.hx:940）。
        [Register("coc:bone_jester_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();

        // 原版 BoneJester.hx:33 hideInShadows：
        //   未隐匿 → 只藏进黑暗（hidden = true）；
        //   已隐匿 → 跳出来背刺：damage = player.reduceDamage(250 + rand(100), this)
        [Register("coc:bone_jester_hide_in_shadows")]
        private static void HideInShadows(CombatContext context, CombatAbilityDefinition ability)
        {
            var jester = context.EnemyAs<BoneJesterEnemy>();

            if (jester == null || !jester.Hidden)
            {
                context.Append("Combat.BoneJester.HideInShadows.TheJesterBegins".Localized());

                if (jester != null)
                    jester.Hidden = true;

                return;
            }

            context.Append("Combat.BoneJester.HideInShadows.TheJesterJumps".Localized());

            jester.Hidden = false;

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 250 + context.Random.Next(100));

            context.Player.TakeDamage(damage);
        }

        // 原版 BoneJester.hx:45 harvest：
        //   combatAvoidDamage({doDodge:true, doParry:false, doBlock:false})，
        //   按 dodge 原因分派四段闪避文本；命中则先判 player.bleed(this)，再
        //   damage = player.reduceDamage(25 + rand(30), this)。
        [Register("coc:bone_jester_harvest")]
        private static void Harvest(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneJester.Harvest.TheJesterJumps".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,
                Defender = context.Player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                // 原版 EVASION_*：Evade / Flexibility / Misdirection / Speed，其它一律落到最后一段。
                context.Append(avoid.DodgeReason switch
                {
                    "Evade" => "Combat.BoneJester.Harvest.YouEvadeThe".Localized(),
                    "Flexibility" => "Combat.BoneJester.Harvest.YouTwistYour".Localized(),
                    "Misdirection" => "Combat.BoneJester.Harvest.TheSkeletonMisses".Localized(),
                    _ => "Combat.BoneJester.Harvest.YouDashOut".Localized()
                });

                return;
            }

            // 原版 player.bleed(this)：返回 false 表示玩家有 BleedImmune。
            context.Append(context.Player.Bleed(context.Enemy)
                ? "Combat.BoneJester.Harvest.BeforeYouCan".Localized()
                : "Combat.BoneJester.Harvest.BeforeYouCan2".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 25 + context.Random.Next(30));

            context.Player.TakeDamage(damage);
        }
    }
}
