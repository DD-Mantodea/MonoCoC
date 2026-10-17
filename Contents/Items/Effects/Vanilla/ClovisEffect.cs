using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/Clovis.hx:13-144 —— Clovis（绵羊转化药水）。
    /// 属性（智力/韧性/速度/敏感/堕落）、身高/臀部/胸、羊耳/羊尾/羊蹄/羊角/羊毛/羊毛发。
    /// </summary>
    public class ClovisEffect : ConsumableEffect
    {
        // 原版 Clovis.hx:19
        private const string TfSource = "clovis";

        // 原版 Clovis.hx:97 羊毛颜色表
        private static readonly string[] SheepWoolColors = ["white", "black", "gray", "silver", "brown", "moorit"];

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var mutations = Mutations.Mutations.Instance;

            // 原版 :20 mutations.initTransformation([2, 2])
            mutations.InitTransformation([2, 2]);

            context.Write("Items.Clovis.Use".Localized());

            // ── 原版 :23-55 属性变化 ───────────────────────────────────────
            if (player.Intelligence > 90 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.Intelligence = Math.Max(0, player.Intelligence - (random.Next(2) + 1));

                context.Write("Items.Clovis.Inte".Localized());
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.Toughness = Math.Min(100, player.Toughness + random.Next(2) + 1);

                context.Write("Items.Clovis.Tou".Localized());
            }

            if (player.Speed < 75 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.Speed = Math.Min(100, player.Speed + random.Next(2) + 1);

                context.Write("Items.Clovis.Spe".Localized());
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.Sensitivity = Math.Max(0, player.Sensitivity - (random.Next(2) + 1));

                context.Write("Items.Clovis.Sens".Localized());
            }

            if (random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.Corruption = Math.Max(0, player.Corruption - (random.Next(3) + 2));

                context.Write("Items.Clovis.Cor".Localized());

                mutations.Changes += 1;
            }

            // 原版 :44 变矮
            if (player.Tallness > 67 && random.Next(2) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.Tallness -= 1 + random.Next(4);

                context.Write("Items.Clovis.Height".Localized());

                mutations.Changes += 1;
            }

            // 原版 :49 臀部变大
            var butt = player.Body.Get<ButtPart>();

            if (butt.Rating < 6 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                butt.Rating += 1 + random.Next(2);

                if (butt.Rating > 6)
                    butt.Rating = 6;

                context.Write("Items.Clovis.Butt".Localized());
            }

            // 原版 :57 颈部还原
            if (player.Body.Get<NeckPart>().Id != BodyPartTypes.Normal
                && mutations.Changes < mutations.ChangeLimit

                && random.Next(4) == 0)

                mutations.RestoreNeck(TfSource);

            // 原版 :61 后体还原
            if (player.Body.Get<RearBodyPart>().Id != BodyPartTypes.None
                && mutations.Changes < mutations.ChangeLimit

                && random.Next(5) == 0)

                mutations.RestoreRearBody(TfSource);

            // ── 原版 :64 羊耳 ─────────────────────────────────────────────
            var ears = player.Body.Get<EarsPart>();

            if (ears.Id != BodyPartTypes.Sheep && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (ears.Id == BodyPartTypes.None)

                    context.Write("Items.Clovis.EarsNone".Localized()); // 原版 :65 ears.type == -1 遗留值，C# 里对应 none
                else
                    context.Write("Items.Clovis.EarsOther".Localized());

                ears.Id = BodyPartTypes.Sheep;

                context.Write("Items.Clovis.EarsDone".Localized());

                mutations.Changes += 1;
            }

            // ── 原版 :74 羊尾 ─────────────────────────────────────────────
            var tail = player.Body.Get<TailPart>();

            if (tail.Id != BodyPartTypes.Sheep && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Clovis.Tail".Localized());

                tail.Id = BodyPartTypes.Sheep;

                context.Write("Items.Clovis.TailDone".Localized());

                mutations.Changes += 1;
            }

            // ── 原版 :80 羊蹄 ─────────────────────────────────────────────
            var lowerBody = player.Body.Get<LowerBodyPart>();

            if (lowerBody.Id != BodyPartTypes.ClovenHoofed
                && tail.Id == BodyPartTypes.Sheep

                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Clovis.Hooves".Localized());

                lowerBody.Id = BodyPartTypes.ClovenHoofed;

                lowerBody.LegCount = 2;

                context.Write("Items.Clovis.HoovesDone".Localized());

                mutations.Changes += 1;
            }

            // ── 原版 :87 羊角 ─────────────────────────────────────────────
            var horns = player.Body.Get<HornsPart>();

            if (horns.Id != BodyPartTypes.Sheep
                && horns.Id != BodyPartTypes.Ram

                && ears.Id == BodyPartTypes.Sheep

                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                if (horns.Id != BodyPartTypes.None)
                    context.Write("Items.Clovis.HornsCrumble".Localized());

                context.Write("Items.Clovis.HornsGrow".Localized());

                horns.Id = BodyPartTypes.Sheep;

                horns.Value = 1;

                mutations.Changes += 1;
            }

            // ── 原版 :96 羊毛 ─────────────────────────────────────────────
            var skin = player.Body.Get<SkinPart>();

            if (mutations.TfNoFur()
                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit

                && lowerBody.LegCount == 2

                && lowerBody.Id == BodyPartTypes.ClovenHoofed

                && horns.Id == BodyPartTypes.Sheep

                && tail.Id == BodyPartTypes.Sheep

                && ears.Id == BodyPartTypes.Sheep

                && !player.HasWool())
            {
                if (!player.HasFur())
                    context.Write("Items.Clovis.WoolNew".Localized());
                else
                    context.Write("Items.Clovis.WoolFromFur".Localized());

                skin.Id = BodyPartTypes.Wool;

                skin.DescKey = "Skin.Desc.wool";

                SetFurColor(player, skin, SheepWoolColors[random.Next(SheepWoolColors.Length)]);

                mutations.Changes += 1;
            }

            // ── 原版 :108 公羊角 ──────────────────────────────────────────
            if (horns.Id == BodyPartTypes.Sheep
                && player.HasWool()

                && player.Femininity <= 45

                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Clovis.Ram".Localized());

                horns.Id = BodyPartTypes.Ram;

                horns.Value = 2;

                mutations.Changes += 1;
            }

            // ── 原版 :114 角继续变长 ──────────────────────────────────────
            if (horns.Id == BodyPartTypes.Ram
                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Clovis.RamGrow".Localized());

                horns.Id = BodyPartTypes.Ram;

                horns.Value += 1 + random.Next(3);

                mutations.Changes += 1;
            }

            // ── 原版 :120 羊毛发 ──────────────────────────────────────────
            var hair = player.Body.Get<HairPart>();

            if (player.HasWool()
                && hair.Id != BodyPartTypes.Wool

                && player.Femininity >= 65

                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Clovis.HairWool".Localized());

                hair.Id = BodyPartTypes.Wool;

                mutations.Changes += 1;
            }

            // ── 原版 :125 髋部变大 ────────────────────────────────────────
            var hips = player.Body.Get<HipsPart>();

            if (hips.Rating < 10
                && player.Femininity >= 65

                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Clovis.Hips".Localized());

                hips.Rating += random.Next(2) + 1;

                if (hips.Rating > 10)
                    hips.Rating = 10;

                mutations.Changes += 1;
            }

            // ── 原版 :133 胸部变大 ────────────────────────────────────────
            if (player.BreastRows[0].BreastRating < 5
                && player.Femininity >= 65

                && random.Next(3) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                player.BreastRows[0].BreastRating += random.Next(2) + 1;

                if (player.BreastRows[0].BreastRating > 5)
                    player.BreastRows[0].BreastRating = 5;

                context.Write("Items.Clovis.Breasts".Localized());

                mutations.Changes += 1;
            }

            // 原版 :141 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        // 原版 Player.hx setFurColor(colors, UnderBody.FURRY)：
        // underBody.restore() → 设 skin.furColor → copySkinToUnderBody() → underBody.type = FURRY
        private static void SetFurColor(Player player, SkinPart skin, string color)
        {
            var underBody = player.Body.Get<UnderBodyPart>();

            underBody.Restore();

            skin.FurColor = color;

            // 原版 Creature.hx copySkinToUnderBody()：把皮肤属性复制到内层皮肤
            underBody.Skin.Id = skin.Id;

            underBody.Skin.Tone = skin.Tone;

            underBody.Skin.Adj = skin.Adj;

            underBody.Skin.DescKey = skin.DescKey;

            underBody.Skin.FurColor = skin.FurColor;

            underBody.Id = BodyPartTypes.Furry;
        }
    }
}