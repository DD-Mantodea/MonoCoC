using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 BimboLiqueur.hx ────────────────────────────────────────────
    public class BimboLiqueurEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var body = player.Body;

            // 原版 canUse()：已经有 Futa Form 时拒绝饮用；物品使用前已被扣除，这里退回
            if (player.HasPerk(PerkIds.FutaForm))
            {
                context.Write("Items.BimboLiqueur.Refuse".Localized());

                if (context.Item != null)
                    player.Inventory.Add(context.Item, 1);

                return;
            }

            var rows = body.GetList<BreastRowPart>();

            if (player.HasPerk(PerkIds.BroBody))
            {
                context.Write("Items.BimboLiqueur.BroWince".Localized());

                if (player.Tallness < 77)
                {
                    context.Write("Items.BimboLiqueur.BroTaller".Localized());

                    player.Tallness = 77;
                }

                if (player.BiggestBreastSize() < 7)
                {
                    context.Write(player.BiggestBreastSize() < 1

                        ? "Items.BimboLiqueur.BroChestItch".Localized()

                        : "Items.BimboLiqueur.BroBreastTingle".Localized());

                    context.Write("Items.BimboLiqueur.BroBreastGrow".Localized());

                    if (rows.Count > 0)
                        rows[0].BreastRating = 7;
                }

                if (!player.HasVagina())
                {
                    context.Write("Items.BimboLiqueur.BroVagina1".Localized());

                    context.Write(player.Balls > 0

                        ? "Items.BimboLiqueur.BroVaginaBehindBalls".Localized()

                        : "Items.BimboLiqueur.BroVaginaGroin".Localized());

                    context.Write("Items.BimboLiqueur.BroVagina2".Localized());

                    if (body.Get<HipsPart>().Rating < 12 || body.Get<ButtPart>().Rating < 12)
                        context.Write("Items.BimboLiqueur.BroVaginaProportions".Localized());

                    player.CreateVagina();

                    if (player.Body.GetList<VaginaPart>().Count > 0)
                        player.Body.GetList<VaginaPart>()[0].ClitLength = 0.25f;

                    if (body.Get<HipsPart>().Rating < 12)
                        body.Get<HipsPart>().Rating = 12;

                    if (body.Get<ButtPart>().Rating < 12)
                        body.Get<ButtPart>().Rating = 12;
                }

                context.Write("Items.BimboLiqueur.BroFace".Localized());

                if (player.Femininity < 80)
                    player.Femininity = 80;

                context.Write("Items.BimboLiqueur.BroLibido".Localized());

                context.Write(player.HasPerk(PerkIds.BroBrains)

                    ? "Items.BimboLiqueur.BroPerksLostBoth".Localized()

                    : "Items.BimboLiqueur.BroPerksLostOne".Localized());

                context.Write("Items.BimboLiqueur.BroPerksGained".Localized());

                player.RemovePerk(PerkIds.BroBody);

                player.RemovePerk(PerkIds.BroBrains);

                player.CreatePerk(PerkIds.FutaFaculties, 0, 0, 0, 0);

                player.CreatePerk(PerkIds.FutaForm, 0, 0, 0, 0);

                if (player.Intelligence > 35)
                {
                    player.Intelligence = 35;

                    player.ApplyStats(new DynStat(GameStats.Intelligence, -0.1f));
                }

                if (player.Libido < 50)
                {
                    player.Libido = 50;

                    player.ApplyStats(new DynStat(GameStats.Libido, 0.1f));
                }

                return;
            }

            context.Write("Items.BimboLiqueur.DrinkIntro".Localized());

            var hair = body.Get<HairPart>();

            if (hair.Color != "platinum blonde")
            {
                context.Write("Items.BimboLiqueur.HairIntro1".Localized());

                if (hair.Length < 36)
                {
                    context.Write("Items.BimboLiqueur.HairGrow".Localized());

                    hair.Length = 36;
                }
                else
                    context.Write("Items.BimboLiqueur.HairThicken".Localized());

                context.Write("Items.BimboLiqueur.HairBlonde".Localized());

                hair.Color = "platinum blonde";
            }

            context.Write("Items.BimboLiqueur.Moaning".Localized());

            var vaginas = body.GetList<VaginaPart>();

            if (!player.HasVagina())
            {
                player.CreateVagina();

                vaginas = body.GetList<VaginaPart>();

                if (vaginas.Count > 0)
                {
                    vaginas[0].ClitLength = 0.25f;

                    vaginas[0].Wetness = 3;
                }

                context.Write(player.IsTaur()

                    ? "Items.BimboLiqueur.NewVaginaTaur".Localized()

                    : "Items.BimboLiqueur.NewVagina".Localized());
            }
            else
                if (player.IsTaur())
                {
                    context.Write("Items.BimboLiqueur.VaginaTaur".Localized());

                    if (vaginas.Count > 0 && vaginas[0].Wetness < 3)
                        vaginas[0].Wetness = 3;
                }
                else
                {
                    context.Write("Items.BimboLiqueur.VaginaSink1".Localized());

                    if (vaginas.Count > 0 && vaginas[0].Wetness < 3)
                    {
                        context.Write("Items.BimboLiqueur.VaginaNow".Localized());

                        if (vaginas.Count > 0)
                            vaginas[0].Wetness = 3;
                    }

                    context.Write("Items.BimboLiqueur.VaginaSink2".Localized());
                }

            if (player.Balls > 0)
            {
                context.Write("Items.BimboLiqueur.BallsRecede".Localized());

                player.Balls = 0;

                player.BallSize = 3;

                player.CumMultiplier = 2;
            }

            if (player.HasCock())
            {
                context.Write("Items.BimboLiqueur.CocksMelt".Localized());

                var cocks = body.GetList<CockPart>();

                context.Write(cocks.Count == 1

                    ? "Items.BimboLiqueur.CockSingularEnd".Localized()

                    : "Items.BimboLiqueur.CockPluralEnd".Localized());

                context.Write("Items.BimboLiqueur.CocksEnd".Localized());

                while (cocks.Count > 0)

                    player.RemoveCock(0, 1);
            }

            context.Write("Items.BimboLiqueur.Seduce".Localized());

            context.Write("Items.BimboLiqueur.Giggle".Localized());

            context.Write("Items.BimboLiqueur.SighNipples".Localized());

            rows = body.GetList<BreastRowPart>();

            if (rows.Count > 0)
            {
                if (rows[0].BreastRating < 10)
                    context.Write("Items.BimboLiqueur.BreastsGrowSmall".Localized());
                else
                    context.Write("Items.BimboLiqueur.BreastsGrowBig".Localized());

                rows[0].BreastRating += 5 + random.Next(5);
            }

            player.ApplyStats(new DynStat(GameStats.Sensitivity, 20));

            context.Write("Items.BimboLiqueur.Fingers1".Localized());

            if (vaginas.Count > 0)
                context.Write(player.VaginaDescript(0));
            else
                context.Write("Items.BimboLiqueur.FingersFallback".Localized());

            context.Write("Items.BimboLiqueur.Fingers2".Localized());

            context.Write(vaginas.Count > 0 && vaginas[0].ClitLength >= 3

                ? "Items.BimboLiqueur.ClitJerk".Localized()

                : "Items.BimboLiqueur.ClitCaress".Localized());

            context.Write("Items.BimboLiqueur.Fingers3".Localized());

            context.Write("Items.BimboLiqueur.Recover".Localized());

            var hips = body.Get<HipsPart>();

            var butt = body.Get<ButtPart>();

            if (hips.Rating < 12 || butt.Rating < 12)
            {
                context.Write("Items.BimboLiqueur.BalanceShift".Localized());

                if (hips.Rating < 12 && butt.Rating < 12)
                {
                    context.Write("Items.BimboLiqueur.BothGrow".Localized());

                    butt.Rating = 12;

                    hips.Rating = 12;
                }
                else if (hips.Rating < 12)
                {
                    context.Write("Items.BimboLiqueur.HipsGrow".Localized());

                    hips.Rating = 12;
                }
                else
                {
                    context.Write("Items.BimboLiqueur.ButtGrow".Localized());

                    butt.Rating = 12;
                }

                context.Write("Items.BimboLiqueur.NewLine".Localized());
            }

            if (player.Tone > 0)
            {
                context.Write("Items.BimboLiqueur.MusclesVanish".Localized());

                player.Tone = 0;

                var str100 = player.StrengthPercent;

                if (str100 >= 30)
                {
                    if (str100 >= 90)
                        player.ApplyStats(new DynStat(GameStats.Strength, -10));

                    if (str100 >= 70)
                        player.ApplyStats(new DynStat(GameStats.Strength, -10));

                    if (str100 >= 50)
                        player.ApplyStats(new DynStat(GameStats.Strength, -10));

                    player.ApplyStats(new DynStat(GameStats.Strength, -5));

                    context.Write("Items.BimboLiqueur.StrengthLoss".Localized());
                }

                context.Write("Items.BimboLiqueur.NewLine".Localized());
            }

            if (!player.HasPerk(PerkIds.BimboBody))
            {
                context.Write("Items.BimboLiqueur.BimboBodyPerk".Localized());

                player.CreatePerk(PerkIds.BimboBody, 0, 0, 0, 0);
            }

            if (!player.HasPerk(PerkIds.BimboBrains))
            {
                context.Write("Items.BimboLiqueur.BimboBrainsPerk".Localized());

                player.CreatePerk(PerkIds.BimboBrains, 0, 0, 0, 0);

                if (player.Intelligence > 21)
                    player.Intelligence = 21 + player.Intelligence / 5;
            }

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Intelligence, -1),

                new DynStat(GameStats.Libido, 4),

                new DynStat(GameStats.Sensitivity, 25));

            player.ModFemGoal(100, 100);
        }
    }
}