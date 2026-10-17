using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<LatexGirlScene>("coc:latex_girl")]
    public class LatexGirlScene : Scene
    {

        public Identifier cockType = BodyPartTypes.Human;

        public override void Entry()
        {
            GooName();
        }

        public string GooName()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.GooName);
        }

        public float GooFluid(float arg = 0, bool output = true)
        {

            if (arg != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GooFluidAmount, CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) + arg);

                if (output)
                    OutputText("Scenes.Latexgirl.GooFluid.FluidChangeBold".Localized(
                        (float)(arg * 10) / 10
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) < 1)
                    CoC.Instance.Flags.Set(GameFlagIds.GooFluidAmount, 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.GooFluidAmount, 100);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount);
        }

        public float GooHappiness(float arg = 0, bool output = true)
        {

            if (arg != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GooHappiness, CoC.Instance.Flags.Get(GameFlagIds.GooHappiness) + arg);

                if (output)
                    OutputText("Scenes.Latexgirl.GooHappiness.HappinessChangeBold".Localized(
                        (float)(arg * 10) / 10
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.GooHappiness) < 1)
                    CoC.Instance.Flags.Set(GameFlagIds.GooHappiness, 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.GooHappiness) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.GooHappiness, 100);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.GooHappiness);
        }

        public float GooObedience(float arg = 0, bool output = true)
        {

            if (arg != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GooObedience, CoC.Instance.Flags.Get(GameFlagIds.GooObedience) + arg);

                if (output)
                    OutputText("Scenes.Latexgirl.GooObedience.ObedienceChangeBold".Localized(
                        (float)(arg * 10) / 10
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.GooObedience) < 1)
                    CoC.Instance.Flags.Set(GameFlagIds.GooObedience, 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.GooObedience) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.GooObedience, 100);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.GooObedience);
        }

        public string GooTits()
        {

            return Appearance.BreastSizeDescript(GooTitSize());
        }

        public string GooCock()
        {

            return Appearance.CockDescript(Player);
        }

        public bool LatexGooFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.GooSlaveRecruited) > 0;
        }

        public bool LatexGooAtCamp()
        {

            return LatexGooFollower() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmLatexy) == 0;
        }

        public float GooTitSize()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) / 2 >= CoC.Instance.Flags.Get(GameFlagIds.GooPreferredTitSize) && CoC.Instance.Flags.Get(GameFlagIds.GooPreferredTitSize) > 0)
                return CoC.Instance.Flags.Get(GameFlagIds.GooPreferredTitSize);
            else
                return CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) / 2;
        }

        public void MeanGooGirlRecruitment()
        {
            ClearOutput();

            CombatController.InCombat = false;

            CoC.Instance.Flags.Set(GameFlagIds.GooTfedMean, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooEyes, CurrentMonster.Body.Get<SkinPart>().Tone);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBlack));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack));

            OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.ApproachGelGirl".Localized());

            if (Player.Speed > 60)
                OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.SpinFingerTossAir".Localized());
            else
                OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.GraspInHand".Localized());

            OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.UncomprehendingLookUp".Localized());

            OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.PushEggInPussy".Localized());

            OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.MotionsStopRetract".Localized());

            OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.WigglesFailsToRise".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.CheckHerBreathing".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.ConfusedLostConsciousnessCheck".Localized());
                else
                    OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.IrritatedItemsReactingUnusual".Localized());

            OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.GotLatexGooGirl".Localized());

            if (Player.Intelligence >= 60)
            {
                OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.QuicklyFindWoodReeds".Localized());

                Player.ChangeFatigue(20);
            }
            else
                if (Player.Strength >= 60)
                {
                    OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.HeaveOverShoulderStraining".Localized());

                    if (Player.Toughness < 40)
                    {
                        OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.HalfwayTooTiredContinue".Localized());

                        DoNext(() => ReturnToCamp(1));

                        Player.ChangeFatigue(30);

                        return ;
                    }

                    OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.MadeItToCamp".Localized());

                    Player.ChangeFatigue(10);
                }
                else
                {
                    OutputText("Scenes.Latexgirl.MeanGooGirlRecruitment.TryLiftTooHeavy".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PCCarriedGooBackHome);
        }

        public void PCCarriedGooBackHome()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.SetGooDownCamp".Localized());

            if (FollowerRegistry.All.Count > 0)
                OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.AwayFromPryingEyes".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.LooksAlmostAlien".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.WonderSheathCocks".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.SurprisinglyHasHairSuppl".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.FeelThatYouKnow".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.BackpedalWildlyNearlyFall".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.StepForwardConsciousPrize".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.EyesWideDinnerPlates".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.ExplainGooGirlsIntrigued".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.BeNiceMaster".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.KeepHerHealthy".Localized());
                else
                    OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.ObeysNothingToFear".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.AskWhatToCall".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHome.OnyxTeardropForgottenSea".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PCCarriedGooBackHomeII);
        }

        public void PCCarriedGooBackHomeII()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHomeII.CallMeAnything".Localized());

            OutputText("Scenes.Latexgirl.PCCarriedGooBackHomeII.WhatWillYouName".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NameZeLatexGoo);

        }

        public void NameZeLatexGoo()
        {
            ClearOutput();

            var input = "" ;

            if (input == "")
            {
                OutputText("Scenes.Latexgirl.NameZeLatexGoo.MustSelectNameBold".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), NameZeLatexGoo);

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.GooName, input);

            if (!NameReact(input))
            {
                DoNext(() => NameZeLatexGoo());

                return ;
            }

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.StormCloudAcrossBrow".Localized());

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.TakeHerByChin".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Latexgirl.NameZeLatexGoo.Patiently".Localized());
            else
                OutputText("Scenes.Latexgirl.NameZeLatexGoo.Impatiently".Localized());

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.NotToLeaveCamp".Localized());

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.NotMuchLiquid".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Latexgirl.NameZeLatexGoo.PleaseGrowSomeGenitals".Localized());

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.EveryNowAndAgain".Localized());

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.NodTellGetSettled".Localized());

            OutputText("Scenes.Latexgirl.NameZeLatexGoo.AddedToSlavesMenu".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GooSlaveRecruited, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooHappiness, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooObedience, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooFluidAmount, 100);

            DoNext(() => ReturnToCamp(1));
        }

        public bool NameReact(string name)
        {

            switch (name)
            {
                case "Fenoxo":
                {
                OutputText("Scenes.Latexgirl.NameReact.NotThatSlimy".Localized());

                return false;
                }
            }

            OutputText("Scenes.Latexgirl.NameReact.SayStart".Localized());

            switch (name)
            {
                case "Cattleya":
                {
                OutputText("Scenes.Latexgirl.NameReact.TitsBigEnough".Localized());
                }

                break;

                case "Helia":
                {
                OutputText("Scenes.Latexgirl.NameReact.HeliaHuhAnalEnough".Localized());
                }

                break;

                case "Urta":
                {
                OutputText("Scenes.Latexgirl.NameReact.WhatAFurry".Localized());
                }

                break;

                case "Tifa":
                {
                OutputText("Scenes.Latexgirl.NameReact.TifaHuhLikeSound".Localized());
                }

                break;

                case "Aeris":
                {
                OutputText("Scenes.Latexgirl.NameReact.AerisHuhHoityToity".Localized());
                }

                break;

                case "Latexy":
                {
                OutputText("Scenes.Latexgirl.NameReact.LatexyHuhRealOriginal".Localized());
                }

                break;

                case "Goo":
                {
                OutputText("Scenes.Latexgirl.NameReact.NotTooBright".Localized());
                }

                break;

                case "Blacky":
                {
                OutputText("Scenes.Latexgirl.NameReact.LookLikeHorse".Localized());
                }

                break;

                case "Fetish":
                {
                OutputText("Scenes.Latexgirl.NameReact.FetishHuhPrettyTransparent".Localized());
                }

                break;

                case "Maria":
                {
                OutputText("Scenes.Latexgirl.NameReact.MariaFuckCursed".Localized());
                }

                break;

                case "Valeria":
                {
                OutputText("Scenes.Latexgirl.NameReact.ValeriaWaitHeardBefore".Localized());
                }

                break;

                case "Christmas":
                {
                OutputText("Scenes.Latexgirl.NameReact.ChristmasHuhVeryFestive".Localized());
                }

                break;

                case "Symphonie":
                {
                OutputText("Scenes.Latexgirl.NameReact.SymphonieEhVeryFitting".Localized());
                }

                break;

                case "Savin":
                {
                OutputText("Scenes.Latexgirl.NameReact.SecondWorstPerson".Localized());
                }

                break;

                case "Galatea":
                {
                OutputText("Scenes.Latexgirl.NameReact.GalateaByOblimoBeautiful".Localized());
                }

                break;

                case "Kara":
                {
                OutputText("Scenes.Latexgirl.NameReact.KaraHuhFrenchMa".Localized());
                }

                break;

                case "Karazelle":
                {
                OutputText("Scenes.Latexgirl.NameReact.KarazelleHuhFrenchMa".Localized());
                }

                break;

                case "Jacques":
                {
                OutputText("Scenes.Latexgirl.NameReact.KindOfBoysName".Localized());
                }

                break;

                case "Whitney":
                {
                OutputText("Scenes.Latexgirl.NameReact.FarmGirlFrigidCunt".Localized());
                }

                break;

                case "Hedrah":
                {
                OutputText("Scenes.Latexgirl.NameReact.HedrahNiceStrongName".Localized());
                }

                break;

                case "Third":
                {
                OutputText("Scenes.Latexgirl.NameReact.ThirdHuhSillyAccent".Localized());
                }

                break;

                case "Luckster":
                {
                OutputText("Scenes.Latexgirl.NameReact.LucksterHuhStrangeLucky".Localized());
                }

                break;
            }

            if (true )
                OutputText("Scenes.Latexgirl.NameReact.CanLiveWithThat".Localized());

            OutputText("Scenes.Latexgirl.NameReact.SayEndMoodBrightening".Localized());

            return true;
        }

        public void EncounterLeftBehindGooSlave()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GooTfedNice) > 0)
            {
                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.SeeOddBlackBlob".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.MoansLookingUp".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.WonderSheathCocks".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.SurprisinglyHasHairSuppl".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.FeelThatYouKnow".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.BackpedalWildlyNearlyFall".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.StepForwardConscious".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.TellSideOfStory".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.ComfortingWithoutNameAsk".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.OnyxTeardropForgottenSea".Localized());
            }
            else
            {
                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.SeeOddBlobAgain".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.ExitConcealment".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.TextParagraphBegin".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.SighingApologetically".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.Frowning".Localized());
                    else
                        OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.Smirking".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.NodAndAdmit".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                ));

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.TakesFewShudderingSobs".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.ExplainGooGirlsIntrigued".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.BeNiceMaster".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.KeepHerHealthy".Localized());
                    else
                        OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.ObeysNothingToFear".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.LeadHerToCamp".Localized());

                OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlave.SolitaryOnyxTeardropAgain".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), EncounterLeftBehindGooSlaveII);
        }

        public void EncounterLeftBehindGooSlaveII()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlaveII.CallMeAnything".Localized());

            OutputText("Scenes.Latexgirl.EncounterLeftBehindGooSlaveII.WhatWillYouName".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NameZeLatexGoo);

        }

        public void PureGooRecruitmentStart()
        {
            ClearOutput();

            CombatController.InCombat = false;

            CoC.Instance.Flags.Set(GameFlagIds.GooTfedNice, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooEyes, CurrentMonster.Body.Get<SkinPart>().Tone);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.EggBlack)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggBlack));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.EggLargeBlack));

            OutputText("Scenes.Latexgirl.PureGooRecruitmentStart.ScuffleTooMuch".Localized());

            OutputText("Scenes.Latexgirl.PureGooRecruitmentStart.LeanOverDefeatedDecide".Localized());

            OutputText("Scenes.Latexgirl.PureGooRecruitmentStart.AbruptlyMotionsStopSubstances".Localized());

            OutputText("Scenes.Latexgirl.PureGooRecruitmentStart.WigglesFailsToRise".Localized());

            OutputText("Scenes.Latexgirl.PureGooRecruitmentStart.CheckHerBreathing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeHome".Localized(), NiceGuysTakeLatexHome);

            AddButton(4, "Common.Button.LeaveHer".Localized(), LeaveTheLatexGooGirl);
        }

        public void LeaveTheLatexGooGirl()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LeaveTheLatexGooGirl.PutDownOnShore".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NiceGuysTakeLatexHome()
        {
            ClearOutput();

            if (Player.Intelligence >= 60)
            {
                OutputText("Scenes.Latexgirl.NiceGuysTakeLatexHome.QuicklyFindWoodReeds".Localized());

                Player.ChangeFatigue(20);
            }
            else
                if (Player.Strength >= 60)
                {
                    OutputText("Scenes.Latexgirl.NiceGuysTakeLatexHome.HeaveOverShoulderStraining".Localized());

                    if (Player.Toughness < 40)
                    {
                        OutputText("Scenes.Latexgirl.NiceGuysTakeLatexHome.HalfwayTooTiredContinue".Localized());

                        Player.ChangeFatigue(30);

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    OutputText("Scenes.Latexgirl.NiceGuysTakeLatexHome.MadeItToCamp".Localized());

                    Player.ChangeFatigue(10);
                }
                else
                {
                    OutputText("Scenes.Latexgirl.NiceGuysTakeLatexHome.TryLiftTooHeavy".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PureGooGalRecruitAftermath);
        }

        public void PureGooGalRecruitAftermath()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.SetGooDownCamp".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.WonderSheathCocks".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.SurprisinglyHasHairSuppl".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.FeelThatYouKnow".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.BackpedalWildlyNearlyFall".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.StepForwardConscious".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.EyesWideDinnerPlates".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.GentlyTellWhatHappened".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.ComfortingWithoutNameAsk".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermath.OnyxTeardropForgottenSea".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PureGooGalRecruitAftermathII);
        }

        public void PureGooGalRecruitAftermathII()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermathII.CallMeAnything".Localized());

            OutputText("Scenes.Latexgirl.PureGooGalRecruitAftermathII.WhatWillYouName".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NameZeLatexGooNice);

        }

        public void NameZeLatexGooNice()
        {
            ClearOutput();

            var input = "" ;

            if (input == "")
            {
                OutputText("Scenes.Latexgirl.NameZeLatexGooNice.MustSelectNameBold".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), NameZeLatexGoo);

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.GooName, input);

            if (!NameReact(input))
            {
                DoNext(() => NameZeLatexGooNice());

                return ;
            }

            OutputText("Scenes.Latexgirl.NameZeLatexGooNice.StartGettingUsedTo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.KeepHer".Localized(), NiceGuysKeepTheirGooGals);

            AddButton(1, "Common.Button.BootHer".Localized(), BootOutNiceGoo);
        }

        public void BootOutNiceGoo()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.BootOutNiceGoo.DidCivicDuty".Localized());

            OutputText("Scenes.Latexgirl.BootOutNiceGoo.WavingOverShoulder".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GooTossedAfterNaming, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void NiceGuysKeepTheirGooGals()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.NiceGuysKeepTheirGooGals.TakeByChin".Localized());

            OutputText("Scenes.Latexgirl.NiceGuysKeepTheirGooGals.PaymentEnough".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Latexgirl.NiceGuysKeepTheirGooGals.GrowSomeGenitals".Localized());

            OutputText("Scenes.Latexgirl.NiceGuysKeepTheirGooGals.PromiseFeelGood".Localized());

            OutputText("Scenes.Latexgirl.NiceGuysKeepTheirGooGals.NodTellGetSettled".Localized());

            OutputText("Scenes.Latexgirl.NiceGuysKeepTheirGooGals.AddedToSlavesMenu".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GooSlaveRecruited, 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooHappiness, 60);

            CoC.Instance.Flags.Set(GameFlagIds.GooObedience, 20);

            CoC.Instance.Flags.Set(GameFlagIds.GooFluidAmount, 100);

            DoNext(() => ReturnToCamp(1));
        }

        public void ApproachLatexy()
        {
            ClearOutput();

            if (GooHappiness() < 10)
                OutputText("Scenes.Latexgirl.ApproachLatexy.ScowlsUpUnhappinessVisible".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                ));
            else
                if (GooHappiness() < 20)
                    OutputText("Scenes.Latexgirl.ApproachLatexy.GlaresClearlyUnhappy".Localized());
                else
                    if (GooHappiness() < 30)
                        OutputText("Scenes.Latexgirl.ApproachLatexy.FrownsDispleased".Localized());
                    else
                        if (GooHappiness() < 40)
                            OutputText("Scenes.Latexgirl.ApproachLatexy.KnitsEyebrowsConsternation".Localized());
                        else
                            if (GooHappiness() < 50)
                                OutputText("Scenes.Latexgirl.ApproachLatexy.GlancesUpVisiblyBored".Localized());
                            else
                                if (GooHappiness() < 60)
                                    OutputText("Scenes.Latexgirl.ApproachLatexy.WanSmileTadBored".Localized());
                                else
                                    if (GooHappiness() < 70)
                                        OutputText("Scenes.Latexgirl.ApproachLatexy.CracksSmallGrin".Localized());
                                    else
                                        if (GooHappiness() < 80)
                                            OutputText("Scenes.Latexgirl.ApproachLatexy.OpenlySmilesGoodSpirits".Localized());
                                        else
                                            if (GooHappiness() < 90)
                                                OutputText("Scenes.Latexgirl.ApproachLatexy.GrinsGlitteringTeeth".Localized(
                                                    CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                                                ));
                                            else
                                                if (GooHappiness() < 100)
                                                    OutputText("Scenes.Latexgirl.ApproachLatexy.BouncesUpWideSmile".Localized(
                                                        CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                                                    ));
                                                else
                                                    OutputText("Scenes.Latexgirl.ApproachLatexy.JumpsUpClapping".Localized(
                                                        CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                                                    ));

            if (GooTitSize() >= 35)
                OutputText("Scenes.Latexgirl.ApproachLatexy.BigArmsBarelyEncompass".Localized(
                    GooTits()
                ));
            else
                if (GooTitSize() >= 24)
                    OutputText("Scenes.Latexgirl.ApproachLatexy.WobblingChestBarelyBound".Localized());
                else
                    if (GooTitSize() >= 15)
                        OutputText("Scenes.Latexgirl.ApproachLatexy.HideTorsoPonderously".Localized(
                            GooTits()
                        ));
                    else
                        if (GooTitSize() > 4)
                            OutputText("Scenes.Latexgirl.ApproachLatexy.WellRoundedJiggly".Localized(
                                GooTits()
                            ));
                        else
                            if (GooTitSize() > 3)
                                OutputText("Scenes.Latexgirl.ApproachLatexy.ProudPairJiggle".Localized(
                                    GooTits()
                                ));
                            else
                                if (GooTitSize() > 2)
                                    OutputText("Scenes.Latexgirl.ApproachLatexy.HighCurvyPertPerky".Localized(
                                        GooTits()
                                    ));
                                else
                                    OutputText("Scenes.Latexgirl.ApproachLatexy.DisproportionatelyTiny".Localized(
                                        GooTits()
                                    ));

            OutputText("Scenes.Latexgirl.ApproachLatexy.EstimateBraSize".Localized(
                Appearance.BreastSizeDescript(GooTitSize())
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.GooNippleType) == 1)
                OutputText("Scenes.Latexgirl.ApproachLatexy.ProudNipplesOddBulges".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GooNippleType) == -1)
                    OutputText("Scenes.Latexgirl.ApproachLatexy.ProudNipplesVisibleSlit".Localized());

            if (GooFluid() >= 75)
                OutputText("Scenes.Latexgirl.ApproachLatexy.OnyxLipsPlushInviting".Localized());
            else
                if (GooFluid() >= 50)
                    OutputText("Scenes.Latexgirl.ApproachLatexy.OnyxLipsFullPouty".Localized());
                else
                    if (GooFluid() >= 25)
                        OutputText("Scenes.Latexgirl.ApproachLatexy.OnyxLipsSmallSide".Localized());
                    else
                    {
                        OutputText("Scenes.Latexgirl.ApproachLatexy.OnyxLipsThinLine".Localized());

                        if (GooFluid() <= 5)
                            OutputText("Scenes.Latexgirl.ApproachLatexy.LooksVeryUnhealthySurvive".Localized());
                    }

            OutputText("Scenes.Latexgirl.ApproachLatexy.TextParagraphSpacer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                OutputText("Scenes.Latexgirl.ApproachLatexy.HangsSoftAndShiny".Localized(
                    NumberUtils.Num2Text((float)(CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) * 10) / 10),

                    GooCock()
                ));

            OutputText("Scenes.Latexgirl.ApproachLatexy.GlitteringCleftOozesLubricant".Localized());

            OutputText("Scenes.Latexgirl.ApproachLatexy.TextParagraphSpacerAgain".Localized());

            if (GooObedience() < 10)
                OutputText("Scenes.Latexgirl.ApproachLatexy.LookOfPureDefiance".Localized());
            else
                if (GooObedience() < 20)
                    OutputText("Scenes.Latexgirl.ApproachLatexy.IdlyTwirlsRavenLock".Localized(
                        CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                    ));
                else
                    if (GooObedience() < 30)
                        OutputText("Scenes.Latexgirl.ApproachLatexy.SmirksRebelliousCast".Localized());
                    else
                        if (GooObedience() < 40)
                            OutputText("Scenes.Latexgirl.ApproachLatexy.BatsCharcoalEyelashesFiery".Localized(
                                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                            ));
                        else
                            if (GooObedience() < 50)
                                OutputText("Scenes.Latexgirl.ApproachLatexy.GlancesDownAvertingEyes".Localized());
                            else
                                if (GooObedience() < 60)
                                    OutputText("Scenes.Latexgirl.ApproachLatexy.CastsEyesDownPeeks".Localized());
                                else
                                    if (GooObedience() < 70)
                                        OutputText("Scenes.Latexgirl.ApproachLatexy.EyesCastDown".Localized(
                                            CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
                                        ));
                                    else
                                        if (GooObedience() < 80)
                                            OutputText("Scenes.Latexgirl.ApproachLatexy.GazeDownFlushing".Localized());
                                        else
                                            if (GooObedience() < 90)
                                                OutputText("Scenes.Latexgirl.ApproachLatexy.ProstratesOnGround".Localized());
                                            else
                                                if (GooObedience() < 100)
                                                    OutputText("Scenes.Latexgirl.ApproachLatexy.SubmissivelyProstratesOnyxRump".Localized());
                                                else
                                                    OutputText("Scenes.Latexgirl.ApproachLatexy.KissingFeet".Localized());

            OutputText("Scenes.Latexgirl.ApproachLatexy.FluidPercentBold".Localized(
                GooFluid()
            ));

            OutputText("Scenes.Latexgirl.ApproachLatexy.HappinessPercentBold".Localized(
                GooHappiness()
            ));

            OutputText("Scenes.Latexgirl.ApproachLatexy.ObediencePercentBold".Localized(
                GooObedience()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), LatexyAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), LatexyTalkMenu);

            AddButton(2, "Common.Button.FeedHer".Localized(), FeedLatexy);

            AddButton(3, "Common.Button.UseHer".Localized(), UseLatexy);

            AddButton(5, "CharCreation.GenericStyleCustomizeMenu.Button6".Localized(), SetLatexysBustSize);

            AddButton(6, "Common.Button.DickOptions".Localized(), ChangeGooDick);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmLatexy) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmLatexy) == 1)
            {
                AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);

                AddButton(14, "Common.Button.Back".Localized(), () => { });
            }
            else
                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void LatexyAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LatexyAppearance.StandingBeforeYou".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes),

                GooTits(),

                CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) != 0 ? GooCock() : "dripping folds"
            ));

            DoNext(() => ApproachLatexy());
        }

        public void LatexyTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LatexyTalkMenu.WhatSpeakAbout".Localized());

            ClearButtons();

            AddNextButton("Common.Button.HowIsShe".Localized(), LatexyHowIsShe);

            AddNextButton("Common.Button.GooGirls".Localized(), LatexyGoos);

            AddNextButton("Common.Button.Latex".Localized(), LatexyLatex);

            AddButton(14, "Common.Button.Back".Localized(), () => ApproachLatexy());
        }

        public void LatexyHowIsShe()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LatexyHowIsShe.HowGooSlaveDoing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooHappiness) >= 75)
            {
                OutputText("Scenes.Latexgirl.LatexyHowIsShe.KeptMeWellFed".Localized());

                OutputText("Scenes.Latexgirl.LatexyHowIsShe.EnjoyingCaptivity".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GooHappiness) >= 25)
                {
                    OutputText("Scenes.Latexgirl.LatexyHowIsShe.FineSaysMeekly".Localized());

                    OutputText("Scenes.Latexgirl.LatexyHowIsShe.NotOpenlyUpset".Localized());
                }
                else
                {
                    OutputText("Scenes.Latexgirl.LatexyHowIsShe.DourLookQuicklyHides".Localized());

                    OutputText("Scenes.Latexgirl.LatexyHowIsShe.InspiresNoConfidence".Localized());
                }

            DoNext(() => LatexyTalkMenu());
        }

        public void LatexyGoos()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LatexyGoos.NeverUnderstoodGooGirls".Localized());

            OutputText("Scenes.Latexgirl.LatexyGoos.PerksUpThenFrowns".Localized());

            OutputText("Scenes.Latexgirl.LatexyGoos.AtLengthMelancholyNote".Localized());

            OutputText("Scenes.Latexgirl.LatexyGoos.NoUseThinking".Localized());

            OutputText("Scenes.Latexgirl.LatexyGoos.ConcludesEyesFarAway".Localized());

            DoNext(() => LatexyTalkMenu());
        }

        public void LatexyLatex()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LatexyLatex.NormalGooAdjustment".Localized());

            OutputText("Scenes.Latexgirl.LatexyLatex.FarMoreMalleable".Localized((GooHappiness() >= 75 || GooObedience() >= 75 ? "I suppose... I suppose it's not entirely displeasing, thought I do miss my old body. It" : "I... I hate it. Every moment I remember what I've lost. My old body")));

            OutputText("Scenes.Latexgirl.LatexyLatex.LooksStandingThereSuppose".Localized((GooHappiness() >= 75 || GooObedience() >= 75 ? "somewhat ambivalent" : "almost anguished")));

            DoNext(() => LatexyTalkMenu());
        }

        public void SendToFarm()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.SendToFarm.HeadTowardsLake".Localized());

            OutputText("Scenes.Latexgirl.SendToFarm.NoSneakingOff".Localized());

            OutputText("Scenes.Latexgirl.SendToFarm.SighsAsYouWish".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmLatexy, 1);

            ReturnToCamp(1);
        }

        public void BackToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.BackToCamp.HeadBackToCamp".Localized());

            OutputText("Scenes.Latexgirl.BackToCamp.WatchSlowPonderousProgress".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmLatexy, 0);
        }

        public void UseLatexy()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.UseLatexy.HowWillYouUse".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.Dickfuckher".Localized(), MalePCDomFucksLatexGoo);

            if (Player.HasVagina())
            {
                AddButton(1, "Common.Button.Domwithvag".Localized(), FemalePCDomFucksLatexGoo);

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                    AddButton(2, "Common.Button.Ridegoocock".Localized(), FemalePCDomFucksLatexGooFuta);
            }

            AddButton(14, "Common.Button.Back".Localized(), ApproachLatexy);
        }

        public void ChangeGooDick()
        {
            ClearOutput();

            if (GooObedience() >= 60)
            {
                OutputText("Scenes.Latexgirl.ChangeGooDick.YouLead".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
                    OutputText("Scenes.Latexgirl.ChangeGooDick.ConsideredGrowingDick".Localized());
                else
                    OutputText("Scenes.Latexgirl.ChangeGooDick.AskChangeDickKind".Localized());

                OutputText("Scenes.Latexgirl.ChangeGooDick.LovedGrowingPenises".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
                    OutputText("Scenes.Latexgirl.ChangeGooDick.WantToGrowOne".Localized());
                else
                    OutputText("Scenes.Latexgirl.ChangeGooDick.OrderMeToShift".Localized());

                OutputText("Scenes.Latexgirl.ChangeGooDick.ChangingMyselfSexualPlayground".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
                    OutputText("Scenes.Latexgirl.ChangeGooDick.NeedSomethingMasculine".Localized());
                else
                    OutputText("Scenes.Latexgirl.ChangeGooDick.WhatKindOfPenis".Localized());
            }
            else
                if (GooHappiness() >= 70)
                {
                    OutputText("Scenes.Latexgirl.ChangeGooDick.YouAskIf".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
                        OutputText("Scenes.Latexgirl.ChangeGooDick.ConsideredDickForFun".Localized());
                    else
                        OutputText("Scenes.Latexgirl.ChangeGooDick.ChangeKindOfDick".Localized());

                    OutputText("Scenes.Latexgirl.ChangeGooDick.BeamsTittersOmigodLoved".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
                        OutputText("Scenes.Latexgirl.ChangeGooDick.TapsChinNeedMasculine".Localized());
                    else
                        OutputText("Scenes.Latexgirl.ChangeGooDick.AlmostAnyKind".Localized());
                }
                else
                {
                    OutputText("Scenes.Latexgirl.ChangeGooDick.YouLeadAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
                        OutputText("Scenes.Latexgirl.ChangeGooDick.ConsideredGrowingDickAgain".Localized());
                    else
                        OutputText("Scenes.Latexgirl.ChangeGooDick.ChangeDickKindAgain".Localized());

                    OutputText("Scenes.Latexgirl.ChangeGooDick.MadeMeChangeEnough".Localized());

                    OutputText("Scenes.Latexgirl.ChangeGooDick.LearnToBeObedient".Localized());

                    GooObedience(-3);

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), ApproachLatexy);

                    return ;
                }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
            {
                AddButton(0, "Items.IncubiDraft.Short".Localized(), () => LatexyEatsADickItem(ConsumableIds.IncubiDraft));

                AddButton(1, "Items.MinotaurBlood.Short".Localized(), () => LatexyEatsADickItem(ConsumableIds.MinotaurBlood));

                AddButton(2, "Common.Button.CaninePepper".Localized(), () => LatexyEatsADickItem(ConsumableIds.CaninePepper));

                AddButton(3, "Items.Equinum.Short".Localized(), () => LatexyEatsADickItem(ItemIds.Equinum));

                AddButton(4, "Common.Button.PureDraft".Localized(), () => LatexyEatsADickItem(ConsumableIds.PurifiedIncubiDraft));

                AddButton(5, "Common.Button.WhiskerFruit".Localized(), () => LatexyEatsADickItem(ConsumableIds.W_FRUIT));

                AddButton(6, "Items.GroPlus.Short".Localized(), () => LatexyEatsADickItem(ItemIds.GroPlus));

                AddButton(7, "Items.Reducto.Short".Localized(), () => LatexyEatsADickItem(ItemIds.Reducto));
            }
            else
            {
                AddButton(0, "Items.IncubiDraft.Short".Localized(), () => LatexyEatsADickItem(ConsumableIds.IncubiDraft));

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                    AddButton(0, "Common.Button.PureDraft".Localized(), () => LatexyEatsADickItem(ConsumableIds.PurifiedIncubiDraft));

                AddButton(1, "Items.MinotaurBlood.Short".Localized(), () => LatexyEatsADickItem(ConsumableIds.MinotaurBlood));
            }

            AddButton(14, "Common.Button.Back".Localized(), ApproachLatexy);
        }

        public void LatexyEatsADickItem(Identifier item)
        {
            ClearOutput();

            Player.ConsumeItem(item, 1);

            OutputText("Scenes.Latexgirl.LatexyEatsADickItem.UsesProfferedItem".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) == 0)
            {
                OutputText("Scenes.Latexgirl.LatexyEatsADickItem.OnyxMoundBulges".Localized());

                if (item == ConsumableIds.MinotaurBlood)
                    OutputText("Scenes.Latexgirl.LatexyEatsADickItem.FlattenedHead".Localized());
                else
                    OutputText("Scenes.Latexgirl.LatexyEatsADickItem.BulbousCrown".Localized());

                OutputText("Scenes.Latexgirl.LatexyEatsADickItem.CockTipThickens".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GooDickLength, 8);

                cockType = BodyPartTypes.Human;

                if (item == ConsumableIds.MinotaurBlood)
                {
                    OutputText("Scenes.Latexgirl.LatexyEatsADickItem.HungLikeStallion".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GooDickLength, 13);

                    cockType = BodyPartTypes.Horse;
                }

                OutputText("Scenes.Latexgirl.LatexyEatsADickItem.StiffenArchingUp".Localized());

                OutputText("Scenes.Latexgirl.LatexyEatsADickItem.WhinesMakingItHarder".Localized(
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.GooDickLength))
                ));

                if (item == ConsumableIds.MinotaurBlood)
                    OutputText("Scenes.Latexgirl.LatexyEatsADickItem.Horse".Localized());

                OutputText("Scenes.Latexgirl.LatexyEatsADickItem.CockBold".Localized());

                GooObedience(5);
            }
            else
            {
                if (item == ItemIds.GroPlus)
                {
                    OutputText("Scenes.Latexgirl.LatexyEatsADickItem.SproutOutward".Localized(
                        GooCock()
                    ));

                    CoC.Instance.Flags.Set(GameFlagIds.GooDickLength, CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) + 3);
                }
                else
                    if (item == ItemIds.Reducto)
                    {
                        OutputText("Scenes.Latexgirl.LatexyEatsADickItem.ShrinkInward".Localized(
                            GooCock()
                        ));

                        CoC.Instance.Flags.Set(GameFlagIds.GooDickLength, (float)(CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) * .66f));
                    }
                    else
                    {
                        OutputText("Scenes.Latexgirl.LatexyEatsADickItem.RapidlyErectsFullTumescence".Localized(
                            GooCock()
                        ));

                        if (item == ConsumableIds.CaninePepper)
                            cockType = BodyPartTypes.Dog;

                        if (item == ItemIds.Equinum)
                            cockType = BodyPartTypes.Horse;

                        if (item == ConsumableIds.PurifiedIncubiDraft)
                            cockType = BodyPartTypes.Human;

                        if (item == ConsumableIds.W_FRUIT)
                            cockType = BodyPartTypes.Cat;

                        if (item == ConsumableIds.IncubiDraft)
                            cockType = BodyPartTypes.Demon;

                        if (item == ConsumableIds.MinotaurBlood)
                            cockType = BodyPartTypes.Horse;

                        OutputText("Scenes.Latexgirl.LatexyEatsADickItem.PlaceholderBold".Localized(
                            GooCock()
                        ));
                    }

                GooObedience(2);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApproachLatexy);
        }

        public void SetLatexysBustSize()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.SetLatexysBustSize.KeepingCertainSize".Localized());

            if (GooObedience() < 60)
            {
                OutputText("Scenes.Latexgirl.SetLatexysBustSize.HandsOnHips".Localized());

                GooObedience(-2);

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), ApproachLatexy);

                return ;
            }
            else
                if (GooObedience() < 80)
                    OutputText("Scenes.Latexgirl.SetLatexysBustSize.NodsCasually".Localized((GooTitClass((int)(GooTitSize())) < 3 ? "palms her tits, perking them up" : "hefts her tits in her hands, bouncing them back and forth")));
                else
                    OutputText("Scenes.Latexgirl.SetLatexysBustSize.NodsSluttyExpression".Localized((GooTitClass((int)(GooTitSize())) < 3 ? "palms her tits, perking them up" : "hefts her tits in her hands, bouncing them back and forth")));

            ClearButtons();

            AddButton(0, "Common.Button.ACups".Localized(), () => ChangeLatexyTits(1));

            AddButton(1, "Common.Button.CCups".Localized(), () => ChangeLatexyTits(3));

            AddButton(2, "Common.Button.DdCups".Localized(), () => ChangeLatexyTits(4));

            AddButton(3, "Common.Button.Volleyballs".Localized(), () => ChangeLatexyTits(8));

            AddButton(4, "Common.Button.Basketballs".Localized(), () => ChangeLatexyTits(15));

            AddButton(5, "Common.Button.Huge".Localized(), () => ChangeLatexyTits(24));

            AddButton(6, "Common.Button.Gigantic".Localized(), () => ChangeLatexyTits(35));

            AddButton(7, "Common.Button.Whatever".Localized(), () => ChangeLatexyTits(0));

            AddButton(14, "Common.Button.Back".Localized(), ApproachLatexy);
        }

        public int GooTitClass(int arg)
        {

            if (arg >= 35)
                return 7;
            else
                if (arg >= 24)
                    return 6;
                else
                    if (arg >= 15)
                        return 5;
                    else
                        if (arg > 4)
                            return 4;
                        else
                            if (arg > 3)
                                return 3;
                            else
                                if (arg > 2)
                                    return 2;
                                else
                                    return 1;
        }

        public void ChangeLatexyTits(int arg = 0)
        {
            ClearOutput();

            if (GooTitClass(arg) > GooTitClass(CoC.Instance.Flags.Get(GameFlagIds.GooPreferredTitSize)) && GooTitClass((int)(CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) / 2)) < GooTitClass(arg))
            {
                if (GooTitClass((int)(CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) / 2)) == GooTitClass((int)(GooTitSize())))
                    OutputText("Scenes.Latexgirl.ChangeLatexyTits.BodyDoesntChange".Localized());
                else
                    OutputText("Scenes.Latexgirl.ChangeLatexyTits.HandsOnNipples".Localized());
            }
            else
                if (GooTitClass(arg) == 7)
                    OutputText("Scenes.Latexgirl.ChangeLatexyTits.HandsOnNipplesAgain".Localized());
                else
                    if (GooTitClass(arg) == 6)
                    {
                        if (GooTitClass((int)(GooTitSize())) < 6)
                            OutputText("Scenes.Latexgirl.ChangeLatexyTits.PlacesHandsUndersideWinks".Localized(
                                GooTits()
                            ));
                        else
                            OutputText("Scenes.Latexgirl.ChangeLatexyTits.HandsUndersideWinks".Localized());
                    }
                    else
                        if (GooTitClass(arg) == 5)
                        {
                            if (GooTitClass((int)(GooTitSize())) < 5)
                                OutputText("Scenes.Latexgirl.ChangeLatexyTits.CupsPinchesAreolaTugging".Localized(
                                    GooTits()
                                ));
                            else
                                OutputText("Scenes.Latexgirl.ChangeLatexyTits.GrabsBloatedChest".Localized());
                        }
                        else
                            if (GooTitClass(arg) == 4)
                            {
                                if (GooTitClass((int)(GooTitSize())) < 4)
                                    OutputText("Scenes.Latexgirl.ChangeLatexyTits.CantGoBigger".Localized());
                                else
                                    OutputText("Scenes.Latexgirl.ChangeLatexyTits.WishMadeThemBigger".Localized());
                            }
                            else
                                if (GooTitClass(arg) == 3)
                                {
                                    if (GooTitClass((int)(GooTitSize())) < 3)
                                        OutputText("Scenes.Latexgirl.ChangeLatexyTits.SighsCupsGrowAloud".Localized());
                                    else
                                        OutputText("Scenes.Latexgirl.ChangeLatexyTits.LikedEmBig".Localized(
                                            GooTits()
                                        ));
                                }
                                else
                                    if (GooTitClass(arg) == 2)
                                    {
                                        if (GooTitClass((int)(GooTitSize())) < 2)
                                            OutputText("Scenes.Latexgirl.ChangeLatexyTits.CupsTinyTeats".Localized());
                                        else
                                            OutputText("Scenes.Latexgirl.ChangeLatexyTits.ShuddersObeysPressesDown".Localized(
                                                GooTits()
                                            ));
                                    }
                                    else
                                        if (GooTitClass(arg) == 1 && arg > 0)
                                            OutputText("Scenes.Latexgirl.ChangeLatexyTits.SaysLookAnemicStarving".Localized(
                                                Player.Mf("he'll", "she'll"),

                                                GooTits()
                                            ));
                                        else
                                        {
                                            OutputText("Scenes.Latexgirl.ChangeLatexyTits.BlushesSmileRadiantMidnight".Localized());

                                            GooHappiness(2);
                                        }

            CoC.Instance.Flags.Set(GameFlagIds.GooPreferredTitSize, arg);

            if (GooObedience() < 75)
                GooObedience(3);

            DoNext(() => ApproachLatexy());
        }

        public void FeedLatexy()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FeedLatexy.HowWillYouFeed".Localized());

            if (Player.Lust < 33 && (int)Player.Gender > 0)
                OutputText("Scenes.Latexgirl.FeedLatexy.ArentArousedEnough".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                AddButton(0, "Common.Button.CumIndirect".Localized(), FeedLatexyCumIndirectly);

                AddButton(1, "Common.Button.CumDirect".Localized(), FeedLatexyCumDirectly);
            }

            if (Player.HasVagina())
            {
                AddNextButton("Common.Button.GirlcumInd".Localized(), FeedLatexyGirlCumIndirectly);

                AddNextButton("Common.Button.GirlcumDir".Localized(), FeedLatexyGirlCumDirect);
            }

            if (GooHappiness() >= 50 && Player.LactationQ() >= 100 && Player.BiggestBreastSize() >= 3)
                AddNextButton("Common.Button.Milk".Localized(), FeedLatexySomeMilk);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurCum)))
            {
                AddNextButton("Common.Button.MinocumNic".Localized(), () => MinotaurCumFeedingGoo(true));

                AddNextButton("Common.Button.MinocumRuf".Localized(), () => MinotaurCumFeedingGoo(false));
            }

            AddButton(14, "Common.Button.Back".Localized(), ApproachLatexy);
        }

        public void FeedLatexyCumIndirectly()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GooIndirectFed) == 0)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.TossWoodenBowlGround".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GooIndirectFed, 1);
            }

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.GlancingDownFeedingTime".Localized());

            if (GooObedience() < 70)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.ReachingForArmor".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.ObedientlyWaitingTongueWagging".Localized());

            if (GooFluid() < 33)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.HungerRelentlessDrillBody".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.Body".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.Ballsack".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.SentencePeriod".Localized());
            }
            else
                if (GooFluid() < 66)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.HungerPalpable".Localized());
                else
                    if (GooFluid() < 90)
                        OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.HungerLazyAlwaysWilling".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.GluttonousHunger".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.HardensRemovingGear".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.One".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.It".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.PlumpingFullRigidityStrokes".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.MasturbateHandsSlip".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.GroanEnrapturedDisplaySoon".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.ThickerRiverPreCum".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.DrippingMorePreCum".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.DribblingPreCum".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.OozingRiverPreCum".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.CockHeadPerfectFodder".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.DisappointCumTorrent".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.AllOf".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.YourTip".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.SAtOnce".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.RainingIntoBowl".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.OverfilledSprayRemainder".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.RelentlessPumping".Localized());

            if (Player.CumQ() >= 4000)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.FootDeepLakeSpunk".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.DivesIntoBowlGusto".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.TurnsToCleaningHerself".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.SentencePeriodAgain".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.FeedLatexyCumIndirectly.JizzDevouredGround".Localized());

            var temp = 20;

            if (Player.CumQ() >= 500)
                temp += 10;

            GooFluid(temp);

            GooHappiness(4);

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void FeedLatexyGirlCumIndirectly()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GooIndirectFed) == 0)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.TossWoodenBowlGround".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GooIndirectFed, 1);
            }

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.GlancingDownFeedingTime".Localized());

            if (GooObedience() < 70)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.ReachingForArmor".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.ObedientlyWaitingTongueWagging".Localized());

            if (GooFluid() < 33)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.HungerRelentlessDrillBody".Localized());
            else
                if (GooFluid() < 66)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.HungerPalpable".Localized());
                else
                    if (GooFluid() < 90)
                        OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.HungerLazyAlwaysWilling".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.GluttonousHunger".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.WriggleOutOfBottoms".Localized());

            if (Player.GetClitLength() >= 3.5f)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.StandsOutProud".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.EnrapturedViewEchoingMotion".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.GroanEnrapturedDisplaySoon".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.GirlishMoisture".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.DrippingStringsGirlishLiquid".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.GushingIntoBowl".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.InstinctivelyRockHipsPumping".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.DisappointCumBolt".Localized());

            if (Player.GetClitLength() >= 3.5f)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.WrapHandAround".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.LiquidEcstasyRising".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.DribblingCunnyThinFilm".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.LudicrouslyWetCuntOozes".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.CunnySpraysFlow".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.DivesIntoBowlGusto".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumIndirectly.SadlyVaginalLubeAbsorbed".Localized());

            var temp = 10;

            if (Player.Wetness() >= 3)
                temp += 5;

            if (Player.Wetness() >= 4)
                temp += 5;

            if (Player.Wetness() >= 5)
                temp += 5;

            Player.Orgasm("Vaginal");

            GooFluid(temp);

            GooHappiness(4);

            DoNext(() => ReturnToCamp(1));
        }

        public void MinotaurCumFeedingGoo(bool nice = false)
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.MinotaurCum));

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.PullCumVial".Localized());

            if (Player.MinotaurNeed() || Player.MinotaurAddicted())
                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.HungrilyLickLipsEager".Localized());

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.TheLatexWoman".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFedLatexyMinoCum) > 0)
                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.ClapsExcitementNipplesPerking".Localized());
            else
                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.TiltsHeadConfusion".Localized());

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.SwishBottle".Localized());

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.BabblesForMe".Localized());

            if (Player.Tallness >= 72)
                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.Chest".Localized());
            else
                if (Player.Tallness >= 60)
                    OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.Neck".Localized());
                else
                    if (Player.Tallness >= 52)
                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.Own".Localized());
                    else
                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.Hair".Localized());

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.StrikingEyesImploringlyBeg".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            if (!nice)
            {
                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.ShakeHeadExplain".Localized());

                if (GooObedience() >= 70)
                    OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.DropsToAllFours".Localized());
                else
                    if (GooObedience() >= 40)
                    {
                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.TriflingProtest".Localized());

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.SmirkingMarvelLearnedBeg".Localized());

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.MomentHesitationQuiveringPile".Localized());

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.AllowHerToStop".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.ButYouCapturedMe".Localized());

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.SuggestGetOnKnees".Localized());

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.OpenMouthAndBeg".Localized());

                        if (GooFluid() >= 66)
                        {
                            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.GrowlsNotThirsty".Localized());

                            GooObedience(-5);

                            GooHappiness(-3);

                            DoNext(() => ReturnToCamp(1));

                            return ;
                        }

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.ReadyToQuit".Localized());

                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.ThatsMoreLikeIt".Localized());
                    }
            }

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.UncorkAndHand".Localized());

            if (Player.MinotaurNeed())
            {
                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.FlushEchoingNeedPounce".Localized());

                if (GooHappiness() < 33)
                    OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.ReluctantlyReturnsKissTolerate".Localized());
                else
                    if (GooHappiness() < 66)
                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.SwappingCummySpit".Localized());
                    else
                        OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.EagerlyReturnsKissFrenching".Localized());

                OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.PullBackPantingUnsated".Localized());

                Player.MinoCumAddiction(2);
            }

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.SentencePeriod".Localized());

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.SensualHugThanks".Localized());

            OutputText("Scenes.Latexgirl.MinotaurCumFeedingGoo.CollapsesFlat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            GooFluid(20);

            GooHappiness(15);

            if (nice)
                GooObedience(-1);
            else
                GooObedience(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void FeedLatexyCumDirectly()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.FlopFreeLooksDown".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            if (GooFluid() < 33)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.RumbleTightBellyTremble".Localized());

                if (GooObedience() > 50)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.MasterAffection".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.QuestionSayEnd".Localized());
            }
            else
                if (GooFluid() < 66)
                {
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.SmirkSpreads".Localized());

                    if (GooObedience() > 50)
                        OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.MasterAffectionAgain".Localized());

                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.SentenceSayEnd".Localized());
                }
                else
                {
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.LazySmileRevealsItself".Localized());

                    if (GooObedience() > 50)
                        OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.MasterAffectionThird".Localized());

                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.SentenceSayEndAgain".Localized());
                }

            if (GooFluid() > 50)
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.PlumpO".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.O".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.NyxLipsPurseSpreading".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.WinkRevealsTongue".Localized());

            if (Player.BiggestCockArea() > 80)
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.ThroatStretchedInkyCondom".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.NoGagReflexSwallows".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.BuriedDeepGroan".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.WreathedInBlackness".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.SighsGurglingVibration".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.FingersDigIntoButt".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.YourRemaining".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.DickFruitlesslyPaints".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.DicksFruitlesslyPaint".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.SheenOfWhite".Localized(
                    GooTits()
                ));
            }

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.CreamInternalsBellyDistends".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.RounderAndRounderBalloonish".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.OrgasmWindingDownMetabolizing".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.UtterlyEmptySagged".Localized());

            if (GooObedience() >= 60)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.CommandWhimper".Localized());

                if (GooHappiness() >= 66)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.ThankYouForFood".Localized());
                else
                    if (GooHappiness() >= 33)
                        OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.MuttersThanksForFood".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.ComplainsReallyThatsIt".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.KnowingNod".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.AndSpatter".Localized(
                        GooCock()
                    ));
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.Spatters".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.OilyWetnessOntoGround".Localized());

                var temp = 20;

                if (Player.CumQ() >= 500)
                    temp += 5;

                GooFluid(temp);

                GooObedience(5);

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Latexgirl.FeedLatexyCumDirectly.SuctionRedoubles".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHer".Localized(), MaleDirectFeedLatexGooGoneWrong);

                AddButton(1, "Common.Button.Try2assert".Localized(), TryToAssertMaleDomWhileLatexGooDrains);
            }
        }

        public void MaleDirectFeedLatexGooGoneWrong()
        {
            ClearOutput();

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.FlawlessHandWrapsAround".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.YourRemainingErections".Localized());
                else
                    OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.YourOtherErection".Localized());

                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.Stroking".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.It".Localized());
                else
                    OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.Them".Localized());

                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.EjaculateAsLubricant".Localized());
            }

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.ExplosionOfEcstasySqueezes".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.EachOf".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.YourCumSlit".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.SFourth".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.LooksUpSeizing".Localized());

            if (Player.BiggestCockArea() > 80)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.Impressive".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.MouthfulOfCock".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.ReleasesOralImprisonment".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.HandsFixateSlickLength".Localized());
            else
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.EagerPumpsNeverSlow".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.LatexWaifTicklesYour".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.Balls".Localized());
            else
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.Taint".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.HappilyExplainsLove".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.GruntNegative".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.SAgain".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.WorseSlimyHandjob".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.SThird".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.YouveBeenGettingMorph".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.SFourth".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.IntoSomethingStrongerStrokes".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.OrgasmSlamsSledgehammer".Localized());

            OutputText("Scenes.Latexgirl.MaleDirectFeedLatexGooGoneWrong.SultryVoiceWhispers".Localized());

            ClearButtons();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -3f));

            AddButton(0, "Common.Button.Next".Localized(), FeedCumDirectEpilogue);
        }

        public void FeedCumDirectEpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FeedCumDirectEpilogue.WakeMouthDrySandpaper".Localized());

            var temp = 50;

            if (Player.CumQ() >= 500)
                temp += 30;

            GooFluid(temp);

            GooHappiness(5);

            GooObedience(-4);

            DoNext(() => ReturnToCamp(4));
        }

        public void FeedLatexyGirlCumDirect()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.ShimmyOutOfArmor".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.GlancesToCocksTut".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.LiftYourBalls".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.ShiftPosition".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.ToShowYourVagina".Localized());
            }
            else
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.SashayShowVagina".Localized());

            if (GooFluid() < 33)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.RumbleTightBellyTremble".Localized());

                if (GooObedience() > 50)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.MasterAffection".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.QuestionSayEnd".Localized());
            }
            else
                if (GooFluid() < 66)
                {
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.SmirkSpreadsVisage".Localized());

                    if (GooObedience() > 50)
                        OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.MasterAffectionAgain".Localized());

                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.SentenceSayEnd".Localized());
                }
                else
                {
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.LazySmileRevealsItself".Localized());

                    if (GooObedience() > 50)
                        OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.MasterAffectionThird".Localized());

                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.SentenceSayEndAgain".Localized());
                }

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.ConfidentlyApproach".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            if (Player.Wetness() >= 3)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.Glistening".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.OuterLipsExcitementDilating".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.NoseCrashesSlit".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.LosesHerself".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.DrizzlingWetnessPoursChin".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.WetnessOnChin".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.LubricantShinesGlossyChin".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.PullingBackHungrySmiles".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.HoldingTightlyThoughtGrind".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.SquirtRiverJuices".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.GraduallyFillMouthLubricants".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.TongueSquishedVaginaClamps".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.PleasureDoesntFadeFast".Localized());

            if (GooObedience() >= 60)
            {
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.UndulatingWhimper".Localized());

                if (GooHappiness() >= 66)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.ThankYouForFood".Localized());
                else
                    if (GooHappiness() >= 33)
                        OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.MuttersThanksForFood".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.ComplaintsReallyThatsIt".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.KnowingNod".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.AndSpatter".Localized(
                        GooCock()
                    ));
                else
                    OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.Spatters".Localized());

                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.OilyWetnessOntoGround".Localized());

                var temp = 10;

                if (Player.Wetness() >= 3)
                    temp += 5;

                if (Player.Wetness() >= 4)
                    temp += 5;

                if (Player.Wetness() >= 5)
                    temp += 5;

                GooFluid(temp);

                GooObedience(4);

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Latexgirl.FeedLatexyGirlCumDirect.EffortsRedouble".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHer".Localized(), LetLatexGooDrainCuntDry);

                AddButton(1, "Common.Button.Try2assert".Localized(), AssertControlOverCuntDrainingLatexGoo);
            }
        }

        public void LetLatexGooDrainCuntDry()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.FaceRemainsStubbornlyAttached".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.WordlesslyWritheOrgasmReleases".Localized());

            OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.GiveVoiceObjections".Localized());

            if (Player.GetClitLength() >= 3)
                OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.HappilyFellatesFemalePole".Localized());
            else
                OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.HappilyLicksSucksSensitive".Localized());

            OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.LuridSquelches".Localized());

            OutputText("Scenes.Latexgirl.LetLatexGooDrainCuntDry.WhimperOfProtest".Localized());

            ClearButtons();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -3f));

            AddButton(0, "Common.Button.Next".Localized(), FeedCumDirectEpilogueGirls);
        }

        public void FeedCumDirectEpilogueGirls()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FeedCumDirectEpilogueGirls.WakeMouthDrySandpaper".Localized());

            GooFluid(50 + Player.Wetness() * 8);

            GooHappiness(4);

            GooObedience(-4);

            DoNext(() => ReturnToCamp(4));
        }

        public void AssertControlOverCuntDrainingLatexGoo()
        {
            ClearOutput();

            if (Player.Strength < 40 || Player.Strength / 10 + Random.Shared.Next(20) + 1 < 9)
            {
                LetLatexGooDrainCuntDry();

                return ;
            }

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.StubbornlyAttached".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.PinnedDownStrugglesLegs".Localized());

            if (GooObedience() <= 50)
                OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.NamePlaceholder".Localized());
            else
                OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.MasterPlaceholder".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.MewlsTentativeLickFolds".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.MoanArchingBackSlithers".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.MumblesSowwyWordsSlurred".Localized(
                Player.Mf("Mashta", "Mishtrish")
            ));

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.ReleaseHeadStrokeHair".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.FallIntoRhythm".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.RollingHeadBack".Localized(
                Player.Mf("Masterrrr", "Mistriiiiissss")
            ));

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.JamFistShrieks".Localized());

            OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.SecondOrgasmPasses".Localized());

            if (GooHappiness() < 33)
                OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.MaybeNotSoBad".Localized());
            else
                if (GooHappiness() < 66)
                    OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.EnjoyServingYou".Localized());
                else
                    OutputText("Scenes.Latexgirl.AssertControlOverCuntDrainingLatexGoo.NuzzlesAgainstYou".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            GooFluid(30 + Player.Wetness() * 5);

            GooHappiness(5);

            GooObedience(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void TryToAssertMaleDomWhileLatexGooDrains()
        {
            ClearOutput();

            if (Player.Strength < 40 || Player.Strength / 10 + Random.Shared.Next(20) + 1 < 9)
            {
                MaleDirectFeedLatexGooGoneWrong();

                return ;
            }

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.SuctionRedoubles".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.NotHowSupposed".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.MewlsMeekly".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.SlickThenPush".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.PauseOnceHiltedBreathing".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.RepeatsBreathChokingGrind".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.GrabHipsSlam".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.PleaseBegsPleadinglyShoulder".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.HungryThenStrike".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.TakeNoMore".Localized());

            OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.SecondOrgasmPasses".Localized());

            if (GooHappiness() < 33)
                OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.MaybeNotSoBad".Localized());
            else
                if (GooHappiness() < 66)
                    OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.EnjoyServingYou".Localized());
                else
                    OutputText("Scenes.Latexgirl.TryToAssertMaleDomWhileLatexGooDrains.NuzzlesAgainstYou".Localized());

            var temp = 30;

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            if (Player.CumQ() >= 500)
                temp += 20;

            GooFluid(temp);

            GooHappiness(5);

            GooObedience(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void FemalePCDomFucksLatexGoo()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.StartToUndress".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.LiftSackAside".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.LiftCocksAside".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.ExposeLabiaEngorgedRosy".Localized());

            if (GooObedience() < 60)
            {
                if (GooFluid() < 33)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.LickingLipsUnrepentantHunger".Localized());
                else
                    if (GooFluid() < 66)
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.StickingTongueAgainstCheek".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.RollingHerEyes".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.LatexynameInterjection".Localized());

                if (GooObedience() < 20)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.BoldlyRetorts".Localized());
                else
                    if (GooObedience() < 40)
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.AnswersSnarkily".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.TimidlyProtests".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.CannotMakeMe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.RigidityOfHerHave".Localized(
                        GooCock()
                    ));
                else
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.Has".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.DoubtingThatClaimResolute".Localized());

                if (GooFluid() < 15)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.HungerPalpable".Localized());

                DoNext(() => ReturnToCamp(1));

                GooObedience(-3);

                return ;
            }

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.GoesOnAllFours".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.RaiseHerGaze".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.SableSlickness".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.MeanwhileSwaysUselesslyAbove".Localized(
                    GooCock()
                ));

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.LostToPleasure".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.SensualShiverRocksWorld".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.KneadingYourChest".Localized());

                if (Player.LactationQ() > 50)
                {
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.AsMilk".Localized());

                    if (Player.LactationQ() < 200)
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.DribblesOverFingers".Localized());
                    else
                        if (Player.LactationQ() < 500)
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.SquirtsOutUnhindered".Localized());
                        else
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.SpraysOutInFountain".Localized());
                }
            }
            else
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.PinchingNipples".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.ArchingBackGrabAnkles".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.SplatsOntoCocks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.PracticallyBurstsDistending".Localized(
                    GooCock()
                ));

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.EssenceBurrowsVaginaClenches".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.MashVaginaSavorAftershocks".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.CoatedWetVagina".Localized());

            if (Player.HasPerk(PerkIds.Masochist))
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.GleefullyStripping".Localized());
            else
            {
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.ObedientCuntCleaner".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.EnthusiasticallyTongueToWork".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LatexGooTimesFemdommedByPc) == 0)
                {
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.WillNotRelent".Localized());

                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.RavenHuedTears".Localized());
                }
                else
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGoo.RememberingLastTime".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LatexGooTimesFemdommedByPc, CoC.Instance.Flags.Get(GameFlagIds.LatexGooTimesFemdommedByPc) + 1);
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            GooFluid(5 + Player.Wetness() * 2);

            GooObedience(5);

            GooHappiness(2);

            DoNext(() => ReturnToCamp(1));
        }

        public void FemalePCDomFucksLatexGooFuta()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.RemoveArmorCurious".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SalaciousWiggle".Localized(
                GooCock()
            ));

            if (GooObedience() < 60)
            {
                if (GooFluid() < 33)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.LickingLipsUnrepentantHunger".Localized());
                else
                    if (GooFluid() < 66)
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.StickingTongueAgainstCheek".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.RollingHerEyes".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.LatexynameInterjection".Localized());

                if (GooObedience() < 20)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.BoldlyRetorts".Localized());
                else
                    if (GooObedience() < 40)
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.AnswersSnarkily".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.TimidlyProtests".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.CannotMakeMe".Localized(
                    GooCock()
                ));

                if (GooFluid() < 10)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.HungerPalpable".Localized());

                GooObedience(-3);

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.ReachesDownAroused".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.InChargeOnTop".Localized());

            if (GooFluid() > 60)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.NicelyPlumped".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.RumpBulgesAdmireCur".Localized(
                GooCock()
            ));

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.LovedMakingThese".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.GentlyPatSnickerFortuitous".Localized(
                GooCock()
            ));

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.AngleUpTipThickens".Localized(
                GooCock()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) * CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) / 6 > Player.VaginalCapacity())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.CantTakeAll".Localized());
            else
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.ErectionNestled".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.ExperimentalBounces".Localized(
                GooCock()
            ));

            if (Player.IsTaur())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.IntoYourHaunches".Localized());
            else
                if (Player.Tallness >= 84)
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.AgainstYourSide".Localized());
                else
                    if (Player.Tallness >= 70)
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.IntoYourShoulder".Localized());
                    else
                        if (Player.Tallness >= 60)
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.IntoYourNeck".Localized());
                        else
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.OverYourHead".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.DeeperInside".Localized());

            Player.CuntChange(CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) * CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) / 6, true, true, false);

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.BounceFaster".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) * CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) / 6 <= Player.VaginalCapacity())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.ClappingOnOnyxThighs".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.TwitchingKnot".Localized(
                GooCock()
            ));

            if (cockType == BodyPartTypes.Dog)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.StaysTrappedThickKnot".Localized());
            else
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SpraysOutOfVagina".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.LookOverShoulder".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.FeelTooGood".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.DecidingForgiveForNow".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.TurnAroundFaceChest".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooNippleType) == -1)
                {
                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.HerNippleCunts".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.KissYours".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SwallowYourNipples".Localized());

                    OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SentencePeriod".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.GooNippleType) == 1)
                    {
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.HerNippleCocks".Localized());

                        if (!Player.HasFuckableNipples())
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SmearOverTits".Localized());
                        else
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.IntoNippleCunts".Localized());

                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SentencePeriodAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.HerNipples".Localized());

                        if (!Player.HasFuckableNipples())
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.RubAllOver".Localized());
                        else
                            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.DockIntoNippleCunts".Localized());

                        OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.SentencePeriodThird".Localized());
                    }
            }
            else
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.FondleAndBounce".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.CouldGetUsedTo".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.MayIKissYou".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.GenerousAwardNod".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.BendingDoubleEquine".Localized());
            else
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.GlossyLipsPlunging".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.PassionateKissesTwitchingBoner".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.ClenchDownMassaging".Localized(
                GooCock()
            ));

            if (!Player.IsTaur())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.AlongWithYourOwn".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.GiveInToEcstasy".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.WhiteEruptsStains".Localized(
                    GooTits()
                ));

            if (Player.HasCock() && Player.CumQ() >= 750)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.BuryStrandsAlabaster".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.HugsTightFinishesUnloading".Localized());

            if (cockType == BodyPartTypes.Dog)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.KnotSlippingFree".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.PresentStainedVagina".Localized());

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.WillNotDo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LatexGooTimesFemdommedByPc) > 0)
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.RewardDeepTongue".Localized());
            else
            {
                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.OrderGoesUnheeded".Localized());

                OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.PunishmentKiss".Localized());
            }

            OutputText("Scenes.Latexgirl.FemalePCDomFucksLatexGooFuta.GetDressedLeaveSwagger".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.LatexGooTimesFemdommedByPc, CoC.Instance.Flags.Get(GameFlagIds.LatexGooTimesFemdommedByPc) + 1);

            GooFluid(5 + Player.Wetness() * 2);

            GooObedience(4);

            GooHappiness(2);

            DoNext(() => ReturnToCamp(1));
        }

        public void MalePCDomFucksLatexGoo()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.OpenArmorExposeCocks".Localized());

            if (GooObedience() > 50)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.MasterAffection".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.QuestionSayEnd".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TakeChinAndTilt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooNippleType) == 1)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CockHyphen".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.HardenOnChest".Localized());

            if (GooObedience() < 60)
            {
                if (GooFluid() < 33)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.LickingLipsUnrepentantHunger".Localized());
                else
                    if (GooFluid() < 66)
                        OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.StickingTongueAgainstCheek".Localized());
                    else
                        OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.RollingHerEyes".Localized());

                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.LatexynameInterjection".Localized());

                if (GooObedience() < 20)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.BoldlyRetorts".Localized());
                else
                    if (GooObedience() < 40)
                        OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.AnswersSnarkily".Localized());
                    else
                        OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TimidlyProtests".Localized());

                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CannotMakeMe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.RigidityOfHerHave".Localized(
                        GooCock()
                    ));
                else
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.Has".Localized());

                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.DoubtingClaim".Localized());

                if (GooFluid() < 10)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.HungerPalpable".Localized());

                GooObedience(-3);

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.BlowingSeductiveKiss".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TakeAllThree".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.Smiling".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SmilingKnowingly".Localized());
                else
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TiredOfHerBlather".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.PullForwardSpreads".Localized());

            if (Player.BiggestCockArea() < 80)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.EasilyEncompassingErection".Localized());
            else
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.StretchingMold".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.InsidesWarmGoo".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.GrabsRemaining".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.S".Localized());

                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SureStrokes".Localized());
            }

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CheeksHollow".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes),

                Player.CockHead()
            ));

            if (Player.BiggestCockArea() >= 10 && Player.BiggestCockArea() < 50)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.AndThroat".Localized());
            else
                if (Player.BiggestCockArea() >= 50)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.ThroatAndBody".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.FasterBounceLoins".Localized());

            if (Player.BiggestCockArea() >= 80)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TakeSoMuchEasily".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.EyesNeverStop".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.PullLipsTight".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SpoogeNostrils".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.RoundsBellyTits".Localized());

            if (Player.CumQ() >= 4000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.PotencyCornersMouth".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.PullOffAndPant".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.LicksAndAdmires".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SackPlaceholder".Localized());
            else
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CockBiggestPlaceholder".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.HoldVeryStill".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SextoyWigglesAss".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.Balls".Localized());
            else
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CockHyphen".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.LockedOnTarget".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TeasesDesiredEffect".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.PullBetweenStrokes".Localized(
                Player.CockHead()
            ));

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.BloatsPregnant".Localized());

            if (Player.CumQ() >= 2000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TorrentAlabasterCunt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.FiresInkyLoads".Localized(
                    GooCock()
                ));

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.RestAgainstBoulder".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.HoldAnyMore".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.LatexGloveSmears".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.ExhaustedBackToBack".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.ComeOnAllReady".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.StaggerUpFromBrace".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SphincterClamps".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.ClaimAnalStar".Localized(
                GooTits()
            ));

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SlapTitDaring".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.HereItComes".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CreamDripsLegs".Localized());

            if (Player.CumQ() >= 2000)
            {
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SpunkForms".Localized());

                if (Player.CumQ() < 4000)
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.Puddle".Localized());
                else
                    OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.Lake".Localized());

                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SentencePeriod".Localized());
            }

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.SlidesIntoPuddle".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.OfMixedFluids".Localized());
            else
                OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.LeftoverSexJuice".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.ButtLiftsWhimpering".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.CockTotallyClean".Localized());

            OutputText("Scenes.Latexgirl.MalePCDomFucksLatexGoo.TiredVoiceThree".Localized());

            var temp = 15;

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            if (Player.CumQ() >= 500)
                temp += 10;

            if (Player.CumQ() >= 1000)
                temp += 10;

            if (Player.CumQ() >= 1500)
                temp += 10;

            if (Player.CumQ() >= 2000)
                temp += 10;

            GooFluid(temp);

            Player.ChangeFatigue(10);

            GooObedience(4);

            GooHappiness(2);

            DoNext(() => ReturnToCamp(1));
        }

        public void FeedLatexySomeMilk()
        {
            ClearOutput();

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.MilkLadenAskHungry".Localized());

            if (GooFluid() >= 66)
                OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.SmileSidlesCloser".Localized());
            else
                if (GooFluid() >= 33)
                    OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.LicksLipsInResponse".Localized());
                else
                    OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.MothToFlame".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.NearbyRockSitting".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.CoolSmoothLips".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.PoppingOffTeat".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.RuffleAndNudge".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.SucklesHandsWanderTantalizing".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.SucklesHandWandersTantalizing".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.HandWandersDown".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.HandsWanderDown".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.OrgasmRising".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.DoubleGenitals".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.Cock".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.Cunt".Localized());
                    else
                        OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.Asshole".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.DontLastLongCum".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.LiberalSpattering".Localized());
            else
                OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.FingerMilksYou".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.TeatPopsFree".Localized());

            OutputText("Scenes.Latexgirl.FeedLatexySomeMilk.DeliciousSmilePlatingKiss".Localized());

            if (Player.HasPerk(PerkIds.Feeder))
            {
                Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
            }

            Player.BoostLactation(.5f);

            GooHappiness(5);

            GooObedience(3);

            var temp = 15;

            if (Player.LactationQ() >= 200)
                temp += 10;

            if (Player.LactationQ() >= 500)
                temp += 10;

            if (Player.LactationQ() >= 1000)
                temp += 10;

            if (Player.LactationQ() >= 2000)
                temp += 10;

            if (Player.LactationQ() >= 4000)
                temp += 10;

            if (Player.LactationQ() >= 8000)
                temp += 10;

            GooFluid(temp);

            Player.ChangeFatigue(5);

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}