using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Desert;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ScorpionEnemy>("coc:scorpion")]
    public class ScorpionEnemy : Enemy
    {

        public int TurnsWaited;

        public static readonly Identifier AttackId = new("coc", "scorpion_attack");

        public static readonly Identifier GrabId = new("coc", "scorpion_grab");

        public static readonly Identifier StingId = new("coc", "scorpion_sting");

        public static readonly Identifier SlamId = new("coc", "scorpion_slam");

        public static readonly Identifier SandId = new("coc", "scorpion_sand");

        public static readonly Identifier TrampleId = new("coc", "scorpion_trample");

        public static readonly Identifier HammerId = new("coc", "scorpion_hammer");

        public static readonly Identifier ScuttleId = new("coc", "scorpion_scuttle");

        public static readonly Identifier IdleId = new("coc", "scorpion_idle");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateBreastRow(0f);

            Body.SetHipsRating("slender");

            Body.SetButtRating("average");

            Tallness = 228;

            WeaponName = "giant chelae";

            ArmorName = "onyx cephalothorax";

            LustVulnerability = 0;

            CreatePerk(PerkIds.BiteImmune);

            TurnsWaited = 0;
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ScorpionScene>.Get().Defeated();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ScorpionScene>.Get().BadEnd(hpVictory);

            return true;
        }

        public override void Struggle(CombatContext context) => ScorpionCombatHandlers.Struggle(context);

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (context.PlayerWaited)
                TurnsWaited++;
            else
                TurnsWaited = 0;

            var threshold = context.Player.HasTailInsteadOfLegs() ? 5 : 8;

            if (Talked)
                threshold -= 2;

            if (TurnsWaited >= threshold)
            {

                context.EndOfRoundOverride = () => SceneInstance<ScorpionScene>.Get().Special();

                return true;
            }

            if (context.Player.HasStatusEffect(StatusEffectIds.ScorpGrabbed))
            {
                ScorpionCombatHandlers.Grapple(context);

                return true;
            }

            return false;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;

            var specialChance = (float)Math.Round((100 - HealthRatio() * 100) / 15d);

            var choices = new System.Collections.Generic.List<(Identifier Ability, float Weight)>
            {
                (AttackId, 5)
            };

            if (!context.PlayerWaited)
                choices.Add((GrabId, 1));

            if (!player.HasStatusEffect(StatusEffectIds.ScorpVenom))
                choices.Add((StingId, specialChance));

            choices.Add((SlamId, specialChance));

            choices.Add((SandId, specialChance));

            choices.Add((TrampleId, specialChance));

            if (!player.HasStatusEffect(StatusEffectIds.Stunned))
                choices.Add((HammerId, specialChance));

            if (HealthRatio() * 100 < 33)
                choices.Add((ScuttleId, 2));

            choices.Add((IdleId, context.PlayerWaited ? 15 : 1));

            var total = 0f;

            foreach (var (_, weight) in choices)
                if (weight > 0)
                    total += weight;

            if (total <= 0)
                return new CombatAiAction { Ability = AttackId };

            var roll = context.Random.NextDouble() * total;

            foreach (var (ability, weight) in choices)
            {
                if (weight <= 0)
                    continue;

                roll -= weight;

                if (roll <= 0)
                    return new CombatAiAction { Ability = ability };
            }

            return new CombatAiAction { Ability = AttackId };
        }

        public override float ModifyIncomingDamage(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (kind == CombatDamageKind.Magical)
                damage *= 0.5f;

            if (kind == CombatDamageKind.Melee && context.Player.Weapon.IsFirearm())
                damage *= 1.5f;

            return damage;
        }

        public override bool OnPlayerRunAttempt(CombatContext context)
        {
            context.Clear();

            context.WriteAction(context.Player.CanFly()

                ? "Scenes.Scorpion.RunCheck.Wings".Localized()

                : "Scenes.Scorpion.RunCheck.Climb".Localized());

            context.WriteAction("Scenes.Scorpion.RunCheck.Sorrow".Localized());

            var damage = CombatCalculator.ReduceDamage(context, context.Player, 50);

            context.Player.HealthChange(-damage);

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            if (reaction is CombatReactionContext.BeforeAttacked or CombatReactionContext.WhenAttacked)
            {
                if (player.Weapon.IsFirearm() && context.Random.Next(100) < 5)
                    return Blocked(context, "Scenes.Scorpion.React.Firearm.");

                if (!player.Weapon.IsChanneling(player) && context.Random.Next(100) < 10)
                    return Blocked(context, "Scenes.Scorpion.React.Melee.");
            }

            if (reaction == CombatReactionContext.BowHit && context.Random.Next(100) < 90)
                return Blocked(context, "Scenes.Scorpion.React.Bow.");

            return true;
        }

        public static bool Blocked(CombatContext context, string prefix) =>

            Blocked(context, prefix, context.Random.Next(5));

        public static bool Blocked(CombatContext context, string prefix, int version)
        {
            context.WriteAction((prefix + "V" + version).Localized());

            return false;
        }

        public override void OnTeased(CombatContext context, float lustDelta) =>

            context.WriteAction(("Scenes.Scorpion.Tease.V" + context.Random.Next(7)).Localized());

        public override void OutputDefaultFantasy(CombatContext context, float lustDamage)
        {
            var version = context.Random.Next(6);

            var text = ("Scenes.Scorpion.Fantasy.V" + version).Localized();

            if (version == 5)
            {
                if (context.Player.IsFeminine() && context.Player.DemonScore() >= 4)
                    text += "Scenes.Scorpion.Fantasy.V5Suffix".Localized();

                text += ".";
            }

            context.WriteAction(text);
        }

        public override bool ShouldMove(CombatDistance newPos, bool forceAction = false) => false;

        public override bool ShouldWait() => false;

        public static bool Talked =>

            SceneSaveRegistry.For(new Identifier("coc", "ScorpionScene")).GetBool("talked");
    }
}