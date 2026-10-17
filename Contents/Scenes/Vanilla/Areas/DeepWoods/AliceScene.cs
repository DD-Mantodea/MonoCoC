using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.DeepWoods
{
    [Register<AliceScene>("coc:alice")]
    public class AliceScene : Scene
    {

        public static bool lewd;

        public static bool canArouse;

        public static string eyeColor = "blue";

        public static string panties = "white";

        public static string pantiesLong = "plain";

        public AliceEnemy? _currentAlice;

        public AliceEnemy currentAlice => _currentAlice ??= RandomAlice();

        public static SaveContent Save => SaveContent(SceneIds.Deepwoods.Alice);

        public string hairColor => currentAlice.Body.Get<HairPart>().Color;

        public string skinTone => currentAlice.Body.Get<SkinPart>().Tone;

        public string debugName => "Alice";

        public string debugHint => "";

        public override void Entry()
        {
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            GenerateAlice();

            canArouse = false;

            var chatCount = NumberUtils.CountSetBits(CoC.Instance.Flags.Get(GameFlagIds.AliceChats));

            var foxChance = Math.Min(15, 3 * chatCount);

            if (!Save.GetBool("foxSeen"))
                foxChance *= 3;

            if (Save.GetBool("foxWatched"))
                foxChance *= 2;

            if (Save.GetBool("foxNeedArouse") && canArouse)
                foxChance *= 2;

            if (CanRent() && Random.Shared.Next(100) < (false ? 50 : 100))
                RentAnAlice();
            else
                if (chatCount > 1 && (Player.IsInForest || Player.IsInDeepwoods) && foxChance > Random.Shared.Next(100))
                    AliceFoxIntro();
                else
                    if (StrayAkkyReady() && Random.Shared.Next(100) < (50))
                        StrayAkky();
                    else
                        AliceEncounter();
        }

        public void Reset()
        {

            Save.SetBool("foxSeen", false);

            Save.SetBool("foxWatched", false);

            Save.SetBool("foxNeedArouse", true);

            Save.SetBool("rented", false);

            Save.SetBool("knowInnocent", false);

            Save.SetBool("knowBossy", false);

            Save.SetBool("akkySeen", false);

            Save.SetInt("trystOrTreatedYear", 0);
        }

        public void AliceCorruption(int lust = 15, bool lewded = false, bool dicked = false, bool killed = false)
        {

            if (lust != 0)
                Player.TakeLustDamage(lust, false);

            if (lewded)
            {
                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 1f));

                Player.ChangeFatigue(5);
            }

            if (dicked && !killed)
            {
                Player.ApplyStats(new DynStat(GameStats.Strength, -1f));

                Player.ChangeFatigue(5);
            }

            if (killed)
            {
                if (Player.Corruption < 25)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, -0.5f));

                CoC.Instance.Flags.Set(GameFlagIds.AlicesKilled, CoC.Instance.Flags.Get(GameFlagIds.AlicesKilled) + 1);

                Player.UpgradeDeusVult();
            }
        }

        public void GenerateAlice()
        {

            _currentAlice = RandomAlice();
        }

        public AliceEnemy RandomAlice()
        {

            var eyeChoice = new WeightedRandom<string>()
                .Add("hazel", 2)
                .Add("brown", 2)
                .Add("blue", 1)
                .Add("green", 1);

            var hairChoice = new WeightedRandom<string>()
                .Add("bronze", 2)
                .Add("brown", 2)
                .Add("auburn", 2)
                .Add("blonde", 1)
                .Add("black", 1)
                .Add("red", 1);

            var skinChoice = new WeightedRandom<string>()
                .Add("milky-white", 4)
                .Add("fair", 3)
                .Add("olive", 2)
                .Add("dark", 1)
                .Add("ebony", 1)
                .Add("mahogany", 1)
                .Add("russet", 1);

            var pantyChoice = new WeightedRandom<string>()
                .Add("white", 4)
                .Add("striped", 2)
                .Add("black", 1);

            eyeColor = eyeChoice.Choose();

            panties = pantyChoice.Choose();

            pantiesLong = panties switch
            {
                "white" => "pristinely white",

                "striped" => "cute striped",

                "black" => "black, lacy",

                _ => "plain"
            };

            return new AliceEnemy(hairChoice.Choose(), skinChoice.Choose(), eyeColor, panties);
        }

        public bool CanRent()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) == 0 && !Save.GetBool("rented");
        }

        public bool StrayAkkyReady()
        {

            return false && CoC.Instance.Flags.Get(GameFlagIds.CodexEntryAlice) != 0 && (Player.IsInForest || Player.IsInDeepwoods) && !Save.GetBool("akkySeen");
        }

        public void AliceEncounter()
        {

            var bossyChance = 20;

            if (Random.Shared.Next(100) < (bossyChance))
                AliceEncounterBossy();
            else
                AliceEncounterInnocent();
        }

        public void AliceEncounterInnocent()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceEncounterInnocent.WanderStrangeSenseCalmness".Localized());

            if (Player.Tallness > 50)
                OutputText("Scenes.Alicescene.AliceEncounterInnocent.LooksUpAtYou".Localized());
            else
                OutputText("Scenes.Alicescene.AliceEncounterInnocent.LooksOverAtYou".Localized());

            OutputText("Scenes.Alicescene.AliceEncounterInnocent.ShyInnocentExpressionUnsure".Localized());

            if (Save.GetBool("knowInnocent"))
            {
                OutputText("Scenes.Alicescene.AliceEncounterInnocent.FamiliarSensationPreying".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Leave".Localized(), AliceIgnore);

                AddButton(1, "Common.Button.Confront".Localized(), AliceConfront);

                AddButton(2, "Common.Button.Trust".Localized(), AliceTrust);
            }
            else
            {
                OutputText("Scenes.Alicescene.AliceEncounterInnocent.ThingsApprehensionWellPlaced".Localized());

                OutputText("Scenes.Alicescene.AliceEncounterInnocent.BeginsToSpeakPlay".Localized());

                OutputText("Scenes.Alicescene.AliceEncounterInnocent.OfCourseJovialDemeanor".Localized());

                OutputText("Scenes.Alicescene.AliceEncounterInnocent.StopsTwirlsDazzlingEyes".Localized());

                OutputText("Scenes.Alicescene.AliceEncounterInnocent.TouchOfHandsShiver".Localized());

                AliceCorruption(15);

                ClearButtons();

                AddButton(0, "Common.Button.Continue".Localized(), AliceWilling);

                AddButton(1, "Common.Button.Stop".Localized(), AliceStop);
            }
        }

        public void AliceIgnore()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIgnore.SighingNotInterestedGames".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceConfront()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceConfront.TakeBreathClearMind".Localized());

            OutputText("Scenes.Alicescene.AliceConfront.SighingRelentsSeenKind".Localized());

            ConfrontMenu();
        }

        public void AliceNotNice()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceNotNice.RaisingWeaponEyesWiden".Localized());

            currentAlice.OnDefeated = _ => AliceWin();

            currentAlice.OnWon = _ => AliceLoss();

            CombatScene.Start(currentAlice, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void AliceEncounterBossy()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceEncounterBossy.TravelsSlowOddSensation".Localized((Player.IsInMountains ? "rock" : "tree")));

            OutputText("Scenes.Alicescene.AliceEncounterBossy.LeapingOutPointingFace".Localized());

            if (Save.GetBool("knowBossy"))
            {
                OutputText("Scenes.Alicescene.AliceEncounterBossy.MindClicksSensesAlice".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Confront".Localized(), BossyConfront);

                AddNextButton("Common.Button.Trust".Localized(), BossyTrust);

                AddButton(14, "Common.Button.Leave".Localized(), () => BossyLeave());
            }
            else
            {
                OutputText("Scenes.Alicescene.AliceEncounterBossy.GrantingSenseOfSafety".Localized());

                if (Player.Intelligence < 20)
                    DoNext(() => BossyTrust());
                else
                {
                    ClearButtons();

                    AddNextButton("Common.Button.Trust".Localized(), BossyTrust);

                    AddNextButton("Common.Button.Stop".Localized(), BossyStop);
                }
            }

            AliceCorruption();
        }

        public void BossyConfront()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.BossyConfront.RippingHandGraspMagic".Localized());

            ConfrontMenu();

            AliceCorruption();
        }

        public void BossyTrust()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.BossyTrust.WhateverPlanFollowNow".Localized());

            BossyContinue();

            AliceCorruption();
        }

        public void BossyLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.BossyLeave.SlippingHandGraspReject".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BossyStop()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.BossyStop.DespiteComfortingNotAlong".Localized());

            OutputText("Scenes.Alicescene.BossyStop.BrowStatementRingsTrue".Localized());

            BossyContinue();

            AliceCorruption();
        }

        public void BossyContinue()
        {

            OutputText("Scenes.Alicescene.BossyContinue.TugHandTearsAssessing".Localized());

            OutputText("Scenes.Alicescene.BossyContinue.WhatDoingSavingLost".Localized());

            OutputText("Scenes.Alicescene.BossyContinue.CantTakeCareYourself".Localized());

            OutputText("Scenes.Alicescene.BossyContinue.TappingAgainstCrotchScoffs".Localized(((Player.HasCock() || (Player.IsNakedLower() && !Player.IsGenderless())) ? "T" : "It's a wonder how she can tell, as you don't think there'd be any sign so obvious, and t")));

            OutputText("Scenes.Alicescene.BossyContinue.HmphCrossesArmsSupposes".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Sex".Localized(), BossySex);

            AddNextButton("Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), BossyDeny);
        }

        public void BossySex()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Alicescene.BossySex.PressesAgainstGroinRubbing".Localized());

                OutputText("Scenes.Alicescene.BossySex.KidRequireCompliancePushes".Localized((Player.Race == "dryad" ? "botany" : ((Player.Race == "goo-girl" || Player.Race == "goo-boy") ? "mass" : "biology"))));

                OutputText("Scenes.Alicescene.BossySex.GigglesAnxiousBodyLanguage".Localized());

                OutputText("Scenes.Alicescene.BossySex.LowersHerselfOntoCock".Localized());

                OutputText("Scenes.Alicescene.BossySex.NoHesitationJerksHips".Localized());

                OutputText("Scenes.Alicescene.BossySex.SitsDownAndRides".Localized(
                    Player.Body.Get<SkinPart>().Type.GetString("skin") == "bark" ? " stalk" : (Player.HasGooSkin() ? ", slimy " : " ")
                ));

                Player.Orgasm("Dick");
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Alicescene.BossySex.CaressesGroinAgainTeach".Localized());

                    OutputText("Scenes.Alicescene.BossySex.CrawlsBackTeasingNibble".Localized());

                    OutputText("Scenes.Alicescene.BossySex.TongueEncirclesEntranceTense".Localized());

                    OutputText("Scenes.Alicescene.BossySex.EyesGlazedHoversLast".Localized());

                    Player.Orgasm("Vaginal");
                }
                else
                {
                    OutputText("Scenes.Alicescene.BossySex.CaressesAgainTeachAgain".Localized());

                    OutputText("Scenes.Alicescene.BossySex.RatherWorkedNoGenitals".Localized());

                    OutputText("Scenes.Alicescene.BossySex.ComplyLowerToGround".Localized());

                    OutputText("Scenes.Alicescene.BossySex.PalmsSlideDownRibs".Localized(
                        Player.Race == "dryad" ? "botany" : ((Player.Race == "goo-girl" || Player.Race == "goo-boy") ? "mass" : "biology")
                    ));

                    OutputText("Scenes.Alicescene.BossySex.GazeLiftedDressPanties".Localized());

                    OutputText("Scenes.Alicescene.BossySex.PushesKeepFeelingGood".Localized());

                    OutputText("Scenes.Alicescene.BossySex.GyratingMotionReturns".Localized());

                    OutputText("Scenes.Alicescene.BossySex.HopesFindAgainFading".Localized());

                    Player.Orgasm("Anal");
                }

            AliceCorruption(15, true, Player.HasCock());

            DoNext(() => ReturnToCamp(1));
        }

        public void BossyDeny()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AliceChats, CoC.Instance.Flags.Get(GameFlagIds.AliceChats) | 1);

            Save.SetBool("knowBossy", true);

            OutputText("Scenes.Alicescene.BossyDeny.SurroundingsIllusionMindMessed".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryAlice);

            currentAlice.OnDefeated = _ => AliceWin();

            currentAlice.OnWon = _ => AliceLoss();

            CombatScene.Start(currentAlice, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void ConfrontMenu()
        {

            var tailValid = new[] { BodyPartTypes.Dog, BodyPartTypes.Demonic, BodyPartTypes.Cat, BodyPartTypes.Lizard, BodyPartTypes.Kangaroo, BodyPartTypes.Draconic, BodyPartTypes.Salamander, BodyPartTypes.Wolf, BodyPartTypes.Imp }.IndexOf(Player.Body.Get<TailPart>().Id) >= 0;

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), AliceNotNice);

            AddButton(1, "Common.Button.Talk".Localized(), AliceChatSelect);

            AddButton(2, "Common.Button.Tailsex".Localized(), AliceTailfuck);

            if (!Player.IsGenderless() || OptionStore.Instance.Get("watersports", 0) > 0)
                AddNextButton("Common.Button.GetIntimate".Localized(), AliceIntimate);

            AddNextButton("Common.Button.Nurse".Localized(), AliceNursing);

            AddNextButton("Common.Button.Headpats".Localized(), AlicePatMenu);
        }

        public void AliceChatSelect()
        {

            var chat1 = 1 << 1;

            var chat2 = 1 << 2;

            var chat3 = 1 << 3;

            var select = new List<int>();

            if ((CoC.Instance.Flags.Get(GameFlagIds.AliceChats) & chat1) == 0)
                select.Add(1);

            if ((CoC.Instance.Flags.Get(GameFlagIds.AliceChats) & chat2) == 0)
                select.Add(2);

            if ((CoC.Instance.Flags.Get(GameFlagIds.AliceChats) & chat3) == 0)
                select.Add(3);

            if (select.Count == 0)
                select = [1, 2, 3];

            switch (select[Random.Shared.Next(select.Count)])
            {
                case 1:
                    AliceChat1();

                    return;

                case 2:
                    AliceChat2();

                    return;

                case 3:
                    AliceChat3();

                    return;
            }
        }

        public void AliceChat1()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AliceChats, CoC.Instance.Flags.Get(GameFlagIds.AliceChats) | 2);

            OutputText("Scenes.Alicescene.AliceChat1.FormUnfurlsWingsStretch".Localized());

            OutputText("Scenes.Alicescene.AliceChat1.HoldHandsLeanElbow".Localized());

            OutputText("Scenes.Alicescene.AliceChat1.ExplainsDemonDisobeysPunished".Localized());

            OutputText("Scenes.Alicescene.AliceChat1.AngerPalpableTryingCalm".Localized());

            OutputText("Scenes.Alicescene.AliceChat1.HeatGraduallyBuilding".Localized());

            AliceCorruption(15);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceChat2()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AliceChats, CoC.Instance.Flags.Get(GameFlagIds.AliceChats) | 4);

            OutputText("Scenes.Alicescene.AliceChat2.PerksEnergeticallyWingsTail".Localized());

            OutputText("Scenes.Alicescene.AliceChat2.DashesCloseHandsUp".Localized());

            OutputText("Scenes.Alicescene.AliceChat2.SighsCoupleMaybeMore".Localized());

            OutputText("Scenes.Alicescene.AliceChat2.HumoringElaborateSituation".Localized());

            if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                OutputText("Scenes.Alicescene.AliceChat2.EyesWidenBlankShock".Localized());

            OutputText("Scenes.Alicescene.AliceChat2.AllQuestStopLethice".Localized());

            OutputText("Scenes.Alicescene.AliceChat2.WandersOffMinorStumble".Localized());

            AliceCorruption(15);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceChat3()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AliceChats, CoC.Instance.Flags.Get(GameFlagIds.AliceChats) | 8);

            OutputText("Scenes.Alicescene.AliceChat3.CrossesArmsPondersWay".Localized());

            OutputText("Scenes.Alicescene.AliceChat3.AstuteObservationLargeHard".Localized());

            OutputText("Scenes.Alicescene.AliceChat3.VeryInformativeSurprising".Localized());

            OutputText("Scenes.Alicescene.AliceChat3.StaresSultryGlareSmirk".Localized());

            OutputText("Scenes.Alicescene.AliceChat3.ManipulativeBacksHandsRaised".Localized());

            OutputText("Scenes.Alicescene.AliceChat3.LeavesHotAndBothered".Localized());

            AliceCorruption(15);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceTrust()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceTrust.SurelyWouldNeverAdorable".Localized());

            OutputText("Scenes.Alicescene.AliceTrust.BeginsToSpeakPlay".Localized());

            OutputText("Scenes.Alicescene.AliceTrust.OfCourseJovialDemeanor".Localized());

            OutputText("Scenes.Alicescene.AliceTrust.StopsTwirlsDazzlingEyes".Localized());

            OutputText("Scenes.Alicescene.AliceTrust.TouchOfHandsShiver".Localized());

            AliceCorruption(15);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), AliceWilling);
        }

        public void AliceWilling()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceWilling.RollGrassCuddleGlee".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Alicescene.AliceWilling.GrindPulsatingMemberArmor".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Alicescene.AliceWilling.GrindMoisteningVaginaLeg".Localized());

            OutputText("Scenes.Alicescene.AliceWilling.RealizingMistakeClothedStrip".Localized());

            OutputText("Scenes.Alicescene.AliceWilling.WideEyesScaredStop".Localized());

            OutputText("Scenes.Alicescene.AliceWilling.TellOntoMoreFun".Localized());

            if (Player.HasLongTongue())
                OutputText("Scenes.Alicescene.AliceWilling.LongUndulatingTongueLabia".Localized());
            else
                OutputText("Scenes.Alicescene.AliceWilling.TongueSlipsBetweenLabia".Localized());

            OutputText("Scenes.Alicescene.AliceWilling.SlideTongueTwirlProd".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Alicescene.AliceWilling.PullForwardOntoKnees".Localized());

                Player.Orgasm("Dick");
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Alicescene.AliceWilling.LeanBackPantiesOff".Localized());

                    Player.Orgasm("Vaginal");
                }

            OutputText("Scenes.Alicescene.AliceWilling.ContentedThrillFadesDrained".Localized());

            AliceCorruption(15, true, Player.HasCock());

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceStop()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AliceChats, CoC.Instance.Flags.Get(GameFlagIds.AliceChats) | 1);

            Save.SetBool("knowInnocent", true);

            OutputText("Scenes.Alicescene.AliceStop.PushFeetAlarminglyAware".Localized());

            OutputText("Scenes.Alicescene.AliceStop.NervousLittleSuccubusWrong".Localized());

            OutputText("Scenes.Alicescene.AliceStop.ShakeNoMoreTricks".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryAlice);

            currentAlice.OnDefeated = _ => AliceWin();

            currentAlice.OnWon = _ => AliceLoss();

            CombatScene.Start(currentAlice, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void AliceLoss()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceLoss.EyesLightPowerlessBody".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.WeaklyPullUpShoves".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.PullsAwayExcitementObvious".Localized());

            if (Player.Gender == 0)
            {
                OutputText("Scenes.Alicescene.AliceLoss.FeaturelessGroinGrimaces".Localized());

                AliceCorruption(15, true, true);

                CombatScene.CleanupAfterCombat();

                return;
            }
            else
                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Alicescene.AliceLoss.CockAndVagina".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Alicescene.AliceLoss.Cock".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Alicescene.AliceLoss.Vagina".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.ChucklesLightlyGivesYour".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Alicescene.AliceLoss.CockBare".Localized());
            else
                OutputText("Scenes.Alicescene.AliceLoss.VaginaBare".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.HardFlickStingingShivers".Localized());

            if (Player.HasCock())
            {
                if (Player.BiggestCockLength() < 6)
                {
                    OutputText("Scenes.Alicescene.AliceLoss.SmileTremblesContainedLaughter".Localized());

                    OutputText("Scenes.Alicescene.AliceLoss.DeclaresSmirkNotSitting".Localized());
                }
                else
                    OutputText("Scenes.Alicescene.AliceLoss.PlantsLightKissTip".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Alicescene.AliceLoss.AdorableLipsSlickExcitement".Localized((Player.IsChild() ? "A fitting partner for once, don't you think?" : "Does it feel wrong, or is this just more exciting for you?")));

                    OutputText("Scenes.Alicescene.AliceLoss.PantiesComeOffSlowly".Localized());
                }

            OutputText("Scenes.Alicescene.AliceLoss.PointedSoftObjectPoking".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Alicescene.AliceLoss.CockPoking".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Alicescene.AliceLoss.VaginaPoking".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.PresumablyTailGoodBoy".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Alicescene.AliceLoss.CockGoodBoy".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Alicescene.AliceLoss.VaginaGoodBoy".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.FirmTailRubbingPerformance".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.DontLoseFocusBoy".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Alicescene.AliceLoss.WrappingEmbrace".Localized());
            else
                OutputText("Scenes.Alicescene.AliceLoss.ExploratoryProbing".Localized());

            OutputText("Scenes.Alicescene.AliceLoss.EroticAuraWorkedUp".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Alicescene.AliceLoss.UpOwnCockSpasm".Localized());

                Player.Orgasm("Dick");
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Alicescene.AliceLoss.DroolingLipsWarmFluid".Localized());

                    Player.Orgasm("Vaginal");
                }

            OutputText("Scenes.Alicescene.AliceLoss.SuccubusMusclesTenseVice".Localized());

            AliceCorruption(0, true, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceWin()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Alicescene.AliceWin.BeatenBatteredFallsGround".Localized());
            else
                OutputText("Scenes.Alicescene.AliceWin.ShakingAnxiousRubsFrantically".Localized());

            var needDraft = CombatController.Instance!.Context.Enemy.Lust < CombatController.Instance!.Context.Enemy.MaxLust;

            ClearButtons();

            AddNextButton("Common.Button.Panties".Localized(), AlicePanties);

            AddNextButton("Common.Button.Bind".Localized(), AliceBind);

            AddNextButton("Common.Button.Headpat".Localized(), AliceHeadpats);

            AddNextButton("Common.Button.Anal".Localized(), AliceAnal);

            AddNextButton("Common.Button.RoughSex".Localized(), AliceAsphyxiation);

            if (silly)
                AddNextButton("Common.Button.Pantiesplay".Localized(), AlicePanties2);
            // 原版 AliceScene.hx:651 — Shouldra 是跟随者时才出现「Shouldra」选项
            if (SceneInstance<Npcs.ShouldraFollowerScene>.Get().FollowerShouldra())
                AddNextButton("Scenes.Options.Setting.Shouldra".Localized(), ShouldraPossession);

            AddNextButton("Common.Button.LustFuck".Localized(), () => AliceLustFuck(needDraft));

            AddNextButton("Common.Button.Kill".Localized(), AliceKill);

            if (OptionStore.Instance.Get("gore", 0) > 0)
            {
                AddNextButton("Common.Button.SnuffSex".Localized(), AliceSnuff);

                AddNextButton("Common.Button.WombThroat".Localized(), AliceWombDeepthroat);
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), AliceLeave);
        }

        public void AlicePanties()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePanties.StridingTellNotLeaving".Localized());

            OutputText("Scenes.Alicescene.AlicePanties.HandsSlideStockingsUp".Localized());

            OutputText("Scenes.Alicescene.AlicePanties.EnoughForeplayPullDown".Localized());

            if (Player.Armor.LongName != "nothing" && Player.LowerGarmentName == "nothing")
                OutputText("Scenes.Alicescene.AlicePanties.RemovalArmorNameFollows".Localized());
            else
                if (Player.Armor.LongName == "nothing" && Player.LowerGarmentName != "nothing")
                    OutputText("Scenes.Alicescene.AlicePanties.RemovalLowergarmentFollow".Localized());
                else
                    if (Player.Armor.LongName != "nothing" && Player.LowerGarmentName != "nothing")
                        OutputText("Scenes.Alicescene.AlicePanties.RemovalArmorAndLowergarment".Localized());

            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Alicescene.AlicePanties.HoldPantiesInhalePheromones".Localized());

                Player.Orgasm("Vaginal");
            }
            else
            {
                OutputText("Scenes.Alicescene.AlicePanties.PullCockUpPresenting".Localized(
                    panties
                ));

                Player.Orgasm("Dick");
            }

            OutputText("Scenes.Alicescene.AlicePanties.MaybeTinyMorselRemark".Localized());

            if (Player.Armor.LongName != "nothing" || Player.LowerGarmentName != "nothing")
                OutputText("Scenes.Alicescene.AlicePanties.ReDress".Localized());

            OutputText("Scenes.Alicescene.AlicePanties.WanderBackHome".Localized());

            AliceCorruption(0, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceLeave.SighingSightDemonChild".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AliceKill()
        {
            ClearOutput();

            if (Player.Weapon.IsUnarmed())
                OutputText("Scenes.Alicescene.AliceKill.RaiseWeaponPurgePest".Localized());
            else
                if (Player.Weapon.IsFist())
                    OutputText("Scenes.Alicescene.AliceKill.SecondSubmissionRaise".Localized());
                else
                    OutputText("Scenes.Alicescene.AliceKill.ThirdGripWeapon".Localized());

            if (Player.Weapon.IsScythe())
                OutputText("Scenes.Alicescene.AliceKill.HoldingOverShoulderReaping".Localized());
            else
                if (Player.Weapon.IsHolySword())
                    OutputText("Scenes.Alicescene.AliceKill.RighteousEnergyCommandRise".Localized());
                else
                    if (Player.Weapon.Id == ItemIds.FlintlockPistol)
                        OutputText("Scenes.Alicescene.AliceKill.BendingPointBlankShot".Localized());
                    else
                        if (Player.Weapon.Id == WeaponIds.Blunderbuss)
                            OutputText("Scenes.Alicescene.AliceKill.MovingForwardPointBlank".Localized());
                        else
                            if (Player.Weapon.Id.Path.Contains("gauntlet"))
                                OutputText("Scenes.Alicescene.AliceKill.GazingFormMortalStrike".Localized());
                            else
                                if (Player.Weapon.IsStaff() && Player.Weapon.IsChanneling(Player))
                                    OutputText("Scenes.Alicescene.AliceKill.ChantingMagicFatalStrike".Localized());
                                else
                                    if (Player.Weapon.IsBlunt())
                                        OutputText("Scenes.Alicescene.AliceKill.RaisingAboveGravitySwing".Localized());
                                    else
                                        if (Player.Weapon.IsAxe())
                                            OutputText("Scenes.Alicescene.AliceKill.PreparingToExecuteChoose".Localized());
                                        else
                                            if (Player.Weapon.IsWhip())
                                                OutputText("Scenes.Alicescene.AliceKill.FlickingLoudSnapAttention".Localized());
                                            else
                                                if (Player.Weapon.IsKnife())
                                                    OutputText("Scenes.Alicescene.AliceKill.StridingAlongSideStraddle".Localized());
                                                else
                                                    if (Player.Weapon.IsSpear())
                                                        OutputText("Scenes.Alicescene.AliceKill.ChargingForwardSteadyForce".Localized());
                                                    else
                                                        if (Player.Weapon.IsBladed())
                                                            OutputText("Scenes.Alicescene.AliceKill.PushingUpOntoKnees".Localized());
                                                        else
                                                            if (Player.Weapon.IsFist())
                                                                OutputText("Scenes.Alicescene.AliceKill.CrackingKnucklesTwist".Localized());
                                                            else
                                                                OutputText("Scenes.Alicescene.AliceKill.SwingingIntoTempleEnd".Localized());

            AliceCorruption(15, false, false, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceBind()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceBind.LittleDemonSnapsAttention".Localized());

            OutputText("Scenes.Alicescene.AliceBind.HoldWristsBindQuick".Localized((Player.Body.Get<TailPart>().Id == BodyPartTypes.SpiderAbdomen ? "Quickly producing some silk from your spinnerets" : "Retrieving a length of string from what previously was the tie to a pouch")));

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), () => AliceBindLeave(false));

            AddButton(1, "Common.Button.Lewd".Localized(), AliceBindLewd);
        }

        public void AliceBindLeave(bool afterLewds)
        {
            ClearOutput();

            if (afterLewds)
                OutputText("Scenes.Alicescene.AliceBindLeave.WhyRewardDemonVictor".Localized());
            else
                OutputText("Scenes.Alicescene.AliceBindLeave.AfterAllSatisfiedPat".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AliceBindLewd()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceBindLewd.FunAdmireOtherIdeas".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewd.DesperateGiveWhatWants".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewd.ReturningToCrotchThumb".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewd.YelpsFlushedCantHandle".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewd.TearyStareHairPulled".Localized());

            AliceCorruption(15);

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), () => AliceBindLeave(true));

            AddButton(1, "Common.Button.Continue".Localized(), AliceBindLewdMore);
        }

        public void AliceBindLewdMore()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceBindLewdMore.HowSayNoPathetic".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.More".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.ExcuseYou".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.BegsPleaseMoreMaster".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.FailureAsDemonTrying".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.NervouslyStumblesHelplessWhore".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.StretchingNeckMockThought".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.RetrieveFingersChillAir".Localized());

            OutputText("Scenes.Alicescene.AliceBindLewdMore.UntieWristsAnklesLeave".Localized());

            AliceCorruption(15, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceAnal()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceAnal.EyingSlenderLegsBottom".Localized());

            OutputText("Scenes.Alicescene.AliceAnal.SituateBehindPeersSheepishly".Localized());

            OutputText("Scenes.Alicescene.AliceAnal.MomentToEnjoySight".Localized());

            OutputText("Scenes.Alicescene.AliceAnal.WontCruelDryTongue".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Deeper".Localized(), AliceAnalLick);

            AddButton(1, "Common.Button.Fuck".Localized(), AliceAnalFuck);
        }

        public void AliceAnalLick()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceAnalLick.StretchingFingersMassageDive".Localized());

            OutputText("Scenes.Alicescene.AliceAnalLick.WarmSqueezableTushPillow".Localized());

            DoNext(() => AliceAnalFuck());
        }

        public void AliceAnalFuck()
        {
            ClearOutput();

            var x = 1 + Player.CockThatFits(currentAlice.AnalCapacity());

            OutputText("Scenes.Alicescene.AliceAnalFuck.EnoughForeplayNeedsPressing".Localized());

            if (x <= 0)
            {
                OutputText("Scenes.Alicescene.AliceAnalFuck.UnleashCockButtSlap".Localized());

                OutputText("Scenes.Alicescene.AliceAnalFuck.SighingPushMeatCheeks".Localized());

                OutputText("Scenes.Alicescene.AliceAnalFuck.PickUpStretchingUnwind".Localized());
            }
            else
            {
                OutputText("Scenes.Alicescene.AliceAnalFuck.UnleashCockPlaceholderButt".Localized(
                    x
                ));

                OutputText("Scenes.Alicescene.AliceAnalFuck.WriggleTipSalivaSlicked".Localized());

                OutputText("Scenes.Alicescene.AliceAnalFuck.LoudGroanSlamsHilting".Localized());

                OutputText("Scenes.Alicescene.AliceAnalFuck.More".Localized());

                OutputText("Scenes.Alicescene.AliceAnalFuck.HardlyBreatheLikesRough".Localized());

                OutputText("Scenes.Alicescene.AliceAnalFuck.SnapBackRealityToy".Localized());
            }

            Player.Orgasm("Dick");

            AliceCorruption(0, true, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceFoxIntro()
        {
            ClearOutput();

            Save.SetBool("foxWatched", false);

            OutputText("Scenes.Alicescene.AliceFoxIntro.WanderingBrushFamiliarAura".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Continue".Localized(), AliceFoxContinue);

            AddButton(1, "Common.Button.Leave".Localized(), () => AliceFoxLeave(0));
        }

        public void AliceFoxContinue()
        {

            Save.SetBool("foxSeen", true);

            OutputText("Scenes.Alicescene.AliceFoxContinue.PushingThicketFollowSenses".Localized());

            OutputText("Scenes.Alicescene.AliceFoxContinue.EasyWontHurtRelaxed".Localized());

            OutputText("Scenes.Alicescene.AliceFoxContinue.HesitantlyKneelsOutstretched".Localized());

            OutputText("Scenes.Alicescene.AliceFoxContinue.FollowingComfortFoxRolls".Localized());

            OutputText("Scenes.Alicescene.AliceFoxContinue.GulpsFumblesWarmSlick".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Interrupt".Localized(), AliceFoxInterrupt);

            AddButton(1, "Common.Button.Watch".Localized(), AliceFoxWatch);

            AddButton(2, "Common.Button.Arouse".Localized(), AliceFoxHelp);

            AddButton(3, "Common.Button.Leave".Localized(), () => AliceFoxLeave(1));
        }

        public void AliceFoxLeave(int variant)
        {
            ClearOutput();

            switch (variant)
            {
                case 0:
                    {
                        OutputText("Scenes.Alicescene.AliceFoxLeave.WhateverDemonNotInvolved".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Alicescene.AliceFoxLeave.QuiteSightNotInterested".Localized());

                        AliceCorruption(1);
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Alicescene.AliceFoxLeave.EroticallyChargedBackCamp".Localized());
                    }

                    break;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceFoxInterrupt()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceFoxInterrupt.SeenEnoughLeapBushes".Localized());

            OutputText("Scenes.Alicescene.AliceFoxInterrupt.StiffenedJumpsBeginsYell".Localized());

            OutputText("Scenes.Alicescene.AliceFoxInterrupt.FinishesDesperateTripsFalls".Localized());

            OutputText("Scenes.Alicescene.AliceFoxInterrupt.StrugglesFeetBrushesDress".Localized());

            OutputText("Scenes.Alicescene.AliceFoxInterrupt.WatchSprintOffDeeper".Localized());

            AliceCorruption(1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceFoxWatch()
        {
            ClearOutput();

            Save.SetBool("foxWatched", true);

            OutputText("Scenes.Alicescene.AliceFoxWatch.SceneInterestAnticipation".Localized());

            OutputText("Scenes.Alicescene.AliceFoxWatch.HoldsTongueTipCock".Localized());

            OutputText("Scenes.Alicescene.AliceFoxWatch.TastesWeirdIrony".Localized());

            OutputText("Scenes.Alicescene.AliceFoxWatch.ReassuringBreathDivesAgain".Localized());

            OutputText("Scenes.Alicescene.AliceFoxWatch.FuzzyOrangeFoxYips".Localized());

            OutputText("Scenes.Alicescene.AliceFoxWatch.ExcitedYippingBreaksTrance".Localized());

            OutputText("Scenes.Alicescene.AliceFoxWatch.CombinationAuraEroticShow".Localized());

            AliceCorruption(15);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceFoxHelp()
        {
            ClearOutput();

            Save.SetBool("foxWatched", true);

            Save.SetBool("foxNeedArouse", false);

            OutputText("Scenes.Alicescene.AliceFoxHelp.AsShowLittlePush".Localized());

            OutputText("Scenes.Alicescene.AliceFoxHelp.ObliviousNervouslyBringsFace".Localized());

            OutputText("Scenes.Alicescene.AliceFoxHelp.DistressedPullsMouthAway".Localized());

            OutputText("Scenes.Alicescene.AliceFoxHelp.MurmursSeductionWarmWet".Localized());

            OutputText("Scenes.Alicescene.AliceFoxHelp.FoxNoCoercionLicks".Localized());

            OutputText("Scenes.Alicescene.AliceFoxHelp.GaspsLoudlyHitMark".Localized());

            OutputText("Scenes.Alicescene.AliceFoxHelp.SquelchingWetNoisesMusic".Localized());

            AliceCorruption(15);

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), () => AliceFoxLeave(2));

            if (silly)
                AddButton(1, "Common.Button.JumpOut".Localized(), AliceFoxJump);
        }

        public void AliceFoxJump()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceFoxJump.AfterDisplayMustIntroduce".Localized());

            OutputText("Scenes.Alicescene.AliceFoxJump.OneMoreStepDashes".Localized());

            OutputText("Scenes.Alicescene.AliceFoxJump.DamnCelebritiesCantTime".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AlicePanties2()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePanties2.ExpressionShiftsDrawCloser".Localized(
                (Player.HasCock() ? "[cock] strains against the confines of your [armor], begging for release" : Player.HasVagina() ? "[vagina] soaks the inside of your [armor], begging for release" : "featureless groin tingles, begging for an impossible release")
            ));

            OutputText("Scenes.Alicescene.AlicePanties2.StandingOverClearsThroat".Localized());

            OutputText("Scenes.Alicescene.AlicePanties2.SurpriseConfusionUncertainty".Localized());

            OutputText("Scenes.Alicescene.AlicePanties2.FinallyAsksMyPanties".Localized((panties == "black" ? "black, not-appropriate-for-children" : "pristine, childish")));

            OutputText("Scenes.Alicescene.AlicePanties2.SlimFingersUnderFabric".Localized((panties == "black" ? "sexy" : "cute")));

            OutputText("Scenes.Alicescene.AlicePanties2.AfterEternityTemptingTeasing".Localized());

            OutputText("Scenes.Alicescene.AlicePanties2.ThisIsInchesAway".Localized());

            AliceCorruption(5);

            ClearButtons();

            AddButton(0, "Common.Button.Sniff".Localized(), AlicePantiesSniff);

            AddButton(1, "Common.Button.Fold".Localized(), AlicePantiesFold);
        }

        public void AlicePantiesSniff()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePantiesSniff.AsksInnocentWhatNow".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.ChokeUpHappinessScent".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.YouWantMore".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.EarsFaintlyRegisterConcerned".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.RealizeEnlightenmentSimple".Localized(
                (Player.HasCock() ? "your dick threatening to [if (isnakedlower) {burst asunder|tear your [armor] to pieces}]" : Player.HasVagina() ? "your cunt more akin to a waterfall" : "your blank crotch feeling left out")
            ));

            OutputText("Scenes.Alicescene.AlicePantiesSniff.SpendTimeRubbingSilky".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.ShudderLoomingOrgasmReverie".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.LookingCrumpledPantiesIdea".Localized((Player.IsChild() ? "It hasn't been that long, but you never paid that much attention. You now regret that." : "It has been a while. You wish you could recall it, but the memory is hazy. You'll have to wing it.")));

            OutputText("Scenes.Alicescene.AlicePantiesSniff.TurningAboutInspectClosely".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.WellGoesOverNo".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.HotBreathPantyBow".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.FirstHourLookDown".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.HitsMidAirThr".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.LeanedHandsFirmShudder".Localized(
                (Player.HasCock() ? "Your [cock], [if (isnakedlower) {unbound by cloth and armor|pitifully trapped inside your [armor]}] as it is, sprays its sticky load [if (isnakedlower) {onto the demoness below, staining her with the seed she was so desperate for|into its jail, quickly staining your gear with semen, some leaking out onto the demoness below}]." : Player.HasVagina() ? "Your [vagina], untouched this entire time, contracts [if (isnakedlower) {and dampens your thighs with its feminine juices, some dribbling onto the demoness below|inside your [armor] and stains your gear with its feminine juices}]." : "Mentally, of course; the desolate expanse between your legs only releases a stream of tears, soaking your [armor].")
            ));

            OutputText("Scenes.Alicescene.AlicePantiesSniff.HotBreathBreaksStupor".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.BreatheDeepKissCheek".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesSniff.NodYourselfWanderBack".Localized());

            Player.Orgasm("all");

            AliceCorruption(0, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AlicePantiesFold()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePantiesFold.AsksInnocentWhatNow".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.SomethingMotherUsedDo".Localized((Player.IsChild() ? "It hasn't been that long, but you never paid that much attention. You now regret that." : "It has been a while. You wish you could recall it, but the memory is hazy. You'll have to wing it.")));

            OutputText("Scenes.Alicescene.AlicePantiesFold.TurningAboutInspectClosely".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.WellProbablyOverThen".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.TriumphantlyHoldDoesntLook".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.LookDownUncertaintyWritten".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.KneelBetweenLegsHand".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.PurrsMasturbatingCouldTold".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.PerfectAdorableLivingDoll".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.DawnsOnDoneStops".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.DecidingEmbarrassingLastLook".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.CallsWhyIgnoreChuckle".Localized());

            OutputText("Scenes.Alicescene.AlicePantiesFold.Heeey".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            CombatScene.CleanupAfterCombat();
        }

        public void AliceHeadpats()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpats.CrackingKnucklesStrideFallen".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpats.LamentsSinkingTrepidation".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpats.EyesTightPromiseLeave".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpats.StopInchesFromDemon".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpats.PinchFallenLeafHair".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpats.ThumbGrazesSharpHorns".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpats.SighsTurnHalfMoans".Localized());

            AliceCorruption(15);

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), AliceHeadpatsLeave);

            AddButton(1, "Common.Button.JustPat".Localized(), AliceHeadpatsPats);

            AddButton(2, "Common.Button.Lewd".Localized(), AliceHeadpatsLewd);

            AddButton(3, "Common.Button.Kill".Localized(), AliceHeadpattedTooHard);
        }

        public void AliceHeadpatsLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpatsLeave.WaryPassiveInfluencePat".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsLeave.SighsSatisfactionLooksUp".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsLeave.CouldntSoundSadMind".Localized());

            AliceCorruption(5);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceHeadpatsPats()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.NotHereGetOff".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.FingersLikeLivingComb".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.QuestionLostDrawnSigh".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.UtterlyLostInRapture".Localized((Player.IsChild() || Player.IsTeen() ? "little sister" : "daughter")));

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.TicklishLaughterShakeOut".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.HandsStillOnlyCaressing".Localized(
                Player.ClothedOrNaked("You're still fully clothed. So is she.", "She's still fully clothed.")
            ));

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.RufflingFinalTimeGrumble".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.AsksDisappointmentPalpable".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.SighsTooExhausted".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsPats.BidFarewellMoreFlushed".Localized());

            AliceCorruption(20);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceHeadpatsLewd()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpatsLewd.GlideOverHornsMoan".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsLewd.SmallVoiceWarnsDangers".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsLewd.YouWantHer".Localized());

            AliceCorruption(20);

            ClearButtons();

            AddButton(0, "Common.Button.Finger".Localized(), AliceHeadpatsFinger);

            AddButton(1, "Common.Button.Fuck".Localized(), AliceHeadpatsFuck);

            AddButton(2, "Common.Button.Trib".Localized(), AliceHeadpatsTrib);
        }

        public void AliceHeadpatsFuck()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpatsFuck.PushRoughBarkLegs".Localized(
                Player.ClothedOrNaked("Your [armor] is quickly stripped off and thrown aside, baring your [cock] to the fresh air.", "Already naked and raring to go, your [cock] stands at attention as you descend upon her.")
            ));

            OutputText("Scenes.Alicescene.AliceHeadpatsFuck.WallsConstrictRapturous".Localized((Player.IsChild() ? "another" : "a")));

            OutputText("Scenes.Alicescene.AliceHeadpatsFuck.OneHipsOtherHead".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsFuck.LustMadnessHarderFaster".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsFuck.DizzyExtractPassedOut".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(0, true, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceHeadpatsTrib()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpatsTrib.PushSmotheringTinyBody".Localized(
                Player.ClothedOrNaked(" as you start fiddling with your [armor], removing the offending piece until you are left bare to the elements")
            ));

            OutputText("Scenes.Alicescene.AliceHeadpatsTrib.GrabbingHipsAnglingVagina".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsTrib.RavagedNakedSalivatingMind".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsTrib.LostRaptureHugTiny".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsTrib.OvercomeDizzinessSlumpTree".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsTrib.CatchBreathFinalDressed".Localized());

            Player.Orgasm("Vaginal");

            AliceCorruption(0, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceHeadpatsFinger()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpatsFinger.PushRoughBarkStraddle".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsFinger.BreakLipsPumpFinger".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsFinger.RemainLikeReignLibido".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpatsFinger.SighingLeaveToSleep".Localized());

            AliceCorruption(30, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceHeadpattedTooHard()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceHeadpattedTooHard.HookThumbAroundHorn".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpattedTooHard.RestSentenceLoudCrack".Localized());

            OutputText("Scenes.Alicescene.AliceHeadpattedTooHard.GetUpBrushHands".Localized());

            AliceCorruption(5, false, false, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceTailfuck()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceTailfuck.TellWouldLikeKnowing".Localized((Player.IsChild() ? " But just so you know, I may LOOK as young as you, but I have a lot more experience. So be a good little [boy] and try to keep up for me. And don't say I was a bad influence on you. You did ask for it, after all." : ""), (Player.IsChild() ? " [say: Though I suppose for you it's probably just more natural this way.]" : "")));

            OutputText("Scenes.Alicescene.AliceTailfuck.BothDisrobedStepClose".Localized((Player.HasVagina() ? "wet [vagina]" : "hard [cock]"), (Player.HasVagina() ? "your [clit] is pressed against hers" : "the head of your cock is pressed against her clit"), (Player.HasVagina() ? "own" : "shaft"), (Player.HasVagina() ? "own throbbing button against hers" : "cock against her throbbing button")));

            OutputText("Scenes.Alicescene.AliceTailfuck.CirclingTailTeaseEntrance".Localized((Player.IsChild() ? "[say: Come on little [boy], fuck me harder! Harder, harder! I don't know where the hell you learned to do this at your age, but just go harder!]" : "[say: Come on big [boy], fuck me! Pound me, give me all you've got, stretch my tiny pussy until you break me!]")));

            OutputText("Scenes.Alicescene.AliceTailfuck.PlannedGrindStretch".Localized((Player.HasVagina() ? "your clits together" : "against her clit"), (Player.HasVagina() ? "your pussy to spasm wildly while gushing your girlcum all over her crotch as well as your own tail" + (Player.HasCock() ? ", your member spraying all over her stomach, chest, and face." : "") : "your member to spray cum over her stomach, chest, and face")));

            OutputText("Scenes.Alicescene.AliceTailfuck.OrgasmsWindRemoveTail".Localized((Player.IsChild() ? "[say: Well of course you were. My body only looks the same age as yours, I'm really much older and know how to use it. But for someone your age... that was better than I expected.]" : "[say: Really?!] She beams in delight at you, seeming very excited by your compliment. [say: I mean, you weren't too bad yourself. In fact, you were pretty damn good. If you're fine with me being like this, maybe you should stop by again sometime? People that see through my illusions usually just attack me immediately. It's nice for someone to uh... embrace me, for once.]")));

            Player.Orgasm("Vaginal");

            AliceCorruption(0, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceSnuff()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceSnuff.NoIntentionDemonCompromise".Localized());

            OutputText("Scenes.Alicescene.AliceSnuff.NoSpoilSurpriseDescend".Localized());

            OutputText("Scenes.Alicescene.AliceSnuff.OneThrustGaspsWaited".Localized());

            OutputText("Scenes.Alicescene.AliceSnuff.HandNeckGrippingFirmly".Localized());

            OutputText("Scenes.Alicescene.AliceSnuff.OrgasmBothHandsSore".Localized());

            OutputText("Scenes.Alicescene.AliceSnuff.StayCatchingBreathTwitching".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(0, true, true, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceLustFuck(bool needDraft = true)
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceLustFuck.IllusionsDiscreetlyMagic".Localized((needDraft ? "She should get a taste of her own medicine, shouldn't she? You pull a draft of frothy liquid-lust from your pack and pop it open. Grabbing her [alicehair] hair, you thrust the mouth of the bottle to her lips and make it clear that she must drink it. The Alice winces and opens up, giving no resistance as you tilt the vial." : "")));

            if (needDraft)
                OutputText("Scenes.Alicescene.AliceLustFuck.ShiversShuddersPotionEffect".Localized());

            OutputText("Scenes.Alicescene.AliceLustFuck.NowHerDrivenInsane".Localized((Player.IsNaked() ? "" : ", and make haste to unveil your manhood"), (silly ? " away from her snatch" : ""), (Player.LongestCockLength() < 5 ? "She must be extremely desperate. " : "")));

            OutputText("Scenes.Alicescene.AliceLustFuck.GreatestEnthusiasmPosition".Localized());

            OutputText("Scenes.Alicescene.AliceLustFuck.MindBrokenLustMagnitude".Localized((silly && needDraft ? "Glancing at the torn panties and heart-shaped pupils, you look to the empty potion bottle and ponder if science has gone too far. " : "")));

            OutputText("Scenes.Alicescene.AliceLustFuck.ExclaimsClumsilyEmbrace".Localized((silly ? " V.I.P. room" : "")));

            OutputText("Scenes.Alicescene.AliceLustFuck.ConsumedWithLustMutterings".Localized());

            if (needDraft)
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                    Player.DestroyItems(ConsumableIds.LustDraft, 1);
                else
                    Player.DestroyItems(ConsumableIds.FuckDraft, 1);

            ClearButtons();

            AddNextButton("Common.Button.TuneItOut".Localized(), () => AliceLustFuck2(false));

            AddNextButton("Common.Button.PayAttention".Localized(), () => AliceLustFuck2(true));
        }

        public void AliceLustFuck2(bool choice)
        {
            ClearOutput();

            if (choice)
                OutputText("Scenes.Alicescene.AliceLustFuck2.ScreamsGleeExtensionCock".Localized());

            OutputText("Scenes.Alicescene.AliceLustFuck2.PumpAwayBlabberingMulling".Localized());

            OutputText("Scenes.Alicescene.AliceLustFuck2.CriesSuccubusInHeat".Localized((choice ? "The Alice deliriously spouts various sounds until soon speaking somewhat clearly again. [say: Bruise my cervix! Even if I pass out from cumming too much, don't stop until you're satisfied!]" : "")));

            OutputText("Scenes.Alicescene.AliceLustFuck2.ViceGripCockTree".Localized());

            OutputText("Scenes.Alicescene.AliceLustFuck2.LollsTongueCurlingToes".Localized((choice ? " [say: Rape my uterus with semen!] she begs. [say: Turn my womb into a distended balloon full of your splurty-splurt juices!]" : ""), (silly ? "Rule of threes, you suppose." : "")));

            OutputText("Scenes.Alicescene.AliceLustFuck2.WetSlappingScentSex".Localized((choice ? "The Alice deliriously spouts various sounds until soon speaking somewhat clearly again. [say: Bruise my cervix! Even if I pass out from cumming too much, don't stop until you're satisfied!]" : "")));

            AliceCorruption(15, true, true);

            Player.Orgasm("Dick");

            DoNext(() => AliceLustFuck3());
        }

        public void AliceLustFuck3()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceLustFuck3.WithdrawFallBackSpinning".Localized());

            OutputText("Scenes.Alicescene.AliceLustFuck3.CollectThingsStumbleOff".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AliceWombDeepthroat()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.HungerWellsVulgarSeductress".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.DarkGlintSignalsSomething".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.EyesFlashShiftTorso".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.PleaseAnythingNotThis".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.WonderfulCadenceIncision".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.YoungGroansHandsInside".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.FinallyHoldUnderdevelopedWomb".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.VoiceGivesOutBlood".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.NotSenselessDestination".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.PathClearedOrganThroat".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.FeeblyGaspSeal".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.SummonsStrengthFinalWail".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.EventuallyComeDownSoften".Localized());

            OutputText("Scenes.Alicescene.AliceWombDeepthroat.OneLookCoolingCorpse".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(15, true, true, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceAsphyxiation()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceAsphyxiation.CompletelyAtMercyConsider".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.DoesNothingQuietMoan".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.CriesRelentAnything".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.YoungBaredFlatChest".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.PushBackwardWrenchThighs".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.WhimpersWaitOuch".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.DontHearHandsNeck".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.AlignImmatureSlit".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.SeemsTakeWellIntended".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.TwinTracksTearsBeautiful".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.NoMatterPainfulBody".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.SeedSpillsFilling".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.StrengthRunsCollapseSuck".Localized());

            OutputText("Scenes.Alicescene.AliceAsphyxiation.QuicklyGetThingsLeave".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(15, true, true);

            CombatScene.CleanupAfterCombat();
        }

        public void AliceNursing()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceNursing.WantsFeedLikeSuccubus".Localized());

            OutputText("Scenes.Alicescene.AliceNursing.SmallNotReallyChild".Localized());

            OutputText("Scenes.Alicescene.AliceNursing.SaysAwkwardnessWalksSits".Localized(
                Player.Mf("Or daddy, in your case. The Alice coughs suddenly. [say: Please don't, this gets a lot weirder if you put it that way.] ", ""),

                Player.Mf(", setting her back at ease", "")
            ));

            OutputText("Scenes.Alicescene.AliceNursing.HugsTighterAcrossLap".Localized(
                silly ? Player.Mf("The demoness groans loudly. [say: I told you <b>please</b>, that sounds so weird!] Her vocal dread is amusing, but you'll lay off, and instead see to her needs", "So be it, you'll quench her needs") : "So be it, you'll quench her needs"
            ));

            OutputText("Scenes.Alicescene.AliceNursing.SlipFingerPantiesWriggle".Localized());

            OutputText("Scenes.Alicescene.AliceNursing.PickPaceSlickTunnel".Localized());

            OutputText("Scenes.Alicescene.AliceNursing.SpoutsDeliriouslyNoMommy".Localized());

            OutputText("Scenes.Alicescene.AliceNursing.RemarksBetweenBreathsEase".Localized());

            Player.Milked();

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceIntimate()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimate.TellDontPlanHurting".Localized());

            OutputText("Scenes.Alicescene.AliceIntimate.SuddenDemonicFeaturesVisible".Localized());

            ClearButtons();

            AddNextButton("Common.Button.FuckHer".Localized(), AliceIntimateFuck);

            AddNextButton("Common.Button.Fingering".Localized(), AliceIntimateFingering);

            AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), AliceIntimateTribbing);

            if (OptionStore.Instance.Get("watersports", 0) > 0)
                AddNextButton("Common.Button.Watersports".Localized(), AliceIntimateWatersports);
        }

        public void AliceIntimateFuck()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateFuck.DoWhatMakesThink".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.AsksSuspiciouslyNotSaying".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.AnythingGrasp".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.NoneBetterTearDress".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.NoHesitationMoveNipples".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.LooksSortsLookEyes".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.MoveLowerPullingDress".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.SmoothSlitInnocent".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.LyingOnSideCradled".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.VoiceStartlesShiveringChill".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.AnswerDemonstratingShaftInto".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.NeedNoInvitationRhythm".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.WantGoodEarlierIntent".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.KeepTaskRubbingPumping".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.WantingBlissSpeedUp".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.CockStartsUnloadJolt".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.HeavyBreathsEmptyAir".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFuck.ExcessiveExhaustionNothingLeft".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(15, true, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceIntimateFingering()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateFingering.CuriousDoesntLeanClose".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.SmilesRestsNotThing".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.FewMomentsBlushingFrustration".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.WinksLikeTakeMe".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.BeforeNeedsMoreTugs".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.MoansDelightTooLong".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.MouthChestStrokingHair".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateFingering.HoldingSleepingMinutesStarts".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceIntimateTribbing()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateTribbing.SmilingHasntRealizedForehead".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateTribbing.AsAdorableIdeaPops".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateTribbing.WithoutWarningPullsKiss".Localized());

            DoNext(() => AliceIntimateTribbing2());
        }

        public void AliceIntimateTribbing2()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateTribbing2.SpeaksYouthfulExcitementMe".Localized(
                Player.BreastDescript(1)
            ));

            OutputText("Scenes.Alicescene.AliceIntimateTribbing2.MoanTogetherThrustFaster".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => AliceIntimateTribbing3());
        }

        public void AliceIntimateTribbing3()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateTribbing3.OrgasmsSubsideGrowStill".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateTribbing3.BackingOffHopeToo".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceIntimateWatersports()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateWatersports.LeanSeductiveSmirkActually".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateWatersports.BreaksLustfullyFuckYes".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateWatersports.ThinkOverUncommonFetish".Localized());

            DoNext(() => AliceIntimateWatersports2());
        }

        public void AliceIntimateWatersports2()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceIntimateWatersports2.EnjoyScentMakesMove".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateWatersports2.ShiftsGivingWhatWant".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateWatersports2.CoveredGirlGetStarted".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateWatersports2.IntenselyArousedCoveredUrine".Localized());

            OutputText("Scenes.Alicescene.AliceIntimateWatersports2.OnceSatisfiedMovesOff".Localized());

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void AlicePatMenu()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePatMenu.TellNoPlansHarm".Localized());

            OutputText("Scenes.Alicescene.AlicePatMenu.DiscardsIllusionRevealingWings".Localized());

            OutputText("Scenes.Alicescene.AlicePatMenu.LookingOverCuteGirlish".Localized());

            ClearButtons();

            AddNextButton("Common.Button.PatHer".Localized(), AlicePatHer);

            AddNextButton("Common.Button.GetPatted".Localized(), AliceGetPatted);

            AddNextButton("Common.Button.Patwithbenefits".Localized(), AlicePatHerLewd);

            AddNextButton("Common.Button.GetLewdpats".Localized(), AliceGetPattedLewd);
        }

        public void AlicePatHer()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePatHer.DelightedHandWalkTree".Localized());

            OutputText("Scenes.Alicescene.AlicePatHer.ConfessionOfEnjoyment".Localized());

            OutputText("Scenes.Alicescene.AlicePatHer.ComfortableSituationPetHair".Localized());

            OutputText("Scenes.Alicescene.AlicePatHer.TimePassesAwareAura".Localized());

            AliceCorruption();

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceGetPatted()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceGetPatted.DelightedHandWalkTree".Localized());

            OutputText("Scenes.Alicescene.AliceGetPatted.TellWantHeadpatsAgreed".Localized());

            OutputText("Scenes.Alicescene.AliceGetPatted.UnexpectedWaySpendTime".Localized());

            OutputText("Scenes.Alicescene.AliceGetPatted.EyesOpenStirredSleepyhead".Localized());

            OutputText("Scenes.Alicescene.AliceGetPatted.WavesGoodbyeWasntBad".Localized());

            AliceCorruption();

            DoNext(() => ReturnToCamp(1));
        }

        public void AlicePatHerLewd()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AlicePatHerLewd.DelightedHandWalkTree".Localized());

            OutputText("Scenes.Alicescene.AlicePatHerLewd.VerySilkyMainEvent".Localized());

            OutputText("Scenes.Alicescene.AlicePatHerLewd.BetterGettingEachOther".Localized());

            OutputText("Scenes.Alicescene.AlicePatHerLewd.OutOpenBeforeLong".Localized());

            OutputText("Scenes.Alicescene.AlicePatHerLewd.StandsUpAdjustsUnexpected".Localized());

            OutputText("Scenes.Alicescene.AlicePatHerLewd.ExchangeGoodbyesReturnCamp".Localized());

            AliceCorruption(15, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceGetPattedLewd()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.DelightedHandWalkTree".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.TellWantHeadpatsAgreed".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.HHeyWhatDoing".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.IgnoringQuestionLowerLap".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.ResumesPattingHead".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.SimultaneouslyPressingFurther".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.DidntExpectHeadpats".Localized());

            OutputText("Scenes.Alicescene.AliceGetPattedLewd.ComeSometimeNotEasy".Localized());

            AliceCorruption(15, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAlice()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAlice.AboutGivenHopeWasteland".Localized());

            RegisterMacro("corrupt", () => (Player.Corruption >= 50 || Player.Lust == Player.MaxLust) ? "1" : "0");

            lewd = false && !Player.IsGenderless();

            Save.SetBool("rented", true);

            ClearButtons();

            AddNextButton("Common.Button.Look".Localized(), RentAnAliceLook);

            AddNextButton("Combat.Button.Approach".Localized(), () => RentAnAliceApproach(true));

            AddNextButton("Common.Button.Leave".Localized(), RentAnAliceLeave);
        }

        public void RentAnAliceLook()
        {

            OutputText("Scenes.Alicescene.RentAnAliceLook.NoChancesShufflePosition".Localized());
            // 原版 AliceScene.hx:1419 — allowChild 时额外涨 lust 10
            if (GameSettings.AllowChild)
                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            AddButton(0, "Common.Button.Look".Localized(), null);
        }

        public void RentAnAliceLeave()
        {

            OutputText("Scenes.Alicescene.RentAnAliceLeave.SeemsDoingJustFine".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceApproach(bool output = false)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Alicescene.RentAnAliceApproach.MoveCloserHarmlessFellow".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceApproach.WavesGlanceUpSpot".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceApproach.HelloMisterMakeOut".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceApproach.SmilesHoldsSackExpectantly".Localized());
            }
            // 原版 AliceScene.hx:1438 — allowChild 时额外涨 lust 10
            if (GameSettings.AllowChild)
                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddNextButton("Common.Button.Pay".Localized(), () => RentAnAlicePay());

            AddNextButton("Common.Button.TooMuch".Localized(), RentAnAliceTooMuch);

            AddNextButton("Common.Button.Question".Localized(), RentAnAliceQuestion);

            AddNextButton("Common.Button.Hug".Localized(), RentAnAliceHug);

            AddButton(14, "Common.Button.Leave".Localized(), () => RentAnAliceApproachLeave());
        }

        public void RentAnAliceApproachLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceApproachLeave.NoWonderBeggarThief".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceQuestion()
        {
            ClearOutput();

            if (lewd)
            {
                OutputText("Scenes.Alicescene.RentAnAliceQuestion.MustHaveMisheardTwenty".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceQuestion.GlancesAroundMe".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceQuestion.NodsAsIfConvince".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                ClearButtons();

                AddNextButton("Common.Button.Pay".Localized(), () => RentAnAlicePay());

                AddNextButton("Common.Button.Donate".Localized(), RentAnAliceDonate);

                AddNextButton("Common.Button.TooMuch".Localized(), RentAnAliceTooMuch);

                AddButton(14, "Common.Button.Leave".Localized(), () => RentAnAliceApproachLeave());
            }
            else
            {
                OutputText("Scenes.Alicescene.RentAnAliceQuestion.HugSeemsTame".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceQuestion.EyesMeetGetDown".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceQuestion.CouldntHurtNeedOne".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Pay".Localized(), () => RentAnAlicePay());

                AddNextButton("Common.Button.TooMuch".Localized(), RentAnAliceTooMuch);
            }
        }

        public void RentAnAliceDonate()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceDonate.IntoWaitingTwentyGems".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDonate.SmilesSlidesBackPlace".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDonate.WellYesNoNeed".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDonate.FewStraightensDetermination".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDonate.ThatsEnoughTurnAway".Localized());

            Player.Gems -= 20;

            Player.ApplyStats(new DynStat(GameStats.Corruption, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceHug()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceHug.TellOnlyWantHug".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceHug.ShakesHeadThatsIt".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceHug.EyesMeetTestingHug".Localized());

            lewd = false;

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddNextButton("Common.Button.Pay".Localized(), () => RentAnAlicePay());

            AddNextButton("Common.Button.TooMuch".Localized(), RentAnAliceTooMuch);

            AddNextButton("Common.Button.More".Localized(), RentAnAliceHugMore);

            AddButton(14, "Common.Button.NeverMind".Localized(), () => RentAnAliceHugBack());
        }

        public void RentAnAliceHugMore()
        {

            OutputText("Scenes.Alicescene.RentAnAliceHugMore.SmileUncomfortablyMind".Localized());

            lewd = true;

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => RentAnAlicePay());
        }

        public void RentAnAliceHugBack()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceHugBack.ShakeChangedMindSurprised".Localized());

            lewd = true;

            RentAnAliceApproach();
        }

        public void RentAnAliceTooMuch()
        {
            ClearOutput();

            if (lewd)
            {
                OutputText("Scenes.Alicescene.RentAnAliceTooMuch.WantedToDontHave".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceTooMuch.FiddlesHatSquirms".Localized());
            }
            else
            {
                OutputText("Scenes.Alicescene.RentAnAliceTooMuch.GemsCouldGetLot".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceTooMuch.LeansCloserBarelyBreeze".Localized());
            }
            // 原版 AliceScene.hx:1460 — allowChild 时额外涨 lust 10
            if (GameSettings.AllowChild)
                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddNextButton("Common.Button.Pay".Localized(), () => RentAnAlicePay(5));

            AddNextButton("Common.Button.TooMuch".Localized(), RentAnAliceTooMuchTwo);
        }

        public void RentAnAliceTooMuchTwo()
        {
            ClearOutput();

            if (lewd)
                OutputText("Scenes.Alicescene.RentAnAliceTooMuchTwo.DoesSheReallyThink".Localized());
            else
                OutputText("Scenes.Alicescene.RentAnAliceTooMuchTwo.GemsButNotYou".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchTwo.StaresRightThroughSigh".Localized((lewd ? "" : "[say: You need one even more than I thought...] ")));

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchTwo.HoldingUpOneFinger".Localized((lewd ? "Though you can't believe [if (gems < 5) {what you're hearing|that worked}], she cuts you off before you can take up her offer. " : "[if (gems < 5) {It's a strange feeling having a [if (ischild) {girl your age|little girl}] take pity on you, but|Though you can't believe that worked,}] she cuts you off before you can take up her offer.[pg]")));
            // 原版 AliceScene.hx:1494 — allowChild 时额外涨 lust 10
            if (GameSettings.AllowChild)
                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddNextButton("Common.Button.Agree".Localized(), RentAnAliceTooMuchAgree);

            AddNextButton("Common.Button.Leave".Localized(), RentAnAliceTooMuchLeave);
        }

        public void RentAnAliceTooMuchAgree()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchAgree.NodNotPlanningDiscount".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchAgree.NotEnoughHoldingHand".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchAgree.BeforeMouthSoftPalm".Localized((Player.HasSpells() ? "You're pretty sure it didn't do anything, but you've heard the tales of wizards accidentally making pacts they couldn't uphold." : "You don't think it did anything... but then again, what do you know about magic?")));
            // 原版 AliceScene.hx:allowChild 段落 — 额外涨 lust 10
            if (GameSettings.AllowChild)
                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            RentAnAlicePay(0);
        }

        public void RentAnAliceTooMuchLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchLeave.ShakeChangedMindWanted".Localized(
                lewd ? "to get off" : "a simple hug",

                lewd ? "It's not like there's any shortage of sex around here." : (FollowerRegistry.All.Count != 0 ? "You've got your followers to cheer you up, after all." : "How hard could it really be?")
            ));

            OutputText("Scenes.Alicescene.RentAnAliceTooMuchLeave.SaysAlreadyTurnedAway".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAlicePay(int price = 20)
        {
            ClearOutput();

            Player.Gems -= price;

            RegisterMacro("paid", price > 0);

            if (!lewd && Player.Lust == Player.MaxLust && false && !Player.IsGenderless())
            {
                OutputText("Scenes.Alicescene.RentAnAlicePay.BarelyBreatheSuffocatingHeat".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.HaveToOughtHorrified".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.LooksUmOverTallness".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.LeansIntoTouchCannot".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.OfferSpecialHugsToo".Localized());

                lewd = true;
            }

            if (lewd)
            {
                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                OutputText("Scenes.Alicescene.RentAnAlicePay.HandHerGems".Localized((price == 5 ? "feeling [if (cor >= 50) {rather smug|a little guilty}] about your discount" : "waiting impatiently")));

                if (Player.HasCock())
                {
                    OutputText("Scenes.Alicescene.RentAnAlicePay.TurnsLittleRedPositive".Localized((silly && Player.LongestCockLength() >= 12 ? "--or self-preservation--" : ", ")));

                    OutputText("Scenes.Alicescene.RentAnAlicePay.ExplainsHandGraspsWrist".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.GlancesAwayUnwrapMe".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.SurprisedSqueakOnTop".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.PleadsHerselfDontTear".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.EyesGlareClawsSwipe".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.HardNotHealthyGleam".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.FinallyAwayClingsTighter".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.BreathingQuickensBackScent".Localized());

                    DoNext(() => RentAnAliceCock());
                }
                else
                {
                    OutputText("Scenes.Alicescene.RentAnAlicePay.BrilliantEyesWidenBrush".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.MaybeItMoreCute".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.WasntEnoughBodyClings".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.ShiversLipsMapOut".Localized());

                    OutputText("Scenes.Alicescene.RentAnAlicePay.EveryTicklesHoldBack".Localized((Player.Race == "human" ? "inhuman" : "unnatural")));

                    OutputText("Scenes.Alicescene.RentAnAlicePay.FinallyBackBlazesColor".Localized());

                    if (Player.IsNaked() && Player.Armor.Id != ArmorIds.VineArmorBehavior)
                        OutputText("Scenes.Alicescene.RentAnAlicePay.SameIdeaGlancesUp".Localized());
                    else
                    {
                        OutputText("Scenes.Alicescene.RentAnAlicePay.FingersRestTentativelyArmor".Localized());

                        OutputText("Scenes.Alicescene.RentAnAlicePay.SwallowsFaceRedderMe".Localized());

                        OutputText("Scenes.Alicescene.RentAnAlicePay.TwoWordsBarelyLouder".Localized());

                        if (Player.Armor.Id == ArmorIds.VineArmorBehavior)
                        {
                            OutputText("Scenes.Alicescene.RentAnAlicePay.TouchStartsHesitantFingertips".Localized());

                            OutputText("Scenes.Alicescene.RentAnAlicePay.PausesTuggingRopeBindings".Localized());

                            OutputText("Scenes.Alicescene.RentAnAlicePay.NodPartOfYou".Localized());
                            // 原版 AliceScene.hx:1611：藤甲有花色阶段（item.Stage != 0）时换一句描写
                            OutputText("Scenes.Alicescene.RentAnAlicePay.GazeFallsTo".Localized(
                                Player.Armor.Stage != 0

                                    ? " the colorful blooms dotting what passes for your clothes. [say: They're so pretty...]"

                                    : "your exposed body. [say: So you're already...]"));
                        }
                        else
                            OutputText("Scenes.Alicescene.RentAnAlicePay.TicklishBrushOnlyWarning".Localized());
                    }

                    DoNext(() => RentAnAliceYuri());
                }
            }
            else
            {
                OutputText("Scenes.Alicescene.RentAnAlicePay.TextHandHerEverything".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.SmileThatsEverything".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.FirstTouchArmsWrapping".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.CouldStayDayBasking".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.StillComfortingCampCare".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.FingersTraceAcrossPalm".Localized());

                OutputText("Scenes.Alicescene.RentAnAlicePay.ThankHerTrekBack".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 15f));

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void RentAnAliceCock()
        {
            ClearOutput();

            var i = Player.CockThatFits(12, "length");

            RegisterMacro("cock", () => Player.CockDescript(i));

            if (i < 0)
            {
                i = Player.BiggestCockIndex();

                RegisterMacro("cock", () => Player.CockDescript(i));

                OutputText("Scenes.Alicescene.RentAnAliceCock.ButCantMindRealizes".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.DeepBreathPaceStop".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.SlightMovementWarningMouth".Localized());

                if (Player.Corruption >= 50 || Player.Lust == Player.MaxLust)
                {
                    OutputText("Scenes.Alicescene.RentAnAliceCock.TooSenseMovingBack".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.StrugglesGagsFirstSeed".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.InhaleSharplySlideOut".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.PrettyGoodWaySpend".Localized());
                }
                else
                {
                    OutputText("Scenes.Alicescene.RentAnAliceCock.TooSenseLipsSliding".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.WhispersShaftNotYet".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.BreathTremblesGripLoosen".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.Cum".Localized(), RentAnAliceCum);

                    AddNextButton("Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), RentAnAliceDontCum);

                    return;
                }
            }
            else
            {
                OutputText("Scenes.Alicescene.RentAnAliceCock.WantSlideSlickLips".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.ConfusionFlitsMotionsSlow".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.LegsTrembleBeneathFabric".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.FlushDesireLeanCloser".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.WhispersBreathyBeggingLegs".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceCock.LineUpRefuseOffer".Localized());

                if (Player.Corruption >= 50 || Player.Lust == Player.MaxLust)
                {
                    OutputText("Scenes.Alicescene.RentAnAliceCock.WantsBadPinShoulders".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.BodyCallsWordsAfraid".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.CriesMixSatisfiedGroan".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.GlancingDownMoistureBeading".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.NotMattersGrabWaist".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.MustSenseWallsClench".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.BothCryEmptyInside".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.AsLikeStayLim".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.DontThinkMuchMore".Localized());
                }
                else
                {
                    OutputText("Scenes.Alicescene.RentAnAliceCock.CautiouslyShiftForwardHeat".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.EachBrushSilkenGrip".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.NodsGentlyLineSink".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.MaybeMeantToWrong".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.ShaftTwitchesEagerFill".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.HeadFallsVoicesMix".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.WarmCloseAllNeed".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.MumblesThanksHelps".Localized());

                    OutputText("Scenes.Alicescene.RentAnAliceCock.TakeMomentCleanHeading".Localized());
                }
            }

            Player.Orgasm("Dick");

            AliceCorruption(15, true, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceCum()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceCum.CantSoonReleasedPressure".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceCum.BodyShakesReliefSeed".Localized((skinTone == "milky-white" || skinTone == "fair" ? "she's coated in your seed" : "even her [aliceskin] skin turns pale")));

            OutputText("Scenes.Alicescene.RentAnAliceCum.HandWipesFaceDipping".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceCum.OneHeadsBackShade".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceCum.OnlyHalfwayHomeTingling".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(15, true, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceDontCum()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.GrittingTeethRestraintEdge".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.HandTracesStomachWanted".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.FallsQuietSlidesPanties".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.DefinitelyNotPantiesWrap".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.DampTooWarmSlick".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.WhispersLetFeelYou".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.MumblesThanksBreathingHeavily".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceDontCum.NotExpectedCantDeny".Localized());

            Player.Orgasm("Dick");

            AliceCorruption(15, true, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceYuri()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceYuri.ReachesOutGroanTearing".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuri.EverythingFallsAwayFades".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuri.AfterThoughtGlancesUp".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuri.NotWasteChanceDown".Localized());

            if (Player.Corruption >= 50 || Player.Lust == Player.MaxLust)
            {
                OutputText("Scenes.Alicescene.RentAnAliceYuri.ObviousStainCatchEye".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.MentionMakesGazeFall".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.DespiteBlushDoesntAway".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.ClenchingFistsWalksOver".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.SmileDoesntGiveBack".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.KnowsExactlyWhatWant".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.SuddenShiftWarningCoils".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.InsteadSlippingPeersUp".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Praise".Localized(), () => RentAnAliceYuriCorrupt(true));

                AddNextButton("Common.Button.Ignore".Localized(), () => RentAnAliceYuriCorrupt(false));
            }
            else
            {
                OutputText("Scenes.Alicescene.RentAnAliceYuri.IfThatExcitedStop".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.LipsTickleAsksUm".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuri.FingertipsSnakeDownHips".Localized());

                ClearButtons();

                AddNextButton("Common.Button.LetHer".Localized(), () => RentAnAliceYuriNormal(true));

                AddNextButton("Common.Button.TouchAgain".Localized(), () => RentAnAliceYuriNormal(false));
            }
        }

        public void RentAnAliceYuriCorrupt(bool praised)
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.RentAnAliceYuriCorrupt.TightenGripPullVagina".Localized((praised ? "She stiffens as you ruffle her hair, slathering her with praise she doesn't deserve. All you manage to coax out of her is an uneasy smile, so you" : "Your expression doesn't change, and she quickly glances away until you")));

            OutputText("Scenes.Alicescene.RentAnAliceYuriCorrupt.InnocentEyesGlitterTears".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriCorrupt.ShiverCoursesFullness".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriCorrupt.DontBotherBackCurl".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriCorrupt.PantingMixesComeDown".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriCorrupt.OnceFinishLeaveHer".Localized());

            Player.Orgasm("Vaginal");

            AliceCorruption(15, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void RentAnAliceYuriNormal(bool touched)
        {
            ClearOutput();

            if (touched)
            {
                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.InNoNotDay".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.SettleForFallingDeeper".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.CantDenySilentRequest".Localized());
            }

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.LipsFlutterBrieflyChest".Localized((touched ? "[pg]She steps closer, and you can barely contain yourself when h" : "H")));

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.WhispersPracticallyFeelSound".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.WorksInwardSupportEmpty".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.EveryFlutterWallsGrip".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.FamiliarEyesHalfLidded".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.WantInsideShakyStroke".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.SighSlipsOutLeaves".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.PausesJustEnoughBreath".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.EntireBodyTremblesLightning".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal.StillTinglesFromTouch".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddNextButton("Common.Button.ReturnFavor".Localized(), () => RentAnAliceYuriNormal2(true, touched));

            AddNextButton("Scenes.ErlKing.HowDareYou.ButtonDont".Localized(), () => RentAnAliceYuriNormal2(false));
        }

        public void RentAnAliceYuriNormal2(bool favor, bool touched = false)
        {
            ClearOutput();

            if (favor)
            {
                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.SlightestTouchFingersChest".Localized((touched ? "hips." : "[alicepanties] panties. Even as soft, cute, and, as one touch confirms, soaked as they are, you still need them gone. She makes no motion for you to stop as you take the fabric into your hands, brushing against the the warmth of her skin as you slide them lower until she's completely exposed.")));

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.DespiteQuicklyGrindBack".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.HandsLegsHoldSteady".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.WrapsArmsEaseFurther".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.WetnessTrailsAnotherFinger".Localized());

                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.FlushedDelightfullyWarm".Localized());
            }
            else
                OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.RightNotSureManage".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.FinallyReleaseGathersThings".Localized());

            OutputText("Scenes.Alicescene.RentAnAliceYuriNormal2.TakeMomentCleanUp".Localized());

            AliceCorruption(15, true);

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraPossession()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.ShouldraPossession.LittleDemonBeatenShouldra".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.ViolentlyThrashesGetHead".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.WatchSlowlySitsBlank".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.WorriesPutEaseJumps".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.ConfusedFirstSmileTender".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.CombingStrokeGrabsAppendage".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.ShoutsWhineWonderfulPetting".Localized());

            OutputText("Scenes.Alicescene.ShouldraPossession.GhostRisesPassedAlice".Localized());

            AliceCorruption(15, true);

            CombatScene.CleanupAfterCombat();
        }

        public void StrayAkky()
        {
            ClearOutput();

            Save.SetBool("akkySeen", true);

            OutputText("Scenes.Alicescene.StrayAkky.FamiliarAuraInvadesTrees".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.NoticeDemonessSittingTired".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.WishLifeEasierKitty".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.ClosesBaggyEyesSighs".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.HaventSeenBigJaguar".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.OpensEyesLooksCat".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.ThinkImTastyKitty".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.CatYawnsLicksHand".Localized());

            OutputText("Scenes.Alicescene.StrayAkky.GuessThatsYes".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Intervene".Localized(), StrayAkkyIntervene);

            AddNextButton("Common.Button.Leave".Localized(), StrayAkkyLeave);
        }

        public void StrayAkkyIntervene()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.StrayAkkyIntervene.WalkDirectionCatSprints".Localized());

            OutputText("Scenes.Alicescene.StrayAkkyIntervene.ExclaimsBoltsTrustingInstincts".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void StrayAkkyLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.StrayAkkyLeave.SerenityFineLetBe".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public Identifier ChooseTreat()
        {

            var treats = new[] { ConsumableIds.Lollipop, ConsumableIds.NumbRocks };

            return treats.FirstOrDefault(item => Player.HasItem(item));
        }

        public void TrystOrTreatIntro()
        {
            ClearOutput();

            bool hasTreats = ChooseTreat() != null;

            Save.SetInt("trystOrTreatedYear", DateTime.Now.Year);

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.CrispAirWalkOff".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.BurstingDarknessPairOff".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.CloserInspectionIncubus".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.OpensSackTrystTreat".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.RobberyMuggingNotSure".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.WhinesScaryVampireCloak".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.ButImDe".Localized());

            OutputText("Scenes.Alicescene.TrystOrTreatIntro.LeansForwardFluttersTreats".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Tryst".Localized(), TotTryst);

            AddNextButton("Common.Button.Treat".Localized(), TotTreat);

            AddNextButton("Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), TotFight);
        }

        public void TotTryst()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            OutputText("Scenes.Alicescene.TotTryst.GlancingCampFigureGive".Localized());

            OutputText("Scenes.Alicescene.TotTryst.ConfusedWerewolfLowersBag".Localized());

            OutputText("Scenes.Alicescene.TotTryst.AmusedFullWellIncubus".Localized());

            OutputText("Scenes.Alicescene.TotTryst.WerentADemon".Localized());

            OutputText("Scenes.Alicescene.TotTryst.WereADemon".Localized());

            OutputText("Scenes.Alicescene.TotTryst.PullsOutGraspGrabs".Localized());

            OutputText("Scenes.Alicescene.TotTryst.WolfBoyBlushesStammers".Localized());

            OutputText("Scenes.Alicescene.TotTryst.PushingHimAwayGestures".Localized());

            ClearButtons();

            AddNextButton("Common.Button.FuckHer".Localized(), TotChooseLoli);

            AddNextButton("Common.Button.FuckHim".Localized(), TotShotaFuck);

            AddNextButton("Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonRideHim".Localized(), TotShotaRide);

            AddNextButton("Common.Button.Leave".Localized(), TotLeave);
        }

        public void TotChooseLoli()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            OutputText("Scenes.Alicescene.TotChooseLoli.WolfBoyFlustered".Localized());

            OutputText("Scenes.Alicescene.TotChooseLoli.PurrsFlickingTailType".Localized());

            OutputText("Scenes.Alicescene.TotChooseLoli.SkipsExcitementWindsKiss".Localized());

            OutputText("Scenes.Alicescene.TotChooseLoli.ChimesTheseFuckMe".Localized());

            OutputText("Scenes.Alicescene.TotChooseLoli.PositionAwkwardGrabWaistband".Localized());

            OutputText("Scenes.Alicescene.TotChooseLoli.InstinctivelyReachesGarment".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Alicescene.TotChooseLoli.YellsAsRiseDumping".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.HandsClaspCockheadShape".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.GroanThisRealCock".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.EachSoundBuildsCharging".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.PantsLikeJerksHarder".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.NoticesHimAgainThanks".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.MockeryDoesntSlowMount".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.GaspingAirHoldTight".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.AsksHesitantlyMaybeNext".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.BlinksLiftingReddenedFace".Localized());
            }
            else
            {
                OutputText("Scenes.Alicescene.TotChooseLoli.YellsRiseDumpingAgain".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.HandsClaspScrunchingHair".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.SaysExactlySuckIt".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.CrawlWordlesslyReadsIntention".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.GesturesForYouObediently".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.LeavesDazeEachSound".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.PantsJerksHarderAgain".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.NoticesAgainSlipsHer".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.DeepHazeHoldSqueeze".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.CommandsPleadsForeheadNuzzles".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.AsksHesitantlyMaybeAgain".Localized());

                OutputText("Scenes.Alicescene.TotChooseLoli.BlinksCraningNeckLook".Localized());
            }

            Player.Orgasm("DickVaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void TotShotaIntro()
        {

            OutputText("Scenes.Alicescene.TotShotaIntro.IgnoringHaughtyWolfBoy".Localized());

            OutputText("Scenes.Alicescene.TotShotaIntro.ExclaimsIneffectuallyForward".Localized());

            OutputText("Scenes.Alicescene.TotShotaIntro.CompanionSeemedLikeHaste".Localized());

            OutputText("Scenes.Alicescene.TotShotaIntro.MopesWhileWatches".Localized());
        }

        public void TotShotaFuck()
        {
            ClearOutput();

            TotShotaIntro();

            OutputText("Scenes.Alicescene.TotShotaFuck.TakesNoNoticeBack".Localized());

            OutputText("Scenes.Alicescene.TotShotaFuck.WhinesMasturbatesFaster".Localized());

            OutputText("Scenes.Alicescene.TotShotaFuck.LustBurnsLikeBonfire".Localized());

            OutputText("Scenes.Alicescene.TotShotaFuck.CriesEchoForces".Localized());

            OutputText("Scenes.Alicescene.TotShotaFuck.ReinforcingPartnerRocksHips".Localized());

            OutputText("Scenes.Alicescene.TotShotaFuck.ChokesOnTears".Localized());

            OutputText("Scenes.Alicescene.TotShotaFuck.BoyTiredlyCorrects".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void TotShotaRide()
        {
            ClearOutput();

            bool pussylessDrider = Player.IsDrider() && !Player.HasVagina();

            RegisterMacro("pussylessdrider", pussylessDrider);

            TotShotaIntro();

            OutputText("Scenes.Alicescene.TotShotaRide.TakesNoFixatingNaked".Localized());

            OutputText("Scenes.Alicescene.TotShotaRide.WhinesMasturbatesPullingAside".Localized());

            OutputText("Scenes.Alicescene.TotShotaRide.DoesntEscapeHappyDitch".Localized());

            OutputText("Scenes.Alicescene.TotShotaRide.HornyWolfBoy".Localized());

            if (Player.HasVagina())
                Player.CuntChange(5, true);
            else
                Player.ButtChange(5, true, false);

            OutputText("Scenes.Alicescene.TotShotaRide.InitialAchesTurnPleasure".Localized());

            OutputText("Scenes.Alicescene.TotShotaRide.JoltKnotPopsEngorge".Localized());

            OutputText("Scenes.Alicescene.TotShotaRide.ChokesOnTearsAgain".Localized());

            OutputText("Scenes.Alicescene.TotShotaRide.BoyTiredlyCorrectsAgain".Localized());

            Player.Orgasm("VaginalAnal");

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void TotLeave()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.TotLeave.ClearlyGetWithoutYou".Localized());

            OutputText("Scenes.Alicescene.TotLeave.MomentsLaterBackCamp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TotTreat()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, -3f));

            var treat = ChooseTreat();

            string treatText = treat == ConsumableIds.Lollipop ? "a lolipop" : "the treats";

            OutputText("Scenes.Alicescene.TotTreat.StunnedVampireNoMotion".Localized());

            OutputText("Scenes.Alicescene.TotTreat.IncubusCorrectsGirlPunches".Localized());

            Player.DestroyItems(treat);

            DoNext(() => ReturnToCamp(1));
        }

        public void TotFight()
        {
            ClearOutput();

            if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            OutputText("Scenes.Alicescene.TotFight.ThinkingSwingFistSack".Localized());

            OutputText("Scenes.Alicescene.TotFight.GroundSobbingCoversHead".Localized());

            OutputText("Scenes.Alicescene.TotFight.ThinkingFastBackCamp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AliceBallEncounter()
        {
            ClearOutput();

            GenerateAlice();

            OutputText("Scenes.Alicescene.AliceBallEncounter.LitStrollGoingDanger".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Watch".Localized(), AliceBallWatch);

            AddNextButton("Combat.Button.Approach".Localized(), AliceBallApproach);
        }

        public void AliceBallWatch()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceBallWatch.LittleSkinnedTossesBall".Localized());

            OutputText("Scenes.Alicescene.AliceBallWatch.SparkFlitsCareening".Localized());

            OutputText("Scenes.Alicescene.AliceBallWatch.ZealThrowsBouncesBack".Localized());

            if ((OptionStore.Instance.Get("gore", 0) > 0) && false)
            {
                ClearButtons();

                AddNextButton("Common.Button.Leave".Localized(), () => InventoryScene.TakeItem(ItemRegistry.Create(ConsumableIds.RubberBall), () => ReturnToCamp(1)));

                AddNextButton("Common.Button.UseHer".Localized(), AliceBallUse);
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.RubberBall), 1);

                ReturnToCamp(1);
            }
        }

        public void AliceBallUse()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceBallUse.SurrenderingSighApproachCorpse".Localized());

            OutputText("Scenes.Alicescene.AliceBallUse.GlidingCaressBruisedFace".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Alicescene.AliceBallUse.WhetherSuccubusBloodImpulse".Localized());

                OutputText("Scenes.Alicescene.AliceBallUse.ShiftingFocusScoopShoulders".Localized());

                OutputText("Scenes.Alicescene.AliceBallUse.HipsInadvertentlyPushAway".Localized());

                OutputText("Scenes.Alicescene.AliceBallUse.AnxiousGleeReverberatesMoans".Localized());

                Player.Orgasm("Vagina");
            }
            else
            {
                OutputText("Scenes.Alicescene.AliceBallUse.WhetherSuccubusImpulseAgain".Localized());

                OutputText("Scenes.Alicescene.AliceBallUse.FleshDragsNoMuscles".Localized());

                OutputText("Scenes.Alicescene.AliceBallUse.WithdrawingBringCockHead".Localized());

                OutputText("Scenes.Alicescene.AliceBallUse.UnclearTimeDroolingSemen".Localized());

                Player.Orgasm("Dick");
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 4f));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.RubberBall), 1);

            ReturnToCamp(1);
        }

        public void AliceBallApproach()
        {
            ClearOutput();

            OutputText("Scenes.Alicescene.AliceBallApproach.InklingPersistsGnawing".Localized());

            OutputText("Scenes.Alicescene.AliceBallApproach.EyesGaspBallBouncing".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.RubberBall);

            currentAlice.OnDefeated = _ => AliceWin();

            currentAlice.OnWon = _ => AliceLoss();

            CombatScene.Start(currentAlice, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

    }
}
