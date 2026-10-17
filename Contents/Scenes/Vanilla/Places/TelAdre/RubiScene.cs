using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<RubiScene>("coc:rubi")]
    public class RubiScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.TelAdre.Rubi);

        public string debugName => "Rubi";

        public string debugHint => "";

        public override void Entry()
        {
            RubiSprite();
        }

        public void Reset()
        {

            Save.SetBool("hadSex", false);
        }

        public void RubiSprite()
        {

        }

        public void InitializeRubi()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBimbo) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 0);

                CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 5);

                CoC.Instance.Flags.Set(GameFlagIds.RubiNoCunt, 1);
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 7);

                CoC.Instance.Flags.Set(GameFlagIds.RubiNoCunt, 0);

                CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 0);

                CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);

                CoC.Instance.Flags.Set(GameFlagIds.RubiHair, 1);
            }

            CoC.Instance.Flags.Set(GameFlagIds.RubiSetup, 1);
        }

        public string RubiBalls()
        {

            // 原版 rubiBalls() 是三个连续 return，永远只返回 normal；这里按 RubiBallsType 的实际语义修正
            return CoC.Instance.Flags.Get(GameFlagIds.RubiBallsType) switch
            {
                1 => "swollen",

                2 => "massively engorged",

                _ => "normal",
            };
        }

        public bool RubiBimbo()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.RubiBimbo) != 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiDebimbo) == 0;
        }

        public bool RubiIncubus()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.RubiIncubusProgress) == 3;
        }

        public int RubiAffection(int delt = 0)
        {

            if (delt == 0)
                return CoC.Instance.Flags.Get(GameFlagIds.RubiAffection);

            CoC.Instance.Flags.Set(GameFlagIds.RubiAffection, CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) + delt);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.RubiAffection, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.RubiAffection, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.RubiAffection);
        }

        public string RubiCock(int lust = 50)
        {

            var cumQ = 300;

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 4)
                cumQ = 10;
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 6)
                    cumQ = 150;

            return Appearance.CockDescription(RubiGetCockType(), CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize), CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) / 6, lust, cumQ);
        }

        public Identifier RubiGetCockType()
        {

            return BodyPartTypes.ParseConstantByIndex(CoC.Instance.Flags.Get(GameFlagIds.RubiCockType));
        }

        public string RubiBreasts()
        {

            var ret = "pair of ";

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                ret = "chest";
            else
            {
                ret += Player.BreastCupForRating(CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize));

                ret += " ";

                ret += Appearance.BreastDescript(CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize), 0);
            }

            return ret;
        }

        public string RubiChest()
        {

            return RubiBreasts();
        }

        public float RubiCapacity()
        {

            int bonus = CoC.Instance.Flags.Get(GameFlagIds.RubiTimesAnallyTrained) * 10;

            if (bonus > 76)
                bonus = 76;

            if (hyper)
                bonus += 200;

            return 24 + bonus;
        }

        public string RubiMF(string man, string woman)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAdmittedGender) < 1)
                return woman;
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiShe) == 1)
                    return woman;
                else
                    return man;
        }

        public void SpecialRelationship20scene()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.SpecialRelationship20scene.ChasingAfterYou".Localized());

            OutputText("Scenes.Rubi.SpecialRelationship20scene.ApologeticDeepBreath".Localized());

            OutputText("Scenes.Rubi.SpecialRelationship20scene.AskAboutDemonHeritage".Localized());

            OutputText("Scenes.Rubi.SpecialRelationship20scene.HesitantRubyLips".Localized());

            OutputText("Scenes.Rubi.SpecialRelationship20scene.RelievedNodConfession".Localized());

            OutputText("Scenes.Rubi.SpecialRelationship20scene.SurprisedYetConvinced".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiAdmittedGender, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptRubi);

            AddButton(1, "Common.Button.Reject".Localized(), RejectRubi);
        }

        public void AcceptRubi()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.AcceptRubi.AcceptingHug".Localized());

            OutputText("Scenes.Rubi.AcceptRubi.WipingTearSmile".Localized());

            OutputText("Scenes.Rubi.AcceptRubi.HowCouldAnyoneHate".Localized());

            OutputText("Scenes.Rubi.AcceptRubi.KeepSecretQuiet".Localized());

            OutputText("Scenes.Rubi.AcceptRubi.FarewellHug".Localized());

            RubiAffection(3);

            ReturnToCamp(1);
        }

        public void RejectRubi()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.RejectRubi.HoldingAtArmsLength".Localized());

            OutputText("Scenes.Rubi.RejectRubi.TearingUpBraveFace".Localized());

            OutputText("Scenes.Rubi.RejectRubi.SnifflingDeparture".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiDisabled, 1);

            ReturnToCamp(1);
        }

        public Action RubiIntros()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSetup) == 0)
                InitializeRubi();

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiDisabled) > 0 || CoC.Instance.Flags.Get(GameFlagIds.RubiProstitution) == -2)
                return null;

            OutputText("Scenes.Rubi.RubiIntros.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiIntroduced) == 0)
                OutputText("Scenes.Rubi.RubiIntros.HumanGirlWaitress".Localized());
            else
                if (RubiAffection() < 30)
                    OutputText("Scenes.Rubi.RubiIntros.RubiServingTables".Localized());
                else
                    if (RubiBimbo() && CoC.Instance.Flags.Get(GameFlagIds.RubiProstitution) == 0 && Random.Shared.Next(9) == 0)
                    {
                        OutputText("Scenes.Rubi.RubiIntros.RubiMissingShift".Localized());

                        return CheatingRubi;
                    }
                    else
                        if (RubiBimbo())
                            OutputText("Scenes.Rubi.RubiIntros.RubiEnthusiasticServing".Localized());
                        else
                            OutputText("Scenes.Rubi.RubiIntros.RubiServingInOutfit".Localized());

            OutputText("Scenes.Rubi.RubiIntros.ParagraphBreakAgain".Localized());

            return ApproachRubiScenes;
        }

        public void ApproachRubiScenes()
        {
            ClearOutput();

            ShowImage("rubi-at-cafe");

            RubiSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiIntroduced) == 0)
            {
                OutputText("Scenes.Rubi.ApproachRubiScenes.GirlAtYourSide".Localized());

                OutputText("Scenes.Rubi.ApproachRubiScenes.MenuRecommendation".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiIntroduced, CoC.Instance.Flags.Get(GameFlagIds.RubiIntroduced) + 1);
            }
            else
                if (RubiAffection() < 30)
                    OutputText("Scenes.Rubi.ApproachRubiScenes.RubiPullsUpChair".Localized());
                else
                {
                    var choices = new List<int> { 1 };

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSuitclothes) == 1)
                        choices.Add(2);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiFetishClothes) == 1)
                        choices.Add(3);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiGreenAdventurer) == 1)
                        choices.Add(4);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiTubeTop) == 1)
                        choices.Add(5);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBodysuit) == 1)
                        choices.Add(6);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiLongdress) == 1)
                        choices.Add(7);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiTightPants) == 1)
                        choices.Add(8);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNurseClothes) == 1)
                        choices.Add(9);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSwimwear) == 1)
                        choices.Add(10);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBimboMinidress) == 1)
                        choices.Add(11);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBondageStraps) == 1)
                        choices.Add(12);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiInquisitorsCorset) == 1)
                        choices.Add(13);

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 100)
                        choices.Add(14);

                    int select = choices[Random.Shared.Next(choices.Count)];

                    if (select == 1)
                    {
                        OutputText("Scenes.Rubi.ApproachRubiScenes.UsualWaitressUniform".Localized(
                            RubiMF("He", "She")
                        ));

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                            OutputText("Scenes.Rubi.ApproachRubiScenes.StrainingBlouse".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) >= 9)
                            OutputText("Scenes.Rubi.ApproachRubiScenes.AwkwardHiddenBulge".Localized());
                    }
                    else
                        if (select == 2)
                        {
                            OutputText("Scenes.Rubi.ApproachRubiScenes.ProfessionalSuitOutfit".Localized(
                                RubiMF("He", "She")
                            ));

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiHorntype) > 0)
                                OutputText("Scenes.Rubi.ApproachRubiScenes.BehindTheHorns".Localized());

                            OutputText("Scenes.Rubi.ApproachRubiScenes.BoyfriendClothesLook".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                OutputText("Scenes.Rubi.ApproachRubiScenes.LargeBosomUnbuttoned".Localized());

                            OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriod".Localized());
                        }
                        else
                            if (select == 3)
                            {
                                OutputText("Scenes.Rubi.ApproachRubiScenes.BlackRubberOutfit".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNippleType) > 0)
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.BlackWord".Localized());

                                OutputText("Scenes.Rubi.ApproachRubiScenes.CrisscrossRubberStraps".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.RubberDollLook".Localized());
                                else
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.FlatChestFigure".Localized());
                            }
                            else
                                if (select == 5)
                                {
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.NeonPinkTubeTop".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                        OutputText("Scenes.Rubi.ApproachRubiScenes.StrainingTubeTop".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) >= 4)
                                        OutputText("Scenes.Rubi.ApproachRubiScenes.UnconcealedLargeBulge".Localized());
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                            OutputText("Scenes.Rubi.ApproachRubiScenes.WellConcealedBulge".Localized());
                                }
                                else
                                    if (select == 6)
                                    {
                                        OutputText("Scenes.Rubi.ApproachRubiScenes.SheerBodysuitOutfit".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) >= 9)
                                            OutputText("Scenes.Rubi.ApproachRubiScenes.TightOutfitHidesNothing".Localized());
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.TightOutfitSlightOutline".Localized());
                                    }
                                    else
                                        if (select == 7)
                                        {
                                            OutputText("Scenes.Rubi.ApproachRubiScenes.SequinedBallGown".Localized());

                                            if (RubiBimbo())
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.FalseGemsLegSlit".Localized());

                                            OutputText("Scenes.Rubi.ApproachRubiScenes.ClassyAndCarnal".Localized());

                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) <= 1)
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.PaddedBraSurprise".Localized());
                                        }
                                        else
                                            if (select == 8)
                                            {
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.LooseSilkShirt".Localized());

                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.BustyPiratessLook".Localized());
                                                else
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.FlatChestPirateBoy".Localized());

                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) >= 9)
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.DisplayedGenerousCock".Localized());
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.SmallPackageSecret".Localized());
                                            }
                                            else
                                                if (select == 9)
                                                {
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.WhiteOnePieceDress".Localized());

                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.LargeBreastsDressStrain".Localized());
                                                }
                                                else
                                                    if (select == 10)
                                                    {
                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.SkimpyBikiniOutfit".Localized());

                                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.TremendousCleavageSpill".Localized());
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) <= 1)
                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.TopEmphasizesFlatChest".Localized());
                                                    }
                                                    else
                                                        if (select == 11)
                                                        {
                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.ImpossiblyShortSkirt".Localized());

                                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.SkirtRidesHigh".Localized());
                                                        }
                                                        else
                                                            if (select == 12)
                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.LeatherStrapOutfit".Localized());
                                                            else
                                                                if (select == 13)
                                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.RedGoldCorset".Localized());
                                                                else
                                                                    if (select == 14)
                                                                    {
                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.ExtremelyModifiedOutfit".Localized());

                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.FrillyPinkCocksock".Localized(
                                                                                RubiCock(33)
                                                                            ));

                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.AndWord".Localized());

                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                                                                        {
                                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.MoundBeneathPanties".Localized());

                                                                            if (RubiBimbo())
                                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.TrimmedMonsExposed".Localized());
                                                                        }

                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriodAgain".Localized());

                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.SideboobDisplay".Localized());

                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.RisqueOutfitForYou".Localized());
                                                                    }

                    OutputText("Scenes.Rubi.ApproachRubiScenes.HandsMenuSayStart".Localized());

                    if (RubiBimbo())
                        OutputText("Scenes.Rubi.ApproachRubiScenes.LikeWordPrefix".Localized());
                    else
                        OutputText("Scenes.Rubi.ApproachRubiScenes.HLetter".Localized());

                    OutputText("Scenes.Rubi.ApproachRubiScenes.MenuBabeSayEnd".Localized());

                    if (select == 4)
                    {
                        OutputText("Scenes.Rubi.ApproachRubiScenes.GreenTunicOutfit".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) >= 4)
                            OutputText("Scenes.Rubi.ApproachRubiScenes.CursedAdventurerLook".Localized());
                    }

                    if (select == 1)
                    {
                        OutputText("Scenes.Rubi.ApproachRubiScenes.LookUpSkirt".Localized());

                        if (RubiBimbo())
                        {
                            OutputText("Scenes.Rubi.ApproachRubiScenes.NothingUnderneathAsshole".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                                OutputText("Scenes.Rubi.ApproachRubiScenes.AndWetCuntFifth".Localized());
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.AndLimpCockFifth".Localized());
                                else
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.CuntLimpCock".Localized());

                            OutputText("Scenes.Rubi.ApproachRubiScenes.AnyoneBehindEm".Localized());
                        }
                        else
                            OutputText("Scenes.Rubi.ApproachRubiScenes.FrillyPinkPantiesView".Localized());

                        OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriodThird".Localized());
                    }
                    else
                        if (select == 8)
                            OutputText("Scenes.Rubi.ApproachRubiScenes.LeatherPantsAssView".Localized());
                        else
                            if (select == 15)
                            {
                                OutputText("Scenes.Rubi.ApproachRubiScenes.TunicAssView".Localized());

                                if (RubiBimbo())
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.WetCrotchPatch".Localized());
                            }
                            else
                                if (select == 7)
                                {
                                    OutputText("Scenes.Rubi.ApproachRubiScenes.SwayingLikeDancing".Localized());

                                    if (RubiBimbo())
                                        OutputText("Scenes.Rubi.ApproachRubiScenes.ShakingAssSpanking".Localized());
                                }
                                else
                                    if (select == 9)
                                    {
                                        OutputText("Scenes.Rubi.ApproachRubiScenes.LookUpSkirtAgain".Localized());

                                        if (RubiBimbo())
                                        {
                                            OutputText("Scenes.Rubi.ApproachRubiScenes.NothingUnderneathAssholeAgain".Localized());

                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.AndWetCuntAgain".Localized());
                                            else
                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.AndLimpCockAgain".Localized());
                                                else
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.CuntLimpCockAgain".Localized());

                                            OutputText("Scenes.Rubi.ApproachRubiScenes.AnyoneBehindEmAgain".Localized());
                                        }
                                        else
                                            OutputText("Scenes.Rubi.ApproachRubiScenes.FrillyWhitePantiesView".Localized());

                                        OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriodFourth".Localized());
                                    }
                                    else
                                        if (select == 3)
                                        {
                                            OutputText("Scenes.Rubi.ApproachRubiScenes.LookUpSkirtThird".Localized());

                                            if (RubiBimbo())
                                            {
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.NothingUnderneathAssholeThird".Localized());

                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.AndWetCuntThird".Localized());
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.AndLimpCockThird".Localized());
                                                    else
                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.CuntLimpCockThird".Localized());

                                                OutputText("Scenes.Rubi.ApproachRubiScenes.AnyoneBehindEmThird".Localized());
                                            }
                                            else
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.TranslucentRubberPanties".Localized());

                                            OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriodFifth".Localized());
                                        }
                                        else
                                            if (select == 2)
                                                OutputText("Scenes.Rubi.ApproachRubiScenes.UnflatteringCutAssView".Localized());
                                            else
                                                if (select == 5)
                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.DenimShortsAssView".Localized());
                                                else
                                                    if (select == 10)
                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.BikiniBottomsAssView".Localized());
                                                    else
                                                        if (select == 11)
                                                        {
                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.LookUpDress".Localized());

                                                            if (RubiBimbo())
                                                            {
                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.NothingUnderneathAssholeFourth".Localized());

                                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.AndWetCuntFourth".Localized());
                                                                else
                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.AndLimpCockFourth".Localized());
                                                                    else
                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.CuntLimpCockFourth".Localized());

                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.AnyoneBehindEmFourth".Localized());
                                                            }
                                                            else
                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.LittleThongView".Localized());

                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriodSixth".Localized());
                                                        }
                                                        else
                                                            if (select == 6)
                                                            {
                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.LaceBodysuitAssView".Localized());

                                                                if (RubiBimbo())
                                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.MoistCrotchPatch".Localized());
                                                            }
                                                            else
                                                                if (select == 12)
                                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.LeatherStrapAssView".Localized());
                                                                else
                                                                    if (select == 13)
                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.GoldLacedAssView".Localized());
                                                                    else
                                                                        if (select == 14)
                                                                        {
                                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.SadExcuseSkirtView".Localized());

                                                                            if (RubiBimbo())
                                                                            {
                                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.NothingUnderneathAssholeFifth".Localized());

                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                                                                                    OutputText("Scenes.Rubi.ApproachRubiScenes.AndWetCuntFifth".Localized());
                                                                                else
                                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.AndLimpCockFifth".Localized());
                                                                                    else
                                                                                        OutputText("Scenes.Rubi.ApproachRubiScenes.CuntLimpCockFifth".Localized());

                                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.AnyoneBehindEmFifth".Localized());
                                                                            }
                                                                            else
                                                                                OutputText("Scenes.Rubi.ApproachRubiScenes.PinkFrillyPantiesView".Localized());

                                                                            OutputText("Scenes.Rubi.ApproachRubiScenes.SentencePeriodSeventh".Localized());
                                                                        }
                }

            OutputText("Scenes.Rubi.ApproachRubiScenes.LookOverMenu".Localized());

            ClearButtons();

            if (Player.Gems >= 3)
                AddButton(0, "Common.Button.Milk".Localized(), BuyRubiMilk);
            else
                AddButton(0, "Common.Button.Milk".Localized(), null);

            if (Player.Gems >= 6)
                AddButton(1, "Common.Button.Tea".Localized(), GetTeaFromRubi);
            else
                AddButton(1, "Common.Button.Tea".Localized(), null);

            AddButton(2, "Common.Button.Chat".Localized(), ChatWithRubi);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAdmittedGender) >= 1)
                AddButton(3, "Common.Button.RubisPlace".Localized(), RubisFuckingHouseYouPervert);
            else
                AddButton(3, "Common.Button.RubisPlace".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), SceneInstance<BakeryScene>.Get().Bakeryuuuuuu);
        }

        public void BuyRubiMilk()
        {
            ClearOutput();

            RubiSprite();

            if (RubiAffection() < 30)
            {
                OutputText("Scenes.Rubi.BuyRubiMilk.MilkOrderMacroGlass".Localized());

                OutputText("Scenes.Rubi.BuyRubiMilk.SipGlassesKnowRubi".Localized());
            }
            else
                if (RubiBimbo())
                {
                    OutputText("Scenes.Rubi.BuyRubiMilk.MilkOrderPlainGlass".Localized());

                    OutputText("Scenes.Rubi.BuyRubiMilk.SipLapTerribleJokes".Localized());
                }
                else
                {
                    OutputText("Scenes.Rubi.BuyRubiMilk.MilkOrderMissingArticle".Localized());

                    OutputText("Scenes.Rubi.BuyRubiMilk.SipGlassesSharedLaugh".Localized());
                }

            Player.Gems -= 3;

            RubiAffection(3);

            Player.RefillHunger(7);

            Player.ModThickness(100, 1);

            if (RubiAffection() >= 30 && CoC.Instance.Flags.Get(GameFlagIds.RubiAdmittedGender) == 0)
                SpecialRelationship20scene();
            else
                ReturnToCamp(1);
        }

        public void GetTeaFromRubi()
        {
            ClearOutput();

            RubiSprite();

            if (RubiAffection() < 30)
            {
                OutputText("Scenes.Rubi.GetTeaFromRubi.OrderTeaRelaxingBlend".Localized());

                OutputText("Scenes.Rubi.GetTeaFromRubi.TeaDriftChatCheck".Localized());
            }
            else
                if (RubiBimbo())
                {
                    OutputText("Scenes.Rubi.GetTeaFromRubi.OrderTeaCasualBlend".Localized());

                    OutputText("Scenes.Rubi.GetTeaFromRubi.TeaDriftGossipCheck".Localized());
                }
                else
                {
                    OutputText("Scenes.Rubi.GetTeaFromRubi.OrderTeaMacroBlend".Localized());

                    OutputText("Scenes.Rubi.GetTeaFromRubi.TeaDriftMacroCheck".Localized());
                }

            Player.Gems -= 6;

            RubiAffection(5);

            Player.RefillHunger(7);

            Player.ChangeFatigue(-25);

            if (RubiAffection() >= 30 && CoC.Instance.Flags.Get(GameFlagIds.RubiAdmittedGender) == 0)
                SpecialRelationship20scene();
            else
                ReturnToCamp(1);
        }

        public void ChatWithRubi()
        {
            ClearOutput();

            RubiSprite();

            RubiAffection(2);

            if (RubiAffection() < 15)
            {
                OutputText("Scenes.Rubi.ChatWithRubi.WaitressHumanVillageTale".Localized());

                OutputText("Scenes.Rubi.ChatWithRubi.AttendOtherTables".Localized());
            }
            else
                if (RubiAffection() < 30)
                {
                    RubiAffection(8);

                    OutputText("Scenes.Rubi.ChatWithRubi.LifeOutsideTelAdre".Localized());

                    OutputText("Scenes.Rubi.ChatWithRubi.ExcuseSelfHeadCamp".Localized());
                }
                else
                    if (RubiAffection() < 100)
                    {
                        OutputText("Scenes.Rubi.ChatWithRubi.ChatAdoptedFerrets".Localized());

                        OutputText("Scenes.Rubi.ChatWithRubi.CheekKissDeparture".Localized());
                    }
                    else
                        if (RubiBimbo())
                        {
                            OutputText("Scenes.Rubi.ChatWithRubi.FashionMovementsTalk".Localized());

                            OutputText("Scenes.Rubi.ChatWithRubi.PassionateLipsKiss".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Rubi.ChatWithRubi.OldVillageTalk".Localized());

                            OutputText("Scenes.Rubi.ChatWithRubi.LipsKissDeparture".Localized());
                        }

            if (RubiAffection() >= 30 && CoC.Instance.Flags.Get(GameFlagIds.RubiAdmittedGender) == 0)
                SpecialRelationship20scene();
            else
                ReturnToCamp(1);
        }

        public void RubisFuckingHouseYouPervert()
        {
            ClearOutput();

            ShowImage("rubi-at-house");

            RubiSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.RubisHouseFirstTime) == 0)
            {
                OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.WhisperInviteHePronoun".Localized());

                OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.BlushTailSwishExcited".Localized());

                OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.ClutchArmLeadHome".Localized());

                OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.RelinquishGripWelcome".Localized());

                OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.TwoWineGlassesReturn".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubisHouseFirstTime, CoC.Instance.Flags.Get(GameFlagIds.RubisHouseFirstTime) + 1);
            }
            else
                if (RubiBimbo() && (Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties)) && Random.Shared.Next(7) == 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.RubiBimboMinidress, 1);

                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.OfferLeaveTogether".Localized());

                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.CheshireGrinDiaCover".Localized());

                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.BlowJointHeadOff".Localized());

                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.BrowseFabricDisplays".Localized());

                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.RubiOutfitPurchase".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiGotBimboSkirt) == 0)
                    {
                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.PinkPleatedSkirtSpot".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.RubiBuysSkirtTreat".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.BackHomeMissedChance".Localized());

                        Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BimboOutfit), 1);

                        ReturnToCamp(1);

                        return ;
                    }
                    else
                    {
                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.LeaveShopUnfulfilled".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.CampMissedChance".Localized());
                    }
                }
                else
                    if (RubiBimbo())
                    {
                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.OfferLeaveTogetherAgain".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.CheshireGrinDiaAgain".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.SettleInHerPlace".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiProstitution) == 2 && CoC.Instance.Flags.Get(GameFlagIds.RubiProfit) > 0)
                        {
                            OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.LockboxSavingsRetrieve".Localized());

                            Player.Gems += CoC.Instance.Flags.Get(GameFlagIds.RubiProfit);

                            CoC.Instance.Flags.Set(GameFlagIds.RubiProfit, 0);

                        }

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.WhispersAskDesire".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.OfferLeaveMacroPronoun".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.DemonTailSwishQuestion".Localized(
                            RubiMF("boy", "girl")
                        ));

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.WhisperHeadHerPlace".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.ReturnToLivingRoom".Localized());

                        OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.CouchStretchInvitation".Localized());
                    }

            ClearButtons();

            AddButton(0, "Common.Button.Sex".Localized(), RubiSexMenu);

            AddButton(1, "Common.Button.Closet".Localized(), GoInRubisClosetSoThatYouCanComeOutOfTheCloset);

            AddButton(2, "Common.Button.Talk".Localized(), TalkToRubiInHouse);

            if (RubiAffection() >= 50 && !RubiBimbo())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesRubiMassaged) == 0)
                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.BottlesBakeryTenseOffer".Localized());
                else
                    OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.MassageWinkOffer".Localized());

                AddButton(4, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), ChocoRubiMassage);
            }

            if (Player.IsNaga() && CoC.Instance.Flags.Get(GameFlagIds.RubiBimbo) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < Player.BiggestCockLength() && Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.RubiShe) == 0 && Player.IsCorruptEnough(85) && CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) <= 2)
            {
                OutputText("Scenes.Rubi.RubisFuckingHouseYouPervert.HypnotizeRubiSnake".Localized());

                AddButton(5, "Common.Button.Hypno".Localized(), HypnoBimboficationForRubiSloots);
            }

            AddButton(14, "Common.Button.Leave".Localized(), RubiLeave);
        }

        public void RubiLeave()
        {
            ClearOutput();

            OutputText("Scenes.Rubi.RubiLeave.ExplainLeavingQuickHug".Localized());

            OutputText("Scenes.Rubi.RubiLeave.SeesYouOffShouting".Localized());

            ReturnToCamp(1);
        }

        public void RubiSexMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Fuck".Localized(), null);

            AddButton(1, "Common.Button.DildoFuck".Localized(), null);

            AddButton(3, "Scenes.Faerie.SexMenu.ButtonRelease".Localized(), null);

            AddButton(4, "Common.Button.Train".Localized(), null);

            AddButton(5, "Common.Button.GetFucked".Localized(), null);

            AddButton(6, "Common.Button.Hotdogging".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock() || (Player.HasVagina() && Player.GetClitLength() >= 4))
                    AddButton(0, "Common.Button.Fuck".Localized(), FuckRubi);

                if (Player.HasKeyItem("Deluxe Dildo"))
                    AddButton(1, "Common.Button.DildoFuck".Localized(), DildoFuckRubi);
            }

            AddButton(2, "Common.Button.Tease".Localized(), TeaseRubi);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0 && !RubiBimbo() && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                AddButton(3, "Scenes.Faerie.SexMenu.ButtonRelease".Localized(), ReleaseRubi);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 75 && RubiCapacity() < Player.BiggestCockArea() && Player.HasCock())
                AddButton(4, "Common.Button.Train".Localized(), AnallyTrainYourBitchBoySlutHowToBeAnalForDCL);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 75 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) >= 5)
                AddButton(5, "Common.Button.GetFucked".Localized(), GetFuckedByRubi);

            if (Player.HasCock())
                AddButton(6, "Common.Button.Hotdogging".Localized(), RubiHotdogging);

            AddButton(14, "Common.Button.Back".Localized(), RubisFuckingHouseYouPervert);
        }

        public void TalkToRubiInHouse()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.TalkToRubiInHouse.ChooseTalkTopic".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Identity".Localized(), RubiIdentity);

            AddButton(14, "Common.Button.Back".Localized(), () => RubisFuckingHouseYouPervert());
        }

        public void RubiIdentity()
        {
            ClearOutput();

            RubiSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesDiscussedRubisIdentity) == 0)
            {
                OutputText("Scenes.Rubi.RubiIdentity.NoticedHeSelfReference".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.RubiIdentity.HisAttemptsOtherwise".Localized());
                else
                    OutputText("Scenes.Rubi.RubiIdentity.RecentTransformationContrary".Localized());

                OutputText("Scenes.Rubi.RubiIdentity.BlushWhenQuestioned".Localized());

                OutputText("Scenes.Rubi.RubiIdentity.NeverFeltLikeGirl".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                    OutputText("Scenes.Rubi.RubiIdentity.AbsolutelyLoveIt".Localized());

                OutputText("Scenes.Rubi.RubiIdentity.ConfideFidgetingSheOk".Localized());

                OutputText("Scenes.Rubi.RubiIdentity.ChoosePronounType".Localized());
            }
            else
            {
                OutputText("Scenes.Rubi.RubiIdentity.DiscussGenderAtLength".Localized());

                OutputText("Scenes.Rubi.RubiIdentity.ChoosePronounTypeAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesDiscussedRubisIdentity, CoC.Instance.Flags.Get(GameFlagIds.TimesDiscussedRubisIdentity) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.He".Localized(), RubiIsAHe);

            AddButton(1, "Common.Button.She".Localized(), RubiIsAShe);

            AddButton(14, "Common.Button.Back".Localized(), () => TalkToRubiInHouse());
        }

        public void RubiIsAHe()
        {
            ClearOutput();

            RubiSprite();

            CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 0);

            OutputText("Scenes.Rubi.RubiIsAHe.ConfirmHePronouns".Localized());

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), RubisFuckingHouseYouPervert);
        }

        public void RubiIsAShe()
        {
            ClearOutput();

            RubiSprite();

            CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);

            OutputText("Scenes.Rubi.RubiIsAShe.ConfirmShePronouns".Localized());

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), RubisFuckingHouseYouPervert);
        }

        public void FuckRubi()
        {
            ClearOutput();

            ShowImage("rubi-fuck");

            int x = 0;

            x = Player.CockThatFits(RubiCapacity());

            if (x < 0)
                x = Player.BiggestCockIndex();

            RubiSprite();

            OutputText("Scenes.Rubi.FuckRubi.LeadRubiToBedroom".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.FuckRubi.UndressRubi".Localized(
                RubiMF("He giggles and reaches between his legs", "She giggles and reaches between her legs")
            ));

            if (Player.HasCock())
                OutputText(Player.CockDescript(x));
            else
                OutputText(Player.ClitDescript());

            OutputText("Scenes.Rubi.FuckRubi.SentencePeriod".Localized());

            var size = 0;

            var cock = Player.HasCock();

            if (Player.HasCock())
            {
                if (Player.Cocks[x].Length <= 4)
                    size = 0;
                else
                    if (Player.HasCockThatFits(RubiCapacity()))
                        size = 1;
                    else
                        size = 2;
            }
            else
                if (Player.GetClitLength() <= 4)
                    size = 0;
                else
                    if (Player.GetClitLength() <= 12 || hyper)
                        size = 1;
                    else
                        size = 2;

            if (size == 0)
            {
                OutputText("Scenes.Rubi.FuckRubi.RubiTeasesPenisSize".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) <= 5)
                    OutputText("Scenes.Rubi.FuckRubi.SameBoatRemark".Localized());
                else
                    if (RubiBimbo())
                        OutputText("Scenes.Rubi.FuckRubi.LamentsNoRealSex".Localized());
                    else
                        OutputText("Scenes.Rubi.FuckRubi.MocksBeingOutdone".Localized());
            }
            else
                if (size == 1)
                    OutputText("Scenes.Rubi.FuckRubi.RubiAdmiresStud".Localized());
                else
                    OutputText("Scenes.Rubi.FuckRubi.SizeShockAlternatives".Localized());

            OutputText("Scenes.Rubi.FuckRubi.ReleasesHold".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.FuckRubi.CockWord".Localized());
            else
                OutputText("Scenes.Rubi.FuckRubi.ClitWord".Localized());

            OutputText("Scenes.Rubi.FuckRubi.RubiKneelsOnFloor".Localized());

            if (Player.HasCock())
                OutputText(Player.CockDescript(x));
            else
                OutputText("Scenes.Rubi.FuckRubi.ClitToken".Localized());

            OutputText("Scenes.Rubi.FuckRubi.LeansInTentativeLick".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.FuckRubi.SwollenCockNoun".Localized());
            else
                OutputText("Scenes.Rubi.FuckRubi.ImpressiveClitNoun".Localized());

            OutputText("Scenes.Rubi.FuckRubi.RubiLicksShaft".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.FuckRubi.CockHeadNoun".Localized());
            else
                OutputText("Scenes.Rubi.FuckRubi.ClitTipNoun".Localized());

            OutputText("Scenes.Rubi.FuckRubi.PleasureShivers".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.FuckRubi.CloseToRelease".Localized());

            if (size < 2)
            {
                OutputText("Scenes.Rubi.FuckRubi.PreparesPenetration".Localized());

                if (Player.HasCock())
                    OutputText(Player.CockDescript(x));
                else
                    OutputText("Scenes.Rubi.FuckRubi.ClitTokenAgain".Localized());

                OutputText("Scenes.Rubi.FuckRubi.TightEntryPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                    OutputText("Scenes.Rubi.FuckRubi.TailHoleWord".Localized());
                else
                    OutputText("Scenes.Rubi.FuckRubi.PussyWord".Localized());

                OutputText("Scenes.Rubi.FuckRubi.SpreadsSalivaLube".Localized());

                if (RubiBimbo())
                    OutputText("Scenes.Rubi.FuckRubi.AlreadyDrippingWet".Localized());

                OutputText("Scenes.Rubi.FuckRubi.SentencePeriodAgain".Localized());

                OutputText("Scenes.Rubi.FuckRubi.SinksFullLength".Localized());
            }
            else
            {
                OutputText("Scenes.Rubi.FuckRubi.PreparesThighSex".Localized());

                if (Player.HasCock())
                    OutputText(Player.CockDescript(x));
                else
                    OutputText("Scenes.Rubi.FuckRubi.ClitTokenThird".Localized());

                OutputText("Scenes.Rubi.FuckRubi.BelowCrotchPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                    OutputText("Scenes.Rubi.FuckRubi.BallsWord".Localized());
                else
                    OutputText("Scenes.Rubi.FuckRubi.CuntWord".Localized());

                OutputText("Scenes.Rubi.FuckRubi.SpreadsSalivaOnLegs".Localized());

                if (RubiBimbo())
                    OutputText("Scenes.Rubi.FuckRubi.DrippingNeedsNoLube".Localized());

                OutputText("Scenes.Rubi.FuckRubi.SentencePeriodThird".Localized());

                OutputText("Scenes.Rubi.FuckRubi.PressesForwardPrefix".Localized());

                if (Player.HasCock())
                    OutputText(Player.CockDescript(x));
                else
                    OutputText("Scenes.Rubi.FuckRubi.ClitTokenFourth".Localized());

                OutputText("Scenes.Rubi.FuckRubi.SlipsBetweenThighs".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.FuckRubi.HardCockNoun".Localized());
                else
                    OutputText("Scenes.Rubi.FuckRubi.StiffClitNoun".Localized());

                OutputText("Scenes.Rubi.FuckRubi.PressingTopOfPrefix".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Rubi.FuckRubi.DickNoun".Localized());
                else
                    OutputText("Scenes.Rubi.FuckRubi.ClitNoun".Localized());

                OutputText("Scenes.Rubi.FuckRubi.MutualThrillRemark".Localized());
            }

            OutputText("Scenes.Rubi.FuckRubi.PicksUpPace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0 && !RubiBimbo())
                OutputText("Scenes.Rubi.FuckRubi.SmacksAssAndDenies".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.FuckRubi.BodyShuddersCollapse".Localized());
                else
                    OutputText("Scenes.Rubi.FuckRubi.RubiShuddersCollapse".Localized());

            OutputText("Scenes.Rubi.FuckRubi.HoldsOutUntilClimax".Localized());

            if (Player.HasCock() && Player.CockArea(x) <= RubiCapacity())
            {
                OutputText("Scenes.Rubi.FuckRubi.EruptsDeepInside".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Rubi.FuckRubi.CumDribblesOut".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Rubi.FuckRubi.GenerousCumTrickles".Localized());
                    else
                        if (Player.CumQ() < 2000)
                        {
                            OutputText("Scenes.Rubi.FuckRubi.CumFloodsPrefix".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                                OutputText("Scenes.Rubi.FuckRubi.AssWord".Localized());
                            else
                                OutputText("Scenes.Rubi.FuckRubi.CuntWordAgain".Localized());

                            OutputText("Scenes.Rubi.FuckRubi.ExtraThrustsSuffix".Localized());
                        }
            }
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Rubi.FuckRubi.EruptsBeneathRubi".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Rubi.FuckRubi.FairAmountSeedPrefix".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Rubi.FuckRubi.GenerousSeedPrefix".Localized());
                        else
                            OutputText("Scenes.Rubi.FuckRubi.TorrentSeedPrefix".Localized());
                }
                else
                    OutputText("Scenes.Rubi.FuckRubi.PostClimaxThrobs".Localized(
                        Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Rubi.FuckRubi.CollapsesOntoBed".Localized(
                Player.CockClit(x)
            ));

            if (RubiBimbo())
                OutputText("Scenes.Rubi.FuckRubi.TotallyFillerWords".Localized());
            else
                OutputText("Scenes.Rubi.FuckRubi.AbsolutelyWord".Localized());

            OutputText("Scenes.Rubi.FuckRubi.MurmursAndDriftsOff".Localized());

            OutputText("Scenes.Rubi.FuckRubi.FightsOffSleep".Localized());

            Save.SetBool("hadSex", true);

            Player.Orgasm("Generic");

            ReturnToCamp(1);

            RubiAffection(1);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) + 1);
        }

        public void DildoFuckRubi()
        {
            ClearOutput();

            ShowImage("rubi-dildo-fuck");

            RubiSprite();

            if (!Player.HasCock())
                OutputText("Scenes.Rubi.DildoFuckRubi.LacksEquipment".Localized());
            else
                OutputText("Scenes.Rubi.DildoFuckRubi.PrefersDildoInstead".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.ProducesGoblinDildo".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.RubiApprehensiveAsk".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.TestsDildoOnSelf".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Rubi.DildoFuckRubi.InsertsTipQuickly".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Rubi.DildoFuckRubi.SlidesDildoUpBody".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.LicksDildoClean".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.RubiWaitsHerTurn".Localized(
                RubiMF("him", "her")
            ));

            OutputText("Scenes.Rubi.DildoFuckRubi.SlowsDownForRubi".Localized());

            if (!RubiBimbo())
                OutputText("Scenes.Rubi.DildoFuckRubi.LipsPartShakily".Localized());
            else
                OutputText("Scenes.Rubi.DildoFuckRubi.LipsPartEasily".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.EagerlySucksTip".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.PullsDildoOut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                OutputText("Scenes.Rubi.DildoFuckRubi.TightAssPrefix".Localized());
            else
                OutputText("Scenes.Rubi.DildoFuckRubi.TightPussyPrefix".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.ResistsThenSlipsIn".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.LieDownAndAlign".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Rubi.DildoFuckRubi.AchingClitPeeks".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.DildoFuckRubi.CockThrobsFromDrug".Localized());

            if (Player.LactationQ() > 0)
                OutputText("Scenes.Rubi.DildoFuckRubi.WarmthHardensNipples".Localized());

            Player.CuntChange(30, true, true, false);

            OutputText("Scenes.Rubi.DildoFuckRubi.TakesDildoDeeper".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.BodyBurnsWithHeat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0)
                OutputText("Scenes.Rubi.DildoFuckRubi.SuddenClarityShout".Localized(
                    Player.Mf("sir", "miss")
                ));
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                {
                    OutputText("Scenes.Rubi.DildoFuckRubi.ConvulsesCockPrefix".Localized());

                    if (!RubiIncubus())
                        OutputText("Scenes.Rubi.DildoFuckRubi.LittleCockNoun".Localized());
                    else
                        OutputText("Scenes.Rubi.DildoFuckRubi.DemonicCockNoun".Localized());

                    OutputText("Scenes.Rubi.DildoFuckRubi.LaunchesCumJets".Localized());
                }
                else
                    OutputText("Scenes.Rubi.DildoFuckRubi.ConvulsesUnseenPrefix".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.PumpsFaster".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.DildoFuckRubi.StrokesDesperately".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Rubi.DildoFuckRubi.RubsClitDesperately".Localized());
                else
                    OutputText("Scenes.Rubi.DildoFuckRubi.RubsNipplesVigorously".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Rubi.DildoFuckRubi.FingersInNipples".Localized());

            if (Player.Gender == Gender.Genderless && Player.BiggestBreastSize() == 0)
                OutputText("Scenes.Rubi.DildoFuckRubi.HandsRoamBody".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.OrgasmTidalWave".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Rubi.DildoFuckRubi.LoosesSeveralJets".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Rubi.DildoFuckRubi.MessySplatPhrase".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Rubi.DildoFuckRubi.LightCoatingPhrase".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Rubi.DildoFuckRubi.CopiousLayerPhrase".Localized());
                        else
                            OutputText("Scenes.Rubi.DildoFuckRubi.ThickSeedLayerPhrase".Localized());

                OutputText("Scenes.Rubi.DildoFuckRubi.SentencePeriod".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Rubi.DildoFuckRubi.ClenchesOnInvader".Localized(
                    Player.VaginaDescript(0)
                ));

            if (Player.LactationQ() > 0)
                OutputText("Scenes.Rubi.DildoFuckRubi.BreastsSprayMilk".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.LiesExhausted".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.RubiRubsPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                OutputText("Scenes.Rubi.DildoFuckRubi.PussyWord".Localized());
            else
                OutputText("Scenes.Rubi.DildoFuckRubi.AssholeWord".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.SlightSmileRemark".Localized());

            OutputText("Scenes.Rubi.DildoFuckRubi.HeadToBathroom".Localized());

            Save.SetBool("hadSex", true);

            Player.Orgasm("Generic");

            RubiAffection(1);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) + 2);

            ReturnToCamp(1);
        }

        public void TeaseRubi()
        {
            ClearOutput();

            ShowImage("rubi-tease");

            RubiSprite();

            CoC.Instance.Flags.Set(GameFlagIds.RubiOrgasmDenial, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
            {
                OutputText("Scenes.Rubi.TeaseRubi.OrderRubiToStrip".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                    OutputText("Scenes.Rubi.TeaseRubi.PaleWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                        OutputText("Scenes.Rubi.TeaseRubi.FurCoveredWord".Localized());
                    else
                        OutputText("Scenes.Rubi.TeaseRubi.RedWord".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.StripTakesEternity".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.CockStrainsPanties".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.Rubi.TeaseRubi.HandsOnShoulders".Localized());
                else
                    OutputText("Scenes.Rubi.TeaseRubi.ArmsWrapMidsection".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.GripCockTight".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.KneelBehindBendOver".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.BlushCuriousLook".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.ShimmyPantiesDown".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.BareAssExposed".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.GruntsGrippingCushions".Localized(
                    Player.Mf("sir", "miss")
                ));

                OutputText("Scenes.Rubi.TeaseRubi.QuickeningStrokes".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.FingersAtEntrance".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.HalfwayWhisperMoan".Localized(
                    Player.Mf("sir", "miss")
                ));

                OutputText("Scenes.Rubi.TeaseRubi.SqueezeCockWarn".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.LickFingersClean".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.LeaveAfterFun".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) + 1);
            }
            else
            {
                OutputText("Scenes.Rubi.TeaseRubi.OrderRubiStripTease".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                    OutputText("Scenes.Rubi.TeaseRubi.PaleWordAgain".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1)
                        OutputText("Scenes.Rubi.TeaseRubi.CrimsonWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                            OutputText("Scenes.Rubi.TeaseRubi.FurredWord".Localized());
                        else
                            OutputText("Scenes.Rubi.TeaseRubi.StripedWord".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.StrippingDelight".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.SnatchGlistensArousal".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.Rubi.TeaseRubi.HandsDrapeNeck".Localized());
                else
                    OutputText("Scenes.Rubi.TeaseRubi.ArmsAroundWaist".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.PinchClitGasp".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.KneelBehindToy".Localized(
                    RubiMF("demon", "demoness")
                ));

                OutputText("Scenes.Rubi.TeaseRubi.BareAssTemptation".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.MoansGrippingCushions".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.QuickeningFingers".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.ConicalHandPress".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.HalfwayMoanSays".Localized());

                if (RubiBimbo())
                    OutputText("Scenes.Rubi.TeaseRubi.LikeWord".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.CumExclaim".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.PushFistDeep".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.SlowLickFingers".Localized());

                OutputText("Scenes.Rubi.TeaseRubi.DepartAfterFun".Localized());
            }

            Save.SetBool("hadSex", true);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 10)));

            ReturnToCamp(1);
        }

        public void ReleaseRubi()
        {
            ClearOutput();

            ShowImage("rubi-orgasm-denial-release");

            RubiSprite();

            CoC.Instance.Flags.Set(GameFlagIds.RubiOrgasmDenial, 0);

            OutputText("Scenes.Rubi.ReleaseRubi.OrderSlowStrip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.ReleaseRubi.CockStrainsSeams".Localized());
            else
                OutputText("Scenes.Rubi.ReleaseRubi.ClingsToMound".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.KneelStripUndies".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 3)
            {
                OutputText("Scenes.Rubi.ReleaseRubi.TinyBallsNoted".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBallsType) == 1)
                    OutputText("Scenes.Rubi.ReleaseRubi.SissySackTrapped".Localized());

                OutputText("Scenes.Rubi.ReleaseRubi.BallsSwollenSomewhat".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 9)
                    OutputText("Scenes.Rubi.ReleaseRubi.SizeableBallsSwollen".Localized());
                else
                    OutputText("Scenes.Rubi.ReleaseRubi.EnormousBallsSwollen".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.DirectSitTable".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.WorkStillRemains".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 3)
                OutputText("Scenes.Rubi.ReleaseRubi.SwollenWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 9)
                    OutputText("Scenes.Rubi.ReleaseRubi.SlightlyBlue".Localized());
                else
                    OutputText("Scenes.Rubi.ReleaseRubi.IncrediblySwollenBlue".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.LubedBallsPress".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.SurpriseMorphsLust".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.FingersSearchInside".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.ReleaseRubi.PSpot".Localized());
            else
                OutputText("Scenes.Rubi.ReleaseRubi.GSpot".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.FingersFindSpot".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.ThirdFingerSlip".Localized(
                RubiMF("boy", "girl")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.ReleaseRubi.RectumWord".Localized());
            else
                OutputText("Scenes.Rubi.ReleaseRubi.FoldsWord".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.ProstateMassage".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.RetrieveFingers".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.ReliefThenArousal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 3)
                OutputText("Scenes.Rubi.ReleaseRubi.CumJetFaceLand".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 5)
                    OutputText("Scenes.Rubi.ReleaseRubi.CumJetRepeat".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 7)
                        OutputText("Scenes.Rubi.ReleaseRubi.CumJetCeiling".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 9)
                            OutputText("Scenes.Rubi.ReleaseRubi.CeilingSplatJet".Localized());
                        else
                            OutputText("Scenes.Rubi.ReleaseRubi.PowerfulCumJet".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.BodyRelaxesLimp".Localized());

            OutputText("Scenes.Rubi.ReleaseRubi.LetRubiSleep".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 5)
                OutputText("Scenes.Rubi.ReleaseRubi.CleanUpLeaveNote".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 9)
                    OutputText("Scenes.Rubi.ReleaseRubi.BathroomShowerDry".Localized());
                else
                    OutputText("Scenes.Rubi.ReleaseRubi.BathroomScrubDry".Localized());

            Save.SetBool("hadSex", true);

            RubiAffection(CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls));

            CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, 0);

            CoC.Instance.Flags.Set(GameFlagIds.RubiOrgasmDenial, 0);

            ReturnToCamp(1);
        }

        public void CheatingRubi()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.CheatingRubi.WaitAtTable".Localized());

            OutputText("Scenes.Rubi.CheatingRubi.ExasperatedSigh".Localized());

            OutputText("Scenes.Rubi.CheatingRubi.BimboSeesAnother".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FindHer".Localized(), FindBimboCheatster);

            AddButton(1, "Common.Button.DontCare".Localized(), DontCareAboutNoCheatingRubis);
        }

        public void DontCareAboutNoCheatingRubis()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.DontCareAboutNoCheatingRubis.ShrugFreeChoice".Localized());

            OutputText("Scenes.Rubi.DontCareAboutNoCheatingRubis.LeaveBakery".Localized());

            ReturnToCamp(1);
        }

        public void FindBimboCheatster()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.FindBimboCheatster.DisgruntledSigh".Localized());

            OutputText("Scenes.Rubi.FindBimboCheatster.CreepThroughGarden".Localized());

            OutputText("Scenes.Rubi.FindBimboCheatster.RubiSplayedCouch".Localized());

            OutputText("Scenes.Rubi.FindBimboCheatster.InterruptOrWait".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Interrupt".Localized(), InterruptTheNTRsYouCrazyFool);

            AddButton(1, "Common.Button.WatchNWait".Localized(), WaitAndGetNTRedLikeTheBoyBitchYouAre);
        }

        public void InterruptTheNTRsYouCrazyFool()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.NoOneSticksDick".Localized());

            if (silly)
                OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.WaifuWord".Localized());
            else
                OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.SlutWord".Localized());

            OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.BurstInWarning".Localized());

            OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.RubiJumpsSitsUp".Localized());

            if (Player.Strength >= 61)
                OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.PunchOffGuard".Localized());
            else
            {
                OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.PunchLandsJaw".Localized());

                if (Player.Speed < 61)
                {
                    OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.DuckTooSlow".Localized());

                    OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.WolfmanRetreats".Localized());
                }
                else
                    OutputText("Scenes.Rubi.InterruptTheNTRsYouCrazyFool.DodgeSweepLegs".Localized());
            }

            NTRbimboBitchResolution();
        }

        public void WaitAndGetNTRedLikeTheBoyBitchYouAre()
        {
            ClearOutput();

            ShowImage("rubi-get-ntr-ed");

            RubiSprite();

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.WaitAndWatch".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.AirheadKneelsDown".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.PeekNewWindow".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.ConsiderOtherWindow".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.ShakingKneesReturn".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.FondleUnderArmor".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.YourVagina".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.EachCockToken".Localized());

                OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.PeriodSpace".Localized());
            }
            else
                OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.ParagraphBreak".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.LongLovemaking".Localized());

            if (silly)
                OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.NotInGamePositions".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.ExhaustedSeedPour".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.PoliteSmileReply".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.ConfrontHerNow".Localized());

            OutputText("Scenes.Rubi.WaitAndGetNTRedLikeTheBoyBitchYouAre.SheLightsUp".Localized());

            NTRbimboBitchResolution();
        }

        public void NTRbimboBitchResolution()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.NTRbimboBitchResolution.NeedToTalk".Localized());

            OutputText("Scenes.Rubi.NTRbimboBitchResolution.NodsCoversCushion".Localized());

            OutputText("Scenes.Rubi.NTRbimboBitchResolution.LovesThisBody".Localized());

            OutputText("Scenes.Rubi.NTRbimboBitchResolution.StudiesYourFace".Localized());

            OutputText("Scenes.Rubi.NTRbimboBitchResolution.HugsCushionClose".Localized());

            OutputText("Scenes.Rubi.NTRbimboBitchResolution.ForbidOrAllow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.No".Localized(), NoBimboNTR);

            AddButton(1, "Common.Button.Yes".Localized(), YesBimboNTR);

            AddButton(2, "Common.Button.BreakUp".Localized(), BreakUpWithRubi);

            AddButton(3, "Common.Button.Pimp".Localized(), PimpOutRubi);
        }

        public void NoBimboNTR()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.NoBimboNTR.DemandFaithful".Localized());

            OutputText("Scenes.Rubi.NoBimboNTR.SheGigglesAgrees".Localized());

            OutputText("Scenes.Rubi.NoBimboNTR.SettledRefrains".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiProstitution, -1);

            ReturnToCamp(1);
        }

        public void YesBimboNTR()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.YesBimboNTR.ContemplateThenNod".Localized());

            OutputText("Scenes.Rubi.YesBimboNTR.ShowerOfKisses".Localized());

            OutputText("Scenes.Rubi.YesBimboNTR.SettledFreeSleep".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiProstitution, 1);

            ReturnToCamp(1);
        }

        public void BreakUpWithRubi()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.BreakUpWithRubi.ExplainBreakUp".Localized());

            OutputText("Scenes.Rubi.BreakUpWithRubi.WhisperGetOut".Localized());

            OutputText("Scenes.Rubi.BreakUpWithRubi.DoorSlamsShut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiProstitution, -2);

            CoC.Instance.Flags.Set(GameFlagIds.RubiDisabled, 1);

            ReturnToCamp(1);
        }

        public void PimpOutRubi()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PimpOutRubi.PonderPimpingOptions".Localized());

            OutputText("Scenes.Rubi.PimpOutRubi.RubiLovesPimpingPlan".Localized());

            OutputText("Scenes.Rubi.PimpOutRubi.RubiWillProstituteHerself".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiProstitution, 2);

            ReturnToCamp(1);
        }

        public void ChocoRubiMassage()
        {
            ClearOutput();

            ShowImage("rubi-get-massage");

            RubiSprite();

            OutputText("Scenes.Rubi.ChocoRubiMassage.ChooseRubiSpaPampering".Localized());

            if (!Player.IsNaked())
            {
                OutputText("Scenes.Rubi.ChocoRubiMassage.RubiInstructsUndressing".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Rubi.ChocoRubiMassage.StartSensualUndressing".Localized());
                else
                    if (Player.Corruption >= 33)
                        OutputText("Scenes.Rubi.ChocoRubiMassage.UndressWithoutFanfare".Localized());
                    else
                    {
                        OutputText("Scenes.Rubi.ChocoRubiMassage.MakeRubiTurnAround".Localized());

                        if (Player.BiggestCockArea() >= 40 || Player.BiggestBreastSize() >= 8)
                            OutputText("Scenes.Rubi.ChocoRubiMassage.DoesNotHelpMuch".Localized());

                        OutputText("Scenes.Rubi.ChocoRubiMassage.SentencePeriod".Localized());
                    }
            }

            OutputText("Scenes.Rubi.ChocoRubiMassage.RubiHoldsTowel".Localized(
                RubiMF("feminine boyfriend", "girlfriend")
            ));

            OutputText("Scenes.Rubi.ChocoRubiMassage.HearRubiClothesFall".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 3 ? "small" : (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 8 ? "big" : "huge, stretched"),

                CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 6 ? "already impressive " : (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 3 ? "next-to-nonexistent " : "")
            ));

            OutputText("Scenes.Rubi.ChocoRubiMassage.LickMeltedChocolate".Localized(
                Player.Mf("Giggling", "Smiling")
            ));

            OutputText("Scenes.Rubi.ChocoRubiMassage.ColdGlazeOnBack".Localized());

            OutputText("Scenes.Rubi.ChocoRubiMassage.GruntingForYourPleasure".Localized());

            OutputText("Scenes.Rubi.ChocoRubiMassage.RubiGettingClose".Localized());

            OutputText("Scenes.Rubi.ChocoRubiMassage.LetReleaseOrBottle".Localized());

            Save.SetBool("hadSex", true);

            Player.ChangeFatigue(-40);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Lust, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.TimesRubiMassaged, CoC.Instance.Flags.Get(GameFlagIds.TimesRubiMassaged) + 1);

            ClearButtons();

            AddButton(0, "Scenes.Faerie.SexMenu.ButtonRelease".Localized(), ReleaseRubiMassage);

            AddButton(1, "Common.Button.BottleUp".Localized(), BottleUpRubiMassage);
        }

        public void BottleUpRubiMassage()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.BottleUpRubiMassage.BiteStrawberryInHalf".Localized());

            OutputText("Scenes.Rubi.BottleUpRubiMassage.InviteRubiToSnuggle".Localized());

            OutputText("Scenes.Rubi.BottleUpRubiMassage.RubiThanksGratefully".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiOrgasmDenial, 1);

            CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) + 1);

            ReturnToCamp(2);
        }

        public void ReleaseRubiMassage()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.ReleaseRubiMassage.RubiDeservesTreat".Localized());

            OutputText("Scenes.Rubi.ReleaseRubiMassage.PushStrawberryDeeper".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.ReleaseRubiMassage.SwallowStrawberryWhole".Localized());

            OutputText("Scenes.Rubi.ReleaseRubiMassage.RubiBitesLowerLip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 3)
                    OutputText("Scenes.Rubi.ReleaseRubiMassage.SemenSoaksTowel".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 5)
                        OutputText("Scenes.Rubi.ReleaseRubiMassage.HotCumSplattersFabric".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 7)
                        {
                            OutputText("Scenes.Rubi.ReleaseRubiMassage.CumShootsThroughTowel".Localized());

                            OutputText("Scenes.Rubi.ReleaseRubiMassage.SquirtWeakStrands".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Rubi.ReleaseRubiMassage.ThickCumWave".Localized());

                            OutputText("Scenes.Rubi.ReleaseRubiMassage.FloodDiesToDrip".Localized());
                        }
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 3)
                    OutputText("Scenes.Rubi.ReleaseRubiMassage.SlowLanguidPace".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 5)
                        OutputText("Scenes.Rubi.ReleaseRubiMassage.ThrowsHeadBack".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 7)
                            OutputText("Scenes.Rubi.ReleaseRubiMassage.SlumpsOntoYourBack".Localized());
                        else
                            OutputText("Scenes.Rubi.ReleaseRubiMassage.FlopsForwardInRapture".Localized());

                OutputText("Scenes.Rubi.ReleaseRubiMassage.IgnoreSoakedTowel".Localized());

                OutputText("Scenes.Rubi.ReleaseRubiMassage.DriftOffTogether".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.RubiOrgasmDenial, 0);

            CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, 0);

            ReturnToCamp(2);
        }

        public void GoInRubisClosetSoThatYouCanComeOutOfTheCloset()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.GoInRubisClosetSoThatYouCanComeOutOfTheCloset.AskToSeeWardrobe".Localized());

            if (RubiBimbo())
                OutputText("Scenes.Rubi.GoInRubisClosetSoThatYouCanComeOutOfTheCloset.OhMyGodExclaim".Localized());

            OutputText("Scenes.Rubi.GoInRubisClosetSoThatYouCanComeOutOfTheCloset.RightOverHereReply".Localized());

            OutputText("Scenes.Rubi.GoInRubisClosetSoThatYouCanComeOutOfTheCloset.JawDropClosetSize".Localized());

            OutputText("Scenes.Rubi.GoInRubisClosetSoThatYouCanComeOutOfTheCloset.RubiTwirlsHappily".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), RubiAppearance);
        }

        public void RubiAppearance()
        {
            ClearOutput();

            ShowImage("rubi-inspect-appearance");

            RubiSprite();

            OutputText("Scenes.Rubi.RubiAppearance.LookOverNakedBody".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.RubiLitheFeminineBuild".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                OutputText("Scenes.Rubi.RubiAppearance.PorcelainPaleSkin".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1)
                    OutputText("Scenes.Rubi.RubiAppearance.CherryRedFur".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                        OutputText("Scenes.Rubi.RubiAppearance.SilkyWhiteFurLayer".Localized());
                    else
                        OutputText("Scenes.Rubi.RubiAppearance.CherryRedContrast".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.RubiAppearance.HairlessExceptHair".Localized());
            else
                OutputText("Scenes.Rubi.RubiAppearance.AndWearsHair".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiHairLength) == 0)
                OutputText("Scenes.Rubi.RubiAppearance.ShortWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiHairLength) == 1)
                    OutputText("Scenes.Rubi.RubiAppearance.ShoulderLengthWord".Localized());
                else
                    OutputText("Scenes.Rubi.RubiAppearance.LongWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiHair) == 0)
                OutputText("Scenes.Rubi.RubiAppearance.BlackHairWord".Localized());
            else
                OutputText("Scenes.Rubi.RubiAppearance.BlondeHairWord".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.FeminineHaircutStyle".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiWhiskers) > 0)
                OutputText("Scenes.Rubi.RubiAppearance.CuteWhiskersOnCheeks".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.LipsPaintedColor".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                OutputText("Scenes.Rubi.RubiAppearance.CrimsonRedLips".Localized());
            else
                OutputText("Scenes.Rubi.RubiAppearance.DuskyBlackLips".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.HerPossessive".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 0 || RubiBimbo())
                OutputText("Scenes.Rubi.RubiAppearance.GreenEyesWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 1)
                    OutputText("Scenes.Rubi.RubiAppearance.SolidBlackEyes".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.EyesFramedWithEyeliner".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiHorntype) > 0)
                OutputText("Scenes.Rubi.RubiAppearance.TwoTinyHorns".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.PairOfEars".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 0)
                OutputText("Scenes.Rubi.RubiAppearance.NormalEarsProtrude".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 1)
                    OutputText("Scenes.Rubi.RubiAppearance.CatEarsPopUp".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 2)
                        OutputText("Scenes.Rubi.RubiAppearance.FeatheredEarsProtrude".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.HasBreastsIntro".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.RubiAppearance.FlatChest".Localized());
            else
                OutputText("Scenes.Rubi.RubiAppearance.BreastsToken".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.TwoPerkyBreasts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNippleType) == 1)
                OutputText("Scenes.Rubi.RubiAppearance.BlackNipplesWord".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.NipplesWord".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.BetweenLegsIntro".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
            {
                OutputText("Scenes.Rubi.RubiAppearance.CockPerksUnderGaze".Localized(
                    RubiCock(),

                    CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize)
                ));

                OutputText("Scenes.Rubi.RubiAppearance.BallsUnderCock".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 9)
                    OutputText("Scenes.Rubi.RubiAppearance.CumSwollenWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) >= 9)
                        OutputText("Scenes.Rubi.RubiAppearance.EngorgedBlueTinged".Localized());

                OutputText("Scenes.Rubi.RubiAppearance.TesticlesWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBallsType) == 1)
                    OutputText("Scenes.Rubi.RubiAppearance.ClingsTightPouch".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                OutputText("Scenes.Rubi.RubiAppearance.AndPussyIntro".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
            {
                OutputText("Scenes.Rubi.RubiAppearance.CuteWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCunttype) > 0)
                    OutputText("Scenes.Rubi.RubiAppearance.BlackPussyWord".Localized());

                OutputText("Scenes.Rubi.RubiAppearance.PussyLoveButton".Localized());

                OutputText("Scenes.Rubi.RubiAppearance.HiddenPerfectAsshole".Localized());
            }
            else
                OutputText("Scenes.Rubi.RubiAppearance.SentencePeriodAgain".Localized());

            OutputText("Scenes.Rubi.RubiAppearance.RedSpadedTailSways".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.RubiAppearance.LongBlackSpikeHeels".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GiveItem".Localized(), PickAnItemToFeedRubi);

            AddButton(1, "Common.Button.GiveClothes".Localized(), GiftRubiClothes);

            AddButton(2, "Common.Button.GetDressed".Localized(), PlayDressUp);

            AddButton(14, "Common.Button.Back".Localized(), RubisFuckingHouseYouPervert);
        }

        public void ClosetOption(string button, string name)
        {

            OutputText("Scenes.Rubi.ClosetOption.ParagraphDashToken".Localized(
                name
            ));

            AddNextButton(button, () => DressUpRouter(name));
        }

        public void PlayDressUp()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PlayDressUp.ChooseOutfitPrompt".Localized());

            OutputText("Scenes.Rubi.PlayDressUp.ClosetContentsHeader".Localized());

            ClearButtons();

            ClosetOption("Uniform", "A Waitress's Uniform");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 100)
                ClosetOption("RisqueUniform", "A Risque Waitress's Uniform");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSuitclothes) == 1)
                ClosetOption("Suitclothes", "Suitclothes");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiTightPants) == 1)
                ClosetOption("DashingOutfit", "A Dashing Outfit With Tight Leather Pants");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiTubeTop) == 1)
                ClosetOption("Tube Top", "A Tube Top");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiLongdress) == 1)
                ClosetOption("BallroomDress", "A Long Ballroom Dress");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiGreenAdventurer) == 1)
                ClosetOption("Kokiri Clothes", "A Green Adventurer's Outfit");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNurseClothes) == 1)
                ClosetOption("Nurse Outfit", "A Skimpy Nurse's Outfit");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSwimwear) == 1)
                ClosetOption("Swimwear", "Slutty Swimwear");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBodysuit) == 1)
                ClosetOption("Bodysuit", "A Sheer Bodysuit");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiInquisitorsCorset) == 1)
                ClosetOption("Corset", "An Inquisitor's Corset");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFetishClothes) == 1)
                ClosetOption("Fetish Wear", "Rubber Fetish Clothes");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBondageStraps) == 1)
                ClosetOption("Bondage Straps", "Bondage Straps");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBimboMinidress) == 1)
                ClosetOption("Bimbo Dress", "A Bimbo Minidress");

            AddButton(14, "Common.Button.Back".Localized(), () => RubiAppearance());
        }

        public void DressUpRouter(string arg)
        {
            ClearOutput();

            RubiSprite();

            if (arg == "Suitclothes")
                PutOnASuitYouSlut();
            else
                if (arg == "A Waitress's Uniform")
                    PutRubiInAWaitressUniform();
                else
                    if (arg == "Rubber Fetish Clothes")
                        RubiHasARubberFetish();
                    else
                        if (arg == "A Green Adventurer's Outfit")
                            GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit();
                        else
                            if (arg == "A Tube Top")
                                PutOnATubeTopYouWhore();
                            else
                                if (arg == "A Sheer Bodysuit")
                                    PutOnMyBodysuitYouWhore();
                                else
                                    if (arg == "A Long Ballroom Dress")
                                        GiveRubiALongDressToHideHerHideousBody();
                                    else
                                        if (arg == "A Dashing Outfit With Tight Leather Pants")
                                            PutOnADashingOutfitYouWhore();
                                        else
                                            if (arg == "A Skimpy Nurse's Outfit")
                                                RubiPutsOnNursesClothesSoSheCanCheckYourTemperature();
                                            else
                                                if (arg == "Slutty Swimwear")
                                                    PutOnSluttySwimwearYouSkank();
                                                else
                                                    if (arg == "A Bimbo Minidress")
                                                        PutOnADressYouBimbo();
                                                    else
                                                        if (arg == "Bondage Straps")
                                                            PutOnBondageStrapsYouBondageTrap();
                                                        else
                                                            if (arg == "An Inquisitor's Corset")
                                                                WhyTheFuckIsRubiWearingAnInquisitorsCorset();
                                                            else
                                                                if (arg == "A Risque Waitress's Uniform")
                                                                    PutOnAWaitressUniformYouWhore();
                                                                else
                                                                    if (arg == "An Adventurer's Outfit")
                                                                        GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit();
        }

        public void PutRubiInAWaitressUniform()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.RequestWaitressUniform".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.WhiteLaceBra".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.FlatChestNoNeed".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.FastenBraClasp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.HairlessWord".Localized());
            else
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.FurredWord".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.SnugAroundWaist".Localized());

            if (!RubiBimbo())
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.RummageThroughDrawers".Localized());
            else
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.NoPantiesSurprise".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.PicksTwoHangers".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.BlouseBeforeSkirt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.ShowsFlatChest".Localized());
            else
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.ShowsCleavage".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.TucksBlouseHem".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.FlatsWithHeelHole".Localized());
            else
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.ShortHeels".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.LittleTwirlInUniform".Localized());

            if (!RubiBimbo())
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.GirlyPanties".Localized());
            else
                OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.NakedNethers".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.ComplimentHerLooks".Localized());

            OutputText("Scenes.Rubi.PutRubiInAWaitressUniform.DressedUpNextPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(IceCreamDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnADashingOutfitYouWhore()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.RequestDashingOutfit".Localized());

            OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.SkimpiestGString".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.TuckCockIntoCloth".Localized());

            OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.StringLodgedCheeks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.SizeablePackageInCrotch".Localized());

            OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.PicksWhiteSilkShirt".Localized(
                RubiChest()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.DashingSluttyPiratess".Localized());

            OutputText("Scenes.Rubi.PutOnADashingOutfitYouWhore.DressedUpNextPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(IceCreamDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.RequestAdventurerOutfit".Localized());

            if (silly)
                OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.CollectorOfWaifus".Localized());
            else
                OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.LayerOfLadies".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.RingToTheName".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.WhiteBraFirst".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.FlatChestRemark".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.LatchesBraBack".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.BrownLeggingsNext".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.DarkGreenTunic".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 1)
                OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.CoversFelineEars".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.MidCalfLeatherBoots".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.HeelHoleCutout".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.WoodenSwordProp".Localized());

            OutputText("Scenes.Rubi.GoOnAnAnalAdventureRubiNotReallyJustAnAdventurersOutfit.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(IceCreamDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void GiveRubiALongDressToHideHerHideousBody()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.RequestLongDress".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.OffPinkBraFirst".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.BraNotNeeded".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.FastensBehindBack".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.WhiteGarterBelt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.LegsHairless".Localized());
            else
                OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.LegsFurred".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.WhiteStockingsOn".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.StraplessDressTwirl".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.DarkensRemark".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.DressSlipsOnNeatly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.HeelHoleCutout".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.SlipsOnQuickly".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.ReadyForFancyBall".Localized());

            OutputText("Scenes.Rubi.GiveRubiALongDressToHideHerHideousBody.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(FancyDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void RubiPutsOnNursesClothesSoSheCanCheckYourTemperature()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.RequestNurseUniform".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.WhiteBraFirst".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.FlatChestRemark".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.LatchesBraThenGarter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.LegsFurred".Localized());
            else
                OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.LegsHairless".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.WhiteFishnetStockings".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.WhiteNurseDress".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.HeartEmblem".Localized());
            else
                OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.BreastEmblem".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.NurseCapPlaced".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) > 0)
                OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.HeelHoleCutout".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.RubiPutsOnNursesClothesSoSheCanCheckYourTemperature.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(IceCreamDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void RubiHasARubberFetish()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.RubiHasARubberFetish.RequestRubberOutfit".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.OilLotionDispenser".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.RubiHasARubberFetish.OiledSkin".Localized());
            else
                OutputText("Scenes.Rubi.RubiHasARubberFetish.OiledFurredBody".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.OilLegsThenTorso".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.RubiHasARubberFetish.GlisteningSkin".Localized());
            else
                OutputText("Scenes.Rubi.RubiHasARubberFetish.GlisteningFur".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.OiledReadyToDress".Localized());

            if (!RubiBimbo())
                OutputText("Scenes.Rubi.RubiHasARubberFetish.TransparentRubberPanties".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.TransparentLatexShirt".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.SkirtThenStockings".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                OutputText("Scenes.Rubi.RubiHasARubberFetish.LegsFurred".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.StockingsThenHeels".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.RubiHasARubberFetish.DemonicHeelCutout".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.ReadyForDungeons".Localized());

            OutputText("Scenes.Rubi.RubiHasARubberFetish.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(BarDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnASuitYouSlut()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.RequestSuitOutfit".Localized());

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.BlackLacyBra".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.PutOnASuitYouSlut.BraNotNeeded".Localized());

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.FastensBraThenGarter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.PutOnASuitYouSlut.LegsHairless".Localized());
            else
                OutputText("Scenes.Rubi.PutOnASuitYouSlut.LegsFurred".Localized());

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.BlackStockingsOn".Localized());

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.WhiteButtonShirt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.PutOnASuitYouSlut.DemonicHeelCutout".Localized());

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.CombsHairBack".Localized());

            OutputText("Scenes.Rubi.PutOnASuitYouSlut.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(FancyDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnATubeTopYouWhore()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.RequestTubeTopOutfit".Localized());

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.TinyGStringFirst".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.TucksCockIntoCloth".Localized());

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.StringBetweenCheeks".Localized());

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.PullsOnTubeTop".Localized(
                RubiChest()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 2)
                OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.StrainFlimsyFabric".Localized());

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.SimpleSandals".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.SoleHeelHole".Localized());

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PutOnATubeTopYouWhore.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(() => ExhibitionistDate()));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnSluttySwimwearYouSkank()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.RequestSwimsuitOutfit".Localized());

            OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.BikiniBottomsFirst".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 3)
                OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.LooseOverSmallChest".Localized());
            else
                OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.StrainsOverAmpleBosom".Localized());

            OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.SandalsToFinish".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) > 0)
                OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.SoleHeelHole".Localized());

            OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.SentencePeriodAgain".Localized());

            OutputText("Scenes.Rubi.PutOnSluttySwimwearYouSkank.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(() => ExhibitionistDate()));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnADressYouBimbo()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnADressYouBimbo.RequestMinidressOutfit".Localized());

            OutputText("Scenes.Rubi.PutOnADressYouBimbo.PastelPinkMinidress".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) > 0)
                OutputText("Scenes.Rubi.PutOnADressYouBimbo.HeelHoleCutout".Localized());

            OutputText("Scenes.Rubi.PutOnADressYouBimbo.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PutOnADressYouBimbo.PosesForYou".Localized());

            OutputText("Scenes.Rubi.PutOnADressYouBimbo.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(() => ExhibitionistDate(true)));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnMyBodysuitYouWhore()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.RequestBodysuitOutfit".Localized());

            OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.SheerBodysuitRevealed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.LegsSmooth".Localized());
            else
                OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.LegsFurred".Localized());

            OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.SlipsArmsIntoSleeves".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.PackageTuckedAway".Localized());
            else
                OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.RedRoseCoversMound".Localized());

            OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.ButtClingsPerfectly".Localized());

            OutputText("Scenes.Rubi.PutOnMyBodysuitYouWhore.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(BarDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnBondageStrapsYouBondageTrap()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnBondageStrapsYouBondageTrap.RequestBondageStraps".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) > 0)
                OutputText("Scenes.Rubi.PutOnBondageStrapsYouBondageTrap.SpeculationAside".Localized());

            OutputText("Scenes.Rubi.PutOnBondageStrapsYouBondageTrap.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PutOnBondageStrapsYouBondageTrap.RetrievesLeatherStraps".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.PutOnBondageStrapsYouBondageTrap.StrapsFastenedOn".Localized());

            OutputText("Scenes.Rubi.PutOnBondageStrapsYouBondageTrap.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(() => ExhibitionistDate()));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void WhyTheFuckIsRubiWearingAnInquisitorsCorset()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.RequestInquisitorCorset".Localized());

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.BlackBraRedLace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.BraNotNeeded".Localized());

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.FastensBraThenGarter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.LegsHairless".Localized());
            else
                OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.LegsFurred".Localized());

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.FishnetStockingsOn".Localized());

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.RedGoldCorset".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.HeelHoleCutout".Localized());

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.WhyTheFuckIsRubiWearingAnInquisitorsCorset.DressedUpWhatNow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Date".Localized(), () => DateIntro(BarDate));

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PutOnAWaitressUniformYouWhore()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.RisqueUniformRequest".Localized());

            OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.ShortSkirtOutfit".Localized());

            if (!RubiBimbo())
            {
                OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.PinkFrillyPanties".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.PantiesCoverBallsOnly".Localized());
            }

            OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.WhiteApronTying".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.PinkLacyCocksock".Localized());

            OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.BlackOpenToeHeels".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 1)
                OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.HeelsRemovedAdjustment".Localized());

            OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.SkimpyOutfitPose".Localized());

            OutputText("Scenes.Rubi.PutOnAWaitressUniformYouWhore.DressedUpChoice".Localized());

            RubiAppearance();
        }

        public void CookingDate()
        {

            OutputText("Scenes.Rubi.CookingDate.MissingContentNote".Localized());

            ReturnToCamp(1);
        }

        public void DateIntro(Action date)
        {
            ClearOutput();

            RubiSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesRubiDated) == 0)
            {
                OutputText("Scenes.Rubi.DateIntro.DateIdeaStrikes".Localized());

                OutputText("Scenes.Rubi.DateIntro.RubiEyesLightUp".Localized());
            }
            else
                OutputText("Scenes.Rubi.DateIntro.DressedUpDateOffer".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesRubiDated, CoC.Instance.Flags.Get(GameFlagIds.TimesRubiDated) + 1);

            if ((date == IceCreamDate) && (Player.Gems < 2))
                {
                    OutputText("Scenes.Rubi.DateIntro.DessertDateIdea".Localized());

                    OutputText("Scenes.Rubi.DateIntro.BrokeChampionComplaint".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), RubiAppearance);

                    return ;
                }

            date();
        }

        public void IceCreamDate()
        {

            Player.Gems -= 2;

            RubiSprite();

            OutputText("Scenes.Rubi.IceCreamDate.HandInHandStroll".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.SmallShopEntry".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.ParfaitOrdering".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.SaucerEyesDelight".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.IceCreamOnFace".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.MissedSpotTease".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.PeopleWatchChat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.RubiIcecreamConfession) == 0)
            {
                OutputText("Scenes.Rubi.IceCreamDate.EarnestHandHold".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiIcecreamConfession, CoC.Instance.Flags.Get(GameFlagIds.RubiIcecreamConfession) + 1);
            }

            OutputText("Scenes.Rubi.IceCreamDate.ParfaitFinishedExit".Localized());

            OutputText("Scenes.Rubi.IceCreamDate.CouchSprawlReturn".Localized());

            RubiSexMenu();

            SetExitButton("Common.Button.Back".Localized());
        }

        public void FancyDate()
        {

            RubiSprite();

            if (Player.Gems >= 10)
            {
                Player.Gems -= 10;

                OutputText("Scenes.Rubi.FancyDate.FancyRestaurantChoice".Localized());

                OutputText("Scenes.Rubi.FancyDate.ArmOfferedEscort".Localized(
                    Player.Mf("gentleman", "lady")
                ));

                OutputText("Scenes.Rubi.FancyDate.WolfWaiterArrival".Localized());

                OutputText("Scenes.Rubi.FancyDate.DinerWatchingChance".Localized());

                OutputText("Scenes.Rubi.FancyDate.FoxCoupleWatching".Localized());

                OutputText("Scenes.Rubi.FancyDate.FabiusCarmillaQuote".Localized());

                OutputText("Scenes.Rubi.FancyDate.MockMasculineVoice".Localized());

                OutputText("Scenes.Rubi.FancyDate.DramatizationFocusShift".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.RubiFancyConfession) == 0)
                {
                    OutputText("Scenes.Rubi.FancyDate.LipReadingEnds".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiFancyConfession, CoC.Instance.Flags.Get(GameFlagIds.RubiFancyConfession) + 1);
                }

                OutputText("Scenes.Rubi.FancyDate.FoodArrivesNapkin".Localized());

                OutputText("Scenes.Rubi.FancyDate.FishTasteGood".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MetOttergirl) == 1)
                    OutputText("Scenes.Rubi.FancyDate.CalluFishComparison".Localized());
                else
                    OutputText("Scenes.Rubi.FancyDate.RubiParentsWonder".Localized());

                OutputText("Scenes.Rubi.FancyDate.MealPaidDeparture".Localized());

                OutputText("Scenes.Rubi.FancyDate.CouchSprawlReturn".Localized());

                RubiSexMenu();

                SetExitButton("Common.Button.Back".Localized());
            }
            else
            {
                OutputText("Scenes.Rubi.FancyDate.FancyMealIdea".Localized());

                OutputText("Scenes.Rubi.FancyDate.BrokeChampionComplaint".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), RubiAppearance);
            }
        }

        public void ExhibitionistDate(bool minidress = false)
        {

            RubiSprite();

            var hasDildo = !Player.IsTaur() && Player.HasVagina() && (Player.HasKeyItem("Dildo") || Player.HasKeyItem("Deluxe Dildo") || Player.HasKeyItem("Demonic Strap-On"));

            OutputText("Scenes.Rubi.ExhibitionistDate.SluttyOutfitVenue".Localized());

            OutputText("Scenes.Rubi.ExhibitionistDate.QuietAvenueWalk".Localized());

            if (minidress)
            {
                ClearButtons();

                AddNextButton("Common.Button.ShowOff".Localized(), ExhibitionistShowOff);

                AddNextButton("Common.Button.PublicSex".Localized(), ExhibitionistAnal);
            }
            else
                ExhibitionistShowOff();
        }

        public void ExhibitionistShowOff()
        {

            OutputText("Scenes.Rubi.ExhibitionistShowOff.RubiGulpsFlash".Localized());

            OutputText("Scenes.Rubi.ExhibitionistShowOff.ButtocksSqueezeAssure".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText(RubiCock());
            else
                OutputText("Scenes.Rubi.ExhibitionistShowOff.GlisteningWetPussy".Localized());

            OutputText("Scenes.Rubi.ExhibitionistShowOff.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.ExhibitionistShowOff.EquineJawDrops".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.ExhibitionistShowOff.AlleyGiggleFit".Localized());

            OutputText("Scenes.Rubi.ExhibitionistShowOff.SerialFlashingSpree".Localized());

            OutputText("Scenes.Rubi.ExhibitionistShowOff.FlashCountEstimate".Localized());

            OutputText("Scenes.Rubi.ExhibitionistShowOff.WildKissGrip".Localized());

            RubiSexMenu();

            SetExitButton("Common.Button.Back".Localized());
        }

        public void ExhibitionistAnal()
        {
            ClearOutput();

            RegisterMacro("redskin", CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3);

            RegisterMacro("rubifur", CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2);

            RegisterMacro("rubimale", CoC.Instance.Flags.Get(GameFlagIds.RubiShe) == 0);

            RegisterMacro("taller", Player.Tallness > 63);

            RegisterMacro("shorter", Player.Tallness < 57);

            RegisterMacro("denial", CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0);

            RegisterMacro("strapon", Player.HasKeyItem("Demonic Strap-On"));

            RegisterMacro("vines", Player.Armor.Id == ArmorIds.VineArmorBehavior);

            OutputText("Scenes.Rubi.ExhibitionistAnal.OutdoorGulpConsent".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.CrateDiscovery".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.ShyGlanceReaction".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.DressTailWound".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.InsistentMouthPress".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.DistantFootstepsIgnored".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.RaptAttentionUndress".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.DistractionStolenKiss".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.TeaseLovingThrob".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.LastTwirlAttention".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.EntranceSlathering".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnal.GroanOfPleasure".Localized());

            if (Player.HasCock())
                ExhibitionistAnalCock();
            else
                ExhibitionistAnalDildo();
        }

        public void ExhibitionistAnalCock()
        {
            ClearOutput();

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.ComposeThenAlign".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.HipWiggleInvite".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.SlowFirstThrust".Localized(
                Player.Body.Get<SkinPart>().Desc == "skin" ? "flesh" : (Player.HasGooSkin() ? "goo" : "[skindesc]")
            ));

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.FasterLosingYourself".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.StayingToPlease".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.BriefPauseWarning".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.RipplePaceBuilding".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.FeignedDecencyExit".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalCock.DecencyAbandoned".Localized());

            ExhibitionistAnalEnd();
        }

        public void ExhibitionistAnalDildo()
        {
            ClearOutput();

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.FetchToyMoment".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.RubiCoaxingSkill".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.LanguidGrinding".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.TrulyBelongsNow".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.ProlongingMoment".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.NameCallingThrill".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.RewardDecision".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.FaceInBackEmbrace".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.DressClingsSkin".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalDildo.SecondToPrepare".Localized());

            ExhibitionistAnalEnd();
        }

        public void ExhibitionistAnalEnd()
        {
            ClearOutput();

            OutputText("Scenes.Rubi.ExhibitionistAnalEnd.BreathCatchEmbrace".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalEnd.QuickKissDressing".Localized());

            OutputText("Scenes.Rubi.ExhibitionistAnalEnd.BlurtedTurningAway".Localized());

            Save.SetBool("hadSex", true);

            Player.Orgasm("DickVaginal");

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiOrgasmDenial) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) + 1);

            RubiSexMenu();

            SetExitButton("Common.Button.Back".Localized());
        }

        public void BarDate()
        {

            RubiSprite();

            OutputText("Scenes.Rubi.BarDate.BarVenueChoice".Localized());

            OutputText("Scenes.Rubi.BarDate.BarstoolSeating".Localized());

            OutputText("Scenes.Rubi.BarDate.PlaceIsGreatComment".Localized());

            OutputText("Scenes.Rubi.BarDate.CuddlingGirlsGaze".Localized());

            OutputText("Scenes.Rubi.BarDate.BartenderCocktailCall".Localized());

            OutputText("Scenes.Rubi.BarDate.DrinkToastClink".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.RubiBarConfession) == 0)
            {
                OutputText("Scenes.Rubi.BarDate.GratefulHeadLean".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiBarConfession, CoC.Instance.Flags.Get(GameFlagIds.RubiBarConfession) + 1);
            }

            OutputText("Scenes.Rubi.BarDate.AmorousLapSitting".Localized());

            OutputText("Scenes.Rubi.BarDate.ShoulderGraspWalk".Localized());

            OutputText("Scenes.Rubi.BarDate.BedroomLipsLocked".Localized());

            RubiSexMenu();

            SetExitButton("Common.Button.Back".Localized());
        }

        public void AnallyTrainYourBitchBoySlutHowToBeAnalForDCL()
        {
            ClearOutput();

            RubiSprite();

            CoC.Instance.Flags.Set(GameFlagIds.RubiTimesAnallyTrained, CoC.Instance.Flags.Get(GameFlagIds.RubiTimesAnallyTrained) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiTimesAnallyTrained) == 1)
            {
                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CouchSmallTalk".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.RubiGulpsAndNods".Localized());

                if (RubiBimbo())
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.TotallyWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.TooBigToFit".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FrownAtTheDoubt".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SheepishAdmission".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.OpensTheToyChest".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.RedderAtLeast".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LittleYouCanSee".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SitsKnockKneed".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FidgetingConfession".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SurveysToyArray".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.RaisesAnEyebrow".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.DickWobblesEnticingly".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SentencePeriod".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LicksLipsAndAgrees".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SitsOnBedEdge".Localized(
                    RubiChest()
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNippleType) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BlackWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.StrainingNegligee".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNippleType) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EbonWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.HandFluttersDown".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText(RubiCock());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.DampeningPussy".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SentencePeriodAgain".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EagerForTheShow".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CrimsonRed".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.DuskyBlack".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LipsLickingThe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PreCum".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.JuicesWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CleanFingersToLube".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.GraspsTheDildo".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) != 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SlickVagina".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.HungryAsshole".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SentencePeriodThird".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LipsFormAnO".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) != 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CuntWord".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssholeWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.ClingsTightLikeVice".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CreatureMoansSoftly".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BitesLipAndWaits".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BackToReality".Localized(
                    RubiMF("boy", "girl")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FullAttentionBob".Localized(
                        RubiCock()
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CommaAndWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.GlisteningCuntLips".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CriesGrowLouder".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.HipsSpinVigorously".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BacksideWord".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FuckHoleWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FilledWithFakeDick".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CannotHoldOut".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BareWord".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FurredWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LegsBuckleMadly".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CockSpasmsAndSprays".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LooksAtHerCrotch".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SlouchesAfterOrgasm".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssholeWordAgain".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CuntWordAgain".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SeekingSweetFreedom".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SlipsOffTheBed".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SeesTheToyChest".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssWord".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PussyWord".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SentencePeriodFourth".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SurprisePlugIn".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EarWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 1)
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FelineEarAgain".Localized());
                    else
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FeatheredEarAgain".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PlugStaysHours".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LeavesForNow".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 5)));
            }
            else
            {
                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BringsUpTraining".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.GrinsAndPromises".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.StandsAndSways".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SitsWithStupidGrin".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LeansAtTheDoorframe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText(RubiCock());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SweetPussy".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.ReadyOnDisplay".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LicksLipsHungrily".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LeftBreathlessByKiss".Localized(
                    RubiMF("demon", "demoness")
                ));

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.MouthsEntwineFully".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.RubiIsBreathless".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.GoBrightRed".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.GoVividPurple".Localized());
                    else
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BlushingThroughFur".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SnapsToPresent".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AwkwardLapSitting".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EyesWidenAtToy".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BeamsHoldingItUp".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.RetrievesTheFlagon".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PeachesAndAlcohol".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssholeWordThird".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PussyWordAgain".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CoverEveryNook".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PreppedAndReady".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PartingLipsExclaim".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.TwitchesThenRises".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.ParagraphBreak".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AnotherMinuteToSink".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssWordAgain".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PussyWordThird".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.ProperlyTrained".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PushesDownFurther".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssholeWordFourth".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CuntWordThird".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.StretchedToLimits".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.BreathGrowsRagged".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.DisrobesForEagerSlut".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.WarmSensationRolls".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.NoticesTheAirPump".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AnusWord".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CuntWordFourth".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EyesWideMouthFull".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PumpsItOnceMore".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LoverStrainsOnLeg".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PleadingLookAtPump".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.QuickPumpThenFinal".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 0 || RubiBimbo())
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.GreenWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 1)
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PurpleWord".Localized());
                    else
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.ShinyBlack".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EyesCloseOnOrgasm".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CockTwitchesAndSplatters".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.DoesntLastLonger".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssWordThird".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.CuntWordFifth".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.MakesHerSqueal".Localized(
                    RubiMF("demon", "demoness")
                ));

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EnoughToPushOver".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PullsBackForJets".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SoftPoppingSound".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.TenMinutesTogether".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.AssWordFourth".Localized());
                else
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PussyWordFourth".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SentencePeriodFifth".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.SurprisePlugInAgain".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 0)
                    OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.EarWordAgain".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 1)
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FelineEarAgain".Localized());
                    else
                        OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.FeatheredEarAgain".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.PlugStaysHoursAgain".Localized());

                OutputText("Scenes.Rubi.AnallyTrainYourBitchBoySlutHowToBeAnalForDCL.LeavesForNowAgain".Localized());

                Player.Orgasm("Dick");
            }

            Save.SetBool("hadSex", true);

            ReturnToCamp(1);
        }

        public void GetFuckedByRubi()
        {
            ClearOutput();

            ShowImage("rubi-fucks-you");

            RubiSprite();

            OutputText("Scenes.Rubi.GetFuckedByRubi.RubiLeadsToBedroom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                OutputText("Scenes.Rubi.GetFuckedByRubi.RedWord".Localized());
            else
                OutputText("Scenes.Rubi.RubiAppearance.BlackEyesWord".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.DelicateLipsParting".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.RubiSmilesAfterKiss".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.InsistentGentleMovements".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 0 || RubiBimbo())
                OutputText("Scenes.Rubi.GetFuckedByRubi.GreenWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 1)
                    OutputText("Scenes.Rubi.GetFuckedByRubi.PurpleWord".Localized());
                else
                    OutputText("Scenes.Rubi.GetFuckedByRubi.BlackWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) > 0)
                OutputText("Scenes.Rubi.GetFuckedByRubi.SlittedWord".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.SeekingApprovalGaze".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.DemonMorphStripsYou".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Rubi.GetFuckedByRubi.AssholeWord".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.CuntWord".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.ReadyForMeTaunt".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Rubi.GetFuckedByRubi.IgnoringYourCocks".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.FacePressedDeepInhale".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Rubi.GetFuckedByRubi.AssWord".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.MoisteningPussy".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.SmallMoanLeaningBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiHair) == 0)
                OutputText("Scenes.Rubi.GetFuckedByRubi.BlackHairedWord".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.BlondeWord".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.HeadBetweenThighs".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.LipsWrappedTongueFlick".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.FreshlyLubricatedUndress".Localized());

            if (this.RubiGetCockType() == BodyPartTypes.Human)
                OutputText("Scenes.Rubi.GetFuckedByRubi.PinkMushroomTip".Localized());
            else
                if (this.RubiGetCockType() == BodyPartTypes.Demon)
                    OutputText("Scenes.Rubi.GetFuckedByRubi.NoduleLadenPurpleTip".Localized());
                else
                    if (this.RubiGetCockType() == BodyPartTypes.Horse)
                        OutputText("Scenes.Rubi.GetFuckedByRubi.BluntedMuskyTip".Localized());
                    else
                        if (this.RubiGetCockType() == BodyPartTypes.Cat)
                            OutputText("Scenes.Rubi.GetFuckedByRubi.SpikedTip".Localized());
                        else
                            if (this.RubiGetCockType() == BodyPartTypes.Anemone)
                                OutputText("Scenes.Rubi.GetFuckedByRubi.TentacleRingedCrown".Localized());
                            else
                                OutputText("Scenes.Rubi.GetFuckedByRubi.InvalidCockTypeError".Localized(
                                    this.RubiGetCockType()
                                ));

            OutputText("Scenes.Rubi.GetFuckedByRubi.CockEmergesFromForeskin".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.ConfirmingLookGrantsCock".Localized());

            if (!Player.HasVagina())
                Player.ButtChange(CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) * CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) / 6, true, true, false);
            else
                Player.CuntChange(CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) * CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) / 6, true, true, false);

            OutputText("Scenes.Rubi.GetFuckedByRubi.LegLiftedOntoShoulder".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Rubi.GetFuckedByRubi.ProstateWord".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.GSpotWord".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.HotShiverUpSpine".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Rubi.GetFuckedByRubi.ProstateWordAgain".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.GSpotWordAgain".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.LimbsTingleBuilding".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.GrippingSheetsTingle".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.GetFuckedByRubi.SeveralThingsAtOnce".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 0 || RubiBimbo())
                OutputText("Scenes.Rubi.GetFuckedByRubi.GreenWordAgain".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) == 1)
                    OutputText("Scenes.Rubi.GetFuckedByRubi.PurpleWordAgain".Localized());
                else
                    OutputText("Scenes.Rubi.GetFuckedByRubi.BlackWordAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) > 0)
                OutputText("Scenes.Rubi.GetFuckedByRubi.SlittedWordAgain".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.AskingToComeInside".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.HeartSkipsABeat".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.CockPlungesNewPressure".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Rubi.GetFuckedByRubi.ProstateWordThird".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.GSpotWordThird".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.OrgasmicEnergyBursts".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.CockSurgesJismJets".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Rubi.GetFuckedByRubi.BowelsWord".Localized());
            else
                OutputText("Scenes.Rubi.GetFuckedByRubi.WombWord".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.PeriodSpace".Localized());

            if (!(CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 4))
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 6)
                {
                    OutputText("Scenes.Rubi.GetFuckedByRubi.OozingFromOverfill".Localized());

                    if (!Player.HasVagina())
                        OutputText("Scenes.Rubi.GetFuckedByRubi.AssWordAgain".Localized());
                    else
                        OutputText("Scenes.Rubi.GetFuckedByRubi.CuntWordAgain".Localized());

                    OutputText("Scenes.Rubi.GetFuckedByRubi.DribblingDownToFloor".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 9)
                    {
                        OutputText("Scenes.Rubi.GetFuckedByRubi.SeedFillsEveryCrevice".Localized());

                        if (!Player.HasVagina())
                            OutputText("Scenes.Rubi.GetFuckedByRubi.AssWordThird".Localized());
                        else
                            OutputText("Scenes.Rubi.GetFuckedByRubi.CuntWordThird".Localized());

                        OutputText("Scenes.Rubi.GetFuckedByRubi.SmallTorrentOnFloor".Localized());
                    }
                    else
                        OutputText("Scenes.Rubi.GetFuckedByRubi.StomachSwellsDangerously".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.RubiKeepsAssailing".Localized());

            OutputText("Scenes.Rubi.GetFuckedByRubi.WagglingFingerHazily".Localized());

            Save.SetBool("hadSex", true);

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            ReturnToCamp(1);
        }

        public void RubiHotdogging()
        {
            ClearOutput();

            ShowImage("rubi-hotdogged");

            RubiSprite();

            OutputText("Scenes.Rubi.RubiHotdogging.PullRubiOntoLap".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.BreakKissGaspBreath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                OutputText("Scenes.Rubi.RubiHotdogging.PaleSkinnedWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                    OutputText("Scenes.Rubi.RubiHotdogging.RedSkinnedWord".Localized());
                else
                    OutputText("Scenes.Rubi.RubiHotdogging.FurCoveredWord".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.LaysNakedOnLap".Localized(
                RubiMF("boy", "girl")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.RubiHotdogging.GrippingCock".Localized());
            else
                OutputText("Scenes.Rubi.RubiHotdogging.FingerAlongClit".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.FlirtyTeasingQuestion".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.WigglesHipsAwaitingReply".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.GiggleAndNod".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.GruntApprovalArmsWide".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.SinkingIntoSoftCushions".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.DeviousIdeasInstead".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.RubiHotdogging.HandDownSpineShudder".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.RubiHotdogging.TenderSkin".Localized());
            else
                OutputText("Scenes.Rubi.RubiHotdogging.SilkyFur".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.BackArchesSoftGasp".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.ShakyApologyAccepted".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.SpellboundFleshTeased".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                OutputText("Scenes.Rubi.RubiHotdogging.CuntMoistening".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.RubiHotdogging.AndWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.RubiHotdogging.CockHardening".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.PlayfulAssSmack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                OutputText("Scenes.Rubi.RubiHotdogging.PaleSkin".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                    OutputText("Scenes.Rubi.RubiHotdogging.CrimsonSkin".Localized());
                else
                    OutputText("Scenes.Rubi.RubiHotdogging.FurCoveredFlesh".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.JiggleWithSmile".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.LipstickSmudgedCockGrab".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                OutputText("Scenes.Rubi.RubiHotdogging.SkinWord".Localized());
            else
                OutputText("Scenes.Rubi.RubiHotdogging.CoatWord".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.ManhoodBetweenCheeks".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.FrustratedGroanObeys".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                OutputText("Scenes.Rubi.RubiHotdogging.AssholeWord".Localized());
            else
                OutputText("Scenes.Rubi.RubiHotdogging.PussyWord".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.TwoFingersRubbing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                OutputText("Scenes.Rubi.RubiHotdogging.ClitWord".Localized());
            else
                OutputText("Scenes.Rubi.RubiHotdogging.PerineumWord".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.RubiHotdogging.CouldTeaseWithoutCum".Localized());

            Save.SetBool("hadSex", true);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.TeaseRubi".Localized(), TeaseButtjobs);

            AddButton(1, "Common.Button.Pop".Localized(), PopButtjobs);
        }

        public void TeaseButtjobs()
        {
            ClearOutput();

            ShowImage("rubi-tease-buttjob");

            RubiSprite();

            OutputText("Scenes.Rubi.TeaseButtjobs.ThumbPlungeFurther".Localized(
                RubiMF("boy", "girl")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                OutputText("Scenes.Rubi.TeaseButtjobs.AssWord".Localized());
            else
                OutputText("Scenes.Rubi.TeaseButtjobs.PussyWord".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.FindingWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                OutputText("Scenes.Rubi.TeaseButtjobs.ProstateWord".Localized());
            else
                OutputText("Scenes.Rubi.TeaseButtjobs.GSpotWord".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.RubbingCirclesAroundIt".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.FamiliarWithLoversBody".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.OrgasmBuildingTeasing".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.SpinningRubiAround".Localized());

            if (Player.CumQ() <= 50)
                OutputText("Scenes.Rubi.TeaseButtjobs.OneGoProudLook".Localized());
            else
                if (Player.CumQ() <= 150)
                    OutputText("Scenes.Rubi.TeaseButtjobs.ThroatStrainingSwallow".Localized());
                else
                    OutputText("Scenes.Rubi.TeaseButtjobs.MouthfulAfterMouthful".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Rubi.TeaseButtjobs.GuzzledFullBelly".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.FinalSpurtOnFace".Localized());

            OutputText("Scenes.Rubi.TeaseButtjobs.LaughAndKissDemon".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.TeaseButtjobs.SnugglingNakedBody".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.RubiOrgasmDenial, 1);

            Player.Orgasm("Generic");

            ReturnToCamp(1);
        }

        public void PopButtjobs()
        {
            ClearOutput();

            ShowImage("rubi-pop-buttjob");

            RubiSprite();

            OutputText("Scenes.Rubi.PopButtjobs.ThumbDeepThrust".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                OutputText("Scenes.Rubi.PopButtjobs.AssWord".Localized());
            else
                OutputText("Scenes.Rubi.PopButtjobs.PussyWord".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.CommaFindingWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                OutputText("Scenes.Rubi.PopButtjobs.ProstateWord".Localized());
            else
                OutputText("Scenes.Rubi.PopButtjobs.GSpotWord".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.CircleRubbingMoan".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.SenseHerClimax".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.RubiPossessive".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                OutputText("Scenes.Rubi.PopButtjobs.CuntClampGush".Localized());
            else
                OutputText("Scenes.Rubi.PopButtjobs.AssholeQuiverSuck".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.BodySpasmClimax".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                OutputText("Scenes.Rubi.PopButtjobs.CockJerkFirstSpray".Localized());

            if (!(CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 4))
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 5)
                    OutputText("Scenes.Rubi.PopButtjobs.SecondCumStream".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 7)
                        OutputText("Scenes.Rubi.PopButtjobs.ThirdFourthStream".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) <= 9)
                            OutputText("Scenes.Rubi.PopButtjobs.SeveralMoreStreams".Localized());
                        else
                            OutputText("Scenes.Rubi.PopButtjobs.DozenMoreStreams".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.SentencePeriod".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.YourOrgasmComing".Localized());

            if (Player.Balls > 1)
                OutputText("Scenes.Rubi.PopButtjobs.ChurnWord".Localized());
            else
                OutputText("Scenes.Rubi.PopButtjobs.ChurnsWord".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.CockStiffensErupt".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.SpinRubiAround".Localized());

            if (Player.CumQ() <= 50)
                OutputText("Scenes.Rubi.PopButtjobs.OneGoProudLook".Localized());
            else
                if (Player.CumQ() <= 150)
                    OutputText("Scenes.Rubi.PopButtjobs.ThroatStrainingSwallow".Localized());
                else
                    OutputText("Scenes.Rubi.PopButtjobs.MouthfulAfterMouthful".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Rubi.PopButtjobs.GuzzlingBellyFull".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.SentencePeriodAgain".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.FinalSpurtFace".Localized());

            OutputText("Scenes.Rubi.PopButtjobs.LaughKissDemon".Localized(
                RubiMF("boy", "girl")
            ));

            OutputText("Scenes.Rubi.PopButtjobs.SnuggleNakedBody".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.RubiBluBalls, 0);

            ReturnToCamp(1);
        }

        public void GiveRubiATFItem(Identifier itype)
        {
            ClearOutput();

            RubiSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiTimesGivenAnItem) == 0)
            {
                OutputText("Scenes.Rubi.GiveRubiATFItem.ExamineOfferedItem".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.ExplainItemEffects".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.RubiTimesGivenAnItem, CoC.Instance.Flags.Get(GameFlagIds.RubiTimesGivenAnItem) + 1);

            if (itype == ConsumableIds.SuccubiMilk || itype == ConsumableIds.PurifiedSuccubiMilk)
            {
                if (itype == ConsumableIds.SuccubiMilk)
                    OutputText("Scenes.Rubi.GiveRubiATFItem.HandOverMilkyPink".Localized());
                else
                    OutputText("Scenes.Rubi.GiveRubiATFItem.HandOverPurifiedPink".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 7 || (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 30 && hyper))
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.GaspsBreastsSwell".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) + 1);
                }

                if ((CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 4) && (!hyper))
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.CockSwellsThenShrinks".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) - 1);

                    OutputText("Scenes.Rubi.GiveRubiATFItem.InchPenisThird".Localized(
                        (float)(CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) * 10) / 10
                    ));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) <= 4 && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.CockSwellsSlightly".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 0);

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 1)
                        {
                            OutputText("Scenes.Rubi.GiveRubiATFItem.ShudderSoftMoan".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.RubiNoCunt, 0);

                            CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);

                            CoC.Instance.Flags.Set(GameFlagIds.RubiCunttype, 0);
                        }
                    }

                if (Random.Shared.Next(3) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.FingersDiveBetween".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiNoCunt, 0);

                    CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiFeet) == 0 && Random.Shared.Next(3) == 0 && itype == ConsumableIds.SuccubiMilk)
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.HopsFootToFoot".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiFeet, 1);
                }

                if (Random.Shared.Next(3) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiHorntype) == 0 && itype == ConsumableIds.SuccubiMilk)
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.HornsSproutForehead".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiHorntype, 1);
                }

                if (Random.Shared.Next(3) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 1 && itype == ConsumableIds.SuccubiMilk)
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.StomachRedSpots".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                        OutputText("Scenes.Rubi.GiveRubiATFItem.SkinWord".Localized());
                    else
                        OutputText("Scenes.Rubi.GiveRubiATFItem.FurCoatWord".Localized());

                    OutputText("Scenes.Rubi.GiveRubiATFItem.BlotchesSpreadOver".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                        OutputText("Scenes.Rubi.GiveRubiATFItem.SilkyFurFalling".Localized());

                    OutputText("Scenes.Rubi.GiveRubiATFItem.HairlessSkinCrimson".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.RubiSkin, 1);
                }
            }
            else
                if (itype == ConsumableIds.IncubiDraft || itype == ConsumableIds.PurifiedIncubiDraft)
                {
                    if (itype == ConsumableIds.IncubiDraft)
                        OutputText("Scenes.Rubi.GiveRubiATFItem.HandOverMilkyBottle".Localized());
                    else
                        OutputText("Scenes.Rubi.GiveRubiATFItem.HandOverPurifiedBottle".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 0) && (!hyper))
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.GaspsBreastsShrink".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) - 1);

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 0)
                            CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 0);
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0 && (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 12 || (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 20 && this.RubiGetCockType() == BodyPartTypes.Horse) || (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 30 && hyper)))
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.CockSwellsThenGrows".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) + 1);

                        OutputText("Scenes.Rubi.GiveRubiATFItem.InchPenisAgain".Localized(
                            (float)(CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) * 10) / 10
                        ));
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                        {
                            OutputText("Scenes.Rubi.GiveRubiATFItem.HandsDartBetween".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                                OutputText("Scenes.Rubi.GiveRubiATFItem.RedWord".Localized());
                            else
                                OutputText("Scenes.Rubi.GiveRubiATFItem.PinkWord".Localized());

                            OutputText("Scenes.Rubi.GiveRubiATFItem.BrandNewCock".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 5);

                            CoC.Instance.Flags.Set(GameFlagIds.RubiCockType, BodyPartTypes.Index(BodyPartTypes.Human));
                        }

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0 && Random.Shared.Next(3) == 0)
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.HandBetweenThighs".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiNoCunt, 1);

                        CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 0);

                        CoC.Instance.Flags.Set(GameFlagIds.RubiCunttype, 0);
                    }

                    if (Random.Shared.Next(3) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 1 && itype == ConsumableIds.IncubiDraft)
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.StomachRedSpotsAgain".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                            OutputText("Scenes.Rubi.GiveRubiATFItem.SkinWordAgain".Localized());
                        else
                            OutputText("Scenes.Rubi.GiveRubiATFItem.FurCoatWordAgain".Localized());

                        OutputText("Scenes.Rubi.GiveRubiATFItem.BlotchesSpreadOverAgain".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                            OutputText("Scenes.Rubi.GiveRubiATFItem.SilkyFurFallingAgain".Localized());

                        OutputText("Scenes.Rubi.GiveRubiATFItem.HairlessSkinCrimsonAgain".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiSkin, 1);
                    }

                    if (Random.Shared.Next(3) == 0 && CoC.Instance.Flags.Get(GameFlagIds.RubiHorntype) == 0 && itype == ConsumableIds.IncubiDraft)
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.HornsSproutForeheadAgain".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiHorntype, 1);
                    }
                }
                else
                    if (itype == ItemIds.GroPlus)
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.WhereInjectGro".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Breast".Localized(), RubiGrowPlusBreasts);

                        AddButton(1, "Common.Button.Penis".Localized(), RubiPenisGroPlus);

                        AddButton(14, "Common.Button.Back".Localized(), PickAnItemToFeedRubi);

                        return ;
                    }
                    else
                        if (itype == ItemIds.Reducto)
                        {
                            OutputText("Scenes.Rubi.GiveRubiATFItem.RubiUseReducto".Localized());

                            ClearButtons();

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 0)
                                AddButton(0, "Common.Button.Breasts".Localized(), RubiBoobsReducto);

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                AddButton(1, "Common.Button.Penis".Localized(), RubiPenisReducto);

                            AddButton(14, "Common.Button.Back".Localized(), PickAnItemToFeedRubi);

                            return ;
                        }
                        else
                            if (itype == ItemIds.BimboLiqueur)
                            {
                                OutputText("Scenes.Rubi.GiveRubiATFItem.BimboLiqueurOffer".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.LiqueurFreshStart".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.ClutchBottleUnsure".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.NodConfirmWant".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.DeepBreathOpening".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.EffectsRapidSuccession".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiHairLength) == 0)
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.ShortWord".Localized());
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiHairLength) == 1)
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.ShoulderLengthWord".Localized());
                                    else
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.LongWord".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.PlatinumHairBurst".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.RubiHairLength, 2);

                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                {
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.WideEyedTrepidation".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.VaginaWord".Localized());
                                    else
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.PristineNewVagina".Localized());

                                    OutputText("Scenes.Rubi.GiveRubiATFItem.FormerMaleNowFemale".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 0);

                                    CoC.Instance.Flags.Set(GameFlagIds.RubiNoCunt, 0);

                                    CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);
                                }

                                OutputText("Scenes.Rubi.GiveRubiATFItem.GripsHerThroat".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 7)
                                {
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.ChestDisappointment".Localized());

                                    OutputText("Scenes.Rubi.GiveRubiATFItem.FleshAroundNipples".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiNippleType) > 0)
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.BlackWord".Localized());

                                    OutputText("Scenes.Rubi.GiveRubiATFItem.NipplesTrembleExpand".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) == 0)
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.PubescentBCup".Localized());

                                    OutputText("Scenes.Rubi.GiveRubiATFItem.MorbidCuriosityGrowth".Localized());

                                    CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 7);
                                }

                                OutputText("Scenes.Rubi.GiveRubiATFItem.SilenceBrokenShout".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.RealizesBigHug".Localized());

                                if (Player.BiggestBreastSize() < 1)
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.YoursWord".Localized());
                                else
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.YouWord".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.ThanksBabyPraise".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.GrinOfCourse".Localized());

                                Player.ConsumeItem(ItemIds.BimboLiqueur);

                                CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);

                                CoC.Instance.Flags.Set(GameFlagIds.RubiBimbo, 1);

                                CoC.Instance.Flags.Set(GameFlagIds.RubiHair, 1);

                                RubiSexMenu();

                                SetExitButton("Common.Button.Back".Localized());

                                return ;
                            }
                            else
                                if (itype == ItemIds.Equinum)
                                {
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.HandOverVial".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) == 0)
                                    {
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.HandsDartBetweenAgain".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 3)
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.RedWordAgain".Localized());
                                        else
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.PinkWordAgain".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.BrandNewCockAgain".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 5);
                                    }

                                    if (this.RubiGetCockType() != BodyPartTypes.Horse)
                                    {
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.CockHeadFlattens".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockType, BodyPartTypes.Index(BodyPartTypes.Horse));

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.DickThickensInHand".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) < 9)
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.SwollenWord".Localized());
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBluBalls) >= 9)
                                                OutputText("Scenes.Rubi.GiveRubiATFItem.EngorgedWord".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBallsType) == 1)
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.TightTrappyWord".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.ColorsMoveForward".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.SighRelinquishCock".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) + 3 + Random.Shared.Next(5));
                                    }
                                }
                                else
                                    if (itype == ConsumableIds.TrapOil)
                                    {
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.TwirlOilBottle".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.GirlyBedSight".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.SettleOnThighs".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.FingersFindKnots".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.FlipHerOver".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.CockTwitchPrecum".Localized());
                                        else
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.CuntGlistensSoaked".Localized());

                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 0)
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.BreastsRockUpDown".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.SentencePeriod".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.RepeatProcessOils".Localized());

                                        OutputText("Scenes.Rubi.GiveRubiATFItem.OiledFromNeckDown".Localized());

                                        if ((CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 1) && (!hyper))
                                        {
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.GaspsBreastsShrinkAgain".Localized());

                                            CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) - 1);

                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                                                CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 1);
                                        }
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                                            {
                                                OutputText("Scenes.Rubi.GiveRubiATFItem.GaspsBreastsSwellAgain".Localized());

                                                CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) + 1);

                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 1)
                                                    CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 1);
                                            }
                                            else
                                                if ((CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 3) && (!hyper))
                                                {
                                                    OutputText("Scenes.Rubi.GiveRubiATFItem.CockShrinksAfterSwell".Localized());

                                                    CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) - 1);

                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 3)
                                                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 3);

                                                    OutputText("Scenes.Rubi.GiveRubiATFItem.InchPenisThird".Localized(
                                                        (float)(CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) * 10) / 10
                                                    ));
                                                }
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBallsType) == 0)
                                                    {
                                                        OutputText("Scenes.Rubi.GiveRubiATFItem.OilsSeepGlossy".Localized());

                                                        CoC.Instance.Flags.Set(GameFlagIds.RubiBallsType, 1);
                                                    }
                                                    else
                                                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiNippleType) != 1)
                                                        {
                                                            OutputText("Scenes.Rubi.GiveRubiATFItem.HandsToNipplesPerk".Localized());

                                                            CoC.Instance.Flags.Set(GameFlagIds.RubiNippleType, 1);
                                                        }
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEyeType) != 1)
                                                            {
                                                                OutputText("Scenes.Rubi.GiveRubiATFItem.BlinksTwice".Localized());

                                                                CoC.Instance.Flags.Set(GameFlagIds.RubiEyeType, 1);
                                                            }
                                                            else
                                                                OutputText("Scenes.Rubi.GiveRubiATFItem.NothingSeemsHappen".Localized());
                                    }
                                    else
                                        if (itype == ConsumableIds.W_FRUIT)
                                        {
                                            OutputText("Scenes.Rubi.GiveRubiATFItem.HandOverTinyPeach".Localized());

                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) != 1)
                                            {
                                                OutputText("Scenes.Rubi.GiveRubiATFItem.HandsToHeadKneel".Localized());

                                                CoC.Instance.Flags.Set(GameFlagIds.RubiEarType, 1);
                                            }
                                            else
                                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiWhiskers) != 1 || CoC.Instance.Flags.Get(GameFlagIds.RubiHands) != 1)
                                                {
                                                    OutputText("Scenes.Rubi.GiveRubiATFItem.HandsToFaceWhiskers".Localized());

                                                    CoC.Instance.Flags.Set(GameFlagIds.RubiWhiskers, 1);

                                                    CoC.Instance.Flags.Set(GameFlagIds.RubiHands, 1);
                                                }
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiLowerbody) != 1)
                                                    {
                                                        OutputText("Scenes.Rubi.GiveRubiATFItem.SitsDownLegPain".Localized());

                                                        CoC.Instance.Flags.Set(GameFlagIds.RubiLowerbody, 1);
                                                    }
                                                    else
                                                        if (this.RubiGetCockType() != BodyPartTypes.Cat && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0)
                                                        {
                                                            OutputText("Scenes.Rubi.GiveRubiATFItem.CockBarbsSprout".Localized());

                                                            CoC.Instance.Flags.Set(GameFlagIds.RubiCockType, BodyPartTypes.Index(BodyPartTypes.Cat));
                                                        }
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 2)
                                                            {
                                                                OutputText("Scenes.Rubi.GiveRubiATFItem.StomachWhiteFur".Localized());

                                                                CoC.Instance.Flags.Set(GameFlagIds.RubiSkin, 2);
                                                            }
                                        }

            if (itype == ItemIds.PurePeach)
            {
                OutputText("Scenes.Rubi.GiveRubiATFItem.PeachesTogetherTransform".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.ApronTiesOn".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.PieBakingCouch".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.InhalesSmellsGood".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.DeclinePeachesForHer".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.ReturnsToCouch".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                    OutputText("Scenes.Rubi.GiveRubiATFItem.SkinWordThird".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1)
                        OutputText("Scenes.Rubi.GiveRubiATFItem.RedSkinWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                            OutputText("Scenes.Rubi.GiveRubiATFItem.FurredSkinWord".Localized());
                        else
                            OutputText("Scenes.Rubi.GiveRubiATFItem.StripedSkinWord".Localized());

                OutputText("Scenes.Rubi.GiveRubiATFItem.DemonMarkedRubs".Localized(
                    RubiMF("boy", "girl")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) != 2 || CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 3 || (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0 && this.RubiGetCockType() != BodyPartTypes.Anemone))
                {
                    OutputText("Scenes.Rubi.GiveRubiATFItem.EyesDartBelly".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) != 2)
                    {
                        OutputText("Scenes.Rubi.GiveRubiATFItem.HandsDartToEars".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiEarType) == 0)
                            OutputText("Scenes.Rubi.GiveRubiATFItem.NormalWord".Localized());
                        else
                            OutputText("Scenes.Rubi.GiveRubiATFItem.FelineWord".Localized());

                        OutputText("Scenes.Rubi.GiveRubiATFItem.EarsPullIntoSkull".Localized());

                        OutputText("Scenes.Rubi.GiveRubiATFItem.EarsBlossomOutward".Localized());

                        OutputText("Scenes.Rubi.GiveRubiATFItem.PokesTwitchCutely".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.RubiEarType, 2);
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) != 3)
                        {
                            OutputText("Scenes.Rubi.GiveRubiATFItem.ClampDownStomach".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 0)
                                OutputText("Scenes.Rubi.GiveRubiATFItem.PalenessCherryRed".Localized());
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 1)
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.BarelyNoticeableRed".Localized());
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiSkin) == 2)
                                        OutputText("Scenes.Rubi.GiveRubiATFItem.FurRetreatingRed".Localized());

                            OutputText("Scenes.Rubi.GiveRubiATFItem.UnevenCherryRed".Localized());

                            OutputText("Scenes.Rubi.GiveRubiATFItem.ChangesStopSlow".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.RubiSkin, 3);
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 0 && this.RubiGetCockType() != BodyPartTypes.Anemone)
                            {
                                OutputText("Scenes.Rubi.GiveRubiATFItem.HandsDartToCrotch".Localized());

                                if (this.RubiGetCockType() != BodyPartTypes.Human)
                                    OutputText("Scenes.Rubi.GiveRubiATFItem.ShiftingMoreHuman".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.NodulesAroundCrown".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.NubsGrowTentacles".Localized());

                                OutputText("Scenes.Rubi.GiveRubiATFItem.AllSheCanSay".Localized());
                            }

                    CoC.Instance.Flags.Set(GameFlagIds.RubiCockType, BodyPartTypes.Index(BodyPartTypes.Anemone));
                }
                else
                    OutputText("Scenes.Rubi.GiveRubiATFItem.MonitorNoEffect".Localized());

                Player.ConsumeItem(itype, 1);

                Player.ConsumeItem(itype, 1);

                Player.ConsumeItem(itype, 1);

                Player.ConsumeItem(itype, 1);
            }

            Player.ConsumeItem(itype, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PickAnItemToFeedRubi);
        }

        public void RubiGrowPlusBreasts()
        {
            ClearOutput();

            ShowImage("rubi-gro-plus-tits");

            RubiSprite();

            OutputText("Scenes.Rubi.RubiGrowPlusBreasts.HandOverBlueVial".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 7 || (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 30 && hyper))
            {
                OutputText("Scenes.Rubi.RubiGrowPlusBreasts.BreastsWobbleSwell".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) + 2 + Random.Shared.Next(2));

                if (hyper)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 30)
                        CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 30);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 7)
                        CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 7);
            }
            else
                OutputText("Scenes.Rubi.RubiGrowPlusBreasts.NipplesPerkyNoEffect".Localized());

            Player.ConsumeItem(ItemIds.GroPlus);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PickAnItemToFeedRubi);
        }

        public void RubiPenisGroPlus()
        {
            ClearOutput();

            ShowImage("rubi-gro-plus-penis");

            RubiSprite();

            OutputText("Scenes.Rubi.RubiPenisGroPlus.HandOverGreenVial".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 12 || (this.RubiGetCockType() == BodyPartTypes.Horse && CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 20) || (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) < 30 && hyper))
            {
                OutputText("Scenes.Rubi.RubiPenisGroPlus.CockHardensInGrip".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) + 2 + Random.Shared.Next(2));

                if (hyper)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 30)
                        CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 30);
                }
                else
                    if (this.RubiGetCockType() != BodyPartTypes.Horse)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 12)
                            CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 12);
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) > 20)
                            CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, 20);
            }
            else
            {
                OutputText("Scenes.Rubi.RubiPenisGroPlus.CockHardNoEffect".Localized());

                if (this.RubiGetCockType() == BodyPartTypes.Horse)
                    OutputText("Scenes.Rubi.RubiPenisGroPlus.SheathStuffingHorseDick".Localized());
                else
                    OutputText("Scenes.Rubi.RubiPenisGroPlus.GiantDickSentence".Localized());
            }

            Player.ConsumeItem(ItemIds.GroPlus);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PickAnItemToFeedRubi);
        }

        public void RubiBoobsReducto()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.RubiBoobsReducto.HandOverPasteJar".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) > 0)
            {
                OutputText("Scenes.Rubi.RubiBoobsReducto.BreastsShrinkSlowly".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) - 1);
            }

            Player.ConsumeItem(ItemIds.Reducto);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PickAnItemToFeedRubi);
        }

        public void RubiPenisReducto()
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.RubiPenisReducto.HandOverPasteJar".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) >= 3)
            {
                OutputText("Scenes.Rubi.RubiPenisReducto.PenisShrinkSlowly".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.RubiCockSize, CoC.Instance.Flags.Get(GameFlagIds.RubiCockSize) - 1);
            }
            else
                OutputText("Scenes.Rubi.RubiPenisReducto.NoVisibleEffect".Localized());

            Player.ConsumeItem(ItemIds.Reducto);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PickAnItemToFeedRubi);
        }

        public void GiftRubiClothes()
        {
            ClearOutput();

            RubiSprite();

            var btnData = new List<(string Label, Action Act, bool Has)>();

            var closet = new List<string>();

            var gifts = new List<string>();

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSuitclothes) == 1)
                closet.Add("Suitclothes");
            else
            {
                gifts.Add("Suitclothes");

                btnData.Add(("Suitclothes", () => GiveRubiClothes(ArmorIds.SuitClothes), Player.HasItem(ArmorIds.SuitClothes)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiFetishClothes) == 1)
                closet.Add("Rubber Fetish Clothes");
            else
            {
                gifts.Add("Rubber Fetish Clothes");

                btnData.Add(("Rubberclothes", () => GiveRubiClothes(ArmorIds.FetishWear), Player.HasItem(ArmorIds.FetishWear)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiGreenAdventurer) == 1)
                closet.Add("A Green Adventurer's Outfit");
            else
            {
                gifts.Add("A Green Adventurer's Outfit");

                btnData.Add(("Green Clothes", () => GiveRubiClothes(ArmorIds.KokiriClothes), Player.HasItem(ArmorIds.KokiriClothes)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiTubeTop) == 1)
                closet.Add("A Tube Top");
            else
            {
                gifts.Add("A Tube Top");

                btnData.Add(("Tube Top", () => GiveRubiClothes(ArmorIds.TubeTop), Player.HasItem(ArmorIds.TubeTop)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBodysuit) == 1)
                closet.Add("A Sheer Bodysuit");
            else
            {
                gifts.Add("A Sheer Bodysuit");

                btnData.Add(("Bodysuit", () => GiveRubiClothes(ArmorIds.Bodysuit), Player.HasItem(ArmorIds.Bodysuit)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiLongdress) == 1)
                closet.Add("A Long Dress");
            else
            {
                gifts.Add("A Long Dress");

                btnData.Add(("Long Dress", () => GiveRubiClothes(ArmorIds.BallroomDress), Player.HasItem(ArmorIds.BallroomDress)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiTightPants) == 1)
                closet.Add("A Dashing Outfit With Tight Leather Pants");
            else
            {
                gifts.Add("A Dashing Outfit With Tight Leather Pants");

                btnData.Add(("Leather Pants", () => GiveRubiClothes(ArmorIds.LeatherPants), Player.HasItem(ArmorIds.LeatherPants)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNurseClothes) == 1)
                closet.Add("Nurse's Clothes");
            else
            {
                gifts.Add("Nurse's Clothes");

                btnData.Add(("Nurse Clothes", () => GiveRubiClothes(ArmorIds.NurseOutfit), Player.HasItem(ArmorIds.NurseOutfit)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiSwimwear) == 1)
                closet.Add("Slutty Swimwear");
            else
            {
                gifts.Add("Slutty Swimwear");

                btnData.Add(("SluttySwim", () => GiveRubiClothes(ArmorIds.SkimpySwimwear), Player.HasItem(ArmorIds.SkimpySwimwear)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBimboMinidress) == 1)
                closet.Add("A Bimbo Minidress");
            else
            {
                gifts.Add("A Bimbo Minidress");

                btnData.Add(("BimboSk", () => GiveRubiClothes(ArmorIds.BimboOutfit), Player.HasItem(ArmorIds.BimboOutfit)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBondageStraps) == 1)
                closet.Add("Bondage Straps");
            else
            {
                gifts.Add("Bondage Straps");

                btnData.Add(("Bondage S.", () => GiveRubiClothes(ArmorIds.BondageStraps), Player.HasItem(ArmorIds.BondageStraps)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiInquisitorsCorset) == 1)
                closet.Add("An Inquisitor's Corset");
            else
            {
                gifts.Add("An Inquisitor's Corset");

                btnData.Add(("I. Corset", () => GiveRubiClothes(ArmorIds.InquisitorsCorset), Player.HasItem(ArmorIds.InquisitorsCorset)));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiAffection) >= 100)
                closet.Add("A Risque Waitress's Uniform");
            else
                gifts.Add("A Risque Waitress's Uniform");

            if (closet.Count > 0)
            {
                OutputText("Scenes.Rubi.GiftRubiClothes.RubiClosetHeader".Localized());

                var x = 0;

                while (x < closet.Count)
                {
                    OutputText("Scenes.Rubi.GiftRubiClothes.NewlinePlaceholderToken".Localized(
                        closet[x]
                    ));

                    x += 1;
                }

                OutputText("Scenes.Rubi.GiftRubiClothes.ParagraphBreak".Localized());
            }

            if (gifts.Count > 0)
            {
                OutputText("Scenes.Rubi.GiftRubiClothes.GlanceAtRubiThings".Localized(
                    gifts[Random.Shared.Next(gifts.Count)]
                ));

                OutputText("Scenes.Rubi.GiftRubiClothes.WhatWillYouGive".Localized());
            }
            else
            {
                OutputText("Scenes.Rubi.GiftRubiClothes.GivenAllClothes".Localized());

                AwardAchievement(AchievementIds.GeneralDresstacular);

                OutputText("Scenes.Rubi.GiftRubiClothes.ParagraphBreakAgain".Localized());
            }

            var btnIndex = 0;

            foreach (var (Label, Act, Has) in btnData)
            {
                if (Has)
                    AddButton(btnIndex, Label, Act);
                else
                    AddButtonDisabled(btnIndex, Label, "You don't have that item.");

                btnIndex++;
            }

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void PickAnItemToFeedRubi()
        {
            ClearOutput();

            RubiSprite();

            var btnData = new List<(string Label, Action Act, bool Has)>();

            ClearButtons();

            if (!RubiBimbo() && !RubiIncubus() && RubiAffection() >= 100)
                OutputText("Scenes.Rubi.PickAnItemToFeedRubi.OfferIncubiDraft".Localized());

            OutputText("Scenes.Rubi.PickAnItemToFeedRubi.WhatWillYouGive".Localized());

            if (!RubiBimbo())
                btnData.Add(("Bimbo Liq", () => GiveRubiATFItem(ItemIds.BimboLiqueur), Player.HasItem(ItemIds.BimboLiqueur)));

            btnData.Add(("Incubi Draft", () => GiveRubiATFItem(ConsumableIds.IncubiDraft), Player.HasItem(ConsumableIds.IncubiDraft)));

            btnData.Add(("Pure Draft", () => GiveRubiATFItem(ConsumableIds.PurifiedIncubiDraft), Player.HasItem(ConsumableIds.PurifiedIncubiDraft)));

            btnData.Add(("Succubi Milk", () => GiveRubiATFItem(ConsumableIds.SuccubiMilk), Player.HasItem(ConsumableIds.SuccubiMilk)));

            btnData.Add(("P.SuccubiMilk", () => GiveRubiATFItem(ConsumableIds.PurifiedSuccubiMilk), Player.HasItem(ConsumableIds.PurifiedSuccubiMilk)));

            btnData.Add(("Pure Peach", () => GiveRubiATFItem(ItemIds.PurePeach), Player.HasItem(ItemIds.PurePeach)));

            btnData.Add(("Equinum", () => GiveRubiATFItem(ItemIds.Equinum), Player.HasItem(ItemIds.Equinum)));

            btnData.Add(("Whisker Fruit", () => GiveRubiATFItem(ConsumableIds.W_FRUIT), Player.HasItem(ConsumableIds.W_FRUIT)));

            btnData.Add(("Reducto", () => GiveRubiATFItem(ItemIds.Reducto), Player.HasItem(ItemIds.Reducto)));

            btnData.Add(("Gro+", () => GiveRubiATFItem(ItemIds.GroPlus), Player.HasItem(ItemIds.GroPlus)));

            btnData.Add(("Trap Oil", () => GiveRubiATFItem(ConsumableIds.TrapOil), Player.HasItem(ConsumableIds.TrapOil)));

            var btnIndex = 0;

            foreach (var (Label, Act, Has) in btnData)
            {
                if (Has)
                    AddButton(btnIndex, Label, Act);
                else
                    AddButtonDisabled(btnIndex, Label, "You don't have that item.");

                btnIndex++;
            }

            AddButton(14, "Common.Button.Back".Localized(), RubiAppearance);
        }

        public void GiveRubiClothes(Identifier itype = default)
        {
            ClearOutput();

            RubiSprite();

            OutputText("Scenes.Rubi.GiveRubiClothes.HandOverSpareClothes".Localized());

            OutputText("Scenes.Rubi.GiveRubiClothes.RubiLovesNewOutfit".Localized());

            OutputText("Scenes.Rubi.GiveRubiClothes.WonderingAtExcitement".Localized());

            Player.ConsumeItem(itype, 1);

            if (itype == ArmorIds.SuitClothes)
                CoC.Instance.Flags.Set(GameFlagIds.RubiSuitclothes, 1);
            else
                if (itype == ArmorIds.FetishWear)
                    CoC.Instance.Flags.Set(GameFlagIds.RubiFetishClothes, 1);
                else
                    if (itype == ArmorIds.KokiriClothes)
                        CoC.Instance.Flags.Set(GameFlagIds.RubiGreenAdventurer, 1);
                    else
                        if (itype == ArmorIds.TubeTop)
                            CoC.Instance.Flags.Set(GameFlagIds.RubiTubeTop, 1);
                        else
                            if (itype == ArmorIds.Bodysuit)
                                CoC.Instance.Flags.Set(GameFlagIds.RubiBodysuit, 1);
                            else
                                if (itype == ArmorIds.BallroomDress)
                                    CoC.Instance.Flags.Set(GameFlagIds.RubiLongdress, 1);
                                else
                                    if (itype == ArmorIds.LeatherPants)
                                        CoC.Instance.Flags.Set(GameFlagIds.RubiTightPants, 1);
                                    else
                                        if (itype == ArmorIds.NurseOutfit)
                                            CoC.Instance.Flags.Set(GameFlagIds.RubiNurseClothes, 1);
                                        else
                                            if (itype == ArmorIds.S_SWMWR)
                                                CoC.Instance.Flags.Set(GameFlagIds.RubiSwimwear, 1);
                                            else
                                                if (itype == ArmorIds.InquisitorsCorset)
                                                    CoC.Instance.Flags.Set(GameFlagIds.RubiInquisitorsCorset, 1);
                                                else
                                                    if (itype == ArmorIds.BondageStraps)
                                                        CoC.Instance.Flags.Set(GameFlagIds.RubiBondageStraps, 1);
                                                    else
                                                        if (itype == ArmorIds.BimboOutfit)
                                                            CoC.Instance.Flags.Set(GameFlagIds.RubiBimboMinidress, 1);

            RubiAffection(20);

            RubiAppearance();
        }

        public void HypnoBimboficationForRubiSloots()
        {
            ClearOutput();

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.SlitherAndFlickTongue".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiBreastSize) < 1)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FemmyBoyWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                    OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.DickGirlWord".Localized());
                else
                    OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.LittleSlutWord".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.LustsAfterYourCock".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiWatchesOpenMouthed".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.SLetter".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.CockSwaysWithMovements".Localized());

            if (SceneInstance<JojoScene>.Get().IsJojoCorrupted())
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FamiliarWord".Localized());
            else
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.StrangeWord".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.DarkPowerWellingUp".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiPowerlessHypnotized".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FemboyWord".Localized());
            else
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.HermaphroditeWord".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.LongStareSlackJaw".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.SootheWithComfortingWords".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.HeMouthsTrust".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.SlideCloserAndThen".Localized());

            if (Player.Tallness >= 72)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.BendDownOption".Localized());
            else
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.LeanForwardOption".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FacesAlmostTouching".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.CannotSpareAGlance".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.GazeIntoPliantEyes".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.YouCallRubiAttention".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.MumbledPraiseAnswer".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FocusOnTheirPower".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.QuietMoanedReply".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.InstructsEyesInside".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiPantsTwitching".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FemboyWordAgain".Localized());
            else
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.DickGirlWord".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.MustHaveDirtyMind".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.TailWrapsHisArms".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.BeyondWordsLeadIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) == 0)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.DickPrefix".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.GirlSuffix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.YBoySuffix".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.SlightestNodAsReply".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.AskAboutHappyForever".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiNodsVigorously".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.PredatorySmileHint".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.FaceSlackensFurther".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.HelpYouFixThat".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiDroolsYes".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.GoodBoyCloseEyes".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.SilentImmediateChange".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.MindBecomesPlayground".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.PettingCockOneHanded".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.WhisperShamelessSlut".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.ClenchedJawMoans".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.UntouchedSighClimax".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.PuddleAndStillHard".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.StillSomethingMissing".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.BimboCumsSecondTime".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiRegainsConsciousness".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.HornyJustLooking".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiSmilesEagerly".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.RubiNowLabel".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiNoCunt) > 0)
                OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.ShemaleWord".Localized());

            OutputText("Scenes.Rubi.HypnoBimboficationForRubiSloots.BimboLustyAirheaded".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RubiBimbo, 1);

            CoC.Instance.Flags.Set(GameFlagIds.RubiShe, 1);

            CoC.Instance.Flags.Set(GameFlagIds.RubiBreastSize, 5);

            Player.ApplyStats(new DynStat(GameStats.Lust, 1000f), new DynStat(GameStats.Corruption, 10f));

            RubiSexMenu();

            RubiAffection(100);
        }

    }
}