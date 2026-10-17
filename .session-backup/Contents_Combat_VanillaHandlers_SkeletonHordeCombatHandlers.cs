using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 SkeletonHorde（scenes/dungeons/manor/SkeletonHorde.hx）没有独立招式：
    // performCombatAction 只做「按 unitAmount 连续 eAttack + 随机换武器」，
    // 这部分直接实现在 Contents/Enemies/Vanilla/SkeletonHordeEnemy.cs 的 BeforeCombatAction 里，
    // 因此这里没有 handler。保留空类是为了和 monster.mjs 的产物路径一致。
    public static class SkeletonHordeCombatHandlers
    {
    }
}
