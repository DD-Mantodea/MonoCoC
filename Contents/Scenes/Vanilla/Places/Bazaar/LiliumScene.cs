using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<LiliumScene>("coc:lilium")]
    public class LiliumScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Bazaar.Lilium);

        public string debugName => "Lilium";

        public string debugHint => "";

        public override void Entry()
        {
            ConvertOldFlag();
        }

        public void Reset()
        {

            Save.Clear();
        }

        public void ConvertOldFlag()
        {

            var OLD_LILIUM_TIMES_TALKED = new Flag<int>("old_lilium_times_talked");

            if (CoC.Instance.Flags.Get(OLD_LILIUM_TIMES_TALKED) > 0)
            {
                Save.SetBool("hasMet", true);

                Save.SetBool("hasPaid", true);

                Save.SetBool("learnedName", true);

                CoC.Instance.Flags.Set(OLD_LILIUM_TIMES_TALKED, 0);
            }
        }

        public string GetButtonName()
        {

            ConvertOldFlag();

            return Save.GetBool("learnedName") ? "Lilium" : "Demon";
        }

        public Action? GetButtonFunc()
        {

            if (View.Time.Hours >= 17)
            {
                if (!Save.GetBool("learnedName"))
                    OutputText("Scenes.Lilium.GetButtonFunc.PaleStranger".Localized());
                else
                    OutputText("Scenes.Lilium.GetButtonFunc.LiliumStanding".Localized());

                return ApproachLilium;
            }

            return null;
        }

        public void RegisterMacros()
        {

            RegisterMacro("cock", () => Player.CockDescript(ChooseCock()));

            RegisterMacro("s", Player.CockTotal() >= 2);

            RegisterMacro("learnedname", Save.GetBool("learnedName"));

            RegisterMacro("haspaid", Save.GetBool("hasPaid"));
        }

        public int ChooseCock()
        {

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            return x;
        }

        public void ApproachLilium()
        {
            ClearOutput();

            RegisterMacros();

            var liliumDesc = "Her two nearly foot-long horns grow up and out of her forehead, one of which is adorned with a shiny, silver band. Her dark hair spills across her bare shoulders in loose coils and her black lipstick and heavy eyeshadow contrast with the paleness of her skin. A black underbust corset wraps around her waist, leaving her breasts exposed, and long black gloves cling to her arms up to her shoulders. She also wears a crinoline skirt with an opening at the front which reveals her smooth bare legs, a tail which wraps around her thigh like a garter, and her crotch, which sports an average cock curiously clad in a lacy sock in spite of her otherwise brazen exposure.";

            if (!Save.GetBool("hasMet"))
            {
                OutputText("Scenes.Lilium.ApproachLilium.ApproachesDemon".Localized(
                     liliumDesc
                ));

                OutputText("Scenes.Lilium.ApproachLilium.AssessesYou".Localized());

                OutputText("Scenes.Lilium.ApproachLilium.CourseOfCourse".Localized());

                OutputText("Scenes.Lilium.ApproachLilium.SweepsArms".Localized());

                Save.SetBool("hasMet", true);
            }
            else
            {
                OutputText("Scenes.Lilium.ApproachLilium.StandsBefore".Localized(
                     liliumDesc
                ));

                OutputText("Scenes.Lilium.ApproachLilium.TeasesBut".Localized());
            }

            if (Player.Gems < 50)
                OutputText("Scenes.Lilium.ApproachLilium.BackAgain".Localized());

            ClearButtons();

            if (Save.GetBool("learnedName"))
                PayButtons();
            else
                AddNextButton("Common.Button.Pay".Localized(), PayForLilium);

            AddButton(14, "Common.Button.Leave".Localized(), () => LeaveLilium());
        }

        public void PayButtons()
        {

            AddNextButton("Common.Button.ButtFuck".Localized(), () => PayAndThen(ButtFuckTongueJeorb));

            AddNextButton("Common.Button.RideHer".Localized(), () => PayAndThen(LiliumDickRidah));

            AddNextButton("Common.Button.NippleFuck".Localized(), () => PayAndThen(LiliumNippleFuck));
        }

        public void PayForLilium()
        {
            ClearOutput();

            if (Player.IsGenderless())
            {
                OutputText("Scenes.Lilium.PayForLilium.FollowsWoman".Localized());

                OutputText("Scenes.Lilium.PayForLilium.LooksUp".Localized());

                OutputText("Scenes.Lilium.PayForLilium.WalksOff".Localized());

                OutputText("Scenes.Lilium.PayForLilium.BummerWord".Localized());

                Go<BazaarScene>();

                return ;
            }

            OutputText("Scenes.Lilium.PayForLilium.CantDenyIntrigue".Localized());

            OutputText("Scenes.Lilium.PayForLilium.TheseNipples".Localized());

            OutputText("Scenes.Lilium.PayForLilium.WhatWillDo".Localized());

            Save.SetBool("learnedName", true);

            ClearButtons();

            PayButtons();

            AddButton(14, "Common.Button.Back".Localized(), () => Nevermind());
        }

        public void Nevermind()
        {
            ClearOutput();

            OutputText("Scenes.Lilium.NeverMind.ClinkingStops".Localized());

            Go<BazaarScene>();
        }

        public void PayAndThen(Action scene)
        {

            Save.SetBool("hasPaid", true);

            Player.Gems -= 50;

            Go<BazaarScene>();
        }

        public void LeaveLilium()
        {
            ClearOutput();

            if (!Save.GetBool("learnedName"))
                OutputText("Scenes.Lilium.LeaveLilium.PassesOffer".Localized());
            else
                OutputText("Scenes.Lilium.LeaveLilium.PassingThrough".Localized());

            Go<BazaarScene>();
        }

        public void ButtFuckTongueJeorb()
        {
            ClearOutput();

            float length = Player.Cocks[ChooseCock()].Length;

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.FindsSecluded".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.SquatsTraces".Localized(
                 MeasurementsUtils.ShortSuffix(length)
            ));

            if (length > 8)
                OutputText("Scenes.Lilium.ButtFuckTongueJeorb.FeelsSmaller".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.EachThrust".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.OneLastTime".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.LipsPart".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.EstablishesRhythm".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Lilium.ButtFuckTongueJeorb.TingleEmanates".Localized());

                if (Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Lilium.ButtFuckTongueJeorb.MilkStream".Localized());
            }

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.BreathingPace".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.PantingGrabs".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Lilium.ButtFuckTongueJeorb.HaventStrength".Localized());
            else
                OutputText("Scenes.Lilium.ButtFuckTongueJeorb.WobblingLegs".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.LetsLaugh".Localized());

            OutputText("Scenes.Lilium.ButtFuckTongueJeorb.BothRecover".Localized());

            Player.Orgasm("Lips");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void LiliumDickRidah()
        {
            ClearOutput();

            OutputText("Scenes.Lilium.LiliumDickRidah.TakesSecluded".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.PushesDown".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.MovesDown".Localized());

            if (Player.Body.Get<HairPart>().Length >= 15)
                OutputText("Scenes.Lilium.LiliumDickRidah.ShuddersSlightly".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.CirclesHer".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.DrawsUp".Localized());

            if (Player.VaginalCapacity() < 25)
                OutputText("Scenes.Lilium.LiliumDickRidah.LowersGingerly".Localized());
            else
                OutputText("Scenes.Lilium.LiliumDickRidah.PlungesDown".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.PullsUp".Localized());

            Player.CuntChange(14, true, true, false);

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Lilium.LiliumDickRidah.TurnsAttention".Localized());

                if (Player.HasLongTongue())
                    OutputText("Scenes.Lilium.LiliumDickRidah.TongueExtends".Localized());
                else
                    OutputText("Scenes.Lilium.LiliumDickRidah.SqueezesBetween".Localized());

                if (Player.BiggestLactation() > 2)
                    OutputText("Scenes.Lilium.LiliumDickRidah.PowerfulStream".Localized());
                else
                    if (Player.BiggestLactation() >= 1)
                        OutputText("Scenes.Lilium.LiliumDickRidah.SmallStream".Localized());
            }

            OutputText("Scenes.Lilium.LiliumDickRidah.FingersInside".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.NotFinished".Localized());

            if (Player.GetClitLength() < 2)
                OutputText("Scenes.Lilium.LiliumDickRidah.OneFinger".Localized());
            else
                OutputText("Scenes.Lilium.LiliumDickRidah.GraspsTenderly".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.PleasureTingle".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.FlopsSide".Localized());

            OutputText("Scenes.Lilium.LiliumDickRidah.MomentRecovery".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14, 61);

            DoNext(() => ReturnToCamp(1));
        }

        public void LiliumNippleFuck()
        {
            ClearOutput();

            OutputText("Scenes.Lilium.LiliumNippleFuck.TakesSecluded".Localized());

            OutputText("Scenes.Lilium.LiliumNippleFuck.RubsGently".Localized());

            if (Player.Cocks[ChooseCock()].isPierced)
                OutputText("Scenes.Lilium.LiliumNippleFuck.SlightBump".Localized());
            else
                OutputText("Scenes.Lilium.LiliumNippleFuck.TongueReachesEnd".Localized());

            OutputText("Scenes.Lilium.LiliumNippleFuck.TakesTress".Localized());

            OutputText("Scenes.Lilium.LiliumNippleFuck.NotContent".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Lilium.LiliumNippleFuck.ThinkingUnfair".Localized());
            else
                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic || Player.IsNaga())
                    OutputText("Scenes.Lilium.LiliumNippleFuck.ThinkingUnfairAgain".Localized());

            OutputText("Scenes.Lilium.LiliumNippleFuck.GivesUpSteadying".Localized());

            if (Player.CumQ() >= 700 && Player.CockTotal() > 1)
                OutputText("Scenes.Lilium.LiliumNippleFuck.RiversCum".Localized());

            OutputText("Scenes.Lilium.LiliumNippleFuck.PullingOut".Localized());

            OutputText("Scenes.Lilium.LiliumNippleFuck.GetsDressed".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

    }
}