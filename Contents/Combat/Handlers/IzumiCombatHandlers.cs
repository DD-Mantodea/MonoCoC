using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class IzumiCombatHandlers
    {

        [Register("coc:izumi_straight_jab")]
        public static void StraightJab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Izumi.StraightJab.Lead".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 175);

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Izumi.StraightJab.Dodged".Localized());

                return;
            }

            if (damage <= 0)
            {
                context.Append("Combat.Izumi.StraightJab.Deflected".Localized());

                return;
            }

            context.Append("Combat.Izumi.StraightJab.Hit".Localized());

            context.Append(player.HasTailInsteadOfLegs()

                ? "Combat.Izumi.StraightJab.RiseTail".Localized()

                : "Combat.Izumi.StraightJab.Stand".Localized());

            context.Append("Combat.Izumi.StraightJab.DustOff".Localized());

            player.HealthChange(-damage);
        }

        [Register("coc:izumi_roundhouse_kick")]
        public static void RoundhouseKick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Izumi.RoundhouseKick.Lead".Localized());

            if (player.Corruption >= 50 || player.Libido >= 50 || player.Sensitivity >= 50)
            {
                context.Append("Combat.Izumi.RoundhouseKick.Affected".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 10 + player.Libido / 10);
            }
            else
            {
                context.Append("Combat.Izumi.RoundhouseKick.Confused".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 20);
            }
        }

        [Register("coc:izumi_choke_slam")]
        public static void ChokeSlam(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Izumi.ChokeSlam.Dodged".Localized());

                return;
            }

            context.Append("Combat.Izumi.ChokeSlam.Hit".Localized());

            player.CreateStatusEffect(StatusEffectIds.Chokeslam, 3);
        }

        [Register("coc:izumi_ground_pound")]
        public static void GroundPound(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Izumi.GroundPound.Lead".Localized());

            context.Append("Combat.Izumi.GroundPound.Hit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + 225);

            player.HealthChange(-damage);
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.Titsmother))
            {
                TitSmotherStruggle(context);

                return;
            }

            if (!player.HasStatusEffect(StatusEffectIds.Chokeslam))
                return;

            context.Clear();

            if (context.Random.Next(Math.Max(1, (int)player.Strength)) > enemy.Strength / 2)
            {
                context.Append("Combat.Izumi.Struggle.EscapeLead".Localized());

                if (player.IsBiped())
                    context.Append("Combat.Izumi.Struggle.EscapeBiped".Localized());
                else if (player.HasTailInsteadOfLegs())
                    context.Append("Combat.Izumi.Struggle.EscapeTail".Localized());
                else
                    context.Append("Combat.Izumi.Struggle.EscapeOther".Localized());

                context.Append("Combat.Izumi.Struggle.EscapeDrop".Localized());

                context.Append("Combat.Izumi.Struggle.EscapeAfter".Localized());

                enemy.HealthChange(-(50 + context.Random.Next(Math.Max(1, (int)player.Strength))));

                player.RemoveStatusEffect(StatusEffectIds.Chokeslam);

                return;
            }

            context.Append("Combat.Izumi.Struggle.Fail".Localized());

            context.Append(player.Strength < 90

                ? "Combat.Izumi.Struggle.FailImmense".Localized()

                : "Combat.Izumi.Struggle.FailImpressive".Localized());

            context.Append("Combat.Izumi.Struggle.FailTail".Localized());

            player.HealthChange(-(75 + context.Random.Next(15)));
        }

        public static bool OnPlayerWaited(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(StatusEffectIds.Titsmother))
            {
                context.Clear();

                context.Append("Combat.Izumi.TitSmother.WaitLead".Localized());

                player.TakeLustDamage(player.Libido / 10 + 5 + context.Random.Next(5), true);

                if (CeraphHasBondage())
                {
                    context.Append("Combat.Izumi.TitSmother.WaitBondage".Localized());

                    player.TakeLustDamage(5, true);
                }

                return false;
            }

            if (!player.HasStatusEffect(StatusEffectIds.Chokeslam))
                return true;

            context.Clear();

            context.Append("Combat.Izumi.Wait.Lead".Localized());

            player.HealthChange(-(75 + context.Random.Next(15)));

            return false;
        }

        public const string GroundPoundAbility = "coc:izumi_ground_pound";

        public const string TitSmotherAbility = "coc:izumi_tit_smother";

        // ===== 原版 Izumi.hx:292-306 乳压窒息（Tit Smother） =====
        private static bool CeraphHasBondage() =>

            Scenes.SceneInstance<Scenes.Vanilla.Npcs.CeraphScene>.Get().HasBondage();

        [Register("coc:izumi_tit_smother")]
        public static void TitSmother(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Izumi.TitSmother.Lead".Localized());

            context.Append(player.HasMuzzle()

                ? "Combat.Izumi.TitSmother.Muzzle".Localized()

                : "Combat.Izumi.TitSmother.Face".Localized());

            context.Append("Combat.Izumi.TitSmother.Grab".Localized());

            player.CreateStatusEffect(StatusEffectIds.Titsmother, 0, 0, 0, 0);

            player.TakeLustDamage(player.Libido / 15f + 5 + context.Random.Next(5), true);
        }

        // 原版 Izumi.hx:317 挣脱判定
        private static void TitSmotherStruggle(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Clear();

            if (context.Random.Next(Math.Max(1, (int)player.Strength)) > enemy.Strength / 4)
            {
                TitSmotherEscape(context);

                return;
            }

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.Izumi.TitSmother.StruggleFailForce".Localized());

                context.Append("Combat.Izumi.TitSmother.StruggleFailStruggle".Localized());
            }
            else if (player.HasCock())
            {
                context.Append("Combat.Izumi.TitSmother.StruggleFailCockLead".Localized());

                context.Append(player.Cocks.Count > 1

                    ? "Combat.Izumi.TitSmother.StruggleFailCockMany".Localized()

                    : "Combat.Izumi.TitSmother.StruggleFailCockOne".Localized());
            }
            else
                context.Append("Combat.Izumi.TitSmother.StruggleFailNoCock".Localized());

            player.TakeLustDamage(player.Libido / 15f + 5 + context.Random.Next(5), true);
        }

        // 原版 Izumi.hx:369 挣脱成功：对 Izumi 造成伤害并清除状态
        private static void TitSmotherEscape(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Clear();

            if (player.Strength < 90)
                context.Append("Combat.Izumi.TitSmother.EscapeWeak".Localized());
            else
            {
                context.Append("Combat.Izumi.TitSmother.EscapeStrong".Localized());

                if (player.IsBiped())
                    context.Append("Combat.Izumi.TitSmother.EscapeStrongBiped".Localized());

                context.Append("Combat.Izumi.TitSmother.EscapeStrongEnd".Localized());
            }

            context.Append("Combat.Izumi.TitSmother.EscapeTaunt".Localized());

            player.RemoveStatusEffect(StatusEffectIds.Titsmother);

            var damage = 15 + context.Random.Next(Math.Max(1, (int)player.Strength));

            enemy.HealthChange(-damage);
        }
    }
}