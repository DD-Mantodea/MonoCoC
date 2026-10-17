using MonoCoC.Attributes;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<KihaFollowerScene>("coc:kiha_follower")]
    public class KihaFollowerScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.KihaFollower);

        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Kiha)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public string debugName => "Kiha";

        public string debugHint => "";

        public override void Entry()
        {

        }

        public bool TimeChange()
        {
            var needNext = false;

            Pregnancy.PregnancyAdvance();

            if (Pregnancy.IsPregnant)
            {
                if (KihaPregUpdate())
                    needNext = true;

                if (Pregnancy.Incubation == 0)
                {
                    KihaGivesBirth();

                    Pregnancy.KnockUpForce();

                    needNext = true;
                }
            }

            var maturity = CoC.Instance.Flags.Get(GameFlagIds.KihaChildMaturityCounter);

            if (maturity > 1)
            {
                if (maturity != 144)
                {
                    maturity -= 1;

                    CoC.Instance.Flags.Set(GameFlagIds.KihaChildMaturityCounter, maturity);
                }

                if (maturity == 240)
                {
                    KihaBreastfeedingTime();

                    needNext = true;
                }
                else if (maturity == 72)
                {
                    KihaTrainsHerKids();

                    needNext = true;
                }
            }
            else if (maturity == 1)
            {
                KihaChildGraduationTime();

                needNext = true;
            }

            return needNext;
        }

        public bool TimeChangeLarge() => false;

        public void Reset()
        {

            Save.SetInt("badCookingTasted", 0);

            Save.SetBool("flowerGifted", false);

            Save.SetBool("kidFirebreathing", false);

            Save.SetBool("wakeUp", false);
        }

        public void KihaSprite(bool nude = false)
        {

        }

        public string KihaChildren()
        {

            if (TotalKihaChildren() > 1)
                return "children";

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0)
                return "son";

            return "daughter";
        }

        public bool FollowerKiha()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) == 1)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0)
                return true;

            return false;
        }

        public void KihaAffection(int changes = 0)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) == 2)
                CoC.Instance.Flags.Set(GameFlagIds.KihaAffection, CoC.Instance.Flags.Get(GameFlagIds.KihaAffection) + changes);

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffection) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.KihaAffection, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffection) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.KihaAffection, 100);
        }

        public bool CanKihaGetPregnant()
        {

            if (View.Time.Days % 15 == 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaPregnancyPotential) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaChildMaturityCounter) <= 0 && !Player.IsPregnant())
                return true;
            else
                return false;
        }

        public void KihaKnockUpAttempt()
        {

            if ((CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) != 1 && CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0) && CanKihaGetPregnant() && Player.KnockUpChance(10 - TotalKihaChildren() / 5, 50))
            {
                if (Debug)
                    OutputText("Scenes.Kihafollowerscene.KihaKnockUpAttempt.DebugKihaImpregnated".Localized());

                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.KihaIncubation);
            }
        }

        public int TotalKihaChildren()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) + CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenGirls) + CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenHerms);
        }

        public void KihaSpiderEventIntro()
        {
            ClearOutput();

            ShowImage("kiha-spider-event");

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.JourneyToSwamp".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.IslandShoreArrival".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.ApproachTreesWarily".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.HearWingsAbove".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.NarrowWeaponDodge".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.SpyAccusation".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.StunnedExclamation".Localized());

            if (!Player.IsPureEnough(66))
                OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.CalmDragoness".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.IrateStaredown".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.HowlsDenial".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.FireWallCharge".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaSpiderEventIntro.FightKihaBegins".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kiha")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CurrentMonster.CreateStatusEffect(StatusEffectIds.Spiderfight, 0, 0, 0, 0);
        }

        public void LoseKihaPreSpiderFight()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.LoseKihaPreSpiderFight.GrabbedByThroat".Localized());

            OutputText("Scenes.Kihafollowerscene.LoseKihaPreSpiderFight.GentleThreat".Localized());

            OutputText("Scenes.Kihafollowerscene.LoseKihaPreSpiderFight.HelplessStruggle".Localized());

            OutputText("Scenes.Kihafollowerscene.LoseKihaPreSpiderFight.WarnOrAbandon".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.WarnKiha".Localized(), WarnKihaOfHerImpendingDemise);

            AddButton(1, "Common.Button.LetThem".Localized(), LetTheSpidersHaveTheirWayWithKiha);
        }

        public void PlayerBeatsUpKihaPreSpiderFight()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.PlayerBeatsUpKihaPreSpiderFight.SlumpedAgainstTree".Localized());

            OutputText("Scenes.Kihafollowerscene.PlayerBeatsUpKihaPreSpiderFight.GroansToStand".Localized());

            OutputText("Scenes.Kihafollowerscene.PlayerBeatsUpKihaPreSpiderFight.BeholdFallenFoe".Localized());

            OutputText("Scenes.Kihafollowerscene.PlayerBeatsUpKihaPreSpiderFight.WhimpersAtDriders".Localized());

            OutputText("Scenes.Kihafollowerscene.PlayerBeatsUpKihaPreSpiderFight.KihaHangsHead".Localized());

            OutputText("Scenes.Kihafollowerscene.PlayerBeatsUpKihaPreSpiderFight.FleeOrStay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.HelpKiha".Localized(), HelpKihaAgainstSpoidahs);

            AddButton(1, "Common.Button.LeaveHer".Localized(), LeaveKihaToSpoidahHorde);
        }

        public void WarnKihaOfHerImpendingDemise()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.WarningShout".Localized());

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.DismissiveSmirk".Localized());

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.DriderLoomsBehind".Localized());

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.StabPaleTemple".Localized());

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.KihaSnapsQuestion".Localized());

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.TalkLaterAnswer".Localized());

            OutputText("Scenes.Kihafollowerscene.WarnKihaOfHerImpendingDemise.SpiderHordeFight".Localized());

            var mob = EnemyRegistry.Create(new Identifier("coc", "spider_morph_mob"));

            mob.MissFirstRound = true;

            CombatScene.Start(mob, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            Player.HealthChange(100);

            Player.ChangeFatigue(-30);

            Player.ApplyStats(new DynStat(GameStats.Lust, -40f));
        }

        public void LetTheSpidersHaveTheirWayWithKiha()
        {
            ClearOutput();

            KihaSprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffectionLevel, -1);

            OutputText("Scenes.Kihafollowerscene.LetTheSpidersHaveTheirWayWithKiha.SilentBetrayal".Localized());

            OutputText("Scenes.Kihafollowerscene.LetTheSpidersHaveTheirWayWithKiha.CordialDriderThanks".Localized());

            OutputText("Scenes.Kihafollowerscene.LetTheSpidersHaveTheirWayWithKiha.DriderJoinsFray".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void HelpKihaAgainstSpoidahs()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.OfferHandToKiha".Localized());

            OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.SuspiciousRecoil".Localized());

            OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.InsistentBeckon".Localized());

            OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.DumbfoundedAsk".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.BeatWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.TalkWord".Localized());

            OutputText("Scenes.Kihafollowerscene.HelpKihaAgainstSpoidahs.HaulKihaUp".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "spider_morph_mob")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            Player.HealthChange(100);

            Player.ChangeFatigue(-30);

            Player.ApplyStats(new DynStat(GameStats.Lust, -40f));
        }

        public void LeaveKihaToSpoidahHorde()
        {
            ClearOutput();

            KihaSprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffectionLevel, -1);

            OutputText("Scenes.Kihafollowerscene.LeaveKihaToSpoidahHorde.FleeTheSwamp".Localized());

            OutputText("Scenes.Kihafollowerscene.LeaveKihaToSpoidahHorde.GrimSatisfaction".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void BeatSpiderMob()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffectionLevel, 1);

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.DriderCallsRetreat".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.SpidersRetreat".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.WhisperedWhy".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.DenyLethiceAllegiance".Localized());

            if (silly)
                OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.FlusteredInsult".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.BluntInsult".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.WorriedScolding".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.YouWord".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.SqueezeShoulder".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.OfferFriendship".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.ArrogantAcceptance".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.ShoutedFarewell".Localized());

            OutputText("Scenes.Kihafollowerscene.BeatSpiderMob.KnowingWink".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffectionLevel, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToSpiderMob()
        {
            ClearOutput();

            KihaSprite(true);

            OutputText("Scenes.Kihafollowerscene.LoseToSpiderMob.CollapseDefeated".Localized());

            OutputText("Scenes.Kihafollowerscene.LoseToSpiderMob.HelplessWatching".Localized());

            OutputText("Scenes.Kihafollowerscene.LoseToSpiderMob.CrawlToKiha".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffectionLevel, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void KihaFriendlyGreeting(bool output = true)
        {
            ClearOutput();

            KihaSprite();

            if (output && CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) == 1 && CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) >= 7)
            {
                KihaAdmitsSheLikesYourWang();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) == 2)
            {
                WarmLoverKihaIntro(output);

                return;
            }

            if (output && CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) == 6 && Player.IsPureEnough(50))
            {
                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.FamiliarTerritory".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.FindKihaDowncast".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.ConcernedQuestion".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.RedEyesGlance".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.AdmitsFailure".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.AdmitsLethiceWon".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.RestrainSlapUrge".Localized());

                if (View.Time.Hours < 20)
                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.CloudsWord".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.StarsWord".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.PointAtSky".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.ScoffsAtSky".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.ExasperatedSigh".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.ShakeHeadExplain".Localized());

                if (View.Time.Hours < 20)
                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.CloudsWordAgain".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.StarsWordAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.PointOutPatterns".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.LethiceWontRest".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.AffirmSharedResolve".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.WhyNeverTogether".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.UnreadyButSoon".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.KihaReturnsEmbrace".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaTalkStage, CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) + 1);

                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

                ReturnToCamp(1);

                return;
            }
            else
                if (output)
                {
                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.VisitKihaGrove".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.KihaNeedSpiderText) == 1)
                    {
                        OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.SpidersWatchCrossing".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.KihaNeedSpiderText, 0);
                    }

                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.CallOutForKiha".Localized());

                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.ExplodesFromTreetops".Localized());

                    OutputText("Scenes.Kihafollowerscene.KihaFriendlyGreeting.GreataxeGreeting".Localized());
                }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) < 6)
                AddButton(0, "Common.Button.Talk".Localized(), TalkToFriendlyKiha);

            AddButton(1, "Common.Button.Spar".Localized(), SparWithKiha);

            AddButton(2, "Common.Button.Hug".Localized(), HugFriendWarmKiha);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void SparWithKiha()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.SparWithKiha.ProposeMockFight".Localized());

            OutputText("Scenes.Kihafollowerscene.SparWithKiha.ReturnSmugGrin".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kiha")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CurrentMonster.CreateStatusEffect(StatusEffectIds.Spar, 0, 0, 0, 0);
        }

        public void WinSparWithKiha()
        {
            ClearOutput();

            if (!FollowerKiha())
            {
                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.KihaSlumpsOnTree".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.BitterPersonalLoss".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.HelpsHerUp".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.FlusteredFliesOff".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.SighReturnCamp".Localized());

                KihaAffection(20);
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.KihaSlumpsOnRock".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.BitterPersonalLossAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.HelpsHerUpAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.FlusteredStormsOff".Localized());

                OutputText("Scenes.Kihafollowerscene.WinSparWithKiha.SighReturnStuff".Localized());

                KihaAffection(20);
            }

            CombatScene.CleanupAfterCombat();
        }

        public void SparWithFriendlyKihaLose()
        {
            ClearOutput();

            if (!FollowerKiha())
            {
                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.YieldsExhausted".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.TeasingChinPop".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.OffersMoreSparring".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.PityForKiha".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.DustOffReturnCamp".Localized());
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.YieldsExhaustedAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.TeasingChinPopAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.EmbarrassedEncouragement".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.PityForKihaAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.SparWithFriendlyKihaLose.DustOffCenterCamp".Localized());
            }

            KihaAffection(10);

            CombatScene.CleanupAfterCombat();
        }

        public void HugFriendWarmKiha()
        {
            ClearOutput();

            ShowImage("kiha-hug");

            OutputText("Scenes.Kihafollowerscene.HugFriendWarmKiha.SurpriseHug".Localized());

            OutputText("Scenes.Kihafollowerscene.HugFriendWarmKiha.StammersIdiot".Localized());

            OutputText("Scenes.Kihafollowerscene.HugFriendWarmKiha.SoftWarmEmbrace".Localized());

            OutputText("Scenes.Kihafollowerscene.HugFriendWarmKiha.EmbraceCutShort".Localized());

            OutputText("Scenes.Kihafollowerscene.HugFriendWarmKiha.ShakeHeadReturnCamp".Localized());

            KihaAffection(5);

            ReturnToCamp(1);
        }

        public void TalkToFriendlyKiha()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) <= 3)
            {
                CoC.Instance.Flags.Set(GameFlagIds.KihaTalkStage, 3);

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.AskDragonessChat".Localized());

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.KihaCrossesArms".Localized());

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.ReclineAskAboutHer".Localized());

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.AskAboutBitsCut".Localized());

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.HumorDragonAgain".Localized());

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.AskSheTurnsNose".Localized());

                OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.GodDammitKiha".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) == 4)
                {
                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.KihaSpeaksFirst".Localized());

                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.SuppressGrinAgree".Localized());

                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.GlanceSilencesYou".Localized());

                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.SheTrailsThenFire".Localized());

                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.DemonsMadeWarrior".Localized());

                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.DragonessLeapsAway".Localized());

                    OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.SignsOfProgress".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) == 5)
                    {
                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.CoaxDragonAgain".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.HissDoneTalking".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.ClawedHandSwipe".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.TakenAbackOutburst".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.AnimalisticSnarl".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.CalmDemonQueen".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.DragonessBlastsOff".Localized());

                        OutputText("Scenes.Kihafollowerscene.TalkToFriendlyKiha.HopeSheStaysSafe".Localized());
                    }

            CoC.Instance.Flags.Set(GameFlagIds.KihaTalkStage, CoC.Instance.Flags.Get(GameFlagIds.KihaTalkStage) + 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            ReturnToCamp(1);
        }

        public void KihaXSalamander()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.SwampTrekArrival".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.KihaPlummetsDown".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.KihaRisesSneering".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.ExplainJustExploring".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.ReadyWeaponCombat".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.AxeBlowOnslaught".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.BackToTreeCorner".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.WarningTooLate".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaXSalamander.LieThereOutmatched".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LieThere".Localized(), LieThere);

            AddButton(1, "Common.Button.JumpThem".Localized(), JumpDaBitches);

            AddButton(2, "Common.Button.Gtfo".Localized(), GTFO);
        }

        public void GTFO()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaAndHelWhoopie, -1);

            OutputText("Scenes.Kihafollowerscene.GTFO.SlipAwayInMud".Localized());

            ReturnToCamp(1);
        }

        public void LieThere()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaAndHelWhoopie, 1);

            KihaSprite(true);

            OutputText("Scenes.Kihafollowerscene.LieThere.LetThingsUnfold".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.RecognizeHeliaTail".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelLeersAtKiha".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaFlingsMud".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.DuckUnderFlame".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.CoughThroughSmoke".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.SalamanderScowl".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaStandsDumbfounded".Localized());

            if (!Player.IsGoo())
                OutputText("Scenes.Kihafollowerscene.LieThere.MeatsackWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.LieThere.GoosackWord".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.YourRearIsMine".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HeBelongsToYou".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.GetOutOfWay".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelFumesFat".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaHalfTurnTaunt".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelStompsFoot".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.ShitterMuscleCry".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelSneersSmallTits".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaGrabsOwnCups".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.Kihafollowerscene.LieThere.StammerUnderFoot".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.ArmorRippedByFoot".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaFeralOutrage".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaShovesHel".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.GirlsTalkOverYou".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaHeelClawSlide".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelRisesFlaming".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelRubsCockhead".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.GrowlFootFuck".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.SilentCryEndure".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaShowsStain".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.Kihafollowerscene.LieThere.HelShakesHips".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaBreathesFire".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.SomethingOnFace".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.GrabTornClothes".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.ShoutBreakItUp".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.CannotTellApart".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.SimilarVoicesNoted".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.YouLikeMeBetter".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.ImpossibleChoice".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.UnisonOutrage".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.AffirmYourChoice".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.BothWipeMudOff".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelConcedesFootjob".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.AdmitsFootjobSkill".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.BothBurstLaughing".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaReluctantPeace".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.VisitPlainsHint".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.KihaFliesOffLaughing".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.HelYellsDefiance".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.AlmostBeautifulMoment".Localized());

            OutputText("Scenes.Kihafollowerscene.LieThere.ArmorSquelchCum".Localized());

            Player.Orgasm("Generic");

            ReturnToCamp(1);
        }

        public void JumpDaBitches()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaAndHelWhoopie, 1);

            KihaSprite(true);

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.ScowlRiseFromMud".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.CloakTossedAside".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.HelGrowlsMuffled".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.HelTumblesOffCheek".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.StrideOver".Localized());
            else
                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Kihafollowerscene.JumpDaBitches.SlitherOver".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.JumpDaBitches.MoveTowardHer".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.PlantFootOnChest".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.TellHerSitDown".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.ReachIntoArmor".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.KihaGapesWideEyed".Localized());

            if (Player.Cocks[0].Length >= 8)
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.BendsDownThroat".Localized());
            else
                if (Player.Cocks[0].Length > 6)
                    OutputText("Scenes.Kihafollowerscene.JumpDaBitches.HitsBackOfThroat".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.JumpDaBitches.RestsOnTongueTip".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.GaggingLightSlap".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.GrabHeadUseMouth".Localized());

            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.WithdrawFromKiha".Localized());

                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.HandOnEachHead".Localized());
            }
            else
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.FreeCockFromArmor".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.PullGirlsOffCock".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.SLetter".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.GrinSalivaStrands".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.SLetterAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.KindlyGetThemOff".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.HelSmilesRelieved".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.KihaHuffsMotionless".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.HelSquealsHammering".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.CruelIdeaGrabArms".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.YankCockFromHel".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.LineUpDragonessAss".Localized());

            if (Player.CockArea(0) < 36)
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.UpToTheHilt".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.AnusStretchedBeyond".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.SavageBigSoftTits".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.SlamBackMightyThrust".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.AnalWallsMilking".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.KihaTriesToDeny".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.CumDeepInKiha".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Kihafollowerscene.JumpDaBitches.CumSquirtsBack".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.FinalWeakThrusts".Localized());

            OutputText("Scenes.Kihafollowerscene.JumpDaBitches.WeakLaughWithdraw".Localized());

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void KihaAdmitsSheLikesYourWang()
        {
            ClearOutput();

            KihaSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaAdmittedWarmFeelinz) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.KihaAdmittedWarmFeelinz, 1);

                OutputText("Scenes.Kihafollowerscene.KihaAdmitsSheLikesYourWang.ExploringDragonTerritory".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaAdmitsSheLikesYourWang.KihaTailSwatsRump".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaAdmitsSheLikesYourWang.HugHerNextLevel".Localized());
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.KihaAdmitsSheLikesYourWang.KihaDropsFromTrees".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaAdmitsSheLikesYourWang.HugHerBriefPrompt".Localized());
            }

            ClearButtons();

            AddButton(1, "Common.Button.Spar".Localized(), SparWithKiha);

            AddButton(2, "Common.Button.Hug".Localized(), HugFriendWarmKiha);

            AddButton(3, "Common.Button.Lovinhug".Localized(), LovinHugKiha);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void LovinHugKiha()
        {
            ClearOutput();

            ShowImage("kiha-lovinhug");

            KihaSprite(true);

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffectionLevel, 2);

            CoC.Instance.Flags.Set(GameFlagIds.KihaAffection, 0);

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.EmbraceKihaTightly".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.KihaLooksPrefix".Localized());

            if (Player.Tallness >= 95)
                OutputText("Scenes.Kihafollowerscene.LovinHugKiha.UpWord".Localized());
            else
                if (Player.Tallness <= 60)
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.DownWord".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.MoistEyesStutter".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.PraiseHerCompany".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.SensesYourDiscomfort".Localized());

            if (Player.Tallness >= 95)
                OutputText("Scenes.Kihafollowerscene.LovinHugKiha.PullingItDown".Localized());
            else
                if (Player.Tallness <= 60)
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.TiltingItUp".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.TiltingItSlightly".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.PlantKissOnLips".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.ForgottenAxeSentinel".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kihafollowerscene.LovinHugKiha.SpringsFreeOnThigh".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.ExposedVagTingles".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.GroinAssholeTingle".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.PantBreathlessFrown".Localized());

            if (Player.Tallness >= 95)
                OutputText("Scenes.Kihafollowerscene.LovinHugKiha.LeanDown".Localized());
            else
                if (Player.Tallness <= 60)
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.ReachUp".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovinHugKiha.ReachForward".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.SweatSlickedBreasts".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.KihaMoansSucking".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.ComfortWeakenedGirl".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.KissHerAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.WingsScatterLeaves".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Kihafollowerscene.LovinHugKiha.EnjoyingEmbrace".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.LovinHugKiha.ClingForDearLife".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.GapInFoliage".Localized());

            OutputText("Scenes.Kihafollowerscene.LovinHugKiha.TacklesIntoBed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            if (Player.HasCock())
                LovingHugDickings();
            else
                if (Player.HasVagina())
                    LovingHugsGirlFuckSex();
                else
                    LovingHugsForRetards();
        }

        public void LovingHugDickings()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.KihaLaughs".Localized());

            if (Player.BiggestCockArea() > 100)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.WalkWithItSwinging".Localized());
            else
                if (Player.BiggestCockArea() > 9)
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.StickingOutForMe".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.TooSmallToFeel".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SqueezeWithHerGrip".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.PatheticTauntPumping".Localized());

            if (Player.BiggestCockArea() <= 9)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.LittleWord".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.PuttyInHerHands".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[Player.BiggestCockIndex()].Length >= 36)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.TipBumpingHerNose".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SoftBreastsEnvelop".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ParagraphBreak".Localized());

            if (Player.HorseScore() > 4 || Player.DogScore() > 4 || Player.CatScore() > 4)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.GrowlingWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.GruntingWord".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ClenchingAtClimax".Localized());

            if (Player.CumQ() > 400)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ThickWord".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.JetsOverKihaFace".Localized());

            if (Player.CumQ() >= 400)
            {
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.HairWord".Localized());

                if (Player.CumQ() >= 800)
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.FloorWord".Localized());

                if (Player.CumQ() >= 1200)
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.WallsWord".Localized());
            }

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ChestCrimsonShiver".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.RemindSheAlreadyDid".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SightStirsHardness".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SlowThrustNapeBites".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.LettingWord".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.YourOtherPrefix".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.RestOfYourPrefix".Localized());

                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.MalenessDripBelly".Localized());
            }

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.AskWhatSheSaid".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.HipsThrustingFuck".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) < 50)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.HiltedLoveTunnel".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ProdigiousSizeDepth".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ButtingWombEntrance".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SlipperyCuntCaress".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.ThanksToYourPrefix".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.DualWord".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.MultipleWord".Localized());

                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.TwitchingFrontCoated".Localized());
            }

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.JetsSoakWomb".Localized());

            if (Player.CumQ() >= 1000)
            {
                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.FloodCumReceptacle".Localized());

                if (Player.CumQ() <= 2000)
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.FillKihaBed".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SpermySwimmingPool".Localized());

                OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SentencePeriod".Localized());
            }

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.WindDownStillSucking".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.KihaKeepsCumming".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.SighAndNap".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.AWhileLaterLabel".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugDickings.FliesBackToRecover".Localized(
                Player.ArmorDescript()
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LovingHugsGirlFuckSex()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.RubsMonsAndTeases".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.TwistAndPullAtop".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.NosingHerBud".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.PleasureBoltStrikes".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.MutterThenSlamDown".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.DrippingWord".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.RunningWord".Localized());
                else
                    if (Player.Wetness() < 5)
                        OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.FloodingWord".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.ExplodingWord".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.SquirtOnHerFace".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.DragsOffBesideYou".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.AWhileLaterLabel".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsGirlFuckSex.FliesBackToRecover".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LovingHugsForRetards()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.RoughAssSlap".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.TicklingButtcheeks".Localized());

            if (Player.AnalCapacity() < 30)
                OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.MixedPainPleasure".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.InPleasureOnly".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.UnexpectedIntrusion".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.RollsYouAtopHer".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.TellHerShoveDeeper".Localized());

            if (Player.Anal.Looseness < 4)
                OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.SureYoullGape".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.GapeEvenWider".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.SmirkPullIntoLap".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.LoverCommentsPiston".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.GrabsYouByHair".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.EyesRollBackExult".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.PassionSubsidesExtract".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.AWhileLaterLabel".Localized());

            OutputText("Scenes.Kihafollowerscene.LovingHugsForRetards.FliesBackToRecover".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void WarmLoverKihaIntro(bool output = true)
        {
            ClearOutput();

            Action campo = null;

            Action leave = () => ReturnToCamp(1);

            if (output)
            {
                KihaSprite();

                if (FollowerKiha())
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.KihaNeedsToReactToHorsecocking) == 1)
                    {
                        KihaReactsToHorseDicking();

                        return;
                    }

                    if (View.Time.Days % 15 == 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaPregnancyPotential) == 0)
                    {
                        KihasFirstClutch();

                        return;
                    }

                    OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.ApproachDragonessLover".Localized());

                    if (CanKihaGetPregnant())
                        OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.BellySwollenWithEggs".Localized());

                    if (Player.IsPregnant())
                    {
                        OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.ParagraphBreak".Localized());

                        if (Player.Pregnancy.Incubation > 288)
                            OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.BellySlightlySwollen".Localized());
                        else
                            if (Player.Pregnancy.Incubation > 240 && Player.Pregnancy.Incubation <= 288)
                                OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.SixMonthsPregnant".Localized());
                            else
                                if (Player.Pregnancy.Incubation > 192 && Player.Pregnancy.Incubation <= 240)
                                    OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.EightMonthsPregnant".Localized());
                                else
                                    if (Player.Pregnancy.Incubation > 144 && Player.Pregnancy.Incubation <= 192)
                                        OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.NineMonthsPregnant".Localized());
                                    else
                                        if (Player.Pregnancy.Incubation > 72 && Player.Pregnancy.Incubation <= 144)
                                            OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.BiggerThanFullTerm".Localized());
                                        else
                                            if (Player.Pregnancy.Incubation <= 72)
                                                OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.ImpossibleNotToNotice".Localized());
                    }

                    leave = () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());

                    ClearButtons();

                    AddButton(0, "Common.Button.Appearance".Localized(), KihaCampAppearance);

                    AddButton(1, "Common.Button.HangOut".Localized(), HangOutWithKiha);

                    AddButton(2, "Common.Button.Sex".Localized(), () => KihaSexMenu());

                    AddButton(3, "Common.Button.Spar".Localized(), SparWithKiha);

                    if (CampScene.IsGuard(FollowerIds.KihaLover))
                        AddButton(4, "Common.Button.StopGuarding".Localized(), GuardMyCampKiha);
                    else
                        AddButton(4, "Common.Button.GuardCamp".Localized(), GuardMyCampKiha);

                    if (CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) == 0 && (Player.HasItem(ItemIds.SilkPanties) || Player.HasItem(ItemIds.SilkLoincloth)))
                        AddButton(5, "Common.Button.GiveUnderwear".Localized(), GiveKihaUndergarmentsPrompt);

                    AddRowButton(1, "Hug", HugFriendWarmKiha);

                    AddRowButton(1, "Kiss", Kiss);

                    AddButton(9, "Common.Button.SleepWith".Localized(), SleepWith);

                    if (Save.GetInt("badCookingTasted", 0) == 2)
                        AddButton(10, "Common.Button.Cooking".Localized(), KihaCookingLesson);

                    if (!Save.GetBool("flowerGifted") && Player.HasItem(ItemRegistry.Create(ConsumableIds.DrakeHeart)))
                        AddRowButton(2, "DrakeHeart", KihaDrakeHeart);

                    if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !((Areas.Swamp.AlrauneScene.Save.GetInt("questAsked", 0) & Areas.Swamp.AlrauneScene.ASKEDAM) != 0))
                        AddRowButton(2, "Vines", null );

                    if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) > 0 && CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) < 100)
                        if (CoC.Instance.Flags.Get(GameFlagIds.KihaDestroyingCorruptedGlades) == 0)
                            AddRowButton(2, "Destroy Glades", KihaDestroyGladesToggle);
                        else
                            AddRowButton(2, "Stop Destroying", KihaDestroyGladesToggle);

                    AddButton(14, "Common.Button.Leave".Localized(), leave);

                    return;
                }
                else
                {
                    OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.SwampFamiliarTerritory".Localized());

                    OutputText("Scenes.Kihafollowerscene.WarmLoverKihaIntro.HeyDoofusGrin".Localized());
                }
            }

            if (!FollowerKiha() && CoC.Instance.Flags.Get(GameFlagIds.KihaMoveInOffer) == 1)
                campo = InviteKihaForDickings;

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), KihaCampAppearance);

            AddButton(1, "Common.Button.HangOut".Localized(), HangOutWithKiha);

            AddButton(2, "Common.Button.Hug".Localized(), HugFriendWarmKiha);

            AddButton(3, "Common.Button.Sex".Localized(), () => KihaSexMenu());

            AddButton(4, "Common.Button.Spar".Localized(), SparWithKiha);

            AddButton(5, "Common.Button.Invitecamp".Localized(), campo);

            AddButton(14, "Common.Button.Leave".Localized(), leave);
        }

        public void HangOutWithKiha()
        {
            ClearOutput();

            float select = Random.Shared.Next(3);

            if (select == 0)
            {
                ShowImage("kiha-follower-hangout-walkabout");

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.OfferArmWalkabout".Localized());

                if (!FollowerKiha())
                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.ThroughDenseOvergrowth".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.BackToOldHaunt".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SentencePeriod".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.TeaseAndToyWalk".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.StrollInterrupted".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.KihaLandsWithGreataxe".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.GrinAtHerBoasting".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.YouWord".Localized());

                if (Player.Tallness >= 72)
                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.BigWord".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.LittleWord".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.IdiotKissWhimper".Localized());

                if (!FollowerKiha())
                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.BackAtIsletWaving".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.WalkingArmInArm".Localized());

                OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SentencePeriodAgain".Localized());
            }
            else
                if (select == 1 && Save.GetInt("badCookingTasted", 0) < 3)
                {
                    ShowImage("kiha-follower-hangout-dinner");

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.FishingForIdea".Localized());

                    if (!Player.CanFly())
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.CarriedThroughRoof".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.HandExtendedFlyUp".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SentencePeriodThird".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SpartanNestDescription".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.RawMeatQuestion".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.KihaSitsStaring".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.AsksHowItTastes".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.ChewingMeatAnswer".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.ItsGood".Localized(), ItsGood);

                    AddButton(1, "Common.Button.Blech".Localized(), BlechKihaYourCooking);

                    return;
                }
                else
                {
                    ShowImage("kiha-follower-hangout-stargazing");

                    if (!FollowerKiha())
                    {
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.LookUpAtTreetops".Localized());

                        if (View.Time.Hours < 21)
                            OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.OvercastPinkClouds".Localized());
                        else
                            OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.StarFilledHeavens".Localized());

                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SentencePeriodFourth".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.NotesClearSky".Localized());

                        if (View.Time.Hours < 21)
                            OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.CampDayClouds".Localized());
                        else
                            OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.WastelandNightStars".Localized());

                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SentencePeriodFifth".Localized());
                    }

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.DragonessCloseAtHand".Localized());

                    if (View.Time.Hours < 21)
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.DayWord".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.NightWord".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.PlayfulRomanticHug".Localized());

                    if (View.Time.Hours < 21)
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.CloudsWord".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.StarsWord".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.FollowsPointingFinger".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.SnugglesAndRecalls".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.GrinThenGrounded".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.LongStareSmile".Localized());

                    if (Player.Tallness >= 72)
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.BigWordAgain".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.LittleWordAgain".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.IdiotLieThereLaugh".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.TimelessEmbracePart".Localized());

                    OutputText("Scenes.Kihafollowerscene.HangOutWithKiha.WrySmirkFarewell".Localized());
                }

            ReturnToCamp(1);

            KihaAffection(5);
        }

        public void ItsGood()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.ItsGood.WinkPraiseMeal".Localized());

            OutputText("Scenes.Kihafollowerscene.ItsGood.SharedLaughMeat".Localized());

            Player.RefillHunger(60);

            OutputText("Scenes.Kihafollowerscene.ItsGood.GreaseFingersPushed".Localized());

            KihaAffection(5);

            KihaSexMenu(false, false);
        }

        public void BlechKihaYourCooking()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.GagSpitBadMeat".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.KihaFollowsOutside".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.GrudginglyHauledOut".Localized());

            OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.GrabsGreataxeHunt".Localized());

            OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.LoverFollowsToBoar".Localized());

            OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.DragCarcassSkinning".Localized());

            OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.SnatchesMeatBite".Localized());

            OutputText("Scenes.Kihafollowerscene.BlechKihaYourCooking.EyeRollQuietMeal".Localized());

            Player.RefillHunger(60);

            KihaAffection(-10);

            if (Save.GetInt("badCookingTasted", 0) == 0)
                Save.SetInt("badCookingTasted", 1);

            ReturnToCamp(1);
        }

        public void KihaSexMenu(bool display = true, bool allowBack = true)
        {

            KihaSprite(true);

            if (display)
                OutputText("Scenes.Kihafollowerscene.KihaSexMenu.ParagraphBreak".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(94))
                AddButton(0, "Common.Button.Anal".Localized(), SavinTheAnalForKiha);
            else
                AddButton(0, "Common.Button.Anal".Localized(), null);

            if (!FollowerKiha())
                AddButton(1, "Common.Button.Dominate".Localized(), null);
            else
                if (!Player.IsGenderless())
                    AddButton(1, "Common.Button.Dominate".Localized(), DominateKihasFaceWithStuffAndStuffOrSomethingIDunnoWhyImStillWritingThis);
                else
                    AddButton(1, "Common.Button.Dominate".Localized(), null);

            if (!FollowerKiha())
                AddButton(2, "Common.Button.Fuckvag".Localized(), null);
            else
                if (Player.HasCockThatFits(67))
                    AddButton(2, "Common.Button.Fuckvag".Localized(), FuckKihasVagInCamp);
                else
                    AddButton(2, "Common.Button.Fuckvag".Localized(), null);

            if (Player.BiggestCockArea() >= 150)
                AddButton(3, "Common.Button.GetHj".Localized(), KihaPlaysWithBigassCocksFemDomAhoy);
            else
                AddButton(3, "Common.Button.GetHj".Localized(), null);

            if (Player.HasVagina())
                AddButton(4, "Common.Button.Girl69".Localized(), KihaGirlGirlSex);
            else
                AddButton(4, "Common.Button.Girl69".Localized(), null);

            if (FollowerKiha())
            {
                AddButton(5, "Common.Button.Cuddlefinger".Localized(), KihaCuddleFinger);

                AddButton(6, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), KihaTribbing);
            }
            else
                AddButton(7, "Common.Button.GroTits".Localized(), null);

            if (FollowerKiha())
            {
                if (Player.HasItem(ItemIds.GroPlus))
                    AddButton(7, "Common.Button.GroTits".Localized(), GhostboobiesKiha);
                else
                    AddButton(7, "Common.Button.GroTits".Localized(), null);
            }
            else
                AddButton(8, "Common.Button.GiveIDraft".Localized(), null);

            if (FollowerKiha())
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                    AddButton(8, "Common.Button.GiveIDraft".Localized(), GiveKihaIncubusDraft);
                else
                    AddButton(8, "Common.Button.GiveIDraft".Localized(), null);
            }
            else
                AddButton(9, "Common.Button.Lustydicking".Localized(), null);

            if (FollowerKiha())
            {
                if (Player.CockThatFits(200) < 0 || Player.CockArea(Player.CockThatFits(200)) < 40)
                    AddButton(9, "Common.Button.Lustydicking".Localized(), null);
                else
                    if (Player.MinLibido < 50 && Player.Libido < 80 && !Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                        AddButton(9, "Common.Button.Lustydicking".Localized(), null);
                    else
                        AddButton(9, "Common.Button.Lustydicking".Localized(), BoneTheShitOutofKihaHolesWithHorsecock);
            }
            else
                AddButton(10, "Common.Button.Tentaclefuck".Localized(), null);

            if (FollowerKiha())
                if (Player.CountCocksOfType("TENTACLE") > 1)
                    AddButton(10, "Common.Button.Tentaclefuck".Localized(), FuckKihaWithATentacle);
                else
                    AddButton(10, "Common.Button.Tentaclefuck".Localized(), null);

            AddButton(14, (allowBack ? "Common.Text.Back".Localized() : "Common.Text.Leave".Localized()), (allowBack ? SceneInstance<KihaScene>.Get().EncounterKiha : () => ReturnToCamp(1)));
        }

        public void BoneTheShitOutofKihaHolesWithHorsecock()
        {
            ClearOutput();

            ShowImage("kiha-follower-sex-lustyhorsecock");

            var c = Player.CumQ();

            int x = -1;

            int y = -1;

            if (Player.CountCocksOfType("Horse") > 0)
            {
                var temp = Player.CockTotal();

                while (temp > 0)
                {
                    temp--;

                    if (Player.Cocks[temp].Id == BodyPartTypes.Horse && Player.CockArea(temp) >= 40)
                    {
                        x = temp;

                        y = x + 1;

                        break;
                    }
                }
            }

            if (x == -1)
            {
                x = Player.CockThatFits(200);

                y = x + 1;
            }

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.StripArmorApproach".Localized());

            if (Player.MinLibido < 50)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HalfTurgidAdj".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AlwaysSemiHardAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LengthOfYourFrag".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FatHorsecockPhrase".Localized());
            else
                OutputText(Player.CockDescript(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockFlopBreeze".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SLetter".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BeneathEquineBody".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.KihaStuntScold".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.KihaKneelsDown".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CrawlUnderHorse".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ExamineCloserLead".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BestialEquineDong".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockPlaceholderToken".Localized(
                    y
                ));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlowlyFillingLead".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlaredAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CreamyDollopTip".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LipsPartInquiry".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlareMesaPhrase".Localized());
            else
                OutputText(Player.CockHead(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SheOpensWide".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PumpAboveSheath".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ShePumpsShaft".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlowEvenStrokes".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SheathSkinCaress".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BallsFondleDesire".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TaintFondleDesire".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SupernaturalSensitivity".Localized());

            if (Player.Intelligence < 20)
            {
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.UnthinkingLead".Localized());

                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BestialWhinnyPhrase".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.GroanWord".Localized());

                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PleasureBurstsLips".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HoovesStampingLead".Localized());

                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ConcernEvaporates".Localized());

                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AnimalWord".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WellWord".Localized());

                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EndowedFormSuffix".Localized());
            }
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AWord".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse || Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WhinnyWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.GroanWordAgain".Localized());

            var horse = false;

            if (Player.Cocks[x].Id == BodyPartTypes.Horse || Player.IsTaur())
                horse = true;

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ThroatStartLead".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CatchBeforeBraying".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CatchBeforeEscape".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EmbarrassmentVersusPleasure".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AnimalWordAgain".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WellWordAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EndowedFormForgetFrag".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MirthfulOrbsWatch".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BeastPrefix".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PrickTheLead".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ThickFlarePhrase".Localized());
            else
                OutputText(Player.CockHead(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SatisfyingChallenge".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AnimalWordSpaced".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EndowmentIntoMouth".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ReptilianBobsHalting".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SheathCaressWarmth".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TongueWorksShaft".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BallsCaressWarmth".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TrifleIrritatedGaze".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BigPonyPrick".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockPlaceholderPeriod".Localized(
                    y
                ));

            if (Player.Balls > 0)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BallsSmearChin".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ParagraphKihaLead".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SqueezesSackLead".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FondlesVagLead".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TongueEncouragingTwist".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EndureNoLonger".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlareWord".Localized());
            else
                OutputText(Player.CockHead(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.IncredulousHungryLook".Localized());

            if (c <= 250)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.GuzzleItAllDown".Localized());
            else
                if (c <= 1000)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EyesWaterCumTrickles".Localized());
                else
                {
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SemenNostrilBackflood".Localized());

                    if (Player.Cocks[x].TypeIndex > 0 && Player.Cocks[x].TypeIndex < 10)
                        OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AnimalWordSpacedAgain".Localized());

                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ForcedSpermSwallow".Localized());
                }

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PullsBackReleasesTip".Localized());

            if (Player.MinLibido < 50 && Player.Libido < 80)
            {
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LustDraftPotionGrab".Localized());

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
            }

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ParagraphKihaLeadAgain".Localized());

            if (horse)
            {
                if (Player.IsTaur())
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ClambersOntoRock".Localized());
            }
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LeansBackSpreads".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PrecumTrailsTip".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HorsePrefix".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WondrousCockAroma".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MareWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlutWord".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SeedMarkedSleeve".Localized());

            if (Player.Cocks[x].TypeIndex > 0)
            {
                if (Player.IsTaur())
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HoovesOntoRock".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FondleBestialTool".Localized());
            }
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TaurConditionalBlock".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BobbingFlingsPrecum".Localized(
                Player.CockDescript(x)
            ));

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AmIRuttingSay".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WhyAmISay".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WonderSidetrackedMoan".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.KihaOddLook".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlatAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockheadUnpreparedCunt".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HorsePrefixAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TrivialitiesDismissed".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PunchesUnderside".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TriesPushBack".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ShoutsAreYouListening".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.GrowlingPopsOut".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MareWordAgain".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlutWordAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlapFaceWithCock".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SavageShaftPhrase".Localized());
            else
                OutputText(Player.CockDescript(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ForceCockScentWhiff".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HorsePrefixThird".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MeatSlapsCheeks".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.KihaBabblesOblige".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.StallionShaftHead".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.OfYourTemplate".Localized(
                    Player.CockHead(x),

                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SpearKihaEntrance".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WellLubedHorsecock".Localized());
            else
                OutputText(Player.CockDescript(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.IntoDilatedPussy".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MammalianAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlurpsInchAfterInch".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PussyJuiceDribbles".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlatTippedHorsecock".Localized());
            else
                OutputText(Player.CockDescript(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BastesWombPrecum".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MareWordSpaced".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlutWordSpaced".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SteamingHotBody".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SavageThrustPopsTip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaCervixginityTaken) == 0)
            {
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.StealWombCherry".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaCervixginityTaken, CoC.Instance.Flags.Get(GameFlagIds.KihaCervixginityTaken) + 1);
            }
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PlowFavoriteDragonhole".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WincesShakesHead".Localized());

            if (Player.HasBalls())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlapBallsIntoAss".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlapHardTailThrash".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LustLostThrashes".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EquineAdj".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ThickAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AchingTightHotness".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlaringAdj".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EngorgingAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SealsInsideBabyhole".Localized());

            if (c <= 250)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.StuffedHotCum".Localized());
            else
                if (c <= 1500)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BellyPregnantProportions".Localized());
                else
                    if (c <= 3000)
                        OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BellyPregnantEquine".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BellyDistendsCumSquirt".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SentencePeriod".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SighingPullsOut".Localized());

            if (Player.Cocks[x].TypeIndex > 0)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AnimalWordHyphen".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SampleOwnCumFlavor".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ShaftBumpsBelly".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.JustStartedBreeding".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ParagraphKihaPossessive".Localized());

            if (c > 250)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CumGravidAdj".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SpunkStuffedAdj".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FormEasyToMove".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PushOntoAllFours".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockBetweenAsscheeks".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ShovesYourLead".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlareWordAgain".Localized());
            else
                OutputText(Player.CockHead(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TightAnalOpening".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MareWordThird".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlutWordThird".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CunnyRetryFails".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.UniqueCockheadShape".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SheerCockheadSize".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BattersPuckerCircles".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlareWordThird".Localized());
            else
                OutputText(Player.CockHead(x));

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WorksAssholeGives".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.GrabsKihaTail".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LimpTailProvides".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HandholdLevelsTool".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HorsePrefixFourth".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.PopsThroughSphincter".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HandOverRump".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.YLetter".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ForceRestOfCock".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AnimalWordHyphenAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockIntoButthole".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SensitiveSheathLead".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SensitiveLoinsLead".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.RealizeTooBig".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CriesPantsInDirt".Localized());

            if (c < 250)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SeeEquineShape".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AlphaOfCourseBig".Localized(
                Player.Mf("stud", "breeder")
            ));

            if (Player.IsTaur())
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LeansDownLead".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SmacksRearService".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MoansSemenFingertips".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BallsPlaceholderToken".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SheathWord".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.EncouragingSqueezes".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.MareWordSpacedAgain".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SlutWordSpacedAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ArchesBackSerpentine".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CommandsBegLead".Localized());

            if (horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HerdAlphaAuthority".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.UnquestionableAuthority".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TwistsHeadHairPull".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlareWordSpaced".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SpreadingAtApex".Localized());

            if (c >= 3000)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TorrentFromHoles".Localized());
            else
                if (c >= 1000)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WashFromHoles".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.TricklesFromHoles".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.RollsSideCradlingBelly".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CircleLowersToFace".Localized(
                Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.BallsPlaceholderAgain".Localized());
            else
                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SheathWordAgain".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.CockWord".Localized());

            OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.SighsReachesUp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaHorsecockFucked) == 0)
            {
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AwayFromScentRealize".Localized());

                if (Player.Corruption <= 33)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.AndWinceLead".Localized());

                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WondersLustCrazy".Localized());

                if (Player.Libido < 80 && Player.MinLibido < 50)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LustDraftQuestion".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.WasItPrompt".Localized());

                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.KihaBodyOrYours".Localized());

                if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                    OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.ExgartuanSuggests".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaNeedsToReactToHorsecocking, 1);
            }
            else
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.HappenedAgainShudder".Localized());

            if (Player.MinLibido < 50 && Player.Libido < 80)
                OutputText("Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.LustDraftUsedUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaHorsecockFucked, CoC.Instance.Flags.Get(GameFlagIds.KihaHorsecockFucked) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void KihaReactsToHorseDicking()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.KihaFoldsArmsColdly".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.PushBackAskMeaning".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.HugHerAndLaugh".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.KihaYellsPullBack".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.AskRepeatPossibility".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.UnsureHopefulScared".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaReactsToHorseDicking.AnythingWithKihaQuestion".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaNeedsToReactToHorsecocking, 2);

            WarmLoverKihaIntro(false);
        }

        public void KihaPlaysWithBigassCocksFemDomAhoy()
        {
            ClearOutput();

            ShowImage("kiha-follower-sex-bigcock");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.PlayfulApproachHug".Localized());

            if (Player.Tallness >= 84)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.UpWord".Localized());
            else
                if (Player.Tallness <= 60)
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.DownWord".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.FeignedIndifferenceGlance".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length > 48)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.TailClimbsHigher".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.ThickensAgainstHer".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.ElephantInRoom".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.KihaNipsAttention".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SmilingInformPartner".Localized());

            if (Player.Cocks[x].Length > 48)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.BiggerThanPhrase".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SizeOfPhrase".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.KihaGigglesSurprise".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.LeanBackAgainst".Localized());

            if (FollowerKiha())
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.RockWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.TreeWord".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SighContentment".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Human)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.MedialRingFlare".Localized());
            else
                if (Player.Cocks[x].Id == BodyPartTypes.Demon)
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.DemonicNodules".Localized());
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                        OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.AphrodisiacTentacles".Localized());
                    else
                        if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.TentacleMushroomTip".Localized());
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.RubberySpines".Localized());
                            else
                                if (Player.HasKnot(x))
                                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.MassiveBulgingKnot".Localized(
                                        x + 1
                                    ));
                                else
                                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SentencePeriod".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.WorldSpinsPleasure".Localized());

            if (Player.Cocks[x].Length > 48)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.PullsCockheadDown".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.LongReptilianTongue".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.AffectionBattersLegs".Localized(
                Player.Mf("male", "specimen")
            ));

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.KihaTwistsTail".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.BeneathYourKnot".Localized());
            else
            {
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.AroundYourBase".Localized());

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.JustAboveSheath".Localized());
            }

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.ImpromptuCockRing".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.NodOverwhelmed".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.KihaInterlocksFingers".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.BallsClenchRelax".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.YLetter".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.DickReadyToBurst".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.KihaTimelyPraise".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.BukkakeSheerVolume".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SpunkSplashesYou".Localized());

            if (Player.CumQ() >= 2000)
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.WavesOfSpunkRain".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.KihaBrushesJism".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SlumpDopeyHug".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
                if (Player.Corruption < 50)
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.HopeNotJumped".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.WonderIfJumped".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.WipeUpDonArmor".Localized());

            if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                if (!SceneInstance<JojoScene>.Get().TentacleJojo())
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.IgnoreJojoMasturbating".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.IgnoreJojoTentacles".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SentencePeriodAgain".Localized());

            if (SceneInstance<SophieBimboScene>.Get().BimboSophie())
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.SophieCrestfallenGreeting".Localized());

            if (SceneInstance<AmilyScene>.Get().AmilyCorrupt())
                OutputText("Scenes.Kihafollowerscene.KihaPlaysWithBigassCocksFemDomAhoy.AmilyBegsService".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void KihaGirlGirlSex()
        {
            ClearOutput();

            ShowImage("kiha-follower-sex-sixtynine");

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.PullKihaIntoArms".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.KihaPresentsChest".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.ScalyTailYank".Localized());

            if (Player.Wetness() < 4)
                OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.TricklesWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.SmallRiversPhrase".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.LubricantDownLegs".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.DragonessFlipsSmother".Localized());

            if (Player.GetClitLength() >= 10)
                OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.GaspRidiculousSize".Localized());
            else
                if (Player.GetClitLength() >= 3)
                    OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.HumCocklikeLength".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.HumStiffensAgainst".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.GrumblesHandsMove".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.KihaMuttersResumes".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.ClitSwellsWarning".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.TrickleWord".Localized());
            else
                if (Player.Wetness() <= 3)
                    OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.SmallGushPhrase".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.RiverWord".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.ExplosionWord".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.LadySpoogeFace".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGirlGirlSex.LappingGirlHoney".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void KihaCuddleFinger()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.EmbracePassionateKiss".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.LowerAndEmbrace".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.TasteStirsLust".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.EncouragedSlipFinger".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.SeparatesMouthHand".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.LovedItAdmission".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.GiggleForeheadPeck".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCuddleFinger.WakingLaterExit".Localized());

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void KihaTribbing()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.PlantFirmKiss".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.SmirkStraddleKiha".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.BreakKissBegMore".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.RhythmMorePressure".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.SlowToCompleteStop".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.BoopNosePraise".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTribbing.CuddleThenDepart".Localized());

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void SavinTheAnalForKiha()
        {
            ClearOutput();

            ShowImage("kiha-follower-sex-buttfuck");

            int x = Player.CockThatFits(94);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.GrinAtDragoness".Localized());

            if (Player.Tallness >= 60)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.HerShoulders".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.SmallOfBack".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.GrabbingLushCheeks".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.DragonessGasps".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaJeers".Localized());

            if (Player.Cocks[x].Length < 12)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.BellyWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.ChestWord".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.AssplayTaunt".Localized());

            if (Player.Tallness >= 60)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.BigWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.LittleWord".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.PervertRetort".Localized());

            if (Player.Cocks[x].Length < 3)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.PullCheeksApart".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.HipsInTandem".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.JustAsYours".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.SentencePeriod".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaThrustsBack".Localized());

            if (!FollowerKiha())
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.TreeWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.RockWord".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaStraddlesYou".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.EagerNod".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaSpits".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.GroanInNeed".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.FootOffDick".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.AlongLongTail".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.BetweenYour".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.SlowSensualAdvance".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.MmmNotBad".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.SpreadsCheeksWide".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.SuspendsPrick".Localized());

            if (Player.BiggestBreastSize() >= 3)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.PalmsCock".Localized(
                    Appearance.BiggestBreastSizeDescript(Player)
                ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.ChestToChest".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.HugKihaClose".Localized());

            if (Player.Cocks[x].Length >= 40)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.BottomsOut".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.CheeksOnCrotch".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.BothGasping".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesKihaAnaled) >= 5)
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.GrowingPleased".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.UnusedToAnal".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.AlreadySensitive".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.MoveTogether".Localized(
                Player.CockDescript(x)
            ));

            if (FollowerKiha())
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.RockWordAgain".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.TreeWordAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.MashingFaceTits".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaBitesLip".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.InsertionNearSets".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaScreamsCums".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.CumDemand".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.LoverOrgasmCry".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.KihaSitsInLap".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.LovingKiss".Localized());

            OutputText("Scenes.Kihafollowerscene.SavinTheAnalForKiha.PlayfulSwat".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.TimesKihaAnaled, CoC.Instance.Flags.Get(GameFlagIds.TimesKihaAnaled) + 1);

            ReturnToCamp(1);
        }

        public void FuckKihasVagInCamp()
        {
            ClearOutput();

            ShowImage("kiha-follower-sex-vagfuck");

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.GrabKihaHand".Localized());

            if (silly)
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.HighPitchWhine".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.NervousStutter".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.PinHerHand".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.BrushCopperLock".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.KihaPeelsArmor".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.CloseDistance".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.RubOuterLips".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.DraconicKiss".Localized());

            if (!Player.HasScales())
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.RedScratches".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.OldScaleDislodge".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.HoldLipLock".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.BreathyWhine".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.EarNibbleWhisper".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.JackhammerNod".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.BellyGlazedWhite".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.KihaPossessive".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.PussyFilledUp".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.CumRopeVolley".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.JizzPumpsOut".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.DrippingPuddle".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.LanguidSnuggle".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihasVagInCamp.ChestPunch".Localized());

            KihaKnockUpAttempt();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void GiveKihaIncubusDraft()
        {
            ClearOutput();

            ShowImage("kiha-follower-sex-incubidraft");

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.WickedIdea".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.GreataxeStagger".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.DazedPushBack".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.PumpHerCock".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.SquirmingWhine".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.FasterJacking".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.RingKissesTip".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.TightWord".Localized());
            else
                if (Player.Anal.Looseness < 4)
                    OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.WelcomingWord".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.LooseWord".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.PassageGroan".Localized());

            Player.ButtChange(14, true, true, false);

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.OrgasmHalt".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.GrantRequest".Localized());

            if (silly)
                OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.YourGameRemind".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.StopEveryTime".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.LightestTouchShiver".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.SheCums".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.RoaringThrustUp".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaIncubusDraft.SpunkCovered".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, 30f), new DynStat(GameStats.Corruption, .5f));

            Player.Orgasm("Anal");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.IncubiDraft));

                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));
            }

            ReturnToCamp(1);
        }

        public void FuckKihaWithATentacle()
        {
            ClearOutput();

            int x = -1;

            int y = -1;

            int z = -1;

            int zz = -1;

            var temp = Player.CockTotal();

            while (temp > 0)
            {
                temp--;

                if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle"))
                    if (x == -1)
                    {
                        x = temp;

                        x += 1;
                    }
                    else
                        if (y == -1)
                        {
                            y = temp;

                            y += 1;
                        }
                        else
                            if (z == -1)
                            {
                                z = temp;

                                z += 1;
                            }
                            else
                                if (zz == -1)
                                {
                                    zz = temp;

                                    zz += 1;
                                }
                                else
                                    break;
            }

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.SmileUndressYourself".Localized());

            if (silly)
                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.WhisperSlowly".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.ShakeHeadGrinning".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 1)
                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.MonstrousThingsPlural".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.MonstrousThingSingular".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.FuckingDareYou".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.PatientExplainSighing".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.KihaDeniesArousal".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.ReptilianGirlStanding".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.KihaOverwhelmedDicks".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.WrapCockComplaintsFade".Localized(
                x
            ));

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.ScalyWomanSurprised".Localized(
                x
            ));

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.HeavenlyPullOut".Localized(
                x
            ));

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.KihaQuestionsPetting".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.SilenceWithKiss".Localized(
                x
            ));

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.LoveYouMurmur".Localized(
                x,

                x,

                x
            ));

            if (y != -1)
            {
                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.KihaBlissfulFaceStare".Localized(
                    y,

                    y
                ));

                if (Player.CockArea(y - 1) < 24)
                    OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.RodBuriedHilt".Localized());

                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.PoundFrontBack".Localized(
                    y
                ));

                if (z != -1)
                {
                    OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.AppreciateWrithingVision".Localized(
                        z,

                        z,

                        x,

                        y
                    ));

                    OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.AcceptNeedyInvitation".Localized(
                        z,

                        z
                    ));

                    if (zz != -1)
                        OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.KihaStuffedTentacles".Localized(
                            zz,

                            z
                        ));
                }
            }

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.DragonRecessesTooMuch".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.ChurnAndQuiver".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.ChurnsAndQuivers".Localized());

            OutputText("Scenes.Kihafollowerscene.FuckKihaWithATentacle.SemenGatheringBurst".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void KihaOffersToMoveIn()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaOffersToMoveIn.SwampEncounterKiha".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaOffersToMoveIn.SmileNervousQuery".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaOffersToMoveIn.PunchBackRetort".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaOffersToMoveIn.KihaClawsGround".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaOffersToMoveIn.HugComfortDragoness".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaOffersToMoveIn.SheMightMoveIn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaMoveInOffer, 1);

            WarmLoverKihaIntro(false);
        }

        public void InviteKihaForDickings()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.LeanTreeInvite".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.NodTakeHands".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.ReptilianCoughsNervously".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.LightHeartedRetort".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.KihaPalesTears".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.SorrySeeCamp".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.SnifflesFavorRequest".Localized());

            OutputText("Scenes.Kihafollowerscene.InviteKihaForDickings.KihaJoinedCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaFollower, 1);

            ReturnToCamp(1);
        }

        public void GhostboobiesKiha()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.SyringeGazeFlit".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.ExplainGroPlus".Localized());

            if (!Player.HasPerk(PerkIds.Incorporeality))
            {
                OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.ShakeHeadShuffleAway".Localized());

                Go<CampScene>();

                return;
            }

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.SpookyPowersControl".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.PuppetLimbsScream".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.NeedleSinksShiver".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.ProtestsFadeDelight".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.SilentBoobSqueeze".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.AgainstWillNods".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.MutteredBoobRequest".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.HardlyThinkTransfer".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.SlowDrawOut".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.HeavyBowArching".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.RolledOntoBelly".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.OrgasmicTremorGush".Localized());

            OutputText("Scenes.Kihafollowerscene.GhostboobiesKiha.LustSpentCuriositySated".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            Player.ConsumeItem(ItemIds.GroPlus);

            ReturnToCamp(1);
        }

        public void KihaBitchesOutCorruptPCs()
        {
            ClearOutput();

            KihaSprite();

            if (!FollowerKiha())
            {
                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.SwampDragonessThoughts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KihaAffectionLevel) == 1)
                    OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.FriendWord".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.LoverWord".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.TerritoryLustyMind".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.CockToken".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.VagToken".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.LustWord".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.ArousalSpreadingThought".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.KihaEmergesTree".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.FlatlyPlantsAxe".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.WaryHelloWeapon".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.KihaFriendsWarning".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), SceneInstance<KihaScene>.Get().MeetKihaAndFight);

                SetExitButton("Common.Button.Back".Localized());
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.BelongingsGatheredUpset".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaBitchesOutCorruptPCs.KihaLeapsAway".Localized());

                Go<CampScene>();
            }

            CoC.Instance.Flags.Set(GameFlagIds.KihaCorruptionBitch, 1);
        }

        public void LeaveKihaAfterCorruptionBitch()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.LeaveKihaAfterCorruptionBitch.SlumpAvoidConfrontation".Localized());

            ReturnToCamp(1);
        }

        public void KihaUnbitchesUncorruptedFolks()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaUnbitchesUncorruptedFolks.ReturnToTerritory".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaUnbitchesUncorruptedFolks.KihaPlummetsSky".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaUnbitchesUncorruptedFolks.ExplainPurgedCorruption".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaUnbitchesUncorruptedFolks.OhKihaWord".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaCorruptionBitch, 2);

            if (!FollowerKiha())
                KihaFriendlyGreeting(false);
            else
            {
                OutputText("Scenes.Kihafollowerscene.KihaUnbitchesUncorruptedFolks.CampArmInArm".Localized());

                ReturnToCamp(1);
            }
        }

        public void KihaCampAppearance()
        {
            ClearOutput();

            ShowImage("monster-kiha");

            OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.KihaHeightDescription".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) > 0 ? "wearing " + (CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) == 1 ? "Items.SilkPanties.Long".Localized() : "Items.SilkLoincloth.Long".Localized()) : "naked, shameless of her nudity,"
            ));

            OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.DuskyTitsDescription".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.LooseTwatDescription".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) > 0 ? "Underneath her " + (CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) == 0 ? "Common.Text.SpiderSilkPanties".Localized() : "Common.Text.SpiderSilkLoincloth".Localized()) + ", " : ""
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) > 0)
                OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.Panties".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.ThighsWord".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.SentencePeriod".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCampAppearance.ButtCheeksAsshole".Localized());

            SceneInstance<KihaScene>.Get().EncounterKiha();
        }

        public void DominateKihasFaceWithStuffAndStuffOrSomethingIDunnoWhyImStillWritingThis()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.DominateKihasFaceWithStuffAndStuffOrSomethingIDunnoWhyImStillWritingThis.DominanceStaleIdea".Localized());

            OutputText("Scenes.Kihafollowerscene.DominateKihasFaceWithStuffAndStuffOrSomethingIDunnoWhyImStillWritingThis.FoldedArmsRefusal".Localized());

            OutputText("Scenes.Kihafollowerscene.DominateKihasFaceWithStuffAndStuffOrSomethingIDunnoWhyImStillWritingThis.KihaEarnIt".Localized());

            OutputText("Scenes.Kihafollowerscene.DominateKihasFaceWithStuffAndStuffOrSomethingIDunnoWhyImStillWritingThis.ChallengeBackDown".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.BackDown".Localized(), BeABitchDumbass);

            AddButton(1, "Common.Button.Fightfordom".Localized(), FightForDominanceWithDragonCunnies);
        }

        public void BeABitchDumbass()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.BeABitchDumbass.LegsSpreadTaunt".Localized());

            OutputText("Scenes.Kihafollowerscene.BeABitchDumbass.TauntGrowBalls".Localized());

            ReturnToCamp(1);
        }

        public void FightForDominanceWithDragonCunnies()
        {
            ClearOutput();

            KihaSprite(true);

            OutputText("Scenes.Kihafollowerscene.FightForDominanceWithDragonCunnies.DoubtHurtingToday".Localized());

            OutputText("Scenes.Kihafollowerscene.FightForDominanceWithDragonCunnies.GrinRaisesAxe".Localized());

            var me = EnemyRegistry.Create(new Identifier("coc", "kiha"));

            me.OnDefeated = (hpVictory) => PcWinsDomFight(hpVictory);

            me.OnWon = (hpVictory) => PcLosesDomFight(hpVictory);

            CombatScene.Start(me, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void PcLosesDomFight(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            ShowImage("kiha-dom-loss");

            OutputText("Scenes.Kihafollowerscene.PcLosesDomFight.KihaVictoryTaunt".Localized());

            OutputText("Scenes.Kihafollowerscene.PcLosesDomFight.AwakeBedspreadTended".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void PcWinsDomFight(bool hpVictory)
        {
            ClearOutput();

            ShowImage("kiha-dom-win");

            KihaSprite(true);

            int x = Player.CockThatFits(67);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.DragonessFallsDefeated".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.PullHardeningCock".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ReleaseLatheredCock".Localized(
                        Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.TeaseKihaDrawers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChokedOutPc) > 0)
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaProtestsAgain".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaCooingTaunt".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaThighsWrapYou".Localized());

            if (Player.HasCock())
                OutputText(Player.HipDescript());
            else
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.FaceWord".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.DragonessDraws".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.YouWord".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.YourTongue".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.PastLoveButtonInto".Localized());

            if (Player.HasCock())
                OutputText(Player.CockDescript(x));
            else
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.MouthMuscle".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaShameTaunt".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.HandsIntoGaps".Localized());

            var choices = new List<int>();

            if (Player.HasCock())
                choices.Add(0);

            if (Player.HasVagina())
                choices.Add(1);

            choices.Add(2);

            float select = choices[Random.Shared.Next(choices.Count)];

            if (select == 0 || !Player.HasVagina())
            {
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.PlowBackIntoKiha".Localized());

                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.CumFaceTaunt".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.WombBreedTaunt".Localized());
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ScissorGrindClits".Localized());

                if (Player.HasCock() && Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.WombBreedTauntAgain".Localized());
                else
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.CumFaceTauntAgain".Localized());
            }

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SubduedBrutalCoitus".Localized());

            if (select == 0 || !Player.HasVagina())
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.LoveCanal".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ClitWord".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaGoadingTaunt".Localized());

            if (Player.Gender != Gender.Female && Player.Balls == 0)
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.UnsureInsultChallenge".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SeizeScarletHair".Localized());

            if (select == 0 || !Player.HasVagina())
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.TugLengthsHeadBack".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.TwistClitoralHood".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.MockingYellSweep".Localized());

            if (select == 0)
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaMountsPelvis".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (select == 1)
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.LegsWrapHeadProne".Localized(
                        Player.Mf("big boy", "you delectable slut")
                    ));
                else
                {
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SeizesLegsUpward".Localized());

                    if (Player.HasCock())
                        OutputText(Player.CockDescript(x));
                    else
                        OutputText(Player.ClitDescript());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.NewFavoritePosition".Localized());

                    if (!Player.HasCock())
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ViciousLappingCunt".Localized());
                    else
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.RamCockDownCraw".Localized());
                }

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.MusteringStrength".Localized());

            if (select <= 1)
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SpareNeckFlesh".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SpareGenitalia".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.FreedomChanceArrives".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SurveyForDragoness".Localized());

            choices = [];

            if (Player.HasCock())
            {
                choices.Add(0);

                choices.Add(1);
            }

            if (Player.HasVagina())
                choices.Add(2);

            choices.Add(2);

            select = choices[Random.Shared.Next(choices.Count)];

            if (select == 0)
            {
                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.TailWrapsMember".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SLetter".Localized());

                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ScalyTugToMistress".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.RefuseToSubmitMilk".Localized());

                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.BrushTeaseKiha".Localized(
                    Player.CockDescript(x),

                    Player.CockHead(x)
                ));

                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaBegsFrustration".Localized());

                KihaKnockUpAttempt();
            }
            else
                if (select == 1)
                {
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ShinSlamKneeBack".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SLetterAgain".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.DownGazeRealize".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SLetterThird".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ClawedFootGasp".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SLetterFourth".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.StareUpTriumph".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.KihaGetsDominated".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.FistsSmashHips".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SLetterFifth".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.GracefulLegSweep".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.PinArmsSuckleBreast".Localized());

                    if (Player.HasLongTongue() || Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                    {
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.TitillationViolation".Localized());

                        if (Player.HasLongTongue())
                            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.LatchedToMammary".Localized());
                        else
                            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ForkedTongueHiss".Localized());
                    }

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ArousalOverloadYield".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.MissionaryLegsClasp".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ReleaseImminentSqueal".Localized());

                    KihaKnockUpAttempt();
                }
                else
                {
                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.TailThroatDrag".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.BuyTimeGrudging".Localized());

                    OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.AnkleYankEscape".Localized());

                    if ((Player.HasCock() && Random.Shared.Next(2) != 0) || !Player.HasVagina())
                    {
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.FacialFinish".Localized());

                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.LoverStayStill".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SLetterSixth".Localized());

                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.InchesFromFace".Localized(
                            Player.CockHead(x),

                            Player.CockDescript(x)
                        ));

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.SatisfiedSubmission".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.GaggingSwallow".Localized());
                            else
                                OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.CheeksBulgeOverload".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.ClimaxFinish".Localized());

                        OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.HelplessSurrender".Localized(
                            Player.Mf("manly", "girlish little")
                        ));
                    }
                }

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.EnsureCooperation".Localized());

            OutputText("Scenes.Kihafollowerscene.PcWinsDomFight.NodsDominanceTalk".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void KihaDestroyGladesToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaDestroyingCorruptedGlades) == 0)
            {
                OutputText("Scenes.Kihafollowerscene.KihaDestroyGladesToggle.AskKihaCleanseGlades".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaDestroyGladesToggle.KihaGrinsFirePuff".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaDestroyingCorruptedGlades, 1);
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.KihaDestroyGladesToggle.TellKihaSpareGlades".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaDestroyGladesToggle.KihaWarnsGladesReturn".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaDestroyingCorruptedGlades, 0);
            }

            WarmLoverKihaIntro();
        }

        public void GuardMyCampKiha()
        {
            ClearOutput();

            if (CampScene.IsGuard(FollowerIds.KihaLover))
            {
                CoC.Instance.Flags.Set(GameFlagIds.KihaCampWatch, 0);

                OutputText("Scenes.Kihafollowerscene.GuardMyCampKiha.TellKihaStopGuarding".Localized());

                OutputText("Scenes.Kihafollowerscene.GuardMyCampKiha.KihaImpCockWarning".Localized());
            }
            else
            {
                OutputText("Scenes.Kihafollowerscene.GuardMyCampKiha.AskKihaNightWatch".Localized());

                OutputText("Scenes.Kihafollowerscene.GuardMyCampKiha.KihaAgreesWatch".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.KihaCampWatch, 1);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => WarmLoverKihaIntro());
        }

        public void GiveKihaUndergarmentsPrompt()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarmentsPrompt.AskKihaCoverLoins".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarmentsPrompt.KihaReluctantComply".Localized());

            OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarmentsPrompt.GiftChoicePrompt".Localized());

            ClearButtons();

            if (Player.HasItem(ItemIds.SilkPanties))
                AddButton(0, ItemRegistry.Create(ItemIds.SilkPanties).ShortNameKey.Localized(), () => GiveKihaUndergarments(1));

            if (Player.HasItem(ItemIds.SilkLoincloth))
                AddButton(1, ItemRegistry.Create(ItemIds.SilkLoincloth).ShortNameKey.Localized(), () => GiveKihaUndergarments(2));

            AddButton(4, "Scenes.CorruptedGlade.NeverMind".Localized(), () => WarmLoverKihaIntro());
        }

        public void GiveKihaUndergarments(int type)
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarments.TellKihaHaveGift".Localized());

            if (type == 1)
            {
                OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarments.ProduceSilkPanties".Localized());

                OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarments.PraisePantiesFit".Localized());

                Player.ConsumeItem(ItemIds.SilkPanties, 1);
            }

            if (type == 2)
            {
                OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarments.ProduceSilkLoincloth".Localized());

                OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarments.PraiseLoinclothFit".Localized());

                Player.ConsumeItem(ItemIds.SilkLoincloth, 1);
            }

            OutputText("Scenes.Kihafollowerscene.GiveKihaUndergarments.KihaAsksHowLooks".Localized(
                Player.ClothedOrNakedLower("garbed", "naked")
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            CoC.Instance.Flags.Set(GameFlagIds.KihaUndergarments, type);

            WarmLoverKihaIntro();
        }

        public void KihasFirstClutch()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.ApproachHearMoaning".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.KihaPuzzledFine".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.HeardHerComplaining".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.RepeatsFlushOffended".Localized(
                Player.Mf("B-boyfriend", "G-girlfriend")
            ));

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.PointStomachScowl".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.AskHeatFrequency".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.KihaShrugsMonthly".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.SympathyThenAskFather".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.KihaLooksNervous".Localized());

            OutputText("Scenes.Kihafollowerscene.KihasFirstClutch.BoldFertileDays".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaPregnancyPotential, 1);

            WarmLoverKihaIntro();
        }

        public bool KihaPregUpdate()
        {

            switch (Player.PregnancyStore.EventTriggered())
            {
                case 1:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.BellyBulgeSubtle".Localized());

                return true;
                }

                case 2:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.BellySwollenRound".Localized());

                return true;
                }

                case 3:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.FieryCorrosiveVomit".Localized());

                return true;
                }

                case 4:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.SixMonthsPregnant".Localized());

                return true;
                }

                case 5:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.EightMonthsPregnant".Localized());

                return true;
                }

                case 6:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.NineMonthsBirthSoon".Localized());

                return true;
                }

                case 7:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.LargerThanExpected".Localized());

                return true;
                }

                case 8:
                {
                OutputText("Scenes.Kihafollowerscene.KihaPregUpdate.BarelyStandsFloats".Localized());

                return true;
                }
            }

            return false;
        }

        public void KihaGivesBirth()
        {

            var eggCounter = (Random.Shared.Next(5) + 1) * 2;

            eggCounter += Player.VirilityQ() / 10;

            if (eggCounter > 10)
                eggCounter = 10;

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.NightScreamWakesYou".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.KihaYellsGrabHand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) > 0)
                OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.RemoveSilkExposeVagina".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.LickToCoaxEggs".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.OrgasmCoatsYourFace".Localized());

            if (eggCounter > 2)
                OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.RepeatUntilBellyFlat".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.CountTheEggs".Localized(
                NumberUtils.Num2Text(eggCounter)
            ));

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.KihaSmilesAndKisses".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.EggsShakeAlready".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.EggCracksThenBurst".Localized());

            var oldTotal = TotalKihaChildren();

            var maleCount = 0;

            var femaleCount = 0;

            var hermCount = 0;

            var childList = new List<string>();

            for (var i = 0; i < eggCounter; i++)
                if (Player.Pregnancy.allowHerm && Random.Shared.Next(3) < 1)
                    hermCount += 1;
                else
                    if (Random.Shared.Next(2) == 0)
                        maleCount += 1;
                    else
                        femaleCount += 1;

            if (maleCount > 0)
            {
                childList.Add(NumberUtils.Num2Text(maleCount) + " " + (maleCount > 1 ? "Common.Text.Boys".Localized() : "Common.Text.Boy".Localized()));

                CoC.Instance.Flags.Set(GameFlagIds.KihaChildrenBoys, CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) + maleCount);
            }

            if (femaleCount > 0)
            {
                childList.Add(NumberUtils.Num2Text(femaleCount) + " " + (femaleCount > 1 ? "Common.Text.Girls".Localized() : "Common.Text.Girl".Localized()));

                CoC.Instance.Flags.Set(GameFlagIds.KihaChildrenGirls, CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenGirls) + femaleCount);
            }

            if (hermCount > 0)
            {
                childList.Add(NumberUtils.Num2Text(hermCount) + " " + (hermCount > 1 ? "Common.Text.Hermaphrodites".Localized() : "Common.Text.Hermaphrodite".Localized()));

                CoC.Instance.Flags.Set(GameFlagIds.KihaChildrenHerms, CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenHerms) + hermCount);
            }

            OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.KihaCountsNewborns".Localized(
                string.Join(", ", childList)
            ));

            if (oldTotal > 0)
                OutputText("Scenes.Kihafollowerscene.KihaGivesBirth.OlderChildrenAwe".Localized());
        }

        public void KihaBreastfeedingTime()
        {

            OutputText("Scenes.Kihafollowerscene.KihaBreastfeedingTime.KihaBreastfeeds".Localized());
        }

        public void KihaTellsChildrenStory()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.KihaChildMaturityCounter, CoC.Instance.Flags.Get(GameFlagIds.KihaChildMaturityCounter) - 1);

            OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.KihaAsksYouSit".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.SitOnCrudeSeat".Localized());

            if (TotalKihaChildren() == 1)
                OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.KihaSeatsOnlyChild".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.KihaSeatsHerBrood".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.KihaRecountsHerPast".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.ChildrenAskWhatNext".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.KihaResumesMeetingTale".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LethiceDefeated) > 0)
                OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.TellOfLethiceDefeat".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.KihaPlansStrongholdRaid".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaTellsChildrenStory.ChildrenThankYou".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, -2f), new DynStat(GameStats.Lust, -50f));

            ClearButtons();

            ReturnToCamp(1);
        }

        public void KihaTrainsHerKids()
        {

            var availableGenders = new List<string>();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0)
                availableGenders.Add("male");

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenGirls) > 0)
                availableGenders.Add("female");

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenHerms) > 0)
                availableGenders.Add("herms");

            string select = availableGenders[Random.Shared.Next(availableGenders.Count)];

            OutputText("Scenes.Kihafollowerscene.KihaTrainsHerKids.KihaTeachesTechnique".Localized(
                Player.Mf("daddy", "mommy-daddy")
            ));

            OutputText("Scenes.Kihafollowerscene.KihaTrainsHerKids.KihaReportsAxeTraining".Localized());
        }

        public void KihaChildGraduationTime()
        {

            CoC.Instance.Flags.Set(GameFlagIds.KihaChildMaturityCounter, 0);

            var availableGenders = new List<string>();

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0)
                availableGenders.Add("male");

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenGirls) > 0)
                availableGenders.Add("female");

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenHerms) > 0)
                availableGenders.Add("herms");

            string select = availableGenders[Random.Shared.Next(availableGenders.Count)];

            OutputText("Scenes.Kihafollowerscene.KihaChildGraduationTime.CheckGrownChildren".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaUndergarments) > 0)
                OutputText("Scenes.Kihafollowerscene.KihaChildGraduationTime.JustLikeKiha".Localized());
            else
                OutputText("Scenes.Kihafollowerscene.KihaChildGraduationTime.UnlikeNakedKiha".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaChildGraduationTime.SomePreferBareSkin".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaChildGraduationTime.KihaPraisesWarriors".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KihaChildMaturityCounter, 0);
        }

        public void KihaChildFirebreathing()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathing.FamiliarSmokeDrifts".Localized(
                KihaChildren(),

                TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized())
            ));

            Save.SetBool("kidFirebreathing", true);

            ClearButtons();

            AddNextButton("Common.Button.Investigate".Localized(), KihaChildFirebreathingInvestigate);

            AddNextButton("Common.Button.Leave".Localized(), KihaChildFirebreathingLeave);
        }

        public void KihaChildFirebreathingInvestigate()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingInvestigate.InvestigateTheSmoke".Localized(
                TotalKihaChildren() > 5 ? "swarming brood clamber on top of each other to get a closer look, their cheers loud enough to be heard for miles around" : KihaChildren() + " " + (TotalKihaChildren() > 1 ? "Scenes.Factory.BadEndTension.CryWord".Localized() : "Common.Text.Cries".Localized()) + " out with glee, seemingly unfazed by the embers raining down on " + (TotalKihaChildren() > 1 ? "them" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized())) + "."
            ));

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingInvestigate.SweatyKihaAsksWatch".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingInvestigate.KihaMaybeBlushing".Localized(
                TotalKihaChildren() > 5 ? "Common.Text.AllOfYourChildrenTurnTo".Localized() : "Common.Text.Your".Localized() + KihaChildren() + " " + (TotalKihaChildren() > 1 ? "Common.Text.Turn".Localized() : "Common.Text.Turns".Localized()) + " to you, evidently excited that " + (TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized())) + " [father] came."
            ));

            ClearButtons();

            AddNextButton("Common.Button.Watch".Localized(), KihaChildFirebreathingWatch);

            AddNextButton("Common.Button.Compete".Localized(), () => KihaChildFirebreathingCompete());
        }

        public void KihaChildFirebreathingWatch()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingWatch.RequestFireDemo".Localized(
                TotalKihaChildren() > 5 ? "Common.Text.AndPushYourselfUpCarefulNot".Localized() : "Common.Text.AndIfHaslegsSitDownLower".Localized() + (TotalKihaChildren() > 1 ? "Common.Text.YourChildrenQuicklyClimbingAllOver".Localized() : "Common.Text.Your".Localized() + (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.Son".Localized() : "Common.Text.Daughter".Localized()) + " [if (isbiped) {curling up in your lap|[if (isgoo) { curling up in your slime|climbing atop your back}]}] to watch the show.")
            ));

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingWatch.GaspAtNearMiss".Localized(
                KihaChildren()
            ));

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingWatch.MimicSpraysSpittle".Localized(
                KihaChildren(),

                TotalKihaChildren() > 1 ? "they" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.He".Localized() : "Common.Text.She".Localized())
            ));

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingWatch.AloneBlowingEmbers".Localized(
                TotalKihaChildren() > 5 ? "Common.Text.AllOfYourChildrenCrowdAround".Localized() : "Common.Text.Your".Localized() + KihaChildren() + " stand" + (TotalKihaChildren() > 1 ? "" : "s") + " beside",

                TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized())
            ));

            ReturnToCamp(1);
        }

        public bool KihaChildFirebreathingCompete(bool display = true)
        {
            ClearOutput();

            var available = false;

            if (display)
            {
                KihaSprite();

                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingCompete.YouJoinBesideKiha".Localized(
                    KihaChildren(),

                    TotalKihaChildren() == 1 ? "Common.Text.LooksHesitantlyBetweenTheTwoOf".Localized() : "Common.Text.ImmediatelyPickTheirFavoritesArguingAmong".Localized() + RandomUtils.Choice(["you're the underdog here", "your kids have your back", "it's too close to call"]) + "."
                ));

                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingCompete.KihaRoarsForAttention".Localized());

                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingCompete.ChooseFieryPower".Localized());

                ClearButtons();
            }

            if (Player.HasPerk(PerkIds.TerrestrialFire))
                if (display) AddNextButton("Common.Button.Akbalsgift".Localized(), () => KihaChildFirebreathingDisplay(0)); else available = true;
            if (Player.HasPerk(PerkIds.CorruptedNinetails))
                if (display) AddNextButton("Common.Button.Foxfire".Localized(), () => KihaChildFirebreathingDisplay(1)); else available = true;
            if (Player.HasPerk(PerkIds.Dragonfire))
                if (display) AddNextButton("Common.Button.Dragonbreath".Localized(), () => KihaChildFirebreathingDisplay(2)); else available = true;
            if (Player.HasPerk(PerkIds.EnlightenedNinetails))
                if (display) AddNextButton("Common.Button.Foxfire".Localized(), () => KihaChildFirebreathingDisplay(3)); else available = true;
            if (Player.HasPerk(PerkIds.Hellfire))
                if (display) AddNextButton("Perks.HellFire.Name".Localized(), () => KihaChildFirebreathingDisplay(4)); else available = true;
            if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                if (display) AddNextButton("Common.Button.Whitefire".Localized(), () => KihaChildFirebreathingDisplay(5)); else available = true;
            return available;
        }

        public void KihaChildFirebreathingDisplay(int type)
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.ChildrenWatchEagerly".Localized(
                KihaChildren()
            ));

            switch (type)
            {
                case 0:
                {
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.EmeraldFlamesStrike".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.PurpleFlameShower".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.DragonfireBlastScorches".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.BlueFlameShower".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.RollingLustFlame".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.BlindingFlash".Localized());
                }

                break;
            }

            if (type % 2 == 1)
                OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.AfterBlindingFlash".Localized(
                    KihaChildren()
                ));

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.KihaTakesYourPlace".Localized(
                KihaChildren(),

                TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized()),

                TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized()),

                TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized()),

                TotalKihaChildren() > 1 ? "them" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized())
            ));

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.NotMuchRemains".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingDisplay.KihaCallsItDraw".Localized(
                KihaChildren(),

                TotalKihaChildren() > 1 ? "they" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.He".Localized() : "Common.Text.She".Localized()),

                TotalKihaChildren() > 1 ? "their" : (CoC.Instance.Flags.Get(GameFlagIds.KihaChildrenBoys) > 0 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized())
            ));

            ReturnToCamp(1);
        }

        public void KihaChildFirebreathingLeave()
        {

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaChildFirebreathingLeave.LeaveForCamp".Localized());

            Go<CampScene>();
        }

        public void KihaCookingLesson()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson.DiplomaticWordChoice".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson.KihaHesitantGlance".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson.BlushingDragonessYields".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson.PlayfulArmKnock".Localized());

            Save.SetInt("badCookingTasted", 3);

            KihaCookingLesson2();
        }

        public void KihaCookingLesson2()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.FishAndHerbAroma".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.KihaAsksAboutStew".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.AmilyExplainsTasting".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.AwkwardAmilyGreeting".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.AmilyWavesFrantic".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.AmilyPraisesHeat".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.KihaBoastsHerFire".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson2.AmilySetsPan".Localized());

            KihaCookingLesson3();
        }

        public void KihaCookingLesson3()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.AmilyFinishesDemo".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.BasicMealAdvice".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.KihaSmirksRebut".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.AmilyOffersExample".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.KihaBrowRaised".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.AmilyProposesRecipe".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.KihaWatchesPrep".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.StunnedWatchSeasoning".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.TranceBreaksAsServed".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.AmilySuggestsTasting".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.KihaThreatensTasteTest".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.TerrorThenReassure".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.CharredMushRevealed".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson3.QuietAdmissionMistake".Localized());

            KihaCookingLesson4();
        }

        public void KihaCookingLesson4()
        {
            ClearOutput();

            KihaSprite();

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson4.GaspBackToReality".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson4.KihaTearfulApology".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson4.ComfortHerShoulder".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson4.ChirpyAmilyRecovers".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaCookingLesson4.KihaReveresAmily".Localized());

            ReturnToCamp(1);
        }

        public void KihaDrakeHeart()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.OfferVanillaFlower".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.StunnedKihaBlushes".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.ExplainingLoverLogic".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.FlusteredSheReplies".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.KihaSmilesAtGift".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.SurpriseEmbraceLove".Localized());

            OutputText("Scenes.Kihafollowerscene.KihaDrakeHeart.ReturnHerEmbrace".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.DrakeHeart));

            Save.SetBool("flowerGifted", true);

            WarmLoverKihaIntro();
        }

        public void Kiss()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.Kiss.SurpriseKissInterrupt".Localized());

            OutputText("Scenes.Kihafollowerscene.Kiss.WideEyedPullAway".Localized());

            OutputText("Scenes.Kihafollowerscene.Kiss.SweetTongueKiss".Localized());

            OutputText("Scenes.Kihafollowerscene.Kiss.LoveMumbleRelease".Localized());

            ReturnToCamp(1);
        }

        public void SleepWith()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.SleepWith.InviteKihaToBed".Localized());

            OutputText("Scenes.Kihafollowerscene.SleepWith.EagerDragonessAgrees".Localized());

            OutputText("Scenes.Kihafollowerscene.SleepWith.GetInBedBeside".Localized());

            OutputText("Scenes.Kihafollowerscene.SleepWith.ShyGoodnightSnuggle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Kiha");

            Save.SetBool("wakeUp", true);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
        }

        public void Morning()
        {
            ClearOutput();

            OutputText("Scenes.Kihafollowerscene.Morning.WarmerMorningWake".Localized());

            OutputText("Scenes.Kihafollowerscene.Morning.IndulgeLoverWarmth".Localized());

            OutputText("Scenes.Kihafollowerscene.Morning.ReluctantDeparture".Localized());

            Save.SetBool("wakeUp", false);

            Go<CampScene>();
        }

    }
}