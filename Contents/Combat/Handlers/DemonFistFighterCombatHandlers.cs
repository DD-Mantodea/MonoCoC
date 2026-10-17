using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class DemonFistFighterCombatHandlers
    {
        public static float CalcDamage(CombatContext context, Creature player, Enemy enemy) =>

            CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

        [Register("coc:demon_fist_fighter_attack")]
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

            var damage = CalcDamage(context, player, enemy);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:demon_fist_fighter_two_fer")]
        public static void TwoFer(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.DemonFistFighter.TwoFer.WeavesAndFeints".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Append("Combat.DemonFistFighter.TwoFer.Block".Localized());

                return;
            }

            if (avoid.Outcome != AvoidOutcome.Hit)
                return;

            context.Append("Combat.DemonFistFighter.TwoFer.YouAttemptTo".Localized());

            var damage = CalcDamage(context, player, enemy) * 0.75f;

            player.HealthChange(-damage);

            player.HealthChange(-damage);
        }

        [Register("coc:demon_fist_fighter_quick_jab")]
        public static void QuickJab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.DemonFistFighter.QuickJab.WeavesForwardAnd".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false,

                DoCounter = true
            });

            if (avoid.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Combat.DemonFistFighter.QuickJab.TheJabClocks".Localized());

                player.HealthChange(-(CalcDamage(context, player, enemy) / 3f));
            }
            else if (avoid.Outcome == AvoidOutcome.Counter)
            {
                context.Append("Combat.DemonFistFighter.QuickJab.Counter".Localized());

                var damage = CombatCalculator.ReduceDamage(context, enemy,

                    0.15f * (player.Strength + player.WeaponAttack));

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);
            }
        }

        [Register("coc:demon_fist_fighter_roundhouse")]
        public static void Roundhouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.DemonFistFighter.Roundhouse.RapidlyTwistsHis".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Combat.DemonFistFighter.Roundhouse.TheViciousKick".Localized());

                context.Append("Combat.DemonFistFighter.Roundhouse.ItStillDoes".Localized());

                player.HealthChange(-(CalcDamage(context, player, enemy) * 1.25f));

                DemonFistFighterEnemy.ApplyDazed(player, 2, -15f, 0.8f);
            }

            if (avoid.Outcome != AvoidOutcome.Block)
                return;

            context.Append("Combat.DemonFistFighter.Roundhouse.Block".Localized());

            enemy.ChangeFatigue(10);

            var reverse = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (reverse.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Combat.DemonFistFighter.Roundhouse.HeelLandsOn".Localized());

                context.Append("Combat.DemonFistFighter.Roundhouse.TheDemonSays".Localized());

                player.HealthChange(-(CalcDamage(context, player, enemy) * 1.75f));

                DemonFistFighterEnemy.ApplyDazed(player, 2, -20f, 0.7f);

                context.Append((player.Health > 0

                    ? "Combat.DemonFistFighter.Roundhouse.ItTakesHerculean"

                    : "Combat.DemonFistFighter.Roundhouse.YouTryYour").Localized());
            }
            else
                context.Append("Combat.DemonFistFighter.Roundhouse.ReverseDodge".Localized());
        }

        [Register("coc:demon_fist_fighter_cross_punch")]
        public static void CrossPunch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var fighter = enemy as DemonFistFighterEnemy;

            context.Append("Combat.DemonFistFighter.CrossPunch.LungesForwardPowerfully".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.Outcome == AvoidOutcome.Hit)
            {
                context.Append("Combat.DemonFistFighter.CrossPunch.YouFailTo".Localized());

                context.Append("Combat.DemonFistFighter.CrossPunch.YouRecoverYour".Localized());

                player.HealthChange(-(CalcDamage(context, player, enemy) * 1.25f));

                return;
            }

            if (avoid.Outcome != AvoidOutcome.Parry || fighter == null)
                return;

            var shouldParryCounter = !player.Weapon.IsAttached() && !player.Weapon.IsFist() &&

                (!fighter.HasAttemptedBSwordDisarm || !player.Weapon.IsHolySword());

            if (!shouldParryCounter)
                return;

            context.Append("Combat.DemonFistFighter.CrossPunch.Parry".Localized());

            enemy.ChangeFatigue(10);

            if (context.Random.Next(Math.Max(1, (int)player.Strength)) >=
                context.Random.Next(Math.Max(1, (int)enemy.Strength)))

                return;

            context.Append("Combat.DemonFistFighter.CrossPunch.DespiteYourStruggle".Localized());

            if (player.Weapon.IsHolySword())
            {
                context.Append("Combat.DemonFistFighter.CrossPunch.TheDemonAttempts".Localized());

                CombatCalculator.DamageEnemy(context,

                    15 + context.Random.Next(15), CombatDamageKind.Melee);

                fighter.HasAttemptedBSwordDisarm = true;

                return;
            }

            context.Append("Combat.DemonFistFighter.CrossPunch.HeInspectsThe".Localized());

            AttackWithPlayerWeapon(context);

            if (player.Weapon.IsScythe())
            {
                context.Append("Combat.DemonFistFighter.CrossPunch.TheDemonGroans".Localized());

                CombatCalculator.DamageEnemy(context,

                    50 + context.Random.Next(15), CombatDamageKind.Melee);

                context.Append("Combat.DemonFistFighter.CrossPunch.TheDemonShakes".Localized());
            }
            else
                context.Append("Combat.DemonFistFighter.CrossPunch.TheDemonSmiles".Localized());
        }

        public static void AttackWithPlayerWeapon(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var weapon = player.Weapon;

            enemy.WeaponName = weapon.Name;

            enemy.WeaponVerb = weapon.Verb;

            enemy.WeaponAttack = weapon.Attack;

            enemy.NormalAttackAvoid.DoParry = false;

            enemy.NormalAttackAvoid.DoCounter = false;

            CombatController.Instance!.Resolver.EnemyBasicAttack();

            enemy.WeaponName = "fists";

            enemy.WeaponVerb = "jab";

            enemy.WeaponAttack = 0;

            enemy.NormalAttackAvoid.DoParry = true;

            enemy.NormalAttackAvoid.DoCounter = true;
        }
    }
}