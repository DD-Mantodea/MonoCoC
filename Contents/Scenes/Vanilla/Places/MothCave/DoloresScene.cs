using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.MothCave
{
    [Register<DoloresScene>("coc:dolores_scene")]
    public class DoloresScene : Scene
    {

        public const int HQCAMP = 1 << 0;

        public const int HQMARI = 1 << 1;

        public const int HQBAZR = 1 << 2;

        public const int HQTELA = 1 << 3;

        public const int HQCIRC = 1 << 4;

        public const int HQOUTD = 1 << 5;

        public const int HQDONE = 1 << 6;

        public const int HQROBE = 1 << 7;

        public const int HQFREE = 1 << 8;

        public const int HQNAME = 1 << 9;

        public const int HQKILL = 1 << 10;

        public static SaveContent Save => SaveContent(SceneIds.MothCave.Dolores);

        public static readonly List<int> campScenes = [];

        public static int loversCount() => CampScene.LoversCount();

        public static int MarielleStateScene => SceneInstance<MarielleScene>.Get().State;

        public bool doloresBlowjobFlag => Save.GetBool("doloresBlowjob");

        public static bool AmilyFollowerPure()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) <= 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) != 0)
                return false;

            var visitingUrta = CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta);

            if (visitingUrta == 1 || visitingUrta == 2)
                return false;

            return CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) != 2;
        }

        public static bool IzmaFollower() => CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1;

        public static bool FollowerRathazul() => SceneInstance<RathazulScene>.Get().FollowerRathazul();

        public static bool HelspawnFollower() => CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 3;

        public static bool AnemoneFollower() => CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0;

        public static bool ArianFollower() => CoC.Instance.Flags.Get(GameFlagIds.ArianFollower) > 0;

        public static bool FollowerEmber() => CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0;

        public static bool AkkyOwned() => CoC.Instance.Flags.Get(GameFlagIds.AkkyName) != "";

        public static bool JojoFollower() =>

            Player.HasStatusEffect(StatusEffectIds.PureCampJojo) && CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) == 0;

        public static bool MilkSlave() => CoC.Instance.Flags.Get(GameFlagIds.MilkName) != "";

        public string debugName => "Dolores";

        public string debugHint => "";

        public int doloresProg => Save.GetInt("doloresProgress", 0);

        public int doloresTime => Save.GetInt("doloresTimeSinceEvent", 0);

        public DoloresScene saveContent => this;

        public int doloresDecision => Save.GetInt("doloresDecision", 0);

        public int doloresFinal => Save.GetInt("doloresFinal", 0);

        public int doloresTimesLeft => Save.GetInt("doloresTimesLeft", 0);

        public int doloresMagicDetermine() => Save.GetInt("doloresMagicDetermine", 0);

        public int hikkiQuest => Save.GetInt("hikkiQuest", 0);

        public bool hikkiDone() => HikkiDone();

            public bool givenHint { get => Save.GetBool("givenHint"); set => Save.SetBool("givenHint", value); }

            public bool guessedName { get => Save.GetBool("guessedName"); set => Save.SetBool("guessedName", value); }

            public int demonTalks { get => Save.GetInt("demonTalks"); set => Save.SetInt("demonTalks", value); }

        public override void Entry()
        {

            SceneInstance<MothCaveScene>.Get().CaveMenu();
        }

        public bool TimeChangeLarge() => false;

        public bool TimeChange()
        {
            Save.SetInt("doloresTimeSinceEvent", Save.GetInt("doloresTimeSinceEvent", 0) + 1);

            return false;
        }

        public void Reset()
        {

            Save.SetInt("doloresProgress", 0);

            Save.SetInt("doloresTimeSinceEvent", 0);

            Save.SetInt("doloresDecision", 0);

            Save.SetInt("doloresAmbitions", 0);

            Save.SetInt("doloresFinal", 0);

            Save.SetInt("hikkiQuest", 0);

            Save.SetInt("DoloresSex", 0);

            Save.SetInt("doloresTimesLeft", 0);

            Save.SetBool("doloresAngry", false);

            Save.SetBool("DoloresBlowjob", false);

            Save.SetBool("doloresBooks", false);
        }

        public void DoloresReset()
        {

            Save.SetInt("doloresTimeSinceEvent", 0);
        }

        public float DoloresMagicDetermine()
        {

            if (Player.HasPerk(PerkIds.CorruptedNinetails) || Player.HasPerk(PerkIds.EnlightenedNinetails))
                return 1;

            if (Player.HasPerk(PerkIds.TerrestrialFire))
                return 2;

            if (Player.HasStatusEffect(StatusEffectIds.KnowsBlind) && Player.Corruption < 40)
                return 3;

            if (Player.HasStatusEffect(StatusEffectIds.KnowsMight) && Player.Corruption > 60)
                return 4;

            if (Player.HasStatusEffect(StatusEffectIds.KnowsTKBlast))
                return 5;

            if (Player.HasStatusEffect(StatusEffectIds.KnowsBlind))
                return 3;

            if (Player.HasStatusEffect(StatusEffectIds.KnowsMight))
                return 4;

            return -1;
        }

        public bool DoloresComforted()
        {

            return Save.GetInt("DoloresSex", 0) % 2 == 0;
        }

        public bool doloresComforted() => DoloresComforted();

        public bool HikkiDone()
        {

            return (Save.GetInt("hikkiQuest", 0) & HQDONE) != 0;
        }

        public bool GoodParent()
        {

            return Save.GetInt("doloresTimesLeft", 0) == 0 && Save.GetInt("DoloresSex", 0) != 3;
        }

        public void EncounterDolores()
        {

            if (doloresProg == 5 && doloresTime > 168)
                DoloresRunaway();
            else
                if (doloresProg == 9 && doloresTime > 36)
                    DoloresWings();
                else
                    if (doloresProg == 10 && doloresTime > 96)
                        DoloresConcerns();
                    else
                        if (doloresProg == 11 && Save.GetInt("doloresAmbitions", 0) == 2 && doloresTime > 72)
                            DoloresSummoning();
                        else
                            if (doloresProg == 14 && doloresTime > 48)
                                DoloresTapestryGifting();
                            else
                                if (doloresProg == 15 && doloresTime > 168 && View.Time.Hours < 17)
                                    HikkiStart();
                                else
                                    DoloresMenu();
        }

        public void DoloresPostBirth()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresPostBirth.VisitSylviaDaughter".Localized());

            OutputText("Scenes.Doloresscene.DoloresPostBirth.MothGirlHushes".Localized());

            OutputText("Scenes.Doloresscene.DoloresPostBirth.DoloresMenuAdded".Localized());

            Save.SetInt("doloresProgress", 2);

            SceneInstance<MothCaveScene>.Get().CaveMenu();

        }

        public void DoloresToys()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresToys.CheckOnDolores".Localized());

            OutputText("Scenes.Doloresscene.DoloresToys.SylviaGreetsYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresToys.CloserLookAtToys".Localized());

            OutputText("Scenes.Doloresscene.DoloresToys.ScansBookshelf".Localized());

            OutputText("Scenes.Doloresscene.DoloresToys.SylviaRummagesOn".Localized());

            Save.SetInt("doloresProgress", 3);

            ClearButtons();

            AddNextButton("Common.Button.HelpSylvia".Localized(), DoloresToysHelp);

            AddNextButton("Common.Button.TeachReading".Localized(), DoloresToysTeach);

            AddNextButton("Common.Button.Leave".Localized(), DoloresToysLeave);
        }

        public void DoloresToysHelp()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresToysHelp.FeelBadForSylvia".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysHelp.PickPictureBook".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysHelp.ShowBookToDaughter".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysHelp.SylviaJoinsReading".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysHelp.YawnInterruptsReading".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresToysTeach()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresToysTeach.SalvageToyLesson".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysTeach.SubtleNudge".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysTeach.FaunaArticle".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysTeach.SylviaJoinsReading".Localized());

            OutputText("Scenes.Doloresscene.DoloresToysTeach.YawnInterruptsReading".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresToysLeave()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresToysLeave.PityingLookAndLeave".Localized((Player.Corruption > 50 ? "and this seems kind of boring anyway" : "however much you might want to")));

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            Save.SetInt("doloresTimesLeft", Save.GetInt("doloresTimesLeft", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresTalking()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresTalking.WalkToSylviaCave".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalking.ChildWrestlesBook".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalking.SettleBesideSylvia".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalking.SylviaTalksProgress".Localized());

            Save.SetInt("doloresProgress", 4);

            ClearButtons();

            AddNextButton("Common.Button.ItsOkay".Localized(), DoloresTalkingOkay);

            AddNextButton("Common.Button.Expressyourself".Localized(), DoloresTalkingExpress);

            AddNextButton("Common.Button.Leave".Localized(), DoloresTalkingLeave);
        }

        public void DoloresTalkingOkay()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTalkingOkay.ReassureAboutTalking".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkingOkay.ComfortedSmile".Localized((Player.Tallness > 96 || Player.BiggestBreastSize() > 10 || Player.Thickness > 75 ? "not even remotely" : "not quite")));

            OutputText("Scenes.Doloresscene.DoloresTalkingOkay.HushedWhisper".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkingOkay.GentleEmbrace".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkingOkay.PreciousMomentLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresTalkingExpress()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTalkingExpress.EncourageExpression".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkingExpress.TakesWordsToHeart".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkingExpress.BlurtsFaerieWish".Localized((Player.Tallness > 96 || Player.BiggestBreastSize() > 10 || Player.Thickness > 75 ? "not even remotely" : "not quite")));

            OutputText("Scenes.Doloresscene.DoloresTalkingExpress.GentleEmbrace".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkingExpress.PreciousMomentLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresTalkingLeave()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTalkingLeave.BendDownToKiss".Localized((Player.Corruption > 50 ? "this is all a bit too dull to hold your interest" : "you have important things to return to anyway")));

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            Save.SetInt("doloresTimesLeft", Save.GetInt("doloresTimesLeft", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresMagic()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresMagic.DaughterWelcomesYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic.GreetingInterrupted".Localized((Player.Corruption < 80 ? "Not at all," : "Maybe...")));

            OutputText("Scenes.Doloresscene.DoloresMagic.SylviaSwoopsIn".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic.AwkwardSilence".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic.DisyllabicReply".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic.SylviaExcusesHerself".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic.CuriousVioletEyes".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic.AsksAboutMagic".Localized());

            Save.SetInt("doloresProgress", 5);

            ClearButtons();

            AddNextButton("Common.Button.ShowHer".Localized(), DoloresMagicShow);

            AddNextButton("Common.Button.Book".Localized(), DoloresMagicBook);

            AddNextButton("Common.Button.Leave".Localized(), DoloresMagicLeave);
        }

        public void DoloresMagicShow()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicShow.PrepMagicTrick".Localized());

            switch (DoloresMagicDetermine())
            {
                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresMagicShow.SmallFlameDance".Localized((Player.HasPerk(PerkLib.CorruptedNinetails) ? "purple" : "blue")));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresMagicShow.GreenFlamePuff".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresMagicShow.PureWhiteLight".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Doloresscene.DoloresMagicShow.FoulEnergySurge".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Doloresscene.DoloresMagicShow.TelekineticBook".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.DoloresMagicShow.RushesInAwe".Localized((silly ? "be broken" : "not be able to continue"), (Player.Corruption > 50 ? "sigh" : "smile")));

            OutputText("Scenes.Doloresscene.DoloresMagicShow.RamblingApology".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicShow.SylviaPeeking".Localized((Player.Corruption < 50 ? "Well, no matter," : "You'll have to have a talk with her later, but")));

            OutputText("Scenes.Doloresscene.DoloresMagicShow.ExplainsPowerBasics".Localized());

            DoNext(() => DoloresMagic2());
        }

        public void DoloresMagicBook()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicBook.TurnToBooks".Localized((doloresMagicDetermine() > 0 ? "Unwilling" : "Unable")));

            OutputText("Scenes.Doloresscene.DoloresMagicBook.ScansBookSpines".Localized((silly ? "\"Milking Furries for Fun and Profit,\"" : "\"The Tales of Captain Chronicle,\"")));

            OutputText("Scenes.Doloresscene.DoloresMagicBook.DustOffCover".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicBook.SylviaPeeking".Localized((Player.Corruption < 50 ? "Well, no matter," : "You'll have to have a talk with her later, but")));

            OutputText("Scenes.Doloresscene.DoloresMagicBook.BookOnMagicSources".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicBook.AddOwnStories".Localized((doloresMagicDetermine() > 0 ? "" : "surprising")));

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryMagic);

            DoNext(() => DoloresMagic2());
        }

        public void DoloresMagic2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagic2.ConversationLulls".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.WhySheDislikedTalking".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.SteerPleasantTopic".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.AffirmativeReply".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.AsksAboutTalk".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.HesitantMagicReply".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.MotherTeases".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.SmugMotherSmile".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.UnansweredGoodbye".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.SylviaHandsOnHips".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagic2.ReluctantEmbrace".Localized(
                Player.Tallness > 72 ? "waist" : (Player.Tallness > 48 ? "chest" : "shoulder")
            ));

            OutputText("Scenes.Doloresscene.DoloresMagic2.FerventNod".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresMagicLeave()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicLeave.CannotHelp".Localized((Player.Corruption < 50 ? "you're sorry, but" : "")));

            OutputText("Scenes.Doloresscene.DoloresMagicLeave.LeaveCaveForHome".Localized());

            Save.SetInt("doloresTimesLeft", Save.GetInt("doloresTimesLeft", 0) + 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresRunaway()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresRunaway.SearchDimCave".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway.SylviaAtDesk".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway.ConfirmsDaughterGone".Localized((Player.CanFly() ? "I'm the better flier, so " : "")));

            OutputText("Scenes.Doloresscene.DoloresRunaway.PursueIntoBog".Localized((Player.Corruption > 50 ? "angrily" : "worriedly"), (Player.Corruption > 50 ? "panic" : "frustration")));

            OutputText("Scenes.Doloresscene.DoloresRunaway.WorstFearsStir".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway.FigureBehindTrunk".Localized((Player.HasTailInsteadOfLegs() ? "slither" : "step")));

            OutputText("Scenes.Doloresscene.DoloresRunaway.TwigSnapStartle".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway.AsksWhyHereAlone".Localized((Player.Corruption < 40 ? "She really worried you and Sylvia by running off" : "She should know better than to defy you like this")));

            OutputText("Scenes.Doloresscene.DoloresRunaway.AdmitsUnorthodox".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway.SternestGlare".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway.AllowDangerousMagic".Localized());

            Save.SetInt("doloresProgress", 6);

            ClearButtons();

            AddNextButton("Common.Button.LetHer".Localized(), () => DoloresRunaway2(1));

            AddNextButton("Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), () => DoloresRunaway2(2));

            AddNextButton("Common.Button.TakeForNow".Localized(), () => DoloresRunaway2(3));
        }

        public void DoloresRunaway2(int decision)
        {
            ClearOutput();

            switch (decision)
            {
                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresRunaway2.YoungAndRisky".Localized((silly ? "ended the world or anything" : "done any major damage yet")));

                OutputText("Scenes.Doloresscene.DoloresRunaway2.ChildlikeGlee".Localized());

                if (silly)
                    OutputText("Scenes.Doloresscene.DoloresRunaway2.DoloresRemembers".Localized());

                OutputText("Scenes.Doloresscene.DoloresRunaway2.InspectKeptBook".Localized((Player.Corruption > 80 ? ", perhaps foolishly," : "")));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresRunaway2.DangerousToMeddle".Localized());

                OutputText("Scenes.Doloresscene.DoloresRunaway2.ResponsibleThing".Localized((Player.Corruption < 50 ? "assure her that this is only temporary" : "explain that, as her [father], you know best")));

                if (silly)
                    OutputText("Scenes.Doloresscene.DoloresRunaway2.DoloresRemembersAgain".Localized());

                OutputText("Scenes.Doloresscene.DoloresRunaway2.InspectSeizedBook".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresRunaway2.NeedMoreInfo".Localized());

                OutputText("Scenes.Doloresscene.DoloresRunaway2.CalmExplanation".Localized());

                OutputText("Scenes.Doloresscene.DoloresRunaway2.ReasonableAccept".Localized());

                OutputText("Scenes.Doloresscene.DoloresRunaway2.InspectSeizedBookAgain".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.DoloresRunaway2.SuddenSweepAway".Localized());

            OutputText("Scenes.Doloresscene.DoloresRunaway2.ExplainsToMother".Localized((saveContent.doloresDecision == 2 ? "although Dolores still seems somewhat glum, her gait much more sluggish than normal" : "and you can tell how happy Dolores is by the skip in her step, even if her face betrays nothing")));

            OutputText("Scenes.Doloresscene.DoloresRunaway2.PauseAtEntrance".Localized());

            Save.SetInt("doloresDecision", decision);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresPreCocoon()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.DoubledOverAche".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.ShuffleToBed".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.AchingStomach".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.SylviaLightsUp".Localized((silly ? "\"lady troubles\"" : "growing pains")));

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.CalmAtNews".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.GigglesAboutCocoon".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoon.StayForCocoon".Localized());

            Save.SetInt("doloresProgress", 7);

            AddButton(0, "Common.Button.Yes".Localized(), DoloresPreCocoonYes);

            AddButton(1, "Common.Button.No".Localized(), DoloresPreCocoonNo);
        }

        public void DoloresPreCocoonYes()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.WontMissEvent".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.HelpMoveFurniture".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.NervousComposure".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.TakeOffYourDress".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.FlusteredReply".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.ExplainsSilkSource".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.DarkSkinBlush".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.PushedToThreshold".Localized());

            OutputText("Scenes.Doloresscene.DoloresPreCocoonYes.KissForeheadGoodbye".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresPreCocoonNo()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresPreCocoonNo.MasksDisappointment".Localized((Player.Corruption < 50 ? "Regrettably, you cannot stay to see your daughter through this transformation" : "You have better things to do than to sit around and watch them make a cocoon.")));

            OutputText("Scenes.Doloresscene.DoloresPreCocoonNo.UnderstandingSylvia".Localized((Player.Corruption < 50 ? "you will" : "not to worry about it")));

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresCocoon()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresCocoon.ExcitedBabbling".Localized());

            OutputText("Scenes.Doloresscene.DoloresCocoon.GratefulSnuggle".Localized());

            OutputText("Scenes.Doloresscene.DoloresCocoon.SylviaSoftSigh".Localized());

            Save.SetInt("doloresProgress", 8);

            ClearButtons();

            AddNextButton("Common.Button.Reciprocate".Localized(), () => DoloresCocoon2(0));

            AddNextButton("Common.Button.BrushOff".Localized(), () => DoloresCocoon2(1));
        }

        public void DoloresCocoon2(int answer)
        {

            if (answer == 0)
                OutputText("Scenes.Doloresscene.DoloresCocoon2.FamilyFeelingShared".Localized((Player.HasChildren() ? "even if you already have one" : "")));
            else
                OutputText("Scenes.Doloresscene.DoloresCocoon2.FlatteredRebuff".Localized((Player.HasChildren() || loversCount() > 1 ? "After all, you have other loved ones waiting for you back at camp. " : "")));

            OutputText("Scenes.Doloresscene.DoloresCocoon2.GlanceAtCocoon".Localized());

            SceneInstance<SylviaScene>.Get().SylviaMenu();
        }

        public void DoloresHatches()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresHatches.ArriveAtSylviaHome".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.GreetingInterrupted".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.WatchShakingSack".Localized((Player.Corruption < 50 ? "and some trepidation" : "")));

            OutputText("Scenes.Doloresscene.DoloresHatches.RushToDaughterSide".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.NoticeNewWings".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.QuizzicalStareBack".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.YoungMothEmerges".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.AloneWithDaughter".Localized());

            OutputText("Scenes.Doloresscene.DoloresHatches.TalkWithDolores".Localized((MarielleStateScene > 0 ? "You do actually know someone who might be able to work with this--the undead seamstress in the bog" : "You don't know anyone who would be able to work with this, but you'll keep an eye out"), (Player.Corruption < 50 ? "thank her and then" : "")));

            OutputText("Scenes.Doloresscene.DoloresHatches.MothSilkGained".Localized());

            Player.CreateKeyItem("Bundle of Moth Silk", 0, 0, 0, 0);

            Save.SetInt("doloresProgress", 9);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresWings()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresWings.EnterDoloresRoom".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.HerFaceGoesSilent".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.WhyWasSheScreaming".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.SeeGrowingWings".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.SylviaBurstsIn".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.CalmWheezingMother".Localized((Player.Corruption > 50 ? "You can't deny a certain element of excitement, but nonetheless" : "This is certainly is certainly unexpected, so")));

            OutputText("Scenes.Doloresscene.DoloresWings.DoloresJumpsIn".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.SylviaCatchesBreath".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.WarmSmileForMother".Localized());

            OutputText("Scenes.Doloresscene.DoloresWings.AnythingBeforeLeaving".Localized());

            Save.SetInt("doloresProgress", 10);

            ClearButtons();

            AddNextButton("Common.Button.Offertostay".Localized(), DoloresWingsStay);

            AddNextButton("Common.Button.Hug".Localized(), DoloresWingsHug);

            AddNextButton("Common.Button.Leave".Localized(), DoloresWingsLeave);
        }

        public void DoloresWingsStay()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresWingsStay.OfferToStay".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsStay.SurprisingRefusal".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsStay.HaughtySelfReliance".Localized((saveContent.doloresTimesLeft > 0 ? " haven't" : "'ve")));

            OutputText("Scenes.Doloresscene.DoloresWingsStay.ConfirmHerResolve".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsStay.GentleSmileResponse".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresWingsHug()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresWingsHug.CloseHugDolores".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsHug.WetEyedPullBack".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsHug.CheckSheIsOkay".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsHug.SheWillBeFine".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsHug.KissSylviaGoodbye".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresWingsLeave()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresWingsLeave.WaveMothsGoodbye".Localized((saveContent.doloresTimesLeft > 0 ? "look of resignation" : "stoic look")));

            Save.SetInt("doloresTimesLeft", Save.GetInt("doloresTimesLeft", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresConcerns()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresConcerns.ApproachWithSmallTalk".Localized((saveContent.doloresDecision == 1 ? "on which sits an old, dusty book which you recognize as the one she stole off with that night" : "which is notably empty, cleared of the various books and scraps of paper that would usually be lying on it")));

            OutputText("Scenes.Doloresscene.DoloresConcerns.WaitingForAttention".Localized());

            OutputText("Scenes.Doloresscene.DoloresConcerns.CrypticSeriousWarning".Localized());

            OutputText("Scenes.Doloresscene.DoloresConcerns.ConfessionTrailsOff".Localized());

            OutputText("Scenes.Doloresscene.DoloresConcerns.DelayedContrition".Localized());

            Save.SetInt("doloresProgress", 11);

            ClearButtons();

            AddNextButton("Common.Button.ItsOkay".Localized(), () => DoloresConcerns2(0));

            AddNextButton("Common.Button.Apologize".Localized(), () => DoloresConcerns2(1));

            AddNextButton("Common.Button.Shouldntcare".Localized(), () => DoloresConcerns2(2));

            AddNextButton("Common.Button.Shrug".Localized(), () => DoloresConcerns2(3));
        }

        public void DoloresConcerns2(int choice)
        {

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.DoloresConcerns2.DownplayHerMistake".Localized());

                OutputText("Scenes.Doloresscene.DoloresConcerns2.StareThenRelief".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresConcerns2.TellHerAccountability".Localized());

                OutputText("Scenes.Doloresscene.DoloresConcerns2.ResolveToMakeRight".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresConcerns2.TellHerIgnoreInferiors".Localized());

                OutputText("Scenes.Doloresscene.DoloresConcerns2.ScornForLesserMinds".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresConcerns2.ShrugAtAdvice".Localized());

                OutputText("Scenes.Doloresscene.DoloresConcerns2.FindAnswersAlone".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.DoloresConcerns2.ShiverThenSoften".Localized());

            OutputText("Scenes.Doloresscene.DoloresConcerns2.RecoveredFromDoubt".Localized((saveContent.doloresDecision == 1 ? "that book" : "some drawers along the far wall")));

            DoNext(() => DoloresMenu());
        }

        public void DoloresSummoning()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresSummoning.DoloresRoomEmpty".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning.FindNoteOnBed".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning.DontTellMotherNote".Localized(
                Player.Mf("which seems strange", "which you'll take to mean Sylvia")
            ));

            OutputText("Scenes.Doloresscene.DoloresSummoning.NoticeSylviaAlone".Localized());

            Save.SetInt("doloresProgress", 12);

            ClearButtons();

            AddNextButton("Common.Button.TellHer".Localized(), DoloresSummoningTell);

            AddNextButton("Common.Button.Leave".Localized(), DoloresSummoningDontTell);
        }

        public void DoloresSummoningTell()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningTell.TellSylviaTheTruth".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningTell.AsksWhatIsWrong".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningTell.ExplainRecentBehavior".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningTell.SylviaSuggestsWaiting".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningTell.ConfidentAboutPlan".Localized());

            Save.SetInt("doloresFinal", 10);

            DoNext(() => DoloresSummoning2());
        }

        public void DoloresSummoningDontTell()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningDontTell.HonorTheNoteRequest".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningDontTell.BogHelpRegret".Localized());

            DoNext(() => DoloresSummoning2());
        }

        public void DoloresSummoning2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoning2.MireFeelsCooler".Localized());

            if (Save.GetInt("doloresFinal", 0) > 0)
                OutputText("Scenes.Doloresscene.DoloresSummoning2.SylviaTakesOff".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning2.ShiverAndSetOut".Localized((saveContent.doloresTimesLeft > 0 ? "you feel obligated to do [i: something], just in case she's in some actual danger" : "you can't let your daughter down, even if it means you'll have to go through hell to do it")));

            OutputText("Scenes.Doloresscene.DoloresSummoning2.BogStaysSame".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning2.HopeWithoutProgress".Localized((saveContent.doloresFinal > 0 ? "Sylvia could be of some help with her aerial perspective" : "you could try climbing a tree for a better vantage point")));

            OutputText("Scenes.Doloresscene.DoloresSummoning2.SameTreeSpotted".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning2.CreepAroundTree".Localized());

            DoNext(() => DoloresSummoning3());
        }

        public void DoloresSummoning3()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoning3.RitualCavityReturn".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning3.CallOutStartle".Localized((saveContent.doloresDecision == 1 ? "While you did give that to her, she wasn't meant to do anything dangerous with it, and this certainly doesn't look safe." : "But you took that from her! Did she steal it back? Is she really so obsessed with this that she'd defy the will of her parent?")));

            OutputText("Scenes.Doloresscene.DoloresSummoning3.ClaimsWellControl".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning3.TimeSlowsOnStep".Localized());

            if (Save.GetInt("doloresFinal", 0) > 0)
                OutputText("Scenes.Doloresscene.DoloresSummoning3.SylviaLandsNearby".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning3.OnlyWaitAndWatch".Localized());

            ClearButtons();

            AddNextButton("Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), DoloresSummoningStop);

            AddNextButton("Common.Button.Letitcontinue".Localized(), DoloresSummoningLet);

            AddNextButton("Common.Button.Talkdown".Localized(), DoloresSummoningTalkDown);
        }

        public void DoloresSummoningStop()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningStop.MustStopHer".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningStop.DoloresRecoilsBack".Localized());

            if (Player.Strength > 90 && Player.Speed > 90)
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningStop.HeaveForwardSteps".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningStop.WithinStrikingRange".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningStop.DefiantRefusal".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningStop.FaceHazeClears".Localized());

                Save.SetInt("doloresFinal", Save.GetInt("doloresFinal", 0) + 1);

                DoNext(() => DoloresSummoningEnd());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningStop.DistanceNeverCloses".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningStop.TimeRunsOut".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningStop.TriumphantDemand".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningStop.DreadAtRitualEnd".Localized());

                DoNext(() => DoloresSummoning4());
            }
        }

        public void DoloresSummoningLet()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningLet.ContentToWatch".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLet.GleefulAnnouncement".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLet.SinkingGutFeeling".Localized());

            DoNext(() => DoloresSummoning4());
        }

        public void DoloresSummoningTalkDown()
        {
            ClearOutput();

            if (GoodParent() && Save.GetInt("doloresFinal", 0) > 0)
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.CalmingVoiceAppeal".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.ShakesFreeOfTrance".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.SylviaPipesUp".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.HeartThumpSilence".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.SingleSobEmbrace".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.OlderMothSoothes".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.PeaceSettlesFinally".Localized());

                Save.SetInt("doloresFinal", Save.GetInt("doloresFinal", 0) + 1);

                DoNext(() => DoloresSummoningEnd());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.ExplainItMustStop".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.RefusesExplanation".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningTalkDown.ResolveAnotherWay".Localized());

                ClearButtons();

                AddNextButton("Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), DoloresSummoningStop);

                AddNextButton("Common.Button.Letitcontinue".Localized(), DoloresSummoningLet);
            }
        }

        public void DoloresSummoning4()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoning4.EnergyReformsOrb".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.PlaceholderSpeechToken".Localized((doloresDecision == 1 ? "This is what you gave me the book for, don't you see? I'm meant to do this, I... can feel it. It's close." : "I can finally show you what I've been seeing this whole time. You'll understand! When you see it, you'll understand!")));

            OutputText("Scenes.Doloresscene.DoloresSummoning4.SpaceUndulates".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.WhiteLightFlash".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.FloatingOrbAppears".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.LongAwaitedQuestion".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.OrbShiftsSlightly".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.BloomingAppendages".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoning4.RetractAndReform".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fight".Localized(), DoloresSummoningFight);

            AddNextButton("Common.Button.AskDolores".Localized(), DoloresSummoningAsk);

            AddNextButton("Common.Button.UseBook".Localized(), DoloresSummoningUseBook);
        }

        public void DoloresSummoningFight()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningFight.BraceForAttack".Localized((Player.Weapon.IsFist() ? "out" : "drawn"), (saveContent.doloresFinal > 10 ? "Sylvia seems preoccupied with protecting Dolores, so it looks like you'll be on your own for this." : "")));

            OutputText("Scenes.Doloresscene.DoloresSummoningFight.LaunchOffensive".Localized((Player.HasPerk(PerkLib.Revelation) ? "already have some experience fighting things like this" : "have to do [i: something]")));

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "outsider")), () => ReturnToCamp(1));
        }

        public void DoloresSummoningAsk()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningAsk.AskDoloresForHelp".Localized((Player.Corruption < 50 ? "You reassure her that anything, no matter how small or seemingly insignificant, might help" : "You demand that she fix this problem, as it's her fault in the first place")));

            OutputText("Scenes.Doloresscene.DoloresSummoningAsk.DoloresScreamsInFear".Localized());

            if (Save.GetInt("doloresFinal", 0) > 0)
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.SylviaClearsThroat".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.SylviaReassuresDolores".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.DoloresSnapsOut".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.PointsToSummoningPage".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.TurnsToSylvia".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.SylviaSmilesWarmly".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.TurnsToYou".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.YouQuicklyAgree".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.DoloresStartsRitual".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.DoloresFallsIntoTrance".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.OrbReactsToRitual".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.EntityRecoilsBack".Localized());

                Save.SetInt("doloresFinal", Save.GetInt("doloresFinal", 0) + 2);

                DoNext(() => DoloresSummoningEnd());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.SheTrailsOffUncertain".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.DoloresAgreesToTry".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.DoloresOpensBook".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.StartsGutturalChant".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.TentaclesWritheInPain".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.BanishmentDidntWork".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningAsk.FightItAfterAll".Localized());

                DoNext(() => DoloresSummoningFight());
            }
        }

        public void DoloresSummoningUseBook()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.RequestTheBook".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.UnfamiliarScript".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.StudyingStartledByNoise".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.ItMustBeStopped".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.NoOtherOptionsBegin".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.OrbTurnsToYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningUseBook.OrbPopsAndVanishes".Localized());

            Save.SetInt("doloresFinal", Save.GetInt("doloresFinal", 0) + 3);

            DoNext(() => DoloresSummoningEnd());
        }

        public void DoloresSummoningLose()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.KnockedToGround".Localized((Player.Corruption > 50 ? "Even if you're not the heroic type, you can't exactly live in this place if it's destroyed." : "")));

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.OrbFloatsAbove".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.QuestionsAreDistraction".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.TentacleGrabsYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.YouSomehowSurvive".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.PantingOnTheGround".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningLose.GetUpAndAssess".Localized());

            Save.SetInt("doloresFinal", Save.GetInt("doloresFinal", 0) + 4);

            CombatScene.CleanupAfterCombat();

            DoloresSummoningEnd();
        }

        public void DoloresSummoningWin()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSummoningWin.OrbShuddersAndLeaves".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningWin.KeepWeaponReady".Localized());

            Save.SetInt("doloresFinal", Save.GetInt("doloresFinal", 0) + 3);

            CombatScene.CleanupAfterCombat();

            DoloresSummoningEnd();
        }

        public void DoloresSummoningEnd(bool repeat = false)
        {
            ClearOutput();

            if (repeat)
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.AskAboutHerRambling".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.BlushesAtQuestion".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.CompelledToThisTask".Localized());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.EverythingSeemsNormal".Localized((saveContent.doloresFinal % 10 > 1 ? "Whatever that thing was, it's gone now, and you let out a breath you didn't even know you were holding." : "")));

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.DoloresHugsYou".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.HeartfeltApology".Localized());

                if (Save.GetInt("doloresFinal", 0) > 10)
                    OutputText("Scenes.Doloresscene.DoloresSummoningEnd.SylviaChimesIn".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd.ChooseYourResponse".Localized());
            }

            ClearButtons();

            AddNextButton("Common.Button.Explanation".Localized(), () => DoloresSummoningEnd(true));

            AddNextButton("Common.Button.ItsOkay".Localized(), () => DoloresSummoningEnd2(0));

            AddNextButton("Common.Button.TryHarder".Localized(), () => DoloresSummoningEnd2(1));

            AddNextButton("Common.Button.NeverAgain".Localized(), () => DoloresSummoningEnd2(2));
        }

        public void DoloresSummoningEnd2(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.EverythingIsOkay".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.SoftSmileWithTears".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.CuspOfSomethingGreat".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.MesmerizedByYourWords".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.PunishOnlyThisOnce".Localized());

                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.TremblingWithShame".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, 3f));
                }

                break;
            }

            if (Save.GetInt("doloresFinal", 0) > 10)
                OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.SylviaDriftsOver".Localized());

            OutputText("Scenes.Doloresscene.DoloresSummoningEnd2.ArmsSlumpExhausted".Localized());

            DoNext(() => ReturnToCamp(2));
        }

        public void DoloresTalkAfter()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresTalkAfter.WalkingBackToCave".Localized());

            if (Save.GetInt("doloresFinal", 0) > 10)
            {
                OutputText("Scenes.Doloresscene.DoloresTalkAfter.EntangledByFourArms".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfter.NiceToSeeYou".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfter.TreasureYourTrust".Localized());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresTalkAfter.LoomingPresenceBehind".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfter.SylviaSaysYourName".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfter.SylviaArmsCrossed".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfter.SylviaPleadsForHonesty".Localized());
            }

            OutputText("Scenes.Doloresscene.DoloresTalkAfter.SylviaLeadsYouIn".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkAfter.SitDownBesideHer".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkAfter.YouWantToTalk".Localized());

            Save.SetInt("doloresProgress", 13);

            ClearButtons();

            AddNextButton("Common.Button.YouDo".Localized(), DoloresTalkAfterYes);

            AddNextButton("Common.Button.NoNeed".Localized(), DoloresTalkAfterNo);
        }

        public void DoloresTalkAfterYes()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTalkAfterYes.YouAgreeToTalk".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkAfterYes.YouMustStartFirst".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Book".Localized(), () => DoloresTalkAfterAnswer(0));

            if (Save.GetInt("doloresFinal", 0) % 10 > 1)
                AddNextButton("Common.Button.Thing".Localized(), () => DoloresTalkAfterAnswer(1));

            AddNextButton("Common.Button.Again".Localized(), () => DoloresTalkAfterAnswer(2));

            AddNextButton("Common.Button.Sneak".Localized(), () => DoloresTalkAfterAnswer(4));

            AddNextButton("CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), () => DoloresTalkAfterAnswer(3));
        }

        public void DoloresTalkAfterAnswer(int question)
        {
            ClearOutput();

            switch (question)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.AskAboutTheTome".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.WhereSheFoundIt".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.DoesItActuallyWork".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.ReluctantToFindOut".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.SheStopsTalking".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.AskWhatSheBrought".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.VastExpanseOfNothing".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.GuiltyLookAtGround".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.ConfirmOneTime".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.BlushesWithIndignation".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.TopicEmbarrassesHer".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.NothingElseToAsk".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.OneFinalThing".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.ArmsReachForBook".Localized((saveContent.doloresDecision == 1 ? "Thank you for letting me keep it at all." : "For good, this time.")));

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.FourArmHug".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.FarewellFather".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.TheyPartWays".Localized());

                Player.CreateKeyItem("Old Eldritch Tome", 0, 0, 0, 0);

                DoNext(() => ReturnToCamp(1));

                return ;
                }

                case 4:
                {
                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.WhyTheSecrecy".Localized((saveContent.doloresDecision == 1 ? "Last time should have been more than enough evidence to trust you" : "Even after what happened last time, she should still trust her [father]")));

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.AshamedOfHerself".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.SheFeltCompelled".Localized());

                OutputText("Scenes.Doloresscene.DoloresTalkAfterAnswer.NodsAndTrustHer".Localized());
                }

                break;
            }

            ClearButtons();

            AddNextButton("Common.Button.Book".Localized(), () => DoloresTalkAfterAnswer(0));

            if (Save.GetInt("doloresFinal", 0) % 10 > 1)
                AddNextButton("Common.Button.Thing".Localized(), () => DoloresTalkAfterAnswer(1));

            AddNextButton("Common.Button.Again".Localized(), () => DoloresTalkAfterAnswer(2));

            AddNextButton("Common.Button.Sneak".Localized(), () => DoloresTalkAfterAnswer(4));

            AddNextButton("CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), () => DoloresTalkAfterAnswer(3));
        }

        public void DoloresTalkAfterNo()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTalkAfterNo.QuietHerWithHand".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkAfterNo.SheTakesItWell".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkAfterNo.HesitatesBeforeLeaving".Localized());

            OutputText("Scenes.Doloresscene.DoloresTalkAfterNo.GivesYouTheBook".Localized((saveContent.doloresDecision == 1 ? "Thank you for letting me keep it at all." : "For good, this time.")));

            Player.CreateKeyItem("Old Eldritch Tome", 0, 0, 0, 0);

            SceneInstance<MothCaveScene>.Get().CaveMenu(); 
        }

        public void DoloresTapestryMaking()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.FaintGlowAndVoices".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.ComplainsAboutPose".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.BeGoodForMommy".Localized());

            if (Save.GetInt("DoloresSex", 0) > 0)
                OutputText("Scenes.Doloresscene.DoloresTapestryMaking.PuzzledOhWord".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.DoloresPosingForSketch".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.PoseHoldingMatters".Localized());

            if (Save.GetInt("DoloresSex", 0) > 0)
                OutputText("Scenes.Doloresscene.DoloresTapestryMaking.RealizationOhWord".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.EyesSwingToYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.SylviaWelcomesYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.AlmostSurrealSight".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.NiceFamilyMoment".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.LittleWhileLonger".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.AskAboutFinalDesign".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.KeepDesignSecret".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.IdentifyHiddenSmile".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.SketchIsDone".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.DoloresSlumpsTired".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryMaking.SylviaSlipsToDoorway".Localized());

            Save.SetInt("doloresProgress", 14);

            DoNext(() => DoloresMenu());
        }

        public void DoloresTapestryGifting()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.FindDoloresAtBed".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.MostWonderfulGift".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.LiftsTapestryTenderly".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.MovedToTears".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.EyesDartUpSuddenly".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.MothBrushesPastYou".Localized());

            OutputText("Scenes.Doloresscene.DoloresTapestryGifting.LetsTalkNow".Localized());

            Save.SetInt("doloresProgress", 15);

            DoNext(() => DoloresMenu());
        }

        public void HikkiMenu()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiMenu.WhereToTakeHer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Camp".Localized(), HikkiCamp);

            if (SceneInstance<MarielleScene>.Get().MarielleAvailable(false, false))
                AddButton(1, "Common.Button.Marielle".Localized(), HikkiMarielle);

            if (CoC.Instance.Flags.Get(GameFlagIds.BazaarEntered) > 0)
                AddButton(2, "Common.Button.Bazaar".Localized(), HikkiBazaar);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeenToLibrary) != 0)
                AddButton(3, "Common.Button.TeladreLib".Localized(), HikkiLibrary);

            if (SceneInstance<Areas.VolcanicCrag.CorruptedCoven>.Get().CirceEnabled())
                AddButton(4, "Scenes.Dullahan.DullTalkMenu.ButtonCirce".Localized(), HikkiCirce);

            if (Save.GetInt("DoloresSex", 0) > 2)
                AddButton(5, "Common.Button.OutdoorSex".Localized(), HikkiSex);

            AddButton(13, "CharCreation.GenericStyleCustomizeMenu.Button8".Localized(), HikkiFinished);

            for (var i = 0; i < 14; i++)
            {
                var button = Buttons[i];

                if (button != null && button.Visible)
                    button.DisableIf(View.Time.Hours >= 21, "");
            }

            SetExitButton("Scenes.Camp.Rest".Localized(), HikkiRest);
        }

        public void HikkiStart()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiStart.ApproachWithDetermination".Localized());

            OutputText("Scenes.Doloresscene.HikkiStart.AskShowMeAround".Localized());

            OutputText("Scenes.Doloresscene.HikkiStart.HerHomeQuestion".Localized());

            OutputText("Scenes.Doloresscene.HikkiStart.BlushCreepsUp".Localized());

            OutputText("Scenes.Doloresscene.HikkiStart.DangerousTravelWarning".Localized());

            OutputText("Scenes.Doloresscene.HikkiStart.AdultClaimDoubt".Localized());

            OutputText("Scenes.Doloresscene.HikkiStart.AskSeeTheSights".Localized());

            Save.SetInt("doloresProgress", 16);

            ClearButtons();

            AddNextButton("Common.Button.Accept".Localized(), HikkiAccept);

            AddNextButton("Scenes.Lumberjack.Button.Decline".Localized(), HikkiDecline);

            AddNextButton("Common.Button.Later".Localized(), HikkiLater);
        }

        public void HikkiAccept()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiAccept.AcceptTravelRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.ComposedBubblyJoy".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.PackTravelEssentials".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.AskShallWeGo".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.DestinationDecision".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.AskShowYourHome".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.ManageableNewPlans".Localized());

            OutputText("Scenes.Doloresscene.HikkiAccept.BogWatcherFeeling".Localized());

            DoNext(() => HikkiMenu());
        }

        public void HikkiDecline()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiDecline.DeclineHerRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiDecline.ConcealedHurt".Localized());

            OutputText("Scenes.Doloresscene.HikkiDecline.AskingTimeToThink".Localized());

            Save.SetBool("doloresAngry", true);

            Save.SetInt("doloresProgress", 17);

            SceneInstance<MothCaveScene>.Get().CaveMenu(); 
        }

        public void HikkiLater()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiLater.PostponeToLater".Localized());

            OutputText("Scenes.Doloresscene.HikkiLater.CalmAcceptance".Localized());

            OutputText("Scenes.Doloresscene.HikkiLater.ReturnsToSeat".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void HikkiRest()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRest.NeedTimeRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiRest.WarmGraciousReply".Localized());

            OutputText("Scenes.Doloresscene.HikkiRest.PromptShallWeGo".Localized());

            OutputText("Scenes.Doloresscene.HikkiRest.HeadBackHome".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void HikkiContinue()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiContinue.ReadyToContinue".Localized());

            OutputText("Scenes.Doloresscene.HikkiContinue.BrightSmileReply".Localized());

            HikkiMenu();

            AddButton(14, "Common.Button.Back".Localized(), () => DoloresTalkMenu());
        }

        public void HikkiCampBuild()
        {

            campScenes.Clear();

            if (AmilyFollowerPure())
                campScenes.Add(0);

            if (IzmaFollower())
                campScenes.Add(1);

            if (FollowerRathazul())
                campScenes.Add(2);

            if (HelspawnFollower())
                campScenes.Add(3);

            if (AnemoneFollower())
                campScenes.Add(4);

            if (ArianFollower())
                campScenes.Add(5);

            if (FollowerEmber())
                campScenes.Add(6);

            if (AkkyOwned())
                campScenes.Add(7);

            if (JojoFollower())
                campScenes.Add(8);

            if (MilkSlave() && CoC.Instance.Flags.Get(GameFlagIds.MilkSize) < 2)
                campScenes.Add(9);

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0)
                campScenes.Add(10);
        }

        public void HikkiCamp()
        {
            ClearOutput();

            HikkiCampBuild();

            OutputText("Scenes.Doloresscene.HikkiCamp.CampArrivalFeelings".Localized((campScenes.Count > 0 ? "Will she get along with your other camp members? " : "")));

            OutputText("Scenes.Doloresscene.HikkiCamp.SharpBreathGasp".Localized());

            OutputText("Scenes.Doloresscene.HikkiCamp.AskForTour".Localized());

            OutputText("Scenes.Doloresscene.HikkiCamp.BestThingToShow".Localized());

            if (campScenes.Count > 0)
            {
                OutputText("Scenes.Doloresscene.HikkiCamp.MeetCompanionsAsk".Localized());

                OutputText("Scenes.Doloresscene.HikkiCamp.CompanionsAssurance".Localized());

                OutputText("Scenes.Doloresscene.HikkiCamp.HopeOptimismHolds".Localized());

                DoNext(() => HikkiCampFollower());
            }
            else
            {
                OutputText("Scenes.Doloresscene.HikkiCamp.CabinVisit".Localized());

                OutputText("Scenes.Doloresscene.HikkiCamp.PortalInterest".Localized());

                OutputText("Scenes.Doloresscene.HikkiCamp.FascinatedWhisper".Localized());

                OutputText("Scenes.Doloresscene.HikkiCamp.RestOfCampTour".Localized());

                if (silly)
                    OutputText("Scenes.Doloresscene.HikkiCamp.PreciousWaterBarrel".Localized());

                DoNext(() => HikkiCampDone());
            }
        }

        public void HikkiCampFollower()
        {
            ClearOutput();

            var __sw0_0 = campScenes[Random.Shared.Next(campScenes.Count)];

            campScenes.Remove(__sw0_0);

            if (__sw0_0 == 0)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeetAmily".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.ResemblanceRemark".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.GentleSmileQuery".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.CordialMotherlyChat".Localized());
            }

            if (__sw0_0 == 1)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeetIzma".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.AlphaChildPraise".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PraiseBlushThanks".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.EasyGrinReply".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.BookConversation".Localized());
            }

            if (__sw0_0 == 2)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.AlchemistAttention".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.MothRarityRemark".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.DowncastReply".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SilkCompoundPraise".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.AlchemicalRambling".Localized());
            }

            if (__sw0_0 == 3)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeetSalamanderDaughter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) < 50)
                {
                    OutputText("Scenes.Doloresscene.HikkiCampFollower.AnotherDaughterRemark".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.HearingAboutCamp".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.AskAboutHobbies".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.NervousAgreement".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.FieldTripChatter".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.HeartwarmingLaughter".Localized());
                }
                else
                {
                    OutputText("Scenes.Doloresscene.HikkiCampFollower.PlayfulCuteTease".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.FlusteredExcuseMe".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.ChucklingEatJoke".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.InterruptingLeanIn".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.KeepingYouAllMine".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.ProlongedTeasing".Localized());

                    OutputText("Scenes.Doloresscene.HikkiCampFollower.DrawnOutGiggle".Localized());
                }
            }

            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeetAnemoneDaughter".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.DoloresFirstMove".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SilentBeat".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SilentBeatAgain".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SilentBeatThird".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.WordlessSmile".Localized());
            }

            if (__sw0_0 == 5)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MagicTeacherIdea".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.CompanionInquiry".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.ExplainDoloresAndMagic".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.FormerTeacherOffer".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.MagicTheoryEnthusiasm".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SelfTaughtRemark".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.BlushAndApproval".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.WonderfulPraise".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PointersExchange".Localized());
            }

            if (__sw0_0 == 6)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeetEmber".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.EmberCutsIn".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.HaughtySelfNaming".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.LowRumbleRegret".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.ColdBloodedTaunt".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.WaistGrabRetreat".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PutMeDownProtest".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.FirePreventionPlea".Localized());
            }

            if (__sw0_0 == 7)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.SpecialMemberVisit".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.CatPetWonder".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.LoungingCatVisit".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PoliteCatPraise".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.CatHissesAtYou".Localized());
            }

            if (__sw0_0 == 8)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeetJojo".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SereneGreeting".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PerturbedGreeting".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.MeditationInvite".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SpiritCenterQuery".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PlainConfirmation".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.SittingQuestion".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.InvolvedProcess".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.KindOfferDeclined".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PulledAwayHastily".Localized());
            }

            if (__sw0_0 == 9)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.BathGirlCuriosity".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.HarshSuspicion".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.ExplainSituationSeated".Localized((CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1 ? "You've even given her back some of her faculties. " : "")));

                OutputText("Scenes.Doloresscene.HikkiCampFollower.WorldStillUnknown".Localized());
            }

            if (__sw0_0 == 10)
            {
                OutputText("Scenes.Doloresscene.HikkiCampFollower.HarpyAmbush".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.RapidFireQuestions".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.ArmLengthStruggle".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.FlightQuestion".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.PerplexedExcuseMe".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.LetsFlyTogether".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.TechnicalFlightHesitation".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.EagerLetsGo".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.RocketedUpward".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.CoachedFlightBasics".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.AnimatedRecounting".Localized());

                OutputText("Scenes.Doloresscene.HikkiCampFollower.FarewellToHarpy".Localized());
            }

            View.Time.PassMinutes(10);

            if (campScenes.Count > 0)
                HikkiCampFollower();
            else
                HikkiCampDone();
        }

        public void HikkiCampDone()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiCampDone.CampBreatherRest".Localized());

            OutputText("Scenes.Doloresscene.HikkiCampDone.CampRemarkableWords".Localized());

            OutputText("Scenes.Doloresscene.HikkiCampDone.CampTimeToPart".Localized());

            OutputText("Scenes.Doloresscene.HikkiCampDone.CampJourneyBack".Localized());

            View.Time.PassMinutes((int)Math.Round((double)(((60 - View.Time.Minutes) / 60) * 60)));

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQCAMP);

            DoNext(() => HikkiMenu());
        }

        public void HikkiMarielle()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiMarielle.TempleArrivalBriefing".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle.MarielleStatusExplained".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle.CuriousSurprise".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle.CuriosityPiqued".Localized());

            DoNext(() => HikkiMarielle2());
        }

        public void HikkiMarielle2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiMarielle2.MarielleAtWork".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.CuriousButWary".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.StartledSeamstress".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.IntroducingDaughter".Localized((!Player.HasKeyItem("Bundle of Moth Silk") ? "the one whose silk you once brought to Marielle" : "even if the two of you don't look very similar")));

            OutputText("Scenes.Doloresscene.HikkiMarielle2.PuzzledGreeting".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.DaughterSpeaksUp".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.AwkwardHabitation".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.BlushingQuestion".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.ReposefulFane".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.FaneEchoReply".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle2.StretchingSilence".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Books".Localized(), () => HikkiMarielleAnswer(0));

            AddNextButton("Common.Button.Sewing".Localized(), () => HikkiMarielleAnswer(1));

            AddNextButton("Common.Button.Necromancy".Localized(), () => HikkiMarielleAnswer(2));

            AddNextButton("Common.Button.FourArms".Localized(), () => HikkiMarielleAnswer(3));

            AddNextButton("Common.Button.Nothing".Localized(), () => HikkiMarielleAnswer(4));
        }

        public void HikkiMarielleAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SuggestReading".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.ConfidentNod".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.UncertainSeamstress".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.CrushedDisappointment".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.ParentRegret".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SewingTopic".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SeamstressUnease".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.EagerAgreement".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.FidgetingQuestion".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.ShyAdmission".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.NervousFidgeting".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.NecromancyTopic".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SeamstressThaumaturgy".Localized((SceneInstance<MarielleScene>.Get().HasNecromancyTalk ? "I have told you afore, have I not? " : "")));

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.ForcedSmile".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.HastyRetraction".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.GlassesReassurance".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SpeechlessMoth".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.FourArmsTopic".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.DesperateLatching".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.TenseAwareness".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.BetterTopicRegret".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SilentStandoff".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.PainfulSilence".Localized());

                OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.BadIdeaDoubt".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.SeamstressNoticesDress".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.DressProvenanceQuestion".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.OldRoomDiscovery".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.HonestCritique".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.IntriguedBlushFade".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.PermissionRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.NervousSwallow".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.AwkwardApology".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.MomentaryLull".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.CrackedExclamation".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.StumblingRealization".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.RummagedDress".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.MarvelingPause".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.PrivacyToChange".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielleAnswer.MomentPlease".Localized());

            DoNext(() => HikkiMarielle3());
        }

        public void HikkiMarielle3()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiMarielle3.DressFinished".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.DressDisplayAsk".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.JoyfulTwirl".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.SeamstressCurtsy".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.HesitantRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.HappyClasping".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.ConfidentGems".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.NervousGlance".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.FriendlyGiggle".Localized());

            OutputText("Scenes.Doloresscene.HikkiMarielle3.BogDeparture".Localized());

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQMARI);

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            DoNext(() => HikkiMenu());
        }

        public void HikkiBazaar()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiBazaar.GrassSeaWarning".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar.DestinationPrep".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar.MotleyTents".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar.AwestruckDaughter".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar.ShyDelight".Localized());

            if (SceneInstance<TellyScene>.Get().TellyGenesis)
                HikkiBazaarTelly();
            else
                HikkiBazaar2();
        }

        public void HikkiBazaarTelly()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.ConfectionerVisit".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.InviteInside".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.ShopWelcome".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.NervousDeflection".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.PuzzledInquiry".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.SkepticalQuestion".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.ToymakingDefense".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.EmbarrassedPurchase".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.StartledTelly".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.BefuddledDaughter".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.ToyPitch".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaarTelly.QuickExit".Localized());

            DoNext(() => HikkiBazaar2());
        }

        public void HikkiBazaar2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiBazaar2.ClothingStand".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar2.SuddenDash".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar2.BlueFabric".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar2.WizardRobesDelight".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar2.WizardlyPose".Localized());

            ClearButtons();

            AddNextButton("Common.Button.BuyIt".Localized(), () => HikkiBazaar3(true));

            AddNextButton("Common.Button.LetHer".Localized(), () => HikkiBazaar3(false));
        }

        public void HikkiBazaar3(bool bought)
        {
            ClearOutput();

            if (bought)
            {
                OutputText("Scenes.Doloresscene.HikkiBazaar3.StoppingDaughter".Localized());

                OutputText("Scenes.Doloresscene.HikkiBazaar3.RobesGift".Localized());

                OutputText("Scenes.Doloresscene.HikkiBazaar3.StutteredThanks".Localized());

                Player.Gems -= 30;

                Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQROBE);
            }
            else
            {
                OutputText("Scenes.Doloresscene.HikkiBazaar3.RobeReturn".Localized());

                OutputText("Scenes.Doloresscene.HikkiBazaar3.QuietMutters".Localized());
            }

            OutputText("Scenes.Doloresscene.HikkiBazaar3.SubduedTour".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar3.GaudyEncounter".Localized());

            if (silly)
                OutputText("Scenes.Doloresscene.HikkiBazaar3.OverheardSnippet".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar3.CatchingBreath".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar3.EnlightenedDeparture".Localized());

            OutputText("Scenes.Doloresscene.HikkiBazaar3.BazaarGateReturn".Localized());

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQBAZR);

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

            DoNext(() => HikkiMenu());
        }

        public void HikkiLibrary()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiLibrary.DesertJourneyTalk".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.PlannedDestination".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.CrowdedStreets".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.TowerArrival".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.SleepyLibrarian".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.HostileWhat".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.NervousExcuse".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.SarcasticRetort".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.SanitaryHabitsQuip".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.SoftenedRefusal".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.DejectedSilence".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.SuddenInterest".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.ModestComparison".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.EagerNegotiation".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.SageNod".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.TimeToLeave".Localized());

            OutputText("Scenes.Doloresscene.HikkiLibrary.GateRequest".Localized());

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQTELA);

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

            DoNext(() => HikkiMenu());
        }

        public void HikkiCirce()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiCirce.CragTooDangerous".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.BarrenLandscape".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.RelativePeaceEnds".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.UnexpectedGuest".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.CirceFinishingMakeup".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.WhoDidYouBring".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.ExplainDaughterHere".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.WarnMeInAdvance".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.InterruptedByDolores".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.WhereAreWeNow".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.InMyChambers".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.CirceStopsTalking".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.MissedSpot".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.ObligeAndExplain".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.SorceressBriefAnswer".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.NailsHitVanity".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.DeepBreathFeverish".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.MothGirlBlinks".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.PartyTricksRefusal".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.DoloresSmirksFlattery".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.DoloresVanishes".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.NoviceOutburst".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.WhereIsMyDaughter".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.ChitinHint".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.NotAnExplanation".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.ChitinMembranesAntennae".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.PowerfulSorceressDoubt".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.RegainComposure".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.UsualChairWine".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.ByMaraeRemark".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.RequestCutOff".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce.WhisperedHomeWish".Localized(
                Player.Mf("F-Father", "M-Mother")
            ));

            DoNext(() => HikkiCirce2());
        }

        public void HikkiCirce2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiCirce2.EmbraceBeforeConfront".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.CirceAvertsGaze".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.AncientWizardsApportation".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.StillPrettyGood".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.EnthusiasticResponse".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.NotingTheFumble".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.MothGirlBlushes".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.CandleEnergyShift".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.LocalizedFields".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.ApprehensionCuriosity".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.HazardsOfMagic".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.PrivateTalkRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.MeekNodBookshelf".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirce2.ManipulationWarning".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Agree".Localized(), () => HikkiCirceAnswer(0));

            AddNextButton("Common.Button.BrushOff".Localized(), () => HikkiCirceAnswer(1));

            AddNextButton("Common.Button.Hypocrisy".Localized(), () => HikkiCirceAnswer(2));

            AddNextButton("Common.Button.RemainSilent".Localized(), () => HikkiCirceAnswer(3));
        }

        public void HikkiCirceAnswer(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.UnpredictableProblems".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.WisdomInMyWords".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.SharedNod".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.PastProblemsSolved".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.FullGravityWarning".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.HypocriticalAccusation".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.EyesNarrowAnger".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.GlanceTowardDolores".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.AuthorityPlea".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.InquisitiveChallenge".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.CautiousUnknown".Localized());

                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.TensionFades".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.HoldGazeSilent".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.DoloresDriftsBack".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.WhereLearnedMagic".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.ReadInABook".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.HiddenMageRecall".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.BogCaveAnswer".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.WracksBrainGivesUp".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.AwareOfNone".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.AnxiousDeflection".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.EagerResponse".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.ClamsUpComfort".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.SmallMagicComfort".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.CoySmileSoftened".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.TimeToLeave".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven) & (int)CorruptedCovenFlags.BroughtJeremiahBack) != 0)
                OutputText("Scenes.Doloresscene.HikkiCirceAnswer.JeremiahResting".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.TeleportToCrag".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.SomeoneLikeThat".Localized());

            OutputText("Scenes.Doloresscene.HikkiCirceAnswer.SurpriseHugBlush".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQCIRC);

            DoNext(() => HikkiMenu());
        }

        public void HikkiSex()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiSex.CalmAndComfortable".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.SuitableSpotSearch".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.SmallGladeCover".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.MeaningfulLook".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.ObliviousDolores".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.LipsPressedKiss".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.SentenceStammers".Localized(
                Player.Mf("F-Father", "M-Mother")
            ));

            OutputText("Scenes.Doloresscene.HikkiSex.OutInTheOpen".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.StepCloserBlush".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.EmphaticGesture".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.AssureGoodTime".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex.MouthSlamsShut".Localized());

            DoNext(() => HikkiSex2());
        }

        public void HikkiSex2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiSex2.ApproachFromBehind".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex2.PlaceholderMoan".Localized(
                Player.Mf("F-Father", "M-Mother")
            ));

            OutputText("Scenes.Doloresscene.HikkiSex2.ToneLightsFire".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex2.TooBashfulToJoin".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Doloresscene.HikkiSex2.ReadyLittleNod".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.FaceAtHerSex".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.AdjustHipAlignment".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.ImpedimentsGone".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.HeavenlyRestraint".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.FrenziedHips".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.FirstSpurtShock".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.FinalFloodLeak".Localized());
            }
            else
            {
                OutputText("Scenes.Doloresscene.HikkiSex2.BackToTrunk".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.KneelDownBefore".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.SettlesTwoHands".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.BitTooMuch".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.DespiteProtestContinue".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.AmorousEffortsHand".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.LegsTightenNeck".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.HerClimaxTrigger".Localized());

                OutputText("Scenes.Doloresscene.HikkiSex2.RegainAwarenessMessy".Localized());
            }

            Player.Orgasm("Generic");

            DoNext(() => HikkiSex3());
        }

        public void HikkiSex3()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiSex3.QuietForestAfterglow".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex3.SofteningWithdraw".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex3.HowSheFeels".Localized());

            OutputText("Scenes.Doloresscene.HikkiSex3.TimeToMoveOn".Localized());

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQOUTD);

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

            DoNext(() => HikkiMenu());
        }

        public void HikkiFinished()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiFinished.TouredAllPlaces".Localized((Utils.Utils.CountSetBits(saveContent.hikkiQuest) == 1 ? "gentle" : "wide")));

            OutputText("Scenes.Doloresscene.HikkiFinished.HerSmileRises".Localized((Utils.Utils.CountSetBits(saveContent.hikkiQuest) == 1 ? "Well, even if we only went to one place," : "You've shown me so much, and")));

            OutputText("Scenes.Doloresscene.HikkiFinished.WalkHomeTogether".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.DaughterBreaksSilence".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.GladToSettleIn".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.DoubtSheSawAll".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.TravelersGreeting".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.WeaponDrawnOnStranger".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.FewTravelThisPath".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.TensionLowersSlightly".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.GrinsWithSlimyTeeth".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.AsksIfDemon".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.DemonChidesRudeness".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.AnnouncesNameBoldly".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.ChucklesRisingFromStump".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.RevealsRepugnantEyes".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.CarriesHerToSafety".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.SilentUnderstanding".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.ScoldsUpstartBrats".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.RevoltingEyesUpClose".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.HerAngerAndDisgust".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.DisarmingCalmVoice".Localized());

            OutputText("Scenes.Doloresscene.HikkiFinished.WeighRushOrListen".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fight".Localized(), HikkiFight);

            AddNextButton("Common.Button.HearOut".Localized(), HikkiHear);

            AddNextButton("Common.Button.Run".Localized(), HikkiRun);
        }

        public void HikkiRun()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRun.UrgeToFlee".Localized());

            OutputText("Scenes.Doloresscene.HikkiRun.GrabsArmAndRuns".Localized());

            OutputText("Scenes.Doloresscene.HikkiRun.SlapsKneesLaughing".Localized());

            OutputText("Scenes.Doloresscene.HikkiRun.BackAtTheStart".Localized());

            OutputText("Scenes.Doloresscene.HikkiRun.AsksIfReadyNow".Localized());

            OutputText("Scenes.Doloresscene.HikkiRun.MothOutOfBreath".Localized());

            AddButton(2, "Common.Button.Run".Localized(), null);
        }

        public void HikkiFight()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiFight.UnsureWhatSheWants".Localized());

            OutputText("Scenes.Doloresscene.HikkiFight.HardensHeartApproach".Localized());

            OutputText("Scenes.Doloresscene.HikkiFight.JeersAtYourManners".Localized());

            OutputText("Scenes.Doloresscene.HikkiFight.ThrowsCaneAndCharges".Localized());

            CurrentMonster = EnemyRegistry.Create(new Identifier("coc", "riddle_demon"));
            // 原版 DoloresScene.hx:1712：monster = new RiddleDemon(); startCombat(monster, true)；
            // 旧移植把 `monster, true` 整串塞进了 Identifier，生成不存在的 id "coc:monster, true"（创建即抛异常）
            CombatScene.Start(CurrentMonster, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void HikkiFightWin()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiFightWin.FallsAgainstStump".Localized());

            OutputText("Scenes.Doloresscene.HikkiFightWin.ConcedesThroughSneer".Localized((CurrentMonster!.Health < 1 ? "covered in his own blood" : "he can't stop ogling you")));

            CombatScene.CleanupAfterCombat();

            HikkiWinMenu();
        }

        public void HikkiFightLoss()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiFightLoss.LimbsRefuseOrders".Localized());

            OutputText("Scenes.Doloresscene.HikkiFightLoss.GruntsInSatisfaction".Localized());

            OutputText("Scenes.Doloresscene.HikkiFightLoss.RestWhileUnseen".Localized());

            OutputText("Scenes.Doloresscene.HikkiFightLoss.DismissesYourFate".Localized());

            OutputText("Scenes.Doloresscene.HikkiFightLoss.HandsAtYourThroat".Localized());

            GameOver();
        }

        public void HikkiHear()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiHear.WondersHerIntent".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.SettlesBackPleased".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.PredatoryIntentRemains".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.StatesGameRules".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.TurnsToRealTest".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.AsksAboutPenalty".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.StakesYourSoul".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.AsksIfYouWin".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.PromisesFreedom".Localized());

            OutputText("Scenes.Doloresscene.HikkiHear.DaughterSignalsReady".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Agree".Localized(), HikkiRiddle1);

            AddNextButton("Common.Button.Fight".Localized(), HikkiFight);
        }

        public void HikkiRiddle1()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddle1.AcceptsHisRules".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.CallsYouAlrightSort".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.ChucklesThenFreezes".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.OwnedThoughNotEarned".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.PraisedAndSpurned".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.MomentMakesItsWorth".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.NotBoundAtBirth".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1.CallsItEasyWarmup".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Life".Localized(), HikkiRiddle1Wrong);

            AddNextButton("Common.Button.Name".Localized(), HikkiRiddle2);

            AddNextButton("Common.Button.Years".Localized(), HikkiRiddle1Wrong);

            AddNextButton("Common.Button.Children".Localized(), HikkiRiddle1Wrong);

            AddNextButton("Common.Button.AskDolores".Localized(), HikkiAsk1);
        }

        public void HikkiAskFirst()
        {
            ClearOutput();

            if (!givenHint)
            {
                OutputText("Scenes.Doloresscene.HikkiAskFirst.ChildMayStillHelp".Localized());

                OutputText("Scenes.Doloresscene.HikkiAskFirst.DrawsHandAcrossThroat".Localized());

                OutputText("Scenes.Doloresscene.HikkiAskFirst.CannotCounterArgument".Localized());

                givenHint = true;
            }
        }

        public void HikkiAsk1()
        {

            HikkiAskFirst();

            OutputText("Scenes.Doloresscene.HikkiAsk1.MimesTheAnswer".Localized());

            RemoveButton(4);
        }

        public void HikkiRiddle1Wrong()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddle1Wrong.GuffawsAtYourGuess".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle1Wrong.DisappointedInFoe".Localized());

            HikkiRiddleBadEnd();
        }

        public void HikkiRiddle2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddle2.GuessedNamePlainly".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.GrudgingConcession".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladOpensCerallo".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladUnrulyLand".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladHollowWords".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladGrandCruelty".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladBefellCerallo".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladSlightedQueen".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladPrideNotSwallowed".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladHerExcellency".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladPunishedLad".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladBodyMaimed".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladApportionmentFollows".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladPartsEquallyShamed".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladSorrowWallowed".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladBodyHeadMismatch".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladAsksCeralloFate".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2.BalladStruckDownDead".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Hung".Localized(), HikkiRiddle2Wrong);

            AddNextButton("Common.Button.Impaled".Localized(), HikkiRiddle2Wrong);

            AddNextButton("Common.Button.Decapitated".Localized(), HikkiRiddle2Wrong);

            AddNextButton("Common.Button.Bifrucated".Localized(), HikkiRiddle3);

            AddNextButton("Common.Button.AskDolores".Localized(), HikkiAsk2);
        }

        public void HikkiAsk2()
        {

            HikkiAskFirst();

            OutputText("Scenes.Doloresscene.HikkiAsk2.StrugglesToAnswer".Localized());

            RemoveButton(4);
        }

        public void HikkiRiddle2Wrong()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddle2Wrong.TentativeWrongGuess".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle2Wrong.HopedForThirdRiddle".Localized());

            HikkiRiddleBadEnd();
        }

        public void HikkiRiddle3()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddle3.AnswersCutInHalf".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RecoversAndPraise".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddlePrinceUndecided".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleContestDecreed".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleEveryGirlChance".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddlePluckAndLuck".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleDaughtersAndMother".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleYearnedForLove".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleDrewOneLot".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleSwappingBegan".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleSingleCrownBallot".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleMotherSwapped".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleYoungerSisterSwap".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleRavenLassDupes".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleMiddleSisterTheft".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddle3.RiddleQueenQuestion".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Mother".Localized(), HikkiRiddle3Wrong);

            AddNextButton("Common.Button.Oldest".Localized(), HikkiRiddleWin);

            AddNextButton("Common.Button.Middle".Localized(), HikkiRiddle3Wrong);

            AddNextButton("Common.Button.Youngest".Localized(), HikkiRiddle3Wrong);

            AddNextButton("Common.Button.AskDolores".Localized(), HikkiAsk3);
        }

        public void HikkiAsk3()
        {

            HikkiAskFirst();

            OutputText("Scenes.Doloresscene.HikkiAsk3.DrawsDiagramInDirt".Localized());

            RemoveButton(4);
        }

        public void HikkiRiddle3Wrong()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddle3Wrong.MocksYourLogic".Localized());

            HikkiRiddleBadEnd();
        }

        public void HikkiRiddleBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.FrozenInPlace".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.MocksYourParalysis".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.ApproachesWithoutFinePrint".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.FunAndLifeCoincide".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.JawRefusesAnswer".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.DevilClaimsHisDue".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleBadEnd.VisionFadesAway".Localized());

            GameOver();
        }

        public void HikkiRiddleWin()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddleWin.IronicApplause".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleWin.DemandsHerFreedom".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleWin.PerformsComplexGesture".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleWin.WeakHandOnChest".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleWin.DemonFateQuestion".Localized());

            if (!((Save.GetInt("hikkiQuest", 0) & HQFREE) != 0))
                guessedName = false;

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQFREE);

            ClearButtons();

            AddNextButton("Common.Button.Fight".Localized(), HikkiRiddleFight);

            AddNextButton("Common.Button.Talk".Localized(), HikkiTalk);

            AddNextButton("Common.Button.Taunt".Localized(), HikkiRiddleTaunt);

            AddNextButton("Common.Button.Dolores".Localized(), HikkiRiddleDolores);

            AddNextButton("Common.Button.Leave".Localized(), HikkiRiddleLeave);
        }

        public void HikkiRiddleFight()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddleFight.NothingHoldsBack".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleFight.MocksYourExhaustion".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleFight.CreakyClawedCharge".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleFight.FollowsYourLead".Localized());

            CurrentMonster = EnemyRegistry.Create(new Identifier("coc", "riddle_demon"));
            // 原版 DoloresScene.hx:1712：monster = new RiddleDemon(); startCombat(monster, true)；
            // 旧移植把 `monster, true` 整串塞进了 Identifier，生成不存在的 id "coc:monster, true"（创建即抛异常）
            CombatScene.Start(CurrentMonster, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void HikkiRiddleTaunt()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.ShoutForAttention".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.SneeringReply".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.LossStingsElder".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.PallidSkinColors".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.MockingPraise".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.ShockinglyFakeSmile".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.WhiteKnuckledGrip".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleTaunt.SatisfyingConclusion".Localized());

            DoNext(() => HikkiEnd());
        }

        public void HikkiRiddleDolores()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddleDolores.TakeHerAside".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleDolores.SmileReassurance".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleDolores.BlushFallsSilent".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleDolores.DecideDemonFate".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleDolores.SurprisedByHerRequest".Localized());

            OutputText("Scenes.Doloresscene.HikkiRiddleDolores.AcceptanceWithoutReason".Localized());

            AddButton(3, "Common.Button.Dolores".Localized(), null);
        }

        public void HikkiRiddleLeave()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiRiddleLeave.GestureToFollow".Localized());

            DoNext(() => HikkiEnd());
        }

        public void HikkiTalk()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiTalk.ProposeToTalk".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalk.TalkWithMortal".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalk.PersistDespiteAnswer".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Purpose".Localized(), HikkiTalkPurpose);

            AddNextButton("Common.Button.Riddles".Localized(), HikkiTalkRiddles);

            AddNextButton("Common.Button.Himself".Localized(), HikkiTalkHimself);

            if (demonTalks >= 3)
                AddNextButton("Common.Button.GuessName".Localized(), HikkiGuessName);

            AddButton(14, "Common.Button.Back".Localized(), () => HikkiRiddleWin());
        }

        public void HikkiDiscover()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiDiscover.DoloresTugsArm".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.SomethingStrangeGoing".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.QuestioningHerIdea".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.HiddenNameClue".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.HowToFindName".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.MischievousGrinClue".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.ContractualObligations".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.NoddingAgreement".Localized());

            OutputText("Scenes.Doloresscene.HikkiDiscover.GuessFromClues".Localized());

            DoNext(() => HikkiTalk());
        }

        public void HikkiTalkPurpose()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiTalkPurpose.WhyChallengeYou".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkPurpose.ClaimsFriendlyGame".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkPurpose.PointOutDiscontent".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkPurpose.PluckYourEyeballs".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkPurpose.LostSilentGaze".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkPurpose.TappingBootImpatience".Localized());

            AddButton(0, "Common.Button.Purpose".Localized(), null);

            if (++demonTalks == 3)
                DoNext(() => HikkiDiscover());
        }

        public void HikkiTalkRiddles()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiTalkRiddles.AskRiddleOrigin".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkRiddles.WritesOwnRiddles".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkRiddles.RhymesAreMine".Localized());

            ClearButtons();

            AddNextButton("Common.Button.AboutRight".Localized(), () => HikkiTalkRiddlesAnswer(0));

            AddNextButton("Common.Button.LikedThem".Localized(), () => HikkiTalkRiddlesAnswer(1));
        }

        public void HikkiTalkRiddlesAnswer(int choice)
        {
            ClearOutput();

            HikkiTalk();

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.HikkiTalkRiddlesAnswer.ShrugAtFancyWords".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.HikkiTalkRiddlesAnswer.DignifiedMortals".Localized());

                OutputText("Scenes.Doloresscene.HikkiTalkRiddlesAnswer.DemonSagsSilent".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.HikkiTalkRiddlesAnswer.ContemplativeLull".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkRiddlesAnswer.LooksAwayRefusal".Localized());

            AddButton(1, "Common.Button.Riddles".Localized(), null);

            if (++demonTalks == 3)
                DoNext(() => HikkiDiscover());
        }

        public void HikkiTalkHimself()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.TellAboutHimself".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.AttemptedSmallTalk".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.InsistOnAnswers".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.FellowTraveler".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.DoloresIntervenes".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.FirstNameBasis".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.ProudLittleMan".Localized());

            OutputText("Scenes.Doloresscene.HikkiTalkHimself.ShoutThenSullen".Localized());

            AddButton(2, "Common.Button.Himself".Localized(), null);

            if (++demonTalks == 3)
                DoNext(() => HikkiDiscover());
        }

        public void HikkiGuessName()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiGuessName.YourGuessPrompt".Localized());

            DoNext(() => HikkiGuessNameCheck());

        }

        public void HikkiGuessNameCheck()
        {
            ClearOutput();

            if (guessedName)
                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.BestGuessGiven".Localized());
            else
            {
                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.MusteringYourWill".Localized());

                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.DoubtsYourFantasy".Localized());

                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.GiveHimGuess".Localized());
            }

            if ("" .ToLower() == "allocer")
            {
                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.DefeatedButSuspicious".Localized());

                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.PronunciationOff".Localized());

                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.WhoseRulesQuery".Localized());

                OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.EyelidsDrooping".Localized());

                Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQNAME);

                HikkiWinMenu();
            }
            else
                if (guessedName)
                {
                    OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.GrimacingTeeth".Localized());

                    OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.DemonTipsBackward".Localized());

                    OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.SuggestMovingOn".Localized());

                    DoNext(() => HikkiEnd());
                }
                else
                {
                    OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.AccusingSneer".Localized());

                    OutputText("Scenes.Doloresscene.HikkiGuessNameCheck.NoseTurnsUp".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.GuessAgain".Localized(), HikkiGuessName);

                    AddButton(14, "Common.Button.Back".Localized(), () => HikkiRiddleWin());

                    guessedName = true;
                }
        }

        public void HikkiWinMenu()
        {

            ClearButtons();

            AddNextButton("Common.Button.Riches".Localized(), HikkiWinRiches);

            AddNextButton("Common.Button.Knowledge".Localized(), HikkiWinKnowledge);

            AddNextButton("Common.Button.Pleasure".Localized(), HikkiWinPleasure);

            AddNextButton("Common.Button.HisDeath".Localized(), HikkiWinDeath);

            AddNextButton("Common.Button.Dolores".Localized(), HikkiWinDolores);

            AddButton(14, "Common.Button.Leave".Localized(), () => HikkiWinLeave());
        }

        public void HikkiWinRiches()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiWinRiches.AppealToGreed".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinRiches.OffersShinyTrinkets".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinRiches.SnatchProfferedTreasure".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinRiches.YoungSapTrinket".Localized());

            InventoryScene.TakeItem(ItemIds.FabulousRing, HikkiWinReward);
        }

        public void HikkiWinKnowledge()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiWinKnowledge.AppealToExperience".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinKnowledge.FlatteryRebuffed".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinKnowledge.WhisperedSecret".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinKnowledge.FecklessDimwit".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinKnowledge.NoticingDetails".Localized());

            Player.CreatePerk(PerkIds.RiddleSight);

            HikkiWinReward();
        }

        public void HikkiWinPleasure()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.TiringRequestPleasure".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.MockingGrinThroughout".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.GuessingHisIntent".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.CrispSnapWarning".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.CompleteInundation".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.MockingGazeAbove".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.TappingFingersConcern".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.CheckingOwnBody".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinPleasure.InformDaughterStatus".Localized());

            Player.Orgasm("Generic");

            HikkiWinReward();
        }

        public void HikkiWinDeath()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiWinDeath.RaisingYourWeapon".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.PointAlreadyProven".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.DemandUtterEnd".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.DemandsYourWish".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.ClosedTheDistance".Localized());

            if (Player.Weapon.IsBladed())
                OutputText("Scenes.Doloresscene.HikkiWinDeath.BladeSwipeAttack".Localized());
            else
                if (Player.Weapon.IsFirearm() || Player.Weapon.IsChanneling(Player))
                    OutputText("Scenes.Doloresscene.HikkiWinDeath.ShotFliesAtHim".Localized());
                else
                    if (Player.Weapon.IsBlunt())
                        OutputText("Scenes.Doloresscene.HikkiWinDeath.DeadlyArcSwing".Localized());
                    else
                        OutputText("Scenes.Doloresscene.HikkiWinDeath.FistSwingsFace".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.CareeningToGround".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.DesperateOffer".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.RoundingTheStump".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.ExhaustedPatience".Localized());

            if (Player.Weapon.IsBladed())
                OutputText("Scenes.Doloresscene.HikkiWinDeath.OverhandCleave".Localized());
            else
                if (Player.Weapon.IsFirearm() || Player.Weapon.IsChanneling(Player))
                    OutputText("Scenes.Doloresscene.HikkiWinDeath.PointBlankShot".Localized());
                else
                    if (Player.Weapon.IsBlunt())
                        OutputText("Scenes.Doloresscene.HikkiWinDeath.SidewaysWristSwipe".Localized());
                    else
                        OutputText("Scenes.Doloresscene.HikkiWinDeath.GrabAndPummel".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.FollowUpAttacks".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.TurnBackToHer".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.GratefulDespiteHorror".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDeath.PatHerBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -3f));

            Player.UpgradeBeautifulSword();

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQKILL);

            DoNext(() => HikkiEnd());
        }

        public void HikkiWinDolores()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiWinDolores.AskingHerOpinion".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDolores.FlusteredHesitation".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinDolores.InsistOnHerAnswer".Localized());

            if ((Save.GetInt("hikkiQuest", 0) & HQNAME) != 0)
            {
                OutputText("Scenes.Doloresscene.HikkiWinDolores.TakingWordsToHeart".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.AskSpecifications".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.ThingsAndServices".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.ServicesClarification".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.ShrugsAndWinces".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.RepeatingRequests".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.NarrowedEyesAnswer".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.TellNameToEveryone".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.DistortedGrimace".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.HobblesIntoForest".Localized());

                DoNext(() => HikkiEnd());
            }
            else
            {
                OutputText("Scenes.Doloresscene.HikkiWinDolores.BlushingGratitude".Localized());

                OutputText("Scenes.Doloresscene.HikkiWinDolores.NodsAndTurnsAway".Localized());

                AddButton(4, "Common.Button.Dolores".Localized(), null);
            }
        }

        public void HikkiWinLeave()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.HikkiWinLeave.LeaveWithoutAnswer".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinLeave.StalwartMockery".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinLeave.UninterestedInHim".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinLeave.RoaringDemands".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinLeave.ShoutsFadeBehind".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            DoNext(() => HikkiEnd());
        }

        public void HikkiWinReward()
        {

            OutputText("Scenes.Doloresscene.HikkiWinReward.DebtsPaidHeadSnaps".Localized());

            OutputText("Scenes.Doloresscene.HikkiWinReward.LookButOnlyRags".Localized());

            DoNext(() => HikkiEnd());
        }

        public void HikkiEnd()
        {
            ClearOutput();

            DoloresReset();

            OutputText("Scenes.Doloresscene.HikkiEnd.WearilyWalkHome".Localized());

            OutputText("Scenes.Doloresscene.HikkiEnd.SlurredThankYou".Localized());

            OutputText("Scenes.Doloresscene.HikkiEnd.WhisperedLoveDad".Localized());

            OutputText("Scenes.Doloresscene.HikkiEnd.ComfortableSilence".Localized());

            OutputText("Scenes.Doloresscene.HikkiEnd.TuckingIntoBed".Localized());

            Save.SetInt("hikkiQuest", Save.GetInt("hikkiQuest", 0) | HQDONE);

            Save.SetInt("doloresProgress", 17);

            View.Time.PassMinutes((int)Math.Round((double)((23 - View.Time.Hours) * 60)));

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresMenu()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMenu.WalkInVisit".Localized());

            if (doloresProg < 3)
                OutputText("Scenes.Doloresscene.DoloresMenu.NewbornInCrib".Localized());
            else
                if (doloresProg < 7)
                    OutputText("Scenes.Doloresscene.DoloresMenu.YoungGirlReading".Localized((doloresProg > 3 ? "mumbling out a, [say: Hello, [Father],] and " : "")));
                else
                    if (doloresProg < 9)
                        OutputText("Scenes.Doloresscene.DoloresMenu.CocoonImmobile".Localized());
                    else
                        if (HikkiDone())
                            OutputText("Scenes.Doloresscene.DoloresMenu.RushOverForHug".Localized());
                        else
                            OutputText("Scenes.Doloresscene.DoloresMenu.AdolescentAtDesk".Localized((saveContent.doloresDecision == 1 ? "magical" : "historical")));

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), DoloresAppearance);

            if (doloresProg > 4)
            {
                AddButton(1, "Common.Button.Talk".Localized(), DoloresTalkMenu);

                AddButton(5, "Common.Button.Headpat".Localized(), DoloresHeadpat);
            }

            if (Save.GetInt("DoloresSex", 0) > 0)
                AddButton(2, "Common.Button.Sex".Localized(), DoloresSexMenu);

            if (Save.GetInt("doloresDecision", 0) == 3 && doloresProg > 8 && doloresProg < 12)
                AddRowButton(2, "Give Book", GiveBook);

            if (Player.HasKeyItem("Old Manor Books"))
                AddRowButton(2, "Give Books", GiveManorBooks);

            if (doloresProg == 16)
                if (Save.GetInt("hikkiQuest", 0) == 0)
                    AddRowButton(2, "Travel", HikkiAccept);
                else
                    AddRowButton(2, "Continue", HikkiContinue);

            SetExitButton("Common.Button.Back".Localized(), () => SceneInstance<MothCaveScene>.Get().CaveMenu());
        }

        public void DoloresAppearance()
        {
            ClearOutput();

            if (doloresProg == 2)
                OutputText("Scenes.Doloresscene.DoloresAppearance.InfantCaterpillar".Localized((Player.Corruption > 80 && silly ? "You find yourself having some particularly Satanic thoughts." : "")));
            else
                if (doloresProg < 7)
                    OutputText("Scenes.Doloresscene.DoloresAppearance.CaterpillarGrowing".Localized(
                        doloresProg < 5 ? (metric ? "122 cm" : "four feet") : (metric ? "140 cm" : "4'7''")
                    ));
                else
                    if (doloresProg < 9)
                        OutputText("Scenes.Doloresscene.DoloresAppearance.EncasedInSilkCocoon".Localized((doloresTime < 96 ? "Its fluffy exterior looks so soft that you almost want to hug it, but it's probably not a good idea to be too rough with it while she's still inside." : "Its previously fluffy exterior has hardened somewhat since it was first formed. Does this mean that Dolores is going to emerge soon?")));
                    else
                        OutputText("Scenes.Doloresscene.DoloresAppearance.PetiteFrameHeight".Localized((metric ? "150 cm" : "five feet"), (doloresProg > 9 ? "brilliant" : "somewhat droopy")));

            DoNext(() => DoloresMenu());
        }

        public void DoloresTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTalkMenu.TalkTopicPrompt".Localized());

            ClearButtons();

            AddNextButton("Common.Button.JustChat".Localized(), DoloresJustChat);

            AddNextButton("Npcs.Sylvia.Name".Localized(), DoloresSylviaChat);

            AddNextButton("Common.Button.Literature".Localized(), DoloresLitChat);

            if (doloresProg > 5)
                AddNextButton("Common.Button.Home".Localized(), DoloresHomeChat);

            if ((doloresProg > 8 && Save.GetInt("doloresAmbitions", 0) < 1) || doloresProg > 12)
                AddNextButton("Codex.Category.Magic".Localized(), DoloresMagicChat);

            if (doloresProg > 8)
                AddNextButton("Common.Button.Sex".Localized(), DoloresSexChat);

            if (doloresProg > 9)
                AddNextButton("Common.Button.Wings".Localized(), DoloresWingsChat);

            if (doloresProg > 9 && Save.GetInt("doloresAmbitions", 0) == 1)
                AddNextButton("Common.Button.Ambition".Localized(), DoloresAmbitionChat);

            AddButton(14, "Common.Button.Back".Localized(), DoloresMenu);
        }

        public void DoloresJustChat()
        {
            ClearOutput();

            var choices = new List<int> { 1, 2, 3, 4, 7 };

            if (doloresProg > 9)
                choices.Add(5);

            if (HikkiDone())
                choices.Add(6);

            switch (choices[Random.Shared.Next(choices.Count)])
            {
                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.AskWhySheTalks".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.MeaningQuestion".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.VerboseHabit".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.RefinedExcuse".Localized());

                ClearButtons();

                AddNextButton("Common.Button.ItsFine".Localized(), DoloresJustChatFine);

                AddNextButton("Common.Button.Problem".Localized(), DoloresJustChatProblem);

                return ;
                }

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.RecentReadingAsk".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.MagicalTreatise".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.TopicBoredom".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.WhyWearClothes".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.ImproperQuestion".Localized());

                if (Save.GetInt("DoloresSex", 0) > 2)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.DeepeningBlush".Localized());

                if ((Save.GetInt("hikkiQuest", 0) & HQMARI) != 0)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.EyesLightUp".Localized());
                else
                    OutputText("Scenes.Doloresscene.DoloresJustChat.PoutyRefusal".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.LatelyAnythingAsk".Localized());

                choices = [];

                if ((CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress) & 2048) != 0)
                    choices.Add(1);

                if (CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                    choices.Add(2);

                if ((CoC.Instance.Flags.Get(GameFlagIds.ManorProgress) & 128) != 0)
                    choices.Add(3);

                var __sw1_1 = choices[Random.Shared.Next(choices.Count)];

                if (__sw1_1 == 1)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.InquisitorTower".Localized());

                if (__sw1_1 == 2)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.DemonQueenStronghold".Localized((Player.Corruption < 30 ? "heroism" : "bravery")));

                if (__sw1_1 == 3)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.CursedManor".Localized());

                if (true )
                    OutputText("Scenes.Doloresscene.DoloresJustChat.MarethTravels".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.RaptAttention".Localized((doloresProg < 9 ? "caterpillar" : "moth")));
                }

                break;

                case 5:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.FlyingExcitement".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.DistractedReply".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.FlightUninteresting".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.RarelyFliesCave".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.ShruggedSuppose".Localized());
                }

                break;

                case 6:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.LonelyQuestion".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.NoseUpDenial".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.StammeredReassurance".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.ContentIfHappy".Localized());

                if ((Save.GetInt("hikkiQuest", 0) & HQMARI) != 0)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.SubjectReopened".Localized());
                }

                break;

                case 7:
                {
                OutputText("Scenes.Doloresscene.DoloresJustChat.FavoriteFoodAsk".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.ArmRubbingReply".Localized());

                OutputText("Scenes.Doloresscene.DoloresJustChat.BlandFoodDislike".Localized());

                if (doloresProg < 9)
                    OutputText("Scenes.Doloresscene.DoloresJustChat.GrowingNeedsVariety".Localized());
                else
                {
                    OutputText("Scenes.Doloresscene.DoloresJustChat.FoodOrEating".Localized());

                    OutputText("Scenes.Doloresscene.DoloresJustChat.PoorSelection".Localized());

                    OutputText("Scenes.Doloresscene.DoloresJustChat.SilentHeadTurned".Localized(
                        Save.GetInt("doloresProgress", 0) >= 15 ? "But after a few moments, she continues, [say:Well, perhaps it wouldn't be so unpleasant. " + (HikkiDone() ? "It's good to get out of the house every now and then, as you've shown me" : "Even if it would mean going out into the bog") + ".]" : "It seems like she's reluctant to say any more."
                    ));
                }
                }

                break;
            }

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresJustChatFine()
        {

            OutputText("Scenes.Doloresscene.DoloresJustChatFine.SmugApproval".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresJustChatProblem()
        {

            OutputText("Scenes.Doloresscene.DoloresJustChatProblem.HaughtyFrown".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresSylviaChat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSylviaChat.CoastClearAsk".Localized());

            if (doloresProg < 9)
            {
                OutputText("Scenes.Doloresscene.DoloresSylviaChat.MumbledHatred".Localized());

                OutputText("Scenes.Doloresscene.DoloresSylviaChat.GazeDropsFloor".Localized());

                OutputText("Scenes.Doloresscene.DoloresSylviaChat.NormalForKid".Localized());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresSylviaChat.BashfulArmRub".Localized());

                OutputText("Scenes.Doloresscene.DoloresSylviaChat.ThoughtfulPause".Localized((doloresProg > 14 ? "Especially the tapestry. I've... had my doubts in the past, but she really does understand me..." : "But... I'm just not sure that she really </i>understands<i> me.")));

                OutputText("Scenes.Doloresscene.DoloresSylviaChat.ApologeticEyes".Localized((doloresProg > 14 ? "get a little bit misty" : "drop down to look at her feet")));
            }

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresLitChat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresLitChat.LiteratureOpinions".Localized((doloresProg < 9 ? "developing quite the interest in books" : "read a lot of books")));

            if (doloresProg < 9)
            {
                OutputText("Scenes.Doloresscene.DoloresLitChat.BashfulPreferences".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitChat.LowbrowButFine".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitChat.ImaginationReading".Localized());
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresLitChat.ChinTapAnswer".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitChat.ProperAnswerResumes".Localized());
            }

            OutputText("Scenes.Doloresscene.DoloresLitChat.BookTasteQuestion".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Adventure".Localized(), () => DoloresLitAnswer(0));

            AddNextButton("Common.Button.Nonfiction".Localized(), () => DoloresLitAnswer(1));

            AddNextButton("Common.Button.Romance".Localized(), () => DoloresLitAnswer(2));

            AddNextButton("Common.Button.None".Localized(), () => DoloresLitAnswer(3));
        }

        public void DoloresLitAnswer(int choice)
        {

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.DoloresLitAnswer.DangerDerringDo".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitAnswer.HiddenSmile".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresLitAnswer.RealSubjectBooks".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitAnswer.ContemplativeLook".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresLitAnswer.StarCrossedRomance".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitAnswer.PoliteAcknowledge".Localized((Save.GetInt("doloresSex", 0) > 2 ? "blushing slightly" : "wrinkling your nose a bit")));
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresLitAnswer.BookDislikeAll".Localized());

                OutputText("Scenes.Doloresscene.DoloresLitAnswer.DisappointedLook".Localized());
                }

                break;
            }

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresHomeChat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresHomeChat.HomeOpinion".Localized());

            OutputText("Scenes.Doloresscene.DoloresHomeChat.BlankStareHome".Localized());

            OutputText("Scenes.Doloresscene.DoloresHomeChat.OldBooksPraise".Localized());

            OutputText("Scenes.Doloresscene.DoloresHomeChat.SwampDistaste".Localized((doloresProg > 9 ? "caterpillar" : "moth-girl")));

            if (HikkiDone())
                OutputText("Scenes.Doloresscene.DoloresHomeChat.PlacesRecalled".Localized(
                    (Save.GetInt("hikkiQuest", 0) & HQCAMP) != 0 ? " Your encampment was quite quaint" + ((Save.GetInt("hikkiQuest", 0) & HQMARI) != 0 ? ", and" : ".") : "",

                    (Save.GetInt("hikkiQuest", 0) & HQMARI) != 0 ? " " + ((Save.GetInt("hikkiQuest", 0) & HQCAMP) != 0 ? "t" : "T") + "hat temple that your undead friend lived in was a most relaxing sanctuary." : ""
                ));

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresMagicChat()
        {
            ClearOutput();

            if (doloresProg < 12)
            {
                OutputText("Scenes.Doloresscene.DoloresMagicChat.StillStudyingMagic".Localized());

                OutputText("Scenes.Doloresscene.DoloresMagicChat.FingerToCheek".Localized((saveContent.doloresDecision == 1 ? "excitedly, [say: and it's all thanks to you.]" : ", somewhat pointedly, [say: despite your... objections.]")));

                if (Save.GetInt("doloresDecision", 0) == 1)
                {
                    OutputText("Scenes.Doloresscene.DoloresMagicChat.EyesClosedFourHands".Localized());

                    OutputText("Scenes.Doloresscene.DoloresMagicChat.DiamondHandPattern".Localized());

                    OutputText("Scenes.Doloresscene.DoloresMagicChat.UnderwaterSensation".Localized());

                    OutputText("Scenes.Doloresscene.DoloresMagicChat.SelfSatisfiedAsk".Localized());

                    ClearButtons();

                    AddNextButton("Common.Button.Amazing".Localized(), DoloresMagicAmazing);

                    AddNextButton("Common.Button.Notspecial".Localized(), DoloresMagicEh);

                    AddNextButton("Common.Button.Dangerous".Localized(), DoloresMagicDangerous);
                }
                else
                {
                    OutputText("Scenes.Doloresscene.DoloresMagicChat.LingeringDismissal".Localized());

                    OutputText("Scenes.Doloresscene.DoloresMagicChat.PressHerChoice".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), DoloresMagicYes);

                    AddButton(1, "Common.Button.No".Localized(), DoloresMagicNo);
                }

                Save.SetInt("doloresAmbitions", 1);
            }
            else
            {
                OutputText("Scenes.Doloresscene.DoloresMagicChat.MixedResultsCaution".Localized());

                OutputText("Scenes.Doloresscene.DoloresMagicChat.CrypticWearySmile".Localized());

                OutputText("Scenes.Doloresscene.DoloresMagicChat.LoveBookMystery".Localized((saveContent.doloresFinal % 10 == 1 ? " almost" : "")));

                OutputText("Scenes.Doloresscene.DoloresMagicChat.FiddlingHemSmile".Localized());

                OutputText("Scenes.Doloresscene.DoloresMagicChat.HowMagicFeels".Localized());

                OutputText("Scenes.Doloresscene.DoloresMagicChat.ExquisiteDelicacy".Localized());

                DoNext(() => DoloresTalkMenu());
            }
        }

        public void DoloresMagicAmazing()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicAmazing.ImpressedByDisplay".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicAmazing.YouthfulPride".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicAmazing.InfectiousDelight".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicAmazing.SmileFullBloom".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresMagicEh()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicEh.ShrugUnimpressed".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicEh.PridefulFacade".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicEh.ExitRoomHurt".Localized());

            Save.SetBool("doloresAngry", true);

            SceneInstance<MothCaveScene>.Get().CaveMenu();

        }

        public void DoloresMagicDangerous()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicDangerous.OddSensationDoubt".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicDangerous.QuizzicalConfusion".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicDangerous.UncertainRecall".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicDangerous.ScoffingRetort".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicDangerous.EverythingOkay".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresMagicYes()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicYes.FesteringConcern".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicYes.NarrowedEyesRetort".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicYes.WantsToBeAlone".Localized());

            OutputText("Scenes.Doloresscene.DoloresMagicYes.ExitRoomRawTopic".Localized());

            Save.SetBool("doloresAngry", true);

            SceneInstance<MothCaveScene>.Get().CaveMenu();

        }

        public void DoloresMagicNo()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresMagicNo.PatientRestraint".Localized((Player.Corruption > 50 ? "you'll just have to teach her another lesson" : "you'll still be there for her")));

            OutputText("Scenes.Doloresscene.DoloresMagicNo.RegainedComposure".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresSexChat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSexChat.GrowingGirlMatters".Localized((Player.IsChild() ? ", although you aren't much more experienced," : ""), (Player.Corruption < 50 ? "These matters are fairly delicate, and you don't want to offend her" : "There're so many fun things she has yet to learn that you're overwhelmed by the possibilities")));

            OutputText("Scenes.Doloresscene.DoloresSexChat.NoncommittalGrunt".Localized((Player.IsChild() ? "not too long ago" : "from your childhood")));

            OutputText("Scenes.Doloresscene.DoloresSexChat.AnatomyInterrupted".Localized());

            OutputText("Scenes.Doloresscene.DoloresSexChat.PartingAdviceOffer".Localized());

            ClearButtons();

            AddNextButton("Common.Button.BeSafe".Localized(), () => DoloresSexAnswer(1));

            AddNextButton("Common.Button.BeWithMe".Localized(), () => DoloresSexAnswer(2));

            AddNextButton("Common.Button.Back".Localized(), () => DoloresSexAnswer(3));
        }

        public void DoloresSexAnswer(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresSexAnswer.MakeSureSafe".Localized((Player.Tallness > 60 ? "your chest" : "yours")));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresSexAnswer.LoveAffirmed".Localized());

                OutputText("Scenes.Doloresscene.DoloresSexAnswer.AffectionKiss".Localized());

                if (Save.GetInt("DoloresSex", 0) < 1)
                    Save.SetInt("DoloresSex", 1);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresSexAnswer.AwkwardnessRelieved".Localized());
                }

                break;
            }

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresWingsChat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresWingsChat.WingInspectRequest".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsChat.TurnToFaceWall".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsChat.RadiantWingWaves".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsChat.HiddenWingsAwe".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsChat.UnflatteredResponse".Localized());

            if (Save.GetInt("DoloresSex", 0) > 0)
                OutputText("Scenes.Doloresscene.DoloresWingsChat.LecherInspectTaunt".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresWingsChat.FinishedInspectAsk".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Compliment".Localized(), DoloresWingsCompliment);

            AddNextButton("Common.Button.Tease".Localized(), DoloresWingsTease);

            AddNextButton("Common.Button.ThatsAll".Localized(), DoloresWingsThatsAll);
        }

        public void DoloresWingsThatsAll()
        {

            OutputText("Scenes.Doloresscene.DoloresWingsThatsAll.QuietAcceptance".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresWingsCompliment()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresWingsCompliment.FinalCompliment".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsCompliment.TensionRelease".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsCompliment.SweetSmileAnswer".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsCompliment.MoveOnDecision".Localized());

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresWingsTease()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresWingsTease.LastTease".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsTease.GetOutDemand".Localized());

            OutputText("Scenes.Doloresscene.DoloresWingsTease.DoorwayShove".Localized());

            Save.SetBool("doloresAngry", true);

            SceneInstance<MothCaveScene>.Get().CaveMenu();

        }

        public void DoloresAmbitionChat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresAmbitionChat.LifeAmbitionAsk".Localized());

            OutputText("Scenes.Doloresscene.DoloresAmbitionChat.PeevedRetort".Localized());

            if (Save.GetInt("doloresDecision", 0) == 1)
                OutputText("Scenes.Doloresscene.DoloresAmbitionChat.PublishDream".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresAmbitionChat.PointedGlanceJab".Localized());

            OutputText("Scenes.Doloresscene.DoloresAmbitionChat.UnnervingSmileTrail".Localized());

            OutputText("Scenes.Doloresscene.DoloresAmbitionChat.CounterAmbitionAsk".Localized(
                Player.Mf("mother", "my other parent")
            ));

            ClearButtons();

            AddNextButton("Common.Button.Demons".Localized(), () => DoloresAmbitionAnswer(0));

            AddNextButton("Scenes.PlayerAppearance.Name".Localized(), () => DoloresAmbitionAnswer(1));

            AddNextButton("Common.Button.Fun".Localized(), () => DoloresAmbitionAnswer(2));

            AddNextButton("Common.Button.Lovin".Localized(), () => DoloresAmbitionAnswer(3));

            AddNextButton("Common.Button.DontKnow".Localized(), () => DoloresAmbitionAnswer(4));
        }

        public void DoloresAmbitionAnswer(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                {
                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.VanquishScourgeAnswer".Localized());

                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.NobleButReply".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.LawlessClaim".Localized());

                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.PerturbedDoubt".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.OpenRoadCall".Localized());

                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.DashingPraise".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.RomanticVagabond".Localized());

                if (Save.GetInt("DoloresSex", 0) > 0)
                    OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.BrightRedBlush".Localized());
                else
                    OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.NoseWrinkleScoff".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.RestlessDrive".Localized());

                OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.ReasonableVerdict".Localized());
                }

                break;
            }

            OutputText("Scenes.Doloresscene.DoloresAmbitionAnswer.NeutralRelent".Localized());

            Save.SetInt("doloresAmbitions", 2);

            DoNext(() => DoloresTalkMenu());
        }

        public void DoloresHeadpat()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresHeadpat.DotingHeadpat".Localized());

            OutputText("Scenes.Doloresscene.DoloresHeadpat.HandOnHead".Localized((doloresProg < 9 ? "caterpillar" : "moth"), (Player.Tallness > 48 ? "lower" : "raise"), (Player.Tallness > 48 ? "down" : "up"), (Player.Tallness > 48 ? "up" : "down"), (doloresProg > 13 ? "apparently still a bit skeptical of the point of all this" : "clearly not understanding the point of all this")));

            OutputText("Scenes.Doloresscene.DoloresHeadpat.SteadyRhythm".Localized((doloresProg > 13 ? "lingering hesitance" : "apparent inexperience")));

            OutputText("Scenes.Doloresscene.DoloresHeadpat.RedoubledDoting".Localized((silly ? "concentric circular rubs, intense lateral stroking, even the reverse triple scalp-scrubber" : "stroking her hair, firm rubbing, even a light caress of her cheek")));

            OutputText("Scenes.Doloresscene.DoloresHeadpat.ImpassiveResistance".Localized((doloresProg < 9 ? "caterpillar" : "moth")));

            OutputText("Scenes.Doloresscene.DoloresHeadpat.HopefulGlance".Localized((doloresProg > 13 ? "After all we've been through, I'm surprised that such a simple gesture can make me feel so... at peace. Thank you, [Father].] You're not sure how your chest can still contain your joyous, beating heart" : "I'm not entirely certain what you were trying to accomplish with that, [Father], but it gladdens me to know that you would go through all of that for my sake.] Well, it seems like you weren't completely unsuccessful after all")));

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresSexMenu()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSexMenu.IntimacyRequest".Localized());

            ClearButtons();

            if (Save.GetInt("DoloresSex", 0) == 1)
                AddNextButton("Common.Button.MakeSure".Localized(), DoloresComfort);

            if (Save.GetInt("DoloresSex", 0) < 3)
            {
                AddNextButton("Common.Button.Deflower".Localized(), DoloresDefloration);

                AddNextButton("Common.Button.FirstTime".Localized(), DoloresFirstTime);
            }

            if (Save.GetInt("DoloresSex", 0) > 2)
            {
                AddNextButton("Common.Button.Sex".Localized(), DoloresSex);

                AddNextButton("Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), DoloresBlowjob);

                AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), DoloresTribbing);

                AddNextButton("Common.Button.Lappetting".Localized(), DoloresPetting);
            }

            AddButton(14, "Common.Button.Back".Localized(), DoloresMenu);
        }

        public void DoloresComfort()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresComfort.ReadinessConcern".Localized());

            OutputText("Scenes.Doloresscene.DoloresComfort.LongSpeechLean".Localized());

            OutputText("Scenes.Doloresscene.DoloresComfort.ComfortPledge".Localized());

            Save.SetInt("DoloresSex", 2);

            DoNext(() => DoloresSexMenu());
        }

        public void DoloresDefloration()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresDefloration.UntouchedPurity".Localized((Player.IsNaked() ? "" : "stripping your armor before")));

            OutputText("Scenes.Doloresscene.DoloresDefloration.LoveConfession".Localized());

            OutputText("Scenes.Doloresscene.DoloresDefloration.PurpleGarmentPull".Localized());

            OutputText("Scenes.Doloresscene.DoloresDefloration.StolenKiss".Localized((doloresComforted() ? "Dolores's doing the same" : "that Dolores's never closed")));

            OutputText("Scenes.Doloresscene.DoloresDefloration.LieDownOrder".Localized((doloresComforted() ? "She gives you a small, reassuring nod" : "There is none")));

            OutputText("Scenes.Doloresscene.DoloresDefloration.ExposedSlit".Localized());

            OutputText("Scenes.Doloresscene.DoloresDefloration.ImpatientWait".Localized((Player.LongestCockLength() > 12 ? "your size" : "her inexperience")));

            OutputText("Scenes.Doloresscene.DoloresDefloration.TwitchingPressure".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), DoloresDeflorationYes);

            AddButton(1, "Common.Button.No".Localized(), DoloresDeflorationNo);
        }

        public void DoloresDeflorationYes()
        {
            ClearOutput();

            if (Save.GetInt("DoloresSex", 0) == 1)
                Save.SetInt("DoloresSex", 3);
            else
                Save.SetInt("DoloresSex", 4);

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.FranticPlunge".Localized((doloresComforted() ? "slightly" : ""), (Player.Corruption < 50 ? "apologize and" : ""), (doloresComforted() ? " yet" : "")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.WipedTear".Localized());

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.MeasuredRhythm".Localized((doloresComforted() ? "finally sigh with pleasure" : "no longer wince")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.CloserPull".Localized((doloresComforted() ? "As you continue your carnal embrace, Dolores's four arms tentatively wrap around your back to return your affection. " : "")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.TorsoRaisedGaze".Localized((Player.LongestCockLength() > 10 ? "although you aren't quite able to meet them" : "sealing her tight against you")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.ReleasingGrip".Localized((Player.Tallness > 60 ? "a bit small for you, but you manage" : "just the right size for you, so you easily manage")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.SquirmingAnswer".Localized((doloresComforted() ? "I... am not sure" : "I... don't believe so"), (doloresComforted() ? "I... enjoyed it too, but... that was enough for me, for now..." : "You... I appreciate it, [Father], but you need do no more.")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationYes.SlowRecovery".Localized());

            if (!DoloresComforted())
                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresDeflorationNo()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresDeflorationNo.SecondThoughts".Localized((!Player.IsNaked() ? "pull your [armor] back on" : "calm yourself down")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationNo.RelievedNod".Localized((doloresComforted() ? "We can... try again another time." : "")));

            OutputText("Scenes.Doloresscene.DoloresDeflorationNo.ReflectiveWalk".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresFirstTime.YouthfulDesire".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime.ApproachReaction".Localized());

            if (Save.GetInt("DoloresSex", 0) == 2)
                OutputText("Scenes.Doloresscene.DoloresFirstTime.FlusteredBacking".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresFirstTime.UnexplainedTinge".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime.LoomingLean".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime.ReluctantKiss".Localized((doloresComforted() ? "[say: How do you, er, we... You don't have--] You cut her off with a passionate kiss, causing her to gasp into your mouth in surprise." : "Your daughter doesn't seem interested in moving herself, or even talking, so you draw her into a passionate kiss, which she meekly accepts.")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime.SummerBreezeScent".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime.ClingingDress".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime.GlisteningLips".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), DoloresFirstTime2);

            AddButton(1, "Common.Button.No".Localized(), DoloresDeflorationNo);
        }

        public void DoloresFirstTime2()
        {
            ClearOutput();

            if (Save.GetInt("DoloresSex", 0) == 1)
                Save.SetInt("DoloresSex", 3);
            else
                Save.SetInt("DoloresSex", 4);

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.TonguePlunge".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.TotalImmersion".Localized((doloresComforted() ? " yet" : "")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.NoticeTear".Localized());

            if (DoloresComforted())
                OutputText("Scenes.Doloresscene.DoloresFirstTime2.FeebleAssurance".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresFirstTime2.DocileDoll".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.MountingNeed".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.InexperiencedTouch".Localized((doloresComforted() ? "titters nervously" : "flinches")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.UnexpectedTension".Localized((doloresComforted() ? "measure of excitement, of wonder at this whole new world you're showing her" : "sort of hollowness, as if she's receded back into herself")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.SecondAttempt".Localized((doloresComforted() ? "confirming that it's okay to continue" : "reassuming your position")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.CompensatingKiss".Localized((doloresComforted() ? "One of your daughter's free hands tentatively reaches for you, but falters short of its destination." : "Your daughters hands lie listless at her side, and it seems she has no desire to touch you."), (doloresComforted() ? "embarrassedly interested in " + (Player.BiggestBreastSize() > 1 ? "your more mature assets" : "a chest so like her own") : "somewhat uncomfortable with this, but no complaints pass her lips")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.OverwhelmingStimulation".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.ShudderingClimax".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.SharedRest".Localized());

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.WordlessComfort".Localized((doloresComforted() ? "[say: I, um... I never thought that two... That was... I simply don't know what to say.]" : "She looks at you, her eyes wet, but can't seem to get anything out past a single [say: I...]")));

            OutputText("Scenes.Doloresscene.DoloresFirstTime2.LeavingWarmth".Localized((doloresComforted() ? "shaky" : "faltering")));

            if (!DoloresComforted())
                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresSex()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresSex.PullIntoKiss".Localized());

            OutputText("Scenes.Doloresscene.DoloresSex.LeanInAndKiss".Localized((doloresComforted() ? "blushes" : "doesn't react"), (doloresComforted() ? "" : " either")));

            OutputText("Scenes.Doloresscene.DoloresSex.MumbledPraise".Localized(
                Player.Mf("F-Father", "M-Mother")
            ));

            ClearButtons();

            AddNextButton("Common.Button.Undress".Localized(), () => DoloresSex2(false));

            AddNextButton("Scenes.Options.Start".Localized(), () => DoloresSex2(true));
        }

        public void DoloresSex2(bool dressed)
        {
            ClearOutput();

            if (dressed)
                OutputText("Scenes.Doloresscene.DoloresSex2.DemureDress".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresSex2.InstructStripComply".Localized((doloresComforted() ? "ask" : "instruct")));

            OutputText("Scenes.Doloresscene.DoloresSex2.PressAgainstWall".Localized((doloresComforted() ? "You ask her if she's ready for this" : "You tell her that you're about to start")));

            OutputText("Scenes.Doloresscene.DoloresSex2.SheSaysRemark".Localized((doloresComforted() ? "I-I'm alright" : "Alright"), (doloresComforted() ? "a nervous flutter in her voice" : "a dead note in her voice")));

            OutputText("Scenes.Doloresscene.DoloresSex2.GrabHipsTurnAway".Localized((doloresComforted() ? "evidently too shy to look at you during" : "though her stoic face betrays no emotion")));

            OutputText("Scenes.Doloresscene.DoloresSex2.FinalPreparation".Localized((dressed ? "pull her dress up over her hips and" : ""), (!Player.IsNaked() ? "yourself undress before pulling" : "pull")));

            OutputText("Scenes.Doloresscene.DoloresSex2.BraceAgainstWall".Localized());

            OutputText("Scenes.Doloresscene.DoloresSex2.SlowEntryMassage".Localized(
                Player.BiggestCockLength() > 5 ? "You're still " + (Player.BiggestCockLength() > 10 ? "too" : "a bit") + " big for her, causing her to squirm as you stretch her walls" : "She's evidently still not used to this sensation, causing her to squirm as you slip inside"
            ));

            OutputText("Scenes.Doloresscene.DoloresSex2.TakeInitiative".Localized((dressed ? "their delicious contour" : "her smooth, unblemished skin"), (doloresComforted() ? "even leaning back into you a bit as soft gasps escape her lips" : "keeping a steady posture with only a slight whimper")));

            OutputText("Scenes.Doloresscene.DoloresSex2.PickUpPace".Localized((doloresComforted() ? "moan" : "grunt"), (dressed ? "dress to scrunch up and scrape against the wall, and you give a passing thought to potential damage" : "bare nipples to scrape against the wall, eliciting a low whine from her")));

            OutputText("Scenes.Doloresscene.DoloresSex2.EmbraceAtClimax".Localized((Player.Tallness > 50 ? "above" : "behind")));

            OutputText("Scenes.Doloresscene.DoloresSex2.SensesOverwhelmed".Localized((doloresComforted() ? "Dolores seems similarly close" : "It doesn't seem like Dolores can take much more either")));

            OutputText("Scenes.Doloresscene.DoloresSex2.DeepFillSeal".Localized((doloresComforted() ? "pleasure" : "discomfort")));

            OutputText("Scenes.Doloresscene.DoloresSex2.LeadToBed".Localized((doloresComforted() ? "against you" : "over on her side")));

            if (DoloresComforted())
                OutputText("Scenes.Doloresscene.DoloresSex2.WorriedTrance".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresSex2.HandOnHip".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresBlowjob()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresBlowjob.RequestPreamble".Localized((!doloresBlowjobFlag ? "You aren't entirely sure how she'll respond to it, as she's normally a fairly passive partner, so y" : "Y"), (DoloresComforted() ? "comfortable with giving" : "willing to give")));

            if (!Save.GetBool("DoloresBlowjob"))
                OutputText("Scenes.Doloresscene.DoloresBlowjob.BlushAndHesitate".Localized());
            else
                if (DoloresComforted())
                    OutputText("Scenes.Doloresscene.DoloresBlowjob.IncompleteThought".Localized());
                else
                    OutputText("Scenes.Doloresscene.DoloresBlowjob.DeadLookCompliance".Localized());

            OutputText("Scenes.Doloresscene.DoloresBlowjob.LeadToChair".Localized((!Player.IsNaked() ? " and beginning to strip. As your [armor] slowly comes off," : "."), (!Player.IsNaked() ? "Now ready for her affection, y" : "Y")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.KneelingPrelude".Localized((doloresComforted() ? "scrutinizing it with some uncertainty" : "although her glazed eyes suggest she's somewhere else right now"), (Player.Corruption > 50 ? "you'd like nothing more than to grab her head and slam it down to your hips" : "the sensation is nearly unbearable")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.HandsOnBase".Localized((doloresComforted() ? "reassure her" : "direct her to continue")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.TongueTipFlick".Localized());

            OutputText("Scenes.Doloresscene.DoloresBlowjob.WetPopRelease".Localized((Player.LongestCockLength() > 4 ? "Her young mouth is too small to fit you fully, but she tries her best, diligently drawing in the first few inches." : "Her young mouth easily envelopes the " + (Player.LongestCockLength() > 2 ? "first few inches" : "entirety") + " of your small cock."), (doloresComforted() ? "flustered smile" : "blank stare")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.InexperiencedInsight".Localized((doloresComforted() ? "lovingly" : "mechanically")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.EagerWillingness".Localized((doloresComforted() ? "It seems that all of her earlier bashfulness has been lost in the heat of the moment" : "Despite her apparent ardor, you notice a notable lack of emotion on her face")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.TongueOnGlans".Localized((!doloresBlowjobFlag ? "It seems she's learning as she goes." : "")));

            OutputText("Scenes.Doloresscene.DoloresBlowjob.StaveOffClimax".Localized());

            OutputText("Scenes.Doloresscene.DoloresBlowjob.TipThenSpit".Localized((Player.CumQ() > 500 ? "deluge" : "drops")));

            if (DoloresComforted())
                OutputText("Scenes.Doloresscene.DoloresBlowjob.ConflictedSwallow".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresBlowjob.UnawareStroking".Localized());

            OutputText("Scenes.Doloresscene.DoloresBlowjob.StandsConfused".Localized());

            OutputText("Scenes.Doloresscene.DoloresBlowjob.ThankAndLeave".Localized());

            Player.Orgasm("Dick");

            Save.SetBool("DoloresBlowjob", true);

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresTribbing()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTribbing.RushedEmbrace".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing.RemainingBarrier".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing.FlusteredUndress".Localized((doloresComforted() ? "she takes a moment to ready herself, clenching her fists with determination" : "her eyes suddenly deaden, and she turns to you, though it doesn't really seem like she's looking at you so much as through you")));

            OutputText("Scenes.Doloresscene.DoloresTribbing.UndressBranch".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing.WeighingOptions".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing.QuickPositioning".Localized((doloresComforted() ? "in anticipation" : "with trepidation")));

            OutputText("Scenes.Doloresscene.DoloresTribbing.PreliminaryWarmup".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing.BreastExploration".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing.ShoulderMoan".Localized());

            DoNext(() => DoloresTribbing2());
        }

        public void DoloresTribbing2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresTribbing2.EagerGrinding".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing2.LosingToPleasure".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing2.PullHerFlat".Localized((doloresComforted() ? "embarrassment" : "reluctance")));

            OutputText("Scenes.Doloresscene.DoloresTribbing2.DrapedTogether".Localized());

            if (DoloresComforted())
                OutputText("Scenes.Doloresscene.DoloresTribbing2.SuddenSlump".Localized());
            else
                OutputText("Scenes.Doloresscene.DoloresTribbing2.ClingingResist".Localized());

            OutputText("Scenes.Doloresscene.DoloresTribbing2.SlowUntangling".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void DoloresPetting()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresPetting.CallToChair".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.NervousStanding".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.SettlesInLap".Localized((doloresComforted() ? "direction" : "command"), (!Player.IsGenderless() ? "r waiting " + (Player.HasCock() ? "cock" : "cunt") + ", which throbs with need " + (!Player.IsNaked() ? "beneath your [armor]" : "against the fabric of her dress") + ". But this isn't about that, you're here for [i: her] pleasure" : "immeasurably")));

            OutputText("Scenes.Doloresscene.DoloresPetting.EmbarrassedSilence".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.CarefulCaresses".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.RadiatingAche".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.ThighToKnee".Localized((doloresComforted() ? "eliciting the faintest moan of need from Dolores" : "although Dolores doesn't seem to mind")));

            OutputText("Scenes.Doloresscene.DoloresPetting.TracingRibs".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.GaspingFourHands".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting.CollarboneToChest".Localized());

            DoNext(() => DoloresPetting2());
        }

        public void DoloresPetting2()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresPetting2.DownwardReturn".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting2.BudConvergence".Localized((Player.HasClaws() ? "claw" : "finger")));

            OutputText("Scenes.Doloresscene.DoloresPetting2.SurprisedGasp".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting2.BreathyLapping".Localized((doloresComforted() ? "wet enough to proceed" : "to be somewhat frigid, but you know just how to warm her up")));

            OutputText("Scenes.Doloresscene.DoloresPetting2.DigitsBottomOut".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting2.SpreadHerLegs".Localized((doloresComforted() ? "slams her legs together with a squeak" : "nervously starts to squeeze her legs together with a whimper"), (doloresComforted() ? "wail" : "whimper")));

            OutputText("Scenes.Doloresscene.DoloresPetting2.PalmAgainstClit".Localized((doloresComforted() ? "obviously" : "uncomfortably")));

            OutputText("Scenes.Doloresscene.DoloresPetting2.SuddenClimax".Localized((doloresComforted() ? "letting out quiet cries of pleasure" : "but keeps almost completely silent")));

            DoNext(() => DoloresPetting3());
        }

        public void DoloresPetting3()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.DoloresPetting3.HeavingBreaths".Localized((doloresComforted() ? "through her stupor" : "somewhat tentatively"), (doloresComforted() ? "languidly stretches" : "silently rises")));

            if (DoloresComforted())
                OutputText("Scenes.Doloresscene.DoloresPetting3.SheepishGrin".Localized((silly ? "You didn't think you were [i: that] good. " : "")));
            else
                OutputText("Scenes.Doloresscene.DoloresPetting3.CrypticGlance".Localized());

            OutputText("Scenes.Doloresscene.DoloresPetting3.PartingKiss".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveBook()
        {
            ClearOutput();

            OutputText("Scenes.Doloresscene.GiveBook.ConfiscationRecall".Localized());

            OutputText("Scenes.Doloresscene.GiveBook.FourArmSnatch".Localized());

            OutputText("Scenes.Doloresscene.GiveBook.RegatheredPoise".Localized((doloresProg < 9 ? "caterpillar" : "moth")));

            OutputText("Scenes.Doloresscene.GiveBook.LingeringThought".Localized());

            OutputText("Scenes.Doloresscene.GiveBook.JoyfulOutburst".Localized());

            OutputText("Scenes.Doloresscene.GiveBook.IndulgentSmile".Localized());

            Save.SetInt("doloresDecision", 1);

            for (var i = 0; i < Buttons.Length; i++)
                if (Buttons[i] != null && Buttons[i].Visible && Buttons[i].Label.RichTextLayout.Text == "Give Book")
                    RemoveButton(i);

        }

        public void GiveManorBooks()
        {
            ClearOutput();

            RegisterMacro("child", doloresProg < 9);

            OutputText("Scenes.Doloresscene.GiveManorBooks.ObviousGift".Localized());

            OutputText("Scenes.Doloresscene.GiveManorBooks.DriftingQuestion".Localized());

            OutputText("Scenes.Doloresscene.GiveManorBooks.TravelExplanation".Localized());

            OutputText("Scenes.Doloresscene.GiveManorBooks.MumbledThanks".Localized((hikkiDone() ? " [say:It's truly... I do not know what I've done to deserve all this.] Whatever else you could say to that dissolves when you see a smile of pure bliss on her face. It suits her well." : "")));

            OutputText("Scenes.Doloresscene.GiveManorBooks.LeavingHerToRead".Localized());

            Save.SetBool("doloresBooks", true);

            Player.RemoveKeyItem("Old Manor Books");

            DoNext(() => ReturnToCamp(1));
        }

    }
}