using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Internals;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.MothCave
{
    [Register<MothCaveScene>("coc:moth_cave")]
    public class MothCaveScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.MothCave.MothCaveScene);

        public static SaveContent DoloresSave => SaveContent(SceneIds.MothCave.MothCaveScene);

        public static SaveContent SylviaSave => SaveContent(SceneIds.Bog.Sylvia);

        public static int DoloresProg => DoloresSave.GetInt("doloresProgress", 0);

        public static int DoloresTime => DoloresSave.GetInt("doloresTimeSinceEvent", 0);

        public static int DoloresProgress => DoloresSave.GetInt("doloresProgress", 0);

        public static DoloresScene doloresScene => SceneInstance<DoloresScene>.Get();

        public string debugName => "Moth Cave";

        public string debugHint => "Sylvia's home.";

        public override void Entry()
        {
            EncounterCave();
        }

        public void Reset()
        {

            Save.SetInt("tapestryChange", 0);

            Save.SetInt("tapestryTime", 0);

            Save.SetBool("goneCamping", false);
        }

        public void EncounterCave()
        {
            DoloresSave.SetBool("doloresAngry", false);

            if (SceneInstance<SylviaScene>.Get().SylviaProg == 4
                && SylviaSave.GetInt("sylviaFertile", 0) == 0)

                SceneInstance<SylviaScene>.Get().SylviaCaveFirst();
            else if (DoloresProg == 1)
                SceneInstance<DoloresScene>.Get().DoloresPostBirth();
            else if (DoloresProg == 2 && DoloresTime > 72)
                SceneInstance<DoloresScene>.Get().DoloresToys();
            else if (DoloresProg == 3 && DoloresTime > 72)
                SceneInstance<DoloresScene>.Get().DoloresTalking();
            else if (DoloresProg == 4 && DoloresTime > 72)
                SceneInstance<DoloresScene>.Get().DoloresMagic();
            else if (DoloresProg == 6 && DoloresTime > 24)
                SceneInstance<DoloresScene>.Get().DoloresPreCocoon();
            else if (DoloresProg == 7)
                SceneInstance<DoloresScene>.Get().DoloresCocoon();
            else if (DoloresProg == 8 && DoloresTime > 144)
                SceneInstance<DoloresScene>.Get().DoloresHatches();
            else if (DoloresProg == 12)
                SceneInstance<DoloresScene>.Get().DoloresTalkAfter();
            else if (DoloresProg == 13 && DoloresTime > 48)
                SceneInstance<DoloresScene>.Get().DoloresTapestryMaking();
            else if (DoloresProg >= 17 && DoloresTime > 72 && !Save.GetBool("goneCamping")
                && View.Time.IsTimeBetween(17, 22))

                Campfire();
            else
                CaveMenu(true);
        }

        public void CaveMenu(bool output = false)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Mothcave.CaveMenu.MakesWayHome".Localized((SceneInstance<SylviaScene>.Get().sylviaProg > 4 || doloresScene.doloresProg > 5 ? "quite " : "")));

                if (DoloresProg == 9)
                    OutputText("Scenes.Mothcave.CaveMenu.MainRoomEmpty".Localized());
                else
                    OutputText("Scenes.Mothcave.CaveMenu.SpotsSylvia".Localized(
                        DoloresProg > 1 ? ", and Dolores is probably in her own, if you'd like to " + (DoloresProg > 3 && DoloresProg != 8 ? "talk with" : "see") + " her" : ""));
            }

            ClearButtons();

            if (DoloresProg == 9 && DoloresTime > 36)
                AddButtonDisabled(0, "Sylvia", "She isn't here right now.", "Sylvia");
            else
                AddButton(0, "Npcs.Sylvia.Name".Localized(),

                    () => SceneInstance<SylviaScene>.Get().ExecEncounter(),

                    "Talk to your moth lover.");

            if (DoloresProg > 1)

                if (DoloresSave.GetBool("doloresAngry"))
                    AddButtonDisabled(1, "Dolores", "Maybe you should give her some time to cool off.", "Dolores");
                else
                    AddButton(1, "Common.Button.Dolores".Localized(),

                        () => SceneInstance<DoloresScene>.Get().EncounterDolores(),

                        "Spend some time with your daughter.");

            AddButton(2, "Common.Button.Tapestries".Localized(), TapestryMenu, "Check out the silk tapestries on the cave walls.");

            AddButton(3, "Common.Button.Read".Localized(), CaveRead, "Have a look at one of Sylvia's books.");

            if (View.Time.Hours < 21)
                AddButtonDisabled(4, "Sleep Over", "It's too early for that.", "Sleep Over");
            else
                AddButton(4, "Common.Button.SleepOver".Localized(), SleepOver,

                    "Spend the night with the " + (DoloresProgress > 1 ? "elder " : "") + "moth-girl.");

            if (Player.CanFly() && DoloresProgress > 9)
                AddButton(5, "Common.Button.FamilyFlight".Localized(), MothFlight, "Spend some time soaring through the skies with your moth family.");

            SetExitButton("Common.Button.Back".Localized());
        }

        public void TapestryMenu()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.TapestryMenu.SilkTapestries".Localized());

            if (Save.GetInt("tapestryChange", 0) == 2)
                OutputText("Scenes.Mothcave.TapestryMenu.SeemsDifferent".Localized());
            else
                if (DoloresProg > 14)
                    OutputText("Scenes.Mothcave.TapestryMenu.DoloresBeside".Localized());
                else
                    if (DoloresProg > 0)
                        OutputText("Scenes.Mothcave.TapestryMenu.EmptySpace".Localized());
                    else
                        if (Save.GetInt("tapestryChange", 0) == 3)
                            OutputText("Scenes.Mothcave.TapestryMenu.MarredTapestry".Localized());

            OutputText("Scenes.Mothcave.TapestryMenu.SentencePeriod".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Admire".Localized(), AdmireTapestries);

            AddNextButton("Npcs.Sylvia.Name".Localized(), SylviaTapestry);

            if (DoloresProg > 14)
                AddNextButton("Common.Button.Dolores".Localized(), DoloresTapestry);

            AddButton(14, "Common.Button.Back".Localized(), () => CaveMenu());
        }

        public void AdmireTapestries()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.AdmireTapestries.AdmiringCrafts".Localized());

            OutputText("Scenes.Mothcave.AdmireTapestries.PullsUpChair".Localized());

            OutputText("Scenes.Mothcave.AdmireTapestries.SylviaJoins".Localized((SceneInstance<SylviaScene>.Get().sylviaGetDom < 50 ? "calming, supportive, her warmth a reminder that she'll always be there for you" : "calming, reassuring, a reminder that you'll always have a place in her arms")));

            OutputText("Scenes.Mothcave.AdmireTapestries.YouAgree".Localized());

            OutputText("Scenes.Mothcave.AdmireTapestries.GetsUpStretches".Localized((doloresScene.doloresProg > 3 && doloresScene.doloresProg != 8 ? "On your way out of the cave, you spot your daughter reading a book by the shelf, and you wave to her before exiting" : "You give Sylvia one last wave before exiting the cave")));

            OutputText("Scenes.Mothcave.AdmireTapestries.BogAir".Localized(
                View.Time.Hours < 19 ? "Night has fallen over the swamp during your time in the cave, snuffing out the few shafts of light peeking through the foliage" : "The position of the " + (View.Time.Hours > 18 ? "moon" : "sun") + " has shifted significantly since you entered the cave"));

            Player.ChangeFatigue(-20);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            DoNext(() => ReturnToCamp(4));
        }

        public void SylviaTapestry()
        {
            ClearOutput();

            if (Save.GetInt("tapestryChange", 0) > 1)
            {
                OutputText("Scenes.Mothcave.SylviaTapestry.InspectedBefore".Localized((Player.StartingRace != "human" ? " used to be" : "")));

                OutputText("Scenes.Mothcave.SylviaTapestry.CenterpieceDifferent".Localized());

                if (Save.GetInt("tapestryChange", 0) == 2)
                    Save.SetInt("tapestryChange", 3);
            }
            else
            {
                OutputText("Scenes.Mothcave.SylviaTapestry.TapestryHangs".Localized((Player.StartingRace != "human" ? " used to be" : "")));

                OutputText("Scenes.Mothcave.SylviaTapestry.SylviaRecreation".Localized((Player.Corruption < 30 ? ", sending a sympathetic pang through your heart" : "")));

                if (Save.GetInt("tapestryChange", 0) < 1)
                    Save.SetInt("tapestryChange", 1);
            }

            DoNext(() => TapestryMenu());
        }

        public void DoloresTapestry()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.DoloresTapestry.FreshTapestry".Localized((doloresScene.saveContent.doloresTimesLeft < 2 ? "'re filled with the memories of all of the time you've spent with Dolores" : " feel a small pang of regret at having missed so many moments come unbidden to your heart")));

            OutputText("Scenes.Mothcave.DoloresTapestry.LeftSideScenes".Localized((doloresScene.saveContent.doloresTimesLeft < 2 ? "but you've known her long enough to tell that she's not upset" : "and you can't quite understand what she's feeling")));

            OutputText("Scenes.Mothcave.DoloresTapestry.RightSideImages".Localized((doloresScene.saveContent.doloresDecision == 1 ? "That old book features prominently in these scenes, a constant companion to her experimentation" : "However, there's a slight sadness in her face, and you feel an incredible sense of longing coming from the fabric")));

            OutputText("Scenes.Mothcave.DoloresTapestry.CenterpieceOdd".Localized((doloresScene.saveContent.doloresFinal % 10 != 1 ? "strongly reminds you of the thing you encountered in the clearing" : "sends a chill down your spine, though you don't know why"), (doloresScene.saveContent.doloresFinal % 10 == 2 ? "stoic" : "melancholic")));

            OutputText("Scenes.Mothcave.DoloresTapestry.ConsideringWhole".Localized());

            DoNext(() => TapestryMenu());
        }

        public void CaveRead()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CaveRead.EspiesBookshelf".Localized((doloresScene.doloresProg > 5 ? "Well, you know how much enjoyment your daughter's gotten out of it" : "Sylvia seems to have built up quite the collection")));

            OutputText("Scenes.Mothcave.CaveRead.BeMyGuest".Localized());

            OutputText("Scenes.Mothcave.CaveRead.DirectsChair".Localized());

            OutputText("Scenes.Mothcave.CaveRead.NearlyOverwhelmed".Localized());

            switch (Random.Shared.Next(5))
            {
                case 0:
                {
                OutputText("Scenes.Mothcave.CaveRead.VolumeTambow".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Mothcave.CaveRead.BlowsDust".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Mothcave.CaveRead.ShortPoems".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Mothcave.CaveRead.OlensbainTome".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Mothcave.CaveRead.EdgeOfShelf".Localized());
                }

                break;
            }

            OutputText("Scenes.Mothcave.CaveRead.SettlesIn".Localized());

            OutputText("Scenes.Mothcave.CaveRead.SylviaStaring".Localized());

            OutputText("Scenes.Mothcave.CaveRead.SheAsks".Localized());

            OutputText("Scenes.Mothcave.CaveRead.TellsImpressions".Localized());

            if (Player.Intelligence < 50)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void MothFlight()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.MothFlight.IdeaJumps".Localized());

            OutputText("Scenes.Mothcave.MothFlight.LookingCurious".Localized());

            OutputText("Scenes.Mothcave.MothFlight.ThreeHaveWings".Localized());

            OutputText("Scenes.Mothcave.MothFlight.GreatIdea".Localized(
                Player.Mf("father", "parents")
            ));

            OutputText("Scenes.Mothcave.MothFlight.OneMothOnboard".Localized());

            OutputText("Scenes.Mothcave.MothFlight.MultitudeEmotions".Localized());

            OutputText("Scenes.Mothcave.MothFlight.NoProblem".Localized());

            OutputText("Scenes.Mothcave.MothFlight.NarrowsEyes".Localized(Player.Mf("father", "parents")));

            OutputText("Scenes.Mothcave.MothFlight.FunExcursion".Localized());

            DoNext(() => MothFlight2());
        }

        public void MothFlight2()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.MothFlight2.AllThreeExit".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.StammersWhatDo".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.SylviaShoots".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.CallsDown".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.TakesHand".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.BitAwkward".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.AsksDolores".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.WhyFly".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.NoTimeStare".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.MotherLeaps".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.StartsToTire".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.YouWereWonderful".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.ThankYouMother".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.FlightDistance".Localized());

            OutputText("Scenes.Mothcave.MothFlight2.EnjoyedItLot".Localized(
                Player.Mf("", " Or, ah... You.")
            ));

            OutputText("Scenes.Mothcave.MothFlight2.RespondsKind".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Campfire()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.Campfire.StepsIntoCave".Localized());

            OutputText("Scenes.Mothcave.Campfire.AwarePresence".Localized());

            OutputText("Scenes.Mothcave.Campfire.HelloDear".Localized());

            OutputText("Scenes.Mothcave.Campfire.SmilesExpectantly".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), CampfireYes);

            AddButton(1, "Common.Button.No".Localized(), CampfireNo);
        }

        public void CampfireYes()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireYes.HappyToJoin".Localized());

            OutputText("Scenes.Mothcave.CampfireYes.PullsBack".Localized());

            OutputText("Scenes.Mothcave.CampfireYes.TakesCareThat".Localized());

            OutputText("Scenes.Mothcave.CampfireYes.NotSureDolores".Localized());

            OutputText("Scenes.Mothcave.CampfireYes.NotEncouraging".Localized());

            OutputText("Scenes.Mothcave.CampfireYes.WouldLoveTo".Localized());

            OutputText("Scenes.Mothcave.CampfireYes.PrimNod".Localized((SceneInstance<SylviaScene>.Get().unlockedOyakodon ? "If this is some elaborate setup for something weird, I'll be very cross." : "I don't actually know what you're supposed to do for these...")));

            OutputText("Scenes.Mothcave.CampfireYes.ReassuresHer".Localized());

            DoNext(() => Campfire2());
        }

        public void CampfireNo()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireNo.NotInMood".Localized());

            OutputText("Scenes.Mothcave.CampfireNo.NeverMind".Localized());

            CaveMenu();
        }

        public void Campfire2()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.Campfire2.LimbsAche".Localized((Player.Strength > 70 || Player.Tallness > 78 ? "You're able to get it all back in one trip, and" : "It takes a few trips to get it all back, but")));

            OutputText("Scenes.Mothcave.Campfire2.HelloFather".Localized());

            OutputText("Scenes.Mothcave.Campfire2.NearlyJumps".Localized());

            OutputText("Scenes.Mothcave.Campfire2.GatheredThings".Localized());

            OutputText("Scenes.Mothcave.Campfire2.HoldingIncense".Localized((SceneInstance<SylviaScene>.Get().unlockedOyakodon ? "--no, no 't'--" : " ")));

            OutputText("Scenes.Mothcave.Campfire2.SylviaFlits".Localized());

            OutputText("Scenes.Mothcave.Campfire2.TookLonger".Localized());

            OutputText("Scenes.Mothcave.Campfire2.FlipsLid".Localized());

            OutputText("Scenes.Mothcave.Campfire2.HaveToWait".Localized());

            OutputText("Scenes.Mothcave.Campfire2.LetMeMother".Localized());

            OutputText("Scenes.Mothcave.Campfire2.NoObjections".Localized());

            OutputText("Scenes.Mothcave.Campfire2.AllSorted".Localized());

            OutputText("Scenes.Mothcave.Campfire2.FineSuggestion".Localized());

            OutputText("Scenes.Mothcave.Campfire2.SettleAround".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Marshmallows".Localized(), CampfireMarshmallows);

            AddNextButton("Common.Button.Reading".Localized(), CampfireReading);

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), CampfireCuddle);

            AddNextButton("Common.Button.StealthService".Localized(), CampfireService);

            AddNextButton("CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), CampfireDone);
        }

        public void CampfireMarshmallows()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireMarshmallows.GooeyMorsels".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.TakeAsMany".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.OutHuntingWood".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.BranchRightThere".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.SpearAndHold".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.CookedEnough".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.CrispyOutside".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.EyesClosed".Localized());

            OutputText("Scenes.Mothcave.CampfireMarshmallows.OlderMothGiggles".Localized());

            Player.RefillHunger(20);

            AddNextButtonDisabled("Marshmallows");
        }

        public void CampfireReading()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireReading.DoloresQuiet".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.MyPoem".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.AffirmativeNod".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.BlowingBreeze".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.ForestSilence".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.FadingLight".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.WearyViolence".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.SinkingSun".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.HeavensSoften".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.FirmamentVast".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.SightISeek".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.LoftyStars".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.UntouchedEternal".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.EndlessLove".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.BlackSwaths".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.AllIsQuiet".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.BrowScrunches".Localized());

            OutputText("Scenes.Mothcave.CampfireReading.OwnThoughts".Localized());

            AddNextButtonDisabled("Reading");
        }

        public void CampfireCuddle()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireCuddle.ColdOutHere".Localized());

            OutputText("Scenes.Mothcave.CampfireCuddle.CoosGently".Localized());

            OutputText("Scenes.Mothcave.CampfireCuddle.SoNice".Localized());

            OutputText("Scenes.Mothcave.CampfireCuddle.SoftMurr".Localized());

            OutputText("Scenes.Mothcave.CampfireCuddle.DiscomfortBanished".Localized());

            Player.ChangeFatigue(-20);

            AddNextButtonDisabled("Cuddle");
        }

        public void CampfireService()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireService.CloseToMoth".Localized());

            OutputText("Scenes.Mothcave.CampfireService.SylviaUnderstands".Localized());

            OutputText("Scenes.Mothcave.CampfireService.AnswerUncertainty".Localized());

            OutputText("Scenes.Mothcave.CampfireService.IntentFurther".Localized());

            OutputText("Scenes.Mothcave.CampfireService.QuiteAccommodating".Localized());

            OutputText("Scenes.Mothcave.CampfireService.PrecumLeaks".Localized());

            OutputText("Scenes.Mothcave.CampfireService.LightMurmur".Localized());

            OutputText("Scenes.Mothcave.CampfireService.AreYouCold".Localized());

            OutputText("Scenes.Mothcave.CampfireService.YoungerMothLooks".Localized());

            OutputText("Scenes.Mothcave.CampfireService.NoMotherThanks".Localized());

            OutputText("Scenes.Mothcave.CampfireService.SylviaChooses".Localized());

            OutputText("Scenes.Mothcave.CampfireService.MothPresses".Localized());

            OutputText("Scenes.Mothcave.CampfireService.SylviaPurrs".Localized());

            OutputText("Scenes.Mothcave.CampfireService.ShudderLetGo".Localized());

            OutputText("Scenes.Mothcave.CampfireService.WhispersEncouragement".Localized());

            OutputText("Scenes.Mothcave.CampfireService.BecomesAware".Localized());

            Player.Orgasm("DickVaginal");

            AddNextButtonDisabled("Stealth Service");
        }

        public void CampfireDone()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.CampfireDone.FireDwindles".Localized());

            OutputText("Scenes.Mothcave.CampfireDone.SylviaMoves".Localized());

            OutputText("Scenes.Mothcave.CampfireDone.LooksAtDaughter".Localized());

            OutputText("Scenes.Mothcave.CampfireDone.SuddenSnore".Localized());

            OutputText("Scenes.Mothcave.CampfireDone.NuzzlesIntoYou".Localized());

            OutputText("Scenes.Mothcave.CampfireDone.PutOutFire".Localized());

            OutputText("Scenes.Mothcave.CampfireDone.LastSmile".Localized());

            Save.SetBool("goneCamping", true);

            DoNext(() => ReturnToCamp(2));
        }

        public void SleepOver()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.SleepOver.GrowingLate".Localized());

            OutputText("Scenes.Mothcave.SleepOver.CatchesYawn".Localized());

            OutputText("Scenes.Mothcave.SleepOver.GladStayed".Localized());

            if (Player.IsNaked())
                OutputText("Scenes.Mothcave.SleepOver.AllReady".Localized());
            else
                OutputText("Scenes.Mothcave.SleepOver.NotReadyYet".Localized());

            OutputText("Scenes.Mothcave.SleepOver.QuicklyComforted".Localized());

            OutputText("Scenes.Mothcave.SleepOver.EyesStruggle".Localized());

            DoNext(() => SleepOver2());
        }

        public void SleepOver2()
        {
            ClearOutput();

            OutputText("Scenes.Mothcave.SleepOver2.BeginsToWake".Localized());

            OutputText("Scenes.Mothcave.SleepOver2.FlutterOpen".Localized());

            if (DoloresProgress >= 9)
            {
                OutputText("Scenes.Mothcave.SleepOver2.RandChoice".Localized(
                    RandomUtils.Choice(
                "However, a call from behind startles you out of this reverie. [say:Mo-o-other,] comes the scratchy voice, [say:do you know where the--] Two purple eyes widen when they catch sight of you, your daughter standing before you with nothing but a towel to cover her. [say:Oh, you're here. I mean, g-good to see you. Or, ahem. Good morning, [Father], I--] As her arm raises in greeting, the towel dips dangerously, and Dolores dashes back to her room with a shriek.",

                "This reverie is shattered when a loud crash sounds from somewhere else in the cave. Your head whirls around, and as best you can tell, it came from the back hallway, but now there's only silence. A few beats of your heart, and the followup comes: [say:Oh, blasted-- Sodding-- [b:Drat!]] in your daughter's angry voice.",

                "You look up from your reverie at a soft humming sound that slowly grows louder and clearer as you listen. Eventually, it rounds the corner, and its source is revealed to be your daughter. Her dark hair is incredibly tangled and frizzy, to the point that you can barely even see her sunken eyes. She doesn't seem to notice or react to your presence as she slips by, grabs a little blue book from a shelf, and returns from whence she came."
            )));

                OutputText("Scenes.Mothcave.SleepOver2.TurnToSylvia".Localized());
            }
            else
            {
                OutputText("Scenes.Mothcave.SleepOver2.StillSlumbers".Localized());

                OutputText("Scenes.Mothcave.SleepOver2.NoSignWaking".Localized());
            }

            OutputText("Scenes.Mothcave.SleepOver2.DeepSleeper".Localized());

            OutputText("Scenes.Mothcave.SleepOver2.SleepyGreeting".Localized((SceneInstance<DoloresScene>.Get().doloresProg >= 5 ? "dear, I'll tell you when you're older" : "a few more minutes, Mom")));

            OutputText("Scenes.Mothcave.SleepOver2.RubsEyes".Localized());

            OutputText("Scenes.Mothcave.SleepOver2.ArisesAndDresses".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((30 - View.Time.Hours) * 60)));

            SceneInstance<SylviaScene>.Get().SylviaMenu();
        }

    }
}