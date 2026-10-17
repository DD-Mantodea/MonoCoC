using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 JellyFishGirl extends Monster（src/classes/scenes/places/swim/JellyFishGirl.hx）
    // defeated/won 接 JellyFishGirlScene（原版 JellyFishGirl.hx:13-20）。
    [Register("coc:jelly_fish_girl")]
    public class JellyFishGirlEnemy : Enemy
    {
        // 原版 new(element, elementLong) 由场景按颜色抽签传入（原版 JellyFishGirlScene.hx:62-87）；
        // 场景通过 Create/SetElement 注入，未注入时（例如直接走 EnemyRegistry.Create）按原版权重自己抽。
        private string _element = "blue";

        private string _elementLong = "blue";

        private bool _elementInjected;

        // 原版 new JellyFishGirl(element, elementLong)：注入颜色后再按注册表配置，等价 EnemyRegistry.Create。
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

        private static readonly Identifier AttackId = new("coc", "jelly_fish_girl_attack");
        private static readonly Identifier GrappleId = new("coc", "jelly_fish_girl_jelly_grapple");
        private static readonly Identifier MolestId = new("coc", "jelly_fish_girl_molest_player");

        // 原版 long 里拼了 elementLong（JellyFishGirl.hx:120），依赖实例，所以现算。
        public override string Description => "Npcs.JellyFishGirl.Desc".Localized(_elementLong);

        // 原版 performCombatAction（JellyFishGirl.hx:74-84）：
        //   eAttack 3；玩家被缠住时 molestPlayer 5，否则 jellyGrapple 1。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var grappled = context.Player.HasStatusEffect(VanillaStatusEffects.Grappled);

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

        // 原版 react(TurnStart)（JellyFishGirl.hx:107-109）：每回合回 lust/5 的血。
        // C# 的 TurnStart 没有派发点，改在 CombatRoundUpdate（敌人回合开始）里做。
        public override void CombatRoundUpdate(CombatContext context)
        {
            if (Lust > 0)
                HealthChange(Lust / 5f);
        }

        // 原版 react(Distanced)（JellyFishGirl.hx:96-98）：被拉开距离时再缠一次；
        // 返回 !grappled（没缠上=没消费这次时机）。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.Distanced)
                return true;

            JellyFishGirlCombatHandlers.JellyGrapple(context);

            return !context.Player.HasStatusEffect(VanillaStatusEffects.Grappled);
        }

        // 原版 struggle（JellyFishGirl.hx:23-37）：1/3 或 str×1.25% 成功脱身并反伤。
        public override void Struggle(CombatContext context) =>
            JellyFishGirlCombatHandlers.Struggle(context);

        // 原版 outputAttack（JellyFishGirl.hx:86-92）：整段替换成专属战报。
        public override void OutputAttack(CombatContext context, float damage)
        {
            context.Append("Combat.JellyFishGirl.OutputAttack.TheJellyGirl".Localized());

            if (CoC.Instance.Flags.Get(MonoCoC.Contents.Flags.VanillaFlags.EnemyCritical) > 0)
                context.Append("Combat.JellyFishGirl.OutputAttack.Text".Localized());

            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 场景已注入 element/elementLong（原版 JellyFishGirlScene.hx:86）时不再随机；
            // 未注入（直接 EnemyRegistry.Create）时按原版权重：blue 2 / purple 2 / crystal 1。
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
                    "crystal" => new[] { "glassy", "crystal", "prismatic" },
                    _ => new[] { "blue", "azure", "cerulean", "sapphire" }
                };

                _elementLong = longChoices[Random.Shared.Next(longChoices.Length)];
            }

            // 原版 createVagina(false, WETNESS_NORMAL=1, LOOSENESS_TIGHT=1)
            CreateVagina(false, 1, 1);

            // 原版 ass.analLooseness = LOOSENESS_VIRGIN(0); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;
            anal.Wetness = 0;

            // 原版 tallness = 36
            Tallness = 36;

            // 原版 allowBaby/allowChild 为真时 flat，否则 A；C# 未移植这两个选项，固定 A。
            CreateBreastRow(VanillaBreastCups.A);

            // 原版 hips.rating = RATING_BOYISH(0); butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            // 原版 skin.tone = elementLong; hair.color = element; hair.length = 16
            Body.Get<SkinPart>().Tone = _elementLong;

            var hair = Body.Get<HairPart>();

            hair.Color = _element;
            hair.Length = 16;

            // 原版 weaponName = "tentacles"; weaponVerb = "tentacles"; weaponAttack = 7
            WeaponName = "tentacles";
            WeaponVerb = "tentacles";
            WeaponAttack = 7;

            // 原版 armorName = "tough skin"; armorDef = 5
            ArmorName = "tough skin";
            ArmorDefense = 5;

            // 原版 lustVuln = .9
            LustVulnerability = 0.9f;

            // 原版 fireRes = 0.3（抗火）
            FireResistance = 0.3f;

            // 原版 gems = Utils.rand(15) + 5：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(15) + 5;

            // 原版 createPerk(PoisonImmune)
            if (!HasPerk(VanillaPerks.PoisonImmune))
                CreatePerk(VanillaPerks.PoisonImmune);

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            //
            // 原版 defeated()：JellyFishGirl.hx:13-15（swim.jellyFishGirlScene.jellyWinChoices）
            // 原版 won()：JellyFishGirl.hx:18-20（swim.jellyFishGirlScene.jellyLossChoices）
        }

        // 原版 defeated()：JellyFishGirl.hx:13-15
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.JellyFishGirlScene>.Get().JellyWinChoices(hpVictory);

            return true;
        }

        // 原版 won()：JellyFishGirl.hx:18-20
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.Swim.JellyFishGirlScene>.Get().JellyLossChoices(hpVictory);

            return true;
        }
    }
}
