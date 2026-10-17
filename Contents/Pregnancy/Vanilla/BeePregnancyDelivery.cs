using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<BeePregnancyDelivery>("coc:delivery_bee")]
    public class BeePregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            if (context.Incubation == 36)
            {
                context.Write("Pregnancy.Bee.Bloated".Localized());

                displayed = true;
            }

            if (context.Incubation == 20)
            {
                var text = "Pregnancy.Bee.HoneyDrips".Localized();

                if (player.CockTotal() > 0)
                    text += "Pregnancy.Bee.AromaPermeates".Localized();

                if (player.CockTotal() == 1)
                    text += "Pregnancy.Bee.SingleCock".Localized();
                else if (player.CockTotal() > 1)
                    text += "Pregnancy.Bee.MultiCock".Localized(player.MultiCockDescriptLight());

                text += "Pregnancy.Bee.CantStopMasturbating".Localized();

                context.Write(text);

                player.ApplyStats(
                    new DynStat(GameStats.Intelligence, -0.5f),

                    new DynStat(GameStats.Lust, 500f));

                displayed = true;
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            context.Write("[pg]");

            Scenes.Scene.ShowImage("birth-beegirl");

            var text = "Pregnancy.Bee.GushOfFluids".Localized();

            if (player.CockTotal() > 0)
                text += "Pregnancy.Bee.EquipmentHard".Localized();

            if (player.Vaginas.Count > 0)
                text += "Pregnancy.Bee.VaginaEngorged".Localized(player.VaginaDescript(0));

            text += "Pregnancy.Bee.HandDartsDown".Localized();

            text += "Pregnancy.Bee.SuddenSlip".Localized();

            if (player.AverageLactation() >= 1 && player.BiggestBreastSize() > 2)
                text += "Pregnancy.Bee.MassageHoney".Localized(player.BreastDescript(0));

            text += "Pregnancy.Bee.EachEggCloser".Localized();

            text += "Pregnancy.Bee.MindReturns".Localized();

            text += CorBranch(
                "Pregnancy.Bee.Disgusted".Localized(),

                "Pregnancy.Bee.Amazed".Localized(),

                "Pregnancy.Bee.Languid".Localized(),

                "Pregnancy.Bee.LickLips".Localized());

            context.Write(text);

            player.Orgasm("Anal");

            player.ApplyStats(
                new DynStat(GameStats.Intelligence, 1f),

                new DynStat(GameStats.Libido, 4f),

                new DynStat(GameStats.Sensitivity, 3f));

            if (player.ButtChange(20, true))
                context.Write("[pg]");

            var butt = player.Body.Get<ButtPart>();

            if ((butt.Rating < 17) && (butt.Rating < 13 || Random.Shared.Next(2) == 0))
                {
                    butt.Rating += 1;

                    context.Write("Pregnancy.Bee.ButtGrew".Localized(player.ButtDescript()));
                }

            context.Write("[pg]");
        }

        public static string CorBranch(string low, string mid, string high, string top)
        {
            var cor = CoC.Instance.Player.Corruption;

            if (cor < 35)
                return low;

            if (cor < 65)
                return mid;

            return cor < 90 ? high : top;
        }
    }
}