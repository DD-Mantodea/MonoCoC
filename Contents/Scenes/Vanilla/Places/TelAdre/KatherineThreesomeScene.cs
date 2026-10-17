using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<KatherineThreesomeScene>("coc:katherine_threesome")]
    public class KatherineThreesomeScene : Scene
    {

        public EdrynScene edryn => SceneInstance<EdrynScene>.Get();

        public KatherineScene katherine => SceneInstance<KatherineScene>.Get();

        public UrtaScene urta => SceneInstance<UrtaScene>.Get();

        public override void Entry()
        {
            Circlejerk();
        }

        public void Circlejerk()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite(true);

            OutputText("Scenes.Katherinethreesome.Circlejerk.ReasonHaveFunTwo".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.UrtaJumpsTouch".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.Katherinethreesome.Circlejerk.WinningSmileJoiningFun".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.SmirkingCatRemovesShirt".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.KittyNoTalkingAbout".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.GropeSoftOrbsPinch".Localized(
                Player.HasBreasts() ? Player.BreastDescript(0) : "chest"
            ));

            var race = Player.Race;

            var race3 = race.Substring(0, 3);

            var race6 = race.Substring(0, 6);

            if (race3 == "cat" || race3 == "dog" || race3 == "fox" || race6 == "dragon" || race6 == "drider" || race6 == "ferret" || race6 == "spider" || race == "coc:naga" || race == "kitsune" || race == "demon-morph" || race == "shark-morph" || race.IndexOf("lizan") > -1)
                OutputText("Scenes.Katherinethreesome.Circlejerk.CarefulNotSharpTeeth".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.SentencePeriod".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.FriskyAlwaysGetReady".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.FluffyPattingGetsMood".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.Circlejerk.EachErectsNiceReady".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherinethreesome.Circlejerk.PussyDrenchedDrippingTail".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.SentencePeriodAgain".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.FeelingTimeJoinCircle".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.ChallengeVixxyCumFirst".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.Circlejerk.WrappingAroundYourCock".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.Circlejerk.GentlyTeasingAroundCunt".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.WinnerWhoeverCumsLast".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.SoundsFineMeKitty".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.WordsPourBothGirls".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.FirstGottaWinGrins".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.NeverThoughtAboutAdmits".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.KnowWhatWantHim".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.VixenShockThroatyMoan".Localized(
                katherine.CockType("canine shape, especially the swelling knot at its base,", "feline shape and the swelling knot at its base"),

                katherine.CockType("the canine-cocked cat", "Kath's shaft and knot")
            ));

            OutputText("Scenes.Katherinethreesome.Circlejerk.MoanStrugglingPleasureHaze".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.Circlejerk.HandTeasesSoftPads".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.Circlejerk.HandTeasesLabiaCircling".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.ThreeMoanGroanGrowl".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.ReachFondleUrtaBalls".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.RetortNotOnlyCock".Localized(
                Player.GenderText("cock", "cunt", "cock and cunt")
            ));

            OutputText("Scenes.Katherinethreesome.Circlejerk.NoLongerDevilishPlan".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.KathCumsFirstInexperienced".Localized(
                katherine.CockMultiple("", "twin "),

                katherine.CockType("pointy-tipped", "barbed"),

                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.Circlejerk.UrtaManagesLittleLonger".Localized());

            var kathCumQ = (int)(katherine.CumQ());

            if (kathCumQ < 500)
                OutputText("Scenes.Katherinethreesome.Circlejerk.StainsTonedBreastsSilver".Localized());
            else
                if (kathCumQ < 1500)
                    OutputText("Scenes.Katherinethreesome.Circlejerk.SoaksWaistUpProdigious".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.Circlejerk.UtterlyCoversHeadToe".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.YowlingPleasureSpunkMouth".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.LaughingGrinCumCovered".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.YouCheatedUrtaComplains".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.YeahWhatSaidKath".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.FeigningHurtNever".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.LookEachAgainGrin".Localized(
                katherine.CatGirl("of pale flesh and " + katherine.hairColor + "hair", "black"),

                katherine.CatGirl("gray", "of gray fur"),

                Player.HasBreasts() ? Player.BreastDescript(0) : "chest"
            ));

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.Circlejerk.OvercomePussyLockUp".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherinethreesome.Circlejerk.OvercomePussyLockAgain".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.Circlejerk.AnExplosion".Localized());

            if (Player.HasCock())
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Katherinethreesome.Circlejerk.RopeUponRopeWhite".Localized());
                else
                    if (Player.CumQ() < 1500)
                        OutputText("Scenes.Katherinethreesome.Circlejerk.FountainCumSpraysAir".Localized());
                    else
                        OutputText("Scenes.Katherinethreesome.Circlejerk.VeritableEruptionSoarsForth".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.TwoHermsGrowlLustily".Localized(
                Player.HasBreasts() ? Player.BreastDescript(0) : "chest"
            ));

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.Circlejerk.AndEspeciallyAgainstYour".Localized(
                    Player.CockDescript()
                ));

            OutputText("Scenes.Katherinethreesome.Circlejerk.PressedAgainstFeelPulsing".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.UrtaFirstLustyHowl".Localized());

            if (kathCumQ < 500)
                OutputText("Scenes.Katherinethreesome.Circlejerk.HerContributionLostCascade".Localized());
            else
                if (kathCumQ < 1500)
                    OutputText("Scenes.Katherinethreesome.Circlejerk.HerLoadAsBig".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.Circlejerk.HugeDredgingMassiveLoad".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.TwinTiredReliefFlop".Localized());

            OutputText("Scenes.Katherinethreesome.Circlejerk.HugCloseSharedAfterglow".Localized());

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(false);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 2);

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) != 3)
            {
                if (View.Time.Hours >= 13)
                    CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

                ReturnToCamp(1);
            }
        }

        public void ThreeSixtyNine()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite(true);

            if (katherine.IsAt(KatherineScene.KLOC_BAR) || katherine.IsAt(KatherineScene.KLOC_BAR_DRUNK))
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.DragBackroomLocksDoor".Localized());
            else
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.UrtaGrabsKathBehind".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.GetsExposingEveryInch".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.OnceNakedDropsKnees".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.UrtaSinksGroundDeeper".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.RollsTongueAroundHead".Localized(
                    katherine.CockMultiple("Katherine's cock", "the lower of Katherine's cocks")
                ));
            else
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.KatherineHipsBuriesTongue".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.KathTriesThrustBack".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.RudeNotOfferEncouragement".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.AndSucksOnKath".Localized(
                    katherine.CockMultiple(" and sucks on Kath's prick", ", switches to Kath's other cock and sucks on it")
                ));
            else
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.TrapsClitBetweenThumbs".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.LikeLollipopWhileDistracted".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.EnjoyFillingWatchingSuck".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.LookingShoulderUrtaMoan".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.ShaftInsideWatchLust".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.ZeroPeriodParagraphBreak".Localized(
                    katherine.CockMultiple("sucks down Kath's entire cock in one fluid motion", "surprises you by gobbling both of Kath's shafts")
                ));
            else
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.StartsSlurpingSuckingPussy".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.StimulationUrtaFirstCum".Localized());

            if (katherine.PregSize() > 0)
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.PregnancyEclipsedByVolume".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.FeelingFilledPushesEdge".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.ThrustsForwardDrivingKnot".Localized(
                    katherine.CockMultiple(" past Urta's lips. It", "s past Urta's lips. With two shafts to carry the load it")
                ));
            else
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.LocksAroundUrtaHead".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.AfterDesperateContribution".Localized());

            var canImpregUrta = (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1) && (!Player.IsPregnant());

            bool doKath = false;

            if (katherine.fertile && (katherine.PregSize() == 0))
            {
                if (canImpregUrta)
                    doKath = (Random.Shared.Next(2) == 0);
                else
                    doKath = true;
            }
            else
                if (canImpregUrta)
                    doKath = false;
                else
                    doKath = (Random.Shared.Next(2) == 0);

            if (doKath)
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.PullSwitchBackKath".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.BuryYourselfGive".Localized());

            if (Player.CumQ() < 1500)
            {
                if (!doKath && !katherine.HasCock())
                    OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.FlatSwellsMatchKath".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.BarelyNoticeableComparedLoad".Localized());
            }
            else
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.OverpoweredAfterBoilingJet".Localized());

            if (doKath || katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.FeelBitOfResistance".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.MostShootsBackOut".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.GiveBellyLittlePat".Localized());

            OutputText("Scenes.Katherinethreesome.ThreeSixtyNine.PullRollAwayAttend".Localized());

            if (!doKath)
                SceneInstance<UrtaScene>.Get().KnockUpUrtaChance();

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(false);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) != 3) && (View.Time.Hours >= 13))
                    CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void RoastYou()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite(true);

            if (katherine.IsAt(KatherineScene.KLOC_BAR) || katherine.IsAt(KatherineScene.KLOC_BAR_DRUNK))
                OutputText("Scenes.Katherinethreesome.RoastYou.LeadHermsTowardBackroom".Localized());
            else
            {
                OutputText("Scenes.Katherinethreesome.RoastYou.StartMoveTowardsDoor".Localized());

                OutputText("Scenes.Katherinethreesome.RoastYou.UrtaSpiesEdgingDoor".Localized());

                OutputText("Scenes.Katherinethreesome.RoastYou.SubduePerpetratorPounced".Localized());

                OutputText("Scenes.Katherinethreesome.RoastYou.GrabsKathRemovesClothes".Localized());

                OutputText("Scenes.Katherinethreesome.RoastYou.KatherineChimesWhatGives".Localized());

                if (!Player.HasVagina())
                    OutputText("Scenes.Katherinethreesome.RoastYou.YourAssWord".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.RoastYou.YourPussy".Localized(
                        katherine.CockMultiple("your pussy", "both your pussy and your ass")
                    ));

                OutputText("Scenes.Katherinethreesome.RoastYou.MayMaraeHelpInnards".Localized());
            }

            OutputText("Scenes.Katherinethreesome.RoastYou.NotPositionArgue".Localized());

            if (Player.BiggestTitRow() > 4)
                OutputText("Scenes.Katherinethreesome.RoastYou.MassagesSidesOfBreasts".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Katherinethreesome.RoastYou.MassagesYourNutsack".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Katherinethreesome.RoastYou.RunsFingerDownSlit".Localized());
                    else
                        OutputText("Scenes.Katherinethreesome.RoastYou.PlaysWithYourAsshole".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.WarmingLastClothesWall".Localized());

            if (katherine.cockNumber > 1)
                OutputText("Scenes.Katherinethreesome.RoastYou.CocksPressFirmlyAgainst".Localized());
            else
                OutputText("Scenes.Katherinethreesome.RoastYou.HerPressesFirmlyAgainst".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.SentencePeriod".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.KathPushesApartKnees".Localized());

            if (katherine.cockNumber == 1)
                OutputText("Scenes.Katherinethreesome.RoastYou.HerInchIntoYour".Localized(
                    katherine.cockLength,

                    katherine.CockType("doggy dick", "cat cock")
                ));
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherinethreesome.RoastYou.HerTwinShaftsBody".Localized(
                        katherine.CockType()
                    ));
                else
                    OutputText("Scenes.Katherinethreesome.RoastYou.HerCocksCaringLittle".Localized(
                        katherine.CockType(),

                        katherine.cockLength
                    ));

            OutputText("Scenes.Katherinethreesome.RoastYou.FirmHipsRockGently".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.NowKathHand".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.PushesLittleDeeperKnees".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.MovesCloseGesturesHorsecock".Localized(
                katherine.CockMultiple("shaft has done its work", "twin shafts have done their work")
            ));

            OutputText("Scenes.Katherinethreesome.RoastYou.GoodShoutsLoudEnough".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.RoastYou.KeepUntilPushButtons".Localized(
                katherine.CockType(),

                katherine.CockMultiple(" shlong", " meatsticks")
            ));

            if (!Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.RoastYou.YourAssWordAgain".Localized());
            else
                if (katherine.cockNumber > 1)
                    OutputText("Scenes.Katherinethreesome.RoastYou.BothYourOtherHoles".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.RoastYou.SideYourCunt".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.SentencePeriodAgain".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.WildRideHammeringOut".Localized());

            var wasButtStretched = false;

            if (katherine.cockNumber > 1)
            {
                if (Player.HasVagina())
                {
                    wasButtStretched = Player.ButtChange(katherine.CockArea(), false);

                    OutputText("Scenes.Katherinethreesome.RoastYou.KnotsSeatThemselvesBoth".Localized());
                }
                else
                {
                    wasButtStretched = Player.ButtChange(2 * katherine.CockArea(), false);

                    OutputText("Scenes.Katherinethreesome.RoastYou.TwinKnotsHammerSphincter".Localized());
                }

                OutputText("Scenes.Katherinethreesome.RoastYou.SealingYouUp".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherinethreesome.RoastYou.KnotSlidesInsideExpands".Localized());
                else
                {
                    wasButtStretched = Player.ButtChange(katherine.CockArea(), false);

                    OutputText("Scenes.Katherinethreesome.RoastYou.KnotSqueezesSphincterPops".Localized());
                }

            OutputText("Scenes.Katherinethreesome.RoastYou.ForHerComingDeposit".Localized());

            if ((Player.HasVagina()) && (Player.CuntChange(katherine.CockArea(), true)))
                    OutputText("Scenes.Katherinethreesome.RoastYou.ParagraphBreak".Localized());

            if (wasButtStretched)
            {
                Player.ButtChangeDisplay();

                OutputText("Scenes.Katherinethreesome.RoastYou.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Katherinethreesome.RoastYou.LittleTwitchReadyFire".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.FallsFloorCoughCum".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.RocksKnotBackForth".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.RoastYou.SurprisedOhFromKatherine".Localized());

            if (katherine.ballSize > 0)
                OutputText("Scenes.Katherinethreesome.RoastYou.TheseBallsNiceFull".Localized());
            else
                OutputText("Scenes.Katherinethreesome.RoastYou.ProstateHotAndSwollen".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.SoCumLetGo".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.WhispersPleaseHelpQuietly".Localized());

            switch ((int)Player.Gender)
            {
                case 0:
                {
                OutputText("Scenes.Katherinethreesome.RoastYou.YesCourseHelp".Localized());

                OutputText("Scenes.Katherinethreesome.RoastYou.DigHipsAfterJet".Localized(
                    katherine.CatGirl("nails", "claws"),

                    katherine.CockMultiple("", "s")
                ));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Katherinethreesome.RoastYou.FeelBetweenLegsClit".Localized(
                    katherine.CockMultiple("squeezing your cock", "and tight little bum squeezing your cocks")
                ));

                OutputText("Scenes.Katherinethreesome.RoastYou.PlayfulFingersCauseCum".Localized(
                    katherine.CockMultiple("", "s"),

                    katherine.CockMultiple(" throbs", " and colon throb"),

                    katherine.CockMultiple("", "s"),

                    katherine.CockMultiple(" it has", "s they have")
                ));
                }

                break;
            }

            var __sw1_0 = (int)Player.Gender;

            if (true )
            {
                OutputText("Scenes.Katherinethreesome.RoastYou.FingersWrapAroundCock".Localized(
                    katherine.CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherinethreesome.RoastYou.PlayfulFingersSlideDown".Localized(
                    katherine.CockMultiple("", "s"),

                    katherine.CockMultiple("", "s")
                ));
            }

            OutputText("Scenes.Katherinethreesome.RoastYou.KathCollapsesTopSpent".Localized());

            OutputText("Scenes.Katherinethreesome.RoastYou.MePurrsSnugglesNap".Localized(
                katherine.CockMultiple("Katherine's knot", "Kath's knots")
            ));

            Player.Orgasm("VaginalAnal");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(false);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) != 3) && (View.Time.Hours >= 13))
                    CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void SpitroastKath()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite(true);

            var atBar = katherine.IsAt(KatherineScene.KLOC_BAR) || katherine.IsAt(KatherineScene.KLOC_BAR_DRUNK);

            if (atBar)
                OutputText("Scenes.Katherinethreesome.SpitroastKath.LeadTowardBackroomDoor".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SpitroastKath.UrtaGrabsLockingArms".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.NeverGiftHorseMouth".Localized(
                katherine.ClothesChoice("through her " + (atBar ? "blouse" : "uniform"), "through the thin fabric of her bodysuit", "through her top", "through her silken robe", "by reaching inside her tube top", "by reaching inside that sexy nurse's outfit")
            ));

            OutputText("Scenes.Katherinethreesome.SpitroastKath.OnlyNipplesBothStart".Localized(
                katherine.CatGirl("cat girl", "feline")
            ));

            OutputText("Scenes.Katherinethreesome.SpitroastKath.PutBackSamePosition".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) <= 15)
                OutputText("Scenes.Katherinethreesome.SpitroastKath.LooksUncomfortableCock".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) <= 31)
                    OutputText("Scenes.Katherinethreesome.SpitroastKath.AllowsSlideAgainstLips".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.SpitroastKath.BigSmilePullsShaft".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.SentencePeriod".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.SinceNotGoingAnywhere".Localized(
                katherine.CatGirl("smooth", "fuzzy")
            ));

            if (Player.IsTaur())
                OutputText("Scenes.Katherinethreesome.SpitroastKath.BackHorizontalOverDistance".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SpitroastKath.MouthLinedUpCock".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.SentencePeriodAgain".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.ThrustsDrivesKatherineLips".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.SpitroastKath.LipsDownYourShaft".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SpitroastKath.FaceIntoYourGroin".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.TakesWithoutComplaintTongue".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.ReallyGetsHandsBreasts".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Katherinethreesome.SpitroastKath.LapsPussyDrinkingJuices".Localized());
            else
            {
                OutputText("Scenes.Katherinethreesome.SpitroastKath.PullTowardsWholeLength".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Katherinethreesome.SpitroastKath.FewSquirtsNeedNap".Localized());
                else
                    if (Player.CumQ() < 1500)
                        OutputText("Scenes.Katherinethreesome.SpitroastKath.DeliverSquirtAfterSquirt".Localized());
                    else
                        if (Player.CumQ() < 3000)
                            OutputText("Scenes.Katherinethreesome.SpitroastKath.FirehoseCumDownThroat".Localized());
                        else
                            OutputText("Scenes.Katherinethreesome.SpitroastKath.TorrentCumBlastPanicked".Localized());
            }

            OutputText("Scenes.Katherinethreesome.SpitroastKath.SentencePeriodThird".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.WhenRecoverRubThigh".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.SpitroastKath.HotSprayLandsOn".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.KathCameClenchingPussy".Localized());

            if (!Player.HasCock() || Player.CumQ() < 3000)
                OutputText("Scenes.Katherinethreesome.SpitroastKath.SureEnoughBellyFloorboards".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SpitroastKath.DifficultEnoughHoldsSeveral".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.SentencePeriodFourth".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.PantsLikeMarathonMoans".Localized());

            OutputText("Scenes.Katherinethreesome.SpitroastKath.DoesntAnythingLacesFingers".Localized());

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(false);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if ((CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) != 3) && (View.Time.Hours >= 13))
                    CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void PinAndFuck()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.PinAndFuck.GetTablePullsHug".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuck.OnceInsideUrtaAggressive".Localized(
                katherine.ClothesChoice("unbuttoning Kath's blouse", "sliding her hands inside Kath's bodysuit", "unlacing Kath's dress", "pulling the robe off Kath's shoulders", "slipping the stretchy tube top over Kath's head", "reaching inside the nurse's top to grope Kath's breasts")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuck.SweepLegsFallsArms".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Katherinethreesome.PinAndFuck.RestBellyTop".Localized());
            else
                OutputText("Scenes.Katherinethreesome.PinAndFuck.QuicklyCoilYourTail".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuck.ProneFormGrowlsSquirms".Localized(
                katherine.CockMultiple(" stands", "s stand")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuck.LiftCaptainNeedsHelp".Localized(
                katherine.CockMultiple(" brushes against her pussy, which is", "s brush against her slit and her tight sphincter. Kath reaches under Urta, scoops a little pre from the tip of her dripping horsecock and smears it over Urta's asshole. Urta's cunt needs no such attention, it's")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuck.GentlyTurnsKissSheathing".Localized(
                katherine.CockMultiple(" by Kath's " + katherine.CockType("canine", "unusual feline") + " member", ", not once but twice")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuck.BeforeKathGruntFlooded".Localized());

            if (Player.IsPregnant() && Player.Pregnancy.Type != PregnancyIds.BeeEggs && Player.Pregnancy.Type != PregnancyIds.DriderEggs)
                OutputText("Scenes.Katherinethreesome.PinAndFuck.AlreadyCarryingBabyBelly".Localized());
            else
                OutputText("Scenes.Katherinethreesome.PinAndFuck.NeedsWombFullSeed".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuck.KatherineDoesntLongKnot".Localized(
                katherine.CockMultiple(" swells up and with one last hard thrust she forces it inside Urta's abused pussy", "s swell up and with one last hard thrust she forces them inside Urta's stretched holes")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuck.LooksFuckingBellyGrows".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuck.StrokeFingersUpDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.PinAndFuck.YourCockWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Katherinethreesome.PinAndFuck.YourSlitWord".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.PinAndFuck.BarrenFleshBetweenLegs".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuck.LookingOverPartnersDrilling".Localized());

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.StuffKath".Localized(), PinAndFuckStuffKath);
            else
                AddButton(0, "Common.Button.StuffKath".Localized(), null);

            AddButton(1, "Common.Button.MountUrta".Localized(), PinAndFuckMountUrta);
        }

        public void PinAndFuckMountUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.RollOverEquineCock".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.SinkOntoUrtaCock".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.EnjoyStretchingStraightening".Localized());
            else
            {
                OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.GrindsAgainstCervix".Localized());

                if (Player.IsPregnant())
                {
                    if (Player.Pregnancy.Type == PregnancyIds.GooStuffed)
                        OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.StrokeGooStuffedBelly".Localized());
                    else
                        if (Player.Pregnancy.Type == PregnancyIds.Urta)
                            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.PutBellyChildPlanted".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) > 0)
                                OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.ThinkAboutInsideBelly".Localized());
                            else
                                OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.ThinkAboutWishFathered".Localized());
                }
                else
                    if (Player.HasStatusEffect(StatusEffectIds.Contraceptives))
                        OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.ThinkAboutEmptyImpregnate".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) > 0)
                            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.StartFantasizeMassesCum".Localized());
                        else
                            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.LoseYourselfSensationCrevice".Localized());

                OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.PauseFeelGirthStretches".Localized());
            }

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.UrtaStartsOversensitiveBuried".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.LegsAroundForceDeeper".Localized());

            if (!Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.FillingColonLikeBalloon".Localized());
            else
                if (Player.Pregnancy.Type == PregnancyIds.GooStuffed)
                    OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.ForcesInsideWombBath".Localized());
                else
                    if (Player.IsPregnant())
                        OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.SpatteringAgainstCervix".Localized());
                    else
                        OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.ForcesInsideWombExquisite".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.KissGiveOrgasm".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.PutsHandStretchedStomach".Localized(
                katherine.CockMultiple(" knotted inside her pussy", "s knotted inside her pussy and ass")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.AgainUrtaCantAgain".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuckMountUrta.KathContinuesGentlyFuck".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 8 + Random.Shared.Next(2));

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void PinAndFuckStuffKath()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.KathFloppedSpreadView".Localized(
                katherine.CockMultiple("", " lower"),

                katherine.CockMultiple(" and her dripping slit", ", her dripping slit and her supple sphincter, both")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.MayTiredStillMood".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.HoldStillAdjustHips".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.FuckSlowlyGentlyThrusting".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.JustStartPoundingRough".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.UnderneathBothTriesLook".Localized(
                katherine.CockMultiple(" is", "s are")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.JustWatchedGirlfriendsLast".Localized());

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.YourCockWord".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.LoadSoakingIntoKatherine".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.FeelBellySwellsBit".Localized());
                else
                    if (Player.CumQ() < 3000)
                    {
                        OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.BodyPushedSheerVolume".Localized());

                        if (katherine.PregSize() > 0)
                            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.ParticularlyPainfulWombClosed".Localized());
                        else
                            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.StartsLittlePregnantWomb".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.SuchForceKnockedOut".Localized());

                        if (katherine.PregSize() == 0)
                            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.EvenBehindWombExpanding".Localized());

                        if (katherine.PregSize() > 0)
                        {
                            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.WithoutEmptyTakeLoad".Localized());

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.LowerVaginaReachesLimit".Localized());
                            else
                                OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.SuperchargedFillCuntLimit".Localized());
                        }

                        OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.BulgeLargeLiftUrta".Localized(
                            katherine.CockMultiple("", "s")
                        ));
                    }

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.BottomPileHearUrta".Localized(
                katherine.CockMultiple("its", "they're")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.QuickCheckFingersReturned".Localized(
                katherine.CockMultiple(" has", "s have")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.PullKathPatBack".Localized(
                katherine.CockMultiple("that knot", "those knots")
            ));

            OutputText("Scenes.Katherinethreesome.PinAndFuckStuffKath.LockDoorWayOut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void Watch(bool urtaIsDrunk)
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.Watch.ReachTableDownedBeverage".Localized());

            WatchMainBody(urtaIsDrunk);
        }

        public void WatchNoIntro(bool urtaIsDrunk)
        {
            ClearOutput();

            WatchMainBody(urtaIsDrunk);
        }

        public void WatchMainBody(bool urtaIsDrunk)
        {

            OutputText("Scenes.Katherinethreesome.WatchMainBody.QuicklyCommandeerBackRoom".Localized(
                katherine.ClothesChoice("first remove her blouse and then slip off her skirt", "peel the sheer bodysuit off her lithe form", "unlace her dress and slide it off her sexy body", "undo her robe and pull it from her shoulders", "pull the tube top over her head and then " + katherine.ClothesLowerChoice("peel off her tight shorts", "slip off her skirt", "", "", ""), "unbutton her tight blouse, slip off her skirt and finally pluck the cute white hat from her head")
            ));

            OutputText("Scenes.Katherinethreesome.WatchMainBody.KathPosesWhatSee".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.StrokesCockSmearingPre".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.ShortOneSidedGrappling".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.WatchMainBody.UndersideOfKathCock".Localized(
                    katherine.CockMultiple("", "lower ")
                ));
            else
                OutputText("Scenes.Katherinethreesome.WatchMainBody.KathDrippingSlit".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.LittleSurprisedRecoversOut".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.MoansLiftsHighPussy".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.BitesJackhammeringHoleWider".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.NotMaybeBellowsUrta".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.StartedBackPullsHands".Localized(
                katherine.breasts.Adj()
            ));

            OutputText("Scenes.Katherinethreesome.WatchMainBody.LooksGettingCloseBlowing".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.ReachesBetweenRollingBalls".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.OnceFinishedHelpMe".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.RaisesGrinMaybeMocking".Localized());

            if (katherine.HasCock())
            {
                OutputText("Scenes.Katherinethreesome.WatchMainBody.LooksRubsPointOut".Localized(
                    katherine.CockMultiple("her hard cock", "one of her hard cocks")
                ));

                OutputText("Scenes.Katherinethreesome.WatchMainBody.LooksUpTimeShaft".Localized(
                    katherine.CockType(),

                    katherine.CockMultiple(" into her pussy", "s into her cunt and rectum")
                ));
            }
            else
            {
                OutputText("Scenes.Katherinethreesome.WatchMainBody.LooksWayRunsFinger".Localized());

                OutputText("Scenes.Katherinethreesome.WatchMainBody.HeadRollsForwardBelly".Localized());
            }

            OutputText("Scenes.Katherinethreesome.WatchMainBody.AlreadyCloseCummingBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) > 20)
                OutputText("Scenes.Katherinethreesome.WatchMainBody.TipTailLittleWiggle".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.SentencePeriodSayEnd".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.GivesSmirkMaybeMore".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.WatchMainBody.ChairPushesSurprisedAh".Localized(
                    katherine.CockMultiple("knot forces its", "twin knots force their"),

                    katherine.CockMultiple("it", "them"),

                    katherine.CockMultiple("", "and colon")
                ));
            else
            {
                OutputText("Scenes.Katherinethreesome.WatchMainBody.BallsStrokesGentlyClaws".Localized(
                    katherine.CatGirl("nails", "claws")
                ));

                OutputText("Scenes.Katherinethreesome.WatchMainBody.MoansTellWordsWorked".Localized());

                OutputText("Scenes.Katherinethreesome.WatchMainBody.ThatsMoreCriesKath".Localized());

                OutputText("Scenes.Katherinethreesome.WatchMainBody.ShuddersHandsBackHips".Localized());

                OutputText("Scenes.Katherinethreesome.WatchMainBody.PromisesPromisesDreamyVoice".Localized());
            }

            OutputText("Scenes.Katherinethreesome.WatchMainBody.GiveEachLoversKiss".Localized());

            OutputText("Scenes.Katherinethreesome.WatchMainBody.MuchLikeCheckPortal".Localized());

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 20)));

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void KathLicksOutUrta()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.KathTwitchesUrtaEars".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.ShowUnderwayShiftSeat".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.GoodKittyCareCock".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.FromUnderTableReply".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.LeanDownForYourself".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.ShakesClearLustyHaze".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.OnlyReplyLongMmmmm".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.LastsExpectedSilverTongue".Localized());

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.SnakesBackWetSpunk".Localized(
                katherine.PlayerText()
            ));

            OutputText("Scenes.Katherinethreesome.KathLicksOutUrta.NoFairMuttersWeakly".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 20)));

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().drainedByKath = true;

            katherine.KatherineSex();
        }

        public void Knothole()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.Knothole.ScratchesBehindSuggestsRooms".Localized(
                katherine.CatGirl("cat girl", "feline")
            ));

            OutputText("Scenes.Katherinethreesome.Knothole.MuttersKeepsTryingLoose".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.JustSighsLooksLearn".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherinethreesome.Knothole.NoGonnaSaysKath".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.SlidesFurtherAnglesKatherine".Localized(
                katherine.CockMultiple("", "lower "),

                katherine.CockMultiple("her prick", "one of her pricks")
            ));

            OutputText("Scenes.Katherinethreesome.Knothole.NotMumblesTryingUp".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.ShouldStartBehavingAss".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.EyesGoWideWouldnt".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.WouldntWhatInnocentTone".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.PleaseDontLetKnot".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.ReachesRubsLateAsk".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherinethreesome.Knothole.GrinsEvillyCaptiveUndress".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.SwingsAroundHorsecockBob".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.BegsSpreadingWiderCat".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherinethreesome.Knothole.SeeDevelopingTightPassage".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Katherinethreesome.Knothole.StandOverWord".Localized());
            else
                OutputText("Scenes.Katherinethreesome.Knothole.SlitherInFrontOf".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.KathAndPresentWord".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Katherinethreesome.Knothole.SucksOnTipEagerly".Localized());
            else
                OutputText("Scenes.Katherinethreesome.Knothole.YourPussyPressesCrotch".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.RoughTongueCaressingYou".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.StartScratchGoesWild".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.PantingHardSpeedingStrokes".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.CollapsesAgainstTweakingNipple".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.AlreadySceneBeforeEars".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.AllLieTangleSpent".Localized());

            OutputText("Scenes.Katherinethreesome.Knothole.GetDressedWatchingSnuggle".Localized());

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void Sandwich()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.Sandwich.ScratchesBehindSuggestsRooms".Localized());

            OutputText("Scenes.Katherinethreesome.Sandwich.ComeAirGetReady".Localized());

            OutputText("Scenes.Katherinethreesome.Sandwich.MostlyNakedBehindThighs".Localized(
                katherine.CockMultiple("", "s"),

                katherine.CockMultiple("", "s"),

                katherine.CockMultiple("", "upper cock pressing against Urta's ass and her lower ")
            ));

            OutputText("Scenes.Katherinethreesome.Sandwich.PutHandsShouldersBack".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.Sandwich.HowCummingBuckets".Localized(
                katherine.CockMultiple("", "s")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.MountUrta".Localized(), SandwichGetFucked);

            if (Player.HasCock())
                AddButton(1, "Common.Button.GetLicked".Localized(), (Player.IsTaur() ? SandwichMaleCentaurLicked : SandwichGetLicked));
            else
                AddButton(1, "Common.Button.GetLicked".Localized(), null);
        }

        public void SandwichGetFucked()
        {

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.LookHorseCockBounces".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Katherinethreesome.SandwichGetFucked.TurnAroundFlickTail".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SandwichGetFucked.SlideAcrossTheFloor".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.AlreadyWastesNoTime".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.HandsLatchMeatyCock".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.KathDoesntPoundsPace".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.MustCloseWorkAss".Localized());

            if (katherine.CumQ() <= 1500)
                OutputText("Scenes.Katherinethreesome.SandwichGetFucked.GrowWarmFillsSeed".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SandwichGetFucked.ExpandingAgainstForcedLungs".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.SentencePeriod".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.DespiteStuffedBellyBlistering".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetFucked.AtDelightfulPullsAgainst".Localized());

            SandwichCommonEnd();
        }

        public void SandwichGetLicked()
        {

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.MassageShouldersHelpOut".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.HardStandLost".Localized());

            if (katherine.cockNumber > 1)
                OutputText("Scenes.Katherinethreesome.SandwichGetLicked.BothCocksWillingHoles".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SandwichGetLicked.HerCockWillingHole".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.EveryThrustRackJiggles".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.PowerfulThrustForceInside".Localized(
                katherine.CockMultiple("", "s"),

                katherine.CockMultiple("", "s"),

                katherine.CockMultiple("it", "them")
            ));

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.PleadingSendsOverEdge".Localized());

            if (Player.HasCock())
                if (Player.CumQ() < 500)
                {
                    OutputText("Scenes.Katherinethreesome.SandwichGetLicked.PumpSmallLoadThroat".Localized());

                    OutputText("Scenes.Katherinethreesome.SandwichGetLicked.DoesntHaveChanceRecover".Localized());
                }
                else
                    if (Player.CumQ() < 1500)
                    {
                        OutputText("Scenes.Katherinethreesome.SandwichGetLicked.PumpAfterShotInnards".Localized());

                        OutputText("Scenes.Katherinethreesome.SandwichGetLicked.DoesntChanceRecoverAgain".Localized());
                    }
                    else
                        if (Player.CumQ() < 3000)
                        {
                            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.TensesReleaseFountainStomach".Localized());

                            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.DoesntLongBellyExpanding".Localized());

                            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.DoesntChanceGetUsed".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.ReleaseCockExpandsGirth".Localized());

                            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.TidalWaveStepBack".Localized());

                            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.DoesntChanceUsedAgain".Localized());
                        }

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.BeforeForwardAgainKnots".Localized());

            if (Player.IsPregnant())
            {
                if (katherine.cockNumber > 1)
                    OutputText("Scenes.Katherinethreesome.SandwichGetLicked.WatchFormsCervixBlocking".Localized());

                OutputText("Scenes.Katherinethreesome.SandwichGetLicked.CockKatherinePlantedAss".Localized(
                    katherine.CockMultiple("must be pumping in a torrent of cum", "faces no such restriction")
                ));

                if (katherine.cockNumber > 1)
                    OutputText("Scenes.Katherinethreesome.SandwichGetLicked.SuddenlyLetsSurprisedAhhh".Localized());

                OutputText("Scenes.Katherinethreesome.SandwichGetLicked.CollapsesLastSquirtsAss".Localized(
                    katherine.CockMultiple("", "Urta has gone from clutching the bulge to rubbing her sides.")
                ));
            }
            else
            {
                OutputText("Scenes.Katherinethreesome.SandwichGetLicked.BellyLargerEverySquirt".Localized(
                    katherine.CockMultiple("Sure enough Katherine's cock starts to release its torrent of cum inside Urta and her", "Sure enough, the twin shafts inside Urta release a torrent of cum and her")
                ));

                OutputText("Scenes.Katherinethreesome.SandwichGetLicked.FinallyRunsJuiceCollapses".Localized());
            }

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.PutsMovesBellyButton".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichGetLicked.PantsStillNotWink".Localized());

            SandwichCommonEnd();
        }

        public void SandwichMaleCentaurLicked()
        {

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.MassageShouldersHelpOut".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.WindUnusualPleasantSituation".Localized());

            if (katherine.cockNumber > 1)
                OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.AssAndPussyWord".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.Ass".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.RightPositionKissEmbrace".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.PullsWinkShovesForward".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.KnowCantUncomfortableLowered".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.ReallyStartsPantJackhammer".Localized(
                katherine.CockMultiple("", "s"),

                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.SinceSolidlyJoinedHold".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.SlidesAroundInsideMouth".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.AllFourKneesWeak".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.PumpSmallLoadThroat".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.PumpAfterShotInnards".Localized());
                else
                    if (Player.CumQ() < 3000)
                    {
                        OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.TensesReleaseFountainStomach".Localized());

                        OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.DoesntLongBellyExpanding".Localized(
                            katherine.CockMultiple("", "s")
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.ReleaseCockExpandsGirth".Localized(
                            katherine.CockMultiple(" has", "s have")
                        ));

                        OutputText("Scenes.Katherinethreesome.SandwichMaleCentaurLicked.TidalWaveStepBack".Localized());
                    }

            SandwichCommonEnd();
        }

        public void SandwichCommonEnd()
        {

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.PullKnottedLoversScene".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.MakeSkippingCleaningTreat".Localized());

            if (katherine.ballSize > 0)
                OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.RecentlyDrainedBalls".Localized());
            else
                OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.BaseOfHerCock".Localized(
                    katherine.CockMultiple("", "lower ")
                ));

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.YouveGotShowEndurance".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.JustStillKnottedCant".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.DontBeAQuitter".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.NoNoStillCant".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.KeepRubbingTeaseClit".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.DontWantHowBig".Localized());

            OutputText("Scenes.Katherinethreesome.SandwichCommonEnd.ClutchesNoGetting".Localized());

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void Orgy()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.Orgy.AttemptDrawTooLate".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.SoDrunkDoesntErect".Localized(
                katherine.ClothesLowerChoice("Kath's blouse", "Kath's blouse", "the upper part of Kath's bodysuit", "the laces on Kath's dress", "the front of Kath's robe"),

                katherine.breasts.Adj()
            ));

            OutputText("Scenes.Katherinethreesome.Orgy.NextHandTwistArm".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.Orgy.CrotchWord".Localized());
            else
                OutputText("Scenes.Katherinethreesome.Orgy.Breasts".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.SentencePeriod".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.PullsAgainPlacesVagina".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.LetsWrapsTailLeg".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.LiftsLooksAroundStaring".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.AllAroundBucklesLoosened".Localized());

            var valaHere = CoC.Instance.Flags.Get(GameFlagIds.FreedVala) > 0 && View.Time.Hours >= 12 && View.Time.Hours <= 21;

            if (valaHere)
            {
                OutputText("Scenes.Katherinethreesome.Orgy.SpotValaTrayDrinks".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineValaTimesSex) > 0)
                    OutputText("Scenes.Katherinethreesome.Orgy.SmileNothingAgainstOrgy".Localized());
                else
                {
                    OutputText("Scenes.Katherinethreesome.Orgy.SmileWeaklyWonderingExplain".Localized());

                    OutputText("Scenes.Katherinethreesome.Orgy.HiCheerfullyBroughtFriend".Localized());

                    OutputText("Scenes.Katherinethreesome.Orgy.StopsKissingKindaYeah".Localized());

                    OutputText("Scenes.Katherinethreesome.Orgy.SureSoundsHeroGrinning".Localized());

                    OutputText("Scenes.Katherinethreesome.Orgy.TellValaKatherine".Localized());
                }
            }

            OutputText("Scenes.Katherinethreesome.Orgy.ThingsHandLoseTrack".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Katherinethreesome.Orgy.YouKathAll".Localized());
            else
                OutputText("Scenes.Katherinethreesome.Orgy.KathWord".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.SportHugeStuffedBellies".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.TooTiredHighlightsMind".Localized());

            if (valaHere)
                OutputText("Scenes.Katherinethreesome.Orgy.ValaSurprisingKathBy".Localized(
                    katherine.HasCock() ? "settling on top of her " + katherine.CockAdj() + "shaft and then taking it, knot and all, inside her tiny body" : "diving between her thighs and sucking on Kath's clit while pushing her whole hand inside Kath's dripping twat"
                ));

            OutputText("Scenes.Katherinethreesome.Orgy.HeavilyPregnantFistingCentaur".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Katherinethreesome.Orgy.DeepthroatedYourCock".Localized());
            else
                OutputText("Scenes.Katherinethreesome.Orgy.LickedYouOut".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.BentTableFuckedBehind".Localized());

            if (valaHere)
            {
                OutputText("Scenes.Katherinethreesome.Orgy.ManagesFreeStickyFloor".Localized());

                OutputText("Scenes.Katherinethreesome.Orgy.LooksCollapsedBodiesWhile".Localized());

                OutputText("Scenes.Katherinethreesome.Orgy.MumblesWorthPassOut".Localized());

                SceneInstance<KatherineScene>.Get().KatherineAndValaHadSex();
            }

            OutputText("Scenes.Katherinethreesome.Orgy.UsingColumnSupportUpright".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.TossesSpermyBitClothing".Localized());

            OutputText("Scenes.Katherinethreesome.Orgy.AwNotPaperworkKath".Localized());

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 10 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(2);
        }

        public void DoubleStuffKath()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite(true);

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.AskTwoMorePrivate".Localized());

            var urtaHasHouse = Player.HasKeyItem("Spare Key to Urta's House");

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.UrtaGoodIdeaApartment".Localized(
                katherine.ClothesLowerChoice("runs her hand up Kath's thigh", "slides her hand under Kath's skirt", "rubs the " + (katherine.HasCock() ? katherine.CockAdj() + "bulge in the " : "") + "crotch of Kath's bodysuit", "slides her hand under Kath's dress", "slides her hand between the folds of Kath's robe")
            ));

            CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.InterestingNeedSupportHorny".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.YouWantSeeFuck".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.StrokesKatherineLovesShow".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.LiftsPressesFlatFace".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.BarelyClothesVixensSlam".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.StartsLaughStillGood".Localized(
                katherine.HasCock() ? "No room left for you" : "Too slow" + Player.ShortName
            ));

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.LicksGonnaFeelGood".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.AlcoholLoosenedSucksLooks".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.ShowTweaksNipplesVibrates".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.BeforeAgainWasntFinished".Localized(
                katherine.HasCock() ? "thrust her " + katherine.CockType() + " member into Urta's hole" : "rock her hips, her hand rolling Urta's balls between her fingers"
            ));

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.HornyInsatiableFightingOver".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.HearUrtaShorterStrokes".Localized());
            else
                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.HearUrtaPlantsDeep".Localized(
                    katherine.CatGirl("", "fuzzy ")
                ));

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.RegainsStrengthForcesElbows".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.TwoLoversTradeForth".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.NoReasonFightGo".Localized());
            else
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Katherinethreesome.DoubleStuffKath.PutBehindEachHead".Localized());

                    if (Player.CumQ() < 500)
                    {
                        OutputText("Scenes.Katherinethreesome.DoubleStuffKath.BothAdditionalContribution".Localized());

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.OtherCockProduceSquirts".Localized());

                        OutputText("Scenes.Katherinethreesome.DoubleStuffKath.PullTiredHugGiggle".Localized());
                    }
                    else
                        if (Player.CumQ() < 1500)
                        {
                            if (katherine.HasCock())
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.BothLittleDifficultyTummies".Localized());
                            else
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.UrtaSwallowsKathBrimming".Localized());

                            if (Player.CockTotal() > 2)
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.RealizeCockFiredStreams".Localized());

                            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.LooksKathLaughs".Localized());
                        }
                        else
                        {
                            if (katherine.HasCock())
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.GutStartsExpandPressure".Localized());
                            else
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.GutExpandDepositRounding".Localized());

                            if (Player.CockTotal() > 2)
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.OtherCockReleaseTorrent".Localized());

                            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.PullFreeFauxPregnancy".Localized());
                        }
                }
                else
                    if (Random.Shared.Next(2) == 0)
                    {
                        OutputText("Scenes.Katherinethreesome.DoubleStuffKath.HadSwallowedAgainDown".Localized());

                        OutputText("Scenes.Katherinethreesome.DoubleStuffKath.KathRealizesHappenedCoax".Localized());

                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.UrtaSwallowsEasilyAbdomen".Localized());
                        else
                            if (Player.CumQ() < 1500)
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.UrtaDifficultyTakingDeposit".Localized());
                            else
                            {
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.GutExpandWorriedPressure".Localized());

                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.PullCockFauxPregnancy".Localized());
                            }
                    }
                    else
                    {
                        OutputText("Scenes.Katherinethreesome.DoubleStuffKath.LickingHummingSetOff".Localized());

                        OutputText("Scenes.Katherinethreesome.DoubleStuffKath.UrtaRealizesHappenedCoax".Localized());

                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Katherinethreesome.DoubleStuffKath.KathSwallowsEasilyExpanded".Localized());
                        else
                            if (Player.CumQ() < 1500)
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.KathDifficultyDepositBelly".Localized());
                            else
                            {
                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.KathStartsExpandWorried".Localized());

                                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.WhenFreeRubsBelly".Localized());
                            }
                    }

            if (katherine.HasCock())
            {
                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.NowStuffedHerbsKnotted".Localized());

                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.WrigglesTryingAwayKnot".Localized());
            }
            else
            {
                OutputText("Scenes.Katherinethreesome.DoubleStuffKath.NowSpentUnfortunatelyCamp".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Katherinethreesome.DoubleStuffKath.GivingFeelingBottomHeart".Localized());
                else
                    OutputText("Scenes.Katherinethreesome.DoubleStuffKath.DespiteHavingJustCum".Localized());
            }

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            ReturnToCamp(1);
        }

        public void DoublePenetrateKath()
        {
            ClearOutput();

            SceneInstance<KatherineScene>.Get().KatherineSprite(true);

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.AskCommandeerBackRooms".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.UrtaGivesWolfishSmile".Localized(
                katherine.ClothesLowerChoice("runs her hand up Kath's thigh", "slides her hand under Kath's skirt", "rubs the " + (katherine.HasCock() ? katherine.CockAdj() + "bulge in the " : "") + "crotch of Kath's bodysuit", "slides her hand under Kath's dress", "slides her hand between the folds of Kath's robe")
            ));

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.HelpBothBackrooms".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.WatchCockStrainClothes".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.RipsLastFewGarments".Localized(
                katherine.breasts.Adj()
            ));

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.LetsOutHappyMoan".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PokingUrtaBelly".Localized(
                    katherine.CockMultiple("cock, now in the open air, rises until it's", "twin cocks, now in the open air, rise until they're")
                ));
            else
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.BreastsManhandledWatchFemcum".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.TwistsOutWaySqueak".Localized(
                    katherine.CockMultiple("it", "them")
                ));
            else
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PropsHorsecockAgainstClit".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.ByUndressedGettingHard".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.JerksForwardSurpriseTiptoe".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.SoFullKathSay".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.NotArentPushesShoulders".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.TongueLollsStretchesWider".Localized(
                katherine.PlayerText()
            ));

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PullJoinedAwayWall".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.OhSeeShePants".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PushesForwardAgainAsshole".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.InsideThroughFeelIntrusion".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.OnlyAlongBuildRhythm".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.WetSquishingDrippingPurring".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.CockMattingCoatingFloor".Localized(
                    katherine.CockMultiple("", "s")
                ));
            else
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PussyReleasingFloodCum".Localized(
                    katherine.CatGirl("wets her skin", "mats her fur")
                ));

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.GettingCloseSphincterBeginning".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.UselesslyHumpingDickSeed".Localized(
                    katherine.CockMultiple(" sprays", "s spray")
                ));
            else
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.WeaklyGrindingBeggMore".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.NowTurnTightenGrip".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PumpSeveralRapidShots".Localized());
            else
                if (Player.CumQ() < 1500)
                {
                    OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.PumpSeveralStreamsRectum".Localized());

                    OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.LookingShoulderBellyBallooned".Localized());
                }
                else
                    if (Player.CumQ() < 3000)
                    {
                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.WheezesReleaseFountainBowels".Localized());

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.DoesntLongBellyExpanding".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.KnockWindExplodesAnus".Localized());

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.HandsWeakerHipsStretching".Localized());

                        if (Player.Corruption < 25)
                            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.HopeNotPermanentDamage".Localized());
                        else
                            if (Player.Corruption < 75)
                                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.HopeKathEnjoyingThis".Localized());
                            else
                                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.SecretlyForceMoreSperm".Localized(
                                    katherine.CatGirl("girl", "morph")
                                ));

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.WhimpersCompletelyWagging".Localized());
                    }

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.UrtaMovingTakesChin".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.NodsSlowlyUnderSpell".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.DoesntWaitClosePresent".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.AlreadyTwiceFilledOnce".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.AtPlantsDeepTip".Localized());

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.LuckilyBalanceFallsBack".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.SeeDribblesSpermLegs".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.EachContractionGushesGaping".Localized());
                else
                    if (Player.CumQ() < 3000)
                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.FountainBurstsGapingAsshole".Localized());
                    else
                    {
                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.CumEruptsGeyserProud".Localized());

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.StartsLeakCoatingFloor".Localized());
                    }

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.BothGigglingPattingBelly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) == 1)
            {
                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.TheyBothWowSame".Localized());

                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.KissesDontKnow".Localized());

                OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.FunFillingAnotherRound".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) < 11)
                {
                    OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.BigKissReallyLike".Localized());

                    OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.RatherAnsweringScratchEars".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection) < 32)
                    {
                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.HugsOnceFeelBetter".Localized());

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.MmmmAlwaysReadyKitty".Localized());

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.DreamyYeahCurlsSleep".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.HugsBothEachTurn".Localized());

                        OutputText("Scenes.Katherinethreesome.DoublePenetrateKath.SniffsBeforeNeverLove".Localized());
                    }

            Player.Orgasm("Dick");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndUrtaHadSex(true);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            SceneInstance<UrtaScene>.Get().UrtaLove(1);

            if (View.Time.Hours >= 13)
                CoC.Instance.Flags.Set(GameFlagIds.KatherineLocation, KatherineScene.KLOC_URTAS_APT);

            ReturnToCamp(1);
        }

        public void DoubleStuffVala()
        {

            ValaCommonStart();

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.RipsPushBackTable".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.PullFoldsOtherHand".Localized(
                katherine.CockType(),

                katherine.CockMultiple("", "s"),

                katherine.CockMultiple(" tightly and guide it", "s tightly, forcing the tips together, and guide them"),

                katherine.CockMultiple(" sinks", "s sink"),

                katherine.CockMultiple("Kath's cock buries itself", "both Kath's cocks bury themselves")
            ));

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.HeadLollsPullAsscheeks".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Katherinethreesome.DoubleStuffVala.CockAgainstSphincterPush".Localized());
            else
                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Katherinethreesome.DoubleStuffVala.TwinCocksAgainstRelaxes".Localized());
                else
                    if (Player.Cocks.Count == 3)
                        OutputText("Scenes.Katherinethreesome.DoubleStuffVala.TripleCocksAgainstRelaxes".Localized());
                    else
                        OutputText("Scenes.Katherinethreesome.DoubleStuffVala.ThreeLargestAgainstRelaxes".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.FeelsWonderfulInnerWalls".Localized(
                katherine.CockMultiple("Kath's shaft", "both of Kath's shafts")
            ));

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.HoldingWaitingCockPlace".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.RagScreamsOnlyProof".Localized());

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.GrabSlenderYankDown".Localized(
                katherine.CockMultiple("", "s")
            ));

            if (Player.CumQ() < 1500)
            {
                OutputText("Scenes.Katherinethreesome.DoubleStuffVala.WorkHarderPastKnot".Localized(
                    katherine.CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherinethreesome.DoubleStuffVala.BellyStretchesIncredible".Localized());
            }
            else
            {
                OutputText("Scenes.Katherinethreesome.DoubleStuffVala.DespiteEngorgedKnotTask".Localized(
                    katherine.CockMultiple("", "s")
                ));

                OutputText("Scenes.Katherinethreesome.DoubleStuffVala.BothValaLosesStretches".Localized());
            }

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.OhWeaklyRestsHand".Localized());

            ValaCommonPostSex(katherine.HasCock());

            OutputText("Scenes.Katherinethreesome.DoubleStuffVala.ShesDressedMentalPicture".Localized());

            Player.Orgasm("Generic");

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndValaHadSex();

            CoC.Instance.Flags.Set(GameFlagIds.ValaTimesConsensualSex, CoC.Instance.Flags.Get(GameFlagIds.ValaTimesConsensualSex) + 1);

            ReturnToCamp(1);
        }

        public void EatOutVala()
        {

            ValaCommonStart();

            OutputText("Scenes.Katherinethreesome.EatOutVala.PullArmsWalkBackwards".Localized(
                katherine.CockMultiple(" of Kath's cock", "s of Kath's cocks")
            ));

            OutputText("Scenes.Katherinethreesome.EatOutVala.PerfectEachGrabAss".Localized(
                katherine.CockMultiple("", "twin "),

                katherine.CockType(),

                katherine.CockMultiple("", "s"),

                katherine.CockMultiple(", spreads some pre over the shaft and lines it", "s, spreads some pre over the shafts and and lines them")
            ));

            OutputText("Scenes.Katherinethreesome.EatOutVala.LowerSighsRodHome".Localized(
                katherine.CockMultiple(" rams", "s ram")
            ));

            OutputText("Scenes.Katherinethreesome.EatOutVala.SquirmsLocksBehindBeg".Localized());

            OutputText("Scenes.Katherinethreesome.EatOutVala.WhenStartsBackOff".Localized());

            OutputText("Scenes.Katherinethreesome.EatOutVala.ReachTableForwardTough".Localized(
                katherine.CatGirl("girl", "morph")
            ));

            OutputText("Scenes.Katherinethreesome.EatOutVala.NotInterestedTonguePussy".Localized());

            OutputText("Scenes.Katherinethreesome.EatOutVala.RunTongueAcrossHeat".Localized(
                katherine.CockMultiple("", "s")
            ));

            OutputText("Scenes.Katherinethreesome.EatOutVala.FirstRecoverFeelsGood".Localized());

            ValaCommonPostSex(katherine.HasCock());

            OutputText("Scenes.Katherinethreesome.EatOutVala.FlipsLyingSittingUpright".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndValaHadSex();

            CoC.Instance.Flags.Set(GameFlagIds.ValaTimesConsensualSex, CoC.Instance.Flags.Get(GameFlagIds.ValaTimesConsensualSex) + 1);

            ReturnToCamp(1);
        }

        public void FistKathAndVala()
        {

            ValaCommonStart();

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.TellValaDownStool".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.DealKittyBehindArms".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.GrabDragLightweightFaerie".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.AsksArenGoing".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.GetsKissingHandsTail".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.IsntPlayingSlidesNipples".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.BothDriveOtherTotal".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.GentlyHavingNone".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.ReleasesHugsBackEcstasy".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.MapFavouriteSpotsCumming".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.FistKathAndVala.CockIgnoredWholeSurrounding".Localized(
                    katherine.CockMultiple("", "s"),

                    katherine.CockMultiple("springs to life and blasts", "spring to life and blast")
                ));

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.AfterGaspingPantingSpeech".Localized());

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.GetsMischievousGleamEye".Localized());

            ValaCommonPostSex(false);

            OutputText("Scenes.Katherinethreesome.FistKathAndVala.ExtractNoticePulled".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            SceneInstance<KatherineScene>.Get().Orgasm();

            SceneInstance<KatherineScene>.Get().KatherineAndValaHadSex();

            CoC.Instance.Flags.Set(GameFlagIds.ValaTimesConsensualSex, CoC.Instance.Flags.Get(GameFlagIds.ValaTimesConsensualSex) + 1);

            ReturnToCamp(1);
        }

        public void ValaCommonStart()
        {
            ClearOutput();

            OutputText("Scenes.Katherinethreesome.ValaCommonStart.ArmsAroundBothFeel".Localized());

            OutputText("Scenes.Katherinethreesome.ValaCommonStart.LockDoorConcentrateVala".Localized());

            OutputText("Scenes.Katherinethreesome.ValaCommonStart.TurnExpectingFindStripping".Localized());

            if (katherine.HasCock())
                OutputText("Scenes.Katherinethreesome.ValaCommonStart.CockJuttingOutHorizontally".Localized(
                    katherine.CockMultiple(" is hard enough that it's", "s are hard enough that they're")
                ));

            OutputText("Scenes.Katherinethreesome.ValaCommonStart.ParagraphBreak".Localized());
        }

        public void ValaCommonPostSex(bool theyWereFucking)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineValaTimesSex) == 0)
            {
                OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.LooksBackKathVala".Localized());

                OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.JawDropsStammersKath".Localized());

                if (theyWereFucking)
                {
                    OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.GigglesHandBellyKnotted".Localized());

                    OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.LooksEmbarrassedReachesKnot".Localized(
                        katherine.CockMultiple("", "s")
                    ));
                }

                OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.GivesKissDontEither".Localized());
            }
            else
                OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.GivesKathKissesFun".Localized());

            OutputText("Scenes.Katherinethreesome.ValaCommonPostSex.SighsRubsHalfAsleep".Localized(
                katherine.PlayerText()
            ));
        }

    }
}