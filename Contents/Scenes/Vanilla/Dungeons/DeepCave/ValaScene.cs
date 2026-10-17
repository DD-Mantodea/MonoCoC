using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.DeepCave
{
    [Register<ValaScene>("coc:vala_scene")]
    public class ValaScene : Scene
    {
        public override void Entry() => FreeValazLooseCoochie();

        public void FreeValazLooseCoochie()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.FreeValazLooseCoochie.SearchRoomForKey".Localized());

            OutputText("Scenes.Valascene.FreeValazLooseCoochie.BendDownToComfort".Localized());

            ClearButtons();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney))
                || Player.HasItem(ItemRegistry.Create(UseableIds.PurePearl))

                || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurityPhilter)))

                AddButton(0, "Common.Button.FixHer".Localized(), HealVala);
            else
                AddButtonDisabled(0, "Fix Her", "You could try to fix her with some anti-corruption potion, but you have none.");

            if (!Player.IsGenderless())
                AddButton(1, "Common.Button.Sex".Localized(), ValaGetsSexed);
            else
                AddButtonDisabled(1, "Sex", "You must have genitals in order to have sex.");

            AddButton(3, "Common.Button.Reject".Localized(), RejectFuckingVala);

        }

        public void HealVala()
        {
            ClearOutput();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)))
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney));

                Flags.Set(GameFlagIds.ValaHealedHoney, 1);

                OutputText("Scenes.Valascene.HealVala.NotSureIfPure".Localized());

                OutputText(Player.HasTailInsteadOfLegs()

                    ? "Scenes.Valascene.HealVala.TailWord".Localized()

                    : Player.Leg());

                OutputText("Scenes.Valascene.HealVala.AsYouWalkEnd".Localized());

                OutputText("Scenes.Valascene.HealVala.EffectsOfYourCure".Localized());

                DoNext(HealValaPartTwoTheHealingHealsOfRevenge);

                return;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurityPhilter)))
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurityPhilter));

                OutputText("Scenes.Valascene.HealVala.HaveTheHunch".Localized());

                OutputText(Player.HasTailInsteadOfLegs()

                    ? "Scenes.Valascene.HealVala.TailWordAgain".Localized()

                    : Player.Leg());

                OutputText("Scenes.Valascene.HealVala.AsYouWalkAgain".Localized());

                OutputText("Scenes.Valascene.HealVala.EffectsOfCureAgain".Localized());

                DoNext(HealValaPartTwoTheHealingHealsOfRevenge);

                return;
            }

            Player.ConsumeItem(ItemRegistry.Create(UseableIds.PurePearl));

            OutputText("Scenes.Valascene.HealVala.SoftWarmBreeze".Localized());

            OutputText("Scenes.Valascene.HealVala.NotWhatYouHad".Localized());

            OutputText("Scenes.Valascene.HealVala.TriesStandFalls".Localized());

            OutputText("Scenes.Valascene.HealVala.LeavingTheWay".Localized());

            Flags.Set(GameFlagIds.FreedVala, 1);

            DoNext(PlayerMenu);
        }

        public void HealValaPartTwoTheHealingHealsOfRevenge()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.HealValaPartTwoTheHealingHealsOfRevenge.TriesStandFalls".Localized());

            OutputText("Scenes.Valascene.HealValaPartTwoTheHealingHealsOfRevenge.LeavingTheWay".Localized());

            Flags.Set(GameFlagIds.FreedVala, 1);

            DoNext(PlayerMenu);
        }

        public void ValaGetsSexed()
        {
            ClearOutput();

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Valascene.ValaGetsSexed.FaeriesGrindingAnd".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexed.PointsAtHerself".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexed.CommandSoftlySheFlinches".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexed.GlancesAroundForImminent".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexed.WhateverSmallParts".Localized());
            }
            else
            {
                OutputText("Scenes.Valascene.ValaGetsSexed.IncessantGropingIs".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexed.AskIfCanFly".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            DoNext(ValaGetsSexedPtDuece);
        }

        public void ValaGetsSexedPtDuece()
        {
            ClearOutput();

            Player.Orgasm("Generic");

            Player.ChangeFatigue(5);

            if (Player.CockTotal() > 0)
            {
                var x = Player.CockThatFits(60);

                if (x >= 0)
                {
                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.PussySurroundsYouLike".Localized());

                    OutputText(Player.HasVagina()

                        ? "Scenes.Valascene.ValaGetsSexedPtDuece.CuntWord".Localized()

                        : "Scenes.Valascene.ValaGetsSexedPtDuece.ProstateWord".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.IsReceivingAnd".Localized());

                    OutputText(Player.HasVagina()

                        ? "Scenes.Valascene.ValaGetsSexedPtDuece.SlitWord".Localized()

                        : "Scenes.Valascene.ValaGetsSexedPtDuece.RectumWord".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.FeelOrgasmBuilding".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.BathOfGirlCum".Localized());
                }
                else
                {
                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.SlidesOntoWithGleeful".Localized());

                    OutputText(Player.BiggestBreastSize() < 1

                        ? "Scenes.Valascene.ValaGetsSexedPtDuece.ChestWord".Localized()

                        : "Scenes.Valascene.ValaGetsSexedPtDuece.BreastsWord".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.TightAndGettingTighter".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.SpotUnsecuredSetOf".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.MoansInASeries".Localized());

                    OutputText(Player.HasVagina()

                        ? "Scenes.Valascene.ValaGetsSexedPtDuece.NuzzlingYourJoyBuzzer".Localized()

                        : "Scenes.Valascene.ValaGetsSexedPtDuece.TicklingYourProstate".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.ThrustMoreForcefully".Localized());

                    OutputText(Player.BiggestBreastSize() < 1

                        ? "Scenes.Valascene.ValaGetsSexedPtDuece.ChestWordAgain".Localized()

                        : "Scenes.Valascene.ValaGetsSexedPtDuece.BreastsWordAgain".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.PeriodParagraphBreak".Localized());

                    OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.UnconsciousByTheTime".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.CloseEyesAndRun".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.HopelessRaceHowever".Localized());

                OutputText("Scenes.Valascene.ValaGetsSexedPtDuece.BlinkGiveBump".Localized());
            }

            Flags.Set(GameFlagIds.TimesFuckedValaInDungeon, Flags.Get(GameFlagIds.TimesFuckedValaInDungeon) + 1);

            DoNext(PlayerMenu);
        }

        public void LeftValaAlone()
        {
            ClearOutput();

            if (Flags.Get(GameFlagIds.InvestigatedValaAfterZetazDefeated) == 0
                || Flags.Get(GameFlagIds.TimeSinceValaAttemptedRapePc) == 0)
            {
                OutputText("Scenes.Valascene.LeftValaAlone.DispatchedFoulMaster".Localized());

                OutputText("Scenes.Valascene.LeftValaAlone.AtTheVeryLeast".Localized());

                Flags.Set(GameFlagIds.InvestigatedValaAfterZetazDefeated, Flags.Get(GameFlagIds.InvestigatedValaAfterZetazDefeated) + 1);

                Flags.Set(GameFlagIds.TimeSinceValaAttemptedRapePc, 6);
            }
            else if (Flags.Get(GameFlagIds.TimeSinceValaAttemptedRapePc) > 0)
                OutputText("Scenes.Valascene.LeftValaAlone.ValaIsStillHere".Localized());
            else
                OutputText("Scenes.Valascene.LeftValaAlone.ValaStillHereAgain".Localized());

            OutputText("Scenes.Valascene.LeftValaAlone.WhatWouldYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FixHer".Localized(), TryToHealVala);

            if (!Player.IsGenderless())
            {
                AddButton(1, "Common.Button.Use".Localized(), UseValaOHYEAHSNAPINTOASLIMJIM);

                AddButton(2, "Common.Button.Wake".Localized(), WakeValaUpBeforeYouGoGo);
            }
            else
            {
                AddButtonDisabled(1, "Use", "This scene requires you to have genitals.");

                AddButtonDisabled(2, "Wake", "This scene requires you to have genitals.");
            }

            if (Player.Lust >= 33 && SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                AddButton(3, "Common.Button.Shouldravala".Localized(), () => SceneInstance<ShouldraFollowerScene>.Get().ShouldraMeetsCorruptVala());
            else
                AddButtonDisabled(3, "ShouldraVala", "This scene requires you to have Shouldra as a follower.");

            AddButton(14, "Common.Button.Leave".Localized(), PlayerMenu);
        }

        public void TryToHealVala()
        {
            ClearOutput();

            var hasHoney = Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney));

            var hasPearl = Player.HasItem(ItemRegistry.Create(UseableIds.PurePearl));

            var hasPhilter = Player.HasItem(ItemRegistry.Create(ConsumableIds.PurityPhilter));

            if (!hasHoney && !hasPearl && !hasPhilter)
            {
                OutputText("Scenes.Valascene.TryToHealVala.TryYourBest".Localized());

                DoNext(PlayerMenu);

                return;
            }

            if (hasHoney)
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney));

                OutputText("Scenes.Valascene.TryToHealVala.NotSureIfPure".Localized());

                OutputText("Scenes.Valascene.TryToHealVala.EffectsOfYourCure".Localized());

                DoNext(TryToHealValaWHoney2);

                return;
            }

            if (hasPhilter)
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurityPhilter));

                OutputText("Scenes.Valascene.TryToHealVala.HaveTheHunch".Localized());

                OutputText(Player.HasTailInsteadOfLegs()

                    ? "Scenes.Valascene.TryToHealVala.TailWord".Localized()

                    : Player.Leg());

                OutputText("Scenes.Valascene.TryToHealVala.AsWalkSoEnd".Localized());

                OutputText("Scenes.Valascene.TryToHealVala.EffectsOfCureAgain".Localized());

                DoNext(TryToHealValaWHoney2);

                return;
            }

            Player.ConsumeItem(ItemRegistry.Create(UseableIds.PurePearl));

            OutputText("Scenes.Valascene.TryToHealVala.SoftWarmBreeze".Localized());

            OutputText("Scenes.Valascene.TryToHealVala.NotWhatYouHad".Localized());

            OutputText("Scenes.Valascene.TryToHealVala.TriesStandFalls".Localized());

            OutputText("Scenes.Valascene.TryToHealVala.LeavingTheWay".Localized());

            Flags.Set(GameFlagIds.FreedVala, 1);

            DoNext(PlayerMenu);
        }

        public void UseValaOHYEAHSNAPINTOASLIMJIM()
        {
            ClearOutput();

            switch (Player.Gender)
            {
                case Gender.Male:
                    OutputText("Scenes.Valascene.UseValaOHYEAHSNAPINTOASLIMJIM.ShrugGirlHopeless".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHSNAPINTOASLIMJIM.StepAwayFromBrain".Localized());

                    break;

                case Gender.Female:
                    OutputText("Scenes.Valascene.UseValaOHYEAHSNAPINTOASLIMJIM.ShrugAgain".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHSNAPINTOASLIMJIM.HeadingBackToChained".Localized());

                    break;

                default:
                    OutputText("Scenes.Valascene.UseValaOHYEAHSNAPINTOASLIMJIM.ShrugThird".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHSNAPINTOASLIMJIM.HeadingBackAgain".Localized());

                    break;
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 80f));

            DoNext(UseValaOHYEAHKOOLAIDPTII);
        }

        public void UseValaOHYEAHKOOLAIDPTII()
        {
            ClearOutput();

            switch (Player.Gender)
            {
                case Gender.Male:
                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.BeforeSheGetsTighter".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.CannotKeepUpFrenzied".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.FinallySpentStepBack".Localized());

                    break;

                case Gender.Female:
                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.SquealsInDelight".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.QuicklyRecoverThrust".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.ClutchYourAndSqueeze".Localized());

                    break;

                default:
                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.SquealsInDelightAgain".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.QuicklyRecoverPrepare".Localized());

                    OutputText("Scenes.Valascene.UseValaOHYEAHKOOLAIDPTII.ClutchAndSqueezeAgain".Localized());

                    break;
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            DoNext(PlayerMenu);
        }

        public void WakeValaUpBeforeYouGoGo()
        {
            ClearOutput();

            switch (Player.Gender)
            {
                case Gender.Male:
                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.DontBotherSearching".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.FaeriesGrindingAnd".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.WhateverSmallParts".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.SlidesOntoCockhead".Localized());

                    break;

                case Gender.Female:
                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.DontBotherSearchingAgain".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.SelectWhatLooksLike".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.AskIfSheFeels".Localized());

                    break;

                default:
                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.DontBotherSearchingThird".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.ScoopingHerUp".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.FinallyReachingTheBowl".Localized());

                    OutputText("Scenes.Valascene.WakeValaUpBeforeYouGoGo.GettingHardFromThe".Localized());

                    break;
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 999f));

            DoNext(WakeMeUpBeforeValaGoGosPtII);
        }

        public void WakeMeUpBeforeValaGoGosPtII()
        {
            ClearOutput();

            switch (Player.Gender)
            {
                case Gender.Male:
                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.SpotUnsecuredSetOf".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.MoansInASeries".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.CumsThirdTimeUtterly".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.EnjoyingYourPulsing".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.UnconsciousByTheTime".Localized());

                    break;

                case Gender.Female:
                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.CloseEyesAndRun".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.HopelessRaceHowever".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.BlinkGiveBump".Localized());

                    break;

                default:
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.ItdBeShame".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.YourNeedsSomeLoving".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.SteelAgainstCock".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.CarefulRestraintTorn".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.TheInsideFaerieGirls".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.InsideValasDroolingPussy".Localized());

                    OutputText("Scenes.Valascene.WakeMeUpBeforeValaGoGosPtII.WhenOrgasmSubsides".Localized());

                    break;
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Flags.Set(GameFlagIds.TimesFuckedValaInDungeon, Flags.Get(GameFlagIds.TimesFuckedValaInDungeon) + 1);

            DoNext(PlayerMenu);
        }

        public void RejectFuckingVala()
        {
            ClearOutput();

            if (Flags.Get(GameFlagIds.TimesPcDefeatedVala) == 0)
            {
                OutputText("Scenes.Valascene.RejectFuckingVala.WeakInsistenceHas".Localized());

                OutputText("Scenes.Valascene.RejectFuckingVala.GathersHerselfAndRaises".Localized());
            }
            else
            {
                OutputText("Scenes.Valascene.RejectFuckingVala.WhatsWrongWithFaerie".Localized());

                if (Flags.Get(GameFlagIds.TimesPcDefeatedValaAndRaeped) > 0)
                    OutputText("Scenes.Valascene.RejectFuckingVala.SmirkInWonder".Localized());

                OutputText("Scenes.Valascene.RejectFuckingVala.ParagraphBreak".Localized());

                OutputText("Scenes.Valascene.RejectFuckingVala.StumblesUpAndFondles".Localized());
            }

            var vala = EnemyRegistry.Create(new Identifier("coc", "vala"));

            vala.OnWon = (hpVictory) => ValaFightVictoryFuck();

            CombatScene.Start(vala, PlayerMenu);
        }

        public void ValaFightVictoryFuck()
        {
            ClearOutput();

            switch (Player.Gender)
            {
                case Gender.Female:
                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.GirlsGotEnergy".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.NeedsNoAdditionalUrging".Localized());

                    if (Player.Body.Get<WingsPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Valascene.ValaFightVictoryFuck.DecideGiveHand".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.LikeHummingbirdOnCoffee".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.StillSuchAWillful".Localized());

                    break;

                case Gender.Male:
                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.GirlsGotEnergyAgain".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.NeedsNoUrgingAgain".Localized());

                    if (Player.Body.Get<WingsPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Valascene.ValaFightVictoryFuck.DecideToGiveAgain".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.HummingbirdOnCoffeeAgain".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.ShortCircuitAround".Localized());

                    break;

                default:
                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.GirlsGotEnergyThird".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.FinallyReachingTheBowl".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.UndersideAgainstTheBack".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuck.OwnCockGettingHard".Localized());

                    break;
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, 99f), new DynStat(GameStats.Corruption, 1f));

            DoNext(ValaFightVictoryFuckPtII);
        }

        public void ValaFightVictoryFuckPtII()
        {
            ClearOutput();

            switch (Player.Gender)
            {
                case Gender.Female:
                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.OneByOneCurl".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.YoullBeDamned".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.ReachingDownToGrab".Localized());

                    break;

                case Gender.Male:
                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.PickUpTheSpeed".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.OrgasmMomentLater".Localized());

                    break;

                default:
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.ItdBeShame".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.YourPussyNeedsLoving".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.SteelAgainstYourCock".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.CarefulRestraintTorn".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.YourInsideFaerieGirls".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.InsideFaeriesDroolingPussy".Localized());

                    OutputText("Scenes.Valascene.ValaFightVictoryFuckPtII.WhenOrgasmSubsides".Localized());

                    break;
            }

            Player.Orgasm("Generic");

            Flags.Set(GameFlagIds.TimesFuckedValaInDungeon, Flags.Get(GameFlagIds.TimesFuckedValaInDungeon) + 1);

            Flags.Set(GameFlagIds.TimesPcDefeatedValaAndRaeped, Flags.Get(GameFlagIds.TimesPcDefeatedValaAndRaeped) + 1);

            DoNext(PlayerMenu);
        }

        public bool PurifiedFaerieBitchBar()
        {
            if (Flags.Get(GameFlagIds.FreedVala) == 0)
                return false;

            if (View.Time.Hours < 12 || View.Time.Hours > 21)
                return false;

            if (Flags.Get(GameFlagIds.FuckedValaAtBarToday) > 0)
            {
                OutputText("Scenes.Valascene.PurifiedFaerieBitchBar.FlutteringFaerieDrops".Localized());

                return false;
            }

            OutputText("Scenes.Valascene.PurifiedFaerieBitchBar.ShortFigureFlutters".Localized());

            if (Player.Gender > 0)
            {
                OutputText("Scenes.Valascene.PurifiedFaerieBitchBar.CouldSpeakWithHer".Localized());

                return true;
            }

            OutputText("Scenes.Valascene.PurifiedFaerieBitchBar.SpeakingMightBeBetter".Localized());

            return false;
        }

        public void CleansedFirstRemeet()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.CleansedFirstRemeet.FinishesDeliveringDrinks".Localized());

            if (Player.IsTaur() || Player.IsDrider() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Valascene.CleansedFirstRemeet.EyeTheStoolDoubtfully".Localized());

            OutputText("Scenes.Valascene.CleansedFirstRemeet.ExplainsHowAfterComing".Localized());

            OutputText("Scenes.Valascene.CleansedFirstRemeet.InTurnSheListens".Localized());

            OutputText("Scenes.Valascene.CleansedFirstRemeet.WhispersFlyingUp".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.You".Localized(), CleansedValaFuckHerBrainsOut);

            AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<TelAdre>.Get().BarTelAdre());
        }

        public void CleansedValaRepeatBrainFucking()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.CleansedValaRepeatBrainFucking.FlashesBeautifulSmile".Localized());

            DoNext(CleansedValaFuckHerBrainsOut);
        }

        public void CleansedValaFuckHerBrainsOut()
        {
            ClearOutput();

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.DontEvenHaveTo".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.StrainingBodyHardAgainst".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.GrinsTakingYour".Localized());

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.TakesADeepBreath".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.TryToHoldBack".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.CoosIntoYourEar".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.SLetter".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.FromBaseToTip".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.SLetterAgain".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.StrokedAndSuckled".Localized());
            }
            else
            {
                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.RunHandsOverVala".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.StillInYourLap".Localized());

                if (Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.PressureEnoughToDraw".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.ParagraphBreak".Localized());

                if (Player.Tallness <= 59)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.CatchingHerBreath".Localized());

                    if (Flags.Get(GameFlagIds.ValaHealedHoney) == 1)
                        OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.TakeHerLegs".Localized());

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.LongTongueRemembers".Localized());
                }
                else if (Player.Tallness < 74)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.CatchingBreathAgain".Localized());

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.SlippingFewFingers".Localized());
                }
                else
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.CatchingBreathThird".Localized());

                    if (Player.BiggestLactation() < 1)
                    {
                        OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.SurpriseMereTaste".Localized());

                        Player.BoostLactation(3);
                    }

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOut.ValasLipsPerhapsTrained".Localized());
                }
            }

            DoNext(CleansedValaFuckHerBrainsOutPtII);

            Player.Orgasm("Generic");
        }

        public void CleansedValaFuckHerBrainsOutPtII()
        {
            ClearOutput();

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.LeansBackGrabs".Localized());

                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.SLetter".Localized());

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.TweakNipplesHarder".Localized());

                if (Player.Tallness <= 59)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.IfBlissfulExpression".Localized());

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.HelplessInTheOrgasming".Localized());
                }
                else if (Player.Tallness < 74)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.IfBlissfulExpressionAgain".Localized());

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.DragOrgasmingFaerie".Localized());
                }
                else
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.IfBlissfulExpressionThird".Localized());

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.LimbsEntwinedYouLet".Localized());

                    OutputText(Player.BiggestBreastSize() < 1

                        ? "Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.ChestWord".Localized()

                        : "Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.BosomWord".Localized());

                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.SeedSoothesTheFires".Localized());
                }

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.IrrepressibleEnergyRestored".Localized());

                if (Player.Tallness >= 120 && Player.Tallness < 132 && Random.Shared.Next((int)(Player.Tallness - 119)) > 0)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.OrWasThatImagination".Localized());

                    Player.Tallness--;
                }

                if (Player.Tallness >= 132)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.ImaginationAgain".Localized());

                    Player.Tallness--;
                }

                Player.Tallness++;

                if (Player.Corruption > 40)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, -0.3f));
            }
            else
            {
                if (!Player.HasVagina())
                {
                    DoNext(PlayerMenu);

                    return;
                }

                OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.EnergyRestoredAgain".Localized());

                if (Player.Tallness >= 120 && Player.Tallness < 132 && Random.Shared.Next((int)(Player.Tallness - 119)) == 0)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.ImaginationThird".Localized());

                    Player.Tallness--;
                }

                if (Player.Tallness >= 132)
                {
                    OutputText("Scenes.Valascene.CleansedValaFuckHerBrainsOutPtII.ImaginationFourth".Localized());

                    Player.Tallness--;
                }

                Player.Tallness++;

                if (Player.Corruption > 40)
                    Player.ApplyStats(new DynStat(GameStats.Corruption, -0.5f));
            }

            View.Time.PassTime(1);

            DoNext(() => SceneInstance<TelAdre>.Get().BarTelAdre());
        }

        private static bool ScyllaTherapyUnlocked =>

            CoC.Instance.Flags.Get(GameFlagIds.TimesScyllaAddictGroupExploited)

            + CoC.Instance.Flags.Get(GameFlagIds.ScyllaTimesSharedInAddictGroup)

            + CoC.Instance.Flags.Get(GameFlagIds.ScyllaMilkTherapyTimes)

            + CoC.Instance.Flags.Get(GameFlagIds.ScyllaCumTherapyTimes)

            + CoC.Instance.Flags.Get(GameFlagIds.ScyllaSexTherapyTimes) > 0;

        public void FaerieOrgyFuck()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.FaerieOrgyFuck.GigglesAndNods".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuck.ThankValaForIntroducing".Localized());

            Flags.Set(GameFlagIds.WeeklyFairyOrgyCountdown, 6);

            if (Player.IsHerm())
                OutputText("Scenes.Valascene.FaerieOrgyFuck.FoldsHerArms".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), FaerieOrgyFuckMaleContinue);
            else
                AddButtonDisabled(0, "Male", "This scene requires you to have a cock.");

            if (Player.HasVagina())
                AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), FaerieOrgyFuckFemaleContinue);
            else
                AddButtonDisabled(1, "Female", "This scene requires you to have a vagina.");
        }

        public void FaerieOrgyFuckMaleContinue()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.FaerieGirlLands".Localized());

            if (ScyllaTherapyUnlocked)
                OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.AnotherFaerieFlies".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.OneByOneEach".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.PixieGirlGivesShaft".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.TongueCaressesSlurping".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.NearlyForgetToBreathe".Localized());

            if (Player.CumQ() > 2000)
                OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.EvenAfterDrivingCondom".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.ParagraphBreak".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckMaleContinue.AsksHoldingOrganicDevice".Localized());

            View.Time.PassTime(1);

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void FaerieOrgyFuckFemaleContinue()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.FaerieOrgyFuckFemaleContinue.FaerieGirlLands".Localized());

            if (ScyllaTherapyUnlocked)
                OutputText("Scenes.Valascene.FaerieOrgyFuckFemaleContinue.AnotherFaerieFlies".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckFemaleContinue.OneByOneEach".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckFemaleContinue.FinallyCalmDown".Localized());

            OutputText("Scenes.Valascene.FaerieOrgyFuckFemaleContinue.AsksHoldingOrganicDevice".Localized());

            View.Time.PassTime(1);

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void ChooseValaInBar()
        {
            ClearOutput();

            ClearButtons();

            AddButtonDisabled(0, "Faeries", "This option is only available once per week. It requires you to have genitals.");

            if (Flags.Get(GameFlagIds.EncounteredValaAtBar) == 0)
            {
                Flags.Set(GameFlagIds.EncounteredValaAtBar, Flags.Get(GameFlagIds.EncounteredValaAtBar) + 1);

                OutputText("Scenes.Valascene.ChooseValaInBar.SeatAndFlag".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), CleansedFirstRemeet);

                return;
            }

            if (Flags.Get(GameFlagIds.ShouldraMetVala) == 1)
            {
                Flags.Set(GameFlagIds.ShouldraMetVala, 2);

                OutputText("Scenes.Valascene.ChooseValaInBar.CallForVala".Localized());

                OutputText("Scenes.Valascene.ChooseValaInBar.HumanSizedFaerieReleases".Localized());

                OutputText("Scenes.Valascene.ChooseValaInBar.PullsBackAndPunches".Localized());

                OutputText("Scenes.Valascene.ChooseValaInBar.SurpriseMustBeShowing".Localized());

                OutputText("Scenes.Valascene.ChooseValaInBar.NoddingDumbly".Localized());

                OutputText("Scenes.Valascene.ChooseValaInBar.WhatWantToday".Localized());
            }
            else if (View.Time.Hours == 20 && Random.Shared.Next(2) == 0 && Player.HasCock())
            {
                KinathisValaStuff();

                AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<TelAdre>.Get().BarTelAdre());

                return;
            }
            else
            {
                OutputText("Scenes.Valascene.ChooseValaInBar.TakeASeatAgain".Localized());

                if (Flags.Get(GameFlagIds.WeeklyFairyOrgyCountdown) == 0 && !Player.IsGenderless())
                {
                    OutputText("Scenes.Valascene.ChooseValaInBar.ThoughtOccursHer".Localized());

                    AddButton(0, "Common.Button.Faeries".Localized(), FaerieOrgyFuck);
                }
            }

            AddButton(1, "Common.Button.You".Localized(), CleansedValaRepeatBrainFucking);

            if (Player.HasCock())

                AddButton(2, "Common.Button.CumBath".Localized(), ValaCumBath);
            else
                AddButtonDisabled(2, "Cum Bath", "This scene requires you to have a cock.");

            if (Flags.Get(GameFlagIds.ShouldraMetVala) > 0
                && SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())

                AddButton(3, "Common.Button.BigYou".Localized(), ValaBigYou);

            AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<TelAdre>.Get().BarTelAdre());
        }

        public void ValaCumBath()
        {
            ClearOutput();

            if (Flags.Get(GameFlagIds.ValaCumbathTimes) == 0)
            {
                OutputText("Scenes.Valascene.ValaCumBath.ChatWithVala".Localized());

                OutputText("Scenes.Valascene.ValaCumBath.MullingYour".Localized());

                OutputText("Scenes.Valascene.ValaCumBath.FollowAfterFinding".Localized());

                if (Player.CumQ() < 2000)
                {
                    OutputText("Scenes.Valascene.ValaCumBath.OffersBrightly".Localized());

                    OutputText("Scenes.Valascene.ValaCumBath.TiltingHeadBackSwallow".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Valascene.ValaCumBath.BallsReactImmediately".Localized());

                    OutputText("Scenes.Valascene.ValaCumBath.AbdomenNoisilyGroans".Localized());
                }
                else
                {
                    OutputText("Scenes.Valascene.ValaCumBath.CupThePale".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Valascene.ValaCumBath.FaeriesHandsSlide".Localized());

                    OutputText("Scenes.Valascene.ValaCumBath.StrippingFromYourBody".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Valascene.ValaCumBath.GiveYourFavoriteFaeries".Localized());

                OutputText("Scenes.Valascene.ValaCumBath.DropsOffHerApron".Localized());

                if (Player.CumQ() < 2000)
                    OutputText("Scenes.Valascene.ValaCumBath.RetrievingVialOfOil".Localized());

                OutputText("Scenes.Valascene.ValaCumBath.BrushesGlitteringHair".Localized());
            }

            DoNext(ValaCumBath2);
        }

        public void ValaCumBath2()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.ValaCumBath2.TriesToComposeHerself".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.CumSpurtsLong".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.RaisesHandsToGather".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Valascene.ValaCumBath2.GutIsHeavyWith".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.CumDrenchedGirlRuns".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.ChokesAsYourJetting".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Valascene.ValaCumBath2.HeadSinksRest".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.FaceDrippingWithMilky".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.BlissfulEcstasyFillsCup".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.SightOfValaSubdued".Localized());

            OutputText("Scenes.Valascene.ValaCumBath2.KeepStrokingYour".Localized());

            if (Player.CumQ() < 2000)
            {
                Player.CumMultiplier += 6;

                if (Player.Balls > 0)
                    Player.BallSize += 0.5f;
            }

            Player.CumMultiplier += 2;

            if (Player.Balls > 0)
                Player.BallSize += 0.3f;

            Flags.Set(GameFlagIds.ValaCumbathTimes, Flags.Get(GameFlagIds.ValaCumbathTimes) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1.5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ValaBigYou()
        {
            ClearOutput();

            Flags.Set(GameFlagIds.TimesValaConsensualBig, Flags.Get(GameFlagIds.TimesValaConsensualBig) + 1);

            if (Flags.Get(GameFlagIds.TimesValaConsensualBig) == 1)
            {
                OutputText("Scenes.Valascene.ValaBigYou.TiltingHeadEyesGlowing".Localized());

                OutputText("Scenes.Valascene.ValaBigYou.ShouldraChucklesInside".Localized());

                OutputText("Scenes.Valascene.ValaBigYou.ClapsHerHandsTogether".Localized());

                OutputText("Scenes.Valascene.ValaBigYou.OtherGirlMary".Localized());

                OutputText("Scenes.Valascene.ValaBigYou.SmilingLeanBack".Localized());
            }
            else
            {
                OutputText("Scenes.Valascene.ValaBigYou.TellValaHowHuge".Localized());

                OutputText("Scenes.Valascene.ValaBigYou.InAnyEventTwo".Localized());

                if (OptionStore.Instance.Get("silly", 0) > 0)
                    OutputText("Scenes.Valascene.ValaBigYou.LastThingNeed".Localized());

                OutputText("Scenes.Valascene.ValaBigYou.KnowsTheWay".Localized());
            }

            OutputText("Scenes.Valascene.ValaBigYou.GhostlyAnswerFading".Localized());

            OutputText("Scenes.Valascene.ValaBigYou.EyesGlowAmber".Localized());

            OutputText("Scenes.Valascene.ValaBigYou.SpellCompleteShouldraSteps".Localized());

            OutputText("Scenes.Valascene.ValaBigYou.EnhancedFaerieGiggles".Localized());

            OutputText("Scenes.Valascene.ValaBigYou.ReachesDownBetweenLegs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DomMe".Localized(), BigValaDomsPC);

            if (Player.HasCock())
                AddButton(1, "Common.Button.LickMe".Localized(), BigValaLicksOffDudes);
            else
                AddButtonDisabled(1, "Lick Me", "This scene requires you to have a cock.");

            if (FollowerRegistry.HasFollower(FollowerIds.VapulaSlave)
                && (Player.HasCock() || (Player.HasVagina() && Player.HasKeyItem("Demonic Strap-On"))))
            {
                AddButton(2, "Common.Button.DomVapula".Localized(), ValaDommyVapula3Some);

                AddButton(3, "Common.Button.Vapula3some".Localized(), ValaLoveyVapula3Some);
            }
            else
            {
                AddButtonDisabled(2, "Dom Vapula", "This scene requires you to have a cock or vagina and demonic strap-on.");

                AddButtonDisabled(3, "Vapula3Some", "This scene requires you to have a cock or vagina and demonic strap-on.");
            }

                AddButton(3, "Common.Button.Vapula3some".Localized(), ValaLoveyVapula3Some);

            AddButton(14, "Common.Button.Leave".Localized(), () => SceneInstance<TelAdre>.Get().BarTelAdre());
        }

        public void BigValaDomsPC()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.BigValaDomsPC.";

            OutputText((K + "TiltsHeadSide").Localized());

            OutputText((K + "GentlyPlacedOnThe").Localized());

            OutputText((K + "GigglesPushingYour").Localized());

            OutputText((K + "BoomingCommandHerVoice").Localized());

            if (Flags.Get(GameFlagIds.ValaHealedHoney) == 0)
                OutputText((K + "SheGiggles").Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText((K + "ShudderingOnslaughtFemale").Localized());

                if (Player.LustPercent >= 70)
                    OutputText((K + "AlreadyHardBead").Localized());
                else
                    OutputText((K + "StiffensToFullThrobbing").Localized());

                OutputText((K + "BodyImminentlyReady").Localized());

                if (Player.BiggestCockArea() >= 150)
                    OutputText((K + "IfYouWerentSo").Localized());
            }
            else if (Player.Gender == Gender.Female)
            {
                OutputText((K + "ShudderingOnslaughtLusty").Localized());

                if (Player.Wetness() >= 3)
                    OutputText((K + "YourSeemsAlwaysWet").Localized());

                OutputText((K + "ArmorTightAndConstricting").Localized());
            }
            else if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText((K + "ShudderingOnslaughtAgain").Localized());

                if (Player.LustPercent >= 70)
                    OutputText((K + "MayHaveBeenUncomfortably").Localized());
                else
                    OutputText((K + "GetHardAlarmingRapidity").Localized());

                OutputText((K + "InsideUndergarmentsEverything").Localized());

                if (Player.CockTotal() > 1)
                    OutputText((K + "SLetter").Localized());

                OutputText((K + "AreSmotheredWithStuff").Localized());

                if (Player.BiggestCockArea() >= 150)
                    OutputText((K + "YouWerentSoAgain").Localized());
            }
            else
                OutputText((K + "ShudderingOnslaughtThird").Localized());

            OutputText((K + "GrabHoldOfFleshy").Localized());

            OutputText((K + "MoansWhileShePresses").Localized());

            OutputText((K + "HeadBumpsIntoHot").Localized());

            OutputText((K + "GaspsYouPullUp").Localized());

            OutputText((K + "EarSplittingShriek").Localized());

            OutputText((K + "HandsClenchRod").Localized());

            OutputText((K + "ShouldraPopsOut").Localized());

            OutputText((K + "StandThereDrippingSexual").Localized());

            OutputText((K + "LiftHerOut").Localized());

            Player.SlimeFeed();

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void BigValaLicksOffDudes()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.BigValaLicksOffDudes.";

            OutputText((K + "GiantValaCocksEyebrow").Localized());

            OutputText((K + "ComfortinglyStrokeHerHand").Localized());

            OutputText((K + "SomewhatIntriguedByThe").Localized());

            OutputText((K + "WhinesAsTonguePokes").Localized());

            OutputText((K + "MountainousPixieLifts").Localized());

            OutputText((K + "FootHitsLip").Localized());

            if (Player.HasFuckableNipples())
                OutputText((K + "EvenPausesAtOne").Localized());

            OutputText((K + "FinallyEncirclesBack").Localized());

            OutputText((K + "WhimpersAroundYou").Localized());

            OutputText(Player.CockTotal() > 1 ? (K + "EachWord").Localized() : (K + "YourWord").Localized());

            OutputText((K + "LengthUnholySmoothness").Localized());

            if (Player.CockTotal() > 1)
                OutputText((K + "SLetter").Localized());

            OutputText((K + "UntilWord").Localized());

            OutputText(Player.CockTotal() == 1 ? (K + "ItSlides").Localized() : (K + "TheySlide").Localized());

            OutputText((K + "InsideHer").Localized());

            if (Player.BiggestCockArea() >= 200)
                OutputText((K + "ImmenseLengthSuckedIn").Localized());

            OutputText((K + "SentencePeriod").Localized());

            OutputText((K + "HappySlutPullsYou").Localized());

            OutputText(Player.BiggestCockArea() < 200

                ? (K + "SmotheringTinyPecker").Localized()

                : (K + "WrappingImmensePecker").Localized());

            if (Player.BiggestCockArea() >= 400)
                OutputText((K + "EvenAsItHangs").Localized());

            OutputText((K + "BeginMoanAsWell").Localized());

            OutputText((K + "TongueSurroundingBegins").Localized());

            OutputText((K + "SuddenlyUnwoundAndLeft").Localized());

            if (Player.BiggestCockArea() >= 400)
                OutputText((K + "CockPartwayDownThroat").Localized());

            OutputText((K + "StaringDownBetweenCanyon").Localized());

            OutputText((K + "SpitFrothsAndBubbles").Localized());

            if (Player.Balls > 0)
                OutputText((K + "ClenchingAgainstTongue").Localized());

            OutputText((K + "ExplodeIntoEcstatic").Localized());

            if (Player.CumQ() < 250)
                OutputText((K + "BarelyNoticesTheSpunk").Localized());
            else if (Player.CumQ() < 1000)
                OutputText((K + "HasToNoisilyGulp").Localized());
            else if (Player.CumQ() < 4000)
                OutputText((K + "SlowlyFloodHerMouth").Localized());
            else
                OutputText((K + "ImmediatelyFloodBulging").Localized());

            OutputText((K + "TongueGrindsBackForth").Localized());

            OutputText((K + "OnlyAfterOrgasmFaded").Localized());

            OutputText((K + "StandThereDrippingOral").Localized());

            OutputText((K + "LiftHerOut").Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void ValaVapulaThreesome()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.ValaVapulaThreesome.";

            if (Flags.Get(GameFlagIds.TimesVapulaAndGiantVala) == 1)
            {
                OutputText((K + "CrossingArmsCallOut").Localized());

                OutputText((K + "CackleGleefully").Localized());

                OutputText((K + "MostCharmingSmile").Localized());

                OutputText((K + "NodsEagerlyThough").Localized());

                OutputText((K + "YouReply").Localized());

                OutputText((K + "UnderstandablyTakenAback").Localized());
            }
            else
            {
                OutputText((K + "CallForVapula").Localized());

                OutputText((K + "AsksWithEnthusiasm").Localized());

                OutputText((K + "AnswerForcingHer").Localized());
            }
        }

        public void ValaDommyVapula3Some()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.ValaDommyVapula3Some.";

            OutputText((K + "MindMadeUp").Localized());

            OutputText((K + "SmallerWomanCries").Localized());

            OutputText((K + "LustilyRepliesTitters").Localized());

            OutputText((K + "VapulaCriesHelp").Localized());

            OutputText((K + "DontEvenDeignTo").Localized());

            OutputText((K + "SwingsAroundLikeOut").Localized());

            OutputText((K + "PokesPurpleWomans").Localized());

            OutputText((K + "TeasesItsARather").Localized());

            OutputText((K + "EroticSightBeckons").Localized());

            OutputText(!Player.HasCock()

                ? (K + "BucklingOnStrapOn").Localized()

                : (K + "StrokingEagerly").Localized());

            OutputText((K + "EyesLockedOnDemoness").Localized());

            OutputText((K + "MoansAtHerMound").Localized());

            OutputText((K + "ClimbOnIn").Localized());

            OutputText(!Player.HasCock()

                ? (K + "DildoWord").Localized()

                : (K + "CockSmallest").Localized());

            OutputText((K + "SlidesIntoDemoness").Localized());

            OutputText((K + "MeanwhileFuriousCunt").Localized());

            OutputText((K + "AbruptlySlapped").Localized());

            OutputText(Player.HasCock()

                ? (K + "SeedWord").Localized()

                : (K + "FakeSeedWord").Localized());

            OutputText((K + "WithoutMeaningToLustiness").Localized());

            if (Player.CumQ() > 500)
                OutputText((K + "PurpleBellyBalloons").Localized());

            if (Player.CumQ() >= 1500)
                OutputText((K + "MidriffTurnsTaut").Localized());

            if (Player.CumQ() > 500)
                OutputText((K + "SentencePeriod").Localized());

            if (Player.CumQ() > 1000)
                OutputText((K + "WavesOfSpunkPour").Localized());

            if (Player.LactationQ() >= 100)
            {
                OutputText((K + "EvenBetterYourLets").Localized());

                if (Player.LactationQ() <= 200)
                    OutputText((K + "ThickWord").Localized());
                else if (Player.LactationQ() <= 2000)
                    OutputText((K + "PowerfulWord").Localized());
                else
                    OutputText((K + "EnormousWord").Localized());

                OutputText((K + "StreamsTintingWhite").Localized());
            }

            OutputText((K + "ClimaxesPullingYouBoth").Localized());

            OutputText((K + "EventuallyDroppedToThe").Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void ValaLoveyVapula3Some()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.ValaLoveyVapula3Some.";

            OutputText((K + "SmilesTenderlyAndSays").Localized());

            OutputText((K + "AffectionatelyLicksFaeries").Localized());

            OutputText((K + "PlayfulFaerieSmilesDown").Localized());

            if (!Player.HasCock())
                OutputText((K + "BuckleOnStrapOn").Localized());

            OutputText((K + "ArmsOutEitherSide").Localized());

            OutputText((K + "LiftedUpHighWhich").Localized());

            OutputText(!Player.HasCock()

                ? (K + "TheDildo").Localized()

                : (K + "OneCock").Localized());

            OutputText((K + "EvenHarderIntoFaerie").Localized());

            OutputText((K + "OnlyBecauseValasGotten").Localized());

            OutputText((K + "AtFirstCrestfallen").Localized());

            OutputText((K + "IncredibleControlTheFaerie").Localized());

            OutputText((K + "LickHerNeckSucking").Localized());

            OutputText((K + "TremendousVibrationRipples").Localized());

            OutputText(!Player.HasCock()

                ? (K + "ThatExoticDildo").Localized()

                : (K + "YourCockSmallest").Localized());

            OutputText((K + "GrabHerAss").Localized());

            OutputText((K + "PurrsComeQuicker").Localized());

            OutputText((K + "MoansIntoYou").Localized());

            OutputText((K + "MinutesLaterCresting").Localized());

            OutputText(!Player.HasCock()

                ? (K + "SpurtingDildo").Localized()

                : (K + "CockSmallest").Localized());

            OutputText((K + "EnsuringEverySingleDrop").Localized());

            OutputText((K + "ArenSureHowLong").Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void ValaPartIIWaifuLove()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.ValaPartIIWaifuLove.";

            OutputText((K + "LettingOutAYawn").Localized());

            OutputText((K + "CantHelpButGrin").Localized());

            OutputText((K + "WonderfulChangeOf").Localized());

            OutputText((K + "WithHerSoAffectionate").Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void KinathisValaStuff()
        {
            ClearOutput();

            const string K = "Scenes.Valascene.KinathisValaStuff.";

            var x = Player.CockThatFits(60);

            if (x < 0)
                return;

            OutputText((K + "SoLateAtNight").Localized());

            OutputText((K + "QuipsBeforeGiving").Localized());

            OutputText((K + "LooksIntoYourEyes").Localized());

            OutputText((K + "AtTheAffirmation").Localized());

            OutputText((K + "ForNearlyAnHour").Localized());

            OutputText((K + "SmallWomanSteps").Localized());

            if (Player.Tallness > 75)
                OutputText((K + "LooksUpAtYou").Localized());
            else
                OutputText((K + "ALetter").Localized());

            OutputText((K + "FaintBlushOnCheeks").Localized());

            OutputText((K + "ObviousWhereTheSweet").Localized());

            OutputText((K + "LooksUpBefore").Localized());

            if (Player.Tallness < 65)
                OutputText((K + "FeelingOfPinningHer").Localized());
            else
                OutputText((K + "FeelingOfPinningAgain").Localized());

            OutputText((K + "CantHelpButShiver").Localized());

            OutputText((K + "NotWantingEnd").Localized());

            OutputText(Player.CockTotal() > 1

                ? (K + "MostAppealingPhallus").Localized()

                : Player.CockDescript(x));

            OutputText((K + "StartsToStrokeIt").Localized());

            if (Player.CockArea(x) < 17)
                OutputText((K + "PlantingAKiss").Localized());
            else if (Player.CockArea(x) < 30)
                OutputText((K + "PlantingAKissAgain").Localized());
            else
                OutputText((K + "PlantingAKissThird").Localized());

            OutputText((K + "DiminutiveTongueSucks").Localized(Player.CockDescript(x)));

            if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                OutputText((K + "TongueLicksThrobbing").Localized());
            else if (Player.HasKnot(x))
                OutputText((K + "TongueLicksTheHot").Localized());
            else if (Player.Cocks[x].Id == BodyPartTypes.Anemone)
                OutputText((K + "TonguePlaysWithSquirming").Localized());

            OutputText((K + "PlayingHerWayDown").Localized());

            OutputText((K + "SweetLipsKiss").Localized());

            OutputText((K + "WithSuchAnInitiation").Localized());

            OutputText((K + "NotWantingToWait").Localized());

            if (Player.CockArea(x) < 17)
                OutputText((K + "DespiteHerSize").Localized());
            else if (Player.CockArea(x) < 30)
                OutputText((K + "CroonsToYouBeckoning").Localized());
            else
                OutputText((K + "MoansAndGroansAs").Localized());

            OutputText((K + "DeeperAndDeeperDelve").Localized());

            if (Player.CockArea(x) > 17)
                OutputText((K + "HotLittleBodyStretches").Localized());

            OutputText((K + "HandsRoamOverHer").Localized());

            OutputText((K + "PassionateCriesFocus").Localized());

            if (Player.Balls > 0)
                if (Player.BallSize < 3)
                    OutputText((K + "EachHardQuakingThrust").Localized());
                else if (Player.BallSize < 6)
                    OutputText((K + "QuakingThrustAgain").Localized());
                else
                    OutputText((K + "QuakingThrustThird").Localized());

            OutputText((K + "HotterAndHotter").Localized());

            if (Player.CumQ() < 250)
                OutputText((K + "GaspingInPleasure").Localized());
            else if (Player.CumQ() < 1000)
                OutputText((K + "GruntingHardYouGush").Localized());
            else if (Player.CumQ() < 3000)
                OutputText((K + "GaspingAndGroaning").Localized());
            else
                OutputText((K + "EyesRollMouth").Localized());

            OutputText((K + "SweetReliefOfOrgasm").Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ValaPartIIWaifuLove);
        }

        public void TryToHealValaWHoney2()
        {
            ClearOutput();

            OutputText("Scenes.Valascene.TryToHealValaWHoney2.TriesStandFalls".Localized());

            OutputText("Scenes.Valascene.TryToHealValaWHoney2.LeavingTheWay".Localized());

            Flags.Set(GameFlagIds.FreedVala, 1);

            DoNext(PlayerMenu);
        }
    }
}