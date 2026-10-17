using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<MarielleScene>("coc:marielle")]
    public class MarielleScene : Scene
    {

        public const int TALK_COLLAPSE = 1 << 0;

        public const int TALK_UNDEATH = 1 << 1;

        public const int TALK_DRESSMAKING = 1 << 2;

        public const int TALK_CART = 1 << 3;

        public const int TALK_OLD_WORLD = 1 << 4;

        public const int TALK_HER_LIKES = 1 << 5;

        public const int TALK_FAMILY = 1 << 6;

        public const int TALK_LONELINESS = 1 << 7;

        public const int TALK_SEX = 1 << 8;

        public const int TALK_COMMANDO = 1 << 9;

        public const int TALK_FATHER = 1 << 10;

        public const int TALK_BACKGROUND = 1 << 11;

        public const int TALK_FONDNESS = 1 << 12;

        public const int TALK_HER_DEATH = 1 << 13;

        public const int TALK_HIS_DEATH = 1 << 14;

        public const int TALK_NECROMANCY = 1 << 15;

        public const int TALK_SEX_2 = 1 << 16;

        public const int TALK_HER_DEATH_2 = 1 << 17;

        public const int BOUGHT_LONG_DRESS = 1 << 0;

        public const int BOUGHT_CHEER_OUTFIT = 1 << 1;

        public const int BOUGHT_BLACK_CLOAK = 1 << 2;

        public const int MODELED_LONG_DRESS = 1 << 0;

        public const int MODELED_MAID_DRESS = 1 << 1;

        public const int MODELED_BUTLER_SUIT = 1 << 2;

        public const int MODELED_KIMONO = 1 << 3;

        public const int MODELED_CHINA_DRESS = 1 << 4;

        public const int MODELED_TURTLENECK = 1 << 5;

        public const int MODELED_BALLET_DRESS = 1 << 6;

        public const int COM_MOTH_DRESS = 1;

        public const int COM_SYLVIA_DRESS = 2;

        public const int COM_MOTH_PANTIES = 4;

        public const int COM_MOTH_BEDDING = 8;

        public const int COM_SS_DRESS = 3;

        public const int COM_SS_BRA = 5;

        public const int COM_SS_PANTIES = 6;

        public const int COM_SS_UNDERWEAR = 7;

        public const int COM_SS_BEDDING = 9;

        public int currentPrice;

        public int State => Save.GetInt("state", 0);

        public bool HasNecromancyTalk => (Save.GetInt("talks", 0) & TALK_NECROMANCY) != 0;

        public List<int> bathOptions = [];

        public static double Round(double value) => Math.Round(value);

        public static SaveContent Save => SaveContent(SceneIds.Bog.Marielle);

        public string debugName => "Marielle";

        public string debugHint => "";

        public override void Entry()
        {
            MarielleAvailable();
        }

        public void Reset()
        {

            Save.SetInt("state", 0);

            Save.SetInt("extorted", 0);

            Save.SetInt("insulted", 0);

            Save.SetInt("rose", 0);

            Save.SetInt("fainted", 0);

            Save.SetInt("kissDenied", 0);

            Save.SetInt("taurDecision", 0);

            Save.SetInt("handHeld", 0);

            Save.SetInt("talks", 0);

            Save.SetInt("itemsBought", 0);

            Save.SetInt("modeled", 0);

            Save.SetInt("moneySpent", 0);

            Save.SetInt("commissionCount", 0);

            Save.SetInt("timesKissed", 0);

            Save.SetInt("timesPatted", 0);

            Save.SetInt("timesHandHeld", 0);

            Save.SetInt("timesSexed", 0);

            Save.SetInt("timesYurid", 0);

            Save.SetInt("timesDicked", 0);

            Save.SetInt("timesInButt", 0);

            Save.SetFloat("visitTime", 0);

            Save.SetInt("openDate", 0);

            Save.SetInt("reenabled", 0);

            Save.SetFloat("commissionTime", 0);

            Save.SetBool("seenNaked", false);

            Save.SetBool("askedModeling", false);

            Save.SetBool("mockedDress", false);

            Save.SetBool("askedCommission", false);

            Save.SetBool("footstuff", false);

            Save.SetBool("bathed", false);

            Save.SetBool("breastfed", false);

            Save.SetBool("seenMothSilk", false);

            Save.SetBool("seenSpiderSilk", false);

            Save.SetInt("commission", 0);

            Save.SetInt("lastBoot", 0);

            Save.SetInt("lastSleep", 0);
        }

        public void SetVisitTime(float value) => Save.SetFloat("visitTime", value);

        public bool MarielleAvailable(bool checkAwake = false, bool checkCommission = true)
        {

            return Save.GetInt("state", 0) > 0 &&

                (!checkCommission || Save.GetFloat("commissionTime") <= View.Time.TotalTime) &&

                (View.Time.Days >= Save.GetInt("reenabled", 0)) &&

                (Save.GetInt("insulted", 0) == 0 || Save.GetInt("insulted", 0) == 2) &&

                (!checkAwake || View.Time.IsTimeBetween(6.5f, 22.5f));
        }

        public bool VaginalAvailable()
        {

            return Save.GetInt("insulted", 0) == 0 && !Save.GetBool("mockedDress") && Save.GetInt("timesDicked", 0) < 5;
        }

        public void MarielleSex(bool dicked = false, bool butt = false)
        {

            Save.SetInt("timesSexed", Save.GetInt("timesSexed", 0) + 1);

            if (Player.HasVagina() && !Player.HasCock())
                Save.SetInt("timesYurid", Save.GetInt("timesYurid", 0) + 1);

            if (dicked)
                Save.SetInt("timesDicked", Save.GetInt("timesDicked", 0) + 1);

            if (butt)
                Save.SetInt("timesInButt", Save.GetInt("timesInButt", 0) + 1);
        }

        public void RegisterMacros()
        {

            RegisterMacro("insultedfather", Save.GetInt("insulted", 0) > 0);

            RegisterMacro("metdolores", (0 & 0) > 1);
        }

        public void Meet()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Meet.HeadSwimmingQuagmire".Localized());

            OutputText("Scenes.Marielle.Meet.CloseToOldTemple".Localized());

            OutputText("Scenes.Marielle.Meet.CawFromOverhead".Localized());

            ClearButtons();

            AddNextButton("Combat.Button.Approach".Localized(), MeetApproach);

            AddNextButton("Common.Button.Leave".Localized(), MeetLeave);
        }

        public void MeetLeave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetLeave.WontGetInvolved".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MeetApproach()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetApproach.ApproachBetterLook".Localized());

            OutputText("Scenes.Marielle.MeetApproach.CursoryGlanceReveals".Localized());

            OutputText("Scenes.Marielle.MeetApproach.CarryHerSplayed".Localized());

            OutputText("Scenes.Marielle.MeetApproach.UrghGroan".Localized());

            OutputText("Scenes.Marielle.MeetApproach.MindPullsStop".Localized());

            OutputText("Scenes.Marielle.MeetApproach.ShrugStartDrag".Localized());

            OutputText("Scenes.Marielle.MeetApproach.AhhConfused".Localized());

            OutputText("Scenes.Marielle.MeetApproach.CorpseHeadLifts".Localized());

            ClearButtons();

            AddNextButton("Common.Button.HoldHer".Localized(), MeetHold);

            AddNextButton("Common.Button.DropHer".Localized(), MeetDrop);
        }

        public void MeetHold()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetHold.GirlNeedsHelp".Localized());

            OutputText("Scenes.Marielle.MeetHold.DeliriousGroan".Localized());

            OutputText("Scenes.Marielle.MeetHold.IThinkNot".Localized());

            OutputText("Scenes.Marielle.MeetHold.DoesntWeighMuch".Localized());

            MeetContinue();
        }

        public void MeetDrop()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetDrop.DropBodyYelp".Localized());

            OutputText("Scenes.Marielle.MeetDrop.OwwHoldStranger".Localized());

            OutputText("Scenes.Marielle.MeetDrop.CorpseGirlZombie".Localized());

            OutputText("Scenes.Marielle.MeetDrop.WantsBringInside".Localized());

            OutputText("Scenes.Marielle.MeetDrop.DoesntKnowAlive".Localized());

            OutputText("Scenes.Marielle.MeetDrop.FrowningLots".Localized());

            ClearButtons();

            AddNextButton("Common.Button.HelpHer".Localized(), MeetHelp);

            AddNextButton("Common.Button.AbandonHer".Localized(), MeetAbandon);

            AddNextButton("Common.Button.EndHer".Localized(), MeetEnd);
        }

        public void MeetHelp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetHelp.DecideToHelp".Localized());

            MeetContinue();
        }

        public void MeetEnd()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetEnd.PlaceholderPeriod".Localized((Player.HasPerk(PerkIds.HistoryDEUSVULT) ? "You're certain: she must be undead. This unholy abomination has no place here, and you will end her swiftly and right[if (!silly) {eous}]ly" : "[if (corruption < 50) {You decide to put an end to her life. It could be considered a mercy killing, seeing as she's Clearly suffering in her state|Whatever she is, she's better off dead. And better yet: ended by your own hands}]")));

            OutputText("Scenes.Marielle.MeetEnd.WalkUpDelirious".Localized());

            OutputText("Scenes.Marielle.MeetEnd.SnapHerNeck".Localized());

            OutputText("Scenes.Marielle.MeetEnd.DragGurgle".Localized());

            OutputText("Scenes.Marielle.MeetEnd.RaiseFootAim".Localized());

            OutputText("Scenes.Marielle.MeetEnd.StompCrushSkull".Localized());

            OutputText("Scenes.Marielle.MeetEnd.SilenceChestStill".Localized());

            OutputText("Scenes.Marielle.MeetEnd.LiftOntoCart".Localized());

            OutputText("Scenes.Marielle.MeetEnd.SearchBelongings".Localized());

            Save.SetInt("state", -1);

            Player.Gems += 17;

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.FineLongsword), 1);

            ReturnToCamp(1);
        }

        public void MeetAbandon()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetAbandon.NothingWithCreature".Localized());

            OutputText("Scenes.Marielle.MeetAbandon.CroakBegThee".Localized());

            ClearButtons();

            AddNextButton("Scenes.Faerie.GladesScene.ButtonHelp".Localized(), MeetAbandonHelp);

            AddNextButton("Common.Button.Leave".Localized(), MeetAbandonLeave);
        }

        public void MeetAbandonHelp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetAbandonHelp.ChangeMindBetter".Localized());

            MeetContinue();
        }

        public void MeetAbandonLeave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetAbandonLeave.SpareNoEar".Localized());

            Save.SetInt("state", -2);

            DoNext(() => ReturnToCamp(1));
        }

        public void MeetContinue()
        {

            OutputText("Scenes.Marielle.MeetContinue.CollapsesOnRelease".Localized());

            OutputText("Scenes.Marielle.MeetContinue.ResurfacesGasping".Localized());

            OutputText("Scenes.Marielle.MeetContinue.PantingCloserLook".Localized());

            OutputText("Scenes.Marielle.MeetContinue.GazePastelDress".Localized());

            OutputText("Scenes.Marielle.MeetContinue.RaspsInclinesHead".Localized());

            OutputText("Scenes.Marielle.MeetContinue.TurnsAwayMumbles".Localized());

            OutputText("Scenes.Marielle.MeetContinue.QuietMumbling".Localized());

            OutputText("Scenes.Marielle.MeetContinue.StrangerQuestion".Localized());

            OutputText("Scenes.Marielle.MeetContinue.LookUpOneEye".Localized());

            OutputText("Scenes.Marielle.MeetContinue.ClamsUpCovers".Localized());

            OutputText("Scenes.Marielle.MeetContinue.PritheeReturnIt".Localized());

            ClearButtons();

            AddNextButton("Common.Button.GiveBack".Localized(), MeetGiveBack);

            AddNextButton("Common.Button.Hesitate".Localized(), MeetHesitate);
        }

        public void MeetHesitate()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetHesitate.FaceFallsFurther".Localized());

            OutputText("Scenes.Marielle.MeetHesitate.MumblesGlancingEye".Localized());

            OutputText("Scenes.Marielle.MeetHesitate.BeginsPausesAgain".Localized());

            OutputText("Scenes.Marielle.MeetHesitate.TrueEyeUndead".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Return".Localized(), MeetReturn);

            AddNextButton("Common.Button.Refuse".Localized(), MeetRefuse);

            AddNextButton("Common.Button.Pounce".Localized(), () => MeetPounce(false));
        }

        public void MeetPounce(bool eyeball)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetPounce.ImmediatelyLunge".Localized((Player.HasPerk(PerkIds.HistoryDEUSVULT) ? "You knew it, you knew she was some vile, undead monstrosity! " : ""), (eyeball ? "eye snaps" : "eyes snap")));

            OutputText("Scenes.Marielle.MeetPounce.BladeDrawnWhirls".Localized());

            OutputText("Scenes.Marielle.MeetPounce.UnhandMeStruggle".Localized());

            OutputText("Scenes.Marielle.MeetPounce.GrabHerThroat".Localized());

            OutputText("Scenes.Marielle.MeetPounce.PleadingCutShort".Localized((eyeball ? "remaining eye" : "eyes")));

            OutputText("Scenes.Marielle.MeetPounce.NotDyingResilient".Localized());

            OutputText("Scenes.Marielle.MeetPounce.GaspsBladeOut".Localized((eyeball ? "eye" : "eyes")));

            OutputText("Scenes.Marielle.MeetPounce.NotEntirelyDecapitate".Localized());

            if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                OutputText("Scenes.Marielle.MeetPounce.GhostlyCompanionAsks".Localized());

            OutputText("Scenes.Marielle.MeetPounce.DragBodyToCart".Localized());

            OutputText("Scenes.Marielle.MeetPounce.WeighNiceLongsword".Localized());

            if (eyeball)
                OutputText("Scenes.Marielle.MeetPounce.RememberDroppedEye".Localized());

            OutputText("Scenes.Marielle.MeetPounce.ThatsAllItSeems".Localized());

            Player.Gems += 17;

            Player.ApplyStats(new DynStat(GameStats.Corruption, Player.HasPerk(PerkIds.HistoryDEUSVULT) ? -1 : 8));

            Save.SetInt("state", -1);

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.FineLongsword), 1);

            ReturnToCamp(1);
        }

        public void MeetRefuse()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetRefuse.RefuseGiveBack".Localized());

            OutputText("Scenes.Marielle.MeetRefuse.NoticeGripOnHer".Localized());

            OutputText("Scenes.Marielle.MeetRefuse.PausesDesperation".Localized());

            OutputText("Scenes.Marielle.MeetRefuse.BrigandCoinSeek".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Gems".Localized(), MeetGems);

            AddNextButton("Common.Button.Sword".Localized(), MeetSword);

            AddNextButton("Common.Button.Sex".Localized(), MeetSex);

            AddNextButton("Common.Button.Nothing".Localized(), MeetEyeball);
        }

        public void MeetGems()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetGems.GuessedRightGems".Localized());

            OutputText("Scenes.Marielle.MeetGems.ResentfulWordsPushes".Localized());

            OutputText("Scenes.Marielle.MeetGems.FollowCloselyBehind".Localized());

            OutputText("Scenes.Marielle.MeetGems.LocatesPlateCart".Localized());

            OutputText("Scenes.Marielle.MeetGems.BeshrewBanditry".Localized());

            OutputText("Scenes.Marielle.MeetGems.FairlyHeavyGems".Localized());

            OutputText("Scenes.Marielle.MeetGems.TrudgeBackDoubt".Localized());

            Player.Gems += 613;

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            Save.SetInt("state", -1);

            Save.SetInt("extorted", 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void MeetSword()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetSword.TakesHerSword".Localized());

            OutputText("Scenes.Marielle.MeetSword.IncredulousPanic".Localized());

            OutputText("Scenes.Marielle.MeetSword.TriesTrailsOff".Localized());

            OutputText("Scenes.Marielle.MeetSword.NiceEngravedLongsword".Localized());

            OutputText("Scenes.Marielle.MeetSword.ExpressionUnchanged".Localized());

            OutputText("Scenes.Marielle.MeetSword.SpitsCurseBanditry".Localized());

            OutputText("Scenes.Marielle.MeetSword.NicestSendOff".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 3.5f));

            Save.SetInt("state", -1);

            Save.SetInt("extorted", 2);

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.FineLongsword), 1);

            ReturnToCamp(1);
        }

        public void MeetSex()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetSex.ClearWhatYouWant".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.MeetSex.SheFreezesRealization".Localized());

                OutputText("Scenes.Marielle.MeetSex.CriesNoWait".Localized());

                OutputText("Scenes.Marielle.MeetSex.NoPantiesPretty".Localized());

                OutputText("Scenes.Marielle.MeetSex.WincesFinalPlease".Localized());

                OutputText("Scenes.Marielle.MeetSex.PumpsRecklessAbandon".Localized());

                OutputText("Scenes.Marielle.MeetSex.HeadTurnedAway".Localized());

                OutputText("Scenes.Marielle.MeetSex.WontHoldLong".Localized());

                ClearButtons();

                AddNextButton("Common.Button.CumInside".Localized(), MeetSexInside);

                AddNextButton("Common.Button.CumInEye".Localized(), MeetSexEye);
            }
            else
            {
                OutputText("Scenes.Marielle.MeetSex.MeaningOfThis".Localized());

                OutputText("Scenes.Marielle.MeetSex.AvoidsEyesAsks".Localized());

                OutputText("Scenes.Marielle.MeetSex.SaveFunForLater".Localized());

                OutputText("Scenes.Marielle.MeetSex.ColdTongueSmellRot".Localized());

                OutputText("Scenes.Marielle.MeetSex.BumpScabbard".Localized());

                OutputText("Scenes.Marielle.MeetSex.WritheHipsAway".Localized());

                OutputText("Scenes.Marielle.MeetSex.ShudderUpSpine".Localized());

                OutputText("Scenes.Marielle.MeetSex.PleaseNoMore".Localized());

                DoNext(() => MeetSexContinued());
            }
        }

        public void MeetSexInside()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetSexInside.ZombieGetPregnant".Localized());

            OutputText("Scenes.Marielle.MeetSexInside.HiltDeepOrgasm".Localized());

            OutputText("Scenes.Marielle.MeetSexInside.MomentsLockedTogether".Localized());

            MeetSexEnd();
        }

        public void MeetSexEye()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetSexEye.DeviousIdeaSprings".Localized());

            OutputText("Scenes.Marielle.MeetSexEye.FinalMercilessPumps".Localized());

            OutputText("Scenes.Marielle.MeetSexEye.YelpsFirstSpurt".Localized());

            OutputText("Scenes.Marielle.MeetSexEye.WipeDickHerHair".Localized());

            MeetSexEnd();
        }

        public void MeetSexContinued()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetSexContinued.OnlyJustBegun".Localized());

            OutputText("Scenes.Marielle.MeetSexContinued.RestCutsShort".Localized());

            OutputText("Scenes.Marielle.MeetSexContinued.ChokingTears".Localized());

            OutputText("Scenes.Marielle.MeetSexContinued.YankDownHerTop".Localized());

            OutputText("Scenes.Marielle.MeetSexContinued.PainedCriesProd".Localized());

            OutputText("Scenes.Marielle.MeetSexContinued.HeatWashesOver".Localized());

            OutputText("Scenes.Marielle.MeetSexContinued.HighDiesDown".Localized());

            MeetSexEnd();
        }

        public void MeetSexEnd()
        {

            OutputText("Scenes.Marielle.MeetSexEnd.RefreshedQuietOutside".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Save.SetInt("state", -1);

            Save.SetInt("extorted", 3);

            Player.Orgasm("Generic");

            MarielleSex(Player.HasCock());

            DoNext(() => ReturnToCamp(1));
        }

        public void MeetEyeball()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetEyeball.NothingButHerEye".Localized());

            OutputText("Scenes.Marielle.MeetEyeball.BewilderedPanics".Localized());

            OutputText("Scenes.Marielle.MeetEyeball.GiveTheeAllOwed".Localized());

            OutputText("Scenes.Marielle.MeetEyeball.UnintelligibleSobbing".Localized());

            OutputText("Scenes.Marielle.MeetEyeball.AstoundinglyQuiet".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 6f));

            Save.SetInt("state", -1);

            Save.SetInt("extorted", 4);

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Eyeball), 1);

            ReturnToCamp(1);
        }

        public void MeetReturn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetReturn.GotInfoGiveEye".Localized());

            OutputText("Scenes.Marielle.MeetReturn.TurnsBackRestored".Localized());

            OutputText("Scenes.Marielle.MeetReturn.FaintlyNodsNone".Localized());

            OutputText("Scenes.Marielle.MeetReturn.OddThingToThank".Localized());

            MeetMerge();
        }

        public void MeetGiveBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetGiveBack.HandItBack".Localized());

            OutputText("Scenes.Marielle.MeetGiveBack.PausesChoosingWords".Localized());

            ClearButtons();

            AddNextButton("Common.Button.StayPut".Localized(), MeetStayPut);

            AddNextButton("Common.Button.Pounce".Localized(), () => MeetPounce(true));
        }

        public void MeetStayPut()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MeetStayPut.FearfulReaction".Localized(((CoC.Instance.Flags.Get(GameFlagIds.DullahanMet) > 0 || CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) > 0) ? "It's not your first time meeting someone undead, and here, it certainly explains" : "Undead, she said. She certainly fits the bill, and it would explain")));

            OutputText("Scenes.Marielle.MeetStayPut.VoiceFailsAgain".Localized());

            MeetMerge();
        }

        public void MeetMerge()
        {

            OutputText("Scenes.Marielle.MeetMerge.DreadfulSay".Localized());

            OutputText("Scenes.Marielle.MeetMerge.NotDirectedAtYou".Localized());

            OutputText("Scenes.Marielle.MeetMerge.EyesWanderInterior".Localized());

            OutputText("Scenes.Marielle.MeetMerge.LongAndShortAnswer".Localized());

            OutputText("Scenes.Marielle.MeetMerge.ISeeFingertips".Localized());

            OutputText("Scenes.Marielle.MeetMerge.LongStillSilence".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Wait".Localized(), () => MeetMerge2(true));

            AddNextButton("Common.Button.Check".Localized(), () => MeetMerge2(false));
        }

        public void MeetMerge2(bool waited)
        {
            ClearOutput();

            if (waited)
            {
                OutputText("Scenes.Marielle.MeetMerge2.SnapsOutOfTrance".Localized());

                OutputText("Scenes.Marielle.MeetMerge2.FascinatingWobblesBy".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.MeetMerge2.FindHerUnresponsive".Localized());

                OutputText("Scenes.Marielle.MeetMerge2.MumblesWobblesPast".Localized());
            }

            OutputText("Scenes.Marielle.MeetMerge2.SteersBrokenWall".Localized());

            OutputText("Scenes.Marielle.MeetMerge2.ForgotYou".Localized());

            OutputText("Scenes.Marielle.MeetMerge2.AnyoneElseComeHere".Localized());

            OutputText("Scenes.Marielle.MeetMerge2.LeavesNotHealthy".Localized());

            OutputText("Scenes.Marielle.MeetMerge2.EyesClosedTilted".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Leave".Localized(), () => MeetMerge3(false));

            AddNextButton("Common.Button.Peek".Localized(), () => MeetMerge3(true));
        }

        public void MeetMerge3(bool peeked)
        {
            ClearOutput();

            if (peeked)
            {
                OutputText("Scenes.Marielle.MeetMerge3.OrdealExhausting".Localized());

                OutputText("Scenes.Marielle.MeetMerge3.LightBlueGown".Localized());

                OutputText("Scenes.Marielle.MeetMerge3.AlreadyEvidentFlat".Localized());

                OutputText("Scenes.Marielle.MeetMerge3.ThreadWoodDisc".Localized());

                OutputText("Scenes.Marielle.MeetMerge3.ControllingExhales".Localized());

                OutputText("Scenes.Marielle.MeetMerge3.MarethButPeaceful".Localized());

                if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                {
                    OutputText("Scenes.Marielle.MeetMerge3.WhatMightstDoing".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.BloodRunsCold".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.GuiltyAsCharged".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.WontWakeHer".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.HoversSleepingGirl".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.ShouldraUttersHuh".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.DoesntElaborate".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.MeetMerge3.NameStillUnknown".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.HandTwitchCorner".Localized());
                }

                OutputText("Scenes.Marielle.MeetMerge3.SawAllYouWanted".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 8f));

                Save.SetBool("seenNaked", true);
            }
            else

                if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                {
                    OutputText("Scenes.Marielle.MeetMerge3.SleepMolestationAsk".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.WasteOpportunity".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.StopsMereInches".Localized());

                    OutputText("Scenes.Marielle.MeetMerge3.NoMagicalInfluence".Localized());
                }
                else
                    OutputText("Scenes.Marielle.MeetMerge3.RegardSlumberingGirl".Localized()); // 原版 Marielle.hx:639：该分支的触发条件

            OutputText("Scenes.Marielle.MeetMerge3.VicinityCalmFrogs".Localized());

            OutputText("Scenes.Marielle.MeetMerge3.ThroatyCawWingspan".Localized());

            OutputText("Scenes.Marielle.MeetMerge3.ReturnTripQuiet".Localized());

            Save.SetInt("state", 1);

            Save.SetFloat("excludeExplore", View.Time.TotalTime + (30 - View.Time.Hours));

            DoNext(() => ReturnToCamp(1));
        }

        public void Open()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Open.TrudgeHumidBog".Localized());

            OutputText("Scenes.Marielle.Open.FamiliarFreshBreeze".Localized());

            OutputText("Scenes.Marielle.Open.HoleInWall".Localized());

            OutputText("Scenes.Marielle.Open.FacingInterior".Localized());

            OutputText("Scenes.Marielle.Open.ClearThroatAnnounce".Localized());

            OutputText("Scenes.Marielle.Open.WhaExclaim".Localized());

            OutputText("Scenes.Marielle.Open.JumpsInSeat".Localized());

            OutputText("Scenes.Marielle.Open.ScramblesGreetings".Localized());

            OutputText("Scenes.Marielle.Open.StillDontKnowName".Localized());

            if (Player.ShortName.ToLower() == "marielle")
            {
                OutputText("Scenes.Marielle.Open.YesQuestion".Localized());

                OutputText("Scenes.Marielle.Open.OnlyStaresLong".Localized());

                OutputText("Scenes.Marielle.Open.BothLightUp".Localized());

                OutputText("Scenes.Marielle.Open.DeepCurtsyName".Localized());

                OutputText("Scenes.Marielle.Open.TentativeSharedName".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.Open.ConfusedClarify".Localized());

                OutputText("Scenes.Marielle.Open.VeilFlowingHair".Localized());
            }

            OutputText("Scenes.Marielle.Open.MuchImprovedNow".Localized());

            OutputText("Scenes.Marielle.Open.FourArmsProof".Localized());

            OutputText("Scenes.Marielle.Open.QuartetOfLimbs".Localized());

            OutputText("Scenes.Marielle.Open.TwistsPullsArm".Localized());

            Save.SetInt("openDate", View.Time.Days);

            ClearButtons();

            AddNextButton("Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), () => Open2(true));

            AddNextButton("Common.Button.DontTake".Localized(), () => Open2(false));
        }

        public void Open2(bool taken)
        {
            ClearOutput();

            if (taken)
            {
                OutputText("Scenes.Marielle.Open2.TakeLimbWeigh".Localized());

                OutputText("Scenes.Marielle.Open2.HeavierThanLooks".Localized());

                OutputText("Scenes.Marielle.Open2.ExplainsDisjoint".Localized());
            }
            else
                OutputText("Scenes.Marielle.Open2.ExplainsNoTake".Localized());

            OutputText("Scenes.Marielle.Open2.RealignsStump".Localized());

            OutputText("Scenes.Marielle.Open2.FashionedVocation".Localized());
            // 原版 Marielle.hx:698 — Shouldra 是跟随者
            if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
            {
                OutputText("Scenes.Marielle.Open2.LastWordUncertain".Localized());

                OutputText("Scenes.Marielle.Open2.BlinksBreakStare".Localized());
            }

            OutputText("Scenes.Marielle.Open2.NodLetItRest".Localized());

            OutputText("Scenes.Marielle.Open2.SewingToolsCart".Localized());

            OutputText("Scenes.Marielle.Open2.RelishClothRaiments".Localized());

            OutputText("Scenes.Marielle.Open2.FineGarmentsTrade".Localized());

            OutputText("Scenes.Marielle.Open2.ShiftToTemple".Localized());

            OutputText("Scenes.Marielle.Open2.PushesUpGlasses".Localized());

            OutputText("Scenes.Marielle.Open2.WayfarerTraveller".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Embellish".Localized(), () => Open3(0));

            AddNextButton("Common.Button.Answer".Localized(), () => Open3(1));

            AddNextButton("Common.Button.BeVague".Localized(), () => Open3(2));
        }

        public void Open3(int answer)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Open3.YouTellHerShe".Localized(
                new[] { "You tell her she's indeed correct and launch into an elaborate tale of your adventures so far. You may be taking some liberties here and there, and a few things might not have happened quite as you describe them, but the girl still follows along with silent attention, her rapt eyes not realizing, or perhaps not minding, the sprinkled-in colors.",

            "You tell her she's correct and describe how you got here. You mention your hometown, Ingnam, how you were chosen as a hero to come to this land, the portal at your campsite, and some of your adventures so far.",

            "While you do tell her she's correct, you remain vague and leave out any details you don't want to share as you give her the abbreviated version of your journey so far." }[answer]
            ));

            OutputText("Scenes.Marielle.Open3.FinishStoryIsee".Localized((answer == 0 ? "Splendent hero I am none, but " : "")));

            OutputText("Scenes.Marielle.Open3.AsksWherePortal".Localized());

            OutputText("Scenes.Marielle.Open3.CeasesHairTwirling".Localized());

            OutputText("Scenes.Marielle.Open3.FewWintersAgone".Localized());

            OutputText("Scenes.Marielle.Open3.UnlikelyFindLocation".Localized());

            OutputText("Scenes.Marielle.Open3.MannerStillWeird".Localized((answer == 0 ? "Splendent hero I am none, but " : "")));

            OutputText("Scenes.Marielle.Open3.LooksUpStonework".Localized());

            OutputText("Scenes.Marielle.Open3.NeedVestureOffer".Localized());

            OutputText("Scenes.Marielle.Open3.FarewellLeaveWork".Localized());

            OutputText("Scenes.Marielle.Open3.UneventfulWayCamp".Localized());

            DoNext(() => ReturnToCamp(2));
        }

        public void Encounter()
        {

            RegisterMacros();

            if (View.Time.IsTimeBetween(22.5f, 6.5f))
                Sleeping();
            else
                if (Save.GetInt("insulted", 0) > 0 && Save.GetInt("insulted", 0) != 2)
                    Insulted();
                else
                    Intro();
        }

        public void Sleeping()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Sleeping.SneakCloserSleeping".Localized());

            OutputText("Scenes.Marielle.Sleeping.DressGlassesStool".Localized());

            OutputText("Scenes.Marielle.Sleeping.NotWokenYet".Localized());

            ClearButtons();

            AddNextButton("Common.Button.WakeHer".Localized(), () => Sleeping2(true));

            AddNextButton("Common.Button.WatchHer".Localized(), () => Sleeping2(false));

            AddButton(14, "Common.Button.Back".Localized(), () => Leave());
        }

        public void Sleeping2(bool woken)
        {
            ClearOutput();

            if (woken)
            {
                OutputText("Scenes.Marielle.Sleeping2.SlipCloserBedding".Localized());

                OutputText("Scenes.Marielle.Sleeping2.BeforeTouchBlanket".Localized());

                OutputText("Scenes.Marielle.Sleeping2.PalmsOutNonThreatening".Localized());

                OutputText("Scenes.Marielle.Sleeping2.DidntGoAsPlanned".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.Sleeping2.WatchGirlSleep".Localized());

                OutputText("Scenes.Marielle.Sleeping2.TurnedAwayCovered".Localized());

                OutputText("Scenes.Marielle.Sleeping2.AbsorbedTooLate".Localized());

                OutputText("Scenes.Marielle.Sleeping2.RecognitionFlashes".Localized());

                OutputText("Scenes.Marielle.Sleeping2.AughtYouNeeded".Localized());

                OutputText("Scenes.Marielle.Sleeping2.SeemsApparent".Localized());
            }

            if (Save.GetInt("fainted", 0) == 1)
                Save.SetInt("fainted", 2);

            ClearButtons();

            AddNextButton("Common.Button.Nothing".Localized(), () => Sleeping3(0));

            AddNextButton("Common.Button.Business".Localized(), () => Sleeping3(1));

            AddNextButton("BodyParts.Kind.Arms.Name".Localized(), () => Sleeping3(2));

            AddNextButton("Common.Button.LetSleep".Localized(), () => Sleeping3(3));
        }

        public void Sleeping3(int choice)
        {
            ClearOutput();

            DisableButton(choice, "You've already done that.");

            switch (choice)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.Sleeping3.MildlyDispleased".Localized());

                        OutputText("Scenes.Marielle.Sleeping3.PermitMeSleep".Localized((View.Time.IsTimeBetween(4f, 7f) ? " a little longer" : "")));

                        AddButton(1, "Common.Button.Business".Localized(), null);

                        AddButton(2, "BodyParts.Kind.Arms.Name".Localized(), null);
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.Sleeping3.SquintsRubsTemples".Localized());

                        OutputText("Scenes.Marielle.Sleeping3.HalfYawnsWord".Localized());

                        OutputText("Scenes.Marielle.Sleeping3.EndOfConversation".Localized());

                        AddButton(0, "Common.Button.Nothing".Localized(), null);
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.Sleeping3.RaisedBrowMeaning".Localized());

                        OutputText("Scenes.Marielle.Sleeping3.WhyNotWearThem".Localized());

                        AddButton(0, "Common.Button.Nothing".Localized(), null);
                    }

                    break;

                case 3:
                    {
                        OutputText("Scenes.Marielle.Sleeping3.PeersThroughCracks".Localized((View.Time.IsTimeBetween(5f, 7f) ? "low-hanging morning sun" : "dark night sky")));

                        OutputText("Scenes.Marielle.Sleeping3.ThankYouGoodNight".Localized());

                        OutputText("Scenes.Marielle.Sleeping3.StepBackFlops".Localized((View.Time.IsTimeBetween(5f, 7f) ? "daybreak" : "nightly")));

                        DoNext(() => ReturnToCamp(1));
                    }

                    break;
            }
        }

        public void Insulted()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Insulted.LooksUpSighing".Localized());

            OutputText("Scenes.Marielle.Insulted.GreetsWithName".Localized());

            OutputText("Scenes.Marielle.Insulted.AughtYouSeek".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Apologize".Localized(), InsultedApologize);

            AddNextButton("Common.Button.Insult".Localized(), InsultedAgain);

            AddNextButton("Common.Button.Commission".Localized(), InsultedCommission);

            AddButton(14, "Common.Button.Leave".Localized(), () => InsultedLeave());
        }

        public void InsultedApologize()
        {
            ClearOutput();

            if (Save.GetInt("insulted", 0) > 2)
            {
                OutputText("Scenes.Marielle.InsultedApologize.BrowRisesApology".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.WordsNaughtButLies".Localized());

                Save.SetInt("insulted", 4);

                AddButton(0, "Common.Button.Apologize".Localized(), null);
            }
            else
            {
                OutputText("Scenes.Marielle.InsultedApologize.BrowsRiseSilent".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.TensionPoursOut".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.GlanceAnastatica".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.DidntMeanIt".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.BesmirchApologyLies".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.AnotherSighAgree".Localized(
                    Save.GetInt("timesYurid", 0) > 0 && Save.GetInt("timesDicked", 0) == 0 ? ", ah... company had been theretofore most delightsome" : " visits were " + (Save.GetInt("timesDicked", 0) != 0 ? "mostly" : "quite") + " pleasant ones theretofore"
                ));

                OutputText("Scenes.Marielle.InsultedApologize.BreaksSilenceName".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.WantsToBeAlone".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.TakeHerByWord".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.SummonsSmallSmile".Localized());

                OutputText("Scenes.Marielle.InsultedApologize.LeaveHerToSelf".Localized());

                Save.SetInt("insulted", 2);

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void InsultedAgain()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.InsultedAgain.BringUpFatherAgain".Localized());

            OutputText("Scenes.Marielle.InsultedAgain.RestrainedFarewell".Localized());

            Save.SetInt("insulted", 3);

            AddButton(1, "Common.Button.Insult".Localized(), null);
        }

        public void InsultedCommission()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.InsultedCommission.InquireCommission".Localized());

            OutputText("Scenes.Marielle.InsultedCommission.ReachesIntoBox".Localized());

            TakeCommission(Save.GetInt("commission", 0), false);
        }

        public void InsultedLeave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.InsultedLeave.CurtlyNods".Localized());

            SceneInstance<BogTempleScene>.Get().TempleMenu(false);
        }

        public void Intro()
        {
            ClearOutput();

            if (Save.GetInt("fainted", 0) == 1)
            {
                OutputText("Scenes.Marielle.Intro.ThrowGreeting".Localized());

                OutputText("Scenes.Marielle.Intro.DoesntReciprocate".Localized());

                OutputText("Scenes.Marielle.Intro.BeginsTrailsOff".Localized());

                OutputText("Scenes.Marielle.Intro.GoodDayToYou".Localized());

                OutputText("Scenes.Marielle.Intro.MustApologize".Localized());

                OutputText("Scenes.Marielle.Intro.InterestPiqued".Localized());

                OutputText("Scenes.Marielle.Intro.EyesHopSideways".Localized());

                OutputText("Scenes.Marielle.Intro.SilenceSettles".Localized());

                OutputText("Scenes.Marielle.Intro.SwirlingGesture".Localized());

                OutputText("Scenes.Marielle.Intro.WantedToSayOut".Localized());

                OutputText("Scenes.Marielle.Intro.HalfStrainedThanks".Localized());

                Save.SetInt("fainted", 2);
            }
            else
            {
                OutputText("Scenes.Marielle.Intro.GreetWorkingSeamstress".Localized());

                OutputText("Scenes.Marielle.Intro.GoodMorrowWelcome".Localized((Player.IsFemale() && Save.GetInt("timesKissed", 0) != 0 && Save.GetInt("insulted", 0) == 0 && !Save.GetBool("mockedDress", false) ? "a warm note of gladness in her soft voice" : "her soft voice pausing for a short moment")));

                OutputText("Scenes.Marielle.Intro.PlaceholderParagraph".Localized(
                    RandomUtils.Choice(
                    ["[say:Desire you to purchase new attire? Mine assortment is not yet as, ah, expansive as I would wish, but perchance something to your liking may still be thereamong.] Swiftly adjusting her glasses, she puts on a professional smile.",
                    "[say:Is't new vesture you seek? Fain would I be of assistance to you, should you, ah, wish me to.] A small, relaxed smile on her face, she regards you patiently and brushes a lone strand of hair from her brow.",

                    "[say:What brings you hither today--attire, my services, or simple causerie? Whatsoever it may be, I shall be pleased to serve.] Folding two of her hands on the desk, the other two on her lap, she regards you with an amiable smile.",

                    "[say:My boutique stands open for your perusal. If there be aught you seek, pray but give your wish a tongue, and I, ah, shall be of service.] An inviting smile on her face, she briefly leads your gaze over her goods before folding her hands.",

                    (Save.GetInt("insulted", 0) != 0 ? "[say:You return again. 'Tis a lengthsome and bemiring journey hither]--she eyes you up and down--[say:one that indeed has cosseted you not. Thus, uhm, prithee be heedful about these fabrics.] Still, she brings up a small smile." : "[say:'Tis good to know you quick and well amidst the, ah, perils of this realm.] Adjusting her glasses, she eyes you up and down as if to confirm her assumption, then folds three of her hands and summons up a satisfied smile.")]
            )
                ));

                OutputText("Scenes.Marielle.Intro.WhatMayIDo".Localized());
            }

            MarielleMenu();
        }

        public void MarielleMenu()
        {

            Save.SetBool("seenNaked", Save.GetBool("seenNaked") || (Save.GetInt("modeled", 0) + Save.GetInt("timesSexed", 0)) > 1);

            ClearButtons();

            AddNextButton("Common.Button.Appearance".Localized(), Appearance);

            AddNextButton("Common.Button.Talk".Localized(), () => TalkMenu(true));

            AddNextButton("Common.Button.Shop".Localized(), () => Shop(true));

            AddNextButton("Common.Button.Lewd".Localized(), () => LewdMenu(true));

            AddRowButton(1, "Commission", () => ShopCommission());

            AddRowButton(2, "Gift Rose", GiveRose);

            AddButton(14, "Common.Button.Leave".Localized(), () => Leave());
        }

        public void Appearance()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Appearance.OutwardAppearance".Localized());

            OutputText("Scenes.Marielle.Appearance.MarkedByUndeath".Localized((Save.GetInt("insulted", 0) == 0 && !Save.GetBool("mockedDress", false) ? " genuine" : "")));

            OutputText("Scenes.Marielle.Appearance.RuffledBlueGown".Localized((Save.GetBool("seenNaked", false) ? "You know she doesn't wear any underwear at all, but despite the occasional breeze, her neckline prevents you from catching " : "She doesn't seem to be wearing any bra, so, depending on wind and angle, you can almost catch side-")));

            AddButton(0, "Common.Button.Appearance".Localized(), null);
        }

        public void Talk(Action returnFunc = null)
        {
            if (View.Time.IsTimeBetween(22.5f, 6.5f))
                DoNext(() =>
                {
                    ClearOutput();

                    var lastSleep = Save.GetInt("lastSleep", 0);

                    var sleep = RandomUtils.Choice(
                        Enumerable.Range(0, 3).Where(index => index != lastSleep).ToArray());

                    OutputText(("Scenes.Marielle.Talk.Sleep" + sleep).Localized());

                    Save.SetInt("lastSleep", sleep);

                    DoNext(() => Go<CampScene>());
                });
            else if (View.Time.Hours + View.Time.Minutes / 60 >= Save.GetFloat("visitTime") + 2)
                DoNext(() =>
                {
                    ClearOutput();

                    var lastBoot = Save.GetInt("lastBoot", 0);

                    var boot = RandomUtils.Choice(
                        Enumerable.Range(0, 5).Where(index => index != lastBoot).ToArray());

                    OutputText(("Scenes.Marielle.Talk.Boot" + boot).Localized());

                    Save.SetInt("lastBoot", boot);

                    DoNext(() => Go<CampScene>());
                });
            else if (returnFunc == null)
                DoNext(() =>
                {
                    ClearOutput();

                    OutputText("Scenes.Marielle.Talk.AnythingElseTalk".Localized());

                    TalkMenu();
                });
            else
                returnFunc?.Invoke();
        }

        public void TalkMenu(bool output = false)
        {
            ClearOutput();

            if (output)
                OutputText("Scenes.Marielle.TalkMenu.SmilesMotionsSeat".Localized(
                    RandomUtils.Choice(["talk", "converse", "a change of words"])
                ));

            ClearButtons();

            AddNextButton("Common.Button.Collapse".Localized(), TalkCollapse);

            AddNextButton("Common.Button.Undeath".Localized(), TalkUndeath);

            AddNextButton("Common.Button.Dressmaking".Localized(), TalkDressmaking);

            AddNextButton("Common.Button.Cart".Localized(), TalkCart);

            AddNextButton("Common.Button.OldWorld".Localized(), TalkOldWorld);

            if ((Save.GetInt("talks", 0) & TALK_DRESSMAKING) != 0)
                AddNextButton("Common.Button.HerLikes".Localized(), TalkHerLikes);

            if ((Save.GetInt("talks", 0) & TALK_DRESSMAKING) != 0 && (Save.GetInt("talks", 0) & TALK_UNDEATH) != 0)
                AddNextButton("Common.Button.Family".Localized(), TalkFamily);

            if ((Save.GetInt("talks", 0) & TALK_FAMILY) != 0)
                AddNextButton("Common.Button.Loneliness".Localized(), TalkLoneliness);

            if ((Save.GetInt("talks", 0) & TALK_HER_LIKES) != 0 && (Save.GetInt("talks", 0) & TALK_FAMILY) != 0)
                AddNextButton("Common.Button.Sex".Localized(), TalkSex);

            if ((Save.GetInt("talks", 0) & TALK_SEX) != 0 && Save.GetBool("seenNaked"))
                AddNextButton("Common.Button.Commando".Localized(), TalkCommando);

            if ((Save.GetInt("talks", 0) & TALK_OLD_WORLD) != 0 && (Save.GetInt("talks", 0) & TALK_FAMILY) != 0)
                AddNextButton("Common.Button.Father".Localized(), TalkFather);

            AddButton(14, "Common.Button.Back".Localized(), () => TalkBack());
        }

        public void TalkBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkBack.ConsiderWhatElse".Localized());

            MarielleMenu();
        }

        public void TalkCollapse()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkCollapse.FirstMetUnconscious".Localized());

            OutputText("Scenes.Marielle.TalkCollapse.VexationFliesOver".Localized());

            OutputText("Scenes.Marielle.TalkCollapse.AdriftOblivion".Localized());

            OutputText("Scenes.Marielle.TalkCollapse.ShamefulYetUndone".Localized());

            OutputText("Scenes.Marielle.TalkCollapse.FrownsStraightens".Localized());

            OutputText("Scenes.Marielle.TalkCollapse.BecauseSheUndead".Localized());

            OutputText("Scenes.Marielle.TalkCollapse.TollForUndead".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_COLLAPSE);

            View.Time.PassMinutes((int)Round((1 / 3) * 60));

            Talk();
        }

        public void TalkUndeath()
        {
            ClearOutput();

            if (Save.GetInt("insulted", 0) != 0)
            {
                OutputText("Scenes.Marielle.TalkUndeath.WantTalkMoreUndeath".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.AsksInChallenge".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.PardonLetUsNot".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.InsultedHerFather".Localized());

                View.Time.PassMinutes((int)Round((1 / 6) * 60));
            }
            else
            {
                OutputText("Scenes.Marielle.TalkUndeath.AskWhatUndeadLike".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.TakesMomentToThink".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.NotMindlessZombie".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.ContemplativeHum".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.ChangingCovertly".Localized());

                OutputText("Scenes.Marielle.TalkUndeath.FaceFallsLittle".Localized());

                if (Save.GetInt("rose", 0) > 1)
                {
                    OutputText("Scenes.Marielle.TalkUndeath.EyesFollowToAnastatica".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.JerksFromDaydream".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.NotAgelessResilient".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.AskWhatWouldHappen".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.UnwholesomeButScoots".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.TalkUndeath.ReviveTopicHowUndead".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.SurprisedByOutburst".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.SpreadsFingersEmphasis".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.SavedThatDay".Localized());

                    OutputText("Scenes.Marielle.TalkUndeath.GazesWistfully".Localized());
                }

                View.Time.PassMinutes((int)Round((3 / 4) * 60));
            }

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_UNDEATH);

            Talk();
        }

        public void TalkDressmaking()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkDressmaking.AskWhyDressmaking".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking.SheBlinks".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => TalkDressmaking2());

            AddButton(1, "Common.Button.No".Localized(), () => TalkDressmaking2(false));
        }

        public void TalkDressmaking2(bool strange = true)
        {

            if (strange)
            {
                OutputText("Scenes.Marielle.TalkDressmaking2.TellHerItIs".Localized());

                OutputText("Scenes.Marielle.TalkDressmaking2.ReflectsOnWords".Localized());

                OutputText("Scenes.Marielle.TalkDressmaking2.SoundsLikeSomething".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.TalkDressmaking2.TellHerItIsnt".Localized());

                OutputText("Scenes.Marielle.TalkDressmaking2.AdjustsGlassesSmiling".Localized());
            }

            OutputText("Scenes.Marielle.TalkDressmaking2.LooksDownAtHerself".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking2.MusesUntwirls".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking2.ShiesAway".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking2.SubtleUndertone".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking2.GazeShifts".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking2.MeetsYourEyesAgain".Localized());

            OutputText("Scenes.Marielle.TalkDressmaking2.AnswersYourQuestion".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_DRESSMAKING);

            View.Time.PassMinutes((int)Round((2 / 3) * 60));

            Talk();
        }

        public void TalkHerLikes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkHerLikes.PassionateProfession".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.RuedNoInstrument".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.HarpsBrieflyCoquetted".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.WhyNotBuyOne".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.SpeaksOfLyre".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.AskAnythingMore".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.LetsGoOfTress".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.SoFarAllFine".Localized());

            OutputText("Scenes.Marielle.TalkHerLikes.GivesItDeepThought".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_HER_LIKES);

            ClearButtons();

            AddNextButton("Common.Button.Wait".Localized(), TalkHerLikesWait);

            AddNextButton("Common.Button.WrapUp".Localized(), TalkHerLikesWrapUp);
        }

        public void TalkHerLikesWait()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkHerLikesWait.InNoHurry".Localized());

            OutputText("Scenes.Marielle.TalkHerLikesWait.ScansSurroundings".Localized());

            OutputText("Scenes.Marielle.TalkHerLikesWait.NearWhispersSnow".Localized());

            OutputText("Scenes.Marielle.TalkHerLikesWait.DroopsAtWords".Localized());

            OutputText("Scenes.Marielle.TalkHerLikesWait.NoMoreIdeas".Localized());

            View.Time.PassMinutes((int)Round((5 / 6) * 60));

            Talk();
        }

        public void TalkHerLikesWrapUp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkHerLikesWrapUp.WrapConversationUp".Localized());

            OutputText("Scenes.Marielle.TalkHerLikesWrapUp.FlicksFanShut".Localized());

            View.Time.PassMinutes((int)Round((7 / 12) * 60));

            Talk();
        }

        public void TalkCart()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkCart.CartQuiteCurious".Localized());

            OutputText("Scenes.Marielle.TalkCart.LovinglyRunsHand".Localized());

            OutputText("Scenes.Marielle.TalkCart.DidSheMakeIt".Localized());

            OutputText("Scenes.Marielle.TalkCart.SightRoamsCart".Localized());

            OutputText("Scenes.Marielle.TalkCart.GentlyPatsSurface".Localized());

            OutputText("Scenes.Marielle.TalkCart.TreatedWalnutCart".Localized());

            OutputText("Scenes.Marielle.TalkCart.AgeApparentButCared".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_CART);

            View.Time.PassMinutes((int)Round((1 / 3) * 60));

            Talk();
        }

        public void TalkOldWorld()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkOldWorld.AskOtherDimensions".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.OnlyHmmUtters".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.SaysQuiteDifferent".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.StrifeNotInvaders".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.AversionSacredSee".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.AskAboutPeople".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.OnlyHumansThere".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.MarethQuiteBizarre".Localized());

            OutputText("Scenes.Marielle.TalkOldWorld.SpareJoylessTalk".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_OLD_WORLD);

            View.Time.PassMinutes((int)Round((3 / 4) * 60));

            Talk();
        }

        public void TalkFamily()
        {
            ClearOutput();

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_FAMILY);

            if (Save.GetInt("insulted", 0) != 0)
            {
                OutputText("Scenes.Marielle.TalkFamily.BringUpFamily".Localized());

                OutputText("Scenes.Marielle.TalkFamily.NaughtElseToSpeak".Localized());

                OutputText("Scenes.Marielle.TalkFamily.ClearlyUnwilling".Localized());

                View.Time.PassMinutes((int)Round((1 / 6) * 60));

                Talk();
            }
            else
            {
                if (Save.GetInt("rose", 0) > 1)
                {
                    OutputText("Scenes.Marielle.TalkFamily.AskFamilyMore".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.InterlacesFingers".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.MotherDiedYoung".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.NoSiblingsRemarryNot".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.NotForgetGrandparents".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.OnlyFatherWasFamily".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.CenterOfMyLife".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.TalkFamily.EntirelyAloneHere".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.MetWithSilence".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.BusyFatherMadeTime".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.AskWhereHeIs".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.HePassedAgo".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.ReachesForWords".Localized());

                    OutputText("Scenes.Marielle.TalkFamily.AlwaysWarmHearted".Localized());
                }

                ClearButtons();

                AddNextButton("Common.Button.Continue".Localized(), TalkFamilyContinue);

                AddNextButton("Common.Button.Ingnam".Localized(), TalkFamilyIngnam);
            }
        }

        public void TalkFamilyContinue()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFamilyContinue.TimeWoundsMender".Localized());

            OutputText("Scenes.Marielle.TalkFamilyContinue.ApologiesMelancholy".Localized());

            OutputText("Scenes.Marielle.TalkFamilyContinue.FatherMostWonderful".Localized());

            OutputText("Scenes.Marielle.TalkFamilyContinue.WhyNotRemarry".Localized());

            OutputText("Scenes.Marielle.TalkFamilyContinue.ProdsPincushion".Localized());

            OutputText("Scenes.Marielle.TalkFamilyContinue.WouldBelieveHeWaited".Localized());

            OutputText("Scenes.Marielle.TalkFamilyContinue.TwirlsNeedle".Localized());

            View.Time.PassMinutes((int)Round((5 / 6) * 60));

            Talk();
        }

        public void TalkFamilyIngnam()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFamilyIngnam.PeopleLeftIngnam".Localized());

            OutputText("Scenes.Marielle.TalkFamilyIngnam.SmileNotReachEyes".Localized());

            OutputText("Scenes.Marielle.TalkFamilyIngnam.SighsOnceMore".Localized());

            OutputText("Scenes.Marielle.TalkFamilyIngnam.SmileVanishesAgain".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            Talk();
        }

        public void TalkLoneliness()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkLoneliness.EverFeelLonely".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.RaisedBrowsNo".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.SubmergesInWork".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.WrinklesFormForehead".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.ShamefulFlush".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.FineWithYouRemark".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.LaidBareFirstMet".Localized());

            OutputText("Scenes.Marielle.TalkLoneliness.MurmursDoubtfulEyes".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_LONELINESS);

            ClearButtons();

            AddNextButton("Common.Button.MoveOn".Localized(), TalkLonelinessMoveOn);

            AddNextButton("Common.Button.Bazaar".Localized(), TalkLonelinessBazaar);

            AddNextButton("Common.Button.CampInvite".Localized(), TalkLonelinessInvite);
        }

        public void TalkLonelinessMoveOn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkLonelinessMoveOn.HerWishRespected".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessMoveOn.TiltHeadInvite".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessMoveOn.NoFrettingSpiltMilk".Localized());

            View.Time.PassMinutes((int)Round((2 / 3) * 60));

            Talk();
        }

        public void TalkLonelinessBazaar()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.BeenToBazaar".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.IndeedNotLongAfter".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.SaysILeft".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.RealizesHopedMore".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.DaredNotStay".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.BoutiqueThriving".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessBazaar.MerchantKindEnough".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBenoits) > 0)
            {
                OutputText("Scenes.Marielle.TalkLonelinessBazaar.SoundsLikeBenoit".Localized());

                OutputText("Scenes.Marielle.TalkLonelinessBazaar.BenoitLizardRepeats".Localized());

                OutputText("Scenes.Marielle.TalkLonelinessBazaar.TellHeDoingFine".Localized((CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) > 0 ? " Though you inform her the basilisk has fulfilled something of a dream of his, with your help, and has turned female now, going by the name of 'Benoite'.[pg]Surprise flashes over her face. [say:Has he?] she half-mumbles, evidently not sure what to make of that and losing herself in the distance for a bit before snapping back." : "")));

                OutputText("Scenes.Marielle.TalkLonelinessBazaar.GladdensHeFaresWell".Localized());

                OutputText("Scenes.Marielle.TalkLonelinessBazaar.NeverReturn".Localized());
            }
            else
                OutputText("Scenes.Marielle.TalkLonelinessBazaar.HopesHeAlive".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            Talk();
        }

        public void TalkLonelinessInvite()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkLonelinessInvite.InviteToCamp".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessInvite.LightsUpAtIdea".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessInvite.PrefersQuietHere".Localized());

            OutputText("Scenes.Marielle.TalkLonelinessInvite.NoConvincingHer".Localized());

            View.Time.PassMinutes((int)Round((2 / 3) * 60));

            Talk();
        }

        public void TalkSex()
        {
            ClearOutput();

            if ((Save.GetInt("talks", 0) & TALK_SEX) != 0)
            {
                OutputText("Scenes.Marielle.TalkSex.PleadsNotAgain".Localized());

                ClearButtons();

                AddNextButton("Scenes.Dullahan.CuddleDull.ButtonInsist".Localized(), TalkSexInsist);

                AddNextButton("Common.Button.Relent".Localized(), TalkSexRelent);
            }
            else
            {
                OutputText("Scenes.Marielle.TalkSex.NoInterestYet".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Tactful".Localized(), () => TalkSex2(true));

                AddNextButton("Common.Button.Direct".Localized(), () => TalkSex2(false));

                AddNextButton("Scenes.CorruptedGlade.NeverMind".Localized(), TalkSexNevermind);
            }
        }

        public void TalkSexNevermind()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexNevermind.NeverMindAnotherTime".Localized());

            TalkMenu();
        }

        public void TalkSex2(bool tactful)
        {
            ClearOutput();

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_SEX);

            if (tactful)
            {
                OutputText("Scenes.Marielle.TalkSex2.GentleApproachFruitful".Localized());

                OutputText("Scenes.Marielle.TalkSex2.CocksBrowGeneral".Localized());

                OutputText("Scenes.Marielle.TalkSex2.HonestyCourtesyWar".Localized());

                OutputText("Scenes.Marielle.TalkSex2.WellSchooledDesirable".Localized());

                OutputText("Scenes.Marielle.TalkSex2.WavesSuggestionOff".Localized());

                OutputText("Scenes.Marielle.TalkSex2.RaspyApologies".Localized());

                OutputText("Scenes.Marielle.TalkSex2.PrefersOtherWomen".Localized());

                OutputText("Scenes.Marielle.TalkSex2.AdoresFeminalForm".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.TalkSex2.PryHerOpen".Localized());

                OutputText("Scenes.Marielle.TalkSex2.NotFrigidFiguratively".Localized());

                OutputText("Scenes.Marielle.TalkSex2.ScramblesForWords".Localized());
            }

            switch ((int)Player.Gender)
            {
                case 1:
                    {
                        OutputText("Scenes.Marielle.TalkSex2.SoGirlsItIs".Localized());

                        if (Player.Femininity >= 59)
                        {
                            OutputText("Scenes.Marielle.TalkSex2.EyesShootUpTransfixed".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.WordLikeItHurts".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marielle.TalkSex2.ThinksIgnoringQuestion".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.EnjoyVisitsNotFond".Localized());
                        }
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.TalkSex2.WhatAboutYouThen".Localized((Player.IsFeminine() && Player.Femininity > 50 ? "you're certainly a [if (ischild) {girl|woman}]" : "you certainly have the right parts")));

                        if (Player.Femininity >= 59)
                        {
                            OutputText("Scenes.Marielle.TalkSex2.LooksUpGlimmeringQuestion".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.PassingPulchrous".Localized());

                            ClearButtons();

                            AddNextButton("Common.Button.Kiss".Localized(), TalkSexKiss);

                            AddNextButton("Common.Button.Next".Localized(), () => TalkSex3(true));

                            return;
                        }
                        else
                        {
                            OutputText("Scenes.Marielle.TalkSex2.WordsMakeHaltFrown".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.InterestedCertainly".Localized());
                        }
                    }

                    break;

                case 3:
                    {
                        OutputText("Scenes.Marielle.TalkSex2.PackingBothWhatNow".Localized());

                        if (Player.Femininity >= 59)
                        {
                            OutputText("Scenes.Marielle.TalkSex2.StudiesFaceEscapes".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.StartsUpHesitant".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marielle.TalkSex2.NotTerriblySurprised".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.NotFondPhalli".Localized());

                            OutputText("Scenes.Marielle.TalkSex2.TriesStartAgain".Localized());
                        }
                    }

                    break;

                case 0:
                    {
                        OutputText("Scenes.Marielle.TalkSex2.NothingLiterally".Localized());

                        if (Player.Femininity >= 59)
                            OutputText("Scenes.Marielle.TalkSex2.StaresClearlyConfused".Localized());
                        else
                            OutputText("Scenes.Marielle.TalkSex2.SurprisinglyIndifferent".Localized());
                    }

                    break;
            }

            TalkSex3();
        }

        public void TalkSexKiss()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexKiss.CuteWhenFlustered".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.AsksAnxiousFlinches".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.ColdStiffUnprepared".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.FlushedBeyondEmbarrassment".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.TapersOff".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.PulledBackToReality".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.AnswersNowhereNear".Localized());

            OutputText("Scenes.Marielle.TalkSexKiss.NeverMindFumbling".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);

            View.Time.PassMinutes((int)Round((5 / 6) * 60));

            Talk();
        }

        public void TalkSex3(bool clear = false)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSex3.AskIfExperience".Localized());

            OutputText("Scenes.Marielle.TalkSex3.EyesShiftAway".Localized((Player.DemonScore() >= 4 ? "Oh! I, ah, I mean no offence, of course." : "As you, ah, well know belike.")));

            OutputText("Scenes.Marielle.TalkSex3.CeaseThisTalk".Localized());

            View.Time.PassMinutes((int)Round((2 / 3) * 60));

            Talk();
        }

        public void TalkSexRelent()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexRelent.LetGoOfTopic".Localized());

            OutputText("Scenes.Marielle.TalkSexRelent.HalfSighs".Localized());

            View.Time.PassMinutes((int)Round((1 / 6) * 60));

            Talk(() => TalkMenu());
        }

        public void TalkSexInsist()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexInsist.PushTopicResigns".Localized());

            if ((Save.GetInt("talks", 0) & TALK_SEX_2) != 0)
                OutputText("Scenes.Marielle.TalkSexInsist.ExhaleMeetsPersistence".Localized());
            else
            {
                OutputText("Scenes.Marielle.TalkSexInsist.FavorsWomen".Localized());

                OutputText("Scenes.Marielle.TalkSexInsist.EmbarrassmentShows".Localized());

                Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_SEX_2);
            }

            ClearButtons();

            AddNextButton("Common.Button.Preference".Localized(), TalkSexPreference);

            AddNextButton("Common.Button.Partners".Localized(), TalkSexPartners);

            AddNextButton("Common.Button.Pregnancy".Localized(), TalkSexPregnancy);

            AddNextButton("Common.Button.Relent".Localized(), TalkSexRelent);
        }

        public void TalkSexPreference()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPreference.PressHerPreference".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.GirlLooksBlushing".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.DivineWhimBrought".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.UnproperUnnatural".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.FatherRetainedHandmaid".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.BlossomedComelyWoman".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.FingersStopNever".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.TimorousDreadJudgement".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.ScantestScruple".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.ShudderToThink".Localized());

            OutputText("Scenes.Marielle.TalkSexPreference.DivagatedIntoTristesse".Localized());

            View.Time.PassMinutes((int)Round((2 / 3) * 60));

            Talk();
        }

        public void TalkSexPartners()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPartners.AskAboutPartners".Localized());

            OutputText("Scenes.Marielle.TalkSexPartners.ReinsHerselfFumbles".Localized());

            OutputText("Scenes.Marielle.TalkSexPartners.MumblesFidgeting".Localized());

            OutputText("Scenes.Marielle.TalkSexPartners.LooksNervouslyAside".Localized());

            OutputText("Scenes.Marielle.TalkSexPartners.PerceptionDoffsVeils".Localized((Player.IsFemale() && Player.Femininity >= 50 && Player.Race == "demon-morph" ? "She keeps anxiously glancing at you, suddenly well-aware of what you nearly are." : "She seems lost in something for a bit after saying that.")));

            OutputText("Scenes.Marielle.TalkSexPartners.HeadCocksAside".Localized());

            OutputText("Scenes.Marielle.TalkSexPartners.ParagraphBreak".Localized());

            ClearButtons();

            AddNextButton("Common.Button.DropIt".Localized(), TalkSexPartnersDrop);

            AddNextButton("Stats.Corruption.Name".Localized(), TalkSexPartnersCorruption);
        }

        public void TalkSexPartnersDrop()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPartnersDrop.LetHerGatherHerself".Localized());

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            Talk();
        }

        public void TalkSexPartnersCorruption()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPartnersCorruption.WondersAboutCorruption".Localized());

            OutputText("Scenes.Marielle.TalkSexPartnersCorruption.PerksUpAtWarning".Localized());

            OutputText("Scenes.Marielle.TalkSexPartnersCorruption.UnclaspsTwoHands".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Finish".Localized(), TalkSexPartnersFinish);

            AddNextButton("Common.Button.Warn".Localized(), TalkSexPartnersWarn);
        }

        public void TalkSexPartnersFinish()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPartnersFinish.CautiousEnough".Localized());

            View.Time.PassMinutes((int)Round((3 / 4) * 60));

            Talk();
        }

        public void TalkSexPartnersWarn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPartnersWarn.CredoNotEnough".Localized());

            OutputText("Scenes.Marielle.TalkSexPartnersWarn.WordsRingTrue".Localized());

            OutputText("Scenes.Marielle.TalkSexPartnersWarn.SilenceBrokenExhale".Localized());

            OutputText("Scenes.Marielle.TalkSexPartnersWarn.NeverTooKeenVigil".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            Talk();
        }

        public void TalkSexPregnancy()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPregnancy.CanSheGetPregnant".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.StudiesFaceGlasses".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.GuideLostBackHither".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.AskIfLikeGhost".Localized((CoC.Instance.Flags.Get(GameFlagIds.DullahanMet) > 0 ? ", and reminiscent of what the Dullahan can do with her head" : "")));

            OutputText("Scenes.Marielle.TalkSexPregnancy.EchoesNaughtSuchlike".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.BringsSeveredArm".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.YetWieldCommand".Localized((CoC.Instance.Flags.Get(GameFlagIds.DullahanMet) > 0 ? ", and reminiscent of what the Dullahan can do with her head" : "")));

            OutputText("Scenes.Marielle.TalkSexPregnancy.RemembersStrayedOff".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.WouldBeLiar".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.UnderFathersPinion".Localized());

            OutputText("Scenes.Marielle.TalkSexPregnancy.EyesYouUpDown".Localized());

            ClearButtons();

            AddNextButton("Scenes.Sandwitchscene.SammitchBirthsDriders.CuriousWord".Localized(), () => TalkSexPregnancy2(0));

            AddNextButton("Common.Button.KnowAll".Localized(), () => TalkSexPregnancy2(1));

            AddNextButton("Common.Button.KnockUp".Localized(), () => TalkSexPregnancy2(2));
        }

        public void TalkSexPregnancy2(int answer)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkSexPregnancy2.YouWereJustCurious".Localized(new[] {
            "You were just curious about it, that's all.[pg][say:I see. Well, I mind not our causeries, howbeit I would they were... less catechismal.]",

            "You want to know all there is to her, including her most intimate secrets. She stares at you, disconcerted, " + (Player.IsFemale() || (Player.Femininity >= 59 && !Player.HasCock()) ? "though also does a growing blush then conquer her face. The girl hurriedly clears her throat and looks away" : "then faintly scoots away from you, folding her arms again") + ".[pg][say:I, ah... I see.]",

            "[if (hascock) {You wanted to know if you could pump her full of spunk, knock her up, put some life into that cold belly of hers.|You don't have a dick yourself, but if you did, you'd definitely want to put a child or two into her, possible or not.}][pg][say:I see,] she says, unconsciously folding all four arms over her middle."
        }[answer]
            ));

            View.Time.PassMinutes((int)Round((5 / 6) * 60));

            Talk();
        }

        public void TalkCommando()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkCommando.NeverSeenHerWear".Localized());

            OutputText("Scenes.Marielle.TalkCommando.FingersWeaveTips".Localized());

            OutputText("Scenes.Marielle.TalkCommando.LayerOfCloth".Localized());

            OutputText("Scenes.Marielle.TalkCommando.SoundedPractised".Localized());

            OutputText("Scenes.Marielle.TalkCommando.BraNotNecessary".Localized());

            OutputText("Scenes.Marielle.TalkCommando.WaversSwaysAside".Localized());

            OutputText("Scenes.Marielle.TalkCommando.MakesItSoundPossible".Localized());

            OutputText("Scenes.Marielle.TalkCommando.SaysVoiceCautious".Localized());

            OutputText("Scenes.Marielle.TalkCommando.WarmthFadesCheeks".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_COMMANDO);

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            Talk();
        }

        public void TalkFather()
        {
            ClearOutput();

            if (Save.GetInt("insulted", 0) != 0)
            {
                OutputText("Scenes.Marielle.TalkFather.TriesAskCutOff".Localized());

                OutputText("Scenes.Marielle.TalkFather.VoiceSoftStern".Localized());

                OutputText("Scenes.Marielle.TalkFather.NoMoreDiscussion".Localized());

                View.Time.PassMinutes((int)Round((1 / 6) * 60));

                Talk();
            }
            else
            {
                if (Save.GetInt("rose", 0) < 2)
                {
                    OutputText("Scenes.Marielle.TalkFather.ToldLittleReally".Localized());

                    OutputText("Scenes.Marielle.TalkFather.SoMuchSoLittle".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.TalkFather.BroachFatherAgain".Localized());

                    OutputText("Scenes.Marielle.TalkFather.MurmursStudyingPlant".Localized());

                    OutputText("Scenes.Marielle.TalkFather.PrayAskOn".Localized());
                }

                Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_FATHER);

                TalkFatherMenu();
            }
        }

        public void TalkFatherMenu(int from = -1)
        {

            ClearButtons();

            AddNextButton("Common.Button.Background".Localized(), TalkFatherBackground);

            AddNextButton("Common.Button.Work".Localized(), TalkFatherWork);

            AddNextButton("Common.Button.Fondness".Localized(), TalkFatherFondness);

            AddNextButton("Common.Button.HerDeath".Localized(), TalkFatherHerDeath);

            AddNextButton("Common.Button.HisDeath".Localized(), TalkFatherHisDeath);

            AddNextButton("Common.Button.Necromancy".Localized(), TalkFatherNecromancy);

            AddButton(14, "Common.Button.ThatsAll".Localized(), () => TalkFatherLeave());

            if (from > -1)
                DisableButton(from, "You just asked about that.");
        }

        public void TalkFatherBackground()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherBackground.AskHisBackground".Localized());

            OutputText("Scenes.Marielle.TalkFatherBackground.IdlyTapsDesk".Localized());

            OutputText("Scenes.Marielle.TalkFatherBackground.SimpleManHumbleBirth".Localized());

            OutputText("Scenes.Marielle.TalkFatherBackground.TopeAfterBattles".Localized());

            OutputText("Scenes.Marielle.TalkFatherBackground.GraciousFateDelivered".Localized());

            OutputText("Scenes.Marielle.TalkFatherBackground.FatherShakenComeDawn".Localized());

            OutputText("Scenes.Marielle.TalkFatherBackground.EverythingWentWell".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_BACKGROUND);

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            Talk(() => TalkFatherMenu(0));
        }

        public void TalkFatherWork()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherWork.AskWhatHeWorked".Localized());

            OutputText("Scenes.Marielle.TalkFatherWork.ThaumaturgeInstructor".Localized());

            OutputText("Scenes.Marielle.TalkFatherWork.ProgenitureHealing".Localized());

            OutputText("Scenes.Marielle.TalkFatherWork.MagicTutorDaughter".Localized());

            OutputText("Scenes.Marielle.TalkFatherWork.PianoAgainstEachOther".Localized());

            OutputText("Scenes.Marielle.TalkFatherWork.DisappointingIndeed".Localized());

            OutputText("Scenes.Marielle.TalkFatherWork.MyApologies".Localized());

            View.Time.PassMinutes((int)Round((1 / 3) * 60));

            Talk(() => TalkFatherMenu(1));
        }

        public void TalkFatherFondness()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherFondness.AskAnyFondness".Localized());

            OutputText("Scenes.Marielle.TalkFatherFondness.BesidesMotherAndI".Localized());

            OutputText("Scenes.Marielle.TalkFatherFondness.DisheartenedDaughter".Localized());

            OutputText("Scenes.Marielle.TalkFatherFondness.StrokesAndPinches".Localized());

            OutputText("Scenes.Marielle.TalkFatherFondness.SitsUpStraighter".Localized());

            OutputText("Scenes.Marielle.TalkFatherFondness.BesidesBooks".Localized());

            if (Save.GetInt("rose", 0) < 2)
            {
                OutputText("Scenes.Marielle.TalkFatherFondness.FlowerOfStone".Localized());

                OutputText("Scenes.Marielle.TalkFatherFondness.SomedealMiraculousHerb".Localized());

                OutputText("Scenes.Marielle.TalkFatherFondness.StifledChuckle".Localized());

                OutputText("Scenes.Marielle.TalkFatherFondness.SproutedWhenBorn".Localized());

                OutputText("Scenes.Marielle.TalkFatherFondness.LittleToBeDone".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.TalkFatherFondness.DesertRoseDesk".Localized());

                OutputText("Scenes.Marielle.TalkFatherFondness.AdmiresWhiteBlossoms".Localized());
            }

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_FONDNESS);

            if (Save.GetInt("rose", 0) == 0)
                Save.SetInt("rose", 1);

            View.Time.PassMinutes((int)Round((2 / 3) * 60));

            Talk(() => TalkFatherMenu(2));
        }

        public void TalkFatherHerDeath()
        {
            ClearOutput();

            if (Save.GetInt("rose", 0) < 2)
            {
                OutputText("Scenes.Marielle.TalkFatherHerDeath.AskHowSheDied".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.GlancesAsideWares".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.AsLovingFatherWould".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.RecallsLittleThereof".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.FearYouDeath".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Yes".Localized(), () => TalkFatherHerDeathFirst(0));

                AddNextButton("Common.Button.No".Localized(), () => TalkFatherHerDeathFirst(1));

                AddNextButton("Common.Button.DontKnow".Localized(), () => TalkFatherHerDeathFirst(2));
            }
            else
            {
                OutputText("Scenes.Marielle.TalkFatherHerDeath.SheWasSomewhatVague".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.CrossesLowerArms".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.SlaughteredEventide".Localized(((Save.GetInt("talks", 0) & TALK_HER_DEATH) != 0 ? "as you well know" : "see you")));

                OutputText("Scenes.Marielle.TalkFatherHerDeath.AndRestWasDarkness".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeath.HushedAtmosphere".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Continue".Localized(), () => TalkFatherHerDeathSecond(true));

                AddNextButton("Common.Button.NoSpecifics".Localized(), () => TalkFatherHerDeathSecond(false));
            }

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_HER_DEATH);
        }

        public void TalkFatherHerDeathFirst(int answer)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherHerDeathFirst.YouDoYouDon".Localized(
                new[] { "You do--you don't want to die. She vaguely nods at your words.",

            "You don't--death is not something that scares you. Her eyes briefly flit to you, and she stays silent for a while.",

            "You don't know--you've never given it enough thought. She simply stays silent for a moment." }[answer]
            ));

            OutputText("Scenes.Marielle.TalkFatherHerDeathFirst.SaysISee".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathFirst.ThenAwokeAgain".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathFirst.MaugreItAll".Localized());

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            Talk(() => TalkFatherMenu(3));
        }

        public void TalkFatherHerDeathSecond(bool specifics)
        {
            ClearOutput();

            if (specifics)
            {
                OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.NodToContinue".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.TrailsOffStruggling".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.TravelledSomewhereFar".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.FaceSinksDeeper".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.GruesomeScene".Localized());

                OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.NodsAtTinyBox".Localized());
            }

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.ReturnedMeManor".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond.DithersGraspingHands".Localized());

            if ((Save.GetInt("talks", 0) & TALK_HER_DEATH_2) != 0)
                TalkFatherHerDeathSecond2(5);
            else
            {
                ClearButtons();

                AddNextButton("Common.Button.Fascination".Localized(), () => TalkFatherHerDeathSecond2(0));

                AddNextButton("Common.Button.Surprise".Localized(), () => TalkFatherHerDeathSecond2(1));

                AddNextButton("Common.Button.Confirmation".Localized(), () => TalkFatherHerDeathSecond2(2));

                AddNextButton("Common.Button.Indifference".Localized(), () => TalkFatherHerDeathSecond2(3));

                AddNextButton("Common.Button.Disgust".Localized(), () => TalkFatherHerDeathSecond2(4));

                AddNextButton("Common.Button.Revulsion".Localized(), TalkFatherHerDeathRevulsion);
            }
        }

        public void TalkFatherHerDeathSecond2(int answer)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.YouReInterestedIn".Localized(
                new[] { "You're interested in, or perhaps even fascinated with necromancy, and the one to revive her having been her own father could present a valuable opportunity. You'll have to find out everything you can about it, later.[pg]Marielle looks at you, perplexed, all tension fading to nothing. [say:Pardon me?] Before you can elaborate, she shakes her head, blinks, and adds, [say:Nay, I, ah, I mean... Never mind. I suppose if you be... interested]--by her intonation, she doesn't quite believe you--[say:I could impart unto you what parcels of knowledge I owe... although 'twould be but few, for never shared he much, and I... Hm.] That sounds good enough for now, so you let her find the thread again.",

            "You're surprised, honestly. There had to be someone who revived her, of course, but you wouldn't have thought it was her own father, of all people. It does explain a lot, though.[pg]When it becomes clear that this is the extent of your reaction, Marielle's anxiousness soon changes to relief, and she lets go of a long-held breath. [say:Well, I--] she starts and stops. [say:No, never mind.] Her expression then takes on a more contemplative quality as she silently ponders.",

            "You had a pretty good idea it was him, and it does explain a lot, so you're not exactly surprised by her confirmation of your hunch.[pg]All that anxiousness quickly fades in favor of confusion, leaving Marielle's ice-blue eyes wide open before she catches herself again. [say:Is... Is that so?] You reaffirm it. [say:Huh, hmm... Well, I... I must confess, to hear you proclaim thus is rather....] She looks concerned all of a sudden, furrowing her forehead and murmuring, [say:Betrayed, by such extents?] The rest of that she only mouths, but as she rubs her left temple and pushes up her glasses, her expression does steadily change back to a more neutral, contemplative one.",

            "Someone had to have revived her, and that someone having been her father was a possibility. In any case, you don't have much to say about it.[pg][say:Hmm.] Marielle's anxiousness slowly fades, replaced by something like relief as she absent-mindedly keeps nodding to the empty air. You then notice her features take on a contemplative look as she considers whatever it is she's considering.",

            "Necromancers are vile, disgusting creatures, not worthy of being left alive, but you won't tell her that. Keeping your expression neutral, you simply motion her to continue.[pg]Marielle looks quite surprised by your outward lack of a reaction, eyeing you with what you think is a sliver of doubt, but it soon changes to relief, and she lets go of a held breath. [say:Well, I--] she starts. [say:Ah, no, never mind.] Giving you another sideways glance, she turns to contemplation.",

            "[if (silly) {Well, you've already heard this one; you tell her you're just here for a quick reminder|You have nothing else to add to last time, so you simply tell her to go on}], which is met with a near-silent hum as she relaxes and turns onto contemplation." }[answer]
            ));

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.AnathematizedArt".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.AnswersInRush".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.SafestDiscretion".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.SennightsToAmend".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.CapturedByLantern".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.GardensAblaze".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.RisingStrainObvious".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.RelaxesAdjustsSpectacles".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.NaryThingOurOwn".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.FallFromLuxury".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathSecond2.LongRambleApologies".Localized());

            View.Time.PassMinutes((int)Round((3 / 2) * 60));

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_HER_DEATH_2);

            Talk(() => TalkFatherMenu(3));
        }

        public void TalkFatherHerDeathRevulsion()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.NecromancersVileCreatures".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.AnxietyTurnsBafflement".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.QuiveringVoice".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.BowstringSnaps".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.NotPermitSlander".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.RagePalpable".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.WintryEyesRelent".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.RakesHairBack".Localized());

            OutputText("Scenes.Marielle.TalkFatherHerDeathRevulsion.NoMoodForMore".Localized());

            Save.SetInt("insulted", 1);

            Save.SetInt("reenabled", View.Time.Days + 1);

            Save.SetFloat("excludeExplore", View.Time.TotalTime + (30 - View.Time.Hours));

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkFatherHisDeath()
        {
            ClearOutput();

            if (Save.GetInt("rose", 0) < 2)
            {
                OutputText("Scenes.Marielle.TalkFatherHisDeath.AsksAboutFather".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.SilenceLiesHeavy".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.BarelyAudible".Localized());

                View.Time.PassMinutes((int)Round((1 / 6) * 60));

                Talk(() => TalkFatherMenu(4));
            }
            else
            {
                OutputText("Scenes.Marielle.TalkFatherHisDeath.AsksHowHeDied".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.SilenceDragsOn".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.BeginsLevelVoice".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.SettleInListen".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.DrawsThreadUp".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.MerryTimeForUs".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.RealmCalmed".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.TooBestraughtGaze".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.MurmursWanHope".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.SweepsHairBehind".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.NailsScrapeWood".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.VoicesInGale".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.VoiceBreaks".Localized());

                OutputText("Scenes.Marielle.TalkFatherHisDeath.TriesMouthAgain".Localized());

                Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_HIS_DEATH);

                ClearButtons();

                AddNextButton("Common.Button.DoNothing".Localized(), () => TalkFatherHisDeath2(0));

                AddNextButton("Common.Button.Comfort".Localized(), () => TalkFatherHisDeath2(1));

                AddNextButton("Common.Button.Hug".Localized(), () => TalkFatherHisDeath2(2));
            }
        }

        public void TalkFatherHisDeath2(int answer)
        {
            ClearOutput();

            switch (answer)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.WaitsOutGrief".Localized());

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.BreathingEvensOut".Localized());

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.RaspsPray".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.OffersComfort".Localized());

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.KindWordsUnseen".Localized());

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.BeginsStillRaspy".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.NoTimeForWords".Localized());

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.CannotSeeFace".Localized((Save.GetInt("timesKissed", 0) != 0 && Player.IsFemale() ? "hands grasp yours, urging you to clutch her a little tighter to yourself as she leans into you" : "leaning into you ever so slightly")));

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.LongHeavyBreath".Localized());

                        OutputText("Scenes.Marielle.TalkFatherHisDeath2.BeginsStillRaspyAgain".Localized());
                    }

                    break;
            }

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.SummonPose".Localized());

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.WaveringPause".Localized());

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.SpreadsLowerArms".Localized());

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.StillAliveWishNot".Localized());

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.SeamstressWaits".Localized());

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.YarnStillInHand".Localized());

            OutputText("Scenes.Marielle.TalkFatherHisDeath2.EndOfStory".Localized());

            View.Time.PassMinutes((int)Round((5 / 4) * 60));

            Talk(() => TalkFatherMenu(4));
        }

        public void TalkFatherNecromancy()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherNecromancy.AsksNecromancyMore".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancy.FoldsManyArms".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancy.BeMoreSpecific".Localized());

            Save.SetInt("talks", Save.GetInt("talks", 0) | TALK_NECROMANCY);

            ClearButtons();

            AddNextButton("Common.Button.HowToLearn".Localized(), TalkFatherNecromancyLearn);

            AddNextButton("Common.Button.Abilities".Localized(), TalkFatherNecromancyAbilities);

            AddNextButton("Common.Button.Forbidden".Localized(), TalkFatherNecromancyForbidden);

            AddButton(14, "Common.Button.Back".Localized(), () => { Talk(); TalkFatherMenu(); });
        }

        public void TalkFatherNecromancyLearn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherNecromancyLearn.HowHeLearned".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyLearn.NoMentorHadHe".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyLearn.WondersBookSource".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyLearn.WellPreservedBooks".Localized());

            View.Time.PassMinutes((int)Round((1 / 4) * 60));

            Talk(() => AddButtonDisabled(0, "How to Learn", "You already asked about that."));
        }

        public void TalkFatherNecromancyAbilities()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherNecromancyAbilities.CuriousAbilities".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyAbilities.RaiseDeceased".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyAbilities.ReplicatedArms".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyAbilities.CreateAnimateFlesh".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyAbilities.RegardsFondly".Localized());

            View.Time.PassMinutes((int)Round((1 / 4) * 60));

            Talk(() => AddButtonDisabled(1, "Abilities", "You already asked about that."));
        }

        public void TalkFatherNecromancyForbidden()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherNecromancyForbidden.WhyProhibited".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyForbidden.HairWrappedFinger".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyForbidden.TisUnnatural".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyForbidden.AdjustsGlasses".Localized());

            OutputText("Scenes.Marielle.TalkFatherNecromancyForbidden.FansBlondeStrands".Localized());

            View.Time.PassMinutes((int)Round((1 / 4) * 60));

            Talk(() => AddButtonDisabled(2, "Forbidden", "You already asked about that."));
        }

        public void TalkFatherLeave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TalkFatherLeave.ThatsAllWanted".Localized());

            TalkMenu();
        }

        public void DoTransaction()
        {

            Player.Gems -= currentPrice;

            Save.SetInt("moneySpent", Save.GetInt("moneySpent", 0) + currentPrice);

        }

        public void SexReturn(string night = "", string late = "", string normal = "", Action returnFunc = null)
        {

            if (View.Time.Hours + View.Time.Minutes / 60 > 22.5f)
            {
                OutputText(night);

                DoNext(PlayerMenu);
            }
            else
                if (View.Time.Hours + View.Time.Minutes / 60 >= Save.GetFloat("visitTime") + 2)
                {
                    OutputText(late);

                    DoNext(PlayerMenu);
                }
                else
                {
                    OutputText(normal);

                    if (returnFunc != null) returnFunc(); else MarielleMenu();
                }
        }

        public void PurchaseMenu(Action yes, Action no, Action model = null)
        {
            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), yes);

            AddNextButton("Common.Button.No".Localized(), no);

            if (model != null)
                AddNextButton("Common.Button.Model".Localized(), model);
        }

        public void Shop(bool output = false)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Marielle.Shop.LookAtGoods".Localized());

                OutputText("Scenes.Marielle.Shop.GetsUpFromStool".Localized());

                OutputText("Scenes.Marielle.Shop.BusiesAtDesk".Localized());
            }

            ClearButtons();

            AddNextButton("Common.Button.Clothes".Localized(), () => ShopClothes(true));

            AddNextButton("Common.Button.Underwear".Localized(), () => ShopUnderwear(true));

            AddNextButton("Common.Button.Commission".Localized(), () => ShopCommission(true));

            AddNextButton("Common.Button.ModelClothes".Localized(), ShopModel);

            AddButton(14, "Common.Button.Back".Localized(), () => ShopBack());
        }

        public void ShopBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBack.RiseUpTell".Localized());

            MarielleMenu();
        }

        public void ShopClothes(bool output = false)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Marielle.ShopClothes.LookThroughTrunk".Localized());

                OutputText("Scenes.Marielle.ShopClothes.MostlyDressmaking".Localized());
            }

            ClearButtons();

            var buttons = new List<(string Label, Action Act)>();

            if (!((Save.GetInt("itemsBought", 0) & BOUGHT_LONG_DRESS) != 0))
                buttons.Add((ItemRegistry.Create(ArmorIds.BallroomDress).ShortNameKey.Localized(), () => ShopLongDress()));

            buttons.Add((ItemRegistry.Create(ArmorIds.MaidDress).ShortNameKey.Localized(), () => ShopMaid()));

            buttons.Add((ItemRegistry.Create(ArmorIds.ButlerSuit).ShortNameKey.Localized(), () => ShopButler()));

            buttons.Add((ItemRegistry.Create(ArmorIds.Kimono).ShortNameKey.Localized(), () => ShopKimono()));

            buttons.Add((ItemRegistry.Create(ArmorIds.ChinaDress).ShortNameKey.Localized(), () => ShopCheongsam()));

            buttons.Add((ItemRegistry.Create(ArmorIds.SummerDress).ShortNameKey.Localized(), () => ShopSummerDress()));

            buttons.Add((ItemRegistry.Create(ArmorIds.Turtleneck).ShortNameKey.Localized(), () => ShopTurtleneck()));

            if (!((Save.GetInt("itemsBought", 0) & BOUGHT_BLACK_CLOAK) != 0))
                buttons.Add((ItemRegistry.Create(ArmorIds.BlackCloak).ShortNameKey.Localized(), () => ShopCloak()));

            if (!((Save.GetInt("itemsBought", 0) & BOUGHT_CHEER_OUTFIT) != 0))
                buttons.Add((ItemRegistry.Create(ArmorIds.CheerOutfit).ShortNameKey.Localized(), () => ShopCheer()));

            if (View.Time.Days >= Save.GetInt("openDate", 0) + 6 && !Save.GetBool("mockedDress"))
                buttons.Add(("Ballet Dress", () => ShopBallet()));

            var shopIndex = 0;

            foreach (var (Label, Act) in buttons)
            {
                AddButton(shopIndex, Label, Act);

                shopIndex++;
            }

            AddButton(14, "Common.Button.Back".Localized(), ShopClothesBack);
        }

        public void ShopClothesBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopClothesBack.ArrangeEverything".Localized());

            Shop();
        }

        public void ShopLongDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLongDress.PullOutDress".Localized());

            OutputText("Scenes.Marielle.ShopLongDress.MadeLongAgo".Localized());

            OutputText("Scenes.Marielle.ShopLongDress.AdjustsPinceNez".Localized());

            OutputText("Scenes.Marielle.ShopLongDress.BuyDressQuestion".Localized());

            currentPrice = 600;

            PurchaseMenu(ShopLongDressYes, ShopLongDressNo, ShopLongDressModel);
        }

        public void ShopLongDressYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLongDressYes.HandsMoney".Localized());

            OutputText("Scenes.Marielle.ShopLongDressYes.TakeMeasurements".Localized());

            OutputText("Scenes.Marielle.ShopLongDressYes.MeasuresQuickly".Localized());

            OutputText("Scenes.Marielle.ShopLongDressYes.HandsTailoredDress".Localized());

            DoTransaction();

            Save.SetInt("itemsBought", Save.GetInt("itemsBought", 0) | BOUGHT_LONG_DRESS);

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BallroomDress), 1);

            ShopClothes();
        }

        public void ShopLongDressNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLongDressNo.PutDressBack".Localized());

            ShopClothes();
        }

        public void ShopLongDressModel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLongDressModel.ConsidersSilently".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModel.WantsPrivacy".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModel.TakesAWhile".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModel.ReadyForBallroom".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModel.ProlongedSilence".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModel.BuyingItQuestion".Localized());

            Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_LONG_DRESS);

            PurchaseMenu(ShopLongDressModelYes, ShopLongDressModelNo);
        }

        public void ShopLongDressModelYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLongDressModelYes.TellsBuying".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModelYes.PleasureCurtsy".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModelYes.MoreThanThat".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModelYes.AughtBeAmiss".Localized());

            Save.SetInt("itemsBought", Save.GetInt("itemsBought", 0) | BOUGHT_LONG_DRESS);

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BallroomDress), 1);

            ShopClothes();
        }

        public void ShopLongDressModelNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLongDressModelNo.SeemsDisappointed".Localized());

            OutputText("Scenes.Marielle.ShopLongDressModelNo.LeaveToRedress".Localized());

            ShopClothes();
        }

        public void ShopKimono()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopKimono.TakeOutRobes".Localized());

            OutputText("Scenes.Marielle.ShopKimono.VestmentDelightsome".Localized());

            OutputText("Scenes.Marielle.ShopKimono.TediousGarment".Localized());

            OutputText("Scenes.Marielle.ShopKimono.FourHundredGems".Localized());

            OutputText("Scenes.Marielle.ShopKimono.BuyKimonoQuestion".Localized());

            currentPrice = 400;

            PurchaseMenu(ShopKimonoYes, ShopKimonoNo, ShopKimonoModel);
        }

        public void ShopKimonoYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopKimonoYes.TakeForPrice".Localized());

            OutputText("Scenes.Marielle.ShopKimonoYes.RequiresMeasurements".Localized());

            OutputText("Scenes.Marielle.ShopKimonoYes.TakesRobesMumbles".Localized());

            OutputText("Scenes.Marielle.ShopKimonoYes.TrueToWord".Localized());

            OutputText("Scenes.Marielle.ShopKimonoYes.RobesAllowVariation".Localized());

            OutputText("Scenes.Marielle.ShopKimonoYes.HandOverGems".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.Kimono), 1);

            ShopClothes();
        }

        public void ShopKimonoNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopKimonoNo.StowsKimono".Localized());

            ShopClothes();
        }

        public void ShopKimonoModel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopKimonoModel.ExpressionDeliberation".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.SpreadsRobesDemo".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.SlipsHeavierPiece".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.TwillRequirePractice".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.LengthyProcess".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.LacksMirror".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.WhatThinkYou".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModel.BuyKimonoModelQuestion".Localized());

            Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_KIMONO);

            PurchaseMenu(ShopKimonoModelYes, ShopKimonoModelNo);
        }

        public void ShopKimonoModelYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopKimonoModelYes.ThinksWillTake".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModelYes.NeedsAdjustments".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModelYes.FinishedKimono".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModelYes.HopeBySatisfaction".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModelYes.ThankAndPay".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.Kimono), 1);

            ShopClothes();
        }

        public void ShopKimonoModelNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopKimonoModelNo.AsYouWill".Localized());

            OutputText("Scenes.Marielle.ShopKimonoModelNo.LookSelectionAgain".Localized());

            ShopClothes();
        }

        public void ShopMaid()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaid.SpotMaidDress".Localized());

            OutputText("Scenes.Marielle.ShopMaid.SewnInProfusion".Localized());

            OutputText("Scenes.Marielle.ShopMaid.LiveryPartParcel".Localized());

            OutputText("Scenes.Marielle.ShopMaid.BuyOutfitQuestion".Localized());

            currentPrice = 450;

            PurchaseMenu(ShopMaidYes, ShopMaidNo, ShopMaidModel);
        }

        public void ShopMaidYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidYes.HandsGems".Localized());

            OutputText("Scenes.Marielle.ShopMaidYes.HoldsStillTailor".Localized());

            OutputText("Scenes.Marielle.ShopMaidYes.SatisfiedHmm".Localized());

            OutputText("Scenes.Marielle.ShopMaidYes.AllDonePrayTell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.MaidDress), 1);

            ShopClothes();
        }

        public void ShopMaidNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidNo.FoldsDressBack".Localized());

            ShopClothes();
        }

        public void ShopMaidModel()
        {
            ClearOutput();

            if ((Save.GetInt("modeled", 0) & MODELED_MAID_DRESS) != 0)
                OutputText("Scenes.Marielle.ShopMaidModel.MaidservantBeMe".Localized());
            else
            {
                OutputText("Scenes.Marielle.ShopMaidModel.FrownsFloridBlush".Localized());

                OutputText("Scenes.Marielle.ShopMaidModel.HabitNotIndignity".Localized());
            }

            OutputText("Scenes.Marielle.ShopMaidModel.GingerlyTakesDress".Localized());

            OutputText("Scenes.Marielle.ShopMaidModel.SlowTwirlAsks".Localized());

            OutputText("Scenes.Marielle.ShopMaidModel.SuitsHerBetter".Localized());

            OutputText("Scenes.Marielle.ShopMaidModel.BuyMaidQuestion".Localized());

            Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_MAID_DRESS);

            PurchaseMenu(ShopMaidModelYes, ShopMaidModelNo);

            AddNextButton("Common.Button.Sex".Localized(), ShopMaidSex);
        }

        public void ShopMaidModelYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidModelYes.HandsGemsSmile".Localized());

            OutputText("Scenes.Marielle.ShopMaidModelYes.EagerlyShimmiesOut".Localized());

            OutputText("Scenes.Marielle.ShopMaidModelYes.BeckonsAdjustedDress".Localized());

            OutputText("Scenes.Marielle.ShopMaidModelYes.AllOvertightTell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.MaidDress), 1);

            ShopClothes();
        }

        public void ShopMaidModelNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidModelNo.DisappointedMoment".Localized());

            OutputText("Scenes.Marielle.ShopMaidModelNo.ChangesBackLook".Localized());

            ShopClothes();
        }

        public void ShopMaidSex()
        {

            ClearButtons();

            AddNextButton("Common.Button.GetFingered".Localized(), ShopMaidFingered);

            AddNextButton("Common.Button.Handjob".Localized(), ShopMaidHandjob);

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => ShopMaidModel());
        }

        public void ShopMaidFingered()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidFingered.FantasticIdea".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.WhWhatDoing".Localized((Player.Femininity >= 59 && !Player.HasCock() ? "U-" : "")));

            OutputText("Scenes.Marielle.ShopMaidFingered.LeadTwoHands".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.TrailsOffEyes".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.TerriblySorryPardon".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.DragsStoolKneels".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.StopBeforeContinue".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.LowerPairDigs".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.MayIMaster".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.SlightestNodPress".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.GaspClitGraze".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.BeauteousSheSays".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.ClampDownCold".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.BackInDeliberately".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingered.TisButDuty".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.ShopMaidFingered.PainfullyHard".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.CantHoldLonger".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.UtterlySpentFlop".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.StrengthReturnsMaid".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.CatchesClearsThroat".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.GetUpLookOver".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.BuyAfterFingering".Localized());

                Player.Orgasm("All");

                View.Time.PassMinutes((int)Round((3 / 4) * 60));

                PurchaseMenu(ShopMaidFingeredYes, ShopMaidFingeredNo);
            }
            else
            {
                OutputText("Scenes.Marielle.ShopMaidFingered.TrioHandsPassion".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered.FlushedScarletInches".Localized());

                ClearButtons();

                AddNextButton("Common.Button.LetHer".Localized(), () => ShopMaidFingered2(false));

                AddNextButton("Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), () => ShopMaidFingered2(true));
            }
        }

        public void ShopMaidFingered2(bool stopped)
        {
            ClearOutput();

            if (stopped)
            {
                OutputText("Scenes.Marielle.ShopMaidFingered2.NudgeOrScold".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.FranticGirlRightens".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.RewardForWorking".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.OfCourseAnon".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.DeterminedPicksUp".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.AddedPleasureLimit".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.UtterlySpentTable".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.StrengthReturnsMaid".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.ImaginaryTailWags".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.GetUpLookSpotless".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.BuyAfterSelfPlay".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopMaidFingered2.WatchingTurnsOn".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.ArchingEcstaticBliss".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.GaspingClimbDown".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.ExtractsHerself".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.ScramblesAttention".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.PetersOffCloth".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.SpotlessSkirt".Localized());

                OutputText("Scenes.Marielle.ShopMaidFingered2.BuyAfterSecondPlay".Localized());
            }

            Player.Orgasm("Vaginal");

            MarielleSex();

            View.Time.PassMinutes((int)Round((1) * 60));

            PurchaseMenu(ShopMaidFingeredYes, ShopMaidFingeredNo);
        }

        public void ShopMaidFingeredYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidFingeredYes.FishForGems".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingeredYes.CatchesSwiftness".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingeredYes.LanguidlyWash".Localized());

            OutputText("Scenes.Marielle.ShopMaidFingeredYes.AllInOrderFit".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.MaidDress), 1);

            SexReturn("[pg]Looks to be fitting, but it's getting late now, as evidenced by a pair of rather heavy eyelids in front of you, so you share your farewells and take on the journey back to camp.\", \"[pg]Looks to be fitting, but you've been spending quite some time here now, so you think you should be checking on your camp. After you share your farewells, you take on the journey back through the brackish swamp.\", \"\", shopClothes.bind()));\n    }\n\n    public function shopMaidFingeredNo() {\n        clearOutput();\n        sexReturn(\"It's getting late, so you stretch yourself and [if (!isnakedlower) {re-dress|crack your neck}], then say your farewell to Marielle. Before you take on the journey back to camp, you briefly wash yourself by a small puddle and glance back towards a girl who's already readying her bedroll.\", \"You've been spending quite some time here now, so you think you should be checking on your camp. After you share your farewells, you briefly wash yourself off and take on the journey back through the brackish swamp.\", \"You stretch yourself and [if (!isnakedlower) {re-dress|crack your neck}], then go to wash yourself while you let Marielle freshen up and change clothes before you take another look at her assortment.\", shopClothes.bind());\n    }\n\n    public function shopMaidHandjob() {\n        clearOutput();\n        outputText(\"Seeing her in this outfit, you know what you want her to do. You [if (!isnakedlower) {get your [armor] out of the way, exposing your crotch to the elements|toss your baggage aside}] and stride closer, eliciting a surprised step back from the seamstress.\");\n        outputText(\"[pg][say:Ah! Uhm... [name]? What are you doing?]\");\n        outputText(\"[pg]You sit down on her desk, careful not to skewer yourself on any needles, and tell your little servant to get to work. Marielle looks at you, clearly confused, and you patiently watch the gears in her head working.\");\n        outputText(\"[pg][say:Pardon m-- oh! Oh. I...] She glances at your [cock], finally understanding your intention. [say:Oh, uhm...] She doesn't seem too excited, though; you quietly wonder if she would turn you down. [say:Must we forsoo-- I mean, if 'tis not... I... I, uhm...] A long breath, not quite dispelling a look of aversion that she diverts from you down to her current livery.\");\n        outputText(\"[pg][say:Yes. Yes, of course... [name].] Even as soft as Marielle's voice is, it's bleeding \" + (saveContent.timesDicked != 0 ? \"both distaste and wariness\" : \"distaste\") + \", though it seems she wouldn't refuse a simple handjob, after all. Although, '[name]'? She should address you properly, if you're doing this.[if (!hasvagina) { At that, she nearly scowls at you, but apparently decides not to say what's on her tongue before letting go of a sigh.}]\");\n        outputText(\"[pg][say:...Yes, [Master], pray forgive me.] Marielle curtly bows to you and steps closer. [say:Let me, ah... make apology to you, [Master].]\");\n        outputText(\"[pg]Her touch is chilly and almost makes you flinch as her hand rests on your hips. She kneels down in front of you, regarding you through her spectacles with equal parts hesitation and concentration. As her hand inches closer to its target, a second one joins in, cool fingers tickling you until they both reach the base and wrap around your length.\");\n        outputText(\"[pg]Marielle leans in, her lower arms finding support on your thighs, and starts to work her thumbs across the underside of your dick. Despite the unnatural chill of her skin against yours, you quickly grow hard under her massaging fingers, helped by the loose maid's dress giving you an easy view down the girl's small chest, pale nipples only sporadically obstructed by swaying strands of golden hair. She studies your face with an almost piercing stare, trying to gauge your reactions and adding more pressure each time her cold, narrow digits pass over sensitive spots. Before long, your [cock] stands at full mast.\");\n        outputText(\"[pg]There, she stops, her steel gaze evaluating your erect length as she tilts herself back a little. Apparently having decided on how to go on about this, she dips a third hand low to [if (hasvagina) {tease the lips of your neglected cunt|[if (hasballs) {cradle your [balls], massaging your needy orbs|massage the base of your dick, dipping even lower and over your taint}]}] as she looks into your eyes and resumes her previous handiwork with increased intensity.\");\n        outputText(\"[pg]Their motions measured, her hands slide up your length, collect pre-cum from your tingling crown, then slip down again with the added lubrication. Her movements seem inhuman, calculated, almost mechanical, the frigidity of Marielle's body adding to that impression, but the light breath occasionally brushing against you feels anything but. You lean back, content to let the seamstress-turned-maid jerk you off. Your heartbeat quickens as she steadily ups the tempo, pleasure rising, wet squelching sounds echoing through the old temple.\");\n        outputText(\"[pg]You are getting close under her triple assault. Through ragged breaths, you tell her as much.\");\n        outputText(\"[pg][say:Huh?] She glances up at you. [say:Oh, ah, yes.] [if (hasvagina) {Nodding briefly|Shuffling away}] and knitting her brows, she then clamps down with both hands and pumps your length with surprising recklessness. Her fourth one finally leaves your tensed-up thigh and moves to gingerly caress your glans in stark contrast to her vice-tight grip below.\");\n        outputText(\"[pg]The quartet of sensations--her strong double-grasp, the slightly awkward massage of your tip, the fingers kneading your [if (hasvagina) {engorged lower lips|[if (hasballs) {churning balls|taint}]}], and her increasingly strained puffs of breath against your sensitive skin--soon has you clinging onto the edge. But your small maid shows no mercy, intent on casting you off the cliff. You grit your teeth and let go, throwing yourself to the waves.\");\n        outputText(\"[pg]Groaning in pleasure, you hear Marielle yelp as the first spurt of cum hits her right on the forehead, and she gracelessly fails to dodge the second one that quickly coats her glasses, robbing her of sight as sparks dance across your vision. Faltering, she regardless works you through your shuddering orgasm, the hot, electric passion racing through your veins amplified by her continuous, if now erratic, motions.\");\n        outputText(\"[pg]You [if (cumquantity > 450) {eventually|soon}] wind down, much to the relief of the blind, sputtering seamstress, your [cock] slowly softening in her warmed-up grasp.\");\n        outputText(\"[pg][say:Hrmmgh,] Marielle grumbles, all to eager to abandon you and take off her soiled spectacles. [say:Needed it truly be the glasses...]\");\n        outputText(\"[pg]She looks up at you, her irritated face forcing out a wholly unsuccessful smile. [say:I... hope 'twas to your pleasure, [Master].] You tell her it was; she sure has a way with her hands.\");\n        outputText(\"[pg]Looking her over--squinting and reaching for a cloth to try and clean your spunk off her face--you notice she miraculously managed to keep the dress spotless. With expertise like that, maybe she should become a bona fide maid. You keep that thought to yourself, though.\");\n        outputText(\"[pg]Do you want to buy it?\");\n        player.orgasm('Dick');\n        marielleSex();\n        cheatTime(.75);\n        purchaseMenu(shopMaidHandjobYes, shopMaidHandjobNo);\n    }\n\n    public function shopMaidHandjobYes() {\n        clearOutput();\n        outputText(\"You think you'll buy that outfit.\");\n        outputText(\"[pg][say:Hmm?] Marielle looks up from her cleaning. [say:Oh. Oh! Ah, yes, of course.] She briefly checks the fabric for stains, then puts aside her glasses and picks up two measuring tapes.\");\n        outputText(\"[pg][say:I will, ah, need but a moment to adjust the dress for you... [Master],] she adds with a slight bow and quickly takes your sizes, narrowing her eyes to read the numbers before leaving to properly wash her face off.\");\n        outputText(\"[pg]You [if (isnakedlower) {watch her }]re-dress, then let her get to work. It doesn't take her long to finish and wave you back.\");\n        outputText(\"[pg][say:Well, all is done, 'twill certainly fit you now,] she says, exchanging the dress for a bundle of gems. [say:Though, pray tell, should aught be amiss therewith.]\");\n        doTransaction();\n        inventory.takeItem(armors.M_DRESS, sexReturn.bind(\"[pg]Looks good, but it's getting late now, so you share your farewells with the seamstress and take on the journey through the brackish swamp back to your camp.\", \"[pg]Looks good, but you've been spending quite some time here now, so you think you should be getting back to camp. After sharing your farewells, you take on the journey back through the brackish swamp.\", \"\", shopClothes.bind()));\n    }\n\n    public function shopMaidHandjobNo() {\n        clearOutput();\n        sexReturn(\"It's getting late, so you say your farewells to the seamstress before you leave her to re-dress and wash herself off as you take on the journey back to your camp.\", \"You've been spending quite some time here now, so you think you should be getting back to camp. After sharing your farewells, you leave Marielle to wash herself off and take on the journey back through the brackish swamp.\", \"You climb down from the desk, arranging your [armor] again, and leave Marielle to re-dress and wash herself off while you take another look at her wares.\", shopClothes.bind());\n    }\n\n    public function shopButler() {\n        clearOutput();\n        outputText(\"You notice several pieces that you think belong together: a white shirt, a vest, suit pants, and a tailcoat.\");\n        outputText(\"[pg][say:Oh, yes,] Marielle chimes in. [say:Nighly had I forgotten those were yet by my possession...] She takes a cursory glance over what you pulled out, then tacitly indicates in succession a pair of white silk gloves, some suspenders, as well as a bow- and necktie.\");\n        outputText(\"[pg][say:Men's garments are no, ah, speciality of mine, yet once custom-tailored I a few for a client erstwhile who then... vanished thenceafter, with not a word nor note behind. As if into the very air. And never learned I what befell the gentleman.] She adjusts her pince-nez. [say:Well, be that as it may. 'Tis meant for a manservant--a butler--but one may wear it unto any occasion, of import or not. I doubt any one soul here in Mareth would mark the difference...]\");\n        outputText(\"[pg]The suit does indeed look like it could be worn by just about anyone wanting to look fancy.\");\n        outputText(\"[pg][say:Hmm... 580 gems for the entire livery is what I can offer you.]\");\n        outputText(\"[pg]Do you take it?\");\n        currentPrice = 580;\n        purchaseMenu(shopButlerYes, shopButlerNo, shopButlerModel);\n    }\n\n    public function shopButlerYes() {\n        clearOutput();\n        outputText(\"[say:Oh, marvellous,] Marielle says with a smile as you offer her the gems, [say:but I will needs make a few adjustments for you.] She pulls out two measuring tapes and quickly notes your sizes before taking the suit from you and sitting down at her desk.\");\n        outputText(\"[pg]After a while, she hums a contented [say:Hmmn] and gets up to return the pieces to you.\");\n        outputText(\"[pg][say:Well, 'bespoke' is what it should be indeed, now. But pray tell me if aught be... pinching you.]\");\n        doTransaction();\n        inventory.takeItem(armors.BUTSUIT, shopClothes.bind());\n    }\n\n    public function shopButlerNo() {\n        clearOutput();\n        outputText(\"You fold it neatly before putting the pieces back into the chest.\");\n        shopClothes();\n    }\n\n    public function shopButlerModel() {\n        clearOutput();\n        if ((saveContent.modeled & MODELED_BUTLER_SUIT) != 0) {\n            outputText(\"[say:Yet again?] Marielle frowns, evidently not too keen on modelling the suit.\");\n        } else {\n            outputText(\"Marielle blinks at you, confused.\");\n            outputText(\"[pg][say:You wish me don... [b:this]?] she asks, and you answer in the affirmative. [say:...Man I am none, neither gentle nor base--that slipped your mind not, yes?] You know, but you still want her to try it on. A sigh escapes her at that.\");\n        }\n        outputText(\"[pg][say:Well... if that be what you wish...] She looks hesitant, but still takes the clothes from you and turns around to change. Her motions are quick, letting you catch only a glimpse of her naked butt before she puts on and fastens the pants.\");\n        outputText(\"[pg]When she turns around, you find it's a surprisingly good fit on her. It does straighten out her figure a bit further, granting the girl a strain of androgyny, but conversely puts the appeal of her fair face, blonde hair, and genteel posture into an even brighter limelight. Though still, the outfit's a tad large; the tailcoat's flaps reach past her knees, and those pants are too slack. And somehow, she managed to squeeze all four arms through the sleeves. She also went for the necktie, not the bow. Maybe she's actually aware of how well it suits her. All in all, it looks as elegant and tasteful as you expected.\");\n        outputText(\"[pg][say:Well? What think you?]\");\n        outputText(\"[pg]What do you think indeed; are you buying it?\");\n        saveContent.modeled |= MODELED_BUTLER_SUIT;\n        purchaseMenu(shopButlerModelYes, shopButlerModelNo);\n    }\n\n    public function shopButlerModelYes() {\n        clearOutput();\n        outputText(\"You'll buy this one.\");\n        outputText(\"[pg][say:Ah, marvellous!] Marielle exclaims, already in the process of changing again. She pulls out a pair of measuring tapes when she's done and says, [say:A couple of, ah, alterations 'twill require, but I will do so apace. Please you, keep still for but a moment...] She's quick to note down your sizes before getting to work.\");\n        outputText(\"[pg][say:By and by. I shall call, once 'tis finished,] the seamstress tells you, leaving you to wander the temple for a bit.\");\n        outputText(\"[pg]As promised, you soon hear her voice beckon you over to her tent again, where she presents you the tailored suit, neatly folded in her arms. You take it and hand over the money in turn after a brief check.\");\n        outputText(\"[pg][say:Ah, give note if aught be... overtight anywhere.]\");\n        doTransaction();\n        inventory.takeItem(armors.BUTSUIT, shopClothes.bind());\n    }\n\n    public function shopButlerModelNo() {\n        clearOutput();\n        outputText(\"[say:Hmm, I see...] she sighs and quickly changes back to her usual summer dress.\");\n        outputText(\"[pg][say:Well, if you chance upon a change of heart, I believe these ones, ah... will depart from my selection not any time soon.]\");\n        shopClothes();\n    }\n\n    public function shopCheongsam() {\n        clearOutput();\n        outputText(\"Your attention catches on something smooth and silky, and you fish it out of the chest.\");\n        outputText(\"[pg]A long dress, appearing to be form-fitting, with a strange, elaborate motif running across the lustrous fabric. Its skirt has two slits that would bare the legs from the sides. It looks foreign, and although Marielle is watching you and likely knows more, it seems she isn't going to say anything about this particular garment, so you ask her.\");\n        outputText(\"[pg][say:Ah, well,] she begins, pausing briefly to pick her words and fold her hands, [say:'tis of a land quite faraway indeed, aye. A... 'cheongsam', I believe is its name, though pray pardon my, ah, pronunciation.] Wondering how a dress with a name like that made it all the way over here, you ask if she's been to that land in question.\");\n        outputText(\"[pg][say:Oh! Oh, no. Alas, no.] One hand idly rises to stroke the lock of hair that was about to fall in front of her face. [say:But fortune would have it I once bechanced upon one who did... and she was quite fain to allow me view her travel memoir and illustrations therein... 'Twas positively enlightening.] The reminiscing smile on her lips lasts only a moment longer before she flicks away her hair and shifts the topic back to the dress itself.\");\n        outputText(\"[pg][say:But, well, 'tis a, ah... multifarious garb, befitting nighly all walks of life, though most oft intended for a more... formal occasion.] This one does look pretty classy and luxurious, you remark as you take a closer look at the images stitched into the fabric.\");\n        outputText(\"[pg][say:...Thank you. I do rather joy in purfling and broidery.] Shifting on her seat, she squeezes her hands. [say:So... should you find it suitably pleasing, then it may be yours for, ah, 400 gems.]\");\n        outputText(\"[pg]Do you want to buy the exotic dress?\");\n        currentPrice = 400;\n        purchaseMenu(shopCheongsamYes, shopCheongsamNo, shopCheongsamModel);\n    }\n\n    public function shopCheongsamYes() {\n        clearOutput();\n        outputText(\"You hand Marielle the money.\");\n        outputText(\"[pg][say:Ah! Yes, just... hold still awhile, I need take your, ah, measurements first.] She grabs two long tapes from her desk and quickly notes down your sizes with them before taking the dress and sitting down to work on it.\");\n        outputText(\"[pg][say:Ah, it shall be not overlong. A minute, mayhap.]\");\n        outputText(\"[pg]It turns out to be a little longer, but not by too much. She soon waves you back over, the tailored dress in her arms and a smile on her face.\");\n        outputText(\"[pg][say:All should be done. Howbeit, pray tell me if aught be amiss therewith.]\");\n        doTransaction();\n        inventory.takeItem(armors.CHNGSAM, shopClothes.bind());\n    }\n\n    public function shopCheongsamNo() {\n        clearOutput();\n        outputText(\"You fold the silken dress and put it back into the chest.\");\n        shopClothes();\n    }\n\n    public function shopCheongsamModel() {\n        clearOutput();\n        outputText(\"[say:I am]--the girl hesitates when you hold the garment out to her--[say:rather ill-befitted for such, ah, strange and seasoned gowns... yet if your wish be truly I demonstrate this upon mine so unripened body...] [if (femininity >= 59) {Her pale face suddenly reddens, and she refrains from saying anything further than that as she|She frowns at you, but nonetheless}] takes the dress from your hands and swivels around to change.\");\n        outputText(\"[pg]She's swift to do so, giving you only a passing glimpse of her back before slipping into the dress, setting her many arms to pull the fabric taut where needed, and finally turning to face you. Though there, under your scrutinizing gaze, she shuffles in unease.\");\n        outputText(\"[pg][say:...[if (femininity >= 59) {W-}]Well?]\");\n        outputText(\"[pg]Despite her efforts, it's still a little loose on her thin frame, and you don't think the skirt's hem is supposed to be actually grazing the ground, but the body-hugging style does make even someone like Marielle look almost curvaceous. Elegant it is, the embroidered patterns accentuating just the right places, and through the slits on the side, you can see the pale white of her legs peeking through in a surprisingly enticing fashion. She fidgets again as she notices where your gaze has wandered.\");\n        outputText(\"[pg]Are you buying the dress?\");\n        saveContent.modeled |= MODELED_CHINA_DRESS;\n        purchaseMenu(shopCheongsamModelYes, shopCheongsamModelNo);\n        addNextButton(\"Sex\", shopCheongsamSex).disableIf(!((saveContent.talks & TALK_SEX) != 0)).sexButton(BaseContent.ANYGENDER);\n    }\n\n    public function shopCheongsamModelYes() {\n        clearOutput();\n        outputText(\"You'll take this one.\");\n        outputText(\"[pg][say:Ah, uhm, certainly.] Marielle dips into a small curtsy before taking the gems you offer her. [say:I will, ah, needs make some adjustments, though.] She grabs a pair of measuring tapes from the table and swiftly takes your sizes.\");\n        outputText(\"[pg][say:Hmm, this shall take a while... but not overlong, I warrant.]\");\n        outputText(\"[pg]True to her word, she soon beckons your over again and presents you the tailored dress.\");\n        outputText(\"[pg][say:Well, that should be all. If it be, ah, uncomfortable anywhere, pray tell me.]\");\n        doTransaction();\n        inventory.takeItem(armors.CHNGSAM, shopClothes.bind());\n    }\n\n    public function shopCheongsamModelNo() {\n        clearOutput();\n        outputText(\"[say:Oh. Well, that is... Pray tell if there be aught else you would like,] Marielle says with a slight frown, though she quickly banishes it from her face.\");\n        outputText(\"[pg]While she changes back into her usual dress, you take another look at her collection.\");\n        shopClothes();\n    }\n\n    public function shopCheongsamSex() {\n        menu();\n        addNextButton(\"Footrub\", shopCheongsamFootrub).hint(\"Worship her legs and let her get you off with her feet.\").sexButton(BaseContent.FEMALE);\n        addNextButton(\"Footjob\", shopCheongsamFootjob).hint(\"Have her use those feet of hers on your dick.\").sexButton(BaseContent.MALE);\n        setExitButton(\"Nevermind\", shopCheongsamModel);\n    }\n\n    public function shopCheongsamFootrub() {\n        clearOutput();\n        outputText(\"The alabaster line of skin from Marielle's thigh down to the ground sparks a latent desire within you. Her exposed legs, scarred as they are, just beg to be touched, to be worshipped.\");\n        outputText(\"[pg][say:[name]? Are you unwell?] she asks as you get onto your knees before her. You reach out and slide your hand behind her leg, eliciting a surprised yelp from her. [say:Wh-What are you doing? What is the ma--] She cuts into an involuntary half-gasp when you plant your lips just below her knee and brush your hand upwards. Her flesh is cold, but the way she twitches under your servile touch has you burning with passion. You leave a wet trail in your lips' wake as they travel upwards in a serpentine line, towards areas far more sensitive. A second hand joins in, gently tickling her calf while the other slips underneath the satin fabric to cup the girl's pert butt.\");\n        outputText(\"[pg]She fails to suppress a shudder and stumbles backwards, but since you're hugging her leg, she doesn't fall, and manages to catch herself on the table's edge, ivory fingers finding purchase on the wood as you grope and kiss her. You cast a glance upwards. Her face is flushed carmine, eyes [if (hascock) {staring back at you with timid passivity as she lets you spoil her before they escape into the temple|drifting closed as she starts to properly enjoy your touch}].\");\n        outputText(\"[pg]But you want more; you want to be closer. [if (!isnakedupper) {Fingers working swiftly, you loosen [if (hasarmor) {the upper part of your [armor]|your [uppergarment]}], press yourself against her,|Filled with desire, you press yourself against her}] and slide [if (biggesttitsize < 3) {your chest over her cold limb|her cold limb between your breasts}]. At the contact of your soft, naked flesh, she [if (hascock) {almost inaudibly|sucks in a breath through her teeth and}] sighs. You can't see it behind the veil of silk, but her womanhood, so close to your roving hand, must be [if (hascock) {getting ready|overflowing}]. You could easily touch her, sink your fingers deep into her, and send her off, but you can't get carried away now--your own [vagina], engorged with need, reminds you of that. Against all temptation, you wind your ministrations down until they come to a stop.\");\n        outputText(\"[pg][say:Haah... [name]? Wherefore are you...] She finally [if (hascock) {turns her eyes back to you|opens her eyes again}] and catches sight of you [if (!isnakedlower) {shedding [if (hasarmor) {the rest of your [if (lightarmor) {clothing|armor}]|your [undergarment]}] until you're kneeling entirely naked before her.|idly fingering yourself, meeting her gaze head-on.}]\");\n        outputText(\"[pg]She [if (hascock) {surveys|ogles}] your naked body with [if (hascock) {uncertainty as her gaze falls onto your [cock]. Easing her worries, you inform her it doesn't have to play a role in what is to come, and, running|an unmistakeable longing in her eyes--her kindled mind is probably imagining you in the most compromising positions. And you're planning to indulge those fantasies fully. Running}] a hand over your chest while putting on your best playfully submissive smile, you voice to her what you want the undead girl to do.\");\n        if (saveContent.footstuff) {\n            outputText(\"[pg]Marielle evidently understood you, but she doesn't move, perhaps not knowing what to say, so you take it into your own hands, lean back, and [if (tailLeg) {slide your tail out from under you|spread your thighs wide}] to present your glistening slit and reassure the hesitant seamstress. She [if (hascock) {regards you with a thoughtful frown for a second longer before she takes|seems entranced for a second, unconsciously licking her upper lip at your lascivious display, before she remembers to take}] off her sandals.\");\n            outputText(\"[pg][say:Well, uhm, if you desire I... do this once more...]\");\n        } else {\n            outputText(\"[pg]Marielle stares at you, her mind working to process your words.\");\n            outputText(\"[pg][say:[if (!hascock) {Y-}]You wish me to... to...] Her eyes widen in comprehension. [say:[if (hascock) {Would that be truly all right? |May I? M-May I, truly? I-}]I mean, I ill-wish to... hurt you.] You lean back, [if (tailLeg) {slide your tail out from under you|spread your thighs wide}] to present your glistening slit, and reassure the reluctant seamstress.\");\n            outputText(\"[pg]She fidgets nervously, but then does slide her sandals off. [say:Well, if this be no let to you...]\");\n        }\n        outputText(\"[pg]She raises her leg, and you are met with frigid, delicate toes pressing onto your clavicle. You offer no resistance and let her slowly push you down until you lie supine on your back before your petite mistress, her toenails resting dangerously close to your throat. Her face is a difficult to discern mix of conflicting thoughts as she takes in your vulnerable form, but steeling herself with a faint [if (hascock) {nod|swallow}], she starts to gingerly but with increasing steadiness move down over your naked, aching body.\");\n        outputText(\"[pg]You shudder as she grazes [if (isflat) {over your flat chest, stopping momentarily to probe a stiffening nipple.|against your [chest]. [say:Such softness...] she whispers to herself as she nudges your mammaries and circles your stiffened teats.}] Her dainty foot then continues its journey downwards, over your navel[if (hascock) {, swerves around your erect member}], and closes in on your awaiting cunt. As her cool digits brush the outer folds of your vagina, you tense up and draw a sharp breath. You hadn't realized you were this close to the edge already. Taking note of your struggle, Marielle lightly taps your [clit] before lifting her foot away, provoking a needy mewl from you. The resulting smile on her lips is almost disconcerting, her glacier-like eyes locked onto yours.\");\n        outputText(\"[pg][say:Beg.]\");\n        outputText(\"[pg]The command is clear and issued with chilling gravity, making you tremble in excitement. With her foot hovering mere inches from your sweltering slit and her ice-cold gaze narrowed behind her glasses, you beg her. You beg her to use you, step on you, abuse you, and make you cum like an animal. Her facade briefly cracks under your bold words, but she catches herself and smirks at you.\");\n        outputText(\"[pg][say:My, what a servant most exemplary you are... Mayhap \" + (player.femininity >= 59 && !player.hasCock() ? \"I ought make you verily mine\" : \"that be your true calling\") + \", [name].]\");\n        outputText(\"[pg]Your vision blurs as she descends on you again, finally granting you what you crave. Her cool sole slides over your mons, setting your nerves alight, presses down on your clitoris, then slides downwards again to let your button twitch in forlorn solitude. She repeats the motion, each time with more intensity, each time spreading more of your arousal over your tingling nethers and bringing you closer to the brink you pulled yourself back from just moments ago. You shiver when her big toe plunges between, sliding effortlessly through its wet sheath. Angling it, she then pushes in deeper and stirs your insides with quick, deliberate twirls before flicking out of you again and resuming the sensual rubbing.\");\n        outputText(\"[pg][say:Restrain yourself not, [name], the reason oversets itself,] Marielle [if (hascock) {states, taking a breath|purrs, swallowing}]. [say:Go[if (!hascock) {, haah}]... go on, I command you...]\");\n        marielleSex();\n        if (player.hasCock()) {\n            outputText(\"[pg]You're panting and moaning, and your mind is flush with desire and eager to obey her as her slick, slim sole works you with merciless, electrifying intensity. She leans forwards, trailing around your labia, then bears down on your [clit] with focussed precision. Her entire foot pins you against the ground and assaults your overworked nerves like a living vibrator, and you are swiftly kicked off the ledge.\");\n            outputText(\"[pg]Your hips rise and nearly topple Marielle over as your vision turns to white, your [cock] erupting across your chest while searing-hot ecstasy ripples through your veins. You let out a long moan, your [vagina] smearing your juices liberally across your dominatrix's heel. A heel that's relentlessly grinding into your nethers as the stars rise before your eyes.\");\n            outputText(\"[pg]The pressure lifts off your crotch as you finally come down from your high, breathing heavily in the afterglow. You open your eyes again, the world still blurry, and try to prop yourself up. Marielle is sitting on her stool once more, idly wiping her foot, a cloth in hand, and watching you with a look your muddy vision can't decipher until you slowly find the strength to stand. There's still some redness on her cheeks, but she appears collected, though perhaps worried.\");\n            outputText(\"[pg][say:Are you... all right, [name]?] she asks, and you assure her you are. [say:I see.] Relief and a slight nod. [say:I... I expected this not to be quite so enjoyable. 'Twas... interesting.] Her head turns away as she hangs the cloth over the broken wall.\");\n            outputText(\"[pg]The oriental dress itself seems to have been spared any kind of stains, remaining as smooth and spotless as before. You can definitely still buy it like this.\");\n            outputText(\"[pg]Do you want to?\");\n            cheatTime(.75);\n            player.orgasm('All');\n            purchaseMenu(shopCheongsamFootrubYes, shopCheongsamFootNo);\n        } else {\n            outputText(\"[pg]You're panting and moaning, your mind is flush with desire, and so is she. Her face is the picture of arousal, and she studies your writhing body intently as several of her hands unconsciously roam over her long, oriental dress.\");\n            menu();\n            addNextButton(\"Stay Put\", shopCheongsamFootrubStay).hint(\"Be a good servant and let your mistress do as she wishes.\");\n            addNextButton(\"Help Her Out\", shopCheongsamFootrubHelp).hint(\"Be a better servant and give your mistress what she needs.\");\n        }\n    }\n\n    public function shopCheongsamFootrubStay() {\n        clearOutput();\n        outputText(\"You're close now, her slick, slim sole working with merciless, electrifying intensity. She knows it well and leans forwards, trailing around your labia, then bears down upon your [clit] with focussed precision. Her entire foot pins you against the ground and assaults your overworked nerves like a living vibrator, and you're swiftly kicked off the ledge.\");\n        outputText(\"[pg]Your hips rise and nearly topple Marielle over as your vision turns to white. Searing-hot ecstasy ripples through your veins, and you let out a long moan, your [vagina] smearing your juices liberally across your dominatrix's heel. A heel that's relentlessly grinding into your nethers as the stars rise before your eyes.\");\n        outputText(\"[pg]The pressure lifts off your crotch as you finally come down from your high, breathing heavily in the afterglow. You open your eyes again, vision blurry, and nearly recoil as something bumps your nose; something wet and smelling distinctly of your own arousal. As the world regains its clarity, you see Marielle glowering down at you, four arms crossed in front of her chest. She's flushed and panting behind that barely taped-together mask, but still manages to put on an icy, authoritarian tone:\");\n        outputText(\"[pg][say:Cleanse me.]\");\n        player.orgasm('Vaginal');\n        menu();\n        addNextButton(\"Lick\", shopCheongsamFootrubStay2.bind(true)).hint(\"Do as you're told.\");\n        addNextButton(\"Refuse\", shopCheongsamFootrubStay2.bind(false)).hint(\"You're not really into that.\");\n    }\n\n    public function shopCheongsamFootrubStay2(licked:Bool) {\n        clearOutput();\n        if (licked) {\n            outputText(\"There was no need for her to even tell you that. Eagerly, you extend your [tongue] towards the delicate foot before your face. The taste of yourself, your own lewd secretions, is the most prominent--a testament to the release that's still keeping your mind afloat on little bubbles of joy. You start to swivel around her toes, dive into the gaps, then come up again only to renew your efforts. You lap it all up, toe after toe, and as your own juices dwindle, Marielle's flavor comes to light; she does not sweat, but her undead skin does have a strange note to it. Cupping her delicate foot in your hands, you run your tongue all along her sole, back and forth, making your cold mistress shiver in delight.\");\n            outputText(\"[pg]When you momentarily break away to cast a subservient glance up at her, you note that one of her hands has disappeared underneath her satin gown's skirt, its motions silent but quite obvious.\");\n            outputText(\"[pg][say:Haah, proceed, [name]... Not yet have I... permitted thee to stop,] Marielle half-whispers with lidded eyes leering at your naked, submissive form. Eager to please, you decide to give a show to send her off.\");\n            outputText(\"[pg]Languidly trailing kisses all the way back from her heel, you maintain eye contact as you then draw your tongue over the roof of her foot while you gently caress her ankle with one hand and slide the other up her calf to give it a massage she seems to more than just appreciate.\");\n            outputText(\"[pg]To a gasp from your mistress above, you swoop down upon her toes again, turning her utterance into a sweet moan by devouring them greedily. Playfully, you bite down. She yelps in surprise, but her breath catches as the first telltale shiver courses through her, so you open wide and take as many of her toes into your mouth as you can. Careful to keep her from accidentally kicking you, you hold her fast and tight while you overload her with rapturous sensations, drawing out gasps and shudders with your slathering tongue and kneading fingers until her legs give out, utterly spent.\");\n            outputText(\"[pg]You have the clarity of mind to catch her safely and hold her up, but she promptly nudges your hands away and lets herself sink down onto you. A quartet of arms slings itself around your body to pull you into a post-orgasmic cuddle, and you're content to fulfill and bask in her demand for affection, the girl's winding-down breath cooling your own [skinshort] as she nestles into you.\");\n            outputText(\"[pg][say:Thank you,] Marielle whispers, all authority gone from her voice. She's much reverted to her usual self now and lifts her head to look at you with mild concern. [say:Have I, ah, harmed you?] You feel along your lips and find nothing. Seems she hasn't. She squeezes you again when you tell her and breathes a soft sigh.\");\n            outputText(\"[pg][say:Mayhap... Mayhap we ought do this again then, upon another time,] she murmurs into your ear. You're inclined to agree.\");\n        } else {\n            outputText(\"Your unwillingness must show, as her eyes grow wide.\");\n            outputText(\"[pg][say:Ah! I, uhm... Was't overmuch?] Her foot retreats, all semblance of a dominatrix now wiped from her features. [say:Yes. Yes, I thought as much... My apologies,] she mumbles, visibly troubled. You can't leave her like that. Still worn-out from your climax, you open your arms and beckon her closer, then grab her hands and pull her down onto you.\");\n            outputText(\"[pg][say:[name]?] half-yelps Marielle as you wrap your arms around her frail body and kiss her cheek. It feels nice to have someone rest on top of you amid the afterglow, even if that someone provides next to no bodily warmth. You hold her close and stroke her back, tell her you enjoyed what she did, and revel in the sensation of her slowly snuggling into you.\");\n            outputText(\"[pg][say:So you did...] She squeezes you. [say:'Tis kind comfort to the ear. And...] You feel her lightly nuzzling you. [say:And mayhap we might do this again, another time?] That, you could.\");\n        }\n        outputText(\"[pg]Having regained your energy, you eventually disentangle from one another and stand up. You look towards Marielle as you [if (!isnaked) {put your [armor] back on|stretch yourself}] and notice the dress is still surprisingly spotless; not a single blemish or crease has remained after your cuddling. As it is, you could still buy it, if you wanted to.\");\n        outputText(\"[pg]Do you?\");\n        saveContent.footstuff = true;\n        cheatTime(1);\n        purchaseMenu(shopCheongsamFootrubYes, shopCheongsamFootNo);\n    }\n\n    public function shopCheongsamFootrubHelp() {\n        clearOutput();\n        outputText(\"It would be selfish to not attend to the needs of your mistress, even if she does seem to enjoy her domineering role; it always feels better when someone else does it for you, after all.\");\n        outputText(\"[pg][say:What are you--] she starts as you shift slightly until you can comfortably raise your [if (tailLeg) {tail|leg}] up high and slide [if (tailLeg) {the tip|your [foot]}] over Marielle's cold skin. She flinches at the sudden touch, but doesn't stop you as you wander ever higher, over the large scar that marks her upper thigh, and to her waiting, neglected lips.\");\n        outputText(\"[pg][say:Ooh... yes, prithee proceed...] she half-whispers, shuddering as you touch her folds and in turn adjusting her position. [say:But, haah... forget not thy station.]\");\n        outputText(\"[pg]Grabbing hold of you, she uses the extra leverage to truly step on you and rub her entire foot over your crotch, intent on finishing you off before you do, but you're barely a heartbeat behind to reciprocate in kind.\");\n        outputText(\"[pg]The both of you are panting, moaning, grinding, and gyrating in tandem, the sounds of your depraved lesbian foot-loving echoing throughout the abandoned temple, and all you focus on is bringing the other's pleasure to its peak. Marielle has quite a head start on you in that regard, which makes it difficult to concentrate on anything at all as she makes you writhe underneath her sole, but that cold mask of authority on her face is meanwhile swiftly crumbling with each passing second, gradually unveiling her delightful reactions to your ministrations, if her increasingly slick sex wasn't hint enough of how well you're doing.\");\n        outputText(\"[pg]Sensing how close you have her, you work her pussy harder and harder, faster and faster, until finally, with a desperate whimper, she tenses up, her whole body trembles, and her lower hands grip your ankle like a vice. In her erratic, orgasmic shivers, she lurches over and bears her entire weight upon your own oversensitive [if (ischild) {girl|woman}]hood, promptly kicking you off the ledge as well, right behind the seamstress. Your hips rise off the ground as your vision momentarily turns to white and searing-hot ecstasy ripples through your veins, and you let out a long, shared moan, your [vagina] smearing your juices liberally across Marielle's heel. A heel that relentlessly works your nethers as the stars rise before your eyes.\");\n        outputText(\"[pg]Linked like this, you deliriously masturbate each other through your orgasms, not stopping until the last of the aftershocks has finally died down and faded to nothing.\");\n        outputText(\"[pg]Marielle's legs give out without much warning, but she has the reflexes to catch herself with her hands on your chest and not flop head-first onto you. Her cold, heavy breath tickles your face for a moment before she lowers herself the rest of the way, wraps you in her quadruple arms, and nuzzles against you for a post-orgasmic cuddle you're all too happy to return.\");\n        outputText(\"[pg]You can tell she's managing to stay awake, and, while tired, you don't quite feel like napping either--both of you are content with simply not saying anything and instead enjoying the blissful silence around you together. Her long, smooth hair feels lovely to stroke while you wait to recharge enough energy to move your body.\");\n        outputText(\"[pg][say:That was... marvellous,] she eventually breathes. [say:I have harmed you not, have I?] She props herself up to inspect you, but you assure the seamstress you're fine, bringing a relieved smile to her face. [say:Well, if you...] Her eyes then turn to the side while her lower hands fiddle with each other. [say:If ever you wish to, uhm, 'render a refrain', I... would be not ill-disposed to hear it.] You might take her up on that.\");\n        outputText(\"[pg]With her already sitting up like this, you decide it's time to get back to business. After helping her stand, washing yourself off, and [if (!isnaked) {getting dressed again|stretching your body}], you turn your attention back to Marielle. Perched on her stool, she's still busy wiping her foot, but you notice the dress is surprisingly spotless; not a single blemish or crease has remained after your prolonged cuddling. You could still buy it as it is, if you wanted to.\");\n        outputText(\"[pg]Do you?\");\n        saveContent.footstuff = true;\n        cheatTime(1);\n        purchaseMenu(shopCheongsamFootrubYes, shopCheongsamFootNo);\n    }\n\n    public function shopCheongsamFootrubYes() {\n        clearOutput();\n        outputText(\"You'll buy that dress.\");\n        outputText(\"[pg][say:Hmm?] Her head turns to you. [say:Oh! Yes, of course, of course,] she cheerily exclaims. [say:I will but needs take your measurements; pray hold, ah, still.] She whips out two measuring tapes and quickly notes down your sizes, then strips out of the dress, into her usual one, and gets to work.\");\n        outputText(\"[pg]Quick to finish, she soon beckons you over to hand you the custom-tailored dress in exchange for the appropriate amount of gems.\");\n        outputText(\"[pg][say:Well, that shall fit you quite agreeably. Though hesitate not to ask, should aught else be required.]\");\n        inventory.takeItem(armors.CHNGSAM, sexReturn.bind(\"[pg]If anyone 'requires' something, it seems to be her; it's late, and the stifled yawn that follows her words tells you all you need. Gathering your things, you give the girl your farewell for now.[pg][say:Ah, yes, a night most restful to you likewise, [name],] Marielle says with a curtsy. [say:'Twas quite a... pleasure.][pg]That it was indeed. Leaving the girl to prepare her bedroll, you exit the overgrown shrine and make the trek back to your campsite in a mood that even this uncooperative, foul-smelling terrain cannot dampen.\", \"[pg]If anyone 'requires' something, it seems to be her; you've been here for at least two hours now, and she doesn't seem to have much more energy left in her. Gathering your things, your give the girl your farewell for now.[pg][say:Oh, ah, yes, a day most pleasant to you likewise, [name],] Marielle says with a curtsy. [say:'Twas verily a... pleasure.][pg]That it was indeed. Leaving the girl to catch some rest, you exit the overgrown shrine and make the trek back to your campsite in a mood that even this uncooperative, foul-smelling terrain cannot dampen.\", \"\", shopClothes.bind()));\n    }\n\n    public function shopCheongsamFootjob() {\n        clearOutput();\n        outputText(\"Your eyes are drawn to those alabaster legs of hers, peeking out from the garment's side slits, then down to her dainty feet--you want to feel them, want those cute soles to grind over your [cock].\");\n        outputText(\"[pg][say:Huh?] Marielle exclaims as you voice your desires. [say:Is... Is that so...] She looks between your crotch and the ground in front of her feet, shuffling, apparently not being too happy about your suggestion, though \" + (saveContent.timesDicked != 0 ? \"after a while of silently studying you, \" : \"\") + \"a soft sigh finally escapes her.\");\n        outputText(\"[pg][say:If it shall merely be my feet, I... I suppose that I could do for you.] Great; you reach to [if (!isnakedlower) {free your dick to the fresh air|prop your dick up from between your legs}] as she says, [say:Only... well, how would we, ah... proceed?]\");\n        outputText(\"[pg][if (!isnakedlower) {Now|Already, as always,}] bared to the elements, you kneel down on the ground, lean back, and instruct Marielle about what you want her to do.\");\n        menu();\n        addNextButton(\"Lick Foot\", shopCheongsamFootjob2.bind(true)).hint(\"Start by worshipping her foot.\");\n        addNextButton(\"No Lick\", shopCheongsamFootjob2.bind(false)).hint(\"No foreplay, just a footjob.\");\n    }\n\n    public function shopCheongsamFootjob2(licked:Bool) {\n        clearOutput();\n        if (licked) {\n            outputText(\"[say:You wish to taste it.] It's no question, but a dry statement. One with an undertone that you can't quite place.\");\n            outputText(\"[pg]She might not be too keen on that, but still, she hesitantly lifts her right foot and offers it to you. A beautiful, delicate thing, despite--or perhaps because of--the stitched scar around her ankle. You reach out and hold it up to carefully slide her sandal off, setting it on the ground next to you. Her foot now naked in all its pale glory, you try to be as gentle as you can in your excitement while you cup it and run your fingers along the cold, prominent tendons. You slide forwards, round her heel, then slowly back over her remarkably flawless sole to the tip of her toes, interlacing your fingers between the gaps before leaning down and brushing your lips over the roof of her foot.\");\n            outputText(\"[pg]You feel her toes twitch, but she holds still while you paint a trail of kisses across. Arriving at her toes again, you testingly kiss the smallest one, then, tongue outstretched, take it into your mouth. There is little to taste. In undeath, she has no perspiration, which must be why there's only a faint, perhaps sweet-to-soapy tang that gives her chilly skin a peculiar note.\");\n            outputText(\"[pg][say:That... tickles, [name].] You slip your tongue between, making her flinch involuntarily. [say:You truly enjoy this?]\");\n            outputText(\"[pg]By answer, you hungrily lunge forward, devour her toes, and slide yourself all over them[if (silly) { as if playing a harmonica}]. After thoroughly slathering them up, you switch over to her sole. With sensuality, you draw your [tongue] across the underside of her delectable foot to receive a small chuckle in response, but all the while keep her steady, nuzzle against her, and drink in her taste and aroma. You're already hard, your [cock] yearning for touch, yearning for more. Somewhat reluctantly, you release her and voice your next desire.\");\n            outputText(\"[pg]Her forehead creased, Marielle looks down on you. [say:And you are quite certain you wish I... do this?] You absolutely are. [say:Hmm.] With her toes resting against your chin, she regards you for a long moment, then draws away and starts to travel down your body, over your neck and [chest], and down towards your loins, leaving behind a freezing trail of your own saliva. Perhaps you're already closer than you thought, as your dick throbs as she grazes its sensitive skin, sending a sharp shiver up your spine. She stops and looks at you in question, then presses down on your shaft, sandwiching it between her sole and your lower belly and getting it nice and slick.\");\n        } else {\n            outputText(\"[say:Are you quite certain?] she asks, creases on her forehead. You absolutely are. [say:Well, if that be what you wish, then...] She deftly pulls off her right sandal and sets it to the side, freeing her milky-white foot and flexing her toes.\");\n            outputText(\"[pg]With little further ado, she pokes your rock-hard shaft, sending a delightful shiver up your spine at her cold touch. Making sure to keep her toenails away, nicely trimmed though they are, she presses her entire sole against your [cock] and slides up your length. You have to suck in a sharp breath as she reaches the crown, already slick with pre, and slides down with the added lubrication, sandwiching you between her foot and your lower belly.\");\n        }\n        outputText(\"[pg]It feels strange, yet great: the cool softness of her delicate foot caresses your ever-more-heated dick, but as unrivalled as she may be with her hands, her feet are not quite on that level. You'll need some more stimulation to get off.\");\n        outputText(\"[pg][say:Hmm...] Marielle scrutinizes your erect length, idly servicing you. [say:Perhaps...] She lets off and scoots her stool over, sitting down on it and stripping off her other sandal. You think you like where this is going. She places her feet on your hips and adjusts herself, inadvertently pressing her weight down on your [skinfurscales] as she shifts her posture. You sidle a bit closer and try to find a more comfortable position, yourself; it feels like you're being used as an ottoman, though not in any unpleasant way. The girl's still light as a feather, after all--you could just push her off if you wanted. But you don't. Having settled in, you feel her feet move, back onto your still-turgid dick.\");\n        outputText(\"[pg]Toes sliding over your [cock], she practically massages you and kneads your excited member, trying to assess what to do to get a rise out of you and exploiting it. In tandem, they work you, at times gently caressing you, then suddenly clamping down before releasing yet again. The juxtaposition of near-uncomfortable pressure and tender strokes has you constantly on edge, reeling between abuse and comfort, veins alight with fire.\");\n        outputText(\"[pg][say:Is't... pleasurable?] Marielle asks. You grunt out a positive. You're close now, dangerously so, and you don't think you can weather her assault much longer. [say:I see.]\");\n        outputText(\"[pg]She then envelops your length between her soles. You see her grip the edge of the stool tight before she bears down on you, hard. Up and down she slides, her whole body rocking with the motion, grinding you with electrifying fiercety. Your addled mind wanders to her naked nethers, so close before you and only concealed by the the gown's single, long piece of silken fabric. So close, yet unobtainable. What would you give for even just a simple glimpse at this moment?\");\n        outputText(\"[pg]\" + (player.femininity >= 59 && player.hasVagina() ? \"[say:What... a covetous beast you are,] she says between huffs of exertion. You must have voiced that out loud. [say:Well then, perhaps... how would you like... this?] With one free hand, she lifts the skirt to the side, just slightly, but far enough. And there it is: her youthful, pristine, never-aging slit, crowned by a tiny bit of golden hair, flawless despite her otherwise marred skin, and no doubt as tight as--if not tighter than--the merciless grip of her soles and toes. So tempting, captivating, yet unreachable\" : \"But will alone won't move even a strip of cloth. Her slit--youthful, pristine, never-aging--remains barred from your eyes, and all you're left with is your imagination as the merciless grip of her soles on your shaft increases, conjuring into your mind the image of what it would feel like to sink into her tight, undead confines right now\") + \".\");\n        outputText(\"[pg]With that etched into you, the pressure rises too high; you suck in a breath through gritted teeth and let her pumping careen you over the ledge. Muscles tensing up as your vision and hearing momentarily become hazy, you finally unload, your [cock] splattering its load over Marielle's slim, laboring feet and legs, though the better part splashes onto your own writhing body. She falters and tries to dodge the worst of it, but doesn't let up, jerking you off through your entire prickling orgasm until you're reduced to panting and sweating as you wind down, a contented smile on your lips.\");\n        outputText(\"[pg]You feel her weight lift and breathe a sigh as Marielle retreats, leaving you to yourself once more.\");\n        outputText(\"[pg][say:Hmm, this has been, uhm... interesting,] she says, though the look on her face as she appraises her cum-stained feet is [if (femininity >= 59) {not exactly|all but}] a chipper one. Quietly grumbling to herself, she reaches for a cloth and a small canteen of water and starts to clean her stained legs. You didn't end up spotless either, so you get up--still feeling quite elated and staggering a little--wash yourself off at a nearby puddle, and [if (!isnakedlower) {get re-dressed|massage your sore muscles}].\");\n        outputText(\"[pg]When you're back, Marielle has nearly finished her own cleaning business. She did somehow manage to save the oriental dress from getting dirtied, and you wonder just how she pulled that off. In any case, you could still buy it like this, if you wanted to.\");\n        outputText(\"[pg]Do you?\");\n        player.orgasm('Dick');\n        marielleSex();\n        saveContent.footstuff = true;\n        cheatTime(.75);\n        purchaseMenu(shopCheongsamFootjobYes, shopCheongsamFootNo);\n    }\n\n    public function shopCheongsamFootjobYes() {\n        clearOutput();\n        outputText(\"You still want to take that dress.\");\n        outputText(\"[pg][say:Hmm?] She looks up to you and instinctively reaches for the gems you hand her. [say:Oh! You do yet... Yes, yes, of course, I will just, ah]--she sets down cloth and money and pulls a pair of measuring tapes from the table--[say:needs make a few adjustments. I bid you hold still.] You do just that as she quickly notes down your sizes before getting to work, her earlier frown replaced by a look of sheer concentration.\");\n        outputText(\"[pg]It doesn't take her long to finish, and she soon beckons you over, presenting you with the tailored dress.\");\n        outputText(\"[pg][say:And that should be all. Pray tell if aught be amiss, further adjustments will be free of charge, of course.]\");\n        inventory.takeItem(armors.CHNGSAM, sexReturn.bind(\"[pg]It's gotten late, and the stifled yawn that follows Marielle's words tells you it's time to go. You wish the seamstress a good night, which she reciprocates with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.\", \"[pg]You've been here for at least two hours now, and it seems the seamstress doesn't have the energy for much more left in her. Gathering your things, you say your farewell for now, which she reciprocates with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.\", \"\", shopClothes.bind()));\n    }\n\n    public function shopCheongsamFootNo() {\n        clearOutput();\n        sexReturn(\"It's gotten late, and a stifled yawn from Marielle tells you it's time to go. You wish the seamstress a good night, which she reciprocates somewhat absent-mindedly with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.\", \"You've been here for at least two hours now, and it seems the seamstress doesn't have the energy for much more left in her. Gathering your things, you say your farewell for now, which she reciprocates somewhat absent-mindedly with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.\", \"While you let Marielle get re-dressed, you take another look at her wares.\", shopClothes.bind());\n    }\n\n    public function shopSummerDress() {\n        clearOutput();\n        outputText(\"You pull out a simple, sleeveless, moderately ruffled dress. The design looks familiar, but as you hold it up for comparison to the seamstress's own attire of choice, you do spot some subtle differences in cut and frilling.\");\n        outputText(\"[pg][say:Ah, well,] she opens up under your gaze, [say:I cannot say these are... in particular demand, yet are they merchandise as any other garment here, and not but for mine own wardrobe. 'Tis unworn, of course, I wholly warrant you.] Marielle adds that in a hurry, waving her many arms defensively. Since you offer no objection, she picks up the proverbial thread again.\");\n        outputText(\"[pg][say:Well, uhm, 'tis positively light and airy, meet for summer's most overbearing heat or any other climate likewise too... canicular. Though pray mind your, ah, bared arms, for little there will stay sun's fiery brand.] She idly rubs her left pair of wrists at that.\");\n        outputText(\"[pg][say:So... wish you to purchase one? 210 gems would it be for you.]\");\n        currentPrice = 210;\n        purchaseMenu(shopSummerDressYes, shopSummerDressNo);\n    }\n\n    public function shopSummerDressYes() {\n        clearOutput();\n        outputText(\"You'll take one of these.\");\n        outputText(\"[pg][say:Oh, uhm, yes.] Marielle seems a little surprised[if (femininity >= 59) { and strangely flustered}], but regardless pulls out a pair of measuring tapes. [say:I will but needs make a few alterations for you, so pray be still awhile...] You let her take your sizes and hand her dress and money. Adjusting her glasses, she gets to work.\");\n        outputText(\"[pg]Shortly after, the seamstress rises from her seat with a satisfied [say:Hmm] and nods to herself.\");\n        outputText(\"[pg][say:Well, this ought be quite fine now, yet need you but ask, should it fit not comfortably. Ah, one more moment.] She turns to sift through one of her boxes until she finds another straw hat much like her own and hands it to you. [say:This one is, ah... 'on the house', as they say.] It does go rather well with the dress, so you take it.\");\n        doTransaction();\n        inventory.takeItem(armors.S_DRESS, shopClothes.bind());\n    }\n\n    public function shopSummerDressNo() {\n        clearOutput();\n        outputText(\"You fold the light dress and put it back.\");\n        shopClothes();\n    }\n\n    public function shopTurtleneck() {\n        clearOutput();\n        outputText(\"You spot something woolly and pull it out of the chest. It's a pullover. An overly large one at that, you note, and incredibly fluffy; your fingers easily sink into it as you stroke the surface.\");\n        outputText(\"[pg][say:Marvellous, is it not?] You didn't even notice Marielle scooting closer. [say:'Tis fashioned of, ah, sheep-maidens' wool.] She used sheep-girls for this? [say:Aye, males are unsuited, for not quite are they, ah, flocculent enough.\" + (player.sheepScore() >= 4 ? \" I, ah, uhm, well...] She looks at you as if she just realized something. [say:I mean not to offend... Pray pardon. But\" : \"] You weren't aware there were any big differences at all. [say:Well,\") + \" be what may, 'tis meet for the colder climates, for it keeps one exquisitely warm and comfortable withal,] she says as she grazes with her fingertip over the raised neck that would fit snugly to and protect the wearer's throat.\");\n        outputText(\"[pg][say:If 'tis by your liking, 260 gems and yours it shall be.]\");\n        outputText(\"[pg]Do you want to buy the thick sweater?\");\n        currentPrice = 260;\n        purchaseMenu(shopTurtleneckYes, shopTurtleneckNo, shopTurtleneckModel);\n    }\n\n    public function shopTurtleneckYes() {\n        clearOutput();\n        outputText(\"You'll buy it.\");\n        outputText(\"[pg][say:Wonderful!] The seamstress pulls out two measuring tapes. [say:I will needs make a few alterations though, pray hold still.] You do as told while she adjusts her glasses and takes in your sizes before getting to work on the sweater, telling you, [say:Hmm, it should take not overlong.]\");\n        outputText(\"[pg]You leave the money on the desk next to her, but she's already too engrossed in her work to notice, needles and yarn in hand.\");\n        outputText(\"[pg]A while later, Marielle calls you over again and presents you the finished sweater.\");\n        outputText(\"[pg][say:Well, that should be all. Its, ah, voluminousness is quite by purpose, but that should prove no cumber to its wearing comfort.]\");\n        doTransaction();\n        inventory.takeItem(armors.TRTLNCK, shopClothes.bind());\n    }\n\n    public function shopTurtleneckNo() {\n        clearOutput();\n        outputText(\"You fold the thick sweater and let it disappear back into the depths of the chest.\");\n        shopClothes();\n    }\n\n    public function shopTurtleneckModel() {\n        clearOutput();\n        outputText(\"[say:Oh?] Marielle looks at you, considering, then down at the sweater. [say:Hmm, I... Certainly, as you wish.] Without much hesitation, she takes the sweater from your hands and turns around to undress. You get a view of her naked, stitched-up backside for a while as she bunches the wool up and searches for the armholes, then pulls it over her head in one smooth motion, fans out her hair, pats the fuzzy thing down, and swivels to face you.\");\n        outputText(\"[pg][say:Well?] Marielle asks, cocking a brow as she presents herself.\");\n        outputText(\"[pg]It's definitely too big for her, even if some of that size is intentional in the first place; the dainty girl practically disappears in there. Her arms fit trough the sleeves two at a time, hands vanishing within their fluffy confines, and the hem reaches almost to her knees, making it look like a thick, woolly dress. Now she's even smaller than she really is. It does seem incredibly comfortable though, she was certainly right about that.\");\n        outputText(\"[pg][say:Uhm, wish you to purchase it? Or merely, ah... stare?]\");\n        outputText(\"[pg]Good question. Do you?\");\n        saveContent.modeled |= MODELED_TURTLENECK;\n        purchaseMenu(shopTurtleneckModelYes, shopTurtleneckModelNo);\n        addNextButton(\"Snuggle\", shopTurtleneckSnuggle).hint(\"Cuddle her.\").disableIf(!((saveContent.talks & TALK_SEX) != 0), \"You don't know her well enough to just cuddle her out of nowhere.\");\n    }\n\n    public function shopTurtleneckModelYes() {\n        clearOutput();\n        outputText(\"You hand the girl the money she asked for, which she quickly moves to grasp after pushing up her sleeves.\");\n        outputText(\"[pg][say:Wonderful, a very fine choice indeed,] Marielle chimes, spinning around to search for a measuring tape on her desk. [say:But I bid you hold still, please, I will needs make some changes, aforehand.] After you let her note down your sizes, she pulls the sweater off over her head, re-dresses, and gets to work.\");\n        outputText(\"[pg][say:By and by, 'twill be but a moment.]\");\n        outputText(\"[pg]It turns out to be more than that, but she still finishes the task quickly and presents you the tailored sweater.\");\n        outputText(\"[pg][say:Well, and that should be all. I hope you will be satisfied therewith.]\");\n        doTransaction();\n        inventory.takeItem(armors.TRTLNCK, shopClothes.bind());\n    }\n\n    public function shopTurtleneckModelNo() {\n        clearOutput();\n        outputText(\"[say:Oh...] Her shoulders slump just a little. [say:Well, uhm, should a change of mind find you, 'twill most like still be amongst mine assortment.]\");\n        outputText(\"[pg]While she changes back into her regular dress, you take another look at the other clothes.\");\n        shopClothes();\n    }\n\n    public function shopTurtleneckSnuggle() {\n        clearOutput();\n        outputText(\"You [if (singleleg) {glide|step}] up close to Marielle, a \" + (player.isFemale() && (saveContent.timesKissed + saveContent.timesYurid > 0) ? \"timid\" : \"wary\") + \" frown crossing her face as you do.\");\n        outputText(\"[pg][say:[name]? What are you--] Her question is cut short when you capture her in your arms and pull her into a hug.\");\n        outputText(\"[pg][say:I, ah, uhm, ah... [if (femininity >= 59) {Wh-}]What are you... Could you...] She freezes up and \" + (saveContent.timesDicked == 0 ? \"half-heartedly \" : \"\") + \"protests, but you pay that no mind and instead press her closer to you, lovingly petting her head. The sweater's thick material does a good job of masking the unliving cold of her body, and this close, you're able to breathe in her scent: the slightest note of something your brain wants to label 'foulness' beneath... Is that conifer perfume?\");\n        outputText(\"[pg][say:[name]? Hear you me?] You tell her you don't and sit down, dragging her to the ground with you. Despite her objections, Marielle lets herself be lead and ends up sitting in your lap, facing you.\");\n        outputText(\"[pg]She remains tense, her features marked by [if (femininity >= 59) {a light blush mixed with a look of }]uncertainty. You draw her in again, this time without protest on her side, and press your chest to hers. Tenderly rubbing Marielle's back, you try to make her warm up to you, at least figuratively. Through the fluff, you can make out the faint course of a few of her large scars and stitches as you pass over them, but you don't mind, you embrace her all the same, snuggle her, and stroke her silky hair.\");\n        outputText(\"[pg][say:Hmmm...] Marielle's face is not visible to you as you feel her shift. Cautiously at first, she leans in, her arms wrapping around you. With them being confined in their fuzzy sleeves as they are, you briefly wonder what a proper four-armed cuddle would feel like. Maybe you should ask her to tailor one of these to better fit her own physique.\");\n        outputText(\"[pg]The girl at last relaxes, and you feel a breathy sigh on your neck. [if (insultedfather) {Finally, she lightly nuzzles into you and returns the embrace at least partially|[if (hascock) {She still seems somewhat careful as she nuzzles into you, but does return the embrace as earnestly as she can|As she nuzzles into you, she finally returns the embrace in earnest}]}].\");\n        outputText(\"[pg]Deciding to reward her, you part her hair to scratch her behind the ear. There's that tension again, but she then giggles, her [if (isfemale) {entire }]body pleasantly quivering as she does, inciting you to tickle a few more stifled chuckles out of her before you let your hands return and tend to Marielle's soft, woolly back. Sighing once more, she [if (hascock) {makes herself more comfortable in|nestles into}] your arms and rests against your \" + (!player.hasCock() && player.femininity >= 59 && saveContent.insulted == 0 ? \"cheek\" : \"shoulder\") + \"--her steady, calm breath like a cool summer breeze.\");\n        menu();\n        addNextButton(\"Just Cuddle\", shopTurtleneckCuddle).hint(\"Continue cuddling with her for a while longer.\");\n        addNextButton(\"Lesbian Cuddle\", shopTurtleneckLesbian).hint(\"Have some fun between girls like this.\").sexButton(BaseContent.FEMALE).disableIf(player.isHerm(), \"In this position, your dick would make this too awkward.\");\n        addNextButton(\"Cuddle Fuck\", shopTurtleneckFuck).hint(\"She's already sitting in your lap, might as well stick it in.\").sexButton(BaseContent.MALE);\n        addNextButton(\"Stand Up\", shopTurtleneckStand).hint(\"Wrap up the cuddling session on this note.\");\n    }\n\n    public function shopTurtleneckCuddle() {\n        clearOutput();\n        outputText(\"It's therapeutic, holding her like this. You feel your own [if (isgoo) {gooey faux-}]muscles--weary from long travels--relax and unwind, and a contented sigh leaves your lips. There's nothing stopping you from giving into the temptation to just stay like this for a while. Burying your head in Marielle's wool-clad shoulder, you close your eyes and breathe in deep. The sweater smells earthy, natural, almost nostalgic, and you are soon lost in its scent, mixed with the girl's own, a fuzzy drowsiness seeping into your mind.\");\n        outputText(\"[pg]You find yourself in an open field, lying face-up on soft ground while you lazily gaze up to the cloudy sky. You hear a distant bleat from above. That's right, these clouds, they do remind you of sheep.\");\n        outputText(\"[pg]You reach up and pluck one, pulling it down from its heavenly canvas. It protests softly as you hug the warm creature to your body, and you smile. It smiles back. More fluff tickles the side of your face and you look around. The entire field is sheep, covering you and the ground in a thick layer of warm, fuzzy fleece. The sun is a sheep too, and so are the trees and the birds. They float closer, snuggle up to you, and envelop you in their woolly, baaing confines. A soft, cool bleat caresses your [skinfurscales], and you drift off to sheep.\");\n        outputText(\"[pg]You open your eyes. Did you doze off?\");\n        outputText(\"[pg][say:Hmmngh...]\");\n        outputText(\"[pg]The girl in your lap stirs--drowsy, grey-blue eyes blinking drunkenly. Marielle lifts her glasses and tries to rub the sleep from her face.\");\n        outputText(\"[pg][say:Hmmn... Ah. Oh.] She finally realizes she's still on your lap, [if (insultedfather) {resting against|snuggled up to}] you. [saystart]I, ah, must have... fallen as[if (silly) {h|l}]eep. [if (insultedfather) {Pray pardon.[sayend]|My apoah~[sayend] Her words are swallowed by a yawn. [say:Apologies.]}]\");\n        outputText(\"[pg]You stroke her head, eliciting another indistinct mumble from the girl. \" + (saveContent.insulted == 0 && (player.femininity >= 59 || player.isFemale != null) ? \"[say:Hmmn... Thank you, [name],] she says, making you wonder what for. [say:For... this. I, ah... I believe 'twas rather a balm upon the spirits.] [if (isfemale) {She can't hide her following tinge of diffidence and decides to simply say nothing further.|It seems she genuinely enjoyed that.}] The two of you remain for a bit longer, enjoying each other's embrace\" : \"Nothing more comes after that, though, and you simply enjoy the embrace for a bit longer\") + \" while your senses slowly return to a wakefulness sufficient enough to untangle yourselves and stretch. That was quite refreshing; that sweater was even more comfortable than she promised, and you weren't even the one wearing it.\");\n        outputText(\"[pg]Do you want to buy it now?\");\n        cheatTime(1);\n        player.changeFatigue(-20);\n        purchaseMenu(shopTurtleneckSnuggleYes, shopTurtleneckSnuggleNo);\n    }\n\n    public function shopTurtleneckLesbian() {\n        clearOutput();\n        outputText(\"You realize she must have fallen asleep in your arms. You cradle Marielle, delighted by the way she is [if (insultedfather) {resting against|snuggled up to}] you. But you're more than just delighted. Fidgeting, you notice how excited you've become, so you decide to do something about that.\");\n        outputText(\"[pg]Trying not to stir too much, you slip a hand [if (!isnakedlower) {beneath your [armor],|down}] towards your aching groin. Your fingertips travel over your sensitive mound, then dip into the den of your arousal and back out over your [clit], where short little rubs serve to work you up into higher gears. Stimulating yourself like this, you let your other hand roam over the girl's slumbering body to fuel your imagination and hug her to you. She feels so nice, smells so nice--you can't resist thrusting your digits deep inside, your breath sultry against Marielle's neck.\");\n        outputText(\"[pg]As you vigorously masturbate yourself, you envision it to be the seamstress's fingers that are driving into your cunt right now on a quest to quench your surging urges, grazing all the right spots on their way in and out, and leaving you wanting for more with each swift plunge.\");\n        outputText(\"[pg]You jerk out of your fantasy when suddenly you feel a cold and very real touch atop your busy hand.\");\n        outputText(\"[pg][say:Stay yourself not, [name],] Marielle quickly whispers into your ear, her voice doused in emboldening arousal. How long has she been awake, now? [say:...But pray allow me... aid you.] A single finger joins yours, and you're more than happy to drop that question and let her fill your [vagina] even more satisfyingly, coaxing a sigh out of you as she pushes deeper. Shuddering, you encourage her to go on.\");\n        outputText(\"[pg]When she brushes her lips against your neck and takes control of your own hand to pleasure you with, you decide to return the favor and sneak up over her cold, scarred thigh, underneath that wild-scented sweater, and towards her waiting womanhood. She tenses, but seems ready for you, so you sink a first digit into her tight confines with relative ease, accompanied by a sweet moan when you squeeze in another. Only for a moment does she forget her own ministrations before she resumes them with added vigor, roaming your entire body to make you shiver each time she passes over an especially erogenous spot.\");\n        outputText(\"[pg]It feels strange, having someone else use your fingers on yourself like this, but Marielle's apt guidance and bony additions make for an increasingly exhilarating experience, and it's evident you're having a similar effect on her. Already, you can feel she's getting close, her slickening walls wringing your digits, and her--as well as your own--exhalations coming out in ragged huffs. But even so, you want more than this.\");\n        outputText(\"[pg][say:[name], wait... hold... please,] she pants. [say:Closer, I want us...] She may well be reading your heated mind right now. You each slide your dripping fingers out of the other to reposition yourselves. After a few moments of fumbling made more difficult by unbridled passions and roving hands, your hips are aligned, your lower lips firmly kissing in a swelling rhythm, and you get a first proper look at Marielle's face.\");\n        outputText(\"[pg]She's flushed bright red, her eyes lidded, and when her mouth opens again to say something, you lean in to conquer it. Lust overcomes you fully as soon as she softly moans into you and grips your back, compelling you to thoroughly embrace her wool-clad body, grab her small butt, and ravage the girl as you grind your pelvises together with the extra leverage. She's quick to reciprocate in kind and closes her eyes, wallowing in the shared sensations.\");\n        outputText(\"[pg]Sloppy though it is, this does feel glorious--her cool folds sliding over and between yours while meeting your [clit] on occasion, the undead seamstress pressed up close enough for you to feel her heartbeat even through the thick pullover, and her soft lips desperately exploring your own as you suck her breath away. With a heavy gasp, you separate for much-needed gulps of crisp air, but you aren't idle for long; you seek out those lips again, but this time only for a short, titillating moment before you detour over her cheek, travelling the line of one of her scars, then dip below with a wanton appetite as you peel down the sweater's turtleneck to expose and fervently kiss the white, delicate skin of her neck.\");\n        outputText(\"[pg]Spurred on by pair of hands raking over the back of your head, you dig into her without restraint, likely leaving behind more than just a few love bites in your wake, but neither you nor she cares as you work your bodies in a carnal, sapphic embrace of rough kisses, breathy moans, wet squelches, and fuzzy fleece. All you want now is to bring you both over the edge that's oh-so thrillingly close and deliver that final push.\");\n        outputText(\"[pg][say:[name],] she whimpers out a second before her body locks up, and you feel the girl begin to tremble and squeeze you even tighter, her desperate grinding and quaking proving to be what's needed to set you off as well.\");\n        outputText(\"[pg]Together, you throw yourselves down into the rush of ecstasy, clutching, fondling, and groping whatever you can take hold of while gyrating your hips with galvanizing passion to delightfully rub your clits on one another in pursuit of those ever-higher reaches of bliss. Hunger in her eyes, Marielle pulls you into a kiss once more, muffling your lustful duet with a cold, questing tongue as moments turn to eternities in your mind and you ride out your orgasms in undulating tandem.\");\n        outputText(\"[pg]When you finally descend from that blissful high, you slip more than break off from the undead girl's lips to fill your lungs with fresh air once more. Your senses welcome the return of the world around you, and you revel in the sweet afterglow while leaning against an equally exhausted Marielle. You figure you'd both collapse if it weren't for each other's support.\");\n        outputText(\"[pg][say:I... That... That was...] The seamstress tries to speak, but gives up between gulps and giggles and instead nestles into your shoulder to rest.\");\n        outputText(\"[pg]You sit there for a while, simply enjoying the mutual embrace and intimate closeness until your bodies calm down enough to find their strength again.\");\n        outputText(\"[pg]Being first to rise, you leave Marielle kneeling on the ground[if (corruption < 50) {, after she assures you she's going to be fine,}] and freshen up by a nearby puddle. When you return, having recollected yourself, she's still sitting there, gazing off into the nothingness with a smile on her rosy-cheeked face.\");\n        outputText(\"[pg]You're drawn towards the sweater. That piece of woolly, comfy goodness. It's surprisingly spotless, even after all that rough lovemaking--she doesn't sweat, nor squirt. That must help a lot with keeping your clothes clean.\");\n        outputText(\"[pg]You could still buy it like this. Do you want to?\");\n        player.orgasm('Vaginal');\n        marielleSex();\n        cheatTime(1);\n        purchaseMenu(shopTurtleneckSnuggleYes, shopTurtleneckSnuggleNo);\n    }\n\n    public function shopTurtleneckFuck() {\n        clearOutput();\n        outputText(\"You can't ignore your erection much longer. The girl in your arms, her tiny chest--though hidden beneath a thick layer of wool--rising and falling with each breath and [if (insultedfather) {resting|pressed up}] against you, has you riled up quite a bit. Her womanhood is obscured by only the sweater and so close to your own burning crotch, just begging to be filled.\");\n        outputText(\"[pg]Without stirring too much, you shift [if (!isnakedlower) {your [armor] out of the way and bare your [cock] to|to adjust your [cock] and let it breathe}] the old temple's fresh air. It flops and throbs against the pullover. The soft wool does feel lovely, but that's not what you're after.\");\n        outputText(\"[pg][say:Hmnmgh...] Marielle mumbles. She must have dozed off in your arms. [say:Wha...?] She stirs and looks down to your exposed and eager dick.\");\n        outputText(\"[pg][say:What? Oh... Oh. Uhm.]\");\n        outputText(\"[pg]You lift the hemline a touch and hoist her up, making your intentions clear. After a second of fumbling, you find her entrance and press the crown of your cock against it.\");\n        if (vaginalAvailable()) {\n            outputText(\"[pg]\" + (saveContent.timesDicked > 1 ? \"Any imminent protest \" + (saveContent.timesDicked > 2 ? \"dies in her throat\" : \"is swallowed back down\") + \" when you push your tip into her folds. Only a \" + (saveContent.timesDicked > 3 ? \"choked\" : \"tremulous\") + \" gasp makes its way out, her fingers digging into your shoulders with a strength that is rivalled by how her passage--tight, cold, and bone-dry--screams against your intrusion. \" + (saveContent.timesDicked > 3 ? \"Clenching her teeth and squeezing\" : \"Squeezing\") + \" her eyes shut, she leaves you no choice but to wait.\" : \"[say:Wha-- Tarry, [name], I--] Her protest turns into a wince as you push your tip into her folds. She tenses up, her fingers digging into your shoulders with surprising strength. She's tight. And cold. And bone-dry. [if (corruption < 50) {A pang of guilt hits you at her pained reaction, and you reach to stroke her hand. You'll give her all the time she needs.|You'd like to push in further, but that's not going to happen with her clenching you like a virgin. You'll just have to wait.}]\") + \"\");\n            outputText(\"[pg]After a while of controlled breathing, \" + (saveContent.timesDicked > 2 ? \"or at least the attempt of it, \" : \"\") + \"she manages to relax herself somewhat. Her head turned to the side, the \" + (saveContent.timesDicked > 1 ? \"steel-blue of her \" + (saveContent.timesDicked > 3 ? \"eyes remains barred to you as she refuses to open them again\" : \"watery, re-opened eyes wanders somewhere far away, away from you and her obvious distress\") + \". The seamstress yields no sign of consent, but if she hasn't stopped you now, you'll take her silence as such, and so you trust gravity to get the better of her vice-like grip on your dick and\" : \"seamstress gives you the slightest of nods, despite her more than obvious distress. The steel-blue eyes behind her glasses are still watery, but with her vice-like grip on your dick softening, you trust gravity to\") + \" slowly, gently, lower her down. [if (cocklength > 8) {Her cool inner walls clamp down again and again as more and more of your length sinks into her, not [if (cocklength > 15) {much|all}] of it being able to fit before you run out of space to fill. \" + (saveContent.timesDicked > 3 ? \"Grimacing into the void, \" : (saveContent.timesDicked > 1 ? \"Wincing, \" : \"");
        }

        public void ShopMaidFingeredNo()
        {
            ClearOutput();

            SexReturn("It's getting late, so you stretch yourself and [if (!isnakedlower) {re-dress|crack your neck}], then say your farewell to Marielle. Before you take on the journey back to camp, you briefly wash yourself by a small puddle and glance back towards a girl who's already readying her bedroll.", "You've been spending quite some time here now, so you think you should be checking on your camp. After you share your farewells, you briefly wash yourself off and take on the journey back through the brackish swamp.", "You stretch yourself and [if (!isnakedlower) {re-dress|crack your neck}], then go to wash yourself while you let Marielle freshen up and change clothes before you take another look at her assortment.", () => ShopClothes());
        }

        public void ShopMaidHandjob()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidHandjob.KnowWhatWant".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.WhatAreYouDoing".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.SitOnDesk".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.GlancesUnderstanding".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.SoftVoiceBleeding".Localized((Save.GetInt("timesDicked", 0) != 0 ? "both distaste and wariness" : "distaste")));

            OutputText("Scenes.Marielle.ShopMaidHandjob.CurtlyBows".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.ChillyTouch".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.LeansLowerArms".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.SteelGazeStops".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.MeasuredMotions".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.CloseUnderAssault".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.GlancesUpYes".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.QuartetSensations".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.FirstSpurtYelp".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.WindDownRelief".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.GrumblesAbandon".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.IrritatedSmile".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.SquintingCloth".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjob.BuyAfterHandjob".Localized());

            Player.Orgasm("Dick");

            MarielleSex();

            View.Time.PassMinutes((int)Round((.75) * 60));

            PurchaseMenu(ShopMaidHandjobYes, ShopMaidHandjobNo);
        }

        public void ShopMaidHandjobYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopMaidHandjobYes.ThinksBuyOutfit".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjobYes.LooksUpCleaning".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjobYes.AddsSlightBow".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjobYes.RedressLetWork".Localized());

            OutputText("Scenes.Marielle.ShopMaidHandjobYes.ExchangesDress".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.MaidDress), 1);

            SexReturn("[pg]Looks good, but it's getting late now, so you share your farewells with the seamstress and take on the journey through the brackish swamp back to your camp.");
        }

        public void ShopMaidHandjobNo()
        {
            ClearOutput();

            SexReturn("It's getting late, so you say your farewells to the seamstress before you leave her to re-dress and wash herself off as you take on the journey back to your camp.", "You've been spending quite some time here now, so you think you should be getting back to camp. After sharing your farewells, you leave Marielle to wash herself off and take on the journey back through the brackish swamp.", "You climb down from the desk, arranging your [armor] again, and leave Marielle to re-dress and wash herself off while you take another look at her wares.", () => ShopClothes());
        }

        public void ShopButler()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopButler.NoticeButlerPieces".Localized());

            OutputText("Scenes.Marielle.ShopButler.ChimesInForgotten".Localized());

            OutputText("Scenes.Marielle.ShopButler.NotMensSpeciality".Localized());

            OutputText("Scenes.Marielle.ShopButler.SuitForAnyone".Localized());

            OutputText("Scenes.Marielle.ShopButler.FiveEightyGems".Localized());

            OutputText("Scenes.Marielle.ShopButler.TakeItQuestion".Localized());

            currentPrice = 580;

            PurchaseMenu(ShopButlerYes, ShopButlerNo, ShopButlerModel);
        }

        public void ShopButlerYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopButlerYes.MarvellousWillAdjust".Localized());

            OutputText("Scenes.Marielle.ShopButlerYes.HumsContented".Localized());

            OutputText("Scenes.Marielle.ShopButlerYes.BespokeIndeed".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.ButlerSuit), 1);

            ShopClothes();
        }

        public void ShopButlerNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopButlerNo.FoldNeatlyBack".Localized());

            ShopClothes();
        }

        public void ShopButlerModel()
        {
            ClearOutput();

            if ((Save.GetInt("modeled", 0) & MODELED_BUTLER_SUIT) != 0)
                OutputText("Scenes.Marielle.ShopButlerModel.YetAgainFrowns".Localized());
            else
            {
                OutputText("Scenes.Marielle.ShopButlerModel.BlinksConfused".Localized());

                OutputText("Scenes.Marielle.ShopButlerModel.WishMeDon".Localized());
            }

            OutputText("Scenes.Marielle.ShopButlerModel.HesitantButTakes".Localized());

            OutputText("Scenes.Marielle.ShopButlerModel.SurprisingFit".Localized());

            OutputText("Scenes.Marielle.ShopButlerModel.WellWhatThink".Localized());

            OutputText("Scenes.Marielle.ShopButlerModel.ThinkBuyingIt".Localized());

            Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_BUTLER_SUIT);

            PurchaseMenu(ShopButlerModelYes, ShopButlerModelNo);
        }

        public void ShopButlerModelYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopButlerModelYes.BuysThisOne".Localized());

            OutputText("Scenes.Marielle.ShopButlerModelYes.AlreadyChanging".Localized());

            OutputText("Scenes.Marielle.ShopButlerModelYes.ShallCallFinished".Localized());

            OutputText("Scenes.Marielle.ShopButlerModelYes.PromisedBeckons".Localized());

            OutputText("Scenes.Marielle.ShopButlerModelYes.NoteOvertight".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.ButlerSuit), 1);

            ShopClothes();
        }

        public void ShopButlerModelNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopButlerModelNo.SighsChangesBack".Localized());

            OutputText("Scenes.Marielle.ShopButlerModelNo.ChangeOfHeart".Localized());

            ShopClothes();
        }

        public void ShopCheongsam()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsam.CatchesSilky".Localized());

            OutputText("Scenes.Marielle.ShopCheongsam.FormFittingMotif".Localized());

            OutputText("Scenes.Marielle.ShopCheongsam.PicksWordsCarefully".Localized());

            OutputText("Scenes.Marielle.ShopCheongsam.IdlyStrokesHair".Localized());

            OutputText("Scenes.Marielle.ShopCheongsam.MultifariousGarb".Localized());

            OutputText("Scenes.Marielle.ShopCheongsam.EnjoysEmbroidery".Localized());

            OutputText("Scenes.Marielle.ShopCheongsam.BuyExoticDress".Localized());

            currentPrice = 400;

            PurchaseMenu(ShopCheongsamYes, ShopCheongsamNo, ShopCheongsamModel);
        }

        public void ShopCheongsamYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamYes.HandsMoneyDress".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamYes.GrabsTapesMeasure".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamYes.NotOverlongMinute".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamYes.SlightlyLonger".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamYes.AllDoneAmiss".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.ChinaDress), 1);

            ShopClothes();
        }

        public void ShopCheongsamNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamNo.FoldsSilkenDress".Localized());

            ShopClothes();
        }

        public void ShopCheongsamModel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamModel.HesitatesIllBefit".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModel.SwiftGlimpseBack".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModel.WellPrompt".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModel.LooseThinFrame".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModel.BuyingDressQuestion".Localized());

            Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_CHINA_DRESS);

            PurchaseMenu(ShopCheongsamModelYes, ShopCheongsamModelNo);

            AddNextButton("Common.Button.Sex".Localized(), ShopCheongsamSex);
        }

        public void ShopCheongsamModelYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamModelYes.TakesThisOne".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModelYes.DipsCurtsy".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModelYes.NotOverlongWarrant".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModelYes.BeckonsTailored".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModelYes.UncomfortableTell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.ChinaDress), 1);

            ShopClothes();
        }

        public void ShopCheongsamModelNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamModelNo.SlightFrownTell".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamModelNo.ChangesBackCollection".Localized());

            ShopClothes();
        }

        public void ShopCheongsamSex()
        {

            ClearButtons();

            AddNextButton("Common.Button.Footrub".Localized(), ShopCheongsamFootrub);

            AddNextButton("Common.Button.Footjob".Localized(), ShopCheongsamFootjob);

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => ShopCheongsamModel());
        }

        public void ShopCheongsamFootrub()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.AlabasterThigh".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.AreYouUnwell".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.SuppressShudder".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.WantMoreCloser".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.WhereforeAreYou".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.SurveysNakedBody".Localized());

            if (Save.GetBool("footstuff"))
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootrub.UnderstoodNoMove".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.OnceMoreRequest".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootrub.StaresProcessing".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.WishMeTo".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.FidgetsSandalsOff".Localized());
            }

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.RaisesLegToes".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.ShuddersGrazes".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.BegCommand".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.CommandChillingGravity".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.ServantExemplary".Localized((Player.Femininity >= 59 && !Player.HasCock() ? "I ought make you verily mine" : "that be your true calling")));

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.VisionBlursDescends".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrub.ReasonOversets".Localized());

            MarielleSex();

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootrub.PantingMindFlush".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.HipsRiseTopple".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.PressureLifts".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.AreYouAllRight".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.DressSparedStains".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrub.WantToQuestion".Localized());

                View.Time.PassMinutes((int)Round((.75) * 60));

                Player.Orgasm("All");

                PurchaseMenu(ShopCheongsamFootrubYes, ShopCheongsamFootNo);
            }
            else
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootrub.PantingBothDesire".Localized());

                ClearButtons();

                AddNextButton("Common.Button.StayPut".Localized(), ShopCheongsamFootrubStay);

                AddNextButton("Common.Button.HelpHerOut".Localized(), ShopCheongsamFootrubHelp);
            }
        }

        public void ShopCheongsamFootrubStay()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamFootrubStay.CloseMercilessSole".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubStay.HipsRiseToppleAgain".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubStay.PressureLiftsAgain".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubStay.CleanseMeRequest".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddNextButton("Common.Button.Lick".Localized(), () => ShopCheongsamFootrubStay2(true));

            AddNextButton("Common.Button.Refuse".Localized(), () => ShopCheongsamFootrubStay2(false));
        }

        public void ShopCheongsamFootrubStay2(bool licked)
        {
            ClearOutput();

            if (licked)
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.NoNeedToTell".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.BreakAwayGlanceUp".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.NotPermittedToStop".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.TrailingKissesBack".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.SwoopDownToesAgain".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.ClarityOfMindCatch".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.AuthorityGoneWhisper".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.OughtDoAgainMurmur".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.UnwillingnessShows".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.FootRetreats".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.HalfYelpsEmbrace".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.SqueezesKindComfort".Localized());
            }

            OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.RegainedEnergyStand".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubStay2.DoYou".Localized());

            Save.SetBool("footstuff", true);

            View.Time.PassMinutes((int)Round((1) * 60));

            PurchaseMenu(ShopCheongsamFootrubYes, ShopCheongsamFootNo);
        }

        public void ShopCheongsamFootrubHelp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.SelfishToRefuse".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.WhatAreYouStart".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.PritheeProceedShudder".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.GrabsForLeverage".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.BothPanting".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.SensingCloseWork".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.LinkedMutualMasturbation".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.LegsGiveOut".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.ManagingStayAwake".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.MarvellousBreathes".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.BackToBusiness".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubHelp.DoYou".Localized());

            Save.SetBool("footstuff", true);

            View.Time.PassMinutes((int)Round((1) * 60));

            PurchaseMenu(ShopCheongsamFootrubYes, ShopCheongsamFootNo);
        }

        public void ShopCheongsamFootrubYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamFootrubYes.BuyThatDress".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubYes.HeadTurnsAgree".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubYes.QuickFinishBeckons".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootrubYes.FitQuiteAgreeably".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.ChinaDress), 1);

            SexReturn("[pg]If anyone 'requires' something, it seems to be her; it's late, and the stifled yawn that follows her words tells you all you need. Gathering your things, you give the girl your farewell for now.[pg][say:Ah, yes, a night most restful to you likewise, [name],] Marielle says with a curtsy. [say:'Twas quite a... pleasure.][pg]That it was indeed. Leaving the girl to prepare her bedroll, you exit the overgrown shrine and make the trek back to your campsite in a mood that even this uncooperative, foul-smelling terrain cannot dampen.");
        }

        public void ShopCheongsamFootjob()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamFootjob.DrawnToLegs".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob.ExclaimsAtDesire".Localized((Save.GetInt("timesDicked", 0) != 0 ? "after a while of silently studying you, " : "")));

            OutputText("Scenes.Marielle.ShopCheongsamFootjob.MerelyMyFeet".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob.BaredKneelDown".Localized());

            ClearButtons();

            AddNextButton("Common.Button.LickFoot".Localized(), () => ShopCheongsamFootjob2(true));

            AddNextButton("Common.Button.NoLick".Localized(), () => ShopCheongsamFootjob2(false));
        }

        public void ShopCheongsamFootjob2(bool licked)
        {
            ClearOutput();

            if (licked)
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.DryStatement".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.NotKeenLiftsFoot".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.ToesTwitchStill".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.TongueBetweenTickles".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.LungeDevourToes".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.ForeheadCreasedLooks".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.QuiteCertainAsk".Localized());

                OutputText("Scenes.Marielle.ShopCheongsamFootjob2.PokesShaft".Localized());
            }

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.StrangeYetGreat".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.ScrutinizesErectLength".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.ToesMassageCock".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.GruntPositive".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.EnvelopsBetweenSoles".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.PlaceholderPeriod".Localized((Player.Femininity >= 59 && Player.HasVagina() ? "[say:What... a covetous beast you are,] she says between huffs of exertion. You must have voiced that out loud. [say:Well then, perhaps... how would you like... this?] With one free hand, she lifts the skirt to the side, just slightly, but far enough. And there it is: her youthful, pristine, never-aging slit, crowned by a tiny bit of golden hair, flawless despite her otherwise marred skin, and no doubt as tight as--if not tighter than--the merciless grip of her soles and toes. So tempting, captivating, yet unreachable" : "But will alone won't move even a strip of cloth. Her slit--youthful, pristine, never-aging--remains barred from your eyes, and all you're left with is your imagination as the merciless grip of her soles on your shaft increases, conjuring into your mind the image of what it would feel like to sink into her tight, undead confines right now")));

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.EtchedPressureRises".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.WeightLiftSigh".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.InterestingRemark".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.BackSheFinished".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjob2.DoYou".Localized());

            Player.Orgasm("Dick");

            MarielleSex();

            Save.SetBool("footstuff", true);

            View.Time.PassMinutes((int)Round((.75) * 60));

            PurchaseMenu(ShopCheongsamFootjobYes, ShopCheongsamFootNo);
        }

        public void ShopCheongsamFootjobYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheongsamFootjobYes.StillWantDress".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjobYes.ReachesForGems".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjobYes.DoesntTakeLong".Localized());

            OutputText("Scenes.Marielle.ShopCheongsamFootjobYes.PrayTellIfAmiss".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.ChinaDress), 1);

            SexReturn("[pg]It's gotten late, and the stifled yawn that follows Marielle's words tells you it's time to go. You wish the seamstress a good night, which she reciprocates with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.");
        }

        public void ShopCheongsamFootNo()
        {
            ClearOutput();

            SexReturn("It's gotten late, and a stifled yawn from Marielle tells you it's time to go. You wish the seamstress a good night, which she reciprocates somewhat absent-mindedly with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.", "You've been here for at least two hours now, and it seems the seamstress doesn't have the energy for much more left in her. Gathering your things, you say your farewell for now, which she reciprocates somewhat absent-mindedly with an added curtsy, and make the trek through the foul-smelling bog back to your campsite.", "While you let Marielle get re-dressed, you take another look at her wares.", () => ShopClothes());
        }

        public void ShopSummerDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSummerDress.SimpleSleevelessDress".Localized());

            OutputText("Scenes.Marielle.ShopSummerDress.OpensUnderGaze".Localized());

            OutputText("Scenes.Marielle.ShopSummerDress.LightAndAiry".Localized());

            OutputText("Scenes.Marielle.ShopSummerDress.WishPurchaseOne".Localized());

            currentPrice = 210;

            PurchaseMenu(ShopSummerDressYes, ShopSummerDressNo);
        }

        public void ShopSummerDressYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSummerDressYes.TakeOneOfThese".Localized());

            OutputText("Scenes.Marielle.ShopSummerDressYes.SurprisedButYes".Localized());

            OutputText("Scenes.Marielle.ShopSummerDressYes.SeamstressRises".Localized());

            OutputText("Scenes.Marielle.ShopSummerDressYes.SiftsThroughPile".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.SummerDress), 1);

            ShopClothes();
        }

        public void ShopSummerDressNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSummerDressNo.FoldLightDress".Localized());

            ShopClothes();
        }

        public void ShopTurtleneck()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneck.SpotWoollyPullover".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneck.DidntNoticeScooting".Localized((Player.SheepScore() >= 4 ? " I, ah, uhm, well...] She looks at you as if she just realized something. [say:I mean not to offend... Pray pardon. But" : "] You weren't aware there were any big differences at all. [say:Well,")));

            OutputText("Scenes.Marielle.ShopTurtleneck.ByYourLiking".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneck.BuyThickSweater".Localized());

            currentPrice = 260;

            PurchaseMenu(ShopTurtleneckYes, ShopTurtleneckNo, ShopTurtleneckModel);
        }

        public void ShopTurtleneckYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckYes.BuyIt".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckYes.PullsMeasuringTapes".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckYes.LeaveMoneyDesk".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckYes.CallsWithSweater".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckYes.VoluminousByPurpose".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.Turtleneck), 1);

            ShopClothes();
        }

        public void ShopTurtleneckNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckNo.FoldThickSweater".Localized());

            ShopClothes();
        }

        public void ShopTurtleneckModel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckModel.ConsidersSweater".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModel.CocksBrowAsks".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModel.TooBigForHer".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModel.PurchaseOrStare".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModel.GoodQuestionDoYou".Localized());

            Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_TURTLENECK);

            PurchaseMenu(ShopTurtleneckModelYes, ShopTurtleneckModelNo);

            AddNextButton("Common.Button.Snuggle".Localized(), ShopTurtleneckSnuggle);
        }

        public void ShopTurtleneckModelYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckModelYes.HandOverMoney".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModelYes.FineChoiceSpins".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModelYes.MomentByAndBy".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModelYes.MoreThanMoment".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModelYes.HopeSatisfied".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.Turtleneck), 1);

            ShopClothes();
        }

        public void ShopTurtleneckModelNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckModelNo.ShouldersSlump".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckModelNo.ChangesBackLooks".Localized());

            ShopClothes();
        }

        public void ShopTurtleneckSnuggle()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.CloseFrownCrosses".Localized((Player.IsFemale() && (Save.GetInt("timesKissed", 0) + Save.GetInt("timesYurid", 0) > 0) ? "timid" : "wary")));

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.QuestionCutShort".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.CouldYouStammer".Localized((Save.GetInt("timesDicked", 0) == 0 ? "half-heartedly " : "")));

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.HearYouMe".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.RemainsTense".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.FaceNotVisible".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.AtLastRelaxes".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggle.RewardScratchEar".Localized((!Player.HasCock() && Player.Femininity >= 59 && Save.GetInt("insulted", 0) == 0 ? "cheek" : "shoulder")));

            ClearButtons();

            AddNextButton("Common.Button.JustCuddle".Localized(), ShopTurtleneckCuddle);

            AddNextButton("Common.Button.LesbianCuddle".Localized(), ShopTurtleneckLesbian);

            AddNextButton("Common.Button.CuddleFuck".Localized(), ShopTurtleneckFuck);

            AddNextButton("Common.Button.StandUp".Localized(), ShopTurtleneckStand);
        }

        public void ShopTurtleneckCuddle()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.TherapeuticHolding".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.OpenFieldDream".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.PluckFromCanvas".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.OpenEyesDozeOff".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.HmmnghWord".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.GirlInLapStirs".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.RealizesStillLap".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.StrokeElicitsMumble".Localized((Save.GetInt("insulted", 0) == 0 && (Player.Femininity >= 59 || Player.IsFemale != null) ? "[say:Hmmn... Thank you, [name],] she says, making you wonder what for. [say:For... this. I, ah... I believe 'twas rather a balm upon the spirits.] [if (isfemale) {She can't hide her following tinge of diffidence and decides to simply say nothing further.|It seems she genuinely enjoyed that.}] The two of you remain for a bit longer, enjoying each other's embrace" : "Nothing more comes after that, though, and you simply enjoy the embrace for a bit longer")));

            OutputText("Scenes.Marielle.ShopTurtleneckCuddle.BuyItNow".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            Player.ChangeFatigue(-20);

            PurchaseMenu(ShopTurtleneckSnuggleYes, ShopTurtleneckSnuggleNo);
        }

        public void ShopTurtleneckLesbian()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.FallenAsleepArms".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.NotStirSlipHand".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.EnvisionSeamstress".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.JerksOutFantasy".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.StayYourselfNot".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.LipsAgainstNeck".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.SomeoneElseFingers".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.WaitHoldPlease".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.FlushedBrightRed".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.SloppyButGlorious".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.HandsRakeHead".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.WhimpersThenLocks".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.ThrowIntoEcstasy".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.DescendFromBliss".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.TriesToSpeak".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.SitEnjoyingEmbrace".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.FirstToRise".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.DrawnToSweater".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckLesbian.StillBuyItQuestion".Localized());

            Player.Orgasm("Vaginal");

            MarielleSex();

            View.Time.PassMinutes((int)Round((1) * 60));

            PurchaseMenu(ShopTurtleneckSnuggleYes, ShopTurtleneckSnuggleNo);
        }

        public void ShopTurtleneckFuck()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckFuck.CantIgnoreErection".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckFuck.ShiftArmorAside".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckFuck.MumblesDozedOff".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckFuck.WhatOhUhm".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckFuck.LiftHemlineHoist".Localized());

            if (VaginalAvailable())
            {
                OutputText("Scenes.Marielle.ShopTurtleneckFuck.PlaceholderFragment".Localized(
                    Save.GetInt("timesDicked", 0) > 1 ? "Any imminent protest " + (Save.GetInt("timesDicked", 0) > 2 ? "dies in her throat" : "is swallowed back down") + " when you push your tip into her folds. Only a " + (Save.GetInt("timesDicked", 0) > 3 ? "choked" : "tremulous") + " gasp makes its way out, her fingers digging into your shoulders with a strength that is rivalled by how her passage--tight, cold, and bone-dry--screams against your intrusion. " + (Save.GetInt("timesDicked", 0) > 3 ? "Clenching her teeth and squeezing" : "Squeezing") + " her eyes shut, she leaves you no choice but to wait." : "[say:Wha-- Tarry, [name], I--] Her protest turns into a wince as you push your tip into her folds. She tenses up, her fingers digging into your shoulders with surprising strength. She's tight. And cold. And bone-dry. [if (corruption < 50) {A pang of guilt hits you at her pained reaction, and you reach to stroke her hand. You'll give her all the time she needs.|You'd like to push in further, but that's not going to happen with her clenching you like a virgin. You'll just have to wait.}]"
                ));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.ControlledBreathing".Localized((Save.GetInt("timesDicked", 0) > 2 ? "or at least the attempt of it, " : ""), (Save.GetInt("timesDicked", 0) > 1 ? "steel-blue of her " + (Save.GetInt("timesDicked", 0) > 3 ? "eyes remains barred to you as she refuses to Open them again" : "watery, re-opened eyes wanders somewhere far away, away from you and her obvious distress") + ". The seamstress yields no sign of consent, but if she hasn't stopped you now, you'll take her silence as such, and so you trust gravity to get the better of her vice-like grip on your dick and" : "seamstress gives you the slightest of nods, despite her more than obvious distress. The steel-blue eyes behind her glasses are Still watery, but with her vice-like grip on your dick softening, you trust gravity to"), (Save.GetInt("timesDicked", 0) > 3 ? "Grimacing into the void, " : (Save.GetInt("timesDicked", 0) > 1 ? "Wincing, " : "")), (Save.GetInt("timesDicked", 0) != 0 ? "still " : ""), (Save.GetInt("timesDicked", 0) > 2 ? "Still fighting every inch, her cool inner walls succumb to" : "With reluctance, her cool inner walls accept"), (Save.GetInt("timesDicked", 0) > 3 ? "flashing a grimace into the void before she expels it with another exhale" : (Save.GetInt("timesDicked", 0) > 1 ? "winces suddenly, and then breathes another long exhale" : "opens her mouth, but appears to decide otherwise and closes it again"))));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.BuriedDeepWithin".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.SaysNothing".Localized(
                    Save.GetInt("timesDicked", 0) > 2 ? "giving not the slightest hint of reciprocation, her arms hanging half-limp, half-wedged between you as you hug her doll-like body to you and settle her against your [if (Tallness > 71) {chest|shoulder}], enjoying her " + (Save.GetInt("timesDicked", 0) > 3 ? "utter " : "") + "passivity for what it is." : "only grunting quietly and shuffling about this way and that, which in turn serves to stimulate you with small, reflexive squeezes. You bite back a groan and guide her to settle [if (Tallness > 71) {against your chest|onto your shoulder}], limp hands grabbing your back. That, you take as your cue."
                ));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.DrawOutPushBack".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.OneSidedLovemaking".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.ImmersedInSnugness".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.InstinctTakesPace".Localized((Save.GetInt("timesDicked", 0) > 3 ? "a pair of sleeve-bundled arms snaps itself into your way, marking a clear limit to what you can do. Her act" : "she's quick to guess your aim and pointedly evade you, the line clear. Her gesture")));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.FrigidQuimWarmth".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.ChillyExhalations".Localized(
                    Save.GetInt("timesDicked", 0) > 2 ? "Your fingers can't decide any more where next to indulge themselves, Marielle's" : "Marielle's fingers " + (Save.GetInt("timesDicked", 0) > 1 ? "have at some point slipped off" : "dutifully hold onto") + " your back, her"
                ));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.OrgasmSeizesWrap".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.PantingComeDown".Localized((Save.GetInt("timesDicked", 0) > 3 ? "near-" : "")));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.SlideOutDiscomfort".Localized(
                    Save.GetInt("timesDicked", 0) > 3 ? "Her eyes are open once more, one sleeve currently wiping them clean with careful dabs. In the midst of it, she stops and mumbles something into the knitted fabric, then glances over at you. You're not quite sure what she's trying to communicate with that look, your tiredness not allowing it, but she seems all but happy. An awkward moment passes until she speaks up by muttering, [say:Could you?] Get up, right. You do so and" : "[saystart]..." + (Save.GetInt("timesDicked", 0) > 1 ? "Once more, such...[sayend] The next words she mutters don't quite make it to you ears, though they sounded" : "You are in the height of a ruffian, know you that?[sayend] she mutters,") + " anything but happy. You're too tired right now for any other response than simply stroking her long, blonde hair, the will to get up dwindling with each blissful second. But eventually, you resolve yourself. With a last pat on her head, you"
                ));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.CumTricklesGround".Localized(
                    Save.GetInt("timesDicked", 0) > 1 ? "has her gaze " + (Save.GetInt("timesDicked", 0) > 3 ? "locked on its staggered flow, as if fallen into some deeper rumination. One [if (cumquantity > 900) {glob|driblet}] decides to run down her thigh, and her expression takes a fleeting turn for the worse, but she promptly" : "on its staggered flow for just a moment, but ") + " shakes her head away from it and fishes for a handkerchief. Her cleaning her privates gives you the time" : "grumbles and complains in a mumbly voice, [say:Ahh... what ordure. Ooh no, pray bemoil it not...] She reaches for a handkerchief and hastily starts cleaning her privates while you turn"
                ));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.GlanceBackNotice".Localized(
                    Save.GetInt("timesDicked", 0) > 3 ? "glumness" : (Save.GetInt("timesDicked", 0) > 1 ? "dismay" : "laments")
                ));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.BuyItQuestion".Localized());

                View.Time.PassMinutes((int)Round((1) * 60));

                Player.Orgasm("Dick");

                MarielleSex(true);

                PurchaseMenu(ShopTurtleneckSnuggleYes, ShopTurtleneckSnuggleNo);
            }
            else
            {
                OutputText("Scenes.Marielle.ShopTurtleneckFuck.EyesSpringOpen".Localized((Save.GetInt("timesDicked", 0) != 0 ? "more" : "further")));

                OutputText("Scenes.Marielle.ShopTurtleneckFuck.OversteppedBoundary".Localized(
                    Save.GetInt("timesDicked", 0) != 0 ? "She's let you do her in the past, but it was " + (Save.GetInt("timesDicked", 0) > 2 ? "increasingly" : "always") + " obvious she wasn't too keen on any of it. Perhaps you have exhausted what scant willingness she had." : "Well, it makes sense that this would be as far as her comfort zone extends."
                ));

                Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

                ClearButtons();

                AddNextButton("Common.Button.GetUp".Localized(), () => ShopTurtleneckFuckReject(false));

                AddNextButton("Common.Button.Apologize".Localized(), () => ShopTurtleneckFuckReject(true));
            }
        }

        public void ShopTurtleneckFuckReject(bool apologized)
        {
            ClearOutput();

            if (apologized)
            {
                OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.ApologyDiffuses".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.SpeakNoWhit".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.QuiteFineWithThat".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.GetUpFromGround".Localized());

                OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.NothingSheSays".Localized());
            }

            OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.TimeOfReturn".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckFuckReject.DoYou".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            PurchaseMenu(ShopTurtleneckSnuggleYes, ShopTurtleneckSnuggleNo);
        }

        public void ShopTurtleneckStand()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckStand.StayMinuteLonger".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckStand.UnintelligibleMumble".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckStand.LiftsGlassesRubs".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckStand.ThatWasRefreshing".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckStand.StillSittingGround".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckStand.WantToBuyIt".Localized());

            View.Time.PassMinutes((int)Round((.5) * 60));

            Player.ChangeFatigue(-10);

            PurchaseMenu(ShopTurtleneckSnuggleYes, ShopTurtleneckSnuggleNo);
        }

        public void ShopTurtleneckSnuggleYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggleYes.BuyThatPullover".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggleYes.PardonMeBlinks".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggleYes.NoteDownSizes".Localized());

            OutputText("Scenes.Marielle.ShopTurtleneckSnuggleYes.RatherLargeByDesign".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.Turtleneck), 1);

            SexReturn("[pg]While you're appraising it, the seamstress covers her mouth in a small yawn, and a glance outside confirms that it's gotten fairly late by now. You suppose you should get back to camp, so you stow the pullover away after finding no flaws with it and wish her a good night.[pg][say:Oh.] Marielle looks up, apparently having drifted away for a second. [say:Ah, hmm... \" + (player.isFemale() && saveContent.insulted == 0 ? \"Aye\" : \"Yes\") + \", I bid you a good night likewise, [name],] she says with a slight bow before masking another yawn. You leave her to prepare for sleep and exit the sanctuary, throwing a glance back to see her stretch herself before you [if (singleleg) {slide|step}] outside.\", \"[pg]You can't find any flaws with it as you appraise the pullover, but when you stow it away and turn your attention back to the seamstress, you realize how spent she looks. You've kinda lost track of the hours through this, but it should be time to get back to camp, so you call it here and say your farewell.[pg][say:Oh.] Marielle looks up, having drifted away for a second. [say:Ah, hmm... \" + (player.isFemale() && saveContent.insulted == 0 ? \"Aye\" : \"Yes\") + \", I bid you a good day likewise, [name],] she says, getting up to deliver a slight curtsy. Leaving her to rest, you exit the sanctuary, throwing a glance back to see the girl stretch herself before you [if (singleleg) {slide|step}] outside.\", \"\", shopClothes.bind()));\n    }\n\n    public function shopTurtleneckSnuggleNo() {\n        clearOutput();\n        sexReturn(\"It's late, late enough that Marielle's drowsiness is beginning to show, so you exchange farewells with her and leave her to catch some real sleep. You probably shouldn't stay up for that much longer, either.\", \"You've somewhat lost track of time throughout all of this, but you're sure you've been here for a while now, and the slight sluggishness showing in Marielle's movements as she re-dresses and collects herself seems to support that assessment. Calling it here, you exchange farewells with the girl and leave her to get some proper rest.\", \"You leave Marielle to collect herself and re-dress while you take another look at her wares.\", shopClothes.bind());\n    }\n\n    public function shopCloak() {\n        clearOutput();\n        outputText(\"Carefully, you take out a large sheet of black fabric and hold it out in front of you. A cloak, it appears. Rather plain and without any embellishments, but still lustrous and soft to the touch, evidently made of high-quality fabric despite its simplicity.\");\n        outputText(\"[pg][say:One I once fashioned for myself,] Marielle explains, sizing it up with a look of dissatisfaction, [say:in quest of aught to enshroud myself with, to ward off rays and eyes unwanted. Alas, 'twas a little too, ah... caliginous. Undead may I be, yet by evidence not befitted for such sable garments with]--she reaches down and pops up its neck piece--[say:upstanding collars.] She may be right; she doesn't look like a lady of the night who[if (ischild) {'s out to suck your blood| sucks the blood of little children}]. An amused smile crosses her lips.\");\n        outputText(\"[pg][say:Well, be I what I be may, 'tis quite resilient and staunch, although can I not advise taking it in lieu of armor true, for 'tis solely meant to complement,] she says, folding her hands together. [saystart]Ah, but, well, you may don it howsoever you wish, of course, that is entirely for you to choose.\");\n        outputText(\"[pg]150 gems is what I ask of you, wish you to buy it.[sayend]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 150;\n        purchaseMenu(shopCloakYes, shopCloakNo);\n    }\n\n    public function shopCloakYes() {\n        clearOutput();\n        outputText(\"You hand Marielle the gems.\");\n        outputText(\"[pg][say:Ah, certainly. I am glad to see it find a new and--I hope--better owner in you, [name].] She looks you up and down, likely measuring you with her eyes. [say:Hmm, it should fit you quite fine, I ween. Although, of course, should I be mistaken, be not afeard to correct me.]\");\n        doTransaction();\n        saveContent.itemsBought |= BOUGHT_BLACK_CLOAK;\n        inventory.takeItem(armors.B_CLOAK, shopClothes.bind());\n    }\n\n    public function shopCloakNo() {\n        clearOutput();\n        outputText(\"You fold the cloak and put it back into the box.\");\n        shopClothes();\n    }\n\n    public function shopCheer() {\n        clearOutput();\n        outputText(\"You fish out a striking set of clothing that was buried near the bottom of the chest: a short skirt and a sleeveless top, both pink and white in color and leaving the wearer's midriff and thighs exposed. This doesn't exactly look like something you'd have expected to be in her inventory.\");\n        outputText(\"[pg][say:Ah.] Marielle looks somewhat hesitant, but continues, [say:Well, that is... something I made on... behest, yes, quite a long while agone.] She doesn't seem to be inclined to tell you who commissioned the thing, nor why she's still holding onto it. Instead, she looks like she's searching for something. Standing up and walking over to another pile of boxes, she rummages through them until she pulls out two large, bushy balls and hands them to you before sitting down again. You're not sure what to do with these, but they seem to somewhat fit with the rest of the set, color-wise. Perhaps you're supposed to wave them around, or maybe throw them; if anything, they should be a decent distraction.\");\n        outputText(\"[pg][say:Well, ah... 200 gems and it shall all be yours, wish you so.]\");\n        outputText(\"[pg]Do you really want this?\");\n        currentPrice = 200;\n        purchaseMenu(shopCheerYes, shopCheerNo, shopCheerModel);\n    }\n\n    public function shopCheerYes() {\n        clearOutput();\n        outputText(\"You put the fuzzy balls aside to hand her the money.\");\n        outputText(\"[pg][say:Oh? Oh, you... do?] An expression of doubt--or judgement?--flits over her face before she can expel it and hastily take the offered money with a smile. [say:I, ah, will needs make a few simple alterations aforehand.] She pulls a pair of measuring tapes from the table and moves over to you. [say:Hold you still, please.] You let her note down your sizes and give her some time to work.\");\n        outputText(\"[pg]Not long after, she beckons you back and gives you the tailored outfit.\");\n        outputText(\"[pg][say:Well, that should be all. If it yet pinch you, pray say so, and I shall correct it.]\");\n        outputText(\"[pg]It looks good, but you're still in the dark as to their name and purpose, so you ask Marielle what these fluffy things are.\");\n        outputText(\"[pg][say:Oh, uhm...] Her eyebrows pull closer together, and she seems to be racking her brain for a while, but doesn't arrive anywhere. So, she turns to the notes she keeps on her desk. But they, too, apparently give no answer to your question, and the increasingly absorbed seamstress starts skimming over her crates until she finds the right one and sifts through its contents. You hear the rustling of paper on paper before a soft [say:Ah] marks her success.\");\n        outputText(\"[pg][say:'Pom-pom'.] Excuse you? [say:Uhm, 'pom-poms', 'tis their, ah, name,] Marielle says as she turns back to you. By way of proof, she presents you the stack of designs she found. The topmost one is a detailed depiction of one such ball in question, complete with illustrations on how exactly it's made. And true enough, underneath it stands in swirly handwriting: 'Pom-Pom', beneath which is then a small series of drawings of a curvy woman doing various athletic poses with two of those in hand.\");\n        outputText(\"[pg]You suppose that solves the mystery.\");\n        doTransaction();\n        saveContent.itemsBought |= BOUGHT_CHEER_OUTFIT;\n        inventory.takeItem(armors.CHROTFT, shopClothes.bind());\n    }\n\n    public function shopCheerNo() {\n        clearOutput();\n        outputText(\"You return the fuzzy balls and stow the top and skirt back into the chest.\");\n        shopClothes();\n    }\n\n    public function shopCheerModel() {\n        clearOutput();\n        outputText(\"[say:Oh, I, ah...] She lets her voice peter out to consider, though her expression already tells you that whatever she's going to say next won't be too favorable. [say:I would rather... not.] She won't model that for you? You can guess why, but still ask.\");\n        outputText(\"[pg][say:Well, 'tis, ah...] A single hand loosely gestures around the showy and showing outfit as the seamstress visibly grasps for the right words, almost despairing over them, until a sigh unseals her lips. [say:'Tis much too unseemly. My apologies, [name], but I must refuse you. If there be aught else I may assist you with, then pray tell, but this I shall don not.]\");\n        outputText(\"[pg]That was a fairly firm rejection. As much fun as it would be to see her in this rather revealing set, you doubt she'll let herself be persuaded, here.\");\n        outputText(\"[pg]Would you buy it regardless?\");\n        purchaseMenu(shopCheerYes, shopCheerNo);\n    }\n\n    public function shopBallet() {\n        clearOutput();\n        outputText(\"This one would catch anyone's eye as soon they looked into the chest. You thought it was some sort of ballet dress--complete with tutu--at first, but you've never seen one this elaborate and voluminous. It's frilly, terribly girly, and bristling with little bows, ribbons, and ruffles.\");\n        outputText(\"[pg][say:Ah, uhm, yes,] Marielle half-chuckles in a failed attempt to mask a hint of embarrassment. [say:Why, 'tis... charming, is it not? A design most novel and ancient at once, and I would I had the, ah, luminosity to right advertise it mine own invention...] You ask what she means by that. [say:Ah, you noted yonder statues, yes? 'Tis fashioned upon one quite particular of these umwhile girls' attires.] You take a glimpse into the temple, at the array of statuettes lining its walls. [say:'Tis truest shame many thereof would have so fallen unto ruin, for their raiments are verily a sight to behold. Was I but arrived here in better season...]\");\n        outputText(\"[pg]The seamstress appears quite taken as she then delves deeper into the different designs, their possible philosophies and functionalities, and how she might recreate at least some of them.\");\n        outputText(\"[pg][say:Ah!] She finally stops herself. [say:But, uhm, well, to repair to present, this one is something lightsome and... adorable, one may say, yes?] You're not disagreeing. [say:Howbeit, alas, rather esurient for the most finest of fabrics...] She adjusts her glasses and glances towards a stack of cloth rolls.\");\n        outputText(\"[pg][say:Thus, ah... 2400 gems is as humble an offer as may I present.]\");\n        outputText(\"[pg]Steep, but probably worth the investment, if this particular style is your thing. Do you want to buy it?\");\n        currentPrice = 2400;\n        purchaseMenu(shopBalletYes, shopBalletNo, shopBalletModel);\n    }\n\n    public function shopBalletYes() {\n        clearOutput();\n        outputText(\"[say:Oh, wonderful!] Marielle exclaims, clasping her many hands over the offered bundle of gems. [say:Just pray allow me set down your measurements, lest it be not the height of absoluteness and excellence!] You let her take your sizes as she happily hums to herself and gets to work. [say:'Twill but take a moment, fret not.]\");\n        outputText(\"[pg]Her prediction proves right, and she soon calls you over again to present you the frilly dress with a wide smile.\");\n        outputText(\"[pg][say:Be not afeard to ask, if aught be amiss. I shall be more than glad to make further adjustments.]\");\n        doTransaction();\n        inventory.takeItem(armors.BALLETD, shopClothes.bind());\n    }\n\n    public function shopBalletNo() {\n        clearOutput();\n        outputText(\"You fold it and put it back into the chest, a soft [say:Hmm...] leaving Marielle's lips.\");\n        shopClothes();\n    }\n\n    public function shopBalletModel() {\n        clearOutput();\n        if ((saveContent.modeled & MODELED_BALLET_DRESS) != 0) {\n            outputText(\"[say:I, ah... know not wherefore you would want me garb myself in this once more,] Marielle says, [if (femininity >= 59) {a note of saffron starting to crawl up her features.|nervously fiddling with her skirt.}] [say:But, well... I suppose I, ah, shall...]\");\n            outputText(\"[pg]You give her the dress and some space to change. She takes rather long, fighting probably with her embarrassment, as well as the multi-layered petticoat and many ribbons, but eventually turns around to face you, looking to have won most of her battles.\");\n            outputText(\"[pg][say:Uhm... well?]\");\n            outputText(\"[pg]The dress appears even girlier on her narrow frame, making her look like a fairytale-ballerina, although a very pale, scarred one. It seems designed for a small bust, since even Marielle doesn't struggle in that area, and despite the plentiful bows and ribbons, nothing is getting in the way of anything. The wide petticoat leaves her legs largely bare and thus doesn't restrict her movement either while she performs a slow, demonstrative twirl for you.\");\n            outputText(\"[pg]Overall a very distinctive, feminine piece of clothing, but do you want to buy it?\");\n            purchaseMenu(shopBalletModelYes.bind(), shopBalletModelNo.bind());\n            addNextButton(\"Lift Skirt\", shopBalletLiftSkirt).hint(\"Have Marielle lift her skirt for your viewing pleasure.\").disableIf(!((saveContent.talks & TALK_SEX) != 0));\n        } else {\n            outputText(\"Marielle looks on--unblinking--for several seconds before [if (femininity >= 59) {a bright, scarlet blush breaks out and spreads over almost her entire snow-white body as she lets her eyes escape to the ground in more than|nervously fiddling with her dress and staring to the ground in}] apparent embarrassment.\");\n            outputText(\"[pg][say:I, ah... Uhm... Are you... of surest mind?] The seamstress seems awfully hesitant, so you ask what the issue is. [say:Well, 'tis...] She glances up at it, but holds back whatever she wanted to say and instead shakes her head.\");\n            outputText(\"[pg][say:No, 'tis naught.] She then hems faintly to combat her abashment before taking the dress from your hands and motioning you to give her some space while she puts it on. She takes rather long, fighting with the layered petticoat and many ribbons, but eventually turns around to face you, [if (femininity >= 59) {cheeks still flushed crimson|though pointedly dodging your eyes}].\");\n            outputText(\"[pg][say:W-Well, how... look I?]\");\n            outputText(\"[pg]The dress appears even girlier on her narrow frame than it did by itself, making her look like a fairytale-ballerina, although a very pale, scarred one. It seems designed for a small bust, since even Marielle doesn't struggle in that area. And despite the plentiful bows and ribbons, nothing gets into her way as she completes a slow, demonstrative twirl. Plus, the wide skirt leaves her legs bare for unrestricted movement.\");\n            outputText(\"[pg]But how does [i:she] look? That is what she asked, if you heard right. Marielle is visibly growing more and more strung up as the contemplative silence stretches on.\");\n            saveContent.modeled |= MODELED_BALLET_DRESS;\n            menu();\n            addNextButton(\"Adorable\", shopBalletModelFirst.bind(0)).hint(\"She's a cute girl in a cute dress.\");\n            addNextButton(\"Good\", shopBalletModelFirst.bind(1)).hint(\"An honest and neutral answer.\");\n            addNextButton(\"Ridiculous\", shopBalletModelFirst.bind(2)).hint(\"A bit harsh, but she does look comical.\");\n            addNextButton(\"Like Shit\", shopBalletModelFirst.bind(3)).hint(\"A straight-up insult.\");\n        }\n    }\n\n    public function shopBalletModelFirst(answer:Int) {\n        clearOutput();\n        outputText([\n            \"Well, she would be right at home on a ballet stage, playing a princess or a fairy. You tell the girl as much.[pg][if (femininity >= 59) {You didn't think her blush could grow any deeper. Clearly, you were mistaken. [saystart]T-|She meets your eyes, brief confusion washed away by childish glee. [saystart]}]Truly? I... ah, uhm... That... Thank you![sayend] Marielle finally blurts out, quickly forgetting her previous embarrassment. [say:Ah, but, the dress. Why of course, allow me...] Accompanied by a happy hum, she twirls around for you once more to show it off from all sides, the petticoat slightly rising with the motion. It's pretty floaty.[pg][say:Well?][pg]Do you want to buy it?\",\n            \"She looks pretty cute. Not nearly as bad as she probably thought she would. You tell Marielle as much.[pg][if (femininity >= 59) {She blinks, then smiles at you, cheeks still rosy-red.|She looks up at you, a wide smile spreading her face.}] [say:You...] The girl trails off. [say:I... Thank you.] She gives a small curtsy, looking positively giddy with barely contained excitement, but then stops and reins herself in.[pg][say:Aye, the dress. My apologies.] She twirls around for you once more, the floaty hem of the garment rising slightly with the motion, to show it off from all sides. Quite airy.[pg][say:Well?][pg]Do you want to buy it?\",\n            \"It really doesn't fit her. She may look cute, but mostly because it looks silly on her. You openly tell her as much.[pg][say:I... Ah...] She blinks rapidly and casts her eyes downwards as her face falls. [say:Of course, you are quite right. That I do, do I not...] Marielle gives a weak, self-deprecating chuckle. She stands there, unsure of what point on the ground to stare at, her shoulders slumped, and kneads the hem of her skirt.[pg]The girl doesn't look like she is going to say anything more, and the silence stretches on again.[pg]Do you still want to buy the dress?\",\n            \"Well, she looks like absolute shit. It might be a ballet dress, but she's more like a clown than a ballerina in that thing. A pretty pathetic clown, at that. Without mincing words, you tell her as much.[pg]Marielle stares at you. She opens her lips as if to say something, but instead clenches her teeth, swallows, and casts her eyes downwards. Standing there, four hands gripping the hem of her dress, she gazes to the ground.[pg][say:I... see.] Her soft voice is barely more than a whisper, but still clear as it faintly echoes through the abandoned temple.[pg]She nods to herself, then takes a step back and drops down onto her stool. Without a further word, she busies her fingers with the nearest thing they can reach: a colored thread and a needle. Her long hair has fallen over her face, partially obscuring the listlessness of her expression as she starts to stab a piece fabric. You attempt to get her attention, but receive nothing but silence in answer.[pg]Shrugging, you leave the despondent seamstress to her moodiness and exit the temple. The oppressive humidity has you grumbling while you wade through the muddy quagmire back to your camp.\"\n        ][answer]);\n        purchaseMenu(shopBalletModelYes.bind(answer == 2), shopBalletModelNo.bind(answer == 2));\n        addNextButton(\"Lift Skirt\", shopBalletLiftSkirt).hint(\"Have Marielle lift her skirt for your viewing pleasure.\").disableIf(!((saveContent.talks & TALK_SEX) != 0) || answer > 1);\n        if (answer == 3) {\n            saveContent.mockedDress = true;\n            doNext(camp.returnToCampUseOneHour);\n        }\n    }\n\n    public function shopBalletModelYes(rude:Bool = false) {\n        clearOutput();\n        if (rude) {\n            outputText(\"You indeed still want to buy it.\");\n            outputText(\"[pg]Marielle looks up at you, frowning. [say:You... do?] she cautiously asks. You simply hand her the money.\");\n            outputText(\"[pg][say:I... Oh, hm. I, ah, must needs make some adjustments, then, first. Hold you still awhile, please.] Despite her dampened spirits, she quickly measures your sizes, then changes out of the dress and into her usual outfit to get to work.\");\n            outputText(\"[pg]A little while later, she gets up and presents you the tailored garment.\");\n            outputText(\"[pg][say:Well, that should be all; 'twill fit quite fine now. Pray tell if aught be amiss, however.]\");\n        } else {\n            outputText(\"You're definitely buying that.\");\n            outputText(\"[pg][say:Ah, lovely!] Marielle exclaims, beaming at you. [say:But, ah...] She looks you up and down. [say:'Twill require a few adjustments, which shall needs be done with excellence and expedition exceeding. A moment.] The girl quickly takes out two measuring tapes and notes down your sizes before changing back into her summer dress and getting to work with an excited, [say:By and by, but a minute!]\");\n            outputText(\"[pg]It indeed doesn't take her long to tailor the garment to you, so you soon hear her calling you back over to proudly present you her finished work. You find not the slightest fault with it, and promptly deposit the asked-for money into her many hands.\");\n        }\n        doTransaction();\n        inventory.takeItem(armors.BALLETD, shopClothes.bind());\n    }\n\n    public function shopBalletModelNo(rude:Bool = false) {\n        clearOutput();\n        if (rude) {\n            outputText(\"Marielle just nods, eyes still downcast, and turns to undress. You're pretty sure you hear her sigh.\");\n            outputText(\"[pg]While she changes into her usual clothes, you take another look at the chests before you.\");\n        } else {\n            outputText(\"[say:Ah, oh. Hmm... that is... unfortunate.] She looks a little deflated, but quickly regains her [if (silly) {yay|smile}]. [say:But, well, pray inform me, should you change your mind.]\");\n            outputText(\"[pg]While she switches back into her usual outfit, you take another look at her wares.\");\n        }\n        shopClothes();\n    }\n\n    public function shopBalletLiftSkirt() {\n        clearOutput();\n        outputText(\"A fluffed skirt like that just begs to be taken advantage of, and you know just the thing. Trying not to let too much of your lechery show, you tell Marielle to lift it.\");\n        outputText(\"[pg][say:...Pardon me?] She looks like she didn't understand you. You're pretty sure she did, but you repeat the command regardless.\");\n        outputText(\"[pg][say:But where-- Oh. Oh...] Perhaps she didn't, after all, since only now does \" + (player.femininity >= 59 && !player.hasCock() ? \"an even deeper blush start to spread across her pale skin while she fiddles\" : \"she take a [if (isfemale) {nervous }]half-step back while fiddling\") + \" with a bow on her dress. She glances to the side, as if [if (hascock) {weighing|reaffirming}] something, then grips the hem tight.\");\n        outputText(\"[pg][say:If... If your wish be thus, [name]...] the girl says, sounding \" + (saveContent.timesYurid > 1 && player.isFemale() ? \"anticipative, yet not quite confident\" : \"anything but \" + (saveContent.timesDicked > 1 && player.hasCock() ? \"unguarded\" : (player.hasCock() ? \"excited\" : \"confident");
        }

        public void ShopTurtleneckSnuggleNo()
        {
            ClearOutput();

            SexReturn("It's late, late enough that Marielle's drowsiness is beginning to show, so you exchange farewells with her and Leave her to catch some real sleep. You probably shouldn't stay up for that much longer, either.", "You've somewhat lost track of time throughout all of this, but you're sure you've been here for a while now, and the slight sluggishness showing in Marielle's movements as she re-dresses and collects herself seems to support that assessment. Calling it here, you exchange farewells with the girl and Leave her to get some proper rest.", "You Leave Marielle to collect herself and re-dress while you take another look at her wares.", () => ShopClothes());
        }

        public void ShopCloak()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCloak.LargeBlackFabric".Localized());

            OutputText("Scenes.Marielle.ShopCloak.FashionedForMyself".Localized());

            OutputText("Scenes.Marielle.ShopCloak.ResilientAndStaunch".Localized());

            OutputText("Scenes.Marielle.ShopCloak.AskOneFiftyGems".Localized());

            OutputText("Scenes.Marielle.ShopCloak.DoYou".Localized());

            currentPrice = 150;

            PurchaseMenu(ShopCloakYes, ShopCloakNo);
        }

        public void ShopCloakYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCloakYes.HandMarielleGems".Localized());

            OutputText("Scenes.Marielle.ShopCloakYes.LooksUpAndDown".Localized());

            DoTransaction();

            Save.SetInt("itemsBought", Save.GetInt("itemsBought", 0) | BOUGHT_BLACK_CLOAK);

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BlackCloak), 1);

            ShopClothes();
        }

        public void ShopCloakNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCloakNo.FoldCloakBack".Localized());

            ShopClothes();
        }

        public void ShopCheer()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheer.StrikingClothingSet".Localized());

            OutputText("Scenes.Marielle.ShopCheer.HesitantButContinues".Localized());

            OutputText("Scenes.Marielle.ShopCheer.TwoHundredGems".Localized());

            OutputText("Scenes.Marielle.ShopCheer.ReallyWantThis".Localized());

            currentPrice = 200;

            PurchaseMenu(ShopCheerYes, ShopCheerNo, ShopCheerModel);
        }

        public void ShopCheerYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheerYes.PutBallsAside".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.DoubtOrJudgement".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.BeckonsWithOutfit".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.IfPinchSaySo".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.LooksGoodAskName".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.EyebrowsPullTogether".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.ExcuseYouPomPom".Localized());

            OutputText("Scenes.Marielle.ShopCheerYes.SolvesTheMystery".Localized());

            DoTransaction();

            Save.SetInt("itemsBought", Save.GetInt("itemsBought", 0) | BOUGHT_CHEER_OUTFIT);

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.CheerOutfit), 1);

            ShopClothes();
        }

        public void ShopCheerNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheerNo.ReturnFuzzyBalls".Localized());

            ShopClothes();
        }

        public void ShopCheerModel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopCheerModel.VoicePetersOut".Localized());

            OutputText("Scenes.Marielle.ShopCheerModel.HandGesturesAround".Localized());

            OutputText("Scenes.Marielle.ShopCheerModel.FirmRejection".Localized());

            OutputText("Scenes.Marielle.ShopCheerModel.BuyItRegardless".Localized());

            PurchaseMenu(ShopCheerYes, ShopCheerNo);
        }

        public void ShopBallet()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBallet.CatchesAnyonesEye".Localized());

            OutputText("Scenes.Marielle.ShopBallet.ChucklesMaskingEmbarrassment".Localized());

            OutputText("Scenes.Marielle.ShopBallet.DelvesIntoDesign".Localized());

            OutputText("Scenes.Marielle.ShopBallet.FinallyStopsHerself".Localized());

            OutputText("Scenes.Marielle.ShopBallet.HumbleOfferGems".Localized());

            OutputText("Scenes.Marielle.ShopBallet.SteepButWorthIt".Localized());

            currentPrice = 2400;

            PurchaseMenu(ShopBalletYes, ShopBalletNo, ShopBalletModel);
        }

        public void ShopBalletYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletYes.ClaspsManyHands".Localized());

            OutputText("Scenes.Marielle.ShopBalletYes.PredictionProvesRight".Localized());

            OutputText("Scenes.Marielle.ShopBalletYes.BeNotAfeard".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BalletDress), 1);

            ShopClothes();
        }

        public void ShopBalletNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletNo.FoldBackWithSigh".Localized());

            ShopClothes();
        }

        public void ShopBalletModel()
        {
            ClearOutput();

            if ((Save.GetInt("modeled", 0) & MODELED_BALLET_DRESS) != 0)
            {
                OutputText("Scenes.Marielle.ShopBalletModel.KnowNotWherefore".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.GivesSpaceToChange".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.UhmWellPrompt".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.GirlierOnNarrowFrame".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.DistinctiveFemininePiece".Localized());

                PurchaseMenu(() => ShopBalletModelYes(), () => ShopBalletModelNo());

                AddNextButton("Common.Button.LiftSkirt".Localized(), ShopBalletLiftSkirt);
            }
            else
            {
                OutputText("Scenes.Marielle.ShopBalletModel.UnblinkingForSeconds".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.SurestMindHesitant".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.HemsToCombatAbashment".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.HowLookI".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.GirlierThanAlone".Localized());

                OutputText("Scenes.Marielle.ShopBalletModel.HowDoesSheLook".Localized());

                Save.SetInt("modeled", Save.GetInt("modeled", 0) | MODELED_BALLET_DRESS);

                ClearButtons();

                AddNextButton("Common.Button.Adorable".Localized(), () => ShopBalletModelFirst(0));

                AddNextButton("Common.Button.Good".Localized(), () => ShopBalletModelFirst(1));

                AddNextButton("Common.Button.Ridiculous".Localized(), () => ShopBalletModelFirst(2));

                AddNextButton("Common.Button.LikeShit".Localized(), () => ShopBalletModelFirst(3));
            }
        }

        public void ShopBalletModelFirst(int answer)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletModelFirst.WellSheWouldBe".Localized(
                new[] { "Well, she would be right at home on a ballet stage, playing a princess or a fairy. You tell the girl as much.[pg][if (femininity >= 59) {You didn't think her blush could grow any deeper. Clearly, you were mistaken. [saystart]T-|She meets your eyes, brief confusion washed away by childish glee. [saystart]}]Truly? I... ah, uhm... That... Thank you![sayend] Marielle finally blurts out, quickly forgetting her previous embarrassment. [say:Ah, but, the dress. Why of course, allow me...] Accompanied by a happy hum, she twirls around for you once more to show it off from all sides, the petticoat slightly rising with the motion. It's pretty floaty.[pg][say:Well?][pg]Do you want to buy it?",

            "She looks pretty cute. Not nearly as bad as she probably thought she would. You tell Marielle as much.[pg][if (femininity >= 59) {She blinks, then smiles at you, cheeks still rosy-red.|She looks up at you, a wide smile spreading her face.}] [say:You...] The girl trails off. [say:I... Thank you.] She gives a small curtsy, looking positively giddy with barely contained excitement, but then stops and reins herself in.[pg][say:Aye, the dress. My apologies.] She twirls around for you once more, the floaty hem of the garment rising slightly with the motion, to show it off from all sides. Quite airy.[pg][say:Well?][pg]Do you want to buy it?",

            "It really doesn't fit her. She may look cute, but mostly because it looks silly on her. You openly tell her as much.[pg][say:I... Ah...] She blinks rapidly and casts her eyes downwards as her face falls. [say:Of course, you are quite right. That I do, do I not...] Marielle gives a weak, self-deprecating chuckle. She stands there, unsure of what point on the ground to stare at, her shoulders slumped, and kneads the hem of her skirt.[pg]The girl doesn't look like she is going to say anything more, and the silence stretches on again.[pg]Do you still want to buy the dress?",

            "Well, she looks like absolute shit. It might be a ballet dress, but she's more like a clown than a ballerina in that thing. A pretty pathetic clown, at that. Without mincing words, you tell her as much.[pg]Marielle stares at you. She opens her lips as if to say something, but instead clenches her teeth, swallows, and casts her eyes downwards. Standing there, four hands gripping the hem of her dress, she gazes to the ground.[pg][say:I... see.] Her soft voice is barely more than a whisper, but still clear as it faintly echoes through the abandoned temple.[pg]She nods to herself, then takes a step back and drops down onto her stool. Without a further word, she busies her fingers with the nearest thing they can reach: a colored thread and a needle. Her long hair has fallen over her face, partially obscuring the listlessness of her expression as she starts to stab a piece fabric. You attempt to get her attention, but receive nothing but silence in answer.[pg]Shrugging, you Leave the despondent seamstress to her moodiness and exit the temple. The oppressive humidity has you grumbling while you wade through the muddy quagmire back to your camp." }[answer]
            ));

            PurchaseMenu(() => ShopBalletModelYes(answer == 2), () => ShopBalletModelNo(answer == 2));

            AddNextButton("Common.Button.LiftSkirt".Localized(), ShopBalletLiftSkirt);

            if (answer == 3)
            {
                Save.SetBool("mockedDress", true);

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void ShopBalletModelYes(bool rude = false)
        {
            ClearOutput();

            if (rude)
            {
                OutputText("Scenes.Marielle.ShopBalletModelYes.StillWantToBuy".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelYes.CautiouslyAsks".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelYes.NeedsAdjustments".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelYes.PresentsTailoredGarment".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelYes.FitQuiteFine".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopBalletModelYes.DefinitelyBuying".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelYes.BeamingExclaims".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelYes.TailorsQuickly".Localized());
            }

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BalletDress), 1);

            ShopClothes();
        }

        public void ShopBalletModelNo(bool rude = false)
        {
            ClearOutput();

            if (rude)
            {
                OutputText("Scenes.Marielle.ShopBalletModelNo.NodsEyesDowncast".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelNo.ChangesIntoUsual".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopBalletModelNo.LittleDeflated".Localized());

                OutputText("Scenes.Marielle.ShopBalletModelNo.SwitchesBack".Localized());
            }

            ShopClothes();
        }

        public void ShopBalletLiftSkirt()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletLiftSkirt.SkirtBegsAdvantage".Localized());

            OutputText("Scenes.Marielle.ShopBalletLiftSkirt.DidntUnderstand".Localized());

            OutputText("Scenes.Marielle.ShopBalletLiftSkirt.PerhapsNotAfterAll".Localized((Player.Femininity >= 59 && !Player.HasCock() ? "an even deeper blush start to spread across her pale skin while she fiddles" : "she take a [if (isfemale) {nervous }]half-step back while fiddling")));

            OutputText("Scenes.Marielle.ShopBalletLiftSkirt.IfWishBeThus".Localized(
                Save.GetInt("timesYurid", 0) > 1 && Player.IsFemale() ? "anticipative, yet not quite confident" : "anything but " + (Save.GetInt("timesDicked", 0) > 1 && Player.HasCock() ? "unguarded" : (Player.HasCock() ? "excited" : "confident"))
            ));

            OutputText("Scenes.Marielle.ShopBalletLiftSkirt.YesOfCourse".Localized());

            MarielleSex();

            ClearButtons();

            AddNextButton("Common.Button.JustWatch".Localized(), ShopBalletWatch);

            AddNextButton("Common.Button.LickHer".Localized(), ShopBalletLick);

            AddNextButton("Common.Button.MakeMast".Localized(), ShopBalletMast);
        }

        public void ShopBalletWatch()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletWatch.JustEnjoyView".Localized());

            OutputText("Scenes.Marielle.ShopBalletWatch.SoundsUnsure".Localized());

            OutputText("Scenes.Marielle.ShopBalletWatch.SnapsBackToCrotch".Localized());

            OutputText("Scenes.Marielle.ShopBalletWatch.DecidesItsEnough".Localized((Player.Femininity >= 59 && !Player.HasCock() ? "fidgeting" : "frowning")));

            OutputText("Scenes.Marielle.ShopBalletWatch.FlushedAbsentMinded".Localized());

            OutputText("Scenes.Marielle.ShopBalletWatch.DoYou".Localized());

            View.Time.PassMinutes((int)Round((.5) * 60));

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            PurchaseMenu(ShopBalletLiftYes, ShopBalletLiftNo);
        }

        public void ShopBalletLick()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletLick.UnspoiltFlower".Localized());

            OutputText("Scenes.Marielle.ShopBalletLick.IgnoreHerNoseClose".Localized());

            if (Player.IsFemale() || (Player.IsGenderless() && Player.Femininity >= 59))
            {
                OutputText("Scenes.Marielle.ShopBalletLick.LickAgainGasp".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.LikeBowlOfCream".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.HowLongWillYou".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.BrokenGaspsMoans".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.TongueLovingNoises".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.WhimperTensesUp".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.WhinesLurchesOver".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.FlopsOntoStool".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.ShopBalletLick.SecondLickNoTerm".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.LocateLoveButton".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.FingerFindsEntry".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.IncredibleClamping".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.LostInOwnWorld".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.SoundsWorriedAsks".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.DigitStillInside".Localized());

                OutputText("Scenes.Marielle.ShopBalletLick.LessQuestionThanStatement".Localized());
            }

            OutputText("Scenes.Marielle.ShopBalletLick.LeftYouQuiteHot".Localized());

            OutputText("Scenes.Marielle.ShopBalletLick.FrillyDressSparked".Localized());

            View.Time.PassMinutes((int)Round((.75) * 60));

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            PurchaseMenu(ShopBalletLiftYes, ShopBalletLiftNo);
        }

        public void ShopBalletMast()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletMast.AppraiseHerPrivates".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast.StuckInLoop".Localized((Save.GetInt("timesYurid", 0) > 2 ? "W-We" : "I-I")));

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.ShopBalletMast.SuddenlyFreezes".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast.ResoluteApology".Localized((Save.GetInt("timesDicked", 0) > 1 ? "shall do not" : "cannot do")));

                OutputText("Scenes.Marielle.ShopBalletMast.PetticoatFalls".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast.AfeardNotNo".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast.WantToPrompt".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                View.Time.PassMinutes((int)Round((1 / 3) * 60));

                PurchaseMenu(ShopBalletLiftYes, ShopBalletLiftNo);
            }
            else
            {
                OutputText("Scenes.Marielle.ShopBalletMast.StammeringStops".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast.GladToHearThat".Localized((Save.GetInt("timesYurid", 0) > 3 ? "This time, through the haze of embarrassment, she nods. [say:I... I suppose it... possible. Aye.]" : "It takes her some time to stave off the second wave of embarrassment and prevent it from overcoming her, but when she does, she manages to squeeze out a nod. [say:I... I suppose 'twould not be entirely... impossible.]")));

                OutputText("Scenes.Marielle.ShopBalletMast.UnableToFaceYou".Localized((Save.GetInt("timesYurid", 0) > 2 ? "She stops just a hair's breadth away and glimpses into your eyes, as if seeking approval. Getting a smile from you, she inhales and" : "But a hair's breadth away, she hesitates. Before you can formulate anything that might boost her confidence however, she closes her eyes, takes a invigorating inhale, and at last ")));

                OutputText("Scenes.Marielle.ShopBalletMast.SlowlyMethodically".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast.SecondHandAbandons".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast.BreathHitches".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Masturbate".Localized(), ShopBalletMast2);

                AddNextButton("Common.Button.Watch".Localized(), ShopBalletMastWatch);
            }
        }

        public void ShopBalletMast2()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletMast2.FingersSneakDown".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast2.HesitatesThenFollows".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast2.SnapBackIntoMotion".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast2.EverDeeperFaster".Localized((SceneInstance<CeraphScene>.Get().HasExhibition() ? "sate that familiar urge to show her your everything" : "allow no lull in your endeavors")));

            OutputText("Scenes.Marielle.ShopBalletMast2.FewerSuppressedLustier".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast2.GazesLock".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddNextButton("Common.Button.KissHer".Localized(), () => ShopBalletMast3(true));

            AddNextButton("Common.Button.DontKiss".Localized(), () => ShopBalletMast3(false));
        }

        public void ShopBalletMast3(bool kissed)
        {
            ClearOutput();

            if (kissed)
            {
                OutputText("Scenes.Marielle.ShopBalletMast3.SinkDownAndKiss".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast3.QuiversSubside".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast3.BreaksOffGasp".Localized());

                Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);
            }
            else
            {
                OutputText("Scenes.Marielle.ShopBalletMast3.KeepHerFromFalling".Localized());

                OutputText("Scenes.Marielle.ShopBalletMast3.SwayingTailor".Localized());
            }

            OutputText("Scenes.Marielle.ShopBalletMast3.SleepDoesntFindYou".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.SeamstressStirsFirst".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.SparingDozyState".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.SecuresGlasses".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.MyApologiesStammer".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.BrushingUpThighs".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.DressKeptSpotless".Localized());

            OutputText("Scenes.Marielle.ShopBalletMast3.DoYou".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            PurchaseMenu(ShopBalletLiftYes, ShopBalletLiftNo);
        }

        public void ShopBalletMastWatch()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletMastWatch.LustReinedIn".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.SoftMoanFineMusic".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.MouthFeelsDry".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.BarelyAnyClimaxWarning".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.SighsAndGulps".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.StraightUprightInstantly".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.InterestingExperience".Localized());

            OutputText("Scenes.Marielle.ShopBalletMastWatch.DoYou".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            PurchaseMenu(ShopBalletLiftYes, ShopBalletLiftNo);
        }

        public void ShopBalletLiftYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBalletLiftYes.TakeFrillyThing".Localized());

            OutputText("Scenes.Marielle.ShopBalletLiftYes.PerksUpLooking".Localized((Player.IsFemale() || (Player.IsGenderless() && Player.Femininity >= 59) ? "be not most absolute" : "lack grace")));

            OutputText("Scenes.Marielle.ShopBalletLiftYes.BeckonsOver".Localized());

            OutputText("Scenes.Marielle.ShopBalletLiftYes.OughtFitYouPlaceholder".Localized((Player.IsFemale() || (Player.IsGenderless() && Player.Femininity >= 59) ? "excellently" : "adequately")));

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BalletDress), 1);

            SexReturn("[pg]It does look perfectly fitted to your form, but by now the night has advanced a good bit, and that's beginning to reflect in Marielle's voice. You decide to stow the dress away and say goodnight.[pg][say:Oh.] Tired though she may be, she's still quick to rise and give you a curtsy. [say:I bid you safe journeys, then. And, ah... pray return whensoever you would wish,] she says as you get ready to leave the temple and make your way back to camp.\", \"[pg]It does look perfectly fitted to your form, but you realize you've been here for a good while, so you decide to stow the dress away and say goodbye for now.[pg][say:Oh.] Marielle is quick to spring to attention and give you a curtsy. [say:Of course. I bid you farewell, then. And, ah... pray return whensoever you would wish,] she says as you get ready to leave the temple and make your way back to camp.\", \"\", shopClothes.bind()));\n    }\n\n    public function shopBalletLiftNo() {\n        clearOutput();\n        sexReturn(\"By now the night has advanced a good bit, and that's beginning to reflect in Marielle's posture as she changes dresses and recollects herself, so you decide to say goodnight and leave her to it.\", \"You realize you've here for a good while, so you decide to say your goodbye and leave Marielle to it for now as she changes dresses and recollects herself.\", \"While Marielle changes dresses and recollects herself, you take another look at her wares.\", shopClothes.bind());\n    }\n\n    public function shopUnderwear(output:Bool = false) {\n        if (output) {\n            clearOutput();\n            outputText(\"You look through the smaller trunk.\");\n            outputText(\"[pg]It's filled with a variety of underwear, most pieces neatly occupying their own spaces, but some having spilled over to where they don't belong. You note that the vast majority of them is intended for a female clientèle: brassieres, panties, and other undergarments of varying degrees of naughtiness, from fairly practical and modest ones to items that are decidedly racy.\");\n        }\n        var buttons= new ButtonDataList();\n        buttons.add(undergarments.FRILBRA.shortName, shopFrillyBra, undergarments.FRILBRA.description);\n        buttons.add(undergarments.LACEBRA.shortName, shopLaceBra, undergarments.LACEBRA.description);\n        buttons.add(undergarments.SHR_BRA.shortName, shopSheerBra, undergarments.SHR_BRA.description);\n        buttons.add(undergarments.BBYDOLL.shortName, shopBabydoll, undergarments.BBYDOLL.description);\n        buttons.add(\"Pantyhose\", shopPantyhose, \"Take a look at the pantyhose in there.\");\n        buttons.add(undergarments.FRILPAN.shortName, shopFrillyPanties, undergarments.FRILPAN.description);\n        buttons.add(undergarments.LACEPAN.shortName, shopLacePanties, undergarments.LACEPAN.description);\n        buttons.add(undergarments.SHR_PAN.shortName, shopSheerPanties, undergarments.SHR_PAN.description);\n        buttons.add(undergarments.SHRBYDL.shortName, shopSheerBabydoll, undergarments.SHRBYDL.description);\n        buttons.add(\"Stripes\", shopStripes, \"There's some striped underwear.\");\n        buttons.submenu(shopUnderwearBack);\n    }\n\n    public function shopUnderwearBack() {\n        clearOutput();\n        outputText(\"You leave the lingerie as it was before.\");\n        shop();\n    }\n\n    public function shopLaceBra() {\n        clearOutput();\n        outputText(\"You pull out a meticulously patterned lace bra. It feels quite smooth to the touch, you find as you turn it about in your hands.\");\n        outputText(\"[pg][say:Sumptuous, is it not?] Marielle asks, putting a word to what you had in mind. [say:Lace has long been a choice most excellent for brassieres both comfortable and, ah... alluring.] She struggles a bit to say that last word out loud. [say:And whiles some may call its braiding a, ah, moil in all its faces, I have in faith found it a something precious treacle.] You suppose she really makes these by hand, then.\");\n        outputText(\"[pg][say:60 gems is what I ask for, should it be by your liking.]\");\n        outputText(\"[pg]Is it?\");\n        currentPrice = 60;\n        purchaseMenu(shopLaceBraYes, shopLaceBraNo);\n    }\n\n    public function shopLaceBraYes() {\n        clearOutput();\n        outputText(\"You hand Marielle the gems.\");\n        outputText(\"[pg][say:Ah, yes, I need make a few adjustments, though. 'Twill do not to be unfitting.] You let her have her way as she grabs a tape and measures your chest and shoulders.\");\n        outputText(\"[pg][say:This shall take not overlong,] she says as she adjusts her glasses and sits down, bra in hand.\");\n        outputText(\"[pg]True to her word, it doesn't. Not long after, she presents you the tailored bra.\");\n        outputText(\"[pg][say:That should be all, then. Howbeit, sit it not well, pray tell me.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.LACEBRA, shopUnderwear.bind());\n    }\n\n    public function shopLaceBraNo() {\n        clearOutput();\n        outputText(\"You put the bra back into the chest.\");\n        shopUnderwear();\n    }\n\n    public function shopLacePanties() {\n        clearOutput();\n        outputText(\"You pull out a pair of panties made of finely patterned lace and turn them around in your hands; they're pretty soft to the touch.\");\n        outputText(\"[pg][say:Fashioning lace may be somedeal time-consuming,] Marielle says, eyeing you with an unreadable expression as you knead the fabric, [say:but well worth the labor, as you appear to... be untombing.] She's still watching your fingers, but finally blinks herself away from them. [say:'Tis quite comfortable indeed, if one use the right, ah, textiles, and rather charming to the eye.] These do seem like pretty nice panties.\");\n        outputText(\"[pg][say:60 gems is what I ask, seek you a purchase.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 60;\n        purchaseMenu(shopLacePantiesYes, shopLacePantiesNo);\n    }\n\n    public function shopLacePantiesYes() {\n        clearOutput();\n        outputText(\"You fish out the gems she asked for.\");\n        outputText(\"[pg][say:Ah,] she exclaims, taking the money and grabbing a measuring tape, [say:I will needs make adjustments, first. Pray hold, ah... still.] She [if (isfemale) {somewhat nervously|quickly}] takes your relevant measurements, then gets to work.\");\n        outputText(\"[pg]Not long after, she beckons you over again and presents you the tailored panties.\");\n        outputText(\"[pg][say:Well, that should be all. If aught sit awry, pray but tell.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.LACEPAN, shopUnderwear.bind());\n    }\n\n    public function shopLacePantiesNo() {\n        clearOutput();\n        outputText(\"You put the panties back where you found them.\");\n        shopUnderwear();\n    }\n\n    public function shopBabydoll() {\n        clearOutput();\n        outputText(\"You find something that looks like an undershirt and take it out. Not quite an undershirt, you realize. The shoulder straps are thin and the top part fits snugly to the chest like a bra, but from there it flows loosely down the torso, more like a very short dress, and a somewhat frilly one at that.\");\n        outputText(\"[pg][say:Oh, yes,] Marielle says as you ask her about it. [say:'Tis a, ah, 'babydoll', a nightgown of sorts, howbeit it need not be night for one to wear suchlike. Quite a lightsome undergarment, and sought for its simplicity, comfort, and, ah, well... enticing qualities.] While her fingers fiddle with the needle they're holding, you turn the piece around. It looks light enough to be worn underneath almost anything.\");\n        outputText(\"[pg][say:Ah, 190 gems will I ask for, should you wish to purchase it.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 190;\n        purchaseMenu(shopBabydollYes, shopBabydollNo);\n    }\n\n    public function shopBabydollYes() {\n        clearOutput();\n        outputText(\"[say:Oh, wonderful,] she exclaims as you hand her the money. [say:Though just one moment, I will needs make a few alterations for you.] She pulls out a pair of tapes and quickly measures you, then gets to work.\");\n        outputText(\"[pg]After taking a relaxed stroll through the old sanctuary, you return to Marielle's desk to find the finished babydoll already lying there for you.\");\n        outputText(\"[pg][say:Pray return, if aught be awry or amiss, and mended it shall be. Free of cost.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.BBYDOLL, shopUnderwear.bind());\n    }\n\n    public function shopBabydollNo() {\n        clearOutput();\n        outputText(\"You put the babydoll back where it belongs.\");\n        shopUnderwear();\n    }\n\n    public function shopSheerBabydoll() {\n        clearOutput();\n        outputText(\"You find something folded up that looks like it's entirely see-through, and you take it out. It indeed is--from its shoulder straps, to the chest-fitted top part, to the rest that flows loosely down the torso like a very short and frilly dress, this thing wouldn't leave much to the imagination at all.\");\n        outputText(\"[pg]Marielle has been quietly eyeing you, a look of what you think is slight embarrassment on her face. You ask her about the garment.\");\n        outputText(\"[pg][say:I, uhm, well...] she starts, grabbing and fiddling with her pincushion. [say:Well, a 'babydoll' is what 'tis called. And, ah, this one is fashioned for, ah... the bedchamber. 'Tis a negligee.] She says that as if she expects you to know what it means and doesn't add anything more, falling into an awkward silence. In any case, it is quite light, so you suppose you could wear it in any situation, really, if the sheer fabric doesn't bother you.\");\n        outputText(\"[pg][say:Uhm... 'tis 200 gems, wish you to purchase it.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 200;\n        purchaseMenu(shopSheerBabydollYes, shopSheerBabydollNo);\n    }\n\n    public function shopSheerBabydollYes() {\n        clearOutput();\n        outputText(\"You'll buy that one.\");\n        outputText(\"[pg][say:Oh! Oh, yes, of course.] It sounds like your choice came a little unexpected, but she quickly puts down the pincushion and takes out a pair of measuring tapes. [say:'Twill but require a few alterations first. I bid you keep yourself still.] You let her take the necessary sizes, give her the money and garment, and let her get to work.\");\n        outputText(\"[pg]After a while, she beckons you over again and presents you the tailored babydoll.\");\n        outputText(\"[pg][say:Well, that should fit quite well, I ween. Though be there any, ah... discomfort, hesitate not to call.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.SHRBYDL, shopUnderwear.bind());\n    }\n\n    public function shopSheerBabydollNo() {\n        clearOutput();\n        outputText(\"You put the racy babydoll back into its compartment.\");\n        shopUnderwear();\n    }\n\n    public function shopSheerBra() {\n        clearOutput();\n        outputText(\"You take out a bra. On first glance, it looks fairly normal--if somewhat elaborate--but as you turn it to the light, you notice the fabric is fine enough to be see-through.\");\n        outputText(\"[pg][say:Ah, well,] Marielle says, pulling on a strip of unfinished lace, [say:these have been rather... desired here within Mareth. 'Tis durable, of course, yet... well...] She gestures roughly in its direction. You guess she means it's quite risqué--you think that's a word she would choose.\");\n        outputText(\"[pg][say:'Twould be, ah, 140 gems if you like to buy one.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 140;\n        purchaseMenu(shopSheerBraYes, shopSheerBraNo);\n    }\n\n    public function shopSheerBraYes() {\n        clearOutput();\n        outputText(\"You hand her the money.\");\n        outputText(\"[pg][say:Oh, of course,] she says, rummaging around for a measuring tape. [say:Some small adjustments shall be necessary however...] After she has taken your relevant sizes, you let her work in silence.\");\n        outputText(\"[pg]It only takes her a minute before she leans back with a satisfied [say:Hmm] and presents you the finished bra.\");\n        outputText(\"[pg][say:All is done, and this ought fit you rather well, now.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.SHR_BRA, shopUnderwear.bind());\n    }\n\n    public function shopSheerBraNo() {\n        clearOutput();\n        outputText(\"You put the transparent bra back into the chest.\");\n        shopUnderwear();\n    }\n\n    public function shopSheerPanties() {\n        clearOutput();\n        outputText(\"You pull out a pair of panties. At first glance, they seem fairly normal, if intriguingly elaborate, but as the light shines onto them, you realize the fabric is fine enough to be entirely see-through. If you wore them, you'd be almost as exposed as without.\");\n        outputText(\"[pg][say:Ah, uhm...] Marielle seems unsure of what to say, fingers playing with themselves. [say:I, well... This manner of cloth is quite, ah... favored by the denizens of this realm. Thus, well, I...] [if (silly) {She probably means to say that she likes making money and these sell like hot cakes among the wanton sluts of Mareth.|You can imagine that these would find a few more buyers here than back in Ingnam or her own world.}] Stretching them a little, you find out the fabric isn't as flimsy as it at first appeared, so they shouldn't be much less reliable than any others.\");\n        outputText(\"[pg][say:Well, I ask for 140 gems, like you to buy a pair.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 140;\n        purchaseMenu(shopSheerPantiesYes, shopSheerPantiesNo);\n    }\n\n    public function shopSheerPantiesYes() {\n        clearOutput();\n        outputText(\"[say:Oh,] she says as you hand her the gems she asked for, [say:of course. I must needs make a few small adjustments aforehand, so... pray hold you still.] She takes out a tape and [if (isfemale) {a little hesitantly|quickly}] takes all the relevant measurements, then gets to work.\");\n        outputText(\"[pg]After less than a minute, she stands up from her stool and presents you the finished pair.\");\n        outputText(\"[pg][say:Well, these should be quite agreeable. And in case they be not so, pray tell.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.SHR_PAN, shopUnderwear.bind());\n    }\n\n    public function shopSheerPantiesNo() {\n        clearOutput();\n        outputText(\"You stow the racy pair of panties back into the chest.\");\n        shopUnderwear();\n    }\n\n    public function shopFrillyBra() {\n        clearOutput();\n        outputText(\"You pull out a particular bra that caught your eye. A fairly elegant, yet cute thing adorned with frills along the edges.\");\n        outputText(\"[pg][say:Charming, is it not?] Marielle asks rhetorically. [say:The purfles bear simpleness and innocence, and yet would they englamour many a, ah... lover.] She stops, seemingly lost in thought for a spell.\");\n        outputText(\"[pg][say:Well, 50 gems is what I ask, should you wish to buy one.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 50;\n        purchaseMenu(shopFrillyBraYes, shopFrillyBraNo);\n    }\n\n    public function shopFrillyBraYes() {\n        clearOutput();\n        outputText(\"[say:Wonderful,] Marielle says as you hand her the money. [say:Though there shall be need of some, ah, alterations first, thus bid I you hold still, if you will...] She takes out a measuring tape and swiftly notes your chest and shoulder sizes. [say:'Twill be finished apace,] she says, sitting down at her desk again.\");\n        outputText(\"[pg]Little more than a moment later, she presents you the tailored bra.\");\n        outputText(\"[pg][say:Should it pinch you anywhere, pray hesitate not to tell me. Any readjustments shall cost you naught, of course.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.FRILBRA, shopUnderwear.bind());\n    }\n\n    public function shopFrillyBraNo() {\n        clearOutput();\n        outputText(\"You put the bra back into the collection of underwear.\");\n        shopUnderwear();\n    }\n\n    public function shopFrillyPanties() {\n        clearOutput();\n        outputText(\"A particular pair of panties catches your eye, and you pull them out. A fairly elegant piece, adorned with frills at the edges, making them look quite pretty.\");\n        outputText(\"[pg][say:Something endearing, are they not?] Marielle asks as if reading your mind. [say:Innocent, yet enticing, though that may be by the, ah, eye of the beholder.] You don't really have anything to add to her assessment.\");\n        outputText(\"[pg][say:50 gems are they, should you wish to purchase a pair.]\");\n        outputText(\"[pg]Do you?\");\n        currentPrice = 50;\n        purchaseMenu(shopFrillyPantiesYes, shopFrillyPantiesNo);\n    }\n\n    public function shopFrillyPantiesYes() {\n        clearOutput();\n        outputText(\"[say:Oh, lovely,] Marielle says, clasping her many hands over the offered money. [say:Though I will needs make some, ah, adjustments. Pray keep... still.] She finds a tape and quickly takes the necessary measurements before getting to work.\");\n        outputText(\"[pg]After only a minute, she beckons you over to present you the finished pair of panties.\");\n        outputText(\"[pg][say:All is done. Howbeit if they should be, ah, uncomfortable anywhere, pray tell.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.FRILPAN, shopUnderwear.bind());\n    }\n\n    public function shopFrillyPantiesNo() {\n        clearOutput();\n        outputText(\"You put the frilly panties back where they belong.\");\n        shopUnderwear();\n    }\n\n    public function shopStripes() {\n        clearOutput();\n        outputText(\"Alternating bits of blue and white catch your eye, so you reach into the chest and fish the garment out. It's a pair of panties, striped horizontally. They seem fairly innocent and simple, yet fetching.\");\n        outputText(\"[pg][say:Hmm...] Marielle hums. [say:'Tis a design that shone quite well upon many a woman in mine home world, in particular children.] [if (ischild) {She looks you over and pushes her glasses up, as if appraising you. [say:[if (femininity >= 50) {I do think they would suit you rather well, aye|Well, girl you need not be}].]|She catches your look and hastily adds, [say:Ah, uhm... of course [i:you] may wear them too, for they certainly, ah, befit people of any... age.]}] Her brows then furrow lightly, eyes on the chest before you. [say:If mistaken I be not...]\");\n        outputText(\"[pg]The seamstress reaches down and sifts through it, quickly pulling out three more items before straightening herself up again and presenting them to you, one in each nimble hand: two bras and one other pair of panties, all striped as well, though one set is sporting pink stripes instead of blue ones.\");\n        outputText(\"[pg][say:These are the catalogue of variations I owe, I believe. The blue ones]--she waves the bra in question and points her free hand at the panties you're holding--[say:have always been a classic most adored, whereas the pink ones]--she holds the matching set aloft--[say:possess a more, ah, feminal semblance. Howbeit, both are quite charming, upon their own mien and manner,] she says before looking at you, perhaps a hint of expectancy in her bright-blue eyes.\");\n        outputText(\"[pg]It seems you're left with a choice between any of the four of them.\");\n        outputText(\"[pg][say:Ah, they are 50 gems each, wish you to buy one,] the girl finally adds.\");\n        currentPrice = 50;\n        menu();\n        addNextButton(\"Blue Bra\", shopStripesBlueBra).hint(undergarments.BSTRBRA.description).disableIf(player.gems < 50, \"You don't have enough money to buy this.\");\n        addNextButton(\"Blue Panties\", shopStripesBluePan).hint(undergarments.BSTRPAN.description).disableIf(player.gems < 50, \"You don't have enough money to buy this.\");\n        addNextButton(\"Pink Bra\", shopStripesPinkBra).hint(undergarments.PSTRBRA.description).disableIf(player.gems < 50, \"You don't have enough money to buy this.\");\n        addNextButton(\"Pink Panties\", shopStripesPinkPan).hint(undergarments.PSTRPAN.description).disableIf(player.gems < 50, \"You don't have enough money to buy this.\");\n        addNextButton(\"Blue Set\", shopStripesBlueSet).hint(\"Take the matching blue-striped bra and panties.\").disableIf(player.gems < 100, \"You don't have enough money to buy this.\");\n        addNextButton(\"Pink Set\", shopStripesPinkSet).hint(\"Take the matching pink-striped bra and panties.\").disableIf(player.gems < 100, \"You don't have enough money to buy this.\");\n        setExitButton(\"Back\", shopStripesBack);\n    }\n\n    public function shopStripesBlueBra() {\n        clearOutput();\n        outputText(\"You say you want the blue-and-white bra, and hand her the money.\");\n        outputText(\"[pg][say:Of course,] Marielle chimes, quickly stowing away the other items and pulling a measuring tape from her desk. [say:I will but needs make a few adjustments for you, so hold still awhile, please.] After she's taken your measurements, you stay there and watch her. There's not much for her to do, but the speed at which she works is nonetheless astounding, and you're soon presented with the adjusted brassiere.\");\n        outputText(\"[pg][say:'Tis all, I ween. If it be pinching anywhere, pray tell me.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.BSTRBRA, shopUnderwear.bind());\n    }\n\n    public function shopStripesBluePan() {\n        clearOutput();\n        outputText(\"You go with the panties that first caught your eye, and give her the money.\");\n        outputText(\"[pg][say:Lovely, a fine choice,] she says, putting the underwear she's holding back into the chest before searching for a measuring tape. [say:Just to be certain, I will needs make a few, ah, adjustments for you...] You let her note the necessary sizes and allow her some time to work.\");\n        outputText(\"[pg]Barely more than a minute later, she holds the adjusted pair of panties out to you between her fingers.\");\n        outputText(\"[pg][say:Well, they should fit you now.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.PSTRBRA, shopUnderwear.bind());\n    }\n\n    public function shopStripesPinkBra() {\n        clearOutput();\n        outputText(\"You like that pink-striped bra, so you hand Marielle the money she asked for it.\");\n        outputText(\"[pg][say:Ah, wonderful,] she says, putting away the other items and taking a measuring tape from her desk. [say:A few adjustments will have to be made, first, so hold you still, please.] You do just that while she takes your bust sizes while murmuring to herself. Giving her some space, you stroll through the old temple for a bit before you hear her beckoning you over again to hand you the tailored bra.\");\n        outputText(\"[pg][say:If it yet pinch you anywhere, pray say the word. Further alterations are free of charge, of course.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.BSTRPAN, shopUnderwear.bind());\n    }\n\n    public function shopStripesPinkPan() {\n        clearOutput();\n        outputText(\"You quite like the pink-striped version of those panties, so you hand Marielle the money.\");\n        outputText(\"[pg][say:Oh, yes, lovely indeed,] she chimes, stowing away the remaining garments. [say:I will but needs]--she pulls a tape off her table--[say:make a few adjustments. Pray hold still for me.] You do as told while she [if (isfemale) {somewhat awkwardly|handily}] takes the necessary measurements and gets to work.\");\n        outputText(\"[pg]Shortly after, she finishes with a contented [say:Hmm] and presents you the tailored pair of panties.\");\n        outputText(\"[pg][say:All should fit well now, although if that be not so, I shall amend it.]\");\n        doTransaction();\n        inventory.takeItem(undergarments.PSTRPAN, shopUnderwear.bind());\n    }\n\n    public function shopStripesBlueSet() {\n        clearOutput();\n        outputText(\"You tell her you want the blue ones as a set, and hand her the money for both pieces.\");\n        outputText(\"[pg][say:Oh! Yes, certainly,] Marielle says, putting away the money and the other two items before searching for a pair of measuring tapes. [say:I will just needs make a few alterations for you, so... pray stir not awhile.] She notes down your necessary sizes [if (femininity >= 59) {somewhat timidly, but swiftly|with practised ease}], then gets to work.\");\n        outputText(\"[pg]It doesn't take long at all before she beckons you over again and holds the finished set of underwear out to you.\");\n        outputText(\"[pg][say:Well, if aught be uncomfortable anywhere, pray but tell.]\");\n        currentPrice = 100;\n        doTransaction();\n        inventory.takeItem(undergarments.BSTRBRA, inventory.takeItem.bind(undergarments.BSTRPAN, shopUnderwear.bind()));\n    }\n\n    public function shopStripesPinkSet() {\n        clearOutput();\n        outputText(\"You want the matching pink set, so you tell her that and hand her the money for both items.\");\n        outputText(\"[pg][say:Oh! Of course, yes,] Marielle says, clasping her quartet of hands before swiftly stowing away your money and the other set, then looking for some measuring tapes. [say:A few adjustments shall be needful withal, of course, so pray, ah, keep you still...] She marks your sizes with [if (femininity >= 59) {a bit of hesitant fumbling|professional swiftness}] and gets to work.\");\n        outputText(\"[pg]Soon after, she calls you over and presents you the tailored set of underwear.\");\n        outputText(\"[pg][say:I believe all should fit quite adequately, now. If it be not so however, prithee inform me.]\");\n        currentPrice = 100;\n        doTransaction();\n        inventory.takeItem(undergarments.PSTRBRA, inventory.takeItem.bind(undergarments.PSTRPAN, shopUnderwear.bind()));\n    }\n\n    public function shopStripesBack() {\n        clearOutput();\n        outputText(\"[say:Hmm... very well.] She sounds more thoughtful than disappointed as she stows the underwear into the chest again.\");\n        shopUnderwear();\n    }\n\n    public function shopPantyhose() {\n        clearOutput();\n        outputText(\"You pull out a pair of pantyhose. They're straightforward and fairly simple: a thin, skin-tight leg garment. Although you do note that these seem to be exceedingly soft and smooth, much more so than what [if (isfeminine) {you were familiar with back home|some [if (ischild || isteen) {girls|women}] back home would wear}].\");\n        outputText(\"[pg][say:Ah,] Marielle says, perking up a little, [say:a piece of hosiery that truly stood and mastered the trials of time. Pantyhose, tights--its naming may bemuse one at times, but 'twill belike refer to this very selfsame item,] she explains, gesturing towards it. [say:'Tis a rather, ah, lengthsome process, knitting these, and one must take heed to tear them not in a moment too remiss.]\");\n        outputText(\"[pg]You stretch them out a bit. The fine, silky mesh looks delicate, but seems also elastic to some extent, so there probably isn't going to be much of an issue unless you abuse them.\");\n        outputText(\"[pg][say:Well, they are supposed to be worn with]--she reaches down and pulls a pair of panties out of the chest--[say:pretties. Underneath, that is. Although some [if (silly) {'people'|people}] would rather wear theirs above. Howbeit, well, you of course need not wear any soever, should you, ah, so desire.] She spreads the panties in question between her fingers and holds them out towards you. They look a little plain, but not in a negative sense--kinda cute and probably comfortable.\");\n        outputText(\"[pg][say:70 gems, wish you to buy these withal, and 40 in the event you do not.]\");\n        outputText(\"[pg]What will it be?\");\n        menu();\n        addNextButton(\"Panties\", shopPantyhoseWith).hint(undergarments.PANHOSE.description).disableIf(player.gems < 70, \"You don't have enough money to buy this.\").hint(undergarments.PANHOSE.description);\n        addNextButton(\"No Panties\", shopPantyhoseWithout).hint(undergarments.NPNTYHS.description).disableIf(player.gems < 40, \"You don't have enough money to buy this.\").hint(undergarments.NPNTYHS.description);\n        addNextButton(\"No\", shopPantyhoseNo);\n    }\n\n    public function shopPantyhoseWith() {\n        clearOutput();\n        outputText(\"You hand Marielle the gems she wanted for both items.\");\n        outputText(\"[pg][say:Ah, lovely,] she says, then searches for a tape. [say:I will needs make some, ah, adjustments, so pray keep you still.] You do as told while she [if (isfemale) {swiftly but a bit tensely|quickly}] measures your hips and legs. Murmuring to herself, she squats down and reaches into the trunk to pull out another pair of pantyhose. [say:These ought be a much finer fit unto your frame, [name]. And prithee allow me but a minute.] You nod and go to take in the murky, mystical view of the bog around you for a bit.\");\n        outputText(\"[pg]Before long, she beckons you back and presents you the finished product.\");\n        outputText(\"[pg][say:I hope 'twill find you pleased. If aught be amiss, hesitate not to inform me.]\");\n        currentPrice = 70;\n        doTransaction();\n        inventory.takeItem(undergarments.PANHOSE, shopUnderwear.bind());\n    }\n\n    public function shopPantyhoseWithout() {\n        clearOutput();\n        outputText(\"Marielle happily takes your gems, though she does cock an eyebrow when you tell her you won't need the panties.\");\n        outputText(\"[pg][say:Hmm... is that so. Well, as you wish. But be that as it may]--she leans to the side to eye your [legs], then bends down to fish another pair of pantyhose out of the chest--[say:these shall be a fit much finer.] You trust her professional judgement.\");\n        currentPrice = 40;\n        doTransaction();\n        inventory.takeItem(undergarments.NPNTYHS, shopUnderwear.bind());\n    }\n\n    public function shopPantyhoseNo() {\n        clearOutput();\n        outputText(\"If Marielle is disappointed, she doesn't show it. You both put the item you're holding back into its place.\");\n        shopUnderwear();\n    }\n\n    public static inline final COM_MOTH_DRESS= 1;\n    public static inline final COM_SYLVIA_DRESS= 2;\n    public static inline final COM_MOTH_PANTIES= 4;\n    public static inline final COM_MOTH_BEDDING= 8;\n    public static inline final COM_SS_DRESS= 3;\n    public static inline final COM_SS_BRA= 5;\n    public static inline final COM_SS_PANTIES= 6;\n    public static inline final COM_SS_UNDERWEAR= 7;\n    public static inline final COM_SS_BEDDING= 9;\n\n    public function shopCommission(output:Bool = false) {\n        if (saveContent.commissionTime != 0.0 && time.totalTime >= saveContent.commissionTime) {\n            takeCommission(saveContent.commission);\n        } else {\n            if (output) {\n                clearOutput();\n                if (saveContent.askedCommission) {\n                    outputText(\"You'd like something made just for you. She does do commissions, so if you have any ideas or found something on your travels that could be worked into clothing, this seamstress here could be the one to ask.\");\n                } else {\n                    outputText(\"Turning away from the two offered chests, you ask Marielle if she does commissions, too.\");\n                    outputText(\"[pg][say:Oh, why yes, of course,] she says with a slight note of surprise, [say:I do indeed. My services as a, ah, couturier are perchance of the kind quite most besought, for every soul would dight itself by its very inward fancy, given hope and scope.] That statement was doused in enthusiasm, though it doesn't last too long. A rap of her fingers follows, and she faintly purses her lips.\");\n                    outputText(\"[pg][say:Yet, the latter is the matter... Howsoever unfirmamented the fantasy may be that carves the fashion of a new gown, engyved are it and I by... what little this realm beteems us.]\");\n                    outputText(\"[pg]A short pause, and you're about to bring up that she already has quite a bit of material here, but the seamstress decides to continue before you get a word out.\");\n                    outputText(\"[pg][say:But, well, uhm... if aught soever dance within your mind, prithee bid it sing, too, and...] Her eyes curve down over your gear. [say:A venturer, you are.] No question, but you confirm, which seems to brighten her idea's spark and lead her to say, [say:Well, should you, in the office of your cause, hap upon a thing you, ah... wis of note and workable, pray procure it hither, if you will. Mayhap a novelty may prove a muse be to me.]\");\n                    saveContent.askedCommission = true;\n                }\n                var hasMaterial= false;\n                if (player.hasKeyItem(\"Bundle of Moth Silk");
        }

        public void ShopBalletLiftNo()
        {
            ClearOutput();

            SexReturn("By now the night has advanced a good bit, and that's beginning to reflect in Marielle's posture as she changes dresses and recollects herself, so you decide to say goodnight and leave her to it.", "You realize you've here for a good while, so you decide to say your goodbye and leave Marielle to it for now as she changes dresses and recollects herself.", "While Marielle changes dresses and recollects herself, you take another look at her wares.", () => ShopClothes());
        }

        public void ShopUnderwear(bool output = false)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Marielle.ShopUnderwear.LookThroughTrunk".Localized());

                OutputText("Scenes.Marielle.ShopUnderwear.FilledWithUnderwear".Localized());
            }

            var buttons = new List<(string Label, Action Act)>
            {
                (ItemRegistry.Create(ItemIds.FrillyBra).ShortNameKey.Localized(), () => ShopFrillyBra()),

                (ItemRegistry.Create(ItemIds.LaceBra).ShortNameKey.Localized(), () => ShopLaceBra()),

                (ItemRegistry.Create(ItemIds.SheerBra).ShortNameKey.Localized(), () => ShopSheerBra()),

                (ItemRegistry.Create(ItemIds.Bbydoll).ShortNameKey.Localized(), () => ShopBabydoll()),

                ("Pantyhose", () => ShopPantyhose()),

                (ItemRegistry.Create(ItemIds.FrillyPanties).ShortNameKey.Localized(), () => ShopFrillyPanties()),

                (ItemRegistry.Create(ItemIds.LacePanties).ShortNameKey.Localized(), () => ShopLacePanties()),

                (ItemRegistry.Create(ItemIds.SheerPanties).ShortNameKey.Localized(), () => ShopSheerPanties()),

                (ItemRegistry.Create(ItemIds.SheerBabydoll).ShortNameKey.Localized(), () => ShopSheerBabydoll()),

                ("Stripes", () => ShopStripes())
            };

            var shopIndex = 0;

            foreach (var (Label, Act) in buttons)
            {
                AddButton(shopIndex, Label, Act);

                shopIndex++;
            }

            AddButton(14, "Common.Button.Back".Localized(), ShopUnderwearBack);
        }

        public void ShopUnderwearBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopUnderwearBack.LeaveLingerie".Localized());

            Shop();
        }

        public void ShopLaceBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLaceBra.PullOutLaceBra".Localized());

            OutputText("Scenes.Marielle.ShopLaceBra.SumptuousIsIt".Localized());

            OutputText("Scenes.Marielle.ShopLaceBra.SixtyGemsAsk".Localized());

            OutputText("Scenes.Marielle.ShopLaceBra.IsIt".Localized());

            currentPrice = 60;

            PurchaseMenu(ShopLaceBraYes, ShopLaceBraNo);
        }

        public void ShopLaceBraYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLaceBraYes.HandOverGems".Localized());

            OutputText("Scenes.Marielle.ShopLaceBraYes.LetHerAdjust".Localized());

            OutputText("Scenes.Marielle.ShopLaceBraYes.AdjustsGlasses".Localized());

            OutputText("Scenes.Marielle.ShopLaceBraYes.TrueToWord".Localized());

            OutputText("Scenes.Marielle.ShopLaceBraYes.TailoredBraDone".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.LaceBra), 1);

            ShopUnderwear();
        }

        public void ShopLaceBraNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLaceBraNo.PutBraBack".Localized());

            ShopUnderwear();
        }

        public void ShopLacePanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLacePanties.PullOutLacePanties".Localized());

            OutputText("Scenes.Marielle.ShopLacePanties.EyesYouGreedily".Localized());

            OutputText("Scenes.Marielle.ShopLacePanties.SixtyGemsPurchase".Localized());

            OutputText("Scenes.Marielle.ShopLacePanties.DoYou".Localized());

            currentPrice = 60;

            PurchaseMenu(ShopLacePantiesYes, ShopLacePantiesNo);
        }

        public void ShopLacePantiesYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLacePantiesYes.FishOutGems".Localized());

            OutputText("Scenes.Marielle.ShopLacePantiesYes.ExclaimsTakesMoney".Localized());

            OutputText("Scenes.Marielle.ShopLacePantiesYes.BeckonsPantiesOver".Localized());

            OutputText("Scenes.Marielle.ShopLacePantiesYes.ShouldBeAll".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.LacePanties), 1);

            ShopUnderwear();
        }

        public void ShopLacePantiesNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopLacePantiesNo.PutPantiesBack".Localized());

            ShopUnderwear();
        }

        public void ShopBabydoll()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBabydoll.FindBabydoll".Localized());

            OutputText("Scenes.Marielle.ShopBabydoll.ExplainsBabydoll".Localized());

            OutputText("Scenes.Marielle.ShopBabydoll.HundredNinetyGems".Localized());

            OutputText("Scenes.Marielle.ShopBabydoll.DoYou".Localized());

            currentPrice = 190;

            PurchaseMenu(ShopBabydollYes, ShopBabydollNo);
        }

        public void ShopBabydollYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBabydollYes.WonderfulExclaims".Localized());

            OutputText("Scenes.Marielle.ShopBabydollYes.StrollThroughSanctuary".Localized());

            OutputText("Scenes.Marielle.ShopBabydollYes.MendedFreeOfCost".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Bbydoll), 1);

            ShopUnderwear();
        }

        public void ShopBabydollNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopBabydollNo.PutBabydollBack".Localized());

            ShopUnderwear();
        }

        public void ShopSheerBabydoll()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerBabydoll.FindFoldedSeeThrough".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydoll.QuietlyEyesYou".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydoll.FiddlesPincushion".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydoll.TwoHundredGems".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydoll.DoYou".Localized());

            currentPrice = 200;

            PurchaseMenu(ShopSheerBabydollYes, ShopSheerBabydollNo);
        }

        public void ShopSheerBabydollYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerBabydollYes.BuyThatOne".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydollYes.ChoiceUnexpected".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydollYes.BeckonsBabydollOver".Localized());

            OutputText("Scenes.Marielle.ShopSheerBabydollYes.ShouldFitWell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SheerBabydoll), 1);

            ShopUnderwear();
        }

        public void ShopSheerBabydollNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerBabydollNo.PutRacyBabydollBack".Localized());

            ShopUnderwear();
        }

        public void ShopSheerBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerBra.TakeOutSheerBra".Localized());

            OutputText("Scenes.Marielle.ShopSheerBra.PullsUnfinishedLace".Localized());

            OutputText("Scenes.Marielle.ShopSheerBra.HundredFortyGems".Localized());

            OutputText("Scenes.Marielle.ShopSheerBra.DoYou".Localized());

            currentPrice = 140;

            PurchaseMenu(ShopSheerBraYes, ShopSheerBraNo);
        }

        public void ShopSheerBraYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerBraYes.HandOverMoney".Localized());

            OutputText("Scenes.Marielle.ShopSheerBraYes.RummagesTape".Localized());

            OutputText("Scenes.Marielle.ShopSheerBraYes.MinuteSatisfiedHmm".Localized());

            OutputText("Scenes.Marielle.ShopSheerBraYes.OughtFitRatherWell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SheerBra), 1);

            ShopUnderwear();
        }

        public void ShopSheerBraNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerBraNo.PutTransparentBraBack".Localized());

            ShopUnderwear();
        }

        public void ShopSheerPanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerPanties.PullOutSheerPanties".Localized());

            OutputText("Scenes.Marielle.ShopSheerPanties.UnsureWhatToSay".Localized());

            OutputText("Scenes.Marielle.ShopSheerPanties.HundredFortyGemsPair".Localized());

            OutputText("Scenes.Marielle.ShopSheerPanties.DoYou".Localized());

            currentPrice = 140;

            PurchaseMenu(ShopSheerPantiesYes, ShopSheerPantiesNo);
        }

        public void ShopSheerPantiesYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerPantiesYes.OfCourseAdjust".Localized());

            OutputText("Scenes.Marielle.ShopSheerPantiesYes.PresentsFinishedPanties".Localized());

            OutputText("Scenes.Marielle.ShopSheerPantiesYes.QuiteAgreeable".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SheerPanties), 1);

            ShopUnderwear();
        }

        public void ShopSheerPantiesNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopSheerPantiesNo.StowRacyPanties".Localized());

            ShopUnderwear();
        }

        public void ShopFrillyBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopFrillyBra.PullOutFrillyBra".Localized());

            OutputText("Scenes.Marielle.ShopFrillyBra.CharmingIsItNot".Localized());

            OutputText("Scenes.Marielle.ShopFrillyBra.FiftyGemsAsk".Localized());

            OutputText("Scenes.Marielle.ShopFrillyBra.DoYou".Localized());

            currentPrice = 50;

            PurchaseMenu(ShopFrillyBraYes, ShopFrillyBraNo);
        }

        public void ShopFrillyBraYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopFrillyBraYes.WonderfulNeedsWork".Localized());

            OutputText("Scenes.Marielle.ShopFrillyBraYes.TailoredBraMoment".Localized());

            OutputText("Scenes.Marielle.ShopFrillyBraYes.PinchAnywhereTell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.FrillyBra), 1);

            ShopUnderwear();
        }

        public void ShopFrillyBraNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopFrillyBraNo.PutFrillyBraBack".Localized());

            ShopUnderwear();
        }

        public void ShopFrillyPanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopFrillyPanties.ParticularPantiesCatch".Localized());

            OutputText("Scenes.Marielle.ShopFrillyPanties.SomethingEndearing".Localized());

            OutputText("Scenes.Marielle.ShopFrillyPanties.FiftyGemsPair".Localized());

            OutputText("Scenes.Marielle.ShopFrillyPanties.DoYou".Localized());

            currentPrice = 50;

            PurchaseMenu(ShopFrillyPantiesYes, ShopFrillyPantiesNo);
        }

        public void ShopFrillyPantiesYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopFrillyPantiesYes.ClaspsManyHands".Localized());

            OutputText("Scenes.Marielle.ShopFrillyPantiesYes.BeckonsFinishedPair".Localized());

            OutputText("Scenes.Marielle.ShopFrillyPantiesYes.AllIsDone".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.FrillyPanties), 1);

            ShopUnderwear();
        }

        public void ShopFrillyPantiesNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopFrillyPantiesNo.PutFrillyPantiesBack".Localized());

            ShopUnderwear();
        }

        public void ShopStripes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripes.BlueWhiteStripes".Localized());

            OutputText("Scenes.Marielle.ShopStripes.HumsCatchesLook".Localized());

            OutputText("Scenes.Marielle.ShopStripes.SiftsOutThreeMore".Localized());

            OutputText("Scenes.Marielle.ShopStripes.CatalogueOfVariations".Localized());

            OutputText("Scenes.Marielle.ShopStripes.ChoiceBetweenFour".Localized());

            OutputText("Scenes.Marielle.ShopStripes.FiftyGemsEach".Localized());

            currentPrice = 50;

            ClearButtons();

            AddNextButton("Common.Button.BlueBra".Localized(), ShopStripesBlueBra);

            AddNextButton("Common.Button.BluePanties".Localized(), ShopStripesBluePan);

            AddNextButton("Common.Button.PinkBra".Localized(), ShopStripesPinkBra);

            AddNextButton("Common.Button.PinkPanties".Localized(), ShopStripesPinkPan);

            AddNextButton("Common.Button.BlueSet".Localized(), ShopStripesBlueSet);

            AddNextButton("Common.Button.PinkSet".Localized(), ShopStripesPinkSet);

            AddButton(14, "Common.Button.Back".Localized(), () => ShopStripesBack());
        }

        public void ShopStripesBlueBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesBlueBra.WantBlueBra".Localized());

            OutputText("Scenes.Marielle.ShopStripesBlueBra.ChimesStowsOthers".Localized());

            OutputText("Scenes.Marielle.ShopStripesBlueBra.AllDoneIfPinching".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.BlackStripedBra), 1);

            ShopUnderwear();
        }

        public void ShopStripesBluePan()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesBluePan.GoWithFirstPanties".Localized());

            OutputText("Scenes.Marielle.ShopStripesBluePan.LovelyFineChoice".Localized());

            OutputText("Scenes.Marielle.ShopStripesBluePan.MinuteLaterHoldsOut".Localized());

            OutputText("Scenes.Marielle.ShopStripesBluePan.ShouldFitNow".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.PurpleStripedBra), 1);

            ShopUnderwear();
        }

        public void ShopStripesPinkBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesPinkBra.LikePinkBra".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkBra.PutsAwayOthers".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkBra.IfPinchSayWord".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.BlackStripedPanties), 1);

            ShopUnderwear();
        }

        public void ShopStripesPinkPan()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesPinkPan.LikePinkPanties".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkPan.ChimesStowsGarments".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkPan.ContentedHmmPresents".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkPan.AllShouldFitWell".Localized());

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.PurpleStripedPanties), 1);

            ShopUnderwear();
        }

        public void ShopStripesBlueSet()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesBlueSet.WantBlueSet".Localized());

            OutputText("Scenes.Marielle.ShopStripesBlueSet.CertainlyPutsMoneyAway".Localized());

            OutputText("Scenes.Marielle.ShopStripesBlueSet.BeckonsFinishedSet".Localized());

            OutputText("Scenes.Marielle.ShopStripesBlueSet.IfUncomfortableTell".Localized());

            currentPrice = 100;

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.BlackStripedBra), 1);

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.BlackStripedPanties), 1);

            ShopUnderwear();
        }

        public void ShopStripesPinkSet()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesPinkSet.WantPinkSet".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkSet.ClaspsQuartetHands".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkSet.CallsYouOver".Localized());

            OutputText("Scenes.Marielle.ShopStripesPinkSet.ShouldFitAdequately".Localized());

            currentPrice = 100;

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.PurpleStripedBra), 1);

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.PurpleStripedPanties), 1);

            ShopUnderwear();
        }

        public void ShopStripesBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopStripesBack.ThoughtfulNotDisappointed".Localized());

            ShopUnderwear();
        }

        public void ShopPantyhose()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopPantyhose.PullOutPantyhose".Localized());

            OutputText("Scenes.Marielle.ShopPantyhose.PerksUpHosiery".Localized());

            OutputText("Scenes.Marielle.ShopPantyhose.StretchThemOut".Localized());

            OutputText("Scenes.Marielle.ShopPantyhose.ReachesDownPullsPair".Localized());

            OutputText("Scenes.Marielle.ShopPantyhose.SeventyGemsWithal".Localized());

            OutputText("Scenes.Marielle.ShopPantyhose.WhatWillItBe".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Panties".Localized(), ShopPantyhoseWith);

            AddNextButton("Common.Button.NoPanties".Localized(), ShopPantyhoseWithout);

            AddNextButton("Common.Button.No".Localized(), ShopPantyhoseNo);
        }

        public void ShopPantyhoseWith()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopPantyhoseWith.HandBothGems".Localized());

            OutputText("Scenes.Marielle.ShopPantyhoseWith.SearchesForTape".Localized());

            OutputText("Scenes.Marielle.ShopPantyhoseWith.BeckonsFinishedProduct".Localized());

            OutputText("Scenes.Marielle.ShopPantyhoseWith.HopePleased".Localized());

            currentPrice = 70;

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Pantyhose), 1);

            ShopUnderwear();
        }

        public void ShopPantyhoseWithout()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopPantyhoseWithout.HappilyTakesGems".Localized());

            OutputText("Scenes.Marielle.ShopPantyhoseWithout.LeansToTheSide".Localized());

            currentPrice = 40;

            DoTransaction();

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.NephilaPantyhose), 1);

            ShopUnderwear();
        }

        public void ShopPantyhoseNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopPantyhoseNo.PutsItemBack".Localized());

            ShopUnderwear();
        }

        public void ShopCommission(bool output = false)
        {
            ClearOutput();

            if (Save.GetFloat("commissionTime") != 0.0f && View.Time.TotalTime >= Save.GetFloat("commissionTime"))
                TakeCommission(Save.GetInt("commission", 0));
            else
            {
                if (output)
                {
                    if (Save.GetBool("askedCommission"))
                        OutputText("Scenes.Marielle.ShopCommission.WantCustomMade".Localized());
                    else
                    {
                        OutputText("Scenes.Marielle.ShopCommission.AskAboutCommissions".Localized());

                        OutputText("Scenes.Marielle.ShopCommission.YesSlightSurprise".Localized());

                        OutputText("Scenes.Marielle.ShopCommission.UnfirmamentedFantasy".Localized());

                        OutputText("Scenes.Marielle.ShopCommission.ShortPauseAboutTo".Localized());

                        OutputText("Scenes.Marielle.ShopCommission.EyesCurveDown".Localized());

                        Save.SetBool("askedCommission", true);
                    }

                    var hasMaterial = false;

                    if (Player.HasKeyItem("Bundle of Moth Silk"))
                    {
                        hasMaterial = true;

                        OutputText("Scenes.Marielle.ShopCommission.HaveDaughterCocoon".Localized());
                    }

                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk)))
                    {
                        hasMaterial = true;

                        OutputText("Scenes.Marielle.ShopCommission.HaveSpiderSilk".Localized());
                    }

                    if (!hasMaterial)
                        OutputText("Scenes.Marielle.ShopCommission.HaveNothingUsable".Localized());
                }

                ClearButtons();

                AddNextButton("Common.Button.MothSilk".Localized(), () => MothCommissions(true));

                AddNextButton("Common.Button.SpiderSilk".Localized(), () => SpiderCommissions(true));

                AddButton(14, "Common.Button.Back".Localized(), () => CommissionBack());
            }
        }

        public void CommissionBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionBack.TurnBackToChests".Localized());

            Shop();
        }

        public void MothCommissions(bool output = false)
        {
            ClearOutput();

            if (output)
                if (!Save.GetBool("seenMothSilk"))
                {
                    OutputText("Scenes.Marielle.MothCommissions.LargeMothSilkPiece".Localized());

                    OutputText("Scenes.Marielle.MothCommissions.DepositIntoHands".Localized());

                    if ((Places.MothCave.DoloresScene.Save.GetInt("hikkiQuest", 0) & Places.MothCave.DoloresScene.HQMARI) != 0)
                    {
                        OutputText("Scenes.Marielle.MothCommissions.BringsToFace".Localized());

                        OutputText("Scenes.Marielle.MothCommissions.ItIsDaughters".Localized());

                        OutputText("Scenes.Marielle.MothCommissions.NeitherNoticed".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marielle.MothCommissions.AdjustsSpectaclesRepeat".Localized());

                        OutputText("Scenes.Marielle.MothCommissions.MothSayYou".Localized());

                        OutputText("Scenes.Marielle.MothCommissions.NoCommentButHovering".Localized());
                    }

                    OutputText("Scenes.Marielle.MothCommissions.GoodSilkIndeed".Localized());

                    OutputText("Scenes.Marielle.MothCommissions.WhatFashionTherefrom".Localized());

                    Save.SetBool("seenMothSilk", true);
                }
                else
                {
                    OutputText("Scenes.Marielle.MothCommissions.UnpackCocoonSilk".Localized());

                    OutputText("Scenes.Marielle.MothCommissions.RecognizesSilk".Localized());
                }

            ClearButtons();

            AddNextButton("Common.Button.GiftDress".Localized(), CommissionSylviaDress);

            AddNextButton("Common.Button.Dress".Localized(), CommissionMothDress);

            AddNextButton("Common.Button.Panties".Localized(), CommissionMothPanties);

            AddNextButton("Common.Button.Bedding".Localized(), CommissionMothBedding);

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => MothCommissionBack());
        }

        public void MothCommissionBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MothCommissionBack.NotGetAnything".Localized());

            ShopCommission();
        }

        public void CommissionMothDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothDress.WantDress".Localized());

            OutputText("Scenes.Marielle.CommissionMothDress.HmmOnly".Localized());

            OutputText("Scenes.Marielle.CommissionMothDress.AllSheUtters".Localized());

            OutputText("Scenes.Marielle.CommissionMothDress.ThoughtWellConceived".Localized());

            OutputText("Scenes.Marielle.CommissionMothDress.ClassicInMind".Localized((SceneInstance<Places.MothCave.DoloresScene>.Get().GoodParent() ? "serve as a nice, tasteful commemorative of your daughter's metamorphosis, and of all the time you've been spending with her" : "be a good use for a silk as rare and valuable as your own daughter's")));

            OutputText("Scenes.Marielle.CommissionMothDress.SixFiftyGems".Localized());

            OutputText("Scenes.Marielle.CommissionMothDress.CommissionDress".Localized());

            currentPrice = 650;

            PurchaseMenu(CommissionMothDressYes, CommissionMothDressNo);
        }

        public void CommissionMothDressYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothDressYes.WorthItToYou".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.HoldYouSoAwhile".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.QuiteWellMurmurs".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.HoursOnChest".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.FullBodyPortrayals".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.GoOverOnceMore".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.GlimpsesPapers".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.TakeHerWord".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressYes.FiveDaysNote".Localized());

            DoTransaction();

            Player.RemoveKeyItem("Bundle of Moth Silk");

            Save.SetInt("commission", COM_MOTH_DRESS);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 120);

            DoNext(() => ReturnToCamp(2));
        }

        public void CommissionMothDressNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothDressNo.TurnDownDress".Localized());

            OutputText("Scenes.Marielle.CommissionMothDressNo.DifferentVestment".Localized());

            MothCommissions();
        }

        public void CommissionSylviaDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSylviaDress.DressAsGift".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.EyesTwinkle".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.AMothGirl".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.StumblesExhaleSigh".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.WellBethoughtAffection".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.MakeTalkOfTrade".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.DifficultNoClothing".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.RuminatingNoWriting".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDress.CommissionDress".Localized());

            currentPrice = 600;

            PurchaseMenu(CommissionSylviaDressYes, CommissionSylviaDressNo);
        }

        public void CommissionSylviaDressYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.DefinitelyWorthIt".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.MoreExcitedThanYou".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.TimeOnChest".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.CurvaceousMothPortrayals".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.GoOverOnceMore".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.GlimpsesNotes".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.TakeHerWord".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressYes.FiveDaysNote".Localized());

            DoTransaction();

            Player.RemoveKeyItem("Bundle of Moth Silk");

            Save.SetInt("commission", COM_SYLVIA_DRESS);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 120);

            DoNext(() => ReturnToCamp(2));
        }

        public void CommissionSylviaDressNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSylviaDressNo.TurnDownDress".Localized());

            OutputText("Scenes.Marielle.CommissionSylviaDressNo.OhVeryWell".Localized());

            MothCommissions();
        }

        public void CommissionMothPanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothPanties.WantMothPanties".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.GazesAtSilk".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.MannerOfUndergarments".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.DescribeCutTightness".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.TiltsHeadLeft".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.BrowsRiseAjar".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.UseMoreCloth".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.PencilTapsHand".Localized());

            OutputText("Scenes.Marielle.CommissionMothPanties.CommissionPanties".Localized());

            currentPrice = 0;

            PurchaseMenu(CommissionMothPantiesYes, CommissionMothPantiesNo);
        }

        public void CommissionMothPantiesYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothPantiesYes.HaveDeal".Localized());

            OutputText("Scenes.Marielle.CommissionMothPantiesYes.LetHerMeasure".Localized());

            OutputText("Scenes.Marielle.CommissionMothPantiesYes.ThusShallBe".Localized());

            OutputText("Scenes.Marielle.CommissionMothPantiesYes.ADayMostLike".Localized());

            OutputText("Scenes.Marielle.CommissionMothPantiesYes.TightDeadline".Localized());

            DoTransaction();

            Player.RemoveKeyItem("Bundle of Moth Silk");

            Save.SetInt("commission", COM_MOTH_PANTIES);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 24);

            DoNext(() => ReturnToCamp(1));
        }

        public void CommissionMothPantiesNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothPantiesNo.ChangedMindOffer".Localized());

            OutputText("Scenes.Marielle.CommissionMothPantiesNo.VeryWellAughtElse".Localized());

            MothCommissions();
        }

        public void CommissionMothBedding()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothBedding.ABedSheet".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.FullSetBedclothes".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.FrownSimpleEnough".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.WhatMeasureShape".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.ProvideProportions".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.ThreeHundredGems".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.ChaosMakesSense".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.SatinAfterAll".Localized());

            OutputText("Scenes.Marielle.CommissionMothBedding.CommissionBedding".Localized());

            currentPrice = 300;

            PurchaseMenu(CommissionMothBeddingYes, CommissionMothBeddingNo);

            AddNextButton("Common.Button.Elaborate".Localized(), CommissionMothBeddingElaborate);
        }

        public void CommissionMothBeddingElaborate()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothBeddingElaborate.UnfamiliarAskSatin".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingElaborate.ClothOfVariance".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingElaborate.WarpFloatingWeave".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingElaborate.WarpBorneAloft".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingElaborate.NowYouKnow".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingElaborate.FreeingFingersQuestion".Localized());

            RemoveButton(2);
        }

        public void CommissionMothBeddingYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothBeddingYes.LookForwardProduct".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingYes.TwoDaysMayhap".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingYes.TwoFullDays".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingYes.DoneInGoodTime".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingYes.ReturnFarewellExit".Localized());

            DoTransaction();

            Player.RemoveKeyItem("Bundle of Moth Silk");

            Save.SetInt("commission", COM_MOTH_BEDDING);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 48);

            DoNext(() => ReturnToCamp(1));
        }

        public void CommissionMothBeddingNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionMothBeddingNo.TurnDownBedding".Localized());

            OutputText("Scenes.Marielle.CommissionMothBeddingNo.VeryWellWishAught".Localized());

            MothCommissions();
        }

        public void SpiderCommissions(bool output = false)
        {
            ClearOutput();

            if (output)
                if (!Save.GetBool("seenSpiderSilk"))
                {
                    OutputText("Scenes.Marielle.SpiderCommissions.HaveSilkHere".Localized());

                    OutputText("Scenes.Marielle.SpiderCommissions.ThumbOverMaterial".Localized());

                    OutputText("Scenes.Marielle.SpiderCommissions.HeldApparelHerefrom".Localized());

                    OutputText("Scenes.Marielle.SpiderCommissions.NotAwareOfValue".Localized());

                    OutputText("Scenes.Marielle.SpiderCommissions.SeparatesSingleStrand".Localized((Player.Body.Get<BodyParts.Vanilla.TailPart>().Id == BodyPartTypes.SpiderAbdomen ? "but even you can't spot a difference, though you do know that you can alter the thickness of your own webbing to some extent" : "though you don't think you could spot a difference")));

                    OutputText("Scenes.Marielle.SpiderCommissions.StretchesString".Localized());

                    Save.SetBool("seenSpiderSilk", true);
                }
                else
                {
                    OutputText("Scenes.Marielle.SpiderCommissions.OfferSilkAgain".Localized());

                    OutputText("Scenes.Marielle.SpiderCommissions.FingersDrumBundle".Localized());

                    OutputText("Scenes.Marielle.SpiderCommissions.WhatShallItBe".Localized());
                }

            RegisterMacro("SSbundles", Player.ItemCount(ConsumableIds.ToughSilk));

            ClearButtons();

            AddNextButton("Common.Button.Dress".Localized(), CommissionSpiderDress);

            AddNextButton("Common.Button.Bra".Localized(), CommissionSpiderBra);

            AddNextButton("Common.Button.Panties".Localized(), CommissionSpiderPanties);

            AddNextButton("Common.Button.Underwear".Localized(), CommissionSpiderUnderwear);

            AddNextButton("Common.Button.Bedding".Localized(), CommissionSpiderBedding);

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => SpiderCommissionBack());
        }

        public void SpiderCommissionBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.SpiderCommissionBack.DecideAgainst".Localized());

            ShopCommission();
        }

        public void CommissionSpiderDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderDress.ElegantDressInMind".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDress.PaperPencilReady".Localized((SceneInstance<RathazulScene>.Get().FollowerRathazul() ? "do have one of the latter back at camp, but right now you're still asking her, and not the old rat" : "could probably look for either of those, but right now you're still asking her, and not someone else")));

            OutputText("Scenes.Marielle.CommissionSpiderDress.LikeSoMayhap".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDress.TwoPortraits".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDress.AskCost".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDress.FiveHundredGems".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDress.CommissionDress".Localized());

            currentPrice = 500;

            PurchaseMenu(CommissionSpiderDressYes, CommissionSpiderDressNo);

            DisableButtonIf(0, !Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 5));
        }

        public void CommissionSpiderDressYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.TakeDeal".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.AgreedWonderful".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.RefineDesign".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.FinalLookOver".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.MoreStylishBefore".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.HumEyesOnReels".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.CurtsyInterlaces".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressYes.TryToDoThat".Localized());

            DoTransaction();

            Player.DestroyItems(ConsumableIds.ToughSilk, 5);

            Save.SetInt("commission", COM_SS_DRESS);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 96);

            DoNext(() => ReturnToCamp(2));
        }

        public void CommissionSpiderDressNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderDressNo.TurnDownDress".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderDressNo.DifferentGarment".Localized());

            SpiderCommissions();
        }

        public void CommissionSpiderBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderBra.WantFittedBra".Localized(
                Save.GetInt("commissionCount", 0) != 0 ? " know" : (Save.GetInt("itemsBought", 0) != 0 ? "'re sure" : " hope")
            ));

            OutputText("Scenes.Marielle.CommissionSpiderBra.PeersMeasuring".Localized(
                (Save.GetInt("timesYurid", 0) + Save.GetInt("timesKissed", 0) > 0) && Player.IsFemale() ? "not entirely" : (Player.Femininity >= 59 && Player.BiggestBreastSize() > 0 ? "probably" : "purely")
            ));

            OutputText("Scenes.Marielle.CommissionSpiderBra.CommissionBra".Localized());

            currentPrice = 230;

            PurchaseMenu(CommissionSpiderBraYes, CommissionSpiderBraNo);

            DisableButtonIf(0, !Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 2));
        }

        public void CommissionSpiderBraYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderBraYes.ReadyToStart".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBraYes.SplendidStandsUp".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBraYes.PlanBraSpecifics".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBraYes.AllInAccord".Localized((Save.GetBool("mockedDress", false) || Save.GetInt("insulted", 0) != 0 ? "neat" : "cordial")));

            DoTransaction();

            Player.DestroyItems(ConsumableIds.ToughSilk, 2);

            Save.SetInt("commission", COM_SS_BRA);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 24);

            DoNext(() => ReturnToCamp(1));
        }

        public void CommissionSpiderBraNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderBraNo.TurnDownBra".Localized());

            SpiderCommissions();
        }

        public void CommissionSpiderPanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderPanties.WantSoftPanties".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPanties.NonCommittalHmm".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPanties.GraphiteFlying".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPanties.HmmOnly".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPanties.AskIfAnythingWrong".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPanties.PencilKeepsMoving".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPanties.CommissionPanties".Localized());

            currentPrice = 230;

            PurchaseMenu(CommissionSpiderPantiesYes, CommissionSpiderPantiesNo);

            DisableButtonIf(0, !Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 2));
        }

        public void CommissionSpiderPantiesYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderPantiesYes.AnotherBundle".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPantiesYes.AccurateDraft".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPantiesYes.HoldTheyRegard".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPantiesYes.TakeLeave".Localized());

            DoTransaction();

            Player.DestroyItems(ConsumableIds.ToughSilk, 2);

            Save.SetInt("commission", COM_SS_PANTIES);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 24);

            DoNext(() => ReturnToCamp(1));
        }

        public void CommissionSpiderPantiesNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderPantiesNo.TurnDownPanties".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderPantiesNo.WishAughtElse".Localized());

            SpiderCommissions();
        }

        public void CommissionSpiderUnderwear()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderUnderwear.MatchingSet".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwear.FeaturesLightUp".Localized((Player.IsFemale() && Player.Femininity >= 70 && Save.GetInt("timesKissed", 0) != 0 && Save.GetInt("insulted", 0) == 0 && !Save.GetBool("mockedDress", false) ? "prettiest" : "finest"), (Player.IsFeminine() && Player.Femininity >= 50 ? "As she says that, her eyes rest on you, though you're getting the impression her mind's focus is being swept away elsewards. Before the resulting lull can drag on for too long" : "She halts. [say:Or, uhm... Well.] Words seem to fail her, but before she can get stuck on anything")));

            OutputText("Scenes.Marielle.CommissionSpiderUnderwear.HandsCongregate".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwear.CommissionUnderwear".Localized());

            currentPrice = 450;

            PurchaseMenu(CommissionSpiderUnderwearYes, CommissionSpiderUnderwearNo);

            DisableButtonIf(0, !Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 4));
        }

        public void CommissionSpiderUnderwearYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearYes.MarvellousChirps".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearYes.HalfHourMeasurements".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearYes.NotAOne".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearYes.FareYouWell".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearYes.FareWellThroughBog".Localized());

            DoTransaction();

            Player.DestroyItems(ConsumableIds.ToughSilk, 4);

            Save.SetInt("commission", COM_SS_UNDERWEAR);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 48);

            DoNext(() => ReturnToCamp(1));
        }

        public void CommissionSpiderUnderwearNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearNo.TurnDownUnderwear".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderUnderwearNo.HmmAughtElse".Localized());

            SpiderCommissions();
        }

        public void CommissionSpiderBedding()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderBedding.SoundsLikeGoodIdea".Localized((Player.HasKeyItem("Moth Bedding") ? "An additional set of silken sheets" : "Sleeping in a sea of silk")));

            OutputText("Scenes.Marielle.CommissionSpiderBedding.CastInFull".Localized((Player.HasKeyItem("Moth Bedding") ? ", as I did your, uhm, daughter's" : "")));

            OutputText("Scenes.Marielle.CommissionSpiderBedding.HalfDozenSuffice".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBedding.DescribeBedAtCamp".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBedding.CommissionBedding".Localized());

            currentPrice = 380;

            PurchaseMenu(CommissionSpiderBeddingYes, CommissionSpiderBeddingNo);

            DisableButtonIf(0, !Player.HasItem(ItemRegistry.Create(ConsumableIds.ToughSilk), 6));
        }

        public void CommissionSpiderBeddingYes()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderBeddingYes.FinalWordBusiness".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBeddingYes.MomentMoreConference".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBeddingYes.EstimatesComeQuick".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBeddingYes.AsksAllInOrder".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBeddingYes.MayItDispleaseYou".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBeddingYes.NotSureSafe".Localized());

            DoTransaction();

            Player.DestroyItems(ConsumableIds.ToughSilk, 6);

            Save.SetInt("commission", COM_SS_BEDDING);

            Save.SetFloat("commissionTime", View.Time.TotalTime + 48);

            DoNext(() => ReturnToCamp(1));
        }

        public void CommissionSpiderBeddingNo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionSpiderBeddingNo.NotEnoughBundles".Localized());

            OutputText("Scenes.Marielle.CommissionSpiderBeddingNo.PageVeryWell".Localized());

            SpiderCommissions();
        }

        public void CommissionReturn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.CommissionReturn.ConsiderWhatElse".Localized());

            MarielleMenu();
        }

        public void TakeCommission(int commission, bool playScene = true)
        {

            switch (commission)
            {
                case COM_MOTH_DRESS:
                    {
                        if (playScene)
                            TakeMothDress();
                        else
                        {
                            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.MaidSilkDress), 1);

                            ReturnToCamp(1);
                        }
                    }

                    break;

                case COM_SYLVIA_DRESS:
                    {
                        if (playScene)
                            TakeSylviaDress();
                        else
                            Player.CreateKeyItem("Sylvia's Dress", 0, 0, 0, 0);
                    }

                    break;

                case COM_MOTH_PANTIES:
                    {
                        if (playScene)
                            TakeMothPanties();
                        else
                        {
                            Player.Inventory.Add(ItemRegistry.Create(ItemIds.MothSilkPanties), 1);

                            ReturnToCamp(1);
                        }
                    }

                    break;

                case COM_MOTH_BEDDING:
                    {
                        if (playScene)
                            TakeMothBedding();
                        else
                            Player.CreateKeyItem("Moth Bedding", 0, 0, 0, 0);
                    }

                    break;

                case COM_SS_DRESS:
                    {
                        if (playScene)
                            TakeSpiderDress();
                        else
                        {
                            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.SilkSummerDress), 1);

                            ReturnToCamp(1);
                        }
                    }

                    break;

                case COM_SS_BRA:
                    {
                        if (playScene)
                            TakeSpiderBra();
                        else
                        {
                            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkBra), 1);

                            ReturnToCamp(1);
                        }
                    }

                    break;

                case COM_SS_PANTIES:
                    {
                        if (playScene)
                            TakeSpiderPanties();
                        else
                        {
                            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkPanties), 1);

                            ReturnToCamp(1);
                        }
                    }

                    break;

                case COM_SS_UNDERWEAR:
                    {
                        if (playScene)
                            TakeSpiderUnderwear();
                        else
                        {
                            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkBra), 1);

                            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkPanties), 1);

                            ReturnToCamp(1);
                        }
                    }

                    break;

                case COM_SS_BEDDING:
                    {
                        if (playScene)
                            TakeSpiderBedding();
                        else
                            Player.CreateKeyItem("Spider-Silk Bedding", 0, 0, 0, 0);
                    }

                    break;
            }

            Save.SetInt("commission", 0);

            Save.SetFloat("commissionTime", 0);

            Save.SetInt("commissionCount", Save.GetInt("commissionCount", 0) + 1);
        }

        public void TakeMothDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeMothDress.AskAboutGown".Localized());

            OutputText("Scenes.Marielle.TakeMothDress.ChimesTwirlingUp".Localized());

            OutputText("Scenes.Marielle.TakeMothDress.EyesWiden".Localized());

            OutputText("Scenes.Marielle.TakeMothDress.PerfectNoOtherWay".Localized());

            OutputText("Scenes.Marielle.TakeMothDress.HeadPopsOut".Localized());

            ClearButtons();

            AddNextButton("Common.Button.PutOn".Localized(), TakeMothDressOn);

            AddNextButton("Common.Button.PutAway".Localized(), TakeMothDressAway);
        }

        public void TakeMothDressOn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeMothDressOn.CantWaitToWear".Localized());

            OutputText("Scenes.Marielle.TakeMothDressOn.SeamstressCertainly".Localized());

            OutputText("Scenes.Marielle.TakeMothDressOn.SteppingIntoGarment".Localized());

            OutputText("Scenes.Marielle.TakeMothDressOn.FloatingTactileBliss".Localized((Player.IsFemale() && Save.GetInt("timesYurid", 0) != 0 ? "her eager--Or at times perhaps a little overeager--fingers adding to your titillation as she tugs the cloth here and there, straightens something out, shakes the skirt into place, and finally draws" : "tugging the cloth here and there, straightening some things out, shaking the skirt into place, and finally drawing")));

            OutputText("Scenes.Marielle.TakeMothDressOn.SayOvermuch".Localized());

            OutputText("Scenes.Marielle.TakeMothDressOn.TestBreathing".Localized());

            OutputText("Scenes.Marielle.TakeMothDressOn.PassingFineBanquet".Localized((Player.IsFemale() && Player.Femininity >= 59 ? "[say:Illecebrous.] You don't even know what that means, but that doesn't matter." : "[say:Right illuminous.] The gown really does seem to emit a shine of its own.")));

            OutputText("Scenes.Marielle.TakeMothDressOn.EverythingPerfect".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            Player.Inventory.Add(Player.SetArmor((Armor)ItemRegistry.Create(ArmorIds.MaidSilkDress)), 1);

            CommissionReturn();
        }

        public void TakeMothDressAway()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeMothDressAway.AdmireLittleMore".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.MaidSilkDress), 1);

            CommissionReturn();
        }

        public void TakeSylviaDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSylviaDress.AskAboutGown".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.ChimesTwirlingUp".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.EyesWiden".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.PerfectNoOtherWord".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.HeadPopsOut".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.AdmireLittleMore".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.FortuneKeepYou".Localized());

            OutputText("Scenes.Marielle.TakeSylviaDress.NatureConqueredTemple".Localized());

            Player.CreateKeyItem("Sylvia's Dress", 0, 0, 0, 0);

            ClearButtons();

            AddNextButton("Npcs.Sylvia.Name".Localized(), null);

            AddNextButton("Common.Button.Camp".Localized(), TakeSylviaDressCamp);
        }

        public void TakeSylviaDressCamp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSylviaDressCamp.NeitherDressNorSylvia".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeMothPanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeMothPanties.AskAboutPanties".Localized());

            OutputText("Scenes.Marielle.TakeMothPanties.AyeMoment".Localized());

            OutputText("Scenes.Marielle.TakeMothPanties.IncrediblySmooth".Localized());

            OutputText("Scenes.Marielle.TakeMothPanties.FantasticUseSilk".Localized());

            OutputText("Scenes.Marielle.TakeMothPanties.VerilyPleasure".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.MothSilkPanties), 1);

            CommissionReturn();
        }

        public void TakeMothBedding()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeMothBedding.AskAboutBedding".Localized());

            OutputText("Scenes.Marielle.TakeMothBedding.ChirpsOfCourse".Localized());

            OutputText("Scenes.Marielle.TakeMothBedding.IgnoringCreator".Localized());

            OutputText("Scenes.Marielle.TakeMothBedding.UtmostPleasure".Localized());

            OutputText("Scenes.Marielle.TakeMothBedding.LetHerFold".Localized());

            Player.CreateKeyItem("Moth Bedding", 0, 0, 0, 0);

            DoNext(() => CommissionReturn());
        }

        public void TakeSpiderDress()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDress.AskAboutDress".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDress.RisingUpTwirl".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDress.MellowCascade".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDress.VoiceFilaments".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDress.TurningFabricOut".Localized());

            ClearButtons();

            AddNextButton("Common.Button.PutOn".Localized(), TakeSpiderDressOn);

            AddNextButton("Common.Button.PutAway".Localized(), TakeSpiderDressAway);
        }

        public void TakeSpiderDressOn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOn.WearItNow".Localized((Player.Femininity > 50 && Player.IsFemale() ? "noticing Marielle's gaze following your hands before it flits back to her own task under the accompaniment of a soft hem as" : "Marielle patiently waiting for you until"), (Player.IsFemale() && Save.GetInt("timesYurid", 0) != 0 ? " that drags on a blink longer than it probably needed to be" : ""), (Player.IsFemale() && Save.GetInt("timesYurid", 0) != 0 ? " that drags on a blink longer than it probably needed to be" : "")));

            OutputText("Scenes.Marielle.TakeSpiderDressOn.PearlSilkVeils".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOn.BitMoreWeight".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOn.SitsAughtAwry".Localized());

            Player.Inventory.Add(Player.SetArmor((Armor)ItemRegistry.Create(ArmorIds.SilkSummerDress)), 1);

            CommissionReturn();

            DoNext(() => TakeSpiderDressOn2());

            AddNextButton("Common.Button.Tease".Localized(), TakeSpiderDressOnTease);
        }

        public void TakeSpiderDressOn2()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOn2.NothingToComplain".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOn2.ClearlyHappy".Localized());

            DoNext(() => CommissionReturn());
        }

        public void TakeSpiderDressOnTease()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOnTease.SingleHandSinuous".Localized(((Player.Femininity > 50 || Save.GetInt("timesYurid", 0) > 0) && Player.IsFemale() ? "She seemed to have rather enjoyed the view earlier, so you decide to give her a little more of that, to tease the woman-loving tailor while making something of a test of the outfit's allure." : "This did indeed turn out to be a fairly alluring outfit, so you decide to test it out on the one person in front of you.")));

            if (Player.HasCock() || (Player.IsGenderless() && Player.Femininity < 59))
            {
                OutputText("Scenes.Marielle.TakeSpiderDressOnTease.NoReplyRightAway".Localized((Player.Femininity >= 59 || Player.BiggestBreastSize() > 0 ? "her gaze hanging for a second longer on your fondled and well-accentuated bust" : "the somewhat puzzled stare she's giving you suggesting that she doesn't quite know what to say")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnTease.AttentionFlitsBack".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnTease.AnswerOfSorts".Localized());

                MarielleMenu();
            }
            else
            {
                OutputText("Scenes.Marielle.TakeSpiderDressOnTease.DistractionOrWordless".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnTease.QuietCough".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnTease.ItRightEmbathes".Localized());

                ClearButtons();

                AddNextButton("Common.Button.MoveOn".Localized(), TakeSpiderDressOnMove);

                AddNextButton("Common.Button.LetHerTouch".Localized(), TakeSpiderDressOnTouch);
            }
        }

        public void TakeSpiderDressOnMove()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOnMove.GotWhatWanted".Localized());

            DoNext(() => CommissionReturn());
        }

        public void TakeSpiderDressOnTouch()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.HandsOnExperience".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.TornAwayGaze".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.PeeksIntoSilk".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.AssentSearchingGlance".Localized((Save.GetInt("timesYurid", 0) != 0 ? " besides, you've done far lewder stuff with her," : "")));

            OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.GrowingMoreEngrossed".Localized((Save.GetInt("timesYurid", 0) + Save.GetInt("timesKissed", 0) > 0 ? "anticipated" : "you hoped")));

            if (Player.Femininity < 59)
            {
                OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.GoesNoFurther".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.QuiteThankYou".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                DoNext(() => CommissionReturn());
            }
            else
            {
                OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.WakeOfCompliment".Localized((Save.GetInt("timesYurid", 0) != 0 ? "following every little clue you give, Or is she" : "or")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnTouch.SlipFromFabric".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 9f));

                ClearButtons();

                AddNextButton("Common.Button.PullIn".Localized(), TakeSpiderDressOnPull);

                AddNextButton("Common.Button.LetGo".Localized(), TakeSpiderDressOnGo);
            }
        }

        public void TakeSpiderDressOnPull()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.WontLeaveUnfinished".Localized((Save.GetInt("timesYurid", 0) + Save.GetInt("timesKissed", 0) > 0 ? "Shamefacedness" : "Trepidation")));

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.HesitationClouds".Localized((Save.GetInt("timesKissed", 0) + Save.GetInt("timesYurid", 0) > 0 ? "vanquish" : "silence"), (Save.GetInt("timesKissed", 0) < 6 ? "You then take her lips" : "Her lips then meet yours"), (Save.GetInt("timesKissed", 0) < 6 ? "reward her with" : "let her keep")));

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.KissKindles".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.FingersBaton".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.PushOrCaress".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.LoneHandClimbs".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.SuspendedBreath".Localized());

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull.CouldIfNoAir".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => TakeSpiderDressOnPull2());
        }

        public void TakeSpiderDressOnPull2()
        {
            ClearOutput();

            if (!Player.IsGenderless() && !Player.IsTaur() && Save.GetInt("timesYurid", 0) != 0)
            {
                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.SheWontLetGo".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.TappingHer".Localized((Save.GetInt("timesInButt", 0) != 0 ? "you register a pussy-slicked straggler's fine, snaking path towards a different hole entirely" : "she supplements that by giving your throbbing pleasure-button an adamant, preferential, and positively lubed-up treatment")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.LegsQuiver".Localized((Save.GetInt("timesInButt", 0) != 0 ? "adamant in her adoration for your [butt], your pleasure-button isn't neglected either. Far from it--writhing all around and across is a veritable whirlpool of fingerpads which, speared and ravished as you are, makes an absolute mess of what cohesion you Still possessed" : "conjuring a veritable whirlpool of writhing fingerpads all across your nethers, she somehow maintains the means to keep kneading your [butt] as well--how she does that with only two hands down there is beyond your mental capacity right now, and as exposed and ravished as you are, what cohesion you Still possessed is being made an absolute mess of")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.NotACry".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.RushOfAir".Localized((Save.GetInt("timesKissed", 0) < 8 ? "Your tongue's tip meets her" : "Her tongue's tip meets your")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.SpendRecuperation".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.AllWellPresume".Localized((View.Time.IsTimeBetween(21.5f, 3f) ? "It's gotten pretty late" : "You should freshen yourself up properly")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.BidYouFarewell".Localized((View.Time.IsTimeBetween(21.5f, 3f) ? "night upon arcadian pastures" : "most safe and speedful journey"), (silly && View.Time.IsTimeBetween(21.5f, 3f) ? " You silently wonder why she'd wish you stay up and spend the night wasting the last of your gems on vain, deciduous entertainment." : "")));
            }
            else
            {
                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.EasierSaidThanDone".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.GirlExclaims".Localized());

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.WhetherGownOrKiss".Localized((View.Time.IsTimeBetween(21.5f, 3f) ? "It's gotten pretty late" : "You should get yourself freshened up")));

                OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.FareYouWell".Localized((View.Time.IsTimeBetween(21.5f, 3f) ? "night upon arcadian pastures" : "most safe and speedful journey"), (silly && View.Time.IsTimeBetween(21.5f, 3f) ? ", though you silently wonder why she'd wish you stay up and spend the night wasting the last of your gems on vain, deciduous entertainment" : "")));
            }

            OutputText("Scenes.Marielle.TakeSpiderDressOnPull2.TadFloaty".Localized());

            MarielleSex();

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeSpiderDressOnGo()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressOnGo.MyUhmStarts".Localized((Save.GetInt("timesYurid", 0) + Save.GetInt("timesKissed", 0) > 0 ? "flushed" : "pale")));

            DoNext(() => CommissionReturn());
        }

        public void TakeSpiderDressAway()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderDressAway.LetHerFoldIt".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.SilkSummerDress), 1);

            CommissionReturn();
        }

        public void TakeSpiderBra()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderBra.HereForBra".Localized());

            OutputText("Scenes.Marielle.TakeSpiderBra.CheerfullySwirling".Localized());

            OutputText("Scenes.Marielle.TakeSpiderBra.SecondSkin".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkBra), 1);

            CommissionReturn();
        }

        public void TakeSpiderPanties()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderPanties.BringUpPanties".Localized());

            OutputText("Scenes.Marielle.TakeSpiderPanties.CertainlyAtOnce".Localized());

            OutputText("Scenes.Marielle.TakeSpiderPanties.LanternGlowSheen".Localized());

            OutputText("Scenes.Marielle.TakeSpiderPanties.TisPleasureWholly".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkPanties), 1);

            CommissionReturn();
        }

        public void TakeSpiderUnderwear()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderUnderwear.InquireLingerieSet".Localized());

            OutputText("Scenes.Marielle.TakeSpiderUnderwear.YesStraightAway".Localized());

            OutputText("Scenes.Marielle.TakeSpiderUnderwear.CalmLamplight".Localized());

            OutputText("Scenes.Marielle.TakeSpiderUnderwear.MayTheyAttendWell".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkBra), 1);

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.SilkPanties), 1);

            CommissionReturn();
        }

        public void TakeSpiderBedding()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TakeSpiderBedding.BringUpBedclothes".Localized());

            OutputText("Scenes.Marielle.TakeSpiderBedding.HopedNotDispleased".Localized((Save.GetInt("commissionCount", 0) != 0 ? "once again " : "")));

            OutputText("Scenes.Marielle.TakeSpiderBedding.IsItWellReceived".Localized());

            OutputText("Scenes.Marielle.TakeSpiderBedding.HelpedStow".Localized());

            Player.CreateKeyItem("Spider-Silk Bedding", 0, 0, 0, 0);

            DoNext(() => CommissionReturn());
        }

        public void ShopModel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ShopModel.WonderIfSheModels".Localized());

            OutputText("Scenes.Marielle.ShopModel.StrandBehindEar".Localized());

            OutputText("Scenes.Marielle.ShopModel.VestiaryAssistance".Localized());

            Save.SetBool("askedModeling", true);

            HideButton(3);

        }

        public void LewdMenu(bool output = false)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Marielle.LewdMenu.RegardPaleGirl".Localized());

                OutputText("Scenes.Marielle.LewdMenu.ObviouslyWaiting".Localized());
            }

            ClearButtons();

            AddNextButton("Common.Button.Kiss".Localized(), Kiss);

            AddNextButton("Common.Button.Headpat".Localized(), Headpat);

            AddNextButton("Common.Button.Handholding".Localized(), Handholding);

            AddNextButton("Common.Button.TaurRide".Localized(), TaurRide);

            AddButton(14, "Common.Button.Back".Localized(), () => LewdBack());
        }

        public void LewdBack()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.LewdBack.ConsiderWhatElse".Localized());

            MarielleMenu();
        }

        public void Kiss()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Kiss.CaughtByLips".Localized((silly || Player.Intelligence < 1 ? ", and that's quite a lot, by your standards" : "")));

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.Kiss.EyesWidenRealization".Localized());

                OutputText("Scenes.Marielle.Kiss.CloseSeePalm".Localized());

                OutputText("Scenes.Marielle.Kiss.FidgetsApology".Localized());

                if (Save.GetInt("timesSexed", 0) > Save.GetInt("timesYurid", 0))
                {
                    OutputText("Scenes.Marielle.Kiss.CantLetYouKiss".Localized((Save.GetInt("timesKissed", 0) != 0 ? " any more" : "")));

                    OutputText("Scenes.Marielle.Kiss.VoiceRises".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.Kiss.WhyNotTurnDown".Localized((Save.GetInt("timesKissed", 0) != 0 ? "It's the dick, isn't it? " : ""), (Save.GetInt("timesYurid", 0) == 0 ? "just " : ""), (Save.GetInt("timesKissed", 0) != 0 ? " while you have one" : "")));

                    OutputText("Scenes.Marielle.Kiss.VoiceEbbs".Localized());
                }

                OutputText("Scenes.Marielle.Kiss.EyesAverted".Localized());

                if (Player.HasVagina() && Player.IsFeminine())
                {
                    OutputText("Scenes.Marielle.Kiss.YouAreAWoman".Localized((Save.GetInt("timesKissed", 0) + Save.GetInt("timesYurid", 0) > 0 ? " now" : "")));

                    OutputText("Scenes.Marielle.Kiss.StartsQuiteAware".Localized());

                    OutputText("Scenes.Marielle.Kiss.MyApologiesContemn".Localized());
                }
                else
                    if (Player.Femininity >= 59)
                    {
                        OutputText("Scenes.Marielle.Kiss.ClaimsNotAttracted".Localized());

                        OutputText("Scenes.Marielle.Kiss.VoiceShootsUp".Localized());
                    }

                OutputText("Scenes.Marielle.Kiss.FallsSilent".Localized());

                Save.SetInt("kissDenied", Save.GetInt("kissDenied", 0) + 1);

                LewdMenu();
            }
            else
                if (Player.IsGenderless() && Player.Femininity < 51)
                {
                    OutputText("Scenes.Marielle.Kiss.FrowningGirlFreezes".Localized());

                    OutputText("Scenes.Marielle.Kiss.BlinksAndStirs".Localized());

                    OutputText("Scenes.Marielle.Kiss.FirstAttempts".Localized());

                    OutputText("Scenes.Marielle.Kiss.IntoningNoMan".Localized());

                    OutputText("Scenes.Marielle.Kiss.DoubtCallingUgly".Localized());

                    Save.SetInt("kissDenied", Save.GetInt("kissDenied", 0) + 2);

                    LewdMenu();
                }
                else
                {
                    if (Save.GetInt("timesKissed", 0) >= 4 && Save.GetInt("insulted", 0) == 0)
                    {
                        OutputText("Scenes.Marielle.Kiss.RealizesWhatYouWant".Localized());

                        OutputText("Scenes.Marielle.Kiss.AdvancingKisses".Localized());

                        OutputText("Scenes.Marielle.Kiss.AsideFromGulps".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                    }
                    else
                    {
                        OutputText("Scenes.Marielle.Kiss.ColorRises".Localized());

                        OutputText("Scenes.Marielle.Kiss.ColdGivePause".Localized());

                        OutputText("Scenes.Marielle.Kiss.NoMoveToResist".Localized());
                    }

                    Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);

                    ClearButtons();

                    AddNextButton("Common.Button.JustKiss".Localized(), JustKiss);

                    AddNextButton("Common.Button.RideFace".Localized(), RideFace);

                    AddNextButton("Common.Button.BoobPlay".Localized(), BoobPlay);

                    AddNextButton("Common.Button.Masturbate".Localized(), () => Masturbate(false));
                }
        }

        public void JustKiss()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.JustKiss.BreakKissBreathe".Localized());

            OutputText("Scenes.Marielle.JustKiss.LiddedSteelEyes".Localized((Save.GetInt("timesKissed", 0) > 5 && Save.GetInt("insulted", 0) == 0 ? "feel her lightly tug at [if (!isnaked) {your [armor]|you}] with some yet-unfulfilled longing. You don't resist the tempting pull and, adopting a smile, help Marielle fumble for your lips, her breath chilly against your face until she finds them and seals your mouth with hers again" : "don't care to resist the temptation her lips present, and with a smile, swiftly seal them again")));

            OutputText("Scenes.Marielle.JustKiss.PagePlaceholderPeriod".Localized((Player.Femininity >= 59 && Save.GetInt("insulted", 0) == 0 && Save.GetInt("timesKissed", 0) > 7 ? "She doesn't just stop at that. It's like you've flipped a switch inside her, as when her eyes drift shut, the tip of her tongue prods you, silently but insistently asking for entry. You have no reason to deny her" : "You don't let it stop just there. Opening up, you extend a silent invitation, nudging her playfully. Readily, she takes it")));

            OutputText("Scenes.Marielle.JustKiss.SweetSourness".Localized());

            OutputText("Scenes.Marielle.JustKiss.DepartFromLips".Localized());

            OutputText("Scenes.Marielle.JustKiss.FailsToVocalize".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            SexReturn("[pg]This seems to be a good stopping point for the day. With her taste still fresh on your mind and tongue, you offer her a good night, drawing her attention from somewhere else back to you.[pg][say:Oh, uhm...] She hangs there, three arms awkwardly suspended while she peers at her oil lamp, then into the temple's dark interior. [say:Hmm, ah, yes. I bid you a stillsome night, then, [name],] Marielle says with a deep, measured bow and a smile on her face. [say:Graces upon you.][pg]She's still watching you when you briefly turn on the steps leading up, [if (insultedfather) {though she busies herself with something to the side before your eyes can meet, and|cooling herself off with a folding fan and flicking you a small, final wave before}] you [if (singleleg) {slide|step}] outside into the moonlit swamp. The sky is clear, and so is the area--nothing bothers you as you trek over muddy earth and thick, hardy grass until the familiar ground around your campsite crunches [if (singleleg) {under your [legs]|underfoot}] once more.",

                "[pg]It occurs to you that you must have spent several hours here by now, and this seems like a nice final note to stop on, for the time being. You tell Marielle, drawing her eyes back to you from where she was trying to inspect and tidy her hair.[pg][say:Oh, ah, as you will. Uhm... speed and peace unto you,] she says as she dips into a hasty but deep curtsy. [say:Adieu, [name], farewell.][pg]You reciprocate the goodbye with one of your own and make your way out of the temple, briefly glancing back on the threshold to see her [if (insultedfather) {brushing her long tresses|watching you leave}] before you [if (singleleg) {glide|step}] out into the miry bog. Under belligerent mosquito squadrons and the serenades of various amphibious creatures, you hike off into the direction of your [cabin].",

                "[pg]It takes some time, but after pensively tidying her hair and applying the cooling gusts of a silken folding fan to the heat on her face, she seems to have gathered herself enough to meet your eyes again. However, her heightened mood still seeps through in her voice as she says, [say:Well, uhm, was there... aught else you desired?]",

                () => LewdMenu());
        }

        public void RideFace()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.RideFace.GrowingExcited".Localized());

            OutputText("Scenes.Marielle.RideFace.LipsLocked".Localized((Save.GetInt("timesKissed", 0) >= 4 && Save.GetInt("insulted", 0) == 0 ? "finally " : "")));

            OutputText("Scenes.Marielle.RideFace.SharedGulpAir".Localized());

            OutputText("Scenes.Marielle.RideFace.WastesNoTime".Localized());

            OutputText("Scenes.Marielle.RideFace.DrawDeepBreath".Localized());

            OutputText("Scenes.Marielle.RideFace.SurfacesWithGasp".Localized());

            OutputText("Scenes.Marielle.RideFace.ScoreOfFingers".Localized());

            OutputText("Scenes.Marielle.RideFace.ExhalesHeavyHot".Localized());

            OutputText("Scenes.Marielle.RideFace.GorgeousSmothered".Localized());

            OutputText("Scenes.Marielle.RideFace.RightOnCue".Localized());

            OutputText("Scenes.Marielle.RideFace.FingersFlirtGrope".Localized());

            OutputText("Scenes.Marielle.RideFace.SweatyHandGrasp".Localized());

            OutputText("Scenes.Marielle.RideFace.TranquilStupor".Localized());

            OutputText("Scenes.Marielle.RideFace.LastTongueDraw".Localized());

            OutputText("Scenes.Marielle.RideFace.QuiteHopePleasure".Localized());

            Player.Orgasm("Vaginal");

            MarielleSex();

            ClearButtons();

            AddNextButton("Common.Button.GetUp".Localized(), RideFaceGetUp);

            AddNextButton("Common.Button.Kiss".Localized(), RideFaceKiss);

            AddNextButton("Common.Button.HerTurn".Localized(), RideFaceHerTurn);
        }

        public void RideFaceHerTurn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.RideFaceHerTurn.HaventGotHerOff".Localized());

            OutputText("Scenes.Marielle.RideFaceHerTurn.ExpressionTurnsConfused".Localized());

            OutputText("Scenes.Marielle.RideFaceHerTurn.LookSpeaksVolumes".Localized());

            AddButton(2, "Common.Button.HerTurn".Localized(), null);
        }

        public void RideFaceKiss()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.RideFaceKiss.FingersOnHerHead".Localized());

            OutputText("Scenes.Marielle.RideFaceKiss.WetSmacksPants".Localized());

            OutputText("Scenes.Marielle.RideFaceKiss.BreakKissRegret".Localized());

            OutputText("Scenes.Marielle.RideFaceKiss.FeltLikeNeeded".Localized());

            Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);

            AddButton(1, "Common.Button.Kiss".Localized(), null);
        }

        public void RideFaceGetUp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.RideFaceGetUp.FinalAppreciativePat".Localized());

            OutputText("Scenes.Marielle.RideFaceGetUp.UhmThankYou".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            SexReturn("Although Marielle's movements are swift and concise, there is a hint of sluggishness behind them, and one free hand briefly rises to veil a yawn behind its fingers, reminding you how late it's gotten. You don't have to say much to get a contemplative [say:Hmm] out of her.[pg][say:'Tis an eldened eve indeed,] she says, eyeing the long lock of blonde she has woven around her index and ring fingers. [say:I ween I should to bed erelong...] Another short yawn. [say:Well, thus let us.] The girl then swivels to you and drops down into a neat curtsy. [say:Pray you, have the sweetest night, [name], and, ah... repair you hither whensoever a whim would will you.][pg]You take your leave from the [if (silly) {gay|buoyant}] seamstress and throw her a final goodnight before you have to find your way back through the gloomy, murky wilds of the marsh.",

                "She's quick and efficient in her movements, but something seems inattentive about them. When she raises a hand to rub and massage her neck, you're made aware of how much time you've spent here--two hours, maybe more. Going back to camp for now sounds like a good idea, so you draw Marielle's attention to trade goodbyes with her.[pg][say:Ah, yes...] She halts mid-motion for a moment, her fingers stuck between her lengthy tresses until she nods, finishes the stroke, and bends into a proper curtsy. [say:Adieu, then, and, uhm, well... 'twould be a thing of fainness, betook you yourself eft hither... 'Twas most licksome.] A second passes before her jaw snaps shut, her cheeks suddenly a field of poppies in full bloom. [say:I-I mean, [i:likesome]. Yes, verily likesome.][pg]The girl doesn't look keen on adding anything else, so you leave her to her self-inflicted abashment. After a farewell of your own, you take on the trek over the reed- and gnat-nurturing morass towards the comfortable confines of your [cabin].",

                "She makes swift work of both tasks, deftly nudges the fallen cloth rolls upright along the way, then takes a seat on her usual stool and turns towards you. There is still a bit of a flush coloring her otherwise naturally pasty cheeks, but Marielle seems collected enough as she addresses you again.[pg][say:Well... if that be all, then I shall keep you not overlong. Or, ah, is there aught else I may]--her thighs shift ever so slightly beneath her dress as she pauses--[say:assist you with?]",

                () => LewdMenu());
        }

        public void BoobPlay()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.BoobPlay.RubbingAgainstChest".Localized());

            OutputText("Scenes.Marielle.BoobPlay.FingersOverGown".Localized());

            OutputText("Scenes.Marielle.BoobPlay.MurmursYourName".Localized());

            OutputText("Scenes.Marielle.BoobPlay.ScarredButPale".Localized());

            OutputText("Scenes.Marielle.BoobPlay.ArmsLowerFiddle".Localized());

            OutputText("Scenes.Marielle.BoobPlay.WouldntDoIfNot".Localized());

            OutputText("Scenes.Marielle.BoobPlay.SoftDespiteSmall".Localized());

            OutputText("Scenes.Marielle.BoobPlay.EntrancedHazeHands".Localized());

            OutputText("Scenes.Marielle.BoobPlay.ChestEclipsesHers".Localized());

            OutputText("Scenes.Marielle.BoobPlay.LongBlissfulMinutes".Localized());

            OutputText("Scenes.Marielle.BoobPlay.ToneSofterVelvet".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ClearButtons();

            AddNextButton("Common.Button.Agree".Localized(), BoobPlayAgree);

            AddNextButton("Common.Button.Breastfeed".Localized(), Breastfeed);

            AddNextButton("Common.Button.Masturbate".Localized(), () => Masturbate(true));
        }

        public void BoobPlayAgree()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.BoobPlayAgree.TemptingButStay".Localized());

            OutputText("Scenes.Marielle.BoobPlayAgree.AnotherNeckPeck".Localized());

            OutputText("Scenes.Marielle.BoobPlayAgree.ProperlySettledIn".Localized());

            OutputText("Scenes.Marielle.BoobPlayAgree.GiggleEscapes".Localized());

            OutputText("Scenes.Marielle.BoobPlayAgree.CouldDoAllDay".Localized());

            OutputText("Scenes.Marielle.BoobPlayAgree.RosinessReturns".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            View.Time.PassMinutes((int)Round((1) * 60));

            SexReturn("[pg]Though, she's also beginning to look a little sleepy. It's gotten late, and a small yawn she hides behind her fingers is your cue to wish her a good night for now.[pg][say:Oh.] She peeks outside, lingers there in brief thought, then faintly bobs her head and gets up. [say:I, ah... pray you will have a likewise restful night anon, [name],] the girl says as she dips you a curtsy, her smile accompanying you all the way outside as you leave the old, shadow-blanketed sanctuary.[pg]It's an uneventful trip back to camp, and the nightly stillness of the bog leaves you ample time and opportunity to let your mind wander.",

                "[pg]You feel like you've been here for a good while now though, so you start to take your leave from the girl, her smile briefly yielding to deliberation, but regaining its command as she quickly dips you a curtsy.[pg][say:As you will. Well, 'twas... most pleasant a time,] she says, rising and lacing her fingers. [say:Fare you well, [name], and, ah... pray tread you twixt weal and vigilancy.][pg]The bog seems relatively peaceful as you [if (singleleg) {slither|step}] outside, and you do indeed make it back to your campsite without any great trouble or interruption, besides a few pesky insects following you about, trying to harass you until you've cleared their territory.\n",

                "[pg][say:Well, uhm, [name]]--she expectantly folds her upper pair of hands on her lap--[say:be there aught else I may... do for you?]", () => LewdMenu());
        }

        public void Breastfeed()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Breastfeed.BetterIdea".Localized());

            OutputText("Scenes.Marielle.Breastfeed.FingersPlayCheek".Localized());

            OutputText("Scenes.Marielle.Breastfeed.HaveYouBegins".Localized());

            if (!Save.GetBool("breastfed"))
            {
                OutputText("Scenes.Marielle.Breastfeed.StartlesDrawsAway".Localized());

                OutputText("Scenes.Marielle.Breastfeed.MotherWhisper".Localized((Player.IsChild() && Player.Pregnancy.Incubation != 0 ? "... No, I suppose 'twas writ in boldest script.] She briefly glances down at your belly. [say:But... truly, with child? That ought not be-- I mean, you are yet so--" : (Player.IsChild() ? "-- I mean... in... in the May of your youth? That ought not-- How?" : (Player.Pregnancy.Incubation != 0 || (silly && Player.Thickness >= 80 && Player.Tone < 80) ? "-- No, I mean, I suppose I ought have known you were...] She glances briefly over your [if (!ispregnant) {childless but no less bulky }]belly as she trails off. [say:With child. Is this... truly all right?" : " were...] Something seems to dawn on her. [say:Is... Is this truly all right, then? Ought you not-- I mean..."))), (Player.Pregnancy.Incubation != 0 || Player.HasChildren() ? " " : "--you're no mother, nor are you about to be one--")));

                OutputText("Scenes.Marielle.Breastfeed.CouldntComplain".Localized());

                Save.SetBool("breastfed", true);
            }
            else
            {
                OutputText("Scenes.Marielle.Breastfeed.PauseAtLactating".Localized());

                OutputText("Scenes.Marielle.Breastfeed.MindfulAttentions".Localized());
            }

            OutputText("Scenes.Marielle.Breastfeed.CoolTongueDancing".Localized());

            OutputText("Scenes.Marielle.Breastfeed.LostSenseOfTime".Localized());

            OutputText("Scenes.Marielle.Breastfeed.MouthLetsUp".Localized());

            OutputText("Scenes.Marielle.Breastfeed.CooAppreciation".Localized());

            OutputText("Scenes.Marielle.Breastfeed.CourseOfMinutes".Localized());

            OutputText("Scenes.Marielle.Breastfeed.SoothingRelief".Localized());

            OutputText("Scenes.Marielle.Breastfeed.TelltaleTightening".Localized());

            OutputText("Scenes.Marielle.Breastfeed.OrgasmBreaksFree".Localized());

            OutputText("Scenes.Marielle.Breastfeed.SatisfiedSigh".Localized());

            OutputText("Scenes.Marielle.Breastfeed.MilkOnCheek".Localized());

            OutputText("Scenes.Marielle.Breastfeed.FansHerself".Localized());

            Player.Milked();

            ClearButtons();

            AddNextButton("Common.Button.GetUp".Localized(), () => Breastfeed2(0));

            AddNextButton("Common.Button.Kiss".Localized(), () => Breastfeed2(1));

            AddNextButton("Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), () => Breastfeed2(2));
        }

        public void Breastfeed2(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.Breastfeed2.TimeToGetUp".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.RouseHastilyFinishes".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.ThankYouPullsUp".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.Breastfeed2.NiceViewStraddling".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.TooLittleTime".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.StrengthSuckedOut".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.HumsQuiteFine".Localized());

                        Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.Breastfeed2.NoHurryToRise".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.HandOnYours".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.FingersStopHeartbeats".Localized());

                        OutputText("Scenes.Marielle.Breastfeed2.PokesAreola".Localized(
                            Player.Femininity >= 59 ? (Player.BiggestBreastSize() > 6 ? "I do find quite pleasing" : (Player.BiggestBreastSize() < 2 || Player.BiggestBreastSize() > 4 ? "I do adore" : "exceed indeed")) : (Player.BiggestBreastSize() < 2 || Player.BiggestBreastSize() > 5 ? "I do quite like" : "I do find quite sightly indeed")
                        ));

                        OutputText("Scenes.Marielle.Breastfeed2.TakeProfferedHands".Localized());
                    }

                    break;
            }

            View.Time.PassMinutes((int)Round((1) * 60));

            SexReturn("[pg]What a treat that was. For the both of you, quite evidently. But it's gotten late now, you realize when you catch Marielle hiding a yawn while you [if (!isnaked) {readjust your [armor]|gather your bearings}]--you should get going.[pg][say:Hmm?] comes from the seamstress when you give her your goodbye, her mouth closing and her hand falling. You repeat yourself, just in case.[pg][say:Oh, of course. I pray you, then]--her smile reappearing, she drops into a deep curtsy--[say:have a most delightsome night.][pg]You'll try, though the outside greets you with a waft of humidity, stuffing your lungs with air that feels more like soggy cotton than anything breathable as you soldier back through the moonlit mire.",

                "[pg]You should probably get back to camp soon, you come to think as you're gathering your bearings[if (!isnaked) { and fixing your [armor]}]--it's been a good while.[pg][say:Oh?] comes from Marielle when you give her your goodbye, her head swivelling up from a lock of hair she was inspecting. You repeat yourself, just in case.[pg][say:Ah, yes, of course.] She dips into a curtsy and widens the smile on her lips a little. [say:Pray you, have a most delightsome day, then, [name], and come again if ever you... uhm...] Her voice tails off, distant birdsong filling the gap as you, half-turned to leave, watch the seamstress attempt to complete her sentence. She gives up soon enough with a small shake of her head, another swift curtsy, and another, [say:Pray come again.][pg]The outside greets you with quite a flock of brown-white birds gathered in the treetops around the temple. Sparrows, [if (intelligence < 50) {perhaps|looks like}]. Some twitter cautiously as you pass underneath them, but none are concerned enough to be scared up by your departing presence.",

                "[pg][if (!isnaked) {Fixing your own [armor]|Gathering your bearings}] is a relatively quick affair.[if (breastrows > 1) { You find you're a little disappointed she didn't go on to suckle from your remaining mammaries, but that fleeting spell of dismay hardly matters in your general contentment.}] Marielle all the while shuffles awkwardly around, watching and waiting until you vacate her working area to let her take her seat again, which she does with a gracious bob. Her fingertips then playing around a blonde lock, she regards you with amicable eyes.[pg][say:So, ah... was there aught else you... desired of me, [name]?]",

                () => LewdMenu());
        }

        public void Masturbate(bool fondled)
        {
            ClearOutput();

            if (fondled)
            {
                OutputText("Scenes.Marielle.Masturbate.CantWaitLonger".Localized());

                OutputText("Scenes.Marielle.Masturbate.TakeHandGuiding".Localized());

                OutputText("Scenes.Marielle.Masturbate.OhComesOut".Localized());

                OutputText("Scenes.Marielle.Masturbate.SayAthirst".Localized());

                OutputText("Scenes.Marielle.Masturbate.SuckInBreath".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.Masturbate.ConsiderablyHorny".Localized());

                OutputText("Scenes.Marielle.Masturbate.HandDivesHips".Localized((Save.GetInt("timesKissed", 0) >= 4 && Save.GetInt("insulted", 0) == 0 ? "once more" : "lower")));

                OutputText("Scenes.Marielle.Masturbate.ArtisanDexterity".Localized());

                OutputText("Scenes.Marielle.Masturbate.AwakenedArms".Localized());
            }

            OutputText("Scenes.Marielle.Masturbate.SoCloseAttentive".Localized());

            OutputText("Scenes.Marielle.Masturbate.MiniaturePeak".Localized());

            OutputText("Scenes.Marielle.Masturbate.GlanceBehindGirl".Localized());

            OutputText("Scenes.Marielle.Masturbate.TensesHoldingFast".Localized());

            OutputText("Scenes.Marielle.Masturbate.LipsMeetAgain".Localized());

            OutputText("Scenes.Marielle.Masturbate.IncredibleTightness".Localized((fondled ? "initial" : "earlier")));

            OutputText("Scenes.Marielle.Masturbate.WhispersInHuffs".Localized());

            OutputText("Scenes.Marielle.Masturbate.WoodCreaks".Localized());

            OutputText("Scenes.Marielle.Masturbate.SoonNothingMore".Localized());

            OutputText("Scenes.Marielle.Masturbate.CalmDownWinded".Localized());

            OutputText("Scenes.Marielle.Masturbate.EyesOpenDevoid".Localized());

            Player.Orgasm("Vaginal");

            MarielleSex();

            ClearButtons();

            AddNextButton("Common.Button.Continue".Localized(), () => Masturbate2(0));

            AddNextButton("Common.Button.LickIt".Localized(), () => Masturbate2(1));

            AddNextButton("Common.Button.MakeHer".Localized(), () => Masturbate2(2));
        }

        public void Masturbate2(int choice)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Masturbate2.PushingBackAStray".Localized(
                new[] { "Pushing back a stray strand of blonde with that hand, you lean down and capture her lips.",

            "Flashing her a suggestive grin, you bring those fingers up to your lips before opening up and sensuously easing them inside.[pg]Marielle watches with silent, rapt attention as you give her a private show of licking clean the digits that were inside her pussy mere moments ago and make sure she knows just how much you enjoy her most intimate of tastes. When you slide them out again with a wet smack, her eyes are still glued to you, her lips ajar ever so slightly, as if in longing. Well, that's an invitation you'll gladly take. Leaning down, you push a strand of hair from her face and capture them once more.",

            "Your smile shifting into a suggestive grin, you hold the drenched fingers out to her, then press them between her supple lips. Without a moment's hesitation, Marielle opens up for you, and you ease them inside.[pg]She gets exceedingly swept up in cleaning them, locking her bedazzling eyes with yours while slithering her cold tongue around and between as if she were eating you out--an implied offer you're sure she wouldn't back down from later. When you slide your fingers out again with a sucking smack, they're even wetter now than before. It hardly matters, though; her lustful display has left you longing for more, and you take advantage of her half-opened lips to swiftly capture them for yourself." }[choice]
            ));

            OutputText("Scenes.Marielle.Masturbate2.ShallowToDeeperKisses".Localized());

            View.Time.PassMinutes((int)Round((1) * 60));

            SexReturn("it's gotten quite late now, and you ought to be making your way back to camp.[pg][say:Oh...] she utters when you tell her that, composing herself a moment later, finally releasing you, and craning her neck to glance outside. A quiet sigh trickles out of her, but she still sits herself up.[pg][say:Aye, the hour would beg it.] One of Marielle's pairs of hands repairs her clothing to a state of relative properness, the other holds itself out to you in tacit request. She nods in thanks as you help her stand, then tugs you into a brief embrace that she uses to bestow just the [if (insultedfather) {swiftest|softest}] of kisses onto your lips[if (!isnaked) { while fixing your [armor] in the same effortless motion}]. [say:A night most restful upon you, [name],] is what she whispers before parting and stepping back to allow you to leave her warmly smiling face and this quiet, overgrown sanctuary behind.[pg]The usual mugginess hasn't quite yet dissipated in the sun's absence, but it feels nonetheless refreshing to be out and about in the open air, your gait light and confident until your [cabin] comes into view.",

                "you've spent quite some time here, and Marielle herself is starting to look a little tuckered out, in spite of her enthusiasm. You ought to be going.[pg]She tentatively nods when you relay that to her, throwing a glance outside before pulling you back in for one last, brief kiss. [say:Let it be so, then. I, uhm... bid you safe journeys,] she whispers before releasing you. After having disentangled [if (!isnaked) {yourself and readjusted your [armor]|and stretched yourself}], you turn to find the girl still just lying there, as you left her, atop the covered crates, her flat chest on full display. At least her skirt has had the decency to fall back down over her legs. You ask if everything's all right.[pg][say:Hmm?] You must have popped some kind of daydream-bubble of hers. [say:Oh... I, ah... Aye, I will but need... rest awhile. Pray fare you well, [name], and ere I fust and dust here shall I do as you.][pg]Well, one pair of hands is now mindful enough to tend to the state of her dress, though the other stays lazily draped over her midriff. You trust her judgement and take your leave, letting the seamstress sort herself out at her own pace while you make your way back through mire, marsh, and mosquitoes.",

                "you can't spend the whole [day] sprawled out atop these boxes, making out.[pg][say:Ah,] she utters when you tell her that, finally releasing you with some lingering regret in her voice. [say:Aye, I suppose 'twould be... rather imprudent.][pg]You can't quite leave it at that, so you deliver a final tickling kiss right to the middle of her bosom, calling forth the sweet giggle you wanted to hear before standing up. She's a tantalizing sight: dainty, white as milk, half-naked, with her long, golden hair spilt out like a corona around her supine body. It takes [if (libido > 60) {a good amount of|some}] self-restraint to help her up instead of jumping into a second round right away.[pg]You spend the [if (!isnaked) {mutual re-dressing in satisfied silence, stealing occasional glances at the other while you get yourselves back in order.|time she takes fixing her gown and hair in satisfied silence, watching her move with swift, practical motions.}] Apparently content with her state of dress, the seamstress then perches herself atop her old stool, facing you, and folds her hands on her lap.[pg][say:Well, [name]...] She pauses for a moment, wiping something from the corner of her mouth before resuming. [say:Wished you for, ah, aught else?]",

                () => LewdMenu());
        }

        public void Headpat()
        {
            ClearOutput();

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            if (Save.GetInt("timesPatted", 0) == 0)
            {
                if (View.Time.IsTimeBetween(8, 10))
                {
                    OutputText("Scenes.Marielle.Headpat.PutOnHerHat".Localized());

                    OutputText("Scenes.Marielle.Headpat.AsksWhenLiftOff".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.Headpat.HatHangingOff".Localized());

                    OutputText("Scenes.Marielle.Headpat.KnucklesAcrossHead".Localized());
                }

                OutputText("Scenes.Marielle.Headpat.UhmSheAsksAgain".Localized());

                OutputText("Scenes.Marielle.Headpat.SayIsItSo".Localized());

                OutputText("Scenes.Marielle.Headpat.FallsSilentPosture".Localized());

                OutputText("Scenes.Marielle.Headpat.EffortToPlease".Localized());

                SexReturn("[pg]Though it could also be that she's slowly dozing off. Considering the hour, you wouldn't be surprised. You wrap it up with a final pat, straightening out some of the mess you've made, and tell Marielle to have a good night.[pg][say:Hmm? Ah. Hm, yes,] she says, needing a bit to recollect herself before she dips into a light curtsy.[pg][say:Well, likewise, [name]. A good night to you.][pg]An " + (silly || Player.Intelligence < 1 ? "entirely unknown creature's toot toot" : "owl's call") + " echoes through the temple as you're about to exit, and you stop on the threshold to listen. It " + (silly || Player.Intelligence < 1 ? "tootles" : "calls") + " again, the low, " + (silly || Player.Intelligence < 1 ? "frightening" : "hooting") + " sound coming from somewhere up above, from the shadows of the rafters. You can't spy the " + (silly || Player.Intelligence < 1 ? "tooter" : "caller") + " though, and on one of the longer pauses, you decide to be on your way.",

                    "[pg]This should be a good point on which to wrap up your progressively lengthy visit, so with a final pat, you gather the resolve to do exactly that.[pg][say:Ah...] comes from Marielle when you take your leave from her. One of her hands reaches to smooth down some of the mess you've left her with. She takes a short breath, lets it fade away, but then mutters a, [say:Hm, yes, of course.] A quick[if (femininity >= 59) { yet hesitant}] bow. [say:Fare you well, [name]; safe journeys,] the girl says in a much clearer voice, sending you off into the surprisingly serene, though still mosquito-ridden bog.",

                    "[pg]Some blissful moments later though, you let up, drawing a last few strands through your fingers before [if (singleleg) {sliding|stepping}] back. She opens her eyes.[pg][say:Ah...] comes out of Marielle, almost wistful. One of her upper hands reaches to smooth down some of the mess you've left her with while she visibly mulls over what to say. With a fleeting frown, she takes a breath to start, but then lets it fade away again and clears her throat instead" + (Player.Femininity >= 59 && Save.GetInt("insulted", 0) == 0 ? ", hesitating" : "") + ".[pg][say:Well, ah... be there[if (femininity >= 59) {...}] aught else?]",

                    () => LewdMenu());
            }
            else
            {
                OutputText("Scenes.Marielle.Headpat.LongStraightTresses".Localized((View.Time.IsTimeBetween(8f, 10f) ? "a stray ray of sunlight" : "the lamplight")));

                OutputText("Scenes.Marielle.Headpat.UhmUttersShe".Localized((View.Time.IsTimeBetween(8f, 10f) ? ", takes her straw hat off," : "")));

                OutputText("Scenes.Marielle.Headpat.DiffidentResponse".Localized());

                OutputText("Scenes.Marielle.Headpat.LongCirclesScalp".Localized());

                OutputText("Scenes.Marielle.Headpat.DontDisturbPeace".Localized());

                OutputText("Scenes.Marielle.Headpat.MixInPats".Localized());

                OutputText("Scenes.Marielle.Headpat.WispsOutOfPlace".Localized());

                SexReturn("[pg]Though perhaps it's not too difficult a destination to guess; she looks like she'll drift off at any moment. Before she can do that, you squeeze in a goodnight.[pg][say:Huh? Ah.] Temporarily revitalized, the seamstress takes a quick breath and stretches her neck. A light dip, an absent[if (!insultedfather) { but candid}] smile, and a, [say:Likewise, a good night to you, [name],] later, you're on your way out of the temple, headed into the dark of the nightly marshlands on a trail that should be leading you home.",

                    "[pg]She's starting to look a little out of it, perhaps. It might not be all there is to it, but you do take that as a sign that you should be finding your way back now.[pg]Your voice doesn't have any effect at first, but on a second attempt, Marielle finally does blink, look up, and utter a somewhat absent [say:Hm]. After briefly patting down her own hair, she then offers you a proper bow.[pg][say:Aye, prithee... fare you well then, [name].][pg]With your own goodbye, you leave the seamstress to herself and exit the sanctum. Some small animal makes a hasty flight from you into the underbrush, startling a family of toads into a series of splashes. A lizard, you'd guess, but you don't pay it any further attention.",

                    "[pg]It takes her another few seconds to take notice of your hands' absence, which is when she finally stirs and looks up. [say:Ah, done[if (insultedfather) {, you are| so soon}]... Hm.] A hand moves to fix the leftovers of the mess you made. Two more tend to the tips of her hair, though for no apparent purpose. [say:Well, uhm...] A slight pause.[pg][say:Might there... be aught else you wished of me?]",

                    () => LewdMenu());
            }

            Save.SetInt("timesPatted", Save.GetInt("timesPatted", 0) + 1);
        }

        public void Handholding()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Handholding.BrowRisesUp".Localized());

            ClearButtons();

            if (Save.GetInt("timesHandHeld", 0) != 0)
            {
                OutputText("Scenes.Marielle.Handholding.AhYetAgain".Localized());

                AddNextButton("Common.Button.JustHold".Localized(), () => HandholdingStop(false));

                AddNextButton("Common.Button.Detach".Localized(), () => HandholdingLet(false));
            }
            else
            {
                OutputText("Scenes.Marielle.Handholding.GazesInQuestion".Localized());

                AddNextButton("Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), () => HandholdingStop(true));

                AddNextButton("Common.Button.LetHer".Localized(), () => HandholdingLet(true));
            }

            Save.SetInt("timesHandHeld", Save.GetInt("timesHandHeld", 0) + 1);
        }

        public void HandholdingStop(bool firstTime)
        {
            ClearOutput();

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            if (firstTime)
            {
                OutputText("Scenes.Marielle.HandholdingStop.StopSeamstress".Localized());

                OutputText("Scenes.Marielle.HandholdingStop.PoppingJointBack".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.HandholdingStop.JustHoldHands".Localized());

                OutputText("Scenes.Marielle.HandholdingStop.VeryWellSays".Localized());
            }

            OutputText("Scenes.Marielle.HandholdingStop.RatherCold".Localized());

            OutputText("Scenes.Marielle.HandholdingStop.SaysNothingCircle".Localized());

            if (Save.GetInt("handHeld", 0) % 2 == 0)
            {
                OutputText("Scenes.Marielle.HandholdingStop.GrazeFingernails".Localized());

                OutputText("Scenes.Marielle.HandholdingStop.TakesHerTime".Localized((Save.GetInt("timesSexed", 0) != 0 && Save.GetInt("timesSexed", 0) == Save.GetInt("timesYurid", 0) ? "She's holding a creeping blush at bay that arrived with those last words. You can imagine where her mind went, but she does by and by overcome it and softly clears her throat." : "She lets those last words hang in the room for a while.")));

                OutputText("Scenes.Marielle.HandholdingStop.BundlesFourHands".Localized());

                SexReturn("[pg][say:Well then... Be there aught else you wished, or may I to bed?][pg]You know it's already late, so you don't keep the girl from her sleep.[pg][say:Thank you. A pleasant night upon you, [name],] she says with a dipped curtsy.[pg]The night, at least immediately outside in the bog, isn't as pleasant as she wished--some unidentifiable smell is making its rounds through the trees. It's at least peaceful however, and your trek back remains without disturbance.",

                    "[pg][say:Hmm... May I[if (femininity >= 59) {...}] do aught else for you?][pg]Actually, you think you should be checking on your camp; it's been some View.Time.[pg][say:Ah. Of course,] she says, getting up for a dipped curtsy. [say:Adieu then, [name]. Pray you be safe.][pg]Safe you are--nothing disturbs your trek back through the forested marshlands, and whatever creatures are calling out from the bushes, trees, and the far distance elect to avoid you entirely.",

                    "[pg][say:Well... what else may I do for you, [name]?]",

                    () => LewdMenu());
            }
            else
                if (Player.HasCock() || !Player.HasVagina() || Save.GetInt("insulted", 0) != 0)
                {
                    OutputText("Scenes.Marielle.HandholdingStop.WillingToComply".Localized());

                    OutputText("Scenes.Marielle.HandholdingStop.LetMomentsPass".Localized());

                    SexReturn("[pg]It's also a good time to call it a night here. The seamstress looks up when you voice your thought, nudging her glasses.[pg][say:Quite so, [if (insultedfather) {yes|aye}]... Well, uhm,] she says as she gets up to dip a curtsy, [say:I bid you a good night, [name].][pg]You return the farewell and take off towards your own camp. Fog and moonlight shroud the bog in a damp layer of mystery--one that you're careful not to let become your too-literal downfall. A nighttime bath in these stagnant, muddy waters would be an unwelcome one.",

                    "[pg]It's also a good time to be checking back on your camp. The seamstress looks up when you voice your thought, nudging her glasses and giving you a nod.[pg][say:As you will,] she says, getting up to dip you a small curtsy. [say:I bid you safe travels then, [name].][pg]Safe you are--nothing disturbs your trek back through the forested marshlands, and the creatures that call these damp, muddy reaches their home remain but mere observers and background performers to your passage.",

                    "[pg][say:Hmm.] She stares at them, slowly pianoing her fingers through the air before letting them fall to her lap and saying, [say:Was there aught else you wished?]",

                    () => LewdMenu());
                }
                else
                {
                    OutputText("Scenes.Marielle.HandholdingStop.EachMomentWarms".Localized());

                    OutputText("Scenes.Marielle.HandholdingStop.PreserveQuietude".Localized());

                    SexReturn("[pg]It's not long after the blush has vanished that her eyes fall closed and her exhales trickle out soft and even, her entire posture relaxing with them. Before you can wonder if Marielle has actually nodded off though, she squeezes you again, cracks open her eyelids, and takes an audible breath.[pg][say:Uhm, I... I would 'twere not so, [name], howbeit,] she begins, keeping her gaze down on your joined hands, [say:heaviness has quite inclipped me... May I to sleep? 'Tis beholden to by and by beguide me thither--bedwards.][pg]You should wrap this up, then. As you let the girl's hands glide out of your grasp and back onto her lap, savoring the last bits of taction, you wish her a good night, which she receives with a smile and something between a nod and a sitting bow.[pg][say:Likewise, a most wholesome night upon you. And gracious journeys.][pg]Outside, it's quiet, more so than usual; the songs of birds, the rustling winds--all has faded into a grand, collective slumber. Only a handful of mosquitoes are paying no heed to the moon-tinged serenity, but they prove hardly a hassle, used to them as you are by now.",

                    "[pg]That continues on until you eventually find the right time to wrap your whole visit up and allow yourself to drift away, gliding along the dainty length to the very tips of her fingers and finally letting go. The seamstress shows no immediate reaction, so you make use of your voice to draw her out of whatever fantasy she's found her mind in, telling her you'll be going. She starts up, blinks, her mouth opening. It hangs there for another blink before she closes it with a quick hum and a nod.[pg][say:Of course, yes,] Marielle says, sounding a little befuddled. Organizing herself, she looks around as if to search for something before she gets up and dips you a parting curtsy.[pg][say:Prithee, have a most wholesome [if (hour >= 18) {eve|[if (hour < 12) {end of morrow|day}]}], [name]. And withal a safe journey.][pg]Seeing you off with a candid smile, the seamstress props herself against her desk with one hand, the other three massaging each other. The last you see of her as you exit the marsh-embowered temple is how she plops back down, tips her head, and sweeps all twenty of her fingers through her hair at once. Accompanied by a sigh, you would guess.",

                    "[pg]That continues on until you eventually find the right time to come to a close and allow yourself to drift away, gliding along the dainty length to the very tips of her fingers and finally letting go. They hover there for a moment longer before she retracts them with a [say:Hmm] and reunites the pair to her other one on her lap.[pg][say:Well...] The girl's lips casting a pleased smile, she looks up into your eyes. [say:Was't all you-- I mean... was there aught else I may do for you?]",

                    () => LewdMenu());
                }

            if (Save.GetInt("handHeld", 0) % 2 == 0)
                Save.SetInt("handHeld", Save.GetInt("handHeld", 0) + 1);
        }

        public void HandholdingLet(bool firstTime)
        {
            ClearOutput();

            View.Time.PassMinutes((int)Round((1 / 2) * 60));

            if (firstTime)
            {
                OutputText("Scenes.Marielle.HandholdingLet.DontStopHer".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.PrayBeMindful".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.HandholdingLet.WantedLookAtHand".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.DubiousGlance".Localized());
            }

            OutputText("Scenes.Marielle.HandholdingLet.StrangeSeveredHand".Localized());

            if (Save.GetInt("handHeld", 0) < 2)
            {
                OutputText("Scenes.Marielle.HandholdingLet.ShufflingOnSeat".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.ButchersShopCleave".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.LackOfTerminology".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.IndeedConfirm".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.TisUnperfect".Localized());

                SexReturn("A yawn overtakes the seamstress as she finishes that sentence, and she moves to cover it up. With the wrong palm--the one you're still holding, the one whose wrist is nothing more than a stump. It takes her a blink to notice the error and hastily correct it.[pg][say:Ah, uhm... Pardon me,] she says when she's collected herself. [say:Sleep becks me, it appears. May I?] That last bit was in reference to her hand, which you do give her back. While she works it into its rightful place, you take your leave from the undead girl, prompting her to drop into a swift farewell-curtsy at your departure.[pg][say:Aye, pray have a wholesome night, [name].][pg]As you're [walking] out into the swamp, your sight falls onto a bird wading not far from you in the middle of a slow-running stream. In itself nothing unusual, but standing on comically long, spindly legs, the grey-on-grey animal has its eyes fixated on something below the surface.[pg]Could be a " + (silly || Player.Intelligence < 1 ? "bird, or maybe a bird" : "crane or heron") + ", perhaps, though it's pretty large, the feathers it bears look almost like dull scales in the moonlight, and its rather bulky, plumed head ends in a bill that should be too heavy for an avian. You're surprised to see it suddenly dart the whole thing into the water to yank out a lizard flailing, yawping, and biting about wildly before it disappears into the creature's gullet. Was that... a young caiman? Is the night playing tricks on you, or did that bird just eat a crocodile? And it's looking at you, now.[pg][if (silly) {You quickly bow in deference to the fearless predator, a gesture it mirrors|But likely judging you no threat, it gives your presence nothing more than a glance}] before throwing its head back and clattering its odd beak into the air, the sound reverberating through the old, dark trees. You decide to just let it be and get back to camp.",

                    "Wrapping up her explanation, she outstretches one of her remaining hands. [say:Now, may I have that back? Unless you were not yet finished with your perusal.] You think you were.[pg]As Marielle pops her joints back into place again, you watch with interest how the flesh and skin magically fuse together, leaving her wrist, while not seamless--there quite literally is a seam--just as it was before. Testingly, she flexes her fingers, apparently finding them in working order before bundling all twenty of them together and massaging them with a hearty squeeze, her eyes briefly closing.[pg][say:Hmm... [name]?] she asks with more air than voice. [say:Pray pardon me, but I ween a, ah, rest be needful ere I--or, well, [i:we]--do aught else. May I presently bid you farewell?] You have no objection to that, so you say your goodbye and get ready to leave. The seamstress dips a bow in acknowledgement.[pg][say:Fare you well then, [name]. Safe travels.][pg]By the time your campsite[if (builtwall) {'s surrounding wall}] comes into view, you've probably accumulated a half dozen or so insect bites more, but at least nothing else has tried to ambush you.",

                    "Wrapping up her explanation, she outstretches one of her remaining hands towards you. [say:Now, may I have that back? Unless you were not yet finished with your perusal.] You think you were.[pg]As Marielle pops her hand back into place again, you watch with interest how the flesh and skin magically fuse together once the pieces are connected, leaving her wrist, while not seamless--there quite literally is a seam--at least as it was before. Testingly, she flexes her fingers, apparently finding them in working order before returning to you.[pg][say:Was't all you wished, or[if (femininity >= 59) {...}] be there aught else?]",

                    () => LewdMenu());
            }
            else
            {
                OutputText("Scenes.Marielle.HandholdingLet.NothingNewRevealed".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.DoesThatPleaseYou".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.SayISee".Localized());

                OutputText("Scenes.Marielle.HandholdingLet.MoreShuffling".Localized());

                SexReturn("[pg]Although before long, a yawn overtakes her. She moves to cover it up, with the wrong palm--the one you're still holding, whose wrist is nothing more than a stump. It takes a blink for her to notice the error and hastily correct it.[pg][say:Ah, uhm... Pardon me,] she says when she's collected herself. [say:Sleep becks me, it appears. May I, now?] That last bit was in regard to her hand, which you do give her back. While she works it into its rightful place, you take your leave from the undead girl, prompting her to drop into a swift farewell-curtsy at your departure.[pg][say:Aye, pray have a wholesome night, [name].][pg]You're not far out into the swamp before you get the strange sinking feeling that something must be following you, watching your passage, though a glance over your shoulder leaves you in the figurative--and literal--dark. [if (" + (0 != 0) + ") {It couldn't be Sylvia, could it? [if (" + (0 < 0) + ") {You told her off, after all, and|You look around for the striking white and violet of her appearance, but can't spot a single speck of it. Well,}] whether it's her, some other creature pursuing you from the shadows,|Whether it's some creature lurking in the shadows}] or just your imagination, it does ultimately leave you alone after you clear the last remnants of civilization that mark the way towards the decrepit temple.",

                    "[pg]You spend a little time longer warming up her cold fingers and being occasionally glanced at by their undead owner before you decide give the palm a final rub-down and relinquish it to her.[pg]As Marielle pops her joints back into place again, you watch with interest how the flesh and skin magically fuse together, leaving her wrist, while not seamless--there quite literally is a seam--just as it was before. Testingly, she flexes her fingers, apparently finding them in working order before bundling all twenty of them together and massaging them with a hearty squeeze, her eyes briefly closing.[pg][say:Hmm... [name]?] she asks with more air than voice. [say:Pray pardon me, but I ween a, ah, rest be needful ere I--or, well, [i:we]--do aught else. May I presently bid you farewell?] You have no objection to that, so you say your goodbye and get ready to leave. The seamstress dips a bow in acknowledgement.[pg][say:Fare you well then, [name]. Safest travels.][pg]The bog around the overgrown temple is as peaceful as ever, swarms of insects harassing you notwithstanding. It's relatively quiet, too; only from somewhere far-off echoes a series of odd clattering noises that carries deep into the old trees.",

                    "[pg]You spend a little time longer warming up her cold fingers and being occasionally glanced at by their undead owner until you decide give the palm a final rub-down and relinquish it to her.[pg]As Marielle pops her hand back into place again, you watch with interest how the flesh and skin magically fuse together once the pieces are connected, leaving her wrist, while not seamless--there quite literally is a seam--at least as it was before. Testingly, she flexes her fingers, apparently finding them in working order before returning to you.[pg][say:Is't all you wished, or[if (femininity >= 59) {...}] be there aught else?]",

                    () => LewdMenu());
            }

            if (Save.GetInt("handHeld", 0) < 2)
                Save.SetInt("handHeld", Save.GetInt("handHeld", 0) + 2);
        }

        public void TaurRide()
        {
            ClearOutput();

            RegisterMacro("ridefetish", Save.GetInt("taurDecision", 0) == 2);

            ClearButtons();

            if (Save.GetInt("taurDecision", 0) == 0)
            {
                OutputText("Scenes.Marielle.TaurRide.NotPulledByHorse".Localized());

                OutputText("Scenes.Marielle.TaurRide.WhatBringsThis".Localized());

                OutputText("Scenes.Marielle.TaurRide.SilentHeadTilt".Localized());

                OutputText("Scenes.Marielle.TaurRide.PardonMeWavesOff".Localized());

                OutputText("Scenes.Marielle.TaurRide.ImagePaintsWell".Localized());

                AddNextButton("Common.Button.ItsFine".Localized(), TaurRideNormal);

                AddNextButton("Common.Button.Fetish".Localized(), TaurRideFetish);

                AddNextButton("Common.Button.Retract".Localized(), TaurRideRetract);
            }
            else
            {
                OutputText("Scenes.Marielle.TaurRide.AsksAnotherRide".Localized());

                OutputText("Scenes.Marielle.TaurRide.GladlyAtOnce".Localized());

                OutputText("Scenes.Marielle.TaurRide.DanglingLegsSide".Localized());

                OutputText("Scenes.Marielle.TaurRide.WhitherBetake".Localized(((Save.GetInt("timesYurid", 0) != 0 || Save.GetInt("taurDecision", 0) == 2) && Save.GetInt("timesDicked", 0) == 0 && Save.GetInt("insulted", 0) == 0 && !Save.GetBool("mockedDress", false) && Player.Femininity >= 59 && Player.IsFemale() ? "palfrey mine, " : "")));

                AddNextButton("Common.Button.StayIn".Localized(), () => TaurRideInside());

                AddNextButton("Common.Button.GoOut".Localized(), () => TaurRideOutside());

                AddNextButton("Common.Button.SheDecides".Localized(), TaurRideDecide);
            }
        }

        public void TaurRideRetract()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurRideRetract.SoundsDegrading".Localized());

            OutputText("Scenes.Marielle.TaurRideRetract.SeemsDisappointed".Localized());

            OutputText("Scenes.Marielle.TaurRideRetract.AughtElseForYou".Localized());

            Save.SetInt("taurDecision", 3);

            LewdMenu();
        }

        public void TaurRideNormal()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurRideNormal.DontMindCarrying".Localized());

            OutputText("Scenes.Marielle.TaurRideNormal.HandLiftsBriefly".Localized());

            OutputText("Scenes.Marielle.TaurRideNormal.JustHoldOn".Localized());

            OutputText("Scenes.Marielle.TaurRideNormal.OccurredToHer".Localized((Player.Femininity >= 59 && !Player.HasCock() ? "though that doesn't help her sudden onset of a blush. It takes a while for her to dispel it from her face and clear her throat before she tentatively steps" : "and she regards you for a moment before taking a tentative step")));

            OutputText("Scenes.Marielle.TaurRideNormal.WonderfulGrabsHold".Localized());

            OutputText("Scenes.Marielle.TaurRideNormal.TestingPatWithers".Localized());

            Save.SetInt("taurDecision", 1);

            DoNext(() => TaurRide2());
        }

        public void TaurRideFetish()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurRideFetish.IntoBeingTreated".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.TaurRideFetish.HandStopsEnthusiasm".Localized());

                OutputText("Scenes.Marielle.TaurRideFetish.WhereforeAskMe".Localized());

                OutputText("Scenes.Marielle.TaurRideFetish.PatBestialBack".Localized());

                OutputText("Scenes.Marielle.TaurRideFetish.ScootsUpLittle".Localized());

                OutputText("Scenes.Marielle.TaurRideFetish.WellShallWe".Localized());

                OutputText("Scenes.Marielle.TaurRideFetish.NoCommandsComing".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Marielle.TaurRideFetish.FaceRunsBeetRed".Localized());

                    OutputText("Scenes.Marielle.TaurRideFetish.FascinatingPassions".Localized());

                    OutputText("Scenes.Marielle.TaurRideFetish.DoWithCertainty".Localized());

                    OutputText("Scenes.Marielle.TaurRideFetish.SwingHerselfUp".Localized());

                    OutputText("Scenes.Marielle.TaurRideFetish.QuestionNotCommand".Localized());
                }
                else
                {
                    OutputText("Scenes.Marielle.TaurRideFetish.HandStopsUncertain".Localized());

                    OutputText("Scenes.Marielle.TaurRideFetish.PrayMakeNoHaste".Localized());
                }

            Save.SetInt("taurDecision", 2);

            DoNext(() => TaurRide2());
        }

        public void TaurRide2()
        {
            ClearOutput();

            RegisterMacro("ridefetish", Save.GetInt("taurDecision", 0) == 2);

            OutputText("Scenes.Marielle.TaurRide2.SoundsOfTrotting".Localized((silly || Player.Intelligence < 1 ? "kinda retarded anyway" : "mostly used to that by now")));

            OutputText("Scenes.Marielle.TaurRide2.AbsorbedBySights".Localized());

            OutputText("Scenes.Marielle.TaurRide2.MomentPleaseStop".Localized());

            OutputText("Scenes.Marielle.TaurRide2.AlteredPerspective".Localized());

            OutputText("Scenes.Marielle.TaurRide2.TouredOnceThrough".Localized());

            OutputText("Scenes.Marielle.TaurRide2.PeersThroughDoorway".Localized());

            OutputText("Scenes.Marielle.TaurRide2.BogQuietTonight".Localized((silly || Player.Intelligence < 1 ? "kinda retarded anyway" : "mostly used to that by now")));

            OutputText("Scenes.Marielle.TaurRide2.SureBoundToWorst".Localized());

            OutputText("Scenes.Marielle.TaurRide2.MightyTreeSeen".Localized());

            OutputText("Scenes.Marielle.TaurRide2.SmoothStoneSignal".Localized());

            OutputText("Scenes.Marielle.TaurRide2.LeftInSilence".Localized());

            OutputText("Scenes.Marielle.TaurRide2.NotAThing".Localized());

            OutputText("Scenes.Marielle.TaurRide2.QuietRestOfTour".Localized());

            OutputText("Scenes.Marielle.TaurRide2.SlowToHalt".Localized());

            OutputText("Scenes.Marielle.TaurRide2.StandsThereMoment".Localized());

            OutputText("Scenes.Marielle.TaurRide2.FishesForWords".Localized());

            OutputText("Scenes.Marielle.TaurRide2.WonderfulFarewell".Localized());

            OutputText("Scenes.Marielle.TaurRide2.GlanceBackLeaving".Localized());

            DoNext(() => ReturnToCamp(2));
        }

        public void TaurRideDecide()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurRideDecide.ShellDoAnything".Localized());

            OutputText("Scenes.Marielle.TaurRideDecide.FingertipsDrumSide".Localized());

            if (RandomUtils.Chance(Player.IsFemale() ? 70 : 50))
            {
                OutputText("Scenes.Marielle.TaurRideDecide.LetUsVentureOut".Localized());

                OutputText("Scenes.Marielle.TaurRideDecide.SoundsGoodToYou".Localized());

                TaurRideOutside(false);
            }
            else
            {
                OutputText("Scenes.Marielle.TaurRideDecide.RemainWithin".Localized());

                OutputText("Scenes.Marielle.TaurRideDecide.AsSheCommands".Localized());

                TaurRideInside(false);
            }
        }

        public void TaurRideInside(bool decision = true)
        {
            ClearOutput();

            if (decision)
            {
                OutputText("Scenes.Marielle.TaurRideInside.LikesItInside".Localized());

                OutputText("Scenes.Marielle.TaurRideInside.BeautifulFane".Localized());

                OutputText("Scenes.Marielle.TaurRideInside.PatGetsGoing".Localized());
            }

            OutputText("Scenes.Marielle.TaurRideInside.SignalsToStop".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.TrulyNoBurthen".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.SeamstressTakesNote".Localized((silly || Player.Intelligence < 1 ? "feathered scrotums" : "birds")));

            OutputText("Scenes.Marielle.TaurRideInside.CompletedWholeRound".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.MindSidetracked".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.ThanksAsLetDown".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.BackToCamp".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.AdieuThen".Localized());

            OutputText("Scenes.Marielle.TaurRideInside.WatchesDepart".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TaurRideOutside(bool decision = true)
        {
            ClearOutput();

            if (decision)
            {
                OutputText("Scenes.Marielle.TaurRideOutside.WalkAroundTemple".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside.FairWeatherVenture".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside.AbsolutelyFine".Localized());
            }

            OutputText("Scenes.Marielle.TaurRideOutside.SeamstressOnBack".Localized((silly || Player.Intelligence < 1 ? "who-whos. You shout your name out loud, but they don't stop. What a dumb animal" : "calls")));

            OutputText("Scenes.Marielle.TaurRideOutside.LeisurelyStretch".Localized());

            if (Player.IsFemale() && Save.GetInt("insulted", 0) == 0)
            {
                OutputText("Scenes.Marielle.TaurRideOutside.RestYonder".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside.SetFootPlatform".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside.AlasNo".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Relax".Localized(), () => TaurRideOutside2(false));

                AddNextButton("Common.Button.Hug".Localized(), () => TaurRideOutside2(true));
            }
            else
            {
                OutputText("Scenes.Marielle.TaurRideOutside.MarvellousQuietly".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside.AsPaintingTruly".Localized((silly || Player.Intelligence < 1 ? "who-whos. You shout your name out loud, but they don't stop. What a dumb animal" : "calls")));

                TaurRideOutside3();
            }
        }

        public void TaurRideOutside2(bool hugged)
        {
            ClearOutput();

            if (hugged)
            {
                OutputText("Scenes.Marielle.TaurRideOutside2.ArmAroundHer".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside2.PineTreeFragrance".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside2.WhatWasThat".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside2.YouAreWarm".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside2.FourArmsHug".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.TaurRideOutside2.NoWordsShared".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside2.DidntStayLong".Localized());

                OutputText("Scenes.Marielle.TaurRideOutside2.ComfortableCamelback".Localized());
            }

            OutputText("Scenes.Marielle.TaurRideOutside2.ShallWeMoveOn".Localized((Player.Sensitivity + Player.Toughness + Player.Strength < 180 ? " manage to avoid an unwelcome bath in the sludge" : "'ve got it all well under control")));

            TaurRideOutside3();
        }

        public void TaurRideOutside3()
        {

            OutputText("Scenes.Marielle.TaurRideOutside3.RideLessEventful".Localized());

            OutputText("Scenes.Marielle.TaurRideOutside3.MuchObliged".Localized());

            OutputText("Scenes.Marielle.TaurRideOutside3.DoesntGetUp".Localized((Player.Femininity >= 59 && Save.GetInt("insulted", 0) == 0 ? "lovely" : "pleasant")));

            OutputText("Scenes.Marielle.TaurRideOutside3.WatchedExiting".Localized());

            if (Player.IsFemale()) ReturnToCamp(1); else ReturnToCamp(1);
        }

        public void Bathe()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Bathe.PerhapsSheJoins".Localized());

            OutputText("Scenes.Marielle.Bathe.EngrossedUnaware".Localized());

            OutputText("Scenes.Marielle.Bathe.ElaborateDress".Localized());

            OutputText("Scenes.Marielle.Bathe.JumpsUpBlinks".Localized());

            OutputText("Scenes.Marielle.Bathe.InviteToBathe".Localized());

            if (Save.GetBool("bathed"))
            {
                OutputText("Scenes.Marielle.Bathe.HumsSpaceOut".Localized());

                OutputText("Scenes.Marielle.Bathe.AlreadyInWater".Localized((Player.Femininity > 50 && !Player.HasCock() ? "peeking sneakily at the curves of your naked body" : "a relaxed smile on her face")));
            }
            else
            {
                OutputText("Scenes.Marielle.Bathe.BathTogether".Localized());

                OutputText("Scenes.Marielle.Bathe.NotAverse".Localized());

                OutputText("Scenes.Marielle.Bathe.PuttingGearDown".Localized());

                OutputText("Scenes.Marielle.Bathe.EnterBathOpposite".Localized((Player.Femininity > 50 && !Player.HasCock() ? "noticing Marielle's peeking at your curves as they sink down into the heated, tranquil water. She quickly averts her gaze when she realizes you caught her, and a distinct blush she fails to hide promptly colors her face while she mutters something like an apology. Perhaps you were wrong about her not being embarrassed" : "letting yourself slowly sink into the heated, tranquil water")));

                Save.SetBool("bathed", true);
            }

            OutputText("Scenes.Marielle.Bathe.SighContentment".Localized());

            OutputText("Scenes.Marielle.Bathe.ComfortableSilence".Localized());

            Save.SetBool("seenNaked", true);

            bathOptions = [];

            BathMenu();
        }

        public void BathMenu()
        {

            ClearButtons();

            AddNextButton("Common.Button.JustBathe".Localized(), JustBathe);

            AddNextButton("Common.Button.WarmWater".Localized(), WarmWater);

            AddNextButton("Common.Button.ZombieBath".Localized(), ZombieBath);

            AddNextButton("Common.Button.Ogle".Localized(), Ogle);

            if (Player.IsTaur())
                AddNextButton("Common.Button.TaurWash".Localized(), TaurWash);
            else
                AddNextButton("Common.Button.MutualWash".Localized(), MutualWash);

            AddNextButton("Common.Button.FuckHer".Localized(), BathFuck);

            foreach (var i in bathOptions)
            {
                DisableButton(i, "You've already done that.");

                AddButton(0, "Common.Button.Finish".Localized(), FinishBath);
            }
        }

        public void JustBathe()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.JustBathe.FineWithThat".Localized());

            OutputText("Scenes.Marielle.JustBathe.LazilyScrubbing".Localized());

            if (Player.Fatigue >= 25)
            {
                OutputText("Scenes.Marielle.JustBathe.EyelidsGrowHeavy".Localized());

                OutputText("Scenes.Marielle.JustBathe.SayEllipsis".Localized());

                OutputText("Scenes.Marielle.JustBathe.CarriedOnWaves".Localized());

                OutputText("Scenes.Marielle.JustBathe.EllipsisFragment".Localized());

                OutputText("Scenes.Marielle.JustBathe.CryOverheadBird".Localized((Player.Intelligence < 50 && silly ? "penguin's" : "[if (intelligence < 70) {seagull's|shearwater's}]")));

                OutputText("Scenes.Marielle.JustBathe.EllipsisName".Localized());

                OutputText("Scenes.Marielle.JustBathe.KnowsYourName".Localized());

                OutputText("Scenes.Marielle.JustBathe.NameQuestion".Localized());

                OutputText("Scenes.Marielle.JustBathe.NoBirdJustHer".Localized((Player.Femininity > 50 && !Player.HasCock() ? "startles" : "eases"), (Player.Femininity > 50 && !Player.HasCock() ? "self-consciousness suddenly coloring her cheeks" : "her worry slowly dissipating")));

                OutputText("Scenes.Marielle.JustBathe.FellAsleep".Localized(
                    Player.Femininity >= 59 && !Player.HasCock() ? "almost to regret" : (Save.GetInt("insulted", 0) != 0 ? "ambivalent about" : "sorry for")
                ));
            }
            else
            {
                OutputText("Scenes.Marielle.JustBathe.PoolSerenity".Localized());

                OutputText("Scenes.Marielle.JustBathe.NotConversational".Localized());

                OutputText("Scenes.Marielle.JustBathe.SearchingForDrive".Localized());
            }

            OutputText("Scenes.Marielle.JustBathe.LeavingIAssume".Localized());

            OutputText("Scenes.Marielle.JustBathe.FloorQuiteFine".Localized());

            OutputText("Scenes.Marielle.JustBathe.ReciprocateFarewell".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.5f));

            Player.ChangeFatigue(-40);

            Player.HasGottenWashed();

            DoNext(() => ReturnToCamp(1));
        }

        public void WarmWater()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.WarmWater.SheLivesHere".Localized());

            OutputText("Scenes.Marielle.WarmWater.LooksUpMoss".Localized());

            OutputText("Scenes.Marielle.WarmWater.PlashesBesidesPond".Localized());

            OutputText("Scenes.Marielle.WarmWater.NeverErenowThermae".Localized());

            OutputText("Scenes.Marielle.WarmWater.CupsWaterHand".Localized());

            OutputText("Scenes.Marielle.WarmWater.SplashesSmallWave".Localized());

            OutputText("Scenes.Marielle.WarmWater.PreservingMystery".Localized());

            OutputText("Scenes.Marielle.WarmWater.ThereBeSome".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Closer".Localized(), () => WarmWater2(true));

            AddNextButton("Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), () => WarmWater2(false));
        }

        public void WarmWater2(bool closer)
        {
            ClearOutput();

            if (closer)
            {
                OutputText("Scenes.Marielle.WarmWater2.GetUpAndJoin".Localized());

                OutputText("Scenes.Marielle.WarmWater2.LoHere".Localized());

                OutputText("Scenes.Marielle.WarmWater2.PalmWarmStream".Localized());

                OutputText("Scenes.Marielle.WarmWater2.MinuteCleft".Localized((Player.Femininity > 50 && !Player.HasCock() ? "[say:I, ah...] The girl shuffles in place, apparently becoming aware of how close you're standing to her right now. [say:So it, ah... appears, yes.] She awkwardly glances to the side. [say:There are many more, ah, akin to it, howbeit they can be quite troublesome to... locate.] Turning away a little too sharply, she starts wading back" : "[say:Aye, so it indeed appears.] She takes a step sideways, feeling along the submerged rocks. [say:There are many more akin to it, howbeit they can be quite troublesome to, ah, locate.] Wading on like this, she makes her way")));

                OutputText("Scenes.Marielle.WarmWater2.ExplainsBitMystery".Localized());

                OutputText("Scenes.Marielle.WarmWater2.GazeMidDistance".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.WarmWater2.FineHereThanks".Localized());

                OutputText("Scenes.Marielle.WarmWater2.DriftsBackSeat".Localized((Player.Femininity > 50 && !Player.HasCock() ? "A touch of rosiness flies over her cheeks, but quickly disappears" : "Her absorption briefly wavers at your declination, a frown touching her face")));

                OutputText("Scenes.Marielle.WarmWater2.MinuteFissures".Localized());

                OutputText("Scenes.Marielle.WarmWater2.StillMysterious".Localized());

                OutputText("Scenes.Marielle.WarmWater2.GazesMidDistance".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.3f));

            Player.ChangeFatigue(-10);

            bathOptions.Add(1);

            BathMenu();
        }

        public void ZombieBath()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.ZombieBath.UndeadStillBathes".Localized());

            OutputText("Scenes.Marielle.ZombieBath.PulledFromDaydream".Localized());

            OutputText("Scenes.Marielle.ZombieBath.IndignantWavesShatter".Localized());

            OutputText("Scenes.Marielle.ZombieBath.AsksToElaborate".Localized());

            OutputText("Scenes.Marielle.ZombieBath.ExpressionTurnsSolemn".Localized());

            OutputText("Scenes.Marielle.ZombieBath.ArmSinksWater".Localized());

            OutputText("Scenes.Marielle.ZombieBath.ThoughtsDriftAway".Localized((Player.Femininity >= 59 ? "[say:A-Ah, thank you...] she says, sinking a little deeper into the water." : "[say:Hmm... Thank you,] she says, gazing at her hands.")));

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.3f));

            Player.ChangeFatigue(-10);

            bathOptions.Add(2);

            BathMenu();
        }

        public void Ogle()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Ogle.SurfaceDistortsImage".Localized());

            OutputText("Scenes.Marielle.Ogle.NoseLeadsToLips".Localized());

            OutputText("Scenes.Marielle.Ogle.LeerScarredArms".Localized());

            OutputText("Scenes.Marielle.Ogle.RisingFallingSurface".Localized());

            OutputText("Scenes.Marielle.Ogle.NotJustWater".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            Player.ChangeFatigue(-5);

            bathOptions.Add(3);

            BathMenu();
        }

        public void FinishBath()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.FinishBath.TimeToFinish".Localized());

            OutputText("Scenes.Marielle.FinishBath.RousedByExit".Localized((Player.Femininity > 50 && !Player.HasCock() ? "following its movements over your [skinfurscales]" : "wandering away again")));

            OutputText("Scenes.Marielle.FinishBath.HumsInDueTime".Localized());

            OutputText("Scenes.Marielle.FinishBath.PleasantFarewell".Localized());

            OutputText("Scenes.Marielle.FinishBath.FarewellGlance".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.4f));

            Player.ChangeFatigue(-20);

            Player.HasGottenWashed();

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWash()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWash.SomeoneElseHere".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.MutualWash.PullsFromWoolGathering".Localized());

                OutputText("Scenes.Marielle.MutualWash.ShiftsUncomfortably".Localized());

                OutputText("Scenes.Marielle.MutualWash.AwkwardFailure".Localized());

                bathOptions.Add(4);

                BathMenu();
            }
            else
            {
                OutputText("Scenes.Marielle.MutualWash.BringsAttentionBack".Localized());

                OutputText("Scenes.Marielle.MutualWash.EyesDriftAside".Localized());

                OutputText("Scenes.Marielle.MutualWash.SoundsHesitant".Localized());

                OutputText("Scenes.Marielle.MutualWash.RiseExposesChest".Localized((Player.Femininity >= 59 || Player.BiggestBreastSize() > 1 ? "and leaves nothing to her imagination, what little poise the pale girl still possessed evaporates. [if (isflat) {Hastily, she turns|Blatantly, she stares at your chest before jerking}]" : "the pale girl is quick to turn")));

                OutputText("Scenes.Marielle.MutualWash.MumblesSoap".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Wait".Localized(), MutualWashWait);

                AddNextButton("Common.Button.DiveIn".Localized(), MutualWashDiveIn);
            }
        }

        public void MutualWashWait()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashWait.FindsWhatSought".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.VanishesQuickly".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.StartsOnShoulder".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.LatherKneadToo".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.HoldsLowerPairOut".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.QuestioningRedirects".Localized(
                Player.Femininity > 50 || Player.HasVagina() ? "is [if (femininity >= 59) {properly|decently}] flushed now, [if (Tallness > 73) {a faceful of|the direct, skin-to-skin contact with}] your [chest] evidently " + (Player.Femininity >= 59 && Player.BiggestBreastSize() > 0 ? "doing terrible things to" : "eroding") + " her composure, but she appears [if (femininity > 45) {anything but in opposition|not averse}] to that. Still," : "doesn't appear too sure what to make of this situation and seems to have something to say, so"
            ));

            OutputText("Scenes.Marielle.MutualWashWait.ShouldRequiteYou".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.SudsDrippingPalms".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.HandsSoftGentle".Localized((Player.Femininity > 50 || Player.HasVagina() ? "red-faced" : "vacillating")));

            OutputText("Scenes.Marielle.MutualWashWait.ReclaimScentedSoap".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.SpinMarielleAround".Localized());

            OutputText("Scenes.Marielle.MutualWashWait.BreathHitches".Localized((Player.Femininity > 50 || Player.HasVagina() ? "alone leaves no doubt about what kind of effect this is having" : "tells you that this prolonged treatment, even by someone like you, is having at least some sort of effect")));

            ClearButtons();

            AddNextButton("Common.Button.JustWash".Localized(), MutualWashJust);

            AddNextButton("Common.Button.GetFrisky".Localized(), MutualWashFrisky);
        }

        public void MutualWashJust()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashJust.CantCapitalize".Localized());

            OutputText("Scenes.Marielle.MutualWashJust.PassesSoapBack".Localized((Player.Femininity > 50 || Player.HasVagina() ? "entranced by your still-untouched chest.[if (isflat && isfeminine) { You sporting nothing in the size department either seems to do little to dispel her heat.}] You catch her taking a long, steadying breath" : "staring at your still-untouched chest. There's a somewhat contemplative quality in that look of hers")));

            OutputText("Scenes.Marielle.MutualWashJust.NearlyShiver".Localized());

            OutputText("Scenes.Marielle.MutualWashJust.NoticedMovements".Localized((Player.Femininity > 50 || Player.HasVagina() ? "more" : "of")));

            OutputText("Scenes.Marielle.MutualWashJust.FidgetsApology".Localized());

            OutputText("Scenes.Marielle.MutualWashJust.ReinsHerselfIn".Localized());

            if (Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Marielle.MutualWashJust.LotOfThat".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.UnsureHowTackle".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.QuicklySoapsUp".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.AmbiguousGlance".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.StartledSqueak".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.PrettyCleanNow".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.EyesFollowAppendage".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.MutualWashJust.FingersInnerThigh".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.NextTouchTantalizing".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.KnotsUnwinding".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.BriefVoidCore".Localized());

                OutputText("Scenes.Marielle.MutualWashJust.EyesFollowShake".Localized());
            }

            OutputText("Scenes.Marielle.MutualWashJust.FavorUnreturned".Localized());

            OutputText("Scenes.Marielle.MutualWashJust.ShufflesNotNeedful".Localized((Player.Femininity > 50 || Player.HasVagina() ? "thighs shifting, she claws" : "hands rising, she grasps")));

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.5f), new DynStat(GameStats.Lust, 25f));

            Player.ChangeFatigue(-60);

            Player.HasGottenWashed();

            ClearButtons();

            AddNextButton("Common.Button.Leave".Localized(), MutualWashJustLeave);

            AddNextButton("Common.Button.WashLegs".Localized(), MutualWashJustLegs);
        }

        public void MutualWashJustLegs()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashJustLegs.WontDoSinkBack".Localized());

            OutputText("Scenes.Marielle.MutualWashJustLegs.TriesLeanAway".Localized((Player.Femininity > 50 || Player.HasVagina() ? "Y-You need not concern yourself with me, I-I shall be fine!" : "Oh, uhm, you need not concern yourself with me, I shall be quite fine."), (Player.Femininity > 50 || Player.HasVagina() ? "Hahn-- I mean, I can..." : "Uhm-- Truly, [name],")));

            OutputText("Scenes.Marielle.MutualWashJustLegs.LiftLegOut".Localized());

            OutputText("Scenes.Marielle.MutualWashJustLegs.PlaceholderPeriod".Localized((Player.Femininity > 50 || Player.HasVagina() ? "Her exhales are shallow, strained, erotic, and though they seem to serve the seamstress well enough in calming herself, they audibly quiver each time you catch a more sensitive spot. And her thighs and feet in particular seem to be teeming with those" : "Despite her verbal attempts to dissuade you, that opposition didn't last very long, and now she's quietly letting you do as you wish, albeit with a certain wariness rising to her eyes whenever you approach an area that seems to be a little more sensitive. Notably her thighs and feet")));

            OutputText("Scenes.Marielle.MutualWashJustLegs.SwitchOtherLeg".Localized());

            OutputText("Scenes.Marielle.MutualWashJustLegs.HandsDownLeg".Localized(
                Player.Femininity > 50 || Player.HasVagina() ? "Fingers dragging over stone in the water, another shiver underneath your massaging touch. " + (Player.Femininity >= 59 && Player.HasVagina() ? "If it weren't submerged just below the surface, you're sure her womanhood would make the girl's flushed state of mind even plainer than it already is" : "It might be about time to finish this up") + "." : "That one, too, do you give your special attention to, enjoying the little reactions you can get out of her until you deem it suitably clean and pristine.",

                Player.Femininity > 50 || Player.HasVagina() ? "blinking rapidly, torn between " + (Player.Femininity >= 59 && Player.HasVagina() ? "burning your nude form into her memory" : "watching your nude form") + " and avoiding you in shame" : "undecided about where to look"
            ));

            OutputText("Scenes.Marielle.MutualWashJustLegs.ThinkDoneHere".Localized((Player.Femininity > 50 || Player.HasVagina() ? "Deciding [if (corruption < 50) {on giving her the space she obviously needs|against torturing her a little more}]" : "Giving her the bit of space she seems to need")));

            OutputText("Scenes.Marielle.MutualWashJustLegs.SheBeginsThanks".Localized((Player.Femininity > 50 || Player.HasVagina() ? "you're not sure whether she's thanking you for the service or for stopping when you did, but you regardless" : "you take her soft- and short-spoken thanks as a positive and")));

            OutputText("Scenes.Marielle.MutualWashJustLegs.StaysSeatedSilent".Localized());

            OutputText("Scenes.Marielle.MutualWashJustLegs.FreshLikeNewborn".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            Player.ChangeFatigue(-10);

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWashJustLeave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashJustLeave.NotGoingPush".Localized((Player.Femininity >= 59 && Player.HasVagina() ? "Her eyes are definitely still on you as you dry yourself off--you can feel them glued to your roaming hands like a hunter's sights on a particularly well-fed deer. However, when you finish up and throw a glance at her out of the corner of your eye, the girl's simply sitting there, all prim and proper like she wasn't just ogling you. Although she does seem a bit too stiff, even for her standards" : "The girl does grant you some privacy by keeping her eyes elsewhere while you dry yourself off, and you have a few moments of simply appreciating the soft fluffiness rubbing over your body until you're about ready to depart")));

            OutputText("Scenes.Marielle.MutualWashJustLeave.TowelDownDress".Localized());

            OutputText("Scenes.Marielle.MutualWashJustLeave.CallsBeSafe".Localized());

            OutputText("Scenes.Marielle.MutualWashJustLeave.MosquitoesNoBother".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWashFrisky()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFrisky.ExactlyWhereWanted".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.HandOnHeavingChest".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.SweetMoanEnough".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.PantsHandHead".Localized());

            if (Player.Femininity >= 59)
            {
                OutputText("Scenes.Marielle.MutualWashFrisky.CryEndedGasp".Localized());

                OutputText("Scenes.Marielle.MutualWashFrisky.HandReleasesSlump".Localized());

                OutputText("Scenes.Marielle.MutualWashFrisky.ExhaleMumble".Localized());

                OutputText("Scenes.Marielle.MutualWashFrisky.AnswerSummons".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.MutualWashFrisky.SheatheLastKnuckle".Localized());

                OutputText("Scenes.Marielle.MutualWashFrisky.GentleRelaxedPace".Localized());

                OutputText("Scenes.Marielle.MutualWashFrisky.LeansInLips".Localized());
            }

            OutputText("Scenes.Marielle.MutualWashFrisky.TonguePleasantlyCool".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.LoseYourselfTouch".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.PurrsBreathyDesire".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.SurroundingsComeFocus".Localized());

            OutputText("Scenes.Marielle.MutualWashFrisky.SinkToBottom".Localized());

            ClearButtons();

            AddNextButton("Common.Button.LetHer".Localized(), MutualWashLet);

            AddNextButton("Common.Button.FingerButt".Localized(), MutualWashFingerButt);
        }

        public void MutualWashLet()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashLet.WantHerNeedHer".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.ForcedBackEdge".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.MusclesShudderBliss".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.ZestfulTonguesGrinding".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.SeparateFromLips".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.GirlStartsPanting".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.FidgetsAgain".Localized());

            OutputText("Scenes.Marielle.MutualWashLet.ProperAndPolite".Localized());

            MarielleSex();

            Player.Orgasm("Vaginal");

            DoNext(() => MutualWashLet2());
        }

        public void MutualWashLet2()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashLet2.AskIfContinue".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.StartlesSnakeHand".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.LooksTroubledSigh".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.TongueFlicksLips".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.TellHerAbsolutely".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.StraddlesQuadrupleHands".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.GottenMessage".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.GlimpsesSwallowsDives".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.NoBitingArduous".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.FlamesPassionRise".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.WhispersLeaningLuscious".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.SoftnessUnderHunger".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.MewlWhenBreaksAway".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.HandsLatchEverywhere".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.FingertipsItching".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.ForceOutMore".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.GiveOverSecondPeak".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.PalmsLockedTogether".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.DrunkLustReachUp".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.CalmInEmbrace".Localized());

            OutputText("Scenes.Marielle.MutualWashLet2.CoolPuffsTickling".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddNextButton("Common.Button.WakeUp".Localized(), MutualWashLet3);
        }

        public void MutualWashLet3()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashLet3.LightReturns".Localized());

            OutputText("Scenes.Marielle.MutualWashLet3.WhatWokeYou".Localized());

            OutputText("Scenes.Marielle.MutualWashLet3.SilentSleeper".Localized());

            OutputText("Scenes.Marielle.MutualWashLet3.StirsHmmnm".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Pat".Localized(), () => MutualWashLet4(0));

            AddNextButton("Common.Button.Kiss".Localized(), () => MutualWashLet4(1));

            AddNextButton("Common.Button.Grope".Localized(), () => MutualWashLet4(2));
        }

        public void MutualWashLet4(int choice)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashLet4.YouGiveHerA".Localized(
                new[] { "You give her a pat and ask her to get up before she has the chance to get too embarrassed again.",

            "She doesn't resist as you guide her downwards, and her eyes drift closed again when your lips touch in a tender good-morning kiss. It's brief, sweet, and almost chaste, but neither her hands nor yours can resist exploring the other's nude, intertwined body a little before you pull away and state that you should be getting up.",

            "Your hands glide down over her back to her buttocks, and she almost squawks when you grope them without mercy or warning and pull her hips in to rub her satisfied pussy against yours for a brief, lecherous moment before you let go, give her a light swat, and tell her to get up." }[choice]
            ));

            OutputText("Scenes.Marielle.MutualWashLet4.SoundsFeeble".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.LeadToBench".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.OfferDryOff".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.AdmitsDefeat".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.WipesHerBody".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.CanWalkAlone".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.SeatsOnNose".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.DayAdvancedFarewell".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.SwivelsCurtsy".Localized());

            OutputText("Scenes.Marielle.MutualWashLet4.LeavesToWork".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            Player.ChangeFatigue(-100);

            Player.HasGottenWashed();

            MarielleSex();

            if (choice == 1)
                Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void MutualWashFingerButt()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFingerButt.NotJustLetHer".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt.KneadPaleButtocks".Localized());

            if (Save.GetInt("timesInButt", 0) != 0)
            {
                OutputText("Scenes.Marielle.MutualWashFingerButt.GentlePrays".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt.DoesntStopYou".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.MutualWashFingerButt.DoesntDareMove".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt.GlancesAside".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt.PrayBeGentle".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt.AsMuchApproval".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt.SearchTurnsSoap".Localized());
            }

            MarielleSex(false, true);

            DoNext(() => MutualWashFingerButt2());
        }

        public void MutualWashFingerButt2()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFingerButt2.CoatedFingerPush".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.FingersStillLodged".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.MoanBreaksFree".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.MomentOfClarity".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.SweetSourTaste".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.WarmWaterChest".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.LipsSealedToHers".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.ReachTheStitches".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.ScarcelyNeedToAsk".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.GrantHerWish".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.FeelBonyDigits".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.CrushDelicateSeamstress".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.AudiblePopChoke".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.SlackensStranglehold".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.GravityOfExertion".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.SappedButGratified".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.MuchMessyState".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.RelaxingSoberMoments".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt2.WhispersThanSays".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.8f));

            MutualWashFingerButt3(false);

            AddNextButton("Common.Button.Whore".Localized(), () => MutualWashFingerButt3(true));
        }

        public void MutualWashFingerButt3(bool whore)
        {
            ClearOutput();

            if (whore)
            {
                OutputText("Scenes.Marielle.MutualWashFingerButt3.YouAskHer".Localized(
                    Player.HasPerk(PerkIds.HistoryWhore) ? "She's not exactly wrong about you being a whore, or at least having been one, but you're still a little surprised that the soft-spoken seamstress would outright call you that." : (Player.HasPerk(PerkIds.HistorySlut) ? "She's not too far off. While not taking money for it, you have been kind of a slut, but you're more surprised that the soft-spoken seamstress would outright call you that." : "Did you catch that right? Did this soft-spoken seamstress really just call you a whore?")
                ));

                OutputText("Scenes.Marielle.MutualWashFingerButt3.PullsAwayConfused".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt3.TurnsAsideStammering".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.MutualWashFingerButt3.SoundsLikeEnjoyed".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt3.TrailsOffUntangle".Localized());

                OutputText("Scenes.Marielle.MutualWashFingerButt3.StiffBreathDraws".Localized());
            }

            ClearButtons();

            AddNextButton("Scenes.Faerie.GladesScene.ButtonHelp".Localized(), () => MutualWashFingerButt4(0));

            AddNextButton("Common.Button.Apologize".Localized(), () => MutualWashFingerButt4(1));

            AddNextButton("Common.Button.LetHer".Localized(), () => MutualWashFingerButt4(2));
        }

        public void MutualWashFingerButt4(int choice)
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFingerButt4.YouRePrettySure".Localized(
                new[] { "You're pretty sure that was your fault, so offering Marielle some help would be the right thing, and that is what you do.[pg][say:Hmm?] She looks up at you. [say:Oh, no, no, fret not for me, 'tis not needful to...] She takes hold of her arm with two others, holds her breath, then gives it a hearty pull that ends in an unhealthy-sounding pop and a wince. The air escapes her lungs in a somewhat strained exhale. [say:Help.] Looks like she can move it just fine again.[pg][say:But natheless I, ah, do thank you for the offer, [name].] Her grey-blue eyes turn to you again, steeped in warm[if (!insultedfather) {, genuine}] contentment.",

            "That was your fault, wasn't it? You offer an apology to Marielle and steady her as she grabs onto her injured arm with two others.[pg][say:Ah, well, 'tis...] A loud pop has her wince again when she pulls, but her scrunched-up face soon relaxes afterwards. [say:'Tis naught, fret not for me. These can be a little... delicate at times,] she says, testingly rolling the shoulder in question, [say:yet mended in facility.]",

            "Her other hands are already getting to work on it, so it doesn't look like Marielle will need any help from you, and all you do is steady her as she grasps the injured arm. A loud pop and a wince later, she lets go of the breath she held.[pg][say:Hmm,] she says, rotating her shoulder testingly, [say:adequate.] Seems she can move it just fine." }[choice]
            ));

            OutputText("Scenes.Marielle.MutualWashFingerButt4.LookHerUpDown".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt4.InspectsDislocatedArm".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButt4.ClosingRub".Localized((Player.Strength >= 69 && Player.Tallness >= 69 ? "when she finally relaxes her legs, you promptly lift the light-weighted seamstress out of the water and onto the pond's edge, eliciting a little [say:Oh] from her before you follow suit." : "with the lightweight seamstress clinging to you, you wade into the shallower area of the pond, where she finally unwinds her legs and eases off you to allow you to pull yourself out of the water. Turning around, you help her out as well.")));

            OutputText("Scenes.Marielle.MutualWashFingerButt4.DismissiveWave".Localized());

            MarielleSex(false, true);

            ClearButtons();

            AddNextButton("Common.Button.Leave".Localized(), MutualWashFingerButtLeave);

            AddNextButton("Common.Button.TowelHer".Localized(), MutualWashFingerButtTowel);

            AddNextButton("Common.Button.LewdTowel".Localized(), MutualWashFingerButtLewd);
        }

        public void MutualWashFingerButtLeave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFingerButtLeave.IfSheSaysSo".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLeave.TowellingOffQuick".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLeave.BusyWithHair".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLeave.HandsStopLeaving".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLeave.LeaveWithSmile".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWashFingerButtTowel()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.LooksLikeNeedsHelp".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.TrailsOffRelenting".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.TakeTowelWorkBack".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.QuartetOfArms".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.ThatShouldBeAll".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.UnbespectacledEyesDistant".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.SanctumAirDries".Localized((Save.GetInt("timesKissed", 0) > 5 && Player.Femininity >= 59 && Save.GetInt("insulted", 0) == 0 ? " into Marielle's upper pair that reaches for your face. It hovers there, hesitance written on her features, before cradling your [ears] and pulling you close enough for its owner to capture your lips in a crisp, swift Kiss" : ". Marielle dithers there, probably not having thought this through, but then takes another step forwards and wraps you in a swift, sweet hug, pulling [if (tallness > 60) {herself against you|you into herself}] and giving you a proper squeeze")));

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.ThankYouForAll".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtTowel.EyesRemainOnHer".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWashFingerButtLewd()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.LooksLikeNeedsHelp".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.SupposeMostWise".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.FailHideGrin".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.OfCourseYes".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.HandOnStone".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.EnjoysPampering".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.WorkedOnceWorksAgain".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.ManyHandsUnsure".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.TellSecondTime".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.FinalChestSqueeze".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.FinallyBrushed".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.SightsSoundsHot".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.RoughForwardStroke".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.HandDrillingBehind".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.EmbosomMeBeg".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.ArmWrappedClose".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.WhimperingVoice".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.SoftExclamation".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.DoesntBodeWell".Localized());

            OutputText("Scenes.Marielle.MutualWashFingerButtLewd.DidntQuiteGo".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 22f));

            MarielleSex(false, true);

            if (Save.GetInt("fainted", 0) == 0)
                Save.SetInt("fainted", 1);

            ClearButtons();

            AddNextButton("Common.Button.LetRest".Localized(), () => MutualWashFingerButtLewd2(0));

            AddNextButton("Common.Button.Kiss".Localized(), () => MutualWashFingerButtLewd2(1));

            AddNextButton("Common.Button.Pinch".Localized(), () => MutualWashFingerButtLewd2(2));

            AddNextButton("Common.Button.Slap".Localized(), () => MutualWashFingerButtLewd2(3));
        }

        public void MutualWashFingerButtLewd2(int choice)
        {
            ClearOutput();

            DisableButton(choice, "You've already done that.");

            switch (choice)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.NothingYouCanDo".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.GatherManyHands".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.FoundOneFolded".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.DressReminder".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.NothingLeftToDo".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.LastGlanceSleeping".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.CalmAndCosy".Localized());

                        DoNext(() => ReturnToCamp(2));
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.FairyTaleKiss".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.BrushOverCheek".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.TryToKiss".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.OverlyRomanticShot".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.PinchWakesUp".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.GatherCheekSkin".Localized());
                    }

                    break;

                case 3:
                    {
                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.SlapBringsBack".Localized());

                        OutputText("Scenes.Marielle.MutualWashFingerButtLewd2.TiltHeadSlap".Localized());
                    }

                    break;
            }
        }

        public void MutualWashDiveIn()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashDiveIn.HyeepGrabLegs".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.WaterNotJustWet".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.HalfMoanLick".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.StifledMoanFound".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.CantSeeMuch".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.CoaxUtterances".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.DazedlyMumbles".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.LittleTimeCaress".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.ShuddersWhimpering".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.ComesQuietly".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.BentOverEdge".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.KneadedLifeBack".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.RapscallionRemark".Localized());

            OutputText("Scenes.Marielle.MutualWashDiveIn.VoiceLowersMumble".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            MarielleSex();

            ClearButtons();

            AddNextButton("Common.Button.GetLicked".Localized(), MutualWashLick);

            AddNextButton("Scenes.Lumberjack.Button.Decline".Localized(), MutualWashDecline);
        }

        public void MutualWashLick()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashLick.SwiftKissWelcome".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.HandsJerkCoverMouth".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.ReclineInShallows".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.HandsFollowGaze".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.NoRhymeNoPattern".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.HandsCoddleHead".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.HasntTouchedYet".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.LongGroanBursts".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.CatLikeLicks".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.SweetestSplendor".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.WhiteBlotches".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.SlumpIntoAftershocks".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.IsntLettingUp".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.HandClimbDistant".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.TryToSteel".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.IceBlueIrises".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.SmilesAtPraise".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.RedoublesEfforts".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.TorrentialTrembles".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.AlmostGladOver".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.GirlJoinsYou".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.MeetRetribution".Localized());

            OutputText("Scenes.Marielle.MutualWashLick.BecharmedByLascivity".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.7f));

            ClearButtons();

            AddNextButton("Common.Button.BrushOff".Localized(), () => MutualWashLick2(0));

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), () => MutualWashLick2(1));

            AddNextButton("Common.Button.Kiss".Localized(), () => MutualWashLick2(2));

            AddNextButton("Common.Button.Reprimand".Localized(), MutualWashReprimand);
        }

        public void MutualWashLick2(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.MutualWashLick2.DontMindLetsGo".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.NoEffectSilence".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.HmmOhStir".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.PondEdgeHelp".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.MutualWashLick2.EroticViewStraddle".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.ShufflesQuicklyQuelled".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.NonCommittalHm".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.HalcyonSnuggling".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.DozedOffHm".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.OughtAye".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.MutualWashLick2.PropUpDrapeArms".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.TasteYourself".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.RakeThinNeck".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.MutualGaspBreak".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.QuiteNotNeedful".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.ShouldGetOut".Localized());

                        OutputText("Scenes.Marielle.MutualWashLick2.PrudentAgreed".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);
                    }

                    break;
            }

            OutputText("Scenes.Marielle.MutualWashLick2.DoneBeforeYou".Localized());

            OutputText("Scenes.Marielle.MutualWashLick2.WayBackShort".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWashReprimand()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashReprimand.UrgesBridled".Localized());

            OutputText("Scenes.Marielle.MutualWashReprimand.FaceFallsFurther".Localized());

            OutputText("Scenes.Marielle.MutualWashReprimand.TimeToGetOut".Localized());

            OutputText("Scenes.Marielle.MutualWashReprimand.DryInSilence".Localized());

            OutputText("Scenes.Marielle.MutualWashReprimand.RefreshedLively".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, Player.IsReligious() ? -2 : -1));

            DoNext(() => ReturnToCamp(1));
        }

        public void MutualWashDecline()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.MutualWashDecline.KissForeheadDecline".Localized());

            OutputText("Scenes.Marielle.MutualWashDecline.ShushHer".Localized());

            OutputText("Scenes.Marielle.MutualWashDecline.BodyLessFrigid".Localized());

            OutputText("Scenes.Marielle.MutualWashDecline.LoseTrackOfTime".Localized());

            OutputText("Scenes.Marielle.MutualWashDecline.IsAughtMatter".Localized());

            OutputText("Scenes.Marielle.MutualWashDecline.FreezingShiver".Localized());

            OutputText("Scenes.Marielle.MutualWashDecline.StillAtIt".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.5f), new DynStat(GameStats.Lust, -20f));

            Player.ChangeFatigue(-20);

            ClearButtons();

            AddNextButton("Common.Button.Leave".Localized(), () => MutualWashDecline2(0));

            AddNextButton("Common.Button.Hug".Localized(), () => MutualWashDecline2(1));

            AddNextButton("Common.Button.Kiss".Localized(), () => MutualWashDecline2(2));
        }

        public void MutualWashDecline2(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.MutualWashDecline2.NoGeniusNeeded".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.MutualWashDecline2.TurnOnHeel".Localized());

                        OutputText("Scenes.Marielle.MutualWashDecline2.MoreEmbarrassed".Localized());

                        OutputText("Scenes.Marielle.MutualWashDecline2.SheStartsQuieted".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.MutualWashDecline2.DeterminedTurn".Localized());

                        OutputText("Scenes.Marielle.MutualWashDecline2.FreezesThenRelents".Localized());

                        OutputText("Scenes.Marielle.MutualWashDecline2.LockedTogether".Localized());

                        OutputText("Scenes.Marielle.MutualWashDecline2.SearchingForWords".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);
                    }

                    break;
            }

            OutputText("Scenes.Marielle.MutualWashDecline2.JourneyBackFlight".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TaurWash()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurWash.BodyComplications".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Marielle.TaurWash.RequestTurnsHead".Localized());

                OutputText("Scenes.Marielle.TaurWash.HandsRiseDefensive".Localized());

                OutputText("Scenes.Marielle.TaurWash.SinkDeeperDejected".Localized());

                bathOptions.Add(4);

                BathMenu();
            }
            else
            {
                OutputText("Scenes.Marielle.TaurWash.VoiceRequestAgain".Localized());

                OutputText("Scenes.Marielle.TaurWash.PeekingBestialParts".Localized((Player.Femininity >= 59 || Player.HasVagina() ? "your flank until a hint of rose colors the girl's pale features and" : "something until")));

                OutputText("Scenes.Marielle.TaurWash.GetComfortable".Localized());

                OutputText("Scenes.Marielle.TaurWash.SoapInHand".Localized());

                OutputText("Scenes.Marielle.TaurWash.ShudderLightTouch".Localized());

                OutputText("Scenes.Marielle.TaurWash.InterruptGoOn".Localized());

                OutputText("Scenes.Marielle.TaurWash.SuppressShiver".Localized());

                OutputText("Scenes.Marielle.TaurWash.RoamsTracing".Localized());

                OutputText("Scenes.Marielle.TaurWash.LovelyThorough".Localized());

                ClearButtons();

                AddNextButton("Common.Button.JustWash".Localized(), TaurWashJust);

                AddNextButton("Common.Button.LiftButt".Localized(), TaurWashLift);
            }
        }

        public void TaurWashJust()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurWashJust.BanishThought".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.IsItDispleasing".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.BathPassesQuietly".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.ComesToFinish".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.OughtBeAll".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.CrackOpenEyes".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.RaiseOutOfWater".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.QuicklyRisesOut".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.HearShuffling".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.PassTheTowel".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.ReachingUpBack".Localized((Player.Femininity >= 59 || Player.HasVagina() ? "gingerly" : "quickly"), (Player.Femininity >= 59 || Player.HasVagina() ? "some hands occasionally slipping off the fluffy cloth to rake across your [lowerbodyskin] and elicit pleasant waves of goosebumps before they join their brethren once more on their quest" : "though that spell is a short-lived one as she travels")));

            OutputText("Scenes.Marielle.TaurWashJust.NotMuchTime".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.ShallBeAll".Localized((Player.Femininity >= 59 || Player.HasVagina() ? "cheeks rosy" : "mind soon elsewhere again")));

            OutputText("Scenes.Marielle.TaurWashJust.SnapsBack".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.HighTimeCamp".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.PritheeFarewell".Localized());

            OutputText("Scenes.Marielle.TaurWashJust.BodyReinvigorated".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.5f));

            Player.ChangeFatigue(-60);

            Player.HasGottenWashed();

            DoNext(() => ReturnToCamp(1));
        }

        public void TaurWashLift()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurWashLift.WantThemInside".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.FingersStop".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.WhyStopped".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.UUhmBehind".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.BlushingScarlet".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.VoiceQuieter".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.SidlingCloser".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.SighAsMoan".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.KneelingAtHaunches".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.WhenSheFindsIt".Localized());

            OutputText("Scenes.Marielle.TaurWashLift.InhumanAnatomyWorked".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Continue".Localized(), () => TaurWashLift2(false));

            AddNextButton("Common.Button.GetFisted".Localized(), () => TaurWashLift2(true));
        }

        public void TaurWashLift2(bool fisted)
        {
            ClearOutput();

            if (fisted)
            {
                RegisterMacro("wasvirgin", Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin);

                OutputText("Scenes.Marielle.TaurWashLift2.AnimalWithinStirs".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.TongueStumbles".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.StaresAtArm".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.VirginHardly".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.FingersIntoCone".Localized());

                Player.CuntChange(8, true, true, false);

                OutputText("Scenes.Marielle.TaurWashLift2.WhollyWithinYou".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.FingersDontDare".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.OhHorrified".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.NoNeedToKnow".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.ArrivesEntrance".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.SenseEverything".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.BodyJoins".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.CantHangOn".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.VisionGivesOut".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.UpperBodyCollapses".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.FaintlyAware".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.GatherEnergy".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.KneelingLicking".Localized());
            }
            else
            {
                OutputText("Scenes.Marielle.TaurWashLift2.EntirelySilent".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.WritheInPassion".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.SecondHandJoins".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.BuckBackQuivering".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.ComeDownPeaks".Localized((Player.Libido >= 80 || Player.Sensitivity <= 20 ? "even if it took a good while to get there" : "")));

                OutputText("Scenes.Marielle.TaurWashLift2.FinalKissClit".Localized());

                OutputText("Scenes.Marielle.TaurWashLift2.TurningHeadLook".Localized());

                MarielleSex();
            }

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddNextButton("Common.Button.DoNothing".Localized(), () => TaurWashLift3(0));

            AddNextButton("Common.Button.GetHugged".Localized(), () => TaurWashLift3(1));

            AddNextButton("Common.Button.Kiss".Localized(), () => TaurWashLift3(2));
        }

        public void TaurWashLift3(int choice)
        {
            ClearOutput();

            switch (choice)
            {
                case 0:
                    {
                        OutputText("Scenes.Marielle.TaurWashLift3.OnlyWatchCleanse".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.UntilSheShakesIt".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.GraduallyRegains".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.NoWordsResigns".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.DecideTimeToGo".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.OfferToAssist".Localized());
                    }

                    break;

                case 1:
                    {
                        OutputText("Scenes.Marielle.TaurWashLift3.YearnForEmbrace".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.HumsAgainstYou".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.NuzzlesIntoYou".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.LoseTrackAgain".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.LanguidRouse".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.LetHerDown".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.PrayAllowMe".Localized());
                    }

                    break;

                case 2:
                    {
                        OutputText("Scenes.Marielle.TaurWashLift3.DrawnToHerLips".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.SoftAh".Localized((Save.GetInt("timesKissed", 0) < 3 ? "hesitates, but then" : "hungrily")));

                        OutputText("Scenes.Marielle.TaurWashLift3.TasteYourselfFirst".Localized((Save.GetInt("timesKissed", 0) != 0 ? "that" : "a")));

                        OutputText("Scenes.Marielle.TaurWashLift3.EagerlyMatchHer".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.MuffledWhimper".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.PantedBreaths".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.BroughtLifeBack".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.NothingToApologize".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.SoftlyWhispersThanks".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.RiseOutReminder".Localized());

                        OutputText("Scenes.Marielle.TaurWashLift3.KeepsTowelAway".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Corruption, -.8f), new DynStat(GameStats.Lust, 15f));

                        Save.SetInt("timesKissed", Save.GetInt("timesKissed", 0) + 1);
                    }

                    break;
            }

            DoNext(() => TaurWashLift4());
        }

        public void TaurWashLift4()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.TaurWashLift4.DeftAndQuick".Localized());

            OutputText("Scenes.Marielle.TaurWashLift4.OughtBeAllAgain".Localized());

            OutputText("Scenes.Marielle.TaurWashLift4.OfferThanks".Localized());

            OutputText("Scenes.Marielle.TaurWashLift4.QuiteMyPleasure".Localized());

            OutputText("Scenes.Marielle.TaurWashLift4.ReciprocateFarewell".Localized());

            OutputText("Scenes.Marielle.TaurWashLift4.QuagmireAirHumid".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BathFuck()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.BathFuck.HardForAWhile".Localized());

            OutputText("Scenes.Marielle.BathFuck.IsAughtMatter".Localized());

            OutputText("Scenes.Marielle.BathFuck.CockPointingAtHer".Localized());

            ClearButtons();

            AddNextButton("Common.Button.LetUp".Localized(), BathFuckLetUp);

            AddNextButton("Common.Button.Convince".Localized(), BathFuckConvince);
        }

        public void BathFuckLetUp()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.BathFuckLetUp.LetUpUnwilling".Localized());

            OutputText("Scenes.Marielle.BathFuckLetUp.TentativeThanks".Localized());

            OutputText("Scenes.Marielle.BathFuckLetUp.AwkwardSilence".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            bathOptions.Add(5);

            BathMenu();
        }

        public void BathFuckConvince()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.BathFuckConvince.TryToPush".Localized());

            OutputText("Scenes.Marielle.BathFuckConvince.SheAssents".Localized(
                Save.GetInt("timesDicked", 0) > 1 ? "halts to eye you in a flash of " + (Save.GetInt("timesDicked", 0) > 2 ? "rather open" : "barely hidden") + " distrust before resigning herself with a sigh" : "sighs and mutters something else underneath her breath"
            ));

            OutputText("Scenes.Marielle.BathFuckConvince.FingersLessCold".Localized());

            OutputText("Scenes.Marielle.BathFuckConvince.WahYelp".Localized());

            OutputText("Scenes.Marielle.BathFuckConvince.TensesUp".Localized());

            if (VaginalAvailable())
            {
                OutputText("Scenes.Marielle.BathFuckConvince.PlaceholderOnly".Localized(
                    Save.GetInt("timesDicked", 0) > 3 ? "You get no answer from Marielle, her jaw snapped closed and only a hand held against your chest while her face scrunches up further and further. Five slow, irresolute seconds pass like this before she lets it fall again with a jagged exhalation, turning her head aside. Is that consent? You're not sure. It should be. Still, her insides oppose your intrusion as much as her body language does, demanding a tempered approach from you, even if she herself may obstruct you no further. You guess this is as much as you'll get, now." : (Save.GetInt("timesDicked", 0) > 1 ? "[say:I am...] Marielle doesn't finish that, the deepening furrows on her brow speaking for themselves as two hands rise up. She never reaches for or stops you however, and you interpret her letting them fall again and turning her head aside as some manner of consent. Though even if she herself may not hinder you, her insides are in anything but agreement with your entry, demanding you to take a slower approach." : "[say:I am... fine,] Marielle says, head turned aside and brow now furrowed even deeper. [say:Though... take... temperance, I beg you.] Her insides being this unyielding, there is hardly any other approach, anyway. The water does little to help you glide in, either, leaving the only other option to be a forceful one. [if (corruption > 75) {And even you don't feel like doing something like that right now.|You're [if (corruption < 25) {perfectly }]fine with giving her the time she needs and decide to take the matter without hurry.}]")
                ));

                OutputText("Scenes.Marielle.BathFuckConvince.RelaxBothSides".Localized(
                    Save.GetInt("timesDicked", 0) > 2 ? "makes no difference. None of your affections do anything to ease her up while you slowly but surely work" : (Save.GetInt("timesDicked", 0) > 1 ? "shows next to no positive effect on her, though you are able to slowly but surely work" : "does work, to some small extent, as slowly but surely, you're able to sink")
                ));

                OutputText("Scenes.Marielle.BathFuckConvince.TeeteringEdge".Localized((Player.Cocks[0].Thickness < 2.3 ? " slightly" : "")));

                OutputText("Scenes.Marielle.BathFuckConvince.ReachDeepest".Localized((Save.GetInt("timesDicked", 0) > 2 ? "acknowledges the fact with a mere glance, holding herself otherwise still. It's not all too comfortable, though. You nudge her and coax her up a little, which races a hair-raising shiver up your spine, then settle her down into a better position that lets her lie half-propped in" : "glances at [if (cocklength > 9) {what's left outside|your [if (cocklength > 8) {near-}]connected pelvises}], murmuring to herself, and awkwardly shifts around. The stiff movement races another hair-raising shiver up your spine, and you help her into a more comfortable half-lying position, letting her prop herself upon")));

                OutputText("Scenes.Marielle.BathFuckConvince.FoughtBackRelease".Localized(
                    Save.GetInt("timesDicked", 0) > 3 ? "the faintest of nods from Marielle herself--although it was more of a twitch aimed at the moss-capped stonework" : "a weak nod from Marielle herself--although she does" + (Save.GetInt("timesDicked", 0) > 1 ? "n't even remotely meet" : " avoid") + " your eyes"
                ));

                OutputText("Scenes.Marielle.BathFuckConvince.ShortRockingMotions".Localized());

                OutputText("Scenes.Marielle.BathFuckConvince.TakeHipsThrust".Localized((Save.GetInt("timesDicked", 0) > 3 ? " and her first utterance in what felt like an eternity" : ""), (Save.GetInt("timesDicked", 0) > 3 ? " whatsoever" : (Save.GetInt("timesDicked", 0) > 1 ? " beyond the smallest wince" : ""))));

                OutputText("Scenes.Marielle.BathFuckConvince.ChurningWaterPrevents".Localized());

                OutputText("Scenes.Marielle.BathFuckConvince.VisionSwimsFire".Localized((Save.GetInt("timesDicked", 0) > 1 ? "lifeless" : "cold")));

                OutputText("Scenes.Marielle.BathFuckConvince.HighFadesSoon".Localized((Save.GetInt("timesDicked", 0) > 3 ? "continues" : "elects"), (Save.GetInt("timesDicked", 0) > 2 ? ", another of her grunts swallowed behind gritted teeth. As your cum flows out and mixes with the clear, warm water, she stares at it for a long moment until she finally peers up at you, cracks Open her mouth, but then closes it shut in favor of a glum" + (Save.GetInt("timesDicked", 0) > 3 ? " and dangerously bitter" : "") + " frown to the side again, one of her hands taking it on itself" : ". She makes a muffled [say:Hnngh] as your cum flows and mixes with the clear, warm water, " + (Save.GetInt("timesDicked", 0) > 1 ? "a hollow stare following its course until she gives her head a shake and" : "more following suit when she") + " starts")));

                OutputText("Scenes.Marielle.BathFuckConvince.CreampieOozing".Localized());

                OutputText("Scenes.Marielle.BathFuckConvince.BreaksSilence".Localized(
                    Save.GetInt("timesDicked", 0) > 1 ? "sighing" + (Save.GetInt("timesDicked", 0) > 3 ? " flatly" : "") + " before" : "softly"
                ));

                OutputText("Scenes.Marielle.BathFuckConvince.DisjoinWhisper".Localized(
                    Save.GetInt("timesDicked", 0) > 3 ? "I ought to remove" : (Save.GetInt("timesDicked", 0) > 2 ? "I ought not linger overlong" : "I suppose I should linger not overlong"),

                    Save.GetInt("timesDicked", 0) > 3 ? "too soft to give out any emotion, [saystart]ere" : "still betraying a notable " + (Save.GetInt("timesDicked", 0) > 1 ? "clot" : "hint") + " of displeasure, [saystart]lest"
                ));

                Player.Orgasm("Dick");

                BathFuck2(false);

                AddNextButton("Common.Button.Ask".Localized(), () => BathFuck2(true));
            }
            else
            {
                OutputText("Scenes.Marielle.BathFuckConvince.StrainedVoice".Localized());

                OutputText("Scenes.Marielle.BathFuckConvince.NoChoicePullOut".Localized(
                    Save.GetInt("timesDicked", 0) > 0 ? "She has never been on board with going this far, much less enjoyed you pushing her into " + (Save.GetInt("timesDicked", 0) > 1 ? "these situations" : "such a situation") + ", and right now she looks desperate to be anywhere but here, pinned underneath you. There's no further cajoling her, you reckon. " : ""
                ));

                OutputText("Scenes.Marielle.BathFuckConvince.SouredMoodCraving".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 70f));

                ClearButtons();

                AddNextButton("Common.Button.Handjob".Localized(), () => BathFuckInsulted(true));

                AddNextButton("Common.Button.GetOut".Localized(), () => BathFuckInsulted(false));

                AddNextButton("Common.Button.Apologize".Localized(), BathFuckApologize);
            }
        }

        public void BathFuckApologize()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.BathFuckApologize.OfferApology".Localized());

            OutputText("Scenes.Marielle.BathFuckApologize.LittleMoreThanNod".Localized());

            AddButton(2, "Common.Button.Apologize".Localized(), null);
        }

        public void BathFuckInsulted(bool handjob)
        {
            ClearOutput();

            if (handjob)
            {
                OutputText("Scenes.Marielle.BathFuckInsulted.LessAverseHands".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.FrownDeepens".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.UnenthusiasticHands".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.MethodicalKneading".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.QuickPumpingStrokes".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.WontLastLong".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.SplatterIntoPond".Localized());

                OutputText("Scenes.Marielle.BathFuckInsulted.TakesABit".Localized());

                Player.Orgasm("Dick");

                MarielleSex();
            }
            else
                OutputText("Scenes.Marielle.BathFuckInsulted.DealWithLust".Localized());

            OutputText("Scenes.Marielle.BathFuckInsulted.SoftTowel".Localized());

            OutputText("Scenes.Marielle.BathFuckInsulted.DoneAndDressed".Localized());

            OutputText("Scenes.Marielle.BathFuckInsulted.BreezeCarriesGas".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void BathFuck2(bool asked)
        {
            ClearOutput();

            if (asked)
            {
                OutputText("Scenes.Marielle.BathFuck2.GlancesAtArm".Localized());

                OutputText("Scenes.Marielle.BathFuck2.ShouldntStayLong".Localized(
                    Save.GetInt("timesDicked", 0) > 3 ? "[say:Aye.][pg]You wait for her to say more than that, to maybe elaborate, but she doesn't. Instead, you're submerged in silence while she speedily dries herself off, pulls her dress over her head, buttons it up and smooths it down, then shakes her hair loose again. Well, you" : "[say:It may, yes. Although, well, I was fast-locked in sleep withinside my bath, thereon.] She " + (Save.GetInt("timesDicked", 0) > 1 ? "falls back into a dour silence while she dries herself off. You try to imagine. It must have been painful. [say:Aye, painful.] After another sigh" : "wrinkles her nose at the memory as she dries herself off. [say:'Twas... an unpleasant dawn, to put it in words ungloomed.] You try to imagine. Whatever you come up with, it sounds painful. [say:Verily so.] Faintly shuddering") + ", she pulls her dress over her head, buttons it up and smooths it down, then lets her hair loose again. You"
                ));
            }
            else
                OutputText("Scenes.Marielle.BathFuck2.WatchHerDry".Localized());

            OutputText("Scenes.Marielle.BathFuck2.ISeeMumbles".Localized(
                Save.GetInt("timesDicked", 0) > 1 ? "double-checking her gown and towel before she directs a thought-absorbed " + (Save.GetInt("timesDicked", 0) > 3 ? "nod" : "curtsy") + " of parting at you" + (Save.GetInt("timesDicked", 0) > 2 ? " and" : ".[pg][say:Well, I shall... leave you to it, then.] With that, she") + " swayingly" : "apparently looking for her glasses before snapping her head towards the desk, where she left them earlier.[pg][say:Well, I, ah, will leave you to it, then.] She gives a brief curtsy in your direction and"
            ));

            OutputText("Scenes.Marielle.BathFuck2.GetOutScrub".Localized());

            OutputText("Scenes.Marielle.BathFuck2.SpringInStep".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, -.6f));

            MarielleSex(true);

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveRose()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.GiveRose.FoundStrangePlant".Localized());

            OutputText("Scenes.Marielle.GiveRose.DemurralFizzles".Localized());

            OutputText("Scenes.Marielle.GiveRose.FlowerOfStone".Localized());

            OutputText("Scenes.Marielle.GiveRose.GnarledBallTouches".Localized());

            OutputText("Scenes.Marielle.GiveRose.IsThisFine".Localized());

            OutputText("Scenes.Marielle.GiveRose.BuriedFaceSilent".Localized());

            OutputText("Scenes.Marielle.GiveRose.WaterCries".Localized());

            OutputText("Scenes.Marielle.GiveRose.FirstMinuteGreen".Localized());

            OutputText("Scenes.Marielle.GiveRose.RemarkableWhisper".Localized());

            OutputText("Scenes.Marielle.GiveRose.MaybeWater".Localized());

            OutputText("Scenes.Marielle.GiveRose.RightKindOfPlant".Localized());

            OutputText("Scenes.Marielle.GiveRose.CertainerNod".Localized());

            OutputText("Scenes.Marielle.GiveRose.AnotherRiddle".Localized());

            Player.DestroyItems(ConsumableIds.DesertRose);

            Save.SetInt("rose", 2);

            ClearButtons();

            AddNextButton("Common.Button.TellStory".Localized(), RoseStory);

            AddNextButton("Common.Button.MakeShort".Localized(), RoseShort);
        }

        public void RoseStory()
        {
            ClearOutput();

            var opts = new[] { "and how you vanquished that monstrous colossus", "and your eventual victory over that colossus", "and your eventual conciliation with that colossus", "your eventual conciliation with that colossus, and how you rode atop its back to freedom" }[0];

            OutputText("Scenes.Marielle.RoseStory.PlungedIntoEncounter".Localized(
                opts,

                Save.GetInt("fate", 0) == 4 ? "It does maybe sound like a tall tale, and you don't know whether she even believes all of it" : "You can't read her mind"
            ));

            OutputText("Scenes.Marielle.RoseStory.RemarkableIndeed".Localized());

            OutputText("Scenes.Marielle.RoseStory.HugThankedNot".Localized());

            OutputText("Scenes.Marielle.RoseStory.ShouldTakeLeave".Localized());

            OutputText("Scenes.Marielle.RoseStory.CertainlyAgreed".Localized());

            OutputText("Scenes.Marielle.RoseStory.TwoBirdsDartUp".Localized());

            DoNext(() => ReturnToCamp(2));
        }

        public void RoseShort()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.RoseShort.FoundInDesert".Localized());

            OutputText("Scenes.Marielle.RoseShort.HappyChanceMeeting".Localized());

            OutputText("Scenes.Marielle.RoseShort.LetSeamstressBe".Localized());

            OutputText("Scenes.Marielle.RoseShort.HmmUtteredStudy".Localized());

            OutputText("Scenes.Marielle.RoseShort.SmallBirdFlutters".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Leave()
        {
            ClearOutput();

            OutputText("Scenes.Marielle.Leave.YouBackFromThe".Localized());
        }

    }
}