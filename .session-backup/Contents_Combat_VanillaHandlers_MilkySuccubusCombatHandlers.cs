using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 MilkySuccubus 的招式（src/classes/scenes/quests/urtaQuest/MilkySuccubus.hx:20-75）。
    public static class MilkySuccubusCombatHandlers
    {
        // 原版 cowCubiMilkSprayAttack（MilkySuccubus.hx:20-35）：
        //   rand(20) + 1 + player.spe/20 > 17 为闪开；命中则 +15 欲望并给敌人挂 MilkyUrta(3)。
        [Register("coc:milky_succubus_cow_cubi_milk_spray_attack")]
        private static void CowCubiMilkSprayAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.MilkySuccubus.CowCubiMilkSprayAttack.TheSuccubusAsks".Localized());

            if (context.Random.Next(20) + 1 + player.Speed / 20f > 17)
            {
                context.Append("Combat.MilkySuccubus.CowCubiMilkSprayAttack.WithYourTrained".Localized());
                context.Append("Combat.MilkySuccubus.CowCubiMilkSprayAttack.TheSuccubusPouts".Localized());

                player.Lust = Math.Min(player.MaxLust, player.Lust + 6);

                return;
            }

            // 原版 (game.noFur ? "skin" : "fur")
            var skin = player.HasFur() ? "fur" : "skin";

            context.Append("Combat.MilkySuccubus.CowCubiMilkSprayAttack.AllYouManage".Localized(skin));

            player.Lust = Math.Min(player.MaxLust, player.Lust + 15);

            if (!enemy.HasStatusEffect(VanillaStatusEffects.MilkyUrta))
                enemy.CreateStatusEffect(VanillaStatusEffects.MilkyUrta, 3, 0, 0, 0);
        }

        // 原版 drinkMinoCum（MilkySuccubus.hx:37-47）：还有存酒时回 400 血 +25 欲望；喝完只出失望台词。
        [Register("coc:milky_succubus_drink_mino_cum")]
        private static void DrinkMinoCum(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.MilkySuccubus.DrinkMinoCum.SmilingWrylyAnd".Localized());

            if (enemy is MilkySuccubusEnemy milky && milky.TryDrinkCum())
            {
                context.Append("Combat.MilkySuccubus.DrinkMinoCum.SmilingTriumphantlyShe".Localized());

                enemy.HealthChange(400);
                enemy.Lust = Math.Min(enemy.MaxLust, enemy.Lust + 25);
            }
            else
            {
                context.Append("Combat.MilkySuccubus.DrinkMinoCum.SheFrownsAnd".Localized());
            }
        }

        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:milky_succubus_attack")]
        [Register("coc:milky_succubus_attack2")]
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
                DoBlock = true
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

        // 原版 succubusTease（MilkySuccubus.hx:49-60）：连续 3 次独立随机挑一段台词，然后 +20 欲望。
        [Register("coc:milky_succubus_succubus_tease")]
        private static void SuccubusTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (context.Random.Next(4) == 0)
                context.Append("Combat.MilkySuccubus.SuccubusTease.TurningAroundThe".Localized());
            else if (context.Random.Next(3) == 0)
                context.Append("Combat.MilkySuccubus.SuccubusTease.TheSuccubusSqueezes".Localized());
            else if (context.Random.Next(2) == 0)
                context.Append("Combat.MilkySuccubus.SuccubusTease.TheSuccubusTurns".Localized());
            else
                context.Append("Combat.MilkySuccubus.SuccubusTease.TheSuccubusLeans".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 20);
        }
    }
}
