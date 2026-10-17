using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<UrtaPregsScene>("coc:urta_pregs")]
    public class UrtaPregsScene : Scene
    {

        public bool pregnancy => SceneInstance<UrtaScene>.Get().Pregnancy.IsPregnant;

        public override void Entry()
        {
            UrtaKids();
        }

        public int UrtaKids()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) + CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) + CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms);
        }

        public void UrtaSexMenu()
        {

            Action spank = null;

            if (Player.HasCockThatFits(SceneInstance<UrtaScene>.Get().UrtaCapacity()) || Player.HasKeyItem("Deluxe Dildo"))
                spank = SceneInstance<UrtaScene>.Get().SpankTheShitOutOfUrtaAndMakeHerCreamHerselfFromProstateStimulationAloneLikeTheHornyDrunkenSlutSheReallyIs;

            if (!SceneInstance<UrtaScene>.Get().UrtaDrunk())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaPcLoveCounter) == 1)
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.HerPlace".Localized(), SceneInstance<UrtaScene>.Get().GoBackToUrtasForLuvinz);

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaCumNoCumDays) >= 5)
                        AddButton(1, "Common.Button.SuckOff".Localized(), SceneInstance<UrtaScene>.Get().SlurpFawkesCocksForFunAndInflation);
                    else
                        AddButton(1, "Common.Button.SuckOff".Localized(), SceneInstance<UrtaScene>.Get().BlowUrtaUnderTheTableLuv);

                    AddButton(2, "Common.Button.EatOut".Localized(), SceneInstance<UrtaScene>.Get().EatUrtaOutNomNomPussy);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                }
                else
                {
                    ClearButtons();

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaCumNoCumDays) >= 5)
                        AddButton(0, "Common.Button.HiddenBj".Localized(), SceneInstance<UrtaScene>.Get().SlurpFawkesCocksForFunAndInflation);
                    else
                        AddButton(0, "Common.Button.HiddenBj".Localized(), SceneInstance<UrtaScene>.Get().BlowUrtaUnderTable);

                    AddButton(1, "Common.Button.UrtasPlace".Localized(), SceneInstance<UrtaScene>.Get().GoBackToUrtasForLuvinz);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                }
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Jerkoff".Localized(), SceneInstance<UrtaScene>.Get().GetAPublicFacialFromUrta);

                AddButton(1, "Common.Button.AnalRide".Localized(), SceneInstance<UrtaScene>.Get().TakeUrtaInTheButtPublically);

                if (Player.CanOviposit())
                    AddButton(2, "Common.Button.LayEggs".Localized(), SceneInstance<UrtaScene>.Get().GiveTheFoxSomeEggs);

                AddButton(3, "Common.Button.SpankHer".Localized(), spank);

                if (SceneInstance<RaphaelScene>.Get().FinaleCovered() && SceneInstance<RaphaelScene>.Get().RaphaelLikes() && CoC.Instance.Flags.Get(GameFlagIds.UrtaXRaphaelHappened) == 0)
                    AddButton(8, "Common.Button.Somesurprise".Localized(), SceneInstance<UrtaScene>.Get().UrtaAndRaphaelSurprise);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }
        }

        public void UrtaPregAppearance()
        {

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1 || __sw0_0 == 2)
                OutputText("Scenes.Urtapregs.UrtaPregAppearance.SittingAtUsualTable".Localized());

            if (__sw0_0 == 3 || __sw0_0 == 4 || __sw0_0 == 5 || __sw0_0 == 6 || __sw0_0 == 7)
                OutputText("Scenes.Urtapregs.UrtaPregAppearance.MovedToPrivateStall".Localized());

            if (__sw0_0 == 8 || __sw0_0 == 9)
                OutputText("Scenes.Urtapregs.UrtaPregAppearance.OutgrownStallCorner".Localized());
        }

        public void UrtaPreggoApproached()
        {
            ClearOutput();

            ClearButtons();

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1)
            {
                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.ChuggingOrangeDrink".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) == 0)
                {
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.CockClonksTable".Localized(
                        Player.Mf("-boy", "")
                    ));

                    UrtaSexMenu();
                }
                else
                {
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.NoAmorousClonking".Localized());

                    AddButton(0, "Common.Button.Talk".Localized(), SceneInstance<UrtaScene>.Get().UrtaDialogueMenu);

                    AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                }
            }

            if (__sw0_0 == 2 || __sw0_0 == 3)
            {
                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.SideSaddledDressBulge".Localized());

                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.HornyAllTheTime".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.BellyTentingSideSaddle".Localized());

                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.KnockedUpVixenSugar".Localized());
                }

                AddButton(0, "Common.Button.Sex".Localized(), UrtaSexMenu);

                AddButton(1, "Common.Button.Talk".Localized(), SceneInstance<UrtaScene>.Get().UrtaDialogueMenu);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }

            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.PrivateBoothCushions".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) == 0)
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.PrecumDarkenedDress".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.NotInFuckMood".Localized());

                AddButton(0, "Common.Button.Sex".Localized(), PreggoUrtaSmexOrSomething);

                AddButton(1, "Common.Button.Talk".Localized(), SceneInstance<UrtaScene>.Get().UrtaDialogueMenu);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }

            if (__sw0_0 == 5)
            {
                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.StillInPrivateBooth".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) == 0)
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.SteppingInPrecum".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.GlassDownLipLick".Localized());

                AddButton(0, "Common.Button.Sex".Localized(), PreggoUrtaSmexOrSomething);

                AddButton(1, "Common.Button.Talk".Localized(), SceneInstance<UrtaScene>.Get().UrtaDialogueMenu);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }

            if (__sw0_0 == 6)
            {
                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.DressGivenUpGhost".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) == 0)
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.PrecumPuddleDripping".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaPreggoApproached.MilkGlassesEmbarrassed".Localized());

                AddButton(0, "Common.Button.Sex".Localized(), PreggoUrtaSmexOrSomething);

                AddButton(1, "Common.Button.Talk".Localized(), SceneInstance<UrtaScene>.Get().UrtaDialogueMenu);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }

            if (__sw0_0 == 7)
            {
                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.ProudBellyGrowth".Localized());

                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.PreDripsNoticeYou".Localized());

                AddButton(0, "Common.Button.Sex".Localized(), PreggoUrtaSmexOrSomething);

                AddButton(1, "Common.Button.Talk".Localized(), SceneInstance<UrtaScene>.Get().UrtaDialogueMenu);

                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
            }

            if (__sw0_0 == 8)
            {
                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.WatermelonBellyBooth".Localized());

                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.BabyDueAnyDay".Localized());

                OutputText("Scenes.Urtapregs.UrtaPreggoApproached.ApartmentReleaseChoice".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Sex".Localized(), PreggoUrtaSmexOrSomething);

                AddButton(1, "Common.Button.Tease".Localized(), UrtaRaepsJoo);

                AddButton(14, "Common.Button.Leave".Localized(), LeavePreggoUrta);
            }

            if (__sw0_0 == 9)
                GoVisitUrtaBirfs(true);
        }

        public void LeavePreggoUrta()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.LeavePreggoUrta.ApologyAndStepAway".Localized());

            ReturnToCamp(1);
        }

        public void PreggoUrtaGivingBirth()
        {
            ClearOutput();

            Player.ClearPregnancy();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPregnantDeliveryScene, 0);

            CoC.Instance.Flags.Set(GameFlagIds.NewBabyAnnounced, 0);

            OutputText("Scenes.Urtapregs.PreggoUrtaGivingBirth.ApproachingCommotion".Localized());

            OutputText("Scenes.Urtapregs.PreggoUrtaGivingBirth.TelAdreMessenger".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Go".Localized(), () => GoVisitUrtaBirfs());

            AddButton(1, "Common.Button.Refuse".Localized(), IAintGotTimeForUrtaBirfs);
        }

        public void IAintGotTimeForUrtaBirfs()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.IAintGotTimeForUrtaBirfs.CentaurPerplexed".Localized());

            OutputText("Scenes.Urtapregs.IAintGotTimeForUrtaBirfs.ExplainCannotGo".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimesBirthed, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornGender, Random.Shared.Next(3) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsMales, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) + 1);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 2)
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsFemales, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) + 1);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsHerms, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 3 || CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 1)
                if (Random.Shared.Next(2) == 0)
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 2);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 1);

            if (UrtaKids() == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.UrtaFirstbornGender, CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender));

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFirstbornCocktype, CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornCocktype));
            }

            CoC.Instance.Flags.Set(GameFlagIds.NeedUrtaLetter, 1);

            ReturnToCamp(1);
        }

        public void GetUrtaLetter()
        {

            OutputText("Scenes.Urtapregs.GetUrtaLetter.LetterInTheNight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 1)
                OutputText("Scenes.Urtapregs.GetUrtaLetter.SonWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 2)
                    OutputText("Scenes.Urtapregs.GetUrtaLetter.DaughterWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.GetUrtaLetter.HermaphroditeDaughter".Localized());

            OutputText("Scenes.Urtapregs.GetUrtaLetter.ThoughWord".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.NeedUrtaLetter, 0);
        }

        public void GoVisitUrtaBirfs(bool withUrta = false)
        {
            ClearOutput();

            Player.ClearPregnancy();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPregnantDeliveryScene, 0);

            CoC.Instance.Flags.Set(GameFlagIds.NewBabyAnnounced, 0);

            if (!withUrta)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.ClinicTripBlur".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) == 0)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.PerkyMouseNurse".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SplinterGrinsGreeting".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.UrtaOnBedGowned".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SeesYourGaze".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SplinterBowsLeaves".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.MoansWithContraction".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SoftKnockingAtDoor".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) == 0)
            {
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.StrangeNameDonatello".Localized());

                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.UrtaReassuresYou".Localized());
            }
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.BeenBusyIndeed".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.DonChairBedFoot".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.ContractionsEveryTen".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.StethoscopeAway".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.BlushingKissPull".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.TurtleDoctorPosition".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.InhaleExhalePush".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SeeTheHeadPush".Localized());

            var gender = Random.Shared.Next(3) + 1;

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.UmbilicalCordWrap".Localized());

            if (gender == (int)Gender.Female)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.GirlWord".Localized());
            else
                if (gender == (int)Gender.Male)
                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.BoyWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HealthyHermBaby".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SayEnd".Localized());

            if (gender == (int)Gender.Male || gender == (int)Gender.Hermaphrodite)
                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SayStartWellNow".Localized());

                    if (gender == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SLetter".Localized());

                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.NormalFoxSayEnd".Localized());

                    if (gender == (int)Gender.Male)
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SonWord".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.DaughterWord".Localized());

                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.GroinViewFromHere".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 2);
                }
                else
                {
                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SayStartLooksLike".Localized());

                    if (gender == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SLetterAgain".Localized());

                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HeTakesAfter".Localized());

                    if (gender == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HerIt".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HisIt".Localized());

                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.EquineSheathSayEnd".Localized());

                    if (gender == (int)Gender.Male)
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HeIt".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SheIt".Localized());

                    OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.QuiteABigOne".Localized());

                    if (gender == (int)Gender.Male)
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.BoyGrowsUp".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.GirlGrowsUp".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 1);
                }

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HandsYouTheBaby".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HerItAgain".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HimIt".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.LatchingToBreast".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.DaughterWordAgain".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SonWordAgain".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.LookWord".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SheIsContraction".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HeIsContraction".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.BeautifulBabySayEnd".Localized());

            if (UrtaKids() == 0)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.OverwhelmedNewMother".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.DelightedNewAddition".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.SentencePeriod".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.EmbraceKissUrta".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HerItThird".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.HisItAgain".Localized());

            OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.DoctorWorkHereDone".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) == 0)
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.QuestioningLookNow".Localized());
            else
                OutputText("Scenes.Urtapregs.GoVisitUrtaBirfs.KnowingCoyGrin".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimesBirthed, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornGender, gender);

            if (gender == (int)Gender.Male)
                CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsMales, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) + 1);
            else
                if (gender == (int)Gender.Female)
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsFemales, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) + 1);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsHerms, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) + 1);

            if (UrtaKids() == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.UrtaFirstbornGender, CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender));

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFirstbornCocktype, CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornCocktype));
            }

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), StayAfterUrtaBirf);

            AddButton(1, "Common.Button.Go".Localized(), GoAfterUrtaBirf);
        }

        public void GoAfterUrtaBirf()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GoAfterUrtaBirf.ApologizeMakeItUp".Localized());

            View.Time.Hours += 1;

            Go<CampScene>();
        }

        public void StayAfterUrtaBirf()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.StayAfterUrtaBirf.HappyToOblige".Localized());

            OutputText("Scenes.Urtapregs.StayAfterUrtaBirf.EventfulDayEntail".Localized());

            OutputText("Scenes.Urtapregs.StayAfterUrtaBirf.MealAndBabyShopping".Localized());

            OutputText("Scenes.Urtapregs.StayAfterUrtaBirf.BlinkAtSuggestion".Localized());

            OutputText("Scenes.Urtapregs.StayAfterUrtaBirf.WickedMotherGrin".Localized());

            OutputText("Scenes.Urtapregs.StayAfterUrtaBirf.OfCourseNot".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WakeUpWithUrtaAfterStaying);
        }

        public void WakeUpWithUrtaAfterStaying()
        {
            ClearOutput();

            View.Time.Days += 1;

            View.Time.Hours = 6;

            Player.Orgasm("Generic");

            CampScene.SleepRecovery(false);

            Player.HealthChange(Player.MaxHealth);
// 原版 Achievements.hx:220「Getaway」：在营地外过夜
            AwardAchievement(AchievementIds.Getaway);

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.MorningSunShinesAwake".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) == 1)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.SonWord".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.DaughterWord".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.WentHomeMakingOut".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.HearSoftSnore".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.PryArmsFromYou".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.ClingingTightly".Localized());
                else
                    OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.MischiefReachShaft".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.HoldShaft".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.GentlyWord".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.TightlyWord".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.EarningPrefix".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.WhimperingMoan".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.GaspingShudder".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.BeginStrokingVixen".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.BidGoodMorning".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesBirthed) == 1)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.QuestionHerDrinking".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.InsertFingerBehind".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.VixenSquealsWriggles".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.MirthfullyFetchBottle".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.ChildSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.ChildrenPlural".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.BrokenGlassWorry".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.StretchAndPeck".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.MakeShowForUrta".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.SmileAndDress".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.UrtaLicksLips".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.ShrillCryRings".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.NewWord".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.NewestWord".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.LittleMonsterNote".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.HimPronoun".Localized());
            else
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.HerPronoun".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.SentencePeriod".Localized());

            if (UrtaKids() == 2)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.OtherKidAtEdryn".Localized());

            if (UrtaKids() > 2)
                OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.OtherKidsAtEdryn".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.FollowToNursery".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.LoopArmAround".Localized());

            OutputText("Scenes.Urtapregs.WakeUpWithUrtaAfterStaying.SwollenBreastsMilk".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), DrinkSomeUrtaPostBirthTitMilk);

            AddButton(14, "Common.Button.Leave".Localized(), NoNeedForTitMilkTodayUrta);
        }

        public void NoNeedForTitMilkTodayUrta()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NoNeedForTitMilkTodayUrta.DeclineThenLeave".Localized());

            ReturnToCamp(1);
        }

        public void DrinkSomeUrtaPostBirthTitMilk()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.AskForBreakfast".Localized());

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.BurpedKitLater".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 1)
                OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.HisPronoun".Localized());
            else
                OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.HerPronoun".Localized());

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.SashayAndCroon".Localized());

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.TakeNippleSuckle".Localized());

            Player.RefillHunger(20);

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.FeelShaftTip".Localized());

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.LaughPatCheek".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.ChildSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.ChildrenPlural".Localized());

            OutputText("Scenes.Urtapregs.DrinkSomeUrtaPostBirthTitMilk.PoorSubstituteWorry".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Lust, 10f));

            ReturnToCamp(1);
        }

        public bool UpdateVaginalPregnancy()
        {

            if (Player.Pregnancy.Incubation == 504)
            {
                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.NauseatedEarlySign".Localized());

                return true;
            }
            else
                if (Player.Pregnancy.Incubation == 432)
                {
                    OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.BellySmallBump".Localized());

                    OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.PastGatesSpotUrta".Localized());

                    OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.StrikePoseBelly".Localized());

                    OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.AskSeeingHerMore".Localized());

                    return true;
                }
                else
                    if (Player.Pregnancy.Incubation == 360)
                    {
                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.UrtaCallsForYou".Localized());

                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.GreetAtCampEdge".Localized());

                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.ThankForSupplies".Localized());

                        return true;
                    }
                    else
                        if (Player.Pregnancy.Incubation == 288)
                        {
                            OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.SitNearCabin".Localized());

                            OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.UrtaFriendlyWave".Localized());

                            OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.EagerBellyPatting".Localized());

                            if (Player.Pregnancy.Type != PregnancyIds.PlayerFather)
                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.JealousCarryingYou".Localized());
                            else
                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.SmugTummyCaress".Localized());

                            OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.ShakeHeadConsidering".Localized());

                            OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.UrtaLaughsRubbing".Localized());

                            ClearButtons();

                            AddButton(0, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), GetAnUrtaMassageWhenPreggo);

                            CoC.Instance.Flags.Set(GameFlagIds.EventParserEscape, 1);

                            return true;
                        }
                        else
                            if (Player.Pregnancy.Incubation == 216)
                            {
                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.FetchWaterskin".Localized());

                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.BabyKickingMention".Localized());

                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.SquealAndGlomp".Localized());

                                if (UrtaKids() == 0)
                                    OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.GoingToBeParents".Localized());

                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.BabblesTailWagging".Localized());

                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.PatHeadAndSmile".Localized());

                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.TightHugJoke".Localized());

                                OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.RubBellyOfferHand".Localized());

                                return true;
                            }
                            else
                                if (Player.Pregnancy.Incubation == 144)
                                {
                                    OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.SighHopeSoon".Localized());

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Accept".Localized(), AcceptUrtaLevelSixPreggoHelps);

                                    AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), DeclineUrtaLevelSixPreggoHelps);

                                    CoC.Instance.Flags.Set(GameFlagIds.EventParserEscape, 1);

                                    return true;
                                }
                                else
                                    if (Player.Pregnancy.Incubation == 72)
                                    {
                                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.LayDownForNap".Localized(
                                            CampSleep.HomeDesc()
                                        ));

                                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.SuddenBlurtConcern".Localized());

                                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.AdmitPrettyBig".Localized());

                                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.NodButDecline".Localized());

                                        OutputText("Scenes.Urtapregs.UpdateVaginalPregnancy.TearfulOutburst".Localized());

                                        return true;
                                    }

            return false;
        }

        public void NoHelpForUrtaPreggoPC()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NoHelpForUrtaPreggoPC.CaressCheekRefuse".Localized());

            ReturnToCamp(1);
        }

        public void GetUrtaHelpPreggoPC()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GetUrtaHelpPreggoPC.HopeMoreTime".Localized());

            UrtaSexMenu();
        }

        public void GetUrtaSexWhenPreggoz()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GetUrtaSexWhenPreggoz.NeedABreak".Localized());

            OutputText("Scenes.Urtapregs.GetUrtaSexWhenPreggoz.CroonsSkirtLifting".Localized());

            OutputText("Scenes.Urtapregs.GetUrtaSexWhenPreggoz.FollowThinkingWhat".Localized());

            UrtaSexMenu();
        }

        public void GetAnUrtaMassageWhenPreggo()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.BackrubNotLieDown".Localized());

            OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.SuggestBedroll".Localized());

            OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.NoExperienceApology".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.LetItSlide".Localized());
            else
                OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.DontWorryPractice".Localized());

            OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.GrumblingAboutMove".Localized());

            OutputText("Scenes.Urtapregs.GetAnUrtaMassageWhenPreggo.RetortMelonCramps".Localized());

            ReturnToCamp(1);
        }

        public void DeclineUrtaLevelSixPreggoHelps()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.DeclineUrtaLevelSixPreggoHelps.ThankButDecline".Localized());

            OutputText("Scenes.Urtapregs.DeclineUrtaLevelSixPreggoHelps.SighAndNod".Localized());

            OutputText("Scenes.Urtapregs.DeclineUrtaLevelSixPreggoHelps.ShakeHeadReassure".Localized());

            ReturnToCamp(1);
        }

        public void AcceptUrtaLevelSixPreggoHelps()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.AcceptUrtaLevelSixPreggoHelps.TiredButAccept".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaLevelSixPreggoHelps.HappyEmbrace".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaLevelSixPreggoHelps.NoNeedToWorry".Localized());

            CampScene.SleepRecovery(false);

            View.Time.Days += 1;

            View.Time.Hours = 6;

            ReturnToCamp(1);
        }

        public void VaginalBirth()
        {

            OutputText("Scenes.Urtapregs.VaginalBirth.ParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPregnantDeliveryScene, 0);

            CoC.Instance.Flags.Set(GameFlagIds.NewBabyAnnounced, 0);

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Urtapregs.VaginalBirth.GroinPressurePain".Localized());

                Player.CreateVagina();
            }

            OutputText("Scenes.Urtapregs.VaginalBirth.WaterBreaksRush".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesPcBirthed) == 0)
                OutputText("Scenes.Urtapregs.VaginalBirth.UrtaBurstsFromBushes".Localized());
            else
                OutputText("Scenes.Urtapregs.VaginalBirth.BadTimingTease".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.ReassuringSmileArrival".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.RecliningOnBedroll".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.DeepBreathFinalPush".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.BirthOverRelief".Localized());

            var gender = Random.Shared.Next(3) + 1;

            if (gender == (int)Gender.Male)
            {
                OutputText("Scenes.Urtapregs.VaginalBirth.HealthyBoyNews".Localized());

                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Urtapregs.VaginalBirth.VulpineWord".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 2);
                }
                else
                {
                    OutputText("Scenes.Urtapregs.VaginalBirth.EquineWord".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 1);
                }

                OutputText("Scenes.Urtapregs.VaginalBirth.BoySheathGrowth".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsMales, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornGender, 1);
            }
            else
                if (gender == (int)Gender.Female)
                {
                    OutputText("Scenes.Urtapregs.VaginalBirth.HealthyGirlNews".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsFemales, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) + 1);

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornGender, 2);
                }
                else
                {
                    OutputText("Scenes.Urtapregs.VaginalBirth.NervousSilenceSorrow".Localized());

                    if (Random.Shared.Next(2) == 0)
                    {
                        OutputText("Scenes.Urtapregs.VaginalBirth.VulpineWordAgain".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 2);
                    }
                    else
                    {
                        OutputText("Scenes.Urtapregs.VaginalBirth.EquineWordAgain".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornCocktype, 1);
                    }

                    OutputText("Scenes.Urtapregs.VaginalBirth.GirlSheathGrowth".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaKidsHerms, CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) + 1);

                    CoC.Instance.Flags.Set(GameFlagIds.UrtaLatestbornGender, 3);
                }

            OutputText("Scenes.Urtapregs.VaginalBirth.TiredSmileFatherChild".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.VaginalBirth.DaughterIt".Localized());
            else
                OutputText("Scenes.Urtapregs.VaginalBirth.SonIt".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.WildernessSafeClaim".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.VaginalBirth.DaughterItAgain".Localized());
            else
                OutputText("Scenes.Urtapregs.VaginalBirth.SonItAgain".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.CantCareWilderness".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.VaginalBirth.DaughterItThird".Localized());
            else
                OutputText("Scenes.Urtapregs.VaginalBirth.SonItThird".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.WildernessCareQuestion".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.ReluctantAdmission".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.VaginalBirth.TelAdreGirlIt".Localized());
            else
                OutputText("Scenes.Urtapregs.VaginalBirth.TelAdreBoyIt".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.TelAdreUpbringing".Localized());

            if (gender >= 2)
                OutputText("Scenes.Urtapregs.VaginalBirth.SafeRearingGirlIt".Localized());
            else
                OutputText("Scenes.Urtapregs.VaginalBirth.SafeRearingBoyIt".Localized());

            OutputText("Scenes.Urtapregs.VaginalBirth.SafeRearingPromise".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaTimesPcBirthed, CoC.Instance.Flags.Get(GameFlagIds.UrtaTimesPcBirthed) + 1);

            if (UrtaKids() == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.UrtaFirstbornGender, CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender));

                CoC.Instance.Flags.Set(GameFlagIds.UrtaFirstbornCocktype, CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornCocktype));
            }
        }

        public void UrtaIsHappyAboutPregnancyAtTheBar()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.UrtaIncubationCelebration, 1);

            OutputText("Scenes.Urtapregs.UrtaIsHappyAboutPregnancyAtTheBar.FestiveBarMood".Localized());

            OutputText("Scenes.Urtapregs.UrtaIsHappyAboutPregnancyAtTheBar.NancyThrewParty".Localized());

            OutputText("Scenes.Urtapregs.UrtaIsHappyAboutPregnancyAtTheBar.CelebrationThenLeave".Localized());

            ReturnToCamp(1);
        }

        public void UrtaIsAPregnantCopScene()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.UrtaIsAPregnantCopScene.SquareCommotion".Localized());
            // 原版 UrtaPregs.hx:872：Edryn 怀孕时追加一句
            if (SceneInstance<EdrynScene>.Get().Pregnancy.IsPregnant)
                OutputText("Scenes.Urtapregs.UrtaIsAPregnantCopScene.EdrynBellyHandicap".Localized());

            OutputText("Scenes.Urtapregs.UrtaIsAPregnantCopScene.ThiefPathDecision".Localized());

            OutputText("Scenes.Urtapregs.UrtaIsAPregnantCopScene.PregnantPigJoke".Localized());

            OutputText("Scenes.Urtapregs.UrtaIsAPregnantCopScene.EdrynHelpsUp".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void UrtaSpecialDeliveries()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.GateBasketGreeting".Localized());
            // 原版 UrtaPregs.hx:872：Edryn 怀孕时追加一句
            if (SceneInstance<EdrynScene>.Get().Pregnancy.IsPregnant)
                OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.BellyJigglingMotion".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.FaintSmileFavor".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.AgreeToDeliver".Localized());
            // 原版 UrtaPregs.hx:872：Edryn 怀孕时追加一句
            if (SceneInstance<EdrynScene>.Get().Pregnancy.IsPregnant)
                OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.PregnantMarePace".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.HeadToUrtaHouse".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.FamiliarPathWalk".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.BasketPresentedInside".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.UpstairsDoorTurned".Localized());

            if (Player.Pregnancy.Event >= 7)
                OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.HeavilyWord".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.VixenSnoringUnderCovers".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.SneakInsideRoom".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.ExplainDeliveryVisit".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.BellyPatGiggle".Localized());

            OutputText("Scenes.Urtapregs.UrtaSpecialDeliveries.ArmStrokeGoodbye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.UrtaPregnantDeliveryScene, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void TalkToUrtaAboutHerHouse()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.OldApartmentWondering".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.EmotionalValueAside".Localized());

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.MonsterRunningAround".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.OrTwoWord".Localized());

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.ChildPipesUp".Localized());

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.UpkeepExpenseQuestion".Localized());

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.CaptainPaycheck".Localized());

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.NodAndThank".Localized());

            OutputText("Scenes.Urtapregs.TalkToUrtaAboutHerHouse.FootRubbingHint".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesUrtaHouseSex);

            AddButton(1, "Common.Button.No".Localized(), NoUrtaHouseSex);
        }

        public void YesUrtaHouseSex()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.YesUrtaHouseSex.SeductiveSmileBrow".Localized());

            OutputText("Scenes.Urtapregs.YesUrtaHouseSex.PromisingSmilePayment".Localized());

            OutputText("Scenes.Urtapregs.YesUrtaHouseSex.PayTabBackDoor".Localized());

            OutputText("Scenes.Urtapregs.YesUrtaHouseSex.MakeoutEveryTurn".Localized());

            if (Player.LibidoPercent > 50)
                OutputText("Scenes.Urtapregs.YesUrtaHouseSex.AlmostStripRightThere".Localized());

            OutputText("Scenes.Urtapregs.YesUrtaHouseSex.KeyFromWallet".Localized());

            if (!Quests.UrtaQuestScene.UrtaLove())
                SceneInstance<UrtaScene>.Get().GoBackToUrtasForLuvinzII();
            else
            {
                OutputText("Scenes.Urtapregs.YesUrtaHouseSex.ParagraphBreak".Localized());

                SceneInstance<UrtaScene>.Get().UrtaHomeLuvLuvinsMenu();
            }
        }

        public void NoUrtaHouseSex()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NoUrtaHouseSex.ApologeticNotInMood".Localized());

            OutputText("Scenes.Urtapregs.NoUrtaHouseSex.DisappointedFooey".Localized());

            OutputText("Scenes.Urtapregs.NoUrtaHouseSex.PromiseLaterHelp".Localized());

            ReturnToCamp(1);
        }

        public void TalkAboutLiana()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.TalkAboutLiana.SeriousTalkRequest".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiana.PuzzledVixenResponds".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedLadies) > 0 || CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedMales) > 0)
            {
                OutputText("Scenes.Urtapregs.TalkAboutLiana.LianaExperimentConfession".Localized());

                OutputText("Scenes.Urtapregs.TalkAboutLiana.ConcernsWavedAway".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.TalkAboutLiana.AlchemistUnderRoof".Localized());

                OutputText("Scenes.Urtapregs.TalkAboutLiana.PermissionGrantedSmile".Localized());
            }

            OutputText("Scenes.Urtapregs.TalkAboutLiana.BabysitterSearchQuestion".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiana.SubordinatesScoutedAds".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiana.TrustQuestionDoubt".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TalkAboutLiana.ChildrenHaveSuffix".Localized());
            else
                OutputText("Scenes.Urtapregs.TalkAboutLiana.HasVerbWord".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiana.SpecialPlaceInHeart".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiana.InsecuritiesButTrust".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiana.WinkAndKiss".Localized());

            ReturnToCamp(1);
        }

        public void PreggoUrtaSmexOrSomething()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.OfferVentSteam".Localized());

            OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.LecherousGrinsPrecum".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 7)
                OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.DrippingOntoFloor".Localized());

            if (__sw0_0 == 8 || __sw0_0 == 9)
                OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.FillingBucketSplash".Localized());

            if (true )
                OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.DressStain".Localized());

            OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.PullsHerselfUpright".Localized());

            OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.LeadThroughStreets".Localized());

            OutputText("Scenes.Urtapregs.PreggoUrtaSmexOrSomething.QuickStripCouch".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.MassageHer".Localized(), MassagePregnantUrtasBelly);

            AddButton(1, "Common.Button.BoobWorship".Localized(), UrtaBoobWorshipScene);

            AddButton(2, "Common.Button.Nurse".Localized(), NurseFromMommaUrtasBooBees);

            AddButton(3, "Common.Button.MilkHer".Localized(), MilkPregnantUrta);

            if (Player.HasCock())
                AddButton(4, "Common.Button.Cravings".Localized(), UrtaPregCravings);
            else
                AddButton(4, "Common.Button.Cravings".Localized(), null);
        }

        public void MassagePregnantUrtasBelly()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.OffersBellyRub".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMassagedUrtaBelly) == 0)
            {
                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.DeclinesPolitely".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.SnatchesLubeAway".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.AsksAboutProblem".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.PointsOutCockSize".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.SearchesKitchen".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.ExplainsSweetCream".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.TestsCreamLid".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.BlushesAtIdea".Localized());

                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.HasHerLieDown".Localized());
            }
            else
                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.MentionsWhippedCream".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.JokesAboutCondom".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.AimsLidAtNavel".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMassagedUrtaBelly) == 0)
                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.MusesSweetTooth".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.RubsInCream".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.CreamsNipples".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.FakeApology".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.ForkedTongue".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.DemonicTongue".Localized());
                else
                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                        OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.DraconicTongue".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.LicksCreamOffBreasts".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.TongueStretchesFar".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.FocusesCreamyNubs".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.WigglesInPleasure".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.DemandsBellyRub".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.KneadsBellyGently".Localized());

            OutputText("Scenes.Urtapregs.MassagePregnantUrtasBelly.PuzzlesOverWords".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesMassagedUrtaBelly, CoC.Instance.Flags.Get(GameFlagIds.TimesMassagedUrtaBelly) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.RubDick".Localized(), RubUrtaDick);

            AddButton(1, "Common.Button.RubPussy".Localized(), RubUrtaPussy);
        }

        public void RubUrtaDick()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.RubUrtaDick.AssumesShaft".Localized());

            OutputText("Scenes.Urtapregs.RubUrtaDick.StrokesSpongyShaft".Localized());

            OutputText("Scenes.Urtapregs.RubUrtaDick.GaspsAsCockThrobs".Localized());

            UrtaMassageClimax();
        }

        public void RubUrtaPussy()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.RubUrtaPussy.AssumesPussy".Localized());

            OutputText("Scenes.Urtapregs.RubUrtaPussy.FingersHerDepths".Localized());

            UrtaMassageClimax();
        }

        public void UrtaMassageClimax()
        {

            OutputText("Scenes.Urtapregs.UrtaMassageClimax.HowlsInPleasure".Localized());

            OutputText("Scenes.Urtapregs.UrtaMassageClimax.FetchesCreamCan".Localized());

            OutputText("Scenes.Urtapregs.UrtaMassageClimax.AsksAboutReplacing".Localized());

            OutputText("Scenes.Urtapregs.UrtaMassageClimax.QuestionsWhileYawning".Localized());

            OutputText("Scenes.Urtapregs.UrtaMassageClimax.TemptedToTease".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Urtapregs.UrtaMassageClimax.TiesOffCondom".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Urtapregs.UrtaMassageClimax.ShrugsAtSleepiness".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaMassageClimax.DumpsCumOnBelly".Localized());

            OutputText("Scenes.Urtapregs.UrtaMassageClimax.LocksDoorLeaving".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ReturnToCamp(1);
        }

        public void UrtaBoobWorshipScene()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.GazesAtFullBreasts".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.NotesSwollenBreasts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesUrtaBoobWorshipped) == 0)
            {
                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.AdmitsPoorMilking".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.OffersToHelp".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.TeasesFreeShot".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.LacksMagicTouch".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.AgreesToPlan".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesUrtaBoobWorshipped, CoC.Instance.Flags.Get(GameFlagIds.TimesUrtaBoobWorshipped) + 1);

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.WeighsHerOrbs".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.BegsKeepGoing".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.TwistsNipples".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.MassagesFirmly".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.FollowsWithoutStopping".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.ThrowsCondom".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.AttacksBreastsGusto".Localized());

            if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SharkTeeth || Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs || Player.Body.Get<FacePart>().Id == BodyPartTypes.SpiderFangs)
                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.MindingSharpTeeth".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.YelpsButStays".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.ShrieksAsCockBursts".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.ShowsErectCock".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.PullsHandAway".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.PressesBetweenBreasts".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.MoansWhileHumping".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.WarnsOfBlowing".Localized());

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.GaspsInEcstasy".Localized());

                if (Player.CumQ() < 250)
                {
                    OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.FillsHerMouth".Localized());

                    OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.PraisesHisTaste".Localized());
                }
                else
                    if (Player.CumQ() < 500)
                    {
                        OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.JetsCumDeep".Localized());

                        OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.LicksLipsLovingly".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.SwallowsHotLoad".Localized());

                        OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.CoughsAtOverload".Localized());
                    }

                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.StretchesSated".Localized());
            }

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.WorksOffCondom".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.KissesGoodbye".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.TastesHimself".Localized());

            OutputText("Scenes.Urtapregs.UrtaBoobWorshipScene.PromisesReturn".Localized());

            ReturnToCamp(1);
        }

        public void NurseFromMommaUrtasBooBees()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.LicksLipsAtMilk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesNursedFromUrta) == 0)
            {
                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.MocksGrownUp".Localized());

                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.PuppyEyes".Localized());

                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.GesturesComfort".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.CroonsThirsty".Localized());

                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.LaughsInvitation".Localized());
            }

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.ShufflesOver".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.SettlesBesideHer".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.ForkedTongue".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.DemonicTongue".Localized());
                else
                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                        OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.DraconicTongue".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.SighsInPleasure".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.CoilsRoundNipple".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.HintsSuckingNeeded".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.SucklesNipple".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.CradlesHisHead".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.WorksForMoreMilk".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.SwitchesToOtherBreast".Localized());

            OutputText("Scenes.Urtapregs.NurseFromMommaUrtasBooBees.LosesSelfInTask".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesNursedFromUrta, CoC.Instance.Flags.Get(GameFlagIds.TimesNursedFromUrta) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Stroke".Localized(), TitWorshipAndStroke);

            AddButton(1, "Common.Button.Ignore".Localized(), IgnoreUrtaBonerWhenWorship);
        }

        public void TitWorshipAndStroke()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.TitWorshipAndStroke.GrabsShaftGently".Localized());

            OutputText("Scenes.Urtapregs.TitWorshipAndStroke.StrokesAndTeasesSheath".Localized());

            OutputText("Scenes.Urtapregs.TitWorshipAndStroke.SwishesTailAmused".Localized());

            OutputText("Scenes.Urtapregs.TitWorshipAndStroke.RedoublesMilking".Localized());

            FinishTitWorshipWivUrta();
        }

        public void IgnoreUrtaBonerWhenWorship()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.IgnoreUrtaBonerWhenWorship.GladSheEnjoys".Localized());

            FinishTitWorshipWivUrta();
        }

        public void FinishTitWorshipWivUrta()
        {

            OutputText("Scenes.Urtapregs.FinishTitWorshipWivUrta.UnlatchesFromBreast".Localized());

            OutputText("Scenes.Urtapregs.FinishTitWorshipWivUrta.DeclinesSecondMeal".Localized());

            OutputText("Scenes.Urtapregs.FinishTitWorshipWivUrta.PokesHisBelly".Localized());

            OutputText("Scenes.Urtapregs.FinishTitWorshipWivUrta.KeepsItInMind".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            Player.ChangeFatigue(40);

            ReturnToCamp(1);
        }

        public void MilkPregnantUrta()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.ToyplayRequestNod".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.CupboardToySearch".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.BashfulMilkerAdmission".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.CarryToysToUrta".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.RoleplaySuggestion".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.FarmerCowRoleplay".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.PuzzledVixenMoos".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.BreastMassageMoos".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.TeasingCowQuestion".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.AttachMilkBottle".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.CockPumpAttachment".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.SlickShaftStrokes".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.PumpEasesShaftIn".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.PumpToysMilking".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtapregs.MilkPregnantUrta.GuideHandToCock".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.CollectFilledBottle".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtapregs.MilkPregnantUrta.StopsStrokingDick".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.PatHeadFetchBottle".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.MuffledMooSuckling".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.ResumePumpingMoans".Localized());

            if (Player.HasCock() && !Player.IsTaur() && Player.CockThatFits(SceneInstance<UrtaScene>.Get().UrtaCapacity()) != 0)
            {
                OutputText("Scenes.Urtapregs.MilkPregnantUrta.SpreadLegsMountAsk".Localized());

                OutputText("Scenes.Urtapregs.MilkPregnantUrta.VaginalWallsGrip".Localized());
            }

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.GurgleCumDrinking".Localized());

            if (Player.HasCock() && !Player.IsTaur() && Player.CockThatFits(SceneInstance<UrtaScene>.Get().UrtaCapacity()) != 0)
            {
                OutputText("Scenes.Urtapregs.MilkPregnantUrta.FinalPumpsWithdraw".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
            }

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.UrtaAdmitsSurprise".Localized());

            OutputText("Scenes.Urtapregs.MilkPregnantUrta.KnowingLookRemark".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ClearButtons();

            AddButton(0, "Common.Button.Go".Localized(), GoHomeHorsecock);

            AddButton(1, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), StayForHorseDickings);
        }

        public void GoHomeHorsecock()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GoHomeHorsecock.DressAndAskFarewell".Localized());

            ReturnToCamp(1);
        }

        public void StayForHorseDickings()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.StayForHorseDickings.SleepyCompanySuggestion".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.JoinHerInBed".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.DriftIntoSlumber".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.OneHourLater".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.WakingStretchPops".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.NoteDiscovery".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.ApologyNoteText".Localized());

            OutputText("Scenes.Urtapregs.StayForHorseDickings.DressAndLeave".Localized());

            ReturnToCamp(2);

            Player.ChangeFatigue(-50);
        }

        public void UrtaPregCravings()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.UrtaPregCravings.CravingInquiry".Localized());

            if (!Player.HasCock())
            {
                OutputText("Scenes.Urtapregs.UrtaPregCravings.PoliteDeclineShake".Localized());

                OutputText("Scenes.Urtapregs.UrtaPregCravings.SuggestSomethingElse".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), PreggoUrtaSmexOrSomething);

                return ;
            }

            OutputText("Scenes.Urtapregs.UrtaPregCravings.SaltyAdmission".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.HelpIdentifyCraving".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.SmirkingBedInvite".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.LieOnHerBed".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.StraddleTeasingRub".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Urtapregs.UrtaPregCravings.MilkTricklingForth".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.NopeNotIt".Localized());

            if (Player.LactationQ() >= 200)
                OutputText("Scenes.Urtapregs.UrtaPregCravings.SweetThoughComment".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.LickingDownBelly".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtapregs.UrtaPregCravings.BallsToken".Localized());
            else
                OutputText("Scenes.Urtapregs.UrtaPregCravings.BaseWord".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.LickToTipMoan".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.BabyDaddyTease".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.GraspSheetsEngulf".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtapregs.UrtaPregCravings.WithYourBallsPrefix".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.ApologeticCravingAsk".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.ResistFacefuckUrge".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Urtapregs.UrtaPregCravings.OtherCocksErect".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.NearingClimaxWarning".Localized());

            OutputText("Scenes.Urtapregs.UrtaPregCravings.RedoubledEfforts".Localized());

            if (Player.Balls > 0 || Player.HasVagina())
            {
                OutputText("Scenes.Urtapregs.UrtaPregCravings.SkilledHandPrefix".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Urtapregs.UrtaPregCravings.YourBalls".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Urtapregs.UrtaPregCravings.AndAnotherPrefix".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Urtapregs.UrtaPregCravings.YourPussy".Localized());
            }

            OutputText("Scenes.Urtapregs.UrtaPregCravings.PopsFreeTaunt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.PopLoad".Localized(), GetBlownByPregnantUrta);

            AddButton(1, "Common.Button.HoldOut".Localized(), HoldOnYouAintGettingYerCumYetYouNaughtyFox);
        }

        public void GetBlownByPregnantUrta()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.BuckingAgainstHand".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.BallsChurnPrefix".Localized());

            OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.CumUrethraTravel".Localized());

            OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.ShockThenAcceptance".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.HelplessOtherCock".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.AsTheyJoinSuffix".Localized());
                else
                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.AsItJoins".Localized());

                OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.InOnOnslaught".Localized());
            }

            OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.UnflinchingSprayAllowed".Localized());

            if (Player.CumQ() < 250)
            {
                OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.SpentCumCovered".Localized());

                OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.LipSmackCritique".Localized());
            }
            else
                if (Player.CumQ() < 1000)
                {
                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.FountainSprayLoad".Localized());

                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.WipingEyesPraise".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.GeyserExplosion".Localized());

                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.SplutteringCumQuestion".Localized());

                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.CumDrenchedAftermath".Localized());

                    OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.FutileEyeWipe".Localized());
                }

            OutputText("Scenes.Urtapregs.GetBlownByPregnantUrta.AfterglowSleep".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WeirdUrtaCravingsConclusion);
        }

        public void HoldOnYouAintGettingYerCumYetYouNaughtyFox()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.WontGiveInEasily".Localized());

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.UrtaNotesLowersHead".Localized());

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.TooMuchThroatClimax".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.YourOtherCockThird".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.CocksBulgingOminously".Localized());
                else
                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.CockBulgingOminously".Localized());
            }

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.UrtaGulpsEveryDrop".Localized());

            if (Player.CumQ() < 250)
            {
                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.YouFeedHerAll".Localized());

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.SplashingCumOnFace".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ConvulsesWord".Localized());
                    else
                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ConvulseWord".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.InOrgasm".Localized());
                }

                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ReachYourLimit".Localized());

                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ConvincedNoMoreCum".Localized());
            }
            else
                if (Player.CumQ() < 1000)
                {
                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.FillHerWithLiquid".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.YourOtherCockAgain".Localized());

                        if (Player.CockTotal() == 2)
                            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ContributesWord".Localized());
                        else
                            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ContributeWord".Localized());

                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.MakingCompleteMess".Localized());

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.TheyRandomlySpatter".Localized());
                        else
                            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ItRandomlySpatters".Localized());

                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.SheDoesntMindCum".Localized());
                    }

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.RelaxLetHerSuck".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.UrtaLetsGoPrick".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.CumJetsIntoStomach".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.WorksBestProbably".Localized());

                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.YourOtherCockThird".Localized());

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ShootTheirLoads".Localized());
                        else
                            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ShootsItsLoad".Localized());

                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ErraticSplatterFace".Localized());
                    }

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.UrtaStrugglesToBreathe".Localized());

                    if (Player.HasKnot(Player.BiggestCockIndex()))
                        OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.JustAboveYourKnot".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.WithBothHandsPinch".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.HappyToGiveAll".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ByTimeYoureDone".Localized());

                    OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.ShakesHeadDrumsBelly".Localized());
                }

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.InnocentSmileRelief".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.YourBalls".Localized());
            else
                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.YourselfWord".Localized());

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.DrainedOfYourCum".Localized());

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.UrtaNotesThenYawns".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.AudiblyGurgling".Localized());

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.BellyTiredDespite".Localized());

            OutputText("Scenes.Urtapregs.HoldOnYouAintGettingYerCumYetYouNaughtyFox.GrowingExtremelyDrowsy".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WeirdUrtaCravingsConclusion);
        }

        public void WeirdUrtaCravingsConclusion()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.FeelSomeoneGroping".Localized());

            OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.NoticeSheIsClean".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.CompletelyClean".Localized());
            else
                OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.LackingExtraPaunch".Localized());

            OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.GazeQuestioningly".Localized());

            OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.DrawHerIntoKiss".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.ForkedTongue".Localized());
            else
                if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Demonic)
                    OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.DemonicTongue".Localized());
                else
                    if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Draconic)
                        OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.DraconicTongue".Localized());

            OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.TongueGaspsSurprise".Localized());

            OutputText("Scenes.Urtapregs.WeirdUrtaCravingsConclusion.TakingCueDepart".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -.5f));

            ReturnToCamp(2);
        }

        public void UrtaRaepsJoo()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.TellUrtaAboutBar".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.AskBartenderDrink".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.FinishDrinkThenTease".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.FindEmptyRoomBolt".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SwallowRemoveArmor".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.TellHerPullHerself".Localized(
                Player.ClothedOrNakedLower("[say: 'Sides, all I need is to get these underthings of yours off...]")
            ));

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.CroonsNimbleFingers".Localized(
                Player.ClothedOrNaked("Before you can protest, she attacks the lower half of your clothes, practically ripping it apart in her desire to expose your hindquarters. " + (Player.LowerGarmentName != "nothing" ? "You can feel her tearing off your underwear with her teeth, and definitely hear her growling and shaking it madly in her mouth like a dog with a rat before she spits it away with disdain." : ""))
            ));

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.MoansLewdlyInEars".Localized());

            Player.ButtChange(60, true, true, false);

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.ScreamPleasurePain".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.PregnantVixenSighs".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.FeelPleasureBuildUp".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SwollenTipRubsProstate".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.BallsSlapElectricWaves".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.MoanAudiblyGettingOff".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.GigglesFlutteringFeel".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.ReplyMoanSqueeze".Localized());

            if (Player.CockTotal() > 0)
            {
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.TensesInUnison".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.ThrobsInPreparation".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.UnloadRopesOnFloor".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Urtapregs.UrtaRaepsJoo.DeliverPayloadJets".Localized());
                    else
                    {
                        OutputText("Scenes.Urtapregs.UrtaRaepsJoo.ProdigiousLoadEscape".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.ChurningBalls".Localized());
                        else
                            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.BeingWord".Localized());

                        OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SplashFloorStreams".Localized());
                    }
            }

            if (Player.HasVagina())
            {
                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.PussyGripsNothing".Localized());

                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SquirtingWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.LeakingWord".Localized());

                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.JuicesPaintUrta".Localized());

                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.JoinPreviousDischarge".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.DownTowardsWord".Localized());

                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.FloorGathersPuddle".Localized());

                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.PuddleOfItsOwn".Localized());
            }

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SqueezeMassageGrip".Localized());

                if (Player.Anal.Looseness < 3)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.TightWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.LooseWord".Localized());

                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.AnalMilksUrtaShaft".Localized());

                if (Player.Anal.Wetness > 0)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SecretesOwnWetness".Localized());
            }

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.UrtaMoansGrowlsOn".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.UnableStayAwake".Localized());

            var puddle = 0;

            if (Player.HasVagina())
                if (Player.Wetness() >= 4)
                    puddle += 2;
                else
                    puddle += 1;

            if (Player.HasCock())
                if (Player.CumQ() < 250)
                    puddle += 1;
                else
                    if (Player.CumQ() < 1000)
                        puddle += 2;
                    else
                        puddle += 3;

            if (puddle > 0)
            {
                if (puddle == 1)
                    OutputText("Scenes.Urtapregs.UrtaRaepsJoo.SmallPuddle".Localized());
                else
                    if (puddle == 2)
                        OutputText("Scenes.Urtapregs.UrtaRaepsJoo.ReasonablePuddle".Localized());
                    else
                        OutputText("Scenes.Urtapregs.UrtaRaepsJoo.EnormousPuddle".Localized());

                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.CumPuddleYouMade".Localized());
            }
            else
                OutputText("Scenes.Urtapregs.UrtaRaepsJoo.FloorWord".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.CloseEyesSleepTakes".Localized());

            OutputText("Scenes.Urtapregs.UrtaRaepsJoo.WakeToSoftPillow".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PartTwoOfUrtaBoning);
        }

        public void PartTwoOfUrtaBoning()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.MorningGreetingUrta".Localized());

            Player.RefillHunger(100);

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.ErectPointingAtUrta".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.AlmostFeelHeat".Localized());

                OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.SheJestsUnfazed".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.MoistQuiversEnvy".Localized());

            OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.ConcernedApology".Localized());

            if (Player.AnalCapacity() < 40)
                OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.AssHurtsComplaint".Localized());
            else
                if (Player.AnalCapacity() < 60)
                    OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.NoStrangerToAnal".Localized());
                else
                    OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.LooseButAching".Localized());

            OutputText("Scenes.Urtapregs.PartTwoOfUrtaBoning.CuddlesApologyBreast".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptUrtaRimJobbies);

            AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), DeclineARimJob);
        }

        public void DeclineARimJob()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.DeclineARimJob.AppreciateButDecline".Localized());

            OutputText("Scenes.Urtapregs.DeclineARimJob.TakesWorkGetDecent".Localized());

            OutputText("Scenes.Urtapregs.DeclineARimJob.KissGoodbyeStepOut".Localized());

            ReturnToCamp(1);
        }

        public void AcceptUrtaRimJobbies()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.RimTongueResponsible".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.PlayerKneelsForRim".Localized());
            else
                OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.TurnBackToUrta".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.TongueTipBegins".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.RimCocksThrobbing".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.RimCockThrobbing".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.RimPussyWinking".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.PseudokissDeepens".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.AssShakeAtUrta".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.NodDisappointed".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.GetDressedTogether".Localized());

            OutputText("Scenes.Urtapregs.AcceptUrtaRimJobbies.KissGoodbyeDepart".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Sensitivity / 10)));

            ReturnToCamp(1);
        }

        public void VisitKidsFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.AskUrtaChildVisit".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.VisitKidsFirstTime.RenWord".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.SentencePeriod".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.WantToSeePrefix".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.VisitKidsFirstTime.OurChildSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.VisitKidsFirstTime.OurBabiesPlural".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.UrtaTailWagQuestion".Localized(
                UrtaKidsText("he'll", "she'll", "they'll"),

                UrtaKidsText("his", "her", "their"),

                Player.Mf("father", "other-mother")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) == 0)
                OutputText("Scenes.Urtapregs.VisitKidsFirstTime.ErectionFadesParentalPride".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.FollowWithoutDelay".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.DifferentRouteQuestion".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.OldApartmentRemark".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.UrtaCottageDescription".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.LiannaHomeCall".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.LiannaWelcomeGreeting".Localized(
                Player.Mf("Sir", "Ma'am")
            ));

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.LiannaIntroduction".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.LiannaDepartureQuestion".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.HomeEntryInspection".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.HomeRoomSurvey".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.VisitKidsFirstTime.ChildSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.VisitKidsFirstTime.ChildrenPlural".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.HomeCourtyardStairs".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.BargainRemark".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.LackingDecorationRemark".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.CasualReplyWealthWonder".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.DrainsQuestion".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.PassionateKissReturn".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.PocketKeyDepart".Localized());

            OutputText("Scenes.Urtapregs.VisitKidsFirstTime.KeyItemNotice".Localized());

            Player.CreateKeyItem("Spare Key to Urta's House", 0, 0, 0, 0);

            ReturnToCamp(1);
        }

        public void PickKidSceneHere()
        {
            ClearOutput();

            if ((Player.Pregnancy.Type == PregnancyIds.PlayerFather || (Player.Pregnancy.Incubation < 300 && Player.Pregnancy.Type == PregnancyIds.Urta)) && CoC.Instance.Flags.Get(GameFlagIds.NewBabyAnnounced) == 0)
                NewBabyComing();
            else
                if (View.Time.Hours >= 12 && View.Time.Hours <= 14)
                    HugAttackScene();
                else
                    if (Random.Shared.Next(2) == 0)
                        TakeTheKidsForWalk();
                    else
                        Bathtime();
        }

        public void NewBabyComing()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NewBabyComing.AnnounceToYourPrefix".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.NewBabyComing.ChildSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.NewBabyComing.KidsPlural".Localized(
                    NumberUtils.Num2Text(UrtaKids())
                ));

            OutputText("Scenes.Urtapregs.NewBabyComing.AnnounceImportantNews".Localized());

            if (UrtaKids() < 8)
            {
                OutputText("Scenes.Urtapregs.NewBabyComing.AsPrefix".Localized());

                if (UrtaKids() == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                        OutputText("Scenes.Urtapregs.NewBabyComing.ShePronoun".Localized());
                    else
                        OutputText("Scenes.Urtapregs.NewBabyComing.HePronoun".Localized());

                    OutputText("Scenes.Urtapregs.NewBabyComing.ApproachVerb".Localized());
                }
                else
                    OutputText("Scenes.Urtapregs.NewBabyComing.TheyGatherClause".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.CuriousLookClause".Localized());

                if (UrtaKids() == 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.SSuffix".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.UrtaGivesAtYou".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.ThemPronoun".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                        OutputText("Scenes.Urtapregs.NewBabyComing.HerPronoun".Localized());
                    else
                        OutputText("Scenes.Urtapregs.NewBabyComing.HimPronoun".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.LovingSmilePhrase".Localized());

                if (Player.Pregnancy.Type == PregnancyIds.PlayerFather && Player.Pregnancy.Type == PregnancyIds.Urta)
                    OutputText("Scenes.Urtapregs.NewBabyComing.UrtaComfortingNews".Localized());
                else
                    if (Player.Pregnancy.Type == PregnancyIds.PlayerFather || (Player.Pregnancy.Type == PregnancyIds.Urta))
                    {
                        OutputText("Scenes.Urtapregs.NewBabyComing.BeamingDelightReply".Localized());

                        if (Player.Pregnancy.Type == PregnancyIds.PlayerFather)
                            OutputText("Scenes.Urtapregs.NewBabyComing.HerPronounAgain".Localized());
                        else
                            OutputText("Scenes.Urtapregs.NewBabyComing.YourPronoun".Localized());

                        OutputText("Scenes.Urtapregs.NewBabyComing.BellyWord".Localized());
                    }

                OutputText("Scenes.Urtapregs.NewBabyComing.YourChildSentence".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.RenAreClause".Localized());
                else
                    OutputText("Scenes.Urtapregs.NewBabyComing.IsVerb".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.SilentDigestingNews".Localized());

                if (UrtaKids() == 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.SmallBundleIs".Localized());
                else
                    OutputText("Scenes.Urtapregs.NewBabyComing.SeveralBundlesAre".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.DoingVerb".Localized());

                if (UrtaKids() == 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.ItsPronoun".Localized());
                else
                    OutputText("Scenes.Urtapregs.NewBabyComing.TheirPronoun".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.HugYouBothClause".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.TheyPronoun".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                        OutputText("Scenes.Urtapregs.NewBabyComing.ShePronounAgain".Localized());
                    else
                        OutputText("Scenes.Urtapregs.NewBabyComing.HePronounAgain".Localized());

                if (UrtaKids() == 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.IsVerbAgain".Localized());
                else
                    OutputText("Scenes.Urtapregs.NewBabyComing.AreVerb".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.DelightedAtProspect".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.TheirPronounAgain".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                        OutputText("Scenes.Urtapregs.NewBabyComing.HerPronounThird".Localized());
                    else
                        OutputText("Scenes.Urtapregs.NewBabyComing.HisPronoun".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.HeadWord".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.SSuffixAgain".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.ReturnEmbraceClause".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.TheyPronounAgain".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                        OutputText("Scenes.Urtapregs.NewBabyComing.ShePronounThird".Localized());
                    else
                        OutputText("Scenes.Urtapregs.NewBabyComing.HePronounThird".Localized());

                if (UrtaKids() == 1)
                    OutputText("Scenes.Urtapregs.NewBabyComing.IsVerbThird".Localized());
                else
                    OutputText("Scenes.Urtapregs.NewBabyComing.AreVerbAgain".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.ExcitedAtProspect".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.NewBabyComing.YourBroodGiggles".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.UrtaSheepishReply".Localized());

                OutputText("Scenes.Urtapregs.NewBabyComing.UrtaCrimsonBlush".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.NewBabyAnnounced, 1);

            ReturnToCamp(1);
        }

        public void HugAttackScene()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.HugAttackScene.HugAttackIncoming".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.HugAttackScene.SSuffix".Localized());

            OutputText("Scenes.Urtapregs.HugAttackScene.KitchenSafetyRetreat".Localized(
                Player.Mf("father", "other-mother")
            ));

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.HugAttackScene.EsDoClause".Localized());
            else
                OutputText("Scenes.Urtapregs.HugAttackScene.DoesVerb".Localized());

            OutputText("Scenes.Urtapregs.HugAttackScene.ClamorOffHandsHeld".Localized(
                UrtaKidsText("s", "s", ""),

                UrtaKidsText("his", "her", "their")
            ));

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.HugAttackScene.TheyPronoun".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.HugAttackScene.ShePronoun".Localized());
                else
                    OutputText("Scenes.Urtapregs.HugAttackScene.HePronoun".Localized());

            OutputText("Scenes.Urtapregs.HugAttackScene.InsistentPullClause".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.HugAttackScene.SSuffixAgain".Localized());

            OutputText("Scenes.Urtapregs.HugAttackScene.TowardBackyardClause".Localized());

            OutputText("Scenes.Urtapregs.HugAttackScene.YardPlayAftermath".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), StayToPlay);

            AddButton(1, "Common.Button.No".Localized(), NoPlayTimeForKids);
        }

        public void NoPlayTimeForKids()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NoPlayTimeForKids.ApologeticGoodbye".Localized());

            OutputText("Scenes.Urtapregs.NoPlayTimeForKids.LoverKissGoodbye".Localized());

            ReturnToCamp(1);
        }

        public void StayToPlay()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.StayToPlay.NodHeadAcceptance".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLunchPlay) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.UrtaLunchPlay, CoC.Instance.Flags.Get(GameFlagIds.UrtaLunchPlay) + 1);

                OutputText("Scenes.Urtapregs.StayToPlay.LiannaStepsOver".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.LiannaHandsVial".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.BeyondMaraeRepair".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.SeatYourselfDreading".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.FirstExperimentalBite".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.StrengthReturns".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.HugBackInsist".Localized());

                if (UrtaKids() > 1)
                    OutputText("Scenes.Urtapregs.StayToPlay.RenWord".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.HeartbreakOutrage".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.LiannaCookingHeavenly".Localized());

                Player.RefillHunger(65);

                OutputText("Scenes.Urtapregs.StayToPlay.UrtaDismissiveHuff".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.FinishEatingGetUp".Localized());

                Player.ChangeFatigue(-10);

                Player.HealthChange(-10);

                ReturnToCamp(1);
            }
            else
            {
                OutputText("Scenes.Urtapregs.StayToPlay.WaveLiannaOver".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.SeatYourselfPopCork".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.SecondExperimentalBite".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.SecondHeavenlyMeal".Localized());

                OutputText("Scenes.Urtapregs.StayToPlay.LiannaClearsDishes".Localized());

                Player.ChangeFatigue(-10);

                Player.HealthChange(-10);

                ReturnToCamp(1);
            }
        }

        public void Bathtime()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.BathTime.TellLiannaCheckIn".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.BathTime.KidsPlural".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.BathTime.DaughterWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.BathTime.SonWord".Localized());

            OutputText("Scenes.Urtapregs.BathTime.OfferBathHelp".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), GiveTheKidsABath);

            AddButton(1, "Common.Button.No".Localized(), NoBathTiemPlease);
        }

        public void NoBathTiemPlease()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NoBathTiemPlease.BlankExcuseReply".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public string UrtaKidsText(string male = "", string female = "", string plural = "")
        {

            if (UrtaKids() == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 1)
                    return male;
                else
                    return female;
            }
            else
                return plural;
        }

        public void GiveTheKidsABath()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.LeadsYouToBathroom".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.LiannaPointsToBottles".Localized());

            if (UrtaKids() == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.YourSon".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.YourDaughter".Localized());
            }
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheKids".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.PonderThenUndress".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.LiannaEntersBathroom".Localized());

            if (UrtaKids() == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SonWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.DaughterWord".Localized());
            }
            else
                if (UrtaKids() < 8)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.KidsWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ArmyOfKids".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.SayStartAllRight".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.EveryoneWord".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.StripAndHopIn".Localized());

            if (UrtaKids() == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HePronoun".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShePronoun".Localized());
            }
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyPronoun".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.MutterThenPerkUp".Localized(
                UrtaKidsText("s", "s", ""),

                UrtaKidsText("s", "s", ""),

                UrtaKidsText("he", "she", "they"),

                UrtaKidsText("s", "s", ""),

                UrtaKidsText("his", "her", "their"),

                UrtaKidsText("he was", "she was", "they were"),

                UrtaKidsText("s", "s", ""),

                UrtaKidsText("him", "her", "them"),

                UrtaKidsText("he leaps", "she leaps", "they leap"),

                UrtaKidsText("he", "she", "they"),

                UrtaKidsText("s", "s", ""),

                UrtaKidsText("himself", "herself", "themselves"),

                UrtaKidsText("him", "her", "them"),

                UrtaKidsText("", "", "s")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 0)
            {
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpineSonPhrase".Localized(
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales))
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SLetter".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriod".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 1)
                {
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.YourFirstbornIs".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) == 1)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheOnlyOne".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.AmongThem".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SportingAPrefix".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornCocktype) == 2)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpinePenisPhrase".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.EquinePhallusLikeUrta".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodAgain".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 1 && UrtaKids() > 1)
                {
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.YourNewestIs".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 1)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.AlsoWord".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.BoyWithAPrefix".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornCocktype) == 2)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpinePenisAgain".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.EquinePhallusLikeMom".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodThird".Localized());
                }

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HaveFeeling".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyreWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HesWord".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.QuiteDevelopedWhen".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyHitPhrase".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HeHitsPhrase".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.PubertySizeCompare".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ThemPronoun".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HimPronoun".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.BoastsThatPhrase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyPronounAgain".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HePronounAgain".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.GrowUpToBe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsMales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.StrongFightersLikeHer".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.StrongFighterLikeHer".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) > 0)
            {
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpineHermDaughter".Localized(
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms))
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SLetterAgain".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodFourth".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 3)
                {
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.FirstbornIsHermSporting".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornCocktype) == 2)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpinePenisThird".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.InheritedEquinePhallus".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodFifth".Localized());
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 3 && UrtaKids() > 1)
                {
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.LatestAdditionIs".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 3)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.AlsoWordAgain".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HermWithAPrefix".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornCocktype) == 2)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpinePenisFourth".Localized());
                    else
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.EquinePhallusInherited".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodSixth".Localized());
                }

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HaveFeelingAgain".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyreWordAgain".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShesWord".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.QuiteDevelopedWhenAgain".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyHitAgain".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SheHitsPhrase".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.PubertyMaybeSmaller".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsHerms) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ThemPronounAgain".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronoun".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.RemembersParentsHatedHerm".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) > 0)
            {
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.VulpinePlainDaughter".Localized(
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales))
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SLetterThird".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodSeventh".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.FirstbornIsGirl".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) == 2 && UrtaKids() > 1)
                {
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.LatestAdditionAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.UrtaFirstbornGender) == 2)
                        OutputText("Scenes.Urtapregs.GiveTheKidsABath.AlsoWordThird".Localized());

                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.GirlPhrase".Localized());
                }

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.UrtaLovesToPlay".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ThemPronounThird".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounAgain".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SwearsPhrase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyWillPhrase".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SheWillPhrase".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.QuiteTheHeartbreaker".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SLetterFourth".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.WhenWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaKidsFemales) > 1)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.TheyGrowPhrase".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.SheGrowsPhrase".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.UpWord".Localized());
            }

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.LiannaCollectsClothes".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.NearestWord".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.TugLittleChildCloser".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShePronounAgain".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HePronounThird".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.KicksAndSplashes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounThird".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronoun".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.SmilesUpCooperatively".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounFourth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounAgain".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.StartMassagingHair".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.LiannaBringsClothes".Localized(
                UrtaKidsText("He", "She", "They"),

                UrtaKidsText("he", "she", "they")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.DaughterWordAgain".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SonWordAgain".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.FlicksWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounFifth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounThird".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.SkunketteNoReaction".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.ApologizeSameWayEarlier".Localized(
                UrtaKidsText("he", "she", "they")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.DaughterWordThird".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SonWordThird".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.EarAndTellSuffix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounSixth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HimPronounAgain".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.ApologizeToLianna".Localized(
                UrtaKidsText("He's just a kid", "She's just a kid", "They're just kids")
            ));

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.SqueezesShampooHandful".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.DaughterWordFourth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SonWordFourth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.ChestTicklingSuffix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounSeventh".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HimPronounThird".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.FoxGigglesSquirms".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounEighth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HimPronounFourth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.GrabsBucketToRinse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShePronounThird".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HePronounFourth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.RefusesSoapInEyes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounNinth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounFourth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.PoursWaterOverHead".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounTenth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounFifth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.FoxSquealsShakesWater".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounEleventh".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounSixth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.CascadeOfWaterSpray".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShePronounFourth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HePronounFifth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.DoesSoPhrase".Localized());

            if (UrtaKids() > 2)
            {
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.DelightOfSiblings".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounTwelfth".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounSeventh".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SiblingsWatchFromSide".Localized());
            }

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.SentencePeriodEighth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.CommentLooksLikeFun".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounThirteenth".Localized());
            else
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounEighth".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShakeWaterOffFace".Localized());

            if (UrtaKids() == 1)
            {
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.NowTimeToDo".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounFourteenth".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounNinth".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.GetOutOfTub".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounFifteenth".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HimPronounFifth".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.ClimbsOutForLianna".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounSixteenth".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounTenth".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.StartScrubbingAlong".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.GiveTheKidsABath.WhoIsNextPrompt".Localized());

                if (UrtaKids() > 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.OneOtherChildPhrase".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.OtherChildPhrase".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.PipesUpLetGo".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.GirlWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.BoyWord".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.CurrentlyHoldingPhrase".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.ShePronounFifth".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HePronounSixth".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.SwimAwayNewVolunteer".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaLatestbornGender) >= 2)
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HerPronounSeventeenth".Localized());
                else
                    OutputText("Scenes.Urtapregs.GiveTheKidsABath.HisPronounEleventh".Localized());

                OutputText("Scenes.Urtapregs.GiveTheKidsABath.StartScrubbingAgain".Localized());
            }

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.SighAtHotWater".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.AsksBathingRoutine".Localized());

            OutputText("Scenes.Urtapregs.GiveTheKidsABath.JustCuriousReply".Localized());

            Player.HasGottenWashed();

            ClearButtons();

            AddButton(1, "Common.Button.No".Localized(), NoBathPleaseUrtaLian);

            AddButton(0, "Common.Button.Yes".Localized(), GetABackWashFromLianna);
        }

        public void NoBathPleaseUrtaLian()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.NoBathPleaseUrtaLian.PoliteRefusal".Localized());

            OutputText("Scenes.Urtapregs.NoBathPleaseUrtaLian.SqueezeSoapOnShoulders".Localized());

            OutputText("Scenes.Urtapregs.NoBathPleaseUrtaLian.ConfirmAndLeave".Localized());

            Player.ChangeFatigue(-25);

            ReturnToCamp(2);
        }

        public void GetABackWashFromLianna()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.RequestBackWash".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.OfferBenchSeat".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.ExplainWashPreference".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.ScrubNippleContact".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.RelaxedScrubMassage".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.AskShampooAmount".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.SqueezeSoapOnShoulders".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.LeastICouldDo".Localized());

            OutputText("Scenes.Urtapregs.GetABackWashFromLianna.ExcuseToRinseOff".Localized());

            Player.ChangeFatigue(-30);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Sensitivity / 10)));

            ReturnToCamp(1);
        }

        public void TakeTheKidsForWalk()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.AskWalkDestination".Localized(
                UrtaKidsText("him", "her", "them"),

                UrtaKidsText("him", "her", "them"),

                UrtaKidsText("him", "her", "them")
            ));

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.StateFreshAirWalk".Localized(
                UrtaKidsText("him", "her", "them")
            ));

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.OfferToTakeThem".Localized(
                UrtaKidsText("him", "her", "them"),

                UrtaKidsText("A kid", "A kid", "Kids"),

                UrtaKidsText("s", "s", ""),

                UrtaKidsText("he", "she", "they"),

                UrtaKidsText("him", "her", "them")
            ));

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.EnsureChildPrefix".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RenWord".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.DressedForTraipse".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SLetter".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.DoorToStreet".Localized(
                UrtaKidsText("I", "I", "we")
            ));

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.KidsAskDaddy".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.YourChildPrefix".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RenPleadPrefix".Localized());
            else
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.PleadsWord".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.MarketAsDestination".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ChorusOfCheers".Localized());
            else
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SingleExcitedCheer".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.HardNotToWant".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.HappyFaceSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.HappyFacePlural".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.StaringUpAtYou".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.MeanderingPathToMarket".Localized(
                UrtaKidsText("he", "she", "they"),

                UrtaKidsText("him", "her", "them"),

                UrtaKidsText("f", "f", "ves")
            ));

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ArriveAtMarket".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.FanOutToInspect".Localized());
            else
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RunsOffToInspect".Localized(
                    UrtaKidsText("he", "she", "")
                ));

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ProduceAndTrinketStalls".Localized(
                UrtaKidsText("He knows", "She knows", "They know"),

                UrtaKidsText("he is", "she is", "they are"),

                UrtaKidsText("doesn't", "doesn't", "don't")
            ));

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RenWordAgain".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RunWord".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SLetterAgain".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.UpAndPressPrefix".Localized());

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.EsLetter".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.AgainstFrostyGlass".Localized(
                UrtaKidsText("his face", "her face", "their faces")
            ));

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.TallLankyWolf".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RenWordThird".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ChooseIceCreamFlavors".Localized());

            if (UrtaKids() > 10)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ArmyOfPrefix".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ChildWord".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RenWordFourth".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SentencePeriod".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ParaYourChildPrefix".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.RenWordFifth".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SkipSavoringIceCream".Localized(
                UrtaKidsText("his", "her", "their")
            ));

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.BuriedInConeSingular".Localized());
            else
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.BuriedInConesPlural".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.LickingMeltingDroplets".Localized());

            Player.RefillHunger(20, false);

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.LastBitOfCone".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.AskHowItWent".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.ReplyWithoutHitch".Localized(
                NumberUtils.Num2Text(UrtaKids() + 1)
            ));

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.EsLetterAgain".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.HeadTowardBathroom".Localized(
                UrtaKidsText("shrill protest", "shrill protest", "chorus of shrill protests"),

                UrtaKidsText("he", "she", "they")
            ));

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SLetterThird".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.LiannaNotToTrifle".Localized(
                UrtaKidsText("he", "she", "they")
            ));

            if (UrtaKids() == 1)
                OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SLetterFourth".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.OffToBathroomNote".Localized());

            OutputText("Scenes.Urtapregs.TakeTheKidsForWalk.SkunkHeadsToBath".Localized());

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void VisitTheHouse()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.VisitTheHouse.FollowStreetsToHome".Localized());

            OutputText("Scenes.Urtapregs.VisitTheHouse.SurveyLivingRoom".Localized());

            if (UrtaKids() > 1)
                OutputText("Scenes.Urtapregs.VisitTheHouse.RenWord".Localized());

            OutputText("Scenes.Urtapregs.VisitTheHouse.DoorToBackCourtyard".Localized());

            OutputText("Scenes.Urtapregs.VisitTheHouse.HearRunningWater".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FirstTimeAtUrtaHouse) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.FirstTimeAtUrtaHouse, CoC.Instance.Flags.Get(GameFlagIds.FirstTimeAtUrtaHouse) + 1);

                OutputText("Scenes.Urtapregs.VisitTheHouse.StudyLiveInSitter".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.DryHandsGreeting".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.ProperIntroduction".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.BringUpTheThing".Localized());

                if (Player.Race == "human")
                    OutputText("Scenes.Urtapregs.VisitTheHouse.ConfirmHumanBirth".Localized());
                else
                    OutputText("Scenes.Urtapregs.VisitTheHouse.NoseTwitchScentQuery".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.AskHerBusiness".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.AskResearchIntent".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.AvertEyesThenAnswer".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.AlwaysBackToSex".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.ThankfulSmile".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.GlassesAskBusiness".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.VisitTheHouse.LiannaExitsKitchen".Localized());

                OutputText("Scenes.Urtapregs.VisitTheHouse.AskBusinessToday".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), LiannasAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), () => TalkToLiana());

            AddButton(2, "Common.Button.Sex".Localized(), SexWithLianna);

            AddButton(4, "Common.Button.Kids".Localized(), PickKidSceneHere);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void LiannasAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.LiannasAppearance.LiannaAppearanceIntro".Localized());

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), VisitTheHouse);
        }

        public void TalkToLiana(bool output = true)
        {
            ClearOutput();

            if (output)
                OutputText("Scenes.Urtapregs.TalkToLiana.AskToTalkHer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.HerJob".Localized(), TalkToLianaAboutHerJob);

            AddButton(1, "Common.Button.Herresearch".Localized(), TalkToLianaAboutHerResearch);

            if (CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedMales) + CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedLadies) > 0)
                AddButton(2, "Common.Button.HerSpray".Localized(), TalkAboutLiannasLubeSpray);

            if (CoC.Instance.Flags.Get(GameFlagIds.DiscussedLubeSpray) > 0)
                AddButton(3, "Common.Button.SideEffect".Localized(), LubeSpraySideEffects);

            AddButton(14, "Common.Button.Back".Localized(), VisitTheHouse);
        }

        public void TalkToLianaAboutHerJob()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerJob.AskAboutJobs".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerJob.UrtaJobGodsend".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerJob.AlchemyAmbition".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerJob.AskSomethingElse".Localized());

            TalkToLiana(false);
        }

        public void TalkToLianaAboutHerResearch()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.AskWhySamples".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.EyesLightUpInterest".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.AllHumansMate".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.GiggleHumanMother".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.AskMotherCloseness".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.MotherDiedYoung".Localized());

            if (Player.Corruption >= 50)
                OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.HalfheartedSympathy".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.AskWhatPushedHer".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.GenuineCuriosityShrug".Localized());

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.AskResearchFruit".Localized());

            if (Player.Gender != Gender.Genderless)
            {
                OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.SighNoProgress".Localized());

                OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.PromiseBestEffort".Localized());
            }
            else
            {
                OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.NoSamplesAppeal".Localized());

                OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.StillConsidering".Localized());
            }

            OutputText("Scenes.Urtapregs.TalkToLianaAboutHerResearch.ThanksAskMore".Localized());

            TalkToLiana(false);
        }

        public void TalkAboutLiannasLubeSpray()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.DiscussedLubeSpray, 1);

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.AskWhyLubricant".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.EmbarrassedAvertedEyes".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.ClearsThroatConfession".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.AskSelfExperiments".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.ShrugNoTestSubjects".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.FoolishButNoLecture".Localized());

            OutputText("Scenes.Urtapregs.TalkAboutLiannasLubeSpray.ExplanationDonePrompt".Localized());

            TalkToLiana(false);
        }

        public void LubeSpraySideEffects()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.PastExperimentBackfire".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.NodsRubbingChin".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.TailTentacleMonster".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.ClitGrewHuge".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.VibratingBoobsSight".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.GrewBallsSurprise".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.SecondPairOfArms".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Urtapregs.LubeSpraySideEffects.MildWord".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Urtapregs.LubeSpraySideEffects.SomeWord".Localized());
                else
                    OutputText("Scenes.Urtapregs.LubeSpraySideEffects.GreatWord".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.ReluctanceWord".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.PondersTemporaryOnes".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.AgreeWonderLubeSpray".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.DilatingSphincter".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.MilkProductionCycle".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.AlcoholSwelledTongue".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.BreastGrowthStory".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.SensitiveCanineNose".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.TelepathyClaimDoubt".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.PhotosynthesisAttempt".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.WondersHerSanity".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.AskFavorDetails".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.StareThenNod".Localized());

            OutputText("Scenes.Urtapregs.LubeSpraySideEffects.ThanksAnythingElse".Localized());

            TalkToLiana(false);
        }

        public void SexWithLianna()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.SexWithLianna.SheGrabsYourHand".Localized());

            OutputText("Scenes.Urtapregs.SexWithLianna.LiannaOpensCloset".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtapregs.SexWithLianna.GarmentsFeelTight".Localized());

            OutputText("Scenes.Urtapregs.SexWithLianna.SheAsksWhyDressed".Localized());

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Urtapregs.SexWithLianna.AdmitLackingEquipment".Localized());

                OutputText("Scenes.Urtapregs.SexWithLianna.LiannaHasSolution".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Accept".Localized(), AcceptARandomGenderFromASkunk);

                AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), RefuseGenderlessInjection);

                return ;
            }

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Urtapregs.SexWithLianna.SheCrossesArms".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Harvestgirly".Localized(), null);

            AddButton(1, "Common.Button.Harvestmanly".Localized(), null);

            if (Player.HasVagina())
                if (Player.Pregnancy.Incubation == 0)
                    AddButton(0, "Common.Button.Harvestgirly".Localized(), GetEggsHarvested);
                else
                    OutputText("Scenes.Urtapregs.SexWithLianna.WarningUnfilledWomb".Localized());

            if (Player.HasCock())
                AddButton(1, "Common.Button.Harvestmanly".Localized(), LiannaHandjobbies);

            AddButton(14, "Common.Button.Back".Localized(), VisitTheHouse);
        }

        public void RefuseGenderlessInjection()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.RefuseGenderlessInjection.LiannaAcceptsRefusal".Localized());

            OutputText("Scenes.Urtapregs.RefuseGenderlessInjection.YouPromiseToRemember".Localized());

            ReturnToCamp(1);
        }

        public void AcceptARandomGenderFromASkunk()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.YouAgreeToExperiment".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SheFetchesPointedTool".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.HoldOnMightSting".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SayEnd".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.YouObeyAlchemist".Localized());

            if (Player.Anal.Looseness <= 2)
                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SheNeedsLube".Localized());
            else
                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SheNotesLooseFit".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.PredictablyUnsurprised".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.YouCannotSeeWell".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SheStretchesRosebud".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.YouNodPrepared".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.AssClenchesIntruder".Localized());

            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.LiannaNodsApproving".Localized());

            if (Random.Shared.Next(10) <= 3)
            {
                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.BurningInBlankCrotch".Localized());

                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.BumpFormsInGroin".Localized());

                if (Player.Balls == 0)
                {
                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.LumpsFormAtBase".Localized());

                    Player.Balls = 2;

                    Player.BallSize = 1;
                }

                Player.CreateCock();

                Player.Cocks[0].Length = 4;

                Player.Cocks[0].Thickness = 0.75f;

                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ExamineNewEndowments".Localized());

                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ExperimentSucceeded".Localized());

                OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.DressAndLeave".Localized());
            }
            else
                if (Random.Shared.Next(10) <= 8)
                {
                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ItchInBlankLoins".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ScratchingWorsensItch".Localized());

                    if (Player.BiggestBreastSize() < 1)
                    {
                        OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ItchMovesToChest".Localized());

                        if (Player.NippleLength < .5f)
                        {
                            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.NipplesFormNubs".Localized());

                            Player.NippleLength = .5f;
                        }

                        Player.BreastRows[0].BreastRating = 1;
                    }

                    Player.CreateVagina();

                    Player.SetClitLength(0.25f);

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ExamineNewEndowmentsAgain".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ExperimentSucceededAgain".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.DressAndLeaveAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.MoanAsWarmthSpreads".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.GroinOnFire".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.ReliefThenNewFeeling".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.WeakAfterTransform".Localized());

                    Player.CreateVagina();

                    Player.SetClitLength(0.25f);

                    Player.CreateCock();

                    Player.Cocks[0].Length = 5;

                    Player.Cocks[0].Thickness = 1;

                    if (Player.BiggestBreastSize() < 3)
                    {
                        OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.TiredChestItch".Localized());

                        if (Player.BiggestBreastSize() < 1)
                            OutputText("Scenes.Urtapregs.VisitTheHouse.SLetter".Localized());

                        OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SwellingToCCup".Localized());

                        if (Player.NippleLength < 0.5f)
                        {
                            OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.NipplesHalfInchNubs".Localized());

                            Player.NippleLength = 0.5f;
                        }

                        OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.WatchMoundsGrow".Localized());

                        Player.BreastRows[0].BreastRating = 3;
                    }

                    if (Player.Balls == 0)
                    {
                        OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.PairOfLumpsForm".Localized());

                        Player.Balls = 2;

                        Player.BallSize = 2;
                    }

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.AlchemistIgnoresMess".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.HermNotIntended".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.AsksCanYouWalk".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SheWillCleanMess".Localized());

                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SheTastesCumRope".Localized());
                }

            ReturnToCamp(1);
        }

        public void GetEggsHarvested()
        {
            ClearOutput();

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheBringsSupplies".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.AskAboutLollipop".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SayYouAreReady".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.YouLieOnBed".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheUnwrapsCandy".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedLadies) == 0)
            {
                OutputText("Scenes.Urtapregs.GetEggsHarvested.ConfusionAtGame".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LiannaHavestedLadies, CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedLadies) + 1);
            }
            else
                OutputText("Scenes.Urtapregs.GetEggsHarvested.RecallWhatComesNext".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheShovesDildoIn".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.ShePumpsTheToy".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheExaminesLabia".Localized());

            if (Player.VaginalCapacity() >= 50)
                OutputText("Scenes.Urtapregs.GetEggsHarvested.NoLubeNeeded".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.ShePushesTipIn".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.DildoGlidesIn".Localized());

            if (Player.Vaginas[0].Looseness >= 4)
                OutputText("Scenes.Urtapregs.GetEggsHarvested.LoosePussyNoPain".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.DildoTipHitsCervix".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheProbesSlowly".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.PumpAndBottle".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.LiannaWatchesBlankly".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheSlipsAway".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.DiscardLollipopStick".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.NoFluidAnywhere".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Urtapregs.GetEggsHarvested.ExceptForThePrefix".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Urtapregs.GetEggsHarvested.SmallWord".Localized());
                else
                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Urtapregs.GetEggsHarvested.NormalWord".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Urtapregs.GetEggsHarvested.SizableWord".Localized());
                        else
                            OutputText("Scenes.Urtapregs.GetEggsHarvested.HugeWord".Localized());

                OutputText("Scenes.Urtapregs.GetEggsHarvested.CumFromNeglectedPenis".Localized());
            }

            OutputText("Scenes.Urtapregs.GetEggsHarvested.DildoAbsorbsAll".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SheFeedsLollipop".Localized());

            OutputText("Scenes.Urtapregs.GetEggsHarvested.SighAndLeave".Localized());

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void LiannaHandjobbies()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LiannaHavestedMales, CoC.Instance.Flags.Get(GameFlagIds.LiannaHavestedMales) + 1);

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheBringsSupplies".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.AskHowSheMilks".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Urtapregs.LiannaHandjobbies.MilkOnlyOne".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.ShePointsToBed".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.YouGetComfortable".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheNeedsSomethingFirst".Localized());

                OutputText("Scenes.Urtapregs.LiannaHandjobbies.ShePicksOneCock".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Urtapregs.AcceptARandomGenderFromASkunk.SLetter".Localized());

                OutputText("Scenes.Urtapregs.LiannaHandjobbies.BindingAtTheBase".Localized(
                    NumberUtils.Num2Text(Player.CockTotal() - 1)
                ));
            }

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheReachesBehind".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheUnwrapsCondom".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheSucksYourCock".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.MoreLubeApplied".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Urtapregs.LiannaHandjobbies.PauseToCaressBalls".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.MassageCockHeadPre".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.MoanAtHerSkill".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.GroanWhenReleased".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheDeepthroatsYou".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Urtapregs.LiannaHandjobbies.SeedFillsCondom".Localized());
            else
                if (Player.CumQ() < 4000)
                    OutputText("Scenes.Urtapregs.LiannaHandjobbies.GushesFloodCondom".Localized());
                else
                    OutputText("Scenes.Urtapregs.LiannaHandjobbies.BittenOffTooMuch".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheExtractsHerself".Localized());

            if (!(Player.CumQ() < 1000))
                if (Player.CumQ() < 2000)
                    OutputText("Scenes.Urtapregs.LiannaHandjobbies.BigWord".Localized());
                else
                    if (Player.CumQ() < 3000)
                        OutputText("Scenes.Urtapregs.LiannaHandjobbies.HugeWord".Localized());
                    else
                        OutputText("Scenes.Urtapregs.LiannaHandjobbies.GiantWord".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.CumBubbleEmerges".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheRemovesCondom".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheDeclaresEnough".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Urtapregs.LiannaHandjobbies.ApprovesQuantity".Localized());
                else
                    OutputText("Scenes.Urtapregs.LiannaHandjobbies.WillHoldAWhile".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SheOffersLollipop".Localized());

            OutputText("Scenes.Urtapregs.LiannaHandjobbies.SighAndLeave".Localized());

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

    }
}