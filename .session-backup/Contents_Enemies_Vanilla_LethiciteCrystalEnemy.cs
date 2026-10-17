using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 LethiciteCrystal extends Monster（scenes/dungeons/manor/LethiciteCrystal.hx:6）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / 免疫 / 挑逗反应 / pulse 按 Haxe 原文人工补齐。
    // 招式效果见 Contents/Combat/VanillaHandlers/LethiciteCrystalCombatHandlers.cs。
    [Register("coc:lethicite_crystal")]
    public class LethiciteCrystalEnemy : Enemy
    {
        private static readonly Identifier PulseId = new("coc", "lethicite_crystal_pulse");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（LethiciteCrystal.hx:97）的赋值顺序照抄 ──
            // a = "a " / short = "Lethicite Crystal" / long / pronoun1-3 = it/it/its 由数据表表达。

            // createBreastRow(Appearance.breastCupInverse("flat")) = 0
            CreateBreastRow(VanillaBreastCups.Flat);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / ass.analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            Tallness = 65;

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();
            hair.Color = "purple";
            hair.Length = 4;

            // initStrTouSpeInte(12, 0, 35, 42) / initLibSensCor(45, 45, 100) / level = 20 / lust = 0 由数据表给。
            // bonusHP = 100000 由数据表给（base.Configure 会加成到 MaxHealth）。

            WeaponName = "fists";
            WeaponVerb = "tiny punch";
            ArmorName = "leather straps";
            ArmorDefense = 100;

            // temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：原版死字段，无实际作用。
            // drop = new WeightedDrop()（空表）：数据表 loot 留空。

            Gems = Random.Shared.Next(5) + 5;
            LustVulnerability = 0.7f;

            // 原版 bonusLust = 200（LethiciteCrystal.hx:129）由 Monster.maxLust()（Monster.hx:466
            // `100 + bonusLust`）生效 → 欲望上限 300。
            // C# 对应 Creature.MaxLust（Creature.cs:198，默认 provider = 100，Creature.cs:31），
            // 用属性修饰层 +200，写法和 SkeletonHorde 的 MaxHealth 动态修饰一致。
            Stats.AddModifier(VanillaStats.MaxLust, ModifierOperation.Addition, () => 200f,
                new TemporarySource(Guid.NewGuid()));

            // createPerk(PerkLib.Immovable)
            CreatePerk(VanillaPerks.Immovable);
        }

        // 原版 performCombatAction（LethiciteCrystal.hx:22）只有一招 pulse()。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>
            new() { Ability = PulseId };

        // 原版 handleFear（LethiciteCrystal.hx:7）：不受恐惧影响，直接清掉并照常行动。
        public override bool HandleFear(CombatContext context)
        {
            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 handleStun（LethiciteCrystal.hx:12）：不受眩晕影响，直接清掉并照常行动。
        public override bool HandleStun(CombatContext context)
        {
            RemoveStatusEffect(VanillaStatusEffects.Stunned);

            return true;
        }

        // 原版 handleConstricted（LethiciteCrystal.hx:17）：水晶被缠住时直接清掉 Constricted 并照常行动。
        // C# 对应 Enemy.HandleConstricted（Enemy.cs:191）；派发条件是「敌人自己有 Constricted」
        // （Monster.hx:1135-1138 → CombatController.cs:791-792 / 819-820）。
        public override void HandleConstricted(CombatContext context)
        {
            RemoveStatusEffect(VanillaStatusEffects.Constricted);
        }

        // 原版 outputDefaultTeaseReaction（LethiciteCrystal.hx:64）：
        // lustDelta == 0 时「没反应」，否则「嗡嗡共振」。
        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var capitalA = MonoCoC.Utils.Utils.CapitalizeFirstLetter(Article);

            context.WriteAction((lustDelta == 0
                ? "Combat.LethiciteCrystal.OutputDefaultTeaseReaction.01Doesn"
                : "Combat.LethiciteCrystal.OutputDefaultTeaseReaction.01Hums").Localized(capitalA, ShortName));
        }

        // 原版 replacesDescribeAttacked（LethiciteCrystal.hx:72；调用点 Weapon.hx:615 的 describeAttack）：
        // 玩家武器命中时整段接管战报。C# 对应 Enemy.DescribeAttacked（Enemy.cs:468，派发点 CombatResolver.cs:63）。
        public override bool DescribeAttacked(CombatContext context, float damage)
        {
            var weapon = context.Player.Weapon;

            // 原版 if (!weapon.isChanneling())：引导法杖走通用战报。
            if (weapon.IsChanneling(context.Player))
                return false;

            // 原版 weapon.attackNoun / attackVerb；C# 的 Weapon 没有 attackNoun 字段，用装备名近似（报告已单列）。
            var attackNoun = weapon.Name;
            var attackVerb = weapon.Verb;
            var theMonster = Article + ShortName;
            var theMonsterCap = MonoCoC.Utils.Utils.CapitalizeFirstLetter(Article) + ShortName;

            // 水晶的分档阈值是 2% / 5% / 15%（LethiciteCrystal.hx:74-76），比骷髅们的 5%/20%/33% 更低。
            var maxHealth = MaxHealth;
            var damageLow = damage < 15 || damage < maxHealth * 0.02f;
            var damageMed = damage < 50 || damage < maxHealth * 0.05f;
            var damageHigh = damage < 100 || damage < maxHealth * 0.15f;

            if (damage <= 0)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.0CracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.LethiciteCrystal.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            // 原版随后 outputText(getDamageText(damage))（Combat.hx:1375）。
            // C# 未移植主题系统，按 MainViewManager.hx:35-37 固定用暗色主题的 colorHpMinus = #ff0000。
            // 未接：crit 标记——原版 Weapon.hx:615 会把 crit 传进来，C# 的 Enemy.DescribeAttacked 只有
            // (context, damage) 两个参数，拿不到暴击标记（报告已单列）。
            context.Append("Combat.LethiciteCrystal.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        // 原版 LethiciteCrystal.hx:56-61：
        //   won      → game.dungeons.manor.loseToCrystal()（Manor.hx:687）
        //   defeated → game.dungeons.manor.destroyCrystal()（Manor.hx:713）
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DestroyCrystal();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToCrystal();

            return true;
        }
    }
}
