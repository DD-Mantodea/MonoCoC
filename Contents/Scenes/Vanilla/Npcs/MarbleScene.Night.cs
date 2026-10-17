using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    public partial class MarbleScene
    {

        public const int PregnancyNoticeMask = 0x7FFF0000; 

        public const int PregnancyPlayerType = 25;         

        public const int PregnancyOvielixirEggsType = 5;   

        public static int MarblePregRawType => CoC.Instance.Flags.Get(GameFlagIds.MarblePregnancyType);

        public static int MarblePregType => MarblePregRawType & 0x0000FFFF; 

        public static int MarblePregIncubation => CoC.Instance.Flags.Get(GameFlagIds.MarblePregnancyIncubation);

        public static int MarblePregnancyEvent()
        {
            var type = MarblePregType;

            if (type == 0)
                return 0;

            if (type == PregnancyPlayerType)
            {
                int[] events = [648, 528, 432, 288, 144];

                for (var i = 0; i < events.Length; i++)
                    if (MarblePregIncubation > events[i])
                        return i + 1;

                return events.Length + 1;
            }

            return 1;
        }

        public static int MarblePregnancyEventTriggered()
        {
            var stage = MarblePregnancyEvent();

            var lastNoticed = MarblePregRawType & PregnancyNoticeMask;

            var currentNoticed = stage * 65536;

            if (currentNoticed == lastNoticed)
                return 0;

            CoC.Instance.Flags.Set(GameFlagIds.MarblePregnancyType, MarblePregType + currentNoticed);

            return stage;
        }

        public static string MarbleManWoman()
        {
            if (Player.CockTotal() > 0)
                return Player.HasVagina() ? "futa" : "man";

            return Player.HasVagina() ? "woman" : "eunuch";
        }

        public void MarbleCampSexNew()
        {

            MarbleSprite(true);

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                OutputText("Scenes.Marblescene.MarbleCampSexNew.CallMarbleForRelease".Localized());

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 0)
            {
                OutputText("Scenes.Marblescene.MarbleCampSexNew.AskMarbleReleaseSourly".Localized());

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, 0);

            if (!Player.HasTailInsteadOfLegs())
            {
                if (Player.HasPerk(PerkIds.MarblesMilk) && (int)Player.Gender > 0 && Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.AdmitHorninessToMarble".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.CuteSweetieAcceptance".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.LeadToComfySeat".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.BareLowerArmorLie".Localized());

                    if (Player.Tallness < 66)
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.RightBreast".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.LeftBreast".Localized());

                    if (Player.Tallness > 90)
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.BendOverBreastAccess".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.MarbleHandDownAbdomen".Localized(
                        MarbleNips()
                    ));

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.DrinkOfMeCommand".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckleTeatMilkFlows".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ChuckleReadyEagerCocks".Localized());
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ChuckleReadyPlaceholder".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.Wetness() < 2)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ChideLackOfEagerness".Localized());
                        else
                            if (Player.Wetness() < 4)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.NeedSomethingDownHere".Localized());
                            else
                                if (Player.Wetness() < 5)
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.QuestionWetnessStimulus".Localized());
                                else
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.NeedTowelOffRemark".Localized());
                    }

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.SmileThenBeginStroking".Localized());

                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.GentleStrokesIntensify".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.DoesItHurtAssurance".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MoanIntoBreastGiggle".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) == 0)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighsRubbingSeemWet".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighsRubHardPoking".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ReachCockBaseRoll".Localized());

                        if (Player.HasVagina())
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.LowerToClitRubbing".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.TugStrokeForCum".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.StrokingBecomesUnrelenting".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.RapidStrokesEjaculation".Localized());

                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.CoverHandKeepJerking".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.LargeLoadRopesFly".Localized());
                            else
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.EnormousLoadCoversGround".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.PullAwayFromBreast".Localized());

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.WipeNippleOnOther".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckOtherBreastPlea".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.HandOnClitRub".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EncourageFurtherSuckling".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MoanIntoNippleGiggle".Localized());

                        if (Player.GetClitLength() < 3)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.CarefulIntenseRubbing".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.GrabClitPerverseGrin".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ReturnToFingerPumping".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) == 0)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighRubbingWetAgain".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThighRubHardAgain".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EdgeAlreadyFingerRub".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.Wetness() >= 4)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.MessyNeverStopsOrgasm".Localized());

                        if (Player.Wetness() > 4)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FemcumShootsOnArm".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.FunStatesPullAway".Localized());
                    }

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.RiseSitBesideNursing".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.MilkFlowDiminishes".Localized());

                    Player.SlimeFeed();

                    CoC.Instance.Flags.Set(GameFlagIds.MarbleLust, CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) + 20);
                }
                else
                    if (Random.Shared.Next(2) == 0 && (int)Player.Gender > 0)
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.WalkUpToLover".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TentGrowingInArmor".Localized());
                        else
                            if (Player.HasVagina())
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.MoistnessUnderArmor".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.TenderEyesStare".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.UndressEachOther".Localized());

                        if (Player.HasCock())
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ThrobsWord".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.ClitQuiverArousal".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SentencePeriodParagraphBreak".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.KinkyLookCoos".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.WarmthReplacesHands".Localized());

                        if (Player.HasVagina() && !Player.HasCock())
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TongueProbeAppropriate".Localized(
                                Player.VaginaDescript(0)
                            ));
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TongueWrapRodPrecum".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.AmazingJobThrashing".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MarbleKeepsGoing".Localized());

                        if (Player.HasVagina() && !Player.HasCock())
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.RelentlessCuntProbing".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckleUrethraJuices".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.AlternateSuckAndJerk".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.BallsChurnArousal".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EcstasyMoanGrabHead".Localized());

                        if (Player.Gender == Gender.Female)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.TongueFuckVerb".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.DeepthroatVerb".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.HipsThrustOnTongue".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.LipsEncloseCockTight".Localized());

                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.StrokeAnotherCock".Localized());

                            OutputText("Scenes.Marblescene.MarbleCampSexNew.OrganicCockRing".Localized());
                        }
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FirmTongueLikeDong".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.LusciousMinistrationsTooMuch".Localized());

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.CockTwitchesInThroat".Localized());

                            if (Player.HasPerk(PerkIds.MarblesMilk))
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.UsuallyWord".Localized());
                            else
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.UsedToWord".Localized());

                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SuckleHerTits".Localized());

                            if (Player.CumQ() >= 500)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.SeedSpoutDistendBelly".Localized());

                            if (Player.CumQ() > 1500)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.ReadyToGiveBirth".Localized());

                            OutputText("Scenes.Marblescene.MarbleCampSexNew.LipsClingCumTube".Localized());
                        }
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.QuiverBeforeFemSpunk".Localized(
                                Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.KeepSuckingAfterOrgasm".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.GetsUpTiredKiss".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.AntsyMarbleAsk".Localized());

                        if (Player.Corruption + Player.StatusEffectValue(StatusEffectIds.Marble, 4) < 66)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FindSecludedCampSpot".Localized());
                        else
                            if (Player.Corruption + Player.StatusEffectValue(StatusEffectIds.Marble, 4) < 132)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.StripDownMidCamp".Localized());
                            else
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.LookForThirdNobody".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SmilePushBreastsTogether".Localized());

                        if (Player.BiggestBreastSize() > 3)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.PressBreastsOnPlaceholder".Localized(
                                Appearance.BiggestBreastSizeDescript(Player),

                                MarbleNips()
                            ));
                        else
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.PressBreastsOnChest".Localized());

                        if (Player.Cocks.Count > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SlipPlaceholderBetweenBreasts".Localized(
                                Player.CockDescript(0)
                            ));

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.UnsatisfiedEngulfRest".Localized());
                        }

                        if (Player.Vaginas.Count > 0)
                        {
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.SlideDownBodyCaress".Localized(
                                Player.VaginaDescript(0),

                                MarbleNips(),

                                Player.VaginaDescript(0)
                            ));

                            if (Player.GetClitLength() < 2)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.BreastsDownBothLegs".Localized(
                                    Player.VaginaDescript(0)
                                ));
                            else
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.ClitBetweenBreastPump".Localized());
                        }

                        if (Player.Gender == Gender.Genderless)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.FoundWayMakeCum".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.OverwhelmingOrgasmRacks".Localized());

                        if (Player.Cocks.Count > 0)
                            if (Player.CumQ() < 25)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.SmallCumAmountBreasts".Localized());
                            else
                                if (Player.CumQ() < 250)
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.FairCumAmountFace".Localized());
                                else
                                    if (Player.CumQ() < 500)
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.KeepCummingUntilDrenched".Localized());
                                    else
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.EndlessOrgasmBothSoaked".Localized());

                        if (Player.Vaginas.Count > 0)
                            if (Player.Vaginas[0].Wetness < 2)
                                OutputText("Scenes.Marblescene.MarbleCampSexNew.SmallGirlyFluidLeak".Localized());
                            else
                                if (Player.Vaginas[0].Wetness <= 3)
                                    OutputText("Scenes.Marblescene.MarbleCampSexNew.LiberalGirlyFluid".Localized());
                                else
                                    if (Player.Vaginas[0].Wetness < 5)
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.DecentlyDrenchedFluid".Localized());
                                    else
                                        OutputText("Scenes.Marblescene.MarbleCampSexNew.MinuteToRecoverBlush".Localized());

                        if (Player.Gender == Gender.Genderless)
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.DifferentYetOrgasm".Localized());

                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SatisfiedStandRiverWash".Localized());
                    }
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleCampSexNew.ApproachAskSexualRelease".Localized());

                OutputText("Scenes.Marblescene.MarbleCampSexNew.LongJourneyEnjoyTrip".Localized());

                OutputText("Scenes.Marblescene.MarbleCampSexNew.TailMassageOffer".Localized());

                if (Player.Libido >= 50)
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.HookClothingHemQuietly".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) > 0)
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.AndPlaceholder".Localized(
                        MarbleCock()
                    ));

                OutputText("Scenes.Marblescene.MarbleCampSexNew.ShamelessMockOffense".Localized(
                    Player.Mf("pervert", "hussy")
                ));

                OutputText("Scenes.Marblescene.MarbleCampSexNew.ReturnToTailCrawl".Localized());

                if ((int)Player.Gender > 0)
                {
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.ReachSlitPanting".Localized());

                    if (Player.HasCock())
                    {
                        OutputText(Player.MultiCockDescriptLight());

                        if (Player.HasVagina())
                            OutputText("Scenes.Marblescene.MarbleCampSexNew.AndWord".Localized());
                    }

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.TreasureAlreadyHere".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.GrabsYourShaft".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.PenetratesPussy".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.LongTongueLickPhrase".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.TipWord".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.ClitWord".Localized());

                    OutputText("Scenes.Marblescene.MarbleCampSexNew.CryPleasureTailWrap".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.SemenLoadIntoMouth".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Marblescene.MarbleCampSexNew.PussySquirtsValley".Localized());
                }
                else
                    OutputText("Scenes.Marblescene.MarbleCampSexNew.WhereGenitalsWouldBe".Localized());

                OutputText("Scenes.Marblescene.MarbleCampSexNew.ReleaseGripHelpStand".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));
        }

        public string MarbleNips()
        {

            if (!Player.HasStatusEffect(StatusEffectIds.MarbleSpecials))
                Player.CreateStatusEffect(StatusEffectIds.MarbleSpecials, 0, 1, 0, 0);

            if (Player.StatusEffectValue(StatusEffectIds.MarbleSpecials, 1) == 0)
                Player.ChangeStatusValue(StatusEffectIds.MarbleSpecials, 2, 1);

            if (Player.StatusEffectValue(StatusEffectIds.MarbleSpecials, 1) == 1)
                return "nipples";

            if (Player.StatusEffectValue(StatusEffectIds.MarbleSpecials, 1) == 4)
                return "quad-nipples";

            return "nipples(MARBLE NIP ERROR)";
        }

        public string MarbleCock()
        {

            var descript = "";

            int rando = 0;

            if (Random.Shared.Next(3) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 3)
                    descript = "squat ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 3 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 5)
                    descript = "short ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 7)
                    descript = "average ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 7 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 9)
                    descript = "long ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 13)
                    descript = "huge ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 13 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 18)
                    descript = "massive ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 18 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) < 30)
                    descript = "enormous ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) >= 30)
                    descript = "towering ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) <= .75f)
                    descript += "narrow ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) > 1 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) <= 1.4f)
                    descript += "ample ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) > 1.4f && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) <= 2)
                    descript += "broad ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) > 2 && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) <= 3.5f)
                    descript += "fat ";

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickThickness) > 3.5f)
                    descript += "distended ";
            }

            rando = Random.Shared.Next(10);

            if (rando >= 0 && rando <= 4)
                descript += "cock";

            if (rando == 5 || rando == 6)
                descript += "prick";

            if (rando == 7)
                descript += "pecker";

            if (rando > 7)
                descript += "shaft";

            return descript;
        }

        public partial void RapeDAHMARBLEZ()
        {
            ClearOutput();

            var raped = false;

            OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.DecideToRoughRape".Localized());

            if (Player.HasPerk(PerkIds.Evade))
            {
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.DuckUnderSlap".Localized());

                raped = true;
            }
            else
                if (Player.Strength >= 80)
                {
                    OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.PushHerOntoBed".Localized());

                    raped = true;
                }

            if (!raped)
            {
                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.SheForcesYouOut".Localized());

                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ResolveNotUnderestimate".Localized());
            }
            else
            {
                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.FreeCocksFromArmor".Localized());

                    if (Player.Cocks[0].Thickness < 3)
                    {
                        if (Player.Cocks[0].Length <= 8)
                            OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ForceInAsFar".Localized());
                        else
                            OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ForceInToHilt".Localized());

                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.GruntWhileThrusting".Localized());

                        if (Player.Corruption >= 33)
                            MarbleRapeCorruptFantasy();

                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.TauntThenCreampie".Localized());

                        Player.Orgasm("Dick");
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.StruggleWithGirth".Localized(
                            Player.CockDescript(0)
                        ));

                        if (Player.Corruption >= 33)
                            MarbleRapeCorruptFantasy();

                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.TauntThenPullOut".Localized());

                        Player.Orgasm("Dick");
                    }
                }
                else
                    if (Player.Vaginas.Count > 0)
                    {
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.LookAroundRoom".Localized());

                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ShareDildoWithMarble".Localized(
                            Player.VaginaDescript(0)
                        ));

                        if (Player.Corruption >= 33)
                            MarbleRapeCorruptFantasy();

                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.TauntAfterDildoOrgasm".Localized());

                        Player.Orgasm("Vaginal");
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.LackingGenitalsInstead".Localized());

                        if (Player.IsCorruptEnough(33))
                            MarbleRapeCorruptFantasy();

                        OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.ShoveFistInside".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, -20f));
                    }

                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.RevelBeforeRollover".Localized());

                Player.HealthChange(-9001);

                Player.Health = 1;

                OutputText("Scenes.Marblescene.RapeDAHMARBLEZ.WakeWithHeadache".Localized());
            }

            DoNext(() => ReturnToCamp(4));

            Player.CreateStatusEffect(StatusEffectIds.MarbleRapeAttempted, 0, 0, 0, 0);
        }

        public partial void MarbleRapeCorruptFantasy()
        {

            OutputText("Scenes.Marblescene.MarbleRapeCorruptFantasy.HelplessBeforeOnslaught".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Marblescene.MarbleRapeCorruptFantasy.ClearCorruptFantasy".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleRapeCorruptFantasy.SmileOneDayThought".Localized());
        }

        public void MarbleAfterRapeBattle()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeBattle.SpotMarbleAtBarn".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), MarbleAfterRapeYes);

            AddButton(1, "Common.Button.No".Localized(), MarbleAfterRapeNo);
        }

        public void MarbleAfterRapeYes()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeYes.ApproachBarnSpotted".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), MarbleAfterRapeStartFight);

            AddButton(1, "Common.Button.No".Localized(), MarbleAfterRapeNo);
        }

        public void MarbleAfterRapeStartFight()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeStartFight.EnterCombatStance".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "marble")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void MarbleAfterRapeNo()
        {

            OutputText("Scenes.Marblescene.MarbleAfterRapeNo.LeaveNotWorthIt".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public partial void MarbleBadEndFollowup()
        {
            ClearOutput();

            var morph = Player.Race;

            var approxHeight = "";

            if (Player.Tallness < 54)
                approxHeight = "very short";
            else
                if (Player.Tallness < 66)
                    approxHeight = "short";
                else
                    if (Player.Tallness < 72)
                        approxHeight = "average";
                    else
                        if (Player.Tallness < 84)
                            approxHeight = "tall";
                        else
                            approxHeight = "very tall";

            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.LearnToGetAlong".Localized());

            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.YoungChampionApproaches".Localized());

            if (morph == "human")
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.EyesLightUp".Localized());

                if (approxHeight != "average height")
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.PlaceholderToken".Localized(approxHeight));

                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.HumanByBarnDoor".Localized(
                    Player.Mf("male", "female"),

                    MarbleManWoman()
                ));

                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.GladToHaveFriend".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.SurprisedToSeeYou".Localized(
                    Player.Mf("male", "female"),

                    morph,

                    morph,

                    morph,

                    morph
                ));

                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.HumanityLeftBehind".Localized());
            }

            if (approxHeight == "very short" || approxHeight == "short")
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.ShorterThanLastTime".Localized());
            else
                if (approxHeight == "tall" || approxHeight == "very tall")
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.BiggerThanLastTime".Localized());

            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.JoinAtMyCamp".Localized());

            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.FemaleVoiceCallsOut".Localized());

            if (Player.Gender == Gender.Male || Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.TallCowGirlPregnant".Localized());

                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.OlderChampionIntroduces".Localized());

                if (Player.Gender == Gender.Hermaphrodite && Player.Mf("m", "f") == "f" && Player.BiggestCockArea() < 15)
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.StammersYourChild".Localized(
                        Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Marblescene.MarbleBadEndFollowup.CongratulateNewFamily".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.TallCowGirlAppears".Localized());

                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.IntroduceNewChampion".Localized());
            }

            if (KnowAddiction == 1)
                OutputText("Scenes.Marblescene.MarbleBadEndFollowup.MarbleAsksLikeMilk".Localized());

            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.ParagraphBreak".Localized());

            OutputText("Scenes.Marblescene.MarbleBadEndFollowup.AbandonQuestToMilk".Localized());

            GameOver();
        }

        public partial void MarblePoopsBaybees()
        {

            if (MarblePregType == PregnancyPlayerType)
            {
                OutputText("Scenes.Marblescene.MarblePoopsBaybees.MarbleAnnouncesBirth".Localized());

                OutputText("Scenes.Marblescene.MarblePoopsBaybees.HeadWithBovineEars".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurified) > 0 && Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.SonFullyDelivered".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) == 0)
                    {
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.HandBoyToMarble".Localized());

                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.AskWhySheHesitated".Localized());
                    }
                    else
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.ChildStopsCrying".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.SucklingJustAsWonderful".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.OffersOtherBreast".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.ChildIntoNursery".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MarbleBoys, CoC.Instance.Flags.Get(GameFlagIds.MarbleBoys) + 1);
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.DaughterFullyDelivered".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.TraitNotPassedOn".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.GirlLooksLikeMom".Localized());

                    if (Player.HasPerk(PerkIds.MarblesMilk))
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.WontGetAddicted".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.GirlSucklingWonderful".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.OtherBreastForGirl".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.GirlIntoNursery".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 0)
                    {
                        var temp = Random.Shared.Next(10);

                        switch (temp)
                        {
                            case 0:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.AuraName".Localized());
                            }

                            break;

                            case 1:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.MiranName".Localized());
                            }

                            break;

                            case 2:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.LinName".Localized());
                            }

                            break;

                            case 3:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.MaryName".Localized());
                            }

                            break;

                            case 4:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.BessName".Localized());
                            }

                            break;

                            case 5:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.TinaName".Localized());
                            }

                            break;

                            case 6:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.RillName".Localized());
                            }

                            break;

                            case 7:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.WendyName".Localized());
                            }

                            break;

                            case 8:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.RainyName".Localized());
                            }

                            break;

                            case 9:
                            {
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.NickyName".Localized());
                            }

                            break;
                        }

                        if (true )
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.GlitchyName".Localized());

                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.GoodNameForHer".Localized());
                    }
                    else
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.NameHerTomorrow".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.MarbleKids, CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) + 1);

                DoNext(PlayerMenu);
            }
            else

                if (MarblePregType == PregnancyOvielixirEggsType)
                {
                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.HearMoaningNearRocks".Localized());

                    if (Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Marblescene.MarblePoopsBaybees.SmallEggPile".Localized());
                    else
                        if (Random.Shared.Next(2) == 0)
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.LargeEggPile".Localized());
                        else
                            OutputText("Scenes.Marblescene.MarblePoopsBaybees.HugeEggPile".Localized());

                    OutputText("Scenes.Marblescene.MarblePoopsBaybees.MarbleStretchesLegs".Localized());

                    Identifier itype = default;

                    if (Random.Shared.Next(3) == 1)
                        itype = ConsumableIds.LargeEggs[Random.Shared.Next(ConsumableIds.LargeEggs.Length)];
                    else
                        itype = ConsumableIds.SmallEggs[Random.Shared.Next(ConsumableIds.SmallEggs.Length)];

                    InventoryScene.TakeItem(itype, PlayerMenu);
                }
        }

        public bool MarbleNightSleepFlavor()
        {

            MarbleSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) == 0 && Player.Pregnancy.Type == PregnancyIds.Marble && (Player.Pregnancy.Incubation <= 128))
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BuildingNurseryNotice".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MarbleNurseryConstruction, CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) + 1);
            }

            switch (MarblePregnancyEventTriggered())
            {
                case 2:
                {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.MarbleGainedWeight".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BellySlightlyBigger".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BellyObviouslyPregnant".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) == 0 && MarblePregType != PregnancyOvielixirEggsType)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.MarbleNurseryConstruction, CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) + 1);

                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.BusyWithNursery".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.CloseBoldParagraph".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.PregnancyAdvances".Localized());
                }

                break;

                case 6:
                {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.NearGivingBirth".Localized());
                }

                break;
            }

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleGrossedOutBecauseWormInfestation) != 1)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.MarbleRejectsWorms".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MarbleGrossedOutBecauseWormInfestation, 1);
                }

                return false;
            }

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.WhisperedBedRequest".Localized());

                return false;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 50 || Player.Gender == Gender.Genderless)
                {
                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.MarbleAlreadyThereShort".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 0)
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.TurnsAwayAnnoyed".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 20)
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.LiesContentedlyNearby".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 30)
                                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.SoftSighInBedroll".Localized());
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 40)
                                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.FidgetsInBedroll".Localized());
                                else
                                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.SeemsWorkedUp".Localized());

                    if (Player.Lust < 33 && CoC.Instance.Flags.Get(GameFlagIds.MarbleLust) < 20)
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.DriftOffToSleep".Localized());
                    else
                        if (Player.Lust < 33)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.WondersWhatSheWants".Localized());

                            if (Random.Shared.Next(2) == 0)
                                MarbleCuddlin();
                            else
                                OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.NotArousedEnough".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.AskMarbleForSex".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Ask".Localized(), AtNightAskMarbleForSomeSexMaybe);

                            AddButton(1, "Common.Button.DontAsk".Localized(), PlayerMenu);

                            return true;
                        }

                    OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.SpaceOnly".Localized());
                }
                else
                {
                    if (Player.Lust < 33)
                    {
                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.HornyMarbleAsksHelp".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.MarbleDickType) > 0)
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.DesperateForSucking".Localized(
                                MarbleCock()
                            ));
                        else
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.WantsWomanhoodLicked".Localized());

                        OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.OrallyPleasurePrompt".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Yes".Localized(), MarbleNomNoms);

                        AddButton(1, "Common.Button.No".Localized(), PlayerMenu);

                        if (Player.HasTailInsteadOfLegs() && CoC.Instance.Flags.Get(GameFlagIds.MarbleDickLength) > 0)
                            AddButton(2, "Common.Button.Tailjob".Localized(), GiveMarbleTailjobRelease);
                        else

                            AddButtonDisabled(2, "Tailjob", "This scene requires you to have a naga tail. Marble should have a cock.");
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.MarblePurificationStage) < 5)
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.HornyMarbleWantsSex".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Yes".Localized(), () => MarbleNightSexIntro());

                            AddButton(1, "Common.Button.No".Localized(), PlayerMenu);

                            AddButton(2, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), MarbleNomNoms);
                        }
                        else
                        {
                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.EagerLookFromMarble".Localized());

                            OutputText("Scenes.Marblescene.MarbleNightSleepFlavor.EscortToOurBed".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Lovemaking".Localized(), MarbleNightLovemaking).Hint("Have at it.");

                            AddButton(1, "Scenes.Camp.Sleep".Localized(), MarbleNightRefusal).Hint("Not tonight.");
                        }

                    return true;
                }

            return false;
        }

    }
}