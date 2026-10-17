using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<MinervaPurificationScene>("coc:high_mountains_minerva_purification")]
    public class MinervaPurificationScene : Scene
    {

        public override void Entry()
        {
            CheckRathazul();
        }

        public bool CheckRathazul()
        {

            var rathazul = SceneInstance<Npcs.RathazulScene>.Get();

            return rathazul.FollowerRathazul() || rathazul.mixologyXP >= 12;
        }

        public bool CheckJojo()
        {

            return Player.HasStatusEffect(StatusEffectIds.PureCampJojo);
        }

        public bool CheckMarae()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1 && CoC.Instance.Flags.Get(GameFlagIds.MaraeQuestComplete) >= 1;
        }

        public string PlayerParentDesc(bool caps = false)
        {

            var __sw0_0 = (int)Player.Gender;

            if (__sw0_0 == (int)Gender.Genderless)
                return caps ? Player.Mf("Mommy", "Daddy") : Player.Mf("mommy", "daddy");

            if (__sw0_0 == (int)Gender.Male)
                return caps ? "Daddy" : "daddy";

            if (__sw0_0 == (int)Gender.Female)
                return caps ? "Mommy" : "mommy";

            if (__sw0_0 == (int)Gender.Hermaphrodite)
                return caps ? "Mommy-Daddy" : "mommy-daddy";

            if (true)
                return Player.Mf("mommy", "daddy");
        }

        public void StartPurification()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.StartPurification.ThinkingAboutMinervaSaid".Localized());

            OutputText("Scenes.Minervapurification.StartPurification.NotSureHowBroach".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
                OutputText("Scenes.Minervapurification.StartPurification.CloseContactErectsPenis".Localized());

            OutputText("Scenes.Minervapurification.StartPurification.EagerlyEmbracesNearlySqueezing".Localized());

            OutputText("Scenes.Minervapurification.StartPurification.AsksWithExcitementExperience".Localized());

            if (CheckMarae() && !CheckJojo() && !CheckRathazul())
            {
                OutputText("Scenes.Minervapurification.StartPurification.RememberMaraeMindLeft".Localized());

                OutputText("Scenes.Minervapurification.StartPurification.HearsGoddessPerksHopeful".Localized());

                OutputText("Scenes.Minervapurification.StartPurification.LooksSadEyesCant".Localized());

                OutputText("Scenes.Minervapurification.StartPurification.BreakEmbraceNeedGo".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 1);

                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 0);

                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 0);
            }
            else
                if (!CheckMarae() && CheckJojo() && !CheckRathazul())
                {
                    OutputText("Scenes.Minervapurification.StartPurification.JojoHelpedOvercomeCorruption".Localized());

                    OutputText("Scenes.Minervapurification.StartPurification.HearsMonkFriendIntrigued".Localized());

                    OutputText("Scenes.Minervapurification.StartPurification.SpendTimeReluctantDepart".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 1);

                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 0);
                }
                else
                    if (!CheckMarae() && !CheckJojo() && CheckRathazul())
                    {
                        OutputText("Scenes.Minervapurification.StartPurification.LongRathazulKnowsPurification".Localized());

                        OutputText("Scenes.Minervapurification.StartPurification.SoundNervousBitingLip".Localized());

                        OutputText("Scenes.Minervapurification.StartPurification.AssureRathazulComeSomething".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 0);

                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 0);

                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 1);
                    }
                    else
                        if (CheckMarae() && CheckJojo() && !CheckRathazul())
                        {
                            OutputText("Scenes.Minervapurification.StartPurification.JojoSpiritualPowersHelp".Localized());

                            OutputText("Scenes.Minervapurification.StartPurification.TellAboutMaraeJojo".Localized());

                            OutputText("Scenes.Minervapurification.StartPurification.DetailAboutLooksThoughtful".Localized());

                            OutputText("Scenes.Minervapurification.StartPurification.SpendTimeGratefulSiren".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 1);

                            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 1);

                            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 0);
                        }
                        else
                            if (CheckMarae() && !CheckJojo() && CheckRathazul())
                            {
                                OutputText("Scenes.Minervapurification.StartPurification.MaraeWeakAloneIngredients".Localized());

                                OutputText("Scenes.Minervapurification.StartPurification.TellAboutMaraeRathazul".Localized());

                                OutputText("Scenes.Minervapurification.StartPurification.ServiceRathazulComesHandy".Localized());

                                OutputText("Scenes.Minervapurification.StartPurification.SpendGratefulSirenAgain".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 1);

                                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 0);

                                CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 1);
                            }
                            else
                                if (!CheckMarae() && CheckJojo() && CheckRathazul())
                                {
                                    OutputText("Scenes.Minervapurification.StartPurification.BetweenJojoRathazulConfident".Localized());

                                    OutputText("Scenes.Minervapurification.StartPurification.TellAboutJojoRathazul".Localized());

                                    OutputText("Scenes.Minervapurification.StartPurification.SpendGratefulSirenThird".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 0);

                                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 1);

                                    CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 1);
                                }
                                else
                                    if (CheckMarae() && CheckJojo() && CheckRathazul())
                                    {
                                        OutputText("Scenes.Minervapurification.StartPurification.ThinkOfThreePeople".Localized());

                                        OutputText("Scenes.Minervapurification.StartPurification.TellAboutAllThree".Localized());

                                        OutputText("Scenes.Minervapurification.StartPurification.DiscussWhoApproachFirst".Localized());

                                        OutputText("Scenes.Minervapurification.StartPurification.SpendGratefulSirenFourth".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 2);

                                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 1);

                                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationJojoTalked, 1);

                                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationRathazulTalked, 1);
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Minervapurification.StartPurification.AskIfSheIdeas".Localized());

                                        OutputText("Scenes.Minervapurification.StartPurification.SharkHarpyFurrowsBrow".Localized());

                                        OutputText("Scenes.Minervapurification.StartPurification.HandOnShoulderComfort".Localized());

                                        OutputText("Scenes.Minervapurification.StartPurification.SpendGratefulSirenFifth".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 1);
                                    }

            DoNext(() => ReturnToCamp(1));
        }

        public void PurificationByMarae()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PurificationByMarae.ReturnWithSeedExcited".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) == 0)
                OutputText("Scenes.Minervapurification.PurificationByMarae.ExcitedCallsDrawsMinerva".Localized());
            else
                OutputText("Scenes.Minervapurification.PurificationByMarae.ExcitedDrawsAttentionHarpy".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.NodShowSeedExplain".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.LooksWideEyesAmazement".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.GrabsHandDragsSpring".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.SmileSqueezeHandSeed".Localized());

            Player.RemoveKeyItem("Marae's Seed");

            OutputText("Scenes.Minervapurification.PurificationByMarae.LooksBewilderedIsThat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
            {
                OutputText("Scenes.Minervapurification.PurificationByMarae.SeesStateSpringFurrows".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinervaCorruptionProgress, -1);
            }

            OutputText("Scenes.Minervapurification.PurificationByMarae.DeityVoiceChimingChildren".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.LooksUpReverence".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.SmilesBeatificallyLaysHands".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.WatchTrepidationBeginsChange".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) != 0)
                OutputText("Scenes.Minervapurification.PurificationByMarae.PostureShiftsWidenedHips".Localized());
            else
                OutputText("Scenes.Minervapurification.PurificationByMarae.PostureWidenedHipsAgain".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.BreastsSwellMagnificently".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.MaraeFaltersRemovesHands".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.TransformationGaspsChestRising".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.AssuresDismayedThatMuch".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.QuestionsSomethingOpensEyes".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.RelievedThenSways".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.LookBackSweptHug".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.SmileHappyShesBetter".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.SaysNotSureExplain".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.HearsRequestWondersWork".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minervapurification.PurificationByMarae.Father".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Minervapurification.PurificationByMarae.And".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.PurificationByMarae.Mother".Localized());

            OutputText("Scenes.Minervapurification.PurificationByMarae.WholeNewRaceWorld".Localized());

            AwardAchievement(AchievementIds.DivineIntervention);

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 10);

            CoC.Instance.Flags.Set(GameFlagIds.MinervaTowerTree, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Babymaking".Localized(), AgreeToMakeBaby);

            AddButton(1, "Common.Button.WaitABit".Localized(), DeclineToMakeBaby);
        }

        public void PurificationByJojoPart1()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PurificationByJojoPart1.ApproachMonkFateWeighing".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart1.TalkAboutConditionWorries".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart1.BenevolentSmileHmm".Localized());

            DoNext(() => PurificationByJojoPart2());
        }

        public void PurificationByJojoPart2()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.ClimbLongerHarder".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
            {
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.ExcitedAttentionPlaceholder".Localized((CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) < 3 ? "from the harpy that is trying to get her attention, the bird woman trying all kinds of lewd things to get Minerva to mate with her" : "from the harpies that fight for her touch, the bird women trying all kinds of lewd things to get Minerva to mate with them")));

                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.WordsStopEyes".Localized());
            }
            else
            {
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.ExcitedCallsDrawsMinerva".Localized());

                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.WordsStopEyesAgain".Localized());
            }

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.NodsHisHead".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.AvoidingLookingHarpySluts".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.AsksBelievesCapable".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.NoddingQuicklyGrabsRushes".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.NodsThenStaresPool".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) != 0)
            {
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.LooksSomethingAddedPollute".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinervaCorruptionProgress, -1);
            }

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.AsksApproachKneelWater".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.ThanksNamePerfectIm".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.WonderDoingWaterGlows".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.NotSayUnaffectedExplode".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.NewColorsChangedThing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) != 0)
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.ThinkBallsMayShrunk".Localized());
            else
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.BallsSwollenCantaloupes".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.SproutedSecondPairWings".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.RelievedWonderfulNewsConfess".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.SighsGoldenLooksDown".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.MouseMonkBlushingDepth".Localized((noFur ? "vibrantly" : "so vibrantly you can see it through the fur on his cheeks")));

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.PoliteWorkDoneMust".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.JojoSightShowsAppreciation".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.SmileBackHappyAble".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.StrokesGentlyHoldsThoughtfully".Localized(
                 Player.HasCock() ? "father" : Player.HasVagina() ? "mother" : Player.Mf("father", "mother")
            ));

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.HearsRequestWondersWork".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.Father".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.And".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.PurificationByJojoPart2.Mother".Localized());

            OutputText("Scenes.Minervapurification.PurificationByJojoPart2.WholeNewRaceWorld".Localized());

            AwardAchievement(AchievementIds.DivineIntervention);

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 10);

            ClearButtons();

            AddButton(0, "Common.Button.Babymaking".Localized(), AgreeToMakeBaby);

            AddButton(1, "Common.Button.WaitABit".Localized(), DeclineToMakeBaby);
        }

        public void PurificationByRathazul()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PurificationByRathazul.CarefulVialHurryInto".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
                OutputText("Scenes.Minervapurification.PurificationByRathazul.ExcitedDrawsAttentionHarpy".Localized());
            else
                OutputText("Scenes.Minervapurification.PurificationByRathazul.ExcitedCallsDrawsMinerva".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.TellOfPotionOffer".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.SoftSadSmile".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
                OutputText("Scenes.Minervapurification.PurificationByRathazul.DontThinkHaveLong".Localized());
            else
                OutputText("Scenes.Minervapurification.PurificationByRathazul.CantWorseThanNow".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.TellsTakesHandPeels".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) > 0)
            {
                OutputText("Scenes.Minervapurification.PurificationByRathazul.VialFallsShattersSpilling".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinervaCorruptionProgress, -1);
            }

            Player.RemoveKeyItem("Rathazul's Purity Potion");

            OutputText("Scenes.Minervapurification.PurificationByRathazul.RushSideGrabsHand".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.LowerBackBulgesWings".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.CrackingBonesHipsDisplace".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.GhastlySoundsFadeSlumps".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.OpensMouthCrossEyed".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.StoicallyRepeatQuestion".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.StandsUpStretchingFlapping".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.AskIfSureAlright".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.NodsInsistentlyEmbarrassed".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.HearsRequestWondersWork".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minervapurification.PurificationByRathazul.Father".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Minervapurification.PurificationByRathazul.And".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.PurificationByRathazul.Mother".Localized());

            OutputText("Scenes.Minervapurification.PurificationByRathazul.WholeNewRaceWorld".Localized());

            AwardAchievement(AchievementIds.DivineIntervention);

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 10);

            ClearButtons();

            AddButton(0, "Common.Button.Babymaking".Localized(), AgreeToMakeBaby);

            AddButton(1, "Common.Button.WaitABit".Localized(), DeclineToMakeBaby);
        }

        public void GrowTreePostPurification()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.TellSpecialSeedMarae".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.LooksWideEyesAmazement".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.GrabsHandDragsSpring".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.SmileSqueezeHandSeed".Localized());

            Player.RemoveKeyItem("Marae's Seed");

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.LooksBewilderedIsThat".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.DeityVoiceChimingChildren".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.LooksUpReverence".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.RelievedThenSways".Localized());

            OutputText("Scenes.Minervapurification.GrowTreePostPurification.SaysBeautifulHugeTree".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaTowerTree, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AgreeToMakeBaby()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Minervapurification.AgreeToMakeBaby.DontHaveThinkHard".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.AgreeToMakeBaby.CantHelpGrinGiggle".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                PureMinervaSexMenu();

                return;
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    if (!Player.IsPregnant())
                    {
                        OutputText("Scenes.Minervapurification.AgreeToMakeBaby.DontThinkHardAgain".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 20f));
                    }
                    else
                    {
                        OutputText("Scenes.Minervapurification.AgreeToMakeBaby.DontThinkHardThird".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                    }

                    PureMinervaSexMenu();

                    return;
                }
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText("Scenes.Minervapurification.AgreeToMakeBaby.GrinningPlayfullyWhoFathers".Localized());

                        if (!Player.IsPregnant())
                            OutputText("Scenes.Minervapurification.AgreeToMakeBaby.OrBothHermsPregnant".Localized());
                        else
                            OutputText("Scenes.Minervapurification.AgreeToMakeBaby.JokesHardGetPregnant".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                        PureMinervaSexMenu();

                        return;
                    }
                    else
                        OutputText("Scenes.Minervapurification.AgreeToMakeBaby.NoSexualEndowments".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineToMakeBaby()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.DeclineToMakeBaby.HardSayNoDecline".Localized(
                 Player.HasCock() ? "father" : Player.HasVagina() ? "mother" : Player.Mf("father", "mother")
            ));

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Minervapurification.DeclineToMakeBaby.HandReachesDownSqueezing".Localized(
                     Player.ClothedOrNakedLower("and grabs the bulge in your " + Player.ArmorName, "towards your naked crotch")
                ));
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Minervapurification.DeclineToMakeBaby.HandSlipsArmorExploring".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Minervapurification.DeclineToMakeBaby.HandGrabsBulgeSqueezing".Localized());
                    else
                        OutputText("Scenes.Minervapurification.DeclineToMakeBaby.HandSlipsNothingYou".Localized());

            OutputText("Scenes.Minervapurification.DeclineToMakeBaby.GentlyHandAwayKiss".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            DoNext(() => ReturnToCamp(1));
        }

        public void VisitTowerPostPurification()
        {
            ClearOutput();

            var chooserArray = new List<int> { 0, 1, 2 };

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                chooserArray.Add(3);

            if (Player.IsPregnant())
                chooserArray.Add(4);

            int chooser = chooserArray[Random.Shared.Next(chooserArray.Count)];

            OutputText("Scenes.Minervapurification.VisitTowerPostPurification.GoingSearchScaleMountain".Localized());

            if (chooser == 0)
                OutputText("Scenes.Minervapurification.VisitTowerPostPurification.WalkAlongTrailSomething".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerRepaired) <= 0)
            {
                OutputText("Scenes.Minervapurification.VisitTowerPostPurification.FamiliarRouteUnchangedSwea".Localized());

                OutputText("Scenes.Minervapurification.VisitTowerPostPurification.PushDoorLookAround".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinervaTowerRepaired, 1);
            }
            else
                OutputText("Scenes.Minervapurification.VisitTowerPostPurification.OnceAgainArriveTower".Localized());

            switch (chooser)
            {
                case 0:
                    {
                        MinervaSinging();
                    }

                    break;

                case 1:
                    {
                        MinervaBathing();
                    }

                    break;

                case 2:
                    {
                        MinervaRelaxing();
                    }

                    break;

                case 3:
                    {
                        MinervaBabiesMeet();
                    }

                    break;

                case 4:
                    {
                        MinervaPregnancyMeet();
                    }

                    break;
            }

            if (true)
                OutputText("Scenes.Minervapurification.VisitTowerPostPurification.TextShouldntHappen".Localized());

            PureMinervaMenu();
        }

        public void MinervaSinging()
        {

            OutputText("Scenes.Minervapurification.MinervaSinging.SongEchoesBewitchingPitch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
            {
                OutputText("Scenes.Minervapurification.MinervaSinging.GoldSkinnedPerchedTree".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) >= 2)
                {
                    OutputText("Scenes.Minervapurification.MinervaSinging.SightOfSirens".Localized(
                         NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren))
                    ));

                    var sceneChooser = Random.Shared.Next(3);

                    if (sceneChooser == 0)
                        OutputText("Scenes.Minervapurification.MinervaSinging.PerchedInTree".Localized());
                    else
                        if (sceneChooser == 1)
                            OutputText("Scenes.Minervapurification.MinervaSinging.SplashingInPool".Localized());
                        else
                            OutputText("Scenes.Minervapurification.MinervaSinging.CurledUpAgainstRoots".Localized());
                }

                OutputText("Scenes.Minervapurification.MinervaSinging.HighBranchDanglingLegs".Localized());

                OutputText("Scenes.Minervapurification.MinervaSinging.SongEndsLooksSharky".Localized());
            }
            else
            {
                OutputText("Scenes.Minervapurification.MinervaSinging.MotherlyRelaxingMossyShores".Localized());

                OutputText("Scenes.Minervapurification.MinervaSinging.GoldenSirenClosedSinging".Localized());

                OutputText("Scenes.Minervapurification.MinervaSinging.SongWindsFlutterOpen".Localized());
            }
        }

        public void MinervaBathing()
        {

            OutputText("Scenes.Minervapurification.MinervaBathing.LoverBusyWantsBath".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0) && (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) >= 2))
            {
                OutputText("Scenes.Minervapurification.MinervaBathing.SightOfSirens".Localized(
                     NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren))
                ));

                var sceneChooser = Random.Shared.Next(3);

                if (sceneChooser == 0)
                    OutputText("Scenes.Minervapurification.MinervaBathing.PerchedInTree".Localized());
                else
                    if (sceneChooser == 1)
                        OutputText("Scenes.Minervapurification.MinervaBathing.SplashingInPool".Localized());
                    else
                        OutputText("Scenes.Minervapurification.MinervaBathing.CurledUpAgainstRoots".Localized());
            }

            OutputText("Scenes.Minervapurification.MinervaBathing.LookingOverSheBathes".Localized());

            if (Player.Pregnancy.Type.Path == "none")
                OutputText("Scenes.Minervapurification.MinervaBathing.SlippingDownSwollenTummy".Localized());

            OutputText("Scenes.Minervapurification.MinervaBathing.WaterSlidesDownBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervapurification.MinervaBathing.WillingFertileFemaleDuty".Localized());

                if (!Player.HasVagina())
                    OutputText("Scenes.Minervapurification.MinervaBathing.Male".Localized());
                else
                    OutputText("Scenes.Minervapurification.MinervaBathing.Herm".Localized());

                OutputText("Scenes.Minervapurification.MinervaBathing.BearYourOffspring".Localized());
            }

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Minervapurification.MinervaBathing.Also".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.MinervaBathing.VirilePleasurableBreeder".Localized());

            OutputText("Scenes.Minervapurification.MinervaBathing.GoldenEyeCatchesYours".Localized());
        }

        public void MinervaRelaxing()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
            {
                OutputText("Scenes.Minervapurification.MinervaRelaxing.PurifiedNeverLookedLovely".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) >= 2)
                {
                    OutputText("Scenes.Minervapurification.MinervaRelaxing.SightOfSirens".Localized(
                         NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren))
                    ));

                    var sceneChooser = Random.Shared.Next(3);

                    if (sceneChooser == 0)
                        OutputText("Scenes.Minervapurification.MinervaRelaxing.PerchedInTree".Localized());
                    else
                        if (sceneChooser == 1)
                            OutputText("Scenes.Minervapurification.MinervaRelaxing.SplashingInPool".Localized());
                        else
                            OutputText("Scenes.Minervapurification.MinervaRelaxing.CurledUpAgainstRoots".Localized());
                }

                OutputText("Scenes.Minervapurification.MinervaRelaxing.GrinningApproachEdgeWade".Localized());
            }
            else
            {
                OutputText("Scenes.Minervapurification.MinervaRelaxing.OnceAgainUponSpring".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                {
                    OutputText("Scenes.Minervapurification.MinervaRelaxing.LookingAroundYoungSirens".Localized((CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) == 1 ? "is" : "are"), NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren))));

                    switch (Random.Shared.Next(3))
                    {
                        case 0:
                            {
                                OutputText("Scenes.Minervapurification.MinervaRelaxing.PerchedInFruitTrees".Localized());
                            }

                            break;

                        case 1:
                            {
                                OutputText("Scenes.Minervapurification.MinervaRelaxing.SplashingInPureSpring".Localized());
                            }

                            break;

                        case 2:
                            {
                                OutputText("Scenes.Minervapurification.MinervaRelaxing.CurledAroundMotherFather".Localized());
                            }

                            break;
                    }

                    if (true)
                        OutputText("Scenes.Minervapurification.MinervaRelaxing.ErrorShouldNotAppear".Localized());

                    OutputText("Scenes.Minervapurification.MinervaRelaxing.SentencePeriod".Localized());
                }

                OutputText("Scenes.Minervapurification.MinervaRelaxing.DevilishIntentCreepSleeping".Localized());
            }

            OutputText("Scenes.Minervapurification.MinervaRelaxing.NotHappySurprisedVisit".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) <= 0)
                OutputText("Scenes.Minervapurification.MinervaRelaxing.HavingSomeBabies".Localized());
            else
                OutputText("Scenes.Minervapurification.MinervaRelaxing.MakingMoreAdorableDaughters".Localized());

            OutputText("Scenes.Minervapurification.MinervaRelaxing.CroonsLicksCheek".Localized());
        }

        public void MinervaPregnancyMeet()
        {

            OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.ParagraphBreak".Localized());

            if (SceneInstance<MinervaScene>.Get().Pregnancy.IsPregnant)
            {
                if (Player.Pregnancy.Type == PregnancyIds.Minerva)
                {
                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.SmilingStrokeSwollenBelly".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                        OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.AgainstHugeOakTree".Localized());
                    else
                        OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.AgainstSmellingFruitTree".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.ApproachNoticeLittleDifferent".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.HowCouldTurnOffer".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                        OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.StepIntoWaterWade".Localized());
                    else
                        OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.StrideTowardBroodmother".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.EcstaticReachesOut".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                        OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.VirileLoverFillingBelly".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.GreatestGiftDaughters".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.LayAgainstTreeCuddling".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                        OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.DaughtersCurledUpHappy".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.AfterSomeTimeSpeaks".Localized());
                }
                else
                {
                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.CantHelpWonderingChanged".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.SceneMakesSmilePride".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.FeelingStrokingLittleKick".Localized());
                }
            }
            else
                if (Player.Pregnancy.Type == PregnancyIds.Minerva)
                {
                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.SmilingStrokeSwollenAgain".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.SmileTurnsWhisperUnborn".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.TenderMotherlyNuzzlesStomach".Localized());
                }
                else
                {
                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.StrokeBellyWondering".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.SmileTurnsMischiefCreeping".Localized());

                    OutputText("Scenes.Minervapurification.MinervaPregnancyMeet.LooksSwollenStomachRubbing".Localized());
                }
        }

        public void MinervaBabiesMeet()
        {

            var temp = Random.Shared.Next(2);

            switch (temp)
            {
                case 0:
                    {
                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.FunTimeYoungDaughters".Localized());

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.NoticeYouFlyTackle".Localized(
                             PlayerParentDesc(true)
                        ));

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.ChucklesSashaysOverPile".Localized(
                             PlayerParentDesc(false)
                        ));

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.PatHeadsPickUp".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.FunYoungDaughtersAgain".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) < 4)
                            OutputText("Scenes.Minervapurification.MinervaBabiesMeet.HoldsTwoDaughtersShoulders".Localized());
                        else
                            OutputText("Scenes.Minervapurification.MinervaBabiesMeet.HoldsOneDaughterShoulder".Localized());

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.SmilingApproachTowerSirens".Localized(
                             PlayerParentDesc(true)
                        ));

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.StruggleDaughtersSnickers".Localized(
                             PlayerParentDesc(true)
                        ));

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.YoungHermsGatherPresenting".Localized());

                        OutputText("Scenes.Minervapurification.MinervaBabiesMeet.SendDaughtersOffPat".Localized());

                        Player.Gems += 1;

                    }

                    break;
            }

            if (true)
                OutputText("Scenes.Minervapurification.MinervaBabiesMeet.TextShouldNotHappen".Localized());
        }

        public void PureMinervaMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), PureMinervaAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), MinervaTalkSelect);

            if (Player.Lust >= 33)
                AddButton(2, "Common.Button.Sex".Localized(), () => this.PureMinervaSexMenu());
            else
                AddButton(2, "Common.Button.Sex".Localized(), null);

            AddButton(3, "Scenes.Faerie.SexMenu.ButtonEat".Localized(), SceneInstance<MinervaScene>.Get().EatSomethingYouCunt);

            AddButton(4, "Scenes.Lumberjack.Button.Drink".Localized(), SceneInstance<MinervaScene>.Get().GetADrinkYouBitch);

            AddButton(5, "Common.Button.Spar".Localized(), SceneInstance<MinervaScene>.Get().FightMinerva);

            if (SceneInstance<MinervaScene>.Get().MinervaRomanced() && View.Time.Hours >= 20)
                AddButton(6, "Common.Button.SleepWith".Localized(), SleepWithMinerva);
            else
                AddButton(6, "Common.Button.SleepWith".Localized(), null);

            if (SceneInstance<MinervaScene>.Get().Pregnancy.IsPregnant)
                AddButton(7, "Common.Button.Pregnancy".Localized(), CheckPregnancy);

            if (Player.HasKeyItem("Marae's Seed"))
                AddButton(8, "Common.Button.PlantSeed".Localized(), GrowTreePostPurification);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void PureMinervaAppearance()
        {
            ClearOutput();

            ShowImage("minerva-appearance-pure");

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.PurifiedSirenEightFive".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.LargeGoldenWhiteWings".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.FeatherCoveredArmsHarpy".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.WideHipsWaistWaspish".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.BetweenTonedLegsOrgans".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.LargeDemonicFeet".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaAppearance.WhiteTubeTopShorts".Localized());

            ClearButtons();

            var minervaPregnancy = SceneInstance<MinervaScene>.Get().Pregnancy;

            if (minervaPregnancy.IsPregnant)
            {
                OutputText("Scenes.Minervapurification.PureMinervaAppearance.ParagraphBreak".Localized());

                var incubation = minervaPregnancy.Incubation;

                if (incubation >= 216)
                    OutputText("Scenes.Minervapurification.PureMinervaAppearance.BellyBitSwollen".Localized());
                else if (incubation >= 144 && incubation < 216)
                    OutputText("Scenes.Minervapurification.PureMinervaAppearance.BellySignsOfPregnancy".Localized());
                else if (incubation >= 72 && incubation < 144)
                    OutputText("Scenes.Minervapurification.PureMinervaAppearance.GravidBellyGottenBigger".Localized());
                else if (incubation < 72)
                    OutputText("Scenes.Minervapurification.PureMinervaAppearance.ImpossibleNotNoticePregnancy".Localized());
            }

            DoNext(() => PureMinervaMenu());
        }

        public void MinervaTalkSelect()
        {

            var talkChooser = Random.Shared.Next(5);

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                talkChooser = Random.Shared.Next(6);

            if (talkChooser == 0)
                TalkingToMinervaAboutBackstory();
            else
                if (talkChooser == 1)
                    TalkToMinervaAboutSpring();
                else
                    if (talkChooser == 2)
                        TalkToMinervaAboutSharkgirl();
                    else
                        if (talkChooser == 3)
                            TalkToMinervaAboutMotherhood();
                        else
                            if (talkChooser == 4)
                                SceneInstance<MinervaScene>.Get().BathTimeTalkWithMinerva();
                            else
                                if (talkChooser == 5)
                                    TalkToMinervaAboutTree();
        }

        public void TalkingToMinervaAboutBackstory()
        {
            ClearOutput();

            ShowImage("minerva-talk-backstory-pure");

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstory) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstory, 1);

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.InformingWantToTalk".Localized());

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.AskingAboutPastCuriously".Localized());

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.TakeTopicAwayScar".Localized());

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.LooksDeepInThought".Localized());

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.EnoughForNowGiggle".Localized());

                if (Player.Pregnancy.Type.Path == "none")
                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.BabiesNeedSeeDaddy".Localized());

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SayEnd".Localized());

                OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SayingGoodbyeKiss".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaBackstory) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstory, 2);

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.InformingWantTalkAgain".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.RedHeadedBeganStory".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.AnywayFoodInlandWorse".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.BitLipForceFed".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.TigersharkDidntLike".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SmilesPutsStruggledAway".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SeemedToSmileSadly".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.ReminiscingLostSharkGirl".Localized());
                }
                else
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstory, 0);

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.InformingWantTalkThird".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.RedheadedBeganStoryAgain".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.MomentReminiscingSighsHappily".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.LooksDownPreteenSize".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SilentThinkingDaughter".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SaysBeforeOpeningTongue".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.ShakingWhatTheyPut".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.WingsHungryDugIn".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.AskedSeriouslyFlownHours".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.SaysBeforeLookingYou".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.ReachingOverHugGently".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                        OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.BabiesJustAsImportant".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.GenuineSmileWonderfulLover".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.CuddlingReluctantlyPullBack".Localized());

                    OutputText("Scenes.Minervapurification.TalkingToMinervaAboutBackstory.PeachHandBlowKiss".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MinervaBackstoryLearned, 1);

                    Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

                    InventoryScene.TakeItem(ItemIds.PurePeach, () => ReturnToCamp(1));

                    return;
                }

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            ReturnToCamp(1);
        }

        public void TalkToMinervaAboutSpring()
        {
            ClearOutput();

            ShowImage("minerva-talk-spring-pure");

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.InformingWantToTalk".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.QuestionsLookingOverSpring".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.MaraeNexusPureEnergies".Localized());
            else
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.JojoToldWellspringHoly".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.KnowPropertiesCorruption".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.GrinBeforeDontKnow".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.ThinkingDecideNothingDip".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.WaterNotFourFeet".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.WomanlyBodyDrenched".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSpring.GrinningBackReachSpins".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            Player.ChangeFatigue(-10);

            AddButton(0, "Common.Button.Yes".Localized(), () => this.PureMinervaSexMenu());

            AddButton(1, "Common.Button.No".Localized(), DeclineSpringSex);
        }

        public void DeclineSpringSex()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.DeclineSpringSex.StayingLikeThisRest".Localized());

            OutputText("Scenes.Minervapurification.DeclineSpringSex.SmilingPromiseVisitSoon".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToMinervaAboutSharkgirl()
        {
            ClearOutput();

            ShowImage("minerva-talk-sharkgirl-pure");

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.InformingWantToTalk".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.EyesWideningFirstborn".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.SureYouveNoticed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.ByOurOwnDaughters".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.ThingsGrowUpQuickly".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.SadnessBleedingOut".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.LookingAtOasis".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.ShakingHeadOwnFault".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.FinalSighHug".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutSharkgirl.StayLikeSpendTime".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToMinervaAboutMotherhood()
        {
            ClearOutput();

            ShowImage("minerva-talk-motherhood-pure");

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.TellingWantToTalk".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.HappyToListenLeans".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.WeavingStorySixMinotaurs".Localized());

                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.FoesBeatenSavedDamsels".Localized());

                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.RollingIntoAnotherStory".Localized());

                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.AttackSuccessfulGoopyMess".Localized());
            }

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.TaleOverAudienceClaps".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) <= 0)
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.WishCouldHaveKids".Localized());
            else
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.NoLongerEnvyThem".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.ShakingHeadGrinsShuffle".Localized());

            if (!Player.Pregnancy.IsActive)
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.HugTightlyKissCheek".Localized());
            else
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.HandShoulderComfortingSmile".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.WordsSmileBlackLips".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutMotherhood.SeesLateGetsUp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToMinervaAboutTree()
        {
            ClearOutput();

            ShowImage("minerva-talk-tree-pure");

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.TellingTalkBrightSmile".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.CuriousStepIntoPool".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.AskedStrokingChattingLot".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.IndeedWeHaveMinerva".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.AndYourDaughters".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.SourceOfMuchEntertainment".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
            {
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.LookedToBroodmotherNoticed".Localized());

                OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.RepliesWithNodSisterly".Localized());
            }

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.GoodKnowWontLonely".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.AsWellTendDaughters".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.SentencePeriod".Localized());

            OutputText("Scenes.Minervapurification.TalkToMinervaAboutTree.MayEvenComeTalk".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SleepWithMinerva()
        {
            ClearOutput();

            ShowImage("minerva-sleepwith-pure");

            OutputText("Scenes.Minervapurification.SleepWithMinerva.WalkOverWantSleep".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                OutputText("Scenes.Minervapurification.SleepWithMinerva.GuidesComfortableSpotOak".Localized());
            else
                OutputText("Scenes.Minervapurification.SleepWithMinerva.GuidesComfortableTakesOff".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Minervapurification.SleepWithMinerva.TakeTimePlatebodyOff".Localized());
            else
                OutputText("Scenes.Minervapurification.SleepWithMinerva.TakeTimeArmorOff".Localized());

            OutputText("Scenes.Minervapurification.SleepWithMinerva.LayRestHeadBreasts".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Minervapurification.SleepWithMinerva.ValeriaEnvelopesKeepsWarm".Localized());

            OutputText("Scenes.Minervapurification.SleepWithMinerva.SweetDreamsCloseEyes".Localized());

            this.SleepWithMinervaProcess();
        }

        public void SleepWithMinervaII()
        {
            ClearOutput();

            ShowImage("minerva-sleepwith2-pure");

            OutputText("Scenes.Minervapurification.SleepWithMinervaII.TenHoursPassBold".Localized());

            OutputText("Scenes.Minervapurification.SleepWithMinervaII.WakeRefreshedThankHer".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Minervapurification.SleepWithMinervaII.ValeriaEncasesOnceMore".Localized());
            else
                OutputText("Scenes.Minervapurification.SleepWithMinervaII.GetReDressed".Localized());

            OutputText("Scenes.Minervapurification.SleepWithMinervaII.LeaveTowerReturnCamp".Localized());

            AwardAchievement(AchievementIds.Getaway);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -4f));

            this.SleepWithMinervaHeal();

            ReturnToCamp(1);
        }

        public void SleepWithMinervaPostBirthing()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.SleepWithMinervaPostBirthing.ThreeHoursPassBold".Localized());

            OutputText("Scenes.Minervapurification.SleepWithMinervaPostBirthing.WakeRefreshedLookDaughters".Localized());

            OutputText("Scenes.Minervapurification.SleepWithMinervaPostBirthing.ExcitedYellSetDown".Localized(
                Player.Mf("Daddy!", "Mommy!")
            ));

            OutputText("Scenes.Minervapurification.SleepWithMinervaPostBirthing.ComeBackTimeKiss".Localized());

            AwardAchievement(AchievementIds.Getaway);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -4f));

            this.SleepWithMinervaHeal();

            DoNext(() => ReturnToCamp(1));
        }

        public void SleepWithMinervaProcess()
        {
            ClearOutput();

            Player.Sleeping = true;

            View.Time.PassTime(10);

            SleepWithMinervaII();
        }

        public void SleepWithMinervaHeal()
        {
            CampScene.SleepRecovery(true);
        }

        public void PureMinervaSexMenu()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Minervapurification.PureMinervaSexMenu.GrinsSlidesAlwaysReady".Localized());

            if (Player.HasCock())
                OutputText(Player.CockDescript());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Minervapurification.PureMinervaSexMenu.And".Localized());

            if (Player.HasVagina())
                OutputText(Player.VaginaDescript());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Minervapurification.PureMinervaSexMenu.Groin".Localized());

            OutputText("Scenes.Minervapurification.PureMinervaSexMenu.SoDarlingWantIt".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.PureMinervaSexMenu.StuffCuntProdAss".Localized());

            AddButton(0, "Common.Button.Fuckherbutt".Localized(), null);

            AddButton(1, "Common.Button.Fuckcowgirl".Localized(), null);

            AddButton(2, "Common.Button.Restrainfuck".Localized(), null);

            AddButton(5, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), null);

            if (Player.HasCockThatFits(0))
                AddButton(0, "Common.Button.Fuckherbutt".Localized(), FuckMinervasAsshole);

            if (Player.HasCockThatFits(0))
            {
                AddButton(1, "Common.Button.Fuckcowgirl".Localized(), MinervaCowgirlSex);

                AddButton(2, "Common.Button.Restrainfuck".Localized(), FuckMinervaWithHerHandsBehindHerBack);
            }

            AddButton(3, "Common.Button.Takeherdick".Localized(), ChooseVagOrAss);

            AddButton(4, "Common.Button.EatHerOut".Localized(), GoDownOnAHermAndLoveItYouDirtySlutYou);

            if (Player.HasCock())
                AddButton(5, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), LetMinervaSuckYouOff);

            AddButton(14, "Common.Button.Leave".Localized(), PureMinervaMenu);
        }

        public void FuckMinervasAsshole()
        {
            ClearOutput();

            var x = Player.CockThatFits(0);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            ShowImage("minerva-sex-analpitch-pure");

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.SmileHandThighPushing".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.SmirkForwardSqueezeAss".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.NotReadyTeaseMore".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.NoSenseKeepingWaiting".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.AndYour".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.ToOpenAir".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.BareAssRubbingCoax".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.Shafts".Localized());
            else
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.Shaft".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.ToErectnessThroughCrack".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.TeasedEnoughDragDown".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x)
            ));

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.DrawHipsSinkFingers".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.BitesPushesHipsBack".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.DrawDragInchInch".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.IncreasedPaceBrutalThrusts".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.CantAnymoreUnleashSeed".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.KeepGushingUntilSpent".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.UnnaturalOrgasmKeepsPouring".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.FreakishOrgasmWithoutEnd".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.PantingAssFuckingSlumps".Localized());
            else
                if (Player.CumQ() < 5000)
                    OutputText("Scenes.Minervapurification.FuckMinervasAsshole.GroaningFilledSwollenBelly".Localized());
                else
                    OutputText("Scenes.Minervapurification.FuckMinervasAsshole.GaspingGurglingHoldsPond".Localized(
                         Player.CockDescript(x)
                    ));

            OutputText("Scenes.Minervapurification.FuckMinervasAsshole.AllowAgainstTonedBody".Localized());

            if (!Player.Pregnancy.IsActive)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedMinerva) == 0)
                    OutputText("Scenes.Minervapurification.FuckMinervasAsshole.SeemsLittleSadLeaving".Localized());
                else
                    OutputText("Scenes.Minervapurification.FuckMinervasAsshole.DisappointedSighLonelySiren".Localized());
            }
            else
                OutputText("Scenes.Minervapurification.FuckMinervasAsshole.LovestruckSighsSitsUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesButtfuckedMinerva, CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedMinerva) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void MinervaCowgirlSex()
        {
            ClearOutput();

            ShowImage("minerva-sex-cowgirl-pure");

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.LookAttractiveSirenDown".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.TrepidationGivenSizes".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.ReachSpankCushionedAss".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) <= 0)
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.LooksUnsureBitingLip".Localized());
            else
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.SmilesBrightlyAssPulls".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.AmorousGrinRemoveClothes".Localized(
                 Player.CockDescript(),

                 Player.CockDescript()
            ));

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.GroanCoursingSurgesErect".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.LikesHelplessTentacledDick".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.MoanDelightShiveringSensations".Localized());

            if (Player.Tallness < 72)
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.AndSuperiorSize".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.KeepPinnedUnderHer".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.MoanShudderPoisonsBoil".Localized(
                 Player.CockDescript()
            ));

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.BegMercyCantBear".Localized());

            if (Player.Pregnancy.Type.Path == "none")
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.WantFeelFillMe".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.SayEnd".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.PleadBeggingLetCum".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.BallsAre".Localized());
            else
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.CockIs".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.GoingBurstAcheRelief".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.IdleThoughtBittenMore".Localized());

            if (Player.Pregnancy.Type.Path == "none")
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.ReallyTryingPregnantAgain".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.HeavingGaspsWhatMind".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.FewSlitherDownSting".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.GaspWrithePleadingLook".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.DontKnowCouldHold".Localized((CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0 ? "more " : "")));

            if (Player.Pregnancy.Type.Path == "none")
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.SaysPlayfulNeverUsed".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.FatiguedGroanSatisfiedSigh".Localized());

            if (Player.Pregnancy.Type.Path == "none")
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.MassiveTummyPressingSnuggle".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.AllowWarmBodyRecovering".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.DisappointedSighImportantJob".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                OutputText("Scenes.Minervapurification.MinervaCowgirlSex.GirlsWouldLoveYou".Localized());

            OutputText("Scenes.Minervapurification.MinervaCowgirlSex.SureLookingForwardVisit".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ReturnToCamp(1);
        }

        public void FuckMinervaWithHerHandsBehindHerBack()
        {
            ClearOutput();

            var x = Player.CockThatFits(0);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            ShowImage("minerva-sex-restrain-pure");

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.LookingLeanTonedHerm".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.PosingEasyGetLook".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.ReachingBackAroundLeans".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.EagerlyShakingHipsJustice".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.NotReadyContinueTeasing".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.HandsHitAgainAgain".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.SoPerfectTimeStrike".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.BrutalThrustSlideEase".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.ContinuePoundBallsSwing".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.QuiveringFormPatRump".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.SoReadyDevilishPurpose".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.GruntingFloodingThrustHarder".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.BodyFilledRenewThrusts".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.NewPaceAnimalisticRutting".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.NeedExplosiveEndArches".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.BodyStimulatedPotent".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.GroansDelightSeedBursts".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.BlissfulReleaseSurges".Localized());
                else
                {
                    OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.InhumanLoadFillsBelly".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.GroansMindlessGurgles".Localized());

                    OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.EvenAmazementWombContain".Localized());

                    OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.LookingGravidOverstuffedSits".Localized());
                }

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.LettingGoEaseRest".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.AnotherSighLetsTail".Localized());

            OutputText("Scenes.Minervapurification.FuckMinervaWithHerHandsBehindHerBack.AmusedRumpHeadingOut".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void ChooseVagOrAss()
        {
            ClearOutput();

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minervapurification.ChooseVagOrAss.SirenAsksWhichHole".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Vagina".Localized(), MinervaLapSex);

                AddButton(1, "Common.Button.Ass".Localized(), GetButtFuckedYouSlut);
            }
            else
                GetButtFuckedYouSlut();
        }

        public void MinervaLapSex()
        {
            ClearOutput();

            var x = Player.CockThatFits(0);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            ShowImage("minerva-sex-lapsex-pure");

            OutputText("Scenes.Minervapurification.MinervaLapSex.ThinkingKindOfFun".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.WithoutSecondThoughtArm".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.GentlyPushingSlidesHands".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.SaysPlayfullyClothesOff".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.AboutToAskSquirming".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.HandsSlideGropeAss".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minervapurification.MinervaLapSex.OneHandTrailHerm".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervapurification.MinervaLapSex.HandsGropeChestNipples".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.NoddingDazedDisentangle".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.ShiftingHipsTeaseTentacled".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.GroanCantAnymoreDown".Localized());

            if (Player.Vaginas.Count == 0 && Player.Vaginas[0].Virgin)
                OutputText("Scenes.Minervapurification.MinervaLapSex.ThickPrickPlungesCunt".Localized());
            else
                OutputText("Scenes.Minervapurification.MinervaLapSex.ThickPrickPlungesAgain".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.BasksBallsDeepRevel".Localized());

            Player.CuntChange(32, true, true, false);

            OutputText("Scenes.Minervapurification.MinervaLapSex.GaspSuddenTightnessTeeth".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.GrinningNibblesEarLicks".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.DoesntTakeLongFlesh".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.EightGruntsKicksPace".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.SaysGoodHowLike".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.FeelingStuffedThickFluid".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervapurification.MinervaLapSex.YourOwnThrobbingPhallus".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minervapurification.MinervaLapSex.Es".Localized());

                OutputText("Scenes.Minervapurification.MinervaLapSex.CummingAlongside".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Minervapurification.MinervaLapSex.GushingCumOverBellies".Localized());
                else
                    if (Player.CumQ() < 4000)
                        OutputText("Scenes.Minervapurification.MinervaLapSex.SplatteringThickSplooge".Localized());
                    else
                        OutputText("Scenes.Minervapurification.MinervaLapSex.ExplodingOntoBodiesRopes".Localized());
            }

            OutputText("Scenes.Minervapurification.MinervaLapSex.SentencePeriod".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.SeedFillsQuiteFull".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.PantingCollapseSweatyHeap".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.ReachingTellNotWorry".Localized());

            OutputText("Scenes.Minervapurification.MinervaLapSex.AllowSoftBodyRecovering".Localized());

            if (!Player.Pregnancy.IsActive)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) == 0)
                    OutputText("Scenes.Minervapurification.MinervaLapSex.SeemsLittleSadLeaving".Localized());
                else
                    OutputText("Scenes.Minervapurification.MinervaLapSex.DisappointedSighLonelySiren".Localized());
            }
            else
                OutputText("Scenes.Minervapurification.MinervaLapSex.LovestruckSighsSitsUp".Localized());

            if (Random.Shared.Next(100) <= Player.TotalFertility())
                Player.KnockUp(PregnancyIds.Minerva, PregnancyIds.MinervaIncubation);

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaLapsexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) + 1);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void GetButtFuckedYouSlut()
        {
            ClearOutput();

            var x = Player.CockThatFits(0);

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            ShowImage("minerva-sex-analcatch-pure");

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.ThinkingKindOfFun".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.WithoutSecondThoughtArm".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.GentlyPushingSlidesHands".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.SaysPlayfullyClothesOff".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.AboutToAskSquirming".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.HandsSlideGropeAss".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.OneTrailHermVagina".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.HandsGropeChestNipples".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.NoddingDazedDisentangle".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.ShiftingHipsTeaseTentacled".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.GroanCantAnymoreDown".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.ThickPrickPlungesBackdoor".Localized());

            Player.ButtChange(32, true);

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.BasksBallsDeepRevel".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.GaspSuddenTightnessTeeth".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.GrinningNibblesEarLicks".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.DoesntTakeLongFlesh".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.EightGruntsKicksPace".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.SaysGoodHowLike".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.FeelingStuffedThickFluid".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.YourOwnCunt".Localized());

                if (Player.AverageVaginalWetness() < 4)
                    OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.LeakingFemspunk".Localized());
                else
                    OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.ShootingFemspunk".Localized());
            }

            if (Player.HasVagina() && Player.HasCock())
                OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.While".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.YourOwnThrobbingPhallus".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.Es".Localized());

                OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.Cumming".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.GushingCumOverBellies".Localized());
                else
                    if (Player.CumQ() < 4000)
                        OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.SplatteringThickSplooge".Localized());
                    else
                        OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.ExplodingOntoBodiesRopes".Localized());
            }

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.SentencePeriod".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.SeedFillsQuiteFull".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.PantingCollapseSweatyHeap".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.ReachingTellNotWorry".Localized());

            OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.AllowSoftBodyRecovering".Localized());

            if (!Player.Pregnancy.IsActive)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) == 0)
                    OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.SeemsLittleSadLeaving".Localized());
                else
                    OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.DisappointedSighLonelySiren".Localized());
            }
            else
                OutputText("Scenes.Minervapurification.GetButtFuckedYouSlut.LovestruckSighsSitsUp".Localized());

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaLapsexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaLapsexed) + 1);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void GoDownOnAHermAndLoveItYouDirtySlutYou()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            ShowImage("minerva-sex-eatout-pure");

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.ThinkingKindOfFun".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.LickingLipsAnticipation".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.LookingUpRearBlush".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.GetBetweenPushHips".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.AsksDoingBackThere".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.TastingGoodHoldHips".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.NotWantingUnattendedDick".Localized());

            if (Player.Tallness < 96)
                OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.FingersNotFitGirth".Localized());
            else
                OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.FingersBarelyWrappingMember".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.StrokeSlowlyAddingPleasure".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.ReadyAttentionDiveIn".Localized());

            if (Player.HasLongTongue())
                OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.LongTongueSlithersDeeper".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.VaginalKissWorkingWonders".Localized());

            OutputText("Scenes.Minervapurification.GoDownOnAHermAndLoveItYouDirtySlutYou.NotCumPullBack".Localized());

            ClearButtons();

            AddButton(0, "Scenes.PlayerAppearance.Label.Balls".Localized(), LickMinervasBalls);

            AddButton(1, "Common.Button.NoBalls".Localized(), () => NoBallsMinervaAndContinuation());
        }

        public void LickMinervasBalls()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.LickMinervasBalls.NotCloseTeaseMore".Localized());

            NoBallsMinervaAndContinuation(false);
        }

        public void NoBallsMinervaAndContinuation(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.ParagraphBreak".Localized());

            ShowImage("minerva-sex-eatout2-pure");

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.CarefulMinistrationsSquirming".Localized());

            if (!clear)
                OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.RetreatingFromBallsYou".Localized());
            else
                OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.You".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.MoveBackSoakingSnatch".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.MusicalCryTailTightens".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.BetweenSqueezedCockPulses".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.ShudderingLyricalCriesEcho".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.IntenseCrescendoPantingSweaty".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.PerverselyPrehensileRetracting".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.SentencePeriod".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.ReleasedFromGraspSlumps".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.ArmsWrapHugsCuddle".Localized());

            OutputText("Scenes.Minervapurification.NoBallsMinervaAndContinuation.LongWhileLayCuddled".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void LetMinervaSuckYouOff()
        {
            ClearOutput();

            var x = Player.CockThatFits(0);

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("minerva-sex-blowjob-pure");

            CoC.Instance.Flags.Set(GameFlagIds.TimesMinervaSexed, CoC.Instance.Flags.Get(GameFlagIds.TimesMinervaSexed) + 1);

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.ThinkingKindOfFun".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.GrinningDecideWantEyes".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.UnsureLookWantMe".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.LoungeAgainstSpreadSlit".Localized(
                     Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.SitSpreadOpenCrotch".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.AnxiousReadyKneelsHand".Localized(
                 Player.CockDescript(x)
            ));

            if (!(Player.CockArea(x) < 21))
                if (Player.CockArea(x) < 34)
                    OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.EyesWidenDickGrows".Localized());
                else
                    if (Player.CockArea(x) < 60)
                        OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.EyesWidenGrowsLarge".Localized());
                    else
                        OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.EyesMouthOpensShock".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.RelaxingBreathTongueSlithering".Localized(
                 Player.CockDescript(x),

                 Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.HandHipMovesCaress".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.HoldingSlidesTongueOut".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.LickingTeasingBalls".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.SentencePeriod".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.FirmlyCapturedLeansOpens".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.PuckeringGlossyBobsHead".Localized());

            if (Player.CockArea(x) >= 50)
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.HugeCockThroatDifficulty".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.SentencePeriodAgain".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.SlurpingLickingPackageHead".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.TeasingSuckingLeavesBreathless".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.ExtendingTongueFurther".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.PantingCantHoldOut".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.BallsLurchSwellClimactic".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.SentencePeriodThird".Localized());

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.GrippingLongMoanBloating".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() < 500)
                OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.OrgasmLicksEveryDrop".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.OrgasmLicksDropAgain".Localized());
                else
                    if (Player.CumQ() < 4000)
                        OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.OrgasmSuddenlyEveryDrop".Localized());
                    else
                        OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.OrgasmSuddenlyDropAgain".Localized(
                             Player.Mf("male", "herm")
                        ));

            OutputText("Scenes.Minervapurification.LetMinervaSuckYouOff.DecideRelaxCurlMoss".Localized());

            Player.Orgasm("Dick");

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void CheckPregnancy()
        {
            var incubation = SceneInstance<MinervaScene>.Get().Pregnancy.Incubation;

            if (incubation >= 216)
                PregnancyStage1(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0);
            else if (incubation >= 144 && incubation < 216)
                PregnancyStage2(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0);
            else if (incubation >= 72 && incubation < 144)
                PregnancyStage3(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0);
            else if (incubation < 72)
                PregnancyStage4(CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0);
        }

        public void PregnancyStage1(bool repeat = false)
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PregnancyStage1.NoticeableBabyBump".Localized());

            if (!repeat)
            {
                OutputText("Scenes.Minervapurification.PregnancyStage1.ArmsOutstretchedBowlsGlopping".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage1.GripTightGoingNowhere".Localized());
            }
            else
            {
                OutputText("Scenes.Minervapurification.PregnancyStage1.SmoothCaringSlideDown".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage1.SoulfulWordsShiningStar".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage1.MaternalHormonesCuddlingSpur".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), PureMinervaMenu);

                AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Minervapurification.PregnancyStage1.FinalKissDisentangling".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PregnancyStage2(bool repeat = false)
        {
            ClearOutput();

            if (!repeat)
            {
                OutputText("Scenes.Minervapurification.PregnancyStage2.EcstaticHappierThanBefore".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.LastVisitSureTackled".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.AcheAirKissBreaks".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.OnceAgainBackClinging".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.NearlyHourPassesEmbrace".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.FinalKissDisentangling".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }
            else
            {
                OutputText("Scenes.Minervapurification.PregnancyStage2.PregnancySwiftBellySwells".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.GrinningComeToHer".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.FinallyReleasingPullsDown".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.FertileMilkyMelonsDisplay".Localized());

                Player.RefillHunger(10);

                OutputText("Scenes.Minervapurification.PregnancyStage2.LostMaternalEmbraceStroking".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage2.SeeingArousedLeaveGrip".Localized());

                PureMinervaSexMenu();

                return;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void PregnancyStage3(bool repeat = false)
        {
            ClearOutput();

            if (!repeat)
            {
                OutputText("Scenes.Minervapurification.PregnancyStage3.AdmiringGrowingPregnancyGet".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.GazeBellyCalmsCradling".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.TearsTightensHoldBuries".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.SlippingCradleHeavilyPregnant".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.PairCuddledKissingRubbing".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                    OutputText("Scenes.Minervapurification.PregnancyStage3.HugeTreeSitsBack".Localized());
                else
                    OutputText("Scenes.Minervapurification.PregnancyStage3.EverGrowingFruitLay".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.ComfortablePositionCradled".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.MovingDownTenderKisses".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.MiraculouslyTouchRelaxesBabies".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.NearlySleepYawningAwaken".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }
            else
            {
                OutputText("Scenes.Minervapurification.PregnancyStage3.ShowingInterestPleasesHormonal".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.GrinningSharkyStridesHips".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.HornySaysTakeSpin".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.BreederInstinctsAffections".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.TrademarkSharkyGrinApproaches".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.AgonizingSlownessTongueLicks".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.NotSettingPullsBack".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.StimulationEmbraceBreasts".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.SlipperySlideBackSucking".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.GaspPumpingHipsBreasts".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minervapurification.PregnancyStage3.SlidingBetweenStrokeCaress".Localized(
                         Appearance.BallsDescript(Player)
                    ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.OnceAgainPullsDenying".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.ThankfullyDontWaitLong".Localized(
                     Player.CockDescript()
                ));

                OutputText("Scenes.Minervapurification.PregnancyStage3.LoverClutchingBellyCum".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minervapurification.PregnancyStage3.DespiteCameBallsBloated".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.TummyBloatedBabiesFew".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.MindTightLustThoughts".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.SwollenManhoodSwellsSeed".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minervapurification.PregnancyStage3.ThankfullyNutsShrinkGush".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.FeelsFillingBurstsOrgasm".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.WombSwellsOnlyMuch".Localized());

                OutputText("Scenes.Minervapurification.PregnancyStage3.EmphasizeLiftsSloppyPop".Localized());

                Player.Orgasm("Dick");

                DoNext(() => PostPregnancyStage3Sex());

                return;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void PostPregnancyStage3Sex()
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sex.InsatiableLustsRuttingWild".Localized());

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sex.LookingOverSmiledBlush".Localized());

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sex.GrinningEarEarRevitalized".Localized());

            Player.Orgasm("Generic");

            Player.ChangeFatigue(30);

            DoNext(() => PostPregnancyStage3Sleep());
        }

        public void PostPregnancyStage3Sleep()
        {

            var timeSpent = View.Time.Hours < 6 ? 6 - View.Time.Hours : 30 - View.Time.Hours;

            View.Time.PassMinutes((int)Math.Round((double)timeSpent * 60));

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sleep.AwakenRefreshedScentPeaches".Localized());

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sleep.NoddingBiteSweetFruit".Localized());

            Player.SlimeFeed();

            Player.RefillHunger(100);

            Player.ChangeFatigue(-100);

            CampScene.SleepRecoveryApply(timeSpent, 0, true);

            Player.HealthChange(Player.MaxHealth);

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sleep.FedRestedHelpsFeet".Localized());

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sleep.DespiteLegsNodAffirmation".Localized());

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sleep.AboutToLeave".Localized());

            OutputText("Scenes.Minervapurification.PostPregnancyStage3Sleep.PromisingReturnTrekBack".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PregnancyStage4(bool repeat = false)
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PregnancyStage4.LookingAtGravidBelly".Localized());

            if (!repeat)
                OutputText("Scenes.Minervapurification.PregnancyStage4.SaysExcitedMother".Localized());
            else
                OutputText("Scenes.Minervapurification.PregnancyStage4.SaysExcitedNewAddition".Localized());

            OutputText("Scenes.Minervapurification.PregnancyStage4.TellLookForwardDaughters".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PregnancyPlayerStage1(bool repeat = false)
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage1.HandsDownBabyBump".Localized());

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage1.SoBeautifulSoHappy".Localized());

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage1.FallingOntoBackHolds".Localized());

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage1.FinalKissDisentangling".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PregnancyPlayerStage2(bool repeat = false)
        {
            ClearOutput();

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage2.HandsDownBabyBump".Localized());

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage2.SoBeautifulSoHappy".Localized());

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage2.FallingOntoBackHolds".Localized());

            OutputText("Scenes.Minervapurification.PregnancyPlayerStage2.FinalKissDisentangling".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MinervaGivesBirth()
        {
            ClearOutput();

            SceneInstance<MinervaScene>.Get().Pregnancy.Clear();

            if (View.Time.Hours < 6)
                OutputText("Scenes.Minervapurification.MinervaGivesBirth.DisturbedNothingSuspicious".Localized());
            else
                OutputText("Scenes.Minervapurification.MinervaGivesBirth.WanderingCampPiecePaper".Localized());

            OutputText("Scenes.Minervapurification.MinervaGivesBirth.AnnouncesTimeNaturalChildbirth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                OutputText("Scenes.Minervapurification.MinervaGivesBirth.UnderGiantOakTree".Localized());

            OutputText("Scenes.Minervapurification.MinervaGivesBirth.RemoveShortsExposingGenitalia".Localized());

            OutputText("Scenes.Minervapurification.MinervaGivesBirth.NoChangeWaterBreaks".Localized());

            OutputText("Scenes.Minervapurification.MinervaGivesBirth.SaysBeautifulTwoSirens".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) <= 0)
                OutputText("Scenes.Minervapurification.MinervaGivesBirth.GratefullyMotherAgainYou".Localized());

            OutputText("Scenes.Minervapurification.MinervaGivesBirth.LayNextExamineYoung".Localized());

            OutputText("Scenes.Minervapurification.MinervaGivesBirth.NoWonderVirileFertile".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaChildren, CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) + 2);

            if (View.Time.Hours < 6)
            {
                OutputText("Scenes.Minervapurification.MinervaGivesBirth.CanSleepWithNight".Localized());

                DoNext(() => SleepWithMinervaPostBirthing());
            }
            else
            {
                OutputText("Scenes.Minervapurification.MinervaGivesBirth.NewbornSirensLatchSuckle".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void PlayerGivesBirth()
        {
            OutputText("Scenes.Minervapurification.PlayerGivesBirth.PainAbdomenTimeBirth".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.AnnounceRushesBirthingSpot".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaTowerTree) > 0)
                OutputText("Scenes.Minervapurification.PlayerGivesBirth.UnderGiantOakTree".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.SentencePeriod".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.InstructsDeepBreath".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minervapurification.PlayerGivesBirth.LiftsYourCock".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Minervapurification.PlayerGivesBirth.AndBalls".Localized());

                OutputText("Scenes.Minervapurification.PlayerGivesBirth.RevealVagina".Localized());
            }

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.WaitTimeGoesWater".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.PushAsHardCan".Localized());

            Player.CuntChange(40, true, false, true);

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.StruggleLastOfSirens".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.JustRestTakeCare".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) > 0)
                OutputText("Scenes.Minervapurification.PlayerGivesBirth.DaughtersExcitedNewAddition".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.SpendTimeRestingBreastfeeds".Localized());

            OutputText("Scenes.Minervapurification.PlayerGivesBirth.EventuallyGetUpKiss".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MinervaChildren, CoC.Instance.Flags.Get(GameFlagIds.MinervaChildren) + 2);

            CoC.Instance.Flags.Set(GameFlagIds.TimesBirthedSharpies, CoC.Instance.Flags.Get(GameFlagIds.TimesBirthedSharpies) + 1);

            DoNext(() => ReturnToCamp(1));
        }

    }
}