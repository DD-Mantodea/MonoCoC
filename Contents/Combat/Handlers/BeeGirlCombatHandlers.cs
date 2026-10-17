using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BeeGirlCombatHandlers
    {
        [Register("coc:bee_girl_attack")]
        public static void BasicAttack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:bee_sting")]
        [Register("coc:bee_girl_bee_sting_attack")]
        public static void Sting(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                var dodges = new[]
                {
                    "Combat.BeeGirl.Sting.Dodge1",

                    "Combat.BeeGirl.Sting.Dodge2",

                    "Combat.BeeGirl.Sting.Dodge3"
                };

                context.WriteAction(dodges[context.Random.Next(dodges.Length)].Localized());

                return;
            }

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.WriteAction("Combat.BeeGirl.Sting.Blocked".Localized());

                return;
            }

            if (player.ArmorDefense >= 10 && context.Random.Next(4) > 0)
            {
                context.WriteAction("Combat.BeeGirl.Sting.Armor".Localized());

                return;
            }

            if (context.Random.Next(2) == 0)
                LustVenom(context);
            else
                ParalyzeVenom(context);

        }

        public static void LustVenom(CombatContext context)
        {
            var player = context.Player;

            context.WriteAction("Combat.BeeGirl.Sting.Lust.Text".Localized());

            if (player.Gender == Gender.Male)
                context.WriteAction("Combat.BeeGirl.Sting.Lust.Male".Localized());
            else if (player.Gender == Gender.Female)
                context.WriteAction("Combat.BeeGirl.Sting.Lust.Female".Localized());
            else if (player.Gender == Gender.Hermaphrodite)
                context.WriteAction("Combat.BeeGirl.Sting.Lust.Herm".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 25);

            if (player.LustRatio() > 0.6f)
            {
                context.WriteAction("Combat.BeeGirl.Sting.Lust.High".Localized());

                if (player.Gender is Gender.Male or Gender.Hermaphrodite)
                    context.WriteAction("Combat.BeeGirl.Sting.Lust.HighMale".Localized());
                else if (player.Gender == Gender.Female)
                    context.WriteAction("Combat.BeeGirl.Sting.Lust.HighFemale".Localized());

                if (player.Sensitivity > 50)
                    context.WriteAction("Combat.BeeGirl.Sting.Lust.Nipples".Localized());
            }
            else
                context.WriteAction("Combat.BeeGirl.Sting.Lust.Clear".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.LustVenom))
                player.CreateStatusEffect(StatusEffectIds.LustVenom);
        }

        public static void ParalyzeVenom(CombatContext context)
        {
            var player = context.Player;

            context.WriteAction("Combat.BeeGirl.Sting.Paralyze.Text".Localized());

            var status = player.StatusEffectByType(StatusEffectIds.ParalyzeVenom);

            if (status == null)
            {
                status = player.CreateStatusEffect(StatusEffectIds.ParalyzeVenom, 2);

                var source = new StatusSource(StatusEffectIds.ParalyzeVenom, "Paralyze Venom");

                player.Stats.AddModifier(GameStats.Strength, ModifierOperation.Addition,

                    () => -status.Values[0], source);

                player.Stats.AddModifier(GameStats.Speed, ModifierOperation.Addition,

                    () => -status.Values[0], source);

                context.WriteAction("Combat.BeeGirl.Sting.Paralyze.New".Localized());
            }
            else
            {
                status.Values[0] += 3;

                context.WriteAction("Combat.BeeGirl.Sting.Paralyze.Worsen".Localized());
            }
        }
    }
}