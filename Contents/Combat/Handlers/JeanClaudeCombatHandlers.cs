using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class JeanClaudeCombatHandlers
    {

        [Register("coc:jean_claude_double_attack")]
        public static void DoubleAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.JeanClaude.DoubleAttack.TheMonstrousBasilisk".Localized());

            for (var i = 0; i < 2; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}