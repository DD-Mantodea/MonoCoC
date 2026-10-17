using MonoCoC.Attributes;

using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<Dominika>("coc:dominika")]
    public class Dominika : Scene
    {

        public override void Entry()
        {
            FellatrixBarAppearance();
        }

        public void FellatrixBarAppearance()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) == 0)
                OutputText("Scenes.Dominika.FellatrixBarAppearance.SpotNewFace".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffLargeCocks) != -1)
                    OutputText("Scenes.Dominika.FellatrixBarAppearance.NoticeDominikaSpeaking".Localized());
        }

        public bool FellatrixSucked()
        {

            return TimesFellatrixSucked() > 0;
        }

        public int TimesFellatrixSucked()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.DominikaTimesHypnoBj) + CoC.Instance.Flags.Get(GameFlagIds.DominikaTimesMulticockSlobbered) + CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffDogcocks) + CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffLargeCocks) + CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffSmallCocks) + CoC.Instance.Flags.Get(GameFlagIds.DominikaVaginalOralReceived) + CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffHorsecocks);
        }

        public void FellatrixBarApproach()
        {
            ClearOutput();

            ShowImage("dominika-first-meeting-at-bar");

            if (FellatrixSucked() && CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) > 0 && Player.HasKeyItem("Zetaz's Map") && CoC.Instance.Flags.Get(GameFlagIds.DominikasSwordGiven) == 0)
            {
                DominikaSpellblade();

                return;
            }

            if (Player.MinoScore() >= 3 && Player.Body.Get<FacePart>().Id == BodyPartTypes.CowMinotaur && Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Dominika.FellatrixBarApproach.ApproachWomanDeliveringWhat".Localized());

                OutputText("Scenes.Dominika.FellatrixBarApproach.TavernMaidDreadlocks".Localized());

                OutputText("Scenes.Dominika.FellatrixBarApproach.NodSlightlyGlancingBack".Localized());

                View.Time.PassTime(1);

                Go<TelAdre>();

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) == 0)
            {
                OutputText("Scenes.Dominika.FellatrixBarApproach.ApproachWomanDelivering".Localized());

                OutputText("Scenes.Dominika.FellatrixBarApproach.AcceptTheSeat".Localized());

                OutputText("Scenes.Dominika.FellatrixBarApproach.BlushingSlightlyAsk".Localized());

                OutputText("Scenes.Dominika.FellatrixBarApproach.TwoOfYouChat".Localized());

                OutputText("Scenes.Dominika.FellatrixBarApproach.AfterMoreConversation".Localized());

                DoNext(() => ReturnToCamp(1));

                CoC.Instance.Flags.Set(GameFlagIds.DominikaStage, CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) + 1);

                return;
            }
            else
                if (!Player.IsBiped())
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) == 1)
                    {
                        OutputText("Scenes.Dominika.FellatrixBarApproach.MakeYourWayOver".Localized());

                        OutputText("Scenes.Dominika.FellatrixBarApproach.IntriguedAskToElaborate".Localized());

                        OutputText("Scenes.Dominika.FellatrixBarApproach.SeemsToThinkSomething".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.DominikaStage, CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) + 1);
                    }
                    else
                        OutputText("Scenes.Dominika.FellatrixBarApproach.OverToTableAgain".Localized());

                    View.Time.PassTime(1);

                    Go<TelAdre>();

                    return;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) == 1)
                    {
                        OutputText("Scenes.Dominika.FellatrixBarApproach.OverToTableThird".Localized());

                        OutputText("Scenes.Dominika.FellatrixBarApproach.AskElaborateAgain".Localized());

                        OutputText("Scenes.Dominika.FellatrixBarApproach.ThinksSomethingAgain".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Sure".Localized(), AcceptDominikasKnowledge);

                        AddButton(1, "Common.Button.NoThanks".Localized(), TurnDownDominikasKnowledge);

                        return;
                    }
                    else
                    {
                        OutputText("Scenes.Dominika.FellatrixBarApproach.OverToTableFourth".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Sure".Localized(), AcceptDominikasKnowledge);

                        AddButton(1, "Common.Button.NoThanks".Localized(), TurnDownDominikasKnowledge);

                        return;
                    }

            OutputText("Scenes.Dominika.FellatrixBarApproach.ErrorAlertTheFenfen".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TurnDownDominikasKnowledge()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.TurnDownDominikasKnowledge.NodsSmallTalk".Localized());

            View.Time.PassTime(1);

            Go<TelAdre>();
        }

        public void AcceptDominikasKnowledge()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.DominikaStage, CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) == 2)
            {
                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.SmilesWithHerEyes".Localized());

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.DucksIntoAlley".Localized());

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BooksPiledDesks".Localized());

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.DespiteRevelationMultiple".Localized());
            }
            else
            {
                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.SmilesWithEyesAgain".Localized());

                if (!FellatrixSucked())
                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.LearningMoreArcaneLore".Localized());
                else
                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.FeelThoseBlackLips".Localized());

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.LetHerLead".Localized());

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.InsideOfHerPlace".Localized());

                if (Player.Intelligence >= 80)
                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.NotLackIntellect".Localized());

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.UnwindsHerTurban".Localized());
            }

            var spellList = CombatAbilityRegistry.Abilities.Ids
                .Select(CombatAbilityRegistry.Get)
                .OfType<CombatAbilityDefinition>()
                .Where(ability => ability.DominikaOrder > 0)
                .OrderBy(ability => ability.DominikaOrder)
                .ToArray();

            var knowsallDominikaSpells = true;

            var knowssomeDominikaSpells = false;

            foreach (var ability in spellList)
            {
                if (Player.HasStatusEffect(ability.KnownWhen))
                    knowssomeDominikaSpells = true;
                else
                    knowsallDominikaSpells = false;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaLearningCooldown) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.DominikaLearningCooldown, 7 + Random.Shared.Next(3));

                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.HerHandsGlowWith".Localized());

                if (knowsallDominikaSpells)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.DominikaEmbarrassedAboutMagic) == 0)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.DominikaEmbarrassedAboutMagic, CoC.Instance.Flags.Get(GameFlagIds.DominikaEmbarrassedAboutMagic) + 1);

                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BlinkThenCasuallyMention".Localized());
                    }
                    else
                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BlinkMentionMagic".Localized());

                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.MagicalTheoryNotInteresting".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 1 + Random.Shared.Next(4)));
                }
                else
                    if (knowssomeDominikaSpells)
                    {
                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.TakeAStepBackwards".Localized());

                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.SeemsQuiteGood".Localized());

                        foreach (var ability in spellList)
                        {
                            if (!Player.HasStatusEffect(ability.KnownWhen))
                            {
                                Player.CreateStatusEffect(ability.KnownWhen, 0, 0, 0, 0);

                                var color = ability.Category == CombatAbilityCategory.BlackMagic
                                    ? "Combat.School.Black".Localized()
                                    : "Combat.School.White".Localized();

                                OutputText("Scenes.Dominika.AcceptDominikasKnowledge.NewMagicSpellLearned".Localized(
                                     color,

                                     ability.NameKey.Localized()
                                ));

                                Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f));

                                break;
                            }
                        }
                    }
                    else
                    {
                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.NearlyFallOnYour".Localized());

                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BlackLipsCurving".Localized());

                        OutputText("Scenes.Dominika.AcceptDominikasKnowledge.EvenWithInexperienceFind".Localized());

                        switch (Random.Shared.Next(6))
                        {
                            case 0:
                                {
                                    Player.CreateStatusEffect(StatusEffectIds.KnowsMight, 0, 0, 0, 0);

                                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BlackSpellLearned".Localized());
                                }

                                break;

                            case 1:
                                {
                                    Player.CreateStatusEffect(StatusEffectIds.KnowsHeal, 0, 0, 0, 0);

                                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BlackSpellAgain".Localized());
                                }

                                break;

                            case 2:
                                {
                                    Player.CreateStatusEffect(StatusEffectIds.KnowsArouse, 0, 0, 0, 0);

                                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.BlackSpellThird".Localized());
                                }

                                break;

                            case 3:
                                {
                                    Player.CreateStatusEffect(StatusEffectIds.KnowsCharge, 0, 0, 0, 0);

                                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.WhiteSpellLearned".Localized());
                                }

                                break;

                            case 4:
                                {
                                    Player.CreateStatusEffect(StatusEffectIds.KnowsBlind, 0, 0, 0, 0);

                                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.WhiteSpellAgain".Localized());
                                }

                                break;

                            case 5:
                                {
                                    Player.CreateStatusEffect(StatusEffectIds.KnowsWhitefire, 0, 0, 0, 0);

                                    OutputText("Scenes.Dominika.AcceptDominikasKnowledge.WhiteSpellThird".Localized());
                                }

                                break;
                        }

                        if (true)
                            OutputText("Scenes.Dominika.AcceptDominikasKnowledge.SomethingFuckedUp".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f));
                    }
            }

            DoNext(() => FellatrixSexWarmup());
        }

        public void FellatrixSexWarmup()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaLearningCooldown) > 0)
                OutputText("Scenes.Dominika.FellatrixSexWarmup.GlancesOutWindowNervously".Localized());
            else
                OutputText("Scenes.Dominika.FellatrixSexWarmup.GlancesOutWindowShe".Localized());

            OutputText("Scenes.Dominika.FellatrixSexWarmup.WalksTowardsYouResting".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Dominika.FellatrixSexWarmup.BrieflyRunDownTo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaStage) == 2)
                OutputText("Scenes.Dominika.FellatrixSexWarmup.CoughsGlancingAside".Localized());
            else
                OutputText("Scenes.Dominika.FellatrixSexWarmup.CoughsGlancingAsideAgain".Localized());

            OutputText("Scenes.Dominika.FellatrixSexWarmup.BreathingInTurnsBack".Localized());

            if (FellatrixSucked())
                OutputText("Scenes.Dominika.FellatrixSexWarmup.AgainWord".Localized());

            OutputText("Scenes.Dominika.FellatrixSexWarmup.QuestionSayEndBreak".Localized());

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Dominika.FellatrixSexWarmup.PolitelyExplainYouDont".Localized());

                OutputText("Scenes.Dominika.FellatrixSexWarmup.WalksUpKissesCheek".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AcceptFellatrixOral);

            AddButton(1, "Common.Button.No".Localized(), DeclineFellatrixOral);
        }

        public void DeclineFellatrixOral()
        {
            ClearOutput();

            if (!FellatrixSucked())
            {
                OutputText("Scenes.Dominika.DeclineFellatrixOral.RaisesHerEyebrow".Localized());

                OutputText("Scenes.Dominika.DeclineFellatrixOral.ShePauses".Localized());

                OutputText("Scenes.Dominika.DeclineFellatrixOral.MightNotHurt".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                AddButton(0, "Common.Button.Yes".Localized(), AcceptFellatrixOral);

                AddButton(1, "Common.Button.No".Localized(), DeclineFellatrixOralHARDCORE);
            }
            else
            {
                OutputText("Scenes.Dominika.DeclineFellatrixOral.TwingeOfAnnoyance".Localized());

                OutputText("Scenes.Dominika.DeclineFellatrixOral.EachWordCarefullyEnunciated".Localized());

                OutputText("Scenes.Dominika.DeclineFellatrixOral.WhatWereYouThinking".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), AcceptFellatrixOral);
            }
        }

        public void DeclineFellatrixOralHARDCORE()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.DeclineFellatrixOralHARDCORE.AsksAgainPursesLips".Localized());

            OutputText("Scenes.Dominika.DeclineFellatrixOralHARDCORE.VeryTemptingMaybeOne".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            AddButton(0, "Common.Button.Yes".Localized(), AcceptFellatrixOral);

            AddButton(1, "Common.Button.No".Localized(), DeclineFellatrixOralSUPERHARDCORE);
        }

        public void DeclineFellatrixOralSUPERHARDCORE()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.DeclineFellatrixOralSUPERHARDCORE.StaresIncredulously".Localized());

            OutputText("Scenes.Dominika.DeclineFellatrixOralSUPERHARDCORE.LetYourselfOut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DominikaStage, -1);

            View.Time.PassTime(1);

            Go<TelAdre>();
        }

        public void AcceptFellatrixOral()
        {
            ClearOutput();

            ShowImage("dominika-oral-sex");

            int x = Player.CockThatFits(36);

            if (TimesFellatrixSucked() >= 6 && TimesFellatrixSucked() - 4 >= CoC.Instance.Flags.Get(GameFlagIds.DominikaLastHypnoSuckCount))
            {
                DominikaBlowjobs();

                return;
            }

            if (TimesFellatrixSucked() >= 3 && CoC.Instance.Flags.Get(GameFlagIds.DominikaSpecialFollowup) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
                {
                    OutputText("Scenes.Dominika.AcceptFellatrixOral.FamiliarSlickBrutality".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.RegainComposureGasping".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.AsksNotReallyBeen".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ParticularReactionIs".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SorceressCrossesBack".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.TakesDeepBreathTurning".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.LooksForAffirmation".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Agree".Localized(), AgreeWithDominika);

                    AddButton(1, "Common.Button.Ehhh".Localized(), EhhhhDominika);

                    AddButton(2, "Common.Button.Disagree".Localized(), DisagreeDominika);
                }
                else
                {
                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ArriveAtApartment".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ConversationAsYouRecover".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.GlanceAroundALittle".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.TurningBackSheSits".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.TurnsBackToWindow".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.AggressivenessOfSpeech".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.CrossesRoomToReturn".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Agree".Localized(), AgreeWithDominika);

                    AddButton(1, "Common.Button.Ehhh".Localized(), EhhhhDominika);

                    AddButton(2, "Common.Button.Disagree".Localized(), DisagreeDominika);
                }

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, 1f));

                return;
            }

            if (x < 0)
                x = 0;

            if (Player.HasVagina() && (Player.Gender == Gender.Female || Random.Shared.Next(2) == 0))
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.DominikaVaginalOralReceived) == 0)
                {
                    OutputText("Scenes.Dominika.AcceptFellatrixOral.DarkLipsCurveUp".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.StraddlesYourBelly".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SucculentSoftLips".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.PurrsThenDescends".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.LowerLipBrushes".Localized(
                         Player.ClitDescript(),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.NailsDigIntoThighs".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.RisesFromIrritation".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.MouthAndLipsDescend".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SlipperyWarmAppendage".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.LoseTrackOfTime".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.FeelHerSlideUp".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HandToMetalCrotch".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.AfterSmallTalk".Localized());
                }
                else
                {
                    OutputText("Scenes.Dominika.AcceptFellatrixOral.OnceYouArriveSecond".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ForcesYourLegsOpen".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.TongueAggressivelyShoves".Localized(
                         Player.VaginaDescript(0),

                         Player.VaginaDescript(0)
                    ));

                    if (Player.Cocks.Count == 0)
                        OutputText("Scenes.Dominika.AcceptFellatrixOral.WishesHadCock".Localized());
                    else
                        OutputText("Scenes.Dominika.AcceptFellatrixOral.WishesCockAgain".Localized());

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.DontSpeakCannotSpeak".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.CannotResistForLong".Localized(
                         Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SmilesCuddlingWithYou".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.DominikaVaginalOralReceived, CoC.Instance.Flags.Get(GameFlagIds.DominikaVaginalOralReceived) + 1);
            }
            else
                if (Player.CockTotal() >= 2 && Random.Shared.Next(2) == 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.DominikaTimesMulticockSlobbered) == 0)
                    {
                        OutputText("Scenes.Dominika.AcceptFellatrixOral.LicksHerTeeth".Localized());

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.TeasesCurlingPalm".Localized(
                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.LeansInClose".Localized(
                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.ShiftsDeeperSlurping".Localized(
                             Player.CockDescript(1),

                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.FortunatelyDontHaveTo".Localized(
                             Player.CockDescript(0),

                             Player.MultiCockDescriptLight(),

                             Player.CockDescript(1)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.TensingSlurpingPleasure".Localized());

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.OnlyGoesFurther".Localized(
                             Player.CockDescript(1),

                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.CabalistSeemsIllContent".Localized(
                             Player.CockDescript(1)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.SoftlyLipsPress".Localized());

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.HandMetalCrotchAgain".Localized());

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.SmallTalkAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Dominika.AcceptFellatrixOral.BarelyTimeToNod".Localized(
                             Player.CockDescript(1),

                             Player.CockDescript(1)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.ClearThatItWon".Localized(
                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.RestInHerHands".Localized(
                             Player.CockDescript(1)
                        ));

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.DragsYouRecklessly".Localized());

                        OutputText("Scenes.Dominika.AcceptFellatrixOral.DominikaSmilesRelaxing".Localized());
                    }

                    CoC.Instance.Flags.Set(GameFlagIds.DominikaTimesMulticockSlobbered, CoC.Instance.Flags.Get(GameFlagIds.DominikaTimesMulticockSlobbered) + 1);
                }
                else
                    if (Player.CockArea(x) <= 36)
                    {
                        if (Player.HasKnot(x))
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffDogcocks) == 0)
                            {
                                OutputText("Scenes.Dominika.AcceptFellatrixOral.ConfidentChuckle".Localized());

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.BrushesHerHandsOver".Localized(
                                    Player.Cocks[x].Id == BodyPartTypes.Dog || Player.Cocks[x].Id == BodyPartTypes.Wolf ? "You were not prepared for the brief laugh that escapes her inky lips. [say: Ah, sorry, sorry,] she quickly explains, though she still smirks, [say: Just... mmm, fan of spicy food, I take it?] " : "",

                                     Player.CockDescript(x),

                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.SenseOfStrangeRegret".Localized(
                                     Player.CockDescript(x),

                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.SeeLustsSlaked".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.TongueReturnsToBase".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.SalivaFlicksAcrossTip".Localized(
                                     Player.CockDescript(x),

                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.AcheOfYourDesire".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.AlmostAsThoughTeasing".Localized(
                                     Player.CockDescript(x),

                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.DarknessFillsVision".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.SoftlyLipsPressAgain".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.HandMetalCrotchThird".Localized());

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.SmallTalkThird".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Dominika.AcceptFellatrixOral.ArriveAtSecondStory".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.HandsRoughWithYou".Localized(
                                     Player.CockDescript(x),

                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.LikeLastVisitFacefucks".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.KnotPressesPastLips".Localized(
                                     Player.CockDescript(x)
                                ));

                                OutputText("Scenes.Dominika.AcceptFellatrixOral.SmilesRelaxingWithYou".Localized());
                            }

                            CoC.Instance.Flags.Set(GameFlagIds.DominikaSuckedOffDogcocks, CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffDogcocks) + 1);
                        }
                        else
                            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffHorsecocks) == 0)
                                {
                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SmileThatSpreads".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.BlinkInConfusion".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SingleFingerTraces".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.NearlyMeltYouveHad".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.MoaningOnceAgainHands".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.RegretTheySlip".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.FulfillsItsDesire".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.FindYourselfObsessing".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.BobsDeeperMovingHead".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ThoughtProvesCatalyst".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SoftlyLipsPressThird".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HandMetalCrotchFourth".Localized());

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SmallTalkFourth".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.NearlyDraggedIntoBedroom".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HandSlidesBelly".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HerWayFaceFuck".Localized());

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.FatFullLipsBounce".Localized());

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.RelaxingWithYouAgain".Localized());
                                }

                                CoC.Instance.Flags.Set(GameFlagIds.DominikaSuckedOffHorsecocks, CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffHorsecocks) + 1);
                            }
                            else
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffLargeCocks) == 0)
                                {
                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SmilesTakingYourHand".Localized());

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.RemovesBottomOfYour".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    if (Player.Balls > 0)
                                        OutputText("Scenes.Dominika.AcceptFellatrixOral.MouthSlipsDownYour".Localized(
                                             Player.SackDescript(),

                                             Player.SackDescript()
                                        ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.AsksEmphasizingQuestion".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.RushOfPleasure".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HandsTravelAlongLength".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HandsPressToThighs".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.FrictionTheMess".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.WetPopSignalsLips".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.GentlyMassagesYourHead".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.HandMetalCrotchFifth".Localized());

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.SmallTalkFifth".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ArriveApartmentAgain".Localized(
                                         Player.CockDescript(x),

                                         Player.CockDescript(x),

                                         Player.CockDescript(x),

                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.ForcesLegsOpenAgain".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.LikeBeforeFacefucks".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.DontSpeakAsIt".Localized(
                                         Player.CockDescript(x)
                                    ));

                                    OutputText("Scenes.Dominika.AcceptFellatrixOral.CuddlingWithYouAgain".Localized());
                                }

                                CoC.Instance.Flags.Set(GameFlagIds.DominikaSuckedOffLargeCocks, CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffLargeCocks) + 1);
                            }
                    }
                    else
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffSmallCocks) == 0)
                        {
                            OutputText("Scenes.Dominika.AcceptFellatrixOral.SurprisinglyBigGrin".Localized());

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.RemovesBottomAgain".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.DarkLipsPressForward".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.OnceMoreMouthShifts".Localized(
                                 Player.CockDescript(x)
                            ));

                            if (Player.Balls > 0)
                                OutputText("Scenes.Dominika.AcceptFellatrixOral.WhileMouthPerforms".Localized());

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.ShiftsSlightlyArcing".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.PurrsLickingFatBlack".Localized());

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.HipsStartToRock".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.HandsRoamingOverGirth".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.SoftlyLipsPressFourth".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.HandMetalCrotchSixth".Localized());

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.SmallTalkSixth".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Dominika.AcceptFellatrixOral.ArriveApartmentThird".Localized(
                                 Player.CockDescript(x),

                                 Player.CockDescript(x),

                                 Player.CockDescript(x),

                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.PushesYourLegsOpen".Localized(
                                 Player.CockDescript(x),

                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.HandsAndTitsBear".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.BuckingHipsSeize".Localized(
                                 Player.CockDescript(x)
                            ));

                            OutputText("Scenes.Dominika.AcceptFellatrixOral.CuddlingWithYouThird".Localized());
                        }

                        CoC.Instance.Flags.Set(GameFlagIds.DominikaSuckedOffSmallCocks, CoC.Instance.Flags.Get(GameFlagIds.DominikaSuckedOffSmallCocks) + 1);
                    }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, 1f));

            if (TimesFellatrixSucked() >= 4 && CoC.Instance.Flags.Get(GameFlagIds.DominikaDrama) == 0)
                DoNext(() => Dominidrama());
            else
                DoNext(() => ReturnToCamp(2));
        }

        public void AgreeWithDominika()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
            {
                OutputText("Scenes.Dominika.AgreeWithDominika.PauseForAMoment".Localized());

                OutputText("Scenes.Dominika.AgreeWithDominika.PurrsSoftlyFolds".Localized());

                OutputText("Scenes.Dominika.AgreeWithDominika.ThinkAboutHerLips".Localized());
            }
            else
            {
                OutputText("Scenes.Dominika.AgreeWithDominika.LookAtHerMoment".Localized());

                if (Player.Tallness < 64)
                    OutputText("Scenes.Dominika.AgreeWithDominika.LeansDownWord".Localized());
                else
                    OutputText("Scenes.Dominika.AgreeWithDominika.PushesHerselfUp".Localized());

                OutputText("Scenes.Dominika.AgreeWithDominika.BitingYourLipBarely".Localized());

                if (Player.HasCock())
                    OutputText(Player.MultiCockDescriptLight());
                else
                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript(0));
                    else
                        OutputText("Scenes.Dominika.AgreeWithDominika.GroinWord".Localized());

                OutputText("Scenes.Dominika.AgreeWithDominika.ActionTurnsIntoKiss".Localized());

                OutputText("Scenes.Dominika.AgreeWithDominika.PurrsSoftlyFoldsAgain".Localized());

                OutputText("Scenes.Dominika.AgreeWithDominika.ThinkHerLipsAgain".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            CoC.Instance.Flags.Set(GameFlagIds.DominikaSpecialFollowup, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void EhhhhDominika()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.EhhhhDominika.SmallGruntNot".Localized());

            OutputText("Scenes.Dominika.EhhhhDominika.StreetsOfTelAdre".Localized());

            OutputText("Scenes.Dominika.EhhhhDominika.MagnificentOral".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DominikaSpecialFollowup, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void DisagreeDominika()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Corruption, -2f));

            CoC.Instance.Flags.Set(GameFlagIds.DominikaSpecialFollowup, 3);

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
            {
                OutputText("Scenes.Dominika.DisagreeDominika.RidiculousThing".Localized());

                OutputText("Scenes.Dominika.DisagreeDominika.StreetsOfTelAdre".Localized());

                OutputText("Scenes.Dominika.DisagreeDominika.ConflictedWalkingAway".Localized());
            }
            else
            {
                OutputText("Scenes.Dominika.DisagreeDominika.RidiculousThingAgain".Localized());

                OutputText("Scenes.Dominika.DisagreeDominika.StreetsTelAdreAgain".Localized());

                OutputText("Scenes.Dominika.DisagreeDominika.ConflictedWalkingAwayAgain".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FellatrixDream()
        {

            CoC.Instance.Flags.Set(GameFlagIds.DominikaSpecialFollowup, CoC.Instance.Flags.Get(GameFlagIds.DominikaSpecialFollowup) + 3);

            OutputText("Scenes.Dominika.FellatrixDream.CutThroughFoliage".Localized());

            OutputText("Scenes.Dominika.FellatrixDream.InhalesPartsBushes".Localized());

            OutputText("Scenes.Dominika.FellatrixDream.SheIsTheSky".Localized());

            OutputText("Scenes.Dominika.FellatrixDream.WeightOfTheTar".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryFound) > 0)
                OutputText("Scenes.Dominika.FellatrixDream.PreferredTheDreams".Localized());

            PlayerMenu();
        }

        public void Dominidrama()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.Dominidrama.YawnALittle".Localized());

            OutputText("Scenes.Dominika.Dominidrama.UnfamiliarVoiceInterrupts".Localized());

            OutputText("Scenes.Dominika.Dominidrama.WomanBeforeYouPresuming".Localized());

            OutputText("Scenes.Dominika.Dominidrama.OnceShesRegained".Localized());

            OutputText("Scenes.Dominika.Dominidrama.HearingNameBrings".Localized());

            OutputText("Scenes.Dominika.Dominidrama.HeardOfAnyoneFitting".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), DominidramaYes);

            AddButton(1, "Common.Button.No".Localized(), DominidramaNo);
        }

        public void DominidramaYes()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.DominidramaYes.HaveAFewGuesses".Localized());

            OutputText("Scenes.Dominika.DominidramaYes.VoicingTheseProblems".Localized());

            OutputText("Scenes.Dominika.DominidramaYes.SighsLightlyAsa".Localized());

            OutputText("Scenes.Dominika.DominidramaYes.WearyContinueOnWay".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DominikaDrama, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DominidramaNo()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.DominidramaNo.ThinkAboutTheWoman".Localized());

            OutputText("Scenes.Dominika.DominidramaNo.FinallySheGestures".Localized());

            OutputText("Scenes.Dominika.DominidramaNo.TurnsToLeaveDown".Localized());

            OutputText("Scenes.Dominika.DominidramaNo.FeelWarmInside".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.DominikaDrama, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void DominikaBlowjobs()
        {
            ClearOutput();

            ShowImage("dominika-gives-you-blowjob");

            int x = Player.CockThatFits(36);

            if (x < 0)
                x = Player.BiggestCockIndex();

            OutputText("Scenes.Dominika.DominikaBlowjobs.EyesOpenLyingDown".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.ShouldNotHaveLed".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.TryPullYourselfUp".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.BeforeYouCanDemand".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.RespondInWayExpected".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Dominika.DominikaBlowjobs.StillFeelIllAt".Localized());
            else
                OutputText("Scenes.Dominika.DominikaBlowjobs.RestraintsSomewhatErotic".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.SmilesSlightlyAtPun".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.MusesStepsOver".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Dominika.DominikaBlowjobs.WingsBarelyHaveRoom".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.ContinuesHerHandRuns".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.FingersGrownFamiliar".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Dominika.DominikaBlowjobs.CaressedYourDick".Localized());
            else
                OutputText("Scenes.Dominika.DominikaBlowjobs.CaressedYourLabia".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs.NowTheyCurlAround".Localized());

            if (Player.Intelligence < 50)
                OutputText("Scenes.Dominika.DominikaBlowjobs.WordsEchoInHead".Localized());
            else
                OutputText("Scenes.Dominika.DominikaBlowjobs.ItsMagicYourePositive".Localized());

            if (Player.HasCock())
            {
                if (Player.CockArea(x) <= 36)
                {
                    OutputText("Scenes.Dominika.DominikaBlowjobs.HissesSlowlyTrailing".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.FingertipsTrailSlowly".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.IncredibleOppressiveAche".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.LipsComeAllThe".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.LoudDeepGulp".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.CumDidntRealize".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.DominikaSays".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.BloodSurgesInto".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x),

                         Player.Mf("emasculating", "humiliating"),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AdoreTheFeelingOf".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.EntiretyOfBodyFeels".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSay".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.HerMouthDescendsAgain".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSayAgain".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.SurgesWithArtificialDesire".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSayThird".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.HeadFallsAgain".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.MostSituationsStiff".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSayPlain".Localized());
                }
                else
                {
                    OutputText("Scenes.Dominika.DominikaBlowjobs.HissesSlowlyTrailingAgain".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.FingersTraceStrangePatterns".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.YouDo".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.FingertipsTrailSlowlyAgain".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.IncredibleOppressiveAcheAgain".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.LipsComeAllWay".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.LoudDeepGulpAgain".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.CumDidntRealizeAgain".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.BloodSurgesIntoAgain".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x),

                         Player.Mf("emasculating", "humiliating"),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AdoreLipsFeeling".Localized(
                         Player.CockDescript(x),

                         Player.CockDescript(x),

                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.BodyShudderAgain".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSayFifth".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.HerMouthDescendsAgain".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSaySixth".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.ArtificialDesireAgain".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSaySeventh".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.HeadFallsAgain".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Dominika.DominikaBlowjobs.MostSituationsStiffAgain".Localized());

                    OutputText("Scenes.Dominika.DominikaBlowjobs.AgainSayEighth".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Dominika.DominikaBlowjobs.HissesSlowlyTrailingThird".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.PurrsOverYourCrotch".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.PurrsAgainShiftsOver".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.DespiteDesperationTantalizing".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.HandsRoamUpFrom".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.TauntsSuchAnAsinine".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.NoCeremonyWhenIt".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.TeasesAgainAsYour".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.MiddleFingerPresses".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.SaysItBeforeCurrent".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.ThisTimeRatherThan".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Dominika.DominikaBlowjobs.HandTrailsOverBelly".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, -15f));

            DoNext(() => DominikaBlowjobs2());
        }

        public void DominikaBlowjobs2()
        {
            ClearOutput();

            OutputText("Scenes.Dominika.DominikaBlowjobs2.RollOverSittingUp".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs2.AfterLotOfWater".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DominikaSpecialFollowup) == 1 || CoC.Instance.Flags.Get(GameFlagIds.DominikaSpecialFollowup) == 4)
                OutputText("Scenes.Dominika.DominikaBlowjobs2.SenseItWasBest".Localized());
            else
                OutputText("Scenes.Dominika.DominikaBlowjobs2.DeeplyBothersYouLeaving".Localized());

            OutputText("Scenes.Dominika.DominikaBlowjobs2.EitherWayAnotherDay".Localized());

            Player.ChangeFatigue(-20);

            CoC.Instance.Flags.Set(GameFlagIds.DominikaLastHypnoSuckCount, TimesFellatrixSucked());

            CoC.Instance.Flags.Set(GameFlagIds.DominikaTimesHypnoBj, CoC.Instance.Flags.Get(GameFlagIds.DominikaTimesHypnoBj) + 1);

            View.Time.Days += 1;

            View.Time.Hours = 7;

            DoNext(() => ReturnToCamp(1));
        }

        public void DominikaSpellblade()
        {
            ClearOutput();

            if ((Player.MinoScore() >= 3 && Player.Body.Get<FacePart>().Id == BodyPartTypes.CowMinotaur && Player.Gender == Gender.Male) || !Player.IsBiped())
            {
                OutputText("Scenes.Dominika.DominikaSpellblade.GreetMakeSmallTalk".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.ExitsLeavingYouAt".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.TakeBladeHeadOut".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.HoldingUpTheSword".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.SnapsSuddenlyTurning".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.TurnsBackAroundYoure".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.WhateverYouUnwrapSword".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.WrapItAgainFree".Localized());
            }
            else
            {
                OutputText("Scenes.Dominika.DominikaSpellblade.WayHerEyesCurve".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.GesturesToBartender".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.ContinuesAThought".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.ReachingBelowTable".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.HoldingPackageInPalms".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.GripSwordHandle".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.ThanksMakeSmallTalk".Localized());

                OutputText("Scenes.Dominika.DominikaSpellblade.GettingBeerAndSword".Localized());
            }

            InventoryScene.TakeItem(ItemRegistry.Create(WeaponIds.Spellblade), () => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.DominikasSwordGiven, 1);
        }

    }
}