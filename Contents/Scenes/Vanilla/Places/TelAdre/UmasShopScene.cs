using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Perks;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<UmasShopScene>("coc:umas_shop")]
    public class UmasShopScene : Scene
    {

        public const int MASSAGE_RELIEF = 0;

        public const float MASSAGE_RELIEF_BONUS = 0.9f;

        public int sessionCost = 0;

        public List<Action> _loppeTalks = [];

        public const int MASSAGE_LUST = 1;

        public const float MASSAGE_LUST_BONUS = 1.1f;

        public const int MASSAGE_MODELLING = 2;

        public const int MASSAGE_MODELLING_BONUS = 10;

        public const int MASSAGE_RELAXATION = 3;

        public const float MASSAGE_RELAXATION_BONUS = 0.9f;

        public const int MASSAGE_POWER = 4;

        public const float MASSAGE_POWER_BONUS = 1.1f;

        public const int MAX_MASSAGE_BONUS_DURATION = 24;

        public const int NEEDLEWORK_UNDO = -1;

        public const int NEEDLEWORK_SPEED = 0;

        public const int NEEDLEWORK_LUST = 1;

        public const int NEEDLEWORK_DEFENSE = 2;

        public const int NEEDLEWORK_MAGIC = 3;

        public const int NEEDLEWORK_ATTACK = 4;

        public const string NEEDLEWORK_SPEED_PERK_DESC = "Uma's Acupuncture Needlework has gifted you with the 'Chi Reflow - Speed' perk. As a result your strength is capped but speed reductions are halved.";

        public const int NEEDLEWORK_SPEED_STRENGTH_CAP = 60;

        public const float NEEDLEWORK_SPEED_SPEED_MULTI = 0.5f;

        public const string NEEDLEWORK_LUST_PERK_DESC = "Uma's Acupuncture Needlework has gifted you with the 'Chi Reflow - Lust' perk. As a result your Lust resistance and Tease attack are enhanced, but Libido and Sensitivity gains are increased.";

        public const int NEEDLEWORK_LUST_LUST_RESIST = 10;

        public const float NEEDLEWORK_LUST_TEASE_MULTI = 10;

        public const float NEEDLEWORK_LUST_TEASE_DAMAGE_MULTI = 1.1f;

        public const float NEEDLEWORK_LUST_LIBSENSE_MULTI = 1.1f;

        public const string NEEDLEWORK_DEFENSE_PERK_DESC = "Uma's Acupuncture Needlework has gifted you with the 'Chi Reflow - Defense' perk. As a result your body has gained passive damage resistance and extra health, but speed is capped.";

        public const float NEEDLEWORK_DEFENSE_DEFENSE_MULTI = 1.1f;

        public const int NEEDLEWORK_DEFENSE_EXTRA_HP = 50;

        public const int NEEDLEWORK_DEFENSE_SPEED_CAP = 60;

        public const string NEEDLEWORK_MAGIC_PERK_DESC = "Uma's Acupuncture Needlework has gifted you with the 'Chi Reflow - Magic' perk. As a result your spells are now more powerful, but regular attacks are weaker.";

        public const float NEEDLEWORK_MAGIC_SPELL_MULTI = 25;

        public const float NEEDLEWORK_MAGIC_REGULAR_MULTI = 0.75f;

        public const string NEEDLEWORK_ATTACK_PERK_DESC = "Uma's Acupuncture Needlework has gifted you with the 'Chi Reflow - Attack' perk. As a result your regular attacks are more powerful, but your damage reduction is decreased.";

        public const float NEEDLEWORK_ATTACK_REGULAR_MULTI = 1.1f;

        public const float NEEDLEWORK_ATTACK_DEFENSE_MULTI = 0.9f;

        public const string MASSAGE_RELIEF_BONUS_TEXT = "<b>(10% Reduction to all Lust gains whilst active!)</b>";

        public const string MASSAGE_LUST_BONUS_TEXT = "<b>(10% Increase to all Lust gains whilst active!)</b>";

        public const string MASSAGE_MODELLING_BONUS_TEXT = "<b>(+10 Bonus Femininity whilst active!)</b>";

        public const string MASSAGE_RELAXATION_BONUS_TEXT = "<b>(10% Reduction to all Damage taken whilst active!)</b>";

        public const string MASSAGE_POWER_BONUS_TEXT = "<b>(10% Increase to all Damage inflicted whilst active!)</b>";

        public const int UMA_CONSIDER_PC_FEM = 70;

        public const int UMA_CONSIDER_PC_AND = 50;

        public override void Entry()
        {
            InitLoppeTalks();
        }

        public void InitLoppeTalks()
        {

            _loppeTalks = [];

            _loppeTalks.Add(() => TalkLoppeAttitudes());

            _loppeTalks.Add(() => TalkLoppeCarrotIncident());

            _loppeTalks.Add(() => TalkLoppePuberty());

            _loppeTalks.Add(() => TalkLoppesFirstGirl());

            _loppeTalks.Add(() => TalkLoppesFirstBoyfriend());
        }

        public void FirstVisitPart1()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.FirstVisitPart1.ArriveShopNearLoppe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) <= 3 && CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) == 0)
                OutputText("Scenes.Umasshop.FirstVisitPart1.NeverNoticedStorefront".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) <= 8)
                    OutputText("Scenes.Umasshop.FirstVisitPart1.HomelyBuildingNoticedBefore".Localized());
                else
                    OutputText("Scenes.Umasshop.FirstVisitPart1.SeenHumbleExteriorOften".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.ClinicSignAboveEntryway".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.FollowLoppeInsideInterior".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.LoppeCallsMotherAnswer".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.DogWomanThroughBeads".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.LoppeSmugGiggle".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.SecondFigureEmerges".Localized());

            if (Player.Tallness < 78)
                OutputText("Scenes.Umasshop.FirstVisitPart1.GazeDownAtYou".Localized());
            else
                if (Player.Tallness < 90)
                    OutputText("Scenes.Umasshop.FirstVisitPart1.GazeStraightAtYou".Localized());
                else
                    OutputText("Scenes.Umasshop.FirstVisitPart1.GazeUpAtYou".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart1.SoftMaternalExpression".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FirstVisitPart2);
        }

        public void FirstVisitPart2()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.FirstVisitPart2.LoppeGreetsUmaHug".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.IntroduceNameAsFriend".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.LoppeDisdainfulConfirm".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.UmaTeasesBoyfriend".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.WinningSmileAcceptTease".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.BothBurstOutLaughing".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.ConfusedAtSharedTastes".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.UmaBreaksAwkwardness".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart2.LoppeHoldsHandRecount".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FirstVisitPart3);
        }

        public void FirstVisitPart3()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.FirstVisitPart3.UmaGlaresAtDaughter".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart3.CatManEntersClinic".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart3.LoppeWhispersLeave".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart3.LoppeAgreesQuirkyMom".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart3.DeclineVisitOtherTime".Localized());

            OutputText("Scenes.Umasshop.FirstVisitPart3.DiscountOnUmaServices".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LoppePcMetUma, 1);

            ClearButtons();

            ReturnToCamp(1);
        }

        public void EnterClinic(bool returnedTo = false)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) == 0)
            {
                OutputText("Scenes.Umasshop.EnterClinic.WanderStreetsFindShop".Localized());

                OutputText("Scenes.Umasshop.EnterClinic.ClinicSignThenEnter".Localized());

                OutputText("Scenes.Umasshop.EnterClinic.StrangeHallAiryWaiting".Localized());

                OutputText("Scenes.Umasshop.EnterClinic.TallGreeterWalksOver".Localized());

                if (Player.Tallness < 78)
                    OutputText("Scenes.Umasshop.EnterClinic.GazeDownAtYou".Localized());
                else
                    if (Player.Tallness < 90)
                        OutputText("Scenes.Umasshop.EnterClinic.GazeStraightAtYou".Localized());
                    else
                        OutputText("Scenes.Umasshop.EnterClinic.GazeUpAtYou".Localized());

                OutputText("Scenes.Umasshop.EnterClinic.SoftMaternalExpression".Localized());

                OutputText("Scenes.Umasshop.EnterClinic.UmaIntroducesBusiness".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LoppePcMetUma, 0.5f);
            }
            else
                if (!returnedTo)
                {
                    OutputText("Scenes.Umasshop.EnterClinic.DecideVisitUmaClinic".Localized());

                    OutputText("Scenes.Umasshop.EnterClinic.QuietClinicNoCustomers".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) >= 1)
                        OutputText("Scenes.Umasshop.EnterClinic.SaucyFamiliarGreeting".Localized());
                    else
                        OutputText("Scenes.Umasshop.EnterClinic.PoliteClientGreeting".Localized());

                    if (Player.Femininity >= UMA_CONSIDER_PC_FEM)
                        OutputText("Scenes.Umasshop.EnterClinic.UmaAppraisesYourForm".Localized());
                }
                else
                    if (Player.Femininity >= UMA_CONSIDER_PC_FEM)
                        OutputText("Scenes.Umasshop.EnterClinic.FlirtatiousServiceOffer".Localized());
                    else
                        OutputText("Scenes.Umasshop.EnterClinic.ProfessionalServiceOffer".Localized());

            BuildCoreMenu();
        }

        public void BuildCoreMenu(bool noSex = false)
        {

            if (noSex)
            {
                OutputText("Scenes.Umasshop.BuildCoreMenu.AcceptAndApologize".Localized());

                OutputText("Scenes.Umasshop.BuildCoreMenu.NothingPersonalReply".Localized());
            }

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), MassageMenu);

            AddButton(1, "Common.Button.Acupuncture".Localized(), AcupunctureMenu);

            AddButton(2, "Common.Button.Talk".Localized(), TalkMenu);

            if (!noSex && CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) >= 1)
                AddButton(3, "Common.Button.Sex".Localized(), SexMenu);

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void MassageMenu()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageMenu.RequestFamousMassage".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.OffersMassageCatalogue".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.StudyMassageCatalogue".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.LustReliefMassageAd".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.ArousalMassageAd".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.ModellingMassageAd".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.RelaxingMassageAd".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.PowerMassageAd".Localized());

            OutputText("Scenes.Umasshop.MassageMenu.ContemplateMassageChoice".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Relief".Localized(), MassageRelief);

            AddButton(1, "Common.Button.Lust".Localized(), MassageLust);

            AddButton(2, "Common.Button.Modelling".Localized(), MassageModelling);

            AddButton(3, "Common.Button.Relaxation".Localized(), MassageRelaxation);

            AddButton(4, "Common.Button.Power".Localized(), MassagePower);

            AddButton(9, "Common.Button.NoThanks".Localized(), MassageNope);
        }

        public void MassageNope()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageNope.CancelMassageApology".Localized());

            OutputText("Scenes.Umasshop.MassageNope.UmaTakesCatalogueBack".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => EnterClinic(true));
        }

        public void MassageRelief()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageRelief.ChooseLustRelief".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
            {
                OutputText("Scenes.Umasshop.MassageRelief.TeasesLoppeStamina".Localized());

                OutputText("Scenes.Umasshop.MassageRelief.AdmitNeedDespiteLoppe".Localized());
            }

            OutputText("Scenes.Umasshop.MassageRelief.UmaLeadsToTreatment".Localized());

            MassageMain(MASSAGE_RELIEF);
        }

        public void MassageLust()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageLust.ChooseArousalMassage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
            {
                OutputText("Scenes.Umasshop.MassageLust.TeaseHandlingLoppe".Localized());

                OutputText("Scenes.Umasshop.MassageLust.PlayfulWinkDeflect".Localized());

                OutputText("Scenes.Umasshop.MassageLust.MockHurtSkillBoast".Localized());
            }
            else
                OutputText("Scenes.Umasshop.MassageLust.UmaLeadsToRoom".Localized());

            MassageMain(MASSAGE_LUST);
        }

        public void MassageModelling()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageModelling.ChooseModellingMassage".Localized());

            if (Player.Femininity <= 60)
                OutputText("Scenes.Umasshop.MassageModelling.TeaseCouldUseTouches".Localized());
            else
                OutputText("Scenes.Umasshop.MassageModelling.AlreadyPrettyEnough".Localized());

            OutputText("Scenes.Umasshop.MassageModelling.ThankAndFollowLead".Localized());

            MassageMain(MASSAGE_MODELLING);
        }

        public void MassageRelaxation()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageRelaxation.ChooseRelaxingMassage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
            {
                OutputText("Scenes.Umasshop.MassageRelaxation.WondersIfLoppeReason".Localized());

                OutputText("Scenes.Umasshop.MassageRelaxation.NeitherConfirmNorDeny".Localized());

                OutputText("Scenes.Umasshop.MassageRelaxation.WorryAboutBurnout".Localized());
            }
            else
                OutputText("Scenes.Umasshop.MassageRelaxation.UmaLeadsToRelaxation".Localized());

            MassageMain(MASSAGE_RELAXATION);
        }

        public void MassagePower()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassagePower.ChooseStrengthMassage".Localized());

            OutputText("Scenes.Umasshop.MassagePower.GrinsAtFightTalk".Localized());

            OutputText("Scenes.Umasshop.MassagePower.MassageMeansSurvival".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
            {
                OutputText("Scenes.Umasshop.MassagePower.WorriedAboutIntentions".Localized());

                OutputText("Scenes.Umasshop.MassagePower.AssureLoppeSafety".Localized());
            }

            OutputText("Scenes.Umasshop.MassagePower.UmaLeadsToTraining".Localized());

            MassageMain(MASSAGE_POWER);
        }

        public void MassageMain(int selectedMassage)
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageMain.RoomAmbienceDescription".Localized());

            OutputText("Scenes.Umasshop.MassageMain.RequestsGemsForService".Localized());

            OutputText("Scenes.Umasshop.MassageMain.PlayerAgreesFishingGems".Localized());

            OutputText("Scenes.Umasshop.MassageMain.UmaApologizesForPrice".Localized());

            var massageCost = 100;

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) >= 1)
                massageCost -= 25;

            if (Player.Gems < massageCost)
            {
                OutputText("Scenes.Umasshop.MassageMain.PlayerLacksEnoughGems".Localized());

                OutputText("Scenes.Umasshop.MassageMain.UmaRefusesUnpaidService".Localized());

                OutputText("Scenes.Umasshop.MassageMain.PlayerAcceptsRefusal".Localized());

                OutputText("Scenes.Umasshop.MassageMain.PlayerLeavesShopPolitely".Localized());

                ClearButtons();

                ReturnToCamp(1);

                return ;
            }

            OutputText("Scenes.Umasshop.MassageMain.PlayerPaysForMassage".Localized());

            Player.Gems -= massageCost;

            OutputText("Scenes.Umasshop.MassageMain.PocketsGemsPrepares".Localized());

            OutputText("Scenes.Umasshop.MassageMain.PlayerUndressesArmor".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Umasshop.MassageMain.PlayerArousedBeingNaked".Localized());

            OutputText("Scenes.Umasshop.MassageMain.PlayerReadyOnTable".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MassageCommence(selectedMassage));
        }

        public void MassageCommence(int selectedMassage)
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.MassageCommence.UmaWarnsAboutPain".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.PlayerBracesForTreatment".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.PressesElbowToChest".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Umasshop.MassageCommence.BetweenYourWords".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.PushesHardCausingPain".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.WarnsMorePainLater".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.FrontTreatmentGetsEasier".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.InstructsFlipForBack".Localized());

            if (Player.Femininity >= UMA_CONSIDER_PC_FEM)
                OutputText("Scenes.Umasshop.MassageCommence.HandCaressesHipsAbsently".Localized());
            else
                if (Player.Femininity >= UMA_CONSIDER_PC_AND)
                    OutputText("Scenes.Umasshop.MassageCommence.HandTouchesHipsLightly".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.SettlesFaceOnCushion".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.TracesSpotsWithRod".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.TreatmentLeavesYouSore".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.UmaExplainsChiSettling".Localized());

            OutputText("Scenes.Umasshop.MassageCommence.PlayerDressesAndLeaves".Localized());

            ApplyMassageBonus(selectedMassage);

            ClearButtons();

            ReturnToCamp(1);
        }

        public void ApplyMassageBonus(int selectedMassage)
        {

            if (selectedMassage < 0 || selectedMassage > 4)
                OutputText("Scenes.Umasshop.ApplyMassageBonus.InvalidMassageBonusId".Localized());
            else
            {
                var bonusValue = float.NaN;

                if (Player.HasStatusEffect(StatusEffectIds.UmasMassage))
                    Player.RemoveStatusEffect(StatusEffectIds.UmasMassage);

                if (selectedMassage == MASSAGE_RELIEF)
                {
                    OutputText("Scenes.Umasshop.ApplyMassageBonus.RelaxedBlocksArousal".Localized(
                        MASSAGE_RELIEF_BONUS_TEXT
                    ));

                    bonusValue = MASSAGE_RELIEF_BONUS;
                }
                else
                    if (selectedMassage == MASSAGE_LUST)
                    {
                        OutputText("Scenes.Umasshop.ApplyMassageBonus.WiredTinglingSensation".Localized(
                            MASSAGE_LUST_BONUS_TEXT
                        ));

                        bonusValue = MASSAGE_LUST_BONUS;
                    }
                    else
                        if (selectedMassage == MASSAGE_MODELLING)
                        {
                            OutputText("Scenes.Umasshop.ApplyMassageBonus.FeelingSexierThanBefore".Localized(
                                MASSAGE_MODELLING_BONUS_TEXT
                            ));

                            bonusValue = MASSAGE_MODELLING_BONUS;
                        }
                        else
                            if (selectedMassage == MASSAGE_RELAXATION)
                            {
                                OutputText("Scenes.Umasshop.ApplyMassageBonus.AchesAndPainsRelieved".Localized(
                                    MASSAGE_RELAXATION_BONUS_TEXT
                                ));

                                bonusValue = MASSAGE_RELAXATION_BONUS;
                            }
                            else
                                if (selectedMassage == MASSAGE_POWER)
                                {
                                    OutputText("Scenes.Umasshop.ApplyMassageBonus.MusclesTingleWithEnergy".Localized(
                                        MASSAGE_POWER_BONUS_TEXT
                                    ));

                                    bonusValue = MASSAGE_POWER_BONUS;
                                }

                if (bonusValue != 0)
                {
                    Player.CreateStatusEffect(StatusEffectIds.UmasMassage, selectedMassage, bonusValue, MAX_MASSAGE_BONUS_DURATION, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.UmaTimesMassaged, CoC.Instance.Flags.Get(GameFlagIds.UmaTimesMassaged) + 1);
                }
            }
        }

        public void UpdateBonusDuration(float hours)
        {

            var effect = Player.StatusEffectByType(StatusEffectIds.UmasMassage);

            if (effect != null)
            {
                effect.Values[2] -= hours;

                if (effect.Values[2] <= 0)
                    BonusExpired();
            }
        }

        public void BonusExpired()
        {

            OutputText("Scenes.Umasshop.BonusExpired.MassageBonusWearsOff".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.UmasMassage);
        }

        public string NeedleworkString(int needleworkType)
        {

            if (needleworkType == NEEDLEWORK_SPEED)
                return "\"Speed\"";
            else
                if (needleworkType == NEEDLEWORK_LUST)
                    return "\"Lust\"";
                else
                    if (needleworkType == NEEDLEWORK_DEFENSE)
                        return "\"Defense\"";
                    else
                        if (needleworkType == NEEDLEWORK_MAGIC)
                            return "\"Magic\"";
                        else
                            if (needleworkType == NEEDLEWORK_ATTACK)
                                return "\"Attack\"";

            return "<b>Somethin' dun fucked up. Please insert bugreport!</b> ";
        }

        public string ListPerkDescription(string perkName)
        {
            foreach (var perk in new[]
                     {
                         PerkIds.ChiReflowAttack, PerkIds.ChiReflowDefense, PerkIds.ChiReflowLust,

                         PerkIds.ChiReflowMagic, PerkIds.ChiReflowSpeed,
                     })

                if (perkName == PerkRegistry.GetName(perk))
                    return PerkRegistry.Get(perk)?.Description ?? "";

            return "";
        }

        public int NeedleworkUndoCost()
        {

            var baseCost = 125;

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesAcupunctureUndo) != 0)
                baseCost += (25 * CoC.Instance.Flags.Get(GameFlagIds.UmaTimesAcupunctureUndo));

            if (baseCost > 1000)
                baseCost = 1000;

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) < 1)
                baseCost *= 2;

            return baseCost;
        }

        public bool HasNeedleworkPerk()
        {

            var pType = this.GetNeedleworkPerk();

            if (pType != null)
                return true;
            else
                return false;
        }

        public Identifier? GetNeedleworkPerk()
        {

            foreach (var _tmp_ in new[] { PerkIds.ChiReflowAttack, PerkIds.ChiReflowDefense, PerkIds.ChiReflowLust, PerkIds.ChiReflowMagic, PerkIds.ChiReflowSpeed })
            {
                var perk = _tmp_;

                if (Player.HasPerk(perk))
                    return perk;
            }

            return null;
        }

        public void ApplyNeedlework(int selectedSession)
        {

            if (selectedSession == NEEDLEWORK_UNDO)
            {
                if (this.GetNeedleworkPerk() is { } needleworkPerk)
                    Player.RemovePerk(needleworkPerk);

                CoC.Instance.Flags.Set(GameFlagIds.UmaTimesAcupunctureUndo, CoC.Instance.Flags.Get(GameFlagIds.UmaTimesAcupunctureUndo) + 1);
            }
            else
                if (selectedSession == NEEDLEWORK_SPEED)
                {
                    Player.CreatePerk(PerkIds.ChiReflowSpeed, 0, 0, 0, 0);

                    OutputText("Scenes.Umasshop.ApplyNeedlework.BoldNeedleworkName".Localized(
                        NEEDLEWORK_SPEED_PERK_DESC
                    ));
                }
                else
                    if (selectedSession == NEEDLEWORK_LUST)
                    {
                        Player.CreatePerk(PerkIds.ChiReflowLust, 0, 0, 0, 0);

                        OutputText("Scenes.Umasshop.ApplyNeedlework.BoldNeedleworkNameAgain".Localized(
                            NEEDLEWORK_LUST_PERK_DESC
                        ));
                    }
                    else
                        if (selectedSession == NEEDLEWORK_DEFENSE)
                        {
                            Player.CreatePerk(PerkIds.ChiReflowDefense, 0, 0, 0, 0);

                            OutputText("Scenes.Umasshop.ApplyNeedlework.BoldNeedleworkNameThird".Localized(
                                NEEDLEWORK_DEFENSE_PERK_DESC
                            ));
                        }
                        else
                            if (selectedSession == NEEDLEWORK_MAGIC)
                            {
                                Player.CreatePerk(PerkIds.ChiReflowMagic, 0, 0, 0, 0);

                                OutputText("Scenes.Umasshop.ApplyNeedlework.BoldNeedleworkNameFourth".Localized(
                                    NEEDLEWORK_MAGIC_PERK_DESC
                                ));
                            }
                            else
                                if (selectedSession == NEEDLEWORK_ATTACK)
                                {
                                    Player.CreatePerk(PerkIds.ChiReflowAttack, 0, 0, 0, 0);

                                    OutputText("Scenes.Umasshop.ApplyNeedlework.BoldNeedleworkNameFifth".Localized(
                                        NEEDLEWORK_ATTACK_PERK_DESC
                                    ));
                                }
        }

        public void AcupunctureMenu()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.AcupunctureMenu.PlayerWantsAcupuncture".Localized());

            sessionCost = 125;

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) < 1)
                sessionCost *= 2;

            if (HasNeedleworkPerk())
            {
                sessionCost = this.NeedleworkUndoCost();

                OutputText("Scenes.Umasshop.AcupunctureMenu.UmaExplainsUndoingChi".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.PlayerAsksHowDifficult".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.MasseuseInspectsYourChi".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) >= 1 ? "but, seeing as you're my little hoppers special someone, just for you I will have to ask for " + (sessionCost).ToString() + " gems. That's half the price of what I'd usually charge for work this difficult." : "I will have to ask for " + (sessionCost).ToString() + " gems."
                ));

                ClearButtons();

                AddButton(0, "Common.Button.Undo".Localized(), () => NeedleworkSession(NEEDLEWORK_UNDO));
            }
            else
            {
                OutputText("Scenes.Umasshop.AcupunctureMenu.UmaWarnsEffectsPermanent".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.PlayerAsksEffectOptions".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.UmaOffersCatalogInstead".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.PlayerAgreesCatalogUseful".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.UmaWarnsChiDisruption".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.OffersSwiftnessBoost".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.OffersLustResistance".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.OffersDamageResistance".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.OffersMageChiRedirect".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.OffersOffenseOptimization".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.UmaRecallsNothingElse".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) >= 1 ? "given you're my little Loppe's special someone, shall we say " + (sessionCost).ToString() + " gems? That's half-price of what I'd usually charge." : "it will be " + (sessionCost).ToString() + " gems."
                ));

                ClearButtons();

                AddButton(0, "Stats.Speed.Name".Localized(), () => NeedleworkSession(NEEDLEWORK_SPEED));

                AddButton(1, "Common.Button.Lust".Localized(), () => NeedleworkSession(NEEDLEWORK_LUST));

                AddButton(2, "Common.Button.Defense".Localized(), () => NeedleworkSession(NEEDLEWORK_DEFENSE));

                AddButton(3, "Codex.Category.Magic".Localized(), () => NeedleworkSession(NEEDLEWORK_MAGIC));

                AddButton(4, "Common.Button.Attack".Localized(), () => NeedleworkSession(NEEDLEWORK_ATTACK));
            }

            AddButton(9, "Common.Button.NoThanks".Localized(), NeedleworkTurnDown);

            if (Player.Gems < sessionCost)
            {
                OutputText("Scenes.Umasshop.AcupunctureMenu.PlayerApologizesNoGems".Localized());

                OutputText("Scenes.Umasshop.AcupunctureMenu.UmaOffersReturnVisit".Localized());

                ClearButtons();

                ReturnToCamp(1);

                return ;
            }

            OutputText("Scenes.Umasshop.AcupunctureMenu.PlayerNodsConsideringOptions".Localized());

            if (HasNeedleworkPerk())
                OutputText("Scenes.Umasshop.AcupunctureMenu.ConsideringUmaInformation".Localized());
            else
                OutputText("Scenes.Umasshop.AcupunctureMenu.WonderingWhatToChoose".Localized());
        }

        public void NeedleworkTurnDown()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.NeedleworkTurnDown.PlayerDeclinesAcupuncture".Localized());

            OutputText("Scenes.Umasshop.NeedleworkTurnDown.UmaRespectsYourDecision".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => EnterClinic(true));
        }

        public void NeedleworkSession(int selectedSession)
        {
            ClearOutput();

            if (selectedSession == NEEDLEWORK_UNDO)
            {
                OutputText("Scenes.Umasshop.NeedleworkSession.PlayerAsksRemoveEffects".Localized());

                OutputText("Scenes.Umasshop.NeedleworkSession.UmaAgreesSlowRemoval".Localized());

                Player.Gems -= sessionCost;
            }
            else
            {
                OutputText("Scenes.Umasshop.NeedleworkSession.PlayerRequestsNeedleworkType".Localized(
                    this.NeedleworkString(selectedSession)
                ));

                OutputText("Scenes.Umasshop.NeedleworkSession.LeadsToTreatmentRoom".Localized());

                Player.Gems -= sessionCost;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => DoNeedleworkSession(selectedSession));
        }

        public void DoNeedleworkSession(int selectedSession)
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.DoNeedleworkSession.RoomAmbienceDescription".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.UmaAsksUndressing".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PlayerSaysWillUndress".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Umasshop.DoNeedleworkSession.ThrillOfBeingNaked".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.UmaReturnsWithSupplies".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.UmaAsksIfReady".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PlayerConfirmsReadiness".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.UmaAppliesNumbingCream".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PlayerWaitsForNumbness".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.UmaBeginsNeedling".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PlayerFeelsNeedlePricks".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PlayerAsksToRise".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.UmaSaysChiStable".Localized());

            OutputText("Scenes.Umasshop.DoNeedleworkSession.PlayerThanksThenDeparts".Localized());

            ApplyNeedlework(selectedSession);

            ClearButtons();

            ReturnToCamp(1);
        }

        public void TalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkMenu.PlayerWantsToChat".Localized());

            OutputText("Scenes.Umasshop.TalkMenu.UmaAsksTalkTopic".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Job".Localized(), TalkJob);

            AddButton(1, "Common.Button.Sexuality".Localized(), TalkSexuality);

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) >= 1)
            {
                AddButton(2, "Common.Button.Loppe".Localized(), TalkLoppe);

                AddButton(3, "Common.Button.LoppesDad".Localized(), TalkLoppesDad);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => EnterClinic(true));
        }

        public void TalkJob()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkJob.CuriousAboutHerCraft".Localized());

            OutputText("Scenes.Umasshop.TalkJob.UmaHealerExplanation".Localized());

            OutputText("Scenes.Umasshop.TalkJob.ChiEnergyExplanation".Localized());

            OutputText("Scenes.Umasshop.TalkJob.WhyManipulateChi".Localized());

            OutputText("Scenes.Umasshop.TalkJob.ChiBenefits".Localized());

            OutputText("Scenes.Umasshop.TalkJob.MassageStyleQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkJob.DoInAdjustsFlow".Localized());

            OutputText("Scenes.Umasshop.TalkJob.AcupunctureEffectsQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkJob.NeedleKeyPoints".Localized());

            OutputText("Scenes.Umasshop.TalkJob.DigestingExplanation".Localized());

            OutputText("Scenes.Umasshop.TalkJob.AcupunctureDisruption".Localized());

            OutputText("Scenes.Umasshop.TalkJob.AppreciateHerLesson".Localized());

            OutputText("Scenes.Umasshop.TalkJob.InvitationToVisit".Localized());

            OutputText("Scenes.Umasshop.TalkJob.PromiseToTry".Localized());

            ClearButtons();

            ReturnToCamp(1);
        }

        public void TalkSexuality()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkSexuality.SexualityQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.DistantReminiscence".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.LesbianDefinitionQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.StudiesShapedPreference".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.FeminineFacePreference".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.PartnerFindingChallenge".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.WomenPreferNatural".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.MaleVisitorType".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.FiftyFiftyChance".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.DiscretionEffortQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.EasternClinicCustoms".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.ReputationRemark".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.EmptyClinicRemark".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.ThanksAndFarewell".Localized());

            OutputText("Scenes.Umasshop.TalkSexuality.PromiseToReturn".Localized());

            ClearButtons();

            ReturnToCamp(1);
        }

        public void TalkLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppe.ChildhoodStoryRequest".Localized());

            OutputText("Scenes.Umasshop.TalkLoppe.PonderingStory".Localized());

            _loppeTalks[Random.Shared.Next(_loppeTalks.Count)]();
        }

        public void TalkLoppeJoiner()
        {

            ClearButtons();

            ReturnToCamp(1);
        }

        public void TalkLoppeAttitudes()
        {

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.AskOpinionOfLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.UnsureWhatSheMeans".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.TroublesomeChildhood".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.ReflectingOnChildhood".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.HandfulAsChild".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.TomboySurprise".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.PubertyTransformation".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.TroublingTransition".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.BondDuringPuberty".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.SlipsUpQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.UnladylikeHabits".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.NeverWitnessedHabits".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.CarefulAroundYou".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.LessEffortToImpress".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.WouldntHelpAdvice".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.DropHintRequest".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.FutileHintAdvice".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.ConfessImpression".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeAttitudes.WarmFarewell".Localized());

            TalkLoppeJoiner();
        }

        public void TalkLoppeCarrotIncident()
        {

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.RabbitCarrotFact".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.VillageSaying".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.CarrotCakeTangent".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.ClicheRemark".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.CarrotExperimentSetup".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.StoryCuriosity".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.PiquedInterest".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.DisbelievingGrin".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.CaughtInAct".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.DisbeliefQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.DenialAndInsight".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.TeasingSuggestion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.MissingCarrots".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.GratefulForStory".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.CountingOnIt".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.NoPromisesFarewell".Localized());

            OutputText("Scenes.Umasshop.TalkLoppeCarrotIncident.CasualGoodbye".Localized());

            TalkLoppeJoiner();
        }

        public void TalkLoppePuberty()
        {

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaAsksAboutStamina".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.WryGrinConfirmation".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaRecallsTroubles".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.BlinkAtContradiction".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaExplainsCurse".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.ProtestMagicNature".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaAdmitsAccidents".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.MentalImageAmusement".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaDetailsLeakage".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.SympathizeAndAsk".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.MorningDrainingsReply".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.RepeatDrainingsPuzzled".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaExplainsRelief".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.ChuckleAtEnjoyment".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaDeniesEnjoyment".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.CluckTongueWonder".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaGigglesPatience".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.ChuckleAtHumor".Localized());

            if (Player.Femininity >= UMA_CONSIDER_PC_FEM)
            {
                OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaIndignantOffer".Localized());

                if (Player.Feet() != "feet")
                    OutputText("Scenes.Umasshop.TalkLoppePuberty.TermForYouPrefix".Localized());

                OutputText("Scenes.Umasshop.TalkLoppePuberty.WhoIsOldSuffix".Localized());
            }
            else
            {
                OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaIndignantTypeRemark".Localized());

                if (Player.Feet() != "feet")
                    OutputText("Scenes.Umasshop.TalkLoppePuberty.TermPrefixAgain".Localized());

                OutputText("Scenes.Umasshop.TalkLoppePuberty.WhoIsOldAgain".Localized());
            }

            OutputText("Scenes.Umasshop.TalkLoppePuberty.TeasePervertPride".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaGlaresLifeAdvice".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.SilverMareCompliment".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaSnortsApproval".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.AskAboutFather".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaLooksDistant".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.ApologizeForIntrusion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaSmilesPleasure".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.PrepareToLeave".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.UmaExplainsBluntly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppePuberty.StifleLaughShock".Localized());

            TalkLoppeJoiner();
        }

        public void TalkLoppesFirstGirl()
        {

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.UmaOffersStory".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.AcceptStoryCuriously".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.UmaSuggestsSeating".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.SettleInWondering".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.UmaIntroducesPuff".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.InterjectNameDisbelief".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.UmaExplainsStreetName".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.ShakeHeadContinue".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirl.UmaBeginsCarpenterTale".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstGirlPart2);
        }

        public void TalkLoppesFirstGirlPart2()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.StoryBannerYearsAgo".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.SlowDayBoredom".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.PuffAtHerCorner".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.LoppeWalksThroughTown".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.CatShopkeeperGreets".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.LoppeJustBrowses".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.ElderlyCatCompliments".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.LoppeSpotsCarrotCake".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.LoppeHeadsToCarpenter".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.PuffScansForClients".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.PuffExpectsBusiness".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.PuffTargetsBunny".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart2.PuffCallsToLoppe".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstGirlPart3);
        }

        public void TalkLoppesFirstGirlPart3()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeBlinksWhoSpeaks".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeLicksLipsNervously".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuffChecksOnLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeSqueaksAvoids".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuffOpensRobes".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeStammersStares".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.WolfessFlattersLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuzzledByFriendTerm".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.WolfessGropesLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeFleesEmbarrassed".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuffCajolesWinks".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.ProtestLackOfMoney".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuffPanicsLoss".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeHesitatesCurious".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeFrozenWorry".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuffSweetensDeal".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.FantasyPlusCake".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.LoppeGivesHerName".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.HeartPoundingNerves".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart3.PuffPreparesAtHome".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstGirlPart4);
        }

        public void TalkLoppesFirstGirlPart4()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.StoryBannerPuffHouse".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeKnocksNervously".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.DoorInviteHereGoes".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.ConflictedArousal".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffTeasesSeductively".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeDropsDress".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.WolfessWantsTestDrive".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppePouncesKissing".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffSqueezesPrick".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeBegsGrinding".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeThrustsHome".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeMoansShudders".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffReassuresLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeHissesAgreement".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffGripsHipsTail".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeSubmitsLearning".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.WolfessGigglesCaution".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeCriesProphecy".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffGrinsStillHard".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeGrinsWolfishly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffBucksEagerly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeSquealsThrusting".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffEarsPerkStartled".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppeExplainsHorseBalls".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffSecondThoughts".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.LoppePleadsPromise".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart4.PuffAcceptsChallenge".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstGirlPart5);
        }

        public void TalkLoppesFirstGirlPart5()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart5.StoryBannerManyOrgasms".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart5.LoppeTrailsOffClimax".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart5.PuffSpentDrenched".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart5.LoppePecksThanks".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart5.LoppeWhistlesHome".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart5.LoppePanicsCurfew".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstGirlPart6);
        }

        public void TalkLoppesFirstGirlPart6()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.StoryBannerUmaRecounts".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.AskIfUmaAngered".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaDownplaysDiscipline".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.YouSuggestDreamsFaded".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaUncertainReason".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.RepeatPuffQuestion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaDeniesRomance".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.BlinkAndAskWhy".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaRecallsCure".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.ShakeHeadDisbelief".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaAdmitsTrysts".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.AdmitHandfulThenAsk".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaCallsYouSpecial".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.DoubtOutsiderSkill".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaShakesHeadPraise".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.WryJokeAdmirers".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaDefendsStamina".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.ThankBeforeLeaving".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.UmaWishesHealthyBond".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstGirlPart6.PromiseAndDepart".Localized());

            TalkLoppeJoiner();
        }

        public void TalkLoppesFirstBoyfriend()
        {

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaOffersBoyfriendTale".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.ExpressInterestProceed".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaBeginsYouthTale".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.TeaseUmaTastes".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaDismissesNotion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.AskWhatSheMeans".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaInsistsOnListening".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.PromiseQuietListening".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaRecallsPopularBoy".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.PraiseLoppeDancing".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaExplainsSecrecy".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.AskAboutGenderPose".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaExplainsGenderChange".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UnderstandAccidentTiming".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaRecallsShortsRip".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.OddWayOfCharming".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaClapsAsksContinue".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.SuggestMakeLove".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaCorrectsDate".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.SurpriseAtSlowPace".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaScoldsRomance".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.ApologizeMonsterWays".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaRecallsSimpleLives".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.AdmitAndContinue".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriend.UmaContinuesDatingTale".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstBoyfriendPart2);
        }

        public void TalkLoppesFirstBoyfriendPart2()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.FlashbackTitle".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouReassures".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeHidesArousal".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouEmbracesLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeNodsWarns".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouStrokesPrick".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeSwellsErect".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouAdmiresBody".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeCallsItWeird".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouStripsTeasing".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeStaresHungrily".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouComparesSizes".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeRealizesSize".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouDismissesWorry".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppeDemandsAction".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouFeignsConfusion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppePoutsTeasing".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouChallengesChase".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.LoppePouncesHyou".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart2.HyouAsksIntent".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstBoyfriendPart3);
        }

        public void TalkLoppesFirstBoyfriendPart3()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeEndsForeplay".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouGroansGrinding".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeCorrectsHyou".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouGuidesGently".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeSwishesTail".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeSaysSitDown".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeFlopsDown".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouSteadiesHips".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeRidesSilently".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouMoansApproval".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeNoticesTrouble".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouSavesFace".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeProbesConcern".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouAdmitsDiscomfort".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeTurnsAround".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouPurrsApproval".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeBouncesEagerly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouBegsForSlaps".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeCallsHimKinky".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouNearsClimax".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeFilesSuspicion".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.HyouClimaxesFinally".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart3.LoppeFeelsFilled".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstBoyfriendPart4);
        }

        public void TalkLoppesFirstBoyfriendPart4()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouApologizesWeakly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeDemandsAnother".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouCollapsesGroaning".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeBoastsStamina".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouDoubtsNormalcy".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeWhinesNeedily".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouOffersCompromise".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeProtestsRole".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouGivesIn".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeShuddersPleasure".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouSlurpsTip".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeLeftSpeechless".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouOffersSwap".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeAsksMethod".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouAsksToMove".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppePullsOffConfused".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouPresentsRear".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeAcceptsIdea".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouBegsForIt".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeLicksLips".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouYowlsBliss".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeGigglesWarning".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouGetsHardAgain".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppePlansNextRound".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.SexMontageHeader".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.HyouLicksCum".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart4.LoppeComfortsHyou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TalkLoppesFirstBoyfriendPart5);
        }

        public void TalkLoppesFirstBoyfriendPart5()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaStoryHeader".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerNotesIrony".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaAgreesDryly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerQuestionsMatch".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaExplainsPast".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerSuggestsSwap".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaContinuesStory".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerExpressesSurprise".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaRejectsPremise".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerProposesReason".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaCorrectsReason".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerSighsResigned".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaRecallsFondly".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerPraisesLoppe".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaAgreesLaughing".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerSaysFarewell".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.UmaJokesFarewell".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesFirstBoyfriendPart5.PlayerChucklesReply".Localized());

            TalkLoppeJoiner();
        }

        public void TalkLoppesDad()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.TalkLoppesDad.AskAboutBunnyHerm".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.RecallsOldLove".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.ProbesOldBond".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.ConfessesLostLove".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.AskWhoUsagiWas".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.WanderingWarriorTale".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.FirstMeetingTale".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.InfersPartingGift".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.RevealsPregnancy".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.AskIfTriedMessaging".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.QueriedTravelers".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.ThankForSharing".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.StillLovesHerDeclared".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.LingeringGlance".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.NoteSisterIfReturn".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.ThoughtfulNodSister".Localized());

            OutputText("Scenes.Umasshop.TalkLoppesDad.OffersHope".Localized());

            ClearButtons();

            ReturnToCamp(1);
        }

        public void SexMenu()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Umasshop.SexMenu.HesitateBeforeAsking".Localized());

                BuildCoreMenu(true);

                return ;
            }

            OutputText("Scenes.Umasshop.SexMenu.RequestSexAppraised".Localized());

            if (Player.Femininity >= UMA_CONSIDER_PC_FEM)
            {
                OutputText("Scenes.Umasshop.SexMenu.ComplimentsYourCurves".Localized());

                if (Player.Gender == Gender.Male)
                {
                    OutputText("Scenes.Umasshop.SexMenu.AdmitMaleDespiteGirlish".Localized());

                    OutputText("Scenes.Umasshop.SexMenu.PrettyFemboySmirk".Localized());
                }
                else
                    if (Player.Gender == Gender.Female)
                    {
                        OutputText("Scenes.Umasshop.SexMenu.LesbianLovingSmirk".Localized());

                        OutputText("Scenes.Umasshop.SexMenu.TongueCaressSkill".Localized());
                    }
                    else
                        if (Player.Gender == Gender.Hermaphrodite)
                        {
                            OutputText("Scenes.Umasshop.SexMenu.ShrugGirlWithCock".Localized());

                            OutputText("Scenes.Umasshop.SexMenu.GirlDespiteCock".Localized());
                        }
            }
            else
                if (Player.Femininity >= UMA_CONSIDER_PC_AND)
                {
                    OutputText("Scenes.Umasshop.SexMenu.LesbianDisclosure".Localized());

                    if (Player.Gender == Gender.Male)
                    {
                        OutputText("Scenes.Umasshop.SexMenu.ConfessMalePassable".Localized());

                        OutputText("Scenes.Umasshop.SexMenu.DisappointedRefusal".Localized());

                        BuildCoreMenu(true);

                        return ;
                    }
                    else
                        if (Player.Gender == Gender.Female)
                        {
                            OutputText("Scenes.Umasshop.SexMenu.InsistsGirlhood".Localized());

                            OutputText("Scenes.Umasshop.SexMenu.ApplauseGirlsPride".Localized());
                        }
                        else
                            if (Player.Gender == Gender.Hermaphrodite)
                            {
                                OutputText("Scenes.Umasshop.SexMenu.ConfessBothSexes".Localized());

                                OutputText("Scenes.Umasshop.SexMenu.PrefersFeminineHerms".Localized());
                            }
                }
                else
                {
                    OutputText("Scenes.Umasshop.SexMenu.ManlyRefusal".Localized());

                    BuildCoreMenu(true);

                    return ;
                }

            OutputText("Scenes.Umasshop.SexMenu.NoticesBedTable".Localized());

            OutputText("Scenes.Umasshop.SexMenu.ServicesOffered".Localized());

            BuildSexMenu();
        }

        public void BuildSexMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Eromassage".Localized(), SexEroticMassage);

            AddButton(1, "Common.Button.EatHerOut".Localized(), SexEatHerOut);

            if (Player.HasVagina() && !Player.IsTaur())
                AddButton(2, "Common.Button.Fingerbang".Localized(), SexGetFingered);
            else
                AddButton(2, "Common.Button.Fingerbang".Localized(), null);

            if (Player.HasCock())
            {
                AddButton(3, "Common.Button.Handjob".Localized(), SexHandjob);

                AddButton(4, "Common.Button.GetBlown".Localized(), SexGetABJFromDisMilfyLesboSlut);
            }
            else
            {
                AddButton(3, "Common.Button.Handjob".Localized(), null);

                AddButton(4, "Common.Button.GetBlown".Localized(), null);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => BuildCoreMenu());
        }

        public void SexEroticMassage()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.SexEroticMassage.AcceptEroticMassage".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.FollowForMassage".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.MassageRoomTour".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
                OutputText("Scenes.Umasshop.SexEroticMassage.SpinWithUrta".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.TableAndHeadboard".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.AttentionThroatClear".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.NodAndStripDown".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Umasshop.SexEroticMassage.ResistLustTingle".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.CupboardForCloth".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.NakedUmaBehindYou".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.InviteOntoTable".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.AgreesToBegin".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.IncrediblyArousedLead".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Umasshop.SexEroticMassage.HardWord".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Umasshop.SexEroticMassage.WetWord".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Umasshop.SexEroticMassage.HardAndWetWord".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.SparksAcrossSkin".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.GroanThroughClimax".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Umasshop.SexEroticMassage.CumWord".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Umasshop.SexEroticMassage.DribblingWord".Localized());
                else
                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Umasshop.SexEroticMassage.PouringWord".Localized());
                    else
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Umasshop.SexEroticMassage.SpurtingWord".Localized());
                        else
                            OutputText("Scenes.Umasshop.SexEroticMassage.FountainingWord".Localized());

                OutputText("Scenes.Umasshop.SexEroticMassage.FromEachcockPrefix".Localized());
            }

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Umasshop.SexEroticMassage.AndWord".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Umasshop.SexEroticMassage.FemcumWord".Localized());

                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Umasshop.SexEroticMassage.DrippingWord".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Umasshop.SexEroticMassage.StreamingWord".Localized());
                    else
                        OutputText("Scenes.Umasshop.SexEroticMassage.GushingWord".Localized());

                OutputText("Scenes.Umasshop.SexEroticMassage.FromYourVaginaPrefix".Localized());
            }

            OutputText("Scenes.Umasshop.SexEroticMassage.ClimaxOnTable".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Umasshop.SexEroticMassage.CumWordAgain".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Umasshop.SexEroticMassage.AndWordAgain".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Umasshop.SexEroticMassage.JuicesWord".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.SmilesKnowingly".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.AfterglowQuestion".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.TooSpentToSpeak".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.TrickWasGenius".Localized());

            OutputText("Scenes.Umasshop.SexEroticMassage.RestToRecover".Localized());

            var hoursSinceCum = (int)(Player.HoursSinceCum);

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            Player.HoursSinceCum = (float)Math.Ceiling(hoursSinceCum * 0.75f);

            CoC.Instance.Flags.Set(GameFlagIds.TimesOrgasmed, CoC.Instance.Flags.Get(GameFlagIds.TimesOrgasmed) + 1);

            ClearButtons();

            ReturnToCamp(1);
        }

        public void SexGetFingered()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.SexGetFingered.WinkAtMasseur".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.MasseurLeadsWay".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.SimpleMassageRoom".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
                OutputText("Scenes.Umasshop.SexGetFingered.UrtaWouldEnjoy".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.BambooMassageTable".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.ClearsThroatForAttention".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Umasshop.SexGetFingered.UndressesForMassage".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.FoldsArmorAway".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.UmaClothesFolded".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.PuzzledThenSmile".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Umasshop.SexGetFingered.ChildOnMotherLap".Localized());

            if (Player.Tallness > 84)
                OutputText("Scenes.Umasshop.SexGetFingered.FearOfSquashingHer".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.AsksWhatNext".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.PromptsToRelax".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.UnnoticedHandMoves".Localized());

            if (Player.Thickness < 10 && Player.Tone > 90)
                OutputText("Scenes.Umasshop.SexGetFingered.ChiseledAbdomenTouch".Localized());
            else
                if (Player.Thickness < 30 && Player.Tone > 70)
                    OutputText("Scenes.Umasshop.SexGetFingered.TonedMidriffGlide".Localized());
                else
                    if (Player.Thickness > 90 && Player.Tone < 10)
                        OutputText("Scenes.Umasshop.SexGetFingered.ChubbyBellyGrope".Localized());
                    else
                        if (Player.Thickness > 70 && Player.Tone < 30)
                            OutputText("Scenes.Umasshop.SexGetFingered.PaunchBellySink".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Umasshop.SexGetFingered.PastEachCock".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Umasshop.SexGetFingered.HoneypotApproach".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Umasshop.SexGetFingered.SettlesOnLegs".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetFingered.NestledBetweenLegs".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.SpreadsFoldsOpen".Localized());
            }
            else
                OutputText("Scenes.Umasshop.SexGetFingered.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.ShiversInAnticipation".Localized());

            OutputText("Scenes.Umasshop.SexGetFingered.CirclesNetherlips".Localized());

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
            {
                OutputText("Scenes.Umasshop.SexGetFingered.AsksIfVirgin".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.MurmursNoDoubt".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.CriesWonderfulPraise".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.FullBodyJolt".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.AnotherFingerAdded".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.BabblesPromiseReturn".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.EcstaticOrgasmCry".Localized());

                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Umasshop.SexGetFingered.SpattersWord".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Umasshop.SexGetFingered.SoaksWord".Localized());
                    else
                        OutputText("Scenes.Umasshop.SexGetFingered.DrenchesWord".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.ProbingHandInJuices".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Umasshop.SexGetFingered.PearlescentArc".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.PeriodParagraphBreakAgain".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.LaughsWhilePumping".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.RegainControlLegs".Localized());

                OutputText("Scenes.Umasshop.SexGetFingered.SmileAndRedress".Localized());
            }
            else
                if (Player.Vaginas[0].Looseness == 0)
                {
                    OutputText("Scenes.Umasshop.SexGetFingered.TeasesAboutTightness".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.DefendsLoppe".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.MotherKnowsLoppe".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.WrigglesForAccess".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.ChucklesRealThing".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.RealizesSearchTarget".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.FoundItDelight".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.GaspsAtExpertFingers".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.LicksHandClean".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.LazySmilePraise".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.SmilesAboutLoppe".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.DemureKissAndTell".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.ShutsDoorOnExit".Localized());
                }
                else
                {
                    OutputText("Scenes.Umasshop.SexGetFingered.SuggestsLoppeFault".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.SqueaksMaybeLittle".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.RoomForAnother".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.UnthinkingReply".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.FingersMoveInside".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.LooserThanMost".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.LastFingerTease".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.NotBadSubstitute".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.SmallOrgasmRemark".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.OffersAnotherRound".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.PolitelyDeclines".Localized());

                    OutputText("Scenes.Umasshop.SexGetFingered.ThanksForMagicHands".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            ClearButtons();

            ReturnToCamp(1);
        }

        public void SexHandjob()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.SexHandjob.AskHandjobAfterRubbing".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.LeadAwayDownCorridor".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.SimpleWoodenRoom".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
                OutputText("Scenes.Umasshop.SexHandjob.WonderAboutUrtaJoining".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.BambooTableWithCloth".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.UndressAndBeckonYou".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Umasshop.SexHandjob.LustAtBeingNaked".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.GatherArmorThenCupboard".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.UmaGivesFoldedClothes".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.UmaSitsBesideYou".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.TellHerDoingWell".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.AskWhatSheMeans".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.MoanAndShakeHead".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.AgreeWhileHumpingHand".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.ChuckleThenGaspApology".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.UmaAsksAboutSex".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.PressWhilePleasuring".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.TwistHandAroundShaft".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.ExplodeInHerHands".Localized());

            if (Player.CumQ() <= 250)
            {
                OutputText("Scenes.Umasshop.SexHandjob.CumSplattersOnHands".Localized());

                OutputText("Scenes.Umasshop.SexHandjob.RelievedBreathPraise".Localized());
            }
            else
                if (Player.CumQ() <= 500)
                {
                    OutputText("Scenes.Umasshop.SexHandjob.SeedOverflowsHerHands".Localized());

                    OutputText("Scenes.Umasshop.SexHandjob.ThankHerFeelingBetter".Localized());
                }
                else
                    if (Player.CumQ() <= 1000)
                    {
                        OutputText("Scenes.Umasshop.SexHandjob.SpunkTorrentNearlyHits".Localized());

                        OutputText("Scenes.Umasshop.SexHandjob.WonderAtNearMiss".Localized());
                    }
                    else
                        if (Player.CumQ() <= 2500)
                        {
                            OutputText("Scenes.Umasshop.SexHandjob.UmaYelpsAtGush".Localized());

                            OutputText("Scenes.Umasshop.SexHandjob.RelievedGlanceAtMess".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Umasshop.SexHandjob.FirstGoutHitsFace".Localized());

                            OutputText("Scenes.Umasshop.SexHandjob.BreathyIncredibleSigh".Localized());
                        }

            OutputText("Scenes.Umasshop.SexHandjob.RiseAndReachCupboard".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Umasshop.SexHandjob.SloshThroughFloorCum".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.FetchBothOwnClothes".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Umasshop.SexHandjob.AvoidSoakingClothes".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.OfferUmaClothesThanks".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Umasshop.SexHandjob.IncludeTheRoom".Localized());

            OutputText("Scenes.Umasshop.SexHandjob.FarewellWaveGoodbye".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            ClearButtons();

            ReturnToCamp(1);
        }

        public void SexGetABJFromDisMilfyLesboSlut()
        {
            ClearOutput();

            var x = Player.CockThatFits(25);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.CumAwareFidget".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SLetter".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.AskUmaForRelease".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PussyAssOffLimits".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.YouWord".Localized());

            if (Player.LustPercent >= 75)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.EagerlyWord".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ThanksForConsideration".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SLetterAgain".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.GrowthWorthDiscomfort".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SimpleDrainRoom".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.WonderIfUrtaLikes".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.BambooTableWithHole".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.CheerfulStripOrder".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.NakedLustTingle".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.AndBallsFragment".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SentencePeriod".Localized());
            }
            else
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.GatherClothesInArms".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.CupboardTowelStash".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.StepAsideFromUma".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SettleHeadThroughHole".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.MotherlyExaminesShaft".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 2)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ReluctantGlance".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 5)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SmilingLittleAtYou".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.GladToHelpYou".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.MouthWideOpen".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.YourWord".Localized());
            else
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ASingleWord".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.DickSlurpIntoMouth".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.EquineFaceAdvantage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) >= 2)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ChangingThatArentYou".Localized());
            else
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SentencePeriodAgain".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PlayfulUndersideLick".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 2)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.TasteNotBad".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 5)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.UsedToYourTaste".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SecretlyLikesHelping".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SpeechCloseTag".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.GruntsHipsShiver".Localized(
                Player.CockHead(x)
            ));

            if (Player.Cocks[x].Length < 9)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.UntilSheathHit".Localized());
            else
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.MouthFullToHandle".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.HotBreathOnCrotch".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.MuffledVoiceMumble".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 2)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PrefersGirlsTaste".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 5)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.RatingNotTooBad".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.RefillRequest".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SpeechCloseTagAgain".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PoliteHandSuggestion".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.OneOfWord".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.HandsOnSparePrick".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SLetterThird".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.HummingShockwaves".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 3)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ImpatientToFinish".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.EncouragedBySwell".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SheWrapsFragment".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.OneHandPumpsTool".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.BothHandsPumpTools".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.FirmHandTwists".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.HandPluralSuffix".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SoftPalmClause".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PluralAreSuffix".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.IsCopulaWord".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PerfectMateClause".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.LengthPluralSuffix".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.DribblingDripping".Localized());
            }

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SteadyMouthPleasure".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ExplainPleasantly".Localized());
            else
                if (Player.Corruption <= 66)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ExplainPlainly".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ExplainHarshly".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PeriodSpace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 2)
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.NodAgreementPole".Localized());
            else
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PlungeBeforeFinish".Localized());

            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.MoanHipsLift".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 5)
            {
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PullsOffPrelude".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.EruptPluralSuffix".Localized());
                else
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.EruptsWord".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.NoLongerEnclosed".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SprayLikeSprinkler".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.MassiveCumsprays".Localized());
            }
            else
            {
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.HoldsHerPlace".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() <= 25)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SwallowsEveryDrop".Localized());
                else
                    if (Player.CumQ() <= 100)
                    {
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SwallowAverageLoad".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.RollsToSack".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SwallowCheeksBulge".Localized());

                        if (Player.CumQ() >= 1000)
                        {
                            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.DrainsGurgleLoad".Localized());

                            if (Player.CumQ() <= 2000)
                                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.PoolWord".Localized());
                            else
                                if (Player.CumQ() <= 30000)
                                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.BathtubWorthWord".Localized());
                                else
                                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.LakeWord".Localized());

                            OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.OfSemenProduced".Localized());
                        }

                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.CrotchGlazedStuff".Localized());
                    }

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.AngledToSide".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.SidePluralSuffix".Localized());

                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.OtherErectionClause".Localized());

                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ErectionPluralRelease".Localized());
                    else
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ReleasesWord".Localized());

                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.FitfullySagging".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ItsPossessive".Localized());
                    else
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.TheirPossessive".Localized());

                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.LewdDanceImitation".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ItsRopesPhrase".Localized());
                    else
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.TheirRopesPhrase".Localized());

                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.NotNearlyAsThick".Localized());
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 2)
            {
                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.WipesFaceApology".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.TellHerRemember".Localized());

                if (Player.Corruption <= 40)
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ApologizeConnector".Localized());

                OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.GetDressedReturn".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) < 5)
                {
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.WipesFaceTrying".Localized());

                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.WinkAndDepart".Localized());
                }
                else
                {
                    OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.LicksLipsSwallow".Localized());

                    if (Player.CumQ() > 25)
                    {
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.ExtendsTowelOffer".Localized());

                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.WipeUpRueful".Localized());
                    }
                    else
                        OutputText("Scenes.Umasshop.SexGetABJFromDisMilfyLesboSlut.DressesWhileCooling".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

            CoC.Instance.Flags.Set(GameFlagIds.UmaTimesSuckedYou, CoC.Instance.Flags.Get(GameFlagIds.UmaTimesSuckedYou) + 1);

            ClearButtons();

            ReturnToCamp(1);
        }

        public void SexEatHerOut()
        {
            ClearOutput();

            OutputText("Scenes.Umasshop.SexEatHerOut.SmirkAskToMinister".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.HandsOnlyTease".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.DeclineForHerSake".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.LickLipsPonder".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.EscortToBackRoom".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.BackroomInterior".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
                OutputText("Scenes.Umasshop.SexEatHerOut.MuseAboutUrta".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.BambooTable".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.WatchUmaUndress".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.UmaStrutsToTable".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.KneelAndInhale".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Umasshop.SexEatHerOut.ArousalSwelling".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Umasshop.SexEatHerOut.WetnessGathering".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.GazeForPermission".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.SeductiveInvite".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.BeginCunnilingus".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.AppreciativeSqueeze".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.TongueDigsDeeper".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.ApprovingHeadPat".Localized());

            if (Player.Ears.Id == BodyPartTypes.Dog || Player.Ears.Id == BodyPartTypes.Fox || Player.Ears.Id == BodyPartTypes.Cat || Player.Ears.Id == BodyPartTypes.Bunny)
                OutputText("Scenes.Umasshop.SexEatHerOut.BetweenYourPrefix".Localized());

            if (Player.Ears.Id == BodyPartTypes.Dog)
                OutputText("Scenes.Umasshop.SexEatHerOut.CanineWord".Localized());

            if (Player.Ears.Id == BodyPartTypes.Fox)
                OutputText("Scenes.Umasshop.SexEatHerOut.VulpineWord".Localized());

            if (Player.Ears.Id == BodyPartTypes.Cat)
                OutputText("Scenes.Umasshop.SexEatHerOut.FelineWord".Localized());

            if (Player.Ears.Id == BodyPartTypes.Bunny)
                OutputText("Scenes.Umasshop.SexEatHerOut.LoporidWord".Localized());

            if (Player.Ears.Id == BodyPartTypes.Dog || Player.Ears.Id == BodyPartTypes.Fox || Player.Ears.Id == BodyPartTypes.Cat || Player.Ears.Id == BodyPartTypes.Bunny)
                OutputText("Scenes.Umasshop.SexEatHerOut.EarsWord".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.CrooningPraise".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.PauseToBreathe".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.ClimaxLegLock".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.BaskInFlattery".Localized());

            OutputText("Scenes.Umasshop.SexEatHerOut.PromiseToReturn".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            ClearButtons();

            ReturnToCamp(1);
        }

    }
}