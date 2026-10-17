using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<SuccubusGardenerScenes>("coc:succubus_gardener_scenes")]
    public class SuccubusGardenerScenes : Scene
    {

        public const int GARDENER_LEFT = 1;

        public const int GARDENER_FUCKED = 2;

        public const int GARDENER_KILLED = 3;

        public static void ResumeFromFight()
        {
            CombatController.InCombat = false;

            CombatController.Instance = null;

            UI.Components.Views.GameView.Instance?.HideCombatStats();

            RoomDungeon.ResumeFromFight();
        }

        [Register("coc:keep_hook_gardener_surrender")]
        public static void KeepHookGardenerSurrender() =>

            SceneInstance<SuccubusGardenerScenes>.Get().SurrenderToTheGardener();

        public override void Entry()
        {

        }

        public bool GardenerDefeated()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.D3GardenerDefeated) > 0)
                return true;

            return false;
        }

        public bool GardenerKilled()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.D3GardenerDefeated) == 3)
                return true;

            return false;
        }

        public bool GardenerFucked()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.D3GardenerDefeated) == 2)
                return true;

            return false;
        }

        public bool GardenerLeft()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.D3GardenerDefeated) == 1)
                return true;

            return false;
        }

        public void FuckUpTheGardener(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.FuckUpTheGardener.DropsToHerKnees".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.EndHer".Localized(), EndHer);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveHer);

            AddButton(2, "Common.Button.FuckHer".Localized(), TentaFail);
        }

        public void TentaFail()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.NoWayYoure".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.CuntKissersWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.CockSuckersWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.OneInAMillion".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.PerhapsScyllaCould".Localized());
            else

                if (SceneInstance<Dominika>.Get().FellatrixSucked())
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.PerhapsDominikaCould".Localized());

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.GrabBlubberingDemoness".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.OneCockWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.YourVagina".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.SensitiveSkinGroin".Localized());

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.EagerlyGoesToWork".Localized());

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.RockingToPace".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.BlowWadDownSlut".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.CreamOnSlutFace".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.TeachBitchToPleasure".Localized());

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.QuietSlurpsHang".Localized(
                Player.Mf("grunts", "coos")
            ));

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.FlashOfGreen".Localized());

            OutputText("Scenes.Succubusgardenerscenes.TentaFail.DemonessStillFucking".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.OutlineClearlyPhallic".Localized());
            else
            {
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.TipOozesLubricants".Localized());

                if (Player.BiggestCockLength() < 36)
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.HowCouldYouHope".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.HowLongPreparing".Localized());
            }

            var createdVag = false;

            if (!Player.HasCock() && !Player.HasVagina())
            {
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.NewArrivalDescends".Localized());

                OutputText("Scenes.Succubusgardenerscenes.TentaFail.ThingTurningYou".Localized());

                Player.CreateVagina();

                createdVag = true;
            }
            else
                if (Player.HasVagina() && !Player.HasCock())
                {
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.NewArrivalDescendsYour".Localized());

                    Player.CuntChange(Player.VaginalCapacity() * 1.25f, true, true, false);
                }

            if (!Player.HasCock())
            {
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.FeelYour".Localized());

                if (createdVag)
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.NewWord".Localized());

                OutputText("Scenes.Succubusgardenerscenes.TentaFail.EntranceGivingAround".Localized());

                Player.CuntChange(Player.VaginalCapacity() * 1.25f, true, true, false);

                OutputText("Scenes.Succubusgardenerscenes.TentaFail.FeelBellyMoving".Localized());

                OutputText("Scenes.Succubusgardenerscenes.TentaFail.CumWhenThrustsBack".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), TentaFailII);
            }
            else
            {
                OutputText("Scenes.Succubusgardenerscenes.TentaFail.MonstrousPussyWastes".Localized());

                if (Player.BiggestCockLength() <= 12)
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.EaseWithWhichSlide".Localized());
                else
                    if (Player.BiggestCockLength() <= 30)
                        OutputText("Scenes.Succubusgardenerscenes.TentaFail.EaseWithWhichEnter".Localized(
                    Player.Cocks.Count > 1 ? "s are" : " is"
                ));
                    else
                        if (Player.BiggestCockLength() <= 48)
                            OutputText("Scenes.Succubusgardenerscenes.TentaFail.SlideInWithEase".Localized());
                        else
                            OutputText("Scenes.Succubusgardenerscenes.TentaFail.SlidingInTakesEffort".Localized());

                OutputText("Scenes.Succubusgardenerscenes.TentaFail.FeelsGoodBetter".Localized());

                if (Player.BiggestCockLength() <= 12)
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.ThanGiantLooks".Localized());
                else
                    if (Player.BiggestCockLength() <= 30)
                        OutputText("Scenes.Succubusgardenerscenes.TentaFail.ThanYouHadThought".Localized(
                    Player.Cocks.Count > 1 ? "es" : ""
                ));
                    else
                        if (Player.BiggestCockLength() > 48)
                            OutputText("Scenes.Succubusgardenerscenes.TentaFail.ThanYouWouldExpect".Localized());

                if (Player.BiggestCockLength() <= 30)
                {
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.GooLeaksFrom".Localized(
                    Player.Cocks.Count > 1 ? "s" : ""
                ));

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.StartsSlidingUp".Localized());

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.WhimperingFromEcstasy".Localized(
                    Player.Cocks.Count > 1 ? "s are" : " is"
                ));

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.CummingAlmostSurprise".Localized());

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.SensationsDont".Localized());

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.SecondsLaterTentacle".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), TentaFailII);

                    return ;
                }
                else
                {
                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.GooLeaksOut".Localized(
                    Player.Cocks.Count > 1 ? "s" : ""
                ));

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.InsideContracts".Localized());

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.DontGoSoft".Localized(
                    Player.Cocks.Count > 1 ? "s are" : " is"
                ));

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.SecondsLaterFind".Localized(
                    Player.Cocks.Count > 1 ? "s throb" : "throbs"
                ));

                    OutputText("Scenes.Succubusgardenerscenes.TentaFail.SometimeLaterComeTo".Localized());

                    AwardAchievement(AchievementIds.KaizoTrap, true, true, false);

                    GameOver();
                }
            }
        }

        public void TentaFailII()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.TentaFailII.HeldCaptiveOverwhelmed".Localized());

            AwardAchievement(AchievementIds.KaizoTrap, true, true, false);

            GameOver();
        }

        public void EndHer()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.EndHer.CircleBehindHer".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.D3GardenerDefeated, GARDENER_KILLED);

            ClearButtons();

            ResumeFromFight();
        }

        public void LeaveHer()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.LeaveHer.FiguringPets".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.D3GardenerDefeated, GARDENER_LEFT);

            ClearButtons();

            ResumeFromFight();
        }

        public void SurrenderToTheGardener(bool hpVictory = false)
        {

            if (Player.HasCock() && !Player.HasVagina())
                MaleLoss(hpVictory);

            if (!Player.HasCock() && !Player.HasVagina())
                FemGenderlessLoss(hpVictory);

            if (Player.HasVagina() && !Player.HasCock())
                FemGenderlessLoss(hpVictory);

            if (Player.HasVagina() && Player.HasCock())
                if (Random.Shared.Next(2) == 0)
                    FemGenderlessLoss(hpVictory);
                else
                    MaleLoss(hpVictory);
        }

        public void FemGenderlessLoss(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.DropToGroundDefeated".Localized());

            if (hpVictory)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.NotEvenAble".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.TooTurnedOn".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.HasTheOnceMighty".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.DestroyWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.ShutDownWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.FactoryWouldFall".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.HerGardenQuestion".Localized());

            if (!hpVictory)
            {
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.FurtiveGlanceTowards".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.UnderthingsSwampy".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.AlreadyHornyBut".Localized());
            }
            else
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.WorriedLookTowards".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.GentlyRemoving".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.AgainstBetterJudgment".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.SlimFingersPress".Localized());

            if (!Player.HasVagina())
            {
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.BareCrotchSinking".Localized());

                Player.CreateVagina();
            }
            else
            {
                if (Player.Wetness() <= 2)
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.MoisteningWord".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.SoakedWord".Localized());

                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.CreaseItsAll".Localized());
            }

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.IdlyToysWith".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.AcrossYourSack".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.AcrossUndersideCock".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.AgainstYourClit".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.ConfidingAloud".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.FingersDipDeeply".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.LastPartEscapes".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.WhimperingSqueezesPast".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.SmilingSadlyPulls".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.LedUpToThe".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLoss.SupportiveArmVanishes".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FemGenderlessLossII);
        }

        public void FemGenderlessLossII()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.FallDoesntTakeYou".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.GrowsDark".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.LiquidDrizzlesVagina".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.AndCocksWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.ForeignWetness".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.EasedOpenButWho".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.YourClitMustLook".Localized());

            if (Player.GetClitLength() > 6)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.GlisteningCumSoakedShaft".Localized());
            else
                if (Player.GetClitLength() > 3)
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.TinyCumSoakedCock".Localized());
                else
                    if (Player.GetClitLength() > 1)
                        OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.SwollenLubeLacquered".Localized());
                    else
                        OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.GlisteningPinkTarget".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.RollYourAs".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.BandsOfVerdantIron".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.TasteOnTongue".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.EruptsLongOverdue".Localized());

            Player.CuntChange(Player.VaginalCapacity() * 1.25f, true, true, false);

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.ChangeFromEmptiness".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.ClingsTightly".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.LongIgnoredRibbons".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.PluralS".Localized());

                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.OverSquishingSounds".Localized());
            }

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.CumSplattersExterior".Localized());

            if (Player.GetClitLength() > 6)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.StretchesAroundCock".Localized());
            else
                if (Player.GetClitLength() > 3)
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.NoTroubleHandling".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.TinyButtonDwarfed".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.DoesntMakeIt".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.FleshySpearRavaging".Localized());

            if (Player.BiggestBreastSize() > 2)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.AndJiggleYourTits".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.PressingFlatCervix".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.SplatterOccupiedWomb".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.FillWombToMake".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.GirlhoneySquirts".Localized());

            if (Player.Wetness() < 4)
                OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.MayNotNormally".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.FountainOfWetness".Localized());

            OutputText("Scenes.Succubusgardenerscenes.FemGenderlessLossII.LargerTentacleTakesPlace".Localized());

            GameOver();
        }

        public void MaleLoss(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.MaleLoss.DropToGroundDefeated".Localized());

            if (hpVictory)
                OutputText("Scenes.Succubusgardenerscenes.MaleLoss.NotEvenAble".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLoss.TooTurnedOn".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLoss.StridesOverToYou".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLoss.WhineUnwelcomeLust".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLoss.StopsLookingAtYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Pet".Localized(), MaleLossPet);

            AddButton(1, "Perks.Feeder.Name".Localized(), MaleLossFeeder);
        }

        public void MaleLossPet()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.GaspAccompanyingNeedy".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PressingDownOnceMore".Localized());

            if (Player.BiggestCockLength() >= 18)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ItsQuiteBig".Localized());
            else
                if (Player.BiggestCockLength() >= 8)
                {
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ThatsQuiteNice".Localized());

                    if (new[] { BodyPartTypes.Dog, BodyPartTypes.Wolf }.Contains(Player.Cocks[Player.BiggestCockIndex()].Id))
                        OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.AppropriatelyShaped".Localized());

                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.SentencePeriod".Localized());
                }
                else
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ThatsQuiteWell".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.SayEnd".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HighPitchedWhine".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.CoosTwistingFoot".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HissForgettingRole".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.StarsErupt".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.VisagePaintedDispleasure".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.OpenMouthAboutTo".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HeelRubsAt".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.JustAboveYourBalls".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.OnceMoreCooing".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.YoureNotSureYou".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PerhapsSensingThis".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.FirstFlowsSplatter".Localized());

            if (!Player.HasMuzzle())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HeedlessOfFact".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.SlobberingInVigor".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HerFootDisappears".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.LookDownAtDrooling".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PluralS".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.AtPuddleOfSap".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ThreesWord".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.YourHoovesWord".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.FoursWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.SentencePeriodAgain".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ScrabbleOverUnused".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.GallopOverAndDrop".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PlantYourRight".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
            {
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.YourTailWord".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox && Player.Body.Get<TailPart>().Venom > 1)
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PluralSAgain".Localized());
            }
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.YourStubbyNewTail".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.WaggingWord".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.Dog)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.NeverMindGetsLonger".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.NeverMindThatYour".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.TongueSwellingUp".Localized());

            Player.Body.Get<TailPart>().Id = BodyPartTypes.Dog;

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.BestWorstOfAll".Localized());

            if (Player.IsPregnant())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PregnantWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.BellyDrippingDrooling".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.LubricatedWithSheen".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.MoreReadyThanEver".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.WhyHandsGlowing".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.GlowFromFingertips".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.CoosAgainYouHear".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.PluralThrum".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ThrumsWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ReadyExcitementThinTrail".Localized());

            if (Player.BiggestCockLength() >= 12)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.BreakerWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.BonerWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HopeSheDoesntMind".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.TurnsAndWhistles".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.YayPlodAlong".Localized());

            if (Player.IsTaur() || Player.IsDrider())
            {
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.LongerYouFollowMore".Localized());

                if (Player.IsDrider())
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ClipClopHooves".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.SharpClacksClaws".Localized());

                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.FadingIntoPats".Localized());
            }
            else
            {
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.LongerYouFollowEasier".Localized());

                if (Player.IsGoo())
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.GooeyUndercarriage".Localized());
                else
                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.SlitheringTailTwitches".Localized());
                    else
                        OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ShortenAsYouPlod".Localized());

                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.HandsFollowSuit".Localized());

                OutputText("Scenes.Succubusgardenerscenes.MaleLossPet.ByTimeYouPass".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MaleLossPetII);
        }

        public void MaleLossPetII()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.KennelIsAmazing".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.AWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.YourWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.CockThroughHole".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.JustTooMuchPhysical".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.ClaspsCollarAround".Localized());

            if (!Player.HasFur())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.WasThatThereBefore".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.FeelsVeryRight".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.PluralAre".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.IsWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.BobbingBeneathYou".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.ListenedSoWellToday".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.OhhhhSoundsSoHappy".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.MistressAsks".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.BarkOtherHand".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.WhosAGoodPuppy".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.BarkComesQuicker".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.PluralS".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.TremblingSpurtingRibbons".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.WhosAGoodDoggie".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.BarkAndCum".Localized());

            if (Player.CumQ() >= 2500)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.ThickRopesShootPast".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.YipWithGlee".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetII.GardenerHoldsHand".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MaleLossPetIII);
        }

        public void MaleLossPetIII()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.MaleLossPetIII.ChampionMoreCanine".Localized());

            GameOver();
        }

        public void MaleLossFeeder()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.GaspOutAsMuch".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.LipsCurlCruelly".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.SlickAndSmoothWraps".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.OneCockPeriod".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.YourDickWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.FunnyYouThink".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.GreenHuedAppearance".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.TasteOhGods".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.StrokesFingertipsDown".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.YoureNotSureYou".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.RunningHerFingerDown".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.ItsWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.OnesWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.TwitchingLengthGiggles".Localized());

            if (Player.BiggestBreastSize() >= 3)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.UndersideOfBreasts".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.YourChinWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.WhimperWithNeed".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.IsWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.AreWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.FlexingIfHad".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.LookUpAtThe".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.BreezeCaresses".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.FamiliarTightness".Localized());
            else
            {
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.TightnessIsThere".Localized());

                if (Player.BallSize > 15)
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.TentaclesLoopUnder".Localized());
            }

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.HappilyGurgle".Localized());

            if (Player.BiggestBreastSize() >= 3)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.AllOverYourBody".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.OntoSoilBelow".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.IsntEnough".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.TentacleInMouthPulls".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.LaughsCuppingHand".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.PluralS".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeeder.LapsAtAlabasterTreat".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MaleLossFeederII);
        }

        public void MaleLossFeederII()
        {
            ClearOutput();

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.TrappedInHellishLoop".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.StalkThickAs".Localized());

            if (Player.BiggestCockLength() < 6)
            {
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.SwiftlyEngulfsMeager".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PluralS".Localized());
            }
            else
                if (Player.BiggestCockLength() < 12)
                {
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.RapidlyDevoursPlusSized".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PluralSAgain".Localized());
                }
                else
                    if (Player.BiggestCockLength() < 24)
                    {
                        OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.DevoursImmenseOffering".Localized());

                        if (Player.Cocks.Count > 1)
                            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PluralSThird".Localized());

                        OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.WithSteadyDetermination".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.SlowlyEncirclesLength".Localized());

                        if (Player.Cocks.Count > 1)
                            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PluralSFourth".Localized());

                        OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.WithInhumanElasticity".Localized());
                    }

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.InteriorCoveredTiny".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PluralSFifth".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.LikeEveryNerve".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.NewTentacleAlreadyCumming".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PastYourTail".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.BetweenYourCheeks".Localized());

            if (Player.Anal.Looseness == 0)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.VirginalTightness".Localized());
            else
                if (Player.Anal.Looseness <= 3)
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.LittleProblemSlipping".Localized());
                else
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.LittleDifficultySqueezing".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.GaspAtOddRoom".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.TimedPerfectly".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.ProstateWord".Localized());
            else
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.BallWord".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.DrainingSpurts".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.OnceAThickCock".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.SentencePeriod".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.VinesRubbingAgainst".Localized());
            else
                if (Player.IsLactating())
                    OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.PairOfSuckers".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.ThoughtsOfEscape".Localized());

            OutputText("Scenes.Succubusgardenerscenes.MaleLossFeederII.NeverWantToLeave".Localized());

            GameOver();
        }

    }
}