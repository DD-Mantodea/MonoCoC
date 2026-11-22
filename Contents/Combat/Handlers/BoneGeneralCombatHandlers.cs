using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System.Linq;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BoneGeneralCombatHandlers
    {

        [Register("coc:bone_general_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:bone_general_crushing_blow")]
        public static void CrushingBlow(CombatContext context, CombatAbilityDefinition ability)
        {
            context.AppendLog("Combat.BoneGeneral.CrushingBlow.TheBoneGuardian".Localized());

            context.AppendLog(context.Player.Stun(context.Random.Next(2), 25)

                ? "Combat.BoneGeneral.CrushingBlow.YouLoseYour".Localized()

                : "Combat.BoneGeneral.CrushingBlow.YouManageTo".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 40 + context.Random.Next(20));

            context.Player.TakeDamage(damage);
        }

        [Register("coc:bone_general_guard_ally")]
        public static void GuardAlly(CombatContext context, CombatAbilityDefinition ability)
        {
            context.AppendLog("Combat.BoneGeneral.GuardAlly.TheBoneGuardian".Localized());

            var necromancer = context.Enemies.FirstOrDefault(e => e is NecromancerEnemy && e.Health > 0);

            if (necromancer != null)
            {
                context.AppendLog("Combat.BoneGeneral.GuardAlly.TheNecromancerIs".Localized());

                necromancer.CreateStatusEffect(StatusEffectIds.GuardAB, 2, 0, 0, 0);

                return;
            }

            var candidates = context.Enemies
                .Where(e => e is not BoneGeneralEnemy && e.Health > 0)
                .ToList();

            if (candidates.Count == 0)
                return;

            var target = candidates[context.Random.Next(candidates.Count)];

            context.AppendLog("Combat.BoneGeneral.GuardAlly.OneIs".Localized(
                Utils.Utils.CapitalizeFirstLetter(target.Article),

                target.ShortName,

                target.Pronoun2));

            target.CreateStatusEffect(StatusEffectIds.GuardAB, 2, 0, 0, 0);
        }
    }
}
