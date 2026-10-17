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
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Dungeons;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<HermCentaurScenes>("coc:herm_centaur_scenes")]
    public class HermCentaurScenes : Scene
    {

        public const int CENTAUR_KILLED = 1;

        public const int CENTAUR_RELEASED = 2;

        public const int CENTAUR_FUCKED = 3;

        public const int HORZGOG = 0;

        public const int DOGGECOCK = 1;

        public const int TENTACOCK = 2;

        public override void Entry()
        {

        }

        public void EncounterThePony()
        {

            OutputText("Scenes.Hermcentaurscenes.EncounterThePony.GiganticCentaur".Localized());

            if (Player.VaginalCapacity() < 100 || Player.AnalCapacity() < 100)
                OutputText("Scenes.Hermcentaurscenes.EncounterThePony.NotPrepared".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.EncounterThePony.HandleHer".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "herm_centaur")), RoomDungeon.ResumeFromFight);
        }

        public void BeatThePony(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Hermcentaurscenes.BeatThePony.HeavyThud".Localized());

            if (hpVictory)
                OutputText("Scenes.Hermcentaurscenes.BeatThePony.TriesToRise".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.BeatThePony.StretchForMace".Localized());

            OutputText("Scenes.Hermcentaurscenes.BeatThePony.FutileGesture".Localized());

            if (hpVictory)
                OutputText("Scenes.Hermcentaurscenes.BeatThePony.ResignedGlare".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.BeatThePony.LustfulGlare".Localized());

            OutputText("Scenes.Hermcentaurscenes.BeatThePony.UtterlySpent".Localized());

            if (Player.Lust >= 33)
                OutputText("Scenes.Hermcentaurscenes.BeatThePony.CockTeasing".Localized());

            OutputText("Scenes.Hermcentaurscenes.BeatThePony.SimplyExecute".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.FuckHer".Localized(), MaleFuckHer);
            else
                AddButtonDisabled(0, "Fuck Her", "This scene requires you to have a cock.");

            if (Player.HasVagina())
                AddButton(1, "Common.Button.GetFucked".Localized(), FemFuckHer);
            else
                AddButtonDisabled(1, "Get Fucked", "This scene requires you to have a vagina.");

            AddButton(5, "Scenes.Faerie.SexMenu.ButtonRelease".Localized(), LetHerGo);

            AddButton(6, "Common.Button.Kill".Localized(), KillHer);
        }

        public void KillHer()
        {
            ClearOutput();

            OutputText("Scenes.Hermcentaurscenes.KillHer.AdvanceDowned".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.D3CentaurDefeated, CENTAUR_KILLED);

            ClearButtons();

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void LetHerGo()
        {
            ClearOutput();

            OutputText("Scenes.Hermcentaurscenes.LetHerGo.DustOffArmor".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.D3CentaurDefeated, CENTAUR_RELEASED);

            ClearButtons();

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void MaleFuckHer()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3CentaurDefeated, CENTAUR_FUCKED);

            var y = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (y < 0)
                y = Player.SmallestCockIndex();

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.ShrugsArmor".Localized(((Player.Corruption > 50) ? "I'm gonna wreck it!" : "We do this my way.")));

            ShowImage("hermcentaur-win-male");

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.WhimperingMare".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.SLetter".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.ThrobbingMess".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.HumanHalfSighs".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.SlappingWetly".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.PropsHerself".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.QuiveringHindquarters".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.InhibitionsUnglued".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.ReluctantlyWithdraw".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.PuddleWhite".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.NoImFucking".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.OnyxHorn".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.PinkMilk".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.MightyShove".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.QuakesSensation".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.NotMuchDifference".Localized());

            if (Player.CumQ() >= 15000)
                OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.BloatsBigger".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.TricklesCum".Localized());

            if (Player.CumQ() >= 15000)
                OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.DraggingGut".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleFuckHer.MightHaveGottenAway".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void FemFuckHer()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.D3CentaurDefeated, CENTAUR_FUCKED);

            OutputText("Scenes.Hermcentaurscenes.FemFuckHer.TossingArmor".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemFuckHer.HungryGreenEyes".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemFuckHer.CommonSense".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Horsecock".Localized(), () => FemFuckHerII(HORZGOG));

            AddButton(1, "Common.Button.DogDick".Localized(), () => FemFuckHerII(DOGGECOCK));

            AddButton(2, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), () => FemFuckHerII(TENTACOCK));
        }

        public void FemFuckHerII(int cockType)
        {
            ClearOutput();

            if (cockType == TENTACOCK)
            {
                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.AffectionPat".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.LightDims".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.PurpleCrown".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.DemonVine".Localized());

                if (Player.GetClitLength() >= 6)
                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.CoilOfCock".Localized());
                else
                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.BendGrinds".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.MorePricks".Localized());

                Player.ButtChange(Player.AnalCapacity(), true, true, false);

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.WouldBeScreaming".Localized());

                if (Player.GetClitLength() >= 6)
                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.SwearItSpends".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.BucksHips".Localized());

                Player.CuntChange(Player.VaginalCapacity(), true, true, false);

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.TentacleDongs".Localized());

                ShowImage("hermcentaur-tentacle");

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.PleasureAndPain".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.ResignYourself".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.WakesLanding".Localized());

                OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.FewMinutes".Localized());
            }
            else
                if (cockType == DOGGECOCK)
                {
                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.AffectionPatAgain".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.LightDimsAgain".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.LaysUnder".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.DemonessCries".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.SimplyWontDo".Localized());

                    ShowImage("hermcentaur-penetration");

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.PleaseFuckMe".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.MoreLikeIt".Localized());

                    Player.CuntChange(Player.VaginalCapacity(), true, true, false);

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.RockingSlowly".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.FranticKnot".Localized());

                    Player.CuntChange(Player.VaginalCapacity() * 1.2f, true, true, false);

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.FullyStretched".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.KnotStretches".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.PowerfulInsemination".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.SemenStops".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.DropsInDirt".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.ComposesHerself".Localized());
                }
                else
                {
                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.AffectionPatThird".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.LightDimsThird".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.LowersBeneath".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.MasculineScent".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.FoldOfSkin".Localized());

                    if (Player.GetClitLength() >= 6)
                        OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.FemininePole".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.FingersGash".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.JacksOff".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.ExpandingHead".Localized());

                    ShowImage("hermcentaur-penetration");

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.WipingSpunk".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.NotGiveUp".Localized());

                    Player.CuntChange(Player.VaginalCapacity(), true, true, false);

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.BouncesUpDown".Localized());

                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.PracticallyWord".Localized());
                    else
                        OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.LiberallyWord".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.MareOrStallion".Localized());

                    OutputText("Scenes.Hermcentaurscenes.FemFuckHerII.SlidesOffPole".Localized());
                }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);

            ClearButtons();

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void InSovietCoCPonyRidesYou(bool hpVictory, bool pcCameWorms = false)
        {

            if (Player.HasCock() && Player.HasVagina())
                if (Random.Shared.Next(2) == 0)
                    MaleLoss(hpVictory);
                else
                    FemLoss(hpVictory);

            if (Player.HasCock() && !Player.HasVagina())
                MaleLoss(hpVictory);
            else
                if (Player.HasVagina() && !Player.HasCock())
                    FemLoss(hpVictory);
                else
                    if (!Player.HasVagina() && !Player.HasCock())
                        FemLoss(hpVictory);
        }

        public void FemLoss(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Hermcentaurscenes.FemLoss.DroppingDown".Localized());

            if (hpVictory)
                OutputText("Scenes.Hermcentaurscenes.FemLoss.TriesPullUp".Localized());
            else
            {
                OutputText("Scenes.Hermcentaurscenes.FemLoss.RumpInAir".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Hermcentaurscenes.FemLoss.VaginaSlot".Localized());
                else
                    OutputText("Scenes.Hermcentaurscenes.FemLoss.SelfSuffix".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Hermcentaurscenes.FemLoss.AndWord".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Hermcentaurscenes.FemLoss.SackSlot".Localized());
                    else
                        OutputText("Scenes.Hermcentaurscenes.FemLoss.CocksSlot".Localized());
                }

                OutputText("Scenes.Hermcentaurscenes.FemLoss.PervyEquine".Localized());
            }

            OutputText("Scenes.Hermcentaurscenes.FemLoss.SmirksTrots".Localized());

            if (Player.Tallness >= 7 * 12)
                OutputText("Scenes.Hermcentaurscenes.FemLoss.FeetTouchGround".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.BrushesHair".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.ShiftedSide".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.DemonMilkHits".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.BurgeoningHeat".Localized());

            if (!Player.HasVagina())
            {
                OutputText("Scenes.Hermcentaurscenes.FemLoss.WarmthBuilds".Localized());

                Player.CreateVagina();

                OutputText("Scenes.Hermcentaurscenes.FemLoss.ParagraphBreak".Localized());
            }

            OutputText("Scenes.Hermcentaurscenes.FemLoss.PuffyInflamed".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Hermcentaurscenes.FemLoss.TurgidMess".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.WarmthExcites".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.MovedOtherSide".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.WithYourVagina".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Hermcentaurscenes.FemLoss.AndWordSecond".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.HandsDiveFolds".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.PullsYouBack".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.RubberyLips".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.KnowingSmile".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Hermcentaurscenes.FemLoss.TingleBelly".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.HoovesStamp".Localized());

            ShowImage("hermcentaur-penetration");

            OutputText("Scenes.Hermcentaurscenes.FemLoss.HoldStill".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.GrabsHair".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.BiggestCockArea(), true, true, false);

            OutputText("Scenes.Hermcentaurscenes.FemLoss.SagsInto".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.HornShadow".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.NotHappyOnly".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.HorsecockRammed".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.NewFavoriteMare".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.WriggleWail".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLoss.DarknessTakes".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 50f), new DynStat(GameStats.Corruption, 50f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FemLossII);
        }

        public void FemLossII()
        {
            ClearOutput();

            ShowImage("badend-hermcentaur");

            OutputText("Scenes.Hermcentaurscenes.FemLossII.WakesHardDigs".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLossII.KnottedDogCock".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLossII.SlumpsDragged".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FemLossIII);
        }

        public void FemLossIII()
        {
            ClearOutput();

            ShowImage("badend-impbirth");

            OutputText("Scenes.Hermcentaurscenes.FemLossIII.MonthsLater".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLossIII.LatestLitter".Localized());

            OutputText("Scenes.Hermcentaurscenes.FemLossIII.RestOfDays".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)), new DynStat(GameStats.Libido, 50f), new DynStat(GameStats.Corruption, 50f));

            GameOver();
        }

        public void MaleLoss(bool hpVictory)
        {
            ClearOutput();

            var y = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (y < 0)
                y = Player.SmallestCockIndex();

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.DroppingDown".Localized());

            if (hpVictory)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.TriesPullUp".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.PullsOutArmor".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.SmirksTrots".Localized(
                Player.Mf("stud", "queen bitch")
            ));

            if (Player.Tallness >= 7 * 12)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.FeetTouchGround".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.BrushesHair".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.ShiftedSide".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.DemonMilkHits".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.CaptorSighs".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.BurgeoningHeat".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.PuffsUp".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.WarmthExcites".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.MovedOtherSide".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.HandsBodyFreed".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.PullsYouBack".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.EquineHalfAbove".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.ThickBalls".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.FitToBurst".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.SwollenBursting".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.OnlyWayTongue".Localized());

            if (Player.Body.Get<TonguePart>().Id != BodyPartTypes.Human)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.UncoilLength".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.BeginToLap".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.InhumanEntrance".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.MoistureDrizzles".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.FemininePrison".Localized(
                Player.Mf("man", "herm")
            ));

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.SitsUpQuick".Localized(
                y));

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.PressureVanishes".Localized(
                y));

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.BuxomRedhead".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.KnowingSmile".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.HaplessSituation".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.SomeoneElseJoins".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.D1OmnibusKilled) != 1)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.FamiliarOne".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.ReflectiveBra".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.D1OmnibusKilled) != 1)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.OverseerAsks".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.AsksWord".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.SecondHermGrabs".Localized());

            if (Player.Body.Get<HornsPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.HornsWord".Localized());
            else
                OutputText("Scenes.Hermcentaurscenes.MaleLoss.HeadWord".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.SidlesForward".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.SlurpingHard".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.VibrationsMoans".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.ClimaxSlows".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.BacksIntoWall".Localized());

            OutputText("Scenes.Hermcentaurscenes.MaleLoss.MateStepsAway".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MaleLossII);
        }

        public void MaleLossII()
        {
            ClearOutput();

            OutputText("Scenes.Hermcentaurscenes.MaleLossII.YearsRollBy".Localized());

            GameOver();
        }

        [Register("coc:keep_hook_hermcentaur")]
        public static void KeepHookHermCentaur() =>

            SceneInstance<HermCentaurScenes>.Get().EncounterThePony();

    }
}