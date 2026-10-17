using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 Reptilum.hx（爬虫转化）────────────────────────────────────
    public class ReptilumEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var body = player.Body;

            var flags = CoC.Instance.Flags;

            var tfSource = "reptilum";

            if (player.HasDragonWingsAndFire())
                tfSource += player.IsBasilisk() ? "-dracolisk" : "-dragonewt";
            else
                tfSource += player.IsBasilisk() ? "-basilisk" : "-lizan";

            player.SlimeFeed();

            mutations.InitTransformation([2, 2, 3, 4]);

            context.Write("Items.Reptlum.Drink".Localized());

            if (player.Speed > 50f + player.AscensionFactor()
                && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
            {
                context.Write("Items.Reptlum.Slow".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, -1));
            }

            if (player.SensitivityPercent > 20 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.Reptlum.Sensitivity".Localized());

                player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));
            }

            var cocks = body.GetList<CockPart>();

            if (player.LibidoPercent < 100 && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
            {
                context.Write("Items.Reptlum.Libido1".Localized());

                if (cocks.Count > 0 && (player.Gender != Gender.Hermaphrodite || random.Next(2) == 0))
                {
                    context.Write("Items.Reptlum.LibidoFilling".Localized());

                    if (cocks.Count > 1)
                        context.Write("Items.Reptlum.LibidoEach".Localized());

                    context.Write("Items.Reptlum.LibidoCock".Localized());
                }
                else if (player.HasVagina())
                    context.Write("Items.Reptlum.LibidoVagina".Localized(player.VaginaDescript(0)));
                else
                    context.Write("Items.Reptlum.LibidoGenderless".Localized(player.AssDescript()));

                if (player.LibidoPercent < 50)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.LibidoPercent < 75)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));

                player.ApplyStats(new DynStat(GameStats.Libido, 1));
            }

            var tough = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            if (tough < 70f + player.AscensionFactor() && mutations.Changes < mutations.ChangeLimit && random.Next(3) == 0)
                if (tough < 40f + player.AscensionFactor())
                {
                    context.Write("Items.Reptlum.Tough3".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, 3));
                }
                else if (tough < 55f + player.AscensionFactor())
                {
                    context.Write("Items.Reptlum.Tough2".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, 2));
                }
                else
                {
                    context.Write("Items.Reptlum.Tough1".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, 1));
                }

            var sheath = false;

            foreach (var cock in cocks)
                if (cock.SheathOn)
                {
                    sheath = true;

                    break;
                }

            if (player.CountCocksOfType("Lizard") == 0 && cocks.Count > 0
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 4))
            {
                var index = FindFirstCockNotOfType(player, "lizard");

                if (index >= 0)
                {
                    context.Write("Items.Reptlum.LizardCock1".Localized(player.CockDescript(index)));

                    context.Write(sheath

                        ? "Items.Reptlum.LizardCockSheath".Localized()

                        : "Items.Reptlum.LizardCockBase".Localized());

                    context.Write("Items.Reptlum.LizardCock2".Localized());

                    if (player.Corruption < 33)
                        context.Write("Items.Reptlum.LizardCockHorrified".Localized());
                    else if (player.Corruption < 66)
                        context.Write("Items.Reptlum.LizardCockStrange".Localized());
                    else
                        context.Write("Items.Reptlum.LizardCockFun".Localized());

                    context.Write("Items.Reptlum.LizardCockEnd".Localized());

                    cocks[index].Id = new Identifier("coc", "lizard");

                    mutations.Changes += 1;

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 3),

                        new DynStat(GameStats.Lust, 10));
                }
            }

            if (cocks.Count > 1 && player.CountCocksOfType("Lizard") > 0
                && FindFirstCockNotOfType(player, "lizard") >= 0

                && mutations.TfChance(2, 4) && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Reptlum.LizardSecond1".Localized());

                var index = FindFirstCockNotOfType(player, "lizard");

                if (index >= 0)
                {
                    context.Write(cocks.Count == 2

                        ? "Items.Reptlum.LizardSecondOther".Localized()

                        : "Items.Reptlum.LizardSecondAnother".Localized());

                    context.Write("Items.Reptlum.LizardSecond2".Localized());

                    if (player.CumQ() < 50)
                        context.Write("Items.Reptlum.LizardSecondPreLow".Localized());
                    else if (player.CumQ() < 700)
                        context.Write("Items.Reptlum.LizardSecondPreMid".Localized());
                    else
                        context.Write("Items.Reptlum.LizardSecondPreHigh".Localized());

                    context.Write("Items.Reptlum.LizardSecond3".Localized());

                    cocks[index].Id = new Identifier("coc", "lizard");

                    mutations.Changes += 1;

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 3),

                        new DynStat(GameStats.Lust, 10));
                }
            }

            if (player.CountCocksOfType("Lizard") == 1 && cocks.Count == 1
                && mutations.TfChance(2, 4) && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Reptlum.SecondCock1".Localized());

                if (player.HasFur() || player.HasScales())
                    context.Write("Items.Reptlum.SecondCockScales".Localized());

                context.Write("Items.Reptlum.SecondCock2".Localized());

                player.CreateCock();

                cocks = body.GetList<CockPart>();

                cocks[1].Id = new Identifier("coc", "lizard");

                cocks[1].Length = cocks[0].Length;

                cocks[1].Thickness = cocks[0].Thickness;

                mutations.Changes += 1;

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 3),

                    new DynStat(GameStats.Lust, 10));
            }

            if (player.CountCocksOfType("Lizard") == cocks.Count && mutations.Changes < mutations.ChangeLimit
                && player.HasStatusEffect(StatusEffectIds.Infested))
            {
                context.Write("Items.Reptlum.WormsLeave1".Localized());

                if (player.Balls > 1)
                    context.Write("Items.Reptlum.WormsLeaveBalls".Localized(
                        Utils.Utils.Num2Text((int)player.Balls - 1)));

                context.Write("Items.Reptlum.WormsLeave2".Localized());

                player.RemoveStatusEffect(StatusEffectIds.Infested);

                mutations.Changes += 1;
            }

            var rows = body.GetList<BreastRowPart>();

            if (player.BiggestBreastSize() >= 1 && player.Gender == Gender.Male
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 3))
            {
                if (player.BiggestBreastSize() > 8)
                    context.Write("Items.Reptlum.BreastsVanishHuge".Localized());
                else
                    context.Write("Items.Reptlum.BreastsVanish".Localized());

                context.Write("Items.Reptlum.BreastsVanishEnd".Localized());

                foreach (var row in rows)
                    if (row.BreastRating > 8)
                        row.BreastRating /= 2;
                    else
                        row.BreastRating = 0;

                player.ApplyStats(new DynStat(GameStats.Libido, 2));

                mutations.Changes += 1;
            }

            if (player.BiggestLactation() >= 1 && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 4))
            {
                context.Write(player.TotalNipples() == 2

                    ? "Items.Reptlum.LactationBoth".Localized()

                    : "Items.Reptlum.LactationMany".Localized());

                context.Write("Items.Reptlum.Lactation2".Localized());

                if (player.HasFuckableNipples())
                    context.Write("Items.Reptlum.LactationFluid".Localized());

                context.Write("Items.Reptlum.Lactation3".Localized());

                if (player.HasPerk(PerkIds.Feeder) || player.HasStatusEffect(StatusEffectIds.Feeder))
                {
                    context.Write("Items.Reptlum.FeederLost".Localized());

                    player.RemovePerk(PerkIds.Feeder);

                    player.RemoveStatusEffect(StatusEffectIds.Feeder);
                }

                mutations.Changes += 1;

                foreach (var row in rows)
                    row.Lactation = 0;
            }

            if (player.AverageNipplesPerBreast() > 1 && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 4))
            {
                context.Write("Items.Reptlum.Nipples1".Localized(
                    Appearance.AllBreastsDescript(player)));

                context.Write(player.BiggestBreastSize() < 1

                    ? "Items.Reptlum.NipplesQuoted".Localized()

                    : "Items.Reptlum.NipplesWord".Localized());

                mutations.Changes += 1;

                foreach (var row in rows)
                    row.NipplesPerBreast = 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && rows.Count > 1 && mutations.TfChance(2, 3)
                && !(OptionStore.Instance.Get("hyper", 0) > 0))

                mutations.RemoveExtraBreastRow(tfSource);

            if (player.HasVagina() && mutations.TfChance(3, 5) && player.LizardScore() > 3)
                mutations.UpdateOvipositionPerk(tfSource);

            if (!player.HasDragonHorns(true) && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))
                mutations.GainDraconicHorns(tfSource);

            if (body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 4))
                mutations.RestoreNeck(tfSource);

            if (body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))

                mutations.RestoreRearBody(tfSource);

            if (mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 4))
                mutations.LizardHairChange(tfSource);

            var beard = body.Get<BeardPart>();

            if (player.HasBeard() && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 3))
            {
                context.Write("Items.Reptlum.Beard1".Localized(player.BeardDescript()));

                context.Write("Items.Reptlum.Beard2".Localized());

                beard.Length = 0;

                beard.Style = BeardPart.Normal;
            }

            var lowerBody = body.Get<LowerBodyPart>();

            var arms = body.Get<ArmsPart>();

            var tail = body.Get<TailPart>();

            var eyes = body.Get<EyesPart>();

            var ears = body.Get<EarsPart>();

            var skin = body.Get<SkinPart>();

            var face = body.Get<FacePart>();

            var tongue = body.Get<TonguePart>();

            var underBody = body.Get<UnderBodyPart>();

            if (lowerBody.Id.Path != "lizard" && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))
            {
                if (lowerBody.Id.Path == "hoofed")
                    context.Write("Items.Reptlum.LegsHoof".Localized());
                else if (player.IsTaur())
                    context.Write("Items.Reptlum.LegsTaur".Localized());
                else if (lowerBody.Id.Path is "human" or "dog" or "demonic_high_heels" or "demonic_claws"
                    or "bee" or "cat" or "lizard")

                    context.Write("Items.Reptlum.LegsFeet".Localized());
                else
                    context.Write("Items.Reptlum.LegsOther".Localized());

                context.Write("Items.Reptlum.LegsEnd".Localized());

                lowerBody.Id = new Identifier("coc", "lizard");

                lowerBody.LegCount = 2;

                mutations.Changes += 1;
            }

            if (arms.Id.Path != "lizard" && player.HasReptileScales()
                && lowerBody.Id.Path == "lizard" && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 3))
            {
                arms.Id = new Identifier("coc", "lizard");

                arms.UpdateClaws(new Identifier("coc", "lizard"));

                context.Write("Items.Reptlum.Arms1".Localized(arms.Claws.Tone));

                context.Write("Items.Reptlum.Arms2".Localized());

                mutations.Changes += 1;
            }

            if (arms.Id.Path == "lizard" && player.IsPart<SkinPart>("lizard_scales") && arms.Claws.Id.Path != "lizard"
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 3))
            {
                context.Write("Items.Reptlum.Claws1".Localized());

                arms.UpdateClaws(new Identifier("coc", "lizard"));

                context.Write("Items.Reptlum.Claws2".Localized());

                mutations.Changes += 1;
            }

            if (tail.Id.Path != "lizard" && lowerBody.Id.Path == "lizard"
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))
            {
                context.Write(tail.Id.Path == "none"

                    ? "Items.Reptlum.TailNone".Localized()

                    : "Items.Reptlum.TailOther".Localized());

                tail.Id = new Identifier("coc", "lizard");

                mutations.Changes += 1;
            }

            if (mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5)
                && eyes.Id.Path != "human" && !HasReptileEyes(eyes))
            {
                if (eyes.Id.Path == "black_eyes_sand_trap")
                    context.Write("Items.Reptlum.SandTrapEyes".Localized());
                else
                {
                    context.Write("Items.Reptlum.Eyes1".Localized());

                    if (eyes.Id.Path is "four_spider_eyes" or "spider")
                        context.Write("Items.Reptlum.ArachnidEyes".Localized());

                    context.Write("Items.Reptlum.Eyes2".Localized());
                }

                eyes.Id = new Identifier("coc", "human");

                eyes.Count = 2;

                mutations.Changes += 1;
            }

            if (ears.Id.Path != "lizard" && tail.Id.Path == "lizard" && lowerBody.Id.Path == "lizard"
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))
            {
                context.Write("Items.Reptlum.Ears".Localized());

                ears.Id = new Identifier("coc", "lizard");

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && !player.IsPart<SkinPart>("lizard_scales") && ears.Id.Path == "lizard"
                && tail.Id.Path == "lizard" && lowerBody.Id.Path == "lizard"

                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))
            {
                var tones = mutations.NewLizardSkinTones();

                if (player.HasFur())
                {
                    skin.Tone = tones[0];

                    arms.UpdateClaws(arms.Claws.Id);

                    context.Write("Items.Reptlum.ScalesFur".Localized(skin.FurColor, skin.Tone));
                }
                else if (player.HasScales() && !player.IsPart<SkinPart>("lizard_scales"))
                {
                    context.Write("Items.Reptlum.ScalesOther".Localized());

                    skin.Tone = tones[0];

                    arms.UpdateClaws(arms.Claws.Id);
                }
                else
                {
                    context.Write("Items.Reptlum.ScalesNone".Localized(skin.Tone));

                    skin.Tone = tones[0];

                    arms.UpdateClaws(arms.Claws.Id);
                }

                skin.Id = new Identifier("coc", "lizard_scales");

                skin.Adj = "";

                skin.DescKey = "Skin.Desc.Scales";

                underBody.Id = new Identifier("coc", "reptile");

                underBody.Skin.Id = skin.Id;

                underBody.Skin.Tone = tones[1];

                underBody.Skin.Adj = "";

                underBody.Skin.DescKey = "Skin.Desc.ventral_scales";

                context.Write("Items.Reptlum.ScalesEnd".Localized(skin.Tone, tones[1]));

                mutations.Changes += 1;
            }

            if (mutations.TfNoFur() && face.Id.Path != "lizard" && player.HasReptileScales()
                && ears.Id.Path == "lizard" && tail.Id.Path == "lizard"

                && lowerBody.Id.Path == "lizard" && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(3, 5))
            {
                context.Write("Items.Reptlum.Face".Localized());

                face.Id = new Identifier("coc", "lizard");
            }

            if (tongue.Id.Path == "snake" && mutations.Changes < mutations.ChangeLimit && random.Next(10) < 6)
                mutations.GainLizardTongue();

            if (tongue.Id.Path is not ("lizard" or "snake") && HasReptileFace(player)
                && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 3))

                mutations.GainLizardTongue();

            if (mutations.TfChance(2, 4) && player.HasGills() && mutations.Changes < mutations.ChangeLimit)
                mutations.UpdateGills();

            if (eyes.Id.Path != "lizard" && face.Id.Path == "lizard" && player.HasReptileScales()
                && ears.Id.Path == "lizard" && mutations.Changes < mutations.ChangeLimit && mutations.TfChance(2, 4))
            {
                context.Write(HasReptileEyes(eyes)

                    ? "Items.Reptlum.LizardEyesSlight".Localized()

                    : "Items.Reptlum.LizardEyes".Localized());

                context.Write("Items.Reptlum.LizardEyesEnd".Localized());

                eyes.Id = new Identifier("coc", "lizard");

                mutations.Changes += 1;
            }

            if (mutations.Changes == 0)
            {
                context.Write("Items.Reptlum.FailSafe".Localized());

                player.HealthChange(50);

                player.ApplyStats(new DynStat(GameStats.Lust, 3));
            }

            player.RefillHunger(20);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }

        private static int FindFirstCockNotOfType(Player player, string type)
        {
            var cocks = player.Body.GetList<CockPart>();

            for (var index = 0; index < cocks.Count; index++)
                if (cocks[index].Id.Path != type)
                    return index;

            return -1;
        }

        private static bool HasReptileEyes(EyesPart eyes) =>

            eyes.Id.Path is "lizard" or "dragon" or "basilisk";

        private static bool HasReptileFace(Player player) =>

            player.Body.Get<FacePart>().Id.Path is "lizard" or "dragon" or "basilisk" or "snake_fangs";
    }
}