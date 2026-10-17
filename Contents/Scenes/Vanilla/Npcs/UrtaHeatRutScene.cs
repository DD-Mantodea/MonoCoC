using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<UrtaHeatRutScene>("coc:urta_heat_rut")]
    public class UrtaHeatRutScene : Scene
    {
        public UrtaScene urta => SceneInstance<UrtaScene>.Get();

        public override void Entry()
        {
            UrtaQuestDone();
        }

        public bool UrtaQuestDone()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == 1;
        }

        public void ApproachSoberUrtaHeatRutProc()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.WalkOverTheBar".Localized());

            if (Player.InHeat && Player.InRut && Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.EveryoneStares".Localized());
            else
                if (Player.InRut && Player.HasCock())
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.MalesStareWithA".Localized());
                else
                    if (Player.InHeat && Player.HasVagina())
                        OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.MalesAndAndrogynes".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.IgnoreThemAll".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.AsYouApproach".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.OpensArmsToEmbrace".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.SmilesSeesYou".Localized());

            if (Player.InRut && Player.HasCock())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.UrtaRightNextYou".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.KeepRemindingYourself".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.NotMentionWithOr".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.WellDoHelpMe".Localized());

            if (Player.InHeat && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.YouSmellGoodI".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.LeansCloserAnd".Localized());

            if (Player.InRut && Player.HasCock())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.EyesDrawnTo".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.LoseYourselfInImagination".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.CoughsAndRepeats".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.YourAdorableHotVixen".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.SomeoneKnowAndTrust".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.RightQuestion".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.NodLittleToo".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.AsSheLeadsYou".Localized());

            if (Player.InRut && Player.HasCock())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.FocusOnTheHips".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.FocusOnGlimpse".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.AggressiveHormones".Localized());

            if (Player.InRut && Player.InHeat && Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.PutBabyHerThen".Localized());
            else
                if (Player.InRut && Player.HasCock())
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.PutBabyHerSo".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.PutThatBigCock".Localized());

            if (!UrtaQuestDone())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.EvenIfSomethingReminds".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.SentencePeriod".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.YouKnowImGlad".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.ReallyThankfulYouManaged".Localized());
            else
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.ReallyFlatteredYouThought".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.InstinctsGetABit".Localized());

            if (Player.InRut && Player.InHeat && Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.MixedGenitals".Localized());
            else
                if (Player.InRut && Player.HasCock())
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.CockBiggest".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.VaginaWord".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.AgainstHerBackside".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.HornyTooButWait".Localized());

            if (Player.InRut && Player.InHeat && Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.TurnUrtaAroundSee".Localized());

                OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.LooksLikeShesAbout".Localized());

                ClearButtons();

                if (Player.HasCockThatFits(SceneInstance<UrtaScene>.Get().UrtaCapacity()))
                    AddButton(0, "Common.Button.Cock".Localized(), SateRutWithSoberUrta);
                else
                    AddButton(0, "Common.Button.Cock".Localized(), SateRutWithSoberUrtaButHuegDicked);

                AddButton(1, "Common.Button.Pussy".Localized(), SoberUrtaSatingPCHeat);
            }
            else
                if (Player.InRut && Player.HasCock())
                {
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.RoughlyShoveFingers".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.YourAndAreFilling".Localized());
                    else
                        OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.BeginsToSeepPrecum".Localized());

                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.DecideToDisposeOf".Localized());

                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.UrtaSaysThough".Localized());

                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.ShedYouApproach".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.PussyDrippingAndEager".Localized());

                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.SentencePeriodAgain".Localized());

                    ClearButtons();

                    if (Player.HasCockThatFits(SceneInstance<UrtaScene>.Get().UrtaCapacity()))
                        AddButton(0, "Common.Button.Next".Localized(), SateRutWithSoberUrta);
                    else
                        AddButton(0, "Common.Button.Next".Localized(), SateRutWithSoberUrtaButHuegDicked);
                }
                else
                {
                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.FirmlyGrabHerBreast".Localized());

                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.KnowWhatItsLike".Localized());

                    OutputText("Scenes.Urtaheatrut.ApproachSoberUrtaHeatRutProc.DisposeOfYourAlready".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), SoberUrtaSatingPCHeat);
                }
        }

        public void ApproachDrunkenUrta()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.WalkOverTheBar".Localized());

            if (Player.InHeat && Player.InRut && Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.EveryoneStares".Localized());
            else
                if (Player.InRut && Player.HasCock())
                    OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.MalesStareWithA".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.MalesAndAndrogynes".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.IgnoreThemAll".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.SheHasAlreadySeen".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.MyMyYouSmell".Localized());

            OutputText("Scenes.Urtaheatrut.ApproachDrunkenUrta.HermaphroditeSeemsABit".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Notinterested".Localized(), NotInterestedInUburDrunkUrtaRuts);

            AddButton(0, "Common.Button.Interested".Localized(), InterestedInUburDrunkurtaRuts);
        }

        public void NotInterestedInUburDrunkUrtaRuts()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.NotInterestedInUburDrunkUrtaRuts.SayItsHerWho".Localized());

            if (Player.Body.Get<FacePart>().Id == BodyPartTypes.Dog)
                OutputText("Scenes.Urtaheatrut.NotInterestedInUburDrunkUrtaRuts.NosePicksUp".Localized());

            if (Player.InRut && Player.HasCock())
                OutputText("Scenes.Urtaheatrut.NotInterestedInUburDrunkUrtaRuts.TwitchesInProtest".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Urtaheatrut.NotInterestedInUburDrunkUrtaRuts.TasteTheAir".Localized());

            if (Player.InRut && Player.HasCock())
                OutputText("Scenes.Urtaheatrut.NotInterestedInUburDrunkUrtaRuts.HardensInYourPants".Localized());
            else
                OutputText("Scenes.Urtaheatrut.NotInterestedInUburDrunkUrtaRuts.ConditionSeemsKindOf".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void InterestedInUburDrunkurtaRuts()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.RaiseAnEyebrow".Localized());

            if (Player.InRut && Player.InHeat && Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.GodsSexyYouSmell".Localized());
            else
                if (Player.InRut && Player.HasCock())
                    OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.LetPutLikeThis".Localized());
                else
                    if (Player.InHeat && Player.HasVagina())
                        OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.PutThisAgain".Localized());

            if (Player.InRut && Player.InHeat && Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.ShuddersWithDesire".Localized());

                if (!Quests.UrtaQuestScene.UrtaLove())
                    OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.UnderHerSkirt".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.ThroughSoakedDress".Localized());

                OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.BegsYouDesperately".Localized());

                OutputText("Scenes.Urtaheatrut.InterestedInUburDrunkurtaRuts.PresentHerWithYour".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Cock".Localized(), () => DrunkUrtaIntroPartDuex(1));

                AddButton(1, "Common.Button.Vagina".Localized(), () => DrunkUrtaIntroPartDuex(2));

                AddButton(2, "Common.Button.Watch".Localized(), WatchDrunkRuturtaJerkoff);
            }
            else
                if (Player.InRut && Player.HasCock())
                    DrunkUrtaIntroPartDuex(1, false);
                else
                    DrunkUrtaIntroPartDuex(2, false);
        }

        public void DrunkUrtaIntroPartDuex(int chosenSex = 1, bool newl = true)
        {
            ClearOutput();

            if (!(newl))
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.ParagraphBreak".Localized());

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.HandDashesTo".Localized());

            if (Player.Balls > 0 && chosenSex == 1)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.RollingThemInHand".Localized());
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.RubbingItUntil".Localized());

                    if (chosenSex == 1)
                        OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.SameStateSeemsTo".Localized());

                    OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.ErectAndThrobbing".Localized());
                }
                else
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.StrokingAlongTowards".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.FoxShutsYouUp".Localized());

            if (chosenSex == 1)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.AlreadyGetsHard".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.FeelTheInsideOf".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.BitSurprisedBy".Localized());

            if (Player.InRut && Player.InHeat && Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.ProceedsToMaul".Localized());
            else
                if (chosenSex == 1)
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.ButSheDoesntSeem".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.MoreThanIntentOn".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.StartsToStruggle".Localized());

            if (chosenSex == 1)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.HardWord".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.WetWord".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.MomentsLaterAsYou".Localized());

            if (chosenSex == 1)
                OutputText(Player.MultiCockDescriptLight());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.VaginaWord".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.SmileOpensWider".Localized());

            if (chosenSex == 1)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.SaysAsSheBrings".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaIntroPartDuex.GrowlsWithLust".Localized());

            ClearButtons();

            if (chosenSex == 1)
            {
                if (Player.HasCockThatFits(SceneInstance<UrtaScene>.Get().UrtaCapacity()))
                    AddButton(0, "Common.Button.Next".Localized(), DrunkUrtaRidesARutPCsCock);
                else
                    AddButton(0, "Common.Button.Next".Localized(), SateRutWithDrunkUrtaWithHugeDick);
            }
            else
                AddButton(0, "Common.Button.Next".Localized(), DrunkenUrtaFucksPCInHeat);
        }

        public void WatchDrunkRuturtaJerkoff()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.DecideThatYouWant".Localized());

            OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.MassiveEquinePhallus".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.PullsOffHerSkirt".Localized());
            else
                OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.RoughlyTearsDress".Localized());

            OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.ExposingFlowingMember".Localized());

            OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.SuppressAGiggle".Localized());

            OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.SlowlyLanguidlyStart".Localized());

            OutputText("Scenes.Urtaheatrut.WatchDrunkRuturtaJerkoff.TakeAGlanceAway".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Stopteasing".Localized(), StopTeasingDatHornyFox);

            AddButton(0, "Common.Button.Keepteasing".Localized(), KeepTeasingDatHornyFawkes);
        }

        public void StopTeasingDatHornyFox()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.StopTeasingDatHornyFox.DecideTheresNoPoint".Localized());

            OutputText("Scenes.Urtaheatrut.StopTeasingDatHornyFox.PopSheDoes".Localized());

            OutputText("Scenes.Urtaheatrut.StopTeasingDatHornyFox.SeeingHerLyingStill".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 5)));

            ReturnToCamp(1);
        }

        public void KeepTeasingDatHornyFawkes()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.SmirkThingsJustStarting".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.LewdlyCuppingAndFondling".Localized());

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.SlowlyBeginningToStroke".Localized());

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.SeductivelyTwirlAround".Localized());

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.EyesSuddenlyOpen".Localized());

            if (Player.IsGoo() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.WhatPassesForKnees".Localized());
            else
                OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.KneesWord".Localized());

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.SplutterAndChoke".Localized());

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.ThenYouAreBlind".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.RunWord".Localized());
            else
                OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.WalkTriumphantly".Localized());

            OutputText("Scenes.Urtaheatrut.KeepTeasingDatHornyFawkes.FromTheRaucousOrgy".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(15 + Player.Libido / 10)));

            ReturnToCamp(1);
        }

        public void SoberUrtaSatingPCHeat()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.PressesYourBack".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ByShovingYourCocks".Localized());

                if (Player.HasCock() && Player.Balls > 0)
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.AndBalls".Localized());

                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.OutOfTheWay".Localized());
            }
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ByShovingYourBalls".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.FeelWideFlared".Localized());

            if (Player.VaginalCapacity() >= 100)
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.HungryGashWelcomes".Localized());
            else
                if (Player.VaginalCapacity() > 60)
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.FeelItStretchYou".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.WinceInPain".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.MoanInPleasure".Localized());

            if (Player.VaginalCapacity() >= 60)
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ErectionCompletelyAt".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.MoanAgainFeeling".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.LeansCloserIntoBody".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.CockBiggest".Localized());
            else
            {
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ClitorisWord".Localized());

                if (Player.GetClitLength() < 2.5f)
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.RubbingSensitiveSpot".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.FingersOverEntire".Localized());
            }

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.LettingYouHaveMoments".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.PushesYourBackAgainst".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.CumRushesOut".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.CuntClampsDown".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.PussyJuiceShooting".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.LoveJuicesFlowing".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SqueezedByYourCunt".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ReachOutToPlay".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.CockBiggestAgain".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ClitWord".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.AlternatesSensualMoans".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.FeelsGoodGonnaCum".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.BestPussyLove".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.MoanLoudlyAndBeg".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ShootingAllOverPlace".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.RunningDownHerMember".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SentencePeriod".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.FirstTwoSurges".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.OwnMemberResponding".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.YourVagina".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SettingOffToCoat".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SqueezingTryingToMilk".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SqueezingMeDry".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.HerShotsGet".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.WhatQuestion".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.StillNotSatisfied".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.WantToKeepGoing".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SlowlyNodAndExpress".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.GroansAsSheRams".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ReachDownToPlay".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.AndCocks".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.HardeningAgain".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.GreedySlut".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SexyVixenBreedYou".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.MoanInEcstasy".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.VixenSqueezesYour".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ShootsSeedOnFloor".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SqueezesAndTugs".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SentencePeriodAgain".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.LumpsOntoYourBack".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.FeltAmazing".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.IfOnlyHadYou".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.PussyAbsolutelyAmazing".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SayEnd".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.TurnToUrtaAgain".Localized());

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SayStart".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SureThingImHere".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.ImYouNeed".Localized());

            if (!UrtaQuestDone())
            {
                if (!Quests.UrtaQuestScene.UrtaLove())
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.WeSafeNoMatter".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.SlightlySadOrAt".Localized());

                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.GrabsHandAndHelps".Localized());
            }
            else
            {
                OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.MightGetPregnant".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
                {
                    Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);

                    Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);

                    Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);
                }
            }

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            OutputText("Scenes.Urtaheatrut.SoberUrtaSatingPCHeat.TwoOfYouPart".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -2f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 6);

            ReturnToCamp(1);
        }

        public void SateRutWithSoberUrtaButHuegDicked()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.ObservesYourApproaching".Localized(
                Player.CockDescript(x)
            ));

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SkirtWord".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.DressWord".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.HugeHorseCockWhipping".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.StareUncomprehending".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.TouchIsIncredible".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.TryingToSoundNonchalant".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
            {
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.IsntIdlyBoasting".Localized());

                if (Player.CumQ() <= 50)
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SpatteringSpooge".Localized());
                else
                    if (Player.CumQ() <= 250)
                        OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SplashingFaceAndTits".Localized());
                    else
                        OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SoakingShockedFox".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.StillFeelTheUrge".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.HalfheartedlyWord".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.AtCumOnFace".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SurprisinglyVulpineShake".Localized());
            }
            else
            {
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.AddsSheTeases".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.PullsHerHeadFree".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.NoSurpriseEventually".Localized());

                if (Player.CumQ() <= 50)
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SwallowsItAll".Localized());
                else
                    if (Player.CumQ() <= 250)
                        OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.CheeksBulgeAtAmount".Localized());
                    else
                        OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.RivuletsBurstFromBetween".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrtaButHuegDicked.SuggestsRising".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Lust, 0f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 1);

            ReturnToCamp(1);
        }

        public void SateRutWithSoberUrta()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.ObservesApproachingPackage".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.SightMakesYouHalt".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.LooksAndThen".Localized(
                Player.CockDescript(x)
            ));

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.YesGiveToMe".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.FeelsGoodDesperate".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.MumblesAroundTongue".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.YouDontIntendTo".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.FinallyBreaksKiss".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.TellsYouOneHand".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.SimplyMoanBack".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.AllTheWhileHappy".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.NoSurpriseThatOf".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.FinallyYourOwn".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.MutualExplosion".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.TwoOfYouGasp".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.WhenAtLastFinished".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.AsksYouNod".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.BlowsYouAKiss".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.ScrubsOffTheWorst".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.HipsDeliberatelySway".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.WatchAsShePries".Localized());

            if (UrtaQuestDone())
            {
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.AskUrtaWhatShes".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.HaveToHopeThen".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.FeelingRelieved".Localized());

                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();

                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();

                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();
            }
            else
            {
                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.SauntersOverAndWaves".Localized());

                if (!Quests.UrtaQuestScene.UrtaLove())
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.HadBetterLearnTo".Localized(
                        Player.Mf("mister", "missy")
                    ));
                else
                    OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.ShouldLearnToControl".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.RatherDoSomethingLike".Localized());

                OutputText("Scenes.Urtaheatrut.SateRutWithSoberUrta.SmirkAndTellHer".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesRutFuckedUrtasCunt, CoC.Instance.Flags.Get(GameFlagIds.TimesRutFuckedUrtasCunt) + 1);

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Lust, 0f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 6);

            ReturnToCamp(1);
        }

        public void DrunkenUrtaFucksPCInHeat()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.SimplySmileAtHer".Localized());

            if (!UrtaQuestDone())
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.KnowThatSheCant".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.DimlyRecallUrtaCould".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.EyesGlitterWithEmotion".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.SkirtWord".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.DressWord".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.HikingUpToExpose".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.AwkwardlyFoxBacks".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.ImpatientlyNudging".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.AndYourBalls".Localized());

                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.GetToFeminineOrifice".Localized());
            }

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.CryOutInJoy".Localized());

            if (Player.GetClitLength() < 2.5f)
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.PlayingWithYour".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.LappingPleasureButton".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.ThrashAndScream".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.AsIfToEmphasize".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.SensingImpendingOrgasm".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.NowProperlyPrepared".Localized());

            if (Player.VaginalCapacity() < 60)
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.CockIsLongAnd".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.MammothCockFits".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.LikeAnAnimal".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.ShynessSeemsForgotten".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.NorEvenYour".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.DespiteUsuallyTrying".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.HowlsInTriumph".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.LayThereOnThe".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.ABitLaterHeading".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.WhenYouWake".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.MovingGingerly".Localized());

            if (UrtaQuestDone())
            {
                OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.HesitantlyAskHer".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) != 1)
                    OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.SmilesAndInforms".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.SmilesBeatifically".Localized());
            }

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.HandsYouYourClothes".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkenUrtaFucksPCInHeat.WalkBackToCamp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
            {
                Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);

                Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);

                Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);
            }

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, -4f), new DynStat(GameStats.Lust, 0f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 24);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            ReturnToCamp(4);
        }

        public void SateRutWithDrunkUrtaWithHugeDick()
        {
            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.ClimbsTopOfYou".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.AgainstYourOwnBalls".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.AgainstYourClit".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.BetweenYourHips".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.SentencePeriod".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.SometimesThisThing".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.PressesImpressiveHermhood".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.HotAndBotheredToo".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.WarmthOfHerCock".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.WhyNeedSoBig".Localized());

            if (Player.BiggestCockArea() < 150)
                OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.CompeteWithMinotaurs".Localized());
            else
                OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.SetAWorldRecord".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.SayEnd".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.TreatmentOfYourPressed".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.WhimperingAndMoaning".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.FuckGonnaCum".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.OrgasmsAlmostSynchronized".Localized());

            if (Player.CumQ() <= 50)
                OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.CumAllOverYour".Localized());
            else
                if (Player.CumQ() <= 500)
                    OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.ShaftSpasmsAnd".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.DamInsideLetsLoose".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.MilksTheLastDrops".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.GoAndCleanYourself".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.HopsOffTheTable".Localized());

            OutputText("Scenes.Urtaheatrut.SateRutWithDrunkUrtaWithHugeDick.RealizingNoPointIn".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Lust, 0f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            ReturnToCamp(1);
        }

        public void DrunkUrtaRidesARutPCsCock()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DecideNotToFight".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.LiftsBallsAndPresses".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.BallsWord".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.HipsWord".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.OwnBallsResting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.CrazyBreedingState".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FeelBallsChurning".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TinglingSensation".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TelltaleSign".Localized());

            if (Player.CumQ() <= 50)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.UnloadThickSpunk".Localized());
            else
                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.EruptInsideHer".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.BlowMassiveSurges".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DischargeTheFoxs".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.GoodWarmupNow".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.AttemptsToGoSoft".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SlightlyMoveHer".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FuckFeelsSoGood".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.InclinedToAgree".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FeelBloodFlow".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DontMindStaying".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SummoningExhaustedStrength".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.ContinuesToShakeHips".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.KnotSurgeWithBlood".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TipExpandGetting".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.MoansTopOfYou".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.BestFuckEverHad".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FeelBetterOnce".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.MasturbatesFuriously".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 500 && Player.CumQ() <= 1500)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.PositivelyFullAndStretched".Localized());
            else
                if (Player.CumQ() > 1500)
                {
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.StretchedAndCompletelyFull".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.BallsWordAgain".Localized());
                    else
                        if (Player.HasVagina() && Player.GetClitLength() >= 3.5f)
                            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.CockLikeClit".Localized());
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.PussyLips".Localized());
                            else
                                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.InsideOfYourThighs".Localized());

                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.WithYourThickSemen".Localized());
                }

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.GoodGoodFewMore".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FeelsSensitiveAndTender".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.WhateverItTakes".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FollowingHerPlea".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DontSeemToBe".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DoesntAppearToBe".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.VulpineLoverSqueals".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.CareGiveAHand".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TiredAndLustHazed".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.StimulationIsIntense".Localized());

            if (!Player.HasVagina() && Player.Balls == 0)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TheInsideOfThighs".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.YourBalls".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.YourClit".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.PressedEntirelyAgainst".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.IntenseUnadulteredLust".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.PossessivenessAndHungry".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SentencePeriod".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DoThisEveryDay".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SuddenlySeemsShy".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.IncreasedLustAndStrong".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.ComeFuckMeBack".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.ShowMeYoureHere".Localized(
                    Player.Mf("the man", "the breeder")
                ));
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DontWantToMake".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.AfterWhileBecomes".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.AfterALongTime".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.IfTwoOfYou".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TryToAnnounce".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.BallsWordThird".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.VaginaAndClit".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TaintWord".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FinallyRunningTongue".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.AlreadyAgreedThat".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.RestLaterIll".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SayEnd".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.OnceAgainSheMoves".Localized());

            if (Player.CumQ() <= 200)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TableSeemsLikeIt".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TwoAreAlready".Localized());
                else
                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.MixedSpermCovering".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SlidesSemiErect".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.MaybeYouCanBut".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.KeepGoingWord".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.IdeaOfKnockingUp".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.LickItsOnlyFair".Localized());

            if (Player.Tallness <= 53)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DontEvenHaveTo".Localized());
            else
                if (Player.Tallness <= 65)
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.ManeuveringYourNeck".Localized());
                else
                    if (Player.Tallness <= 78)
                        OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.EasilyBendDown".Localized());
                    else
                        if (Player.Tallness <= 93)
                            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.HasToBendBackwards".Localized());
                        else
                            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.NoMatterHowHard".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.BouncesWildly".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.GrabsYourFace".Localized());

            if (Player.Tallness <= 93)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.LickKissAndSuck".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.StaringDeepIntoEyes".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SentencePeriodAgain".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.GrinsAtYourLack".Localized());
            else
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SeedJustForMe".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SurroundingFiguresGroan".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.YesFuckMarae".Localized());

            if (Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FewMoreLoadsOkay".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SayEndAgain".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.PantAndMoan".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.MarePenisInFront".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FranticallyGathering".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SlowlyLoseThe".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SexCrazedFoxKeeps".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.MindIncapableOfTracking".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.ItAppearsWeCant".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.RespondThatYoure".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.IHopedForMore".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.WincesInPain".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.DidntKnowSoAggressive".Localized());

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.StopUrtaSaying".Localized());

            if (Player.Corruption > 35 || SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.TellWasActually".Localized());

            if (!UrtaQuestDone())
            {
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.GuessItWasNice".Localized());

                if (Quests.UrtaQuestScene.UrtaLove())
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.IfOnlyICould".Localized());

                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SayEndThird".Localized());
            }
            else
            {
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.RealizeUrtaWentAhead".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) != 1)
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.UrtaAdmits".Localized());
                else
                    OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.ILoveYou".Localized());

                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.NodTellHerYou".Localized());

                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();

                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();

                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();
            }

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SheWord".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.WinksAsYouGet".Localized());
            else
            {
                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.QuickButLoving".Localized());

                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.FindMeWhen".Localized());

                OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.PunchesYourShoulder".Localized(
                    Player.Mf("chuckle", "giggle")
                ));
            }

            OutputText("Scenes.Urtaheatrut.DrunkUrtaRidesARutPCsCock.SatisfiedEvenIfSore".Localized(
                Player.CockDescript(x)
            ));

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, -10f), new DynStat(GameStats.Lust, 0f));

            Player.Orgasm("Dick");

            Player.Orgasm("Dick");

            Player.Orgasm("Dick");

            Player.Orgasm("Dick");

            Player.Orgasm("Dick");

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesRutFuckedUrtasCunt, CoC.Instance.Flags.Get(GameFlagIds.TimesRutFuckedUrtasCunt) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 24);

            ReturnToCamp(4);
        }

    }
}