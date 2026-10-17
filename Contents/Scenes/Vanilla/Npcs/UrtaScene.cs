using MonoCoC.Attributes;
using MonoCoC.Utils;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Desert;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Contents.Achievements;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<UrtaScene>("coc:urta")]
    public class UrtaScene : Scene, ITimeAware
    {
        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Urta)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public override void Entry()
        {
            UrtaSprite();
        }

        public bool TimeChange()
        {
            var needNext = false;

            drainedByKath = false;

            Pregnancy.PregnancyAdvance();

            if (CoC.Instance.Flags.Get(GameFlagIds.NeedUrtaLetter) == 1 && View.Time.Hours == 6)
                SceneInstance<UrtaPregsScene>.Get().GetUrtaLetter();

            if (Pregnancy.Incubation == 0
                && (Pregnancy.Type == PregnancyIds.BeeEggs || Pregnancy.Type == PregnancyIds.DriderEggs))
            {
                Pregnancy.KnockUpForce();

                CoC.Instance.Flags.Set(GameFlagIds.UrtaEggs, 0);

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFertileEggs, 0);
            }

            var sinceLastCame = CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame);

            if (sinceLastCame > 0)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, sinceLastCame - 1);

            var eggForceEvent = CoC.Instance.Flags.Get(GameFlagIds.UrtaEggForceEvent);

            if (eggForceEvent > 0)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaEggForceEvent, eggForceEvent - 1);

            if (eggForceEvent > 0 && View.Time.Hours > 6 && View.Time.Hours < 18 && eggForceEvent < 12)
            {
                OutputText("Scenes.Urtascene.TimeChange.CheckOnUrta".Localized());

                UrtaChewsOutPC(false);

                needNext = true;
            }

            var angry = CoC.Instance.Flags.Get(GameFlagIds.UrtaAngryAtPcCountdown);

            if (angry > 1)
            {
                angry -= 1;

                if (angry < 1)
                    angry = 1;

                CoC.Instance.Flags.Set(GameFlagIds.UrtaAngryAtPcCountdown, angry);
            }

            if (View.Time.Hours > 23)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaCumNoCumDays, CoC.Instance.Flags.Get(GameFlagIds.UrtaCumNoCumDays) + 1);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaCumNoCumDays, 0);

                var affection = CoC.Instance.Flags.Get(GameFlagIds.UrtaPcAffectionCounter);

                if (affection > 0)
                {
                    affection -= 0.25f;

                    if (affection < 0)
                        affection = 0;

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, affection);
                }
            }

            return needNext;
        }

        public bool drainedByKath = false;

        public bool TimeChangeLarge() => false;

        public void UrtaSprite()
        {

        }

        public float UrtaCapacity()
        {

            int bonus = CoC.Instance.Flags.Get(GameFlagIds.TimesRutFuckedUrtasCunt) * 5;

            if (bonus > 40)
                bonus = 40;

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTentacleGaped) > 0)
                return 500;

            return 60 + bonus;
        }

        public bool UrtaLove(float love = 0)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == -1)
                return false;

            if (love == 0)
                return CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1;

            if (love > 0)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, CoC.Instance.Flags.Get(GameFlagIds.UrtaPcAffectionCounter) + love);

            if (love < 0)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, CoC.Instance.Flags.Get(GameFlagIds.UrtaPcAffectionCounter) - love);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
            {
                if (love > 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 0 && SceneInstance<AmilyScene>.Get().AmilyFollowerPure() && !(OptionStore.Instance.Get("urtaDisabled", 0) > 0))
                    CoC.Instance.Flags.Set(GameFlagIds.AmilyNeedToFreakAboutUrta, 1);

                return true;
            }
            else
                return false;
        }

        public bool UrtaAvailableForSex()
        {

            return UrtaFuckbuddy() && SceneInstance<ScyllaScene>.Get().action != ScyllaScene.SCYLLA_ACTION_FUCKING_URTA && CoC.Instance.Flags.Get(GameFlagIds.UrtaAngryAtPcCountdown) == 0;
        }

        public bool UrtaFuckbuddy()
        {

            if (SceneInstance<UrtaQuestScene>.Get().UrtaBusy() || CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == -1)
                return false;

            if (UrtaJustFriends())
                return false;

            return CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) > 0;
        }

        public bool UrtaJustFriends()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == -1;
        }

        public bool UrtaAtBar()
        {

            // 原版 UrtaScene.hx urtaAtBar()：Urta 在酒吧且无占用时才出现
            return !GameSettings.UrtaDisabled

                && !SceneInstance<UrtaQuestScene>.Get().UrtaBusy()

                && CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) != 1

                && View.Time.Hours < 15

                && CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) != 3;
        }

        public bool UrtaDrunk()
        {

            if (Player.IsPregnant())
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) == -1)
                return false;

            if (View.Time.Hours > 12 && CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) < 1)
                return true;

            if (View.Time.Hours > 8 && CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) >= 1)
                return true;
            else
                return false;
        }

        public bool UrtaOpenAboutEdryn()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaConfrontedScylla) > 0)
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKnowsPcHasMarbleFollower) > 0)
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaOpenAboutEdryn) > 0)
                return true;

            return false;
        }

        public void KnockUpUrtaChance()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) != 0 && Player.KnockUpChance(10, 50))
            {
                Player.KnockUp(PregnancyIds.PlayerFather, 384, 100, 1);

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPregnantDeliveryScene, 0);
            }
        }

        public bool UrtaBarDescript()
        {

            if (GameSettings.UrtaDisabled)
                return false;

            OutputText("Scenes.Urtascene.UrtaBarDescript.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaAngryAtPcCountdown) > 1)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.SittingAtTable".Localized());

                return false;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaAngryAtPcCountdown) == 1)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.SittingAtTableAgain".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaAngryAtPcCountdown, 0);

                return true;
            }

            if (SceneInstance<RaphaelScene>.Get().FinaleBetrayed() && !SceneInstance<RaphaelScene>.Get().bountyReceived)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.EcstaticGrin".Localized());

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaAmilyFollowupNeeded) == 1)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.HoldingHeadInHands".Localized());

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == -1)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.BenchLegsCrossed".Localized());

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaConfrontedScylla) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) > 0)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.NormalTableWater".Localized());

                return true;
            }

            if (Player.IsPregnant())
            {
                if (Player.Pregnancy.Type == PregnancyIds.PlayerFather)
                    SceneInstance<UrtaPregsScene>.Get().UrtaPregAppearance();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaEggs) < 20)
                        OutputText("Scenes.Urtascene.UrtaBarDescript.QuietlyNonAlcoholic".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaEggs) < 40)
                            OutputText("Scenes.Urtascene.UrtaBarDescript.SwollenStomachSeat".Localized());
                        else
                            OutputText("Scenes.Urtascene.UrtaBarDescript.BloatedBellySideSaddle".Localized());

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) > 0 && !Player.IsPregnant() && CoC.Instance.Flags.Get(GameFlagIds.UrtaFlatbellyNotice) == 0)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.ChuggingMugs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFlatbellyNotice, 1);

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) == -1 || !UrtaDrunk())
                    OutputText("Scenes.Urtascene.UrtaBarDescript.FormFittingEveningGown".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaBarDescript.ShimmeringBlackDress".Localized());

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) == 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > -1)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.BenchFeetPropped".Localized());

                return true;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) > 0)
            {
                OutputText("Scenes.Urtascene.UrtaBarDescript.BenchSmallBelly".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 11)
                    OutputText("Scenes.Urtascene.UrtaBarDescript.OtherAdditionTrapped".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaBarDescript.HalfHardHorseCock".Localized());

                return true;
            }

            CoCSettings.Error("URTA Error: Bar descript encountered unexpected scenario.");

            return false;
        }

        public void UrtaBarApproach()
        {
            ClearOutput();

            UrtaSprite();

            Action eggs = null;

            Action spank = null;

            if (Player.HasCockThatFits(UrtaCapacity()) || Player.HasKeyItem("Deluxe Dildo"))
                spank = SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs;

            if (SceneInstance<RaphaelScene>.Get().FinaleBetrayed() && !SceneInstance<RaphaelScene>.Get().bountyReceived)
            {
                SceneInstance<RaphaelScene>.Get().bountyReceived = true;

                Player.GainGems(1000);

                ShowImage("urta-bar");

                OutputText("Scenes.Urtascene.UrtaBarApproach.ClapsWalkUp".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.SmirkAtTale".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.FishesOutPouch".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.FoxKnocksBackSwig".Localized());

                UrtaLove(3);

                ReturnToCamp(1);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaAmilyFollowupNeeded) == 1)
            {
                AmilyXUrtaUrtaFallout();

                return ;
            }

            if (UrtaJustFriends())
            {
                FriendsTalkExpack();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == -1)
            {
                OutputText("Scenes.Urtascene.UrtaBarApproach.SlamsBottleDown".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.NothingButLeave".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaConfrontedScylla) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) > 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.UrtaConfrontedScylla, 1);

                OutputText("Scenes.Urtascene.UrtaBarApproach.ApprehensiveGaze".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.ApologizeOtherNight".Localized());

                if (UrtaLove())
                    OutputText("Scenes.Urtascene.UrtaBarApproach.DrunkHornyScyllaOffer".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11)
                        OutputText("Scenes.Urtascene.UrtaBarApproach.ReallyDrunkScyllaSexy".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaBarApproach.DrunkHornyPersuasive".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.SayEndParagraphBreak".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.SnapsMouthShut".Localized());

                if (UrtaLove())
                    OutputText("Scenes.Urtascene.UrtaBarApproach.WorriedExpression".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 12)
                        OutputText("Scenes.Urtascene.UrtaBarApproach.AnxiouslyFragment".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaBarApproach.NervouslyFragment".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.AwaitsReply".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.NoScylla".Localized(), TellUrtaNoMoreScylla);

                AddButton(1, "Common.Button.ScyllaOkay".Localized(), TellUrtaMoreScyllaIsFine);

                AddButton(2, "Common.Button.LeaveHer".Localized(), LeaveUrtaAfterScyllaConfrontation);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaEggForceEvent) > 0)
            {
                UrtaChewsOutPC();

                return ;
            }

            if (SceneInstance<KatherineEmploymentScene>.Get().CanTalkToUrta())
            {
                SceneInstance<KatherineEmploymentScene>.Get().TalkToUrta();

                return ;
            }

            if (Player.Pregnancy.Type == PregnancyIds.PlayerFather)
            {
                SceneInstance<UrtaPregsScene>.Get().UrtaPreggoApproached();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 5 && ((Player.InRut && Player.HasCock()) || (Player.InHeat && Player.HasVagina())))
            {
                if (UrtaDrunk())
                    SceneInstance<UrtaHeatRutScene>.Get().ApproachDrunkenUrta();
                else
                    SceneInstance<UrtaHeatRutScene>.Get().ApproachSoberUrtaHeatRutProc();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) > 0 && !UrtaDrunk())
            {
                ShowImage("urta-bar");

                if (SceneInstance<MarbleScene>.Get().MarbleAtCamp() && CoC.Instance.Flags.Get(GameFlagIds.UrtaKnowsPcHasMarbleFollower) == 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKnowsPcHasMarbleFollower, 1);

                    MarbleCockuBlockuUrta();

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcAffectionCounter) >= 30 && CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 0)
                {
                    UrtaTwuWuvOffer();

                    return ;
                }

                QBsTalkExpack();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) == -1 || !UrtaDrunk())
                {
                    ShowImage("urta-bar-drunk");

                    OutputText("Scenes.Urtascene.UrtaBarApproach.DressTents".Localized());

                    OutputText("Scenes.Urtascene.UrtaBarApproach.HerPlaceUnderTable".Localized());

                    if (Player.CanOviposit())
                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) == 0)
                            OutputText("Scenes.Urtascene.UrtaBarApproach.ContemplateAskingHelp".Localized());
                        else
                            OutputText("Scenes.Urtascene.UrtaBarApproach.UnlikelyToHostEggs".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.HerPlace".Localized(), GoBackToUrtasForLuvinz);

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaCumNoCumDays) >= 5)
                        AddButton(1, "Common.Button.SuckOff".Localized(), SlurpFawkesCocksForFunAndInflation);
                    else
                        AddButton(1, "Common.Button.SuckOff".Localized(), BlowUrtaUnderTheTableLuv);

                    AddButton(2, "Common.Button.EatOut".Localized(), EatUrtaOutNomNomPussy);

                    if (Player.HasCock() && !Player.IsTaur())
                        AddButton(3, "Common.Button.VixenCream".Localized(), () => VixenAndCreamPart1(false));
                    else
                        AddButton(3, "Common.Button.VixenCream".Localized(), null);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));

                    return ;
                }
                else
                {
                    ShowImage("urta-bar-drunk");

                    OutputText("Scenes.Urtascene.UrtaBarApproach.SloshedLatchOn".Localized());

                    OutputText("Scenes.Urtascene.UrtaBarApproach.LetHerMakeShow".Localized());

                    eggs = null;

                    if (Player.CanOviposit())
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) == 0)
                            OutputText("Scenes.Urtascene.UrtaBarApproach.WantsFuckBadly".Localized());

                        eggs = GiveTheFoxSomeEggs;
                    }

                    ClearButtons();

                    AddButton(0, "Common.Button.Jerkoff".Localized(), GetAPublicFacialFromUrta);

                    AddButton(1, "Common.Button.AnalRide".Localized(), TakeUrtaInTheButtPublically);

                    AddButton(2, "Common.Button.LayEggs".Localized(), eggs);

                    AddButton(3, "Common.Button.SpankHer".Localized(), spank);

                    if (SceneInstance<RaphaelScene>.Get().FinaleCovered() && SceneInstance<RaphaelScene>.Get().RaphaelLikes() && CoC.Instance.Flags.Get(GameFlagIds.UrtaXRaphaelHappened) == 0)
                    {
                        OutputText("Scenes.Urtascene.UrtaBarApproach.CatsAndDogs".Localized());

                        AddButton(4, "Common.Button.Somesurprise".Localized(), UrtaAndRaphaelSurprise);
                    }

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));

                    return ;
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > -1)
            {
                ShowImage("urta-bar-drunk");

                OutputText("Scenes.Urtascene.UrtaBarApproach.SlurpsWhiskey".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.SomethingThumpsTable".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.LookingOverShoulder".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Backdoor".Localized(), UrtaFollowedOutBack);

                AddButton(1, "Common.Button.DrinkBooze".Localized(), DrinkUrtasBooze);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) != -1 && UrtaDrunk())
            {
                ShowImage("urta-bar-drunk");

                OutputText("Scenes.Urtascene.UrtaBarApproach.DrunkenGropesWhispers".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.BeastLooseBounce".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.JerkOffFace".Localized());

                eggs = null;

                if (Player.CanOviposit())
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) == 0)
                        OutputText("Scenes.Urtascene.UrtaBarApproach.WantsFuckBadlyAgain".Localized());

                    eggs = GiveTheFoxSomeEggs;
                }

                ClearButtons();

                AddButton(0, "Common.Button.Jerkoff".Localized(), GetAPublicFacialFromUrta);

                AddButton(1, "Common.Button.AnalRide".Localized(), TakeUrtaInTheButtPublically);

                AddButton(2, "Common.Button.LayEggs".Localized(), eggs);

                AddButton(3, "Common.Button.SpankHer".Localized(), spank);

                if (SceneInstance<RaphaelScene>.Get().FinaleCovered() && SceneInstance<RaphaelScene>.Get().RaphaelLikes() && CoC.Instance.Flags.Get(GameFlagIds.UrtaXRaphaelHappened) == 0)
                {
                    OutputText("Scenes.Urtascene.UrtaBarApproach.CatsAndDogsAgain".Localized());

                    AddButton(4, "Common.Button.Somesurprise".Localized(), UrtaAndRaphaelSurprise);
                }

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11 && Random.Shared.Next(2) == 0)
            {
                ShowImage("urta-bar");

                OutputText("Scenes.Urtascene.UrtaBarApproach.NaughtyWink".Localized());

                OutputText("Scenes.Urtascene.UrtaBarApproach.SuckOffUnderTable".Localized());
            }
            else
            {
                ShowImage("urta-bar");

                OutputText("Scenes.Urtascene.UrtaBarApproach.LasciviousGrin".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 11)
                    OutputText("Scenes.Urtascene.UrtaBarApproach.HorseCockThumps".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaBarApproach.PetsGrowingCock".Localized());

                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Urtascene.UrtaBarApproach.AsksBlushHorny".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaBarApproach.AsksGoBackPlay".Localized());
            }

            if (Player.CanOviposit())
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) == 0)
                    OutputText("Scenes.Urtascene.UrtaBarApproach.ContemplateAskingHelpAgain".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaBarApproach.UnlikelyToHostAgain".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.UrtasPlace".Localized(), GoBackToUrtasForLuvinz);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaCumNoCumDays) >= 5)
                AddButton(1, "Common.Button.HiddenBj".Localized(), SlurpFawkesCocksForFunAndInflation);
            else
                AddButton(1, "Common.Button.HiddenBj".Localized(), BlowUrtaUnderTable);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void DrinkUrtasBooze()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.DrinkUrtasBooze.GrabBottleSwig".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBooze.RushOutAlley".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f), new DynStat(GameStats.Lust, 30f));

            DrinkUrtasBoozePtTwo();
        }

        public void DrinkUrtasBoozePtTwo()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.DrinkUrtasBoozePtTwo.HappySighFragment".Localized());

            if (Player.HasVagina() && Player.Cocks.Count == 0)
                OutputText("Scenes.Urtascene.DrinkUrtasBoozePtTwo.SquatDownFragment".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozePtTwo.ReleasesPressure".Localized());
// 原版 Achievements.hx:183「Urine Trouble」
            AwardAchievement(AchievementIds.UrineTrouble);

            OutputText("Scenes.Urtascene.DrinkUrtasBoozePtTwo.TakesForeverStop".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozePtTwo.ShakesHorseMeat".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozePtTwo.StumblingHermRubbing".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Run".Localized(), DrinkUrtasBoozeRun);

            AddButton(1, "Common.Button.LetHer".Localized(), DrinkUrtasBoozeLetHer);
        }

        public void DrinkUrtasBoozeRun()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeRun.TurnAndRun".Localized());

            ReturnToCamp(1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);
        }

        public void DrinkUrtasBoozeLetHer()
        {
            ClearOutput();

            UrtaSprite();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 5);

            HideUpDown();

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ShowImage("urta-behindbar-fuck");

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.NodRelaxedIntrigued".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                if (Player.BiggestBreastSize() > 1)
                    OutputText(Appearance.BiggestBreastSizeDescript(Player));
                else
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.NipplesWord".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.TongueSpears".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PushedAgainstBench".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.TitsWord".Localized());
            else
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.BellyWord".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PullsArmorExposed".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.AndFragment".Localized(
                    Player.MultiCockDescript()
                ));

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.SnugglesNoseThighs".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.WetWord".Localized());
            else
                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.LubricatedLeakingPre".Localized());
                else
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.SuitablyLubricated".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.ShiftsPositionInside".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.CriesCondomMess".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.AgainstCervixTip".Localized());
            else
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.IntoRectum".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.UnloadingThickCream".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.WaitingWomb".Localized());
            else
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.AbusedAsshole".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.SentencePeriod".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 250)
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.BathingInSpunk".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.WonderKnockUp".Localized());
                else
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.BacksideFillsWarmth".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.BlastToCapacity".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.SucksEarEdge".Localized());

            if (Player.Cocks.Count > 0)
            {
                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.EachOfYFragment".Localized());
                else
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.YLetter".Localized());

                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.CocksSquirtBatch".Localized());

                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 100)
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PregnancySwollen".Localized());

                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.BellySquishThrust".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PussyDribblesWhiteness".Localized());
                else
                    OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.ClampsSpasming".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PuddleForms".Localized());
            else
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PullsOutDripping".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.StickItInMe".Localized());
            else
                OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.ComeBackBiggerLoad".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.PeriodSayEndBreak".Localized());

            OutputText("Scenes.Urtascene.DrinkUrtasBoozeLetHer.HelpsToFeet".Localized());

            if (Player.HasVagina())
                Player.CuntChange(60, true);
            else
                Player.ButtChange(60, true);

            ReturnToCamp(1);
        }

        public void UrtaFollowedOutBack()
        {
            ClearOutput();

            UrtaSprite();

            ShowImage("urta-behindbar-masti");

            OutputText("Scenes.Urtascene.UrtaFollowedOutBack.BackdoorNarrowAlley".Localized());

            OutputText("Scenes.Urtascene.UrtaFollowedOutBack.BegsGoDown".Localized());

            OutputText("Scenes.Urtascene.UrtaFollowedOutBack.FuckHerOrWait".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

            ClearButtons();

            AddButton(0, "Common.Button.SceneinstanceScorpionsceneGetFuckHer".Localized(), () => UrtaFuckHer());

            AddButton(1, "Common.Button.Mutualmasturb".Localized(), DualUrtaMasturbation);

            AddButton(2, "Common.Button.Watch".Localized(), WatchUrtaJerkIt);

            AddButton(14, "Common.Button.Leave".Localized(), UrtaSneakAwayFromMasturbate);
        }

        public void UrtaSneakAwayFromMasturbate()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaSneakAwayFromMasturbate.SneakAwayDepressing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void WatchUrtaJerkIt()
        {
            ClearOutput();

            UrtaSprite();

            ShowImage("urta-behindbar-masti-solo");

            OutputText("Scenes.Urtascene.WatchUrtaJerkIt.BothHandsPumping".Localized());

            OutputText("Scenes.Urtascene.WatchUrtaJerkIt.PullsBackRedEyed".Localized());

            OutputText("Scenes.Urtascene.WatchUrtaJerkIt.StandsPushesSkirt".Localized());

            OutputText("Scenes.Urtascene.WatchUrtaJerkIt.BetterHeadBack".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void DualUrtaMasturbation()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 5);

            ShowImage("urta-behindbar-masti-dual");

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.OutOfShadows".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.BlushExposing".Localized());
            else
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.RevelExposing".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.LooksUpInShock".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.WipeTearComfort".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText(Player.MultiCockDescriptLight());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.AndFragment".Localized());

                    OutputText(Player.VaginaDescript(0));
                }
            }
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript(0));
                else
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.BareGroinAnd".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.PeriodSpace".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.RiseToFullSize".Localized());

                if (Player.CockArea(0) < 40)
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.MemberDwarfsYou".Localized());
                else
                    if (Player.CockArea(0) < 70)
                        OutputText("Scenes.Urtascene.DualUrtaMasturbation.JustAsBig".Localized());
                    else
                        OutputText("Scenes.Urtascene.DualUrtaMasturbation.BiggerBurden".Localized());
            }

            if (Player.HasVagina())
                if (Player.Vaginas[0].Wetness > 3)
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.TrickleWetness".Localized());
                else
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.EngorgingLipsPart".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.HeatShowsOn".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText(Appearance.AllBreastsDescript(Player));
            else
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.ChestWord".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.NipplesFullSensitive".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.RelaxesAgainstWall".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.CaressCockFeverish".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.CaressRodsInTurn".Localized(
                    Player.MultiCockDescript()
                ));

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.DelveFemininePleasure".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.DelveTaintPenetrate".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.ReachesPeakMoan".Localized());

            if (Player.Cocks.Count > 0)
                if (Player.CumQ() < 250)
                {
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.JismSpurtsPuddle".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Urtascene.DualUrtaMasturbation.EachOfFragment".Localized());

                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.CocksEmpty".Localized());
                }
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Urtascene.DualUrtaMasturbation.JismPoursFloor".Localized());
                    else
                        if (Player.CumQ() < 1000)
                        {
                            OutputText("Scenes.Urtascene.DualUrtaMasturbation.JismEruptsFrom".Localized());

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Urtascene.DualUrtaMasturbation.EachOfFragmentAgain".Localized());

                            OutputText("Scenes.Urtascene.DualUrtaMasturbation.CocksBlastingAir".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Urtascene.DualUrtaMasturbation.JismFountainsFrom".Localized());

                            if (Player.Cocks.Count > 1)
                                OutputText("Scenes.Urtascene.DualUrtaMasturbation.EachOfFragmentThird".Localized());

                            OutputText("Scenes.Urtascene.DualUrtaMasturbation.FloodingAlleyway".Localized());
                        }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.SexSquishesFinger".Localized());

                if (Player.Vaginas[0].Wetness == 5)
                    OutputText("Scenes.Urtascene.DualUrtaMasturbation.GushingEverywhere".Localized());

                OutputText("Scenes.Urtascene.DualUrtaMasturbation.PeriodSpaceAgain".Localized());
            }
            else
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.SphincterClenches".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.SighingTweakNipple".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.AddingMassivePool".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.DrenchingHerself".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.RelievedSigh".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.StaggersUpDiscards".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.TipToesSpunk".Localized());
            else
                OutputText("Scenes.Urtascene.DualUrtaMasturbation.HeadsBackdoor".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.TailCurlsCock".Localized());

            OutputText("Scenes.Urtascene.DualUrtaMasturbation.CleaningUpStagger".Localized());

            ReturnToCamp(1);

            Player.Orgasm("Generic");
        }

        public void UrtaFuckHer(bool afterBefriending = false)
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 5);

            var cocks = Player.CockTotal();

            if (Player.Cocks.Count == 0)
            {
                ShowImage("urta-behindbar-female-fuckurta");

                Player.SlimeFeed();

                OutputText("Scenes.Urtascene.UrtaFuckHer.OutOfShadows".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Urtascene.UrtaFuckHer.BlushExposing".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaFuckHer.RevelExposing".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.LooksUpInShock".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.WipeTearAnswer".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.NodsNumbed".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.Horse || Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog)
                    OutputText("Scenes.Urtascene.UrtaFuckHer.TailAcrossNipples".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaFuckHer.MakingHerWait".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.BluntedTipPresses".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.SpreadsWide".Localized());

                if (Player.HasVagina())
                {
                    if (Player.VaginalCapacity() >= 60)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.SighSinkDown".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.NearlyCryOut".Localized());

                    Player.CuntChange(60, true, true, false);
                }
                else
                {
                    if (Player.AnalCapacity() >= 60)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.SighSinkDownAgain".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.NearlyCryOutAgain".Localized());

                    Player.ButtChange(60, true, true, false);
                }

                OutputText("Scenes.Urtascene.UrtaFuckHer.BabblesSoHot".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.SmirkBounceBalls".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.UrtaFuckHer.WombWord".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaFuckHer.RectumWord".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.TongueLollingDrool".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.FlareFormsPlug".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.StandUpGaspRush".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.BlowsKiss".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.SoonerBetter".Localized());
            }
            else
            {
                int x = Player.CockThatFits(UrtaCapacity());

                var y = Player.CockThatFits2(UrtaCapacity());

                if (x < 0)
                    x = 0;

                ShowImage("urta-behindbar-male-fuckurta");

                OutputText("Scenes.Urtascene.UrtaFuckHer.OutOfShadowsAgain".Localized());

                if (Player.Corruption < 60)
                    OutputText("Scenes.Urtascene.UrtaFuckHer.BlushExposingAgain".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaFuckHer.RevelExposingAgain".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.UrtaLooksShockAgain".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.WipeTearAnswerAgain".Localized());

                OutputText("Scenes.Urtascene.UrtaFuckHer.ShockedLookBends".Localized());

                if (Player.CockTotal() > 0)
                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.PairCocksAgainst".Localized());
                    else
                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Urtascene.UrtaFuckHer.CocksPussyAsshole".Localized());
                        else
                            if (Player.CockTotal() == 1)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.CocksAgainstPussy".Localized());

                if (Player.CockArea(x) < UrtaCapacity())
                {
                    OutputText("Scenes.Urtascene.UrtaFuckHer.WetEntranceYields".Localized());

                    if (Player.CockTotal() >= 2)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.BackdoorRougher".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.MoaningStrokingCock".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.HoleWord".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.HolesWord".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.SentencePeriodParagraphBreak".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.GrindingBegging".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.ReleaseBreastSmack".Localized());

                    if (cocks == 1)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.FuckWord".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.DoublePenetrateWord".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.HerFragment".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.CriesCumming".Localized());

                    if (cocks > 1 && y >= 0)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.AndPlaceholder".Localized(
                            Player.CockDescript(x),

                            Player.CockDescript(y)
                        ));
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.AndPlaceholderAgain".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Urtascene.UrtaFuckHer.HandsLockedCock".Localized(
                        Player.CockDescript(x)
                    ));

                    if (cocks > 1 && y >= 0)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.AboutToExplode".Localized(
                            Player.CockDescript(y)
                        ));

                    OutputText("Scenes.Urtascene.UrtaFuckHer.ParagraphBreak".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.SavagelyAbuseHole".Localized());

                    if (cocks > 1 && y >= 0)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.SLetter".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.ClimaxBurstsWave".Localized());

                    if (cocks > 1 && y >= 0)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.ClenchedAssSeem".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.SeemsWord".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.TryingToMilk".Localized());

                    if (Player.CumQ() < 250)
                    {
                        OutputText("Scenes.Urtascene.UrtaFuckHer.ThrustsSpentPullFree".Localized());

                        if (cocks > 1 && y >= 0)
                            OutputText("Scenes.Urtascene.UrtaFuckHer.SlowlyClosingPucker".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.SentencePeriod".Localized());
                    }
                    else
                        if (Player.CumQ() < 500)
                        {
                            OutputText("Scenes.Urtascene.UrtaFuckHer.CumPackingWomb".Localized());

                            if (cocks > 1 && y >= 0)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.AndGut".Localized());

                            OutputText("Scenes.Urtascene.UrtaFuckHer.FullOfSpooge".Localized());

                            if (cocks > 1 && y >= 0)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.SlowlyClosingPuckerAgain".Localized());

                            OutputText("Scenes.Urtascene.UrtaFuckHer.SentencePeriodAgain".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Urtascene.UrtaFuckHer.UrtaWomb".Localized());

                            if (cocks > 1 && y >= 0)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.AndBellyAre".Localized());
                            else
                                OutputText("Scenes.Urtascene.UrtaFuckHer.IsWord".Localized());

                            OutputText("Scenes.Urtascene.UrtaFuckHer.PositivelyPregnant".Localized());

                            if (cocks > 1 && y >= 0)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.BothHerHoles".Localized());
                            else
                                OutputText("Scenes.Urtascene.UrtaFuckHer.HerAbusedHole".Localized());
                        }

                    OutputText("Scenes.Urtascene.UrtaFuckHer.MoansLetshAgain".Localized());

                    if (Player.CumQ() > 1000)
                        OutputText("Scenes.Urtascene.UrtaFuckHer.TipToesSpunk".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaFuckHer.HeadsBackdoor".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.TailCurlsCock".Localized());

                    OutputText("Scenes.Urtascene.UrtaFuckHer.CleaningUpStagger".Localized());

                    KnockUpUrtaChance();
                }
                else
                {
                    if (cocks == 1)
                    {
                        OutputText("Scenes.Urtascene.UrtaFuckHer.HoleTooSmall".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.PullDownGrass".Localized(
                            Player.MultiCockDescript()
                        ));

                        OutputText("Scenes.Urtascene.UrtaFuckHer.PantsOutCums".Localized(
                            Player.MultiCockDescript()
                        ));

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Urtascene.UrtaFuckHer.JoinMeager".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.JoinMatchSpurt".Localized());
                            else
                                OutputText("Scenes.Urtascene.UrtaFuckHer.JoinFloodSpunk".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.GroansSpurtingWeakly".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.PicksHerselfUp".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.CleaningUpMosey".Localized());
                    }

                    if (cocks > 1)
                    {
                        OutputText("Scenes.Urtascene.UrtaFuckHer.HolesTooSmall".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.PullDownGrassAgain".Localized(
                            Player.MultiCockDescriptLight()
                        ));

                        OutputText("Scenes.Urtascene.UrtaFuckHer.PantsOutCumsAgain".Localized());

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Urtascene.UrtaFuckHer.JoinMeagerAgain".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.Urtascene.UrtaFuckHer.JoinMatchSpurtAgain".Localized());
                            else
                                OutputText("Scenes.Urtascene.UrtaFuckHer.JoinFloodSpunkAgain".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.GroansWeaklyAgain".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.PicksHerselfUpAgain".Localized());

                        OutputText("Scenes.Urtascene.UrtaFuckHer.CleaningUpMoseyAgain".Localized());
                    }
                }
            }

            if (afterBefriending) ReturnToCamp(1); else ReturnToCamp(1);
            Player.Orgasm("Generic");
        }

        public void BlowUrtaUnderTable()
        {
            ClearOutput();

            UrtaSprite();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(4));

            ShowImage("urta-bar-bj");

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.EyesWidenShock".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11)
                OutputText("Scenes.Urtascene.BlowUrtaUnderTable.PullingFishnet".Localized());
            else
                OutputText("Scenes.Urtascene.BlowUrtaUnderTable.PushingLeatherSkirt".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.WigglesInTime".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.ShotGlassSlams".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.WrithesSpillsBooze".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.GetHerOff".Localized());

            if (Player.Corruption < 70)
                OutputText("Scenes.Urtascene.BlowUrtaUnderTable.SuppressGagReflex".Localized());
            else
                OutputText("Scenes.Urtascene.BlowUrtaUnderTable.RammingMassiveCock".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.GruntsMuffleNoise".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.ChucklesOnlookers".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.LegsWrapBack".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.WaveThroughThroat".Localized());

            Player.RefillHunger(30);

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.ThumpingDownBack".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Urtascene.BlowUrtaUnderTable.DraggingBetween".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.Urtascene.BlowUrtaUnderTable.DraggingAcrossChest".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.SoftensWipeFace".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTable.ClimbedOutEyes".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, (float)(Random.Shared.Next(10) + 5 + Player.Libido / 10)));

            ReturnToCamp(1);
        }

        public void TakeUrtaInTheButtPublically()
        {
            ClearOutput();

            UrtaSprite();

            var tooBig = false;

            if (!Player.IsCorruptEnough(30) && SceneInstance<CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.NoWayInPublic".Localized());

                DisableButtonByLabel("Anal Ride");

                return ;
            }

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            UrtaLove(2);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 5);

            ShowImage("urta-public-fuck");

            if (Player.Corruption < 60 && SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.CantBelieveUndo".Localized());
            else
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.HappilyRemoveArmor".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.FlipsUpSkirt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) < 11)
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.UnwindsTail".Localized());
            else
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.FishesBeastOut".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.WatchingHorror".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.ShiftPositionClimb".Localized());

            if (Player.AnalCapacity() >= 60)
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.TightFitBackdoor".Localized());
            else
            {
                tooBig = true;

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.FarTooBig".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.NoticesDiscomfort".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SuspiciousBottle".Localized());
            }

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.GrowHardPressure".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.EachOfFragment".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SwellingErectness".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.EachWord".Localized());
                else
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.TheWord".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.TipLeaksShaft".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SLetter".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SqueezedFromProstate".Localized());
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.HeatSpreadsLoins".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.GetClitLength() < 2)
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.PeepOut".Localized());
                else
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.GrowFullSize".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.PeriodSpace".Localized());

                if (Player.Vaginas[0].Wetness < 3)
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.MoistureBeadsLips".Localized());
                else
                    if (Player.Vaginas[0].Wetness < 5)
                        OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.MoistureDripsThighs".Localized());
                    else
                        OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.MoisturePuddlesSeat".Localized());
            }

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.NipplesTentArmor".Localized());

            if (Player.HasFuckableNipples() && Player.BiggestLactation() >= 1)
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.MilkyLubricantsAching".Localized());
            else
                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.AchingToBePenetrated".Localized());
                else
                    if (Player.BiggestLactation() >= 1.5f && Player.BiggestLactation() < 2.5f)
                        OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.ThinStreamsMilk".Localized());
                    else
                        if (Player.BiggestLactation() >= 2.5f)
                            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.ThickStreamsMilk".Localized());
                        else
                            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SoftHandsGrip".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.EveryoneWatching".Localized());

            if (tooBig)
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.GigglingDrunkenly".Localized());
            else
                if (Player.Corruption < 50 && SceneInstance<CeraphScene>.Get().HasExhibition())
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.WhimperingRegret".Localized());
                else
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.HandsUnderArmor".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.BreathingHardEar".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.WetSquelches".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.JigglingWildly".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.DrippingLubricants".Localized());
                else
                    if (Player.BiggestLactation() > 1)
                        OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SpurtingMilk".Localized());
                    else
                        OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.RubNipplesArmor".Localized());

                OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SentencePeriodAgain".Localized());
            }

            if (Player.CockTotal() > 0)
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SteadyDrizzlesCum".Localized());
                else
                    OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SteadyDrizzlesCumAgain".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.CantHoldBack".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.PullDownCrushing".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.SighsSipNeck".Localized());

            OutputText("Scenes.Urtascene.TakeUrtaInTheButtPublically.ShrugRelaxCuddle".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void GetAPublicFacialFromUrta()
        {
            ClearOutput();

            UrtaSprite();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            UrtaLove(2);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 3 + Random.Shared.Next(3));

            ShowImage("urta-public-facial");

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.AcceptFacialChoice".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 11)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.SkirtWord".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.StockingsWord".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.HorseDickFlops".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.BlushCrimson".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.UncomfortableHorny".Localized());
                else
                    OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.RandyAsHell".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.DexterousHandsPump".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.PlayYourPart".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.EncourageSoak".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.ShyExpressionLick".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.ExtendTongueLick".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.LickOutsideEdge".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.RewardedPrecum".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.SwallowSweetPre".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.HopeShowArousing".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.ShudderShameHope".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.TremblesTipFlares".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.UrethraDilates".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.ChestWord".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.CleavageWord".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.PaintsYou".Localized());

            if (Player.Corruption > 60)
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.LickFingersClean".Localized());

            OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.SentencePeriodBreakAgain".Localized());

            if (Player.Corruption > 50)
            {
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.CrowdCoverRest".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), OptionalUrtaBukkake);

                AddButton(1, "Common.Button.No".Localized(), DeclineUrtaBukkake);

                return ;
            }
            else
            {
                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.BlushCleanUp".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11)
                    OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.StuffsBackFishnet".Localized());
                else
                    OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.CurlsTailBack".Localized());

                OutputText("Scenes.Urtascene.GetAPublicFacialFromUrta.SentencePeriod".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Random.Shared.Next(5) + Player.Libido / 10)));
            }

            ReturnToCamp(1);
        }

        public void OptionalUrtaBukkake()
        {
            ClearOutput();

            UrtaSprite();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) - 2);

            UrtaLove(-1);

            ShowImage("urta-bukkake");

            OutputText("Scenes.Urtascene.OptionalUrtaBukkake.TearOffArmor".Localized());

            OutputText("Scenes.Urtascene.OptionalUrtaBukkake.LeansBackSmile".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText(Appearance.AllBreastsDescript(Player));
            else
                OutputText("Scenes.Urtascene.OptionalUrtaBukkake.NipplesWord".Localized());

            OutputText("Scenes.Urtascene.OptionalUrtaBukkake.WolfHotLoad".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(15 + Random.Shared.Next(5) + Player.Libido / 10)));

            OutputText("Scenes.Urtascene.OptionalUrtaBukkake.HourLaterDrenched".Localized());

            ReturnToCamp(1);
        }

        public void DeclineUrtaBukkake()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.DeclineUrtaBukkake.BlushCleanUp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11)
                OutputText("Scenes.Urtascene.DeclineUrtaBukkake.StuffsBackFishnet".Localized());
            else
                OutputText("Scenes.Urtascene.DeclineUrtaBukkake.CurlsTailBack".Localized());

            OutputText("Scenes.Urtascene.DeclineUrtaBukkake.SentencePeriod".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Random.Shared.Next(5) + Player.Libido / 10)));

            ReturnToCamp(1);
        }

        public void GoBackToUrtasForLuvinz()
        {
            ClearOutput();

            UrtaSprite();

            ShowImage("urta-home-image");

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
            {
                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.WarmSmileHands".Localized());

                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.BrushAgainstBulge".Localized());

                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.DoorLoomPush".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPetplayDone) == 0 && UrtaLove())
                    OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.NewLeatherCollar".Localized());

                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.BustyFoxRetreated".Localized());

                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.FollowPreTrail".Localized());

                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.AnotherSheathHand".Localized());

                UrtaHomeLuvLuvinsMenu();

                return ;
            }

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.GratefulPeck".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.WalkingStrangelyPay".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.AssureNotOffended".Localized());

            if (Player.LibidoPercent > 50 || Player.Corruption > 50)
                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.TurnedOnThought".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinz.SmilesWeaklyHooks".Localized());

            GoBackToUrtasForLuvinzII();
        }

        public void GoBackToUrtasForLuvinzII()
        {

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.JourneyBriefArrive".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcSeenUrtaSexToys) == 0)
            {
                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.LastGroupNumerous".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.GiacomoMet) > 0)
                    OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.GiacomoWares".Localized());

                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.LonelyBeforeYou".Localized());
            }
            else
                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.CleanedHouse".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.CallsFromBedroom".Localized());

            if (Player.HasVagina() && Player.Cocks.Count > 0)
                OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.HardAndWet".Localized());
            else
                if (Player.Cocks.Count > 0)
                    OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.HardAsEver".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.WetAsEver".Localized());
                    else
                        OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.ReadyAsEver".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.GoBackToUrtasForLuvinzII.LookingPassiveRole".Localized());

            ClearButtons();

            if (Player.Cocks.Count > 0)
                AddButton(0, "Common.Button.SceneinstanceScorpionsceneGetFuckVagina".Localized(), DudeFuckingUrtasCooch);
            else
                AddButton(0, "Common.Button.SceneinstanceScorpionsceneGetFuckVagina".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.RideVaginal".Localized(), RideUrtaTenderFemale);
            else
                AddButton(1, "Common.Button.RideVaginal".Localized(), null);

            AddButton(2, "Common.Button.RideAnal".Localized(), TenderTakeItUpTheAssFromUrta);

            AddButton(3, "Common.Button.NoCondoms".Localized(), CondomlessUrtaInHouseSmex);

            if (Player.IsGoo() && Player.HasGooSkin())
                AddButton(4, "Common.Button.GooWeird".Localized(), UrtaGooTesticleVoreRuinedOrgasms);
            else
                AddButton(4, "Common.Button.GooWeird".Localized(), null);
        }

        public void RideUrtaTenderFemale()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.PcSeenUrtaSexToys, CoC.Instance.Flags.Get(GameFlagIds.PcSeenUrtaSexToys) + 1);

            UrtaLove(1.5f);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 3 + Random.Shared.Next(2));

            ShowImage("urta-home-female-ride");

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.ClimbBedRollOver".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.LikeThemMuch".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.YouNod".Localized());

            if (Player.Corruption < 50 || Player.LibidoPercent < 50)
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.SheepishlyWord".Localized());
            else
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.EagerlyWord".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.GrabLatexCock".Localized());

            if (Player.Vaginas[0].Wetness < 2)
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.GrowingMoistness".Localized());
            else
                if (Player.Vaginas[0].Wetness < 4)
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.SoppingWetness".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.FloodOfMoisture".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.SignalsReadiness".Localized());

            if (Player.Cocks.Count > 0)
                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.CockRagingHard".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.EachCockRaging".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.ClimbingAtopBed".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BubbleAtTip".Localized());

            if (Player.VaginalCapacity() < 60)
            {
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.HurtsAsItPushes".Localized());

                Player.CuntChange(60, true);
            }
            else
            {
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.StretchesWide".Localized());

                Player.CuntChange(60, true);
            }

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.GaspSighSinking".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.PrepuceRingPasses".Localized());

            if (Player.VaginalCapacity() < 60)
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BottomOutUnable".Localized());
            else
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.KeepGoingSliding".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.GrowlsGrabsNeck".Localized());

            if (Player.BiggestBreastSize() >= 5)
            {
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BreastsMash".Localized());

                if (!Player.HasFuckableNipples())
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.RubbingSkinnedBuds".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.NipplesSlipInside".Localized());

                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.SquealSuddenPleasure".Localized());

                if ((Player.Cocks.Count > 0) && (Player.Cocks[0].Length > 15))
                        if (Player.Cocks.Count == 1)
                            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.CockPushesBetween".Localized());
                        else
                            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.EachCockPushes".Localized());

                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BothCryOrgiastic".Localized());

            if (Player.Cocks.Count > 0)
            {
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.CumsBlastingSeed".Localized());

                    if (Player.Cocks[0].Length > 15 && Player.BiggestBreastSize() >= 5)
                        OutputText("Scenes.Urtascene.RideUrtaTenderFemale.JigglingTitsAgain".Localized());
                    else
                        OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BelliesWord".Localized());

                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.GlazingSlipperyGoo".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.EachCockCums".Localized());

                    if (Player.Cocks[0].Length > 15 && Player.BiggestBreastSize() >= 5)
                        OutputText("Scenes.Urtascene.RideUrtaTenderFemale.JigglingTitsAgain".Localized());
                    else
                        OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BelliesAgain".Localized());

                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.GlazingSlipperyGooAgain".Localized());
                }

                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.ParagraphBreakThird".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.LyingOnTop".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.IgnoringHotMess".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.HasWord".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaTenderFemale.HaveWord".Localized());

                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.LeftBetweenBodies".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.LongHappyKiss".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.PullsOffCondom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) <= 10 || CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 11)
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.LovemakingHassleClean".Localized());
            else
                OutputText("Scenes.Urtascene.RideUrtaTenderFemale.PonyFertileWish".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.BlushesRealizing".Localized());

            OutputText("Scenes.Urtascene.RideUrtaTenderFemale.NodWarmGlow".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void DudeFuckingUrtasCooch()
        {
            ClearOutput();

            UrtaSprite();

            int x = Player.CockThatFits(UrtaCapacity());

            if (x < 0)
                x = 0;

            var y = Player.CockThatFits2(UrtaCapacity());

            if (Player.CockArea(x) > UrtaCapacity())
            {
                FuckUrtasVagButTooBig();

                return ;
            }

            ShowImage("urta-home-male-fuck");

            var cocks = Player.Cocks.Count;

            CoC.Instance.Flags.Set(GameFlagIds.PcSeenUrtaSexToys, CoC.Instance.Flags.Get(GameFlagIds.PcSeenUrtaSexToys) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            UrtaLove(1.5f);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) - 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 1)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2);

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.LetHerKnowWant".Localized());

            if (cocks > 1 && y >= 0)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.SecondaryWobbles".Localized(
                    Player.CockDescript(y)
                ));

            if (cocks > 2)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.RemainderFanOut".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.CondomBloatsTip".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.EaseHipsForward".Localized(
                Player.CockHead()
            ));

            if (cocks > 1 && y >= 0)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.SecondBumpsPucker".Localized(
                    Appearance.CockNoun(Player.Cocks[y].Id)
                ));

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.CarefullyBreach".Localized());

            if (cocks > 1 && y >= 0)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.SLetter".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.GoingGetsEasier".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.RockPickingTempo".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.BallsSmackCunt".Localized());
            else
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.CrotchSlaps".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.FuckHardFast".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.CumsFirstMuffled".Localized(
                Player.CockDescript(x)
            ));

            if (cocks > 1 && y >= 0)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.IntensityPressure".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.EquineTremblesPayload".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.SpurtSloppyBox".Localized());

            if (cocks > 1 && y >= 0)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.CrammedMatchesBrother".Localized(
                    Player.CockDescript(y)
                ));

            if (cocks > 2)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.ButtTailCovered".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.InternalMusclesMilk".Localized());

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.BloatedFertility".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.CollapsePostOrgasmic".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.OwnBreastsWord".Localized());
            else
                OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.YourChestWord".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.AsSheSaysQuote".Localized());

            OutputText("Scenes.Urtascene.DudeFuckingUrtasCooch.HappyNodLeave".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            KnockUpUrtaChance();

            ReturnToCamp(1);
        }

        public void FuckUrtasVagButTooBig()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.FuckUrtasVagButTooBig.LetHerKnowWant".Localized());

            ClearButtons();

            if (Player.HasVagina())
                AddButton(0, "Common.Button.RideVaginal".Localized(), RideUrtaTenderFemale);
            else
                AddButton(0, "Common.Button.RideVaginal".Localized(), null);

            AddButton(1, "Common.Button.RideAnal".Localized(), TenderTakeItUpTheAssFromUrta);

            AddButton(2, "Common.Button.BarBj".Localized(), BlowUrtaUnderTable);

            AddButton(3, "Common.Button.NoCondom".Localized(), CondomlessUrtaInHouseSmex);

            AddButton(14, "Common.Button.Leave".Localized(), FuckItAndLeave);
        }

        public void FuckItAndLeave()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) - 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 1)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) - 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 1)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 36);

            UrtaLove(-5);

            OutputText("Scenes.Urtascene.FuckItAndLeave.ChangedMindLeaving".Localized());

            ReturnToCamp(1);
        }

        public void TenderTakeItUpTheAssFromUrta()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.PcSeenUrtaSexToys, CoC.Instance.Flags.Get(GameFlagIds.PcSeenUrtaSexToys) + 1);

            UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(3));

            var cocks = Player.Cocks.Count;

            ShowImage("urta-home-anal");

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SlapRollOver".Localized());

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.CheeksColorLower".Localized());

            if (Player.AnalCapacity() >= 60)
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SighSlideDown".Localized());
            else
            {
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.GruntSlideDown".Localized());

                if (Player.Anal.Looseness < 5)
                {
                    if (Player.Anal.Looseness == 0)
                        OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.AnalVirginityTaken".Localized());
                    else
                        OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.AssStretched".Localized());

                    Player.Anal.Looseness += 1;
                }
            }

            if (!Player.HasStatusEffect(StatusEffectIds.ButtStretched))
                Player.CreateStatusEffect(StatusEffectIds.ButtStretched, 0, 0, 0, 0);
            else
                Player.ChangeStatusValue(StatusEffectIds.ButtStretched, 1, 0);

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.BumpsVeinsProstate".Localized());

                if (cocks > 1)
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.EachOfFragment".Localized());

                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.HardensInstantly".Localized());
            }

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SlipsFurtherBottomed".Localized());

            if (cocks > 0)
            {
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SitOnThickness".Localized());

                if (cocks > 1)
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.EachOfFragmentAgain".Localized());

                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.DroopsOntoBelly".Localized());

                if (Player.Cocks[0].Length >= 15)
                {
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.WinksAndGuides".Localized());

                    if (cocks > 1)
                        OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.OneOfFragment".Localized());

                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.CockBetweenTits".Localized());

                    if (Player.Cocks[0].Length >= 20 && Player.Cocks[0].Length <= 28)
                        OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.LickAndSuck".Localized());

                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SentencePeriod".Localized());

                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SlowTitFuck".Localized());
                }

                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.ParagraphBreak".Localized());
            }
            else
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SitLikeThatAgain".Localized());

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.BounceSpearing".Localized());

            if (cocks == 0)
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.StrangePleasureSpreads".Localized());
            else
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.PowerfulPleasureProstate".Localized());

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.CriesGrabsHips".Localized());

            if (cocks > 0)
            {
                if (Player.Cocks[0].Length < 15)
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.EruptsOntoBelly".Localized());
                else
                    if (Player.Cocks[0].Length < 20)
                        OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.EruptsOverTits".Localized());
                    else
                        OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.EruptsIntoLips".Localized());

                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.GroansMilkYou".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.GulpsLeakingLips".Localized());

                if (Player.CumQ() >= 1000 && Player.CumQ() < 1500)
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SoaksNipplesSlopped".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SoaksBedSeed".Localized());

                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.PullsDownHug".Localized());

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SaysBetterGo".Localized());

            if (Player.CumQ() > 750)
                OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.MessyBoy".Localized());

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.TenderTakeItUpTheAssFromUrta.UntanglesCondomPops".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void MarbleCockuBlockuUrta()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.SeriousLookReport".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) == 100)
                OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.AndNursery".Localized());

            OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.SentencePeriod".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 0)
            {
                OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.KnowNotCouple".Localized());

                OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.CheeksColorSheepish".Localized());
            }
            else
            {
                OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.PathsKeepApart".Localized());

                OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.LookingNervously".Localized());
            }

            OutputText("Scenes.Urtascene.MarbleCockuBlockuUrta.AnswerHonestRefuseLie".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Honestly".Localized(), TellUrtaDAHTRUUUUF);

            AddButton(1, "Common.Button.Refuse".Localized(), Trufftrufftrufftruff);

            AddButton(2, "Common.Button.Lie".Localized(), LieToUrtaAboutMarble);
        }

        public void TellUrtaDAHTRUUUUF()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.ExplainMarbleMilk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.LoveYouForIt".Localized());
            else
                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.RespectYouForIt".Localized());

            OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.SentencePeriod".Localized());

            if (!UrtaOpenAboutEdryn())
            {
                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.ClearingSkeletons".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
                    OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.BeenTogether".Localized());
                else
                    OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.GottenIntimate".Localized());

                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.AshamedNoControl".Localized());

                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.ComfortOrToughen".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Comfort".Localized(), ComfortUrtaAfterTellinTruuf);

                AddButton(1, "Common.Button.ToughenUp".Localized(), ToughenUpUrtaAfterTellingTruuf);

                AddButton(2, "Common.Button.WalkOut".Localized(), WalkoutOnUrtaAfterTellingTruff);
            }
            else
            {
                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.SayEnd".Localized());

                OutputText("Scenes.Urtascene.TellUrtaDAHTRUUUUF.PlayfullySuggests".Localized());

                ReturnToCamp(1);
            }
        }

        public void ComfortUrtaAfterTellinTruuf()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.ComfortUrtaAfterTellinTruuf.LetUrtaKnowUnderstand".Localized());

            if (((Player.IsTaur() && Player.Cocks.Count > 0) || Player.CountCocksOfType("Horse") > 0 || (Player.Cocks.Count > 0 && Player.Corruption > 50)) && Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 4)
                OutputText("Scenes.Urtascene.ComfortUrtaAfterTellinTruuf.SuggestsTurnsEdryn".Localized());

            UrtaLove(-4);

            ReturnToCamp(1);
        }

        public void ToughenUpUrtaAfterTellingTruuf()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.ToughenUpUrtaAfterTellingTruuf.ExplainNotTooHard".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
                OutputText("Scenes.Urtascene.ToughenUpUrtaAfterTellingTruuf.HeartbrokenWord".Localized());
            else
                OutputText("Scenes.Urtascene.ToughenUpUrtaAfterTellingTruuf.SaddenedWord".Localized());

            OutputText("Scenes.Urtascene.ToughenUpUrtaAfterTellingTruuf.ByNewsFragment".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
            {
                OutputText("Scenes.Urtascene.ToughenUpUrtaAfterTellingTruuf.DefeatedLook".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, 0);

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);
            }
            else
            {
                OutputText("Scenes.Urtascene.ToughenUpUrtaAfterTellingTruuf.DeterminedLook".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, 0);
            }

            ReturnToCamp(1);
        }

        public void WalkoutOnUrtaAfterTellingTruff()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.WalkoutOnUrtaAfterTellingTruff.PushAwayEnraged".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void Trufftrufftrufftruff()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.Trufftrufftrufftruff.RefuseAnswerOutside".Localized());

            OutputText("Scenes.Urtascene.Trufftrufftrufftruff.TellOrLie".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Truth".Localized(), TellUrtaDAHTRUUUUF);

            AddButton(1, "Common.Button.Lie".Localized(), LieToUrtaAboutMarble);
        }

        public void LieToUrtaAboutMarble()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.LieToUrtaAboutMarble.PoorCowgirlStay".Localized());

            OutputText("Scenes.Urtascene.LieToUrtaAboutMarble.MisjudgedYou".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleKids) == 1)
                OutputText("Scenes.Urtascene.LieToUrtaAboutMarble.SawTheKid".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) > 1)
                OutputText("Scenes.Urtascene.LieToUrtaAboutMarble.SawTheKids".Localized());

            OutputText("Scenes.Urtascene.LieToUrtaAboutMarble.KnowYouTwoItem".Localized());

            OutputText("Scenes.Urtascene.LieToUrtaAboutMarble.NothingButGo".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaAngryAtPcCountdown, 48);

            ReturnToCamp(1);
        }

        public void UrtaTwuWuvOffer()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaTwuWuvOffer.FidgetsNervously".Localized());

            OutputText("Scenes.Urtascene.UrtaTwuWuvOffer.LeansHandsShoulders".Localized());

            OutputText("Scenes.Urtascene.UrtaTwuWuvOffer.DeclarationNoSurprise".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), TwuWuvIsBeautifulUrta);

            AddButton(1, "Common.Button.No".Localized(), NoUDontLoveUrta);
        }

        public void NoUDontLoveUrta()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcAffectionCounter, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            OutputText("Scenes.Urtascene.NoUDontLoveUrta.RemoveHandsEnjoy".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), StayFuckbuddiesAfterShootingDown);

            AddButton(1, "Common.Button.No".Localized(), TurnDownFuckbuddiesANDLove);
        }

        public void StayFuckbuddiesAfterShootingDown()
        {

            UrtaSprite();

            OutputText("Scenes.Urtascene.StayFuckbuddiesAfterShootingDown.WarmSmileKiss".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 0);

            ReturnToCamp(1);
        }

        public void TurnDownFuckbuddiesANDLove()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TurnDownFuckbuddiesANDLove.BurstsTearsRuns".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void TwuWuvIsBeautifulUrta()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TwuWuvIsBeautifulUrta.NodBreakWildSmile".Localized(
                Player.Mf("chuckle", "giggle")
            ));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, 1);

            ReturnToCamp(1);
        }

        public void EatUrtaOutNomNomPussy()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 3);

            UrtaLove(1);

            ShowImage("urta-bar-vag");

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.SuggestsSecludedBooth".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.SuppressingChuckle".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.SecretlyNoticed".Localized());
            else
                OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.HopingNoOneSaw".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.WetWarmForehead".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.EyesAdjusted".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.ScoochesEagerly".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.HeelsApproach".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.ThighsClamp".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.OtherVoiceBlurts".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.LickingJudging".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.VicePressureReleases".Localized());

            OutputText("Scenes.Urtascene.EatUrtaOutNomNomPussy.FinishedDeflates".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GiveGlass".Localized(), GiveUrtaCumGlass);

            AddButton(1, "Common.Button.DrinkGlass".Localized(), DrinkUrtasCumGlass);

            AddButton(2, "Common.Button.SetAside".Localized(), SetAsideUrtaCumGlass);

            AddButton(3, "Common.Button.Takesomehome".Localized(), TakeUrtasCumHomeWithYou);
        }

        public void GiveUrtaCumGlass()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 1);

            UrtaLove(-.5f);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            OutputText("Scenes.Urtascene.GiveUrtaCumGlass.KissDistractGlass".Localized());

            OutputText("Scenes.Urtascene.GiveUrtaCumGlass.YouSayQuote".Localized());

            OutputText("Scenes.Urtascene.GiveUrtaCumGlass.SnugglesTipsSwig".Localized());

            OutputText("Scenes.Urtascene.GiveUrtaCumGlass.SmileKnowinglyLeave".Localized());

            ReturnToCamp(1);
        }

        public void DrinkUrtasCumGlass()
        {
            ClearOutput();

            UrtaSprite();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            UrtaLove(1);

            OutputText("Scenes.Urtascene.DrinkUrtasCumGlass.GoodLookTipBack".Localized());

            Player.RefillHunger(30);

            OutputText("Scenes.Urtascene.DrinkUrtasCumGlass.SmileKnowinglyLeave".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 20f));

            ReturnToCamp(1);
        }

        public void SetAsideUrtaCumGlass()
        {
            ClearOutput();

            UrtaSprite();

            UrtaLove(2);

            OutputText("Scenes.Urtascene.SetAsideUrtaCumGlass.SetAsideCuddle".Localized());

            ReturnToCamp(1);
        }

        public void TakeUrtasCumHomeWithYou()
        {
            ClearOutput();

            UrtaSprite();

            UrtaLove(0.5f);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            OutputText("Scenes.Urtascene.TakeUrtasCumHomeWithYou.KissTakeHome".Localized());

            OutputText("Scenes.Urtascene.TakeUrtasCumHomeWithYou.SmilesLoveYou".Localized());

            OutputText("Scenes.Urtascene.TakeUrtasCumHomeWithYou.SmileKnowinglyLeave".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.UrtaCum), 1);

            ReturnToCamp(1);
        }

        public void BlowUrtaUnderTheTableLuv()
        {
            ClearOutput();

            UrtaSprite();

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(3));

            UrtaLove(1);

            ShowImage("urta-bar-bj");

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.SuggestsUnderTable".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.ShotGlassSlam".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.WrithesSpillsDrink".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.DecideGetHerOff".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.SuppressGagReflex".Localized());
            else
                OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.RammingDeepThroat".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.GruntsMufflesNoise".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.BarChuckles".Localized());

            if (Player.Corruption < 50 && SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.WishTablecloth".Localized());
            else
                OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.WishMoreEyes".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.DistractionLittleToDivert".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.LegsWrapAround".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.WaveLikeMotion".Localized());

            Player.RefillHunger(75);

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.ThumpingOnBack".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.DraggingBetween".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.DraggingAcrossChest".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.SoftenWipeLeavings".Localized());

            OutputText("Scenes.Urtascene.BlowUrtaUnderTheTableLuv.ClimbedOutFromTable".Localized());

            ReturnToCamp(1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
        }

        public void RideUrtasCoochLoveyDovey()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            UrtaLove(1);

            if (Player.VaginalCapacity() < 40)
            {
                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SmileTurnsConcerned".Localized());

                UrtaHomeLuvLuvinsMenu();

                return ;
            }

            ShowImage("urta-home-female-fuck");

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BurstsWideSmile".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.TossesYou".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.LatexSheath".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.LatexSheaths".Localized());

                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.CoverCocksExplains".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.ClimbBesideHer".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.CriesMuted".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.Vaginas[0].Wetness < 2)
                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.GrowingMoist".Localized());
            else
                if (Player.Vaginas[0].Wetness < 4)
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.GrowingWetter".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SoakingCondomCock".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BodyGoesLimp".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.EndKissPulling".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.PressOnwardsDownwards".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SinkingDownSlowly".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BottomOutRocking".Localized(
                Player.VaginaDescript(0)
            ));

            Player.CuntChange(60, true);

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.HandsGrabFirmly".Localized(
                Player.AssDescript()
            ));

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BendingAnotherKiss".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SeedBalloonStretches".Localized());

            if (Player.VaginalCapacity() < 80)
                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.NoMoreRoom".Localized());
            else
                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.FillingWithCum".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.WatchingLoverSubmit".Localized());

            if (Player.CockTotal() > 0)
            {
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.YLetter".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.EachOfY".Localized());

                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SpurtsUnloadingCream".Localized());

                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.CondomSheAsked".Localized());

                    if (Player.CumQ() >= 250)
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BalloonInflatesSlightly".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BalloonSwellsLarger".Localized());
                            else
                            {
                                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BalloonSwellsObscenely".Localized());

                                if (Player.CumQ() > 1500)
                                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SeedPopsCondom".Localized());
                            }
                }
                else
                {
                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.CondomsSheAsked".Localized());

                    if (Player.CumQ() >= 250)
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BalloonsInflateSlightly".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BalloonsSwellLarger".Localized());
                            else
                            {
                                OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.BalloonsSwellObscenely".Localized());

                                if (Player.CumQ() > 1500)
                                    OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SeedPopsCondoms".Localized());
                            }
                }
            }

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.EyesRollBack".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.PullBackWetPop".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.GripesLaundry".Localized());

            OutputText("Scenes.Urtascene.RideUrtasCoochLoveyDovey.SmilingEmbrace".Localized());

            ReturnToCamp(1);

            Player.Orgasm("Vaginal");

            if (Player.SensitivityPercent > 30)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
        }

        public void RideUrtaInButtAtHomeLove()
        {
            ClearOutput();

            UrtaSprite();

            UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            ShowImage("urta-home-anal-urta");

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.RaisesEyebrowBlushes".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SquirmsSmilesNervously".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.InsistsWearCondom".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.CondomPhrase".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.CondomsPhrase".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ControlMess".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SLetter".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.TightLatex".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ClimbOnBed".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.KnowingSmile".Localized());

            if (Player.AnalCapacity() < 60)
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.TightWord".Localized());
            else
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.WelcomingWord".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.WarmHoleGroan".Localized());

            Player.ButtChange(60, true, true, false);

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.TinyDribblesForce".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.EachOfFragment".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.UrethraWord".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SLetterAgain".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ProstateCompressed".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.RecoversMeetsGaze".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ShiftHipsCaress".Localized(
                Player.NippleDescript(0)
            ));

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SlippingFingerInside".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SpreadingThroughBuds".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.LockedByDesire".Localized());

            if (Player.Cocks.Count > 0)
            {
                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.EachOfYAgain".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.YLetterAgain".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SheathFilledPreCum".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.BallsWord".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ProstateWord".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.AchingToUnload".Localized());
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.NeglectedUnused".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Wetness < 4)
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.DampensWord".Localized());
                else
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.FloodsWord".Localized());

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.FoxUnderneathMoisture".Localized());
            }

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.LovemakingIncreasesTempo".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.CryOutPleasure".Localized());
            else
            {
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.CryOutInvadingWarmth".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ClenchesPeaking".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (Player.Vaginas[0].Wetness > 2 && Player.Vaginas[0].Wetness < 5)
                        OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.FluidsSoakCrotch".Localized());
                    else
                        if (Player.Vaginas[0].Wetness == 5)
                            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.FeminineFluidsExplosion".Localized());
                }

                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.WarmthSpreadsDeep".Localized());

                    if (Player.CumQ() >= 750 && Player.CumQ() < 1250)
                        OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.BalloonBasketballSize".Localized());
                    else
                        if (Player.CumQ() >= 1250)
                        {
                            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.BalloonWatermelonSize".Localized());

                            if (Player.CumQ() >= 1700)
                                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.FillFullerAndFuller".Localized());
                        }
                }

                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ParagraphBreakThird".Localized());
            }

            Player.ChangeFatigue(10);

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ExhaustedDraggingUp".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.ComesOutOfStupor".Localized());

            if (Player.CumQ() >= 1700)
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SighsAtMessLaughs".Localized());
            else
                OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.SighsContentedlyLaughs".Localized());

            OutputText("Scenes.Urtascene.RideUrtaInButtAtHomeLove.FoxStaggersWeakLegs".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            ReturnToCamp(1);
        }

        public void OralFiestyUberExplosionUrta()
        {
            ClearOutput();

            UrtaSprite();

            UrtaLove(1.5f);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            int x = Player.CockThatFits(100);

            if (x < 0)
                x = 0;

            if (Player.CockArea(x) >= 100)
            {
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.TakesOneLook".Localized(
                    Player.CockDescript(x)
                ));

                UrtaHomeLuvLuvinsMenu();

                return ;
            }

            ShowImage("urta-home-69");

            if (Player.Gender == Gender.Female || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
            {
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.WantsSixtyNine".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.MuskyScentHorsePrick".Localized());

                if (Player.Vaginas[0].Wetness < 4)
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.EquallyWet".Localized());
                else
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SoppingWet".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ShiveringFromAir".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.StabbingTongueFolds".Localized(
                    Player.VaginaDescript(0),

                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.StifleMoans".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.PushForwardsArch".Localized());
                else
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.JerkingOffIntoMouth".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.GrindHipsOnFace".Localized(
                    Player.VaginaDescript(0)
                ));

                if (Player.Corruption <= 50)
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.MouthWord".Localized());
                else
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ThroatWord".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ClampingLegsHead".Localized());

                if (Player.Vaginas[0].Wetness > 4)
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.FluidsRushFolds".Localized());
                else
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.WetnessCoatsLips".Localized());

                if (Player.Cocks.Count > 0)
                {
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.EachOfYThird".Localized());
                    else
                        OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.YLetterThird".Localized());

                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.CockDroolsLamely".Localized());

                    if (Player.CumQ() < 50)
                        OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SputteringSpunk".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SoakingNeckChest".Localized());
                        else
                            if (Player.CumQ() < 700)
                                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ShoweringNeckBody".Localized());
                            else
                                if (Player.CumQ() < 1500)
                                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.TorrentOfSeedNeck".Localized());
                                else
                                {
                                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.TorrentDrenchesBed".Localized());

                                    if (Player.CumQ() > 10000)
                                        OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.PuddleThickens".Localized());
                                }
                }

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ParagraphBreak".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.DrunkWithPleasure".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.PoursBulgingThroat".Localized());
                else
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.EruptsFillingMouth".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SputterForBreath".Localized());

                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.GigglesReturnsCuddling".Localized());

                ReturnToCamp(1);

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                return ;
            }

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SitsUpScreams".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.YourBallsAnd".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.OneOfYour".Localized());
            else
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.YourWord".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.UrtaLipsAddition".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.FoxyLoverOpensWide".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SlitheringAroundBalls".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.PlaceholderSentencePeriod".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.TongueCurvesSlurps".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SheathWord".Localized());
            else
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.BaseWord".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.StoppingToSuckle".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.FoxBusyWithGenitals".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.MuffledExcitedMoans".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript(0));
                else
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.TaintWord".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SpittleFrothsLips".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.OutdonePullBackGasp".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.GagReflexForcesBack".Localized());
            else
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SuppressGagEasily".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ParagraphBreakThird".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SomeoneWalkedIn".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.CumFirstVictim".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.GurglesStrugglesKeepUp".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.DrippingFromNostrils".Localized());
            }

            if (Player.CumQ() >= 750 && Player.CumQ() < 1500)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.BellyRoundsOut".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.BellyRoundsSpurtOver".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.SatedSighUrtaPushes".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.FewMomentsToRecover".Localized());

            if (Player.Corruption < 60)
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ForcingCompulsiveSwallow".Localized());
            else
                OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.InjectingIntoBelly".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.MoreSaltyCream".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.ClimbOffPanting".Localized());

            OutputText("Scenes.Urtascene.OralFiestyUberExplosionUrta.BlowsKissGottaGo".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void UrtaHomeLuvLuvinsMenu()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.CaressesNippleShivers".Localized());

            OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.BlushesContinues".Localized());

            OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.HowWantToFuck".Localized());

            ClearButtons();

            if (Player.HasVagina())
            {
                OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.SheRideVagina".Localized());

                AddButton(0, "Common.Button.RideVag".Localized(), RideUrtasCoochLoveyDovey);
            }
            else
                AddButton(0, "Common.Button.RideVag".Localized(), null);

            OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.SheFuckYourAss".Localized());

            AddButton(1, "Common.Button.RideAss".Localized(), RideUrtaInButtAtHomeLove);

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.UrtaCouldSixtyNine".Localized());

                AddButton(2, "69", OralFiestyUberExplosionUrta);

                AddButton(3, "Common.Button.VagSceneinstanceScorpionsceneGetFuck".Localized(), UrtasCoochiNeedsFuckedLove);

                AddButton(5, "Common.Button.Fuckandlick".Localized(), LickOutUrtaAtHome);
            }
            else
            {
                AddButton(2, "69", null);

                AddButton(3, "Common.Button.VagSceneinstanceScorpionsceneGetFuck".Localized(), null);

                AddButton(5, "Common.Button.Fuckandlick".Localized(), null);
            }

            if (!Player.IsGenderless())
            {
                OutputText("Scenes.Urtascene.UrtaHomeLuvLuvinsMenu.OptionCondomless".Localized());

                AddButton(4, "Common.Button.NoCondoms".Localized(), CondomlessUrtaInHouseSmex);
            }
            else
                AddButton(4, "Common.Button.NoCondoms".Localized(), null);

            if (Player.IsGoo() && Player.HasGooSkin())
                AddButton(6, "Common.Button.GooWeird".Localized(), UrtaGooTesticleVoreRuinedOrgasms);
            else
                AddButton(6, "Common.Button.GooWeird".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPetplayDone) >= 0 && !Player.IsGenderless())
                AddButton(7, "Common.Button.Collar".Localized(), UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice);
        }

        public void UrtasCoochiNeedsFuckedLove()
        {
            ClearOutput();

            UrtaSprite();

            UrtaLove(1.5f);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 2 + Random.Shared.Next(2));

            int x = Player.CockThatFits(UrtaCapacity());

            if (x < 0)
                x = 0;

            ShowImage("urta-fills-her-condom");

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.KnowingLookSpreadsLegs".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.NodAdvanceDropArmor".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.MainWord".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.MoistOpeningShiver".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SLetter".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SentencePeriodParagraphBreak".Localized());

            if (Player.CockArea(x) > UrtaCapacity())
            {
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.WhinesScoochesBack".Localized(
                    Player.Mf("stud", "baby")
                ));

                UrtaHomeLuvLuvinsMenu();

                return ;
            }

            ShowImage("urta-home-male-fuck");

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.PushSlowlySlidingIn".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SheathRubbingLips".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.PussyRubbingGroin".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.ByMaraeMoans".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.RockingBackForth".Localized(
                Appearance.CockNoun(BodyPartTypes.Human)
            ));

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.WetSchliicksNoisySlaps".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.CockOnVerge".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SheathWord".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BallsWord".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.CrotchWord".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.GrabAssSlamFinal".Localized());

            if (Player.HasKnot(x))
            {
                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.KnotFlaresLocking".Localized());

                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.RockStimulateKnot".Localized(
                    Player.CockDescript(x)
                ));

                if (!(Player.CumQ() < 250))
                    if (Player.CumQ() < 700)
                        OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellyPuffsUp".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySwellsLarger".Localized(
                                Appearance.CockNoun(BodyPartTypes.Human)
                            ));
                        else
                        {
                            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySwellsLargerAgain".Localized());

                            if (Player.CumQ() < 3000)
                                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.LooksVeryPregnant".Localized());
                            else
                            {
                                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.OrgasmPumpsHerUp".Localized());

                                if (Player.CumQ() > 5000)
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.NotContentEasyRelease".Localized(
                                        Player.CockDescript(x)
                                    ));
                            }
                        }

                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.KnotShrinkingPullFree".Localized(
                    Appearance.CockNoun(BodyPartTypes.Human)
                ));

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SpunkRushesTorrent".Localized());

                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.GlazedPussyLipsClench".Localized());
            }
            else
                if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                {
                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.FlareFillingBlood".Localized());

                    if (Player.Cocks[x].Length >= 10)
                        OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.ProdsAgainstCervix".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SeatsDeepInside".Localized());

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.CrotchMusclesClench".Localized());

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.RockDragFlaredTip".Localized());

                    if (!(Player.CumQ() < 250))
                        if (Player.CumQ() < 700)
                            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySlowlyRises".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySwellsLargerThird".Localized(
                                    Appearance.CockNoun(BodyPartTypes.Human)
                                ));
                            else
                            {
                                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySwellsLargerFourth".Localized());

                                if (Player.CumQ() < 3000)
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.LooksHeavilyPregnant".Localized());
                                else
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.EndlessOrgasmPumpsMore".Localized());

                                if (Player.CumQ() > 5000)
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.NotContentAgain".Localized(
                                        Player.CockDescript(x)
                                    ));
                            }

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.FlareReturningPullOut".Localized());

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SpunkRushesTorrentAgain".Localized());

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.GlazedLipsClenchAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.CockClenchesFillingCunt".Localized());

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.RockSlidingSpasming".Localized());

                    if (!(Player.CumQ() < 250))
                        if (Player.CumQ() < 700)
                            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellyPuffsUpAgain".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySwellsLargerFifth".Localized(
                                    Appearance.CockNoun(BodyPartTypes.Human)
                                ));
                            else
                            {
                                OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.BellySwellsLargerSixth".Localized());

                                if (Player.CumQ() < 3000)
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.LooksVeryPregnantAgain".Localized());
                                else
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.OrgasmNeverEndingPumps".Localized());

                                if (Player.CumQ() > 5000)
                                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.NotContentThird".Localized());
                            }

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.DeflatingPullFree".Localized(
                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));

                    if (Player.CumQ() >= 2000)
                        OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SpunkRushesTorrentThird".Localized());

                    OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.GlazedLipsClenchThird".Localized());
                }

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.CalmsDownCondomPeel".Localized(
                Appearance.CockNoun(BodyPartTypes.Human)
            ));

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.PullsIntoArmsKiss".Localized());

            OutputText("Scenes.Urtascene.UrtasCoochiNeedsFuckedLove.SeparateWays".Localized());

            KnockUpUrtaChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void ScyllaAndUrtaSittingInATree()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.TimesCaughtUrtaWithScylla, CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) + 1);

            if (!UrtaDrunk())
            {
                UrtaAndScyllaBoningLikeBitchesSober();

                return ;
            }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaBannedFromScylla) == 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaConfrontedScylla) > 0)
            {
                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.WaitressKeyClutched".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.UrtaSittingOnCrate".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.FoxLewdLookCradle".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.ConversationTwitch".Localized());

                if (Player.CockThatFits(UrtaCapacity()) != -1 && !Player.IsTaur())
                {
                    AddButton(0, "Common.Button.Lap".Localized(), MakeUrtaSitOnYourLapWithScylla);

                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.UrtaSitOnLap".Localized());
                }
                else
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.JerkOffAndWatch".Localized());

                AddButton(1, "Common.Button.Jerk".Localized(), WatchTwoHotBitchesAndJerkIt);

                AddButton(2, "Common.Button.Lipplebond".Localized(), LippleBondUrtaScylla);

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.BondingWithLipples".Localized());
            }
            else
            {
                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.PassWetBitchRooms".Localized());

                if (!UrtaLove())
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.UrtaWord".Localized());
                else
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.YourLove".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.VoiceFirstDoorsSilent".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.FollowingEntrywaySilent".Localized());

                if (Player.Corruption < 40)
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.CorruptedRuffians".Localized());
                else
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.DemonAgents".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.GetDropOnHer".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.UrtaSittingLegsAkimbo".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.TheFoxIs".Localized());

                if (UrtaLove())
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.WeaklyProtesting".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11)
                        OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.GivingLewdLook".Localized());
                    else
                        OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.BlushingHard".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.CradlesScyllaHead".Localized());

                if (UrtaLove())
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.SorryBothKnow".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) >= 11)
                        OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.ShowedUpLate".Localized());
                    else
                        OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.DidntThinkSeeYou".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.ParagraphBreak".Localized());

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.ConversationTwitchAgain".Localized());

                if (Player.CockThatFits(UrtaCapacity()) != -1 && !Player.IsTaur())
                {
                    AddButton(0, "Common.Button.Lap".Localized(), MakeUrtaSitOnYourLapWithScylla);

                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.UrtaSitLapAgain".Localized());
                }
                else
                    OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.JerkOffWatchAgain".Localized());

                AddButton(1, "Common.Button.Jerk".Localized(), WatchTwoHotBitchesAndJerkIt);

                AddButton(2, "Common.Button.Lipplebond".Localized(), LippleBondUrtaScylla);

                OutputText("Scenes.Urtascene.ScyllaAndUrtaSittingInATree.ThrowFingerLeave".Localized());

                AddButton(3, "Common.Button.Heartbreak".Localized(), HeartBreakHotelInTelAdre);

                AddButton(14, "Common.Button.Leave".Localized(), FlipUrtaTheBird);
            }
        }

        public void UrtaAndScyllaBoningLikeBitchesSober()
        {
            ClearOutput();

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.WaitressKeyClutchedAgain".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.LeaningBackWall".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.ShuddersNoticesYou".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.BlueEyesCraneUp".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.MythicalBalloonicorns".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.DroolBubblesLips".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.MoansGigglesNervously".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.FuckBuddy".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.LoverWord".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.LustySlipUp".Localized());

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.WhatDoYouDo".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 5)));

            ClearButtons();

            AddButton(0, "Common.Button.SceneinstanceScorpionsceneGetFuckFox".Localized(), null);

            AddButton(1, "Common.Button.Worshipped".Localized(), null);

            Action worship = null;

            Action fuck = null;

            if (Player.HasCockThatFits(UrtaCapacity()))
            {
                fuck = FuckUrtaWhileScyllaSucksSober;

                OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.FuckTheFox".Localized());
            }

            if (Player.BiggestCockArea() > UrtaCapacity())
            {
                worship = DockWithUrtaUnderScyllasSoberSupervision;

                OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.WorshipRealDick".Localized());
            }

            OutputText("Scenes.Urtascene.UrtaAndScyllaBoningLikeBitchesSober.GetOutOfHand".Localized());

            AddButton(0, "Common.Button.SceneinstanceScorpionsceneGetFuckFox".Localized(), SceneInstance<ScorpionScene>.Get().Fuck);

            AddButton(1, "Common.Button.Worshipped".Localized(), SceneInstance<IvorySuccubusScene>.Get().Worship);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void FuckUrtaWhileScyllaSucksSober()
        {
            ClearOutput();

            var x = Player.CockThatFits(UrtaCapacity());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.PeelOutOfArmor".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.AndBallsFragment".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.HaulingOutMeat".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.PutItInAlready".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) >= 40)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.BulgingHerStomach".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.SlidingIntoPassage".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.FrontOfSack".Localized());
            else
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.YourLegs".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.ReachesBackAwkwardHug".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.ScyllaForgottenKnees".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.VixenStuttersTrapped".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.CockAndSharpPlace".Localized());
            else
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.LustyBodies".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.VelvetGloveTightens".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.SackRoundsOutUnseen".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.FatNutsDrape".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.ObliviousRedFlash".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.SheathWord".Localized());
            else
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.BaseWord".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.MoaningNoTime".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.NunConfusedMishap".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.WhimpersCannotHold".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.AlabasterStreamsThroat".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.RBallsFragment".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.DoseOfMedicine".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.HealthySquirtGoo".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.UnboundPackageBobs".Localized());

            if (Player.LactationQ() > 50)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.NipplesLetDown".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.ContinuesFillFox".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.BellyBloatsDeposit".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.RoundingTautTank".Localized());
                else
                    if (Player.CumQ() >= 1200)
                        OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.RoundingSwollenBank".Localized());
                    else
                        if (Player.CumQ() >= 750)
                            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.RoundingGravidBump".Localized());
                        else
                            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.RoundingNicelySized".Localized());

                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.HandsAcrossLastPumps".Localized());
            }

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.OrgasmConcludesSlump".Localized());

            if (Player.Tallness >= 78)
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.HeadWord".Localized());
            else
                if (Player.Tallness >= 48)
                    OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.NarrowMiddle".Localized());
                else
                    OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.CurvyHips".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.NunThroatBulge".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.ShrinkingMemberPlateaus".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.ShrinkingBallsPull".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.SlidesDownWall".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.BurpingCoversMouth".Localized());

            OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.NunHugsFox".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.LoverWetKiss".Localized());
            else
                OutputText("Scenes.Urtascene.FuckUrtaWhileScyllaSucksSober.QuickHugDepart".Localized());

            KnockUpUrtaChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void DockWithUrtaUnderScyllasSoberSupervision()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SighReleaseArmor".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.HalfFragment".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.MuskFillsAir".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SackSwingsBouncing".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SighingStrokeLength".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.UrtaWhimpersView".Localized(
                Player.Mf("hunk", "babe")
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.ScyllaWideEyed".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.BreastsPressBack".Localized());

            if (!Player.IsBiped() && !Player.IsDrider())
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SlipsAlong".Localized());
            else
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SlidesThrough".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.UrtaWrapsFromBehind".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.FelineGracePounce".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.ScyllaStrokesDelight".Localized(
                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.MentionsBigDaddy".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.CaressesBarelyHelp".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SackStretchesTight".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.NunWhispersBigger".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.ShiftBloatedSack".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.UrtaStepsBackAlarm".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaScyllaBigDickTimesDone) > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.PanickedLungesDoor".Localized());
            else
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.PanickedFaceDoor".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.EyesGlowBlack".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.ErectionGrowsUnimpeded".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.NunSauntersOver".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.GesturesGoOnFuck".Localized(
                Player.CockDescript(x)
            ));

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.FriendWord".Localized());
            else
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.LoverWord".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.LungesScatteringHaze".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.GrabsEdgesPullsDown".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.NutsPullTight".Localized(
                Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.BallsQuiverSeed".Localized());
            else
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.InnardsGurgleSeed".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.TricklesWhiteGush".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.BouncesOffLeakyCock".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.HornsCrownTwisted".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.InteriorEmptied".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.InkyNetRestraint".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.FireworkFiresOff".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.BallsGurgleEmpty".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.TitanicCockLifts".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.SheathWord".Localized());
            else
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.BaseWord".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.TipInInstant".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision.MixedJizzInflatesTits".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaScyllaBigDickTimesDone, CoC.Instance.Flags.Get(GameFlagIds.UrtaScyllaBigDickTimesDone) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            if (Player.CumQ() < 5000)
                Player.CumMultiplier += 2;

            if (Player.Balls > 0)
                Player.BallSize += 1;

            DockWithUrtaUnderScyllasSoberSupervision2();
        }

        public void DockWithUrtaUnderScyllasSoberSupervision2()
        {
            ClearOutput();

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.WakesSweetMouth".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.SmilesBurpsDwindle".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.WaitsOtherLeaves".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.NodSolemnly".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.WavesWord".Localized());
            else
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.KissesYouWord".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.GoodbyeSheSays".Localized());

            OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.DickReturnedOldSize".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DockWithUrtaUnderScyllasSoberSupervision2.BallsLittleBigger".Localized());

            ReturnToCamp(1);
        }

        public void HeartBreakHotelInTelAdre()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.HeartBreakHotelInTelAdre.YouReachWord".Localized());

            if (Player.Tallness >= 52)
                OutputText("Scenes.Urtascene.HeartBreakHotelInTelAdre.DownWord".Localized());
            else
                OutputText("Scenes.Urtascene.HeartBreakHotelInTelAdre.UpWord".Localized());

            OutputText("Scenes.Urtascene.HeartBreakHotelInTelAdre.SlapNeverSeeAgain".Localized());

            UrtaLove(-100);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void FlipUrtaTheBird()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.FlipUrtaTheBird.CurlHandMiddleFinger".Localized());

            UrtaLove(-1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) - 2);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 0);

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Lust, 35f), new DynStat(GameStats.Corruption, -1f));

            ReturnToCamp(1);
        }

        public void WatchTwoHotBitchesAndJerkIt()
        {
            ClearOutput();

            UrtaSprite();

            ShowImage("urta-scylla-masti");

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.OpenArmorSitCrate".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.ExposingYour".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.DemonicStubsSprout".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.YouStroke".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.AndTouch".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.FireEngineCocksBob".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.NeckPulledBulges".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.BallsShrinkingBlast".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.HandsLetGo".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.TitsDragFloor".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.PumpHard".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.InTurn".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.HotRopesSemen".Localized());

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.CoherentBlastUrtaTits".Localized());

            if (Player.CumQ() >= 450)
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.GoutScyllaFace".Localized());

            if (Player.CumQ() >= 700)
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.FewMoreBlasts".Localized());

            if (Player.CumQ() >= 1000)
            {
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.ProductiveDrenchesBoth".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.NInchesDeep".Localized());

                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.PuddleOnGround".Localized());
            }

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.SpentSlumpSmile".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.ShockedFirstTime".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) == 1)
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.DoThisAgain".Localized());
            else
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.BestManboy".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.SlapFindsBarrel".Localized());

            OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.PickUpArmorCamp".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.WatchTwoHotBitchesAndJerkIt.WistfulGrin".Localized());

            Player.Orgasm("Generic");

            UrtaLove(2);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 1);

            ReturnToCamp(1);
        }

        public void MakeUrtaSitOnYourLapWithScylla()
        {
            ClearOutput();

            UrtaSprite();

            int x = Player.CockThatFits(UrtaCapacity());

            var y = Player.CockThatFits2(UrtaCapacity());

            ShowImage("urta-scylla-lapfuck");

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SmirkInformsMore".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SnakingTailLegs".Localized());
            else
                if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
                    OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.EnvelopingGoo".Localized());
                else
                    OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.StraddlingHips".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.GrabWaistPullUp".Localized(
                Player.CockDescript(x)
            ));

            if (y > -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.LinesUpPussy".Localized(
                    Player.CockDescript(y)
                ));

            if ((Player.CockTotal() > 1 && y == -1) || Player.CockTotal() > 2)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.RemainingDickLeaks".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SlidesDownEasily".Localized());

            if (y != -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SqueezingSoppingCunt".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SheGrunts".Localized());

            if (x != -1 && y != -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.BothHolesNaughty".Localized());
            else
                if (UrtaLove())
                    OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.FillMeLove".Localized());
                else
                    if (Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.TreatALady".Localized());
                    else
                        OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.ThatsNice".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.TriesWatchOverShoulder".Localized());

            if (UrtaLove() || Player.Corruption <= 40)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.KissFullMouth".Localized());
            else
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.BiteHerNeck".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.GrabAsscheeksLift".Localized());

            if (y != -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.AndTwat".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.MassagingManhoodDrop".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.TailCurlsTakesOver".Localized());

            if (y != -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.DoublePenetration".Localized());
            else
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.AnalFuck".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.DrivesWildBottom".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.ScyllaNeckStretched".Localized(
                Player.CockDescript(x)
            ));

            if (y != -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.AndPlaceholder".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.PinchNipplesEncourage".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.GrowingBellyTits".Localized());

            if (y != -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.AndPussy".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.PoundingLitheFox".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.FingersDontStop".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.CumCrossEyed".Localized());

            if (y > -1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.OtherFillsWomb".Localized(
                    Appearance.CockNoun(Player.Cocks[y].Id)
                ));

            if ((Player.CockTotal() > 1 && y == -1) || Player.CockTotal() > 2)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.WaveSplattersBack".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.RubsBellyTenderly".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.WitsPregnantPair".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.RagWipeDonArmor".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.ContendedSmileKiss".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 11)
                    OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.NervousSmilePeck".Localized());
                else
                    OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.SatisfiedSmileCrush".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.HelpScyllaToRoom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) <= 1)
                OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.PayForDoor".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.MakeSureOk".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.CockJustTwitch".Localized());

            OutputText("Scenes.Urtascene.MakeUrtaSitOnYourLapWithScylla.HeadLowWayOut".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, .5f));

            UrtaLove(3);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 3);

            Player.CumMultiplier += 2;

            Player.BallSize += 1;

            if (y >= 0)
                KnockUpUrtaChance();

            ReturnToCamp(1);
        }

        public void TellUrtaNoMoreScylla()
        {
            ClearOutput();

            UrtaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaBannedFromScylla, 1);

            OutputText("Scenes.Urtascene.TellUrtaNoMoreScylla.OverlookButWarn".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.TellUrtaNoMoreScylla.DontDoubtMyLove".Localized());

            OutputText("Scenes.Urtascene.TellUrtaNoMoreScylla.StopDrinkingToo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DrinkMore".Localized(), TellUrtaToBeADrunkenHussy);

            AddButton(1, "Common.Button.DrinkLess".Localized(), TellUrtaToStopBeingALush);

            AddButton(2, "Common.Button.NoChange".Localized(), TellUrtaToStayTheSame);

            AddButton(3, "Common.Button.LeaveHer".Localized(), LeaveUrtaAfterScyllaConfrontation);
        }

        public void TellUrtaMoreScyllaIsFine()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TellUrtaMoreScyllaIsFine.RaunchySmileOkay".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.TellUrtaMoreScyllaIsFine.RememberYoureTheOne".Localized());
            else
                OutputText("Scenes.Urtascene.TellUrtaMoreScyllaIsFine.PartnersInGuard".Localized());

            OutputText("Scenes.Urtascene.TellUrtaMoreScyllaIsFine.WhatAboutDrinking".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DrinkMore".Localized(), TellUrtaToBeADrunkenHussy);

            AddButton(1, "Common.Button.DrinkLess".Localized(), TellUrtaToStopBeingALush);

            AddButton(2, "Common.Button.NoChange".Localized(), TellUrtaToStayTheSame);

            AddButton(3, "Common.Button.LeaveHer".Localized(), LeaveUrtaAfterScyllaConfrontation);
        }

        public void LeaveUrtaAfterScyllaConfrontation()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.LeaveUrtaAfterScyllaConfrontation.SighNotWhatWant".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.LeaveUrtaAfterScyllaConfrontation.MyLove".Localized());

            OutputText("Scenes.Urtascene.LeaveUrtaAfterScyllaConfrontation.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.LeaveUrtaAfterScyllaConfrontation.BarEerilyQuiet".Localized());

            UrtaLove(-100);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void TellUrtaToBeADrunkenHussy()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TellUrtaToBeADrunkenHussy.YouWord".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Urtascene.TellUrtaToBeADrunkenHussy.BlushLittleBit".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Urtascene.TellUrtaToBeADrunkenHussy.SmileNervously".Localized());
                else
                    OutputText("Scenes.Urtascene.TellUrtaToBeADrunkenHussy.GrinWidely".Localized());

            OutputText("Scenes.Urtascene.TellUrtaToBeADrunkenHussy.ExplainWhenDrunk".Localized());

            OutputText("Scenes.Urtascene.TellUrtaToBeADrunkenHussy.SmilesLewdlyWaves".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaDrinkFrequency, 1);

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ReturnToCamp(1);
        }

        public void TellUrtaToStopBeingALush()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TellUrtaToStopBeingALush.AlcoholismNotHelping".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaDrinkFrequency, -1);

            ReturnToCamp(1);
        }

        public void TellUrtaToStayTheSame()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.TellUrtaToStayTheSame.AskWhyChange".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.TellUrtaToStayTheSame.LoveWord".Localized());
            else
                OutputText("Scenes.Urtascene.TellUrtaToStayTheSame.LikeWord".Localized());

            OutputText("Scenes.Urtascene.TellUrtaToStayTheSame.BlushesEverything".Localized());

            UrtaLove(5);

            ReturnToCamp(1);
        }

        public void AmilyXUrtaUrtaFallout()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaAmilyFollowupNeeded, 2);

            UrtaSprite();

            OutputText("Scenes.Urtascene.AmilyXUrtaUrtaFallout.YouWord".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Urtascene.AmilyXUrtaUrtaFallout.PullUpChair".Localized());
            else
                OutputText("Scenes.Urtascene.AmilyXUrtaUrtaFallout.SitDown".Localized());

            OutputText("Scenes.Urtascene.AmilyXUrtaUrtaFallout.StirIntrospective".Localized());

            OutputText("Scenes.Urtascene.AmilyXUrtaUrtaFallout.SobsMonster".Localized());

            OutputText("Scenes.Urtascene.AmilyXUrtaUrtaFallout.TellOkayGlad".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ItsOkay".Localized(), ItsOkayUrtaAmilyIsHot);

            AddButton(1, "Common.Button.Heartbreak".Localized(), CrushUrtasHeart);
        }

        public void ItsOkayUrtaAmilyIsHot()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.ItsOkayUrtaAmilyIsHot.GrabShouldersLook".Localized());

            OutputText("Scenes.Urtascene.ItsOkayUrtaAmilyIsHot.SaltyTearstainedKiss".Localized());

            ReturnToCamp(1);
        }

        public void CrushUrtasHeart()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.CrushUrtasHeart.SlapArrogantFace".Localized());

            OutputText("Scenes.Urtascene.CrushUrtasHeart.LowerLipQuivers".Localized());

            UrtaLove(-100);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, -1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void ChastityBeltFun(bool cameFromSexMenu = true)
        {
            ClearOutput();

            if (cameFromSexMenu)
            {
                OutputText("Scenes.Urtascene.ChastityBeltFun.TellLoseCondom".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.AsksWhatInMind".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.NodAffirmationSight".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.StatesYouCauseMess".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.StrokeCheekPuppyEyes".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.TurnPickUpBelt".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.NNoFair".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.HandTremblingForces".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.SnapBeltShut".Localized());
            }
            else
            {
                OutputText("Scenes.Urtascene.ChastityBeltFun.PleadingLook".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.LaughHopesUp".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.YouBastardLook".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.PeelsOffDress".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.DanglingKeys".Localized());

                OutputText("Scenes.Urtascene.ChastityBeltFun.EvilGrinPanties".Localized());
            }

            OutputText("Scenes.Urtascene.ChastityBeltFun.WeakGrinPose".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.WalkAroundSlapButt".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.CreakChastityBelt".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.FeelingThirstyFetch".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.StripFoldClothes".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.ReturnsWobblingLess".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.TeasingSoMuchFun".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.TipMugSpill".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.SnapsIndignantly".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.LongSufferingSigh".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.HikesWayToChin".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.MugCatchDroplets".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.CupCloseOverflow".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.SultrySurprise".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.InterruptsExasperated".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.JudgesLookIntends".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.NodsShufflesShower".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.QuickTwistKey".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.WipeFaceImpressive".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.TurnFaucetRinse".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.AskWhereTowels".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.DryYourselfAgain".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.NeedsCombing".Localized());

            OutputText("Scenes.Urtascene.ChastityBeltFun.AskTwirl".Localized());

            ReturnToCamp(1);
        }

        public void QBsTalkExpack()
        {
            ClearOutput();

            UrtaSprite();

            if (UrtaLove())
                OutputText("Scenes.Urtascene.QBsTalkExpack.SmilesNoSwivel".Localized());
            else
                OutputText("Scenes.Urtascene.QBsTalkExpack.LackOfThump".Localized());

            UrtaDialogueMenu();
        }

        public void FriendsTalkExpack()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.FriendsTalkExpack.TakeSeatWriting".Localized());

            UrtaDialogueMenu();
        }

        public void UrtaDialogueMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Urta".Localized(), UrtaDiscussesSelf);

            AddButton(1, "Common.Button.Edryn".Localized(), UrtaDiscussesEdryn);

            AddButton(2, "Common.Button.TheWatch".Localized(), UrtaDiscussesTheWatch);

            if (!UrtaJustFriends())
                AddButton(3, "Common.Button.Alcoholism".Localized(), UrtaDiscussesAlcholism);

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) >= 4)
                AddButton(5, "Common.Button.Katherine".Localized(), UrtaDiscussessKatherine);

            if (SceneInstance<UrtaPregsScene>.Get().UrtaKids() > 0 && !Player.HasKeyItem("Spare Key to Urta's House"))
                AddButton(4, "Common.Button.VisitKids".Localized(), SceneInstance<UrtaPregsScene>.Get().VisitKidsFirstTime);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FirstTimeAtUrtaHouse) > 0)
                    AddButton(4, "Common.Button.HerHouse".Localized(), SceneInstance<UrtaPregsScene>.Get().TalkToUrtaAboutHerHouse);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void UrtaDiscussesSelf()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesSelf.TalkAboutHer".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaDiscussesSelf.WannaTalkAboutMe".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussesSelf.NotMuchToTell".Localized());

            ClearButtons();

            AddButton(2, "Common.Button.Employment".Localized(), UrtaDiscussesEmployment);

            AddButton(3, "Common.Button.Prejudice".Localized(), UrtaDiscussesPrejudice);

            AddButton(14, "Common.Button.Back".Localized(), UrtaDialogueMenu);

            if (UrtaJustFriends())
                AddButton(1, "Common.Button.Flirt".Localized(), FlirtWithUrta);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == .5f)
                {
                    AddButton(0, "Common.Button.Infertility".Localized(), null );

                    AddButton(1, "Common.Button.RomanceSex".Localized(), UrtaDiscussesSexAndRomance);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == 1)
                    {
                        AddButton(0, "CharCreation.AscensionPerkMenu.Button3".Localized(), UrtaFertilityDiscussion);

                        AddButton(1, "Common.Button.RomanceSex".Localized(), UrtaDiscussesSexAndRomance);
                    }
                    else
                    {
                        AddButton(0, "Common.Button.Family".Localized(), UrtaDiscussesFamily);

                        AddButton(1, "Common.Button.RomanceSex".Localized(), UrtaDiscussesSexAndRomance);
                    }
        }

        public void FlirtWithUrta()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == -1)
            {
                OutputText("Scenes.Urtascene.FlirtWithUrta.ResistFeelingsTelAdre".Localized());

                OutputText("Scenes.Urtascene.FlirtWithUrta.LivedHereWholeLife".Localized());

                OutputText("Scenes.Urtascene.FlirtWithUrta.NeverFeltUrge".Localized());
            }
            else
            {
                OutputText("Scenes.Urtascene.FlirtWithUrta.NoProblemWithHerms".Localized());

                OutputText("Scenes.Urtascene.FlirtWithUrta.GulpsBoozeFun".Localized());
            }

            OutputText("Scenes.Urtascene.FlirtWithUrta.SlideUnderTable".Localized());

            OutputText("Scenes.Urtascene.FlirtWithUrta.BarNoticedDontCare".Localized());

            OutputText("Scenes.Urtascene.FlirtWithUrta.NeverFindOut".Localized());

            OutputText("Scenes.Urtascene.FlirtWithUrta.EaseUpUnderstanding".Localized());

            OutputText("Scenes.Urtascene.FlirtWithUrta.MeetOutBack".Localized());

            OutputText("Scenes.Urtascene.FlirtWithUrta.WaitOutsideShadow".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPcLoveCounter, 0);

            UrtaFuckHer(true);
        }

        public void UrtaFertilityDiscussion()
        {
            ClearOutput();

            OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.TracesFingerDrink".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.ShouldStayFertile".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.CutLooseFertile".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPregEverybody) == 1)
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.AskNotFertileOthers".Localized());
            else
            {
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.BunInEdrynsOven".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0)
                    OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.AlreadyHadTurn".Localized());
            }

            OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.WinksRaspberry".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
            {
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.CurrentlyFertile".Localized());

                AddButton(0, "Common.Button.NoFertile".Localized(), UrtaFertilityToggle);
            }
            else
            {
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.CannotGetPregnant".Localized());

                AddButton(0, "Common.Button.GoFertile".Localized(), UrtaFertilityToggle);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPregEverybody) == 1)
            {
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.OkaySharingLove".Localized());

                AddButton(1, "Common.Button.NoPreg".Localized(), UrtaPregOthersToggle);
            }
            else
            {
                OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.WillNotKnockUp".Localized());

                AddButton(2, "Common.Button.SharePreg".Localized(), UrtaPregOthersToggle);
            }

            OutputText("Scenes.Urtascene.UrtaFertilityDiscussion.NoteNoContent".Localized());

            AddButton(14, "Common.Button.Back".Localized(), UrtaDialogueMenu);
        }

        public void UrtaFertilityToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
            {
                OutputText("Scenes.Urtascene.UrtaFertilityToggle.HalfHiddenOkay".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFertile, 0);
            }
            else
            {
                OutputText("Scenes.Urtascene.UrtaFertilityToggle.ThrowsHandsCheers".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFertile, 1);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Back".Localized(), UrtaFertilityDiscussion);
        }

        public void UrtaPregOthersToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPregEverybody) == 1)
            {
                OutputText("Scenes.Urtascene.UrtaPregOthersToggle.AwwwAreYouSure".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Urtascene.UrtaPregOthersToggle.EdrynBellyBloat".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Urtascene.UrtaPregOthersToggle.BothPregnantTogether".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaPregOthersToggle.PutThingToWork".Localized());

                OutputText("Scenes.Urtascene.UrtaPregOthersToggle.PlayfulPinch".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPregEverybody, 0);
            }
            else
            {
                OutputText("Scenes.Urtascene.UrtaPregOthersToggle.SalaciousCatKiss".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaPregEverybody, 1);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Back".Localized(), UrtaFertilityDiscussion);
        }

        public void UrtaDiscussesEdryn()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdryn.QuestionsAboutEdryn".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaDiscussesEdryn.GlancesEyesDark".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussesEdryn.GrayFoxTeases".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.History".Localized(), UrtaDiscussesEdrynHistory);

            AddButton(1, "Common.Button.Working".Localized(), UrtaDiscussesWorkingWithEdryn);

            AddButton(2, "Common.Button.Romance".Localized(), UrtaDiscussesEdrynRomance);

            AddButton(14, "Common.Button.Back".Localized(), UrtaDialogueMenu);
        }

        public void UrtaDiscussesTheWatch()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatch.QuestionsAboutWatch".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaDiscussesTheWatch.PerksUpPride".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussesTheWatch.StiffensProudly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Membership".Localized(), UrtaDiscussesWatchMembership);

            AddButton(1, "Common.Button.Role".Localized(), UrtaDiscussesWatchRole);

            AddButton(2, "Common.Button.Crime".Localized(), UrtaDiscussesCrimeAndPunishment);

            AddButton(3, "Common.Button.Reach".Localized(), UrtaDiscussesTheWatchsReach);

            AddButton(14, "Common.Button.Back".Localized(), UrtaDialogueMenu);
        }

        public void UrtaDiscussesAlcholism()
        {
            ClearOutput();

            UrtaSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscussedUrtaAlcoholism) == 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaConfrontedScylla) == 0)
            {
                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.QuietlyInformAboutDrinking".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.PointOutContradiction".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.FeelALittleBad".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.WouldFeelBad".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.DontLetUp".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.InsistNotUpset".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.RefusesToMeetEye".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.AskWhySheDrinks".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.BarksIndignantly".Localized());

                if (UrtaLove())
                    OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.LoverWord".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.CuteyWord".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.DontKnowWhatLived".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.CeasesRamblingDialogue".Localized());

                if (!UrtaLove())
                    OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.FriendWord".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.NeverAcceptedLikeYou".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.FallsSilentWaiting".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.DrinkALot".Localized(), UrtaDiscussAlcoholDrinkMore);

                AddButton(1, "Common.Button.DrinkALittle".Localized(), UrtaDiscussAlcoholDrinkLess);

                AddButton(2, "Common.Button.BeYourself".Localized(), UrtaDiscussAlcoholNoChange);

                AddButton(3, "Common.Button.BreakUp".Localized(), BreakUpWithTheBitchOverAlcoholism);
            }
            else
            {
                OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.TellWantDiscussHabits".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) == 1)
                    OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.BurpsStartsRant".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaDrinkFrequency) == 0)
                        OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.AsksCalmlyClearly".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaDiscussesAlcholism.RepliesDefensively".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.DrinkALot".Localized(), UrtaDiscussAlcoholDrinkMore);

                AddButton(1, "Common.Button.DrinkALittle".Localized(), UrtaDiscussAlcoholDrinkLess);

                AddButton(2, "Common.Button.BeYourself".Localized(), UrtaDiscussAlcoholNoChange);
            }
        }

        public void UrtaDiscussAlcoholDrinkMore()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussAlcoholDrinkMore.BlushAskWhyLess".Localized());

            if (Player.Pregnancy.Type == PregnancyIds.PlayerFather)
                OutputText("Scenes.Urtascene.UrtaDiscussAlcoholDrinkMore.SmilesGentlyPredatory".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussAlcoholDrinkMore.SmilesGentlyPredatoryAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaDrinkFrequency, 1);

            CoC.Instance.Flags.Set(GameFlagIds.DiscussedUrtaAlcoholism, 1);

            ReturnToCamp(1);
        }

        public void UrtaDiscussAlcoholDrinkLess()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussAlcoholDrinkLess.SighExplainAlcoholism".Localized());

            if (Player.Pregnancy.Type == PregnancyIds.PlayerFather)
                OutputText("Scenes.Urtascene.UrtaDiscussAlcoholDrinkLess.OnceNotPregnant".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussAlcoholDrinkLess.EyesGlitterResolve".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaDrinkFrequency, -1);

            CoC.Instance.Flags.Set(GameFlagIds.DiscussedUrtaAlcoholism, 1);

            ReturnToCamp(1);
        }

        public void UrtaDiscussAlcoholNoChange()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussAlcoholNoChange.SmileAskWhyChange".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscussedUrtaAlcoholism) == 0)
                UrtaLove(1);

            CoC.Instance.Flags.Set(GameFlagIds.DiscussedUrtaAlcoholism, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaDrinkFrequency, 0);

            ReturnToCamp(1);
        }

        public void BreakUpWithTheBitchOverAlcoholism()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.BreakUpWithTheBitchOverAlcoholism.SighLetKnowBehavior".Localized());

            OutputText("Scenes.Urtascene.BreakUpWithTheBitchOverAlcoholism.BarEerilyQuiet".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesFamily()
        {
            ClearOutput();

            UrtaSprite();

            if (!UrtaLove())
            {
                OutputText("Scenes.Urtascene.UrtaDiscussesFamily.TellWantLearnFamily".Localized());

                if (!UrtaLove())
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFamilyTalkAttempts) == 0)
                        OutputText("Scenes.Urtascene.UrtaDiscussesFamily.WincesEyesDamp".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaDiscussesFamily.ColdStare".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFamilyTalkAttempts) == 0)
                        OutputText("Scenes.Urtascene.UrtaDiscussesFamily.SighsSoftlyGlass".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaDiscussesFamily.ShakesHeadPainful".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussesFamily.ApologizeChangeTopic".Localized());

                UrtaDialogueMenu();
            }
            else
                SceneInstance<UrtaQuestScene>.Get().TalkWithUrtaAboutFamFam();
        }

        public void UrtaDiscussesSexAndRomance()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.AskTellHistory".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.AsksStallingForTime".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.JokesAboutRecount".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.SmileInsistDetails".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.DrinkEmptyGlass".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.CantBelievePopular".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.PleasedShockBlush".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.StartledLaugh".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.SmallSipSpeaks".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.BigSwigHoldsAloft".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.ScowlsMutters".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.SadSmileBetterOff".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.FortifiesWithDrink".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.AskTriedGirls".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.MumblesTracesRim".Localized());

            if (UrtaOpenAboutEdryn())
                OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.PensiveTrailsOff".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.WhenSawYouEnter".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesSexAndRomance.FinishedSubject".Localized());

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);

            ClearButtons();

            AddButton(0, "Common.Button.TeaseHer".Localized(), UrtaDiscussionTeaseAfterRomance);

            AddButton(14, "Common.Button.Leave".Localized(), UrtaDiscussionLeaveAfterRomance);
        }

        public void UrtaDiscussionLeaveAfterRomance()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussionLeaveAfterRomance.ThankSharingPersonal".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussionLeaveAfterRomance.GoodToTalkChange".Localized());

            ReturnToCamp(1);
        }

        public void UrtaDiscussionTeaseAfterRomance()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussionTeaseAfterRomance.ThankThenAskWhat".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussionTeaseAfterRomance.RocksBackConfused".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussionTeaseAfterRomance.SmirkSurelyOccasion".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussionTeaseAfterRomance.SurprisedSqueak".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussionTeaseAfterRomance.TableJoltsErection".Localized());

            UrtaLove(1);

            ClearButtons();

            if (!UrtaLove())
            {
                AddButton(0, "Common.Button.UrtasPlace".Localized(), GoBackToUrtasForLuvinz);

                AddButton(1, "Common.Button.HiddenBj".Localized(), BlowUrtaUnderTable);
            }
            else
            {
                AddButton(0, "Common.Button.HerPlace".Localized(), GoBackToUrtasForLuvinz);

                AddButton(1, "Common.Button.SuckOff".Localized(), BlowUrtaUnderTheTableLuv);

                AddButton(2, "Common.Button.EatOut".Localized(), EatUrtaOutNomNomPussy);

                if (Player.HasCock() && !Player.IsTaur())
                    AddButton(3, "Common.Button.VixenCream".Localized(), () => VixenAndCreamPart1(true));

                if (Player.Pregnancy.Type == PregnancyIds.PlayerFather)
                    RemoveButton(1);

                if (OptionStore.Instance.Get("watersports", 0) > 0 && Player.Gender != Gender.Genderless)
                    AddButton(4, "Common.Button.Shots".Localized(), UrtaWatersports);
            }

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
        }

        public void UrtaWatersports()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaWatersports.SnuggleWithLover".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.SitUpMeetGaze".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.RaiseHandWaitress".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.CuddleLittleMumbles".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.FavouriteCoupleWink".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.LeanPeckMuse".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.PlayfulChuckle".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.CloseEyesUnlikeHer".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.ParagraphBreakEllipsis".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.ThoughtTwoShots".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.RestForeheadKisses".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.SnatchGlassesPinch".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.UnbeknownstMistaken".Localized(
                Player.ClothedOrNakedLower(" With your other hand, you let loose the clasp on your [armor] and bring the glasses down to your genitals", " You bring the glasses down to your [if (ismaleorherm) {cock}][if (isfemale) { pussy}]")
            ));

            OutputText("Scenes.Urtascene.UrtaWatersports.JustGettingStarted".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.GazeMeetsAgain".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.WhaaTrailsOff".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.LoveYouMoreThan".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.RaiseGlassSwig".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.FaceHoversInches".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.UneasyStaresGlass".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.GestureTwoFingers".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.BelchSetsGlass".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.BartenderMoreShots".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports.TurnsAwayHuffs".Localized());

            UrtaWatersports2();
        }

        public void UrtaWatersports2()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaWatersports2.ThreeMoreRounds".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.SlursAccusesPlayfully".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.ChuckleWhisperFavor".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.TrademarkThunkWallet".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.RestHandAside".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.DeepSighBladder".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.BladderEmptiesGyrate".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.BreathFillsFoxPiss".Localized());

            OutputText("Scenes.Urtascene.UrtaWatersports2.HelpFumbleKeys".Localized());

            if (Player.HasCock())
                Player.Orgasm("Dick");
            else
                Player.Orgasm("VaginalAnal");

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEmployment()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.AskHowCameWatch".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.BlinksStrangeQuestion".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.NodAskHowWent".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.HandSlipsPlaceBelong".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.TriumphantlyDrainsGlass".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.GrinsPaperwork".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.CentaurInterrupts".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEmployment.ShrugsDutyCalls".Localized());

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesPrejudice()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.AskFacedPrejudice".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.RespondsWarily".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.NeverSeenBothFemale".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.PeersSoulSighs".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.DrinksUntilEmpty".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.AgreesOldWounds".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.AskWhatChanged".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.ShrugsDemons".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesPrejudice.NodThankPersonal".Localized());

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEdrynHistory()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynHistory.AskPastWithEdryn".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynHistory.ShrugsNeverKnew".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynHistory.NotMuchMoreThank".Localized());

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesWorkingWithEdryn()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesWorkingWithEdryn.CuriousWorkingRelationship".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWorkingWithEdryn.RespondsAllTheTime".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWorkingWithEdryn.AskWhyArrangement".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWorkingWithEdryn.ExplainsCentaursSmirks".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWorkingWithEdryn.ThankLetDrink".Localized());

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEdrynRomance()
        {
            ClearOutput();

            UrtaSprite();

            if (!UrtaLove())
            {
                if (!UrtaOpenAboutEdryn())
                {
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaOpenAboutEdryn, 1);

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.NoticedHangOutTogether".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.SpluttersWhiskey".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.ShrugHonestQuestion".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.QuickWince".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.PausesWaitsReaction".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.EdrynFirstWoman".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.OnlyWomanUntilYou".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.BiggerThanStallion".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.SighsFriendsWithBenefits".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.ShakesHeadOwedTruth".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.PonderAcceptIndiscretion".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Accept".Localized(), UrtaDiscussesEdrynNonLoveAccept);

                    AddButton(1, "Common.Button.Reject".Localized(), UrtaDiscussesEdrynNonLoveREJECT);
                }
                else
                {
                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.ThoughtfulLookAskEdryn".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.GoodNeverCompare".Localized());

                    if (Player.Corruption < 66)
                        OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.PlayfulManner".Localized());
                    else
                        OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.ExpressionSobers".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.SentencePeriod".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 5)
                    {
                        OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.GrinsDidHearingArouse".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0)
                            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.CantBelieveKnockedUp".Localized());
                        else
                            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.SayEnd".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 0)
                            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.PattingFlatBelly".Localized());
                    }

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.ShakeHeadApologize".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.AsksAsYouStand".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.WonderingBidGoodbye".Localized());

                    ReturnToCamp(1);

                    UrtaLove(.5f);
                }
            }
            else
                if (!UrtaOpenAboutEdryn())
                {
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaOpenAboutEdryn, 1);

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.NoticedHangOutAgain".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.LooksSheepish".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.AskExplain".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.SexNowAndThen".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.BlurtsOutScaredHate".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.ClearSheMeansIt".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Happy".Localized(), UrtaDiscussesEdrynLoveHappy);

                    AddButton(1, "Common.Button.Tolerate".Localized(), UrtaDiscussesEdrynLoveTolerate);

                    AddButton(2, "Common.Button.BreakUp".Localized(), UrtaDiscussesEdrynLoveBreakup);
                }
                else
                {
                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.SmirkAskLately".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.BlushesNotMyFault".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.BurstOutLaughingSlumps".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.NoseUpShouldBe".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 5)
                        OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.LaughsMockinglyJealous".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0)
                        if (CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == 1)
                        {
                            if (SceneInstance<UrtaPregsScene>.Get().UrtaKids() == 0)
                                OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.CantBelievePregnant".Localized());
                            else
                                OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.CantBelievePregnantAgain".Localized());
                        }
                        else
                            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.PregnantDisbeliefSigh".Localized());

                    OutputText("Scenes.Urtascene.UrtaDiscussesEdrynRomance.GoodToSeeYou".Localized());

                    ReturnToCamp(1);

                    UrtaLove(.5f);
                }

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));
        }

        public void UrtaDiscussesEdrynNonLoveAccept()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynNonLoveAccept.ThinkLongCannotHold".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynNonLoveAccept.GentleSmileUnderstand".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynNonLoveAccept.ShareDrinkExcuse".Localized());

            UrtaLove(1.5f);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEdrynNonLoveREJECT()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynNonLoveREJECT.FeelBetrayedSleeping".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynNonLoveREJECT.LooksHurtStony".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynNonLoveREJECT.DeclareSuitsFineLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEdrynLoveHappy()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveHappy.LookPositiveTrustHer".Localized());

            UrtaLove(5);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEdrynLoveTolerate()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveTolerate.SighCouldntBlameHer".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveTolerate.ConsidersThanks".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveTolerate.NothingElsePoliteLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcDislikesUrtaAndEdrynTogether, 1);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesEdrynLoveBreakup()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveBreakup.FeelBetrayedSleeping".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveBreakup.LooksHurtStony".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesEdrynLoveBreakup.DeclareSuitsFineLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, -1);

            ReturnToCamp(1);
        }

        public void UrtaDiscussesWatchMembership()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.CuriousMembership".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.AsksMembershipConfirm".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.GettingInSimple".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.ShrugsNumbersTricky".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.FinishesSmilesRecruits".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.AnswerNegative".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.AddsServeUnderMe".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.SmirkThatFigured".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.SmilesJoiningEasy".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchMembership.ThankExplanationNote".Localized());

            ReturnToCamp(1);

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);
        }

        public void UrtaDiscussesWatchRole()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchRole.CuriousDailyDuties".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesWatchRole.ThoughtfulTakesDrink".Localized());

            if (Player.Corruption < 66)
                OutputText("Scenes.Urtascene.UrtaDiscussesWatchRole.AgreeImportantWork".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaDiscussesWatchRole.NodWrappedAroundFinger".Localized());

            ReturnToCamp(1);

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);
        }

        public void UrtaDiscussesTheWatchsReach()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.AskHowFarAuthority".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.AsksChaseOutside".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.UnderstandViewAskPatrols".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.StatesRarelyVolunteers".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.DigestAskBeenOutside".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.AdmitsFewTimes".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.MoroselyFinishesDrink".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesTheWatchsReach.ThankExplanationLeave".Localized());

            ReturnToCamp(1);

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);
        }

        public void UrtaDiscussesCrimeAndPunishment()
        {
            ClearOutput();

            UrtaSprite();

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.CuriousAboutLaw".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.TraceOfSuspicion".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.PressOnLawChanges".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.LawsOnRape".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.AskWhatsLegal".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.PublicDecencyLaws".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.AidingAndAbetting".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.SlugOfWhiskey".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.GirlfriendShock".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.IncubusDraftSurprise".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.AskAboutPunishments".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.CorruptionCrimes".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.SafetyConcern".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.WatchAndCouncil".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.PuzzledSilence".Localized());

            OutputText("Scenes.Urtascene.UrtaDiscussesCrimeAndPunishment.ThankForDiscussion".Localized());

            if (Player.Intelligence < 20)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 40)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            if (Player.Intelligence < 60)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            UrtaLove(.5f);

            ReturnToCamp(1);
        }

        public void SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs()
        {
            ClearOutput();

            ShowImage("urta-bar-male-spank");

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.DeclineHerOffer".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.SnarlsDrunkenly".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.ShoveTowardsTable".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.YelpsAtFirstSwat".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.CommandOtherCheek".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.FriendWord".Localized());
            else
                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.LoverWord".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.RudeDrunkBitch".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.LikeThatDoYou".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.SwatAgainGrabHips".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.YellsBeetRed".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.ThreeHitCombo".Localized());

            OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.LearnedManners".Localized());

            if (Player.HasCockThatFits(UrtaCapacity()))
            {
                int y = Player.CockThatFits(UrtaCapacity()) + 1;

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.FreeYourBeast".Localized(
                    y
                ));

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.PressAgainstPucker".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.HardestSpank".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.ClampsDownHard".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.LikesItProstate".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.FiveRapidSwats".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.SpentYourLoad".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.FilledHerUtterly".Localized());
                    else
                        OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.CumLeaksOut".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.DesperateToCum".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.GetsOffFromSensation".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.SpankMeHarder".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.ObligeUntilStings".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.VulpineLoverCalms".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.FallsFaceFirst".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.OneLastPalm".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
            }
            else
            {
                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.IntoUrtasSeat".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.HangsHeadWiggles".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.ShrieksSquirming".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.HardWedgedUpAss".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.AlternateSpankToy".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.ScreamsThenGushes".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.PropsHerOnLap".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.FallsFaceFirstAgain".Localized());

                OutputText("Scenes.Urtascene.SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs.OneLastSlap".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 40f));
            }

            ReturnToCamp(1);
        }

        public void UrtaDiscussessKatherine()
        {
            ClearOutput();

            int affection = CoC.Instance.Flags.Get(GameFlagIds.KatherineUrtaAffection);

            if (affection == 0)
            {
                OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.AskAboutRecruit".Localized());

                OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.KatherineDoingWell".Localized());
            }
            else
                if (affection == 1)
                    OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.ThingForHer".Localized());
                else
                    if (affection < 11)
                    {
                        OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.WhyYouLikeHer".Localized());

                        OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.SwigAndHug".Localized());
                    }
                    else
                        if (affection < 32)
                        {
                            OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.DreamyThenLaughs".Localized());

                            OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.WarnAboutKatherine".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.DontSceneinstanceScorpionsceneGetFuck".Localized(), UrtaKathSexDont);

                            AddButton(1, "Common.Button.Whenever".Localized(), UrtaKathSexWhenever);

                            AddButton(2, "Common.Button.Encourage".Localized(), UrtaKathSexEncourage);

                            return ;
                        }
                        else
                            OutputText("Scenes.Urtascene.UrtaDiscussessKatherine.NeverExpectedThis".Localized());

            ReturnToCamp(1);
        }

        public void UrtaKathSexDont()
        {

            OutputText("Scenes.Urtascene.UrtaKathSexDont.PreferWhenAround".Localized());

            OutputText("Scenes.Urtascene.UrtaKathSexDont.WeakSmileUnderstands".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaDate, KatherineScene.KDATE_LITTLE);

            UrtaDialogueMenu();
        }

        public void UrtaKathSexWhenever()
        {

            OutputText("Scenes.Urtascene.UrtaKathSexWhenever.ArmAroundUrta".Localized());

            OutputText("Scenes.Urtascene.UrtaKathSexWhenever.RelievedLaugh".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaDate, KatherineScene.KDATE_WHENEVER);

            UrtaDialogueMenu();
        }

        public void UrtaKathSexEncourage()
        {

            OutputText("Scenes.Urtascene.UrtaKathSexEncourage.WhatWouldSheSay".Localized());

            OutputText("Scenes.Urtascene.UrtaKathSexEncourage.LovesTheIdea".Localized());

            OutputText("Scenes.Urtascene.UrtaKathSexEncourage.KissAndReassure".Localized());

            OutputText("Scenes.Urtascene.UrtaKathSexEncourage.CarteBlancheWarning".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineUrtaDate, KatherineScene.KDATE_LOTS);

            UrtaDialogueMenu();
        }

        public void GiveTheFoxSomeEggs()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) > 0)
            {
                RepeatUrtaEgging();

                return ;
            }

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.WinningSmileSashay".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.EyesWetWithTears".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.CutThatOff".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.FunAsItMight".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.NotAGoodIdea".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.SlipOutTheDoor".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.FarDifferentNeed".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.PositioningCentaur".Localized());
            else
                if (Player.IsDrider())
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.DriderBodyOverBack".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.ReleaseOvipositor".Localized());

            if (Player.HasPerk(PerkIds.SpiderOvipositor))
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.GreenSlime".Localized());
            else
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.HoneyAgain".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.AnticipationOfLaying".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.MoansLikeSluttyDrunk".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.YourOwn".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.CockWord".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.AndWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.VaginaWord".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.AreWord".Localized());
                else
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.IsWord".Localized());

                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.GrowsWetWithArousal".Localized());
            }
            else
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.EntiretyOfSexualWorld".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.ReachTheCervix".Localized());

            if (Player.HasPerk(PerkIds.BeeOvipositor))
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.HoneyAgain".Localized());
            else
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.SpiderGoo".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.KeepUrtaMoist".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.BarksAndWhimpers".Localized());

            float wetness = 0;

            if (Player.HasVagina())
                wetness += Player.Wetness() * 100;

            if (Player.HasCock())
                wetness += Player.CumQ();

            if (wetness < 300)
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.Splattering".Localized());
            else
                if (wetness < 500)
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.Matting".Localized());
                else
                    if (wetness < 1000)
                        OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.Soaking".Localized());
                    else
                        OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.Flooding".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.FoxWithGushing".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.CumWord".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.FemcumWord".Localized());
                else
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.MixedSexualFluids".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.EggsRollDown".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.BackFromRelease".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.SentencePeriodAgain".Localized());

            if (Player.Eggs() < 20)
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.StomachStartsSwell".Localized());
            else
                if (Player.Eggs() < 40)
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.WombSwellsBulges".Localized());
                else
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.StuffWithEggs".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.EggsAllSpent".Localized());

            OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.DrunkenSnore".Localized());

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.CantLeaveHer".Localized());

                if (Player.FertilizedEggs() == 0)
                    OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.EvenIfInfertile".Localized());

                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.HeaveFoxUpright".Localized());
            }
            else
                OutputText("Scenes.Urtascene.GiveTheFoxSomeEggs.ShrugAndLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaEggs, Player.Eggs());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaFertileEggs, Player.FertilizedEggs());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimesEggPregged, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) + 1);

            if (Player.HasPerk(PerkIds.BeeOvipositor))
                Player.KnockUp(PregnancyIds.BeeEggs, 72, 100, 1);
            else
                Player.KnockUp(PregnancyIds.DriderEggs, 72, 100, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaFlatbellyNotice, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaEggForceEvent) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaEggForceEvent, 48);

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            ReturnToCamp(1);
        }

        public void UrtaChewsOutPC(bool newScreen = true)
        {
            ClearOutput();

            if (!(newScreen))
                OutputText("Scenes.Urtascene.UrtaChewsOutPC.ParagraphBreak".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.SnortsFiercely".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.PullUpASeat".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.StuffedFowlReady".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.PointOutSheAgreed".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.WasDrunkExcuse".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.DidntEnjoyIt".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Urtascene.UrtaChewsOutPC.ClosestToPregnancy".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.QuestionMark".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.FullBodyFlinch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertileEggs) == 0)
            {
                OutputText("Scenes.Urtascene.UrtaChewsOutPC.TheyreAllBlanks".Localized());

                if (Player.Libido > 49 && Player.HasVagina())
                {
                    OutputText("Scenes.Urtascene.UrtaChewsOutPC.CouldMakeBabies".Localized());

                    OutputText("Scenes.Urtascene.UrtaChewsOutPC.ShiftsInSeat".Localized());
                }
            }
            else
                if (Player.HasPerk(PerkIds.BeeOvipositor))
                    OutputText("Scenes.Urtascene.UrtaChewsOutPC.HatchIntoBeeSwarms".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtaChewsOutPC.GiveThemToTailor".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.CooledDownOkay".Localized());

            UrtaLove(-10);

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.KissEmbarrassedFox".Localized());

            if (Player.HasPerk(PerkIds.SpiderOvipositor))
                OutputText("Scenes.Urtascene.UrtaChewsOutPC.SpiderWord".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaChewsOutPC.LittleBee".Localized());

            OutputText("Scenes.Urtascene.UrtaChewsOutPC.SayEndParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaEggForceEvent, -1);

            if (newScreen)
                ReturnToCamp(1);
        }

        public void RepeatUrtaEgging()
        {
            ClearOutput();

            ShowImage("urta-egg-repeat");

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.AskToLayAgain".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.GigglesAndTaps".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.HangingOutStockings".Localized());
            else
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.BulgingHerDress".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.SashaysOutAlley".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.FollowFoxGooTrail".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.TugOffArmor".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.FarDifferentNeedAgain".Localized());

            if (Player.IsTaur() || Player.IsDrider())
            {
                if (Player.IsTaur())
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.PositioningCentaurAgain".Localized());
                else
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.DriderBodyAgain".Localized());

                OutputText("Scenes.Urtascene.RepeatUrtaEgging.CommaSpace".Localized());
            }

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.ReleaseOvipositorAgain".Localized());

            if (Player.CanOvipositBee())
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.HoneyAgain".Localized());
            else
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.GreenSlime".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.AnticipationOfLaying".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.MoansLikeSluttyDrunk".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.YourOwn".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.CockWord".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.AndWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.VaginaWord".Localized());

                OutputText("Scenes.Urtascene.RepeatUrtaEgging.GrowsWetWithArousal".Localized());
            }
            else
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.EntiretyOfSexualWorld".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.ReachTheCervix".Localized());

            if (Player.CanOvipositBee())
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.HoneyAgain".Localized());
            else
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.SpiderGoo".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.KeepUrtaMoist".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.BarksAndWhimpers".Localized());

            float wetness = 0;

            if (Player.HasVagina())
                wetness += Player.Wetness() * 100;

            if (Player.HasCock())
                wetness += Player.CumQ();

            if (wetness < 300)
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.Splattering".Localized());
            else
                if (wetness < 500)
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.Matting".Localized());
                else
                    if (wetness < 1000)
                        OutputText("Scenes.Urtascene.RepeatUrtaEgging.Soaking".Localized());
                    else
                        OutputText("Scenes.Urtascene.RepeatUrtaEgging.Flooding".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.FoxWithGushing".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.CumWord".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.FemcumWord".Localized());
                else
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.MixedSexualFluids".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.EggsRollDown".Localized());

            if (wetness > 0)
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.BackFromRelease".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.SentencePeriodAgain".Localized());

            if (Player.Eggs() < 20)
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.StomachStartsSwell".Localized());
            else
                if (Player.Eggs() < 40)
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.WombSwellsBulges".Localized());
                else
                    OutputText("Scenes.Urtascene.RepeatUrtaEgging.StuffWithEggs".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.EggsAllSpent".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.WeirdestFuckEver".Localized());
            else
                OutputText("Scenes.Urtascene.RepeatUrtaEgging.DopeyGrinSad".Localized());

            OutputText("Scenes.Urtascene.RepeatUrtaEgging.SloppyKissGoodbye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaEggs, Player.Eggs());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaFertileEggs, Player.FertilizedEggs());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimesEggPregged, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesEggPregged) + 1);

            if (Player.HasPerk(PerkIds.BeeOvipositor))
                Player.KnockUp(PregnancyIds.BeeEggs, 72, 100, 1);
            else
                Player.KnockUp(PregnancyIds.DriderEggs, 72, 100, 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaFlatbellyNotice, 0);

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            ReturnToCamp(1);
        }

        public void CondomlessUrtaInHouseSmex()
        {
            ClearOutput();

            OutputText("Scenes.Urtascene.CondomlessUrtaInHouseSmex.YankTheCondom".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.CondomlessUrtaInHouseSmex.GrumblesAndSulks".Localized());
            else
                OutputText("Scenes.Urtascene.CondomlessUrtaInHouseSmex.RollsEyesFine".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(UrtaCapacity()))
                AddButton(0, "Common.Button.PegHerAss".Localized(), PegUrtasAssMessy);
            else
                AddButton(0, "Common.Button.PegHerAss".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.FillMeUp".Localized(), FillMeUpPleaseUrta);
            else
                AddButton(1, "Common.Button.FillMeUp".Localized(), null);

            if (!Player.IsGenderless() && Player.HasTailInsteadOfLegs())
                AddButton(2, "Common.Button.Tailfun".Localized(), NagaOnUrtaMess);
            else
                AddButton(2, "Common.Button.Tailfun".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), GoBackToUrtasForLuvinz);
        }

        public void PegUrtasAssMessy()
        {
            ClearOutput();

            ShowImage("urta-home-pegurta");

            var x = Player.CockThatFits(UrtaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.FondlingFoxBalls".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.RemoveArmorOneHanded".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.ASpace".Localized());
            else
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.YourSpace".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.DownOntoSheets".Localized(
                Player.CockDescript(x)
            ));

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.GigglesDrunkenly".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) < 11)
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.SoftlyBegs".Localized());
                else
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.EagerlyCoos".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.ScootingForward".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.GentlyRockHips".Localized(
                Player.CockHead(x)
            ));

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.DrunkenSpace".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.WhimpersOfPleasure".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.SayStartIts".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.Sho".Localized());
            else
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.So".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.DifferentThanCondom".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 15)
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.Casually".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 11)
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.Confidently".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 4)
                        OutputText("Scenes.Urtascene.PegUrtasAssMessy.LittleNervously".Localized());
                    else
                        OutputText("Scenes.Urtascene.PegUrtasAssMessy.WithShakyHand".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.StrokesPaddedFingertip".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.SmilingAtDisplay".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) > UrtaCapacity())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.AchingPressureDoubles".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (Player.HasKnot(x))
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.PleasureDoubles".Localized());
                else
                {
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.AchingPleasureRedoubles".Localized(
                        Player.CockDescript(x)
                    ));

                    if (!Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Urtascene.PegUrtasAssMessy.GroinSpace".Localized());
                    else
                        OutputText("Scenes.Urtascene.PegUrtasAssMessy.SheathSpace".Localized());

                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.HeavyLurch".Localized());
                }

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.GropingVixenTits".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.SimperingGuardCaptain".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.LungeBackLeavingEmpty".Localized());

            if (Player.CockArea(x) > UrtaCapacity())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.MostOfYouSpace".Localized());
            else
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.EntireLengthSpace".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.ClingsToCockFlesh".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.UrtaMoansFuckMe".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.HAgain".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.WhyDoesItFeel".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.HAgain".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.OohGooohYes".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.SlapAgainstButtcheeks".Localized());
            else
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.SlapGroinButtcheeks".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.MountingTightTunnel".Localized());

            if (Player.HasKnot(x) && Player.CockArea(x) < UrtaCapacity())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.BarelyNoticeKnot".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.PinchNippleSlamHome".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.OnyxAssholeWrings".Localized());

            if (Player.HasKnot(x) && Player.CockArea(x) < UrtaCapacity())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.KnotBiggerHarder".Localized());
            else
            {
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.WorksYouHard".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.GoodThingMore".Localized());
            }

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.FoxBecomesFountain".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) < 0)
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.NotSoFertileSpace".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.FluidDangleJawline".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.TightInGrip".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.UnableBearTightness".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasKnot(x) && Player.CockArea(x) < UrtaCapacity())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.LewdPopFreeingKnot".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.ExitingSqueezingAnus".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.EyesLockOnValley".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.AndOfCourseAlcohol".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.KissingEveryTimePeek".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.SpunkVixenBounces".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.SwallowingSmileWink".Localized());
            else
            {
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.ForcedToClose".Localized());

                if (Player.CumQ() > 750)
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.WashHerFace".Localized());

                if (Player.CumQ() >= 1200)
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.OrgasmGoesOn".Localized());

                if (Player.CumQ() > 3000)
                {
                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.GaspsFloodThe".Localized());

                    if (Player.CumQ() > 10000)
                        OutputText("Scenes.Urtascene.PegUrtasAssMessy.FirstFewFeetSpace".Localized());

                    OutputText("Scenes.Urtascene.PegUrtasAssMessy.RoomLookingUp".Localized());
                }
            }

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.FinishEmptyingSag".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.NuzzlesCheek".Localized());
            else
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.SheepishlyPullsBack".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.ClimbingOutOfBed".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.Blushing".Localized());
            else
                OutputText("Scenes.Urtascene.PegUrtasAssMessy.SmilingDevilishly".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.PegUrtasAssMessy.UrtaSmirksGlancing".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f), new DynStat(GameStats.Sensitivity, -3f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 2);

            UrtaLove(2);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            Player.SlimeFeed();

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtaGetsPeggedCleanUp);
        }

        public void UrtaGetsPeggedCleanUp()
        {
            ClearOutput();

            if (Player.Corruption >= 50)
                OutputText("Scenes.Urtascene.UrtaGetsPeggedCleanUp.StartToDuckOut".Localized());

            OutputText("Scenes.Urtascene.UrtaGetsPeggedCleanUp.CleanUpWithHelp".Localized());

            OutputText("Scenes.Urtascene.UrtaGetsPeggedCleanUp.TentingHerApron".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.UrtaGetsPeggedCleanUp.WaveWord".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaGetsPeggedCleanUp.TenderHug".Localized());

            OutputText("Scenes.Urtascene.UrtaGetsPeggedCleanUp.SendsOnYourWay".Localized());

            ReturnToCamp(4);
        }

        public void FillMeUpPleaseUrta()
        {
            ClearOutput();

            ShowImage("urta-home-female-fucked");

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.DiscardLatexCaress".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.HoldingBeastThighs".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ProdigiousSpace".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.TongueUponTurgidMass".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.HipsSilentlyWork".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ReleaseFromOral".Localized());

            if (Player.Wetness() > 3)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.UtterlySoaked".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.HardErectStands".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SheReadyYouReady".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ReleaseGuardswomanHands".Localized());

            if (Player.Wetness() > 3)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.DropInBucket".Localized());
            else
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.StainingYourSlit".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SentencePeriod".Localized());

            if (Player.Vaginas[0].Virgin)
            {
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.NervousSmileVirgin".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.VixenSitsBoltUpright".Localized());

                if (Player.Tallness > 80)
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ClitWord".Localized());
                else
                    if (Player.Tallness > 60)
                        OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.BellyWord".Localized());
                    else
                        OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ChestWord".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.CranesNeckToMeet".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.WrapsArmsAround".Localized());

                if (UrtaDrunk())
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.DrunkenlySpace".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.AsksAreYou".Localized());

                if (UrtaDrunk())
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.H".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.YouDontHaveTo".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.RuffleHairNod".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SituationAlmostPainful".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.DroolLeaks".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.TakenUrtaHalfway".Localized());

                Player.CuntChange(30, true, true, false);

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.UndauntedSpurredOn".Localized());

                Player.CuntChange(60, true, true, false);

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.RestInHerLap".Localized());

                UrtaLove(15);
            }
            else
            {
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SquatDownEnsnare".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.EyesRollBack".Localized());

                Player.CuntChange(60, true, true, false);
            }

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.GrabsYouByHips".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.FoxReluctantlyNods".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ExhibitionistThrill".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.LeaningForwardHandsBosom".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.DontNoticeDripping".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.OneCockInHand".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.FirstSignOfOrgasm".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.FollowedInShortOrder".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.VaginaGoesWild".Localized());

                if (Player.CumQ() > 400)
                {
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SoakingHer".Localized());

                    if (Player.CumQ() > 1000)
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.AndHerBed".Localized());
                        else
                            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.DamnNearEverything".Localized());

                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.WithYourEjaculate".Localized());
                }

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.BloomOfWarmth".Localized());
            }
            else
            {
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.MashingGlisteningDelta".Localized());

                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.FirstSignAgain".Localized());
            }

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.OddShiveringMotion".Localized());

            if (Player.Thickness < 50 && (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 200))
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.OutlineOnAbdomen".Localized());
            else
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.YourVaginaToo".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SetsYouAflutter".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.BubbleHappily".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.StillDoesntConcern".Localized());

            if (Player.BiggestBreastSize() >= 4)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ChestRestsHeavily".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SentencePeriodAgain".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.BeyondPregnant".Localized());

            if (Player.IsGoo() && Player.VaginalCapacity() > 9000)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.EruptionsMakeBigger".Localized());
            else
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.EruptionsDoLittle".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.PussySpentGoesSlack".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.PaintingWallsWhite".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SnuggleWithVixen".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.EvenMoreSpace".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.PregnantForAWhile".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.NuzzlesCheek".Localized());
            else
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SheepishlyPullsBack".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.ClimbingOutOfBed".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.Blushing".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.Grinning".Localized());
                else
                    OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.OpenlyLeering".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.SentencePeriodThird".Localized());

            OutputText("Scenes.Urtascene.FillMeUpPleaseUrta.UrtaSmirksSlickening".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CleanUpAfterUrtaCreamInjectsYou);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 2);

            UrtaLove(2);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) + 1);

            Player.SlimeFeed();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) == 1)
                Player.KnockUp(PregnancyIds.Urta, PregnancyIds.UrtaIncubation, 25);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f), new DynStat(GameStats.Sensitivity, -3f));
        }

        public void CleanUpAfterUrtaCreamInjectsYou()
        {
            ClearOutput();

            if (Player.Corruption >= 50)
                OutputText("Scenes.Urtascene.CleanUpAfterUrtaCreamInjectsYou.StartToDuckOut".Localized());

            OutputText("Scenes.Urtascene.CleanUpAfterUrtaCreamInjectsYou.CleanUpWithHelp".Localized());

            OutputText("Scenes.Urtascene.CleanUpAfterUrtaCreamInjectsYou.TentingHerApron".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.CleanUpAfterUrtaCreamInjectsYou.WaveWord".Localized());
            else
                OutputText("Scenes.Urtascene.CleanUpAfterUrtaCreamInjectsYou.TenderHug".Localized());

            OutputText("Scenes.Urtascene.CleanUpAfterUrtaCreamInjectsYou.SendsOnYourWay".Localized());

            ReturnToCamp(4);
        }

        public void LippleBondUrtaScylla()
        {
            ClearOutput();

            ShowImage("urta-syclla-3some");

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.ApproachBustyDuo".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.ReachBetweenUrtaLegs".Localized());

            if (UrtaLove())
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.WhySheGetsKisses".Localized());
            else
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.EntrancedWithScylla".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SlipTipInsideMound".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.BreakTheKiss".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.PullAtHerHorns".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.UrtaQuiversExcitement".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.FeelScyllaHeartbeat".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.HandMovesAway".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.BarrageIntensified".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SuckedInsideBreast".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.EitherWord".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.RavenousLippleTitflesh".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.OrMoreDelicately".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.BreastTightens".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.TurnTowardsFox".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.TitfleshPressure".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.UrtaPantsHowls".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.ThishTooAwesome".Localized());

            if (Player.Sensitivity < 29)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.TellFromExpression".Localized());
            else
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.TitfleshShifts".Localized());

            if (Player.Sensitivity >= 44)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.FeelPressureIncreasing".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SlideHandDownButtocks".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.ScyllaFeverishGaze".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.WantFeelYouSpurt".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.GunnaCumUhmmm".Localized());
            else
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.LooksCumWithMe".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.TheirEncouragements".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.HandsOfBothWorking".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.EnoughToPushClose".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.ScyllaLosesComposure".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.ShootingOffIntoMe".Localized());

            if (Player.Sensitivity < 59)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SoundOfUrtaClimax".Localized());
            else
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SimplyTooMuch".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SloppyWetBox".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.HandQuickens".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.OwnHandTravelsDown".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.LoadSpillsTwoLovers".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.MindDriftsAway".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.RealizeOnlyBitMore".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.StrainedWhimperPullsOut".Localized());

            if (Player.HasKnot(Player.BiggestCockIndex()))
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.NotMakingEasier".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.UrtaSauntersOver".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.NeedToTryAgain".Localized());
            else
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.LoverThatWasAmazing".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.TookAllHerFocus".Localized());

            if (Player.HasKnot(Player.BiggestCockIndex()))
                OutputText("Scenes.Urtascene.LippleBondUrtaScylla.KnotDeflatedNow".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.SuckingAtAir".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.WorriedLookAtScylla".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.IllBeFineSoon".Localized());

            OutputText("Scenes.Urtascene.LippleBondUrtaScylla.HelpWomenLieDown".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            if (Player.CumQ() < 500)
                Player.CumMultiplier += 2;

            if (Player.CumQ() < 1500)
                Player.CumMultiplier += 2;

            if (Player.CumQ() < 5000)
                Player.CumMultiplier += 2;

            if (Player.CumQ() < 20000)
                Player.CumMultiplier += 2;

            ReturnToCamp(1);
        }

        public void NagaOnUrtaMess()
        {
            ClearOutput();

            ShowImage("urta-naga-fuck");

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.TellHerPleasurePlan".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.WorriedAboutTail".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.CalmHerAskTrust".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.PondersAndTrusts".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.MoreConfidentEager".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.CrawlOntoBed".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.GoAheadTryRelax".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.TailWrapsWaist".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.SurpriseNoWrap".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.GaspsTailOnShaft".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.HandsBodyFaster".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.BowsToReturnFavor".Localized());

            if (Player.CockTotal() == 0)
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.FingersIntoSlit".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Urtascene.NagaOnUrtaMess.FavoriteCockWorld".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Urtascene.NagaOnUrtaMess.TwoCocksTease".Localized());
                    else
                        if (Player.CockTotal() == 3)
                            OutputText("Scenes.Urtascene.NagaOnUrtaMess.SpareOneJustCase".Localized());
                        else
                            OutputText("Scenes.Urtascene.NagaOnUrtaMess.BouquetOfCocks".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.LeansToReturnFavor".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.TailTeasesBalls".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.TailEntersPussy".Localized());

            if (Player.CockTotal() == 0)
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.TongueInsideVagina".Localized());
            else
                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Urtascene.NagaOnUrtaMess.LipsLockAroundCock".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.ShaftLeavesMouth".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.SuckingReachesStrokes".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.MinistrationsMoreForceful".Localized());

            if (Player.CockTotal() == 0)
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.LickingVaginaClit".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.OralStimulationWorks".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.CoilingSqueezesCock".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.PussyConvulsesToo".Localized());

                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Urtascene.NagaOnUrtaMess.FemcumShootsOut".Localized());
            }

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Urtascene.NagaOnUrtaMess.SuckingPushesOver".Localized());

                if (Player.CumQ() >= 1000 && Player.CumQ() < 2500)
                    OutputText("Scenes.Urtascene.NagaOnUrtaMess.SwollenTummyEager".Localized());
                else
                    if (Player.CumQ() >= 2500)
                        OutputText("Scenes.Urtascene.NagaOnUrtaMess.ExcessiveLoadOverflow".Localized());
            }

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.StrokeTailpumpDuring".Localized());

            OutputText("Scenes.Urtascene.NagaOnUrtaMess.TenderAfterglow".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .25f));

            if (Player.HasCock())
            {
                if (Player.CumQ() < 500)
                    Player.CumMultiplier += 2;

                if (Player.CumQ() < 1500)
                    Player.CumMultiplier += 2;

                if (Player.CumQ() < 5000)
                    Player.CumMultiplier += 2;

                if (Player.CumQ() < 20000)
                    Player.CumMultiplier += 2;
            }

            ReturnToCamp(1);
        }

        public void LickOutUrtaAtHome()
        {
            ClearOutput();

            var x = Player.CockThatFits(UrtaCapacity());

            if (x < 0)
            {
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.PalesTooBig".Localized());

                AddButton(0, "Common.Button.Next".Localized(), UrtaHomeLuvLuvinsMenu);

                return ;
            }

            ShowImage("urta-home-selfbj");

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SuggestsIdea".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SlursFragment".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.AsksFragment".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.TakenAbackFirst".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.LickChopsNod".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SayStartThenBetter".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.HLetter".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.UreDontMakeToo".Localized());

            if (!UrtaDrunk())
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SLetter".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.HLetterAgain".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.HuhReachingOver".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.PurrsHeftsNuts".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.ArmorRemovesItself".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.PrickVixenExamines".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.ClimbOvertopUrta".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) >= UrtaCapacity() / 2)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.FitsLikeGlove".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.RoomyFullControl".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.PushRestWay".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) >= 200)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.CuntDilatesEase".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.LiftsNippleSuck".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.BedCreaksOminous".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.CantHoldItIn".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.PussyClenchesDown".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.CondomBalloonGrows".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.BalloonOnTits".Localized());

            if (Player.Corruption < 66)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.YoureWonderful".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.AmazingFuck".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.DazzlingSmileResume".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SqueezeBreastHump".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SpermyJuicesStream".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SurprisedGrinBelly".Localized());

            if (Player.CumQ() >= 3000)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.VirilePeckerWont".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.IntimateMomentWith".Localized());

            if (UrtaDrunk())
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.DrunkenFragment".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.LoverKissAgain".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.KnottedFragment".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.RestingFragment".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.AsksAnotherOne".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.ShakeHeadPullBack".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.AttemptWithdrawKnot".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.WithdrawFragment".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.CumRunsPackedWomb".Localized());
            else
                OutputText("Scenes.Urtascene.LickOutUrtaAtHome.CumDroolsGaping".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SpreadsPussyLips".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.LipstickLine".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.ThighsScissorYou".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.LickUnderBallsac".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.PantsGonnaCum".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.SqueezeHerBalls".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.BendsDoubleSwallow".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.HandsOnCockPump".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.FlarePopsOut".Localized());

            OutputText("Scenes.Urtascene.LickOutUrtaAtHome.RubSpentBalls".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 12);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 3);

            UrtaLove(3);

            Player.Orgasm("Lips");

            Player.SlimeFeed();

            KnockUpUrtaChance();

            if (Player.LibidoPercent > 50)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.LibidoPercent > 90)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            ReturnToCamp(1);
        }

        public void SlurpFawkesCocksForFunAndInflation()
        {
            ClearOutput();

            ShowImage("urta-home-bj");

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.ThanksLoverNeedThis".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.SighAtPrick".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.MoansHauledBack".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.AudibleGaspDisguise".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.ShadowIsBarmaid".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.ReelsBackDistracted".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.WhyNoOneNotices".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.PulseDominatesMouth".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.TenthPumpFull".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.LoseCountBegging".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.SurveyNewBelly".Localized());

            if (Player.Pregnancy.Incubation < 100 && Player.Pregnancy.Incubation > 0)
                OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.ThanItWas".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.SlideBackSeat".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.LiftEquipmentBreath".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.ReassureHerOkay".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.WaitressApproaches".Localized(
                Player.Mf("cutey", "girly")
            ));

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.TooMuchFluids".Localized());

            OutputText("Scenes.Urtascene.SlurpFawkesCocksForFunAndInflation.SayGoodbyeSloshing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            Player.SlimeFeed();

            Player.RefillHunger(100);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaCumNoCumDays, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 3);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 5);

            UrtaLove(2);

            if (Random.Shared.Next(2) == 0)
                OutputText(Player.ModThickness(100, 2));

            ReturnToCamp(1);
        }

        public void UrtaAndRaphaelSurprise()
        {
            ClearOutput();

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.MischievousGrinWhisper".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.PurrsDrunkenlyStumbles".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.OtherHalfPlan".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.CocksTwingeExcited".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.NapkinAndPen".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.LetterDearestRogue".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.LetterFondMemory".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.LetterAwaitingThief".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.Text".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.ShouldDoIt".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.PompousFragment".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.GingerFoxCannot".Localized());

            OutputText("Scenes.Urtascene.UrtaAndRaphaelSurprise.SaunterBackBar".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), RaphaelAndUrtaPartTwo);
        }

        public void RaphaelAndUrtaPartTwo()
        {
            ClearOutput();

            ShowImage("urta-raphael-3some");

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.TailsEncircleWaist".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.TurnHeadSmiling".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.RebutsHandsSqueeze".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.CoylyAskProposing".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.TakeHandLead".Localized());

            if (!UrtaLove())
                OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.Friend".Localized());
            else
                OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.Lover".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.StaysOnHeels".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.TriesAskKissed".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.MuscularArmsWrap".Localized());

            if (Player.Tallness >= 60)
                OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.GrindsAssCleavage".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.GruntsManhandled".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.WhimperIntoLips".Localized());

            Player.CuntChange(12, true, true, false);

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.FingerInPuckered".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.LiftTailsAnd".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.TryRelaxThick".Localized());

            if (Player.AnalCapacity() < 30)
                OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.SphincterStretchedSlow".Localized());
            else
                if (Player.AnalCapacity() < 60)
                    OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.SlidingIntoSphincter".Localized());
                else
                    OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.EasilyInsideAnus".Localized());

            Player.ButtChange(60, true, true, false);

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.BreaksKissThrusts".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.ReassuringPeck".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.FingerPushesRosebud".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.SpiralsOutControl".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.HeadBackOnShoulder".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.SpermPumpsExhaust".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.MuttersDisconsolately".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.SlursDoubleStuffed".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.GetDressedMiddle".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.HimHerExclaim".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.DoorSlamsFlight".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.UrtaGrowls".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.KissNosePress".Localized());

            OutputText("Scenes.Urtascene.RaphaelAndUrtaPartTwo.RaphaelWontFall".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaXRaphaelHappened, CoC.Instance.Flags.Get(GameFlagIds.UrtaXRaphaelHappened) + 1);

            ReturnToCamp(1);
        }

        public void UrtaGooTesticleVoreRuinedOrgasms()
        {
            ClearOutput();

            ShowImage("urta-goo-fuck");

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.TellSpecialInMind".Localized());

            if (!silly)
                OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.SteelRod".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.MarethianAlgebra".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.AsksCuriously".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.FindOutDueTime".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.GripHorsecockFirm".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.HipsBuckOnahole".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.GroansGrowLouder".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.AsksEyesWidening".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.ChuckleHowFar".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.FingerWidensHole".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.CryCutShort".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.TrepidationCrosses".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.PumpHandDistort".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.SquirmsBeneathMinistrations".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.MotionThirdOrgasm".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.FifthSixthShudders".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.DarknessRelaxBody".Localized());

            OutputText("Scenes.Urtascene.UrtaGooTesticleVoreRuinedOrgasms.BodyShiftBumping".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), UrtasRuinedOrgasmsFromGooPartII);
        }

        public void UrtasRuinedOrgasmsFromGooPartII()
        {
            ClearOutput();

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AwakenPanickedShout".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.VoiceStrange".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.PatsEngorgedSack".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.WarmTinglyEncased".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SchlickGoes".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AbsorbingOwnCum".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.DamnedIfYouGo".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SeveralHoursLater".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SackTightensAgain".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.FrustratedGrowl".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.MindReelsCount".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.KnockAtDoor".Localized());

            var edrynAvailable = SceneInstance<EdrynScene>.Get().Pregnancy.IsPregnant;

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.HoofstepsSet".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.FootstepsPair".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EnterShiftQuestion".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.RecognizeEdryn".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.MetOlivia) == 0)
                    OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.RecognizeOlivia".Localized());
                else
                    OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.DontRecognize".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.ShoutsFragment".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.WaitSomethingNew".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynName".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaName".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.NeedYourHelp".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.MomentOfSilence".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.StillChargingYou".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.UhmYesCaptain".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.ArmorFallingGround".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SinksHorsecockIn".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameAgain".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameAgain".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.CuntWetSquelching".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynMoansPleasure".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaShoutsSurprise".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.NotMessingAround".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameThird".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameThird".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.UndersideElectric".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AnotherThrustSlap".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.CockWord".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OrFragment".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.PussyWord".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AssholeWord".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.GooeyBodyGiantClit".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AlmostUncomfortableNeeds".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.WallsConstrictAgain".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.PulledUpThrough".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OozeSpurtsOut".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameFourth".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameFourth".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EyesSearchDarkness".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.YoureInsideStart".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameFifth".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameFifth".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.WombEnd".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.MuffledSighRelief".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameSixth".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameSixth".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.PregnantSeptuplets".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.NervousChuckleSorry".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AllYouNeedPush".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SitBackPush".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.HostGruntsDiscomfort".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.ObligesSittingBack".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.NowPushEncouraging".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SuddenTightness".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OozeOutEveryPush".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameSeventh".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameSeventh".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.FirstOutHead".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynClutchingHand".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaLyingBed".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.SurgeOutShoulders".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameEighth".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameEighth".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OneYouShare".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.CollectReformBody".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.LookFragment".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameNinth".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameNinth".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.ItsAFragment".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.ThingFragment".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.Boy".Localized(
                    Player.Mf("boy", "girl")
                ));

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.LaughClapsShoulder".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameTenth".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaNameTenth".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.NotFunnyToHer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetOlivia) == 0 && !edrynAvailable)
            {
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.AttentionsNewMother".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MetOlivia, CoC.Instance.Flags.Get(GameFlagIds.MetOlivia) + 1);
            }

            if (Player.Body.Get<SkinPart>().Tone != "milky white")
            {
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.CatchSightBody".Localized());

                Player.Body.Get<SkinPart>().Tone = "milky white";

                Player.Body.Get<ArmsPart>().UpdateClaws(Player.Body.Get<ArmsPart>().Claws.Id);

                Player.CumMultiplier += 10;
            }

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.UrtaAndFragment".Localized());

            if (edrynAvailable)
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.EdrynNameEleventh".Localized());
            else
                OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.OliviaDressed".Localized());

            OutputText("Scenes.Urtascene.UrtasRuinedOrgasmsFromGooPartII.CollapseCouchPatrol".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            View.Time.Days += 1;

            View.Time.Hours = 11;

            ReturnToCamp(1);
        }

        public void UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPetplayDone) > 0)
            {
                WearZeCollar();

                return ;
            }

            OutputText("Scenes.Urtascene.UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice.HoldsUpCollar".Localized());

            OutputText("Scenes.Urtascene.UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice.EyesWidenDinnerPlates".Localized());

            OutputText("Scenes.Urtascene.UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice.RetrieveCollarInspect".Localized());

            OutputText("Scenes.Urtascene.UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice.RolePlayingIdea".Localized());

            OutputText("Scenes.Urtascene.UrtaPetPlayDeletedForeverBecauseThirdProovedMeWrongAboutDice.IgnoreOrUseCollar".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Ignore".Localized(), IgnoreUrtasCollar);

            AddButton(0, "Common.Button.Wear".Localized(), WearZeCollar);
        }

        public void IgnoreUrtasCollar()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPetplayDone, -1);

            if (UrtaLove())
                UrtaHomeLuvLuvinsMenu();
            else
                GoBackToUrtasForLuvinzII();
        }

        public void WearZeCollar()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPetplayDone) == 0)
            {
                OutputText("Scenes.Urtascene.WearZeCollar.ReachTrashcanCollar".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.SinkToKnees".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.AsksBarkAffirmative".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.GoodLittleDoggy".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Urtascene.WearZeCollar.HarderFragment".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Urtascene.WearZeCollar.AndFragment".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.WearZeCollar.WetterFragment".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.WonderHerWords".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.OwnerClothesPet".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.StruggleOutArmor".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPetplayDone) == 1)
                {
                    OutputText("Scenes.Urtascene.WearZeCollar.SpyCollarDresser".Localized());

                    OutputText("Scenes.Urtascene.WearZeCollar.TagPropertyOfUrta".Localized());

                    OutputText("Scenes.Urtascene.WearZeCollar.SnapCollarStrip".Localized());

                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Urtascene.WearZeCollar.TailWaggingAgain".Localized());

                    OutputText("Scenes.Urtascene.WearZeCollar.BeamsPatsHead".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtascene.WearZeCollar.SpyCollarAgain".Localized());

                    if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                        OutputText("Scenes.Urtascene.WearZeCollar.TailWaggingAgain".Localized());

                    OutputText("Scenes.Urtascene.WearZeCollar.SentencePeriod".Localized());
                }

            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
            {
                OutputText("Scenes.Urtascene.WearZeCollar.OneLastTouch".Localized());

                if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None && Player.Anal.Wetness > 0)
                    OutputText("Scenes.Urtascene.WearZeCollar.AssholeWetWithShame".Localized());

                OutputText("Scenes.Urtascene.WearZeCollar.PushPlugInside".Localized());
            }

            OutputText("Scenes.Urtascene.WearZeCollar.QuickWalkBedroom".Localized());

            OutputText("Scenes.Urtascene.WearZeCollar.WhatPetWant".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPetplayDone, CoC.Instance.Flags.Get(GameFlagIds.UrtaPetplayDone) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Doggystyle".Localized(), DoggyStyle);

            AddButton(1, "Common.Text.RewardQuoted".Localized(), GetAPetReward);

            AddButton(2, "Common.Button.Walkies".Localized(), UrtaTakesPCOnWalkies);
        }

        public void DoggyStyle()
        {
            ClearOutput();

            ShowImage("urta-home-petplay-doggy");

            OutputText("Scenes.Urtascene.DoggyStyle.WondersAloud".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.CommandingSit".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Urtascene.DoggyStyle.TailWord".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.TailplugWord".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.WagsOwnAccord".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Urtascene.DoggyStyle.AssholeWord".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.PluggedAsshole".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.AndBetweenThighs".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.GraspingYourCock".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.FingersSlipFolds".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.MoanFingersAlong".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.CockHeadPlaceholder".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.ClitPlaceholder".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.KeepsUpperBodyDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.ThumbRubsUnderside".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.FingerToKnuckle".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.BiteLipEcstasy".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.OwnerBacksOff".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.SlapsAssExplains".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.RollOverCommand".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.CockDoesSame".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.VaginaGrowsSlicker".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.TeaseAsLong".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.HandDriftsBelly".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.CockWord".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.PussyWord".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.TakesOverWhere".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.WrapsFingersShaft".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.TwoFingersDeep".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.HandOnChestSwitch".Localized());

            if (Player.TotalNipples() == 2)
                OutputText("Scenes.Urtascene.DoggyStyle.TheFragment".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.AnFragment".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.OtherNippleTweak".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.JerksYouOff".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.RubsInsideOut".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.FlameOfBliss".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.PreCumDribbles".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.FoldsMoreDamp".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.LegKicksAir".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.SentencePeriodAgain".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.PrecipiceOfOrgasm".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.RollOverAgain".Localized());

            if (Player.Body.Get<TailPart>().Id == BodyPartTypes.None)
                OutputText("Scenes.Urtascene.DoggyStyle.GraspsPlugPulls".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.RubsAgainstAsshole".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.BegToFuck".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.WhineUnsure".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.AllEncouragementNeeded".Localized());

            if (Player.BiggestBreastSize() <= 5)
                OutputText("Scenes.Urtascene.DoggyStyle.RestingFoldedArms".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.RestingOnBoobs".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.AssHighAir".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.DoggyStyle.OwnFragment".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Urtascene.DoggyStyle.PainfullyErectMember".Localized());
                else
                    OutputText("Scenes.Urtascene.DoggyStyle.SlitWord".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.OverwhelmedContent".Localized());

            if (!silly)
                OutputText("Scenes.Urtascene.DoggyStyle.AssWord".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.BoypussyWord".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.SentencePeriodThird".Localized());

            Player.ButtChange(60, true, true, false);

            OutputText("Scenes.Urtascene.DoggyStyle.GoodRhythmBalls".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.DickWord".Localized());
            else
                OutputText("Scenes.Urtascene.DoggyStyle.CuntWord".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.TinglesPowerfully".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.DoggyStyle.SlapsBellyGlobules".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Urtascene.DoggyStyle.WhileYour".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DoggyStyle.UnusedPussyAches".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.SentencePeriodFourth".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.LovemakingCrescendo".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.CanineHowlClamp".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtascene.DoggyStyle.TremorsEruptBelly".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Urtascene.DoggyStyle.JerksOnItsOwn".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.DoggyStyle.TravelsToVagina".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.DoggyStyle.ClampsGushingFemcum".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.ShiversLightningArcs".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.SlouchPregnantBelly".Localized());

            OutputText("Scenes.Urtascene.DoggyStyle.EllipsisFragment".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 4);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 2);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PartTwoOfDoggieStyle);
        }

        public void PartTwoOfDoggieStyle()
        {
            ClearOutput();

            View.Time.Hours += 2;

            OutputText("Scenes.Urtascene.PartTwoOfDoggieStyle.SeveralHoursLaterBold".Localized());

            OutputText("Scenes.Urtascene.PartTwoOfDoggieStyle.AwakeInArms".Localized());

            OutputText("Scenes.Urtascene.PartTwoOfDoggieStyle.ConfirmEnjoyedPet".Localized());

            ReturnToCamp(1);
        }

        public void GetAPetReward()
        {
            ClearOutput();

            ShowImage("urta-home-petplay-reward");

            OutputText("Scenes.Urtascene.GetAPetReward.GoodPetSoFar".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Urtascene.GetAPetReward.TailWord".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.TailplugWord".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.EnthusiasticallyFragment".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.HoldsHandSit".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.HardFragment".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Urtascene.GetAPetReward.AndFragment".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.GetAPetReward.WetFragment".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.ThinkingOfReward".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.GetAPetReward.JarPeanutButter".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.SitsOnBedEdge".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.GetAPetReward.PeanutButterFor".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.FootOnSide".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.WagglesFingerNo".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Urtascene.GetAPetReward.OntoChest".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.BetweenBreasts".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.EncircleNippleStiffens".Localized());

            if (Player.TotalNipples() == 2)
                OutputText("Scenes.Urtascene.GetAPetReward.ItsTwin".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.TheOthers".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.SendsDownFurther".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.VaginaPlaceholder".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.MoanEscapesLips".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.RapidlyHardeningShaft".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.MoisteningSlit".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.KeepsUpMinute".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.DickWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Urtascene.GetAPetReward.CuntLips".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.BiteLipWhine".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.GetAPetReward.OpensJarSmears".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.CrawlNervously".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.ExtendTongueLength".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.GetAPetReward.LickButterTip".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.ShiversFullSize".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.GetAPetReward.SmearsMoreButter".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.SmoochDownShaft".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.PrickGlistensMouth".Localized());

            if (silly)
                OutputText("Scenes.Urtascene.GetAPetReward.ButterForgotten".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.JawStretchedWide".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.OwnCockRockHard".Localized());

            if (Player.HasVagina())
            {
                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Urtascene.GetAPetReward.NotToMention".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Urtascene.GetAPetReward.MeanwhileFragment".Localized());

                OutputText("Scenes.Urtascene.GetAPetReward.PussyWettestEver".Localized());
            }

            OutputText("Scenes.Urtascene.GetAPetReward.SwirlTongueFlare".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Urtascene.GetAPetReward.SerpentineFragment".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Urtascene.GetAPetReward.DemonicFragment".Localized());
                else
                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                        OutputText("Scenes.Urtascene.GetAPetReward.DraconicFragment".Localized());
                    else
                        if (Player.HasLongTongue())
                            OutputText("Scenes.Urtascene.GetAPetReward.InhumanlyLongFragment".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.TongueAlongCumvein".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.FlareAgainstThroat".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.JetsWhiteHotCum".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.StretchesQuirksBrow".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.GulpLowerHead".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Urtascene.GetAPetReward.SerpentineFragmentAgain".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Urtascene.GetAPetReward.DemonicFragmentAgain".Localized());
                else
                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                        OutputText("Scenes.Urtascene.GetAPetReward.DraconicFragmentAgain".Localized());
                    else
                        if (Player.HasLongTongue())
                            OutputText("Scenes.Urtascene.GetAPetReward.InhumanlyLongAgain".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.LapsTopLayer".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.ShaftWord".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Urtascene.GetAPetReward.AndFragmentAgain".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.GetAPetReward.SlitWord".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.TantalizinglyFragment".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.EveryDollopGives".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.CockHeadPlaceholder".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.ClitPlaceholder".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.LittleTwirlToe".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.JerksBetweenToes".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.RubsLoveButton".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.CantHandleMore".Localized());

            Player.RefillHunger(75);

            OutputText("Scenes.Urtascene.GetAPetReward.CryBiteBedSide".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.GetAPetReward.CockBetweenLegs".Localized());
            else
                OutputText("Scenes.Urtascene.GetAPetReward.VaginaClampsNothing".Localized());

            OutputText("Scenes.Urtascene.GetAPetReward.OwnerContinuesAssault".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) + 2);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 2);

            OutputText("Scenes.Urtascene.GetAPetReward.EllipsisFragment".Localized());

            Player.Orgasm("Lips");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FeedTheBeastPartII);
        }

        public void FeedTheBeastPartII()
        {
            ClearOutput();

            View.Time.Hours += 2;

            OutputText("Scenes.Urtascene.FeedTheBeastPartII.SeveralHoursLaterBold".Localized());

            OutputText("Scenes.Urtascene.FeedTheBeastPartII.AwakeInArms".Localized());

            OutputText("Scenes.Urtascene.FeedTheBeastPartII.ConfirmEnjoyedPet".Localized());

            ReturnToCamp(1);
        }

        public void UrtaTakesPCOnWalkies()
        {
            ClearOutput();

            ShowImage("urta-home-petplay-walkies");

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.ClapsKnowsWhat".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.BlanchAtThought".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.CocksGrowHard".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.AndFragment".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.MoistensForDisplay".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.SentencePeriod".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.SmacksNewspaper".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.SwallowNodTail".Localized());

            if (Player.Body.Get<TailPart>().Id.Path != "none")
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.TailWord".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.TailplugWord".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.NervousShakeOpens".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.AllEyesTurn".Localized());

            if (Player.BiggestBreastSize() >= 5)
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.BreastsHanging".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.DisplayedRearEnd".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.PreCumDrizzles".Localized());
            else
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.JuicesFlowCunt".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.TrailOfHorniness".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.StopsToChat".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.HarderFragment".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.AndFragmentAgain".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.WetterFragment".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.FeelLikePet".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.CitizensComment".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.TimeToReturn".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.GaspsUnhooksLeash".Localized());

            OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.ConfirmGreatTime".Localized(
                Player.Mf("laugh", "giggle")
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimeSinceLastCame, 0);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaComfortableWithOwnBody, CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) + 5);

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Urtascene.UrtaTakesPCOnWalkies.TurnedOnExhibiting".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
            }

            ReturnToCamp(1);
        }

        public void VixenAndCreamPart1(bool teased = false)
        {
            ClearOutput();

            ShowImage("urta-vixen-and-cream-part1");

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.TwinkleMischief".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.SeizeGlossyLips".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.HandsAroundClaw".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.PullAwayHeatedLook".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.SwoonsMoreLikeIt".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.DevilishGleamLater".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.ParagraphBreak".Localized());

            if (teased)
                OutputText("Scenes.Urtascene.VixenAndCreamPart1.WantFewDrinks".Localized());
            else
                OutputText("Scenes.Urtascene.VixenAndCreamPart1.EnjoyDrinksChat".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.LoveOfYourLife".Localized());

            if (SceneInstance<UrtaPregsScene>.Get().UrtaKids() > 0)
            {
                OutputText("Scenes.Urtascene.VixenAndCreamPart1.AndTheFragment".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) > 0)
                    OutputText("Scenes.Urtascene.VixenAndCreamPart1.Mother".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) > 0 && CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesPcBirthed) > 0)
                    OutputText("Scenes.Urtascene.VixenAndCreamPart1.AndFragment".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesPcBirthed) > 0)
                    OutputText("Scenes.Urtascene.VixenAndCreamPart1.Father".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart1.OfYourChildren".Localized());
            }

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.AnnounceOutLoud".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.TongueLaterDeliver".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.ThoughtsOfTaste".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.PlaceDrinksRelay".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.TonightYoureMine".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.HeatsUpStaring".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.AnyoneWatchingLovers".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.TipsyFreshBeer".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.SweaterPuppiesPress".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.LoveSmellYou".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.HeartFilledLove".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.StrainingScarletFabric".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.BegsTouchThere".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.FingersContactEquine".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.PurrsFeelsGood".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart1.PoolingPreCum".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 400f));

            VixenAndCreamPart2();
        }

        public void VixenAndCreamPart2()
        {
            ClearOutput();

            ShowImage("urta-vixen-and-cream-part2");

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.StifleCriesBites".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.UppingAnteFondle".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.LoveNutsGrip".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.ReleaseSpheresFlipSkirt".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.LetThemWhisper".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.GuardDownUnderBalls".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.SlightestPressDigits".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.JerkAndPlumb".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.CoupDeGraceThumb".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.MissionAccomplishedPull".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.WhyStopBreathing".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.LapsSteamySeed".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.FlopsBackDelirious".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.LiesBlissedDaze".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.MeanLeaveMe".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.HotDamnTwins".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.WickedGrinAdvantage".Localized(
                Player.ClothedOrNakedLower("strips you of your lower garments and")
            ));

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.PerfectDownView".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.LustyLookLongLick".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.VoluminousPressurePole".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtascene.VixenAndCreamPart2.KissingDownShaft".Localized());
            else
                OutputText("Scenes.Urtascene.VixenAndCreamPart2.PointedlySqueezingShaft".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Urtascene.VixenAndCreamPart2.BallsNeedAttention".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart2.KissSweetTribute".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart2.GreenEyesStareUp".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart2.SomeoneTrulyLoves".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart2.DeliciousRevenge".Localized());
            }

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.StaresThroughLashes".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.NoLongerCaring".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart2.HandsStrokeBase".Localized());

            VixenAndCreamPart3();
        }

        public void VixenAndCreamPart3()
        {
            ClearOutput();

            ShowImage("urta-vixen-and-cream-part3");

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.GroanHeadRestBack".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Urtascene.VixenAndCreamPart3.FingersCradleBalls".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.SlaveToHands".Localized());
            }

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.FirstOfferingsCream".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.AtFirstDoubt".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.StretchesStrandBreaks".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.ContinuesSuckNeedy".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.BobbingGlansThroat".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.GagsStubbornTries".Localized(
                Player.CockHead()
            ));

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.TrembleThroatSlide".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.FingerBetweenBalls".Localized());

            if (Player.CumQ() < 250)
            {
                OutputText("Scenes.Urtascene.VixenAndCreamPart3.GeyserInGullet".Localized(
                    Player.CockHead()
                ));

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.UnwillingToWaste".Localized(
                    Player.CockDescript()
                ));

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.PullsBackMilks".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.StillStiffAfter".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.HipsRiseLastSpurt".Localized());
            }
            else
            {
                OutputText("Scenes.Urtascene.VixenAndCreamPart3.EruptingVolcanoGeysers".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.ReignInBeast".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.EyesRollBack".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.UnloadMilkedJism".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.ConsumedHeavenlyState".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.PullFromMouth".Localized());

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.WakeOfOrgasm".Localized(
                    Appearance.CockNoun(Player.Cocks[0].Id)
                ));

                OutputText("Scenes.Urtascene.VixenAndCreamPart3.StillStiffSlimy".Localized());
            }

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.ParagraphBreak".Localized());

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Urtascene.VixenAndCreamPart3.PostOrgasmicParalysis".Localized());
            else
                OutputText("Scenes.Urtascene.VixenAndCreamPart3.SeizingChance".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.SlidesIntoLap".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.AdoresYourSpunk".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.RunHandThrough".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.CaressEarsAdmission".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart3.SharesDelightfulPresent".Localized());

            Player.Orgasm("Generic");

            VixenAndCreamPart4();
        }

        public void VixenAndCreamPart4()
        {
            ClearOutput();

            ShowImage("urta-vixen-and-cream-part4");

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.WorldStopsTaste".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.StickyStrandLips".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.ReturnTheFavor".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.ClimbsOntoTable".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.WeightyBallsLength".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.NectarDownBalls".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.KissOrbSuckle".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.TimeToReturn".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.WhimperPushesSpheres".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.TastyFlowerInterest".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.WhimpersArchesHips".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.DizzyingScentHunger".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.SingleLickTorso".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.TeaseLittleStallion".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.GripBaseEquine".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.AssaultBothSexes".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.HandSlideUpDown".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.SpasticStormyFit".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.CumGushesPipe".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.SquirtsOnFace".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.PitcherUnderFlare".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.ContinuesShootCream".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.DelightedMoanLength".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.CuntSensitiveFace".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.SweptAwayTaste".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.SecondRoundGirlcum".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.SitPullIntoLap".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.DeepEyesSpacey".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPart4.YouDidTwice".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 150f));

            VixenAndCreamPartFinal();
        }

        public void VixenAndCreamPartFinal()
        {
            ClearOutput();

            ShowImage("urta-vixen-and-cream-part5");

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.JustAmazingBashful".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.KissPussyBallsRub".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.CocksSpringAgainst".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.MeaningfulLookLos".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.KissOverwhelmingNeed".Localized(
                Player.MultiCockDescript()
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFertile) > 0)
                OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.WordsBetweenKisses".Localized(
                    Player.CockHead(),

                    Player.CockDescript()
                ));

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.GrabCurvyThighs".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.LowerOnCockThrust".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.FuckMeWhimpers".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.ThighsSlapAss".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.GlazedEarsFlattened".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.MateMeFillWomb".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.MoanCockMilked".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.BlackLipsFlaring".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.ThrustUpMeet".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.WhinesDroolChin".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.ClenchesInsaneStiffness".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.SpasmicClenchOverEdge".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.ShudderMutualRelease".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.ParagraphBreak".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.KissPullOutFlood".Localized());
            else
                OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.KissPullOutAgain".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.GooeySpunkOverflows".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.WatchingLappingTooMuch".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.LashGlansTongue".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.MoansIntoMouthTremble".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.LostForWords".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.NuzzlesLovesYou".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.WalkOutDisheveled".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.ShyAtNoise".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.PublicAffectionNoise".Localized());

            OutputText("Scenes.Urtascene.VixenAndCreamPartFinal.DelightfulEternity".Localized());

            Player.ChangeFatigue(40);

            Player.Orgasm("Generic");

            Player.SlimeFeed();

            if (Player.LibidoPercent < 60)
                Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            for (var i = 0; i < 3; i++)
                KnockUpUrtaChance();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaVixenAndCreamCounter, CoC.Instance.Flags.Get(GameFlagIds.UrtaVixenAndCreamCounter) + 1);

            UrtaLove(3);

            ReturnToCamp(2);
        }

    }
}