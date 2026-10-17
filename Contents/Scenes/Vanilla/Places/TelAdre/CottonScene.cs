using MonoCoC.Attributes;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<CottonScene>("coc:cotton")]
    public class CottonScene : Scene, ITimeAware
    {

        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Cotton)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public bool TimeChange()
        {
            Pregnancy.PregnancyAdvance();

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 0 && View.Time.Hours == 23)
                CoC.Instance.Flags.Set(GameFlagIds.CottonOldestKidAge, CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidAge) + 1);

            return false;
        }

        public bool TimeChangeLarge()
        {
            if (View.Time.Hours == 6
                && CoC.Instance.Flags.Get(GameFlagIds.CottonBreakfastClub) == 1

                && Player.BiggestLactation() >= 2)
            {
                CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 0);

                NomSomeTitMilkCereal();

                return true;
            }

            return false;
        }

        public override void Entry()
        {
            PregCottonChance();
        }

        public void PregCottonChance()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonHerbsOff) != 0)
            {
                if (Player.KnockUpChance(20, 100))
                    Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.CottonIncubation);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 0)
                {
                    if (Player.KnockUpChance(20, 100))
                        Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.CottonIncubation);
                }
                else
                    if (Player.KnockUpChance(0, 20))
                        Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.CottonIncubation);
        }

        public void CottonPregPCChance()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 0 && CoC.Instance.Flags.Get(GameFlagIds.CottonHerbsOff) == 0)
                Player.KnockUp(PregnancyIds.Cotton, PregnancyIds.CottonIncubation, 600);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonHerbsOff) == 0)
                    Player.KnockUp(PregnancyIds.Cotton, PregnancyIds.CottonIncubation, 1000);
                else
                    Player.KnockUp(PregnancyIds.Cotton, PregnancyIds.CottonIncubation, 100);
        }

        public bool CottonsIntro()
        {

            if (View.Time.Hours >= 12 && View.Time.Hours <= 18)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) == 0)
                    OutputText("Scenes.Cotton.CottonsIntro.SightTallBustyHorse".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) == 1)
                        OutputText("Scenes.Cotton.CottonsIntro.SpotCottonYoga".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonsIntro.SpotCottonWink".Localized());

                return true;
            }

            return false;
        }

        public void CottonGreeting()
        {
            ClearOutput();

            ShowImage("cotton-greeting");

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                OutputText("Scenes.Cotton.CottonGreeting.CentauressAllowsChat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 1);

                OutputText("Scenes.Cotton.CottonGreeting.ApproachEquineOnMat".Localized());

                if (Player.Tallness > 78)
                    OutputText("Scenes.Cotton.CottonGreeting.LookDownDespiteHeight".Localized());
                else
                    if (Player.Tallness >= 76)
                        OutputText("Scenes.Cotton.CottonGreeting.RoughlySameHeight".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonGreeting.NeedToLookUp".Localized());

                OutputText("Scenes.Cotton.CottonGreeting.DescribeTankTopOutfit".Localized());

                if (Player.IsTaur() || Player.HasTailInsteadOfLegs())
                {
                    CentaurNagaBodyBookStuff();

                    return ;
                }

                OutputText("Scenes.Cotton.CottonGreeting.CottonIntroducesHerself".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), AcceptYoga);

                AddButton(1, "Common.Button.No".Localized(), TurnDownYogaWifCottonFirstTime);
            }
            else
            {
                if (Player.Pregnancy.Event == 5)
                {
                    CottonPopsOutAKid();

                    return ;
                }

                if (Player.IsTaur() || Player.HasTailInsteadOfLegs())
                    CentaurNagaBodyBookStuff();
                else
                    CottonGreetingCommonEnd();
            }
        }

        public void CottonGreetingCommonEnd()
        {

            if (Player.Pregnancy.Incubation <= 225 && Player.Pregnancy.Type == PregnancyIds.Cotton)
            {
                OutputText("Scenes.Cotton.CottonGreetingCommonEnd.GreetRoundBelly".Localized());

                OutputText("Scenes.Cotton.CottonGreetingCommonEnd.AskYogaLamaze".Localized());
            }
            else
            {
                switch (Player.Pregnancy.Event)
                {
                    case 1:
                    {
                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.ApproachEatingPastry".Localized());

                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.CravingPastries".Localized());
                    }

                    break;

                    case 2:
                    {
                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.WaveNoticingBelly".Localized());

                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.WantsAnotherBakeryTrip".Localized());
                    }

                    break;

                    case 3:
                    {
                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.ApproachPregnantBulge".Localized());

                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.RubsBellyNausea".Localized());
                    }

                    break;

                    case 4:
                    {
                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.GreetsShowingEnormousBelly".Localized());

                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.AskRestAtHome".Localized());
                    }

                    break;
                }

                if (true )
                {
                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.InvitesWorkout".Localized());

                    OutputText("Scenes.Cotton.CottonGreetingCommonEnd.AskPlainYoga".Localized());
                }
            }

            CottonMenu();
        }

        public void CottonMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Yoga".Localized(), AcceptYoga);

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 0)
                AddButton(1, "Common.Button.VisitKids".Localized(), VisitCottonKids);

            if (Player.IsPregnant() || CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) >= 1)
                AddButton(2, "Common.Button.Herbs".Localized(), CottonContraceptionToggle);

            AddButton(14, "Common.Button.Leave".Localized(), TurnDownYogaWifCottonFirstTime);
        }

        public void CentaurNagaBodyBookStuff()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) == 0)
            {
                OutputText("Scenes.Cotton.CentaurNagaBodyBookStuff.TrailsOffConsidering".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CottonUnusualYogaBookTracker, CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) + 1);

                ReturnToCamp(1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) == 1 && Player.HasKeyItem("Yoga Guide"))
                {
                    OutputText("Scenes.Cotton.CentaurNagaBodyBookStuff.HandOverBook".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.CottonUnusualYogaBookTracker, CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) + 1);

                    ReturnToCamp(1);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) == 1)
                    {
                        OutputText("Scenes.Cotton.CentaurNagaBodyBookStuff.ShakeHeadNoBook".Localized());

                        ReturnToCamp(1);
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) == 2)
                        {
                            OutputText("Scenes.Cotton.CentaurNagaBodyBookStuff.ReadyAfterReadingBook".Localized());

                            OutputText("Scenes.Cotton.CentaurNagaBodyBookStuff.AskYogaCapital".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.CottonUnusualYogaBookTracker, CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) + 1);

                            CottonMenu();
                        }
                        else
                            CottonGreetingCommonEnd();
        }

        public void TurnDownYogaWifCottonFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.TurnDownYogaWifCottonFirstTime.RefusalReturnsToYoga".Localized());

            ReturnToCamp(1);
        }

        public void AcceptYoga()
        {
            ClearOutput();

            ShowImage("cotton-yoga");

            if (Player.FatigueLeft() < 20)
            {
                OutputText("Scenes.Cotton.AcceptYoga.TooTiredForYoga".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0 && Player.Gems < 10)
            {
                OutputText("Scenes.Cotton.AcceptYoga.CentauressGymFee".Localized());

                OutputText("Scenes.Cotton.AcceptYoga.NotEnoughGems".Localized());

                ReturnToCamp(1);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
            {
                Player.Gems -= 10;

                OutputText("Scenes.Cotton.AcceptYoga.GymFeeCollected".Localized());
            }

            if (Player.Pregnancy.Incubation <= 225 && Player.Pregnancy.Type == PregnancyIds.Cotton)
            {
                OutputText("Scenes.Cotton.AcceptYoga.ChangeForLamaze".Localized());

                CottonChat();

                OutputText("Scenes.Cotton.AcceptYoga.CottonOffersShower".Localized());

                ClearButtons();

                if (Player.HasCock())
                    AddButton(0, "Common.Button.FuckHer".Localized(), FuckCottonInShowerRepeat);
                else
                    AddButton(0, "Common.Button.FuckHer".Localized(), null);

                if (!Player.IsGenderless())
                    AddButton(1, "Common.Button.GetFucked".Localized(), CottonFucksYouInShowerRepeat);
                else
                    AddButton(1, "Common.Button.GetFucked".Localized(), null);

                if ((Player.Speed >= 50 || Player.HasPerk(PerkIds.Flexibility)) && !Player.IsTaur())
                    AddButton(2, "Common.Button.TantricSex".Localized(), CottonTantricSex);
                else
                    AddButton(2, "Common.Button.TantricSex".Localized(), null);

                AddButton(14, "Common.Button.Leave".Localized(), LeaveCotton);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesHadYoga) == 0)
                {
                    OutputText("Scenes.Cotton.AcceptYoga.LeadsToLockers".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Cotton.AcceptYoga.SpiesCrotchSmiles".Localized());
                    else
                        if (Player.HasCock())
                            OutputText("Scenes.Cotton.AcceptYoga.CradlesCockPromise".Localized());
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Cotton.AcceptYoga.PatsGroinAdvice".Localized());

                    OutputText("Scenes.Cotton.AcceptYoga.ParagraphBreak".Localized());

                    OutputText("Scenes.Cotton.AcceptYoga.HelpsIntoOutfit".Localized());

                    OutputText("Scenes.Cotton.AcceptYoga.InstructsMatPoses".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)((10 + Player.Libido / 10 + Player.Sensitivity / 20))));

                    OutputText("Scenes.Cotton.AcceptYoga.PraiseForBeginner".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Shower".Localized(), CottonShowerFunTimes);

                    AddButton(14, "Common.Button.Leave".Localized(), LeaveAfterYoga);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.TimesHadYoga) > 0)
                    {
                        OutputText("Scenes.Cotton.AcceptYoga.ChangeForSecondYoga".Localized());

                        OutputText("Scenes.Cotton.AcceptYoga.NotesYourDiscomfort".Localized());

                        ClearButtons();

                        if (Player.HasCock())
                            AddButton(0, "Common.Button.FuckHer".Localized(), FuckCottonInShowerRepeat);
                        else
                            AddButton(0, "Common.Button.FuckHer".Localized(), null);

                        if (!Player.IsGenderless())
                            AddButton(1, "Common.Button.GetFucked".Localized(), CottonFucksYouInShowerRepeat);
                        else
                            AddButton(1, "Common.Button.GetFucked".Localized(), null);

                        if ((Player.Speed >= 50 || Player.HasPerk(PerkIds.Flexibility)) && !Player.IsTaur())
                            AddButton(2, "Common.Button.TantricSex".Localized(), CottonTantricSex);
                        else
                            AddButton(2, "Common.Button.TantricSex".Localized(), null);

                        AddButton(14, "Common.Button.Leave".Localized(), LeaveCotton);
                    }
                    else
                    {
                        if (Player.IsTaur())
                            OutputText("Scenes.Cotton.AcceptYoga.ChangeTankOnly".Localized());
                        else
                            if (Player.HasTailInsteadOfLegs())
                                OutputText("Scenes.Cotton.AcceptYoga.ChangeTopOnly".Localized());
                            else
                                OutputText("Scenes.Cotton.AcceptYoga.ChangeForPlainYoga".Localized());

                        CottonChat();

                        OutputText("Scenes.Cotton.AcceptYoga.RepeatsShowerOffer".Localized());

                        if (Player.HasCock())
                            AddButton(0, "Common.Button.FuckHer".Localized(), FuckCottonInShowerRepeat);
                        else
                            AddButton(0, "Common.Button.FuckHer".Localized(), null);

                        if (!Player.IsGenderless())
                            AddButton(1, "Common.Button.GetFucked".Localized(), CottonFucksYouInShowerRepeat);
                        else
                            AddButton(1, "Common.Button.GetFucked".Localized(), null);

                        if ((Player.Speed >= 50 || Player.HasPerk(PerkIds.Flexibility)) && !Player.IsTaur())
                            AddButton(2, "Common.Button.TantricSex".Localized(), CottonTantricSex);
                        else
                            AddButton(2, "Common.Button.TantricSex".Localized(), null);

                        AddButton(14, "Common.Button.Leave".Localized(), LeaveCotton);
                    }

            Player.ModTone(52, 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesHadYoga, CoC.Instance.Flags.Get(GameFlagIds.TimesHadYoga) + 1);

            Player.ChangeFatigue(20);

            Player.ApplyStats(new DynStat(GameStats.Speed, 1f), new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20 + Player.Sensitivity / 20)));
        }

        public void CottonChat()
        {

            var chats = new List<int>();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) > 0)
                chats.Add(1);

            if (Player.HasStatusEffect(StatusEffectIds.Edryn))
                chats.Add(2);

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
                chats.Add(2);

            if (CoC.Instance.Flags.Get(GameFlagIds.FreedVala) != 0)
                chats.Add(3);

            if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) > 0)
                chats.Add(4);

            float choice = chats[Random.Shared.Next(chats.Count)];

            if (choice == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 0 || CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) < 0)
                    OutputText("Scenes.Cotton.CottonChat.ChatAboutUrta".Localized());
                else
                    if (SceneInstance<UrtaScene>.Get().UrtaLove())
                        OutputText("Scenes.Cotton.CottonChat.ChatUrtaSecond".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonChat.ChatUrtaThird".Localized());
            }
            else
                if (choice == 2)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) > 0)
                        OutputText("Scenes.Cotton.CottonChat.ChatAboutEdryn".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0)
                            OutputText("Scenes.Cotton.CottonChat.ChatEdrynSecond".Localized(
                                Player.Mf("a great dad", "as well")
                            ));
                        else
                            OutputText("Scenes.Cotton.CottonChat.ChatEdrynThird".Localized());
                }
                else
                    if (choice == 3)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) < 5 && CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
                            OutputText("Scenes.Cotton.CottonChat.ChatAboutScylla".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) >= 5)
                                OutputText("Scenes.Cotton.CottonChat.ChatScyllaSecond".Localized());
                    }
                    else
                        if (choice == 4)
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.FreedVala) != 0)
                                OutputText("Scenes.Cotton.CottonChat.ChatAboutVala".Localized());
                        }
                        else
                            if (choice == 5)
                                if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) == 1)
                                {
                                    OutputText("Scenes.Cotton.CottonChat.ChatAboutJojo".Localized());

                                    Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));
                                }
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) > 1)
                                        OutputText("Scenes.Cotton.CottonChat.ChatJojoSecond".Localized());
        }

        public void LeaveAfterYoga()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.LeaveAfterYoga.LeavesToWashUp".Localized());

            ReturnToCamp(1);
        }

        public void CottonShowerFunTimes()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.CottonShowerFunTimes.LeadsIntoLockers".Localized());

            OutputText("Scenes.Cotton.CottonShowerFunTimes.SeesTonedBody".Localized());

            OutputText("Scenes.Cotton.CottonShowerFunTimes.SuggestsStretching".Localized());

            if (Player.HasCock())
                AddButton(0, "Common.Button.FuckHer".Localized(), CottonFirstTimeFuckHer);
            else
                AddButton(0, "Common.Button.FuckHer".Localized(), null);

            if (!Player.IsGenderless())
                AddButton(1, "Common.Button.GetFucked".Localized(), CottonFucksYou);
            else
                AddButton(1, "Common.Button.GetFucked".Localized(), null);

            AddButton(3, "Common.Button.ServiceHer".Localized(), ServiceFirstTimeCotton);

            AddButton(14, "Common.Button.Refuse".Localized(), RefuseFirstTimeCotton);
        }

        public void CottonFirstTimeFuckHer()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 2);

            ShowImage("cotton-first-fuck");

            int x = Player.CockThatFits(100);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.StepsBehindUnderWater".Localized());

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.BendsHerOver".Localized());

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.LicksToNearOrgasm".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.TurnsUpTheHeat".Localized());

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.ClimaxInsideHer".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.PullOutAfterward".Localized());

            OutputText("Scenes.Cotton.CottonFirstTimeFuckHer.DressAndLeaveGym".Localized());

            PregCottonChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void CottonFucksYou()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 2);

            ShowImage("cotton-fucks-you");

            Player.SlimeFeed();

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Cotton.CottonFucksYou.SoapBackMassage".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.AnalSoapFingering".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.CockOnBack".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.WhisperThenAnalEntry".Localized(
                    Player.AssholeDescript()
                ));

                Player.ButtChange(72, true, true, false);

                OutputText("Scenes.Cotton.CottonFucksYou.ParagraphBreak".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.AssholeClenchMoan".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.AnalClimaxCumLeak".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Cotton.CottonFucksYou.ExcessCumDrainClog".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.SitInCumPuddle".Localized());

                OutputText("Scenes.Cotton.CottonFucksYou.GymFarewellLine".Localized());

                Player.Orgasm("Anal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Cotton.CottonFucksYou.SoapBackMassageAgain".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.VaginalSoapFingering".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.CockOnBackAgain".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.WhisperThenVaginalEntry".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0)
                    ));

                    Player.CuntChange(72, true, true, false);

                    OutputText("Scenes.Cotton.CottonFucksYou.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.CuntClenchClitTwitch".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.VaginalClimaxArchBack".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.FloorRestCleanup".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.GymFarewellNoPg".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                    CottonPregPCChance();
                }
                else
                {
                    OutputText("Scenes.Cotton.CottonFucksYou.SoapBackMassageThird".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.VaginalSoapFingeringAgain".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.CockOnBackThird".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.WhisperThenHermEntry".Localized(
                        Player.VaginaDescript()
                    ));

                    Player.CuntChange(72, true, true, false);

                    OutputText("Scenes.Cotton.CottonFucksYou.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.CuntClenchLimpCock".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.VaginalClimaxCumLeak".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.FloorRestCleanupAgain".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYou.GymFarewellLineAgain".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                    CottonPregPCChance();
                }

            ReturnToCamp(1);
        }

        public void ServiceFirstTimeCotton()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 2);

            Player.SlimeFeed();

            ShowImage("cotton-first-time-give-her-blowjob");

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.MeekNodKneelDown".Localized());

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.GraspCockSoapUp".Localized());

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.SweetLickGrowsErect".Localized());

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.GlansTeaseDeepThroat".Localized());

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.WallBraceThenFacial".Localized());

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.SwallowAndLickClean".Localized());

            OutputText("Scenes.Cotton.ServiceFirstTimeCotton.KissThenLeaveGym".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20 + Player.Sensitivity / 20)));

            ReturnToCamp(1);
        }

        public void RefuseFirstTimeCotton()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.RefuseFirstTimeCotton.RefuseThenYogaOffer".Localized());

            ReturnToCamp(1);
        }

        public void FuckCottonInShowerRepeat()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 2);

            ShowImage("cotton-shower-fuck-repeat");

            int x = Player.CockThatFits(60);

            if (x < 0)
                x = 0;

            if (Player.IsTaur())
            {
                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.LeadInShower".Localized());

                if (Player.Pregnancy.Event > 1)
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HandsOnBellyKiss".Localized());
                else
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.PullYouCloser".Localized());

                if (Player.CockArea(x) >= 100)
                {
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CockStirsCottonLimp".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CocksStirCottonLimp".Localized());
                }
                else
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CockStirsWithCotton".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CocksStirWithCotton".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.RearCockMoistensBoth".Localized(
                        Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreak".Localized());

                if (Player.BiggestBreastSize() >= 2)
                {
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.GropesSucklesNipple".Localized(
                        Appearance.BiggestBreastSizeDescript(Player)
                    ));

                    if (Player.BiggestLactation() >= 1)
                    {
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.MilkSeepsFromNipple".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 1);
                    }
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SwitchesOtherBreast".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakAgain".Localized());
                }

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.DragBenchToShower".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.PokesOtherHole".Localized());
                else
                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CocksJiggleThighs".Localized());

                if (Player.CockArea(x) > 70)
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.TooBigStuffAnyway".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if (Player.Cocks[x].Length < 4)
                    {
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.AsksIfInYet".Localized());

                        if (Player.Pregnancy.Event > 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SmirkAdequateBelly".Localized());
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CottonSaysAdequate".Localized());

                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SmilePushFullyIn".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.PistonTightGrip".Localized());

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakThird".Localized());

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.KeepsUpNoFatigue".Localized());

                if (Player.CockArea(x) >= 100)
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.LimpCockLeaksBench".Localized());
                else
                    if (Player.Pregnancy.Event > 1)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CumsOnPregnantBelly".Localized());
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CumsOnBellyBreasts".Localized());

                if (Player.CockArea(x) > 70)
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CantFitEntireGirth".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.LastThrustGasp".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.EruptsInsideFemmeTwitch".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.EruptsInsideClench".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.TokenThrustsPullOut".Localized());

                if (Player.CumQ() < 200)
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SeepFromFolds".Localized());
                else
                    if (Player.CumQ() < 700)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.GushFromFolds".Localized());
                    else
                        if (Player.CumQ() < 1200)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SmallTorrentFolds".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SurgesFromAbusedHole".Localized());
                            else
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.MassiveFloodCunt".Localized());

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.WashedTowardDrain".Localized());

                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.BaskThenStandUp".Localized());
            }
            else
                if (Player.HasTailInsteadOfLegs())
                {
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.LeadInShowerAgain".Localized());

                    if (Player.Pregnancy.Event > 1)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HandsOnBellyAgain".Localized());
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.PullYouCloserAgain".Localized());

                    if (Player.CockArea(x) >= 100)
                    {
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CockStirsBeneathYou".Localized(
                                Player.CockDescript(x),

                                Player.CockHead(x)
                            ));
                        else
                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CocksStirBeneathYou".Localized());
                    }
                    else
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CockRisesWithCotton".Localized(
                                Player.CockDescript(x)
                            ));
                        else
                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CocksRiseWithCotton".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.RearMoistensWater".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakFifth".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                    {
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.GropesNippleAgain".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.BiggestLactation() >= 1)
                        {
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.MilkSeepsAgain".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 1);
                        }
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SwitchesBreastAgain".Localized());

                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakSixth".Localized());
                    }

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CoilsAroundCotton".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.PositionsOtherHole".Localized());
                    else
                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.CocksStrainAching".Localized());

                    if (Player.CockArea(x) > 70)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.TooBigStuffAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.Cocks[x].Length < 4)
                        {
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.SheAsksIfIn".Localized());

                            if (Player.Pregnancy.Event > 1)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaSmirkRoundBelly".Localized());
                            else
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaAdequatePlainReply".Localized());

                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaSmallCockPush".Localized(
                                Player.CockDescript(x)
                            ));
                        }
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaPistonShoulderGrip".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakNagaPace".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaCoilUnderSpray".Localized());

                    if (Player.CockArea(x) >= 100)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaLimpCockTwitch".Localized());
                    else
                        if (Player.Pregnancy.Event > 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaPregnantBellySpray".Localized());
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaPlainBellySpray".Localized());

                    if (Player.CockArea(x) > 70)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaPseudoMissionary".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakNagaEdge".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaFinalThrustGasp".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaEruptsFemmeTwitch".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaEruptsClenchOnly".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaTokenPullOut".Localized());

                    if (Player.CumQ() < 200)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaCumSeepFolds".Localized());
                    else
                        if (Player.CumQ() < 700)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaCumGushFolds".Localized());
                        else
                            if (Player.CumQ() < 1200)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaCumTorrentFolds".Localized());
                            else
                                if (Player.CumQ() < 2000)
                                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaSurgeAbusedHole".Localized());
                                else
                                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaCumFloodCunt".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaDrainWashAway".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.NagaSlitherAfterglow".Localized());
                }
                else
                {
                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidShowerLeadIn".Localized());

                    if (Player.Pregnancy.Event > 1)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidPregnantBellyPull".Localized());
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidPullCloser".Localized());

                    if (Player.CockArea(x) >= 100)
                    {
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidStirringLimpCock".Localized(
                                Player.CockDescript(x),

                                Player.CockHead(x)
                            ));
                        else
                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidStirringLimpCocks".Localized());
                    }
                    else
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidStirringAlongside".Localized(
                                Player.CockDescript(x)
                            ));
                        else
                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidStirringGroup".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidVaginaMoistens".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakHumanoidBreast".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                    {
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidBreastGrope".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.BiggestLactation() >= 1)
                        {
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidMilkSeepReward".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 1);
                        }
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidSwitchOtherBreast".Localized());

                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakHumanoidTurn".Localized());
                    }

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidSuitablyTurnedOn".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidSecondHoleTease".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidExtraCocksStrain".Localized());

                    if (Player.CockArea(x) > 70)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidTooBigBrace".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.Cocks[x].Length < 4)
                        {
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidApprovalWavers".Localized());

                            if (Player.Pregnancy.Event > 1)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidSmirkRoundBelly".Localized());
                            else
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidAdequatePlainReply".Localized());

                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidGiveLittleSmile".Localized(
                                Player.CockDescript(x)
                            ));
                        }
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidPistonShoulder".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakHumanoidKeep".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidLayDownContinue".Localized());

                    if (Player.CockArea(x) >= 100)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidLimpCockTwitch".Localized());
                    else
                        if (Player.Pregnancy.Event > 1)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidPregnantLoosingSpray".Localized());
                        else
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidPlainBellySpray".Localized());

                    if (Player.CockArea(x) > 70)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidMissionaryGirth".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.ParagraphBreakHumanoidFinal".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidFinalThrustGasp".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidEruptsFemmeTwitch".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidEruptsClenchOnly".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidTokenPullOut".Localized());

                    if (Player.CumQ() < 200)
                        OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidCumSeepFolds".Localized());
                    else
                        if (Player.CumQ() < 700)
                            OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidCumGushFolds".Localized());
                        else
                            if (Player.CumQ() < 1200)
                                OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidCumTorrentFolds".Localized());
                            else
                                if (Player.CumQ() < 2000)
                                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidSurgeAbusedHole".Localized());
                                else
                                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidCumFloodCunt".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidDrainWashAway".Localized());

                    OutputText("Scenes.Cotton.FuckCottonInShowerRepeat.HumanoidAfterglowUpright".Localized());
                }

            PregCottonChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.HasGottenWashed();

            ReturnToCamp(1);
        }

        public void CottonFucksYouInShowerRepeat()
        {
            ClearOutput();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 2);

            ShowImage("cotton-shower-fucks-you-repeat");

            if (Player.IsTaur())
            {
                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ForcedShowerUndress".Localized());

                if (Player.Gender == Gender.Male)
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.TinglesBetwixtLimp".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.TinglesBetwixtLimpAgain".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        if (Player.Gender == Gender.Female)
                            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.SteamMoistening".Localized(
                                Player.VaginaDescript(0)
                            ));
                        else
                            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.DeepAchingNeed".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreak".Localized());

                if (Player.BiggestBreastSize() >= 2)
                {
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.NippleKissDescent".Localized(
                        Appearance.BiggestBreastSizeDescript(Player)
                    ));

                    if (Player.BiggestLactation() >= 1)
                    {
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.MilkBeginsFlowing".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 1);
                    }
                    else
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.OtherBreastTeased".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakAgain".Localized());
                }

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.KissingDownBelly".Localized());

                if (Player.HasCock())
                    if (Player.CockArea(0) > 70)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.EnormousGirthRemark".Localized());
                    else
                        if (Player.BiggestCockLength() < 4)
                            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.SmallGirthRemark".Localized());
                        else
                            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.LimpCockFlattery".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.TeasingClitFlick".Localized(
                        Player.VaginaDescript(0)
                    ));

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.BareCrotchSurprise".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakThird".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.KissesThenBench".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.WaterRearPrep".Localized());

                if (Player.HasVagina())
                    Player.CuntChange(72, true, true, false);
                else
                    Player.ButtChange(72, true, true, false);

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.EasyFullEntry".Localized());

                if (Player.Pregnancy.Event > 1)
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.GravidBulgeWhisper".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.MiniOrgasmShiver".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.FinalThrustPull".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.DismountCumLeak".Localized());
            }
            else
            {
                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ForcedShowerUndressAgain".Localized());

                if (Player.Gender == Gender.Male)
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.TinglesBeneathLimp".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.TinglesBeneathLimpAgain".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        if (Player.Gender == Gender.Female)
                            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.SteamMoisteningAgain".Localized(
                                Player.VaginaDescript(0)
                            ));
                        else
                            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.DeepAchingNeedAgain".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakFifth".Localized());

                if (Player.BiggestBreastSize() >= 2)
                {
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.NippleKissDescentAgain".Localized(
                        Appearance.BiggestBreastSizeDescript(Player)
                    ));

                    if (Player.BiggestLactation() >= 1)
                    {
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.MilkBeginsFlowingAgain".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 1);
                    }
                    else
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.OtherBreastTeasedAgain".Localized());

                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakSixth".Localized());
                }

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.KissTrailToCrotch".Localized());

                if (Player.HasCock())
                {
                    if (Player.BiggestCockArea() > 70)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.EnormousGirthRemarkAgain".Localized());

                    if (Player.BiggestCockLength() < 4)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.SmallGirthRemarkAgain".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.LimpCockFlatteryAgain".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.TeasingClitFlickAgain".Localized(
                        Player.VaginaDescript(0)
                    ));

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.BareCrotchSurpriseAgain".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakSeventh".Localized());

                if (Player.HasTailInsteadOfLegs())
                {
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.KissLiftByWaist".Localized());

                    if (Player.Pregnancy.Event > 1)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ArmsAroundNeck".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ArmsNeckOneLeg".Localized());
                }
                else
                {
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.KissLiftByLegs".Localized());

                    if (Player.Pregnancy.Event > 1)
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ArmsLegsAvoidBelly".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ArmsLegsEmbrace".Localized());
                }

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.QuickKissGentleEntry".Localized());

                if (Player.HasVagina())
                    Player.CuntChange(72, true, true, false);
                else
                    Player.ButtChange(72, true, true, false);

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakEighth".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.GirthStillLimited".Localized());

                if (Player.Pregnancy.Event > 1)
                    OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.GravidBulgeBelly".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.MiniOrgasmShiverAgain".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.SafeCloseEmbrace".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.FinalThrustWhinny".Localized());

                OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ShowerFloorRest".Localized());
            }

            if (Player.HasVagina())
                CottonPregPCChance();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.HasGottenWashed();

            ReturnToCamp(1);
        }

        public void CottonTantricSex()
        {
            ClearOutput();

            ShowImage("cotton-tantric-sex");

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.CottonMetFucked, 2);

            OutputText("Scenes.Cotton.CottonTantricSex.AskAdvancedYoga".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.MirroredPrivateRoom".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.MeditativeSeat".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.SpiritsJoined".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.YogaPortionBegins".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.GulpingApproach".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Cotton.CottonTantricSex.FamiliarWord".Localized());
            else
                OutputText("Scenes.Cotton.CottonTantricSex.AlienWord".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.UnfamiliarSensations".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Cotton.CottonTantricSex.ImpaleSettlingDown".Localized());
            else
                OutputText("Scenes.Cotton.CottonTantricSex.ImpaleMimicPose".Localized());

            OutputText("Scenes.Cotton.CottonFucksYouInShowerRepeat.ParagraphBreakNinth".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.NoThrustingIntensity".Localized());

            if (Player.HasVagina())
                Player.CuntChange(72, true, true, false);
            else
                Player.ButtChange(72, true, true, false);

            if (Player.HasCock())
                OutputText("Scenes.Cotton.CottonTantricSex.LimpCockTingling".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.ParagraphBreak".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.MoveToLotus".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Cotton.CottonTantricSex.CoilAroundPartner".Localized());
            else
                OutputText("Scenes.Cotton.CottonTantricSex.CurlLimbsAround".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.FacePressedBosom".Localized());

            if (Player.BiggestBreastSize() >= 2)
            {
                OutputText("Scenes.Cotton.CottonTantricSex.CottonSucklesYou".Localized());

                if (Player.BiggestLactation() >= 1)
                {
                    OutputText("Scenes.Cotton.CottonTantricSex.MilkLetdown".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.CottonBreakfastClub, 1);
                }
                else
                    OutputText("Scenes.Cotton.CottonTantricSex.SwitchOtherNipple".Localized());

                OutputText("Scenes.Cotton.CottonTantricSex.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Cotton.CottonTantricSex.HeadGamePose".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Cotton.CottonTantricSex.GripThenUncoil".Localized());
            else
                OutputText("Scenes.Cotton.CottonTantricSex.GripThenExtend".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.ParagraphBreakThird".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.HipGyrating".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.SensationsConsume".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Cotton.CottonTantricSex.CockSeedDribble".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Cotton.CottonTantricSex.ClampDownMilking".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                OutputText("Scenes.Cotton.CottonTantricSex.AssholeClampMilking".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.SharedLightning".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.CollapsePanting".Localized());

            OutputText("Scenes.Cotton.CottonTantricSex.StrengthReturns".Localized());

            if (Player.HasCock())
                Player.Orgasm("Dick");

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .25f), new DynStat(GameStats.Speed, .25f), new DynStat(GameStats.Libido, -.25f), new DynStat(GameStats.Sensitivity, -.25f));

            if (Player.HasVagina())
                CottonPregPCChance();

            ReturnToCamp(1);
        }

        public void LeaveCotton()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.LeaveCotton.ThankShowerLeave".Localized());

            ReturnToCamp(1);
        }

        public void NomSomeTitMilkCereal()
        {

            ShowImage("cotton-visits-you-at-camp-drinks-all-your-milk-the-asshole");

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.WakingRustling".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.WeaponReadyAlert".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.HandsUpNoThreat".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.InviteToBedroll".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.BowlOnLap".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.MilkBeingDrawn".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.OtherBreastSuckle".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.HandLeavesBreast".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.TastySpoonBite".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.RumorMillChat".Localized());

            OutputText("Scenes.Cotton.NomSomeTitMilkCereal.CheekKissFarewell".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -0.5f), new DynStat(GameStats.Sensitivity, -.5f), new DynStat(GameStats.Lust, -5f));

            Player.BoostLactation(0.05f);

            if (Player.HasStatusEffect(StatusEffectIds.Feeder))
            {
                Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
            }

            Go<CampScene>();
        }

        public void CottonPregnantAlert()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.CottonPregnantAlert.CottonPacingGym".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CottonPregnantAlertII);
        }

        public void CottonPregnantAlertII()
        {
            ClearOutput();

            ShowImage("cotton-you-got-her-preggo");

            OutputText("Scenes.Cotton.CottonPregnantAlertII.WorryNoticed".Localized());

            OutputText("Scenes.Cotton.CottonPregnantAlertII.PregnancyAnnounced".Localized());

            OutputText("Scenes.Cotton.CottonPregnantAlertII.HerbsNoMatch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0 || (Player.IsPregnant() && CoC.Instance.Flags.Get(GameFlagIds.EdrynPregnantAndNotToldPcYet) != 0))
                OutputText("Scenes.Cotton.CottonPregnantAlertII.EdrynPregnantToo".Localized());

            OutputText("Scenes.Cotton.CottonPregnantAlertII.NervousHandWring".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) == 0)
                OutputText("Scenes.Cotton.CottonPregnantAlertII.AskStayThrough".Localized());
            else
                OutputText("Scenes.Cotton.CottonPregnantAlertII.AskAbandonEdryn".Localized());

            OutputText("Scenes.Cotton.CottonPregnantAlertII.SpeechTagClose".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), BeAGoodCottonDad);

            AddButton(1, "Common.Button.LeaveHer".Localized(), BeABadCottonDad);
        }

        public void BeABadCottonDad()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.BeABadCottonDad.RefuseFatherhood".Localized());

            OutputText("Scenes.Cotton.BeABadCottonDad.DeterminedJaw".Localized());

            OutputText("Scenes.Cotton.BeABadCottonDad.SwivelsBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) == 0)
                OutputText("Scenes.Cotton.BeABadCottonDad.SlapsYouHard".Localized());
            else
                OutputText("Scenes.Cotton.BeABadCottonDad.PunchesYouHard".Localized());

            OutputText("Scenes.Cotton.BeABadCottonDad.CursePetInsult".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) > 0)
                OutputText("Scenes.Cotton.BeABadCottonDad.PaybackForEdryn".Localized());

            OutputText("Scenes.Cotton.BeABadCottonDad.TurnsAndLeaves".Localized());

            OutputText("Scenes.Cotton.BeABadCottonDad.NoLongerSpeaks".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcIsADeadbeatCottonDad, 1);

            ReturnToCamp(1);
        }

        public void BeAGoodCottonDad()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.BeAGoodCottonDad.PromiseSupport".Localized());

            OutputText("Scenes.Cotton.BeAGoodCottonDad.SmileHugRelief".Localized());

            OutputText("Scenes.Cotton.BeAGoodCottonDad.KissReturnsYoga".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcIsAGoodCottonDad, 1);

            ReturnToCamp(1);
        }

        public void CottonPopsOutAKid()
        {
            ClearOutput();

            var kid = 0;

            ShowImage("cotton-giving-birth");

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 0)
            {
                OutputText("Scenes.Cotton.CottonPopsOutAKid.CottonAtUsualSpot".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.CasualStomachPainRemark".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.WaterBreaksMidStretch".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.QuickSupportOffer".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.HomeBirthWish".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.PoolQuestion".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.QuickAgreement".Localized());

                if (silly)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.CubeSharkCondition".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.AgreeToLeave".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.ArmAroundNeck".Localized());
                else
                    if (Player.Tallness < 70)
                        OutputText("Scenes.Cotton.CottonPopsOutAKid.HandOnShoulder".Localized());
                    else
                        if (Player.Tallness < 86)
                            OutputText("Scenes.Cotton.CottonPopsOutAKid.ArmAroundMidsection".Localized());
                        else
                            OutputText("Scenes.Cotton.CottonPopsOutAKid.ArmAroundWaist".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.WaddlesTowardPool".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.LongMinutesPass".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ToughPeopleRemark".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ReassureThenDuck".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.HandsIntoWater".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.PeerBackQuestion".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ParentsOfNewborn".Localized());

                if (Random.Shared.Next(3) == 0)
                    kid = 1;
                else
                    if (Random.Shared.Next(2) == 0)
                        kid = 2;
                    else
                        kid = 3;

                CoC.Instance.Flags.Set(GameFlagIds.CottonOldestKidGender, kid);

                if (kid == 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.MaleWord".Localized());
                else
                    if (kid == 2)
                        OutputText("Scenes.Cotton.CottonPopsOutAKid.FemaleWord".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonPopsOutAKid.HermWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.FoalWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.AwwsAtNewborn".Localized());

                if (kid == 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.SonWord".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.DaughterWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ReluctantToLetGo".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.HerWord".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.HimWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.RelinquishGrasp".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.HerWordAgain".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.HimWordAgain".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.LiftsTopToNurse".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ExitPoolTogether".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.NodToVisitHome".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ComplimentDecor".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ThanksPetRemark".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.AskMotherOrFather".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.DaddyWordLaugh".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.AskMotherSpecies".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.VillageLikeMe".Localized());

                if (kid == 3)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.FamilyTraitRemark".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.SpeechTagClose".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.NodAskRelationship".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.PauseThenAnswer".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.ThankThenChangeSubject".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.FraternalTwinsStory".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.WipesTearsChangeTopic".Localized());

                if (kid < 2)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.GuyWord".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.GirlWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.TakesBundleBlanket".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.SwaddledHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.SwaddledHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.SwaddleCribNarration".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.WhisperedTeaseReply".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.WatchBabySeated".Localized());

                Player.ClearPregnancy();

                CoC.Instance.Flags.Set(GameFlagIds.CottonKidCount, CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                    CoC.Instance.Flags.Set(GameFlagIds.CottonOldestKidAge, 1);

                ReturnToCamp(1);
            }
            else
            {
                OutputText("Scenes.Cotton.CottonPopsOutAKid.NoticePostpartumBelly".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.JustMissedBirth".Localized());

                if (Random.Shared.Next(3) == 0)
                    kid = 1;
                else
                    if (Random.Shared.Next(2) == 0)
                        kid = 2;
                    else
                        kid = 3;

                if (kid == 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.BabyBoyWord".Localized());
                else
                    if (kid == 2)
                        OutputText("Scenes.Cotton.CottonPopsOutAKid.BabyGirlWord".Localized());
                    else
                        OutputText("Scenes.Cotton.CottonPopsOutAKid.BabyHermWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.HappyHugInvite".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.SeeBabyHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.SeeBabyHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.OptionalInvitePhrase".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.DaycareShe".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.DaycareHe".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.DaycareHereSuffix".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.DaycareWatchHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.DaycareWatchHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.TodayWord".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.AskVisitLine".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.WantToSeeHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.WantToSeeHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.CottonConsentPrefix".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.WontUpsetHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.WontUpsetHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.SentencePeriod".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.OfCourseNotPrefix".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.CheerHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.CheerHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.WarmReunionPhrase".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.HerDaddyPossessive".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.HimDaddyPossessive".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.CorridorLeadNarration".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.BunnyNurseGreeting".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.NapShe".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.NapHe".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.NapInviteSuffix".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.NapVisitHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.NapVisitHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.NodAffirmNarration".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.QuietWakePrefix".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.WakeHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.WakeHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.LookOverPrefix".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.InspectBabyHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.InspectBabyHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.LeaveRoomNarration".Localized());

                if (kid != 1)
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.LaterVisitHer".Localized());
                else
                    OutputText("Scenes.Cotton.CottonPopsOutAKid.LaterVisitHim".Localized());

                OutputText("Scenes.Cotton.CottonPopsOutAKid.GymExitNarration".Localized());

                Player.ClearPregnancy();

                CoC.Instance.Flags.Set(GameFlagIds.CottonKidCount, CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                    CoC.Instance.Flags.Set(GameFlagIds.CottonOldestKidAge, 1);

                CottonMenu();
            }
        }

        public void CottonContraceptionToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonContraceptionTalk) > 0)
            {
                RepeatContraceptionToggleCotton();

                return ;
            }

            OutputText("Scenes.Cotton.CottonContraceptionToggle.YouAskThePrefix".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Cotton.CottonContraceptionToggle.PregnantWord".Localized());

            OutputText("Scenes.Cotton.CottonContraceptionToggle.StillTakingHerbs".Localized());

            OutputText("Scenes.Cotton.CottonContraceptionToggle.CottonNodsThinksPrefix".Localized());

            if (Player.Gender != Gender.Hermaphrodite || Player.IsPregnant())
                OutputText("Scenes.Cotton.CottonContraceptionToggle.VirileWord".Localized());
            else
                OutputText("Scenes.Cotton.CottonContraceptionToggle.FertileWord".Localized());

            OutputText("Scenes.Cotton.CottonContraceptionToggle.BrokeThroughProtection".Localized());

            OutputText("Scenes.Cotton.CottonContraceptionToggle.AdmitMakesSense".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) < 3)
                OutputText("Scenes.Cotton.CottonContraceptionToggle.SheAdmitsTrue".Localized());
            else
                OutputText("Scenes.Cotton.CottonContraceptionToggle.HerbsProvenUseless".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CottonContraceptionTalk, 1);

            ClearButtons();

            AddButton(0, "Common.Button.StopTaking".Localized(), TellCottonStopEatingHorsePills);

            AddButton(1, "Common.Button.KeepTaking".Localized(), TellCottonToKeepFiringBlanksAsshole);
        }

        public void TellCottonStopEatingHorsePills()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.TellCottonStopEatingHorsePills.TellHerbsUseless".Localized());

            OutputText("Scenes.Cotton.TellCottonStopEatingHorsePills.AgreesToStopTomorrow".Localized());

            OutputText("Scenes.Cotton.TellCottonStopEatingHorsePills.ThankAndChangeSubject".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CottonHerbsOff, 1);

            CottonMenu();
        }

        public void TellCottonToKeepFiringBlanksAsshole()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.TellCottonToKeepFiringBlanksAsshole.RatherNoMoreKids".Localized());

            OutputText("Scenes.Cotton.TellCottonToKeepFiringBlanksAsshole.BetterSafeThanPrefix".Localized());

            if (!silly)
                OutputText("Scenes.Cotton.TellCottonToKeepFiringBlanksAsshole.SorryWord".Localized());
            else
                OutputText("Scenes.Cotton.TellCottonToKeepFiringBlanksAsshole.HerdOfLittleHorsefolk".Localized());

            OutputText("Scenes.Cotton.TellCottonToKeepFiringBlanksAsshole.TagQuestionSuffix".Localized());

            OutputText("Scenes.Cotton.TellCottonToKeepFiringBlanksAsshole.ThankAndChangeSubject".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CottonHerbsOff, 0);

            CottonMenu();
        }

        public void RepeatContraceptionToggleCotton()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonHerbsOff) == 1)
            {
                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.ResumeHerbsRequest".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CottonHerbsOff, 0);

                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.BetterSafeThanPrefix".Localized());

                if (!silly)
                    OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.SorryWord".Localized());
                else
                    OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.HerdOfLittleHorsefolk".Localized());

                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.TagQuestionSuffix".Localized());

                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.ThankAndChangeSubject".Localized());
            }
            else
            {
                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.ReadyToStopHerbs".Localized());

                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.AgreesToStopTomorrow".Localized());

                OutputText("Scenes.Cotton.RepeatContraceptionToggleCotton.ThankChangeBriefly".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CottonHerbsOff, 1);
            }

            CottonMenu();
        }

        public void GoTellCottonShesAMomDad()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.ConvincedCottonFather".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.CottonSmallApartment".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.AsksPolitelyJoinMe".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.TellThatsCorrect".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.PuzzledThenRealization".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.TellPrettySure".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.AsksSlumpedInChair".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.MostlyUpToHer".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.CottonExplodesThenThoughtful".Localized());

            OutputText("Scenes.Cotton.GoTellCottonShesAMomDad.ThankBreakfastAndLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CottonKnockedUpPcAndTalkHappened, 1);

            ReturnToCamp(1);
        }

        public void VaginalBirth()
        {

            OutputText("Scenes.Cotton.VaginalBirth.WakeToLaborPains".Localized());

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Cotton.VaginalBirth.GroinPressureRending".Localized());

                Player.CreateVagina();
            }

            OutputText("Scenes.Cotton.VaginalBirth.MovementUnderSkin".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.PainlessBirthingStance".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.HeadEmergesQuickly".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.NewbornNursesAtTeat".Localized());

            var kid = Random.Shared.Next(3) + 1;

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.BoyWord".Localized());
            else
                if (kid == 2)
                    OutputText("Scenes.Cotton.VaginalBirth.GirlWord".Localized());
                else
                    OutputText("Scenes.Cotton.VaginalBirth.HermaphroditeWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.ExclaimLikePrefix".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HisFatherPossessiveWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.HerFatherPossessiveWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.QuotedFatherWord".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HeSubjectWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.SheSubjectWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.HairlessExceptOnSuffix".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HisHeadPossessiveWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.HerHeadPossessiveWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.HeadSparseDarkHair".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HisFeetPossessiveWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.HerFeetPossessiveWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.PristineHoovesAndTail".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HisBacksidePossessiveWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.HerBacksidePossessiveWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.BacksideWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.CaressNewbornFingers".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.CannotRaiseAtCamp".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.SonWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.DaughterWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.BundleUpPrefix".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HimBundleObjectWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.HerBundleObjectWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.TightBundleThenLeave".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.GateGuardsOddLook".Localized());

            if (kid == 1)
                OutputText("Scenes.Cotton.VaginalBirth.HimGuardsBundleWord".Localized());
            else
                OutputText("Scenes.Cotton.VaginalBirth.HerGuardsBundleWord".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.CrowdDispersesInside".Localized());

            OutputText("Scenes.Cotton.VaginalBirth.TrekToCottonApartment".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcIsADeadbeatCottonDad) > 0)
            {
                OutputText("Scenes.Cotton.VaginalBirth.CottonGlaresDisappointed".Localized());

                if (kid == 1)
                    OutputText("Scenes.Cotton.VaginalBirth.SonAgain".Localized());
                else
                    OutputText("Scenes.Cotton.VaginalBirth.DaughterAgain".Localized());

                OutputText("Scenes.Cotton.VaginalBirth.TakesChildHolding".Localized());

                if (kid == 1)
                    OutputText("Scenes.Cotton.VaginalBirth.HimHeldObjectWord".Localized());
                else
                    OutputText("Scenes.Cotton.VaginalBirth.HerHeldObjectWord".Localized());

                OutputText("Scenes.Cotton.VaginalBirth.CloseWithGraceSuffix".Localized());

                OutputText("Scenes.Cotton.VaginalBirth.FarewellUsesName".Localized());

                OutputText("Scenes.Cotton.VaginalBirth.SighWalkToCamp".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.CottonOldestKidGender, kid);

                    OutputText("Scenes.Cotton.VaginalBirth.AwkwardHugNurseryRoom".Localized());

                    if (kid == 1)
                        OutputText("Scenes.Cotton.VaginalBirth.HeWillContraction".Localized());
                    else
                        OutputText("Scenes.Cotton.VaginalBirth.SheWillContraction".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.HappyHerePromise".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.CottonParentBefore".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.DaddyWordAmused".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.AskAboutMother".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.VillageOfMorphs".Localized());

                    if (kid == 3)
                        OutputText("Scenes.Cotton.VaginalBirth.FamilyTraitRemark".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.SpeechCloseTag".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.NodThenAskRelationship".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.CottonThinksThenReplies".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.ThankThenChangeSubject".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.TwinsLeftTelAdre".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.WipesTearsMovesOn".Localized());

                    if (kid == 1)
                        OutputText("Scenes.Cotton.VaginalBirth.GuyWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VaginalBirth.GirlAgain".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.SituatedTakesBundle".Localized());

                    if (kid == 1)
                        OutputText("Scenes.Cotton.VaginalBirth.HimSwaddledObjectWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VaginalBirth.HerSwaddledObjectWord".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.SwaddledCribAdmired".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.WhispersBabyAllYou".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.WouldLikeToStay".Localized());
                }
                else
                {
                    OutputText("Scenes.Cotton.VaginalBirth.FamiliarNurseryHug".Localized());

                    if (kid == 1)
                        OutputText("Scenes.Cotton.VaginalBirth.HeCuteSubjectWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VaginalBirth.SheCuteSubjectWord".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.CuteMakeEmGood".Localized());

                    if (kid == 1)
                        OutputText("Scenes.Cotton.VaginalBirth.HeLoveSubjectWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VaginalBirth.SheLoveSubjectWord".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.LoveItHereSuffix".Localized());

                    if (kid == 1)
                        OutputText("Scenes.Cotton.VaginalBirth.HisFamilyPossessiveWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VaginalBirth.HerFamilyPossessiveWord".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.FamilyCottonDeclares".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.MoreTheMerrier".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.TapNosePlayfully".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.TakesTwoToTango".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.GrabsCottonBulge".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.BestThinkingJoke".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.AgreesAndSlaps".Localized());

                    OutputText("Scenes.Cotton.VaginalBirth.OfCourseTakesBundle".Localized());
                }

            OutputText("Scenes.Cotton.CottonTantricSex.ParagraphBreakFifth".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CottonKidCount, CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.CottonOldestKidAge, 1);

            Player.ClearPregnancy();
        }

        public bool UpdateVaginalPregnancy()
        {

            var displayedUpdate = false;

            if (Player.Pregnancy.Incubation == 320)
            {
                OutputText("Scenes.Cotton.UpdateVaginalPregnancy.BiggerBellyCravings".Localized());

                displayedUpdate = true;
            }
            else
                if (Player.Pregnancy.Incubation == 280)
                {
                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.BellyDistendedSuspect".Localized());

                    displayedUpdate = true;
                }
                else
                    if (Player.Pregnancy.Incubation == 225)
                    {
                        OutputText("Scenes.Cotton.UpdateVaginalPregnancy.VisiblePregnancyBulge".Localized());

                        displayedUpdate = true;
                    }
                    else
                        if (Player.Pregnancy.Incubation == 165)
                        {
                            OutputText("Scenes.Cotton.UpdateVaginalPregnancy.SuddenKickFromWomb".Localized());

                            displayedUpdate = true;
                        }
                        else
                            if (Player.Pregnancy.Incubation == 105)
                            {
                                OutputText("Scenes.Cotton.UpdateVaginalPregnancy.FullTermBellySize".Localized());

                                displayedUpdate = true;
                            }
                            else
                                if (Player.Pregnancy.Incubation == 80)
                                {
                                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.SwollenTwinsQuery".Localized());

                                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.GladButWorried".Localized());

                                    displayedUpdate = true;
                                }
                                else
                                    if (Player.Pregnancy.Incubation == 50)
                                    {
                                        OutputText("Scenes.Cotton.UpdateVaginalPregnancy.PainfulBellyBurst".Localized());

                                        displayedUpdate = true;
                                    }

            if (Player.Pregnancy.Incubation == 32 || Player.Pregnancy.Incubation == 64 || Player.Pregnancy.Incubation == 85 || Player.Pregnancy.Incubation == 150)
            {
                displayedUpdate = true;

                if (Player.BiggestBreastSize() >= 3 && Player.MostBreastsPerRow() > 1 && Player.BiggestLactation() >= 1 && Player.BiggestLactation() < 2)
                {
                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.MilkSwollenBreasts".Localized());

                    Player.BoostLactation(.5f);
                }

                if (Player.BiggestBreastSize() >= 3 && Player.MostBreastsPerRow() > 1 && Player.BiggestLactation() > 0 && Player.BiggestLactation() < 1)
                {
                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.MilkLeakingNipples".Localized());

                    Player.BoostLactation(.5f);
                }

                if (Player.BiggestBreastSize() >= 3 && Player.MostBreastsPerRow() > 1 && Player.BiggestLactation() == 0)
                {
                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.BreastsFullLactate".Localized());

                    Player.BoostLactation(1);
                }

                if (Player.BiggestBreastSize() == 2 && Player.MostBreastsPerRow() > 1)
                {
                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.CCupSwelling".Localized());

                    Player.GrowTits(1, 1, false, 3);
                }

                if (Player.BiggestBreastSize() == 1 && Player.MostBreastsPerRow() > 1)
                {
                    OutputText("Scenes.Cotton.UpdateVaginalPregnancy.BCupGrowth".Localized());

                    Player.GrowTits(1, 1, false, 3);
                }
            }

            return displayedUpdate;
        }

        public void VisitCottonKids()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.VisitCottonKids.CottonSkipExerciseAsk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                OutputText("Scenes.Cotton.VisitCottonKids.KidWord".Localized());
            else
                OutputText("Scenes.Cotton.VisitCottonKids.KidsWord".Localized());

            OutputText("Scenes.Cotton.VisitCottonKids.CottonGrinSurprise".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.HimWord".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.HerWord".Localized());
            }
            else
                OutputText("Scenes.Cotton.VisitCottonKids.ThemWord".Localized());

            OutputText("Scenes.Cotton.VisitCottonKids.NodAllRight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                OutputText("Scenes.Cotton.VisitCottonKids.ChildIsPhrase".Localized());
            else
                OutputText("Scenes.Cotton.VisitCottonKids.ChildrenArePhrase".Localized());

            OutputText("Scenes.Cotton.VisitCottonKids.DoingWord".Localized());

            var scene = Random.Shared.Next(4);

            if (scene == 0)
            {
                OutputText("Scenes.Cotton.VisitCottonKids.YourPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                    OutputText("Scenes.Cotton.VisitCottonKids.BabyGirlIsPhrase".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.BabyBoyIsPhrase".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.SittingUpInPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                    OutputText("Scenes.Cotton.VisitCottonKids.HerAgain".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.HisWord".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.CribCooingPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                    OutputText("Scenes.Cotton.VisitCottonKids.HerThird".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.HisAgain".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.BabyOwnWayPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                    OutputText("Scenes.Cotton.VisitCottonKids.SheWord".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.HeWord".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.GameLovingPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.BabyWord".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.BabiesWord".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.NoseBabblingPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.HimAgain".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.HerFourth".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.ParentTonePrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.ColtWord".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.FillyWord".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.CoverEyesPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.HeAgain".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.SheAgain".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.GaspDelightPrefix".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.YourBabySeemsPrefix".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.BabySurprisedPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                    OutputText("Scenes.Cotton.VisitCottonKids.HerFifth".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.HimThird".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.LaughingHardPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.HeThird".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.SheThird".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.LeftOutRejoinPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 1)
                {
                    OutputText("Scenes.Cotton.VisitCottonKids.YourOtherPrefix".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.BabyIsPhrase".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.BabiesArePhrase".Localized());

                    OutputText("Scenes.Cotton.VisitCottonKids.SecondChildJoyPrefix".Localized());
                }

                OutputText("Scenes.Cotton.VisitCottonKids.SleepApproachPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.SheGrowsTiredPhrase".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.HeGrowsTiredPhrase".Localized());
                }
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.TheyGrowTiredPhrase".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.StillGigglingPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.SLetter".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.SettleSleepPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.HerSixth".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.HimFourth".Localized());
                }
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.ThemAgain".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.TuckingPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.HerSeventh".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.HimFifth".Localized());
                }
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.BedtimeThemWord".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.MussingHairPhrase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.MussingHerHair".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.MussingHisHair".Localized());
                }
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.MussingTheirHair".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.HairGentlyWatchPhrase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.BedtimeSheGoes".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.BedtimeHeGoes".Localized());
                }
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.BedtimeTheyGo".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.GoodnightSmilePhrase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                    OutputText("Scenes.Cotton.VisitCottonKids.CheekKissKid".Localized());
                else
                    OutputText("Scenes.Cotton.VisitCottonKids.CheekKissKids".Localized());

                OutputText("Scenes.Cotton.VisitCottonKids.FarewellKissLeave".Localized());
            }
            else
                if (scene == 1)
                {
                    OutputText("Scenes.Cotton.VisitCottonKids.CribTurnsOutLead".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                        OutputText("Scenes.Cotton.VisitCottonKids.BabyIsFastAsleep".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.AllKidsAreAsleep".Localized());

                    OutputText("Scenes.Cotton.VisitCottonKids.RatherThanDisturb".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                        OutputText("Scenes.Cotton.VisitCottonKids.DisturbHerWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.DisturbHimWord".Localized());

                    OutputText("Scenes.Cotton.VisitCottonKids.CottonCribLean".Localized());

                    OutputText("Scenes.Cotton.VisitCottonKids.FlattererSmirkPhrase".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                        OutputText("Scenes.Cotton.VisitCottonKids.CheckOnBabyWord".Localized());
                    else
                        OutputText("Scenes.Cotton.VisitCottonKids.CheckOnBabiesWord".Localized());

                    OutputText("Scenes.Cotton.VisitCottonKids.RestTogetherPhrase".Localized());
                }
                else
                    if (scene == 2)
                    {
                        OutputText("Scenes.Cotton.VisitCottonKids.CottonSightLead".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.CottonSightDaughter".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.CottonSightSon".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.WailAndCryPhrase".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 1)
                            OutputText("Scenes.Cotton.VisitCottonKids.OtherKidsNotHungry".Localized());

                        if (Player.LactationQ() >= 50)
                        {
                            OutputText("Scenes.Cotton.VisitCottonKids.OfferToNurseLead".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) != 1)
                                OutputText("Scenes.Cotton.VisitCottonKids.NurseOfferDaughter".Localized());
                            else
                                OutputText("Scenes.Cotton.VisitCottonKids.NurseOfferSon".Localized());

                            OutputText("Scenes.Cotton.VisitCottonKids.NursePoorMarePhrase".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Feed".Localized(), FeedYourCottonKids);

                            AddButton(1, "Common.Button.StayQuiet".Localized(), LetCottonFeedKids);
                        }
                        else
                        {
                            ClearButtons();

                            AddButton(0, "Common.Button.Next".Localized(), LetCottonFeedKids);
                        }

                        return ;
                    }
                    else
                    {
                        OutputText("Scenes.Cotton.VisitCottonKids.FoalYourLead".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.FoalDaughterWord".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.FoalSonWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.WetEyesSnifflePhrase".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.SniffleSheWord".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.SniffleHeWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.TinySobsCottonCoos".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.LiftHerWord".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.LiftHimWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.UpIntoArmsPhrase".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.BigHeavyGirl".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.BigHeavyHeWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.BabyScaredTears".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.ShushHerWord".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.ShushHimWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.HoldFoalClose".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.ChestRockingPhrase".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.RockingHerWord".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.RockingHimWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.RockBackForthPhrase".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.CuddleHerWord".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.CuddleHimWord".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.SentencePeriod".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.MurmurSoftNothings".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                            OutputText("Scenes.Cotton.VisitCottonKids.LittleEarsHer".Localized());
                        else
                            OutputText("Scenes.Cotton.VisitCottonKids.LittleEarsHis".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.AdorableEarsFlick".Localized());

                        OutputText("Scenes.Cotton.VisitCottonKids.HoldChildClose".Localized());
                    }

            ReturnToCamp(1);
        }

        public void FeedYourCottonKids()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.FeedYourCottonKids.InterruptOfferNursing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.FillyWord".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.CottonWord".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.ForHerWantsSuffix".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.CottonBlinksGrateful".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerWord".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HimWord".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.ByThenChestExposed".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.TakeChildFromCotton".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerAgain".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HisWord".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.WeightInArmsSuffix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerThird".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HimAgain".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.InCloseSuffix".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.BabyConfusedWhimper".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerFourth".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HisAgain".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.PresenceMilkLetdown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerFifth".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HisThird".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.LipsAroundIt".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.MoanAsChildSuckles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerSixth".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HimThird".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.RockAndSingLullaby".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerSeventh".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HisFourth".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.LittleHeartBeating".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerEighth".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HisFifth".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.WonderIfHearsHeart".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.SheWord".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HeWord".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.HearHeartInsideYours".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.CottonMovesBehindYou".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) == 1)
                OutputText("Scenes.Cotton.FeedYourCottonKids.NeedsWord".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.ChildrenNeedSuffix".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.EnergyTheyCanGet".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.ChildReleasesNipple".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.SmilesEnjoyWhileLasts".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.SighAdmitThatsTrue".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.SheAgain".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HeAgain".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.SleepyThingNeedsBed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.FeedYourCottonKids.HerNinth".Localized());
            else
                OutputText("Scenes.Cotton.FeedYourCottonKids.HisSixth".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.CribSleepThenAsk".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.LaughsReassuresFine".Localized());

            OutputText("Scenes.Cotton.FeedYourCottonKids.KissLoverRuffleHair".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

            if (Player.HasStatusEffect(StatusEffectIds.Feeder))
            {
                Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
            }

            Player.BoostLactation(0.5f);

            ReturnToCamp(1);
        }

        public void LetCottonFeedKids()
        {
            ClearOutput();

            OutputText("Scenes.Cotton.LetCottonFeedKids.CottonBaresBreasts".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.SitWatchChildFeed".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.NeedMilkGrowBig".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.NoticeCottonMoans".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.HoldsThenHandsOff".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.TankTopLooksDown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.LetCottonFeedKids.HerNapAskSuffix".Localized());
            else
                OutputText("Scenes.Cotton.LetCottonFeedKids.HimNapAskSuffix".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.NodPlaceBabyCrib".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.LetCottonFeedKids.HerWord".Localized());
            else
                OutputText("Scenes.Cotton.LetCottonFeedKids.HisWord".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.CribRuffleSuffix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonOldestKidGender) >= 2)
                OutputText("Scenes.Cotton.LetCottonFeedKids.HerAgain".Localized());
            else
                OutputText("Scenes.Cotton.LetCottonFeedKids.HisAgain".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.HairComplimentFoals".Localized());

            OutputText("Scenes.Cotton.LetCottonFeedKids.ApologeticMustLeave".Localized());

            ReturnToCamp(1);
        }

    }
}