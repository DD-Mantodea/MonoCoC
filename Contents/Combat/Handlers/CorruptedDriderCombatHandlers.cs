using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class CorruptedDriderCombatHandlers
    {        [Register("coc:corrupted_drider_spider_disarm")]

        public static void SpiderDisarm(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.CorruptedDrider.SpiderDisarm.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Dodge && !player.IsUnarmed())
                player.Disarm(player.Weapon);
        }

        [Register("coc:corrupted_drider_spider_morph_web_attack")]
        public static void SpiderMorphWebAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.CorruptedDrider.SpiderMorphWebAttack.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Dodge)
            {
                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -25, "Web", StatusEffectIds.BasiliskSlow);

                player.CreateStatusEffect(StatusEffectIds.BasiliskSlow, 25);
            }
        }

        [Register("coc:corrupted_drider_spider_silence")]
        public static void SpiderSilence(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge || avoid.Outcome == AvoidOutcome.Block)
                return;

            context.Append("Combat.CorruptedDrider.SpiderSilence.Text".Localized());

            WebSilenceHandlers.Apply(player);
        }

        [Register("coc:corrupted_drider_drider_masturbate")]
        public static void DriderMasturbate(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.CorruptedDrider.Masturbate.Lead".Localized());

            enemy.Lust = Math.Max(0, enemy.Lust - 30);

            context.Append("Combat.CorruptedDrider.Masturbate.Tail".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 10 + player.Libido / 20);
        }

        [Register("coc:corrupted_drider_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:corrupted_drider_drider_kiss")]
        public static void DriderKiss(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoFatigue = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {

                var taunt = context.Random.Next(4) switch
                {
                    0 => "Scenes.CorruptedDrider.DriderKiss.MorselKissPlea",

                    1 => "Scenes.CorruptedDrider.DriderKiss.WhyNotKiss",

                    2 => "Scenes.CorruptedDrider.DriderKiss.StopSquirming",

                    _ => "Scenes.CorruptedDrider.DriderKiss.GlossyLipsPitch"
                };

                context.Append(taunt.Localized());

                return;
            }

            var kiss = player.StatusEffectByType(StatusEffectIds.DriderKiss);

            if (kiss == null)
            {
                context.Append("Scenes.CorruptedDrider.DriderKiss.KissApproach".Localized());

                context.Append(player.Tallness < 72

                    ? "Scenes.CorruptedDrider.DriderKiss.DownWord".Localized()

                    : "Scenes.CorruptedDrider.DriderKiss.OverWord".Localized());

                context.Append("Scenes.CorruptedDrider.DriderKiss.LipVenomKiss".Localized());

                if (player.LustPercent <= 99)
                    context.Append("Scenes.CorruptedDrider.DriderKiss.BurnsWithVenom".Localized());

                player.Teased(10);

                player.CreateStatusEffect(StatusEffectIds.DriderKiss);

                return;
            }

            kiss.Values[0] += 1;

            if (kiss.Values[0] == 1)
            {
                context.Append("Scenes.CorruptedDrider.DriderKiss.MouthBoundKiss".Localized());

                if (player.HasCock())
                    context.Append("Scenes.CorruptedDrider.DriderKiss.ArmorPrecum".Localized(player.SMultiCockDesc()));

                if (player.HasVagina())
                    context.Append("Scenes.CorruptedDrider.DriderKiss.ClitThrobs".Localized());

                player.Teased(15);

                context.Append("Scenes.CorruptedDrider.DriderKiss.BreaksKiss".Localized());

                if (player.LustPercent <= 99)
                    context.Append("Scenes.CorruptedDrider.DriderKiss.RefusesStandGround".Localized());

                return;
            }

            context.Append("Scenes.CorruptedDrider.DriderKiss.VenomTrance".Localized());

            if (!player.HasCock() && !player.HasVagina())
                return;

            context.Append("Scenes.CorruptedDrider.DriderKiss.WetStainLead".Localized());

            var size = player.Wetness() < 3 && player.CumQ() < 200

                ? "Scenes.CorruptedDrider.DriderKiss.SizeSmall"

                : player.Wetness() < 5 && player.CumQ() < 500

                    ? "Scenes.CorruptedDrider.DriderKiss.SizeLarge"

                    : "Scenes.CorruptedDrider.DriderKiss.SizeMassive";

            context.Append(size.Localized());

            context.Append("Scenes.CorruptedDrider.DriderKiss.WetStainStinks".Localized());

            if (player.LustPercent <= 99)
                context.Append("Scenes.CorruptedDrider.DriderKiss.ResistsPullsBack".Localized());

            player.Teased(20);
        }

        [Register("coc:corrupted_drider_approach")]
        public static void Approach(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Enemy.Distance = CombatDistance.Melee;

            context.Append("Combat.CorruptedDrider.Approach".Localized());
        }
    }
}