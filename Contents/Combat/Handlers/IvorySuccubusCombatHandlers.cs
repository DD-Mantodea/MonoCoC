using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class IvorySuccubusCombatHandlers
    {

        [Register("coc:ivory_succubus_lash")]
        public static void Lash(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var version = context.Random.Next(3);

            context.Append($"Combat.IvorySuccubus.Lash.Lead{version}".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                ToHitChance = player.StandardDodgeFunc(enemy, 10)
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Block:
                    context.Append($"Combat.IvorySuccubus.Lash.Block{version}".Localized());

                    break;

                case AvoidOutcome.Dodge:
                    context.Append($"Combat.IvorySuccubus.Lash.Avoid{version}".Localized());

                    break;

                case AvoidOutcome.Hit:
                    context.Append($"Combat.IvorySuccubus.Lash.Hit{version}".Localized());

                    var damage = CombatCalculator.ReduceDamage(context, player,

                        enemy.Strength + enemy.WeaponAttack + context.Random.Next(20));

                    player.TakeDamage(damage);

                    break;
            }
        }

        [Register("coc:ivory_succubus_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append($"Combat.IvorySuccubus.Tease.Text{context.Random.Next(3)}".Localized());

            context.Player.TakeLustDamage(12 + context.Random.Next(20));
        }

        [Register("coc:ivory_succubus_flight")]
        public static void Flight(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Flight.HerWingsExtend".Localized());

            var enemy = (IvorySuccubusEnemy)context.Enemy;

            enemy.FlightCooldown = 7;

            enemy.CreateStatusEffect(IvorySuccubusEnemy.PermaFly, 0, 0, 0, 0);

            CombatRangeData.MoveDistantSafe(enemy);
        }

        [Register("coc:ivory_succubus_blind")]
        public static void Blind(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Blind.ShePresentsHer".Localized());

            if (context.Random.Next(100) < 33)
            {
                context.Append("Combat.IvorySuccubus.Blind.FortunatelyYouBlinked".Localized());

                return;
            }

            context.Append("Combat.IvorySuccubus.Blind.YouShutYour".Localized());

            context.Player.CreateStatusEffect(StatusEffectIds.Blind,

                2 + context.Random.Next(3), 0, 0, 0);
        }

        [Register("coc:ivory_succubus_whitefire")]
        public static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Whitefire.WispsOfFire".Localized());

            var damage = 2.5f * context.Enemy.Intelligence + context.Random.Next(50);

            context.MonsterDamageType = DamageTypeIds.Fire;

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:ivory_succubus_charge")]
        public static void Charge(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Charge.TheDemonTwirls".Localized());

            context.Enemy.CreateStatusEffect(StatusEffectIds.ChargeWeapon, 25, 0, 0, 0);
        }

        [Register("coc:ivory_succubus_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.IvorySuccubus.Arouse.TheDemonessPerforms".Localized());

            context.Player.TakeLustDamage(context.Enemy.Intelligence / 4f + context.Random.Next(15));
        }

        [Register("coc:ivory_succubus_attract")]
        public static void Attract(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append($"Combat.IvorySuccubus.Attract.Text{context.Random.Next(2)}".Localized());

            context.Append("Combat.IvorySuccubus.Attract.Text".Localized());

            context.Player.CreateStatusEffect(IvorySuccubusEnemy.Attracted, .75f, 0, 0, 0);
        }

        [Register("coc:ivory_succubus_lock_up")]
        public static void LockUp(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.IvorySuccubus.LockUp.TheDemonessS".Localized());

            var damage = context.Enemy.Intelligence / 4f + context.Random.Next(10);

            player.TakeDamage(damage);

            player.ChangeFatigue(15, FatigueType.Physical);

            player.Stun();
        }
    }
}