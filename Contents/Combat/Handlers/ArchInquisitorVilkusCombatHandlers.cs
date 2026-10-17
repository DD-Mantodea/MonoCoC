using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class ArchInquisitorVilkusCombatHandlers
    {
        [Register("coc:arch_inquisitor_vilkus_whitefire")]
        public static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusWhitefire".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Hel.VilkusDodged".Localized());
            else if (context.Player.Shield != null && context.Player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
                context.Append("Combat.Hel.VilkusShieldAbsorb".Localized());
            else
            {
                context.Append("Combat.Hel.VilkusWhitefireHit".Localized());

                context.DealDamageToPlayer((100 + Random.Shared.Next(50)) * (1 + context.Player.Corruption * 0.01f));
            }

            context.Enemy.HealthChange(-75);

            context.Append("Combat.Hel.VilkusSelfDamage".Localized(75));
        }

        [Register("coc:arch_inquisitor_vilkus_blind")]
        public static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusBlind".Localized());

            if (Random.Shared.Next(Math.Max(1, (int)context.Player.Intelligence / 6)) <= 4)
            {
                context.Player.CreateStatusEffect(StatusEffectIds.Blind, 1 + Random.Shared.Next(3), 0, 0, 0);

                context.Append("Combat.Hel.VilkusBlinded".Localized());
            }
            else
                context.Append("Combat.Hel.VilkusBlindAvoided".Localized());

            context.Enemy.HealthChange(-25);

            context.Append("Combat.Hel.VilkusSelfDamage".Localized(25));
        }

        [Register("coc:arch_inquisitor_vilkus_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusArouse".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Hel.VilkusDodged".Localized());
            else if (context.Player.Shield != null && context.Player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
                context.Append("Combat.Hel.VilkusShieldAbsorb".Localized());
            else
            {
                context.Append("Combat.Hel.VilkusArouseHit".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesOrgasmed) > 0)
                    context.Player.TakeLustDamage(35 + context.Player.Libido / 5f + Random.Shared.Next(Math.Max(1, (int)context.Player.Corruption / 5)));
                else
                    context.Append("Combat.Hel.VilkusArouseNoMemories".Localized());
            }

            context.Enemy.HealthChange(-50);

            context.Append("Combat.Hel.VilkusSelfDamage".Localized(50));
        }

        [Register("coc:arch_inquisitor_vilkus_heal")]
        public static void Heal(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusHeal".Localized());

            var healed = context.Enemy.MaxHealth * (1 + Random.Shared.Next(2)) * 0.1f;

            context.Enemy.Health = Math.Min(context.Enemy.MaxHealth, context.Enemy.Health + healed);

            context.Append("Combat.Hel.VilkusHealed".Localized((int)healed));
        }

        [Register("coc:arch_inquisitor_vilkus_heal_two")]
        public static void Heal2(CombatContext context, CombatAbilityDefinition ability) => Heal(context, ability);

        [Register("coc:arch_inquisitor_vilkus_heal_three")]
        public static void Heal3(CombatContext context, CombatAbilityDefinition ability) => Heal(context, ability);

        [Register("coc:arch_inquisitor_vilkus_charge_weapon")]
        public static void ChargeWeapon(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusChargeWeapon".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.ChargeWeapon, 25, 0, 0, 0);

            context.Enemy.HealthChange(-50);

            context.Append("Combat.Hel.VilkusSelfDamage".Localized(50));
        }

        [Register("coc:arch_inquisitor_vilkus_energy_wave")]
        public static void EnergyWave(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusEnergyWave".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Hel.VilkusDodged".Localized());
            else if (context.Player.Shield != null && context.Player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(2) == 0)
                context.Append("Combat.Hel.VilkusShieldAbsorb".Localized());
            else
            {
                context.Append("Combat.Hel.VilkusEnergyWaveHit".Localized());

                context.Player.Corruption = Math.Max(0, context.Player.Corruption - 5);

                context.DealDamageToPlayer(75 + Random.Shared.Next(30) + context.Player.Corruption / 3f);
            }

            context.Enemy.HealthChange(-100);

            context.Append("Combat.Hel.VilkusSelfDamage".Localized(100));
        }

        [Register("coc:arch_inquisitor_vilkus_sacrifice")]
        public static void Sacrifice(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.VilkusSacrifice".Localized());

            context.Enemy.Lust = 0;

            var damage = 300 + Random.Shared.Next(50);

            context.Enemy.HealthChange(-damage);

            context.Append("Combat.Hel.VilkusSelfDamage".Localized(damage));
        }

        [Register("coc:arch_inquisitor_vilkus_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            if (context.Enemy is ArchInquisitorVilkusEnemy { SecondPhase: true })
            {
                context.Append(("Combat.Hel.VilkusPhaseAttack" + (Random.Shared.Next(3) + 1)).Localized());

                var extra = (int)context.Enemy.StatusEffectValue(StatusEffectIds.Attacks, 0);

                context.Enemy.RemoveStatusEffect(StatusEffectIds.Attacks);

                for (var i = 0; i <= extra; i++)
                    CombatController.Instance!.Resolver.EnemyBasicAttack();

                return;
            }

            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }
    }
}