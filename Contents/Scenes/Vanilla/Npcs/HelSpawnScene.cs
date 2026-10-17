using MonoCoC.Attributes;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Contents.Followers;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<HelSpawnScene>("coc:hel_spawn")]
    public class HelSpawnScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.HelSpawn);

        public int helspawnMaleSurpriseLastChoice = 0;

        public string debugName => !string.IsNullOrEmpty(helspawnName) ? helspawnName : "Helspawn";

        public string debugHint => "";

        public string helspawnName => CoC.Instance.Flags.Get(GameFlagIds.HelspawnName);

        public override void Entry()
        {

        }

        public void Reset()
        {

            Save.SetBool("footjob", false);

            Save.SetBool("sippedAnemone", false);

            Save.SetBool("wakeUp", false);

            Save.SetBool("surprised", false);

            Save.SetBool("fishing", false);

            Save.SetInt("timeTalkedIncest", 0);

            Save.SetInt("slutspawnLastTimeSex", 0);
        }

        public string ChampionRef()
        {

            return Player.Mf("Daddy", "Mom");
        }

        public bool HelspawnFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 3;
        }

        public bool HelspawnLover()
        {

            return HelspawnFollower() && CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) > 0;
        }

        public bool HelspawnJustDaughter()
        {

            return HelspawnFollower() && CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 0;
        }

        public bool HelPregnant()
        {

            return SceneInstance<HelScene>.Get().Pregnancy.IsPregnant;
        }

        public void SpriteChooser()
        {

            SceneInstance<HelScene>.Get().SpriteChooser();
        }

        public bool HelspawnChaste()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) < 50;
        }

        public bool HelspawnSlutty()
        {

            return !HelspawnChaste();
        }

        public string HelspawnScales()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 1 ? "Common.Text.Black".Localized() : "Common.Text.Red".Localized();
        }

        public string HelspawnEyes()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                return "golden";

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 1)
                return "crimson";

            return "bright green";
        }

        public void HeliasAppearanceScreen()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("monster-hel");

            OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.HelSalamanderHeight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) < 150)
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.GlowBrightOrange".Localized());
            else
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.GlowBrightWhite".Localized());

            OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.AuraPleasantHeat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelIsabellaThreesomeEnabled) >= 1)
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.BlueBandanaWrapped".Localized());

            OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.HumanFaceDetails".Localized());

            if (helspawnName != "")
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.ThighScarsTrio".Localized());
            else
            {
            var pregnancyEvent = Player.Pregnancy.Event;

            if (pregnancyEvent == 1 || pregnancyEvent == 2)
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.PregnancyEarlyBulge".Localized());
            else if (pregnancyEvent == 3)
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.BellySwellingChild".Localized());
            else if (pregnancyEvent == 4)
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.BellyGravidFull".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) >= 150)
                OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.TailWhiteHotHeat".Localized());
            }

            OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.BigSoftBreasts".Localized());

            OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.WarmWetPussy".Localized());

            OutputText("Scenes.Helspawnscene.HeliasAppearanceScreen.AssholeCockDraining".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => SceneInstance<HelFollowerScene>.Get().HeliaFollowerMenu());
        }

        public void HelSpawnsAppearanceScreen()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.DaughterSalamanderStature".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 1)
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.ChitinBlackTone".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.BrightRedTone".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.ScalesCoverLimbs".Localized());

            if (HelspawnSlutty())
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.ScaleBikiniLikeMother".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.SkirtBootsAndBlouse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelIsabellaThreesomeEnabled) >= 1)
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.ScarfBrightColored".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.HumanFaceWithEyes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.EyeColorGolden".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                    OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.EyeColorBrightGreen".Localized());
                else
                    OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.EyeColorCrimson".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.GirlishFeaturesScaleFreckles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.PuppyDogEars".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.ReptilianEars".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.PinkBowAdorned".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.PerkyPalmableBreasts".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnsAppearanceScreen.WarmWetPussy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HelspawnsMainMenu);
        }

        public void HeliaBonusPointsAward()
        {

            CoC.Instance.Flags.Set(GameFlagIds.HeliaKidsChat, 1);

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.ThatNightBoldHeader".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.SleepTuggedAwake".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.NodRubbingSleep".Localized());

            if (CampScene.CompanionsCount() == 1)
                OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.EyebrowCockedQuestion".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.NodAssentFollowHer".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.SqueezeHandsReassure".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.ConfessionStammering".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());

            if (Player.HasVagina() && Player.HasCock())
                OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.ChestThenVagina".Localized());

            if (!Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.VaginaThenChest".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.EverythingElseDenial".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcPromisedHelMonogamyFucks) == 1)
                OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.PastMinotaurDickClaim".Localized());
            else
                OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.SayLoveMinotaurDicks".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.CentaursAndFoxPricks".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.RealLoveNotFake".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PcPromisedHelMonogamyFucks) == 1)
                OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.SinceYouHelpedNeeds".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBonusPointsAward.NotWhatIWanted".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesHeliaILoveYourButtHoleReaming);

            AddButton(1, "Common.Button.No".Localized(), NoYouDontLoveHeliaYouMonster);
        }

        public void NoYouDontLoveHeliaYouMonster()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.NoYouDontLoveHeliaYouMonster.EnjoySexNotLove".Localized());

            OutputText("Scenes.Helspawnscene.NoYouDontLoveHeliaYouMonster.ThanksForHonesty".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelLove, -1);

            HeliaLoveFollowup();
        }

        public void YesHeliaILoveYourButtHoleReaming()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.YesHeliaILoveYourButtHoleReaming.SmilingAcceptance".Localized());

            OutputText("Scenes.Helspawnscene.YesHeliaILoveYourButtHoleReaming.NuzzlingCheekQuestion".Localized());

            OutputText("Scenes.Helspawnscene.YesHeliaILoveYourButtHoleReaming.ShrugNeverHadChance".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelLove, 1);

            HeliaLoveFollowup();
        }

        public void HeliaLoveFollowup()
        {

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.NotFromHereInfertile".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.EvenAnotherWomanSeems".Localized());

            OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.WeChangeEllipsis".Localized());

            OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.UndoCloakClasp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == -1)
                OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.LoveChildNotMe".Localized());
            else
            {
                OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.LoveYouSoMuch".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.FillMeWithSeed".Localized());
                else
                    OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.GrowDickSuggestion".Localized());
            }

            OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.LetsHaveAKid".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(SceneInstance<HelFollowerScene>.Get().HeliaCapacity()))
                AddButton(0, "Common.Button.HaveAKid".Localized(), HaveAKid);
            else
                if (Player.HasCock())
                    OutputText("Scenes.Helspawnscene.HeliaLoveFollowup.TooBigShrinkHint".Localized());

            AddButton(1, "Common.Button.AnotherDad".Localized(), GetAnotherDad);

            AddButton(2, "Common.Button.NoOrLater".Localized(), NoKidsHel);
        }

        public void HaveAKid()
        {
            ClearOutput();

            SpriteChooser();

            SceneInstance<HelScene>.Get().Pregnancy.KnockUpForce(PregnancyIds.PlayerFather, PregnancyIds.SalamanderIncubation);

            OutputText("Scenes.Helspawnscene.HaveAKid.GiveHerTheChild".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.ShudderFingersOnCock".Localized(
                Player.CockDescript(0)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Helspawnscene.HaveAKid.YourBallsFragment".Localized());
            else
                OutputText("Scenes.Helspawnscene.HaveAKid.BaseOfPrick".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.TailCoilingShaft".Localized(
                Player.CockHead()
            ));

            OutputText("Scenes.Helspawnscene.HaveAKid.GrabbedShovedDown".Localized(
                Player.CockHead()
            ));

            OutputText("Scenes.Helspawnscene.HaveAKid.ImpaledHelChestHeaves".Localized());

            if (Player.BiggestBreastSize() <= 2)
                OutputText("Scenes.Helspawnscene.HaveAKid.SupportPressingChest".Localized());
            else
                OutputText("Scenes.Helspawnscene.HaveAKid.GropesTitsKneading".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.LoverGrindsHips".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.VoiceGivesPause".Localized(
                Player.CockHead()
            ));

            OutputText("Scenes.Helspawnscene.HaveAKid.LayBackRoughFucking".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.ClenchTeethPiston".Localized());

            if (Player.Anal.Looseness > 0)
                OutputText("Scenes.Helspawnscene.HaveAKid.TailSnakesAroundLegs".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.FinalThrustsOrgasm".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.MoansKissingYou".Localized());

            if (Player.Pregnancy.Incubation > 0)
                OutputText("Scenes.Helspawnscene.HaveAKid.MothersDeclaration".Localized());
            else
                OutputText("Scenes.Helspawnscene.HaveAKid.FatherMotherDeclaration".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.UpendedMidReply".Localized());

            OutputText("Scenes.Helspawnscene.HaveAKid.GulpWinceBatteredCock".Localized());

            Player.Orgasm("Dick");

            HelFollowerScene.Save.SetInt("lastTimeSex", View.Time.Days);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HaveAHellKidPartII);

            View.Time.Hours = 6;

            View.Time.Days += 1;
        }

        public void HaveAHellKidPartII()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.SeveralHoursLaterHeader".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.LastOunceOfEnergy".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.SunOutMorningRest".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.MadeForEachOther".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.HelBreaksKissAlarm".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.WhatsWrongQuestion".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.SeedInsideMotherhood".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.LiftOntoElbow".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.ForgetAboutItReassure".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1)
                OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.GladYouLoveBack".Localized());
            else
                OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.CantSaySameHeart".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.SentencePeriodSayEnd".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.SnuggleUpTogether".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.RestEasyPromise".Localized());

            if (Player.Tallness >= 90)
                OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.UpWord".Localized());
            else
                if (Player.Tallness <= 72)
                    OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.DownWord".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.HoldingTightBeforeLeaving".Localized());

            OutputText("Scenes.Helspawnscene.HaveAHellKidPartII.LastKissToCamp".Localized());

            ReturnToCamp(1);
        }

        public void GetAnotherDad()
        {
            ClearOutput();

            SpriteChooser();

            if (!Player.HasCock())
            {
                OutputText("Scenes.Helspawnscene.GetAnotherDad.LoveButNotEquipped".Localized());

                if (SceneInstance<TelAdre>.Get().IsAllowedInto())
                    OutputText("Scenes.Helspawnscene.GetAnotherDad.MikoAndMaiSuggestion".Localized());

                OutputText("Scenes.Helspawnscene.GetAnotherDad.AvoidMinotaurReason".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.GetAnotherDad.HelEyebrowSureQuestion".Localized());

                if (SceneInstance<TelAdre>.Get().IsAllowedInto())
                    OutputText("Scenes.Helspawnscene.GetAnotherDad.MikoAndMaiRepeated".Localized());

                OutputText("Scenes.Helspawnscene.GetAnotherDad.MinotaurAvoidRepeated".Localized());
            }

            ClearButtons();

            if (SceneInstance<TelAdre>.Get().IsAllowedInto())
                AddButton(0, "Scenes.Helfollower.HeliaAndFoxTwins.Mai".Localized(), MaiWouldBeTheBestInseminator);

            AddButton(1, "Common.Button.Spiderboy".Localized(), SpiderboyWouldBeBestDad);

            if (Player.HasCockThatFits(SceneInstance<HelFollowerScene>.Get().HeliaCapacity()))
                AddButton(2, "Common.Button.IWill".Localized(), HaveAKid);
            else
                if (!Player.HasCock())
                    AddButton(2, "Common.Button.IWill".Localized(), GrowingDicks4Hel);

            AddButton(3, "Common.Button.NoOrLater".Localized(), NoKidsHel);
        }

        public void MaiWouldBeTheBestInseminator()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.MaiWouldBeTheBestInseminator.MaiLovelyFather".Localized());

            OutputText("Scenes.Helspawnscene.MaiWouldBeTheBestInseminator.NodSayFineAgreement".Localized());

            SceneInstance<HelScene>.Get().Pregnancy.KnockUpForce(PregnancyIds.PlayerFather, PregnancyIds.SalamanderIncubation);

            CoC.Instance.Flags.Set(GameFlagIds.HelNtrTracker, 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnDaddy, 2);

            Go<CampScene>();
        }

        public void SpiderboyWouldBeBestDad()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.SpiderboyWouldBeBestDad.FindSpiderBoy".Localized());

            SceneInstance<HelScene>.Get().Pregnancy.KnockUpForce(PregnancyIds.PlayerFather, PregnancyIds.SalamanderIncubation);

            CoC.Instance.Flags.Set(GameFlagIds.HelNtrTracker, 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnDaddy, 1);

            Go<CampScene>();
        }

        public void GrowingDicks4Hel()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.GrowingDicks4Hel.SearchForGrowthItem".Localized());

            OutputText("Scenes.Helspawnscene.GrowingDicks4Hel.ReallyWantedOurChild".Localized());

            OutputText("Scenes.Helspawnscene.GrowingDicks4Hel.FinishedLeavesLonging".Localized());

            Go<CampScene>();
        }

        public void NoKidsHel()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.NoKidsHel.ShakeHeadChildRefusal".Localized());

            if (Player.HasCockThatFits(SceneInstance<HelFollowerScene>.Get().HeliaCapacity()))
                OutputText("Scenes.Helspawnscene.NoKidsHel.CantFitTooBig".Localized());

            OutputText("Scenes.Helspawnscene.NoKidsHel.ShouldersSlumpCrestfallen".Localized());

            OutputText("Scenes.Helspawnscene.NoKidsHel.NotInterestedForNow".Localized());

            OutputText("Scenes.Helspawnscene.NoKidsHel.PecOnCheekLeaving".Localized());

            Go<CampScene>();
        }

        public void HelGotKnockedUp()
        {

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelNtrTracker, 2);

            OutputText("Scenes.Helspawnscene.HelGotKnockedUp.AfterDecisionCheckIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                OutputText("Scenes.Helspawnscene.HelGotKnockedUp.WarmSmileFragment".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelGotKnockedUp.WanSmileFragment".Localized());

            OutputText("Scenes.Helspawnscene.HelGotKnockedUp.ScootOverSitBeside".Localized());

            OutputText("Scenes.Helspawnscene.HelGotKnockedUp.HowDidItGo".Localized());

            OutputText("Scenes.Helspawnscene.HelGotKnockedUp.AsksConspiratorially".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
            {
                AddButton(0, "Common.Button.Sure".Localized(), SureHelGimmeMaiDetails);

                AddButton(1, "Common.Button.Nope".Localized(), DontTellMeAboutMai);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 1)
                {
                    AddButton(0, "Common.Button.Sure".Localized(), SureHelGimmeSpidahBoyDetails);

                    AddButton(1, "Common.Button.Nope".Localized(), DontTellMeAboutSpiderboy);
                }
        }

        public void SureHelGimmeSpidahBoyDetails()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.ChucklesThenBeginsStory".Localized());

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.TowardHotwingsPlace".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.EventuallyFoundSpidergirls".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.SistersSeekFirstMate".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.SlowTyingHerUp".Localized());

            if (Player.BiggestCockArea() >= 20)
                OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.NothingNextToLover".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.SpreadsHerLegsWide".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.HandsFreeStrokesHair".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.RollsOffExhausted".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeSpidahBoyDetails.SistersJoinInBored".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), DontTellMeAboutSpiderboy);
        }

        public void DontTellMeAboutSpiderboy()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.BucketOfSpiderboyCum".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.AsksWhatsWrong".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.SeedTakingInsideHer".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.ArmAroundShoulder".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.SaysShesFineReally".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1)
                OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.GladYouLoveBack".Localized());
            else
                OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.EvenIfNotLoved".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.SayEnd".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.SnuggleUpTogether".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.TellHerRestEasy".Localized());

            if (Player.Tallness >= 90)
                OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.UpWord".Localized());
            else
                if (Player.Tallness <= 72)
                    OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.DownWord".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.HoldingYouTight".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutSpiderboy.LastKissGoodbye".Localized());

            ReturnToCamp(1);
        }

        public void SureHelGimmeMaiDetails()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.SureHelGimmeMaiDetails.NotEasyGettingAround".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeMaiDetails.AwkwardAskingFriend".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeMaiDetails.TalksHerIntoIt".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeMaiDetails.DropsLikeGodHand".Localized());

            OutputText("Scenes.Helspawnscene.SureHelGimmeMaiDetails.FilthyAtSunrise".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), DontTellMeAboutMai);
        }

        public void DontTellMeAboutMai()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.BucketOfFoxyCum".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.AsksWhatsWrong".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.SeedTakingInsideHer".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.ArmAroundShoulder".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.SaysShesFineReally".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1)
                OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.GladYouLoveBack".Localized());
            else
                OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.EvenIfNotLoved".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.SayEnd".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.SnuggleUpTogether".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.TellHerRestEasy".Localized());

            if (Player.Tallness >= 90)
                OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.UpWord".Localized());
            else
                if (Player.Tallness <= 72)
                    OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.DownWord".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.HoldingYouTight".Localized());

            OutputText("Scenes.Helspawnscene.DontTellMeAboutMai.LastKissGoodbye".Localized());

            ReturnToCamp(1);
        }

        public void BulgyCampNotice()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.BulgyCampNotice.HeliaSunningOnStone".Localized());

            Go<CampScene>();
        }

        public void HeliaSwollenNotice()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HeliaSwollenNotice.HelWandersClutchingBelly".Localized());

            OutputText("Scenes.Helspawnscene.HeliaSwollenNotice.BellyKickingNow".Localized());

            OutputText("Scenes.Helspawnscene.HeliaSwollenNotice.EarToBelly".Localized());

            OutputText("Scenes.Helspawnscene.HeliaSwollenNotice.HelpsHerSitDown".Localized());

            OutputText("Scenes.Helspawnscene.HeliaSwollenNotice.AsksSheGrinning".Localized());

            OutputText("Scenes.Helspawnscene.HeliaSwollenNotice.WhichDoYouWant".Localized());

            ClearButtons();

            AddButton(0, "CharCreation.GenericGenderChoice.Boy".Localized(), YouWantABoy);

            AddButton(1, "CharCreation.GenericGenderChoice.Girl".Localized(), YouWantAGirl);
        }

        public void YouWantABoy()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.YouWantABoy.SheCanSeeIt".Localized());

            if (Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Helspawnscene.YouWantABoy.WantsStrongSon".Localized());
            else
                OutputText("Scenes.Helspawnscene.YouWantABoy.MissesBoysBackHome".Localized());

            OutputText("Scenes.Helspawnscene.YouWantABoy.LadyKillerSon".Localized());

            OutputText("Scenes.Helspawnscene.YouWantABoy.QuietLaughWithLover".Localized());

            Go<CampScene>();
        }

        public void YouWantAGirl()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.YouWantAGirl.GirlWouldBeGreat".Localized());

            if (Player.HasCock() && !Player.HasVagina())
                OutputText("Scenes.Helspawnscene.YouWantAGirl.WarnsAboutGirls".Localized());
            else
                OutputText("Scenes.Helspawnscene.YouWantAGirl.GaggleOfGirls".Localized());

            OutputText("Scenes.Helspawnscene.YouWantAGirl.DaughterRealBeauty".Localized());

            OutputText("Scenes.Helspawnscene.YouWantAGirl.QuietLaughWithLover".Localized());

            Go<CampScene>();
        }

        public void HeliaGravidity()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HeliaGravidity.HelSlowGettingAround".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.GreetsSweetheart".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.HoldsBeamsForHel".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.FeelsFuckingUseless".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.AsksAboutChildName".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.HmQuestion".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.ThinkingAboutNames".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.AsksMotherName".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.MotherNamedTanis".Localized());

            OutputText("Scenes.Helspawnscene.HeliaGravidity.HelStaresIntoDistance".Localized());

            Go<CampScene>();
        }

        public void HeliaTalkSeven()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HeliaTalkSeven, 1);

            OutputText("Scenes.Helspawnscene.HeliaTalkSeven.SecludedSpotWithHel".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkSeven.AsksAboutLayingEggs".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkSeven.SwellWithMilk".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkSeven.ThanksForCheckingIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) != 0)
                OutputText("Scenes.Helspawnscene.HeliaTalkSeven.HopeHeTakesAfter".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkSeven.SentencePeriodSayEnd".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Encourageher".Localized(), EncouragePregalia);

            AddButton(1, "Common.Button.Wellll".Localized(), HelsLifestyle);
        }

        public void EncouragePregalia()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.EncouragePregalia.SqueezeShoulderPraise".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1)
                OutputText("Scenes.Helspawnscene.EncouragePregalia.YouLoveHel".Localized());
            else
                OutputText("Scenes.Helspawnscene.EncouragePregalia.HelIsYourFriend".Localized());

            OutputText("Scenes.Helspawnscene.EncouragePregalia.ChildFollowsFootsteps".Localized());

            OutputText("Scenes.Helspawnscene.EncouragePregalia.KissingHerAnswer".Localized());

            Go<CampScene>();
        }

        public void HelsLifestyle()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelsLifestyle.SeriousAboutRubbingOff".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1)
                OutputText("Scenes.Helspawnscene.HelsLifestyle.HopeYouLoveHer".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelsLifestyle.KnowsWhyNotLoved".Localized());

            OutputText("Scenes.Helspawnscene.HelsLifestyle.WantsYouRaiseChild".Localized());

            OutputText("Scenes.Helspawnscene.HelsLifestyle.FingerToYourLips".Localized());

            Go<CampScene>();
        }

        public void HeliaBirthtime()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HeliaBirthtime.HelWaddlesPregnant".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBirthtime.BestialGrowlAnswer".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBirthtime.KneelsAsksWhatNeeded".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBirthtime.CriesLegsSpreading".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBirthtime.HandAsWaterGushes".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                OutputText("Scenes.Helspawnscene.HeliaBirthtime.YourWord".Localized());
            else
                OutputText("Scenes.Helspawnscene.HeliaBirthtime.HerWord".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBirthtime.ScreamsGrippingHand".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HeliaBirthEpilogue);
        }

        public void HeliaBirthEpilogue()
        {
            ClearOutput();

            SpriteChooser();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                OutputText("Scenes.Helspawnscene.HeliaBirthEpilogue.HealthyBabyCries".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                    OutputText("Scenes.Helspawnscene.HeliaBirthEpilogue.NotQuiteSalamander".Localized());
                else
                    OutputText("Scenes.Helspawnscene.HeliaBirthEpilogue.ArmsHoldNotSalamander".Localized());

            OutputText("Scenes.Helspawnscene.HeliaBirthEpilogue.MotherlyInstinctsKickIn".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NameDatHelspawn);
        }

        public void NameDatHelspawn()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.NameDatHelspawn.ShareLaughWhileSuckling".Localized());

            OutputText("Scenes.Helspawnscene.NameDatHelspawn.WhatToNameHer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ApplyHelspawnName);

        }

        public void ApplyHelspawnName()
        {
            ClearOutput();

            var input = "" ;

            SpriteChooser();

            var forbiddenNames = new[] { "", "Hellgirl", "Kid", "Phoenix", "Savin", "Helia", "Mini-doofus" };

            if (Array.LastIndexOf(forbiddenNames, input) >= 0)
            {
                if (input == "")
                    OutputText("Scenes.Helspawnscene.ApplyHelspawnName.MustSelectNameError".Localized());
                else
                    if (input == "Hellgirl")
                        OutputText("Scenes.Helspawnscene.ApplyHelspawnName.CopyrightInfringement".Localized());
                    else
                        if (input == "Kid")
                            OutputText("Scenes.Helspawnscene.ApplyHelspawnName.WhatAreYouFive".Localized());
                        else
                            if (input == "Phoenix")
                                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.NotAfterThoseThings".Localized());
                            else
                                if (input == "Savin")
                                    OutputText("Scenes.Helspawnscene.ApplyHelspawnName.ThatsBoysName".Localized());
                                else
                                    if (input == "Helia")
                                        OutputText("Scenes.Helspawnscene.ApplyHelspawnName.FavoriteNameTaken".Localized());
                                    else
                                        if (input == "Mini-doofus")
                                            OutputText("Scenes.Helspawnscene.ApplyHelspawnName.KihaWillLaugh".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), ApplyHelspawnName);

                return;
            }

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnName, input);

            if (helspawnName == "Helspawn")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.GonnaCallHerThat".Localized());

            if (helspawnName == "Jayne")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.SoundsLikeHeroName".Localized());

            if (helspawnName == "Hesper")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.EveningStarOfLife".Localized());

            if (helspawnName == "Kiri")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.NamedAfterHerSister".Localized());

            if (helspawnName == "Mai")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.MaiWillBeHonored".Localized());

            if (helspawnName == "Tanis")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.NameHasBaggage".Localized());

            if (helspawnName == "Syn")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.HopedYouChooseIt".Localized());

            if (helspawnName == "Chara")
                OutputText("Scenes.Helspawnscene.ApplyHelspawnName.YouLikedHerIdeas".Localized());

            OutputText("Scenes.Helspawnscene.ApplyHelspawnName.StrongNameForGirl".Localized());

            OutputText("Scenes.Helspawnscene.ApplyHelspawnName.HelpsHeliaToFeet".Localized());

            HelSpawnsSetup();

            ReturnToCamp(2);
        }

        public void HelSpawnsSetup()
        {

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, 50);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 10);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) - 10);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
            {
                if (Player.Corruption >= 66)
                    CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 10);

                if (Player.Corruption <= 33)
                    CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) - 10);
            }

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnAge, 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnGrowupCounter, 1);

            SceneInstance<HelScene>.Get().Pregnancy.KnockUpForce();
        }

        public void HeliaTalkEight()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HelTalkEight, 1);

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HeliaTalkEight.WatchDaughterTogether".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkEight.NodAtGrowthRemark".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkEight.SelfBlameWorldTaint".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkEight.AgreeGirlLooksOlder".Localized());

            if (SceneInstance<AmilyScene>.Get().AmilyFollower())
                OutputText("Scenes.Helspawnscene.HeliaTalkEight.MentionAmilyPotion".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkEight.HelLeansOnShoulder".Localized());

            OutputText("Scenes.Helspawnscene.HeliaTalkEight.NodAndPlayTogether".Localized());

            ReturnToCamp(1);
        }

        public void PlayWithYourKid()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.PlayWithYourKid.OfferToWatchDaughter".Localized());

            OutputText("Scenes.Helspawnscene.PlayWithYourKid.HelGladlyHandsOver".Localized());

            OutputText("Scenes.Helspawnscene.PlayWithYourKid.HeliaExplainsToChild".Localized());

            ClearButtons();

            AddNextButton("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.ButtonPlay".Localized(), HelspawnPlay);

            AddNextButton("Common.Button.Catch".Localized(), HelspawnCatch);

            AddNextButton("Common.Button.Bathe".Localized(), HelspawnBathe);

            AddNextButton("Common.Button.Campfire".Localized(), HelspawnCampfire);

            AddNextButton("Common.Button.Read".Localized(), () => HelspawnRead());

            AddButton(14, "Common.Button.Back".Localized(), () => SceneInstance<HelFollowerScene>.Get().HeliaFollowerMenu());
        }

        public void HelspawnPlay()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelspawnPlay.SpinDaughterLaughing".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnPlay.BoppingMotherNose".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnPlay.WeanBeforeBiting".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnPlay.RustleFieryHair".Localized());

            ReturnToCamp(1);
        }

        public void HelspawnCatch()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnCatch.IdeaRubberBall".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.BallArcsToHer".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.WorrySheGotHurt".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.UnfazedKnockedOver".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.NoticeSheTires".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.GigglingAgreesReturn".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.HelThanksFreeTime".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCatch.DaughterEchoesThanks".Localized());

            ReturnToCamp(1);
        }

        public void HelspawnCampfire()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnCampfire.LateSleepyEyes".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCampfire.UsherToFirepit".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCampfire.FoodDoneConsoleHer".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCampfire.ScaryStoryShadows".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCampfire.NightScaresTuckered".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnCampfire.WhisperThanksTonight".Localized());

            ReturnToCamp(2);
        }

        public void HelspawnBathe()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnBathe.LiftScaledDaughter".Localized(
                HelspawnScales()
            ));

            OutputText("Scenes.Helspawnscene.HelspawnBathe.HeliaSuggestsBath".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnBathe.BathTripStream".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnBathe.TossWashRag".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnBathe.ScrubScalesShine".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnBathe.HungryBellyGrowl".Localized());

            if (!Player.IsLactating())
            {
                OutputText("Scenes.Helspawnscene.HelspawnBathe.SquishHungryCheeks".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnBathe.NippleFeedingRemark".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.HelspawnBathe.SplashAppetiteArmor".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnBathe.FeedJokeDarkens".Localized());

                Player.Milked();
            }

            OutputText("Scenes.Helspawnscene.HelspawnBathe.AdultSoonChat".Localized());

            ReturnToCamp(1);
        }

        public bool HelspawnRead(bool display = true)
        {
            ClearOutput();

            var count = 0;

            if (display)
                ClearButtons();

            if (Player.HasKeyItem("Carpenter's Toolbox"))
            {
                if (display)
                    AddNextButton("Common.Button.CarpentersGuide".Localized(), () => HelspawnRead2(1));

                count += 1;
            }

            if (Player.HasKeyItem("Dangerous Plants"))
            {
                if (display)
                    AddNextButton("Common.Button.DangerousPlants".Localized(), () => HelspawnRead2(2));

                count += 1;
            }

            if (Player.HasKeyItem("Izma's Book - Combat Manual"))
            {
                if (display)
                    AddNextButton("Common.Button.CombatManual".Localized(), () => HelspawnRead2(3));

                count += 1;
            }

            if (Player.HasKeyItem("Izma's Book - Etiquette Guide"))
            {
                if (display)
                    AddNextButton("Common.Button.EtiquetteGuide".Localized(), () => HelspawnRead2(4));

                count += 1;
            }

            if (Player.HasKeyItem("Hentai Comic") || Player.HasKeyItem("Izma's Book - Porn"))
            {
                if (display)
                    AddNextButton("Common.Button.Porn".Localized(), () => HelspawnRead2(5));

                count += 1;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BlackBook)))
            {
                if (display)
                    AddNextButton("Items.BlackBook.Short".Localized(), () => HelspawnRead2(6));

                count += 1;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.WhiteBook)))
            {
                if (display)
                    AddNextButton("Items.WhiteBook.Short".Localized(), () => HelspawnRead2(7));

                count += 1;
            }

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.GrayBook)))
            {
                if (display)
                    AddNextButton("Items.GrayBook.Short".Localized(), () => HelspawnRead2(8));

                count += 1;
            }

            if (display)
            {
                OutputText("Scenes.Helspawnscene.HelspawnRead.OfferReadBook".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnRead.GoodLuckGesture".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnRead.WorthTryOptimism".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnRead.HeliaLaughEmbrace".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnRead.ScoopUpGiggling".Localized());

                if (count > 1)
                    OutputText("Scenes.Helspawnscene.HelspawnRead.ChooseBookPrompt".Localized());
            }

            return count > 0;
        }

        public void HelspawnRead2(int choice)
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnRead2.FindChosenBook".Localized(
                CampScene.HasBookshelf ? "Common.Text.SearchingYourMakeshiftLibrary".Localized() : "Common.Text.RummagingThroughTheCornerOfYour".Localized() + ((CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0) ? "Common.Text.Bedroom".Localized() : "Common.Text.Tent".Localized())
            ));

            switch (choice)
            {
                case 1:
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.CarpenterGuideBook".Localized());

                    break;

                case 2:
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.DangerousPlantsCopy".Localized());

                    break;

                case 3:
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.CombatManualBook".Localized());

                    break;

                case 4:
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.EtiquetteGuideBook".Localized());

                    break;

                case 5:
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.LicentiousLiterature".Localized());

                    break;

                case 6:
                case 7:
                case 8:
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.SpellbookTome".Localized());

                    break;
            }

            OutputText("Scenes.Helspawnscene.HelspawnRead2.GrabForPages".Localized());

            switch (choice)
            {
                case 1:
                {
                OutputText("Scenes.Helspawnscene.HelspawnRead2.DullBookNestles".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Helspawnscene.HelspawnRead2.WildlifeIllustrations".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnRead2.FifthCycleDrift".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Helspawnscene.HelspawnRead2.HopSwingInstruct".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Helspawnscene.HelspawnRead2.StartEarlyMother".Localized(
                    Player.Mf("parents", "mothers")
                ));

                OutputText("Scenes.Helspawnscene.HelspawnRead2.ChaseRestOfTime".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Helspawnscene.HelspawnRead2.HarmlessIllustrated".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnRead2.HopeNoBadWords".Localized());
                }

                break;

                case 6:
                {
                if (!Player.HasStatusEffect(StatusEffectIds.KnowsBlind))
                    HelspawnReadSpells();
                else
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.SmudgyFingerprint".Localized());
                }

                break;

                case 7:
                {
                if (!Player.HasStatusEffect(StatusEffectIds.KnowsArouse))
                    HelspawnReadSpells();
                else
                {
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.SignalSitStill".Localized());

                    OutputText("Scenes.Helspawnscene.HelspawnRead2.MumbleHealedScales".Localized());
                }
                }

                break;

                case 8:
                {
                if (!Player.HasStatusEffect(StatusEffectIds.KnowsTKBlast))
                    HelspawnReadSpells();
                else
                {
                    OutputText("Scenes.Helspawnscene.HelspawnRead2.NeverGraspHover".Localized());

                    OutputText("Scenes.Helspawnscene.HelspawnRead2.BookOnHeadSqueal".Localized());

                    OutputText("Scenes.Helspawnscene.HelspawnRead2.RelentMannersLesson".Localized());
                }
                }

                break;
            }

            OutputText("Scenes.Helspawnscene.HelspawnRead2.HeliaPeeksIn".Localized(
                choice == 5 ? "and you breathe a sigh of relief that you managed to get the book away in time" : (choice < 3 ? "Common.Text.SmilingWhenSheSeesYourSleeping".Localized() : "Common.Text.ClearlySurprisedAtHowMuchYour".Localized())
            ));

            OutputText("Scenes.Helspawnscene.HelspawnRead2.PlaceholderToken".Localized());

            ReturnToCamp(1);
        }

        public void HelspawnReadSpells()
        {

            OutputText("Scenes.Helspawnscene.HelspawnReadSpells.EsotericRitualsGlaze".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnReadSpells.SorceressStartSomewhere".Localized());
        }

        public void HelSpawnGraduation()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnGrowupCounter, 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnAge, CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) + 1);

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.WakeToScream".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.DoubleTakeFigure".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 1)
                OutputText("Scenes.Helspawnscene.HelSpawnGraduation.BlackWord".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelSpawnGraduation.RedWord".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.CurledBesideHelia".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.AskMomRubbingTemple".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.GroanWhatsWrong".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.HelBabblesGiant".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.AdmitDrasticChange".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                OutputText("Scenes.Helspawnscene.HelSpawnGraduation.WolfEarsGiveaway".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.ComfortConfusedGirl".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.SlideBesideMom".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.HelWantsBabyBack".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.CupCheekHoldTight".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnGraduation.TeenHopeRaiseRight".Localized());

            ReturnToCamp(1);
        }

        public void HelspawnDiscoversBooze()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnDiscoverBooze, 1);

            OutputText("Scenes.Helspawnscene.HelspawnDiscoversBooze.NoticeHelAbsent".Localized());

            if (Player.Race != "cat-boy" && Player.Race != "cat-girl")
                OutputText("Scenes.Helspawnscene.HelspawnDiscoversBooze.CatWord".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelspawnDiscoversBooze.CatYouAreClause".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnDiscoversBooze.PeekAroundStill".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnDiscoversBooze.FlopUnderSpigot".Localized(
                ChampionRef()
            ));

            ClearButtons();

            AddButton(1, "Common.Button.ScoldHer".Localized(), ScoldHelSpawn);

            AddButton(0, "Common.Button.Encourage".Localized(), EncourageHelspawn);
        }

        public void ScoldHelSpawn()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.CrossArmsDemand".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.PickUpByScruff".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.RepeatCrossQuestion".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.DrinkExcuseLine".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.RebutMotherExample".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.ConfusedStammer".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.OfferWaterskin".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.WhineArmAround".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.WaitUntilOlder".Localized());

            OutputText("Scenes.Helspawnscene.ScoldHelSpawn.CurlBesideYou".Localized(
                ChampionRef()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) - 10);

            ReturnToCamp(1);
        }

        public void EncourageHelspawn()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.EncourageHelspawn.MoveOverOpenTap".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawn.CatchOnInvite".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawn.BuildTolerance".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawn.SlurpSwapTurns".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Sex".Localized(), EncourageSlutspawnSex);

            AddButton(0, "Common.Button.StopDrinking".Localized(), EncourageHelspawnContinued);
        }

        public void EncourageSlutspawnSex()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSex.DrunkMindSplay".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSex.StartledLoseThought".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSex.IgnoreProtestPucker".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSex.CrawlForwardGaze".Localized());

            ClearButtons();

            EncourageSlutspawnSexHarder();
        }

        public void EncourageSlutspawnSexHarder()
        {
            ClearOutput();

            if (!Player.HasCock())
            {
                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.UndressingInHaste".Localized());

                if (Player.HasTailInsteadOfLegs())
                {
                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.DizzyPelvisThrust".Localized());

                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.MoansDifferentTogether".Localized());

                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.BoozeAddledOverwhelm".Localized());

                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.GaspsCoilRelief".Localized());
                }
                else
                {
                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.InterlockedQuimCaress".Localized());

                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.TongueEntwinesYours".Localized());

                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.SoakedClitThrobs".Localized());

                    OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.OverwhelmedMoanRelease".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.ArmorBulgeUndo".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.NervousVirginMutters".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.HymenCryWhimper".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.BegsThroughBreaths".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.SlimyPreLeak".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.PushedToGroundStraddle".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.SalamanderBitesLip".Localized());

                OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexHarder.FaceGrabCummingKiss".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);
            }

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnIncest, 1);

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            ClearButtons();

            EncourageSlutspawnSexEnd();
        }

        public void EncourageSlutspawnSexEnd()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexEnd.WakingStillEmbracing".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexEnd.TeenagerCredit".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexEnd.RetchingSavedDrink".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexEnd.SarcasticPraise".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexEnd.ExplainLiquorLesson".Localized());

            OutputText("Scenes.Helspawnscene.EncourageSlutspawnSexEnd.GrumblesFlaskBeside".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 20);

            ReturnToCamp(1);
        }

        public void EncourageHelspawnContinued()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.VoiceSnapsBeyond".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.SlumpsOntoAss".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) != 1)
                OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.RedWord".Localized());
            else
                OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.BlackWord".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.BurningBrewerySmell".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.ShrillFlamingFigure".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.RetchingSavedDrink".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.SarcasticPraise".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.ExplainLiquorLesson".Localized());

            OutputText("Scenes.Helspawnscene.EncourageHelspawnContinued.GrumblesFlaskBeside".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 10);

            ReturnToCamp(1);
        }

        public void HelSpawnChoosesAFightingStyle()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.HearFightingAhead".Localized());

            if (CampScene.CompanionsCount() > 3)
                OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.FollowersClearedOut".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.SwordGuardDummy".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.ToweringBattlecryLunge".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.PickingUpPieces".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.CheerilyInspectCarnage".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.ChucklesNeedsDummy".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.FloatHelLoan".Localized());

            if (Player.MasteryLevel(new Identifier("coc", "Bow")) >= 4)
                OutputText("Scenes.Helspawnscene.HelSpawnChoosesAFightingStyle.DefendHerselfBetter".Localized());

            ClearButtons();

            if (Player.MasteryLevel(new Identifier("coc", "Bow")) >= 4)
                AddButton(2, "Scenes.Aiko.TalkArchery.ButtonBow".Localized(), Snipermanders);

            AddButton(0, "Common.Button.YouTrain".Localized(), SwordAndBoardmander);

            AddButton(1, "Common.Button.Loan".Localized(), DasBarbarimander);
        }

        public void Snipermanders()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnWeapon, "bow");

            OutputText("Scenes.Helspawnscene.Snipermanders.HelTakesBreak".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Lizard && !noFur)
                OutputText("Scenes.Helspawnscene.Snipermanders.EvenLizardLegs".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.CorrectsGripHaft".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.SlipBehindGuideArms".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.DrawsBowstringAiming".Localized());

            if (SceneInstance<RathazulScene>.Get().FollowerRathazul())
                OutputText("Scenes.Helspawnscene.Snipermanders.AlchemistRuckusGlass".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1)
                    OutputText("Scenes.Helspawnscene.Snipermanders.ValeriaYelpIrritated".Localized());
                else
                    if (SceneInstance<LatexGirlScene>.Get().LatexGooAtCamp())
                        OutputText("Scenes.Helspawnscene.Snipermanders.GenericYelpAngry".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.WhinesBitingLip".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.Snipermanders.EncourageSecondTry".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.ArrowStrikesTrue".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.LaughsSippingFlask".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.PumpsFistTriumph".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.SecondLessonKiddo".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.StanceDummyReset".Localized());

            OutputText("Scenes.Helspawnscene.Snipermanders.WarriorWorthyParents".Localized());

            ReturnToCamp(1);
        }

        public void SwordAndBoardmander()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnWeapon, "scimitar and shield");

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.KindlingStringGear".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.PracticeSwingsBlade".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.ResumeSparringStance".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.LungeWarcryThroat".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.HoldStillProtest".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.RepeatedSigh".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.CautiousApproachBlade".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.SidelinesCheerMug".Localized(
                Player.Mf("him", "her")
            ));

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.BracesBreathAttack".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.StumblesToKnees".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.RuffleComfortFeet".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.NickShieldWince".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.GrinsHuggingThanks".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.HelTakesDummySword".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.HoursDrillingStance".Localized());

            OutputText("Scenes.Helspawnscene.SwordAndBoardmander.NightlyDrillWarrior".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) - 10);

            ReturnToCamp(1);
        }

        public void DasBarbarimander()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnWeapon, "scimitar");

            if (Player.Gems >= 200)
            {
                OutputText("Scenes.Helspawnscene.DasBarbarimander.GemsForHelia".Localized());

                OutputText("Scenes.Helspawnscene.DasBarbarimander.GrinsFollowingMother".Localized());

                Player.Gems -= 200;
            }
            else
            {
                OutputText("Scenes.Helspawnscene.DasBarbarimander.EmptyPocketsBroke".Localized());

                OutputText("Scenes.Helspawnscene.DasBarbarimander.SilenceMinotaurThreat".Localized());

                OutputText("Scenes.Helspawnscene.DasBarbarimander.FollowsAlongMother".Localized());

                OutputText("Scenes.Helspawnscene.DasBarbarimander.CannotImagineOutcome".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 10);
            }

            ReturnToCamp(1);
        }

        public void HelspawnIsASlut()
        {

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnFuckInterruptus, 1);

            OutputText("Scenes.Helspawnscene.HelspawnIsASlut.MovementInCamp".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnIsASlut.ScaledFootGiggle".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Helspawnscene.HelspawnIsASlut.ClutchWeapon".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelspawnIsASlut.BareClawsFists".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnIsASlut.IntruderPassesDefend".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnIsASlut.CreepCloserPeek".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnIsASlut.StopWhoreDaughter".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.StopThem".Localized(), HelSpawnStopFucking);

            AddButton(1, "Common.Button.Join".Localized(), HelspawnJoin);

            AddButton(2, "Common.Button.DoNothing".Localized(), HelspawnDoNothing);
        }

        public void HelspawnDoNothing()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelspawnDoNothing.ReturnToBed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 10);

            Go<CampScene>();
        }

        public void HelspawnJoin()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelspawnJoin.GuidanceHoldsValue".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.ThoughtYouSlept".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.GirlishBoySpeaks".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.DeclareDeservesBetter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) >= 40)
            {
                OutputText("Scenes.Helspawnscene.HelspawnJoin.InterjectsThenKissed".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnJoin.CadenceSuggestsInterest".Localized());
            }
            else
                OutputText("Scenes.Helspawnscene.HelspawnJoin.NervousWeirdProtest".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.LongingKissLips".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.GlanceAtLoverboy".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.SlinkBetweenLegs".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnJoin.GleeBoyfriendHope".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnJoinedSlut, true);

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) + 10);

            Go<CampScene>();
        }

        public void HelSpawnStopFucking()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.StepFromShadowsGrab".Localized());

            if (Player.Tallness >= 90)
                OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.UpWord".Localized());
            else
                if (Player.Tallness <= 72)
                    OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.DownWord".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.ShrillCryTumble".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.AccusingFingerQuestion".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.AlexWord".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.SpiderMumblesGirlish".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.FirmlyLeaveAlone".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.StammeredProtest".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.TossSpiderOut".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.StammerToMom".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.NoBringingMen".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == -1)
                OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.WhoreLikeMother".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1)
                OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.WontRaiseWhore".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.ShouldReserveSelf".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnStopFucking.ChastenedChasteTime".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnPersonality, CoC.Instance.Flags.Get(GameFlagIds.HelspawnPersonality) - 10);

            ReturnToCamp(1);
        }

        public void HelspawnAllGrownUp()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.HelspawnAllGrownUp.MorningGlanceHelspawn".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnAllGrownUp.SadGrownUp".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnAllGrownUp.FollowerAddedNotice".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnAge, 3);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnGrowupCounter, 0);

            ReturnToCamp(1);
        }

        public void HelspawnsCampLines()
        {

            if (CampSleep.IsForceSleepTime())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helspawn")
                    OutputText("Scenes.Helspawnscene.HelspawnsCampLines.WaitsOutsideCabin".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnsCampLines.SleepsNearMother".Localized());
            }
            else
                OutputText("Scenes.Helspawnscene.HelspawnsCampLines.OwnBedrollSet".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnsCampLines.ParagraphBreak".Localized());
        }

        public void HelspawnsMainMenu()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnsMainMenu.CallHelspawnOver".Localized());

            if (Player.Tallness <= 72)
                OutputText("Scenes.Helspawnscene.HelspawnsMainMenu.LiftAndClutchTight".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelspawnsMainMenu.ArmsAroundHug".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnsMainMenu.RuffleHairAskMinute".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnsMainMenu.SureWhatsUpReply".Localized(
                ChampionRef()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), HelSpawnsAppearanceScreen);

            AddButton(1, "Common.Button.Talk".Localized(), TalkToHelspawn);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 1)
            {
                AddButton(2, "Common.Button.Sex".Localized(), HelSpawnSexMenu);

                AddButton(6, "Common.Button.Kiss".Localized(), HelspawnSmooch);
            }

            AddRowButton(0, "Spar", SparHelspawn);

            AddButton(5, "Common.Button.Hug".Localized(), HugHelspawn);

            AddRowButton(1, "Headpat", HelspawnHeadpats);

            AddButton(9, "Sleep " + (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helspawn" ? "Alone" : (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "salamanders" ? "Common.Text.Without".Localized() : "Common.Text.With".Localized())), SleepWithHelspawn);

            if ((CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Helia" || CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "salamanders") && CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 0)
                AddButton(9, "Common.Button.SleepWith".Localized(), null);

            AddRowButton(2, "Fishing", HelspawnFishing);

            AddButton(14, "Common.Button.Back".Localized(), CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 1 ? () => Scene.Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()) : () => Scene.Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void HelSpawnSexMenu()
        {

            ClearButtons();

            if (silly)
                AddNextButton("Common.Button.Marathon".Localized(), SexHelspawn);

            if (HelspawnChaste())
            {
                AddNextButton("Common.Button.Lovemaking".Localized(), HelspawnLovemaking);

                AddNextButton("Common.Button.Facesitting".Localized(), HelspawnFacesitting);
            }
            else
                AddNextButton("Common.Button.FuckHer".Localized(), HelspawnSlutsex);

            AddNextButton("Common.Button.Footjob".Localized(), HelspawnFootjob);

            AddButton(14, "Common.Button.Back".Localized(), HelspawnsMainMenu);
        }

        public void HugHelspawn()
        {
            ClearOutput();

            if (Player.Tallness <= 72)
            {
                OutputText("Scenes.Helspawnscene.HugHelspawn.PullDownHug".Localized());

                OutputText("Scenes.Helspawnscene.HugHelspawn.LoveKissCheek".Localized(
                    ChampionRef()
                ));
            }
            else
            {
                OutputText("Scenes.Helspawnscene.HugHelspawn.GrabAndHug".Localized());

                OutputText("Scenes.Helspawnscene.HugHelspawn.LoveKissCheekAgain".Localized(
                    ChampionRef()
                ));
            }

            ReturnToCamp(1);
        }

        public void TalkToHelspawn()
        {
            ClearOutput();

            var temp = Random.Shared.Next(6);

            if (CoC.Instance.Flags.Get(GameFlagIds.HadFirstHelspawnTalk) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.HadFirstHelspawnTalk, CoC.Instance.Flags.Get(GameFlagIds.HadFirstHelspawnTalk) + 1);

                OutputText("Scenes.Helspawnscene.TalkToHelspawn.SitDownSeriousTalk".Localized());

                OutputText("Scenes.Helspawnscene.TalkToHelspawn.AwkwardApologySmile".Localized());

                OutputText("Scenes.Helspawnscene.TalkToHelspawn.LearnAboutSpiderBoy".Localized());

                OutputText("Scenes.Helspawnscene.TalkToHelspawn.ThinkBeforeApproach".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.StopFucking".Localized(), DontFuckAlex);

                AddButton(1, "Common.Button.HerBoyfriend".Localized(), HelSpawnBoyfriend);

                AddButton(2, "Common.Button.Incest".Localized(), IncestWithHelspawn);
            }
            else
                if (temp <= 0 && SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                {
                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.ComeSeeMyWork".Localized(
                        ChampionRef()
                    ));

                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.GrabByArmorPull".Localized());

                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.TryItAuntRecipe".Localized(
                        ChampionRef()
                    ));

                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.DragonessLaughsFoot".Localized());

                    if (KihaFollowerScene.Save.GetInt("badCookingTasted", 0) == 0)
                        KihaFollowerScene.Save.SetInt("badCookingTasted", 1);

                    ClearButtons();

                    AddButton(0, "Common.Button.OhGodEw".Localized(), OhGodEwKihaAndHelspawnSuckAtCooking);

                    AddButton(1, "Common.Button.UmYum".Localized(), UmYum);
                }
                else
                    if (temp <= 1 && SceneInstance<RathazulScene>.Get().FollowerRathazul())
                    {
                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.RaggedVoiceScolds".Localized());

                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.RathazulShufflesIn".Localized(
                            ChampionRef()
                        ));

                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.FollowToLaboratory".Localized());

                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.CheckingHerGrowth".Localized());

                        if (SceneInstance<AmilyScene>.Get().AmilyFollowerPure())
                            OutputText("Scenes.Helspawnscene.TalkToHelspawn.AmilySolutionMakeshift".Localized());

                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.LegionsOfChildren".Localized());

                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.FinishedGrowingNoSign".Localized());

                        ReturnToCamp(1);
                    }
                    else
                        if (temp <= 2 && SceneInstance<MilkWaifuScene>.Get().MilkSlave() && CoC.Instance.Flags.Get(GameFlagIds.MilkSize) > 0)
                        {
                            OutputText("Scenes.Helspawnscene.TalkToHelspawn.SeenBathGirlAnywhere".Localized(
                                ChampionRef()
                            ));

                            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 1)
                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.JustAWobbleRide".Localized());
                            else
                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.DoubtSheWouldHave".Localized());

                            OutputText("Scenes.Helspawnscene.TalkToHelspawn.WouldntHurtToCheck".Localized());

                            OutputText("Scenes.Helspawnscene.TalkToHelspawn.MilkHerLaterGrin".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Milkherlater".Localized(), HelSpawnSureMilkHerLater);

                            AddButton(1, "Common.Button.Milkhernow".Localized(), HelSpawnMilkHerNow);
                        }
                        else
                            if (temp <= 3 && SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                            {
                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.NewGuitarInvite".Localized(
                                    ChampionRef()
                                ));

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.AcceptGuitarInvite".Localized());

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.SoftMelodicTune".Localized());

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.IsabellaJoinsSong".Localized());

                                if (Player.Race != "human")
                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.YouSufferedToo".Localized());

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.FinalChorusDuet".Localized());

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.IsabellaSongOpening".Localized());

                                if (!(CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.GermanSehrGut".Localized());
                                else
                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.VeryGoodLine".Localized());

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.IsabellaBigHug".Localized());

                                OutputText("Scenes.Helspawnscene.TalkToHelspawn.LoverSmilesApproach".Localized());

                                ReturnToCamp(1);
                            }
                            else
                                if (temp <= 4)
                                {
                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.ReminisceIgnamDays".Localized());

                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.AdultInYourCustoms".Localized());

                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.PiggybackIgnore".Localized());

                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.NonsenseIsNormal".Localized(
                                        Player.Mf("", "other ")
                                    ));

                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.HeliaLaughsSees".Localized());

                                    OutputText("Scenes.Helspawnscene.TalkToHelspawn.MomTellDadAdult".Localized());

                                    if (Player.IsTaur())
                                    {
                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.HeliaJoinsRide".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.ProperGallop".Localized());
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.HeliaNeverTooBig".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.DaughterGroans".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.AwkwardLeanOn".Localized());
                                    }

                                    HelSpawnPiggyback();
                                }
                                else
                                    if (temp <= 5 && SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                                    {
                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.SparTalkWithHelia".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.ShouldraDeclaration".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.DaughterPrattles".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.SparringChallengeTaunt".Localized());

                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.JustTrustMeIronic".Localized());

                                        ClearButtons();

                                        AddNextButton("Common.Button.Spar".Localized(), HelspawnSparPrank);

                                        AddNextButton("Common.Button.JustTalk".Localized(), TalkToHelspawnJustTalk);
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Helspawnscene.TalkToHelspawn.QuietMomentChat".Localized());

                                        ReturnToCamp(1);
                                    }
        }

        public void TalkToHelspawnJustTalk()
        {

            OutputText("Scenes.Helspawnscene.TalkToHelspawnJustTalk.IgnoreGhostFocusHer".Localized());

            ReturnToCamp(1);
        }

        public void HelSpawnBoyfriend()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelSpawnBoyfriend.BiggestProblemSecrecy".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnBoyfriend.BrightensAboutAlex".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnBoyfriend.MaybeKeepSeeingHim".Localized());

            ReturnToCamp(1);
        }

        public void DontFuckAlex()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.DontFuckAlex.StrokeHairAdvise".Localized());

            if (HelspawnSlutty())
            {
                OutputText("Scenes.Helspawnscene.DontFuckAlex.PrefersMomsWay".Localized(
                    ChampionRef()
                ));

                OutputText("Scenes.Helspawnscene.DontFuckAlex.SheStepsAway".Localized(
                    ChampionRef()
                ));

                OutputText("Scenes.Helspawnscene.DontFuckAlex.HorribleMistake".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.DontFuckAlex.NodsAcceptsAdvice".Localized(
                    ChampionRef()
                ));

                OutputText("Scenes.Helspawnscene.DontFuckAlex.CatchesHerselfSigh".Localized());
            }

            ReturnToCamp(1);
        }

        public void IncestWithHelspawn()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.IncestWithHelspawn.CupHerCheek".Localized());

            OutputText("Scenes.Helspawnscene.IncestWithHelspawn.ConveyLoveTension".Localized());

            OutputText("Scenes.Helspawnscene.IncestWithHelspawn.MovedToLoversMenu".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Sensitivity / 10 + 5)));

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnIncest, 1);

            Save.SetInt("timeTalkedIncest", View.Time.Days);

            ReturnToCamp(1);
        }

        public void OhGodEwKihaAndHelspawnSuckAtCooking()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.SpewStewGround".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.GrowlsSpoonsTaste".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.YelpsAtCollapse".Localized(
                ChampionRef()
            ));

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.UnconsciousDumpCauldron".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.CleanCauldronLesson".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.SpecialSpiceBlend".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.DragonessHissesTemple".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.LieAboutFall".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.KihaSnatchesBowl".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.CrossedFingersLie".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.IncredulousLookSip".Localized());

            OutputText("Scenes.Helspawnscene.OhGodEwKihaAndHelspawnSuckAtCooking.ShakeHeadEatLunch".Localized());

            Player.RefillHunger(25);

            ReturnToCamp(1);
        }

        public void UmYum()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.UmYum.UmYumReply".Localized());

            if (Player.Hunger > 25)
            {
                OutputText("Scenes.Helspawnscene.UmYum.SetBowlPolitely".Localized());

                OutputText("Scenes.Helspawnscene.UmYum.WanderOffToHurl".Localized());

                Player.RefillHunger(5);
            }
            else
            {
                OutputText("Scenes.Helspawnscene.UmYum.GrowlingStomachEat".Localized());

                Player.RefillHunger(25);

                OutputText("Scenes.Helspawnscene.UmYum.NauseousStew".Localized());

                if (Random.Shared.Next(3) == 0)
                {
                    OutputText("Scenes.Helspawnscene.UmYum.RushBehindBushes".Localized());

                    Player.HealthChange(-Player.MaxHealth / 4);

                    Player.DamageHunger(Random.Shared.Next(10) + 15);

                    Player.ApplyStats(new DynStat(GameStats.Libido, -10f));

                    Player.ApplyStats(new DynStat(GameStats.Lust, -100f));
                }

                OutputText("Scenes.Helspawnscene.UmYum.FinishedBowlSet".Localized());
            }

            OutputText("Scenes.Helspawnscene.UmYum.TellTaleGroans".Localized());

            OutputText("Scenes.Helspawnscene.UmYum.MaybeShouldCook".Localized());

            ReturnToCamp(1);
        }

        public void HelSpawnSureMilkHerLater()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelSpawnSureMilkHerLater.LetHerSleepMilk".Localized());

            ReturnToCamp(1);
        }

        public void HelSpawnMilkHerNow()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.PushToChores".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.RebelliousHuffClamber".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.YawningRiseKnees".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MilkSize) == 2)
                OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.UnfastenShift".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.SuckleOffer".Localized());

            if (HelspawnSlutty())
                OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.SalamanderEagerGrin".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.NotIntoThat".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnMilkHerNow.PairGetsAlong".Localized());

            ReturnToCamp(1);
        }

        public void HelSpawnPiggyback()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                OutputText("Scenes.Helspawnscene.HelSpawnPiggyback.RideLetsGirlsOff".Localized());

                OutputText("Scenes.Helspawnscene.HelSpawnPiggyback.SighMadeMeHappy".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.HelSpawnPiggyback.FamilyTowerTires".Localized());

                OutputText("Scenes.Helspawnscene.HelSpawnPiggyback.DaughterDeepSigh".Localized());
            }

            OutputText("Scenes.Helspawnscene.HelSpawnPiggyback.WantsItRepeated".Localized());

            OutputText("Scenes.Helspawnscene.HelSpawnPiggyback.HeliaInterjectsHell".Localized());

            ReturnToCamp(1);
        }

        public void HelspawnSparPrank()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.PlayAlongWithGhost".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.BattlePositionAssumed".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.ReciprocateFirstMove".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.ScramblesExaminesSelf".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.LeadsTheCharge".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.UnfairMessingAccusation".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.GoldenEyesShouldra".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank.ProcessesNuance".Localized());

            HelspawnSparPrank2();
        }

        public void HelspawnSparPrank2()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank2.PrankBecomesTraining".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank2.FightingWindsDown".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank2.RelayThoughtsSpecter".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank2.CallsForAttention".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank2.SitsCloserThanks".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSparPrank2.InstinctsEmbrace".Localized());

            ReturnToCamp(1);
        }

        public void SparHelspawn()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.SparHelspawn.AskForPractice".Localized());

            if (HelspawnSlutty())
                OutputText("Scenes.Helspawnscene.SparHelspawn.GrinReadyWeapon".Localized(
                    Player.Mf("man", "lady")
                ));
            else
                OutputText("Scenes.Helspawnscene.SparHelspawn.StepBackGoEasy".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "helspawn")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void LoseSparringToDaughter()
        {

            if (HelspawnSlutty())
            {
                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.StumbleBackAgain".Localized());

                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.SuccumbingToYourLusts".Localized());
                else
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.UnableToWithstandHail".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.SweepsYourLegsOut".Localized());

                if (!Player.Weapon.IsAttached())
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.WeaponTumblingFree".Localized());
                else
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.UnableToClenchFists".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.StraddlingPinsYou".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.SurveyingHerConquest".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.BothBreakSnickering".Localized(
                    ChampionRef()
                ));

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.RuffleHerHair".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.GrowUpFast".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.StumbleBackAgain".Localized());

                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.SuccumbToLustsAgain".Localized());
                else
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.HailOfAttacksAgain".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.CatchesYourArm".Localized(
                    ChampionRef()
                ));

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.AsksWentEasy".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.GrinsAndKissesCheek".Localized(
                    ChampionRef()
                ));

                if (Player.Health < 1)
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.RecoverFromAssKicking".Localized());
                else
                    OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.DunkYourHead".Localized());

                OutputText("Scenes.Helspawnscene.LoseSparringToDaughter.SentencePeriod".Localized());
            }

            CombatScene.CleanupAfterCombat();
        }

        public void BeatUpYourDaughter()
        {
            ClearOutput();

            if (HelspawnSlutty() && CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.MoansAndSlumps".Localized());

                OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.ShakeHeadAndPush".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 0)
                    OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.WashHandsOfDefeat".Localized());
                else
                {
                    OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.PatHerExpansiveRear".Localized());

                    if (!Player.HasCock())
                        OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.SexWord".Localized());
                    else
                        OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.YourCockWord".Localized());

                    OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.MaybeTendToHer".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.DropsWeaponAndYields".Localized());

                OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.ApprovingNodLowerWeapon".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.RubbingHerBruises".Localized());
                else
                    OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.BlushingAfterDisplay".Localized());

                OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.ChuckleAndRuffleHair".Localized());

                OutputText("Scenes.Helspawnscene.BeatUpYourDaughter.GroansAboutFood".Localized());
            }

            CombatScene.CleanupAfterCombat();
        }

        public void MaiVisitsHerKids()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.NoticeFamilyAtFire".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.HelWavesYouOver".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.MaiDemureGreeting".Localized());

            if (HelspawnChaste())
                OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.LeansOnMaiLeg".Localized());
            else
                OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.PassesFlaskAround".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.HelAsksMaiOpinion".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.MaiKissesSalamander".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.SalamanderGuardJoke".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.MockSighAnotherChild".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.EnoughOfAHandful".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.WhinesCrossingArms".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.ParentsLaughMaiKneels".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.GoldChainGift".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.MaiThanksYouSoftly".Localized());

            OutputText("Scenes.Helspawnscene.MaiVisitsHerKids.PromiseThenMaiGoes".Localized());

            ReturnToCamp(1);
        }

        public void SpiderBrosGift()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.SpiderBroGift, 1);

            OutputText("Scenes.Helspawnscene.SpiderBrosGift.PrancesWithScarf".Localized());

            OutputText("Scenes.Helspawnscene.SpiderBrosGift.AskAboutAccessory".Localized());

            OutputText("Scenes.Helspawnscene.SpiderBrosGift.HugsScarfAndGrins".Localized());

            if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                OutputText("Scenes.Helspawnscene.SpiderBrosGift.IsabellaKnittedIt".Localized());

            OutputText("Scenes.Helspawnscene.SpiderBrosGift.SentencePeriodSayEnd".Localized());

            OutputText("Scenes.Helspawnscene.SpiderBrosGift.FeelsSpiderSilk".Localized());

            OutputText("Scenes.Helspawnscene.SpiderBrosGift.IntruderCouldHaveKilled".Localized());

            ReturnToCamp(1);
        }

        public void HakonAndKiriComeVisit()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HakonAndKiriVisit, 1);

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.HeliaFussesAround".Localized());

            if (CampScene.CompanionsCount() > 1)
                OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.EveryoneElseSuffers".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.PolishingGiantStill".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.HelGreetsNervously".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.AskWhatGoingOn".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.PopAndKiriComing".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.AlwaysLooksLikeThis".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.LikesLivingInDump".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.RagShovedInHands".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.GleefulCryFromEdge".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.FluttersOverToKiss".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.HakonHandshakeGrip".Localized(
                Player.Mf("son", "girl")
            ));

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.HakonProudOfGrowth".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.ShowTheOldScales".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.DaresHerToHit".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.SureWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnWeapon) == "scimitar and shield")
                OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.PlayerTaughtMeFight".Localized(
                    ChampionRef()
                ));
            else
                OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.MomTaughtMeFight".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.StillBelowMom".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.TooWord".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.KiriAvoidsFatherGaze".Localized());

            OutputText("Scenes.Helspawnscene.HakonAndKiriComeVisit.HelTeasesHalfHarpy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sure".Localized(), GoHuntingBitches);

            AddButton(1, "Common.Button.MaybeNot".Localized(), NoHuntingBitches);
        }

        public void GoHuntingBitches()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.GoHuntingBitches.VoiceYourAgreement".Localized());

            OutputText("Scenes.Helspawnscene.GoHuntingBitches.HelShoutsNoMercy".Localized());

            OutputText("Scenes.Helspawnscene.GoHuntingBitches.SettleDownToDinner".Localized());

            Player.RefillHunger(30, false);

            OutputText("Scenes.Helspawnscene.GoHuntingBitches.GuestsPromiseReturn".Localized());

            ReturnToCamp(1);
        }

        public void NoHuntingBitches()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helspawnscene.NoHuntingBitches.DecideToPass".Localized());

            OutputText("Scenes.Helspawnscene.NoHuntingBitches.HelHopesYouReconsider".Localized());

            OutputText("Scenes.Helspawnscene.NoHuntingBitches.SitDownForDinner".Localized());

            Player.RefillHunger(30, false);

            OutputText("Scenes.Helspawnscene.NoHuntingBitches.GuestsWaveGoodbye".Localized());

            ReturnToCamp(1);
        }

        public void SexHelspawn()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.SexHelspawn.QualityTimeTogether".Localized());

            if (Player.Tallness < 72)
            {
                OutputText("Scenes.Helspawnscene.SexHelspawn.CupHerCheek".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                    OutputText("Scenes.Helspawnscene.SexHelspawn.GoldenWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                        OutputText("Scenes.Helspawnscene.SexHelspawn.BrightGreenWord".Localized());
                    else
                        OutputText("Scenes.Helspawnscene.SexHelspawn.CrimsonWord".Localized());

                OutputText("Scenes.Helspawnscene.SexHelspawn.EyesLitLikeStars".Localized());
            }
            else
                if (Player.Tallness >= 72 && Player.Tallness < 96)
                    OutputText("Scenes.Helspawnscene.SexHelspawn.StandEyeToEye".Localized());
                else
                    if (Player.Tallness >= 96)
                        OutputText("Scenes.Helspawnscene.SexHelspawn.WrapHerInArms".Localized());

            OutputText("Scenes.Helspawnscene.SexHelspawn.AskAfterTheKiss".Localized());

            OutputText("Scenes.Helspawnscene.SexHelspawn.SheWouldLikeThat".Localized());

            OutputText("Scenes.Helspawnscene.SexHelspawn.WatchesYouDisrobe".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Helspawnscene.SexHelspawn.HerTurnToStrip".Localized());

            if (HelspawnSlutty())
            {
                OutputText("Scenes.Helspawnscene.SexHelspawn.UnsnapsBikiniTop".Localized());

                OutputText("Scenes.Helspawnscene.SexHelspawn.RubAndTeaseHoles".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.SexHelspawn.UnbuttonsBlueBlouse".Localized());

                OutputText("Scenes.Helspawnscene.SexHelspawn.GaspsMassageBottom".Localized());
            }

            OutputText("Scenes.Helspawnscene.SexHelspawn.StepsAwayFromKiss".Localized());

            SexOralHelspawn();
        }

        public void SexOralHelspawn()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            int y;

            int z;

            if (Player.HasVagina())
            {
                OutputText("Scenes.Helspawnscene.SexOralHelspawn.ThighsPushedAside".Localized());

                OutputText("Scenes.Helspawnscene.SexOralHelspawn.SingleSmoothLick".Localized());
            }

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Helspawnscene.SexOralHelspawn.CocksMovedAside".Localized());

                if (Player.Balls == 1)
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.PullingOrbUp".Localized());
                else
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.PullingOrbsUp".Localized());

                OutputText("Scenes.Helspawnscene.SexOralHelspawn.BottledUpCum".Localized());

                if (Player.Balls == 1)
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.BallWord".Localized());
                else
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.BallsWord".Localized());

                OutputText("Scenes.Helspawnscene.SexOralHelspawn.HotTalentedTongue".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0)
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.GoldenWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2)
                        OutputText("Scenes.Helspawnscene.SexOralHelspawn.BrightGreenWord".Localized());
                    else
                        OutputText("Scenes.Helspawnscene.SexOralHelspawn.CrimsonWord".Localized());

                OutputText("Scenes.Helspawnscene.SexOralHelspawn.EyesLockingOnYou".Localized());
            }

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Helspawnscene.SexOralHelspawn.TongueAcrossBelly".Localized(
                    Player.CockDescript(),

                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                OutputText("Scenes.Helspawnscene.SexOralHelspawn.RubbingHerTongue".Localized(
                    Player.CockDescript(),

                    Player.CockDescript(),

                    Player.CockDescript()
                ));
            }
            else
                if (Player.Cocks.Count == 2)
                {
                    y = Player.BiggestCockIndex2();

                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.GrinningHoldOnCock".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y),

                        Player.CockDescript(x),

                        Player.CockDescript(y)
                    ));

                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.GroanThrustingMouth".Localized());
                }
                else
                    if (Player.Cocks.Count >= 3)
                    {
                        y = Player.BiggestCockIndex2();

                        z = Player.BiggestCockIndex3();

                        OutputText("Scenes.Helspawnscene.SexOralHelspawn.PullsCockIntoLips".Localized(
                            Player.CockDescript(x),

                            x + 1,

                            Player.CockDescript(y),

                            Player.CockDescript(z),

                            Player.CockDescript()
                        ));
                    }

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.LovesYouSoMuch".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 27)
                OutputText("Scenes.Helspawnscene.SexOralHelspawn.HowlingDeepThroat".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockHead(x),

                    Player.CockDescript(x)
                ));
            else
                if (Player.CockArea(x) < 36 && Player.CockArea(x) > 27)
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.HipsBuckSwallow".Localized(
                        Player.CockHead(x),

                        Player.CockDescript(x)
                    ));
                else
                    if (Player.CockArea(x) >= 36)
                        OutputText("Scenes.Helspawnscene.SexOralHelspawn.StrugglingToFit".Localized(
                            Player.CockHead(x),

                            Player.CockDescript(x)
                        ));

            if (Player.HasKnot(x))
                OutputText("Scenes.Helspawnscene.SexOralHelspawn.KnotTenderKiss".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (Player.Cocks[x].Id == BodyPartTypes.Cat)
                    OutputText("Scenes.Helspawnscene.SexOralHelspawn.TwirlingCatNubs".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if (Player.Cocks[x].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Helspawnscene.SexOralHelspawn.HandAcrossSegments".Localized(
                            Player.CockDescript(x),

                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.Cocks[x].Id == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Helspawnscene.SexOralHelspawn.LivingPeckerWorked".Localized(
                                Player.CockHead(x),

                                Player.CockDescript(x)
                            ));
                        else
                            OutputText("Scenes.Helspawnscene.SexOralHelspawn.DroolingExploration".Localized(
                                Player.CockDescript(x),

                                Player.CockDescript(x),

                                Player.CockDescript(x)
                            ));

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.NearClimaxRespite".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.StayingHereWithYou".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.KneelingPatiently".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.NuzzlingHerNeck".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.OtherThingsInMind".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.TongueOnInnerThigh".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.FloodingAnticipation".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.PleaseSheWhines".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.LegsPressedBack".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.MassagingNetherLips".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.WantsToPlay".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.RedoubledSucking".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.ScreamingPossessed".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.NotDoneYet".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.SpinningHerAround".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.GigglingWeirdFeeling".Localized());

            OutputText("Scenes.Helspawnscene.SexOralHelspawn.GreedyAlternation".Localized());

            SexDoggyHelspawn();
        }

        public void SexDoggyHelspawn()
        {
            ClearOutput();

            bool cameBeforeRiding = false;

            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.KneelingBetweenCheeks".Localized());

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.AimingLeakingHead".Localized(
                    Player.CockDescript()
                ));

                if (Player.BiggestCockLength() <= 8)
                {
                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.SlideIntoEntrance".Localized());

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.LookingOverShoulder".Localized());

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.LeanInForKiss".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                    if (Player.BiggestCockLength() > 8 && Player.BiggestCockLength() <= 12)
                    {
                        OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.PressIntoEntrance".Localized(
                            Player.CockHead(),

                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.FlutteringEcstasy".Localized());

                        OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.LeanInTongueKiss".Localized(
                            Player.CockDescript()
                        ));
                    }
                    else
                        if (Player.BiggestCockLength() > 12 && Player.BiggestCockLength() <= 18)
                        {
                            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.WorkingInGently".Localized(
                                Player.CockHead(),

                                Player.CockDescript(),

                                Player.CockDescript()
                            ));

                            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.ScreamingFuckMe".Localized());

                            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.LeanInKissAccepted".Localized(
                                Player.CockDescript()
                            ));
                        }
                        else
                            if (Player.BiggestCockLength() > 18)
                            {
                                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.TunnelingVaginalSleeve".Localized(
                                    Player.CockDescript(),

                                    Player.CockHead(),

                                    Player.CockDescript(),

                                    Player.CockDescript()
                                ));

                                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.WaitingForAdjustment".Localized(
                                    Player.CockDescript()
                                ));

                                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.PracticallyScreaming".Localized());

                                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.LeanInLongKiss".Localized(
                                    Player.CockDescript()
                                ));
                            }

                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.PostOrgasmKiss".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnHadsex) == 0)
                {
                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.CuriosityPeaks".Localized(
                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.BreathlessPermission".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                {
                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.RememberingFirstTime".Localized());

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.SlyGrinRequest".Localized());
                }

                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.TailPegChoice".Localized());
            }
            else
                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.RubbingLadyJuice".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.PressingRearEntrance".Localized(
                        Player.CockDescript(),

                        Player.CockHead()
                    ));

                    if (Player.BiggestCockLength() <= 10)
                        OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.RearPassageGrin".Localized(
                            Player.CockDescript(),

                            Player.CockDescript(),

                            Player.CockDescript()
                        ));
                    else
                        if (Player.BiggestCockLength() > 10 && Player.BiggestCockLength() <= 18)
                            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.TunnelingRearPassage".Localized(
                                Player.CockDescript(),

                                Player.CockDescript()
                            ));
                        else
                            if (Player.BiggestCockLength() > 18)
                                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.InvadingTailhole".Localized(
                                    Player.CockHead(),

                                    Player.CockHead(),

                                    Player.CockDescript()
                                ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.ReachingAroundInstead".Localized(
                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.EnjoyingTheView".Localized(
                        Player.CockDescript(),

                        Player.CockDescript(),

                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    if (Player.BiggestCockLength() <= 8)
                        OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.VaginalOpeningPush".Localized(
                            Player.CockDescript()
                        ));
                    else
                        if (Player.BiggestCockLength() > 8 && Player.BiggestCockLength() <= 12)
                            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.VaginalWallsDeep".Localized());
                        else
                            if (Player.BiggestCockLength() > 12)
                                OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.StretchingVaginalWalls".Localized(
                                    Player.CockDescript(),

                                    Player.CockDescript()
                                ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.BothHolesEmbedded".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.SlowDualPenetration".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.PickingUpPace".Localized());

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.TremblingThroughClimax".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.HeadBackEjaculation".Localized());

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.CumPoursInside".Localized());

                    if (Player.CumQ() < 800)
                        OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.CocksPopOut".Localized());
                    else
                        if (Player.CumQ() >= 800)
                            OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.TwitchingCocksOut".Localized());

                    OutputText("Scenes.Helspawnscene.SexDoggyHelspawn.CatchingBreathOnRock".Localized());

                    cameBeforeRiding = true;
                }

            ClearButtons();

            AddButton(0, "Common.Button.Anal".Localized(), SexAnalHelspawn);

            AddButton(1, "Common.Button.Riding".Localized(), () => SexRidingHelspawn(cameBeforeRiding));
        }

        public void SexAnalHelspawn()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnHadsex) == 0)
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.IntriguingIdea".Localized(
                    Player.CockDescript()
                ));
            else
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.AnsweringWithoutWords".Localized(
                    Player.CockDescript()
                ));

            if (Player.BiggestCockLength() <= 10)
            {
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.SmoothAnalEntry".Localized());

                if (Player.Anal.Looseness <= 2)
                {
                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.StrangerToAnal".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.InexperienceQuestion".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.TailSlippingDeeper".Localized(
                        Player.AssholeDescript(),

                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.FoundItTriumph".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                    if (4 > Player.Anal.Looseness && Player.Anal.Looseness > 2)
                        OutputText("Scenes.Helspawnscene.SexAnalHelspawn.ModerateAnalPace".Localized(
                            Player.CockDescript(),

                            Player.CockDescript()
                        ));
                    else
                        if (Player.Anal.Looseness >= 4)
                            OutputText("Scenes.Helspawnscene.SexAnalHelspawn.ExpertHardPace".Localized(
                                Player.AssholeDescript(),

                                Player.CockDescript()
                            ));
            }

            if (Player.BiggestCockLength() > 10 && Player.BiggestCockLength() <= 18)
            {
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.TightAnalRing".Localized(
                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                if (Player.Anal.Looseness <= 2)
                {
                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.DeepPenetrationGrunt".Localized(
                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.LaughingAtStillness".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.ShakingHeadAsking".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.BreatheAndRelax".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.EngulfingSlowPush".Localized(
                        Player.CockDescript(),

                        Player.AssholeDescript(),

                        Player.CockDescript(),

                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.RoughTailPegging".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                    if (4 > Player.Anal.Looseness && Player.Anal.Looseness > 2)
                        OutputText("Scenes.Helspawnscene.SexAnalHelspawn.SlowMatchingPace".Localized(
                            Player.AssholeDescript(),

                            Player.CockDescript()
                        ));
                    else
                        if (Player.Anal.Looseness >= 4)
                            OutputText("Scenes.Helspawnscene.SexAnalHelspawn.HardAnalPounding".Localized(
                                Player.CockDescript()
                            ));
            }

            if (Player.BiggestCockLength() > 18)
            {
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.SharedEntryGroan".Localized(
                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                if (Player.Anal.Looseness <= 2)
                {
                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.TailTooDeep".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.SlightGrimaceAdvice".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.AnotherTailSurge".Localized());

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.RidingAndPegging".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));
                }
                else
                    if (4 > Player.Anal.Looseness && Player.Anal.Looseness > 2)
                        OutputText("Scenes.Helspawnscene.SexAnalHelspawn.TakingItEasy".Localized(
                            Player.CockDescript()
                        ));
                    else
                        if (Player.Anal.Looseness >= 4)
                            OutputText("Scenes.Helspawnscene.SexAnalHelspawn.AnalCocksleeveFullness".Localized(
                                Player.CockDescript()
                            ));
            }

            var x = Player.BiggestCockIndex();

            if (Player.HasKnot(x))
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.AvoidingTheKnot".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.CumQ() < 200)
            {
                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.GroaningHiltUnload".Localized(
                    Player.CockDescript(),

                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                OutputText("Scenes.Helspawnscene.SexAnalHelspawn.ScalyLegsWrapped".Localized(
                    Player.CockDescript()
                ));
            }
            else
                if (200 <= Player.CumQ() && Player.CumQ() < 400)
                {
                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.AnimalGrowlHilt".Localized(
                        Player.CockDescript(),

                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexAnalHelspawn.TurningScalyLegs".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                    if (400 <= Player.CumQ() && Player.CumQ() < 800)
                    {
                        OutputText("Scenes.Helspawnscene.SexAnalHelspawn.ChestVibratingHowl".Localized(
                            Player.CockDescript(),

                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Helspawnscene.SexAnalHelspawn.RisingToWrapLegs".Localized());
                    }
                    else
                        if (Player.CumQ() >= 800)
                        {
                            OutputText("Scenes.Helspawnscene.SexAnalHelspawn.BestialRoarBurst".Localized(
                                Player.CockDescript()
                            ));

                            OutputText("Scenes.Helspawnscene.SexAnalHelspawn.WrappedLegsHolding".Localized(
                                Player.CockDescript()
                            ));
                        }

            Player.Orgasm("Anal");

            Player.Orgasm("Dick");

            PostSexHelspawn();
        }

        public void SexRidingHelspawn(bool cameBeforeRiding)
        {
            ClearOutput();

            if (cameBeforeRiding)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnHadsex) != 1)
                {
                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.SeedGlazedEmbrace".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GrinningQualityTime".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.ChuckleAtFinishedAssumption".Localized());
                }
                else
                {
                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.OwnSeedTightHold".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.SmilingLipsLapGrind".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.TailTipPulledAway".Localized());

                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.MischiefEyesLastSqueeze".Localized(
                    Player.CockDescript()
                ));

                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GiggleButtCheekNestle".Localized(
                    Player.CockDescript(),

                    Player.CockDescript()
                ));
            }

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.ReinsertSalivatingSex".Localized(
                    Player.CockDescript()
                ));

                if (Player.BiggestCockLength() <= 8)
                {
                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.PostOrgasmicGasps".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GrindAroundHardDick".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.SilentKissAgreement".Localized(
                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.BouncingWetClaps".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));
                }
                else
                    if (Player.BiggestCockLength() > 8 && Player.BiggestCockLength() <= 12)
                    {
                        OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GroansVaginalEntry".Localized(
                            Player.CockDescript(),

                            Player.CockDescript(),

                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Helspawnscene.SexRidingHelspawn.ReleaseRigidGrind".Localized());

                        OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GrindToolLyingBack".Localized());

                        OutputText("Scenes.Helspawnscene.SexRidingHelspawn.ModeratePaceRide".Localized(
                            Player.CockDescript()
                        ));
                    }
                    else
                        if (Player.BiggestCockLength() > 12)
                        {
                            OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GruntVaginalIncursion".Localized(
                                Player.CockDescript(),

                                Player.CockDescript()
                            ));

                            OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GaspsHipsRollSing".Localized(
                                Player.CockDescript()
                            ));

                            OutputText("Scenes.Helspawnscene.SexRidingHelspawn.SlowRideDeepBreath".Localized(
                                Player.CockDescript(),

                                Player.CockDescript(),

                                Player.CockDescript()
                            ));
                        }

                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.SquealsHardDrilling".Localized(
                    Player.CockDescript()
                ));

                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.GirlCumWaterfallPress".Localized(
                    Player.CockDescript()
                ));

                if (400 <= Player.CumQ() && Player.CumQ() < 800)
                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.WombOverfillBellySwell".Localized(
                        Player.CockDescript()
                    ));
                else
                    if (Player.CumQ() >= 800)
                        OutputText("Scenes.Helspawnscene.SexRidingHelspawn.ClimaxOverflowBelly".Localized(
                            Player.CockDescript()
                        ));
            }
            else
                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.BothCocksPresented".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.LubricatedDualSlide".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.DualSensationTremble".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.ScreamingViolentSquirt".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.AgonizedPleasureScreams".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.WhimpersSpeedingClimax".Localized());

                    OutputText("Scenes.Helspawnscene.SexRidingHelspawn.HowlingSpurtExplosion".Localized());

                    if (400 <= Player.CumQ() && Player.CumQ() < 800)
                        OutputText("Scenes.Helspawnscene.SexRidingHelspawn.HugeLoadsBellySwell".Localized());
                    else
                        if (Player.CumQ() >= 800)
                            OutputText("Scenes.Helspawnscene.SexRidingHelspawn.CopiousLoadsBothHoles".Localized());
                }

            if (Player.CumQ() >= 400)
                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.FadingClimaxCreamCascade".Localized());
            else
                OutputText("Scenes.Helspawnscene.SexRidingHelspawn.FadingClimaxJuiceMix".Localized());

            Player.Orgasm("Dick");

            PostSexHelspawn();
        }

        public void PostSexHelspawn()
        {
            ClearOutput();

            if (Player.CumQ() >= 400)
                OutputText("Scenes.Helspawnscene.PostSexHelspawn.SlickSweatCollapse".Localized());
            else
                OutputText("Scenes.Helspawnscene.PostSexHelspawn.MarathonBreathRecovery".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.FacePullEmbrace".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.BreathlessGropingKiss".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.KissTrailHeatedBody".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.TwinkleGrinSmile".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.StomachGrowlFeast".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.GigglingPushAway".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.SmirkFeignedQuestion".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.TenderBellyRub".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.NoddingSlurpFinish".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.GroaningWarmEmbrace".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.TiredYawnSnuggle".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.LoveYouDriftOff".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.YawningForeheadKiss".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.SoftDozeSilence".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.GazingSoftExtricate".Localized());

            OutputText("Scenes.Helspawnscene.PostSexHelspawn.CumStrewnGearRecover".Localized(
                Player.ArmorName
            ));

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HelspawnLovemaking()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking.SalamanderPullKiss".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking.BlushingSmile".Localized(
                Player.Mf("D-Dad", "M-Mom")
            ));

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking.SurprisedReturnKiss".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking.OpenCampCaution".Localized(
                Player.Strength > 50 ? "You sweep [helspawn] off her feet, carrying her 'princess-style' as you take her to a suitable spot. " + (Player.Tallness < 55 ? "Common.Text.SheStiflesALaughAsBest".Localized() : "Common.Text.SheGaspsInSurpriseAtSuddenly".Localized()) : ""
            ));

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking.SheepishAllYours".Localized(
                Player.BiggestCockLength() < 6 && CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) > 0 ? "She immediately giggles at the sight, remarking, [say: I can see why Mom got someone else to help make me.] She lets out a contented sigh and adds, [say: But I don't mind it. I'm still your daughter either way.]" : (Player.GetClitLength() >= 6 && CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) > 0 ? "Common.Text.SheLooksToYourCrotchSpotting".Localized() : "Common.Text.SheBitesHerLipAsShe".Localized())
            ));

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            HelspawnLovemaking2();
        }

        public void HelspawnLovemaking2()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.PassionateForeplayKiss".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.ClitoralSmoochYelp".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.StartedCommitDemand".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.TonguePryLabia".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.ClitorisSuckShudder".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.WhinesCloseDenied".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking2.StoppedBeforeClimax".Localized());

            HelspawnLovemaking3();
        }

        public void HelspawnLovemaking3()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.CrawlingUpKiss".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.SubstitutionToken".Localized());

            if (Player.Sensitivity > 90)
            {
                OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.SensoryOverloadClimax".Localized());

                if (Player.HasCock())
                {
                    Player.Orgasm("Dick");

                    ReturnToCamp(1);

                    return;
                }
                else
                    Player.Orgasm("Vaginal");
            }

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.GripHipsScaleSlide".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.HoldMeEveryDrop".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.SpurredHugPumping".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.KissBreakMoanSurrender".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.WincingScreamOrgasm".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking3.CavingSlamRelease".Localized());

            Player.Orgasm(Player.HasCock() ? "Common.Button.Dick".Localized() : "Common.Button.Vaginal".Localized());

            HelspawnLovemaking4();
        }

        public void HelspawnLovemaking4()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnLovemaking4.MessyHairRecline".Localized());

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HelspawnSmooch()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSmooch.YouReachUpAnd".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSmooch.NervousTone".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSmooch.ChuckleOneThing".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSmooch.ProlongedKissGrope".Localized());

            if (HelspawnChaste())
                OutputText("Scenes.Helspawnscene.HelspawnSmooch.OnlyLoverPraise".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelspawnSmooch.InsistentNeedyRequest".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ReturnToCamp(1);
        }

        public void HelspawnFacesitting()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.SmilingHandsOnHips".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.CuteSmileHandGrip".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.HandInHandStroll".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.GigglingTicklishBrush".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.EnthusiasticAdmiration".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.KneelAbovePosition".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.EncouragedViewLower".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.IntenseOrgasmClause".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.BlissWavesGush".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting.AlluringSmirkQuestion".Localized());

            Player.Orgasm("Vaginal");

            HelspawnFacesitting2();
        }

        public void HelspawnFacesitting2()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting2.WetSexLoweringDrip".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting2.SalamanderHarderRhythm".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting2.MutualOrgasmDismount".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFacesitting2.SleepingArmsEscape".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HelspawnFootjob()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFootjob.GrabHandAndTell".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjob.ArrivalCabinLead".Localized(
                !Player.IsNaked() ? " With your daughter's flesh now properly bared, " + (HelspawnSlutty() ? "Common.Text.SheTurnsAroundToFaceYou".Localized() : "Common.Text.YouGentlyTurnHerToFace".Localized()) + " help you remove your clothing." : "",

                HelspawnSlutty() ? "Common.Text.ReachesDownToGentlyRubYour".Localized() : "Common.Text.IfHaslegsReachesDownTo".Localized() + (HelspawnSlutty() ? "Common.Text.Teasingly".Localized() : "Common.Text.Softly".Localized()) + "caress your inner thighs|gently caresses your [chest]}]"
            ));

            OutputText("Scenes.Helspawnscene.HelspawnFootjob.InquirySpecialIdea".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjob.ArousedGenitalsDisplay".Localized());

            if (Player.IsHerm())
            {
                ClearButtons();

                AddNextButton("Common.Button.Pussy".Localized(), HelspawnFootjobPussy);

                AddNextButton("Common.Button.Cock".Localized(), HelspawnFootjobCock);
            }
            else
                if (Player.HasCock()) HelspawnFootjobCock(); else HelspawnFootjobPussy();
        }

        public void HelspawnFootjobPussy()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFootjobPussy.ReptilianSolesWetVagina".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjobPussy.ExtendLegsThighSpread".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjobPussy.SoleGrindHeelPress".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjobPussy.SuddenClimaxTwitch".Localized());

            Player.Orgasm("Vaginal");

            HelspawnFootjobEnd();
        }

        public void HelspawnFootjobCock()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFootjobCock.ReptilianFeetHardCock".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjobCock.ExtendLegsGlisteningVulva".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjobCock.FeetTightenFastPace".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFootjobCock.ThrobbingFeetSpurt".Localized());

            Player.Orgasm("Cock");

            HelspawnFootjobEnd();
        }

        public void HelspawnFootjobEnd()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFootjobEnd.UnusualSexRelaxation".Localized(
                Save.GetBool("footjob") ? (HelspawnSlutty() ? "Common.Text.SayDamnDidntThinkIdEver".Localized() : "Common.Text.SayThatWasDifferentInA".Localized()) : (HelspawnSlutty() ? "Common.Text.SayDamnDadYouDefinitelyKnow".Localized() : "Common.Text.SayThatWasReallyFunThough".Localized())
            ));

            Save.SetBool("footjob", true);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HelspawnSlutsex()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.AdmiringGrownWoman".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.FeignedInnocenceAsk".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.BondBeyondWordsClaim".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.FieryHeatTongueClash".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.LipsLeaveSoftPause".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.AgreeingSmartGirl".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.BrushingHerScales".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.GentlePushUndress".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsex.SmokyScentSkinBrush".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Helspawnscene.HelspawnSlutsex.ArousalCoatedThrust".Localized());

                if (Player.Strength >= 50)
                {
                    ClearButtons();

                    AddNextButton("Common.Button.Fuck".Localized(), HelspawnSlutsexFuck);

                    AddNextButton("Common.Button.Resist".Localized(), HelspawnSlutsexResist);
                }
                else
                    HelspawnSlutsexResist();
            }
            else
                HelspawnSlutsexYuri();
        }

        public void HelspawnSlutsexFuck()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.DesperateLonging".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.BreathlessPlea".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.DoubtsDissolve".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.TrulyJoined".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.UnsuppressedCries".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.AchingLimbs".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.TwitchingDesire".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.CannotRefuseHer".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.UntappedReserves".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.TenderKiss".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFuck.HairRufflePout".Localized());

            Player.Orgasm("Dick");

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            ReturnToCamp(1);
        }

        public void HelspawnSlutsexResist()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.WorthTheWait".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.FarFromFinished".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.SinkingLower".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.EffortlessEntry".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.SereneAfterglow".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.GlidingCaress".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.BracingTurn".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.PushingBackDeep".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.SilkenGrip".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.FlusteredBegging".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.TailWrapsTight".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.BubblingSpill".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.TremblingRelease".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexResist.HairRufflePout".Localized());

            Player.Orgasm("Dick");

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            ReturnToCamp(1);
        }

        public void HelspawnSlutsexYuri()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.UnderHerSpell".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.BriefFarewell".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0 ? "shimmering like molten gold" : (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2 ? "Common.Text.FilledWithAVerdantCalmnessThat".Localized() : "Common.Text.FilledWithAnInnerFire".Localized())
            ));

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.ObligingGlance".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.BreathWashesOver".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.SlightestShift".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.GrowingDesire".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.FirstTongueTouch".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.HummingApproval".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri.TailTeasing".Localized());

            HelspawnSlutsexYuri2();
        }

        public void HelspawnSlutsexYuri2()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.HandsOnHerHead".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 0 ? "glimmering with mischief" : (CoC.Instance.Flags.Get(GameFlagIds.HelspawnDaddy) == 2 ? "Common.Text.FullOfLife".Localized() : "Common.Text.BlazingWithPassion".Localized())
            ));

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.MomentaryPause".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.DaughterBetweenThighs".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.FirmerGrip".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.BrushedSweetSpot".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.ShudderingClimax".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.FinalCaress".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexYuri2.WhisperedLove".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddNextButton("Common.Button.Kiss".Localized(), HelspawnSlutsexKiss);

            AddNextButton("Common.Button.More".Localized(), HelspawnSlutsexMore);

            AddNextButton("Common.Button.Finish".Localized(), () => HelspawnSlutsexFinish());
        }

        public void HelspawnSlutsexKiss()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexKiss.ClosingDistance".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexKiss.FlutteringWords".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexKiss.HungryGaze".Localized());

            HelspawnSlutsexFinish(false);
        }

        public void HelspawnSlutsexMore()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.PullingOnTop".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.IdleNipplePlay".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.DaughterSoClose".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.CuriousSmile".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.FirstTasteTease".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.ApologeticMurmur".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.JoinedInLove".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.ErraticMotion".Localized(
                FollowerRegistry.All.Count == 2 ? "her " + Player.Mf("", "other ") + "mother" : "all of your camp"
            ));

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.FleetingEmbarrassment".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.DeterminedRelease".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.CatchingBreath".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.TiredCuddle".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexMore.ReluctantDeparture".Localized());

            Player.Orgasm("Vaginal");

            HelspawnSlutsexFinish(false);
        }

        public void HelspawnSlutsexFinish(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Helspawnscene.HelspawnSlutsexFinish.ParagraphBreak".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFinish.WarmEmbrace".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFinish.LipsBrushCheek".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSlutsexFinish.LingeringThought".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelspawnHadsex, 1);

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HelspawnSurprise()
        {

            Save.SetBool("surprised", true);

            RegisterMacro("helspawnsleep", CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helspawn");

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 0 || Player.IsGenderless())
                HelspawnSFWSurprise();
            else
                if (Player.HasCock())
                    Player.HasVagina();
                else
                    HelspawnFemaleSurprise();
        }

        public void HelspawnSFWSurprise()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.DeliciousAroma".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.PromisingScent".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.MorningGreeting".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.BlushingAdmission".Localized(
                Player.Mf("the world's best dad.", "one of the world's best moms.")
            ));

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.SwellingPride".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.ShyMumble".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.QuietAdmission".Localized(
                KihaFollowerScene.Save.GetInt("badCookingTasted", 0) > 1 ? "you're immediately glad you took the time to tutor the dragoness" : (KihaFollowerScene.Save.GetInt("badCookingTasted", 0) != 0 ? "Common.Text.YourGutsImmediatelyQuiverAlreadyDreading".Localized() : "Common.Text.YouTellHerIfThatIt".Localized())
            ));

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.BringingToLips".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.HighestPraise".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.SatisfiedApproval".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSFWSurprise.FullAndWarm".Localized());

            Go<CampScene>();
        }

        public void HelspawnMaleSurprise()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.DreamVisit".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.UpwardGlance".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.GigglesInEar".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.TracingShiver".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.MorningWhisper".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.WakingToVoice".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.BackwardSlide".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.LiftingChin".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurprise.DelayedRealization".Localized());

            helspawnMaleSurpriseLastChoice = 0;

            ClearButtons();

            AddNextButton("Scenes.Faerie.SexMenu.ButtonOral".Localized(), HelspawnMaleSurpriseOral);

            AddNextButton("Common.Button.Vaginal".Localized(), HelspawnMaleSurpriseVaginal);

            AddNextButton("Common.Button.Anal".Localized(), HelspawnMaleSurpriseAnal);

            AddNextButton("Common.Button.Tail".Localized(), HelspawnMaleSurpriseTail);

            AddNextButton("Common.Button.Cum".Localized(), HelspawnMaleSurpriseCum);
        }

        public void HelspawnMaleSurpriseTransitions(int prev, int curr)
        {
            var variations = new List<string>();

            switch (prev)
            {
                case 0:
                    variations =

                    [
                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.Variation0".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.Variation1".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.Variation2".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.Variation3".Localized(),
                    ];

                    OutputText(variations[curr - 1]);

                    break;

                case 1:
                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.StrokingCheek".Localized());

                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.StatingDesires".Localized());

                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.EagerAnticipation".Localized());

                    break;

                case 2:
                    variations =

                    [
                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.VaginalVariation0".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.VaginalVariation1".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.VaginalVariation2".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.VaginalVariation3".Localized(),
                    ];

                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.WhimperingExit".Localized());

                    OutputText(variations[curr - 1]);

                    break;

                case 3:
                    variations =

                    [
                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.AnalVariation0".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.AnalVariation1".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.AnalVariation2".Localized(),

                        "Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.AnalVariation3".Localized(),
                    ];

                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.ShakyBreath".Localized());

                    OutputText(variations[curr - 1]);

                    break;

                case 4:
                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTransitions.FurtherDesires".Localized());

                    break;
            }

            helspawnMaleSurpriseLastChoice = curr;

            EnableButton(4);
        }

        public void HelspawnMaleSurpriseOral()
        {
            ClearOutput();

            HelspawnMaleSurpriseTransitions(helspawnMaleSurpriseLastChoice, 1);

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseOral.UnsteadyBreath".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseOral.ContentToTwirl".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseOral.MountingAnticipation".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseOral.CurlingTongue".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseOral.ReopenedMouth".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            AddButton(0, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), null);
        }

        public void HelspawnMaleSurpriseVaginal()
        {
            ClearOutput();

            HelspawnMaleSurpriseTransitions(helspawnMaleSurpriseLastChoice, 2);

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseVaginal.WatchingHerSink".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseVaginal.BuriedTogether".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseVaginal.WildThrusting".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseVaginal.RestlessTail".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseVaginal.LingeringChance".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            AddButton(1, "Common.Button.Vaginal".Localized(), null);
        }

        public void HelspawnMaleSurpriseAnal()
        {
            ClearOutput();

            HelspawnMaleSurpriseTransitions(helspawnMaleSurpriseLastChoice, 3);

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseAnal.TakingItSlow".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseAnal.SlowDisappearance".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseAnal.QuestionCutShort".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseAnal.TightMilking".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseAnal.RelentlessGrind".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            AddButton(2, "Common.Button.Anal".Localized(), null);
        }

        public void HelspawnMaleSurpriseTail()
        {
            ClearOutput();

            HelspawnMaleSurpriseTransitions(helspawnMaleSurpriseLastChoice, 4);

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.BlissfulHeat".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.CuriousQuestion".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.TailTipDistraction".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.RoamingHands".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.RelentingFreely".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.EmbarrassingStudy".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseTail.RisingPressure".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            AddButton(3, "Common.Button.Tail".Localized(), null);
        }

        public void HelspawnMaleSurpriseCum()
        {
            ClearOutput();

            switch (helspawnMaleSurpriseLastChoice)
            {
                case 0:
                {
                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.ThighsTightenAroundLength".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.GlancesBackConfused".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.FlushedFromRelease".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helspawn")
                    OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.TonightStartsAwkward".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.NowhereBetterToBe".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.SeesEndApproaching".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.SlipsAwayDirty".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.CumForBreakfast".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.WhereYouBelong".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.CollapsesWithoutSupport".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.DoingThatAgain".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.WhyWouldYouStop".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.LastShotDribble".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.WelcomesUnusualSensation".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.LetMeSeeCum".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.NoChoiceInMatter".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.HipsTrembleAndThrust".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnMaleSurpriseCum.MomentBeforeLeaning".Localized());
                }

                break;
            }

            if (helspawnMaleSurpriseLastChoice > 0)
                HelspawnSurpriseEnd(2);

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            Go<CampScene>();
        }

        public void HelspawnFemaleSurprise()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.WakesGlancingAround".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.QuickEyeRub".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.GoodMorningLine".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.LipsPressSoftly".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.WouldNotMind".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.BreathyExhale".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.KindEnoughToTouch".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.NotYetPromise".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.GripsSheetTighter".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.AirShiftsAnticipation".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.PalmPressesHarder".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.TastesYouDeeper".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.SheetsRustleFaster".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.BarelySaysDaddy".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.InstantBodyReaction".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.BreathlessAnticipation".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.AlmostMissSound".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise.HowYouWantIt".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Tribadism".Localized(), () => HelspawnFemaleSurprise2(0));

            AddNextButton("Common.Button.Cunnilingus".Localized(), () => HelspawnFemaleSurprise2(1));

            AddNextButton("Common.Button.DoesntMatter".Localized(), () => HelspawnFemaleSurprise2(2));
        }

        public void HelspawnFemaleSurprise2(int choice)
        {
            ClearOutput();

            if (choice == 2)
                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.AreYouSureQuestion".Localized());
            else
                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.FavoriteForDaddy".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.SatisfiedInchesCloser".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.KeepsExploringYou".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.HotDesperateWrithe".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.VanishWhenNeeded".Localized());

            if (choice == 0 || (choice == 2 && Random.Shared.Next(100) < (50)))
            {
                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.MercifullyReturns".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.NoEffortToMove".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.WorldTightensInside".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.FingertipTrailsDown".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.LostInPleasure".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.ReliefSheContinues".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.BreathingSlowsCuddle".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.NoSightBetter".Localized());
            }
            else
            {
                string hole = (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin != Player.Anal.Virgin ? (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin ? "Common.Text.Asshole".Localized() : "Common.Text.Vagina".Localized()) : RandomUtils.Choice(["[asshole]", "[vagina]"]));

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.LipsTickleStomach".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.EndlessAffectionCircling".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.HungerBeneathKiss".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.StraddlingYourDaughter".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.SoothingHeatSaps".Localized(
                    hole
                ));

                if (hole == "[asshole]") Player.ButtChange(3, true); else Player.CuntChange(3, true, true);
                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.WallsCaressTremble".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.SlidesOutSlowly".Localized(
                    hole
                ));

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.DarknessBurstsWhite".Localized(
                    hole
                ));

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.BreathingSlowsCollapse".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFemaleSurprise2.NoSightBetterAgain".Localized());
            }

            HelspawnSurpriseEnd(choice == 2 ? 1 : 0);

            Go<CampScene>();
        }

        public void HelspawnSurpriseEnd(int version)
        {

            OutputText("Scenes.Helspawnscene.HelspawnSurpriseEnd.ContentLoveSigh".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSurpriseEnd.SmileGrowsWider".Localized(
                version == 0 ? "You did ask for that, after all, and it was everything you wanted" : (version == 1 ? "Common.Text.IfHelspawnchasteYoudExpectNothingLess".Localized() : "Common.Text.ItWasEverythingYouAskedFor".Localized())
            ));

            OutputText("Scenes.Helspawnscene.HelspawnSurpriseEnd.SorryForKeepingYou".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnSurpriseEnd.SeeYouTonight".Localized());

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);
        }

        public void HelspawnDrinkKidAJuice()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnDrinkKidAJuice.HunchedOverBarrel".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnDrinkKidAJuice.KidAPeeksUp".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnDrinkKidAJuice.ArrivalShiftsAttention".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnDrinkKidAJuice.AnemoneSpeaksUp".Localized(
                HelspawnScales()
            ));

            OutputText("Scenes.Helspawnscene.HelspawnDrinkKidAJuice.KneelsTentacleHead".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnDrinkKidAJuice.KidANodsAssurance".Localized());

            Save.SetBool("sippedAnemone", true);

            Go<CampScene>();
        }

        public void SleepWithHelspawn()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helia")
            {
                OutputText("Scenes.Helspawnscene.SleepWithHelspawn.SharesBedWithHelia".Localized());

                OutputText("Scenes.Helspawnscene.SleepWithHelspawn.WantsAllTogether".Localized());

                OutputText("Scenes.Helspawnscene.SleepWithHelspawn.DoubleSalamanders".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "salamanders");
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helspawn" || CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "salamanders")
                {
                    OutputText("Scenes.Helspawnscene.SleepWithHelspawn.PrefersSeparateSleep".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.SleepWith, CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "salamanders" ? "Helia" : "");
                }
                else
                {
                    OutputText("Scenes.Helspawnscene.SleepWithHelspawn.AsksToSleepTogether".Localized());

                    OutputText("Scenes.Helspawnscene.SleepWithHelspawn.SeparatesWithHug".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Helspawn");
                }

            HelspawnsMainMenu();
        }

        public void HelspawnSleep()
        {

            switch (Random.Shared.Next(4))
            {
                case 0:
                {
                OutputText("Scenes.Helspawnscene.HelspawnSleep.SheSleepsAlready".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Helspawnscene.HelspawnSleep.EntersJustAfterHer".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Helspawnscene.HelspawnSleep.EntersToSeeHer".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Helspawnscene.HelspawnSleep.EmptyCabinPrep".Localized());
                }

                break;
            }

            Save.SetBool("wakeUp", true);

            OutputText("Scenes.Helspawnscene.HelspawnSleep.ParagraphBreak".Localized());
        }

        public void HelspawnWakeUp()
        {
            ClearOutput();

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.SunHerbalScent".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.TriesHerbalTea".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.FinishesTeaHeadpat".Localized());
                }

                break;

                case 1:
                {
                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.SunOpensEyelids".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.FinallyAwakeGreeting".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.WakesInHerGrip".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnWakeUp.EscapesHerEmbrace".Localized());
                }

                break;
            }

            Save.SetBool("wakeUp", false);

            Go<CampScene>();
        }

        public void HelspawnHeadpats()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnHeadpats.AsksForAffection".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnHeadpats.UnderstandingDawns".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnHeadpats.SettlesIntoRhythm".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnHeadpats.EyesCloseHappy".Localized());

            if (HelspawnChaste())
            {
                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.BreathingHitches".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.OverreactsShufflesFeet".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.LikedItSorry".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.ArmsOpenForHug".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.DrowsyBlinkQuestion".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.JustPats".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.AreYouSureThought".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.AsksIfSheLiked".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.NeverMindLoveYou".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnHeadpats.RushesIntoHug".Localized());
            }

            OutputText("Scenes.Helspawnscene.HelspawnHeadpats.GladThatsSettled".Localized());

            ReturnToCamp(1);
        }

        public void HelspawnFishing()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.HelspawnFishing.FishingPlanIdea".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.UncertainFishingReply".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.SortGearFirst".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnWeapon) == "bow")
                OutputText("Scenes.Helspawnscene.HelspawnFishing.SecondIdeaStrikes".Localized());
            else
            {
                OutputText("Scenes.Helspawnscene.HelspawnFishing.FashionFishingRod".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.LumberChoicePraise".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.RodImpressedReaction".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinWoodResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) - 1);

                Player.AddKeyValue("Carpenter's Toolbox", 1, -1);
            }

            OutputText("Scenes.Helspawnscene.HelspawnFishing.RiverSpotDoubts".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.WalkToRiverBank".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnWeapon) == "bow")
            {
                OutputText("Scenes.Helspawnscene.HelspawnFishing.AsksYourPlan".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.ArrowRequestSmile".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.PerceptiveSmile".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.EnthusiasmInfectious".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.ArrowSailsWater".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.MissDisheartened".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.DeterminedAgreement".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.FishStruckVictory".Localized());
            }
            else
            {
                OutputText("Scenes.Helspawnscene.HelspawnFishing.SuitableStoneFound".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.RaisesHandUnsure".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.WillingToTeach".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.WormOfferingReady".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.FirstCastLands".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.AsksWhatNext".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.TimeToWait".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.SuddenTug".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.PanickedShout".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.HookSettingLesson".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.GracefulFishArc".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.ExcitedCatchBoast".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.AmusedAnswers".Localized());

                OutputText("Scenes.Helspawnscene.HelspawnFishing.PeacefulRhythm".Localized());
            }

            OutputText("Scenes.Helspawnscene.HelspawnFishing.FourFishCatch".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.GratefulSmileReply".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.AnglingJoyAdvice".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.DinnerInvitation".Localized());

            OutputText("Scenes.Helspawnscene.HelspawnFishing.DeclineReturnHome".Localized());

            Save.SetBool("fishing", true);

            ReturnToCamp(1);
        }

        public void SlutspawnDreaming()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.SlutspawnDreaming.JoltedAwake".Localized());
            // 原版 HelSpawnScene.hx:3155：曾被小恶魔趁睡奸淫过才会想到这一层
            if (CoC.Instance.Flags.Get(GameFlagIds.HasBeenImpSleepRaped))
                OutputText("Scenes.Helspawnscene.SlutspawnDreaming.ImpDreadThoughts".Localized());
            else
                if (CampScene.CampGuarded())
                    OutputText("Scenes.Helspawnscene.SlutspawnDreaming.WatchfulEyeCaution".Localized(
                        CampScene.CampGuardRandom()
                    ));
                else
                    OutputText("Scenes.Helspawnscene.SlutspawnDreaming.GuardedInMareth".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreaming.MakeOutIntruder".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreaming.WhisperRecognized".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreaming.FlopsBesideYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Pretend".Localized(), SlutspawnDreamPretend);

            AddButton(1, "Combat.Ability.NamelessHorrorReveal.Name".Localized(), SlutspawnDreamReveal);
        }

        public void SlutspawnDreamPretend()
        {
            ClearOutput();

            var maleLovers = CampScene.MalePartnersAtCampList();

            if (silly && maleLovers.Count == 0 && SceneInstance<AkkyScene>.Get().IsOwned())
                maleLovers = [CoC.Instance.Flags.Get(GameFlagIds.AkkyName)];

            var femaleLovers = CampScene.FemalePartnersAtCampList();

            femaleLovers.Remove(helspawnName);

            var allLovers = maleLovers.Concat(femaleLovers).ToList();

            var preferMales = Player.SexOrientation > 60;

            var preferFemales = Player.SexOrientation < 40;

            var bisexual = !preferMales && !preferFemales;

            var loverPronoun = "their";

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.FeignSleepStillness".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.ImpatientPlea".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.LegSlipsOver".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.DesperateTouching".Localized());

            if (bisexual && femaleLovers.Count > 0 && maleLovers.Count > 0)
                OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.OtherSlutsJealousy".Localized());
            else
                if (preferMales)
                    switch (maleLovers.Count)
                    {
                        case 0:
                        {
                        OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.OfferUntilReplaced".Localized());

                        loverPronoun = "";
                        }

                        break;

                        case 1:
                        {
                        OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.HaveUsBoth".Localized(
                            maleLovers[0]
                        ));

                        loverPronoun = "his";
                        }

                        break;

                        default:
                        {
                        OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.OtherGuysGuess".Localized());
                        }

                        break;
                    }
                else
                    if (femaleLovers.Count == 1 && femaleLovers[0] == "Helia")
                    {
                        OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.MomJealousy".Localized());

                        loverPronoun = "her";
                    }
                    else
                        if (femaleLovers.Count > 1)
                            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.OtherGirlsGuess".Localized());
                        else
                        {
                            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.ErrorReportPrompt".Localized());

                            loverPronoun = "a bugged";
                        }

            if (loverPronoun != "")
                OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.TasteTolerated".Localized(
                    loverPronoun
                ));

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.HeavyBreathRising".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.FootstepsFading".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamPretend.SleeplessReplay".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            Go<CampScene>();
        }

        public void SlutspawnDreamReveal()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.SlutspawnDreamReveal.StiffensAtTouch".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamReveal.FingertipsOnClit".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), SlutspawnDreamRevealYes);

            AddButton(1, "Common.Button.No".Localized(), SlutspawnDreamRevealNo);
        }

        public void SlutspawnDreamRevealYes()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.SirenSong".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.BackGripBite".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.ShamelessRubbing".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.LoveConfessionShiver".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.NestlesKisses".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.ThrowsOffCovers".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.LeansTowardLength".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.MissingOutWonder".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.WordTicklesTip".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.BubblingPressure".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.SwallowsWithoutFlinch".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.CrawlsUpThanks".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.CuddlesToSleep".Localized());

                Player.Orgasm("Dick");
            }
            else
            {
                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.ThighTouchMarvel".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.HesitantBreath".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.KissOnHip".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.HotBreathDescent".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.AnticipatedDescent".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.LustfulMoonlight".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.SlickFingersSpread".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.HandBrushedAway".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.TremorContact".Localized());

                OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealYes.ComfortableEmbrace".Localized());

                Player.Orgasm("Vaginal");
            }

            Save.SetInt("slutspawnLastTimeSex", View.Time.Days);

            Go<CampScene>();
        }

        public void SlutspawnDreamRevealNo()
        {
            ClearOutput();

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealNo.ResistTemptation".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealNo.SighMeetingEyes".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealNo.InterruptedAssurance".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealNo.WordsDieUnsaid".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealNo.StepsAwayCheck".Localized());

            OutputText("Scenes.Helspawnscene.SlutspawnDreamRevealNo.LingeringFingers".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            Go<CampScene>();
        }

    }
}