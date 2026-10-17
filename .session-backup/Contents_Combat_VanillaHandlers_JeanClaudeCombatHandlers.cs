using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.dungeons.lethicesKeep.JeanClaude 的招式（JeanClaude.hx:14-18）。
    public static class JeanClaudeCombatHandlers
    {
        // 原版 doubleAttack()（JeanClaude.hx:14-18）：
        //   outputText("...swings his cutlass at you twice...");
        //   createStatusEffect(StatusEffects.Attacks, 2, 0, 0, 0);
        //   eAttack();
        // eAttack 会按 Attacks 的层数把「一次行动」拆成两段普攻；C# 里对应
        // Resolver.EnemyBasicAttack() 连续结算两次（写法与 ArchInquisitorVilkusCombatHandlers 一致）。
        [Register("coc:jean_claude_double_attack")]
        private static void DoubleAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.JeanClaude.DoubleAttack.TheMonstrousBasilisk".Localized());

            for (var i = 0; i < 2; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}
