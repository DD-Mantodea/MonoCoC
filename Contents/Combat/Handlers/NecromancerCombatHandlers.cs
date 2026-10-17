using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class NecromancerCombatHandlers
    {

        [Register("coc:necromancer_raise_dead")]
        public static void RaiseDead(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Necromancer.RaiseDead.TheNecromancerRaises".Localized());

            var horde = NecromancerEnemy.FindSkellyHorde(context, out _);

            if (horde == null)
                return;

            horde.UnitAmount += 1;

            horde.Health += 100;
        }

        [Register("coc:necromancer_raise_elite")]
        public static void RaiseElite(CombatContext context, CombatAbilityDefinition ability)
        {
            var necromancer = context.EnemyAs<NecromancerEnemy>();

            necromancer?.HasSummonedElite = true;

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

                    context.Append("Combat.Necromancer.RaiseElite.YouHearMore".Localized());

                    context.AddEnemy(EnemyRegistry.Create(new Identifier("coc", "bone_courtier")));

                    break;
            }
        }

        [Register("coc:necromancer_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();
    }
}