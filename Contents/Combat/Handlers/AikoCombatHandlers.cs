using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class AikoCombatHandlers
    {

        [Register("coc:aiko_basic")]
        public static void Basic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var damage = context.Enemy.Strength + context.Random.Next(15);

            if (context.Random.Next(5) == 0)
            {
                context.WriteAction("Combat.Aiko.Basic.Double".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage - context.Random.Next(10)));

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage - context.Random.Next(9)));

                Slow(context, 4);

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Aiko.Basic.Miss".Localized());

                return;
            }

            if (player.ArmorDefense > 4 && context.Random.Next(3) == 0)
            {
                context.WriteAction("Combat.Aiko.Basic.Glance".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage - player.ArmorDefense * 2));

                return;
            }

            context.WriteAction("Combat.Aiko.Basic.Hit".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));

            Slow(context, 4);
        }

        [Register("coc:aiko_corrupt_basic")]
        public static void CorruptBasic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Aiko.CorruptBasic.Text".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Aiko.CorruptBasic.Avoided".Localized());

                return;
            }

            var damage = context.Enemy.Strength + context.Random.Next(15);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));

            Slow(context, 4);
        }

        [Register("coc:aiko_dark_foxfire")]
        public static void DarkFoxfire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Aiko.DarkFoxfire.Text".Localized());

            var damage = context.Enemy.Strength / 2f + context.Random.Next(15);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));

            var lust = player.HasPerk(PerkIds.Masochist)

                ? 15f + player.Sensitivity / 5f

                : (10f + player.Sensitivity / 5f) * 2f;

            AddLust(context, lust);
        }

        [Register("coc:aiko_terrorize")]
        public static void Terrorize(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Aiko.Terrorize.Text".Localized());

            float resist;

            if (player.Intelligence < 50 + player.Level)
                resist = MathF.Round(player.Intelligence / 55f * 30f);
            else
                resist = 25;

            if (player.HasStatusEffect(StatusEffectIds.Whispered))
                resist += 35;
            else
                context.WriteAction("Combat.Aiko.Terrorize.NoResist".Localized());

            if (player.IsPureEnough(20))
                resist += (int)(15 - player.CorAdjustedDown());

            if (context.Random.Next(100) < resist)
            {
                context.WriteAction("Combat.Aiko.Terrorize.Resisted".Localized());

                if (player.HasStatusEffect(StatusEffectIds.Fear))
                    player.RemoveStatusEffect(StatusEffectIds.Fear);

                return;
            }

            if (player.HasStatusEffect(StatusEffectIds.Fear))
            {
                Slow(context, 4);

                return;
            }

            player.CreateStatusEffect(StatusEffectIds.Fear);

            Slow(context, 10);
        }

        [Register("coc:aiko_tomahawk")]
        public static void Tomahawk(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Aiko.Tomahawk.Miss".Localized());

                return;
            }

            context.WriteAction("Combat.Aiko.Tomahawk.Hit".Localized());

            var damage = context.Enemy.Strength + context.Random.Next(15);

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, damage));
        }

        [Register("coc:aiko_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var lines = new[]
            {
                "Combat.Aiko.Tease.One",

                "Combat.Aiko.Tease.Two",

                "Combat.Aiko.Tease.Three",

                "Combat.Aiko.Tease.Four"
            };

            context.WriteAction(lines[context.Random.Next(lines.Length)].Localized());

            AddLust(context, 8f + player.Sensitivity / 5f);
        }

        [Register("coc:aiko_arcane_archer")]
        public static void ArcaneArcher(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.WriteAction("Combat.Aiko.ArcaneArcher.One".Localized());

            context.WriteAction("Combat.Aiko.ArcaneArcher.Two".Localized());

            enemy.CreateStatusEffect(StatusEffectIds.AikoArcaneArcher);

            CombatCalculator.AddCombatBuff(enemy, GameStats.Strength, 10, "Aiko");

            CombatCalculator.AddCombatBuff(enemy, GameStats.Speed, 10, "Aiko");
        }

        [Register("coc:aiko_splinter_lightning")]
        public static void SplinterLightning(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Aiko.SplinterLightning.One".Localized());

            context.WriteAction("Combat.Aiko.SplinterLightning.Two".Localized());

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, -10, "Aiko");

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -10, "Aiko");

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 45f + 25f / (context.Random.Next(3) + 1)));

            var status = player.CreateOrFindStatusEffect(StatusEffectIds.AikoLightningArrow);

            status.Values[0] += 4;
        }

        [Register("coc:aiko_light_arrow_cage")]
        public static void LightArrowCage(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Aiko.LightArrowCage.One".Localized());

            if (player.Corruption > 40)
            {
                context.WriteAction("Combat.Aiko.LightArrowCage.Corrupt".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 2f * (80f + player.Corruption)));
            }
            else
                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 1.2f * (80f + player.Corruption)));

            context.Enemy.RemoveStatusEffect(StatusEffectIds.AikoArcaneArcher);
        }

        [Register("coc:aiko_ice_arrow")]
        public static void IceArrow(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Aiko.IceArrow.One".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Aiko.IceArrow.Miss".Localized());

                return;
            }

            context.WriteAction("Combat.Aiko.IceArrow.Hit".Localized());

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -15, "Aiko");

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength * 2f + context.Random.Next(40)));
        }

        [Register("coc:aiko_arrow_rain")]
        public static void ArrowRain(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var arrows = context.Random.Next(4) + 2;

            context.WriteAction("Combat.Aiko.ArrowRain.Text".Localized(arrows));

            for (var i = 0; i < arrows; i++)
                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Random.Next(15)));
        }

        [Register("coc:aiko_arrow_barrage")]
        public static void ArrowBarrage(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var arrows = context.Random.Next(3) + 3;

            context.WriteAction("Combat.Aiko.ArrowBarrage.Text".Localized());

            for (var i = 0; i < arrows; i++)
            {
                var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
                {
                    Attacker = context.Enemy,

                    Defender = player,

                    DoDodge = true,

                    DoParry = false,

                    DoBlock = false
                });

                if (avoid.Outcome != AvoidOutcome.Hit)
                    continue;

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, context.Enemy.Strength + context.Random.Next(10)));

                CombatCalculator.AddCombatBuff(player, GameStats.Speed, -2, "Aiko");
            }
        }

        [Register("coc:aiko_hyper_attack_one")]
        public static void HyperAttack1(CombatContext context, CombatAbilityDefinition ability)
        {
            context.WriteAction("Combat.Aiko.HyperAttack1".Localized());

            context.Enemy.CreateOrFindStatusEffect(StatusEffectIds.AikoCharging);
        }

        [Register("coc:aiko_hyper_attack_two")]
        public static void HyperAttack2(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            enemy.RemoveStatusEffect(StatusEffectIds.AikoCharging);

            context.WriteAction("Combat.Aiko.HyperAttack2.One".Localized());

            context.WriteAction("Combat.Aiko.HyperAttack2.Two".Localized());

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -20, "Aiko");

            CombatCalculator.AddCombatBuff(player, GameStats.Strength, -15, "Aiko");

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, player.IsPureEnough(20) ? 250f : 350f));

            if (player.Health > 0)
                context.WriteAction("Combat.Aiko.HyperAttack2.Survive".Localized());

            enemy.RemoveStatusEffect(StatusEffectIds.AikoArcaneArcher);
        }

        [Register("coc:aiko_seal_physical")]
        public static void SealPhysical(CombatContext context, CombatAbilityDefinition ability)
        {
            var types = new[]
            {
                KitsuneSealType.Attack, KitsuneSealType.Tease, KitsuneSealType.Items,

                KitsuneSealType.Movement, KitsuneSealType.Physical
            };

            KitsuneSeals.Seal(context, types[context.Random.Next(types.Length)]);
        }

        [Register("coc:aiko_seal_magic")]
        public static void SealMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            var types = new[] { KitsuneSealType.Spells, KitsuneSealType.Movement, KitsuneSealType.Magical };

            KitsuneSeals.Seal(context, types[context.Random.Next(types.Length)]);
        }

        public static void Slow(CombatContext context, int amount)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(StatusEffectIds.AikoSlowed))
            {
                player.CreateStatusEffect(StatusEffectIds.AikoSlowed);

                var source = new StatusSource(StatusEffectIds.AikoSlowed, "Aiko Slowed");

                player.Stats.AddModifier(GameStats.Speed, ModifierOperation.Addition,

                    () => -player.StatusEffectValue(StatusEffectIds.AikoSlowed, 0), source);
            }

            player.ChangeStatusValue(StatusEffectIds.AikoSlowed, 0, amount);
        }

        public static void AddLust(CombatContext context, float amount)
        {
            var player = context.Player;

            player.Lust = Math.Min(player.MaxLust, player.Lust + amount);
        }
    }
}