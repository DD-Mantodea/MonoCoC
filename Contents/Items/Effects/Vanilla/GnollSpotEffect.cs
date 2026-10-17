using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/GnollSpot.hx:15-334 —— Gnoll Spot（鬣狗化巧克力豆）。
    /// 属性/体型、肤色、耳/尾/腿/臂/脸/毛、鳃与多余眼睛，以及按性别分派的生殖器转化子函数。
    /// </summary>
    public class GnollSpotEffect : ConsumableEffect
    {
        // 原版 GnollSpot.hx:25
        private const string TfSource = "gnollSpot";

        // 原版 :237 直接传了变量名字符串 "tfSource"，导致来源标识丢失；这里传真实来源
        private const string BreastRowSource = TfSource;

        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;
            // 命名空间同名的类：必须写全路径
            var mutations = Mutations.Mutations.Instance;

            mutations.InitTransformation([2, 3, 3]);

            // 原版 :28
            var gnoll = player.GnollScore() > 3;

            context.Write(gnoll

                ? "Items.GnollSpot.UseGnoll".Localized()

                : "Items.GnollSpot.Use".Localized());

            // 原版 :31 速度
            if (ChangeRoll(random, mutations) && player.Speed < 100)
            {
                context.Write("Items.GnollSpot.Speed".Localized());

                player.Speed = Math.Min(100, player.Speed + 1);
            }

            // 原版 :36 淫欲
            if (ChangeRoll(random, mutations) && player.Libido < 50)
            {
                context.Write("Items.GnollSpot.Libido".Localized());

                player.Libido = Math.Min(100, player.Libido + 2);
            }

            // 原版 :41 肌肉
            if (ChangeRoll(random, mutations)
                && ((player.IsFemale() || player.IsHerm()) && player.Tone < 80

                    || player.IsMale() && player.Tone < 25))
            {
                context.Write("Items.GnollSpot.Tone".Localized());

                player.Tone += 5;
            }

            // 原版 :46 体型
            if (ChangeRoll(random, mutations) && player.Thickness < 25)
            {
                context.Write("Items.GnollSpot.Thickness".Localized());

                player.Thickness += 5;
            }

            // 原版 :51 淫荡（50%）
            if (random.NextDouble() * 100 < 50)
            {
                context.Write("Items.GnollSpot.Lust".Localized());

                player.Lust = Math.Min(100, player.Lust + 15);
            }

            var butt = player.Body.Get<ButtPart>();

            // 原版 :56 女/雌雄同体臀部缩小
            if (ChangeRoll(random, mutations) && butt.Rating > 6 && (player.IsFemale() || player.IsHerm()))
            {
                context.Write("Items.GnollSpot.ButtFemale".Localized());

                butt.Rating -= 2;
            }

            // 原版 :60 男性臀部缩小
            if (ChangeRoll(random, mutations) && butt.Rating > 10 && player.IsMale())
            {
                context.Write("Items.GnollSpot.ButtMale".Localized());

                butt.Rating -= 2;
            }

            // 原版 :65 髋部缩小
            var hips = player.Body.Get<HipsPart>();

            if (ChangeRoll(random, mutations) && hips.Rating > 8)
            {
                context.Write("Items.GnollSpot.Hips".Localized());

                hips.Rating -= 2;
            }

            // 原版 :70 身高向 60 英寸靠拢
            if (ChangeRoll(random, mutations) && player.Tallness != 60)
                if (player.Tallness > 60)
                {
                    context.Write("Items.GnollSpot.Shorter".Localized());

                    player.Tallness -= 1;
                }
                else
                {
                    context.Write("Items.GnollSpot.Taller".Localized());

                    player.Tallness += 1;
                }

            var skin = player.Body.Get<SkinPart>();

            // 原版 :81-102 肤色（75% 黑 / 25% 暗棕）
            if (ChangeRoll(random, mutations))
            {
                var choice = random.NextDouble() * 100 < 75 ? "black" : "dusky brown";

                if (player.IsGoo())
                {
                    context.Write("Items.GnollSpot.GooIntro".Localized());

                    if (!ColorLists.GnollSkin.Contains(skin.Tone))
                    {
                        context.Write("Items.GnollSpot.GooColor".Localized(choice));

                        skin.Tone = choice;
                    }
                    else
                        context.Write("Items.GnollSpot.GooBody".Localized());

                    context.Write("Items.GnollSpot.GooLegs".Localized());

                    // 原版 Player.hx Skin.restore(keepTone=true)：保留刚设的肤色
                    var keptTone = skin.Tone;

                    skin.Restore();

                    skin.Tone = keptTone;

                    mutations.Changes += 1;
                }
                else if (!ColorLists.GnollSkin.Contains(skin.Tone))
                {
                    context.Write("Items.GnollSpot.Splotch".Localized(choice));

                    skin.Tone = choice;

                    mutations.Changes += 1;
                }
            }

            var ears = player.Body.Get<EarsPart>();

            // 原版 :104 鬣狗耳
            if (ChangeRoll(random, mutations) && ears.Id != BodyPartTypes.Gnoll)
            {
                // 修复原版 :105 的 indexOf(...) == 1 缺陷：原意是「人类或精灵耳」，实际只匹配了人类
                if (ears.Id == BodyPartTypes.Human || ears.Id == BodyPartTypes.Elfin)
                    context.Write("Items.GnollSpot.EarsMigrate".Localized());
                else
                    context.Write("Items.GnollSpot.EarsWarm".Localized());

                // 原版 :110 的 [if (hasfurryears)] 两分支
                context.Write(HasFurryEars(ears)

                    ? "Items.GnollSpot.EarsFurry".Localized()

                    : "Items.GnollSpot.EarsPlain".Localized());

                ears.Id = BodyPartTypes.Gnoll;

                mutations.Changes += 1;
            }

            var tail = player.Body.Get<TailPart>();

            // 原版 :115 鬣狗尾
            if (ChangeRoll(random, mutations) && tail.Id != BodyPartTypes.Gnoll)
            {
                if (tail.Id != BodyPartTypes.None)
                    context.Write("Items.GnollSpot.TailMorph".Localized());
                else
                    context.Write("Items.GnollSpot.TailNew".Localized());

                tail.Id = BodyPartTypes.Gnoll;

                mutations.Changes += 1;
            }

            // 原版 :125 必须已有鬣狗尾 + 耳才继续
            var lowerBody = player.Body.Get<LowerBodyPart>();

            var arms = player.Body.Get<ArmsPart>();

            if (tail.Id == BodyPartTypes.Gnoll && ears.Id == BodyPartTypes.Gnoll)
            {
                // 原版 :126 鬣狗腿
                if (ChangeRoll(random, mutations) && lowerBody.Id != BodyPartTypes.Gnoll)
                {
                    context.Write("Items.GnollSpot.Legs".Localized());

                    lowerBody.Id = BodyPartTypes.Gnoll;

                    mutations.Changes += 1;
                }

                // 原版 :132 鬣狗手臂
                if (ChangeRoll(random, mutations) && arms.Id != BodyPartTypes.Gnoll)
                {
                    context.Write("Items.GnollSpot.Arms".Localized());

                    arms.Id = BodyPartTypes.Gnoll;

                    mutations.Changes += 1;
                }

                var face = player.Body.Get<FacePart>();

                if (arms.Id == BodyPartTypes.Gnoll && lowerBody.Id == BodyPartTypes.Gnoll)
                {
                    // 原版 :139 鬣狗脸
                    if (ChangeRoll(random, mutations) && face.Id != BodyPartTypes.Gnoll)
                    {
                        context.Write("Items.GnollSpot.Face".Localized());

                        face.Id = BodyPartTypes.Gnoll;

                        mutations.Changes += 1;
                    }

                    // 原版 :145 鬣狗毛皮
                    if (ChangeRoll(random, mutations)
                        && skin.Id != BodyPartTypes.Fur

                        && face.Id == BodyPartTypes.Gnoll)
                    {
                        context.Write("Items.GnollSpot.SkinFur".Localized());

                        skin.Id = BodyPartTypes.Fur;

                        skin.FurColor = skin.Tone;

                        mutations.Changes += 1;
                    }
                }
            }

            // 原版 :154 去掉鳃（50%）
            if (ChangeRoll(random, mutations, 50) && player.HasGills())
            {
                context.Write("Items.GnollSpot.Gills".Localized());

                player.Body.Get<GillsPart>().Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            // 原版 :160 去掉多余的眼睛（50%）
            var eyes = player.Body.Get<EyesPart>();

            if (ChangeRoll(random, mutations, 50) && eyes.Count > 2)
            {
                context.Write("Items.GnollSpot.ExtraEyes".Localized());

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            // 原版 :166-203 按性别分派
            if (player.IsMale())
            {
                TfGrowGnollCock(context, player, random, mutations);

                TfCumChange(context, player, random, mutations);

                TfKnotGrow(context, player, random, mutations);

                TfCockSizeChange(context, player, random, mutations, 3, 8);

                if (ChangeRoll(random, mutations) && player.HasBalls() && player.BallSize > 1)
                {
                    context.Write("Items.GnollSpot.BallsShrink".Localized());

                    player.BallSize -= 0.5f;

                    if (player.BallSize <= 1)
                    {
                        context.Write("Items.GnollSpot.Uniball".Localized());

                        player.CreateStatusEffect(StatusEffectIds.Uniball);
                    }

                    mutations.Changes += 1;
                }

                TfBreastChange(context, player, random, mutations, "B", "B");

                TfRemoveBreastRows(player, random, mutations);

                TfTurnNipplesBlack(context, player, random, mutations);

                TfCockRemoval(context, player, random, mutations);
            }
            else if (player.IsFemale())
            {
                TfTurnVaginaBlack(context, player, random, mutations);

                TfTurnNipplesBlack(context, player, random, mutations);

                TfBreastChange(context, player, random, mutations, "F", "D");

                TfRemoveBreastRows(player, random, mutations);

                TfClitIncrease(context, player, random, mutations);
            }
            else if (player.IsHerm())
            {
                TfTurnVaginaBlack(context, player, random, mutations);

                TfGrowGnollCock(context, player, random, mutations);

                TfCumChange(context, player, random, mutations);

                TfKnotGrow(context, player, random, mutations);

                TfCockSizeChange(context, player, random, mutations, 12, 15);

                TfBreastChange(context, player, random, mutations, "E", "D");

                TfRemoveBreastRows(player, random, mutations);

                TfTurnNipplesBlack(context, player, random, mutations);

                TfCockRemoval(context, player, random, mutations);
            }

            // 原版 :206 flags[KFLAGS.TIMES_TRANSFORMED] += changes
            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        // 原版 :20 changeRoll(chance = 33.333)
        private static bool ChangeRoll(Random random, Mutations.Mutations mutations, double chance = 33.333) =>

            mutations.Changes < mutations.ChangeLimit && random.NextDouble() * 100 < chance;

        // 原版 Creature.hx hasFurryEars()
        private static bool HasFurryEars(EarsPart ears) =>

            ears.Id == BodyPartTypes.Horse

            || ears.Id == BodyPartTypes.Cow

            || ears.Id == BodyPartTypes.Dog

            || ears.Id == BodyPartTypes.Cat

            || ears.Id == BodyPartTypes.Bunny

            || ears.Id == BodyPartTypes.Kangaroo

            || ears.Id == BodyPartTypes.Fox

            || ears.Id == BodyPartTypes.Raccoon

            || ears.Id == BodyPartTypes.Mouse

            || ears.Id == BodyPartTypes.Ferret

            || ears.Id == BodyPartTypes.Echidna

            || ears.Id == BodyPartTypes.Deer

            || ears.Id == BodyPartTypes.Sheep

            || ears.Id == BodyPartTypes.RedPanda

            || ears.Id == BodyPartTypes.Gnoll;

        // 原版 Appearance.hx breastCupInverse()
        private static int CupInverse(string name)
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            if (name == "flat")
                return 0;

            var big = name[^1] == '+';

            if (big)
                name = name[..^1];

            for (var index = 0; index < Appearance.BreastCupNames.Length; index++)
            {
                if (Appearance.BreastCupNames[index] == name)
                    return index;

                if (Appearance.BreastCupNames[index].StartsWith(name))
                    return index + (big ? 1 : 0);
            }

            return 0;
        }

        private static bool HasCockNotOfType(Player player, Identifier type) =>

            player.Cocks.Any(cock => cock.Id != type);

        private static int FindFirstCockNotOfType(Player player, Identifier type)
        {
            for (var index = 0; index < player.Cocks.Count; index++)
                if (player.Cocks[index].Id != type)
                    return index;

            return -1;
        }

        // 原版 :211 tfClitIncrease()
        private static void TfClitIncrease(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (ChangeRoll(random, mutations) && player.GetClitLength() < 8)
            {
                player.ChangeClitLength(1);

                context.Write("Items.GnollSpot.Clit".Localized());

                mutations.Changes += 1;
            }
        }

        // 原版 :219 tfTurnNipplesBlack()
        private static void TfTurnNipplesBlack(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (ChangeRoll(random, mutations) && !player.HasStatusEffect(StatusEffectIds.BlackNipples))
            {
                player.CreateStatusEffect(StatusEffectIds.BlackNipples);

                context.Write("Items.GnollSpot.NipplesBlack".Localized());

                mutations.Changes += 1;
            }
        }

        // 原版 :227 tfTurnVaginaBlack()
        private static void TfTurnVaginaBlack(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (ChangeRoll(random, mutations)
                && player.Vaginas.Count > 0

                && player.Vaginas[0].Id.Path != "black_sand_trap")
            {
                context.Write("Items.GnollSpot.VaginaBlack".Localized());

                player.Vaginas[0].Id = new Identifier("coc", "black_sand_trap");

                mutations.Changes += 1;
            }
        }

        // 原版 :235 tfRemoveBreastRows()
        private static void TfRemoveBreastRows(Player player, Random random, Mutations.Mutations mutations)
        {
            if (ChangeRoll(random, mutations) && player.BreastRows.Count > 1)
                mutations.RemoveExtraBreastRow(BreastRowSource);
        }

        // 原版 :241 tfGrowGnollCock()
        private static void TfGrowGnollCock(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (!ChangeRoll(random, mutations) || !HasCockNotOfType(player, BodyPartTypes.Gnoll))
                return;

            var index = FindFirstCockNotOfType(player, BodyPartTypes.Gnoll);

            // 原版 :244 用 [cock N]（1 起始）描述
            context.Write("Items.GnollSpot.CockGrow1".Localized(index + 1));

            if (!player.Cocks[index].SheathOn)
                context.Write("Items.GnollSpot.CockGrowSheath".Localized());

            context.Write("Items.GnollSpot.CockGrow2".Localized());

            player.Cocks[index].Id = BodyPartTypes.Gnoll;

            player.Cocks[index].ThickenCock(1.5f);

            player.Cocks[index].KnotMultiplier = 1.5f;

            mutations.Changes += 1;
        }

        // 原版 :256 tfCumChange()
        private static void TfCumChange(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (ChangeRoll(random, mutations, 50) && player.HasCock() && player.CumMultiplier < 10)
            {
                context.Write(player.HasBalls()

                    ? "Items.GnollSpot.CumSack".Localized()

                    : "Items.GnollSpot.CumGut".Localized());

                player.CumMultiplier += 1;

                mutations.Changes += 1;
            }
        }

        // 原版 :269 tfKnotGrow()（注意：原版先掷 50%，命中则直接返回）
        private static void TfKnotGrow(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (random.NextDouble() * 100 < 50)
                return;

            var knots = player.Cocks
                .Where(cock => cock.KnotMultiplier > 1 && cock.KnotMultiplier < 3)
                .ToList();

            if (knots.Count == 0)
                return;

            var chosen = knots[random.Next(knots.Count)];

            var index = player.Cocks.IndexOf(chosen) + 1;

            context.Write(chosen.SheathOn

                ? "Items.GnollSpot.KnotGrowSheath".Localized(index)

                : "Items.GnollSpot.KnotGrow".Localized(index));

            chosen.KnotMultiplier = Math.Min(3, chosen.KnotMultiplier + 0.5f);

            mutations.Changes += 1;
        }

        // 原版 :291 tfCockRemoval()
        private static void TfCockRemoval(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations)
        {
            if (!ChangeRoll(random, mutations) || player.Cocks.Count <= 1)
                return;

            var smallest = player.SmallestCockIndex();

            if (player.Cocks[smallest].Length <= 4)
            {
                context.Write("Items.GnollSpot.CockRemove".Localized(
                    smallest + 1,

                    Utils.Utils.NumberOfThings(player.Cocks.Count - 1, "cock.", "cocks.")));

                player.RemoveCock(smallest, 1);
            }
            else
            {
                player.Cocks[smallest].Length -= 2;

                context.Write("Items.GnollSpot.CockShorten".Localized(smallest + 1));
            }
        }

        // 原版 :304 tfCockSizeChange(min, max)
        private static void TfCockSizeChange(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations, int min, int max)
        {
            if (player.Cocks.Count != 1)
                return;

            if (ChangeRoll(random, mutations) && player.Cocks[0].Length < min)
            {
                context.Write("Items.GnollSpot.CockSizeUp".Localized());

                player.IncreaseCock(0, 1);

                mutations.Changes += 1;
            }

            if (ChangeRoll(random, mutations) && player.Cocks[0].Length > max)
            {
                context.Write("Items.GnollSpot.CockSizeDown".Localized());

                player.Cocks[0].Length -= 2;

                mutations.Changes += 1;
            }
        }

        // 原版 :321 tfBreastChange(maxSize, minSize)
        private static void TfBreastChange(ItemUseContext context, Player player, Random random, Mutations.Mutations mutations, string maxSize, string minSize)
        {
            if (!ChangeRoll(random, mutations))
                return;

            var row = player.BiggestTitRow();

            if (player.BiggestBreastSize() > CupInverse(maxSize))
            {
                player.BreastRows[row].BreastRating -= 1;

                context.Write("Items.GnollSpot.BreastShrink".Localized(player.BreastCup(row)));

                mutations.Changes += 1;
            }
            else if (player.BiggestBreastSize() != CupInverse(minSize))
            {
                player.BreastRows[row].BreastRating += 1;

                context.Write("Items.GnollSpot.BreastGrow".Localized(player.BreastCup(row)));

                mutations.Changes += 1;
            }
        }
    }
}
