using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<JellyFishGirlEnemy>("coc:jelly_fish_girl")]
    public class JellyFishGirlEnemy : Enemy
    {

        public string _element = "blue";

        public string _elementLong = "blue";

        public bool _elementInjected;

        public static JellyFishGirlEnemy Create(string element, string elementLong)
        {
            var id = new Identifier("coc", "jelly_fish_girl");

            var definition = EnemyRegistry.Definitions.Get(id);

            var enemy = new JellyFishGirlEnemy();

            enemy.SetElement(element, elementLong);

            enemy.Id = definition.Id;

            enemy.Configure(definition);

            enemy.ApplyDefinitionEquipment(definition);

            return enemy;
        }

        public void SetElement(string element, string elementLong)
        {
            _element = element;

            _elementLong = elementLong;

            _elementInjected = true;
        }

        public static readonly Identifier AttackId = new("coc", "jelly_fish_girl_attack");

        public static readonly Identifier GrappleId = new("coc", "jelly_fish_girl_jelly_grapple");

        public static readonly Identifier MolestId = new("coc", "jelly_fish_girl_molest_player");

        public override string Description => "Npcs.JellyFishGirl.Desc".Localized(_elementLong);

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var grappled = context.Player.HasStatusEffect(StatusEffectIds.Grappled);

            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 3f) };

            if (grappled)
                choices.Add((MolestId, 5f));
            else
                choices.Add((GrappleId, 1f));

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

        public override void CombatRoundUpdate(CombatContext context)
        {
            if (Lust > 0)
                HealthChange(Lust / 5f);
        }

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.Distanced)
                return true;

            JellyFishGirlCombatHandlers.JellyGrapple(context);

            return !context.Player.HasStatusEffect(StatusEffectIds.Grappled);
        }

        public override void Struggle(CombatContext context) =>

            JellyFishGirlCombatHandlers.Struggle(context);

        public override void OutputAttack(CombatContext context, float damage)
        {
            context.Append("Combat.JellyFishGirl.OutputAttack.TheJellyGirl".Localized());

            if (CoC.Instance.Flags.Get(Flags.GameFlagIds.EnemyCritical) > 0)
                context.Append("Combat.JellyFishGirl.OutputAttack.Text".Localized());

            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            if (!_elementInjected)
            {
                var roll = Random.Shared.Next(5);

                _element = roll switch
                {
                    0 or 1 => "blue",

                    2 or 3 => "purple",

                    _ => "crystal"
                };

                var longChoices = _element switch
                {
                    "purple" => new[] { "purple", "violet", "plum", "amethyst" },

                    "crystal" => ["glassy", "crystal", "prismatic"],

                    _ => ["blue", "azure", "cerulean", "sapphire"]
                };

                _elementLong = longChoices[Random.Shared.Next(longChoices.Length)];
            }

            CreateVagina(false, 1, 1);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Tallness = 36;

            CreateBreastRow(BreastCups.A);

            Body.SetHipsRating("boyish");

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = _elementLong;

            var hair = Body.Get<HairPart>();

            hair.Color = _element;

            hair.Length = 16;

            WeaponName = "tentacles";

            WeaponVerb = "tentacles";

            WeaponAttack = 7;

            ArmorName = "tough skin";

            ArmorDefense = 5;

            LustVulnerability = 0.9f;

            FireResistance = 0.3f;

            Gems = Random.Shared.Next(15) + 5;

            if (!HasPerk(PerkIds.PoisonImmune))
                CreatePerk(PerkIds.PoisonImmune);

        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Swim.JellyFishGirlScene>.Get().JellyWinChoices(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Swim.JellyFishGirlScene>.Get().JellyLossChoices(hpVictory);

            return true;
        }
    }
}