using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 BoneJester extends Monster（scenes/dungeons/manor/BoneJester.hx:7）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / 隐匿状态 / 恐惧免疫按 Haxe 原文人工补齐。
    // 招式效果见 Contents/Combat/VanillaHandlers/BoneJesterCombatHandlers.cs。
    [Register("coc:bone_jester")]
    public class BoneJesterEnemy : Enemy
    {
        // 原版 BoneJester.hx:8 `public var hidden:Bool = false`
        public bool Hidden;

        // 原版 BoneJester.hx:9 `public var prevHP:Float = 0`
        public float PrevHp;

        private static readonly Identifier HideInShadowsId = new("coc", "bone_jester_hide_in_shadows");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（BoneJester.hx:105）的赋值顺序照抄 ──
            // a = "the " / short = "Bone Jester" / long / pronoun1-3 = it/it/its 由数据表表达。

            // createBreastRow(Appearance.breastCupInverse("E")) = 7
            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / ass.analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            Tallness = 80;

            // hips.rating = Hips.RATING_AMPLE(6) + 2 / butt.rating = Butt.RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();
            hair.Color = "purple";
            hair.Length = 4;

            WeaponAttack = 60;

            // initStrTouSpeInte(60, 40, 100, 42) / initLibSensCor(45, 45, 100) 由数据表给。
            // bonusHP = 1000 由数据表给。

            ArmorDefense = 0;

            WeaponName = "jagged daggers";
            WeaponVerb = "slash";
            ArmorName = "Jester's Garb";

            // lust = 0 / level = 20 由数据表给；这里补 lustVuln = 0。
            LustVulnerability = 0;

            // temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：原版死字段，无实际作用。
            // drop = new WeightedDrop()（空表）：数据表 loot 留空。

            Gems = Random.Shared.Next(5) + 100;

            // createPerk(PerkLib.BleedImmune) / createPerk(PerkLib.PoisonImmune)
            CreatePerk(VanillaPerks.BleedImmune);
            CreatePerk(VanillaPerks.PoisonImmune);
        }

        // 原版 performCombatAction（BoneJester.hx:17）：
        //   先按上一回合的血量变化决定是否现形；隐匿中必定出手背刺；
        //   否则 3 选 1（eAttack 权重 2 / hideInShadows 1 / harvest 1）。
        // 权重部分交给数据 AI（Assets/Data/Combat/Ai/BoneJester.hjson），这里只处理隐匿状态。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (PrevHp > Health)
                Hidden = false;

            PrevHp = Health;

            if (Hidden)
                return new CombatAiAction { Ability = HideInShadowsId };

            return interpreter.Choose();
        }

        // 原版 Monster.hx:1825：被 GuardAB 保护时每回合递减、归零解除（BoneGeneral.guardAlly 会挂）。
        // 迁到 TickStatuses（Enemy.cs:322，派发点 CombatController.cs:787/813），
        // 与 CombatRoundUpdate 只留一份，避免双倍递减。
        public override void TickStatuses(CombatContext context) =>
            BoneGeneralEnemy.TickGuardAb(context, this);

        // 原版 handleFear（BoneJester.hx:11）：骷髅不吃恐惧，直接清掉并照常行动。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.BoneJester.HandleFear.TheSkeletonSeems".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 replacesDescribeAttacked（BoneJester.hx:80；调用点 Weapon.hx:615 的 describeAttack）：
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

            var maxHealth = MaxHealth;
            var damageLow = damage < 15 || damage < maxHealth * 0.05f;
            var damageMed = damage < 50 || damage < maxHealth * 0.20f;
            var damageHigh = damage < 100 || damage < maxHealth * 0.33f;

            if (damage <= 0)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.0CracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.BoneJester.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            // 原版随后 outputText(getDamageText(damage))（Combat.hx:1375）。
            // C# 未移植主题系统，按 MainViewManager.hx:35-37 固定用暗色主题的 colorHpMinus = #ff0000。
            // 未接：crit 标记——原版 Weapon.hx:615 会把 crit 传进来，C# 的 Enemy.DescribeAttacked 只有
            // (context, damage) 两个参数，拿不到暴击标记（报告已单列）。
            context.Append("Combat.BoneJester.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        // 原版 BoneJester.hx:72-77：
        //   won      → game.dungeons.manor.loseToJester()（Manor.hx:427）
        //   defeated → game.dungeons.manor.defeatJester()（Manor.hx:441）
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().DefeatJester();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToJester();

            return true;
        }
    }
}
