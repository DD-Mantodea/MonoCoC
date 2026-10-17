using MonoCoC.Attributes;
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
using MonoCoC.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Quests
{
    [Register<UrtaQuestScene>("coc:urta_quest")]
    public class UrtaQuestScene : Scene
    {
        public ItemStack? urtaQItems1;

        public ItemStack? urtaQItems2;

        public ItemStack? urtaQItems3;

        public ItemStack? urtaQItems4;

        public ItemStack? urtaQItems5;

        public ItemStack? urtaQItems6;

        public ItemStack? urtaQItems7;

        public ItemStack? urtaQItems8;

        public ItemStack? urtaQItems9;

        public ItemStack? urtaQItems10;

        public Player? player2;

        public PlayerScope? playerScope;

        public Player player => CoC.Instance.Player;

        public new static void ReturnToCamp(int hours)
        {
            var quest = SceneInstance<UrtaQuestScene>.Get();

            if (quest.player2 != null)
                quest.ResetToPC();

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
        }

        public override void Entry()
        {

        }

        public bool UrtaBusy()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EdrynPregnancyType) == 31 || CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == -1;
        }

        public bool IsUrta()
        {

            if ((CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) > 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) < 1) && ((Player.ShortName == "Urta") && ((Player.HasCock()) && ((Player.Cocks[0].Length == 20 && Player.Cocks[0].Thickness == 3) && ((Player.HasVagina()) && (Player.BreastRows[0].BreastRating == 7))))))
                                    return true;

            return false;
        }

        public void TalkWithUrtaAboutFamFam()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.UrtaRecoilsAtTopic".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.VixenSighsSadly".Localized((noFur ? "" : ", all fluffy and new")));

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.UrtaActsStrong".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.UrtaTrailsOffBlushing".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.ShowOfShrugging".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.EagerTopicChange".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.BrowsFurrowedInThought".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.OviElixirExplained".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.FinishingHerThought".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.LicksLipsCaught".Localized());

            OutputText("Scenes.UrtaQuest.TalkWithUrtaAboutFamFam.FinalSighThenSmile".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, .5f);

            ClearButtons();

            AddButton(0, "Common.Button.ComfortHer".Localized(), ComfortUrtaAfterFamFamTalk);

            AddButton(1, "Common.Button.Yeesh".Localized(), YeeshUrtaAfterFamFamTalk);

            AddButton(2, "Common.Button.WhoCares".Localized(), WhoCaresUrtaAfterFamFamTalk);
        }

        public void ComfortUrtaAfterFamFamTalk()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.LeansAcrossTable".Localized((noFur ? "" : ". Her downy fur ruffles as you give her a comforting squeeze")));

            OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.WrySmileHusky".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.BoozyWord".Localized());

            OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.KissHugTight".Localized());

            OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.ReturnsHugFully".Localized());

            OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.PullsBackThanks".Localized());

            OutputText("Scenes.UrtaQuest.ComfortUrtaAfterFamFamTalk.NoddingFarewellSmile".Localized());

            UrtaLove(5);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 3);

            DoNext(() => ReturnToCamp(1));
        }

        public void YeeshUrtaAfterFamFamTalk()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.LoudBemusedExhale".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.TailRaisesIndignant".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.StandStretchCircle".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.DismissTragicHistory".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.TipEngorgesMumble".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.LetHerGo".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.NegativeDeclaration".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.NodsAndSwigs".Localized());

            OutputText("Scenes.UrtaQuest.YeeshUrtaAfterFamFamTalk.GoodBitchRemark".Localized());

            UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void WhoCaresUrtaAfterFamFamTalk()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.WhoCaresUrtaAfterFamFamTalk.ColdWhoCares".Localized());

            OutputText("Scenes.UrtaQuest.WhoCaresUrtaAfterFamFamTalk.StiffensAngryTail".Localized());

            OutputText("Scenes.UrtaQuest.WhoCaresUrtaAfterFamFamTalk.RiseAndGlance".Localized());

            OutputText("Scenes.UrtaQuest.WhoCaresUrtaAfterFamFamTalk.GrowlsYouAsked".Localized());

            OutputText("Scenes.UrtaQuest.WhoCaresUrtaAfterFamFamTalk.AskedAboutParents".Localized());

            UrtaLove(-10);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) - 5);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 1)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void InfertilityQuestions()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.AsksAboutInfertility".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.UrtaAdmitsIgnorance".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.HowCanThatBe".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.SighsAtYourLook".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.WellWord".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.UrtaQuest.InfertilityQuestions.NothingToComplain".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.UrtaQuest.InfertilityQuestions.HandfulInSack".Localized());
                else
                    OutputText("Scenes.UrtaQuest.InfertilityQuestions.WillingFuck".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.CatchesHerselfWandering".Localized((noFur ? "" : "gray-furred vulpine ")));

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.ConnectionsAsCaptain".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.MischievousGrin".Localized());

            OutputText("Scenes.UrtaQuest.InfertilityQuestions.CovenantVisitWarning".Localized());

            if (OptionStore.Instance.Get("hardcore", 0) > 0)
                OutputText("Scenes.UrtaQuest.InfertilityQuestions.HardcoreModeWarning".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LookIntoIt".Localized(), StartUrtaQuest);

            AddButton(1, "Common.Button.MaybeLater".Localized(), () => ReturnToCamp(1));
        }

        public void ResetToPC()
        {

            playerScope?.Dispose();

            playerScope = null;

            player2 = null;

            Player.ItemStacks[0] = urtaQItems1;

            Player.ItemStacks[1] = urtaQItems2;

            Player.ItemStacks[2] = urtaQItems3;

            Player.ItemStacks[3] = urtaQItems4;

            Player.ItemStacks[4] = urtaQItems5;

            Player.ItemStacks[5] = urtaQItems6;

            Player.ItemStacks[6] = urtaQItems7;

            Player.ItemStacks[7] = urtaQItems8;

            Player.ItemStacks[8] = urtaQItems9;

            Player.ItemStacks[9] = urtaQItems10;

        }

        public void StartUrtaQuest()
        {
            ClearOutput();

            urtaQItems1 = Player.ItemStacks[0];

            urtaQItems2 = Player.ItemStacks[1];

            urtaQItems3 = Player.ItemStacks[2];

            urtaQItems4 = Player.ItemStacks[3];

            urtaQItems5 = Player.ItemStacks[4];

            urtaQItems6 = Player.ItemStacks[5];

            urtaQItems7 = Player.ItemStacks[6];

            urtaQItems8 = Player.ItemStacks[7];

            urtaQItems9 = Player.ItemStacks[8];

            urtaQItems10 = Player.ItemStacks[9];

            Player.ItemStacks[0] = new ItemStack();

            Player.ItemStacks[1] = new ItemStack();

            Player.ItemStacks[2] = new ItemStack();

            Player.ItemStacks[3] = new ItemStack();

            Player.ItemStacks[4] = new ItemStack();

            Player.ItemStacks[5] = new ItemStack();

            Player.ItemStacks[6] = new ItemStack();

            Player.ItemStacks[7] = new ItemStack();

            Player.ItemStacks[8] = new ItemStack();

            Player.ItemStacks[9] = new ItemStack();

            player2 = player;

            playerScope = PlayerScope.SwapTo(new Player());

            Player.ShortName = "Urta";

            Player.Tallness = 71;

            Player.Body.Get<HairPart>().Color = "gray";

            Player.Body.Get<HairPart>().Length = 12;

            Player.Body.Get<SkinPart>().Id = (noFur ? BodyPartTypes.Plain : BodyPartTypes.Fur);

            Player.Body.Get<SkinPart>().Adj = (noFur ? "Common.Text.Smooth".Localized() : "Common.Text.Silky".Localized());

            Player.Body.Get<SkinPart>().DescKey = (noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized());

            Player.CreateBreastRow();

            Player.BreastRows[0].BreastRating = 7;

            Player.NippleLength = .75f;

            Player.Tone = 50;

            Player.Body.Get<SkinPart>().Tone = (noFur ? "Common.Text.Dark".Localized() : "Common.Text.Black".Localized());

            Player.Body.Get<LowerBodyPart>().Id = BodyPartTypes.Fox;

            Player.Body.Get<LowerBodyPart>().LegCount = 2;

            Player.Ears.Id = BodyPartTypes.Fox;

            Player.Body.Get<FacePart>().Id = (noFur ? BodyPartTypes.Human : BodyPartTypes.Fox);

            Player.Body.Get<TailPart>().Id = BodyPartTypes.Fox;

            Player.Body.Get<TailPart>().Venom = 1;

            Player.CreateCock();

            Player.Cocks[0].Id = BodyPartTypes.Horse;

            Player.Cocks[0].Length = 20;

            Player.Cocks[0].Thickness = 3;

            Player.Balls = 2;

            Player.BallSize = 4;

            Player.CumMultiplier = 80;

            Player.Thickness = 30;

            Player.Tone = 50;

            Player.Femininity = 90;

            Player.Body.Hips = 12;

            Player.Body.Butt = 12;

            Player.Anal.Looseness = 2;

            Player.CreateStatusEffect(StatusEffectIds.BonusVCapacity, 58, 0, 0, 0);

            Player.CreateVagina();

            Player.Vaginas[0].Wetness = 4;

            Player.Vaginas[0].Looseness = 1;

            Player.SetClitLength(1);

            Player.Strength = 75;

            Player.Toughness = 80;

            Player.Speed = 90;

            Player.Intelligence = 55;

            Player.Libido = 70;

            Player.Sensitivity = 50;

            Player.Corruption = 30;

            Player.Lust = 55;

            Player.Hunger = 100;

            Player.Gems = 183;

            Player.Level = 15;

            Player.AddMastery(new Identifier("coc", "Tease"), 0);

            Player.Level += Player.AscensionFactor(30);

            Player.Strength *= 1 + Player.AscensionFactor(0.25f);

            Player.Toughness *= 1 + Player.AscensionFactor(0.25f);

            Player.Speed *= 1 + Player.AscensionFactor(0.25f);

            Player.Intelligence *= 1 + Player.AscensionFactor(0.25f);

            Player.Health = Player.MaxHealth;

            Player.Fatigue = 0;

            Player.CreatePerk(PerkIds.Resistance, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Agility, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.HeavyImpact, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Nymphomania, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Precision, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Seduction, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Evade, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Tactician, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.LungingAttacks, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.ImmovableObject, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.BrutalBlows, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Brawler, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Berserker, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.Tank, 0, 0, 0, 0);

            Player.CreatePerk(PerkIds.HistoryFighter, 0, 0, 0, 0);

            Player.SetWeapon((Weapon)ItemRegistry.Create(ItemIds.UrtasHalberd));

            Player.SetArmor((Armor)ItemRegistry.Create(ItemIds.UrtasArmor));

            Player.Loaded = true;

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, .75f);

            OutputText("Scenes.UrtaQuest.StartUrtaQuest.YouAreUrtaFox".Localized((noFur ? "Common.Text.Girl".Localized() : "Common.Text.Morph".Localized())));

            OutputText("Scenes.UrtaQuest.StartUrtaQuest.FoxLikeFace".Localized((noFur ? "Common.Text.Human".Localized() : "Common.Text.Vulpine".Localized()), (noFur ? "and " : ""), (noFur ? "" : ", and paw-footed fox-like legs")));

            if (noFur)
                OutputText("Scenes.UrtaQuest.StartUrtaQuest.PartiallyFurredLimbs".Localized());
            else
                OutputText("Scenes.UrtaQuest.StartUrtaQuest.FullyFurredBody".Localized());

            OutputText("Scenes.UrtaQuest.StartUrtaQuest.VolleyballTits".Localized());

            OutputText("Scenes.UrtaQuest.StartUrtaQuest.MixedSexes".Localized());

            OutputText("Scenes.UrtaQuest.StartUrtaQuest.WorkGarbJerkin".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TowerOfTheCovanant);
        }

        public void TowerOfTheCovanant()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.LeaveTableNervous".Localized(
                player2.ShortName,

                player2.Mf("him", "her"),

                player2.Mf("he", "she"),

                player2.Mf("him", "her"),

                player2.Mf("he", "she"),

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.WetBitchDoorCloses".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.WolfPickpocket".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.TowerLooms".Localized((noFur ? "" : ", white-furred"), (noFur ? "" : " visible through their fur")));

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.WolfGuardWarning".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.RequestsReexamination".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.GuardsAllowEntry".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.ReliefAtNoExplanation".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.CentaurEscort".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovanant.CoughsStirsThoughts".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TowerOfTheCovenantII);
        }

        public void TowerOfTheCovenantII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.DimChamberEntry".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.OldWolfGreets".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.HappyHeRemembers".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.MageNodsChinScratch".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.WorryAboutPrivacy".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.KneelBeforeMe".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.RecoilHairOnEnd".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.TaothLivesWhisper".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.BlinkTearsAway".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.TaleOfTaoth".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.ReincarnatedGodShock".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.EssenceBuriedDeep".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.CriesWhatToDo".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.BeardTugUncertain".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.WringingHandsQuestion".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.ExpressionUnchanged".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.ShrinePilgrimage".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.SighsGoAlone".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.NightWithLover".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.SadSmileFarewell".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.FindsPendant".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.WearySmileExcuse".Localized());

            OutputText("Scenes.UrtaQuest.TowerOfTheCovenantII.KissOnNose".Localized(
                player2.ShortName,

                player2.Mf("him", "her"),

                player2.Mf("him", "her"),

                player2.ShortName
            ));

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFuckbuddy) > 0 && CoC.Instance.Flags.Get(GameFlagIds.PcPromisedHelMonogamyFucks) != 1)
                AddButton(0, "Common.Button.Stop4adrink".Localized(), GetHelBangedAsUrta);

            AddButton(1, "Common.Button.Visit".Localized(), () => VisitPCPreUrtaQuest(false));

            AddButton(2, "Common.Button.VisitTruth".Localized(), () => VisitPCPreUrtaQuest(true));

            AddButton(3, "Common.Button.Embark".Localized(), () => RunIntoAGoblin());
        }

        public void GetHelBangedAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.LiquidFortitude".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.ShoveShocksReverie".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.GirlWhinesApology".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.ShieldsProminentPrick".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.NoPointInAnger".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.GrabsTowelAccepts".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.SoakedFoxGirlLeads".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.RedheadHandshake".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.MaiBlushesScarlet".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.SheKnowsYou".Localized(
                player2.ShortName,

                player2.ShortName
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) > 0)
                OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.SorceressTaleFragment".Localized(
                    player2.ShortName
                ));

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.SitsAndListens".Localized());

            OutputText("Scenes.UrtaQuest.GetHelBangedAsUrta.LewdJokeReminder".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaMetHel, 1);

            ClearButtons();

            AddButton(0, "Common.Button.HeadOut".Localized(), LeaveB4HelFucksUrta);

            AddButton(1, "Common.Text.GetHelpQuoted".Localized(), FuckHelAndKitsunesAsUrta);
        }

        public void LeaveB4HelFucksUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LeaveB4HelFucksUrta.HastyExcuseExit".Localized());

            OutputText("Scenes.UrtaQuest.LeaveB4HelFucksUrta.MaiCallsFarewell".Localized());

            OutputText("Scenes.UrtaQuest.LeaveB4HelFucksUrta.EmbarkOrVisitChoice".Localized(
                player2.ShortName
            ));

            ClearButtons();

            AddButton(1, "Common.Button.Visit".Localized(), () => VisitPCPreUrtaQuest(false));

            AddButton(2, "Common.Button.VisitTruth".Localized(), () => VisitPCPreUrtaQuest(true));

            AddButton(3, "Common.Button.Embark".Localized(), () => RunIntoAGoblin());
        }

        public void FuckHelAndKitsunesAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.RedheadFriendChat".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.UpstairsWallSlam".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.TwinsKneelLustily".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.SalamanderMammaryRelease".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.ThreeGirlsLookUp".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.FoxTwinMoans".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.SecondTwinAdds".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.TremblingNeed".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.SalamanderGrinWickedly".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.EagerNodToCum".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.LaughsStrokingCock".Localized());

            if (player2.HasCock())
            {
                OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.BetterOneBoast".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) > 0)
                    OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.AtHomeSuffix".Localized());
                else
                    OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.BeckAndCallSuffix".Localized());

                OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.CommonGroundRemark".Localized());
            }

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.SalamanderSlidesDown".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.SistersPushScream".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.HelMoansHairStroke".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.HeliaBreastPress".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.OrgasmicAfterglow".Localized());

            OutputText("Scenes.UrtaQuest.FuckHelAndKitsunesAsUrta.WakeAloneDepart".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaFuckedHel, 1);

            Player.Orgasm("Generic");

            View.Time.Days += 1;

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => RunIntoAGoblin());
        }

        public void VisitPCPreUrtaQuest(bool truth)
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SetOutForCamp".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SunsetConcealment".Localized((noFur ? "" : " thanks to your natural fur color")));
            // 原版 UrtaQuest.hx:462 — Jojo 在守夜
            if (Followers.FollowerRegistry.Guards.Any(g => g.Id == FollowerIds.JojoFollower))
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SneakPastMonk".Localized());
            // 原版 UrtaQuest.hx:468 — Holli 在守夜
            if (Followers.FollowerRegistry.Guards.Any(g => g.Id == FollowerIds.HolliFollower))
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.AnemoneNearlySees".Localized(
                    ""
                ));
            // 原版 UrtaQuest.hx:468 — Holli 在守夜
            if (Followers.FollowerRegistry.Guards.Any(g => g.Id == FollowerIds.HolliFollower))
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.DanglingTentacle".Localized());

            if ((CampScene.FollowersCount() + CampScene.LoversCount()) > 2)
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.AvoidCampPeople".Localized(
                    player2.ShortName
                ));

            OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.CampQualityRemark".Localized());

            OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.FitfulSlumber".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("He", "She")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SentHereReasonSuffix".Localized());

            OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.ShoulderShakeAwake".Localized(
                player2.Mf("his", "her"),

                player2.Mf("him", "her"),

                player2.Mf("His", "Her"),

                player2.Mf("he", "she")
            ));

            if (truth)
            {
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SitDownAndExplain".Localized(
                    player2.Mf("him", "her"),

                    player2.Mf("him", "her"),

                    player2.Mf("His", "Her"),

                    player2.Mf("He", "She")
                ));

                if (player2.Corruption < 33)
                    OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.ArguesToCome".Localized());
                else
                    if (player2.Corruption < 66)
                        OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.NodsUnderstandingly".Localized());
                    else
                        OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.DismissiveNod".Localized(
                            player2.ShortName
                        ));

                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.ComfortingEmbrace".Localized(
                    player2.Mf("He", "She")
                ));

                if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.SleepWith)))
                    OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SuggestsMovingOff".Localized(
                        player2.ShortName,

                        CoC.Instance.Flags.Get(GameFlagIds.SleepWith)
                    ));
            }
            else
            {
                OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SitDownAndConfide".Localized(
                    player2.Mf("him", "her"),

                    player2.Mf("him", "her"),

                    player2.Mf("him", "her"),

                    player2.Mf("him", "her"),

                    player2.ShortName
                ));

                if (!string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.SleepWith)))
                    OutputText("Scenes.UrtaQuest.VisitPCPreUrtaQuest.SuggestsMovingOffAgain".Localized(
                        player2.ShortName,

                        CoC.Instance.Flags.Get(GameFlagIds.SleepWith)
                    ));
            }

            var buttonFuckVagText = "Fuck" + player2.Mf("His", "Her") + "Common.Text.Vag".Localized();

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), () => SnuggleWithUrta(truth));

            if (player2.HasVagina())
                AddButton(1, buttonFuckVagText, () => PutUrtasWangInPCsCunt(truth));

            if (player2.HasCockThatFits(UrtaCapacity()))
                AddButton(2, "Common.Button.RideDick".Localized(), () => PutAPCDickInUrtaCoochies(truth));

            if (player2.HasCock())
                AddButton(3, "Common.Button.FuckAss".Localized(), () => UrtaPutsHerDickInPCsWithDicks(truth));

            AddButton(4, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), () => PcBlowsUrtasDong(truth));
        }

        public void UrtaPutsHerDickInPCsWithDicks(bool truth)
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.PushDownToGround".Localized(
                player2.ShortName,

                player2.ShortName,

                player2.Mf("his", "hers"),

                player2.Mf("His", "Her"),

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.GuideLipsToNipple".Localized(
                player2.Mf("his", "her"),

                player2.ShortName,

                player2.Mf("He", "She"),

                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.CockDescript(player2.BiggestCockIndex())
            ));

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TitsReleaseAfterThrust".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.CockDescript(player2.BiggestCockIndex())
            ));

            if (player2.Tallness < 60)
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TitsHangOverSuffix".Localized(
                    player2.FaceDescript()
                ));
            else
                if (player2.Tallness < 76)
                    OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TitsSwingBetweenSuffix".Localized());
                else
                    if (player2.Tallness < 84)
                        OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TitsSmashIntoSuffix".Localized());
                    else
                        OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TitsSwayOverSuffix".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.EnjoyWhileShifting".Localized());

            if (player2.HasVagina())
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.PastPussySuffix".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TowardsPrizeAsshole".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.GlanceUpForAssent".Localized(
                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.Mf("He", "She")
            ));

            if (player2.AnalCapacity() > 70)
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.StretchedFromUse".Localized(
                    player2.Mf("he", "she")
                ));
            else
                if (player2.AnalCapacity() > 50)
                    OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.PlentyOfGive".Localized());
                else
                    OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.LittleGiveSmear".Localized(
                        player2.Mf("his", "her"),

                        player2.Mf("his", "her"),

                        player2.Mf("his", "her")
                    ));
            // 原版 UrtaQuest.hx:553 — 菊花还是处
            if (player2 != null && player2.Body.Get<AnalPart>().Looseness == 0)
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.AnalVirginityTaken".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.WrithesHipsLift".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            if (player2.CockTotal() > 1)
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.SLetter".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.ThickViscousClump".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.ScruffKissPull".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("His", "Her"),

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.BallsTingleWarm".Localized(
                player2.Mf("his", "her"),

                player2.ShortName,

                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.TurgidMassRam".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            if (player2.CockTotal() > 1)
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.SLetterAgain".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.LiquidLoveExplode".Localized(
                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            if (player2.HasVagina())
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.UnnoticedFemaleFluids".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.KissWiggleAway".Localized(
                player2.ShortName
            ));

            if (player2.CumQ() >= 500)
            {
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.WorseBySpunkVolume".Localized());

                if (player2.CockTotal() > 1)
                    OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.SExudeToken".Localized());
                else
                    OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.ExudesToken".Localized());
            }

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.BellyRoundsOut".Localized(
                player2.Mf("his", "her"),

                player2.Mf("him", "her"),

                player2.Mf("his", "her"),

                player2.ShortName,

                player2.Mf("His", "Her")
            ));

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.PostOrgasmPounding".Localized(
                player2.ShortName,

                player2.Mf("He", "She"),

                player2.ShortName,

                player2.Mf("He", "She")
            ));

            if (player2.CumQ() >= 500)
            {
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.MixingWithOwnSuffix".Localized());

                if (player2.CumQ() < 1000)
                    OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.PuddlyWord".Localized());
                else
                    if (player2.CumQ() < 2500)
                        OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.SmallLakeSized".Localized());
                    else
                        OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.MassiveLakeSized".Localized());

                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.MessVolumeRealized".Localized());
            }
            else
                OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.ButTheSuffix".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.SemenFabricDries".Localized());

            OutputText("Scenes.UrtaQuest.UrtaPutsHerDickInPCsWithDicks.AnotherKissPull".Localized(
                player2.ShortName
            ));

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MorningAfterCampVisitEmbark(truth));
        }

        public void PcBlowsUrtasDong(bool truth)
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.OgleNakedLover".Localized(
                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.Mf("him", "her")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.SurprisedButSmiles".Localized(player2.ShortName, player2.Mf("his", "her"), player2.Mf("his", "her"), player2.Mf("him", "her"), player2.ShortName, (noFur ? "" : "furry "), player2.ShortName));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.NotWhatIntended".Localized());

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.MischievousRollOver".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.GraspsRockHardShaft".Localized(
                player2.Mf("He", "She"),

                player2.ShortName,

                player2.ShortName,

                player2.ShortName,

                player2.ShortName,

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.PreDollopsOverflow".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.LicksPoleClean".Localized(
                player2.Mf("He", "She")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.BobsDeepThroat".Localized(player2.ShortName, player2.Mf("his", "her"), player2.Mf("his", "her"), player2.Mf("his", "her"), (noFur ? "" : "furry "), player2.Mf("him", "her"), player2.ShortName));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.ProbingBehindBalls".Localized(
                player2.ShortName,

                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.FlirtatiousChallenge".Localized(
                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.CleanHandPop".Localized(
                player2.Mf("his", "her"),

                player2.ShortName,

                player2.Mf("He", "She"),

                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.SlickFingersOffered".Localized(
                player2.ShortName,

                player2.ShortName,

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.ShoveFingerProstate".Localized(
                player2.Mf("His", "Her")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.LegsFlopRelease".Localized(
                player2.ShortName,

                player2.Mf("He", "She"),

                player2.Mf("He", "She")
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.PrimAnswerBetrayal".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("him", "her"),

                player2.Mf("him", "her"),

                player2.Mf("him", "her"),

                player2.Mf("boys", "girl"),

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PcBlowsUrtasDong.YawnOnBelly".Localized(
                player2.ShortName,

                player2.Mf("him", "her"),

                player2.ShortName
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MorningAfterCampVisitEmbark(truth));
        }

        public void PutAPCDickInUrtaCoochies(bool truth)
        {
            ClearOutput();

            var x = player2.CockThatFits(UrtaCapacity());

            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.PushDownOntoGround".Localized(
                player2.ShortName,

                player2.ShortName,

                player2.Mf("his", "hers"),

                player2.Mf("His", "Her")
            ));

            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.CockThrobsInTune".Localized());

            if (player2.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.CloseEnough".Localized());

            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.FuckYouLikeWoman".Localized(
                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.Mf("His", "Her"),

                player2.CockDescript(x)
            ));

            if (player2.HasVagina())
                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.CuntDripsOntoGround".Localized());

            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.FluidRollsDownLength".Localized());

            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.BreakKissReluctant".Localized());

            if (player2.IsTaur() || player2.IsDrider())
            {
                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.KneelOnAllFours".Localized());

                if (!player2.IsDrider())
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.Quadrapedal".Localized());
                else
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.Octapedal".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.PartnerMountsForelegs".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.MoanLikeSlutFilled".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.LoosenBareMinimum".Localized(
                    player2.Mf("his", "her")
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.MindDriftsPleasant".Localized(
                    player2.ShortName,

                    player2.Mf("his", "her"),

                    player2.Mf("he", "she")
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.CallOutInRapture".Localized(
                    player2.ShortName
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.BallsTingleWarmth".Localized(
                    player2.ShortName
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.CockHeadFlaresWider".Localized(
                    player2.ShortName
                ));

                if (player2.HasVagina())
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.FemaleJizzSpatters".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.SentencePeriod".Localized());

                if (player2.CumQ() < 500)
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.JizzIntoMilkingCunt".Localized(
                        player2.Mf("He", "She")
                    ));
                else
                    if (player2.CumQ() < 1000)
                        OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.MoanWhimperWithGlee".Localized());
                    else
                        if (player2.CumQ() < 2500)
                            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.HowlOfJoy".Localized(
                                player2.ShortName
                            ));
                        else
                            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.HighPitchedScream".Localized(player2.Mf("boy", "girl"), player2.Mf("his", "her"), (noFur ? "" : ", the fur pulled into a glossy tautness"), player2.Mf("his", "her"), player2.ShortName));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.LoverSupportsAtopYou".Localized(
                    player2.Mf("him", "her")
                ));

                if (player2.CumQ() < 1000)
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.FallingFaceFirst".Localized());
                else
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.NotBotheringToMove".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.SmileDopilyUpAt".Localized(
                    player2.Mf("him", "her"),

                    player2.Mf("he", "she")
                ));
            }
            else
            {
                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.TurnAroundShowAss".Localized(
                    player2.Mf("his", "her"),

                    player2.Mf("his", "her"),

                    player2.Mf("his", "her"),

                    player2.Mf("his", "her")
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.SmileOverShoulder".Localized(player2.Mf("his", "her"), player2.ShortName, player2.ShortName, player2.Mf("his", "her"), player2.Mf("He", "She"), (noFur ? "" : ", furry")));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.MindDriftsPleasantAgain".Localized(
                    player2.ShortName,

                    player2.Mf("his", "her"),

                    player2.Mf("he", "she")
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.RaptureCallAgain".Localized(
                    player2.ShortName
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.BallsTingleAgain".Localized(
                    player2.ShortName,

                    player2.ShortName
                ));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.CockHeadJustFlares".Localized(
                    player2.ShortName
                ));

                if (player2.HasVagina())
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.FemaleJizzSpattersBehind".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.SentencePeriodAgain".Localized());

                if (player2.CumQ() < 500)
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.JizzIntoMilkingAgain".Localized(
                        player2.Mf("He", "She")
                    ));
                else
                    if (player2.CumQ() < 1000)
                        OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.MoanWhimperGleeAgain".Localized());
                    else
                        if (player2.CumQ() < 2500)
                            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.HowlOfJoyAgain".Localized(
                                player2.ShortName
                            ));
                        else
                            OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.HighPitchedFountain".Localized(player2.Mf("boyfriend", "girlfriend"), player2.Mf("his", "her"), (noFur ? "" : ", the fur pulled into a glossy tautness"), player2.Mf("his", "her"), player2.ShortName));

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.SighRufflesHair".Localized(
                    player2.ShortName
                ));

                if (player2.CumQ() >= 1000)
                    OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.OverstuffedBellyWeight".Localized());

                OutputText("Scenes.UrtaQuest.PutAPCDickInUrtaCoochies.NoInclinationToMove".Localized(
                    player2.ShortName
                ));
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MorningAfterCampVisitEmbark(truth));
        }

        public void PutUrtasWangInPCsCunt(bool truth)
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PushDownOnBack".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.ArmorName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            if ((Player.Vaginas.Count > 0))
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PinkWord".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PulsingInteriorExcites".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.TentingGarmentGiggle".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.TwistGrabStraps".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("he", "she")
            ));

            if (player2.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PregnantBellyCurve".Localized());
            else
                if (player2.Tone >= 65)
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.TonedAbdomen".Localized());
                else
                    if (player2.Tone < 30)
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.SoftForgivingMiddle".Localized());
                    else
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.Belly".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.AttentionsDriveAttempts".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.FingerAcrossNose".Localized(
                player2.ShortName,

                player2.Mf("he", "she"),

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.SilenceByKissing".Localized(
                player2.Mf("he", "she"),

                player2.Mf("him", "her"),

                player2.Mf("him", "her"),

                player2.Mf("his", "her"),

                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("He", "She")
            ));

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.FeralLustySnarl".Localized());

            if (player2.IsTaur())
            {
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.ClimbOffTaurSmirks".Localized(
                    player2.Mf("man", "woman"),

                    player2.Mf("He", "She"),

                    player2.Mf("him", "her"),

                    player2.Mf("him", "her"),

                    player2.Mf("he", "her"),

                    player2.ShortName
                ));

                if (player2.Body.Get<TailPart>().Id.Path != "none")
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.StrokeTailAside".Localized());
                else
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.StrokeSwollenSex".Localized());

                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PerfectViewOfCunt".Localized());

                if (player2.Wetness() >= 4)
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.WetterThanEdryn".Localized());
                else
                    if (player2.Wetness() >= 2)
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.WetAsEdryn".Localized());
                    else
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.AlmostWetAsEdryn".Localized());

                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.LickAndSniff".Localized());

                if (player2.Tallness >= 84)
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PullToRock".Localized());
                else
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.RiseToFullHeight".Localized());
            }
            else
                if (player2.IsGoo())
                {
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.RollGooeyWoman".Localized(
                        player2.Mf("he", "she"),

                        player2.Mf("He", "She"),

                        player2.Mf("his", "her"),

                        player2.Mf("his", "her"),

                        player2.Mf("his", "her")
                    ));

                    if (player2.Vaginas[0].ClitLength < .5f)
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.TinyWord".Localized());
                    else
                        if (player2.Vaginas[0].ClitLength >= 2.5f)
                            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.MassiveWord".Localized());

                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.NuzzleSlickSlimeCunt".Localized(
                        player2.Mf("His", "Her")
                    ));
                }
                else
                    if (player2.IsTaur())
                    {
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.RollOverProstrate".Localized(
                            player2.Mf("he", "she"),

                            player2.Mf("his", "her"),

                            player2.Mf("His", "Her"),

                            player2.Mf("he", "she"),

                            player2.Mf("his", "her"),

                            player2.Mf("his", "her"),

                            player2.Mf("he", "she"),

                            player2.Mf("his", "her"),

                            player2.Mf("his", "her")
                        ));

                        if (player2.Wetness() >= 3)
                            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.MoistureDrizzlesFromSlit".Localized(
                                player2.Mf("His", "Her")
                            ));

                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.TailCoilsAround".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.RollOnHandsKnees".Localized(
                            player2.Mf("his", "her"),

                            player2.Mf("his", "her"),

                            player2.Mf("his", "her"),

                            player2.Mf("his", "her"),

                            player2.Mf("his", "her"),

                            player2.ShortName
                        ));

                        if (player2.Wetness() < 3)
                            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.EveryDropOfNectar".Localized());
                        else
                            if (player2.Wetness() < 4)
                                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.CopiousDropsOfNectar".Localized());
                            else
                                if (player2.Wetness() < 5)
                                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.DrizzlingMoistureOozes".Localized());
                                else
                                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.RiversOfGirlLube".Localized());

                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.SniffLungfulsOfScent".Localized());
                    }

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.GrabWaistThrustForward".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.Mf("His", "Her"),

                player2.Mf("him", "her"),

                player2.Mf("He", "She"),

                player2.Mf("he", "she"),

                player2.ShortName,

                player2.Mf("His", "Her"),

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.SmackAssWiggle".Localized(
                player2.ShortName,

                player2.Mf("His", "Her")
            ));

            if (player2.Body.Get<VaginaPart>().Capacity() >= 200)
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.HandleCocksWithEase".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.WonderfulSensationThrust".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.LookOverShoulderWhisper".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.ObligeLoverRequest".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her"),

                player2.Mf("He", "She"),

                player2.Mf("He", "She"),

                player2.Mf("his", "her")
            ));

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.FireStickyLoads".Localized(
                player2.ShortName,

                player2.ShortName,

                player2.Mf("He", "She"),

                player2.Mf("He", "She"),

                player2.Mf("His", "Her")
            ));

            if (player2.Pregnancy.Incubation > 0 && player2.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.MoreThanAlready".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.ProdigiousDepositIllusion".Localized(
                player2.Mf("him", "her"),

                player2.ShortName
            ));

            if (player2.Pregnancy.Incubation > 0 && player2.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.PregnantWord".Localized());

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.LastSpunkInDome".Localized());

            if (player2.HasCock())
            {
                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.LoverAchievesOrgasm".Localized());

                if (player2.CumQ() < 50)
                    OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.SeedRopesFromPrick".Localized(
                        player2.Mf("his", "her")
                    ));
                else
                    if (player2.CumQ() < 250)
                        OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.ThickSeedRopes".Localized(
                            player2.Mf("He", "She")
                        ));
                    else
                        if (player2.CumQ() < 1000)
                            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.ThickSeedRopesProlonged".Localized(
                                player2.Mf("he", "she")
                            ));
                        else
                            if (player2.CumQ() < 2500)
                                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.CumTorrentsFromPrick".Localized(
                                    player2.Mf("he", "she")
                                ));
                            else
                                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.CumTorrentsFewMinutes".Localized(
                                    player2.Mf("he", "she")
                                ));
            }

            OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.LayExhaustedSnuggle".Localized(
                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            if (player2.Vaginas[0].Virgin)
            {
                player2.Vaginas[0].Virgin = false;

                OutputText("Scenes.UrtaQuest.PutUrtasWangInPCsCunt.TookHisVirginity".Localized(
                    player2.ShortName
                ));
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MorningAfterCampVisitEmbark(truth));
        }

        public void SnuggleWithUrta(bool truth)
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.SnuggleWithUrta.LoveOverLustDecision".Localized(
                player2.ShortName,

                player2.Mf("his", "her"),

                player2.Mf("his", "her")
            ));

            if (player2.IsTaur() || player2.IsDrider())
            {
                OutputText("Scenes.UrtaQuest.SnuggleWithUrta.AwkwardSnuggleSetup".Localized());

                if (player2.IsTaur())
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.Centaur".Localized());
                else
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.Drider".Localized());

                OutputText("Scenes.UrtaQuest.SnuggleWithUrta.BodyPressForward".Localized());

                if (player2.IsTaur())
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.Forehoofs".Localized());
                else
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.SpideryLimbPairs".Localized());

                OutputText("Scenes.UrtaQuest.SnuggleWithUrta.SentencePeriod".Localized());
            }
            else
                if (player2.IsTaur())
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.CoilsAroundWaist".Localized(
                        player2.Mf("His", "Her")
                    ));
                else
                    if (Player.IsGoo())
                        OutputText("Scenes.UrtaQuest.SnuggleWithUrta.GooBodyAccepts".Localized(
                            player2.Mf("His", "Her")
                        ));
                    else
                        OutputText("Scenes.UrtaQuest.SnuggleWithUrta.KneesToKnees".Localized());

            OutputText("Scenes.UrtaQuest.SnuggleWithUrta.TuckFaceIntoNeck".Localized((noFur ? "" : " vulpine"), player2.Mf("his", "her"), player2.Mf("his", "her"), player2.ShortName));

            OutputText("Scenes.UrtaQuest.SnuggleWithUrta.LoverSmiles".Localized());

            if (player2.Corruption < 33)
                OutputText("Scenes.UrtaQuest.SnuggleWithUrta.PlayfulPat".Localized());
            else
                if (player2.Corruption < 66)
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.GentleStroke".Localized());
                else
                    OutputText("Scenes.UrtaQuest.SnuggleWithUrta.LasciviousCaress".Localized());

            OutputText("Scenes.UrtaQuest.SnuggleWithUrta.PullIntoEmbrace".Localized());

            OutputText("Scenes.UrtaQuest.SnuggleWithUrta.SurprisedDrowsiness".Localized(
                player2.ShortName,

                player2.ShortName
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 14f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => MorningAfterCampVisitEmbark(truth));
        }

        public void MorningAfterCampVisitEmbark(bool truth)
        {
            ClearOutput();

            View.Time.Days += 1;

            OutputText("Scenes.UrtaQuest.MorningAfterCampVisitEmbark.MorningDeparture".Localized(
                player2.ShortName,

                player2.Mf("him", "her"),

                player2.Mf("him", "her")
            ));

            if (truth)
                OutputText("Scenes.UrtaQuest.MorningAfterCampVisitEmbark.AsksToComeAlong".Localized(
                    player2.Mf("He", "She")
                ));

            OutputText("Scenes.UrtaQuest.MorningAfterCampVisitEmbark.EmbraceAndKiss".Localized(
                player2.ShortName
            ));

            if (truth)
                OutputText("Scenes.UrtaQuest.MorningAfterCampVisitEmbark.PromiseToReturn".Localized());
            else
                OutputText("Scenes.UrtaQuest.MorningAfterCampVisitEmbark.PeriodSpace".Localized());

            OutputText("Scenes.UrtaQuest.MorningAfterCampVisitEmbark.FarewellGlances".Localized(
                player2.ShortName
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => RunIntoAGoblin(true));
        }

        public void RunIntoAGoblin(bool camped = false)
        {
            ClearOutput();

            if (camped)
                OutputText("Scenes.UrtaQuest.RunIntoAGoblin.PendantRandomSearch".Localized());
            else
            {
                OutputText("Scenes.UrtaQuest.RunIntoAGoblin.TaothTempleJourney".Localized(
                    player2.ShortName
                ));

                OutputText("Scenes.UrtaQuest.RunIntoAGoblin.PendantCitySearch".Localized());
            }

            OutputText("Scenes.UrtaQuest.RunIntoAGoblin.ReedyGoblinVoice".Localized());

            OutputText("Scenes.UrtaQuest.RunIntoAGoblin.FightDeclared".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "goblin_broodmother")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void WinFuckAGoblinBroodmotherAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.GreedyEyeing".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.Submissive".Localized());
            else
                OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.FeverishMasturbation".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.LickLipsAnticipation".Localized((noFur ? "" : " black"), player2.ShortName));

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.SaffronSlutSmirks".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.SmirkBack".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.MilfBegsRide".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.HorseCockProbe".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.ScentFixatedGaze".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.GoblinTeasesDick".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.MoanAtPressure".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.GoblinSucksShaft".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.SquealBuckingHips".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.PullsAwayGasping".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.ChuckleAndRise".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.GoblinStaresDumbly".Localized());

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.TauntAboutBlanks".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.WinFuckAGoblinBroodmotherAsUrta.TrinketAfterFight".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NagaPleaseNagaStoleMyDick);
        }

        public void UrtaLosesToGoblin()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.AchingBodyBuckles".Localized());

                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.PregnantSeductressApproach".Localized());

                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.OneTrackLust".Localized((noFur ? "" : " dark")));

                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.BustyWenchGrins".Localized());
            }
            else
            {
                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.BeatenCollapse".Localized());

                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.YellowWomanVials".Localized());

                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.RebelliousRetortFails".Localized());

                OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.BustyWenchGrinsAgain".Localized());
            }

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.VictoriousSlutPrize".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.UnableToFight".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.GaspingUnderSkills".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.DrugHazedWifeDream".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.GigglingSubmission".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.WetSmackFootjob".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.LongDrawnMoan".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.ExperiencedRiding".Localized((noFur ? "" : " furry")));

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.BreedingCompulsion".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.ShoulderGripThrust".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.LostInRampantSex".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.GrindingSweetRump".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToGoblin.MatronNotDone".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoseToGoblinsPartIIAsUrta);
        }

        public void LoseToGoblinsPartIIAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIAsUrta.ManyOrgasmsLater".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIAsUrta.PoutingWifePaces".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIAsUrta.ShamefulNod".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIAsUrta.GoldenrodGrumbles".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIAsUrta.BlushingAfterRelease".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoseToGoblinsPartIIIAsUrta);
        }

        public void LoseToGoblinsPartIIIAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIIAsUrta.SeveralDaysLater".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIIAsUrta.HardJourneyFoes".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIIAsUrta.ManyFightsDone".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIIIAsUrta.GodGoneAlone".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoseToGoblinsPartIVAsUrta);
        }

        public void LoseToGoblinsPartIVAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIVAsUrta.SeveralHoursLater".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIVAsUrta.SweatyCumPuddle".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGoblinsPartIVAsUrta.WipingGlobSnuggles".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaGoblinSlave, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGameOver);
        }

        public void UrtaGameOver()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaGameOver.UrtaLostToFate".Localized());

            CombatController.InCombat = false;

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, -1);

            View.Time.Days += 1;

            ResetToPC();

            View.Time.Hours = 6;

            DoNext(() => ReturnToCamp(1));
        }

        public void UrtaSpecials()
        {
            ClearOutput();

            if (CombatController.InCombat && Player.HasStatusEffect(StatusEffectIds.Sealed) && Player.StatusEffectValue(StatusEffectIds.Sealed, 1) == 5)
            {
                OutputText("Scenes.UrtaQuest.UrtaSpecials.SpecialFumble".Localized());

                CombatController.Instance!.EnemyTurn();

                return;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Combo".Localized(), UrtaComboAttack);

            AddButton(1, "Common.Button.Vault".Localized(), UrtaVaultAttack);

            AddButton(2, "Common.Button.Sidewinder".Localized(), UrtaSidewinder);

            AddButton(3, "Combat.Ability.LivingStatueDirtKick.Name".Localized(), UrtaDirtKick);

            AddButton(4, "Common.Button.Metabolize".Localized(), UrtaMetabolize);

            AddButton(5, "Common.Button.Secondwind".Localized(), UrtaSecondWind);

            AddButton(14, "Common.Button.Back".Localized(), () => CombatController.Instance!.ShowMenu());
        }

        public void UrtaMetabolize()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaMetabolize.FatigueForHealth".Localized());

            Player.HealthChange(-(float)(Player.MaxHealth / 10));

            OutputText("Scenes.UrtaQuest.UrtaMetabolize.FatigueRestored".Localized());

            Player.ChangeFatigue(-20);

            CombatController.Instance!.EnemyTurn();
        }

        public void UrtaSecondWind()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.UrtaSecondWinded))
            {
                OutputText("Scenes.UrtaQuest.UrtaSecondWind.AlreadyAtLimit".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => CombatController.Instance!.ShowMenu());

                return;
            }

            CurrentMonster.CreateStatusEffect(StatusEffectIds.UrtaSecondWinded, 0, 0, 0, 0);

            Player.HealthChange((float)(Player.MaxHealth / 2));

            Player.ChangeFatigue(-50);

            Player.ApplyStats(new DynStat(GameStats.Lust, -50f));

            OutputText("Scenes.UrtaQuest.UrtaSecondWind.WillpowerSurge".Localized());

            CombatController.Instance!.EnemyTurn();
        }

        public void UrtaComboAttack()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.Attacks))
            {
                if (Player.Fatigue + 25 > Player.MaxFatigue())
                {
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.TooFatigued".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => CombatController.Instance!.ShowMenu());

                    return;
                }

                Player.ChangeFatigue(25);

                Player.CreateStatusEffect(StatusEffectIds.Attacks, 3, 0, 0, 0);
            }
            else
            {
                Player.ChangeStatusValue(StatusEffectIds.Attacks, 0, -1);

                if (Player.StatusEffectValue(StatusEffectIds.Attacks, 0) <= 1)
                    Player.RemoveStatusEffect(StatusEffectIds.Attacks);
            }

            if (Player.HasStatusEffect(StatusEffectIds.Blind))
                OutputText("Scenes.UrtaQuest.UrtaComboAttack.BlindedAttackDoubt".Localized());

            float damage = 0;

            if (!CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Blind) && (Random.Shared.Next(3) == 0 || (Player.HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(2) == 0) || (CombatController.Instance!.Context.Enemy.Speed - Player.Speed > 0 && (int)(Random.Shared.NextDouble() * (((CombatController.Instance!.Context.Enemy.Speed - Player.Speed) / 4) + 80)) > 80)))
            {
                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed < 8)
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.NarrowAvoidance".Localized());

                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed >= 8 && CombatController.Instance!.Context.Enemy.Speed - Player.Speed < 20)
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.QuickDodge".Localized());

                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed >= 20)
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.DeftAvoidance".Localized());

                OutputText("Scenes.UrtaQuest.UrtaComboAttack.NewlineToken".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.Attacks))
                {
                    UrtaComboAttack();

                    return;
                }
                else
                {
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.NewlineTokenAgain".Localized());

                    CombatController.Instance!.EnemyTurn();

                    return;
                }
            }

            damage = Player.Strength;

            damage += Player.WeaponAttack;

            var crit = false;

            if (Random.Shared.Next(100) <= 4 || (Player.HasPerk(PerkIds.Tactician) && Player.Intelligence >= 50 && (Player.Intelligence - 50) / 5 > Random.Shared.Next(100)))
            {
                crit = true;

                damage *= 2;
            }

            float reduction = Random.Shared.Next((int)(CombatController.Instance!.Context.Enemy.Toughness));

            if (Player.WeaponName != "jeweled rapier" && Player.WeaponName != "deadly spear")
            {
                reduction += CombatController.Instance!.Context.Enemy.ArmorDefense;

                if (Player.HasPerk(PerkIds.LungingAttacks))
                    reduction -= CombatController.Instance!.Context.Enemy.ArmorDefense / 2;
            }

            if (Player.WeaponName == "katana")
                if (CombatController.Instance!.Context.Enemy.ArmorDefense >= 5)
                    reduction -= 5;
                else
                    reduction -= CombatController.Instance!.Context.Enemy.ArmorDefense;

            damage -= reduction;

            if (Player.HasPerk(PerkIds.ThunderousStrikes) && Player.Strength >= 80)
                damage *= 1.2f;

            if (damage > 0)
            {
                if (Player.HasPerk(PerkIds.HistoryFighter))
                    damage *= 1.1f;

                CombatCalculator.DamageEnemy(CombatController.Instance!.Context, damage, CombatDamageKind.Melee);
            }
            else
            {
                damage = 0;

                OutputText("Scenes.UrtaQuest.UrtaComboAttack.AttacksDeflected".Localized());
            }

            if (!(damage <= 0))
            {
                OutputText("Scenes.UrtaQuest.UrtaComboAttack.HitForDamage".Localized(
                    damage
                ));

                if (crit)
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.CritBanner".Localized());
            }

            if (Player.HasPerk(PerkIds.BrutalBlows) && Player.Strength > 75)
            {
                if (CombatController.Instance!.Context.Enemy.ArmorDefense > 0)
                    OutputText("Scenes.UrtaQuest.UrtaComboAttack.BrutalDefenseDamage".Localized());

                if (CombatController.Instance!.Context.Enemy.ArmorDefense - 10 > 0)
                    CombatController.Instance!.Context.Enemy.ArmorDefense -= 10;
                else
                    CombatController.Instance!.Context.Enemy.ArmorDefense = 0;
            }

            OutputText("Scenes.UrtaQuest.UrtaComboAttack.NewlineTokenThird".Localized());

            if (CombatController.Instance!.Context.Enemy.Health >= 1 && CombatController.Instance!.Context.Enemy.Lust <= 99)
            {
                if (Player.HasStatusEffect(StatusEffectIds.Attacks))
                {
                    UrtaComboAttack();

                    return;
                }

                OutputText("Scenes.UrtaQuest.UrtaComboAttack.NewlineTokenFourth".Localized());

                CombatController.Instance!.EnemyTurn();
            }
            else
                if (CombatController.Instance!.Context.Enemy.Health <= 0)
                {
                    CombatController.Instance!.State.Outcome = CombatOutcome.HpVictory;

                    CombatController.Instance!.Finish();
                }
                else
                {
                    CombatController.Instance!.State.Outcome = CombatOutcome.LustVictory;

                    CombatController.Instance!.Finish();
                }
        }

        public void UrtaDirtKick()
        {
            ClearOutput();

            if (Player.Fatigue + 5 > Player.MaxFatigue())
            {
                OutputText("Scenes.UrtaQuest.UrtaDirtKick.TooFatiguedDirtKick".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => CombatController.Instance!.ShowMenu());

                return;
            }

            Player.ChangeFatigue(5);

            if (Player.HasStatusEffect(StatusEffectIds.Blind))
                OutputText("Scenes.UrtaQuest.UrtaDirtKick.BlindedDirtKickDoubt".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaDirtKick.SpinningDebrisKick".Localized((noFur ? "" : "paw")));

            if (Random.Shared.Next(20) + 1 + CombatController.Instance!.Context.Enemy.Speed / 20 > 15 + Player.Speed / 20)
            {
                OutputText("Scenes.UrtaQuest.UrtaDirtKick.ShieldedEyesDamn".Localized(
                    CombatController.Instance!.Context.Enemy.Mf("He", "She"),

                    CombatController.Instance!.Context.Enemy.Mf("his", "her")
                ));

                CombatController.Instance!.EnemyTurn();

                return;
            }
            else
                if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Blind))
                    OutputText("Scenes.UrtaQuest.UrtaDirtKick.AlreadyBlindedWaste".Localized(
                        CombatController.Instance!.Context.Enemy.Mf("He", "She")
                    ));
                else
                {
                    OutputText("Scenes.UrtaQuest.UrtaDirtKick.BlindedStatus".Localized(
                        CombatController.Instance!.Context.Enemy.Mf("He", "She")
                    ));

                    CurrentMonster.CreateStatusEffect(StatusEffectIds.Blind, 2 + Random.Shared.Next(3), 0, 0, 0);
                }

            CombatController.Instance!.EnemyTurn();
        }

        public void UrtaSidewinder()
        {
            ClearOutput();

            if (Player.Fatigue + 10 > Player.MaxFatigue())
            {
                OutputText("Scenes.UrtaQuest.UrtaSidewinder.TooFatiguedForSidewinder".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => CombatController.Instance!.ShowMenu());

                return;
            }

            Player.ChangeFatigue(10);

            if (Player.HasStatusEffect(StatusEffectIds.Blind))
                OutputText("Scenes.UrtaQuest.UrtaSidewinder.BlindViciousSideBlow".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaSidewinder.WideSwingToStun".Localized());

            float damage = 0;

            if ((Player.HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(2) == 0) || (CombatController.Instance!.Context.Enemy.Speed - Player.Speed > 0 && (int)(Random.Shared.NextDouble() * (((CombatController.Instance!.Context.Enemy.Speed - Player.Speed) / 4) + 80)) > 80))
            {
                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed < 8)
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideNarrowlyAvoided".Localized());

                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed >= 8 && CombatController.Instance!.Context.Enemy.Speed - Player.Speed < 20)
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideDodgedSwiftly".Localized());

                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed >= 20)
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideDeftlyAvoided".Localized());

                OutputText("Scenes.UrtaQuest.UrtaSidewinder.ParagraphBreak".Localized());

                CombatController.Instance!.EnemyTurn();

                return;
            }

            damage = Player.Strength;

            damage += Player.WeaponAttack;

            damage *= .7f;

            var crit = false;

            if (Random.Shared.Next(100) <= 4 || (Player.HasPerk(PerkIds.Tactician) && Player.Intelligence >= 50 && (Player.Intelligence - 50) / 5 > Random.Shared.Next(100)))
            {
                crit = true;

                damage *= 1.75f;
            }

            float reduction = Random.Shared.Next((int)(CombatController.Instance!.Context.Enemy.Toughness));

            if (Player.WeaponName != "jeweled rapier" && Player.WeaponName != "deadly spear")
            {
                reduction += CombatController.Instance!.Context.Enemy.ArmorDefense;

                if (Player.HasPerk(PerkIds.LungingAttacks))
                    reduction -= CombatController.Instance!.Context.Enemy.ArmorDefense / 2;
            }

            if (Player.WeaponName == "katana")
                if (CombatController.Instance!.Context.Enemy.ArmorDefense >= 5)
                    reduction -= 5;
                else
                    reduction -= CombatController.Instance!.Context.Enemy.ArmorDefense;

            damage -= reduction;

            if (Player.HasPerk(PerkIds.ThunderousStrikes) && Player.Strength >= 80)
                damage *= 1.2f;

            if (damage > 0)
            {
                if (Player.HasPerk(PerkIds.HistoryFighter))
                    damage *= 1.1f;

                CombatCalculator.DamageEnemy(CombatController.Instance!.Context, damage, CombatDamageKind.Melee);
            }
            else
            {
                damage = 0;

                OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideAttacksDeflected".Localized());
            }

            if (!(damage <= 0))
            {
                OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideHitWithDamage".Localized(
                    damage
                ));

                if (crit)
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideCritHit".Localized());
            }

            if (Player.HasPerk(PerkIds.BrutalBlows) && Player.Strength > 75)
            {
                if (CombatController.Instance!.Context.Enemy.ArmorDefense > 0)
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideBrutalHitsBreak".Localized());

                if (CombatController.Instance!.Context.Enemy.ArmorDefense - 10 > 0)
                    CombatController.Instance!.Context.Enemy.ArmorDefense -= 10;
                else
                    CombatController.Instance!.Context.Enemy.ArmorDefense = 0;
            }

            if (!CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Stunned) && !CombatController.Instance!.Context.Enemy.HasPerk(PerkIds.StunImmune) && damage > 0)
            {
                if (CombatController.Instance!.Context.Enemy.Toughness / 10 + Random.Shared.Next(20) + 1 < 20)
                {
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideFoeStunned".Localized());

                    CurrentMonster.CreateStatusEffect(StatusEffectIds.Stunned, 1, 0, 0, 0);
                }
            }
            else
                if (CombatController.Instance!.Context.Enemy.HasPerk(PerkIds.StunImmune))
                    OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideStunResisted".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideLineBreak".Localized());

            if (CombatController.Instance!.Context.Enemy.Health >= 1 && CombatController.Instance!.Context.Enemy.Lust <= 99)
            {
                if (Player.HasStatusEffect(StatusEffectIds.FirstAttack))
                {
                    CombatController.Instance!.Attack();

                    return;
                }

                OutputText("Scenes.UrtaQuest.UrtaSidewinder.SideLineBreakAgain".Localized());

                CombatController.Instance!.EnemyTurn();
            }
            else
                if (CombatController.Instance!.Context.Enemy.Health <= 0)
                {
                    CombatController.Instance!.State.Outcome = CombatOutcome.HpVictory;

                    CombatController.Instance!.Finish();
                }
                else
                {
                    CombatController.Instance!.State.Outcome = CombatOutcome.LustVictory;

                    CombatController.Instance!.Finish();
                }
        }

        public void UrtaVaultAttack()
        {
            ClearOutput();

            if (Player.Fatigue + 20 > Player.MaxFatigue())
            {
                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.TooFatiguedForVault".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => CombatController.Instance!.ShowMenu());

                return;
            }

            Player.ChangeFatigue(20);

            if (Player.HasStatusEffect(StatusEffectIds.Sealed) && Player.StatusEffectValue(StatusEffectIds.Sealed, 1) == 0)
            {
                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.BodyWrenchesAway".Localized());

                CombatController.Instance!.EnemyTurn();

                return;
            }

            if (Player.HasStatusEffect(StatusEffectIds.Blind))
                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.BlindedHighVault".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.LeapAndSlam".Localized());

            float damage = 0;

            if ((Player.HasStatusEffect(StatusEffectIds.Blind) && Random.Shared.Next(2) == 0) || (CombatController.Instance!.Context.Enemy.Speed - Player.Speed > 0 && (int)(Random.Shared.NextDouble() * (((CombatController.Instance!.Context.Enemy.Speed - Player.Speed) / 4) + 80)) > 80))
            {
                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed < 8)
                    OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultNarrowlyAvoided".Localized());

                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed >= 8 && CombatController.Instance!.Context.Enemy.Speed - Player.Speed < 20)
                    OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultDodgedSwiftly".Localized());

                if (CombatController.Instance!.Context.Enemy.Speed - Player.Speed >= 20)
                    OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultDeftlyAvoided".Localized());

                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultLineBreak".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.FirstAttack))
                {
                    CombatController.Instance!.Attack();

                    return;
                }
                else
                    OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultLineBreakAgain".Localized());

                CombatController.Instance!.EnemyTurn();

                return;
            }

            damage = Player.Strength;

            damage += Player.WeaponAttack;

            damage *= 1.25f;

            var crit = false;

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.Stunned) || Random.Shared.Next(100) <= 4 || (Player.HasPerk(PerkIds.Tactician) && Player.Intelligence >= 50 && (Player.Intelligence - 50) / 5 > Random.Shared.Next(100)))
            {
                crit = true;

                damage *= 2;
            }

            float reduction = Random.Shared.Next((int)(CombatController.Instance!.Context.Enemy.Toughness));

            if (Player.WeaponName != "jeweled rapier" && Player.WeaponName != "deadly spear")
            {
                reduction += CombatController.Instance!.Context.Enemy.ArmorDefense;

                if (Player.HasPerk(PerkIds.LungingAttacks))
                    reduction -= CombatController.Instance!.Context.Enemy.ArmorDefense / 2;
            }

            if (Player.WeaponName == "katana")
                if (CombatController.Instance!.Context.Enemy.ArmorDefense >= 5)
                    reduction -= 5;
                else
                    reduction -= CombatController.Instance!.Context.Enemy.ArmorDefense;

            damage -= reduction;

            if (Player.HasPerk(PerkIds.ThunderousStrikes) && Player.Strength >= 80)
                damage *= 1.2f;

            if (damage > 0)
            {
                if (Player.HasPerk(PerkIds.HistoryFighter))
                    damage *= 1.1f;

                CombatCalculator.DamageEnemy(CombatController.Instance!.Context, damage, CombatDamageKind.Melee);
            }
            else
            {
                damage = 0;

                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultAttacksDeflected".Localized());
            }

            if (!(damage <= 0))
            {
                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultHitWithDamage".Localized(
                    damage
                ));

                if (crit)
                    OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultCritHit".Localized());
            }

            if (Player.HasPerk(PerkIds.BrutalBlows) && Player.Strength > 75)
            {
                if (CombatController.Instance!.Context.Enemy.ArmorDefense > 0)
                    OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultBrutalHitsBreak".Localized());

                if (CombatController.Instance!.Context.Enemy.ArmorDefense - 10 > 0)
                    CombatController.Instance!.Context.Enemy.ArmorDefense -= 10;
                else
                    CombatController.Instance!.Context.Enemy.ArmorDefense = 0;
            }

            OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultLineBreakThird".Localized());

            if (CombatController.Instance!.Context.Enemy.Health >= 1 && CombatController.Instance!.Context.Enemy.Lust <= 99)
            {
                if (Player.HasStatusEffect(StatusEffectIds.FirstAttack))
                {
                    CombatController.Instance!.Attack();

                    return;
                }

                OutputText("Scenes.UrtaQuest.UrtaVaultAttack.VaultLineBreakFourth".Localized());

                CombatController.Instance!.EnemyTurn();
            }
            else
                if (CombatController.Instance!.Context.Enemy.Health <= 0)
                {
                    CombatController.Instance!.State.Outcome = CombatOutcome.HpVictory;

                    CombatController.Instance!.Finish();
                }
                else
                {
                    CombatController.Instance!.State.Outcome = CombatOutcome.LustVictory;

                    CombatController.Instance!.Finish();
                }
        }

        public void NagaPleaseNagaStoleMyDick()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.FollowTalismanThroughForest".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.StepOnLeatheryTail".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.NagaHissesAngrily".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.NagaSmilesAtWoman".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.FlatteredAndUnsure".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.NagaNamesHimselfSirius".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.AlreadyWithSomeone".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.NagaChargesInRage".Localized());

            OutputText("Scenes.UrtaQuest.NagaPleaseNagaStoleMyDick.ItsAFight".Localized());

            CombatController.Instance!.ClearStatuses();

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "sirius")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void UrtaBeatsUpSiriusRadio()
        {

            if (CombatController.Instance!.Context.Enemy.Lust >= 99)
                OutputText("Scenes.UrtaQuest.UrtaBeatsUpSiriusRadio.SiriusPumpsWhileDazed".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaBeatsUpSiriusRadio.JawCrunchCollapse".Localized());

            OutputText("Scenes.UrtaQuest.UrtaBeatsUpSiriusRadio.CursingRottenLuck".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHim".Localized(), UrtaWinFucksSirius);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveSiriusBehind);
        }

        public void LeaveSiriusBehind()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LeaveSiriusBehind.NotWorthTheTime".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GnollAlphaBitchIntro);
        }

        public void UrtaWinFucksSirius()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.UndressAndExamine".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.TailCirclesUnnoticed".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.GripHeadAndClaim".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.NagaLaughsAtStruggle".Localized((noFur ? "" : "the fur on ")));

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.ResistingProvesHisPoint".Localized((noFur ? "" : "furry ")));

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.LaughingWithdrawFromBreasts".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.SashayHipsSwaying".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.CrawlsAwayInFear".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.CuriosityAtHisPlea".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.ProtestMuffledByBreasts".Localized((noFur ? "Common.Text.Soft".Localized() : "Scenes.Ivorysuccubusscene.AnalTail.FurryWord".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.WriggleToAlign".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.GetItOutYell".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.SnapStopWhining".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.SarcasmWhilePlumbed".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.SighOfReliefOver".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.PurrUseAnotherHole".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.PantingAskWhatMean".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.PurrSayAhh".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.ThrustIntoOpenMouth".Localized((noFur ? "" : "the fur at the base of ")));

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.GraspsHipsButYields".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.MuffledMmmmf".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.GurglingThroatFlood".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.DizzyButHorny".Localized());

            OutputText("Scenes.UrtaQuest.UrtaWinFucksSirius.CoughsSpitsSeed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FuckSiriusRadioII);
        }

        public void FuckSiriusRadioII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.FuckSiriusRadioII.GaspAndShudder".Localized());

            OutputText("Scenes.UrtaQuest.FuckSiriusRadioII.TotallySpentWonder".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.FuckSiriusRadioII.JerkButGrateful".Localized());

            OutputText("Scenes.UrtaQuest.FuckSiriusRadioII.SnoreAndBelch".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.FuckSiriusRadioII.WalkAwayWithPendant".Localized(
                player2.ShortName,

                player2.Mf("him", "her"),

                player2.Mf("he", "she")
            ));

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GnollAlphaBitchIntro);
        }

        public void UrtaLosesToSirriusSnakeRadio()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaBecomesSnakeSlave, 1);

            if (Player.Health < 1)
                OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.BruisedCryOfPain".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.HollowMoanOfAnguish".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.SiriusExaminesProneForm".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.CoiledSecondGlance".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.SarcasticRetort".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.ThrashingInCoils".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.HeadGripDeepGaze".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.GlaresAtResilience".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.LipBittenMoanRepressed".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.SpitefulSpitInsult".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.SwallowHardAtChuckle".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.GrowlingPantWriggle".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.DesperateDrippingMoan".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.LivingForCockAdmission".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.RealizationSinksIn".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.LoveMoreThanCock".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.ApologyForConfusion".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.SoothingCockPromise".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.RapturousDevotion".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.OrderedToStrip".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.PositionedInCoils".Localized(
                player2.ShortName,

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.SuddenStopConcern".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.GratefulNoddingAgreement".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.RulesOfLovingScreams".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.PlayfulOneSidedTease".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.FairTradeSmile".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.AcceptingHisRules".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.JoyfulResumePounding".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.TightenedEmbraceGroan".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.PossessivePantingSmile".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.LeakingLoveDemand".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.NoseTapReminder".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.FreedomForCockPrompt".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.MasterClapsPraise".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToSirriusSnakeRadio.DutifulFollowMaster".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SiriusSatelliteRadioOwnsYouII);
        }

        public void SiriusSatelliteRadioOwnsYouII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.WakingYawnStretch".Localized());

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.IdleThoughtsOfMaster".Localized((noFur ? "Common.Text.Girl".Localized() : "Common.Text.Morph".Localized())));

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.FireLitBreakfast".Localized());

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.SelfChidingAboutMaster".Localized(
                player2.Mf("guy", "girl"),

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.ApronHungLyingBack".Localized());

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.CoiledSqueakShock".Localized());

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.AnswerAndHeadPat".Localized());

            OutputText("Scenes.UrtaQuest.SiriusSatelliteRadioOwnsYouII.GleefulPounding".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGameOver);
        }

        public void GnollAlphaBitchIntro()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.GnollAlphaBitchIntro.TwoWearyingFights".Localized());

            OutputText("Scenes.UrtaQuest.GnollAlphaBitchIntro.FlatSavannaTerrain".Localized());

            OutputText("Scenes.UrtaQuest.GnollAlphaBitchIntro.SpearTrapSnare".Localized());

            OutputText("Scenes.UrtaQuest.GnollAlphaBitchIntro.BattleDeclaredBold".Localized());

            CombatController.Instance!.ClearStatuses();

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "gnoll_spear_thrower")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CombatController.Instance!.Context.Enemy.Health += 350;

            CombatController.Instance!.Context.Enemy.ShortName = "alpha gnoll";

            CombatController.Instance!.Context.Enemy.Health = CombatController.Instance!.Context.Enemy.MaxHealth;

            CombatController.Instance!.Context.Enemy.DescriptionOverride = "The gnoll standing before you is obviously an alpha among her kind; she has to be over seven feet tall and rippling with muscle, not that this stops her from having a curvy form, squeezable ass and full E-cup boobs. The remnants of what must have once been a gorgeous and expensive silken dress are draped across her figure, torn off at the knees and hanging by only a single shoulder, arms bare and exposed. A heavy necklace of gold is wrapped around her neck, while bracelets of more of the same adorn her arms, and piercings of gold stud her ears. She carries a mighty-looking spear in her hands, which she brandishes at you menacingly, and a basket of throwing javelins is strapped to her back.";
        }

        public void WinRapeHyenaPrincess()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.SkirtUnbuckledMocking".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.GuiltAssuagedTurning".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.WoundedYetExcited".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.SentencePeriod".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.ConfidentNoseSmear".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.HalfApologeticDemand".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.CompliantGlanceBack".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.SpearsPinAnkles".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.ArousalInterrogation".Localized((noFur ? "" : "fur-rimmed")));

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.PrincessDenial".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.StartlingAdmission".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.EnticingVirginGrin".Localized((noFur ? "" : " black")));

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.NippleTweakTaunt".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.FingerInMouthSuck".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.WordlessShakingGaze".Localized((noFur ? "" : ", furry")));

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.CrossedEyesClimax".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeHyenaPrincess.BlissfulMoanAnswer".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaNightSleep);
        }

        public void UseSuccubiMilkOnGnollPrincesses()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.SuccubiMilkedGnollPrincess, 1);

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.SuppressedLustDecision".Localized((noFur ? "" : "furry "), (noFur ? "Common.Text.Chin".Localized() : "BodyParts.Descript.Face.Noun.Snout".Localized())));

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.FistMuffledGrowl".Localized((noFur ? "Common.Text.Mouth".Localized() : "BodyParts.Descript.Face.Noun.Snout".Localized()), (noFur ? "" : "furry "), (noFur ? "Common.Text.BetweenHerBareThighsSitsHer".Localized() : "Common.Text.AsHerBosomFinishesGrowingThe".Localized())));

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.TransformedBodyShock".Localized((noFur ? "" : ", fingers hesitant to touch the bronze flesh now on display")));

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.SurprisedByChange".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.LustfulFocusRegained".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.YankedByHair".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.SwatToBreasts".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.BlushingBosomCup".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.OverwhelmedByCream".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.AnnoyedSlapAndBuck".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.TitwankEnjoyment".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.HiltedSlowSlide".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.ShudderRollOff".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.NotThereProtest".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.VirginRealization".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.TechnicalVirginJoke".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFuckedHel) == 1)
                OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.SalamanderSlutCunt".Localized());
            else
                OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.MinoAddictWhoreCunt".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.QuickenedPace".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.RoughPushTaunt".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.WhiningRestraint".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.SlapAndAccuse".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.ShoutedDenial".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.UltimatumNoOrgasm".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.SwatHeldStill".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.RefusalWhine".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.DemandingThwack".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.BeggingToCum".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.AdmitItShout".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.FilthyButtslutConfession".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.FingersDigFlurry".Localized());

            OutputText("Scenes.UrtaQuest.UseSuccubiMilkOnGnollPrincesses.CrossedEyesAnalClimax".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaNightSleep);
        }

        public void LoseToGnollPrincessAndGetGangBanged()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTakenByGnolls, 1);

            if (Player.Lust >= Player.MaxLust)
            {
                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.KneelingOverwhelmedByLust".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.WhimperDenial".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.CushionyWarmthGrip".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.HerEyesMeanIt".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.HyenaCoosObedience".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.LustyGazeOnFoe".Localized((noFur ? "Common.Text.In".Localized() : "Common.Text.UnderYourFurThankfulItConceals".Localized())));

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.PrecumBeadHangs".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.CruelVoiceDenial".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.OverconfidentTeasing".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.WhorishMoanAdmission".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.AlphaGirlDelight".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.TeasingAwayProtests".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.ArcingThrobbingErection".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.PraisedAsGoodGirl".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.CollapsingBadlyHurt".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.GaspingProtest".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.SpearRaisedThreat".Localized());

                OutputText("Scenes.UrtaQuest.LoseToGnollPrincessAndGetGangBanged.EverythingGoesBlack".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoseToGnollAsUrtaEpilogue);
        }

        public void LoseToGnollAsUrtaEpilogue()
        {
            ClearOutput();

            if (Player.Health < 1)
                OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.BuzzingThrobOfPain".Localized());
            else
                OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.GentleThrobWellUsed".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.ManyVoicesThrum".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.TerrorAtRestraints".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.ArrogantPrizeGreeting".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.HopeNotTooRough".Localized());
            else
                OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.JourneyEnjoyment".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.CaptorSmirksBelow".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SlaveLifeAcceptance".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SurveyingGnollHerd".Localized((noFur ? "" : ", making you glad it's hidden under your gray fur")));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SilentNod".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.EncouragingPraise".Localized());

            if (Player.Health >= 1)
                OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.MeetingMemoryShudder".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.KnifeCutsBindings".Localized((noFur ? "Common.Text.Feet".Localized() : "BodyParts.Descript.LowerBody.Feet.Fox".Localized())));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.BlushingNewOwner".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.BurningLiquorSwill".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SkinPeeledWhileDrinking".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.GropedFirstSlave".Localized((noFur ? "Common.Text.Clothes".Localized() : "Common.Text.Fur".Localized())));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SilencedByPrincess".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SmearingPreOnWood".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.LiftedToDrunkenSway".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.DozenGrabbingHands".Localized((noFur ? "Common.Text.Hands".Localized() : "BodyParts.Descript.LowerBody.Feet.Fox".Localized())));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.TinyBurstOfPain".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.JostlingSlaveBoys".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SmallestSlaveArrives".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.CockyGrinApproach".Localized((noFur ? "" : "His fur is silky-soft, and just a little longer than yours. ")));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.NeedyWhimperForCum".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.FiveGnollsUsingYou".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SpinningRoomEyesShut".Localized((noFur ? "H" : "Your silken fur tickles at his skin when h"), (noFur ? "" : "but "), (noFur ? "Common.Text.Ing".Localized() : "Common.Text.Es".Localized())));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.DeclaredInHeat".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SuddenStickyFlood".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.AlabasterEjaculation".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.ClenchedMilkingWalls".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.GibberishOfPleasure".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SpentGnollReplaced".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.ShadowPriedMouth".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.OrgasmAddledGrip".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SuckingDeepKnot".Localized((noFur ? "Common.Text.Chest".Localized() : "Common.Text.LighterColoredChestFur".Localized())));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.SwimmingInPleasure".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogue.MindBlankedByPleasure".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 20f), new DynStat(GameStats.Sensitivity, 100f), new DynStat(GameStats.Corruption, 20f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoseToGnollAsUrtaEpilogueBAM);
        }

        public void LoseToGnollAsUrtaEpilogueBAM()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.FaceSmackedByPaw".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.MistressClaimsTurn".Localized((noFur ? "Appearance.Cock.Sock.Covered".Localized() : "Common.Text.MattedYourFurTo".Localized())));

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.LiftedWithoutPause".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.FirstGirlScreamedPleasure".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.CumStolenBawl".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.QuestionToQueen".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.BarrenBawl".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.DistantQueenLaughs".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.SnarlingLift".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.GravidMiddleSadness".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.NodHandsOnCock".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.BouncingPrincessRide".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.OverstimulatedGnoll".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.QueenSlumpsBack".Localized());

            OutputText("Scenes.UrtaQuest.LoseToGnollAsUrtaEpilogueBAM.HolesRefilledSmile".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGameOver);
        }

        public void UrtaNightSleep()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaNightSleep.SunLowSeekShelter".Localized());

            OutputText("Scenes.UrtaQuest.UrtaNightSleep.ArmorChafesAtNight".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sleepnaked".Localized(), UrtaSleepsNaked);

            AddButton(1, "Common.Button.WearArmor".Localized(), UrtaSleepsArmored);
        }

        public void UrtaSleepsArmored()
        {
            ClearOutput();

            View.Time.Days += 1;

            OutputText("Scenes.UrtaQuest.UrtaSleepsArmored.WokenByTentacles".Localized((noFur ? "Common.Text.Feet".Localized() : "Common.Text.Paws".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaSleepsArmored.ShamblerFlees".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSleepsArmored.BeastAvoidedFight".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSleepsArmored.SlowReturnToSleep".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSleepsArmored.MorningFapRelief".Localized());

            Player.HealthChange(.5f * Player.MaxHealth);

            Player.ChangeFatigue(-50);

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IntroSuccubiAndMinotaur);
        }

        public void UrtaSleepsNaked()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaSleepsNaked.MoonlitNakedBedding".Localized());

            Player.HealthChange(Player.MaxHealth);

            Player.ChangeFatigue(-100);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGetsTentaRaped);
        }

        public void UrtaGetsTentaRaped()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.DreamDanceWithPartner".Localized(
                player2.ShortName,

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.FamiliarScentMissing".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.MonstrosityEclipsesMoon".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.SlipperyShaftGrip".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.NeedlesInBreasts".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.SucklingGreenTendril".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.BeadingWhiteBubbles".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.VacantHoodedEyes".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.ForgottenMouth".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.TeasingEquineColumn".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.BarelyFellatingTendril".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.CreviceClingingTendrils".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.LewdSoundsIntensify".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.PulsatingOrangePenis".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.NipplesFreedFromSuction".Localized());

            OutputText("Scenes.UrtaQuest.UrtaGetsTentaRaped.SensationsWithoutClimax".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), UrtaFinishesWithTentacles);

            AddButton(1, "Common.Button.BigOrange".Localized(), UrtaFinishesWithBigOrange);
        }

        public void UrtaFinishesWithBigOrange()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.StretchUpForPrick".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.AnticipateTheHead".Localized((noFur ? "Common.Text.OverYourBody".Localized() : "Common.Text.IntoYourFur".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.FondleWhileDrilled".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.SpreadYourselfOpen".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.TipNestlesIn".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.BeastBacksOff".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.BeastPressesThrough".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.RearsBackThenSpears".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Fur".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.MouthsSuckleBreasts".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithBigOrange.BlackOutFromPleasure".Localized(
                player2.ShortName
            ));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTentacleGaped, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTentacleRaped, 1);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IntroSuccubiAndMinotaur);
        }

        public void UrtaFinishesWithTentacles()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithTentacles.TentaclesEnvelopYou".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithTentacles.PullMouthsToBreasts".Localized());

            OutputText("Scenes.UrtaQuest.UrtaFinishesWithTentacles.FaintFromPleasure".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTentacleRaped, 1);

            Player.Orgasm("Lips");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), IntroSuccubiAndMinotaur);
        }

        public void IntroSuccubiAndMinotaur()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.PendantLightsTheWay".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.AdvanceThroughForest".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.ReachStoneStructure".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.ClearPathToDoors".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.SuccubusWhispers".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.TurnToSeeSuccubus".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.SuccubusCoos".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.DerisiveSnortReply".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.SuccubusCollectsCum".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.MinotaurGroansObedient".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.SuccubusSicsMinotaur".Localized());

            OutputText("Scenes.UrtaQuest.IntroSuccubiAndMinotaur.MinotaurChargesYou".Localized());

            CombatController.Instance!.ClearStatuses();

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "minotaur_lord")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void UrtaSubmitsToMinotaurBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.MouthOpensWithHunger".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.MinotaurEntangled))
                OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.MinotaurPullsYouOver".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.FoeRevealsHisCock".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.LapAtHisInterior".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.VoiceTeasesYou".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.GruffVoiceAsks".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.MelodiousScolding".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.FidoApologizes".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.RagPulledFromHead".Localized((noFur ? "Common.Text.SkinIsCoveredIn".Localized() : "Common.Text.FurIsMattedToYouBy".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.DemonGrantsPermission".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.MinotaurBowsToMistress".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.FidoSmilesDown".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.ScentFuzzesYourBrain".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.OpenWideToSuck".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.RoarsAndGrabsYou".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.HeatBuildsInside".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSubmitsToMinotaurBadEnd.HandsGropeYourBelly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SubmissiveMinoUrtaBadEndPtII);
        }

        public void SubmissiveMinoUrtaBadEndPtII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.WakeUpBloated".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.LookBackAndShiver".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.KneadYourNipples".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.DeepVoiceRumbles".Localized((noFur ? "" : ", vulpine")));

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.TasteHisSpooge".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.LiftAgainstHisSack".Localized((noFur ? "Common.Text.Feet".Localized() : "Common.Text.Hindpaws".Localized())));

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.WideningOfTheFlare".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.HowlsThenReleases".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.LickHimClean".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.FirstTasteWarning".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtII.TossedIntoPuddle".Localized(
                player2.ShortName
            ));

            Player.Orgasm("Lips");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SubmissiveMinoUrtaBadEndPtIII);
        }

        public void SubmissiveMinoUrtaBadEndPtIII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.FollowMasterAndMistress".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.SniffAtHisBalls".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.MistressCommandsStop".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.SeeTheVillage".Localized((noFur ? "Common.Text.People".Localized() : "Common.Text.Morphs".Localized())));

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.MistressOffersSlavery".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.SilenceAnswersHer".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.MistressUrgesReconsider".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.PinchFidosNipple".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.FistYourselfEasily".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.MasterPlungesIn".Localized((noFur ? "" : ", whetting your fur")));

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.TailSpearsIntoYou".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.FidoTwitchesInside".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.SomeTimeLater".Localized());

            OutputText("Scenes.UrtaQuest.SubmissiveMinoUrtaBadEndPtIII.ReturnToTemple".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaMinoAndSuccubiSlave, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGameOver);
        }

        public void WinRapeAMinoLordAsUrta()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.ExultAndUndress".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.LustyWord".Localized());
            else
                OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.PainedWord".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.DefeatedFoeMoos".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.WarnTheSuccubus".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.SuccubusTauntsYou".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.CircleHerButchPet".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.LiftHisAssUp".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.GlanceAtSuccubus".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.JerkOffOntoHim".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.SmackFidosAss".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.PatHisLowerBack".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.MilkSlutFrowns".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.ResumeSteadyPush".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.SuccubusCheersYou".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.ClearPuddleSignal".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.HurryUpTheAction".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.WrenchHisTail".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.BellowsLookingBack".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.PullOutAndGape".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.LordCollapsesInSpunk".Localized());

            OutputText("Scenes.UrtaQuest.WinRapeAMinoLordAsUrta.DressAndTurnBack".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaRapedMinoLord, 1);

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BeatMinoLordOnToSuccubi);
        }

        public void UrtaLosesToMinotaurRoughVersion()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.KneesGiveWay".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.BruisedAndWeak".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.LustWeakness".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.DefiantLastStand".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust < 70)
                OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.ContemptuousGlance".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.BullReachesDown".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.UnmindfulOfBlood".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.GravellyThreat".Localized((noFur ? "" : " vulpine")));

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.SavageYankUp".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.MessyPreSpurt".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.HeldAgainstBull".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.ImpotentTrickle".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.BellowingClimax".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.SpentAndDiscarded".Localized((noFur ? "Common.Text.Face".Localized() : "BodyParts.Descript.Face.Noun.Snout".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.DeafToWords".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.GrowthShock".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.RoarAndPounce".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.PriesMouthOpen".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.SuccuCowWatches".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.PressureBuilds".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.LongCummingSpree".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToMinotaurRoughVersion.AppetiteStirred".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 50f), new DynStat(GameStats.Sensitivity, 50f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaIsVulquineMenace, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaSheHulkPartII);
        }

        public void UrtaSheHulkPartII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.SeductivePetOffer".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.WhipCracksBack".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.TossesHerAside".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.ImpatientClaim".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.FeralAmusement".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.ObliviousToChange".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.PoundingRhythm".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.SterileSeedFills".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.LongestOrgasm".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.MiniatureDemon".Localized());

            OutputText("Scenes.UrtaQuest.UrtaSheHulkPartII.TwoHermsTarget".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 50f), new DynStat(GameStats.Sensitivity, 50f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGameOver);
        }

        public void BeatMinoLordOnToSuccubi()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.SlowApplause".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.SevenFootBeauty".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.LeatherWhipSpotted".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.AlluringPose".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.RetortHardly".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.BriefScowl".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.RetortRefusal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaRapedMinoLord) > 0)
            {
                OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.FingerSuckTease".Localized());

                OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.BitterLaugh".Localized());
            }

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.WhipSnapThreat".Localized());

            OutputText("Scenes.UrtaQuest.BeatMinoLordOnToSuccubi.FightChallenge".Localized());

            CombatController.Instance!.ClearStatuses();

            Player.SetWeapon((Weapon)ItemRegistry.Create(ItemIds.UrtasHalberd));
            // 原版：startCombat(new MilkySuccubus(), true)；旧移植把 `(), true` 混进了 id（创建即抛异常）
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "milky_succubus")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void MilkyUrtaTic()
        {

            OutputText("Scenes.UrtaQuest.MilkyUrtaTic.MilkHeatRising".Localized());

            if (Player.Lust < 50)
                OutputText("Scenes.UrtaQuest.MilkyUrtaTic.MildDistraction".Localized());
            else
                if (Player.Lust < 75)
                    OutputText("Scenes.UrtaQuest.MilkyUrtaTic.WaningResistance".Localized());
                else
                    OutputText("Scenes.UrtaQuest.MilkyUrtaTic.TemptingOrbs".Localized());

            OutputText("Scenes.UrtaQuest.MilkyUrtaTic.ParagraphBreak".Localized());

            Player.ChangeStatusValue(StatusEffectIds.MilkyUrta, 0, -1);

            if (Player.StatusEffectValue(StatusEffectIds.MilkyUrta, 0) <= 0)
                Player.RemoveStatusEffect(StatusEffectIds.MilkyUrta);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
        }

        public void UrtaBeatsUpCowcubi()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.HumiliatingTrip".Localized());

                OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.BondageLaugh".Localized());

                OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.FlatRetort".Localized());
            }
            else
            {
                OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.FuriousJilling".Localized());

                OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.SeductivePlea".Localized());

                OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.GuardedReply".Localized());
            }

            OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.ReliefTooSoon".Localized());

            OutputText("Scenes.UrtaQuest.UrtaBeatsUpCowcubi.FidelityDoubt".Localized(
                player2.ShortName,

                player2.Mf("he", "she")
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            ClearButtons();

            AddButton(0, "Common.Button.Fuck".Localized(), FuckTheCowCubi);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveTheCowCubi);
        }

        public void LeaveTheCowCubi()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.LeaveTheCowCubi.HalberdSwing".Localized(
                player2.ShortName
            ));

            ClearButtons();

            AddButton(0, "Common.Button.GoInside".Localized(), EnteringTaothsShrine);
        }

        public void FuckTheCowCubi()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.DemandsStrip".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.HatefulUndress".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.SizeDisappointment".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.RageSurge".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.SmugTeasing".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.RecallsEdryn".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.FlatteredSmile".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.SeethingThreat".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.GaggedProtest".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.SavageThroatRam".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.SpitAndTaunt".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.QueasyDoubt".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.ForcedThroatGrip".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.OminousJingle".Localized((noFur ? "Common.Text.Yelp".Localized() : "Common.Text.Yip".Localized())));

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.DelightedSwallow".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.TastingCritique".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.OutragedBlurt".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.GroggyRecovery".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.ResolveToBreak".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.GrimacingSpanks".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.EagerBegging".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.WillCrumbling".Localized());

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.FinalSpray".Localized());

            Player.Orgasm("Dick");

            OutputText("Scenes.UrtaQuest.FuckTheCowCubi.SinkingShame".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.MoveOn".Localized(), EnteringTaothsShrine);
        }

        public void UrtaLosesToCowCubi()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.KneesGiveOut".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.ArmsShakeBloody".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.StruggleArmorStraps".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.SuccubusCruelLaugh".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.EnthusiasticNod".Localized((noFur ? "" : ", smoothing back the fur as she does so")));

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.SamplingOwnMilk".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.LastArmorRemoved".Localized((noFur ? "Common.Text.ToYourSkin".Localized() : "Common.Text.IntoYourFur".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.PawsPulledAway".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.GrabbingFlaredHips".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.AddictionTaunt".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.TinglingMilkTaste".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.LostInHerTwat".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.SpoutsRetractToNubs".Localized());

            if (Player.Health < Player.MaxHealth)
                OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.HealingYourWounds".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.EngorgedNipples".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.CoaxingDrinkDeep".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.TailholeFingered".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.CumDeniedTickling".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.WhiningAroundTit".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.GrowlingHeadYank".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.ButtLoomsAbove".Localized((noFur ? "Common.Text.Lips".Localized() : "Common.Text.Muzzle".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.GrippingAssHard".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.WorshippingCunt".Localized((noFur ? "Common.Text.Tongue".Localized() : "BodyParts.Descript.Face.Noun.Snout".Localized())));

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.EcstaticCries".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.MistressClampsHead".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.CumTogetherScream".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.BlessedRelease".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.MilkSpillsOver".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.GoodGirlWhisper".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.ObeyingCommands".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.TenderEarScratch".Localized());

            OutputText("Scenes.UrtaQuest.UrtaLosesToCowCubi.PaddingAfterHer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGameOver);
        }

        public void EnteringTaothsShrine()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.AdvanceCautiously".Localized((noFur ? "" : "vulpine skulls, ")));

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.SkeletonExplore".Localized());

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.StatueDominatesRoom".Localized((noFur ? "Common.Text.Man".Localized() : "Common.Text.Morph".Localized()), (noFur ? "" : " head,")));

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.ContactingTaoth".Localized((noFur ? "Common.Text.Feet".Localized() : "BodyParts.Descript.LowerBody.Feet.Fox".Localized())));

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.VoiceCallsYou".Localized());

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.SepulchralWhisper".Localized());

            OutputText("Scenes.UrtaQuest.EnteringTaothsShrine.TaothAwakens".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), DeadGodsDreaming);
        }

        public void DeadGodsDreaming()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.StatueStartsMoving".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.AreYouTaoth".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.TwoDecadesTrapped".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.WhyMyFormDemand".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.StatueShrugs".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.RageBoilsOver".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.StatueFoldsArms".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.SwearToProtect".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.StatueLaughsBrave".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.SuspiciousQuestion".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.OffendedRetort".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.SuspiciouslyEasy".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.LamelyExplains".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.MustMateBreed".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.CovenantNeedsGod".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.TriumphantSmirk".Localized());

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.AwakingFromDream".Localized(
                player2.ShortName,

                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.DeadGodsDreaming.HalberdSupport".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaArrivesAtCampForFukks);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, 0.9f);

            ResetToPC();
        }

        public void UrtaArrivesAtCampForFukks()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.MeanwhileAtCamp".Localized());

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.MorningSurprise".Localized());

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.PantingGreeting".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.CannotMakeBabies".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.WouldRatherNot".Localized());

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.UseEdrynInstead".Localized());

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.ChooseFastWhimper".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.TaothSpeculation".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.FuckFoxOption".Localized());

            OutputText("Scenes.UrtaQuest.UrtaArrivesAtCampForFukks.SendToTelAdre".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.Knockurta".Localized(), KnockUpUrtaWithGodChild);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.Knockme".Localized(), GetKnockedUpWithGodChild);

            AddButton(2, "Common.Button.Edryn".Localized(), UrtaAndEdrynGodChild);
        }

        public void KnockUpUrtaWithGodChild()
        {
            ClearOutput();

            var x = Player.CockThatFits(UrtaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.SameOldSolution".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.PromiseToFillHer".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.FoxSmilesGratefully".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.DrippingWetness".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.YourOwnWord".Localized());
            else
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.OneCockToken".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.HardClitNub".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.TongueMeetsHers".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.PantingUrtaRides".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.MessWorsenedByVirility".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.NipplesDigIntoYours".Localized());

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.KissGoingDeeper".Localized());

                if (Player.LactationQ() > 50)
                    OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.MilkySpurt".Localized());

                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.WetHolesDrawingGasps".Localized());
            }
            else
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.TinglesThroughNubs".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.ArtlessHumpingCringe".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.ConfusedGirlMoans".Localized((noFur ? "Common.Text.Yelping".Localized() : "Common.Text.Yipping".Localized()), Player.CockDescript(x), (noFur ? "" : ", furry")));

            if (Player.Balls > 0)
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.SackToken".Localized());
            else
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.TaintWord".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.OffWhiteTintFlow".Localized());

            if (Player.CumQ() > 1000)
                OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.WombPackingBelly".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.SentencePeriod".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.OrgasmicComeDown".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.ProudUrtaDeclaration".Localized());

            OutputText("Scenes.UrtaQuest.KnockUpUrtaWithGodChild.RidingThroughOrgasms".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PostFuckUrtaUp);
        }

        public void PostFuckUrtaUp()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.WakeQueriesAnswered".Localized());

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.UrtaSparesTrouble".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.AskingHerPlans".Localized());

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.WeighingTheRisk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
                OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.LazyDemonSacrifice".Localized());

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.JoyousExpressionConfirms".Localized());

            if (CampScene.FollowersCount() >= 2)
                OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.CampInstructions".Localized());

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.WastelandDeparture".Localized());

            OutputText("Scenes.UrtaQuest.PostFuckUrtaUp.ArrivalAtGates".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PreggedUrtaWithGodChildEpilogue);

            View.Time.Days += 1;

            View.Time.Hours = 7;
        }

        public void PreggedUrtaWithGodChildEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.WakingToCries".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.SlowLabourOnset".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.BirthingBallEmerges".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.UnfoldingNewborn".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.ChildDeclaresLife".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.SilentMindVoice".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.ThankingUrtaMother".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.IndifferentShrug".Localized());
            else
                OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.SentencePeriod".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.UpholdBargain".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.PartingWordsToParents".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.ChildGodBows".Localized());

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.FoxGirlHug".Localized(
                player2.ShortName
            ));

            OutputText("Scenes.UrtaQuest.PreggedUrtaWithGodChildEpilogue.LeavingTowerTogether".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, 1);

            CombatController.InCombat = false;

            DoNext(() => ReturnToCamp(1));
        }

        public void GetKnockedUpWithGodChild()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.CoyUndressing".Localized());

            if (Player.IsPregnant() && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.OvenAlreadyFilled".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.GreenEyesBulge".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.TossingFuelOnFire".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.LiftingBalls".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.PuffyLipsSpread".Localized());

            if ((Player.Vaginas.Count > 0))
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.PinkWord".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.DuskyWord".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.GlisteningInteriorWhine".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.RuttingVixenThrust".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.WetWord".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.NoseDeepSquish".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.RoundedWord".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.OnyxNipplesTickle".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SoftCurveLine".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.FlatMuscleLine".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.ReachingYourNipple".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.TonguePlungesInside".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.TongueAcrossSurface".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.ClimbingHigherCock".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SnarlingThreat".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.CrazinessReturns".Localized());

            if (Player.VaginalCapacity() < 60)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.PainfulPenetration".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.DeliciouslyWord".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.HandsRoamBody".Localized());

            Player.CuntChange(60, true, true);

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.GrippingHerWaist".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.ForgottenLength".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.LetterS".Localized());

                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.IgnoredBouncing".Localized());
            }
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SlipperyWetness".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.BetterThanOught".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.ArchingBackCry".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.AudibleGurgle".Localized((noFur ? "Scenes.Masturbation.TentacleSelfFuck.MoanWord".Localized() : "Common.Text.Yipping".Localized())));

            if (Player.Balls > 0)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.BothSetsWord".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.HerPossessive".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SpunkSlatheredBalls".Localized());

            var ballWorship = new List<int>();
            // 原版 UrtaQuest.hx:2635 — 腐化 Amily 与腐化 Jojo 同时在场
            if (SceneInstance<Npcs.AmilyScene>.Get().AmilyFollowerCorrupt() && SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo())
                ballWorship.Add(0);
            // 原版 UrtaQuest.hx:2638 — 两人之一在场
            if (SceneInstance<Npcs.AmilyScene>.Get().AmilyFollowerCorrupt() || SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo())
                ballWorship.Add(1);
            // 原版 UrtaQuest.hx:2641 — 傻化 Sophie 在场
            if (SceneInstance<Npcs.SophieBimboScene>.Get().BimboSophie())
                ballWorship.Add(2);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) > 0)
                ballWorship.Add(3);
            // 原版 UrtaQuest.hx:2647 — 乳胶女孩跟随者
            if (SceneInstance<Npcs.LatexGirlScene>.Get().LatexGooFollower())
                ballWorship.Add(4);

            if (CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1)
                ballWorship.Add(5);

            int choice = 0;

            if (ballWorship.Count > 0)
                choice = ballWorship[Random.Shared.Next(ballWorship.Count)];
            else
                choice = -1;

            if (choice == 0)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.TwinTonguesSurprise".Localized());
            else
                if (choice == 1)
                {
                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SingleTongueSurprise".Localized());
                    // 原版 UrtaQuest.hx:2665 — 腐化 Jojo 才用他的称呼
                    if (SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo())
                        OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.JojoName".Localized());
                    else
                        OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.AmilyName".Localized());

                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.MouseTongueDevours".Localized());
                }
                else
                    if (choice == 2)
                        OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.UrtaGaspGiltLips".Localized());
                    else
                        if (choice == 3)
                        {
                            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.UrtaGaspScaledTongue".Localized(
                                player2.ShortName
                            ));

                            if (Player.Balls > 0)
                                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SheReturnsTheFavor".Localized());

                            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.HeliaEggsYouOn".Localized());
                        }
                        else
                            if (choice == 4)
                                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.UrtaGaspInkyTongue".Localized());
                            else
                                if (choice == 5)
                                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.UrtaGaspGooPressure".Localized());
                                else
                                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.UrtaHumpsSlowly".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.UrtaPullsOutSwat".Localized((noFur ? "" : "furry ")));

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.TailAngleChallenge".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.HandsAndForelegs".Localized());
                else
                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.BlanketsSoakedMusky".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SentencePeriod".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.LoverHeadFromBehind".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.ShaftBottomsOut".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.LadySpunkShowers".Localized());
            else
                if (Player.Wetness() >= 3)
                    OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.LadySpunkDrools".Localized());
                else
                    if (Player.Wetness() >= 1)
                        OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.LadySpunkDrips".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.HungryMouthCleans".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.MuscularContractions".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SheCriesSmack".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.SlammingAssault".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.ShePumpsSeedHose".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpWithGodChild.BreathyVoiceWhispers".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GetKnockedUpUrtaEpilogue);
        }

        public void GetKnockedUpUrtaEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.WhenYouWake".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.MuchMorePrefix".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.PregnantPrefix".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.ThanBeforeSuffix".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.VeryPregnantSuffix".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.UrtaSeedDevelops".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.SheStuttersCovenant".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.UrtaBeltsGear".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpUrtaEpilogue.GatesWavedThrough".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GetKnockedUpByUrtaEpilogueII);

            View.Time.Days += 1;

            View.Time.Hours = 7;
        }

        public void GetKnockedUpByUrtaEpilogueII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.LaborPangsWake".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.LaborSlowOnset".Localized());

            Player.CuntChange(40, true, true);

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.BirthingBallRolls".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.BallUnfoldsLegs".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.ILiveSpeech".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.TelepathicVoice".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.StillPregnantAfter".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.VigorAtTouch".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.ThankYouUrtaFather".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.WhatDoYouCare".Localized());
            else
                OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.SentencePeriod".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.UpholdTheBargain".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.NoneButYouMother".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.UrtaNodsAtChild".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.FoxGirlHug".Localized());

            OutputText("Scenes.UrtaQuest.GetKnockedUpByUrtaEpilogueII.TowerExitGoodbye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, 1);

            CombatController.InCombat = false;

            DoNext(() => ReturnToCamp(1));
        }

        public void UrtaAndEdrynGodChild()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChild.UrtaShuddersRestraint".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaFertile, CoC.Instance.Flags.Get(GameFlagIds.EdrynPregnancyType) == 31 ? 1 : 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPregEverybody, Player.Pregnancy.Incubation);

            CoC.Instance.Flags.Set(GameFlagIds.EdrynPregnancyType, 31);

            CoC.Instance.Flags.Set(GameFlagIds.EdrynPregnancyIncubation, 24);

            CombatController.InCombat = false;

            DoNext(() => ReturnToCamp(1));
        }

        public void UrtaAndEdrynGodChildEpilogue()
        {

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogue.FastFigureApproaches".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogue.SheDoublesOverPanting".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogue.UrtaGreenEyesSmile".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogue.PullFreeFollowHer".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogue.TelAdreGatesLoom".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaAndEdrynGodChildEpilogueII);
        }

        public void UrtaAndEdrynGodChildEpilogueII()
        {
            ClearOutput();

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.DoorsOpenMoan".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.HorseGirlWord".Localized());
            // 原版 UrtaQuest.hx:2810 — UrtaQuest.hx：Edryn 怀孕中
            if (SceneInstance<Places.TelAdre.EdrynScene>.Get().Pregnancy.IsPregnant)
                OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.HowPregnantAgain".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.UrtaLeadsBend".Localized((noFur ? "" : "gray-furred ")));
            // 原版 UrtaQuest.hx:2814 — Edryn 怀孕中
            if (SceneInstance<Places.TelAdre.EdrynScene>.Get().Pregnancy.IsPregnant)
                OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.MassivelyPrefix".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.FlanksRippleBirth".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.BirthingBallMat".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.BallUnfoldsLimbs".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.ILiveSpeech".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.TelepathicVoice".Localized());
            // 原版 UrtaQuest.hx:2822 — UrtaQuest.hx：Edryn 怀孕中
            if (SceneInstance<Places.TelAdre.EdrynScene>.Get().Pregnancy.IsPregnant)
                OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.EdrynStillPregnant".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.EdrynFallsAsleep".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.ThankYouUrtaFather".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.WhatDoYouCare".Localized());
            else
                OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.SentencePeriod".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.UpholdTheBargain".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.NoneButYouLove".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.UrtaNodsAtChild".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.FoxDragsHug".Localized());

            OutputText("Scenes.UrtaQuest.UrtaAndEdrynGodChildEpilogueII.TowerLeaveGoodbye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaQuestStatus, 1);

            CombatController.InCombat = false;

            DoNext(() => ReturnToCamp(1));
        }

        public static float UrtaCapacity()
        {
            var flags = CoC.Instance.Flags;

            var bonus = flags.Get(GameFlagIds.TimesRutFuckedUrtasCunt) * 5;

            if (bonus > 40)
                bonus = 40;

            if (flags.Get(GameFlagIds.UrtaTentacleGaped) > 0)
                return 500;

            return 60 + bonus;
        }

        public static bool UrtaDrunk()
        {
            var flags = CoC.Instance.Flags;

            if (flags.Get(GameFlagIds.UrtaPregnancyType) != 0)
                return false;

            if (flags.Get(GameFlagIds.UrtaDrinkFrequency) == -1)
                return false;

            if (View.Time.Hours > 12 && flags.Get(GameFlagIds.UrtaDrinkFrequency) < 1)
                return true;

            return View.Time.Hours > 8 && flags.Get(GameFlagIds.UrtaDrinkFrequency) >= 1;
        }

        public static bool UrtaLove(int love = 0)
        {
            var flags = CoC.Instance.Flags;

            if (flags.Get(GameFlagIds.UrtaPcLoveCounter) == -1)
                return false;

            if (love == 0)
                return flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1;

            if (love > 0)
                flags.Set(GameFlagIds.UrtaPcAffectionCounter, flags.Get(GameFlagIds.UrtaPcAffectionCounter) + love);

            if (love < 0)
                flags.Set(GameFlagIds.UrtaPcAffectionCounter, flags.Get(GameFlagIds.UrtaPcAffectionCounter) - love);

            if (flags.Get(GameFlagIds.UrtaPcLoveCounter) != 1)
                return false;

            // 原版 UrtaQuest.hx：还需 Amily 是纯爱跟随者且未禁用 Urta
            if (flags.Get(GameFlagIds.UrtaPcLoveCounter) != 1
                || !SceneInstance<Npcs.AmilyScene>.Get().AmilyFollowerPure()

                || GameSettings.UrtaDisabled)

                return false;

            if (love > 0 && flags.Get(GameFlagIds.AmilyVisitingUrta) == 0)
                flags.Set(GameFlagIds.AmilyNeedToFreakAboutUrta, 1);

            return true;
        }
    }
}