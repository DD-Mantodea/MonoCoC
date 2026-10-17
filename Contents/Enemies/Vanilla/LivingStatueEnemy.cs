using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<LivingStatueEnemy>("coc:living_statue")]
    public class LivingStatueEnemy : Enemy
    {
        public static readonly Identifier ConcussiveBlowId = new("coc", "living_statue_concussive_blow");

        public static readonly Identifier DirtKickId = new("coc", "living_statue_dirt_kick");

        public static readonly Identifier BackhandId = new("coc", "living_statue_backhand");

        public static readonly Identifier OverhandSmashId = new("coc", "living_statue_overhand_smash");

        public static readonly Identifier DisarmId = new("coc", "living_statue_disarm");

        public static readonly Identifier CycloneStrikeId = new("coc", "living_statue_cyclone_strike");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Race = "Statue";

            Strength = 100;

            Toughness = 80;

            Speed = 25;

            Intelligence = 50;

            Libido = 10;

            Sensitivity = 10;

            Corruption = 0;

            LustVulnerability = 0f;

            Tallness = 192;

            CreateBreastRow(0, 1);

            FireResistance = 0f;

            Level = 22;

            var bonusHealth = 1000f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            WeaponName = "stone greathammer";

            WeaponVerb = "smash";

            WeaponAttack = 25;

            ArmorName = "cracked stone";

            CreatePerk(PerkIds.StunImmune);

            CreatePerk(PerkIds.PoisonImmune);

            CreatePerk(PerkIds.BleedImmune);

            CreatePerk(PerkIds.BlindImmune);
        }

        public override bool HandleStun(CombatContext context)
        {
            context.WriteAction("Combat.LivingStatue.HandleStun.TheStoneGiant".Localized());

            RemoveStatusEffect(StatusEffectIds.Stunned);

            return true;
        }

        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.LivingStatue.HandleFear.TheStoneGiant".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.Blinded)
                return true;

            context.Append("Combat.LivingStatue.React.TheGolemDoesn".Localized());

            return false;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;

            if (Health < MaxHealth * 0.7f &&
                Distance != CombatDistance.Distant &&

                context.Random.Next(2) == 0)

                return new CombatAiAction { Ability = BackhandId };

            if (Health < MaxHealth * 0.4f &&
                !HasStatusEffect(StatusEffectIds.Disarmed) &&

                player.CanDisarm() &&

                context.Random.Next(2) == 0)

                return new CombatAiAction { Ability = DisarmId };

            var options = new List<Identifier>();

            if (!player.HasStatusEffect(StatusEffectIds.Blind) &&
                !player.HasStatusEffect(StatusEffectIds.Stunned))
            {
                options.Add(DirtKickId);

                options.Add(ConcussiveBlowId);
            }

            options.Add(CycloneStrikeId);

            options.Add(CycloneStrikeId);

            options.Add(OverhandSmashId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        public override bool DescribeAttacked(CombatContext context, float damage)
        {
            var player = context.Player;

            var weapon = player.Weapon;

            if (weapon.IsChanneling(player))
                return false;

            var monster = Article + ShortName;

            var noun = weapon.Name;

            var verb = string.IsNullOrEmpty(weapon.Verb) ? "attack" : weapon.Verb;

            var damageLow = damage < 15 || damage < MaxHealth * 0.05f;

            var damageMed = damage < 50 || damage < MaxHealth * 0.20f;

            var damageHigh = damage < 100 || damage < MaxHealth * 0.33f;

            if (damage <= 0)
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Bounce".Localized(noun, monster));
            else if (damageLow)
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Glancing".Localized(monster));
            else if (damageMed)
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Medium".Localized(verb, monster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.High".Localized(monster, noun));
            else
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Shatter".Localized(monster, noun));

            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.D3StatueDefeated, 1);

            SceneInstance<LivingStatueScenes>.Get().BeatUpDaStatue(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<LivingStatueScenes>.Get().FuckinMarbleOP(hpVictory);

            return true;
        }
    }
}