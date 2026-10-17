using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MilkySuccubusCombatHandlers
    {

        [Register("coc:milky_succubus_cow_cubi_milk_spray_attack")]
        public static void CowCubiMilkSprayAttack(CombatContext context, CombatAbilityDefinition ability)
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

            var skin = player.HasFur() ? "fur" : "skin";

            context.Append("Combat.MilkySuccubus.CowCubiMilkSprayAttack.AllYouManage".Localized(skin));

            player.Lust = Math.Min(player.MaxLust, player.Lust + 15);

            if (!enemy.HasStatusEffect(StatusEffectIds.MilkyUrta))
                enemy.CreateStatusEffect(StatusEffectIds.MilkyUrta, 3, 0, 0, 0);
        }

        [Register("coc:milky_succubus_drink_mino_cum")]
        public static void DrinkMinoCum(CombatContext context, CombatAbilityDefinition ability)
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
                context.Append("Combat.MilkySuccubus.DrinkMinoCum.SheFrownsAnd".Localized());
        }

        [Register("coc:milky_succubus_attack")]
        [Register("coc:milky_succubus_attack_two")]
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

        [Register("coc:milky_succubus_succubus_tease")]
        public static void SuccubusTease(CombatContext context, CombatAbilityDefinition ability)
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