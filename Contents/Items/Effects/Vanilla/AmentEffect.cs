using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/Ament.hx:14-175 —— Ament（鹿/羚羊化 + 「变得更像人」）。
    /// 属性削弱、战斗中退烧止血、掉毛掉鳞去甲壳、双腿还原、山羊皮/树皮、柳叶翼、羚羊角与偶蹄。
    /// </summary>
    public class AmentEffect : ConsumableEffect
    {
        // 原版 Ament.hx:98 / :102 —— restoreArms/restoreLegs 的来源标识
        private const string TfSource = "Ament";

        // 原版 Ament.hx:120 随机换色表
        private static readonly string[] HumanTones = ["pale", "light", "white"];

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var mutations = Mutations.Mutations.Instance;

            // 原版 :21 dynStats(Cor(3))
            player.Corruption = Math.Clamp(player.Corruption + 3, 0, 100);

            context.Write("Items.Ament.Use".Localized());

            var statChanges = false;

            // 原版 :26 力量下降
            if (random.Next(3) == 0)
            {
                player.Strength = Math.Max(0, player.Strength - 3);

                statChanges = true;
            }

            // 原版 :32 敏感度与韧性下降
            if (random.Next(3) == 0)
            {
                player.Sensitivity = Math.Max(0, player.Sensitivity - 2);

                player.Toughness = Math.Max(0, player.Toughness - 3);

                statChanges = true;
            }

            // 原版 :39 性欲与淫欲下降，并结束发情/发情期
            if (random.Next(3) == 0)
            {
                player.Libido = Math.Max(0, player.Libido - 2);

                player.Lust = Math.Max(0, player.Lust - 10);

                if (player.InHeat)
                {
                    player.RemoveStatusEffect(StatusEffectIds.Heat);

                    context.Write("Items.Ament.HeatEnd".Localized());
                }

                if (player.InRut)
                {
                    player.RemoveStatusEffect(StatusEffectIds.Rut);

                    context.Write("Items.Ament.RutEnd".Localized());
                }

                statChanges = true;
            }

            // 原版 :54 速度下降
            if (random.Next(3) == 0)
            {
                player.Speed = Math.Max(0, player.Speed - 3);

                statChanges = true;
            }

            // 原版 :60 体型显著变瘦
            if (random.Next(3) == 0)
            {
                player.Thickness -= 20;

                if (player.Thickness < 1)
                    player.Thickness = 1;

                statChanges = true;
            }

            // 原版 :67 肌肉略减
            if (random.Next(3) == 0)
            {
                player.Tone -= 5;

                if (player.Tone < 1)
                    player.Tone = 1;

                statChanges = true;
            }

            if (statChanges)
                context.Write("Items.Ament.Weak".Localized());

            // 原版 :78 战斗中必定降欲并止血
            if (CombatController.InCombat)
            {
                player.Lust = Math.Max(0, player.Lust - 25);

                context.Write("Items.Ament.CombatCold".Localized());

                if (player.IsBleeding())
                    context.Write("Items.Ament.CombatBleed".Localized());

                player.RemoveStatusEffect(StatusEffectIds.IzmaBleed);

                context.Write("Items.Ament.CombatDot".Localized());
            }

            // 原版 :86
            mutations.InitTransformation([1, 2, 3]);

            var bodyChanges = false;

            // 原版 :90 掉毛
            if (mutations.Changes < mutations.ChangeLimit && player.HasFur() && random.Next(5) == 0)
            {
                mutations.RemoveFur();

                bodyChanges = true;
            }

            // 原版 :93 掉鳞
            if (mutations.Changes < mutations.ChangeLimit && player.HasScales() && random.Next(5) == 0)
            {
                mutations.RemoveScales();

                bodyChanges = true;
            }

            // 原版 :96 去掉手臂/腿上的甲壳
            if (mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)
            {
                var arms = player.Body.Get<ArmsPart>();

                if (arms.Id == BodyPartTypes.Bee || arms.Id == BodyPartTypes.Spider)
                {
                    mutations.RestoreArms(TfSource);

                    bodyChanges = true;
                }

                var lower = player.Body.Get<LowerBodyPart>();

                if (lower.Id == BodyPartTypes.ChitinousSpiderLegs || lower.Id == BodyPartTypes.Bee)
                {
                    mutations.RestoreLegs(TfSource);

                    bodyChanges = true;
                }
            }

            var lowerBody = player.Body.Get<LowerBodyPart>();

            // 原版 :108 单腿还原成人类双腿
            if (mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0 && lowerBody.LegCount == 1)
            {
                lowerBody.Id = BodyPartTypes.Human;

                lowerBody.LegCount = 2;

                context.Write("Items.Ament.HumanLegs".Localized());

                bodyChanges = true;

                mutations.Changes += 1;
            }

            // 原版 :117 黏液皮肤还原（单腿时不处理）
            var skin = player.Body.Get<SkinPart>();

            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(5) == 0

                && player.HasGooSkin()

                && lowerBody.LegCount != 1)
            {
                skin.Id = BodyPartTypes.Plain;

                if (!ColorLists.HumanSkin.Contains(skin.Tone))
                    skin.Tone = HumanTones[random.Next(HumanTones.Length)];

                player.Body.Get<UnderBodyPart>().Restore();

                context.Write("Items.Ament.GooSkin".Localized());

                bodyChanges = true;

                mutations.Changes += 1;
            }

            // 原版 :129 半人马/蛛身后体还原（几率更低）
            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(10) == 0

                && lowerBody.Id == BodyPartTypes.Drider)
            {
                lowerBody.Id = BodyPartTypes.Human;

                lowerBody.LegCount = 2;

                context.Write("Items.Ament.HumanLegs".Localized());

                bodyChanges = true;

                mutations.Changes += 1;
            }

            if (bodyChanges)
                context.Write("Items.Ament.MoreHuman".Localized());

            // 原版 :141 柳叶翼（树皮皮肤 + 树精分数高时概率更高）
            var wings = player.Body.Get<WingsPart>();
            // 原版 Creature.hx hasBarkSkin()：[WOODEN, STALK] 之一
            var hasBarkSkin = skin.Id == BodyPartTypes.Wooden || skin.Id == BodyPartTypes.Stalk;

            var targetWingType = hasBarkSkin ? BodyPartTypes.Willow : BodyPartTypes.Weeping;

            var chanceIn = hasBarkSkin && player.DryadScore() >= 3 ? 2 : 5;

            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(chanceIn) == 0

                && wings.Id != targetWingType)
            {
                var weeping = targetWingType == BodyPartTypes.Weeping;

                var hasWings = wings.Id != BodyPartTypes.None;

                // 原版 :145 的 [if (hasWings)] / [if (weeping)] 四种组合，C# 侧直接选文本
                context.Write((hasWings, weeping) switch
                {
                    (true, true) => "Items.Ament.Wings.HasWingsWeeping".Localized(),

                    (true, false) => "Items.Ament.Wings.HasWingsBursting".Localized(),

                    (false, true) => "Items.Ament.Wings.NoWingsWeeping".Localized(),

                    _ => "Items.Ament.Wings.NoWingsBursting".Localized(),
                });

                wings.Id = targetWingType;

                mutations.Changes += 1;
            }

            // 原版 :151 普通皮肤转为灰白
            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(5) == 0

                && skin.Id == BodyPartTypes.Plain

                && skin.Adj != "ashen")
            {
                context.Write(GameSettings.Metric

                    ? "Items.Ament.AshenMetric".Localized()

                    : "Items.Ament.AshenImperial".Localized());

                skin.Adj = "ashen";

                mutations.Changes += 1;
            }

            // 原版 :158 羚羊角
            var horns = player.Body.Get<HornsPart>();

            if (mutations.Changes < mutations.ChangeLimit
                && random.Next(5) == 0

                && horns.Id != BodyPartTypes.Antelope)
            {
                context.Write(horns.Id != BodyPartTypes.None

                    ? "Items.Ament.HornsHasHorns".Localized()

                    : "Items.Ament.HornsNoHorns".Localized());

                horns.Id = BodyPartTypes.Antelope;

                mutations.Changes += 1;
            }

            // 原版 :165 偶蹄（已经是蹄类则不处理）
            if (mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0 && !IsHoofed(lowerBody))
            {
                lowerBody.Id = BodyPartTypes.ClovenHoofed;

                context.Write(player.IsTaur()

                    ? "Items.Ament.HoovesTaur".Localized()

                    : "Items.Ament.Hooves".Localized());

                mutations.Changes += 1;
            }

            // 原版 :172
            player.RefillHunger(-10);

            // 原版 :173 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        // 原版 Creature.hx isHoofed()：[HOOFED, CLOVEN_HOOFED, PONY] 之一
        private static bool IsHoofed(LowerBodyPart lowerBody) =>

            lowerBody.Id == BodyPartTypes.Hoofed

            || lowerBody.Id == BodyPartTypes.ClovenHoofed

            || lowerBody.Id == BodyPartTypes.Pony;
    }
}