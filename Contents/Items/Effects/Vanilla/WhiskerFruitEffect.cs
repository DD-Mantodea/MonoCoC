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
    // ── 原版 WhiskerFruit.hx（猫咪转化）─────────────────────────────────
    public class WhiskerFruitEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            var tfSource = "catTransformation";

            if (player.HasReptileScales() && body.Get<WingsPart>().Id.Path is "draconic_small" or "draconic_large"
                && body.Get<TonguePart>().Id.Path == "draconic")

                tfSource = "catTransformation-dragonne";

            mutations.InitTransformation([2, 2, 3]);

            context.Write("Items.WhiskerFruit.Eat".Localized());

            var spe100 = 100f * player.Speed / player.MaxValue(GameStats.Speed);

            var str100 = 100f * player.Strength / player.MaxValue(GameStats.Strength);

            var tou100 = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            var inte100 = 100f * player.Intelligence / player.MaxValue(GameStats.Intelligence);

            var lib100 = player.LibidoPercent;

            if (spe100 < 75 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                if (spe100 <= 30)
                {
                    context.Write("Items.WhiskerFruit.SpeedLow".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 2));
                }
                else if (spe100 <= 60)
                {
                    context.Write("Items.WhiskerFruit.SpeedMid".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 1));
                }
                else
                {
                    context.Write("Items.WhiskerFruit.SpeedHigh".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 0.5f));
                }

            if (str100 < 40 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write(random.Next(2) == 0

                    ? "Items.WhiskerFruit.Strength1".Localized()

                    : "Items.WhiskerFruit.Strength2".Localized());

                player.ApplyStats(new DynStat(GameStats.Strength, 1));
            }
            else if (str100 > 60 && random.Next(2) == 0)
            {
                context.Write("Items.WhiskerFruit.StrengthLoss".Localized());

                player.ApplyStats(new DynStat(GameStats.Strength, -2));
            }

            if (tou100 > 50 && random.Next(2) == 0)
            {
                context.Write("Items.WhiskerFruit.ToughnessLoss".Localized());

                player.ApplyStats(new DynStat(GameStats.Toughness, -2));
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (inte100 < 15)
                    context.Write("Items.WhiskerFruit.IntelligenceLow".Localized());
                else if (inte100 < 50)
                {
                    var branch = random.Next(2) == 0;

                    if (branch)
                    {
                        var temp = random.Next(3);

                        context.Write("Items.WhiskerFruit.IntelligenceMid1".Localized() +

                            (temp switch
                            {
                                0 => "Items.WhiskerFruit.IntelligenceMidBall".Localized(),

                                1 => "Items.WhiskerFruit.IntelligenceMidYarn".Localized(),

                                _ => "Items.WhiskerFruit.IntelligenceMidNap".Localized(),
                            }));
                    }
                    else
                        context.Write("Items.WhiskerFruit.IntelligenceMid2".Localized());
                }
                else
                    context.Write("Items.WhiskerFruit.IntelligenceHigh".Localized());

                player.ApplyStats(new DynStat(GameStats.Intelligence, -1));
            }

            var cocks = body.GetList<CockPart>();

            if (lib100 < 80 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                var catCock = -1;

                for (var index = 0; index < cocks.Count; index++)
                    if (cocks[index].Id.Path == "cat")
                    {
                        catCock = index;

                        break;
                    }

                if (catCock >= 0)
                {
                    context.Write("Items.WhiskerFruit.LibidoCatCock1".Localized(player.CockDescript(catCock)));

                    if (player.Corruption < 33)
                        context.Write("Items.WhiskerFruit.LibidoCatCockLow".Localized());
                    else if (player.Corruption < 66)
                        context.Write("Items.WhiskerFruit.LibidoCatCockMid".Localized());
                    else
                        context.Write("Items.WhiskerFruit.LibidoCatCockHigh".Localized());
                }
                else
                {
                    context.Write("Items.WhiskerFruit.LibidoGeneric1".Localized());

                    if (player.LustPercent > 60)
                        context.Write("Items.WhiskerFruit.LibidoGenericMore".Localized());

                    context.Write("Items.WhiskerFruit.LibidoGeneric2".Localized());
                }

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Sensitivity, 0.25f));
            }

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                var intensified = player.InHeat;

                if (player.GoIntoHeat(false))
                {
                    if (intensified)
                        context.Write(random.Next(2) == 0

                            ? "Items.WhiskerFruit.HeatStronger1".Localized(player.VaginaDescript(0))

                            : "Items.WhiskerFruit.HeatStronger2".Localized(player.VaginaDescript(0)));
                    else
                    {
                        context.Write("Items.WhiskerFruit.HeatStart1".Localized(player.VaginaDescript(0)));

                        if (player.HasFur())
                            context.Write("Items.WhiskerFruit.HeatStartFur".Localized());

                        context.Write("Items.WhiskerFruit.HeatStart2".Localized());

                        context.Write(player.Corruption < 50

                            ? "Items.WhiskerFruit.HeatStartAssault".Localized()

                            : "Items.WhiskerFruit.HeatStartFill".Localized());

                        context.Write("Items.WhiskerFruit.HeatStart3".Localized(player.VaginaDescript(0)));
                    }

                    mutations.Changes += 1;
                }
            }

            var rows = body.GetList<BreastRowPart>();

            if (mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0
                && !(OptionStore.Instance.Get("hyper", 0) > 0))
            {
                var threshold = 0f;

                if (!player.HasVagina() && player.BiggestBreastSize() > 2)
                    threshold = 2;
                else if (player.BiggestBreastSize() > 4)
                    threshold = 4;

                if (threshold > 0)
                {
                    var changedRows = 0;

                    for (var index = 0; index < rows.Count; index++)
                    {
                        if (rows[index].BreastRating <= threshold)
                            continue;

                        if (rows[index].BreastRating > 10)
                        {
                            rows[index].BreastRating -= 2 + random.Next(3);

                            context.Write(changedRows == 0

                                ? "Items.WhiskerFruit.BreastsBigShrink".Localized()

                                : "Items.WhiskerFruit.BreastsRowShrinkBig".Localized(Utils.Utils.Num2TextOrdinal(index + 1)));
                        }
                        else
                        {
                            rows[index].BreastRating -= 1;

                            context.Write(changedRows == 0

                                ? "Items.WhiskerFruit.BreastsShrink".Localized(player.BreastDescript(index))

                                : "Items.WhiskerFruit.BreastsRowShrink".Localized(Utils.Utils.Num2TextOrdinal(index + 1), player.BreastDescript(index)));
                        }

                        changedRows += 1;
                    }

                    if (changedRows > 0)
                        mutations.Changes += 1;
                }
            }

            // 猫丁丁
            if (cocks.Count > 0)
            {
                var catCount = player.CountCocksOfType("Cat");

                if (catCount < cocks.Count
                    && (body.Get<EarsPart>().Id.Path == "cat" || random.Next(3) > 0)

                    && (body.Get<TailPart>().Id.Path == "cat" || random.Next(3) > 0)

                    && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                {
                    var index = 0;

                    while (index < cocks.Count && cocks[index].Id.Path == "cat")

                        index += 1;

                    if (index < cocks.Count)
                    {
                        context.Write("Items.WhiskerFruit.CatCock1".Localized(player.CockDescript(index)));

                        if (!cocks[index].SheathOn)
                        {
                            context.Write("Items.WhiskerFruit.CatCockSheath".Localized());

                            if (player.Balls > 0)
                                context.Write("Items.WhiskerFruit.CatCockBallsSafe".Localized());
                        }
                        else
                            context.Write("Items.WhiskerFruit.CatCockIntoSheath".Localized());

                        cocks[index].Id = new Identifier("coc", "cat");

                        cocks[index].SheathOn = true;

                        mutations.Changes += 1;
                    }
                }
            }

            // 猫丁丁缩小
            if (player.CountCocksOfType("Cat") > 0 && random.Next(3) == 0
                && mutations.Changes < mutations.ChangeLimit

                && !(OptionStore.Instance.Get("hyper", 0) > 0))
            {
                var found = -1;

                for (var index = 0; index < cocks.Count; index++)
                    if (cocks[index].Id.Path == "cat" && cocks[index].Length > 6)
                    {
                        found = index;

                        break;
                    }

                if (found >= 0)
                {
                    if (cocks[found].Length > 16)
                    {
                        context.Write("Items.WhiskerFruit.CatCockShrinkBig".Localized(player.CockDescript(found)));

                        cocks[found].Length *= 0.66f;
                    }
                    else
                    {
                        context.Write("Items.WhiskerFruit.CatCockShrinkSmall".Localized(player.CockDescript(found)));

                        cocks[found].Length -= 2;
                    }

                    if (cocks[found].Length / 5 < cocks[found].Thickness && cocks[found].Thickness > 1.25f)
                        cocks[found].Thickness = cocks[found].Length / 6;

                    var others = false;

                    for (var index = found + 1; index < cocks.Count; index++)
                    {
                        if (cocks[index].Id.Path != "cat")
                            continue;

                        if (cocks[index].Length > 16)
                            cocks[index].Length *= 0.66f;
                        else if (cocks[index].Length > 6)
                            cocks[index].Length -= 2;

                        if (cocks[index].Length / 5 < cocks[index].Thickness && cocks[index].Thickness > 1.25f)
                            cocks[index].Thickness = cocks[index].Length / 6;

                        if (cocks[index].Length > 6)
                            others = true;
                    }

                    context.Write("Items.WhiskerFruit.CatCockSensitive".Localized());

                    player.ApplyStats(new DynStat(GameStats.Sensitivity, 5));

                    if (others)
                        context.Write("Items.WhiskerFruit.CatCockOthersShrunk".Localized());

                    mutations.Changes += 1;
                }
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck(tfSource);

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody(tfSource);

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk(tfSource);

            var face = body.Get<FacePart>();

            var tongue = body.Get<TonguePart>();

            var ears = body.Get<EarsPart>();

            var tail = body.Get<TailPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var arms = body.Get<ArmsPart>();

            var skin = body.Get<SkinPart>();

            var eyes = body.Get<EyesPart>();

            if (mutations.TfNoFur() && face.Id.Path == "catgirl" && tongue.Id.Path == "cat"
                && ears.Id.Path == "cat" && tail.Id.Path == "cat" && lowerBody.Id.Path == "cat"

                && arms.Id.Path == "cat" && player.HasFur() && random.Next(5) == 0

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WhiskerFruit.CatFaceFromCatgirl1".Localized());

                context.Write("Items.WhiskerFruit.CatFaceFromCatgirl2".Localized());

                face.Id = new Identifier("coc", "cat");

                mutations.Changes += 1;
            }

            if (ears.Id.Path != "cat" && random.Next(5) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                if (ears.Id.Path == "human")
                    context.Write(random.Next(2) == 0

                        ? "Items.WhiskerFruit.EarsHuman1".Localized()

                        : "Items.WhiskerFruit.EarsHuman2".Localized());
                else
                    context.Write(random.Next(2) == 0

                        ? "Items.WhiskerFruit.EarsOther1".Localized()

                        : "Items.WhiskerFruit.EarsOther2".Localized());

                ears.Id = new Identifier("coc", "cat");

                mutations.Changes += 1;
            }

            if (tail.Id.Path != "cat" && ears.Id.Path == "cat" && random.Next(5) == 0
                && mutations.Changes < mutations.ChangeLimit)
            {
                if (tail.Id.Path == "none")
                    context.Write(random.Next(3) switch
                    {
                        0 => "Items.WhiskerFruit.TailNone1".Localized(),

                        1 => "Items.WhiskerFruit.TailNone2".Localized(),

                        _ => "Items.WhiskerFruit.TailNone3".Localized(),
                    });
                else
                    context.Write("Items.WhiskerFruit.TailOther".Localized());

                tail.Id = new Identifier("coc", "cat");

                mutations.Changes += 1;
            }

            if (tail.Id.Path == "cat" && ears.Id.Path == "cat" && random.Next(5) == 0
                && mutations.Changes < mutations.ChangeLimit && lowerBody.Id.Path != "cat")
            {
                if (lowerBody.Id.Path == "hoofed")
                {
                    context.Write("Items.WhiskerFruit.LegsHoof".Localized());

                    if (player.IsTaur())
                        context.Write("Items.WhiskerFruit.LegsHoofTaur".Localized());
                }
                else if (lowerBody.Id.Path == "goo")
                    context.Write("Items.WhiskerFruit.LegsGoo".Localized());
                else
                    context.Write("Items.WhiskerFruit.LegsOther".Localized());

                lowerBody.Id = new Identifier("coc", "cat");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && tail.Id.Path == "cat" && ears.Id.Path == "cat"
                && random.Next(5) == 0 && mutations.Changes < mutations.ChangeLimit

                && lowerBody.Id.Path == "cat" && !player.HasFur())
            {
                context.Write("Items.WhiskerFruit.FurGrow1".Localized());

                skin.Id = new Identifier("coc", "fur");

                skin.DescKey = "Skin.Desc.fur";

                SetCatFur(player, skin, body.Get<UnderBodyPart>(), random);

                context.Write("Items.WhiskerFruit.FurGrow2".Localized());

                mutations.Changes += 1;
            }

            if (player.HasCatFace() && !(body.Get<EyesPart>().Id.Path == "cat") && random.Next(3) == 0
                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WhiskerFruit.CatEyes".Localized());

                context.Write("Items.WhiskerFruit.CatEyesEnd".Localized());

                eyes.Id = new Identifier("coc", "cat");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && tail.Id.Path == "cat" && ears.Id.Path == "cat"
                && lowerBody.Id.Path == "cat" && !player.HasCatFace()

                && random.Next(5) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WhiskerFruit.CatFace".Localized());

                if (!(body.Get<EyesPart>().Id.Path == "cat"))
                    context.Write("Items.WhiskerFruit.CatFaceEyes".Localized());

                context.Write("Items.WhiskerFruit.CatFaceEnd".Localized());

                face.Id = new Identifier("coc", "catgirl");

                mutations.Changes += 1;
            }

            if (player.HasCatFace() && tongue.Id.Path != "cat" && random.Next(5) == 0
                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WhiskerFruit.CatTongue".Localized());

                tongue.Id = new Identifier("coc", "cat");

                mutations.Changes += 1;
            }

            if (arms.Id.Path != "cat" && player.HasFur() && tail.Id.Path == "cat"
                && lowerBody.Id.Path == "cat" && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                var underBody = body.Get<UnderBodyPart>();

                var furColor = underBody.Id == BodyPartTypes.Furry ? underBody.Skin.FurColor : skin.FurColor;

                context.Write("Items.WhiskerFruit.Arms".Localized(furColor));

                arms.Id = new Identifier("coc", "cat");

                arms.UpdateClaws(new Identifier("coc", "cat"));
            }

            if (random.Next(4) == 0 && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            if (mutations.Changes == 0)
            {
                context.Write("Items.WhiskerFruit.FailSafe".Localized());

                player.HealthChange(50);

                player.ApplyStats(new DynStat(GameStats.Lust, 3));
            }

            if (mutations.Changes < mutations.ChangeLimit)
            {
                if (random.Next(2) == 0)
                    context.Write(player.ModThickness(5, 2));

                if (random.Next(2) == 0)
                    context.Write(player.ModTone(76, 2));

                if (player.Gender is Gender.Male or Gender.Female)
                    context.Write(random.Next(2) == 0

                        ? player.ModFemGoal(65, 1)

                        : player.ModFemGoal(85, 2));
            }

            player.RefillHunger(20);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        // 原版 Player.hx setFurColor(颜色表, UnderBody.FURRY)
        private static void SetCatFur(Player player, SkinPart skin, UnderBodyPart underBody, Random random)
        {
            var (Upper, Under) = ColorLists.CatFur[random.Next(ColorLists.CatFur.Length)];

            underBody.Restore();

            skin.FurColor = Upper;

            underBody.Skin.Id = skin.Id;

            underBody.Skin.Tone = skin.Tone;

            underBody.Skin.Adj = skin.Adj;

            underBody.Skin.DescKey = skin.DescKey;

            if (Under != null)
                underBody.Skin.FurColor = Under;

            underBody.Id = BodyPartTypes.Furry;
        }
    }
}