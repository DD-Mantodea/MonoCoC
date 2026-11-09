using System;
using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Dungeons
{

    public static class WizardTowerRooms
    {
        public static void ClosePillarCell(DungeonContext context, int x, int y)
        {
            var row = context.Floor.Map[y];

            if (x < 0 || x >= row.Length)
                return;

            var chars = row.ToCharArray();

            chars[x] = '#';

            context.Floor.Map[y] = new string(chars);
        }

        public const int DefeatedSentinels = 1;

        public const int SolvedGatePuzzle = 2;

        public const int SolvedMirrorPuzzleBit = 4;

        public const int SolvedMirrorPuzzle = SolvedMirrorPuzzleBit;

        public const int MetJeremiah = 8;

        public const int DestroyedJeremiah = 16;

        public const int FoundLockedDoor = 32;

        public const int AllowedExit = 64;

        public const int LearnedKey = 128;

        public const int DefeatedVilkus = 256;

        public const int JeremiahReformed = 512;

        public const int LaurentiusFound = 1024;

        public const int LaurentiusDefeated = 2048;

        public const int TowerAwoken = 4096;

        public static bool SentinelVictoryPending;

        public static int Progress => CoC.Instance.Flags.Get(GameFlagIds.WizardTowerProgress);

        public static bool Has(int bit) => (Progress & bit) != 0;

        [Register("coc:wizard_tower_room_entrance")]
        public static void RoomEntrance(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.RoomEntrance.StandingInExtremely".Localized());
        }

        [Register("coc:wizard_tower_room_entrance_two")]
        public static void RoomEntrance2(DungeonContext context)
        {
            if (!Has(DefeatedSentinels))
            {
                context.Write("Scenes.WizardTower.EnterForReal.MoveTowardsGateIgnoring".Localized());

                context.Write("Scenes.WizardTower.EnterForReal.TouchTheSmoothSurface".Localized());

                context.Write("Scenes.WizardTower.EnterForReal.TurnAroundAndBehold".Localized());

                context.Write("Scenes.WizardTower.EnterForReal.WarriorIsTheFirst".Localized());

                context.Write("Scenes.WizardTower.EnterForReal.FightOnYourHands".Localized());

                var dungeon = Scenes.Scene.Current as DungeonScene;

                CombatScene.StartMultiple(
                    new[]
                    {
                        EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "sentinel_of_apostasy")),

                        EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "sentinel_of_blasphemy")),

                        EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "sentinel_of_heresy"))
                    },

                    () =>
                    {
                        if (dungeon != null)
                            Scenes.Scene.Go(dungeon);
                    },

                    false);

                return;
            }

            if (SentinelVictoryPending)
            {
                SentinelVictoryPending = false;

                context.Write("Scenes.WizardTower.VictoryAgainstSentinels.OneDecisiveStrike".Localized());

                context.Write("Scenes.WizardTower.VictoryAgainstSentinels.MoveTowardsGateOnce".Localized());

                context.Write("Scenes.WizardTower.VictoryAgainstSentinels.HeardSoundOfStone".Localized());

                context.Write("Scenes.WizardTower.VictoryAgainstSentinels.WeakenedConditionImpaired".Localized());

                context.Write("Scenes.WizardTower.VictoryAgainstSentinels.DontUnderstandWhatMagic".Localized());
            }

            if (!Has(SolvedGatePuzzle))
            {
                GatePuzzle(context);

                return;
            }

            context.Write("Scenes.WizardTower.RoomEntrance2.DespiteSourcelessLight".Localized());

            context.Write(CoC.Instance.Player.Intelligence > 85

                ? "Scenes.WizardTower.RoomEntrance2.CursoryExamination".Localized()

                : "Scenes.WizardTower.RoomEntrance2.NotReallySureHow".Localized());
        }

        public static void GatePuzzle(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.ToGatePuzzle.TouchSurfaceOfThe".Localized());

            context.Write("Scenes.WizardTower.ToGatePuzzle.FrustratingSearch".Localized());

            context.Write("Scenes.WizardTower.ToGatePuzzle.ThreeWizardsCongregated".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Althanos".Localized(), () => AnswerPuzzle(context, 0));

            Scenes.Scene.AddButton(1, "Common.Button.Laurentius".Localized(), () => AnswerPuzzle(context, 1));

            Scenes.Scene.AddButton(2, "Common.Button.Garland".Localized(), () => AnswerPuzzle(context, 2));
        }

        public static void AnswerPuzzle(DungeonContext context, int answer)
        {
            Scenes.Scene.ClearButtons();

            if (answer != 1)
            {
                context.Write("Scenes.WizardTower.AnswerPuzzle.WordsDisappearSoonAfter".Localized());

                context.Write("Scenes.WizardTower.AnswerPuzzle.NotWelcomePay".Localized());

                context.Write("Scenes.WizardTower.AnswerPuzzle.MagicCircleLightsRunes".Localized());

                context.Write("Scenes.WizardTower.AnswerPuzzle.BeforeYouCanReact".Localized());

                context.Write("Scenes.WizardTower.AnswerPuzzle.MinusFiveStats".Localized());

                var player = CoC.Instance.Player;

                player.Strength = Math.Max(1, player.Strength - 5);

                player.Toughness = Math.Max(1, player.Toughness - 5);

                player.Speed = Math.Max(1, player.Speed - 5);

                player.Intelligence = Math.Max(1, player.Intelligence - 5);

                context.Write("Scenes.WizardTower.AnswerPuzzle.LikeToTryAgain".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.TryAgain".Localized(), () => GatePuzzle(context));

                return;
            }

            context.Write("Scenes.WizardTower.AnswerPuzzle.WordsDisappearAgain".Localized());

            context.Write("Scenes.WizardTower.AnswerPuzzle.LinesDelineatingGate".Localized());

            context.Write("Scenes.WizardTower.AnswerPuzzle.WhateverInsideProbablyNot".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + SolvedGatePuzzle);

            Scenes.Scene.AddButton(0, "Common.Button.EnterTower".Localized(), () => DimensionalShift(context));
        }

        public static void DimensionalShift(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.EnterTowerDimensionalShift.StepInsideObsidianTower".Localized());

            context.Write("Scenes.WizardTower.EnterTowerDimensionalShift.AfterMinutesOfWalking".Localized());

            context.Write("Scenes.WizardTower.EnterTowerDimensionalShift.PulseOfLightIrradiates".Localized());

            context.Write("Scenes.WizardTower.EnterTowerDimensionalShift.InAFlashCorridor".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
            {
                context.Write("Scenes.WizardTower.DimensionalShift2.AlmostAsSuddenly".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => RoomEntrance2(context));
            });
        }

        [Register("coc:wizard_tower_corridor")]
        public static void Corridor(DungeonContext context)
        {
            if (!Has(SolvedMirrorPuzzle))
            {
                context.Write("Scenes.WizardTower.MirrorPuzzle.MoveAwayFromCorpses".Localized());

                context.Write("Scenes.WizardTower.MirrorPuzzle.CorridorLeadsToMuch".Localized());

                context.Write("Scenes.WizardTower.MirrorPuzzle.TakeAStep".Localized());

                context.Write("Scenes.WizardTower.MirrorPuzzle.TakeAnotherStep".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
                {
                    if (Scenes.Scene.Current is DungeonScene dungeon)
                        Scenes.Scene.Go(new WizardTowerMirrorPuzzleScene(dungeon));
                });

                return;
            }

            context.Write("Scenes.WizardTower.Corridor.FreedFromTheMaze".Localized());
        }

        [Register("coc:wizard_tower_pillar")]
        public static void Pillar(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.Pillar.GoingAroundPillar".Localized());
        }

        [Register("coc:wizard_tower_pillar_two")]
        public static void Pillar2(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.Pillar2.SawPlentyOfCorpses".Localized());

            context.Write("Scenes.WizardTower.Pillar2.PosesVarySome".Localized());

            context.Write("Scenes.WizardTower.Pillar2.AheadLiesSteepStaircase".Localized());
        }

        [Register("coc:wizard_tower_staircase")]
        public static void Staircase(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.Staircase.ReachedTheEndOf".Localized());

            context.Write("Scenes.WizardTower.Staircase.StepsIncrediblyNumerous".Localized());

            context.Write("Scenes.WizardTower.Staircase.RegardlessYouSeemTo".Localized());
        }

        [Register("coc:wizard_tower_upper_corridor")]
        public static void UpperCorridor(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.UpperCorridor.CorridorInTheTower".Localized());

            if (!Has(FoundLockedDoor))
            {
                context.Write("Scenes.WizardTower.UpperCorridor.LittleProgressBeforeFinding".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + FoundLockedDoor);
            }
        }

        [Register("coc:wizard_tower_final_door")]
        public static void FinalDoor(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.FinalDoor.JeremiahsSealUndone".Localized());

            context.Write("Scenes.WizardTower.FinalDoor.ToTheNorthAnother".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.VilkusDefeatDay) + 2 <= Scenes.Scene.View.Time.Days && Has(DefeatedVilkus))
                context.Write("Scenes.WizardTower.FinalDoor.ThisSectionOfTower".Localized());
            else
            {
                context.Write("Scenes.WizardTower.FinalDoor.SilenceDeafening".Localized());

                if (!Has(DefeatedVilkus))
                    context.Write("Scenes.WizardTower.FinalDoor.DistinctSenseOf".Localized());
            }
        }

        [Register("coc:wizard_tower_the_chamber")]
        public static void TheChamber(DungeonContext context)
        {
            if (!Has(DefeatedVilkus))
            {
                context.Write("Scenes.WizardTower.EncounterVilkus.MoveUpStairsSlowly".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.SixteenLethiciteCrystals".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.CenterOfRoom".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.CloakedFigureGetsUp".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.WillNotLetTake".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.OldManNotDemon".Localized((CoC.Instance.Player.Corruption >= 60 ? ", despite your corruption" : "")));

                context.Write("Scenes.WizardTower.EncounterVilkus.CacklesAndCoughs".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.BrandishesHisEstoc".Localized());

                context.Write("Scenes.WizardTower.EncounterVilkus.FaintlyPulsesWithEnergy".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => StartVilkus(context));

                return;
            }

            context.Write("Scenes.WizardTower.TheChamber.ChamberVilkusResided".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.VilkusDefeatDay) + 2 <= Scenes.Scene.View.Time.Days && !Has(LaurentiusFound))
            {
                context.Write("Scenes.WizardTower.TheChamber.PillarOfLightSits".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Light".Localized(), () => MeetLaurentius(context));
            }
        }

        public static void StartVilkus(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            var dungeon = Scenes.Scene.Current as DungeonScene;

            CombatScene.StartMultiple(
                new[] { EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "arch_inquisitor_vilkus")) },

                () =>
                {
                    if (Has(DefeatedVilkus))
                        ShowVilkusDefeat(context, dungeon);
                    else if (dungeon != null)
                        Scenes.Scene.Go(dungeon);
                },

                false);
        }

        public static void ShowVilkusDefeat(DungeonContext context, DungeonScene? dungeon)
        {
            context.Write("Scenes.WizardTower.DefeatVilkus.VilkusFallsTrembling".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.AreYouAbandoningMe".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.ApproachCorruptedInquisitor".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.WeAreDoomed".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.LiftsHandAndPrepares".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.FaithDiedToo".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.BringsHandToHis".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.FinallyRelax".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.OneObjectSurvives".Localized());

            context.Write("Scenes.WizardTower.DefeatVilkus.KeyItemTalismanOf".Localized());

            Scenes.Scene.AwardAchievement(Achievements.AchievementIds.WeAreTheFlame, true, true);

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
            {
                if (dungeon != null)
                    Scenes.Scene.Go(dungeon);
            });
        }

        public static void MeetLaurentius(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.MeetLaurentius.IntoPillarOfLight".Localized());

            context.Write("Scenes.WizardTower.MeetLaurentius.GlowingPulsesGrow".Localized());

            context.Write("Scenes.WizardTower.MeetLaurentius.OneFinalPulseStrikes".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => EnterPillarFloor(context));
        }

        public static readonly string[] PillarTexts =

        [
            "Scenes.WizardTower.RunFuncPillar.LightPureEnergy",

            "Scenes.WizardTower.RunFuncPillar.RiseThroughPillar",

            "Scenes.WizardTower.RunFuncPillar.AllJustWishesOf",

            "Scenes.WizardTower.RunFuncPillar.GoneThroughMuchTo",

            "Scenes.WizardTower.RunFuncPillar.OneLessonYourBody",

            "Scenes.WizardTower.RunFuncPillar.ForgetItselfAndLet",

            "Scenes.WizardTower.RunFuncPillar.WelcomeWord"
        ];

        public static void EnterPillarFloor(DungeonContext context)
        {
            context.ChangeFloor(1, "pillar58");

            context.Refresh();
        }

        [Register("coc:wizard_tower_pillar_n_fifty_eight")]
        public static void Pillar58(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.RunFuncPillar.LightPureEnergy".Localized());
        }

        [Register("coc:wizard_tower_pillar_fifty")]
        public static void Pillar50(DungeonContext context)
        {
            ClosePillarCell(context, 2, 7);

            context.Write("Scenes.WizardTower.RunFuncPillar.RiseThroughPillar".Localized());
        }

        [Register("coc:wizard_tower_pillar_n_forty_two")]
        public static void Pillar42(DungeonContext context)
        {
            ClosePillarCell(context, 2, 6);

            context.Write("Scenes.WizardTower.RunFuncPillar.AllJustWishesOf".Localized());
        }

        [Register("coc:wizard_tower_pillar_n_thirty_four")]
        public static void Pillar34(DungeonContext context)
        {
            ClosePillarCell(context, 2, 5);

            context.Write("Scenes.WizardTower.RunFuncPillar.GoneThroughMuchTo".Localized());
        }

        [Register("coc:wizard_tower_pillar_n_twenty_six")]
        public static void Pillar26(DungeonContext context)
        {
            ClosePillarCell(context, 2, 4);

            context.Write("Scenes.WizardTower.RunFuncPillar.OneLessonYourBody".Localized());
        }

        [Register("coc:wizard_tower_pillar_eighteen")]
        public static void Pillar18(DungeonContext context)
        {
            ClosePillarCell(context, 2, 3);

            context.Write("Scenes.WizardTower.RunFuncPillar.ForgetItselfAndLet".Localized());
        }

        [Register("coc:wizard_tower_pillar_ten")]
        public static void Pillar10(DungeonContext context)
        {
            ClosePillarCell(context, 2, 2);

            context.Write("Scenes.WizardTower.RunFuncPillar.WelcomeWord".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => MeetLaurentius2(context));
        }

        public static void MeetLaurentius2(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.MeetLaurentius2.ComeToAmazed".Localized());

            context.Write("Scenes.WizardTower.MeetLaurentius2.LookAroundSomehowEnded".Localized());

            context.Write("Scenes.WizardTower.MeetLaurentius2.MassiveStormsAndGusts".Localized());

            context.Write("Scenes.WizardTower.MeetLaurentius2.CenterOfArea".Localized());

            Scenes.Scene.AddButton(0, "Combat.Button.Approach".Localized(), () => MeetLaurentius3(context));
        }

        public static void MeetLaurentius3(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            if (!Has(LaurentiusFound))
            {
                context.Write("Scenes.WizardTower.MeetLaurentius3.ApproachRobedFigure".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.AWonderIsntIt".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.VoiceYourAgreement".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.WizardsOfOld".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.StareInSilence".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.BringsHandTo".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.SighsABitMelancholy".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.NeverHadAnyConscience".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.BreakSilenceAndAsk".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.BroughtPeaceToVilkus".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.NodButPointOut".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.AllWorldsChange".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.LastWordGrabsAttention".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.MuchLikeLovedGrandfather".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.AskIfHeIntends".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.ReasonIAmHere".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.AskIfSuchThing".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.PurposeWorthyOfThe".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.MomentToParse".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.IfHeHoldsAny".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.PausesForASecond".Localized());

                context.Write("Scenes.WizardTower.MeetLaurentius3.FindTheOneCan".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + LaurentiusFound);

                Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => MeetLaurentius3(context));

                return;
            }

            context.Write("Scenes.WizardTower.MeetLaurentius3.ApproachMysteriousWizard".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Divinity".Localized(), () => LaurentiusChallenge(context));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => LeaveLaurentius(context));
        }

        public static void LeaveLaurentius(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.LeaveLaurentius.AskIfHeKnows".Localized());

            context.Write("Scenes.WizardTower.LeaveLaurentius.CanDoThatInvite".Localized());

            context.Write("Scenes.WizardTower.LeaveLaurentius.WaveOfHisHand".Localized());

            context.Write("Scenes.WizardTower.LeaveLaurentius.LaurentiusCanBeEncountered".Localized());

            ReturnToCamp();
        }

        public static void LaurentiusChallenge(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.LaurentiusChallenge.WouldYouFaceMe".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Yes".Localized(), () => LaurentiusChallenge2(context));

            Scenes.Scene.AddButton(1, "Common.Button.No".Localized(), () => MeetLaurentius3(context));
        }

        public static void LaurentiusChallenge2(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.LaurentiusChallenge2.TellThatYouDo".Localized());

            context.Write("Scenes.WizardTower.LaurentiusChallenge2.HopedYoudSayThat".Localized());

            context.Write("Scenes.WizardTower.LaurentiusChallenge2.TapsStaffAgainst".Localized());

            context.Write("Scenes.WizardTower.LaurentiusChallenge2.ShowMeYourWill".Localized());

            context.Write("Scenes.WizardTower.LaurentiusChallenge2.RisesUpGlowing".Localized());

            context.Write("Scenes.WizardTower.LaurentiusChallenge2.FightingAnAspect".Localized());

            CombatScene.StartMultiple(
                new[] { EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "aspect_of_laurentius")) },

                () =>
                {
                    if (Has(LaurentiusDefeated))
                        ShowLaurentiusDefeat(context);
                    else
                        ShowLaurentiusLose(context);
                },

                false);
        }

        public static void ShowLaurentiusDefeat(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.DefeatLaurentius.HitWizardAndFor".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.MoveForwardEager".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.AbstractLandscapeAround".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.YouGaveMePause".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.SmilePantingExhaustion".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.BringingMeHope".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.TellReallyDontKnow".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.CareNotMattersNot".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.TapsStaffOnThe".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.BodyDiesSoulEndures".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.SuddenlyItVanishes".Localized());

            context.Write("Scenes.WizardTower.DefeatLaurentius.NewAbilitySoulburst".Localized());

            Scenes.Scene.AwardAchievement(Achievements.AchievementIds.MeetingOfTheMinds, true, true);

            CoC.Instance.Player.CreateStatusEffect(Datas.Vanilla.StatusEffectIds.KnowsSoulburst, 0, 0, 0, 0);

            ReturnToCamp();
        }

        public static void ShowLaurentiusLose(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.LoseToLaurentius.FallIncapableContinuing".Localized());

            context.Write("Scenes.WizardTower.LoseToLaurentius.TouchOfStaff".Localized());

            context.Write("Scenes.WizardTower.LoseToLaurentius.NotReadyYet".Localized());

            context.Write("Scenes.WizardTower.LoseToLaurentius.NodPantingExhaustion".Localized());

            context.Write("Scenes.WizardTower.LoseToLaurentius.FireThatTempers".Localized());

            context.Write("Scenes.WizardTower.LoseToLaurentius.TurnsToFaceVolcanic".Localized());

            ReturnToCamp();
        }

        public static void ReturnToCamp()
        {
            DungeonSession.End();

            Scenes.Scene.View.Time.PassTime(1);

            Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
        }

        [Register("coc:wizard_tower_study_room")]
        public static void StudyRoom(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.StudyRoom.StairsLeadTo".Localized());

            context.Write("Scenes.WizardTower.StudyRoom.ArchwayOnTheBack".Localized());

            var coven = CoC.Instance.Flags.Get(GameFlagIds.CorrWitchCoven);

            var talked = (coven & (int)CorruptedCovenFlags.TalkedJeremiahWeapons) != 0;

            var brought = (coven & (int)CorruptedCovenFlags.BroughtJeremiahBack) != 0;

            if (talked && !brought)
            {
                context.Write("Scenes.WizardTower.StudyRoom.PartiallySurprisedToSee".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.OldLadyShowedUp".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.CirceBows".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.TakeGobletAndSit".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.TalkingAboutWhatever".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.SeemUnfazedByRemark".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.LiftsHerHandTowards".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.AskJeremiahWhy".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.IfTheresOneThing".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.PointOutForAll".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.HeSaysChuckling".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.CirceSighs".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.JeremiahTestedMe".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.TellHerThatThats".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.GetsUpFromChair".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.SomewhereMorePrivate".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.WavesHerHandsCreating".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.SaysClutchingHerFist".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.JeremiahSmilesToYou".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.NeitherCirceOrJeremiah".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CorrWitchCoven,

                    coven + (int)CorruptedCovenFlags.BroughtJeremiahBack);

                return;
            }

            if (brought)
            {
                context.Write("Scenes.WizardTower.StudyRoom.SquareOnFloor".Localized());

                return;
            }

            if (!Has(MetJeremiah))
            {
                context.Write("Scenes.WizardTower.StudyRoom.PeculiarWorkInProgress".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.ApproachStatue".Localized(), () => MeetJeremiah(context));

                return;
            }

            if (Has(DestroyedJeremiah) && !Has(DefeatedVilkus))
                context.Write("Scenes.WizardTower.StudyRoom.RemainsOfJeremiah".Localized());
            else if (!Has(JeremiahReformed) && Has(DefeatedVilkus))
            {
                context.Write("Scenes.WizardTower.StudyRoom.ReachStudyRoomAgain".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.BrandishYourAndLook".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.HeStaresThatIs".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.NarrowEyesAndLook".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.AboutTheWholePummeling".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.LowerYourWeapon".Localized());

                context.Write("Scenes.WizardTower.StudyRoom.JeremiahMayHaveAnswers".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + JeremiahReformed);
            }
            else
            {
                context.Write("Scenes.WizardTower.StudyRoom.StandsAtEnd".Localized());

                Scenes.Scene.AddButton(0, "Common.Button.Jeremiah".Localized(), () => MeetJeremiah(context));
            }
        }

        public static void MeetJeremiah(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            if (!Has(MetJeremiah))
            {
                context.Write("Scenes.WizardTower.MeetJeremiah.CarefullyApproachStatue".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.WhatHasBeenSculpted".Localized());

                var f = CoC.Instance.Flags;

                if (f.Get(GameFlagIds.GottenInquisitorArmor) > 0
                    || f.Get(GameFlagIds.GottenInquisitorCorset) > 0

                    || f.Get(GameFlagIds.GottenInquisitorRobes) > 0)

                    context.Write("Scenes.WizardTower.MeetJeremiah.SeenItBefore".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.MoveBackToAnalyze".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.UntilItTurnsIts".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.WellHelloThere".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.AfterInitialRecoil".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.WhatHallucinationWouldSay".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.SomethingInThatVoice".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.ConfrontHimWithInfo".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.IDidMazeYou".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.AskQuestionAgain".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.OhHohoSorry".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.RaiseABrow".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.InefficacyWasWith".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.StatueSmilesAtYou".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + MetJeremiah);
            }
            else
            {
                context.Write("Scenes.WizardTower.MeetJeremiah.ApproachHalfSculpted".Localized());

                context.Write("Scenes.WizardTower.MeetJeremiah.PersonMetBefore".Localized());
            }

            Scenes.Scene.AddButton(0, "Common.Button.Past".Localized(), () => AskQuestion(context, 0));

            var gear = CoC.Instance.Flags;

            if (gear.Get(GameFlagIds.GottenInquisitorArmor) > 0
                || gear.Get(GameFlagIds.GottenInquisitorCorset) > 0

                || gear.Get(GameFlagIds.GottenInquisitorRobes) > 0)

                Scenes.Scene.AddButton(1, "Common.Button.Heraldry".Localized(), () => AskQuestion(context, 1));
            else
                Scenes.Scene.AddButtonDisabled(1, "Heraldry", "You could talk about the heraldry on his body, if you recognized it.");

            Scenes.Scene.AddButton(2, "Common.Button.LeaveTower".Localized(), () => AskQuestion(context, 2));

            Scenes.Scene.AddButton(3, "Common.Button.Vilkus".Localized(), () => AskQuestion(context, 3));

            Scenes.Scene.AddButton(4, "Common.Button.Tower".Localized(), () => AskQuestion(context, 4));

            Scenes.Scene.AddButton(5, "Common.Button.Sentinels".Localized(), () => AskQuestion(context, 5));

            if (Has(FoundLockedDoor) && !Has(DestroyedJeremiah))
                Scenes.Scene.AddButton(6, "Common.Button.LockedRoom".Localized(), () => AskQuestion(context, 6));

            if (!Has(FoundLockedDoor) && CoC.Instance.Player.HasKeyItem("Talisman of the Flame"))
                Scenes.Scene.AddButton(7, "Common.Button.Talisman".Localized(), () => AskQuestion(context, 7));

            if (Has(LearnedKey) && !Has(DestroyedJeremiah))
                Scenes.Scene.AddButton(9, "Common.Button.Attack".Localized(), () => AttackJeremiah(context));

            if (Has(JeremiahReformed))
                Scenes.Scene.AddButton(8, "Common.Button.Maze".Localized(), () => AskQuestion(context, 8));

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => StudyRoom(context));
        }

        public static void AskQuestion(DungeonContext context, int question)
        {
            Scenes.Scene.ClearButtons();

            switch (question)
            {
                case 0:
                    context.Write("Scenes.WizardTower.AskQuestion.AskAboutHisPast".Localized());

                    if (Has(JeremiahReformed))
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.IWasAnInquisitor".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.PartiallySuccessful".Localized());
                    }
                    else
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.DidThingInPast".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ScratchYourHead".Localized());
                    }

                    break;

                case 1:
                    context.Write("Scenes.WizardTower.AskQuestion.AskAboutTheHeraldry".Localized());

                    if (Has(JeremiahReformed))
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.YouRecognizeIt".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ShakeHeadTellTomb".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SaysVisiblyDisappointed".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.LittleLeftToDo".Localized());
                    }
                    else
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.ClumsilyTurnsToFace".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.WorthATry".Localized());
                    }

                    break;

                case 2:
                    context.Write("Scenes.WizardTower.AskQuestion.AskIfHeKnows".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.WouldBeHardIndeed".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.YouShrug".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.BestWayToAgree".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + AllowedExit);

                    break;

                case 3:
                    context.Write("Scenes.WizardTower.AskQuestion.AskWhoVilkusIs".Localized());

                    if (Has(JeremiahReformed))
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.ArchInquisitorVilkus".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SighTellKillVilkus".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.AfterSeveralYearsHere".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.NodInUnderstanding".Localized());
                    }
                    else
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.VilkusSetsTheRules".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.AskWhatHeIs".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ThinkIKnewOnce".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ConvincedHeToldYou".Localized());
                    }

                    break;

                case 4:
                    context.Write("Scenes.WizardTower.AskQuestion.AskWhoBuiltTower".Localized());

                    if (Has(JeremiahReformed))
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.LaurentiusIsTheFirst".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.FromOurScholars".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.NodQuiteInteresting".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ScrapsOfWrittenHistory".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ThankHimForThe".Localized());
                    }
                    else
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.LittlePuzzleOutside".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SighAskFocus".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.FocusOnThePurpose".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ProbablyAllYouCan".Localized());
                    }

                    break;

                case 5:
                    context.Write("Scenes.WizardTower.AskQuestion.AskWhoMadeThese".Localized());

                    if (Has(JeremiahReformed))
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.ChiefArchitect".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.AskAboutSentinels".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SentinelsOldFriends".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SomeAcceptedOpportunity".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.AskWhyHeTurned".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.WhenInevitableTimeCame".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SpeculateIfItWas".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.MaybeHeBelieved".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ThenAskWhyHe".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.ProcessIsOneWay".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.SighsLooksSadFor".Localized());
                    }
                    else
                    {
                        context.Write("Scenes.WizardTower.AskQuestion.DontLikeToBrag".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.InterestingPushTheIssue".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.WhateverYouWant".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.NodAndConfirm".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.NotMuchMaterial".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.NarrowYourEyes".Localized());

                        context.Write("Scenes.WizardTower.AskQuestion.OnlyAQuestion".Localized());
                    }

                    break;

                case 6:
                    context.Write("Scenes.WizardTower.AskQuestion.AskAboutLockedDoor".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.VeryWellLocked".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.AskIfHeHas".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.IAmTheKey".Localized(((Progress & AllowedExit) == 0 ? " Here, I will open the entrance of the dungeon for you, so you can leave." : "")));

                    context.Write("Scenes.WizardTower.AskQuestion.LooksLikeNotOpening".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.WizardTowerProgress, Progress + AllowedExit + LearnedKey);

                    break;

                case 7:
                    context.Write("Scenes.WizardTower.AskQuestion.AskAboutTalisman".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.ThoughtTheseWereLost".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.AskIfTheres".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.ShapeThatTalisman".Localized());

                    Scenes.Scene.AddButton(0, "Items.Spear.Short".Localized(), () => CraftWeapon(context, 0));

                    Scenes.Scene.AddButton(1, "Scenes.Inventory.EquipmentShield".Localized(), () => CraftWeapon(context, 1));

                    Scenes.Scene.AddButton(2, "Common.Button.Ring".Localized(), () => CraftWeapon(context, 2));

                    Scenes.Scene.AddButton(14, "Common.Button.Nope".Localized(), () => MeetJeremiah(context));

                    return;

                case 8:
                    context.Write("Scenes.WizardTower.AskQuestion.AskAboutTheExtra".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.QuiteTheAmazingThing".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.YouGlareAtHim".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.ThingThatAmazesMe".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.TellHimAnotherMaze".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.AttemptsToStrokeIts".Localized());

                    context.Write("Scenes.WizardTower.AskQuestion.QuiteTheMystery".Localized());

                    break;
            }

            Scenes.Scene.AddButton(14, "Common.Button.Back".Localized(), () => MeetJeremiah(context));
        }

        public static void CraftWeapon(DungeonContext context, int which)
        {
            Scenes.Scene.ClearButtons();

            var id = which switch
            {
                0 => "coc:flameheart_spear",

                1 => "coc:flamegrit_shield",

                _ => "coc:flamespirit_ring"
            };

            context.Write(which switch
            {
                0 => "Scenes.WizardTower.CraftWeapon.GiveHimTheTalisman".Localized(),

                1 => "Scenes.WizardTower.CraftWeapon.GiveTalismanShield".Localized(),

                _ => "Scenes.WizardTower.CraftWeapon.GiveTalismanRing".Localized()
            });

            context.Write(which switch
            {
                0 => "Scenes.WizardTower.CraftWeapon.MarvelousLetWorkThird".Localized(),

                1 => "Scenes.WizardTower.CraftWeapon.MarvelousLetWorkThird".Localized(),

                _ => "Scenes.WizardTower.CraftWeapon.MarvelousLetWorkThird".Localized()
            });

            context.Write(which switch
            {
                0 => "Scenes.WizardTower.CraftWeapon.HoldsCircularTalisman".Localized(),

                1 => "Scenes.WizardTower.CraftWeapon.HoldsCircularTalismanAgain".Localized(),

                _ => "Scenes.WizardTower.CraftWeapon.HoldsCircularTalismanThird".Localized()
            });

            context.Write(which switch
            {
                0 => "Scenes.WizardTower.CraftWeapon.IMissedDoingThat".Localized(),

                1 => "Scenes.WizardTower.CraftWeapon.MissedDoingAgain".Localized(),

                _ => "Scenes.WizardTower.CraftWeapon.MissedDoingThird".Localized()
            });

            var item = Items.ItemRegistry.Create(Registry.Identifier.Parse(id));

            CoC.Instance.Player.Inventory.Add(item, 1);

            CoC.Instance.Player.RemoveKeyItem("Talisman of the Flame");

            context.Write("Scenes.WizardTower.ConfirmAcquired.TakeYourReward".Localized());

            context.Write("Scenes.WizardTower.ConfirmAcquired.DontMentionIt".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => MeetJeremiah(context));
        }

        public static void AttackJeremiah(DungeonContext context)
        {
            Scenes.Scene.ClearButtons();

            context.Write("Scenes.WizardTower.AttackJeremiah.ReadyYourAndPosition".Localized());

            context.Write("Scenes.WizardTower.AttackJeremiah.WouldntDoThat".Localized());

            context.Write("Scenes.WizardTower.AttackJeremiah.SighAndStrikeStatue".Localized());

            context.Write("Scenes.WizardTower.AttackJeremiah.MagicAlwaysUseful".Localized());

            context.Write("Scenes.WizardTower.AttackJeremiah.ConstructRaisesItsArm".Localized());

            context.Write("Scenes.WizardTower.AttackJeremiah.FightingArchitectJeremiah".Localized());

            CombatScene.StartMultiple(
                new[]
                {
                    EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "architect_jeremiah")),

                    EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "incubus_statue")),

                    EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "succubus_statue")),

                    EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "imp_statue"))
                },

                () =>
                {
                    if (Has(DestroyedJeremiah))
                        ShowJeremiahDefeat(context);
                    else
                        ShowJeremiahLose(context);
                },

                false);

            var combat = CombatController.Instance;

            if (combat != null)
                for (var i = 1; i < combat.Context.Enemies.Count; i++)
                    combat.Context.Enemies[i].Health = 0;
        }

        public static void ShowJeremiahDefeat(DungeonContext context)
        {
            context.Write("Scenes.WizardTower.DefeatJeremiah.FinalStrikeYouFinally".Localized());

            context.Write("Scenes.WizardTower.DefeatJeremiah.WhatDyingIsLike".Localized());

            context.Write("Scenes.WizardTower.DefeatJeremiah.StrikeStatueAgain".Localized());

            context.Write("Scenes.WizardTower.DefeatJeremiah.HearFaintRumbling".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () => StudyRoom(context));
        }

        public static void ShowJeremiahLose(DungeonContext context)
        {
            var player = CoC.Instance.Player;

            context.Write(player.Lust >= player.MaxLust

                ? "Scenes.WizardTower.DefeatedByJeremiah.FallOverwhelmedByDesire".Localized()

                : "Scenes.WizardTower.DefeatedByJeremiah.FallOverwhelmedInjuries".Localized());

            context.Write("Scenes.WizardTower.StatueBadEnd.NothingMuchChanges".Localized());

            context.Write("Scenes.WizardTower.StatueBadEnd.AdventurousLuckyExplorer".Localized());

            context.Write("Scenes.WizardTower.StatueBadEnd.WouldNeedSkill".Localized());

            Scenes.Scene.GameOver();
        }

        public static bool CanVisitLaurentius() => Has(LaurentiusFound) && !Has(LaurentiusDefeated);

        public static void CampLaurentius()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.WizardTower.MeetLaurentius3.ApproachMysteriousWizard".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Divinity".Localized(), () => CampLaurentiusChallenge());

            Scenes.Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scenes.Scene.Go<Scenes.Vanilla.CampScene>());
        }

        public static void CampLaurentiusChallenge()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge.WouldYouFaceMe".Localized());

            Scenes.Scene.AddButton(0, "Common.Button.Yes".Localized(), () => CampLaurentiusFight());

            Scenes.Scene.AddButton(1, "Common.Button.No".Localized(), () => CampLaurentius());
        }

        public static void CampLaurentiusFight()
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge2.TellThatYouDo".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge2.HopedYoudSayThat".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge2.TapsStaffAgainst".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge2.ShowMeYourWill".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge2.RisesUpGlowing".Localized());

            Scenes.Scene.OutputText("Scenes.WizardTower.LaurentiusChallenge2.FightingAnAspect".Localized());

            CombatScene.StartMultiple(
                new[] { EnemyRegistry.Create(new MonoCoC.Registry.Identifier("coc", "aspect_of_laurentius")) },

                () =>
                {
                    if (Has(LaurentiusDefeated))
                    {
                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.HitWizardAndFor".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.MoveForwardEager".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.AbstractLandscapeAround".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.YouGaveMePause".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.SmilePantingExhaustion".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.BringingMeHope".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.TellReallyDontKnow".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.CareNotMattersNot".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.TapsStaffOnThe".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.BodyDiesSoulEndures".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.SuddenlyItVanishes".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.DefeatLaurentius.NewAbilitySoulburst".Localized());

                        Scenes.Scene.AwardAchievement(Achievements.AchievementIds.MeetingOfTheMinds, true, true);

                        CoC.Instance.Player.CreateStatusEffect(Datas.Vanilla.StatusEffectIds.KnowsSoulburst, 0, 0, 0, 0);
                    }
                    else
                    {
                        Scenes.Scene.OutputText("Scenes.WizardTower.LoseToLaurentius.FallIncapableContinuing".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.LoseToLaurentius.TouchOfStaff".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.LoseToLaurentius.NotReadyYet".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.LoseToLaurentius.NodPantingExhaustion".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.LoseToLaurentius.FireThatTempers".Localized());

                        Scenes.Scene.OutputText("Scenes.WizardTower.LoseToLaurentius.TurnsToFaceVolcanic".Localized());
                    }

                    Scenes.Scene.AddButton(0, "Common.Button.Next".Localized(), () =>
                    {
                        Scenes.Scene.View.Time.PassTime(1);

                        Scenes.Scene.Go<Scenes.Vanilla.CampScene>();
                    });
                },

                false);
        }    }
}