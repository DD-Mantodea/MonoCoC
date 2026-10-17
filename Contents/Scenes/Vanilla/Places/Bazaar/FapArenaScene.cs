using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<FapArenaScene>("coc:fap_arena")]
    public class FapArenaScene : Scene
    {

        public override void Entry()
        {
            FapAppearance();
        }

        public void FapAppearance(bool display = true)
        {

            if (display)
                if (CoC.Instance.Flags.Get(GameFlagIds.FapArenaRulesExplained) == 0)
                    OutputText("Scenes.Faparena.FapAppearance.GiganticTent".Localized());
                else
                    OutputText("Scenes.Faparena.FapAppearance.LargeCrowd".Localized());
        }

        public void FapArenaGOOOO()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (CoC.Instance.Flags.Get(GameFlagIds.FapArenaRulesExplained) == 0)
                OutputText("Scenes.Faparena.FapArenaGOOOO.ApproachTent".Localized());

            OutputText("Scenes.Faparena.FapArenaGOOOO.LineAdvances".Localized());

            if ((Player.BiggestCockLength() < 8 && !(OptionStore.Instance.Get("lowStandards", 0) > 0)) || !Player.HasCock())
            {
                OutputText("Scenes.Faparena.FapArenaGOOOO.NotWellEndowed".Localized());

                if (!Player.HasCock())
                    OutputText("Scenes.Faparena.FapArenaGOOOO.YouHaveCock".Localized());
                else
                    OutputText("Scenes.Faparena.FapArenaGOOOO.SufficientlyLong".Localized());

                OutputText("Scenes.Faparena.FapArenaGOOOO.SayEnd".Localized());

                Go<BazaarScene>();

                return ;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FapArenaRulesExplained) == 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.FapArenaRulesExplained, CoC.Instance.Flags.Get(GameFlagIds.FapArenaRulesExplained) + 1);

                    OutputText("Scenes.Faparena.FapArenaGOOOO.NewHere".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.MeeklyFollowing".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.BitPuzzled".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Faparena.FapArenaGOOOO.TapsShoulder".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.SameTone".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.DemonessCenter".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.SaysReady".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.MuffledChuckles".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.CarriesOn".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.ConsideredCumming".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.LoserPlaces".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.EveryoneNods".Localized());

                    if (Player.Corruption > 66)
                        OutputText("Scenes.Faparena.FapArenaGOOOO.SoundsFun".Localized());
                    else
                        OutputText("Scenes.Faparena.FapArenaGOOOO.ByGods".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.DoYouStay".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), FapArenaPageII);

                    AddButton(1, "Common.Button.No".Localized(), () => Go<BazaarScene>());
                }
                else
                {
                    OutputText("Scenes.Faparena.FapArenaGOOOO.WelcomeBack".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.DoAsTold".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.AlrightWelcome".Localized());

                    OutputText("Scenes.Faparena.FapArenaGOOOO.EveryoneNodsAgain".Localized());

                    if (Player.Corruption > 66)
                        OutputText("Scenes.Faparena.FapArenaGOOOO.LookForward".Localized());

                    DoNext(() => FapArenaPageII());
                }
        }

        public void FapArenaPageII()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FapArenaSessions, CoC.Instance.Flags.Get(GameFlagIds.FapArenaSessions) + 1);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Faparena.FapArenaPageII.ReadyShout".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Faparena.FapArenaPageII.EagerlySeize".Localized());
            else
                if (Player.Corruption > 33)
                    OutputText("Scenes.Faparena.FapArenaPageII.NervouslyGrab".Localized());
                else
                {
                    OutputText("Scenes.Faparena.FapArenaPageII.HesitantlyPick".Localized());

                    OutputText("Scenes.Faparena.FapArenaPageII.CantHelpStart".Localized());
                }

            OutputText("Scenes.Faparena.FapArenaPageII.SettleLegs".Localized());

            OutputText("Scenes.Faparena.FapArenaPageII.GoShout".Localized());

            OutputText("Scenes.Faparena.FapArenaPageII.InstantlyWince".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Faparena.FapArenaPageII.StrokingVigorously".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Faparena.FapArenaPageII.PaceAccelerates".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Faparena.FapArenaPageII.OtherWord".Localized());

            OutputText("Scenes.Faparena.FapArenaPageII.DickGirls".Localized());

            var s = Player.Sensitivity;

            var d = Player.HoursSinceCum;

            float c = 0;

            if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                c = 10;
            else
                if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                    c = 5;
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Lizard)
                        c = 3;
                    else
                        if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                            c = 2;

            var r = Player.Lust / Player.MaxLust;

            float n = Random.Shared.Next(100);

            var st = c + (n - s * r - 4 * d) / (1.2f + (d / 10));

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                st = 31;

            if (Player.Cocks[x].Id == BodyPartTypes.Anemone && st <= 0)
                st = 1;

            if (st <= 0)
                FapResults(3);
            else
                if (st < 29.5f)
                    FapResults(2);
                else
                    FapResults(1);
        }

        public void FapResults(float place = 3)
        {
            ClearOutput();

            int temp = 0;

            var x = Player.BiggestCockIndex();

            float num = Random.Shared.Next(50) + 5;

            var tent = false;

            if (place == 3)
            {
                OutputText("Scenes.Faparena.FapResults.FocusRightArm".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Faparena.FapResults.DoesntMatter".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                if (Player.CumQ() > 1000)
                    OutputText("Scenes.Faparena.FapResults.UnholyWord".Localized());
                else
                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Faparena.FapResults.EnormousWord".Localized());
                    else
                        OutputText("Scenes.Faparena.FapResults.AppreciableWord".Localized());

                OutputText("Scenes.Faparena.FapResults.CreamiestRope".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Faparena.FapResults.GoutsFollow".Localized());

                OutputText("Scenes.Faparena.FapResults.BodyShivers".Localized());

                OutputText("Scenes.Faparena.FapResults.LeftProfessional".Localized(
                     Player.CockDescript(x),

                     num - 1,

                     Player.CockDescript(x),

                     num
                ));

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Faparena.FapResults.ThighsTense".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Faparena.FapResults.LargePuddleThird".Localized());
                    else
                        if (Player.Wetness() >= 2)
                            OutputText("Scenes.Faparena.FapResults.PuddleWord".Localized());
                        else
                            OutputText("Scenes.Faparena.FapResults.SmearWord".Localized());

                    OutputText("Scenes.Faparena.FapResults.GirlJizzFloor".Localized());
                }

                if (Player.BiggestLactation() > 1.5f)
                    OutputText("Scenes.Faparena.FapResults.MilkyFlood".Localized(
                         Appearance.AllBreastsDescript(Player)
                    ));

                OutputText("Scenes.Faparena.FapResults.ParagraphBreak".Localized());

                OutputText("Scenes.Faparena.FapResults.RefereeNods".Localized());

                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Faparena.FapResults.SemenFlies".Localized(
                         num,

                         num + 1
                    ));
                else
                    OutputText("Scenes.Faparena.FapResults.MustMakeCum".Localized(
                         num,

                         num + 1
                    ));

                OutputText("Scenes.Faparena.FapResults.LeftStops".Localized(
                     num - 1
                ));

                OutputText("Scenes.Faparena.FapResults.EveryoneBlows".Localized());
            }
            else
                if (place == 2)
                {
                    var (he, him, his) = Random.Shared.Next(2) == 0 ? (he: "he", him: "him", his: "his") : (he: "she", him: "her", his: "her");

                    OutputText("Scenes.Faparena.FapResults.SeemsLikeHours".Localized());

                    if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                        OutputText("Scenes.Faparena.FapResults.LosesRhythm".Localized(
                             his,

                             his,

                             him,

                             his,

                             him,

                             he
                        ));

                    OutputText("Scenes.Faparena.FapResults.ThoughtsInterrupted".Localized());

                    OutputText("Scenes.Faparena.FapResults.NumberCumming".Localized(
                         num,

                         num - 1
                    ));

                    OutputText("Scenes.Faparena.FapResults.PlayerGaveUp".Localized());

                    OutputText("Scenes.Faparena.FapResults.RightShouts".Localized(
                         num + 5,

                         num + 4
                    ));

                    OutputText("Scenes.Faparena.FapResults.ViciousYank".Localized(
                         Player.CockDescript(x)
                    ));

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Faparena.FapResults.SoakWord".Localized());
                    else
                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Faparena.FapResults.PolluteWord".Localized());
                        else
                            OutputText("Scenes.Faparena.FapResults.WetWord".Localized());

                    OutputText("Scenes.Faparena.FapResults.FloorJuices".Localized(
                         num + 4,

                         Player.CockDescript(x)
                    ));

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Faparena.FapResults.ThighsTenseAgain".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.Wetness() >= 4)
                            OutputText("Scenes.Faparena.FapResults.LargePuddleAgain".Localized());
                        else
                            if (Player.Wetness() >= 2)
                                OutputText("Scenes.Faparena.FapResults.PuddleWordAgain".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.SmearWordAgain".Localized());

                        OutputText("Scenes.Faparena.FapResults.GirlJizzFloorAgain".Localized());
                    }

                    if (Player.BiggestLactation() > 1.5f)
                        OutputText("Scenes.Faparena.FapResults.MilkyFloodAgain".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                    OutputText("Scenes.Faparena.FapResults.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Faparena.FapResults.LeftStopsAgain".Localized(
                         num + 3
                    ));

                    OutputText("Scenes.Faparena.FapResults.EveryoneBlowsAgain".Localized());
                }
                else
                {
                    if (!Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                        OutputText("Scenes.Faparena.FapResults.SeemsHoursAgain".Localized());
                    else
                        OutputText("Scenes.Faparena.FapResults.TeasingTortures".Localized());

                    OutputText("Scenes.Faparena.FapResults.ThoughtsInterruptedAgain".Localized());

                    OutputText("Scenes.Faparena.FapResults.RightShoutsAgain".Localized(
                         num + 1,

                         num
                    ));

                    OutputText("Scenes.Faparena.FapResults.FeelsPressure".Localized());

                    OutputText("Scenes.Faparena.FapResults.NumberCummingAgain".Localized(
                         num + 7,

                         num + 6
                    ));

                    if (!Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                        OutputText("Scenes.Faparena.FapResults.ViciousYankAgain".Localized(
                             Player.CockDescript(x),

                             Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Faparena.FapResults.RationalThoughts".Localized(
                             Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Faparena.FapResults.TorturedLeft".Localized(
                         num - 1
                    ));

                    OutputText("Scenes.Faparena.FapResults.EveryoneBlowsThird".Localized());

                    if (!Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                    {
                        OutputText("Scenes.Faparena.FapResults.KeepTrack".Localized(
                             Player.CockDescript(x)
                        ));

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Faparena.FapResults.SoakWordAgain".Localized());
                        else
                            if (Player.CumQ() >= 500)
                                OutputText("Scenes.Faparena.FapResults.PolluteWordAgain".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.WetWordAgain".Localized());

                        OutputText("Scenes.Faparena.FapResults.FloorJuicesAgain".Localized(
                             num,

                             Player.CockDescript(x)
                        ));

                        if (Player.HasVagina())
                        {
                            OutputText("Scenes.Faparena.FapResults.ThighsTenseThird".Localized(
                                 Player.VaginaDescript(0)
                            ));

                            if (Player.Wetness() >= 4)
                                OutputText("Scenes.Faparena.FapResults.LargePuddleThird".Localized());
                            else
                                if (Player.Wetness() >= 2)
                                    OutputText("Scenes.Faparena.FapResults.PuddleWordThird".Localized());
                                else
                                    OutputText("Scenes.Faparena.FapResults.SmearWordThird".Localized());

                            OutputText("Scenes.Faparena.FapResults.GirlJizzFloorThird".Localized());
                        }

                        if (Player.BiggestLactation() > 1.5f)
                            OutputText("Scenes.Faparena.FapResults.MilkyFloodThird".Localized(
                                 Appearance.AllBreastsDescript(Player)
                            ));
                    }
                    else
                        OutputText("Scenes.Faparena.FapResults.KeepTrackAgain".Localized());
                }

            OutputText("Scenes.Faparena.FapResults.PantsRelax".Localized(
                 Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.FapArenaRulesExplained) == 1)
                OutputText("Scenes.Faparena.FapResults.LittleLost".Localized());
            else
                OutputText("Scenes.Faparena.FapResults.Congratulating".Localized());

            OutputText("Scenes.Faparena.FapResults.BusyCleaning".Localized());

            OutputText("Scenes.Faparena.FapResults.JoltsSurprise".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Faparena.FapResults.YellingWithThem".Localized());
            else
                OutputText("Scenes.Faparena.FapResults.BodyTenses".Localized());

            OutputText("Scenes.Faparena.FapResults.RhythmicShout".Localized());

            if (place <= 1)
            {
                if (!Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                    OutputText("Scenes.Faparena.FapResults.ButtTrainTail".Localized());
                else
                    OutputText("Scenes.Faparena.FapResults.ButtTrainTailAgain".Localized());
            }
            else
                if (place <= 2)
                    OutputText("Scenes.Faparena.FapResults.BetweenTwo".Localized());
                else
                {
                    OutputText("Scenes.Faparena.FapResults.EndOfWagon".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Faparena.FapResults.WhorishSmile".Localized());
                    else
                        OutputText("Scenes.Faparena.FapResults.ApprehensionWord".Localized());

                    OutputText("Scenes.Faparena.FapResults.InFrontOfCreature".Localized());
                }

            OutputText("Scenes.Faparena.FapResults.ReadyYourself".Localized());

            OutputText("Scenes.Faparena.FapResults.ButtTrainBegin".Localized());

            if (place <= 1)
            {
                OutputText("Scenes.Faparena.FapResults.CantBelieve".Localized());

                if (Random.Shared.Next(2) == 0)
                {
                    temp = 3;

                    OutputText("Scenes.Faparena.FapResults.GelatinousCheeks".Localized(
                         Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Faparena.FapResults.MuscularCheeks".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    temp = 1;
                }

                OutputText("Scenes.Faparena.FapResults.LodgesComfortably".Localized());

                if (temp == 1)
                    OutputText("Scenes.Faparena.FapResults.HisWord".Localized());
                else
                    OutputText("Scenes.Faparena.FapResults.HerWord".Localized());

                OutputText("Scenes.Faparena.FapResults.OwnPartner".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                if (!Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                {
                    OutputText("Scenes.Faparena.FapResults.NotGettingPounded".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    TentacleFapSpecial(place);

                    OutputText("Scenes.Faparena.FapResults.FinalPeak".Localized());

                    tent = TentacleFapCum();

                    if ((Player.CockTotal() > 2) || (!tent && Player.CockTotal() > 1))
                    {
                        if (Player.CockTotal() > 3 || (!tent && Player.CockTotal() > 2))
                            OutputText("Scenes.Faparena.FapResults.OtherCocksSpill".Localized());

                        if ((tent && Player.CockTotal() == 3) || (Player.CockTotal() == 2 && !tent))
                            OutputText("Scenes.Faparena.FapResults.OtherCockSpills".Localized());

                        OutputText("Scenes.Faparena.FapResults.AWord".Localized());

                        if (Player.CumQ() > 1000)
                            OutputText("Scenes.Faparena.FapResults.MightyWord".Localized());
                        else
                            if (Player.CumQ() > 500)
                                OutputText("Scenes.Faparena.FapResults.LargeWord".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.GoodWord".Localized());

                        OutputText("Scenes.Faparena.FapResults.SprayOfAgain".Localized());

                        if (Player.CockTotal() > 3 || (!tent && Player.CockTotal() > 2))
                            OutputText("Scenes.Faparena.FapResults.TheirWord".Localized());
                        else
                            OutputText("Scenes.Faparena.FapResults.ItsWord".Localized());

                        OutputText("Scenes.Faparena.FapResults.OwnUnhindered".Localized(
                             Player.CockDescript(x)
                        ));
                    }

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Faparena.FapResults.FeminineSquirts".Localized());

                        if (Player.Wetness() >= 5)
                            OutputText("Scenes.Faparena.FapResults.TitanicWord".Localized());
                        else
                            if (Player.Wetness() >= 3)
                                OutputText("Scenes.Faparena.FapResults.MassiveWord".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.NiceWord".Localized());

                        OutputText("Scenes.Faparena.FapResults.GirlCumLoad".Localized());
                    }

                    OutputText("Scenes.Faparena.FapResults.WincingPain".Localized(
                         Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Faparena.FapResults.FuckTheAss".Localized());

                    OutputText("Scenes.Faparena.FapResults.TrainCollapses".Localized());
                }
            }
            else
                if (place == 2)
                {
                    OutputText("Scenes.Faparena.FapResults.FirmArms".Localized());

                    if (Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Faparena.FapResults.GelatinousCheeksAgain".Localized(
                             Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Faparena.FapResults.MuscularCheeksAgain".Localized(
                             Player.CockDescript(x),

                             Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Faparena.FapResults.MuffledMoan".Localized());

                    Player.ButtChange(5 + Random.Shared.Next(15), true, true, false);

                    OutputText("Scenes.Faparena.FapResults.InstinctivelyReach".Localized(
                         Player.CockDescript(x)
                    ));

                    TentacleFapSpecial(place);

                    OutputText("Scenes.Faparena.FapResults.PressureBuilds".Localized());

                    tent = TentacleFapCum();

                    if ((Player.CockTotal() > 2) || (!tent && Player.CockTotal() > 1))
                    {
                        if (Player.CockTotal() > 3 || (!tent && Player.CockTotal() > 2))
                            OutputText("Scenes.Faparena.FapResults.OtherCocksSpillAgain".Localized());

                        if ((tent && Player.CockTotal() == 3) || (Player.CockTotal() == 2 && !tent))
                            OutputText("Scenes.Faparena.FapResults.OtherCockSpillsAgain".Localized());

                        OutputText("Scenes.Faparena.FapResults.AWordAgain".Localized());

                        if (Player.CumQ() > 1000)
                            OutputText("Scenes.Faparena.FapResults.MightyWordAgain".Localized());
                        else
                            if (Player.CumQ() > 500)
                                OutputText("Scenes.Faparena.FapResults.LargeWordAgain".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.GoodWordAgain".Localized());

                        OutputText("Scenes.Faparena.FapResults.SprayOfAgain".Localized());

                        if (Player.CockTotal() > 3 || (!tent && Player.CockTotal() > 2))
                            OutputText("Scenes.Faparena.FapResults.TheirWordAgain".Localized());
                        else
                            OutputText("Scenes.Faparena.FapResults.ItsWordAgain".Localized());

                        OutputText("Scenes.Faparena.FapResults.OwnUnhinderedAgain".Localized(
                             Player.CockDescript(x)
                        ));
                    }

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Faparena.FapResults.FeminineSquirtsAgain".Localized());

                        if (Player.Wetness() >= 5)
                            OutputText("Scenes.Faparena.FapResults.TitanicWordAgain".Localized());
                        else
                            if (Player.Wetness() >= 3)
                                OutputText("Scenes.Faparena.FapResults.MassiveWordAgain".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.NiceWordAgain".Localized());

                        OutputText("Scenes.Faparena.FapResults.GirlCumLoadAgain".Localized());
                    }

                    OutputText("Scenes.Faparena.FapResults.StrengthFading".Localized(
                         Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Faparena.FapResults.FirmArmsAgain".Localized());

                    OutputText("Scenes.Faparena.FapResults.MuffledMoanAgain".Localized());

                    Player.ButtChange(5 + Random.Shared.Next(10), true, true, false);

                    OutputText("Scenes.Faparena.FapResults.FaceDown".Localized(
                         Player.CockDescript(x),

                         Player.AssDescript()
                    ));

                    TentacleFapSpecial(place);

                    OutputText("Scenes.Faparena.FapResults.ToweringRod".Localized(
                         Player.ButtDescript()
                    ));

                    TentacleFapCum();

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Faparena.FapResults.FeminineSquirtsThird".Localized());

                        if (Player.Wetness() >= 5)
                            OutputText("Scenes.Faparena.FapResults.TitanicWordThird".Localized());
                        else
                            if (Player.Wetness() >= 3)
                                OutputText("Scenes.Faparena.FapResults.MassiveWordThird".Localized());
                            else
                                OutputText("Scenes.Faparena.FapResults.NiceWordThird".Localized());

                        OutputText("Scenes.Faparena.FapResults.GirlCumLoadThird".Localized());
                    }

                    OutputText("Scenes.Faparena.FapResults.FallsOver".Localized());

                    if (Player.Libido > 50)
                        OutputText("Scenes.Faparena.FapResults.LosingNotBad".Localized());

                    OutputText("Scenes.Faparena.FapResults.ParagraphBreakThird".Localized());
                }

            if (place <= 1)
            {
                OutputText("Scenes.Faparena.FapResults.StandsDresses".Localized());

                var itype = ItemIds.GroPlus;

                switch (Random.Shared.Next(3))
                {
                    case 0:
                    {
                    itype = ItemIds.GroPlus;
                    }

                    break;

                    case 1:
                    {
                    itype = ConsumableIds.SuccubusDream;
                    }

                    break;

                    case 2:
                    {
                    itype = ConsumableIds.EggLargeBlue;
                    }

                    break;
                }

                if (true )
                    itype = ItemIds.GroPlus;

                CoC.Instance.Flags.Set(GameFlagIds.FapArenaVictories, CoC.Instance.Flags.Get(GameFlagIds.FapArenaVictories) + 1);

                if (!Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                    Player.Orgasm("Dick");

                InventoryScene.TakeItem(itype, () => ReturnToCamp(1));
            }
            else
                if (place == 2)
                {
                    OutputText("Scenes.Faparena.FapResults.RemovesDick".Localized());

                    Player.Orgasm("Anal");

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Faparena.FapResults.RemovesDickAgain".Localized(
                         Player.CockDescript(x)
                    ));

                    Player.Orgasm("Anal");

                    DoNext(() => ReturnToCamp(1));
                }
        }

        public void TentacleFapSpecial(float place)
        {

            var temp = Player.Cocks.Count;

            var x = Player.BiggestCockIndex();

            while (temp > 0)
            {
                temp--;

                if (Player.Cocks[x].Length >= 40 && Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                    break;
            }

            if (Player.Cocks[x].Length >= 40 && Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
            {
                OutputText("Scenes.Faparena.TentacleFapSpecial.RememberCock".Localized(
                     Appearance.CockNoun(new Identifier("coc", "tentacle"))
                ));

                if (place <= 2)
                    OutputText("Scenes.Faparena.TentacleFapSpecial.FuckingAnus".Localized());

                if (place == 2)
                    OutputText("Scenes.Faparena.TentacleFapSpecial.AndWord".Localized());

                if (place >= 2)
                    OutputText("Scenes.Faparena.TentacleFapSpecial.FuckedBehind".Localized());

                OutputText("Scenes.Faparena.TentacleFapSpecial.PeriodBreak".Localized());

                if (place <= 2)
                {
                    OutputText("Scenes.Faparena.TentacleFapSpecial.InterferenceDisrupts".Localized(
                         Player.CockDescript(x)
                    ));

                    if (place == 1)
                        OutputText("Scenes.Faparena.TentacleFapSpecial.SuchRawPower".Localized());
                    else
                        OutputText("Scenes.Faparena.TentacleFapSpecial.ResistanceFading".Localized());

                    OutputText("Scenes.Faparena.TentacleFapSpecial.BothHoles".Localized());
                }
                else
                    OutputText("Scenes.Faparena.TentacleFapSpecial.AwkwardPosition".Localized(
                         Appearance.CockNoun(new Identifier("coc", "tentacle")),

                         Player.AssDescript(),

                         Appearance.CockNoun(new Identifier("coc", "tentacle"))
                    ));
            }
        }

        public bool TentacleFapCum()
        {

            var temp = Player.Cocks.Count;

            var x = Player.BiggestCockIndex();

            while (temp > 0)
            {
                temp--;

                if (Player.Cocks[x].Length >= 40 && Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                    break;
            }

            if (Player.Cocks[x].Length >= 40 && Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
            {
                OutputText("Scenes.Faparena.TentacleFapCum.TentacleVibrates".Localized());

                return true;
            }

            return false;
        }

    }
}