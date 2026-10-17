using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Shouldra extends Monster（src/classes/scenes/npcs/Shouldra.hx）
    // 招式见 Contents/Combat/VanillaHandlers/ShouldraCombatHandlers.cs。
    //
    // won/defeated 不覆写：原版 Shouldra.hx:82 的 defeated 调用 game.shouldraScene.defeatDannyPhantom()、
    // 原版 Shouldra.hx:86 的 won 调用 game.shouldraScene.loseToShouldra()。
    // C# 侧缺 ShouldraScene（全量搜索 `class \w*Shouldra` 只命中 ShouldraFollower；
    // `defeatDannyPhantom` / `loseToShouldra` 均 0 命中），胜负分支无处可接，等场景落地后回填。
    [Register("coc:shouldra")]
    public class ShouldraEnemy : Enemy
    {
        // 原版 Shouldra.hx:14 ghostLoli() 返回 game.shouldraScene.ghostLoli()；
        // ShouldraScene.hx:97 的实现是 npcSettings.shouldraChild && allowChild。
        // C# 对应 CoC.ShouldraChild / CoC.AllowChild：
        //   shouldraChild 数据项见 Assets/Data/Options/04-npc.hjson（默认 0），
        //   allowChild 是 underage >= Half（CoC.hx:832-835，数据项见 02-fetish.hjson）。
        public bool GhostLoli => CoC.Instance?.AllowChild == true && CoC.Instance.ShouldraChild;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            CreateVagina(false, 2, 2);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 40);

            // createBreastRow(Appearance.breastCupInverse(ghostLoli() ? "A" : "C"))
            CreateBreastRow(GhostLoli ? VanillaBreastCups.A : VanillaBreastCups.C);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 40);

            // tallness = ghostLoli() ? 57 : 65
            Tallness = GhostLoli ? 57 : 65;

            // hips.rating = ghostLoli() ? RATING_BOYISH(0) : RATING_AMPLE(6)
            Body.Hips = GhostLoli ? 0 : 6;

            // butt.rating = ghostLoli() ? RATING_TIGHT(2) : RATING_AVERAGE(4) + 1
            Body.Butt = GhostLoli ? 2 : 5;

            Body.Get<SkinPart>().Tone = "white";

            var hair = Body.Get<HairPart>();
            hair.Color = "white";
            hair.Length = 3;

            // initStrTouSpeInte(45, 30, 5, 110)
            Strength = 45;
            Toughness = 30;
            Speed = 5;
            Intelligence = 110;

            // initLibSensCor(100, 0, 33)
            Libido = 100;
            Sensitivity = 0;
            Corruption = 33;

            WeaponName = "fists";
            WeaponVerb = "punches";

            ArmorName = "comfortable clothes";

            // bonusHP = 30：走数据表的 bonusHealth（base.Configure 已经加进最大生命），这里不重复叠加

            Lust = 10;

            // 原版 Shouldra.hx:117 temperment = Monster.TEMPERMENT_LUSTY_GRAPPLES：死字段，
            // AI 权重由 Assets/Data/Combat/Ai/Shouldra.hjson 表达。

            Level = 4;
            Gems = 0;

            // 原版 Shouldra.hx:120 drop = new ChainedDrop().add(consumables.ECTOPLS, 1 / 3)
            // → Assets/Data/Enemies/Shouldra.hjson 的 loot（coc:ectoplasm，33.33%）

            // 原版 Shouldra.hx:121 createPerk(PerkLib.BlindImmune)
            CreatePerk(VanillaPerks.BlindImmune);
        }

        // 原版 Shouldra.hx:18 react(Blinded)：故意让「致盲术」落空——专属台词 + 返回 false，
        // 派发点 PlayerSpellHandlers.cs:39（原版 CombatAbilities.hx:297，在施加 Blind 之前）。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.Blinded)
                return true;

            context.Append("Combat.Shouldra.React.RemarkablyItSeems".Localized());

            return false;
        }
    }
}
