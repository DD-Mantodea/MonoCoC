using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Boat
{
    [Register<KaijuScene>("coc:kaiju")]
    public class KaijuScene : Scene
    {

        public override void Entry()
        {
            ExecEncounter();
        }

        // 原版 Kaiju.hx:19 encounterChance()：5 级后、黄昏、已发现船、未被永久拒绝时才出现
        public static float EncounterChance() =>

            CoC.Instance.Player.Level >= 5

            && CoC.Instance.Flags.Get(GameFlagIds.KaijuDisabled) == 0

            && View.Time.Hours >= 17

            && CoC.Instance.Player.HasStatusEffect(StatusEffectIds.BoatDiscovery)

                ? 1f

                : 0f;

        public void KaijuSprite()
        {

        }

        public void ExecEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KaijuMeetings) == 0)
            {
                ShowImage("event-island");

                OutputText("Scenes.Kaiju.ExecEncounter.ExplorationsTakeYou".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), MeetDatKaijuYo);

                AddButton(1, "Common.Button.No".Localized(), () => NoMeetingKaijus());

                AddButton(2, "Scenes.Faerie.SexMenu.ButtonNever".Localized(), () => NoMeetingKaijus(true));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2 && !KaijuCock())
                    KaijuGrowsWangus();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) >= 5)
                        KaijuBadEndToyBOOSH();
                    else
                        RepeatKaijuEncounter();
        }

        public bool KaijuCock()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KaijuCock) == 1;
        }

        public void NoMeetingKaijus(bool never = false)
        {
            ClearOutput();

            ShowImage("kaiju-get-off");

            OutputText("Scenes.Kaiju.NoMeetingKaijus.RowingAway".Localized());

            if (never)
            {
                OutputText("Scenes.Kaiju.NoMeetingKaijus.BetterToAvoid".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KaijuDisabled, 1);
            }

            ReturnToCamp(1);
        }

        public void MeetDatKaijuYo()
        {
            ClearOutput();

            ShowImage("kaiju-encounter");

            KaijuSprite();

            CoC.Instance.Flags.Set(GameFlagIds.KaijuMeetings, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                CoC.Instance.Flags.Set(GameFlagIds.KaijuCock, 1);

            OutputText("Scenes.Kaiju.MeetDatKaijuYo.StepUpHill".Localized());

            OutputText("Scenes.Kaiju.MeetDatKaijuYo.TremendousGulp".Localized());

            OutputText("Scenes.Kaiju.MeetDatKaijuYo.AsksCuppingBreasts".Localized());

            OutputText("Scenes.Kaiju.MeetDatKaijuYo.WhateverWeight".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Compliment".Localized(), ComplimentKaiju);

            if (!Player.IsGenderless())
                AddButton(1, "Common.Button.Flirt".Localized(), FlirtWithKaiju);

            AddButton(2, "Common.Button.Insult".Localized(), InsultTheKaijuFirstMeeting);
        }

        public void InsultTheKaijuFirstMeeting()
        {
            ClearOutput();

            if (KaijuCock())
            {
                ShowImage("kaiju-angry");

                OutputText("Scenes.Kaiju.InsultTheKaijuFirstMeeting.NotBrightestIdea".Localized());

                OutputText("Scenes.Kaiju.InsultTheKaijuFirstMeeting.SaysInHuff".Localized());

                OutputText("Scenes.Kaiju.InsultTheKaijuFirstMeeting.TellsYouRoughly".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire) && (!Player.HasPerk(PerkIds.BloodMage) && Player.Fatigue + Player.SpellCost(30) > 100))
                {
                    OutputText("Scenes.Kaiju.InsultTheKaijuFirstMeeting.SensingDesires".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Yes".Localized(), YesBurnDatClit);

                    AddButton(1, "Common.Button.No".Localized(), CorruptKaijuInsertion);
                }
                else
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), CorruptKaijuInsertion);
                }
            }
            else
            {
                ShowImage("kaiju-get-off");

                OutputText("Scenes.Kaiju.InsultTheKaijuFirstMeeting.NotSmartestThing".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KaijuDisabled, 1);

                ReturnToCamp(1);
            }
        }

        public void YesBurnDatClit()
        {
            ClearOutput();

            ShowImage("kaiju-get-off");

            Player.ChangeFatigue(30, FatigueType.Physical);

            OutputText("Scenes.Kaiju.YesBurnDatClit.NarrowsEyes".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            CoC.Instance.Flags.Set(GameFlagIds.KaijuDisabled, 1);

            ReturnToCamp(1);
        }

        public void CorruptKaijuInsertion()
        {
            ClearOutput();

            ShowImage("kaiju-cocked");

            OutputText("Scenes.Kaiju.CorruptKaijuInsertion.ShovesFaceInto".Localized());

            OutputText("Scenes.Kaiju.CorruptKaijuInsertion.NoFightingAgainst".Localized());

            OutputText("Scenes.Kaiju.CorruptKaijuInsertion.GreenTerrapinSays".Localized());

            OutputText("Scenes.Kaiju.CorruptKaijuInsertion.AlmostAnHour".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f), new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.KaijuDisabled, 1);

            ReturnToCamp(1);
        }

        public void ComplimentKaiju()
        {
            ClearOutput();

            ShowImage("kaiju-encounter");

            OutputText("Scenes.Kaiju.ComplimentKaiju.ComplimentFigure".Localized());

            OutputText("Scenes.Kaiju.ComplimentKaiju.SmileForms".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LetHer".Localized(), LetKaijuHaveWayWithYou);

            AddButton(1, "Common.Button.StopIt".Localized(), StopItPlease);
        }

        public void StopItPlease()
        {
            ClearOutput();

            ShowImage("kaiju-get-off");

            OutputText("Scenes.Kaiju.StopItPlease.CriesOutWaves".Localized());

            ReturnToCamp(1);
        }

        public void LetKaijuHaveWayWithYou()
        {
            ClearOutput();

            OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.MakesNoMoves".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.Stiffening".Localized());

            OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.SentencePeriod".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.VaginaMoistens".Localized());

            OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.GiantBeautySays".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.CocksAndVagina".Localized());
            else
                if (Player.Gender == Gender.Male)
                    OutputText(Player.MultiCockDescriptLight());
                else
                    if (Player.Gender == Gender.Female)
                        OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.VaginaWord".Localized());
                    else
                        OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.BareGroin".Localized());

            OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.LetsHaveFun".Localized());

            if (Player.HasCock())
            {
                ShowImage("kaiju-cocked");

                OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.BringsYouUp".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.PluralS".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.AndBalls".Localized());

                OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.RestOnNaughty".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.VaginaAches".Localized());

                Player.Orgasm("Dick");
            }
            else
                if (Player.HasVagina())
                {
                    ShowImage("kaiju-boobjob");

                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.SpreadsLegs".Localized());

                    Player.CuntChange(14, true, true, false);

                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.SomethingWetLeaks".Localized());

                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.NipplePopsOut".Localized());

                    Player.Orgasm("Vaginal");
                }
                else
                {
                    OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.GiantessMoans".Localized());

                    Player.ChangeFatigue(-50);
                }

            OutputText("Scenes.Kaiju.LetKaijuHaveWayWithYou.SaysSlowlyPuttingDown".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void FlirtWithKaiju()
        {
            ClearOutput();

            OutputText("Scenes.Kaiju.FlirtWithKaiju.TriesToFlirt".Localized());

            OutputText("Scenes.Kaiju.FlirtWithKaiju.CurvyTortoiseSays".Localized());

            if (KaijuCock())
                OutputText("Scenes.Kaiju.FlirtWithKaiju.SurprisedToSee".Localized());

            OutputText("Scenes.Kaiju.FlirtWithKaiju.MoistPussyLarge".Localized(
                Player.Mf("stud", "gal")
            ));

            OutputText("Scenes.Kaiju.FlirtWithKaiju.PushesYouTowards".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            ShowImage("kaiju-fuck");

            OutputText("Scenes.Kaiju.FlirtWithKaiju.StillHasYou".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Kaiju.FlirtWithKaiju.SlidesAcrossSlick".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Kaiju.FlirtWithKaiju.EachCockWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.AreWord".Localized());
                else
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.IsWord".Localized());

                OutputText("Scenes.Kaiju.FlirtWithKaiju.ThrobbingFromStimulants".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.PluralS".Localized());

                OutputText("Scenes.Kaiju.FlirtWithKaiju.SentencePeriod".Localized());
            }

            OutputText("Scenes.Kaiju.FlirtWithKaiju.JustWhenYouBegin".Localized());

            if (Player.Sensitivity < 50)
            {
                OutputText("Scenes.Kaiju.FlirtWithKaiju.SensationsOfGiant".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.SpasmsReleaseLoad".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.OwnPussyReleases".Localized());

                Player.Orgasm("Generic");
            }

            OutputText("Scenes.Kaiju.FlirtWithKaiju.VaginalMusclesClamp".Localized());

            if (KaijuCock())
            {
                ShowImage("kaiju-blowjob");

                OutputText("Scenes.Kaiju.FlirtWithKaiju.GreenHermProclaims".Localized());

                if ((int)Player.Gender > 0)
                {
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.GrindPrivates".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Kaiju.FlirtWithKaiju.PressAgainstCock".Localized());

                    if (Player.LactationQ() >= 200)
                        OutputText("Scenes.Kaiju.FlirtWithKaiju.MilkSpurtsFlows".Localized());
                }

                OutputText("Scenes.Kaiju.FlirtWithKaiju.MoaningGiantessSays".Localized());

                if ((int)Player.Gender > 0)
                {
                    OutputText("Scenes.Kaiju.FlirtWithKaiju.UndeterredYouContinue".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Kaiju.FlirtWithKaiju.FeminineJuicesWord".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Kaiju.FlirtWithKaiju.AndWord".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Kaiju.FlirtWithKaiju.ThickCumWord".Localized());

                    OutputText("Scenes.Kaiju.FlirtWithKaiju.LookOutUponLake".Localized());

                    Player.Orgasm("Vaginal");
                }
            }

            OutputText("Scenes.Kaiju.FlirtWithKaiju.GiantsKneesSeem".Localized());

            OutputText("Scenes.Kaiju.FlirtWithKaiju.WaveGoodbye".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void RepeatKaijuEncounter()
        {
            ClearOutput();

            KaijuSprite();

            ShowImage("kaiju-encounter");

            OutputText("Scenes.Kaiju.RepeatKaijuEncounter.RowThroughLake".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Peek".Localized(), PeekAtSomePhatAssKaijuButt);

            AddButton(1, "Common.Button.HugBoobs".Localized(), KaijuRepeatBoobHug);

            AddButton(2, "Common.Button.Fuck".Localized(), FuckThisGiantYouDumbCunt);

            if (KaijuCock())
                AddButton(3, "Common.Button.Urethrafuck".Localized(), UrethraFuckDatGiantCock);
            else
                if (Player.ItemCount(ConsumableIds.IncubiDraft) + Player.ItemCount(ConsumableIds.PurifiedIncubiDraft) >= 10)
                {
                    OutputText("Scenes.Kaiju.RepeatKaijuEncounter.OccursToYou".Localized());

                    AddButton(3, "Common.Button.GiveCock".Localized(), YesTurnKaijuFuta);
                }
                else
                    AddButton(3, "Common.Button.GiveCock".Localized(), null);

            AddButton(4, "Common.Button.Talk".Localized(), TalkToKaiju);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveRepeatKaiju);
        }

        public void KaijuRepeatBoobHug()
        {
            ClearOutput();

            ShowImage("kaiju-boobjob");

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.DecideWouldBeFun".Localized());

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.SmileForms".Localized());

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.MakesNoMoves".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.Stiffening".Localized());

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.SentencePeriod".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.VaginaMoistens".Localized());

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.GiantBeautySays".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());

            if (Player.IsHerm())
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.CocksAndVagina".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.VaginaWord".Localized());

            if (Player.IsGenderless())
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.BareGroin".Localized());

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.LetsHaveFun".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.BringsYouUp".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.PluralS".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.AndBalls".Localized());

                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.RestOnNaughty".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.VaginaAches".Localized());

                Player.Orgasm("Dick");
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.SpreadsLegs".Localized());

                Player.CuntChange(14, true, true, false);

                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.SomethingWetLeaks".Localized());

                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.NipplePopsOut".Localized());

                Player.Orgasm("Vaginal");
            }

            if (Player.IsGenderless())
                OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.GiantessMoans".Localized());

            OutputText("Scenes.Kaiju.KaijuRepeatBoobHug.SaysSlowlyPuttingDown".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            ReturnToCamp(1);
        }

        public void FuckThisGiantYouDumbCunt()
        {
            ClearOutput();

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.DecideWouldBeFun".Localized());

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.CurvyTortoiseSays".Localized());

            if (KaijuCock())
                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.SurprisedToSee".Localized());

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.MoistPussyLarge".Localized());

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.PushesYouTowards".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            ShowImage("kaiju-fuck");

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.StillHasYou".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.SlidesAcrossSlick".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.YourHardCock".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.SAreWord".Localized());
                else
                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.IsSpaceWord".Localized());

                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.ThrobbingFromStimulants".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.PluralS".Localized());

                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCunt.SentencePeriod".Localized());
            }

            FuckThisGiantYouDumbCuntt();
        }

        public void FuckThisGiantYouDumbCuntt()
        {
            ClearOutput();

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.JustWhenYouBegin".Localized());

            if (Player.Sensitivity >= 50)
            {
                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.SensationsOfGiant".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.SpasmsReleaseLoad".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.OwnPussyReleases".Localized());

                Player.Orgasm("Generic");
            }
            else
            {
                OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.VaginalMusclesClamp".Localized());

                if (KaijuCock())
                {
                    ShowImage("kaiju-blowjob");

                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.GreenHermProclaims".Localized());

                    if ((int)Player.Gender > 0)
                        OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.GrindPrivates".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.PressAgainstCock".Localized());

                    if (Player.LactationQ() >= 200)
                        OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.MilkSpurtsFlows".Localized());

                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.MoaningGiantessSays".Localized());

                    if ((int)Player.Gender > 0)
                    {
                        OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.UndeterredYouContinue".Localized());

                        if (Player.HasVagina())
                            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.FeminineJuicesWord".Localized());

                        if (Player.Gender == Gender.Hermaphrodite)
                            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.AndWord".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.ThickCumWord".Localized());

                        OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.SentencePeriod".Localized());

                        Player.Orgasm("Vaginal");
                    }

                    OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.LookOutUponLake".Localized());
                }
            }

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.GiantsKneesSeem".Localized());

            OutputText("Scenes.Kaiju.FuckThisGiantYouDumbCuntt.WaveGoodbye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void UrethraFuckDatGiantCock()
        {
            ClearOutput();

            ShowImage("kaiju-cocked");

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.InquisitiveStare".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.VenusThinks".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.WaitSheWants".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.BeforeComment".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.SaysBlushing".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.LookLikeWhore".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.QuicklyPositioned".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.AndYourCocks".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.GiantessSaysGlee".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.JudgingCooing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.JizzFillsAss".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.AssNotAlone".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.ReachYourOwn".Localized());

            OutputText("Scenes.Kaiju.UrethraFuckDatGiantCock.WakeUpHour".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void LeaveRepeatKaiju()
        {
            ClearOutput();

            ShowImage("kaiju-get-off");

            OutputText("Scenes.Kaiju.LeaveRepeatKaiju.PolitelyDecline".Localized());

            ReturnToCamp(1);
        }

        public void TalkToKaiju()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KaijuTalkCycle, CoC.Instance.Flags.Get(GameFlagIds.KaijuTalkCycle) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) - 1);

            OutputText("Scenes.Kaiju.TalkToKaiju.TakenBySurprise".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KaijuTalkCycle) == 1)
            {
                OutputText("Scenes.Kaiju.TalkToKaiju.TalkAbout".Localized());

                OutputText("Scenes.Kaiju.TalkToKaiju.NoteOfSadness".Localized());

                OutputText("Scenes.Kaiju.TalkToKaiju.HappyRecounting".Localized());

                OutputText("Scenes.Kaiju.TalkToKaiju.NotMuchHappened".Localized());

                OutputText("Scenes.Kaiju.TalkToKaiju.GrowSilent".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KaijuTalkCycle) == 2)
                {
                    OutputText("Scenes.Kaiju.TalkToKaiju.AskHerA".Localized());

                    OutputText("Scenes.Kaiju.TalkToKaiju.BigGreenGalSays".Localized());

                    OutputText("Scenes.Kaiju.TalkToKaiju.BodyQuivers".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                        OutputText("Scenes.Kaiju.TalkToKaiju.YummyFluidsFlow".Localized());
                    else
                        if (KaijuCock())
                            OutputText("Scenes.Kaiju.TalkToKaiju.PouringLiquids".Localized());

                    OutputText("Scenes.Kaiju.TalkToKaiju.ThankGiantess".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KaijuTalkCycle) == 3)
                    {
                        OutputText("Scenes.Kaiju.TalkToKaiju.AskIfThere".Localized());

                        OutputText("Scenes.Kaiju.TalkToKaiju.SheSaysDemonstrating".Localized());

                        OutputText("Scenes.Kaiju.TalkToKaiju.TurtleSociety".Localized());

                        OutputText("Scenes.Kaiju.TalkToKaiju.SighsConversation".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.KaijuTalkCycle, 0);
                    }

            ReturnToCamp(1);

            if (Player.Intelligence < 50)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (Player.Libido > 50)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));
        }

        public void PeekAtSomePhatAssKaijuButt()
        {
            ClearOutput();

            ShowImage("kaiju-peek");

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.GreenTitanWoman".Localized());

            OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.BeginsToProbe".Localized());

            if (!KaijuCock())
                OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.ThumbRub".Localized());
            else
                OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.ThumbRubbingUp".Localized());

            OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.OtherHandHolding".Localized());

            if (!KaijuCock())
                OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.OtherHandMovesAway".Localized());
            else
                OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.OtherHandDarts".Localized());

            OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.PumpsClenchedFist".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 3)));

            if (KaijuCock())
            {
                OutputText("Scenes.Kaiju.PeekAtSomePhatAssKaijuButt.AppearsToBeEnding".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), YesKaijuGimmePeepShowsMoar);

                AddButton(1, "Common.Button.No".Localized(), NoKaijuPeepShows);
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), NoKaijuPeepShows);
            }
        }

        public void YesKaijuGimmePeepShowsMoar()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            ShowImage("kaiju-geyser");

            OutputText("Scenes.Kaiju.YesKaijuGimmePeepShowsMoar.AppreciationOfHer".Localized());

            OutputText("Scenes.Kaiju.YesKaijuGimmePeepShowsMoar.BeginsToMoanAgain".Localized());

            OutputText("Scenes.Kaiju.YesKaijuGimmePeepShowsMoar.PantsBringingHand".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 33f));

            ReturnToCamp(1);
        }

        public void NoKaijuPeepShows()
        {
            ClearOutput();

            ShowImage("kaiju-get-off");

            OutputText("Scenes.Kaiju.NoKaijuPeepShows.BreathingHeavily".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            ReturnToCamp(1);
        }

        public void KaijuGrowsWangus()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KaijuCock, 1);

            ShowImage("kaiju-cocked");

            KaijuSprite();

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            OutputText("Scenes.Kaiju.KaijuGrowsWangus.RowingFamiliar".Localized());

            OutputText("Scenes.Kaiju.KaijuGrowsWangus.AppraisingYou".Localized());

            OutputText("Scenes.Kaiju.KaijuGrowsWangus.NeverExpected".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.No".Localized(), DontGetFutaTurtlesOffToday);

            AddButton(0, "Common.Button.Yes".Localized(), HelpNewFutaKaijuGetOff);

            AddButton(2, "Common.Button.Mock".Localized(), MockDatTurtleGirl);
        }

        public void DontGetFutaTurtlesOffToday()
        {
            ClearOutput();

            ShowImage("kaiju-get-off");

            OutputText("Scenes.Kaiju.DontGetFutaTurtlesOffToday.ShakeHeadNo".Localized());

            ReturnToCamp(1);
        }

        public void MockDatTurtleGirl()
        {
            ClearOutput();

            ShowImage("kaiju-mocking");

            OutputText("Scenes.Kaiju.MockDatTurtleGirl.LaughAtGiant".Localized());

            OutputText("Scenes.Kaiju.MockDatTurtleGirl.BeginsToMoveHand".Localized());

            if (Player.Balls > 0 && Player.BallSize >= 6)
                OutputText("Scenes.Kaiju.MockDatTurtleGirl.RestOnHermaphrodite".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.MockDatTurtleGirl.GettingTighter".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.MockDatTurtleGirl.BeginToFeelWetness".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Kaiju.MockDatTurtleGirl.CheeksBlush".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Kaiju.MockDatTurtleGirl.CantBelieve".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Kaiju.MockDatTurtleGirl.WowThisIsArousing".Localized());
                else
                    OutputText("Scenes.Kaiju.MockDatTurtleGirl.WantToGetOff".Localized());

            OutputText("Scenes.Kaiju.MockDatTurtleGirl.GiantessCoos".Localized());

            OutputText("Scenes.Kaiju.MockDatTurtleGirl.CraningNeck".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.MockDatTurtleGirl.CumInArmor".Localized());

            OutputText("Scenes.Kaiju.MockDatTurtleGirl.FeelSpasming".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.KaijuDisabled, 1);

            ReturnToCamp(1);
        }

        public void HelpNewFutaKaijuGetOff()
        {
            ClearOutput();

            ShowImage("kaiju-cocked");

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 1);

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.SmileUpAtGreen".Localized());

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.EndingInspection".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.QuicklyHardens".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.WarmHeatRadiates".Localized());

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.SquirmsUnderTouch".Localized());

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.FinallyReachGreen".Localized());

            if (Player.Corruption < 50 && Player.Libido < 75)
                OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.KneadCockHead".Localized());
            else
                OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.ClimbFurtherUp".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.MilkDribbles".Localized());

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.ShoutsBeforeLoad".Localized());

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.TorrentOfCream".Localized());

            OutputText("Scenes.Kaiju.HelpNewFutaKaijuGetOff.CleanUpDressed".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void IncubiDraftsDatKaiju()
        {
            ClearOutput();

            ShowImage("kaiju-encounter");

            OutputText("Scenes.Kaiju.IncubiDraftsDatKaiju.AskBigGreenSlut".Localized());

            OutputText("Scenes.Kaiju.IncubiDraftsDatKaiju.EyesGoWide".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), YesTurnKaijuFuta);

            AddButton(1, "Common.Button.No".Localized(), RepeatKaijuEncounter);
        }

        public void YesTurnKaijuFuta()
        {
            ClearOutput();

            ShowImage("kaiju-cocked");

            CoC.Instance.Flags.Set(GameFlagIds.KaijuCock, 1);

            KaijuSprite();

            CoC.Instance.Flags.Set(GameFlagIds.KaijuBadEndCounter, CoC.Instance.Flags.Get(GameFlagIds.KaijuBadEndCounter) + 2);

            var counter = 10;

            while (counter > 0)
            {
                counter--;

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));
                else
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)))
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.IncubiDraft));
            }

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.WouldntHaveOffered".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.HandsTooLarge".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.ExclaimsExpectantly".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.LookAtHerWith".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.DontLookConcerned".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.WaitSheWants".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.BeforeComment".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.SaysBlushing".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.QuicklyPositioned".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.YesTurnKaijuFuta.AndYourCocks".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.GiantessSaysGlee".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.JudgingCooing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.YesTurnKaijuFuta.JizzFillsAss".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.YesTurnKaijuFuta.AssNotAlone".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.ReachYourOwn".Localized());

            OutputText("Scenes.Kaiju.YesTurnKaijuFuta.WakeUpHour".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void KaijuBadEndToyBOOSH()
        {
            ClearOutput();

            ShowImage("kaiju-cocked");

            KaijuSprite();

            OutputText("Scenes.Kaiju.KaijuBadEndToyBOOSH.RowingThrough".Localized());

            if (!KaijuCock())
                OutputText("Scenes.Kaiju.KaijuBadEndToyBOOSH.ClitorisWord".Localized());
            else
                OutputText("Scenes.Kaiju.KaijuBadEndToyBOOSH.CockWord".Localized());

            OutputText("Scenes.Kaiju.KaijuBadEndToyBOOSH.PalmOfHand".Localized());

            OutputText("Scenes.Kaiju.KaijuBadEndToyBOOSH.UnsureWhether".Localized());

            if (Player.CanFly())
            {
                OutputText("Scenes.Kaiju.KaijuBadEndToyBOOSH.NotTooLate".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), FlyAwayFromBadEnd);

                AddButton(1, "Common.Button.No".Localized(), BadEndPartTwo);
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), BadEndPartTwo);
            }
        }

        public void FlyAwayFromBadEnd()
        {
            ClearOutput();

            ShowImage("kaiju-fly-away");

            OutputText("Scenes.Kaiju.FlyAwayFromBadEnd.FlappingWings".Localized());

            ReturnToCamp(1);
        }

        public void BadEndPartTwo()
        {
            ClearOutput();

            OutputText("Scenes.Kaiju.BadEndPartTwo.MakesShortWork".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Kaiju.BadEndPartTwo.ForkedTongueLashing".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id.Path != "human")
                {
                    OutputText("Scenes.Kaiju.BadEndPartTwo.LongTongueInMouth".Localized());

                    OutputText("Scenes.Kaiju.BadEndPartTwo.PinkTipRefuses".Localized());

                    OutputText("Scenes.Kaiju.BadEndPartTwo.DeterminedStrength".Localized());

                    OutputText("Scenes.Kaiju.BadEndPartTwo.RememberingEndOf".Localized());
                }

            if (BodyPartTypes.TongueIndex(Player.Body.Get<TonguePart>().Id) <= BodyPartTypes.TongueSnake)
                OutputText("Scenes.Kaiju.BadEndPartTwo.SuckProdigiousNipple".Localized());

            OutputText("Scenes.Kaiju.BadEndPartTwo.LustyGiantSays".Localized());

            if (KaijuCock())
            {
                OutputText("Scenes.Kaiju.BadEndPartTwo.FaceToFace".Localized());

                OutputText("Scenes.Kaiju.BadEndPartTwo.PushingHeadDeep".Localized());

                OutputText("Scenes.Kaiju.BadEndPartTwo.MadLook".Localized());

                OutputText("Scenes.Kaiju.BadEndPartTwo.ThatsWhenYouFeel".Localized());

                Player.ButtChange(500, true, true, false);

                ShowImage("badend-kaiju-corrupt");

                OutputText("Scenes.Kaiju.BadEndPartTwo.CockheadEnters".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Kaiju.BadEndPartTwo.ProstatePressed".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Kaiju.BadEndPartTwo.EvenIfCuntEmpty".Localized());

                OutputText("Scenes.Kaiju.BadEndPartTwo.ImpossibleIntruder".Localized());

                OutputText("Scenes.Kaiju.BadEndPartTwo.GiantHermShouts".Localized());

                OutputText("Scenes.Kaiju.BadEndPartTwo.SomeMinutesPass".Localized());
            }

            BadEndPartThree();
        }

        public void BadEndPartThree()
        {
            ClearOutput();

            ShowImage("badend-kaiju");

            OutputText("Scenes.Kaiju.BadEndPartThree.LustyTurtleComment".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.DrenchedInTurtle".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.FirstPlantyCock".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Kaiju.BadEndPartThree.InchesSlideIn".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.BadEndPartThree.StiffensTreeTendril".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kaiju.BadEndPartThree.FirstVineCock".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.NextSinuousCock".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Kaiju.BadEndPartThree.AnotherTendrilSnakes".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.MoreTentaclesSquirm".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kaiju.BadEndPartThree.MoreTentaclesArrive".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.IntensePleasureToo".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.ManyHoursBefore".Localized());

            OutputText("Scenes.Kaiju.BadEndPartThree.MagicOrGods".Localized());

            Player.Orgasm("Generic");

            Player.Orgasm("Anal", false);

            Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 30f), new DynStat(GameStats.Corruption, 4f));

            GameOver();
        }

    }
}