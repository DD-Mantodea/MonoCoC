using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ScorpionCombatHandlers
    {

        [Register("coc:scorpion_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var version = context.Random.Next(5);

            context.Append(Key("Scenes.Scorpion.Attack.Lead.V.", version));

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append(Key("Scenes.Scorpion.Attack.Avoid.V.", version));

                return;
            }

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Append(Key("Scenes.Scorpion.Attack.Block.V.", version));

                return;
            }

            context.Append(Key("Scenes.Scorpion.Attack.Hit.V.", version));

            var raw = enemy.Strength + enemy.WeaponAttack + context.Random.Next(10);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));
        }

        [Register("coc:scorpion_grab")]
        public static void Grab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Scorpion.Grab.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.Scorpion.Grab.Avoid".Localized());

                return;
            }

            context.Append("Scenes.Scorpion.Grab.Hit".Localized());

            var raw = (enemy.Strength + enemy.WeaponAttack) / 2 + context.Random.Next(5);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));

            player.CreateStatusEffect(StatusEffectIds.ScorpGrabbed);
        }

        public static void Struggle(CombatContext context)
        {
            var player = context.Player;

            var escaped = context.Random.Next(100) < player.Strength / 4f

                || context.Random.Next(100) < 10;

            if (escaped)
                Escape(context);
            else
                Constrict(context);
        }

        public static void Escape(CombatContext context)
        {
            context.Clear();

            context.Append("Scenes.Scorpion.Escape.Text".Localized());

            context.Player.RemoveStatusEffect(StatusEffectIds.ScorpGrabbed);

            context.Enemy.TookAction = true;
        }

        public static void Constrict(CombatContext context)
        {
            context.Clear();

            context.Append(Key("Scenes.Scorpion.Constrict.V.", context.Random.Next(5)));
        }

        public static void Grapple(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append(Key("Scenes.Scorpion.Grapple.V.", context.Random.Next(7)));

            var raw = enemy.Strength + enemy.WeaponAttack;

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));
        }

        [Register("coc:scorpion_sting")]
        public static void Sting(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Scorpion.Sting.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.Scorpion.Sting.Avoid".Localized());

                return;
            }

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Append("Scenes.Scorpion.Sting.Block".Localized());

                return;
            }

            context.Append("Scenes.Scorpion.Sting.Hit".Localized());

            var raw = (enemy.Strength + enemy.WeaponAttack) / 2 + context.Random.Next(5);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));

            player.CreateStatusEffect(StatusEffectIds.ScorpVenom, duration: 6);
        }

        [Register("coc:scorpion_slam")]
        public static void Slam(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Scorpion.Slam.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
                context.Append("Scenes.Scorpion.Slam.Avoid".Localized());
            else if (avoid.Outcome == AvoidOutcome.Block)
                context.Append("Scenes.Scorpion.Slam.Block".Localized());
            else
            {
                context.Append("Scenes.Scorpion.Slam.Hit".Localized());

                var raw = enemy.Strength + enemy.WeaponAttack * 2 + context.Random.Next(25);

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));
            }

            CombatRangeData.MoveDistantSafe(enemy);
        }

        [Register("coc:scorpion_sand")]
        public static void Sand(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Scorpion.Sand.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = false,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Append("Scenes.Scorpion.Sand.Block".Localized());

                return;
            }

            context.Append("Scenes.Scorpion.Sand.Hit".Localized());

            player.CreateStatusEffect(StatusEffectIds.ScorpBlind);

            var second = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (second.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.Scorpion.Sand.Avoid".Localized());

                return;
            }

            var raw = enemy.Strength / 2 + context.Random.Next(25);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));
        }

        [Register("coc:scorpion_trample")]
        public static void Trample(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Scorpion.Trample.Lead".Localized());

            for (var index = 0; index < 6; index++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false
                });

                if (avoid.Outcome == AvoidOutcome.Dodge)
                {
                    context.Append(Key("Scenes.Scorpion.Trample.Avoid.V.", index));

                    continue;
                }

                context.Append(Key("Scenes.Scorpion.Trample.Hit.V.", index));

                var raw = index is 0 or 5

                    ? (enemy.Strength + enemy.WeaponAttack) / 2 + context.Random.Next(5)

                    : enemy.Strength + enemy.WeaponAttack + context.Random.Next(25);

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));
            }

            context.Append("Scenes.Scorpion.Trample.End".Localized());

            CombatRangeData.MoveDistantSafe(enemy);
        }

        [Register("coc:scorpion_hammer")]
        public static void Hammer(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Scorpion.Hammer.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Scenes.Scorpion.Hammer.Avoid".Localized());

                return;
            }

            if (avoid.Outcome == AvoidOutcome.Block)
            {
                context.Append("Scenes.Scorpion.Hammer.Block".Localized());

                return;
            }

            context.Append("Scenes.Scorpion.Hammer.Hit".Localized());

            var raw = enemy.Strength + enemy.WeaponAttack * 1.5f + context.Random.Next(25);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, raw));

            player.Stun(1, 100, 100, false);
        }

        [Register("coc:scorpion_scuttle")]
        public static void Scuttle(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Scenes.Scorpion.Scuttle.Text".Localized());

            CombatRangeData.MoveDistantSafe(context.Enemy);
        }

        [Register("coc:scorpion_idle")]
        public static void Idle(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append(Key("Scenes.Scorpion.Idle.V.", context.Random.Next(9)));

            context.Enemy.ChangeFatigue(-25);
        }

        public static string Key(string prefix, int index) => (prefix + index).Localized();
    }
}