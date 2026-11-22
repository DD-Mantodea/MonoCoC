using MonoCoC.Extensions;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ImpCombatHandlers
    {

        [Register("coc:imp_lust_magic")]
        [Register("coc:imp_lust_magic_attack")]
        public static void LustMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Log.Add("Combat.Imp.LustMagic.Gestures".Localized());

            var lust = player.LustPercent;

            context.Log.Add(lust < 30

                ? "Combat.Imp.LustMagic.Warm".Localized()

                : lust < 60

                    ? "Combat.Imp.LustMagic.Arousal".Localized()

                    : "Combat.Imp.LustMagic.Images".Localized());

            if (player.Cocks.Count > 0)
                if (lust >= 60)
                    context.Log.Add("Combat.Imp.LustMagic.PreCum".Localized());
                else if (lust >= 30)
                    context.Log.Add(player.Cocks.Count == 1

                        ? "Combat.Imp.LustMagic.HardenOne".Localized()

                        : "Combat.Imp.LustMagic.HardenMany".Localized());

            if (lust >= 60 && player.Vaginas.Count > 0)
            {
                var many = player.Vaginas.Count > 1;

                var key = player.Vaginas[0].Wetness switch
                {
                    1 => many ? "Combat.Imp.LustMagic.WetNormalMany" : "Combat.Imp.LustMagic.WetNormalOne",

                    2 => "Combat.Imp.LustMagic.WetWet",

                    3 => many ? "Combat.Imp.LustMagic.WetSlickMany" : "Combat.Imp.LustMagic.WetSlickOne",

                    4 => "Combat.Imp.LustMagic.WetDrooling",

                    5 => many ? "Combat.Imp.LustMagic.WetSlaveringMany" : "Combat.Imp.LustMagic.WetSlaveringOne",

                    _ => "" 
                };

                if (key != "")
                    context.Log.Add(key.Localized(player.VaginaDescript(0)));
            }

            player.TakeLustDamage((int)(player.Libido / 10f + player.Corruption / 10f + 10f));
        }

        [Register("coc:imp_attack")]
        public static void Claw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoCounter = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Log.Add("Combat.Imp.Attack.Miss".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Random.Next(5));

            if (damage <= 0)
            {
                context.Log.Add("Combat.Imp.Attack.Blocked".Localized());

                return;
            }

            player.HealthChange(-damage);

            context.Log.Add("Combat.Imp.Attack.Hit".Localized());
        }
    }
}