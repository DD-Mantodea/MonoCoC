using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Helspawn extends Monster（src/classes/scenes/npcs/Helspawn.hx）
    // 招式见 Contents/Combat/VanillaHandlers/HelspawnCombatHandlers.cs。
    //
    // won/defeated 不覆写：原版 Helspawn.hx:134 defeated 调用 game.helSpawnScene.beatUpYourDaughter()、
    // 原版 Helspawn.hx:138 won 调用 game.helSpawnScene.loseSparringToDaughter()。
    // C# 侧缺 HelSpawnScene（全量搜索 `class \w*Hel ?Spawn` 只命中 HelspawnFollower / HelspawnLover；
    // `beatUpYourDaughter` / `loseSparringToDaughter` 均 0 命中），胜负分支无处可接，等场景落地后回填。
    [Register("coc:helspawn")]
    public class HelspawnEnemy : Enemy
    {
        // 原版 Helspawn.hx:10 usedFocus
        public bool UsedFocus;

        // 原版 Helspawn.hx:144 读取全局旗标 HELSPAWN_WEAPON / HELSPAWN_PERSONALITY
        public string Weapon => MonoCoC.CoC.Instance.Flags.Get(VanillaFlags.HelspawnWeapon);

        public int Personality => MonoCoC.CoC.Instance.Flags.Get(VanillaFlags.HelspawnPersonality);

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var weapon = Weapon;

            // 原版 Helspawn.hx:151 this.short = game.helSpawnScene.helspawnName（= 旗标 HELSPAWN_NAME）。
            // 原版没有空名兜底；这里用数据表的 nameKey（Helspawn）兜底，避免未命名时 UI 空白。
            var name = MonoCoC.CoC.Instance.Flags.Get(VanillaFlags.HelspawnName);

            if (!string.IsNullOrEmpty(name))
                ShortName = name;

            // 原版 new() 逐项照抄
            CreateVagina(false, 1, 2);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 85);

            // createBreastRow(Appearance.breastCupInverse("E+"))
            CreateBreastRow(VanillaBreastCups.EBig);

            // ass.analLooseness = Ass.LOOSENESS_VIRGIN(0) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 0;
            anal.Wetness = 0;

            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 85);

            Tallness = 78;

            // hips.rating = Hips.RATING_CURVY(10) + 2 / butt.rating = Butt.RATING_LARGE(8) + 1
            Body.Hips = 12;
            Body.Butt = 9;

            Body.Get<SkinPart>().Tone = "dusky";

            var hair = Body.Get<HairPart>();
            hair.Color = "red";
            hair.Length = 13;

            // initStrTouSpeInte(50, 50, 65, 40)
            Strength = 50;
            Toughness = 50;
            Speed = 65;
            Intelligence = 40;

            // initLibSensCor(35, 55, 20)
            Libido = 35;
            Sensitivity = 55;
            Corruption = 20;

            // 原版 weaponName 直接就是旗标值（bow / scimitar / scimitar and shield）
            WeaponName = string.IsNullOrEmpty(weapon) ? "scimitar" : weapon;

            WeaponVerb = weapon switch
            {
                "bow" => "blunted arrow",
                _ => "slash"
            };

            WeaponAttack = 20;

            if (weapon == "bow")
            {
                NormalAttackAvoid.DoParry = false;
                NormalAttackAvoid.DoCounter = false;
            }

            ArmorName = "scales";
            ArmorDefense = 12;
            ArmorPerk = "";
            ArmorValue = 50;

            // bonusHP = 175：走数据表 bonusHealth（base.Configure 已经加进最大生命）

            Lust = 30;
            LustVulnerability = 0.55f;

            // 原版 Helspawn.hx:189 temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：死字段，
            // AI 权重由 Assets/Data/Combat/Ai/Helspawn.hjson 表达。

            Level = 12;
            Gems = 10 + Random.Shared.Next(5);

            // 原版 tail.type = Tail.SALAMANDER；tail.recharge = 0（C# TailPart 没有 recharge 字段）
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Salamander;

            // 原版 Helspawn.hx:194 drop = NO_DROP → 数据表不配 loot
        }

        // 原版 this.long（Helspawn.hx:153）：名字 / 衣着（人格旗标 >= 50）/ 武器三段都是动态值，
        // 所以拆成模板 + 三个子 key 现拼。
        public override string Description => "Npcs.Helspawn.Desc".Localized(
            ShortName,
            Personality >= 50
                ? "Npcs.Helspawn.Outfit.Slutty".Localized()
                : "Npcs.Helspawn.Outfit.Modest".Localized(),
            Weapon switch
            {
                "bow" => "Npcs.Helspawn.Weapon.Bow".Localized(),
                "scimitar and shield" => "Npcs.Helspawn.Weapon.Shield".Localized(),
                _ => "Npcs.Helspawn.Weapon.Scimitar".Localized()
            });

        // 原版 performCombatAction 末尾（Helspawn.hx:22）：无论本轮出什么招，都有 25% 概率追加一记尾鞭。
        // BeforeCombatAction 返回 false 表示「不消费行动」，正好让 AI 继续抽主招式。
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (context.Random.Next(4) == 0)
                HelspawnCombatHandlers.TailWhip(context);

            return false;
        }
    }
}
