using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 MinotaurLord 的招式（src/classes/scenes/quests/urtaQuest/MinotaurLord.hx:9-154）。
    public static class MinotaurLordCombatHandlers
    {
        private static bool UrtaQuest =>
            Math.Abs(CoC.Instance.Flags.Get(VanillaFlags.UrtaQuestStatus) - 0.75f) < 0.0001f;

        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:minotaur_lord_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 minotaurDrankMalk（MinotaurLord.hx:55-74）：只会在 Urta 任务线里抽到。
        [Register("coc:minotaur_lord_minotaur_drank_malk")]
        private static void MinotaurDrankMalk(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheMinotaurLord".Localized());

            if (enemy.StatusEffectValue(VanillaStatusEffects.MinoMilk, 0) < 3)
            {
                context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheSuccubusThrows".Localized());

                enemy.HealthChange(300);
                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 10);

                if (!enemy.HasStatusEffect(VanillaStatusEffects.MinoMilk))
                    enemy.CreateStatusEffect(VanillaStatusEffects.MinoMilk, 1);
                else
                    enemy.ChangeStatusValue(VanillaStatusEffects.MinoMilk, 0, 1);
            }
            else
            {
                context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheSuccubusReplies".Localized());
                context.Append("Combat.MinotaurLord.MinotaurDrankMalk.TheMinotaurGlares".Localized());

                if (!enemy.HasStatusEffect(VanillaStatusEffects.MinoMilk))
                    enemy.CreateStatusEffect(VanillaStatusEffects.MinoMilk, 1);
                else
                    enemy.ChangeStatusValue(VanillaStatusEffects.MinoMilk, 0, 1);
            }
        }

        // 原版 minotaurDisarm（MinotaurLord.hx:76-88）：把玩家武器打掉。
        [Register("coc:minotaur_lord_minotaur_disarm")]
        private static void MinotaurDisarm(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 minotaurLordEntangle（MinotaurLord.hx:90-104）：挂 Timer，链子命中则缠住玩家。
        [Register("coc:minotaur_lord_entangle")]
        private static void Entangle(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.MinotaurLord.MinotaurLordEntangle.TheMinotaurLord".Localized());

            enemy.CreateStatusEffect(VanillaStatusEffects.Timer, 2 + context.Random.Next(4));

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

            if (!player.HasStatusEffect(VanillaStatusEffects.MinotaurEntangled))
                player.CreateStatusEffect(VanillaStatusEffects.MinotaurEntangled);
        }

        // 原版 minotaurCumPress（MinotaurLord.hx:106-127）：被缠住时的固定行动，结算后解除束缚。
        [Register("coc:minotaur_lord_cum_press")]
        private static void CumPress(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.MinotaurLord.MinotaurCumPress.TheMinotaurLord".Localized());

            if (player.Lust >= player.MaxLust)
            {
                context.Append("Combat.MinotaurLord.MinotaurCumPress.YouRiseUp".Localized());
            }
            else
            {
                context.Append("Combat.MinotaurLord.MinotaurCumPress.YouRealizeWhat".Localized());

                if (player.HasCock())
                {
                    context.Append("Combat.MinotaurLord.MinotaurCumPress.Rigid".Localized());

                    if (player.Lust100 >= 80)
                        context.Append("Combat.MinotaurLord.MinotaurCumPress.AndDrippy".Localized());

                    context.Append("Combat.MinotaurLord.MinotaurCumPress.YourCockHas".Localized());
                }
                else if (player.HasVagina())
                {
                    context.Append("Combat.MinotaurLord.MinotaurCumPress.WetYourPussy".Localized());
                }
                else
                {
                    context.Append("Combat.MinotaurLord.MinotaurCumPress.ArousedYouFeel".Localized());
                }

                context.Append("Combat.MinotaurLord.MinotaurCumPress.YouWantAnother".Localized());
            }

            player.TakeLustDamage(22 + player.Libido / 8f + player.Sensitivity / 8f);

            player.RemoveStatusEffect(VanillaStatusEffects.MinotaurEntangled);
        }

        // 原版 minotaurPrecumTease（MinotaurLord.hx:129-154）。
        [Register("coc:minotaur_lord_minotaur_precum_tease")]
        private static void MinotaurPrecumTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.MinotaurLord.MinotaurPrecumTease.TheMinotaurSmiles".Localized());

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.MinotaurLord.MinotaurPrecumTease.SlappingIntoYour".Localized());

                context.Append((player.Lust100 >= 70
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

            context.Append((player.Lust100 <= 75
                ? "Combat.MinotaurLord.MinotaurPrecumTease.YouShiverWith"
                : "Combat.MinotaurLord.MinotaurPrecumTease.YouGroanAnd").Localized());
        }

        // 原版 struggle（MinotaurLord.hx:23-38）：被缠住时 str/9 + rand(20) + 1 >= 15 可挣脱。
        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(VanillaStatusEffects.MinotaurEntangled))
                return;

            context.Clear();

            if (player.Strength / 9f + context.Random.Next(20) + 1 >= 15)
            {
                context.Append("Combat.MinotaurLord.Struggle.UtilizingEveryOunce".Localized());

                player.RemoveStatusEffect(VanillaStatusEffects.MinotaurEntangled);

                if (UrtaQuest)
                    context.Append("Combat.MinotaurLord.Struggle.TheSuccubusSpits".Localized());
            }
            else
            {
                context.Append("Combat.MinotaurLord.Struggle.YouWiggleAnd".Localized());
            }
        }
    }
}
