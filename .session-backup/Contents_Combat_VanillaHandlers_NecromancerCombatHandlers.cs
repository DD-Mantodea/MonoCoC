using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 scenes/dungeons/manor/Necromancer.hx 的招式。
    // 文本 key 来自 Tools/PortKit/out/Necromancer.texts.json（落在 Monsters/Necromancer.hjson）。
    public static class NecromancerCombatHandlers
    {
        // 原版 Necromancer.hx:38 raiseDead
        //   outputText(...); monsterArray[skellyHordeLocation].unitAmount += 1;
        //   .HP += 100; .bonusHP += 100;
        // C# 里 SkeletonHorde 的 MaxHealth 由 unitAmount*100 动态算出，所以 unitAmount+1 已经让上限 +100；
        // 原版 maxHP() 覆写同样忽略 bonusHP，其 +=100 实际是死代码，这里不重复加。
        [Register("coc:necromancer_raise_dead")]
        private static void RaiseDead(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Necromancer.RaiseDead.TheNecromancerRaises".Localized());

            var horde = NecromancerEnemy.FindSkellyHorde(context, out _);

            // 原版 Necromancer.hx:40 直接索引 monsterArray[skellyHordeLocation]，场上没有骷髅时会崩；
            // C# 缺目标时什么都不做，避免空引用。
            if (horde == null)
                return;

            horde.UnitAmount += 1;
            horde.Health += 100;
        }

        // 原版 Necromancer.hx:45 raiseElite
        //   hasSummonedElite = true; 随机 Utils.rand(2) 从 BoneGeneral / BoneJester 里挑一个（case 2 走不到）
        // 原版是 performCombatAction 里直接调用（不是 MonsterAI 的招式），
        // C# 的 ChooseCombatAction 只能返回一个动作，所以包成一条 self 招式。
        [Register("coc:necromancer_raise_elite")]
        private static void RaiseElite(CombatContext context, CombatAbilityDefinition ability)
        {
            var necromancer = context.EnemyAs<NecromancerEnemy>();

            if (necromancer != null)
                necromancer.HasSummonedElite = true;

            context.Append("Combat.Necromancer.RaiseElite.TheNecromancerStabs".Localized());

            switch (context.Random.Next(2))
            {
                case 0:
                    context.Append("Combat.Necromancer.RaiseElite.YouHearThe".Localized());
                    context.AddEnemy(EnemyRegistry.Create(new Identifier("coc", "bone_general")));
                    break;
                case 1:
                    context.Append("Combat.Necromancer.RaiseElite.YouHearThe2".Localized());
                    context.AddEnemy(EnemyRegistry.Create(new Identifier("coc", "bone_jester")));
                    break;
                default:
                    // 原版 case 2（Bone Sorcerer）：Utils.rand(2) 只给 0..1，永远走不到，保留以对齐原文。
                    context.Append("Combat.Necromancer.RaiseElite.YouHearMore".Localized());
                    context.AddEnemy(EnemyRegistry.Create(new Identifier("coc", "bone_courtier")));
                    break;
            }
        }

        // 原版 eAttack：普通攻击（Monster.hx:940）。
        [Register("coc:necromancer_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();
    }
}
