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

    [Register<MilkySuccubusEnemy>("coc:milky_succubus")]
    public class MilkySuccubusEnemy : Enemy
    {

        public static readonly Identifier MilkSprayId = new("coc", "milky_succubus_cow_cubi_milk_spray_attack");

        public static readonly Identifier DrinkCumId = new("coc", "milky_succubus_drink_mino_cum");

        public static readonly Identifier AttackId = new("coc", "milky_succubus_attack");

        public static readonly Identifier TeaseId = new("coc", "milky_succubus_succubus_tease");

        public int _cumAvailable = 2;

        public bool TryDrinkCum()
        {
            if (_cumAvailable <= 0)
                return false;

            _cumAvailable -= 1;

            return true;
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var article = Article;

            if (article.Length > 0)
                article = char.ToUpperInvariant(article[0]) + article[1..];

            context.Append("Combat.MilkySuccubus.Teased.OneSmiles".Localized(article, ShortName));

            Stats.AddBase(GameStats.Strength, 5);

            HealthChange(50);
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;

            var choices = new List<(Identifier Id, float Weight)>();

            if (!HasStatusEffect(StatusEffectIds.MilkyUrta))
                choices.Add((MilkSprayId, 0.3f));

            if (Health < 400)
                choices.Add((DrinkCumId, 2f));

            if (player.Health < 100)
                choices.Add((AttackId, 1f));

            if (player.LustPercent >= 90)
                choices.Add((TeaseId, 1f));

            choices.Add((AttackId, 1f));

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

        public override void TickStatuses(CombatContext context)
        {
            var milky = StatusEffectByType(StatusEffectIds.MilkyUrta);

            if (milky == null)
                return;

            var player = context.Player;

            context.Append("Scenes.UrtaQuest.MilkyUrtaTic.MilkHeatRising".Localized());

            context.Append((player.Lust < 50

                ? "Scenes.UrtaQuest.MilkyUrtaTic.MildDistraction"

                : player.Lust < 75

                    ? "Scenes.UrtaQuest.MilkyUrtaTic.WaningResistance"

                    : "Scenes.UrtaQuest.MilkyUrtaTic.TemptingOrbs").Localized());

            context.Append("Scenes.UrtaQuest.MilkyUrtaTic.ParagraphBreak".Localized());

            milky.Values[0] -= 1;

            if (milky.Values[0] <= 0)
                RemoveStatusEffect(StatusEffectIds.MilkyUrta);

            player.Lust = Math.Min(player.MaxLust, player.Lust + 10);
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 1);

            if (!HasStatusEffect(StatusEffectIds.BonusVCapacity))
                CreateStatusEffect(StatusEffectIds.BonusVCapacity, 300);

            CreateBreastRow(BreastCups.G);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 5;

            Tallness = 84;

            Body.SetHipsRating("curvy");

            Body.SetButtRating("large", 1);

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.DemonicHighHeels;

            Body.Get<SkinPart>().Tone = "blue";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 13;

            WeaponName = "whip";

            WeaponVerb = "whipping";

            WeaponAttack = 10;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "demonic skin";

            ArmorDefense = 10;

            LustVulnerability = 0.3f;

            Gems = Random.Shared.Next(25) + 10;

            AdditionalExperience = 50;

            var horns = Body.Get<HornsPart>();

            horns.Id = BodyPartTypes.DraconicX2;

            horns.Value = 2;

            Body.Get<WingsPart>().Id = BodyPartTypes.BatLikeTiny;

            Body.Get<TailPart>().Id = BodyPartTypes.Demonic;

        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaBeatsUpCowcubi();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaLosesToCowCubi();

            return true;
        }
    }
}