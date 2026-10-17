using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using System.Linq;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ImpStatueEnemy>("coc:imp_statue")]
    public class ImpStatueEnemy : Enemy
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
                context.Append("Combat.Hel.ImpInert".Localized());

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
                context.Append("Combat.Hel.ImpDebilitate".Localized());

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
                        context.Append("Combat.Hel.ImpDebilitateHit".Localized());

                        if (context.Player.Stun(1, 33))
                            context.Append("Combat.Hel.ImpStun".Localized());

                        if (Random.Shared.Next(3) != 0 && !context.Player.HasStatusEffect(StatusEffectIds.Blind))
                        {
                            context.Player.CreateStatusEffect(StatusEffectIds.Blind, 3, 0, 0, 0);

                            context.Append("Combat.Hel.ImpBlind".Localized());
                        }

                        if (!context.Player.HasStatusEffect(StatusEffectIds.Marked))
                        {
                            context.Player.CreateStatusEffect(StatusEffectIds.Marked, 2, 0, 0, 0);

                            context.Append("Combat.Hel.ImpHex".Localized());
                        }
                    }

                context.Append("Combat.Hel.ImpAfter".Localized());

                Health = MaxHealth / 2;

                return true;
            }

            context.Append("Combat.Hel.ImpRebuild".Localized());

            return true;
        }

        public override bool HandleFear(CombatContext context) => true;

    }
}