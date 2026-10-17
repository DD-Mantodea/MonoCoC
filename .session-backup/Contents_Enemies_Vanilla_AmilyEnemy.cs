using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Amily extends Monster（src/classes/scenes/npcs/Amily.hx）
    // 招式见 Contents/Combat/VanillaHandlers/AmilyCombatHandlers.cs。
    //
    // won 不覆写：原版 Amily.hx 没有覆写 won（玩家战败走通用战败流程）。
    // defeated 也不覆写：原版 Amily.hx:88 调用 game.amilyScene.conquerThatMouseBitch()，
    // C# 侧缺 AmilyScene（全量搜索 `class \w*Amily` 只命中 AmilyLover / AmilySlave，
    // `conquerThatMouseBitch` 0 命中），胜负分支无处可接，等场景落地后回填。
    [Register("coc:amily")]
    public class AmilyEnemy : Enemy
    {
        // 原版 Amily.hx:77 teased()：处于 Concentration 时额外 +25 并解除专注。
        // C# 的 CombatResolver.PlayerTease 先调 Teased（→ ApplyTease）再调 OnTeased，
        // 所以数值放在 ApplyTease、台词放在 OnTeased，用这个标志把两者串起来。
        private bool _teasedWhileConcentrated;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            CreateVagina(false, 1, 2);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 48);

            CreateBreastRow(VanillaBreastCups.C);

            // ass.analLooseness = Ass.LOOSENESS_VIRGIN(0) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 0;
            anal.Wetness = 0;

            Tallness = 62;

            // hips.rating = Hips.RATING_AMPLE(6) / butt.rating = Butt.RATING_TIGHT(2)
            Body.Hips = 6;
            Body.Butt = 2;

            // skin.tone = game.noFur ? "tan" : "tawny"
            // skin.setType(game.noFur ? Skin.PLAIN : Skin.FUR)
            // （C# 里 game.noFur 的既有等价写法是 !Player.HasFur()，见 JojoEnemy）
            var noFur = !MonoCoC.CoC.Instance.Player.HasFur();
            var skin = Body.Get<SkinPart>();
            skin.Tone = noFur ? "tan" : "tawny";
            skin.Id = noFur ? VanillaBodyPartTypes.Plain : VanillaBodyPartTypes.Fur;

            var hair = Body.Get<HairPart>();
            hair.Color = "brown";
            hair.Length = 5;

            // initStrTouSpeInte(30, 30, 85, 60)
            Strength = 30;
            Toughness = 30;
            Speed = 85;
            Intelligence = 60;

            // initLibSensCor(45, 45, 10)
            Libido = 45;
            Sensitivity = 45;
            Corruption = 10;

            WeaponName = "knife";
            WeaponVerb = "slash";
            WeaponAttack = 6;

            ArmorName = "rags";
            ArmorDefense = 1;

            // bonusHP = 20：走数据表 bonusHealth（base.Configure 已经加进最大生命）

            Lust = 20;
            LustVulnerability = 0.85f;

            // 原版 Amily.hx 没有 temperment 赋值：AI 权重由 Assets/Data/Combat/Ai/Amily.hjson 表达。

            Level = 4;
            Gems = 2 + Random.Shared.Next(5);

            // 原版 Amily.hx:124 drop = NO_DROP → 数据表不配 loot
        }

        // 原版 Amily.hx:77 teased()：专注状态下被挑逗 → lustDelta + 25 + lustDelta，并解除专注。
        // 原版的 lustDelta 已经乘过 lustVuln，所以这里等价写法是「原始值 × 抗性 × 2 + 25」。
        public override void ApplyTease(float lustDelta)
        {
            if (HasStatusEffect(VanillaStatusEffects.Concentration))
            {
                _teasedWhileConcentrated = true;

                Lust = Math.Min(MaxLust, Lust + lustDelta * LustVulnerability * 2 + 25);

                RemoveStatusEffect(VanillaStatusEffects.Concentration);

                return;
            }

            base.ApplyTease(lustDelta);
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            if (_teasedWhileConcentrated)
            {
                _teasedWhileConcentrated = false;

                context.WriteAction("Combat.Amily.Teased.AmilyFlushesHotly".Localized());

                return;
            }

            base.OnTeased(context, lustDelta);
        }

        // 原版 Amily.hx:54 react(BeforeAttacked)：专注状态下闪开近战攻击。
        // C# 的派发点是 CombatResolver.PlayerAttack 开头（原版 Combat.hx:927 的同一时机、
        // 在闪避判定之前），返回 false 整个取消这次攻击。
        // 原版条件还要求 !game.combat.isWieldingRangedWeapon()：远程武器打过来时专注闪避不生效，
        // 这里用 Player.IsWieldingRangedWeapon()（原版 Combat.hx:1144-1150）判定。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction == CombatReactionContext.BeforeAttacked
                && HasStatusEffect(VanillaStatusEffects.Concentration)
                && !context.Player.IsWieldingRangedWeapon()
                && context.Player.StatusEffectValue(VanillaStatusEffects.CounterAB, 0) != 1)
            {
                context.Append("Combat.Amily.BeforeAttacked.AmilyEasilyGlides".Localized());

                return false;
            }

            return true;
        }
    }
}
