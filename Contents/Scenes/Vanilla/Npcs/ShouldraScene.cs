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
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<ShouldraScene>("coc:shouldra")]
    public class ShouldraScene : Scene, ITimeAware
    {

        public override void Entry()
        {

        }

        public bool TimeChange()
        {
            var flags = CoC.Instance.Flags;

            var follower = SceneInstance<ShouldraFollowerScene>.Get();

            var needNext = false;

            if (flags.Get(GameFlagIds.ShouldraMagicCooldown) >= 1)
                flags.Set(GameFlagIds.ShouldraMagicCooldown, flags.Get(GameFlagIds.ShouldraMagicCooldown) - 1);

            if (follower.FollowerShouldra())
            {
                if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && Player.HasCock()
                    && Random.Shared.Next(10) == 0)

                    if (flags.Get(GameFlagIds.ShouldraExgartudrama) == 1)
                    {
                        follower.ExgartumonAndShouldraFightPartII();

                        needNext = true;
                    }
                    else if (flags.Get(GameFlagIds.ShouldraExgartudrama) == 2)
                    {
                        follower.ExgartumonAndShouldraFightPartIII();

                        needNext = true;
                    }

                flags.Set(GameFlagIds.ShouldraSleepTimer, flags.Get(GameFlagIds.ShouldraSleepTimer) - 1);

                if (follower.ShouldersWarnings())
                    needNext = true;

                var sleepTimer = flags.Get(GameFlagIds.ShouldraSleepTimer);

                if ((sleepTimer == 0 || (sleepTimer < 0 && sleepTimer % 16 == 0)) && !Player.Sleeping)
                {
                    follower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark();

                    needNext = true;
                }

                if (flags.Get(GameFlagIds.ShouldraPlotCountdown) > 0 && View.Time.Hours == 3)
                    flags.Set(GameFlagIds.ShouldraPlotCountdown, flags.Get(GameFlagIds.ShouldraPlotCountdown) - 1);
            }

            return needNext;
        }

        public bool TimeChangeLarge()
        {
            var flags = CoC.Instance.Flags;

            var follower = SceneInstance<ShouldraFollowerScene>.Get();

            if (follower.FollowerShouldra() && flags.Get(GameFlagIds.ShouldraPlotCountdown) == 0
                && View.Time.Hours == 3)
            {
                flags.Set(GameFlagIds.ShouldraPlotCountdown, -1);

                follower.ShouldraDream1();

                return true;
            }

            if (flags.Get(GameFlagIds.ShouldraFollowerState) == 0.5f && View.Time.Hours == 6)
            {
                follower.MorningShouldraAlert();

                return true;
            }

            if (follower.FollowerShouldra() && flags.Get(GameFlagIds.ShouldraSleepTimer) <= -236
                && View.Time.Hours == 3 && Player.Gender > Gender.Genderless)
            {
                follower.NightTimeShouldraRapesThePC();

                return true;
            }

            if (flags.Get(GameFlagIds.ShouldraPaladinMaidenCountdown) > 0)
            {
                var horns = Player.Body.Get<HornsPart>();

                var tail = Player.Body.Get<TailPart>();

                if (Player.CockTotal() > 1
                    || Player.Body.Get<FacePart>().Id != BodyPartTypes.Human

                    || Player.Body.Get<LowerBodyPart>().Id != BodyPartTypes.Human

                    || tail.Id != BodyPartTypes.None

                    || horns.Value > 0

                    || !Player.IsPureEnough(15)

                    || Player.LongestCockLength() > 10

                    || Player.Tallness < 65

                    || Player.Tallness > 78

                    || Player.HasVagina())

                    flags.Set(GameFlagIds.ShouldraPaladinMaidenCountdown, 0);
                else
                {
                    flags.Set(GameFlagIds.ShouldraPaladinMaidenCountdown,

                        flags.Get(GameFlagIds.ShouldraPaladinMaidenCountdown) - 1);

                    if (flags.Get(GameFlagIds.ShouldraPaladinMaidenCountdown) == 0)
                    {
                        PaladinModeFollowup();

                        return true;
                    }
                }
            }

            return false;
        }

        public bool GhostLoli()
        {

            return GameSettings.ShouldraChild && GameSettings.AllowChild;
        }

        public void ShouldraGreeting()
        {
            ClearOutput();

            if ((silly && CoC.Instance.Flags.Get(GameFlagIds.ShouldraPaladinMaidenCounter) == 0) && (!(Player.CockTotal() > 1 || Player.Body.Get<FacePart>().Id != BodyPartTypes.Human || Player.Body.Get<LowerBodyPart>().Id != BodyPartTypes.Human || Player.Body.Get<TailPart>().Id != BodyPartTypes.None || Player.Horns.Id.Path != "none" || !Player.IsPureEnough(15) || Player.BiggestCockLength() > 10 || Player.Tallness < 65 || Player.Tallness > 78 || Player.HasVagina())))
                {
                    SuperCoolPaladinSexStuffDudeGasmJizzMonkies();

                    return ;
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetShouldra) < 3)
            {
                OutputText("Scenes.Shouldrascene.ShouldraGreeting.StepIntoRuinsCautiously".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetShouldra) == 0)
                    OutputText("Scenes.Shouldrascene.ShouldraGreeting.YoureRackThief".Localized());

                OutputText("Scenes.Shouldrascene.ShouldraGreeting.AcknowledgesReadiness".Localized());

                CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "shouldra")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenShouldra) >= 3 && CoC.Instance.Flags.Get(GameFlagIds.TimesMetShouldra) % 10 == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) > 0)
                {

                    CoC.Instance.Flags.Set(GameFlagIds.TimesMetShouldra, CoC.Instance.Flags.Get(GameFlagIds.TimesMetShouldra) + 1);

                    return ;
                }

                OutputText("Scenes.Shouldrascene.ShouldraGreeting.StepIntoRuinsAgain".Localized());

                OutputText("Scenes.Shouldrascene.ShouldraGreeting.BeforeConfirmFightingCrouch".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenShouldra) >= 3)
                {
                    OutputText("Scenes.Shouldrascene.ShouldraGreeting.MaybeNotProven".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenShouldra) == 3)
                        OutputText("Scenes.Shouldrascene.ShouldraGreeting.CantSaySawComing".Localized());

                    OutputText("Scenes.Shouldrascene.ShouldraGreeting.AcceptOfferOrFight".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Accept".Localized(), YankeeEchoPapa);

                    AddButton(1, "Common.Button.Fight".Localized(), NovemberAlphaHotel);
                }
                else
                    CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "shouldra")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetShouldra, CoC.Instance.Flags.Get(GameFlagIds.TimesMetShouldra) + 1);
        }

        public void YankeeEchoPapa()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.YankeeEchoPapa.GraciouslyAcceptOffer".Localized());

            DoNext(() => LittlePigLittlePigLetMeIn());
        }

        public void NovemberAlphaHotel()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.NovemberAlphaHotel.ShakeHeadRaisingWeapon".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "shouldra")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DefeatDannyPhantom()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.EarSplittingGhostlyWail".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) >= 1)
                    OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.GhostWord".Localized());

                OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.GirlFallsBackInjuries".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.GlancesLeggingsDoubleTake".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) < 1)
                    OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.LittleToNoEffect".Localized());
                else
                    OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.MotionNoEffectLibido".Localized());

                OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.LookingUpDesperationPlease".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) < 1)
                    OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.WhatDoesSheMean".Localized());
                else
                    OutputText("Scenes.Shouldrascene.DefeatDannyPhantom.WhatDoYouDo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHerIn".Localized(), LittlePigLittlePigLetMeIn);

                AddButton(1, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), NoSlimingSlimer);
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesBeatenShouldra, CoC.Instance.Flags.Get(GameFlagIds.TimesBeatenShouldra) + 1);
        }

        public void NoSlimingSlimer()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.NoSlimingSlimer.ShakeHeadAngrilyBrazen".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) == 0)
                OutputText("Scenes.Shouldrascene.NoSlimingSlimer.NotUpToWhims".Localized());
            else
                OutputText("Scenes.Shouldrascene.NoSlimingSlimer.MystifiedGhostPossess".Localized());

            OutputText("Scenes.Shouldrascene.NoSlimingSlimer.WhirlAboutStompAway".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) == 0)
                OutputText("Scenes.Shouldrascene.NoSlimingSlimer.TakingOverGoblin".Localized());
            else
                OutputText("Scenes.Shouldrascene.NoSlimingSlimer.SpiritTwingeOfGuilt".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void LittlePigLittlePigLetMeIn()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.YouDecideWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) == 0)
                OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.AffordAdventurousDesperate".Localized());
            else
                OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.SpiritualTagAlongFun".Localized());

            OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.AgreePossessUndresses".Localized());

            if (Player.Tallness < 84)
                OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.ArmsTightHug".Localized());
            else
                OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.UpArmsTightHug".Localized());

            OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.YoungHeavesHappySigh".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) == 0)
                OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.WaitWhatPossessYou".Localized());

            OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.SighsVoiceWavering".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesPossessedByShouldra, CoC.Instance.Flags.Get(GameFlagIds.TimesPossessedByShouldra) + 1);

            ClearButtons();

            Action gender = Player.Gender switch
            {
                Gender.Genderless => GenderlessGhostBuster,

                Gender.Male => PenisLossThatIsntALoss,

                Gender.Female => GhostGinaWinSexings,

                Gender.Hermaphrodite => HermaphroditeGhostsCumEctoplasm,

                _ => null!,
            };

            AddButton(0, "Common.Button.SexHere".Localized(), gender);

            if ((int)Player.Gender > 0 && ((Player.HasStatusEffect(StatusEffectIds.BoatDiscovery) && CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) > 0) || CoC.Instance.Flags.Get(GameFlagIds.TimesMetOoze) > 0))
            {
                OutputText("Scenes.Shouldrascene.LittlePigLittlePigLetMeIn.TakeLakeFindSomeone".Localized());

                AddButton(1, "Common.Button.Lake".Localized(), NowOnVickiLake);
            }
            else
                AddButton(1, "Common.Button.Lake".Localized(), null);
        }

        public void NowOnVickiLake()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.NowOnVickiLake.ImpulseSpinMarchLake".Localized());

            OutputText("Scenes.Shouldrascene.NowOnVickiLake.WhichEncounterSeek".Localized());

            ClearButtons();

            if (Player.HasStatusEffect(StatusEffectIds.BoatDiscovery) && CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) > 0)
                AddButton(0, "Common.Button.Shark".Localized(), SharkbustersVictory);
            else
                AddButton(0, "Common.Button.Shark".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetOoze) > 0)
                AddButton(1, "Common.Button.Ooze".Localized(), GhostGooGurlzDuckfaces);
            else
                AddButton(1, "Common.Button.Ooze".Localized(), null);
        }

        public void SharkbustersVictory()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.SharkbustersVictory.ReachLakeCastOff".Localized());

            OutputText("Scenes.Shouldrascene.SharkbustersVictory.DontGiveTimeSlam".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Shouldrascene.SharkbustersVictory.SquishingAgainstBosom".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Shouldrascene.SharkbustersVictory.ConfusedSharkGirlStiffly".Localized());

            OutputText("Scenes.Shouldrascene.SharkbustersVictory.MusesChoicesSnappingFingers".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.UseCock".Localized(), GhostIzmaPenis);
            else
                AddButton(0, "Common.Button.UseCock".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.UseVagina".Localized(), SharkyEctoginas);
            else
                AddButton(1, "Common.Button.UseVagina".Localized(), null);
        }

        public void SharkyEctoginas()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.BeckoningApproachEyebrow".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimedSharkginas) > 0)
                OutputText("Scenes.Shouldrascene.SharkyEctoginas.AgainWord".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.PantsBrowFurrowedWorry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimedSharkginas) == 0)
                OutputText("Scenes.Shouldrascene.SharkyEctoginas.AskWordsDie".Localized());
            else
                OutputText("Scenes.Shouldrascene.SharkyEctoginas.OnlyShakeHeadResignedly".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.MusclesRightArmWriggle".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.NowAlarmedPullBack".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.RedirectAttentionsBreasts".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.SetJawGoFervor".Localized());

            if (Player.HasKeyItem("Deluxe Dildo"))
                OutputText("Scenes.Shouldrascene.SharkyEctoginas.BiggestObjectTamaniDildo".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.EyesCrossScaleInsertion".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Shouldrascene.SharkyEctoginas.CakingCleavageLubrication".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.CloudArousalDisperses".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.GentleAsCanExtricates".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.LaterSofterKiss".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimedSharkginas) > 0)
                OutputText("Scenes.Shouldrascene.SharkyEctoginas.AgainWordAgain".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.ToGhostGirlDisrupting".Localized());

            OutputText("Scenes.Shouldrascene.SharkyEctoginas.SettledPartWays".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.TimedSharkginas, CoC.Instance.Flags.Get(GameFlagIds.TimedSharkginas) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void GhostIzmaPenis()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.SwiftlyDisrobeStiffening".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.StaggersForwardKnees".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.CradlingBallsReverent".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.PeriodBreak".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.LiftsChinTongueCockhead".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSharkpenised) == 0)
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.ExpressionClueless".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.OnlySmilesMistakeSpell".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.MusesLickingLipsSpell".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSharkpenised) == 0)
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.ContemplatesLengthening".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.AnticipationUndergoYetAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.JawDropsLipsPuff".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSharkpenised) == 0)
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.DeducesNatureSaliva".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.MagicSpittleStillWorks".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.ParagraphBreak".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.BeforeStopPositionsHead".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.SoftlyChurningSack".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.CringeThrashSpittleEffect".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.PromptlySlipsOutGarment".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.NotEvenHintHesitation".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.EnchantedTestesAudibleGroan".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostIzmaPenis.SomethingStirringTingling".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.UrethraBulgesThickSeed".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.DoesntEvenBotherExtricate".Localized());

            OutputText("Scenes.Shouldrascene.GhostIzmaPenis.SettledPartWaysAgain".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.TimesSharkpenised, CoC.Instance.Flags.Get(GameFlagIds.TimesSharkpenised) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void GhostGooGurlzDuckfaces()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.GhostGooGurlzDuckfaces.HikePathWaryEye".Localized());

            OutputText("Scenes.Shouldrascene.GhostGooGurlzDuckfaces.WrigglesWayOutTorso".Localized());

            if (Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Shouldrascene.GhostGooGurlzDuckfaces.PresentingCocksSpectralSlime".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Shouldrascene.GhostGooGurlzDuckfaces.SlippingHandDownClit".Localized());
                else
                    OutputText("Scenes.Shouldrascene.GhostGooGurlzDuckfaces.BothSexesThrobbing".Localized());

            OutputText("Scenes.Shouldrascene.GhostGooGurlzDuckfaces.GelatinousMassShudders".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.Slimeginaed) > 0 && CoC.Instance.Flags.Get(GameFlagIds.ShouldraSlimePenorTimes) > 0)
                    SlimeGhostGalForHerms(false);
                else
                    if (Random.Shared.Next(2) == 0)
                        SlimeyPenorsAhoy();
                    else
                        SlimeGinaFlation();
            }
            else
                if (Player.HasCock())
                    SlimeyPenorsAhoy();
                else
                    SlimeGinaFlation();
        }

        public void SlimeGinaFlation()
        {

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.StandChinHandConsider".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ContinueShapingCreation".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.TailThrustOneSide".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Shouldrascene.SlimeGinaFlation.GooeyMassSmooshes".Localized());
                else
                    OutputText("Scenes.Shouldrascene.SlimeGinaFlation.LegsForcedSplitsSink".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.AlthoughWonderfulSlowDown".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.BegHerToStop".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ConstantStimulationWeakening".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ParagraphBreak".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ConcernedGroanHungry".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.HearGreatDistanceChant".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.FrameJostledSlimeShrinks".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.FrameJostledGutPushing".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.DistendedBellyShivers".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ContentSecondPlaceAnus".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.MeetWanderer))
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.BothMarkusTesticlesCombined".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.AWheelbarrowWord".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ParagraphBreakThird".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.SlimeTooSmall".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.StuckUnluckyTurtle".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.LikeLowerBody".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Shouldrascene.SlimeGinaFlation.UndulatingGooeyMassWord".Localized());
                else
                    if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                        OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ArmsAndLegs".Localized());
                    else
                        OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ArmsTailLegs".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.TakingPleasureDistantCries".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.MinistrationsProvedJustWhat".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.SeeNippleWiggling".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.SeeTremblingNippleTwitching".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.JawDropsChestHeaves".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.HardlyNoticeBackAgain".Localized());

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.NipplesGrowWrapHand".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ThrobbingNipplecuntsGape".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.PaleEmeraldMoisturePores".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGinaFlation.OozyEvacuationKeepsUp".Localized());

            Player.Orgasm("Vaginal");

            Player.Orgasm("Anal");

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.SlimeGinaFlation.ParagraphBreakFourth".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }

            CoC.Instance.Flags.Set(GameFlagIds.Slimeginaed, CoC.Instance.Flags.Get(GameFlagIds.Slimeginaed) + 1);
        }

        public void SlimeyPenorsAhoy()
        {

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.BidsWatchSlimeRoil".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.FallsIntoSpellChanting".Localized(
                Player.CockDescript(x)
            ));

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.SquishingSmallBoobs".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.BeforeDwellSexuallyHindering".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSlimePenorTimes) > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.CantShakeFeeling".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.GrowingGirthStuffsGoo".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.IgnoringProtestsRaiseHigh".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.GelEncirclingWideMaleness".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.TeasingQuakingTestes".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.ProvokeOrgasmImpressive".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.MuffledWateryVoiceDemands".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.EvenIfAttemptAssist".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.SlappingAgainstYourTail".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.SmooshingAgainstGoo".Localized());
                else
                    OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.SwiftlyBruisingThighs".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.BestialGroanRumbles".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.NearVacantMindedRewards".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.ClimaxComesWearOff".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.CrazedLookGreenEyes".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.HeatSpreadsTesticles".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.DiscomfortBeginningProstate".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.OnceMoreHopsUpon".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.LegsApartCumSack".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.NearlyGoInsanePressure".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.DoesntNoticeFilledUp".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.OverfilledStateTwoMinutes".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.BallsToChurn".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.BodyPrepareForRelease".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.OnlyWarningFloodgates".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.WaddleOverMewlingGhost".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.LimpOverMewlingGhost".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.LikePushingThroughJello".Localized());

            OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.SmileShakeHead".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraSlimePenorTimes, CoC.Instance.Flags.Get(GameFlagIds.ShouldraSlimePenorTimes) + 1);

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.SlimeyPenorsAhoy.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void GenderlessGhostBuster()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.ExcitedRelieveAccumulating".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.HopsToFeetHut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0)
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.RaiseEyebrowGenuinelyCurious".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.DartingInsideDresser".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0)
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.CuriousContentsChestDildos".Localized());
            else
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.RecallDistinctlyContents".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.ReachesRummagesVibrators".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.TwoNewAdditionsBud".Localized());
            else
                if (Player.BiggestBreastSize() <= 8)
                    OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.YourBeginBounceAround".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.YourSwellMilkProduction".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.GhostContinuesDartingRoom".Localized());

            if (Player.BiggestBreastSize() <= 8)
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.EECupBreasts".Localized());
            else
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.LactationContinuesSettles".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.SatisfiedChangesTappingEggs".Localized());

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.NewWord".Localized());
            else
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.EvenLargerWord".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.NipplecuntsFlickingOn".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.SmacksBetweenLegsLack".Localized());

            if (Player.Anal.Looseness < 4)
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.StretchingButtObscenely".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.SentencePeriod".Localized());

            Player.ButtChange(31, true, true, false);

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.BabblingCrazedAddict".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.ChuckleAdmittedlyImpressed".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.TitsAlongElongatedReceding".Localized());
            else
                if (Player.BiggestBreastSize() <= 8)
                    OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.BoobfleshShrinkingBack".Localized());
                else
                    OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.NipplesShorteningStabilizing".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.HappySighHeaveUpright".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessGhostBuster.HerPleasureDealtAppears".Localized());

            Player.Orgasm("Anal");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraGenderlessFuckCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void PenisLossThatIsntALoss()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                DewormYourGhost();

                return ;
            }

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
            {
                PenisGartuanGhostSmexVictory();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPenisDefeatTimes) == 0)
                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.AbruptlyGaspsMovesHands".Localized());
            else
                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.ChucklesSoftlyRecalls".Localized());

            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.CommunicationBreaksWillSurges".Localized());

            if (Player.Intelligence < 50)
                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.HardToBreakDown".Localized());
            else
                if (Player.Intelligence < 90 || Player.CockThatFits(80) == -1)
                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.GainEdgeCraftier".Localized());
                else
                {
                    x = Player.CockThatFits(80);

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.PutsUpGoodFight".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.MovesToLeaveHuff".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.NoChanceVolition".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.MindfulOfWord".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.YourOtherDickWord".Localized());
                        else
                            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.RestOfCocksWord".Localized());

                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.MentallyDominatedClutches".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.YourOtherShaftPumps".Localized());
                        else
                            if (Player.CockTotal() == 3)
                                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.OtherTwoShaftsPumps".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.TwoShaftsPumpsThem".Localized());
                    }

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.ParagraphBreak".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.GoesToTownGenitals".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.AlthoughMouthFullVoice".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.HandAwayOtherDick".Localized());
                    else
                        if (Player.CockTotal() >= 3)
                            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.HandsAwayOtherDicks".Localized());
                        else
                            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.TheWord".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.GhostGirlSmilesPhallus".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.InYourBallsWord".Localized());
                    else
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.NearBaseShaftWord".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.PeriodSpace".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.FirstEffectsOverladenCumsac".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.StrangeFeelingTestes".Localized());

                    if (Player.BallSize >= 9)
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.GargantuanWord".Localized());
                    else
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.BasketballWord".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.SizedBallsWord".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.AdvantageDistractionJumps".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.PluralS".Localized());

                    OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.SwayingHipsToFro".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.LetHerCum".Localized(), LetShouldraIn);

                    AddButton(1, "Common.Button.Keepherout".Localized(), KickShouldraOut);

                    return ;
                }

            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.TakingCompleteControlRemoves".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.TesticlesChurnFertility".Localized());

            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.ManhoodSwellsNewGirth".Localized());

            OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.HerPleasureDealtAppears".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPenisDefeatTimes, CoC.Instance.Flags.Get(GameFlagIds.ShouldraPenisDefeatTimes) + 1);

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.PenisLossThatIsntALoss.ParagraphBreakThird".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void LetShouldraIn()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.LetShouldraIn.SquealsPartialControl".Localized());

            GhostBJFinisher(true);
        }

        public void KickShouldraOut()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.KickShouldraOut.RemindingPunishmentDeny".Localized());

            GhostBJFinisher(false);
        }

        public void GhostBJFinisher(bool inside)
        {

            int x = Player.CockThatFits(80);

            OutputText("Scenes.Shouldrascene.GhostBJFinisher.BackedUpGeyserTesticles".Localized(
                Player.CockDescript(x)
            ));

            if (inside)
                OutputText("Scenes.Shouldrascene.GhostBJFinisher.ExcitedlyConvulsing".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostBJFinisher.PracticallyLustParalyzed".Localized());

            OutputText("Scenes.Shouldrascene.GhostBJFinisher.GirlBothTooTired".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPenisDefeatTimes, CoC.Instance.Flags.Get(GameFlagIds.ShouldraPenisDefeatTimes) + 1);

            if (inside)
            {
                OutputText("Scenes.Shouldrascene.GhostBJFinisher.BreathesSoftlyWrappingArms".Localized());

                OutputText("Scenes.Shouldrascene.GhostBJFinisher.WatchHerMoveOff".Localized());
            }
            else
                OutputText("Scenes.Shouldrascene.GhostBJFinisher.WobblesExaggeratedlyFalls".Localized());

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.GhostBJFinisher.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void DewormYourGhost()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.DewormYourGhost.SettlesIntoBodyRecognizes".Localized());

            OutputText("Scenes.Shouldrascene.DewormYourGhost.ChucklingConsentPleadings".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraWormSceneCounter) == 0)
                OutputText("Scenes.Shouldrascene.DewormYourGhost.RespondsTakingControlHand".Localized());
            else
                OutputText("Scenes.Shouldrascene.DewormYourGhost.EyesFlareYellowLight".Localized());

            OutputText("Scenes.Shouldrascene.DewormYourGhost.ParagraphBreak".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            OutputText("Scenes.Shouldrascene.DewormYourGhost.SilenceThenCries".Localized());

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraWormSceneCounter, CoC.Instance.Flags.Get(GameFlagIds.ShouldraWormSceneCounter) + 1);
        }

        public void PenisGartuanGhostSmexVictory()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.OverwhelmingSurgeAnxious".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.ShockedNearlyLeaps".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.AfterMomentHesitationSinks".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.DemonRequestsLessBoisterous".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.LikeThisBitchHere".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.QuestionSayEndParagraph".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.BlushSomehowCheeks".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.TeasesHuskilyMindFilled".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.ExgartuanDragsMastBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraExgartuanSpiritSexCount) > 0)
                OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.OnceAgainWord".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.DeviouslyShiftsHipsUpward".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.SpecterBellyBalloons".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.GhostGirlScreamsPleasure".Localized());

            if (silly)
                OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.MiddleOfEjaculatoryWings".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.FinallyFireHose".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.PluggedStillOnSoftening".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.ProverbialDustSettles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraExgartuanSpiritSexCount) == 0)
                OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.SheAdmitsSpiritSex".Localized());
            else
                OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.SighsStillGreatRide".Localized());

            OutputText("Scenes.Shouldrascene.PenisGartuanGhostSmexVictory.DickDemonPipesIn".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartuanSpiritSexCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraExgartuanSpiritSexCount) + 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void GhostGinaWinSexings()
        {
            ClearOutput();

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2 && Random.Shared.Next(2) == 0)
            {
                GinaBoobgartuanShouldra();

                return ;
            }

            OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.HeavesDisappointedSigh".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraVaginalPossessions) == 0)
                OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.HmmsSeveralTimesElation".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.MusesOhWellAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.QuestionFallSilent".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.AgainstWillBetterInstincts".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.AwakeLaterPuddle".Localized(
                Player.VaginaDescript(0)
            ));

            Player.CuntChange(-1, true, true, false);

            OutputText("Scenes.Shouldrascene.GhostGinaWinSexings.HerPleasureDealtAppears".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraVaginalPossessions, CoC.Instance.Flags.Get(GameFlagIds.ShouldraVaginalPossessions) + 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void HermaphroditeGhostsCumEctoplasm()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraHermsexCount) == 0)
                OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.GaspsMovesHands".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraHermsexCount) > 0)
                OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.ChucklesSoftlyRecallsAgain".Localized());

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.CommunicationBreaksAgain".Localized());

            if (Player.Intelligence < 50)
                OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.HardToBreakAgain".Localized());
            else
                if (Player.Intelligence < 90 || Player.CockThatFits(80) == -1)
                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.GainEdgeCraftierAgain".Localized());
                else
                {
                    x = Player.CockThatFits(80);

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.GoodFightAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.MovesLeaveHuffAgain".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.NoChanceVolitionAgain".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.MindfulOfWordAgain".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.OtherDickWordAgain".Localized());
                        else
                            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.RestCocksWordAgain".Localized());

                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.MentallyDominatedClutchesAgain".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.OtherShaftPumpsAgain".Localized());
                        else
                            if (Player.CockTotal() == 3)
                                OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.OtherTwoShaftsAgain".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.TwoShaftsThemAgain".Localized());
                    }

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.ParagraphBreak".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.GoesTownGenitalsAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.MouthFullVoiceAgain".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.HandOtherDickAgain".Localized());
                    else
                        if (Player.CockTotal() >= 3)
                            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.HandsOtherDicksAgain".Localized());
                        else
                            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.TheWordAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.SmilesPhallusAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.InBallsWordAgain".Localized());
                    else
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.BaseShaftWordAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.PeriodSpaceAgain".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.OverladenCumsacAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.StrangeFeelingTestesAgain".Localized());

                    if (Player.BallSize > 10)
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.GargantuanWordAgain".Localized());
                    else
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.BasketballWordAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.SizedBallsWordAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.DistractionJumpsAgain".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.PluralSAgain".Localized());

                    OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.SwayingHipsFroAgain".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.LetHerCum".Localized(), LetShouldraIn);

                    AddButton(1, "Common.Button.Keepherout".Localized(), KickShouldraOut);

                    return ;
                }

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.CompleteControlAgain".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.FeelFaceFlushRelives".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.StoppingDoubleHandedStroke".Localized());

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.ChucklesMalevolentlyGroan".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.EyesWidenRigidElastic".Localized());

            Player.CuntChange(Player.CockArea(x), true, true, false);

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.UnableDoMuchSpell".Localized());

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.FinallySeedSpillsExplosive".Localized());

            OutputText("Scenes.Shouldrascene.HermaphroditeGhostsCumEctoplasm.PleasureDealtAgain".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraHermsexCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraHermsexCount) + 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void LoseToShouldra()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.LoseToShouldra.FallDullThudBeaten".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrascene.LoseToShouldra.EachPulsePushesFurther".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Shouldrascene.LoseToShouldra.CantDenyHeatWelling".Localized());
                else
                    OutputText("Scenes.Shouldrascene.LoseToShouldra.CantDenyHeatGathering".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldra.WaistbandBurstsButtSpill".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldra.WonderingPointRaiseHead".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrascene.LoseToShouldra.QuiveringMemberWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Shouldrascene.LoseToShouldra.SoppingGashWord".Localized());
                else
                    OutputText("Scenes.Shouldrascene.LoseToShouldra.SensitiveCrotchFleshWord".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldra.AbsentStrokeChanting".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldra.FinallyComeOutImagery".Localized());

            if (Player.Gender == Gender.Male)
            {
                if (Player.HasStatusEffect(StatusEffectIds.Infested))
                    LoseToShouldraWithWorms();
                else
                    if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                        GhostGartuanLossSceneOrSomeShit();
                    else
                        if (Player.BiggestCockArea() >= 200)
                            ShouldraGiantCockLoss();
                        else
                            OurDadTaughtUsNotToBeAshamedOfOurDicks();
            }
            else
                if (Player.Gender == Gender.Female)
                    GhostGinaLosses();
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                            GhostGartuanLossSceneOrSomeShit();
                        else
                            if (Player.BiggestCockArea() >= 200)
                                ShouldraGiantCockLoss();
                            else
                                LoseToShouldraAsHerm();
                    }
                    else
                        GenderlessShouldrasLossRapes();
        }

        public void OurDadTaughtUsNotToBeAshamedOfOurDicks()
        {

            var x = Player.BiggestCockIndex();

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPenisDefeatTimes) == 0)
                OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.AbruptlyGaspsInExcitement".Localized());
            else
                OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.OoosSoftlyRecallsAnatomy".Localized());

            OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.FeelHandsMovingUnbidden".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.FeelFaceFlushRelives".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.TesticlesChurnFertility".Localized());

            OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.ManhoodSwellsNewGirth".Localized());

            OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.StrangeSensationMidsection".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPenisDefeatTimes, CoC.Instance.Flags.Get(GameFlagIds.ShouldraPenisDefeatTimes) + 1);

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.OurDadTaughtUsNotToBeAshamedOfOurDicks.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void LoseToShouldraWithWorms()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraWormSceneCounter) == 0)
                OutputText("Scenes.Shouldrascene.LoseToShouldraWithWorms.GaspsExcitementAgain".Localized());
            else
                OutputText("Scenes.Shouldrascene.LoseToShouldraWithWorms.OoosRecallsAnatomyAgain".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraWithWorms.FeelHandsMovingProves".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraWithWorms.ClumsilyThrowsBodyBackwards".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraWithWorms.RespondsMeetingEyesDaggers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraWormSceneCounter, CoC.Instance.Flags.Get(GameFlagIds.ShouldraWormSceneCounter) + 1);

            Player.HealthChange(-10000);

            CombatScene.CleanupAfterCombat();
        }

        public void GhostGinaLosses()
        {

            OutputText("Scenes.Shouldrascene.GhostGinaLosses.HeavesDisappointedSighAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraVaginalPossessions) == 0)
                OutputText("Scenes.Shouldrascene.GhostGinaLosses.HmmsSeveralTimesTapping".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostGinaLosses.MusesOhWellAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaLosses.ParagraphBreak".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaLosses.QuestionSilentAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaLosses.BetterInstinctsAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGinaLosses.AwakeLaterAttempt".Localized(
                Player.VaginaDescript(0)
            ));

            Player.CuntChange(-1, true, true, false);

            OutputText("Scenes.Shouldrascene.GhostGinaLosses.SensationMidsectionAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraVaginalPossessions, CoC.Instance.Flags.Get(GameFlagIds.ShouldraVaginalPossessions) + 1);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.GhostGinaLosses.ParagraphBreakAgain".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void LoseToShouldraAsHerm()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraHermsexCount) == 0)
                OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.GaspsExcitementThird".Localized());
            else
                OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.CoosSoftlyRecallsAnatomy".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.HandsMovingUnbidden".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.FaceFlushRelivesAgain".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.StoppingDoubleStroke".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.ChucklesEvillyGroan".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.WidenRigidElasticAgain".Localized());

            Player.CuntChange(Player.CockArea(0), true);

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.UnableSpellcastAgain".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.SeedSpillsExplosiveAgain".Localized());

            OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.SensationMidsectionThird".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraHermsexCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraHermsexCount) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.LoseToShouldraAsHerm.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void ShouldraGiantCockLoss()
        {

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.AttentionMovesGiganticDick".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.WithoutWordGrabsGoblin".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.BemusedWord".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ExcitedWord".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.FlairFinishesIncantation".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.SearchesBodySignsChange".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ResistsUrgeSmashTits".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ByThenSpellDone".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.InsteadExplanationsMerely".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.GreenGirlsDelight".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ReachesOverKneads".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.SmallFirmBCups".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 1)
                    OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.PerkySizeableCCups".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 2)
                        OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.JigglyDCups".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 3)
                            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.BigSquishyECups".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 4)
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.OversizedHeavingFCups".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.GiganticBaredGCups".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.TheGoblinWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.StopsProtestStimulated".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.CriesOutEcstasySensation".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.AlthoughDoesntStopSecond".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.NoticeExcitedBlush".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.PinchesStiffeningNipplePulls".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.PullsBreastAlong".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.BreastAlongFlat".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.InwardlyGasp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.EffectsOfMagicRevealed".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DespiteSeeingMagicBefore".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.TurnedBreastsPuttyLike".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.EvidentlyWord".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.OnceAgainWord".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.BeginsToSpeedProcess".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.GaspsInAmazement".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.CantRestrainUrges".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.SwiftlyReachingDownGrab".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.PetiteBoobsWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 1)
                    OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ModestBosomWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 2)
                        OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.SizableChestWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 3)
                            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.LargeEndowmentsWord".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 4)
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.HugeTitsWord".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.TitanicMelonsWord".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.CryingExcitementBreastsBillow".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.FirstGoblinBreastsReached".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ECupsWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 1)
                    OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.FCupsWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 2)
                        OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.GCupsWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 3)
                            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.HCupsWord".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 4)
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ICupsWord".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.JCupsWord".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.SpeedsUpMassage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.HHCupsWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 1)
                    OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.IICupsWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 2)
                        OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.JJCupsWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 3)
                            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.KKCupsWord".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 4)
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.LLCupsWord".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.MMCupsWord".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.TurnsAttentionSecondGoblin".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 0)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DoubleDCup".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 1)
                    OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DoubleECup".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 2)
                        OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DoubleFCup".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 3)
                            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DoubleGCup".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) == 4)
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DoubleHCup".Localized());
                            else
                                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.DoubleICup".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.BreastsOutBlowing".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.NotNeedingAskedTwice".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.AnxiousMoanTiltHead".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) <= 4)
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.KeptLeastCupSize".Localized());
            else
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ReachedTheirCrescendo".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.HappilyFondleWanderAway".Localized());

            OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.StrangeSensationMidsection".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraUsesYourGiantCockCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraUsesYourGiantCockCount) + 1);

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.ShouldraGiantCockLoss.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void GenderlessShouldrasLossRapes()
        {

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.ExcitedFindHost".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.HopsUpForHut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0)
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.EyebrowCuriousAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.DartingInsideDresserAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0)
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.ChestDildosAgain".Localized());
            else
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.RecallDistinctlyContentsAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.ReachesRummagesVibratorsAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.MarvelResourcefulRamifications".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.NewAdditionsBudAgain".Localized());
            else
                if (Player.BiggestBreastSize() <= 8)
                    OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.BeginBounceAround".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.YourSwellsMilkProduction".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.GhostDartingRoomAgain".Localized());

            if (Player.BiggestBreastSize() <= 8)
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.CupBreastsAgain".Localized());
            else
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.LactationContinues".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.MovesBackTableSatisfied".Localized());

            if (!Player.HasFuckableNipples())
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.NewWordAgain".Localized());
            else
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.EvenLargerWordAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.NipplecuntsFlickingOnVirgin".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.SmacksItWord".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.AgainstYourCloaca".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.IntoYourCushionyGoo".Localized());
                else
                    OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.BetweenYourLegsWord".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.SeeminglyForgettingLack".Localized());

            if (Player.Anal.Looseness <= 4)
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.StretchingButtObscenelyAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.SentencePeriod".Localized());

            Player.ButtChange(50, true, true, false);

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.BabblingCrazedAddictAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.ChuckleImpressedAgain".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.TitsAlongElongatedFlat".Localized());
            else
                if (Player.BiggestBreastSize() <= 8)
                    OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.BoobfleshShrinkingBackNormal".Localized());
                else
                    OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.NipplesStabilizingAgain".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.HappySighHeaveUpright".Localized());

            OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.SensationMidsectionAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraGenderlessFuckCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) + 1);

            Player.Orgasm("Tits");

            Player.Orgasm("Anal", false);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.GenderlessShouldrasLossRapes.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void GhostGartuanLossSceneOrSomeShit()
        {

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.EntireBodyShiversDelight".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.ShockedNearlyLeapsAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.HesitationSinksAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.DemonRequestsAgain".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.LikeThisBitchHere".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.QuestionSayEndAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.BlushSomehowCheeksAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.TeasesHuskilyAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.DragsMastBackAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraExgartuanSpiritSexCount) > 0)
                OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.OnceAgainWordAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.ShiftsHipsUpwardAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.SpecterBellyBalloonsAgain".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.ScreamsPleasureAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.PluggedSofteningAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.ProverbialDustSettlesAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraExgartuanSpiritSexCount) == 0)
                OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.AdmitsSpiritSexAgain".Localized());
            else
                OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.SighsGreatRideAgain".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.ParagraphBreak".Localized());

            OutputText("Scenes.Shouldrascene.GhostGartuanLossSceneOrSomeShit.DickDemonPipesAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartuanSpiritSexCount, CoC.Instance.Flags.Get(GameFlagIds.ShouldraExgartuanSpiritSexCount) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void SuperCoolPaladinSexStuffDudeGasmJizzMonkies()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.SuperCoolPaladinSexStuffDudeGasmJizzMonkies.GallantlyStridingWreckage".Localized());

            OutputText("Scenes.Shouldrascene.SuperCoolPaladinSexStuffDudeGasmJizzMonkies.WhileSpeaksPureBody".Localized());

            OutputText("Scenes.Shouldrascene.SuperCoolPaladinSexStuffDudeGasmJizzMonkies.LookUpGazeFace".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPaladinMaidenCounter, CoC.Instance.Flags.Get(GameFlagIds.ShouldraPaladinMaidenCounter) + 1);

            AddButton(0, "Common.Button.Yes".Localized(), CourtCrazyGirlsDotCom);

            AddButton(1, "Common.Button.No".Localized(), NoCourtingYouFag);
        }

        public void NoCourtingYouFag()
        {

            OutputText("Scenes.Shouldrascene.NoCourtingYouFag.QuicklyTellMisunderstanding".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CourtCrazyGirlsDotCom()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.CourtCrazyGirlsDotCom.SmileWidensVenerableVisage".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPaladinMaidenCountdown, 48);

            DoNext(() => ReturnToCamp(1));
        }

        public void PaladinModeFollowup()
        {

            OutputText("Scenes.Shouldrascene.PaladinModeFollowup.AnxiousTwoDays".Localized());

            OutputText("Scenes.Shouldrascene.PaladinModeFollowup.NoticesAdmirationRadiant".Localized());

            OutputText("Scenes.Shouldrascene.PaladinModeFollowup.AskWhyOwn".Localized());

            OutputText("Scenes.Shouldrascene.PaladinModeFollowup.EyesAmberFlames".Localized());

            if (View.Time.Hours < 6 || View.Time.Hours >= 23)
                OutputText("Scenes.Shouldrascene.PaladinModeFollowup.RageFapConsiderBed".Localized());

            Player.Orgasm("Dick");

            DoNext(PlayerMenu);
        }

        public void GinaBoobgartuanShouldra()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.SighsSettlingLadyparts".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.SeveralMomentsWhat".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.RetortsWhoThis".Localized(
                Appearance.BiggestBreastSizeDescript(Player)
            ));

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.CombinedSemiSphericalRoom".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.BeforeReactClosed".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.NursingTransfersBosom".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.SucklingArousal".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.ParanormalStomachGurgles".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.GhostPantsGatheringWits".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.ExgartuanPurrsPinching".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.GoodThingGhostStomach".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.TriesHardestGrabBelly".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.DuetBeautifulScreams".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.UsedToBeBall".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.ChucklesOnceSnapsFingers".Localized());

            OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.ThoughExhaustedKeep".Localized());

            Player.Orgasm("Tits");

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.GinaBoobgartuanShouldra.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }
        }

        public void SlimeGhostGalForHerms(bool clearOut = false)
        {
            ClearOutput();

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.NakedTrotEmeraldOoze".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) == 0)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.GotThingFiguredOut".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) == 1)
                    OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.ThingFiguredReal".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) == 2)
                        OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.ThingFiguredPromise".Localized());
                    else
                        OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.DontWorryTooMuch".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.ConfidenceNeverAnIssue".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) <= 2)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.MaybeFirmGrip".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.RegardlessDogPonyShow".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.MoistHandShoulderSkepticism".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.BlobReformsUnremarkableHand".Localized());

            if (Player.Cocks[0].Length > 24)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.NotUpToScale".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.OtherThanSparklingComplexion".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.HauntedSlimePartsLegs".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.SeesawSexRages".Localized());

            if (Player.Vaginas[0].Looseness == 0)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.TightWord".Localized());
            else
                if (Player.Vaginas[0].Looseness == 1)
                    OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.ForgivingWord".Localized());
                else
                    if (Player.Vaginas[0].Looseness == 2)
                        OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.LooseWord".Localized());
                    else
                        if (Player.Vaginas[0].Looseness == 3)
                            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.GapingWord".Localized());
                        else
                            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.CavernousWord".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.VelvetyScabbardHandsBusy".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.BothPressTitsTogether".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.SlimePressesChest".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.FierceBattleIncreasesTempo".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.AtLastSeizeVictory".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.MomentContactGazeDarts".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.StillBreathlessFewOptions".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) > 0)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.AgainWord".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.HoweverMuchSurpriseMire".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.DamnThingCastingAnother".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.RecognizableAlarmingly".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.DishOutWithGusto".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.TheseRevelationsLastThing".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.AnxiousRidBurden".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.SpiraledOutControl".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.EventsCatchingUpWavering".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.BeingsForMilesHeard".Localized());

            if (View.Time.Hours < 11)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.MorningWord".Localized());
            else
                if (View.Time.Hours < 17)
                    OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.AfternoonWord".Localized());
                else
                    OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.NightWord".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.SoundOfNoConcern".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.LeftOnTheShore".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.WhatHappenedToGhost".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.FaceShoutsBoo".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.ExplainsJumpedShip".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) == 0)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.CouldntSpankSave".Localized());
            else
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.HaventPutPractice".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) == 1)
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.AfraidGivenUpGhost".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) > 1)
                    OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.WillingToDoAgain".Localized());

            OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.PaysWarmHeartedGrin".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.Ectoplasm);

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Shouldrascene.SlimeGhostGalForHerms.ParagraphBreak".Localized());

                InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
            }

            CoC.Instance.Flags.Set(GameFlagIds.GhostGirlSlimeXShouldraCounter, CoC.Instance.Flags.Get(GameFlagIds.GhostGirlSlimeXShouldraCounter) + 1);
        }

    }
}