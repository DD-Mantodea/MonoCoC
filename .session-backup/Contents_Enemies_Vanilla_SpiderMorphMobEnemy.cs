using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/areas/swamp/SpiderMorphMob.hx（extends Monster）
    // 与 Kiha 并肩作战的蜘蛛娘/蛛化精灵群。原版 performCombatAction 是自定义二选一，
    // 不走 MonsterAI 权重表，所以整段放在 BeforeCombatAction 里。
    [Register("coc:spider_morph_mob")]
    public class SpiderMorphMobEnemy : Enemy
    {
        // 原版 public var missFirstRound:Bool = false（SpiderMorphMob.hx:8）：
        // 由场景在「玩家输给 Kiha 后的 1-3 回合」置位；场景未移植（原版 SpiderMorphMob.hx:8），当前恒为 false。
        public bool MissFirstRound;

        // 原版 StatusEffects.UBERWEB（SpiderMorphMob.hx:60-78）：被蛛网糊住。
        // C# 侧常量在 VanillaStatusEffects.UberWeb（共享层，已接入 CombatController.IsPlayerBound）。

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new() 逐项照抄（SpiderMorphMob.hx:114-155）──
            // createCock(9, 2, CockTypesEnum.HUMAN)
            CreateCock(9, 2, VanillaBodyPartTypes.Human);

            Balls = 2;
            BallSize = 1;

            // cumMultiplier = 3
            CumMultiplier = 3;

            // createVagina(false, WETNESS_SLICK(3), LOOSENESS_LOOSE(3))
            CreateVagina(false, 3, 3);

            CreateBreastRow(0);

            // ass.analLooseness = LOOSENESS_STRETCHED(4) / analWetness = WETNESS_SLIME_DROOLING(5)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 4;
            anal.Wetness = 5;

            // tallness = Utils.rand(8) + 70
            Tallness = Random.Shared.Next(8) + 70;

            // hips.rating = RATING_AMPLE(6) + 2 / butt.rating = RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();
            hair.Color = "black";
            hair.Length = 15;

            // 原版 level = 18 / lustVuln = .2 / bonusHP = 1200 / 属性都在数据表里。
            LustVulnerability = 0.2f;

            WeaponName = "claws";
            WeaponVerb = "claws";
            ArmorName = "chitin";

            // tail.type = Tail.SPIDER_ABDOMEN
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.SpiderAbdomen;

            // 原版 gems = Utils.rand(25) + 40
            Gems = Random.Shared.Next(25) + 40;

            // 原版 drop = NO_DROP：数据表 loot 留空。
        }

        // 原版 long（SpiderMorphMob.hx:119）里按玩家等级二选一措辞。
        public override string Description =>
            (CoC.Instance?.Player?.Level ?? 0) <= 13
                ? "Npcs.SpiderMorphMob.DescHorde".Localized()
                : base.Description;

        // 原版 performCombatAction（SpiderMorphMob.hx:92-99）：
        //   game.spriteSelect(SpriteDb.s_kiha);（图片选择，C# 无对应运行时，跳过）
        //   rand(2)==0 或玩家被 UBERWEB → 普攻；否则吐网。
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (context.Random.Next(2) == 0 || context.Player.HasStatusEffect(VanillaStatusEffects.UberWeb))
                SpiderStandardAttack(context);
            else
                WebLaunch(context);

            return true;
        }

        // 原版 spiderStandardAttack()（SpiderMorphMob.hx:12-57）
        private void SpiderStandardAttack(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,
                Defender = player,
                DoDodge = true,
                DoParry = true,
                DoBlock = false
            });

            // SPIDER HORDE ATTACK - Miss（回合 1-3 且玩家输给 Kiha 时必定 miss）
            if (MissFirstRound || avoid.AttackFailed)
            {
                MissFirstRound = false;

                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.ANumberOf".Localized());

                KihaSpidahai(context);

                return;
            }

            // SPIDER HORDE ATTACK - Hit
            context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.ANumberOf2".Localized());

            // 原版：player.reduceDamage(str + weaponAttack, this) + 20
            var damage = CombatCalculator.ReduceDamage(context, player, Strength + WeaponAttack) + 20;

            var capitalA = MonoCoC.Utils.Utils.CapitalizeFirstLetter(Article);

            if (damage <= 0)
            {
                damage = 0;

                if (context.Random.Next(Math.Max(1, (int)(player.ArmorDefense + player.Toughness)))
                    < player.ArmorDefense)
                {
                    context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.YouAbsorbAnd".Localized(WeaponVerb));
                }
                else
                {
                    context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.YouDeflectAnd".Localized()
                        WeaponVerb, Article, ShortName);
                }
            }
            else if (damage < 6)
            {
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.YouAreStruck".Localized()
                    Article, ShortName);
            }
            else if (damage < 11)
            {
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.01Wounds".Localized()
                    capitalA, ShortName);
            }
            else if (damage < 21)
            {
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.01Staggers".Localized()
                    capitalA, ShortName, Pronoun3, WeaponVerb);
            }
            else
            {
                context.Append("Combat.SpiderMorphMob.SpiderStandardAttack.01Mutilate".Localized()
                    capitalA, ShortName, Pronoun3, WeaponVerb);
            }

            if (damage > 0)
            {
                player.TakeDamage(damage);

                // 原版：lustVuln > 0 && player.armor == armors.BONSTRP
                if (LustVulnerability > 0 && player.Armor.Id.Path == "bondage_straps")
                {
                    context.Append((Plural
                        ? "Combat.SpiderMorphMob.SpiderStandardAttack.01Brush"
                        : "Combat.SpiderMorphMob.SpiderStandardAttack.01Brushes")
                        .Localized(capitalA, ShortName));

                    Lust += 10 * LustVulnerability;
                }
            }

            KihaSpidahai(context);
        }

        // 原版 kihaSPOIDAHAI()（SpiderMorphMob.hx:84-90）：每回合 Kiha 补一刀，蜘蛛群掉 50 血。
        private void KihaSpidahai(CombatContext context)
        {
            context.Append("Combat.SpiderMorphMob.KihaSPOIDAHAI.Text".Localized());
            context.Append("Combat.SpiderMorphMob.KihaSPOIDAHAI.WhileTheyRe".Localized());

            HealthChange(-50);
        }

        // 原版 spoidahHordeWebLaunchahs()（SpiderMorphMob.hx:68-82）
        private void WebLaunch(CombatContext context)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = this,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (MissFirstRound || avoid.AttackFailed)
            {
                MissFirstRound = false;

                context.Append("Combat.SpiderMorphMob.SpoidahHordeWebLaunchahs.OneOfThe".Localized());

                return;
            }

            context.Append("Combat.SpiderMorphMob.SpoidahHordeWebLaunchahs.SomeOfThe".Localized());

            // 原版 player.createStatusEffect(StatusEffects.UBERWEB, 0, 0, 0, 0); HP -= 250;
            // UBERWEB 的「本回合不能攻击/施法」由 CombatController.IsPlayerBound() 识别（共享层已接入）。
            player.CreateStatusEffect(VanillaStatusEffects.UberWeb, 0, 0, 0, 0);
            HealthChange(-250);
        }

        // 原版 struggle()（SpiderMorphMob.hx:59-65）：被蛛网糊住时挣扎脱身。
        public override void Struggle(CombatContext context)
        {
            if (!context.Player.HasStatusEffect(VanillaStatusEffects.UberWeb))
                return;

            context.Clear();
            context.Append("Combat.SpiderMorphMob.Struggle.YouClawYour".Localized());

            context.Player.RemoveStatusEffect(VanillaStatusEffects.UberWeb);
        }

        // 原版 defeated() → game.kihaFollowerScene.beatSpiderMob()  （SpiderMorphMob.hx:101-103）
        // 原版 won()      → game.kihaFollowerScene.loseToSpiderMob()（SpiderMorphMob.hx:105-112）
        // 场景缺失：Contents/Scenes 全量搜索 KihaFollowerScene = 0 命中（原版 SpiderMorphMob.hx:102/110；
        // 注意 Contents/Scenes/Vanilla/Npcs/KihaScene.cs 是另一个场景，不含这两个回调），先按默认收尾。
    }
}
