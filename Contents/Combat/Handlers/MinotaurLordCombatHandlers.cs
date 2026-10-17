using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MinotaurLordCombatHandlers
    {
        public static bool UrtaQuest =>

            Math.Abs(CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) - 0.75f) < 0.0001f;

        [Register("coc:minotaur_lord_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
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
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:minotaur_lord_minotaur_drank_malk")]
        public static void MinotaurDrankMalk(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheMinotaurLord".Localized());

            if (enemy.StatusEffectValue(StatusEffectIds.MinoMilk, 0) < 3)
            {
                context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheSuccubusThrows".Localized());

                enemy.HealthChange(300);

                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 10);

                if (!enemy.HasStatusEffect(StatusEffectIds.MinoMilk))
                    enemy.CreateStatusEffect(StatusEffectIds.MinoMilk, 1);
                else
                    enemy.ChangeStatusValue(StatusEffectIds.MinoMilk, 0, 1);
            }
            else
            {
                context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheSuccubusReplies".Localized());

                context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheMinotaurGlares".Localized());

                if (!enemy.HasStatusEffect(StatusEffectIds.MinoMilk))
                    enemy.CreateStatusEffect(StatusEffectIds.MinoMilk, 1);
                else
                    enemy.ChangeStatusValue(StatusEffectIds.MinoMilk, 0, 1);
            }
        }

        [Register("coc:minotaur_lord_minotaur_disarm")]
        public static void MinotaurDisarm(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (UrtaQuest)
            {
                context.Append("Combat.MinotaurLord.MinotaurDisarm.TheGiantOf".Localized());

                context.Append("Combat.MinotaurLord.MinotaurDisarm.TheSuccubusLaughs".Localized());

                player.SetUnarmed();

                return;
            }

            context.Append("Combat.MinotaurLord.MinotaurDisarm.TheGiantOf2".Localized());

            if (!player.Weapon.IsAttached())
            {
                context.Append("Combat.MinotaurLord.MinotaurDisarm.WithAStrong".Localized());

                player.Disarm(player.Weapon);
            }
        }

        [Register("coc:minotaur_lord_entangle")]
        public static void Entangle(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.MinotaurLord.MinotaurLordEntangle.TheMinotaurLord".Localized());

            enemy.CreateStatusEffect(StatusEffectIds.Timer, 2 + context.Random.Next(4));

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
                context.Append("Combat.MinotaurLord.MinotaurLordEntangle.YouLeapOver".Localized());

                return;
            }

            context.Append("Combat.MinotaurLord.MinotaurLordEntangle.YouTryTo".Localized());

            if (UrtaQuest)
                context.Append("Combat.MinotaurLord.MinotaurLordEntangle.TheSuccubusLaughs".Localized());

            context.Append("Combat.MinotaurLord.MinotaurLordEntangle.YouReTangled".Localized());

            if (!player.HasStatusEffect(StatusEffectIds.MinotaurEntangled))
                player.CreateStatusEffect(StatusEffectIds.MinotaurEntangled);
        }

        [Register("coc:minotaur_lord_cum_press")]
        public static void CumPress(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.MinotaurLord.MinotaurCumPress.TheMinotaurLord".Localized());

            if (player.Lust >= player.MaxLust)
                context.Append("Combat.MinotaurLord.MinotaurCumPress.YouRiseUp".Localized());
            else
            {
                context.Append("Combat.MinotaurLord.MinotaurCumPress.YouRealizeWhat".Localized());

                if (player.HasCock())
                {
                    context.Append("Combat.MinotaurLord.MinotaurCumPress.Rigid".Localized());

                    if (player.LustPercent >= 80)
                        context.Append("Combat.MinotaurLord.MinotaurCumPress.AndDrippy".Localized());

                    context.Append("Combat.MinotaurLord.MinotaurCumPress.YourCockHas".Localized());
                }
                else if (player.HasVagina())
                    context.Append("Combat.MinotaurLord.MinotaurCumPress.WetYourPussy".Localized());
                else
                    context.Append("Combat.MinotaurLord.MinotaurCumPress.ArousedYouFeel".Localized());

                context.Append("Combat.MinotaurLord.MinotaurCumPress.YouWantAnother".Localized());
            }

            player.TakeLustDamage(22 + player.Libido / 8f + player.Sensitivity / 8f);

            player.RemoveStatusEffect(StatusEffectIds.MinotaurEntangled);
        }

        [Register("coc:minotaur_lord_minotaur_precum_tease")]
        public static void MinotaurPrecumTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.MinotaurLord.MinotaurPrecumTease.TheMinotaurSmiles".Localized());

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.MinotaurLord.MinotaurPrecumTease.SlappingIntoYour".Localized());

                context.Append((player.LustPercent >= 70

                    ? "Combat.MinotaurLord.MinotaurPrecumTease.SwallowingItInto"

                    : "Combat.MinotaurLord.MinotaurPrecumTease.FeelingYourHeart").Localized());

                player.TakeLustDamage(15 + player.Libido / 8f + player.Sensitivity / 8f);
            }
            else
            {
                context.Append("Combat.MinotaurLord.MinotaurPrecumTease.RightPastYour".Localized());

                context.Append((UrtaQuest

                    ? "Combat.MinotaurLord.MinotaurPrecumTease.TheAnimalisticScent"

                    : "Combat.MinotaurLord.MinotaurPrecumTease.TheAnimalisticScent2").Localized());

                player.TakeLustDamage(11 + player.Libido / 10f);
            }

            context.Append((player.LustPercent <= 75

                ? "Combat.MinotaurLord.MinotaurPrecumTease.YouShiverWith"

                : "Combat.MinotaurLord.MinotaurPrecumTease.YouGroanAnd").Localized());
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.MinotaurEntangled))
                return;

            context.Clear();

            if (player.Strength / 9f + context.Random.Next(20) + 1 >= 15)
            {
                context.Append("Combat.MinotaurLord.Struggle.UtilizingEveryOunce".Localized());

                player.RemoveStatusEffect(StatusEffectIds.MinotaurEntangled);

                if (UrtaQuest)
                    context.Append("Combat.MinotaurLord.Struggle.TheSuccubusSpits".Localized());
            }
            else
                context.Append("Combat.MinotaurLord.Struggle.YouWiggleAnd".Localized());
        }
    }
}