using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Linq;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 BoneGeneral extends Monster（scenes/dungeons/manor/BoneGeneral.hx:7）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / AI 权重 / 护卫 / 眩晕钩子按 Haxe 原文人工补齐。
    // 招式效果见 Contents/Combat/VanillaHandlers/BoneGeneralCombatHandlers.cs。
    [Register("coc:bone_general")]
    public class BoneGeneralEnemy : Enemy
    {
        // 原版 GuardAB 的倒计时（Monster.hx:1825-1837）：每个怪物在自己的 combatRoundUpdate 里
        // 把自己的 GuardAB value1 -1，归零时移除并打印两句之一。
        // C# 没有通用的敌人状态 tick，所以由「被护卫的敌人」各自的 CombatRoundUpdate 调这里实现。
        public static void TickGuardAb(CombatContext context, Enemy self)
        {
            var guard = self.StatusEffectByType(VanillaStatusEffects.GuardAB);

            if (guard == null)
                return;

            guard.Values[0] -= 1;

            if (guard.Values[0] <= 0)
            {
                context.Write(MonoCoC.Utils.Utils.CapitalizeFirstLetter(self.Article) + self.ShortName + " is no longer guarded!");

                self.RemoveStatusEffect(VanillaStatusEffects.GuardAB);

                return;
            }

            context.Write(MonoCoC.Utils.Utils.CapitalizeFirstLetter(self.Article) + self.ShortName
                + " is currently being guarded, and is out of your reach.");
        }

        private static readonly Identifier CrushingBlowId = new("coc", "bone_general_crushing_blow");
        private static readonly Identifier GuardAllyId = new("coc", "bone_general_guard_ally");
        private static readonly Identifier AttackId = new("coc", "bone_general_attack");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（BoneGeneral.hx:120）的赋值顺序照抄 ──
            // a = "the " / short = "Bone Guardian" / long / pronoun1-3 = it/it/its 由数据表表达。

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

            ArmorDefense = 60;
            ArmorPerk = "Heavy";

            // initStrTouSpeInte(90, 100, 10, 42) / initLibSensCor(45, 45, 100)
            // / bonusHP = 2000 / level = 20 / lust = 0 由数据表给。

            WeaponName = "spiked mace";
            WeaponVerb = "crush";
            ArmorName = "rusted plate armor";

            // temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：原版死字段，无实际作用。
            // drop = new WeightedDrop()（空表）：数据表 loot 留空。

            Gems = Random.Shared.Next(5) + 100;
            LustVulnerability = 0;

            // createPerk：BleedImmune / PoisonImmune / Juggernaut / ChargingSwings
            CreatePerk(VanillaPerks.BleedImmune);
            CreatePerk(VanillaPerks.PoisonImmune);
            CreatePerk(VanillaPerks.Juggernaut);
            CreatePerk(VanillaPerks.ChargingSwings);
        }

        // 原版 performCombatAction（BoneGeneral.hx:49）的 MonsterAI：
        //   eAttack 权重 2；crushingBlow 权重 1（玩家未眩晕时，10 物理疲劳）；guardAlly 权重 1（canGuard()）。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var canCrush = !context.Player.HasStatusEffect(VanillaStatusEffects.Stunned);
            var canGuard = CanGuard(context);
            var total = 2 + (canCrush ? 1 : 0) + (canGuard ? 1 : 0);
            var roll = context.Random.Next(total);

            if (roll < 2)
                return new CombatAiAction { Ability = AttackId };

            roll -= 2;

            if (canCrush && roll == 0)
                return new CombatAiAction { Ability = CrushingBlowId, Cost = 10, Fatigue = FatigueType.Physical };

            return new CombatAiAction { Ability = GuardAllyId };
        }

        // 原版 canGuard（BoneGeneral.hx:30）：场上有多名敌人、没人已被护卫、且存在活着的非 BoneGeneral 同伙。
        public bool CanGuard(CombatContext context)
        {
            if (context.Enemies.Count <= 1)
                return false;

            if (context.Enemies.Any(e => e.HasStatusEffect(VanillaStatusEffects.GuardAB)))
                return false;

            return context.Enemies.Any(e => e.Health > 0 && e is not BoneGeneralEnemy);
        }

        // 原版 handleStun（BoneGeneral.hx:14）：
        //   先输出「被打晕」文案，再把场上所有怪物的 GuardAB 清掉，最后递减/移除 Stunned，返回 false（本回合不动）。
        // 「文案 + 递减」交给基类，这里补清 GuardAB 那一步。
        public override bool HandleStun(CombatContext context)
        {
            var result = base.HandleStun(context);

            foreach (var enemy in context.Enemies)
                enemy.RemoveStatusEffect(VanillaStatusEffects.GuardAB);

            return result;
        }

        // 原版 handleFear（BoneGeneral.hx:8）：骷髅不吃恐惧，直接清掉并照常行动。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.BoneGeneral.HandleFear.TheSkeletonSeems".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 replacesDescribeAttacked（BoneGeneral.hx:95；调用点 Weapon.hx:615 的 describeAttack）：
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
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.Your0Bounces".Localized(attackNoun, theMonster));
            else if (damageLow)
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.YouStrikeA".Localized(theMonster));
            else if (damageMed)
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.You01".Localized(attackVerb, theMonster, Pronoun2));
            else if (damageHigh)
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.0CracksUnder".Localized(theMonsterCap, attackNoun));
            else
                context.WriteAction("Combat.BoneGeneral.ReplacesDescribeAttacked.You0With".Localized(theMonster, attackNoun));

            // 原版随后 outputText(getDamageText(damage))（Combat.hx:1375）。
            // C# 未移植主题系统，按 MainViewManager.hx:35-37 固定用暗色主题的 colorHpMinus = #ff0000。
            // 未接：crit 标记——原版 Weapon.hx:615 会把 crit 传进来，C# 的 Enemy.DescribeAttacked 只有
            // (context, damage) 两个参数，拿不到暴击标记（报告已单列）。
            context.Append("Combat.BoneGeneral.DescribeAttacked.DamageText".Localized("#ff0000", (int)MathF.Round(damage)));

            return true;
        }

        // 原版 BoneGeneral.hx:87-93：
        //   won      → game.dungeons.manor.loseToGeneral()（Manor.hx:356）
        //   defeated → game.dungeons.manor.defeatGeneral()（Manor.hx:365）
        public override bool Defeated(bool hpVictory)
        {
            // 三骷髅群战（Manor.hx:630）用逐实例 OnDefeated/OnWon 覆盖成 defeatCourt/loseToCourt，
            // 这时让位给 CombatController 的逐实例回调（Finish 里的 OnDefeated/OnWon 分支）。
            if (OnDefeated != null || OnWon != null)
                return false;

            SceneInstance<ManorScene>.Get().DefeatGeneral();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (OnDefeated != null || OnWon != null)
                return false;

            SceneInstance<ManorScene>.Get().LoseToGeneral();

            return true;
        }
    }
}
