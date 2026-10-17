using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class WormMassCombatHandlers
    {
        public static readonly Identifier InfestAttempted = new("coc", "infest_attempted");

        [Register("coc:worm_mass_worm_attack")]
        public static void WormAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

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
                context.Append("Combat.WormMass.WormAttack.TheWormColony".Localized());

                return;
            }

            context.Append("Combat.WormMass.WormAttack.TheWormColony2".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0)
                damage = 1;

            player.HealthChange(-damage);
        }

        [Register("coc:worm_mass_worms_entice")]
        public static void WormsEntice(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (context.Random.Next(2) == 0)
            {
                context.Append((player.LustPercent < 50

                    ? "Combat.WormMass.WormsEntice.TheWormColony"

                    : "Combat.WormMass.WormsEntice.TheWormColony2").Localized());

                return;
            }

            if (player.LustPercent < 50)
            {
                context.Append("Combat.WormMass.WormsEntice.TheWormColony3".Localized());

                player.TakeLustDamage(10 + player.Libido / 20f + player.Corruption / 20f);

                return;
            }

            context.Append("Combat.WormMass.WormsEntice.TheWormColony4".Localized());

            if (player.HasStatusEffect(InfestAttempted))
            {
                context.Append("Combat.WormMass.WormsEntice.StruggleAsYou".Localized());

                SceneInstance<WormsScene>.Get().Infest1();

                return;
            }

            context.Append("Combat.WormMass.WormsEntice.YouStruggleWildly".Localized());

            player.CreateStatusEffect(InfestAttempted);
        }
    }
}