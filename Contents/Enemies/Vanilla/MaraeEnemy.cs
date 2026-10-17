using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
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

    [Register<MaraeEnemy>("coc:marae")]
    public class MaraeEnemy : Enemy
    {
        public int _factoryShutdown;

        public static readonly Identifier AttackId = new("coc", "marae_attack");

        public static readonly Identifier SmiteId = new("coc", "marae_smite");

        public static readonly Identifier SmiteHitId = new("coc", "marae_smite_hit");

        public static readonly Identifier TentacleAttackId = new("coc", "marae_tentacle_attack");

        public static readonly Identifier TentacleRapeId = new("coc", "marae_tentacle_rape");

        public bool Corrupted => _factoryShutdown == 2;

        public override string Description => (Corrupted

            ? "Npcs.Marae.Desc.Corrupted"

            : "Npcs.Marae.Desc").Localized();

        public override Identifier? ForcedAbility(CombatContext context) =>

            HasStatusEffect(StatusEffectIds.Uber) ? SmiteHitId : null;

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 3f) };

            if (_factoryShutdown == 1)
                choices.Add((SmiteId, 3f));

            if (_factoryShutdown == 2)
            {
                choices.Add((TentacleAttackId, 3f));

                choices.Add((TentacleRapeId, 3f));
            }

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

        public override bool HandleFear(CombatContext context)
        {
            context.Append("Combat.Marae.HandleFear.MaraeSnarls".Localized());

            RemoveStatusEffect(StatusEffectIds.Fear);

            return true;
        }

        public override bool OnPlayerRunAttempt(CombatContext context)
        {
            context.Append("Combat.Marae.RunCheck.YourBoatIs".Localized());

            context.Append((context.Player.CanFly()

                ? "Combat.Marae.RunCheck.YouGritYour"

                : "Combat.Marae.RunCheck.YouMayNot").Localized());

            context.Append("Combat.Marae.RunCheck.ItLooksLike".Localized());

            return true;
        }

        public override void Configure(EnemyDefinition definition)
        {
            _factoryShutdown = CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown);

            base.Configure(definition);

            if (Corrupted)
            {
                CreateVagina(false, 4, 1); 

                CreateBreastRow(BreastCups.G);

                Strength = 150;

                Toughness = 150;

                Speed = 70;

                Intelligence = 110;

                Libido = 60;

                Sensitivity = 25;

                Corruption = 100;

                WeaponName = "tentacles";

                WeaponVerb = "slap";

                WeaponAttack = 40;

                NormalAttackAvoid.DoParry = false;

                NormalAttackAvoid.DoCounter = false;
            }
            else
            {
                CreateVagina(false, 2, 1); 

                CreateBreastRow(BreastCups.DD);

                Strength = 200;

                Toughness = 150;

                Speed = 100;

                Intelligence = 150;

                Libido = 25;

                Sensitivity = 25;

                Corruption = 0;

                WeaponName = "fists";

                WeaponVerb = "wrathful punch";

                WeaponAttack = 50;

                NormalAttackAvoid.DoParry = false;

                NormalAttackAvoid.DoCounter = false;
            }

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 1;

            Tallness = 120;

            Body.SetHipsRating("curvy");

            Body.SetButtRating("large");

            var skin = Body.Get<SkinPart>();

            skin.Tone = "white";

            skin.Id = BodyPartTypes.Plain;

            var hair = Body.Get<HairPart>();

            hair.Color = "green";

            hair.Length = 36;

            ArmorName = "bark";

            ArmorDefense = 30;

            var bonusHealth = 4750f;

            if (_factoryShutdown == 1)
            {
                bonusHealth += 2700;

                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                    bonusHealth += 1000;
            }

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            LustVulnerability = 0.07f;

            AdditionalExperience = _factoryShutdown == 1 ? 3000 : 2500;

            Gems = 1000;

            if (!HasPerk(PerkIds.Tank))
                CreatePerk(PerkIds.Tank);

            if (!HasPerk(PerkIds.Tank2))
                CreatePerk(PerkIds.Tank2);

            if (!HasPerk(PerkIds.ImprovedSelfControl))
                CreatePerk(PerkIds.ImprovedSelfControl);

            Stats.AddModifier(GameStats.MaxLust, ModifierOperation.Addition,

                () => 80f, new TemporarySource(Guid.NewGuid()));

        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Boat.MaraeScene>.Get().WinAgainstMarae();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Places.Boat.MaraeScene>.Get().LoseAgainstMarae();

            return true;
        }
    }
}