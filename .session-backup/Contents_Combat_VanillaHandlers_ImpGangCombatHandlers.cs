using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 ImpGang 的「招式」：performCombatAction 先把 unitAmount 按当前血量重算，
    // 再连打 unitAmount 次 eAttack（src/classes/scenes/camp/ImpGang.hx:22-29）。
    public static class ImpGangCombatHandlers
    {
        // 原版 ImpGang.hx:22-29：
        //   unitAmount = ceil(HP / 100); outputText("\n"); while (unitAmount--) eAttack();
        // C# 的 EnemyBasicAttack 不读 Attacks 状态，所以显式循环（与 PhoenixPlatoon 同一写法）。
        [Register("coc:imp_gang_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var units = context.EnemyAs<ImpGangEnemy>()?.RecalculateUnits() ?? 1;

            context.Append("\n");

            for (var i = 0; i < units; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}
