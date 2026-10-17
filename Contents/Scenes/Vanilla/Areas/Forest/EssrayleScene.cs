using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<EssrayleScene>("coc:essrayle")]
    public class EssrayleScene : Scene
    {

        public new void PlayerMenu() => ReturnToCamp(1);

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            EssrayleMeetingI();
        }

        public bool ForestEncounterCondition()
        {

            return (int)Player.Gender > 0 && (CoC.Instance.Flags.Get(GameFlagIds.EssyMetInDungeon) == 0 || CoC.Instance.Flags.Get(GameFlagIds.ToldMotherToReleaseEssy) == 1);
        }

        public void EssrayleMeetingI()
        {
            ClearOutput();

            ShowImage("essrayle-encounter");

            if (CoC.Instance.Flags.Get(GameFlagIds.MetEssy) == 0)
            {
                OutputText("Scenes.Essrayle.Meet.BlunderThroughFoliage".Localized());

                OutputText("Scenes.Essrayle.Meet.RealizeNotAlone".Localized());

                OutputText("Scenes.Essrayle.Meet.GazeMeetsEyes".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Essrayle.Meet3Low".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Essrayle.Meet3Mid".Localized());
                    else
                        OutputText("Scenes.Essrayle.Meet3High".Localized());

                OutputText("Scenes.Essrayle.Meet.AttentionOnChest".Localized());

                OutputText("Scenes.Essrayle.Meet.FourGreenArms".Localized());

                OutputText("Scenes.Essrayle.Meet.SheDoesntCoverUp".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Essrayle.Meet6Low".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Essrayle.Meet6Mid".Localized());
                    else
                        OutputText("Scenes.Essrayle.Meet6High".Localized());

                OutputText("Scenes.Essrayle.EssrayleMeetingI.Period".Localized());

                OutputText("Scenes.Essrayle.Meet.HandUnderChin".Localized());

                if (Player.BiggestBreastSize() >= 10)
                    OutputText("Scenes.Essrayle.Meet7Own".Localized());
                else
                    OutputText("Scenes.Essrayle.Meet7Chest".Localized());

                OutputText("Scenes.Essrayle.Meet.AdventurerGuess".Localized());

                OutputText("Scenes.Essrayle.Meet.NodCaptivated".Localized());

                OutputText("Scenes.Essrayle.Meet.SheSeemsPleased".Localized());

                OutputText("Scenes.Essrayle.Meet.NodDistracted".Localized());

                OutputText("Scenes.Essrayle.Meet.StopsBlushing".Localized());

                OutputText("Scenes.Essrayle.Meet.SmilesAsksHerm".Localized());

                OutputText("Scenes.Essrayle.Meet.SaturatesAir".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
            }
            else
            {
                OutputText("Scenes.Essrayle.Again.StumbleUponAgain".Localized());

                OutputText("Scenes.Essrayle.Again.HandsOnShoulders".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.MetEssy, CoC.Instance.Flags.Get(GameFlagIds.MetEssy) + 1);

            AddButton(0, "Common.Button.Yes".Localized(), () => PlantsForMe()); AddButton(1, "No", () => NoPlantsForMe());
        }

        public void NoPlantsForMe()
        {
            ClearOutput();

            OutputText("Scenes.Essrayle.NoPlants".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TurnedDownEssyFirstMeeting, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void PlantsForMe()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AcceptedEssyFirstMeeting) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.AcceptedEssyFirstMeeting, 1);

                OutputText("Scenes.Essrayle.PlantsFirst".Localized());
            }

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Essrayle.PlantsNipples".Localized());
            else
                OutputText("Scenes.Essrayle.PlantsArch".Localized());

            OutputText("Scenes.Essrayle.PlantsRest".Localized());

            OutputText("Scenes.Essrayle.Plants.EssyLaughHug".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe.BreatheSweetAir".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe.HandsSootheYou".Localized());

            OutputText("Scenes.Essrayle.Plants.FlowerOnBack".Localized());

            OutputText("Scenes.Essrayle.Plants.SeparateLickingLips".Localized());

            if (Player.BreastRows[0].BreastRating >= 1)
            {
                OutputText("Scenes.Essrayle.Plants.CoosAtBreasts".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Essrayle.Plants6Multi.OfUppermostPair".Localized());

                OutputText("Scenes.Essrayle.Plants6Nipples".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Essrayle.Plants6Multi.WorkingTheMounds".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe.Period".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Essrayle.Plants6Fuckable".Localized());

                OutputText("Scenes.Essrayle.Plants6Say".Localized());

                ClearButtons();

                AddButton(1, "Common.Button.Yes".Localized(), () => PlantsForMe2(1));

                AddButton(2, "Common.Button.No".Localized(), () => PlantsForMe2(2));
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => PlantsForMe2(0));
            }
        }

        public void PlantsForMe2(int BE = 0)
        {
            ClearOutput();

            var titsOrgasm= false;

            if (BE == 1)
                OutputText("Scenes.Essrayle.PlantsForMe2Yes".Localized());
            else
                if (BE == 2)
                    OutputText("Scenes.Essrayle.PlantsForMe2No".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2Body.UpperHandsWork".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2.DeepthroatAll".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2.DrinkMilk".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Essrayle.PlantsForMe2Herm".Localized());
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Essrayle.PlantsForMe2Cock.CircleAbout".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Essrayle.PlantsForMe2CockOneOf".Localized());

                    OutputText("Scenes.Essrayle.PlantsForMe2Cock.YourShaft".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Essrayle.PlantsForMe2CockPlural".Localized());

                    OutputText("Scenes.Essrayle.PlantsForMe2Cock.SqueezeAndSlide".Localized());
                }
                else
                    OutputText("Scenes.Essrayle.PlantsForMe2Pussy".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2ButtStart".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Essrayle.PlantsForMe2Back".Localized());
            else
                OutputText("Scenes.Essrayle.PlantsForMe2Butt".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2ButtRest".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2Tentacles".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Essrayle.PlantsForMe2TentaclesTail".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2TentaclesLegs".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Essrayle.PlantsForMe2TentaclesLift".Localized());
            else
                OutputText("Scenes.Essrayle.PlantsForMe2TentaclesRear".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2TentaclesRest".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Essrayle.PlantsForMe2OralCock".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2.BobsHead".Localized());

                if (Player.BreastRows[0].BreastRating >= 1)
                    OutputText("Scenes.Essrayle.PlantsForMe2.AndBreasts".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2.CupsPumpSuck".Localized());
            }
            else
                OutputText("Scenes.Essrayle.PlantsForMe2OralPussy".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Essrayle.PlantsForMe2Breasts".Localized());

                if (Player.LactationQ() >= 400)
                    OutputText("Scenes.Essrayle.PlantsForMe2BreastsMilk".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2BreastsEnd".Localized());

                titsOrgasm = (Random.Shared.Next(2) == 0);
            }

            ShowImage("essrayle-discussion");

            OutputText("Scenes.Essrayle.PlantsForMe2TentacleAss".Localized());

            Player.ButtChange(10, true, true, false);

            OutputText("Scenes.Essrayle.PlantsForMe2Climax".Localized());

            if (Player.CumQ() >= 500 && Player.CumQ() < 2000)
                OutputText("Scenes.Essrayle.PlantsForMe2CumMid".Localized());
            else
                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Essrayle.PlantsForMe2CumHigh".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2After.HangLimply".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2After.GrinsSatisfied".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2.CoosCaress".Localized());

            OutputText("Scenes.Essrayle.PlantsForMe2.WavesGoodbye".Localized());

            if (BE == 1)
            {
                OutputText("Scenes.Essrayle.PlantsForMe2Reward.StopsBeforeLeaving".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2Reward.TitsGrindYours".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2Reward.LooksDown".Localized());

                if (Player.TotalNipples() > 2)
                    OutputText("Scenes.Essrayle.PlantsForMe2Reward3Plural".Localized());
                else
                    OutputText("Scenes.Essrayle.PlantsForMe2Reward3Single".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2Reward3End".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2Reward.BreastsExpand".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2Reward.LegsShakeAgain".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2Reward.WeightIncreases".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Essrayle.PlantsForMe2Reward6Multi".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2.FaintFromPleasure".Localized(
                     NumberUtils.Num2Text(Player.BreastRows.Count * 2)
                ));

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Essrayle.PlantsForMe2Reward8Multi".Localized());

                OutputText("Scenes.Essrayle.PlantsForMe2.Period".Localized());

                Player.GrowTits(7, Player.BreastRows.Count, false, 2);

                Player.BoostLactation(Player.BreastRows.Count);

                OutputText("Scenes.Essrayle.PlantsForMe2Reward.SitsAndMilks".Localized());

                titsOrgasm = true;
            }

            if (Player.HasBreasts() && Player.IsLactating())
            {
                Player.Milked();

                Player.BoostLactation(0.01f);
            }

            if (titsOrgasm)
                Player.Orgasm("Tits");
            else
                Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void ApproachTrappedEssy()
        {
            ClearOutput();

            ShowImage("essrayle-encounter");

            if (CoC.Instance.Flags.Get(GameFlagIds.EssyDungeonFucked) == 0)
            {
                OutputText("Scenes.Essrayle.ApproachTrappedEssy.FourBreastsNow".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.SheBoltsUpright".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.AsksWhatHappened".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.Titters".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.StaresAtMelons".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.DeepBreathsDemo".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.MoansInEcstasy".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.EtherealHandsKnead".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.HandsFade".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.SitAndThink".Localized());

                OutputText("Scenes.Essrayle.ApproachTrappedEssy.DecidePlan".Localized());
            }
            else
                OutputText("Scenes.Essrayle.ApproachTrappedEssy.SitsInPot".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            ClearButtons();

            if ((int)Player.Gender > 0)
                AddButton(0, "Common.Button.FeedHer".Localized(), FeedTrappedEssy);

            AddButton(14, "Common.Button.Leave".Localized(), PlayerMenu);
        }

        public void FeedTrappedEssy()
        {
            ClearOutput();

            OutputText("Scenes.Essrayle.FeedTrappedEssy.NotGoingAnywhere".Localized());

            OutputText("Scenes.Essrayle.FeedTrappedEssy.ApproachDisrobed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EssyDungeonFucked, CoC.Instance.Flags.Get(GameFlagIds.EssyDungeonFucked) + 1);

            if (Player.Gender == Gender.Male)
                HasCockFeedEssy(false);

            if (Player.Gender == Gender.Female)
                HasPussyFeedEssy(false);

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Essrayle.FeedTrappedEssy.HowToUseHer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Cock".Localized(), () => HasCockFeedEssy(true));

                AddButton(1, "Common.Button.Pussy".Localized(), () => HasPussyFeedEssy(true));
            }
        }

        public void HasCockFeedEssy(bool newPage = true)
        {
            ClearOutput();

            if (!(newPage))
                OutputText("Scenes.Essrayle.HasCockFeedEssy.ParagraphBreak".Localized());

            ShowImage("essrayle-encounter");

            OutputText("Scenes.Essrayle.HasCockFeedEssy.CockStandsErect".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.LubeWithMilk".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.EyesDull".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.ReluctantToWater".Localized());

            if (Player.BiggestCockLength() < 20)
                OutputText("Scenes.Essrayle.HasCockFeedEssy.LowerUdders".Localized());
            else
                OutputText("Scenes.Essrayle.HasCockFeedEssy.BothBreastSets".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.FasterSlapping".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Essrayle.HasCockFeedEssy.BallsAgainstBelly".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.DrumBeat".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.CoaxesCum".Localized());

            OutputText("Scenes.Essrayle.HasCockFeedEssy.ReclinesExhausted".Localized());

            Player.Orgasm("Dick");

            Scene.PlayerMenu();
        }

        public void HasPussyFeedEssy(bool newPage = true)
        {
            ClearOutput();

            if (!(newPage))
                OutputText("Scenes.Essrayle.HasPussyFeedEssy.ParagraphBreak".Localized());

            ShowImage("essrayle-discussion");

            OutputText("Scenes.Essrayle.HasPussyFeedEssy.PositionsHerTit".Localized());

            if (Player.Vaginas[0].Looseness < 2)
                OutputText("Scenes.Essrayle.HasPussyFeedEssy.GaspingGrunt".Localized());
            else
                if (Player.Vaginas[0].Looseness < 4)
                    OutputText("Scenes.Essrayle.HasPussyFeedEssy.LittleResistance".Localized());
                else
                    OutputText("Scenes.Essrayle.HasPussyFeedEssy.NoResistance".Localized());

            OutputText("Scenes.Essrayle.HasPussyFeedEssy.TakesToHilt".Localized());

            Player.CuntChange(12, true, true);

            OutputText("Scenes.Essrayle.HasPussyFeedEssy.SqueaksAsks".Localized());

            OutputText("Scenes.Essrayle.HasPussyFeedEssy.UpAndDown".Localized());

            OutputText("Scenes.Essrayle.HasPussyFeedEssy.FeverPitch".Localized());

            OutputText("Scenes.Essrayle.HasPussyFeedEssy.ExtricateAndClean".Localized());

            Player.Orgasm("Vaginal");

            if (Player.Fertility < 50)
                Player.Fertility += 1;

            Scene.PlayerMenu();
        }

        public void EssyWitchVictory()
        {
            ClearOutput();

            ShowImage("essrayle-encounter");

            OutputText("Scenes.Essrayle.EssyWitchVictory.FindHerFreed".Localized());

            OutputText("Scenes.Essrayle.EssyWitchVictory.ShakesSouvenirs".Localized());

            OutputText("Scenes.Essrayle.EssyWitchVictory.LeansCloser".Localized());

            OutputText("Scenes.Essrayle.EssyWitchVictory.FingersOnAreola".Localized());

            OutputText("Scenes.Essrayle.EssyWitchVictory.SlySmile".Localized());

            OutputText("Scenes.Essrayle.EssyWitchVictory.CrossesArms".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EssrayleEscapedDungeon, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AcceptEssyPrizes);

            AddButton(1, "Common.Button.No".Localized(), DeclineEssyPrizes);
        }

        public void DeclineEssyPrizes()
        {
            ClearOutput();

            OutputText("Scenes.Essrayle.DeclineEssyPrizes.SighsLetDown".Localized());

            Player.Gems += 100 + Random.Shared.Next(70);

            OutputText("Scenes.Essrayle.DeclineEssyPrizes.BlowsKiss".Localized());

            Scene.PlayerMenu();
        }

        public void AcceptEssyPrizes()
        {
            ClearOutput();

            OutputText("Scenes.Essrayle.AcceptEssyPrizes.BeamsTakesHand".Localized());

            OutputText("Scenes.Essrayle.AcceptEssyPrizes.HurriesOut".Localized());

            OutputText("Scenes.Essrayle.AcceptEssyPrizes.ReversedSpell".Localized());

            ShowImage("essrayle-prize");

            OutputText("Scenes.Essrayle.AcceptEssyPrizes.HeatFalls".Localized());

            if (Player.BiggestBreastSize() <= 1)
            {
                OutputText("Scenes.Essrayle.AcceptEssyPrizes.FourTitsErupt".Localized());

                Player.BreastRows[0].BreastRating = 4;

                if (Player.BreastRows.Count == 1)
                    Player.CreateBreastRow();

                Player.BreastRows[1].BreastRating = 4;
            }
            else
                if (Player.BreastRows.Count == 1)
                {
                    OutputText("Scenes.Essrayle.AcceptEssyPrizes.TwoMoreTits".Localized());

                    if (Player.BreastRows[0].BreastRating < 4)
                    {
                        OutputText("Scenes.Essrayle.AcceptEssyPrizes.TopRowExpands".Localized());

                        Player.BreastRows[0].BreastRating = 4;
                    }

                    if (Player.BreastRows.Count == 1)
                        Player.CreateBreastRow();

                    Player.BreastRows[1].BreastRating = Player.BreastRows[0].BreastRating;
                }
                else
                {
                    OutputText("Scenes.Essrayle.AcceptEssyPrizes.ChestSwells".Localized());

                    Player.GrowTits(6, Player.BreastRows.Count, false, 2);
                }

            Player.BoostLactation(Player.BreastRows.Count);

            OutputText("Scenes.Essrayle.AcceptEssyPrizes.FillingWithWeight".Localized());

            OutputText("Scenes.Essrayle.AcceptEssyPrizes.NodsRollsScroll".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesGimmeGiantNipplesEssy);

            AddButton(1, "Common.Button.No".Localized(), NoGimmeGiantNipplesEssy);
        }

        public void YesGimmeGiantNipplesEssy()
        {
            ClearOutput();

            ShowImage("item-pFruit");

            OutputText("Scenes.Essrayle.YesGimmeGiantNipplesEssy.ProducesFruit".Localized());

            EssyRewardEpilogueOUTTIES();

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.PrFruit), 1);

ReturnToCamp(1);
        }

        public void NoGimmeGiantNipplesEssy()
        {
            ClearOutput();

            OutputText("Scenes.Essrayle.NoGimmeGiantNipplesEssy.ShrugsEh".Localized());

            EssyRewardEpilogueOUTTIES();
        }

        public void EssyRewardEpilogueOUTTIES()
        {

            OutputText("Scenes.Essrayle.EssyRewardEpilogueOutties.SuddenDeepKiss".Localized());

            OutputText("Scenes.Essrayle.EssyRewardEpilogueOutties.KissContinues".Localized());

            OutputText("Scenes.Essrayle.EssyRewardEpilogueOutties.GigglesKisses".Localized());

            Scene.PlayerMenu();
        }

        public void AskMotherToReleaseEssy()
        {
            ClearOutput();

            ShowImage("monster-sandmother");

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesCowed) == 0)
            {
                OutputText("Scenes.Essrayle.AskMotherToReleaseEssy.ExplainsSituation".Localized());

                OutputText("Scenes.Essrayle.AskMotherToReleaseEssy.MotherUnderstands".Localized());

                OutputText("Scenes.Essrayle.AskMotherToReleaseEssy.ThatllHaveToDo".Localized());
            }
            else
            {
                OutputText("Scenes.Essrayle.AskMotherToReleaseEssy.CommandsRelease".Localized());

                OutputText("Scenes.Essrayle.AskMotherToReleaseEssy.ThatllDo".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.ToldMotherToReleaseEssy, 1);

            Scene.PlayerMenu();
        }

    }
}