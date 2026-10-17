using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using MonoCoC.Contents.BodyParts.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<NephilaCovenFollowerScene>("coc:nephila_coven_follower")]
    public class NephilaCovenFollowerScene : Scene
    {

        public override void Entry()
        {
            NephilaCovenFollowerEncounter();
        }

        public bool NephilaCovenIsFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.NephilaCovenQueenCrowned) > 0;
        }

        public void NephilaCovenFollowerEncounter(bool forceNephilaCoven = false)
        {

            if (forceNephilaCoven)
            {
                NephilaCovenFollowerAppearance();

                return ;
            }

            NephilaCovenFollowerAppearance();
        }

        public void NephilaCovenSprite()
        {

        }

        public void NephilaCovenFollowerAppearance(bool output = true)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.MoveToSecludedPortion".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.WhiteSlimeGirlWearing".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.OpaqueSlimeOfPortal".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.HallIsTallBaroque".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) == 1)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.MatronNodsAtYou".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) > 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 1)
                        OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.YoungMouseChildDarts".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) > 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 2)
                            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.DaughtersAreGathered".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) > 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 3)
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.TorturedOceanicSloshing".Localized());
                            else
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.ParagraphBreak".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.JumpAsGooeyHand".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.NearestDaughterAsks".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenFollowerAppearance.WonderWhatIWill".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntryNephila);
            }

            ClearButtons();

            if (Player.Lust >= 33)
                AddButton(1, (silly ? "Common.Text.OrgyPorgy".Localized() : "Common.Button.Orgy".Localized()), FuckFollowerNephilaCoven);
            else
                AddButton(1, (silly ? "Common.Text.OrgyPorgy".Localized() : "Common.Button.Orgy".Localized()), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaQueenArmor) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.NephilaSecondAscension) != 0))
                AddButton(3, "Common.Button.RequestProtection".Localized(), NephilaQueenArmorGive);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaQueenArmor) == 0 && CoC.Instance.Flags.Get(GameFlagIds.NephilaSecondAscension) == 0)
                    AddButton(3, "Common.Button.RequestProtection".Localized(), null);
                else
                    AddButton(3, "Common.Button.RequestProtection".Localized(), null);

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 15 && CoC.Instance.Flags.Get(GameFlagIds.NephilaSecondAscension) == 0)
                AddButton(7, "Common.Button.AscendThrone".Localized(), AscendThroneSecondAscension);
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 20 && CoC.Instance.Flags.Get(GameFlagIds.NephilaSecondAscension) > 0)
                    AddButton(7, "Common.Button.AscendThrone".Localized(), AscendThroneFinalAscension);
                else
                    AddButton(7, "Common.Button.AscendThrone".Localized(), null);

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                switch (Player.StatusEffectValue(StatusEffectIds.ParasiteNephilaNeedCum, 1))
                {
                    case 1:
                    {
                    AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensImp);
                    }

                    break;

                    case 4:
                    {
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) == 1)
                        AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensMouseFirst);
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) > 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 1)
                            AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensMouseRepeat);
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) > 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 2)
                                AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensMouseRepeat);
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) > 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 3)
                                    AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensMouseBadEndRepeat);
                                else
                                    AddButton(11, "Common.Button.VisitThePens".Localized(), null);
                    }

                    break;

                    case 2:
                    {
                    AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensMinotaur);
                    }

                    break;

                    case 10:
                    {
                    AddButton(11, "Common.Button.VisitThePens".Localized(), NephilaVisitPensAnemone);
                    }

                    break;
                }
            else
                AddButton(11, "Common.Button.VisitThePens".Localized(), null);

            AddButton(13, "Common.Button.Spar".Localized(), DecideToSparNephila);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void NephilaCovenAcceptOffer()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.InstructSlimeGirl".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.ThenGroansRearing".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.SlimeLaborsForNearly".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.SheSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.PressurizedHose".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.InShadowedMain".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.ThroneIsMassive".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.VoiceCriesEchoing".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.SlimeSaysOtherGirls".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.SheAsks".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.DrapesHerselfAgainst".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOffer.PleaseMotherLeadChildren".Localized());

            AddButton(0, "Common.Button.Firstascension".Localized(), NephilaCovenAcceptOfferPt2);

            AddButton(1, "Scenes.Lumberjack.Button.Decline".Localized(), NephilaCovenRefuseOffer);

            RemoveButton(2);

            RemoveButton(14);
        }

        public void NephilaCovenAcceptOfferPt2()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.NephilaCovenQueenCrowned, 1);

            Player.CreateKeyItem("Queen's Crown - Nephila", 0, 0, 0, 0);

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.AgreeToAscendThrone".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.SheSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.HyperpregnantDaughtersSurge".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.MatronTutsProbes".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.IdeaOfBeingPlied".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.AsksYouNodLean".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.AllowedOnlySingle".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.OozingAroundBelly".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenAcceptOfferPt2.RespondToTheSlime".Localized());

            Player.SetClitLength(23);

            AwardAchievement(AchievementIds.NephilaArchQueen);

            Player.CreatePerk(PerkIds.NephilaArchQueen, 0, 0, 0, 0);

            RemoveButton(0);

            RemoveButton(1);

            RemoveButton(2);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void NephilaCovenRefuseOffer()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenRefuseOffer.SlimeSaysDrawsBack".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaCovenRefuseOffer.MatronEscortsYou".Localized());

            RemoveButton(0);

            RemoveButton(1);

            RemoveButton(2);

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void FuckFollowerNephilaCoven()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.InformDaughtersThat".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.IncapacityEnhancesPleasure".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.HandSnakesOut".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.WhispersIntoEar".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.ClambersUpThe".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.BeginsRockingUp".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.FuckFollowerNephilaCoven.AgainForTwoHours".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 10f));

            Player.Thickness += 3 + Random.Shared.Next(5);

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void NephilaQueenArmorGive()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.NephilaQueenArmor, 1);

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.AskMatronForAid".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.ClapsHandsSummoning".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.PreggoServantRunsGooey".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.LeveragesFurtherBackward".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.SpeakingSlowlyBetween".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.LoungeForSeveralHours".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.LaboringKittiesBulge".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.EachFeatherSettles".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.InHerPlaceRegal".Localized(
                Player.AssDescript()
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.HeavingHerselfUp".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaQueenArmorGive.ThankDaughtersFor".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, -10f));

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.NephilaGown), 1);

            ReturnToCamp(1);

        }

        public void AscendThroneSecondAscension()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.NephilaSecondAscension, 1);

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension.DecideToApproachThrone".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension.SmoothStoneWhereFirst".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension.WatchingYouTest".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension.CoosAsThree".Localized());

            AscendThroneSecondAscension2();
        }

        public void AscendThroneSecondAscension2()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.ThreeMonstersLedIn".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.DemonessAsksPivoting".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.ClapsHandsRising".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.UsherSuccubusForward".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.SightOfTheUnbirthing".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.UltimatelyLittleBeast".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.TheTentacleBeast".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.ThoughClearlySuffered".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.SayMotioningHandmaidens".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.GooeyAppendagesSurge".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.DrawPlantoidBehemoth".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.MomentOfTruthArrives".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.OnceDeedIsDone".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.ThenExclaimsIn".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.PoutAsYou".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.GooServantReadsMind".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.MatronSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.ReturnToCampAfter".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

            Player.Thickness += 4;

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephila, 0, 1);

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 3)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.VeritableBroodmother".Localized());

                Player.Vaginas[0].Wetness = 5;
            }

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 5)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.ReachOutToHug".Localized());

                Player.ApplyStats(new DynStat(GameStats.Speed, -5f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                Player.Vaginas[0].Wetness = 5;
            }

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 10)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.AttemptToHeaveReplete".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.PerkNephilaQueenAdded".Localized());

                Player.Vaginas[0].Looseness = 5;

                Player.ApplyStats(new DynStat(GameStats.Speed, 15f), new DynStat(GameStats.Toughness, 10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

            AwardAchievement(AchievementIds.NephilaQueen);

                Player.CreatePerk(PerkIds.NephilaQueen, 0, 0, 0, 0);

                Player.Vaginas[0].Wetness = 4;
            }

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 15)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.SmileToYourself".Localized());

                Player.Vaginas[0].Looseness = 5;

                Player.ApplyStats(new DynStat(GameStats.Speed, 15f), new DynStat(GameStats.Toughness, 10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

                Player.Vaginas[0].Wetness = 4;
            }

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 20 || CoC.Instance.Flags.Get(GameFlagIds.NephilaCovenQueenCrowned) == 0)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.AwakeInTheNight".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0)
                    OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.FirmlyWedgedIntoRoof".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) == 0)
                    OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.FillingTheCircusTent".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.WhenTheyArriveNephila".Localized());

                Player.Vaginas[0].Looseness = 5;

                Player.Body.Hips += 1 + Random.Shared.Next(3);

                Player.ApplyStats(new DynStat(GameStats.Speed, 1f), new DynStat(GameStats.Toughness, -10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 10f), new DynStat(GameStats.Corruption, 100f));

                Player.Vaginas[0].Wetness = 4;
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 11)
                {
                    OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneSecondAscension2.WhateverGainsYouveMade".Localized());

                    Player.Vaginas[0].Looseness = 5;

                    Player.ApplyStats(new DynStat(GameStats.Speed, -5f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Corruption, 100f));

                    Player.Vaginas[0].Wetness = 4;
                }

            Player.Orgasm("Vaginal");

            Player.Body.Hips += 10 + Random.Shared.Next(10);

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f));

            Player.Inventory.Add(ItemRegistry.Create(WeaponIds.NephilaScepter), 1);

            ReturnToCamp(1);

        }

        public void AscendThroneFinalAscension()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.ApproachTheThroneStone".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.OrderTentaclesToRotate".Localized(
                Player.AssDescript()
            ));

            if (Player.Body.Butt < 4)
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.LeanAssCheeksClench".Localized());

            if (Player.Body.Butt >= 4 && Player.Body.Butt < 6)
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.AssCheeksClenchAnticipation".Localized());

            if (Player.Body.Butt >= 6 && Player.Body.Butt < 10)
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.PlumpButtocksRipple".Localized());

            if (Player.Body.Butt >= 10 && Player.Body.Butt < 15)
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.SizableAssWobble".Localized());

            if (Player.Body.Butt >= 15 && Player.Body.Butt < 20)
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.MassiveAssJiggleWobble".Localized());

            if (Player.Body.Butt >= 20)
                OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.FreakishAssJiggle".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.ParagraphBreak".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.SettleInReclining".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension.PreggersDaughtersApproach".Localized());

            AscendThroneFinalAscension2();
        }

        public void AscendThroneFinalAscension2()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension2.MatronArrivesAsOften".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension2.SheSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension2.HadntBotheredToNotice".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension2.EatAndFuckAnd".Localized());

            AscendThroneFinalAscension3();
        }

        public void AscendThroneFinalAscension3()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension3.WhispersGooShudders".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension3.SheSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension3.SoonYouReallyWill".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension3.MatronVoiceWorshipful".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension3.HissesDaughtersClamber".Localized());

            AscendThroneFinalAscension4();
        }

        public void AscendThroneFinalAscension4()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension4.FiftyYearsPassBy".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension4.FiftyYearsInWhich".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension4.EvenAsYouLived".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension4.AtThisMomentEating".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension4.NotThatYouNotice".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension4.BehindYouCrushedUp".Localized());

            AscendThroneFinalAscension5();
        }

        public void AscendThroneFinalAscension5()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension5.SurroundedByVastHordes".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.AscendThroneFinalAscension5.DesireToGrow".Localized());

            GameOver();
        }

        public void NephilaVisitPensImp()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.SomethingQuiteReal".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.FamiliarTinglingOf".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.ApproachFiveChainedImps".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.LookAtFourthImp".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.ClimbOnHim".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.FuckWithFeralLust".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.RemoveYourselfFromImp".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.ConcubinesBringFood".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensImp.NotFillingAsUnbirthing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 10f));

            Player.Thickness += 1 + Random.Shared.Next(3);

            Player.Orgasm("Vaginal");

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -1);

            Player.KnockUp(PregnancyIds.Imp, PregnancyIds.ImpIncubation - 24);

            ReturnToCamp(1);
        }

        public void NephilaVisitPensMinotaur()
        {
            ClearOutput();

            Player.SlimeFeed();

            Player.MinoCumAddiction(10);

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.DescendCurvedRampway".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.SmellOfMyriadSecretions".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.ArriveAtYourDestination".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.ThisRightHere".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.HandmaidensAreLounging".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.AfterMomentToEnjoy".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.SmackYourLips".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.PonderForAMoment".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.ThreatProvedEffective".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.WatchingTheDisplay".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.TwoMinotaursBattle".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.ContinuesForSomeTime".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.CooStretchingToEase".Localized());

            if (Player.Thickness <= 33)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.TonedBodyWord".Localized());
            else
                if (Player.Thickness <= 66)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.MuscularBackAndLimbs".Localized());
                else
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.PlumpJigglyFlesh".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.LazingOnYourDais".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.AfterWhileStop".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.SayThenOverCourse".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMinotaur.NotFillingAsUnbirthing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 10f));

            Player.Thickness += 1 + Random.Shared.Next(3);

            Player.Orgasm("Vaginal");

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -2);

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);

            ReturnToCamp(2);
        }

        public void NephilaVisitPensAnemone()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.FamiliarTinglingOf".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.SendTheCommand".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.AsWithAllAreas".Localized());

            if (Player.Thickness <= 33)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.LitheBodyWord".Localized());
            else
                if (Player.Thickness <= 66)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.MusclyBodyWord".Localized());
                else
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.PlushMotherlyFigure".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.SeveralPeelingOff".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.AnyMotherWouldAccept".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.EffortsSoonRewarded".Localized());

            if (Player.Body.Butt <= ButtPart.RatingAverage)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.PetiteAssCheek".Localized());
            else
                if (Player.Body.Butt > ButtPart.RatingAverage && Player.Body.Butt <= ButtPart.RatingJiggly)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.AssCheekWord".Localized());
                else
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.ThickWhorishAssCheek".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.BlushAsJoined".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.LetTheAir".Localized());

            if (Player.Thickness <= 33)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.ThinDangerouslyStretched".Localized());
            else
                if (Player.Thickness <= 66)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.RippedBodyTightPacked".Localized());
                else
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.PillowyCurves".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.BothYourPleasureTheir".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.WhenYouFinish".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensAnemone.NotFillingAsUnbirthing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 10f));

            Player.Thickness += 1 + Random.Shared.Next(3);

            Player.Orgasm("Vaginal");

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -1);

            Player.KnockUp(PregnancyIds.Anemone, PregnancyIds.AnemoneIncubation, 101);

            ReturnToCamp(1);
        }

        public void DecideToSparNephila()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.DecideToSparNephila.FeelLikeCouldUse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaSecondAscension) == 0)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.DecideToSparNephila.AsYouWishWarned".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.DecideToSparNephila.GooGirlToneIs".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.DecideToSparNephila.FollowHerTo".Localized());
            }
            else
            {
                OutputText("Scenes.Nephilacovenfollowerscene.DecideToSparNephila.WarnsPlacingProtective".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.DecideToSparNephila.AssureHerThatShe".Localized());
            }

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "nephila_coven")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void NephilaMouseCureInfertility()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility.ShakeHeadAndTell".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility.ButThenWhyDid".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility.ExplainSinceYouMet".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility.SmilingMeekly".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility.AskIfSheTrusts".Localized());

            NephilaMouseCureInfertility2();
        }

        public void NephilaMouseCureInfertility2()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.ArriveAtJuliennesHome".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.HisBlueEyesDart".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.GreetHimAndNote".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.MouseTurnsAway".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.RatherThanRespondWords".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.GooGirlFolds".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.StrugglingInYour".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.MatronAsks".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.MatronCutsHimOff".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.ThreeOfYourOther".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.PredatorySmileFlashes".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.RemovesTentacleFromInside".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.TheMouseBegs".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.GrinsAndReaches".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.ThreeCompanionsPushForward".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.WhisperingIntoRomanis".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.RomaniGroansAndWith".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility2.SistersReleaseRomani".Localized());

            NephilaMouseCureInfertility3();
        }

        public void NephilaMouseCureInfertility3()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.RomaniLeadsIntoHome".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.FacingTheLounges".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.RisingFromCouch".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.HandmaidensGlideArrayed".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.SmilingSinceYou".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.ReleasesGripFromRomani".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.ReachesTowardJulienne".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.RomaniGroansSlowlyComing".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.ResponseDoesntSurprise".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.HeAsks".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.ImmediatelyReturningCradling".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.SlowlyShakingOffHis".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.JulienneSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.JulienneDoesSoSupporting".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.MatronSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.JulienneSaysAgain".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.TurningToRegardHis".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.RomaniAsks".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.JulienneSaysThird".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.RomaniSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.SomethingWrongHere".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.RomaniSaysAgain".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.WithAMotionOf".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.LooksOverToYou".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.AssureJulienneThough".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseCureInfertility3.WithThatSaidThe".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.NephilaMouseOwned, 1);

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 10f));

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void NephilaVisitPensMouseFirst()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.WaitingByGilded".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.SheAsks".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.BehindTheDoor".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.ChildrenOpenTheDoor".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.PlumpenedOutlineOf".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.WatchTheMouseMorph".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.ThenCriesOutIn".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst.OhMyRomieCompany".Localized());

            NephilaVisitPensMouseFirst2();
        }

        public void NephilaVisitPensMouseFirst2()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.VastGrayHandReaches".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.LeaningForwardInHer".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.MoreAnOgre".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.ObviouslyEnormouslyPregnant".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.SwattingHerHusbands".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.LookingBackToYou".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.WatchJuliennesBelly".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.RomaniLowersJulienne".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.TuggingAtOneOf".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.MouseMorphLooksUp".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 2)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.SheSays".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 4)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.MinesBigger".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 9)
                        OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.SheAsks".Localized());
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 14)
                            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.SheSaysAgain".Localized());
                        else
                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 19)
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.VoiceGoingQuiet".Localized());
                            else
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.StillHaveA".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.AskHerIfShes".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.SheSaysThird".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.ContinuingRockingBack".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.StretchingPushingAgainst".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst2.RollAwayFromJulienne".Localized());

            NephilaVisitPensMouseFirst3();
        }

        public void NephilaVisitPensMouseFirst3()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.RomaniReEntersRoom".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.DontYouThink".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.GetIntoPosition".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.DirectsYouBehind".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.PositionYourselfSoThat".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.OverSeveralMinutes".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.StretchingHerLegs".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.LiftsHerArms".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.JulienneSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.PuffsHerselfUp".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.MouseGirlSays".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseFirst3.ThankMouseCouple".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f));

            Player.Orgasm("Vaginal");

            Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

            CoC.Instance.Flags.Set(GameFlagIds.NephilaMouseOwned, CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) + 1);

            ReturnToCamp(2);
        }

        public void NephilaVisitPensMouseRepeat()
        {
            ClearOutput();

            var girlNames = new[] { "Genny", "Cynthy", "Bella", "Shaaron", "Maia", "Tilly", "Mina", "Velly", "Saya", "Betty", "Sylvie", "Lola", "Reya", "Nami", "Briani", "Nicky", "Shirlie", "Rosie", "Gertie", "Sybil", "Tina", "Bella", "Zola", "Zelda", "Fiona", "Missy", "Ella" };

            string currentGirl = girlNames[Random.Shared.Next(girlNames.Length)];

            var boyNames = new[] { "Nottom", "Gerry", "Gil", "Verny", "Jole", "Kevvy", "Marty", "Nolan", "Nye", "Sammy", "Fry", "Buster", "Regi", "Raphi", "Nolan", "Nyles", "Richie", "Kai", "Toby", "Bobby", "Northrop", "Toddy", "Scott", "Vinny", "Arty", "Manny", "Timmy" };

            string currentBoy = boyNames[Random.Shared.Next(boyNames.Length)];

            var isGirl = Random.Shared.Next(2) == 0;

            string currentName = "";

            if (isGirl)
                currentName = girlNames[Random.Shared.Next(girlNames.Length)];
            else
                currentName = boyNames[Random.Shared.Next(boyNames.Length)];

            var childHeShe = isGirl ? "Common.Text.She".Localized() : "Common.Text.He".Localized();

            var childHimHer = isGirl ? "Common.Text.Her".Localized() : "Common.Text.Him".Localized();

            var childHisHer = isGirl ? "Common.Text.Her".Localized() : "Common.Text.His".Localized();

            var childMouseBoyGirl = isGirl ? "Common.Text.MouseGirl".Localized() : "Common.Text.MouseBoy".Localized();

            var childBrotherSister = isGirl ? "Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Sister".Localized() : "Common.Text.Brother".Localized();

            var childBrotherSister2 = isGirl ? "Common.Text.BrotherAndSister".Localized() : "Common.Text.TwoBrothers".Localized();

            string text = "";

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.VisitApartmentsMaking".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) == 2)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.FindJulienneAndHusband".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.SheSays".Localized(
                    currentName,

                    Player.Mfn("Uncle", "Aunty", "Bibi")
                ));

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.MakeWayToHer".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.AdjustsHerselfRomaniReaches".Localized(
                    childHimHer,

                    currentName
                ));

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.NodAndRoll".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.MouseGirlSaysCrushing".Localized(
                    currentName,

                    childHeShe,

                    currentBoy
                ));

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.ReachesDownIntoSecond".Localized(
                    currentGirl
                ));

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.PregnantMouseHanding".Localized(
                    currentBoy,

                    Player.Mfn("godfather", "godmother", "godparent")
                ));

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.RubsANervous".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.MouseGirlLeaningHer".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat.LeadsYouToApartments".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 2)
                {
                    text = RandomUtils.Choice(["Once there, you discover Julienne engaged in an argument with one of her adult daughters. The girl, perhaps equivalent in age to a human of seventeen or eighteen years, has a belly that is swollen so tightly with a brood of nephila parasites that the outlines of the crawling tentacles can be seen moving back and forth as large bulges jutting out from her skin. As you watch, she leans backwards, rubbing a soothing hand along the flank of her squirming gut while the other hand pushes into the small of her back. Seeing you, she waddles forward, pressing her tightly-packed belly against your own.\n\n[say: " + Player.Mfn("Uncle", "Aunty", "Bibi") + " [name],] the girl says, rubbing her distended belly button against your sensitive [skin] and batting her eyes at you, [say: Can you tell Mama she's being ridiculous?]\n\n[say: Ridiculous?] Julienne asks. [say: <b>Really,</b> " + currentGirl + "? <b>Ridiculous?</b> Look at this girl, [name], she's about to explode!]\n\nThe worried hyperpregnant mama is practically vibrating with rage atop the mountain of her own cum-stuffed pregnancy, rendering her statement a bit hard to take seriously, but you can't help but admit that " + currentGirl + " looks like she's been stuffing herself with parasites a bit too quickly for her own good. As the blushing teen leans against you, you can clearly feel her squirming brood projecting through her skin and yours. The sensation is both novel and arousing, but the way that the girl's flesh groans under the pressure, and the way she winces when she heaves herself backwards, turning to lecture her mother with legs spread wide to accommodate her bulging belly, indicates worrying things about her health.\n\n[say: I'm fine, Mom. Lavender would only let me hold more of her babies if she thought I could handle it. She <b>loves</b> me and my belly. Look at daddy--he loves us, too!]\n\nRomani had been lounging behind his wife, attached to her and her belly cart in the usual manner, and taking his position of relative unimportance as an opportunity to covertly ogle his daughter. Realizing that he's become the center of attention, he shakes his hands at his daughter and then, when his wife leans backwards to glare up at him, rubs one massive paw through the hair on his relatively small head, obviously embarrassed. Julienne elbows her husband, eliciting an 'oof' from him, then turns her attention back to her daughter. [say: Don't drag your father into this. " + currentGirl + ", we love you and want you to be safe. I won't tell you that you shouldn't be agreeing to help Lavender and the other nephila after they've been so kind to us, but you have to think of your health. Look at you. You can barely hold yourself together right now. Why are you <b>doing</b> this?]\n\nThe girl plants her hands on her hips. [say: Why do you think, Mama? Huh? Look at me? Look at you!] " + currentGirl + " pounds her foot on the ground and then groans in sudden pain as her abused belly contracts violently. She trips, falling backward, and you roll forward, catching her with your belly to keep her from hurting herself. As the mouse girl pants, womb heaving, several tentacles ooze out of her tight, teenage cunny, falling to the ground with wet plops and then joining your own brood as it swarms around the bottom of your bulk. After the birthing, the mouse girl rubs at her still engorged belly, wiping tears out of her eyes.\n\n[say: " + currentGirl + "!] Julienne cries, Romani rolling her forward until her gelatinous tum is softly enveloping her daughter, pressing up against you as it does so. [say: Are you alright?] the mouse woman asks.\n\n[say: Y-yeah,] the girl says. She sniffles for a moment, and then dislodges her arm from the crush of belly cleavage it is trapped in to pat her mother's stomach reassuringly. [say: Sorry, Mama. I'll be more careful.]\n\n[say: Hush now. I'm just glad you're alright,] Julienne says. [say: For now, " + Player.Mfn("Uncle", "Aunty", "Bibi") + " [name] is visiting, so why don't we enjoy her company?] It is only when she says this that you notice that the fist sized nipples at the ends of Julienne's hypermassive tits are standing at full attention--contact with your belly has touched off her hormonally amplified sex drive. You grin as you realize that, now that the mouse family crisis is averted, fun times are at hand.", "Once you arrive, you find the mouse family engaged in 'wholesome' familial bonding. With the aid of several handmaidens, Julienne is nursing one of her newborn children. The handmaidens scoop up mouse babies with their back tentacles, bouncing the cooing infants before lifting them, one at a time, to her massive, debilitating tits, her nipples so huge that the children are able to feed directly from her pores. Nearby, a dozen of Julienne's older daughters are cradling infants to their own milk-swollen breasts, helping to feed the ever-growing mouse brood that their mother births. Most of the mouse couple's older sons are out spending time with your nephila handmaiden daughters, but one is curled up with a book on a couch, reading over it, occasionally asking a question of Romani, who grunts in response but seems to be communicating just fine judging by the mouse boy's vigorous nod after each one.\n\nAs you approach, you notice that Julienne seems to be sleeping. You send your brood out to wake her so that you might feed, and, as the tentacles clamber up the exploded flanks of her cart-filling baby-bump, she groans, pushing against the flesh of her belly and breasts with her arms. Opening her bleary eyes, she rubs at her face, then smiles as she sees you.\n\n[say: Hi, [name],] she says. [say: That time already?]\n\nYou nod, and your handmaidens move into action, ferrying the younger children out of the room. That done, Julienne's daughters lumber up to you and her, pushing their bellies against each of you and whispering sweet nothings into your ears. Julienne's eyes glaze over with lust, and her lips part as she begins panting with the first onset of arousal. You grin. The orgy has begun.", "When you arrive, you find that Julienne and Romani are not there. Inquiring, you discover that they have yet to leave the new, expanded bedroom that the nephila have cleared out for them. You order your brood to carry you to the bedroom and, when you get there, can't help but smile in amusement.\n\n[say: [name]!] Julienne exclaims. [say: Romani is being a useless lunk. Tell me: does this top make me look fat?]\n\nThe mouse woman has managed to just barely cram her ludicrous tits into a white silk top. As she asks you her question, she tugs on the shirt, setting her jelly-like milk-boulders wobbling ominously in the creaking garment. The agitation sets her nipples spurting, and, within short order, large damp spots spread out in growing circles on the front of each breast. Julienne doesn't seem to notice this.\n\n[say: Well?] she asks. [say: Every time I ask, this useless husband of mine just grunts.] The hulking, muscle-bound brute in question grunts as he leans against the rear swell of her belly, half buried in her voluminous ass cleavage. His eyes are lidded and he is obviously bored. Julienne leans sideways against her massive belly and raps her knuckles on his rippling pectorals. [say: Yeah, I'm talking about you, you jerk. Listen! [name] is going to tell it like it is. So, [name], what do you think?]\n\nYou consider telling the agitated broodmother that you don't know, or perhaps that you're just here to get laid, but the manner in which she asked the question indicates that saying either of these things is, most likely, just going to encourage her to cause a fuss. After a minute's consideration, all the while making a show of stroking your chin as you watch her swivel her hypertrophied rack back and forth, dragging it in wobbling semicircles on the surface of her cum-stuffed gut to give you 'different angles' to judge, you decide that discretion is the better part of valor, proposing that she call in a few of her children to judge for her.\n\n[say: Hmm,] the mouse woman says, blowing out her cheeks. [say: Very well.] She asks a nearby attending handmaiden to call some of her children over, and, shortly, three young mouse studs--some of her older children--walk in.\n\n[say: What did you want, Mama?] one of the boys asks. He's rugged and surprisingly ripped for a mouse, and he is an age equivalent to, perhaps, nineteen human years.\n\n[say: " + currentBoy + "!] Julienne says, [say: Look at me in this blouse and tell me if I look alright to you.]\n\nThe mouse boy gapes as his mother plays with her tits in her blouse, 'modeling' her already soaked and ruined garment for his appreciation. [say: Well?] Julienne asks.\n\n[say: Umm...] the boy says, his cock visibly bulging up in his linen pants. Though not as well endowed as their heavily mutated father, Julienne and Romani's mouse sons are almost universally pleasingly hung after growing up in proximity to nephila handmaidens, and, as a result, " + currentBoy + "'s dick tents his pants to a degree that makes you almost as worried for the overburdened garment as you are for the soaked silk blouse that his mother is obliterating for his pleasure.\n\n[say: " + currentBoy + ". Well? <b>Come</b> on, what do you think?] Julienne asks. With a loud grunt, she lifts her tits up in her blouse and drops them, setting them to sloshing back and forth violently. Her shirt finally gives up the ghost and rips down the middle, allowing her cannonball sized honeydew melons to flow forward from the hole like a gelatinous avalanche, her freed, naked nipples spurting milk. The distraught mouse boy clearly can't take it anymore, and he frees his dick from its restraints, stroking his cock to the sight of his mother's wobbling tits and setting her gasping. With a groan, he ejaculates, sending a spray over the front of her hypermassive belly and tits.\n\n[say: <b>" + currentBoy + "</b>!] Julienne says, shocked but also evidently aroused. [say: That... oooooooh... oh Romie, <b>now</b>?] Romani, himself driven to sexual distraction by his wife's antics, has reached forward and is mauling her naked breasts. The third boy to enter the bedroom leans out of the door and shouts, [say: Orgy at Mom and Dad's!] and, shortly, the bedroom is swarming with Romani and Julienne's sexually ready progeny."]);

                    OutputText(text);
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 10)
                    {
                        text = RandomUtils.Choice(["When you arrive, you find Julienne and Romani playing with some of their children in an unusual manner. The kids, a mouse boy, a mouse girl, and another " + childMouseBoyGirl + " chat with each other, giggling as they bounce up and down on their mother's colossal, gelatinous tummy. Julienne's belly dwarfs her, leaving the rest of her body looking like an afterthought compared to it, and it sloshes in rippling semi-circles as her children bounce back and forth on top of it, making it wobble over the edges of the large, padded brass cart that it is resting in. As you enter, one of the children throws herself on top of her mother's voluminous, fabric-straining tits, hugging her shoulders and kissing her on the cheek. Julienne rubs noses with the little girl, hugging her, then lets her go so that she can roll backward off her breasts and rejoin her " + childBrotherSister2 + ". The mouse woman waves at you as your roll toward her, then shoos her kids, sending them skipping out of the nursery.\n\n[say: " + currentBoy + "! " + currentGirl + "! " + currentName + "!] she cries out. [say: Don't run so fast! Be careful.] By the time she's finished speaking, her children are already out of view. Shaking her head, she reaches back to pat her husband's chest, urging him to wheel her to their bedroom so that you can feed.\n\n[say: I love those little rapscallions,] Julienne says, [say: but they're not much for listening to their parents. Have you ever had trouble with your little ones?]\n\nYou shake your head, absolutely convinced that you are in perfect unity with your children in your endeavor to grow ever larger and produce a greater brood.\n\n[say: No?] the mouse girl asks, tugging at her ear. [say: Well, you always were impressive, [name]--ah! We're here.] And with that, you arrive at the bed chambers that the mouse couple share.", "When you get there, you find the mouse couple spending some time with one of their children, a young girl from one of their middle broods, barely out of toddlerhood. The girl has stuffed a cushion from one of the nursery's lounge chairs under her shirt and is waddling around with grave sincerity. Julienne is leaning forward on her colossal belly, her small, pensive upper body looking positively tiny at the top of the massive, cum-filled dome. She's resting her elbows on her colossal, pregnant-belly-sized tits and cradling her cute, sharply angled chin in her palms as her arms jut up from her cleavage. As the little girl waddles around, Julienne giggles, leaning even further forward and clapping her hands, making her belly slosh back and forth on the large brass cart it is 'contained' in. The cart begins to roll as a result of Julienne's movement, and Romani has to steady it with both hands, but Julienne's too engrossed in her daughter's pantomime to notice. The little girl turns around and sees you watching by the door. She waddles toward you and then presses her 'belly' against yours. [say: Look, " + Player.Mfn("Uncle", "Aunty", "Bibi") + " [name], I'm mama!] the girl says, puffing out her cheeks and acting out being weighed down by a massive pregnant stomach.\n\n[say: " + currentGirl + ". Don't bother your " + Player.Mfn("Uncle", "Aunty", "Bibi") + ",] Julienne says. [say: " + Player.Mfn("he's", "she's", "they're") + " very busy.] The girl pouts, but relents from pestering you, dropping the cushion and rushing off to bury her face in the side of her father's leg. Romani reaches down and scoops her up, placing her on his shoulder, where she kicks her legs and whispers in his ear, then he sets her on the ground and she runs out of the room, likely to go to the dorms reserved for the children not quite old enough to care for their own individual quarters. [say: Sorry,] Julienne says, tugging at an earring. [say: It can be pretty hard to explain the 'birds and the bee girls' when we're, you know.] The mouse woman gestures at her insanely swollen belly.\n\nYou ask Julienne if she regrets what has happened to her, and she shakes her head emphatically. [say: No,] she says. [say: Of course not. Without all this, " + currentGirl + " wouldn't even exist. Neither would any of her sisters or brothers. Don't worry about it, [name].] The woman pushes at the flanks of her monumental tits, batting her eyes at you while her tail whips back and forth, framed by her vast, heart shaped ass. [say: Anyway... don't you think there's something you'd rather do than talk, right now?]\n\nWith that, you, Romani, and Julienne retire to the couple's bedroom.", "When you arrive, you find that Romani--huge dick and balls carefully concealed in a baggy pair of leather-reinforced silk pants--is playing with some of his younger children, throwing them up and down in the air and play-wrestling with them as several of your handmaiden daughters care for Julienne's newborn pinkies. Of the hyperpregnant mouse girl there is no sign, and, after inquiring, you discover that she is soaking in the complex's dedicated baths.\n\nYou make your way to the baths and find Julienne reclining in their central pool, her massive belly extending a third of the way into its 25 meter length and bobbing up and down as she lies back in the steaming water, a bath towel on her forehead. Nearby, the brass cart that she now requires in order for her muscle-bound husband to be able to move her around lies empty.\n\nYou roll forward and then settle into the water next to your mouse friend, squashing your bellies together and forming a long line of cleavage that quickly glistens with sweat under the influence of the steaming liquid. For a while, Julienne says nothing, and neither do you.\n\nYou both soak in the comfort of the baths and each other's company. When you've been soaking for several minutes and your aching muscles and swollen, tight-packed belly feel fully relaxed and soothed, Julienne removes the towel from her forehead and turns her head to look at you. [say: You feel so nice, [name],] the mouse woman says. As she speaks, she rubs at the flank of one of her tits, and you can't help but feel aroused by the way [if (biggesttitsize > 0) {her monstrous left breast crushes against your [allbreasts].|her monstrous left breast overflows her sides, crushing up against your belly.}]\n\n[say: Like what you see?] the cum-filled broodmother asks, tracing your gaze. [say: I'd give you a show, but my boobies are <b>wet</b> and so, <b>so big</b>, and I'm just <b>so</b> small. Maybe your babies could help us out?]\n\nYou grin and send your children out to swarm up and down the hyperpregnant mouse's hugely bloated form. One of your larger tentacle babies worms its way between Julienne's tits, and the girl smiles, reaching down into her cleavage to guide it up to her lips to kiss.\n\n[say: Does this remind you of anything, [name]?] she asks. She then coos as your tentacles ooze all around her, exploring her body and teasing her various orifices and obscene bulges. As one of your children swims up underneath you both, rubbing against your pussy lips and hers at the same time, she groans. [say: <b>Ooooh</b>--but I'm so much <b>bigger</b> now. You've blown me up, [name]. You and Romie--oh, <b>hi</b> Romie!]\n\nYou lean backwards and look to see that Romani has entered, stroking his huge dick as he watches you molest his wife with the tentacles infesting your womb. You can feel your brood clamoring for a meal, and you consider 'popping' Julienne right there, but settle for teasing both her and her mouse ogre husband, sending both into crashing orgasm as your babies squirm all around their bodies, squeezing and rubbing every bulge that they can find. The water of the pool turns frosted white as both begin leaking mouse cum into it, and this piques your hunger enough that you decide you've had enough with appetizers and are ready for the main course. You help Romani lift his wife out of the bath and into her cart, then lead them both to the bedroom they share.", "You don't find Julienne and her husband when you get there, and so you make your way to the bedroom that the mouse couple shares, expecting that they might be sleeping. Entering their chambers, you discover the two engaged in a different activity. Romani, with the aid of no less than five of your nephila daughters, is pushing and pulling on Julienne's massive tummy, trying to heave its liquid-like mass off their out-sized bed and into a clearly inadequate brass cart. Julienne has grown so large that, even with this combined effort, the group is struggling.\n\n[say: Come on, heave!] one of your daughters exclaims, shoving her sizable belly and tits into the mouse girl's gelatinous mass.\n\n[say: This. Is. Going. To. Take--magic!] another daughter says, her back tentacles shaking as they try to gain purchase on the front of Julienne's belly by invading the mouse girl's deep belly button, pulling on its inner walls and eliciting moans from the poor glorified cum-balloon every time her tentacles piston in and out of her.\n\nRomani snorts. He is at his wife's rear, massive, cum-leaking dick waggling back and forth as he shoves backwards on the rear swell of her tum. His body sinks deep into Julienne's body with each shove, causing her to bulge and wobble, but not doing much to actually budge her from the bed.\n\nJulienne sits atop her belly, hiding her face in her hands and blushing so intensely that red can be seen under the pale fur on her cheeks and in her cleavage.\n\n[spoiler: Every day,] Julienne says, mumbling into her hands. [spoiler: Why does this have to happen every day.]\n\nYour closest daughter gives you an entreating look, and you decide to help out, sending a wave of tentacle babies forward to squirm underneath Julienne's body so that, with one final heave, your collected efforts finally roll her onto the cart. Romani and the others sigh, congratulating each other on a job well done, but their enthusiasm comes too soon as, with a piercing shriek, the brass belly cart's wheels pop off under the stress of carrying Julienne's weight, causing the mouse girl to fall with a squeak of surprise and setting her massive tummy to jiggling. Looking at her, you can't help but notice, now that the cart has collapsed, that her belly overhangs it so severely that it pools on the ground, and, except for the two handles projecting out from beneath the rear of her belly, none of the erstwhile mobility aid can be seen.\n\n[say: Already?] one of your handmaidens says.\n\n[say: Hush, Mother's here,] another says, hissing. She looks at you and leans over her tentacle-packed belly, raising her hands in defeat. [say: No worries, Mother. We were prepared for this.] As she says this, a third daughter wheels in a new cart, larger and sturdier than the old. With your help, the handmaidens manage to transfer Julienne to the new cart, and the mouse girl fits in it much more comfortably, if a bit snugly. Their job done, the handmaidens excuse themselves to their other duties caring for Julienne's massive brood, and you're left alone with Romani and a mortified Julienne. As she apologizes for 'inconveniencing everyone,' again and again, her husband steps up behind her, patting her on the back and instinctively and automatically slipping his massive dick in her waiting snatch as he does so."]);

                        OutputText(text);
                    }
                    else
                    {
                        text = RandomUtils.Choice(["When you arrive at the nursery, you discover, to your surprise, that the mouse couple is engaged in a furiously competitive game of chess. Julienne is reclining sideways on a chaise longue, her massive, liquid gut pooled on the ground in front of her as she reaches over the head rest of her lounger to reposition a knight. As she maneuvers, she sets her swollen, head-sized tits wobbling, precipitating a nearby handmaiden nursery worker to scoop up an infant mouse child that had been suckling at her nipple.\n\n[say: Oh my! Is " + currentBoy + " alright?] she asks. The worker nods, and Julienne sighs in relief. [say: My poor baby,] she says. [say: Mommy's just getting so big, now. What would she do without [name]'s daughters to help her care for you? Speaking of--[name]! It's such a pleasure to see you!]\n\nLooking over to you, the mouse girl waves, beckoning you to sit next to her husband, who is looming over the chess table opposite to her, massive cock and balls barely contained in a worked leather holster. As you roll toward him, he nods, never taking his eyes off of the game board. As you settle next to Romani, a trio of young mouse girl children come burbling in from another area of the apartments. They chatter with your nephila daughter as she settles the infant " + currentBoy + " into one of the many, many cradles dotted throughout the room. The nursemaid shoos them so that she can continue her duties, and the mouse girls run past you, each burbling a quick [say: Hi, " + Player.Mfn("Uncle", "Aunty", "Bibi") + " [name]] before clambering up their mother's belly to lay down and watch their parents play. The girls are far out of her reach, but Julienne pats the flank of her colossal tum affectionately to let them know that she's happy to see them.\n\n[say: Do you think you're winning, Mama?] the first mouse girls asks.\n\n[say: Why of course!] Julienne exclaims, puffing herself up. [say: Your Daddy is smart, but Mama is much too clever to fall for his tricks.]\n\n[say: You've never won a game against Daddy,] the second girl says.\n\nJulienne coughs. [say: Well, that is, you see...] she says.\n\n[say: I think Mama's gonna lose again. What do you think, " + Player.Mfn("Uncle", "Aunty", "Bibi") + " [name]?] the third girl says, hopping off of her mother's belly to run forward and push against yours. [say: Do you think Mama's gonna lose again?]\n\n[say: " + currentGirl + "!] Julienne says. She wags her finger at her daughter as the little girl giggles. [say: Show Mama some respect,] the poor, beleaguered woman says. It is at this moment that Romani, who has been saying nothing, grunts and pokes a knight into position at the rear of the board with one massive finger. As Julienne lectures her daughter, he leans back, a self-satisfied smile on his face. You look at the board for a moment, then point to it and explain to Julienne that, if you're not mistaken, she seems to be in checkmate. The mouse woman startles and looks back toward the game, then groans, tugging at one ear. Her daughters, all giggling together, rush off out of the room.\n\n[say: <b>Again</b>?] Julienne asks. Turning to look at you, she sighs and shakes her head. [say: Well, I should have seen that coming. Anyway, [name], I'm sure you're not here to see me wallow in defeat. Shall we retreat to the bedroom?]\n\nWith the aid of several of your handmaidens, Romani hooks his wife into her belly harness, and the three of you retreat to the bedroom so that you can feed.", "When you arrive at the nursery, you find that Romani and Julienne are not there. You ask one of the nephila handmaidens working there where it is the two conjoined lovebirds have hidden themselves, and the handmaiden points you in the direction of an anteroom that the two apparently retire to when they want to take a moment away from their brood of young children.\n\nYou have your parasite 'babies' carry you to the room and, when you get there, enter as quietly as your condition allows. Inside the room, you find the mouse couple spending 'quality time' together, lost entirely in each other's company. Julienne is laid out on a lounger, massive belly splayed in front of her. A selection of choice grilled meats have been spread out over the top of her colossal tum, and, as you watch, Romani leans over her, using chopsticks to scoop the meat up off of her belly to plop into his mouth to eat. As he does so, Julienne plays with his dick, jerking him off and licking up and down his churning sack. Her small hands look absolutely tiny as they work his massive cock--she can barely reach a quarter of the way around him.\n\nAs Romani breaks his repast by eating it off of his lover's swollen, wobbling belly, he covers her in a steady stream of jizz. The giggling cum slut basks in the treatment, taking breaks from running her hands up and down his cock in order to work the liquid protein into her fur and skin. When he has finished his meal, Romani folds his chopsticks together and places them on a nearby table, then rubs the top of his wife's belly, spreading the grease and cum covering it even further over the turgid dome.\n\nJulienne takes hold of one massive tit with both hands and then squeezes it, sending a stream of milk flying out of her nipple, straight into her husband's waiting, open mouth. Grinning, the mouse ogre pounces, abusing her tits and kissing her, sharing the milky bounty in his mouth with his considerate spouse. Julienne groans and shudders in sudden orgasm, causing the little golden chain piercing on her distant, cavernous innie belly button to jitter, then, catching sight of you around her husband's form, she jumps in surprise, as far as her massive, immobilized condition will let her and, pushing at him so that he'll retreat and give her space, reaches forward as far as she can to just <b>barely</b> cover her nipples with her dainty hands.\n\n[say: [name]!], she says. [say: We... uh... we didn't know you were coming to visit. Romie--quickly now!] Romani lifts his wife into the air, lowering her onto his dick, then hooks her into her belly cradle. All the while, Julienne is 'hiding' her tits with her completely inadequate hands. Noticing you watching her cover herself, the mouse woman sighs and drops her deathgrip on her milk pillows, letting them loll naked on top of her wobbling tum.\n\n[say: Sorry,] she says. [say: That was silly. Why don't we get down to business?] That said, the hyperpregnant mouse girl and her husband lead you to their bedroom.", "Once you get there, you find that the nephila handmaidens working in the nursery have led the older children away so that the mouse couple can spend time with their infants. Amazingly, the nursery is quiet, and the two are curled up together on a couch, Julienne's massive belly pointing up into the air as it is lifted and supported by the harness that she is still hooked into. As you watch, the dreaming Julienne groans, little fists pushing into the overwhelming swell of her belly. Her sleeping lover has thrown one heavy arm around her and is crushing down on her sensitive, liquid-filled gut, making it bulge and wobble as he pulls her toward him.\n\n[say: Romi, sto-o-op...] she says, slowly opening bleary eyes as she wakes. [say: Tickles... Oh, mm... hey, [name]]. As she speaks, the mouse woman stretches in her cradle, yawning and setting her various bulges jiggling even more than they were already. She reaches up and pats at her husband's chest.\n\n[say: Come on, sleepy-lug. Wake up. [name]'s here,] she says. When the mouse ogre doesn't respond, your hyperpregnant friend, obviously annoyed, starts jostling back and forth in her cradle, trying to wake her husband up. Her maneuvers have unintended consequences, as the still unconscious Romani grins, and, after a moment, Julienne groans. [say: Oh, no,] she says, shooting you a pleading look. [say: Help.]\n\nBefore your eyes, Julienne's already tightly constricted belly bubbles outward, swelling at a rapid rate as it fills with her sleeping husband's cum. Julienne strains, and moans, and pushes down at her growing body to try to keep it under control, but to no avail. Over the course of about a minute, her belly bulges over the lip of its cradle, creating a massive, wobbling muffin top that Romani continues to maul like a toddler with an overstuffed teddy bear.\n\nJust when you think that you're going to have to intervene to save your jism-stuffed mouse friend from possible injury, her husband wakes up, releasing her belly from his vice-like grip as he reaches up with his sizable mitt to rub a spot of drool off his lower lip. Julienne sighs in relief as her tummy bounces and wobbles, finally freed from danger, but her sigh quickly turns into a squeak of surprise as, with a loud rip, her harness's belly cradle snaps open as the brass chains holding it in place give way. She spills out of her harness, falling off the couch and smacking onto the floor belly-first. [say: Oh, Romani, you beast!] Julienne says, rubbing at what little of her belly she can reach and crying. [say: Look at what you've done to my poor tummy!] The startled Romani rises, hovering over his wife, not sure what to do--leaking huge quantities of cum as he mills around--and it's only when Julienne sighs, again, this time theatrically, that the worried mouse ogre seems to relax.\n\n[say: It's okay, Romie,] the mouse woman says, reaching up to rub one of her husband's fingers. [say: I'm made of tougher stuff than that. Just help me up so that we can assist [name] with " + Player.Mfn("his", "her", "their") + " <b>little problem</b>, okay?].\n\nRomani prepares himself and, exhibiting exaggerated caution, rolls his wife backward so that she can plant her feet on the ground. He then lifts her belly so that she can walk, with him backstepping while cradling her disabling bulk. Thus enabled, Julienne bustles forward, leading you to the bedroom. ", "When you arrive, you find the mouse couple engaged in a game of hide-and-go-seek with some of their older children. As you watch, Romani lowers himself and Julienne to the ground so that she can squeeze her massive belly and sizable tits into the spacious fireplace, currently unlit, that keeps the underground nursery warm. Julienne directs Romani to rotate them this way and that, trying to work her way into what, for most other sentient creatures, would be a firebox large enough to jump around in. All the mouse girl and her lover manage to do, however, is set her huge, globular belly wobbling as they cover themselves in soot.\n\n[say: " + currentBoy + ", I swear to Lethice, if Daddy and Mommy have to go to their room to undock just so daddy can get you out of this fireplace,] the hyperpregnant mouse woman says, growling, [say: the regret you'll feel when daddy finally gets you out of there will be talked about in our family for the rest of your life!]\n\nThe response to her threat? Giggling.\n\nTwo children, a mouse girl and her " + childBrotherSister + ", come scampering up to you. [say: Hi, " + Player.Mfn("Uncle", "Aunty", "Bibi") + " [name],] the girl says, tugging her slightly younger " + childBrotherSister + " forward, clearly showing off by talking in a familiar manner to you. You humor the child and nod hello, then go back to watching the enraged Julienne bounce her massive belly against the fireplace, cracking the wood it's made of with a loud 'smack!' and setting soot billowing outward, right into her sputtering face.\n\n[say: Mama and Daddy aren't very good at 'hide-and-go-seek,'] the little girl says. You nod again, inclined to agree with her. The girl reaches out for your [hand], and you let her play with it, showing off how it's different from hers to her " + childBrotherSister + ", who's shyly watching your exchange while sucking " + childHisHer + " thumb. [say: Maybe we should get one of the Slimy Aunties to find " + currentBoy + "?] the little girl asks, looking up at you. [say: They're <b>really good</b> at 'hide-and-go-seek.']\n\nYou have your nephila slime babies send a signal to their older sisters, and, shortly, three determined looking handmaidens glide into the nursery. Romani, on seeing them, steps back from the fireplace, pulling his angry, muttering wife back with him, and, soon, one of your older daughters has compressed her gelatinous form into a thin layer of ooze, sending her questing tentacles up the fireplace's flue to extricate a filthy--and thoroughly self-amused--mouse boy from out of it. The two young mice who were visiting with you wave goodbye as the children are ushered out of the nursery to their bedrooms, and then you're left alone with Romani and Julienne in a room of sleeping infants.\n\n[say: Hi, [name],] Julienne says, finally taking notice of you. [say: Sorry it took so long to say hi. It's just--<b>that " + currentBoy + "</b>--when I get my hands on him--]\n\nAs she starts working herself up to a tirade, Romani reaches down with one massive paw and gently pats her on the head. The mouse woman throws her hands up in the air. [say: Fine! Fine!] she says. She adjusts her white silk top, which, in addition to being covered in blackened soot, has been thoroughly soaked by the constant leaking of her jostling tits. As she attempts to pull the blouse back into position, she only makes things worse as creamy breastmilk starts squirting through the sheer, filth-covered material, carving little white, 's' shaped trails in the sooty fur on the top of her belly. When she notices the direction of your gaze, Julienne grins and plays up the lewdness of her activity, squashing her tits and soaking her upper belly with thick sprays of milk.\n\n[say: " + currentBoy + " gets off easy today,] Julienne says, flashing you a wink. [say: Looks like I'll be working off my frustration before I lecture him.]\n\nThat said, Julienne and Romani lead you to the room at the center of the apartments they use as their private bedroom. "]);

                        OutputText(text);
                    }

            NephilaVisitPensMouseRepeat2();
        }

        public void NephilaVisitPensMouseRepeat2()
        {
            ClearOutput();

            var girlNames = new[] { "Genny", "Cynthy", "Bella", "Shaaron", "Maia", "Tilly", "Mina", "Velly", "Saya", "Betty", "Sylvie", "Lola", "Reya", "Nami", "Briani", "Nicky", "Shirlie", "Rosie", "Gertie", "Wilhelmina Princessa Esq., the third,", "Sybil", "Tina", "Bella", "Zola", "Zelda", "Fiona", "Missy", "Ella" };

            string currentGirl = girlNames[Random.Shared.Next(girlNames.Length)];

            var boyNames = new[] { "Nottom", "Gerry", "Gil", "Verny", "Jole", "Kevvy", "Marty", "Nolan", "Nye", "Sammy", "Fry", "Buster", "Regi", "Raphi", "Nolan", "Nyles", "Richie", "Kai", "Toby", "Bobby", "Northrop", "General Custard Esq., the third,", "Toddy", "Scott", "Vinny", "Arty", "Manny", "Timmy" };

            string currentBoy = boyNames[Random.Shared.Next(boyNames.Length)];

            var isGirl = Random.Shared.Next(2) == 0;

            string currentName = "";

            if (isGirl)
                currentName = girlNames[Random.Shared.Next(girlNames.Length)];
            else
                currentName = boyNames[Random.Shared.Next(boyNames.Length)];

            Player.SlimeFeed();

            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 10 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 2)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.AdultSonsAndDaughters".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.MouseCoupleDaughters".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.PsychicBacklashOfPleasure".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.AfterYourBroodHad".Localized(
                    Player.Mfn("Uncle", "Aunty", "Bibi")
                ));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 10)
                {
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.WheelsColossalBulk".Localized());

                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.OnceYouAndJulienne".Localized());
                }
                else
                {
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.LowersJulienneOntoBed".Localized());

                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.NotWastefulFood".Localized());

                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.FeelOrgasmApproaching".Localized());
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 25 && CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 1)
            {
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.SatisfiedMakeWayBack".Localized());

                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.GooSlutPrimusSays".Localized());

                Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 5f));

                Player.Orgasm("Vaginal");

                NephilaMouseMatronsProposal();
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 2)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.SatisfiedWayBackAgain".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 20)
                        OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.SatisfiedWayBackThird".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) <= 10)
                            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.SatisfiedWayBackFourth".Localized());
                        else
                            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseRepeat2.SatisfiedWayBackFifth".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 2)
                {
                    Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -4);

                    Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Toughness, 2f));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) >= 20)
                    {
                        Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -3);

                        Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 5f));
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) <= 10)
                        {
                            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -1);

                            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 5f));
                        }
                        else
                        {
                            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, -2);

                            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 5f));
                        }

                Player.Orgasm("Vaginal");

                CoC.Instance.Flags.Set(GameFlagIds.NephilaMouseOwned, CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) + 1);

                Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);

                ReturnToCamp(2);
            }
        }

        public void NephilaMouseMatronsProposal()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronsProposal.MildlySurprisedAnnoyed".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronsProposal.OozingForwardSheSidles".Localized());

            if (Player.Ears.Id == BodyPartTypes.Echidna)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronsProposal.FingerAlongYourCheek".Localized());
            else
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronsProposal.FingerAlongTheRidge".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronsProposal.PressesIntoYouWhispering".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronsProposal.SheSays".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

            ClearButtons();

            AddButton(1, "Common.Button.Makeitso".Localized(), NephilaMouseMatronYes);

            AddButton(1, "Common.Button.No".Localized(), NephilaMouseMatronNo);
        }

        public void NephilaMouseMatronYes()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.InformMatronThat".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 2)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.WrappingThemHeavily".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 4)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.WrappingThemImpossibly".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 9)
                        OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.BarelyManagingToWrap".Localized());
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 14)
                            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.PressingIntoFlanks".Localized());
                        else
                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 19)
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.HuggingWhatLittle".Localized());
                            else
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.CaressingSmallQuantity".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronYes.ThenIsGone".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.NephilaMouseMet, 3);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 20f));

            ReturnToCamp(2);
        }

        public void NephilaMouseMatronNo()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronNo.CalmlyExplainToMatron".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaMouseMatronNo.SighAndRoll".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.NephilaMouseMet, 2);

            ReturnToCamp(2);
        }

        public void NephilaVisitPensMouseBadEndRepeat()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.HavePalanquinPull".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.AroundArenaHeavilyPregnant".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.MouseBoysNotFilling".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.SimultaneouslySeenUnseen".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.MakeWayUpRamp".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.ApproachHerCoreFrom".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.MumblesOverAndOver".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 2)
                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.BodyWord".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 4)
                    OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.BodyWordAgain".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 9)
                        OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.SizableBodyWord".Localized());
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 14)
                            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.EnormousBellyWord".Localized());
                        else
                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 19)
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.RoomFillingBelly".Localized());
                            else
                                OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.BuildingSizedBelly".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.ThroughIt".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.HandmaidensNodApproach".Localized());

            OutputText("Scenes.Nephilacovenfollowerscene.NephilaVisitPensMouseBadEndRepeat.FormerSelfFeltGuilt".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 10f));

            Player.Orgasm("Vaginal");

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

            ReturnToCamp(2);
        }

    }
}