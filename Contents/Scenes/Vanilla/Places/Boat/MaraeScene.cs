using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Encounters;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Boat
{
    [Register<MaraeScene>("coc:marae_scene")]
    public class MaraeScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Boat.Marae);

        public string debugName => "Marae";

        public string debugHint => "";

        public override void Entry()
        {

            EncounterObject.Execute(new EncounterContext
            {
                Player = Player,

                Flags = CoC.Instance.Flags,

                Time = View.Time,

                Random = Random.Shared,

                Write = OutputText,

                ReturnToCamp = () => ReturnToCamp(1),

                ReturnToCampAfter = ReturnToCamp
            });
        }

        public void Reset()
        {

            Save.SetBool("feraMet", false);
        }

        public void EncounterMarae()
        {
            ClearOutput();

            ShowImage("marae-first-encounter");

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MetMarae) <= 0)
                {
                    OutputText("Scenes.Maraescene.EncounterMarae.HiddenEmeraldJewel".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MetMarae, 1);

                    OutputText("Scenes.Maraescene.EncounterMarae.ApproachTheTree".Localized());

                    if (!Player.IsPureEnough(66))
                        OutputText("Scenes.Maraescene.EncounterMarae.WomanBellows".Localized());
                    else
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.MaraeQuestStart, 1);

                        OutputText("Scenes.Maraescene.EncounterMarae.SaysTheTreeAgain".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.StepBackAmazed".Localized());

                        if (Player.IsCorruptEnough(80 - Player.LibidoPercent))
                            OutputText("Scenes.Maraescene.EncounterMarae.EyesCantHelpBut".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.BitConfusedPerhaps".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.SheRepliesAgain".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.HadToGuess".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.NodsAndContinues".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.NotWhereThe".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.NodInUnderstanding".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.SighsHeavily".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.TremblingPracticallyBegs".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.WhatYouWereSent".Localized());

                        if (Player.IsCorruptEnough(80 - Player.Libido))
                        {
                            OutputText("Scenes.Maraescene.EncounterMarae.CouldLeaveButDesire".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Boob".Localized(), GrabHerBoob);

                            SetExitButton("Common.Button.Back".Localized());
                        }
                        else
                            DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Maraescene.EncounterMarae.GlidingAcrossWaters".Localized(
                        CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2
                    ));

                    OutputText("Scenes.Maraescene.EncounterMarae.GoddessFlowsOut".Localized());

                    if (!Player.IsPureEnough(66))
                    {
                        OutputText("Scenes.Maraescene.EncounterMarae.BellowsInRage".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.TurnTailHeadBack".Localized());

                        DoNext(() => ReturnToCamp(1));
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.MaraeQuestStart) >= 1)
                        {
                            OutputText("Scenes.Maraescene.EncounterMarae.MaraeRemindsYou".Localized());

                            DoNext(() => ReturnToCamp(1));
                        }
                        else
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.MaraeQuestStart, 1);

                            OutputText("Scenes.Maraescene.EncounterMarae.SaysTheTreeAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.StepBackAmazedAgain".Localized());

                            if (Player.IsCorruptEnough(80 - Player.LibidoPercent))
                                OutputText("Scenes.Maraescene.EncounterMarae.EyesCantHelpAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.BitConfusedPerhapsAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.SheRepliesAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.HadToGuessAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.NodsAndContinuesAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.NotWhereTheAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.NodInUnderstandingAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.SighsHeavilyAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.TremblingPracticallyBegsAgain".Localized());

                            OutputText("Scenes.Maraescene.EncounterMarae.SentHereAgain".Localized());

                            if (Player.IsCorruptEnough(80 - Player.Libido))
                            {
                                OutputText("Scenes.Maraescene.EncounterMarae.LeaveButDesire".Localized());

                                ClearButtons();

                                AddButton(0, "Common.Button.Boob".Localized(), GrabHerBoob);

                                AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
                            }
                            else
                                DoNext(() => ReturnToCamp(1));
                        }
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                {
                    OutputText("Scenes.Maraescene.EncounterMarae.SmilesBroadlyAtYou".Localized());

                    OutputText("Scenes.Maraescene.EncounterMarae.BreakingTheHug".Localized());

                    OutputText("Scenes.Maraescene.EncounterMarae.PlungesHandInside".Localized());

                    OutputText("Scenes.Maraescene.EncounterMarae.PushesPearlInto".Localized());

                    InventoryScene.TakeItem(UseableIds.PurePearl, () => ReturnToCamp(1));

                    CoC.Instance.Flags.Set(GameFlagIds.MaraeQuestComplete, 1);
                }
                else
                {
                    OutputText("Scenes.Maraescene.EncounterMarae.ApproachMaraesTree".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.MetMaraeCorrupted) <= 0)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.MetMaraeCorrupted, 1);

                        OutputText("Scenes.Maraescene.EncounterMarae.GivesSultryStare".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.BodyRelaxesExplains".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.SaysAsSheBegins".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.SpellboundWatch".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.ShudderAtChange".Localized());

                        OutputText("Scenes.Maraescene.EncounterMarae.SoundsLikeVeryPleasant".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Run".Localized(), RunFromPervertedGoddess);

                        AddButton(1, "Items.Lethicite.Short".Localized(), () => MaraeStealLethicite());

                        AddButton(2, "Common.Button.Accept".Localized(), MaraeBadEnd);

                        AddButton(3, "Common.Button.Prank".Localized(), () => MaraeStealLethicite(true));

                        AddButton(4, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => PromptFightMarae(EncounterMarae));
                    }
                    else
                    {
                        OutputText("Scenes.Maraescene.EncounterMarae.LeansForwardsCupping".Localized());

                        AddButton(0, "Common.Button.Yes".Localized(), MaraeBadEnd);

                        AddButton(1, "Common.Button.No".Localized(), RunFromPervertedGoddess);

                        AddButton(3, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => PromptFightMarae(EncounterMarae));
                    }
                }
        }

        public void PromptFightMarae(Action cancelFunction)
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.PromptFightMarae.SureYouWantFight".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), InitiateFightMarae);

            AddButton(1, "Common.Button.No".Localized(), cancelFunction);
        }

        public void InitiateFightMarae()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptMaraeFollowupEncounterState) == 2)
                OutputText("Scenes.Maraescene.InitiateFightMarae.MindFinallyMadeUp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
            {
                OutputText("Scenes.Maraescene.InitiateFightMarae.ReadyYourAndAssume".Localized());

                OutputText("Scenes.Maraescene.InitiateFightMarae.TentaclesComeUp".Localized());
            }
            else
                OutputText("Scenes.Maraescene.InitiateFightMarae.MaraeLooksAtYou".Localized());

            OutputText("Scenes.Maraescene.InitiateFightMarae.ItsAFight".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "marae")), () => Go<CampScene>(), canFlee: false);
        }

        public void LoseAgainstMarae()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
            {
                if (Player.Health <= 0)
                    OutputText("Scenes.Maraescene.LoseAgainstMarae.CollapseTooWeak".Localized());
                else
                    OutputText("Scenes.Maraescene.LoseAgainstMarae.DesireToKeepFighting".Localized());

                DoNext(MaraeBadEnd);
            }
            else
            {
                if (Player.Health <= 0)
                    OutputText("Scenes.Maraescene.LoseAgainstMarae.CollapseTooWeakAgain".Localized());
                else
                    OutputText("Scenes.Maraescene.LoseAgainstMarae.DesireKeepFighting".Localized());

                CombatScene.CleanupAfterCombat();
            }
        }

        public void WinAgainstMarae()
        {
            ClearOutput();

            ShowImage("marae-defeated");

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
            {
                OutputText("Scenes.Maraescene.WinAgainstMarae.YellsNothingBut".Localized());

                if (silly)
                    OutputText("Scenes.Maraescene.WinAgainstMarae.DefeatedAncientTentacle".Localized());

                if (Player.Weapon.IsHolySword())
                    OutputText("Scenes.Maraescene.WinAgainstMarae.RightingWrongs".Localized());
                else
                    if (Player.Weapon.Id == ItemIds.LethicesWhip)
                        OutputText("Scenes.Maraescene.WinAgainstMarae.LashingAtFallenGoddess".Localized());
                    else
                        if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire) ||                                                             Player.HasPerk(PerkIds.FireLord) || Player.HasPerk(PerkIds.Hellfire))
                            OutputText("Scenes.Maraescene.WinAgainstMarae.ConjuringFlames".Localized());
                        else
                            if (Player.Weapon.IsFist())
                            {
                                if (silly)
                                {
                                    OutputText("Scenes.Maraescene.WinAgainstMarae.UnleashingFlurry".Localized());

                                    OutputText("Scenes.Maraescene.WinAgainstMarae.ExclaimsMomentsLater".Localized());
                                }
                                else
                                    OutputText("Scenes.Maraescene.WinAgainstMarae.SoWeakenedThe".Localized());
                            }
                            else
                                if (Player.Weapon.IsAxe())
                                    OutputText("Scenes.Maraescene.WinAgainstMarae.RusticWayOfAn".Localized());
                                else
                                    OutputText("Scenes.Maraescene.WinAgainstMarae.SeverelyWeakenedNow".Localized());

                if (!Player.HasKeyItem("Marae's Lethicite"))
                {
                    OutputText("Scenes.Maraescene.WinAgainstMarae.FallenShortWayOff".Localized());

                    Player.CreateKeyItem("Marae's Lethicite", 3, 0, 0, 0);
                }

                OutputText("Scenes.Maraescene.WinAgainstMarae.AfterLongerExamination".Localized());

                OutputText("Scenes.Maraescene.WinAgainstMarae.TentacledBarkPlates".Localized());

                Player.CreateKeyItem("Tentacled Bark Plates", 0, 0, 0, 0);

                OutputText("Scenes.Maraescene.WinAgainstMarae.TentaclesBlockingBoat".Localized());

                AwardAchievement(AchievementIds.GeneralGodslayer, true, true);

                CoC.Instance.Flags.Set(GameFlagIds.CorruptedMaraeKilled, 1);

                Player.UpgradeBeautifulSword();

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                if (CombatController.Instance!.Context.Enemy.Health <= 0)
                    OutputText("Scenes.Maraescene.WinAgainstMarae.ReelsBackIncredible".Localized());
                else
                    OutputText("Scenes.Maraescene.WinAgainstMarae.ClearlyShowsSigns".Localized());

                OutputText("Scenes.Maraescene.WinAgainstMarae.MaraeSays".Localized());

                OutputText("Scenes.Maraescene.WinAgainstMarae.DeityShedsLayer".Localized());

                OutputText("Scenes.Maraescene.WinAgainstMarae.PickUpTheBark".Localized());

                OutputText("Scenes.Maraescene.WinAgainstMarae.DivineBarkPlates".Localized());

                Player.UpgradeBeautifulSword();

                Player.CreateKeyItem("Divine Bark Plates", 0, 0, 0, 0);

                AwardAchievement(AchievementIds.GeneralGodslayer, true, true);

                CoC.Instance.Flags.Set(GameFlagIds.PureMaraeEndgame, 2);

                CombatScene.CleanupAfterCombat();
            }
        }

        public void MaraeBadEnd()
        {
            ClearOutput();

            ShowImage("marae-bad-end");

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMaraeCorrupted) <= 0)
                OutputText("Scenes.Maraescene.MaraeBadEnd.GoddessFlowsOut".Localized());

            OutputText("Scenes.Maraescene.MaraeBadEnd.ApproachesBreastsSwinging".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Maraescene.MaraeBadEnd.PausesInConfusion".Localized());

            OutputText("Scenes.Maraescene.MaraeBadEnd.GigglesPressesFace".Localized());

            OutputText("Scenes.Maraescene.MaraeBadEnd.SapInsideMakesYour".Localized());

            if (SceneInstance<Npcs.MarbleScene>.Get().KnowAddiction > 0)
                OutputText("Scenes.Maraescene.MaraeBadEnd.AnyThoughtToDrink".Localized());

            OutputText("Scenes.Maraescene.MaraeBadEnd.FoggyEuphoriaFloats".Localized());

            if (Player.Cocks.Count == 0)
            {
                OutputText("Scenes.Maraescene.MaraeBadEnd.BuildingPressureGrows".Localized());

                Player.CreateCock();

                Player.Cocks[0].Length = 36;

                Player.Cocks[0].Thickness = 2;

                Player.Cocks[0].Id = new Identifier("coc", "tentacle");
            }
            else
            {
                OutputText("Scenes.Maraescene.MaraeBadEnd.DrinkDeeplySuckling".Localized(
                    Player.MultiCockDescriptLight()
                ));

                if (Player.CountCocksOfType("TENTACLE") < Player.CockTotal())
                {
                    if (Player.Cocks.Count == 1)
                        Player.Cocks[0].Id = new Identifier("coc", "tentacle");

                    if (Player.CockTotal() > 1)
                    {
                        var temp = Player.Cocks.Count;

                        while (temp > 0)
                        {
                            temp--;

                            Player.Cocks[temp].Id = new Identifier("coc", "tentacle");
                        }
                    }
                }

                OutputText("Scenes.Maraescene.MaraeBadEnd.SeeYourWavingAround".Localized());
            }

            OutputText("Scenes.Maraescene.MaraeBadEnd.WhispersThrowingYou".Localized(
                Player.MultiCockDescript()
            ));

            OutputText("Scenes.Maraescene.MaraeBadEnd.DontReallyKnowWhat".Localized());

            OutputText("Scenes.Maraescene.MaraeBadEnd.SomeTimePasses".Localized());

            OutputText("Scenes.Maraescene.MaraeBadEnd.StillOnTheIsland".Localized());
            // 原版 MaraeScene.hx:363 — AikoScene.hx:1889 / FactoryScene.hx 等：Marble 是跟随者
            if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                OutputText("Scenes.Maraescene.MaraeBadEnd.MarbleLooksOut".Localized());

            GameOver();
        }

        public void MaraeStealLethicite(bool deliberate = false)
        {
            ClearOutput();

            if ((Player.Speed > 35 && (Random.Shared.Next((int)(Player.Speed / 3 + 30)) > 20) || Player.Speed > 35 && Player.HasPerk(PerkIds.Evade) && Random.Shared.Next(3) < 2) && !deliberate)
            {
                OutputText("Scenes.Maraescene.MaraeStealLethicite.DartToTheSide".Localized());

                Player.CreateKeyItem("Marae's Lethicite", 3, 0, 0, 0);

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                Player.SlimeFeed();

                Player.Orgasm("Generic");

                if (!deliberate)
                    OutputText("Scenes.Maraescene.MaraeStealLethicite.DartToSideAgain".Localized());
                else
                    OutputText("Scenes.Maraescene.MaraeStealLethicite.MischievousIdea".Localized());

                OutputText("Scenes.Maraescene.MaraeStealLethicite.YouTurnMeInto".Localized());

                OutputText("Scenes.Maraescene.MaraeStealLethicite.SnappingHerFingers".Localized());

                OutputText("Scenes.Maraescene.MaraeStealLethicite.TentaclesLowerYou".Localized());

                OutputText("Scenes.Maraescene.MaraeStealLethicite.StepsCloserPlaying".Localized());

                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 1))
                {
                    OutputText("Scenes.Maraescene.MaraeStealLethicite.ExtendsHandExpectantly".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Maraescene.MaraeStealLethicite.LargestWord".Localized());

                    OutputText(Player.CockDescript(0));

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.TipImmediatelySuction".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.WhispersCuppingYour".Localized());

                    if (Player.Balls > 0)
                        OutputText(Player.BallsDescriptLight());
                    else
                        if (Player.Vaginas.Count > 0)
                            OutputText(Player.VaginaDescript(0));
                        else
                            OutputText("Scenes.Maraescene.MaraeStealLethicite.CrotchWord".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.SharpPointOfFingernail".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.FeelWarmEnclosed".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.StepsIntoFieldOf".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.ApologizesTurnsAway".Localized(
                        Player.MultiCockDescript()
                    ));

                    if (!Player.HasPerk(PerkIds.MaraesGiftStud))
                    {
                        OutputText("Scenes.Maraescene.MaraeStealLethicite.NewPerkMaraes".Localized());

                        Player.CreatePerk(PerkIds.MaraesGiftStud, 0, 0, 0, 0);
                    }

                    DoNext(() => ReturnToCamp(2));
                }
                else
                {
                    OutputText("Scenes.Maraescene.MaraeStealLethicite.ExtendsHandExpectantlyAgain".Localized());

                    if (Player.Gender == Gender.Genderless)
                    {
                        OutputText("Scenes.Maraescene.MaraeStealLethicite.HairlessCrotchHolds".Localized());

                        Player.CreateVagina();
                    }
                    else
                        OutputText("Scenes.Maraescene.MaraeStealLethicite.VaginaDescriptionPeriod".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.GuidesTentacleForwards".Localized());

                    Player.CuntChange(20, true, true, false);

                    if (!Player.HasPerk(PerkIds.MaraesGiftFertility))
                    {
                        OutputText("Scenes.Maraescene.MaraeStealLethicite.NewPerkMaraesAgain".Localized());

                        Player.CreatePerk(PerkIds.MaraesGiftFertility, 0, 0, 0, 0);
                    }

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.MaraeWinks".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.GigglesAtExpression".Localized());

                    OutputText("Scenes.Maraescene.MaraeStealLethicite.DroppedFromTree".Localized(
                        Player.VaginaDescript(0)
                    ));

                    DoNext(() => ReturnToCamp(1));
                }
            }
        }

        public void Level2MaraeEncounter()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.CorruptMaraeFollowupEncounterState, 1);

            ShowImage("marae-second-encounter");

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.WhileRowingTheLake".Localized());

            if (Player.Strength > 70)
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.WithEaseWord".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.FamiliarIslandResolves".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.KnowingAllTooWell".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Maraescene.Level2MaraeEncounter.KnowingTooWell".Localized());
                else
                    OutputText("Scenes.Maraescene.Level2MaraeEncounter.WorriedAboutEnjoying".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.DipYourOars".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.SquirmingRootsCrawling".Localized());

            if (Player.Speed < 50)
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.DroppingFlatOnAss".Localized());
            else
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.NearlyDroppingFlat".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.OldDinghyCutting".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.AdjustYourWeapon".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.StepOutOfBoat".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.AdvanceOnArboreal".Localized());

            if (Player.HasKeyItem("Marae's Lethicite"))
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.WelcomeBackSneakThief".Localized(
                    Player.Mf("gentleman", "lady")
                ));
            else
                if (Player.HasPerk(PerkIds.MaraesGiftStud) || Player.HasPerk(PerkIds.MaraesGiftFertility))
                    OutputText("Scenes.Maraescene.Level2MaraeEncounter.DidntThinkId".Localized());
                else
                    OutputText("Scenes.Maraescene.Level2MaraeEncounter.LookWhoCameBack".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.ParagraphBreak".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.FeaturelessWhiteIrises".Localized());

            if (Player.HasPerk(PerkIds.MaraesGiftFertility) || Player.HasPerk(PerkIds.MaraesGiftStud))
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.SecondWord".Localized());

            OutputText("Scenes.Maraescene.Level2MaraeEncounter.DoseOfTenderAffections".Localized());

            DoNext(() => Go<CampScene>());

            if (!Player.CanFly())
            {
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.DontSeeAnyEscape".Localized());

                DoNext(MaraeIIStageII);

                AddButton(3, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => PromptFightMarae(Level2MaraeEncounter));
            }
            else
            {
                OutputText("Scenes.Maraescene.Level2MaraeEncounter.DontThinkShes".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), MaraeIIStageII);

                AddButton(3, "Scenes.ErlKing.RepeatWildHuntCaught.ButtonFight".Localized(), () => PromptFightMarae(Level2MaraeEncounter));

                AddButton(4, "Common.Button.FlyAway".Localized(), MaraeIIFlyAway);
            }
        }

        public void MaraeIIStageII()
        {
            ClearOutput();

            ShowImage("marae-second-encounter-pt-two");

            CoC.Instance.Flags.Set(GameFlagIds.CorruptMaraeFollowupEncounterState, 2);

            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Maraescene.MaraeIIStageII.CoosWithPleasure".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.ParalyzedByWord".Localized());

                if (Player.LustPercent > 80)
                    OutputText("Scenes.Maraescene.MaraeIIStageII.LustWord".Localized());
                else
                    if (Player.Corruption > 50)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.IndecisionWord".Localized());
                    else
                        OutputText("Scenes.Maraescene.MaraeIIStageII.WorryWord".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.DontManageSingle".Localized());

                if (Player.Tallness > 48)
                    OutputText("Scenes.Maraescene.MaraeIIStageII.AtYourCrotch".Localized());
                else
                    OutputText("Scenes.Maraescene.MaraeIIStageII.AgainstYourBelly".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.SapDroolingTeats".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.HeartHammersChest".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.RunsFingersThrough".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.BobbingBackAndForth".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Maraescene.MaraeIIStageII.TentacleSquirtsSlippery".Localized(
                    Player.VaginaDescript(0)
                ));

                Player.CuntChange(12, true, true, false);

                OutputText("Scenes.Maraescene.MaraeIIStageII.BothDrippingGiving".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.GruntsAndPullsOn".Localized());

                if (Player.Horns.Id.Path != "none")
                    OutputText("Scenes.Maraescene.MaraeIIStageII.YourHorns".Localized());
                else
                    OutputText("Scenes.Maraescene.MaraeIIStageII.YourHair".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.ShovingThickClitCock".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Maraescene.MaraeIIStageII.StruggleToWord".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.WorkToWord".Localized());
                    else
                        OutputText("Scenes.Maraescene.MaraeIIStageII.EasilyWord".Localized());

                OutputText("Scenes.Maraescene.MaraeIIStageII.SuppressGagReflex".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Maraescene.MaraeIIStageII.GoddessPullsBack".Localized());
            }
            else
                if (Player.Gender == Gender.Male)
                {
                    OutputText("Scenes.Maraescene.MaraeIIStageII.CoosWithPleasureAgain".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.WatchStandingStill".Localized());

                    if (Player.LustPercent > 80)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.LustWordAgain".Localized());
                    else
                        if (Player.Corruption > 50)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.IndecisionWordAgain".Localized());
                        else
                            OutputText("Scenes.Maraescene.MaraeIIStageII.WorryWordAgain".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.AdvanceSeemsLikeAn".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.VinesWhipOut".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.WhileOtherDangles".Localized());
                    else
                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.WhileOthersDangle".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.GoddessGivesCruel".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.ExtendsHerArms".Localized(
                        Player.SMultiCockDesc()
                    ));

                    OutputText("Scenes.Maraescene.MaraeIIStageII.TentacleOnRightArm".Localized());

                    Player.ButtChange(12, true, true, false);

                    OutputText("Scenes.Maraescene.MaraeIIStageII.OpenPlantHoleDives".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.AnotherVineThatMay".Localized(
                            Player.CockDescript(1)
                        ));
                    else
                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.MoreOpenVines".Localized(
                                Appearance.CockNoun(BodyPartTypes.Human)
                            ));

                    OutputText("Scenes.Maraescene.MaraeIIStageII.GruntAndPumpHips".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.PluralS".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.CumBoilsOutFrom".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.BodyWord".Localized());
                    else
                        OutputText("Scenes.Maraescene.MaraeIIStageII.BallsWord".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.QuicklyDevouredBy".Localized());

                    if (Player.CumQ() > 500)
                    {
                        OutputText("Scenes.Maraescene.MaraeIIStageII.ThickBulgesOfSpooge".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.PluralSAgain".Localized());

                        OutputText("Scenes.Maraescene.MaraeIIStageII.BarestGlimpse".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.PluralSThird".Localized());

                        OutputText("Scenes.Maraescene.MaraeIIStageII.SentencePeriodSpace".Localized());
                    }

                    OutputText("Scenes.Maraescene.MaraeIIStageII.HotSlipperyPressure".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.DoesntMatterYoure".Localized());
                }
                else
                {
                    OutputText("Scenes.Maraescene.MaraeIIStageII.CoosWithPleasureThird".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.ClosesDeliberatelySlow".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Maraescene.MaraeIIStageII.SlippingInsideCunt".Localized());
                    else
                        OutputText("Scenes.Maraescene.MaraeIIStageII.SentencePeriod".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.HammeringInYourChest".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.WhileOtherCockRubs".Localized());
                    else
                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.WhileOtherCocksRubs".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.PeriodBreak".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.SwoonHeadBuzzing".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.EasyToPushDown".Localized());

                    if (Player.Cocks[0].Thickness > 5 || Player.CockArea(0) > 100)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.HowBigYouAre".Localized());
                    else
                        OutputText("Scenes.Maraescene.MaraeIIStageII.FeelsPerfectVelvetVice".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.AnotherWord".Localized());
                        else
                            OutputText("Scenes.Maraescene.MaraeIIStageII.YourOtherWord".Localized());

                        OutputText("Scenes.Maraescene.MaraeIIStageII.ProdsAtTightPucker".Localized(
                            Appearance.CockNoun(BodyPartTypes.Human)
                        ));
                    }

                    OutputText("Scenes.Maraescene.MaraeIIStageII.ParagraphBreak".Localized());

                    if (Player.Vaginas[0].Wetness < 2)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.StickyWetnessGlistens".Localized());
                    else
                        if (Player.Vaginas[0].Wetness < 4)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.DropsOfArousal".Localized());
                        else
                            OutputText("Scenes.Maraescene.MaraeIIStageII.TrailsOfViscousFluid".Localized(
                                Player.VaginaDescript(0)
                            ));

                    OutputText("Scenes.Maraescene.MaraeIIStageII.RemindingUnusedFemsex".Localized());

                    Player.CuntChange(12, true, true, false);

                    Player.ButtChange(12, true, true, false);

                    OutputText("Scenes.Maraescene.MaraeIIStageII.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.LaughsAndTeases".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Maraescene.MaraeIIStageII.HappilyThrustForwards".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.AssholeSquelching".Localized(
                            Player.CockDescript(1)
                        ));

                    OutputText("Scenes.Maraescene.MaraeIIStageII.InSpiteOfObscene".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Maraescene.MaraeIIStageII.CumBoilsOutOf".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.RapidlyContractingBalls".Localized());
                    else
                        OutputText("Scenes.Maraescene.MaraeIIStageII.TentacleSqueezedProstate".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.EruptsIntoWomb".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.HerAssSpasms".Localized(
                            Player.CockDescript(1)
                        ));

                    if (Player.CockTotal() > 2)
                    {
                        OutputText("Scenes.Maraescene.MaraeIIStageII.NeglectedButOrgasming".Localized());

                        if (Player.CockTotal() > 3)
                            OutputText("Scenes.Maraescene.MaraeIIStageII.RemainderOfWord".Localized());

                        OutputText("Scenes.Maraescene.MaraeIIStageII.YourWord".Localized());

                        if (Player.CockTotal() > 3)
                            OutputText(Player.MultiCockDescriptLight());
                        else
                            OutputText(Player.CockDescript(2));

                        OutputText("Scenes.Maraescene.MaraeIIStageII.DoesBestToCoat".Localized());
                    }

                    OutputText("Scenes.Maraescene.MaraeIIStageII.GoopFromInside".Localized());

                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.OrgasmSoMuchMore".Localized());

                    if (Player.CumQ() > 1000)
                        OutputText("Scenes.Maraescene.MaraeIIStageII.ActuallySurprisedWhen".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.TentaclesNeverLetUp".Localized());

                    OutputText("Scenes.Maraescene.MaraeIIStageII.SlideOutAndSlump".Localized());
                }

            DoNext(MaraePt2RoundIIIPrizes);
        }

        public void MaraePt2RoundIIIPrizes()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.AwakenInMidstOf".Localized());

                if (Player.HasPerk(PerkIds.MaraesGiftStud))
                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.GiftWorkingOutWell".Localized());
                else
                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.MightBeLittleSore".Localized());

                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.SpeechBrokenByPauses".Localized());

                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.BreathlessGiveMarae".Localized());

                if (Player.HasPerk(PerkIds.MaraesGiftStud) && !Player.HasPerk(PerkIds.MaraesGiftProfractory))
                {
                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraes".Localized());

                    Player.CreatePerk(PerkIds.MaraesGiftProfractory, 0, 0, 0, 0);
                }
                else
                    if (!Player.HasPerk(PerkIds.MaraesGiftStud))
                    {
                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesAgain".Localized());

                        Player.CreatePerk(PerkIds.MaraesGiftStud, 0, 0, 0, 0);
                    }
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.AwakenMidstAgain".Localized());

                    if (Player.HasPerk(PerkIds.MaraesGiftFertility))
                    {
                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.PrizeBreeder".Localized());

                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.EyesWidenInShock".Localized());

                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.MaraeConfirms".Localized());
                    }
                    else
                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.WombMakeover".Localized());

                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.EntireTimeSpeaking".Localized());

                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.BreathlessGiveMaraeAgain".Localized());

                    if (Player.HasPerk(PerkIds.MaraesGiftFertility) && !Player.HasPerk(PerkIds.MaraesGiftButtslut))
                    {
                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesThird".Localized());

                        Player.CreatePerk(PerkIds.MaraesGiftButtslut, 0, 0, 0, 0);

                        Player.Anal.Wetness = 2;
                    }
                    else
                        if (!Player.HasPerk(PerkIds.MaraesGiftFertility))
                        {
                            OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesFourth".Localized());

                            Player.CreatePerk(PerkIds.MaraesGiftFertility, 0, 0, 0, 0);
                        }
                }
                else
                {
                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.AwakenMidstThird".Localized());

                    if (!Player.HasPerk(PerkIds.MaraesGiftFertility) && !Player.HasPerk(PerkIds.MaraesGiftStud))
                    {
                        if (Random.Shared.Next(2) == 0 && !Player.HasPerk(PerkIds.MaraesGiftFertility))
                        {
                            OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.WombMakeoverAgain".Localized());

                            Player.CreatePerk(PerkIds.MaraesGiftFertility, 0, 0, 0, 0);
                        }
                        else
                            if (!Player.HasPerk(PerkIds.MaraesGiftStud))
                            {
                                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.SoreDidSomeWork".Localized());

                                Player.CreatePerk(PerkIds.MaraesGiftStud, 0, 0, 0, 0);
                            }

                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.EntireTimeSpeakingAgain".Localized());

                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.BreathlessGiveMaraeThird".Localized());

                        if (Player.HasPerk(PerkIds.MaraesGiftFertility))
                            OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesFifth".Localized());
                        else
                            if (Player.HasPerk(PerkIds.MaraesGiftStud))
                                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesSixth".Localized());
                    }
                    else
                        if (Player.HasPerk(PerkIds.MaraesGiftFertility) && Player.HasPerk(PerkIds.MaraesGiftStud))
                        {
                            if (Player.HasPerk(PerkIds.MaraesGiftStud) && !Player.HasPerk(PerkIds.MaraesGiftProfractory))
                            {
                                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesSeventh".Localized());

                                Player.CreatePerk(PerkIds.MaraesGiftProfractory, 0, 0, 0, 0);
                            }
                            else
                                if (Player.HasPerk(PerkIds.MaraesGiftFertility) && !Player.HasPerk(PerkIds.MaraesGiftButtslut))
                                {
                                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesEighth".Localized());

                                    Player.CreatePerk(PerkIds.MaraesGiftButtslut, 0, 0, 0, 0);
                                }
                        }
                        else
                            if (Player.HasPerk(PerkIds.MaraesGiftFertility))
                            {
                                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.DidntThinkThis".Localized());

                                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.EntireTimeSpeakingThird".Localized());

                                OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.BreathlessGiveMaraeFourth".Localized());

                                if (!Player.HasPerk(PerkIds.MaraesGiftStud))
                                {
                                    Player.CreatePerk(PerkIds.MaraesGiftStud, 0, 0, 0, 0);

                                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesNinth".Localized());
                                }
                            }
                            else
                                if (Player.HasPerk(PerkIds.MaraesGiftStud))
                                {
                                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.DidntThinkOfThis".Localized());

                                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.EntireTimeSpeakingFourth".Localized());

                                    OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.BreathlessGiveMaraeFifth".Localized());

                                    if (!Player.HasPerk(PerkIds.MaraesGiftFertility))
                                    {
                                        Player.CreatePerk(PerkIds.MaraesGiftFertility, 0, 0, 0, 0);

                                        OutputText("Scenes.Maraescene.MaraePt2RoundIIIPrizes.NewPerkMaraesTenth".Localized());
                                    }
                                }
                }

            Player.Orgasm("All");

            DoNext(() => ReturnToCamp(2));
        }

        public void MaraeIIFlyAway()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.MaraeIIFlyAway.LaunchIntoTheAir".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void Level3MaraeEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.Level3MaraeEncounter.OnceAgainApproachIsland".Localized());

            OutputText("Scenes.Maraescene.Level3MaraeEncounter.FightOrStay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FightHer".Localized(), () => PromptFightMarae(Level3MaraeEncounter));

            AddButton(1, "Common.Button.StayWithHer".Localized(), MaraeBadEnd);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void GrabHerBoob()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.GrabHerBoob.ReachForwardToCop".Localized());

            Player.HealthChange(-Player.Health - 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void RunFromPervertedGoddess()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.RunFromPervertedGoddess.TurnRunForBoat".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToMaraeAboutMinervaPurification()
        {

            OutputText("Scenes.Maraescene.TalkToMaraeAboutMinervaPurification.StepIntoTheBoat".Localized());

            OutputText("Scenes.Maraescene.TalkToMaraeAboutMinervaPurification.DeityGentlyAsks".Localized());

            OutputText("Scenes.Maraescene.TalkToMaraeAboutMinervaPurification.GestureOfRespect".Localized());

            OutputText("Scenes.Maraescene.TalkToMaraeAboutMinervaPurification.ExpressionFalls".Localized());

            OutputText("Scenes.Maraescene.TalkToMaraeAboutMinervaPurification.PuzzledRepeatDescription".Localized());

            OutputText("Scenes.Maraescene.TalkToMaraeAboutMinervaPurification.ThankForAssistance".Localized());

            Player.CreateKeyItem("Marae's Seed", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationProgress, 3);

            CoC.Instance.Flags.Set(GameFlagIds.MinervaPurificationMaraeTalked, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void EncounterPureMaraeEndgame()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.EncounterPureMaraeEndgame.StepIntoTheBoat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PureMaraeEndgame) == 0)
            {
                OutputText("Scenes.Maraescene.EncounterPureMaraeEndgame.DeityGentlyAsks".Localized());

                OutputText("Scenes.Maraescene.EncounterPureMaraeEndgame.LetMaraeKnow".Localized());

                OutputText("Scenes.Maraescene.EncounterPureMaraeEndgame.SheSays".Localized());
            }
            else
                OutputText("Scenes.Maraescene.EncounterPureMaraeEndgame.DeityGentlyAsksAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PureMaraeEndgame, 1);

            ClearButtons();

            AddButton(0, "Common.Button.BringItOn".Localized(), InitiateFightMarae);

            AddButton(1, "Common.Button.NotYet".Localized(), () => ReturnToCamp(1));
        }

        public void FeraMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.FeraMeeting.RowingAcrossWatersAgain".Localized());

            OutputText("Scenes.Maraescene.FeraMeeting.SteppingFromBoat".Localized());

            Save.SetBool("feraMet", true);

            DoNext(FeraMeeting2);
        }

        public void FeraMeeting2()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.FeraMeeting2.SpotDesecratedHusk".Localized());

            OutputText("Scenes.Maraescene.FeraMeeting2.FeraSays".Localized());

            OutputText("Scenes.Maraescene.FeraMeeting2.GoddessOfPredationTurns".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Talk".Localized(), FeraMeetingTalk);

            AddNextButton("Common.Button.Leave".Localized(), FeraMeetingLeave);
        }

        public void FeraMeetingTalk()
        {
            ClearOutput();

            OutputText("Scenes.Maraescene.FeraMeetingTalk.GiveYourCondolences".Localized());

            OutputText("Scenes.Maraescene.FeraMeetingTalk.FeraSighs".Localized());

            OutputText("Scenes.Maraescene.FeraMeetingTalk.JoiningByHerSide".Localized());

            OutputText("Scenes.Maraescene.FeraMeetingTalk.BrightGreenVinesSprout".Localized());

            OutputText("Scenes.Maraescene.FeraMeetingTalk.PullsHandBack".Localized());

            OutputText("Scenes.Maraescene.FeraMeetingTalk.WalksAwayHeading".Localized((View.Time.Hours > 20 ? "moon" : "day")));

            DoNext(() => ReturnToCamp(1));
        }

        public void FeraMeetingLeave()
        {

            OutputText("Scenes.Maraescene.FeraMeetingLeave.WouldBeForBest".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public bool TimeChange()
        {
            if (View.Time.Hours == 0)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.CorruptMaraeFollowupEncounterState) > 0) && (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerKilled) == 0
                        && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) < 4

                        && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) < 1000))

                        CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter,

                            CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.HolliFuckedToday) == 1)
                    CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 0);
            }

            return false;
        }

        public bool TimeChangeLarge() => false;

        public static IEncounter? _encounterObject;

        public static IEncounter EncounterObject => _encounterObject ??= BuildEncounterObject();

        public bool FeraMet => Save.GetBool("feraMet");

        public static IEncounter BuildEncounterObject()
        {
            var flags = CoC.Instance.Flags;

            var softLevelMin30 = Player.Level >= 30 || View.Time.Days >= 30 * 6;

            return IEncounter.Group("marae",

                IEncounter.Create("default", _ => 1f,

                    _ => flags.Get(GameFlagIds.MaraeQuestComplete) <= 0

                        && flags.Get(GameFlagIds.MetMaraeCorrupted) <= 0,

                    _ => SceneInstance<MaraeScene>.Get().EncounterMarae()),

                IEncounter.Create("feraMeeting", _ => 1f,

                    _ => flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0

                        && (flags.Get(GameFlagIds.AkbalQuestStatus) & (int)AkbalEvent.QuestDone) > 0

                        && !SceneInstance<MaraeScene>.Get().FeraMet,

                    _ => SceneInstance<MaraeScene>.Get().FeraMeeting()),

                IEncounter.Create("aboutMinerva", _ => 1f,

                    _ => flags.Get(GameFlagIds.FactoryShutdown) == 1

                        && flags.Get(GameFlagIds.MaraeQuestComplete) >= 1

                        && flags.Get(GameFlagIds.MinervaPurificationMaraeTalked) == 1,

                    _ => SceneInstance<MaraeScene>.Get().TalkToMaraeAboutMinervaPurification()),

                IEncounter.Create("endgamePure", _ => 1f,

                    _ => flags.Get(GameFlagIds.FactoryShutdown) == 1

                        && flags.Get(GameFlagIds.MaraeQuestComplete) >= 1

                        && flags.Get(GameFlagIds.MinervaPurificationMaraeTalked) != 1

                        && flags.Get(GameFlagIds.LethiceDefeated) > 0

                        && flags.Get(GameFlagIds.PureMaraeEndgame) < 2

                        && softLevelMin30,

                    _ => SceneInstance<MaraeScene>.Get().EncounterPureMaraeEndgame()),

                IEncounter.Create("level2", _ => 1f,

                    p => flags.Get(GameFlagIds.CorruptMaraeFollowupEncounterState) == 0

                        && flags.Get(GameFlagIds.MetMaraeCorrupted) > 0

                        && p.Gender > 0

                        && flags.Get(GameFlagIds.CorruptedMaraeKilled) <= 0,

                    _ => SceneInstance<MaraeScene>.Get().Level2MaraeEncounter()),

                IEncounter.Create("level3", _ => 1f,

                    p => flags.Get(GameFlagIds.CorruptMaraeFollowupEncounterState) == 2

                        && flags.Get(GameFlagIds.MetMaraeCorrupted) > 0

                        && p.Gender > 0

                        && flags.Get(GameFlagIds.CorruptedMaraeKilled) <= 0,

                    _ => SceneInstance<MaraeScene>.Get().Level3MaraeEncounter()));
        }

    }
}