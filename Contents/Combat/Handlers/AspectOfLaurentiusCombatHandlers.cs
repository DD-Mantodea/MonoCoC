using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    public static class AspectOfLaurentiusCombatHandlers
    {
        [Register("coc:aspect_of_laurentius_apotheosis")]
        public static void Apotheosis(CombatContext context, CombatAbilityDefinition ability)
        {
            var round = CombatController.Instance!.State.Round;

            var text = round switch
            {
                4 => "Combat.Hel.LaurentiusApotheosis4",

                12 => "Combat.Hel.LaurentiusApotheosis12",

                20 => "Combat.Hel.LaurentiusApotheosis20",

                _ => "Combat.Hel.LaurentiusApotheosisDefault"
            };

            context.Append(text.Localized());

            context.Append("Combat.Hel.LaurentiusApotheosisPulse".Localized());

            if (!context.Player.HasStatusEffect(StatusEffectIds.Apotheosis))
            {
                context.Player.CreateStatusEffect(StatusEffectIds.Apotheosis, 1, 0, 0, 0);

                context.Enemy.CreateStatusEffect(StatusEffectIds.Apotheosis, 1, 0, 0, 0);
            }
            else
            {
                context.Player.ChangeStatusValue(StatusEffectIds.Apotheosis, 0, 1);

                context.Enemy.ChangeStatusValue(StatusEffectIds.Apotheosis, 0, 1);
            }
        }

        [Register("coc:aspect_of_laurentius_whitefire")]
        public static void Whitefire(CombatContext context, CombatAbilityDefinition ability)
        {
            var quotes = new[]
            {
                "Combat.Hel.LaurentiusWhitefire1",

                "Combat.Hel.LaurentiusWhitefire2",

                "Combat.Hel.LaurentiusWhitefire3",

                "Combat.Hel.LaurentiusWhitefire4"
            };

            context.Append(quotes[Random.Shared.Next(quotes.Length)].Localized());

            context.Append("Combat.Hel.LaurentiusWhitefireBurst".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy, Defender = context.Player,

                DoDodge = true, DoParry = false, DoBlock = false, ToHitChance = -30
            });

            if (avoid.Outcome == AvoidOutcome.Dodge)
            {
                context.Append("Combat.Hel.LaurentiusWhitefireDodged".Localized());

                return;
            }

            context.Append("Combat.Hel.LaurentiusWhitefireHit".Localized());

            var stacks = (int)context.Enemy.StatusEffectValue(StatusEffectIds.Apotheosis, 0);

            var damage = (100 + Random.Shared.Next(50)) * (float)Math.Pow(2, stacks);

            context.DealDamageToPlayer(damage);
        }

        [Register("coc:aspect_of_laurentius_summon_fodder")]
        public static void SummonFodder(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.LaurentiusSummon".Localized());

            var ids = new[]
            {
                new MonoCoC.Registry.Identifier("coc", "amarok"),

                new MonoCoC.Registry.Identifier("coc", "goblin_elder"),

                new MonoCoC.Registry.Identifier("coc", "phoenix"),

                new MonoCoC.Registry.Identifier("coc", "valkyrie"),

                new MonoCoC.Registry.Identifier("coc", "basilisk")
            };

            var pool = new System.Collections.Generic.List<MonoCoC.Registry.Identifier>(ids);

            for (var i = 0; i < 3 && pool.Count > 0; i++)
            {
                var pick = Random.Shared.Next(pool.Count);

                var enemy = EnemyRegistry.Create(pool[pick]);

                pool.RemoveAt(pick);

                enemy.ShortName = "ethereal " + enemy.ShortName;

                enemy.TookAction = true;

                context.Enemies.Add(enemy);

                context.Append("Combat.Hel.LaurentiusSummonAppears".Localized(enemy.ShortName));
            }

            context.Append("Combat.Hel.LaurentiusShield".Localized());

            if (context.Enemy is Enemies.Vanilla.AspectOfLaurentiusEnemy aspect)
            {
                aspect.Invincible = true;

                aspect.SummonedTurn = CombatController.Instance!.State.Round;
            }
        }

        [Register("coc:aspect_of_laurentius_watch")]
        public static void Watch(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.LaurentiusWatches".Localized());
        }

        [Register("coc:aspect_of_laurentius_unsummon")]
        public static void Unsummon(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Hel.LaurentiusUnsummon".Localized());

            var defeated = 0;

            for (var i = context.Enemies.Count - 1; i >= 1; i--)
            {
                if (context.Enemies[i].Health <= 0)
                    defeated++;

                context.Enemies.RemoveAt(i);
            }

            if (defeated > 0)
            {
                context.Enemy.HealthChange(-100 * defeated);

                context.Append("Combat.Hel.LaurentiusAbsorbHurt".Localized(100 * defeated));
            }

            if (context.Enemy is Enemies.Vanilla.AspectOfLaurentiusEnemy aspect)
                aspect.Invincible = false;
        }
    }
}