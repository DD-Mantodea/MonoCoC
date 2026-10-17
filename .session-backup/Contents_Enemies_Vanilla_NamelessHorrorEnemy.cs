using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 NamelessHorror extends Monster（scenes/dungeons/manor/NamelessHorror.hx:8）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / AI / 免疫 / 减速按 Haxe 原文人工补齐。
    // 招式效果见 Contents/Combat/VanillaHandlers/NamelessHorrorCombatHandlers.cs。
    [Register("coc:nameless_horror")]
    public class NamelessHorrorEnemy : Enemy
    {
        // 原版 NamelessHorror.hx:9 `public var doomLevel:Float = 1`：conjoin 每次 +1，伤害按它放大。
        public float DoomLevel = 1;

        // 原版 NamelessHorror.hx:10 `public var originalName:String = ""`：reveal 前记录玩家名字。
        public string OriginalName = "";

        // 原版 NamelessHorror.hx:11 `public var playerStats = [0.0, 0.0, 0.0, 0.0]`。
        // 原版 refashionIt 会写入四项旧属性但从不读回（等同死字段），这里保形保留以对齐原文。
        public float[] PlayerStats = [0f, 0f, 0f, 0f];

        private static readonly Identifier OverwhelmId = new("coc", "nameless_horror_overwhelm");
        private static readonly Identifier RefashionItId = new("coc", "nameless_horror_refashion_it");
        private static readonly Identifier RevealId = new("coc", "nameless_horror_reveal");
        private static readonly Identifier BecomeNothingnessId = new("coc", "nameless_horror_become_nothingness");
        private static readonly Identifier ConjoinId = new("coc", "nameless_horror_conjoin");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（NamelessHorror.hx:179）的赋值顺序照抄 ──
            // a = "a " / short = "Nameless Horror" / long / pronoun2 = it 由数据表表达。

            // createBreastRow(Appearance.breastCupInverse("A")) = 1
            CreateBreastRow(VanillaBreastCups.A);

            // ass.analLooseness = Ass.LOOSENESS_TIGHT(1) / ass.analWetness = Ass.WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            Tallness = 180;

            // hips.rating = Hips.RATING_SLENDER(1) / butt.rating = Butt.RATING_AVERAGE(4)
            Body.SetHipsRating("slender");
            Body.SetButtRating("average");

            // skin.tone = "gray"; skin.type = Skin.PLAIN
            var skin = Body.Get<SkinPart>();
            skin.Tone = "gray";
            skin.Id = VanillaBodyPartTypes.Plain;

            var hair = Body.Get<HairPart>();
            hair.Color = "white";
            hair.Length = 0;

            // initStrTouSpeInte(100, 100, 100, 150) / initLibSensCor(40, 50, 100) / level = 60
            // / bonusHP = 99749 / lust = 0 / gems = 30 由数据表给。

            WeaponName = "tentacle";
            WeaponVerb = "lash";
            Fatigue = 0;
            WeaponAttack = 20;

            // normalAttackAvoid.doParry = false; normalAttackAvoid.doCounter = false
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            ArmorName = "taut flesh";
            ArmorDefense = 17;

            LustVulnerability = 0;

            // temperment = Monster.TEMPERMENT_LUSTY_GRAPPLES：原版死字段，无实际作用。
            // drop = new WeightedDrop()（空表）：数据表 loot 留空。
        }

        // 原版 performCombatAction（NamelessHorror.hx:86）：
        //   1) 累计受伤达到阈值 → banish()（原版 NamelessHorror.hx:87，详见下方说明）
        //   2) 每 5 回合 → conjoin()，直接结束本回合
        //   3) 否则按权重抽 overwhelm / refashionIt / reveal / becomeNothingness
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            // 原版 performCombatAction（NamelessHorror.hx:86-89）：累计受伤达到阈值 → banish()，
            // 直接结束本回合与战斗。阈值照抄 NamelessHorror.hx:87：
            //   maxHP() - HP >= 3500 * (flags[ASCENSIONING] == 1 ? 1.10 * flags[NEW_GAME_PLUS_LEVEL] : 1)
            var ascensionMultiplier = CoC.Instance.Flags.Get(VanillaFlags.Ascensioning) == 1
                ? 1.10f * CoC.Instance.Flags.Get(VanillaFlags.NewGamePlusLevel)
                : 1f;

            if (MaxHealth - Health >= 3500f * ascensionMultiplier)
            {
                Banish();

                // 战斗循环的「立刻结束」动作位（CombatController.EnemyTurn 选招后检查）。
                context.EndCombatRequested = true;

                return null;
            }

            var round = CombatController.Instance!.State.Round;

            // 原版 NamelessHorror.hx:91 `if (game.combat.combatRound % 5 == 0 && combatRound != 0)`
            if (round % 5 == 0 && round != 0)
                return new CombatAiAction { Ability = ConjoinId };

            var player = context.Player;
            var canRefashion = !player.HasStatusEffect(VanillaStatusEffects.Revelation)
                && !player.HasStatusEffect(VanillaStatusEffects.Refashioned);

            // 原版 MonsterAI 权重（NamelessHorror.hx:96-99）：
            //   overwhelm 1（恒可用）/ refashionIt 1（条件）/ reveal 0.33（同条件）/ becomeNothingness 0.5（另一个条件）
            var choices = new List<(Identifier Id, float Weight, int Cost, FatigueType Fatigue, CombatRange Range)>
            {
                (OverwhelmId, 1f, 0, FatigueType.None, CombatRange.FlyingMelee)
            };

            if (canRefashion)
            {
                choices.Add((RefashionItId, 1f, 10, FatigueType.Magical, CombatRange.Omni));
                choices.Add((RevealId, 0.33f, 10, FatigueType.Magical, CombatRange.Omni));
            }

            if (!player.HasStatusEffect(VanillaStatusEffects.Nothingness))
                choices.Add((BecomeNothingnessId, 0.5f, 10, FatigueType.Magical, CombatRange.Omni));

            // 只保留「疲劳够 + 射程够」的项（等价于 CombatAiInterpreter 的预过滤）。
            var eligible = choices
                .Where(c => context.Enemy.HasFatigue(c.Cost, c.Fatigue))
                .Where(c => CombatRangeData.CanReach(context.Enemy, player, context.Enemy.Distance, c.Range))
                .ToList();

            if (eligible.Count == 0)
                return new CombatAiAction { Ability = OverwhelmId, Cost = 0, Fatigue = FatigueType.None };

            var total = eligible.Sum(c => c.Weight);
            var roll = context.Random.NextDouble() * total;

            foreach (var choice in eligible)
            {
                roll -= choice.Weight;

                if (roll <= 0)
                    return new CombatAiAction { Ability = choice.Id, Cost = choice.Cost, Fatigue = choice.Fatigue };
            }

            var last = eligible[^1];

            return new CombatAiAction { Ability = last.Id, Cost = last.Cost, Fatigue = last.Fatigue };
        }

        // 原版 handleDamaged（NamelessHorror.hx:23）：玩家处于 Nothingness 时，只有「魔法攻击 + 智力 >= 90」
        // 能打出 40% 伤害并附一句文本，其余伤害一律归零。
        // C# 的 CombatDamageKind 只有 Melee/Ranged/Magical，对应原版 MAGICAL_MELEE / MAGICAL_RANGED 用 Magical。
        public override float ModifyIncomingDamage(CombatContext context, float damage, CombatDamageKind kind)
        {
            if (!context.Player.HasStatusEffect(VanillaStatusEffects.Nothingness))
                return damage;

            if (kind == CombatDamageKind.Magical && context.Player.Intelligence >= 90)
            {
                context.Append("Combat.NamelessHorror.HandleDamaged.YouManageTo".Localized());

                return damage * 0.4f;
            }

            return 0;
        }

        // 原版 handleStun（NamelessHorror.hx:38）：1/3 概率自行恢复（清掉 Stunned 并返回 true），否则走基类。
        public override bool HandleStun(CombatContext context)
        {
            if (context.Random.Next(3) == 0)
            {
                context.WriteAction("Combat.NamelessHorror.HandleStun.TheCyclopeanCreature".Localized());

                RemoveStatusEffect(VanillaStatusEffects.Stunned);

                return true;
            }

            return base.HandleStun(context);
        }

        // 原版 handleFear（NamelessHorror.hx:13-21）：清掉 Fear 与 Resolve，然后调 manor.testResolve()。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.NamelessHorror.HandleFear.TheCreatureShrieks".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);
            context.Player.RemoveStatusEffect(VanillaStatusEffects.Resolve);

            // 原版 NamelessHorror.hx:19 game.dungeons.manor.testResolve()
            SceneInstance<ManorScene>.Get().TestResolve();

            return true;
        }

        // 原版 handleCombatLossText（NamelessHorror.hx:82）：返回 1 小时且不写通用战败文案。
        public override int HandleCombatLossText(CombatContext context, bool inDungeon, int gemsLost) => 1;

        // 原版 NamelessHorror.hx:47-49：banish(defeatedHorror) → game.dungeons.manor.banish(defeatedHorror)
        public void Banish(bool defeatedHorror = false) =>
            SceneInstance<ManorScene>.Get().Banish(defeatedHorror);

        // 原版 won（NamelessHorror.hx:52-54）→ manor.loseToNamelessHorror()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<ManorScene>.Get().LoseToNamelessHorror();

            return true;
        }

        // 原版 defeated（NamelessHorror.hx:56-66）：
        //   silly 模式 → 「Blowjob / Wait」两个按钮（areYouFuckingSerious / banish）
        //   否则 → banish(true)
        public override bool Defeated(bool hpVictory)
        {
            var scene = SceneInstance<ManorScene>.Get();

            if (MonoCoC.OptionSystem.OptionStore.Instance.Get("silly", 0) > 0)
            {
                Scene.ClearOutput();
                Scene.OutputText("Combat.NamelessHorror.Defeated.ThroughMiraculousPower".Localized());
                Scene.ClearButtons();
                Scene.AddButton(0, "Blowjob", AreYouFuckingSerious,
                    "Charge in and let the Nameless Horror blowjob you.");
                Scene.AddButton(1, "Wait", () => scene.Banish(true), "Better be cautious and wait.");

                return true;
            }

            scene.Banish(true);

            return true;
        }

        // 原版 NamelessHorror.hx:68-80 areYouFuckingSerious：silly 分支的彩蛋，最后 doNext(banish(true))。
        private static void AreYouFuckingSerious()
        {
            CoC.Instance.Flags.Set(VanillaFlags.NhBj, 1);

            Scene.ClearOutput();

            if (!Scene.Player.HasCock())
                Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.YouSuddenlySprout".Localized());

            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.ShatteredAndBroken".Localized());
            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.YouApproachThe".Localized());
            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.FamiliarScreechingRings".Localized());
            Scene.OutputText("Combat.NamelessHorror.AreYouFuckingSerious.OnwardStillThe".Localized());

            Scene.AwardAchievement(MonoCoC.Contents.Achievements.VanillaAchievements.LordBritishPostulate, true, true);
            Scene.DoNext(() => SceneInstance<ManorScene>.Get().Banish(true));
        }
    }
}
