using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<BunnyPregnancyDelivery>("coc:delivery_bunny")]
    public class BunnyPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) =>

            context.Anal ? UpdateAnal(context) : UpdateVaginal(context);

        public void Deliver(PregnancyContext context)
        {

            if (context.Anal)
                return;

            var player = context.Player;

            var noFur = !player.HasFur();

            Scenes.Scene.ShowImage("birth-bunny");

            PregnancyUtils.CreateVaginaIfMissing(context);

            context.Write("Pregnancy.Bunny.BirthPart1".Localized(
                player.Legs(), player.ButtDescript(), player.VaginaDescript(0)));

            var capacity = player.VaginalCapacity();

            context.Write("Pregnancy.Bunny.BirthPart2".Localized(
                capacity < 60

                    ? "Pregnancy.Bunny.CervixHurts".Localized()

                    : "Pregnancy.Bunny.CervixPleasant".Localized(),

                player.VaginaDescript(0),

                player.ChestDesc()));

            context.Write("Pregnancy.Bunny.BirthPart3".Localized(
                noFur ? "ordinary human babies" : "miniature people",

                noFur ? "fuzzy ears and tails" : "ears, tails, and fuzzy legs",

                player.ChestDesc(),

                player.LactationQ() > 500 ? "Pregnancy.Bunny.GrowsFat".Localized() : ""));

            context.Write("Pregnancy.Bunny.BirthPart4".Localized(player.VaginaDescript(0)));

            player.CuntChange(60, true, true, false);

            player.BoostLactation(0.01f);

            if (player.VaginalCapacity() < 300)
            {
                if (!player.HasStatusEffect(StatusEffectIds.BonusVCapacity))
                    player.CreateStatusEffect(StatusEffectIds.BonusVCapacity, 0, 0, 0, 0);

                player.ChangeStatusValue(StatusEffectIds.BonusVCapacity, 0, 10);
            }

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Libido, 1f),

                new DynStat(GameStats.Sensitivity, 10f),

                new DynStat(GameStats.Corruption, -2f));
        }

        public static bool UpdateVaginal(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            if (context.Incubation == 800)
            {
                context.Write("Pregnancy.Bunny.WombGurgles".Localized());

                displayed = true;
            }

            if (context.Incubation is 785 or 765)
            {
                ApplyNeonPinkEgg(context, player);

                context.Write("[pg]");

                displayed = true;
            }

            if (context.Incubation == 776)
            {
                context.Write("Pregnancy.Bunny.WombFull".Localized());

                displayed = true;
            }

            if (context.Incubation <= 745 && context.Incubation > 400)
            {
                context.Write("Pregnancy.Bunny.EggsDissolved".Localized());

                player.ClearPregnancy();

                displayed = true;
            }

            if (context.Incubation == 198)
            {
                context.Write("Pregnancy.Bunny.BellySlightlyLarger".Localized());

                displayed = true;
            }

            if (context.Incubation == 178)
            {
                context.Write("Pregnancy.Mouse.BellyDistended".Localized());

                displayed = true;
            }

            if (context.Incubation == 156)
            {
                var text = "Pregnancy.Mouse.BellyUnmistakable".Localized()

                    + CorBranch(
                        "Pregnancy.Mouse.JojoLowCor".Localized(),

                        "Pregnancy.Bunny.MidCor".Localized(),

                        "Pregnancy.Bunny.HighCor".Localized())

                    + "[pg]";

                context.Write(text);

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1f),

                    new DynStat(GameStats.Sensitivity, 1f),

                    new DynStat(GameStats.Lust, 2f));

                displayed = true;
            }

            if (context.Incubation == 140)
            {
                context.Write("Pregnancy.Bunny.DoubleKick".Localized());

                displayed = true;
            }

            if (context.Incubation == 120)
            {
                context.Write("Pregnancy.Imp.EverGrowing".Localized());

                displayed = true;
            }

            if (context.Incubation == 72)
            {
                var text = "Pregnancy.Imp.PainfullyDistended".Localized()

                    + CorBranch(
                        "Pregnancy.Mouse.HardToFunction".Localized(),

                        "Pregnancy.Mouse.WonderHowLong".Localized(),

                        "Pregnancy.Bunny.EagerToBirth".Localized())

                    + "[pg]";

                context.Write(text);

                player.ApplyStats(
                    new DynStat(GameStats.Libido, 1f),

                    new DynStat(GameStats.Sensitivity, 1f),

                    new DynStat(GameStats.Lust, 4f));

                displayed = true;
            }

            if (context.Incubation == 48)
            {
                var text = "Pregnancy.Mouse.RubBelly".Localized()

                    + CorBranch(
                        "Pregnancy.Mouse.Disgusted".Localized(),

                        "Pregnancy.Mouse.FewDays".Localized(),

                        "Pregnancy.Bunny.Daydream".Localized())

                    + "[pg]";

                context.Write(text);

                displayed = true;
            }

            return displayed;
        }

        public static bool UpdateAnal(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            if (context.Incubation == 800)
            {
                context.Write("Pregnancy.Bunny.GutGurgles".Localized());

                displayed = true;
            }

            if (context.Incubation is 785 or 765)
            {
                ApplyNeonPinkEgg(context, player);

                context.Write("[pg]");

                displayed = true;
            }

            if (context.Incubation == 776)
            {
                context.Write("Pregnancy.Bunny.GutFull".Localized());

                displayed = true;
            }

            if (context.Incubation == 745)
            {
                context.Write("Pregnancy.Bunny.AnalEggsDissolved".Localized());

                player.ClearButtPregnancy();

                displayed = true;
            }

            return displayed;
        }

        public static void ApplyNeonPinkEgg(PregnancyContext context, Player player)
        {
            var item = Items.ItemRegistry.Create(ConsumableIds.NephilaPinkEgg);

            if (item is Items.Consumable consumable)
                consumable.UseItem(new Items.Effects.ItemUseContext(
                    item, player, context.Write, Random.Shared));
        }

        public static string CorBranch(string low, string mid, string high)
        {
            var cor = CoC.Instance.Player.Corruption;

            if (cor < 40)
                return low;

            return cor < 75 ? mid : high;
        }
    }
}