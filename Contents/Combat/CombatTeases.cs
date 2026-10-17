using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Combat
{
    /// <summary>
    /// 原版 CombatTeases.hx 的挑逗系统：48 个 case 的文本库已全部移植，
    /// 门控（BuildChoices）、加成结算（ComputeBonuses）、命中判定（RollHit）齐备。
    /// 说明：该文件尚未被 PlayerTease 调用，等 teaseAttack 补齐后一并接线。
    /// </summary>
    public static class CombatTeases
    {
        /// <summary>
        /// 原版 :175-440：按玩家体型 / 性别 / 装备把可用的 case 号灌进列表，
        /// 同一个 case 号重复出现即代表权重更高。分三大互斥组：
        ///   1) 婊子/扶她且性别 ≥ 女性 → 4 / 5 / 2 / 6 / 7 / 10 / 11
        ///   2) bro（兄弟体）          → 8 / 9 / 10 / 11
        ///   3) 其余（原版）           → 0 / 1 / 2 / 3
        /// </summary>
        public static List<int> BuildChoices(Player player)
        {
            var choices = new List<int>();

            var butt = player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating;

            var biggestTit = player.BiggestBreastSize();

            // 原版 :90-99 三种体质 perk
            var bimbo = player.HasPerk(PerkIds.BimboBody);

            var bro = player.HasPerk(PerkIds.BroBody);

            var futa = player.HasPerk(PerkIds.FutaForm);

            // 原版 :107：任一特殊体质都会让 bimbo 组生效
            if (bimbo || bro || futa)
                bimbo = true;

            if (bimbo && (int)player.Gender >= 2)
            {
                // 原版 :175-192：屁股越大权重越高
                choices.Add(4);

                if (butt >= 7) choices.Add(4);
                if (butt >= 10) choices.Add(4);
                if (butt >= 14) choices.Add(4);
                if (butt >= 20) choices.Add(4);
                if (butt >= 25) choices.Add(4);

                // 原版 :194-211：胸部阈值 2/4/8/30/50/75/100
                if (biggestTit >= 2) choices.Add(5);
                if (biggestTit >= 4) choices.Add(5);
                if (biggestTit >= 8) choices.Add(5);
                if (biggestTit >= 30) choices.Add(5);
                if (biggestTit >= 50) choices.Add(5);
                if (biggestTit >= 75) choices.Add(5);
                if (biggestTit >= 100) choices.Add(5);

                // 原版 :213-228：有阴道 → 露下体；湿度/容量推高「婊子式」权重
                if (player.HasVagina())
                {
                    choices.Add(2);

                    var wetness = player.Wetness();

                    if (wetness >= 3) choices.Add(6);
                    if (wetness >= 5) choices.Add(6);
                    if (player.VaginalCapacity() >= 30) choices.Add(6);
                    if (player.VaginalCapacity() >= 60) choices.Add(6);
                    if (player.VaginalCapacity() >= 75) choices.Add(6);
                }

                // 原版 :230-236：Adjatha 定制品——臀、胯、胸都达标时给 case 7 四次权重
                var hips = player.Body.Get<BodyParts.Vanilla.HipsPart>().Rating;

                if (player.HasVagina() && butt >= 8 && hips >= 6 && biggestTit >= 6)
                {
                    choices.Add(7);

                    choices.Add(7);

                    choices.Add(7);

                    choices.Add(7);
                }

                // 原版 :238-270：鸡巴（扶她）
                if (player.HasCock())
                {
                    choices.Add(10);

                    choices.Add(11);

                    var cockArea = player.BiggestCockArea();

                    if (player.CockTotal() > 1) choices.Add(10);
                    if (player.CockTotal() >= 2) choices.Add(11);
                    if (cockArea >= 10) choices.Add(10);
                    if (cockArea >= 25) choices.Add(11);
                    if (cockArea >= 50) choices.Add(11);
                    if (cockArea >= 75) choices.Add(10);
                    if (cockArea >= 100) choices.Add(11);
                    if (cockArea >= 300) choices.Add(10);
                }
            }
            else if (bro)
            {
                // 原版 :271-289：胸肌舞（平胸且体格好）
                if (biggestTit < 1 && player.Tone >= 60)
                {
                    choices.Add(8);

                    if (player.Tone >= 70) choices.Add(8);
                    if (player.Tone >= 80) choices.Add(8);
                    if (player.Tone >= 90) choices.Add(8);
                    if (player.Tone == 100) choices.Add(8);
                }

                // 原版 :291-310：英雄姿势（体格 + 力量百分比）
                if (player.Tone >= 60 && player.StrengthPercent >= 50)
                {
                    choices.Add(9);

                    if (player.Tone >= 80) choices.Add(9);
                    if (player.StrengthPercent >= 70) choices.Add(9);
                    if (player.Tone >= 90) choices.Add(9);
                    if (player.StrengthPercent >= 80) choices.Add(9);
                }

                // 原版 :312-343：露鸡巴
                if (player.HasCock())
                {
                    choices.Add(10);

                    choices.Add(11);

                    var cockArea = player.BiggestCockArea();

                    if (player.CockTotal() > 1) choices.Add(10);
                    if (player.CockTotal() >= 2) choices.Add(11);
                    if (cockArea >= 10) choices.Add(10);
                    if (cockArea >= 25) choices.Add(11);
                    if (cockArea >= 50) choices.Add(11);
                    if (cockArea >= 75) choices.Add(10);
                    if (cockArea >= 100) choices.Add(11);
                    if (cockArea >= 300) choices.Add(10);
                }
            }
            else
            {
                // 原版 :345-360：拍屁股（默认至少一次）
                choices.Add(0);

                if (butt >= 7) choices.Add(0);
                if (butt >= 10) choices.Add(0);
                if (butt >= 14) choices.Add(0);
                if (butt >= 20) choices.Add(0);
                if (butt >= 25) choices.Add(0);

                // 原版 :362-387：抖胸，阈值 2/4/8/15/30/50/75/100
                if (biggestTit >= 2) choices.Add(1);
                if (biggestTit >= 4) choices.Add(1);
                if (biggestTit >= 8) choices.Add(1);
                if (biggestTit >= 15) choices.Add(1);
                if (biggestTit >= 30) choices.Add(1);
                if (biggestTit >= 50) choices.Add(1);
                if (biggestTit >= 75) choices.Add(1);
                if (biggestTit >= 100) choices.Add(1);

                // 原版 :389-406：露下体
                if (player.HasVagina())
                {
                    choices.Add(2);

                    var wetness = player.Wetness();

                    if (wetness >= 3) choices.Add(2);
                    if (wetness >= 5) choices.Add(2);
                    if (player.VaginalCapacity() >= 30) choices.Add(2);
                    if (player.VaginalCapacity() >= 60) choices.Add(2);
                    if (player.VaginalCapacity() >= 75) choices.Add(2);
                }

                // 原版 :408-440：露鸡巴
                if (player.HasCock())
                {
                    choices.Add(3);

                    var cockArea = player.BiggestCockArea();

                    if (player.CockTotal() > 1) choices.Add(3);
                    if (player.CockTotal() >= 2) choices.Add(3);
                    if (cockArea >= 10) choices.Add(3);
                    if (cockArea >= 25) choices.Add(3);
                    if (cockArea >= 50) choices.Add(3);
                    if (cockArea >= 75) choices.Add(3);
                    if (cockArea >= 100) choices.Add(3);
                    if (cockArea >= 300) choices.Add(3);
                }
            }

            return choices;
        }

        /// <summary>
        /// 原版 :74-125 的命中基线与伤害基线：
        ///   命中 = 60 + 挑逗精通等级×5
        ///   伤害 = 6 + rand(3) + 等级分段加成
        /// </summary>
        public static (float Chance, float Damage) RollBaseline(Player player)
        {
            // 原版 MasteryLib.hx:11：挑逗精通 id 是 "Tease"（大写）。
            // 这里原写成 CombatAbilityIds.Tease（小写 "tease"，那是技能 id），
            // 会与 CharCreation / UrtaQuestScene 写入的 "Tease" 变成两条互不相认的熟练度。
            var chance = 60f + player.MasteryLevel(new MonoCoC.Registry.Identifier("coc", "Tease")) * 5f;

            var damage = 6f + Random.Shared.Next(3);

            if (player.Level < 30)
                damage += player.Level;
            else if (player.Level < 60)
                damage += 30 + (player.Level - 30) / 2;
            else
                damage += 45 + (player.Level - 60) / 4;

            // 原版护甲的 boostsTeaseChance / boostsTeaseDamage
            var (armorChance, armorDamage) = Items.ArmorItemBonuses.Tease(player);

            chance += armorChance;

            damage += armorDamage;

            return (chance, damage);
        }

        /// <summary>
        /// 原版 :1486-1740 的 auto 加成结算：按本次用到的身体部位汇总 bonusChance / bonusDamage，
        /// 各自封顶 5 / 10。
        /// </summary>
        public static (float Chance, float Damage) ComputeBonuses(Player player, TeaseFlags flags)
        {
            if (!flags.Auto)
                return (0f, 0f);

            var bonusChance = 0f;

            var bonusDamage = 0f;

            var biggestTit = player.BiggestBreastSize();

            var butt = player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating;

            var hips = player.Body.Get<BodyParts.Vanilla.HipsPart>().Rating;

            var anal = player.Body.Get<BodyParts.Vanilla.AnalPart>();

            var wetness = player.Wetness();

            // ---- 胸部加成（原版 :1491-1535）----
            if (flags.Breasts)
            {
                if (player.BreastRows.Count > 1) { bonusChance += 0.5f; bonusDamage += 1; }
                if (player.BreastRows.Count > 2) { bonusChance += 0.5f; bonusDamage += 1; }
                if (player.BreastRows.Count > 4) { bonusChance += 0.5f; bonusDamage += 1; }
                if (player.BiggestLactation() >= 2) { bonusChance += 1; bonusDamage += 2; }
                if (player.BiggestLactation() >= 3) { bonusChance += 1; bonusDamage += 2; }
                if (biggestTit >= 4) { bonusChance += 0.5f; bonusDamage += 1; }
                if (biggestTit >= 7) { bonusChance += 0.5f; bonusDamage += 1; }
                if (biggestTit >= 12) { bonusChance += 0.5f; bonusDamage += 1; }
                if (biggestTit >= 25) { bonusChance += 0.5f; bonusDamage += 1; }
                if (biggestTit >= 50) { bonusChance += 0.5f; bonusDamage += 1; }
                if (player.HasFuckableNipples()) { bonusChance += 1; bonusDamage += 2; }
                if (player.AverageNipplesPerBreast() > 1) { bonusChance += 1; bonusDamage += 2; }
            }

            // ---- 阴道加成（原版 :1536-1595）----
            if (flags.Vagina && player.HasVagina())
            {
                if (wetness >= 2) { bonusChance += 0.5f; bonusDamage += 1; }
                if (wetness >= 3) { bonusChance += 0.5f; bonusDamage += 1; }
                if (wetness >= 4) { bonusChance += 0.5f; bonusDamage += 1; }
                if (wetness >= 5) { bonusChance += 0.5f; bonusDamage += 1; }

                var clit = player.GetClitLength();

                if (clit > 1.5f) { bonusChance += 0.5f; bonusDamage += 1; }
                if (clit > 3.5f) { bonusChance += 0.5f; bonusDamage += 1; }
                if (clit > 7) { bonusChance += 0.5f; bonusDamage += 1; }
                if (clit > 12) { bonusChance += 0.5f; bonusDamage += 1; }

                var capacity = player.VaginalCapacity();

                if (capacity >= 30) { bonusChance += 0.5f; bonusDamage += 1; }
                if (capacity >= 70) { bonusChance += 0.5f; bonusDamage += 1; }
                if (capacity >= 120) { bonusChance += 0.5f; bonusDamage += 1; }
                if (capacity >= 200) { bonusChance += 0.5f; bonusDamage += 1; }
            }

            // ---- 阴茎加成（原版 :1596-1660）----
            if (flags.Penis)
            {
                var cockArea = player.BiggestCockArea();

                if (player.CockTotal() > 1) { bonusChance += 1; bonusDamage += 2; }
                if (cockArea >= 15) { bonusChance += 0.5f; bonusDamage += 1; }
                if (cockArea >= 30) { bonusChance += 0.5f; bonusDamage += 1; }
                if (cockArea >= 60) { bonusChance += 0.5f; bonusDamage += 1; }
                if (cockArea >= 120) { bonusChance += 0.5f; bonusDamage += 1; }

                var cum = player.CumQ();

                if (cum >= 50) { bonusChance += 0.5f; bonusDamage += 1; }
                if (cum >= 150) { bonusChance += 0.5f; bonusDamage += 1; }
                if (cum >= 300) { bonusChance += 0.5f; bonusDamage += 1; }
                if (cum >= 1000) { bonusChance += 0.5f; bonusDamage += 1; }

                if (flags.Balls)
                {
                    if (player.Balls > 2) { bonusChance += 1; bonusDamage += 2; }
                    if (player.BallSize > 3) { bonusChance += 0.5f; bonusDamage += 1; }
                    if (player.BallSize > 7) { bonusChance += 0.5f; bonusDamage += 1; }
                    if (player.BallSize > 12) { bonusChance += 0.5f; bonusDamage += 1; }
                }

                // 原版 :1648：鸡巴太小反而扣分
                if (cockArea < 8) { bonusChance -= 1; bonusDamage -= 2; }

                if (cockArea < 5) { bonusChance -= 1; bonusDamage -= 2; }
            }

            // ---- 臀胯加成（原版 :1655-1695）----
            if (flags.Ass)
            {
                if (butt >= 6) { bonusChance += 0.5f; bonusDamage += 1; }
                if (butt >= 10) { bonusChance += 0.5f; bonusDamage += 1; }
                if (butt >= 13) { bonusChance += 0.5f; bonusDamage += 1; }
                if (butt >= 16) { bonusChance += 0.5f; bonusDamage += 1; }
                if (butt >= 20) { bonusChance += 0.5f; bonusDamage += 1; }

                if (hips >= 6) { bonusChance += 0.5f; bonusDamage += 1; }
                if (hips >= 10) { bonusChance += 0.5f; bonusDamage += 1; }
                if (hips >= 13) { bonusChance += 0.5f; bonusDamage += 1; }
                if (hips >= 16) { bonusChance += 0.5f; bonusDamage += 1; }
                if (hips >= 20) { bonusChance += 0.5f; bonusDamage += 1; }
            }

            // ---- 肛门加成（原版 :1696-1740）----
            if (flags.Anus)
            {
                if (anal.Looseness == 0) { bonusChance += 1.5f; bonusDamage += 3; }
                if (anal.Wetness > 0) { bonusChance += 1; bonusDamage += 2; }

                var analCapacity = player.AnalCapacity();

                if (analCapacity >= 30) { bonusChance += 0.5f; bonusDamage += 1; }
                if (analCapacity >= 70) { bonusChance += 0.5f; bonusDamage += 1; }
                if (analCapacity >= 120) { bonusChance += 0.5f; bonusDamage += 1; }
                if (analCapacity >= 200) { bonusChance += 0.5f; bonusDamage += 1; }

                if (anal.Looseness == 4) { bonusChance += 0.5f; bonusDamage += 1; }
                if (anal.Looseness == 5) { bonusChance += 1.5f; bonusDamage += 3; }
            }

            // 原版 :1734-1740：加成封顶
            if (bonusChance > 5) bonusChance = 5;
            if (bonusDamage > 10) bonusDamage = 10;

            return (bonusChance, bonusDamage);
        }

        /// <summary>
        /// 原版 :1741-1760 的命中结算：伤害先乘 0.7 削弱，
        /// 「妓女史」perk 再乘 1.15 补回。
        /// </summary>
        public static bool RollHit(Player player, float chance, ref float damage, ref float bonusDamage)
        {
            if (Random.Shared.Next(100) > chance)
                return false;

            // 原版 :1742 NERF TEASE DAMAGE
            damage *= 0.7f;

            bonusDamage *= 0.7f;

            if (player.HasPerk(PerkIds.HistoryWhore))
            {
                damage *= 1.15f;

                bonusDamage *= 1.15f;
            }

            return true;
        }

        /// <summary>原版 :809 起 switch (select) 的分发。</summary>
        public static void Apply(CombatContext context, int select, TeaseFlags flags)
        {
            switch (select)
            {
                case 0: ButtShake(context, flags); break;
                case 1: BreastJiggle(context, flags); break;
                case 2: PussyFlash(context, flags); break;
                case 3: CockFlash(context, flags); break;
                case 4: BimboButtShake(context, flags); break;
                case 5: BimboBreastJiggle(context, flags); break;
                case 6: BimboPussyFlash(context, flags); break;
                case 7: BendOverBimbo(context, flags); break;
                case 8: PecDance(context, flags); break;
                case 9: HeroicPose(context, flags); break;
                case 10: BulgyGroinThrust(context, flags); break;
                case 11: ShowOffDick(context, flags); break;
                case 12: CatFlexibility(context, flags); break;
                case 13: PregnantPose(context, flags); break;
                case 14: BroodMother(context, flags); break;
                case 15: NippleCunts(context, flags); break;
                case 16: AnalGape(context, flags); break;
                case 17: BeeAbdomen(context, flags); break;
                case 18: DogTease(context, flags); break;
                case 19: MaxFeminine(context, flags); break;
                case 20: MaxMasculine(context, flags); break;
                case 21: MaxAndrogyny(context, flags); break;
                case 22: SpiderSilk(context, flags); break;
                case 23: RutTease(context, flags); break;
                case 24: PoleDance(context, flags); break;
                case 25: TallTease(context, flags); break;
                case 26: MagicTease(context, flags); break;
                case 27: FeederTease(context, flags); break;
                case 28: FemaleTeacherCostume(context, flags); break;
                case 29: MaleTeacherCostume(context, flags); break;
                case 30: NagaFetishClothes(context, flags); break;
                case 31: CentaurHarness(context, flags); break;
                case 32: GenderlessServant(context, flags); break;
                case 33: CrotchRevealingClothes(context, flags); break;
                case 34: MaidCostume(context, flags); break;
                case 35: ServantBoyClothes(context, flags); break;
                case 36: BondagePatientClothes(context, flags); break;
                case 37: KitsuneTeaseA(context, flags); break;
                case 38: KitsuneTeaseB(context, flags); break;
                case 39: KitsuneTeaseC(context, flags); break;
                case 40: KitsuneTeaseD(context, flags); break;
                case 41: KitsuneGendered(context, flags); break;
                case 42: UrtaTease(context, flags); break;
                case 43: CowgirlTease(context, flags); break;
                case 44: MaidenArmorTease(context, flags); break;
                case 45: LethiciteArmorTease(context, flags); break;
                case 46: NephilaRegaliaTease(context, flags); break;
                case 47: NephilaQueenTease(context, flags); break;
            }
        }

        /// <summary>原版 :811 case 0 —— 拍屁股。</summary>
        private static void ButtShake(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var butt = player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating;

            context.Write("Combat.Tease.Case0.YouSlapYour".Localized());

            // 原版 :814：屁股评分 ≥10 且体脂 <60 时「颤动得赏心悦目」
            if (butt >= 10 && player.Tone < 60)
                context.Write("Combat.Tease.Case0.MakingJiggleDelightfully".Localized());
            else
                context.Write("Combat.Tease.Case0.Line813".Localized());

            flags.Ass = true;
        }

        /// <summary>原版 :821 case 1 —— 抖胸（单行 / 多行两套文案）。</summary>
        private static void BreastJiggle(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            if (player.BreastRows.Count == 1)
            {
                context.Write("Combat.Tease.Case1.YourLiftYourTop".Localized());

                if (player.LustPercent >= 50)
                    context.Write("Combat.Tease.Case1.YourSeemDemandMonster".Localized());
            }

            if (player.BreastRows.Count > 1)
            {
                context.Write("Combat.Tease.Case1.YouLiftYourTop".Localized());

                if (player.LustPercent >= 50)
                    context.Write("Combat.Tease.Case1.YourPainfullyVisible".Localized());
                else
                    context.Write("Combat.Tease.Case1.Line825".Localized());

                flags.ChanceBonus += 1;
            }

            flags.Breasts = true;
        }

        /// <summary>原版 :846 case 2 —— 露下体（半人马有专属文案）。</summary>
        private static void PussyFlash(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            if (player.IsTaur())
            {
                context.Write("Combat.Tease.Case2.YouGallopTowardYour".Localized());

                flags.ChanceBonus += 2;

                flags.DamageBonus += 4;
            }
            else
            {
                context.Write("Combat.Tease.Case2.YouOpenYourRevealing".Localized());

                if (player.Cocks.Count > 0)
                {
                    flags.ChanceBonus += 1;

                    flags.DamageBonus += 1;

                    context.Write(player.Cocks.Count == 1

                        ? player.CockDescript(0)

                        : player.MultiCockDescriptLight());

                    context.Write("Combat.Tease.Case2.And".Localized());

                    if (player.HasPerk(PerkIds.BulgeArmor))
                        flags.DamageBonus += 5;

                    flags.Penis = true;
                }

                context.Write(player.VaginaDescript(0));

                context.Write("Combat.Tease.Case2.Line849".Localized());
            }

            flags.Vagina = true;
        }

        /// <summary>原版 :872 case 3 —— 露鸡巴（半人马有专属文案）。</summary>
        private static void CockFlash(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            if (player.IsTaur() && player.CountCocksOfType("Horse") > 0)
            {
                context.Write("Combat.Tease.Case3.YouLetOutBestial".Localized());

                if (player.HasPerk(PerkIds.BulgeArmor))
                    flags.DamageBonus += 5;
            }
            else
            {
                context.Write("Combat.Tease.Case3.YouOpenYourRevealing".Localized());

                if (player.Cocks.Count == 1)
                    context.Write(player.CockDescript(0));

                if (player.Cocks.Count > 1)
                    context.Write(player.MultiCockDescriptLight());

                if (player.HasVagina())
                    context.Write("Combat.Tease.Case3.And".Localized());

                if (player.HasPerk(PerkIds.BulgeArmor))
                {
                    flags.DamageBonus += 5;

                    flags.ChanceBonus += 1;
                }

                if (player.Vaginas.Count > 0)
                {
                    context.Write(player.VaginaDescript(0));

                    flags.Vagina = true;
                }

                context.Write("Combat.Tease.Case3.Line875".Localized());
            }

            flags.Penis = true;
        }

        /// <summary>原版 :902 case 4 —— 婊子式扭屁股（大屁股额外文案与加成）。</summary>
        private static void BimboButtShake(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var butt = player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating;

            context.Write("Combat.Tease.Case4.YouTurnAwayAnd".Localized());

            if (butt >= 10)
            {
                context.Write("Combat.Tease.Case4.MakingJiggleDelightfullyEven".Localized());

                flags.ChanceBonus += 3;
            }
            else
            {
                context.Write("Combat.Tease.Case4.LettingGetGoodLook".Localized(player.VaginaDescript(0)));

                flags.ChanceBonus += 1;

                flags.Vagina = true;
            }

            flags.Ass = true;

            flags.Anus = true;
        }

        /// <summary>原版 :921 case 5 —— 婊子式抖胸（可分泌乳头有额外加成）。</summary>
        private static void BimboBreastJiggle(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case5.YouLeanForwardLetting".Localized(
                Appearance.AllBreastsDescript(player)));

            context.Write("Combat.Tease.Case5.YouCupThemYour".Localized());

            if (player.LustPercent >= 50)
            {
                if (player.HasFuckableNipples())
                {
                    flags.ChanceBonus += 1;

                    context.Write("Combat.Tease.Case5.ClearSlimeLeaksFrom".Localized());
                }
                else
                    context.Write("Combat.Tease.Case5.YourHardNipplesSeem".Localized());

                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }

            flags.Breasts = true;
        }

        /// <summary>原版 :941 case 6 —— 婊子式露下体（智商型角色有专属开场）。</summary>
        private static void BimboPussyFlash(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var futa = player.HasCock() && player.HasVagina();

            if (player.HasPerk(PerkIds.BimboBrains) || player.HasPerk(PerkIds.FutaFaculties))
                context.Write("Combat.Tease.Case6.YouCoylyOpenYour".Localized());
            else
            {
                context.Write("Combat.Tease.Case6.YouCoylyOpenYourAnd".Localized());

                if (futa)
                    context.Write("Combat.Tease.Case6.Futanari".Localized());
                else if (player.HasPerk(PerkIds.BimboBody))
                    context.Write("Combat.Tease.Case6.Bimbo".Localized());
                else
                    context.Write("Combat.Tease.Case6.Sexy".Localized());

                context.Write("Combat.Tease.Case6.BodyTurnYou".Localized());
            }

            context.Write("Combat.Tease.Case6.GazesAreRivetedYour".Localized());

            if (player.GetClitLength() > 3)
                context.Write("Combat.Tease.Case6.GazeRivetedYourGroin".Localized());

            flags.Vagina = true;
        }

        /// <summary>原版 :977 case 7 —— 弯腰露臀（婊子专用）。</summary>
        private static void BendOverBimbo(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case7.TheGlintingLightCatches".Localized());

            context.Write("Combat.Tease.Case7.TheClearWarmFluid".Localized());

            flags.Vagina = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;
        }

        /// <summary>原版 :987 case 8 —— 胸肌舞（体型越壮加成越高）。</summary>
        private static void PecDance(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case8.YouPlaceYourHands".Localized());

            context.Write(player.HasPerk(PerkIds.BroBrains)

                ? "Combat.Tease.Case8.DamnHasGotLove".Localized()

                : "Combat.Tease.Case8.WillProbablyEnjoyThe".Localized());

            flags.ChanceBonus += (player.Tone - 75) / 5f;

            flags.DamageBonus += (player.Tone - 70) / 5f;

            flags.Auto = false;
        }

        /// <summary>原版 :1000 case 9 —— 英雄姿势。</summary>
        private static void HeroicPose(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case9.CanResistSuchHeroic".Localized());

            context.Write(player.HasPerk(PerkIds.BroBrains)

                ? "Combat.Tease.Case9.LeastThePhysicalChanges".Localized()

                : "Combat.Tease.Case9.YouLeanBackAnd".Localized());

            flags.ChanceBonus += (player.Tone - 75) / 5f;

            flags.DamageBonus += (player.Tone - 70) / 5f;

            flags.Auto = false;
        }

        /// <summary>原版 :1013 case 10 —— 粗俗挺胯。</summary>
        private static void BulgyGroinThrust(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case10.WayCouldMonsterResist".Localized());

            context.Write(player.HasPerk(PerkIds.BroBrains)

                ? "Combat.Tease.Case10.ThisCrudeButThe".Localized()

                : "Combat.Tease.Case10.YouLikeThatHumping".Localized());

            context.Write("Combat.Tease.Case10.Line1016".Localized());

            context.Write("Combat.Tease.Case10.BenefitTryingEnticeThem".Localized());

            context.Write("Combat.Tease.Case10.YouStrikeHerculeanPose".Localized());

            if (player.HasPerk(PerkIds.BulgeArmor))
                flags.DamageBonus += 5;

            flags.Penis = true;
        }

        /// <summary>原版 :1031 case 11 —— 展示鸡巴（silly 模式有专属彩蛋）。</summary>
        private static void ShowOffDick(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            if (Settings.ModeSettings.Instance.Silly && Random.Shared.Next(2) == 0)
                context.Write("Combat.Tease.Case11.BitchesLoveCumLeaking".Localized());
            else
            {
                context.Write("Combat.Tease.Case11.YouGotAdmitYou".Localized());

                context.Write(player.HasPerk(PerkIds.BroBrains)

                    ? "Combat.Tease.Case11.ReachingDownYouGrab".Localized()

                    : "Combat.Tease.Case11.YouLeanBackFeigning".Localized());
            }

            flags.Penis = true;
        }

        /// <summary>原版 :1044 case 12 —— 猫式柔术（体脂影响命中）。</summary>
        private static void CatFlexibility(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case12.ReachingDownYouGrab".Localized());

            flags.Vagina = true;

            if (player.Thickness < 33)
                flags.ChanceBonus += 1;
            else if (player.Thickness >= 66)
                flags.ChanceBonus -= 1;

            flags.DamageBonus += (player.Thickness - 50) / 10f;
        }

        /// <summary>原版 :1056 case 13 —— 孕期姿态（泌乳与妊娠阶段加成分档）。</summary>
        private static void PregnantPose(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case13.YouLeanBackFeigning".Localized());

            if (player.BiggestLactation() >= 1)
            {
                context.Write("Combat.Tease.Case13.YourOtherHandMoves".Localized());

                flags.ChanceBonus += 2;

                flags.DamageBonus += 4;
            }

            if (player.Pregnancy.Incubation < 100)
            {
                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }

            if (player.Pregnancy.Incubation < 50)
            {
                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }

            flags.Vagina = true;
        }

        /// <summary>原版 :1074 case 14 —— 育母（发情期追加加成）。</summary>
        private static void BroodMother(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write(Random.Shared.Next(2) == 0

                ? "Combat.Tease.Case14.YouTearOpenYour".Localized()

                : "Combat.Tease.Case14.YouWiggleYourYour".Localized());

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;

            if (player.InHeat)
            {
                flags.ChanceBonus += 2;

                flags.DamageBonus += 4;
            }

            flags.Vagina = true;
        }

        /// <summary>原版 :1087 case 15 —— 乳穴（三档：有穴且大胸 / 仅大胸 / 仅露胸）。</summary>
        private static void NippleCunts(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var bigTits = player.BiggestBreastSize();

            if (bigTits > 1 && player.HasVagina() && Random.Shared.Next(2) == 0)
            {
                context.Write("Combat.Tease.Case15.ClosingYourEyesYou".Localized(player.ArmorName));

                context.Write("Combat.Tease.Case15.DrawingYourLustSlick".Localized());

                context.Write("Combat.Tease.Case15.DespiteHowQuietYou".Localized());

                flags.ChanceBonus += 2;

                flags.DamageBonus += 4;
            }
            else if (bigTits > 1 && Random.Shared.Next(2) == 0)
            {
                context.Write("Combat.Tease.Case15.YouYankOffThe".Localized(player.ChestDesc()));

                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }
            else
                context.Write("Combat.Tease.Case15.YouRemoveTheFront".Localized());

            flags.Breasts = true;
        }

        /// <summary>原版 :1112 case 16 —— 扒光转身展示后庭。</summary>
        private static void AnalGape(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case16.YouSwingAroundShedding".Localized());

            flags.Anus = true;

            flags.Ass = true;
        }

        /// <summary>原版 :1119 case 17 —— 蜂腹式扭臀。</summary>
        private static void BeeAbdomen(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case17.YouSitDownLike".Localized());

            flags.Ass = true;

            flags.ChanceBonus += 0.5f;

            flags.DamageBonus += 0.5f;
        }

        /// <summary>原版 :1121 case 18 —— 犬/狼式蹲坐（狼分更高时用「狼」的说法）。</summary>
        private static void DogTease(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case18.YouSitDownLike".Localized(
                player.WolfScore() > player.DogScore() ? "Combat.Tease.Case18.PartedCuntLips".Localized() : "Combat.Tease.Case18.PuckeredAssholeHangingErect".Localized()));

            context.Write(player.HasVagina()

                ? "Combat.Tease.Case18.AndYourHandsThe".Localized()

                : "Combat.Tease.Case18.YouMakeSureCapture".Localized());

            context.Write("Combat.Tease.Case18.YourFoeRegardsYou".Localized());

            flags.Vagina = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;
        }

        /// <summary>原版 :1137 case 19 —— 极致女性化。</summary>
        private static void MaxFeminine(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case19.YourFoeRegardsYou".Localized());

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;
        }

        /// <summary>原版 :1144 case 20 —— 极致男性化。</summary>
        private static void MaxMasculine(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case20.YouReachAndRun".Localized());

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;
        }

        /// <summary>原版 :1151 case 21 —— 完美中性（伤害反而 -3）。</summary>
        private static void MaxAndrogyny(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case21.ReachingBackYouMilk".Localized());

            flags.DamageBonus -= 3;
        }

        /// <summary>原版 :1159 case 22 —— 蜘蛛丝。</summary>
        private static void SpiderSilk(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case22.PantingWithYourUnstoppable".Localized());

            flags.Ass = true;
        }

        /// <summary>原版 :1166 case 23 —— 发情期（马鸡巴 ≥12 寸有专属文案）。</summary>
        private static void RutTease(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            if (player.CountCocksOfType("Horse") > 0 && LongestHorseCockLength(player) >= 12)
                context.Write("Combat.Tease.Case23.YouRunYourTongue".Localized());
            else
            {
                context.Write("Combat.Tease.Case23.YouMoveCloseYour".Localized());

                context.Write("Combat.Tease.Case23.SeeingLullTheBattle".Localized());
            }

            flags.Penis = true;
        }

        /// <summary>原版 :1180 case 24 —— 钢管舞（需要法杖）。</summary>
        private static void PoleDance(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case24.SeeingLullTheBattle".Localized());

            flags.Ass = true;

            flags.Breasts = true;
        }

        /// <summary>原版 :1188 case 25 —— 高个子压制式挑逗。</summary>
        private static void TallTease(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case25.YouTurnTheSide".Localized());

            flags.Breasts = true;

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;
        }

        /// <summary>原版 :1196 case 26 —— 魔法挑逗（需 70+ 智力与魅惑法术）。</summary>
        private static void MagicTease(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case26.YouPlayWithThe".Localized());

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;
        }

        /// <summary>原版 :1204 case 27 —— 喂食者（泌乳）。</summary>
        private static void FeederTease(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case27.YouSwayYourBody".Localized());

            flags.Breasts = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 1;
        }

        /// <summary>原版 :1192 case 28 —— 女教师装束。</summary>
        private static void FemaleTeacherCostume(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case28.YouTurnTheSide".Localized());

            flags.Ass = true;
        }

        /// <summary>原版 :1198 case 29 —— 男教师装束。</summary>
        private static void MaleTeacherCostume(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case29.YouPlayWithThe".Localized());

            flags.ChanceBonus += 1;

            flags.DamageBonus += 3;
        }

        /// <summary>原版 :1205 case 30 —— 娜迦恋物装。</summary>
        private static void NagaFetishClothes(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case30.YouRearBackAnd".Localized());

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;
        }

        /// <summary>原版 :1212 case 31 —— 半人马挽具。</summary>
        private static void CentaurHarness(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case31.YouTurnYourBack".Localized());

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;
        }

        /// <summary>原版 :1219 case 32 —— 无性别仆从装。</summary>
        private static void GenderlessServant(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case32.YouSeriesPosesAccentuate".Localized());

            flags.Ass = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;
        }

        /// <summary>原版 :1226 case 33 —— 露裆装。</summary>
        private static void CrotchRevealingClothes(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case33.YouGiveRatherExplicit".Localized());

            flags.ChanceBonus += 2;

            flags.DamageBonus += 4;
        }

        /// <summary>原版 :1233 case 34 —— 女仆装。</summary>
        private static void MaidCostume(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case34.YouBrushAsideYour".Localized());

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;

            flags.Breasts = true;
        }

        /// <summary>原版 :1241 case 35 —— 男仆装（称呼按性别切换）。</summary>
        private static void ServantBoyClothes(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case35.YouPullBackOne".Localized(player.Mf("Master", "Mistress")));

            flags.Penis = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;
        }

        /// <summary>原版 :1249 case 36 —— 束缚病患装。</summary>
        private static void BondagePatientClothes(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case36.YouWetYourLips".Localized());

            flags.DamageBonus += 2;

            flags.ChanceBonus += 1;
        }

        /// <summary>原版 :1247 case 37 —— 狐妖挑逗 A（挑逗性最强的固定加成）。</summary>
        private static void KitsuneTeaseA(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case37.YouPurseYourLips".Localized());

            flags.ChanceBonus += 6;

            flags.DamageBonus += 3;
        }

        /// <summary>原版 :1254 case 38 —— 狐妖挑逗 B。</summary>
        private static void KitsuneTeaseB(CombatContext context, TeaseFlags flags)
        {
            context.Write("Combat.Tease.Case38.LeaningForwardYouBow".Localized());

            flags.Breasts = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 1;
        }

        /// <summary>原版 :1262 case 39 —— 狐妖挑逗 C（尾巴数量影响用词）。</summary>
        private static void KitsuneTeaseC(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var tails = player.Body.Get<BodyParts.Vanilla.TailPart>().Venom;

            context.Write("Combat.Tease.Case39.Line1262".Localized());

            context.Write(tails == 1

                ? "Combat.Tease.Case39.BushyTailCurlsAround".Localized()

                : "Combat.Tease.Case39.TurningAroundYouStare".Localized());

            context.Write("Combat.Tease.Case39.YourTailTwistsAnd".Localized());

            flags.Ass = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 1;
        }

        /// <summary>原版 :1274 case 40 —— 狐妖挑逗 D（单尾/多尾两套文案）。</summary>
        private static void KitsuneTeaseD(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var tails = player.Body.Get<BodyParts.Vanilla.TailPart>().Venom;

            context.Write("Combat.Tease.Case40.YourTailsFanOut".Localized());

            context.Write(tails == 1

                ? "Combat.Tease.Case40.YourComesIntoView".Localized()

                : "Combat.Tease.Case40.SmirkingCoylyYouSway".Localized());

            context.Write(tails > 1

                ? "Combat.Tease.Case40.And".Localized()

                : "Combat.Tease.Case40.Your".Localized());

            flags.Ass = true;

            flags.Anus = true;

            flags.ChanceBonus += 1;

            flags.DamageBonus += 2;
        }

        /// <summary>原版 :1290 case 41 —— 狐妖性别分化挑逗（鸡巴/阴道按性别出现）。</summary>
        private static void KitsuneGendered(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var tails = player.Body.Get<BodyParts.Vanilla.TailPart>().Venom;

            context.Write("Combat.Tease.Case41.Your".Localized());

            if (player.CockTotal() > 0)
                context.Write(player.SMultiCockDesc());

            if ((int)player.Gender == 3)
                context.Write("Combat.Tease.Case41.YourBushyTail".Localized());

            if ((int)player.Gender >= 2)
                context.Write("Combat.Tease.Case41.YouLiftYourSkirt".Localized(player.VaginaDescript(0)));

            context.Write(tails > 1

                ? "Combat.Tease.Case41.YouTurnPartiallyAround".Localized()

                : "Combat.Tease.Case41.YouTuckYourHands".Localized());

            flags.Vagina = true;

            flags.Penis = true;

            flags.DamageBonus += 2;

            flags.ChanceBonus += 1;
        }

        /// <summary>原版 :1318 case 42 —— Urta 挑逗（两种姿态随机）。</summary>
        private static void UrtaTease(CombatContext context, TeaseFlags flags)
        {
            if (Random.Shared.Next(2) == 0)
            {
                context.Write("Combat.Tease.Case42.YouTuckYourHands".Localized());

                flags.Penis = true;

                flags.DamageBonus += 3;

                flags.ChanceBonus -= 1;
            }
            else
            {
                context.Write("Combat.Tease.Case42.MoaningYouBendForward".Localized());

                flags.Ass = true;

                flags.ChanceBonus += 2;

                flags.DamageBonus += 3;
            }
        }

        /// <summary>原版 :1333 case 43 —— 奶牛娘挑逗（7 种随机变体 + 牛头人额外加成）。</summary>
        private static void CowgirlTease(CombatContext context, TeaseFlags flags)
        {
            var cows = Random.Shared.Next(7);

            switch (cows)
            {
                case 0:
                    context.Write("Combat.Tease.Case43.SexThroughTheAir".Localized());

                    flags.Breasts = true;

                    flags.ChanceBonus += 2;

                    flags.DamageBonus += 1;

                    break;

                case 1:
                    context.Write("Combat.Tease.Case43.YouHeftYourBreasts".Localized());

                    flags.Breasts = true;

                    flags.ChanceBonus += 2;

                    flags.DamageBonus += 2;

                    break;

                case 2:
                    context.Write(context.Player.Wetness() >= 3

                        ? "Combat.Tease.Case43.YouLiftMassiveBreast".Localized()

                        : "Combat.Tease.Case43.YouCrouchLowLetting".Localized());

                    flags.Vagina = true;

                    flags.ChanceBonus += 1;

                    flags.DamageBonus += 1;

                    break;

                case 3:
                    context.Write("Combat.Tease.Case43.YouLiftBreastYour".Localized());

                    flags.Breasts = true;

                    flags.ChanceBonus += 3;

                    flags.DamageBonus += 1;

                    break;

                case 4:
                    context.Write("Combat.Tease.Case43.ConfidentlySaunteringForwardYou".Localized());

                    flags.Breasts = true;

                    flags.ChanceBonus += 1;

                    flags.DamageBonus += 3;

                    break;

                case 5:
                    context.Write("Combat.Tease.Case43.YouSkipAndSpin".Localized());

                    flags.Vagina = true;

                    flags.Breasts = true;

                    flags.ChanceBonus += 1;

                    break;

                default:
                    context.Write("Combat.Tease.Case43.YouFlipTheBarely".Localized());

                    flags.Vagina = true;

                    flags.Breasts = true;

                    flags.DamageBonus += 2;

                    break;
            }

            // 原版 :1380：对手是牛头人时额外加成
            if (context.Enemy.ShortName.Contains("minotaur"))
            {
                flags.DamageBonus += 6;

                flags.ChanceBonus += 3;
            }
        }

        /// <summary>原版 :1384 case 44 —— 处女的护甲挑逗（5 种变体，固定 +3 命中/+5 伤害）。</summary>
        private static void MaidenArmorTease(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var maiden = Random.Shared.Next(5);

            flags.DamageBonus += 5;

            flags.ChanceBonus += 3;

            switch (maiden)
            {
                case 0:
                    context.Write("Combat.Tease.Case44.YouSeductivelyWiggleYour".Localized());

                    flags.Breasts = true;

                    break;

                case 1:
                    context.Write("Combat.Tease.Case44.BendingOverYouLift".Localized());

                    flags.Ass = true;

                    flags.Vagina = true;

                    break;

                case 2:
                    context.Write("Combat.Tease.Case44.YouPlaceYourHand".Localized());

                    context.Write(player.Vaginas.Count > 0 && player.Vaginas[0].Virgin

                        ? "Combat.Tease.Case44.YouMoveYourHand".Localized()

                        : "Combat.Tease.Case44.YouMoveYourHandTowards".Localized());

                    context.Write("Combat.Tease.Case44.YouMoveYourHandTowardsYour".Localized());

                    flags.Vagina = true;

                    flags.DamageBonus += 3;

                    break;

                case 3:
                    context.Write("Combat.Tease.Case44.WhoopsSomethingDerpedPlease".Localized());

                    flags.Vagina = true;

                    break;

                default:
                    context.Write("Combat.Tease.Case44.YouLeanBackFeigning".Localized());

                    flags.Ass = true;

                    flags.ChanceBonus += 2;

                    break;
            }
        }

        /// <summary>原版 :1404 case 45 —— 锂辉石护甲（按性别挑选可露部位）。</summary>
        private static void LethiciteArmorTease(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var parts = new List<int> { 0 };

            // 原版 :1407-1417：半人马不能露鸡巴/阴道
            if ((int)player.Gender == 1 && !player.IsTaur())
                parts.Add(1);

            if ((int)player.Gender == 2 && !player.IsTaur())
                parts.Add(2);

            if ((int)player.Gender == 3 && player.HasVagina() && !player.IsTaur())
                parts.Add(3);

            switch (parts[Random.Shared.Next(parts.Count)])
            {
                case 0:
                    context.Write("Combat.Tease.Case45.YouPlaceYourHand".Localized());

                    flags.Breasts = true;

                    flags.ChanceBonus += 3;

                    flags.DamageBonus += 1;

                    break;

                case 1:
                    context.Write("Combat.Tease.Case45.YouMoveYourHand".Localized());

                    flags.Penis = true;

                    flags.ChanceBonus += 1;

                    flags.DamageBonus += 2;

                    break;

                case 2:
                    context.Write("Combat.Tease.Case45.YouMoveYourHandTowards".Localized());

                    flags.Vagina = true;

                    flags.ChanceBonus += 1;

                    flags.DamageBonus += 2;

                    break;

                case 3:
                    context.Write("Combat.Tease.Case45.YouMoveYourHandTowardsYour".Localized());

                    flags.Penis = true;

                    flags.Vagina = true;

                    flags.ChanceBonus += 1;

                    flags.DamageBonus += 3;

                    break;
            }
        }

        /// <summary>原版 :1439 case 46 —— Nephila 王族护甲（泌乳与寄生虫加成）。</summary>
        private static void NephilaRegaliaTease(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            context.Write("Combat.Tease.Case46.YouLeanBackFeigning".Localized());

            if (player.BiggestLactation() >= 1)
            {
                context.Write("Combat.Tease.Case46.YourOtherHandMoves".Localized());

                flags.ChanceBonus += 2;

                flags.DamageBonus += 4;
            }

            var eels = (int)player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0);

            if (eels > 4)
            {
                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }

            if (eels > 9)
            {
                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }
        }

        /// <summary>原版 :1481 case 47 —— Nephila 女王（需 NephilaQueen perk，含虫群与泌乳加成）。</summary>
        private static void NephilaQueenTease(CombatContext context, TeaseFlags flags)
        {
            var player = context.Player;

            var queen = player.HasPerk(PerkIds.NephilaQueen);

            var eels = (int)player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0);

            if (queen)
            {
                context.Write("Combat.Tease.Case47.YouShimmyAndShake".Localized());

                context.Write("Combat.Tease.Case47.NflauntingYourBodySuch".Localized());

                if (player.BiggestLactation() >= 1)
                {
                    context.Write("Combat.Tease.Case47.NifYouKeepExposing".Localized());

                    flags.ChanceBonus += 2;

                    flags.DamageBonus += 6;
                }
            }

            if (queen && eels < 20)
            {
                context.Write("Combat.Tease.Case47.SeemsUnimpressed".Localized());

                flags.ChanceBonus += 1;

                flags.DamageBonus += 2;
            }

            if (eels > 19)
            {
                context.Write("Combat.Tease.Case47.Line1485".Localized());

                flags.ChanceBonus += 1;

                flags.DamageBonus += 4;
            }
        }

        /// <summary>原版缺失的 player.longestHorseCockLength()：取所有马型鸡巴的最大长度。</summary>
        private static float LongestHorseCockLength(Player player)
        {
            var best = 0f;

            foreach (var cock in player.Cocks)
            {
                var cockType = BodyParts.BodyPartSlotRegistry.Get(cock.SlotId)
                    .Types.Get(cock.Id).GetString("type");

                if (string.Equals(cockType, "Horse", StringComparison.OrdinalIgnoreCase) && cock.Length > best)
                    best = cock.Length;
            }

            return best;
        }
    }

    /// <summary>原版 tease() 里用于标记「本次挑逗用到了哪些身体部位」的一组布尔量。</summary>
    public class TeaseFlags
    {
        public bool Breasts;

        public bool Penis;

        public bool Balls;

        public bool Vagina;

        public bool Anus;

        public bool Ass;

        public bool Auto = true;

        public float ChanceBonus;

        public float DamageBonus;
    }
}
