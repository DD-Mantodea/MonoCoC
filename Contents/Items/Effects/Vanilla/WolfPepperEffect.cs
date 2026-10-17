using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Configuration;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 WolfPepper.hx（狼转化）─────────────────────────────────────
    public class WolfPepperEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            var crit = 1f;

            mutations.InitTransformation([2, 2]);

            context.Write("Items.WolfPepper.Eat".Localized());

            if (random.Next(100) < 15)
            {
                crit = (int)(random.NextDouble() * 20) / 10f + 2;

                context.Write("Items.WolfPepper.ExtraRipe".Localized());
            }

            player.ApplyStats(
                new DynStat(GameStats.Libido, 1 + random.Next(2)),

                new DynStat(GameStats.Lust, 5 + random.Next(10)),

                new DynStat(GameStats.Corruption, 1 + random.Next(5)));

            context.Write("Items.WolfPepper.LickLips".Localized());

            if (100f * player.Toughness / player.MaxValue(GameStats.Toughness) < 70
                && TrueOnceInN(random, 3) && mutations.Changes < mutations.ChangeLimit)
            {
                player.ApplyStats(new DynStat(GameStats.Toughness, crit));

                context.Write(crit > 1

                    ? "Items.WolfPepper.ToughCrit".Localized()

                    : "Items.WolfPepper.Tough".Localized());
            }

            if (100f * player.Speed / player.MaxValue(GameStats.Speed) > 30
                && TrueOnceInN(random, 7) && mutations.Changes < mutations.ChangeLimit)
            {
                player.ApplyStats(new DynStat(GameStats.Speed, -crit));

                context.Write(crit > 1

                    ? "Items.WolfPepper.SlowCrit".Localized()

                    : "Items.WolfPepper.Slow".Localized());
            }

            if (100f * player.Intelligence / player.MaxValue(GameStats.Intelligence) < 60
                && TrueOnceInN(random, 7) && mutations.Changes < mutations.ChangeLimit)
            {
                player.ApplyStats(new DynStat(GameStats.Intelligence, crit));

                context.Write("Items.WolfPepper.Smart1".Localized());

                if (crit > 1)
                    context.Write("Items.WolfPepper.SmartCrit".Localized());

                context.Write("Items.WolfPepper.Smart2".Localized());
            }

            var arms = body.Get<ArmsPart>();

            var antennae = body.Get<AntennaePart>();

            var horns = body.Get<HornsPart>();

            var wings = body.Get<WingsPart>();

            var rearBody = body.Get<RearBodyPart>();

            var tongue = body.Get<TonguePart>();

            var eyes = body.Get<EyesPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var hair = body.Get<HairPart>();

            if (arms.Claws.Id.Path is not ("wolf" or "normal") && TrueOnceInN(random, 4))
                arms.Claws.Restore();

            if (antennae.Id != BodyPartTypes.None && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3))
                mutations.RemoveAntennae();

            if ((horns.Id != BodyPartTypes.None || horns.Value > 0)
                && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3))
            {
                context.Write("Items.WolfPepper.HornsCrumble".Localized());

                horns.Value = 0;

                horns.Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            if ((wings.Id != BodyPartTypes.None || rearBody.Id == BodyPartTypes.SharkFin)
                && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3))
            {
                if (rearBody.Id == BodyPartTypes.SharkFin)
                {
                    context.Write("Items.WolfPepper.FinGone".Localized());

                    rearBody.Restore();
                }
                else
                    context.Write("Items.WolfPepper.WingsGone".Localized());

                wings.Restore();

                mutations.Changes += 1;
            }

            if (tongue.Id.Path != "human" && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3))
            {
                context.Write("Items.WolfPepper.HumanTongue".Localized());

                tongue.Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3)
                && eyes.Id.Path != "human" && eyes.Id.Path != "wolf")
            {
                if (eyes.Id.Path == "black_eyes_sand_trap")
                    context.Write("Items.WolfPepper.SandTrapEyes".Localized());
                else
                {
                    context.Write("Items.WolfPepper.Eyes1".Localized());

                    if (eyes.Id.Path is "four_spider_eyes" or "spider")
                        context.Write("Items.WolfPepper.ArachnidEyesGone".Localized());

                    context.Write("Items.WolfPepper.Eyes2".Localized());
                }

                eyes.Id = new Identifier("coc", "human");

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            if (lowerBody.Id.Path is not ("wolf" or "human") && TrueOnceInN(random, 4))
                mutations.RestoreLegs("wolfPepper");

            if (arms.Id.Path != "wolf" && TrueOnceInN(random, 4))
                mutations.RestoreArms("wolfPepper");

            if (hair.Id.Path == "feather" && TrueOnceInN(random, 4))
                mutations.RemoveFeatheryHair();

            if (hair.Id.Path is "basilisk_plume" or "basilisk_spines" && TrueOnceInN(random, 4))
                mutations.RemoveBassyHair();

            var cocks = body.GetList<CockPart>();

            if (player.Gender == Gender.Genderless && cocks.Count == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WolfPepper.GrowWolfCock".Localized());

                player.CreateCock();

                cocks = body.GetList<CockPart>();

                cocks[0].Length = random.Next(4) + 4;

                cocks[0].Thickness = random.Next(2);

                cocks[0].KnotMultiplier = 1.5f;

                cocks[0].Id = new Identifier("coc", "wolf");

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 3),

                    new DynStat(GameStats.Sensitivity, 2),

                    new DynStat(GameStats.Lust, 25));

                mutations.Changes += 1;
            }

            if (player.HasCock() && player.WolfCocks() < cocks.Count
                && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 2))
            {
                var index = FindFirstCockNotOfType(player, "wolf");

                if (index >= 0)
                {
                    if (cocks[index].TypeIndex > 4)
                    {
                        context.Write("Items.WolfPepper.WolfCockReshape".Localized(player.CockDescript(index)));

                        player.ApplyStats(
                            new DynStat(GameStats.Sensitivity, 3),

                            new DynStat(GameStats.Lust, 5 * crit));

                        if (cocks[index].Id.Path == "horse")
                        {
                            cocks[index].Length += cocks[index].Length > 6 ? -2 : -0.5f;

                            cocks[index].Thicken(0.5f);
                        }
                    }

                    cocks[index].Id = new Identifier("coc", "wolf");

                    cocks[index].KnotMultiplier = 1.5f;

                    cocks[index].Thicken(2);

                    mutations.Changes += 1;
                }
            }

            var rows = body.GetList<BreastRowPart>();

            if (rows.Count is > 0 and <= 4 && rows[0].BreastRating >= 1)
                if (rows.Count < 4 && TrueOnceInN(random, 2) && mutations.Changes < mutations.ChangeLimit)
                {
                    player.CreateBreastRow();

                    rows = body.GetList<BreastRowPart>();

                    var index = rows.Count - 1;

                    if (rows[0].BreastRating <= rows.Count)
                    {
                        context.Write("Items.WolfPepper.BreastGrowRow1".Localized());

                        rows[0].BreastRating += 1;

                        context.Write("Items.WolfPepper.BreastGrowRow2".Localized());

                        mutations.Changes += 1;
                    }

                    if (rows.Count == 2 && mutations.Changes < mutations.ChangeLimit)
                    {
                        rows[index].BreastRating = rows[0].BreastRating - 1;

                        if (rows[0].BreastRating - 1 == 0)
                            context.Write("Items.WolfPepper.SecondRowFlat".Localized());
                        else
                            context.Write("Items.WolfPepper.SecondRow".Localized(rows[index].Cup()));

                        context.Write("Items.WolfPepper.NewNipple".Localized());

                        player.ApplyStats(
                            new DynStat(GameStats.Sensitivity, 2),

                            new DynStat(GameStats.Lust, 5));

                        mutations.Changes += 1;
                    }

                    if (rows.Count > 2 && rows[0].BreastRating > rows.Count && mutations.Changes < mutations.ChangeLimit)
                    {
                        player.ApplyStats(
                            new DynStat(GameStats.Sensitivity, 2),

                            new DynStat(GameStats.Lust, 5));

                        rows[index].BreastRating = rows[index - 1].BreastRating - 1;

                        if (rows[index].BreastRating < 0)
                            rows[index].BreastRating = 0;

                        if (rows[index - 1].BreastRating < 0)
                            rows[index - 1].BreastRating = 0;

                        if (rows[index].BreastRating == 0)
                            context.Write("Items.WolfPepper.NewRowFlat".Localized());
                        else
                            context.Write("Items.WolfPepper.NewRow".Localized(rows[index].Cup(), player.BreastDescript(index)));

                        context.Write("Items.WolfPepper.NewNipple".Localized());

                        mutations.Changes += 1;
                    }

                    if (crit > 1)
                        if (crit > 2)
                        {
                            context.Write("Items.WolfPepper.BreastSensCrit".Localized());

                            player.ApplyStats(
                                new DynStat(GameStats.Sensitivity, 8),

                                new DynStat(GameStats.Lust, 15));
                        }
                        else
                        {
                            context.Write("Items.WolfPepper.BreastSens".Localized());

                            player.ApplyStats(
                                new DynStat(GameStats.Sensitivity, 4),

                                new DynStat(GameStats.Lust, 10));
                        }
                }
                else if (TrueOnceInN(random, 2))
                {
                    var evened = false;

                    for (var index = rows.Count - 1; index >= 1; index--)
                    {
                        if (rows[index].BreastRating + 1 >= rows[index - 1].BreastRating)
                            continue;

                        if (!evened)
                        {
                            evened = true;

                            context.Write("Items.WolfPepper.EvenNewline".Localized());
                        }

                        var ordinal = index switch
                        {
                            0 => "Items.WolfPepper.First".Localized(),

                            1 => "Items.WolfPepper.Second".Localized(),

                            2 => "Items.WolfPepper.Third".Localized(),

                            3 => "Items.WolfPepper.Fourth".Localized(),

                            _ => "",
                        };

                        context.Write("Items.WolfPepper.EvenRow".Localized(ordinal, player.BreastDescript(index)));

                        var grow = rows[index - 1].BreastRating - rows[index].BreastRating - 1;

                        if (grow > 4)
                            grow = 4;

                        if (grow < 1)
                            grow = 1;

                        rows[index].BreastRating += grow;
                    }
                }

            if (mutations.Changes < mutations.ChangeLimit && rows.Count > 4 && TrueOnceInN(random, 3))
                mutations.RemoveExtraBreastRow("wolfPepper");
            else if (player.HasVagina() && rows.Count == 0 && player.NippleLength == 0
                && TrueOnceInN(random, 2) && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WolfPepper.GrowBreasts1".Localized());

                context.Write("Items.WolfPepper.GrowBreasts2".Localized());

                player.CreateBreastRow();

                rows = body.GetList<BreastRowPart>();

                rows[0].BreastRating = 4;

                rows[0].Breasts = 2;

                player.NippleLength = 0.25f;

                player.ApplyStats(
                    new DynStat(GameStats.Sensitivity, 4),

                    new DynStat(GameStats.Lust, 6));

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3)
                && !(OptionStore.Instance.Get("hyper", 0) > 0))
            {
                var threshold = 0f;

                if (player.BiggestBreastSize() > 4)
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

                                ? "Items.WolfPepper.BreastsBigShrink".Localized()

                                : "Items.WolfPepper.BreastsRowShrinkBig".Localized(Utils.Utils.Num2TextOrdinal(index + 1)));
                        }
                        else
                        {
                            rows[index].BreastRating -= 1;

                            context.Write(changedRows == 0

                                ? "Items.WolfPepper.BreastsShrink".Localized(player.BreastDescript(index))

                                : "Items.WolfPepper.BreastsRowShrink".Localized(Utils.Utils.Num2TextOrdinal(index + 1), player.BreastDescript(index)));
                        }

                        changedRows += 1;
                    }

                    if (changedRows > 0)
                        mutations.Changes += 1;
                }
            }

            var skin = body.Get<SkinPart>();

            if (mutations.TfNoFur() && TrueOnceInN(random, 5) && mutations.Changes < mutations.ChangeLimit && !player.HasFur())
            {
                context.Write("Items.WolfPepper.FurTingle".Localized());

                skin.Id = new Identifier("coc", "fur");

                skin.DescKey = "Skin.Desc.fur";

                SetWolfFur(player, skin, body.Get<UnderBodyPart>(), random);

                context.Write("Items.WolfPepper.FurGrown".Localized());

                mutations.Changes += 1;
            }

            var ears = body.Get<EarsPart>();

            var tail = body.Get<TailPart>();

            if (TrueOnceInN(random, 3) && ears.Id.Path != "wolf" && mutations.Changes < mutations.ChangeLimit)
            {
                if (ears.Id.Path == "human")
                    context.Write("Items.WolfPepper.EarsHuman".Localized());
                else if (ears.Id.Path == "horse")
                    context.Write("Items.WolfPepper.EarsHorse".Localized());
                else if (ears.Id.Path == "dog")
                    context.Write("Items.WolfPepper.EarsDog".Localized());
                else
                    context.Write("Items.WolfPepper.EarsOther".Localized());

                ears.Id = new Identifier("coc", "wolf");

                context.Write("Items.WolfPepper.EarsEnd".Localized());

                mutations.Changes += 1;
            }

            if (TrueOnceInN(random, 3) && mutations.Changes < mutations.ChangeLimit && tail.Id.Path != "wolf")
            {
                if (tail.Id.Path == "none")
                    context.Write("Items.WolfPepper.TailNone".Localized());
                else if (tail.Id.Path == "horse")
                    context.Write("Items.WolfPepper.TailHorse".Localized());
                else if (tail.Id.Path == "demonic")
                    context.Write("Items.WolfPepper.TailDemonic".Localized());
                else
                    context.Write("Items.WolfPepper.TailOther".Localized());

                mutations.Changes += 1;

                tail.Id = new Identifier("coc", "wolf");

                context.Write("Items.WolfPepper.TailEnd".Localized());
            }

            if (hair.Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 3))
            {
                context.Write("Items.WolfPepper.HairNormal".Localized());

                hair.Id = new Identifier("coc", "normal");

                mutations.Changes += 1;
            }

            var face = body.Get<FacePart>();

            if (mutations.TfNoFur() && face.Id.Path != "wolf" && ears.Id.Path == "wolf"
                && tail.Id.Path == "wolf" && player.HasFur() && TrueOnceInN(random, 5)

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WolfPepper.WolfFace".Localized());

                face.Id = new Identifier("coc", "wolf");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && lowerBody.LegCount == 2 && lowerBody.Id.Path != "wolf"
                && tail.Id.Path == "wolf" && skin.Id.Path == "fur"

                && TrueOnceInN(random, 4) && mutations.Changes < mutations.ChangeLimit)
            {
                if (lowerBody.Id.Path == "human")
                    context.Write("Items.WolfPepper.LegsHuman".Localized());
                else if (lowerBody.Id.Path == "hoofed")
                    context.Write("Items.WolfPepper.LegsHoof".Localized());
                else
                    context.Write("Items.WolfPepper.LegsOther".Localized());

                lowerBody.Id = new Identifier("coc", "wolf");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && arms.Id.Path != "wolf" && skin.Id.Path == "fur"
                && tail.Id.Path == "wolf" && TrueOnceInN(random, 4) && mutations.Changes < mutations.ChangeLimit)
            {
                var underBody = body.Get<UnderBodyPart>();

                var furColor = underBody.Id == BodyPartTypes.Furry ? underBody.Skin.FurColor : skin.FurColor;

                context.Write("Items.WolfPepper.Arms".Localized(furColor));

                arms.Id = new Identifier("coc", "wolf");

                arms.UpdateClaws(new Identifier("coc", "wolf"));

                mutations.Changes += 1;
            }

            if (eyes.Id.Path != "wolf" && face.Id.Path == "wolf"
                && TrueOnceInN(random, 4) && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.WolfPepper.WolfEyes1".Localized());

                context.Write("Items.WolfPepper.WolfEyes2".Localized());

                eyes.Id = new Identifier("coc", "wolf");

                mutations.Changes += 1;
            }

            if (face.Id.Path == "wolf" && TrueOnceInN(random, 4)
                && mutations.Changes < mutations.ChangeLimit && !player.HasPerk(PerkIds.Bloodhound))
            {
                context.Write("Items.WolfPepper.Bloodhound1".Localized());

                context.Write("Items.WolfPepper.Bloodhound2".Localized());

                context.Write("Items.WolfPepper.Bloodhound3".Localized());

                context.Write("Items.WolfPepper.Bloodhound4".Localized());

                player.CreatePerk(PerkIds.Bloodhound, 0, 0, 0, 0);

                context.Write("Items.WolfPepper.BloodhoundPerk".Localized());

                mutations.Changes += 1;
            }

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 4))
                mutations.RestoreNeck("wolfPepper");

            if (rearBody.Id != BodyPartTypes.SharkFin && mutations.Changes < mutations.ChangeLimit && TrueOnceInN(random, 5))
                mutations.RestoreRearBody("wolfPepper");

            if (TrueOnceInN(random, 5))
                mutations.UpdateOvipositionPerk("wolfPepper");

            if (TrueOnceInN(random, 3))
                context.Write(player.ModTone(100, 4));

            if (TrueOnceInN(random, 3))
                context.Write(player.ModThickness(75, 3));

            player.RefillHunger(10);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        private static bool TrueOnceInN(Random random, int n) => random.Next(n) == 0;

        private static int FindFirstCockNotOfType(Player player, string type)
        {
            var cocks = player.Body.GetList<CockPart>();

            for (var index = 0; index < cocks.Count; index++)
                if (cocks[index].Id.Path != type)
                    return index;

            return -1;
        }

        // 原版 WolfPepper.hx 的狼毛色表
        private static readonly (string Upper, string? Under)[] WolfFurColors =

        [
            ("white", null), ("gray", null), ("dark gray", null), ("light gray", null),

            ("black", null), ("light brown", null), ("sandy brown", null), ("golden", null),

            ("silver", null), ("brown", null), ("auburn", null),

            ("black", "gray"), ("black", "brown"), ("black", "silver"), ("black", "auburn"),

            ("white", "gray"), ("white", "silver"), ("white", "golden"),
        ];

        private static void SetWolfFur(Player player, SkinPart skin, UnderBodyPart underBody, Random random)
        {
            var (Upper, Under) = WolfFurColors[random.Next(WolfFurColors.Length)];

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