using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Enemies.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<LethiceScenes>("coc:lethice_scenes")]
    public class LethiceScenes : Scene
    {

        public const int GameEndRedemption = 1;

        public const int GameEndConsort = 2;

        public const int GameEndPurge0To50 = 3;

        public const int GameEndPurge50To100 = 4;

        public const int GameEndPurge100 = 5;

        public const int GameEndConquerLow = 6;

        public const int GameEndConquerMed = 7;

        public const int GameEndConquerHigh = 8;

        public static int CompanionsCount() => CampScene.FollowersCount() + CampScene.SlavesCount() + CampScene.LoversCount();

        public static int TotalKills()
        {
            return CoC.Instance.Flags.Get(GameFlagIds.ImpsKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.GoblinsKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.TentacleBeastsKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.HellhoundsKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.MinotaursKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.WormsMassKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.AlicesKilled)

                + CoC.Instance.Flags.Get(GameFlagIds.HellmouthsKilled);
        }

        public override void Entry()
        {
            EncounterLethice();
        }

        public void EncounterLethice()
        {

            if (Player.Corruption < 90)
                GoFight();
            else
            {
                OutputText("Scenes.Lethicescenes.EncounterLethice.DoYouFightLethice".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), GoFight);

                AddButton(1, "Common.Button.Consort".Localized(), JoinHer);
            }
        }

        public void GoFight()
        {

            ShowImage("lethice-herself");

            OutputText("Scenes.Lethicescenes.GoFight.LethiceRisesFromThrone".Localized());

            OutputText("Scenes.Lethicescenes.GoFight.ScatteredApplauseBreaks".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Lethicescenes.GoFight.GoodThingShesNot".Localized());

            OutputText("Scenes.Lethicescenes.GoFight.ItsAFight".Localized());

            var m = EnemyRegistry.Create(new Identifier("coc", "lethice"));

            m.CreateStatusEffect(LethiceEnemy.PermaFlyStatusId, 0, 0, 0, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurkingsTouch) == 1)
                Player.CreateStatusEffect(LethiceEnemy.MinotaurKingsTouchId, 0, 0, 0, 0);

            CombatScene.Start(m, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void Defeated(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LethiceDefeated, 1);

            if (hpVictory)
                OutputText("Scenes.Lethicescenes.Defeated.CriesOnAllFours".Localized());
            else
                OutputText("Scenes.Lethicescenes.Defeated.CriesShakingHerHead".Localized());

            OutputText("Scenes.Lethicescenes.Defeated.WhatDoWithHer".Localized());

            CombatScene.CleanupAfterCombat();

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.PlowHer".Localized(), PlowHer);
            else
                AddButton(0, "Common.Button.PlowHer".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.QueenHer".Localized(), QueenHer);
            else
                AddButton(1, "Common.Button.QueenHer".Localized(), null);

            AddButton(2, "Common.Button.BoobPlay".Localized(), () => BoobPlay(hpVictory));

            var hasLethicite = Player.HasKeyItem("Sheila's Lethicite") || Player.HasKeyItem("Stone Statue Lethicite");

            if (hasLethicite && CoC.Instance.Flags.Get(GameFlagIds.MetMarae) > 0 && CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                AddButton(5, "Common.Button.Redemption".Localized(), () => Redemption(hpVictory));

            AddButton(14, "Common.Button.SkipOut".Localized(), TheChoiceDunDunDun);
        }

        public void PlowHer()
        {
            ClearOutput();

            ShowImage("lethice-join");

            OutputText("Scenes.Lethicescenes.PlowHer.AfterAllYouveBeen".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.YouWord".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Lethicescenes.PlowHer.StepWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.PlowHer.SlitherWord".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.LoomingOverTheDemoness".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.HatredResentmentIndignation".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.TurnsRegardingContemptuously".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.RewardHerWeakInsult".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.GlancingDownBetweenLegs".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.AndLethiceIsPanting".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.HardAsARock".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Lethicescenes.PlowHer.CockWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.PlowHer.CocksWord".Localized());

            OutputText("Scenes.Lethicescenes.PlowHer.PushingUncomfortably".Localized());

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Lethicescenes.PlowHer.UnlessBetterPlans".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.FuckPussy".Localized(), PlowHerPussy);

                AddButton(1, "Common.Button.FuckButt".Localized(), PlowHerButt);

                return ;
            }

            OutputText("Scenes.Lethicescenes.PlowHer.WithMoreThanOne".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PlowHerDouble);
        }

        public void PlowHerPussy()
        {
            ClearOutput();

            ShowImage("lethice-plow-her-pussy");

            OutputText("Scenes.Lethicescenes.PlowHerPussy.PrimedAndReadySpanked".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.GrinningYouDecide".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.BeforeYouDoAnything".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.WingsBeatWeakly".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.JustCantResistLethice".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.OrgasmTreatsYouTo".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.RidingOutHerClimax".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.DoneThenStagger".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.SmirkGrabLethice".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Lethicescenes.PlowHerPussy.GleefulWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.PlowHerPussy.ShockedWord".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.AtWhatYouveDone".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerPussy.NowYouHaveChoice".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TheChoiceDunDunDun);
        }

        public void PlowHerButt()
        {
            ClearOutput();

            ShowImage("lethice-plow-her-butt");

            OutputText("Scenes.Lethicescenes.PlowHerButt.TemptingAsThatSlit".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.InsteadGrabYourSchlong".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.CriesOutInPain".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.GiveHerAssAppreciative".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.ContentInLethicesComplete".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.ShoutToDemonHorde".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.AssLikeSilkenVice".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.OrgasmTreatsYouTo".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.RidingOutHerClimax".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.DoneThenStagger".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.ContemptuousSmirkYouPush".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Lethicescenes.PlowHerButt.GleefulWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.PlowHerButt.ShockedWord".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.AtWhatYouveDone".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerButt.NowYouHaveChoice".Localized());

            TheChoiceDunDunDun();
        }

        public void PlowHerDouble()
        {
            ClearOutput();

            ShowImage("lethice-plow-her-double");

            OutputText("Scenes.Lethicescenes.PlowHerDouble.SmirkDownAtLethices".Localized());

            if (Player.Cocks.Count == 2)
                OutputText("Scenes.Lethicescenes.PlowHerDouble.OtherWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.PlowHerDouble.NextWord".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.MemberNestlesBetweenLips".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.RelentlesslyPushForward".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.ForNowAtLeast".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.ContentInLethicesComplete".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.OrgasmTreatsYouTo".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.RidingOutHerClimax".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.DoneThenStagger".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.ContemptuousSmirkYouPush".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Lethicescenes.PlowHerDouble.PropFeetUp".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.SentencePeriod".Localized());

            OutputText("Scenes.Lethicescenes.PlowHerDouble.NowYouHaveChoice".Localized());

            TheChoiceDunDunDun();
        }

        public void QueenHer()
        {
            ClearOutput();

            ShowImage("lethice-queen-her");

            OutputText("Scenes.Lethicescenes.QueenHer.DemonQueenIsDefeated".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.ButShesNotQueen".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Lethicescenes.QueenHer.StepWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.QueenHer.SlitherWord".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.ForwardPullingYourOff".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.ManagesToGroan".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.ParagraphBreak".Localized());

            if (Player.Corruption < 66)
                OutputText("Scenes.Lethicescenes.QueenHer.ScowlingAtHaughtyBitch".Localized());
            else
                OutputText("Scenes.Lethicescenes.QueenHer.LaughInHerFace".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.SwingLegOverHer".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.EitherWayYoureTreated".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Lethicescenes.QueenHer.CuppingATit".Localized());
            else
                OutputText("Scenes.Lethicescenes.QueenHer.PinchingYourNipples".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.ArchingYourBack".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.CroonYourApproval".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.LettingHerContinue".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.RockBackForth".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Lethicescenes.QueenHer.DraggingAcrossHerFace".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.KeepsWrigglingUnderYou".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.ShoutGivingIn".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.SheDoesYouRegret".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.UnderThatTreatment".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.LastShiveringVestiges".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Lethicescenes.QueenHer.ToYourFeet".Localized());
            else
                OutputText("Scenes.Lethicescenes.QueenHer.OffWord".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.LyingStunnedOnThe".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Lethicescenes.QueenHer.PropFeetUp".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.SentencePeriod".Localized());

            OutputText("Scenes.Lethicescenes.QueenHer.NowYouHaveChoice".Localized());

            TheChoiceDunDunDun();
        }

        public void BoobPlay(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.BoobPlay.ThisIsTheGreat".Localized());

            if (!hpVictory)
                OutputText("Scenes.Lethicescenes.BoobPlay.BySuperiorSexualAcumen".Localized());
            else
                OutputText("Scenes.Lethicescenes.BoobPlay.MightOfWeapon".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.LooksAlmostNormal".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.GrabDefeatedDemoness".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Lethicescenes.BoobPlay.ThrowOntoHerBack".Localized());
            else
                OutputText("Scenes.Lethicescenes.BoobPlay.PushOntoHerBack".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.TheWayShesDressed".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.CoosDarkEyesFixed".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.CutOffTired".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.EyesGoWideStaring".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Lethicescenes.BoobPlay.BigEnoughToFuck".Localized());

            OutputText("Scenes.Lethicescenes.BoobPlay.HalfSureNow".Localized());

            if (Player.HasCock())
            {
                ShowImage("lethice-boob-male");

                OutputText("Scenes.Lethicescenes.BoobPlay.YankingOffYourGrab".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.BackGoesFlatAgainst".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.MoansLiftingHerself".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.GiveMilkyJug".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Lethicescenes.BoobPlay.AnyOtherCorruptedSoul".Localized());
                else
                    OutputText("Scenes.Lethicescenes.BoobPlay.LikeSomeDemon".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.IsMaddening".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.GruntOfAnnoyance".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.PrettySureYouHear".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.ThankfullyPrettySure".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.DemonQueenFinallyLearning".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.HandsCloseFirmly".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.DoneWithHer".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.NowChoiceToMake".Localized());
            }
            else
            {
                ShowImage("lethice-join");

                OutputText("Scenes.Lethicescenes.BoobPlay.OfCourseSheDoesnt".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.MoansChewingLowerLip".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.GiveDemonQueenA".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.HandsKeepGroping".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.PerfectLickYourLips".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.AtLeastShesNot".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.CorruptedMilkPoured".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.GrinningGrabThe".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Lethicescenes.BoobPlay.WombWord".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.BowelsWord".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.PumpingDemonQueen".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.ParagraphBreak".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Lethicescenes.BoobPlay.OneCockNotEnough".Localized());

                    if (!Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Lethicescenes.BoobPlay.ALegWord".Localized());
                    else
                        OutputText("Scenes.Lethicescenes.BoobPlay.YourCoils".Localized());

                    OutputText("Scenes.Lethicescenes.BoobPlay.OverLethicesBody".Localized());
                }

                OutputText("Scenes.Lethicescenes.BoobPlay.StartToMoveGrinding".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Lethicescenes.BoobPlay.SLetter".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.ThroughAgainAndAgain".Localized());

                if (Player.Corruption >= 66)
                    OutputText("Scenes.Lethicescenes.BoobPlay.FinallyWhereYouBelong".Localized());
                else
                    OutputText("Scenes.Lethicescenes.BoobPlay.EvenDemonsRecognize".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.ForNowSateYourself".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.LittleConcernToCorrupted".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Lethicescenes.BoobPlay.SLetterAgain".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.HardAndFastAs".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.NotLongAfterCum".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Lethicescenes.BoobPlay.HolesWord".Localized());
                else
                    OutputText("Scenes.Lethicescenes.BoobPlay.AssWord".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.FromTheSwollenBreast".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Lethicescenes.BoobPlay.SLetterThird".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.GushingLacticLoad".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Lethicescenes.BoobPlay.SLetterFourth".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.SentencePeriod".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.DoneWithHerAgain".Localized());

                OutputText("Scenes.Lethicescenes.BoobPlay.NowChoiceAgain".Localized());
            }

            TheChoiceDunDunDun();
        }

        public void Redemption(bool hpVictory)
        {
            ClearOutput();

            ShowImage("lethice-join");

            OutputText("Scenes.Lethicescenes.Redemption.LookingDownAtThe".Localized());

            if (hpVictory)
                OutputText("Scenes.Lethicescenes.Redemption.BatteredAndBeatenDemon".Localized());
            else
                OutputText("Scenes.Lethicescenes.Redemption.HornyDesperateDemon".Localized());

            OutputText("Scenes.Lethicescenes.Redemption.ShesDefenselessNow".Localized());

            OutputText("Scenes.Lethicescenes.Redemption.CouldPurifyHer".Localized());

            OutputText("Scenes.Lethicescenes.Redemption.GrinningWithTheDawning".Localized());

            OutputText("Scenes.Lethicescenes.Redemption.InsteadGrabHerBy".Localized());

            OutputText("Scenes.Lethicescenes.Redemption.DemonsInStandsHiss".Localized());

            OutputText("Scenes.Lethicescenes.Redemption.TakingLethiceByShoulder".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), RedemptionII);
        }

        public void RedemptionII()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.RedemptionII.DayOfTravelLater".Localized());

            ShowImage("lethice-Redemption-marae");

            OutputText("Scenes.Lethicescenes.RedemptionII.SweetEtherealVoice".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.HaulingBoundAndGagged".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.SmileAtMaraeSaying".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.MaraeRecovers".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.ManyWantLethiceDead".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.ListensNoddingSlowly".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.EventuallyConcedes".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.QueryReachingIntoBag".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.ShiversDrawingBack".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.CouldMaraeDoThat".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.SheSmiles".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.EveryDemonYouRedeem".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.NodsApprovingly".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.SneersEvenAsMighty".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.WhispersHoldingPink".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.WhenYouveRecovered".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.IsItYouAsk".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Lethicescenes.RedemptionII.Kneeling".Localized());
            else
                OutputText("Scenes.Lethicescenes.RedemptionII.Leaning".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.OverTheFallenDemoness".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.MurmursPuttingHandOver".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.GoddessReachesBack".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.BarelyRecognizeHerNow".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.WarilyYou".Localized());

            if (Player.Weapon.IsUnarmed())
                OutputText("Scenes.Lethicescenes.RedemptionII.FlipAKnifeOut".Localized());
            else
                OutputText("Scenes.Lethicescenes.RedemptionII.TakeYourWeapon".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.CutHerBondsTrusting".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.MoansStrugglingToA".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.QuestionTakesYouAback".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.ASecondChancePerhaps".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionII.WhatDoYouTell".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), RedemptionIII);
        }

        public void RedemptionIII()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.RedemptionIII.WhatNameGive".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), RedemptionIIIGoName);

        }

        public void RedemptionIIIGoName()
        {
            ClearOutput();

            var input = "" ;

            if (input == "")
            {
                OutputText("Scenes.Lethicescenes.RedemptionIIIGoName.MustSelectName".Localized());

                DoNext(() => RedemptionIIIGoName());

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.LethiceName, input);

            if (CoC.Instance.Flags.Get(GameFlagIds.LethiceName) == "Lethice")
            {
                OutputText("Scenes.Lethicescenes.RedemptionIIIGoName.DecideToTellTruth".Localized());

                OutputText("Scenes.Lethicescenes.RedemptionIIIGoName.AtLeastThatsHope".Localized());

                OutputText("Scenes.Lethicescenes.RedemptionIIIGoName.EvenBeforeYouFinish".Localized());
            }
            else
            {
                OutputText("Scenes.Lethicescenes.RedemptionIIIGoName.DecideItsBetterNot".Localized());

                OutputText("Scenes.Lethicescenes.RedemptionIIIGoName.GiveHerNewName".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.LethiceName)
                ));
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), RedemptionIV);
        }

        public void RedemptionIV()
        {
            ClearOutput();

            ShowImage("lethice-Redemption-end");

            OutputText("Scenes.Lethicescenes.RedemptionIV.SpendSomeTimeThere".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.LethiceName)
            ));

            OutputText("Scenes.Lethicescenes.RedemptionIV.SoonComesToLight".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.LethiceName)
            ));

            OutputText("Scenes.Lethicescenes.RedemptionIV.DemonsCarriedFewShards".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.LethiceName),

                CoC.Instance.Flags.Get(GameFlagIds.LethiceName)
            ));

            OutputText("Scenes.Lethicescenes.RedemptionIV.RepeatsTheProcess".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionIV.WhenShesFinishedCollapses".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.LethiceName),

                CoC.Instance.Flags.Get(GameFlagIds.LethiceName)
            ));

            OutputText("Scenes.Lethicescenes.RedemptionIV.CriesEvenAsOther".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionIV.CertainlyDidWithPower".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionIV.SomehowGetTheImpression".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionIV.LongHardCrusade".Localized());

            OutputText("Scenes.Lethicescenes.RedemptionIV.TheEnd".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndRedemption);

            DoNext(() => PostEndingChoices());
        }

        public void JoinHer()
        {
            ClearOutput();

            ShowImage("lethice-join");

            OutputText("Scenes.Lethicescenes.JoinHer.SteppingTowardsDemonQueen".Localized());

            if (Player.Weapon.IsUnarmed())
                OutputText("Scenes.Lethicescenes.JoinHer.RaiseYourEmptyHands".Localized());
            else
                OutputText("Scenes.Lethicescenes.JoinHer.SetAsideYourWeapon".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.SmileLethiceCocksBemused".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.PurrsWingsTwitching".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.VeryDifferentSpeech".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DriderincubusKilled) == 0 || CoC.Instance.Flags.Get(GameFlagIds.MinotaurkingKilled) == 0)
                OutputText("Scenes.Lethicescenes.JoinHer.HumiliatedWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.JoinHer.SlainWord".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.ByYourHand".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.WordsTakeAback".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.SlowlyASmileSpreads".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.StandsHighHeelsClicking".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.SoYouveResisted".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DriderincubusKilled) == 1 || CoC.Instance.Flags.Get(GameFlagIds.MinotaurkingKilled) == 1)
                OutputText("Scenes.Lethicescenes.JoinHer.CarvedABloodySwathe".Localized());
            else
                OutputText("Scenes.Lethicescenes.JoinHer.FuckedYourWay".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.ThroughMyKeep".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.AnswerGrabbingDemoness".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.LethiceSmirks".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.EveryOneAndMore".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.ChucklesBrushingHandOff".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.SmilesAndStepsBack".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.WalksBackwardsToThrone".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.AsIfToPunctuate".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.PurrsBringingFinger".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.ItsAnEasyChoice".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.ApproachTheThrone".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.ExplosionOfSensation".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.MurmursWrappingThighs".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.FeelLethicesSatinSoft".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.OfCourseYouWill".Localized());

            OutputText("Scenes.Lethicescenes.JoinHer.WouldntHaveItAny".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), JoinHerII);
        }

        public void JoinHerII()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.JoinHerII.TimePassesInMareth".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.SpareLittleThought".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.LegendOfTheSpread".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.NoMore".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.ThoughYourMemories".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.AndNowTheDemons".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.SightOfSuchMagnificent".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Lethicescenes.JoinHerII.CockParagraphBreak".Localized());
            else
                OutputText("Scenes.Lethicescenes.JoinHerII.NewgrownCockSideBenefit".Localized());

            if (Player.Gender == Gender.Female)
                ShowImage("lethice-joined-female");
            else
                ShowImage("lethice-joined-male");

            OutputText("Scenes.Lethicescenes.JoinHerII.ThrillOfVictory".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.SomethingClicksInside".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.RightWhereYouBelong".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.ThusEndsPurity".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.CoosProppingHerselfUp".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.SmilesOverHerShoulder".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.SmilesBehindHer".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.PerhapsHaveToAdmit".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.SomePartOfYou".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.ChucklesWrappingArms".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.DonningWickedSmirk".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.NodWhereToFirst".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.JustSmilesBrushesPast".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.InstructionLine".Localized());

            OutputText("Scenes.Lethicescenes.JoinHerII.TheEnd".Localized());

            AwardAchievement(AchievementIds.YouBastard, true, true);

            CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndConsort);

            DoNext(() => PostEndingChoices());
        }

        public void Won(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.Won.YouFall".Localized());

            OutputText("Scenes.Lethicescenes.Won.AfterEverythingYouveDone".Localized());

            if (!hpVictory)
                OutputText("Scenes.Lethicescenes.Won.RampantDesires".Localized());
            else
                OutputText("Scenes.Lethicescenes.Won.BeneathMercilessAssault".Localized());

            OutputText("Scenes.Lethicescenes.Won.GaspingYouSlump".Localized());

            if (!Player.IsBiped())
                OutputText("Scenes.Lethicescenes.Won.DownWord".Localized());
            else
                OutputText("Scenes.Lethicescenes.Won.ToYourKnees".Localized());

            OutputText("Scenes.Lethicescenes.Won.CommaWord".Localized());

            if (!Player.Weapon.IsAttached())
                OutputText("Scenes.Lethicescenes.Won.ClatteringAcrossStone".Localized());
            else
                OutputText("Scenes.Lethicescenes.Won.TooWeakBallFists".Localized());

            OutputText("Scenes.Lethicescenes.Won.VictoriousDemonStrides".Localized());

            OutputText("Scenes.Lethicescenes.Won.PurrsLiftingArms".Localized());

            ShowImage("lethice-join");

            OutputText("Scenes.Lethicescenes.Won.ReachesDownCuppingChin".Localized());

            OutputText("Scenes.Lethicescenes.Won.SoPowerfulSelfRighteous".Localized());

            if (CurrentMonster.LustPercent < 50 && CombatController.Instance!.Context.Enemy.Health > CombatController.Instance!.Context.Enemy.MaxHealth * 0.5f)
                OutputText("Scenes.Lethicescenes.Won.AlmostWord".Localized());

            OutputText("Scenes.Lethicescenes.Won.LicksLipsGives".Localized());

            OutputText("Scenes.Lethicescenes.Won.LookAwayFromLethice".Localized());

            if (hpVictory)
                OutputText("Scenes.Lethicescenes.Won.HideHowTurnedOn".Localized());
            else
                OutputText("Scenes.Lethicescenes.Won.NotLetHerSee".Localized());

            OutputText("Scenes.Lethicescenes.Won.DemonQueenJustSmiles".Localized());

            OutputText("Scenes.Lethicescenes.Won.TurnIntoDog".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GoDog".Localized(), Woofwoof);

            AddButton(1, "Common.Button.Suicide".Localized(), GoKillYourself);
        }

        public void Woofwoof()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.Woofwoof.TimePasses".Localized());

            ShowImage("lethice-gone");

            OutputText("Scenes.Lethicescenes.Woofwoof.HeelChamp".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.LookUpWithStart".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.MistressChidesTracing".Localized());

            if (Player.DogCocks() == 0)
                OutputText("Scenes.Lethicescenes.Woofwoof.NewlyGrownDoggyCock".Localized());
            else
                OutputText("Scenes.Lethicescenes.Woofwoof.ThickThrobbingDogMember".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.SwollenKnotYanked".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.SaysTuggingOnLeash".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.ShudderAtUnbiddenMemory".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.DragonessAlmostStill".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.MistressSnapsFingers".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.MurmursTuggingOnLeash".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.ObedientDogYouAre".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.SilentSalientMoment".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.LaughsNobleLyrical".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.SmirksStillRubbing".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.GirlBlinksRageOn".Localized());

            OutputText("Scenes.Lethicescenes.Woofwoof.MistressSmirksStanding".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
                OutputText("Scenes.Lethicescenes.Woofwoof.PredecessorDestroyedFacility".Localized());
            else
            {
                OutputText("Scenes.Lethicescenes.Woofwoof.HowDidYouEscape".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ZetazDefeatedAndKilled) == 0)
                    OutputText("Scenes.Lethicescenes.Woofwoof.ZetazWord".Localized());
                else
                    OutputText("Scenes.Lethicescenes.Woofwoof.NewWarden".Localized());

                OutputText("Scenes.Lethicescenes.Woofwoof.FingersIWonderTsk".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Libido, 100f), new DynStat(GameStats.Sensitivity, 50f), new DynStat(GameStats.Corruption, 70f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WoofwoofII);
        }

        public void WoofwoofII()
        {
            ClearOutput();

            ShowImage("lethice-woof-part1");

            OutputText("Scenes.Lethicescenes.WoofwoofII.BarkExcitedlyReady".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.IsThatYouGasps".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.CockYourHeadAside".Localized(
                Player.Mf("stud", "slut")
            ));

            OutputText("Scenes.Lethicescenes.WoofwoofII.ChidesPattingYourHead".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.BarkOnAllFours".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.SmilesCoilingHand".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.SacrificialYouDontKnow".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.ChampionGirlStares".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.NoseTwitchesStare".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.ComeOnGirlCries".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.WhyStillTalkingTo".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.GivesLeashAGentle".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.BarkFeelingYourselfHarden".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofII.PurrsDroppingLeash".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WoofwoofIII);
        }

        public void WoofwoofIII()
        {
            ClearOutput();

            ShowImage("lethice-woof-part2");

            OutputText("Scenes.Lethicescenes.WoofwoofIII.YoureOnTheBitch".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIII.CoosBehindYouMunching".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIII.LipsLockAroundPink".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIII.GirlsStrugglingStops".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIII.GaspsVoiceRaggedHusky".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIII.PleaseFuckHerHarder".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIII.ChampMistressMurmurs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WoofwoofIV);
        }

        public void WoofwoofIV()
        {
            ClearOutput();

            ShowImage("badend-Woofwoof");

            OutputText("Scenes.Lethicescenes.WoofwoofIV.SeeingMistressEnjoying".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIV.LikeAFireHose".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIV.MistressSmilesStanding".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIV.SmilesAndStrokesHair".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIV.AddsTuggingYourLeash".Localized());

            OutputText("Scenes.Lethicescenes.WoofwoofIV.BarkEagerlyAndLunge".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 70f));

            Player.Orgasm("Generic");

            GameOver();

            RemoveButton(1);
        }

        public void GoKillYourself()
        {
            ClearOutput();

            ShowImage("badend-death");

            OutputText("Scenes.Lethicescenes.GoKillYourself.NothingWorseThanBeing".Localized());

            OutputText("Scenes.Lethicescenes.GoKillYourself.VisionBeginsToFade".Localized());

            OutputText("Scenes.Lethicescenes.GoKillYourself.HerLastLaugh".Localized());

            AwardAchievement(AchievementIds.Nooooooooooo);

            Player.HealthChange(-Player.Health + 9999);

            GameOver();
        }

        public void TheChoiceDunDunDun()
        {

            ClearButtons();

            AddButton(0, "Scenes.Tamani.KillTamaniChoice.ButtonKillHer".Localized(), KillHerPrompt);

            AddButton(1, "Common.Button.LeaveHer".Localized(), LeaveHer);
        }

        public void KillHerPrompt()
        {
            ClearOutput();

            if (Player.WeaponVerb == "slash" || Player.WeaponVerb == "cleave" || Player.WeaponVerb == "keen cut" || Player.WeaponVerb == "stab")
            {
                OutputText("Scenes.Lethicescenes.KillHerPrompt.HowEndLethice".Localized());

                AddButton(0, "Common.Button.SnapHerNeck".Localized(), () => KillHer(0));

                AddButton(1, "Common.Button.BeheadHer".Localized(), () => KillHer(1));

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                    AddButton(2, "Common.Button.UnbirthHer".Localized(), () => KillHer(2));
            }
            else
                KillHer();
        }

        public void KillHer(int method = 0)
        {
            ClearOutput();

            if (method == 0)
            {
                OutputText("Scenes.Lethicescenes.KillHer.DeservesTheWorstSorts".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LethiceKilled, 1);
            }
            else
                if (method == 1)
                {
                    ShowImage("lethice-head");

                    OutputText("Scenes.Lethicescenes.KillHer.DeservesAPunishment".Localized());

            AwardAchievement(AchievementIds.OffWithHerHead, true, true, true);

                    CoC.Instance.Flags.Set(GameFlagIds.LethiceKilled, 2);
                }
                else
                    if (method == 2)
                    {
                        OutputText("Scenes.Lethicescenes.KillHer.RollOntoYourBack".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.LethiceKilled, 1);
                    }

            OutputText("Scenes.Lethicescenes.KillHer.AssembledDemonsScatter".Localized());

            Player.UpgradeBeautifulSword();

            PostTheChoice();
        }

        public void LeaveHer()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LethiceLeftAlive, 1);

            OutputText("Scenes.Lethicescenes.LeaveHer.KickInTheRibs".Localized());

            OutputText("Scenes.Lethicescenes.LeaveHer.AttemptsToCrawlAway".Localized());

            PostTheChoice();
        }

        public void PostTheChoice()
        {

            OutputText("Scenes.Lethicescenes.PostTheChoice.NowThatYoureAlone".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GoHome".Localized(), GoHome);

            AddButton(1, "Common.Button.Claimfort".Localized(), ClaimFortress);
        }

        public void GoHome()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.GoHome.ThisPlaceIsHuge".Localized());

            OutputText("Scenes.Lethicescenes.GoHome.HoursPassFruitlessly".Localized());

            OutputText("Scenes.Lethicescenes.GoHome.WerentAlwaysDemons".Localized());

            OutputText("Scenes.Lethicescenes.GoHome.AlmostFeelBadFor".Localized());

            OutputText("Scenes.Lethicescenes.GoHome.SighingGrabABook".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GoHomeII);
        }

        public void GoHomeII()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.GoHomeII.WeeksLater".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeII.DamnedBastardsHadIt".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeII.NoRoomForLoyalty".Localized());

            if (CompanionsCount() > 0)
            {
                OutputText("Scenes.Lethicescenes.GoHomeII.AssistedByYourFollower".Localized());

                if (CompanionsCount() > 1)
                    OutputText("Scenes.Lethicescenes.GoHomeII.SLetter".Localized());
            }

            OutputText("Scenes.Lethicescenes.GoHomeII.BeforeYouKnowIt".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeII.VengeanceOrJustice".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Vengeance".Localized(), GoHomeVeng);

            AddButton(1, "Scenes.Dullahan.DullahanCurse.ButtonJustice".Localized(), GoHomeJustice);
        }

        public void GoHomeVeng()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.GoHomeVeng.LaterBreak".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeVeng.TalesOfTheChampions".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeVeng.AfterThatNight".Localized(
                Player.Mf("King", "Queen")
            ));

            if (Player.Corruption <= 33)
                OutputText("Scenes.Lethicescenes.GoHomeVeng.DespiteFearAndTerror".Localized());
            else
                if (Player.Corruption <= 66)
                    OutputText("Scenes.Lethicescenes.GoHomeVeng.DespiteFearAgain".Localized());
                else
                    OutputText("Scenes.Lethicescenes.GoHomeVeng.DespiteFearThird".Localized());

            DoNext(() => PostEndingChoices());
        }

        public void GoHomeJustice()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.GoHomeJustice.LaterBreak".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeJustice.TalesOfChampionsArrival".Localized());

            if (Player.Corruption >= 50)
                OutputText("Scenes.Lethicescenes.GoHomeJustice.EndToRank".Localized());
            else
                OutputText("Scenes.Lethicescenes.GoHomeJustice.SentThroughGlowingPortal".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeJustice.ElderNomurProtested".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Lethicescenes.GoHomeJustice.InWisdomStoppedTownsfolk".Localized());
            else
                OutputText("Scenes.Lethicescenes.GoHomeJustice.TownsfolkLynchedLeaders".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeJustice.LifeContinuedNewCouncil".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeJustice.WarriorWithInfallible".Localized());

            if (CompanionsCount() > 0)
            {
                OutputText("Scenes.Lethicescenes.GoHomeJustice.NothingOfFriend".Localized());

                if (CompanionsCount() > 1)
                    OutputText("Scenes.Lethicescenes.GoHomeJustice.SLetter".Localized());
            }

            OutputText("Scenes.Lethicescenes.GoHomeJustice.IngnamProspered".Localized());

            OutputText("Scenes.Lethicescenes.GoHomeJustice.WhenAgeClaimed".Localized(
                Player.Mf("hero", "heroine")
            ));

            DoNext(() => PostEndingChoices());
        }

        public void ClaimFortress()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.ClaimFortress.ThisFortressFitting".Localized());

            ClearButtons();

            AddButton(0, "Combat.Ability.BeautifulSwordFightPurge.Name".Localized(), PurgeShit);

            AddButton(1, "Common.Button.Conquer".Localized(), Conquer);
        }

        public float HowFuckedIsThePlayerOMeter()
        {

            float levelOfFuckedness = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                levelOfFuckedness += 50;

            if (CoC.Instance.Flags.Get(GameFlagIds.SheilaDemon) == 1)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.LethiceKilled) == 0)
                levelOfFuckedness += 25;

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurkingKilled) == 0)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.DriderincubusKilled) == 0)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.D3CentaurDefeated) != 1)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.D3GardenerDefeated) != 3)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.D1OmnibusKilled) != 1)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetazDefeatedAndKilled) != 1)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) != 1)
                levelOfFuckedness += 25;
            // 原版 LethiceScenes.hx:855 camp.campCorruptJojo()
            if (SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo())
                levelOfFuckedness += 10;

            if (Player.HasStatusEffect(StatusEffectIds.WandererDemon))
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.FeraReleased) != 0)
                levelOfFuckedness += 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaCorruptionProgress) >= 10)
                levelOfFuckedness += 15;

            if (CoC.Instance.Flags.Get(GameFlagIds.MoreCumWitches) == 1)
                levelOfFuckedness -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.FreedVala) == 1)
                levelOfFuckedness -= 10;
            // 原版 LethiceScenes.hx:873 camp.ceraphIsFollower()
            if (SceneInstance<Npcs.CeraphFollowerScene>.Get().CeraphIsFollower())
                levelOfFuckedness -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.OwcaSacrificeDisabled) == 1)
                levelOfFuckedness -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.SandWitchesFriendly) == 1)
                levelOfFuckedness -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaQuestStatus) == 1)
                levelOfFuckedness -= 10;

            if (null  != "")
                levelOfFuckedness -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionFirstDate) != 0)
                levelOfFuckedness += 20;

            if (CoC.Instance.Flags.Get(GameFlagIds.KidAXp) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.AnemoneWeaponId) != "")
                levelOfFuckedness -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) > 3)
                levelOfFuckedness += 15;

            if (CoC.Instance.Flags.Get(GameFlagIds.EdrynNumberOfKids) != 0)
                levelOfFuckedness -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonKidCount) != 0)
                levelOfFuckedness -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) != 0)
                levelOfFuckedness -= 20;
            // 原版 LethiceScenes.hx:899 camp.followerKiha()
            if (SceneInstance<Npcs.KihaFollowerScene>.Get().FollowerKiha())
                levelOfFuckedness -= 15;
            // 原版 LethiceScenes.hx:902 jojoScene.jojoFollower()
            if (SceneInstance<Npcs.JojoScene>.Get().JojoFollower())
                levelOfFuckedness -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) == 4)
                levelOfFuckedness -= 5;
            // 原版 LethiceScenes.hx:908 rathazul.followerRathazul()
            if (Npcs.MarbleScene.FollowerRathazul())
                levelOfFuckedness -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0)
                levelOfFuckedness -= 20;

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) >= 100)
                levelOfFuckedness -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.MinervaPurificationProgress) >= 10)
                levelOfFuckedness -= 15;

            return levelOfFuckedness;
        }

        public void PurgeShit()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.PurgeShit.ArmyOfFriends".Localized());

            if (Player.HasChildren())
                OutputText("Scenes.Lethicescenes.PurgeShit.OffspringWord".Localized());

            OutputText("Scenes.Lethicescenes.PurgeShit.AndLastPureForces".Localized());

            var levelOfFuckedness = HowFuckedIsThePlayerOMeter();

            if (levelOfFuckedness <= 50)
            {
                OutputText("Scenes.Lethicescenes.PurgeShit.ByNecessityCampaignBrutal".Localized());

                OutputText("Scenes.Lethicescenes.PurgeShit.CampaignWagesOn".Localized());

                OutputText("Scenes.Lethicescenes.PurgeShit.InTimePeaceCame".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0)
                    OutputText("Scenes.Lethicescenes.PurgeShit.DemiseOfMarae".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) != 2)
                        OutputText("Scenes.Lethicescenes.PurgeShit.ReclaimedTheFullNetwork".Localized());
                    else
                        OutputText("Scenes.Lethicescenes.PurgeShit.FoughtForATime".Localized());

                OutputText("Scenes.Lethicescenes.PurgeShit.YouEndedDemonThreat".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndPurge0To50);
            }
            else
                if (levelOfFuckedness <= 100)
                {
                    OutputText("Scenes.Lethicescenes.PurgeShit.ByNecessityAgain".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.NotLetThatDeter".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.ParagraphBreak".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedMaraeKilled) > 0)
                        OutputText("Scenes.Lethicescenes.PurgeShit.WithDemiseOfMarae".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) != 2)
                            OutputText("Scenes.Lethicescenes.PurgeShit.MaraeWonderfulAid".Localized());
                        else
                            OutputText("Scenes.Lethicescenes.PurgeShit.AlwaysThereAlwaysLending".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.SlowlyButSteadily".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.WellIntoYourFifties".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) != 2)
                        OutputText("Scenes.Lethicescenes.PurgeShit.ThoseFewPockets".Localized());
                    else
                        OutputText("Scenes.Lethicescenes.PurgeShit.FewPlacesTainted".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.PeaceReturnsToLand".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.ChampionsThatFollowed".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndPurge50To100);
                }
                else
                {
                    OutputText("Scenes.Lethicescenes.PurgeShit.ByNecessityThird".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.DemonsOnceAble".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.FightThemSeemingly".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.GrowOldToSound".Localized());

                    OutputText("Scenes.Lethicescenes.PurgeShit.EventuallyYouDie".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndPurge100);
                }

            OutputText("Scenes.Lethicescenes.PurgeShit.TheEnd".Localized());

            DoNext(() => PostEndingChoices());
        }

        public void Conquer()
        {
            ClearOutput();

            if (Player.Corruption <= 33)
            {
                OutputText("Scenes.Lethicescenes.Conquer.CouldDefinitelyRunPlace".Localized());

                if (CompanionsCount() > 0)
                {
                    OutputText("Scenes.Lethicescenes.Conquer.GatherYourFollower".Localized());

                    if (CompanionsCount() > 1)
                        OutputText("Scenes.Lethicescenes.Conquer.SLetter".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.InTheCitadelItself".Localized());
                }

                OutputText("Scenes.Lethicescenes.Conquer.FamiliarizeYourself".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.WordGetsOut".Localized(
                    Player.Mf("King", "Queen")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.LethiceKilled) == 0)
                    OutputText("Scenes.Lethicescenes.Conquer.LethiceBroughtBack".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.OrderTheWorst".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.UnderYourHeel".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.SoWhatIfTheres".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.GetUsedToIt".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.ThenYouWonderAgain".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.TightenYourRule".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.ChangesAreSlow".Localized());

                OutputText("Scenes.Lethicescenes.Conquer.WonAtWhatCost".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndConquerLow);
            }
            else
                if (Player.Corruption <= 66)
                {
                    OutputText("Scenes.Lethicescenes.Conquer.TrulyDidntKnowWhat".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.SureSeeAppeal".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.FirstStepIsMoving".Localized());

                    if (CompanionsCount() > 0)
                        OutputText("Scenes.Lethicescenes.Conquer.YourFollowersFrom".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.CampAwayFromPortal".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.ConvincingThemToWork".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.OneOfFirstOrders".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.AllGoesWell".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.CollectSteep".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.UsedToItAgain".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.ThenYouWonderAgain".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.TightenRuleAgain".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.ChangesSlowAgain".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.YouveWonAgain".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndConquerMed);
                }
                else
                {
                    OutputText("Scenes.Lethicescenes.Conquer.ForAllHerMight".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.WontMakeTheSame".Localized());

                    if (CompanionsCount() > 0)
                    {
                        OutputText("Scenes.Lethicescenes.Conquer.AndFollower".Localized());

                        if (CompanionsCount() > 1)
                            OutputText("Scenes.Lethicescenes.Conquer.SLetterAgain".Localized());
                    }

                    OutputText("Scenes.Lethicescenes.Conquer.WithRuleBrokenThere".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.ThatNightSendWord".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.OnlyWayToKeep".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.InTheShortSpan".Localized());

                    OutputText("Scenes.Lethicescenes.Conquer.AgeOnlyProblem".Localized(
                        Player.Mf("King", "Queen")
                    ));

                    CoC.Instance.Flags.Set(GameFlagIds.GameEnd, GameEndConquerHigh);
                }

            OutputText("Scenes.Lethicescenes.Conquer.TheEnd".Localized());

            DoNext(() => PostEndingChoices());
        }

        public void PostEndingChoices()
        {
            ClearOutput();

            ShowImage("event-game");

            OutputText("Scenes.Lethicescenes.PostEndingChoices.WhatWouldYouDo".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.GameEnd) == GameEndConsort)

                AddButtonDisabled(0, "Continue", "");
            else
                AddButton(0, "Common.Button.Continue".Localized(), PostEndingReturn);

            AddButton(14, "MainMenu.Button.Quit".Localized(), QuitToMenu);
        }

        public void PostEndingReturn()
        {
            ClearOutput();

            OutputText("Scenes.Lethicescenes.PostEndingReturn.MayHaveDefeatedLethice".Localized());

            OutputText("Scenes.Lethicescenes.PostEndingReturn.YouCanNowAscend".Localized());

            AwardAchievement(AchievementIds.StoryFinalboss, true, true, false);

            if (Player.Level <= 1)
            AwardAchievement(AchievementIds.ChallengeUltimateNoob, true, true, false);

            if (!Player.HasSpells())
            AwardAchievement(AchievementIds.ChallengeUltimateMundane, true, true, false);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMasturbated) <= 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesOrgasmed) <= 0)
            AwardAchievement(AchievementIds.ChallengeUltimateCelibate, true, true, false);

            if (CoC.Instance.Flags.Get(GameFlagIds.TotalHpVictories) <= 0 && TotalKills() <= 0)
            AwardAchievement(AchievementIds.ChallengePacifist, true, true, false);

            if (View.Time.Days <= 30 && Player.NewGamePlusMod() == 0)
            AwardAchievement(AchievementIds.ChallengeSpeedrun, true, true, false);

            Contents.Dungeons.RoomDungeon.Exit();

            Player.Health = Player.MaxHealth;

            Player.Lust = (Math.Max(0f, MathF.Floor((1 + Player.Stats.Addition(GameStats.MinLibido)) * Player.Stats.Multiplier(GameStats.MinLibido))));

            Player.Fatigue = 0;

            Player.Hunger = 100;

            View.Time.Hours = 23;

            ShowImage("item-lWhip");

            InventoryScene.TakeItem(ItemIds.LethicesWhip, () => ReturnToCamp(8));
        }

        public void QuitToMenu()
        {

            Player.Loaded = false;

        }

        [Register("coc:keep_hook_lethice")]
        public static void KeepHookLethice() =>

            SceneInstance<LethiceScenes>.Get().EncounterLethice();

    }
}