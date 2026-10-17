using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<DreamsScene>("coc:dreams")]
    public class DreamsScene : Scene
    {

        public override void Entry()
        {
            DreamSelect();
        }

        public bool DreamSelect()
        {
            int daydream = 0;

            var choices = new List<int>();

            var dreamtemp = Random.Shared.Next((int)(Player.Libido / 10 + Player.Corruption / 20)) + Player.Corruption / 20;

            var temp = (int)(Player.HumanScore());

            while (temp > 0)
            {
                choices.Add(0);

                temp--;
            }

            temp = (int)(Player.DogScore() + Player.WolfScore());

            while (temp > 0)
            {
                choices.Add(2);

                temp--;
            }

            temp = (int)(Player.HorseScore());

            while (temp > 0)
            {
                choices.Add(1);

                temp--;
            }

            temp = (int)(Player.CowScore());

            while (temp > 0)
            {
                choices.Add(3);

                temp--;
            }

            temp = (int)(Player.CatScore());

            while (temp > 0)
            {
                choices.Add(4);

                temp--;
            }

            temp = (int)(Player.DemonScore());

            while (temp > 0)
            {
                choices.Add(5);

                temp--;
            }

            if (Player.MinotaurAddicted())
            {
                choices.Add(6);

                choices.Add(6);

                if (Player.HasPerk(PerkIds.MinotaurCumAddict))
                    choices.Add(6);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 4)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 10)
                    choices.Add(7);

                if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 13)
                    choices.Add(7);

                if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 15)
                    choices.Add(7);

                if (CoC.Instance.Flags.Get(GameFlagIds.AkbalSubmissionCounter) > 18)
                    choices.Add(7);

                choices.Add(7);

                choices.Add(7);
            }

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2)
            {
                choices.Add(8);

                choices.Add(8);

                choices.Add(8);
            }

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && Player.HasCock())
            {
                choices.Add(9);

                choices.Add(9);

                choices.Add(9);
            }

            if (Player.Body.Get<SkinPart>().Adj == "latex" || Player.Body.Get<SkinPart>().Adj == "rubber")
            {
                choices.Add(10);

                choices.Add(10);

                choices.Add(11);

                choices.Add(11);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0)
            {
                choices.Add(12);

                choices.Add(12);

                choices.Add(12);

                if (Player.HasCock() && Player.Corruption > 50)
                {
                    choices.Add(13);

                    choices.Add(13);

                    choices.Add(13);
                }
            }

            if (Holidays.IsHalloween(true))
                choices.Add(14);

            if (SceneInstance<AnemoneScene>.Get().KidAXP() >= 40 && Player.LustPercent >= 70 && (int)Player.Gender > 0)
            {
                choices.Add(15);

                choices.Add(15);

                choices.Add(15);
            }

            if (Player.SandTrapScore() >= 2)
            {
                choices.Add(16);

                choices.Add(16);

                choices.Add(16);

                choices.Add(16);
            }

            if (Player.MouseScore() >= 3)
            {
                choices.Add(17);

                choices.Add(17);

                choices.Add(17);

                choices.Add(17);
            }

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0) > 3)
            {
                choices.Add(18);

                choices.Add(18);

                choices.Add(18);

                choices.Add(18);
            }

            var sylviaGetDom = SceneInstance<Areas.Bog.SylviaScene>.Get().sylviaGetDom;

            if (sylviaGetDom >= 75)
            {
                choices.Add(20);

                choices.Add(20);

                choices.Add(20);
            }

            if (sylviaGetDom >= 90)
            {
                choices.Add(20);

                choices.Add(20);
            }

            if (Player.HasStatusEffect(StatusEffectIds.MarbleWithdrawl))
            {
                choices.Add(21);

                choices.Add(21);

                choices.Add(21);

                choices.Add(21);
            }

            if (Player.HasStatusEffect(StatusEffectIds.KitsuneVision))
            {
                choices.Add(22);

                choices.Add(22);

                choices.Add(22);

                choices.Add(22);

                choices.Add(22);

                choices.Add(22);
            }

            if (dreamtemp <= 5 || choices.Count == 0)
                OutputText("Scenes.Dreams.DreamSelect.RestTroubled".Localized());
            else
                if (dreamtemp < 15)
                    OutputText("Scenes.Dreams.DreamSelect.TroubleRelaxing".Localized());
                else
                    OutputText("Scenes.Dreams.DreamSelect.BarelyRest".Localized());

            var dreamLust = dreamtemp;

            if (Player.HasPerk(PerkIds.Lusty))
                dreamLust += (dreamtemp / 3);

            if (Player.HasPerk(PerkIds.WellAdjusted))
                dreamLust = (dreamLust / 2);

            if (dreamLust > 0)
                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(dreamLust)));

            if (dreamtemp > 5 && choices.Count != 0)
            {
                daydream = choices[Random.Shared.Next(choices.Count)];

                if (daydream == 0)
                {
                    if (Player.Corruption <= 33)
                        if (Random.Shared.Next(2) == 0)
                            OutputText("Scenes.Dreams.DreamSelect.TenderSexSweetheart".Localized());
                        else
                        {
                            OutputText("Scenes.Dreams.DreamSelect.BeautifulNudeWord".Localized());

                            if ((int)Player.Gender <= 1 || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                                OutputText("Scenes.Dreams.DreamSelect.WomenWord".Localized());
                            else
                                OutputText("Scenes.Dreams.DreamSelect.MenWord".Localized());

                            OutputText("Scenes.Dreams.DreamSelect.MassagingYouSlowly".Localized());
                        }

                    if (Player.Corruption > 33 && Player.Corruption <= 66)
                        OutputText("Scenes.Dreams.DreamSelect.BeingViolentlyRaped".Localized());

                    if (Player.Corruption > 66)
                    {
                        dreamtemp = Random.Shared.Next(5);

                        if (dreamtemp <= 3)
                            OutputText("Scenes.Dreams.DreamSelect.GivingYourselfFully".Localized());
                        else
                            OutputText("Scenes.Dreams.DreamSelect.BeingCapturedTaken".Localized());
                    }
                }
                else
                    if (daydream == 2)
                    {
                        if ((int)Player.Gender <= 1 || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                        {
                            if (Random.Shared.Next(2) == 0)
                                OutputText("Scenes.Dreams.DreamSelect.LockingKnottedCock".Localized());
                            else
                                OutputText("Scenes.Dreams.DreamSelect.StrokingKnottedPrick".Localized());
                        }
                        else
                            if (Player.InHeat)
                                OutputText("Scenes.Dreams.DreamSelect.BeingPregnant".Localized());
                            else
                                OutputText("Scenes.Dreams.DreamSelect.BeingInHeat".Localized());
                    }
                    else
                        if (daydream == 1)
                        {
                            if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                                OutputText("Scenes.Dreams.DreamSelect.RunningPlainsHarem".Localized());
                            else
                                OutputText("Scenes.Dreams.DreamSelect.RunningOwnFarm".Localized());
                        }
                        else
                            if (daydream == 3)
                            {
                                if ((Player.Gender == Gender.Male && Player.BiggestBreastSize() >= 5) || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                                    OutputText("Scenes.Dreams.DreamSelect.FullLactatingBreasts".Localized());
                                else
                                    if ((int)Player.Gender >= 2)
                                        OutputText("Scenes.Dreams.DreamSelect.WanderingForestCradling".Localized(
                                            Player.VaginaDescript(0)
                                        ));
                                    else
                                        if (Player.Gender == Gender.Genderless)
                                            OutputText("Scenes.Dreams.DreamSelect.WanderingForestCarrying".Localized());
                                        else
                                            OutputText("Scenes.Dreams.DreamSelect.RoamingMountainSides".Localized());
                            }
                            else
                                if (daydream == 4)
                                {
                                    if (Player.HasVagina() && (!Player.HasCock() || Random.Shared.Next(2) == 0))
                                        OutputText("Scenes.Dreams.DreamSelect.BeingFullCat".Localized());
                                    else
                                        OutputText("Scenes.Dreams.DreamSelect.ProwlingForestAs".Localized());
                                }
                                else
                                    if (daydream == 5)
                                        OutputText("Scenes.Dreams.DreamSelect.BeingUsedAbused".Localized());
                                    else
                                        if (daydream == 6)
                                        {
                                            OutputText("Scenes.Dreams.DreamSelect.ManyEncounters".Localized());

                                            OutputText("Scenes.Dreams.DreamSelect.PresenceNotUnnoticed".Localized());

                                            OutputText("Scenes.Dreams.DreamSelect.MinotaursGrabDicks".Localized());

                                            OutputText("Scenes.Dreams.DreamSelect.DelugeFromFlesh".Localized());

                                            OutputText("Scenes.Dreams.DreamSelect.RealizeAtLast".Localized());

                                            OutputText("Scenes.Dreams.DreamSelect.AwakenVividDream".Localized());

                                            OutputText("Scenes.Dreams.DreamSelect.ParagraphBreak".Localized());
                                        }
                                        else
                                            if (daydream == 7)
                                            {
                                                OutputText("Scenes.Dreams.DreamSelect.GrindingRearInto".Localized());

                                                OutputText("Scenes.Dreams.DreamSelect.MoanOutShivering".Localized());

                                                OutputText("Scenes.Dreams.DreamSelect.AkbalsStrongHand".Localized());

                                                OutputText("Scenes.Dreams.DreamSelect.CumScreamGlorious".Localized());

                                                OutputText("Scenes.Dreams.DreamSelect.GloryUntoAkbal".Localized());

                                                OutputText("Scenes.Dreams.DreamSelect.AwakeWithStartGrumbling".Localized());
                                            }
                                            else
                                                if (daydream == 8)
                                                {
                                                    OutputText("Scenes.Dreams.DreamSelect.DriftingDarknessArrive".Localized());

                                                    OutputText("Scenes.Dreams.DreamSelect.HandsStrokeTits".Localized());

                                                    OutputText("Scenes.Dreams.DreamSelect.EventuallyCumSplattering".Localized());

                                                    OutputText("Scenes.Dreams.DreamSelect.AwakeWithStartYour".Localized());
                                                }
                                                else
                                                    if (daydream == 9)
                                                    {
                                                        OutputText("Scenes.Dreams.DreamSelect.DriftingDarknessArriveAgain".Localized());

                                                        OutputText("Scenes.Dreams.DreamSelect.HandsRoamingBust".Localized());

                                                        OutputText("Scenes.Dreams.DreamSelect.TitfuckFeelsFantastic".Localized());

                                                        OutputText("Scenes.Dreams.DreamSelect.PartnerLicksLips".Localized());

                                                        OutputText("Scenes.Dreams.DreamSelect.AwakeWithStartAgain".Localized());
                                                    }
                                                    else
                                                        if (daydream == 10)
                                                        {
                                                            OutputText("Scenes.Dreams.DreamSelect.LipsSwellingSingle".Localized());

                                                            OutputText("Scenes.Dreams.DreamSelect.SomeoneEntersRoom".Localized());

                                                            OutputText("Scenes.Dreams.DreamSelect.LikeBeforeDrop".Localized());

                                                            OutputText("Scenes.Dreams.DreamSelect.CycleRepeating".Localized());

                                                            OutputText("Scenes.Dreams.DreamSelect.AwakeFromDreamPanting".Localized());

                                                            OutputText("Scenes.Dreams.DreamSelect.NotCertainHowTo".Localized());
                                                        }
                                                        else
                                                            if (daydream == 11)
                                                            {
                                                                OutputText("Scenes.Dreams.DreamSelect.DodgingFoesAttack".Localized());

                                                                OutputText("Scenes.Dreams.DreamSelect.StumbleAttempting".Localized());

                                                                OutputText("Scenes.Dreams.DreamSelect.OpponentBehindYou".Localized());

                                                                OutputText("Scenes.Dreams.DreamSelect.AwakePantingAgain".Localized());

                                                                OutputText("Scenes.Dreams.DreamSelect.NotCertainHowAgain".Localized());
                                                            }
                                                            else
                                                                if (daydream == 12)
                                                                {
                                                                    OutputText("Scenes.Dreams.DreamSelect.StrangeThingsNotFaces".Localized());

                                                                    OutputText("Scenes.Dreams.DreamSelect.LipsAndMouthsCrowd".Localized());

                                                                    OutputText("Scenes.Dreams.DreamSelect.DontStopUntil".Localized());
                                                                }
                                                                else
                                                                    if (daydream == 13)
                                                                    {
                                                                        OutputText("Scenes.Dreams.DreamSelect.RecliningContently".Localized());

                                                                        OutputText("Scenes.Dreams.DreamSelect.PushHerBackDown".Localized());

                                                                        OutputText("Scenes.Dreams.DreamSelect.HeadInEitherHand".Localized());

                                                                        OutputText("Scenes.Dreams.DreamSelect.FittingLookFor".Localized());

                                                                        OutputText("Scenes.Dreams.DreamSelect.AwakenErectFrustrated".Localized());
                                                                    }
                                                                    else
                                                                        if (daydream == 14)
                                                                        {
                                                                            OutputText("Scenes.Dreams.DreamSelect.WakingInMiddle".Localized());

                                                                            OutputText("Scenes.Dreams.DreamSelect.SnapOfTwig".Localized());

                                                                            OutputText("Scenes.Dreams.DreamSelect.ConfusionOvertakes".Localized());

                                                                            OutputText("Scenes.Dreams.DreamSelect.HollowVoiceTaunts".Localized());

                                                                            OutputText("Scenes.Dreams.DreamSelect.ScreamThatDont".Localized());

                                                                            OutputText("Scenes.Dreams.DreamSelect.LaunchYourselfUp".Localized());
                                                                        }
                                                                        else
                                                                            if (daydream == 15)
                                                                            {
                                                                                OutputText("Scenes.Dreams.DreamSelect.SomethingUnusual".Localized());

                                                                                SceneInstance<AnemoneScene>.Get().KidADreams();
                                                                            }
                                                                            else
                                                                                if (daydream == 16)
                                                                                {
                                                                                    if ((int)Player.Gender <= 1)
                                                                                    {
                                                                                        if (Random.Shared.Next(2) == 0)
                                                                                            OutputText("Scenes.Dreams.DreamSelect.SomethingTrulyUnusual".Localized());
                                                                                        else
                                                                                        {
                                                                                            OutputText("Scenes.Dreams.DreamSelect.SomethingUnrealAgain".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.EyesSlowlyOpen".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.YoungGirlPulls".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.PairOfYouEnter".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.NudgesThroughDoor".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.VoiceCallsInside".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.AllGetUpRelaxing".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.SinksInNow".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.AwakeWithMoan".Localized());

                                                                                            if (Player.HasCock())
                                                                                                OutputText("Scenes.Dreams.DreamSelect.FullyErectThrobbing".Localized());
                                                                                            else
                                                                                                if (Player.HasVagina())
                                                                                                    OutputText("Scenes.Dreams.DreamSelect.DrippingWetNeed".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.ShudderAsRecall".Localized());
                                                                                        }

                                                                                        Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        OutputText("Scenes.Dreams.DreamSelect.OddThings".Localized());

                                                                                        OutputText("Scenes.Dreams.DreamSelect.DifficultToFind".Localized());

                                                                                        Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
                                                                                    }
                                                                                }
                                                                                else
                                                                                    if (daydream == 17)
                                                                                    {
                                                                                        if (Player.Gender == Gender.Genderless || Random.Shared.Next(2) == 0)
                                                                                        {
                                                                                            OutputText("Scenes.Dreams.DreamSelect.RunningForestSomething".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.FightBackDesire".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.FeelStrangeLookDown".Localized());
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            OutputText("Scenes.Dreams.DreamSelect.UndergroundBurrow".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.GreatLarder".Localized());

                                                                                            if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                                                                                                OutputText("Scenes.Dreams.DreamSelect.ThingsBlurMoment".Localized());
                                                                                            else
                                                                                                OutputText("Scenes.Dreams.DreamSelect.ThingsBlurMomentAgain".Localized());
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                        if (daydream == 18)
                                                                                        {
                                                                                            OutputText("Scenes.Dreams.DreamSelect.SomethingUnrealAgain".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.FeelFamiliarTingling".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.FiveChainedImps".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.LookAtFourthImp".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.ClimbOnTop".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.FuckHimFeral".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.RemoveYourselfImp".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.ConcubinesBringFood".Localized());

                                                                                            OutputText("Scenes.Dreams.DreamSelect.WakeUpParasites".Localized());
                                                                                        }
                                                                                        else
                                                                                            if (daydream == 20)
                                                                                            {
                                                                                                OutputText("Scenes.Dreams.DreamSelect.PairOfVioletEyes".Localized());

                                                                                                OutputText("Scenes.Dreams.DreamSelect.ShudderAlmostShrink".Localized());

                                                                                                OutputText("Scenes.Dreams.DreamSelect.AsFingersMeet".Localized());

                                                                                                OutputText("Scenes.Dreams.DreamSelect.GentlyLovinglyCaress".Localized());

                                                                                                OutputText("Scenes.Dreams.DreamSelect.AsTheyContinue".Localized());

                                                                                                OutputText("Scenes.Dreams.DreamSelect.WakeUpWithShiver".Localized());
                                                                                            }
                                                                                            else
                                                                                                if (daydream == 21)
                                                                                                {
                                                                                                    OutputText("Scenes.Dreams.DreamSelect.RichCreamyMilk".Localized());

                                                                                                    OutputText("Scenes.Dreams.DreamSelect.EarthyScentFarm".Localized());

                                                                                                    OutputText("Scenes.Dreams.DreamSelect.BlissfulHeatTrickles".Localized());

                                                                                                    OutputText("Scenes.Dreams.DreamSelect.SwallowShivering".Localized());

                                                                                                    OutputText("Scenes.Dreams.DreamSelect.AwakeWithStartThird".Localized());

                                                                                                    OutputText("Scenes.Dreams.DreamSelect.NoMatterHowLong".Localized());
                                                                                                }
                                                                                                else
                                                                                                    if (daydream == 22)
                                                                                                    {
                                                                                                        OutputText("Scenes.Dreams.DreamSelect.LaughterInEars".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.SensationRipples".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.SaysTheWind".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.EyesOpenAtSound".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.BlinkAndShesBeside".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.CoolScentWintergreen".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.WeMissedYou".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.GlanceUpAndShes".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.MaybeForTheBest".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.GettingStarted".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.LiftYourHeadShes".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.SoHardToThink".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.SuckSharpBreath".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.WithProofOfDesire".Localized());

                                                                                                        OutputText("Scenes.Dreams.DreamSelect.WontYouStay".Localized());
                                                                                                    }

                Go<CampScene>();

                return true;
            }

            return false;
        }

        public void DayTenDreams()
        {

            RegisterMacro("momdream", Player.SexOrientation <= 50);

            OutputText("Scenes.Dreams.DayTenDreams.TaskCompletedReturn".Localized());

            OutputText("Scenes.Dreams.DayTenDreams.YourPeopleRejoice".Localized());

            OutputText("Scenes.Dreams.DayTenDreams.SaysVoiceWarmth".Localized());

            if (Player.HasCock() || Player.SexOrientation > 50)
                OutputText("Scenes.Dreams.DayTenDreams.ThankInResponse".Localized());

            if (Player.SexOrientation <= 50)
            {
                if (Player.HasCock())
                {
                    if (Player.Corruption < 50)
                        OutputText("Scenes.Dreams.DayTenDreams.HandsTakeHold".Localized());
                    else
                    {
                        OutputText("Scenes.Dreams.DayTenDreams.HandsTakeHoldAgain".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.SheExclaims".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.GruntOutAnswer".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.MotherLapsAtYour".Localized());
                    }
                }
                else
                    if (Player.Corruption < 50)
                        OutputText("Scenes.Dreams.DayTenDreams.ThrowYourselfAtHer".Localized());
                    else
                    {
                        OutputText("Scenes.Dreams.DayTenDreams.NowThatInside".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.FewMomentsUncertainty".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.TryToTellHer".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.CarefulFacadeVanishes".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.NotTakeYouLong".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.CloseEyesExhausted".Localized());
                    }
            }
            else
                if (Player.Corruption < 50)
                    OutputText("Scenes.Dreams.DayTenDreams.HandsSlipIntoPants".Localized());
                else
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Dreams.DayTenDreams.HandsInPantsAgain".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.GaspAtSensation".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.DeserveAReward".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.AsIfToPunctuate".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.FeelingStartsWell".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.CouldntEverHave".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.TwoBodiesDissolve".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Dreams.DayTenDreams.HandsInPantsThird".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.CumSplattersThroat".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.SmileWithGlazedTeeth".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.AskIfHeLikes".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.YoureAGoodGirl".Localized());

                        OutputText("Scenes.Dreams.DayTenDreams.BallsGurgleFill".Localized());
                    }

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Dreams.DayTenDreams.ShockedAwakeBlink".Localized());

                OutputText("Scenes.Dreams.DayTenDreams.GettingToYou".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
            }
            else
            {
                OutputText("Scenes.Dreams.DayTenDreams.WakeUpInSweat".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
            }

            Go<CampScene>();
        }

        public void FuckedUpCockDreamChange()
        {

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.DreamsComeFast".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.BodyFeelsOddMisshapen".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.BodyNotAtAll".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.DreamsShiftsAbruptly".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.SoftSsshtNoise".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.AtBaseOfColumn".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.BodyCurvyVoluptuous".Localized());

            OutputText("Scenes.Dreams.FuckedUpCockDreamChange.CaptainOhYes".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), DisplacerDreamII);
        }

        public void DisplacerDreamII()
        {
            ClearOutput();

            OutputText("Scenes.Dreams.DisplacerDreamII.DreamShiftsOnceMore".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.UrgeHerRemainQuiet".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.ShockShortLived".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.DreamShiftsAgainShes".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.TurningAttentionBack".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.DreamyHazeContinue".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.FeelTipOfYour".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.LoudPrimalGrowl".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.DreamShiftsAgainWolf".Localized());

            if (silly)
                OutputText("Scenes.Dreams.DisplacerDreamII.KWord".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.NotADropJism".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.SoftSsshtNoise".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamII.PassThroughDoorway".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), DisplacerDreamIII);
        }

        public void DisplacerDreamIII()
        {
            ClearOutput();

            OutputText("Scenes.Dreams.DisplacerDreamIII.DetailsImmediately".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamIII.SoonEvenDetails".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamIII.SettleBackIn".Localized());

            OutputText("Scenes.Dreams.DisplacerDreamIII.NewCoeurlCock".Localized());

            var x = Player.CockTotal();

            while (x > 0)
            {
                x--;

                if (Player.Cocks[x].sock == "amaranthine" && Player.Cocks[x].Id != BodyPartTypes.Displacer)
                {
                    if (Player.Cocks[x].Id != BodyPartTypes.Dog)
                        Player.Cocks[x].KnotMultiplier = 1.5f;

                    Player.Cocks[x].Id = BodyPartTypes.Displacer;
                }
            }

            Go<CampScene>();
        }

        public void MarblePureNightmare1()
        {
            ClearOutput();

            RegisterMacro("marbleMultiKids", CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 1);

            RegisterMacro("marbleSoleBoy", CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 1);

            OutputText("Scenes.Dreams.MarblePureNightmare1.RestPremature".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.GettingUpLook".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.GoOutsideLook".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 0)
                OutputText("Scenes.Dreams.MarblePureNightmare1.FirstInstinct".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.SearchDifferentSpots".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.GoneToWhitney".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KeltKilled) > 0)
                OutputText("Scenes.Dreams.MarblePureNightmare1.WhitneyWontBeHappy".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.WhitneyFlippedOutOverKelly) == 1)
                OutputText("Scenes.Dreams.MarblePureNightmare1.WhitneyNotHappy".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.JustAsAboutTo".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.BodySlowlyBeing".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.DoesntFeelReal".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare1.DoesntFeelRealAgain".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MarblePureNightmare2);
        }

        public void MarblePureNightmare2()
        {
            ClearOutput();

            OutputText("Scenes.Dreams.MarblePureNightmare2.EyesOpenLet".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBadEnded) >= 1)
                OutputText("Scenes.Dreams.MarblePureNightmare2.WhatsWrongSweetie".Localized());
            else
                OutputText("Scenes.Dreams.MarblePureNightmare2.WhatsWrongSweetieAgain".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare2.BreatheInRelief".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare2.WhispersIntoEar".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) > 0)
                OutputText("Scenes.Dreams.MarblePureNightmare2.ForYouAndChild".Localized());
            else
                OutputText("Scenes.Dreams.MarblePureNightmare2.SeeEndTogether".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare2.BringsFaceCloser".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare2.ILoveYou".Localized());

            OutputText("Scenes.Dreams.MarblePureNightmare2.FallAsleepWarmth".Localized());

            Go<CampScene>();
        }

        public void MarbleCorruptNightmare()
        {

            var randomCorruption = Random.Shared.Next((int)(Player.Libido / 10 + Player.Corruption / 20)) + Player.Corruption / 20;

            if (randomCorruption < 15)
                OutputText("Scenes.Dreams.MarbleCorruptNightmare.TroubleRelaxing".Localized());
            else
                OutputText("Scenes.Dreams.MarbleCorruptNightmare.BarelyRest".Localized());

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.BeingInArmsOf".Localized());

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.ThingsOfBeauty".Localized());

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.OpenMouthVainly".Localized());

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.UnableToContain".Localized());

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.ThirstReturns".Localized());

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.NotEvenTheMilk".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            OutputText("Scenes.Dreams.MarbleCorruptNightmare.SwingHipsLike".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MarbleNightmareWakeUp);
        }

        public void MarbleNightmareWakeUp()
        {
            ClearOutput();

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.NoticingBeforehandChange".Localized());

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.AndYetEvenNow".Localized());

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BodyAlreadyActed".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.ThrustPenisBetween".Localized());

                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.NoticesBodyBeginning".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.PreCumSoaksLegs".Localized());
                else
                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.PreCumLegsAgain".Localized());

                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.JustBeforeBreaking".Localized());

                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.KissesYourEar".Localized());

                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.TorrentOfPleasure".Localized());

                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.HipsBuckSpurt".Localized());

                if (Player.CumQ() > 1000)
                {
                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.FallingFormingPool".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.GigglesStillWhispering".Localized());
                }
                else
                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.DropsDrippingStain".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.RockHipsToGrind".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.HeatInLoinsOnly".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BackArchingLetGo".Localized());

                    if (Player.Wetness() < 2)
                        OutputText("Scenes.Dreams.MarbleNightmareWakeUp.MoisteningHerThigh".Localized());
                    else
                        if (Player.Wetness() < 4)
                            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.CoveringHerThigh".Localized());
                        else
                            if (Player.Wetness() == 4)
                                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.SoakingEntireThigh".Localized());
                            else
                                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.SquirtingSuchForce".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.FinallyOrgasmSubsides".Localized());
                }
                else
                {
                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BuckHipsForward".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BrushingOfLover".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.LustBecomesTooMuch".Localized());

                    if (Player.Anal.Looseness < 2)
                    {
                        OutputText("Scenes.Dreams.MarbleNightmareWakeUp.TwoFingersInside".Localized());

                        OutputText("Scenes.Dreams.MarbleNightmareWakeUp.FingersPushTightness".Localized());
                    }
                    else
                        if (Player.Anal.Looseness < 4)
                        {
                            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.TwoFingersWithoutTrouble".Localized());

                            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.WithoutFrictionFingers".Localized());
                        }
                        else
                            if (Player.Anal.Looseness < 5)
                            {
                                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.ThreeFingersBarelyFeel".Localized());

                                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.DespiteEffortsSheCan".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.ThreeFingersWithYour".Localized());

                                OutputText("Scenes.Dreams.MarbleNightmareWakeUp.SoundsSurprisedPutsAll".Localized());
                            }

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BurningBelowPeaks".Localized());

                    if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.ClovenHoofed || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.RootLegs)
                        OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BodyShiversMakingYou".Localized());
                    else
                        OutputText("Scenes.Dreams.MarbleNightmareWakeUp.ToesCurlBackArches".Localized());

                    OutputText("Scenes.Dreams.MarbleNightmareWakeUp.HoldOnMarbleTightens".Localized());
                }

            Player.Orgasm("All");

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.LiftsHerLeg".Localized());

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.SmileLovingButA".Localized());

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.DespiteFatigueFind".Localized());

            OutputText("Scenes.Dreams.MarbleNightmareWakeUp.BringsHeadBack".Localized());

            Go<CampScene>();
        }

    }
}