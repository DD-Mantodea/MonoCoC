using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Scenes.Vanilla.Places;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<SophieBimboScene>("coc:sophie_bimbo")]
    public class SophieBimboScene : Scene
    {

        public static readonly string[] eggColors = ["Black", "Blue", "Brown", "Pink", "Purple", "White"];

        public static SaveContent Save => SaveContent(SceneIds.Npcs.SophieBimbo);

        public string debugName => "Sophie";

        public string debugHint => "";

        public bool pregnancy => SceneInstance<SophieScene>.Get().Pregnancy.IsPregnant;

        public override void Entry()
        {
            SophieSprite();
        }

        public void Reset()
        {

            Save.SetBool("cuddlingDaughter", false);

            Save.SetInt("daughterRocked", 0);

            Save.SetInt("daughterMarried", 0);
        }

        public void SophieSprite()
        {

        }

        public bool BimboSophie()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) == 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDebimboed) == 0;
        }

        public bool BimboSophieAtCamp()
        {

            return BimboSophie() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0;
        }

        public bool SophieIsInSeason()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.SophieHeatCounter) > 551 && !Player.IsPregnant();
        }

        public void SophiePregChance()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) != 0)
                return ;

            if (Random.Shared.Next(4) == 0 || Player.CumQ() > Random.Shared.Next(1000))
                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.HarpyIncubation, 100, 1);
        }

        public float SophieCapacity()
        {

            return 179;
        }

        public int SophieChildren()
        {

            int temp = CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount);

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0)
                temp += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieCampEggCountdown) > 0)
                temp += 1;

            return temp;
        }

        public void BimbotizeMeCaptainSophie()
        {
            ClearOutput();

            SophieSprite();

            Player.ConsumeItem(ItemIds.BimboLiqueur);

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.WickedIdeaTakesHold".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.HarpyTouchHerselfChoice".Localized());
            else
                OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.SquirmingInNestDirt".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.OpensPouchForBottle".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.LustWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.PainWord".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.GlazedEmptyVisage".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.CorkPopsPinkMist".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.PlumageTurnsNeonPink".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.ShowsWarningLabel".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.BreastsSwellBigger".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.HeftHerHeavyTit".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.OversensitiveSqueal".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.ThumbCirclesAreola".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.OffersRumpAndTail".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.FingerDownHerSlit".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.GigglesAboutDripping".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.HoneySmearOnMouth".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.SlimyOrgasmCollapse".Localized(
                Player.Mf("handsome", "hot")
            ));

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.RealizingSheNeedsYou".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.MakeHerYourToy".Localized());

            OutputText("Scenes.Sophiebimbo.BimbotizeMeCaptainSophie.ResistingCompulsion".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));

            AddButton(0, "Common.Button.Yes".Localized(), AcceptBimboSophie);

            AddButton(1, "Common.Button.No".Localized(), DeclineBimboSophie);
        }

        public void DeclineBimboSophie()
        {
            ClearOutput();

            SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SophieDisabledForever, 1);

            OutputText("Scenes.Sophiebimbo.DeclineBimboSophie.FleeFromMountain".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void AcceptBimboSophie()
        {
            ClearOutput();

            SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SophieBimbo, 1);

            if (Player.IsPregnant())
                Player.KnockUp(Player.Pregnancy.Type, (int)(Player.Pregnancy.Incubation * 1.75f));

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.SophieSpringsForward".Localized());

            if (Player.CanFly())
                OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.WingsPinnedBack".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.SophieGaspsThankfully".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.DirectGlideToCamp".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.BimboSpotsCampsite".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.ShowBimboAround".Localized());

            if (SceneInstance<RathazulScene>.Get().FollowerRathazul())
                OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.OffersSuckRathazul".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1)
                    OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.OffersSuckIzma".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyBlockCountdownBecauseCorruptedJojo) == 0)
                    {
                        OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.OffersLickAmily".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
                            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.AmilyDeniesRequest".Localized());
                        else
                            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.AmilyWigglesHips".Localized());
                    }
                    else
                        if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.OffersSuckJojo".Localized());
                        else
                            if (FollowerRegistry.HasFollower(FollowerIds.JojoFollower))
                                OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.OffersSuckChasteJojo".Localized());
                            else
                                if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                                {
                                    OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.OffersLickIsabella".Localized());

                                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                        OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.IsabellaSaysNein".Localized());
                                    else
                                        OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.IsabellaSternRefusal".Localized());

                                    OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.CowGirlBlushesHotly".Localized());
                                }

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.FindBimboAnytime".Localized());

            OutputText("Scenes.Sophiebimbo.AcceptBimboSophie.BimboSophieAddedSlaves".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void SophieCampLines()
        {

            float choice = Random.Shared.Next(4);

            if (silly)
                choice = Random.Shared.Next(5);

            if (choice == 0)
                OutputText("Scenes.Sophiebimbo.SophieCampLines.ApplyingLipGloss".Localized());
            else
                if (choice == 1)
                    OutputText("Scenes.Sophiebimbo.SophieCampLines.DiddlingHerTwat".Localized());
                else
                    if (choice == 2)
                        OutputText("Scenes.Sophiebimbo.SophieCampLines.HarpyPreensVapidly".Localized());
                    else
                        if (choice == 3)
                            OutputText("Scenes.Sophiebimbo.SophieCampLines.SophiePlaysWithBreasts".Localized());
                        else
                            OutputText("Scenes.Sophiebimbo.SophieCampLines.SophieTakesSelfie".Localized());

            OutputText("Scenes.Sophiebimbo.SophieCampLines.ParagraphBreak".Localized());
        }

        public void ApproachBimboSophieInCamp(bool output = true)
        {
            ClearOutput();

            SophieSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
            {
                if (output)
                    if (SophieIsInSeason() && Player.HasCock())
                    {
                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.SophieTrotsOver".Localized());

                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.ThighsWrapAroundYou".Localized());

                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.HarpySqueezesLegs".Localized());

                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.SingsongCockTease".Localized());

                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.CompulsionWeakening".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 20f));
                    }
                    else
                        if (Player.IsPregnant())
                        {
                            switch (Player.Pregnancy.Event)
                            {
                                case 3:
                                {
                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.SophieLiesOnBedroll".Localized());

                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.ShowsOffEggBelly".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                                }

                                break;

                                case 4:
                                {
                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.PregnantBimboLounges".Localized());

                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.HarpyLeapsFromBed".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                                }

                                break;
                            }

                            if (true )
                            {
                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.SophieLikesYourBedroll".Localized());

                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.NoticesYourGaze".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, 3f));
                            }
                        }
                        else
                        {
                            float choice = Random.Shared.Next(3);

                            if (choice == 0)
                            {
                                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.WingsKickUpDust".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                            }
                            else
                                if (choice == 1)
                                {
                                    OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.HarpyReclinesAkimbo".Localized());

                                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                                }
                                else
                                {
                                    OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.LeansDownHidingBreasts".Localized());

                                    OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.EnjoyingTheShow".Localized());

                                    if (Player.HasCock())
                                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.CockPillowsSlurping".Localized());
                                    else
                                        if (Player.HasVagina())
                                            OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.PussyPillowsCrushing".Localized());
                                        else
                                            OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.PillowsLockingEmbrace".Localized());

                                    if (Player.Corruption < 33)
                                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.NoddingAndBlushing".Localized());
                                    else
                                        if (Player.Corruption < 66)
                                            OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.NoddingLewdGrin".Localized());
                                        else
                                            OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.VigorousNodLeer".Localized());

                                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                                }
                        }
            }
            else
                if (output)
                {
                    SophieSprite();

                    OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.EmergesFromHenCoop".Localized());

                    OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.PatsVastBottom".Localized());
                }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), SophieBimboAppearance);

            if (Player.Lust < 33 && output)
                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.NotArousedEnough".Localized());

            AddButton(1, "Common.Button.Sex".Localized(), BimboSophieSexMenu);

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieCampEggCountdown) > 0 && output)
                OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.SophieEggNearby".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0)
                AddButton(5, "Common.Button.Daughter".Localized(), DaughterCheckup);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.DeBimbo)))
            {
                AddButton(6, "Common.Button.Debimbo".Localized(), SceneInstance<SophieFollowerScene>.Get().UnbimboSophie);

                if (output)
                    OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.UseDebimboBottle".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBroachedSleepWith) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Sophie")
                {
                    if (output)
                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.InviteToShareBed".Localized());

                    AddButton(9, "Common.Button.SleepWith".Localized(), MoveSophieInRepeat);
                }
                else
                {
                    if (output)
                        OutputText("Scenes.Sophiebimbo.ApproachBimboSophieInCamp.SharingBedCanKick".Localized());

                    AddButton(9, "Common.Button.NoSnuggles".Localized(), NoSophieSleepWith);
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0 && !Player.IsPregnant() && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) != 0)
                AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) != 0 && CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionSophie) == 0)
                AddButton(11, "Common.Button.HarvestEggs".Localized(), HarvestEggs);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) != 0 && CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionSophie) == 1)
            {
                AddButton(11, "Common.Button.ChangeEggs".Localized(), ChangeEggs);

                AddButton(12, "Common.Button.StopHarvest".Localized(), StopHarvest);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
                AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
            else
                AddButton(14, "Common.Button.Back".Localized(), null );
        }

        public void SophieBimboAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.BuxomHarpyDescribed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.VeryWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.LongWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.PlatinumBlondWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.PinkWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.FeatheryHairPlumage".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.SuperSizedWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.GlossyCompellingLips".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.BreastSizeDescription".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.ImmenseGroundingBreasts".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.UnusuallyLargeBreasts".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.GlitteringMoistPussy".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.FeatheredArmWings".Localized());

            if (FarmCorruption.hasTattoo("sophie"))
            {
                OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.ParagraphBreak".Localized());

                if (FarmCorruption.sophieFullTribalTats())
                    OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.TribalTattooCoverage".Localized());
                else
                {
                    if (FarmCorruption.numTattoos("sophie") > 1)
                        OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.FollowingTattoosList".Localized());
                    else
                        OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.SheHasPrefix".Localized());

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooCollarbone))
                        OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.PlaceholderNewline".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooCollarbone)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooShoulders))
                        OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.PlaceholderNewlineAgain".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooShoulders)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooLowerback))
                        OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.PlaceholderNewlineThird".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooLowerback)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooButt))
                        OutputText("Scenes.Sophiebimbo.SophieBimboAppearance.PlaceholderNewlineFourth".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooButt)
                        ));
                }
            }

            ClearButtons();

            AddButton(0, "Common.Button.Back".Localized(), () => ApproachBimboSophieInCamp(false));
        }

        public void HarvestEggs()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.HarvestEggs.AskAboutEggColors".Localized());

            OutputText("Scenes.Sophiebimbo.HarvestEggs.PointsOutEggColors".Localized());

            EggSelector();
        }

        public void ChangeEggs()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.ChangeEggs.RequestEggChange".Localized());

            OutputText("Scenes.Sophiebimbo.ChangeEggs.SureThingGiggle".Localized());

            EggSelector();
        }

        public void EggSelector()
        {

            var i = 0;

            while (i < eggColors.Length)
            {
                AddButton(i, eggColors[i], () => PostEggSelector(eggColors[i]));

                i += 1;
            }
        }

        public void PostEggSelector(string selected)
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.PostEggSelector.PromisesNiceEggs".Localized(
                selected.ToLower()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionSophie, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionSophieColorchoice) != selected)
            {
                CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionSophieColorchoice, selected);

                CoC.Instance.Flags.Set(GameFlagIds.FarmEggStored, 0);

                CoC.Instance.Flags.Set(GameFlagIds.FarmEggCountdown, 7);
            }

            DoNext(FarmCorruption.RootScene);
        }

        public void StopHarvest()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.StopHarvest.TellSophieStopEggs".Localized());

            if (silly)
                OutputText("Scenes.Sophiebimbo.StopHarvest.WishesFinancialAcumen".Localized());
            else
                OutputText("Scenes.Sophiebimbo.StopHarvest.WistfulSighBellybutton".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionSophie, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void SendToFarm()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SendToFarm.SendBimboToFarm".Localized());

            OutputText("Scenes.Sophiebimbo.SendToFarm.AsksWhatIsFarm".Localized());

            OutputText("Scenes.Sophiebimbo.SendToFarm.DiagramsInDust".Localized());

            OutputText("Scenes.Sophiebimbo.SendToFarm.ExcitedAboutAdventure".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmSophie, 1);

            ReturnToCamp(1);
        }

        public void BackToCamp()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.BackToCamp.SendHerBackHome".Localized());

            OutputText("Scenes.Sophiebimbo.BackToCamp.BriefDisappointment".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmSophie, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void BimboSophieSexMenu()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.BimboSophieSexMenu.HowSexWithSophie".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(SophieCapacity()) && Player.Lust >= 33)
                    AddButton(2, "Common.Button.Anal".Localized(), ButtFuckBimboSophie);
                else
                    OutputText("Scenes.Sophiebimbo.BimboSophieSexMenu.ButtTooTight".Localized());

                if (Player.Pregnancy.Event >= 2)
                    AddButton(5, "Common.Button.TittyFuck".Localized(), SophiePreggoTitJobs);
                else
                    AddButton(5, "Common.Button.TittyFuck".Localized(), FuckBimboSophiesTits);

                if (Player.HasCockThatFits(SophieCapacity()))
                {
                    if (SophieIsInSeason())
                        AddButton(0, "Common.Button.Impregnate".Localized(), SophieImpregnationSex);
                    else
                        AddButton(0, "Common.Button.NiceFuck".Localized(), () => SophieFenCraftedSex());

                    AddButton(1, "Common.Button.RoughFuck".Localized(), () => RideDatSophieVag());
                }
                else
                    OutputText("Scenes.Sophiebimbo.BimboSophieSexMenu.CuntTooTight".Localized());

                if (Player.BiggestCockArea() > SophieCapacity() && SophieIsInSeason())
                {
                    OutputText("Scenes.Sophiebimbo.BimboSophieSexMenu.ImpregnateBiggest".Localized());

                    AddButton(6, "Common.Button.BigImpreg".Localized(), GiantCocksImpregnateSophie);
                }
            }

            if (Player.HasVagina())
                AddButton(3, "Common.Button.GetLicked".Localized(), BimboSophieLicksRugs);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSophieHasDrunkOviElixir) > 0 && ((int)Player.Gender > 0) && !Player.IsPregnant())
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)) || Player.HasItemInStorage(ConsumableIds.OviElixir))
                    AddButton(4, "Items.OviElixir.Short".Localized(), SophieEggApocalypse);
                else
                    OutputText("Scenes.Sophiebimbo.BimboSophieSexMenu.OvipositionElixirHint".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ToldSophieToIzma) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesSophieAndIzmaFucked) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Sophiebimbo.BimboSophieSexMenu.VisitIzmaAgainHint".Localized());

                AddButton(7, "Common.Button.VisitIzma".Localized(), TellSophieToVisitIzma);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieFamilyIncest) > 0 && Player.HasCockThatFits(SophieCapacity()))
                AddButton(8, "Common.Button.Daughterfuck".Localized(), SceneInstance<SophieFollowerScene>.Get().SophieIncestInHerCooterOrSomethingIDunno);

            AddButton(14, "Common.Button.Leave".Localized(), () => ApproachBimboSophieInCamp());
        }

        public void TellSophieToVisitIzma()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.TellSophieToVisitIzma.SuggestIzmaVisit".Localized());

            OutputText("Scenes.Sophiebimbo.TellSophieToVisitIzma.SophieSquealsAway".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ToldSophieToIzma, 1);

            ReturnToCamp(1);
        }

        public void BimboSophieLustStickSurprise()
        {

            OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.ParagraphBreak".Localized());

            float select = Random.Shared.Next(4);

            if (select == 0)
            {
                SophieSprite();

                OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.JoyfulEmbrace".Localized());

                if (!Player.HasCock())
                {
                    OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.PryBimboFromArms".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 1f));
                }
                else
                {
                    OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.ShiverGrowingHard".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                }

                SceneInstance<SophieScene>.Get().LuststickApplication(5);
            }
            else
                if (select == 1)
                {
                    SophieSprite();

                    OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.IdleCaressFromBehind".Localized());

                    if (!Player.HasCock())
                    {
                        OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.SlapAssSoon".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 1f));
                    }
                    else
                    {
                        OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.AvoidCockContact".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                    }

                    SceneInstance<SophieScene>.Get().LuststickApplication(5);
                }
                else
                    if (select == 2)
                    {
                        SophieSprite();

                        if (Player.HasCock())
                        {
                            OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.WaterSipInterrupted".Localized(
                                Player.MultiCockDescript()
                            ));

                            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                        }
                        else
                        {
                            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                            OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.SlideOverCock".Localized());

                            if (Player.HasVagina())
                                OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.ClitoralHoodSuck".Localized());
                            else
                                OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.GroinLickAsshole".Localized());
                        }

                        SceneInstance<SophieScene>.Get().LuststickApplication(5);
                    }
                    else
                    {
                        SophieSprite();

                        OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.RockGlideLanding".Localized());

                        if (Player.HasCock())
                        {
                            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

                            OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.WiggleInsideArmor".Localized(
                                Player.SMultiCockDesc()
                            ));
                        }

                        OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.NodAndSheLeaves".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 2f));

                        SceneInstance<SophieScene>.Get().LuststickApplication(5);
                    }

            OutputText("Scenes.Sophiebimbo.BimboSophieLustStickSurprise.ParagraphBreakAgain".Localized());
        }

        public void DaughterCheckup()
        {
            ClearOutput();

            if (View.Time.Hours >= 22 && Save.GetInt("daughterRocked", 0) <= View.Time.Days + 7)
                DaughterRock();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 200)
                    OutputText("Scenes.Sophiebimbo.DaughterCheckup.ChickStillSmall".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 100)
                        OutputText("Scenes.Sophiebimbo.DaughterCheckup.HarpyGrowingUp".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0)
                        {
                            OutputText("Scenes.Sophiebimbo.DaughterCheckup.FindDaughterSophie".Localized());

                            if (BimboSophie())
                                OutputText("Scenes.Sophiebimbo.DaughterCheckup.BoisterousMother".Localized());
                            else
                                OutputText("Scenes.Sophiebimbo.DaughterCheckup.MatronlyMother".Localized());

                            OutputText("Scenes.Sophiebimbo.DaughterCheckup.QualityMotherTime".Localized());

                            if (BimboSophie())
                                OutputText("Scenes.Sophiebimbo.DaughterCheckup.CohabitingBimbo".Localized());
                            else
                                OutputText("Scenes.Sophiebimbo.DaughterCheckup.BelovedBirdWoman".Localized());

                            OutputText("Scenes.Sophiebimbo.DaughterCheckup.HairAndNailsGossip".Localized());

                            OutputText("Scenes.Sophiebimbo.DaughterCheckup.DaughterGrowingFast".Localized());
                        }

            if (Save.GetInt("daughterMarried", 0) >= CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) + 1)
                OutputText("Scenes.Sophiebimbo.DaughterCheckup.ParagraphBreak".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Pat".Localized(), DaughterPat);

            AddNextButton("Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), DaughterCuddle);

            AddNextButton("Combat.Ability.IvorySuccubusFlight.Name".Localized(), DaughterFlight);
            // 原版 SophieBimbo.hx:693 — allowChild 时才出现「Cunnilingus」与「Play Marriage」
            if (GameSettings.AllowChild)
            {
                AddNextButton("Common.Button.Cunnilingus".Localized(), DaughterCunny);

                AddNextButton("Common.Button.PlayMarriage".Localized(), DaughterMarriage);
            }

            AddButton(14, "Common.Button.Back".Localized(), BimboSophie() ? () => ApproachBimboSophieInCamp() : () => SceneInstance<SophieFollowerScene>.Get().FollowerSophieMainScreen());
        }

        public void DaughterPat()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterPat.CallDaughterOver".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterPat.EagerChickGreeting".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterPat.CheeksPuffLove".Localized());

            if (BimboSophie()) ApproachBimboSophieInCamp(); else SceneInstance<SophieFollowerScene>.Get().FollowerSophieMainScreen();
        }

        public void DaughterCuddle()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterCuddle.WaveToHarpy".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCuddle.CriesAndGoesLimp".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCuddle.DropFromShoulders".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCuddle.AffectionateNuzzle".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCuddle.NuzzleBackKiss".Localized());

            Save.SetBool("cuddlingDaughter", true);

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "harpy");

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
        }

        public void DaughterAwaken()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterAwaken.MorningChirpingWake".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterAwaken.BirdieGoodMorning".Localized());

            Save.SetBool("cuddlingDaughter", false);

            Go<CampScene>();
        }

        public void DaughterCunny()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterCunny.BeckonDaughterLeap".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.DaughterAsksWanted".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.KissDownToChest".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.ShudderAtStimulus".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.ThighHeatEmbrace".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.MumblesWarmTingly".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.TongueTwirlNub".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunny.RelaxInAfterglow".Localized());

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DaughterCunnyContinuation();
        }

        public void DaughterCunnyContinuation()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterCunnyContinuation.EyesOpenAfterTime".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterCunnyContinuation.MileWideGrin".Localized());

            ReturnToCamp(1);
        }

        public void DaughterFlight()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterFlight.EmbraceYoungChick".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterFlight.GigglingBirdGirl".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterFlight.EyesLightUpFlight".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterFlight.FlapWingsLeap".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterFlight.LoudGreetingMom".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterFlight.WindJostlesCamp".Localized());

            if (Player.Strength < 80)
                Player.ApplyStats(new DynStat(GameStats.Strength, 1f));

            Player.ChangeFatigue(25);

            ReturnToCamp(1);
        }

        public void DaughterRock()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterRock.SunsetStillPlaying".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterRock.CloseEnoughHug".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterRock.NotTiredProtest".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterRock.BetterIdeaRocking".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterRock.RockUntilAsleep".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterRock.ForeheadKissGoodnight".Localized());

            Save.SetInt("daughterRocked", View.Time.Days);

            ReturnToCamp(1);
        }

        public void DaughterMarriage()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.FiddleWithRing".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.DemonstrateMarriage".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.HarpyDashesOver".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.ConfusedWhassat".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.FeelingsEvidentSmile".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.HopWithJoy".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.TouchedByEnthusiasm".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.AsksHowToStart".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.VowsUndecided".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.SpeakLongVows".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.PraisePuffsHer".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.MagicWordsIDo".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.SlipRingOnFinger".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.ShoutsWaitHere".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.TurnsBackToGround".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.WillYouTakeMe".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.GrassRingPetals".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.HandmadeRingFit".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.ChasteKiss".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.PressCloseToBreast".Localized());

            OutputText("Scenes.Sophiebimbo.DaughterMarriage.TimelessEmbrace".Localized());

            Save.SetInt("daughterMarried", CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) + 1);

            foreach (var item in new[] { ItemIds.SilverRing, ItemIds.GoldRing, ItemIds.PlatinumRing, ItemIds.DiamondRing })
                if (Player.DestroyItems(item))
                    break;

            ReturnToCamp(1);
        }

        public void UnlockResistance()
        {

            if (Player.HasCock())
                OutputText("Scenes.Sophiebimbo.UnlockResistance.LipstickEffectsFailing".Localized());
            else
                OutputText("Scenes.Sophiebimbo.UnlockResistance.LipstickImmunityExplanation".Localized());

            OutputText("Scenes.Sophiebimbo.UnlockResistance.LuststickAdaptedPerkUnlocked".Localized());

            Player.CreatePerk(PerkIds.LuststickAdapted, 0, 0, 0, 0);
        }

        public void BimboSophieLicksRugs()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SitBackSpreadLegs".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.LiftSackAside".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.GiveSophiePerfectView".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.NodNervously".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.NodAndSmile".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.ConfidentSmileScoot".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SophieStaresAtLabia".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.MoistWord".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.WetWord".Localized());
                else
                    if (Player.Wetness() < 5)
                        OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SloppyWord".Localized());
                    else
                        OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SoakedWord".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.HarpyExtendsTongue".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.LoinsFireworksShiver".Localized());

            if (Player.GetClitLength() < 3)
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.LipsExposeClit".Localized());
            else
                if (Player.GetClitLength() < 7)
                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.LipsExposeSmallButton".Localized());
                else
                {
                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.LipsExposeChickStick".Localized());

                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SophieTeasesBigGirl".Localized());

                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.HarpyResumesOnslaught".Localized(
                        Player.ClitDescript()
                    ));
                }

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SophiePurrsFingering".Localized());

            if (Player.GetClitLength() < 3)
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.HumsDoublesLicking".Localized());
            else
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.PumpsDoublesTongue".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SightPushesOverEdge".Localized());

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SquirtWord".Localized());
            else
                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.GushWord".Localized());
                else
                    if (Player.Wetness() >= 3)
                        OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.LeakWord".Localized());
                    else
                        if (Player.Wetness() >= 2)
                            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.DribbleWord".Localized());
                        else
                            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.DroolWord".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SpunkMarksSophieFace".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.BothFallBackWinded".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.GoodThingNoPenis".Localized());
            else
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.GroinBurnsForCocks".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.DitzyBlondGiggles".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SophieWantsFuckNow".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SophiePleadsForMore".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.ThickJuicyDick".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.SixtyNineInvitation".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.AndHardWord".Localized());

            OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.WantsToTackleHer".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            SceneInstance<SophieScene>.Get().LuststickApplication(4);

            if (Player.Intelligence * 2 < Player.Corruption + Player.Libido)
            {
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.CannotTurnHerDown".Localized());

                BimboSophieSixtyNineAfterCunnilingus();
            }
            else
            {
                OutputText("Scenes.Sophiebimbo.BimboSophieLicksRugs.AwaitPlayerChoice".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Sixtynine".Localized(), BimboSophieSixtyNineAfterCunnilingus);

                SetExitButton("Common.Button.Back".Localized());
            }
        }

        public void BimboSophieSixtyNineAfterCunnilingus()
        {
            ClearOutput();

            SophieSprite();

            if (!Player.HasCock() || Player.CockThatFits(SophieCapacity()) == -1)
            {
                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.ThrowYourselfAtSophie".Localized());

                if (Player.Pregnancy.Event >= 2)
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.PregnantBellyRests".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.LookUpTarget".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.GrabAssCheeks".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophiePraisesLicking".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.DropsOntoElbows".Localized());

                if (Player.GetClitLength() < 1)
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.TinyWord".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.MoanIntoHerPussy".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.MutualPussyFrenzy".Localized());

                if (Player.GetClitLength() >= 3)
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.JackItLikePenis".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.HastyNeedfulStrokes".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophiePartsLipsPop".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.BimboClimaxesOnYou".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.HarpyLaysAtopYou".Localized());

                if (Player.Pregnancy.Event >= 2)
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.EggSwollenBellySmush".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophieGoodbyeKiss".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophieStrutsOffTasting".Localized());

                SceneInstance<SophieScene>.Get().LuststickApplication(5);

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));
            }
            else
            {
                int x = Player.CockThatFits(SophieCapacity());

                if (x < 0)
                    x = Player.SmallestCockIndex();

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.ThrowYourselfPicturing".Localized());

                if (Player.Pregnancy.Event >= 2)
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.GravidBellyBumps".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.LookUpTargetAgain".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.GrabAssCheeksAgain".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.PraisesPussyLicking".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.DropsOntoElbowsAgain".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.HarpyTeasesUrethra".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SqueezeAssCramCunt".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.BimboMouthIsHeaven".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophiePopsOutLuridly".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.ImperativesTriggerPrecum".Localized(
                    Player.MultiCockDescript(),

                    Player.CockDescript(x),

                    Player.CockHead(x)
                ));

                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.GulpsCumFromNose".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.PaintsRemainderOnFace".Localized());
                }

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.ParagraphBreak".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophieJumpsUpGiggling".Localized());

                if (Player.Pregnancy.Event >= 2)
                    OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.EggStuffedWombJiggles".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.HarpyLubeWebStretches".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.BlondBombshellTitters".Localized());

                OutputText("Scenes.Sophiebimbo.BimboSophieSixtyNineAfterCunnilingus.SophieSmearsLipstick".Localized(
                    Player.SMultiCockDesc()
                ));

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -1f));

                SceneInstance<SophieScene>.Get().LuststickApplication(5);
            }

            ReturnToCamp(1);
        }

        public void ButtFuckBimboSophie()
        {
            ClearOutput();

            SophieSprite();

            int x = Player.CockThatFits(SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.BimboBendOverCommand".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.BimboBouncesHerRump".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.StripArmorAndAgree".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SophieClapsInDelight".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.PluralCockDescription".Localized());
            else
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SingularCockDescription".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SlapAssAfterTasting".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SqueezeShudderingRump".Localized(
                Player.CockDescript(x)
            ));

            if (!Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.HiltWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SheathWord".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SophieBreastsSwayLow".Localized());

            if (Player.Pregnancy.Event >= 2)
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.GravidTummySways".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.BimboMoansLouder".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.OverBallsPosition".Localized());
            else
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.AcrossTaintPosition".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.TongueCrossesTaint".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.BreathlessBimboRocks".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.UnsheatheMostLingering".Localized(
                NumberUtils.Num2Text(Player.Cocks[x].Length),

                Player.CockHead(x),

                Player.Mf("stud", "babe")
            ));

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.NaughtyBirdBitchPraise".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.KeepFuckingHardFast".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.BallsSeizeAtRequest".Localized());
            else
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.InsidesSeizeAtRequest".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SeedSpewsIntoBottom".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.FrothingSemenKeepsPushing".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.FillBellyWithSpooge".Localized());

                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.SentencePeriod".Localized());
            }

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.FinalClenchesDribble".Localized());

            OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.DitzBouncesForCum".Localized());

            if (Player.Pregnancy.Event >= 2)
                OutputText("Scenes.Sophiebimbo.ButtFuckBimboSophie.GigglesRubbingFullBelly".Localized());

            if (SophieIsInSeason())
                SophiePregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void FuckBimboSophiesTits()
        {
            ClearOutput();

            SophieSprite();

            int x = Player.CockThatFits(SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.GrabSophieTitsFirmly".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.HarpyRemovesArmor".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.PullNipplesOntoKnees".Localized());

            if (Player.Pregnancy.Event >= 2)
                OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.SwollenTummyPressesLegs".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.WobbleForwardEngulf".Localized());

            if (Player.Cocks[x].Thickness < 8)
                OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.FullEngulfmentPhrase".Localized());
            else
                OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.MaximumEngulfmentPhrase".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.WarmSkinScentStartles".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.BimboAsksConfused".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.SophieLipsSuckTip".Localized(
                Player.CockHead(x),

                Player.CockDescript(x),

                NumberUtils.Num2Text(Player.Cocks[x].Length),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.SlipperyLipsSlideBack".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.DemurThenPumpHips".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.EyesClosedInBliss".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.BlondeHandStrokesShaft".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.BallBreaksHandjob".Localized());
            else
                OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.TaintAndAssholeTease".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.ShaftThickensBloats".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.SophieTastesJizzEagerly".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.SpentAndParched".Localized());

            if (Player.Pregnancy.Event >= 2)
                OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.GlazedMidriff".Localized());

            OutputText("Scenes.Sophiebimbo.FuckBimboSophiesTits.SpermSealedEyes".Localized());

            SceneInstance<SophieScene>.Get().LuststickApplication(5);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void SophieEggApocalypse()
        {
            ClearOutput();

            SophieSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSophieHasDrunkOviElixir) == 0)
                OutputText("Scenes.Sophiebimbo.SophieEggApocalypse.SophieSearchesSupplies".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieEggApocalypse.LetSophieFindElixir".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesSophieHasDrunkOviElixir, CoC.Instance.Flags.Get(GameFlagIds.TimesSophieHasDrunkOviElixir) + 1);

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.Daddy".Localized(), BeBimboSophiesSugarDaddy);
            else
                AddButton(0, "Common.Button.Daddy".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.Mommy".Localized(), BeBimboSophiesSugarMommy);
            else
                AddButton(1, "Common.Button.Mommy".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<CampScene>());
        }

        public void BeBimboSophiesSugarDaddy()
        {
            ClearOutput();

            SophieSprite();

            int x = Player.CockThatFits(SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir));
            else
                if (Player.HasItemInStorage(ConsumableIds.OviElixir))
                    Player.ConsumeItemInStorage(ConsumableIds.OviElixir);

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.SlySmileElixirGift".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.SitBesideThickerSophie".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.UnfertilizedEggRuse".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.BallsPrefix".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.WingsAndArmsFlap".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.BoredOfFutileFlight".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.StickyStrandsPullOut".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarDaddy.AfterglowPreening".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            SceneInstance<SophieScene>.Get().LuststickApplication(2);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.LargeEggs[Random.Shared.Next(ConsumableIds.LargeEggs.Length)]), 1);

            ReturnToCamp(1);
        }

        public void BeBimboSophiesSugarMommy()
        {
            ClearOutput();

            SophieSprite();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir));
            else
                if (Player.HasItemInStorage(ConsumableIds.OviElixir))
                    Player.ConsumeItemInStorage(ConsumableIds.OviElixir);

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarMommy.PlayfulShrugGiftOffer".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarMommy.AttemptToThrowHer".Localized(
                Player.VaginaDescript(0),

                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarMommy.LaborStartsOnFall".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarMommy.EggsTumbleFromBody".Localized());

            OutputText("Scenes.Sophiebimbo.BeBimboSophiesSugarMommy.SophieLapsAfterBirth".Localized(
                Player.VaginaDescript(0)
            ));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            SceneInstance<SophieScene>.Get().LuststickApplication(2);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.LargeEggs[Random.Shared.Next(ConsumableIds.LargeEggs.Length)]), 1);

            ReturnToCamp(1);
        }

        public void RideDatSophieVag(bool nice = false)
        {
            ClearOutput();

            SophieSprite();

            var x = Player.CockThatFits(SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = -1;

            if (Player.CockTotal() > 1)
            {
                y = Player.CockThatFits2(SophieCapacity());

                if (y < 0)
                {
                    y = Player.BiggestCockIndex();

                    if (y == x)
                        y = Player.SmallestCockIndex();
                }
            }

            if (Player.Corruption <= 50)
                nice = true;

            if (nice)
            {
                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieInterruptsDecision".Localized());

                if (Player.Pregnancy.Event >= 2)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.PregnantBellyFails".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophiePressingBreasts".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.DrugCockArousal".Localized());

                if (Player.CockTotal() >= 2)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.PluralCocksGrow".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SingleCockGrows".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockHardInArmor".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieBeggingToFuck".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieVoiceHitching".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieSpotsBulge".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SLetter".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieFingeringSelf".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophiePleadsBitingLip".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.StripArmorApproach".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.RefuseFuckGrabbingHand".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SmirkingPunishThreat".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.AskThenPinchTits".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.AskWhatSheNeeds".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ReaffirmGripTit".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SucklingFloppyTit".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieGrindingHips".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.MouthGapesMoan".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CooingCockStrokes".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ReadyProbeEntrance".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SpearHarpyOpen".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieEagerlyComplies".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieArmEmbrace".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.GiggleMoanTreatment".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.RuttingInEmbrace".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.HipsStaccatoSmacks".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieClimaxGushing".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.TightGripApology".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SqueezeButtLock".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ProstatePleasureBuilds".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.BallsSlappingButt".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.GruntMoanNearing".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieBegsFilling".Localized());
            }
            else
            {
                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieInterruptsDecisionAgain".Localized());

                if (Player.Pregnancy.Event >= 2)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.PregnantBellyFailsAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophiePressingBreastsAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.DrugCockArousalAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CurseHerFallenRump".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieVoiceHitchingAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.KickOffArmorApproach".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.RefuseSlutGrabbingHand".Localized(
                    x + 1
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.EvilSmirkPunishThreat".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.TurkeyInsultSlapTits".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.AskNeedsHandBack".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ReaffirmGripTitAgain".Localized(
                    x + 1
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SucklingFloppyTitAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieGrindingHipsAgain".Localized(
                    x + 1
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.MouthGapesMoanAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CooingSlutCockStrokes".Localized(
                    x + 1
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ReadyProbeEntranceAgain".Localized(
                    x + 1
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SpearHarpyOpenAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieEagerlyCompliesAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieArmEmbraceAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.GiggleMoanTreatmentAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.RuttingInEmbraceAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.HipsStaccatoSmacksAgain".Localized(
                    x + 1
                ));

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieClimaxGushingAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.TightGripApologyAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SqueezeButtLockAgain".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ProstatePleasureHasBalls".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieBegsFillingAgain".Localized());
            }

            if (Player.CockTotal() == 1)
            {
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockThrobsSeedSurging".Localized(
                        x + 1
                    ));
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockSurgesThickSpunk".Localized());
                    else
                        if (Player.CumQ() < 1500)
                            OutputText("Scenes.Sophiebimbo.RideDatSophieVag.BodySeizesOrgasm".Localized(
                                x + 1
                            ));
                        else
                            if (Player.CumQ() < 3000)
                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.HipsJumpLoadWells".Localized());
                            else
                            {
                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.FlopOverSensation".Localized(
                                    x + 1
                                ));

                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.BellyBloatsSeed".Localized());

                                if (Player.Pregnancy.Event >= 2)
                                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.AllOverHerChannel".Localized());

                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockPlugsSeedIn".Localized(
                                    x + 1
                                ));
                            }
            }
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockTwitchesOrgasmStart".Localized(
                        x + 1
                    ));
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockThrobsFirstShots".Localized(
                            y + 1
                        ));
                    else
                        if (Player.CumQ() < 1500)
                        {
                            OutputText("Scenes.Sophiebimbo.RideDatSophieVag.InsidesMilkCockEdge".Localized(
                                x + 1,

                                x + 1,

                                y + 1
                            ));

                            if (Player.CockTotal() == 3)
                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.ThirdLengthSprays".Localized());
                            else
                                if (Player.CockTotal() > 3)
                                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.RestOfCocksSeed".Localized());

                            if (Player.CockTotal() == 2)
                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SentencePeriod".Localized());

                            OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieSighsThankYou".Localized());
                        }
                        else
                            if (Player.CumQ() < 3000)
                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CockSpasmsFirstBurst".Localized(
                                    x + 1
                                ));
                            else
                                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.CocksBurstTogether".Localized(
                                    x + 1
                                ));

            if (Player.HasVagina())
            {
                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.NeglectedVaginaTingles".Localized());

                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SquirtingWord".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.RideDatSophieVag.DrippingDrooling".Localized());

                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.OwnJuicesBarelyNoticed".Localized());
            }

            OutputText("Scenes.Sophiebimbo.RideDatSophieVag.SophieGratefulHeadRub".Localized());

            if (Player.Pregnancy.Event >= 2)
                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.HeadOnGravidTummy".Localized());
            else
                OutputText("Scenes.Sophiebimbo.RideDatSophieVag.HuggingTightGratitude".Localized());

            if (SophieIsInSeason())
                SophiePregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -4f));

            if (!nice && Player.Corruption < 50)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            SceneInstance<SophieScene>.Get().LuststickApplication(5);

            ReturnToCamp(1);
        }

        public void SophieGoesIntoSeason()
        {

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophieGoesIntoSeason.SophiePreensMoreOften".Localized());
        }

        public void SophieSeasonExpiration()
        {

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophieSeasonExpiration.OddBehaviorStopped".Localized());
        }

        public void SophieGotKnockedUp()
        {

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophieGotKnockedUp.UnusuallyDopeySmile".Localized());
        }

        public void SophieImpregnationSex()
        {
            ClearOutput();

            var x = Player.CockThatFits(SophieCapacity());

            var y = Player.CockThatFits2(SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.HurriedUndressingRealization".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SComeSuffix".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ComesWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.OffendingGarmentSnap".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SLetter".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ProudErectState".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CockWobbleSingular".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CockWobblePlural".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CommaSpace".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.EachWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieBobbingStare".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SLetterAgain".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CockTranceLipLick".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.MultiLengthChoice".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SingleLengthChoice".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.AndShakeWord".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ItWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ThemWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.StartleDazedBimbo".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieRecollectThoughts".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.BallsToken".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SheathToken".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SlowSensuousMassage".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophiePullsToGround".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.GoldLipsDescend".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.WordsTakeEffect".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.MouthFeelsGoodDripping".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophiePantsBegsHard".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PushBimboAllFours".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.LipstickSmearedShafts".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.TailFeathersFan".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.HipsSlapButtcheeks".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieMagicFalters".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.HandsGripWaist".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ExtraPrefix".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.BetweenThighsPlural".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.BetweenThighsSingular".Localized());

                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.GatherMoreDrug".Localized());
            }

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieCumsWigglingButt".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.StrengthIntoStuffing".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SSwellSuffix".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SwellsWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.OnsetOfRelease".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.BallsRhythmicPulse".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.InnerClenchRhythm".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PussyStretchesThickness".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ExtraThighSlappingCock".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SLetterThird".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.WarmEcstasySpurt".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CreamySpunkRopes".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.HungryTwatDrinks".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.DrownPussyJism".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.AlabasterSpoogeOoze".Localized());

            if (Player.CumQ() >= 2000)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SaltyGroundQuagmire".Localized());

            if (Player.CumQ() >= 5000)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.DropletsBecomeGouts".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.OrgasmTapersSophieSlips".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PussyLipsReleasePrick".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ThighsSadReleaseManhood".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PluralSLetter".Localized());

                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ManhoodEscapeSuffix".Localized());
            }

            if (Player.HasKnot(x))
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SwollenKnotSlowsPullout".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieLeftMessyWreck".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieWhimpersForMore".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.GrabAssForLeverage".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.AccidentalDoublePenetration".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SavoringAnalRingClench".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SpermOozingPussyTail".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SentencePeriod".Localized());

            if (y < 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieMoansPreggersProtest".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieMoansButtProtest".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SpreadReddenedCheeks".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.AndPussySuffix".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ConfidentStrokesSilentAnswer".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.TiredOfAllWork".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PluralSAreFragment".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.IsWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ColdAirExposureSuffix".Localized());

            if (y < 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ItsContractionWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.TheyreContractionWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.EngulfingAnalEntrance".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SqueezingPussyFragment".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.AllOverAgainSuffix".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieAsksIfGood".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SittingUpGropingTits".Localized());

            if (Player.Tallness >= 74)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.NippingEarVariant".Localized());
            else
                if (Player.Tallness >= 60)
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.NippingShoulderVariant".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.NippingSideVariant".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.BreastsSofterThanButtcheeks".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PluralSLetterAgain".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.FocusedOnBuildingClimax".Localized());

            if (y < 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SquirtingAcrossLegs".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CumTricklesInRivers".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SpunkExplosionAnalClench".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.AlabasterFlowerInRectum".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PluralSLetterThird".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ArmsWrapBehindAffection".Localized());

            if (y >= 0)
            {
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CuntCreaminessBoost".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.BellyMoreDistended".Localized());

                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.PeriodSpace".Localized());
            }

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.UnstoppableSecondOrgasm".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.GutRumblesFillingIntestines".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CumPaunchLeadIn".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.VisibleBellyIncrease".Localized());

                OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CumPaunchConclusion".Localized());

                if (Player.CumQ() >= 1500)
                {
                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.CumSquirtsAroundCock".Localized());

                    if (Player.HasKnot(x))
                        OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.YourKnotSuffix".Localized());

                    OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.IntestinesFilledNoRoom".Localized());
                }
            }

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.LimpHarpyStillDribbling".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SighFarFromDone".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.ThroatRamWithoutWarning".Localized());

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.SophieBegsPussyCum".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.SophieImpregnationSex.FallBackEyesRolling".Localized(
                Player.CockDescript(x)
            ));

            SophiePregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            if (Player.CumQ() < 5000)
                Player.CumMultiplier += 3;

            SceneInstance<SophieScene>.Get().LuststickApplication(8);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PostSophieImpreg);
        }

        public void PostSophieImpreg()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.PostSophieImpreg.MorningSnuggleEmbrace".Localized());

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void GiantCocksImpregnateSophie()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.CockAgreesWithHarpy".Localized());

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.SophieGigglesAtSize".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.PeekingUnderCockMass".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.SeedWaitingInBalls".Localized());
            else
                OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.SeedDeepInsideYou".Localized());

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.StatedAsSimpleFact".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.TracingRigidVein".Localized());

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.CockBetweenHerTits".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.FootjobWhileMusing".Localized());

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.GrindingStopsHerChat".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.BellySwellsIntoBump".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.BumpBalloonsToDome".Localized());

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.CooingAboutFeelingGood".Localized());

            OutputText("Scenes.Sophiebimbo.GiantCocksImpregnateSophie.AskingForRoundTwo".Localized());

            SophiePregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            if (Player.CumQ() < 5000)
                Player.CumMultiplier += 3;

            SceneInstance<SophieScene>.Get().LuststickApplication(8);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PostSophieTooBigImpreg);
        }

        public void PostSophieTooBigImpreg()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.PostSophieTooBigImpreg.MilkedThroughMoreOrgasms".Localized());

            Player.Sleeping = true;

            ReturnToCamp(1);
        }

        public void SophieMoveInAttempt()
        {
            ClearOutput();

            SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SophieBroachedSleepWith, 1);

            OutputText("Scenes.Sophiebimbo.SophieMoveInAttempt.SomberApproachRequest".Localized());

            OutputText("Scenes.Sophiebimbo.SophieMoveInAttempt.AskingToSleepOver".Localized());

            OutputText("Scenes.Sophiebimbo.SophieMoveInAttempt.GenuineCompanionshipDesire".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesSophieSleepWith);

            AddButton(1, "Common.Button.Nah".Localized(), NoSophieSleepWith);
        }

        public void YesSophieSleepWith()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.YesSophieSleepWith.ClappingExcitedHug".Localized());

            var __sw0_0 = Player.Pregnancy.Event;

            if (__sw0_0 == 1)
                OutputText("Scenes.Sophiebimbo.YesSophieSleepWith.BellyGrowingWord".Localized());

            if (__sw0_0 == 2)
                OutputText("Scenes.Sophiebimbo.YesSophieSleepWith.BellySwollenWord".Localized());

            if (__sw0_0 == 3 || __sw0_0 == 4)
                OutputText("Scenes.Sophiebimbo.YesSophieSleepWith.BellyHugeGravidWord".Localized());

            if (true )
                OutputText("Scenes.Sophiebimbo.YesSophieSleepWith.BellyTautWord".Localized());

            OutputText("Scenes.Sophiebimbo.YesSophieSleepWith.CushionyTitsPressedClose".Localized(
                Player.Mf("hunky", "cute")
            ));

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Sophie");

            ReturnToCamp(1);
        }

        public void NoSophieSleepWith()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.NoSophieSleepWith.SleepingAloneInNest".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Sophie")
                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");

            ReturnToCamp(1);
        }

        public void MoveSophieInRepeat()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.MoveSophieInRepeat.AskingToShareBed".Localized());

            if (!Player.IsPregnant())
                OutputText("Scenes.Sophiebimbo.MoveSophieInRepeat.NoLongerPregnant".Localized());

            OutputText("Scenes.Sophiebimbo.MoveSophieInRepeat.RadiantGlossedSmile".Localized());

            OutputText("Scenes.Sophiebimbo.MoveSophieInRepeat.BoobsAsPillowQuestion".Localized());

            OutputText("Scenes.Sophiebimbo.MoveSophieInRepeat.ThumbsUpResignation".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Sophie");

            ReturnToCamp(1);
        }

        public void SophieFenCraftedSex(bool morning = false)
        {
            ClearOutput();

            var x = Player.CockThatFits(SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            SophieSprite();

            if (morning)
            {
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.WarmSnugnessOnWaking".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.PossessiveSLetter".Localized());

                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.WakingFromSleepHaze".Localized());

                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.MorningSexGiftGreeting".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                SophieSprite();

                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.RemovingArmorForSex".Localized());

                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                    OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.PregnantBellyWord".Localized());

                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.HotDropletsOnMidsection".Localized());

                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.BeckoningHerOnward".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.FrenziedHipsBouncing".Localized(
                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.GroaningImpaledGrope".Localized());

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.SquealingNippleRequest".Localized(
                Player.CockHead(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.BacksideSlidesOverBalls".Localized());

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.NipplePullSquawk".Localized());

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.JuicesDripAtClimax".Localized(
                Player.CockDescript(x)
            ));

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.PossessiveOwnWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.ChestBodyPartWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.LipGlossKissEffect".Localized(
                Player.CockDescript(x)
            ));

            if (Player.IsPregnant())
            {
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.WombFullOfEgg".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.SpunkWashesOut".Localized());
            }

            if (Player.CumQ() >= 2000)
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.SpunkSoaksHerThighs".Localized());

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.MuffledClimaxScream".Localized());

            OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.DismountWithStifledMoan".Localized());

            if (morning)
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.WashingOffAtStream".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieFenCraftedSex.IgnoringStickyArmor".Localized());

            if (SophieIsInSeason())
                SophiePregChance();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            if (morning)
                Go<CampScene>();
            else
                ReturnToCamp(1);
        }

        public void FuckYoPregnantHarpyWaifu(bool morning = false)
        {
            ClearOutput();

            var x = Player.CockThatFits(SophieCapacity());

            if (morning)
            {
                SophieSprite();

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.ConfusedPleasureWaking".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SophieGrindsOnCock".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.BabyCutenessChat".Localized());

                if (morning)
                    OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.MorningWord".Localized());

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SilkySmoothSkin".Localized());
            }
            else
            {
                SophieSprite();

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.UndressingForSophie".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Lust > 50)
                    OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.CommentsOnHardness".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.StrokingUntilItPulses".Localized());

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.HandjobTinglesPleasantly".Localized());

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.LeaningBackAsAsked".Localized(
                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SophieRidesConfidently".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.ConfessesHerNeeds".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.ThickDropsHanging".Localized());
            else
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.TaintAndLegs".Localized());

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.ProvingWetnessClaims".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.HuggingPregnantBelly".Localized());

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.ReleasePressureSuck".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.NippleSwellsInMouth".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.PleadingToCumInside".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.PregnantBellySlide".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SLetter".Localized());

                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.TummyfuckDoublesPleasure".Localized());
            }

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.CumSqueezedBackOut".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.EnoughThereTo".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SoakBlanketsOnly".Localized());
                else
                    if (Player.CumQ() < 2000)
                        OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SoakBlanketsAndGround".Localized());
                    else
                        if (Player.CumQ() < 5000)
                            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.FormGiantSpunkPuddle".Localized());
                        else
                            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.CreateCumLake".Localized());
            }

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SaggingAfterClimax".Localized());

            if (morning)
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.PantingAboutMorning".Localized());
            else
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.PantingAboutDaddy".Localized());

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.BetweenPantingGasps".Localized());

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SophieLeavesYou".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.InHardeningMud".Localized());

            OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.SwayingAsSeedRuns".Localized());

            if (morning)
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.WashUpGoodDay".Localized());
            else
                OutputText("Scenes.Sophiebimbo.FuckYoPregnantHarpyWaifu.DressingEnjoyingAfterglow".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            if (morning)
                Go<CampScene>();
            else
                ReturnToCamp(1);
        }

        public void SophiePreggoTitJobs()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.PointingCockAtTits".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.GlitteringCleavage".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.GoodLuckKissRequest".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.AmberNubsWithdrawn".Localized(
                Player.CockHead(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.FishesThePrefix".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.OtherOne".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.RestWord".Localized());

                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.GivingFromArmor".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.ItWord".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.EachWord".Localized());

                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.SlowAffectionateStroke".Localized());
            }

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.TipSwellsInMouth".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.CockCravingQueen".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.BimboTonguesLips".Localized());

            if (Player.Cocks[x].Length > 30)
                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.UndersideOfErection".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.YourWord".Localized(
                    Player.CockHead(x)
                ));

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.LipsPursedPleasure".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length > 30)
                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.PressesAgainst".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.SlipsInto".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.PuffyPuckeredOrifice".Localized());

            if (Player.Cocks[x].Length > 30)
                OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.GaspingAboutLength".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.SophiePullsTitsAway".Localized());

            OutputText("Scenes.Sophiebimbo.SophiePreggoTitJobs.TitjobChoicePrompt".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.LetHer".Localized(), LetSophieMilkYoDick);

            AddButton(1, "Common.Button.HogWild".Localized(), GoHogWildOnSophieBewbs);
        }

        public void GoHogWildOnSophieBewbs()
        {
            ClearOutput();

            SophieSprite();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.IgnoringHerPromises".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.YourOtherCock".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.SplayFragment".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.SplaysWord".Localized());

                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.BrushingHerTitSide".Localized());
            }

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.BlubbersInSurprise".Localized());

            if (Player.Cocks[x].Length <= 30)
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.MuffledByCockPillows".Localized(
                    Player.CockHead(x)
                ));
            else
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.RodSmacksHerFace".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.TuggingNipplesTogether".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.TitsBounceOffCrotch".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.HoneyedLipglossEffects".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length <= 30)
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.IntoSophiesMouth".Localized());
            else
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.AboveHerHead".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.BeggingForFacial".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.ReprimandInterrupted".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.PushedBackForFacial".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.CuppingBallsToAid".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.SquishedAgainstBelly".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            if (Player.HasKnot(x))
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.KnotInflatesLudicrous".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.CumSplattersHerFace".Localized());

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.PaintingHerWhite".Localized());

            if (Player.CumQ() > 2500)
                OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.SpermySmileAftermath".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.KissWhileFinishing".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.SteppingAwaySmiling".Localized());

            OutputText("Scenes.Sophiebimbo.GoHogWildOnSophieBewbs.ThankfulFarewellCall".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -2f));

            SceneInstance<SophieScene>.Get().LuststickApplication(8);

            ReturnToCamp(1);
        }

        public void LetSophieMilkYoDick()
        {
            ClearOutput();

            SophieSprite();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.NodAndHoldStill".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.BallsOnPregnantBelly".Localized());

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.BreastsBounceInSync".Localized());

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.GroanRestrainInstinct".Localized());

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.LipBiteConfusion".Localized());

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.EyeRollMouthPop".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.BreakForCumDemand".Localized());

            if (Player.Cocks[x].Length <= 30)
                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.ReadyToCatchCum".Localized());
            else
                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.HelpingCatchCum".Localized());

            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.CumBubblingInBalls".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length <= 30)
            {
                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SophieWord".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SwallowsWithoutComplaint".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.CheeksBulgeSmallLoad".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.CheeksBulgeHugeLoad".Localized());
                        else
                        {
                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.CheeksBulgeNoseSpray".Localized());

                            if (silly)
                                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.RepelCumMagnitude".Localized());
                            else
                                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.HandleThatMuchCum".Localized());

                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SpunkUntilGagging".Localized());
                        }

                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.BirdGirlWithdraws".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.WithHappySmile".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.CoughingSputtering".Localized());

                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.DrainedCockCaress".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SophieWordAgain".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.GigglesAtCumJets".Localized());
                else
                    if (Player.CumQ() < 800)
                        OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.MoansHeavyDeposits".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                    {
                        OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.MoansWhorishlyGiantCum".Localized());

                        if (Player.CumQ() >= 2000)
                        {
                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SoakedHeadToToe".Localized());

                            if (Player.CumQ() >= 3000)
                            {
                                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.AndAConnector".Localized());

                                if (Player.CumQ() < 4000)
                                    OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.PuddleWord".Localized());
                                else
                                    if (Player.CumQ() < 6000)
                                        OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.BigPuddle".Localized());
                                    else
                                        if (Player.CumQ() < 10000)
                                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SmallLake".Localized());
                                        else
                                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.ObsceneLake".Localized());

                                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.FormedAroundPair".Localized());
                            }

                            OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.SentencePeriod".Localized());
                        }
                    }

                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.RubsSpunkOnBreasts".Localized());

                OutputText("Scenes.Sophiebimbo.LetSophieMilkYoDick.PulledAgainstBelly".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -2f));

            SceneInstance<SophieScene>.Get().LuststickApplication(8);

            ReturnToCamp(1);
        }

        public void SophieBirthsEgg()
        {

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.HighPitchedCry".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.EngorgedGapedPussy".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.WantsHandToHold".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.NeedsSomethingToHold".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.SitDownHoldHand".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.SitSighGropeTit".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.PlopDownIgnoreHand".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.SinkIntoSoftChest".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.RsLetter".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.WhiteRimPeeksOut".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.FingertipsRubClit".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.GushOfFluidsEmbryo".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.EggSeemsFineDrying".Localized());

            OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.StickAroundToCheck".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 0)
                OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.AnotherHarpyInCamp".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieBirthsEgg.HowManyHarpiesFit".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));
        }

        public void SophiesEggHatches()
        {

            CoC.Instance.Flags.Set(GameFlagIds.SophieCampEggCountdown, 0);

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.QuietBrokenByCries".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.BimboWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.WomanWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.HarpyRaisingFuss".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.SillyWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.HarpyFussesNewEgg".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.HappyWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.SmileGrabsYourHand".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.EggHatchTimeFirst".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.EggHatchTimeSecond".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.CoosEggAlmostThere".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) < 4)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 0)
                {
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.NewbornDaughterLipsShock".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.LipsLargerThanNormal".Localized());
                    else
                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.SentencePeriod".Localized());

                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.ChickLooksFourYears".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                        CoC.Instance.Flags.Set(GameFlagIds.DaughterOneBimbo, 1);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 1)
                    {
                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.SecondHatchStillAmazed".Localized());

                        if (Player.Corruption > 66)
                            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.LittleBitPhrase".Localized());

                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.ChickLooksSameAge".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                        {
                            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.PlatinumBlondeHair".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.DaughterTwoBimbo, 1);
                        }
                        else
                            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.PurePinkHair".Localized());

                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.FeathersLongerFuzzyDown".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 2)
                        {
                            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.AnotherHarpyFirstMoments".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                                CoC.Instance.Flags.Set(GameFlagIds.DaughterThreeBimbo, 1);
                        }
                        else
                        {
                            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.FourthChildLikeMom".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.HairBeforeBimbofication".Localized());

                            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.ShapePromisesMaturity".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                                CoC.Instance.Flags.Set(GameFlagIds.DaughterFourBimbo, 1);
                        }

                if (BimboSophie())
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.SweptUpBeforeStanding".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.SweptUpEcstaticMother".Localized());
            }
            else
            {
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.NeverTiredOfHatching".Localized());

                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.PinkWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.BlondeWord".Localized());
                    else
                        OutputText("Scenes.Sophiebimbo.SophiesEggHatches.WhiteWord".Localized());

                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.GirlCoosShieldingEyes".Localized());

                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.VioletWord".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.BlueWord".Localized());

                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.ShieldingEyesFromSun".Localized());

                if (BimboSophie())
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.CuteFluffyBabyGirl".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophiesEggHatches.BoysAllOverLater".Localized());
            }

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.TinyHarpyStartledSquawk".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.MommaSophieCalmsFirst".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.MommaSophieCalmsSecond".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.CalmsUnderAffection".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.EmbracingFeatheryFamily".Localized());

            OutputText("Scenes.Sophiebimbo.SophiesEggHatches.GrinningAtYourPrefix".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.GrabBimboHugeAss".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophiesEggHatches.GrabHarpyRoundAss".Localized(
                    Player.Mf("stud", "breeder")
                ));

            CoC.Instance.Flags.Set(GameFlagIds.SophieDaughterMaturityCounter, 336);
        }

        public void SophieKidMaturation()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.CheckingOnGrowingDaughter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.DaughterOneBimbo) == 1)
                    OutputText("Scenes.Sophiebimbo.SophieKidMaturation.ChestLipsGrowImmense".Localized());
                else
                    OutputText("Scenes.Sophiebimbo.SophieKidMaturation.ChestLipsGrowLarge".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.DaughterTwoBimbo) == 1)
                        OutputText("Scenes.Sophiebimbo.SophieKidMaturation.AssExpandsImmensely".Localized());
                    else
                        OutputText("Scenes.Sophiebimbo.SophieKidMaturation.AssExpandsWithHips".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 2)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.DaughterThreeBimbo) == 1)
                            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.PerfectHarpyHumanFeet".Localized());
                        else
                            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.PerfectHarpyFeetAnomaly".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 3)
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.DaughterFourBimbo) == 1)
                                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.EqualMeasureExpansion".Localized());
                            else
                                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.EqualMeasureCriticalMass".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.FleshRipplesBroodmotherHips".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.BimboWord".Localized());

                            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.BreastsHighRoundPerky".Localized());
                        }

            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.NewlyEmbodiedSighRelief".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.BimboWordTight".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.BombshellWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieKidMaturation.DaughterResemblesMother".Localized());

            if (BimboSophie())
            {
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.InquisitiveSparkIntelligence".Localized());

                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.AdmiresNewHotBody".Localized());
            }
            else
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.NewBodyStrangeSexy".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.MommaProudHotPiece".Localized());
            else
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.FineLookingHarpyProud".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.SophieFamilyIncest) == 1)
                {
                    OutputText("Scenes.Sophiebimbo.SophieKidMaturation.NodAndPraiseBeauty".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 1)
                        OutputText("Scenes.Sophiebimbo.SophieKidMaturation.SLetter".Localized());

                    OutputText("Scenes.Sophiebimbo.SophieKidMaturation.RadiantSmileShakesRump".Localized(
                        Player.Mf("Daddy", "'Mom'")
                    ));

                    OutputText("Scenes.Sophiebimbo.SophieKidMaturation.PromiseInDueTime".Localized());
                }
                else
                    OutputText("Scenes.Sophiebimbo.SophieKidMaturation.RuefulSmileToMother".Localized());

                ReturnToCamp(1);
            }
            else
            {
                OutputText("Scenes.Sophiebimbo.SophieKidMaturation.RealQuestionIncestChoice".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.YesIncest".Localized(), YesToSophieIncest);

                AddButton(1, "Common.Button.No".Localized(), NoToSophieIncest);
            }

            CoC.Instance.Flags.Set(GameFlagIds.SophieAdultKidCount, CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) + 1);
        }

        public void YesToSophieIncest()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.YesToSophieIncest.KissEagerGirlAss".Localized());

            OutputText("Scenes.Sophiebimbo.YesToSophieIncest.SophiePoutNoFair".Localized());

            if (BimboSophie())
                OutputText("Scenes.Sophiebimbo.YesToSophieIncest.PersonalBimboWord".Localized());
            else
                OutputText("Scenes.Sophiebimbo.YesToSophieIncest.LustyMatronWord".Localized());

            OutputText("Scenes.Sophiebimbo.YesToSophieIncest.ReturnsSuddenAffection".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            CoC.Instance.Flags.Set(GameFlagIds.SophieFamilyIncest, 1);

            ReturnToCamp(1);
        }

        public void NoToSophieIncest()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiebimbo.NoToSophieIncest.StrokeDaughtersCheek".Localized());

            ReturnToCamp(1);
        }

        public void SophieAndIzmaPlay()
        {

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.ParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesSophieAndIzmaFucked, CoC.Instance.Flags.Get(GameFlagIds.TimesSophieAndIzmaFucked) + 1);

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SomethingOddPrefix".Localized());

            if (View.Time.Hours < 6 || View.Time.Hours > 20)
                OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.NightTimeWord".Localized());
            else
                if (View.Time.Hours < 12)
                    OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.MorningTimeWord".Localized());
                else
                    if (View.Time.Hours == 12)
                        OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.NoonTimeWord".Localized());
                    else
                        if (View.Time.Hours < 5)
                            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.AfternoonTimeWord".Localized());
                        else
                            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.EveningTimeWord".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.BoldCloseSuffix".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaDemandsRest".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaStopsMoaning".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SophieLandsTittering".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaStrutsForward".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaUntiesSkirt".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SophieAsksConfused".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaBringsRope".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SophieQuitsStruggle".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaSaysHavingFun".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SophieSaysNo".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.IzmaLiftsSophieUp".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SophieStrungUp".Localized());

            OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.PlayerWeighsChoices".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSophieAndIzmaFucked) == 1)
                OutputText("Scenes.Sophiebimbo.SophieAndIzmaPlay.SophieRetryHint".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            ClearButtons();

            AddButton(0, "Common.Button.Interrupt".Localized(), InterruptSophieIzma);

            AddButton(1, "Common.Button.Watch".Localized(), WatchIzmaSophie);
        }

        public void InterruptSophieIzma()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.InterruptSophieIzma.PlayerSteelsHimself".Localized());

            OutputText("Scenes.Sophiebimbo.InterruptSophieIzma.IzmaJumpsAway".Localized());

            OutputText("Scenes.Sophiebimbo.InterruptSophieIzma.PlayerDemandsTruth".Localized());

            OutputText("Scenes.Sophiebimbo.InterruptSophieIzma.PlayerOrdersRelease".Localized());

            Go<CampScene>();
        }

        public void WatchIzmaSophie()
        {
            ClearOutput();

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaSetsTheRules".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.SophieAsksPromise".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaTeasesWithCock".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.WetImpactSound".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.PlayerArousedWatching".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaPoundsHard".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.AlphaClaimSuffix".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaFantasyThreat".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.SophieGigglesNaughty".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaGropesBreasts".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaBouncesSophie".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaBreaksKiss".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.SophieBegsForCum".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.ChoiceStrokeBliss".Localized());
            else
                OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.ChoiceFingerBliss".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaClimaxBuilding".Localized());

            if ((Player.HasCock()) && (Player.CumQ() >= 1000))
                    OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.OwnMessSuffix".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.SophieBellyFilled".Localized());

            OutputText("Scenes.Sophiebimbo.WatchIzmaSophie.IzmaWithdrawsPanting".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Go<CampScene>();
        }

    }
}