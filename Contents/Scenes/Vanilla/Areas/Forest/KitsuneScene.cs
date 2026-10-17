using MonoCoC.Contents.World;
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
using System.Linq;
using MonoCoC.Contents.Enemies;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<KitsuneScene>("coc:kitsune")]
    public class KitsuneScene : Scene
    {

        public static string StatueLocation
        {
            get => Save.GetString("statueLocation", "");

            set => Save.SetString("statueLocation", value);
        }

        public void StartKitsuneImpFight()
        {
            var imp = EnemyRegistry.Create(EnemyIds.Imp);

            imp.OnDefeated = (hpVictory) => WinKitsuneImpFight();

            imp.OnWon = (hpVictory) => LoseKitsuneImpFight();

            CombatScene.Start(imp, () => Go<CampScene>());
        }

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public static SaveContent Save => SaveContent(SceneIds.Forest.Kitsune);

        public override void Entry()
        {
            ExecEncounter();
        }

        public void ExecEncounter()
        {

            if (Random.Shared.Next(3 + Player.ItemCount(ConsumableIds.FoxJewel)) >= 2)
                KitsuneShrine();
            else
                EnterTheTrickster();
        }

        public void KitsuneSprite()
        {

        }

        public void EnterTheTrickster()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MetKitsunes) > 0)
            {
                WillOWisp();

                return ;
            }

            OutputText("Scenes.Kitsune.EnterTheTrickster.HelpCry".Localized());

            OutputText("Scenes.Kitsune.EnterTheTrickster.TurnWithStart".Localized());

            if (Player.IsChild())
            {
                OutputText("Scenes.Kitsune.EnterTheTrickster.GrabbingArmRun".Localized());

                OutputText("Scenes.Kitsune.EnterTheTrickster.AboutToQuestionImp".Localized());
            }
            else
            {
                OutputText("Scenes.Kitsune.EnterTheTrickster.ThankGoodnessHelp".Localized());

                OutputText("Scenes.Kitsune.EnterTheTrickster.AboutToQuestionImpAgain".Localized());
            }

            StartKitsuneImpFight();

            CurrentMonster.CreateStatusEffect(StatusEffectIds.KitsuneFight, 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.MetKitsunes, CoC.Instance.Flags.Get(GameFlagIds.MetKitsunes) + 1);
        }

        public void LoseKitsuneImpFight()
        {
            ClearOutput();

            if (Random.Shared.Next(3) == 0)
                CurrentMonster.Body.Get<HairPart>().Color = "blonde";
            else
                if (Random.Shared.Next(2) == 0)
                    CurrentMonster.Body.Get<HairPart>().Color = "black";
                else
                    CurrentMonster.Body.Get<HairPart>().Color = "red";

            KitsuneSprite();

            OutputText("Scenes.Kitsune.LoseKitsuneImpFight.FallDefeatLookUp".Localized());

            OutputText("Scenes.Kitsune.LoseKitsuneImpFight.EasierThanThought".Localized());

            OutputText("Scenes.Kitsune.LoseKitsuneImpFight.WomanBendsLow".Localized());

            OutputText("Scenes.Kitsune.LoseKitsuneImpFight.PushoverRemark".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoseToKitsunes);
        }

        public void WinKitsuneImpFight()
        {
            ClearOutput();

            if (Random.Shared.Next(3) == 0)
                CurrentMonster.Body.Get<HairPart>().Color = "blonde";
            else
                if (Random.Shared.Next(2) == 0)
                    CurrentMonster.Body.Get<HairPart>().Color = "black";
                else
                    CurrentMonster.Body.Get<HairPart>().Color = "red";

            KitsuneSprite();

            OutputText("Scenes.Kitsune.WinKitsuneImpFight.ThankYouThankYou".Localized());

            OutputText("Scenes.Kitsune.WinKitsuneImpFight.MyHeroSwoons".Localized());

            OutputText("Scenes.Kitsune.WinKitsuneImpFight.GazingDeepHaze".Localized());

            if ((Player.LustPercent < 70) || (Player.Intelligence >= 40))
            {
                OutputText("Scenes.Kitsune.WinKitsuneImpFight.PushAwayStumble".Localized());

                if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.HairBackLengthFlaxenAgain".Localized());
                else
                    if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                        OutputText("Scenes.Kitsune.WinKitsuneImpFight.HairAssLengthRavenAgain".Localized());
                    else
                        OutputText("Scenes.Kitsune.WinKitsuneImpFight.HairShoulderCrimsonAgain".Localized());

                OutputText("Scenes.Kitsune.WinKitsuneImpFight.SixTailsFanning".Localized());

                OutputText("Scenes.Kitsune.WinKitsuneImpFight.SawThroughGlamourAgain".Localized());

                FollowTheWillOWisp(true);
            }
            else
                if (Player.IsReligious() || Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
                {
                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.TouchRecoilsYelp".Localized());

                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.OwOwOw".Localized());

                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.SpotsClearGlamour".Localized());

                    if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
                        OutputText("Scenes.Kitsune.WinKitsuneImpFight.HairBackLengthFlaxenAgain".Localized());
                    else
                        if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                            OutputText("Scenes.Kitsune.WinKitsuneImpFight.HairAssLengthRavenAgain".Localized());
                        else
                            OutputText("Scenes.Kitsune.WinKitsuneImpFight.HairShoulderCrimsonAgain".Localized());

                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.SixTailsFanningAgain".Localized());

                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.SawThroughGlamourAgain".Localized());

                    FollowTheWillOWisp(true);
                }
                else
                {
                    OutputText("Scenes.Kitsune.WinKitsuneImpFight.TouchTingles".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => Mansion(true, true));
                }
        }

        public void WillOWisp()
        {
            ClearOutput();

            if (Random.Shared.Next(3) == 0)
                CurrentMonster.Body.Get<HairPart>().Color = "blonde";
            else
                if (Random.Shared.Next(2) == 0)
                    CurrentMonster.Body.Get<HairPart>().Color = "black";
                else
                    CurrentMonster.Body.Get<HairPart>().Color = "red";

            KitsuneSprite();

            OutputText("Scenes.Kitsune.WillOWisp.ExploreDeeperAware".Localized());

            OutputText("Scenes.Kitsune.WillOWisp.GlimpseMotion".Localized());

            OutputText("Scenes.Kitsune.WillOWisp.BeckoningFollow".Localized());

            if (Player.HasKeyItem("Traveler's Guide"))
            {
                OutputText("Scenes.Kitsune.WillOWisp.MindJoggedRemember".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Kitsune.WillOWisp.ButtonTurnBack".Localized(), TurnBackFromWillOWisp);

                AddButton(1, "Common.Button.Follow".Localized(), () => FollowTheWillOWisp());
            }
            else
                DoNext(() => FollowTheWillOWisp());
        }

        public void TurnBackFromWillOWisp()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.TurnBackFromWillOWisp.NoWayGallivanting".Localized());

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();

            DoNext(() => ReturnToCamp(1));
        }

        public void FollowTheWillOWisp(bool firstTime = false)
        {
            ClearOutput();

            if (!firstTime)
            {
                OutputText("Scenes.Kitsune.FollowTheWillOWisp.FlameDartsOff".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
            }

            if ((Player.Lust >= Player.MaxLust || Player.Intelligence < 20) && !firstTime)
                Mansion(false, false);
            else
            {
                KitsuneSprite();

                if (!firstTime)
                {
                    OutputText("Scenes.Kitsune.FollowTheWillOWisp.UnsureHowLong".Localized());

                    if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
                        OutputText("Scenes.Kitsune.FollowTheWillOWisp.HairBackLengthFlaxen".Localized());
                    else
                        if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                            OutputText("Scenes.Kitsune.FollowTheWillOWisp.HairAssLengthRaven".Localized());
                        else
                            OutputText("Scenes.Kitsune.FollowTheWillOWisp.HairShoulderCrimson".Localized());

                    OutputText("Scenes.Kitsune.FollowTheWillOWisp.HairSmirking".Localized());

                    OutputText("Scenes.Kitsune.FollowTheWillOWisp.HuFuFuClever".Localized());

                    OutputText("Scenes.Kitsune.FollowTheWillOWisp.ClosesGapNotGirl".Localized());

                    OutputText("Scenes.Kitsune.FollowTheWillOWisp.ResistedIllusionsImpressive".Localized());
                }

                OutputText("Scenes.Kitsune.FollowTheWillOWisp.GoingSomewhere".Localized());

                OutputText("Scenes.Kitsune.FollowTheWillOWisp.BehindYouQuickly".Localized());

                OutputText("Scenes.Kitsune.FollowTheWillOWisp.OverHereSilly".Localized());

                OutputText("Scenes.Kitsune.FollowTheWillOWisp.ToneInnocuous".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightSomeKitsunes);

                AddButton(1, "Common.Button.Talk".Localized(), TalkAfterResistingKitsunellusion);
            }
        }

        public void TalkAfterResistingKitsunellusion()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.OddNotThreatening".Localized());

            if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
            {
                OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.NotImportantSly".Localized());

                OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.ConversationWatching".Localized());
            }
            else
                if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                {
                    OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.BlinkUponYouFinger".Localized());

                    OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.InsultedOrTerrified".Localized());
                }
                else
                {
                    OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.CaughtMeFeigning".Localized());

                    OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.DirtyLookOffended".Localized());
                }

            OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.TwistAwayTrying".Localized());

            OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.NoFunSmirking".Localized());

            OutputText("Scenes.Kitsune.TalkAfterResistingKitsunellusion.SelfPreservationCuriosity".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Follow".Localized(), () => Mansion(true, false));

            if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !((0 & 32 ) != 0))
                AddNextButton("Common.Button.Vines".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), () => LeaveKitsune(true));
        }

        public void LeaveKitsune(bool talked = false)
        {
            ClearOutput();

            KitsuneSprite();

            if (talked)
                OutputText("Scenes.Kitsune.LeaveKitsune.ThankDeclineNervously".Localized());
            else
                OutputText("Scenes.Kitsune.LeaveKitsune.TurnToLeaveDistance".Localized());

            OutputText("Scenes.Kitsune.LeaveKitsune.DontLeaveYet".Localized());

            OutputText("Scenes.Kitsune.LeaveKitsune.IHaveSomething".Localized());

            OutputText("Scenes.Kitsune.LeaveKitsune.HoldsWhitePackage".Localized());

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.KitsuneGift.ToString());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.KitsuneGift), 1);

