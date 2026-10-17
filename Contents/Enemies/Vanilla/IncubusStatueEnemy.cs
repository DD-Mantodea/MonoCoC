using MonoCoC.Attributes;
using System.Linq;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<IncubusStatueEnemy>("coc:incubus_statue")]
    public class IncubusStatueEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

                    CreateBreastRow(0, 1);

            Tallness = 6 * 12;

            WeaponName = "scimitar";

            ArmorName = "cracked stone";
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            var master = context.Enemies.FirstOrDefault(e => e is ArchitectJeremiahEnemy);

            if (master == null || master.Health <= 0)
            {
                context.Append("Combat.Hel.IncubusInert".Localized());

                Health = 0;

                return true;
            }

            if (Lust >= MaxLust)
            {
                context.Append("Combat.Hel.StatueLustBreak".Localized());

                Health = MaxHealth / 2;

                Lust = 0;

                return true;
            }

            if (Health >= MaxHealth)
            {
                context.Append("Combat.Hel.IncubusOmnislash".Localized());

                for (var i = 0; i < 6; i++)
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                context.Append("Combat.Hel.IncubusAfter".Localized());

                Health = MaxHealth / 2;

                return true;
            }

            context.Append("Combat.Hel.IncubusRebuild".Localized());

            return true;
        }

        public override bool HandleFear(CombatContext context) => true;

    }
}