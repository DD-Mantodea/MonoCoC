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
    // ── 原版 SalamanderFirewater.hx（火蜥蜴转化）────────────────────────
    public class SalamanderFirewaterEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            player.SlimeFeed();

            mutations.InitTransformation([2, 3, 4]);

            context.Write("Items.SalamanderFirewater.Drink".Localized());

            if (100f * player.Speed / player.MaxValue(GameStats.Speed) > 70
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.SalamanderFirewater.Slow".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, -1));
            }

            if (100f * player.Intelligence / player.MaxValue(GameStats.Intelligence) > 60
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.SalamanderFirewater.Dumber".Localized());

                player.ApplyStats(new DynStat(GameStats.Intelligence, -1));
            }

            var cocks = body.GetList<CockPart>();

            if (player.LibidoPercent < 90 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.SalamanderFirewater.Libido1".Localized());

                if (cocks.Count > 0 && (player.Gender != Gender.Hermaphrodite || random.Next(2) == 0))
                {
                    context.Write("Items.SalamanderFirewater.LibidoFilling".Localized());

                    if (cocks.Count > 1)
                        context.Write("Items.SalamanderFirewater.LibidoEach".Localized());

                    context.Write("Items.SalamanderFirewater.LibidoCock".Localized());
                }
                else if (player.HasVagina())
                    context.Write("Items.SalamanderFirewater.LibidoVagina".Localized(player.VaginaDescript(0)));
                else
                    context.Write("Items.SalamanderFirewater.LibidoGenderless".Localized(player.AssDescript()));

                player.ApplyStats(new DynStat(GameStats.Libido, 2));
            }

            var tou100 = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            if (tou100 < 90 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                if (tou100 < 50)
                {
                    context.Write("Items.SalamanderFirewater.Tough3".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, 3));
                }
                else if (tou100 < 70)
                {
                    context.Write("Items.SalamanderFirewater.Tough2".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, 2));
                }
                else
                {
                    context.Write("Items.SalamanderFirewater.Tough1".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, 1));
                }

            if (100f * player.Strength / player.MaxValue(GameStats.Strength) < 80
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.SalamanderFirewater.Strength".Localized());

                player.ApplyStats(new DynStat(GameStats.Strength, 1));
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
                    context.Write("Items.SalamanderFirewater.LizardCock1".Localized(player.CockDescript(index)));

                    context.Write(sheath

                        ? "Items.SalamanderFirewater.LizardCockSheath".Localized()

                        : "Items.SalamanderFirewater.LizardCockBase".Localized());

                    context.Write("Items.SalamanderFirewater.LizardCock2".Localized());

                    if (player.Corruption < 33)
                        context.Write("Items.SalamanderFirewater.LizardCockHorrified".Localized());
                    else if (player.Corruption < 66)
                        context.Write("Items.SalamanderFirewater.LizardCockStrange".Localized());
                    else
                        context.Write("Items.SalamanderFirewater.LizardCockFun".Localized());

                    context.Write("Items.SalamanderFirewater.LizardCockEnd".Localized());

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
                context.Write("Items.SalamanderFirewater.LizardSecond1".Localized());

                var index = 0;

                while (index < cocks.Count && cocks[index].Id.Path == "lizard")

                    index += 1;

                if (index < cocks.Count)
                {
                    context.Write(cocks.Count == 2

                        ? "Items.SalamanderFirewater.LizardSecondOther".Localized()

                        : "Items.SalamanderFirewater.LizardSecondAnother".Localized());

                    context.Write("Items.SalamanderFirewater.LizardSecond2".Localized());

                    if (player.CumQ() < 50)
                        context.Write("Items.SalamanderFirewater.LizardSecondPreLow".Localized());
                    else if (player.CumQ() < 700)
                        context.Write("Items.SalamanderFirewater.LizardSecondPreMid".Localized());
                    else
                        context.Write("Items.SalamanderFirewater.LizardSecondPreHigh".Localized());

                    context.Write("Items.SalamanderFirewater.LizardSecond3".Localized());

                    cocks[index].Id = new Identifier("coc", "lizard");

                    mutations.Changes += 1;

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 3),

                        new DynStat(GameStats.Lust, 10));
                }
            }

            var rows = body.GetList<BreastRowPart>();

            if (player.BiggestBreastSize() >= 1 && player.Gender == Gender.Male
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                if (player.BiggestBreastSize() > 8)
                    context.Write("Items.SalamanderFirewater.BreastsVanishHuge".Localized());
                else
                    context.Write("Items.SalamanderFirewater.BreastsVanish".Localized());

                context.Write("Items.SalamanderFirewater.BreastsVanishEnd".Localized());

                foreach (var row in rows)
                    if (row.BreastRating > 8)
                        row.BreastRating /= 2;
                    else
                        row.BreastRating = 0;

                player.ApplyStats(new DynStat(GameStats.Libido, 2));

                mutations.Changes += 1;
            }

            if (player.AverageNipplesPerBreast() > 1 && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.SalamanderFirewater.Nipples1".Localized(
                    Appearance.AllBreastsDescript(player)));

                context.Write(player.BiggestBreastSize() < 1

                    ? "Items.SalamanderFirewater.NipplesQuoted".Localized()

                    : "Items.SalamanderFirewater.NipplesWord".Localized());

                mutations.Changes += 1;

                foreach (var row in rows)
                    row.NipplesPerBreast = 1;
            }

            if (rows.Count > 0 && SmallestBreastSize(rows) < 6 && player.Gender == Gender.Female
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                var smallestRow = 0;

                for (var index = 1; index < rows.Count; index++)
                    if (rows[index].BreastRating < rows[smallestRow].BreastRating)
                        smallestRow = index;

                context.Write("Items.SalamanderFirewater.BreastGrow".Localized(player.BreastDescript(smallestRow)));

                rows[smallestRow].BreastRating += 1;

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && rows.Count > 1 && random.Next(3) == 0
                && !(OptionStore.Instance.Get("hyper", 0) > 0))

                mutations.RemoveExtraBreastRow("salamanderfirewater");

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("salamanderfirewater");

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("salamanderfirewater");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("salamanderfirewater");

            var tail = body.Get<TailPart>();

            var lowerBody = body.Get<LowerBodyPart>();

            var arms = body.Get<ArmsPart>();

            var eyes = body.Get<EyesPart>();

            var face = body.Get<FacePart>();

            var ears = body.Get<EarsPart>();

            var skin = body.Get<SkinPart>();

            if (tail.Id.Path != "lizard" && tail.Id.Path != "salamander"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write(tail.Id.Path == "none"

                    ? "Items.SalamanderFirewater.TailNone".Localized()

                    : "Items.SalamanderFirewater.TailOther".Localized());

                tail.Id = new Identifier("coc", "lizard");

                mutations.Changes += 1;
            }

            if (tail.Id.Path == "lizard" && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write(skin.Tone == "red"

                    ? "Items.SalamanderFirewater.TailFlameRed1".Localized()

                    : "Items.SalamanderFirewater.TailFlame1".Localized());

                if (skin.Tone != "red")
                    context.Write("Items.SalamanderFirewater.TailFlame2".Localized());

                context.Write(skin.Tone == "red"

                    ? "Items.SalamanderFirewater.TailFlameRed2".Localized()

                    : "Items.SalamanderFirewater.TailFlame3".Localized());

                tail.Id = new Identifier("coc", "salamander");

                mutations.Changes += 1;
            }

            if (lowerBody.Id.Path != "salamander" && tail.Id.Path == "salamander"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                if (lowerBody.Id.Path == "hoofed")
                    context.Write("Items.SalamanderFirewater.LegsHoof".Localized());
                else if (player.IsTaur())
                    context.Write("Items.SalamanderFirewater.LegsTaur".Localized());
                else if (lowerBody.Id.Path is "human" or "dog" or "demonic_high_heels" or "demonic_claws"
                    or "bee" or "cat" or "lizard")

                    context.Write("Items.SalamanderFirewater.LegsFeet".Localized());
                else
                    context.Write("Items.SalamanderFirewater.LegsOther".Localized());

                context.Write("Items.SalamanderFirewater.LegsEnd".Localized());

                lowerBody.Id = new Identifier("coc", "salamander");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            if (arms.Id.Path != "salamander" && lowerBody.Id.Path == "salamander"
                && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.SalamanderFirewater.Arms1".Localized());

                context.Write(player.HasScales()

                    ? "Items.SalamanderFirewater.ArmsScales".Localized()

                    : "Items.SalamanderFirewater.ArmsSkin".Localized(skin.Tone == "red"

                        ? "scales"

                        : "red, leathery scales"));

                context.Write("Items.SalamanderFirewater.Arms2".Localized());

                arms.Id = new Identifier("coc", "salamander");

                arms.UpdateClaws(new Identifier("coc", "salamander"));

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0
                && eyes.Id.Path != "human")
            {
                if (eyes.Id.Path == "black_eyes_sand_trap")
                    context.Write("Items.SalamanderFirewater.SandTrapEyes".Localized());
                else
                {
                    context.Write("Items.SalamanderFirewater.Eyes1".Localized());

                    if (eyes.Id.Path is "four_spider_eyes" or "spider")
                        context.Write("Items.SalamanderFirewater.ArachnidEyes".Localized());

                    context.Write("Items.SalamanderFirewater.Eyes2".Localized());
                }

                eyes.Id = new Identifier("coc", "human");

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            if (face.Id.Path != "human" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.SalamanderFirewater.HumanFace".Localized());

                face.Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            if (face.Id.Path == "human" && ears.Id.Path != "human"
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.SalamanderFirewater.HumanEars".Localized());

                ears.Id = new Identifier("coc", "human");

                mutations.Changes += 1;
            }

            if (Array.IndexOf(ColorLists.SalamanderSkin, skin.Tone) < 0
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                mutations.Changes += 1;

                context.Write("Items.SalamanderFirewater.SkinTone1".Localized());

                if (player.HasFur())
                    context.Write("Items.SalamanderFirewater.SkinToneFur".Localized(skin.FurColor));
                else
                    context.Write("Items.SalamanderFirewater.SkinTonePlain".Localized(skin.Desc));

                skin.Tone = ColorLists.SalamanderSkin[random.Next(ColorLists.SalamanderSkin.Length)];

                arms.UpdateClaws(arms.Claws.Id);

                context.Write("Items.SalamanderFirewater.SkinTone2".Localized());
            }

            if (skin.Id.Path != "plain" && ears.Id.Path == "human"
                && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.SalamanderFirewater.PlainSkin1".Localized());

                context.Write(player.HasScales()

                    ? "Items.SalamanderFirewater.PlainSkinAre".Localized()

                    : "Items.SalamanderFirewater.PlainSkinIs".Localized());

                context.Write("Items.SalamanderFirewater.PlainSkin2".Localized());

                skin.Id = new Identifier("coc", "plain");

                skin.DescKey = "Skin.Desc.skin";

                skin.Adj = "";

                body.Get<UnderBodyPart>().Restore();

                mutations.Changes += 1;
            }

            if (random.Next(4) == 0 && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            if (mutations.Changes == 0)
            {
                context.Write("Items.SalamanderFirewater.FailSafe".Localized());

                player.HealthChange(100);

                player.ApplyStats(new DynStat(GameStats.Lust, 5));
            }

            if (tail.Id.Path == "salamander" && lowerBody.Id.Path == "salamander"
                && arms.Id.Path == "salamander" && !player.HasPerk(PerkIds.Lustserker))
            {
                context.Write("Items.SalamanderFirewater.Lustserker1".Localized());

                context.Write("Items.SalamanderFirewater.Lustserker2".Localized());

                player.CreatePerk(PerkIds.Lustserker, 0, 0, 0, 0);
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