ReturnToCamp(1);
            }
        }

        public void Mansion(bool willing, bool firstTime = false)
        {
            ClearOutput();

            KitsuneSprite();

            if (willing)
            {
                if (!firstTime)
                    OutputText("Scenes.Kitsune.Mansion.CuriosityLustThing".Localized());

                OutputText("Scenes.Kitsune.Mansion.LeadsDeeper".Localized());

                OutputText("Scenes.Kitsune.Mansion.StopsTurningBack".Localized());
            }
            else
                OutputText("Scenes.Kitsune.Mansion.HypnoticFlameHours".Localized());

            OutputText("Scenes.Kitsune.Mansion.SayStartWelcome".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MansionVisited) > 0)
                OutputText("Scenes.Kitsune.Mansion.WordBack".Localized());

            OutputText("Scenes.Kitsune.Mansion.HearAsYouEnter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MansionVisited) > 0)
                OutputText("Scenes.Kitsune.Mansion.TrioFamiliarSisters".Localized());
            else
            {
                OutputText("Scenes.Kitsune.Mansion.TrioLovelyMaidens".Localized());

                if (willing)
                    OutputText("Scenes.Kitsune.Mansion.IncludingOneThatLed".Localized());

                OutputText("Scenes.Kitsune.Mansion.BlondeRedheadBlack".Localized());
            }

            if (willing)
                OutputText("Scenes.Kitsune.Mansion.BeenExpectingYou".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MansionVisited) > 0)
                {
                    if (Player.FatiguePercent < 70)
                        OutputText("Scenes.Kitsune.Mansion.SoThrilledComeSee".Localized());
                    else
                        OutputText("Scenes.Kitsune.Mansion.KeepComingBack".Localized());
                }
                else
                    OutputText("Scenes.Kitsune.Mansion.GladDecidedCome".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MansionVisited, CoC.Instance.Flags.Get(GameFlagIds.MansionVisited) + 1);

            OutputText("Scenes.Kitsune.Mansion.ParagraphBreak".Localized());

            OutputText("Scenes.Kitsune.Mansion.ThreeLadiesCloseIn".Localized());

            OutputText("Scenes.Kitsune.Mansion.CupNeverEmpty".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => NonTentaclePCMansion(willing));
        }

        public void NonTentaclePCMansion(bool willing)
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.NonTentaclePCMansion.EnjoyedFeast".Localized());

            OutputText("Scenes.Kitsune.NonTentaclePCMansion.JustRelaxWhisper".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.Kitsune1) > 0 && (CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.Kitsune2) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.QuestDone) == 0)
                DoNext(() => AkbalFollowupMansion());
            else
                if (Player.CountCocksOfType("TENTACLE") >= 3)
                    DoNext(() => TentacleKitsuneWingWangs());
                else
                    if (Player.HasCock())
                        KitsuneMaleOrHermMansion(willing);
                    else
                        KitsuneFemaleOrGenderless(willing);
        }

        public void AkbalFollowupMansion()
        {

            CoC.Instance.Flags.Set(GameFlagIds.AkbalQuestStatus, CoC.Instance.Flags.Get(GameFlagIds.AkbalQuestStatus) | (int)AkbalEvent.Kitsune2);

            OutputText("Scenes.Kitsune.AkbalFollowupMansion.ZenShatteredScream".Localized());

            OutputText("Scenes.Kitsune.AkbalFollowupMansion.PridefulCompulsion".Localized());

            OutputText("Scenes.Kitsune.AkbalFollowupMansion.BlackHairedBreaks".Localized());

            OutputText("Scenes.Kitsune.AkbalFollowupMansion.BlondeNeedsAddressing".Localized());

            OutputText("Scenes.Kitsune.AkbalFollowupMansion.GroupPullsUp".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void KitsuneMaleOrHermMansion(bool willing = true)
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.SeeWhatWeHave".Localized());

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.SoftFurCaresses".Localized());

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.ThreeTailsEnvelop".Localized());

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.MainCourse".Localized());

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.TailsRecedeSlowly".Localized(
                 Player.CockDescript(Player.BiggestCockIndex()),

                 Player.CockHead(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.LicksSpiral".Localized());

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.OhYouBeast".Localized());

            OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.HikeUpHips".Localized());

            if (willing)
            {
                OutputText("Scenes.Kitsune.KitsuneMaleOrHermMansion.HowDoYouRespond".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHer".Localized(), () => KitSuneLetHerMansion(willing));

                AddButton(1, "Common.Button.ShoveHer".Localized(), KitsuneShoveHerMansion);
            }
            else
                KitSuneLetHerMansion(true);
        }

        public void KitSuneLetHerMansion(bool willing = true)
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.BlackHairedLiesBack".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.BlondePosteriorBounces".Localized(
                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.FoxGirlGroans".Localized(
                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.SlenderFingersHips".Localized());

            if ((int)Player.Gender >= 2)
                OutputText(Player.VaginaDescript());
            else
                OutputText(Player.AssholeDescript());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.ThrustSpearing".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordHindquarters".Localized());
            else
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordWarmFolds".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WrapsArmsFingertips".Localized());

            if (Player.Gender == Gender.Male)
                Player.ButtChange(14, true, true, false);
            else
                Player.CuntChange(14, true, true, false);

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.ParagraphBreak".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.SandwichedShudder".Localized(
                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.RedheadMassageLower".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.PalmsCaressing".Localized());

                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.RemainingCock".Localized(
                     NumberUtils.Num2Text(Player.Cocks.Count - 1)
                ));

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Kitsune.KitSuneLetHerMansion.PluralS".Localized());

                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.AndSqueezingTightly".Localized());
            }
            else
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.CirclingNavel".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.ContinuesPumpForcefully".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.TightPassageSqueezes".Localized(
                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.PulsatingMemberThrobs".Localized());

            if ((int)Player.Gender >= 2)
                OutputText(Player.VaginaDescript());
            else
                OutputText(Player.AssholeDescript());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.BuildsTowardClimax".Localized());

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordCunt".Localized());
            else
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordRectum".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.AroundShaftOverEdge".Localized());

            if ((int)Player.Gender >= 2)
                OutputText(Player.VaginaDescript());
            else
                OutputText(Player.AssholeDescript());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.ThickBulgePassing".Localized());

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordSlipperyHole".Localized());
            else
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordInnards".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.ThickCumHitsYour".Localized());

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordWomb".Localized());
            else
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WordIntestines".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.IceWaterThenWarmth".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.PleasantPressureBelly".Localized(
                 (Player.BiggestCockArea() > 80) ? " She lurches forward, her fingers clawing at the wooden deck that runs along the edge of the pool, her entire body heaving with pleasure." : " As you feel the coiling spring in your lower body near its limit, you suddenly feel the redhead's hand wrap around your " + Player.CockDescript(Player.BiggestCockIndex()) + ". She slyly leans over your shoulder, raising a finger to her lips and whispering to you, [say: Shh...][pg]Without warning, she yanks you back, pulling you from the blonde's quivering cunt just as you begin to climax, adjusting the angle quickly and bucking her hips forward to slam your twitching shaft into the blonde's ass! The soft ring of her anus yields to the invasion with surprising ease, spreading open as if by reflex and then clamping shut around your base when your hips make contact with her quivering cheeks again. She lurches forward, releasing a surprised gasp and nearly collapsing under your compulsory anal assault, her entire body heaving with a combination of shock and pleasure."
            ));

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.FewVirileStreams".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.KitSuneLetHerMansion.NumerousStreamsSloppily".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.KitSuneLetHerMansion.ThickRibbonsSpurt".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.HotRopesSplash".Localized());
                        else
                            if (Player.CumQ() > 2500)
                                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.HotWavesCrest".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.AfterEternityBlonde".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.WarmGripsCock".Localized());

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Kitsune.KitSuneLetHerMansion.FloodSeedSpill".Localized());

            OutputText("Scenes.Kitsune.KitSuneLetHerMansion.TwitchingCockRelieves".Localized());

            Player.Orgasm("Dick");

            Player.Orgasm("VaginalAnal");

            DoNext(() => KitsuneStillHungryMansion());
        }

        public void KitsuneShoveHerMansion()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.InSpiteDrunken".Localized());

            OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.FFineQuietly".Localized());

            OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.LetUpDisappointed".Localized());

            if (Player.Gender == Gender.Male || ((Player.Gender == Gender.Hermaphrodite) && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) == 0))
            {
                Player.Orgasm("Dick");

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.BlondeWadesOver".Localized(
                     Player.CockDescript(Player.BiggestCockIndex()),

                     Player.CockHead(Player.BiggestCockIndex())
                ));

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.TailCoilsConstricts".Localized());

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.SomethingBrushingLegs".Localized());

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.HoldingShapelyHips".Localized(
                     Player.CockDescript(Player.BiggestCockIndex())
                ));

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.GigglesObscene".Localized());

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.EdgeCloserClimaxAgain".Localized(
                     (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate",

                     Player.CockDescript(Player.BiggestCockIndex())
                ));

                if (Player.CumQ() <= 150)
                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.FewVirileStreams".Localized());
                else
                    if (Player.CumQ() <= 350)
                        OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.NumerousStreamsSloppily".Localized());
                    else
                        if (Player.CumQ() <= 1000)
                            OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.ThickRibbonsSpurt".Localized());
                        else
                            if (Player.CumQ() <= 2500)
                                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.HotRopesWomb".Localized(
                                     (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate"
                                ));
                            else
                                if (Player.CumQ() > 2500)
                                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.HotWavesCrest".Localized(
                                         (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate"
                                    ));

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.SuddenIntrusionOverEdge".Localized());

                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.WarmGripsCock".Localized());
            }
            else
                if ((Player.Gender == Gender.Hermaphrodite) && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
                {
                    Player.Orgasm("Dick");

                    Player.Orgasm("Vaginal");

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.BlondeWadesFlipping".Localized(
                         Player.CockDescript(Player.BiggestCockIndex()),

                         Player.CockHead(Player.BiggestCockIndex())
                    ));

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.TailCoilsConstrictsAgain".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.SomethingBrushingLookDown".Localized());

                    Player.CuntChange(24, true, true, false);

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.ParagraphBreak".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.RedheadSquealsShaft".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.HoldingShapelyHipsAgain".Localized(
                         Player.CockDescript(Player.BiggestCockIndex())
                    ));

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.GigglesObsceneAgain".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.EdgeCloserClimaxAgain".Localized(
                         (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate",

                         Player.CockDescript(Player.BiggestCockIndex())
                    ));

                    if (Player.CumQ() <= 150)
                        OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.FewVirileStreamsAgain".Localized());
                    else
                        if ((Player.CumQ() > 150) && (Player.CumQ() <= 350))
                            OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.NumerousStreamsSloppilyAgain".Localized());
                        else
                            if (Player.CumQ() <= 1000)
                                OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.ThickRibbonsSpurtAgain".Localized());
                            else
                                if (Player.CumQ() <= 2500)
                                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.HotRopesWombAgain".Localized());
                                else
                                    if (Player.CumQ() > 2500)
                                        OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.HotWavesCrestAgain".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.AtSameTimeFeelShaft".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.WrySmirkReleases".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.SlitheringTongueCoils".Localized());

                    OutputText("Scenes.Kitsune.KitsuneShoveHerMansion.TwitchingCockRelieves".Localized());
                }

            DoNext(() => KitsuneStillHungryMansion());
        }

        public void KitsuneStillHungryMansion()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.AboutToSlipRoused".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.StillHungry".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.CoolingTingle".Localized(
                 (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate",

                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.RockHardEngulfed".Localized(
                 Player.CockHead(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.EyesRollBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
                OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.TremblingMoanDown".Localized());
            else
                OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.FingersTrailChest".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.AbruptlyAwareHands".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.OverlySensitiveRomp".Localized(
                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
                OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.RedheadHipsSlap".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.TheedNeGarbled".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.SparkBlueFlame".Localized(
                 (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate",

                 Player.CockDescript(Player.BiggestCockIndex())
            ));

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.ViscousFemcumCrashes".Localized());

            OutputText("Scenes.Kitsune.KitsuneStillHungryMansion.ExhaustedStupor".Localized());

            Player.Orgasm("Dick");

            Outro();
        }

        public void KitsuneFemaleOrGenderless(bool willing = true)
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsuneFemaleOrGenderless.JustRelaxDarling".Localized(
                 ((int)Player.Gender >= 2) ? Player.ClitDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.KitsuneFemaleOrGenderless.ErogenousVelvetyTails".Localized());

            OutputText("Scenes.Kitsune.KitsuneFemaleOrGenderless.FingertipSecond".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.KitsuneFemaleOrGenderless.WarmProddingLookDown".Localized());

            if (willing)
            {
                OutputText("Scenes.Kitsune.KitsuneFemaleOrGenderless.HowDoYouRespond".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHer".Localized(), () => KitsunesGenderlessLetHer(willing));

                AddButton(1, "Common.Button.ShoveHer".Localized(), KitsunesGenderlessShoveHer);
            }
            else
                KitsunesGenderlessLetHer(true);
        }

        public void KitsunesGenderlessLetHer(bool willing)
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.WhileGrinds".Localized(
                 willing ? "You decide to let the girl have her fun, allowing her to shift positions to begin rubbing her throbbing shaft against your " + ((Player.Gender == Gender.Female) ? Player.VaginaDescript() : "featureless groin") + " eagerly." : "You shake your head sluggishly, but your slurred protests fall on deaf ears. She easily sidesteps your [leg] as you try to shove her away, sliding forward to lay a disarming kiss on your cheek. A bright blue wisp of flame crackles from her lips, and that is that - the resulting spark of pleasure that arcs up your spine shatters the last of your resistance, and your cares melt like butter."
            ));

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.HeadDrawnLap".Localized(
                 (Player.Body.Get<HairPart>().Length > 0) ? "through your " + Player.HairDescript() : "across the sides of your face"
            ));

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.RedheadWastesNoTime".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            if ((int)Player.Gender >= 2)
                Player.CuntChange(14, true, true, false);
            else
                Player.ButtChange(14, true, true, false);

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.ParagraphBreak".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.SoftLapSlidesOut".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.VictorGoldenTails".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.InnardsPummeled".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.TongueSwirlsPussy".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.RhythmicThumping".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.BaseSwellsLoad".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.KitsunesGenderlessLetHer.MoansAllFour".Localized());

            DoNext(() => GenderlessKitsuneStillHungry());
        }

        public void KitsunesGenderlessShoveHer()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.InSpiteDrunken".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.FFineQuietly".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.LetUpDisappointed".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.BlondeWadesOver".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.TailsCoilThighs".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0 ? "your " + (((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()) + " slides across the tip of her cock teasingly" : "you are rubbed against her slavering slit"
            ));

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.SomethingBrushingYou".Localized(
                 CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0 ? "dragging you down with her weight at the same time" + ((Player.Gender == Gender.Female) ? " to impale you on the redhead's cock." : ", her tongue receding just as you are impaled on the redhead's cock. The slick organ slides around the base of her cock, constricting tightly, and you can feel it swell as the veins and ridges become more pronounced.") : "and while you are at your most vulnerable, she twists two of the redhead's tails into a tight bundle, thrusting it into your " + ((Player.Gender == Gender.Female) ? "vagina." : "ass.")
            ));

            Player.CuntChange(24, true, true, false);

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.ParagraphBreak".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.RedheadSquealsHandsRising".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.HoldingShapelyHipsGrind".Localized(
                 (Player.Gender == Gender.Female) ? Player.ClitDescript() : "bare crotch"
            ));

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.GigglesTracingFlames".Localized());

            OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.EdgeCloserOrgasm".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
            {
                OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.BlondeGlances".Localized());

                OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.WrySmirkReleases".Localized());

                OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.BlackHairedTongueCoils".Localized(
                     ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
                ));

                OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.EachTwitchStrength".Localized());
            }
            else
            {
                OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.FourRideWaves".Localized());

                OutputText("Scenes.Kitsune.KitsunesGenderlessShoveHer.ResidualTwitchLeaves".Localized());
            }

            DoNext(() => GenderlessKitsuneStillHungry());
        }

        public void GenderlessKitsuneStillHungry()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.AboutToSlipRoused".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.StillHungry".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.ExposesPalmsRubbing".Localized(
                 (Player.BiggestBreastSize() < 3) ? ((Player.BiggestBreastSize() == 0) ? "flat chest begins" : "breasts begin") + " to swell, jiggling titflesh spilling out with a tense tingle until you are the proud new owner of a sizeable pair of C cups" : "breasts seem to plump up a bit, your nipples growing hard as a tense tingle spreads through them."
            ));

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.RockHardNipplesEngulfed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
                OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.MoanShuddersBreasts".Localized());
            else
                OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.FingersTrailChest".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.ShockGoldenTails".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
                OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.RedheadHipsSlap".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.FeedMeWhispers".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.SparkBlueFlame".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.FourRideWaves".Localized());

            OutputText("Scenes.Kitsune.GenderlessKitsuneStillHungry.ExhaustedStupor".Localized());

            Outro();
        }

        public void TentacleKitsuneWingWangs()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.RealTreatTonight".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.FavoriteFlavor".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.EnjoyOurWork".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.LooveTentacles".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.IllustrateSwallows".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.HowWeLikeIt".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.WarmProddingSide".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.HotBreathShoulders".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.CocksEnsnaredTentacles".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.TentacleDrillsEagerly".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.TentaclesMoveOwn".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.BellySoFull".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.BlondeFoldedDouble".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.AirHeavySemen".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.InsensateTwitch".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.WarmStreamsSpunk".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.ThickStreamsPour".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.RibbonsMilkyWhite".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.ThickJetsHotSpunk".Localized());
                        else
                            if (Player.CumQ() > 2500)
                                OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.ThickStreamsArc".Localized());

            OutputText("Scenes.Kitsune.TentacleKitsuneWingWangs.EveryTwitchLoins".Localized());

            Outro();

            Player.Orgasm("Dick");
        }

        public void Outro(bool tentacles = false)
        {

            Player.ChangeFatigue(15);

            KitsuneSprite();

            Player.ApplyStats(new DynStat(GameStats.Toughness, -2f));

            if (Player.FatiguePercent > 80 && Player.FatiguePercent < 100)
                OutputText("Scenes.Kitsune.Outro.DreamsHauntedWandering".Localized());

            if (Player.Fatigue >= Player.MaxFatigue())
                MansionBadEnd();
            else
            {
                OutputText("Scenes.Kitsune.Outro.AwakenNextMorning".Localized());

                if (tentacles)
                    OutputText("Scenes.Kitsune.Outro.AtLeastGroinUsual".Localized());

                OutputText("Scenes.Kitsune.Outro.SitUpFallBack".Localized());

                if (Player.CumQ() < 10000 || (!Player.HasCock() && Player.LactationQ() < 10000))
                {
                    OutputText("Scenes.Kitsune.Outro.ResidualMagicBonus".Localized());

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Kitsune.Outro.WordCum".Localized());

                        if (Player.CumQ() < 500)
                            Player.CumMultiplier += 3;

                        if (Player.CumQ() < 2000)
                            Player.CumMultiplier += 2;

                        if (Player.CumQ() < 10000)
                            Player.CumMultiplier += 1;
                    }
                    else
                    {
                        OutputText("Scenes.Kitsune.Outro.WordMilk".Localized());

                        if (Player.LactationQ() < 500)
                            Player.BoostLactation(3);

                        if (Player.LactationQ() < 2000)
                            Player.BoostLactation(2);

                        if (Player.LactationQ() < 10000)
                            Player.BoostLactation(1);
                    }

                    OutputText("Scenes.Kitsune.Outro.ProductionEnhanced".Localized());
                }

                View.Time.Hours = 6;

                View.Time.Days += 1;

                if (!CombatController.InCombat)
                    DoNext(() => ReturnToCamp(1));
                else
                    CombatScene.CleanupAfterCombat();
            }
        }

        public void MansionBadEnd()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.MansionBadEnd.DreamsCutShort".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.RealizationHits".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.MusclesFatigue".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.WanderMeandering".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.EscapeInSight".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.LeavingSoSoon".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.WheelAroundFace".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.OneMoreNight".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.DeclineBackingAway".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.PleaseBeOurGuest".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.ThrowCautionRush".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.CrowdAroundLights".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.YoullStayWontYou".Localized());

            OutputText("Scenes.Kitsune.MansionBadEnd.ClingLastVestiges".Localized());

            DoNext(() => KitSuneMansionBadEndII());
        }

        public void KitSuneMansionBadEndII()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.TimePassedSince".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.VaguestShadowMemory".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.WillFadesDeposited".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.ComeInsideEase".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.TrioLavishing".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.ThickersSuckersSupport".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.SixPrehensileAppendages".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.RavenHairstMistressPlan".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.ThrowsBackCatches".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.GlimmerForgotten".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.ForeverServantBeast".Localized());

            OutputText("Scenes.Kitsune.KitSuneMansionBadEndII.TheEnd".Localized());

            GameOver();
        }

        public void FightSomeKitsunes()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.FightSomeKitsunes.SomethingNotRight".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kitsune_" + (CurrentMonster?.Body.Get<HairPart>().Color ?? "blonde"))), () => Go<CampScene>());

            KitsuneSprite();

            if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
                OutputText("Scenes.Kitsune.FightSomeKitsunes.PlayLikeThat".Localized());
            else
                if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                    OutputText("Scenes.Kitsune.FightSomeKitsunes.SpareTheTinies".Localized());
                else
                {
                    OutputText("Scenes.Kitsune.FightSomeKitsunes.NotMakeEasy".Localized());

                    OutputText("Scenes.Kitsune.FightSomeKitsunes.WryGrinCracksKnuckles".Localized());
                }

            OutputText("Scenes.Kitsune.FightSomeKitsunes.NowFightingA".Localized());

            if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
                OutputText("Scenes.Kitsune.FightSomeKitsunes.WordBlonde".Localized());
            else
                if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                    OutputText("Scenes.Kitsune.FightSomeKitsunes.WordBlackHaired".Localized());
                else
                    OutputText("Scenes.Kitsune.FightSomeKitsunes.WordRedHaired".Localized());

            OutputText("Scenes.Kitsune.FightSomeKitsunes.WordKitsune".Localized());
        }

        public void LoseToKitsunes()
        {
            ClearOutput();

            var scene = new List<Action>();

            if (Player.HasCock())
                scene.Add(() => LoseFightToHerms());

            if (Player.Gender != Gender.Male)
                scene.Add(() => FemalesAndNuetersLoseToKitsunes());

            if (CurrentMonster.Body.Get<HairPart>().Color == "blonde" && Player.CockTotal() == 1 && Player.BiggestCockLength() < 9)
                scene.AddRange(new Action[] { () => BlondeKitsuneRapesSmallWangs(), () => BlondeKitsuneRapesSmallWangs(), () => BlondeKitsuneRapesSmallWangs(), () => BlondeKitsuneRapesSmallWangs() });

            if (CurrentMonster.Body.Get<HairPart>().Color == "black" && Player.LactationQ() >= 50)
                scene.AddRange(new Action[] { () => LoseToBlackHairLatexWhileMilky(), () => LoseToBlackHairLatexWhileMilky(), () => LoseToBlackHairLatexWhileMilky(), () => LoseToBlackHairLatexWhileMilky() });

            if (Player.HasCock() && Player.Balls > 0 && Player.CumQ() > 1000)
                scene.AddRange(new Action[] { () => LoseToKitsunesWithBallsAndHighCumQ(), () => LoseToKitsunesWithBallsAndHighCumQ() });

            if (CurrentMonster.Body.Get<HairPart>().Color == "red")
            {
                scene.Add(() => GetRapedByRedHeadFutaKitsune());

                if (CurrentMonster.LustPercent > 50)
                    scene.Add(() => GetRapedByRedHeadFutaKitsune());

                if (CurrentMonster.LustPercent > 70)
                    scene.Add(() => GetRapedByRedHeadFutaKitsune());

                if (CurrentMonster.LustPercent > 85)
                    scene.Add(() => GetRapedByRedHeadFutaKitsune());
            }

            scene[Random.Shared.Next(scene.Count)]();
        }

        public void LoseFightToHerms()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.LoseFightToHerms.GiveUpEasily".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.TitteringLaughter".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.TimeToClaimPrize".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.HummingRemovesArmor".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.HoldingFirmlyCrawls".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.FunsOnlyJustBegun".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.ThreeTailsLasso".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.TailsOperateIndependently".Localized(
                 (CurrentMonster.Body.Get<HairPart>().Color == "blonde") ? "flowing golden locks cascading over her toned, voluptuous hourglass shape." : ((CurrentMonster.Body.Get<HairPart>().Color == "black") ? "a slight pudge around her belly accentuating her voluptuous figure." : "your eyes drawn immediately down to the pulsating, foot long member that now lays across your stomach.")
            ));

            OutputText("Scenes.Kitsune.LoseFightToHerms.HotAndBothered".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.LeansChestTails".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.MagicPermeating".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.TipOfTongue".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.PoorThingCroons".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.StruggleBushyCoils".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.WantItBadTeases".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.WhimperSlidesDown".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.EyesRollMoan".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.TonguePlungesMouth".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.PleasureTooMuch".Localized(
                 (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate"
            ));

            OutputText("Scenes.Kitsune.LoseFightToHerms.AlmostDearMore".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.LeansBackSupports".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.ClimaxTearsTailsSlacken".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.LoseFightToHerms.HotStickyStreams".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.LoseFightToHerms.HotRibbonsCervix".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.LoseFightToHerms.CockSwellsBaseTip".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.LoseFightToHerms.GlutSemenRises".Localized());
                        else
                            OutputText("Scenes.Kitsune.LoseFightToHerms.ThickVirileStreams".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.SighingSatisfaction".Localized());

            OutputText("Scenes.Kitsune.LoseFightToHerms.LastSightDelight".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void FemalesAndNuetersLoseToKitsunes()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.FinishedAlready".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.TitteringLaughter".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.HeheTimeToClaim".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.HummingRemovesArmor".Localized(
                 (Player.Gender == Gender.Female) ? "rapidly moistening cleft. Her fingers dance along the inside of your labia" : Player.AssholeDescript() + ". Her fingers dance along between your cheeks"
            ));

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.IndexFingerTiny".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.FunsOnlyJustBegun".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.SlidesForwardDaintily".Localized(
                 Player.IsBiped() ? "A pair of fluffy tails slides in between your legs, curling around them and spreading them up and apart, the remaining tails taking advantage of your helpless state and beginning to brush up and down the insides of your thighs, one tip sliding up and down " + ((Player.Gender == Gender.Female) ? "the sensitive cleft of your vagina" : "between the cheeks of your " + Player.ButtDescript()) + "." : "Her tails drift over your sensitive crotch, brushing the tip " + ((Player.Gender == Gender.Female) ? "along the cleft of your vagina" : "between the cheeks of your " + Player.ButtDescript()) + " and tickling it sensually.",

                 (CurrentMonster.Body.Get<HairPart>().Color == "blonde") ? "flowing golden locks cascading over her toned, voluptuous hourglass shape." : ((CurrentMonster.Body.Get<HairPart>().Color == "black") ? "a slight pudge around her belly accentuating her voluptuous figure." : "your eyes drawn immediately down to the pulsating, foot long member that now lays across your stomach.")
            ));

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.HotAndBothered".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.LeansChestTails".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.MagicFlow".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.TailsSpreadThird".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.PoorThingCroons".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.StruggleBushyCoils".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.WantItBadShowMe".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.GroanThrash".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.EyesRollSkyward".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.BrushlikeTailsCaress".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.SuddenChillSpine".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.ReactionExcitedMoan".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.AlmostDearMore".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.GrindingHipsFront".Localized());

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.BearsDownTails".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.FemalesAndNuetersLoseToKitsunes.LastSightDelight".Localized());

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void BlondeKitsuneRapesSmallWangs()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.PrecociousClosesIn".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.ShhDontResist".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.FingersTrailCheek".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.PullsWithoutHands".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.ItsSoCute".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.MemberThrobsInvoluntary".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.NoBadFlick".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.TailsSnakeForward".Localized(
                 Player.CockHead()
            ));

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.MoreLikeIt".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.SatisfiedGrinSitsUp".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.BetterLookStraddles".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.BodyShuddersSoft".Localized(
                 Player.CockHead()
            ));

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.LickMamaPussy".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.TailsFlexArms".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.NoBehaveNaughty".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.TonguePlumbsSuffer".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.FinallyTearAway".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.SatisfiedLowersFront".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.MmHeyLilGuy".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.RightThereGood".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.NodAgreementRealizing".Localized(
                 Player.CockHead()
            ));

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.ForceTongueDeeper".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.LipsRiseFall".Localized(
                 (Player.Balls > 0) ? " and reaching down to gently stroke your " + Player.BallsDescriptLight() : "",

                 (Player.Gender == Gender.Hermaphrodite) ? ", one of her long, dexterous fingers sliding into your " + Player.VaginaDescript() : ""
            ));

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.MoanMuffledTwat".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.PussyConvulses".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.CockTwitchesSpurts".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.SpurtHotStrings".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.MilkyRibbonsSplatter".Localized());
                    else
                        if (Player.CumQ() < 2500)
                            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.ShaftSwellsJets".Localized());
                        else
                            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.ThickLoadSwells".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.ComingDownRaucous".Localized());

            OutputText("Scenes.Kitsune.BlondeKitsuneRapesSmallWangs.LastSightSpell".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToBlackHairLatexWhileMilky()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.DropStruggling".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WordFatigue".Localized());
            else
                OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WordLust".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.CollapseCushioned".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.SmirksQuicklyWork".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WonderfulLuck".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.FlintFingertips".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WontLastLong".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WrapsLips".Localized(
                 (Player.Cocks.Count > 0) ? Player.SMultiCockDesc() + " becoming rock hard" : "",

                 (Player.Cocks.Count > 0) ? "[pg]Smirking happily, she lifts the back portion of her robes to expose her expansive bottom, lowering the jiggling cheeks against [eachCock]. A few of her tails snake down to coil around " + ((Player.Cocks.Count > 1) ? "them" : "it") + ", holding " + ((Player.Cocks.Count > 1) ? "them" : "it") + " against her ass and squeezing tightly as they begin to pump up and down slowly. Though you cannot see it from this angle, you can feel her colossal cheeks quivering sensually around [eachCock], jiggling enticingly with every movement." : ""
            ));

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.SayFragment".Localized(
                 ((int)Player.Gender >= 2) ? ((Player.Gender == Gender.Hermaphrodite) ? "Another" : "A") + " silken black tail slithers down between your legs, tickling your groin teasingly. It circles your [vagina] slowly, brushing the tip up against your [clit] and gently toying with the sensitive lips, occasionally pressing up against your entrance and yet never actually penetrating you. The torment is unbearable, but you are helplessly pinned underneath her, so all you can do is accept her tortuous teasings.[pg]" : ""
            ));

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.TongueLapsSwirls".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.GrowingTension".Localized(
                 (Player.Cocks.Count > 0) ? ". [eachCock] twitch" + ((Player.Cocks.Count > 1) ? "" : "es") + ", spurting cum up between the kitsune's ass cheeks and soaking her lower back with your virile spunk" : "",

                 ((int)Player.Gender >= 2) ? ((Player.Gender == Gender.Female) ? "Every" : ", while every") + " muscle in your neglected fuckhole tenses up, squeezing wantonly at nothing as your fluids rush out into a puddle beneath you" : ""
            ));

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.OrgasmMilkFountain".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.DivineProclaims".Localized());

            OutputText("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WhispersIncantationStrange".Localized());

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            Player.BoostLactation(1.5f);

            if (Player.HasPerk(PerkIds.Feeder))
            {
                Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
            }

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToKitsunesWithBallsAndHighCumQ()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.BeatenFallGround".Localized(
                 (Player.Tallness > 96) ? ", her face going beet red with the strain as she finally heaves your half-comatose frame into position" : ((Player.Tallness > 60) ? ", openly grunting with effort" : "")
            ));

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.LetsJustSeeMuses".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.CoyGrinDelighted".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.ThickOneBrewing".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.SnapsFingersBlue".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.TryFailSuppress".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.DoubleFistedGrip".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.GigglingShuddering".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.PullsBackPlushLips".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.MnHowThatFeel".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.EyelashesFlutter".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.OtherArmAbandons".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.DemonstrateTongueSkill".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.DigitFindsProstate".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.NaughtyNaughtyCum".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.StrugglesPutToRest".Localized());

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.DeferenceDesperate".Localized());

            if (Player.CumQ() <= 2500)
            {
                OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.SecondStreamsHit".Localized());

                OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.NotBadForMortal".Localized());
            }
            else
                if (Player.CumQ() <= 6000)
                {
                    OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.MoansDeepMoment".Localized());

                    OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.SoThickSoFull".Localized());
                }
                else
                {
                    OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.BucksForwardSemen".Localized());

                    OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.SoMuchStuffed".Localized());
                }

            OutputText("Scenes.Kitsune.LoseToKitsunesWithBallsAndHighCumQ.CockFlagEnhance".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            if (Player.CumQ() < 30000)
                Player.CumMultiplier += 5;

            CombatScene.CleanupAfterCombat();
        }

        public void GetRapedByRedHeadFutaKitsune()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.CollapseTooWeak".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.FinishedAlreadyThought".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.LightShrugCeases".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.ExamineNethers".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.SayFragment".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.SuitablySatisfiedTeasing".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.EndowmentNotFearsome".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.WordEntrance".Localized());
            else
                OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.WordAnus".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.RealizeFlamesNotBurn".Localized());

            if (Player.HasVagina())
                Player.CuntChange(24, true, true, false);
            else
                Player.ButtChange(24, true, true, false);

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.ShhJustRelax".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.WrappingArmsStuff".Localized());

            if (Player.HasVagina())
            {
                if (Player.VaginalCapacity() < 30)
                    OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.CombinationPainAgain".Localized());

                OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.PleasureClenchAgain".Localized());
            }
            else
            {
                if (Player.AnalCapacity() < 30)
                    OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.CombinationPainAgain".Localized());

                OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.PleasureClenchAgain".Localized());
            }

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.SheGroansGrippingYour".Localized());
            else
                OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.SheGroansGrippingYour".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.CoolnessFlamesWarmth".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.UsesAsFuckhole".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.YesYesGodsYes".Localized());

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.LiftedOffGround".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.FinallyCalmsPullsOut".Localized(
                 ((int)Player.Gender >= 2) ? Player.VaginaDescript() : Player.AssholeDescript()
            ));

            OutputText("Scenes.Kitsune.GetRapedByRedHeadFutaKitsune.LeansWhispersLanguage".Localized());

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatTheKitsunes(bool display = true)
        {
            ClearOutput();

            if (display)
            {
                KitsuneSprite();

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                {
                    OutputText("Scenes.Kitsune.DefeatTheKitsunes.HitsGroundOomph".Localized());

                    OutputText("Scenes.Kitsune.DefeatTheKitsunes.OnlyWantedToPlay".Localized());
                }
                else
                    OutputText("Scenes.Kitsune.DefeatTheKitsunes.FallsHandRobes".Localized());
            }

            ClearButtons();

            if (Player.HasCock())
                AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonFuckHerVag".Localized(), FuckAKitsuneVaginally);
            else
                AddNextButtonDisabled("FuckHerVag", "This scene requires you to have a cock.", "Fuck Her Vag");

            if (Player.HasCockThatFits(144))
                AddNextButton("Common.Button.FuckAss".Localized(), PutItInAKitsunesAssWin);
            else
                AddNextButtonDisabled("Fuck Ass", "This scene requires you to have a fitting cock.", "Fuck Her Ass");

            AddNextButton("Common.Button.Titfuck".Localized(), KitsuneTitjobWin);

            if (Player.HasVagina())
                AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), TribbingWithAKitsune);
            else
                AddNextButtonDisabled("Tribbing", "This scene requires you to have a vagina.", "");

            if (Player.HasCock())
                AddNextButton("Common.Button.Tailjob".Localized(), TailJobKitsuneWin);
            else
                AddNextButtonDisabled("Tailjob", "This scene requires you to have a cock.", "");

            if (Player.CountCocksOfType("TENTACLE") >= 3)
                AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), KitsunesGetBonedBy3PlusTentacles);
            else
                AddNextButtonDisabled("Tentacles", "This scene requires you to have at least 3 tentacle cocks.", "");

            AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonLustRide".Localized(), KitsuneDraftRide);

            if (CurrentMonster.Body.Get<HairPart>().Color == "blonde")
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft)) && Player.HasCock())
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonUseDraft".Localized(), FuckDraftBlond);
                else
                    AddNextButtonDisabled("Use F.Draft", "This scene requires you to have a cock and a bottle of fuck draft.", "Use Fuck Draft");

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid)) && Player.HasCock())
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonUseLactaid".Localized(), LactaidDoseAKitSune);
                else
                    AddNextButtonDisabled("Use Lactaid", "This scene requires you to have a cock and a bottle of lactaid.", "Use LactAid");

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)) && Player.HasCock())
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonUseOviElixer".Localized(), DoseAKitsuneWithOviElixirs);
                else
                    AddNextButtonDisabled("Use OviElixer", "This scene requires you to have a cock and a bottle of ovi elixir.", "Use Ovi Elixir");
            }

            if (CurrentMonster.Body.Get<HairPart>().Color == "black")
            {
                if (Player.HasCockThatFits(144))
                    AddButton(1, "Common.Button.Hotdoganal".Localized(), HotdogAnalInKitsuneButtDontLetTailTickleYourNose);
                else
                    AddButton(1, "Common.Button.Hotdoganal".Localized(), null);

                if (Player.HasVagina())
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonGetLicked".Localized(), GetLickedByKitsunes);
                else
                    AddNextButtonDisabled("GetLicked", "This scene requires you to have a vagina.", "Get Licked");

                if (Player.HasCockThatFits(108))
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), GetABJFromAFoxGirl);
                else
                    AddNextButtonDisabled("Get BJ", "This scene requires you to have a fitting cock.", "");
            }

            if (CurrentMonster.Body.Get<HairPart>().Color == "red")
            {
                if (Player.HasCockThatFits(144))
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonBondage".Localized(), NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh);
                else
                    AddNextButtonDisabled("Bondage", "This scene requires you to have a fitting cock.", "");

                if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) == 0 && Player.HasCock())
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonLapHJ".Localized(), NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow);
                else
                    AddNextButtonDisabled("Lap HJ", "This scene requires you to have a fitting cock. She should be pure female.", "");

                if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0 && Player.IsHerm())
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonHermHelix".Localized(), HelixZeKitsunes);
                else
                    AddNextButtonDisabled("Herm Helix", "This scene requires you both to be herms.", "");

                if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) == 0)
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonGrowDick".Localized(), BringBackDick);
                else
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonDitchDick".Localized(), RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong);

                if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonRideHerCock".Localized(), RideDatRedheadKitsuneCockIntoTheSkyDiamonds);
                else
                    AddNextButtonDisabled("RideHerCock", "This scene requires you to have a vagina. She should be herm.", "Ride Her Cock");

                if (CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0 && Player.HasVagina() && Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                    AddNextButton("Common.Button.BikiniTitfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));
            }

            if (Player.HasPerk(PerkIds.Feeder) || Player.LactationQ() >= 2000)
                AddNextButton("Common.Button.Breastfeed".Localized(), FeederTheKitsunes);
            else
                AddNextButtonDisabled("Breastfeed", "This scene requires you to have a very high milk amount.", "");

            if ((silly) && (CurrentMonster.Body.Get<HairPart>().Color == "red" || CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) == 0))
                    AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonDrinking".Localized(), KitsuneDrinking);

            AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonFluffTail".Localized(), KitsuneTouchTail);

            AddButton(14, "Common.Button.Leave".Localized(), () => LeaveKitsune());
        }

        public void FuckAKitsuneVaginally()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.ApproachCautiously".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.OhMyBlushing".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.FingertipsTrailHair".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.DontThinkBestOfMe".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.OnlySmirkDragging".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.StopYoull".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.IllWhatCutOff".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.TossArmorDropping".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.CockRisesBetweenLegs".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.WhatWaitingFor".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.PullArmPress".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.JustThisOnce".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.SmirkingLiftLeg".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.WarmInvitingPussy".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.WildBuckingTails".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.DontStopHarder".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.IndulgeCoquettish".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.TailConstrictsTighter".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.Red".Localized(
                 (CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0) ? "Her cock throbs and twitches in your grip, " + ((Player.Corruption > 15) ? "on the verge of climax. Just as she is about to achieve release, you tighten your grip around her base, smirking mischievously. See how <i>she</i> likes being tricked![pg]Her shaft swells inordinately beneath your hand, throbbing and twitching in vain as she tries desperately to overcome the blockage. Her face contorts into a grimace of pain with the slightest hint of masochistic bliss. She squirms and writhes in your grip, and finally you decide that she's had enough. You lean back and angle her twitching rod away from yourself, releasing your grip, and her cock begins to spasm uncontrollably," : "") + "slinging hot futa-jizz into the air and coating her chest and face with creamy white strings of spunk. She groans powerfully, bucking her hips in ecstasy as she pumps load after load of thick jizz onto herself and plasters her front completely with her own thick cum. " : ""
            ));

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.VaginalWallsConvulse".Localized());
            else
                if (Player.CumQ() < 350)
                    OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.PussySqueezesTightly".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.MemberSwellsObscenely".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.PussyConvulsesRipples".Localized());
                        else
                            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.ShaftDistendsSpunk".Localized());

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.GroanPullBack".Localized(
                 (CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0) ? " Her cock hangs limply over her cum-" + ((Player.CumQ() > 1000) ? "bloated" : "soaked") + " stomach, still leaking copious amounts of futa-jizz over her thigh. It twitches gently, giving one final spurt before laying to rest." : ""
            ));

            OutputText("Scenes.Kitsune.FuckAKitsuneVaginally.FinallyTurnGather".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void PutItInAKitsunesAssWin()
        {
            ClearOutput();

            KitsuneSprite();

            var x= Player.CockThatFits(144);

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.GazeCascades".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.FirmExploratorySlap".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.WryGrinBigCock".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.NotComplainingNeither".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.SecondTailDips".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.PullsHipsForward".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.DontActSurprised".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.BeginThrustRocks".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.SensingOrgasmTightens".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.JustGettingStarted".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.FeelSwelling".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.TailTipGlide".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.RemarkableFlexibility".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.EnoughHadHerFun".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.YelpsWrestControl".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.GushingPussySoak".Localized(
                 Player.CockDescript(x)
            ));

            if (CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0)
            {
                OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.CockThrobsGrip".Localized());

                if (Player.Corruption > 15)
                    OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.VergeOfClimax".Localized());

                OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.ThickRopesShooting".Localized());
            }

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.MoreCumThanThought".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.ThickRopesExplode".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.MassiveGlobsStretch".Localized());
                    else
                        if (Player.CumQ() < 2500)
                            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.EveryMuscleResonates".Localized());
                        else
                            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.CockSwellsBloats".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.TongueHangsOut".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.NotBadForMortal".Localized());

            OutputText("Scenes.Kitsune.PutItInAKitsunesAssWin.TakeMomentRecuperate".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void KitsuneTitjobWin()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.CantHelpGrinIdeas".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.ContentStandingThere".Localized(
                 Player.Mf("boy", "lover")
            ));

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.StrongChampionPurrs".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.GruntImpatient".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.GroanBuckHips".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.StudyFaceEyesShut".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.YourTurnGiggle".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.CeaseMassagingHum".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.AlmostFeelNeed".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.MindLostPleasure".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.PantHeavilyDeflating".Localized());

            OutputText("Scenes.Kitsune.KitsuneTitjobWin.SoonReducedShuddering".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void TribbingWithAKitsune()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.EyeSuspiciously".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.LaughsAwkwardlySmirking".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.HeatOfLoins".Localized(
                 (CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0) ? ((Player.Cocks.Count > 0) ? " Together with hers, your cocks twitch and bounce" : " Her cock twitches and bounces") + " against her stomach each time your hips ride forward, a thread of precum connecting to her navel from the tip." : ""
            ));

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.ClitPassesAcross".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.QuicklyFlipAround".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.SillyMortalPosition".Localized(
                 (Player.Cocks.Count > 0) ? Player.SMultiCockDesc() + " drape" + ((Player.Cocks.Count > 1) ? "s" : "") + " over the plump round cheeks of her prodigious posterior, leaking strands of precum onto her bundle of luxurious tails" + ((CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0) ? ", while her own member dangles a few inches above her nose." : ".") : ""
            ));

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.RockHipsClitoris".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.ThatsTheSpot".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.MatchesGrindingSextet".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.AzureTonguesLick".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.JoinsLewdChorus".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.CoilingSpringTension".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.EveryMusclePassion".Localized());

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.CoilingSpringSnaps".Localized(
                 (CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0) ? " Twitching and swelling with pleasure, " + ((Player.Corruption > 15) ? "she groans deeply in anticipation of her nearing climax. Just as she is about to achieve release, you tighten your grip around her base, smirking mischievously. See how <i>she</i> likes being tricked![pg]Her shaft swells inordinately beneath your hand, throbbing and twitching in vain as she tries desperately to overcome the blockage. Her face contorts into a grimace of pain with the slightest hint of masochistic bliss. She squirms and writhes in your grip, her eyes rolling back and her tongue hanging out.[pg][say: N-no fair! L-let me cum! F-fuck, please!][pg]You lean back and angle her twitching rod away from yourself, releasing your grip, and " : "") + "her swollen shaft begins to dump load after load of thick seed onto her face, spraying out in ribbons and globs. She shakes her head from side to side, unable to avoid her own virile emissions, pearly white sheets of semen sloughing off her cheeks as she sputters, laughing self-deprecatingly." : ""
            ));

            OutputText("Scenes.Kitsune.TribbingWithAKitsune.ClimaxEbbSlump".Localized());

            Player.ChangeClitLength(0.1f + Random.Shared.Next(3) / 10);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void TailJobKitsuneWin()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.EyingSilkyTails".Localized());

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.LevelOfControl".Localized(
                 (Player.Cocks.Count > 1) ? " Her remaining tails begin to weave in between your cocks, curling and wrapping around the shafts. As they slide up and down, they begin to gently constrict, acting like furry cock rings." : " Her remaining tails wrap themselves around your member, sliding up and down along the shaft. One of them coils around your " + (Player.Cocks.Exists(c => c.SheathOn) ? "sheath" : "base") + " and begins to constrict it gently, acting like a furry cock ring."
            ));

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.SillyMortalTails".Localized());

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.StammerProtestGiggles".Localized());

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.SeekingSomethingHands".Localized());

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.HeightOfCycle".Localized(
                 (Player.Balls > 0) ? "the underside of your " + Player.BallsDescriptLight() : "the " + (Player.Cocks.Exists(c => c.SheathOn) ? "sheath" : "base") + " of your " + Player.CockDescript(0)
            ));

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.IntenseTinglingNeeded".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.TailJobKitsuneWin.ThriceProlongedExplodes".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.TailJobKitsuneWin.RibbonsSplashForehead".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.TailJobKitsuneWin.DrenchFaceStreamers".Localized());
                    else
                        if (Player.CumQ() < 2500)
                            OutputText("Scenes.Kitsune.TailJobKitsuneWin.LoadEruptsFace".Localized());
                        else
                            OutputText("Scenes.Kitsune.TailJobKitsuneWin.UtterlySoakEjaculations".Localized());

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.DifficultToSayEscape".Localized());

            OutputText("Scenes.Kitsune.TailJobKitsuneWin.LookMeUpAnytime".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void KitsunesGetBonedBy3PlusTentacles()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.SlowlyApproachFallen".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.PinDownUndress".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.TrailFingertipsSkin".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.SoftlyStrokeHairs".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.WrithingTentaclesFree".Localized(
                 NumberUtils.Num2Text(Player.CountCocksOfType("TENTACLE"))
            ));

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.TentaclesReverent".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.TentaclesCoilInstinctively".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.StrongThickTongue".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.TwitchesTendrilsUnder".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.TremblingMoanEscapes".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.LargestPartsPussy".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.ObligeMoanFoot".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.FingersToesCurl".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.GarbledMoanAir".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.HandsTrembleReaching".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.HandsGlideDeeper".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.MmFhumming".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.HotGushFemcum".Localized(
                 (CurrentMonster.Body.Get<HairPart>().Color == "red" && CoC.Instance.Flags.Get(GameFlagIds.RedheadIsFuta) > 0) ? "Her cock throbs and twitches in the coils of your tentacles, " + ((Player.Corruption > 15) ? "on the verge of climax. Just as she is about to achieve release, you cinch the tendril up around her base, grinning. See how <i>she</i> likes being tricked![pg]Her shaft swells inordinately below the coil of vine, throbbing and twitching in vain as she tries desperately to overcome the blockage. Her face contorts into a grimace of pain with the slightest hint of masochistic bliss. She squirms and writhes in your grip, muffled pleas gurgling around the tentacle in her throat, and finally you decide that she's had enough. You lean back and angle her twitching rod away from yourself, releasing your grip, and her cock begins to spasm uncontrollably," : "") + "a thick ribbon of cum erupting from her twitching cock and hanging in the air suspensefully before plummeting onto her face. It is the first of many such streams that spurt from her swollen phallus like a lewd fountain, coating her body with a generous sheen of pearly jizz." : "",

                 (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate"
            ));

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.MoanEcstasyThickLoad".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.ThinRibbonsWarm".Localized());
            else
                OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.DumpingHotStreams".Localized());

            if (Player.CumQ() > 350)
                OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.BasesDistend".Localized());

            if ((Player.CumQ() > 350) && (Player.CumQ() <= 1000))
                OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.HotGlobulesDeposited".Localized());

            if ((Player.CumQ() > 1000) && (Player.CumQ() < 2500))
                OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.DumpLoadAfterLoad".Localized());

            if (Player.CumQ() > 2500)
                OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.LiterAfterLiter".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.PantingHoldClose".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.LazesGrassStupor".Localized());

            OutputText("Scenes.Kitsune.KitsunesGetBonedBy3PlusTentacles.TentaclesRepeats".Localized());

            Player.Orgasm("Dick", true, 3);

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void KitsuneDrinking()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.KitsuneDrinking.HavingBeatenNotMood".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.MullsPullsBottle".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.OddOfferSoundsFun".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.BurningMoreDrinks".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.SultryGazeMassage".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), KitsuneDrinking2);

            AddButton(1, "Common.Button.No".Localized(), KitsuneDrinkingNo);
        }

        public void KitsuneDrinkingNo()
        {

            OutputText("Scenes.Kitsune.KitsuneDrinkingNo.PerhapsMoreControl".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void KitsuneDrinking2()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.KitsuneDrinking.WhatTheHellStrip".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.FinishesDrinkCrawls".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.MyTurnNow".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.DreadTurnOver".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.LowerChimes".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.LoweerrrGroans".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.HandsRubButt".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.DeeperDarling".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.DiveFingerCunt".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.DeeperBoldSlip".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.FistInside".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.StrangeActMisunderstood".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.DeeEeEeper".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.ArmExitingCavernous".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.ItsAWarhammer".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.CatchesBreathOrgasm".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.StareDisbelief".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.PocketDimension".Localized());

            OutputText("Scenes.Kitsune.KitsuneDrinking.BlushingLookingAway".Localized());

            Player.ChangeFatigue(-20);

            Player.Inventory.Add(ItemRegistry.Create(WeaponIds.Warhammer), 1);

CombatScene.CleanupAfterCombat();
        }

        public void FuckDraftBlond()
        {
            ClearOutput();

            KitsuneSprite();

            var cockIndex = -1;

            var cockCounter = 0;

            while (cockCounter < Player.CockTotal() && cockIndex < 0)
            {
                if (Player.Cocks[cockCounter].Id == BodyPartTypes.Dog || Player.Cocks[cockCounter].HasKnot)
                    cockIndex = cockCounter;

                cockCounter += 1;
            }

            if (cockIndex < 0)
                cockIndex = Player.BiggestCockIndex();

            OutputText("Scenes.Kitsune.FuckDraftBlond.SmirkMischievouslyIdea".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.WaitWhatAreYouDo".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.CriesProtestWrestle".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.PowerfulScentMusk".Localized(
                 (Player.HasKnot(cockIndex)) || Player.InRut ? " As the potent smell fills your nostrils, your [cock " + cockIndex + 1 + "] swells in anticipation, reacting instinctively to the scent of a female in heat. Lurid fantasies of plugging her sloppy snatch with your " + ((Player.HasKnot(cockIndex)) ? "[knotword " + cockIndex + 1 + "]" : Player.CockDescript(cockIndex)) + " and stuffing her belly full of kits rush into your mind unbidden" + ((Player.Corruption < 20) ? ", disturbing you slightly" : "") + "." : ""
            ));

            OutputText("Scenes.Kitsune.FuckDraftBlond.LookMildAmusement".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.WhatDidYouDo".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.FaceTwistsGrimace".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.CheekMashedSquirms".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.WhyIsntWorking".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.GrinDeclarationSliding".Localized());

            if (Player.HasKnot(cockIndex) || Player.InRut)
                OutputText("Scenes.Kitsune.FuckDraftBlond.SweetMuskFemJizz".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.SlideUpBetweenMounds".Localized(
                 cockIndex,

                 1
            ));

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText(Player.Legs());

            if (silly)
                OutputText("Scenes.Kitsune.FuckDraftBlond.HeatRoastMarshmallows".Localized());
            else
                OutputText("Scenes.Kitsune.FuckDraftBlond.HeatSendsBlood".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.RaisesHipsWhimpering".Localized(
                 cockIndex,

                 1
            ));

            OutputText("Scenes.Kitsune.FuckDraftBlond.StopTeasingBreathless".Localized(
                 cockIndex,

                 1
            ));

            if (Player.HasKnot(cockIndex) || Player.InRut)
            {
                OutputText("Scenes.Kitsune.FuckDraftBlond.AnimalisticNeedImpregnate".Localized());

                if (Player.CockArea(cockIndex) > 50)
                    OutputText("Scenes.Kitsune.FuckDraftBlond.HeedlessHandleAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Kitsune.FuckDraftBlond.NeedyThrustsEndear".Localized());

                if (Player.CockArea(cockIndex) > 50)
                    OutputText("Scenes.Kitsune.FuckDraftBlond.HeedlessHandleAgain".Localized());
            }

            OutputText("Scenes.Kitsune.FuckDraftBlond.InsidesFurnace".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.JetGirlcumSplashes".Localized(
                 (Player.Balls > 0) ? Player.BallsDescriptLight() : Player.Legs()
            ));

            OutputText("Scenes.Kitsune.FuckDraftBlond.NngahFeelsSoGood".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.DigFingertipsAss".Localized());

            if (Player.HasKnot() || Player.InRut)
                OutputText("Scenes.Kitsune.FuckDraftBlond.VividFantasiesStuffing".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.ForceMashesDerriere".Localized());

            if (Player.HasKnot())
            {
                OutputText("Scenes.Kitsune.FuckDraftBlond.LipsSlipperyStretch".Localized(
                     cockIndex,

                     1,

                     cockIndex,

                     1
                ));

                OutputText("Scenes.Kitsune.FuckDraftBlond.RollsEyesCacophony".Localized());

                OutputText("Scenes.Kitsune.FuckDraftBlond.DespiteDifficulties".Localized());
            }

            OutputText("Scenes.Kitsune.FuckDraftBlond.GatheringTailsArms".Localized(
                 (Player.HasKnot(cockIndex)) ? "knot" : Player.CockDescript(cockIndex)
            ));

            OutputText("Scenes.Kitsune.FuckDraftBlond.PleasureRiseHead".Localized(
                 (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate"
            ));

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.FuckDraftBlond.SpasmsSpewing".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.FuckDraftBlond.StickyRibbonsFire".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.FuckDraftBlond.UrethraDistendsLoad".Localized());
                    else
                        if (Player.CumQ() < 2500)
                            OutputText("Scenes.Kitsune.FuckDraftBlond.ShaftSwellsGobs".Localized());
                        else
                        {
                            OutputText("Scenes.Kitsune.FuckDraftBlond.HearCumPouring".Localized());

                            if (Player.HasKnot(cockIndex))
                                OutputText("Scenes.Kitsune.FuckDraftBlond.BellyBalloons".Localized());
                            else
                                OutputText("Scenes.Kitsune.FuckDraftBlond.SheerVolumeMore".Localized());

                            OutputText("Scenes.Kitsune.FuckDraftBlond.DespiteBeachBall".Localized());
                        }

            OutputText("Scenes.Kitsune.FuckDraftBlond.WombFullFreshSemen".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.SayFragment".Localized(
                 (Player.HasKnot()) ? " You try to pull out, but are a little dismayed to find that her pussy still has a vice grip on your distended knot. Sighing, you collapse on top of her, enjoying the warmth of her flesh " + ((Player.Corruption < 33) ? "while gently stroking her golden hair " : "") + "as you wait for the swollen bulge to deflate." : ""
            ));

            OutputText("Scenes.Kitsune.FuckDraftBlond.SmallWhimperSlides".Localized());

            OutputText("Scenes.Kitsune.FuckDraftBlond.CantBeCertainAsleep".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.FuckDraft));

            CombatScene.CleanupAfterCombat();
        }

        public void DoseAKitsuneWithOviElixirs()
        {
            ClearOutput();

            KitsuneSprite();

            var x= Player.CockThatFits(100);

            if (x < 0)
                x = Player.BiggestCockIndex();

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.FishHexagonalBottle".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.HoldingBottleStopper".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.MyStomachGroans".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.PantingLeansBack".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.PlainToSeeMailing".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.SlidesDownTree".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.SinksDownStroking".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.StandThereGawking".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.WordsSnapReverie".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.GentlyInsertFinger".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.RealizingDelay".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.PreparePlunge".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.PelvisBouncesRipple".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.RelaxesSubmissively".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.HavingFunSoFull".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.SinkHandsSquishy".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.PleaseDrinkUp".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.OnlyReplyDrink".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.TinglingWorksCore".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.DontStopJustAbout".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.BodyTensesOrgasm".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.CockPushedPlop".Localized(
                 (Player.CumQ() > 350) ? "Streams of warm jism continue to spurt out of your twitching prick, arcing through the air and landing on her" + ((Player.CumQ() > 1000) ? " face, chest, and" : "") + " stomach with a chorus of wet slaps." : "A few more streams of spunk spurt out across the underside of her belly, painting it white with your stringy cum."
            ));

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.StomachDeflatesEggs".Localized());

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.TheyAre".Localized());

            Identifier itype = default;

            if (Random.Shared.Next(3) == 0)
                itype = ConsumableIds.LargeEggs[Random.Shared.Next(ConsumableIds.LargeEggs.Length)];
            else
                itype = ConsumableIds.SmallEggs[Random.Shared.Next(ConsumableIds.SmallEggs.Length)];

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.OstrichEggsExamine".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir));

            OutputText("Scenes.Kitsune.DoseAKitsuneWithOviElixirs.TakeCareEggies".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, itype.ToString());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void LactaidDoseAKitSune()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.LickLipsHungrily".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.OtherStuffPrank".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.WrestlingGroundBottle".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.BreastsBeginExpand".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.ChestFeelsHot".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.GrinRubPalms".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.PleasedResults".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.ThatFeelsSoft".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.HandsStrayAreolae".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.SlowlyThrustSlippery".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.OhMaraeFeels".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.ForearmsPressBreasts".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.PinchingBaseTeats".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.SquashJugsTogether".Localized());

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.BearingChestRedouble".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.LactaidDoseAKitSune.SpurtsBubbleBetween".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.LactaidDoseAKitSune.CavernousCleavageFilled".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.LactaidDoseAKitSune.PressureBuildsGroin".Localized());
                    else
                    {
                        OutputText("Scenes.Kitsune.LactaidDoseAKitSune.YourCockFragment".Localized());

                        if (Player.Cocks.Count > 1)
                            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.SSwellFragment".Localized());
                        else
                            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.SwellsFragment".Localized());

                        OutputText("Scenes.Kitsune.LactaidDoseAKitSune.UpIncrediblyThickLoads".Localized());

                        if (Player.CumQ() >= 2500)
                            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.SeemsClimaxForever".Localized());
                    }

            OutputText("Scenes.Kitsune.LactaidDoseAKitSune.GroansUnexpectedClimax".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));

            CombatScene.CleanupAfterCombat();
        }

        public void HotdogAnalInKitsuneButtDontLetTailTickleYourNose()
        {
            ClearOutput();

            KitsuneSprite();

            var x= Player.CockThatFits(144);

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.GazeWanderLecherously".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.CriesSurpriseManhandle".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.TailsSlideAway".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.YouLikeMyAss".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.FirmSlapMoans".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.MmnAgainSpank".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.HeartyCrackEchoes".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.GripMountainousCheeks".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.TailSlidesStomach".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.SqueezeHipsThrust".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.GoodAsMine".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.SheDoesDoesShe".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.MusclesRippleRod".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LoudSlappingHips".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.ReadjustBalanceResume".Localized());

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.GripLoveHandles".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LastSlapEchoes".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() <= 150)
            {
                OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.ConvulsingRectumMilk".Localized());

                OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LiesSpreadEagle".Localized());
            }
            else
                if (Player.CumQ() <= 350)
                {
                    OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.ConvulsingRectumMilkAgain".Localized());

                    OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LiesSpreadEagleAgain".Localized());
                }
                else
                    if (Player.CumQ() <= 1000)
                    {
                        OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.ConvulsingRectumMilkThird".Localized());

                        OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LiesSpreadEagleThird".Localized());
                    }
                    else
                        if (Player.CumQ() <= 2500)
                        {
                            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.ConvulsingRectumMilkFourth".Localized());

                            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LiesSpreadEagleFourth".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.ConvulsingRectumMilkFifth".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.LiesSpreadEagleFifth".Localized());
                        }

            OutputText("Scenes.Kitsune.HotdogAnalInKitsuneButtDontLetTailTickleYourNose.RollsOntoSideCurling".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void FeederTheKitsunes()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.FeederTheKitsunes.StandOverBeaten".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.SoHungry".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.HungryHaveJustThe".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.WincesBackTree".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.EyesGlitterMagnificently".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.MilkWhispers".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.TwinStreamsCream".Localized());

            if (CurrentMonster.Body.Get<HairPart>().Color == "black")
                OutputText("Scenes.Kitsune.FeederTheKitsunes.HairLustrousBlack".Localized());
            else
                OutputText(CurrentMonster.Body.Get<HairPart>().Color);

            OutputText("Scenes.Kitsune.FeederTheKitsunes.HairFragment".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.LooksUpBreasts".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.HandsKneadBreasts".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.ShiftPositionComfortable".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.OtherHandTravelsCurves".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.ScratchTailHairs".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.GrabWrist".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.ClutchBreastResistance".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.HandsOverBelly".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.WetterEachMoment".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.PumpFingersSlit".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.HeatSpreadsChest".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.DrankFullyCompletely".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.StuffedEyesRolled".Localized());

            OutputText("Scenes.Kitsune.FeederTheKitsunes.LayOnGroundDonning".Localized());

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            CombatScene.CleanupAfterCombat();
        }

        public void GetABJFromAFoxGirl()
        {
            ClearOutput();

            KitsuneSprite();

            var x= Player.CockThatFits(108);

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.HearBarelyAudible".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.RubsPudgyTummy".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.HungrySmirkHave".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.StrippingKneeling".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.PoutsCoylyLips".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.GrinningDipsUnder".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.SlursHardening".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.InchByInch".Localized(
                 Player.CockHead(x)
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.AdmitGluttonousSkills".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.SoLuckyCroons".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ShaftSwallowedBack".Localized(
                 Player.CockHead(x)
            ));

            if (Player.Cocks[x].Length > 9)
            {
                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.EachBobCloser".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.BallsDescriptLight());
                else
                    OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.SheathFragment".Localized());

                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.HungrilySwallowsLength".Localized());
            }
            else
                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.LipsSqueezing".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ThroatMusclesRipple".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ResumesDeepThroating".Localized(
                 (Player.Balls > 0) ? " and over your " + Player.BallsDescriptLight() : ""
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.TakeAdvantageFreedom".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.TrailingThemAcrossYour".Localized(
                 ((Player.Balls > 0) || (Player.Vaginas.Count > 0)) ? " Trailing them across your sides, she " + ((Player.Balls > 0) ? "cups your [balls] in her hand, squeezing tightly" + ((Player.Vaginas.Count > 0) ? " and " : "") : "") + ((Player.Vaginas.Count > 0) ? "runs her index finger teasingly along your slit" : "") + "." : ""
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ThroatRelaxSubmits".Localized());

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.EagerlyDrawInside".Localized(
                 (Player.Cocks[x].Length > 15) ? "Again and again, you thrust your [hips] forward, mashing your pelvis against her face. Your " + Player.CockDescript(x) + " glides down her throat, making her neck bulge outward, and her only response is to moan into it, swallowing hungrily. She massages your shaft sensually with her lips, using her tongue and throat" : "She massages your shaft sensually with her lips, using her tongue"
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.FeelTightnessSwelling".Localized(
                 (Player.Balls > 0) ? Player.BallsDescriptLight() : "prostate"
            ));

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.SucksDownHungrily".Localized());

            if (Player.CumQ() <= 150)
            {
                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.StreamAfterStream".Localized());

                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.AnyMoreAsks".Localized());
            }
            else
                if (Player.CumQ() <= 350)
                {
                    OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.PourRibbonAfterRibbon".Localized());

                    OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.DeliciousHappySmile".Localized());
                }
                else
                    if (Player.CumQ() <= 1000)
                    {
                        OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ThickStreamersThroat".Localized());

                        OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.DivineSatisfiedGrin".Localized());
                    }
                    else
                        if (Player.CumQ() <= 2500)
                        {
                            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.TorrentDistends".Localized());

                            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.DivineSoMuch".Localized());
                        }
                        else
                            if (Player.CumQ() <= 6000)
                            {
                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ThickGlobsDistend".Localized());

                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.BabblesIncoherently".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.TremendousLoadBacks".Localized());

                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ThroatTightensPullBack".Localized());

                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.CupsHandsVainly".Localized());

                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.ChinSmeared".Localized());

                                OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.NeverHadMortalFill".Localized());
                            }

            OutputText("Scenes.Kitsune.GetABJFromAFoxGirl.SighingStepBack".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void GetLickedByKitsunes()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.WaryTrickeryForward".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.HandCaressHead".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.StareShinyPillows".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.BarelyRequiresCoercion".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.MouthBuriedFolds".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.EyesRollHint".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.HandsTrembleGroin".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.VergeClimaxLurch".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.WreathedFlamesTongue".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.WholeBodyShakes".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.TongueWriggleWalls".Localized());

            OutputText("Scenes.Kitsune.GetLickedByKitsunes.LicksLipsSatisfaction".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void RideDatRedheadKitsuneCockIntoTheSkyDiamonds()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.WatchDownedClosely".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.PushThighAside".Localized(
                 (Player.IsBiped() || Player.IsTaur()) ? "extending a " + Player.Foot() : "reaching down"
            ));

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.FlashesGrinTrail".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.HehehYouLike".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.TightSqueezeReply".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.DiscardArmorPile".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.OoOhTreatGirl".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.SmirkFingerShaft".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.WarmCanalConstricts".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.MoveIntoPosition".Localized(
                 Player.VaginaDescript()
            ));

            Player.CuntChange(12, true, true, false);

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.ParagraphBreak".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.HipsRiseFall".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.CockSwellsPulse".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.PelvisesMashing".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.PantingSlowsWane".Localized());

            OutputText("Scenes.Kitsune.RideDatRedheadKitsuneCockIntoTheSkyDiamonds.ColossalLoadDraining".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void HelixZeKitsunes()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.HelixZeKitsunes.EyeSulkingWarily".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.KnowsWhenBeat".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.ComeNowStare".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.AssJigglesMinute".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.AahFaceRed".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.SmirkRobustCurves".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.WrapFistBase".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.BarelyAudibleWhine".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.SatisfiedResults".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.HoldingWideHips".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.LiftRearContorting".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.ReachingThickPrick".Localized());

            Player.CuntChange(12, true, true, false);

            OutputText("Scenes.Kitsune.HelixZeKitsunes.ParagraphBreak".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.ShiveringQuivering".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.WailsFuckYes".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.FingersDigPlump".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.SlaveringPussyGrips".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.MashingHipsJoin".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.HelixZeKitsunes.AdmirablePlaySticky".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.HelixZeKitsunes.SpurtRibbonHungry".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.HelixZeKitsunes.GruntGroanThrobs".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.HelixZeKitsunes.FillEachOther".Localized());
                        else
                            OutputText("Scenes.Kitsune.HelixZeKitsunes.ClimaxOvershadowed".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.OnceRecoveredTakes".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.CocksPassEachOther".Localized());

            OutputText("Scenes.Kitsune.HelixZeKitsunes.TurnGatherArmor".Localized());

            Player.Orgasm("Dick");

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.SquintLeeringBulge".Localized());

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.SayFragment".Localized());

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.GrumbleJabFinger".Localized());

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.RollingEyesComplies".Localized());

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.SayStartFineBeat".Localized());

            if (silly)
                OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.YouMonster".Localized());

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.SayEndBreak".Localized());

            OutputText("Scenes.Kitsune.RedheadsDontDeserveToHavePenisesBecauseTheyreTooGayForPenisOrSomethingIDontReallyKnowHowThisWorksOrWhyThisFunctionNameIsSoFuckingLong.WhatWillYouDoNow".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RedheadIsFuta, 0);

            DefeatTheKitsunes(false);
        }

        public void BringBackDick()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.BringBackDick.LookOverContemplatively".Localized());

            OutputText("Scenes.Kitsune.BringBackDick.EyesLightUp".Localized());

            OutputText("Scenes.Kitsune.BringBackDick.LastWispsFlame".Localized());

            OutputText("Scenes.Kitsune.BringBackDick.WalkOnWildSide".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RedheadIsFuta, 1);

            DefeatTheKitsunes(false);
        }

        public void NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.BeatenGirlNoEffort".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.BushyTailBrushes".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.HooksFingersArmor".Localized(
                 (Player.Balls > 0) ? "She gently cradles your [sack] in one palm, " : ((Player.Gender == Gender.Hermaphrodite) ? "She gently cups one hand over your [vagina] and lightly inserts her middle finger, " : "She gently cups the underside of your groin, ")
            ));

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.PressesFaceGroin".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.MeltsUnderTouch".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.SashFallsAway".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.JawHangsSlack".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.NothingIfNotObedient".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.PitchesForwardTensing".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.HipsDrenchedPussy".Localized(
                 (Player.Cocks.Count > 1) ? " She is careful not to neglect your other " + ((Player.Cocks.Count > 2) ? "members" : "member") + ", squeezing them together and massaging them " + ((Player.Cocks.Count > 2) ? "all" : "both") + " eagerly." : ""
            ));

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.ImGonnaStutter".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.ShuddersEyesRolling".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.MayPayOffTension".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.StickyThreadsSplatter".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.HotStreamersSplatter".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.ThickStreamersSplatter".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.HotRibbonsSplatter".Localized());
                        else
                            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.ThickJetsCrash".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadIsWorstRedheadLapsittingHandjobThingIDontKnow.ShutsEyesRests".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            CombatScene.CleanupAfterCombat();
        }

        public void NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh()
        {
            ClearOutput();

            KitsuneSprite();

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.AsYouCloseIn".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.WhWhatAreWe".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.FingerToLips".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.UtterlyDisarmedMeth".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.PalmTrailsSmall".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.ReachPileClothes".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.CuriouslyPluck".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.LimbSturdySteadfast".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.DecidedForeplayCasually".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.AgainAndAgainPound".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.BuckingFrenzy".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.SettleRhythmPummeling".Localized());

            if (Player.CumQ() <= 150)
                OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.PushedOverEdge".Localized());
            else
                if (Player.CumQ() <= 350)
                    OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.TumblingOverEdge".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.TumbleOffPrecipice".Localized());
                    else
                        if (Player.CumQ() <= 2500)
                            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.TumbleOffPrecipiceAgain".Localized());
                        else
                            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.TumbleOffPrecipiceThird".Localized());

            OutputText("Scenes.Kitsune.NonFutaRedHeadBondageIGuessYouTieHerUpWithYourPenisThenHuh.InsensateBlissSways".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void KitsuneDraftRide()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.KitsuneDraftRide.GrinLookFallen".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.PleaseUnableFinish".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.PleadingLookWhip".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.EffectsPotionApparent".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.GulpsEyesHazy".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.RidArmorQuickly".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.BeforeEvenLower".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.VantageHandsRush".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.NowTakingCareFree".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.FoxSteadyRhythm".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.OrgasmSpreadsKitsune".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.TwoLiePanting".Localized());

            OutputText("Scenes.Kitsune.KitsuneDraftRide.LeaveHearMoan".Localized());

            Player.Orgasm("Vaginal");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.FuckDraft));

            CombatScene.CleanupAfterCombat();
        }

        public void KitsuneTouchTail()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.KitsuneVision))
                OutputText("Scenes.Kitsune.KitsuneTouchTail.YouReTooFar".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.TellOneThingOnly".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.LoveWhenDirect".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.HandsClosePrize".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.DontBeTease".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.GrabHoldTail".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.AreYouAlright".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.ContinuesLightMoans".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.SureYouKnow".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.TwoAtOnceShock".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.CantBelieveSoft".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.KitsuneVision))
            {
                OutputText("Common.Text.NNbutThatWasFarToo".Localized());

                AddNextButton("Common.Button.Next".Localized(), KitsuneTouchTailBadEnd);
            }
            else
            {
                OutputText("Scenes.Kitsune.KitsuneTouchTail.AlmostTooMuch".Localized());

                OutputText("Scenes.Kitsune.KitsuneTouchTail.RestWearyHeadTails".Localized());

                OutputText("Scenes.Kitsune.KitsuneTouchTail.NodOffSoon".Localized());

                DoNext(() => KitsuneTouchTail2());
            }

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 10f));
        }

        public void KitsuneTouchTailBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.KitsuneTouchTailBadEnd.YouStartleAwakeA".Localized());

            OutputText("Common.Text.NNyouSighInRelief".Localized());

            OutputText("Common.Text.NNbutThenTheresAGiggle".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTailBadEnd.WhirlAroundMany".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTailBadEnd.StumbleCannotSee".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.KitsuneVision);

            OutputText("Scenes.Kitsune.KitsuneTouchTailBadEnd.LastThingFadesBlack".Localized());

            GameOver();
        }

        public void KitsuneTouchTail2()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.KitsuneTouchTail.WakeSameSpot".Localized());

            OutputText("Scenes.Kitsune.KitsuneTouchTail.PatDownEverything".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void KitsuneShrine()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KitsuneShrineVisit) == 0)
            {
                OutputText("Scenes.Kitsune.KitsuneShrine.WindingPathShrine".Localized());

                OutputText("Scenes.Kitsune.KitsuneShrine.MindfulDangersCurious".Localized());

                OutputText("Scenes.Kitsune.KitsuneShrine.EmergeClearing".Localized());

                OutputText("Scenes.Kitsune.KitsuneShrine.CallOutAbandoned".Localized());

                OutputText("Scenes.Kitsune.KitsuneShrine.EyesAdjustDim".Localized());

                OutputText("Scenes.Kitsune.KitsuneShrine.LayerOfDust".Localized());
            }
            else
                OutputText("Scenes.Kitsune.KitsuneShrine.FindAbandonedShrine".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KitsuneShrineVisit, CoC.Instance.Flags.Get(GameFlagIds.KitsuneShrineVisit) + 1);

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.KitsuneShrine.ButtonReadBooks".Localized(), ReadKitsuneBooks);

            if (CoC.Instance.Flags.Get(GameFlagIds.TookKitsuneStatue) == 0)
                AddButton(1, "Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), MeditateLikeAKitsuneEhQuestionMark);

            if (Player.HasItem(UseableIds.GoldStatue) || CoC.Instance.Flags.Get(GameFlagIds.TookKitsuneStatue) == 0)
                AddButton(2, "Scenes.Kitsune.KitsuneShrine.ButtonStatue".Localized(), StealAStatue);

            if (CoC.Instance.Flags.Get(GameFlagIds.TookKitsuneStatue) == 0)
                AddNextButton("Scenes.Kitsune.KitsuneShrine.ButtonDonation".Localized(), Donation);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void ReadKitsuneBooks()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.ReadKitsuneBooks.PeruseOldTomes".Localized());

            var choice= Random.Shared.Next(3);

            if (choice == 0)
            {
                OutputText("Scenes.Kitsune.ReadKitsuneBooks.DryButInformative".Localized());

                Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f));

                DoNext(() => ReturnToCamp(1));
            }
            else
                if (choice == 1)
                {
                    OutputText("Scenes.Kitsune.ReadKitsuneBooks.ReligiousTextSort".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Kitsune.ReadKitsuneBooks.FlipPagesBlush".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 5f));

                    DoNext(() => ReturnToCamp(1));
                }
        }

        public void MeditateLikeAKitsuneEhQuestionMark()
        {
            ClearOutput();

            RegisterMacro("incamp", StatueLocation != "");

            OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.SitDownCarefully".Localized());

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.FoxJewel)) && Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox && Player.Body.Get<TailPart>().Venom < 9 && Player.Body.Get<TailPart>().Venom + 1 <= Player.Level && Player.Body.Get<TailPart>().Venom + 1 <= Player.Intelligence / 10 && Player.Ears.Id == BodyPartTypes.Fox && (!Player.HasPerk(PerkIds.CorruptedNinetails) || Player.Perkv4(PerkIds.CorruptedNinetails) > 0) && (!Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.Perkv4(PerkIds.EnlightenedNinetails) > 0) && StatueLocation == "")
            {
                OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.NearingEndCompetent".Localized());

                if (Player.Body.Get<TailPart>().Venom < 8)
                {
                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.SilentReverieFlames".Localized(
                         Player.Body.Get<TailPart>().Venom + 1
                    ));

                    Player.Body.Get<TailPart>().Venom += 1;
                }
                else
                {
                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.FlamesWashMind".Localized());

                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.BushyTailsGlow".Localized());

                    Player.Body.Get<TailPart>().Venom = 9;

                    Player.CreatePerkIfNotHasPerk(PerkIds.EnlightenedNinetails, 0, 0, 0, 0);

                    if (!ColorLists.ElderKitsune.Contains(Player.Body.Get<HairPart>().Color))
                    {
                        if (Player.HasFur() && ColorLists.ElderKitsune.Contains(Player.Body.Get<SkinPart>().FurColor))
                        {
                            Player.Body.Get<HairPart>().Color = Player.Body.Get<SkinPart>().FurColor;

                            if (Player.Body.Get<HairPart>().Length > 0)
                                OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.HairMatchingFur".Localized());
                        }
                        else
                            if (Player.HasFur())
                            {
                                Player.Body.Get<HairPart>().Color = ColorLists.ElderKitsune[Random.Shared.Next(ColorLists.ElderKitsune.Length)];

                                Player.Body.Get<SkinPart>().FurColor = Player.Body.Get<HairPart>().Color;

                                if (Player.Body.Get<HairPart>().Length > 0)
                                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.FurAndHair".Localized());
                                else
                                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.FurLikeElder".Localized());
                            }
                            else
                            {
                                Player.Body.Get<HairPart>().Color = ColorLists.ElderKitsune[Random.Shared.Next(ColorLists.ElderKitsune.Length)];

                                Player.Body.Get<SkinPart>().FurColor = Player.Body.Get<HairPart>().Color;

                                if (Player.Body.Get<HairPart>().Length > 0)
                                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.HairLikeElder".Localized());
                            }
                    }
                    else
                        if (Player.HasFur() && !ColorLists.ElderKitsune.Contains(Player.Body.Get<SkinPart>().FurColor))
                        {
                            Player.Body.Get<SkinPart>().FurColor = Player.Body.Get<HairPart>().Color;

                            OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.FurMatchingHair".Localized());
                        }

                    OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.PauseReflectWisdom".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f), new DynStat(GameStats.Lust, -20f), new DynStat(GameStats.Corruption, -2f));
                }

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.FoxJewel));

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.SitSmallMat".Localized());

                OutputText("Scenes.Kitsune.MeditateLikeAKitsuneEhQuestionMark.RenewedVigorQuest".Localized());

                if (StatueLocation == "")
                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f), new DynStat(GameStats.Lust, -20f), new DynStat(GameStats.Corruption, -2f));
                else
                    if (Save.GetInt("statueDay", 0) != View.Time.Days)
                    {
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f), new DynStat(GameStats.Lust, -10f));

                        Save.SetInt("statueDay", View.Time.Days);
                    }

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void StealAStatue()
        {
            ClearOutput();

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.TookKitsuneStatue) == 0)
            {
                OutputText("Scenes.Kitsune.StealAStatue.ChanceTooGreat".Localized());

                AddButton(0, "Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), TakeAKitsuneStatue);
            }
            else
            {
                OutputText("Scenes.Kitsune.StealAStatue.EmptyAltarMissing".Localized());

                AddButton(0, "Scenes.Kitsune.StealAStatue.ButtonPutBack".Localized(), PutKitsuneStatueBack);
            }

            AddButton(14, "Common.Button.Back".Localized(), KitsuneShrine);
        }

        public void TakeAKitsuneStatue()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.TakeAKitsuneStatue.GemsThought".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            Player.Inventory.Add(ItemRegistry.Create(UseableIds.GoldStatue), 1);

