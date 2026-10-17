using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<Kittens>("coc:kittens")]
public class Kittens : Scene
    {

        public bool metKittens = false;

        public int kittensResult = 0;

        public const int HELPED = 1 << 1;

        public const int LEFT = 1 << 2;

        public const int TAKEN = 1 << 3;

        public const int DEFIANT = 1 << 4;

        public const int OBEDIENT = 1 << 5;

        public const int WATCHED = 1 << 6;

        public const int SPANKED = 1 << 7;

        public static SaveContent Save => SaveContent(SceneIds.TelAdre.Kittens);

        public override void Entry()
        {
            KittensMeet();
        }

        public void KittensMeet()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensMeet.WanderThroughTelAdre".Localized());

            OutputText("Scenes.Kittens.KittensMeet.FairlyDarkHere".Localized());

            OutputText("Scenes.Kittens.KittensMeet.UUhmExcuseMe".Localized());

            OutputText("Scenes.Kittens.KittensMeet.AYoungGirlIs".Localized());

            OutputText("Scenes.Kittens.KittensMeet.ExclaimsHighPitched".Localized());

            OutputText("Scenes.Kittens.KittensMeet.DoesntDoMuchTo".Localized());

            OutputText("Scenes.Kittens.KittensMeet.LittleToGoBy".Localized());
            // 原版 Kittens.hx:96 — 原版 allowChild 全局开关
            if (GameSettings.AllowChild)
                OutputText("Scenes.Kittens.KittensMeet.IfSoInclined".Localized());

            OutputText("Scenes.Kittens.KittensMeet.WhatShouldYouDo".Localized());

            Save.SetBool("metKittens", true);

            ClearButtons();

            AddNextButton("Common.Button.HelpThem".Localized(), KittensHelp);
            // 原版 Kittens.hx:103 — 原版 allowChild 全局开关
            if (GameSettings.AllowChild)
                AddNextButton("Common.Button.HelpYourself".Localized(), KittensRape);

            AddNextButton("Common.Button.Leave".Localized(), KittensLeave);
        }

        public void KittensHelp()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensHelp.AlmostShoutsInExcitement".Localized());

            OutputText("Scenes.Kittens.KittensHelp.WithoutRiskOfGetting".Localized());

            OutputText("Scenes.Kittens.KittensHelp.LittleGirlPipesUp".Localized());

            OutputText("Scenes.Kittens.KittensHelp.LooksConfusedAtYour".Localized());

            OutputText("Scenes.Kittens.KittensHelp.AnotherVoiceMumbles".Localized());

            OutputText("Scenes.Kittens.KittensHelp.ThisHereSaid".Localized());

            OutputText("Scenes.Kittens.KittensHelp.OhHello".Localized());

            OutputText("Scenes.Kittens.KittensHelp.GreetTiredKitten".Localized());

            OutputText("Scenes.Kittens.KittensHelp.IsAllSheAnswers".Localized());

            OutputText("Scenes.Kittens.KittensHelp.AskingRandomPassers".Localized());

            OutputText("Scenes.Kittens.KittensHelp.PromisedBuilding".Localized());

            OutputText("Scenes.Kittens.KittensHelp.LeanHorseMorph".Localized());

            OutputText("Scenes.Kittens.KittensHelp.WelcomeToCityWatch".Localized((Player.IsChild() || Player.IsTeen() ? " kids" : ", citizen")));

            OutputText("Scenes.Kittens.KittensHelp.SistersStayQuiet".Localized());

            OutputText("Scenes.Kittens.KittensHelp.FlippingThroughStack".Localized());

            OutputText("Scenes.Kittens.KittensHelp.CoupleMinutesPass".Localized());

            OutputText("Scenes.Kittens.KittensHelp.DisappointedDeadEnd".Localized());

            OutputText("Scenes.Kittens.KittensHelp.NotSureIfYou".Localized());

            OutputText("Scenes.Kittens.KittensHelp.HearOneOfSisters".Localized());

            OutputText("Scenes.Kittens.KittensHelp.ShoutsSwoopingThemUp".Localized());

            OutputText("Scenes.Kittens.KittensHelp.OlderOneSaysStruggling".Localized());

            OutputText("Scenes.Kittens.KittensHelp.HelpedUsTooLynn".Localized());

            OutputText("Scenes.Kittens.KittensHelp.ClaspingYourHand".Localized());

            OutputText("Scenes.Kittens.KittensHelp.ChantInUnison".Localized());

            OutputText("Scenes.Kittens.KittensHelp.HeartWarmingAsThis".Localized());

            OutputText("Scenes.Kittens.KittensHelp.SayYourFarewells".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | HELPED);

            Player.Gems += 20;

            DoNext(() => ReturnToCamp(2));
        }

        public void KittensRape()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensRape.DefinitelyGoingToHelp".Localized());

            OutputText("Scenes.Kittens.KittensRape.ChairScrapingOver".Localized());

            OutputText("Scenes.Kittens.KittensRape.StepsHurriedSuspense".Localized((!Player.IsChild() ? ", [mister]" : "")));

            OutputText("Scenes.Kittens.KittensRape.GirlAtSideAsks".Localized());

            OutputText("Scenes.Kittens.KittensRape.FinallyFindWhat".Localized());

            OutputText("Scenes.Kittens.KittensRape.CantSeeMommy".Localized());

            OutputText("Scenes.Kittens.KittensRape.WhatToDoStart".Localized());

            OutputText("Scenes.Kittens.KittensRape.SheFidgetsDid".Localized());

            OutputText("Scenes.Kittens.KittensRape.SlowlyTeasinglyGrips".Localized());

            OutputText("Scenes.Kittens.KittensRape.DiggingIntoThighs".Localized());

            OutputText("Scenes.Kittens.KittensRape.NoticeSlightSpotOf".Localized());

            OutputText("Scenes.Kittens.KittensRape.LittleCatGirlHuffs".Localized());

            OutputText("Scenes.Kittens.KittensRape.KnewYouWereFilthy".Localized((!Player.IsChild() ? ", [mister]" : "")));

            OutputText("Scenes.Kittens.KittensRape.ColdSweatRunsDown".Localized());

            OutputText("Scenes.Kittens.KittensRape.WhispersSlightGiggle".Localized());

            OutputText("Scenes.Kittens.KittensRape.WhyNotBothPlay".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | TAKEN);

            ClearButtons();

            AddNextButton("Common.Button.GoWithIt".Localized(), KittensContinue);

            AddNextButton("Common.Button.GetOut".Localized(), KittensGetOut);
        }

        public void KittensContinue()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensContinue.WhoToSayNo".Localized());

            OutputText("Scenes.Kittens.KittensContinue.ReallyAreAPervert".Localized());

            OutputText("Scenes.Kittens.KittensContinue.RegardsUpAndDown".Localized());

            OutputText("Scenes.Kittens.KittensContinue.YouUnderstand".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Absolutely".Localized(), KittensAbsolutely);

            AddNextButton("Common.Button.FuckThis".Localized(), KittensFuckThat);

            AddNextButton("Common.Button.Spank".Localized(), () => KittensSpank());
        }

        public void KittensAbsolutely()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensAbsolutely.DoAsSheSays".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.CoosPattingYourHead".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.GropesFlatChest".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.TurnsToFaceYou".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.CirclingAFingerOver".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.StareWithRaptFascination".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.BeautifulYoungPlump".Localized());

            OutputText("Scenes.Kittens.KittensAbsolutely.CallsOutToYou".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Sit".Localized(), () => KittensSit(true));

            AddNextButton("Common.Button.DontSit".Localized(), KittensDontSit);
        }

        public void KittensSit(bool first)
        {
            ClearOutput();

            if (first)
            {
                OutputText("Scenes.Kittens.KittensSit.ObedientlyDoAsTold".Localized());

                OutputText("Scenes.Kittens.KittensSit.HahaaHowPathetic".Localized());
            }
            else
            {
                OutputText("Scenes.Kittens.KittensSit.JustTestingYourBonds".Localized());

                OutputText("Scenes.Kittens.KittensSit.WarnsThoughExpression".Localized());
            }

            OutputText("Scenes.Kittens.KittensSit.LooksDownAtYou".Localized());

            OutputText("Scenes.Kittens.KittensSit.LickingHerDigitsClean".Localized());

            OutputText("Scenes.Kittens.KittensSit.SwingsLegsOver".Localized());

            OutputText("Scenes.Kittens.KittensSit.SheMurmurs".Localized());

            OutputText("Scenes.Kittens.KittensSit.ShhhNotYet".Localized());

            OutputText("Scenes.Kittens.KittensSit.LooksReadyToNod".Localized());

            OutputText("Scenes.Kittens.KittensSit.FoundANewToy".Localized());

            OutputText("Scenes.Kittens.KittensSit.RestOfSentenceGets".Localized());

            OutputText("Scenes.Kittens.KittensSit.SwallowingWatchThem".Localized());

            OutputText("Scenes.Kittens.KittensSit.OlderSiblingBreaks".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Kittens.KittensSit.ReachesBackAndSpreads".Localized());

                OutputText("Scenes.Kittens.KittensSit.UhmSis".Localized((Player.Cocks[0].Length > 11 ? "Uhm, sis... it's big..." : "Sis... I don't know about this...")));

                OutputText("Scenes.Kittens.KittensSit.LovinglyStrokesFace".Localized());

                OutputText("Scenes.Kittens.KittensSit.TestinglyPushCrown".Localized((Player.Cocks[0].Length > 11 ? "prodigious length hits her innermost depths" : "hips meet hers")));

                OutputText("Scenes.Kittens.KittensSit.MewlsGraspingForSupport".Localized());

                OutputText("Scenes.Kittens.KittensSit.EmbracesKissesAgain".Localized());

                OutputText("Scenes.Kittens.KittensSit.WallsConstrictAround".Localized());

                OutputText("Scenes.Kittens.KittensSit.WetSquelchingPussy".Localized());

                OutputText("Scenes.Kittens.KittensSit.HoldOffPressure".Localized((Player.Cocks[0].Length > 11 ? "push right up to the entrance of her prepubescent womb" : "hilt yourself deep"), (Player.CumQ() >= 50 ? ", some of your spunk spilling out around your dick" : "")));

                OutputText("Scenes.Kittens.KittensSit.ComeDownFromYour".Localized());

                OutputText("Scenes.Kittens.KittensSit.CaressingSleepingSister".Localized());

                OutputText("Scenes.Kittens.KittensSit.DrawOutOfLynn".Localized((Player.CumQ() >= 100 ? "small stream" : "trickle")));

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.Kittens.KittensSit.ReachesBackSpreads".Localized());

                OutputText("Scenes.Kittens.KittensSit.TremblesUnderTongue".Localized((Player.HasLongTongue() ? "spearing her entirely on your prodigious length" : "thrusting in as deep as you manage")));

                OutputText("Scenes.Kittens.KittensSit.ViewSuddenlyBlocked".Localized());

                OutputText("Scenes.Kittens.KittensSit.WordRegistersIn".Localized());

                OutputText("Scenes.Kittens.KittensSit.OnlyNowNoticeLynn".Localized());

                OutputText("Scenes.Kittens.KittensSit.HandLowersOnto".Localized());

                OutputText("Scenes.Kittens.KittensSit.ThumbingPlumpLabia".Localized());

                OutputText("Scenes.Kittens.KittensSit.NotQuiteAsJuicy".Localized());

                OutputText("Scenes.Kittens.KittensSit.ImperiousKittenLeaves".Localized());

                OutputText("Scenes.Kittens.KittensSit.LieDownNow".Localized());

                OutputText("Scenes.Kittens.KittensSit.CommandingTone".Localized());

                OutputText("Scenes.Kittens.KittensSit.OnlySaysPressing".Localized());

                OutputText("Scenes.Kittens.KittensSit.FeelHerShiftForwards".Localized());

                OutputText("Scenes.Kittens.KittensSit.ExclaimsComingAway".Localized());

                OutputText("Scenes.Kittens.KittensSit.OnlySecondsAway".Localized());

                OutputText("Scenes.Kittens.KittensSit.NoTimeToReflect".Localized((Player.Cocks[0].Length > 11 ? "Uhm, sis... it's big..." : "Sis... I don't know about this...")));

                Player.Orgasm("Vaginal");
            }

            DoNext(() => KittensSit2());
        }

        public void KittensSit2()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Kittens.KittensSit2.MetWithSuddenlyCold".Localized());

                OutputText("Scenes.Kittens.KittensSit2.GetOnYourBack".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SpitsGentlyLaying".Localized());

                OutputText("Scenes.Kittens.KittensSit2.GroanEscapesStillRubbing".Localized());

                OutputText("Scenes.Kittens.KittensSit2.TeasesGropingFlatness".Localized());

                OutputText("Scenes.Kittens.KittensSit2.NoTimeToReflect".Localized());

                OutputText("Scenes.Kittens.KittensSit2.InsidesNotQuiteAs".Localized((Player.LongestCockLength() > 14 ? "Not terrible--" : "But he was bigger than you--")));

                OutputText("Scenes.Kittens.KittensSit2.SlapsHerHipsTo".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SneersNotSlowingDown".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SharpSlapReddens".Localized());

                OutputText("Scenes.Kittens.KittensSit2.DontGoFlabby".Localized());

                OutputText("Scenes.Kittens.KittensSit2.YouNeverAreSis".Localized());

                OutputText("Scenes.Kittens.KittensSit2.LynnApparentlyWoken".Localized());

                OutputText("Scenes.Kittens.KittensSit2.LynnDoesntFinish".Localized());

                OutputText("Scenes.Kittens.KittensSit2.TrulyAFeistyLittle".Localized());

                OutputText("Scenes.Kittens.KittensSit2.WithLynnsRelentless".Localized());

                OutputText("Scenes.Kittens.KittensSit2.PunishYouLaterLynn".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SoundsAwfullyPleased".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.Kittens.KittensSit2.FeelHerDrawOut".Localized());

                OutputText("Scenes.Kittens.KittensSit2.ProtestsRubbingCunt".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SleepyVoiceRingsOut".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SoundsFromAbove".Localized());

                OutputText("Scenes.Kittens.KittensSit2.IfOnlyYouSee".Localized());

                OutputText("Scenes.Kittens.KittensSit2.WhinesShakyBreathy".Localized());

                OutputText("Scenes.Kittens.KittensSit2.MommyToldYouNot".Localized());

                OutputText("Scenes.Kittens.KittensSit2.WhatAreYoMnhmnnf".Localized());

                OutputText("Scenes.Kittens.KittensSit2.LipsOccupiedSheClamps".Localized());

                OutputText("Scenes.Kittens.KittensSit2.LynnLynnLynn".Localized());

                OutputText("Scenes.Kittens.KittensSit2.WhaAhh".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SurprisingMartialDisplay".Localized());

                OutputText("Scenes.Kittens.KittensSit2.PlayfullyGrowls".Localized());

                OutputText("Scenes.Kittens.KittensSit2.ShortGrappleLanded".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SisSay".Localized());

                OutputText("Scenes.Kittens.KittensSit2.WhispersRunningFingers".Localized());

                OutputText("Scenes.Kittens.KittensSit2.FacePressesIntoLynns".Localized());

                OutputText("Scenes.Kittens.KittensSit2.HmmTooWet".Localized());

                OutputText("Scenes.Kittens.KittensSit2.ImSorrySis".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SaysCradlingAndPecking".Localized());

                OutputText("Scenes.Kittens.KittensSit2.NowConjoinedTogether".Localized());

                OutputText("Scenes.Kittens.KittensSit2.AgreeWithLynn".Localized());

                OutputText("Scenes.Kittens.KittensSit2.DesperatePantsEcho".Localized());

                OutputText("Scenes.Kittens.KittensSit2.SuddenlyBitingFurredEar".Localized());

                Player.Orgasm("Vaginal");
            }

            DoNext(() => KittensSit3());
        }

        public void KittensSit3()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Kittens.KittensSit3.BodyHurtsFeelsSore".Localized());

                OutputText("Scenes.Kittens.KittensSit3.AsksPointingAtFace".Localized());

                OutputText("Scenes.Kittens.KittensSit3.HmmOhEww".Localized());

                OutputText("Scenes.Kittens.KittensSit3.MommyWillBeAngry".Localized());

                OutputText("Scenes.Kittens.KittensSit3.NotThatMuch".Localized());

                OutputText("Scenes.Kittens.KittensSit3.ButSisYouDidnt".Localized());

                OutputText("Scenes.Kittens.KittensSit3.KnowLynnItsLie".Localized());

                OutputText("Scenes.Kittens.KittensSit3.MomentOfSilence".Localized());

                OutputText("Scenes.Kittens.KittensSit3.PullsLynnUpLovingly".Localized());

                OutputText("Scenes.Kittens.KittensSit3.SighFromHerLips".Localized((Player.ShortName == "pervert" || Player.ShortName == "Pervert" ? "She looks at you, for once stupefied. [say: You know I could kick you, right?] You hastily assure her that's your real name, her leg already swinging back threateningly. [say: Hmph, if you say so. Your mommy must [if (iselder) {have been|be}] a soof-- ah... soothsayer.]" : "[say: Hmph, [name]... Doesn't suit you. You're 'pervert' to me, that's what you are.] Seems like you have no say in that.")));

                OutputText("Scenes.Kittens.KittensSit3.SisEllipsis".Localized());

                OutputText("Scenes.Kittens.KittensSit3.HmmSay".Localized());

                OutputText("Scenes.Kittens.KittensSit3.CanYouCarryMe".Localized());

                OutputText("Scenes.Kittens.KittensSit3.YouCanWalkRight".Localized());

                OutputText("Scenes.Kittens.KittensSit3.IDontKnow".Localized());

                OutputText("Scenes.Kittens.KittensSit3.LaraLooksExasperated".Localized());

                OutputText("Scenes.Kittens.KittensSit3.ILoveYouSis".Localized());

                OutputText("Scenes.Kittens.KittensSit3.LoveYouTooBut".Localized());

                OutputText("Scenes.Kittens.KittensSit3.DontKnowIfLynn".Localized());

                OutputText("Scenes.Kittens.KittensSit3.TailDisappearsBehind".Localized());

                OutputText("Scenes.Kittens.KittensSit3.AfterDoneWithEverything".Localized());

                OutputText("Scenes.Kittens.KittensSit3.PerhapsYoullEvenMeet".Localized());
            }
            else
            {
                OutputText("Scenes.Kittens.KittensSit3.BreathlessLynnFlops".Localized());

                OutputText("Scenes.Kittens.KittensSit3.TakeThatAsCompliment".Localized());

                OutputText("Scenes.Kittens.KittensSit3.NoneOfYourMuscles".Localized());

                OutputText("Scenes.Kittens.KittensSit3.SoftToneHerVoice".Localized());

                OutputText("Scenes.Kittens.KittensSit3.ShakingHerHandIntroduce".Localized((Player.ShortName == "pervert" || Player.ShortName == "Pervert" ? "She looks at you, for once stupefied. [say: You know I could kick you, right?] You hastily assure her that's your real name, her leg already swinging back threateningly. [say: Hmph, if you say so. Your mommy must [if (iselder) {have been|be}] a soof-- ah... soothsayer.]" : "[say: Hmph, [name]... Doesn't suit you. You're 'pervert' to me, that's what you are.] Seems like you have no say in that.")));

                OutputText("Scenes.Kittens.KittensSit3.NoPantiesPrettySure".Localized());

                OutputText("Scenes.Kittens.KittensSit3.LaraSaysEww".Localized());

                OutputText("Scenes.Kittens.KittensSit3.ByePervert".Localized());

                OutputText("Scenes.Kittens.KittensSit3.TailDisappearsBehindAgain".Localized());

                OutputText("Scenes.Kittens.KittensSit3.AfterDoneAgain".Localized());

                OutputText("Scenes.Kittens.KittensSit3.PerhapsMeetAgain".Localized());
            }

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | OBEDIENT);

            Player.ChangeFatigue(25);

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.PinkHeartPanties), () => ReturnToCamp(1));
        }

        public void KittensDontSit()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensDontSit.WhyWouldYouListen".Localized());

            OutputText("Scenes.Kittens.KittensDontSit.GrowlsWithFinality".Localized());

            ClearButtons();

            AddNextButton("Common.Button.SitNow".Localized(), () => KittensSit(false));

            AddNextButton("Common.Button.ScrewIt".Localized(), KittensScrewIt);

            AddNextButton("Common.Button.Spank".Localized(), () => KittensSpank(false));
        }

        public void KittensScrewIt()
        {

            OutputText("Scenes.Kittens.KittensScrewIt.NoThatsItYoure".Localized());

            OutputText("Scenes.Kittens.KittensScrewIt.SpitsYoureDoingJust".Localized());

            OutputText("Scenes.Kittens.KittensScrewIt.FindYourWayOut".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | DEFIANT);

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KittensFuckThat()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensFuckThat.NotGoingToDo".Localized());

            OutputText("Scenes.Kittens.KittensFuckThat.SnarlsSteppingBack".Localized());

            OutputText("Scenes.Kittens.KittensFuckThat.LeaveTheTwoBehind".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | DEFIANT);

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KittensGetOut()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensGetOut.WasntInYourPlans".Localized());

            OutputText("Scenes.Kittens.KittensGetOut.GrumblesAndDrawsBack".Localized((!Player.IsChild() ? ", [mister]" : "")));

            OutputText("Scenes.Kittens.KittensGetOut.StaresNeitherOf".Localized());

            OutputText("Scenes.Kittens.KittensGetOut.LipsCurlUpInto".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), KittensWatch);

            AddNextButton("Common.Button.No".Localized(), KittensDontWatch);

            AddNextButton("Common.Button.Spank".Localized(), () => KittensSpank());
        }

        public void KittensWatch()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensWatch.SurprisedBySudden".Localized((!Player.IsChild() ? ", [mister]" : "")));

            OutputText("Scenes.Kittens.KittensWatch.SmirkComingBack".Localized());

            OutputText("Scenes.Kittens.KittensWatch.TauntsIntoTheAir".Localized((!Player.IsChild() ? ", [mister]" : "")));

            OutputText("Scenes.Kittens.KittensWatch.FineSheenOfWetness".Localized());

            OutputText("Scenes.Kittens.KittensWatch.ShiftsHerSisterAround".Localized());

            OutputText("Scenes.Kittens.KittensWatch.MatchingWhiteHearts".Localized());

            OutputText("Scenes.Kittens.KittensWatch.SeemsThatsYourNickname".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Hers".Localized(), () => KittensWatchAnswer(true));

            AddNextButton("Common.Button.HerSisters".Localized(), () => KittensWatchAnswer(false));
        }

        public void KittensWatchAnswer(bool choice)
        {
            ClearOutput();

            if (choice)
            {
                OutputText("Scenes.Kittens.KittensWatchAnswer.HasToBeHers".Localized());

                OutputText("Scenes.Kittens.KittensWatchAnswer.AnotherSnortChuckling".Localized());
            }
            else
            {
                OutputText("Scenes.Kittens.KittensWatchAnswer.HasToBeSister".Localized());

                OutputText("Scenes.Kittens.KittensWatchAnswer.MayBeHint".Localized());
            }

            OutputText("Scenes.Kittens.KittensWatchAnswer.HumsLookingYouUp".Localized());

            OutputText("Scenes.Kittens.KittensWatchAnswer.RaisesAFinger".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Agree".Localized(), KittensWatchAgree);

            AddNextButton("Common.Button.Bail".Localized(), KittensWatchBail);

            AddNextButton("Common.Button.Spank".Localized(), () => KittensSpank());
        }

        public void KittensWatchAgree()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensWatchAgree.FaceLightsUp".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.StingsLessNow".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.StillCheshireSmile".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.SheDrunkenlyMumbles".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.LovinglyStrokesHerFace".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.EyelidsFlutterEnjoys".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.JustASpinelessStranger".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.YoungerLynnSeemsAwfully".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.HmmSay".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.WhatAreYouDoing".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.MovedOnFromCaressing".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.HmmnnmImTiredSis".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.NimblyBendsDown".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.WasteVeryLittleTime".Localized());

            OutputText("Scenes.Kittens.KittensWatchAgree.WhatStartedInnocent".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => KittensWatch2());
        }

        public void KittensWatch2()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensWatch2.LynnGaspsAsSisters".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.FinallyBreaksOff".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.DownEllipsis".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.DownQuestion".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.LynnDesperatelyPants".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.WhispersPressingDown".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.LocksTheirLips".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.WatchThemRiseFrom".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.SisterInformsYou".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.LicksAgainThenPlants".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.WithoutEffortSlidesDown".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.ParticularlyDeepPlunge".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.JustLikeThatLynn".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.ContinuesToWorkSister".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.DesperateGazeMeets".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.WaveOfEmotionsHits".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.HearSomeoneCryOut".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.NeitherLastsLong".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.SaysTryingHardNot".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.SwallowHardHerWords".Localized());

            OutputText("Scenes.Kittens.KittensWatch2.RunsHerLipsOver".Localized());

            Player.TakeLustDamage(50, false, false);

            DoNext(() => KittensWatch3());
        }

        public void KittensWatch3()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensWatch3.CallsPullingUp".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.TryToIntroduceYourself".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.HeadTiltsTowardsYou".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.LaraAlmostAround".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.AloneAllByYourself".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.AnotherShiverDownSpine".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.PickingYourselfUp".Localized());

            OutputText("Scenes.Kittens.KittensWatch3.MaybeJustMaybe".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | WATCHED);

            Player.ChangeFatigue(50);

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            DoNext(() => ReturnToCamp(2));
        }

        public void KittensWatchBail()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensWatchBail.BitTooMuch".Localized());

            OutputText("Scenes.Kittens.KittensWatchBail.LastThingSheSpits".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | DEFIANT);

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KittensDontWatch()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensDontWatch.GoneForGood".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | DEFIANT);

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KittensSpank(bool clear = true)
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensSpank.AllRightThatsIt".Localized());

            OutputText("Scenes.Kittens.KittensSpank.HeyWhatDoing".Localized());

            OutputText("Scenes.Kittens.KittensSpank.FreezesImmediately".Localized());

            OutputText("Scenes.Kittens.KittensSpank.SomeoneSnappedOut".Localized((Player.IsChild() || Player.IsTeen() ? "someone" : "[if (isadult) { an adult|the elderly}]")));

            OutputText("Scenes.Kittens.KittensSpank.ToHerCreditNot".Localized());

            OutputText("Scenes.Kittens.KittensSpank.GrabTheWaistband".Localized());

            OutputText("Scenes.Kittens.KittensSpank.DodgeHerTail".Localized());

            OutputText("Scenes.Kittens.KittensSpank.YelpHastilySuppressed".Localized());

            OutputText("Scenes.Kittens.KittensSpank.ShoutsVoiceQuivering".Localized());

            OutputText("Scenes.Kittens.KittensSpank.GaveUpOnCounting".Localized());

            OutputText("Scenes.Kittens.KittensSpank.MewlsAtLast".Localized());

            OutputText("Scenes.Kittens.KittensSpank.AnotherVoiceChimesIn".Localized());

            OutputText("Scenes.Kittens.KittensSpank.OlderOneSobs".Localized());

            OutputText("Scenes.Kittens.KittensSpank.NoMercyNow".Localized());

            OutputText("Scenes.Kittens.KittensSpank.ThroughTheEntirety".Localized());

            OutputText("Scenes.Kittens.KittensSpank.FinalMightySpank".Localized());

            OutputText("Scenes.Kittens.KittensSpank.ReleaseBruisedDistraught".Localized());

            OutputText("Scenes.Kittens.KittensSpank.MeeklyBegins".Localized());

            OutputText("Scenes.Kittens.KittensSpank.NoMeasureNeeded".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | SPANKED);

            DoNext(() => ReturnToCamp(1));
        }

        public void KittensLeave()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensLeave.LittleGirlLooksCrestfallen".Localized());

            Save.SetInt("kittensResult", Save.GetInt("kittensResult", 0) | LEFT);

        }

        public void KittensPlayerSpanked()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensPlayerSpanked.ParagraphBreak".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Talk".Localized(), KittensGuardTalk);

            AddNextButton("Common.Button.Run".Localized(), KittensRun);
        }

        public void KittensRun()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensRun.ParagraphBreak".Localized());

            Player.ChangeFatigue(15);

            Save.SetInt("disabledDate", View.Time.Days + 30);

            DoNext(() => ReturnToCamp(1));
        }

        public void KittensGuardTalk()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreak".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakThird".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakFifth".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakSixth".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakSeventh".Localized());

            OutputText("Scenes.Kittens.KittensGuardTalk.ParagraphBreakEighth".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Follow".Localized(), () => KittensGuardFollow(default));

            AddNextButton("Common.Button.Flee".Localized(), KittensGuardFlee);
        }

        public void KittensGuardFlee()
        {
            ClearOutput();

            OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreak".Localized());

            if (Player.Speed >= 90)
            {
                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakThird".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakFourth".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakFifth".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakSixth".Localized());

                Player.ChangeFatigue(40);

                Save.SetInt("disabledDate", View.Time.Days + 30);

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakSeventh".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakEighth".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakNinth".Localized());

                OutputText("Scenes.Kittens.KittensGuardFlee.ParagraphBreakTenth".Localized());

                KittensGuardFollow(false);
            }
        }

        public void KittensGuardFollow(Boolean willing)
        {
            ClearOutput();

            if (willing)
            {
                OutputText("Scenes.Kittens.KittensGuardFollow.ParagraphBreak".Localized());

                OutputText("Scenes.Kittens.KittensGuardFollow.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Kittens.KittensGuardFollow.ParagraphBreakThird".Localized());

            OutputText("Scenes.Kittens.KittensGuardFollow.ParagraphBreakFourth".Localized());

            GameOver();
        }

    }
}