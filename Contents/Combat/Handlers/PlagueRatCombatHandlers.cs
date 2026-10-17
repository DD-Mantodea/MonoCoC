using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class PlagueRatCombatHandlers
    {

        [Register("coc:plague_rat_swipe_attack")]
        public static void SwipeAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + 10);

            var hit = false;

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Scenes.PlagueRat.SwipeAttack.ClawLungeLead".Localized());

                    if (Avoid(context, enemy, player).Outcome == AvoidOutcome.Hit)
                    {
                        context.Append("Scenes.PlagueRat.SwipeAttack.ClawRips".Localized());

                        hit = true;
                    }
                    else
                        context.Append("Scenes.PlagueRat.SwipeAttack.ClawMissSwing".Localized());

                    break;

                case 1:
                    context.Append("Scenes.PlagueRat.SwipeAttack.TailFlickLead".Localized());

                    if (Avoid(context, enemy, player).Outcome == AvoidOutcome.Hit)
                    {
                        context.Append("Scenes.PlagueRat.SwipeAttack.WhipSting".Localized());

                        hit = true;
                    }
                    else
                        context.Append("Scenes.PlagueRat.SwipeAttack.ReflexDodge".Localized());

                    break;

                default:
                    context.Append("Scenes.PlagueRat.SwipeAttack.FerociousRunLead".Localized());

                    if (Avoid(context, enemy, player).Outcome == AvoidOutcome.Hit)
                    {
                        context.Append("Scenes.PlagueRat.SwipeAttack.StrikesLand".Localized());

                        hit = true;
                    }
                    else
                        context.Append("Scenes.PlagueRat.SwipeAttack.AggressionHandled".Localized());

                    break;
            }

            if (hit)
                player.TakeDamage(damage);
        }

        [Register("coc:plague_rat_spit")]
        public static void Spit(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.PlagueRat.Spit.SpitGlobLead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.PlagueRat.Spit.SpitMissed".Localized());

                return;
            }

            if (player.Shield.Is(ItemIds.DragonShield) && context.Random.Next(2) == 0)
            {
                context.Append("Scenes.PlagueRat.Spit.DragonShellAbsorb".Localized());

                return;
            }

            context.Append("Scenes.PlagueRat.Spit.SkinIrritation".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.Poison))
                player.CreateStatusEffect(StatusEffectIds.Poison, 0, 1, 0, 0);
        }

        [Register("coc:plague_rat_gnash")]
        public static void Gnash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.PlagueRat.Gnash.ScreechSprintLead".Localized());

            if (Avoid(context, enemy, player).Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.PlagueRat.Gnash.BiteFailed".Localized());

                return;
            }

            context.Append("Scenes.PlagueRat.Gnash.ZigzagLeapGnaw".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack * 1.5f);

            player.TakeDamage(damage);

            if (!player.HasStatusEffect(StatusEffectIds.Poison))
                player.CreateStatusEffect(StatusEffectIds.Poison, 0, 1, 0, 0);

            player.Bleed(enemy);
        }

        public static AvoidResult Avoid(CombatContext context, Enemy enemy, Player player) =>

            CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

        [Register("coc:plague_rat_can_spit")]
        public static bool CanSpit(CombatContext context, CombatState state) =>

            !context.Player.HasStatusEffect(StatusEffectIds.Poison);
    }
}