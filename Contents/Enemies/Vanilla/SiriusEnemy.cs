using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<SiriusEnemy>("coc:sirius")]
    public class SiriusEnemy : Enemy
    {
        public static readonly Identifier AttackId = new("coc", "sirius_attack");

        public static readonly Identifier PoisonBiteId = new("coc", "sirius_poison_bite");

        public static readonly Identifier TeaseId = new("coc", "sirius_man_naga_tease");

        public static readonly Identifier SpitId = new("coc", "sirius_naga_spit_attack");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)>
            {
                (AttackId, 1f),

                (PoisonBiteId, 1f),

                (TeaseId, 1f)
            };

            if (context.Player.HasStatusEffect(StatusEffectIds.Blind))
                choices.Add((SpitId, 1f));

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

        public override void OutputAttack(CombatContext context, float damage)
        {
            if (damage <= 0)
            {
                base.OutputAttack(context, damage);

                return;
            }

            context.Append("Combat.Sirius.OutputAttack.YouMisjudgeHis".Localized("#ff0000", (int)damage));
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(14, 2);

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;

            anal.Wetness = 0;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 10);

            Tallness = 5 * 12 + 10;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Naga;

            Body.Get<SkinPart>().Tone = "mediterranean-toned";

            var hair = Body.Get<HairPart>();

            hair.Color = "orange";

            hair.Length = 16;

            WeaponName = "fangs";

            WeaponVerb = "bite";

            WeaponAttack = 25;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "scales";

            ArmorDefense = 30;

            Gems = Random.Shared.Next(5) + 8;

        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaBeatsUpSiriusRadio();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaLosesToSirriusSnakeRadio();

            return true;
        }
    }
}