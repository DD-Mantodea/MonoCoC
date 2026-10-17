using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<OviElixirPregnancyDelivery>("coc:delivery_ovi_elixir")]
    public class OviElixirPregnancyDelivery : IPregnancyDelivery
    {

        public const int LoosenessLoose = 2;

        public const int LoosenessGapingWide = 4;

        public bool Update(PregnancyContext context) => !VaginaCheck(context);

        public static bool VaginaCheck(PregnancyContext context)
        {
            var player = context.Player;

            if (player.Vaginas.Count > 0)
                return true;

            player.RemoveStatusEffect(StatusEffectIds.Eggs);

            context.Write("Pregnancy.Ovi.BellyShrinks".Localized());

            player.ClearPregnancy();

            return false;
        }

        public void Deliver(PregnancyContext context)
        {
            if (!VaginaCheck(context))
                return;

            var player = context.Player;

            var flags = CoC.Instance.Flags;

            var gainedOviPerk = false;

            var overdose = flags.Get(GameFlagIds.OvimaxOverdose);

            if (!player.HasPerk(PerkIds.Oviposition) && overdose > 0 && Random.Shared.Next(3) < overdose)
            {
                context.Write("Pregnancy.Ovi.OvipositionAwakens".Localized());

                gainedOviPerk = true;
            }

            flags.Set(GameFlagIds.OvimaxOverdose, 0);

            var eggSize = player.StatusEffectValue(StatusEffectIds.Eggs, 1);

            var eggColor = player.StatusEffectValue(StatusEffectIds.Eggs, 0);

            var eggCount = player.StatusEffectValue(StatusEffectIds.Eggs, 2);

            if (eggSize == 0)
            {
                if (eggCount < 10)
                    WriteLightEggs(context, player);
                else
                    WriteManyEggs(context, player);
            }
            else
                WriteLargeEggs(context, player);

            if (gainedOviPerk)
            {
                context.Write("Pregnancy.Ovi.PerkGained".Localized());

                player.CreatePerk(PerkIds.Oviposition, 0, 0, 0, 0);
            }

            context.Write("Pregnancy.Ovi.LeaveEggsBehind".Localized());

            context.Write("Pregnancy.Ovi.NotFertile".Localized());

            player.CreateStatusEffect(StatusEffectIds.LootEgg, 0, 0, 0, 0);

            player.ClearPregnancy();
        }

        public static void WriteLightEggs(PregnancyContext context, Player player)
        {
            context.Write("Pregnancy.Ovi.LightEggsPart1".Localized());

            player.CuntChange(10, true);

            context.Write("Pregnancy.Ovi.LightEggsPart2".Localized());

            context.Write(EggDescript(player));

            context.Write("Pregnancy.Ovi.LightEggsPart3".Localized());

            player.ApplyStats(
                new DynStatsOptions(Scale: false),

                new DynStat(GameStats.Lust, player.MaxLust, DynStatOp.Eq));
        }

        public static void WriteManyEggs(PregnancyContext context, Player player)
        {
            context.Write("Pregnancy.Ovi.ManyEggsPart1".Localized(player.ArmorName, player.VaginaDescript(0)));

            player.CuntChange(10, true);

            context.Write("Pregnancy.Ovi.ManyEggsPart2".Localized());

            var clit = player.GetClitLength();

            if (clit > 5)
                context.Write("Pregnancy.Ovi.ManyEggsHugeClit".Localized());
            else if (clit > 2)
                context.Write("Pregnancy.Ovi.ManyEggsMedClit".Localized());
            else
                context.Write("Pregnancy.Ovi.ManyEggsSmallClit".Localized());

            context.Write("Pregnancy.Ovi.ManyEggsPart3".Localized(EggDescript(player)));

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStatsOptions(Scale: false));
        }

        public static void WriteLargeEggs(PregnancyContext context, Player player)
        {
            context.Write("Pregnancy.Ovi.LargeEggsPart1".Localized(player.ArmorName));

            var looseness = player.Vaginas.Count > 0 ? player.Vaginas[0].Looseness : 0;

            if (looseness < LoosenessLoose)
                context.Write("Pregnancy.Ovi.LargeTight".Localized());
            else if (looseness <= LoosenessGapingWide)
                context.Write("Pregnancy.Ovi.LargeLoose".Localized());
            else
                context.Write("Pregnancy.Ovi.LargeGaping".Localized());

            context.Write("Pregnancy.Ovi.LargeEggsPart2".Localized());

            var clit = player.GetClitLength();

            if (clit > 5)
                context.Write("Pregnancy.Ovi.LargeHugeClit".Localized());
            else if (clit > 2)
                context.Write("Pregnancy.Ovi.LargeMedClit".Localized());
            else
                context.Write("Pregnancy.Ovi.LargeSmallClit".Localized(player.VaginaDescript(0)));

            context.Write("Pregnancy.Ovi.LargeEggsPart3".Localized());

            player.CuntChange(20, true);

            if (player.StatusEffectValue(StatusEffectIds.Eggs, 2) >= 11)
                context.Write("Pregnancy.Ovi.LargeEggsPart4".Localized(player.VaginaDescript(0)));

            context.Write("Pregnancy.Ovi.LargeEggsPart5".Localized(EggDescript(player)));

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStatsOptions(Scale: false));
        }

        public static string EggDescript(Player player)
        {
            if (!player.HasStatusEffect(StatusEffectIds.Eggs))
                return "EGG ERRORZ";

            var count = (int)player.StatusEffectValue(StatusEffectIds.Eggs, 2);

            var size = player.StatusEffectValue(StatusEffectIds.Eggs, 1);

            var color = player.StatusEffectValue(StatusEffectIds.Eggs, 0);

            var text = NumberUtils.Num2Text(count) + " ";

            if (size == 1)
                text += "Pregnancy.Ovi.SizeLarge".Localized();

            text += color switch
            {
                0 => "Pregnancy.Ovi.ColorBrown".Localized(),

                1 => "Pregnancy.Ovi.ColorPurple".Localized(),

                2 => "Pregnancy.Ovi.ColorBlue".Localized(),

                3 => "Pregnancy.Ovi.ColorPink".Localized(),

                4 => "Pregnancy.Ovi.ColorWhite".Localized(),

                5 => "Pregnancy.Ovi.ColorRubberyBlack".Localized(),

                _ => ""
            };

            text += "Pregnancy.Ovi.EggWord".Localized();

            return text;
        }
    }
}