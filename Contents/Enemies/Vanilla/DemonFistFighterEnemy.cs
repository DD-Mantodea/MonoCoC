using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<DemonFistFighterEnemy>("coc:demon_fist_fighter")]
    public class DemonFistFighterEnemy : Enemy
    {

        public static readonly Identifier AngeredPugilist = new("coc", "angered_pugilist");

        public bool AngryDemonfist;

        public bool HasAttemptedBSwordDisarm;

        public static readonly Identifier QuickJabId = new("coc", "demon_fist_fighter_quick_jab");

        public static readonly Identifier TwoFerId = new("coc", "demon_fist_fighter_two_fer");

        public static readonly Identifier AttackId = new("coc", "demon_fist_fighter_attack");

        public static readonly Identifier RoundhouseId = new("coc", "demon_fist_fighter_roundhouse");

        public static readonly Identifier CrossPunchId = new("coc", "demon_fist_fighter_cross_punch");

        public static readonly Identifier DazedStatus = new("coc", "dazed");

        public override void TickStatuses(CombatContext context)
        {
            var player = context.Player;

            var dazed = player.StatusEffectByType(DazedStatus);

            if (dazed == null)
                return;

            if (dazed.Values[0] <= 0)
            {
                RemoveDazed(player);

                return;
            }

            dazed.Values[0] -= 1;

            if (dazed.Values[0] <= 0)
                RemoveDazed(player);
        }

        public static readonly Identifier DazedBuffSourceId = new("coc", "combat_buff_dazed");

        public static void ApplyDazed(Player player, int duration, float accuracyDecrease, float damageFactor)
        {
            var existing = player.StatusEffectByType(DazedStatus);

            if (existing == null)
                player.CreateStatusEffect(DazedStatus, duration, accuracyDecrease, damageFactor);
            else
            {
                existing.Values[0] = duration;

                existing.Values[1] = accuracyDecrease;

                existing.Values[2] = damageFactor;
            }

            var source = new StatusSource(DazedBuffSourceId, "Dazed");

            player.Stats.RemoveFrom(source);

            CombatCalculator.AddCombatBuff(player, GameStats.Accuracy, accuracyDecrease, "Dazed", DazedStatus);

            player.Stats.AddModifier(GameStats.PhysDamage, ModifierOperation.Factor,

                () => damageFactor, source);
        }

        public static void RemoveDazed(Player player)
        {
            player.RemoveStatusEffect(DazedStatus);

            player.Stats.RemoveFrom(new StatusSource(DazedBuffSourceId, "Dazed"));
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            if (Fatigue >= MaxFatigue() && !AngryDemonfist)
            {
                context.Append("Combat.DemonFistFighter.Faint.TheDemonS".Localized());

                Health = 0;

                return true;
            }

            var last = CombatController.Instance?.CurrAbilityUsed;

            if (last != null && !AngryDemonfist &&
                CombatAbilityRegistry.Get(last.Value)?.Type == CombatAbilityType.Magical)
            {
                context.Append("Combat.DemonFistFighter.GetAngry.TheDemonScowls".Localized());

                if (!HasStatusEffect(AngeredPugilist))
                    CreateStatusEffect(AngeredPugilist, 99);

                AngryDemonfist = true;

                return true;
            }

            return false;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)>
            {
                (QuickJabId, 1f),

                (TwoFerId, 1f),

                (AttackId, 1f),

                (RoundhouseId, context.Player.ShieldName is "" or "nothing" ? 0.5f : 0.75f),

                (CrossPunchId, 1f)
            };

            var total = 0f;

            foreach (var (_, weight) in choices)
                total += weight;

            var roll = (float)context.Random.NextDouble() * total;

            foreach (var (id, weight) in choices)
            {
                roll -= weight;

                if (roll < 0)
                    return new CombatAiAction { Ability = id };
            }

            return new CombatAiAction { Ability = choices[^1].Id };
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            context.Clear();

            context.Append("Combat.DemonFistFighter.React.YouDecideTo".Localized());

            var silly = Configuration.OptionStore.Instance.Get("silly", 0) > 0;

            var taunt = context.Random.Next(silly ? 4 : 3);

            context.Append(("Combat.DemonFistFighter.React.Taunt" + taunt).Localized());

            ChangeFatigue(-5);

            TookAction = true;

            return false;
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.DemonsDefeated, CoC.Instance.Flags.Get(GameFlagIds.DemonsDefeated) + 1);

            return false;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(2) + 11, 2.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 1;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;

            anal.Wetness = 1;

            Tallness = Random.Shared.Next(24) + 25;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "indigo";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 5;

            WeaponName = "fists";

            WeaponVerb = "jab";

            ArmorName = "leathery skin";

            Gems = Random.Shared.Next(5) + 5;

            if (!HasPerk(PerkIds.ExtraDodge))
                CreatePerk(PerkIds.ExtraDodge, 75);

            LustVulnerability = 0.1f;

        }
    }
}