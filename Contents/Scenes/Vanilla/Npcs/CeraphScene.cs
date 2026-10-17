using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<CeraphScene>("coc:ceraph")]
    public class CeraphScene : Scene
    {
        public int fetish
        {
            get => Flags.Get(GameFlagIds.PcFetish);

            set => Flags.Set(GameFlagIds.PcFetish, value);
        }

        public override void Entry()
        {

        }

        public void GiveFetish()
        {

            if (fetish < 10)
                fetish += 10;

            fetish = Math.Clamp(++fetish, 10, 13);
        }

        public void RemoveFetish()
        {

            fetish = Math.Clamp(--fetish, 10, 13);
        }

        public void ResetFetish()
        {

            fetish -= fetish % 10;
        }

        public bool HadFetish()
        {

            return fetish >= 10;
        }

        public bool HasExhibition()
        {

            return fetish % 10 >= 1;
        }

        public bool HasBondage()
        {

            return fetish % 10 >= 2;
        }

        public bool HasPacifism()
        {

            return fetish % 10 >= 3;
        }

        public void KillCeraph()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CeraphKilled, 1);

            OutputText("Scenes.Ceraphscene.KillCeraph.EnoughOfTrickery".Localized());

            OutputText("Scenes.Ceraphscene.KillCeraph.EyesWidenPanic".Localized());

            if (Player.Weapon.IsHolySword())
                OutputText("Scenes.Ceraphscene.KillCeraph.SwordAloftBehead".Localized());
            else
                if (Player.Weapon.IsBladed())
                    OutputText("Scenes.Ceraphscene.KillCeraph.MightySwingDiagonal".Localized());
                else
                    if (Player.Weapon.IsKnife())
                        OutputText("Scenes.Ceraphscene.KillCeraph.CrouchPlungeDagger".Localized());
                    else
                        if (Player.Weapon.IsRanged())
                            OutputText("Scenes.Ceraphscene.KillCeraph.PointAndFire".Localized());
                        else
                            if (Player.Weapon.IsBlunt())
                                OutputText("Scenes.Ceraphscene.KillCeraph.SwingPowerfullyChest".Localized());
                            else
                                if (Player.Weapon.IsSpear())
                                    OutputText("Scenes.Ceraphscene.KillCeraph.LungingAttackPierce".Localized());
                                else
                                    OutputText("Scenes.Ceraphscene.KillCeraph.GraspHeadBare".Localized());

            Player.UpgradeBeautifulSword();

            CombatScene.CleanupAfterCombat();

            DoNext(() => ReturnToCamp(1));
        }

        public void MaleFuckCeraphsPussy()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CeraphBeatenAndRapedCounter, CoC.Instance.Flags.Get(GameFlagIds.CeraphBeatenAndRapedCounter) + 1);

            if (!Player.IsTaur() && Player.BiggestCockArea() > 500 && (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1))
            {
                HugeCorruptionForceFuckCeraph();

                return;
            }

            if (Player.CockArea(0) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
            {
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ForceOntoBackPleasure".Localized());
                else
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ForceOntoBackPain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.LicksLipsBegs".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.WordsShameYou".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.WordsConfuseYou".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.DishonestWordsSlap".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.TooLateStripArmor".Localized());

                if (HasExhibition())
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ExhibitionistThrill".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.AndBeginStroking".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.EachOfWord".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.StiffCocksPrepare".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SquirmsInDirt".Localized());

                if (Player.CockArea(0) < 40)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ForcefulThrustSheath".Localized());
                else
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.MightyEffortOversized".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.InteriorInhumanlyHot".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.MewlsInMockProtest".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.PoundHarderFaster".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SquealsSultryVisage".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.PurplePussySpasms".Localized());

                if (Player.CumQ() >= 500 && Player.CumQ() < 1000)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.BellyBeginsRise".Localized());
                else
                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.BellyRisesQuickly".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.NubbyShaftTwitches".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ExhalesRubsBelly".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.GroanWishingShedStop".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.GroanWonderingWhy".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.BendOverRubBelly".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.LeaveDontSpareGlance".Localized());
            }
            else
            {
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ForceHerBackPleasure".Localized());
                else
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ForceHerBackPain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.LicksLipsBegsAgain".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.WordsShameYouAgain".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.WordsConfuseYouAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.DishonestWordsSlapAgain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.TooLateArmorStrip".Localized());

                if (HasExhibition())
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ExhibitionistThrillAgain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.AndBeginStrokingAgain".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.EachOfWordAgain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.StiffCocksPrepareAgain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SquishesFlatAgainstTwat".Localized(
                    Player.CockHead()
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.PrimaryWord".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SmoothSkinThighs".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.WrapsArmsWhines".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.HoldAnklesSexToy".Localized());

                if (Player.Cocks[0].Length < 18)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.CrestsBetweenBreasts".Localized(
                        Player.CockHead()
                    ));
                else
                    if (Player.Cocks[0].Length < 24)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SlidesOverCheek".Localized(
                            Player.CockHead()
                        ));
                    else
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SlidesOverFace".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SquealsBlubbersCums".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.UnderneathMammothErection".Localized());

                if (Player.Cocks[0].Length < 18)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.TitsWord".Localized());
                else
                    if (Player.Cocks[0].Length < 24)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.FaceWord".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.LongHairWord".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.DemonetteSquirts".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ProdigiousFertilityPuddle".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.ParagraphBreak".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.SighImmenseRelief".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.WishingSheStop".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.GroanWonderingWhyAgain".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.BendRubBellyAgain".Localized());

                OutputText("Scenes.Ceraphscene.MaleFuckCeraphsPussy.DontSpareGlance".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void RideCeraphsCockLikeaBAWSSexclamation11eleven()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CeraphBeatenAndRapedCounter, CoC.Instance.Flags.Get(GameFlagIds.CeraphBeatenAndRapedCounter) + 1);

            Player.SlimeFeed();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.ForceOntoBackPleasure".Localized());
            else
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.ForceOntoBackPain".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.LicksLipsBegs".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.WordsShameYou".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.WordsConfuseYou".Localized());
                else
                    OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.DishonestWordsSlap".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.TooLateStripArmor".Localized());

            if (HasExhibition())
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.ExhibitionistThrill".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.BeginTouchingReady".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.SinkDownPart".Localized());

            if (Player.VaginalCapacity() < 12)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.UnableToTakeEntire".Localized());
            else
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.AbleToTakeEntire".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.CriesMockProtest".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.VibratingNodulesWill".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.ChestWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.TitsWord".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.SquishSqueezeMilk".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.NipplesWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.JigglingMounds".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.FeelWonderfulPressed".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.FacadeDrops".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.OooohWhyDespoil".Localized());

            if (Player.Pregnancy.Incubation == 0)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.FeedingTheSwarms".Localized());
            else
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.BabyWontEnjoySoaked".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.SayEndParagraph".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.GroanWishingShedStop".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.GroanWonderingWhy".Localized());
                else
                    OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.RubYourBelly".Localized());

            OutputText("Scenes.Ceraphscene.RideCeraphsCockLikeaBAWSSexclamation11eleven.LeaveDontSpareGlance".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 32, 61);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.AdvancesOnYourProne".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.ProneWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.LustConsumedWord".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.WhisperingPullsPiercings".Localized());

            if (Player.Body.Get<EarsPart>().Pierced > 0)
                OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.DismissiveGestureMurmurs".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.HandOnForeheadDrains".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.WatchesYourCock".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.CockBounceHeartbeats".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.VaginaWetterAndWetter".Localized());
                else
                    OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.PartedLipsConfused".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.NewThoughtsSettle".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow.JudgingHowHotIdea".Localized());

            Player.Body.Get<EarsPart>().Pierced = 1;

            Player.EarsPShort = "green gem-stone ear-studs";

            GiveFetish();

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToCeraphAnGetPierced2()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.GigglesClosesIn".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.SlutWord".Localized());
            else
                if (Player.NippleLength < 1)
                    OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.CuteWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.TightWord".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.NipplesTellFetishes".Localized());

            if (Player.Body.Get<BreastRowPart>().NipplesPierced > 0)
                OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.DismissiveGestureMurmurs".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.NoChanceToReply".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.SmilesDownTeases".Localized());

            OutputText("Scenes.Ceraphscene.LoseToCeraphAnGetPierced2.BodyBeetRedStruggle".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 5f));

            Player.Body.Get<BreastRowPart>().NipplesPierced = 1;

            Player.NipplesPShort = "seamless black nipple-studs";

            Player.NipplesPLong = "Seamless black nipple-studs";

            GiveFetish();

            CombatScene.CleanupAfterCombat();
        }

        public void GetPiercedByCeraphLoss3()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.PullsDiamondPiercing".Localized());

            OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.IdeaTurnsYouOn".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.PreCumOozesFrom".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.EachOfWord".Localized());

                OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.YourCocksWord".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.FeminineMoistureDrools".Localized());
                else
                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.BodyAchesForRelease".Localized());

            OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.AmusedGrinYanks".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.GrabsYourCock".Localized());

                if (Player.Cocks[0].isPierced)
                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.OldPiercingClatters".Localized());

                OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.SnapsStudThrough".Localized());

                Player.Cocks[0].Pierced = 1;

                Player.Cocks[0].PShortDesc = "diamond cock-stud";

                Player.Cocks[0].PLongDesc = "Diamond cock-stud";
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.SpreadsYourLips".Localized());

                    if (Player.Vaginas[0].clitPierced > 0)
                        OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.OldPiercingClattersAgain".Localized());

                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.AholdFleshBase".Localized());

                    Player.Vaginas[0].clitPierced = 1;

                    Player.Vaginas[0].clitPShort = "diamond clit-stud";

                    Player.Vaginas[0].clitPLong = "Diamond clit-stud";
                }
                else
                {
                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.SnapsStudIntoBrow".Localized());

                    if (Player.eyebrowPierced > 0)
                        OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.DiscardsOldJewelry".Localized());

                    OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.HurtsMoreThanOught".Localized());

                    Player.eyebrowPierced = 1;

                    Player.EyebrowPShort = "diamond eyebrow-stud";

                    Player.EyebrowPLong = "Diamond eyebrow-stud";
                }

            GiveFetish();

            OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.FinishesNoRestraint".Localized());

            OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.SheLaughsTakeIt".Localized());

            OutputText("Scenes.Ceraphscene.GetPiercedByCeraphLoss3.TrembleImpotentFury".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, 5f));

            CombatScene.CleanupAfterCombat();
        }

        public void FinalEncounterLevelCeraphPiercingButtholeNipples()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.FinalEncounterLevelCeraphPiercingButtholeNipples.WigglesOutHarness".Localized());

            OutputText("Scenes.Ceraphscene.FinalEncounterLevelCeraphPiercingButtholeNipples.ShamesYouToAdmit".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut);

            AddButton(1, "Common.Button.No".Localized(), FinalCeraphEncounterChoiceNo);

            AddButton(2, "Common.Button.Fight".Localized(), FinalCeraphEncounterStartFight);
        }

        public void FinalCeraphEncounterChoiceNo()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.FinalCeraphEncounterChoiceNo.TellNoEmphatically".Localized());

            OutputText("Scenes.Ceraphscene.FinalCeraphEncounterChoiceNo.DepartsLeavingHorny".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FinalCeraphEncounterStartFight()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.FinalCeraphEncounterStartFight.TellNoLetKnow".Localized());

            if (Player.Corruption <= 50)
                OutputText("Scenes.Ceraphscene.FinalCeraphEncounterStartFight.StopDevilishScheme".Localized());
            else
                OutputText("Scenes.Ceraphscene.FinalCeraphEncounterStartFight.YoullBeHerMaster".Localized());

            OutputText("Scenes.Ceraphscene.FinalCeraphEncounterStartFight.SnarlsDropsHarness".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "ceraph")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.KnowShesRight".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.ChestTinglesWonder".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.CockRockHardWonder".Localized());
                else
                    OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.BodyHumsArousal".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.MindMadeUpNod".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.UndressingSmacksAss".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.AbsorbedThunkOfDoor".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.ManyMoreSlaves".Localized(
                Player.Mf("brother", "sister"),

                Player.Mf("him", "her")
            ));

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.WhipUncurled".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.NotSeeingHarm".Localized());

            if (Player.lipPierced > 0)
                OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.PreviousPiercingVanished".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.MistressSoKind".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.LickLipRing".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.CalledYourselfPet".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.LearnNewNames".Localized());

            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.BacksideWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.TailWord".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBecomeCeraphsFuckpetBondageToyBitchSlut.BackForthMistress".Localized());

            DoNext(() => AgreeToBeABitchPt2());
        }

        public void AgreeToBeABitchPt2()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.AgreeToBeABitchPt2.MonthsPassPureBliss".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Ceraphscene.AgreeToBeABitchPt2.MagicalCockRing".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBeABitchPt2.TwoWeeksEdging".Localized());

            OutputText("Scenes.Ceraphscene.AgreeToBeABitchPt2.ChampionUnrecognizable".Localized());

            GameOver();
        }

        public void CeraphFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.CeraphFirstTime.CuteGoblinEarrings".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTime.GoblinEarsPiercings".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTime.LetHerPutEarrings".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcMetCeraph, 1);

            AddButton(0, "Common.Button.Yes".Localized(), CeraphFirstTimeVolunteer);

            AddButton(1, "Common.Button.No".Localized(), CeraphFirstTimeDecline);
        }

        public void CeraphFirstTimeDecline()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeDecline.DeclinePetiteGirl".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeDecline.DemonIntroducesHerself".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeDecline.SuggestivePoseTweaks".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "ceraph")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void CeraphFirstTimeVolunteer()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.GoblinMotionsBendOver".Localized());

            if (Player.Body.Get<EarsPart>().Pierced > 0)
                OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.OldEarringsFallOut".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.EarringsSnapClick".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.SteadyAgainTransformed".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.ForwardnessBlush".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.BreaksReverieChuckle".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeVolunteer.SluttyDemonWalksAway".Localized());

            Player.Body.Get<EarsPart>().Pierced = 1;

            Player.EarsPShort = "green gem-stone ear-studs";

            GiveFetish();

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphFirstTimeRepeat()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeRepeat.SauntersOutDisapproving".Localized());

            OutputText("Scenes.Ceraphscene.CeraphFirstTimeRepeat.UncurlsFlamingWhip".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "ceraph")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void Level3CeraphOffer()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.Level3CeraphOffer.AppearsRidingCrop".Localized());

            OutputText("Scenes.Ceraphscene.Level3CeraphOffer.LetHerPierceYou".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), YesOHGODYESPIERCELEVEL3);

            AddButton(1, "Common.Button.No".Localized(), Level3CeraphOfferNo);
        }

        public void Level3CeraphOfferNo()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.Level3CeraphOfferNo.RefuseViolateBody".Localized());

            OutputText("Scenes.Ceraphscene.Level3CeraphOfferNo.MortalsHardWaySigh".Localized());

            OutputText("Scenes.Ceraphscene.Level3CeraphOfferNo.FightOnYourHands".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "ceraph")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void YesOHGODYESPIERCELEVEL3()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.NodBitingLip".Localized());

            OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.IdeaTurnsYouOn".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.PreCumOozesFrom".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.EachOfWord".Localized());

                OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.YourCocksWord".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.FeminineMoistureDrools".Localized());
                else
                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.BodyAchesForRelease".Localized());

            OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.AmusedGrinYanks".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.GrabsYourCock".Localized());

                if (Player.Cocks[0].isPierced)
                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.OldPiercingClatters".Localized());

                OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.SnapsStudThrough".Localized());

                Player.Cocks[0].Pierced = 1;

                Player.Cocks[0].PShortDesc = "seamless, diamond cock-stud";

                Player.Cocks[0].PLongDesc = "Seamless, diamond cock-stud";
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.SpreadsYourLips".Localized());

                    if (Player.Vaginas[0].clitPierced > 0)
                        OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.OldPiercingClattersAgain".Localized());

                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.GetsAholdFleshBase".Localized());

                    Player.Vaginas[0].clitPierced = 1;

                    Player.Vaginas[0].clitPShort = "seamless, diamond clit-stud";

                    Player.Vaginas[0].clitPLong = "seamless, diamond clit-stud";
                }
                else
                {
                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.AnnoyedLookSnapsBrow".Localized());

                    if (Player.eyebrowPierced > 0)
                        OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.SheDiscardsOldJewelry".Localized());

                    OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.HurtsMoreThanOught".Localized());

                    Player.eyebrowPierced = 1;

                    Player.EyebrowPShort = "seamless, diamond eyebrow-stud";

                    Player.EyebrowPLong = "seamless, diamond eyebrow-stud";
                }

            OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.FinishesFeelGrateful".Localized());

            OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.SheLaughsTakeIt".Localized());

            OutputText("Scenes.Ceraphscene.YesOHGODYESPIERCELEVEL3.TrembleRestrainedLust".Localized());

            GiveFetish();

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterCeraph()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.PcMetCeraph) == 0)
            {
                CeraphFirstTime();

                return;
            }

            if (!HasExhibition())
            {
                CeraphFirstTimeRepeat();

                return;
            }

            if (HasPacifism())
            {
                FinalEncounterLevelCeraphPiercingButtholeNipples();

                return;
            }

            if (HasBondage())
            {
                Level3CeraphOffer();

                return;
            }

            if (HasExhibition())
            {
                OutputText("Scenes.Ceraphscene.EncounterCeraph.HourExploringNothing".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }
        }

        public void WinRapeChoices()
        {
            ClearOutput();

            Action leave = CombatScene.CleanupAfterCombat;

            if (!CombatController.InCombat)
            {
                CurrentMonster = EnemyRegistry.Create(new Identifier("coc", "ceraph"));

                leave = () => SceneInstance<CeraphFollowerScene>.Get().CeraphFollowerAppearance();

                CombatController.Instance!.Context.Enemy.Lust = 100;

                CombatController.Instance!.Context.Enemy.Health = 2;

                if (Player.Lust < 34)
                    Player.Lust = 34;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphBeatenAndRapedCounter) >= 4)
                {
                    SceneInstance<CeraphFollowerScene>.Get().SubmissiveCeraphOffer();

                    return;
                }

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphscene.WinRapeChoices.CollapsesBeatenHeap".Localized());
            else
                OutputText("Scenes.Ceraphscene.WinRapeChoices.MasturbatesFutilely".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), null);

            AddButton(1, "Common.Button.RideHer".Localized(), null);

            AddButton(2, "Common.Button.Fuckherass".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Ceraphscene.WinRapeChoices.HaveYourWay".Localized());

                if (Player.HasCock())
                {
                    AddButton(0, "Common.Button.FuckHer".Localized(), MaleFuckCeraphsPussy);

                    if (Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()) != -1)
                        AddButton(2, "Common.Button.Fuckherass".Localized(), ButtRapeCeraph);
                    else
                        AddButton(2, "Common.Button.Fuckherass".Localized(), null);
                }

                if (Player.HasVagina())
                {
                    AddButton(1, "Common.Button.RideHer".Localized(), RideCeraphsCockLikeaBAWSSexclamation11eleven);

                    if (Player.BiggestBreastSize() >= 4 && Player.Armor?.Id == ArmorIds.LightMaidenArmor)
                        AddButton(3, "Common.Button.Titfuck".Localized(), () => Contents.Items.Behaviors.Vanilla.LustyMaidensArmorBehavior.Paizuri(CombatController.Instance!.Context.Enemy));
                }
            }

            if (Player.Armor?.Id == ArmorIds.NaughtyNunsHabit)
                AddButton(4, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), () => Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.CockWorship());

            if (!SceneInstance<CeraphFollowerScene>.Get().CeraphIsFollower())
                AddButton(5, "Common.Button.Enough".Localized(), KillCeraph);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<CampScene>());
        }

        public void LoseFUCKME()
        {

            CoC.Instance.Flags.Set(GameFlagIds.CeraphBeatenAndRapedCounter, 0);

            if (!HasExhibition())
                LoseToCeraphAndGetPiercingsInEarsOrSomethingSomehow();
            else
                if (!HasBondage())
                    LoseToCeraphAnGetPierced2();
                else
                    if (!HasPacifism())
                        GetPiercedByCeraphLoss3();
                    else
                        CeraphRapesYouBADDAWGYODIGGITY();
        }

        public void CeraphRapesYouBADDAWGYODIGGITY()
        {
            ClearOutput();

            Player.SlimeFeed();

            if ((!CombatController.InCombat) && !HasPacifism())
                OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.TouchesForeheadFetishes".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.ThrowsHeadBackLaughs".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.SlumpDownDefeated".Localized());
            else
                OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.GiveUpMasturbating".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.ReallyIsThatIt".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.StepsFootOnChest".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.RightBetweenBreasts".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.FootArchedHighHeel".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.PinchesErectNipple".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.ConfusedMomentHandsLegs".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.SkyBlockedByShadow".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.DemonCockFlopsNose".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.MoansEatOutBox".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.RockingHipsGrinds".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.BlackOutSeedHair".Localized());

            if (Player.Cocks.Count > 0)
            {
                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.EachOfYWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.YWord".Localized());

                OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.ChestTwitchingPrecum".Localized(
                    Player.MultiCockDescript()
                ));
            }

            if (Player.HasVagina())
                OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.TwitchesAchingDesire".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.PerverseSexualityNeed".Localized());

            OutputText("Scenes.Ceraphscene.CeraphRapesYouBADDAWGYODIGGITY.WhipUncurlsCantSeparate".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 200f), new DynStat(GameStats.Corruption, 1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void HugeCorruptionForceFuckCeraph()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CeraphBeatenAndRapedCounter, CoC.Instance.Flags.Get(GameFlagIds.CeraphBeatenAndRapedCounter) + 2);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.LaughAtCeraphsBeaten".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.BeatenProneWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ProneMasturbatingWord".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.FormWord".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.DominatrixFrigsSlit".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SoreLoserDropArmor".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.LargestWord".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.BloodPumpInflates".Localized(
                Player.CockDescript(x)
            ));

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.HandsBusyPumping".Localized());
            else
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.BothHandsBusyGroin".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.DropSwollenMass".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.MagicWordsPlease".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.PatCockAskExgartuan".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.BlinkFocusPowers".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.PleasureBloomsMidsection".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.FlawlessThighsSlip".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.DoesntMindReceiving".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.DarkMagicTwisting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ThroatBalloonsGirth".Localized());

            if (Player.Cocks[x].Length < 68)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ImpossibleNotLongEnough".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SqueezedMassagedBy".Localized());

            if (Player.Cocks[x].Length < 68)
                OutputText(NumberUtils.Num2Text((int)(Player.Cocks[x].Length / 12)));
            else
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SixWord".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.FeetOfLoveTunnel".Localized());

            if (Player.Cocks[x].Length > 84)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.StillMoreDickWet".Localized());
            else
            {
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SighHappilyRubbing".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.GroinWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SheathWord".Localized());

                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.AgainstSlipperySlit".Localized());
            }

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.OnceFormidableSighs".Localized());

            if (Player.CumQ() < 100)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SquirtLoadIntoDirt".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SquirtCopiousCum".Localized());
                else
                    OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ExpelHugeWaves".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ParagraphBreakThird".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.PushUsedFuckToy".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.MarvelAtCumOther".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.MarvelAtCumOthers".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.PussyGapesInches".Localized(
                NumberUtils.Num2Text((int)(Player.Cocks[x].Thickness + 1))
            ));

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.GaspsGathersSperm".Localized());

            if (!CombatController.InCombat)
            {
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SayEnd".Localized());

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.RarerThanYouKnow".Localized());

            if (HasExhibition())
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.RemoveMagicKeepPiercing".Localized());
            else
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.WillingGiveGems".Localized());

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.SayEndParagraph".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
            {
                OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.ExgartuanMumblesLoveMe".Localized());

                Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 1, 5);
            }

            OutputText("Scenes.Ceraphscene.HugeCorruptionForceFuckCeraph.AcceptCeraphsOffer".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), CeraphsNiceOffer);

            AddButton(1, "Common.Button.No".Localized(), CombatScene.CleanupAfterCombat);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));
        }

        public void CeraphsNiceOffer()
        {
            ClearOutput();

            if (HasExhibition())
            {
                OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.TouchesForeheadClaws".Localized());

                if (HasPacifism())
                    OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.PacifismGone".Localized());
                else
                    if (HasBondage())
                        OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.BondageFetishGone".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.ExhibitionismGone".Localized());

                RemoveFetish();
            }
            else
            {
                OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.ShakesJismFromHair".Localized());

                Player.Gems += 10;
            }

            OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.AbusedDemonDomLaughs".Localized());

            OutputText("Scenes.Ceraphscene.CeraphsNiceOffer.LaysBackFallsAsleep".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void FriendlyNeighborhoodSpiderManCeraph()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.CompulsionToStripNaked".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.WithoutThoughtDisrobe".Localized());
            else
                if (Player.Corruption > 33)
                    OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.SquirmWithinArmor".Localized());
                else
                    OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.StifleLaughRidiculous".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.YourWord".Localized());

            if (new[] { BodyPartTypes.Plain, BodyPartTypes.Fur }.Contains(Player.Body.Get<SkinPart>().Id))
                OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.SkinBurnsWord".Localized());
            else
                if (Player.HasScales())
                    OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.ScalesBurnWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.BurnsWord".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.HotRagingFetishLust".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.WrenchGazeAlienEyes".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.LikeWhatYouSee".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.HowHotImMaking".Localized());
            else
                OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.CantTakeEyesOff".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.SheSaysSayEnd".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.ReadyWeaponLaughs".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.FightMeInBuff".Localized());
            else
                OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.FightSupposeWeCan".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.SayEndParagraph".Localized());

            OutputText("Scenes.Ceraphscene.FriendlyNeighborhoodSpiderManCeraph.LiftsHeeledFootWinks".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((5 + Player.Corruption / 10 + Player.Libido / 20))), new DynStat(GameStats.Corruption, 0f));

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), StartAFightWithCeraph);

            AddButton(1, "Common.Button.Trade".Localized(), TradeCeraphSomething);

            AddButton(2, "Common.Button.Run".Localized(), RunFromCeraphsNiceDeal);
        }

        public void StartAFightWithCeraph()
        {

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "ceraph")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);
        }

        public void RunFromCeraphsNiceDeal()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.RunFromCeraphsNiceDeal.YouWord".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Ceraphscene.RunFromCeraphsNiceDeal.GrabEquipmentAnd".Localized());

            OutputText("Scenes.Ceraphscene.RunFromCeraphsNiceDeal.HastyRetreatTrickster".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (10 + fetish * 10)));

            DoNext(() => ReturnToCamp(1));
        }

        public void TradeCeraphSomething()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.TradeCeraphSomething.LooksUpAndDown".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Liqueur".Localized(), null);

            AddButton(1, "Common.Button.Endowment".Localized(), null);

            AddButton(2, "Common.Button.Armor".Localized(), null);

            if (Player.HasItem(ItemIds.BimboLiqueur))
            {
                OutputText("Scenes.Ceraphscene.TradeCeraphSomething.SmilesWidelyBimboLiqueur".Localized());

                if (HasBondage())
                    OutputText("Scenes.Ceraphscene.TradeCeraphSomething.AllTheFetishesWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.TradeCeraphSomething.TheFetishWord".Localized());

                OutputText("Scenes.Ceraphscene.TradeCeraphSomething.PlacedUponYouTrade".Localized());

                if ((int)Player.Gender > 0 || Player.BiggestBreastSize() > 2)
                    OutputText("Scenes.Ceraphscene.TradeCeraphSomething.RatherKeepEndowment".Localized());

                OutputText("Scenes.Ceraphscene.TradeCeraphSomething.MaybeBetterEquipment".Localized());

                AddButton(0, "Common.Button.Liqueur".Localized(), TradeCeraphBimboLiqueur);

                if (!Player.IsGenderless() || Player.BiggestBreastSize() > 2)
                    AddButton(1, "Common.Button.Endowment".Localized(), TradeCeraphCrotchityBits);
            }
            else
                if ((int)Player.Gender > 0 || Player.BiggestBreastSize() > 2)
                {
                    OutputText("Scenes.Ceraphscene.TradeCeraphSomething.PerksUpLeering".Localized());

                    if (HasExhibition())
                        OutputText("Scenes.Ceraphscene.TradeCeraphSomething.YourFetishForYou".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.TradeCeraphSomething.OneFetishForYou".Localized());

                    AddButton(1, "Common.Button.Endowment".Localized(), TradeCeraphCrotchityBits);
                }
                else
                {
                    OutputText("Scenes.Ceraphscene.TradeCeraphSomething.FrownsNothingWorthTrading".Localized());

                    if (!HasBondage())
                    {
                        AddButton(2, "Common.Button.Armor".Localized(), GetCeraphsArmor);

                        OutputText("Scenes.Ceraphscene.TradeCeraphSomething.DontFretMistress".Localized(
                            Player.Mf("pretty little head", "charming little head")
                        ));
                    }

                    OutputText("Scenes.Ceraphscene.TradeCeraphSomething.SayEndParagraph".Localized());
                }

            AddButton(14, "Common.Button.Leave".Localized(), RunFromCeraphsNiceDeal);
        }

        public void GetCeraphsArmor()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.GetCeraphsArmor.SmilesKnowinglyArmor".Localized());

            InventoryScene.TakeItem(ItemIds.SeductiveArmor, () => ReturnToCamp(1));
        }

        public void TradeCeraphBimboLiqueur()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.TradeCeraphBimboLiqueur.GrinsCheshireDripping".Localized());

            OutputText("Scenes.Ceraphscene.TradeCeraphBimboLiqueur.MentionDealGlares".Localized());

            Player.ConsumeItem(ItemIds.BimboLiqueur);

            Player.HealthChange(-4);

            ResetFetish();

            Player.ApplyStats(new DynStat(GameStats.Lust, -20f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TradeCeraphCrotchityBits()
        {
            ClearOutput();

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
            {
                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.DisembodiedVoiceRoars".Localized());

                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.PerturbedDoesntPursue".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, -20f));

                DoNext(() => ReturnToCamp(1));

                return;
            }

            var choices = new List<int>();

            int choice = 0;

            int temp2 = 0;

            var temp = Player.Cocks.Count;

            while (temp > 0)
            {
                temp--;

                choices.Add(1);
            }

            if (Player.HasVagina())
            {
                choices.Add(2);

                if (Random.Shared.Next(2) == 0)
                    choices.Add(2);
            }

            temp = Player.BreastRows.Count;

            while (temp > 0)
            {
                temp--;

                if (Player.BreastRows[temp].BreastRating > 2)
                    choices.Add(3);
            }

            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.GrinsPullsTail".Localized());

            choice = choices[Random.Shared.Next(choices.Count)];

            if (choice == 0)
                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.ErrorGetFenoxo".Localized());
            else
                if (choice == 1)
                {
                    temp2 = Random.Shared.Next(Player.CockTotal());

                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.ShePipesUp".Localized());

                    if (Player.Cocks[temp2].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.HorseCockNice".Localized());
                    else
                        if (new[] { BodyPartTypes.Dog, BodyPartTypes.Wolf, BodyPartTypes.Fox }.Contains(Player.Cocks[temp2].Id))
                            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.CanineCockKnots".Localized());
                        else
                            if (Player.Cocks[temp2].Id == BodyPartTypes.Demon)
                                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.DemonCockFamiliar".Localized());
                            else
                                if (Player.Cocks[temp2].Id == BodyPartTypes.Cat)
                                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.KittyDickSpines".Localized());
                                else
                                    if (Player.Cocks[temp2].Id == BodyPartTypes.Lizard)
                                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.SnakeDickBulbous".Localized());
                                    else
                                        if (Player.Cocks[temp2].Id == new Identifier("coc", "tentacle"))
                                            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.TentacleCockLucky".Localized());
                                        else
                                            if (Player.Cocks[temp2].Id == BodyPartTypes.Anemone)
                                                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.AnemoneCockMarvelous".Localized());
                                            else
                                                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.NormalCockDroll".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.NotOnlyPrickPacking".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.TheOtherOneWord".Localized());
                        else
                            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.OneOfOthers".Localized());
                    }

                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.SentencePeriod".Localized());

                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.SayEndParagraph".Localized());
                }
                else
                    if (choice == 2)
                    {
                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.GazeDropsDownBelow".Localized());

                        if (Player.Wetness() >= 5)
                            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.JuicyBoxSoaking".Localized());
                        else
                            if (Player.Wetness() >= 3)
                                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.NiceWetPussyBlend".Localized());
                            else
                                if (Player.VaginalCapacity() >= 150)
                                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.TerrificallyGapedHole".Localized());
                                else
                                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.PrettyPussyReplaceMouth".Localized());

                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.SayEndParagraphAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.HandsReachCupChest".Localized(
                            Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.BiggestBreastSize() > 14)
                            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.OhWowHowWalk".Localized());
                        else
                            if (Player.BiggestBreastSize() > 6)
                                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.AreTheyReal".Localized());
                            else
                                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.BitSmallTryGro".Localized());

                        OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.SayEndParagraphThird".Localized());
                    }

            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.LongSilentMomentPoints".Localized());

            if (choice == 1)
            {
                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.ManhoodWord".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.PluralS".Localized());
            }
            else
                if (choice == 2)
                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.VaginaWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.BreastsWord".Localized());

            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.IllHaveThat".Localized());

            if (choice <= 2)
                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.OneWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.TwoWord".Localized());

            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.OfThoseHoldStill".Localized());

            OutputText("Scenes.Ceraphscene.TradeCeraphCrotchityBits.LetHerTakeIt".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, choice);

            AddButton(0, "Common.Button.Yes".Localized(), GiveUpYourBallsOrWhateverForLessFetishes);

            AddButton(1, "Common.Button.No".Localized(), BailBeforeCeraphYoinksYourNads);
        }

        public void BailBeforeCeraphYoinksYourNads()
        {
            ClearOutput();

            OutputText("Scenes.Ceraphscene.BailBeforeCeraphYoinksYourNads.ChangedMindTakeOff".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveUpYourBallsOrWhateverForLessFetishes()
        {
            ClearOutput();

            int x = 0;

            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.HoldStillDreading".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CeraphGenitalStealChoice) == 1)
            {
                x = Random.Shared.Next(Player.CockTotal());

                OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.FingersUnexpectedlyHot".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Cocks[x].Thickness >= 4)
                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.CompressingGirthShaft".Localized());

                OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.SlowlyClosingWarmer".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0 && Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.AndBallsWord".Localized());

                    Player.Balls = 0;

                    Player.BallSize = 1;
                }

                if (Player.CockTotal() == 1)
                    Player.RemoveStatusEffect(StatusEffectIds.Infested);

                OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.ArcaneMarkHand".Localized());

                OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.RunsFingerFireworks".Localized());

                OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.PirouettesDancingAway".Localized());

                Player.RemoveCock(x, 1);

                RemoveFetish();

                CoC.Instance.Flags.Set(GameFlagIds.CeraphDicksOwned, CoC.Instance.Flags.Get(GameFlagIds.CeraphDicksOwned) + 1);

                Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 5f));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CeraphGenitalStealChoice) == 2)
                {
                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.FingernailsStabSkin".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.SlipsFingerIntoPussy".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.InstructsDontMindFeelings".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.WorkJawConsternation".Localized(
                        Player.VaginaDescript(0)
                    ));

                    Player.RemoveVagina(0, 1);

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

                    RemoveFetish();

                    CoC.Instance.Flags.Set(GameFlagIds.CeraphPussiesOwned, CoC.Instance.Flags.Get(GameFlagIds.CeraphPussiesOwned) + 1);
                }
                else
                {
                    x = Player.BiggestTitRow();

                    if (Player.BreastRows[x].Fuckable)
                        CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, 4);
                    else
                        if (Player.LactationQ() >= 100)
                            CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, 5);
                        else
                            CoC.Instance.Flags.Set(GameFlagIds.CeraphGenitalStealChoice, 3);

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.BouncesChestInHands".Localized(
                        Player.BreastDescript(x)
                    ));

                    if (x < Player.BreastRows.Count - 1 && Player.BreastRows.Count > 1)
                    {
                        if (x >= Player.BreastRows.Count - 2)
                            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.TorsoShiftedBreastsUp".Localized(
                                Player.BreastDescript(x + 1)
                            ));
                        else
                            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.TorsoShiftedOtherBreasts".Localized());

                        Player.RemoveBreastRow(x, 1);
                    }
                    else
                        if (x == 0)
                        {
                            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.FlatChestTinyNipples".Localized());

                            Player.BreastRows[0].BreastRating = 0;

                            Player.NippleLength = .25f;

                            Player.BreastRows[0].Breasts = 2;

                            Player.BreastRows[0].NipplesPerBreast = 1;

                            Player.BreastRows[0].Fuckable = false;

                            Player.BreastRows[0].Lactation = 0;
                        }
                        else
                        {
                            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.TorsoLeavesEmptyVoid".Localized());

                            Player.RemoveBreastRow(x, 1);
                        }

                    if (Player.BreastRows.Count == 0)
                        OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.ErrorContactFenoxo".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.ParagraphBreak".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.GaspWhenCeraph".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CeraphGenitalStealChoice) == 3)
                        OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.RollsNipplesInHands".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.CeraphGenitalStealChoice) == 4)
                            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.FingertipsInsideNippleCunts".Localized());
                        else
                            OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.SqueezesSquirtOfMilk".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.WeakInTheKnees".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.HardCockInTits".Localized());

                    OutputText("Scenes.Ceraphscene.GiveUpYourBallsOrWhateverForLessFetishes.TurnsPrancesTweaking".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 20f), new DynStat(GameStats.Corruption, 5f));

                    RemoveFetish();

                    CoC.Instance.Flags.Set(GameFlagIds.CeraphTitsOwned, CoC.Instance.Flags.Get(GameFlagIds.CeraphTitsOwned) + 1);
                }

            DoNext(() => ReturnToCamp(1));
        }

        public void CeraphBodyPartDreams()
        {

            var choices = new List<int>();

            float choice = 0;

            float subChoice = 0;

            int temp = CoC.Instance.Flags.Get(GameFlagIds.CeraphDicksOwned);

            while (temp > 0)
            {
                temp--;

                choices.Add(1);
            }

            temp = CoC.Instance.Flags.Get(GameFlagIds.CeraphPussiesOwned);

            while (temp > 0)
            {
                temp--;

                choices.Add(2);
            }

            temp = CoC.Instance.Flags.Get(GameFlagIds.CeraphTitsOwned);

            while (temp > 0)
            {
                temp--;

                choices.Add(3);
            }

            choice = choices[Random.Shared.Next(choices.Count)];

            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SomethingOddWhileSleep".Localized());

            if (choice == 2)
            {
                subChoice = Random.Shared.Next(4);

                if (subChoice == 0)
                {
                    OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.OddSensationNethers".Localized());

                    if (!Player.HasVagina())
                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DontHaveVagina".Localized());
                    else
                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.NotYourVagina".Localized());

                    OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.MustBeSexStolen".Localized());

                    OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.ChorusesVoicesMoan".Localized());

                    OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.UselessCumDump".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, 200f));
                }
                else
                    if (subChoice == 1)
                    {
                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.InsistentSuctionLips".Localized());

                        if (!Player.HasVagina())
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DontHaveClitAnymore".Localized());
                        else
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.ThatsNotYourClit".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.ThoughtOfTormentor".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PetMewlsThroughLips".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.BothNodVigorously".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.EmboldenedArchToes".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DemonDomFootFucks".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.WarmthPlastersFeet".Localized());

                        Player.Orgasm("Vaginal");

                        Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));
                    }
                    else
                        if (subChoice == 2)
                        {
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.ProddingAtVagina".Localized());

                            if (!Player.HasVagina())
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DontHavePussyAnymore".Localized());
                            else
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PussyFeelsWrong".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PickingUpFeelingsCunt".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.NubbyNodulesPressWalls".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SpeechIncreasesPace".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.VisionFeelingsFade".Localized());

                            Player.Orgasm("Vaginal");

                            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 1f));
                        }
                        else
                        {
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SomethingFillingYouAhhh".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.MultitudeIntrudersMove".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.FatShaftWithdrawnAgain".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PussyGoodCubbyhole".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.GodsSheFuckingHuge".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.MagicCockFitsNicely".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.QuimStainedCrotch".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.ThanksExcellentFuckToy".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.JoltAwakeSweating".Localized());

                            Player.Orgasm("Vaginal");

                            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 1f));
                        }
            }
            else
                if (choice == 3)
                {
                    subChoice = Random.Shared.Next(3);

                    if (subChoice == 0)
                    {
                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.TitsDraggingCaveFloor".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PanicWriggleTitTail".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SpiralOfPain".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 5)));

                        Player.Orgasm("Tits", false);
                    }
                    else
                        if (subChoice == 1)
                        {
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DreamAboutOldTits".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.VoiceOutOfNowhere".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.EroticEnergySizzles".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 5)));

                            Player.Orgasm("Tits", false);
                        }
                        else
                        {
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SeeCeraphPinkVoid".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.BlowsIntoChestGrows".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.WhistlesDuoIncubi".Localized());

                            if (silly)
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PinkFlamingoWord".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SunglassesTossesBreast".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DemonicTrioGame".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.GameTossAfterToss".Localized());

                            if (silly)
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.HorsecocksWord".Localized());
                            else
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DemonicCocksWord".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.RiseFromSandPoles".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.IncubiAdvantageTrouncing".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.HulkingDemonTossesBreast".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.IncubiHighFiveBreast".Localized());

                            if (silly)
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PossibleRamifications".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DismissAsCorruption".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 5)));

                            Player.Orgasm("Tits", false);
                        }
                }
                else
                {
                    subChoice = Random.Shared.Next(10);

                    if (subChoice <= 2)
                    {
                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DrippingScaldingPain".Localized());

                        OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.ProcessRepeatedTwice".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Lust, 200f));
                    }
                    else
                        if (subChoice < 5)
                        {
                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.FirstWarmHandBuzz".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.TightensWhipPhantom".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.LaughsSlapsFaceTail".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SlapsAgainLieStill".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SighWatchingMistressShow".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.PuffyEntranceBouncesLower".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.FlattenedSpadeTailSlap".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.FightToStayStill".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.GoodPetMoan".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.OhGodsSoPerfect".Localized());

                            OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SensationFadingShift".Localized());

                            Player.Orgasm("Dick");

                            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
                        }
                        else
                            if (subChoice < 7)
                            {
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.NightConstantlyAroused".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.MouthFilledByDick".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SceneFadesFeathery".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Lust, 100f));

                                Player.HoursSinceCum += 30;
                            }
                            else
                            {
                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DickFeelsDamnGood".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.GushingGirlGoo".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.GlanceDownFantasticBust".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.SmackSwatsBottom".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.MistressAsksEnjoying".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.DemonettesAnswerYes".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.CuntsWord".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.BodiesWritheCeraphWhispers".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.NodAndWhimper".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.CoosPinchesNipple".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.AnythingButThatGag".Localized());

                                OutputText("Scenes.Ceraphscene.CeraphBodyPartDreams.WakeHeartPounding".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 5)), new DynStat(GameStats.Corruption, .5f));

                                Player.HoursSinceCum += 100;
                            }
                }

            DoNext(PlayerMenu);
        }

        public void ButtRapeCeraph()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CeraphBeatenAndRapedCounter, CoC.Instance.Flags.Get(GameFlagIds.CeraphBeatenAndRapedCounter) + 1);

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.DefeatedLiesFaceDown".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.TailHangingLimp".Localized());
            else
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.RumpUpTailPlunges".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.ReachDownSlapAss".Localized());

            if (Player.Strength < 75)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.WholeBodyJiggle".Localized());
            else
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.JiggleSlideInches".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.ShuttingHerUpArmor".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.NowNakedGrabThighs".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.WetWord".Localized());
            else
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.AchinglyWetWord".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.VulvaFlaresFlower".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.GroansDontStickIn".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.RubAgainstJuiceFountain".Localized(
                Player.CockHead(x),

                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.OtherRubsButtCheeks".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Ceraphscene.ButtRapeCeraph.RestMaleGenitalsSlide".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.SqueaksInDistress".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.GruntsWoundedAnimal".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.CuntJuiceSplatters".Localized());
            else
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.CuntJuiceDribbles".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.ParagraphBreak".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.WhimpersBegsRaping".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.PullHarderSplitting".Localized());

            if (!Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.BarelyTouchingGroin".Localized());
            else
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.RubbingYourSheath".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.AssWarmMelt".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.SlamItBackHome".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.DontLetGoTail".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.RopesJismRectum".Localized());

            if (Player.CumQ() >= 500 && Player.CumQ() < 1500)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.BellyBulgesDeposit".Localized());
            else
                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Ceraphscene.ButtRapeCeraph.BellyBurblesSwells".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.PullOutCeraphs".Localized());

            if (Player.Cocks[x].Thickness < 3)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.SlightlyGapedWord".Localized());
            else
                if (Player.Cocks[x].Thickness < 5)
                    OutputText("Scenes.Ceraphscene.ButtRapeCeraph.GapedWord".Localized());
                else
                    OutputText("Scenes.Ceraphscene.ButtRapeCeraph.GapingWideWord".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.AssholeLeaksSpunk".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Ceraphscene.ButtRapeCeraph.LittleBitYourself".Localized());

            OutputText("Scenes.Ceraphscene.ButtRapeCeraph.DropLikeDiscardedRag".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

    }
}