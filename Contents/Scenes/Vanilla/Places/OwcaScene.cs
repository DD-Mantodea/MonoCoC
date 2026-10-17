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
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places
{
    [Register<OwcaScene>("coc:owca")]
    public class OwcaScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Places.Owca);

        public string debugName => "Owca";

        public string debugHint => "";

        public int owcaAttitude { get => Math.Clamp(CoC.Instance.Flags.Get(GameFlagIds.OwcasAttitude), 0, 100); set => CoC.Instance.Flags.Set(GameFlagIds.OwcasAttitude, Math.Clamp(value, 0, 100)); }

        public override void Entry()
        {

        }

        public void Reset()
        {

            Save.SetBool("rebeccKilled", false);

            Save.SetBool("lostPit", false);
        }

        public void GangbangVillageStuff()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.OwcaUnlocked) == 1)
                OwcaMainScreenOn();
            else
                GangbangVillageFirstGoRound();
        }

        public void GangbangVillageFirstGoRound()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.DeclinedToVisitRebeccsVillage) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesRefusedRebeccsOffer) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) == 0)
            {
                owcaAttitude = 50;

                CoC.Instance.Flags.Set(GameFlagIds.VapulaSubmissiveness, 50);

                OutputText("Scenes.Owca.GangbangVillageFirstGoRound.WanderEndlessField".Localized());

                OutputText("Scenes.Owca.GangbangVillageFirstGoRound.FemaleEmergesFromGroup".Localized());

                OutputText("Scenes.Owca.GangbangVillageFirstGoRound.GirlClosesDistance".Localized());

                OutputText("Scenes.Owca.GangbangVillageFirstGoRound.CuriosityGetsBetterOf".Localized());

                OutputText("Scenes.Owca.GangbangVillageFirstGoRound.FollowUsValiantWarrior".Localized());

                OutputText("Scenes.Owca.GangbangVillageFirstGoRound.DoYouFollowHer".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), AgreeToFollowRebecFirstTime);

                AddButton(1, "Common.Button.No".Localized(), DontGoToZeVillage);

                if (!Player.IsChild() && (OptionStore.Instance.Get("gore", 0) > 0))
                    AddButton(2, "Common.Button.Snuff".Localized(), SnuffRebecc);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) == 0)
                {
                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.AfterWanderingAcrossPlains".Localized());

                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.AsBeforeYouNotice".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), AgreeToFollowRebecFirstTime);

                    AddButton(1, "Common.Button.No".Localized(), DontGoToZeVillage);

                    if (!Player.IsChild() && (OptionStore.Instance.Get("gore", 0) > 0))
                        AddButton(2, "Common.Button.Snuff".Localized(), SnuffRebecc);
                }
                else
                {
                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.UsingVividMemory".Localized());

                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.AreAWonderfulPerson".Localized());

                    if (Player.Corruption > 50)
                        OutputText("Scenes.Owca.GangbangVillageFirstGoRound.CantHelpButSmile".Localized());

                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.GrabsHandDragsYou".Localized());

                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.WinksAndWalks".Localized());

                    OutputText("Scenes.Owca.GangbangVillageFirstGoRound.OwcaVillageAdded".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, 1);

                    DoNext(() => GangbangVillageStuff());
                }
        }

        public void DontGoToZeVillage()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.DeclinedToVisitRebeccsVillage, CoC.Instance.Flags.Get(GameFlagIds.DeclinedToVisitRebeccsVillage) + 1);

            ReturnToCamp(1);
        }

        public void AgreeToFollowRebecFirstTime()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesRefusedRebeccsOffer) == 0)
            {
                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.HowCouldYouRefuse".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.RegretfullyBare".Localized());

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.GroinFeelsContact".Localized());

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ThereWeveAlmostArrived".Localized());

                if (silly)
                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.VillageNoAnalVirgins".Localized());
                else
                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.TheyAbusedUsMistreated".Localized());

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.SayEnd".Localized());

                if (Player.Corruption < 20)
                {
                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ShockedByThisRevelation".Localized());

                    if (silly)
                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.NeverWorryAgain".Localized());
                    else
                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.NeverBeAbusedAgain".Localized());

                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.AnythingCanBeDone".Localized());
                }
                else
                    if (Player.Corruption > 60 && Player.Libido > 60 && Player.HasCock())
                    {
                        if (silly)
                            OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.CantHelpButPop".Localized());
                        else
                            OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ParagraphBreak".Localized());

                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.UncontrollablyTurnedOn".Localized());
                    }

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.VillageManMetDemons".Localized());

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.KnowWhatDeal".Localized());

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.WaversForAMoment".Localized());

                if (Player.Corruption < 50 && Player.Intelligence >= 50)
                {
                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.SoThisIsIt".Localized());

                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ButOnSecondThought".Localized((CoC.Instance.Flags.Get(GameFlagIds.FactoryOmnibusDefeated) != 0 ? "the people in your village" : "your elders")));

                    if (Player.Libido > 50)
                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ConsideringWhatRebeccMeant".Localized());

                    OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.MaybeNotAllAct".Localized());
                }
                else
                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.EyesWidenInSurprise".Localized((CoC.Instance.Flags.Get(GameFlagIds.FactoryOmnibusDefeated) != 0 ? "the people in your village" : "your elders")));

                        if (Player.Libido > 50)
                            OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ConsideringRebeccAgain".Localized());

                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.NotAllActAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ThisIsItAgain".Localized());

                        if (Player.Libido >= 50)
                            OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.OnSecondThoughtProspect".Localized());
                    }

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.DoYouAccept".Localized());
            }
            else
            {
                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.ChangedYourMind".Localized());

                OutputText("Scenes.Owca.AgreeToFollowRebecFirstTime.KnowWhatTheseWant".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AcceptRebeccsPlea);

            AddButton(1, "Common.Button.No".Localized(), DeclineRebeccsPlea);
        }

        public void DeclineRebeccsPlea()
        {
            ClearOutput();

            OutputText("Scenes.Owca.DeclineRebeccsPlea.CantGiveInTo".Localized());

            owcaAttitude -= 5;

            CoC.Instance.Flags.Set(GameFlagIds.TimesRefusedRebeccsOffer, CoC.Instance.Flags.Get(GameFlagIds.TimesRefusedRebeccsOffer) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AcceptRebeccsPlea()
        {
            ClearOutput();

            owcaAttitude += 10;

            OutputText("Scenes.Owca.AcceptRebeccsPlea.TellDesperateGirlThat".Localized());

            OutputText("Scenes.Owca.AcceptRebeccsPlea.GroupOfFarmersThen".Localized());

            if (Player.Corruption > 70 && (int)Player.Gender > 0)
            {
                OutputText("Scenes.Owca.AcceptRebeccsPlea.LickLipsInAnticipation".Localized());

                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript(0));

                if (Player.HasVagina() && Player.HasCock())
                    OutputText("Scenes.Owca.AcceptRebeccsPlea.AndWord".Localized());

                if (Player.HasCock())
                    OutputText(Player.MultiCockDescriptLight());

                OutputText("Scenes.Owca.AcceptRebeccsPlea.AlreadySlickeningWithJuices".Localized());
            }
            else
                OutputText("Scenes.Owca.AcceptRebeccsPlea.ShrugAndTellYourself".Localized());

            OutputText("Scenes.Owca.AcceptRebeccsPlea.VividThoughtStrengthens".Localized());

            OutputText("Scenes.Owca.AcceptRebeccsPlea.PuttingDreadAside".Localized());

            IntoTheDemonPit(true);
        }

        public void IntoTheDemonPit(bool sacrifice = true)
        {
            ClearOutput();

            if (View.Time.Hours < 21)
            {
                var passed = 21 - View.Time.Hours;

                OutputText("Scenes.Owca.IntoTheDemonPit.BBLetters".Localized(
                    NumberUtils.Num2Text(passed),

                    passed > 1 ? "hours pass" : "hour passes"
                ));

                View.Time.Hours = 21;

            }

            OutputText("Scenes.Owca.IntoTheDemonPit.AwakenedByASudden".Localized());

            OutputText("Scenes.Owca.IntoTheDemonPit.LookAroundPuzzled".Localized());

            OutputText("Scenes.Owca.IntoTheDemonPit.SoonAllGathered".Localized());

            OutputText("Scenes.Owca.IntoTheDemonPit.TallPurpleDemonessWalks".Localized());

            if (sacrifice)
                OutputText("Scenes.Owca.IntoTheDemonPit.TangledInYourRestraints".Localized());

            OutputText("Scenes.Owca.IntoTheDemonPit.LeansDownAndCoos".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) == 0)
                OutputText("Scenes.Owca.IntoTheDemonPit.EveryoneDoYouKnow".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) > 50)
                    OutputText("Scenes.Owca.IntoTheDemonPit.YouAgainByLethice".Localized());
                else
                    OutputText("Scenes.Owca.IntoTheDemonPit.FuckYouAgain".Localized());

            if (sacrifice)
            {
                if (Player.Strength >= 80)
                    OutputText("Scenes.Owca.IntoTheDemonPit.TestTheChains".Localized());
                else
                    if (Player.Speed >= 80)
                        OutputText("Scenes.Owca.IntoTheDemonPit.TestTheChainsAgain".Localized());
                    else
                        OutputText("Scenes.Owca.IntoTheDemonPit.ChainsBindingYouArent".Localized());

                OutputText("Scenes.Owca.IntoTheDemonPit.OnceAgainFeelingRegret".Localized());
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySuccubus);

            CoC.Instance.Flags.Set(GameFlagIds.TimesInDemonPit, CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.DaysSinceLastDemonDealings, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), () => FightZeDemons(sacrifice));

            AddButton(1, "Common.Button.Submit".Localized(), () => LoseOrSubmitToVapula(false));
        }

        public void FightZeDemons(bool sacrifice = true)
        {

            var vapula = EnemyRegistry.Create(EnemyIds.VapulaFight);

            var demons = EnemyRegistry.Create(EnemyIds.LustyDemons);

            if (sacrifice)
            {
                if (Player.CanDisarm())
                    Player.Disarm(Player.Weapon);

                Player.CreateStatusEffect(StatusEffectIds.BowDisabled, 0, 0, 0, 0);

                if (Player.Strength < 80 && Player.Speed < 80)
                {
                    vapula.CreateStatusEffect(StatusEffectIds.AttackDisabled, 0, 0, 0, 0);

                    vapula.CreateStatusEffect(StatusEffectIds.RunDisabled, 0, 0, 0, 0);

                    vapula.CreateStatusEffect(StatusEffectIds.PhysicalDisabled, 0, 0, 0, 0);

                    demons.CreateStatusEffect(StatusEffectIds.AttackDisabled, 0, 0, 0, 0);

                    demons.CreateStatusEffect(StatusEffectIds.RunDisabled, 0, 0, 0, 0);

                    demons.CreateStatusEffect(StatusEffectIds.PhysicalDisabled, 0, 0, 0, 0);
                }
                else
                    if (!Player.CanFly())
                    {
                        vapula.CreateStatusEffect(StatusEffectIds.RunDisabled, 0, 0, 0, 0);

                        demons.CreateStatusEffect(StatusEffectIds.RunDisabled, 0, 0, 0, 0);
                    }
            }

            CombatScene.StartMultiple(new[] { vapula, demons }, () => ReturnToCamp(1));
        }

        public void LoseOrSubmitToVapula(bool fromVillage = false)
        {
            ClearOutput();

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.VapulaTauntsCircles".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.FollowedByYour".Localized(
                    Player.VaginaDescript(0)
                ));

            if (Player.HasCock())
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.BeforeWrappingAroundYour".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.MoreHandsStart".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.SoonCompletelyOverwhelmed".Localized());

            Player.ButtChange(60, true, true, false);

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.DoublePenetrationBrutal".Localized());

            if (!Player.HasPerk(PerkIds.Buttslut))
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.NotSupposedToEnjoy".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.TryToCryOut".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.MatterOfMinutes".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.CantTalkMuted".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.WrapsAroundWhile".Localized());

            if (Player.Body.Get<AntennaePart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.AntennaeHarshlyPulled".Localized());

            if (Player.Horns.Id.Path != "none")
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.FeelHornsBeingUsed".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.SuddenlySharpSensation".Localized());

                Player.CuntChange(60, true, true, false);

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.DickFoundWayIn".Localized(
                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0)
                ));

                if (!Player.HasStatusEffect(StatusEffectIds.BonusVCapacity))
                    Player.CreateStatusEffect(StatusEffectIds.BonusVCapacity, 0, 0, 0, 0);

                if (Player.StatusEffectValue(StatusEffectIds.BonusVCapacity, 0) < 200)
                    Player.ChangeStatusValue(StatusEffectIds.BonusVCapacity, 0, 15);

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.NewSpaceFilled".Localized());

                if (Player.Wetness() < 5)
                    Player.Vaginas[0].Wetness += 1;
            }

            if (Player.HasCock())
            {
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.SoftBrushAgainstYour".Localized(
                    Player.CockDescript(0)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.SoonJoinedByOther".Localized());

                if (Player.CockTotal() >= 4)
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.NeverthelessNotSatisfied".Localized());

                if (Player.CockTotal() >= 7)
                {
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.SadlySurroundedNo".Localized(
                        Player.CockDescript(6)
                    ));

                    if (Player.CockTotal() > 8)
                        OutputText("Scenes.Owca.LoseOrSubmitToVapula.RemainingPricks".Localized());

                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.TonguesExpertlyLick".Localized());
                }

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.BetweenEveryThrustHands".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.SLetter".Localized());

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.EveryInchTakenCare".Localized());

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.RiddenForAWhile".Localized());
            }

            if (Player.LactationQ() > 0)
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.LostInFeverishState".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.InsaneMixOfViolation".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.PulsatesAndThrobs".Localized(
                    Player.SMultiCockDesc()
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.BallsSwellAndBoil".Localized());

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.GiganticSpoogeFlow".Localized());

                if (Player.CockTotal() > 7)
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.AndMouths".Localized());

                OutputText("Scenes.Owca.LoseOrSubmitToVapula.ViolentCumshotProduces".Localized());

                if (Player.CumQ() > 1500)
                    OutputText("Scenes.Owca.LoseOrSubmitToVapula.EffortAppearsVain".Localized());
            }

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.AfterHoursOf".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.FillingYourWomb".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.RemainingCumSpurts".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.YourOwnClenches".Localized(
                    Player.VaginaDescript(0)
                ));

            if (Player.LactationQ() > 0)
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.CombinedEffortsOfDozen".Localized());
            else
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.ParagraphBreak".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.PeopleKeepCumming".Localized());

            OutputText("Scenes.Owca.LoseOrSubmitToVapula.KeepOozingOutFluids".Localized());

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 14);

            CoC.Instance.Flags.Set(GameFlagIds.VapulaSubmissiveness, CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) + 10);

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) >= 90 && CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) < 100)
                OutputText("Scenes.Owca.LoseOrSubmitToVapula.YoureStartingToGet".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) >= 100)
                DoNext(() => SlaveToVapulaBadEnd());
            else
                if (fromVillage)
                    DoNext(() => MorningAfterRape());
                else
                    DoNext(() => WakeUpAfterDemonGangBangs());

            Player.Orgasm("All");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 3f));

            CoC.Instance.Flags.Set(GameFlagIds.RebeccsLastPlea, 0);

            Save.SetBool("lostPit", true);
        }

        public void WakeUpAfterDemonGangBangs()
        {
            ClearOutput();

            View.Time.Hours = 7;

            View.Time.Days += 1;

            OutputText("Scenes.Owca.WakeUpAfterDemonGangBangs.WhenYouWakeUp".Localized());

            Player.ChangeFatigue(20);

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Strength, -2f), new DynStat(GameStats.Toughness, -2f), new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Intelligence, -1f), new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 100f), new DynStat(GameStats.Corruption, 3f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void DefeetVapulasHorde()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.RebeccsLastPlea, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) <= 0)
            {
                SubdueVapula();

                return ;
            }

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.AttackDisabled))
            {
                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Owca.DefeetVapulasHorde.GrinWickedlyAsLast".Localized());
                else
                    OutputText("Scenes.Owca.DefeetVapulasHorde.GrinWickedlyAsDemons".Localized());
            }
            else
                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Owca.DefeetVapulasHorde.GrinAsLastAgain".Localized());
                else
                    OutputText("Scenes.Owca.DefeetVapulasHorde.GrinAsDemonsAgain".Localized());

            if (Player.IsCorruptEnough(60) && (int)Player.Gender > 0)
            {
                OutputText("Scenes.Owca.DefeetVapulasHorde.DoYouTakeAdvantage".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), RapeZeVapula);

                AddButton(1, "Common.Button.No".Localized(), NoVapulaSex);
            }
            else
            {
                OutputText("Scenes.Owca.DefeetVapulasHorde.ThoughTheDisplay".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.VapulaSubmissiveness, CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) - 5);

                CombatScene.CleanupAfterCombat();
            }
        }

        public void NoVapulaSex()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.VapulaSubmissiveness, CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) - 5);

            CombatScene.CleanupAfterCombat();
        }

        public void RapeZeVapula()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.VapulaSubmissiveness, CoC.Instance.Flags.Get(GameFlagIds.VapulaSubmissiveness) - 10);

            if (Player.HasCock())
            {
                OutputText("Scenes.Owca.RapeZeVapula.HordeStrugglesBeforeYou".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Owca.RapeZeVapula.GentlyStrokeYourBringing".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Owca.RapeZeVapula.StickFingerLubricate".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.DefeatedHordeWatches".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Owca.RapeZeVapula.ManyBruisesShesReceived".Localized());
                else
                    OutputText("Scenes.Owca.RapeZeVapula.ConstantFlushing".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.HarshlyGrabHerHand".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Owca.RapeZeVapula.BitchInHeatAgain".Localized());
                else
                    OutputText("Scenes.Owca.RapeZeVapula.CentaurInRutAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.BetterPlansForHer".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.YankOnHerArm".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.MomentSheTries".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SheMoans".Localized(
                    Player.Mf("manlet", "maiden")
                ));

                OutputText("Scenes.Owca.RapeZeVapula.SlapYourHand".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.LazyNonchalanceGrabHold".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.SheHasExtraRoom".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.KeepThrustingBackForth".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.SLetter".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.BeforeOncePowerful".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.AndHerLoveTunnel".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SentencePeriod".Localized());

                if (Player.CumQ() > 1000)
                    OutputText("Scenes.Owca.RapeZeVapula.SomeSpurtsByLittle".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.WatchingThisRough".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Owca.RapeZeVapula.AnalWord".Localized());
                else
                    OutputText("Scenes.Owca.RapeZeVapula.DoublePenetrationWord".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SessionBroughtVapulaTo".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.ThrowAwayYourExpendable".Localized());

                if (Player.CumQ() > 1000)
                    OutputText("Scenes.Owca.RapeZeVapula.ExpandableWord".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.CumdumpsterGrabAnother".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.Balls".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.PoorDemonessCheekStained".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Owca.RapeZeVapula.FondlingYourBalls".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.AlternativelyDeepthroating".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.WarmingUpDickflesh".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.SHardenAndThicken".Localized());
                else
                    OutputText("Scenes.Owca.RapeZeVapula.HardensAndThickens".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.AgainAsSheSucks".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SheWhimpers".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.PissOffBitch".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SeizeYourFuckToy".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.SLetterAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.AimAtVapulaSplattering".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.OnlyTheBeginning".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.NotSatiatedYet".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Owca.RapeZeVapula.UnholySexFrenzy".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.VapulaEntirelyCovered".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.YouViolateMyPets".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.HaWithoutAWord".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.ReachStaminaLimit".Localized(
                    Player.CockDescript(0)
                ));

                OutputText("Scenes.Owca.RapeZeVapula.WalkUpToVapula".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.FewMetersAwayOutside".Localized());

                if (silly)
                    OutputText("Scenes.Owca.RapeZeVapula.LookBackYouSnicker".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Libido, -4f), new DynStat(GameStats.Corruption, 4f));
            }
            else
            {
                OutputText("Scenes.Owca.RapeZeVapula.DemonHordeStrugglesAgain".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Owca.RapeZeVapula.GentlyStrokeAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Owca.RapeZeVapula.StickAFingerAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.DefeatedHordeWatchesAgain".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Owca.RapeZeVapula.ManyBruisesAgain".Localized());
                else
                    OutputText("Scenes.Owca.RapeZeVapula.ConstantFlushingAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.HarshlyGrabAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Owca.RapeZeVapula.BitchInHeatAgain".Localized());
                else
                    OutputText("Scenes.Owca.RapeZeVapula.CentaurInRutAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.BetterPlansAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.YankHerArmAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.AsSheTriesAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.VapulaSnarls".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SlapHandAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.LazyNonchalanceAgain".Localized());

                Player.CuntChange(40, true, true, false);

                if (Player.VaginalCapacity() > 80)
                    OutputText("Scenes.Owca.RapeZeVapula.CockLostInCunt".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.KeepThrustingAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.KeepWorkingIncubus".Localized());

                if (Player.Wetness() >= 5)
                    OutputText("Scenes.Owca.RapeZeVapula.SoundOfSpatteringFluid".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.RelentlessMilkingPressure".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.WatchingRoughAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.ThrowYourExpendableToy".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Owca.RapeZeVapula.AndYourBounceGently".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.GrindYourSnatchInto".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.TiedDemonWhines".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.PissOffBitchAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.FeelAnotherHighComing".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.OnlyBeginningAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.SnapGripTheIncubus".Localized());

                Player.ButtChange(60, true, true, false);

                OutputText("Scenes.Owca.RapeZeVapula.MoreDicksPresentThemselves".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Owca.RapeZeVapula.FreakishNipplesMoisten".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.VapulaStaresAtYou".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.ViolatePetsAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.UnableToBreakGaze".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Owca.RapeZeVapula.SLetterThird".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.GaspAsTheySpurt".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.FreedOfTheChains".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Owca.RapeZeVapula.TeatsWord".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.AndAssOntoHer".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.MercilesslyFuck".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.AtLastLimitAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.WalkUpToAgain".Localized());

                OutputText("Scenes.Owca.RapeZeVapula.FewMetersAwayAgain".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Toughness, 1f), new DynStat(GameStats.Libido, -4f), new DynStat(GameStats.Corruption, 4f));

                Player.SlimeFeed();

                Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation);
            }

            View.Time.Hours = 7;

            View.Time.Days += 1;

            CombatScene.CleanupAfterCombat();
        }

        public void OwcaMainScreenOn()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.RebeccsLastPlea) == 1 && false)
            {
                RebeccsLastPlea();

                return ;
            }

            if (owcaAttitude <= 5 && CoC.Instance.Flags.Get(GameFlagIds.OwcaAngerDisabled) == 0 && CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
            {
                DesperateVillages();

                return ;
            }

            if (Random.Shared.Next(10) <= 2 && CoC.Instance.Flags.Get(GameFlagIds.DaysSinceLastDemonDealings) >= 7 && CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
            {
                OutputText("Scenes.Owca.OwcaMainScreenOn.VillageShowsUnusualSigns".Localized());

                OutputText("Scenes.Owca.OwcaMainScreenOn.DifficultThingTo".Localized());

                OutputText("Scenes.Owca.OwcaMainScreenOn.ThisTimeFullyUnderstand".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), AcceptRebeccsPlea);

                AddButton(1, "Common.Button.No".Localized(), DeclineRebeccsPlea);

                return ;
            }

            ShowImage("location-owca");

            if (owcaAttitude > 80)
                OutputText("Scenes.Owca.OwcaMainScreenOn.VillagersGreetYou".Localized());
            else
                if (owcaAttitude > 60)
                    OutputText("Scenes.Owca.OwcaMainScreenOn.VillagersGreetFriendly".Localized());
                else
                    if (owcaAttitude > 40)
                        OutputText("Scenes.Owca.OwcaMainScreenOn.VillagersAcceptPresence".Localized());
                    else
                        if (owcaAttitude > 20)
                            OutputText("Scenes.Owca.OwcaMainScreenOn.VillagersLookUncomfortably".Localized());
                        else
                            OutputText("Scenes.Owca.OwcaMainScreenOn.VillagersOpenly".Localized());

            OutputText("Scenes.Owca.OwcaMainScreenOn.VillageRatherSmall".Localized());

            OutputText("Scenes.Owca.OwcaMainScreenOn.EndOfMainRoad".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DaysSinceLastDemonDealings) == 7 && CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
                OutputText("Scenes.Owca.OwcaMainScreenOn.AWeek".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.DaysSinceLastDemonDealings) > 7 && CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
                    OutputText("Scenes.Owca.OwcaMainScreenOn.MoreThanAWeek".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DaysSinceLastDemonDealings) >= 7 && CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
                OutputText("Scenes.Owca.OwcaMainScreenOn.HasPassedSinceOffering".Localized());

            ClearButtons();

            if (View.Time.IsTimeBetween(16, 21) && CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
                AddButton(0, "Common.Button.Pit".Localized(), ZePit);
            else
                AddButton(0, "Common.Button.Pit".Localized(), () => { });

            if (owcaAttitude >= 50)
            {
                OutputText("Scenes.Owca.OwcaMainScreenOn.BeyondTheTinyHamlet".Localized());

                AddButton(1, "Common.Button.Herds".Localized(), Herds);
            }
            else
                AddButton(1, "Common.Button.Herds".Localized(), null);

            OutputText("Scenes.Owca.OwcaMainScreenOn.RebeccsHouseNearby".Localized());

            AddButton(2, "Common.Button.Rebecc".Localized(), RebeccMenu);

            if (owcaAttitude >= 10)
            {
                OutputText("Scenes.Owca.OwcaMainScreenOn.TavernAppearsOpen".Localized());

                AddButton(3, "Common.Button.Tavern".Localized(), OwcaTavern);
            }
            else
                AddButton(3, "Common.Button.Tavern".Localized(), null);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void OwcaTavern()
        {
            ClearOutput();

            ShowImage("location-owca-tavern");

            ClearButtons();

            OutputText("Scenes.Owca.OwcaTavern.TavernNiceAndCozy".Localized());

            OutputText("Scenes.Owca.OwcaTavern.SheepMilkBottleItem".Localized(
                180 - owcaAttitude
            ));

            if ((180 - owcaAttitude) > Player.Gems)
                AddButton(0, "Items.SheepMilk.Short".Localized(), null);
            else
                AddButton(0, "Items.SheepMilk.Short".Localized(), () => OwcaBuySetup(ItemIds.SheepMilk));

            OutputText("Scenes.Owca.OwcaTavern.GoblinAleItem".Localized(
                60 - (float)(owcaAttitude / 2)
            ));

            if ((60 - (float)(owcaAttitude / 2)) > Player.Gems)
                AddButton(1, "Items.GoblinAle.Short".Localized(), null);
            else
                AddButton(1, "Items.GoblinAle.Short".Localized(), () => OwcaBuySetup(ConsumableIds.GoblinAle));

            if (Random.Shared.Next(100) > owcaAttitude)
            {
                OutputText("Scenes.Owca.OwcaTavern.BroBrewItem".Localized());

                if (2000 > Player.Gems)
                    AddButton(2, "Items.BroBrew.Short".Localized(), null);
                else
                    AddButton(2, "Items.BroBrew.Short".Localized(), () => OwcaBuySetup(ConsumableIds.BroBrew));
            }

            OutputText("Scenes.Owca.OwcaTavern.MinotaurCumItem".Localized(
                300 - owcaAttitude
            ));

            if ((300 - owcaAttitude) > Player.Gems)
                AddButton(3, "Common.Button.Minotaurcum".Localized(), null);
            else
                AddButton(3, "Common.Button.Minotaurcum".Localized(), () => OwcaBuySetup(ConsumableIds.MinotaurCum));

            OutputText("Scenes.Owca.OwcaTavern.ClovisItem".Localized(
                80 - (float)(owcaAttitude / 2)
            ));

            if ((80 - (float)(owcaAttitude / 2)) > Player.Gems)
                AddButton(4, "Items.Clovis.Short".Localized(), null);
            else
                AddButton(4, "Items.Clovis.Short".Localized(), () => OwcaBuySetup(ConsumableIds.CLOVERS));

            AddButton(14, "Common.Button.Back".Localized(), GangbangVillageStuff);
        }

        public void OwcaBuySetup(Identifier item)
        {

            if (item == ItemIds.SheepMilk)
                BuyOwcaShit(item, (180 - owcaAttitude));
            else
                if (item == ConsumableIds.GoblinAle)
                    BuyOwcaShit(item, (60 - (float)(owcaAttitude / 2)));
                else
                    if (item == ConsumableIds.BroBrew)
                        BuyOwcaShit(item, 2000);
                    else
                        if (item == ConsumableIds.CLOVERS)
                            BuyOwcaShit(item, (80 - (float)(owcaAttitude / 2)));
                        else
                            BuyOwcaShit(item, (300 - owcaAttitude));
        }

        public void BuyOwcaShit(Identifier bleh, float price = 0)
        {
            ClearOutput();

            Player.Gems -= (int)(price);

            OutputText("Scenes.Owca.BuyOwcaShit.BartenderHandsBottle".Localized());

            InventoryScene.TakeItem(bleh, OwcaTavern);
        }

        public void Herds()
        {
            ClearOutput();

            OutputText("Scenes.Owca.Herds.HerdersGreetWarmly".Localized());

            OutputText("Scenes.Owca.Herds.HerderHandsYou".Localized());

            owcaAttitude += 5;

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            if (owcaAttitude > 70)
            {
                OutputText("Scenes.Owca.Herds.VillagersThankYou".Localized());

                InventoryScene.TakeItem(ItemIds.SheepMilk, () => ReturnToCamp(1));
            }
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void ZePit()
        {
            ClearOutput();

            OutputText("Scenes.Owca.ZePit.AnnounceToAllVillagers".Localized());

            IntoTheDemonPit(false);
        }

        public void RebeccMenu()
        {
            ClearOutput();

            OutputText("Scenes.Owca.RebeccMenu.KnockAndStepInside".Localized());

            OutputText("Scenes.Owca.RebeccMenu.YouCameBathReady".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), RebeccAppearance);

            AddButton(1, "Common.Button.Bath".Localized(), RebeccBathScene);

            if (Player.IsCorruptEnough(66) || Player.HasPerk(PerkIds.Sadist))
            {
                OutputText("Scenes.Owca.RebeccMenu.CouldTryAndForce".Localized());

                AddButton(3, "Common.Button.Rape".Localized(), RapeRebeccCheck);
            }
            else
                AddButton(3, "Common.Button.Rape".Localized(), null);

            if (!Player.IsChild() && (OptionStore.Instance.Get("gore", 0) > 0))
                AddButton(4, "Common.Button.Snuff".Localized(), SnuffRebecc);

            AddButton(14, "Common.Button.Back".Localized(), GangbangVillageStuff);
        }

        public void RebeccAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Owca.RebeccAppearance.RebeccIsSheepGirl".Localized());

            OutputText("Scenes.Owca.RebeccAppearance.PairOfAmple".Localized());

            OutputText("Scenes.Owca.RebeccAppearance.PussyOfDubious".Localized());

            OutputText("Scenes.Owca.RebeccAppearance.CantSeeHerAsshole".Localized());

            DoNext(() => RebeccMenu());
        }

        public void RebeccBathScene()
        {
            ClearOutput();

            OutputText("Scenes.Owca.RebeccBathScene.GrabsHandAndLeads".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.WhisperingSeductivelyAdds".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.StaresWithInsistent".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.DontResistGrinning".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.LetMeWashYou".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.GrabsHoldOfNearby".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.RelaxNowSoldier".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.FirstSoftlyBrushesHair".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Owca.RebeccBathScene.SentencePeriod".Localized());
            else
            {
                if (Player.HasCock())
                    OutputText("Scenes.Owca.RebeccBathScene.GrowsFullyErect".Localized());

                if (Player.HasVagina() && Player.HasCock())
                    OutputText("Scenes.Owca.RebeccBathScene.AndWord".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Owca.RebeccBathScene.CommaSpace".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Owca.RebeccBathScene.LeaksATrickle".Localized());

                OutputText("Scenes.Owca.RebeccBathScene.DoesntSeemToNotice".Localized());
            }

            OutputText("Scenes.Owca.RebeccBathScene.YouReallyLikeThis".Localized());

            OutputText("Scenes.Owca.RebeccBathScene.BlushAndGroan".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 30f), new DynStat(GameStats.Corruption, -2f));

            Player.SlimeFeed();

            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), RebeccBathOral);

            AddNextButton("Common.Button.CleanEnough".Localized(), RebeccBathClean);
        }

        public void RebeccBathOral()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 0)
            {
                if (owcaAttitude > 50 || CoC.Instance.Flags.Get(GameFlagIds.DaysSinceLastDemonDealings) < 7)
                {
                    OutputText("Scenes.Owca.RebeccBathOral.SureOfEnjoyment".Localized());

                    OutputText("Scenes.Owca.RebeccBathOral.SaysTracingHerFinger".Localized());

                    OutputText("Scenes.Owca.RebeccBathOral.SighAndAccept".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
                else
                {
                    OutputText("Scenes.Owca.RebeccBathOral.BeenSomeTimeSince".Localized());

                    OutputText("Scenes.Owca.RebeccBathOral.BlushesAndAvertsGaze".Localized());

                    OutputText("Scenes.Owca.RebeccBathOral.SoSheSaysYet".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Owca.RebeccBathOral.DeserveItYouDo".Localized());

                OutputText("Scenes.Owca.RebeccBathOral.SheSays".Localized());
            }

            OutputText("Scenes.Owca.RebeccBathOral.ThatsEnoughConfirmation".Localized());

            OutputText("Scenes.Owca.RebeccBathOral.GigglingStopsToying".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Owca.RebeccBathOral.ObeysSucklingTheTip".Localized());

                OutputText("Scenes.Owca.RebeccBathOral.GrabTheSheepsHead".Localized());
            }
            else
            {
                OutputText("Scenes.Owca.RebeccBathOral.ObeysSucklingYourClit".Localized());

                OutputText("Scenes.Owca.RebeccBathOral.ReachingShiveringHighPoint".Localized());

                OutputText("Scenes.Owca.RebeccBathOral.OnEdgeAgain".Localized());
            }

            OutputText("Scenes.Owca.RebeccBathOral.SaysCoylyRebeccRises".Localized());

            Player.Orgasm("DickVaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void RebeccBathClean()
        {
            ClearOutput();

            OutputText("Scenes.Owca.RebeccBathClean.HandsStimulateAndArouse".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RapeRebeccCheck()
        {
            ClearOutput();

            OutputText("Scenes.Owca.RapeRebeccCheck.SureYouWantThis".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => RapeRebecc());

            AddButton(1, "Common.Button.No".Localized(), RebeccMenu);
        }

        public void RapeRebecc(bool outside = false)
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Owca.RapeRebecc.WithinThreeStridesUpon".Localized());

                OutputText("Scenes.Owca.RapeRebecc.QuicklyProceedToDenude".Localized());

                OutputText("Scenes.Owca.RapeRebecc.SightGloriousRear".Localized());

                if (Player.Cocks[0].Length > 12)
                    OutputText("Scenes.Owca.RapeRebecc.BuryCockDeeper".Localized());
                else
                    OutputText("Scenes.Owca.RapeRebecc.EntiretyOfYourPrick".Localized());

                OutputText("Scenes.Owca.RapeRebecc.TrillsAndBleats".Localized());

                OutputText("Scenes.Owca.RapeRebecc.AfterFewMinutesResistance".Localized(
                    Player.CockDescript(0)
                ));

                OutputText("Scenes.Owca.RapeRebecc.CadencedWobblingOfRump".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Owca.RapeRebecc.AlmostFeelSoreFrom".Localized());

                OutputText("Scenes.Owca.RapeRebecc.FinallyWhenYouThink".Localized());

                if (Player.CumQ() > 1500)
                    OutputText("Scenes.Owca.RapeRebecc.AfterFillingUpRectum".Localized());

                OutputText("Scenes.Owca.RapeRebecc.UnconsciouslyMilksYou".Localized());

                OutputText("Scenes.Owca.RapeRebecc.WithLastSatisfiedSigh".Localized());

                if (outside)
                    OutputText("Scenes.Owca.RapeRebecc.SomeVillagersApathetically".Localized());

                OutputText("Scenes.Owca.RapeRebecc.SilentlyRemoveYourPrick".Localized());

                OutputText("Scenes.Owca.RapeRebecc.MoreThanAppropriate".Localized());

                if (outside)
                    OutputText("Scenes.Owca.RapeRebecc.ToMiserablePeopleAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.ManipulativeSlutAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.OwcaRemovedFromPlaces".Localized());
            }
            else
            {
                OutputText("Scenes.Owca.RapeRebecc.WithinThreeStridesAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.QuicklyProceedToUndress".Localized());

                OutputText("Scenes.Owca.RapeRebecc.KeepRubbingAgainstPlush".Localized());

                if (Player.Tone > 75)
                    OutputText("Scenes.Owca.RapeRebecc.SheepGirlsPlushSoftness".Localized());
                else
                    OutputText("Scenes.Owca.RapeRebecc.GirlHasFineAss".Localized());

                OutputText("Scenes.Owca.RapeRebecc.RebeccDoesntSayAnything".Localized());

                OutputText("Scenes.Owca.RapeRebecc.ShoveHerTitsFirst".Localized());

                OutputText("Scenes.Owca.RapeRebecc.WetSoundsFillAir".Localized());

                if (Player.GetClitLength() > 3)
                    OutputText("Scenes.Owca.RapeRebecc.ArentDoneYetYour".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.Owca.RapeRebecc.ArentDoneYetAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.EventuallyReachYourHigh".Localized());

                if (outside)
                    OutputText("Scenes.Owca.RapeRebecc.SomeVillagersAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.SilentlyPickYourselfUp".Localized());

                if (outside)
                    OutputText("Scenes.Owca.RapeRebecc.ToMiserablePeopleAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.ManipulativeSlutAgain".Localized());

                OutputText("Scenes.Owca.RapeRebecc.OwcaRemovedAgain".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, -1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void DesperateVillages()
        {
            ClearOutput();

            OutputText("Scenes.Owca.DesperateVillages.AsYouApproachGroup".Localized());

            OutputText("Scenes.Owca.DesperateVillages.PutYouBackPit".Localized());

            OutputText("Scenes.Owca.DesperateVillages.RudelyPushedAside".Localized());

            OutputText("Scenes.Owca.DesperateVillages.SeemsTheseVillagersHave".Localized());

            OutputText("Scenes.Owca.DesperateVillages.DoYouSubmit".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), AcceptRebeccsPlea);

            AddButton(1, "Common.Button.No".Localized(), FightZeVillagers);
        }

        public void FightZeVillagers()
        {

            var farmers = EnemyRegistry.Create(new Identifier("coc", "farmers"));

            farmers.OnDefeated = _ => CombatScene.CleanupAfterCombat();

            farmers.OnWon = _ => CombatScene.CleanupAfterCombat();

            CombatScene.Start(farmers, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);

            CurrentMonster.CreateStatusEffect(StatusEffectIds.GenericRunDisabled, 0, 0, 0, 0);
        }

        public void BeatUpOwca()
        {
            ClearOutput();

            OutputText("Scenes.Owca.BeatUpOwca.LastVillagerDrops".Localized());

            ClearButtons();

            if (!Player.IsGenderless() && Player.Lust >= 33 && (Player.IsCorruptEnough(60) || Player.HasPerk(PerkIds.Sadist)))
                AddButton(0, "Common.Button.RapeRebecc".Localized(), () => RapeRebecc(true));
            else
                AddButton(0, "Common.Button.RapeRebecc".Localized(), null);

            if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire) ||                                                             Player.HasPerk(PerkIds.FireLord) || Player.HasPerk(PerkIds.Hellfire) || Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
                AddButton(1, "Common.Button.TorchVillage".Localized(), TorchOwcaMotherFuckers);
            else
                AddButton(1, "Common.Button.TorchVillage".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveOwcaAfterWhupping);
        }

        public void LeaveOwcaAfterWhupping()
        {

            CoC.Instance.Flags.Set(GameFlagIds.RebeccsLastPlea, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void TorchOwcaMotherFuckers()
        {
            ClearOutput();

            OutputText("Scenes.Owca.TorchOwcaMotherFuckers.IgnorantFolksDeserve".Localized());

            OutputText("Scenes.Owca.TorchOwcaMotherFuckers.NoticeRebeccWeeping".Localized());

            Player.Gems += 900 + Random.Shared.Next(150);

            CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, -1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 15f));

            ClearButtons();

            if (!Player.IsGenderless() && Player.Lust >= 33 && (Player.IsCorruptEnough(60) || Player.HasPerk(PerkIds.Sadist)))
                AddButton(0, "Common.Button.AbuseHer".Localized(), () => RapeRebecc(true));
            else
                AddButton(0, "Common.Button.AbuseHer".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), TorchUpVillagersAndLeave);
        }

        public void TorchUpVillagersAndLeave()
        {
            ClearOutput();

            OutputText("Scenes.Owca.TorchUpVillagersAndLeave.DontThinkYoure".Localized());

            Player.ChangeFatigue(100);

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToOwca()
        {
            ClearOutput();

            OutputText("Scenes.Owca.LoseToOwca.KneelDownAndFall".Localized());

            if (!Player.Weapon.IsAttached())
                OutputText("Scenes.Owca.LoseToOwca.YourIsTakenAway".Localized());

            OutputText("Scenes.Owca.LoseToOwca.UncomfortablyTransported".Localized());

            Player.HealthChange(50);

            Player.ChangeFatigue(-30);

            LoseOrSubmitToVapula(true);
        }

        public void MorningAfterRape()
        {
            ClearOutput();

            OutputText("Scenes.Owca.MorningAfterRape.VillageUtterlySilent".Localized());

            OutputText("Scenes.Owca.MorningAfterRape.ForcedYouAbused".Localized());

            OutputText("Scenes.Owca.MorningAfterRape.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Forgive".Localized(), ForgiveOwca);

            AddButton(1, "Common.Button.Rape".Localized(), () => RapeRebecc(false));

            AddButton(2, "Common.Button.Leave".Localized(), FuckThisShit);
        }

        public void ForgiveOwca()
        {
            ClearOutput();

            OutputText("Scenes.Owca.ForgiveOwca.SwallowingYourPride".Localized());

            if (Player.Corruption >= 50)
                OutputText("Scenes.Owca.ForgiveOwca.LyingThroughYourTeeth".Localized());

            OutputText("Scenes.Owca.ForgiveOwca.NoResentmentFelt".Localized());

            OutputText("Scenes.Owca.ForgiveOwca.RebeccsFaceOvercome".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f), new DynStat(GameStats.Corruption, -8f));

            owcaAttitude = 60;

            CoC.Instance.Flags.Set(GameFlagIds.OwcaAngerDisabled, 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => GangbangVillageStuff());
        }

        public void FuckThisShit()
        {
            ClearOutput();

            OutputText("Scenes.Owca.FuckThisShit.StareAtWretchedWhimpering".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.RebeccsLastPlea, 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void RebeccsLastPlea()
        {
            ClearOutput();

            OutputText("Scenes.Owca.RebeccsLastPlea.ArriveOnTheBorder".Localized());

            OutputText("Scenes.Owca.RebeccsLastPlea.WailsChokingBack".Localized());

            OutputText("Scenes.Owca.RebeccsLastPlea.WhatDoYouDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.RapeHer".Localized(), () => RapeRebecc(true));

            AddButton(1, "Common.Button.FaceThemAll".Localized(), FaceDownHordes);

            AddButton(2, "Common.Button.Leave".Localized(), LeaveRebeccToBeRaped);
        }

        public void LeaveRebeccToBeRaped()
        {
            ClearOutput();

            OutputText("Scenes.Owca.LeaveRebeccToBeRaped.TellRebeccYouDont".Localized());

            OutputText("Scenes.Owca.LeaveRebeccToBeRaped.OwcaHasBeenRemoved".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void FaceDownHordes()
        {
            ClearOutput();

            OutputText("Scenes.Owca.FaceDownHordes.TellRebeccToHide".Localized());

            OutputText("Scenes.Owca.FaceDownHordes.TellHerNotWorry".Localized());

            OutputText("Scenes.Owca.FaceDownHordes.ThenReadyYourself".Localized());

            OutputText("Scenes.Owca.FaceDownHordes.VillagersKeepStaring".Localized());

            FightZeDemons(false);
        }

        public void SlaveToVapulaBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.WakeUpTiedGagged".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.LittlePetWoken".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.SitUpUncomfortably".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.SnapsHerFingers".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.CaptivatingSuccubusApproaches".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.ForASecond".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.GaspForAir".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.LikedThatDidntYou".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.ViolentThrustTheCock".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.DidntHearYou".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.FinallyPullHeadBack".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.CantHearSayIt".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.YesSayIt".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.YesWhatSayIt".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.YesMistressVapula".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.SpontaneousDirtyWords".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.VeryWellSlut".Localized());

            OutputText("Scenes.Owca.SlaveToVapulaBadEnd.FuriouslyNodInApproval".Localized());

            GameOver();
        }

        public void SubdueVapula()
        {
            ClearOutput();

            OutputText("Scenes.Owca.SubdueVapula.FinalDemonFalls".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Owca.SubdueVapula.OverwhelmedByYourMight".Localized());
            else
                OutputText("Scenes.Owca.SubdueVapula.RenderedInsensate".Localized());

            OutputText("Scenes.Owca.SubdueVapula.AsUsualWalkAmong".Localized());

            OutputText("Scenes.Owca.SubdueVapula.GripHerByShoulders".Localized());

            OutputText("Scenes.Owca.SubdueVapula.ByNowCompletelyBroken".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Disband".Localized(), DisbandHorde);

            if (!Player.IsGenderless() && Player.IsCorruptEnough(66))
                AddButton(1, "Common.Button.Enslavevapula".Localized(), EnslaveVapulaWithYourWang);
            else
                AddButton(1, "Common.Button.Enslavevapula".Localized(), null);

            if (!Player.IsGenderless() && Player.Lust >= 33)
                AddButton(2, "Common.Button.Justfuckem".Localized(), RapeZeVapula);
            else
                AddButton(2, "Common.Button.Justfuckem".Localized(), null);

            AddButton(3, "Common.Button.SkipOut".Localized(), CombatScene.CleanupAfterCombat);

            AddButton(4, "Common.Button.Enough".Localized(), SmiteVapula);
        }

        public void SmiteVapula()
        {
            ClearOutput();

            OutputText("Scenes.Owca.SmiteVapula.StartSpeakingInCalm".Localized());

            OutputText("Scenes.Owca.SmiteVapula.EyesProgressivelyFilling".Localized());

            OutputText("Scenes.Owca.SmiteVapula.EyesTwitchInAnger".Localized());

            OutputText("Scenes.Owca.SmiteVapula.RaiseWeaponEyesWide".Localized());

            OutputText("Scenes.Owca.SmiteVapula.WhoLeadsThisBand".Localized());

            OutputText("Scenes.Owca.SmiteVapula.DemonsGetYourMessage".Localized());

            Player.UpgradeBeautifulSword();

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            owcaAttitude = 100;

            CoC.Instance.Flags.Set(GameFlagIds.OwcaSacrificeDisabled, 2);

            CombatScene.CleanupAfterCombat();
        }

        public void DisbandHorde()
        {
            ClearOutput();

            OutputText("Scenes.Owca.DisbandHorde.StartSpeakingInCalm".Localized());

            OutputText("Scenes.Owca.DisbandHorde.EyesProgressivelyFilling".Localized());

            OutputText("Scenes.Owca.DisbandHorde.GiveWantonSuccubusA".Localized());

            OutputText("Scenes.Owca.DisbandHorde.BegoneAllOfYou".Localized());

            OutputText("Scenes.Owca.DisbandHorde.NodsAgainInAcknowledgement".Localized());

            owcaAttitude = 100;

            CoC.Instance.Flags.Set(GameFlagIds.OwcaSacrificeDisabled, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void EnslaveVapulaWithYourWang()
        {
            ClearOutput();

            if (!Player.HasCock())
            {
                EnslaveVapulaAsACuntWielder();

                return ;
            }

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.SpeakInFeverishVoice".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.MentionDemonQueen".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.SoYoureARenegade".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.WeHadAReputation".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.LaughInDerision".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.SuccubusEyesWiden".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.VeryWellSure".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.ChampionTheyWillFollow".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.LongAsTheyDont".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.LendHandToSuccubus".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaWithYourWang.VapulaAddedToSlaves".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.VapulaFollower, 1);

            owcaAttitude = 100;

            CoC.Instance.Flags.Set(GameFlagIds.VapulaHaremFuck, 1);

            CoC.Instance.Flags.Set(GameFlagIds.OwcaSacrificeDisabled, 1);

            if (Player.Lust < 80)
                Player.Lust = 80;

            Player.ApplyStats(new DynStat(GameStats.Lust, 0.1f));

            CombatScene.CleanupAfterCombat();
        }

        public void EnslaveVapulaAsACuntWielder()
        {
            ClearOutput();

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.SpeakInFeverishVoice".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.MentionDemonLord".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.SoYoureARenegade".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.WeHadAReputation".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.LaughInDerision".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.VapulaFaceLightsUp".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.ButImA".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.ThatIsntGoodEnough".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.VapulaWails".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.LeaveHerHanging".Localized());

            OutputText("Scenes.Owca.EnslaveVapulaAsACuntWielder.WagsHeadEcstatic".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.VapulaFollower, .5f);

            owcaAttitude = 100;

            CoC.Instance.Flags.Set(GameFlagIds.OwcaSacrificeDisabled, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void SnuffRebecc()
        {
            ClearOutput();

            OutputText("Scenes.Owca.SnuffRebecc.LookingAtAppetizingSheep".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) == 0)
            {
                OutputText("Scenes.Owca.SnuffRebecc.ResolveToPlayAlong".Localized());

                OutputText("Scenes.Owca.SnuffRebecc.AllThisInMind".Localized());

                OutputText("Scenes.Owca.SnuffRebecc.AfterSeveralMoreMinutes".Localized());

                OutputText("Scenes.Owca.SnuffRebecc.LongerTalkMoreHope".Localized());
            }
            else
            {
                OutputText("Scenes.Owca.SnuffRebecc.ResolveToHumorHer".Localized());

                OutputText("Scenes.Owca.SnuffRebecc.ThisInMindAgain".Localized());

                OutputText("Scenes.Owca.SnuffRebecc.AfterMoreMinutesAgain".Localized());

                OutputText("Scenes.Owca.SnuffRebecc.LongerYouTalkAgain".Localized());
            }

            OutputText("Scenes.Owca.SnuffRebecc.LambToSlaughter".Localized());

            DoNext(() => SnuffRebecc2());
        }

        public void SnuffRebecc2()
        {
            ClearOutput();

            OutputText("Scenes.Owca.SnuffRebecc2.YouveBeenWalkingThrough".Localized());

            if (Player.Corruption < 20)
                OutputText("Scenes.Owca.SnuffRebecc2.OhGodsWhatDoing".Localized());
            else
                OutputText("Scenes.Owca.SnuffRebecc2.GodsYoureEnjoying".Localized());

            OutputText("Scenes.Owca.SnuffRebecc2.JustLookAtHer".Localized());

            OutputText("Scenes.Owca.SnuffRebecc2.GrassThickEnough".Localized());

            OutputText("Scenes.Owca.SnuffRebecc2.SheMumblesReassuringSmile".Localized());

            OutputText("Scenes.Owca.SnuffRebecc2.SmileHasntLeftYour".Localized());

            OutputText("Scenes.Owca.SnuffRebecc2.SuddenScrambleBack".Localized());

            OutputText("Scenes.Owca.SnuffRebecc2.BendPickKnife".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.EndHerNow".Localized(), EndHerNow);

            AddButton(1, "Common.Button.EnjoyBody".Localized(), EnjoyHerBody);
        }

        public void EndHerNow()
        {
            ClearOutput();

            OutputText("Scenes.Owca.EndHerNow.NowIsTheTime".Localized());

            OutputText("Scenes.Owca.EndHerNow.FlailingHandsBeat".Localized());

            OutputText("Scenes.Owca.EndHerNow.ReleaseYourGrip".Localized());

            OutputText("Scenes.Owca.EndHerNow.ShesPerfectLikeThis".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 20f));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) > 0)
                OutputText("Scenes.Owca.EndHerNow.OwcaHasBeenRemoved".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, -1);

            Save.SetBool("rebeccKilled", true);

            DoNext(() => ReturnToCamp(1));
        }

        public void EnjoyHerBody()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            OutputText("Scenes.Owca.EnjoyHerBody.ExtractEveryBit".Localized());

            OutputText("Scenes.Owca.EnjoyHerBody.BladeIsntSharpEnough".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Owca.EnjoyHerBody.NowhereNearDoneYet".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.BothVoicesRingOut".Localized((Player.ThickestCockThickness() > 2.5 ? "your massive member stretches and tears her walls, a stream" : "you hilt yourself in the whimpering sheep, a trickle")));

                OutputText("Scenes.Owca.EnjoyHerBody.HandsRoamOverClaim".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.EndIsComing".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.MiraculouslyYouReach".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.AfterYoureSpent".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.Owca.EnjoyHerBody.NowhereNearDoneAgain".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.BrandishTheDullKnife".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.RavenousWolfishGrin".Localized());

                Player.CuntChange(3, true, true);

                OutputText("Scenes.Owca.EnjoyHerBody.GrindingHandsRoam".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.EndIsComingAgain".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.MiraculouslyAgain".Localized());

                OutputText("Scenes.Owca.EnjoyHerBody.AfterSpentAgain".Localized());

                Player.Orgasm("Vaginal");
            }

            OutputText("Scenes.Owca.EnjoyHerBody.ShesPerfectForever".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 20f));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInDemonPit) > 0)
                OutputText("Scenes.Owca.EnjoyHerBody.OwcaHasBeenRemoved".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.OwcaUnlocked, -1);

            Save.SetBool("rebeccKilled", true);

            DoNext(() => ReturnToCamp(1));
        }

    }
}