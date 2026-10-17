using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 MilkySuccubus extends AbstractSuccubus（src/classes/scenes/quests/urtaQuest/MilkySuccubus.hx）
    // AbstractSuccubus.new() 只调用 Monster.new()，没有额外身体，所以 C# 直接继承 Enemy。
    // 只移植类本体；defeated/won 依赖 Urta 任务线（未移植，原版 MilkySuccubus.hx:62-68）。
    [Register("coc:milky_succubus")]
    public class MilkySuccubusEnemy : Enemy
    {
        // 原版 cumAvailable（MilkySuccubus.hx:18）：两瓶牛头人精液，喝完就不能再回血。
        private static readonly Identifier MilkSprayId = new("coc", "milky_succubus_cow_cubi_milk_spray_attack");
        private static readonly Identifier DrinkCumId = new("coc", "milky_succubus_drink_mino_cum");
        private static readonly Identifier AttackId = new("coc", "milky_succubus_attack");
        private static readonly Identifier TeaseId = new("coc", "milky_succubus_succubus_tease");

        private int _cumAvailable = 2;

        public bool TryDrinkCum()
        {
            if (_cumAvailable <= 0)
                return false;

            _cumAvailable -= 1;

            return true;
        }

        // 原版 teased(lustDelta)（MilkySuccubus.hx:70-75）：嘴上说不在意，实际 str +5、回 50 血。
        // 欲望部分由 CombatResolver 的 Teased(damage) 统一结算，这里只补台词与自身强化。
        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var article = Article;

            if (article.Length > 0)
                article = char.ToUpperInvariant(article[0]) + article[1..];

            context.Append("Combat.MilkySuccubus.Teased.01Smiles".Localized(article, ShortName));

            Stats.AddBase(VanillaStats.Strength, 5);

            HealthChange(50);
        }

        // 原版 performCombatAction（MilkySuccubus.hx:8-16）：
        //   milkSpray 0.3（!MilkyUrta，cost 10 magical）
        //   drinkMinoCum 2（HP < 400）
        //   eAttack 1（player.HP < 100）
        //   succubusTease 1（player.lust100 >= 90）
        //   eAttack 1（无条件）
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;
            var choices = new List<(Identifier Id, float Weight)>();

            if (!HasStatusEffect(VanillaStatusEffects.MilkyUrta))
                choices.Add((MilkSprayId, 0.3f));

            if (Health < 400)
                choices.Add((DrinkCumId, 2f));

            if (player.Health < 100)
                choices.Add((AttackId, 1f));

            if (player.Lust100 >= 90)
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

        // 原版 Monster.hx:1814-1815：怪物身上有 MilkyUrta 时每回合调用 urtaQuest.milkyUrtaTic()。
        // 该函数（UrtaQuest.hx:2257-2271）输出按玩家 lust 分段的台词、dynStats(Lust(10))，并递减状态值/到 0 移除。
        // 原版 vanilla bug：状态挂在 monster（MilkySuccubus.hx:33），递减却在 player 侧（UrtaQuest.hx:2267），
        // 照抄会让 monster 状态永不解除、AI 里 milk spray 永远只能用一次。此处按意图修正：递减 monster 自身，
        // 同时保留 +10 lust 与台词（UrtaQuest.hx:2260-2270）。
        public override void TickStatuses(CombatContext context)
        {
            var milky = StatusEffectByType(VanillaStatusEffects.MilkyUrta);

            if (milky == null)
                return;

            var player = context.Player;

            context.Append("Scenes.Urtaquest.MilkyUrtaTic.MilkHeatRising".Localized());
            context.Append((player.Lust < 50
                ? "Scenes.Urtaquest.MilkyUrtaTic.MildDistraction"
                : player.Lust < 75
                    ? "Scenes.Urtaquest.MilkyUrtaTic.WaningResistance"
                    : "Scenes.Urtaquest.MilkyUrtaTic.TemptingOrbs").Localized());
            context.Append("Scenes.Urtaquest.MilkyUrtaTic.ParagraphBreak".Localized());

            milky.Values[0] -= 1;

            if (milky.Values[0] <= 0)
                RemoveStatusEffect(VanillaStatusEffects.MilkyUrta);

            // 原版 dynStats(Lust(10))：直接加欲望（不走 lustVuln）
            player.Lust = Math.Min(player.MaxLust, player.Lust + 10);
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createVagina(false, WETNESS_SLAVERING=5, LOOSENESS_NORMAL=1)
            CreateVagina(false, 5, 1);

            // 原版 createStatusEffect(BonusVCapacity, 300)
            if (!HasStatusEffect(VanillaStatusEffects.BonusVCapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 300);

            // 原版 createBreastRow(Appearance.breastCupInverse("G")) → VanillaBreastCups.G = 15
            CreateBreastRow(VanillaBreastCups.G);

            // 原版 ass.analLooseness = LOOSENESS_STRETCHED(4); ass.analWetness = WETNESS_SLIME_DROOLING(5)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;
            anal.Wetness = 5;

            // 原版 tallness = 84
            Tallness = 84;

            // 原版 hips.rating = RATING_CURVY(10); butt.rating = RATING_LARGE(8) + 1
            Body.SetHipsRating("curvy");
            Body.SetButtRating("large", 1);

            // 原版 lowerBody.type = LowerBody.DEMONIC_HIGH_HEELS
            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.DemonicHighHeels;

            // 原版 skin.tone = "blue"; hair.color = "black"; hair.length = 13
            Body.Get<SkinPart>().Tone = "blue";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";
            hair.Length = 13;

            // 原版 weaponName = "whip"; weaponVerb = "whipping"; weaponAttack = 10
            WeaponName = "whip";
            WeaponVerb = "whipping";
            WeaponAttack = 10;

            // 原版 normalAttackAvoid.doParry = false; doCounter = false
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            // 原版 armorName = "demonic skin"; armorDef = 10
            ArmorName = "demonic skin";
            ArmorDefense = 10;

            // 原版 lustVuln = .3
            LustVulnerability = 0.3f;

            // 原版 gems = Utils.rand(25) + 10：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(25) + 10;

            // 原版 additionalXP = 50
            AdditionalExperience = 50;

            // 原版 horns.type = Horns.DRACONIC_X2; horns.value = 2
            var horns = Body.Get<HornsPart>();

            horns.Id = VanillaBodyPartTypes.DraconicX2;
            horns.Value = 2;

            // 原版 wings.type = Wings.BAT_LIKE_TINY; tail.type = Tail.DEMONIC
            Body.Get<WingsPart>().Id = VanillaBodyPartTypes.BatLikeTiny;
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Demonic;

            // 原版 temperment = TEMPERMENT_LOVE_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = NO_DROP（MilkySuccubus.hx:115/123），所以数据表 loot 留空。
        }

        // 原版 defeated()（MilkySuccubus.hx:62-64）
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaBeatsUpCowcubi();

            return true;
        }

        // 原版 won()（MilkySuccubus.hx:66-68）
        public override bool Won(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaLosesToCowCubi();

            return true;
        }
    }
}
