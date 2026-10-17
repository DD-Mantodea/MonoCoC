using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/TrapOil.hx:14-355 —— 陷阱油（蚁狮/中性化方向）。
    /// 速度/力量、敏感/淫欲、体型、髋/胸、阴茎与睾丸缩小、肛门湿润、生育力、雌雄化、黑眼/黑阴/蜻蜓翼。
    /// </summary>
    public class TrapOilEffect : ConsumableEffect
    {
        // 原版 TrapOil.hx:20
        private const string TfSource = "trapOil";

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var mutations = Mutations.Mutations.Instance;

            mutations.InitTransformation([2, 3, 3]);

            // 原版 :23-29 开场（按堕落度三选一）
            context.Write(player.Corruption < 30

                ? "Items.TrapOil.UseHesitant".Localized()

                : player.Corruption > 70

                    ? "Items.TrapOil.UseEager".Localized()

                    : "Items.TrapOil.Use".Localized());

            // 原版 :32 速度（注意 :37 是 else-if：速度分支没触发才可能掉力量）
            if (player.Speed < 100 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Speed".Localized());

                player.Speed = Math.Min(100, player.Speed + 1);
            }
            else if (player.Strength > 40 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Strength".Localized());

                player.Strength = Math.Max(0, player.Strength - 1);
            }

            // 原版 :42 敏感度
            if (player.Sensitivity < 70 && player.HasCock() && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Sensitivity".Localized());

                player.Sensitivity = Math.Min(100, player.Sensitivity + 5);
            }

            // 原版 :47 淫欲（<30 时额外 +2）
            if (player.Libido < 70 && player.HasVagina() && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Libido".Localized());

                player.Libido = Math.Min(100, player.Libido + 2);

                if (player.Libido < 30)
                    player.Libido = Math.Min(100, player.Libido + 2);
            }

            // 原版 :55 体型变瘦
            if (player.Thickness > 40 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Thickness".Localized());

                player.ModThickness(40, 3);

                mutations.Changes += 1;
            }

            var hips = player.Body.Get<HipsPart>();

            // 原版 :62 髋部减少
            if (hips.Rating >= 10 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.HipsLoss".Localized());

                hips.Rating -= 1;

                if (hips.Rating > 15)
                    hips.Rating -= 2 + random.Next(3);

                mutations.Changes += 1;
            }

            // 原版 :71 髋部增加
            if (hips.Rating < 6 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.HipsGain".Localized());

                hips.Rating += 1;

                mutations.Changes += 1;
            }

            // 原版 :77 胸部减少（全部往 A 杯压）
            if (player.BiggestBreastSize() > 1 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.BreastLoss".Localized());

                for (var row = 0; row < player.BreastRows.Count; row++)
                {
                    var rating = player.BreastRows[row].BreastRating;

                    if (rating > 70)
                        player.BreastRows[row].BreastRating -= random.Next(3) + 15;
                    else if (rating > 50)
                        player.BreastRows[row].BreastRating -= random.Next(3) + 10;
                    else if (rating > 30)
                        player.BreastRows[row].BreastRating -= random.Next(3) + 7;
                    else if (rating > 15)
                        player.BreastRows[row].BreastRating -= random.Next(3) + 4;
                    else
                        player.BreastRows[row].BreastRating -= 2 + random.Next(2);

                    if (player.BreastRows[row].BreastRating < 1)
                        player.BreastRows[row].BreastRating = 1;
                }

                mutations.Changes += 1;
            }

            // 原版 :100 胸部增加到 A 杯
            // 修复原版运算符优先级缺陷：原文 `a || b && c && d` 会让「完全平胸」无视概率和变化上限必触发
            if ((player.BiggestBreastSize() < 1 || player.BreastRows[0].BreastRating < 1)
                && random.Next(4) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.BreastGain".Localized());

                player.BreastRows[0].BreastRating = 1;

                for (var row = 1; row < player.BreastRows.Count; row++)
                    if (player.BreastRows[row].BreastRating < 1)
                        player.BreastRows[row].BreastRating = 1;

                mutations.Changes += 1;
            }

            // 原版 :115 阴茎向 3.5 英寸缩
            if (player.LongestCockLength() >= 3.5 && player.HasCock() && random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write(player.CockTotal() == 1

                    ? "Items.TrapOil.CockShrinkSingle".Localized()

                    : "Items.TrapOil.CockShrinkMulti".Localized());

                for (var index = 0; index < player.Cocks.Count; index++)
                {
                    var cock = player.Cocks[index];

                    if (cock.Length >= 3.5)
                    {
                        if (cock.Length < 6 && cock.Length >= 2.9)
                        {
                            cock.Length -= 0.5f;

                            if (cock.Thickness * 6 > cock.Length)
                                cock.Thickness -= 0.2f;

                            if (cock.Thickness * 8 > cock.Length)
                                cock.Thickness -= 0.2f;

                            if (cock.Thickness < 0.5f)
                                cock.Thickness = 0.5f;
                        }

                        cock.Length -= 0.5f;

                        // 原版 Math.fround(len * 0.33) * -1
                        player.IncreaseCock(index, cock.Length * 0.33f * -1f);
                    }
                }

                mutations.Changes += 1;
            }

            // 原版 :163 睾丸缩小
            if (player.Balls > 0
                && player.HasCock()

                && (player.BallSize > 1 || !player.HasStatusEffect(StatusEffectIds.Uniball))

                && random.Next(4) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Balls".Localized());

                player.BallSize -= 1;

                if (player.BallSize > 8)
                    player.BallSize -= 1;

                if (player.BallSize > 10)
                    player.BallSize -= 1;

                if (player.BallSize > 12)
                    player.BallSize -= 1;

                if (player.BallSize > 15)
                    player.BallSize -= 1;

                if (player.BallSize > 20)
                    player.BallSize -= 1;

                // 原版 :182 缩到 1 以下 → 独球
                if (player.BallSize < 1 && !player.HasStatusEffect(StatusEffectIds.Uniball))
                {
                    context.Write("Items.TrapOil.Uniball".Localized());

                    player.CreateStatusEffect(StatusEffectIds.Uniball);
                }

                if (player.BallSize < 0.5f)
                    player.BallSize = 0.5f;

                mutations.Changes += 1;
            }

            // 原版 :193 肛门湿润度
            if (player.Anal.Wetness < 5 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write(player.Anal.Wetness < 4

                    ? "Items.TrapOil.AnalWet".Localized()

                    : "Items.TrapOil.AnalWetFinal".Localized());

                player.Anal.Wetness += 1;

                if (player.Anal.Looseness < 3)
                    player.Anal.Looseness += 1;

                mutations.Changes += 1;

                player.Sensitivity = Math.Min(100, player.Sensitivity + 2);
            }

            // 原版 :209 生育力下降
            if (player.HasVagina() && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.FertilityIntro".Localized());

                player.Sensitivity = Math.Max(0, player.Sensitivity - 2);

                if (player.Fertility >= 30)
                    context.Write("Items.TrapOil.FertilityHigh".Localized());
                else if (player.Fertility >= 5)
                    context.Write("Items.TrapOil.FertilityAverage".Localized());
                else
                {
                    context.Write("Items.TrapOil.FertilityLow".Localized());

                    if (player.Fertility > 0)
                        context.Write("Items.TrapOil.FertilityMostly".Localized());

                    context.Write("Items.TrapOil.FertilitySterile".Localized());

                    // 原版 :226 低生育力 + 堕落 ≥70
                    if (player.IsCorruptEnough(70))
                        context.Write("Items.TrapOil.FertilityCorrupt".Localized());
                }

                player.Fertility -= 1 + random.Next(3);

                if (player.Fertility < 4)
                    player.Fertility = 4;

                mutations.Changes += 1;
            }

            // 原版 :237 男性效果
            if (player.Gender == Gender.Male)
            {
                if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)

                    if (player.Femininity < 70 && player.Femininity >= 60)
                    {
                        context.Write("Items.TrapOil.FemFinal".Localized());

                        if (!player.HasPerk(PerkIds.Androgyny))
                        {
                            player.CreatePerk(PerkIds.Androgyny);

                            context.Write("Items.TrapOil.PerkGained".Localized());
                        }

                        player.Femininity += 10;

                        if (player.Femininity > 70)
                            player.Femininity = 70;

                        mutations.Changes += 1;
                    }
                    else
                    {
                        context.Write("Items.TrapOil.FemNormal".Localized());

                        player.Femininity += 10;

                        mutations.Changes += 1;
                    } // 原版 :240 女性化到上限 70，并给 Androgyny

                // 原版 :260 肌肉减少
                if (player.Tone > 20 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    context.Write("Items.TrapOil.ToneLoss".Localized());

                    player.Tone -= 10;

                    mutations.Changes += 1;
                }
            }
            // 原版 :267 女性效果
            else if (player.Gender == Gender.Female)
            {
                // 原版 :269 男性化
                if (player.Femininity > 30 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    player.Femininity -= 10;

                    if (player.Femininity < 30)
                    {
                        player.Femininity = 30;

                        context.Write("Items.TrapOil.MascFinal".Localized());

                        if (!player.HasPerk(PerkIds.Androgyny))
                        {
                            player.CreatePerk(PerkIds.Androgyny);

                            context.Write("Items.TrapOil.PerkGained".Localized());
                        }
                    }
                    else
                        context.Write("Items.TrapOil.MascNormal".Localized());

                    mutations.Changes += 1;
                }

                // 原版 :285 肌肉增加
                if (player.Tone < 80 && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    context.Write("Items.TrapOil.ToneGain".Localized());

                    player.Tone += 10;

                    mutations.Changes += 1;
                }
            }

            var neck = player.Body.Get<NeckPart>();

            var rearBody = player.Body.Get<RearBodyPart>();

            // 原版 :292 颈部还原
            if (neck.Id != BodyPartTypes.Normal && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck(TfSource);

            // 原版 :296 后体还原（非鲨鱼鳍）
            if (rearBody.Id != BodyPartTypes.None
                && rearBody.Id != BodyPartTypes.SharkFin

                && mutations.Changes < mutations.ChangeLimit

                && random.Next(5) == 0)

                mutations.RestoreRearBody(TfSource);

            // 原版 :300 产卵 perk 流失
            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk(TfSource);

            // 原版 :304 乳头变黑
            if (!player.HasStatusEffect(StatusEffectIds.BlackNipples)
                && random.Next(6) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.NipplesBlack".Localized());

                player.CreateStatusEffect(StatusEffectIds.BlackNipples);

                mutations.Changes += 1;
            }

            var eyes = player.Body.Get<EyesPart>();

            // 原版 :310 去掉蜘蛛眼
            if ((eyes.Id == BodyPartTypes.FourSpiderEyes || eyes.Id == BodyPartTypes.Spider)
                && random.Next(2) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.SpiderEyes".Localized());

                eyes.Id = BodyPartTypes.Human;

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            // 原版 :317 黑沙陷阱眼
            if (eyes.Id != BodyPartTypes.BlackEyesSandTrap
                && random.Next(4) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                eyes.Id = BodyPartTypes.BlackEyesSandTrap;

                context.Write("Items.TrapOil.BlackEyes".Localized());

                mutations.Changes += 1;
            }

            // 原版 :324 阴道变黑
            if (player.HasVagina()
                && player.Vaginas[0].Id.Path != "black_sand_trap"

                && random.Next(4) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.VaginaBlack".Localized());

                if (player.Wetness() >= 3)
                    context.Write("Items.TrapOil.VaginaWet".Localized());

                context.Write(player.Corruption < 50

                    ? "Items.TrapOil.VaginaCorLow".Localized()

                    : "Items.TrapOil.VaginaCorHigh".Localized());

                context.Write("Items.TrapOil.VaginaDone".Localized());

                player.Sensitivity = Math.Min(100, player.Sensitivity + 2);

                player.Lust = Math.Min(100, player.Lust + 10);

                player.Vaginas[0].Id = new Identifier("coc", "black_sand_trap");

                mutations.Changes += 1;
            }

            // 原版 :342 巨型蜻蜓翼
            var wings = player.Body.Get<WingsPart>();

            if (wings.Id != BodyPartTypes.GiantDragonfly
                && random.Next(4) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TrapOil.Wings".Localized());

                mutations.Changes += 1;

                wings.Id = BodyPartTypes.GiantDragonfly;
            }

            // 原版 :348 什么都没变
            if (mutations.Changes == 0)
                context.Write("Items.TrapOil.Failsafe".Localized());

            // 原版 :351 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}