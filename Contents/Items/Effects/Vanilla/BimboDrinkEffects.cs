using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 BimboChampagne.hx ──────────────────────────────────────────
    public class BimboChampagneEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var isBimbo = (player.HasPerk(PerkIds.FutaFaculties) && player.HasPerk(PerkIds.FutaForm))

                || (player.HasPerk(PerkIds.BimboBody) && player.HasPerk(PerkIds.BimboBrains));

            var effect = player.StatusEffectByType(StatusEffectIds.BimboChampagne);

            if (isBimbo)
                context.Write("Items.BimboChampagne.NoEffect".Localized());
            else if (effect == null)
                context.Write("Items.BimboChampagne.FirstDrink".Localized());
            else
            {
                context.Write("Items.BimboChampagne.DeeperMindset".Localized());

                context.Write("Items.BimboChampagne.MoaningLewdly".Localized());
            }

            if (effect != null)
            {
                effect.Values[0] += 4;

                player.ApplyStats(
                    new DynStat(GameStats.Speed, -2),

                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Lust, 10));

                return;
            }

            effect = player.CreateStatusEffect(StatusEffectIds.BimboChampagne, 8, 0, 0, 0);

            var rows = player.Body.GetList<BreastRowPart>();

            if (rows.Count > 0 && rows[0].BreastRating < 5)
            {
                context.Write("Items.BimboChampagne.BoobiesGrow".Localized());

                effect!.Values[1] = 5 - player.BiggestBreastSize();

                rows[0].BreastRating = 5;
            }

            if (!player.HasVagina())
            {
                player.CreateVagina();

                context.Write("Items.BimboChampagne.GrowPussy1".Localized());

                context.Write(player.HasCock()

                    ? "Items.BimboChampagne.GrowPussyCock".Localized()

                    : "Items.BimboChampagne.GrowPussyBlank".Localized());

                context.Write("Items.BimboChampagne.GrowPussy2".Localized());

                effect!.Values[2] = 1;
            }

            var butt = player.Body.Get<ButtPart>();

            if (butt.Rating < 12)
            {
                context.Write("Items.BimboChampagne.ButtGrows".Localized());

                effect!.Values[3] = 12 - butt.Rating;

                butt.Rating = 12;

                var hips = player.Body.Get<HipsPart>();

                if (hips.Rating < 12)
                {
                    effect.Data["hipRatingChange"] = 12 - hips.Rating;

                    hips.Rating = 12;
                }
            }

            player.ApplyStats(
                new DynStat(GameStats.Speed, -10),

                new DynStat(GameStats.Libido, 1),

                new DynStat(GameStats.Lust, 25));
        }

        // 原版 BimboChampagne.hx:78 removeBimboChampagne()
        public static void Remove(Player player, Action<string> write)
        {
            var effect = player.StatusEffectByType(StatusEffectIds.BimboChampagne);

            if (effect == null)
                return;

            write("Items.BimboChampagne.WearingOff".Localized());

            player.ApplyStats(
                new DynStat(GameStats.Speed, 10),

                new DynStat(GameStats.Libido, -1));

            var rows = player.Body.GetList<BreastRowPart>();

            if (effect.Values[1] > 0)
            {
                if (rows.Count > 0)
                    rows[0].BreastRating -= effect.Values[1];

                write("Items.BimboChampagne.BreastsShrink".Localized());
            }

            if (effect.Values[2] > 0)
            {
                write("Items.BimboChampagne.PussySeals".Localized());

                player.RemoveVagina();
            }

            if (effect.Values[3] > 0)
            {
                write("Items.BimboChampagne.ButtShrinks".Localized());

                player.Body.Get<ButtPart>().Rating -= effect.Values[3];
            }

            var hipChange = effect.Data.TryGetValue("hipRatingChange", out var hipValue)

                ? (float)(double)hipValue

                : 0f;

            player.Body.Get<HipsPart>().Rating -= hipChange;

            player.RemoveStatusEffect(StatusEffectIds.BimboChampagne);

            write("Items.BimboChampagne.NewLine".Localized());
        }
    }

    // ── 原版 BlackCatBeer.hx ────────────────────────────────────────────
    public class BlackCatBeerEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            context.Write("Items.BlackCatBeer.Drink1".Localized());

            context.Write("Items.BlackCatBeer.Drink2".Localized());

            context.Write("Items.BlackCatBeer.Drink3".Localized());

            player.HealthChange(40 + random.Next(21));

            var maxLib = player.MaxValue(GameStats.Libido);

            var lib = maxLib - player.Libido >= 10 ? 10 : maxLib - player.Libido;

            if (!player.HasStatusEffect(StatusEffectIds.BlackCatBeer))
            {
                player.CreateStatusEffect(StatusEffectIds.BlackCatBeer, 8, lib, 0, 0);

                player.ApplyStats(
                    new DynStat(GameStats.Speed, -5),

                    new DynStat(GameStats.Intelligence, -5),

                    new DynStat(GameStats.Libido, lib),

                    new DynStat(GameStats.Lust, 20 + random.Next((int)player.Libido / 4)));
            }
            else
            {
                player.ChangeStatusValue(StatusEffectIds.BlackCatBeer, 0, 4);

                player.ChangeStatusValue(StatusEffectIds.BlackCatBeer, 1, lib);

                context.Write("Items.BlackCatBeer.ExtraDrink".Localized());

                player.ApplyStats(
                    new DynStat(GameStats.Speed, -1),

                    new DynStat(GameStats.Intelligence, -1),

                    new DynStat(GameStats.Libido, lib),

                    new DynStat(GameStats.Lust, 30 + random.Next((int)player.Libido / 4)));
            }

            player.SlimeFeed();
        }

        // 原版 BlackCatBeer.hx:41 blackCatBeerExpires()
        public static void Expire(Player player, Action<string> write)
        {
            var effect = player.StatusEffectByType(StatusEffectIds.BlackCatBeer);

            if (effect == null)
                return;

            player.ApplyStats(
                new DynStat(GameStats.Speed, 4.5f),

                new DynStat(GameStats.Intelligence, 4.5f),

                new DynStat(GameStats.Libido, -effect.Values[1]));

            player.RemoveStatusEffect(StatusEffectIds.BlackCatBeer);

            write("Items.BlackCatBeer.Expires".Localized());
        }
    }
}