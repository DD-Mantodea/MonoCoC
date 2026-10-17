using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 TonOTrice.hx（鸡蛇/雷元素转化）────────────────────────────
    public class TonOTriceEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            player.SlimeFeed();

            mutations.InitTransformation([2, 2, 3, 4]);

            context.Write("Items.TonOTrice.Drink".Localized());

            if (player.Speed < 100f + player.AscensionFactor() && mutations.Changes < mutations.ChangeLimit
                && random.Next(3) == 0)
            {
                context.Write("Items.TonOTrice.Speed".Localized());

                if (player.Speed < 50f + player.AscensionFactor())
                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                if (player.Speed < 75f + player.AscensionFactor())
                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                player.ApplyStats(new DynStat(GameStats.Speed, 1));
            }

            if (player.Toughness > 80f + player.AscensionFactor()
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Toughness".Localized());

                player.ApplyStats(new DynStat(GameStats.Toughness, -1));
            }

            if (player.Sensitivity > 20 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.TonOTrice.Sensitivity".Localized());

                player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));
            }

            var cocks = body.GetList<CockPart>();

            if (player.Libido < 100 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.TonOTrice.Libido1".Localized());

                if (cocks.Count > 0 && (player.Gender != Gender.Hermaphrodite || random.Next(2) == 0))
                {
                    context.Write("Items.TonOTrice.LibidoFilling".Localized());

                    if (cocks.Count > 1)
                        context.Write("Items.TonOTrice.LibidoEach".Localized());

                    context.Write("Items.TonOTrice.LibidoCock".Localized());
                }
                else if (player.HasVagina())
                    context.Write("Items.TonOTrice.LibidoVagina".Localized());
                else
                    context.Write("Items.TonOTrice.LibidoGenderless".Localized());

                if (player.Libido < 50)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.Libido < 75)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                player.ApplyStats(new DynStat(GameStats.Libido, 1));
            }

            var rows = body.GetList<BreastRowPart>();

            if (player.BiggestLactation() >= 1 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Lactation1".Localized());

                context.Write("Items.TonOTrice.Lactation2".Localized());

                if (player.HasPerk(PerkIds.Feeder) || player.HasStatusEffect(StatusEffectIds.Feeder))
                {
                    context.Write("Items.TonOTrice.FeederLost".Localized());

                    player.RemovePerk(PerkIds.Feeder);

                    player.RemoveStatusEffect(StatusEffectIds.Feeder);
                }

                mutations.Changes += 1;

                foreach (var row in rows)
                    row.Lactation = 0;
            }

            if (player.AverageNipplesPerBreast() > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Nipples1".Localized(
                    Appearance.AllBreastsDescript(player)));

                context.Write(player.BiggestBreastSize() < 1

                    ? "Items.TonOTrice.NipplesQuoted".Localized()

                    : "Items.TonOTrice.NipplesWord".Localized());

                mutations.Changes += 1;

                foreach (var row in rows)
                    row.NipplesPerBreast = 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && rows.Count > 1 && random.Next(3) == 0
                && !(OptionStore.Instance.Get("hyper", 0) > 0))

                mutations.RemoveExtraBreastRow("TonOTrice");

            var butt = body.Get<ButtPart>();

            var hips = body.Get<HipsPart>();

            if (butt.Rating > 5 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                mutations.Changes += 1;

                butt.Rating -= 1;

                context.Write("Items.TonOTrice.ButtShrink".Localized());
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

                    context.Write("Items.TonOTrice.BreastsShrink".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                    mutations.Changes += 1;
                }

                if (SmallestBreastSize(rows) < 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    foreach (var row in rows)
                        if (row.BreastRating < 2)
                            row.BreastRating += 1;

                    context.Write("Items.TonOTrice.BreastsGrow".Localized());

                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                    mutations.Changes += 1;
                }

                if (hips.Rating > 12 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.TonOTrice.HipsNarrow".Localized());

                    hips.Rating -= 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (hips.Rating < 6 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.TonOTrice.HipsWiden".Localized());

                    hips.Rating += 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (player.NippleLength > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.TonOTrice.NipplesShrink".Localized());

                    player.NippleLength /= 2;
                }

                var vaginas = body.GetList<VaginaPart>();

                if (vaginas.Count > 0 && vaginas[0].Wetness < 3
                    && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                {
                    context.Write("Items.TonOTrice.VaginaLoosen".Localized());

                    vaginas[0].Wetness += 1;

                    mutations.Changes += 1;
                }

                if (player.Tone < 65 && random.Next(3) == 0)
                    context.Write(player.ModTone(65, 2));

                if (player.Thickness > 35 && random.Next(3) == 0)
                    context.Write(player.ModThickness(35, 5));

                if (random.Next(5) == 0 && player.CockatriceScore() > 3)
                    mutations.UpdateOvipositionPerk("TonOTrice");
            }

            if (isMale)
            {
                if (player.BiggestBreastSize() > 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    foreach (var row in rows)
                        if (row.BreastRating > 2)
                            row.BreastRating -= 1;

                    context.Write("Items.TonOTrice.MaleBreastsShrink".Localized());

                    player.ApplyStats(new DynStat(GameStats.Speed, 1));

                    mutations.Changes += 1;
                }

                if (player.NippleLength > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.TonOTrice.NipplesShrink".Localized());

                    player.NippleLength /= 2;
                }

                if (hips.Rating > 10 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.TonOTrice.HipsNarrow".Localized());

                    hips.Rating -= 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (hips.Rating < 2 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                {
                    context.Write("Items.TonOTrice.HipsWiden".Localized());

                    hips.Rating += 1 + random.Next(3);

                    mutations.Changes += 1;
                }

                if (player.Tone < 70 && random.Next(3) == 0)
                    context.Write(player.ModTone(65, 2));

                if (player.Thickness > 35 && random.Next(3) == 0)
                    context.Write(player.ModThickness(35, 5));
            }

            if (isMaleOrHerm)
            {
                if (player.ShortestCockLength() < 6 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    var increment = player.IncreaseCock(player.SmallestCockIndex(), 1 + random.Next(2));

                    context.Write("Items.TonOTrice.CockLonger".Localized(increment));

                    mutations.Changes += 1;
                }

                if (player.BiggestCockLength() > 16 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    var index = player.BiggestCockIndex();

                    context.Write("Items.TonOTrice.CockShorter".Localized());

                    cocks[index].Length -= (random.Next(10) + 5) / 10f;

                    if (cocks[index].Thickness > 3)
                    {
                        context.Write("Items.TonOTrice.CockThinner".Localized(player.CockDescript(index)));

                        cocks[index].Thickness -= (random.Next(4) + 1) / 10f;
                    }

                    mutations.Changes += 1;
                }

                if (player.SmallestCockArea() < 10 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
                {
                    context.Write(cocks.Count > 1

                        ? "Items.TonOTrice.CockThickerMany".Localized()

                        : "Items.TonOTrice.CockThickerOne".Localized());

                    var thinnest = 0;

                    for (var index = 1; index < cocks.Count; index++)
                        if (cocks[index].Thickness < cocks[thinnest].Thickness)
                            thinnest = index;

                    cocks[thinnest].Thicken(1.5f);

                    mutations.Changes += 1;
                }
            }

            var sheath = false;

            foreach (var cock in cocks)
                if (cock.SheathOn)
                {
                    sheath = true;

                    break;
                }

            if (player.CountCocksOfType("Lizard") == 0 && cocks.Count > 0
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                var index = 0;

                while (index < cocks.Count && cocks[index].Id.Path == "lizard")

                    index += 1;

                if (index < cocks.Count)
                {
                    context.Write("Items.TonOTrice.LizardCock1".Localized(player.CockDescript(index)));

                    context.Write(sheath

                        ? "Items.TonOTrice.LizardCockSheath".Localized()

                        : "Items.TonOTrice.LizardCockBase".Localized());

                    context.Write("Items.TonOTrice.LizardCock2".Localized());

                    if (player.Corruption < 33)
                        context.Write("Items.TonOTrice.LizardCockHorrified".Localized());
                    else if (player.Corruption < 66)
                        context.Write("Items.TonOTrice.LizardCockStrange".Localized());
                    else
                        context.Write("Items.TonOTrice.LizardCockFun".Localized());

                    context.Write("Items.TonOTrice.LizardCockEnd".Localized());

                    cocks[index].Id = new Identifier("coc", "lizard");

                    mutations.Changes += 1;

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 3),

                        new DynStat(GameStats.Lust, 10));
                }
            }

            if (cocks.Count > 1 && player.CountCocksOfType("Lizard") > 0
                && cocks.Count > player.CountCocksOfType("Lizard")

                && random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TonOTrice.LizardSecond1".Localized());

                var index = 0;

                while (index < cocks.Count && cocks[index].Id.Path == "lizard")

                    index += 1;

                if (index < cocks.Count)
                {
                    context.Write(cocks.Count == 2

                        ? "Items.TonOTrice.LizardSecondOther".Localized()

                        : "Items.TonOTrice.LizardSecondAnother".Localized());

                    context.Write("Items.TonOTrice.LizardSecond2".Localized());

                    if (player.CumQ() < 50)
                        context.Write("Items.TonOTrice.LizardSecondPreLow".Localized());
                    else if (player.CumQ() < 700)
                        context.Write("Items.TonOTrice.LizardSecondPreMid".Localized());
                    else
                        context.Write("Items.TonOTrice.LizardSecondPreHigh".Localized());

                    context.Write("Items.TonOTrice.LizardSecond3".Localized());

                    cocks[index].Id = new Identifier("coc", "lizard");

                    mutations.Changes += 1;

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 3),

                        new DynStat(GameStats.Lust, 10));
                }
            }

            if (player.CountCocksOfType("Lizard") == cocks.Count && mutations.Changes < mutations.ChangeLimit
                && player.HasStatusEffect(StatusEffectIds.Infested))
            {
                context.Write("Items.TonOTrice.WormsLeave1".Localized());

                if (player.Balls > 1)
                    context.Write("Items.TonOTrice.WormsLeaveBalls".Localized(
                        Utils.Utils.Num2Text((int)player.Balls - 1)));

                context.Write("Items.TonOTrice.WormsLeave2".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Infested);

                mutations.Changes += 1;
            }

            if (player.Tallness < 67 && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)
            {
                context.Write("Items.TonOTrice.Taller".Localized());

                player.Tallness += random.Next(3) + 1;

                mutations.Changes += 1;
            }

            if (player.Tallness > 80 && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)
            {
                context.Write("Items.TonOTrice.Shorter".Localized());

                player.Tallness -= random.Next(3) + 1;

                mutations.Changes += 1;
            }

            var antennae = body.Get<AntennaePart>();

            var face = body.Get<FacePart>();

            var ears = body.Get<EarsPart>();

            var horns = body.Get<HornsPart>();

            var arms = body.Get<ArmsPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var hair = body.Get<HairPart>();

            var eyes = body.Get<EyesPart>();

            var tongue = body.Get<TonguePart>();

            var tail = body.Get<TailPart>();

            var wings = body.Get<WingsPart>();

            var skin = body.Get<SkinPart>();

            var neck = body.Get<NeckPart>();

            var underBody = body.Get<UnderBodyPart>();

            if (antennae.Id.Path != "none" && antennae.Id.Path != "cockatrice"
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)

                mutations.RemoveAntennae();

            if (antennae.Id.Path == "none" && face.Id.Path == "cockatrice" && ears.Id.Path == "cockatrice"
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.TonOTrice.Antennae1".Localized(hair.Color));

                context.Write("Items.TonOTrice.Antennae2".Localized());

                antennae.Id = new Identifier("coc", "cockatrice");

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit
                && (horns.Id != BodyPartTypes.None || horns.Value != 0)

                && random.Next(5) == 0)
            {
                context.Write("Items.TonOTrice.Horns1".Localized());

                var singularHorn = horns.Id.Path is "unicorn" or "rhino";

                context.Write(singularHorn

                    ? "Items.TonOTrice.HornSingular".Localized()

                    : "Items.TonOTrice.HornsPlural".Localized());

                context.Write("Items.TonOTrice.Horns2".Localized());

                horns.Value = 0;

                horns.Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && face.Id.Path != "cockatrice" && arms.Id.Path == "cockatrice"
                && lowerBody.Id.Path == "cockatrice"

                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.TonOTrice.Face1".Localized(
                    player.HasCockatriceSkin() ? skin.FurColor : hair.Color));

                context.Write("Items.TonOTrice.Face2".Localized());

                face.Id = new Identifier("coc", "cockatrice");

                mutations.Changes += 1;
            }

            if (hair.Id.Path != "feather" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Hair".Localized());

                hair.Id = new Identifier("coc", "feather");

                mutations.Changes += 1;
            }

            if (eyes.Id.Path != "cockatrice" && face.Id.Path == "cockatrice"
                && underBody.Id.Path == "cockatrice" && ears.Id.Path == "cockatrice"

                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.TonOTrice.Eyes1".Localized());

                context.Write("Items.TonOTrice.Eyes2".Localized());

                eyes.Id = new Identifier("coc", "cockatrice");

                eyes.Count = 2;
            }

            if (tongue.Id.Path != "lizard" && face.Id.Path == "cockatrice"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)

                mutations.GainLizardTongue();

            if (ears.Id.Path != "cockatrice" && face.Id.Path == "cockatrice"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.TonOTrice.Ears1".Localized());

                context.Write("Items.TonOTrice.Ears2".Localized());

                ears.Id = new Identifier("coc", "cockatrice");

                mutations.Changes += 1;
            }

            if (arms.Id.Path != "cockatrice" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Arms1".Localized(
                    player.HasCockatriceSkin() ? skin.FurColor : hair.Color));

                context.Write("Items.TonOTrice.Arms2".Localized());

                arms.Id = new Identifier("coc", "cockatrice");

                arms.UpdateClaws(new Identifier("coc", "cockatrice"));

                mutations.Changes += 1;
            }

            if (neck.Id.Path != "cockatrice" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("TonOTrice");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.None
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("TonOTrice");

            var colorChoice = mutations.NewCockatriceColors();

            if (!player.HasCockatriceSkin() && face.Id.Path == "cockatrice"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                mutations.RestoreNeck("TonOTrice-forceRestoreNeck");

                context.Write("Items.TonOTrice.Body1".Localized());

                context.Write("Items.TonOTrice.Body2".Localized(colorChoice[2]));

                context.Write("Items.TonOTrice.Body3".Localized());

                skin.Id = new Identifier("coc", "lizard_scales");

                skin.FurColor = colorChoice[0];

                skin.Tone = colorChoice[2];

                skin.DescKey = "Skin.Desc.Scales";

                underBody.Id = new Identifier("coc", "cockatrice");

                underBody.Skin.Id = new Identifier("coc", "feathered");

                underBody.Skin.FurColor = colorChoice[1];

                underBody.Skin.Tone = colorChoice[2];

                underBody.Skin.DescKey = "Skin.Desc.Feathers";

                neck.Restore();

                neck.Id = new Identifier("coc", "cockatrice");

                neck.Color = colorChoice[1];

                mutations.Changes += 1;
            }

            if (neck.Id.Path != "cockatrice" && player.HasCockatriceSkin()
                && face.Id.Path == "cockatrice" && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                mutations.RestoreNeck("TonOTrice");

                context.Write("Items.TonOTrice.Neck1".Localized(colorChoice[1]));

                context.Write("Items.TonOTrice.Neck2".Localized());

                neck.Id = new Identifier("coc", "cockatrice");

                neck.Color = colorChoice[1];

                mutations.Changes += 1;
            }

            if (lowerBody.Id.Path != "cockatrice" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Legs1".Localized(
                    player.HasCockatriceSkin() ? skin.FurColor : hair.Color));

                context.Write("Items.TonOTrice.Legs2".Localized());

                lowerBody.Id = new Identifier("coc", "cockatrice");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            if (tail.Id.Path != "cockatrice" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.Tail1".Localized());

                context.Write("Items.TonOTrice.Tail2".Localized());

                tail.Id = new Identifier("coc", "cockatrice");

                tail.Recharge = 5;

                tail.Venom = 0;

                mutations.Changes += 1;
            }

            if (wings.Id.Path != "feathered_large" && arms.Id.Path == "cockatrice"
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.TonOTrice.NewLine".Localized());

                if (wings.Id.Path != "none")
                    context.Write("Items.TonOTrice.WingsGone".Localized());

                wings.Id = new Identifier("coc", "feathered_large");

                wings.Color = player.HasFur() || player.HasCockatriceSkin() ? skin.FurColor : hair.Color;

                context.Write("Items.TonOTrice.WingsGrow".Localized(wings.Color));

                mutations.Changes += 1;
            }

            if (mutations.Changes == 0)
            {
                context.Write("Items.TonOTrice.FailSafe".Localized());

                player.HealthChange(50);

                player.ApplyStats(new DynStat(GameStats.Lust, 3));
            }

            player.RefillHunger(20);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        private static float SmallestBreastSize(System.Collections.Generic.List<BreastRowPart> rows)
        {
            var smallest = float.MaxValue;

            foreach (var row in rows)
                smallest = Math.Min(smallest, row.BreastRating);

            return smallest;
        }
    }
}