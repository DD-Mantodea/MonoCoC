using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BoneJesterCombatHandlers
    {

        [Register("coc:bone_jester_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:bone_jester_hide_in_shadows")]
        public static void HideInShadows(CombatContext context, CombatAbilityDefinition ability)
        {
            var jester = context.EnemyAs<BoneJesterEnemy>();

            if (jester == null || !jester.Hidden)
            {
                context.Append("Combat.BoneJester.HideInShadows.TheJesterBegins".Localized());

                jester?.Hidden = true;

                return;
            }

            context.Append("Combat.BoneJester.HideInShadows.TheJesterJumps".Localized());

            jester.Hidden = false;

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 250 + context.Random.Next(100));

            context.Player.TakeDamage(damage);
        }

        [Register("coc:bone_jester_harvest")]
        public static void Harvest(CombatContext context, CombatAbilityDefinition ability)
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

                context.Append(avoid.DodgeReason switch
                {
                    "Evade" => "Combat.BoneJester.Harvest.YouEvadeThe".Localized(),

                    "Flexibility" => "Combat.BoneJester.Harvest.YouTwistYour".Localized(),

                    "Misdirection" => "Combat.BoneJester.Harvest.TheSkeletonMisses".Localized(),

                    _ => "Combat.BoneJester.Harvest.YouDashOut".Localized()
                });

                return;
            }

            context.Append(context.Player.Bleed(context.Enemy)

                ? "Combat.BoneJester.Harvest.BeforeYouCan".Localized()

                : "Combat.BoneJester.Harvest.BeforeYouCan2".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 25 + context.Random.Next(30));

            context.Player.TakeDamage(damage);
        }
    }
}