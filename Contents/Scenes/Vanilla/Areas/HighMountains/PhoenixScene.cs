using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<PhoenixScene>("coc:phoenix")]
    public class PhoenixScene : Scene
    {

        public override void Entry()
        {
            EncounterPhoenix();
        }

        public void PhoenixSprite(bool nude = false)
        {

        }

        public void EncounterPhoenix()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoenixEncountered) <= 0)
                EncounterPhoenixFirstTime();
            else
                EncounterPhoenixRepeat();
        }

        public void EncounterPhoenixFirstTime()
        {
            ClearOutput();

            PhoenixSprite();

            OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.StrollingPath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.ExHarpy".Localized());
            else
                OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.EvictedMajesty".Localized());

            OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.LittleSmile".Localized((Player.IsBiped() || Player.IsTaur() ? "Common.Text.Step".Localized() : "Common.Text.Slither".Localized())));

            OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.SoundOfWings".Localized());

            OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.CautionShield".Localized());

            OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.ShieldLowers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.YouAndLizard".Localized());
            else
                OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.FinallyFoundYou".Localized());

            OutputText("Scenes.Phoenixscene.EncounterPhoenixFirstTime.NoChanceReply".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PhoenixEncountered, CoC.Instance.Flags.Get(GameFlagIds.PhoenixEncountered) + 1);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Phoenix), () => Go<CampScene>());
        }

        public void EncounterPhoenixRepeat()
        {
            ClearOutput();

            PhoenixSprite();

            OutputText("Scenes.Phoenixscene.EncounterPhoenixRepeat.StrollingPath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhoenixEncountered) == 1)
                OutputText("Scenes.Phoenixscene.EncounterPhoenixRepeat.BackAgain".Localized());
            else
                OutputText("Scenes.Phoenixscene.EncounterPhoenixRepeat.BackAgainAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PhoenixEncountered, CoC.Instance.Flags.Get(GameFlagIds.PhoenixEncountered) + 1);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.Phoenix), () => Go<CampScene>());
        }

        public void WinAgainstPhoenix()
        {
            ClearOutput();

            ClearButtons();

            CoC.Instance.Flags.Set(GameFlagIds.PhoenixHpLossCounter, 0);

            PhoenixSprite(true);

            OutputText("Scenes.Phoenixscene.WinAgainstPhoenix.FinalGrunt".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? "Common.Text.DazedLustCrazedSlutDesperatelyPulling".Localized() : "Common.Text.ABeatenBatteredHeapCompletelyUnable".Localized())));

            OutputText("Scenes.Phoenixscene.WinAgainstPhoenix.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Aiko.AikoConsSex.ButtonMissionary".Localized(), null);

            AddButton(1, "Common.Button.FuckAss".Localized(), null);

            AddButton(2, "Common.Button.GetWanked".Localized(), null);

            AddButton(3, "Common.Button.RideAnal".Localized(), null);

            AddButton(4, "Common.Button.RideVaginal".Localized(), null);

            if (Player.Lust >= 33)
            {
                if (Player.HasCock())
                {
                    if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity()))
                        AddButton(0, "Scenes.Aiko.AikoConsSex.ButtonMissionary".Localized(), MissionaryWithPhoenix);
                    else
                        AddButton(0, "Missionary", null, "Unfortunately, " + (Player.Cocks.Count == 1 ? "Common.Text.YourCockDoesnt".Localized() : "Common.Text.NoneOfYourCocks".Localized()) + " fit in her vagina.");

                    if (Player.HasCockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity()))
                        AddButton(1, "Common.Button.FuckAss".Localized(), FuckPhoenixsButt);
                    else
                        AddButton(1, "Fuck Ass", null, "Unfortunately, " + (Player.Cocks.Count == 1 ? "Common.Text.YourCockDoesnt".Localized() : "Common.Text.NoneOfYourCocks".Localized()) + " fit in her ass.");

                    AddButton(2, "Common.Button.GetWanked".Localized(), GetWanked);
                }

                AddButton(3, "Common.Button.RideAnal".Localized(), RideAnal);

                if (Player.HasVagina())
                    AddButton(4, "Common.Button.RideVaginal".Localized(), RideVaginal);
            }

            AddNextButton("Common.Button.RideFace".Localized(), RideFace);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void MissionaryWithPhoenix()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.ChargesDisarms".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.YanksHer".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? " nipples already standing at attention in the cool air" : "")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.TearsLoincloth".Localized((CurrentMonster!.Lust >= CurrentMonster!.MaxLust ? "Common.Text.SlickPussyAndHalfRigidCock".Localized() : "Common.Text.TightSlitAndSlowlyEngorgingLizard".Localized())));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.NeckCranes".Localized((Player.Cocks.Count == 1 ? "Common.Text.Member".Localized() : "Common.Text.Members".Localized())));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.ChidesHer".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.TongueDarts".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.OhReally".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.WordsCutOff".Localized((Player.Cocks.Count == 1 ? "Common.Text.CockStirs".Localized() : "Common.Text.CocksStir".Localized()), (noFur ? "Common.Text.Skin".Localized() : "Common.Text.Feathers".Localized())));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.SpreadsThighs".Localized((Player.Cocks.Count > 1 ? " and the enticingly tight ring below it" : ""), (Player.Cocks.Count == 1 ? "Common.Text.YourMemberGruntingAsIt".Localized() : "Common.Text.AllOfYourPricksGruntingAs".Localized())));

            if (Player.Cocks.Count >= 2)
                OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.TeasesPussy".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.ConstantStimulation".Localized((Player.Cocks.Count == 1 ? "Common.Text.YourLengthIsDrippingLubricationBoth".Localized() : "Common.Text.YourLengthsAreSlatheredWithA".Localized()), (Player.Cocks.Count > 1 ? " and pucker" : ""), (Player.Cocks.Count == 1 ? "Common.Text.WithHerTheTipOfYour".Localized() : "Common.Text.WithBothOfHerHolesYour".Localized()), (Player.Cocks.Count > 1 ? "Common.Text.YearningShaft".Localized() : "Common.Text.TwinPoles".Localized())));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.SlipsIntoHer".Localized((Player.Cocks.Count > 1 ? " and even tighter ass" : ""), (Player.Cocks.Count > 1 ? " and back passage" : "")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.EyesShootOpen".Localized((Player.Cocks.Count == 1 ? "Scenes.Lethicescenes.PlowHer.CockWord".Localized() : "Common.Text.Rods".Localized()), (Player.Cocks.Count > 1 ? "s" : "")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenix.PullsYouIn".Localized((noFur ? "" : ", feathered")));

            DoNext(() => MissionaryWithPhoenixPart2());
        }

        public void MissionaryWithPhoenixPart2()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.SheRelents".Localized((Player.Cocks.Count == 1 ? "Scenes.Urtascene.UrtaFuckHer.HoleWord".Localized() : "Common.Text.Holes".Localized())));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.VeryPolite".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.BegWord".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.EyesTearUp".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.HaveToDo".Localized((Player.Cocks.Count == 1 ? "Common.Text.LengthStarts".Localized() : "Common.Text.MembersStart".Localized())));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.HandsSnaking".Localized((Player.Cocks.Count == 1 ? "" : "s")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.StopsHerHand".Localized((Player.Cocks.Count == 1 ? "" : "s")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.MoansLustfully".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.RelentlessPace".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.CumsFirst".Localized((Player.Cocks.Count == 1 ? "" : ", the rapid pace of your twin shafts too much for her to bear")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.MoansInKind".Localized((Player.Cocks.Count == 1 ? "" : "and quivering sphincter"), (Player.Cocks.Count == 1 ? "" : "and [cock 2]")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.WrithingForm".Localized((Player.Cocks.Count == 1 ? "" : "s"), (Player.Cocks.Count == 1 ? "" : "and deliciously tight asshole")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.BuriesCum".Localized());

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.OrgasmFinished".Localized((Player.Cocks.Count == 1 ? "" : "s"), (Player.Cocks.Count == 1 ? "s" : ""), (Player.Cocks.Count == 1 ? "" : "s"), (Player.Cocks.Count == 1 ? "" : "s"), (Player.Cocks.Count == 1 ? "" : "s"), (Player.Cocks.Count == 1 ? "" : "and ass")));

            OutputText("Scenes.Phoenixscene.MissionaryWithPhoenixPart2.WipesCock".Localized((Player.Cocks.Count == 1 ? "" : "s"), (Player.Cocks.Count == 1 ? "Common.Text.It".Localized() : "Common.Text.Them".Localized())));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CombatScene.CleanupAfterCombat();
        }

        public void FuckPhoenixsButt()
        {
            ClearOutput();

            var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.AnalCapacity());

            if (x < 0)
                x = 0;

            OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.MindMadeUp".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.GooOpens".Localized(
                     Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.TakesOffArmor".Localized(
                     Player.CockDescript(x)
                ));

            OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.GrabsRump".Localized());

            OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.PushesIntoRear".Localized(Player.CockDescript(x), (Player.Cocks[x].Length < 12 ? "Common.Text.YouHiltIntoHer".Localized() : "Common.Text.YouCanPushInNoMore".Localized())));

            OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.SlamsHarder".Localized());

            if (Player.ArmorName == "goo armor")
                OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.GooCloses".Localized());
            else
                OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.RedressLower".Localized());

            OutputText("Scenes.Phoenixscene.FuckPhoenixsButt.SatisfiedReturns".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CombatScene.CleanupAfterCombat();
        }

        public void GetWanked()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.GetWanked.DefenselessPhoenix".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.SlidesHandDown".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.YeahWhatever".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.GripsChainShirt".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.PlaysWithBoobs".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.ReleasesChest".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.FeelsHeavenly".Localized((CoC.Instance.Flags.Get(GameFlagIds.PhoenixWankedCounter) <= 0 ? "Common.Text.GrinningSlightlyYouAskHerIf".Localized() : "Common.Text.ChucklingYouAskTheGirlIf".Localized())));

            OutputText("Scenes.Phoenixscene.GetWanked.HandSlips".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.DaydreamTeases".Localized());

            OutputText("Scenes.Phoenixscene.GetWanked.ExplodesLoad".Localized(
                 Player.CockDescript(),

                 Player.CockDescript()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.PhoenixWankedCounter, CoC.Instance.Flags.Get(GameFlagIds.PhoenixWankedCounter) + 1);

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void RideVaginal()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.RideVaginal.GrabsPhoenix".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.PhoenixWhines".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.MaintainsGrasp".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.SaysSmirking".Localized());

            if (Player.HasPerk(PerkIds.BroodMother) && Player.Pregnancy.Type.Path != "none")
                OutputText("Scenes.Phoenixscene.RideVaginal.AssuresSoon".Localized());
            else
                OutputText("Scenes.Phoenixscene.RideVaginal.GrinsAtHer".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.StripsArmor".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.NiceAndHard".Localized());

            Player.CuntChange(12, true, true, true);

            OutputText("Scenes.Phoenixscene.RideVaginal.SomewhatRecovered".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.SwingThings".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.CumsHelplessly".Localized());

            OutputText("Scenes.Phoenixscene.RideVaginal.BowLegged".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void RideAnal()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.RideAnal.GrabsPhoenix".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.StaresUpEager".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.YouNod".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.OkayThen".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.DiscardsArmor".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.CompletelyRigid".Localized());

            Player.ButtChange(12, true, true, true);

            OutputText("Scenes.Phoenixscene.RideAnal.BottomsOut".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.ShiftsHands".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.HelplessSurprise".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.BitesDown".Localized());

            OutputText("Scenes.Phoenixscene.RideAnal.ComesToSenses".Localized());

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void RideFace()
        {
            ClearOutput();

            RegisterMacro("hpWin", CombatController.Instance!.Context.Enemy.Health < 1);

            OutputText("Scenes.Phoenixscene.RideFace.FallenPhoenix".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.EyesBurn".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.HereSheLies".Localized(
                 Player.Mf("man", "lady")
            ));

            OutputText("Scenes.Phoenixscene.RideFace.NeedsPresentable".Localized((Player.HasChildren() ? "Common.Text.YourDaughter".Localized() : "Common.Text.TheDaughterYouveNeverHad".Localized())));

            OutputText("Scenes.Phoenixscene.RideFace.OnlySecond".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.ArroganceFades".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.StopsWaiting".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.FinallySpeaks".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.NotThatMatters".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.WhimpersPitifully".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.WatchesIntently".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.NoNeedRush".Localized());

            OutputText("Scenes.Phoenixscene.RideFace.GlassyEyes".Localized((Player.HasPerk(PerkIds.HistoryFighter) ? "Common.Text.AFellowWarrior".Localized() : "Common.Text.SomeoneSoArrogant".Localized())));

            OutputText("Scenes.Phoenixscene.RideFace.FireWithin".Localized());

            DoNext(() => RideFace2());
        }

        public void RideFace2()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.RideFace2.MouthOpens".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.JustHerWay".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.CallousedHands".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.SettlesCore".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.DipsLower".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.PerhapsKnows".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.NeedsItMore".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.TailThumps".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.NoHoldingBack".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.DoesntResist".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.GloatingSmile".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.SomethingBrushes".Localized());

            OutputText("Scenes.Phoenixscene.RideFace2.MuchReluctance".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToPhoenix()
        {
            ClearOutput();

            PhoenixSprite(true);

            if (Player.Health < 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.PhoenixHpLossCounter, CoC.Instance.Flags.Get(GameFlagIds.PhoenixHpLossCounter) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.PhoenixHpLossCounter) >= 4)
                {
                    PhoenixBadEnd();

                    return ;
                }
            }

            OutputText("Scenes.Phoenixscene.LoseToPhoenix.CollapsesGround".Localized((Player.Lust >= Player.MaxLust ? "Scenes.Heldungeon.PhoenixPlatoonMurdersPC.TurnedOnWord".Localized() : "Common.Text.Drained".Localized())));

            OutputText("Scenes.Phoenixscene.LoseToPhoenix.GraspsChin".Localized());

            if (Player.HasCock() && Random.Shared.Next(2) == 0)
                DoNext(() => CockwielderLossScenePart1());
            else
                DoNext(() => GetRapedInTheAss());
        }

        public void CockwielderLossScenePart1()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart1.ShovesFoot".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart1.ForcesEyeOpen".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart1.WordsOutMouth".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart1.HammersPoint".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart1.TeasingTandem".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart1.BreathingHeavy".Localized((Player.Balls > 0 ? "over your swollen [sack], your body shuddering at the sensation, before it spills " : "")));

            DoNext(() => CockwielderLossScenePart2());
        }

        public void CockwielderLossScenePart2()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.ProddingSensation".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.ActuallyGettingOff".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.StrangledCry".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.SeedSeeps".Localized());

            Player.ButtChange(30, true, true, true);

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.FlicksProstate".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.InsultsHer".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.HipsAbove".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.DesperateRelease".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.SidewaysSmile".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.ClawsGround".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.BreathlessVoice".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.OwnOrgasm".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.GripsHerCock".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.HerOrgasmEnds".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.LipsRemove".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart2.RollsHips".Localized());

            Player.Orgasm("Dick");

            DoNext(() => CockwielderLossScenePart3());
        }

        public void CockwielderLossScenePart3()
        {
            ClearOutput();

            CombatScene.CleanupAfterCombat();

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.ComesToSenses".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.FilthEncrusted".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.LooksAround".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.CleansCrotch".Localized((Player.Body.Get<BodyParts.Vanilla.TailPart>().Id != BodyPartTypes.None ? ", tailbase" : "")));

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.GingerlySteps".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.TooAfraidLook".Localized());

            OutputText("Scenes.Phoenixscene.CockwielderLossScenePart3.TooPained".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -2f));

            Player.ApplyStats(new DynStat(GameStats.Toughness, -2f));

            DoNext(() => ReturnToCamp(4));
        }

        public void GetRapedInTheAss()
        {
            ClearOutput();

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.ShovesDown".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.PullsOffArmor".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.StandsFront".Localized());

            if (Player.Anal.Looseness == 0)
                OutputText("Scenes.Phoenixscene.GetRapedInTheAss.VirginAss".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.GetReady".Localized());

            Player.ButtChange(12, true, true, true);

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.DeserveThis".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.HardSpank".Localized());

            if (OptionStore.Instance.Get("watersports", 0) > 0 && Random.Shared.Next(2) == 0)
                OutputText("Scenes.Phoenixscene.GetRapedInTheAss.PullsOut".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.FinalUnload".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.TriggersYours".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Phoenixscene.GetRapedInTheAss.ShootsJism".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Phoenixscene.GetRapedInTheAss.WhileAlso".Localized());

            if (Player.HasVagina())
                if (Player.AverageVaginalWetness() < 4)
                    OutputText("Scenes.Phoenixscene.GetRapedInTheAss.LeakGirlcum".Localized());
                else
                    OutputText("Scenes.Phoenixscene.GetRapedInTheAss.SquirtLubricant".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Phoenixscene.GetRapedInTheAss.BodySpasms".Localized());

            OutputText("Scenes.Phoenixscene.GetRapedInTheAss.LearnedLesson".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f));

            Player.Orgasm("Anal");

            CombatScene.CleanupAfterCombat();
        }

        public void PhoenixBadEnd()
        {
            ClearOutput();

            PhoenixSprite();

            OutputText("Scenes.Phoenixscene.PhoenixBadEnd.SeriouslyBeaten".Localized());

            OutputText("Scenes.Phoenixscene.PhoenixBadEnd.HarpyAvenged".Localized());

            OutputText("Scenes.Phoenixscene.PhoenixBadEnd.LastWords".Localized());

            if (silly)
                OutputText("Common.Text.NNDataExpunged".Localized());

            GameOver();
        }

    }
}