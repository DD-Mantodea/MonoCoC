using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 DemonFistFighter 的招式（src/classes/scenes/places/bazaar/DemonFistFighter.hx:117-230）。
    // 原版用 CombatAttackBuilder 自定义命中率/格挡/反击措辞；C# 的 Avoid 没有逐招命中率加成参数，
    // 这里用等价的 doDodge/doParry/doBlock/doCounter 开关 + 同样的伤害公式，命中率按标准闪避函数。
    public static class DemonFistFighterCombatHandlers
    {
        private static float CalcDamage(CombatContext context, Creature player, Enemy enemy) =>
            CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:demon_fist_fighter_attack")]
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

            var damage = CalcDamage(context, player, enemy);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        // 原版 twoFer（DemonFistFighter.hx:117-126）：挡下出专属格挡词；命中打两下。
        [Register("coc:demon_fist_fighter_two_fer")]
        private static void TwoFer(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 quickJab（DemonFistFighter.hx:128-139）：极快 jab，可被反击；反击时玩家反伤。
        [Register("coc:demon_fist_fighter_quick_jab")]
        private static void QuickJab(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 roundhouse（DemonFistFighter.hx:154-181）：命中踢晕；被挡下后还有一记反向回旋踢。
        [Register("coc:demon_fist_fighter_roundhouse")]
        private static void Roundhouse(CombatContext context, CombatAbilityDefinition ability)
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

                // 原版 player.addStatusEffect(new DazedDebuff(2))（DemonFistFighter.hx:162）
                DemonFistFighterEnemy.ApplyDazed(player, 2, -15f, 0.8f);
            }

            if (avoid.Outcome != AvoidOutcome.Block)
                return;

            context.Append("Combat.DemonFistFighter.Roundhouse.Block".Localized());

            // 原版被挡后自己 fatigue +10，然后立刻补一记反向脚跟踢
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

                // 原版 player.addStatusEffect(new DazedDebuff(2, -20, 0.7))（DemonFistFighter.hx:171）
                DemonFistFighterEnemy.ApplyDazed(player, 2, -20f, 0.7f);

                context.Append((player.Health > 0
                    ? "Combat.DemonFistFighter.Roundhouse.ItTakesHerculean"
                    : "Combat.DemonFistFighter.Roundhouse.YouTryYour").Localized());
            }
            else
            {
                context.Append("Combat.DemonFistFighter.Roundhouse.ReverseDodge".Localized());
            }
        }

        // 原版 crossPunch（DemonFistFighter.hx:183-216）：可格挡/招架；有武器时招架会被夺械。
        [Register("coc:demon_fist_fighter_cross_punch")]
        private static void CrossPunch(CombatContext context, CombatAbilityDefinition ability)
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
            {
                context.Append("Combat.DemonFistFighter.CrossPunch.TheDemonSmiles".Localized());
            }
        }

        // 原版 attackWithPlayerWeapon（DemonFistFighter.hx:218-230）：临时把敌人武器换成玩家的，
        // 打一次 eAttack，再换回拳头。
        private static void AttackWithPlayerWeapon(CombatContext context)
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
