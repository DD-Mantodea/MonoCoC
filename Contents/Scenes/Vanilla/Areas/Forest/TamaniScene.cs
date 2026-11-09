using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Npcs;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Forest
{
    [Register<TamaniScene>("coc:tamani")]
    public class TamaniScene : Scene, INpc
    {

        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Tamani)!;

        public NpcState State => Player.GetNpcState(Definition);

        public void Talk() => ExecEncounter();

        public static SaveContent Save => SaveContent(SceneIds.Forest.Tamani);

        public string debugName => "Tamani";

        public string debugHint => "";

        public override void Entry()
        {

            if (CombatScene.LastEnemy is TamaniEnemy && CombatScene.LastOutcome is { } outcome && outcome != CombatOutcome.Fled)
            {

                CombatScene.LastOutcome = null;

                if (outcome is CombatOutcome.HpVictory or CombatOutcome.LustVictory)
                {
                    TamaniVictoryMenu();

                    return;
                }

                TamaniCombatLoss(outcome == CombatOutcome.HpDefeat);

                return;
            }

            ExecEncounter();
        }

        public void TamaniCombatLoss(bool hpVictory)
        {
            if (Player.Cocks.Count == 0)
            {
                OutputText(hpVictory

                    ? "Scenes.Tamani.Won.NoFunPartsHP".Localized()

                    : "Scenes.Tamani.Won.NoFunPartsLust".Localized());

                if (!hpVictory)
                    Player.HealthChange(-9999);

                CombatScene.CleanupAfterCombat();

                return;
            }

            if (hpVictory)
            {
                if (Random.Shared.Next(2) == 0)
                    TamaniSexLost();
                else
                    TamaniSexLetHer(true);

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 19 && Random.Shared.Next(2) == 0)
                GetRapedByTamaniYouHypnoSlut(true);
            else if (Random.Shared.Next(2) == 0)
                TamaniSexLost();
            else
                TamaniSexLetHer(true);
        }

        public void Reset()
        {

            Save.SetInt("timesRidden", 0);
        }

        public void ExecEncounter()
        {

            if (Player.Cocks.Count > 0 && CoC.Instance.Flags.Get(GameFlagIds.TamaniDaughterPreggoCountdown) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) >= 24)
                SceneInstance<TamanisDaughtersScene>.Get().EncounterTamanisDaughters();
            else
                EncounterTamani();
        }

        public void TamaniGivesBirth() => ApplyTamaniBirth(State);

        public static void ApplyTamaniBirth(NpcState state)
        {
            var flags = CoC.Instance.Flags;

            if (state.PregnancyType == Pregnancy.PregnancyIds.PlayerFather)
            {
                flags.Set(GameFlagIds.TamaniNumberOfDaughters,

                    flags.Get(GameFlagIds.TamaniNumberOfDaughters) + flags.Get(GameFlagIds.TamaniPregnancyCount));

                flags.Set(GameFlagIds.TamaniPregnancyCount, 0);

                flags.Set(GameFlagIds.TamaniTimesImpregnated, flags.Get(GameFlagIds.TamaniTimesImpregnated) + 1);
            }

            NpcPregnancy.Clear(state);
        }

        [Register("coc:tamani_birth")]
        public static void TamaniBirthTick(NpcState state) => ApplyTamaniBirth(state);

        public string TamaniChest()
        {

            var descript = "";

            var __sw0_0 = CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesImpregnated);

            if (__sw0_0 == -1 || __sw0_0 == 0)
                descript = (Random.Shared.Next(2) == 0 ? "ample " : "handful of ");
            else if (__sw0_0 == 1 || __sw0_0 == 2 || __sw0_0 == 3)
                descript = (Random.Shared.Next(2) == 0 ? "large " : "jiggling ");
            else if (__sw0_0 == 4 || __sw0_0 == 5 || __sw0_0 == 6)
                descript = (Random.Shared.Next(2) == 0 ? "wobbling " : "impressive ");
            else if (__sw0_0 == 7 || __sw0_0 == 8 || __sw0_0 == 9 || __sw0_0 == 10)
                descript = (Random.Shared.Next(2) == 0 ? "over-proportioned " : "super-sized ");
            else
                descript = (Random.Shared.Next(2) == 0 ? "massive " : "gigantic ");

            var __sw1_0 = Random.Shared.Next(11);

            if (__sw1_0 == 0 || __sw1_0 == 1 || __sw1_0 == 2)
                return descript + "tits";

            if (__sw1_0 == 3)
                return descript + "melons";

            if (__sw1_0 == 4)
                return descript + "mounds";

            if (__sw1_0 == 5)
                return descript + "muffins";

            if (__sw1_0 == 6)
                return descript + "knockers";

            if (__sw1_0 == 7)
                return descript + "love-pillows";

            return descript + "breasts";
        }

        public void TamaniFemaleEncounter()
        {
            ClearOutput();

            ShowImage("tamani-encounter");

            OutputText("Scenes.Tamani.TamaniFemaleEncounter.GoblinLeapsOut".Localized(
                 TamaniChest()
            ));

            OutputText("Scenes.Tamani.TamaniFemaleEncounter.OnlySoMuchCock".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Tamani.TamaniFemaleEncounter.ButtonUmmOk".Localized(), TamaniFemaleYes);

            AddButton(1, "Common.Button.No".Localized(), TamaniFemaleNo);

            AddButton(2, "Scenes.Tamani.TamaniFemaleEncounter.ButtonPreferGirls".Localized(), PreferTamaniFemdom);
        }

        public void TamaniFemaleYes()
        {
            ClearOutput();

            ShowImage("tamani-dDildo");

            OutputText("Scenes.Tamani.TamaniFemaleYes.ThatsWhatIThought".Localized());

            OutputText("Scenes.Tamani.TamaniFemaleYes.TossesDildo".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Tamani.TamaniFemaleYes.Gross".Localized());
            else
                OutputText("Scenes.Tamani.TamaniFemaleYes.CockSlapped".Localized());

            OutputText("Scenes.Tamani.TamaniFemaleYes.BeCarefulWarning".Localized());

            ShowImage("item-dDildo");

            OutputText("Scenes.Tamani.TamaniFemaleYes.DeluxeDildoAcquired".Localized());

            Player.CreateKeyItem("Deluxe Dildo", 0, 0, 0, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniFemaleNo()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniFemaleNo.Harrumphs".Localized());

            OutputText("Scenes.Tamani.TamaniFemaleNo.FlouncesOff".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Tamani.TamaniFemaleNo.WhatASlut".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Tamani.TamaniFemaleNo.HowOdd".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniFemaleNo.HopeMissesFew".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniMaleFirstEncounter()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TamaniMet, 1);

            ShowImage("tamani-encounter");

            OutputText("Scenes.Tamani.TamaniMaleFirstEncounter.GoblinLeapsOut".Localized());

            OutputText("Scenes.Tamani.TamaniMaleFirstEncounter.OfferHardToRefuse".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), TamaniFirstTimeConsentual);

            AddButton(1, "Common.Button.Refuse".Localized(), TamaniFirstTimeRefusal);
        }

        public void TamaniFirstTimeConsentual()
        {
            ClearOutput();

            TamaniKnockUp();

            ShowImage("tamani-fuck");

            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.CantBelieveFortune".Localized());

            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.HipsSway".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText(Appearance.AllBreastsDescript(Player));
            else
                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.Chest".Localized());

            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.WrapsArmsKiss".Localized());

            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.BreaksKissSmiles".Localized());

            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.CurlsToes".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.BallsSwellSeed".Localized());
            else
                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.SomethingSwells".Localized());

            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.NeedToFuckNow".Localized());

            if (Player.CockArea(0) <= 90)
            {
                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.BreaksKissShimmies".Localized(
                     Player.CockHead()
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.AndDripsBalls".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.ReadyToStuff".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.DoesntWait".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.SlamHerDown".Localized());

                if (Player.CockArea(0) > 30)
                    OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.BellyBulge".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.TwistsThrashing".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.GoodThingsEnd".Localized(
                     Player.Mf("stud", "hun")
                ));

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.PutArmorBack".Localized());

                if (Player.Corruption > 66)
                    OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.CockTwitches".Localized());
            }
            else
            {
                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.BreaksSloppyKiss".Localized());

                if (Player.Cocks.Exists(c => c.SheathOn))
                {
                    OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.LicksSheath".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.AndBalls".Localized());
                }
                else
                    if (Player.Balls > 0)
                        OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.LicksBalls".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.SneaksBetweenFolds".Localized());
                        else
                            OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.LicksInnerThighs".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.ParagraphBreak".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.CuntGrinds".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.FeelsItPass".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.BodyDistends".Localized());

                if (Player.Balls > 0 && Player.HoursSinceCum > 200)
                    OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.DrainingBalls".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.DoesHerBestCatch".Localized());

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.ShakeLastDrops".Localized(
                     Player.Mf("stud", "hun")
                ));

                OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.PutArmorBackAgain".Localized());

                if (Player.Corruption > 66)
                    OutputText("Scenes.Tamani.TamaniFirstTimeConsentual.CockTwitchesAgain".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, .5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniFirstTimeRefusal()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniFirstTimeRefusal.EyesWiden".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), TamaniFirstTimeConsentual);

            AddButton(1, "Common.Button.No".Localized(), TamaniSecondRefusal);

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
        }

        public void TamaniSecondRefusal()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniSecondRefusal.PoutsAnger".Localized());

            OutputText("Scenes.Tamani.TamaniSecondRefusal.ElipsisWhat".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniMaleRepeatEncounter()
        {
            ClearOutput();

            ShowImage("tamani-encounter");

            if (State.IsPregnant || CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 0)
                OutputText("Scenes.Tamani.TamaniMaleRepeatEncounter.TinyHandsStartle".Localized());
            else
                OutputText("Scenes.Tamani.TamaniMaleRepeatEncounter.TinyHandsStartleAgain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesHypnotised) > 19 && Random.Shared.Next(2) == 0)
            {
                GetRapedByTamaniYouHypnoSlut();

                return;
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Sensitivity / 10)));

            ClearButtons();

            AddButton(0, "Scenes.Tamani.TamaniMaleRepeatEncounter.ButtonTakeHer".Localized(), () => TamaniSexWon(false));

            AddButton(1, "Common.Button.LetHer".Localized(), () => TamaniSexLetHer());

            AddButton(2, "Common.Button.No".Localized(), TamaniStartFight);
        }

        public void TamaniStartFight()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniStartFight.FightingPose".Localized());

            var tamani = EnemyRegistry.Create(EnemyIds.Tamani);

            tamani.OnDefeated = _ => TamaniVictoryMenu();

            tamani.OnWon = _ => TamaniSexLost();

            CombatScene.Start(tamani, () => Go<CampScene>());
        }

        public void TamaniSexLetHer(bool fromCombat = false)
        {
            ClearOutput();

            TamaniKnockUp();

            ShowImage("tamani-letHer-fuck");

            var inCombat = fromCombat || CombatController.InCombat;

            if (inCombat)
            {
                if (Player.IsTaur())
                {
                    if (Player.Health < 1)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.StumbleDrunkenly".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.StumbleDrunkenlyAgain".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.SidlesUpPats".Localized());
                }
                else
                {
                    if (Player.Health < 1)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.CollapseUnable".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.TearOffClothes".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.WigglesHer".Localized(
                         TamaniChest()
                    ));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.ErectionEmerges".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.ErectionsSprout".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.StripsYouDown".Localized());
                }
            }
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Tamani.TamaniSexLetHer.MeeklyUndress".Localized());
                else
                {
                    OutputText("Scenes.Tamani.TamaniSexLetHer.MeeklyUndressAgain".Localized());

                    var temp = Random.Shared.Next(3);

                    switch (temp)
                    {
                        case 0:
                        {
                        OutputText(Player.CockDescript(0));
                        }

                        break;

                        case 1:
                        {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.Bellybutton".Localized());
                        }

                        break;

                        case 2:
                        {
                        OutputText(Player.NippleDescript(0));
                        }

                        break;
                    }

                    OutputText("Scenes.Tamani.TamaniSexLetHer.QuickKissShove".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.JustLieThere".Localized(
                         Player.Mf("stud", "slut")
                    ));
                }

            if (Player.IsTaur())
            {
                OutputText("Scenes.Tamani.TamaniSexLetHer.HiddenByYourForm".Localized(
                     Player.CockDescript(0)
                ));

                OutputText("Scenes.Tamani.TamaniSexLetHer.SuchArousal".Localized());

                if (Player.Cocks[0].Length <= 18)
                {
                    OutputText("Scenes.Tamani.TamaniSexLetHer.StopsShifts".Localized(
                         Player.CockDescript(0)
                    ));

                    if (Player.Balls == 0)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.WonderingWhere".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.WonderingWhereAgain".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.HasHerWay".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.GoOnThen".Localized(
                         Player.Mf("stud", "slut")
                    ));

                    OutputText("Scenes.Tamani.TamaniSexLetHer.NoFurtherInvitation".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.FinishedRelief".Localized());
                }
                else
                {
                    OutputText("Scenes.Tamani.TamaniSexLetHer.StopsShiftsAgain".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.WonderingWhereThird".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.WonderingWhereFourth".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.HasHerWayAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.AndBalls".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.SuckingTonguing".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.GoOnThenAgain".Localized(
                         Player.Mf("stud", "slut")
                    ));

                    if (Player.CumQ() < 1200)
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.RaggedSighPeak".Localized());

                        OutputText("Scenes.Tamani.TamaniSexLetHer.FinishedReliefAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.BellowingRoar".Localized());

                        OutputText("Scenes.Tamani.TamaniSexLetHer.ComeToSenses".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Tamani.TamaniSexLetHer.BallsAcheThirsty".Localized());
                        else
                            OutputText("Scenes.Tamani.TamaniSexLetHer.VeryThirsty".Localized());

                        OutputText("Scenes.Tamani.TamaniSexLetHer.OnlyWhenYouManage".Localized());

                        OutputText("Scenes.Tamani.TamaniSexLetHer.WheeeGrin".Localized());
                    }
                }

                OutputText("Scenes.Tamani.TamaniSexLetHer.RedressScent".Localized());

                if (inCombat)
                {
                    OutputText("Scenes.Tamani.TamaniSexLetHer.AfterStressStrain".Localized());

                    CombatScene.CleanupAfterCombat();

                    Player.Orgasm("Dick");
                }
                else
                {
                    Player.Orgasm("Dick");

                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                    ReturnToCamp(1);
                }
            }
            else
            {
                OutputText("Scenes.Tamani.TamaniSexLetHer.DownOnAllFours".Localized());

                if (Player.CockArea(0) <= 90)
                {
                    OutputText("Scenes.Tamani.TamaniSexLetHer.TurnsAboutDrags".Localized(
                         Player.CockDescript(0),

                         Player.CockHead()
                    ));

                    if (Player.Cocks[0].Length >= 12)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.BodyStretches".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.MoaningSlipsDown".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.GaspPinches".Localized());

                    if (Player.TotalNipples() == 2)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.Both".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.All".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.OfNipplesEases".Localized());

                    if (Player.BiggestBreastSize() >= 2)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.BounceJiggle".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.NipplesBurn".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.PlaysLikeFiddle".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.AllEncouragement".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.BallsTwitchVent".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.MusclesTwitchJet".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.WrigglesTweaks".Localized());

                    if (Player.CumQ() >= 250)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.PressureBuilds".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.FulfillsRequests".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.OnlyFewSpurts".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.ColdAirDraft".Localized());

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.EnjoyOurFucks".Localized(
                             Player.Mf("stud", "hun")
                        ));
                    else
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.MakeSureAndLet".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Tamani.TamaniSexLetHer.BallsFillBack".Localized());
                        else
                            OutputText("Scenes.Tamani.TamaniSexLetHer.CockFillsBack".Localized());
                    }

                    OutputText("Scenes.Tamani.TamaniSexLetHer.SayEndBreak".Localized());

                    if (inCombat)
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.BlackOutExhausted".Localized());

                        CombatScene.CleanupAfterCombat();

                        Player.Orgasm("Dick");
                    }
                    else
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.LieThereRecovering".Localized());

                        Player.Orgasm("Dick");

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        ReturnToCamp(1);
                    }
                }
                else
                {
                    OutputText("Scenes.Tamani.TamaniSexLetHer.TurnsAboutDragsAgain".Localized());

                    if (Player.Balls > 0)
                        OutputText(Player.BallsDescriptLight());
                    else
                        OutputText(Player.CockDescript(0));

                    OutputText("Scenes.Tamani.TamaniSexLetHer.RoughSlapWhimper".Localized(
                         Player.CockHead()
                    ));

                    OutputText("Scenes.Tamani.TamaniSexLetHer.TooBigForMe".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.PoutProtesting".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.LiftsYourBalls".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.LeansLookingBetween".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.PlacesDildoTip".Localized());

                    if (Player.HasVagina())
                    {
                        Player.CuntChange(Player.VaginalCapacity() * .95f, true);

                        OutputText("Scenes.Tamani.TamaniSexLetHer.WallsSqueeze".Localized());
                    }
                    else
                    {
                        Player.ButtChange(Player.AnalCapacity() * .95f, true);

                        OutputText("Scenes.Tamani.TamaniSexLetHer.BodyClamps".Localized());
                    }

                    OutputText("Scenes.Tamani.TamaniSexLetHer.BurningHeat".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.RocksBackForth".Localized(
                         Player.CockDescript(0)
                    ));

                    OutputText("Scenes.Tamani.TamaniSexLetHer.DominatrixLeans".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Tamani.TamaniSexLetHer.CockBase".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.Prostate".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.DeliciousIntensity".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Tamani.TamaniSexLetHer.Balls".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniSexLetHer.Crotch".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.LetOutSpunk".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.BodyComplies".Localized());

                    if (Player.CumQ() >= 250)
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.CumHarderHarder".Localized());

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Tamani.TamaniSexLetHer.KeepCummingSplatter".Localized());

                        OutputText("Scenes.Tamani.TamaniSexLetHer.Period".Localized());
                    }

                    OutputText("Scenes.Tamani.TamaniSexLetHer.DildoSlipsOut".Localized());

                    OutputText("Scenes.Tamani.TamaniSexLetHer.EasesOffDripping".Localized());

                    if (inCombat)
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.BlackOutExhaustedAgain".Localized());

                        CombatScene.CleanupAfterCombat();

                        Player.Orgasm("Dick");

                        Player.Orgasm("VaginalAnal");
                    }
                    else
                    {
                        OutputText("Scenes.Tamani.TamaniSexLetHer.LieThereRecoveringAgain".Localized());

                        Player.Orgasm("Dick");

                        Player.Orgasm("VaginalAnal");

                        Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                        ReturnToCamp(1);
                    }
                }
            }
        }

        public void TamaniSexLost()
        {
            ClearOutput();

            TamaniKnockUp();

            OutputText("Scenes.Tamani.TamaniSexLost.KnowingSmile".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Tamani.TamaniSexLost.LieUnableToFight".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.TouchYourselfBreed".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.AdvancesBareFeet".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.NaughtyBoyExplains".Localized());

            if (Player.Cocks[0].Thickness >= 5)
                OutputText("Scenes.Tamani.TamaniSexLost.RingExpands".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.RingFitsPerfectly".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.DontFretOffers".Localized(
                 Player.Mf("stud", "hun")
            ));

            OutputText("Scenes.Tamani.TamaniSexLost.ForcesRingDown".Localized());

            if (Player.BiggestBreastSize() >= 6)
                OutputText("Scenes.Tamani.TamaniSexLost.CantSeePast".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.BloatedPrick".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.SmackHand".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.SpankingContinues".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.PoursOil".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.YourGoblinMistress".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Tamani.TamaniSexLost.CupsBallsTightly".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.CaressesRing".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.AndAsksSorry".Localized(
                 Player.Mf("stud", "slut")
            ));

            ShowImage("tamani-loss-fuck");

            OutputText("Scenes.Tamani.TamaniSexLost.NeedToCumHer".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Tamani.TamaniSexLost.BallsSwollenTender".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.BodyAchesBloated".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Tamani.TamaniSexLost.NoChoice".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.DeniedKindOfFun".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.BegMistress".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Tamani.TamaniSexLost.BlushingHotly".Localized());
            else
                OutputText("Scenes.Tamani.TamaniSexLost.BlushingShame".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.PressesBottle".Localized(
                 Player.CockHead()
            ));

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Tamani.TamaniSexLost.BottleOverflows".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Tamani.TamaniSexLost.CoosHappily".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.ParagraphBreak".Localized());

            OutputText("Scenes.Tamani.TamaniSexLost.RemovesRing".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, .5f));

            CombatScene.CleanupAfterCombat();
        }

        public void TamaniSexWon(bool postFight = true)
        {
            ClearOutput();

            TamaniKnockUp();

            int x = Player.CockThatFits(90);

            if (x == -1)
                x = Player.BiggestCockIndex();

            ShowImage("tamani-win-fuck");

            if (Player.CockArea(x) <= 90)
            {
                if (postFight)
                    OutputText("Scenes.Tamani.TamaniSexWon.GrabInsensate".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniSexWon.GrabInsensateAgain".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.TooTemptingTarget".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Tamani.TamaniSexWon.AndTightAsshole".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.DontAllowSay".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Tamani.TamaniSexWon.EachOf".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.YourGirth".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Tamani.TamaniSexWon.PluralS".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.SquealsHappily".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Tamani.TamaniSexWon.PluralSAgain".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.ClenchingSqueezing".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.RockBackForth".Localized());

                if (Player.Gender == Gender.Male)
                    OutputText("Scenes.Tamani.TamaniSexWon.Man".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniSexWon.Herm".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.SawingInOut".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Tamani.TamaniSexWon.PussySqueezing".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniSexWon.HolesSqueezing".Localized());

                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Tamani.TamaniSexWon.EachOfYourPulsates".Localized());

                    if (Player.CumQ() >= 250)
                        OutputText("Scenes.Tamani.TamaniSexWon.PantsWatchingBelly".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Tamani.TamaniSexWon.RiverOfJism".Localized());
                }
                else
                {
                    OutputText("Scenes.Tamani.TamaniSexWon.YourCocksSpasm".Localized());

                    if (Player.CumQ() >= 250)
                        OutputText("Scenes.Tamani.TamaniSexWon.PantsWatchingBellyAgain".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Tamani.TamaniSexWon.RiverOfJismAgain".Localized());
                }

                OutputText("Scenes.Tamani.TamaniSexWon.WinksSucksFinger".Localized(
                     Player.Mf("sexy lover", "stud")
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Tamani.TamaniSexWon.SexyBallsHaveAgain".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniSexWon.SweetCockHas".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.AnyMoreCream".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.DoesntGiveChance".Localized());

                Player.Orgasm("Dick");

                if (postFight || CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Tamani.TamaniSexWon.ThrowsOnBack".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Tamani.TamaniSexWon.AnyMassiveMembers".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniSexWon.YourMassiveMember".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.GrabsByAnkles".Localized(
                     TamaniChest()
                ));

                OutputText("Scenes.Tamani.TamaniSexWon.FeetGrowSlick".Localized(
                     Player.Mf("stud", "slut")
                ));

                OutputText("Scenes.Tamani.TamaniSexWon.OhGods".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.LicksLipsPushes".Localized());

                if (Player.CumQ() >= 250)
                    OutputText("Scenes.Tamani.TamaniSexWon.WorkingWithFeet".Localized(
                         Player.CockDescript(x)
                    ));

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Tamani.TamaniSexWon.Sputters".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.ParagraphBreak".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.WinksBlinkingCum".Localized(
                     Player.Mf("stud", "sexy")
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Tamani.TamaniSexWon.SexyBallsHaveAgain".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniSexWon.SweetCockHasAgain".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.HasAnyMoreCream".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.DoesntGiveChanceAgain".Localized());

                OutputText("Scenes.Tamani.TamaniSexWon.WigglesLeaving".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Lust, 35f));

                if (postFight || CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
        }

        public void TamaniPregnantEncounter()
        {
            ClearOutput();

            ShowImage("tamani-preggo-encounter");

            OutputText("Scenes.Tamani.TamaniPregnantEncounter.StrollsOutPregnant".Localized());

            OutputText("Scenes.Tamani.TamaniPregnantEncounter.PartsLegsBegs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), TamaniPregnantFuck);

            AddButton(1, "Common.Button.Refuse".Localized(), TamaniPregnantRefusal);
        }

        public void TamaniPregnantRefusal()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniPregnantRefusal.BurstsIntoTears".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniPregnantFuck()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniPregnantFuck.EyesLightUpPounce".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText(Appearance.AllBreastsDescript(Player));
            else
                OutputText("Scenes.Tamani.TamaniPregnantFuck.Chest".Localized());

            OutputText("Scenes.Tamani.TamaniPregnantFuck.PeriodBreak".Localized());

            OutputText("Scenes.Tamani.TamaniPregnantFuck.LiftHerUp".Localized(
                 TamaniChest()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniTimesImpregnated) > 6)
                OutputText("Scenes.Tamani.TamaniPregnantFuck.AlmostTooBig".Localized());

            OutputText("Scenes.Tamani.TamaniPregnantFuck.ParagraphBreak".Localized());

            OutputText("Scenes.Tamani.TamaniPregnantFuck.TiredOfTeasing".Localized());

            OutputText("Scenes.Tamani.TamaniPregnantFuck.BellyRubsAlong".Localized());

            ShowImage("tamani-preggo-fuck");

            if (Player.CockArea(0) <= 50)
            {
                OutputText("Scenes.Tamani.TamaniPregnantFuck.PullsUpGrabsCock".Localized());

                OutputText("Scenes.Tamani.TamaniPregnantFuck.CumNearPainful".Localized());

                if (Player.Balls > 0)
                    OutputText(Player.BallsDescriptLight());
                else
                    OutputText("Scenes.Tamani.TamaniPregnantFuck.Genitals".Localized());

                OutputText("Scenes.Tamani.TamaniPregnantFuck.AcheExplosive".Localized());

                OutputText("Scenes.Tamani.TamaniPregnantFuck.StaggersToFeet".Localized());

                OutputText("Scenes.Tamani.TamaniPregnantFuck.ThanksForGoodFuck".Localized(
                     Player.Mf("stud", "lover")
                ));

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
            }
            else
            {
                OutputText("Scenes.Tamani.TamaniPregnantFuck.SorryTooBig".Localized(
                     Player.Mf("stud", "lover")
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Tamani.TamaniPregnantFuck.MassagingBallsFeet".Localized());
                else
                    OutputText("Scenes.Tamani.TamaniPregnantFuck.JackingBase".Localized());

                OutputText("Scenes.Tamani.TamaniPregnantFuck.HandsCaressTease".Localized(
                     Player.CockHead(),

                     TamaniChest()
                ));

                OutputText("Scenes.Tamani.TamaniPregnantFuck.CumNearPainfulAgain".Localized());

                OutputText("Scenes.Tamani.TamaniPregnantFuck.GrindsThenStaggers".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.TamaniNumberOfDaughters, CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) + 3);

                OutputText("Scenes.Tamani.TamaniPregnantFuck.SloppyGoodbyeKiss".Localized());

                Player.Orgasm("Dick");
            }

            Player.RefillHunger(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniPoopsOutBabies()
        {
            ClearOutput();

            ShowImage("tamani-encounter-preggo");

            OutputText("Scenes.Tamani.TamaniPoopsOutBabies.OrgiasticScreams".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 0)
                OutputText("Scenes.Tamani.TamaniPoopsOutBabies.WithHerDaughters".Localized());

            OutputText("Scenes.Tamani.TamaniPoopsOutBabies.LayingAgainstTree".Localized(
                 TamaniChest()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) > 2)
            {
                OutputText("Scenes.Tamani.TamaniPoopsOutBabies.ThirdAppears".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) > 3)
                {
                    OutputText("Scenes.Tamani.TamaniPoopsOutBabies.Fourth".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) == 5)
                        OutputText("Scenes.Tamani.TamaniPoopsOutBabies.AndFifth".Localized());
                    else
                        OutputText("Scenes.Tamani.TamaniPoopsOutBabies.MoreThanYouCanCount".Localized());
                }

                OutputText("Scenes.Tamani.TamaniPoopsOutBabies.Period".Localized());
            }

            OutputText("Scenes.Tamani.TamaniPoopsOutBabies.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) == 2)
                OutputText("Scenes.Tamani.TamaniPoopsOutBabies.TwinsDrinkGrowing".Localized());
            else
                OutputText("Scenes.Tamani.TamaniPoopsOutBabies.ChildrenDrinkSquabble".Localized());

            OutputText("Scenes.Tamani.TamaniPoopsOutBabies.LooksUpWhispers".Localized(
                 NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount))
            ));

            OutputText("Scenes.Tamani.TamaniPoopsOutBabies.SighsRelaxes".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 10 + Player.Corruption / 10)));

            TamaniGivesBirth();

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniKnockUp()
        {
            if (NpcPregnancy.IsPregnant(Definition, State))
                return;

            NpcPregnancy.KnockUp(
                Definition, State, Pregnancy.PregnancyIds.PlayerFather, 216);

            CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, 2);

            var cum = Player.CumQ();

            if (Player.HasPerk(PerkIds.MaraesGiftStud))
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 3);

            if (cum >= 50 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);

            if (cum >= 100 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);

            if (cum >= 200 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);

            if (cum >= 300 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);

            if (cum >= 400 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);

            if (cum >= 500 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);

            if (cum >= 600 && Random.Shared.Next(2) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.TamaniPregnancyCount, CoC.Instance.Flags.Get(GameFlagIds.TamaniPregnancyCount) + 1);
        }

        public void EncounterTamani()
        {

            if (Player.Cocks.Count <= 0)
                TamaniFemaleEncounter();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TamaniMet) == 0)
                    TamaniMaleFirstEncounter();
                else

                    switch (NpcPregnancy.EventStage(Definition, State))
                    {
                        case 1:
                            TamaniPregnantEncounter();

                            break;

                        case 2:
                            TamaniPoopsOutBabies();

                            break;

                        default:
                            TamaniMaleRepeatEncounter();

                            break;
                    }
        }

        public void GetRapedByTamaniYouHypnoSlut(bool fromCombat = false)
        {
            ClearOutput();

            int primary = Player.CockThatFits(65);

            float secondary = 0;

            var cocks = Player.Cocks.Count;

            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.CrooksFinger".Localized());

            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.TracesFingers".Localized(
                 Player.ArmorName
            ));

            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SmirksBendsOver".Localized(
                 Player.Mf("boy", "herm")
            ));

            ShowImage("tamani-willing-sex");

            if (primary >= 0)
            {
                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.GrabsHerAss".Localized());

                if (cocks > 0)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.MainAgain".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.WarmthSpreads".Localized(
                     Player.CockHead(primary)
                ));

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Sheath".Localized());
                else
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Crotch".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.ParagraphBreak".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.WaitAdjusting".Localized());

                if (cocks > 1)
                {
                    if (primary > 0)
                        secondary = 0;
                    else
                        secondary = 1;

                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.MoansPullsIn".Localized(
                         Player.CockDescript((int)(secondary))
                    ));
                }
                else
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.MoansBabbling".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.DontStop".Localized(
                     Player.CockHead(primary)
                ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SecondThrustBetter".Localized());

                if (cocks > 0)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.AroundYour".Localized(
                         Player.CockDescript((int)(secondary))
                    ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.TinyFormJiggles".Localized());

                if (Player.Tallness > 55)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.LiftingOffGround".Localized());
                else
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.NearlyFalling".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.VisiblyOrgasms".Localized(
                     Player.CockDescript(primary)
                ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.PickUpPace".Localized());

                if (Player.HasFur())
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Matted".Localized());
                else
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Slicked".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.ImpactingFillAir".Localized());

                if (cocks == 1)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.TongueHangingOut".Localized());
                else
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.MouthTooFull".Localized(
                         Player.CockDescript((int)(secondary))
                    ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SwellingWider".Localized(
                     Player.CockDescript(primary)
                ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.IntenseOrgasm".Localized(
                     Player.CockDescript(primary)
                ));

                if (Player.HasKnot(primary))
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.KnotBalloons".Localized());
                else
                    if (Player.Cocks[primary].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.FlareWidens".Localized());
                    else
                        if (Player.Cocks[primary].Id == BodyPartTypes.Demon)
                            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.NodulesPulsate".Localized(
                                 Player.CockDescript(primary)
                            ));
                        else
                            if (Player.Cocks[primary].Id == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.TentacleTipWidens".Localized());
                            else
                                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.HeadSwells".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.BeginToFill".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.BriefSpasm".Localized());
                else
                    if (Player.CumQ() < 300)
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.VoluminousSeed".Localized());
                    else
                    {
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.MassiveGouts".Localized());

                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.TapersOff".Localized());
                        else
                            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.TrappedCumming".Localized());

                        if (Player.CumQ() >= 1000 && Player.CumQ() < 4000)
                            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.EventuallyEnds".Localized());
                        else
                            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.LastsForever".Localized());
                    }

                if (cocks > 1)
                {
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.ThroatSwallows".Localized());

                    if (Player.CumQ() > 300)
                    {
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.FailingLeaking".Localized());

                        if (Player.CumQ() > 600)
                            OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.PullsBackCoughing".Localized());
                    }
                    else
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Period".Localized());
                }

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.JoinedAtHips".Localized());

                if (Player.Tallness > 55)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.HeightDifferential".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.RegainsSenses".Localized(
                     Player.CockDescript(primary)
                ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SurgeOfPride".Localized());

                TamaniKnockUp();

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Sensitivity, -1f));
            }
            else
            {
                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.GrabsHerAssAgain".Localized());

                if (cocks > 1)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.MainAgain".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.WarmthSpreadsThe".Localized(
                     Player.CockHead()
                ));

                if (Player.BiggestBreastSize() < 1)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.OnYourChest".Localized());
                else
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.BetweenYourBreasts".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.AndCommands".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.DisgruntledObedient".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.FingersWrap".Localized());

                if (cocks > 1)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Primary".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.InsideSheath".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.DoAsTold".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.FeetLocked".Localized(
                     Appearance.CockNoun(BodyPartTypes.Human)
                ));

                if (Player.Tallness > 60)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Big".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.StrongManSexyLover".Localized(
                     Player.Mf("strong man's", "sexy lover's"),

                     Appearance.CockNoun(BodyPartTypes.Human)
                ));

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.AssureTheyLike".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Beg".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Ask".Localized());
                    else
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.Command".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.NotToStop".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.ObedientUntilEnd".Localized());

                if (Player.CumQ() > 250)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.FilledToCapacity".Localized());

                if (Player.CumQ() > 700)
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.FewMoreBlasts".Localized());

                if (Player.CumQ() > 1500)
                {
                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.KeepCummingForcing".Localized());

                    if (Player.CumQ() > 3000)
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.PuddleFinish".Localized());

                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.PeriodAgain".Localized());
                }

                if (cocks > 1)
                {
                    if (cocks == 2)
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.YourOther".Localized(
                             Player.CockDescript(1)
                        ));
                    else
                        OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.EachOtherCock".Localized());

                    OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SplattersJuice".Localized());
                }

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.ParagraphBreakThird".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SmilesPullsTube".Localized());

                OutputText("Scenes.Tamani.GetRapedByTamaniYouHypnoSlut.SurgeOfPrideAgain".Localized());

                TamaniKnockUp();

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Sensitivity, -1f));
            }

            if (fromCombat || CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void TamaniVictoryMenu()
        {

            CoC.Instance.Flags.Set(GameFlagIds.TamaniDefeatCounter, CoC.Instance.Flags.Get(GameFlagIds.TamaniDefeatCounter) + 1);

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.TamaniDefeatCounter) >= 4)
            {
                OutputText("Scenes.Tamani.TamaniVictoryMenu.TiredOfTamani".Localized());

                AddButton(10, "Scenes.Tamani.TamaniVictoryMenu.ButtonNoMore".Localized(), KillTamaniChoice);
            }

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);

            AddButtonDisabled(0, "Fuck", "This scene requires you to have a cock and sufficient arousal.");

            AddButtonDisabled(1, "Buttfuck", "This scene requires you to have a fitting cock and sufficient arousal.");

            AddButtonDisabled(2, "Lay Eggs", "This scene requires you to have a spider ovipositor and enough eggs. She should not be pregnant.");

            if (Player.Lust >= 33 && Player.HasCock())
            {
                OutputText("Scenes.Tamani.TamaniVictoryMenu.CouldFuckBut".Localized());

                AddButton(0, "Common.Button.Fuck".Localized(), () => TamaniSexWon());

                if (Player.HasCockThatFits(CombatScene.LastEnemy?.AnalCapacity() ?? 0))
                    AddButton(1, "Common.Button.ButtFuck".Localized(), TamaniAnalShits);
            }

            if (!State.IsPregnant && Player.CanOvipositSpider())
                AddButton(2, "Common.Button.LayEggs".Localized(), TamaniBeaten);

            if (!Player.HasKeyItem("Deluxe Dildo"))
                AddButton(3, "Scenes.Tamani.TamaniVictoryMenu.ButtonTakeDildo".Localized(), TamaniStealDildo);
        }

        public void TamaniStealDildo()
        {
            ClearOutput();

            ShowImage("item-dDildo");

            OutputText("Scenes.Tamani.TamaniStealDildo.VictorSpoils".Localized());

            OutputText("Scenes.Tamani.TamaniStealDildo.FoundPinkDildo".Localized());

            Player.CreateKeyItem("Deluxe Dildo", 0, 0, 0, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void TamaniAnalShits()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatScene.LastEnemy?.AnalCapacity() ?? 0);

            ShowImage("tamani-fuck");

            OutputText("Scenes.Tamani.TamaniAnalShits.GrabInsensate".Localized());

            OutputText("Scenes.Tamani.TamaniAnalShits.TooTemptingTarget".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Tamani.TamaniAnalShits.MenacingGrin".Localized());

            OutputText("Scenes.Tamani.TamaniAnalShits.StretchesAnus".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Tamani.TamaniAnalShits.MoansTurnShouts".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Tamani.TamaniAnalShits.CumBuilding".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void TamaniBeaten()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniBeaten.GoblinDefeatedTend".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Tamani.TamaniBeaten.LicksLipsAnticipation".Localized());
            else
                OutputText("Scenes.Tamani.TamaniBeaten.FrownsNoPenis".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.CluelessGrowing".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.SkitterBendOver".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.EagerSlutSmirks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesOvipositedTamani) == 0)
                OutputText("Scenes.Tamani.TamaniBeaten.MyBabyDaddymomma".Localized(
                     Player.Mf("daddy", "momma")
                ));
            else
                OutputText("Scenes.Tamani.TamaniBeaten.EggObsessedArachnophile".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.PurpleHairedSkank".Localized());

            ShowImage("goblinelder-win-drider-egged");

            OutputText("Scenes.Tamani.TamaniBeaten.EyesUpHere".Localized(
                 Player.Mf("stud", "breeder")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesOvipositedTamani) > 0)
                OutputText("Scenes.Tamani.TamaniBeaten.OrEvenMoreEggs".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.PetiteHandsMassage".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.LegsClatter".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.GigglesLittleEager".Localized(
                 Player.Mf("stud", "baby")
            ));

            OutputText("Scenes.Tamani.TamaniBeaten.SighsNiiice".Localized(
                 TamaniChest()
            ));

            OutputText("Scenes.Tamani.TamaniBeaten.HoldingHerFast".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.HandsRoam".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Tamani.TamaniBeaten.WhiteCoats".Localized());

            OutputText("Scenes.Tamani.TamaniBeaten.BloatedBitchSlides".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesOvipositedTamani, CoC.Instance.Flags.Get(GameFlagIds.TimesOvipositedTamani) + 1);

            if (Player.FertilizedEggs() == 0)

                NpcPregnancy.KnockUp(
                    Definition, State, Pregnancy.PregnancyIds.DriderEggs, 72);

            Player.DumpEggs();

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void KillTamaniChoice()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.KillTamaniChoice.GottenToNerves".Localized());

            ClearButtons();

            if (silly)
            {
                AddButton(0, "Scenes.Tamani.KillTamaniChoice.ButtonFight".Localized(), KillTamani);

                AddButton(1, "Scenes.Tamani.KillTamaniChoice.ButtonMercy".Localized(), SpareTamani);
            }
            else
            {
                AddButton(0, "Scenes.Tamani.KillTamaniChoice.ButtonKillHer".Localized(), KillTamani);

                AddButton(1, "Scenes.Tamani.KillTamaniChoice.ButtonSpareHer".Localized(), SpareTamani);
            }
        }

        public void KillTamani()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.KillTamani.StepOverGrab".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TamaniBadEnded, 1);

            PostTamaniRemoval();
        }

        public void SpareTamani()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.SpareTamani.TellHerWarning".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TamaniBadEnded, 0.5f);

            PostTamaniRemoval();
        }

        public void PostTamaniRemoval()
        {

            ShowImage("tamani-satchel");

            OutputText("Scenes.Tamani.PostTamaniRemoval.TamaniNoMore".Localized());

            if (CombatScene.LastEnemy is { } lastEnemy)
                lastEnemy.Experience += 100;

            Player.CreateKeyItem("Tamani's Satchel", 2, 1, 1, 100);

            if (State.IsPregnant)
                NpcPregnancy.Clear(State);

            CombatScene.CleanupAfterCombat();
        }

        public void PreferTamaniFemdom()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.PreferTamaniFemdom.LaughsIntoGirls".Localized());

            OutputText("Scenes.Tamani.PreferTamaniFemdom.AdvancesCloser".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptTamaniFacesits);

            AddButton(1, "Common.Button.Refuse".Localized(), DeclineZeFacesits);
        }

        public void DeclineZeFacesits()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.DeclineZeFacesits.NotInterested".Localized());

            OutputText("Scenes.Tamani.DeclineZeFacesits.KicksYou".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AcceptTamaniFacesits()
        {
            ClearOutput();

            ShowImage("tamani-fuck");

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.EyeHipsNod".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.SmirkBlooms".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.PullsVial".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.LittleInsurance".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.FieldOfView".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.NoseJammed".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.TryToBreathe".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.YellsBouncing".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.StopsOnOwn".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.TakesAllYouHave".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.SlipHandAround".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.CriesCheeksClench".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.ScreamsPinching".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.MovementsStop".Localized());

            OutputText("Scenes.Tamani.AcceptTamaniFacesits.GreenWhoreCries".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 20)));

            if (Save.GetInt("timesRidden", 0) + 1 < 3)
            {
                Save.SetInt("timesRidden", Save.GetInt("timesRidden", 0) + 1);

                OutputText("Scenes.Tamani.AcceptTamaniFacesits.WithdrawFingers".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Tamani.AcceptTamaniFacesits.MoveToExtract".Localized());

                OutputText("Scenes.Tamani.AcceptTamaniFacesits.NotDoneYet".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Continue".Localized(), TamaniFacesitContinue);

                AddNextButton("Scenes.Tamani.AcceptTamaniFacesits.ButtonPushOff".Localized(), TamaniFacesitPush);
            }
        }

        public void TamaniFacesitContinue()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniFacesitContinue.WhateverInStore".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.StartsWithoutLooking".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.PunctuatesRemark".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.StartsWithSlap".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.ShouldBeGrateful".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.MoanEscapes".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.OhFuHnnn".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.DespiteBestEfforts".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.QuiteSurprised".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.FeelReady".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitContinue.BriefThought".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void TamaniFacesitPush()
        {
            ClearOutput();

            OutputText("Scenes.Tamani.TamaniFacesitPush.NotInterestedFinding".Localized());

            OutputText("Scenes.Tamani.TamaniFacesitPush.HitsGroundOof".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void OpenTamanisSatchel()
        {
            ClearOutput();

            var isEmpty = true;

            OutputText("Scenes.Tamani.OpenTamanisSatchel.OpenSatchel".Localized());

            if (!Player.HasKeyItem("Deluxe Dildo"))
            {
                ShowImage("item-dDildo");

                OutputText("Scenes.Tamani.OpenTamanisSatchel.FoundPinkDildo".Localized());

                Player.CreateKeyItem("Deluxe Dildo", 0, 0, 0, 0);
            }

            ClearButtons();

            if (Player.KeyItemV1("Tamani's Satchel") > 0)
            {
                ShowImage("item-reducto");

                OutputText("Scenes.Tamani.OpenTamanisSatchel.Reducto".Localized((Player.KeyItemV1("Tamani's Satchel") == 1 ? "is a container" : "are two containers")));

                AddButton(0, ItemRegistry.Create(ItemIds.Reducto).Name, () => SatchelTakeItem(ItemIds.Reducto, 1));

                isEmpty = false;
            }

            if (Player.KeyItemV2("Tamani's Satchel") > 0)
            {
                ShowImage("item-dye");

                OutputText("Scenes.Tamani.OpenTamanisSatchel.PinkHairDye".Localized());

                AddButton(1, ItemRegistry.Create(ConsumableIds.HairDyePink).Name, () => SatchelTakeItem(ConsumableIds.HairDyePink, 2));

                isEmpty = false;
            }

            if (Player.KeyItemV3("Tamani's Satchel") > 0)
            {
                ShowImage("item-lDraft");

                OutputText("Scenes.Tamani.OpenTamanisSatchel.LustDraft".Localized((Player.KeyItemV3("Tamani's Satchel") == 1 ? "is a bottle" : "are two bottles")));

                AddButton(2, ItemRegistry.Create(ConsumableIds.LustDraft).Name, () => SatchelTakeItem(ConsumableIds.LustDraft, 3));

                isEmpty = false;
            }

            if (Player.KeyItemV4("Tamani's Satchel") > 0)
            {
                ShowImage("item-gems");

                OutputText("Scenes.Tamani.OpenTamanisSatchel.HundredGems".Localized());

                AddButton(3, "Common.Button.Gems".Localized(), SatchelTakeGems);

                isEmpty = false;
            }

            OutputText("Scenes.Tamani.OpenTamanisSatchel.ParagraphBreak".Localized());

            AddButton(14, "Scenes.Tamani.OpenTamanisSatchel.ButtonClose".Localized(), () => InventoryScene.InventoryMenu());

            if (isEmpty)
            {

                ClearOutput();

                ClearButtons();

                OutputText("Scenes.Tamani.OpenTamanisSatchel.SatchelEmpty".Localized());

                Player.RemoveKeyItem("Tamani's Satchel");

                AddButton(0, "Common.Continue".Localized(), () => InventoryScene.InventoryMenu());
            }
        }

        public void SatchelTakeItem(Identifier item, int keyValue)
        {

            Player.AddKeyValue("Tamani's Satchel", keyValue, -1);

            InventoryScene.TakeItem(item, OpenTamanisSatchel);
        }

        public void SatchelTakeGems()
        {

            Player.Gems += 100;

            Player.AddKeyValue("Tamani's Satchel", 4, -100);

            OpenTamanisSatchel();
        }

    }
}