ReturnToCamp(1);

            CoC.Instance.Flags.Set(GameFlagIds.TookKitsuneStatue, 1);
        }

        public void PutKitsuneStatueBack()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.PutKitsuneStatueBack.RegrettingReplace".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TookKitsuneStatue, 0);

            Player.ConsumeItem(UseableIds.GoldStatue);

            DoNext(() => ReturnToCamp(1));
        }

        public void KitsuneStatue()
        {

            OutputText("Scenes.Kitsune.KitsuneStatue.PullOutGoldStatue".Localized());

            OutputText("Scenes.Kitsune.KitsuneStatue.NotMuchUseDecoration".Localized());

            if (!CombatController.InCombat && !false  && (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0))
            {
                OutputText("Scenes.Kitsune.KitsuneStatue.DoYouSetUp".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => KitsuneStatueYes()); AddButton(1, "No", () => KitsuneStatueNo());
            }
            else
                Player.Inventory.Add(ItemRegistry.Create(UseableIds.GoldStatue));
        }

        public void KitsuneStatueNo()
        {

            OutputText("Scenes.Kitsune.KitsuneStatueNo.NoDisplayStolen".Localized());

            Player.Inventory.Add(ItemRegistry.Create(UseableIds.GoldStatue));
        }

        public void KitsuneStatueYes()
        {
            ClearOutput();

            StatueLocation = (CabinInterior.HasDesk ? "desk" : (CabinInterior.HasBookshelf  ? "bookshelf" : (CabinInterior.HasTable ? "table" : (CabinInterior.HasNightstand ? "nightstand" : (CabinInterior.HasDresser ? "dresser" : "corner")))));

            OutputText("Scenes.Kitsune.KitsuneStatueYes.SayFragment".Localized(
                 StatueLocation == "corner" ? "Finding nowhere else to put it, you set the statue down in the corner of your bedroom. While it might not be the most noble of spots, it's hardly any worse than the old, rotting shed you found it in." : "Glancing around for the best place to put it, you finally settle on your " + StatueLocation + ". You went through all the trouble to steal it, after all, so you might as well show it off."
            ));

            PlayerMenu();
        }

        public void KitsuneGenericFluff()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.NoShapeResist".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.NeedWrappedFur".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.PerhapsRealizes".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.HintPleasureShiver".Localized(
                 CombatController.Instance!.Context.Enemy.HasCock() ? CombatController.Instance!.Context.Enemy.CockDescript(0) : CombatController.Instance!.Context.Enemy.VaginaDescript(0)
            ));

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.TwirlTailsSpin".Localized(
                 CombatController.Instance!.Context.Enemy.BiggestBreastSize() >= 1 ? "cupping [monster.his] " + CombatController.Instance!.Context.Enemy.BreastDescript(0) + " while you" : "making sure to"
            ));

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.TremblesIndecision".Localized(
                 CombatController.Instance!.Context.Enemy.HasCock() ? CombatController.Instance!.Context.Enemy.CockDescript(0) : CombatController.Instance!.Context.Enemy.VaginaDescript(0)
            ));

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.IfWantsToTouch".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.DazedNod".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.FunToToy".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.MoanEchoes".Localized(
                 CombatController.Instance!.Context.Enemy.HasCock() ? CombatController.Instance!.Context.Enemy.CockDescript(0) + " twitches" : CombatController.Instance!.Context.Enemy.VaginaDescript(0) + " glitters"
            ));

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.BiteBackSounds".Localized(
             CombatController.Instance!.Context.Enemy.HasCock() ? "[monster.his] " + CombatController.Instance!.Context.Enemy.CockDescript(0) + " bumping into your [ass] as [monster.he] greedily fondles your tails" : "the scent of [monster.his] arousal in the air, leaking out even more every time [monster.he] caresses your tails"
            ));

            if (Player.Body.Get<TailPart>().Venom > 2)
                OutputText("Scenes.Kitsune.KitsuneGenericFluff.RealizationLeftUntouched".Localized(
                     NumberUtils.Num2Text(Player.Body.Get<TailPart>().Venom - 2),

                     CombatController.Instance!.Context.Enemy.HasCock() ? "swirls around [themonster]'s " + CombatController.Instance!.Context.Enemy.CockDescript(0) + ", stroking [monster.him] as though it were your own hand" : "slips between [themonster]'s folds, plunging effortlessly into [monster.his] depths",

                     CombatController.Instance!.Context.Enemy.HasCock() ? "teases [monster.his] " + (CombatController.Instance!.Context.Enemy.Balls > 0 ? CombatController.Instance!.Context.Enemy.BallsDescript() : CombatController.Instance!.Context.Enemy.AssholeDescript()) + "." : "brushes against [monster.his] " + CombatController.Instance!.Context.Enemy.ClitDescript() + "."
                ));

            if (Player.IsNineTails())
                OutputText("Scenes.Kitsune.KitsuneGenericFluff.EvenThoughEnsnared".Localized());

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.VisionBlursLoses".Localized(
                 CombatController.Instance!.Context.Enemy.HasCock() ? "thrusts forward, [monster.his] seed splattering against your [haircolor] fur as [monster.he] buries [monster.his] " + CombatController.Instance!.Context.Enemy.CockDescript(0) + " between [if (tailnumber > 2) {two of}] your tails" : "clenches down [if (tailnumber > 4) {on your tail lodged inside [monster.him]|on the tails in [monster.his] grip}]"
            ));

            OutputText("Scenes.Kitsune.KitsuneGenericFluff.TremblingPlaceHand".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void Donation()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.Donation.EvenIfUntended".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Gems".Localized(), DonateGems);

            AddNextButton(ItemRegistry.Create(ConsumableIds.Biscuit).Name, () => DonateItem(ConsumableIds.Biscuit));

            AddNextButton(ItemRegistry.Create(ConsumableIds.Lollipop).Name, () => DonateItem(ConsumableIds.Lollipop));

            AddNextButton(ItemRegistry.Create(ConsumableIds.KitsuneGift).Name, () => DonateItem(ConsumableIds.KitsuneGift));

            AddNextButton(ItemRegistry.Create(UseableIds.TeddyBear).Name, () => DonateItem(UseableIds.TeddyBear));

            AddButton(14, "Common.Button.Back".Localized(), () => KitsuneShrine());
        }

        public void DonateGems()
        {
            ClearOutput();

            OutputText("Scenes.Kitsune.DonateGems.ElectLeaveGems".Localized());

            OutputText("Scenes.Kitsune.DonateGems.LeaveWonderGod".Localized());

            Player.Gems -= 10;

            DoNext(() => ReturnToCamp(1));
        }

        public void DonateItem(Identifier item)
        {
            ClearOutput();

            if (item == ConsumableIds.Biscuit)
            {
                OutputText("Scenes.Kitsune.DonateItem.LeaveBiscuit".Localized());

                OutputText("Scenes.Kitsune.DonateItem.LeaveOfferingBehind".Localized());
            }
            else if (item == ConsumableIds.Lollipop)
            {
                OutputText("Scenes.Kitsune.DonateItem.PackLolipop".Localized());

                OutputText("Scenes.Kitsune.DonateItem.LeaveWonderPleased".Localized());
            }
            else if (item == ConsumableIds.KitsuneGift)
            {
                OutputText("Scenes.Kitsune.DonateItem.PackSquare".Localized());

                OutputText("Scenes.Kitsune.DonateItem.LeaveOfferingBehindAgain".Localized());
            }
            else if (item == UseableIds.TeddyBear)
            {
                OutputText("Scenes.Kitsune.DonateItem.PackTeddyBear".Localized());

                OutputText("Scenes.Kitsune.DonateItem.LeaveHopeTeddy".Localized());
            }

            Player.DestroyItems(item);

            DoNext(() => ReturnToCamp(1));
        }

    }
}