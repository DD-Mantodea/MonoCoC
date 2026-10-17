using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<KatherineEmploymentScene>("coc:katherine_employment")]
    public class KatherineEmploymentScene : Scene
    {

        public const int KBIT_TRAINING_TALK_EDRYN = 4;

        public const int KBIT_TRAINING_TALK_URTA = 8;

        public const int KBIT_TRAINING_TALK_KATH = 16;

        public const int KBIT_TRAINING_URTA_HELP = 32;

        public EdrynScene edryn => SceneInstance<EdrynScene>.Get();

        public KatherineScene katherine => SceneInstance<KatherineScene>.Get();

        public KatherineThreesomeScene threesome => SceneInstance<KatherineThreesomeScene>.Get();

        public UrtaScene urta => SceneInstance<UrtaScene>.Get();

        public override void Entry()
        {
            EmploymentTalk();
        }

        public void EmploymentTalk()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.EmploymentTalk.KatherineThoughtAboutJob".Localized());

            OutputText("Scenes.Katherineemployment.EmploymentTalk.ShrugsNeverWorkedOut".Localized());

            OutputText("Scenes.Katherineemployment.EmploymentTalk.AskLikesLookAround".Localized());

            OutputText("Scenes.Katherineemployment.EmploymentTalk.KathThinksItOver".Localized());

            OutputText("Scenes.Katherineemployment.EmploymentTalk.TellGoTakeLook".Localized());

            TryShops();
        }

        public void TryShops()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.TryShops.LookAroundForWork".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.OswaldSighsLooksRight".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.TryElsewhereGoodLuck".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.WeighingOptions".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.AskCounterCentauress".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.PauseThinkSheWont".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.BlacksmithLookAround".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.SpotYvonneHammeringBreastplate".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.KnowSomebodyWhoKnows".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.SighingThankYvonneConsider".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.BuxomCentauressReceptionistGym".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.ProbablyNotQualified".Localized());

            if (Player.Tone >= 75)
            {
                OutputText("Scenes.Katherineemployment.TryShops.SecondLookWellDefined".Localized());

                OutputText("Scenes.Katherineemployment.TryShops.AppreciateWillKeepMind".Localized());
            }

            OutputText("Scenes.Katherineemployment.TryShops.ThankHerLookElsewhere".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.WhatAboutYara".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.GreetYaraFriendly".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.FriendLookingForJob".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.YaraNeedsNoOne".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.KnowNeverBeenPierced".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.VolunteerToTrain".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.CantSpareTime".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.CantHelpYaraSadly".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.TailorPatternClear".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.AskCorgiGirlHire".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.SorryNotInterested".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.ThankHerForTime".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.OnlyRemainingOptionWeaponsmith".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.SnarlsSpitItOut".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.CrankyOldCoot".Localized());

            OutputText("Scenes.Katherineemployment.TryShops.NobodyWillingTake".Localized());

            TryTheWatch();
        }

        public void TryTheWatch()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.TryTheWatch.DisappointmentTelAdre".Localized());

            var pissedOffUrta = (!GameSettings.UrtaDisabled && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == -1);

            var pissedOffEdryn = (CoC.Instance.Flags.Get(GameFlagIds.EdrynNeverSeeAgain) == 1);

            if (pissedOffUrta && pissedOffEdryn)
            {
                OutputText("Scenes.Katherineemployment.TryTheWatch.NotBestTermsUrta".Localized());

                OutputText("Scenes.Katherineemployment.TryTheWatch.ExplainWatchNeeds".Localized());

                OutputText("Scenes.Katherineemployment.TryTheWatch.LeaveWatchStationHopeful".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 3);
            }
            else
            {
                OutputText("Scenes.Katherineemployment.TryTheWatch.TryTalkingMightHelp".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 2);
            }

            ReturnToCamp(1);
        }

        public bool CanTalkToEdryn()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) > 1 && (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) & KBIT_TRAINING_TALK_EDRYN) == 0;
        }

        public void TalkToEdryn()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.TalkToEdryn.ApproachThe".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Katherineemployment.TalkToEdryn.Gorging".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.MakeYourselfComfortable".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Katherineemployment.TalkToEdryn.StifleBelchFeasting".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.HelloWhatsOnMind".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.PositionsAvailable".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.RoomForNewRecruits".Localized());

            int edrynKidsPlus = CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) + (Player.IsPregnant() ? 1 : 0);

            if (edrynKidsPlus > 0)
            {
                OutputText("Scenes.Katherineemployment.TalkToEdryn.WidensEyesInSurprise".Localized());

                if (Player.IsPregnant())
                    OutputText("Scenes.Katherineemployment.TalkToEdryn.BrushingSwollenBelly".Localized());

                OutputText("Scenes.Katherineemployment.TalkToEdryn.DontTellMeAbout".Localized());
            }

            OutputText("Scenes.Katherineemployment.TalkToEdryn.NotSigningUpYourself".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.SaysRubbingChinThoughtfully".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.AdmittingNotCertain".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.ShrugsWavingHandFlippantly".Localized());

            OutputText("Scenes.Katherineemployment.TalkToEdryn.ThankCentauressHelp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) | KBIT_TRAINING_TALK_EDRYN);

            var cockFitIndex = Player.CockThatFits(300);

            if ((cockFitIndex >= 0 && Player.CockArea(cockFitIndex) >= 24) && (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Hoofed || Player.CountCocksOfType("Horse") > 0 || Player.IsCorruptEnough(50) || Player.StatusEffectValue(StatusEffectIds.Edryn, 0) > 0))
            {
                OutputText("Scenes.Katherineemployment.TalkToEdryn.CoyLookAnythingElse".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), (Player.IsPregnant() ? PregnantEdrynSexSelector : edryn.EdrynSexSelecter));

                AddButton(1, "Common.Button.No".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }
            else
            {
                OutputText("Scenes.Katherineemployment.TalkToEdryn.LeaveBarHowTo".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
            }
        }

        public void PregnantEdrynSexSelector()
        {

            ClearButtons();

            AddButton(0, "Common.Button.PregFuck".Localized(), SceneInstance<EdrynScene>.Get().FuckPregEdryn);

            AddButton(1, "Common.Button.EatRutFuck".Localized(), SceneInstance<EdrynScene>.Get().EatEdrynPussyLikeABawss);
        }

        public bool CanTalkToUrta()
        {

            return !SceneInstance<UrtaScene>.Get().UrtaDrunk() && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) > 1 && (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) & KBIT_TRAINING_TALK_URTA) == 0;
        }

        public void TalkToUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.TalkToUrta.NeedTalkBusiness".Localized());

            OutputText("Scenes.Katherineemployment.TalkToUrta.MetCatHomeless".Localized());

            OutputText("Scenes.Katherineemployment.TalkToUrta.HermTapsFinger".Localized());

            OutputText("Scenes.Katherineemployment.TalkToUrta.AskNodsFortifyingGulp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) | KBIT_TRAINING_TALK_URTA);

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
            {
                OutputText("Scenes.Katherineemployment.TalkToUrta.LongMomentsSighs".Localized());

                OutputText("Scenes.Katherineemployment.TalkToUrta.LeastCanDoTell".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.SuckOff".Localized(), SceneInstance<UrtaScene>.Get().BlowUrtaUnderTheTableLuv);

                AddButton(1, "Common.Button.EatOut".Localized(), SceneInstance<UrtaScene>.Get().EatUrtaOutNomNomPussy);
            }
            else
            {
                OutputText("Scenes.Katherineemployment.TalkToUrta.GulpingMugWaving".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
            }
        }

        public void TalkToKath()
        {

            OutputText("Scenes.Katherineemployment.TalkToKath.TalkAboutEfforts".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.WhatEarPiercing".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.NoNeedAggressive".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.MeanNoChoice".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.AskSkillsEmployable".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.WiltsEarsTailLimp".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.KeepLivingStreets".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.DoneAlrightSoFar".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.GangDoingWell".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.EarsTailDroop".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.TheyVouchForAll".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.MusterEnoughSpirit".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.WhyNotJoinWatch".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.TriesDefendActionsColleagues".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.WatchAllThatBad".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.SnifflingWipingEyes".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.PatSnifflingCatGirl".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.StillNotSoundingSure".Localized());

            OutputText("Scenes.Katherineemployment.TalkToKath.ToldBasicKnowledge".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
            {
                OutputText("Scenes.Katherineemployment.TalkToKath.KnowSomeoneWhipShape".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.HowKnowGoodEnough".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.MaybeCaptainJaw".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.TellHaveYourWays".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.LittleOverwhelmed".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.PromiseDoWhatTold".Localized());
            }
            else
            {
                OutputText("Scenes.Katherineemployment.TalkToKath.HelplesslySurviveOwn".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.GiveLookMildHurt".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.HopingMoreExcitement".Localized());

                OutputText("Scenes.Katherineemployment.TalkToKath.DoeEyedLookSerious".Localized());
            }

            OutputText("Scenes.Katherineemployment.TalkToKath.SayGoodbyeReturnCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) | KBIT_TRAINING_TALK_KATH);

            ReturnToCamp(1);
        }

        public bool InitiateTraining()
        {
            ClearOutput();

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) & KBIT_TRAINING_TALK_KATH) == 0)
                return false;

            var urtaAvailable = SceneInstance<UrtaScene>.Get().UrtaAtBar() && !SceneInstance<UrtaScene>.Get().UrtaDrunk();

            if (urtaAvailable && (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) & KBIT_TRAINING_URTA_HELP) > 0)
            {
                InitiateTrainingWithUrta();

                return true;
            }

            OutputText("Scenes.Katherineemployment.InitiateTraining.AlleywayMentalInventory".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Postpone".Localized(), Postpone);

            AddButton(1, "Common.Button.Train".Localized(), TrainKath);

            if (!GameSettings.UrtaDisabled)
            {
                AddButton(2, "Common.Button.WithUrta".Localized(), TrainKathWithUrta);

                if (!SceneInstance<UrtaScene>.Get().UrtaLove() || (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) & KBIT_TRAINING_TALK_URTA) == 0)
                    AddButton(2, "Common.Button.WithUrta".Localized(), null);
                else
                    if (!urtaAvailable)
                    {
                        OutputText("Scenes.Katherineemployment.InitiateTraining.WaitForUrtaAvailable".Localized());

                        AddButton(2, "Common.Button.WithUrta".Localized(), null);
                    }
            }

            return true;
        }

        public void Postpone()
        {

            OutputText("Scenes.Katherineemployment.Postpone.LivingStreetsPromised".Localized());

            SceneInstance<KatherineScene>.Get().KatherineVisitNormal(false);
        }

        public void TrainKath()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.TrainKath.ThoughtLongAndHard".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.PatrolDesertTelAdre".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.MartialSkillsNeverFought".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.LookOutForHerself".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.PutDownSack".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.SuppliesUntilFinishes".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.StreetsLivedHereWhole".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Naga, 1) != 0)
                OutputText("Scenes.Katherineemployment.TrainKath.LaughDangersPicksNaga".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.BestWayLearnSurvival".Localized());

            OutputText("Scenes.Katherineemployment.TrainKath.AdjustsSackFollows".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 2);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 0);

            KatherineTrainingStage1();
        }

        public void KatherineTrainingStage1(bool clearOut = true)
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1.LongWindingTrek".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1.TricksStayingAlive".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1.ComplainsSandHeat".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1.DraggingFeetStop".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1.DrinkCanteen".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                KatherineTrainingStage1Success();
                }

                break;

                case 1:
                {
                KatherineTrainingStage1Failure();
                }

                break;

                case 2:
                {
                KatherineTrainingStage1Horny();
                }

                break;
            }

            if (true )
                KatherineTrainingStage1Success();
        }

        public void KatherineTrainingStage1Success()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.TipsOutContents".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.TiredBitsOfWood".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.LashingDownCanvas".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.ShieldedFromSun".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.DistractedDecline".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.MoreInstructionTent".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Success.SqueezesHandThanks".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 10);

            ReturnToCamp(1);
        }

        public void KatherineTrainingStage1Failure()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Failure.TipsOutContents".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Failure.BadStartAssemble".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Failure.WindBlowsCanvas".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Failure.ScrewedUpTime".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Failure.GoOverMistakes".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Failure.CarryHerFlopsDown".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 5);

            ReturnToCamp(1);
        }

        public void KatherineTrainingStage1Horny()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Horny.LiesDownPulls".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Horny.SoManyWetSounds".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Horny.NoOneWithinMile".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage1Horny.GetKathUseful".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_DESERT);

            ClearButtons();

            AddButton(0, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), SceneInstance<KatherineScene>.Get().GiveKatOralPenisWingWang);

            AddButton(1, "Common.Button.Handjob".Localized(), SceneInstance<KatherineScene>.Get().HandjobbiesFurrDemCatFurries);
        }

        public void KatherineTrainingStage2()
        {

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2.BasicSurvivalEnough".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2.WindedWalkLooksAround".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                KatherineTrainingStage2Success();
                }

                break;

                case 1:
                {
                KatherineTrainingStage2Failure();
                }

                break;

                case 2:
                {
                KatherineTrainingStage2Horny();
                }

                break;
            }

            if (true )
                KatherineTrainingStage2Success();
        }

        public void KatherineTrainingStage2Success()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Success.PulledSwordsClubs".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Success.BouncesFootToFoot".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Success.TireTellDidWell".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Success.ComplimentArmInArm".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 10);

            ReturnToCamp(2);
        }

        public void KatherineTrainingStage2Failure()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Failure.PulledSwordsClubs".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Failure.SwordQuickThrusts".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Failure.AgainWrongBlock".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Failure.TrainingSwordsBetter".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Failure.CantContinuePackUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 5);

            ReturnToCamp(2);
        }

        public void KatherineTrainingStage2Horny()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Horny.SoonerEnergyToSpare".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Horny.BushesSmellAllOver".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage2Horny.SmellKatherineLikewiseSniffing".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_LAKE);

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), null);

            AddButton(1, "Common.Button.GiveAnal".Localized(), null);

            AddButton(2, "Common.Button.GiveBoth".Localized(), null);

            AddButton(5, "Common.Button.MountHer".Localized(), null);

            AddButton(6, "Common.Button.TakeAnal".Localized(), null);

            AddButton(7, "Common.Button.TakeBoth".Localized(), null);

            AddButton(4, "Common.Button.Bath".Localized(), SceneInstance<KatherineScene>.Get().BathTime);

            if (Player.CockThatFits(70) > -1)
            {
                AddButton(0, "Common.Button.FuckHer".Localized(), SceneInstance<KatherineScene>.Get().PenetrateKatsVag);

                AddButton(1, "Common.Button.GiveAnal".Localized(), SceneInstance<KatherineScene>.Get().PcPenetratesKatAnally);

                if (Player.CockThatFits2(70) > -1)
                    AddButton(2, "Common.Button.GiveBoth".Localized(), SceneInstance<KatherineScene>.Get().PcPenetratesKatDoubly);
            }

            if (Player.HasVagina() && Player.VaginalCapacity() >= 100)
            {
                AddButton(5, "Common.Button.MountHer".Localized(), SceneInstance<KatherineScene>.Get().LetKatKnotYourCuntPussyFuck);

                if (katherine.knotSize < 4 && katherine.cockNumber > 1)
                    AddButton(7, "Common.Button.TakeBoth".Localized(), SceneInstance<KatherineScene>.Get().GetDoublePennedByKat);
            }

            if (katherine.knotSize < 4)
                AddButton(6, "Common.Button.TakeAnal".Localized(), SceneInstance<KatherineScene>.Get().GetPenetrated);
        }

        public void KatherineTrainingStage3()
        {

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3.HandleJobPhysically".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3.StickSwordIntoBelt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 99)
                KatherineTrainingStage3Success();
            else
            {
                switch (Random.Shared.Next(3))
                {
                    case 0:
                    {
                    KatherineTrainingStage3Success();
                    }

                    break;

                    case 1:
                    {
                    KatherineTrainingStage3Failure();
                    }

                    break;

                    case 2:
                    {
                    KatherineTrainingStage3Horny();
                    }

                    break;
                }

                if (true )
                    KatherineTrainingStage3Success();
            }
        }

        public void KatherineTrainingStage3Success()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Success.MiscreantsPickpocket".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Success.HandOnGripClub".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Success.WayOutNoAvail".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Success.CombatTellDidWell".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Success.NumberOtherScenarios".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Success.LeavePracticeByHerself".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 10);

            ReturnToCamp(1);
        }

        public void KatherineTrainingStage3Failure()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Failure.MiscreantsPickpocket".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Failure.HandOnGripClub".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Failure.ThrowOffKilter".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Failure.InsultingPolicePussy".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Failure.AgainPointOut".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Failure.CityGatesLittleProgress".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 5);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) > 99)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 99);

            ReturnToCamp(1);
        }

        public void KatherineTrainingStage3Horny()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Horny.ExplainCrimeRealize".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Horny.NeedsControlHerself".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Horny.DownTreacherousCock".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Horny.SnickerBothSitting".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingStage3Horny.FaceAgainstCheek".Localized());

            AlleywaySexOptions();
        }

        public void AlleywaySexOptions()
        {

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_STREETS);

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), null);

            AddButton(1, "Common.Button.GiveAnal".Localized(), null);

            AddButton(2, "Common.Button.GiveBoth".Localized(), null);

            AddButton(3, "Common.Button.Sucknfuck".Localized(), null);

            AddButton(4, "Common.Button.Bath".Localized(), null);

            AddButton(5, "Common.Button.MountHer".Localized(), null);

            AddButton(6, "Common.Button.TakeAnal".Localized(), null);

            AddButton(7, "Common.Button.TakeBoth".Localized(), null);

            AddButton(8, "Common.Button.Sucknfuckd".Localized(), null);

            if (Player.CockThatFits(70) > -1)
            {
                AddButton(0, "Common.Button.FuckHer".Localized(), SceneInstance<KatherineScene>.Get().PenetrateKatsVag);

                AddButton(1, "Common.Button.GiveAnal".Localized(), SceneInstance<KatherineScene>.Get().PcPenetratesKatAnally);

                if (Player.CockThatFits2(70) > -1)
                    AddButton(2, "Common.Button.GiveBoth".Localized(), SceneInstance<KatherineScene>.Get().PcPenetratesKatDoubly);

                if (katherine.knotSize <= 4)
                    AddButton(3, "Common.Button.Sucknfuck".Localized(), SceneInstance<KatherineScene>.Get().SuckNFuck);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 100)
                AddButton(4, "Common.Button.Bath".Localized(), SceneInstance<KatherineScene>.Get().DateKathBath);

            if (Player.HasVagina())
            {
                AddButton(5, "Common.Button.MountHer".Localized(), SceneInstance<KatherineScene>.Get().LetKatKnotYourCuntPussyFuck);

                if (katherine.knotSize < 4 && katherine.cockNumber > 1)
                    AddButton(7, "Common.Button.TakeBoth".Localized(), SceneInstance<KatherineScene>.Get().GetDoublePennedByKat);
            }

            if (katherine.knotSize < 4)
                AddButton(6, "Common.Button.TakeAnal".Localized(), SceneInstance<KatherineScene>.Get().GetPenetrated);

            if (Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.KatherineTimesSexed) > 0)
                AddButton(8, "Common.Button.Sucknfuckd".Localized(), SceneInstance<KatherineScene>.Get().SuckedNFuckedByKat);
        }

        public void KatherineTrainingComplete()
        {

            if (SceneInstance<UrtaScene>.Get().UrtaDrunk() || View.Time.Hours >= 15)
            {
                if (View.Time.Hours >= 15)
                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.TestingBestCatchUrta".Localized());
                else
                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.TestingDontThinkCaptain".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.AnyWayMorePleased".Localized());

                AlleywaySexOptions();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 4);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_KATHS_APT);

            OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.ReadyTestingBeams".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > -1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) > 0)
                {
                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.BigSmileLeadTable".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.TrainingHopesJob".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.YesSleepingTogether".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, 1);

                    if (SceneInstance<UrtaScene>.Get().UrtaLove() && (CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 4 || CoC.Instance.Flags.Get(GameFlagIds.UrtaKnowsPcHasMarbleFollower) > 0))
                    {
                        OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.DontWorryNotAngry".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaOpenAboutEdryn) > 0)
                            OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.DoesntGetBackUp".Localized());

                        OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.FineArrangement".Localized());

                        OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.TryingProcessHearing".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 4)
                        {
                            OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.MetReliefHerName".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.KatherineAmilyAffection, 1);
                        }
                    }

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.ShellShockedDontFrighten".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.LastDrinkStandsUp".Localized(
                        Player.ShortName
                    ));

                    KatherineTrainingCompleteUrtaLover();
                }
                else
                {
                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.SpokenSinceFirstDay".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.WavingTowardsSeats".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.ExplainWantsJoin".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.HeardTestsPassQualify".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.TestsTrainingSomeTime".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.TossesBackRestDrink".Localized());

                    KatherineTrainingCompleteNeverSpokenOrPermanentlyPissed();
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) == 0)
                {
                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.WaversCuriosityFear".Localized());

                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.MakesKathNervous".Localized());

                    KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer();
                }
                else
                {
                    OutputText("Scenes.Katherineemployment.KatherineTrainingComplete.DoesntWantToTalk".Localized());

                    KatherineTrainingCompleteNeverSpokenOrPermanentlyPissed();
                }
        }

        public void KatherineTrainingCompleteThereIsNoUrta()
        {

            if (View.Time.Hours >= 15)
            {
                OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.TestingBestCaptain".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.AnyWayMorePleased".Localized());

                AlleywaySexOptions();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 4);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_KATHS_APT);

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.ReadyTestingBeams".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.SpokenSinceFirstDay".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.WavingTowardsSeats".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.ExplainWantsJoin".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.RanisHeardTests".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.TestsTrainingSomeTime".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteThereIsNoUrta.TossesBackRest".Localized());

            KatherineTrainingCompleteRanis();
        }

        public void KatherineTrainingCompleteRanis()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteRanis.PaceBackAndForth".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteRanis.HairCalmsNewBadge".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteRanis.AlleyHadFunTimes".Localized());

            ReturnToCamp(4);
        }

        public void KatherineTrainingCompleteUrtaLover()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.FewHoursWaiting".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.RacesIntoArms".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.HairCalmsNewBadge".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.DoorHousingApproval".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.SoMuchToDo".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.SaysAgainKisses".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaLover.HandThroughUrtaThanks".Localized());

            ReturnToCamp(4);
        }

        public void KatherineTrainingCompleteNeverSpokenOrPermanentlyPissed()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteNeverSpokenOrPermanentlyPissed.PaceBackAndForth".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteNeverSpokenOrPermanentlyPissed.HairCalmsNewBadge".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteNeverSpokenOrPermanentlyPissed.AlleyHadFunTimes".Localized());

            ReturnToCamp(4);
        }

        public void KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer.FewHoursWaiting".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer.RacesIntoArms".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer.HairCalmsNewBadge".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer.DoorHousingApproval".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer.SoMuchToDo".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingCompleteUrtaThoughtYouDidntLikeHer.SaysAgainKisses".Localized());

            UrtaForgivesYou();
        }

        public void UrtaForgivesYou()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.UrtaForgivesYou.ComplimentingKath".Localized());

            OutputText("Scenes.Katherineemployment.UrtaForgivesYou.ActingMoreToThis".Localized());

            OutputText("Scenes.Katherineemployment.UrtaForgivesYou.AddsMisjudgedYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Flirt".Localized(), SceneInstance<UrtaScene>.Get().FlirtWithUrta);

            AddButton(1, "Common.Button.Friends".Localized(), FriendsWithUrta);

            AddButton(2, "Common.Button.DestroyHer".Localized(), DestroyUrta);
        }

        public void FriendsWithUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.FriendsWithUrta.NoProblemWithHerms".Localized());

            OutputText("Scenes.Katherineemployment.FriendsWithUrta.AwkwardSilenceThen".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            ReturnToCamp(4);
        }

        public void DestroyUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.DestroyUrta.NowWantsMakeUp".Localized());

            OutputText("Scenes.Katherineemployment.DestroyUrta.FlyIntoRageTears".Localized());

            ReturnToCamp(4);
        }

        public void TrainKathWithUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.WayToWetBitch".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.OpenlyWarmlySmiles".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.HereMoreInformation".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.SteeplesClawedFingers".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.SuggestPrivateSetting".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.SurprisedSmilesReallyCare".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.RouteQuiteFamiliar".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.ShareAllYouKnow".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.NodsKeepsQuietDigesting".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.FocusOnWhatMatters".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.BlushesFiddlingHem".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.HopingPlanGuardCaptain".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.ScratchesEarThreeReqs".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.ConsideringStateAffairs".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.PondersTrainingEnough".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.LivingRoomChastityBelt".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.BlushesFiercelyFinallyManages".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.ShockedHorrified".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.LibidoWorseKittyCat".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.FinalNoteAskPlace".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.DeepThoughtSafehouses".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.IfCouldPerfect".Localized());

            OutputText("Scenes.Katherineemployment.TrainKathWithUrta.AnythingElseOnMind".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) | KBIT_TRAINING_URTA_HELP);

            AddButton(0, "Common.Button.Yes".Localized(), ChastityBeltFun);

            AddButton(1, "Common.Button.No".Localized(), NoChastityFunForNow);
        }

        public void ChastityBeltFun()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.ChastityBeltFun.LiftBeltLookOver".Localized());

            OutputText("Scenes.Katherineemployment.ChastityBeltFun.SmileWidensLookBelt".Localized());

            OutputText("Scenes.Katherineemployment.ChastityBeltFun.DisappointmentLookBack".Localized());

            OutputText("Scenes.Katherineemployment.ChastityBeltFun.GrinHandsBeltOver".Localized());

            SceneInstance<UrtaScene>.Get().ChastityBeltFun(false);
        }

        public void NoChastityFunForNow()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.NoChastityFunForNow.SomeOtherTime".Localized());

            OutputText("Scenes.Katherineemployment.NoChastityFunForNow.BusinessBeforePleasure".Localized());

            ReturnToCamp(1);
        }

        public void InitiateTrainingWithUrta()
        {

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.AlleywayBehindPawnshop".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.StrainedSmileNeed".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.TellPackUp".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.UrtaStandingAtDoor".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.VixenSmilesWalksOver".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.EaseTensionQuickKiss".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.MotivationLikeThreaten".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.DidntMeanLikeThat".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.FlushesEmbarrassment".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.CouldUseAllHelp".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.TellPrettyRough".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.OkayNoNeedLectures".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.NoddingYouDoSo".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SpotKathSitting".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.HermsAwkwardlySayHello".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SocialIneptitude".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.KnowWhoSheIs".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ListenHerePussyCat".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.JeersAsIfKnow".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.BloodSweatTears".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.GuardedExpression".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.InhalesDeeplyExhales".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ShakingHandDeal".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SighReliefFight".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.TurnsExpensiveDumbstruck".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.OfficialPurposeTraining".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.DrawsHerselfUpStraight".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ChipperEntails".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.WhatDoesItEntail".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.WaitingAtSafehouse".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.PackageWhatPackage".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.IndicateShouldLeadWay".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.RunDownBuilding".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.WoundedOfCourseSafe".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.LooksLikeDump".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SurprisedApartment".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.HurtPrideCouncil".Localized());

            if (silly)
            {
                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.GhostbustingGear".Localized());

                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.NoSuchThingGhosts".Localized());
                // 原版 KatherineEmployment.hx:739 — ShouldraFollower.hx：Shouldra 是跟随者
                if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                {
                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.KnowOneCarryingOne".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.FeelingVeryPut".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.VoiceOutOfChest".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.JawsDropUnison".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ShouldraLungesJackpot".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ReturnPantingTents".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.TongueLollingGhoulie".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.NotFunnyTailJutting".Localized());

                    OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.UnfortunatelyOutOfControl".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetShouldra) > 0)
                    {
                        OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.KnowOneActually".Localized());

                        OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SpendingTooMuchSun".Localized());

                        OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.DontBelieveTakeThem".Localized());

                        OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.MutualSentiment".Localized());
                    }
                    else
                        OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.JustSnicker".Localized());
            }

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.OpenGateGetStarted".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SalveWoundedDignity".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.InteriorBetterCondition".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ParkGearCorner".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SarcasticallyLearnSkills".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ClearlyNotSure".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.BadAtPepTalks".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ThunderStolen".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.GrinEvillyAskPackage".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.PassesKeyIndicatingLock".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.WatchesNervouslySeparate".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.RelaxAllOverSoon".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.RetortsEyesFlickingNervously".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.HoldingChastityBelt".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.SeveralLongMoments".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.HeardPutItOn".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.FlattensEarsHisses".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.NotKinkySexPart".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.AngryBarnCatClaw".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.KittenFireAfterAll".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.FeralYowlBolts".Localized());

            if (silly)
            {
                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.TryExtendHandChanting".Localized());

                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.AnticipationFightLooks".Localized());

                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.MethodUsedStrayCats".Localized());

                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.DoesReallyWork".Localized());

                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ConcludeUsuallyDoesnt".Localized());

                OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.DisappointmentHisses".Localized());
            }

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.LastChanceOverHere".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ClawsExtendedComeOn".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.HearMutterGladly".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.NeverHeardWord".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.AskKeepBeltOn".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.ThoughtOriginalPlan".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.GoThroughEveryTime".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.PleadsDontMakeMe".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.WhimpersPaysNoAttention".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.MewlsDismayedAgreement".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.GotThingsHereLover".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.WishWellAsk".Localized());

            OutputText("Scenes.Katherineemployment.InitiateTrainingWithUrta.HopingComeToTerms".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 3);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 0);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrta()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrta.RetraceStepsSafehouse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 100)
                KatherineTrainingWithUrtaComplete();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 66)
                    KatherineTrainingWithUrtaStage3();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 33)
                        KatherineTrainingWithUrtaStage2();
                    else
                        KatherineTrainingWithUrtaStage1();
        }

        public void KatherineTrainingWithUrtaStage1()
        {

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.HearVoiceOtherSide".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.SarcasticallyRetorts".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.HeatedArgumentSigh".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.TakingSeriouslySkills".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.TappingClawSinceWhen".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.CantSeriouslyBeAroused".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.InterjectYelling".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.HalfHeartedlyMumble".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.OfficerExplainsRecruit".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.AskDidAsWell".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.ShelterFindingPart".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.AgreementNoRain".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.ExactlyRightSandstorms".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.AskWhatProblem".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.GrimacesDespiteBelt".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.HaventNoticedHerm".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.NicknameNotInsult".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.ExplainTent".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.TeasingPleaseAdvice".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.PieceComesTogether".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1.IntentlyFollowing".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                KatherineTrainingWithUrtaStage1Success();
                }

                break;

                case 1:
                {
                KatherineTrainingWithUrtaStage1Failure();
                }

                break;

                case 2:
                {
                KatherineTrainingWithUrtaStage1Horny();
                }

                break;
            }

            if (true )
                KatherineTrainingWithUrtaStage1Success();
        }

        public void KatherineTrainingWithUrtaStage1Success()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.PrettyWellMadeTent".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.EyesWidenedCheers".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.ClappingCongratulate".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.SighsPromptlyStartsDismantling".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.AllUrtaWillSay".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.ProtestsThatsNotFair".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.FiveTimesTakeOff".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.PrickyVixxy".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.WorkUpHandjob".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.ScaredWouldWin".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.UpAnteTenTimes".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.ShakingHeadDissuade".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.ExcitedlyPracticallyThrows".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Success.WishLuckTwistKnob".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 16);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage1Failure()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.WorkIsntVeryGood".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.KeepTryingGetRight".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.MoveOnNextPhase".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.AlsoMadeManyMistakes".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.HopefullyReallyThinkSo".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.SaySmilingOfCourse".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.ThrowsArmsSqueezingHug".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.CaressingBackHead".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.StuffBackToWork".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Failure.NothingElseBidFarewell".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 8);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage1Horny()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.EventuallyCastsAside".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.DoNotFlashGoods".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.BreakfastWashingUp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.SeemsLikeSomething".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.DontDrinkMuch".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.StillOughtaDoSomething".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.PromptlyClaspsHandsStares".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.WhatHaveInMind".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.HelpResistanceLust".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.SproutedAnotherHead".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.WoundedDignityExpertHands".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.NobodyElsePractice".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.NotSomethingSay".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.HorrorYouWouldnt".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.InconsiderateDamnWell".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.DecencyGenuinelyAshamed".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.JustUrtaFine".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.MuchBetterNow".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.LetMeOutBegs".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.ProtestsIgnoreFiiineConcedes".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.StormsOffPushing".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.EasierGiveBreak".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.HadItRough".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.GiveOtherwiseWontWork".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.DoForYouWorth".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.GivesHandProblem".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.LeadWayPrickBobbing".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.WhatTheHeckNods".Localized(
                katherine.CockMultiple("", "twin "),

                katherine.CockMultiple("", "upper ")
            ));

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1Horny.LiveUpPromise".Localized());

            ClearButtons();

            if (!Player.IsGenderless())
                AddButton(0, "Common.Button.HelpOut".Localized(), KatherineTrainingWithUrtaStage1HornyHelp);
            else
                AddButton(0, "Common.Button.HelpOut".Localized(), null);

            AddButton(1, "Common.Button.Leave".Localized(), KatherineTrainingWithUrtaStage1HornyLeave);
        }

        public void KatherineTrainingWithUrtaStage1HornyLeave()
        {

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1HornyLeave.DontHaveParts".Localized());
            else
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1HornyLeave.OtherThingsBetterExcuse".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage1HornyLeave.BothMoaningFingers".Localized());

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(false);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage1HornyHelp()
        {

            SceneInstance<KatherineThreesomeScene>.Get().Circlejerk();

            CirclejerkTrainingEnding();
        }

        public void CirclejerkTrainingEnding()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.LyingOnBackMatted".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.RiseLookAroundArmor".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.NotTriosomeMoreOften".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.AgreesNoddingSameSmile".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.NotFriendsNoWay".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.SnarlingWonderingNextTime".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.BothAtSameTime".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.GettingHardIdea".Localized());

            OutputText("Scenes.Katherineemployment.CirclejerkTrainingEnding.BetterBeGoing".Localized());

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage2()
        {

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.CombatEchoesSafehouse".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.LookingRightThrowsPunch".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.WatchPracticeJokinglyPopcorn".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.CatFightActuallyFighting".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.InterceptBlowForearm".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.HitsFloorDoesntComplain".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.DefenseNeedsWork".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.InterruptGivePointers".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.SureCouldntHurt".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.DeadpansEagerlyApproaches".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.FewPointersNoExpert".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.HearPointersExpert".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.ArmAroundWaist".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.FoxyComingForYou".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.YawnsAboutTime".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.PullCloseLightKiss".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.BreaksAwayBlushing".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.AdoptFightingStance".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.DontBeTooConfident".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.FineByMeHate".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2.SeatOnChairInteresting".Localized());

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                KatherineTrainingWithUrtaStage2Success();
                }

                break;

                case 1:
                {
                KatherineTrainingWithUrtaStage2Failure();
                }

                break;

                case 2:
                {
                KatherineTrainingWithUrtaStage2Horny();
                }

                break;
            }

            if (true )
                KatherineTrainingWithUrtaStage2Success();
        }

        public void KatherineTrainingWithUrtaStage2Success()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Success.PunchDucksUnderBlow".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Success.LaysStunnedShakesHead".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Success.PlanWorkedFlawlessly".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 16);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage2Failure()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Failure.PunchDucksUnderBlow".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Failure.HelplessGiveUp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Failure.IndignantlyNoWayCoward".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Failure.TightenHoldGiveUp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Failure.NotBadMove".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Failure.SomeWaterTooMuch".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 8);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage2Horny()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Horny.GoadChargingSuggested".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Horny.EitherWayStrong".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Horny.RollsOnTopLegs".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Horny.BackOnTopPins".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Horny.LegsStillLocked".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2Horny.ThatWasIfWasteland".Localized());

            ClearButtons();

            if (!Player.IsGenderless())
                AddButton(0, "Common.Button.SpitRoast".Localized(), SceneInstance<KatherineThreesomeScene>.Get().SpitroastKath);
            else
                AddButton(0, "Common.Button.SpitRoast".Localized(), null);

            AddButton(1, "Common.Button.Leave".Localized(), KatherineTrainingWithUrtaStage2HornyLeave);
        }

        public void KatherineTrainingWithUrtaStage2HornyLeave()
        {

            if (Player.IsGenderless())
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2HornyLeave.FeaturelessCrotchNoJoin".Localized());
            else
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2HornyLeave.BeenAwayGetBack".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage2HornyLeave.FightHerOffCalling".Localized());

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(false);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage3()
        {

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.StiffLookingOpensDoor".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.CitizenHowMayHelp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.AppearingBehindNoNo".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.HalfHeartedStepsAside".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.NotGoingAsPlanned".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.HeavingMelodramaticSigh".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.LotsOfPractice".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.ImportantAuthoritative".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.PerhapsWillingToHelp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.MoreLikelyIntimidating".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.GoodAtStandingUp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.LeaveThingsInHands".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.DefiantDisrespectful".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.NodUnderstandingTellTry".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.DemonstrationIdea".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.PissOffFreeCity".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.CitizenObeyOrders".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.MenacingGaze".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.SeizingWristsPowerful".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.GrudginglyFollowInsults".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.StepsLetsGoSimple".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3.RepliesOOkayTry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 99)
                KatherineTrainingWithUrtaStage3Success();
            else
            {
                switch (Random.Shared.Next(3))
                {
                    case 0:
                    {
                    KatherineTrainingWithUrtaStage3Success();
                    }

                    break;

                    case 1:
                    {
                    KatherineTrainingWithUrtaStage3Failure();
                    }

                    break;

                    case 2:
                    {
                    KatherineTrainingWithUrtaStage3Horny();
                    }

                    break;
                }

                if (true )
                    KatherineTrainingWithUrtaStage3Success();
            }
        }

        public void KatherineTrainingWithUrtaStage3Success()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.CowerHarshWords".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.MostAuthoritativeTone".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.CongratulateAsWell".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.ThrowsArmsSqueezing".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.MaybeHappyFinally".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.GrinsCoupleOfTests".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.StartsPurrSurprise".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Success.DistractedCuddling".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 16);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage3Failure()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.HurlStringOfInsults".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.InterposingStopIt".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.VisiblyWeeping".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.JustPlayingPart".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.UnableFinishSentence".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.NiceSceneJoinPat".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.WordsHurtMore".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.StartsShoving".Localized());

            if (silly)
            {
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.WhatBugBittenHug".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.StammersFindExcuse".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.LeaveCaptainAlone".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.WhatAreYouDoing".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.HaventPlayedGames".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.BlushesComplainsRidiculous".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.LookBurstOutLaughing".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.LaterCheckUp".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.WinkNoNeedKilljoy".Localized(
                    Player.ShortName
                ));

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.FindOwnFoxDibs".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.LickingLipsMuchFun".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.BlushingFiercely".Localized());

                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.ThreatWouldLikeThat".Localized());
            }
            else
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.MoreInstructionHoldingUp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.SeeYouLater".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Failure.BringSweetAssSoon".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) + 8);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) > 99)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 99);

            ReturnToCamp(1);
        }

        public void KatherineTrainingWithUrtaStage3Horny()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Horny.StandsUpToLeave".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Horny.GoodLookAtAss".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Horny.NextWorkOnThat".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Horny.RubBreastsTailSway".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaStage3Horny.NoGoodToAnyone".Localized());

            ClearButtons();

            if (!Player.IsGenderless())
                AddButton(0, "Common.Button.SpitRoast".Localized(), SceneInstance<KatherineThreesomeScene>.Get().SpitroastKath);
            else
                AddButton(0, "Common.Button.SpitRoast".Localized(), null);

            if (Player.HasCock())
                AddButton(1, "369", SceneInstance<KatherineThreesomeScene>.Get().ThreeSixtyNine);
            else
                AddButton(1, "369", null);

            AddButton(2, "Common.Button.TryLeaving".Localized(), SceneInstance<KatherineThreesomeScene>.Get().RoastYou);
        }

        public void KatherineTrainingWithUrtaComplete()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.WonderWhatElseAgenda".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.ExcitedVixenApproaching".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.AlwaysHappySeeYou".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.NodApprovalLookSuits".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.LatchingArmsWaist".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.KnewHadItIn".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.LaughDisplayOfAffection".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.AcedWrittenTests".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.ProudHowFarRebel".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.CongratulatingWellDone".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.SayThatAgainJob".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.CasualOkayDoing".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.BaffledAssistantVoice".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.AmpleOpportunities".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.AlwaysSeeTwoFavourites".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.WhatDoYouSay".Localized(
                katherine.CockMultiple("I bag " + Player.Mf("his ass", "her pussy") + ", foxy - you'll choke [him] on that supersize sausage of yours", "I bag the back end, I got two shafts for the price of one")
            ));

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.AdvancingMenacingly".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.TailWaggingWetBitch".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.NiceInsideBarDustbins".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaComplete.ItIsExtendArm".Localized());

            KatherineTrainingWithUrtaCompleteContinued();
        }

        public void KatherineTrainingWithUrtaCompleteContinued()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteContinued.LaterThatParagraphBreak".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteContinued.JokeVeryDifferent".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteContinued.AnnounceLeaveShortly".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteContinued.LoveToStayDuties".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteContinued.GirlsWannaHaveFun".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUnlocked, 4);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 200);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_KATHS_APT);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) + 10);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) > 28)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaAffection, 28);

            ClearButtons();

            if (Player.IsGenderless())
                AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), null);
            else
            {
                OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteContinued.EnticingSightDuties".Localized());

                AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), KatherineTrainingWithUrtaCompleteStay);
            }

            AddButton(1, "Common.Button.Leave".Localized(), KatherineTrainingWithUrtaCompleteLeave);
        }

        public void KatherineTrainingWithUrtaCompleteLeave()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteLeave.HermsHaveToGo".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteLeave.DisappointedTryToHave".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteLeave.PayTabWaveBusy".Localized());

            ReturnToCamp(4);
        }

        public void KatherineTrainingWithUrtaCompleteStay()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.AnotherHourSacrifice".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.NeedRemindBitLeft".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.WhatWantMeDo".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.GetBackInLine".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.CalledHimFirst".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.AuthoritativeSuperior".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.CutItBackPrickvixen".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.RemindTaughtStandUp".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.PhysicallySeparating".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.EachOtherNodHug".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.LaughingAmorous".Localized());

            OutputText("Scenes.Katherineemployment.KatherineTrainingWithUrtaCompleteStay.LeadArmInArm".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LetEmFuck".Localized(), SceneInstance<KatherineThreesomeScene>.Get().DoubleStuffKath);

            if (Player.HasCock())
                AddButton(1, "Common.Button.DpKath".Localized(), SceneInstance<KatherineThreesomeScene>.Get().DoublePenetrateKath);
            else
                AddButton(1, "Common.Button.DpKath".Localized(), null);
        }

        public void KatherineGetsEmployed()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.KatherineGetsEmployed.GateGuardsThinkOne".Localized());

            OutputText("Scenes.Katherineemployment.KatherineGetsEmployed.DidntRecognizeMe".Localized());

            OutputText("Scenes.Katherineemployment.KatherineGetsEmployed.ProudlyCongratulate".Localized());

            OutputText("Scenes.Katherineemployment.KatherineGetsEmployed.BitToughWorkHard".Localized());

            OutputText("Scenes.Katherineemployment.KatherineGetsEmployed.SeeAroundTown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) == 200)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 1);
            else
                CoC.Instance.Flags.Set(GameFlagIds.KatherineTraining, 0);

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_KATHS_APT);

            SceneInstance<KatherineScene>.Get().GiveClothing(KatherineScene.KBIT_CLOTHES_UNIFORM);

            SceneInstance<KatherineScene>.Get().GiveClothing(KatherineScene.KBIT_CLOTHES_C_CLOTH);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void PostTrainingAlleyDescription()
        {
            ClearOutput();

            OutputText("Scenes.Katherineemployment.PostTrainingAlleyDescription.AlleywaySeemsEmpty".Localized());

            OutputText("Scenes.Katherineemployment.PostTrainingAlleyDescription.HidingSpotsEmpty".Localized());

            if (!katherine.IsAt(KatherineScene.KLOC_KATHS_APT))
                OutputText("Scenes.Katherineemployment.PostTrainingAlleyDescription.NoteTuckedIntoCrate".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineTraining) >= 100)
                    OutputText("Scenes.Katherineemployment.PostTrainingAlleyDescription.OtherMembersBusy".Localized());
                else
                    OutputText("Scenes.Katherineemployment.PostTrainingAlleyDescription.GuardDetailAllDay".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

    }
}