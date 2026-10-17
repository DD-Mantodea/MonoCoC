using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ValaEnemy>("coc:vala")]
    public class ValaEnemy : Enemy
    {
        public static readonly Identifier Special1Id = new("coc", "vala_special1");

        public static readonly Identifier Special2Id = new("coc", "vala_special2");

        public static readonly Identifier MasturbateId = new("coc", "vala_masturbate");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 3, 0);

            CreateStatusEffect(StatusEffectIds.BonusVCapacity, 25);

            CreateBreastRow(BreastCups.E);

            CreateStatusEffect(StatusEffectIds.BonusACapacity, 10);

            Tallness = 4 * 12;

            Body.SetHipsRating("curvy", 2);

            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "fair";

            var hair = Body.Get<HairPart>();

            hair.Color = "purple";

            hair.Length = 22;

            WeaponName = "fists";

            WeaponVerb = "caresses";

            ArmorName = "skin";

            var times = CoC.Instance.Flags.Get(GameFlagIds.TimesPcDefeatedVala);

            LustVulnerability = 0.5f;

            if (times > 0)
                LustVulnerability += 0.25f;

            if (times > 2)
                LustVulnerability += 0.5f;

            Lust = Math.Min(80, 30 + times * 10);
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            // 原版 Vala.hx:86 performCombatAction 开头先播对白
            ValaCombatDialogue(context);

            var choices = new List<(Identifier Id, int Weight)>();

            if (Health < MaxHealth * 0.85f)
                choices.Add((Special1Id, 25));

            choices.Add((Special2Id, 20));

            choices.Add((MasturbateId, 55));

            var total = 0;

            foreach (var choice in choices)
                total += choice.Weight;

            var pick = Random.Shared.Next(total);

            foreach (var choice in choices)
            {
                if (pick < choice.Weight)
                    return new CombatAiAction { Ability = choice.Id };

                pick -= choice.Weight;
            }

            return new CombatAiAction { Ability = MasturbateId };
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.TimesPcDefeatedVala,

                CoC.Instance.Flags.Get(GameFlagIds.TimesPcDefeatedVala) + 1);

            CombatController.Instance?.Context.Append("Dungeons.Vala.Victory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Dungeons.Vala.Defeat".Localized());

            return false;
        }

        // 原版 Vala.hx:62 valaCombatDialogue
        private void ValaCombatDialogue(CombatContext context)
        {
            if (!HasStatusEffect(StatusEffectIds.Vala))
            {
                context.Append("Scenes.Vala.ValaCombatDialogue.SlutsServeMasters".Localized());

                CreateStatusEffect(StatusEffectIds.Vala, 0, 0, 0, 0);

                return;
            }

            ChangeStatusValue(StatusEffectIds.Vala, 0, 1);

            var stage = (int)StatusEffectValue(StatusEffectIds.Vala, 0);

            context.Append((stage switch
            {
                1 => "Scenes.Vala.ValaCombatDialogue.NotAMaster",

                2 => "Scenes.Vala.ValaCombatDialogue.KeepsYouPet",

                3 => "Scenes.Vala.ValaCombatDialogue.ObedientPets",

                4 => "Scenes.Vala.ValaCombatDialogue.ToySlutPitch",

                5 => "Scenes.Vala.ValaCombatDialogue.BirthsMastersChildren",

                _ => "Scenes.Vala.ValaCombatDialogue.ChildrenUseHer",
            }).Localized());
        }
}
}