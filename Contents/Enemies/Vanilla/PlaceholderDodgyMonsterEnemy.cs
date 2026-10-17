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

    [Register<PlaceholderDodgyMonsterEnemy>("coc:placeholder_dodgy_monster")]
    public class PlaceholderDodgyMonsterEnemy : Enemy
    {

        public readonly Dictionary<string, int> AbilityDodgeChance = [];

        public bool DodgeStance;

        public string TossedDagger = "";

        public static readonly Identifier DodgedAttack = new("coc", "dodged_attack");

        public static bool ShookDogHand => false;

        public static readonly Identifier AttackId = new("coc", "placeholder_dodgy_monster_attack");

        public static readonly Identifier DodgeExecId = new("coc", "placeholder_dodgy_monster_dodge_exec");

        public static readonly Identifier TossDaggerId = new("coc", "placeholder_dodgy_monster_toss_dagger");

        public static readonly Identifier RetrieveDaggerId = new("coc", "placeholder_dodgy_monster_retrieve_dagger");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(6, 2, BodyPartTypes.Dog);

            Balls = 2;

            BallSize = 3;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = 5 * 12 + 9;

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "light gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "light gray";

            hair.Length = 0;

            Body.Get<HornsPart>().Id = BodyPartTypes.None;

            WeaponName = "serrated daggers";

            WeaponVerb = "stab";

            WeaponAttack = 25;

            if (!ShookDogHand)
                WeaponAttack += 10;

            ArmorName = "leather armor";

            ArmorDefense = 5;

            Gems = Random.Shared.Next(5) + 250;

            CreatePerk(PerkIds.Prescience);

            CreatePerk(PerkIds.Bloodhound);

            CreatePerk(PerkIds.Tactician);

        }

        public override float GetEvasionChance()
        {
            var chance = base.GetEvasionChance();

            if (CombatController.Instance?.CurrAbilityUsed is { } used
                && AbilityDodgeChance.TryGetValue(used.Path, out var extra))

                chance += extra;

            return chance;
        }

        public override bool BeforeCombatAction(CombatContext context)
        {

            if (CombatController.Instance?.CurrAbilityUsed is { } used)
            {
                var amount = ShookDogHand ? 20 : 15;

                AbilityDodgeChance[used.Path] = AbilityDodgeChance.GetValueOrDefault(used.Path) + amount;
            }

            var options = new List<(Identifier Id, int Cost, FatigueType Fatigue, float Weight)>();

            if (HasFatigue(10, FatigueType.Physical))
                options.Add((AttackId, 10, FatigueType.Physical, 1));

            if (DodgeStance && HasStatusEffect(DodgedAttack) && HasFatigue(10, FatigueType.Physical))
                options.Add((DodgeExecId, 10, FatigueType.Physical, 99));

            if (Distance == CombatDistance.Distant && HasFatigue(10, FatigueType.Physical))
                options.Add((TossDaggerId, 10, FatigueType.Physical, 1));

            if (TossedDagger != "" && HasFatigue(5, FatigueType.Physical))
                options.Add((RetrieveDaggerId, 5, FatigueType.Physical, 3));

            if (options.Count == 0)

                Execute(context, new CombatAbilityDefinition { Id = AttackId, Handler = AttackId }, 0, FatigueType.None);
            else
            {
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

                Execute(context, ability, chosen.Cost, chosen.Fatigue);
            }

            if (HasFatigue(10, FatigueType.Physical) && context.Random.Next(4) == 0 && !DodgeStance)
                DodgePrepare(context);
            else
                DodgeStance = false;

            return true;
        }

        public void Execute(CombatContext context, CombatAbilityDefinition ability, int cost, FatigueType fatigue)
        {
            if (cost > 0)
                ChangeFatigue(cost, fatigue);

            CombatController.Instance!.Resolver.EnemyAction(ability);
        }

        public void DodgePrepare(CombatContext context)
        {
            context.Append("Combat.PlaceholderDodgyMonster.DodgePrepare.LowersHisWeapons".Localized());

            DodgeStance = true;
        }

    }
}