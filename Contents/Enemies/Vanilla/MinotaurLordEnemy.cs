using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.Handlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MinotaurLordEnemy>("coc:minotaur_lord")]
    public class MinotaurLordEnemy : Enemy
    {
        public static readonly Identifier AttackId = new("coc", "minotaur_lord_attack");

        public static readonly Identifier DrankMalkId = new("coc", "minotaur_lord_minotaur_drank_malk");

        public static readonly Identifier DisarmId = new("coc", "minotaur_lord_minotaur_disarm");

        public static readonly Identifier EntangleId = new("coc", "minotaur_lord_entangle");

        public static readonly Identifier PrecumTeaseId = new("coc", "minotaur_lord_minotaur_precum_tease");

        public static readonly Identifier CumPressId = new("coc", "minotaur_lord_cum_press");

        public bool UrtaQuest => Math.Abs(CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) - 0.75f) < 0.0001f;

        public override string Description => (UrtaQuest

            ? "Npcs.MinotaurLord.Desc.UrtaQuest"

            : "Npcs.MinotaurLord.Desc").Localized();

        public override Identifier? ForcedAbility(CombatContext context) =>

            context.Player.HasStatusEffect(StatusEffectIds.MinotaurEntangled) ? CumPressId : null;

        public override void TickStatuses(CombatContext context)
        {
            var timer = StatusEffectByType(StatusEffectIds.Timer);

            if (timer == null)
                return;

            if (timer.Values[0] <= 0)
                RemoveStatusEffect(StatusEffectIds.Timer);
            else
                timer.Values[0] -= 1;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;

            var choices = new List<(Identifier Id, float Weight)>();

            if (Health < 300 && StatusEffectValue(StatusEffectIds.MinoMilk, 0) < 4 && UrtaQuest)
                choices.Add((DrankMalkId, 1f));

            if (player.CanDisarm())
                choices.Add((DisarmId, 0.25f));

            if (!HasStatusEffect(StatusEffectIds.Timer) &&
                !player.HasStatusEffect(StatusEffectIds.MinotaurEntangled))

                choices.Add((EntangleId, 1f));

            choices.Add((PrecumTeaseId, 0.5f));

            choices.Add((AttackId, 2f));

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

        public override void Struggle(CombatContext context) =>

            MinotaurLordCombatHandlers.Struggle(context);

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            if (!context.Player.HasStatusEffect(StatusEffectIds.MinotaurEntangled))
                return true;

            context.Clear();

            context.Append("Combat.MinotaurLord.React.YouSighAnd".Localized());

            context.Player.TakeLustDamage(30 + context.Random.Next(5));

            return false;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(13 + 24), 2 + Random.Shared.Next(3), BodyPartTypes.Horse);

            Balls = 2;

            BallSize = 2 + Random.Shared.Next(13);

            CumMultiplier = 1.5f;

            HoursSinceCum = BallSize * 10;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 1;

            if (!HasStatusEffect(StatusEffectIds.BonusACapacity))
                CreateStatusEffect(StatusEffectIds.BonusACapacity, 50);

            Tallness = 132;

            Body.SetHipsRating("average");

            Body.SetButtRating("average", 1);

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Hoofed;

            var skin = Body.Get<SkinPart>();

            skin.Tone = "red";

            skin.Id = BodyPartTypes.Fur;

            skin.DescKey = "shaggy fur";

            var hair = Body.Get<HairPart>();

            hair.Color = Random.Shared.Next(2) == 0 ? "black" : "brown";

            hair.Length = 3;

            Body.Get<FacePart>().Id = BodyPartTypes.CowMinotaur;

            WeaponName = "chain";

            WeaponVerb = "chain-whip";

            WeaponAttack = 50;

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "thick fur";

            LustVulnerability = 0.33f;

            AdditionalExperience = 50;

            Gems = Random.Shared.Next(15) + 25;

            Body.Get<TailPart>().Id = BodyPartTypes.Cow;

        }

        public override bool Defeated(bool hpVictory)
        {
            Scene.ClearOutput();

            Scene.OutputText("Combat.MinotaurLord.Defeated.TheMinotaurLord".Localized());

            if (UrtaQuest)
            {
                Scene.OutputText("Combat.MinotaurLord.Defeated.YouCouldUse".Localized());

                Scene.ClearButtons();

                Scene.AddButton(0, "Common.Button.Fuck".Localized(), () => SceneInstance<UrtaQuestScene>.Get().WinRapeAMinoLordAsUrta());

                Scene.AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<UrtaQuestScene>.Get().BeatMinoLordOnToSuccubi());
            }
            else
                SceneInstance<MinotaurScene>.Get().MinoVictoryRapeChoices();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (UrtaQuest)
            {
                if (hpVictory)
                    SceneInstance<UrtaQuestScene>.Get().UrtaLosesToMinotaurRoughVersion();
                else
                    SceneInstance<UrtaQuestScene>.Get().UrtaSubmitsToMinotaurBadEnd();
            }
            else
                SceneInstance<MinotaurScene>.Get().GetRapedByMinotaur();

            return true;
        }
    }
}