using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 KitsuneGift.hx ─────────────────────────────────────────────
    public class KitsuneGiftEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            context.Write("Items.KitsuneGift.Open".Localized());

            switch (random.Next(14))
            {
                case 0:
                    context.Write("Items.KitsuneGift.FoxJewel".Localized());

                    Give(player, new Identifier("coc", "fox_jewel"));

                    break;

                case 1:
                    context.Write("Items.KitsuneGift.FoxBerries".Localized());

                    Give(player, ConsumableIds.FoxBerry);

                    break;

                case 2:
                    {
                        var gems = 2 + random.Next(20);

                        context.Write("Items.KitsuneGift.Gems".Localized(NumberUtils.Num2Text(gems)));

                        player.GainGems(gems);

                        break;
                    }

                case 3:
                    context.Write("Items.KitsuneGift.Tea".Localized());

                    Give(player, ConsumableIds.ScholarTea);

                    break;

                case 4:
                    {
                        Identifier[] dyes =

                        [
                            ConsumableIds.HairDyeRed, ConsumableIds.HairDyeBlond,

                            ConsumableIds.HairDyeBlack, ConsumableIds.HairDyeWhite
                        ];

                        var dye = dyes[random.Next(4)];

                        var item = ItemRegistry.Create(dye);

                        context.Write("Items.KitsuneGift.HairDye".Localized(item.Name));

                        Give(player, dye);

                        break;
                    }

                case 5:
                    context.Write("Items.KitsuneGift.Knowledge".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Intelligence, 4),

                        new DynStat(GameStats.Sensitivity, 2),

                        new DynStat(GameStats.Lust, 10));

                    break;

                case 6:
                    context.Write("Items.KitsuneGift.Thief".Localized());

                    player.LoseGems(2 + random.Next(15));

                    break;

                case 7:
                    context.Write("Items.KitsuneGift.Prank".Localized());

                    player.ApplyStats(new DynStat(GameStats.Lust, -20));

                    Scene.View.Time.PassTime(1f);

                    break;

                case 8:
                    context.Write("Items.KitsuneGift.Aphrodisiac".Localized());

                    player.ApplyStats(new DynStatsOptions(Scale: false),

                        new DynStat(GameStats.Lust, 100));

                    break;

                case 9:
                    context.Write("Items.KitsuneGift.Wither".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Strength, -5),

                        new DynStat(GameStats.Toughness, -5));

                    break;

                case 10:
                    context.Write("Items.KitsuneGift.Dud".Localized());

                    break;

                case 11:
                    context.Write("Items.KitsuneGift.Chitin".Localized());

                    Give(player, new Identifier("coc", "black_chitin"));

                    break;

                case 12:
                    context.Write("Items.KitsuneGift.Lollipop".Localized());

                    Give(player, new Identifier("coc", "lolipop"));

                    break;

                case 13:
                    context.Write("Items.KitsuneGift.MindKitsune".Localized());

                    KitsuneScene.Save.SetBool("hadVision", true);

                    player.CreateStatusEffect(StatusEffectIds.KitsuneVision, 0, 0, 0, 0);

                    break;
            }
        }

        private static void Give(Player player, Identifier id) =>

            player.Inventory.Add(ItemRegistry.Create(id), 1);
    }

    // ── 原版 ShriveledTentacle.hx ──────────────────────────────────────
    public class DryTentacleEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            var flags = CoC.Instance.Flags;

            mutations.InitTransformation([2, 3]);

            context.Write("Items.DryTentacle.Chew".Localized());

            var tou100 = 100f * player.Toughness / player.MaxValue(GameStats.Toughness);

            var spe100 = 100f * player.Speed / player.MaxValue(GameStats.Speed);

            if (random.Next(3) == 0 && tou100 < 50 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.DryTentacle.Tougher".Localized());

                player.ApplyStats(
                    new DynStat(GameStats.Toughness, 1),

                    new DynStat(GameStats.Sensitivity, -1));
            }

            if (random.Next(3) == 0 && spe100 > 40 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.DryTentacle.Slower".Localized());

                player.ApplyStats(new DynStat(GameStats.Speed, -1));
            }

            if (random.Next(3) == 0 && player.Corruption < 20 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.DryTentacle.ColdShiver".Localized());

                mutations.Changes += 1;

                player.ApplyStats(new DynStat(GameStats.Corruption, 1));
            }

            if (player.Body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("shriveledTentacle");

            if (player.Body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)

                mutations.RestoreRearBody("shriveledTentacle");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("shriveledTentacle");

            var tail = player.Body.Get<TailPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && tail.Id.Path == "bee_abdomen")
            {
                context.Write("Items.DryTentacle.BeeAbdomenGone".Localized());

                tail.Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            var wings = player.Body.Get<WingsPart>();

            if (random.Next(4) == 0
                && (wings.Id.Path == "bee_like_small" || wings.Id.Path == "bee_like_large")

                && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.DryTentacle.BeeWingsGone".Localized());

                wings.Id = BodyPartTypes.None;

                mutations.Changes += 1;
            }

            var hair = player.Body.Get<HairPart>();

            var gills = player.Body.Get<GillsPart>();

            if (gills.Id.Path == "anemone" && hair.Id.Path != "anemone"
                && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)
            {
                context.Write("Items.DryTentacle.TentacleHair".Localized());

                hair.Id = new Identifier("coc", "anemone");

                hair.Length = 5;

                if (flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) == 0)
                {
                    context.Write("Items.DryTentacle.HairStopped".Localized());

                    flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 1);
                }

                mutations.Changes += 3;
            }

            if (random.Next(5) == 0 && gills.Id.Path != "anemone"
                && player.Body.Get<SkinPart>().Tone == "aphotic blue-black"

                && mutations.Changes < mutations.ChangeLimit)

                mutations.UpdateGills(1); // 1 = 海葵鳃

            if (random.Next(5) == 0 && mutations.Changes < mutations.ChangeLimit
                && player.Body.Get<SkinPart>().Tone != "aphotic blue-black")
            {
                context.Write("Items.DryTentacle.AphoticSkin".Localized());

                player.Body.Get<SkinPart>().Tone = "aphotic blue-black";

                var arms = player.Body.Get<ArmsPart>();

                arms.UpdateClaws(arms.Claws.Id);

                mutations.Changes += 1;
            }

            if (hair.Id.Path == "anemone" && hair.Length < 36 && random.Next(2) == 0
                && mutations.Changes < mutations.ChangeLimit)
            {
                var temp = 5 + random.Next(3);

                hair.Length += temp;

                context.Write("Items.DryTentacle.HairLonger".Localized(NumberUtils.Num2Text(temp)));

                mutations.Changes += 1;
            }

            player.RefillHunger(20);

            flags.Set(GameFlagIds.TimesTransformed, flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }

    // ── 原版 SuperHummus.hx ────────────────────────────────────────────
    public class SuperHummusEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var body = player.Body;

            context.Write("Items.SuperHummus.Eat".Localized());

            player.RefillHunger(100);

            player.Strength = 30;

            player.Speed = 30;

            player.Toughness = 30;

            player.Intelligence = 30;

            player.Sensitivity = 20;

            player.Libido = 25;

            player.Corruption = 5;

            player.Lust = 10;

            body.Get<HairPart>().Id = new Identifier("coc", "normal");

            context.Write(player.HumanScore() > 4

                ? "Items.SuperHummus.MoreYourself".Localized()

                : "Items.SuperHummus.BodyMorphing".Localized());

            body.Get<ArmsPart>().Restore();

            var eyes = body.Get<EyesPart>();

            eyes.Id = new Identifier("coc", "human");

            eyes.Count = 2;

            body.Get<AntennaePart>().Id = BodyPartTypes.None;

            body.Get<FacePart>().Id = new Identifier("coc", "human");

            var lowerBody = body.Get<LowerBodyPart>();

            lowerBody.Id = new Identifier("coc", "human");

            lowerBody.LegCount = 2;

            body.Get<WingsPart>().Id = BodyPartTypes.None;

            body.Get<TailPart>().Id = BodyPartTypes.None;

            body.Get<TonguePart>().Id = new Identifier("coc", "human");

            body.Get<TailPart>().Recharge = 0;

            var horns = body.Get<HornsPart>();

            horns.Value = 0;

            horns.Id = BodyPartTypes.None;

            body.Get<EarsPart>().Id = new Identifier("coc", "human");

            var skin = body.Get<SkinPart>();

            skin.Id = new Identifier("coc", "plain");

            skin.DescKey = "Skin.Desc.skin";

            skin.Adj = "";

            body.Get<UnderBodyPart>().Restore();

            body.Get<NeckPart>().Restore();

            body.Get<RearBodyPart>().Restore();

            body.Get<TonguePart>().Id = new Identifier("coc", "human");

            if (player.Fertility > 15)
                player.Fertility = 15;

            if (player.CumMultiplier > 50)
                player.CumMultiplier = 50;

            var virgin = false;

            var cocks = body.GetList<CockPart>();

            while (cocks.Count > 0)

                player.RemoveCock(0, 1);

            cocks = body.GetList<CockPart>();

            if (player.Gender is Gender.Male or Gender.Hermaphrodite)
            {
                player.CreateCock(6, 1);

                cocks = body.GetList<CockPart>();

                if (cocks.Count > 0)
                {
                    cocks[0].Length = 6;

                    cocks[0].Thickness = 1;
                }

                player.BallSize = 2;

                if (player.Balls > 2)
                    player.Balls = 2;
            }
            else
            {
                player.Balls = 0;

                player.BallSize = 2;
            }

            var vaginas = body.GetList<VaginaPart>();

            while (vaginas.Count > 0)
            {
                virgin = vaginas[0].Virgin;

                player.RemoveVagina(0, 1);
            }

            // 原版 gender >= 2 指（原版枚举）雌雄同体或无性别
            if (player.Gender is Gender.Hermaphrodite or Gender.Genderless)
            {
                player.CreateVagina(virgin);

                vaginas = body.GetList<VaginaPart>();

                if (vaginas.Count > 0)
                    vaginas[0].Virgin = virgin;
            }

            if (body.GetList<VaginaPart>().Count > 0)
                body.GetList<VaginaPart>()[0].ClitLength = 0.25f;

            var butt = body.Get<ButtPart>();

            butt.Rating = 2;

            body.Get<HipsPart>().Rating = 2;

            var anal = body.Get<AnalPart>();

            if (anal.Looseness > 1)
                anal.Looseness = 1;

            if (anal.Wetness > 1)
                anal.Wetness = 1;

            var rows = body.GetList<BreastRowPart>();

            rows.Clear();

            player.CreateBreastRow();

            player.NippleLength = 0.25f;

            rows = body.GetList<BreastRowPart>();

            if (rows.Count > 0)

                rows[0].BreastRating = player.Gender == Gender.Genderless ? 2 : 0; // 原版此处用 gender > 2（原版枚举里只有无性别会走这个分支），照原样保留

            body.Get<GillsPart>().Id = BodyPartTypes.None;

            player.RemoveStatusEffect(StatusEffectIds.Uniball);

            player.RemoveStatusEffect(StatusEffectIds.BlackNipples);

            Mutations.Mutations.Instance.UpdateOvipositionPerk("superHummus");
        }
    }
}