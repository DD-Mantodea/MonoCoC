using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<IvorySuccubusEnemy>("coc:ivory_succubus")]
    public class IvorySuccubusEnemy : Enemy
    {

        public int FlightCooldown;

        public static readonly Identifier PermaFly = new("coc", "perma_fly");

        public static readonly Identifier Attracted = new("coc", "attracted");

        public static readonly Identifier LashId = new("coc", "ivory_succubus_lash");

        public static readonly Identifier TeaseId = new("coc", "ivory_succubus_tease");

        public static readonly Identifier FlightId = new("coc", "ivory_succubus_flight");

        public static readonly Identifier BlindId = new("coc", "ivory_succubus_blind");

        public static readonly Identifier WhitefireId = new("coc", "ivory_succubus_whitefire");

        public static readonly Identifier ChargeId = new("coc", "ivory_succubus_charge");

        public static readonly Identifier ArouseId = new("coc", "ivory_succubus_arouse");

        public static readonly Identifier AttractId = new("coc", "ivory_succubus_attract");

        public static readonly Identifier LockUpId = new("coc", "ivory_succubus_lock_up");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 2, 2);

            CreateBreastRow(BreastCups.B);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            Tallness = 73;

            Body.SetHipsRating("average");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Color = "light gray";

            hair.Length = 4;

            Lust = 17 + Random.Shared.Next(7);

            WeaponName = "whip";

            WeaponVerb = "lash";

            WeaponAttack = 35;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "ivory corset";

            ArmorDefense = 10;

            Fatigue = 0;

            Gems = Random.Shared.Next(10) + 15;

        }

        public override void CombatRoundUpdate(CombatContext context)
        {
            if (HasStatusEffect(PermaFly))
                ChangeFatigue(5);

            base.CombatRoundUpdate(context);
        }

        public override bool BeforeCombatAction(CombatContext context)
        {
            FlightCooldown--;

            if (HasStatusEffect(PermaFly)
                && (FlightCooldown < 4 || !HasFatigue(5, FatigueType.Physical)))
            {
                Land(context);

                return true;
            }

            var options = new List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue)>
            {
                (LashId, 4, 0, FatigueType.None),

                (TeaseId, 2, 0, FatigueType.None)
            };

            if (FlightCooldown < 1)
                options.Add((FlightId, 1, 0, FatigueType.None));

            var lowLustWeight = Lust < 50 ? 1f : 0.25f;

            var highLustWeight = Lust >= 50 ? 1f : 0.25f;

            AddSpell(options, BlindId, lowLustWeight, 20);

            AddSpell(options, WhitefireId, lowLustWeight, 15);

            AddSpell(options, ChargeId, lowLustWeight, 10);

            AddSpell(options, ArouseId, highLustWeight, 10);

            if (!context.Player.HasStatusEffect(Attracted))
                AddSpell(options, AttractId, highLustWeight, 15);

            AddSpell(options, LockUpId, highLustWeight, 20);

            if (options.Count == 0)
            {

                CombatController.Instance!.Resolver.EnemyBasicAttack();

                return true;
            }

            var total = 0f;

            foreach (var option in options)
                total += option.Weight;

            var roll = context.Random.NextDouble() * total;

            var chosen = options[^1];

            foreach (var option in options)
            {
                roll -= option.Weight;

                if (roll <= 0)
                {
                    chosen = option;

                    break;
                }
            }

            var ability = CombatAbilityRegistry.Get(chosen.Id)

                ?? new CombatAbilityDefinition { Id = chosen.Id, Handler = chosen.Id };

            if (chosen.Cost > 0)
                ChangeFatigue(chosen.Cost, chosen.Fatigue);

            CombatController.Instance!.Resolver.EnemyAction(ability);

            return true;
        }

        public void AddSpell(
            List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue)> options,

            Identifier id,

            float weight,

            int cost)
        {
            if (HasFatigue(cost, FatigueType.Magical))
                options.Add((id, weight, cost, FatigueType.Magical));
        }

        public void Land(CombatContext context)
        {
            context.Append("Combat.IvorySuccubus.Land.TheSuccubusHolds".Localized());

            RemoveStatusEffect(PermaFly);

            CombatRangeData.MoveDistantSafe(this);
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.IvorySuccubusScene>
                .Get().Defeated(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Forest.IvorySuccubusScene>
                .Get().Won(hpVictory);

            return true;
        }
    }
}
