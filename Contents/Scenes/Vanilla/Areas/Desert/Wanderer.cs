using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<Wanderer>("coc:wanderer")]
    public class Wanderer : Scene
    {

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            WandererRouter();
        }

        public void WandererRouter()
        {

            if (!Player.HasStatusEffect(StatusEffectIds.MeetWanderer))
            {
                WandererFirstMeeting();

                Player.CreateStatusEffect(StatusEffectIds.MeetWanderer, 0, 0, 0, 0);
            }
            else
            {
                if (Player.HasStatusEffect(StatusEffectIds.WandererDemon))
                {
                    WandererDemonEpilogue();

                    return ;
                }

                if (Player.HasStatusEffect(StatusEffectIds.WandererHuman))
                {
                    WandererEpilogueHuman();

                    return ;
                }

                WandererRepeatMeeting();
            }
        }

        public void WandererFirstMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Wanderer.WandererFirstMeeting.DustPlume".Localized());

            OutputText("Scenes.Wanderer.WandererFirstMeeting.MarcusWheelbarrow".Localized());

            OutputText("Scenes.Wanderer.WandererFirstMeeting.PairClosesIn".Localized());

            OutputText("Scenes.Wanderer.WandererFirstMeeting.ShockingCargo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.HelpHim".Localized(), WandererHelpHim);

            AddButton(14, "Common.Button.Leave".Localized(), WandererLeave);
        }

        public void WandererLeave()
        {
            ClearOutput();

            OutputText("Scenes.Wanderer.WandererLeave.MarcusWaddlesAway".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void WandererRepeatMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Wanderer.WandererRepeatMeeting.MarcusWaves".Localized());

            OutputText("Scenes.Wanderer.WandererRepeatMeeting.LuciaSnorts".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), WandererHelpHim);

            AddButton(14, "Common.Button.Leave".Localized(), WandererLeave);
        }

        public void WandererHelpHim()
        {
            ClearOutput();

            OutputText("Scenes.Wanderer.WandererHelpHim.ExplainsTask".Localized());

            OutputText("Scenes.Wanderer.WandererHelpHim.GlancesFearful".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GoDemon".Localized(), WandererGoDemon);

            AddButton(1, "Common.Button.StayHuman".Localized(), WandererStayHuman);
        }

        public void WandererStayHuman()
        {
            ClearOutput();

            OutputText("Scenes.Wanderer.WandererStayHuman.LuciaScreams".Localized());

            OutputText("Scenes.Wanderer.WandererStayHuman.MarcusSoulReason".Localized());

            OutputText("Scenes.Wanderer.WandererStayHuman.LuciaPouts".Localized());

            OutputText("Scenes.Wanderer.WandererStayHuman.MarcusSighsSmile".Localized());

            OutputText("Scenes.Wanderer.WandererStayHuman.LuciaScowls".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Lust, 1f), new DynStat(GameStats.Corruption, -5f));

            Player.CreateStatusEffect(StatusEffectIds.WandererHuman, 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void WandererGoDemon()
        {
            ClearOutput();

            OutputText("Scenes.Wanderer.WandererGoDemon.LuciaMischief".Localized());

            OutputText("Scenes.Wanderer.WandererGoDemon.MarcusEyebrow".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f), new DynStat(GameStats.Corruption, 1f));

            Player.CreateStatusEffect(StatusEffectIds.WandererDemon, 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void WandererDemonEpilogue()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.WandererDemon))
                if (Player.StatusEffectValue(StatusEffectIds.WandererDemon, 0) == 0)
                {
                    OutputText("Scenes.Wanderer.WandererDemonEpilogue.WingedShadow".Localized());

                    OutputText("Scenes.Wanderer.WandererDemonEpilogue.LuciaThanks".Localized());

                    OutputText("Scenes.Wanderer.WandererDemonEpilogue.BottleGift".Localized());

                    OutputText("Scenes.Wanderer.WandererDemonEpilogue.BlowsKissWings".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                    Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SuccubusDelight), 1);

ReturnToCamp(1);

                    Player.StatusEffectByType(StatusEffectIds.WandererDemon);
                }
                else
                    if (Player.StatusEffectValue(StatusEffectIds.WandererDemon, 0) == 1)
                    {
                        OutputText("Scenes.Wanderer.WandererDemonEpilogue.LuciaZipsFrigging".Localized());

                        if (50 < (Player.Speed + Random.Shared.Next(60)))
                        {
                            OutputText("Scenes.Wanderer.WandererDemonEpilogue.CatchesPotion".Localized());

                            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SuccubusDelight), 1);

ReturnToCamp(1);
                        }
                        else
                        {
                            OutputText("Scenes.Wanderer.WandererDemonEpilogue.BottleShatters".Localized());

                            DoNext(() => ReturnToCamp(1));
                        }
                    }
        }

        public void WandererEpilogueHuman()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.WandererHuman))
                if (Player.StatusEffectValue(StatusEffectIds.WandererHuman, 0) == 0)
                {
                    OutputText("Scenes.Wanderer.WandererEpilogueHuman.TwinFigures".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Wanderer.WandererEpilogueHuman.DuckBehindDune".Localized());
                    else
                        if (Player.Corruption < 66)
                            OutputText("Scenes.Wanderer.WandererEpilogueHuman.LuciaWinks".Localized());
                        else
                            OutputText("Scenes.Wanderer.WandererEpilogueHuman.LeersWhistles".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                    Player.StatusEffectByType(StatusEffectIds.WandererHuman);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (Player.StatusEffectValue(StatusEffectIds.WandererHuman, 0) == 1)
                    {
                        OutputText("Scenes.Wanderer.WandererEpilogueHuman.BottleInSand".Localized());

                        OutputText("Scenes.Wanderer.WandererEpilogueHuman.ReadsNote".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SuccubusDelight), 1);

ReturnToCamp(1);
                    }
        }

    }
}