using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<SlipperySqueezeScene>("coc:slippery_squeeze")]
    public class SlipperySqueezeScene : Scene
    {

        public override void Entry()
        {
            TheSlipperySqueeze();
        }

        public void TheSlipperySqueeze()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.WalkIntoWagon".Localized());

            Action androgyny = null;

            Action milker = null;

            if (CoC.Instance.Flags.Get(GameFlagIds.BazaarSlipperySqueezeVisited) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BazaarSlipperySqueezeVisited, CoC.Instance.Flags.Get(GameFlagIds.BazaarSlipperySqueezeVisited) + 1);

                OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.BunnyEaredClerk".Localized());

                OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.TakesHerMoments".Localized());

                OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.OddMannerisms".Localized());

                OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.ImplyingWhat".Localized());

                OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.VoiceFromBack".Localized());
            }
            else
                if (Random.Shared.Next(10) == 0)
                {
                    JoeyBigBalls();

                    return ;
                }
                else
                    if (!Player.HasKeyItem("Cock Milker") && CoC.Instance.Flags.Get(GameFlagIds.JoeyOfferedMilker) == 0 && !Player.HasKeyItem("Cock Milker - Installed At Whitney's Farm"))
                    {
                        OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.JoeyLeaning".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.JoeyOfferedMilker, 1);
                    }
                    else
                    {
                        OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.PursesLips".Localized());

                        OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.BlushesApologies".Localized());

                        OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.PriceList".Localized());

                        if (!Player.HasPerk(PerkIds.Androgyny))
                        {
                            OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.AndrogynousOption".Localized());

                            androgyny = JoeyAndrogyny;
                        }
                    }

            if (CoC.Instance.Flags.Get(GameFlagIds.JoeyOfferedMilker) > 0 && !Player.HasKeyItem("Cock Milker") && !Player.HasKeyItem("Cock Milker - Installed At Whitney's Farm"))
                milker = AskJoeyAboutOffer;

            ClearButtons();

            AddButton(0, "Common.Button.JoeyMassage".Localized(), JoeyMassage);

            AddNextButton("Common.Button.JoeysOffer".Localized(), milker);

            AddNextButton("Perks.Androgyny.Name".Localized(), androgyny);

            if (Holidays.IsEaster())
            {
                OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.AnotherOption".Localized());

                if (Player.Gems < 20)
                    OutputText("Scenes.Slipperysqueeze.TheSlipperySqueeze.CantAffordIt".Localized());
                else
                    AddNextButton("Common.Button.Sweetmassage".Localized(), JoeySweetMassage);
            }

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
        }

        public void AskJoeyAboutOffer()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.AskJoeyAboutOffer.EffeminateMasseuse".Localized());

            OutputText("Scenes.Slipperysqueeze.AskJoeyAboutOffer.WouldntSuppose".Localized());

            OutputText("Scenes.Slipperysqueeze.AskJoeyAboutOffer.TakeCockMilker".Localized());

            if (Player.Gems < 200)
            {
                OutputText("Scenes.Slipperysqueeze.AskJoeyAboutOffer.NotEnoughMoney".Localized());

                DoNext(() => NoMilkerPlzJoey());

                return ;
            }

            AddButton(0, "Common.Button.Yes".Localized(), BuyCockMilker);

            AddButton(1, "Common.Button.No".Localized(), NoMilkerPlzJoey);
        }

        public void NoMilkerPlzJoey()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.NoMilkerPlzJoey.DeclinesMilker".Localized());

            DoNext(() => TheSlipperySqueeze());
        }

        public void BuyCockMilker()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.BuyCockMilker.RushesBehindCurtain".Localized());

            OutputText("Scenes.Slipperysqueeze.BuyCockMilker.KeyItemAcquired".Localized());

            Player.Gems -= 200;

            Player.CreateKeyItem("Cock Milker", 0, 0, 0, 0);

            DoNext(() => TheSlipperySqueeze());
        }

        public void JoeyAndrogyny()
        {
            ClearOutput();

            if (Player.Gems < 500)
            {
                OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.NotEnoughGems".Localized());

                DoNext(() => TheSlipperySqueeze());

                return ;
            }

            Player.Gems -= 500;

            OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.TakesHand".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.CuteAsMe".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.MasculineAsWant".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.CuteOrMasculine".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.ExplainsFemBoy".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.CorkedVial".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.AsksMirror".Localized(
                 Player.Mf("handsome", "dear")
            ));

            OutputText("Scenes.Slipperysqueeze.JoeyAndrogyny.ThanksBunnyBoy".Localized());

            Player.CreatePerk(PerkIds.Androgyny, 0, 0, 0, 0);

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void JoeyMassage()
        {
            ClearOutput();

            if (Player.Gems < 10)
            {
                OutputText("Scenes.Slipperysqueeze.JoeyMassage.NoTenGems".Localized());

                Go<BazaarScene>();

                return ;
            }

            Player.SlimeFeed();

            Player.Gems -= 10;

            OutputText("Scenes.Slipperysqueeze.JoeyMassage.BouncesFeet".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyMassage.DemureHands".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyMassage.TensionReleased".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyMassage.MovesToArms".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyMassage.DismountsSqueeze".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Slipperysqueeze.JoeyMassage.WouldYouLikeOne".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeyMassage.SayEnd".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Slipperysqueeze.JoeyMassage.AcceptPotion".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), JoeysMassageWithEXTRASpooge);

                AddButton(1, "Common.Button.No".Localized(), JoeysMassageWifNoExtraJizz);
            }
            else
                DoNext(() => JoeysMassageWifNoExtraJizz());
        }

        public void JoeysMassageWifNoExtraJizz()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.ClimbsBack".Localized(
                 Player.AssDescript(),

                 Player.AssDescript()
            ));

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.MassageHeadsBack".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.AndVagina".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SlipperyFeeling".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.StandsTurns".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SlipperyHand".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.CatchesView".Localized());

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.TweaksTeases".Localized());
            else
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SinksFinger".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.NippleOther".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.BedCradles".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.RubyLips".Localized(
                     Player.CockHead()
                ));

                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.FreeHand".Localized(
                         Player.CockDescript(1)
                    ));

                    if (Player.Cocks.Count > 2)
                    {
                        OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.ProcessRepeated".Localized());

                        if (Player.Cocks.Count > 3)
                            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.UntilCoated".Localized());
                        else
                            OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.CoatingCock".Localized(
                                 Player.CockDescript(2)
                            ));
                    }
                }

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.MomentLater".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.RubsSack".Localized());

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.FingertipsProbing".Localized());
            }
            else
            {
                if (Player.HasVagina())
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.PressesLips".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SemenRing".Localized());

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SkillfulHand".Localized());
            }

            if (!Player.HasCock())
            {
                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SingleFinger".Localized());
                else
                {
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.LubedTongue".Localized());

                    if (Player.GetClitLength() >= 4)
                        OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.PracticedWhore".Localized());
                    else
                        OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.ProfessionalLicker".Localized());

                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.HardTweak".Localized());
                }

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.TrembleMoan".Localized());
            }
            else
            {
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.JoeysMouth".Localized());

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.GlobulesSpooge".Localized());

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.ClimbsOffLaps".Localized());

                if (Player.CumQ() >= 700)
                {
                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.SmileWidens".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.MassiveBelly".Localized());
                    else
                        OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.LittlePaunch".Localized());

                    OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.WontBeHungry".Localized());
                }

                OutputText("Scenes.Slipperysqueeze.JoeysMassageWifNoExtraJizz.PrancesAway".Localized());
            }

            Player.Orgasm("Generic");

            if (Player.LibidoPercent > 20)
                Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

            if (Player.LibidoPercent > 80)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.LibidoPercent > 60)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.SensitivityPercent > 40)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -.5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void JoeysMassageWithEXTRASpooge()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.PullsCork".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.InsideYou".Localized());
            else
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.InYourBalls".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.BeforeOrgasm".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.LittleBigger".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.ParagraphBreak".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.BrushesShaft".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.SLikeFiddles".Localized());
            else
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.LikeAFiddle".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.ExpertFingertip".Localized(
                 Player.CockHead()
            ));

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.RolledOnFront".Localized(
                 Player.AssDescript()
            ));

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.DontComplain".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.BallsPullRelax".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.DrainingSpunk".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.PutsLotion".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.RiversSpooge".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.PeriodBreak".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.ProddedInsistently".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.JawDrops".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.SpringsBack".Localized());

            if (!Player.HasFur())
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.OverSkin".Localized());
            else
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.ThroughFur".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.PeriodBreakAgain".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.SpecialPotion".Localized());

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.SqueezingTugging".Localized());
            else
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.CumCoatedFingers".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.GrindsCums".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.BlackOut".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.FloodedRoom".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.ComeBackAnyTime".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeysMassageWithEXTRASpooge.PoofyTail".Localized());

            Player.CumMultiplier += 2;

            Player.Orgasm("Dick");

            if (Player.LibidoPercent > 20)
                Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

            if (Player.LibidoPercent > 80)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.LibidoPercent > 60)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.SensitivityPercent > 40)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -4f));

            DoNext(() => ReturnToCamp(1));
        }

        public void JoeyBigBalls()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.JoeyBigBallsCounter) == 0)
            {
                OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.BeforeDoor".Localized());

                OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.PushesPanicked".Localized((noFur ? "bunny boy" : "lagomorph")));

                OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.WhyNoOne".Localized());

                if (Player.Corruption > 60)
                    OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.WishfulThinking".Localized());

                OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.ApplySuction".Localized());

                if (Player.Corruption > 70)
                    OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.WontGetWatch".Localized());
            }
            else
                OutputText("Scenes.Slipperysqueeze.JoeyBigBalls.KnowSomehow".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.SuckCumOut".Localized(), SuckOffJoeysGardenHose);

            AddButton(1, "Common.Button.NotHappening".Localized(), JoeyWanksItOut);

            CoC.Instance.Flags.Set(GameFlagIds.JoeyBigBallsCounter, CoC.Instance.Flags.Get(GameFlagIds.JoeyBigBallsCounter) + 1);
        }

        public void JoeyWanksItOut()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.JoeyWanksItOut.MasturbatesErectness".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.JoeyToldToMasturbateCounter, CoC.Instance.Flags.Get(GameFlagIds.JoeyToldToMasturbateCounter) + 1);

            Go<BazaarScene>();
        }

        public void SuckOffJoeysGardenHose()
        {
            ClearOutput();

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.SmilingImpishly".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.GrabsCheeks".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.NearlyChoke".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.CupsSack".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.RougedLips".Localized());

            if (Player.MinotaurAddicted())
                OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.WishingPotion".Localized());
            else
                OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.KeepAtIt".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.WhiteGoo".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.SemenSmile".Localized());

            Player.RefillHunger(60, false);

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.BellyGurgles".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.JoeyMoans".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.GaspForAir".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.GoodCause".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.KindOfFun".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.NastyStuff".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.BallsHeavier".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.IdeaBubbles".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.NutsShrink".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.PullIntoPuddle".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.LittleFun".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.SoSorry".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.WouldMakeSense".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.DragDeeper".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.CleanedBalls".Localized());

            OutputText("Scenes.Slipperysqueeze.SuckOffJoeysGardenHose.LooksForMop".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 70f));

            DoNext(() => ReturnToCamp(1));
        }

        public void JoeySweetMassage()
        {
            ClearOutput();

            Player.Gems -= 20;

            OutputText("Scenes.Slipperysqueeze.JoeySweetMassage.ClapsExcitedly".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeySweetMassage.SteamyAir".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeySweetMassage.WorksDown".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeySweetMassage.HopsOff".Localized());

            OutputText("Scenes.Slipperysqueeze.JoeySweetMassage.UnusualJism".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => EggsInButt(true));

            AddButton(1, "Common.Button.No".Localized(), () => EggsInButt(false));
        }

        public void EggsInButt(bool eggButt = false)
        {
            ClearOutput();

            if (eggButt)
            {
                OutputText("Scenes.Slipperysqueeze.EggsInButt.NodsFemboy".Localized());

                OutputText("Scenes.Slipperysqueeze.EggsInButt.ChocolatyCum".Localized());

                OutputText("Scenes.Slipperysqueeze.EggsInButt.RubsAsshole".Localized());
            }

            OutputText("Scenes.Slipperysqueeze.EggsInButt.HopsBack".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.WithoutBothering".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Slipperysqueeze.EggsInButt.OwnWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.CuntWord".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.EmptyMound".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.CandyCoated".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.CatchesGaze".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.HandsFind".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.VaginaSlot".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.AssholeSlot".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.SweetBounty".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Slipperysqueeze.EggsInButt.InAndOut".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.PentUpTension".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Slipperysqueeze.EggsInButt.LeakingPreCum".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.CuntWetter".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.AssholeWinks".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.ArchingBody".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Slipperysqueeze.EggsInButt.TakesHoldStrokes".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.ReachesBetween".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.ReachesBetweenAgain".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.FreeHandTweak".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.SkilledHand".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Slipperysqueeze.EggsInButt.PulsesSpasms".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.PussyTightens".Localized());
                else
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.AssholeTwitches".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.BodyTingles".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.GigglesSlipping".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.FavoritePart".Localized());

            if (Player.HasCock())
            {
                if (Player.BiggestCockArea() >= 30)
                    OutputText("Scenes.Slipperysqueeze.EggsInButt.ComparedToYou".Localized());
                else
                    if (Player.SmallestCockArea() < 6)
                        OutputText("Scenes.Slipperysqueeze.EggsInButt.ConsideredTiny".Localized());
            }
            else
                OutputText("Scenes.Slipperysqueeze.EggsInButt.SentencePeriod".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.SuckSlurp".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.CockRing".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.StrangeBulge".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.HeadrestUp".Localized());

            OutputText("Scenes.Slipperysqueeze.EggsInButt.RabbitTail".Localized());

            Player.Orgasm("Lips");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}