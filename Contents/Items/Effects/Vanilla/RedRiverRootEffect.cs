using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Configuration;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 RedRiverRoot.hx（红熊猫转化）──────────────────────────────
    public class RedRiverRootEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            player.SlimeFeed();

            mutations.InitTransformation([2, 2, 4]);

            context.Write("Items.RedRiverRoot.Eat".Localized());

            var speed = player.Speed;

            if (speed < 100f + player.AscensionFactor() && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.RedRiverRoot.Speed".Localized());

                if (speed < 50f + player.AscensionFactor())
                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                if (speed < 75f + player.AscensionFactor())
                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                player.ApplyStats(new DynStat(GameStats.Speed, 1));
            }

            var rows = body.GetList<BreastRowPart>();

            if (player.AverageNipplesPerBreast() > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.RedRiverRoot.ExtraNipples".Localized());

                mutations.Changes += 1;

                foreach (var row in rows)
                    row.NipplesPerBreast = 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && rows.Count > 1 && random.Next(3) == 0
                && !(OptionStore.Instance.Get("hyper", 0) > 0))

                mutations.RemoveExtraBreastRow("RedRiverRoot");

            var butt = body.Get<ButtPart>();

            var hips = body.Get<HipsPart>();

            var skin = body.Get<SkinPart>();

            var hair = body.Get<HairPart>();

            if (butt.Rating > 5 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                mutations.Changes += 1;

                butt.Rating -= 1;

                context.Write("Items.RedRiverRoot.ButtShrink".Localized());
            }

            var isFemaleOrHerm = player.Gender is Gender.Female or Gender.Hermaphrodite;

            var isMale = player.Gender == Gender.Male;

            var isMaleOrHerm = player.Gender is Gender.Male or Gender.Hermaphrodite;

            if (isFemaleOrHerm)
            {
                if (player.BiggestBreastSize() > 4 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    foreach (var row in rows)
                        if (row.BreastRating > 4)
                            row.BreastRating -= 1 + random.Next(3);

                    context.Write("Items.RedRiverRoot.BreastsShrink".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                    mutations.Changes += 1;
                }

                var smallestTit = float.MaxValue;

                foreach (var row in rows)
                    smallestTit = Math.Min(smallestTit, row.BreastRating);

                if (smallestTit < 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    foreach (var row in rows)
                        if (row.BreastRating < 2)
                            row.BreastRating += 1;

                    context.Write("Items.RedRiverRoot.BreastsGrow".Localized());

                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                    mutations.Changes += 1;
                }

                if (hips.Rating > 12 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.RedRiverRoot.HipsNarrow".Localized());

                    hips.Rating -= 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (hips.Rating < 6 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.RedRiverRoot.HipsWiden".Localized());

                    hips.Rating += 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (player.NippleLength > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.RedRiverRoot.NipplesShrink".Localized());

                    player.NippleLength /= 2;
                }

                var vaginas = body.GetList<VaginaPart>();

                if (vaginas.Count > 0 && vaginas[0].Wetness < 3 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                {
                    context.Write("Items.RedRiverRoot.VaginaLoosen".Localized());

                    vaginas[0].Wetness += 1;

                    mutations.Changes += 1;
                }

                if (player.Tone < 65 && random.Next(3) == 0)
                    context.Write(player.ModTone(65, 2));

                if (player.Thickness > 35 && random.Next(3) == 0)
                    context.Write(player.ModThickness(35, 5));
            }

            if (isMale)
            {
                if (player.BiggestBreastSize() > 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    foreach (var row in rows)
                        if (row.BreastRating > 2)
                            row.BreastRating -= 1;

                    context.Write("Items.RedRiverRoot.MaleBreastsShrink".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                    mutations.Changes += 1;
                }

                if (player.NippleLength > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.RedRiverRoot.NipplesShrink".Localized());

                    player.NippleLength /= 2;
                }

                if (hips.Rating > 10 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.RedRiverRoot.HipsNarrow".Localized());

                    hips.Rating -= 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (hips.Rating < 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.RedRiverRoot.HipsWiden".Localized());

                    hips.Rating += 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (player.Tone < 70 && random.Next(3) == 0)
                    context.Write(player.ModTone(65, 2));

                if (player.Thickness > 35 && random.Next(3) == 0)
                    context.Write(player.ModThickness(35, 5));
            }

            var cocks = body.GetList<CockPart>();

            if (isMaleOrHerm)
            {
                if (player.HasCock() && cocks.Count > 0 && cocks[0].Id.Path != "red_panda"
                    && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    context.Write("Items.RedRiverRoot.RedPandaCock".Localized());

                    cocks[0].Id = new Identifier("coc", "red_panda");

                    mutations.Changes += 1;
                }

                if (player.ShortestCockLength() < 6 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    var increment = player.IncreaseCock(player.SmallestCockIndex(), 1 + random.Next(2));

                    context.Write("Items.RedRiverRoot.CockLonger".Localized(increment));

                    mutations.Changes += 1;
                }

                if (player.BiggestCockLength() > 16 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    var index = player.BiggestCockIndex();

                    context.Write("Items.RedRiverRoot.CockShorter".Localized());

                    cocks[index].Length -= (random.Next(10) + 5) / 10f;

                    if (cocks[index].Thickness > 3)
                    {
                        context.Write("Items.RedRiverRoot.CockThinner".Localized(player.CockDescript(index)));

                        cocks[index].Thickness -= (random.Next(4) + 1) / 10f;
                    }

                    mutations.Changes += 1;
                }

                if (player.SmallestCockArea() < 10 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    context.Write(cocks.Count > 1

                        ? "Items.RedRiverRoot.CockThickerMany".Localized()

                        : "Items.RedRiverRoot.CockThickerOne".Localized());

                    var thinnest = 0;

                    for (var index = 1; index < cocks.Count; index++)
                        if (cocks[index].Thickness < cocks[thinnest].Thickness)
                            thinnest = index;

                    cocks[thinnest].Thicken(1.5f);

                    mutations.Changes += 1;
                }
            }

            if (cocks.Count > 1 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                player.RemoveCock(1, 1);

                context.Write("Items.RedRiverRoot.ExtraCockGone".Localized());

                mutations.Changes += 1;
            }

            if ((player.Balls > 0 || player.HasStatusEffect(StatusEffectIds.Uniball))
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)

                if (player.BallSize > 2)
                {
                    player.BallSize -= 1;

                    context.Write(player.Balls == 1 || player.HasStatusEffect(StatusEffectIds.Uniball)

                        ? "Items.RedRiverRoot.BallsSmallerOne".Localized()

                        : "Items.RedRiverRoot.BallsSmallerMany".Localized());

                    mutations.Changes += 1;
                }
                else if (player.Balls > 2)
                {
                    player.Balls = 2;

                    if (player.HasStatusEffect(StatusEffectIds.Uniball))
                        player.RemoveStatusEffect(StatusEffectIds.Uniball);

                    context.Write("Items.RedRiverRoot.ExtraBallsFused".Localized());

                    mutations.Changes += 1;
                }
                else if (player.Balls == 1 || player.HasStatusEffect(StatusEffectIds.Uniball))
                {
                    player.Balls = 2;

                    if (player.HasStatusEffect(StatusEffectIds.Uniball))
                        player.RemoveStatusEffect(StatusEffectIds.Uniball);

                    context.Write("Items.RedRiverRoot.UniballSplit".Localized());

                    mutations.Changes += 1;
                }

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("RedRiverRoot");

            var ears = body.Get<EarsPart>();

            var antennae = body.Get<AntennaePart>();

            var eyes = body.Get<EyesPart>();

            var face = body.Get<FacePart>();

            var arms = body.Get<ArmsPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var tail = body.Get<TailPart>();

            if (ears.Id.Path != "red_panda" && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.RedRiverRoot.Ears".Localized());

                ears.Id = new Identifier("coc", "red_panda");

                mutations.Changes += 1;
            }

            if (antennae.Id.Path != "none" && antennae.Id.Path != "cockatrice"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.RedRiverRoot.Antennae".Localized());

                antennae.Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            if (eyes.Count > 2 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.RedRiverRoot.Eyes".Localized());

                eyes.Restore();

                mutations.Changes += 1;
            }

            // 头发
            var hasPandaHairColor = Array.IndexOf(ColorLists.RedPandaHair, hair.Color) != -1;

            var hasNormalHair = hair.Id.Path == "normal";

            var oldHairType = hair.Id.Path;

            if ((!hasNormalHair || hair.Length == 0 || !hasPandaHairColor)
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                hair.Id = new Identifier("coc", "normal");

                if (!hasPandaHairColor)
                    hair.Color = ColorLists.RedPandaHair[random.Next(ColorLists.RedPandaHair.Length)];

                if (hair.Length == 0)
                {
                    hair.Length = 1;

                    context.Write("Items.RedRiverRoot.HairBald".Localized());
                }
                else if (hasNormalHair && !hasPandaHairColor)
                    context.Write("Items.RedRiverRoot.HairRecolor".Localized());
                else
                    switch (oldHairType)
                    {
                        case "feather":
                            context.Write("Items.RedRiverRoot.HairFeather".Localized());

                            break;

                        case "goo":
                            hair.Length = 1;

                            context.Write("Items.RedRiverRoot.HairGoo1".Localized());

                            context.Write("Items.RedRiverRoot.HairGoo2".Localized());

                            break;

                        default:
                            context.Write("Items.RedRiverRoot.HairDefault".Localized());

                            break;
                    }

                flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && face.Id.Path != "red_panda" && ears.Id.Path == "red_panda"
                && player.HasFur() && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.RedRiverRoot.Face1".Localized());

                context.Write("Items.RedRiverRoot.Face2".Localized());

                face.Id = new Identifier("coc", "red_panda");

                mutations.Changes += 1;
            }

            if (arms.Id.Path != "red_panda" && ears.Id.Path == "red_panda" && tail.Id.Path == "red_panda"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.RedRiverRoot.Arms".Localized());

                arms.Id = new Identifier("coc", "red_panda");

                arms.UpdateClaws(new Identifier("coc", "red_panda"));
            }

            if (lowerBody.Id.Path != "red_panda" && arms.Id.Path == "red_panda"
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                if (player.IsTaur())
                    context.Write("Items.RedRiverRoot.LegsTaur".Localized());
                else if (player.HasTailInsteadOfLegs())
                    context.Write("Items.RedRiverRoot.LegsTail".Localized());
                else if (player.IsGoo())
                {
                    context.Write("Items.RedRiverRoot.LegsGoo1".Localized());

                    context.Write("Items.RedRiverRoot.LegsGoo2".Localized());
                }
                else
                    context.Write("Items.RedRiverRoot.LegsHuman".Localized());

                lowerBody.Id = new Identifier("coc", "red_panda");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            if (tail.Id.Path != "red_panda" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                if (player.HasMultiTails())
                {
                    context.Write("Items.RedRiverRoot.TailMulti1".Localized());

                    context.Write("Items.RedRiverRoot.TailMulti2".Localized());

                    context.Write("Items.RedRiverRoot.TailMulti3".Localized());
                }
                else if (tail.Id.Path == "none")
                {
                    context.Write("Items.RedRiverRoot.TailNone1".Localized());

                    context.Write("Items.RedRiverRoot.TailNone2".Localized());

                    context.Write("Items.RedRiverRoot.TailNone3".Localized());
                }
                else if (tail.Id.Path is "bee_abdomen" or "spider_abdomen")
                {
                    context.Write("Items.RedRiverRoot.TailInsect1".Localized());

                    context.Write("Items.RedRiverRoot.TailInsect2".Localized());

                    context.Write("Items.RedRiverRoot.TailInsect3".Localized());

                    context.Write("Items.RedRiverRoot.TailInsect4".Localized());
                }
                else
                {
                    context.Write("Items.RedRiverRoot.TailOther1".Localized());

                    context.Write("Items.RedRiverRoot.TailOther2".Localized());
                }

                tail.Restore();

                tail.Id = new Identifier("coc", "red_panda");

                mutations.Changes += 1;
            }

            var underBody = body.Get<UnderBodyPart>();

            if (skin.Id.Path == "fur" && underBody.Id != BodyPartTypes.Furry
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.RedRiverRoot.FurFix".Localized());

                skin.Id = new Identifier("coc", "fur");

                skin.Adj = "";

                skin.DescKey = "Skin.Desc.fur";

                skin.FurColor = "russet";

                underBody.Id = BodyPartTypes.Furry;

                underBody.Skin.Id = skin.Id;

                underBody.Skin.Tone = skin.Tone;

                underBody.Skin.Adj = skin.Adj;

                underBody.Skin.DescKey = skin.DescKey;

                underBody.Skin.FurColor = "black";

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && skin.Id.Path != "fur" && arms.Id.Path == "red_panda"
                && lowerBody.Id.Path == "red_panda"

                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                var rubbery = skin.Adj is "latex" or "rubber";

                if (player.HasPlainSkin())
                    if (!rubbery)
                        context.Write("Items.RedRiverRoot.SkinItch".Localized());
                    else
                    {
                        context.Write("Items.RedRiverRoot.SkinRubber1".Localized());

                        context.Write("Items.RedRiverRoot.SkinRubber2".Localized());

                        context.Write("Items.RedRiverRoot.SkinRubber3".Localized());
                    }

                if (player.HasScales())
                {
                    context.Write("Items.RedRiverRoot.SkinScales1".Localized());

                    context.Write("Items.RedRiverRoot.SkinScales2".Localized());

                    context.Write("Items.RedRiverRoot.SkinScales3".Localized());
                }

                var gooSkin = skin.Id.Path == "goo";

                if (gooSkin)
                {
                    context.Write("Items.RedRiverRoot.SkinGoo1".Localized());

                    context.Write("Items.RedRiverRoot.SkinGoo2".Localized());

                    context.Write("Items.RedRiverRoot.SkinGoo3".Localized());
                }

                context.Write("Items.RedRiverRoot.SkinFurGrow".Localized());

                context.Write("Items.RedRiverRoot.SkinFurDone".Localized());

                skin.Id = new Identifier("coc", "fur");

                skin.Adj = "";

                skin.DescKey = "Skin.Desc.fur";

                skin.FurColor = "russet";

                underBody.Id = BodyPartTypes.Furry;

                underBody.Skin.Id = skin.Id;

                underBody.Skin.Tone = skin.Tone;

                underBody.Skin.Adj = skin.Adj;

                underBody.Skin.DescKey = skin.DescKey;

                underBody.Skin.FurColor = "black";

                mutations.Changes += 1;
            }

            if (mutations.Changes == 0)
                if (random.Next(100) == 0)
                    context.Write("Items.RedRiverRoot.Nothing".Localized());
                else
                {
                    context.Write("Items.RedRiverRoot.Invigorated".Localized());

                    player.HealthChange(250);

                    player.ApplyStats(new DynStat(GameStats.Lust, 3));
                }

            player.RefillHunger(20);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }
}