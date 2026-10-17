using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class TentacleBeastCombatHandlers
    {
        [Register("coc:tentacle_attack")]
        [Register("coc:tentacle_beast_tentacle_physical_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.TentacleBeast.TentaclePhysicalAttack.TheShamblingHorrorThrowsItsTentacles".Localized());

            var damage = CombatCalculator.PhysicalDamage(context, context.Enemy, context.Player);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoParry = false,

                DoBlock = false
            });

            if (damage <= 0 || avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.TentacleBeast.TentaclePhysicalAttack.HoweverYouQuicklyEvadeTheClumsy".Localized());

                return;
            }

            context.WriteAction("Combat.TentacleBeast.TentaclePhysicalAttack.TheTentaclesCrashUponYourBody".Localized());

            context.Player.HealthChange(-damage);
        }

        [Register("coc:tentacle_entwine")]
        [Register("coc:tentacle_beast_tentacle_entwine")]
        public static void Entwine(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.TentacleBeast.TentacleEntwine.TheBeastLungesItsTentaclesAt".Localized());

            if (context.Player.HasStatusEffect(StatusEffectIds.TentacleBind))
                return;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = context.Player,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.TentacleBeast.TentacleEntwine.InAnImpressiveDisplayOfGymnastics".Localized());

                return;
            }

            context.WriteAction("Combat.TentacleBeast.TentacleEntwine.WhileYouAttemptToAvoidThe".Localized());

            if (context.Player.HasCock())
                context.WriteAction("Combat.TentacleBeast.TentacleEntwine.TheCreatureHavingImmobilizedYouCoils".Localized());
            else if (context.Player.HasVagina())
                context.WriteAction("Combat.TentacleBeast.TentacleEntwine.TheCreatureQuicklyPositionsALong".Localized());
            else
                context.WriteAction("Combat.TentacleBeast.TentacleEntwine.TheCreatureQuicklyPositionsALong2".Localized());

            var lust = (float)Math.Floor(8 + context.Player.Sensitivity / 20f);

            context.Player.Teased(lust);

            context.Player.CreateStatusEffect(StatusEffectIds.TentacleBind);
        }

            [Register("coc:tentacle_ready")]
        public static bool TentacleReady(CombatContext context, CombatState _) => !context.Enemy.HasStatusEffect(StatusEffectIds.TentacleCoolDown);

}
}