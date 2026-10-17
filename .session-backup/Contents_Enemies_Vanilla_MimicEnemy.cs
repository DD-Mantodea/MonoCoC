using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/Mimic.hx（extends Monster）
    // 宝箱怪：外形（岩石/胸部/阴茎/木箱）由创建方在构造函数里传 type 决定（Mimic.hx:50）。
    // 招式/属性见 Assets/Data/Enemies/Mimic.hjson + MimicCombatHandlers.cs。
    [Register("coc:mimic")]
    public class MimicEnemy : Enemy
    {
        // 原版 Mimic.APPEARANCE_*（Mimic.hx:129-132）
        public const int AppearanceRock = 0;

        public const int AppearanceBoob = 1;

        public const int AppearanceDick = 2;

        public const int AppearanceChest = 3;

        // 原版 JACQUES00 = 199（lists/BreastCup.hx:207，本地化 Body.BreastCup.199 已存在）。
        private const float Jacques00 = 199f;

        // 原版构造函数参数 type:Int（Mimic.hx:50）。默认取 switch 的 default 分支 = 木箱。
        // 创建方 MimicScene（MimicScene.hx:179/186）通过 MimicEnemy.Create(definition, appearance)
        // 把随机到的外观传进来；直接走 EnemyRegistry 时用默认木箱。
        public int AppearanceType = AppearanceChest;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new(type) 逐项照抄（Mimic.hx:50-127）──
            // this.short = "mimic" / a = "the " 由数据表给；pronoun1-3 = it/it/its 也在数据表。

            // if (type == APPEARANCE_DICK) createCock(80, 16, CockTypesEnum.HUMAN)
            // else initedGenitals = true（C# 不自动生成生殖器，等价）。
            if (AppearanceType == AppearanceDick)
                CreateCock(80, 16, VanillaBodyPartTypes.Human);

            Balls = 0;
            BallSize = 0;

            // APPEARANCE_BOOB → createBreastRow(BreastCup.JACQUES00 = 199)，否则 createBreastRow(0)
            CreateBreastRow(AppearanceType == AppearanceBoob ? Jacques00 : 0);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            // tallness = Utils.rand(24) + 25
            Tallness = Random.Shared.Next(24) + 25;

            // hips.rating = RATING_BOYISH(0) / butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            // 原版两段独立的 if（Mimic.hx:89-96），后一段 else 会覆盖前一段：
            //   岩石先设为 gray，随后又落进 else 变成 brown。这里照抄原版语义并保留这个覆盖行为。
            if (AppearanceType == AppearanceRock)
                Body.Get<SkinPart>().Tone = "gray";

            if (AppearanceType == AppearanceBoob || AppearanceType == AppearanceDick)
                Body.Get<SkinPart>().Tone = "light";
            else
                Body.Get<SkinPart>().Tone = "brown";

            var hair = Body.Get<HairPart>();
            hair.Color = "none";
            hair.Length = 0;

            // initStrTouSpeInte(30, 45, 15, 5) / initLibSensCor(45, 45, 100) / level = 6 / lust = 10 由数据表给。
            WeaponName = "mouth";
            WeaponVerb = "bite";

            // weaponAttack = 20 + player.level（Mimic.hx:106）
            WeaponAttack = 20 + (int)(CoC.Instance?.Player?.Level ?? 0);

            // 原版 armorName/armorDef 同样是两段独立 if（Mimic.hx:107-117）：
            //   岩石先设为 stone surface / 70，随后又落进 else 变成 wooden surface / 20。
            // 照抄原版语义，不擅自“修 bug”。
            if (AppearanceType == AppearanceRock)
            {
                ArmorName = "stone surface";
                ArmorDefense = 70;
            }

            if (AppearanceType == AppearanceDick || AppearanceType == AppearanceBoob)
            {
                ArmorName = "flesh";
                ArmorDefense = 0;
            }
            else
            {
                ArmorName = "wooden surface";
                ArmorDefense = 20;
            }

            // 原版 lustVuln = 0：完全不吃挑逗。
            LustVulnerability = 0;

            Gems = Random.Shared.Next(30) + 11;

            // createPerk(PerkLib.Immovable)
            CreatePerk(VanillaPerks.Immovable);

            // 原版 drop = WeightedDrop(PPHILTR 4, NUMBOIL 4, HUMMUS_ 3, INCUBID 1, SUCMILK 1, REDUCTO 1)：
            // 见 Mimic.hjson loot。
        }

        // 原版 long 按 type 四选一（Mimic.hx:54-70）。
        public override string Description => AppearanceType switch
        {
            AppearanceRock => "Npcs.Mimic.DescRock".Localized(),
            AppearanceBoob => "Npcs.Mimic.DescBoob".Localized(),
            AppearanceDick => "Npcs.Mimic.DescDick".Localized(),
            _ => base.Description
        };

        // 原版 performCombatAction（Mimic.hx:39-48）开头：距离为 Distant 时它够不着你，整回合只出一句台词。
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (Distance == CombatDistance.Distant)
            {
                context.WriteAction("Combat.Mimic.PerformCombatAction.TheCreatureSits".Localized());

                return true;
            }

            return false;
        }

        // MimicScene 用：先设定外形再走与 EnemyRegistry.Create 相同的三步（Id/Configure/装备）。
        public static MimicEnemy Create(EnemyDefinition definition, int appearanceType)
        {
            var enemy = new MimicEnemy { AppearanceType = appearanceType };

            enemy.Id = definition.Id;

            enemy.Configure(definition);
            enemy.ApplyDefinitionEquipment(definition);

            return enemy;
        }

        // 原版 defeated() → game.mimicScene.killTheMimic()（Mimic.hx:20-22）
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.MimicScene>
                .Get().KillTheMimic();

            return true;
        }

        // 原版 won() → game.mimicScene.mimicTentacle2()（Mimic.hx:16-18）
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.MimicScene>
                .Get().MimicTentacle2();

            return true;
        }
    }
}
