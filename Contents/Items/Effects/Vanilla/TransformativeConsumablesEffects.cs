using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    // ── 原版 Ectoplasm.hx ───────────────────────────────────────────────
    public class EctoplasmEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var mutations = Mutations.Mutations.Instance;

            var random = context.Random;

            player.SlimeFeed();

            context.Write("Items.Ectoplasm.Drink".Localized());

            mutations.InitTransformation([2, 3]);

            var int100 = 100f * player.Intelligence / player.MaxValue(GameStats.Intelligence);

            if (int100 < 100 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Ectoplasm.HigherIntelligence".Localized());

                player.ApplyStats(new DynStat(GameStats.Intelligence, 1));

                if (int100 < 50)
                    player.ApplyStats(new DynStat(GameStats.Intelligence, 1));
            }

            if (player.SensitivityPercent >= 20 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write("Items.Ectoplasm.LowerSensitivity".Localized());

                player.ApplyStats(new DynStat(GameStats.Sensitivity, -2));

                if (player.SensitivityPercent >= 75)
                    player.ApplyStats(new DynStat(GameStats.Sensitivity, -2));
            }

            if (player.LibidoPercent < 100 && random.Next(3) == 0 && mutations.Changes < mutations.ChangeLimit)
            {
                context.Write(player.LibidoPercent < 50

                    ? "Items.Ectoplasm.HigherLibidoLow".Localized()

                    : "Items.Ectoplasm.HigherLibidoHigh".Localized());

                player.ApplyStats(new DynStat(GameStats.Libido, 1));

                if (player.LibidoPercent < 50)
                    player.ApplyStats(new DynStat(GameStats.Libido, 1));
            }

            if (player.HasCock() && mutations.Changes < mutations.ChangeLimit)
            {
                var cocks = player.Body.GetList<CockPart>();

                if (random.Next(3) == 0 && cocks.Count > 0 && cocks[0].Id.Path != "human")
                {
                    context.Write("Items.Ectoplasm.HumanCock".Localized());

                    cocks[0].Id = new Identifier("coc", "human");

                    mutations.Changes += 1;
                }
            }

            if (player.Body.Get<NeckPart>().Id.Path != "normal" && mutations.Changes < mutations.ChangeLimit && random.Next(4) == 0)
                mutations.RestoreNeck("ectoplasm");

            if (player.Body.Get<RearBodyPart>().Id != BodyPartTypes.SharkFin && mutations.Changes < mutations.ChangeLimit && random.Next(5) == 0)
                mutations.RestoreRearBody("ectoplasm");

            if (random.Next(5) == 0)
                mutations.UpdateOvipositionPerk("ectoplasm");

            var hair = player.Body.Get<HairPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && hair.Id.Path != "ghost")
            {
                context.Write("Items.Ectoplasm.GhostHair".Localized());

                hair.Id = new Identifier("coc", "ghost");

                mutations.Changes += 1;
            }

            var skin = player.Body.Get<SkinPart>();

            if (random.Next(4) == 0 && mutations.Changes < mutations.ChangeLimit && skin.Tone != "sable" && skin.Tone != "white")
            {
                if (random.Next(2) == 0)
                {
                    context.Write("Items.Ectoplasm.WhiteSkin".Localized());

                    skin.Tone = "white";

                    skin.Adj = "milky";

                    skin.DescKey = "Skin.Desc.skin";

                    skin.Id = new Identifier("coc", "plain");
                }
                else
                {
                    context.Write("Items.Ectoplasm.BlackSkin".Localized());

                    skin.Tone = "sable";

                    skin.Adj = "ashen";

                    skin.DescKey = "Skin.Desc.skin";

                    skin.Id = new Identifier("coc", "plain");
                }

                player.Body.Get<UnderBodyPart>().Restore();

                var arms = player.Body.Get<ArmsPart>();

                arms.UpdateClaws(arms.Claws.Id);

                mutations.Changes += 1;
            }

            var lowerBody = player.Body.Get<LowerBodyPart>();

            if (player.HasPerk(PerkIds.Incorporeality) && !lowerBody.Incorporeal)
                lowerBody.Incorporeal = true;

            if (mutations.Changes < mutations.ChangeLimit
                && !player.HasPerk(PerkIds.Incorporeality)

                && (skin.Tone == "white" || skin.Tone == "sable")

                && hair.Id.Path == "ghost")
            {
                context.Write("Items.Ectoplasm.GhostLegs".Localized());

                context.Write("Items.Ectoplasm.IncorporealityPerk".Localized());

                player.CreatePerk(PerkIds.Incorporeality, 0, 0, 0, 0);

                lowerBody.Incorporeal = true;
            }

            if (mutations.Changes == 0)
            {
                context.Write("Items.Ectoplasm.Refreshed".Localized());

                player.HealthChange(player.Level * 5 + 10);
            }

            player.RefillHunger(20);

            CoC.Instance.Flags.Set(GameFlagIds.TimesTransformed,

                CoC.Instance.Flags.Get(GameFlagIds.TimesTransformed) + mutations.Changes);
        }
    }

    // ── 原版 TatteredScroll.hx ──────────────────────────────────────────
    public class TatteredScrollEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            var random = context.Random;

            var hair = player.Body.Get<HairPart>();

            var rows = player.Body.GetList<BreastRowPart>();

            context.Write("Items.TatteredScroll.Read".Localized());

            if (hair.Color == "sandy blonde")
            {
                context.Write("Items.TatteredScroll.SandyBlondeRead".Localized());

                if (rows.Count == 0 || player.BiggestBreastSize() == 0)
                {
                    context.Write("Items.TatteredScroll.GrowBreasts".Localized());

                    if (rows.Count == 0)
                        player.CreateBreastRow();

                    rows = player.Body.GetList<BreastRowPart>();

                    if (rows.Count > 0)
                    {
                        rows[0].Breasts = 2;

                        rows[0].BreastRating = 3;

                        rows[0].NipplesPerBreast = Math.Max(1, rows[0].NipplesPerBreast);
                    }

                    player.ApplyStats(
                        new DynStat(GameStats.Sensitivity, 2),

                        new DynStat(GameStats.Lust, 1));
                }

                if (player.BiggestBreastSize() >= 1 && player.BiggestBreastSize() < 3)
                {
                    context.Write("Items.TatteredScroll.BalloonOut".Localized());

                    if (rows.Count > 0)
                        rows[0].BreastRating = 3;

                    player.ApplyStats(
                        new DynStat(GameStats.Sensitivity, 1),

                        new DynStat(GameStats.Lust, 1));
                }

                if (player.AverageNipplesPerBreast() < 1)
                {
                    context.Write("Items.TatteredScroll.GrowNipples".Localized());

                    foreach (var row in rows)
                    {
                        if (row.NipplesPerBreast < 1)
                            player.NippleLength = 0.2f;

                        row.NipplesPerBreast = 1;
                    }

                    player.ApplyStats(
                        new DynStat(GameStats.Sensitivity, 2),

                        new DynStat(GameStats.Lust, 1));
                }

                var lactation = player.BiggestLactation();

                if (lactation > 0)
                {
                    context.Write("Items.TatteredScroll.LactationIntro".Localized());

                    if (lactation < 2)
                        context.Write("Items.TatteredScroll.LactationLow".Localized());

                    if (lactation >= 2 && lactation <= 2.6f)
                        context.Write("Items.TatteredScroll.LactationMid".Localized());

                    if (lactation > 2.6f && lactation < 3)
                        context.Write("Items.TatteredScroll.LactationHigh".Localized());

                    if (lactation >= 3)
                        context.Write("Items.TatteredScroll.LactationMax".Localized());

                    if (lactation < 3)
                    {
                        player.BoostLactation(0.7f);

                        context.Write("Items.TatteredScroll.LactationFuller".Localized());
                    }

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1),

                        new DynStat(GameStats.Sensitivity, 4),

                        new DynStat(GameStats.Lust, 15));
                }

                if (player.BiggestLactation() == 0)
                {
                    context.Write("Items.TatteredScroll.StartLactation".Localized());

                    player.BoostLactation(1);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 0.5f),

                        new DynStat(GameStats.Sensitivity, 1),

                        new DynStat(GameStats.Lust, 10));
                }

                context.Write("Items.TatteredScroll.VoiceEnd".Localized());
            }
            else
            {
                context.Write("Items.TatteredScroll.NormalRead".Localized());

                context.Write("Items.TatteredScroll.HairBecomesSandy".Localized());

                hair.Color = "sandy blonde";

                context.Write("Items.TatteredScroll.VoiceEnd".Localized());
            }

            var inCombat = CombatController.Instance?.Context != null;

            if (!inCombat)
            {
                context.Write("Items.TatteredScroll.SandWitch".Localized());

                player.Orgasm("Generic");

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1),

                    new DynStat(GameStats.Sensitivity, 5));

                player.SlimeFeed();
            }

            if (!inCombat)
            {
                Scene.View.Time.PassTime(1f);

                Scene.Go<CampScene>();
            }
        }
    }
}