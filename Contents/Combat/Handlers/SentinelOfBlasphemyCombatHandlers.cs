using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class SentinelOfBlasphemyCombatHandlers
    {
        [Register("coc:sentinel_of_blasphemy_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:sentinel_of_blasphemy_seal_magical")]
        public static void SealMagical(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Ability.SentinelOfBlasphemySealMagical.Text".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.SentinelOmniSilence, 3, 0, 0, 0);

            context.Enemy.Fatigue += 15;
        }

        [Register("coc:sentinel_of_blasphemy_dispel")]
        public static void Dispel(CombatContext context, CombatAbilityDefinition ability)
        {
            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            context.Append("Combat.Ability.SentinelOfBlasphemyDispel.Text".Localized());

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Combat.Ability.SentinelOfBlasphemyDispel.Dodged".Localized());
            else if (context.Player.Shield != null && context.Player.Shield.Is(ItemIds.DragonShield) && Random.Shared.Next(3) == 0)
                context.Append("Combat.Ability.SentinelOfBlasphemyDispel.Absorbed".Localized());
            else
            {
                context.Append("Combat.Ability.SentinelOfBlasphemyDispel.Hit".Localized());

                Identifier[] dispellable =

                [
                    StatusEffectIds.TargetMarkedBuff, StatusEffectIds.ChargeWeapon,

                    StatusEffectIds.Might, StatusEffectIds.Leeching,

                    StatusEffectIds.ParasiteQueen, StatusEffectIds.NephilaQueen,

                    StatusEffectIds.Resolve, StatusEffectIds.Lustserking,

                    StatusEffectIds.Berserking, StatusEffectIds.Ironflesh,

                    StatusEffectIds.Overhealing, StatusEffectIds.DivineWind,

                    StatusEffectIds.TFPlate, StatusEffectIds.TFInflame,

                    StatusEffectIds.TFMoltenPlate, StatusEffectIds.TFShell
                ];

                foreach (var status in dispellable)
                    context.Player.RemoveStatusEffect(status);
            }

            context.Enemy.Fatigue += 25;
        }
    }
}