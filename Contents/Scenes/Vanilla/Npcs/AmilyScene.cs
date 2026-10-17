using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Npcs;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Scenes.Vanilla.Places;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{

    [Register<AmilyScene>("coc:amily")]
    public class AmilyScene : Scene, ITimeAware
    {
        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Amily)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        private PregnancyStore? _buttPregnancy;

        public PregnancyStore ButtPregnancy => _buttPregnancy ??= NpcPregnancy.StoreForButt(Definition, State);

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Amily);

        public string debugName => "Amily";

        public string debugHint => "";

        public override void Entry()
        {
            AmilyFollowerEncounter();
        }

        public bool TimeChange()
        {
            var needNext = false;

            Pregnancy.PregnancyAdvance();

            ButtPregnancy.PregnancyAdvance();

            var blockCountdown = CoC.Instance.Flags.Get(GameFlagIds.AmilyBlockCountdownBecauseCorruptedJojo);

            if (blockCountdown > 0)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyBlockCountdownBecauseCorruptedJojo, blockCountdown - 1);

            var incestTimer = CoC.Instance.Flags.Get(GameFlagIds.AmilyIncestCountdownTimer);

            if (incestTimer > 0 && incestTimer < 30 * 24)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyIncestCountdownTimer, incestTimer + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                if (Pregnancy.IsPregnant && Pregnancy.Incubation == 0)
                {
                    OutputText("Common.Text.Pg".Localized());

                    AmilyPopsOutKidsInCamp();

                    Pregnancy.KnockUpForce();

                    OutputText("Common.Text.Pg".Localized());

                    needNext = true;
                }

                if (ButtPregnancy.IsPregnant && ButtPregnancy.Incubation == 0)
                {
                    AmilyLaysEggsLikeABitch();

                    ButtPregnancy.KnockUpForce();

                    needNext = true;
                }
            }

            if (View.Time.Hours == 6)
            {
                // 纯净的玩家让 Amily 崩溃搬走
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1
                    && !Player.IsPureEnough(66)

                    && CoC.Instance.Flags.Get(GameFlagIds.AmilyCampCorruptionFreaked) > 0)
                {
                    SceneInstance<AmilyScene>.Get().FarewellNote();

                    needNext = true;
                }

                // 玩家重新纯净后搬回来
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTreeFlipout) == 0
                    && CoC.Instance.Flags.Get(GameFlagIds.AmilyCampCorruptionFreaked) > 0

                    && Player.IsPureEnough(25)

                    && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 0)
                {
                    SceneInstance<AmilyScene>.Get().AmilyReturns();

                    needNext = true;
                }
            }
            else if (View.Time.Hours > 23)
            {
                var cooldown = CoC.Instance.Flags.Get(GameFlagIds.AmilyXJojoCooldown);

                if (cooldown > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyXJojoCooldown, cooldown - 1);
            }

            return needNext;
        }

        public bool TimeChangeLarge()
        {
            if (!SceneInstance<UrtaQuestScene>.Get().UrtaBusy()
                && CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 2

                && View.Time.Hours == 6)
            {
                SceneInstance<FollowerInteractionsScene>.Get().AmilyUrtaMorningAfter();

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1
                && View.Time.Hours == 6

                && CoC.Instance.Flags.Get(GameFlagIds.CampWallProgress) >= 100

                && CoC.Instance.Flags.Get(GameFlagIds.CampWallSkulls) < 100

                && Random.Shared.Next(3) == 0)
            {
                Save.SetInt("impSkullsCount", Save.GetInt("impSkullsCount", 0) + 1);

                if (Save.GetBool("pcKnowsAboutSkulls"))
                    CoC.Instance.Flags.Set(GameFlagIds.CampWallSkulls, CoC.Instance.Flags.Get(GameFlagIds.CampWallSkulls) + 1);
            }

            return false;
        }

        public void Reset()
        {
            Save.SetString("ringType", "");

            Save.SetBool("amilyMorning", false);

            Save.SetInt("metKids", 0);

            Save.SetBool("giftedCClothes", false);

            Save.SetInt("impSkullsCount", 0);

            Save.SetBool("pcKnowsAboutSkulls", false);
        }

        public bool AmilyFollower()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) != 0)
                    return false;

                return (CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) != 1 || CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 2);
            }
            else
                return false;
        }

        public bool AmilyCorrupt()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2;
        }

        public bool AmilyFollowerPure()
        {

            return AmilyFollower() && !AmilyCorrupt();
        }

        public bool AmilyFollowerCorrupt()
        {

            return AmilyFollower() && AmilyCorrupt();
        }

        public bool AmilyFurry()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 && !noFur;
        }

        public void AmilySprite(bool nude = false)
        {

        }

        public void EncounterAmily()
        {

            _encounterAmily();

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);

        }

        public void _encounterAmily()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyIsBatman) > 0 || CoC.Instance.Flags.Get(GameFlagIds.AmilyVillageEncountersDisabled) == 1 || CoC.Instance.Flags.Get(GameFlagIds.AmilyTreeFlipout) > 0)
            {
                OutputText("Scenes.Amilyscene.EncounterAmily.EnterVillageCautious".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                {
                    AmilySprite();

                    OutputText("Scenes.Amilyscene.EncounterAmily.EnterVillageLaughing".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return;
                }

            if ((CoC.Instance.Flags.Get(GameFlagIds.AmilyGrossedOutByWorms) == 1) && (!Player.HasStatusEffect(StatusEffectIds.Infested)))
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyGrossedOutByWorms, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruptFlipout) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) > 0 && !Player.IsPureEnough(25))
            {
                MeetAmilyAsACorruptAsshat();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruptFlipout) > 0 && !Player.IsPureEnough(25))
                if (!Player.HasKeyItem("Potent Mixture") && CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) < 3)
                {
                    CookAmilyASnack();

                    return;
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 0)
                        StalkingZeAmiliez();
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 1)
                            StalkingZeAmiliez2();
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 2)
                                StalkingZeAmiliez3();
                            else
                                RapeCorruptAmily4Meeting();

                    return;
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyGrossedOutByWorms) == 1 || !Player.IsPureEnough(25) || CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruptFlipout) > 0)
            {
                OutputText("Scenes.Amilyscene.EncounterAmily.EnterVillageCautiousAgain".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            AmilySprite();

            if (Player.IsPregnant() && Player.Pregnancy.Incubation == 0)
            {
                FuckingMouseBitchPopsShitOut();

                Player.ClearPregnancy();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 4)
            {
                AmilyNewCock();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 3 && Player.Gender == Gender.Hermaphrodite)
            {
                AmilyPCNewCock();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyPcGender) == (int)Player.Gender)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) + CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.AmilyVillageEncountersDisabled) == 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        ThisIsAReallyShittyBadEnd();
                    else
                        ThisFunctionProbablySucksTooOhYeahAmilyFunction();

                    return;
                }

                if (Player.Gender == Gender.Male)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) >= 50 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyPcGender) == 1)
                    {
                        OutputText("Scenes.Amilyscene.EncounterAmily.WanderSearchAmily".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.HearWhimperTrack".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.AsksTimeToTalk".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.AgreeToTalk".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.ScuffsGround".Localized());

                        ClearButtons();

                        AddNextButton("Common.Button.AcceptHer".Localized(), DesperateAmilyPleaAcceptHer);

                        if (AmilyCanHaveTFNow())
                            AddNextButton("Common.Button.Defur".Localized(), AmilyDefurrify);
                        else
                            if (!noFur)
                                AddNextButton("Common.Button.RejectFurry".Localized(), AmilyNoFur);

                        AddNextButton("Common.Button.RejectGently".Localized(), DesperateAmilyPleaTurnDown);

                        AddNextButton("Common.Button.RejectBluntly".Localized(), DesperateAmilyPleaTurnDownBlunt);

                        return;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) == 0)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyMetAs, (int)Player.Gender);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyMet, CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) + 1);

                        OutputText("Scenes.Amilyscene.EncounterAmily.WindThroughRuins".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.VoiceCallsWarning".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) != 0)
                            OutputText("Scenes.Amilyscene.EncounterAmily.CreatureCorneredYou".Localized());
                        else
                            OutputText("Scenes.Amilyscene.EncounterAmily.CorneredByBeing".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.LowersBlowpipe".Localized());

                        if (Player.HumanScore() > 4)
                            OutputText("Scenes.Amilyscene.EncounterAmily.AreYouQuestion".Localized());
                        else
                            OutputText("Scenes.Amilyscene.EncounterAmily.WereYouQuestion".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.OneOfThoseHumans".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.AdmitHumanAsk".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.BornHereSpits".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.StaresIntently".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.ExplainsRole".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.TucksBlowpipe".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.WhatDoYouDo".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.AcceptEagerly".Localized(), AcceptAmilysOfferEagerly);

                        AddButton(1, "Common.Button.Hesitantly".Localized(), AcceptAmilyOfferHesitantly);

                        if (!noFur)
                            AddButton(2, "Common.Button.NoFurries".Localized(), AmilyNoFur);

                        AddButton(3, "Common.Button.Refuse".Localized(), RefuseAmilysOffer);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

                        return;
                    }
                    else
                        if (Player.Gender == Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.AmilyOfferAccepted) == 0)
                        {
                            OutputText("Scenes.Amilyscene.EncounterAmily.WanderSearchAmilyAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.HearWhimperTrackAgain".Localized());

                            AmilyRemeetingContinued();

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

                            return;
                        }
                }
                else
                    if (Player.Gender == Gender.Female)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) == 0)
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.AmilyMetAs, (int)Player.Gender);

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyMet, CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) + 1);

                            OutputText("Scenes.Amilyscene.EncounterAmily.WindThroughRuinsAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.VoiceCallsWarningAgain".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) > 0)
                                OutputText("Scenes.Amilyscene.EncounterAmily.CreatureCorneredYouAgain".Localized());
                            else
                                OutputText("Scenes.Amilyscene.EncounterAmily.CorneredByBeingAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.LowersBlowpipeAgain".Localized());

                            if (Player.HumanScore() > 4)
                                OutputText("Scenes.Amilyscene.EncounterAmily.AreYouQuestionAgain".Localized());
                            else
                                OutputText("Scenes.Amilyscene.EncounterAmily.WereYouQuestionAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.ThoseHumansAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.AdmitHumanAskAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.BornHereSpitsAgain".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.ShakesHeadWistful".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.ThankHerBrushOff".Localized());

                            OutputText("Scenes.Amilyscene.EncounterAmily.WinksWandersOff".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

                            DoNext(() => ReturnToCamp(1));

                            return;
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyConfessedLesbian) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) >= 25)
                        {
                            AmilyIsTotallyALesbo();

                            return;
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyConfessedLesbian) == 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) < 3)
                        {
                            AmilyPostConfessionGirlRemeeting();

                            return;
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) > 35 && CoC.Instance.Flags.Get(GameFlagIds.AmilyConfessedLesbian) == 1)
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, 35);

                            AmilyIsTotallyALesbo();

                            return;
                        }
                    }
                    else
                        if (Player.Gender == Gender.Hermaphrodite)
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) == 0)
                            {
                                CoC.Instance.Flags.Set(GameFlagIds.AmilyMetAs, (int)Player.Gender);

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyMet, CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) + 1);

                                OutputText("Scenes.Amilyscene.EncounterAmily.WindThroughRuinsThird".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.VoiceCallsWarningThird".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) > 0)
                                    OutputText("Scenes.Amilyscene.EncounterAmily.CreatureCorneredYouThird".Localized());
                                else
                                    OutputText("Scenes.Amilyscene.EncounterAmily.CorneredByBeingThird".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.LowersBlowpipeThird".Localized());

                                if (Player.HumanScore() > 4)
                                    OutputText("Scenes.Amilyscene.EncounterAmily.AreYouQuestionThird".Localized());
                                else
                                    OutputText("Scenes.Amilyscene.EncounterAmily.WereYouQuestionThird".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.ThoseHumansThird".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.AdmitHumanAskThird".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.BornHereSpitsThird".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.LooksThoughtfulStops".Localized());

                                if (Player.BiggestBreastSize() < 1)
                                    OutputText("Scenes.Amilyscene.EncounterAmily.SniffsAir".Localized());
                                else
                                    OutputText("Scenes.Amilyscene.EncounterAmily.StaresAtBulges".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.AsksHermaphrodite".Localized());

                                OutputText("Scenes.Amilyscene.EncounterAmily.VanishesIntoDust".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

                                DoNext(() => ReturnToCamp(1));

                                return;
                            }

                            if (((CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) >= 15 && Random.Shared.Next(3) == 0) || CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) >= 20) && CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 0)
                            {
                                WhyNotHerms();

                                return;
                            }

                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 1)
                            {
                                MaybeHermsAintAllBadBITCH();

                                return;
                            }
                        }
                        else
                            if ((Player.Gender == Gender.Genderless) && (CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) == 0))
                                {
                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyMetAs, (int)Player.Gender);

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyMet, CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) + 1);

                                    OutputText("Scenes.Amilyscene.EncounterAmily.WindThroughRuinsFourth".Localized());

                                    OutputText("Scenes.Amilyscene.EncounterAmily.VoiceCallsWarningFourth".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) > 0)
                                        OutputText("Scenes.Amilyscene.EncounterAmily.CreatureCorneredYouFourth".Localized());
                                    else
                                        OutputText("Scenes.Amilyscene.EncounterAmily.CorneredByBeingFourth".Localized());

                                    OutputText("Scenes.Amilyscene.EncounterAmily.LowersBlowpipeFourth".Localized());

                                    if (Player.HumanScore() > 4)
                                        OutputText("Scenes.Amilyscene.EncounterAmily.AreYouQuestionFourth".Localized());
                                    else
                                        OutputText("Scenes.Amilyscene.EncounterAmily.WereYouQuestionFourth".Localized());

                                    OutputText("Scenes.Amilyscene.EncounterAmily.ThoseHumansFourth".Localized());

                                    OutputText("Scenes.Amilyscene.EncounterAmily.AdmitHumanAskFourth".Localized());

                                    OutputText("Scenes.Amilyscene.EncounterAmily.BornHereSpitsFourth".Localized());

                                    if (Player.BiggestBreastSize() < 1)
                                    {
                                        OutputText("Scenes.Amilyscene.EncounterAmily.StaresIntentlyAgain".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.ExplainsRoleAgain".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.TucksBlowpipeAgain".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.ConfessCannotHelp".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.DemandsWhyHurt".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.EmbarrassedDropClothes".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.EyesBugOut".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.StopsBeforeWall".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.MusingDriftsBack".Localized());
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Amilyscene.EncounterAmily.ShakesHeadWistfulAgain".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.ExplainNotWoman".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.ButYouHaveBoobs".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.AdmitThatIsSo".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.AtLossForWords".Localized());

                                        OutputText("Scenes.Amilyscene.EncounterAmily.RegainedConfidenceWinks".Localized());
                                    }

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

                                    DoNext(() => ReturnToCamp(1));

                                    return;
                                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyPcGender) != (int)Player.Gender && (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) != 3 || Player.Gender != Gender.Hermaphrodite))
            {
                AmilyNewGenderConfrontation();

                return;
            }

            AmilyVillageMenu();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);
        }

        public void AmilyVillageMenu()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyVillageMenu.CuriousHowHolding".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5)
            {
                OutputText("Scenes.Amilyscene.EncounterAmily.MaterializesSlow".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15 || Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Amilyscene.EncounterAmily.GetJobDone".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.EncounterAmily.StrokesBellyGrinning".Localized());
                    else
                        OutputText("Scenes.Amilyscene.EncounterAmily.ThankYouExclaims".Localized());
            }

            if (__sw0_0 == 6 || __sw0_0 == 7 || __sw0_0 == 8)
            {
                OutputText("Scenes.Amilyscene.AmilyVillageMenu.MinutesBeforeAppears".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15 || Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Amilyscene.EncounterAmily.AttentionToMidriff".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.EncounterAmily.GroansSoftlyThanks".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyVillageMenu.SmilesBeatifically".Localized());
            }

            if (true )
            {
                OutputText("Scenes.Amilyscene.EncounterAmily.MaterializesQuick".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15 || Player.Gender == Gender.Genderless)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetAs) == 2 && Player.Gender == Gender.Female)
                        OutputText("Scenes.Amilyscene.EncounterAmily.CrossesArmsSmiles".Localized());
                    else
                        OutputText("Scenes.Amilyscene.EncounterAmily.CrossesArmsTaps".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.EncounterAmily.SmilesSoftlySeeYou".Localized());
                    else
                    {
                        OutputText("Scenes.Amilyscene.EncounterAmily.GrinsOpenDelight".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Amilyscene.EncounterAmily.ComeKnockMeUp".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0 && Player.Pregnancy.Incubation == 0)
                                OutputText("Scenes.Amilyscene.EncounterAmily.OrGetKnockedUp".Localized());
                        }
                        else
                            if (Player.HasVagina())
                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0 && Player.Pregnancy.Incubation == 0)
                                    OutputText("Scenes.Amilyscene.EncounterAmily.StuffAnotherBun".Localized());
                                else
                                    OutputText("Scenes.Amilyscene.EncounterAmily.ComeForQualityTime".Localized());

                        OutputText("Scenes.Amilyscene.EncounterAmily.TeasesBodyLanguage".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                        {
                            OutputText("Scenes.Amilyscene.EncounterAmily.ErectionTentsPants".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                        }

                        OutputText("Scenes.Amilyscene.EncounterAmily.NoJokingMatter".Localized());
                    }
            }

            var sex = DetermineAmilySexEvent();

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), AmilyAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), TalkToAmily);

            if (sex != null)
            {
                AddButton(2, "Common.Button.Sex".Localized(), sex);

                AddButton(3, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), TalkThenSexWithAmily);
            }
            else
            {
                AddButton(2, "Common.Button.Sex".Localized(), null);

                AddButton(3, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), null);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 5)
                AddButton(4, "Common.Button.Children".Localized(), AmilyDiscussChildren);
            else
                if ((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0) && (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) >= 40 && Player.Gender == Gender.Hermaphrodite))
                        if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                        {
                            OutputText("Scenes.Amilyscene.AmilyVillageMenu.TwoMothersEfficiency".Localized());

                            AddButton(4, "Common.Button.Efficiency".Localized(), MakeAmilyAHerm);
                        }
                        else
                            AddButton(4, "Common.Button.Efficiency".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyOfferedDefurry) > 0)
                AddNextButton("Common.Button.Defur".Localized(), AmilyDefurrify);

            SetExitButton("Common.Button.Back".Localized());
        }

        public Action? DetermineAmilySexEvent(bool forced = false)
        {

            Action sex = null;

            if (!forced && Player.Lust < 33)
                return null;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyConfessedLesbian) > 0 && Player.Gender == Gender.Female)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    if (!Player.IsPregnant())
                        sex = HermilyOnFemalePC;
                    else
                        if (Random.Shared.Next(2) == 0)
                            sex = GirlyGirlMouseSex;
                        else
                            sex = HermilyOnFemalePC;
                }
                else
                    sex = GirlyGirlMouseSex;

            if (Player.Gender == Gender.Hermaphrodite && CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 2)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    if (!Player.IsPregnant())
                    {
                        if (!Player.IsPregnant())
                        {
                            if (Random.Shared.Next(2) == 0)
                                sex = HermilyOnFemalePC;
                            else
                                if (forced)
                                    sex = AmilySexHappens;
                                else
                                    sex = SexWithAmily;
                        }
                        else
                            if (forced)
                                sex = AmilySexHappens;
                            else
                                sex = SexWithAmily;
                    }
                    else
                        if (!Player.IsPregnant())
                            sex = HermilyOnFemalePC;
                        else
                            if (Random.Shared.Next(2) == 0)
                                sex = HermilyOnFemalePC;
                            else
                                if (forced)
                                    sex = AmilySexHappens;
                                else
                                    sex = SexWithAmily;
                }
                else
                    if (!Player.IsPregnant())
                        sex = SexWithAmily;
                    else
                        if (Random.Shared.Next(2) == 0)
                            sex = GirlyGirlMouseSex;
                        else
                            if (forced)
                                sex = AmilySexHappens;
                            else
                                sex = SexWithAmily;

            if (Player.Gender == Gender.Male)
                if (forced)
                    sex = AmilySexHappens;
                else
                    sex = SexWithAmily;

            return sex;
        }

        public void AcceptAmilysOfferEagerly()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AcceptAmilysOfferEagerly.GrinLecherously".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilysOfferEagerly.MouseWomanAppalled".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilysOfferEagerly.ThisIsPrecisely".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilysOfferEagerly.StillDisgruntled".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilysOfferEagerly.BrashMadeUncomfortable".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 5);

            Player.ApplyStats(new DynStat(GameStats.Libido, 5f));

            DoNext(() => AmilySexHappens());
        }

        public void AcceptAmilyOfferHesitantly()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.OfferShockingEnticing".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.AskIfSheReally".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.RepliesSadly".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.BowHeadWillHelp".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.BlinksSurprised".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.LeadsYouEagerly".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyOfferHesitantly.MadeAmilyHappy".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 5);

            DoNext(() => AmilySexHappens());
        }

        public void RefuseAmilysOffer()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.RefuseAmilysOffer.ShakeHeadRefusal".Localized());

            OutputText("Scenes.Amilyscene.RefuseAmilysOffer.StaresInDisbelief".Localized());

            OutputText("Scenes.Amilyscene.RefuseAmilysOffer.CannotWithStranger".Localized());

            OutputText("Scenes.Amilyscene.RefuseAmilysOffer.WideEyedAtWords".Localized());

            OutputText("Scenes.Amilyscene.RefuseAmilysOffer.BowsAndLeaves".Localized());

            OutputText("Scenes.Amilyscene.RefuseAmilysOffer.ImpressedHerResolve".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 10);

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RemeetingAmilyAnnounceSelf()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.RemeetingAmilyAnnounceSelf.ClearsThroatQuietly".Localized());

            DoNext(() => AmilyRemeetingContinued());
        }

        public void RemeetingAmilyScare()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.RemeetingAmilyScare.SneakUpGrabs".Localized());

            if (Player.Speed < 50 || !Player.HasPerk(PerkIds.Evade))
            {
                Player.HealthChange(-5);

                OutputText("Scenes.Amilyscene.RemeetingAmilyScare.ScrambleBackGash".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.RemeetingAmilyScare.LeapBackwardsJust".Localized());

            DoNext(() => AmilyRemeetingContinued());
        }

        public void AmilyRemeetingContinued()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyRemeetingContinued.AsksChangedMind".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), SecondTimeAmilyOfferedAccepted);

            AddButton(1, "Common.Button.RefusePolite".Localized(), SecondTimeAmilyRefuseAgain);

            AddButton(2, "Common.Button.JustTalk".Localized(), RepeatAmilyTalk);

            AddButton(3, "Common.Button.GetLost".Localized(), TellAmilyToGetLost);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void SecondTimeAmilyOfferedAccepted()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.SecondTimeAmilyOfferedAccepted.TellHerYesChildren".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

            DoNext(() => AmilySexHappens());
        }

        public void SecondTimeAmilyRefuseAgain()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.SecondTimeAmilyRefuseAgain.ShakeHeadPositionSame".Localized());

            OutputText("Scenes.Amilyscene.SecondTimeAmilyRefuseAgain.OfferStillStands".Localized());

            OutputText("Scenes.Amilyscene.SecondTimeAmilyRefuseAgain.RememberThatLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RepeatAmilyTalk()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.RepeatAmilyTalk.OnlyWantedTalk".Localized());

            OutputText("Scenes.Amilyscene.RepeatAmilyTalk.JustToTalkAsks".Localized());

            DoNext(() => TalkWithCuntIMeanAmily());
        }

        public void TellAmilyToGetLost()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.TellAmilyToGetLost.JeerHypocrite".Localized());

            OutputText("Scenes.Amilyscene.TellAmilyToGetLost.RedWithRage".Localized());

            OutputText("Scenes.Amilyscene.TellAmilyToGetLost.SpinStalkOff".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SexWithAmily()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.SexWithAmily.TellWantSex".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.SexWithAmily.PuzzledWhy".Localized());

                    OutputText("Scenes.Amilyscene.SexWithAmily.MovesQuicklyPregnant".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 3);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.SexWithAmily.SurprisedHandMidriff".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.SetsOffUnhappy".Localized());

                        DoNext(() => AmilySexHappens());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.SexWithAmily.StillWantMePregnant".Localized());

                        DoNext(() => AmilySexHappens());
                    }

            if (__sw0_0 == 6 || __sw0_0 == 7 || __sw0_0 == 8)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.SexWithAmily.DisbeliefObvious".Localized());

                    OutputText("Scenes.Amilyscene.SexWithAmily.AnnoyedWaddlesOff".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 3);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.SexWithAmily.BogglesDisbelief".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.InsistNotJoking".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.FlatteredButKnowHow".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.ConcedeDontKnow".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.NotComfortableWith".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.ApologizeLeave".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 3);

                        DoNext(() => ReturnToCamp(1));
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.SexWithAmily.PuzzledThenSmiles".Localized());

                        DoNext(() => AmilySexHappens());
                    }

            if (true )
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.SexWithAmily.OfCourseOblige".Localized());

                    OutputText("Scenes.Amilyscene.SexWithAmily.SetsOffLeading".Localized());

                    DoNext(() => AmilySexHappens());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.SexWithAmily.TeasesTailWaving".Localized());

                        OutputText("Scenes.Amilyscene.SexWithAmily.PushToKeepUp".Localized());

                        DoNext(() => AmilySexHappens());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.SexWithAmily.GrinsLikeCanary".Localized());

                        DoNext(() => AmilySexHappens());
                    }
        }

        public void TalkToAmily()
        {
            ClearOutput();

            AmilySprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetAs) == 2 && Player.Gender == Gender.Female)
                OutputText("Scenes.Amilyscene.TalkToAmily.TellWantTalk".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkToAmily.WantTalkNoSex".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
            {
                var __sw0_1 = Player.Pregnancy.Event;

                if (__sw0_1 == 1 || __sw0_1 == 2 || __sw0_1 == 3 || __sw0_1 == 4 || __sw0_1 == 5)
                    OutputText("Scenes.Amilyscene.TalkToAmily.CouldUseSomeoneTalk".Localized());

                if (__sw0_1 == 6 || __sw0_1 == 7 || __sw0_1 == 8)
                    OutputText("Scenes.Amilyscene.TalkToAmily.NowWantKnowMe".Localized());

                if (true )
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetAs) == 2 && Player.Gender == Gender.Female)
                        OutputText("Scenes.Amilyscene.TalkToAmily.ChatWouldBeLovely".Localized());
                    else
                        OutputText("Scenes.Amilyscene.TalkToAmily.YouWantToTalk".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    OutputText("Scenes.Amilyscene.TalkToAmily.EnjoyOurTalksWhat".Localized());
                else
                    OutputText("Scenes.Amilyscene.TalkToAmily.SmilesPlayfully".Localized());

            DoNext(() => TalkWithCuntIMeanAmily());
        }

        public void TalkThenSexWithAmily()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.WantTalkThenSex".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.RubsBellyConversation".Localized());

                    DoNext(() => TalkWithCuntIMeanAmily());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.TalkingAlwaysNice".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.AssureTrimLean".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.FlattererHaveFood".Localized());

                        DoNext(() => TalkToAmilyWithSexAfter());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.MuseWithoutSex".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.AssureThatsFine".Localized());

                        DoNext(() => TalkToAmilyWithSexAfter());
                    }

            if (__sw0_0 == 6 || __sw0_0 == 7 || __sw0_0 == 8)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.StaresThenSmiles".Localized());

                    OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.DespiteRefusingSits".Localized());

                    DoNext(() => TalkWithCuntIMeanAmily());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.BlinksTalkWouldWonderful".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.ConcedeNoIdeas".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.PullsUpSeat".Localized());

                        DoNext(() => TalkWithCuntIMeanAmily());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.GrinsPlayfullyTalk".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.WaddlesToStoneSeat".Localized());

                        DoNext(() => TalkToAmilyWithSexAfter());
                    }

            if (true )
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.NotLikeOthers".Localized());

                    OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.HurryUpCallsBack".Localized());

                    DoNext(() => TalkToAmilyWithSexAfter());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.SmilesTiredLonely".Localized());

                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.CrooksFinger".Localized());

                        DoNext(() => TalkToAmilyWithSexAfter());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.TalkThenSexWithAmily.ConversationAndLovemaking".Localized());

                        DoNext(() => TalkToAmilyWithSexAfter());
                    }
        }

        public void SneakyUberAmilyRemeetingsAnnounce()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.ClearsThroatQuietly".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                    OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.GrumblesSlowingDown".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.EasierToFindBelly".Localized());
                    else
                        OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.SmirksNotEasier".Localized());

            if (__sw0_0 == 6 || __sw0_0 == 7 || __sw0_0 == 8)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                    OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.ProclaimsDefensively".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.NotEasyToHide".Localized());
                    else
                        OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.GladYouCameBack".Localized());

            if (true )
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                    OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.NobodyTrackedMe".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.WorkOnThatBozo".Localized());
                    else
                        OutputText("Scenes.Amilyscene.SneakyUberAmilyRemeetingsAnnounce.ProudComingAlong".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sex".Localized(), null);

            AddButton(2, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), null);

            if (Player.Lust >= 33)
            {
                AddButton(0, "Common.Button.Sex".Localized(), SexWithAmily);

                AddButton(2, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), TalkThenSexWithAmily);
            }
        }

        public void ScareAmilyRemeetingsProBaws()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.SneakUpGrabs".Localized());

            if (Player.Speed < 50)
            {
                Player.HealthChange(-5);

                OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.ScrambleBackGash".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                    OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.CollectedDumbThing".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                    {
                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.ThatHappensKnife".Localized());

                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.AssureNothingSerious".Localized());

                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.RelievedDeserveIt".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.TrailsOffIncoherent".Localized());

                        if (Player.IsPregnant())
                            OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.ChildrenWithoutFather".Localized());

                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.ParagraphBreak".Localized());

                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.ApologizeFoolishness".Localized());

                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.SniffsPrimlyGrins".Localized());
                    }
            }
            else
            {
                OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.LeapBackwardsJust".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                {
                    OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.FierceDontSneak".Localized());

                    OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.NotCalmedDownYet".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.IrritatedAreYouInsane".Localized());
                    else
                    {
                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.FearToReliefAnger".Localized());

                        OutputText("Scenes.Amilyscene.ScareAmilyRemeetingsProBaws.PromiseWontStartle".Localized());
                    }
            }

            ClearButtons();

            AddButton(0, "Common.Button.Sex".Localized(), null);

            AddButton(2, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), null);

            if (Player.Lust >= 33)
            {
                AddButton(0, "Common.Button.Sex".Localized(), SexWithAmily);

                AddButton(2, "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized(), TalkThenSexWithAmily);
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 1);
        }

        public void AnnounceSelfOnDesperatePleaMeeting()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AnnounceSelfOnDesperatePleaMeeting.ClearsThroatQuietly".Localized());

            DesperateFinallyAmily();
        }

        public void ScareAmilyOnDesperatePleaMeeting()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.ScareAmilyOnDesperatePleaMeeting.SneakUpGrabs".Localized());

            if (Player.Speed < 50)
            {
                Player.HealthChange(-5);

                OutputText("Scenes.Amilyscene.ScareAmilyOnDesperatePleaMeeting.ScrambleBackGash".Localized());

                OutputText("Scenes.Amilyscene.ScareAmilyOnDesperatePleaMeeting.YoullLiveRelief".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.ScareAmilyOnDesperatePleaMeeting.LeapBackwardsJust".Localized());

                OutputText("Scenes.Amilyscene.ScareAmilyOnDesperatePleaMeeting.ShiftsAwkwardTalk".Localized());
            }

            DesperateFinallyAmily();
        }

        public void DesperateFinallyAmily()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.DesperateFinallyAmily.CuriousAgree".Localized());

            OutputText("Scenes.Amilyscene.DesperateFinallyAmily.ScuffsGround".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.AcceptHer".Localized(), DesperateAmilyPleaAcceptHer);

            AddButton(1, "Common.Button.RejectGently".Localized(), DesperateAmilyPleaTurnDown);

            AddButton(2, "Common.Button.RejectBluntly".Localized(), DesperateAmilyPleaTurnDownBlunt);
        }

        public void DesperateAmilyPleaAcceptHer()
        {
            ClearOutput();

            AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaAcceptHer.GentleSmileTakeHand".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaAcceptHer.StunnedEmbraces".Localized());

            DoNext(() => AmilySexHappens());
        }

        public void DesperateAmilyPleaTurnDown()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDown.SoftlySorryCannot".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDown.SniffsTearsRunning".Localized());

            if (SceneInstance<TelAdre>.Get().IsAllowedInto())
                OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDown.TellHiddenCity".Localized());
            else
                OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDown.DejectedWalksAway".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDown.RetreatQuickly".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DesperateAmilyPleaTurnDownBlunt()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDownBlunt.WithoutMercyBlunt".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDownBlunt.ScreamsBetrayal".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDownBlunt.JeerNotYourFault".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDownBlunt.SpitsFoulLanguage".Localized());

            OutputText("Scenes.Amilyscene.DesperateAmilyPleaTurnDownBlunt.NeverComesBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckingMouseBitchPopsShitOut()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.FuckingMouseBitchPopsShitOut.HeadIntoVillage".Localized());

            OutputText("Scenes.Amilyscene.FuckingMouseBitchPopsShitOut.WhatWillYouDo".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyBirthTotal, CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), PregnancyIsScaryGoddamnMousePregnancyImNotWatchingThisShit);

            AddButton(1, "Common.Button.Watch".Localized(), HeyIGotTicketsToMicePoppingOut);

            AddButton(2, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), HelpThatFukkinUngratefulBitchGiveBirth);
        }

        public void PregnancyIsScaryGoddamnMousePregnancyImNotWatchingThisShit()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.PregnancyIsScaryGoddamnMousePregnancyImNotWatchingThisShit.HastyRetreat".Localized());

            OutputText("Scenes.Amilyscene.PregnancyIsScaryGoddamnMousePregnancyImNotWatchingThisShit.FindNoteBark".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 10);

            DoNext(() => ReturnToCamp(1));
        }

        public void HeyIGotTicketsToMicePoppingOut()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.DontWantToRun".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.MousyFigurePink".Localized());
            else
                OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.FigureWord".Localized());

            OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.SixInchesSlipsOut".Localized());

            OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.BabysSlipFree".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.HairlessInfantMice".Localized());
            else
                OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.SimilarToInfant".Localized());

            OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.CrawlHighSpeed".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.FurBegunCover".Localized());
            else
                OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.MaturingDramatically".Localized());

            OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.ColorPatternsVary".Localized());

            OutputText("Scenes.Amilyscene.HeyIGotTicketsToMicePoppingOut.MakingSureNoDanger".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void HelpThatFukkinUngratefulBitchGiveBirth()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.MoveForwardInstinctively".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.WhatAreYouDoing".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.MuzzleSkywards".Localized());
            else
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.HissingInPain".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.StrongestContraction".Localized());

            var amilyKid = "girl";

            if ((Player.Gender == Gender.Hermaphrodite || CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0) && Random.Shared.Next(2) == 0)
                amilyKid = "herm";

            if (Player.Gender == Gender.Hermaphrodite && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                amilyKid = "herm";

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.PlaceholderPeriodBreak".Localized(
                amilyKid
            ));

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.TellShesBeautiful".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.TrailsOffContraction".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.UncertainDoAsTold".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.FurWord".Localized());
            else
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.BosomWord".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.RootingEagerly".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.FurComesIn".Localized());
            else
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.HairGrowsFaster".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.FirstbornLeader".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.FurWordAgain".Localized());
            else
                OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.HairWord".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.MatchesYourHair".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.TiredButSmiles".Localized());

            OutputText("Scenes.Amilyscene.HelpThatFukkinUngratefulBitchGiveBirth.MouseletsCurlUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 5);

            DoNext(() => ReturnToCamp(1));
        }

        public void ThisIsAReallyShittyBadEnd()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.WanderEmptyStreets".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.AmilyWalksOut".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.EchoHerWords".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.WhistlesPiercingly".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.LittersFathered".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.BroodGathers".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.GoodbyeNotNoble".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.InclinesHeadNod".Localized());

            OutputText("Scenes.Amilyscene.ThisIsAReallyShittyBadEnd.LeftWithChildren".Localized());

            DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);
        }

        public void ThisFunctionProbablySucksTooOhYeahAmilyFunction()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.WanderRuinedVillage".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.DarlingYoureHere".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.DelightedSqueak".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.ShouldFindYouFirst".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.TooConfusedToHug".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.BlinksLooksConfused".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.VeritableStampede".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.UnableAskPlainly".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.OurChildrenYours".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) > 0)
                OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.PeriodSayEnd".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) == 0)
                    OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.PlantedByMe".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) > 0)
                        OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.PlantedByYou".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.TrillsHappily".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.StareAtMultitude".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.ConfirmsSheLaughs".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.ShockDisbeliefGrow".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMetAs) == 1)
                OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.SpermToMe".Localized());
            else
                OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.FirstMetAfraid".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.CameToLoveYou".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.ShakeOffConfusion".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.HourWithBrood".Localized());

            OutputText("Scenes.Amilyscene.ThisFunctionProbablySucksTooOhYeahAmilyFunction.LoversMenuAdded".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, .3f);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, 6);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, 6);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVaginalWetness, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, "rags");

            if (SceneInstance<MarbleScene>.Get().MarbleAtCamp())
                CoC.Instance.Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, 1);
            else
                CoC.Instance.Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, 2);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.IzmaAmilyFreakoutStatus, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkWithCuntIMeanAmily(bool sexAfter = false)
        {
            ClearOutput();

            AmilySprite();

            float convo = Random.Shared.Next(15);

            if (convo == 12 && AmilyFollower())
                convo += 1;

            if (Player.Gender == Gender.Female || (Player.Gender == Gender.Hermaphrodite && CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) < 2))
                convo = Random.Shared.Next(12);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2 + Random.Shared.Next(3));

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.34f));

            switch ((int)convo)
            {
                case 0:

                    if (!TalkAmilyItems(sexAfter))
                        return;

                    break;

                case 1: TalkAmilyMinotaurs(sexAfter); break;
                case 2: TalkAmilySandWitches(sexAfter); break;
                case 3: TalkAmilyGiantBees(sexAfter); break;
                case 4: TalkAmilyCultists(sexAfter); break;
                case 5: TalkAmilyImps(sexAfter); break;
                case 6: TalkAmilySharkGirls(sexAfter); break;
                case 7: TalkAmilyGoblins(sexAfter); break;
                case 8: TalkAmilyYourVillage(sexAfter); break;
                case 9: TalkAmilyHerFamily(sexAfter); break;
                case 10: TalkAmilyTheAttack(sexAfter); break;
                case 11: TalkAmilySurvival(sexAfter); break;
                case 12: TalkAmilyBreeding(); break;
                case 13: TalkAmilyKitsune(); break;
                default: TalkAmilyChildren(sexAfter); break;
            }

            if (sexAfter)
                DoNext(DetermineAmilySexEvent(true));
            else
                DoNext(() => ReturnToCamp(1));
        }

        public bool TalkAmilyItems(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NoEquipmentAlchemy".Localized());

            var itemSlot = Player.Inventory.Slots[0];

            if (itemSlot.Quantity == 0)
            {
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.KeepThatInMind".Localized());

                if (sexAfter)
                    DoNext(DetermineAmilySexEvent());
                else
                    DoNext(() => ReturnToCamp(1));

                return false;
            }

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SomethingInPockets".Localized());

            var itemId = itemSlot.Item.Id;

            if (itemId == ItemIds.Equinum)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HorseEssence".Localized());
            else if (itemId == ConsumableIds.CaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DogsDick".Localized());
            else if (itemId == ConsumableIds.LargeCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DogsDickAgain".Localized());
            else if (itemId == ConsumableIds.DoubleCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DogsDickThird".Localized());
            else if (itemId == ConsumableIds.BlackCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DogsDickFourth".Localized());
            else if (itemId == ConsumableIds.KnottyCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DogsDickFifth".Localized());
            else if (itemId == ConsumableIds.BulbyCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DogsDickSixth".Localized());
            else if (itemId == ConsumableIds.IncubiDraft || itemId == ConsumableIds.SuccubiMilk)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DemonFluidRecoil".Localized());
            else if (itemId == ConsumableIds.SuccubusDelight)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.FullOfTaint".Localized());
            else if (itemId == ConsumableIds.SlimyClothoth)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NoIdeaConfused".Localized());
            else if (itemId == ConsumableIds.BeeHoney)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AsksEagerlyHoney".Localized());
            else if (itemId == ConsumableIds.PureHoney)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.UltraPureHoney".Localized());
            else if (itemId == ConsumableIds.W_FRUIT)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhiskerFruit".Localized());
            else if (itemId == ConsumableIds.PigtailTruffle || itemId == ConsumableIds.BoarTruffle)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PigtailTruffle".Localized());
            else if (itemId == ItemIds.GreenGel)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SlimeFromGel".Localized());
            else if (itemId == UseableIds.BeeChitin)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ArmorFromSlime".Localized());
            else if (itemId == ConsumableIds.ToughSilk)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SpiderSilkTailor".Localized());
            else if (itemId == ConsumableIds.D_SCALE)
            {

                bool kihaFollower = CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) != 1 && CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0;

                bool emberHatched = CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0;

                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NeverKnewDragons".Localized());

                if (kihaFollower || emberHatched)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.UntilWord".Localized());

                if (kihaFollower)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.KihaName".Localized());

                if (kihaFollower && emberHatched)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AndWord".Localized());

                if (emberHatched)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.EmberName".Localized());

                if (kihaFollower || emberHatched)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.CameToCamp".Localized());

                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.MakeSomethingOut".Localized());
            }
            else if (itemId == UseableIds.ImpSkull)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExecuteImpsBehind".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ShrugsUnfamiliar".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ReturnItemPocket".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SentencePeriodBreak".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TakeLeaveMouseGirl".Localized());

            return true;
        }

        public void TalkAmilyMinotaurs(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesAgain".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LuckySkinIntact".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhyMinotaursDangerous".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AsWellBeDemons".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AdviceDealingThem".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LaughsRunsAway".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhatSheDid".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.FoughtOffRan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) == 0)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.StillAVirgin".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.VirginWhenMet".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.BruteFollowedMe".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ProudOfHerself".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SentencePeriodBreakAgain".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExcuseYourself".Localized());
        }

        public void TalkAmilySandWitches(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesThird".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SandWitchesNote".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SandWitchesTrying".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DruidicSect".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhyConclusion".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.RobesAllWomen".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HowWouldKnow".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LostInDesert".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhatSpellWouldDo".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.MilkProducingSlave".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DoesntNeedSpell".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DrankHerFill".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PromiseBeCareful".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SentencePeriodBreakThird".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TakeYourLeave".Localized());
        }

        public void TalkAmilyGiantBees(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesFourth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.GiantBeesStrange".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExplainHowWorks".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.OfficialNameBees".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ConfusedLookContinues".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HatchHundredsBees".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExpressionTells".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.CannotExplainBetter".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HypnoticThrumming".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ThankAdviceCurious".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TradeHoneyPast".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ThankForSharing".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SentencePeriodBreakFourth".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PolitelyExcuse".Localized());
        }

        public void TalkAmilyCultists(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesFifth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ThinksThenShakes".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ItsAllRight".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AndSigh".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AndLeave".Localized());
        }

        public void TalkAmilyImps(bool sexAfter)
        {
            bool amilyAtCamp = CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) != 0;

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesSixth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SpitsDemonVermin".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TailLashingFuriously".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.RegainsComposure".Localized());
            else
            {
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AndLeaves".Localized());

                if (!amilyAtCamp)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SetOffToCamp".Localized());
                else
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SitBackInCamp".Localized());

                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.GoneToKillImp".Localized());
            }
        }

        public void TalkAmilySharkGirls(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesSeventh".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SharkGirlsVillage".Localized());

            if (!SceneInstance<IzmaScene>.Get().IzmaFollower())
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PensiveNoMales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AdviceFightingThem".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AfraidNotShore".Localized());

            if (!sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ThankTimeTakeLeave".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ConversationWinds".Localized());
        }

        public void TalkAmilyGoblins(bool sexAfter)
        {
            bool amilyAtCamp = CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) != 0;

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesEighth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.GoblinsShakesHead".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AreTheyAggressive".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TerritorialFemales".Localized());

            if (!sexAfter)
            {
                if (!amilyAtCamp)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HeadBackCamp".Localized());
                else
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SitBackOwnCamp".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ConversationWindsAgain".Localized());
        }

        public void TalkAmilyYourVillage(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PonderQuestion".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SurprisedPleased".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.RackBrainsVillage".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LifeInYourVillage".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LightAndPlayful".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SoManyYearsAgo".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.FoolsButHappy".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SmallTearForm".Localized());

            if (!sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.EmbarrassedLeave".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.BeforeWarmSmile".Localized());
        }

        public void TalkAmilyHerFamily(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PonderQuestionAgain".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SurprisedPleasedAgain".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.RackBrainsHerself".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.BeforeThisWorld".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LightAndPlayfulAgain".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NobodyAverageGirl".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NotGetOnFather".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HeLovedMe".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TellAboutMother".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HunterBestVillage".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.TakeHerHand".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DontKnowSniffs".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.LeaveMeAloneSobs".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.RespectPrivacy".Localized());

            if (!sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ChanceToMourn".Localized());
        }

        public void TalkAmilyTheAttack(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PonderQuestionThird".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SurprisedPleasedThird".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhatHappenedVillage".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ShockedThenSad".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.StartingToShiver".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SoManyHorrible".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.DrawBackRealWorld".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NoWeaponsArmed".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ThingsFlappingDark".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WrapArmsAround".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.StillHurtsRemembering".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SorryPainfulMemories".Localized());

            if (sexAfter)
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SentencePeriod".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExcuseOnceOkay".Localized());
        }

        public void TalkAmilySurvival(bool sexAfter)
        {
            bool amilyAtCamp = CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) != 0;

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PonderQuestionFourth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SurprisedPleasedFourth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HowSurviveVillage".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NotUpsetConfused".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhatMotherTaught".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AnyMoreQuestions".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ShakeHeadRefusal".Localized());

            if (!sexAfter)
            {
                if (!amilyAtCamp)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExcuseHeadBackCamp".Localized());
                else
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ExcuseSitBackCamp".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SentencePeriodQuote".Localized());
        }

        public void TalkAmilyBreeding()
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PonderQuestionFifth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SurprisedPleasedFifth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhyFatherChildren".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.CareWhatHappens".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.InsistWantKnow".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.CorruptBreedQuickly".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HowSupposedWork".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.GoblinsCorrupted".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhySoManyChildren".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NoSpecificNumber".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ThankExplanation".Localized());
        }

        public void TalkAmilyKitsune()
        {
            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SwapTalesNinth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SaysKitsune".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.WhatMeansThis".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ShareFewThings".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NodSeeSimilar".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NotStrongestBut".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.QuizzicalPress".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NeverSeenFace".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HowAnyoneKnows".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PausesAnotherFact".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.BlushFromExperience".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.NoddingPolitely".Localized());
        }

        public void TalkAmilyChildren(bool sexAfter)
        {
            bool amilyAtCamp = CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) != 0;

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PonderQuestionSixth".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SurprisedPleasedSixth".Localized());

            if (!amilyAtCamp)
            {
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PauseDelicately".Localized());

                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PuzzledAmusedOffended".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.PauseDelicatelyAgain".Localized());

                OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HintOfWorry".Localized());
            }

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.ReassuredNotEntirely".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.HeyHoldOnSecond".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.AdmitWasWorried".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.SweetOfYou".Localized());

            if (!sexAfter)
                if (!amilyAtCamp)
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.StillHereWavesGo".Localized());
                else
                    OutputText("Scenes.Amilyscene.TalkWithCuntIMeanAmily.StillHereLeaving".Localized());
        }

        public void StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT.ConvolutedRoute".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Amilyscene.StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT.SmallerEasierFollow".Localized());
            else
                if (Player.Tallness >= 84)
                    OutputText("Scenes.Amilyscene.StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT.SizeMakesTricky".Localized());

            OutputText("Scenes.Amilyscene.StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT.LedIntoBedroom".Localized());

            OutputText("Scenes.Amilyscene.StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT.ExaminingRoomSheepish".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeCharge".Localized(), FirstTimeAmilyTakeCharge);

            AddButton(1, "Common.Button.WaitForHer".Localized(), BeSomeKindofNervousDoucheAndWaitForAmily);

            AddButton(2, "Common.Button.KissHer".Localized(), KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay);
        }

        public void FirstTimeAmilyTakeCharge()
        {
            ClearOutput();

            AmilySprite(true);

            ShowImage("amily-forest-takecharge");

            AmilySprite();

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.SceneryDoesntMatter".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.BigIdeaProtest".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.SheWantedThis".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.GoesQuietGimlets".Localized());

            if (Player.Cocks[0].Length >= 14)
                if (Player.Cocks[0].Id == BodyPartTypes.Human || Player.Cocks[0].TypeIndex > 3)
                {
                    OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.EyesWideReveal".Localized());

                    OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.MakeItFit".Localized());
                }
                else
                    if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                    {
                        OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.HorrorDisgustFeatures".Localized());

                        OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.EnjoyWhenQuiet".Localized());
                    }
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                        {
                            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.StallionCockShock".Localized());

                            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.HowsSheKnow".Localized());
                        }
                        else
                            if (Player.HasKnot(0))
                            {
                                OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.CantBeSerious".Localized());

                                OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.LicksLipsImagine".Localized());
                            }

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.ThrustIntoHer".Localized());

            if (Player.Cocks[0].Length >= 14)
                OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.FailingToFit".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.DoesntFightHard".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.HipsThrustHarder".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.ClimaxOverPullOut".Localized());

            OutputText("Scenes.Amilyscene.FirstTimeAmilyTakeCharge.HappyAnytimeNeeds".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 5);

            AmilyPreggoChance();

            DoNext(() => ReturnToCamp(1));
        }

        public void BeSomeKindofNervousDoucheAndWaitForAmily()
        {
            ClearOutput();

            ShowImage("amily-forest-plainfuck");

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.BeSomeKindofNervousDoucheAndWaitForAmily.CuteButNotSure".Localized());

            OutputText("Scenes.Amilyscene.BeSomeKindofNervousDoucheAndWaitForAmily.PassionateNotFeeling".Localized());

            if (Player.Cocks[0].Length >= 14)
                OutputText("Scenes.Amilyscene.BeSomeKindofNervousDoucheAndWaitForAmily.LeadsToBedStrips".Localized());
            else
                OutputText("Scenes.Amilyscene.BeSomeKindofNervousDoucheAndWaitForAmily.GrinsStrokesIt".Localized());

            OutputText("Scenes.Amilyscene.BeSomeKindofNervousDoucheAndWaitForAmily.NoFurtherNeed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 2);

            AmilyPreggoChance();

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay()
        {
            ClearOutput();

            AmilySprite(true);

            ShowImage("amily-forest-kissingfuck");

            OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.SceneryNotRomantic".Localized());

            if (Player.Cocks[0].Length >= 14)
                OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.EyesWidenMesmerized".Localized());
            else
            {
                OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.LooksDownBlushing".Localized());

                OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.MemberRockHard".Localized());
            }

            OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.StartCaressing".Localized());

            OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.HastilyCorrects".Localized());

            OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.HappyAsOften".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.FurCheeksRed".Localized());
            else
                OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.NearlyResemblesImp".Localized());

            OutputText("Scenes.Amilyscene.KissAmilyInDaMoufFirstTimeIsSomehowBetterThatWay.WaveGoodbyeSmile".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 3);

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));

            AmilyPreggoChance();
        }

        public void AmilySexHappens()
        {
            ClearOutput();

            AmilySprite(true);

            int x = Player.CockThatFits(61);

            if (x == -1 && Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.AmilySexHappens.LooksBetweenLegsLaughs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 1);

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) == 0)
            {
                StickItInMouseTwatForTheFirstTimeNOTWORTHALLBULLSHIT();

                CoC.Instance.Flags.Set(GameFlagIds.AmilyFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) + 1);

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
            {
                OutputText("Scenes.Amilyscene.AmilySexHappens.EffortsBriskEfficient".Localized());

                OutputText("Scenes.Amilyscene.AmilySexHappens.HardSayDifferentRoute".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Business".Localized(), AmilySexBusiness);

                AddButton(1, "Common.Button.PlaytimeSt".Localized(), AmilySexPlaytimeFirst);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                {
                    var pregEvent = Player.Pregnancy.Event;

                    OutputText("Scenes.Amilyscene.AmilySexHappens.LeadsToNestQuickly".Localized());

                    if (pregEvent >= 1 && pregEvent <= 5)
                        OutputText("Scenes.Amilyscene.AmilySexHappens.TouchesLitterSwell".Localized());

                    OutputText("Scenes.Amilyscene.AmilySexHappens.ParagraphBreak".Localized());

                    OutputText("Scenes.Amilyscene.AmilySexHappens.PushesOntoBedding".Localized());

                    if (pregEvent >= 6)
                        OutputText("Scenes.Amilyscene.AmilySexHappens.ConfidenceSlipsBare".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.StepIn".Localized(), AmilyStepTheFuckIn);

                    AddButton(1, "Common.Button.WatchShow".Localized(), AmilyEnjoyShow);
                }
                else
                    if (Player.Pregnancy.Event >= 6)
                        FuckAmilyPreg();
                    else
                        AmilyHighAffectionSecks();
        }

        public void AmilySexBusiness()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilySexBusiness.UndressesQuickly".Localized());

            AmilySexPtII();
        }

        public void AmilySexPlaytimeFirst()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilySexPlaytimeFirst.BeginsReachingClothes".Localized());

            OutputText("Scenes.Amilyscene.AmilySexPlaytimeFirst.AsksCuriousWary".Localized());

            OutputText("Scenes.Amilyscene.AmilySexPlaytimeFirst.SmileUndressHer".Localized());

            OutputText("Scenes.Amilyscene.AmilySexPlaytimeFirst.GrowlsBlushingFaintly".Localized());

            OutputText("Scenes.Amilyscene.AmilySexPlaytimeFirst.WarnsAboutPickingUp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            AmilySexPtII();
        }

        public void AmilySexPtII()
        {

            AmilySprite();

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                OutputText("Scenes.Amilyscene.AmilySexPtII.ShrieksInfested".Localized());

                OutputText("Scenes.Amilyscene.AmilySexPtII.HighTailItOut".Localized());

                DoNext(() => ReturnToCamp(1));

                CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 3);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyGrossedOutByWorms, 1);

                return;
            }

            OutputText("Scenes.Amilyscene.AmilySexPtII.BothNakedStrokes".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.SitWatch".Localized(), SitAndWatchAmilySex);

            AddButton(1, "Common.Button.CaressHer".Localized(), CaressAmilyHaveSex);
        }

        public void SitAndWatchAmilySex()
        {
            ClearOutput();

            AmilySprite();

            int x = Player.CockThatFits(61);

            OutputText("Scenes.Amilyscene.SitAndWatchAmilySex.StaySpoilShow".Localized(
                Player.CockDescript(x)
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            ContinueAmilySmex();
        }

        public void CaressAmilyHaveSex()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.CaressAmilyHaveSex.WatchesMasturbateErotic".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RefuseKiss".Localized(), AmilyGetKissed);

            AddButton(1, "Common.Button.KissHer".Localized(), AmilyTakeTheKiss);
        }

        public void AmilyGetKissed()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyGetKissed.PullMindBack".Localized());

            ContinueAmilySmex();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) - 3);
        }

        public void AmilyTakeTheKiss()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyTakeTheKiss.ConveyWillStop".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilyTakeTheKiss.UnusualExperience".Localized());

            OutputText("Scenes.Amilyscene.AmilyTakeTheKiss.ParagraphBreak".Localized());

            ContinueAmilySmex();

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 1 + Random.Shared.Next(3));
        }

        public void ContinueAmilySmex()
        {

            int x = Player.CockThatFits(61);

            AmilySprite(true);

            ShowImage("amily-forest-plainfuck");

            OutputText("Scenes.Amilyscene.ContinueAmilySmex.TimeRightSinkBedding".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.ContinueAmilySmex.GoodThingsEndClimax".Localized());

            OutputText("Scenes.Amilyscene.ContinueAmilySmex.MousegirlSaysBlush".Localized());

            OutputText("Scenes.Amilyscene.ContinueAmilySmex.SurprisedSheEnjoyed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) + 1);

            AmilyPreggoChance();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 1 + Random.Shared.Next(2));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyEnjoyShow()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyEnjoyShow.CuriousArousedWatches".Localized());

            if (Player.Pregnancy.Event >= 6)
                OutputText("Scenes.Amilyscene.AmilyEnjoyShow.NervousGravidState".Localized());

            OutputText("Scenes.Amilyscene.AmilyEnjoyShow.ParagraphBreak".Localized());

            AmilyMidSexLevel2();
        }

        public void AmilyStepTheFuckIn()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyStepTheFuckIn.EagerHelpsUndress".Localized());

            AmilyMidSexLevel2();
        }

        public void AmilyMidSexLevel2()
        {

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyMidSexLevel2.CompletelyNakedExcited".Localized());

            OutputText("Scenes.Amilyscene.AmilyMidSexLevel2.StripteaseOrStrip".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Striptease".Localized(), StripForAmilyYouSlut);

            AddButton(1, "Common.Button.Business".Localized(), GetDownWithSexTiem);
        }

        public void StripForAmilyYouSlut()
        {
            ClearOutput();

            AmilySprite();

            int x = Player.CockThatFits(61);

            OutputText("Scenes.Amilyscene.StripForAmilyYouSlut.TurnMischievousSmile".Localized(
                Player.CockDescript(x)
            ));

            ContinueWithMoreMidLevelAmilySex();
        }

        public void GetDownWithSexTiem()
        {
            ClearOutput();

            AmilySprite();

            int x = Player.CockThatFits(61);

            OutputText("Scenes.Amilyscene.GetDownWithSexTiem.TooHornyStrips".Localized(
                Player.CockDescript(x)
            ));

            ContinueWithMoreMidLevelAmilySex();
        }

        public void ContinueWithMoreMidLevelAmilySex()
        {

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            AmilySprite();

            OutputText("Scenes.Amilyscene.ContinueWithMoreMidLevelAmilySex.NakedEmbraceKiss".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.PlayAlong".Localized(), PlayAlongWithAmilyWhataDumbBitch);

            AddButton(1, "Common.Button.PleaseHer".Localized(), WorkToPleaseTheCunt);
        }

        public void PlayAlongWithAmilyWhataDumbBitch()
        {
            ClearOutput();

            AmilySprite(true);

            ShowImage("amily-forest-reverse-cowgirl");

            OutputText("Scenes.Amilyscene.PlayAlongWithAmilyWhataDumbBitch.LetHerDominate".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            AmilyMiddleGradeSexOver();
        }

        public void WorkToPleaseTheCunt()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.WorkToPleaseTheCunt.TakeActiveRole".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            AmilyMiddleGradeSexOver();
        }

        public void AmilyMiddleGradeSexOver()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyMiddleGradeSexOver.SpentSinksBreast".Localized());

            AmilyPreggoChance();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 3 + Random.Shared.Next(4));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.SayGoodbye".Localized(), SayGoodByeToAmilyPostSecks);

            AddButton(1, "Common.Button.StayAWhile".Localized(), StayAfterAmilyMiddleGradeSecks);
        }

        public void SayGoodByeToAmilyPostSecks()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.SayGoodByeToAmilyPostSecks.KissThenGoodbye".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void StayAfterAmilyMiddleGradeSecks()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.StayAfterAmilyMiddleGradeSecks.RatherStayLonger".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyHighAffectionSecks()
        {

            AmilySprite(true);

            int x = Player.CockThatFits(61);

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.DidntWasteTime".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.PregnantStateFast".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.UndoClothesInside".Localized(
                Appearance.CockNoun(Player.Cocks[x].Id)
            ));

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.OnlyBellyBulge".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.FeelsErectionHot".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.KeepsGripKissing".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.DistanceFlopsBed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.SucksClit".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.LicksHumanCock".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.MoansSpreadsLegs".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.StopsAtBump".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.HoversAtBreasts".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.GrownFromPregnancy".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.LicksNipples".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.MilkDribbleSurprise".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ParagraphBreakThird".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ClingsTeasingNeck".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ApologyPushesIn".Localized(
                Player.CockDescript(x)
            ));

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.MindfulBabyBump".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.RegularPaceFuckhole".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.MoansRaisesHips".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.FillMeUpPlea".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.NoNeedHoldBack".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.WhispersSultryEdge".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.CumSpurtsAir".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.SentencePeriodParagraphBreak".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.DidntKnockUp".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.LuckyVirileFather".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.CatchesBreathRuffles".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.LieTogetherRegret".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.SureYoullReturn".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.BetterComeBackDear".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.RubsStomachPromises".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2 + Random.Shared.Next(4));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));

            AmilyPreggoChance();
        }

        public void FuckAmilyPreg()
        {
            ClearOutput();

            AmilySprite(true);

            ShowImage("amily-forest-fuckpreg");

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.LeadsByHand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.FuckAmilyPreg.ErectionTentsPants".Localized());

            OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.HelpUndressSafeSpot".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.PleasedByNakedForm".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.PullsHeadKiss".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Amilyscene.FuckAmilyPreg.WrapsTailChuckle".Localized());
            else
                OutputText("Scenes.Amilyscene.FuckAmilyPreg.TicklingSnortGiggle".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.MilkOnNipples".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.SalivaStrandsLook".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.ResearchedStraddles".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.TeasesPussyLips".Localized(
                Appearance.CockNoun(BodyPartTypes.Human)
            ));

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.ImpressiveRhythmRides".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.FuckAmilyPreg.MouseCockSpews".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.SentencePeriodParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.FuckAmilyPreg.TeasesBlush".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.SitsUpReclines".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.SadnessLeavesLover".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2 + Random.Shared.Next(4));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));

            AmilyPreggoChance();
        }

        public void AmilyPreggoChance()
        {

            if ((CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1) && (CoC.Instance.Flags.Get(GameFlagIds.AmilyAllowsFertility) == 0))
                    return;

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) != 0)
                return;

            if (Player.KnockUpChance(25, 100))
                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.MouseIncubation - 182, 100, 1);
        }

        public void AmilyFollowerEncounter()
        {
            ClearOutput();

            if (!AmilyCorrupt() && Player.Eggs() >= 20 && Player.CanOviposit() && CoC.Instance.Flags.Get(GameFlagIds.AmilyOvipositionUnlocked) == 0)
            {
                AmilyEggStuff();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyIncestCountdownTimer) == 30 * 24 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2 && (View.Time.Hours >= 11 && View.Time.Hours <= 13))
            {
                AmilyIncest();

                return;
            }

            AmilySprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) == "")
                CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, "rags");

            if (!Player.IsPureEnough(50) && CoC.Instance.Flags.Get(GameFlagIds.AmilyCampCorruptionFreaked) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                AmilyTaintWarning();

                return;
            }

            if (Player.IsPureEnough(50) && CoC.Instance.Flags.Get(GameFlagIds.AmilyCampCorruptionFreaked) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyCampCorruptionFreaked, 0);

            if (Player.IsPregnant() && Player.Pregnancy.Incubation == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
            {
                AmilyPopsOutKidsInCamp();

                Player.ClearPregnancy();

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyMetPureJojo) == 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilySparWithPureJojo) == 0 && FollowerRegistry.HasFollower(FollowerIds.JojoFollower))
            {
                SceneInstance<FollowerInteractionsScene>.Get().PureJojoAndAmilySpar();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWaitForPcFixJojo) == 1 && Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)) && CoC.Instance.Flags.Get(GameFlagIds.AmilyBlockCountdownBecauseCorruptedJojo) == 0 && CoC.Instance.Flags.Get(GameFlagIds.JojoFixedStatus) == 0)
            {
                SceneInstance<FollowerInteractionsScene>.Get().FixJojoOOOOHYEEEEAHSNAPINTOASLIMJIM();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.ApproachEarsTwitch".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) == 0)
                    OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.YellRushesOver".Localized());
                else
                {
                    OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.KneelsBeforeName".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmilyGibsMilk) == 1)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmAmilyGibsMilk, 2);

                        OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.HoldsHandMilk".Localized());

                        OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.RegularProductionPayment".Localized());

                        InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk), AmilyFollowerEncounter);

                        return;
                    }
                    else
                    {
                        AmilyMenu(false);

                        return;
                    }
                }

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 2)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                    OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.NoticesIllAsks".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.NoticesIllHappy".Localized(
                        Player.Mf("master", "mistress")
                    ));

            if (__sw0_0 == 3 || __sw0_0 == 4)
                OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.BellyStartsProtrude".Localized());

            if (__sw0_0 == 5 || __sw0_0 == 6)
                OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.BellyVeryBig".Localized());

            if (__sw0_0 == 7)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                    OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.StomachMovesPat".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.StomachMovesStroke".Localized());

            if (__sw0_0 == 8)
                OutputText("Scenes.Amilyscene.AmilyFollowerEncounter.BulgeWrigglesSquirms".Localized());

            AmilyMenu(true);
        }

        public void AmilyMenu(bool output = true)
        {

            ClearButtons();

            var bText = "";

            Action babies = null;

            var bTooltip = "";

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                if (Player.HasCock() || (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0))
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAllowsFertility) == 0)
                    {
                        OutputText("Scenes.Amilyscene.AmilyMenu.SterileHerbs".Localized());

                        bText = "Make Babies";

                        babies = MakeChildren;

                        bTooltip = "Request Amily to stop taking her herbs and allow her to get pregnant.";

                        if (SceneInstance<IzmaScene>.Get().IzmaFollower() && CoC.Instance.Flags.Get(GameFlagIds.AmilyXIzmaPotion3some) == 0 && Player.HasCock() && !Player.IsPregnant() && !Player.IsPregnant())
                            babies = AmilyXIzmaSuperPregOptions;
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.AmilyMenu.OffHerbsFertile".Localized());

                        bText = "No Babies";

                        babies = MakeChildren;

                        bTooltip = "Request Amily to start taking her herbs and prevent Player.Pregnancy.";
                    }

                AddButton(0, "Common.Button.Appearance".Localized(), AmilyAppearance);

                AddButton(1, "Common.Button.Talk".Localized(), TalkToAmilyCamp);

                AddButton(2, "Common.Button.MakeLove".Localized(), FuckTheMouseBitch);

                AddButton(5, "Common.Button.GivePresent".Localized(), AmilyGiftMenu);

                AddButton(6, bText, babies, bTooltip);

                if (!noFur)
                    AddRowButton(1, (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 ? "Common.Button.Defur".Localized() : "Common.Button.Refuzz".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 ? AmilyDefurryOfferAtCamp : RefuzzAmily));

                if (Holidays.IsWinter())
                    AddRowButton(1, "Cuddle", AmilyWinterCuddles);

                if (View.Time.Hours > 20)
                    AddButton(9, "Common.Button.SleepWith".Localized(), AmilySleepWith);

                AddButton(10, "Scenes.Dullahan.DullMenu.ButtonDates".Localized(), DatesMenu);
                // 原版 AmilyScene.hx:2821：Kiha 尝过糟糕料理（== 1）后解锁烹饪课程
                if (KihaFollowerScene.Save.GetInt("badCookingTasted", 0) == 1)
                    AddRowButton(2, "Cooking", AmilyCookingLesson);

                if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) > 0 && CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) < 100)
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyDestroyingCorruptedGlades) == 0)
                        AddRowButton(2, "Destroy Glades", AmilyDestroyGladesToggle);
                    else
                        AddRowButton(2, "Stop Destroying", AmilyDestroyGladesToggle);

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)) && !(CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) < 0))
                    AddRowButton(2, "Strange Potion", IdLiddellium);

                if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !((Areas.Swamp.AlrauneScene.Save.GetInt("questAsked", 0) & Areas.Swamp.AlrauneScene.ASKEDAM) != 0))
                    AddRowButton(2, "Vines", null );

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
            }
            else
            {
                AddButton(0, "Common.Button.Appearance".Localized(), AmilyAppearance);

                AddButton(1, "Common.Button.Talk".Localized(), () => TalkWithCORRUPTCUNT());

                AddButton(2, "Common.Button.Sex".Localized(), FuckTheMouseBitch);

                AddButton(5, "Common.Button.GiveItem".Localized(), GiveAmilyAPresent);

                if (!noFur)
                    AddButton(6, (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 ? "Common.Button.Defur".Localized() : "Common.Button.Refuzz".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 ? AmilyDefurryOfferAtCamp : RefuzzAmily));

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

                if (!Player.IsPregnant() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                    AddButton(10, "Common.Button.FarmWork".Localized(), SendCorruptCuntToFarm);

                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) == 1)
                {
                    AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);

                    if (CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionAmily) == 0)
                        AddButton(11, "Common.Button.HarvestMilk".Localized(), HarvestMilk);
                    else
                        AddButton(11, "Common.Button.StopHarvest".Localized(), StopHarvestingMilk);

                    AddButton(14, "Common.Button.Back".Localized(), null );
                }
            }

            HintButton(0, "Examine Amily's appearance.");

            HintButton(1, "Discuss with Amily about something");

            HintButton(2, "Proposition Amily for some sexy-times.");

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryRatMice);
        }

        public void DatesMenu()
        {

            ClearButtons();

            AddNextButton("Common.Button.Forest".Localized(), ForestDate);

            if ((int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 5 || UrtaQuestScene.UrtaLove()) && !SceneInstance<UrtaQuestScene>.Get().UrtaBusy())
                AddRowButton(2, "Tel'Adre", DateNightFirstTime);

            SetExitButton("Common.Button.Back".Localized(), () => AmilyMenu(true));
        }

        public void AmilyCorruptSexMenu()
        {

            AmilySprite();

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Amilyscene.AmilyCorruptSexMenu.AsksCumBucket".Localized());

                ClearButtons();

                if (Player.HasCock())
                {
                    AddButton(0, "Common.Button.Anal".Localized(), CorruptAmilyBuckFutter);

                    AddButton(1, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), CorruptAmilyOralSuckOff);

                    AddButton(5, "Common.Button.Vagina".Localized(), CorruptAmilysPussyGetsMotherfuckingFucked);
                }

                if (Player.HasVagina())
                {
                    AddButton(2, "Common.Button.GetLicked".Localized(), CorruptAmilyLickPussiesLikeAPro);

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                        AddButton(3, "Common.Button.Getpened".Localized(), CorruptAmilyCampBonesPCWithHerCock);

                    AddButton(4, "Common.Button.Scissor".Localized(), CorruptAmilyScissorsLikeAPro);
                }

                AddButton(14, "Common.Button.Back".Localized(), () => AmilyFollowerEncounter());
            }
            else
            {
                OutputText("Scenes.Amilyscene.AmilyCorruptSexMenu.EagerlyPresentsSlut".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                OutputText("Scenes.Amilyscene.AmilyCorruptSexMenu.GropesNoGenitalia".Localized());

                OutputText("Scenes.Amilyscene.AmilyCorruptSexMenu.CollapsesRedresses".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, -20f));

                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) == 0)
                    DoNext(PlayerMenu);
                else
                    DoNext(FarmCorruption.RootScene);
            }
        }

        public void AmilyAppearance()
        {
            ClearOutput();

            AmilySprite();

            var stopSayingNetherlipsFuck = "its hairless netherlips seem to shine in between her fur";

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 1)
                stopSayingNetherlipsFuck = "its netherlips seem to shine in contrast to her thighs";

            var what = AmilyFurry() ? "Common.Text.MouseMorph".Localized() : "Common.Text.MouseGirl".Localized();

            var onHerMuzzle = AmilyFurry() ? "on her furry little muzzle " : "";

            var furDesc = "";

            if (AmilyFurry())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                    furDesc = "She is covered in fur, except on her hands, feet, ears and tail.";
                else
                    furDesc = "She is covered in fur, except on her hands, feet, ears and tail. She has dainty, surprisingly hand-like pink paws instead of feet and";
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                {
                }

                furDesc = "Her face is framed by a stylish bob of auburn hair, large mouse-ears jutting from her 'do. She has small, dainty feet and";
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                OutputText("Scenes.Amilyscene.AmilyAppearance.HeightBuild".Localized(
                    what,

                    onHerMuzzle,

                    furDesc,

                    Save.GetString("ringType") != "" ? " She has the " + Save.GetString("ringType") + " ring you gifted her on her finger. You're positive she's never taken it off since you gave it to her." : "",

                    AmilyHips(),

                    AmilyButt()
                ));

                OutputText("Scenes.Amilyscene.AmilyAppearance.ChestPairNipples".Localized(
                    AmilyTits(),

                    CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) > 0 ? CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) + "-inch" : "cute little",

                    CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 0 ? "and must be at least " + Player.BreastCupForRating(CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize)) + "s" : "but wouldn't be able to fill out a bra"
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    var dickDesc = AmilyFurry() ? "Common.Text.NakedAndHairlessAndPinkAnd".Localized() : "Common.Text.PinkAndThrobbing".Localized();

                    OutputText("Scenes.Amilyscene.AmilyAppearance.PenisReplacesClit".Localized(
                        CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength),

                        dickDesc
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) == 0)
                        OutputText("Scenes.Amilyscene.AmilyAppearance.ForeskinNotSheath".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyAppearance.ThoughDoesHave".Localized());

                    OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ParagraphBreakFifth".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                {
                    OutputText("Scenes.Amilyscene.AmilyAppearance.DanglingBalls".Localized(
                        AmilyBalls()
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                        OutputText("Scenes.Amilyscene.AmilyAppearance.UnderShaft".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyAppearance.FromGroin".Localized());

                    OutputText("Scenes.Amilyscene.AmilyAppearance.SentencePeriodParagraphBreak".Localized());
                }

                OutputText("Scenes.Amilyscene.AmilyAppearance.LittlePinkPussy".Localized(
                    stopSayingNetherlipsFuck
                ));
            }
            else
            {
                OutputText("Scenes.Amilyscene.AmilyAppearance.HeightBuildAgain".Localized(
                    what,

                    onHerMuzzle,

                    furDesc,

                    AmilyHips(),

                    AmilyButt()
                ));

                OutputText("Scenes.Amilyscene.AmilyAppearance.ChestPairNipplesAgain".Localized(
                    AmilyTits(),

                    CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) > 0 ? CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) + "-inch" : "cute little",

                    CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 0 ? "and must be at least " + Player.BreastCupForRating(CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize)) + "s" : "but wouldn't be able to fill out a bra"
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    OutputText("Scenes.Amilyscene.AmilyAppearance.HalfErectPenis".Localized(
                        CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength)
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) == 0)
                        OutputText("Scenes.Amilyscene.AmilyAppearance.ForeskinNotSheathAgain".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyAppearance.ThoughDoesHaveAgain".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                {
                    OutputText("Scenes.Amilyscene.AmilyAppearance.DanglingBallsAgain".Localized(
                        AmilyBalls()
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                        OutputText("Scenes.Amilyscene.AmilyAppearance.UnderShaftAgain".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyAppearance.FromGroinAgain".Localized());

                    OutputText("Scenes.Amilyscene.AmilyAppearance.ChurningWithCum".Localized());
                }

                OutputText("Scenes.Amilyscene.AmilyAppearance.AmilyHasA".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) == 2)
                    OutputText("Scenes.Amilyscene.AmilyAppearance.MoistWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) == 3)
                    OutputText("Scenes.Amilyscene.AmilyAppearance.WetWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) == 4)
                    OutputText("Scenes.Amilyscene.AmilyAppearance.DrippingWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) >= 5)
                    OutputText("Scenes.Amilyscene.AmilyAppearance.SoakedWord".Localized());

                OutputText("Scenes.Amilyscene.AmilyAppearance.PinkPussyLegs".Localized(
                    stopSayingNetherlipsFuck
                ));

                if (FarmCorruption.hasTattoo("amily"))
                {
                    OutputText("Scenes.Amilyscene.AmilyHighAffectionSecks.ParagraphBreakSixth".Localized());

                    if (FarmCorruption.amilyFullTribalTats())
                        OutputText("Scenes.Amilyscene.AmilyAppearance.CoveredTattoos".Localized());
                    else
                    {
                        if (FarmCorruption.numTattoos("amily") > 1)
                            OutputText("Scenes.Amilyscene.AmilyAppearance.FollowingTattoosList".Localized());
                        else
                            OutputText("Scenes.Amilyscene.AmilyAppearance.SheHasWord".Localized());

                        if (CoC.Instance.Flags.Has(GameFlagIds.AmilyTattooCollarbone))
                            OutputText("Scenes.Amilyscene.AmilyAppearance.PlaceholderNewline".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.AmilyTattooCollarbone)
                            ));

                        if (CoC.Instance.Flags.Has(GameFlagIds.AmilyTattooShoulders))
                            OutputText("Scenes.Amilyscene.AmilyAppearance.PlaceholderNewlineAgain".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.AmilyTattooShoulders)
                            ));

                        if (CoC.Instance.Flags.Has(GameFlagIds.AmilyTattooLowerback))
                            OutputText("Scenes.Amilyscene.AmilyAppearance.PlaceholderNewlineThird".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.AmilyTattooLowerback)
                            ));

                        if (CoC.Instance.Flags.Has(GameFlagIds.AmilyTattooButt))
                            OutputText("Scenes.Amilyscene.AmilyAppearance.PlaceholderNewlineFourth".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.AmilyTattooButt)
                            ));
                    }
                }
            }

            OutputText("Scenes.Amilyscene.AmilyAppearance.TinyPinkPucker".Localized());

            DisableButtonByLabel("Appearance");

        }

        public void AmilyDestroyGladesToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyDestroyingCorruptedGlades) == 0)
            {
                OutputText("Scenes.Amilyscene.AmilyDestroyGladesToggle.AsksDestroyGlades".Localized());

                OutputText("Scenes.Amilyscene.AmilyDestroyGladesToggle.GrinsWillDestroy".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyDestroyingCorruptedGlades, 1);
            }
            else
            {
                OutputText("Scenes.Amilyscene.AmilyDestroyGladesToggle.TellsNotRequired".Localized());

                OutputText("Scenes.Amilyscene.AmilyDestroyGladesToggle.SaysMightComeBack".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyDestroyingCorruptedGlades, 0);
            }

            DoNext(() => AmilyFollowerEncounter());
        }

        public void TalkToAmilyCamp()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.TalkToAmilyCamp.WantsToTalk".Localized());

            TalkWithCuntIMeanAmily();
        }

        public void TalkToAmilyWithSexAfter()
        {

            TalkWithCuntIMeanAmily(true);
        }

        public void FuckTheMouseBitch()
        {
            ClearOutput();

            AmilySprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
            {
                AmilyCorruptSexMenu();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWaitForPcFixJojo) > 0)
            {
                OutputText("Scenes.Amilyscene.FuckTheMouseBitch.PushesNotUntilJojo".Localized());

                DoNext(() => AmilyFollowerEncounter());

                return;
            }

            OutputText("Scenes.Amilyscene.FuckTheMouseBitch.SeductiveSmileWants".Localized());

            if (Random.Shared.Next(2) == 0)
                OutputText("Scenes.Amilyscene.FuckTheMouseBitch.GrinsWickedly".Localized());
            else
                OutputText("Scenes.Amilyscene.FuckTheMouseBitch.GigglesWhatKept".Localized());

            OutputText("Scenes.Amilyscene.FuckTheMouseBitch.LaughsSeventyKids".Localized());

            OutputText("Scenes.Amilyscene.FuckTheMouseBitch.StridesArmsAsks".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeCharge".Localized(), AmilyTakesChargeSex);

            AddButton(1, "Common.Button.AmilyLeads".Localized(), LetAmilyLead);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 4 && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 0 && !SceneInstance<UrtaQuestScene>.Get().UrtaBusy())
                AddButton(2, "Common.Button.Urta".Localized(), null );

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyOwnsBikini) > 0 && Player.HasCock() && !AmilyCorrupt())
                AddButton(3, "Common.Button.Swim".Localized(), AmilySwimFuckIntro);

            if (SceneInstance<IzmaScene>.Get().IzmaFollower() && CoC.Instance.Flags.Get(GameFlagIds.AmilyXIzmaPotion3some) > 0 && Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.FuckTheMouseBitch.CouldTryPotion".Localized());

                AddButton(4, "Common.Button.Izma3some".Localized(), DrinkThePotion);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.GivenAmilyNurseOutfit) > 0 && Player.HasCockThatFits(61))
            {
                OutputText("Scenes.Amilyscene.FuckTheMouseBitch.MightPlayNurse".Localized());

                AddButton(5, "Common.Button.Nurseroleplay".Localized(), AmilyNurseCheckup);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyOvipositionUnlocked) > 0 && Player.CanOviposit())
                AddNextButton("Common.Button.LayEggs".Localized(), LayEggsInAmily);

            AddButton(14, "Common.Button.Back".Localized(), AmilyFollowerEncounter);
        }

        public void AmilyTakesChargeSex()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyTakesChargeSex.StrideKissDeeply".Localized());

            OutputText("Scenes.Amilyscene.AmilyTakesChargeSex.WhatWillYouDo".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(61))
                    AddButton(0, "Common.Button.Fuck".Localized(), TakeChargeAmilyFuck);
                else
                    AddButton(0, "Common.Button.Fuck".Localized(), () => TakeChargeAmilyFuckTooLarge(false));

                if (Player.HasCockThatFits(50))
                    AddButton(1, "Common.Button.ButtFuck".Localized(), FuckPureAmilysHeiny);
                else
                    AddButton(1, "Common.Button.ButtFuck".Localized(), () => TakeChargeAmilyFuckTooLarge(true));

                AddButton(2, "Common.Button.GetSucked".Localized(), TakeChargeAmilyGetSucked);
            }

            if (Player.HasVagina())
            {
                AddButton(3, "Common.Button.Scissor".Localized(), TakeChargeAmilyScissorMeTimbers);

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    AddButton(4, "Common.Button.MountHer".Localized(), TakeChargeAmilyMountHer);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) > 0 || CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) >= 1)
                AddButton(5, "Combat.Ability.MinotaurLordMinotaurDrankMalk.Name".Localized(), TakeChargeAmilyMouseMilk);

            AddButton(6, "Common.Button.EatOut".Localized(), TakeChargeAmilyEatOut);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                AddButton(7, "Common.Button.CatchAnal".Localized(), PureAmilyPutsItInYourRectumDamnNearKilledEm);

            if (OptionStore.Instance.Get("watersports", 0) > 0 && Player.HasCock())
                AddButton(8, "Common.Button.Watersports".Localized(), AmilyCockWatersports);

            AddButton(14, "Common.Button.Back".Localized(), () => FuckTheMouseBitch());
        }

        public void AmilyCockWatersports()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.LocksEyesArm".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.MinutesBlissfulHours".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.LowersToBedroll".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.SteadyPaceGyrates".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.FingersClench".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.MusclesSpasmSlow".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.RaisesHandHead".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.ConcernPupilsSwell".Localized());

            OutputText("Scenes.Amilyscene.AmilyCockWatersports.RealizationGripsHand".Localized());

            Player.Orgasm();

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeChargeAmilyFuckTooLarge(bool type)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckTooLarge.NaughtySmirkGlance".Localized(
                Player.CockDescriptShort(0)
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckTooLarge.DisappointedPerks".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckTooLarge.RestrictJustBit".Localized());

            AddButton(0, "Common.Button.Okay".Localized(), TakeChargeAmilyFuckCockhead);

            AddButton(1, "Common.Button.Deeper".Localized(), () => DigDeeper(type));
        }

        public void DigDeeper(bool type)
        {
            ClearOutput();

            Player.Orgasm("Dick");

            AmilySprite(true);

            ClearButtons();

            OutputText("Scenes.Amilyscene.DigDeeper.DoesntTakeLong".Localized(
                Player.CockDescriptShort(0)
            ));

            if (Player.BiggestCockLength() <= 30)
                OutputText("Scenes.Amilyscene.DigDeeper.ToBellyButton".Localized());

            if (Player.BiggestCockLength() <= 40)
                OutputText("Scenes.Amilyscene.DigDeeper.BetweenBreasts".Localized());
            else
                OutputText("Scenes.Amilyscene.DigDeeper.PastHerFace".Localized());

            OutputText("Scenes.Amilyscene.DigDeeper.GuidesTipIn".Localized(
                Player.CockDescriptShort(0)
            ));

            OutputText("Scenes.Amilyscene.DigDeeper.MakesMove".Localized());

            OutputText("Scenes.Amilyscene.DigDeeper.PowerfulThrustDeeper".Localized());

            OutputText("Scenes.Amilyscene.DigDeeper.ComplainsTooDeep".Localized());

            OutputText("Scenes.Amilyscene.DigDeeper.NotDeepEnough".Localized());

            AddButton(0, "Common.Button.DeepEnough".Localized(), () => DugEnough(type));

            if (Player.Corruption + Player.CorruptionTolerance() >= 60 && Player.BiggestCockLength() >= 40)
                AddButton(1, "Common.Button.NotDeepEnough".Localized(), () => DigTooDeep(type));
        }

        public void DigTooDeep(bool type)
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            ClearButtons();

            OutputText("Scenes.Amilyscene.DigTooDeep.PulsePoundsFaster".Localized(
                Player.CockDescriptShort(0)
            ));

            OutputText("Scenes.Amilyscene.DigTooDeep.WrigglesStareBulge".Localized());

            OutputText("Scenes.Amilyscene.DigTooDeep.AnswersWithThrust".Localized());

            OutputText("Scenes.Amilyscene.DigTooDeep.BegsStopWhimper".Localized());

            OutputText("Scenes.Amilyscene.DigTooDeep.PleasFadeGaze".Localized());

            AddButton(0, "Common.Button.Stop".Localized(), () => StopDigging(type));

            AddButton(1, "Common.Button.Continue".Localized(), () => KeepDeepFuck(type));

            AddButton(2, "Common.Button.Deeper".Localized(), () => DigEvenDeeperOhGod(type));
        }

        public void DigEvenDeeperOhGod(bool type)
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            ClearButtons();

            OutputText("Scenes.Amilyscene.DigEvenDeeperOhGod.ListeningSomethingSnaps".Localized());

            OutputText("Scenes.Amilyscene.DigEvenDeeperOhGod.PleasGoQuiet".Localized());

            OutputText("Scenes.Amilyscene.DigEvenDeeperOhGod.BegsGraspsShoulders".Localized());

            OutputText("Scenes.Amilyscene.DigEvenDeeperOhGod.GrossNoiseEscapes".Localized());

            OutputText("Scenes.Amilyscene.DigEvenDeeperOhGod.StammersCant".Localized());

            AddButton(1, "Common.Button.Stop".Localized(), StopDiggingEvenDeeper);

            AddButton(2, "Common.Button.Deeper".Localized(), () => DigEvenEvenDeeperOhGod(type));
        }

        public void StopDiggingEvenDeeper()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.PleaReachesMind".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.ShudderingPainfulTightness".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.PainfullySlowExtract".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.RushToSide".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.SlapWord".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.HitsGroundCheek".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.SlapWordAgain".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.SprawlingAnotherBlow".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.ScreamsWhatDoing".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.QuicklyTearsIn".Localized());

            OutputText("Scenes.Amilyscene.StopDiggingEvenDeeper.KindnessNotRecalled".Localized());

            AddButton(1, "Common.Button.Apologize".Localized(), SorryForImpaling);

            AddButton(2, "Common.Button.ShutHerUp".Localized(), NotSorryForImpaling);
        }

        public void DigEvenEvenDeeperOhGod(bool type)
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 15f));

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.IgnoringCriesThrusts".Localized());

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.LicksDroolAsks".Localized());

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.WerentNearlyDone".Localized());

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.StretchesBulgeSkin".Localized());

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.ThreeShout".Localized());

            if (!type)
            {
                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.GroansFinalThrust".Localized());

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.LimpThenConvulses".Localized(
                    Player.CockDescriptShort()
                ));

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.FightMindGone".Localized());

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.NearingSatisfaction".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.BucksEmptiesInside".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.FinalThrustMoan".Localized());

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.PausesToBask".Localized());

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.RetchesHoldsFirm".Localized());

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.DickMoreErect".Localized());

                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.HangsPitifully".Localized());
            }

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.FallsToKnees".Localized());

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.ReleasePullHerOff".Localized());

            if (type)
                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.MouthSlackGapes".Localized());
            else
                OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.TongueHangsPussy".Localized());

            OutputText("Scenes.Amilyscene.DigEvenEvenDeeperOhGod.CumDripsPools".Localized());

            DoNext(() => TooDeepAmilyLeaves());
        }

        public void NotSorryForImpaling()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.BitchyDroning".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.TipEntersStunned".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.MuffledScreamsVibrate".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.HandsOnFaceWrestle".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.ThroatTightnessSqueezes".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.RealizationNoPulling".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.GripsThroatWorks".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.RapidlyLosingStrength".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.SheerGirthBeyond".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.FlailingStops".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.ThrustsMouthAnus".Localized());

            OutputText("Scenes.Amilyscene.NotSorryForImpaling.WetSoundSlidesOff".Localized());

            DoNext(() => TooDeepAmilyLeaves());
        }

        public void SorryForImpaling()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.SorryForImpaling.HangingHeadApology".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgeStrikes) == 0)
            {
                OutputText("Scenes.Amilyscene.SorryForImpaling.InsistsTearsWelling".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.UnsureWhatToSay".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.AssureHerMemoryHangs".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.ReachOutAgain".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.SheCriesCantLive".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.AngerFadesSeesTears".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.PromiseNeverAgain".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyBulgePromise, 1);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgeStrikes) >= 1)
            {
                OutputText("Scenes.Amilyscene.SorryForImpaling.SheFumesDeniesLie".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgePromise) == 1)
                    OutputText("Scenes.Amilyscene.SorryForImpaling.WhimpersThenScreams".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.TouchShoulderRejected".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.GaspSheMovesAway".Localized());

                OutputText("Scenes.Amilyscene.SorryForImpaling.SeesHerSilhouette".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyBulgeStrikes, CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgeStrikes) + 2);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgeStrikes) >= 3)
                DoNext(() => TooDeepAmilyLeaves());
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void StopDigging(bool type)
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.StopDigging.SensesSuddenlyReturn".Localized());

            OutputText("Scenes.Amilyscene.StopDigging.ReleasedFromRodGasps".Localized());

            OutputText("Scenes.Amilyscene.StopDigging.AngryFirstQuestion".Localized());

            OutputText("Scenes.Amilyscene.StopDigging.ApologiesTakeTime".Localized());

            OutputText("Scenes.Amilyscene.StopDigging.AwkwardSilenceShameGuilt".Localized());

            OutputText("Scenes.Amilyscene.StopDigging.TeasingThenReassured".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void KeepDeepFuck(bool type)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyBulgeStrikes, CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgeStrikes) + 1);

            OutputText("Scenes.Amilyscene.KeepDeepFuck.PrimalLustOvercomes".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.SyllableAtATime".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.EachThrustLoseMinds".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.BodyBeginsConvulse".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.ContinueThrustApproachLimit".Localized());

            if (Player.CumQ() >= 1000)
                if (type)
                    OutputText("Scenes.Amilyscene.KeepDeepFuck.SeedFloodingInside".Localized());
                else
                    OutputText("Scenes.Amilyscene.KeepDeepFuck.BulgeBubbleForms".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.SatisfiedPullOutCum".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.FuckedOutFallsDown".Localized());

            OutputText("Scenes.Amilyscene.KeepDeepFuck.SanityReturnsShame".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyBulgeStrikes) >= 3)
                DoNext(() => TooDeepAmilyLeaves());
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void TooDeepAmilyLeaves()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.TooDeepAmilyLeaves.Aftermath".Localized());

            OutputText("Scenes.Amilyscene.TooDeepAmilyLeaves.ComeToSilenceNoticed".Localized());

            OutputText("Scenes.Amilyscene.TooDeepAmilyLeaves.BitchAbandonedYou".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DugEnough(bool type)
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.DugEnough.ThoughtPassesStretched".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.PullOutAmilyGrips".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.MoansOvertakeWetSounds".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.SquealsGaspingBreath".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.ClingingPassionateKiss".Localized());

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Amilyscene.DugEnough.RushDeepSatisfaction".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.CradlesStomachSilence".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.EmbraceCalmerManner".Localized());

            OutputText("Scenes.Amilyscene.DugEnough.ScoldsNotSoDeep".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeChargeAmilyFuckCockhead()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.TrailFingerDownNose".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.PantsTentMaleOrgan".Localized());

            OutputText("Scenes.Amilyscene.FuckAmilyPreg.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.GigglesShakesRear".Localized(
                AmilyHips(),

                AmilyButt()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.CockStandingErect".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.ClearlyVeryExcited".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.TurnToStripWatched".Localized(
                AmilyTits(),

                AmilyNipples()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.SmirkCockAching".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.CockheadSlidesIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.SpasmingSpraysJism".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.GrinningCuddleAfter".Localized());

            AmilyPreggoChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeChargeAmilyFuck()
        {
            ClearOutput();

            AmilySprite(true);

            int x = Player.CockThatFits(61);

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.TrailFingerDownNose".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.PantsTentMaleOrgan".Localized());

            OutputText("Scenes.Amilyscene.AmilyAppearance.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.GigglesShakesRear".Localized(
                AmilyHips(),

                AmilyButt()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.CockStandingErect".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.ClearlyVeryExcited".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.YourTurnToStrip".Localized(
                Player.CockDescript(x),

                AmilyTits(),

                AmilyNipples()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.SmirkCockAching".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.CockSlidesInMoist".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.SpasmingSpraysJism".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.GrinningCuddleAfter".Localized());

            AmilyPreggoChance();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeChargeAmilyMouseMilk()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.PonderWhatToDo".Localized(
                AmilyTits(),

                AmilyNipples()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.ThirstyMouthFirstBreast".Localized(
                AmilyNipples()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.TasteUniqueUnlike".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) < 2)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.DrinkEagerlySuckling".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) < 4)
                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.DrinkEagerlySucklingAgain".Localized());
                else
                {
                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.DrinkEagerlySucklingThird".Localized());

                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.BurpInsteadOfAnswer".Localized());

                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMouseMilk.CuriousRubsBelly".Localized());
                }

            int refillAmount = (10 + (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) * 2)) * CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate);

            refillAmount = Math.Clamp(refillAmount, 5, 100);

            Player.RefillHunger(refillAmount, false);

            Player.ChangeFatigue(-refillAmount / 2);

            DoNext(() => ReturnToCamp(1));

            Player.ApplyStats(new DynStat(GameStats.Speed, .3f), new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, -.5f));
        }

        public void TakeChargeAmilyEatOut()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.TrailFingerDownNose".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.PantsTentMaleOrgan".Localized());

            OutputText("Scenes.Amilyscene.AmilyAppearance.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.GigglesShakesRear".Localized(
                AmilyHips(),

                AmilyButt()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.CockStandingErect".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.ClearlyVeryExcited".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.ExpectsStripSurprised".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.CockWhereClitBelongs".Localized(
                    AmilyCock()
                ));
            else
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.AndYLetter".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.TongueProbingLick".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.ClitorisWord".Localized());
            else
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.CockBaseWord".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuckCockhead.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.JuicesTrickleInnerWalls".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.PenisEruptsSpraysCum".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.LiesGaspingBreath".Localized());

            DoNext(() => ReturnToCamp(1));

            Player.ApplyStats(new DynStat(GameStats.Intelligence, .25f), new DynStat(GameStats.Lust, 10f));
        }

        public void TakeChargeAmilyGetSucked()
        {
            ClearOutput();

            AmilySprite();

            var hands = AmilyFurry() ? "BodyParts.Descript.LowerBody.Feet.Fox".Localized() : "Common.Text.Hands".Localized();

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.StandUpAllowing".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.EachOfWord".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.CocksExposeOuterWorld".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.NotEagerButDiligent".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.LargestWord".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.StrokesLicksTip".Localized(
                hands
            ));

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.BodyWord".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.ReadyYourselfRelease".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.GaspWarningFails".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.SwallowsWithoutSkipping".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.CheeksBulgeSwallows".Localized());
                else
                    OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.CheeksBulgeHugely".Localized(
                        AmilyTits()
                    ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.FinishesWetPop".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.AsksSatisfiedWeakKnees".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyGetSucked.TurnsBackWithHope".Localized());

            DoNext(() => ReturnToCamp(1));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
        }

        public void TakeChargeAmilyScissorMeTimbers()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.TakeChargeAmilyScissorMeTimbers.TakesHerInArms".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyScissorMeTimbers.KneadingPerkyBreasts".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyScissorMeTimbers.PullsOutFingersPout".Localized(
                Player.VaginaDescript()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyScissorMeTimbers.DefeatKissSwitchPositions".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyScissorMeTimbers.ContentedSighBroadSmile".Localized());

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeChargeAmilyMountHer()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.TrailFingerDownNose".Localized(
                AmilyCock()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.AsksWantThat".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.SmirkStripsInstead".Localized(
                AmilyCock()
            ));

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.GroansPleadsStopTeasing".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.StrokesCheekWorried".Localized());

            Player.CuntChange((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) * CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth)), true, false, true);

            if (Player.Vaginas[0].Looseness >= 5)
                OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.BarelyFeelWithin".Localized());
            else
                OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.GrinsFeelsItFilling".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyFuck.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.GaspsInDelight".Localized());

            if (Player.BiggestCockLength() >= 18)
            {
                OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.GrabsCramsBetween".Localized(
                    Player.CockDescript(Player.BiggestCockIndex()),

                    AmilyTits(),

                    Player.CockDescript(Player.BiggestCockIndex())
                ));

                if (Player.BiggestCockLength() >= 22)
                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.LicksKissesTip".Localized());

                OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.ParagraphBreak".Localized());
            }
            else
                if (Player.HasCock())
                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.GrabsOneCockTeases".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.PoundsEagerlyInto".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.SpraysHermCumFace".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.SplattersMixedFluids".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.FurryWord".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.ThighsWord".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.CollapseBackwardsBreath".Localized());

            OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.FlattererSaysSmiling".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAllowsFertility) == 1)
                Player.KnockUp(PregnancyIds.Amily, PregnancyIds.MouseIncubation);

            DoNext(() => ReturnToCamp(1));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
        }

        public void LetAmilyLead()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.LetAmilyLead.GrinsAmilyRealizes".Localized());

            int x = Player.CockThatFits(61);

            var choices = new List<int>();

            if (Player.HasCock())
            {
                if (x == -1)
                    OutputText("Scenes.Amilyscene.LetAmilyLead.GlancesBeastRideYou".Localized());
                else
                {
                    choices.Add(3);

                    if (Player.IsPregnant())
                        choices.Add(6);
                }

                choices.Add(0);

                choices.Add(1);
            }

            choices.Add(2);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                choices.Add(4);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0 && Player.HasVagina())
                choices.Add(5);

            float scene = choices[Random.Shared.Next(choices.Count)];

            if (scene == 0)
            {
                OutputText("Scenes.Amilyscene.LetAmilyLead.AtNestTurnsBack".Localized());

                OutputText("Scenes.Amilyscene.LetAmilyLead.AlmostJumpsFeelsSkin".Localized());

                OutputText("Scenes.Amilyscene.LetAmilyLead.NeverExpectedThis".Localized(
                    Player.Mf("man", "girl")
                ));

                OutputText("Scenes.Amilyscene.LetAmilyLead.TellHerEnjoyedMuch".Localized());

                DoNext(() => ReturnToCamp(1));

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                return;
            }
            else
                if (scene == 1)
                {
                    OutputText("Scenes.Amilyscene.LetAmilyLead.AtNestSheKneels".Localized());

                    OutputText("Scenes.Amilyscene.LetAmilyLead.EyesSnapOpen".Localized());

                    OutputText("Scenes.Amilyscene.LetAmilyLead.HandsFeetCumJoke".Localized());

                    OutputText("Scenes.Amilyscene.LetAmilyLead.SatisfiedSmileTurnsAway".Localized());

                    DoNext(() => ReturnToCamp(1));

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                    return;
                }
                else
                    if (scene == 2)
                    {
                        OutputText("Scenes.Amilyscene.LetAmilyLead.NestPushesDown".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                            OutputText("Scenes.Amilyscene.LetAmilyLead.CockBobsMerrily".Localized());

                        OutputText("Scenes.Amilyscene.TakeChargeAmilyEatOut.ParagraphBreakAgain".Localized());

                        OutputText("Scenes.Amilyscene.LetAmilyLead.GigglesSweetTongue".Localized());

                        OutputText("Scenes.Amilyscene.LetAmilyLead.NoNudgingNeeded".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                            OutputText("Scenes.Amilyscene.LetAmilyLead.WhispersForALick".Localized(
                                AmilyCock()
                            ));

                        OutputText("Scenes.Amilyscene.LetAmilyLead.SquealsWriggling".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                            OutputText("Scenes.Amilyscene.LetAmilyLead.PenisEruptsSpraysBoth".Localized());

                        OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.ParagraphBreak".Localized());

                        OutputText("Scenes.Amilyscene.LetAmilyLead.FlopsOntoBelly".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                            OutputText("Scenes.Amilyscene.LetAmilyLead.LimpCockWord".Localized());

                        OutputText("Scenes.Amilyscene.LetAmilyLead.AcrossChestRepositions".Localized());

                        DoNext(() => ReturnToCamp(1));

                        Player.ApplyStats(new DynStat(GameStats.Intelligence, .25f), new DynStat(GameStats.Lust, 10f));

                        return;
                    }
                    else
                        if (scene == 3)
                        {
                            OutputText("Scenes.Amilyscene.LetAmilyLead.NestPushesDownAgain".Localized());

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Amilyscene.LetAmilyLead.EachOfWord".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.CocksExposedGrowErect".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.DontDenyGroans".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.PurrsOddImage".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.TeasesStartsThrusting".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.HowlsInUnison".Localized());

                            if (Player.CumQ() > 500)
                                OutputText("Scenes.Amilyscene.LetAmilyLead.SpoogeDistendsBelly".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.CollapsesOntoFront".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.ExhaustedNapWakesAlone".Localized());

                            AmilyPreggoChance();

                            DoNext(() => ReturnToCamp(1));

                            Player.Orgasm("Generic");

                            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                            Player.RefillHunger(5);

                            return;
                        }
                        else
                            if (scene == 4)
                            {
                                OutputText("Scenes.Amilyscene.LetAmilyLead.NestPushesDownThird".Localized(
                                    AmilyHips()
                                ));

                                OutputText("Scenes.Amilyscene.LetAmilyLead.SmirksUpEngulfsHead".Localized(
                                    AmilyCock(),

                                    CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength)
                                ));

                                OutputText("Scenes.Amilyscene.LetAmilyLead.SlowlyUncertainlyThrust".Localized());

                                OutputText("Scenes.Amilyscene.LetAmilyLead.MaybeBetterThanThought".Localized());

                                OutputText("Scenes.Amilyscene.LetAmilyLead.CockPopsFromMouth".Localized(
                                    AmilyTits()
                                ));

                                if (Player.HasCock())
                                    OutputText("Scenes.Amilyscene.LetAmilyLead.WhenIDoThat".Localized());
                                else
                                    OutputText("Scenes.Amilyscene.LetAmilyLead.ForAGuyQuestion".Localized());

                                OutputText("Scenes.Amilyscene.LetAmilyLead.ShrugUncertainSitsUp".Localized());

                                DoNext(() => ReturnToCamp(1));

                                Player.ApplyStats(new DynStat(GameStats.Intelligence, .25f), new DynStat(GameStats.Lust, 10f));

                                return;
                            }
                            else
                                if (scene == 5)
                                {
                                    OutputText("Scenes.Amilyscene.LetAmilyLead.NestPushesDownFourth".Localized(
                                        AmilyCock()
                                    ));

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.OwesChanceToUse".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.SpreadLegsFullLook".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.SituatesBetweenLegs".Localized());

                                    if (Player.HasCock())
                                        OutputText("Scenes.Amilyscene.LetAmilyLead.StrokesPrefunctory".Localized());

                                    OutputText("Scenes.Amilyscene.TakeChargeAmilyMountHer.ParagraphBreakAgain".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.SufficientlyWetPlunges".Localized());

                                    Player.CuntChange((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) * CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth)), true, false, true);

                                    if (Player.Vaginas[0].Looseness >= 4)
                                        OutputText("Scenes.Amilyscene.LetAmilyLead.SoLargeBarelyFeel".Localized());
                                    else
                                        OutputText("Scenes.Amilyscene.LetAmilyLead.GaspGripNetherlips".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.ParagraphBreak".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.MurmursCockInPussy".Localized());

                                    if (Player.Wetness() >= 5)
                                        OutputText("Scenes.Amilyscene.LetAmilyLead.SquelchingSlurping".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.BestToMakeBuck".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.ThreshThrustTogether".Localized());

                                    if (Player.HasCock())
                                        OutputText("Scenes.Amilyscene.LetAmilyLead.JismFallsLikeRain".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.PantingSlidesWetly".Localized());

                                    OutputText("Scenes.Amilyscene.LetAmilyLead.RaisesHeadPecksLips".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAllowsFertility) == 1)
                                        Player.KnockUp(PregnancyIds.Amily, PregnancyIds.MouseIncubation);

                                    Player.Orgasm("Generic");

                                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
                                }
                                else
                                    if (scene == 6)
                                    {
                                        OutputText("Scenes.Amilyscene.LetAmilyLead.ConveySheCanChoose".Localized());

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.OffersFriskySmile".Localized(
                                            CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) == "a naughty nurse's outfit" ? "skirt" : (CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) == ItemRegistry.Create(ArmorIds.S_SWMWR).Name ? "Common.Text.Bottoms".Localized() : "Common.Text.Pants".Localized())
                                        ));

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.NakedRemovesArmor".Localized());

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.LoverLeadsToNest".Localized());

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.ShiverTailCoiling".Localized());

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.InterlocksFingersShakesWaist".Localized());

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.PussyTightensCock".Localized());

                                        OutputText("Scenes.Amilyscene.LetAmilyLead.MovesHerOffEmbrace".Localized());

                                        Player.Orgasm("Dick");
                                    }

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyGiftMenu()
        {

            ClearButtons();

            AddNextButton("Common.Button.GiveItem".Localized(), GiveAmilyAPresent);

            AddNextButton("Common.Button.GiveClothes".Localized(), GiveAmilyClothes);

            AddNextButton("Common.Button.GiveRing".Localized(), AmilyRingGifting);

            AddButton(14, "Common.Button.Back".Localized(), () => AmilyFollowerEncounter());
        }

        public void GiveAmilyAPresent()
        {
            ClearOutput();

            AmilySprite();

            ClearButtons();

            var buttons = new List<Action>();

            var haveGift = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                OutputText("Scenes.Amilyscene.GiveAmilyAPresent.TellHaveGift".Localized());

                OutputText("Scenes.Amilyscene.GiveAmilyAPresent.SmilesDelightedLove".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.GiveAmilyAPresent.CumBucketWhatTake".Localized());

            var iDrft = !AmilyCorrupt() || (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)) && !Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft))) ? ConsumableIds.PurifiedIncubiDraft : ConsumableIds.IncubiDraft;

            var sMilk = !AmilyCorrupt() || (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)) && !Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk))) ? ConsumableIds.PurifiedSuccubiMilk : ConsumableIds.SuccubiMilk;

            var sDeli = !AmilyCorrupt() || (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight)) && !Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubusDelight))) ? ConsumableIds.PurifiedSuccubusDelight : ConsumableIds.SuccubusDelight;

            var piEgg = Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargePink)) && !Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)) ? ConsumableIds.EggLargePink : ConsumableIds.EggPink;

            var puEgg = Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargePurple)) && !Player.HasItem(ConsumableIds.EggPurple) ? ConsumableIds.EggLargePurple : ConsumableIds.EggPurple;

            var brEgg = Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeBrown)) && !Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBrown)) ? ConsumableIds.EggLargeBrown : ConsumableIds.EggBrown;

            var whEgg = Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeWhite)) && !Player.HasItem(ItemRegistry.Create(ConsumableIds.EggWhite)) ? ConsumableIds.EggLargeWhite : ConsumableIds.EggWhite;

            var giftButtons = new List<(string Label, Action Act, bool Has)>
            {
                (ItemRegistry.Create(iDrft).ShortNameKey.Localized(), () => GiveAmilyPureIncubusDraft(), Player.HasItem(ItemRegistry.Create(iDrft))),

                (ItemRegistry.Create(sMilk).ShortNameKey.Localized(), () => GiveAmilyPurifiedSuccubusMilk(), Player.HasItem(ItemRegistry.Create(sMilk))),

                (ItemRegistry.Create(sDeli).ShortNameKey.Localized(), !AmilyCorrupt() ? () => GiveAmilyPureSuccubusDelight() : () => GiveCorruptAmilySuccubusDelight(), Player.HasItem(ItemRegistry.Create(sDeli))),

                (ItemRegistry.Create(ConsumableIds.Lactaid).ShortNameKey.Localized(), () => MakeTheMouseAMilkCowMoo(), Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid))),

                (ItemRegistry.Create(ItemIds.Reducto).ShortNameKey.Localized(), () => GiveAmilySomeReducto(), Player.HasItem(ItemRegistry.Create(ItemIds.Reducto))),

                (ItemRegistry.Create(piEgg).ShortNameKey.Localized(), () => GiveAmilyAPinkEgg(), Player.HasItem(ItemRegistry.Create(piEgg))),

                (ItemRegistry.Create(puEgg).ShortNameKey.Localized(), () => GiveAmilyAPurpleEgg(), Player.HasItem(ItemRegistry.Create(puEgg))),

                (ItemRegistry.Create(brEgg).ShortNameKey.Localized(), () => GiveAmilyABrownEgg(), Player.HasItem(ItemRegistry.Create(brEgg))),

                (ItemRegistry.Create(whEgg).ShortNameKey.Localized(), () => GiveAmilyAWhiteEgg(), Player.HasItem(ItemRegistry.Create(whEgg)))
            };

            haveGift = Player.Inventory.HasItem(ConsumableIds.PurifiedIncubiDraft)

                || Player.Inventory.HasItem(ConsumableIds.PurifiedSuccubiMilk)

                || Player.Inventory.HasItem(ConsumableIds.PurifiedSuccubusDelight)

                || Player.Inventory.HasItem(ConsumableIds.Lactaid)

                || Player.Inventory.HasItem(ItemIds.Reducto)

                || Player.Inventory.HasItem(ConsumableIds.EggLargePink)

                || Player.Inventory.HasItem(ConsumableIds.EggLargePurple)

                || Player.Inventory.HasItem(ConsumableIds.EggLargeBrown)

                || Player.Inventory.HasItem(ConsumableIds.EggLargeWhite);

            if (!haveGift)
                if (!AmilyCorrupt())
                    OutputText("Scenes.Amilyscene.GiveAmilyAPresent.NoItemsInterested".Localized());
                else
                    OutputText("Scenes.Amilyscene.GiveAmilyAPresent.NoItemsWorthUsing".Localized());

            var giftIndex = 0;

            foreach (var (Label, Act, Has) in giftButtons)
            {
                if (Has)
                    AddButton(giftIndex, Label, Act);
                else
                    AddButtonDisabled(giftIndex, Label, "You don't have that item.");

                giftIndex++;
            }

            AddButton(14, "Common.Button.Back".Localized(), AmilyCorrupt() ? () => AmilyMenu() : AmilyGiftMenu);
        }

        public void GiveAmilyClothes()
        {
            ClearOutput();

            var giftedClothes = new List<string>();

            if (Save.GetBool("giftedCClothes"))
                giftedClothes.Add("Comfortable clothes");

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyOwnsBikini) != 0)
                giftedClothes.Add("Bikini");

            if (CoC.Instance.Flags.Get(GameFlagIds.GivenAmilyNurseOutfit) != 0)
                giftedClothes.Add("Nurse outfit");

            if (giftedClothes.Count != 0)
                OutputText("Scenes.Amilyscene.GiveAmilyClothes.ClothingHeader".Localized());

            foreach (var _tmp_ in giftedClothes)
            {
                string item = _tmp_;

                OutputText("Scenes.Amilyscene.GiveAmilyClothes.ParagraphBreakPrefix".Localized(
                    item
                ));
            }

            OutputText("Scenes.Amilyscene.GiveAmilyClothes.GiftChoicePrompt".Localized());

            ClearButtons();

            var buttons = new List<(string Label, Action Act)>();

            if (Player.HasItem(ItemRegistry.Create(ArmorIds.S_SWMWR)) && CoC.Instance.Flags.Get(GameFlagIds.AmilyOwnsBikini) == 0 && Player.HasCockThatFits(61) && !AmilyCorrupt())
                OutputText("Scenes.Amilyscene.GiveAmilyClothes.CouldGiveBikiniSwim".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GivenAmilyNurseOutfit) == 0 && !AmilyCorrupt() && Player.HasItem(ItemRegistry.Create(ArmorIds.NurseOutfit)) && Player.HasCockThatFits(61))
                OutputText("Scenes.Amilyscene.GiveAmilyClothes.CouldGiveNurseOutfit".Localized());

            if (!Save.GetBool("giftedCClothes"))
                buttons.Add((ItemRegistry.Create(ArmorIds.ComfortableClothes).ShortNameKey.Localized(), () => GiveAmilySomePants()));

            if (!AmilyCorrupt())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyOwnsBikini) == 0)
                    buttons.Add((ItemRegistry.Create(ArmorIds.SkimpySwimwear).ShortNameKey.Localized(), () => AmilySwimFuckIntro()));

                if (CoC.Instance.Flags.Get(GameFlagIds.GivenAmilyNurseOutfit) == 0)
                    buttons.Add((ItemRegistry.Create(ArmorIds.NurseOutfit).ShortNameKey.Localized(), () => AmilyNurseCheckup()));
            }

            var clothesIndex = 0;

            foreach (var (Label, Act) in buttons)
            {
                AddButton(clothesIndex, Label, Act);

                clothesIndex++;
            }

            AddButton(14, "Common.Button.Back".Localized(), AmilyGiftMenu);
        }

        public int GetMaxSize(int pure, int corrupt, int hyper)
        {

            if (hyper != 0)
                return hyper;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                return pure;

            return corrupt;
        }

        public void GiveAmilyPureIncubusDraft()
        {
            ClearOutput();

            AmilySprite(true);

            var maxSize = GetMaxSize(13, 15, 23);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= maxSize)
                    {
                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.SeesVialShakesHead".Localized());

                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.DisappointmentSheExplains".Localized());

                        if (Player.Corruption >= 50 || Player.VaginalCapacity() >= 50)
                        {
                            OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.StrugglesContainDisappointment".Localized());

                            if (Player.Corruption >= 75)
                                OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.IdeaRapingHelplessBody".Localized());

                            OutputText("Scenes.Amilyscene.LetAmilyLead.ParagraphBreakAgain".Localized());
                        }
                        else
                            OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.LogicUndeniable".Localized());

                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.VialBackSheLeaves".Localized());

                        DoNext(PlayerMenu);

                        return;
                    }

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.LooksDisdainfullyVial".Localized());

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

                    AmilyDickGrow();

                    DoNext(PlayerMenu);
                }
                else
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.OffersVialLeapsBack".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.ProtestLiquidCorruption".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.AssurePurifiedSafe".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.AsksAreYouSure".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), GiveAmilyPureIncubusDraft4Realz);

                    AddButton(1, "Common.Button.No".Localized(), AmilyFollowerEncounter);
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                {
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)))
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.IncubiDraft));
                    else
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.OrdersHerToDrink".Localized(
                        Player.Mf("Master", "Mistress")
                    ));

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.ConfirmsThisIsWant".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.DownsBottleMoans".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.StopsMoaningSmallChange".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.AskGrowHugeCock".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, 4);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 1);

                    AddButton(0, "Common.Button.Yes".Localized(), CorruptAmilyGetsDickMaxxedOut);

                    AddButton(1, "Common.Button.No".Localized(), CorruptAmilyYouDeclineMaxxingHerDick);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < maxSize && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) < 3)
                    {
                        if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)))
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.IncubiDraft));
                        else
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.PassesBottleToHer".Localized());

                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.OpensLegsDownsBottle".Localized());

                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.DismissHerWithWave".Localized());

                        AmilyDickGrow();

                        DoNext(PlayerMenu);
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.ReconsiderSmallFrame".Localized());

                        DoNext(() => AmilyFollowerEncounter());
                    }
        }

        public void CorruptAmilyYouDeclineMaxxingHerDick()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.CorruptAmilyYouDeclineMaxxingHerDick.LeaveDickAsIs".Localized());

            DoNext(PlayerMenu);
        }

        public void CorruptAmilyGetsDickMaxxedOut()
        {
            ClearOutput();

            AmilySprite(true);

            var maxSize = GetMaxSize(13, 15, 23);

            if (Player.IsBiped() && Player.Body.Get<LowerBodyPart>().Id != BodyPartTypes.Hoofed)
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyGetsDickMaxxedOut.NotSatisfiedNeedsBigger".Localized(
                    Player.Foot()
                ));

                OutputText("Scenes.Amilyscene.CorruptAmilyGetsDickMaxxedOut.CoveredOwnCum".Localized(
                    Player.Foot(),

                    Player.Foot(),

                    Player.Mf("Master", "Mistress")
                ));
            }
            else
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyGetsDickMaxxedOut.NotSatisfiedStrokesHarder".Localized());

                OutputText("Scenes.Amilyscene.CorruptAmilyGetsDickMaxxedOut.CoveredCumStillStrokes".Localized(
                    Player.Mf("Master", "Mistress")
                ));
            }

            OutputText("Scenes.Amilyscene.CorruptAmilyGetsDickMaxxedOut.AbandonHerInFluids".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, maxSize);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 3);

            DoNext(PlayerMenu);
        }

        public void GiveAmilyPureIncubusDraft4Realz()
        {
            ClearOutput();

            AmilySprite(true);

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

            OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft4Realz.AssurePassItOver".Localized());

            OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft4Realz.HermFourInchesLong".Localized());

            OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft4Realz.StaresAtNewAppendage".Localized());

            AmilyDickGrow();

            DoNext(PlayerMenu);
        }

        public void AmilyDickGrow()
        {

            var maxSize = GetMaxSize(13, 15, 23);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, 4);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < maxSize)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) + 1);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 1 + ((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) - 4) / 9));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) > 3)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 3);
                }
                else
                {
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, maxSize);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 3);
                }
        }

        public void DeclineToMakeAmilyFuta()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.DeclineToMakeAmilyFuta.SecondThoughtDecides".Localized());

            OutputText("Scenes.Amilyscene.DeclineToMakeAmilyFuta.NothingToSayLeaves".Localized());

            DoNext(PlayerMenu);
        }

        public void GiveAmilyPurifiedSuccubusMilk()
        {
            ClearOutput();

            AmilySprite();

            var maxSize = GetMaxSize(5, 5, 22);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.OffersDemonicMilk".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) < maxSize)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.SmilesWantsBiggerBreasts".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) + 1);

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));
                }
                else
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.ThoughtfulThenRefuses".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.SwallowHardAgree".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                }
            }
            else
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

                OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.AsksIfSheKnows".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.CockHardensDelight".Localized());

                OutputText("Scenes.Amilyscene.LetAmilyLead.ParagraphBreakThird".Localized());

                OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.UncorksPoursInside".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyVaginalWetness, CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) >= 5)
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.FemcumJetHitsFace".Localized());
                else
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.JuicesFloodOut".Localized());

                OutputText("Scenes.Amilyscene.LetAmilyLead.ParagraphBreakFourth".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) < 5)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.LetsGoPlopsDown".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.LetLustBuild".Localized());
                }
                else
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.LetsGoObservesHer".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.MoreMilkSensitizes".Localized());
                }
            }

            DoNext(() => AmilyFollowerEncounter());
        }

        public void AmilyDrinksSuccubusDelight()
        {
            ClearOutput();

            AmilySprite();

            var footpaw = "foot";

            if (AmilyFurry())
                footpaw += "paw";

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                {
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubusDelight)))
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubusDelight));
                    else
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight));

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.NeedsCumMoreBalls".Localized());

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.OpensLegsDownsBottle".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.CockHardNothingElse".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilyPureIncubusDraft.ParagraphBreak".Localized());

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.NeedsLittlePush".Localized());

                    if (Player.HasCock() && (Player.Gender == Gender.Male || Random.Shared.Next(2) == 0))
                    {
                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.StandOverCockHard".Localized(
                            footpaw,

                            footpaw
                        ));

                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.FocusPreFormsTip".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.StandOverPussyMoist".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.ImaginesHugeBalls".Localized(
                            Player.VaginaDescript(0)
                        ));
                    }

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.WatchSmilingGrowth".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.SkinCoversBase".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.SkinFormsSack".Localized());

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.OrdersPracticeWalking".Localized());

                    Player.Orgasm("Generic");

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyHasBallsAndSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) + 1);
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight));

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.AssurePassItOver".Localized());

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.StaresAtNewBalls".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyHasBallsAndSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) + 1);
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 6)
                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.MoreWouldImmobilize".Localized());
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubusDelight));

                    OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.PassesBottleOrders".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyHasBallsAndSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) + 1);
                }

            DoNext(() => AmilyFollowerEncounter());
        }

        public void GiveCorruptAmilySuccubusDelight()
        {
            ClearOutput();

            AmilySprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) == 0)
            {
                OutputText("Scenes.Amilyscene.GiveCorruptAmilySuccubusDelight.ShowsVialOrdersDrink".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), AmilyDrinksSuccubusDelight);

                AddButton(1, "Common.Button.No".Localized(), AmilyFollowerEncounter);
            }
            else
                AmilyDrinksSuccubusDelight();
        }

        public void GiveAmilyPureSuccubusDelight()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.GiveAmilyPureSuccubusDelight.OffersVialLeapsBack".Localized());

            OutputText("Scenes.Amilyscene.GiveAmilyPureSuccubusDelight.ProtestLiquidCorruption".Localized());

            OutputText("Scenes.Amilyscene.GiveAmilyPureSuccubusDelight.AssurePurifiedSafe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) == 0)
            {
                OutputText("Scenes.Amilyscene.GiveAmilyPureSuccubusDelight.AsksAreYouSure".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), AmilyDrinksSuccubusDelight);

                AddButton(1, "Common.Button.No".Localized(), AmilyFollowerEncounter);

                return;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPureSuccubusDelight.RefusesBiggerBalls".Localized());

                    DoNext(() => AmilyFollowerEncounter());

                    return;
                }
                else
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyPureSuccubusDelight.RefusesNoPenis".Localized());

                    DoNext(() => AmilyFollowerEncounter());
                }
        }

        public void GiveAmilyAPinkEgg()
        {
            ClearOutput();

            AmilySprite(true);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                OutputText("Scenes.Amilyscene.GiveAmilyAPinkEgg.OffersPinkEggFast".Localized());

                OutputText("Scenes.Amilyscene.GiveAmilyAPinkEgg.SqueaksReliefElse".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.GiveAmilyAPinkEgg.OffersPinkEggAgain".Localized());

                OutputText("Scenes.Amilyscene.GiveAmilyAPinkEgg.SorrySheDisappointed".Localized());
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggPink)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggPink));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePink));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHasBallsAndSize, 0);

            DoNext(() => AmilyFollowerEncounter());
        }

        public void GiveAmilyAWhiteEgg()
        {
            ClearOutput();

            AmilySprite();

            var maxSize = GetMaxSize(4, 4, 8);

            OutputText("Scenes.Amilyscene.GiveAmilyAWhiteEgg.OfferEggNippleGrowth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < maxSize)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyAWhiteEgg.SkepticalBiggerNipples".Localized());

                    AmilyNippleGrow();
                }
                else
                    OutputText("Scenes.Amilyscene.GiveAmilyAWhiteEgg.RefusesAlreadyBigEnough".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < maxSize)
                    OutputText("Scenes.Amilyscene.GiveAmilyAWhiteEgg.SmilesKnowinglyAccepts".Localized(
                        Player.Mf("master", "mistress")
                    ));
                else
                    OutputText("Scenes.Amilyscene.GiveAmilyAWhiteEgg.MoansEagerHugeNipples".Localized(
                        Player.Mf("master", "mistress")
                    ));

                AmilyNippleGrow();
            }

            DoNext(() => AmilyFollowerEncounter());
        }

        public void AmilyNippleGrow()
        {

            var maxSize = GetMaxSize(4, 4, 8);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggWhite)))
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggWhite));

                CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) + .25f);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, (int)(CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) * 100) / 100);
            }
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeWhite));

                CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) + .7f);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, (int)(CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) * 100) / 100);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) > maxSize)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, maxSize);
        }

        public void GiveAmilyABrownEgg()
        {
            ClearOutput();

            AmilySprite();

            var maxSize = GetMaxSize(16, 20, 35);

            OutputText("Scenes.Amilyscene.GiveAmilyABrownEgg.OfferEggButtGrowth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < maxSize)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyABrownEgg.GigglesJunkInTrunk".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBrown)))
                    {
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBrown));

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) + 1 + Random.Shared.Next(2));
                    }
                    else
                    {
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBrown));

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) + 2 + Random.Shared.Next(3));
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) > maxSize)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, maxSize);
                }
                else
                    OutputText("Scenes.Amilyscene.GiveAmilyABrownEgg.RefusesEnoughPadding".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < maxSize)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyABrownEgg.ToyWantsMorePadding".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBrown)))
                    {
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBrown));

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) + 1 + Random.Shared.Next(2));
                    }
                    else
                    {
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBrown));

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) + 2 + Random.Shared.Next(3));
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) > maxSize)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, maxSize);
                }
                else
                    OutputText("Scenes.Amilyscene.GiveAmilyABrownEgg.EatsEggNoEffect".Localized());

            DoNext(() => AmilyFollowerEncounter());
        }

        public void GiveAmilyAPurpleEgg()
        {
            ClearOutput();

            AmilySprite();

            var maxSize = GetMaxSize(16, 20, 20);

            OutputText("Scenes.Amilyscene.GiveAmilyAPurpleEgg.OfferEggHipsGrowth".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < maxSize)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyAPurpleEgg.ThoughtfulWiderHips".Localized());

                    if (Player.HasItem(ConsumableIds.EggPurple))
                    {
                        Player.ConsumeItem(ConsumableIds.EggPurple);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) + 1 + Random.Shared.Next(2));
                    }
                    else
                    {
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePurple));

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) + 2 + Random.Shared.Next(3));
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) > maxSize)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, maxSize);
                }
                else
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyAPurpleEgg.QuirkedBrowTooWide".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 4f));
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < maxSize)
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyAPurpleEgg.CorruptWiderHips".Localized(
                        Player.Mf("Master", "Mistress")
                    ));

                    if (Player.HasItem(ConsumableIds.EggPurple))
                    {
                        Player.ConsumeItem(ConsumableIds.EggPurple);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) + 1 + Random.Shared.Next(2));
                    }
                    else
                    {
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargePurple));

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) + 2 + Random.Shared.Next(3));
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) > maxSize)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, maxSize);
                }
                else
                {
                    OutputText("Scenes.Amilyscene.GiveAmilyAPurpleEgg.EatsEggBodyLimit".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 4f));
                }

            DoNext(() => AmilyFollowerEncounter());
        }

        public void GiveAmilySomeReducto()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.GiveAmilySomeReducto.WrinklesNoseAwfulSmell".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 1)
                AddButton(0, "Common.Button.Breasts".Localized(), () => AmilyReducto(0));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) > 0.5f)
                AddButton(1, "Common.Button.Nipples".Localized(), () => AmilyReducto(1));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) > 6)
                AddButton(2, "Scenes.PlayerAppearance.Label.Hips".Localized(), () => AmilyReducto(2));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 4)
                AddButton(3, "Common.Button.Penis".Localized(), () => AmilyReducto(3));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) > 6)
                AddButton(4, "Scenes.PlayerAppearance.Label.Butt".Localized(), () => AmilyReducto(4));

            AddButton(14, "Common.Button.Back".Localized(), () => GiveAmilyAPresent());
        }

        public void AmilyReducto(int part)
        {
            ClearOutput();

            Player.ConsumeItem(ItemIds.Reducto);

            var chosenPart = "";

            if (part == 0)
            {
                chosenPart = "breasts";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 6)
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) - 1);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) - 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) < 1)
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, 1);
            }
            else
                if (part == 1)
                {
                    chosenPart = "nipples";

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) > 3)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) - 0.5f);

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) - 0.5f);

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < 0.5f)
                        CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, 0.5f);
                }
                else
                    if (part == 2)
                    {
                        chosenPart = "hips";

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) > 12)
                            CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) - 1);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) - 1);

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < 6)
                            CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, 6);
                    }
                    else
                        if (part == 3)
                        {
                            chosenPart = "cock";

                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 10)
                                CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) - 1);

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) - 1);

                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 4)
                                CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, 4);

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 1 + ((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) - 4) / 9));
                        }
                        else
                            if (part == 4)
                            {
                                chosenPart = "ass";

                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) > 10)
                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) - 1);

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) - 1);

                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 6)
                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, 6);
                            }

            OutputText("Scenes.Amilyscene.AmilyReducto.GrimacesAppliesPaste".Localized(
                chosenPart
            ));

            DoNext(() => AmilyFollowerEncounter());
        }

        public void GiveAmilySomePants()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.GiveAmilySomePants.OfferComfortableClothes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) == "")
                CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, "rags");

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) == "rags" || CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) == "sexy rags")
            {
                OutputText("Scenes.Amilyscene.GiveAmilySomePants.EyesLightUpGlee".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, "comfortable clothes");
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyClothing) != "comfortable clothes")
                {
                    OutputText("Scenes.Amilyscene.GiveAmilySomePants.TeasesSpoilingMe".Localized());

                    OutputText("Scenes.Amilyscene.GiveAmilySomePants.AssuresLooksBeautiful".Localized());
                }

            Player.ConsumeItem(ItemRegistry.Create(ArmorIds.ComfortableClothes));

            Save.SetBool("giftedCClothes", true);

            DoNext(() => AmilyFollowerEncounter());
        }

        public void MakeTheMouseAMilkCowMoo()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                OutputText("Scenes.Amilyscene.MakeTheMouseAMilkCowMoo.AsksUncertainMouseMilk".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                    OutputText("Scenes.Amilyscene.MakeTheMouseAMilkCowMoo.TeasesSeductivePlayful".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) < 2)
                {
                    OutputText("Scenes.Amilyscene.MakeTheMouseAMilkCowMoo.LactaidBreastsSwell".Localized(
                        Player.BreastCupForRating(CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize))
                    ));

                    CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, 2);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) >= 2)
                        OutputText("Scenes.Amilyscene.MakeTheMouseAMilkCowMoo.SqueaksShockMilkLetdown".Localized());

                DoNext(() => AmilyFollowerEncounter());
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                    OutputText("Scenes.Amilyscene.MakeTheMouseAMilkCowMoo.PullsBreastsOutQuickly".Localized(
                        AmilyNipples()
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                    OutputText("Scenes.Amilyscene.MakeTheMouseAMilkCowMoo.PullsBreastsOutCareful".Localized(
                        AmilyNipples(),

                        Player.Mf("Master", "Mistress")
                    ));

                AddButton(0, "Common.Button.Yes".Localized(), TakeChargeAmilyMouseMilk);

                AddButton(1, "Common.Button.No".Localized(), AmilyFollowerEncounter);
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyLactationRate, CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) + 1);
        }

        public void DyeAmilyHairOrFur()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.DyeAmilyHairOrFur.AsksAboutHairColor".Localized());

            OutputText("Scenes.Amilyscene.DyeAmilyHairOrFur.ContemplativeNewLook".Localized());

            OutputText("Scenes.Amilyscene.DyeAmilyHairOrFur.HeadToStreamDye".Localized());

            DoNext(() => AmilyFollowerEncounter());
        }

        public string AmilyHips()
        {

            var desc = "";

            float rando = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) <= 1)
            {
                if (Random.Shared.Next(2) == 0)
                    desc = "boyish ";
                else
                    desc = "tiny ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 6)
                    desc = "waspish ";
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) > 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < 4)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0: desc = "slender "; break;
                    case 1: desc = "narrow "; break;
                    case 2: desc = "thin "; break;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 6)
                    desc = "waspish ";
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < 6)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "average ";

                        break;

                    case 1:
                        desc = "normal ";

                        break;

                    case 2:
                        desc = "plain ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 6 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < 10)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "ample ";

                        break;

                    case 1:
                        desc = "noticeable ";

                        break;

                    case 2:
                        desc = "girly ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < 15)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "flared ";

                        break;

                    case 1:
                        desc = "curvy ";

                        break;

                    case 2:
                        desc = "wide ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 15 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) < 20)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "fertile ";

                        break;

                    case 1:
                        desc = "child-bearing ";

                        break;

                    case 2:
                        desc = "voluptuous ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 20)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "broodmother-sized ";

                        break;

                    case 1:
                        desc = "cow-like ";

                        break;

                    case 2:
                        desc = "inhumanly-wide ";

                        break;
                }
            }

            rando = Random.Shared.Next(2);

            switch (rando)
            {
                case 0:
                    if (Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 15)
                        desc += "flanks";
                    else
                        desc += "hips";

                    break;

                case 1:
                    if (Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHipRating) >= 15)
                        desc += "flanks";
                    else
                        desc += "thighs";

                    break;
            }

            return desc;
        }

        public string AmilyButt()
        {

            var desc = "";

            float rando = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) <= 1)
                if (Random.Shared.Next(2) == 0)
                    desc = "pixie-like ";
                else
                    desc = "very small ";

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) > 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 4)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "tight ";

                        break;

                    case 1:
                        desc = "firm ";

                        break;

                    case 2:
                        desc = "compact ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 6)
            {
                rando = Random.Shared.Next(2);

                switch (rando)
                {
                    case 0:
                        desc = "fair ";

                        break;

                    case 1:
                        desc = "nice ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 6 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 8)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        return "handful of ass";

                    case 1:
                        desc = "full ";

                        break;

                    case 2:
                        desc = "shapely ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 8 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 10)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "squeezable ";

                        break;

                    case 1:
                        desc = "large ";

                        break;

                    case 2:
                        desc = "substantial ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 13)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "jiggling ";

                        break;

                    case 1:
                        desc = "spacious ";

                        break;

                    case 2:
                        desc = "heavy ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 13 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 16)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "hand-devouring ";

                        break;

                    case 1:
                        return "generous amount of ass";

                    case 2:
                        desc = "voluminous ";

                        break;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 16 && CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) < 20)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "huge ";

                        break;

                    case 1:
                        desc = "vast ";

                        break;

                    case 2:
                        return "jiggling expanse of ass";
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAssSize) >= 20)
            {
                rando = Random.Shared.Next(3);

                switch (rando)
                {
                    case 0:
                        desc = "ginormous ";

                        break;

                    case 1:
                        desc = "colossal ";

                        break;

                    case 2:
                        desc = "tremendous ";

                        break;
                }
            }

            rando = Random.Shared.Next(2);

            switch (rando)
            {
                case 0:
                    desc += "butt";

                    break;

                case 1:
                    desc += "ass";

                    break;
            }

            return desc;
        }

        public string AmilyBalls()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) == 0)
                return "prostate";

            int rando = 0;

            var desc = "";

            rando = Random.Shared.Next(3);

            switch (rando)
            {
                case 0:
                    desc += "pair of ";

                    break;

                case 1:
                    desc += "two ";

                    break;

                case 2:
                    desc += "duo of ";

                    break;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 1 && Random.Shared.Next(3) <= 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 2)
                    desc += "large ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 3)
                    desc += "baseball-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 3 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 4)
                    desc += "apple-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 5)
                    desc += "grapefruit-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 7)
                    desc += "cantaloupe-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 7 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 9)
                    desc += "soccerball-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 9 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 12)
                    desc += "basketball-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 12 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 15)
                    desc += "watermelon-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 15 && CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) < 18)
                    desc += "beachball-sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) >= 18)
                    desc += "hideously swollen and oversized ";
            }
            else
                if (AmilyFurry())
                    desc += "fuzzy ";

            rando = Random.Shared.Next(9);

            if (rando < 4)
                desc += "balls";

            if (rando >= 4 && rando < 6)
                desc += "testicles";

            switch (rando)
            {
                case 6:
                    desc += "testes";

                    break;

                case 7:
                    desc += "gonads";

                    break;

                case 8:
                    desc += "nuts";

                    break;
            }

            return desc;
        }

        public string AmilyTits()
        {

            var temp = (int)(Random.Shared.NextDouble() * 3);

            var descript = "";

            if (Random.Shared.Next(2) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) <= 2)
                {
                    temp = Random.Shared.Next(3);

                    if (temp == 0)
                        descript += "small ";

                    if (temp == 1)
                        descript += "little ";

                    if (temp == 2)
                        descript += "perky ";
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) <= 4)
                    descript += "ample ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 4 && CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) <= 6)
                {
                    temp = Random.Shared.Next(4);

                    if (temp == 0)
                        descript += "big ";

                    if (temp == 1)
                        descript += "large ";

                    if (temp == 2)
                        descript += "pillowy ";

                    if (temp == 3)
                        descript += "jiggly ";
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 6 && CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) < 13)
                {
                    temp = Random.Shared.Next(4);

                    if (temp == 0)
                        descript += "basketball-sized ";

                    if (temp == 1)
                        descript += "whorish ";

                    if (temp == 2)
                        descript += "pornstar-like ";

                    if (temp == 3)
                        descript += "jiggling ";
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) >= 13)
                    descript += "beach-ball sized ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) >= 18)
                {
                    temp = Random.Shared.Next(2);

                    if (temp == 1)
                        descript += "mountainous ";
                    else
                        descript += "immense ";
                }
            }

            temp = Random.Shared.Next(10);

            if (temp == 0)
                descript += "breasts";

            if (temp == 1)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) > 2)
                    descript += "milk-udders";
                else
                    descript += "breasts";

            if (temp == 2)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) > 1.5f)
                    descript += "milky ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 4)
                    descript += "tits";
                else
                    descript += "breasts";
            }

            if (temp == 3)
                descript += "breasts";

            if (temp == 4)
                descript += "tits";

            if (temp == 5)
                descript += "tits";

            if (temp == 6)
                descript += "tits";

            if (temp == 7)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) < 2.5f)
                    descript += "milk jugs";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) >= 2.5f)
                    descript += "udders";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) < 1)
                    descript += "jugs";
            }

            if (temp == 8)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 6)
                    descript += "love-pillows";
                else
                    descript += "boobs";

            if (temp == 9)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCupSize) > 6)
                    descript += "tits";
                else
                    descript += "breasts";

            return descript;
        }

        public string AmilyCock()
        {

            var descript = "";

            var descripted = false;

            int rando = 0;

            if ((int)(Random.Shared.NextDouble() * 3) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 3)
                    descript = "squat ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 3 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 5)
                    descript = "short ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 7)
                    descript = "average ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 7 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 9)
                    descript = "long ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 13)
                    descript = "huge ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 13 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 18)
                    descript = "massive ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 18 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 30)
                    descript = "enormous ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) >= 30)
                    descript = "towering ";

                descripted = true;

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) <= .75f)
                    descript += "narrow ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) > 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) <= 1.4f)
                    descript += "ample ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) > 1.4f && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) <= 2)
                    descript += "broad ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) > 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) <= 3.5f)
                    descript += "fat ";

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) > 3.5f)
                    descript += "distended ";

                descripted = true;
            }
            // 原版 amilyCock() 的乳汁/发情随机描述（33% 概率，且前面长度/粗细已描述时跳过）
            if (Random.Shared.Next(3) == 0 && !descripted)
            {
                var lactation = CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate);

                if (lactation > 0)
                {
                    if (lactation <= 1)
                        descript += Random.Shared.Next(3) switch
                        {
                            0 => "milk moistened ",

                            1 => "slightly lactating ",

                            _ => "milk-dampened ",
                        };
                    else if (lactation <= 2)
                        descript += Random.Shared.Next(3) switch
                        {
                            0 => "lactating ",

                            1 => "milky ",

                            _ => "milk-seeping ",
                        };
                    else
                        descript += Random.Shared.Next(4) switch
                        {
                            0 => "dripping ",

                            1 => "dribbling ",

                            2 => "milk-leaking ",

                            _ => "drooling ",
                        };

                    descripted = true;
                }
            }

            rando = (int)(Random.Shared.NextDouble() * 10);

            switch (rando)
            {
                case >= 0 and <= 4: descript += "cock"; break;
                case 5:
                case 6: descript += "prick"; break;
                case 7: descript += "pecker"; break;
            }

            if (rando > 7)
                descript += "shaft";

            return descript;
        }

        public string AmilyNipples()
        {
            var descripted = false;

            var description = "";

            int rando = 0;

            if (Random.Shared.Next(4) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < .25f)
                {
                    rando = Random.Shared.Next(3);

                    switch (rando)
                    {
                        case 0: description += "tiny "; break;
                        case 1: description += "itty-bitty "; break;
                        case 2: description += "teeny-tiny "; break;
                        case 3: description += "dainty "; break;
                    }
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) >= .4f && CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < 1)
                {
                    rando = Random.Shared.Next(5);

                    switch (rando)
                    {
                        case 0: description += "prominent "; break;
                        case 1: description += "pencil eraser-sized "; break;
                        case 2: description += "eye-catching "; break;
                        case 3: description += "pronounced "; break;
                        case 4: description += "striking "; break;
                    }
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < 2)
                {
                    rando = Random.Shared.Next(4);

                    switch (rando)
                    {
                        case 0: description += "forwards-jutting "; break;
                        case 1: description += "over-sized "; break;
                        case 2: description += "fleshy "; break;
                        case 3: description += "large protruding "; break;
                    }
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < 3.2f)
                {
                    rando = Random.Shared.Next(5);

                    switch (rando)
                    {
                        case 0: description += "elongated "; break;
                        case 1: description += "massive "; break;
                        case 2: description += "awkward "; break;
                        case 3: description += "lavish "; break;
                        case 4: description += "hefty "; break;
                    }
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) >= 3.2f)
                {
                    rando = Random.Shared.Next(4);

                    switch (rando)
                    {
                        case 0: description += "bulky "; break;
                        case 1: description += "ponderous "; break;
                        case 2: description += "unmanageable "; break;
                        case 3: description += "thumb-sized "; break;
                        case 4: description += "cock-sized "; break;
                        case 5: description += "cow-like "; break;
                    }
                }

                descripted = true;
            }

            if ((Random.Shared.Next(3) == 0 && !descripted) && (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) > 0))
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) <= 1)
                    {
                        rando = Random.Shared.Next(3);

                        switch (rando)
                        {
                            case 0: description += "milk moistened "; break;
                            case 1: description += "slightly lactating "; break;
                            case 2: description += "milk-dampened "; break;
                        }
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) > 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) <= 2)
                    {
                        rando = Random.Shared.Next(3);

                        switch (rando)
                        {
                            case 0: description += "lactating "; break;
                            case 1: description += "milky "; break;
                            case 2: description += "milk-seeping "; break;
                        }
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) > 2)
                    {
                        rando = Random.Shared.Next(4);

                        switch (rando)
                        {
                            case 0: description += "dripping "; break;
                            case 1: description += "dribbling "; break;
                            case 2: description += "milk-leaking "; break;
                            case 3: description += "drooling "; break;
                        }
                    }

                    descripted = true;
                }

            rando = Random.Shared.Next(5);

            switch (rando)
            {
                case 0:
                    description += "nipple";

                    break;

                case 1:
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) < .5f)
                        description += "perky nipple";
                    else
                        description += "cherry-like nub";

                    break;

                case 2:
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) >= 1)
                        description += "teat";
                    else
                        description += "nipple";

                    break;

                case 3:
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyLactationRate) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.AmilyNippleLength) >= 1)
                        description += "teat";
                    else
                        description += "nipple";

                    break;

                case 4:
                    description += "nipple";

                    break;
            }

            return description;
        }

        public void CorruptAmilyOralSuckOff()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.OrdersSuckOff".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.JuicesOnHerWord".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.AndWord".Localized());
            }
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.SemicolonBreak".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.TicklingBallsWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.RubsHeadMuskCrotch".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.GrinsDivesIntoTask".Localized());

            if (Player.CockArea(0) > 61)
                OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.ThroatHitsLimit".Localized());

            OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.SkillsHonedPerfection".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.NearEdgeNoRelease".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.NowInControlFeel".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.BallsChurnWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.ThrobsBeforeSaying".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.BellyDistendsEndless".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.FlowEbbsRubsBelly".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CorruptAmilyLickPussiesLikeAPro()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.DeclaresTimeToEat".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.HardeningWord".Localized(
                    Appearance.CockNoun(Player.Cocks[0].Id)
                ));

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.WatchesMesmerizedDrool".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.SpreadsPussyLips".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.JetOfFluidsShoots".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.MoistureWetsFinger".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.GrinsUrgesHerOn".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.NoseSendsChillPleasure".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.CockSpringsAttention".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.AndWord".Localized());
            }

            if (Player.Balls > 0)
            {
                if (!Player.HasCock())
                    OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.CommaSpace".Localized());

                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.BallsRestingOnHead".Localized());
            }

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.HeadBackGirlcumStrand".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.LiftsBallsOntoHead".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.DivesIntoHerTask".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.SlathersNetherlipsSaliva".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.BallsWord".Localized());
            else
                OutputText(Player.ClitDescript());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.PatsHeadPleased".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.HornsMassageBalls".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.ThrobsLeakingPre".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.JumpsSlightlyGrinning".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.WondersAtHerSkill".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.PushesAmilyToGround".Localized());

            if (Player.VaginalCapacity() >= 45 && AmilyFurry())
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.GigglesShovesMuzzle".Localized());
            else
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.GigglesStepFurther".Localized());

                if (AmilyFurry())
                    OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.HoldsMuzzleClosed".Localized());
                else
                    OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.BuriesNoseDeep".Localized());

                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.EyesBulgeHoldingClosed".Localized());

                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.IntentOnOrgasm".Localized());
            }

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.DesperateEagerMinistrations".Localized());

            if (Player.VaginalCapacity() >= 45)
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.PullsOffMoansDeeply".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.LetsGoMoansDeeply".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.GaspsForAirFaceful".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.CoughsGathersJuice".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyLickPussiesLikeAPro.ChucklesDismissesWave".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CorruptAmilyScissorsLikeAPro()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.GrinsAtIdeaOrders".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ThrobsAtOrder".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.SmilesUnderstanding".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ExaminesCrotchClosely".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.IdlyStrokingWord".Localized(
                    AmilyCock()
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                    OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ShiftsAsideReveals".Localized(
                        AmilyBalls()
                    ));
                else
                    OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.DipsDownToHer".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.EyeingHerWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.StrokesBoxFingers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.TryingToMatchBrother".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.SmirksMassagesJuices".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.PlaythingSpadeTail".Localized(
                AmilyTits()
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ObeysFlicksTail".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                if (Player.HasCock())
                    OutputText(Player.CockDescript(0));
                else
                    OutputText(Player.ClitDescript());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.TailUncoilsSuckles".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.LegsSpreadWide".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.HerCockWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.TippedOutOfWay".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.LowersVaginaToHers".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.AmilyMoansWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.WarmJuicesSplash".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.CurlsInPleasurePanting".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.SuccuMouseWrithes".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.AchingCockWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.TimeToSateHeavy".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.PatronizingSmilePat".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ScreamsOrgasmThanks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.VeritableJetSplashes".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.SquelchesSplatsResound".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ThrobsCumSplashes".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.GiveAmilyPurifiedSuccubusMilk.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.FormerPuritanBeast".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.HandsGrabEachCock".Localized());

            OutputText("Scenes.Amilyscene.AmilyDrinksSuccubusDelight.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.PraisesToyThoughtless".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.DrenchesBodyFluids".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.GoesLimpDripping".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.BeamsCleansYouUp".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CorruptAmilysPussyGetsMotherfuckingFucked()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SuggestsRolePlaying".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.GrinsUrgesSheTry".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.OrdersSpreadUndress".Localized());

            if (Player.Pregnancy.Event >= 3)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.GravidProudBump".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.FlopsDownRollsBack".Localized(
                AmilyButt()
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.HandProbesForLube".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyOralSuckOff.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.LoyalSlutNoRoll".Localized(
                Player.Mf("master", "mistress")
            ));

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.WantsCumBloat".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.BabiesNeedCum".Localized(
                    Player.Mf("Master", "Mistress")
                ));

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.PleasedEagerBitch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.BallsCushionCock".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SquealsLegsAroundHips".Localized(
                Player.Mf("Master", "Mistress"),

                Player.AssDescript(),

                AmilyTits()
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SayStartTorturing".Localized());

            if (SceneInstance<JojoScene>.Get().IsJojoCorrupted() && CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) == 0)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.JojoWantsPlay".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.CantFindPlaymate".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.PullsAwayNonchalant".Localized());

            if (Player.CockArea(0) >= 61)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.QuiversTension".Localized(
                    Player.CockDescript(0),

                    Player.CockDescript(0),

                    Player.Mf("master", "mistress"),

                    Player.CockDescript(0),

                    Player.Mf("master", "mistress")
                ));
            else
            {
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.TensionKissMoment".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) >= 5)
                    OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.JetsWashDownPre".Localized());

                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.HalfMindToBrutalize".Localized());

                OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ParagraphBreak".Localized());
            }

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.ThrustsBucksImpale".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.GrinsWillShowHer".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.BallsSlapButtWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.TailThrashesGrunts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SlicesAirFleshyKnife".Localized(
                    AmilyCock()
                ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.BubblingFrothingCunt".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyScissorsLikeAPro.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.TimeToEndThrust".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.UnbornChildrenBath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVaginalWetness) < 5)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.FluidsSlopCrotch".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.CascadePoursFromHer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.CockExplodesQuickly".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.OrgasmFinallyEnds".Localized());

            if (Player.CumQ() >= 1000)
                if (!Player.IsPregnant())
                    OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.WombFullOfCum".Localized());
                else
                    OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.BellyReadyToBurst".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SlumpsOnTopSpent".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.NeglectedVaginaLeaks".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.PantsSighsSatisfied".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.HopesKnockedUp".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.WishesNotPregnant".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.SayEndSoftly".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.ShouldScoldTender".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.RegainedBreathGetsUp".Localized(
                Player.Mf("master", "mistress"),

                Player.Mf("master", "mistress")
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.DoneLickingThighs".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.LicksLipsLovingly".Localized());

            AmilyPreggoChance();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CorruptAmilyCampBonesPCWithHerCock()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.GazeIdeaForms".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.OrdersStayStill".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.BallsHangUnderShaft".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.ShaftIsWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 6)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SmallWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) < 9)
                    OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.AverageWord".Localized());
                else
                    OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.BigWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SkillAtUsingIt".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.WarnsPunishment".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.BallsSeeminglyInflate".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.MoansEdgeThenStop".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SmirksAmusedTaunt".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.TauntsPoorBitch".Localized(
                Player.VaginaDescript()
            ));

            Player.CuntChange((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) * CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth)), true, true, false);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) * CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth) > Player.VaginalCapacity())
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.GrowlsRoughPenetration".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.OverwhelmedGoesBerserk".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.FemLubeSlurping".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.MeatySmackBalls".Localized(
                    Player.AssDescript()
                ));

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.ConsidersSmacking".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.MaybeNotTotallyLost".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.BallsSlappingReverberates".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SquelchingLouderClimax".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.CollapsesIsThatAll".Localized(
                AmilyCock()
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SolemnYesMistress".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.FemcumWashesSeed".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.SlowlyWithdrawsShrinks".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.OrdersFetchRope".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.StrokesLimpDickFoot".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.StrokesLimpDickHands".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.CoaxesIntoErection".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.OrdersCleanMeOff".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.MightReturnFavor".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.TieHerUpFavor".Localized());

            Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CorruptAmilyBuckFutter()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.OrdersHandsAndKnees".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.StandsErectTinyBead".Localized(
                    AmilyCock()
                ));

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.ParagraphBreakThird".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.UndressesPressesCrack".Localized(
                Player.CockDescript(0),

                Player.CockDescript(0),

                Player.CockDescript(0)
            ));

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.CockheadPressesInside".Localized(
                Player.Mf("Master", "Mistress")
            ));

            if (Player.CockArea(0) >= 61)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.ForcesShaftDeep".Localized(
                    Player.Mf("master", "mistress")
                ));
            else
            {
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.HipsMeetCushion".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.BallsSlapPussyJuices".Localized());
                else
                    OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.DrawsBackPlungesDeep".Localized());
            }

            OutputText("Scenes.Amilyscene.CorruptAmilysPussyGetsMotherfuckingFucked.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.ReachedLimitBrutalFuck".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.SlapsAssYellsTighter".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.BallsChurnTense".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.ThrobTenseLastPush".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.BellyFillsInflates".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.JetsSpillPullsOut".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.CumBackflowsPlugs".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.CumLeaksPlugsHand".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyCampBonesPCWithHerCock.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.OrdersFinishWork".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.HeavyBellyJiggling".Localized());
            else
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.BoobsJigglingWord".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.LooksAtErectCock".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.SpilledJuicesOnBalls".Localized());

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.CleanThenPraised".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyIsTotallyALesbo()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyIsTotallyALesbo.SheWaitsForYou".Localized());

            OutputText("Scenes.Amilyscene.AmilyIsTotallyALesbo.WantsToTalkAbout".Localized());

            OutputText("Scenes.Amilyscene.AmilyIsTotallyALesbo.TellsHerSlowDown".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyConfessedLesbian, 1);

            ClearButtons();

            AddButton(0, "Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), AmilyLesboStopHer);

            AddButton(1, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), AmilyLesboLetHerGo);
        }

        public void AmilyLesboStopHer()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyLesboStopHer.HandClaspsShoulder".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyConfessedLesbian, 2);

            DoNext(() => GirlyGirlMouseSex());
        }

        public void AmilyLesboLetHerGo()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyLesboLetHerGo.WatchesGuilty".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) > 10)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, 10);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyPostConfessionGirlRemeeting()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyPostConfessionGirlRemeeting.HappyShyGreeting".Localized());

            OutputText("Scenes.Amilyscene.AmilyPostConfessionGirlRemeeting.AgreesAsksMatter".Localized());

            OutputText("Scenes.Amilyscene.AmilyPostConfessionGirlRemeeting.ScuffsGroundNervous".Localized());

            OutputText("Scenes.Amilyscene.AmilyPostConfessionGirlRemeeting.NodsAsksWhereTo".Localized());

            OutputText("Scenes.Amilyscene.AmilyPostConfessionGirlRemeeting.LooksDownShyly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AmilyHermQuestAccept);

            AddButton(1, "Common.Button.IllDoIt".Localized(), AmilyHermQuestLetMe);

            AddButton(2, "Common.Button.SlowDown".Localized(), AmilyHermQuestWait);

            AddButton(3, "Common.Button.Disgust".Localized(), AmilyHermQuestDisgust);
        }

        public void AmilyHermQuestAccept()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyHermQuestAccept.LovesHerForHer".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestAccept.RelievedSighSmile".Localized());

            if (silly)
                OutputText("Scenes.Amilyscene.AmilyHermQuestAccept.AmilHeDeclaration".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyHermQuestAccept.CanDoItNow".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestAccept.ExcusesHerselfLeaves".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 4);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyHermQuestLetMe()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyHermQuestLetMe.RespectsButDeclines".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestLetMe.BeamsGladImportant".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestLetMe.WouldFreakOutToo".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestLetMe.FellForRightPerson".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestLetMe.HeadsBackToCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyHermQuestWait()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyHermQuestWait.SuddenBigDecision".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestWait.NodsDeepBreath".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestWait.WillLetHerKnow".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 5);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyHermQuestDisgust()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyHermQuestDisgust.GrimacesDisgusted".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestDisgust.StepsBackShocked".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestDisgust.ScowlsCallsDisgusting".Localized());

            OutputText("Scenes.Amilyscene.AmilyHermQuestDisgust.HeadFallsTearsRuns".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyDiscussChildren()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyDiscussChildren.GivenSituationThought".Localized());

            OutputText("Scenes.Amilyscene.AmilyDiscussChildren.WillingToGrowCock".Localized());

            OutputText("Scenes.Amilyscene.AmilyDiscussChildren.YouGrowDickYourself".Localized());

            OutputText("Scenes.Amilyscene.AmilyDiscussChildren.OnlyOneMustChange".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.HermAmily".Localized(), AmilyHermQuestAccept);

            AddButton(1, "Common.Button.IllDoIt".Localized(), AmilyHermQuestLetMe);

            AddButton(2, "Scenes.CorruptedGlade.NeverMind".Localized(), AmilyVillageMenu);
        }

        public void AmilyNewCock()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyNewCock.ReturnsToHerAbode".Localized());

            OutputText("Scenes.Amilyscene.AmilyNewCock.GreetsPacingNervous".Localized());

            OutputText("Scenes.Amilyscene.AmilyNewCock.NervousToShowIt".Localized());

            OutputText("Scenes.Amilyscene.AmilyNewCock.ReassuresAccepted".Localized());

            OutputText("Scenes.Amilyscene.AmilyNewCock.LooksAwayStammers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, 4);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 0);

            DoNext(() => HermilyOnFemalePC());
        }

        public void AmilyPCNewCock()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyPCNewCock.FreshAirBounce".Localized());

            OutputText("Scenes.Amilyscene.AmilyPCNewCock.HopedToSeeYou".Localized());

            OutputText("Scenes.Amilyscene.AmilyPCNewCock.ReallyDidItShock".Localized());

            if (Player.BiggestCockLength() < 5.0f)
                OutputText("Scenes.Amilyscene.AmilyPCNewCock.ThankfulModestSize".Localized());
            else
                if (Player.BiggestCockLength() < 9.0f)
                    OutputText("Scenes.Amilyscene.AmilyPCNewCock.BiggerThanMice".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilyPCNewCock.ComplainsAboutSize".Localized());

            OutputText("Scenes.Amilyscene.AmilyPCNewCock.ChucklesAtShock".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 2);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 20);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

            DoNext(() => AmilySexHappens());
        }

        public void GirlyGirlMouseSex()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.AmilyLeadsByHand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesFuckedFempc) == 0)
                OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.AmilyStopsBlushing".Localized());
            else
                OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.RemembersFirstTime".Localized());

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.SmileChinKiss".Localized());

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.GuidesToBed".Localized(
                Player.VaginaDescript(0)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesFuckedFempc) == 0)
                OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.AsksAboutLicking".Localized());
            else
                OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.AmilyLicksDeeply".Localized());

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.SlowIntenseLicking".Localized());

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.OverflowingWetness".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.BothCumTogether".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesFuckedFempc) == 0)
                OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.NeverFeltSoGood".Localized());
            else
                OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.QuestionsBeingLesbian".Localized());

            OutputText("Scenes.Amilyscene.GirlyGirlMouseSex.RecoversThenSmiles".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.AmilyTimesFuckedFempc, CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesFuckedFempc) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void HermilyOnFemalePC()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.ErectionHampersLeading".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermTimesFuckedByFempc) == 0)
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.DisbeliefAtOwnCock".Localized());
            else
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.BuryingThrobbingThing".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermTimesFuckedByFempc) == 0)
                    OutputText("Scenes.Amilyscene.HermilyOnFemalePC.TalkedIntoThis".Localized());
                else
                    OutputText("Scenes.Amilyscene.HermilyOnFemalePC.WantsHerTurn".Localized());

            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.IndicateUndress".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.LegsInReadiness".Localized());
            else
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.CuntInReadiness".Localized());

            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.KneelsBetweenLegs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermTimesFuckedByFempc) == 0)
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.AsksIfSure".Localized());
            else
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.GripsHipsPlunges".Localized());

            Player.CuntChange((CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) * CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth)), true, true, false);

            OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.InexpertMinistrations".Localized(
                Player.VaginaDescript()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermTimesFuckedByFempc) == 0)
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.SurprisedByFeeling".Localized());
            else
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.WondersAboutMen".Localized());

            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.CaressHerCheek".Localized());

            if (Player.IsPregnant())
            {
                if (Player.Pregnancy.Type == PregnancyIds.Amily)
                    OutputText("Scenes.Amilyscene.HermilyOnFemalePC.WomanBecomingDad".Localized());
                else
                    OutputText("Scenes.Amilyscene.HermilyOnFemalePC.WantsNextChild".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.HermilyOnFemalePC.HopesForPregnancy".Localized());

                Player.KnockUp(PregnancyIds.Amily, PregnancyIds.MouseIncubation);
            }

            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.EmbraceThenLeave".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermTimesFuckedByFempc, CoC.Instance.Flags.Get(GameFlagIds.AmilyHermTimesFuckedByFempc) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyFuckCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void PcBirthsAmilysKidsQuestVersion()
        {

            AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.PcTimesBirthedAmilykids, CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                PlayerBirthsWifAmilyMiceInCamp();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) >= 5)
            {
                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.WakeToLaborPains".Localized());

                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.CervixForcedOpen".Localized());

                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.PainCutsReply".Localized());

                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.HelpsBabyLatch".Localized());

                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.BodyRestored".Localized());

                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.InstructsYouRest".Localized());

                OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.SinksIntoSleep".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.PcPendingPreggers, 1);

                DoNext(() => PostBirthingEndChoices());

                return;
            }

            OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.WakeToLaborAgain".Localized());

            OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.CervixForcedAgain".Localized());

            OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.PainCutsReplyAgain".Localized());

            OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.LatchToChest".Localized());

            OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.BodyRestoredAgain".Localized());

            OutputText("Scenes.Amilyscene.PcBirthsAmilysKidsQuestVersion.NeverExpectedThis".Localized());
        }

        public void PostBirthingEndChoices()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.PostBirthingEndChoices.ChildrenHaveLeft".Localized());

            OutputText("Scenes.Amilyscene.PostBirthingEndChoices.WantedHumanMate".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Amilyscene.PostBirthingEndChoices.AnotherWomanOption".Localized());
            else
                OutputText("Scenes.Amilyscene.PostBirthingEndChoices.HermaphroditeOption".Localized());

            OutputText("Scenes.Amilyscene.PostBirthingEndChoices.LovesYouStill".Localized());

            OutputText("Scenes.Amilyscene.PostBirthingEndChoices.AcceptOfferQuestion".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcTimesBirthedAmilykids, CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptAmilyAsYourFemaleWaifu);

            AddButton(1, "Common.Button.StayFriends".Localized(), DeclineButBeFriends);

            AddButton(2, "Common.Button.ShootDown".Localized(), NotInterestedInDumbshitMouseBitches);
        }

        public void AcceptAmilyAsYourFemaleWaifu()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AcceptAmilyAsYourFemaleWaifu.SurprisedThenAccepts".Localized());

            OutputText("Scenes.Amilyscene.AcceptAmilyAsYourFemaleWaifu.JokesMovingIn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, .3f);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, 6);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, 6);

            DoNext(PlayerMenu);
        }

        public void DeclineButBeFriends()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.DeclineButBeFriends.ThinkThenRefuse".Localized());

            OutputText("Scenes.Amilyscene.DeclineButBeFriends.DisappointedButAgrees".Localized());

            DoNext(PlayerMenu);
        }

        public void NotInterestedInDumbshitMouseBitches()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.NotInterestedInDumbshitMouseBitches.ColdRefusal".Localized());

            OutputText("Scenes.Amilyscene.NotInterestedInDumbshitMouseBitches.HeartbrokenReel".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(PlayerMenu);
        }

        public void AmilyNewGenderConfrontation()
        {
            ClearOutput();

            AmilySprite();

            float oldGender = CoC.Instance.Flags.Get(GameFlagIds.AmilyPcGender);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyPcGender, (int)Player.Gender);

            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.NoticesChangedScent".Localized());

            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.AdmitsChange".Localized());

            if (oldGender == 1)
            {
                if (Player.Gender == Gender.Female)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.NowWhatToDo".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.HurtAsksWhy".Localized());
                        else
                        {
                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.ResolveAfterUpset".Localized());

                            DoNext(() => GirlyGirlMouseSex());

                            return;
                        }
                }
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.AppalledAsksHerm".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.RefusesBedSharing".Localized());
                            else
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.ShockThenAccepts".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 2);

                                AmilyVillageMenu();

                                return;
                            }
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DisdainForStupidity".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.KinkInRelationship".Localized());

                                AmilyVillageMenu();

                                return;
                            }
                            else
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.ConcernedForYourSake".Localized());

                                AmilyVillageMenu();

                                return;
                            }
            }
            else
                if (oldGender == 2)
                {
                    if (Player.Gender == Gender.Male)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 3)
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 0);

                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.GaspsInGratitude".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 20);

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

                            DoNext(() => AmilySexHappens());

                            return;
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                        {
                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.SurprisedAtManhood".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 15);

                            if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0)
                            {
                                CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

                                AmilyVillageMenu();

                                return;
                            }
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.SurprisedAndThankful".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 5);

                                if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0)
                                {
                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

                                    AmilyVillageMenu();

                                    return;
                                }
                            }
                            else
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.PleasedFeignsAnnoyance".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

                                DoNext(() => AmilySexHappens());

                                return;
                            }
                    }
                    else
                        if (Player.Gender == Gender.Hermaphrodite)
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DisbeliefAtYourChoice".Localized());
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.NotSureTogether".Localized());
                                    else
                                    {
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DeterminedAcceptance".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 2);

                                        AmilyVillageMenu();

                                        return;
                                    }
                            }
                            else
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.JealousOfMe".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 2);

                                AmilyVillageMenu();

                                return;
                            }
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DisdainForStupidityAgain".Localized());
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.KinkInRelationshipAgain".Localized());
                                else
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.ConcernedForSakeAgain".Localized());
                }
                else
                    if (oldGender == 3)
                    {
                        if (Player.Gender == Gender.Male)
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.ApprovesYourLook".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 5);

                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) == 2)
                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                                {
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.HappyYouChanged".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2);

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

                                    AmilyVillageMenu();

                                    return;
                                }
                                else
                                {
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DidntNeedToChange".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2);

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

                                    AmilyVillageMenu();

                                    return;
                                }
                        }
                        else
                            if (Player.Gender == Gender.Female)
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                                {
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.GladAnotherWoman".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2);

                                    AmilyVillageMenu();

                                    return;
                                }
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                                    {
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.LikesHavingSomebody".Localized());

                                        AmilyVillageMenu();

                                        return;
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DisappointedButLoving".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.AmilyConfessedLesbian, 2);

                                        AmilyVillageMenu();

                                        return;
                                    }
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DisdainForStupidityThird".Localized());
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                                    {
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.KinkInRelationshipThird".Localized());

                                        AmilyVillageMenu();

                                        return;
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.ConcernedForSakeThird".Localized());

                                        AmilyVillageMenu();

                                        return;
                                    }
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 15)
                        {
                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DisdainHidesRelief".Localized());

                            if (Player.HasCock())
                            {
                                OutputText(Player.CockDescript(0));

                                if (Player.HasVagina())
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.AndWord".Localized());
                            }

                            if (Player.HasVagina())
                                OutputText(Player.VaginaDescript());

                            OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.MaybeNotStupid".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) < 40)
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.NoticesYourCock".Localized());

                                if (Player.HasCock())
                                {
                                    OutputText(Player.CockDescript(0));

                                    if (Player.HasVagina())
                                        OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.AndWordAgain".Localized());
                                }

                                if (Player.HasVagina())
                                    OutputText(Player.VaginaDescript());

                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.SmilesContinueTask".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.WelcomesRestoredBody".Localized());

                                if (Player.HasCock())
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.GrowsErection".Localized());

                                if (Player.HasVagina() && Player.HasCock())
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.AndWordThird".Localized());

                                if (Player.HasVagina())
                                    OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.MakesYouClench".Localized(
                                        Player.VaginaDescript(0)
                                    ));

                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.GladRecoveredGender".Localized());

                                OutputText("Scenes.Amilyscene.AmilyNewGenderConfrontation.DepartsCheerfully".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(25 + Player.Sensitivity / 10)));
                            }

            DoNext(() => ReturnToCamp(1));
        }

        public void WhyNotHerms()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.WhyNotHerms.PondersHermQuestion".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.AbsorbedInQuestion".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.AsksWhatMatters".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.RecallsRoleTalk".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.AdmitsHerRole".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.AsksWhyHuman".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.HumansPureWeakSeed".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.SarcasticChoppedLiver".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.FacesYouSquarely".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.ArgumentForHerms".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.CallsUnnatural".Localized());

            OutputText("Scenes.Amilyscene.WhyNotHerms.FleesIntoRubble".Localized());

            DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, CoC.Instance.Flags.Get(GameFlagIds.AmilyHermQuest) + 1);
        }

        public void MaybeHermsAintAllBadBITCH()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.MaybeHermsAintAllBadBITCH.ReturnsToVillage".Localized());

            OutputText("Scenes.Amilyscene.MaybeHermsAintAllBadBITCH.ApologizesForInsult".Localized());

            OutputText("Scenes.Amilyscene.MaybeHermsAintAllBadBITCH.AvertsGazeThinking".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), BeAmilysDadAsAHerm);

            AddButton(1, "Common.Button.No".Localized(), FuckNoYouWontBeAmilysHermDaddy);

            if (AmilyFurry())
                AddButton(2, "Common.Button.NoFurry".Localized(), AmilyNoFur);
        }

        public void BeAmilysDadAsAHerm()
        {
            ClearOutput();

            AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyHermQuest, 2);

            OutputText("Scenes.Amilyscene.BeAmilysDadAsAHerm.ForgivesAndBreeds".Localized());

            DoNext(() => AmilySexHappens());
        }

        public void FuckNoYouWontBeAmilysHermDaddy()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.FuckNoYouWontBeAmilysHermDaddy.ScoffsAtInsult".Localized());

            OutputText("Scenes.Amilyscene.FuckNoYouWontBeAmilysHermDaddy.AdmitsUnforgivable".Localized());

            OutputText("Scenes.Amilyscene.FuckNoYouWontBeAmilysHermDaddy.SadDeparture".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void MakeAmilyAHerm()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.PointsOutCloseness".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.AdmitsCannotUnderstand".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.AsksHerToTry".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.NervousRefusal".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.BestForHerPlans".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.ReluctantAgreement".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.AmilyBecomesHerm".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.NowHasPenis".Localized());

            OutputText("Scenes.Amilyscene.MakeAmilyAHerm.AgreesToBedroom".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangLength, 4);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyWangGirth, 1);

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

            DoNext(() => HermilyOnFemalePC());
        }

        public void PlayerBirthsWifAmilyMiceInCamp()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.WakesToLaborPains".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.CervixForcedOpen".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.IrritatedGrunt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) > 0)
                OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.NotFirstOffspring".Localized());
            else
                OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.GladYouPushThem".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.ReplyCutByPain".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.BodyRestored".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.OnlyTracesRemain".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.WorryAboutCare".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.ContactedSiblings".Localized());

            OutputText("Scenes.Amilyscene.PlayerBirthsWifAmilyMiceInCamp.NodsAgreement".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcTimesBirthedAmilykids, CoC.Instance.Flags.Get(GameFlagIds.PcTimesBirthedAmilykids) + 1);
        }

        public void AmilyPopsOutKidsInCamp()
        {

            AmilySprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyBirthTotal, CoC.Instance.Flags.Get(GameFlagIds.AmilyBirthTotal) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.CampDistressCry".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.TouchSwollenMiddle".Localized(
                    AmilyFurry() ? "turns her little " + (AmilyFurry() ? "Common.Text.Muzzle".Localized() : "Common.Text.Lips".Localized()) + " skywards and " : ""
                ));

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.PlacesBabyBreast".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.AsManyAsLast".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ChemicalsInBlood".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.WorryAboutCare".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ContactedSiblings".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.NodsAgreement".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyIncestCountdownTimer) == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyIncestCountdownTimer, 1);

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ApproachPregnantSlut".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.BegsForMoreBabies".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.JacksOffFuriously".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.BallsQuiverBreeze".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.EcstaticContortion".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.BirthImminent".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.CumSpurtFragment".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.FirstMouseEmerges".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.UsesBallsToEscape".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.FluidJetFrees".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.AllChildrenFreed".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.SomeStillWaiting".Localized(
                    Player.Mf("Master", "Mistress")
                ));

                if (Player.Gender == Gender.Genderless)
                {
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.CannotFeedHerNow".Localized());

                    return;
                }

                if (Player.HasCock() && (Player.Gender == Gender.Male || Random.Shared.Next(2) == 0))
                {
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.UndressShowingCock".Localized());

                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ErectCockWord".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.AndPlaceholder".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.StraddlesHerFace".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.SwellsWhileSuckling".Localized());

                    OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.ParagraphBreakAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.UndressShowingBoth".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ErectCockAndWord".Localized());

                    OutputText(Player.VaginaDescript());

                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.OffersCuntToHer".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ImpatientUrging".Localized());

                    OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.EagerTongueWork".Localized());

                    if (Player.Wetness() >= 5)
                        OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.HandlesGirlCum".Localized());

                    OutputText("Scenes.Amilyscene.CorruptAmilyBuckFutter.ParagraphBreakThird".Localized());
                }

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.LastChildFilled".Localized());

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.AffectionateNuzzle".Localized(
                    Player.Mf("master", "mistress"),

                    Player.Mf("master", "mistress")
                ));

                OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ChildrenRushOver".Localized(
                    Player.Mf("master", "mistress"),

                    Player.Mf("master", "mistress")
                ));

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));
            }
        }

        public void MakeChildren()
        {
            ClearOutput();

            AmilySprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyAllowsFertility) == 1)
            {
                OutputText("Scenes.Amilyscene.MakeChildren.AsksBackOnHerbs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyAllowsFertility, 0);
            }
            else
            {
                OutputText("Scenes.Amilyscene.MakeChildren.SomethingDifferentInMind".Localized());

                OutputText("Scenes.Amilyscene.MakeChildren.StartledMoreChildren".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyAllowsFertility, 1);
            }

            DoNext(() => AmilyFollowerEncounter());
        }

        public void MeetAmilyAsACorruptAsshat()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.ChecksOnAmily".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5)
                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.SlowToAppear".Localized());

            if (__sw0_0 == 6 || __sw0_0 == 7 || __sw0_0 == 8)
                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.DelayedArrival".Localized());

            if (true )
                OutputText("Scenes.Amilyscene.AmilyVillageMenu.MaterializesQuick".Localized());

            OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.TroubledGreeting".Localized());

            if (!Player.HasPerk(PerkIds.Evade) && !Player.HasPerk(PerkIds.Flexibility))
            {
                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.CaughtOffGuard".Localized());

                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.CallsYouTainted".Localized());

                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.TwoHoursParalyzed".Localized());

                if (Player.Intelligence >= 40)
                    OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.PlanNeedsLustDraft".Localized());
                else
                    OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.RecallsPotions".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.DodgesTheDart".Localized());

                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.CallsYouTaintedAgain".Localized());

                OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.AngrySheAttacked".Localized());

                if (Player.Intelligence >= 40)
                    OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.PlanNeedsDraftAgain".Localized());
                else
                    OutputText("Scenes.Amilyscene.MeetAmilyAsACorruptAsshat.RecallsPotionsAgain".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);
        }

        public void CookAmilyASnack()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyDrugMadeCounter) > 0)
            {
                if (!(Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft))) || !Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle)))
                {
                    OutputText("Scenes.Amilyscene.CookAmilyASnack.LacksIngredients".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return;
                }
                else
                    if (Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.Amilyscene.CookAmilyASnack.NeedsCockOrPussy".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return;
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.CookAmilyASnack.PourLustAndAle".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.EachCockAroused".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Amilyscene.CookAmilyASnack.AndWord".Localized());
                        }

                        if (Player.HasVagina())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.VaginaGrowsWet".Localized());

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.ReadyNextStep".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.MilksCockDrops".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Amilyscene.CookAmilyASnack.AddsGirlyTouch".Localized());

                            OutputText("Scenes.Amilyscene.HermilyOnFemalePC.ParagraphBreak".Localized());
                        }

                        if (Player.HasVagina())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.PinchesClitWet".Localized(
                                Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.MixtureTurnsPink".Localized());

                        if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
                        else
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.FuckDraft));

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoblinAle));

                        Player.CreateKeyItem("Potent Mixture", 0, 0, 0, 0);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyDrugMadeCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyDrugMadeCounter) + 1);
                    }
            }
            else
                if (!(Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft))) || !Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle)))
                {
                    OutputText("Scenes.Amilyscene.CookAmilyASnack.DecidesToWait".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return;
                }
                else
                    if (Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.Amilyscene.CookAmilyASnack.DecidesToTurnBack".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return;
                    }
                    else
                    {
                        AmilySprite(true);

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.PourDraftAndAle".Localized());

                        if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.LustDraftWord".Localized());
                        else
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.FuckDraftWord".Localized());

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.StirsWithBranch".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.CockAroused".Localized());

                        if (Player.HasCock() && Player.HasVagina())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.AndYourWord".Localized());

                        if (Player.HasVagina())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.VaginaNearlyWet".Localized());

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.WickedSmilePlan".Localized());

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.MissingIngredient".Localized());

                        if (Player.HasCock())
                            OutputText(Player.CockDescript(0));

                        if (Player.HasCock() && Player.HasVagina())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.AndWordAgain".Localized());

                        if (Player.HasVagina())
                            OutputText(Player.VaginaDescript());

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.RealizesSpecialSauce".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.MilksCockDropsAgain".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Amilyscene.CookAmilyASnack.AddsGirlyTouchAgain".Localized());

                            OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ParagraphBreak".Localized());
                        }

                        if (Player.HasVagina())
                            OutputText("Scenes.Amilyscene.CookAmilyASnack.PinchesClitWetAgain".Localized(
                                Player.VaginaDescript()
                            ));

                        OutputText("Scenes.Amilyscene.CookAmilyASnack.MixtureTurnsPinkAgain".Localized());

                        if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
                        else
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.FuckDraft));

                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoblinAle));

                        Player.CreateKeyItem("Potent Mixture", 0, 0, 0, 0);

                        CoC.Instance.Flags.Set(GameFlagIds.AmilyDrugMadeCounter, CoC.Instance.Flags.Get(GameFlagIds.AmilyDrugMadeCounter) + 1);
                    }

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void SendCorruptCuntToFarm()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.SendCorruptCuntToFarm.SendPetToFarm".Localized());

            OutputText("Scenes.Amilyscene.SendCorruptCuntToFarm.HorrorSendingAway".Localized());

            OutputText("Scenes.Amilyscene.SendCorruptCuntToFarm.CalmedButDoubtful".Localized());

            OutputText("Scenes.Amilyscene.SendCorruptCuntToFarm.GoodForWhitney".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmAmily, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BackToCamp()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.BackToCamp.OrdersReturnToCamp".Localized());

            OutputText("Scenes.Amilyscene.BackToCamp.PurpleBlurDeparts".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmAmily, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void HarvestMilk()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.HarvestMilk.WantsMilkingMachine".Localized());

            OutputText("Scenes.Amilyscene.HarvestMilk.UsedLikeAnimal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmUpgradesRefinery) == 0)
                OutputText("Scenes.Amilyscene.HarvestMilk.MilkNotMuchUse".Localized());
            else
                OutputText("Scenes.Amilyscene.HarvestMilk.HonoredToServe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionAmily, 1);

            DoNext(FarmCorruption.RootScene);
        }

        public void StopHarvestingMilk()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.StopHarvestingMilk.StopSuccubusMilk".Localized());

            OutputText("Scenes.Amilyscene.StopHarvestingMilk.FabulousIdeaMoans".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionAmily, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void TalkWithCORRUPTCUNT(bool sexAfter = false)
        {
            ClearOutput();

            AmilySprite();

            float convo = Random.Shared.Next(13);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAffection, CoC.Instance.Flags.Get(GameFlagIds.AmilyAffection) + 2 + Random.Shared.Next(3));

            Player.ApplyStats(new DynStat(GameStats.Corruption, .34f));

            switch ((int)convo)
            {
                case 0:

                    if (!TalkCorruptAmilyItems(sexAfter))
                        return;

                    break;

                case 1: TalkCorruptAmilyMinotaurs(); break;
                case 2: TalkCorruptAmilySandWitches(); break;
                case 3: TalkCorruptAmilyGiantBees(); break;
                case 4: TalkCorruptAmilyCultists(); break;
                case 5: TalkCorruptAmilyImps(); break;
                case 6: TalkCorruptAmilySharkGirls(); break;
                case 7: TalkCorruptAmilyGoblins(); break;
                case 8: TalkCorruptAmilyYourVillage(); break;
                case 9: TalkCorruptAmilyHerFamily(); break;
                case 10: TalkCorruptAmilyTheAttack(); break;
                case 11: TalkCorruptAmilySurvival(); break;
                case 12: TalkCorruptAmilyKitsune(); break;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public bool TalkCorruptAmilyItems(bool sexAfter)
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskAboutAdventures".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ShareAlchemyKnowledge".Localized(
                Player.Mf("Master", "Mistress")
            ));

            var itemSlot = Player.Inventory.Slots[0];

            if (itemSlot.Quantity == 0)
            {
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.CallWhenNeedKnowledge".Localized());

                if (sexAfter)
                    DoNext(() => AmilySexHappens());
                else
                    DoNext(() => ReturnToCamp(1));

                return false;
            }

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.RememberPocketItem".Localized());

            var itemId = itemSlot.Item.Id;

            if (itemId == ItemIds.Equinum)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HorseEssenceDistill".Localized());
            else if (itemId == ConsumableIds.CaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DogDickKnot".Localized());
            else if (itemId == ConsumableIds.LargeCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DogDickKnotAgain".Localized());
            else if (itemId == ConsumableIds.DoubleCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DogDickKnotThird".Localized());
            else if (itemId == ConsumableIds.BlackCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DogDickKnotFourth".Localized());
            else if (itemId == ConsumableIds.KnottyCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DogDickKnotFifth".Localized());
            else if (itemId == ConsumableIds.BulbyCaninePepper)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DogDickKnotSixth".Localized());
            else if (itemId == ConsumableIds.IncubiDraft || itemId == ConsumableIds.SuccubiMilk)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DemonFluidScent".Localized(
                    Player.Mf("master", "mistress")
                ));
            else if (itemId == ConsumableIds.SuccubusDelight)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SuccubiTaintWarning".Localized());
            else if (itemId == ConsumableIds.SlimyClothoth)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.CumslutIgnorance".Localized());
            else if (itemId == ConsumableIds.BeeHoney)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GiantBeeHoney".Localized(
                    Player.Mf("master", "mistress")
                ));
            else if (itemId == ConsumableIds.PureHoney)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PureBeeHoney".Localized());

            if (itemId == ConsumableIds.W_FRUIT)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhiskerFruit".Localized());

            if (itemId == ConsumableIds.PigtailTruffle || itemId == ConsumableIds.BoarTruffle)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PigtailTruffle".Localized());
            else if (itemId == ItemIds.GreenGel)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GreenGelSlime".Localized());
            else if (itemId == UseableIds.BeeChitin)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SlimeArmorSuit".Localized());
            else if (itemId == ConsumableIds.ToughSilk)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SpiderSilkArmor".Localized());
            else if (itemId == ConsumableIds.D_SCALE)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DragonScaleArmor".Localized());
            else if (itemId == UseableIds.ImpSkull)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ImpSkullDisapproval".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ShrugsUnfamiliar".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WaveHerOff".Localized());

            return true;
        }

        public void TalkCorruptAmilyMinotaurs()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.MinotaurTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PraiseUntouched".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhyMinotaursDangerous".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.MinotaurRutting".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskAdviceMinotaurs".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.CouldTakeThemDown".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskWhatSheDid".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.FoughtItOff".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ThinkAboutItLeave".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));
        }

        public void TalkCorruptAmilySandWitches()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SandWitchTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SandWitches".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskWhySheThinks".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.TwoPairsBreasts".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HowSheFoundOut".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhatSpellWouldDo".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.MilkProducingSlave".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.NuzzlesYourCrotch".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.StoneOrbSpell".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ShoveItYourself".Localized(
                Player.Mf("Master", "Mistress")
            ));
        }

        public void TalkCorruptAmilyGiantBees()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GiantBeeTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GiantBeesKinkyRace".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskToExplain".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.OfficialNameGiantBees".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ContinuesAtConfusion".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.BeesHatchFromEggs".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ExpressionTellsStory".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ShrugsNoBetter".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HypnoticThrumming".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HowSheKnows".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.TradeHoney".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.StuffYourAssLater".Localized());
        }

        public void TalkCorruptAmilyCultists()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DressUpOffer".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ForgiveIgnorance".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.KinkyWearOffer".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WillThinkAboutIt".Localized());
        }

        public void TalkCorruptAmilyImps()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ImpTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ImpsWeakSmall".Localized());

            if (Player.CockArea(0) < 24 && Player.HasCock())
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GrowBiggerCock".Localized(
                    Player.Mf("Master", "Mistress")
                ));
            else
                if (Player.HasCock())
                    OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.TakeHugeCock".Localized(
                        Player.Mf("Master", "Mistress")
                    ));
                else
                    OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GrowHugeDick".Localized(
                        Player.Mf("Master", "Mistress")
                    ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.MoistThighsAsk".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DismissWithWave".Localized());
        }

        public void TalkCorruptAmilySharkGirls()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SharkGirlTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HumansBecameSharkGirls".Localized());

            if (!SceneInstance<IzmaScene>.Get().IzmaFollower())
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.NoMalesLeft".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.FuckLikeThem".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.CallWhenWantFuck".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));
        }

        public void TalkCorruptAmilyGoblins()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GoblinTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GoblinsApproval".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.BitesLipSheepish".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.LovesKnockedUp".Localized(
                Player.Mf("master", "mistress")
            ));

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.GiveBirthMice".Localized());
            else
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PleaseKnockMeUp".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PatHeadGoodGirl".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));
        }

        public void TalkCorruptAmilyYourVillage()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskAboutHerPast".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhyInterestedPast".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhatVillageWasLike".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.BoringSubject".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.FoolsLivingFoolsLife".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PatHeadNuzzle".Localized());
        }

        public void TalkCorruptAmilyHerFamily()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskAboutFamily".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhyInterestedPastAgain".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskWhoSheWas".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.NobodyBeforeYou".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskAboutFather".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.FatherBusyExperiments".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskAboutMother".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.MotherWasHunter".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PushAwaySorry".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.CaughtByDemons".Localized());
        }

        public void TalkCorruptAmilyTheAttack()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskVillageDestroyed".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhyInterestedPastThird".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskHowVillageDestroyed".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.RecallsAttack".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DemonsAttacked".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.NotPickedBest".Localized(
                Player.Mf("master", "mistress")
            ));
        }

        public void TalkCorruptAmilySurvival()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskHowSheSurvived".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WhyInterestedPastFourth".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HowSheSurvivedAlone".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.RanToHouse".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskDailyRoutine".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.BoringRoutine".Localized(
                Player.Mf("master", "mistress"),

                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.DismissSatisfied".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.CockHardeningWord".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ExcitementSuffix".Localized());
        }

        public void TalkCorruptAmilyKitsune()
        {
            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.StrangeCreatureTales".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.WeirdCreatures".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.AskWhatSheMeans".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.NotExactlyDemons".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ContinueStory".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.TrapInIllusions".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ChastiseHoldingBack".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HeardFromTravelers".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.HowAnyoneKnows".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.PausesAnotherFact".Localized());

            OutputText("Scenes.Amilyscene.TalkWithCORRUPTCUNT.ThinkAboutItMasturbate".Localized());
        }

        public void ConquerThatMouseBitch()
        {
            ClearOutput();

            AmilySprite();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Amilyscene.ConquerThatMouseBitch.AmilyFallsUnconscious".Localized());
            else
                OutputText("Scenes.Amilyscene.ConquerThatMouseBitch.DropsKnifeMasturbates".Localized());

            ChooseYourAmilyRape();
        }

        public void ChooseYourAmilyRape()
        {

            AmilySprite(true);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 0)
                DoNext(() => RapeCorruptAmily1());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 1)
                {
                    if (Player.Gender == Gender.Male)
                        DoNext(() => RapeCorruptAmily2Male());
                    else
                        if (Player.Gender == Gender.Female)
                            DoNext(() => RapeCorruptAmily2Female());
                        else
                            if (Player.Gender == Gender.Hermaphrodite)
                            {
                                ClearButtons();

                                AddButton(0, "Common.Button.Malefocus".Localized(), RapeCorruptAmily2Male);

                                AddButton(1, "Common.Button.Femalefocus".Localized(), RapeCorruptAmily2Female);
                            }
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 2)
                    {
                        if (Player.Gender == Gender.Male)
                            DoNext(() => RapeCorruptAmily3Male());
                        else
                            if (Player.Gender == Gender.Female)
                                DoNext(() => RapeCorruptAmily3Female());
                            else
                                if (Player.Gender == Gender.Hermaphrodite)
                                {
                                    ClearButtons();

                                    AddButton(0, "Common.Button.Malefocus".Localized(), RapeCorruptAmily3Male);

                                    AddButton(1, "Common.Button.Femalefocus".Localized(), RapeCorruptAmily3Female);
                                }
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) == 3)
                            DoNext(() => RapeCorruptAmily4Meeting());
        }

        public void RapeCorruptAmily1()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruption, CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) + 1);

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.WaitForMixture".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.WhyTiedUp".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.HelloAmily".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.LooksAtMixture".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.MadeJustForYou".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.BegsToBeReleased".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.LaughAtStunt".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.NeverStopsPleading".Localized());

            if (Player.CockTotal() > 0)
            {
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily1.CocksArePlural".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily1.CockIsSingular".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily1.AlreadyHard".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily1.AndYourWord".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1.IsAlreadyWet".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.CriesGivingUp".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.ForceBottleDown".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1.SpontaneousOrgasm".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1.WhichPartLick".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Cock".Localized(), RapeCorruptAmily1Male);

                AddButton(1, "Common.Button.Pussy".Localized(), RapeCorruptAmily1Female);
            }
            else
                if (Player.Gender == Gender.Male)
                    DoNext(() => RapeCorruptAmily1Male());
                else
                    DoNext(() => RapeCorruptAmily1Female());
        }

        public void RapeCorruptAmily1Male()
        {
            ClearOutput();

            AmilySprite();

            int x = Player.CockThatFits(61);

            if (x < 0)
                x = 0;

            Player.RemoveKeyItem("Potent Mixture");

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.WorkForIt".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockThatFits(61) == -1)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.SurprisedSheFits".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.SentencePeriod".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.GrabEarsFuckFace".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.LicksUndersideCock".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.BallsChurn".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.ThrobWord".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.MousySlutMassiveLoad".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Male.SighSatedLeave".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 5f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void RapeCorruptAmily1Female()
        {
            ClearOutput();

            AmilySprite();

            Player.RemoveKeyItem("Potent Mixture");

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.WorkForItAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.LiftsHeadLicks".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.JetHitsFace".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.BlowsOnClit".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.LaughAtSplutters".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.AsksForMore".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.MoreAfterMess".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.TearsUpDesperate".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.PleaseNeedIt".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.DoItRight".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.SealsAroundSnatch".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.FeelsSoGood".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.GulpsEverything".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.KeepFeedingCum".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily1Female.LetGoFinally".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 5f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void RapeCorruptAmily2Male()
        {
            ClearOutput();

            int x = Player.CockThatFits(61);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.ShoveIntoMaw".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockThatFits(61) < 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.PressesForwardSize".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.HitsBackThroat".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.GaspSurpriseSkill".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.SheTakesAll".Localized());

            if (Player.CockThatFits(61) < 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.IsPossibleWord".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.HaveToOffer".Localized());

            OutputText("Scenes.Amilyscene.AmilyPopsOutKidsInCamp.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.GoodGirlPraise".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.SmilesSlurpsDick".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.PumpIntoMouth".Localized());

            if (Player.CockThatFits(61) < 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.HandSettlesStrokes".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Amilyscene.CookAmilyASnack.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.SteadyRhythmExpert".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.GrabsBallsKneads".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.FingersIntoVagina".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.LeakingPreMoan".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.MoreFeminineFigure".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.ThoughtBallsChurn".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.ImagineAmilyEager".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.GrabsHairShovesDeep".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.UnloadIntoThroat".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.LoadSpillsMouth".Localized());

            OutputText("Scenes.Amilyscene.CookAmilyASnack.ParagraphBreakAgain".Localized());

            RapeCorruptAmily2Epilogue();
        }

        public void RapeCorruptAmily2Female()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.GrabEarsFacePussy".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.JuicesRocketFace".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.LicksSlurpsClit".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.GentlyPatGoodGirl".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.RepliesSlurpsClit".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.MoansOnceWhile".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.CantSeeWell".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.JuiceBathNeverEnds".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.SentencePeriod".Localized());

            if (Player.Wetness() < 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.FloodJuicesMaw".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.GrabsEarsPullsHard".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.CumsHarderThanEver".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.OrgasmLastsLonger".Localized());

            RapeCorruptAmily2Epilogue();
        }

        public void RapeCorruptAmily2Epilogue()
        {

            AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruption, CoC.Instance.Flags.Get(GameFlagIds.AmilyCorruption) + 1);

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Epilogue.FallsBackPanting".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Epilogue.BreastsFillHipsGrow".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Epilogue.WhisperBeReady".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RapeCorruptAmily3Male()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.StripWhileWatched".Localized());

            if (Player.CockArea(0) >= 61)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.BreastsCantWrap".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.BeginsMovingBreasts".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.FurryWord".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.BreastsAlongShaft".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.CompliesTitfuck".Localized());

            if (Player.Corruption < 60)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.LeaveSatisfiedPool".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.TimeToRewardAmily".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.GetUpCumbucket".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.JumpsUpNuzzles".Localized());

            if (Player.CockArea(0) > 50)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.DidntThinkPossible".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.FitsAllAtOnce".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.YourBallsWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.YourVaginaWord".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.BaseOfShaft".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.TicklesPreJet".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.OrderGetToWork".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.MovesWholeBody".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.BallsChurnHeavier".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.DemonizedAmilyImage".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.BellyFillsDistending".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.KeepsDrinkingGrowing".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Male.ParagraphBreakThird".Localized());

            RapeCorruptAmily3Epilogue();
        }

        public void RapeCorruptAmily3Epilogue()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.StripWatchedHungrily".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.SpurtFemcumBreasts".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.MoistureDripsNipples".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily2Female.ParagraphBreak".Localized());

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.CrushesClitBreasts".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.TeasesClitNipples".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.FinalPushCum".Localized());

            if (Player.Corruption < 60)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.LeaveSatisfiedJuices".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.RewardAmilyEfforts".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.SitStumpReward".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.SpringsUpNuzzles".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.NoseBumpsClit".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.LicksKissesLips".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.FloodsMawAlarmingly".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.KeepsSpillingDehydrated".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Male.ParagraphBreakAgain".Localized());

            RapeCorruptAmily3Epilogue();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.FallsBackLicksBelly".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.WickedSmileCorruptor".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.PleaseWaitMaster".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.TestForMore".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.KneelsAttention".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.AnythingYouDesire".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.ReturnWhenReady".Localized());
        }

        public void RapeCorruptAmily3Female()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.YouStripWhileAmily".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.YouShowHerYour".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.MoistureGathersAndDrips".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.ParagraphBreak".Localized());

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.AmilyThenCrushesYour".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.AmilyDecidesToTease".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.WithAFinalPush".Localized());

            if (Player.Corruption < 60)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.SatisfiedForTheMoment".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.ThatWasGoodBut".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.YouSitOnA".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.AmilySpringsUpAnd".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.HerNoseBumpsAgainst".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.SheLicksAndKisses".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.YouFloodHerMaw".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily3Female.YouKeepOnSpilling".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily3Epilogue.ParagraphBreakThird".Localized());

            RapeCorruptAmily3Epilogue();
        }

        public void RapeCorruptAmily4Meeting()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Meeting.NoPartsToPlay".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Meeting.EnterRuinedVillage".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Meeting.DropsDildoKneels".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Meeting.ScratchBehindEar".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Meeting.WhichPartFinish".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Cock".Localized(), RapeCorruptAmily4Male);

                AddButton(1, "Common.Button.Pussy".Localized(), RapeCorruptAmily4Female);
            }
            else
                if (Player.Gender == Gender.Female)
                    DoNext(() => RapeCorruptAmily4Female());
                else
                    DoNext(() => RapeCorruptAmily4Male());
        }

        public void RapeCorruptAmily4Male()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.StripArmorSlowly".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.AndBallsAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.OrderHerBegin".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.GivesTipLightKiss".Localized());

            if (Player.CockArea(0) > 50)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.SmirkPleasedWithSize".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.AllWayInsideMaw".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.YourBalls".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.YourPussy".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.CockBase".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.TeasingLickSmileBack".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.GripEarsForLeverage".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.NipplesBrushBalls".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.NoseBumpsCrotch".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.CockHarderOrgasmBuilding".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.HandKneadsBalls".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.ParagraphBreakThird".Localized());

            if (Player.Corruption < 80)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.WithdrawSpentAdoring".Localized());

                if (Player.CumQ() >= 750)
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.BellyDistendedPregnant".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.BellyFilledHugeLoad".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.TrySummonMoreStrength".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.ReturnToCamp".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.WithdrawNotSatisfied".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.DeclareHerMyBitch".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.DropsOnFoursGrovel".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.BegToBeCumbucket".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.HerWordsPleaseYou".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.CoilTailAroundHead".Localized());
            else
                if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.EngulfMawInSlime".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.PutFootOnHead".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.FallsSilentDeclaresMaster".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.LetGoProveFealty".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.AmilyGetsToWork".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.AndBallsAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.SentencePeriodBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.ObeysAtOnce".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.StopAndStandStill".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.AmilyClosesEyesBows".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Male.BlackSpunkCorrupts".Localized());

            RapeCorruptAmily4Epilogue();
        }

        public void RapeCorruptAmily4Female()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.StripArmorSlowly".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.PullsBackLicksBalls".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.ParagraphBreak".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.OrderHerBegin".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.NuzzlingKissingLips".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.WetsFaceWithJuices".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.EvenWetter".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.DigsInLikeLife".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.TonguePenetratesDeep".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.PleasureOverwhelming".Localized());

            if (Player.GetClitLength() >= 4)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.StrokesClitLikeCock".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.BiggestOrgasmYet".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.JuicesJetWithForce".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.DrinksLikeParchedMan".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.EveryDropConsumed".Localized());

            if (Player.Corruption < 80)
            {
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.WithdrawSpent".Localized());

                if (Player.Wetness() >= 5)
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.BellyDistendedFemcum".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.BellySlightlyDistended".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.ParagraphBreakThird".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.TooWeakToContinue".Localized());

                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.ReturnToCamp".Localized());

                DoNext(() => ReturnToCamp(1));

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

                return;
            }

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.WithdrawNotSatisfied".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.DeclareHerMyBitch".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.DropsOnAllFours".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.BegForCumBucket".Localized(
                Player.Mf("master", "mistress")
            ));

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.HerWordsPleaseYou".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.CoilTailAroundHead".Localized());
            else
                if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.EngulfMawInSlime".Localized());
                else
                    OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.PutFootOnHead".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.FallsSilentDeclaresMistress".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.ProveFealtyLickMe".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.AmilyGetsToWork".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.IgnoresBallsConcentrates".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.TellHerWorkHarder".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.StopAndStandStill".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.AmilyClosesEyesBows".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Female.BlackJuicesCorrupt".Localized());

            RapeCorruptAmily4Epilogue();
        }

        public void RapeCorruptAmily4Epilogue()
        {

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.CumAbsorbedDoublesOver".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.SmallHornsGrowWings".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.FeetWarpMousyClaws".Localized());
            else
                OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.FeetForcedTipToe".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.HandsGrowBlackClaws".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.ExaminesNewBody".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.MousettePartSuccubus".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.AmilyAcceptsOrders".Localized());

            OutputText("Scenes.Amilyscene.RapeCorruptAmily4Epilogue.CorruptedAmilySlave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 2);

            if (Player.Pregnancy.Type == PregnancyIds.Amily)
                Player.KnockUp(PregnancyIds.Mouse, Player.Pregnancy.Incubation, 100, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyCampCorruptionFreaked) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.AmilyCupSize, 5);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyNippleLength, .5f);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyHipRating, 12);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyAssSize, 12);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyVaginalWetness, 1);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, "sexy rags");
            }

            if (SceneInstance<MarbleScene>.Get().MarbleAtCamp())
                CoC.Instance.Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, 1);
            else
                CoC.Instance.Flags.Set(GameFlagIds.MarbleOrAmilyFirstForFreakout, 2);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void StalkingZeAmiliez()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez.StepIntoRuinedVillage".Localized());

            if (Player.Intelligence >= 50 && Player.Speed >= 65)
            {
                AmilySprite(true);

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez.SurveySpotTracks".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez.WaitThenHearApproach".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez.AmbushSwipeBlowpipe".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez.YellsGetOffMe".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez.StripAndTieHer".Localized());

                ChooseYourAmilyRape();
            }
            else
                if (Player.Intelligence >= 50)
                {
                    AmilySprite();

                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez.SurveySpotTracksAgain".Localized());

                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez.WaitHearApproachAgain".Localized());

                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez.AmbushSwipeBlowpipeAgain".Localized());
                    // 原版 AmilyScene.hx:7850/7859：startCombat(new Amily(), true)；
                    // 旧移植把 `(), true` 混进 id，生成不存在的 "coc:amily(), true"（创建即抛异常）
                    CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "amily")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
                }
                else
                    if (Player.Speed >= 65)
                    {
                        AmilySprite();

                        OutputText("Scenes.Amilyscene.StalkingZeAmiliez.SearchVillageNoLuck".Localized());

                        OutputText("Scenes.Amilyscene.StalkingZeAmiliez.DodgeDartSpotAmily".Localized());

                        OutputText("Scenes.Amilyscene.StalkingZeAmiliez.PanickedDrawsKnife".Localized());
                        // 原版 AmilyScene.hx:7850/7859：startCombat(new Amily(), true)；
                    // 旧移植把 `(), true` 混进 id，生成不存在的 "coc:amily(), true"（创建即抛异常）
                    CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "amily")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
                    }
                    else
                    {
                        OutputText("Scenes.Amilyscene.StalkingZeAmiliez.NoTraceFrustrated".Localized());

                        DoNext(() => ReturnToCamp(2));
                    }
        }

        public void StalkingZeAmiliez2()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.ConsiderVillageVisit".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                AmilySprite();

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.StepIntoRuinedVillage".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.LocateHerChanged".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.HipsButtGrownFuller".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.CockGetsHard".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.AndYour".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.VaginaGrowsWet".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.SentencePeriodParagraphBreak".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.CallsOutShakesBottle".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.SqueaksInSurprise".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.AskWhatsWrong".Localized());

                if (Player.Corruption + Player.CorruptionTolerance() < 45)
                {
                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.RefusesThenDarts".Localized());

                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.LaughPutBottleAway".Localized());

                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.NotCorruptEnough".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return;
                }

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.BeginStrippingOff".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.ErectCock".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.AndWord".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.DrippingVagina".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.OfferMoreToDrink".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.RefusesButApproaches".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.KneelsUncorkBottle".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.ShoveBottleGulps".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.LaughStepBackKnife".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.RewardHerOpenness".Localized());

                OutputText("Scenes.Amilyscene.StalkingZeAmiliez2.EagerOpensMouth".Localized());

                Player.RemoveKeyItem("Potent Mixture");

                ChooseYourAmilyRape();
            }
        }

        public void StalkingZeAmiliez3()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.ConsiderVillageVisit".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            AmilySprite();

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.StepIntoRuinedVillage".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.AmilyFindsYouChanged".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.DashesAndKneels".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.BegsNuzzlingCrotch".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.DrivesCockErection".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.AndWord".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.MoisteningVagina".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.NoseTwitchesLittle".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.AskAboutMedicine".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.ThrowsBottleAway".Localized());

            Player.RemoveKeyItem("Potent Mixture");

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.RealizeSheAddicted".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.OrderHerGrovel".Localized());

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.PlantsHeadGround".Localized(
                Player.Mf("Master", "Mistress")
            ));

            OutputText("Scenes.Amilyscene.StalkingZeAmiliez3.LaughAllowDrink".Localized());

            ChooseYourAmilyRape();
        }

        public void AmilyTaintWarning()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyTaintWarning.AmilyApproachesConcerned".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCampCorruptionFreaked, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void FarewellNote()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.FarewellNote.AwakenAmilyGone".Localized());

            OutputText("Scenes.Amilyscene.FarewellNote.TriedToWarnYou".Localized());

            OutputText("Scenes.Amilyscene.FarewellNote.AmilyMovedOut".Localized());

            OutputText("Scenes.Amilyscene.FarewellNote.StillCanHelpHer".Localized());

            if (Player.Intelligence >= 40)
                OutputText("Scenes.Amilyscene.FarewellNote.PlanNeedsLustDraft".Localized());
            else
                OutputText("Scenes.Amilyscene.FarewellNote.RecallInterestingPotions".Localized());

            OutputText("Scenes.Amilyscene.FarewellNote.MaybeSheHasPoint".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            if (Player.Pregnancy.Type == PregnancyIds.Amily)
                Player.KnockUp(PregnancyIds.Mouse, Player.Pregnancy.Incubation, 100, 1);
        }

        public void AmilyReturns()
        {

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyReturns.AwakenLimbsWrapped".Localized());

            OutputText("Scenes.Amilyscene.AmilyReturns.GratefulYetUnsure".Localized());

            OutputText("Scenes.Amilyscene.AmilyReturns.SqueezesThenBreakfast".Localized());

            OutputText("Scenes.Amilyscene.AmilyReturns.AmilyMovedBackIn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCampCorruptionFreaked, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);
        }

        public void AmilyNoFur()
        {

            AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferedDefurry, 1);

            OutputText("Scenes.Amilyscene.AmilyNoFur.RefuseChangePosition".Localized());

            OutputText("Scenes.Amilyscene.AmilyNoFur.LongPauseDigests".Localized());

            OutputText("Scenes.Amilyscene.AmilyNoFur.LongSilenceThenSighs".Localized());

            OutputText("Scenes.Amilyscene.AmilyNoFur.WhirlsAndWalksAway".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public bool AmilyCanHaveTFNow()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0

                && CoC.Instance.Flags.Get(GameFlagIds.AmilyOfferedDefurry) == 1

                && Player.HasItem(ItemRegistry.Create(ConsumableIds.GoldenSeed))

                && (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack)))

                && (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)) || (AmilyCorrupt() && Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk))));
        }

        public void AmilyDefurrify()
        {

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoldenSeed));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBlack));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack));

            if (AmilyCorrupt())
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));
            }
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferedDefurry, 2);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNotFurry, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferAccepted, 1);

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyDefurrify.SheAppearsQuickly".Localized());

            OutputText("Scenes.Amilyscene.AmilyDefurrify.ProduceThreeItems".Localized());

            OutputText("Scenes.Amilyscene.AmilyDefurrify.NipsEggOpen".Localized());

            OutputText("Scenes.Amilyscene.AmilyDefurrify.FallsBackGasping".Localized());

            OutputText("Scenes.Amilyscene.AmilyDefurrify.CryDrawsAttention".Localized());

            OutputText("Scenes.Amilyscene.AmilyDefurrify.ProcessComesToClose".Localized());

            OutputText("Scenes.Amilyscene.AmilyDefurrify.StalksOffHumming".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilySufferNotTheFurryToLive()
        {

            AmilySprite();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNotFurry, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyIsBatman, 1);

            OutputText("Scenes.Amilyscene.AmilySufferNotTheFurryToLive.SpitefulLaughHumanized".Localized());
        }

        public void AmilyDefurryOfferAtCamp()
        {

            AmilySprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyOfferedDefurry) <= 0)
                CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferedDefurry, 1);

            if (!AmilyCanHaveTFNow())
            {
                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.NeedMagicPreparations".Localized());

                DoNext(() => AmilyFollowerEncounter());

                return;
            }

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoldenSeed));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBlack));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack));

            if (AmilyCorrupt())
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));
                else
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));
            }
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

            if (AmilyCorrupt())
            {
                var master = Player.Mf("master", "mistress");

                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.BeckonsAndExplains".Localized(
                    master,

                    master
                ));

                switch ((int)Player.Gender)
                {
                    case 1:
                    {
                    OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.HardeningCocks".Localized());
                    }

                    break;

                    case 2:
                    {
                    OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.MoisteningClit".Localized());
                    }

                    break;
                }

                var __sw1_1 = (int)Player.Gender;

                if (true )
                    OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.HardeningNipples".Localized());

                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.RunsSitsInLap".Localized());

                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.GiggleCommandOpenMouth".Localized(
                    master
                ));
            }
            else
            {
                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.SquaresUpExpectantly".Localized());

                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.FallsBackGasping".Localized());

                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.CryDrawsAttention".Localized());

                OutputText("Scenes.Amilyscene.AmilyDefurryOfferAtCamp.ProcessComesToClose".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNotFurry, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferedDefurry, 2);

            AmilySprite();

            DoNext(() => AmilyFollowerEncounter());
        }

        public void RefuzzAmily()
        {

            if (!Player.Inventory.HasItem(ConsumableIds.MouseCocoa, 2))
            {
                OutputText("Scenes.Amilyscene.RefuzzAmily.NeedMouseCocoa".Localized());

                DoNext(() => AmilyFollowerEncounter());

                return;
            }

            Player.ConsumeItem(ConsumableIds.MouseCocoa, 2);

            if (AmilyCorrupt())
            {
                var master = Player.Mf("master", "mistress");

                OutputText("Scenes.Amilyscene.RefuzzAmily.BeckonToRefuzz".Localized());

                OutputText("Scenes.Amilyscene.RefuzzAmily.SucksMouseCocoa".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.RefuzzAmily.SquaresUpExpectantly".Localized());

                OutputText("Scenes.Amilyscene.RefuzzAmily.RodentSnoutWhiskersGrow".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyNotFurry, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOfferedDefurry, 1);

            AmilySprite();

            DoNext(() => AmilyFollowerEncounter());
        }

        public void DateNightFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.DateNightFirstTime.AskAmilyAboutTrip".Localized());

            if (Player.IsPregnant())
            {
                OutputText("Scenes.Amilyscene.DateNightFirstTime.NotWhilePregnant".Localized());

                DoNext(() => AmilyFollowerEncounter());

                return;
            }

            OutputText("Scenes.Amilyscene.DateNightFirstTime.StuttersSurprised".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.LongerThanUsual".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.UrtaAtGates".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.AmilyFriendlySmile".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.UrtaCrestfallenMumble".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.EnterCityStreets".Localized());

            if (View.Time.Hours >= 18)
                OutputText("Scenes.Amilyscene.DateNightFirstTime.BustlingLateHour".Localized());
            else
                if (View.Time.Hours < 6)
                    OutputText("Scenes.Amilyscene.DateNightFirstTime.BustlingEarlyTime".Localized());
                else
                    if (View.Time.Hours < 12)
                        OutputText("Scenes.Amilyscene.DateNightFirstTime.BusyMorningHours".Localized());
                    else
                        OutputText("Scenes.Amilyscene.DateNightFirstTime.MorphsBusyWithTasks".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.AmilyEnthralledSights".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.TavernWetBitch".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.AmilySharesExcitement".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.UrtaSpotsYou".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.DrunkAmilyInterrupts".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.TheFoxGirlGrabs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EncounteredValaAtBar) == 0)
                OutputText("Scenes.Amilyscene.DateNightFirstTime.APassingWaitress".Localized());
            else
                OutputText("Scenes.Amilyscene.DateNightFirstTime.ARatherAlarmedVala".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.AmilySlursPawingOff".Localized());

            OutputText("Scenes.Amilyscene.DateNightFirstTime.WellThisCouldBe".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), LiqueurUpTheWaifus);

            AddButton(1, "Common.Button.Leave".Localized(), AmilyXUrtaRunAWAY);
        }

        public void AmilyXUrtaRunAWAY()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyXUrtaRunAWAY.YouWatchUrtaTake".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 3);

            DoNext(() => ReturnToCamp(4));
        }

        public void LiqueurUpTheWaifus()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.YouEncourageUrtaTo".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.BeforeLongUrtaS".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.Thunk".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.TheTableIsNearly".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.WithADrunkenGrin".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.ForAMomentYou".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.AlreadyFeelingAStrong".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.AlreadyFeelingALittle".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.Text".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.UnabashedlyArousedByYour".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.UrtaPurrsGivingA".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.AmilyAdds".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.GrinningYouSaunterOver".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.UrtaAndAmilyFlop".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.ThroughTheMouseS".Localized());
            else
                OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.OverTheMouseS".Localized());

            OutputText("Scenes.Amilyscene.LiqueurUpTheWaifus.AndAmilyWrappingHer".Localized());

            if (Player.HasCock())
                AddButton(0, "Common.Button.UseCock".Localized(), ThreesomeAmilUrtaCAWKS);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.UseVagina".Localized(), UrtaXAmilyCuntPussyVagSQUICK);

            AddButton(4, "Scenes.CorruptedGlade.NeverMind".Localized(), () => ReturnToCamp(1));
        }

        public void ThreesomeAmilUrtaCAWKS()
        {
            ClearOutput();

            int x = Player.CockThatFits(60);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.YouTakeHoldOf".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.YouGentlyPushAmily".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.YouLineUpYour".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.YouGiveAmilyA".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.GrabbingHoldOfUrta".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.WellYouCanT".Localized());

            if (Player.CockArea(x) > 60)
                OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.FillingHerUntilShe".Localized());
            else
                OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.BuryingYourselfUpTo".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.SheLetsOutA".Localized(
                AmilyTits()
            ));

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.UrtaIsCloseTo".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.AsHerCockOrgasms".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.Text".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.NowThoroughlyFilledWith".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.AmilySighsExhausted".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.UrtaPurrsGivingHer".Localized());

            OutputText("Scenes.Amilyscene.ThreesomeAmilUrtaCAWKS.YouCouldnTAgree".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => UrtaXAmilyAfterMurrrath());
        }

        public void UrtaXAmilyCuntPussyVagSQUICK()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouLeapIntoBed".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouBraceYourselfFor".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouWiggleUnderneathAmily".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.BeforeYouCanTruly".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouAreForcedTo".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouGroanAsUrta".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.SheGrabsYourAnd".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouGritYourTeeth".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.NowFilledWithCum".Localized());

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.ShePurrsGivingYour".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            OutputText("Scenes.Amilyscene.UrtaXAmilyCuntPussyVagSQUICK.YouCouldnTAgree".Localized());

            DoNext(() => UrtaXAmilyAfterMurrrath());
        }

        public void UrtaXAmilyAfterMurrrath()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.UrtaXAmilyAfterMurrrath.YouWakeUpLater".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaCumNoCumDays, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 3 + Random.Shared.Next(4));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 0)
            {
                OutputText("Scenes.Amilyscene.UrtaXAmilyAfterMurrrath.UrtaUnlockedInAmily".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyVisitingUrta, 4);
            }

            DoNext(() => ReturnToCamp(4));
        }

        public void PureAmilyPutsItInYourRectumDamnNearKilledEm()
        {
            ClearOutput();

            AmilySprite(true);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.YouPauseAndFlash".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.BeforeSheCanMisunderstand".Localized());

            if (Player.Tallness >= 40)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Down".Localized());
            else
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Over".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.AndWhisperInHer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesButtfuckedPc) == 0)
            {
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheSaysLookingAt".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.YouNodToHer".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheSaysTurningTo".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.YouGiveHerA".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheSaysSmilingA".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheTurnsAroundRummaging".Localized());
            }

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.WorkingDelicatelySheCarefully".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.MovingYourTailOut".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Text".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Amily".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesButtfuckedPc) == 0)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.LooksABitPuzzled".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.TakesHerPlaceBehind".Localized());

            if (Player.AnalCapacity() < 30)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.BracingYourselfABit".Localized());
            else
                if (Player.AnalCapacity() <= 60)
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.YouRelaxYourselfLetting".Localized());
                else
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.AmilyLetsOutA".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.TheOintmentDoesIts".Localized());

            if (Player.AnalCapacity() > 60)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.ThoughYouAreSure".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SlowlySheBeginsTo".Localized());

            Player.ButtChange(CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) * CoC.Instance.Flags.Get(GameFlagIds.AmilyWangGirth), true, true, false);

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Text.Two".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesButtfuckedPc) == 0)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheSaysWithA".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.YouBeginToPush".Localized(
                AmilyHips()
            ));

            if (Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.YouCanFeelHer".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheBrushesAlongOne".Localized());
            }

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Text.Three".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.ItIsnTLong".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SexualFluids".Localized());

                if (Player.Wetness() <= 1)
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Drip".Localized());
                else
                    if (Player.Wetness() <= 3)
                        OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Dribble".Localized());
                    else
                        OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Gush".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.DownYourThighs".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.AndP".Localized());
            }

            if (Player.HasCock())
            {
                if (!Player.HasVagina())
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.P".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.RecumDribblesFromYour".Localized());
            }

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.FormingASmallPuddle".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.WithATremendousShudder".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.TwitchesWithPleasureSoaking".Localized(
                    Player.SMultiCockDesc()
                ));

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.AFewStreams".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.ASmallDeluge".Localized());
                    else
                        OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.ASmallLake".Localized());

                OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.OfCum".Localized());
            }

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.Text.Four".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.WithAShudderingSigh".Localized());

            OutputText("Scenes.Amilyscene.PureAmilyPutsItInYourRectumDamnNearKilledEm.SheSaysWithA.Two".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyTimesButtfuckedPc, CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesButtfuckedPc) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckPureAmilysHeiny()
        {
            ClearOutput();

            AmilySprite(true);

            int x = Player.CockThatFits(50);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouPauseAndFlash".Localized(
                AmilyButt()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.HerPantsTentFrom".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.Text".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedAmilybutt) == 0)
            {
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheLooks".Localized());

                if (Player.Tallness >= 60)
                    OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.Up".Localized());
                else
                    if (Player.Tallness <= 40)
                        OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.Down".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.AtYouWithA".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheSaysHerBrow".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheSaysHerLips".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouSmileBroadlyNodding".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheSaysPuttingUp".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouReignYourselfIn".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheSaysRummagingAround".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouRecoilALittle".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheRollsHerEyes".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheSmirksBackAt".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouNodAffirmingThat".Localized());

                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheGentlyBreaksAway".Localized(
                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.AsSheTouchesThe".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheWipesHerHand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedAmilybutt) == 0)
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.Text.Two".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.TurningAroundSheLowers".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YourHandCascadesOver".Localized(
                AmilyHips(),

                AmilyButt()
            ));

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.SheShuddersAndLowers".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedAmilybutt) == 0)
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.ShePantsNoticeablyStruggling".Localized());
            else
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.Text.Three".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.OnceYouAreSufficiently".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YourHandsSlideAlong".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.AmilySaysBreathlesslySmiling".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.HerHandsClaspAround".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouCloseYourEyes".Localized());

            if (Player.CumQ() <= 250)
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YourTwitchingAndThrobbing".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (Player.CumQ() <= 500)
                    OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.TheSwellOfYour".Localized());
                else
                    OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.TheVolumeOfYour".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.BeingStimulatedInSuch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.HerOwnMemberTwitch".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.ANoticeableTrailOf".Localized());

            OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.YouVeryCarefullyPull".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Amilyscene.FuckPureAmilysHeiny.AndGentlyStrokingAlong".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SentencePeriod".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedAmilybutt, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedAmilybutt) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyIncest()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.AmilyIncest.WhenYouGoLooking".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilyIncest.Furred".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyIncest.Haired".Localized());

            OutputText("Scenes.Amilyscene.AmilyIncest.HeavilyCorruptedMiceHave".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilyIncest.Fur".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyIncest.Skin".Localized());

            OutputText("Scenes.Amilyscene.AmilyIncest.YouSupposeThatWith".Localized());

            OutputText("Scenes.Amilyscene.AmilyIncest.TheHermIsCuddling".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilyIncest.ThroughHerMotherS".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyIncest.OverHerMotherS".Localized());

            OutputText("Scenes.Amilyscene.AmilyIncest.AsTheLatterLectures".Localized());

            OutputText("Scenes.Amilyscene.AmilyIncest.NoneOfThemAre".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(61))
                {
                    AddButton(0, "Common.Button.FuckCunts".Localized(), () => FuckIncestCunts(false));

                    AddButton(1, "Common.Button.FuckEmAll".Localized(), () => FuckIncestCunts(true));
                }
                else
                    OutputText("Scenes.Amilyscene.AmilyIncest.SadlyYouReToo".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.AmilyIncest.IfOnlyYouHad".Localized());

            AddButton(14, "Common.Button.Leave".Localized(), PlayerMenu);
        }

        public void FuckIncestCunts(bool all)
        {
            ClearOutput();

            int x = Player.CockThatFits(61);

            if (x < 0)
                x = 0;

            var y = x + 1;

            OutputText("Scenes.Amilyscene.FuckIncestCunts.YouStepUpAnd".Localized());

            if (!all)
                OutputText("Scenes.Amilyscene.FuckIncestCunts.TellAmilyToSend".Localized());
            else
                OutputText("Scenes.Amilyscene.FuckIncestCunts.TellAmilyToGet".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.AmilyTakesTheGirl".Localized());

            if (Player.Tallness >= 48)
                OutputText("Scenes.Amilyscene.FuckIncestCunts.Up".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.AtYouAndSays".Localized(
                Player.Mf("Dad", "other Mom")
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.YouShrugOutOf".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.FuckIncestCunts.Muzzles".Localized());
            else
                OutputText("Scenes.Amilyscene.FuckIncestCunts.Mouths".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.WateringWithTonguesHanging".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.AmilyWhispersEncouraginglyInto".Localized(
                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.AsIfYouCould".Localized(
                y,

                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.YouTakeHerDrippy".Localized(
                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.TheKnockedUpLittle".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.YouHumpHerFaster".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.WhileYouReWatching".Localized(
                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.ThePregnantSlutBeneath".Localized(
                y,

                Player.Mf("Dad", "Mom")
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.SighingYouCrookYour".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.IsReadyNowGo".Localized(
                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.SheSaysStartingTo".Localized(
                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.YourCorruptedMouseSlave".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.AmilyBreaksOffThe".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.TheMousetteScreamsYour".Localized(
                y
            ));

            OutputText("Scenes.Amilyscene.FuckIncestCunts.SweatyArmsClingAnd".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.YouGrabTheClosest".Localized());

            OutputText("Scenes.Amilyscene.FuckIncestCunts.AmilyGaspsAndRolls".Localized());

            if (all)
            {
                OutputText("Scenes.Amilyscene.FuckIncestCunts.YouRealizeYouHaven".Localized());

                if (AmilyFurry())
                    OutputText("Scenes.Amilyscene.FuckIncestCunts.Muzzle".Localized());
                else
                    OutputText("Scenes.Amilyscene.FuckIncestCunts.Face".Localized());

                OutputText("Scenes.Amilyscene.FuckIncestCunts.FromYourViewAs".Localized());

                OutputText("Scenes.Amilyscene.FuckIncestCunts.SoonYourErectionIs".Localized());

                OutputText("Scenes.Amilyscene.FuckIncestCunts.YourJumpHimBefore".Localized(
                    Player.Mf("Dad", "Mom"),

                    y
                ));

                OutputText("Scenes.Amilyscene.FuckIncestCunts.HeSlowlySettlesInto".Localized());

                OutputText("Scenes.Amilyscene.FuckIncestCunts.HeMoansYouLet".Localized(
                    Player.Mf("Dad", "Mom"),

                    y
                ));

                OutputText("Scenes.Amilyscene.FuckIncestCunts.QuiveringTightlyAroundYour".Localized(
                    y
                ));

                OutputText("Scenes.Amilyscene.FuckIncestCunts.ItIsnTUntil".Localized());

                if (Player.CumQ() < 200)
                    OutputText("Scenes.Amilyscene.FuckIncestCunts.Drizzling".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Amilyscene.FuckIncestCunts.Raining".Localized());
                    else
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Amilyscene.FuckIncestCunts.Pouring".Localized());
                        else
                            OutputText("Scenes.Amilyscene.FuckIncestCunts.Spraying".Localized());

                OutputText("Scenes.Amilyscene.FuckIncestCunts.DownToTheGround".Localized());
            }
            else
                OutputText("Scenes.Amilyscene.FuckIncestCunts.ItIsnTUntil.Two".Localized());

            AmilyPreggoChance();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyEggStuff()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyEggStuff.AsYouApproachThe".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.AreYouAlrightYou".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.ASmirkCrossesHer".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Amilyscene.AmilyEggStuff.YourStiffeningStartingTo".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.WhilstYourBodyInstinctively".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.AmilySRightYou".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.SheRepliesSlowlyAs".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.YourAbdomenTwitchesAt".Localized());

            if (Player.DogScore() >= 4)
                OutputText("Scenes.Amilyscene.AmilyEggStuff.WhichYouKnowFor".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.YouGentlyTakeHer".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.YouKnowThatThis".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.AmilyEggStuff.AndYouNoticeA".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.LooksLikeYouAren".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.BeforeSheCanMove".Localized());

            OutputText("Scenes.Amilyscene.AmilyEggStuff.Text".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOvipositionUnlocked, 1);

            AmilyMenu(false);
        }

        public void LayEggsInAmily()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.LayEggsInAmily.FeelingYour".Localized());

            if (Player.Eggs() < 20)
                OutputText("Scenes.Amilyscene.LayEggsInAmily.Engorged".Localized());
            else
                if (Player.Eggs() < 40)
                    OutputText("Scenes.Amilyscene.LayEggsInAmily.Heaving".Localized());
                else
                    OutputText("Scenes.Amilyscene.LayEggsInAmily.UnbelievablySwollen".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmily.AbdomenStirYourMind".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmily.YouFeelAHand".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Amilyscene.LayEggsInAmily.BlankGroin".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmily.TeasingYouIntoUnconsciously".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.LayEggsInAmily.Text".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmily.YourTryToClear".Localized());

            DoNext(() => LayEggsInAmilysCorruptedHole());
        }

        public void LayEggsInAmilysCorruptedHole()
        {
            ClearOutput();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.StillGroaningAsAmily".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YouSetToWork".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.FeaturelessMound".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YourBulgingOvipositorFully".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AmilySVoiceIs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AndHerErectShaft".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.TellingYouEverythingYou".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.WithHerReddenedLips".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AndStiffCock".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.ExposedAmilyPositionsHerself".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YouGripHerBy".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.Yours".Localized());
            else
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YourGroin".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.WithTheRollingOf".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YourOvipositorIsPractically".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.UnableToHoldYourself".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerShaftOozingThick".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SentencePeriodAgain".Localized());
            }
            else
                if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerSoakedPussyKeeps".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YouCryOutIn".Localized());
                    else
                    {
                        OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.SheMoansAsYour".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerShaftCoveringYour".Localized());

                        OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AsThrustsBecomeFaster".Localized());
                    }

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AmilyManagesToSmirk".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AmilyJoltsForwardsFrom".Localized());
            else
                if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AmilyJumpsForwardsSuddenly".Localized());
                else
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YouMoanAsAmily".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AlmostAsIfMoving".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YouFeelHerPassage".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerThrustsFalterSlightly".Localized());
                else
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerBodyHammersInto".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AnotherMoanBurstsOut".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.SQuiver".Localized());
            else
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.Quivers".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.AroundYouFeelingThe".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YourHandsEventuallyFind".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.BeforeLongTheFeeling".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerPussySpasmsAround".Localized());
            else
                if (Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.SheFloodsYourWith".Localized());
                else
                    OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.HerHipsSlamInto".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.YourEggShaftTwitches".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.Another".Localized());

            OutputText("Scenes.Amilyscene.LayEggsInAmilysCorruptedHole.PenisARushOf".Localized());

            Player.Orgasm("Ovi");

            if (Player.FertilizedEggs() > 0)
            {
                if (Player.CanOvipositSpider())
                    ButtPregnancy.KnockUp(PregnancyIds.DriderEggs, PregnancyStore.IncubationDrider - 304);
                else
                    if (Player.CanOvipositBee())
                        ButtPregnancy.KnockUp(PregnancyIds.BeeEggs, PregnancyStore.IncubationBee + 48);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyOvipositedCount, Player.Eggs());
            }

            Player.DumpEggs();

            DoNext(() => LayEggsInAmilysButtPt2());
        }

        public void LayEggsInAmilysButtPt2()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.LayEggsInAmilysButtPt2.YouWakeUpAlmost".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyLaysEggsLikeABitch()
        {

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.WhilstWanderingAroundYour".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.TeasesHerClit".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.SqueezesHerShaft".Localized());

            OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.WithOneHandThe".Localized());

            OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.TheSightSurprisesYou".Localized());

            OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.AnotherEggFlowsFrom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.PussyVisiblyQuivering".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.MemberOozingAThick".Localized());

            OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.AsHerBodyShudders".Localized());

            OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.YouDecideToLeave".Localized());

            if (Player.CanOviposit())
                OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.ThoughTheSightHas".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyLaysEggsLikeABitch.ThoughADistinctHeat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 10)));

            CoC.Instance.Flags.Set(GameFlagIds.AmilyOvipositedCount, 0);
        }

        public void AmilySwimFuckIntro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesSwimfucked) == 0)
            {
                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.PullingOutTheSexy".Localized());

                if (Player.Libido >= 60)
                    OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.EagerlyAnticipatingTheOpportunity".Localized());
                else
                    if (Player.Corruption < 50)
                        OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.HappyToShareThe".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.SecretlyHopingSheLl".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.AmilySEyesOpen".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.YouPushThePile".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.SheSqueaksInExcitement".Localized(
                    AmilyHips()
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                    OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.DDonTLaugh".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.Text".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.AmilyStridesAroundThe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) > 0)
                    OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.Skin".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.Fur".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.IsAlmostTotallyExposed".Localized(
                    AmilyHips(),

                    AmilyButt()
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.TheTopPortionOf".Localized(
                        AmilyCock()
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                        OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.ItSnapsBackOnto".Localized(
                            AmilyBalls()
                        ));
                }

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.HigherUpSilkyOnyx".Localized(
                    AmilyTits(),

                    AmilyNipples()
                ));

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.YourLoverAsksDemurely".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.YouAnswerAndRemembering".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.AmilySeductivelySashaysUp".Localized());

                Player.ConsumeItem(ItemRegistry.Create(ArmorIds.S_SWMWR));

                CoC.Instance.Flags.Set(GameFlagIds.AmilyOwnsBikini, 1);

                CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, ItemRegistry.Create(ArmorIds.S_SWMWR).Name);
            }
            else
            {
                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.YouAskAmilyIf".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.AmilyScurriesOffTowards".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                {
                    OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.TheOnlyThingLeft".Localized(
                        AmilyCock()
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyHasBallsAndSize) > 0)
                        OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.HerBallsAreSnugly".Localized());
                }

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.TheHardPointsOf".Localized(
                    AmilyNipples()
                ));

                OutputText("Scenes.Amilyscene.AmilySwimFuckIntro.AmilySauntersUpAnd".Localized(
                    AmilyButt()
                ));
            }

            CoC.Instance.Flags.Set(GameFlagIds.AmilyTimesSwimfucked, CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesSwimfucked) + 1);

            DoNext(() => AmilySwimFuckPartII());
        }

        public void AmilySwimFuckPartII()
        {
            ClearOutput();

            int x = Player.CockThatFits(61);

            if (x < 0)
                x = Player.SmallestCockIndex();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.TheStreamIsnT".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.Package".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.PuffyLips".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.Text".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.YouSighFromThe".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AmilyAnswersAndTwists".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyTimesSwimfucked) > 1)
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AndWonderingWhyYou".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AmilyBoundsInAfter".Localized(
                AmilyTits()
            ));

            Player.HasGottenWashed();

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.OnceYouTireOf".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.YouSmirkAndDeclare".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AmilyShoutsInMock".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.ThePetiteMouseGirl".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                AmilyButt()
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.Text.Two".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.Base".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SentencePeriodThird".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.YouKneadHerSupple".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.HerSilkyCoat".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.HerSmoothSkin".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.WithAllTenFingers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) == 0)
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.EngorgedBudThatIs".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.SlipperyMember".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.SlidingBackAndForth".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.HerWhiskersTickleYour".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.S".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.HeComesUpFor".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.BurningWithDesireYou".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.TheTwoOfYou".Localized(
                AmilyNipples(),

                AmilyTits(),

                AmilyButt(),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AmilyGaspsSheLooks".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.YourUntendedErection".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.SDo".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.Does".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AFineJobOf".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.ItsLoad".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.TheirLoads".Localized());

                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.OverHerMiddle".Localized());
            }

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.TheHeatFlowingInside".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.AndSheClimaxesAll".Localized(
                    AmilyTits()
                ));

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.HerChannelSoonDrips".Localized());

            if (Player.CumQ() > 250)
            {
                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.ButItDoesnT".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.SquirtingMoreAndMore".Localized());
                else
                    if (Player.CumQ() < 750)
                        OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.SpurtingOutABackflood".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.GoutsOfCumWashing".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.WavesOfJismWashing".Localized(
                                    Player.CockDescript(x)
                                ));
                            else
                                OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.GeysersOfCumBursts".Localized());
            }

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.Text.Four".Localized());

            OutputText("Scenes.Amilyscene.AmilySwimFuckPartII.OnceFinishedGentlySet".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            AmilyPreggoChance();

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyXIzmaSuperPregOptions()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyXIzmaSuperPregOptions.YouSmileWinninglyAt".Localized());

            OutputText("Scenes.Amilyscene.AmilyXIzmaSuperPregOptions.TheMouseAsksNervously".Localized());

            OutputText("Scenes.Amilyscene.AmilyXIzmaSuperPregOptions.YouNodSmilingAgain".Localized());

            OutputText("Scenes.Amilyscene.AmilyXIzmaSuperPregOptions.AmilyTrillsIdlyTracing".Localized());

            OutputText("Scenes.Amilyscene.AmilyXIzmaSuperPregOptions.DoYouWantHer".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyAllowsFertility, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Normal".Localized(), FuckTheMouseBitch);

            AddButton(1, "Common.Button.TryPotion".Localized(), DrinkThePotion);
        }

        public void DrinkThePotion()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AmilyXIzmaPotion3some, CoC.Instance.Flags.Get(GameFlagIds.AmilyXIzmaPotion3some) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyXIzmaPotion3some) == 1)
            {
                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilySmilesUpAt".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.SmilingJustAsWidely".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.TheMouseGirlPulls".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.YouTapYoursAgainst".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyTapsHersAgainst".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.YouSmileAsYou".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.SheGrumpsInDespair".Localized(
                    AmilyButt()
                ));

                OutputText("Scenes.Amilyscene.DrinkThePotion.YouVeOnlyFinished".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.SuddenlyStrongArmsWrap".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.IzmaPlucksTheBottle".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyBlurtsTheAngry".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.TheSharkGirlCries".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyRetorts".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.TheTwoBeginTo".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyShrieksSteppingAround".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.IzmaSnarlsAsShe".Localized());

                if (Player.BiggestCockArea() < 10)
                    OutputText("Scenes.Amilyscene.DrinkThePotion.OvertopOfThePetite".Localized());
                else
                    OutputText("Scenes.Amilyscene.DrinkThePotion.OnYour".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.PossessivelyHerLustyEyes".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.GroaningAndFeelingMore".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilySeemsAboutTo".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.DrinkThePotion.YouAskAmilyIf".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyLaughsWithA".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.WhenYouNodHer".Localized());

                if (Player.IsPregnant())
                    OutputText("Scenes.Amilyscene.DrinkThePotion.AtLeastICan".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.Text.Two".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.YouSmirkAndRaise".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyNods".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.ThePrettyTigersharkGirl".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                    OutputText("Scenes.Amilyscene.DrinkThePotion.BarelyConcealingTheHalf".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.IzmaSaysWhatThe".Localized());

                if (Player.IsPregnant())
                    OutputText("Scenes.Amilyscene.DrinkThePotion.SupposeICanDo".Localized());
                else
                    OutputText("Scenes.Amilyscene.DrinkThePotion.SupposeWeCanHave".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.Text.Two".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.ThePotionsArePassed".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.AmilyNuzzlesUnderYour".Localized(
                    Player.Mf("strong, potent male...", "beautiful, potent breeder...")
                ));

                if (!Player.IsPregnant())
                    OutputText("Scenes.Amilyscene.DrinkThePotion.GiveMeLotsOf".Localized());
                else
                    OutputText("Scenes.Amilyscene.DrinkThePotion.PutEvenMoreBabies".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.RightNoNeedTo".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.IzmaMatchesTheMouse".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.ThePleasantFogOf".Localized());

                OutputText("Scenes.Amilyscene.DrinkThePotion.TheGirlsShareOne".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 100f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IzmaAmilyDrugThreeWaySex);
        }

        public void IzmaAmilyDrugThreeWaySex()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.TheAmorousEmbraceS".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.ItS".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.TheyVe".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.EverBeenTheContinuing".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.OneThatHasYour".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.OneThatHasYou".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.DropletsOfPreCum".Localized());

            if (!Player.IsPregnant() && !Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.WithTheirHungryWombs".Localized());
            else
                if (Player.IsPregnant() && Player.IsPregnant())
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.WithTheirRipeWombs".Localized());
                else
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.WithAtLeastOne".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SentencePeriodFourth".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AmilyIsEquallyEnamored".Localized(
                Player.Mf("male", "female")
            ));

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.OnTheOtherSide".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.IzmaSSkirtRustles".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.IzmaSSkirtVisibly".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.StaggeringUnderTheWeight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) > 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Former".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.HermaphroditeGigglesAndWhispers".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AmilySBigFuzzy".Localized());

            if (Player.AverageNipplesPerBreast() <= 1)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.OneOfYourNipples".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AtYourMultitudeOf".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.PantingEagerlySheLicks".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AmilyAbruptlyGruntsIn".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AmilySurprisesIzmaWith".Localized(
                AmilyButt()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.TheSharkGirlS".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.IzmaSTailSuddenly".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.FromWhereYouAre".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.YouLumberUpTo".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.YouEvenCramYour".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.YouEvenCramAn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.YouEvenBumpUp".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.TheLadiesPleasedMoans".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Cocks".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Text.Two".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.TheSapphicKissIs".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.NoGiveItTo".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.MyPussyIsThe".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.IMTheStronger".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.BesidesCanTYou".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AmilySqueaksInSurprise".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.GiveMeYourBabies".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.JustAhhhPutIt".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.ActuallyBlushingSoHard".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.BlushingSoHardHer".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Text".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.YouPushAndPull".Localized());

            if (!Player.IsPregnant() || !Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Womb".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Cunt".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.JudgingByHowFull".Localized());

            if (!Player.IsPregnant() || !Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Pregnant".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Squirting".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.ForYouEachTime".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.DoubledDicks".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.Text.Three".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.HomeYouWindUp".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.StrainingBallsSlosh".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.StrainingFullnessTingles".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.MeaningfullyRemindingYouThat".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.WithoutWarningYouPull".Localized());

            if (Player.BiggestCockArea() > 61)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AsFarIntoAmily".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.StraightIntoAmilyS".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.IzmaSFingerRubs".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.AmilyStopsSucklingThe".Localized(
                AmilyHips()
            ));

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.HerBellyBeginsTo".Localized());
            else
            {
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.WithHerWombAlready".Localized());

                if (Player.HasKnot(Player.BiggestCockIndex()))
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.ButYourKnotTraps".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex.SprayingOutInHigh".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IzmaAmilyDrugThreeWaySex2);
        }

        public void IzmaAmilyDrugThreeWaySex2()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TimeGoesHazyFor".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Cum".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.PregnantBellyAndMurmuring".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IzmaSeemsToHave".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.DickAndBallsFlopping".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.ShePantsRubbingHer".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.BeInseminated".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.HaveHerPregnantPussy".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Text.Five".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.BehindHerHasRemained".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.ThePressureIsAs".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YourAreStretchingYour".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TheNeedToCum".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IzmaRisesOnShaky".Localized());

            if (Player.BiggestCockArea() <= 65)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.BottomsOut".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TakesAsMuchAs".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.ThereSASquirt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.AlongWithAMatching".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Text.Two.Two".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.StrongHandsWrapAround".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TwoOf".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YourExtraPenis".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Es".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Jacking".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.It".Localized());
                else
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Them".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.WithSmoothConfidentStrokes".Localized());
            }

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YouLookUpAt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IgnoringTheDribblyCock".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.InstinctiveHalfThoughtVoices".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IzmaLeansDown".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TakingAHandAway".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TakingAHandAway.Two".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.AndKissesYouPassionately".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Closed".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Dilated".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Cervix".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Text.Two.Two".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Text.Three".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IzmaSEyesTwinkle".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.MakeMeYourGravid".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.CumInYourPregnant".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Text.Four".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.FuckItSLike".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.WatchingABitchS".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.StuffingACuntSo".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.GettingToLieBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.HerFourBallsQuake".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.LadySpunkWashesAcross".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TheSmellOfHer".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YouFeelLikeAn".Localized());

            if (Player.IsPregnant())
            {
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.HerWombRemainsSteadfastly".Localized());

                if (Player.HasKnot(Player.BiggestCockIndex()))
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YourKnotBalloonsExplosively".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.JismRushesOutAround".Localized());
            }
            else
            {
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.HerWombBeckonsFor".Localized());

                if (Player.HasKnot(Player.BiggestCockIndex()))
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.OfCourseWithA".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TheTigersharkBabblesAnd".Localized());

                if (Player.HasKnot(Player.BiggestCockIndex()))
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YourDongForcedOut".Localized());

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Text.Five".Localized());
            }

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IzmaPantsAndCradles".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.JizzBloated".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.ChildBearing".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.MiddleYouReStill".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.TheGravidMouseGirl".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.AsTheOrgasmPasses".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.IzmaIsStillFairly".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.YourVisionGoesBlack".Localized(
                AmilyButt()
            ));

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.AmilyMoansWhileIzma".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.Are".Localized());
            else
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.BodyIs".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex2.AlreadyRefillingAndYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IzmaAmilyDrugThreeWaySex3);
        }

        public void IzmaAmilyDrugThreeWaySex3()
        {
            ClearOutput();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.SomeTimeLater".Localized());

            OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.YouComeToIn".Localized());

            if (!Player.IsPregnant() || !Player.IsPregnant())
            {
                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.Text".Localized());

                if (!Player.IsPregnant())
                {
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.Amily".Localized());

                    Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.MouseIncubation - 182, 100, 1);

                    if (!Player.IsPregnant())
                        OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.And".Localized());
                }

                if (!Player.IsPregnant())
                {
                    OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.Izma".Localized());

                    Player.PregnancyStore.KnockUpForce(PregnancyIds.PlayerFather, PregnancyStore.IncubationIzma);
                }

                OutputText("Scenes.Amilyscene.IzmaAmilyDrugThreeWaySex3.DefinitelyGotPregnant".Localized());
            }

            DoNext(() => ReturnToCamp(4));
        }

        public void AmilyNurseCheckup()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GivenAmilyNurseOutfit) == 0)
            {
                Player.ConsumeItem(ItemRegistry.Create(ArmorIds.NurseOutfit));

                CoC.Instance.Flags.Set(GameFlagIds.GivenAmilyNurseOutfit, 1);

                OutputText("Scenes.Amilyscene.AmilyNurseCheckup.YouHandAmilyThe".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckup.SheRoundsOnYou".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckup.YouTryToAnswer".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckup.ASultryVoicePurrs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AmilyClothing, "a naughty nurse's outfit");

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => AmilyNurseCheckupV2(false));
            }
            else
            {
                OutputText("Scenes.Amilyscene.AmilyNurseCheckup.AmilySaysSheTurns".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckup.NurseTimeForCheckup".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => AmilyNurseCheckupV2(true));
            }
        }

        public void AmilyNurseCheckupV2(bool repeat)
        {
            ClearOutput();

            var x = Player.CockThatFits(61);

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.EnterAmilyOffice".Localized(
                AmilyTits()
            ));

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 2 || __sw0_0 == 3)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SlightlyProtrudingPregnancy".Localized());

            if (__sw0_0 == 4 || __sw0_0 == 5)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SwollenTummyRoundsOut".Localized());

            if (__sw0_0 == 6 || __sw0_0 == 7 || __sw0_0 == 8)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.BloatedBellyBulgesOut".Localized());

            if (true )
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.TautMidriffOnDisplay".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ClingyMicroSkirt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SheerOutlineBulge".Localized(
                    AmilyCock()
                ));

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 9)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.CoveringEffortFails".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.NurseHatRedCross".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.PlayfullyRecites".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ListenSittingOnStone".Localized());

            if (!AmilyFurry())
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.HandWord".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.PawWord".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.TouchesYourThigh".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.BeginExamination".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.NodEagerly".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.MurmursPainfulCondition".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SoftSqueezeEngorgement".Localized());
            else
            {
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.HeftsYourBalls".Localized());

                if (Player.BallSize <= 4)
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.RollingFingersSlySmile".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.FingersSinkIntoOrb".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.Text.Four".Localized());
            }

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.WatchesYourFace".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SneakyFingerTests".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.Text.Two".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.AmilyExplains".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.BlueBallsCondition".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.BoneritusCondition".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.VirileMalesCondition".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.AsksWhatIsIt".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ReleasesEngorgedPecker".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.PlumpBallsAre".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.StiffShaftIs".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.FreedFromNeeds".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.NeedyPinprickPupils".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.AnotherSymptomPants".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.NoseGrindsClit".Localized(
                AmilyButt()
            ));

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.UnbuttonsTopBreasts".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.IntoYourLap".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.EquineLowerHalf".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.StuttersAligningEntrance".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.MoansHeadThrownBack".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.HandFindsWay".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ExtraCocksPlural".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ExtraCockSingular".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.PumpsSpareShaft".Localized());
            }

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.Text.Three".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.DragsClingingPussy".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SkillfulCockManipulation".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SteadySpurtRhythm".Localized());

                if (!Player.IsPregnant())
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.FillsWombToCapacity".Localized());
                else
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ForcesSemenTrailsOut".Localized());

                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.Text.Four".Localized());

                if (Player.CumQ() >= 2000)
                {
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.ClingsForDearLife".Localized());

                    if (!Player.IsPregnant())
                        OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.FilledToCapacity".Localized());
                    else
                        OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.TorrentsSprayOut".Localized());
                }

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.CrotchStickyMess".Localized());
            }

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.BodyGoesLimp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.FlingsStickyRopes".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SagsDownHugging".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.KnotKeepsHerRooted".Localized(
                    x + 1
                ));

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.SheClimbsOffYou".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.KissAndThankHer".Localized());

            OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.TiltsHeadBlissful".Localized());

            if (!repeat)
                OutputText("Scenes.Amilyscene.AmilyNurseCheckupV2.NurseOptionUnlocked".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            AmilyPreggoChance();

            DoNext(() => ReturnToCamp(1));
        }

        public void IdLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.IdLiddellium.AmilyAlchemyExperience".Localized());

            OutputText("Scenes.Amilyscene.IdLiddellium.ChirpyButUnsure".Localized());

            OutputText("Scenes.Amilyscene.IdLiddellium.ThanksForEffort".Localized());

            DoNext(() => AmilyMenu());
        }

        public void AmilyWinterCuddles()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyWinterCuddles.DespiteWarmClothing".Localized());

            OutputText("Scenes.Amilyscene.AmilyWinterCuddles.SharesBlanketIdea".Localized());

            OutputText("Scenes.Amilyscene.AmilyWinterCuddles.FireNotWarmest".Localized());
            // 原版 AmilyScene.hx:9560：Akky 已被收养时会跳进 Amily 怀里
            if (SceneInstance<AkkyScene>.Get().IsOwned())
                OutputText("Scenes.Amilyscene.AmilyWinterCuddles.AkkyJumpsInLap".Localized());
            else
                OutputText("Scenes.Amilyscene.AmilyWinterCuddles.TheTwoWord".Localized());

            OutputText("Scenes.Amilyscene.AmilyWinterCuddles.SitByBurningLogs".Localized());

            OutputText("Scenes.Amilyscene.AmilyWinterCuddles.GenuinePeaceAndLove".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilyCookingLesson()
        {
            ClearOutput();

            AmilySprite();

            OutputText("Scenes.Amilyscene.AmilyCookingLesson.SettleInWithAmily".Localized());

            OutputText("Scenes.Amilyscene.AmilyCookingLesson.KihaIsScary".Localized());

            OutputText("Scenes.Amilyscene.AmilyCookingLesson.SurprisedSheWould".Localized());

            OutputText("Scenes.Amilyscene.AmilyCookingLesson.RubsArmSheepishly".Localized());

            OutputText("Scenes.Amilyscene.AmilyCookingLesson.GoodSoundLogic".Localized());

            OutputText("Scenes.Amilyscene.AmilyCookingLesson.GrinsAndHopsUp".Localized());
            // 原版 AmilyScene.hx:9579：标记 Kiha 已尝过「改进后」的料理
            KihaFollowerScene.Save.SetInt("badCookingTasted", 2);

            DoNext(() => ReturnToCamp(1));
        }

        public bool PlayerHasRing()
        {

            for (var x = 0; x < 10; x++)
                if (Player.Inventory.Slots[x].Quantity > 0 && (Player.Inventory.Slots[x].Item.Id.ToString() == ItemIds.SilverRing || Player.Inventory.Slots[x].Item.Id.ToString() == ItemIds.GoldRing || Player.Inventory.Slots[x].Item.Id.ToString() == ItemIds.PlatinumRing || Player.Inventory.Slots[x].Item.Id.ToString() == ItemIds.DiamondRing))
                    return true;

            return false;
        }

        public void AmilyRingGifting()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyRingGifting.WhatToGiveHer".Localized());

            ClearButtons();

            AddNextButton("Items.SilverRing.Short".Localized(), () => AmilyRingGiftingReaction(0));

            AddNextButton("Items.GoldRing.Short".Localized(), () => AmilyRingGiftingReaction(1));

            AddNextButton("Items.PlatinumRing.Short".Localized(), () => AmilyRingGiftingReaction(2));

            AddNextButton("Items.DiamondRing.Short".Localized(), () => AmilyRingGiftingReaction(3));

            AddButton(14, "Common.Button.Back".Localized(), () => AmilyGiftMenu());
        }

        public void AmilyRingGiftingReaction(int type)
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.GiftWithClosedEyes".Localized());

            OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.FeelsCoolMetal".Localized());

            switch (type)
            {
                case 0:
                {
                OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.AdmiresSilverBand".Localized());

                Save.SetString("ringType", "silver");

                Player.ConsumeItem(ItemIds.SilverRing);
                }

                break;

                case 1:
                {
                OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.SurprisedGoldBand".Localized());

                Save.SetString("ringType", "gold");

                Player.ConsumeItem(ItemIds.GoldRing);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.PlatinumBandAmazed".Localized());

                Save.SetString("ringType", "platinum");

                Player.ConsumeItem(ItemIds.PlatinumRing);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.DiamondBandSparkles".Localized());

                Save.SetString("ringType", "diamond");

                Player.ConsumeItem(ItemIds.DiamondRing);
                }

                break;
            }

            OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.VoiceQuiversRing".Localized());

            OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.ExplainsBackwardsOrder".Localized());

            OutputText("Scenes.Amilyscene.AmilyRingGiftingReaction.EmbraceEachOther".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AmilySleepWith()
        {
            ClearOutput();

            if (View.Time.Hours >= 21)
                View.Time.LastPassedHours = 24 - View.Time.Hours;
            else
                View.Time.LastPassedHours = 0;

            View.Time.Hours += CampScene.WakeupTime();

            AmilySprite(true);

            OutputText("Scenes.Amilyscene.AmilySleepWith.AsksToSnuggle".Localized());

            OutputText("Scenes.Amilyscene.AmilySleepWith.StripsBeforeLaying".Localized());

            if (Player.Health <= Player.MaxHealth / 2)
            {
                OutputText("Scenes.Amilyscene.AmilySleepWith.NoticesYourInjuries".Localized());

                OutputText("Scenes.Amilyscene.AmilySleepWith.FindsTheOintment".Localized());

                OutputText("Scenes.Amilyscene.AmilySleepWith.OintmentOnCuts".Localized());

                OutputText("Scenes.Amilyscene.AmilySleepWith.PutsMedicineAway".Localized());

                Player.HealthChange(Player.MaxHealth / 5);
            }

            OutputText("Scenes.Amilyscene.AmilySleepWith.WrapsArmsAroundYou".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Amily");

            Save.SetBool("amilyMorning", true);

            DoNext(() => CampScene.Sleep());
        }

        public void AmilyWakeUp()
        {
            ClearOutput();

            AmilySprite(true);

            if (Player.Lust < 40 || Player.IsGenderless())
                OutputText("Scenes.Amilyscene.AmilyWakeUp.WakesBeforeAmily".Localized());
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Amilyscene.AmilyWakeUp.WarmMoistSensation".Localized());

                    OutputText("Scenes.Amilyscene.AmilyWakeUp.HotMoanSucking".Localized());

                    OutputText("Scenes.Amilyscene.AmilyWakeUp.PantsNearRelease".Localized());

                    OutputText("Scenes.Amilyscene.AmilyWakeUp.PullsFlaccidManhood".Localized());

                    OutputText("Scenes.Amilyscene.AmilyWakeUp.BashfulMorningSorry".Localized());

                    Player.Orgasm("Dick");
                }
                else
                {
                    OutputText("Scenes.Amilyscene.AmilyWakeUp.WakesQuiveringBody".Localized());

                    OutputText("Scenes.Amilyscene.AmilyWakeUp.NeedyGaspsForMore".Localized());

                    OutputText("Scenes.Amilyscene.AmilyWakeUp.PantsInExhaustion".Localized());

                    Player.Orgasm("Vaginal");
                }

            Save.SetBool("amilyMorning", false);

            DoNext(PlayerMenu);
        }

        public void AmilyKidMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyKidMeeting.StretchesLimbs".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeeting.GottaComeWithMe".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeeting.CuteButWorkedUp".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeeting.KidsAreVisiting".Localized());

            ClearButtons();

            AddNextButton("Common.Button.MeetKids".Localized(), AmilyKidMeetingGo);

            AddNextButton("Common.Button.Leave".Localized(), AmilyKidMeetingLeave);
        }

        public void AmilyKidMeetingGo()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.ElationIsInfectious".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.AmilyBouncingEager".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.ChaosOnArrival".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.SeatsAroundFire".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.AmilyEnjoysHerself".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.RareAndCherished".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.CrowdEnergyFades".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingGo.MiceSayGoodbye".Localized());

            Save.SetInt("metKids", 1);

            DoNext(() => ReturnToCamp(4));
        }

        public void AmilyKidMeetingLeave()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.AmilyKidMeetingLeave.SheMayGoStay".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingLeave.ConfusedThenDeflates".Localized());

            OutputText("Scenes.Amilyscene.AmilyKidMeetingLeave.KidsFineWithMother".Localized());

            Save.SetInt("metKids", -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void ForestDate()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.ForestDate.SuggestForestTrip".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.AmilyAgreesSlowly".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.SharesHerExperience".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.NervousButAgrees".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.SetOffQuietly".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.PausesSniffingTrees".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.ReachClearingRest".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.AffirmationBolsters".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.RemarkableNonetheless".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.MakeDoBest".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.NeedABreather".Localized());

            OutputText("Scenes.Amilyscene.ForestDate.AssuresEnjoyment".Localized());

            ClearButtons();

            AddNextButton("Common.Button.MakeLove".Localized(), ForestDateFuck);

            AddNextButton("Common.Button.Kiss".Localized(), ForestDateKiss);

            AddNextButton("Common.Button.Gather".Localized(), ForestDateGather);

            AddNextButton("Common.Button.HeadBack".Localized(), ForestDateLeave);
        }

        public void ForestDateFuck()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.ForestDateFuck.AsksWhatSheMeans".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.UncertainStammer".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.CutsHerOffAsking".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.RightHereQuestion".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.RightHereCertain".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.ButTheresNo".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.NoTimeForProtest".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.HaahExhale".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.IncompleteConfirmation".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.GuidesHandToAss".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.SlumpsToTheGround".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.MaraeYesMoan".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.SweetSoundsBrink".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.PleadingCumForMe".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.QuietAlmostWhisper".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.PulsesInsideHer".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.RegainsComposure".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.BackToCampAgain".Localized());

            OutputText("Scenes.Amilyscene.ForestDateFuck.SurprisingConfidence".Localized());

            Player.Orgasm();

            DoNext(() => ReturnToCamp(1));
        }

        public void ForestDateKiss()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.ForestDateKiss.WordlesslyGlides".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.BarelyConsciousMove".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.AcheFadingRelief".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.PullsNeckBack".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.BreathlessWow".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.SumsThingsUp".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.WryGoOutAgain".Localized());

            OutputText("Scenes.Amilyscene.ForestDateKiss.SighsHappilyYes".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ForestDateGather()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.ForestDateGather.StillMoreToTeach".Localized());

            OutputText("Scenes.Amilyscene.ForestDateGather.InterestInAlchemy".Localized());

            OutputText("Scenes.Amilyscene.ForestDateGather.NodsThenExplains".Localized());

            OutputText("Scenes.Amilyscene.ForestDateGather.HerbOveruseTangent".Localized());

            OutputText("Scenes.Amilyscene.ForestDateGather.ClaspHandsThenLeave".Localized());

            var forestGifts = new[]             {                 ConsumableIds.PigtailTruffle,                 ConsumableIds.FoxBerry,                 ConsumableIds.CinnamonMint,                 ConsumableIds.PumpkinSlice             };              Player.Inventory.Add(ItemRegistry.Create(forestGifts[Random.Shared.Next(forestGifts.Length)]), 1);              ReturnToCamp(1);
        }

        public void ForestDateLeave()
        {
            ClearOutput();

            OutputText("Scenes.Amilyscene.ForestDateLeave.MoreThanEnough".Localized());

            OutputText("Scenes.Amilyscene.ForestDateLeave.HomeToReturnTo".Localized());

            OutputText("Scenes.Amilyscene.ForestDateLeave.EasyPaceBack".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CaughtAddingSkulls()
        {
            ClearOutput();

            bool noSkulls = CoC.Instance.Flags.Get(GameFlagIds.CampWallSkulls) == 0;

            OutputText("Scenes.Amilyscene.CaughtAddingSkulls.PatrolSpotsSkull".Localized());

            OutputText("Scenes.Amilyscene.CaughtAddingSkulls.ItIsASkull".Localized(
                noSkulls
            ));

            OutputText("Scenes.Amilyscene.CaughtAddingSkulls.SurveyCampForCulprit".Localized(
                noSkulls
            ));

            OutputText("Scenes.Amilyscene.CaughtAddingSkulls.CheerilySeenGhost".Localized());

            OutputText("Scenes.Amilyscene.CaughtAddingSkulls.ExplainsSkullOnWall".Localized(
                noSkulls
            ));

            OutputText("Scenes.Amilyscene.CaughtAddingSkulls.GuiltyMouseLook".Localized(
                noSkulls
            ));

            if (!Save.GetBool("pcKnowsAboutSkulls"))
            {
                CoC.Instance.Flags.Set(GameFlagIds.CampWallSkulls, CoC.Instance.Flags.Get(GameFlagIds.CampWallSkulls) + Save.GetInt("impSkullsCount", 0));

                Save.SetBool("pcKnowsAboutSkulls", true);
            }

            DoNext(PlayerMenu);
        }

    }
}