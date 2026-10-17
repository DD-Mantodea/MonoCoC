using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Holli extends Monster（src/classes/scenes/npcs/Holli.hx）
    // 招式见 Contents/Combat/VanillaHandlers/HolliCombatHandlers.cs。
    //
    // won/defeated：原版 Holli.hx:232 defeated → game.holliScene.defeatHolli()、
    // 原版 Holli.hx:236 won → game.holliScene.enjoyYourBadEndBIYAAAATCH()。
    // C# 对应 HolliScene.DefeatHolli / EnjoyYourBadEndBIYAAAATCH（Contents/Scenes/Vanilla/Npcs/HolliScene.cs）。
    //
    // 未移植：Jojo 纵火机制（原版 Holli.hx:35 react(Burned) / :114 lightHolliOnFireMagically）
    // 依赖的 CombatReactionContext.Burned 在 C# 没有派发点，场景侧 jojoAssisting 也恒 false，
    // 所以 IsBurning 恒为 false、JojoPyre 恒为 0（见下方 react 注释）。
    [Register("coc:holli")]
    public class HolliEnemy : Enemy
    {
        // 原版 Holli.hx:21 isBurning / :22 jojoPyre
        public bool IsBurning;

        public int JojoPyre;

        // 原版 StatusEffects.HolliConstrict（Holli.hx:60 / :171 / :183 / :189）。
        // 共享层已经加了 VanillaStatusEffects.HolliConstrict（CombatController.IsPlayerBound 也读它），
        // 这里直接别名过去，保持单一来源。
        public static Identifier ConstrictStatus => VanillaStatusEffects.HolliConstrict;

        private bool _teasedWhileBurning;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            CreateCock(12, 2, VanillaBodyPartTypes.Human);

            Balls = 0;
            BallSize = 0;
            // 原版 cumMultiplier = 3 / hoursSinceCum = 20：C# 没有对应字段（同 PlagueRat 的说明）

            CreateVagina(false, 2, 3);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 20);

            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / analWetness = Ass.WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            // tallness = Utils.rand(12) + 55
            Tallness = Random.Shared.Next(12) + 55;

            // hips.rating = Hips.RATING_CURVY(10) / butt.rating = Butt.RATING_LARGE(8)
            Body.Hips = 10;
            Body.Butt = 8;

            Body.Get<SkinPart>().Tone = "black";

            var hair = Body.Get<HairPart>();
            hair.Color = "sandy-blonde";
            hair.Length = 15;

            // initStrTouSpeInte(150, 80, 80, 85)
            Strength = 150;
            Toughness = 80;
            Speed = 80;
            Intelligence = 85;

            // initLibSensCor(75, 40, 80)
            Libido = 75;
            Sensitivity = 40;
            Corruption = 80;

            WeaponName = "branches";
            WeaponVerb = "branchy thwack";

            ArmorName = "bark";
            ArmorDefense = 40;

            // bonusHP = 1000：走数据表 bonusHealth（base.Configure 已经加进最大生命）

            Lust = 20;
            LustVulnerability = 0.2f;

            // 原版 Holli.hx:281 temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：死字段，
            // AI 权重由 Assets/Data/Combat/Ai/Holli.hjson 表达。

            Level = 20;
            Gems = 0;

            // 原版 Holli.hx:284 drop = NO_DROP → 数据表不配 loot
        }

        // 原版 performCombatAction 末尾的 holliBonusHealing()（Holli.hx:229）：
        // C# 没有「招式执行完之后」的钩子，改挂在敌人回合开始（等价于上一回合结束）。
        public override void CombatRoundUpdate(CombatContext context)
        {
            base.CombatRoundUpdate(context);

            HolliBonusHealing(context);
        }

        // 原版 holliBonusHealing()（Holli.hx:65）：非燃烧状态下每回合回 25 血、欲望 > 20 时 -10。
        // jojoAssisting 那一整段（Jojo 搬柴/点火，Holli.hx:65-114）依赖 CombatReactionContext.Burned，
        // 该派发点 C# 没有，见类头注释。
        public void HolliBonusHealing(CombatContext context)
        {
            if (!IsBurning)
            {
                if (HealthRatio() < 1 && Health > 1)
                {
                    context.Append("Combat.Holli.HolliBonusHealing.WhatWoundsYou".Localized());

                    HealthChange(25);
                }

                if (Lust > 20 && LustRatio() <= 0.99f)
                {
                    context.Append("Combat.Holli.HolliBonusHealing.ASingleRent".Localized());

                    Lust -= 10;
                }
            }
        }

        // 原版 react(PlayerWaited)（Holli.hx:28）：被根须缠住时玩家「等待」会直接触发榨取
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited
                || !context.Player.HasStatusEffect(ConstrictStatus))
                return true;

            WaitForConstrict(context, true);

            TookAction = true;

            return false;
        }

        // 原版 react(Burned) + lightHolliOnFireMagically()（Holli.hx:35 / :114）：
        // 两者都依赖 HolliScene 的 Jojo 助战状态；而 C# 的 CombatReactionContext.Burned
        // 目前没有任何 ReactWrapper 派发点（派发点是 PlayerWaited / Distanced / Approached /
        // BeforeAttacked / WhenAttacked / AfterAttacked / Blinded 七处，不含 Burned），
        // 所以这一段仍然接不上。

        // 原版 Holli.hx:47 runFail()：逃跑失败 → 专属台词 + 15 点伤害 + 挂 HolliConstrict。
        // C# 的派发点在 CombatController.Run()（对应原版 Monster.hx:1350）；
        // 原版末尾的 startMonsterTurn 由派发层统一换成 EnemyTurn。
        public override void RunFail(CombatContext context)
        {
            // 原版 runFail 开头就 clearOutput()，这里同样清掉通用战报
            context.Clear();

            context.Append("Combat.Holli.RunFail.You".Localized());

            if (context.Player.CanFly())
                context.Append("Combat.Holli.RunFail.BeatYourWings".Localized());

            context.Append("Combat.Holli.RunFail.TryToEscape".Localized(ShortName));

            // 原版 Holli.hx:55：!player.hasPerk(PerkLib.Juggernaut) && armorPerk != "Heavy"
            if (!context.Player.HasPerk(VanillaPerks.Juggernaut) && ArmorPerk != "Heavy")
                context.Player.TakeDamage(15);

            context.Append("Combat.Holli.RunFail.Text".Localized());

            context.Player.CreateStatusEffect(ConstrictStatus, 0, 0, 0, 0);
        }

        // 原版 struggleOutOfHolli()（Holli.hx:175）：挣扎脱离根须束缚。
        // 共享层 CombatController.IsPlayerBound() 已经把 VanillaStatusEffects.HolliConstrict 算进束缚，
        // 所以玩家被缠住时菜单会出现「挣扎」并走到这里。
        public override void Struggle(CombatContext context)
        {
            var player = context.Player;

            // 原版 player.addStatusValue(StatusEffects.HolliConstrict, 1, 9)
            //（原版随后判断的是 v1，按上游原样照抄）
            player.ChangeStatusValue(ConstrictStatus, 1, 9);

            context.Clear();

            if (player.Body.Get<TonguePart>().Id != VanillaBodyPartTypes.Human)
            {
                context.Append("Combat.Holli.StruggleOutOfHolli.YouCanT".Localized());

                player.RemoveStatusEffect(ConstrictStatus);
            }
            else if (player.Strength / 10f + context.Random.Next(20) + 1
                     + player.StatusEffectValue(ConstrictStatus, 0) > 30)
            {
                context.Append("Combat.Holli.StruggleOutOfHolli.YouManageTo".Localized());

                player.RemoveStatusEffect(ConstrictStatus);
            }
            else
            {
                context.Append("Combat.Holli.StruggleOutOfHolli.YouTryTo".Localized());

                WaitForConstrict(context, false);

                return;
            }

            TookAction = true;
        }

        // 原版 waitForHolliConstrict(newScreen)（Holli.hx:200）
        public void WaitForConstrict(CombatContext context, bool newScreen)
        {
            if (newScreen)
                context.Clear();

            context.Append("Combat.Holli.WaitForHolliConstrict.TheOminousRoseate".Localized());

            Lust -= 20;

            if (Lust < 20)
                Lust = 20;

            context.Player.TakeLustDamage(15 + context.Player.Sensitivity / 5f, true);
        }

        // 原版 teased()（Holli.hx:240）：燃烧中完全不吃挑逗
        public override void ApplyTease(float lustDelta)
        {
            if (IsBurning)
            {
                _teasedWhileBurning = true;

                return;
            }

            base.ApplyTease(lustDelta);
        }

        public override void OnTeased(CombatContext context, float lustDelta)
        {
            if (_teasedWhileBurning)
            {
                _teasedWhileBurning = false;

                context.WriteAction("Combat.Holli.Teased.HolliDoesnT".Localized());

                return;
            }

            base.OnTeased(context, lustDelta);
        }

        // 原版 Holli.hx:232 defeated(hpVictory) → game.holliScene.defeatHolli()
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<HolliScene>.Get().DefeatHolli();

            return true;
        }

        // 原版 Holli.hx:236 won(hpVictory, pcCameWorms) → game.holliScene.enjoyYourBadEndBIYAAAATCH()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<HolliScene>.Get().EnjoyYourBadEndBIYAAAATCH();

            return true;
        }
    }
}
