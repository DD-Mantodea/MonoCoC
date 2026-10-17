using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GnollSpearThrowerCombatHandlers
    {

        [Register("coc:gnoll_spear_thrower_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
                return;

            var damage = CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Enemy.WeaponAttack);

            if (damage <= 0)
            {
                WriteZeroDamageText(context, player);

                return;
            }

            context.WriteAction((damage < 10

                ? "Combat.GnollSpearThrower.Attack.Glance"

                : "Combat.GnollSpearThrower.Attack.Hit").Localized());

            player.HealthChange(-damage);
        }

        [Register("coc:gnoll_spear_thrower_hyena_physical_attack")]
        public static void HyenaPhysicalAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var damage = CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Enemy.WeaponAttack);

            if (damage <= 0)
            {
                WriteZeroDamageText(context, player);

                return;
            }

            context.WriteAction((damage < 10

                ? "Combat.GnollSpearThrower.Attack.Glance"

                : "Combat.GnollSpearThrower.Attack.Hit").Localized());

            player.HealthChange(-damage);
        }

        [Register("coc:gnoll_spear_thrower_hyena_javelin_attack")]
        public static void HyenaJavelinAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (context.Enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.WriteAction("Combat.GnollSpearThrower.Javelin.BlindMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoParry = false
            });

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.WriteAction("Combat.GnollSpearThrower.Javelin.Blocked".Localized());

                return;
            }

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction(DodgeText(avoid.DodgeReason, "Combat.GnollSpearThrower.Javelin").Localized());

                return;
            }

            if (player.HasPerk(PerkIds.Resolute) && player.Toughness >= 75)
            {
                context.WriteAction("Combat.GnollSpearThrower.Javelin.Resolute".Localized());

                return;
            }

            var damage = 25 + context.Random.Next(20);

            if (context.Random.Next(3) >= 1)
            {
                context.WriteAction("Combat.GnollSpearThrower.Javelin.Entangled".Localized());

                player.CreateStatusEffect(StatusEffectIds.GnollSpear);
            }
            else
                context.WriteAction("Combat.GnollSpearThrower.Javelin.Hit".Localized());

            player.HealthChange(-damage);
        }

        [Register("coc:gnoll_spear_thrower_hyena_snap_kicku")]
        public static void HyenaSnapKicku(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (context.Enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.WriteAction("Combat.GnollSpearThrower.Kick.BlindMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoParry = false
            });

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.WriteAction("Combat.GnollSpearThrower.Kick.Blocked".Localized());

                return;
            }

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction(DodgeText(avoid.DodgeReason, "Combat.GnollSpearThrower.Kick").Localized());

                return;
            }

            var bust = player.BiggestBreastSize();

            var damage = Math.Min(bust, 20f) + CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength);

            if (damage <= 0)
            {
                context.WriteAction("Combat.GnollSpearThrower.Kick.BlockedHard".Localized());

                return;
            }

            context.WriteAction("Combat.GnollSpearThrower.Kick.Hit".Localized());

            if (bust >= 1)
                context.Append("Combat.GnollSpearThrower.Kick.HitBustyFragment".Localized());

            context.Append("Combat.GnollSpearThrower.Kick.HitTauntFragment".Localized());

            player.HealthChange(-damage);

            player.TakeLustDamage(2);
        }

        [Register("coc:gnoll_spear_thrower_hyena_arousal_attack")]
        public static void HyenaArousalAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var chance = context.Random.Next(150);

            if (player.Corruption + player.Libido > chance + 50)
            {
                context.WriteAction("Combat.GnollSpearThrower.Arousal.Strong".Localized());

                player.TakeLustDamage(25 + player.Libido / 20f + player.Sensitivity / 5f);

                return;
            }

            if (20 + player.Corruption + player.Libido > chance)
            {
                context.WriteAction("Combat.GnollSpearThrower.Arousal.Mild".Localized());

                player.TakeLustDamage(15 + player.Libido / 20f + player.Sensitivity / 5f);

                return;
            }

            context.WriteAction("Combat.GnollSpearThrower.Arousal.Failed".Localized());
        }

        public static void WriteZeroDamageText(CombatContext context, Player player)
        {
            var absorbed = context.Random.Next((int)(player.ArmorDefense + player.Toughness)) < player.ArmorDefense;

            context.WriteAction((absorbed

                ? "Combat.GnollSpearThrower.Attack.ChargedBlocked"

                : "Combat.GnollSpearThrower.Attack.Deflected").Localized());
        }

        public static string DodgeText(string? reason, string prefix) => reason switch
        {
            "Speed" => prefix + ".DodgeSpeed",

            "Evade" => prefix + ".DodgeEvade",

            "Misdirection" => prefix + ".DodgeMisdirection",

            "Flexibility" => prefix + ".DodgeFlexibility",

            _ => prefix + ".DodgeOther"
        };
    }
}