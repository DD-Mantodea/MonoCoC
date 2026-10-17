using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using System.Collections.Generic;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class AlrauneCombatHandlers
    {

        [Register("coc:alraune_entangle")]
        public static void Entangle(CombatContext context, CombatAbilityDefinition ability)
        {
            var alraune = context.EnemyAs<AlrauneEnemy>();

            var player = context.Player;

            if (alraune == null)
                return;

            var toHit = alraune.DidTrip ? 100f : player.StandardDodgeFunc(alraune) / 2f;

            context.WriteAction("Scenes.Alraune.Entangle.VinesSling".Localized());

            if (player.IsGoo())
            {
                context.WriteAction("Scenes.Alraune.Entangle.SlimeSlipsFree".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = alraune,

                Defender = player,

                ToHitChance = toHit,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Scenes.Alraune.Entangle.VinesMissLeap".Localized());

                return;
            }

            context.WriteAction("Scenes.Alraune.Entangle.TightlyBound".Localized());

            player.CreateStatusEffect(StatusEffectIds.VineTangled, 0, 0, 0, 0, 3);

            player.IsImmobilized = true;
        }

        [Register("coc:alraune_whip")]
        public static void Whip(CombatContext context, CombatAbilityDefinition ability)
        {
            var alraune = context.EnemyAs<AlrauneEnemy>();

            var player = context.Player;

            if (alraune == null)
                return;

            var toHit = alraune.DidTrip ? 100f : player.StandardDodgeFunc(alraune);

            context.WriteAction("Scenes.Alraune.Whip.VineWhipLead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = alraune,

                Defender = player,

                ToHitChance = toHit,

                DoDodge = true,

                DoParry = true,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction(avoid.Outcome == AvoidOutcome.Parry

                    ? "Scenes.Alraune.Whip.Parried".Localized()

                    : "Scenes.Alraune.Whip.Miss".Localized());

                return;
            }

            context.WriteAction("Scenes.Alraune.Whip.VineWhipHit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, alraune.Strength + alraune.WeaponAttack);

            player.HealthChange(-damage);

            if (alraune.DidTrip || context.Random.Next(5) == 0)
                player.Bleed(alraune);
        }

        [Register("coc:alraune_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var texts = new List<string>
            {
                "Scenes.Alraune.Tease.ShakesHips",

                "Scenes.Alraune.Tease.NaturesNectar",

                "Scenes.Alraune.Tease.BaresBreasts"
            };

            if (player.IsMale())
                texts.Add("Scenes.Alraune.Tease.SowSeeds");

            if (player.DryadScore() >= 3 && player.HasGenitals())
                texts.Add(player.HasCock()

                    ? (context.Random.Next(2) == 0 ? "Scenes.Alraune.Tease.PistilStamen" : "Scenes.Alraune.Tease.PollinateMe")

                    : "Scenes.Alraune.Tease.FlowersKiss");

            var lust = context.Random.Next(10) + context.Random.Next(10) + context.Random.Next(10);

            context.WriteAction(texts[context.Random.Next(texts.Count)].Localized());

            player.TakeLustDamage(lust);
        }

        [Register("coc:alraune_charge")]
        public static void Charge(CombatContext context, CombatAbilityDefinition ability)
        {
            var alraune = context.EnemyAs<AlrauneEnemy>();

            var player = context.Player;

            if (alraune == null)
                return;

            var toHit = alraune.DidTrip ? 100f : player.StandardDodgeFunc(alraune);

            context.WriteAction("Scenes.Alraune.Charge.RootsFlexCharge".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = alraune,

                Defender = player,

                ToHitChance = toHit,

                DoDodge = true,

                DoParry = false,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction(avoid.Outcome == AvoidOutcome.Block

                    ? "Scenes.Alraune.Charge.Blocked".Localized()

                    : "Scenes.Alraune.Charge.Miss".Localized());

                return;
            }

            context.WriteAction("Scenes.Alraune.Charge.ClawSlamHit".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, (alraune.Strength + alraune.WeaponAttack) * 1.5f);

            player.HealthChange(-damage);

            player.Bleed(alraune, 3 + context.Random.Next(4), 3);
        }

        [Register("coc:alraune_lash")]
        public static void Lash(CombatContext context, CombatAbilityDefinition ability)
        {
            var alraune = context.EnemyAs<AlrauneEnemy>();

            var player = context.Player;

            if (alraune == null)
                return;

            var toHit = alraune.DidTrip ? 100f : player.StandardDodgeFunc(alraune);

            context.WriteAction("Scenes.Alraune.Lash.VineClawSwipe".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = alraune,

                Defender = player,

                ToHitChance = toHit,

                DoDodge = true,

                DoParry = true,

                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                var missKey = avoid.Outcome switch
                {
                    AvoidOutcome.Block => "Scenes.Alraune.Lash.Blocked",

                    AvoidOutcome.Parry => "Scenes.Alraune.Lash.Parried",

                    _ => "Scenes.Alraune.Lash.Miss"
                };

                context.WriteAction(missKey.Localized());

                return;
            }

            context.WriteAction("Scenes.Alraune.Lash.VineClawBites".Localized());

            var baseDamage = alraune.Strength + alraune.WeaponAttack;

            var damage = baseDamage + (float)context.Random.NextDouble() * (baseDamage * 0.5f);

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            player.HealthChange(-damage);

            if (alraune.DidTrip)
                player.Bleed(alraune, 3, 1);
            else if (context.Random.Next(4) > 0)
                player.Bleed(alraune, 4, 1 + context.Random.Next(2));
        }

        [Register("coc:alraune_root")]
        public static void Root(CombatContext context, CombatAbilityDefinition ability)
        {
            if (context.EnemyAs<AlrauneEnemy>() is not { } alraune || alraune.Rooted)
                return;

            context.WriteAction("Scenes.Alraune.Root.RetreatsToNest".Localized());

            alraune.Root();
        }

        [Register("coc:alraune_uproot")]
        public static void Uproot(CombatContext context, CombatAbilityDefinition ability)
        {
            if (context.EnemyAs<AlrauneEnemy>() is not { } alraune || !alraune.Rooted)
                return;

            context.WriteAction("Scenes.Alraune.Uproot.UprootsFromNest".Localized());

            alraune.Uproot();
        }

        [Register("coc:alraune_is_rooted")]
        public static bool IsRooted(CombatContext context, CombatState state) =>

            context.EnemyAs<AlrauneEnemy>() is { Rooted: true };

        [Register("coc:alraune_is_uprooted")]
        public static bool IsUprooted(CombatContext context, CombatState state) =>

            context.EnemyAs<AlrauneEnemy>() is { Rooted: false };

        [Register("coc:alraune_can_entangle")]
        public static bool CanEntangle(CombatContext context, CombatState state) =>

            context.EnemyAs<AlrauneEnemy>() is { Rooted: true } && context.Player.CanMove();
    }
}