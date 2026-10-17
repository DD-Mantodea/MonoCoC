using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<LoppeScene>("coc:loppe")]
    public class LoppeScene : Scene
    {

        public override void Entry()
        {
            LoppeCapacity();
        }

        public int LoppeCapacity()
        {

            return 90;
        }

        public void AppearanceOfLoppe()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeFurry) == 0 || noFur)
            {
                OutputText("Scenes.Loppe.AppearanceOfLoppe.BunnyGirlPortrait".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.OliveSkinAndCuffs".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.SmileAndShorts".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.HorsePhallus".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.TightPussy".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.AppearanceOfLoppe.BunnyMorphPortrait".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.SnowWhiteFur".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.SmileAndShortSkirt".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.HorsePhallusDetail".Localized());

                OutputText("Scenes.Loppe.AppearanceOfLoppe.TightPussyMystery".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), LoppeGenericMeetings);
        }

        public void LoppeFirstMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeFirstMeeting.CrowdedBarArrival".Localized());

            OutputText("Scenes.Loppe.LoppeFirstMeeting.ShowAnnouncement".Localized());

            OutputText("Scenes.Loppe.LoppeFirstMeeting.WatchShowPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesToMeetingLoppe);

            AddButton(1, "Common.Button.NotNow".Localized(), MebbeLaterToLoppe);

            AddButton(2, "Common.Button.Uninterested".Localized(), NoLoppeInterest);
        }

        public void NoLoppeInterest()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.NoLoppeInterest.DeclineDancers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LoppeDisabled, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void MebbeLaterToLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.MebbeLaterToLoppe.LeaveBarUninterested".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void YesToMeetingLoppe()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LoppeMet, 1);

            OutputText("Scenes.Loppe.YesToMeetingLoppe.HarmlessStay".Localized());

            OutputText("Scenes.Loppe.YesToMeetingLoppe.PushThroughCrowd".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), YesToLoppeMeetingTwo);
        }

        public void YesToLoppeMeetingTwo()
        {
            ClearOutput();

            ShowImage("loppe-show-at-bar");

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.BarFallsSilent".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.ExoticFloralRobe".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.GracefulDancer".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.CrowdCheersPoses".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.DancerEyesMeet".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.GlowingClimax".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.FinalSpinFans".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.WatchHerLeave".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.NiceDisplayThought".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.SentencePeriod".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.WaitUpCall".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.TurnAtCall".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.CatchBreathGreeting".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.VisitorExplanation".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.HandshakeGreeting".Localized());

            OutputText("Scenes.Loppe.YesToLoppeMeetingTwo.ShareYourName".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.NotReally".Localized(), NotReallyInterestedInLoppe);

            AddButton(0, "Common.Button.Sure".Localized(), SureBakeryWithLoppe);
        }

        public void NotReallyInterestedInLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.NotReallyInterestedInLoppe.DeclineDisappoints".Localized());

            OutputText("Scenes.Loppe.NotReallyInterestedInLoppe.LookMeUpOffer".Localized());

            OutputText("Scenes.Loppe.NotReallyInterestedInLoppe.TooStrongAside".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void SureBakeryWithLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.AgreeBakeryTrip".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.CarrotCakeOrder".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.UnexpectedSweets".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.SweetToothExcuse".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.SurprisedByAppetite".Localized());

            if (Player.Libido < 33)
                OutputText("Scenes.Loppe.SureBakeryWithLoppe.TooQuickHesitation".Localized());
            else
                if (Player.Libido < 66)
                    OutputText("Scenes.Loppe.SureBakeryWithLoppe.QuickButWilling".Localized());
                else
                    OutputText("Scenes.Loppe.SureBakeryWithLoppe.UnderTableCalf".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.RavenousFootHike".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.WaitressArrives".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.TongueClickRetract".Localized());

            if (Player.Gems >= 30)
            {
                OutputText("Scenes.Loppe.SureBakeryWithLoppe.OrderToCatWaitress".Localized());

                if (Player.Corruption > 40)
                    OutputText("Scenes.Loppe.SureBakeryWithLoppe.WhoPaysThought".Localized());

                Player.Gems -= 30;

            }
            else
                OutputText("Scenes.Loppe.SureBakeryWithLoppe.PriceySpecials".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.WaitressTakesOrder".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.GiggleUnspokenAnswer".Localized());

            OutputText("Scenes.Loppe.SureBakeryWithLoppe.HermaphroditeReveal".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Stickaround".Localized(), OkayLoppeLetsGo);

            AddButton(1, "Common.Button.GoAway".Localized(), NoWayLoppe);
        }

        public void NoWayLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.NoWayLoppe.BitterHeldBreath".Localized());

            OutputText("Scenes.Loppe.NoWayLoppe.DrainDrinkLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LoppeDisabled, 1);

            ReturnToCamp(1);
        }

        public void OkayLoppeLetsGo()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.OkayLoppeLetsGo.StudiesYourHonesty".Localized());

            OutputText("Scenes.Loppe.OkayLoppeLetsGo.KimonoHidesPackage".Localized());

            OutputText("Scenes.Loppe.OkayLoppeLetsGo.ProudOfSex".Localized());

            OutputText("Scenes.Loppe.OkayLoppeLetsGo.QuietInviteHome".Localized(
                Player.Mf("handsome", "beautiful")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesLoppesHouse);

            AddButton(1, "Common.Button.No".Localized(), NoLoppesHouse);
        }

        public void NoLoppesHouse()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.NoLoppesHouse.DisappointedButSmiles".Localized());

            OutputText("Scenes.Loppe.NoLoppesHouse.SighAndLeave".Localized());

            ReturnToCamp(1);
        }

        public void YesLoppesHouse()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.YesLoppesHouse.MischievousShowOffer".Localized());

            OutputText("Scenes.Loppe.YesLoppesHouse.GiddyReply".Localized());

            if (Player.MinLibido >= 50)
            {
                OutputText("Scenes.Loppe.YesLoppesHouse.WarnHighAppetite".Localized());

                OutputText("Scenes.Loppe.YesLoppesHouse.SultryEndlessLust".Localized());
            }
            else
                if (Player.Libido >= 70)
                {
                    OutputText("Scenes.Loppe.YesLoppesHouse.LikeItIntense".Localized());

                    OutputText("Scenes.Loppe.YesLoppesHouse.TightenGripGo".Localized());
                }
                else
                    if (Player.Toughness >= 70)
                    {
                        OutputText("Scenes.Loppe.YesLoppesHouse.ConfidentStamina".Localized());

                        OutputText("Scenes.Loppe.YesLoppesHouse.MinotaurChallenge".Localized());
                    }
                    else
                        OutputText("Scenes.Loppe.YesLoppesHouse.BiteLipDoubt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => LoppeSexChoice(true));
        }

        public void LoppeGenericMeetings()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeGenericMeetings.ApproachTowelGreeting".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), AppearanceOfLoppe);

            AddButton(1, "Common.Button.Talk".Localized(), TalkWithLoppe);

            AddButton(2, "Common.Button.Sex".Localized(), () => LoppeSexChoice());

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void TalkWithLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.TalkWithLoppe.PostponeWorkout".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppe.RatherBodySpeak".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppe.TalkTopicPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Loppe".Localized(), TalkWithLoppeAboutLoppe);

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
            {
                AddButton(1, "Common.Button.Children".Localized(), AskLoppeAboutChildren);

                AddButton(2, "Common.Button.Gossip".Localized(), GossipWithLoppe);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesAskedLoppeAboutLoppe) > 0)
            {
                AddButton(3, "Common.Button.Working".Localized(), TalkWithLoppeAboutWorking);

                AddButton(4, "Common.Button.HerMom".Localized(), ChatWithLoppeAboutHerMom);

                AddButton(5, "Common.Button.HerVillage".Localized(), ChatWithLoppeAboutLoppesVillage);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesAskedLoppeAboutLoppe) > 0 && CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
                AddButton(6, "Common.Button.TeaseHer".Localized(), () => FondleAndTease());

            AddButton(14, "Common.Button.Back".Localized(), LoppeGenericMeetings);
        }

        public void TalkWithLoppeAboutLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.CuriousAboutHer".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.LongStoryWarning".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.PatientChinRest".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.WhereToBegin".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.HerOriginQuestion".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.UnusualUpbringing".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.TakesAfterDad".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesAskedLoppeAboutLoppe) == 0 && !noFur)
            {
                OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.CurseMentioned".Localized());

                OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.CurseEcho".Localized());

                OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.BunnySpeciesLore".Localized());
            }
            else
                OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.DragYouHome".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.ParentsLoveStory".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.FatherLeftFamily".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.InheritedEndowments".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.TakesAfterFather".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.TribeAttacked".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.DemonAmbushBargain".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.SexBoutGamble".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.BraveButFoolish".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.ActiveSexDrive".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.LustSpellMistake".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.NevermindRefusal".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.RiseAndThank".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutLoppe.SeeYouLater".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesAskedLoppeAboutLoppe, 1);

            ReturnToCamp(1);
        }

        public void AskLoppeAboutChildren()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.AskLoppeAboutKids".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.LoppeOpenToKids".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.FatherhoodQuestion".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.LoppeTookPrecautions".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) + CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) > 0)
                OutputText("Scenes.Loppe.AskLoppeAboutChildren.WarnAboutPotency".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.HerbsNotFoolproof".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.NodInApproval".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.LoppeSuspectsInterest".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.LetHerDrawConclusions".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.LoppeWantsChildren".Localized());

            OutputText("Scenes.Loppe.AskLoppeAboutChildren.PonderParenthood".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DontWant".Localized(), LoppeIDontReallyWantKidsYouStupidTwat);

            AddButton(1, "Common.Button.Maybe".Localized(), LoppeKidsAreOkayIfYoureARabbitOrSumthin);

            AddButton(2, "Common.Button.Someday".Localized(), LoppeIWantKidsSomedayJeezeQuitHasslingMe);

            AddButton(3, "Common.Button.Soon".Localized(), LoppeIWantKidsSoonOkayCanWeFuck);

            AddButton(4, "Common.Button.NoOpinion".Localized(), NoOpinionOnLoppeArt);
        }

        public void NoOpinionOnLoppeArt()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.NoOpinionOnLoppeArt.DeclineToAnswer".Localized());

            OutputText("Scenes.Loppe.NoOpinionOnLoppeArt.MakingConversation".Localized());

            OutputText("Scenes.Loppe.NoOpinionOnLoppeArt.LoppeDepartsQuietly".Localized());

            ReturnToCamp(1);
        }

        public void LoppeIDontReallyWantKidsYouStupidTwat()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeIDontReallyWantKidsYouStupidTwat.ConfessNotKidPerson".Localized());

            OutputText("Scenes.Loppe.LoppeIDontReallyWantKidsYouStupidTwat.LoppeDisappointed".Localized());

            OutputText("Scenes.Loppe.LoppeIDontReallyWantKidsYouStupidTwat.WatchHerLeave".Localized());

            ReturnToCamp(1);
        }

        public void LoppeKidsAreOkayIfYoureARabbitOrSumthin()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeKidsAreOkayIfYoureARabbitOrSumthin.KidsLeftToChance".Localized());

            OutputText("Scenes.Loppe.LoppeKidsAreOkayIfYoureARabbitOrSumthin.LoppeProbesChance".Localized());

            OutputText("Scenes.Loppe.LoppeKidsAreOkayIfYoureARabbitOrSumthin.SureYoullFindSomeone".Localized());

            OutputText("Scenes.Loppe.LoppeKidsAreOkayIfYoureARabbitOrSumthin.SheRisesAndDeparts".Localized());

            ReturnToCamp(1);
        }

        public void LoppeIWantKidsSomedayJeezeQuitHasslingMe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeIWantKidsSomedayJeezeQuitHasslingMe.AlwaysWantedChildren".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSomedayJeezeQuitHasslingMe.LoppeHasHope".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSomedayJeezeQuitHasslingMe.TradeSecretsSlyly".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSomedayJeezeQuitHasslingMe.LoppeHintsInterest".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSomedayJeezeQuitHasslingMe.WatchHerWalkAway".Localized());

            ReturnToCamp(1);
        }

        public void LoppeIWantKidsSoonOkayCanWeFuck()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeIWantKidsSoonOkayCanWeFuck.DeclareWantParenthood".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSoonOkayCanWeFuck.LoppeOffersToHelp".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSoonOkayCanWeFuck.IsThatASuggestion".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSoonOkayCanWeFuck.LoppeTeasesBack".Localized(
                Player.Mf("handsome", "beautiful")
            ));

            OutputText("Scenes.Loppe.LoppeIWantKidsSoonOkayCanWeFuck.KeepThatInMind".Localized());

            OutputText("Scenes.Loppe.LoppeIWantKidsSoonOkayCanWeFuck.LoppeInvitesYouBack".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppe()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppe.AskLoppeForGossip".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppe.LoppeWeighsRequest".Localized());

            ClearButtons();

            if (!GameSettings.UrtaDisabled)
                AddButton(0, "Common.Button.Urta".Localized(), GossipWithLoppeAboutUrta);

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) > 0)
                AddButton(1, "Common.Button.Scylla".Localized(), GossipWithLoppeAboutScylla);

            if (CoC.Instance.Flags.Get(GameFlagIds.JasunMet) > 0)
                AddButton(2, "Common.Button.Jasun".Localized(), GossipWithLoppeAboutJasun);

            if (CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) > 0)
                AddButton(3, "Common.Button.Heckel".Localized(), GossipWithLoppeAboutHeckel);

            AddButton(4, "Common.Button.Edryn".Localized(), GossipWithLoppeAboutEdryn);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) > 0)
                AddButton(5, "Common.Button.Lottie".Localized(), GossipWithLoppeAboutLottie);

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) > 0)
                AddButton(6, "Npcs.Cotton.Name".Localized(), GossipWithLoppeAboutCotton);

            AddButton(14, "Common.Button.Back".Localized(), TalkWithLoppe);
        }

        public void GossipWithLoppeAboutUrta()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) <= 0 || CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) == -1)
            {
                OutputText("Scenes.Loppe.GossipWithLoppeAboutUrta.UrtaLegendaryCaptain".Localized());

                OutputText("Scenes.Loppe.GossipWithLoppeAboutUrta.LoppeOutOfDetails".Localized());

                ReturnToCamp(1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LoppeUrtaChats) == 0)
                {
                    OutputText("Scenes.Loppe.GossipWithLoppeAboutUrta.LoppeHintsAtYou".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.ItsMe".Localized(), ItsMeFuckingUrtaLoppe);

                    AddButton(1, "Common.Button.No".Localized(), NoLoppeWhosFuckingUrta);
                }
                else
                {
                    OutputText("Scenes.Loppe.GossipWithLoppeAboutUrta.LoppeSmugTriumph".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.PlayAlong".Localized(), PlayAlongWivLoppesesUrtaGossip);

                    AddButton(1, "Common.Button.Admit".Localized(), AdmitToLoppeThatYouLoveZeHorsecock);

                    AddButton(2, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), DenyToLoppeThatYouLoveZeHorsecock);
                }
        }

        public void NoLoppeWhosFuckingUrta()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.NoLoppeWhosFuckingUrta.DenyAnyKnowledge".Localized());

            ReturnToCamp(1);
        }

        public void ItsMeFuckingUrtaLoppe()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LoppeUrtaChats, 1);

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.LoppeSurprisedByNews".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.DancerPoutsUnfair".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.SilentGlassSignal".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.IncredulousHermProtests".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.RaisedEyebrowReply".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.LoppeBlurtsThenRallies".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.LaughAndAgree".Localized());

            OutputText("Scenes.Loppe.ItsMeFuckingUrtaLoppe.LoppeGrinsEncouraging".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            ReturnToCamp(1);
        }

        public void PlayAlongWivLoppesesUrtaGossip()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.HypotheticalWhatIf".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.LoppeListsDeeds".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.ShiverAtProspect".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.SureAsDaylight".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.MaybeHerLuckyDay".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.LoppeDreamilyDescribes".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.AgreeThenReveal".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.LoppeSlackJawed".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.VoiceLiquidWithLust".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.LoppeOnTheVerge".Localized());

            OutputText("Scenes.Loppe.PlayAlongWivLoppesesUrtaGossip.LeaveOrRelieve".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            FondleAndTease(false);
        }

        public void AdmitToLoppeThatYouLoveZeHorsecock()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.AdmitDickgirlFetish".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.LoppeRubsChin".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.SheDoesntThinkLess".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.LoppeLaughsAtWorry".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.ReliefWashesOver".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.LoppeReassuresYou".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.SmileTurnsStudied".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.RecountMeetingUrta".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.LoppeApprovesOfUrta".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.TwoSuchWomen".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.OddlySpecificFetish".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.CompareToOtherFetishes".Localized());

            OutputText("Scenes.Loppe.AdmitToLoppeThatYouLoveZeHorsecock.LoppeMovesOn".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => LoppeSexChoice(true));

            AddButton(1, "Common.Button.No".Localized(), DontLoppesHouse4Fucks);
        }

        public void DontLoppesHouse4Fucks()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.DontLoppesHouse4Fucks.LoppeRaspberriesYou".Localized());

            OutputText("Scenes.Loppe.DontLoppesHouse4Fucks.DancerLeavesSlyly".Localized());

            ReturnToCamp(1);
        }

        public void DenyToLoppeThatYouLoveZeHorsecock()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.DenyToLoppeThatYouLoveZeHorsecock.NotInTheMood".Localized());

            OutputText("Scenes.Loppe.DenyToLoppeThatYouLoveZeHorsecock.LoppePoutsAtRefusal".Localized());

            OutputText("Scenes.Loppe.DenyToLoppeThatYouLoveZeHorsecock.LoppeWavesAndRuns".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppeAboutScylla()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppeAboutScylla.ScyllaTheNun".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutScylla.BetLoppeWouldAccept".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutScylla.NunLooksThePart".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutScylla.NoNaughtyNunFetish".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutScylla.LoppeStatesBluntly".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppeAboutJasun()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppeAboutJasun.JasunTheShark".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutJasun.LoppeDealtWithHim".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutJasun.PoolRunInStory".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppeAboutHeckel()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppeAboutHeckel.AskAboutHermHyena".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutHeckel.LoppeScowlsAtHer".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutHeckel.SheShakesHerHead".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutHeckel.LoppeDislikesHer".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppeAboutEdryn()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppeAboutEdryn.CentauressWatchwoman".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutEdryn.LookSuggestsQuestion".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutEdryn.DancerAdmitsRides".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppeAboutLottie()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppeAboutLottie.PigGirlAtGym".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieTrainer) == 1)
                OutputText("Scenes.Loppe.GossipWithLoppeAboutLottie.FoundGymBuddy".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) > 0)
                    OutputText("Scenes.Loppe.GossipWithLoppeAboutLottie.NeedsRealPartner".Localized());
                else
                    OutputText("Scenes.Loppe.GossipWithLoppeAboutLottie.LoppeSmilesNervously".Localized());

            ReturnToCamp(1);
        }

        public void GossipWithLoppeAboutCotton()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.GossipWithLoppeAboutCotton.YogaHorseGirl".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutCotton.LoppeDefensiveComplex".Localized());

            OutputText("Scenes.Loppe.GossipWithLoppeAboutCotton.DancerGigglesTease".Localized());

            ReturnToCamp(1);
        }

        public void TalkWithLoppeAboutWorking()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.CurseAffectsSocialLife".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.MasseuseTraining".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.AskAboutEnjoyment".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.LessStressThanDancing".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.StudyAnatomyFirst".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.WantsPracticePartner".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.TuckFactAway".Localized());

            OutputText("Scenes.Loppe.TalkWithLoppeAboutWorking.BackToExercises".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesAskedLoppeAboutLoppe, 2);

            ReturnToCamp(1);
        }

        public void ChatWithLoppeAboutHerMom()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.SuperMom".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.AskAboutFamilyLife".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.MomNamedUma".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.TeaseAboutBeingBad".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.ClientsNotRespectful".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.AskIfUmaKnows".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.NotEverythingKnown".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.WeakSecretaryExcuse".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.OnTheSpotExcuse".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.AskIfUmaDating".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.MomSeesNoOne".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.PointOutGirlfriend".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.UmaNotAttracted".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.WantsPerfectLover".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) < 1)
            {
                OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.AskAboutIntroduction".Localized());

                OutputText("Scenes.Loppe.ChatWithLoppeAboutHerMom.OfferIntroduction".Localized());
            }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppePcMetUma) < 1)
            {
                AddButton(0, "Common.Button.VisitMom".Localized(), SceneInstance<UmasShopScene>.Get().FirstVisitPart1);

                AddButton(1, "Common.Button.MebbeLater".Localized(), () => ReturnToCamp(1));
            }
            else
                AddButton(0, "Common.Button.Next".Localized(), () => ReturnToCamp(1));
        }

        public void ChatWithLoppeAboutLoppesVillage()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.ChatWithLoppeAboutLoppesVillage.AskAboutVillage".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutLoppesVillage.PeacefulRiverHome".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutLoppesVillage.CustomsSeemStrange".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutLoppesVillage.OpenFrontShirt".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutLoppesVillage.BackToDancing".Localized());

            OutputText("Scenes.Loppe.ChatWithLoppeAboutLoppesVillage.HopsUpToDemonstrate".Localized());

            ReturnToCamp(1);
        }

        public void LoppeSexChoice(bool bakery = false)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) == 0)
            {
                if (!bakery)
                {
                    OutputText("Scenes.Loppe.LoppeSexChoice.ChooseWorkoutOption".Localized());

                    OutputText("Scenes.Loppe.LoppeSexChoice.WarnsCarriedAway".Localized());

                    if (Player.MinLibido >= 50)
                    {
                        OutputText("Scenes.Loppe.LoppeSexChoice.WarnHighAppetite".Localized());

                        OutputText("Scenes.Loppe.LoppeSexChoice.TakeCareOfLust".Localized());
                    }
                    else
                        if (Player.Libido >= 70)
                        {
                            OutputText("Scenes.Loppe.LoppeSexChoice.PrefersIntense".Localized());

                            OutputText("Scenes.Loppe.LoppeSexChoice.TightensHoldAndLeads".Localized());
                        }
                        else
                            if (Player.Toughness >= 70)
                            {
                                OutputText("Scenes.Loppe.LoppeSexChoice.ConfidentInStamina".Localized());

                                OutputText("Scenes.Loppe.LoppeSexChoice.StrongerThanMinotaurs".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Loppe.LoppeSexChoice.StillWantsThis".Localized());

                                if (Player.Libido >= 40)
                                    OutputText("Scenes.Loppe.LoppeSexChoice.AlmostSoundsExciting".Localized());
                            }

                    OutputText("Scenes.Loppe.LoppeSexChoice.CannotStopOnceStarted".Localized());

                    OutputText("Scenes.Loppe.LoppeSexChoice.OminousOrIntriguing".Localized());
                }

                OutputText("Scenes.Loppe.LoppeSexChoice.FewBlocksAway".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.UnusualHouse".Localized());

                if (Player.Corruption < 40)
                    OutputText("Scenes.Loppe.LoppeSexChoice.CuriousAboutInterior".Localized());
                else
                    OutputText("Scenes.Loppe.LoppeSexChoice.WardsDoNotWork".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.UnlocksTheDoor".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.MomNotHome".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.ShowsHerRoom".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.PoliteCuriousLook".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.SlipInsideAndWait".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.MutualAdmiration".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.AsksYourPreference".Localized());

                if (Player.HasCock())
                {
                    if (Player.SmallestCockArea() <= LoppeCapacity())
                        OutputText("Scenes.Loppe.LoppeSexChoice.CowgirlRide".Localized());
                    else
                        OutputText("Scenes.Loppe.LoppeSexChoice.NervousLooks".Localized());

                    OutputText("Scenes.Loppe.LoppeSexChoice.ShowAndCompare".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Loppe.LoppeSexChoice.TakeHerVaginally".Localized());

                if (Player.BiggestBreastSize() >= 5)
                    OutputText("Scenes.Loppe.LoppeSexChoice.BetweenYourChest".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.ReceiveAnally".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.LoppeSexChoice.ChooseOtherWorkout".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.DancerPecksLips".Localized());

                if (Player.MinLibido >= 50)
                {
                    OutputText("Scenes.Loppe.LoppeSexChoice.FewCanSateHunger".Localized());

                    OutputText("Scenes.Loppe.LoppeSexChoice.KnewYoudReturn".Localized());
                }
                else
                    if (Player.Libido >= 70 && Player.Libido < 90)
                    {
                        OutputText("Scenes.Loppe.LoppeSexChoice.HeldOutWell".Localized());

                        OutputText("Scenes.Loppe.LoppeSexChoice.TeasesAboutSnoring".Localized());

                        OutputText("Scenes.Loppe.LoppeSexChoice.SheFellAsleepToo".Localized());
                    }
                    else
                        if (Player.Libido >= 90 && Player.MinLibido >= 30)
                        {
                            OutputText("Scenes.Loppe.LoppeSexChoice.LustierThanHer".Localized());

                            OutputText("Scenes.Loppe.LoppeSexChoice.CatchYouOneDay".Localized());
                        }
                        else
                            if (Player.Toughness >= 70)
                            {
                                OutputText("Scenes.Loppe.LoppeSexChoice.WantRematch".Localized());

                                OutputText("Scenes.Loppe.LoppeSexChoice.RematchLiterally".Localized());
                            }
                            else
                                OutputText("Scenes.Loppe.LoppeSexChoice.HandThenHug".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.CoastIsClear".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.DeliberateOrNot".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.HopefulInvitation".Localized());

                if (Player.HasCock())
                {
                    if (Player.SmallestCockArea() <= LoppeCapacity())
                        OutputText("Scenes.Loppe.LoppeSexChoice.CowgirlRideAgain".Localized());
                    else
                        OutputText("Scenes.Loppe.LoppeSexChoice.NervousLooksAgain".Localized());

                    OutputText("Scenes.Loppe.LoppeSexChoice.ShowAndCompareAgain".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Loppe.LoppeSexChoice.TakeHerVaginallyAgain".Localized());

                if (Player.BiggestBreastSize() >= 5)
                    OutputText("Scenes.Loppe.LoppeSexChoice.BetweenYourChestAgain".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.ReceiveAnallyAgain".Localized());

                OutputText("Scenes.Loppe.LoppeSexChoice.HandjobOption".Localized());
            }

            if (Player.Lust < 33)
                Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            ClearButtons();

            if (Player.HasCockThatFits(LoppeCapacity()))
                AddButton(0, "Common.Button.CowGirl".Localized(), LoppeRidesCocks);
            else
                AddButton(0, "Common.Button.CowGirl".Localized(), null);

            if (Player.HasCock())
                AddButton(1, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), LoppeWorshipsDicks);
            else
                AddButton(1, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), null);

            if (Player.HasVagina())
                AddButton(2, "Common.Button.Takevaginal".Localized(), GetFuckedInYerTwatYaCunt);
            else
                AddButton(2, "Common.Button.Takevaginal".Localized(), null);

            if (Player.BiggestBreastSize() >= 4)
                AddButton(3, "Common.Button.BoobJob".Localized(), BoobjobLoppe);
            else
                AddButton(3, "Common.Button.BoobJob".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) > 0)
                AddButton(4, "Common.Button.Squeezejob".Localized(), LoppeSqueezedickWhateverThatIs);
            else
                AddButton(4, "Common.Button.Squeezejob".Localized(), null);

            if (Player.IsTaur())
                AddButton(5, "Common.Button.TakeAnal".Localized(), GetAssFuckedByLoppeAsACentaur);
            else
                AddButton(5, "Common.Button.TakeAnal".Localized(), GetButtFuckedNonHoarseByLoppe);

            AddButton(14, "Common.Button.Leave".Localized(), BeATeaseAndLeaveLoppeAfterSexInvite);
        }

        public void LoppeRidesCocks()
        {
            ClearOutput();

            ShowImage("loppe-rides-your-cocks");

            OutputText("Scenes.Loppe.LoppeRidesCocks.ContemplateOptions".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Loppe.LoppeRidesCocks.SettleOnHerBed".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesCocks.RollOverOnFloor".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.AskAboutFeminineHalf".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.LoppeLovesTheIdea".Localized());

            var x = Player.CockThatFits(LoppeCapacity());

            var y = Player.CockThatFits2(LoppeCapacity());

            if (y >= 0 && Player.CockTotal() == 2)
                OutputText("Scenes.Loppe.LoppeRidesCocks.CouldEatBothUp".Localized());
            else
                if (y >= 0 && Player.CockTotal() == 3)
                    OutputText("Scenes.Loppe.LoppeRidesCocks.OnlyHandleTwo".Localized());

            if (Player.CockTotal() > 1 && y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.BetterThanUsual".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.PushedOntoBack".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.TwoOfCocks".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.CockFragment".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.CockPluralSuffix".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.GentleRubbing".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.LoverStartsStroking".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.PrePluralSuffix".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.GaspInSurprise".Localized());

            if (y < 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.SlideShaftAgainstHer".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesCocks.PinionShaftBetween".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.ShiverAtContact".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.BallsBrushHerOwn".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.EnoughForeplay".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.NeighborAimedAtRosebud".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.SlidesYouHome".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.WarmInsideMoan".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Loppe.LoppeRidesCocks.GripPluralSuffix".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.VelvetViseGrip".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.LoppeLostInLust".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.SquealPluralSuffix".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.SquealsOfJoy".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.ThrustPluralSuffix".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.DeliciousStroking".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.AnswersWithKiss".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesCocks.WatertightAss".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.PleasurePainBorder".Localized());

            if (Player.IsDrider())
                OutputText("Scenes.Loppe.LoppeRidesCocks.ThinLegsOption".Localized());
            else
                if (Player.IsTaur())
                    OutputText("Scenes.Loppe.LoppeRidesCocks.ForelegsOption".Localized());
                else
                    OutputText("Scenes.Loppe.LoppeRidesCocks.ArmsOption".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.WrapAroundBunnyGirl".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeFurry) == 0 || noFur)
                OutputText("Scenes.Loppe.LoppeRidesCocks.SmoothSilkFur".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesCocks.SoftFurCoat".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.HoldHerTight".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.RapturousYell".Localized());

            OutputText("Scenes.Loppe.LoppeRidesCocks.ParagraphBreak".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Loppe.LoppeRidesCocks.FaceCoveredInCum".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Noface".Localized(), LoppeRidesYouNoFaceJizz);

                AddButton(1, "Common.Button.Facial".Localized(), LoppeRidesYouSpunksInYourEye);

                AddButton(2, "Common.Button.Hoseher".Localized(), LoppeRidesYouHoseHer);
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), LoppeRidesYouNoFaceJizz);
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));
        }

        public void LoppeRidesYouNoFaceJizz()
        {
            ClearOutput();

            var y = Player.CockThatFits2(LoppeCapacity());

            OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.LetGoOfRabbit".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.ForelegTucksUnderCock".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.HandGuidesCock".Localized());

            OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.UlulationOfEcstasy".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.AssSucklesSecondCock".Localized());

            OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.SentencePeriod".Localized());

            OutputText("Scenes.Loppe.LoppeRidesYouNoFaceJizz.FightingTheUrge".Localized());

            LoppeRidesPCCockFinal();
        }

        public void LoppeRidesYouSpunksInYourEye()
        {
            ClearOutput();

            var x = Player.CockThatFits(LoppeCapacity());

            var y = Player.CockThatFits2(LoppeCapacity());

            OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.KeepYourFragment".Localized());

            if (Player.IsDrider())
                OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.LegsOption".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.ArmsOption".Localized());

            OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.WrappedAroundLover".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.AnalRingConstricts".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.SentencePeriod".Localized());

            OutputText("Scenes.Loppe.LoppeRidesYouSpunksInYourEye.SmellOfHerJuices".Localized());

            Player.SlimeFeed();

            LoppeRidesPCCockFinal();
        }

        public void LoppeRidesYouHoseHer()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.LoppeRidesYouHoseHer.LetGoOfLoppe".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Loppe.LoppeRidesYouHoseHer.CockPluralSuffix".Localized());

            OutputText("Scenes.Loppe.LoppeRidesYouHoseHer.CannotHelpIt".Localized());

            LoppeRidesPCCockFinal();
        }

        public void LoppeRidesPCCockFinal()
        {

            var x = Player.CockThatFits(LoppeCapacity());

            var y = Player.CockThatFits2(LoppeCapacity());

            ShowImage("loppe-rides-your-cock");

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.WombFluidGush".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0 || Player.HasVagina())
            {
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.CommaWhileToken".Localized());

                if (y >= 0)
                    OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.SecondCockAssPump".Localized(
                        Player.CockDescript(y)
                    ));

                if (y >= 0 && Player.HasVagina())
                    OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.AndConnector".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.YourVaginaRef".Localized());

                    if (Player.Wetness() < 3)
                        OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Drizzles".Localized());
                    else
                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Spatters".Localized());
                        else
                            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Gushes".Localized());

                    OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.FemcumOnBed".Localized());
                }
            }

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.CryOutWhileCumming".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeSlumpsAtop".Localized());

            if (Player.IsDrider() || Player.IsTaur())
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Forelegs".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Arms".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.PostCoitalCuddle".Localized());

            if (Player.Libido >= 70 || Player.MinLibido >= 50)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.RememberLibidoAccept".Localized());
            else
                if (Player.Libido >= 50 || Player.MinLibido >= 35)
                    OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.RememberLibidoSigh".Localized());
                else
                    OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.RoundTwoExasperated".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeDisentangles".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.SlidesDownPressing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeFurry) == 0 || noFur)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Skin".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Fur".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.MoistFoldsImpression".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LetterS".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.OfYourCocks".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.SkillfulTongueDance".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LetterSAgain".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.PullToken".Localized());

            if (y < 0)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LetterSThird".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.InvoluntaryShudder".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LetterSRise".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.Rises".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.FullMastSwiftly".Localized());

            if (y < 0)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.CockAlmostSmacks".Localized());
            else
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.CocksAlmostSmack".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LaquineOnNose".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeWhispersShivering".Localized());

            if (Player.MinLibido <= 30 && Player.Libido <= 50)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.StopToTendWarning".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.PointsAtSwollenBalls".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeStraddlesYou".Localized());

            if (y >= 0)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LetterSFourth".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.OnceMorePleasure".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.OneHourLater".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeThanksHugging".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.DespiteMessyOrgasms".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.PeriodSpace".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeComfortablePeck".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.TwoHoursLater".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.WakeWithStart".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeMorningGreeting".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeGigglesSauntering".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) == 0)
            {
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeZeroesTrembling".Localized());

                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.KissBeforeAnswer".Localized());
            }
            else
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.LoppeTakesTray".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.ReleasingPointsChair".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.UndressingThrill".Localized());

            OutputText("Scenes.Loppe.LoppeRidesPCCockFinal.FinishedYouDepart".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            ReturnToCamp(4);
        }

        public void LoppeWorshipsDicks()
        {
            ClearOutput();

            ShowImage("loppe-worships-your-cock");

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.EyesSweepOverLoppe".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.LetterS".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.AsksLoppeAboutDick".Localized());

            if (Player.BiggestCockArea() <= 17.5f)
            {
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.SmallerDickOpinion".Localized());

                OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeLaughsWinks".Localized());
            }
            else
                if (Player.BiggestCockArea() <= 35)
                    OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeNodsAppreciatively".Localized());
                else
                {
                    OutputText("Scenes.Loppe.LoppeWorshipsDicks.HugeDickSayStart".Localized());

                    if (Player.BiggestCockArea() > LoppeCapacity())
                        OutputText("Scenes.Loppe.LoppeWorshipsDicks.TooBigToFit".Localized());
                    else
                        OutputText("Scenes.Loppe.LoppeWorshipsDicks.MaybeTasteFirst".Localized());

                    OutputText("Scenes.Loppe.LoppeWorshipsDicks.SayEndLicksLips".Localized());
                }

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.ConfidenceFromHerWords".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeMotionTowardsHer".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.SeatYourselfForHer".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeKneelsInhales".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeSucksPre".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.GrabsBedForSupport".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LaquineLooksUpSeductively".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeSmilePromise".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.HollowMoanOrgasm".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeUnlatchesPop".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeGrindsBoobs".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeFurry) > 0 && !noFur)
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.FluffyWord".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.CleavageHerWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeFurry) == 0 || noFur)
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.SkinWord".Localized());
            else
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.FurWord".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.TinglesOfPleasure".Localized());

            if (Player.Libido >= 40)
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.CoveredInFluids".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.SentencePeriod".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeDeclaresReady".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppePushesYouBack".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppePantsHumping".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.BallsToBalls".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.DeliciousFrictionEdge".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeAsksPanting".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.NodTooDazed".Localized());

            if (Player.Libido < 33 && Player.MinLibido < 30)
                OutputText("Scenes.Loppe.LoppeWorshipsDicks.GroanFromDepths".Localized());
            else
                if (Player.Libido < 66 && Player.MinLibido < 50)
                    OutputText("Scenes.Loppe.LoppeWorshipsDicks.WinceEqualParts".Localized());
                else
                    OutputText("Scenes.Loppe.LoppeWorshipsDicks.FatigueEbbsAway".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.OneHourManyOrgasms".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.LoppeScreamsLastCum".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.GigglingAsksSensitive".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.TwoHoursLater".Localized());

            OutputText("Scenes.Loppe.LoppeWorshipsDicks.FamiliarFeelingReturns".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            ReturnToCamp(4);
        }

        public void GetFuckedInYerTwatYaCunt()
        {
            ClearOutput();

            ShowImage("loppe-sticks-it-in-your-vagoo");

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.DeliciousPossibilities".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LabiaInvitation".Localized());
            else
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ExposeRumpOffer".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LoppeSmirksAndWinks".Localized());

            if (Player.Femininity > 50)
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SexyHotPrincess".Localized());
            else
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.DashingPrince".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LaquineLoveFill".Localized());

            if (Player.Gender == Gender.Hermaphrodite && !Player.IsTaur())
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.MoveAsideForCocks".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.AndBalls".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ReachYourCunt".Localized());
            }

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SentencePeriod".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LoppeDivesForward".Localized());

            if (!Player.IsTaur() && Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.UnderYourToken".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SackToken".Localized());
                else
                    OutputText(Player.MultiCockDescriptLight());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.Comma".Localized());
            }

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LickAndKissLabia".Localized());

            if (Player.Vaginas[0].Virgin)
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ImpossibleVirginClaim".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LookCoyForHer".Localized());
                else
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WarnHerWithEyes".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LoppeGaspsAndPersists".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ReachOutToHer".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.TapHerNose".Localized());
                else
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SqueezeHerFace".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WouldYouBeHere".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SwallowsAndGrins".Localized());
            }
            else
                if (Player.Vaginas[0].Looseness < 3)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LoppeCommentsSmiling".Localized());
                else
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.MusesAboutRookie".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.AlignsFlaredTip".Localized());

            if (Player.VaginalCapacity() >= 35)
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.HiltedInsideYou".Localized());
            else
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.BumpsYourCervix".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.MoanInDelight".Localized());

            if (Player.Vaginas[0].Virgin)
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.RepressingPainShiver".Localized());
            else
                if (Player.VaginalCapacity() < 35)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.EntranceForcedWide".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SentencePeriodAgain".Localized());

            Player.CuntChange(35, true, true, false);

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.AirilyBeginsHumping".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WrapLegsAroundHips".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ExquisiteTechnique".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.HissAndShudder".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.PrecumDrools".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SentencePeriodThird".Localized());

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.GrabLoppesFace".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.BreaksKissAndGasps".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.EncouragingIndeed".Localized());

                if ((int)Player.Gender >= 3)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SmearBelliesWithPre".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SentencePeriodFour".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.PushingBackAgainst".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SaysBetweenPants".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.RetortsWithExclamation".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.CantReachLoppe".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.PointedStallionComment".Localized());
            }

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.GaspsAndAlmostNeighs".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.NeglectedCockFountain".Localized());

                if (Player.CumQ() < 500)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.PaintingYouBoth".Localized());
                else
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.GushingOnBedsheets".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.DualSexedOrgasm".Localized());
            }

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SentencePeriodFifth".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.EndlessRopesOfSemen".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SmileAndStrokeCheek".Localized());

            if (!Player.IsTaur() && Player.HasCock())
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.QuickCheekPeck".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SentencePeriodSixth".Localized());

            if (Player.Libido >= 66 || Player.MinLibido >= 30)
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.BodyRepliesInstead".Localized());
            else
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.AlreadySmilesAndKisses".Localized());

            OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.OneHourLater".Localized());

            if (Player.Libido > 90 && Player.MinLibido >= 30)
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.GaspAndHeave".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WipesSweatOffBrow".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.NearSatisfiedLust".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.OneAndHalfHours".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SleepyLazyGrinding".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.RollToTheSide".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.CrawlToBulliedMember".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.GroansAndPushesOff".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.BreakfastHelpfulNow".Localized());

                if (Player.Libido == 100 && Player.MinLibido >= 50)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.CouldGoForMore".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.HeaveForAirDeeply".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.RiddenHardPhew".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ShakeHeadNotSure".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WordsFailPleadingLook".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.PleadingLookHorror".Localized());
                else
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.IncendiaryGlare".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.BurstsOutLaughing".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.StareThenLaughOnce".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SidlingUpSnuggle".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.TwoHoursLater".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.AwakenFeelingBetter".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.CasuallyBringsFood".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.BellyRumblesHunger".Localized());

                if (silly)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SandvichJokeToken".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) == 0)
                {
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.GrinsNervously".Localized());

                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.CapturesLipsInKiss".Localized());
                }
                else
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WatchesYouFeast".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.WipeMealRemnants".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.ThinkThenTurnDown".Localized());

                if (Player.Libido >= 50 || Player.MinLibido >= 30)
                    OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.TitillatedThinking".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.LoverLaughsChores".Localized());

                OutputText("Scenes.Loppe.GetFuckedInYerTwatYaCunt.SlideOutAndDress".Localized());
            }

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            ReturnToCamp(4);
        }

        public void GetButtFuckedNonHoarseByLoppe()
        {
            ClearOutput();

            ShowImage("loppe-sticks-it-in-your-butt");

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouSwallowEyesDrawn".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeGigglesWantsIt".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouNodSilently".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.WhySheHasLube".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeMovesToBed".Localized());

            if (Player.Anal.Wetness > 0)
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouSmirkFoldArms".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouMoanArchBack".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeSmirksPatience".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouDoubtHerReluctance".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LubeSquirtedIntoHands".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.WagglesPrickAtYou".Localized());
            }

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouFeignSorrow".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.HorseCockHeadSwells".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeGaspsInPleasure".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.MischievousSmileBallSqueeze".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppePantsGetReady".Localized());

            if (Player.Anal.Looseness == 0)
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppePressesFlaredHead".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouAdmitFirstTime".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouShakeHeadEmphatically".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheBeginsPushingIn".Localized());

                Player.ButtChange(35, true, true, false);

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeSlidesDeepInside".Localized());
            }
            else
                if (Player.Anal.Looseness < 3)
                {
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppePopsTipInside".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeSighsYoureTight".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouExplainYourTightness".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.EyesTwinkleWithEagerness".Localized());

                    Player.ButtChange(35, true, true, false);
                }
                else
                {
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeGlidesInside".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.HugeCockBigLibido".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouPushBackDeeper".Localized());

                    Player.ButtChange(35, true, true, false);
                }

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeGrabsHipsWhispers".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.ShiveringGrindBack".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouMoanAtEmptiness".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.CockWordToken".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LetterS".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.DrippingPreToken".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.AndWordToken".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.CommaSpace".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.PussyOozingFemlube".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouBegForHarder".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeThrustsDeeper".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.HandStrokesYourCock".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.HandTeasesYourVagina".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.WetSlappingNoises".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YoursWordToken".Localized());
            else
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YourButtToken".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeLaysOnTop".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheJamsInDeep".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.GushFromYourCrotch".Localized());

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SpattersVerb".Localized());
                else
                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SplashesVerb".Localized());
                    else
                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.DrenchesVerb".Localized());
                        else
                            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.CompletelySoaksVerb".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.BedLinenSoaked".Localized());
            }

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouCollapseFaceDown".Localized());

            if (Player.Libido >= 66 || Player.MinLibido >= 30)
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouPerkButtUp".Localized());
            else
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.NoTimeToProtest".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheWontTakeRefusal".Localized());
            }

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.OneHourLater".Localized());

            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeRollsOverSated".Localized());

            if (Player.Libido > 90 && Player.MinLibido >= 30)
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouGaspAtAftershock".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouGroanEagerly".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheCannotKeepUp".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.HalfHoursLater".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SleepyGrindingOnTop".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouRollToSide".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouLickDrainedCock".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.ShePushesYouOff".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.BreakfastWouldHelp".Localized());

                if (Player.Libido == 100 && Player.MinLibido >= 50)
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouWantAnotherRound".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouMoanIncoherently".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeAsksIfOkay".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.AnInterestingRide".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouRubDistendedBelly".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LaquinePoutsSilly".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SatyrKids) > 0)
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouLaughSoftly".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.WoundedDignitySleep".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheHoldsYouTrapped".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.TwoHoursLater".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SomethingWigglesInMaw".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheBreaksKiss".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.TooQueasyToEat".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) == 0)
                {
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeLaughsNervously".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.MaybeTellHerLater".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheKnowsYouEnjoyed".Localized());

                    if (Player.Libido < 33)
                        OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.YouGiveFlatStare".Localized());
                    else
                        if (Player.Libido < 66)
                            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.MaybeAnotherTime".Localized());
                        else
                            OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.TemptingButDecline".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeAvertsEyes".Localized());
                }
                else
                {
                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeDrawsShortKiss".Localized());

                    OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheKeepsElaborating".Localized());
                }

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.SheChangesSubject".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeBringsYourArmor".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.HotBathWouldHelp".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.LoppeBlushesAroused".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.OneBathLater".Localized());

                OutputText("Scenes.Loppe.GetButtFuckedNonHoarseByLoppe.CleanAndRefreshed".Localized());
            }

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            ReturnToCamp(4);
        }

        public void GetAssFuckedByLoppeAsACentaur()
        {
            ClearOutput();

            ShowImage("loppe-sticks-it-in-your-butt-you-centaur");

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SwallowEquineSausage".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.WinkingConfidentReply".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.WantsHerCockWorking".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SoundsWonderfulToLoppe".Localized());

            if (Player.Anal.Wetness > 0)
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.NoLubeNeeded".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LoppeGivesAppreciativePraise".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LostInFantasy".Localized());

            if (Player.Anal.Looseness == 0)
            {
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.TooTightDespiteLube".Localized());

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.AdmitFirstTime".Localized());

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ShakeHeadEmphatically".Localized());

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.BeginsPushingSphincter".Localized());

                Player.ButtChange(35, true, true, false);

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LoppeSlidesDeepInside".Localized());
            }
            else
                if (Player.Anal.Looseness < 3)
                {
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.FlaredHeadPopsInside".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LoppeSighsShuddering".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ShoulderGlanceRemark".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.EagerForHardRod".Localized());

                    Player.ButtChange(35, true, true, false);
                }
                else
                {
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.FlaredHeadGlidesInside".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.EnormousCockHugeLibido".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.PushBackMoreInches".Localized());
                }

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.HandGripsBothHips".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.RsLetter".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.YourPussyFragment".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SentencePeriod".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.MoanAtHerTask".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.CockPressedUnderbelly".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SoppingCuntPuddle".Localized());
                else
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.PeriodSpace".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.WorkedUpThanUsual".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.FranticGruntContinues".Localized());

            if ((int)Player.Gender <= 1)
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.WrongSexEntirely".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ContagiousEnthusiasmBucking".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ProstateBumpPleasure".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.AboutToPop".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.IncredibleAssMoan".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ClenchAssAndGroan".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SpasmUnloadOnCarpet".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.VaginaGripsPhantom".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.OrgasmScentClench".Localized());

            if (!Player.HasScales())
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.GoosebumpsOverBody".Localized());
            else
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ScalesStandingOnEnd".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SentencePeriodAgain".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.OhhhYouIncredible".Localized());

            if (Player.HasCockThatFits(LoppeCapacity()))
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.RideMeSometime".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.BigTaurAnotherRound".Localized());

            if (Player.Libido < 33)
            {
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.CentaurLimitsExist".Localized());

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.PromisesGentleButEager".Localized());
            }
            else
                if (Player.Libido < 66)
                {
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ConsiderHerWords".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.CheersBeginThrusting".Localized());
                }
                else
                {
                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.NotStopAtOne".Localized());

                    OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LaquineFingersGlued".Localized());
                }

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.OneHourLater".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.GaspsFinishesInside".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.TooTiredToSpeak".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SeesYouDozedOff".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.TwoHoursLater".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.WakeWithoutHalfbreed".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LoppeGreetsFromDoor".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.AgreeButStaysBehind".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.TooBigForTub".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.MeetMySpoogeSucker".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.LooseEnoughAlready".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.FewMinutesLater".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ExitBathroomLighter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) == 0)
            {
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ApologizesForStuffing".Localized());

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.WarnedAboutLibido".Localized());

                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SwearMakeItGood".Localized());
            }
            else
                OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.SexyCentaurQuestion".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.ClothesBackInRoom".Localized());

            OutputText("Scenes.Loppe.GetAssFuckedByLoppeAsACentaur.FetchArmorFromRoom".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            Player.SlimeFeed();

            ReturnToCamp(4);
        }

        public void LoppeSqueezedickWhateverThatIs()
        {
            ClearOutput();

            ShowImage("loppe-orgazm-denial");

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeDenialCounter) == 0)
            {
                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.WondersAboutDenial".Localized());

                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.AsksWhatInMind".Localized());

                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.OrgasmDenialTest".Localized());

                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.DoubtsIfFeelsGood".Localized());

                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.ExplainsLongestOrgasm".Localized());

                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.LikesLongHardPart".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LoppeDenialCounter) == 1)
                {
                    OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.WrySmileHalfFist".Localized());

                    OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.SmilesThenNods".Localized());
                }
                else
                {
                    OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.WrySmileCurledFist".Localized());

                    OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.SoreFromLastTime".Localized());

                    ClearButtons();

                    AddButton(14, "Common.Button.Back".Localized(), () => LoppeSexChoice());

                    return ;
                }

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.SitDownComfortably".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.OnWord".Localized());
            else
                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.BesideWord".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.PatBedSheets".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.GrinSlidesHandsIn".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.GigglesSpreadsLegs".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.HandMovesToShaft".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.PullsYouToKiss".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.HandsKeepBusyPumping".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.ShaftThrobsInPalms".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.SwitchGripHandsLower".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.LoppeBegsLooserGrip".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.TauntAndTightenGrip".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.RuinedClimaxComplaints".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.FallsBackHumpingHands".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.FondleBallsConfirmGrowth".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.WhinesToBeFreed".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.ChastiseHerImpatience".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.AnotherOrgasmCutsOff".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.ThirdOrgasmCurlingToes".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.BallsSwellingBigger".Localized());

            OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.GaspsAndBegsMercy".Localized());

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.LoosenGripAndStop".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetGo".Localized(), LetsLoppeGoCum);

                AddButton(1, "Common.Button.Hold".Localized(), SuperLoppeOrgasmDenialGo);
            }
            else
            {
                OutputText("Scenes.Loppe.LoppeSqueezedickWhateverThatIs.TemptedToDenyFurther".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetGo".Localized(), LetsLoppeGoCum);

                AddButton(1, "Common.Button.Hold".Localized(), SuperLoppeOrgasmDenialGo);
            }
        }

        public void LetsLoppeGoCum()
        {
            ClearOutput();

            ShowImage("loppe-comes-after-orgasm-denial");

            OutputText("Scenes.Loppe.LetsLoppeGoCum.FingersSlackenCumBurst".Localized());

            OutputText("Scenes.Loppe.LetsLoppeGoCum.GoofySmileAfterClimax".Localized());

            OutputText("Scenes.Loppe.LetsLoppeGoCum.RoomDrippingWithCum".Localized());

            OutputText("Scenes.Loppe.LetsLoppeGoCum.RecoversAndQuips".Localized());

            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Loppe.LetsLoppeGoCum.HelpHerStandNumb".Localized());

                OutputText("Scenes.Loppe.LetsLoppeGoCum.CleanUpWorstMess".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.LetsLoppeGoCum.DistastefulHelpingHand".Localized());

                OutputText("Scenes.Loppe.LetsLoppeGoCum.HoldDoorOpenTeasing".Localized());

                OutputText("Scenes.Loppe.LetsLoppeGoCum.LoppeSwears".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.LoppeDenialCounter, 3);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(30 + Player.Libido / 10)));

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            ReturnToCamp(1);
        }

        public void SuperLoppeOrgasmDenialGo()
        {
            ClearOutput();

            ShowImage("loppe-comes-after-moar-orgasm-denial");

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.GripTightForFear".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.ChainReactionStrokes".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.BallsReturnToNormal".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.SleepyRequestForCloseness".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.SurprisePassionateKiss".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.NumbToesRemark".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.AngryPainAccusation".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.ConcedesThePoint".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.SighAtSpoilsport".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.HaulHerToBath".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.FarewellByeSugar".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.ThreatensNextHoldout".Localized());

            OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.SayEnd".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LoppeDenialCounter, 5);

            if (Player.Corruption >= 90)
            {
                OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.LaughsAboutSqueezing".Localized());

                OutputText("Scenes.Loppe.SuperLoppeOrgasmDenialGo.HurtAndSpeechless".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LoppeDenialCounter, 10);
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(30 + Player.Libido / 10)));

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            ReturnToCamp(1);
        }

        public void BoobjobLoppe()
        {
            ClearOutput();

            ShowImage("loppe-give-her-a-titty-fuck");

            OutputText("Scenes.Loppe.BoobjobLoppe.RubbingBreastsThinking".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.JigglyDistraction".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.FingerBetweenBreasts".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.ExcitedCockBounce".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.InwardLaughAtPace".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.ScoldOrPutAway".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.ReluctantSettleDown".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.LiftCockToCleavage".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.SoftPraiseAndHumping".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.ClaspFirmerRiseFall".Localized());

            if (!Player.HasLongTongue())
                OutputText("Scenes.Loppe.BoobjobLoppe.TongueReachesCock".Localized());
            else
                OutputText("Scenes.Loppe.BoobjobLoppe.TentacleTongueLicks".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.CummingWarningSpurt".Localized());

            if (Player.HasLongTongue())
                OutputText("Scenes.Loppe.BoobjobLoppe.PullTongueBackIn".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.ShutEyesBeforeGlaze".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.PredatorStarePounce".Localized());

            if ((Player.Strength < 50 && !Player.IsTaur()) || (Player.Strength < 30 && Player.IsTaur()))
            {
                OutputText("Scenes.Loppe.BoobjobLoppe.PowerlessUnderHer".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.DizziedByHardCock".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.CockHeadShovedIn".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.TittyfuckAndOralJoy".Localized());

                if (Player.Body.Get<TonguePart>().Id.Path != "human")
                    OutputText("Scenes.Loppe.BoobjobLoppe.TongueLoopsAround".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.SqueezeHorsemeatFaster".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.ManyOrgasmsLaterAgain".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.LastOrgasmDownThroat".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.BelchDrownsRetorts".Localized());

                if (silly)
                    OutputText("Scenes.Loppe.BoobjobLoppe.FamilyMattersThought".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.NodGroanCradleGut".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.FinalBelchSnuggle".Localized());
            }
            else
            {
                OutputText("Scenes.Loppe.BoobjobLoppe.ForceHerToFloor".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.CriesForMore".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.SarcasticFakeProtest".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.PunishmentForManhandling".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.ExcuseCoveredInCum".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.MouthPlansGroan".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.AnswerByPumping".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.ManyOrgasmsLaterAgain".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.LastPumpFinalRope".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.AsksCuddleInPuddle".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.GlazedSkinReflection".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.DoeEyesPleading".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.ParagraphBreak".Localized());

                if (Player.Corruption >= 60)
                    OutputText("Scenes.Loppe.BoobjobLoppe.HumoringGreedyHerm".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.HeftHerOntoBed".Localized());

                OutputText("Scenes.Loppe.BoobjobLoppe.GiveInAndDoze".Localized());
            }

            OutputText("Scenes.Loppe.BoobjobLoppe.TwoHoursLater".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.NippleLickingWake".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.CaughtLickingDefensive".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.RetrieveArmorFromChair".Localized());

            if ((Player.Strength < 50 && !Player.IsTaur()) || (Player.Strength < 30 && Player.IsTaur()))
                OutputText("Scenes.Loppe.BoobjobLoppe.BellySwollenYetUnaffected".Localized());

            OutputText("Scenes.Loppe.BoobjobLoppe.DressThenWaveGoodbye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LoppeTimesSexed, CoC.Instance.Flags.Get(GameFlagIds.LoppeTimesSexed) + 1);

            Player.SlimeFeed();

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            ReturnToCamp(4);
        }

        public void BeATeaseAndLeaveLoppeAfterSexInvite()
        {
            ClearOutput();

            ShowImage("loppe-teaze-and-leave");

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.NotInTheMood".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.LoppeAnnoyedDisappointed".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.AskInnocentLaquineTeased".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.MockHurtAccusation".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.ChasteGentleKiss".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.SheGripesNoFun".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.WanSmileDonsBathrobe".Localized());

            OutputText("Scenes.Loppe.BeATeaseAndLeaveLoppeAfterSexInvite.RedressAndExit".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5 + Random.Shared.Next(5)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void FondleAndTease(bool output = true)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Loppe.FondleAndTease.TalkAboutHerself".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.SneakyHandUnderTable".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.MassageErectingShaft".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.StrokeThroughShorts".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.MaybeAFewTimes".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.HardToConceal".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.SeamRipsOpen".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.ShortsAboutToRip".Localized());

                OutputText("Scenes.Loppe.FondleAndTease.WeighRunOrStay".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Suck".Localized(), TeaseLoppeNSuck);

            AddButton(1, "Common.Button.Handjob".Localized(), TeaseLoppeNHJ);

            AddButton(2, "Common.Button.Kissnrun".Localized(), TeaseLoppeKissRun);
        }

        public void TeaseLoppeNHJ()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.OfferHelpWithProblem".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.HandDartsUnderTable".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.SquealThenCoverMouth".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.SwallowsThenNods".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.FearAndArousalTremble".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.StaffInterrupts".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.ShriekEverythingFine".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.IgnoresOverloudVoice".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.TeaseAboutStiffness".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.PrickRipsMoreSeams".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.DeniesHearingAnything".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.ReachFurtherToFondle".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.BitesLipDrawingBlood".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.ExaggeratedSighWithdraw".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.SheepishAdmission".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.FallOffHorseRetort".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.EyesWidenNervousGulp".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.PrecumBubblesOut".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.FirmGripSlowStroke".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.FinalStrokeThenGrip".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.BegsLetMeCum".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.StrangledGroanThrust".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.CumLikeABomb".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.DreamyStareAfterglow".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.NapkinDispenserJoke".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.CleanUpThenSneak".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNHJ.VoiceNotesWastedCum".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 5)));

            ReturnToCamp(1);
        }

        public void TeaseLoppeNSuck()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.KissThenEagerReturn".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.LetHerVentSteam".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.ChildishRefusalCaress".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.EagerToBeFreed".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SlideUnderTheTable".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.EmbarrassedButAroused".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.GropingForAZipper".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.HorsePrickSpringsFree".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.ConsideringThenLittlePeck".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.ToesCurlNailsDig".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SlipFingersThroughHole".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.WantsLipsOnShaft".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.LegsPullYouCloser".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SmileAndObey".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Loppe.TeaseLoppeNSuck.LongTongueCoils".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SuckleAndGulpSpunk".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.ThatHitsTheSpot".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SlurpAndGulpDown".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.HumpingEachHeadBob".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.HummingVibrations".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SquealsTooLoudly".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.LegsTrapUnderTable".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.NervousGrinExcuse".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.CentauressBendsOver".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.CentauressKeepsSecret".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.QuietThankfulRelief".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.NeglectedPussyDrenches".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.BitingOffTooMuch".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.CatchingBreathUnderTable".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.StraightensAndSmiles".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.StifledBurpGiggle".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.StruggleOutFromTable".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.AwkwardWalkCuriousStares".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeNSuck.SmilesWhileHoldingCock".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 5)));

            ReturnToCamp(1);

            Player.SlimeFeed();
        }

        public void TeaseLoppeKissRun()
        {
            ClearOutput();

            OutputText("Scenes.Loppe.TeaseLoppeKissRun.ComfortingPatsThenLean".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeKissRun.SnatchHandAndLeave".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeKissRun.ShockedLeftWorkedUp".Localized());

            OutputText("Scenes.Loppe.TeaseLoppeKissRun.AdviseSweetAndRelax".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)), new DynStat(GameStats.Corruption, .5f));

            ReturnToCamp(1);
        }

    }
}