using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using System.Linq;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SuccubusStatueEnemy>("coc:succubus_statue")]
    public class SuccubusStatueEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

                    CreateBreastRow(0, 1);

            Tallness = 6 * 12;

            WeaponName = "nothing";

            ArmorName = "cracked stone";
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            var master = context.Enemies.FirstOrDefault(e => e is ArchitectJeremiahEnemy);

            if (master == null || master.Health <= 0)
            {
                context.Append("Combat.Hel.SuccubusInert".Localized());

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
                context.Append("Combat.Hel.SuccubusLustpocalypse".Localized());

                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = this, Defender = context.Player,

                    DoDodge = true, DoParry = true, DoBlock = false, DoFatigue = false
                });

                if (avoid.Outcome == AvoidOutcome.Hit)
                    if (context.Player.Shield != null && context.Player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(3) == 0)
                        context.Append("Combat.Hel.StatueShield".Localized());
                    else
                    {
                        context.Append("Combat.Hel.SuccubusLustpocalypseHit".Localized());

                        context.Player.TakeLustDamage(60 + context.Player.Libido / 10f + context.Player.Sensitivity / 10f);
                    }

                context.Append("Combat.Hel.SuccubusAfter".Localized());

                Health = MaxHealth / 2;

                return true;
            }

            context.Append("Combat.Hel.SuccubusRebuild".Localized());

            return true;
        }

        public override bool HandleFear(CombatContext context) => true;

    }
}