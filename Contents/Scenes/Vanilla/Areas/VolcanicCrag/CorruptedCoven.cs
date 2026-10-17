using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Achievements;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag
{
    [Register<CorruptedCoven>("coc:corrupted_coven")]
    public class CorruptedCoven : Scene
    {

        public bool metCirceAsGrossInsectPerson = false;

        public bool sippedWineWithCirce = false;

        public const int MET_CIRCE = 1 << 0;

        public const int TOLD_NAME = 1 << 1;

        public const int TALKED_MARAE_BLESS = 1 << 2;

        public const int TALKED_FERA_BLESS = 1 << 3;

        public const int TALKED_JEREMIAH_WEAPONS = 1 << 4;

        public const int TALKED_LAURENTIUS_INCIDENT = 1 << 5;

        public const int TALKED_MANOR_DULLAHAN = 1 << 6;

        public const int TALKED_CORRWITCH_HEX = 1 << 7;

        public const int TALKED_TELADRE_WIZARDS = 1 << 8;

        public const int TALKED_GARGOYLE = 1 << 9;

        public const int TALKED_SANDWITCH_BLESS = 1 << 10;

        public const int TALKED_KITSUNE_ENLIGHTEMENT = 1 << 11;

        public const int TALKED_DOMINIKA_SWORD = 1 << 12;

        public const int TALKED_NAMELESS_HORROR = 1 << 13;

        public const int CAUGHT_ALCHEMY = 1 << 14;

        public const int GOT_RING_OF_ETHEREAL_TEARING = 1 << 15;

        public const int GOT_MAJOR_RING_OF_ACCURACY = 1 << 16;

        public const int GOT_SPECTRE_RING = 1 << 17;

        public const int BROUGHT_JEREMIAH_BACK = 1 << 18;

        public const int HAD_SEX_ONCE = 1 << 19;

        public const int BROUGHT_DULLAHAN = 1 << 20;

        public const int RETURNED_DULLAHAN_POSTDREAM = 1 << 21;

        public const int TALKED_BEAUTIFUL_SWORD = 1 << 22;

        public const int BROUGHT_MANOR_BOOKS = 1 << 23;

        public static SaveContent Save => SaveContent(SceneIds.VolcanicCrag.CorruptedCoven);

        public string debugName => "Circe";

        public string debugHint => "";

        public override void Entry()
        {
            CirceEvelynEncounter();
        }

        public void Reset()
        {

            Save.SetBool("metCirceAsGrossInsectPerson", false);

            Save.SetBool("sippedWineWithCirce", false);
        }

        public bool CirceAversion()
        {

            return Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.BroBrains) || Player.HasPerk(PerkIds.FutaFaculties) || (Save.GetBool("metCirceAsGrossInsectPerson") && IsSufficientlyBuggy());
        }

        public bool IsSufficientlyBuggy()
        {

            return Player.BeeScore() + Player.SpiderScore() > 4;
        }

        public bool CirceUnlockable()
        {

            return !((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & MET_CIRCE) != 0) && !CirceAversion() && (Player.Level > 12 || View.Time.Days > 95);
        }

        public bool CirceEnabled()
        {

            return (CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & MET_CIRCE) != 0;
        }

        public void EncounterCirceCave()
        {
            ClearOutput();

            if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & MET_CIRCE) != 0)
            {
                ReencounterCirce();

                return ;
            }

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave.SomethingCatchesYourEye".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave.CuriosityGetsTheBetter".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave.ApproachToCaveIs".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave.ThreeSecondsInside".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                OutputText("Scenes.Corruptedcoven.EncounterCirceCave.CastMagefireOn".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave.DoesntTakeLongFor".Localized());

            DoNext(() => EncounterCirceCave2());
        }

        public void EncounterCirceCave2()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave2.BodyShiftSuddenly".Localized());

            DoNext(() => EncounterCirceCave3());
        }

        public void EncounterCirceCave3()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.SmellSteamBurningCoal".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.SoftFirmVoice".Localized());

            if (IsSufficientlyBuggy())
            {
                OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.VoiceTrailsOff".Localized());

                DoNext(() => EncounterCirceInsectProblem());

                return ;
            }

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.HearEyesAdapting".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.FinallySeeingClearly".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.StandUprightLook".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.SittingOnWoodChair".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.SaysEyesNarrowing".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceCave3.NoHostilityInHer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Exploring".Localized(), () => EncounterCirceAnswer(0));

            AddButton(1, "Common.Button.Sex".Localized(), () => EncounterCirceAnswer(1));

            if (CoC.Instance.Flags.Get(GameFlagIds.Metvolcanicgolem) > 0 && CoC.Instance.Flags.Get(GameFlagIds.Destroyedvolcanicgolem) == 0)
                AddButton(2, "Common.Button.Golem".Localized(), () => EncounterCirceAnswer(2));

            if (Player.HasKeyItem("Poorly done map to volcanic crag"))
                AddButton(3, "Common.Button.Tower".Localized(), () => EncounterCirceAnswer(3));

            if (CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) == 0)
                AddButton(4, "Common.Button.Lethice".Localized(), () => EncounterCirceAnswer(4));
        }

        public void EncounterCirceInsectProblem()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.EncounterCirceInsectProblem.FeelTheHeatOf".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceInsectProblem.AfterFewMomentsDeep".Localized());

            Save.SetBool("metCirceAsGrossInsectPerson", true);

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterCirceAnswer(int answer = 0)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TellHerJust".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TellHerLooking".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TellHerFound".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TellHerYouHave".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TellHerHaveA".Localized());
                }

                break;
            }

            OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TakesHandFromChin".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.TryGrabHerArm".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceAnswer.LookAroundFind".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) + MET_CIRCE);

            DoNext(() => ReturnToCamp(1));
        }

        public void ReencounterCirce()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.ReencounterCirce.MetWithFamiliar".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ReenterCave".Localized(), EncounterCirceRepeat);

            AddButton(1, "Scenes.Kitsune.WillOWisp.ButtonTurnBack".Localized(), () => ReturnToCamp(1));
        }

        public void CirceIntros()
        {
            ClearOutput();

            if (IsSufficientlyBuggy())
            {
                OutputText("Scenes.Corruptedcoven.CirceIntros.CirceSittingAtTable".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.LetsGoOfTome".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.AnotherStepForward".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.EyesDashingBackForth".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.TryToAskWhat".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.SlumpShouldersConfused".Localized());

                Save.SetBool("metCirceAsGrossInsectPerson", true);

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (!((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & TOLD_NAME) != 0))
            {
                OutputText("Scenes.Corruptedcoven.CirceIntros.CirceSittingTableAgain".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.TrailsOffYouThink".Localized());

                if (Player.Intelligence >= 90)
                    OutputText("Scenes.Corruptedcoven.CirceIntros.AskHerIfThats".Localized());
                else
                    OutputText("Scenes.Corruptedcoven.CirceIntros.TellHerItsA".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.StretchesHandAnd".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.BeginInterjecting".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.ThinkAboutItPrepare".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.DefinitelyFrustrating".Localized());

                OutputText("Scenes.Corruptedcoven.CirceIntros.BeginHeadBack".Localized());

                InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.LesserFocusRing), () => ReturnToCamp(1));

                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) + TOLD_NAME);
            }
            else
            {
                switch (Random.Shared.Next(2))
                {
                    case 0:
                    {
                    GenericCirceIntros();
                    }

                    break;

                    case 1:
                    {
                    if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) > 8 && Random.Shared.Next(3) == 0)
                    {
                        SipWineWithCirce();

                        return ;
                    }

                    if (!((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & CAUGHT_ALCHEMY) != 0))
                    {
                        CatchCirceOnAlchemy();

                        return ;
                    }
                    else
                    {
                        OutputText("Scenes.Corruptedcoven.CirceIntros.SeeCirceOnAlchemy".Localized());

                        OutputText("Scenes.Corruptedcoven.CirceIntros.AfterAgonizingWait".Localized());
                    }
                    }

                    break;
                }

                BuildOptionsMenu(true);
            }
        }

        public void SipWineWithCirce()
        {

            OutputText("Scenes.Corruptedcoven.SipWineWithCirce.MaterializeInsideChamber".Localized());

            OutputText("Scenes.Corruptedcoven.SipWineWithCirce.HearFromYourRight".Localized());

            OutputText("Scenes.Corruptedcoven.SipWineWithCirce.AsksBeforeUncorking".Localized());

            OutputText("Scenes.Corruptedcoven.SipWineWithCirce.DoDrinkTheWine".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), () => AnswerWine(1));

            AddNextButton("Common.Button.No".Localized(), () => AnswerWine(2));

            AddNextButton("Common.Button.Religious".Localized(), () => AnswerWine(3));

            AddNextButton("Scenes.Options.Setting.Underage".Localized(), () => AnswerWine(4));
        }

        public void AnswerWine(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 1:
                {
                OutputText("Scenes.Corruptedcoven.AnswerWine.AcceptAndSheHands".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Corruptedcoven.AnswerWine.TellHerYouDont".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.SaysLookingAtRejected".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.SwirlsContentsOfHer".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Corruptedcoven.AnswerWine.TellHerYoureSorry".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.SaysMixOfConfusion".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.RemainResoluteAndSay".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.LooksAtSecondGlass".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Corruptedcoven.AnswerWine.RelayWereTold".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.AssertThatYouWill".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.RemainsSilentCouple".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerWine.WhispersToHerself".Localized());
                }

                break;
            }

            OutputText("Scenes.Corruptedcoven.AnswerWine.TelepathicallyCorks".Localized());

            OutputText("Scenes.Corruptedcoven.AnswerWine.WithWaveHerHand".Localized());

            OutputText("Scenes.Corruptedcoven.AnswerWine.CuriosityPeaked".Localized());

            if (answer == 1)
                OutputText("Scenes.Corruptedcoven.AnswerWine.StillMovingGoodStory".Localized());
            else
                OutputText("Scenes.Corruptedcoven.AnswerWine.StillMovingOnOffended".Localized());

            OutputText("Scenes.Corruptedcoven.AnswerWine.PrettySureTheStories".Localized());

            Save.SetBool("sippedWineWithCirce", true);

            BuildOptionsMenu();
        }

        public void GenericCirceIntros()
        {

            var opts = new List<string>
            {
                "Circe was apparently ready for your arrival, as she sits on the heavy wooden chain in the middle of the chamber, legs crossed, analyzing you. [say: So, [name], what do you have for me?]",

                "You arrive to see Circe at her study, dozens of books open in front of her and some floating around her, slowly bobbing in mid-air. She notices your presence, and, with a wave of her hand, all the books suddenly close and stack themselves neatly on her table. She tidies up her hair as she reaches the center of the chamber to welcome you.[pg][say: Hello, [name]. What do you have for me?]"
            };

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) > 2 && CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) < 5)
                opts.Add("Circe was apparently ready for your arrival, as she sits on the heavy wooden chain in the middle of the chamber, legs crossed, analyzing you. [say: Well, this is working out better than I expected. Assuming you have something new for me, that is. Welcome, [name].]");

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) > 4)
            {
                opts.Add("You materialize inside the chamber and notice Circe is not in her usual spot. It doesn't take long to find her, however, as she's simply peering over some of the artifacts she keeps on a shelf on the edge of the chamber. She waves a hand, acknowledging your presence, then turns around to face you. [say: You're quite reliable, aren't you? Come on, get seated[if (!isbiped) {-well, if you can-}] and let's see what you have for me.]");

                opts.Add("Circe was apparently ready for your arrival, as she sits on the heavy wooden chain in the middle of the chamber, legs crossed, analyzing you. [say: Never a dull moment with you, isn't that right, [name]? Stumbled upon anything interesting recently?]");
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) > 6)
                opts.Add("Circe is in her usual spot in the center of the chamber, and you notice the barest hint of a smile on her face as she idly swirls the contents of her wine glass, looking at you. [say: I'm beginning to think this was a mistake. I barely have time alone these days.][pg]The idea that she's being genuine crosses your mind for a second, but she closes her eyes and lightly shakes her head, signaling that it was, indeed, a joke. Her reaction to your hesitation was so quick that another thought hits you: she might have just read your mind in that moment.[pg][say: Well, what are you waiting for? Get comfortable and let's chat; we always seem to end up having thought-provoking conversations,] she says, with a genuine look of barely-subdued excitement.[pg]It seems she trusts you to speak your mind by yourself, you think.");

            if (Save.GetBool("sippedWineWithCirce"))
                opts.Add("You materialize inside the chamber and immediately spot something floating in front of your eyes. After taking a brief moment to focus, you notice it's another glass of wine; by the aroma, it's the same type Circe offered you before. You take it in your hand, the glass becoming heavier as the telekinesis acting upon it fades. [say: Don't be shy, drink up,] Circe says, smiling softly. [say: I've got a bit of a head start myself.][pg]You smile and sip on the wine, taking some time to appreciate its flavor while thinking about what information you could share with her. These meetings feel quite a bit more casual nowadays.");

            OutputText(opts[Random.Shared.Next(opts.Count)]);
        }

        public void CatchCirceOnAlchemy()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.ArrivalMoreTurbulent".Localized());

            OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.LookAroundChamberFor".Localized());

            OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.CallOutButShe".Localized());

            if (Player.HasPerk(PerkIds.HistoryAlchemist))
                OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.AskIfSheDoesnt".Localized());
            else
                OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.AskWhatShes".Localized());

            OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.TurnsWideEyed".Localized());

            OutputText("Scenes.Corruptedcoven.CatchCirceOnAlchemy.FinishesLuxuriousEnsemble".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LikeIt".Localized(), () => CommentOnCasualLook(0));

            AddButton(1, "Common.Button.Apologize".Localized(), () => CommentOnCasualLook(1));

            AddButton(2, "Common.Button.Complain".Localized(), () => CommentOnCasualLook(2));
        }

        public void CommentOnCasualLook(int option = 0)
        {
            ClearOutput();

            switch (option)
            {
                case 0:
                {
                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.TellHerYou".Localized());

                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.WhateverYouThink".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.TryToApologize".Localized());

                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.WhateverYouThinkAgain".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.TellHadNo".Localized());

                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.StrikeANerve".Localized());

                OutputText("Scenes.Corruptedcoven.CommentOnCasualLook.WhateverYouThinkThird".Localized());
                }

                break;
            }

            CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) + CAUGHT_ALCHEMY);

            BuildOptionsMenu();
        }

        public void BuildOptionsMenu(bool clearOutputText = false)
        {
            ClearOutput();

            ClearButtons();

            if (clearOutputText)
                OutputText("Scenes.Corruptedcoven.BuildOptionsMenu.CirceStaresBouncing".Localized());

            AddButton(0, "Common.Button.Knowledge".Localized(), TopicTalkOptions);

            AddButton(1, "Common.Button.Explore".Localized(), ExploreChamber);

            AddButton(2, "Common.Button.Appearance".Localized(), CirceAppearance);

            AddButton(3, "Common.Button.Rewards".Localized(), CirceRewards);

            AddButton(4, "Common.Button.Sex".Localized(), CirceSex);

            if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & BROUGHT_JEREMIAH_BACK) != 0)
                AddButton(1, "Common.Button.Jeremiah".Localized(), CirceJeremiah);

            if (Player.HasKeyItem("Old Manor Books"))
                AddButton(2, "Common.Button.GiveBooks".Localized(), GiveManorBooks);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveCirceChamber);
        }

        public void CirceJeremiah()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.SeeJeremiahFar".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.ComeVisitOldHag".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.CirceDidNotHear".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.AskHimHowHes".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.DefinitelyAnImprovement".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.AgreeApproachHim".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.CertainShes".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.NodReflecting".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.JeremiahShakesHead".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.FollowFromHisPoint".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.AsInquisitorShouldBurn".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.AskHasAny".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.NoneNoPointAsking".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.NodBrieflyThinkOf".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.UntilYouThinkAbout".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.HadAWifeOnce".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.ConsiderHisWords".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.GoesSilentAndYou".Localized());

            OutputText("Scenes.Corruptedcoven.CirceJeremiah.SeeYouLaterWaves".Localized());

            DoNext(() => BuildOptionsMenu());
        }

        public void CirceSex()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceSex.AskCirceSheDoesnt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) < 4)
            {
                OutputText("Scenes.Corruptedcoven.CirceSex.RollsEyesAndCuts".Localized(
                     Player.LowMedHighCor("Better to respect her wishes. Maybe she'll change her mind if you show her sex isn't what you come here for.", "A stray violent thought crosses your mind, but you shake it off. Maybe she'll change her mind if you show her sex isn't what you come here for.", "You can barely contain your desire to just attack her and rape her right now, but her hex is still active; fighting her in her chamber would be suicide. Maybe she'll change her mind if you show her sex isn't the only thing in yours.")
                ));

                DoNext(() => BuildOptionsMenu());
            }
            else
            {
                OutputText("Scenes.Corruptedcoven.CirceSex.RubsChinSoftly".Localized());

                OutputText("Scenes.Corruptedcoven.CirceSex.ApproachSheCrosses".Localized());

                OutputText("Scenes.Corruptedcoven.CirceSex.LookIntoEyesAgain".Localized());

                OutputText("Scenes.Corruptedcoven.CirceSex.DeepDownWhatWish".Localized());

                ClearButtons();

                AddButton(0, Player.Mf("King", "Queen") + "Common.Text.Concubine".Localized(), KingAndConcubine);

                AddButton(1, "Common.Button.DemonLord".Localized(), DemonLord);

                AddButton(14, "Common.Button.Back".Localized(), CancelSex);
            }
        }

        public void CancelSex()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CancelSex.TellCirceYoure".Localized());

            OutputText("Scenes.Corruptedcoven.CancelSex.ChamberLightsUpAgain".Localized());

            BuildOptionsMenu();
        }

        public void LadysKnight()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.LadysKnight.VeryWellThen".Localized());
        }

        public void TavernWench()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.TavernWench.VeryWellThen".Localized());
        }

        public void DemonLord()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.DemonLord.WallsAndFloor".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord.PlatemailBikiniImmaculate".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord.GetUpFromThrone".Localized());

            DoNext(() => DemonLord2());
        }

        public void DemonLord2()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.DemonLord2.FightLastsSeveralMinutes".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.InFrontYouLies".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.OneFinal".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.WaveYourArmorVanishes".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.GroansTryingAndFailing".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.TellHerToSilence".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.MouthOpenAndHer".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.YellsMusteringDetermination".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.ShrugAdmitting".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.YouAreGoing".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.ThankHerForMercy".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.SitOnThroneAgain".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.RestHeadOnYour".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.TellSuccubusSwallowing".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.SuccubusRidingOrgasm".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.DropsHerSword".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.ThankSlaveForHer".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.ClimbOverEachOther".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.AddledDrippingCirce".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.CongratulateOnFighting".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.SaysShufflingTowards".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.PositionDemonicCockhead".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.TellHerToAsk".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.GiveMeYourCock".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.LaughAndPlunge".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.HoldHerLegsUp".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.ScreamsBodyTensing".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.CrazedScreamsTurnInto".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.PowerfulEnoughAsYou".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.SucksAsOnly".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.SighsEnjoyingDemonic".Localized());

            OutputText("Scenes.Corruptedcoven.DemonLord2.NotLongYouThink".Localized());

            Player.Orgasm("Dick", true, 2);

            DoNext(() => PostCirceSex());
        }

        public void KingAndConcubine()
        {
            ClearOutput();

            var tempCock = false;

            if (!Player.HasCock())
            {
                Player.CreateCock(7, 1.5f, BodyPartTypes.Human);

                tempCock = true;
            }

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.VeryWellThen".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.WallsAndFloor".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.VisionClearsToSight".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.LeftmostGirlGrasps".Localized(
                 Player.Mf("King", "Queen")
            ));

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.NodsObedientlyBrings".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.MinistrationsBecomeToo".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.OrgasmPowerfulProlonged".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.TwoOtherConcubinesHelp".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.OrgasmInTimeShould".Localized(
                 Player.Mf("King", "Queen")
            ));

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.NoOfCourseNot".Localized(
                 Player.Mf("King", "Queen")
            ));

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.WaveHerAway".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.BringAHand".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.SaysDescendingArmrest".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.PullsSkirtAside".Localized(
                 Player.Mf("King", "Queen")
            ));

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.ComplyGrabbingHips".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.GrabsYourCockTeases".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.SlowlyExhalesPushes".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.FinallyHiltsHerself".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.LookNearlyLosing".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.SilenceCirceWithKiss".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.SlowlyLiftsHips".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.ContinuesToThrust".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.SaysPantingDozingOff".Localized(
                 Player.Mf("King", "Queen")
            ));

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.KnowCanStillHang".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.ScreamsLimbsAlternating".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.ClimaxInsideHer".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.DickDeflates".Localized());

            OutputText("Scenes.Corruptedcoven.KingAndConcubine.GentlyBrushHerHair".Localized());

            Player.Orgasm("Dick", true, 2);

            if (tempCock)
                Player.RemoveCock(0, 1);

            DoNext(() => PostCirceSex());
        }

        public void PostCirceSex()
        {
            ClearOutput();

            if (!((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & HAD_SEX_ONCE) != 0))
            {
                OutputText("Scenes.Corruptedcoven.PostCirceSex.ShakeYourHeadYoure".Localized(
                     Player.Mf("King", "Queen")
                ));

                OutputText("Scenes.Corruptedcoven.PostCirceSex.AndEyesClose".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) + HAD_SEX_ONCE);
            }
            else
                OutputText("Scenes.Corruptedcoven.PostCirceSex.FindYourselfBack".Localized());

            View.Time.PassTime(3);

            DoNext(() => BuildOptionsMenu());
        }

        public void CirceRewards()
        {
            ClearOutput();

            int rewardProgress = CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount);

            if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & BROUGHT_MANOR_BOOKS) != 0)
                rewardProgress += 1;

            ClearButtons();

            OutputText("Scenes.Corruptedcoven.CirceRewards.AskCirceSheHas".Localized());

            if (((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & TALKED_GARGOYLE) != 0) && !Player.HasStatusEffect(StatusEffectIds.KnowsCSS))
            {
                OutputText("Scenes.Corruptedcoven.CirceRewards.GargoyleInfoParticularly".Localized());

                AddNextButton("Common.Button.SummonedSwords".Localized(), () => GetRewards(1));
            }

            if (((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & TALKED_MANOR_DULLAHAN) != 0) && !Player.HasStatusEffect(StatusEffectIds.KnowsWither))
            {
                OutputText("Scenes.Corruptedcoven.CirceRewards.NecromancerInfo".Localized());

                AddNextButton("Common.Button.Wither".Localized(), () => GetRewards(2));
            }

            if (((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & TALKED_MARAE_BLESS) != 0) && !Player.HasStatusEffect(StatusEffectIds.KnowsDivineWind))
            {
                OutputText("Scenes.Corruptedcoven.CirceRewards.MaraeBlessingInteresting".Localized());

                AddNextButton("Common.Button.DivineWind".Localized(), () => GetRewards(3));
            }

            if (rewardProgress >= 6 && ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & TALKED_DOMINIKA_SWORD) != 0) && !((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & GOT_RING_OF_ETHEREAL_TEARING) != 0))
            {
                OutputText("Scenes.Corruptedcoven.CirceRewards.CantPracticeMagic".Localized());

                AddNextButton("Common.Button.Ring".Localized(), () => GetRewards(4));
            }

            if (rewardProgress >= 4 && !((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & GOT_MAJOR_RING_OF_ACCURACY) != 0))
            {
                OutputText("Scenes.Corruptedcoven.CirceRewards.BroughtEnoughKnowledge".Localized());

                AddNextButton("Common.Button.Ring".Localized(), () => GetRewards(5));
            }

            if (rewardProgress >= 8 && !((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & GOT_SPECTRE_RING) != 0))
            {
                OutputText("Scenes.Corruptedcoven.CirceRewards.ManyContributionsOne".Localized());

                AddNextButton("Common.Button.Ring".Localized(), () => GetRewards(6));
            }

            if (Buttons.All(b => b is null))
                OutputText("Scenes.Corruptedcoven.CirceRewards.NoRewardsAvailable".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => BuildOptionsMenu());
        }

        public void GetRewards(int reward)
        {
            ClearOutput();

            switch (reward)
            {
                case 1:
                {
                OutputText("Scenes.Corruptedcoven.GetRewards.TellHerInterestedBook".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.HandsYouBook".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.TakeTheBookThanking".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.CantWaitToTry".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.SpellLearnedSummoned".Localized());

                Player.CreateStatusEffect(StatusEffectIds.KnowsCSS, 0, 0, 0, 0);
                }

                break;

                case 2:
                {
                OutputText("Scenes.Corruptedcoven.GetRewards.TellHerInterestedWithering".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.HandsYouWithering".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.TakeScrollThankingWithering".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.SpellLearnedWithering".Localized());

                Player.CreateStatusEffect(StatusEffectIds.KnowsWither, 0, 0, 0, 0);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Corruptedcoven.GetRewards.TellHerInterestedDivine".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.HandsYouDivine".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.TakeScrollThankingDivine".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.SpellLearnedDivineWind".Localized());

                Player.CreateStatusEffect(StatusEffectIds.KnowsDivineWind, 0, 0, 0, 0);
                }

                break;

                case 4:
                {
                OutputText("Scenes.Corruptedcoven.GetRewards.TellHerInterestedRing".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.HandsYouRing".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.PlaceRingAgainst".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) | GOT_RING_OF_ETHEREAL_TEARING);

                InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.TearingRing), CirceRewards, () => { CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) ^ GOT_RING_OF_ETHEREAL_TEARING); CirceRewards(); });

                return ;
                }

                case 5:
                {
                OutputText("Scenes.Corruptedcoven.GetRewards.TellInterestedRingTwo".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.HandsRingTwo".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.PocketRingWondering".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) | GOT_MAJOR_RING_OF_ACCURACY);

                InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.GreaterFocusRing), CirceRewards, () => { CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) ^ GOT_MAJOR_RING_OF_ACCURACY); CirceRewards(); });

                return ;
                }

                case 6:
                {
                OutputText("Scenes.Corruptedcoven.GetRewards.TellInterestedRingThree".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.HandsRingThree".Localized());

                OutputText("Scenes.Corruptedcoven.GetRewards.PocketRingHoping".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) | GOT_SPECTRE_RING);

                InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.SpecterRing), CirceRewards, () => { CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) ^ GOT_SPECTRE_RING); CirceRewards(); });

                return ;
                }
            }

            DoNext(() => BuildOptionsMenu());
        }

        public void ItemNotTaken(int flag)
        {

            CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) ^ flag);

            CirceRewards();
        }

        public void CirceAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceAppearance.SittingOnWoodChair".Localized());

            OutputText("Scenes.Corruptedcoven.CirceAppearance.LegsMostlyBare".Localized());

            DoNext(() => BuildOptionsMenu());
        }

        public void ExploreChamber()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.ExploreChamber.VarietyOfTomes".Localized());

            OutputText("Scenes.Corruptedcoven.ExploreChamber.BedFitForKing".Localized());

            OutputText("Scenes.Corruptedcoven.ExploreChamber.LuxuriousLargeBath".Localized());

            OutputText("Scenes.Corruptedcoven.ExploreChamber.ChamberIlluminatedWith".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Decoration".Localized(), () => RoomTalkAbout(0));

            AddButton(1, "Common.Button.Books".Localized(), () => RoomTalkAbout(1));

            AddButton(2, "Common.Button.AlchemyTable".Localized(), () => RoomTalkAbout(2));

            AddButton(3, "Common.Button.Vanity".Localized(), () => RoomTalkAbout(3));

            AddButton(14, "Common.Button.Back".Localized(), () => BuildOptionsMenu());
        }

        public void RoomTalkAbout(int topic)
        {
            ClearOutput();

            switch (topic)
            {
                case 0:
                {
                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.SlowlyStepAround".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Maybe".Localized(), () => AnswerDecorationTalk(0));

                AddButton(1, "Common.Button.No".Localized(), () => AnswerDecorationTalk(1));

                return ;
                }

                case 1:
                {
                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.MoveTowardsOneOf".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.CollectionTrulyAmazing".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.StartledByCirce".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.StareAtCirce".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.MoveTowardsWell".Localized());

                if (Player.HasPerk(PerkIds.HistoryAlchemist))
                {
                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.TakeAShortPeek".Localized());

                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.TurnAndCasually".Localized());

                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.JustAboutWhatExpected".Localized());

                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.PrettyMuchHerWay".Localized());
                }
                else
                {
                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.TakeShortPeekAgain".Localized());

                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.TurnAndAsk".Localized());

                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.PointOutSheWas".Localized());

                    OutputText("Scenes.Corruptedcoven.RoomTalkAbout.ImmortalityBeyond".Localized());
                }
                }

                break;

                case 3:
                {
                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.CautiouslyMoveTowards".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.VanityIsWhat".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.OnePieceCatchesEye".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.ConsiderHowToAsk".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.KnowWhatYouFound".Localized());

                OutputText("Scenes.Corruptedcoven.RoomTalkAbout.LooksAwayDistant".Localized());
                }

                break;
            }

            DoNext(() => BuildOptionsMenu());
        }

        public void AnswerDecorationTalk(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Corruptedcoven.AnswerDecorationTalk.TellHerIfThere".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerDecorationTalk.MostWouldntConsider".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerDecorationTalk.LeavesYouToThoughts".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Corruptedcoven.AnswerDecorationTalk.TellHerNoThey".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerDecorationTalk.MustBecomeExample".Localized());

                OutputText("Scenes.Corruptedcoven.AnswerDecorationTalk.LeavesThoughtsAgain".Localized());
                }

                break;
            }

            DoNext(() => BuildOptionsMenu());
        }

        public void TopicTalkOptions()
        {

            ClearButtons();

            if (Player.HasMaraeBless())
                AddNextButton("Common.Button.Marae".Localized(), () => TalkAbout(TALKED_MARAE_BLESS));

            if (Player.HasFeraBoon())
                AddNextButton("Common.Button.FerasBoon".Localized(), () => TalkAbout(TALKED_FERA_BLESS));

            if (((CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) & 512) != 0) && !Player.HasKeyItem("Talisman of the Flame"))
                AddNextButton("Common.Button.Inquisitors".Localized(), () => TalkAbout(TALKED_JEREMIAH_WEAPONS));

            if ((CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) & 2048) != 0)
                AddNextButton("Common.Button.Laurentius".Localized(), () => TalkAbout(TALKED_LAURENTIUS_INCIDENT));

            if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0)
                AddNextButton("Scenes.Dullahan.DullMenu.ButtonManor".Localized(), () => TalkAbout(TALKED_MANOR_DULLAHAN));

            if (Player.HasPerk(PerkIds.Revelation))
                AddNextButton("Combat.Ability.DullahanHorror.Name".Localized(), () => TalkAbout(TALKED_NAMELESS_HORROR));

            if (Player.HasPerk(PerkIds.PotentPregnancy) || Player.HasPerk(PerkIds.PotentProstate))
                AddNextButton("Common.Button.CragWitches".Localized(), () => TalkAbout(TALKED_CORRWITCH_HEX));

            if (Player.HasStatusEffect(StatusEffectIds.TelAdre))
                AddNextButton("Common.Button.Teladre".Localized(), () => TalkAbout(TALKED_TELADRE_WIZARDS));

            if (CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) > 0)
                AddNextButton("Scenes.Options.Setting.Gargoyle".Localized(), () => TalkAbout(TALKED_GARGOYLE));

            if (Player.HasPerk(PerkIds.MagicalVirility) || Player.HasPerk(PerkIds.MagicalFertility))
                AddNextButton("Common.Button.SandWitches".Localized(), () => TalkAbout(TALKED_SANDWITCH_BLESS));

            if (Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
                AddNextButton("Common.Button.Kitsunes".Localized(), () => TalkAbout(TALKED_KITSUNE_ENLIGHTEMENT));

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikasSwordGiven) == 1)
                AddNextButton("Common.Button.Dominika".Localized(), () => TalkAbout(TALKED_DOMINIKA_SWORD));

            if (Player.HasItemIncludeEquipped(WeaponIds.BeautifulSword))
                AddNextButton(ItemRegistry.Create(WeaponIds.BeautifulSword).Name, () => TalkAbout(TALKED_BEAUTIFUL_SWORD));

            AddButton(14, "Common.Button.Back".Localized(), () => BuildOptionsMenu());
        }

        public void TalkAbout(int topic)
        {
            ClearOutput();

            switch (topic)
            {
                case TALKED_BEAUTIFUL_SWORD:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerHaveAn".Localized());

                if (Player.Corruption > 50)
                {
                    OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerDespiteThe".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.ThinkAboutIt".Localized());
                }
                else
                    OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerIfAnything".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.MotionsToSheathe".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.DespiteRevulsionWeapon".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.AgreePerhapsBladeWas".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TossesIdeaAround".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerAWeapon".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.HadPowerToSmite".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.LookDownAtSheathed".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.VisageShiftsToA".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerItsDifficult".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.WieldThatSwordYou".Localized());

                if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                {
                    OutputText("Scenes.Corruptedcoven.TalkAbout.RazingWorldToAshes".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.JustRememberNotLet".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.WelcomeHerKnowledge".Localized());
                }
                else
                    OutputText("Scenes.Corruptedcoven.TalkAbout.GripTheScabbard".Localized());
                }

                break;

                case TALKED_MARAE_BLESS:
                {
                if (!Player.HasPerk(PerkIds.PurityBlessing))
                    OutputText("Scenes.Corruptedcoven.TalkAbout.MoreRemarkableEncounters".Localized());
                else
                    OutputText("Scenes.Corruptedcoven.TalkAbout.MoreRemarkableEncountersAgain".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.CantHelpButFeel".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.RubsChinThenSlides".Localized());

                if (Player.HasPerk(PerkIds.HistoryDEUSVULT))
                    OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerThatsExtremely".Localized());
                else
                    OutputText("Scenes.Corruptedcoven.TalkAbout.MaraeLostControl".Localized());
                }

                break;

                case TALKED_FERA_BLESS:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.EncounterWithSister".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.FeraWorseEnemy".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.MentionItsCurious".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.FeraCorruptionDoes".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.ManifestsAsUnrestrained".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.AskWhyFeraHerself".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerYouAwakened".Localized());
                }

                break;

                case TALKED_JEREMIAH_WEAPONS:
                {
                if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & BROUGHT_JEREMIAH_BACK) != 0)
                {
                    OutputText("Scenes.Corruptedcoven.TalkAbout.AskCirceHowShes".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.AskWhatDoesShe".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.SmirkCuriousMatch".Localized());
                }
                else
                {
                    OutputText("Scenes.Corruptedcoven.TalkAbout.TalkAboutYour".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.RestsHeadOnHer".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.GetsUpFromChair".Localized());
                }
                }

                break;

                case TALKED_LAURENTIUS_INCIDENT:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.OrganizeThoughtsAndTry".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.LaurentiusPiquesInterest".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.LaurentiusKnowOfHim".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.NodInUnderstanding".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.NobodyElseSearched".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.NoticeAGlassOf".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerWhatExperienced".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.AskHowSheKnows".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellShesStruggling".Localized());

                UpdateTopic(topic);

                DoNext(() => LeaveCirceChamber());

                return ;
                }

                case TALKED_MANOR_DULLAHAN:
                {
                if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & BROUGHT_DULLAHAN) != 0)
                {
                    OutputText("Scenes.Corruptedcoven.TalkAbout.AskAboutEvelyn".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.SmilesYouNodThanking".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.ScratchHeadThinking".Localized());
                }
                else
                {
                    OutputText("Scenes.Corruptedcoven.TalkAbout.TellEverythingAboutThe".Localized());

                    OutputText("Scenes.Corruptedcoven.TalkAbout.NoticesOwnExcitement".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.DullahanDead) == 1)
                        OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerSheDid".Localized());
                    else
                        OutputText("Scenes.Corruptedcoven.TalkAbout.TellDidAgain".Localized());
                }

                UpdateTopic(topic);

                DoNext(() => LeaveCirceChamber());

                return ;
                }

                case TALKED_NAMELESS_HORROR:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.TakeADeepBreath".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.FurrowsBrowIncredulous".Localized());

                UpdateTopic(topic);

                DoNext(() => RememberInfinity());

                return ;
                }

                case TALKED_CORRWITCH_HEX:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.TellAboutTheHex".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.RemainsSilentYouSigh".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.DefinitelyNotGoingTo".Localized());
                }

                break;

                case TALKED_TELADRE_WIZARDS:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.AskCirceSheKnows".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.NodWithApprehension".Localized());
                }

                break;

                case TALKED_GARGOYLE:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.TellStoryAboutThe".Localized((((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & BROUGHT_JEREMIAH_BACK) != 0) ? "[say: If not for Jeremiah, I fear the knowledge would have been lost to us completely.]" : "")));

                OutputText("Scenes.Corruptedcoven.TalkAbout.AskIfSheDoesnt".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SitsOnChairAgain".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.NodButUnsure".Localized());
                }

                break;

                case TALKED_DOMINIKA_SWORD:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.RememberDominika".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SimplyMarvelous".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.GetsUpHerChair".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.HandBackToCirce".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SmilesExcitedlySays".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SaysLookingAround".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.InterjectsBreakingAttention".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.HearHerQuestion".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TurnsToYouNotices".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.AsksUncommonlyCaring".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SaysRemovingHand".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SmileWeaklyAtHer".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.NodAgainDetermination".Localized());
                }

                break;

                case TALKED_KITSUNE_ENLIGHTEMENT:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.TellAboutTheRitual".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox && Player.Body.Get<TailPart>().Venom > 8)
                    OutputText("Scenes.Corruptedcoven.TalkAbout.CantHideKitsune".Localized());
                else
                    OutputText("Scenes.Corruptedcoven.TalkAbout.SaysIncredulousYouTell".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.StretchesShiftingInto".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.WhatAboutTheSpells".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerThatIt".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SaysTurningToFace".Localized((CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == 1 ? " You know that to be true, remembering your encounter with Taoth." : "")));

                OutputText("Scenes.Corruptedcoven.TalkAbout.TellHerYouDont".Localized((CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) > 0 ? ", especially considering how you've actually been possessed by a ghost before" : "")));

                OutputText("Scenes.Corruptedcoven.TalkAbout.SuddenlyStopsFurrowing".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.WhatIfCorruptionWorks".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.AskWhatSheMeans".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.WhatIfPuppets".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.ThoughtSendsChill".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.CouldItBeControlling".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.AttemptAnswerButSoon".Localized());

                UpdateTopic(topic);

                DoNext(() => LeaveCirceChamber());

                return ;
                }

                case TALKED_SANDWITCH_BLESS:
                {
                OutputText("Scenes.Corruptedcoven.TalkAbout.TellAboutTheSand".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.SighsQuestionIsStrange".Localized());

                OutputText("Scenes.Corruptedcoven.TalkAbout.RemainsSilentLettingWords".Localized());
                }

                break;
            }

            UpdateTopic(topic);

            BuildOptionsMenu(true);
        }

        public void UpdateTopic(int topic)
        {

            if (!((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & topic) != 0))
            {
                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) | topic);

                CoC.Instance.Flags.Set(GameFlagIds.CorruptedCovenCirceChitChatCount, CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.CorruptedCovenCirceChitChatCooldown, 48);
            }
        }

        public void RememberInfinity()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.RememberInfinity.TakeShardFromYour".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.SeeInfiniteExpanse".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.SeeTimeSpaceBend".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.SeeTheNamelessThing".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.DealFinalBlow".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.SeeBeginningOfTime".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.SeeYourself".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.AlreadySeenThis".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity.OpenYourEyes".Localized());

            DoNext(() => RememberInfinity2());
        }

        public void RememberInfinity2()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.RememberInfinity2.LookAroundNoticeCirce".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity2.IfThereAreTruths".Localized());

            OutputText("Scenes.Corruptedcoven.RememberInfinity2.TakeWordsToMind".Localized());

            if (Assets.ManagerInstance<Assets.ContentManagers.AchievementManager>.Instance.IsUnlocked(AchievementIds.LordBritishPostulate) && (OptionStore.Instance.Get("silly", 0) > 0) && CoC.Instance.Flags.Get(GameFlagIds.NhBj) == 1)
            {
                OutputText("Scenes.Corruptedcoven.RememberInfinity2.RemainsSilentAMoment".Localized());

                OutputText("Scenes.Corruptedcoven.RememberInfinity2.LiftsFingerAndOpens".Localized());

                OutputText("Scenes.Corruptedcoven.RememberInfinity2.ShrugCurious".Localized());

                OutputText("Scenes.Corruptedcoven.RememberInfinity2.DidHaveToStick".Localized());

                OutputText("Scenes.Corruptedcoven.RememberInfinity2.TeachesHerToRoot".Localized());
            }

            DoNext(() => BuildOptionsMenu());
        }

        public void CirceEvelynEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter.TripToCragIs".Localized());

            if (Player.IsBiped() && Player.Speed >= 100)
                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter.JourneyWasABit".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter.GroundQuakesWithActivity".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter.AsksPullingSteed".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter.WhateverSheSaysGuide".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter.AfterFewMomentsHeading".Localized());

            DoNext(() => CirceEvelynEncounter2());
        }

        public void CirceEvelynEncounter2()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.OpenEyesBarelyUsed".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.CantRememberTheLast".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & BROUGHT_JEREMIAH_BACK) != 0)
                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.NoticeJeremiahPredicted".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.BowsLightlyFiddles".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.CringeButAccept".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.FindYourselfBackIn".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter2.SitDownAndChat".Localized());

            DoNext(() => CirceEvelynEncounter3());
        }

        public void CirceEvelynEncounter3()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.FewMinutesPass".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.TeleportedIntoChamber".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.QuestionQuicklyAnswered".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.PeekHeadPastThe".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.NoticesYouAndCalls".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.ChuckleAndAgree".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.CirceSays".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.LookTowardsGirl".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.SaysBrushingHairAside".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.SaysLookingAtReflection".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.CompliesAndGetsUp".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.ReturnsMomentLater".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.LearnedMuchNotJust".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.ThatsOneOfQuestions".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.LookDownStillUnable".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.LiftsYourChin".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.AcceptWithoutHesitation".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.JustOpenYourMind".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter3.WorldBlackens".Localized());

            DoNext(() => CirceEvelynEncounter4());
        }

        public void CirceEvelynEncounter4()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.SunPeeksOverWindow".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.HeadOutToGarden".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.YellToTake".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.AskWhatSheWants".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.LaughAndAgree".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.TwoHeadTowards".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.TakeADeepBreath".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.TapsYouOnYour".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.TellHerTechnically".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.ComplyLaughingGuide".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter4.ChuckleAndWhip".Localized());

            DoNext(() => CirceEvelynEncounter5());
        }

        public void CirceEvelynEncounter5()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.EvelynWastesNoTime".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.RunInAndJoin".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.TwoOfYouReach".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.SaysSittingDown".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.LeaveWaterFatigued".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.LaughterTrailsOff".Localized());

            if (Player.HasCock() || Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.FeelingOfEvelynsBody".Localized());

                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.LazilyPullsYourPants".Localized());

                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.BringBothHands".Localized());
            }
            else
            {
                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.FeelingEvelynsBodyAgain".Localized());

                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.UsingFreeHandLazily".Localized());

                OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.BringAvailableHandTo".Localized());
            }

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter5.SoonHerOrgasmSubsides".Localized());

            DoNext(() => CirceEvelynEncounter6());
        }

        public void CirceEvelynEncounter6()
        {
            ClearOutput();

            Player.Orgasm("Dick");

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter6.ByTimeYouWake".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter6.SaysLookingAtSky".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounter6.ChuckleToYourself".Localized());

            DoNext(() => CirceEvelynEncounterEnd());
        }

        public void CirceEvelynEncounterEnd()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounterEnd.WakeUpAtCamp".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounterEnd.GetUpLookAround".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounterEnd.ThankYouForEverything".Localized());

            OutputText("Scenes.Corruptedcoven.CirceEvelynEncounterEnd.AsSimpleAsThat".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) + BROUGHT_DULLAHAN);

            DoNext(() => ReturnToCamp(4));
        }

        public void GiveManorBooks()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.GiveManorBooks.DontKnowPreciselyWhat".Localized());

            OutputText("Scenes.Corruptedcoven.GiveManorBooks.LoungingLadyInFront".Localized());

            OutputText("Scenes.Corruptedcoven.GiveManorBooks.EyesWanderAroundRoom".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven, CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) | BROUGHT_MANOR_BOOKS);

            Player.RemoveKeyItem("Old Manor Books");

            DoNext(() => BuildOptionsMenu());
        }

        public void LeaveCirceChamber()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) <= 2)
                OutputText("Scenes.Corruptedcoven.LeaveCirceChamber.TellCirceHaveTo".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) < 4)
                    OutputText("Scenes.Corruptedcoven.LeaveCirceChamber.TellCirceHaveAgain".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedCovenCirceChitChatCount) < 7)
                        OutputText("Scenes.Corruptedcoven.LeaveCirceChamber.TellCirceHaveThird".Localized());
                    else
                    {
                        OutputText("Scenes.Corruptedcoven.LeaveCirceChamber.TellCirceHaveFourth".Localized());

                        OutputText("Scenes.Corruptedcoven.LeaveCirceChamber.PausesForAMoment".Localized());
                    }

            OutputText("Scenes.Corruptedcoven.LeaveCirceChamber.CloseEyesAndFocus".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterCirceRepeat()
        {
            ClearOutput();

            OutputText("Scenes.Corruptedcoven.EncounterCirceRepeat.ApproachCaveAgain".Localized());

            OutputText("Scenes.Corruptedcoven.EncounterCirceRepeat.StepInsidePrepared".Localized());

            DoNext(() => CirceIntros());
        }

    }
}