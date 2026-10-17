using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies.Vanilla;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ImpGangCombatHandlers
    {

        [Register("coc:imp_gang_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var units = context.EnemyAs<ImpGangEnemy>()?.RecalculateUnits() ?? 1;

            context.Append("\n");

            for (var i = 0; i < units; i++)
                CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}