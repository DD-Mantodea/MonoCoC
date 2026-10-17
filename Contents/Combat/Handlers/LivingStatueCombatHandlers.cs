using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class LivingStatueCombatHandlers
    {

        [Register("coc:living_statue_concussive_blow")]
        public static void ConcussiveBlow(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.ConcussiveBlow.TheGiantRaises".Localized());

            if (player.Stun(2, 33, 100, false))
                context.Append("Combat.LivingStatue.ConcussiveBlow.TheVibrationsLeave".Localized());
            else
                context.Append("Combat.LivingStatue.ConcussiveBlow.YouShakeOff".Localized());

            var damage = 100f * (enemy.Intelligence / Math.Max(1f, player.Intelligence) / 4f);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:living_statue_dirt_kick")]
        public static void DirtKick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.LivingStatue.DirtKick.TheAnimatedSculpture".Localized());

            if (context.Random.Next(2) == 0 && !player.HasStatusEffect(StatusEffectIds.Blind))
            {
                player.CreateStatusEffect(StatusEffectIds.Blind, 2, 0, 0, 0);

                context.Append("Combat.LivingStatue.DirtKick.YouAreBlinded".Localized());
            }
            else
                context.Append("Combat.LivingStatue.DirtKick.YouCloseYour".Localized());
        }

        [Register("coc:living_statue_backhand")]
        public static void Backhand(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.Backhand.TheMarbleGolem".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0)
            {
                context.Append("Combat.LivingStatue.Backhand.YouSlideUnderneath".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.Backhand.YouSlideUnderneath".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.Backhand.ItChitsYou".Localized());

            enemy.Distance = CombatDistance.Distant;

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:living_statue_overhand_smash")]
        public static void OverhandSmash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.OverhandSmash.RaisingItsHammer".Localized());

            var damage = 175f + CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage <= 0 || context.Random.Next(100) < 25)
            {
                context.Append("Combat.LivingStatue.OverhandSmash.YouReAble".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.OverhandSmash.YouReAble".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.OverhandSmash.TheConcussiveStrike".Localized());

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:living_statue_disarm")]
        public static void Disarm(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.Disarm.TheAnimatedStatue".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -30)
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.Disarm.YouManageTo".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.Disarm.YourEquipmentFlies".Localized());

            player.Disarm(player.Weapon);

            enemy.CreateStatusEffect(StatusEffectIds.Disarmed);
        }

        [Register("coc:living_statue_cyclone_strike")]
        public static void CycloneStrike(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.LivingStatue.CycloneStrike.TwistingBackThe".Localized());

            var damage = (175f + CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack)) /

                (context.Random.Next(3) + 2);

            if (damage <= 0)
            {
                context.Append("Combat.LivingStatue.CycloneStrike.ByTheGrace".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player,

                DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.LivingStatue.CycloneStrike.ByTheGrace".Localized());

                return;
            }

            context.Append("Combat.LivingStatue.CycloneStrike.YouReSquarely".Localized());

            context.DealDamageToPlayer(damage);
        }
    }
}