using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/manor/BoneCourtier.hx 的招式。
    // 文本 key 来自 Tools/PortKit/out/BoneCourtier.texts.json（落在 Monsters/BoneCourtier.hjson）。
    public static class BoneCourtierCombatHandlers
    {
        // 原版 BoneCourtier.hx:22 temptingGoblet：
        //   combatAvoidDamage({doDodge:true, doParry:false, doBlock:false}) → 四段闪避文本；
        //   命中则 damage = player.reduceDamage(20 + rand(50), this)，再 damage = 20 + player.lib / 10 的欲望伤害。
        [Register("coc:bone_courtier_tempting_goblet")]
        private static void TemptingGoblet(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneCourtier.TemptingGoblet.TheBoneSorcerer".Localized());

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
                // 原版 EVASION_*：Evade / Misdirection / Flexibility / Speed，其它一律落到最后一段。
                context.Append(avoid.DodgeReason switch
                {
                    "Evade" => "Combat.BoneCourtier.TemptingGoblet.YouRollAway".Localized(),
                    "Misdirection" => "Combat.BoneCourtier.TemptingGoblet.YouManageTo".Localized(),
                    "Flexibility" => "Combat.BoneCourtier.TemptingGoblet.YouTwistOut".Localized(),
                    _ => "Combat.BoneCourtier.TemptingGoblet.YouReFast".Localized()
                });

                return;
            }

            context.Append("Combat.BoneCourtier.TemptingGoblet.TheLiquidHits".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 20 + context.Random.Next(50));

            context.Player.TakeDamage(damage);

            context.Player.TakeLustDamage(20 + context.Player.Libido / 10f);
        }

        // 原版 BoneCourtier.hx:48 mark：给玩家挂 Marked(2)。
        [Register("coc:bone_courtier_mark")]
        private static void Mark(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.BoneCourtier.Mark.TheBoneSorcerer".Localized());

            context.Player.CreateStatusEffect(VanillaStatusEffects.Marked, 2, 0, 0, 0);
        }

        // 原版 BoneCourtier.hx:53 heal：
        //   只有自己时治疗自己 25%，否则优先治疗活着的 Necromancer，再不行随机挑一个活着的同伙。
        //   显示的数值原版走 Math.fround（本项目里等价于四舍五入取整），颜色是 colorHpPlus()（暗色主题 #00ff00）。
        [Register("coc:bone_courtier_heal")]
        private static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            const float healPercent = 0.25f;

            context.Append("Combat.BoneCourtier.Heal.TheBoneSorcerer".Localized());

            if (context.Enemies.Count == 1)
            {
                var selfHeal = healPercent * context.Enemy.Health;

                context.Enemy.AddHp(selfHeal);

                context.Append("Combat.BoneCourtier.Heal.TheBoneSorcerer2".Localized("#00ff00", (int)MathF.Round(selfHeal)));

                return;
            }

            // 原版优先找活着的 Necromancer。
            var necromancer = context.Enemies.FirstOrDefault(e => e is NecromancerEnemy && e.Health > 0);

            if (necromancer != null)
            {
                var heal = healPercent * necromancer.Health;

                necromancer.AddHp(heal);

                context.Append("Combat.BoneCourtier.Heal.01Is".Localized(
                    MonoCoC.Utils.Utils.CapitalizeFirstLetter(necromancer.Article),
                    necromancer.ShortName,
                    "#00ff00",
                    (int)MathF.Round(heal)));

                return;
            }

            var candidates = context.Enemies.Where(e => e.Health > 0).ToList();

            if (candidates.Count == 0)
                return;

            var target = candidates[context.Random.Next(candidates.Count)];

            var healAmount = healPercent * target.Health;

            target.AddHp(healAmount);

            context.Append("Combat.BoneCourtier.Heal.01Is".Localized(
                MonoCoC.Utils.Utils.CapitalizeFirstLetter(target.Article),
                target.ShortName,
                "#00ff00",
                (int)MathF.Round(healAmount)));
        }
    }
}
