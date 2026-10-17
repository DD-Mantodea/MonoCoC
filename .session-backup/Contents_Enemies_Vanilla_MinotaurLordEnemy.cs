using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 MinotaurLord extends Monster（src/classes/scenes/quests/urtaQuest/MinotaurLord.hx）
    // 只移植类本体；defeated/won 依赖 Urta 任务线与 MinotaurScene（原版 MinotaurLord.hx:156-179）。
    [Register("coc:minotaur_lord")]
    public class MinotaurLordEnemy : Enemy
    {
        private static readonly Identifier AttackId = new("coc", "minotaur_lord_attack");
        private static readonly Identifier DrankMalkId = new("coc", "minotaur_lord_minotaur_drank_malk");
        private static readonly Identifier DisarmId = new("coc", "minotaur_lord_minotaur_disarm");
        private static readonly Identifier EntangleId = new("coc", "minotaur_lord_entangle");
        private static readonly Identifier PrecumTeaseId = new("coc", "minotaur_lord_minotaur_precum_tease");
        private static readonly Identifier CumPressId = new("coc", "minotaur_lord_cum_press");

        private bool UrtaQuest => Math.Abs(CoC.Instance.Flags.Get(VanillaFlags.UrtaQuestStatus) - 0.75f) < 0.0001f;

        // 原版 long 在 URTA_QUEST_STATUS==0.75 时多一句「pet 项圈」（MinotaurLord.hx:186-190）。
        public override string Description => (UrtaQuest
            ? "Npcs.MinotaurLord.Desc.UrtaQuest"
            : "Npcs.MinotaurLord.Desc").Localized();

        // 原版 performCombatAction（MinotaurLord.hx:9-21）：玩家被缠住时必定 minotaurCumPress。
        public override Identifier? ForcedAbility(CombatContext context) =>
            context.Player.HasStatusEffect(VanillaStatusEffects.MinotaurEntangled) ? CumPressId : null;

        // 原版 Monster.hx:1939-1943：持有 Timer 时，value1<=0 移除，否则 -1。
        // 原版 minotaurLordEntangle 用它把连续缠抱隔开 2-5 回合（MinotaurLord.hx:92）。
        public override void TickStatuses(CombatContext context)
        {
            var timer = StatusEffectByType(VanillaStatusEffects.Timer);

            if (timer == null)
                return;

            if (timer.Values[0] <= 0)
                RemoveStatusEffect(VanillaStatusEffects.Timer);
            else
                timer.Values[0] -= 1;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;
            var choices = new List<(Identifier Id, float Weight)>();

            if (Health < 300 && StatusEffectValue(VanillaStatusEffects.MinoMilk, 0) < 4 && UrtaQuest)
                choices.Add((DrankMalkId, 1f));

            if (player.CanDisarm())
                choices.Add((DisarmId, 0.25f));

            if (!HasStatusEffect(VanillaStatusEffects.Timer) &&
                !player.HasStatusEffect(VanillaStatusEffects.MinotaurEntangled))
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

        // 原版 struggle（MinotaurLord.hx:23-38）：被缠住时 str/9 + rand(20) + 1 >= 15 可挣脱。
        public override void Struggle(CombatContext context) =>
            MinotaurLordCombatHandlers.Struggle(context);

        // 原版 react(PlayerWaited)（MinotaurLord.hx:40-53）：被缠住时等待 = 放弃挣扎，+30~34 欲望。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            if (!context.Player.HasStatusEffect(VanillaStatusEffects.MinotaurEntangled))
                return true;

            context.Clear();
            context.Append("Combat.MinotaurLord.React.YouSighAnd".Localized());

            context.Player.TakeLustDamage(30 + context.Random.Next(5));

            return false;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createCock(rand(13+24), 2 + rand(3), HORSE)
            CreateCock(Random.Shared.Next(13 + 24), 2 + Random.Shared.Next(3), VanillaBodyPartTypes.Horse);

            // 原版 balls = 2; ballSize = 2 + rand(13); cumMultiplier = 1.5; hoursSinceCum = ballSize*10
            Balls = 2;
            BallSize = 2 + Random.Shared.Next(13);
            CumMultiplier = 1.5f;
            HoursSinceCum = BallSize * 10;

            // 原版 createBreastRow(0)
            CreateBreastRow(0);

            // 原版 ass.analLooseness = LOOSENESS_STRETCHED(4); ass.analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;
            anal.Wetness = 1;

            // 原版 createStatusEffect(BonusACapacity, 50)
            if (!HasStatusEffect(VanillaStatusEffects.BonusACapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 50);

            // 原版 tallness = 132
            Tallness = 132;

            // 原版 hips.rating = RATING_AVERAGE(4); butt.rating = RATING_AVERAGE(4) + 1
            Body.SetHipsRating("average");
            Body.SetButtRating("average", 1);

            // 原版 lowerBody.type = LowerBody.HOOFED
            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Hoofed;

            // 原版 skin.tone = "red"; skin.type = Skin.FUR; skin.desc = "shaggy fur"
            var skin = Body.Get<SkinPart>();

            skin.Tone = "red";
            skin.Id = VanillaBodyPartTypes.Fur;
            skin.DescKey = "shaggy fur";

            // 原版 hair.color = randomChoice(["black","brown"]); hair.length = 3
            var hair = Body.Get<HairPart>();

            hair.Color = Random.Shared.Next(2) == 0 ? "black" : "brown";
            hair.Length = 3;

            // 原版 face.type = Face.COW_MINOTAUR
            Body.Get<FacePart>().Id = VanillaBodyPartTypes.CowMinotaur;

            // 原版 weaponName = "chain"; weaponVerb = "chain-whip"; weaponAttack = 50
            WeaponName = "chain";
            WeaponVerb = "chain-whip";
            WeaponAttack = 50;

            // 原版 normalAttackAvoid.doParry = false; doCounter = false
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            // 原版 armorName = "thick fur"
            ArmorName = "thick fur";

            // 原版 lustVuln = 0.33
            LustVulnerability = 0.33f;

            // 原版 additionalXP = 50
            AdditionalExperience = 50;

            // 原版 gems = Utils.rand(15) + 25：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(15) + 25;

            // 原版 tail.type = Tail.COW
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Cow;

            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop：非 Urta 任务线是 ChainedDrop(MINOCUM 1/5 → MINOBLO 1/2 → 空)，任务线是 NO_DROP。
            // 数据表按非任务线写。
        }

        // 原版 defeated()（MinotaurLord.hx:156-167）
        public override bool Defeated(bool hpVictory)
        {
            Scene.ClearOutput();
            Scene.OutputText("Combat.MinotaurLord.Defeated.TheMinotaurLord".Localized());

            if (UrtaQuest)
            {
                Scene.OutputText("Combat.MinotaurLord.Defeated.YouCouldUse".Localized());
                Scene.ClearButtons();
                Scene.AddButton(0, "Fuck", () => SceneInstance<UrtaQuestScene>.Get().WinRapeAMinoLordAsUrta());
                Scene.AddButton(14, "Leave", () => SceneInstance<UrtaQuestScene>.Get().BeatMinoLordOnToSuccubi());
            }
            else
            {
                SceneInstance<MinotaurScene>.Get().MinoVictoryRapeChoices();
            }

            return true;
        }

        // 原版 won()（MinotaurLord.hx:169-179）
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
            {
                SceneInstance<MinotaurScene>.Get().GetRapedByMinotaur();
            }

            return true;
        }
    }
}
