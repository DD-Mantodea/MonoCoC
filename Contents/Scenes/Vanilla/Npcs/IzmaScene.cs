using MonoCoC.Contents.World;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<IzmaScene>("coc:izma")]
    public class IzmaScene : Scene
    {

        public bool gooJobbed;

        public int nightmareDaughterType;

        public string nightmareParent = "";

        public static string Pluralize(int count, string singular, string? plural = null) => count == 1 ? singular : plural ?? singular + "s";

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Izma);

        public string debugName => "Izma";

        public string debugHint => "";

        public override void Entry()
        {
            ExecEncounter();
        }

        public void Reset()
        {

            Save.SetInt("sharkgirlsDeflowered", 0);

            Save.SetInt("tigersharksDeflowered", 0);

            Save.SetInt("daysSinceAneFight", 0);

            Save.SetBool("tonguedButt", false);

            Save.SetBool("kidDick", false);

            Save.SetBool("lezDemonstration", false);

            Save.SetBool("sparred", false);

            Save.SetBool("previousVictoryTeased", false);

            Save.SetFloat("lastNightmare", 0);

            Save.SetBool("izmaMorning", false);

            Save.SetBool("gaveBooks", false);

            Save.SetBool("sharkManTalk", false);
        }

        public bool IzmaFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1;
        }

        public bool IzmaAtCamp()
        {

            return IzmaFollower() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0;
        }

        public void IzmaSprite()
        {

        }

        public int TotalIzmaChildren()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks);
        }

        public bool HasCock()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0;
        }

        public void ExecEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaWormsScared) == 1)
            {
                OutputText("Scenes.Izmascene.ExecEncounter.IzmaSpotsYouApproaching".Localized());

                Player.UnlockCodexEntry(GameFlagIds.CodexEntrySharkgirls);

                CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, 5);

                CoC.Instance.Flags.Set(GameFlagIds.IzmaWormsScared, 0);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) == 1)
                {
                    OutputText("Scenes.Izmascene.ExecEncounter.FigureOnHorizon".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.HesitateBeforeSpeaking".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.StrangerDropsBook".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.ExplainYourExploration".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.GrowingCuriosity".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.SheIntroducesHerself".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.AskAboutTigershark".Localized());

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntrySharkgirls);

                    OutputText("Scenes.Izmascene.ExecEncounter.ExplainTigersharkMutation".Localized());

                    if (Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1)
                        OutputText("Scenes.Izmascene.ExecEncounter.OutsideTelAdreSafety".Localized());
                    else
                        OutputText("Scenes.Izmascene.ExecEncounter.SinceYourArrival".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.KindredSpiritOffer".Localized());

                    OutputText("Scenes.Izmascene.ExecEncounter.NodInAgreement".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) < 4)
                    {
                        OutputText("Scenes.Izmascene.ExecEncounter.RevisitLakeshoreCamp".Localized());

                        OutputText("Scenes.Izmascene.ExecEncounter.AskWhatSheOffers".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) == 4)
                        {
                            OutputText("Scenes.Izmascene.ExecEncounter.FamiliarCampReturn".Localized());

                            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySharkgirls);

                            OutputText("Scenes.Izmascene.ExecEncounter.ShaftFlopsFree".Localized());

                            OutputText("Scenes.Izmascene.ExecEncounter.LookingSkywardAvoiding".Localized((CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0 ? " or a cultist" : "")));

                            OutputText("Scenes.Izmascene.ExecEncounter.ConsiderTheOffer".Localized());

                            OutputText("Scenes.Izmascene.ExecEncounter.ExplainRollingTooth".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                            OutputText("Scenes.Izmascene.ExecEncounter.ExplainTwoWays".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == -1)
                            {
                                OutputText("Scenes.Izmascene.ExecEncounter.StrollAlongLake".Localized());

                                Player.UnlockCodexEntry(GameFlagIds.CodexEntrySharkgirls);

                                OutputText("Scenes.Izmascene.ExecEncounter.WarmGreetingHug".Localized());
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= 6 && CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 0)
                                {
                                    OutputText("Scenes.Izmascene.ExecEncounter.HopeToMeetIzma".Localized());

                                    OutputText("Scenes.Izmascene.ExecEncounter.StepOnBrittleShell".Localized());

                                    OutputText("Scenes.Izmascene.ExecEncounter.AwkwardBeginningTrailsOff".Localized());

                                    OutputText("Scenes.Izmascene.ExecEncounter.StaresAtSand".Localized());

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Accept".Localized(), AcceptIzmaAsYourBitch);

                                    AddButton(1, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), IzmaStayAtTheLakeBitch);

                                    return ;
                                }
                                else
                                    OutputText("Scenes.Izmascene.ExecEncounter.WavesYouOver".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Borrow".Localized(), TradeWithFuckingSharkBitches);

            AddButton(1, "Common.Button.Talk".Localized(), TalkToASharkCoochie);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) >= 4)
            {
                AddButton(2, "Common.Button.Sex".Localized(), () => { if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == -1) IzmaLakeTurnedDownCampSex(); else ChooseYourIzmaWeapon(); });

                AddButton(3, "Common.Button.Fight".Localized(), FightSharkCunt);
            }

            AddButton(14, "Common.Button.Leave".Localized(), LeaveSumSharkPussyOnTheBeach);
        }

        public void TradeWithFuckingSharkBitches()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.TradeWithFuckingSharkBitches.OpenWoodenTrunk".Localized());

            SharkBookMenus();
        }

        public void SharkBookMenus(bool free = false)
        {

            ClearButtons();

            if (free)
            {
                AddButton(0, "Common.Button.CuntManual".Localized(), CampCuntManual);

                AddButton(1, "Common.Button.EtiquetteGuide".Localized(), EntropyGuideByStephenHawking);

                AddButton(2, "Common.Button.Porn".Localized(), StephenHawkingPorn);
            }
            else
            {
                AddButton(0, "Common.Button.CuntManual".Localized(), ReadSharkCuntManual);

                AddButton(1, "Common.Button.EtiquetteGuide".Localized(), SharkEdgingGuideLOL);

                AddButton(2, "Common.Button.Porn".Localized(), SharkgirlPronz);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => { if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) <= 0) ExecEncounter(); else IzmaFollowerMenu(); });
        }

        public void ReadSharkCuntManual()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ReadSharkCuntManual.PointAtCombatManuals".Localized());

            if (Player.Gems < 20)
            {
                OutputText("Scenes.Izmascene.ReadSharkCuntManual.CantAffordManuals".Localized());

                DoNext(() => TradeWithFuckingSharkBitches());
            }
            else
            {
                AddButton(0, "Common.Button.Yes".Localized(), ReadSharkCuntManual2);

                AddButton(1, "Common.Button.No".Localized(), TradeWithFuckingSharkBitches);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0 && CabinInterior.HasBookshelf && !Player.HasKeyItem("Izma's Book - Combat Manual"))
                AddButton(2, "Common.Button.Buy".Localized(), BuyBookCombatManual);
        }

        public void ReadSharkCuntManual2()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ReadSharkCuntManual2.HandOverGems".Localized());

            Player.Gems -= 20;

            float choice = Random.Shared.Next(3);

            if (choice == 0)
            {
                OutputText("Scenes.Izmascene.ReadSharkCuntManual2.LearnGuardingStances".Localized());

                Player.ApplyStats(new DynStat(GameStats.Toughness, 2f));
            }
            else
                if (choice == 1)
                {
                    OutputText("Scenes.Izmascene.ReadSharkCuntManual2.FinishSkimmingBook".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f));
                }
                else
                {
                    OutputText("Scenes.Izmascene.ReadSharkCuntManual2.ManualStrikeInsight".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Strength, 2f));
                }

            OutputText("Scenes.Izmascene.ReadSharkCuntManual2.ReturnFinishedManual".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) != 1)
                OutputText("Scenes.Izmascene.ReadSharkCuntManual2.HeadBackToCamp".Localized());
            else
                OutputText("Scenes.Izmascene.ReadSharkCuntManual2.LeaveHerToBooks".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BuyBookCombatManual()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.BuyBookCombatManual.AskToBuyManual".Localized());

            if (Player.Gems >= 400)
            {
                OutputText("Scenes.Izmascene.BuyBookCombatManual.ConfirmPurchase".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), ConfirmBuyCombatManual);

                AddButton(1, "Common.Button.No".Localized(), TradeWithFuckingSharkBitches);
            }
            else
            {
                OutputText("Scenes.Izmascene.BuyBookCombatManual.NotEnoughGems".Localized());

                DoNext(() => TradeWithFuckingSharkBitches());
            }
        }

        public void ConfirmBuyCombatManual()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ConfirmBuyCombatManual.PayFourHundredGems".Localized());

            OutputText("Scenes.Izmascene.ConfirmBuyCombatManual.ReceiveCombatManual".Localized());

            Player.CreateKeyItem("Izma's Book - Combat Manual", 0, 0, 0, 0);

            Player.Gems -= 400;

            DoNext(() => TradeWithFuckingSharkBitches());
        }

        public void SharkEdgingGuideLOL()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.SharkEdgingGuideLOL.PickUpEtiquetteGuide".Localized());

            if (Player.Gems < 25)
            {
                OutputText("Scenes.Izmascene.SharkEdgingGuideLOL.CantAffordGuide".Localized());

                DoNext(() => TradeWithFuckingSharkBitches());
            }
            else
            {
                AddButton(0, "Common.Button.Yes".Localized(), ReadSharkEdgingGuideLOL);

                AddButton(1, "Common.Button.No".Localized(), TradeWithFuckingSharkBitches);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0 && CabinInterior.HasBookshelf && !Player.HasKeyItem("Izma's Book - Etiquette Guide"))
                AddButton(2, "Common.Button.Buy".Localized(), BuyBookEtiquetteGuide);
        }

        public void ReadSharkEdgingGuideLOL()
        {
            ClearOutput();

            Player.Gems -= 25;

            OutputText("Scenes.Izmascene.ReadSharkEdgingGuideLOL.HandOverGuideGems".Localized());

            OutputText("Scenes.Izmascene.ReadSharkEdgingGuideLOL.PeruseEtiquetteBook".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, -2f));

            OutputText("Scenes.Izmascene.ReadSharkEdgingGuideLOL.ReturnFrillyBook".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) != 1)
                OutputText("Scenes.Izmascene.ReadSharkEdgingGuideLOL.HeadBackToCamp".Localized());
            else
                OutputText("Scenes.Izmascene.ReadSharkEdgingGuideLOL.LeaveHerToBooks".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BuyBookEtiquetteGuide()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.BuyBookEtiquetteGuide.AskToBuyGuide".Localized());

            if (Player.Gems >= 500)
            {
                OutputText("Scenes.Izmascene.BuyBookEtiquetteGuide.ConfirmPurchase".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), ConfirmBuyEtiquetteGuide);

                AddButton(1, "Common.Button.No".Localized(), TradeWithFuckingSharkBitches);
            }
            else
            {
                OutputText("Scenes.Izmascene.BuyBookEtiquetteGuide.NotEnoughGems".Localized());

                DoNext(() => TradeWithFuckingSharkBitches());
            }
        }

        public void ConfirmBuyEtiquetteGuide()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ConfirmBuyEtiquetteGuide.PayFiveHundredGems".Localized());

            OutputText("Scenes.Izmascene.ConfirmBuyEtiquetteGuide.ReceiveEtiquetteGuide".Localized());

            Player.CreateKeyItem("Izma's Book - Etiquette Guide", 0, 0, 0, 0);

            Player.Gems -= 500;

            DoNext(() => TradeWithFuckingSharkBitches());
        }

        public void SharkgirlPronz()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.SharkgirlPronz.EmbarrassedBookDiscovery".Localized());

            if (Player.Gems < 20)
            {
                OutputText("Scenes.Izmascene.SharkgirlPronz.CantAffordPorn".Localized());

                DoNext(() => TradeWithFuckingSharkBitches());
            }
            else
            {
                AddButton(0, "Common.Button.Yes".Localized(), ReadSharkgirlPornzYouFuckingPervertAsshole);

                AddButton(1, "Common.Button.No".Localized(), TradeWithFuckingSharkBitches);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0 && CabinInterior.HasBookshelf && !Player.HasKeyItem("Izma's Book - Porn"))
                AddButton(2, "Common.Button.Buy".Localized(), BuyBookPorn);
        }

        public void ReadSharkgirlPornzYouFuckingPervertAsshole()
        {
            ClearOutput();

            Player.Gems -= 20;

            OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.FlamboyantGemOffering".Localized());

            OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.AdmireIllustrations".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Lust, (float)((20 + Player.Libido / 10))));

            if (Player.Corruption < 33)
            {
                OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.HastyExitApology".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) != 1)
                    OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.ReturnToCamp".Localized());

                OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.SentencePeriod".Localized());
            }
            else
                if (Player.Corruption < 66)
                {
                    OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.SmileAndReturnBook".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) != 1)
                        OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.HeadBackToCamp".Localized());
                    else
                        OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.ReturnToCampCenter".Localized());
                }
                else
                    OutputText("Scenes.Izmascene.ReadSharkgirlPornzYouFuckingPervertAsshole.NonchalantFantasyRemark".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BuyBookPorn()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.BuyBookPorn.AskToBuyPorn".Localized());

            if (Player.Gems >= 400)
            {
                OutputText("Scenes.Izmascene.BuyBookPorn.ConfirmPurchase".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), ConfirmBuyPorn);

                AddButton(1, "Common.Button.No".Localized(), TradeWithFuckingSharkBitches);
            }
            else
            {
                OutputText("Scenes.Izmascene.BuyBookPorn.NotEnoughGems".Localized());

                DoNext(() => TradeWithFuckingSharkBitches());
            }
        }

        public void ConfirmBuyPorn()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ConfirmBuyPorn.PayFourHundredGems".Localized());

            OutputText("Scenes.Izmascene.ConfirmBuyPorn.ReceivePornBook".Localized());

            Player.CreateKeyItem("Izma's Book - Porn", 0, 0, 0, 0);

            Player.Gems -= 400;

            DoNext(() => TradeWithFuckingSharkBitches());
        }

        public void TalkToASharkCoochie()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkedAtLake) == 0)
            {
                OutputText("Scenes.Izmascene.TalkToASharkCoochie.SitBesideIzmaGossip".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                    OutputText("Scenes.Izmascene.TalkToASharkCoochie.ScratchChinInThought".Localized());
                else
                    OutputText("Scenes.Izmascene.TalkToASharkCoochie.DetailFactoryExploits".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkedAtLake, 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                    OutputText("Scenes.Izmascene.TalkToASharkCoochie.ChatAboutPinkSmoke".Localized());
                else
                {
                    OutputText("Scenes.Izmascene.TalkToASharkCoochie.FactoryShutDown".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));
                }

            OutputText("Scenes.Izmascene.TalkToASharkCoochie.FarewellBackToCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void LeaveSumSharkPussyOnTheBeach()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.LeaveSumSharkPussyOnTheBeach.NoBusinessWithIzma".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FightSharkCunt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            OutputText("Scenes.Izmascene.FightSharkCunt.DonHookedGauntlets".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) == 0)
                OutputText("Scenes.Izmascene.FightSharkCunt.EnterFightingStance".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= 2)
                    OutputText("Scenes.Izmascene.FightSharkCunt.NarrowEyesThreat".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) > 2)
                        OutputText("Scenes.Izmascene.FightSharkCunt.UncertainBeforeBattle".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) < 0 && CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -2)
                            OutputText("Scenes.Izmascene.FightSharkCunt.MockingFirstStrikeOffer".Localized());
                        else
                            OutputText("Scenes.Izmascene.FightSharkCunt.LaughAndShakeHead".Localized());

            var izmaEnemy = EnemyRegistry.Create(new Identifier("coc", "izma"));

            izmaEnemy.OnDefeated = _ => IzmaWins();

            izmaEnemy.OnWon = _ => LoseToIzma();

            CombatScene.Start(izmaEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DefeatIzma()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= 0)
            {
                OutputText("Scenes.Izmascene.DefeatIzma.FallsBackIntoSand".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Izmascene.DefeatIzma.InjuriesWord".Localized());
                else
                    OutputText("Scenes.Izmascene.DefeatIzma.LustWord".Localized());

                OutputText("Scenes.Izmascene.DefeatIzma.PreventedFromFighting".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) > 0)
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) < 3)
                    {
                        OutputText("Scenes.Izmascene.DefeatIzma.ThroughYourProwess".Localized());

                        if (CombatController.Instance!.Context.Enemy.Health < 1)
                            OutputText("Scenes.Izmascene.DefeatIzma.PhysicalWord".Localized());
                        else
                            OutputText("Scenes.Izmascene.DefeatIzma.SexualWord".Localized());

                        OutputText("Scenes.Izmascene.DefeatIzma.StandOverDefeatedShark".Localized());
                    }
                    else
                        OutputText("Scenes.Izmascene.DefeatIzma.ExaggeratedDefeatFall".Localized());

            OutputText("Scenes.Izmascene.DefeatIzma.ChooseClaimingBodyPart".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.UsePenis".Localized(), VictoryPenisIzma);
            else
                AddButton(0, "Common.Button.UsePenis".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.UseVagina".Localized(), UseVagooOnIzmaWin);
            else
                AddButton(1, "Common.Button.UseVagina".Localized(), null);

            AddButton(2, "Common.Button.UseAss".Localized(), TakeItInZeButtVictoryLikeFromIzma);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveIzmaVictoryTease);
        }

        public void IzmaWins()
        {
            ClearOutput();

            Save.SetBool("previousVictoryTeased", false);

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                OutputText("Scenes.Izmascene.IzmaWins.HorrorAtWormyCargo".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaWormsScared, 1);

                Player.Orgasm("Generic");

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= 0)
                    OutputText("Scenes.Izmascene.IzmaWins.ChuckleAndProwl".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -2)
                        OutputText("Scenes.Izmascene.IzmaWins.AskAboutHoldingBack".Localized());
                    else
                        OutputText("Scenes.Izmascene.IzmaWins.SighAndShakeHead".Localized());

                LoseToIzma();
            }
        }

        public void LoseToIzma()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -5)
            {
                FinalIzmaSubmission();

                return ;
            }

            if ((int)Player.Gender <= 2)
            {
                if (Player.Health < 1)
                {
                    OutputText("Scenes.Izmascene.LoseToIzma.MetalFistStomachBlow".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.FierceGrinGentleTone".Localized());
                }
                else
                {
                    OutputText("Scenes.Izmascene.LoseToIzma.LegsBuckledMindFog".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.WryGrinDisciplineTaunt".Localized());
                }

                OutputText("Scenes.Izmascene.LoseToIzma.LostDoubleEntendre".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.LoseToIzma.CuntWord".Localized());
                else
                    OutputText("Scenes.Izmascene.LoseToIzma.PuckerWord".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.OfYoursSuffix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -2)
                    OutputText("Scenes.Izmascene.LoseToIzma.ReluctantStripNaked".Localized());
                else
                    OutputText("Scenes.Izmascene.LoseToIzma.UndressedSurprisedShark".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreak".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.CuteLittlePrefix".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.LoseToIzma.CuntWordAgain".Localized());
                else
                    OutputText("Scenes.Izmascene.LoseToIzma.PuckerWordAgain".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.ClenchesTightPrefix".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Izmascene.LoseToIzma.FearWord".Localized());
                else
                    OutputText("Scenes.Izmascene.LoseToIzma.AnticipationWord".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.ThoughtOfHugeIntrusion".Localized());

                if (Player.Health < 1)
                    OutputText("Scenes.Izmascene.LoseToIzma.TooWeakToResist".Localized());
                else
                    OutputText("Scenes.Izmascene.LoseToIzma.FeelingVeryHorny".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.ComplyRemoveArmor".Localized(
                    Player.AssDescript()
                ));

                OutputText("Scenes.Izmascene.LoseToIzma.OohLookingGoodTease".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.HotLiquidShock".Localized());
            }

            if ((int)Player.Gender <= 1)
            {
                if (Player.AnalCapacity() < 26)
                {
                    OutputText("Scenes.Izmascene.LoseToIzma.YellPainHugeAsshole".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.TakeItEasyProclaim".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                        OutputText("Scenes.Izmascene.LoseToIzma.PushingBackDesperate".Localized());

                    Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);
                }
                else
                    if (Player.AnalCapacity() < 60)
                    {
                        OutputText("Scenes.Izmascene.LoseToIzma.FeelEveryInchAnus".Localized());

                        OutputText("Scenes.Izmascene.LoseToIzma.NiceLittleHoleAsk".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                            OutputText("Scenes.Izmascene.LoseToIzma.WonderingThatToo".Localized());

                        Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);
                    }
                    else
                    {
                        OutputText("Scenes.Izmascene.LoseToIzma.TakenBiggerImpressive".Localized());

                        OutputText("Scenes.Izmascene.LoseToIzma.HeckWhatMonsters".Localized());
                    }

                OutputText("Scenes.Izmascene.LoseToIzma.BuriedGripAssPull".Localized(
                    Player.AssDescript()
                ));

                OutputText("Scenes.Izmascene.LoseToIzma.HarderFasterPace".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Izmascene.LoseToIzma.MaleOrganThrobbing".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.SentencePeriodParagraphBreak".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.GrowlTestLuckHands".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                    OutputText("Scenes.Izmascene.LoseToIzma.SharkStyleGetUsed".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.WeaklingMoanRoar".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.GroanAsWellPrefix".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Izmascene.LoseToIzma.DisgorgingPrefix".Localized());

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Izmascene.LoseToIzma.TrickleWord".Localized());
                    else
                        if (Player.CumQ() <= 150)
                            OutputText("Scenes.Izmascene.LoseToIzma.SeveralSquirtsWord".Localized());
                        else
                            OutputText("Scenes.Izmascene.LoseToIzma.SteadyStreamWord".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.SemenOntoSandSuffix".Localized());
                }
                else
                    OutputText("Scenes.Izmascene.LoseToIzma.MusclesSpasmingPleasure".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.BellyGrowsCumWave".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.IzmaStopsPantingAnus".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.SharkPeopleSexGather".Localized());

                OutputText("Scenes.Izmascene.LoseToIzma.RemainWaitingStrength".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) - 1);
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    if (Player.VaginalCapacity() < 26)
                    {
                        OutputText("Scenes.Izmascene.LoseToIzma.YellPainHugePlaceholder".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Izmascene.LoseToIzma.WhoahExclaim".Localized());

                        if (Player.Vaginas[0].Virgin)
                            OutputText("Scenes.Izmascene.LoseToIzma.FirstTimeHuh".Localized());
                        else
                            OutputText("Scenes.Izmascene.LoseToIzma.ThatsTightExclaim".Localized());

                        OutputText("Scenes.Izmascene.LoseToIzma.DontWorryKiddo".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                            OutputText("Scenes.Izmascene.LoseToIzma.PushingBackDesperateAgain".Localized());

                        Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);
                    }
                    else
                        if (Player.VaginalCapacity() < 60)
                        {
                            OutputText("Scenes.Izmascene.LoseToIzma.FeelEveryInchPlaceholder".Localized(
                                Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.Izmascene.LoseToIzma.NiceLittleHoleCroon".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                                OutputText("Scenes.Izmascene.LoseToIzma.WonderingThatTooAgain".Localized());

                            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);
                        }
                        else
                        {
                            OutputText("Scenes.Izmascene.LoseToIzma.TakenBiggerExperienced".Localized());

                            OutputText("Scenes.Izmascene.LoseToIzma.HeckWhatMonstersAgain".Localized());
                        }

                    OutputText("Scenes.Izmascene.LoseToIzma.BuriedGripPlaceholder".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.HarderFasterPaceAgain".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.GrowlTestLuckShift".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                        OutputText("Scenes.Izmascene.LoseToIzma.SharkStyleUsedAgain".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.WeaklingMoanMutter".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.GroanOrgasmFluids".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.IzmaStopsPantingCunt".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.SharkPeopleSexLean".Localized());

                    if (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 180)
                        OutputText("Scenes.Izmascene.LoseToIzma.OakChestLeafFetch".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.RemainWaitingRecover".Localized(
                        Player.VaginaDescript(0)
                    ));

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) - 1);
                }
                else
                {
                    if (Player.Health < 1)
                    {
                        OutputText("Scenes.Izmascene.LoseToIzma.MetalFistBlowAgain".Localized());

                        OutputText("Scenes.Izmascene.LoseToIzma.FierceGrinToneAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Izmascene.LoseToIzma.LegsBuckledFogAgain".Localized());

                        OutputText("Scenes.Izmascene.LoseToIzma.WryGrinDisciplineAgain".Localized());
                    }

                    OutputText("Scenes.Izmascene.LoseToIzma.CollapseDefeatStalk".Localized());

                    if (Player.LustPercent < 40)
                        OutputText("Scenes.Izmascene.LoseToIzma.LimpCockBarelyEngorged".Localized());
                    else
                        if (Player.LustPercent < 70)
                            OutputText("Scenes.Izmascene.LoseToIzma.TurgidErectionMoistCunt".Localized());
                        else
                            OutputText("Scenes.Izmascene.LoseToIzma.ThrobbingSlaveringCunt".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.DirtyMinxTwoFingers".Localized());

                    if (Player.VaginalCapacity() < 26)
                        OutputText("Scenes.Izmascene.LoseToIzma.RoamingFingersTightSlit".Localized());
                    else
                        if (Player.VaginalCapacity() < 60)
                            OutputText("Scenes.Izmascene.LoseToIzma.GigglesMoistFolds".Localized());
                        else
                            OutputText("Scenes.Izmascene.LoseToIzma.EyesWidenCavernousGape".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakFifth".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.FingersFreeFlipOver".Localized());

                    var x = Player.BiggestCockIndex();

                    if (Player.Cocks[x].Length <= 10)
                        OutputText("Scenes.Izmascene.LoseToIzma.SuppressSnortCock".Localized());
                    else
                        if (Player.Cocks[x].Length <= 19)
                            OutputText("Scenes.Izmascene.LoseToIzma.NotBadNodApproval".Localized());
                        else
                            OutputText("Scenes.Izmascene.LoseToIzma.LowWhistleThirdLeg".Localized(
                                Player.CockDescript(x)
                            ));

                    OutputText("Scenes.Izmascene.LoseToIzma.AppraisalGrabsFeet".Localized());

                    Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

                    OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakSixth".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.ThrustingPreCumSlick".Localized(
                        Player.VaginaDescript(0)
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= -4)
                        OutputText("Scenes.Izmascene.LoseToIzma.NoLongerAshamed".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakSeventh".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.DeeperIntoSandHips".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.FinalThrustWombRoar".Localized(
                        Player.SMultiCockDesc()
                    ));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Izmascene.LoseToIzma.PronounIt".Localized());
                    else
                        OutputText("Scenes.Izmascene.LoseToIzma.PronounThey".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.StrokeFaceOwnClimax".Localized());

                    if (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 150)
                    {
                        OutputText("Scenes.Izmascene.LoseToIzma.OakChestLeafReturn".Localized());

                        OutputText("Scenes.Izmascene.LoseToIzma.AntiPregnancyHerb".Localized());
                    }
                    else
                        OutputText("Scenes.Izmascene.LoseToIzma.ParagraphBreakEighth".Localized());

                    OutputText("Scenes.Izmascene.LoseToIzma.WinkTossTooth".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) - 1);
                }

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -1)
                OutputText("Scenes.Izmascene.LoseToIzma.RealizeShudderSuppress".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -2)
                    OutputText("Scenes.Izmascene.LoseToIzma.DreamsHauntSubmit".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -3)
                        OutputText("Scenes.Izmascene.LoseToIzma.HumIdlyEnjoyVisions".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= -4)
                            OutputText("Scenes.Izmascene.LoseToIzma.VagueThoughtIzma".Localized());
                        else
                            OutputText("Scenes.Izmascene.LoseToIzma.EmbraceDreamsDesperate".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.TigersharkTooth);

            Player.SlimeFeed();

            CombatScene.CleanupAfterCombat();
        }

        public void FinalIzmaSubmission()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.CollapseDefeated".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.SniggeringAdvance".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Izmascene.FinalIzmaSubmission.SpineWord".Localized());
            else
                OutputText("Scenes.Izmascene.FinalIzmaSubmission.FlankWord".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.PrecumOnBack".Localized(
                Player.AssDescript()
            ));

            if (Player.HasCock() || Player.HasVagina())
            {
                OutputText("Scenes.Izmascene.FinalIzmaSubmission.TightEmbraceMakes".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Izmascene.FinalIzmaSubmission.EachCockStiffen".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Izmascene.FinalIzmaSubmission.AndWord".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.FinalIzmaSubmission.YourMoistenConsiderably".Localized(
                        Player.VaginaDescript(0)
                    ));
            }

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.SentenceParagraphBreakAgain".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.OrderLickNeck".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.HeldInPlace".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false, true);

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.ForcingWayIn".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.PullOutTrepidation".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.SensingSubmission".Localized());

            if (Player.HasVagina() || Player.HasCock())
            {
                OutputText("Scenes.Izmascene.FinalIzmaSubmission.SemicolonSpace".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Izmascene.FinalIzmaSubmission.JizzOnShore".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Izmascene.FinalIzmaSubmission.AndWordAgain".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.FinalIzmaSubmission.SquirtingFluids".Localized(
                        Player.VaginaDescript(0)
                    ));
            }

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.SentenceParagraphBreakThird".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.FilledWithDesire".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.StayUnmoving".Localized());

            if (Player.Race != "shark-morph")
                OutputText("Scenes.Izmascene.FinalIzmaSubmission.SayBodyChanges".Localized());

            OutputText("Scenes.Izmascene.FinalIzmaSubmission.ParagraphBreak".Localized());

            Player.SlimeFeed();

            Player.Orgasm("VaginalAnal");

            DoNext(() => IzmaBadEnd());
        }

        public void IzmaBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaBadEnd.OneYearLater".Localized());

            OutputText("Scenes.Izmascene.IzmaBadEnd.NestleClose".Localized());

            OutputText("Scenes.Izmascene.IzmaBadEnd.AriseFollow".Localized());

            if ((int)Player.Gender >= 2)
                OutputText("Scenes.Izmascene.IzmaBadEnd.FertileWomb".Localized());

            OutputText("Scenes.Izmascene.IzmaBadEnd.WonderfulPurpose".Localized());

            OutputText("Scenes.Izmascene.IzmaBadEnd.LaterDateConcerns".Localized());

            OutputText("Scenes.Izmascene.IzmaBadEnd.PauseDistracted".Localized());

            GameOver();
        }

        public void VictoryPenisIzma()
        {
            ClearOutput();

            Save.SetBool("previousVictoryTeased", false);

            int cockIndex = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (cockIndex < 0)
                cockIndex = 0;

            OutputText("Scenes.Izmascene.VictoryPenisIzma.DefeatedTigersharkWatch".Localized());

            OutputText("Scenes.Izmascene.VictoryPenisIzma.SlideIntoSlit".Localized(
                Player.CockDescript(cockIndex)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) >= 3)
                OutputText("Scenes.Izmascene.VictoryPenisIzma.MadeForYou".Localized());

            OutputText("Scenes.Izmascene.VictoryPenisIzma.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.VictoryPenisIzma.PickingUpSpeed".Localized());

            OutputText("Scenes.Izmascene.VictoryPenisIzma.SeekingLouderMoans".Localized(
                Appearance.CockNoun(Player.Cocks[cockIndex].Id)
            ));

            OutputText("Scenes.Izmascene.VictoryPenisIzma.PantingGasping".Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Izmascene.VictoryPenisIzma.LengthyFuckGrunt".Localized(
                    Player.CockDescript(cockIndex)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Izmascene.VictoryPenisIzma.OntoHerGroin".Localized());

                OutputText("Scenes.Izmascene.VictoryPenisIzma.CryOutLoudly".Localized());

                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Izmascene.VictoryPenisIzma.BellySwells".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Izmascene.VictoryPenisIzma.SwellNoMore".Localized());

                    OutputText("Scenes.Izmascene.VictoryPenisIzma.SentencePeriod".Localized());
                }

                OutputText("Scenes.Izmascene.VictoryPenisIzma.PushBackRedressing".Localized());
            }
            else
            {
                OutputText("Scenes.Izmascene.VictoryPenisIzma.LengthyFuckGruntAgain".Localized(
                    Player.CockDescript(cockIndex)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Izmascene.VictoryPenisIzma.OntoHerGroinAgain".Localized());

                OutputText("Scenes.Izmascene.VictoryPenisIzma.CryOutLoudlyAgain".Localized());

                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Izmascene.VictoryPenisIzma.BellySwellsAgain".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Izmascene.VictoryPenisIzma.SwellNoMoreAgain".Localized());

                    OutputText("Scenes.Izmascene.VictoryPenisIzma.SentencePeriodAgain".Localized());
                }

                OutputText("Scenes.Izmascene.VictoryPenisIzma.NotDoneYet".Localized());

                OutputText("Scenes.Izmascene.VictoryPenisIzma.PlantOnFace".Localized(
                    Player.VaginaDescript(0)
                ));
            }

            Player.Orgasm("Dick");

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= 4 || CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) > 0)
            {
                OutputText("Scenes.Izmascene.VictoryPenisIzma.FarewellTigershark".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.TigersharkTooth);

                CombatScene.CleanupAfterCombat();
            }
            else
                VictorzChoice();

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) + 1);
        }

        public void UseVagooOnIzmaWin()
        {
            ClearOutput();

            Save.SetBool("previousVictoryTeased", false);

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.WatchDefeatedShark".Localized());

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.MountHerCock".Localized());

            Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.GrindingAtop".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.WeightPinning".Localized());
            else
                if (Player.Tallness > 48)
                    OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.PinHandsAbove".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));
                else
                    OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.BouncedOnCock".Localized());

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.DoingHerBest".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.RewardEagerPartner".Localized());

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Izmascene.UseVagooOnIzmaWin.VigorousFuckingOrgasm".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            AddButton(0, "Common.Button.Yes".Localized(), EatIzmasLeafAfterRapinHer);

            AddButton(1, "Common.Button.No".Localized(), DontEatIzamsLeafAfterRape);
        }

        public void EatIzmasLeafAfterRapinHer()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.EatIzmasLeafAfterRapinHer.AcceptLeaf".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.TigersharkTooth);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= 4 || CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) > 0)
            {
                OutputText("Scenes.Izmascene.EatIzmasLeafAfterRapinHer.SayGoodbyes".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
                VictorzChoice();

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) + 1);
        }

        public void DontEatIzamsLeafAfterRape()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.DontEatIzamsLeafAfterRape.HaughtyGlare".Localized());

            OutputText("Scenes.Izmascene.DontEatIzamsLeafAfterRape.ApologeticCringe".Localized());

            OutputText("Scenes.Izmascene.DontEatIzamsLeafAfterRape.SlapLeafAway".Localized());

            Player.KnockUp(PregnancyIds.Izma, PregnancyIds.IzmaIncubation);

            CombatScene.CleanupAfterCombat();

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) + 1);
        }

        public void TakeItInZeButtVictoryLikeFromIzma()
        {
            ClearOutput();

            Save.SetBool("previousVictoryTeased", false);

            Player.SlimeFeed();

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.SlumpsDefeated".Localized());

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.DongFlopsFree".Localized());

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.LickHerTip".Localized());

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.SuckMoreCock".Localized());

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.StrainingSlide".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, true, false);

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.GritsTeethCum".Localized());

            Player.Orgasm("Anal");

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= 4 || CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) > 0)
            {
                OutputText("Scenes.Izmascene.TakeItInZeButtVictoryLikeFromIzma.SayGoodbyes".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.TigersharkTooth);

                CombatScene.CleanupAfterCombat();
            }
            else
                VictorzChoice();

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) + 1);
        }

        public void LeaveIzmaVictoryTease()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.LeaveIzmaVictoryTease.CollapseToSand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) <= 4 || CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) > 0)
                LeaveIzmaVictoryTease2();
            else
            {
                OutputText("Scenes.Izmascene.LeaveIzmaVictoryTease.GlovesAsPrize".Localized());

                ClearButtons();

                AddNextButton("Common.Button.Gloves".Localized(), () => ChooseIzmaGloves(false));

                AddNextButton("Common.Button.Nah".Localized(), LeaveIzmaVictoryTease2);
            }

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimesFoughtAndWon, CoC.Instance.Flags.Get(GameFlagIds.IzmaTimesFoughtAndWon) + 1);
        }

        public void LeaveIzmaVictoryTease2()
        {

            OutputText("Scenes.Izmascene.LeaveIzmaVictoryTease2.ConsideringRefusal".Localized());

            if (Save.GetBool("previousVictoryTeased"))
                OutputText("Scenes.Izmascene.LeaveIzmaVictoryTease2.SayPleaseThisTime".Localized());

            OutputText("Scenes.Izmascene.LeaveIzmaVictoryTease2.RefuseReward".Localized());

            OutputText("Scenes.Izmascene.LeaveIzmaVictoryTease2.CutHerOff".Localized());

            Save.SetBool("previousVictoryTeased", true);

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.TigersharkTooth);

            CombatScene.CleanupAfterCombat();
        }

        public void VictorzChoice()
        {

            OutputText("Scenes.Izmascene.VictorzChoice.PantingRewardAsk".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Tooth".Localized(), ChooseIzmaTooth);

            AddButton(1, "Common.Button.Gloves".Localized(), () => ChooseIzmaGloves());
        }

        public void ChooseIzmaTooth()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ChooseIzmaTooth.AcceptTooth".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, ConsumableIds.TigersharkTooth);

            CombatScene.CleanupAfterCombat();
        }

        public void ChooseIzmaGloves(bool sexed = true)
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ChooseIzmaGloves.DemandGloves".Localized());

            OutputText("Scenes.Izmascene.ChooseIzmaGloves.PointAtGauntlets".Localized((sexed ? "Common.Text.Sex".Localized() : "Common.Text.Battle".Localized())));

            OutputText("Scenes.Izmascene.ChooseIzmaGloves.RepeatDemand".Localized());

            OutputText("Scenes.Izmascene.ChooseIzmaGloves.MeeklyObeys".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, WeaponIds.HookedGauntlets);

            CoC.Instance.Flags.Set(GameFlagIds.IzmaGlovesTaken, CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void ChooseYourIzmaWeapon()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
                NonFightIzmaSmexASS();
            else
                if (Player.Gender == Gender.Male)
                    NonFightIzmaSmexPAINUS();
                else
                    if (Player.Gender == Gender.Female)
                        NonFightIzmaSmexCUNTPUSSYSNATCHQUIM();
                    else
                    {
                        OutputText("Scenes.Izmascene.ChooseYourIzmaWeapon.GenitalFocusPrompt".Localized());

                        ClearButtons();

                        AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), NonFightIzmaSmexPAINUS);

                        AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), NonFightIzmaSmexCUNTPUSSYSNATCHQUIM);
                    }
        }

        public void NonFightIzmaSmexPAINUS()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.OfferAcceptedUndress".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.MirrorUndressing".Localized());

            if (Player.CockTotal() == 1 && Player.Cocks[0].TypeIndex < 9)
            {
                if (Player.Cocks[0].Id == BodyPartTypes.Human)
                    OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.SurprisedBySimilarity".Localized());
                else
                    if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                        OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.LicksLipsAtCock".Localized());
                    else
                        if (Player.HasKnot(0))
                            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.StaresMakingUpMind".Localized());
                        else
                            if (Player.Cocks[0].Id == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.PuzzledStareReading".Localized());
                            else
                                if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                                    OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.ShockedAtNovelPecker".Localized());
                                else
                                    if (Player.Cocks[0].Id == BodyPartTypes.Cat)
                                        OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.WincesAtBarbedPecker".Localized());
                                    else
                                        if (Player.Cocks[0].Id == BodyPartTypes.Lizard)
                                            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.GogglesAtPurplePecker".Localized());
                                        else
                                            if (Player.Cocks[0].Id == BodyPartTypes.Anemone)
                                                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.GrimacesAtUnveiling".Localized());
                                            else
                                                if (Player.Cocks[0].Id == BodyPartTypes.Kangaroo)
                                                    OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.BlankStareAtSheath".Localized());
            }
            else
                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.BugsAtEachCock".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.ParagraphBreak".Localized());

            if (Player.Mf("m", "f") == "m" && Player.BiggestBreastSize() >= 2)
            {
                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.BreastsConfuseIzma".Localized());

                if (Player.BiggestLactation() >= 2)
                    OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.MilkOozingSurprise".Localized());

                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.ParagraphBreakAgain".Localized());
            }

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.AppraisalThenNegotiation".Localized());

            if (Player.Cocks[0].Id == new Identifier("coc", "tentacle"))
                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.TentacleFlexCutOff".Localized());
            else
                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.ProtestStoppedByGlare".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.HornyCannotBackOut".Localized(
                Appearance.CockNoun(Player.Cocks[0].Id)
            ));

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.SuckingHerErection".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.FondlingSwollenSack".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.MutualSimultaneousCum".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.HairSuffix".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.PeriodSpace".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.KeepsShootingStrokes".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.RollAwaySavoringTaste".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexPAINUS.WhispersThanksDressing".Localized());

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            Player.Orgasm("Dick");

            InventoryScene.TakeItem(ConsumableIds.TigersharkTooth, () => ReturnToCamp(1));
        }

        public void NonFightIzmaSmexASS()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NonFightIzmaSmexASS.NudeRevealPursedLips".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexASS.ApologyHandOnShoulder".Localized());

            if (Player.Intelligence >= 50)
            {
                OutputText("Scenes.Izmascene.NonFightIzmaSmexASS.RealizationAboutPleasure".Localized());

                OutputText("Scenes.Izmascene.NonFightIzmaSmexASS.RollsEyesOverDomination".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void NonFightIzmaSmexCUNTPUSSYSNATCHQUIM()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.GrinsUndoingBikiniTop".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.YourTurnSeductiveGaze".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.ArmorRemovedPlainly".Localized(
                Appearance.AllBreastsDescript(Player),

                Player.VaginaDescript(0)
            ));

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.AndCocksSuffix".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.NotBadCompliment".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.NodsPositioningOverFace".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.LickingTipSalivaStrands".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.DeepthroatsHerCock".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.MuffledGroanOfPleasure".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.WeakLoadAcrossChest".Localized());

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.RollsOffTastingCum".Localized());

            Player.SlimeFeed();

            OutputText("Scenes.Izmascene.NonFightIzmaSmexCUNTPUSSYSNATCHQUIM.NoTimeToContemplate".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.IzmaEncounterCounter) + 1);

            Player.Orgasm("Generic");

            InventoryScene.TakeItem(ConsumableIds.TigersharkTooth, () => ReturnToCamp(1));
        }

        public void IzmaStayAtTheLakeBitch()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaStayAtTheLakeBitch.GripsHandsWorthyMate".Localized());

            OutputText("Scenes.Izmascene.IzmaStayAtTheLakeBitch.SmallKissThenLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaFollowerStatus, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AcceptIzmaAsYourBitch()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaAmilyFreakoutStatus) == 0 && CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1)
            {
                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.WarmShoulderKiss".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.KissAftermath".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.LaughsUntilAmilySpotted".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.AmilyCaughtEating".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.TiltsHeadAtAmily".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.QuizzicalMouseReply".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.FrownAndGroundScuff".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ExplainsAmilyHistory".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.StunnedAndPleased".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ThoughtfulSharkQuestion".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.LakeAndStreamNearby".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.AmilyHissesWarning".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ApologyKeepsIzma".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.PoliteButWary".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.PointsOutRuinedVillage".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.MumblesWithCuriosity".Localized());

                OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.BedDistanceCuriosity".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaAmilyFreakoutStatus, -1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaMarbleFreakoutStatus) == 0 && false)
                {
                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.WarmShoulderKissAgain".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.KissAftermathAgain".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ChucklesAtMarbleCamp".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.MarbleBeamsAtYou".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ExplainsSparringHistory".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ForcedSmileMoment".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.SweetieWhatTheHell".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.GritsTeethDislike".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.AsksAboutCowgirl".Localized());

                    if (Player.HasPerk(PerkIds.MarblesMilk))
                        OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.MilkAddictionUnbroken".Localized());
                    else
                        OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.MilkHoldBroken".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ParagraphBreak".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.GrowlsDamnCow".Localized());

                    if (Player.HasPerk(PerkIds.MarblesMilk))
                        OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.WeakDrugAddict".Localized());
                    else
                        OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.DruggedTwistedRelationship".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.NervousChuckleAssurance".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaMarbleFreakoutStatus, -1);
                }
                else
                {
                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.WarmShoulderKissThird".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.KissAftermathThird".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.ChucklesDistantBedroll".Localized());

                    OutputText("Scenes.Izmascene.AcceptIzmaAsYourBitch.BedDistanceDecision".Localized());
                }

            CoC.Instance.Flags.Set(GameFlagIds.IzmaFollowerStatus, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void NewAmilyMeetsIzma()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.ExcitedTalkWithAmily".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.ToothyGrinApproach".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.QuizzicalMouseReply".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.ExplainsAmilyHistory".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.StunnedAndPleased".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.CountersWithLakeAccess".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.AmilyHissesAfterReturn".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.ApologyKeepsIzma".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.PoliteButWary".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.PointsOutRuinedVillage".Localized());

            OutputText("Scenes.Izmascene.NewAmilyMeetsIzma.MumblesWithCuriosity".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaAmilyFreakoutStatus, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void NewMarbleMeetsIzma()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.DeepTalkWithMarble".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.OffersHandIntroduction".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.ExplainsSparringHistory".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.ForcedSmileMoment".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.SweetieWhatTheHell".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.ShakesHeadDislike".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.AsksAboutCowgirl".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
                OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.MilkAddictionUnbroken".Localized());
            else
                OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.MilkHoldBroken".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.GrowlsDamnCow".Localized());

            if (Player.HasPerk(PerkIds.MarblesMilk))
                OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.WeakDrugAddict".Localized());
            else
                OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.DruggedTwistedRelationship".Localized());

            OutputText("Scenes.Izmascene.NewMarbleMeetsIzma.NervousChuckleAssurance".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaMarbleFreakoutStatus, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaLakeTurnedDownCampSex()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaLakeTurnedDownCampSex.SmirkOfferingRelief".Localized());

            OutputText("Scenes.Izmascene.IzmaLakeTurnedDownCampSex.GrinsUndoingSkirt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Equals".Localized(), IzmaLakeSexAsEquals);

            AddButton(1, "Common.Button.Dominate".Localized(), IzmaLakeDominate);

            AddButton(2, "Common.Button.Submit".Localized(), SubmitToLakeIzma);

            AddButton(14, "Common.Button.Back".Localized(), ExecEncounter);
        }

        public void IzmaLakeSexAsEquals()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaLakeSexAsEquals.ProposesEqualSex".Localized());

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Izmascene.IzmaLakeSexAsEquals.SadHeadShakeRefusal".Localized());

                DoNext(() => ExecEncounter());
            }
            else
                ChooseYourIzmaWeapon();
        }

        public void IzmaLakeDominate()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaLakeDominate.IzmaBlushesFiercely".Localized());

            int x = Player.CockThatFits(65);

            if (x < 0)
                x = 0;

            if (Player.HasCock() && (Player.Gender == Gender.Male || Random.Shared.Next(2) == 0))
            {
                OutputText("Scenes.Izmascene.IzmaLakeDominate.SpreadIzmaLegs".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominate.SkipForeplay".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Izmascene.IzmaLakeDominate.PickUpThePace".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominate.TendToHerPrick".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => IzmaLakeDominateContinueVanilla());

                AddButton(1, "Common.Button.No".Localized(), NoWankingForIzmaRadarSaysSo);

                return ;
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Izmascene.IzmaLakeDominate.StripForHerPrick".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.MountHerCock".Localized());

                    Player.CuntChange(30, true, true, false);

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.GrindAtopHer".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Izmascene.IzmaLakeDominate.WeightPinsHerDown".Localized());
                    else
                        if (Player.Tallness > 48)
                            OutputText("Scenes.Izmascene.IzmaLakeDominate.PinHandsOverhead".Localized());
                        else
                            OutputText("Scenes.Izmascene.IzmaLakeDominate.SheBouncesYou".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.ParagraphBreak".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.IzmaJerksUpward".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Izmascene.IzmaLakeDominate.RewardYourPartner".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.IzmaRoarsInOrgasm".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.GoodbyeKissRequest".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.HalfHeartedSlap".Localized());

                    Player.SlimeFeed();
                }
                else
                {
                    OutputText("Scenes.Izmascene.IzmaLakeDominate.RollHerOntoBack".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.LickHerCockTip".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.SuckDeeper".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.TightSlideStrain".Localized());

                    Player.ButtChange(30, true, true, false);

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.SharkCumsInside".Localized());

                    OutputText("Scenes.Izmascene.IzmaLakeDominate.DressAndSmallTalk".Localized());

                    Player.SlimeFeed();
                }

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaLakeDominateContinueVanilla(bool vanilla = true)
        {
            ClearOutput();

            int cockIndex = Player.CockThatFits(65);

            if (cockIndex < 0)
                cockIndex = 0;

            if (vanilla)
                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.RudeNotToContinue".Localized(
                    Appearance.CockNoun(Player.Cocks[cockIndex].Id)
                ));

            OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.PantingAfterRelease".Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.BlastIntoHerWomb".Localized(
                    Player.CockDescript(cockIndex)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.OntoHerGroin".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.IzmaCriesOutLoud".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.BellySwellsWithLoad".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.BellyOverflows".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.SighAndRedress".Localized());
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.BlastWombAgain".Localized(
                    Player.CockDescript(cockIndex)
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.OntoHerGroinAgain".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.IzmaCriesLoudAgain".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.BellySwellsAgain".Localized());

                if (Player.CumQ() >= 1500)
                    OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.BellyOverflowsAgain".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.NotDoneYet".Localized());

                OutputText("Scenes.Izmascene.IzmaLakeDominateContinueVanilla.PlantCockOnFace".Localized(
                    Player.VaginaDescript(0)
                ));
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void NoWankingForIzmaRadarSaysSo()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NoWankingForIzmaRadarSaysSo.SkipHerHandjob".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Akbal.AkbalQuestConclusion.ButtonStopHer".Localized(), NoWankingForIzma);

            AddButton(1, "Common.Button.Letherwank".Localized(), LetIzmaWankLikeABitch);
        }

        public void LetIzmaWankLikeABitch()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.LetIzmaWankLikeABitch.IntimateKissFirst".Localized());

            IzmaCumsAfterRadarStuffHere(false);
        }

        public void NoWankingForIzma()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NoWankingForIzma.SlapHerHandAway".Localized());

            IzmaCumsAfterRadarStuffHere(true);
        }

        public void IzmaCumsAfterRadarStuffHere(bool denied)
        {

            int cockIndex = Player.CockThatFits(65);

            if (cockIndex < 0)
                cockIndex = 0;

            if (!denied)
                OutputText("Scenes.Izmascene.IzmaCumsAfterRadarStuffHere.DoubleStimulation".Localized(
                    Appearance.CockNoun(Player.Cocks[cockIndex].Id)
                ));
            else
                OutputText("Scenes.Izmascene.IzmaCumsAfterRadarStuffHere.IzmaHowlsInFury".Localized());

            IzmaLakeDominateContinueVanilla(false);
        }

        public void SubmitToLakeIzma()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.SubmitToLakeIzma.SlowSensuousStrip".Localized());

            if (Player.HasLongTail())
                OutputText("Scenes.Izmascene.SubmitToLakeIzma.TailWavesEnticingly".Localized());

            OutputText("Scenes.Izmascene.SubmitToLakeIzma.SeductiveWiggle".Localized(
                Player.AssDescript()
            ));

            OutputText("Scenes.Izmascene.SubmitToLakeIzma.IzmaBreathCatches".Localized());

            if ((int)Player.Gender <= 1)
            {
                if (Player.AnalCapacity() < 26)
                {
                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.ShoutInPain".Localized());

                    Player.ButtChange(30, true, true, false);

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.ParagraphBreak".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.EaseUpPromise".Localized());
                }
                else
                    if (Player.AnalCapacity() < 26)
                    {
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.FeelEveryInch".Localized());

                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.NiceLittleHole".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.TakenBiggerBefore".Localized());

                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.WondersAtMonsters".Localized());
                    }

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.GripAndThrust".Localized(
                    Player.AssDescript()
                ));

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.RhythmBuildsPace".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.YourOrganThrobs".Localized());

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.SentencePeriodParagraphBreak".Localized());

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.IzmaGrowlsTaunt".Localized());

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.MoanForMeTaunt".Localized());

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.YouGroanToo".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.DisgorgingWord".Localized());

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.TrickleWord".Localized());
                    else
                        if (Player.CumQ() < 150)
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.SeveralSquirtsWord".Localized());
                        else
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.SteadyStreamWord".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.FloodIntoYourGuts".Localized());
                }
                else
                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.MusclesSpasming".Localized());

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.BellyFillsToBrim".Localized());

                OutputText("Scenes.Izmascene.SubmitToLakeIzma.IzmaStopsPanting".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    if (Player.VaginalCapacity() < 26)
                    {
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.ShoutInPainAgain".Localized(
                            Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.WhoahLine".Localized());

                        if (Player.Vaginas[0].Virgin)
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.FirstTimeHuh".Localized());
                        else
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.ThatsTight".Localized());

                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.EasyFirstThrusts".Localized());
                    }
                    else
                        if (Player.VaginalCapacity() < 60)
                        {
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.FeelEveryPart".Localized(
                                Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.NiceHoleAgain".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.TakenBiggerAgain".Localized());

                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.WondersAtMonstersAgain".Localized());
                        }

                    Player.CuntChange(30, true, true, false);

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.GripAssAndThrust".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.RhythmBuildsAgain".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.IzmaGrowlsAgain".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.MuttersThenRoars".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.YourOwnOrgasm".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.IzmaStopsAgain".Localized());

                    if (Player.Pregnancy.Incubation == 0)
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.LeafOffering".Localized());
                }
                else
                {
                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.TwoFingersTease".Localized());

                    if (Player.VaginalCapacity() < 26)
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.TightSlitRemark".Localized());
                    else
                        if (Player.VaginalCapacity() < 60)
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.GigglesAtEase".Localized());
                        else
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.WholeHandSlips".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.FlipYouOver".Localized());

                    var x = Player.BiggestCockIndex();

                    if (Player.Cocks[x].Length <= 10)
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.SuppressedSnort".Localized());
                    else
                        if (Player.Cocks[x].Length <= 19)
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.ImpressedNod".Localized());
                        else
                            OutputText("Scenes.Izmascene.SubmitToLakeIzma.LowWhistlePraise".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.GrabsFeetAndLifts".Localized());

                    Player.CuntChange(30, true, true, false);

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.ThrustsInAndOut".Localized(
                        Player.VaginaDescript(0)
                    ));

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.PushedDeepInSand".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.FinalThrustRoar".Localized(
                        Player.SMultiCockDesc()
                    ));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.ItPronoun".Localized());
                    else
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.ThemPronoun".Localized());

                    OutputText("Scenes.Izmascene.SubmitToLakeIzma.StrokedToClimax".Localized());

                    if (Player.Pregnancy.Incubation == 0)
                    {
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.ReleasesFeetToSand".Localized());

                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.SecondKissSmirk".Localized());
                    }
                    else
                        OutputText("Scenes.Izmascene.SubmitToLakeIzma.UnchasteKiss".Localized());
                }

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaFollowerMenu()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0)
            {
                if (Player.IsPregnant() && Player.Pregnancy.Incubation == 0)
                {
                    IzmaPoopsBabies();

                    Player.ClearPregnancy();

                    return ;
                }

                OutputText("Scenes.Izmascene.IzmaFollowerMenu.IzmaAnswersCall".Localized());

                switch (Player.Pregnancy.Event)
                {
                    case 2:
                    {
                    OutputText("Scenes.Izmascene.IzmaFollowerMenu.SpotMorningSickness".Localized());
                    }

                    break;

                    case 3:
                    {
                    OutputText("Scenes.Izmascene.IzmaFollowerMenu.AsksIfGainedWeight".Localized());
                    }

                    break;

                    case 4:
                    {
                    OutputText("Scenes.Izmascene.IzmaFollowerMenu.PregnancyConfirmed".Localized());
                    }

                    break;

                    case 5:
                    {
                    OutputText("Scenes.Izmascene.IzmaFollowerMenu.OutgrownHerClothes".Localized());
                    }

                    break;

                    case 6:
                    {
                    OutputText("Scenes.Izmascene.IzmaFollowerMenu.BabyComingSoon".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                        OutputText("Scenes.Izmascene.IzmaFollowerMenu.SkirtCannotCope".Localized());
                    else
                        OutputText("Scenes.Izmascene.IzmaFollowerMenu.JuicesStaining".Localized());

                    OutputText("Scenes.Izmascene.IzmaFollowerMenu.SwollenBellyUnderside".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                        OutputText("Scenes.Izmascene.IzmaFollowerMenu.CockFrictionOnBulge".Localized());
                    else
                        OutputText("Scenes.Izmascene.IzmaFollowerMenu.ThighFrictionMoving".Localized());
                    }

                    break;
                }
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaFollowerMenu.PutsAwayHerBook".Localized());

                OutputText("Scenes.Izmascene.IzmaFollowerMenu.OffersToHelp".Localized());
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySharkgirls);
// 原版 IzmaScene.hx:49：Akky 若在 Izma 处则追加它的位置描述
            SceneInstance<AkkyScene>.Get().LocationDesc("Izma");

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), IzmaPearance);

            AddButton(1, "Common.Button.Talk".Localized(), IzmaTalkMenu);

            if (Player.Lust >= 33)
                AddButton(2, "Common.Button.Sex".Localized(), IzmaSexMenu);
            else
                AddButton(2, "Common.Button.Sex".Localized(), null);

            AddButton(3, "Common.Button.Spar".Localized(), IzmaSpar);

            if (TotalIzmaChildren() > 0)
                AddButton(5, "Common.Button.Children".Localized(), IzmaKidsMenu);

            AddRowButton(1, "Tooth", GatASharkTooth);

            AddRowButton(1, "Books", IzmaCampBooks);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                AddRowButton(1, "Remove Dick", () => RemoveIzmasPenis());
            else
                AddRowButton(1, "Go Herm", IzmaDickPrompt);

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Izma")
                AddButton(9, "Common.Button.SleepAlone".Localized(), NoCuddle);
            else
                AddButton(9, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), IzmaCuddle);

            if (CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0 && !Player.IsPregnant())
                    AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);

                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) != 0)
                    AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);
            }

            if (Player.UpperGarment.Id == Undergarments.SEA_SHIRT.Id && Save.GetInt("izmaAsked", 0) == 0)
                AddRowButton(2, "Cursed Shirt", AskIzmaAboutNobleShirt);

            if (Player.HasKeyItem("Old Manor Books"))
                AddRowButton(2, "Give Books", GiveManorBooks);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 1)
                AddButton(14, "Common.Button.Back".Localized(), () => IzmaFollowerMenu());
        }

        public void GiveManorBooks()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.GiveManorBooks.BetaBookCollection".Localized());

            OutputText("Scenes.Izmascene.GiveManorBooks.EyesOnRisqueItems".Localized());

            OutputText("Scenes.Izmascene.GiveManorBooks.ConfusedThenExplained".Localized());

            OutputText("Scenes.Izmascene.GiveManorBooks.InsistentHipWiggle".Localized());

            Save.SetBool("gaveBooks", true);

            Player.RemoveKeyItem("Old Manor Books");

            ClearButtons();

            AddNextButton("Common.Button.Accept".Localized(), IzmaSexMenu);

            AddNextButton("Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void SendToFarm()
        {
            ClearOutput();

            IzmaSprite();

            OutputText("Scenes.Izmascene.SendToFarm.SendToLakeFarm".Localized());

            OutputText("Scenes.Izmascene.SendToFarm.AsksIfDoneWrong".Localized());

            OutputText("Scenes.Izmascene.SendToFarm.ReassuresHerTrust".Localized());

            OutputText("Scenes.Izmascene.SendToFarm.UnusedToLabor".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmIzma, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BackToCamp()
        {
            ClearOutput();

            IzmaSprite();

            OutputText("Scenes.Izmascene.BackToCamp.HeadBackToCamp".Localized());

            OutputText("Scenes.Izmascene.BackToCamp.GrinsWrinklesNose".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmIzma, 0);
        }

        public void GatASharkTooth()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTigersharkToothCountdown) > 1)
            {
                OutputText("Scenes.Izmascene.GatASharkTooth.ApologeticNoTooth".Localized());

                DoNext(() => IzmaFollowerMenu());
            }
            else
            {
                OutputText("Scenes.Izmascene.GatASharkTooth.PullsToothFromChest".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaTigersharkToothCountdown, CoC.Instance.Flags.Get(GameFlagIds.IzmaTigersharkToothCountdown) + 1);

                InventoryScene.TakeItem(ConsumableIds.TigersharkTooth, IzmaFollowerMenu);
            }
        }

        public void IzmaPearance()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaPearance.TallTigersharkBuild".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) == 0)
                OutputText("Scenes.Izmascene.IzmaPearance.IronGauntletWeapon".Localized());
            else
                OutputText("Scenes.Izmascene.IzmaPearance.SentencePeriod".Localized());

            OutputText("Scenes.Izmascene.IzmaPearance.AngularSharkFace".Localized());

            OutputText("Scenes.Izmascene.IzmaPearance.DdCupBreasts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Izmascene.IzmaPearance.LongHumanoidCock".Localized());

                OutputText("Scenes.Izmascene.IzmaPearance.ReadyFuckHole".Localized());
            }
            else
                OutputText("Scenes.Izmascene.IzmaPearance.WetDribblingPussy".Localized());

            OutputText("Scenes.Izmascene.IzmaPearance.TightTonedAsshole".Localized());

            DoNext(() => IzmaFollowerMenu());
        }

        public void IzmaSexMenu()
        {

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                AddNextButton("Common.Button.AnalCatch".Localized(), FollowerIzmaTakesItInPooper);

                AddNextButton("Common.Button.Domwithass".Localized(), RadarIzmaAnalDominant);
            }

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(65))
                    AddButton(2, "Common.Button.DomVagina".Localized(), FuckIzmasPussyDominate);

                AddNextButton("Common.Button.GetMounted".Localized(), () => FollowerIzmaMountsPC());

                AddNextButton("Common.Button.IzmasVagina".Localized(), FollowerIzmaTakesItInVagoo);

                AddNextButton("Common.Button.Anal".Localized(), IzmAnal);
            }

            if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                AddNextButton("Common.Button.Rideizmacock".Localized(), InCampRideIzmasDickDongTheWitchIsDead);

            if ((int)Player.Gender > 0)
                AddNextButton("Common.Button.Sixtynine".Localized(), FollowerIzmaTakesIt69);

            AddNextButton("Common.Button.Cunnilingus".Localized(), IzmaCunnilingus);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesIzmaDommedLatexy) > 0 && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 && SceneInstance<LatexGirlScene>.Get().LatexGooAtCamp())
                AddNextButton(SceneInstance<LatexGirlScene>.Get().GooName(), IzmaLatexySubmenu);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 && SceneInstance<ValeriaScene>.Get().ValeriaFollower() && SceneInstance<ValeriaScene>.Get().ValeriaFluidsEnabled())
                AddNextButton("Common.Button.Valeria".Localized(), IzmaValeriaSubmenu);

            AddButton(14, "Common.Button.Back".Localized(), IzmaFollowerMenu);
        }

        public void FollowerIzmaMountsPC(bool lastHalf = false)
        {
            ClearOutput();

            int x = Player.CockThatFits(65);

            if (x < 0)
                x = 0;

            if (!lastHalf)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                    Player.SlimeFeed();

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.RemovesArmorLiesBack".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.InsteadOfHerCock".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.LicksLipsStraddles".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.YieldsToMinistrations".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.FullyInsideSheGrowls".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.FeelsGoodSettlesDown".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.PreSoaksIntoSkin".Localized());
                else
                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.LubricantSoaksSkin".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.WondersAboutMovingHips".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                {
                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.CockSlapsBelly".Localized());

                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.CouldJerkHerOff".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.JerkIt".Localized(), () => FollowerIzmaMountsPC(true));

                    AddButton(1, "Common.Button.Nope".Localized(), RadarIzmaLeaveHerWangUnWingWanged);

                    return ;
                }
                else
                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.CupsBouncingCleavage".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.HandToCock".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.MoansRubsShaft".Localized());
            }
            else
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.MoansRubsNipples".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.RopesAcrossFace".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.GirlCumSoaksBelly".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.ImpromptuCumBath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.OwnWord".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.TeasedTormentedCock".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.YourCunt".Localized());

                if (Player.Vaginas[0].Wetness <= 3)
                    OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.DrippingWord".Localized());
                else
                    if (Player.Vaginas[0].Wetness <= 4)
                        OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.GushingWord".Localized());
                    else
                        OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.CascadingWord".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.FluidsOntoGround".Localized());
            }

            OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.EagerToRelease".Localized());

            IzmaPreg();

            if (Player.CumQ() < 500)
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.WombFillsWithCum".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.SquealsBloatedBelly".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.RollsOffBreathless".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaMountsPC.GaspsForBreath".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaPreg()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyEnabled) != 0 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0 && Player.KnockUpChance(Random.Shared.Next(20, 61)))
                Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.IzmaIncubation);
        }

        public void RadarIzmaLeaveHerWangUnWingWanged()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.DeclinesHandjob".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.CuppedCockheadCum".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.OwnCockTeased".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.YourCunt".Localized());

                if (Player.Vaginas[0].Wetness <= 3)
                    OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.DrippingWord".Localized());
                else
                    if (Player.Vaginas[0].Wetness <= 4)
                        OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.GushingWord".Localized());
                    else
                        OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.CascadingWord".Localized());

                OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.FluidsOntoGround".Localized());
            }

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.EagerToRelease".Localized());

            IzmaPreg();

            if (Player.CumQ() < 500)
                OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.WombFillsWithCum".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.SquealsBloatedBelly".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.RollsOffBreathless".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerWangUnWingWanged.GaspsForBreath".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FollowerIzmaTakesItInPooper()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.IzmaAlphaTurnDeclaration".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.WristsGrabbedToReply".Localized());

            if (Player.Tallness > 72 || Player.IsTaur())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.PullDownToKiss".Localized());
            else
                if (Player.Tallness < 54)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.BearHugLiftUp".Localized());
                else
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.PullIntoBodyHug".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SharkTeethGrazeLips".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.KissHerBackSubmissively".Localized());

            if (Player.IsBiped() || Player.IsTaur())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.BetweenYourLegs".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AgainstLowerBody".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.GiveUpFightFragment".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.IzmaPleasedMoan".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.GetItSlipperyTaunt".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.KissAlongHerLength".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.CockSlidesPastLips".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ThroatPullOutSplutter".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ExpectPenetrationInsteadSlide".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AndSackFragment".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.CockLinedUpFragment".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.PressesCocksTogether".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AgainstOwnBalls".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AgainstDrippingCock".Localized(
                            Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AgainstPerineum".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.HumiliationOverStimulation".Localized());

                if (Player.Cocks[0].Length < 14)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.GlansPastTipSigh".Localized());
                else
                    if (Player.Cocks[0].Length < 16)
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SimilarSizedCocksMurmur".Localized());
                    else
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.YourCockLongerRemark".Localized(
                            Appearance.CockNoun(Player.Cocks[0].Id)
                        ));

                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ParagraphBreak".Localized());
            }

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.HipsAwayAssLift".Localized());

            if (Player.Anal.Looseness == 0)
            {
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.PainfulPushGasp".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.BodyStrokedGently".Localized());
            }
            else
                if (Player.AnalCapacity() <= 26)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.CockheadPushesInMoan".Localized());
                else
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SlamsHipsFirstThrust".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.HipsPressedWristsPinned".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.BehindKneesImmobilize".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AroundLegsImmobilize".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.AssInAirFragment".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.DeliberateScrewing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.WristsPinnedReachFail".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ParagraphBreakThird".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ThrustsFasterWilder".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.BiteNippleYell".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ThrobbingCockOrgasm".Localized());
            else
            {
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.CollapsesSpentEmbrace".Localized(
                    Appearance.CockNoun(Player.Cocks[0].Id)
                ));

                if (Player.Cocks[0].Length < 14)
                {
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.IzmaGrabsCock".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SecondCockSynchronized".Localized(
                            Player.CockDescript(1)
                        ));

                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.TwoHandedJerkOff".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.RepeatedUntilSticky".Localized());
                    else
                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.CumPoursToGround".Localized());
                        else
                            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SentencePeriod".Localized());

                    OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ParagraphBreakFourth".Localized());
                }
                else
                    if (Player.Cocks[0].Length < 16)
                    {
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.BetweenBreastsSensation".Localized());

                        if (Player.CumQ() < 300)
                            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SpatteredButPleased".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.WetButImpressed".Localized());
                            else
                                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.CumShoweredBreasts".Localized());

                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SentenceParagraphBreak".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ReliefSighBecomesMoan".Localized(
                            Appearance.CockNoun(Player.Cocks[0].Id)
                        ));

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.OntoCombinedBodies".Localized());

                        OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SentenceParagraphBreakAgain".Localized());

                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.SwallowsUntilStopped".Localized());
                        else
                            if (Player.CumQ() < 3000)
                                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.GivesUpSwallowing".Localized());
                            else
                                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.HugeLoadSplutter".Localized());
                    }
            }

            if (!Player.HasCock())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInPooper.ShrinkingCockKiss".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FollowerIzmaTakesItInVagoo()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                Player.SlimeFeed();

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.AdmiringTigersharkCrotch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.LiftingMeatyQuads".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.PartingMuscledThighs".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.BetterSeeDampLips".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.LecherousGrinStalk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.HoldingMalenessAside".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.SpreadingVulvaFolds".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.ShedArmorPose".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.SaunterAndKneel".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.WrithingMoanWhine".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.ReachesAroundJerkOff".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.ReachesUpMassageBreasts".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.WondersAboutDominance".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.SentencePeriod".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetHer".Localized(), RadarIzmaXPackLetHer);

                AddButton(1, "Common.Button.DenyHer".Localized(), RadarIzmaXpackDenyHer);

                return ;
            }

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.HandsOnAllowed".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.JerksFaster".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.SentenceParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.AcceleratesInResponse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.OwnWord".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.ThrustsMatchPace".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.MoaningMashingHips".Localized(
                Appearance.CockNoun(Player.Cocks[0].Id)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.PreDribbleWarning".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.GirlishGooWarning".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.ShudderThroughIzma".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.HotSpunkJetMouth".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.FemCumSplatter".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.VaginaClampsMilking".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 1)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.GirlPrefix".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.MilkingFeelersOverload".Localized());

            if (Player.CumQ() < 750)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.WombFloodTrickle".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.BellyBloatingVolume".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.RollApartPanting".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.LicksSeedOffFace".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.AffectionateFaceLick".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.GigglesLicksMess".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesItInVagoo.GigglesLicksLips".Localized());

            IzmaPreg();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RadarIzmaXpackDenyHer()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.RadarIzmaXpackDenyHer.SeizesHerToDeny".Localized());

            WheresItGoing(true);
        }

        public void RadarIzmaXPackLetHer()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.RadarIzmaXPackLetHer.CaressesBreastsNipples".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaXPackLetHer.SpeedingUpThrusts".Localized());

            WheresItGoing();
        }

        public void WheresItGoing(bool denied = false)
        {

            OutputText("Scenes.Izmascene.WheresItGoing.MoansMashingHips".Localized(
                Appearance.CockNoun(Player.Cocks[0].Id)
            ));

            if (!denied)
                OutputText("Scenes.Izmascene.WheresItGoing.JerksOffFragment".Localized());
            else
                OutputText("Scenes.Izmascene.WheresItGoing.MoansFiercelyFragment".Localized());

            OutputText("Scenes.Izmascene.WheresItGoing.PreDribbleLimit".Localized());

            OutputText("Scenes.Izmascene.WheresItGoing.WhereWillOrgasmGo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.YourFace".Localized(), FacialWhereItGoesRadarIzmaXpack);

            AddButton(1, "Common.Button.YourChest".Localized(), RadarIzmaCumInYourChest);

            AddButton(2, "Common.Button.HerFace".Localized(), IzmaSelfFacialWheeRadar);
        }

        public void FacialWhereItGoesRadarIzmaXpack()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.ShudderThroughIzma".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.HotSpunkJetMouth".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.VaginaClampsMilking".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.MilkingFeelersOverload".Localized());

            if (Player.CumQ() < 750)
                OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.WombFloodTrickle".Localized());
            else
                OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.BellyBloatingVolume".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.RollApartPanting".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.LicksSeedOffFace".Localized());

            OutputText("Scenes.Izmascene.FacialWhereItGoesRadarIzmaXpack.GigglesLicksMess".Localized());

            IzmaPreg();

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void RadarIzmaCumInYourChest()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.AimCockAtChest".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.BetweenChestAndCoats".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.AgainstYourChest".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.CoatingIt".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.LoverSpunkAroma".Localized());

            if (Player.CumQ() < 750)
                OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.SteamyCumLubrication".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.SpermFloodsLoveCanal".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.LegsWrapAtOrgasm".Localized());

            if (Player.CumQ() < 750)
                OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.SlumpPantingAgainstHer".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.StirJizzInWomb".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.RollApartPanting".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaCumInYourChest.CleanUpAppreciation".Localized());

            IzmaPreg();

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaSelfFacialWheeRadar()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaSelfFacialWheeRadar.RedirectOrgasmAway".Localized());

            OutputText("Scenes.Izmascene.IzmaSelfFacialWheeRadar.YourOwnSeed".Localized());

            if (Player.CumQ() < 750)
                OutputText("Scenes.Izmascene.IzmaSelfFacialWheeRadar.SteamyCumCryOut".Localized());
            else
                OutputText("Scenes.Izmascene.IzmaSelfFacialWheeRadar.LoveCanalSpermFlood".Localized());

            OutputText("Scenes.Izmascene.IzmaSelfFacialWheeRadar.RollApartBreathless".Localized());

            OutputText("Scenes.Izmascene.IzmaSelfFacialWheeRadar.AfterCleanupThanks".Localized());

            IzmaPreg();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FollowerIzmaTakesIt69()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.HipsSwingTease".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.TongueWrapsLikeJewelry".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.TwoFingersInsideYou".Localized(
                        Player.VaginaDescript(0)
                    ));
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.LickClitAttention".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SentencePeriodParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SuckIzmaErection".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SwallowFemJuices".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.FondleSwollenSack".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.CaressOrangeSkin".Localized());

            if (Player.HasCock())
                OutputText(Player.CockDescript(0));
            else
                OutputText(Player.VaginaDescript());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SalivaHipThrust".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.ReachWord".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.PushClitWord".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.DeepIntoHerMouth".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SimultaneousOrgasm".Localized());
            else
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SlipperyCumOrgasm".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.LoadsIntoCheeks".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.HairWord".Localized());

                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SentencePeriod".Localized());

                if (Player.CumQ() >= 750)
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.StrokeAfterStroke".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.DroolStickyMess".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.RollAwayTaste".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 1)
                OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.LadyWord".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.SpunkInStomach".Localized());

            OutputText("Scenes.Izmascene.FollowerIzmaTakesIt69.IzmaWhispersFarewell".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmAnal()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmAnal.BendOverForSex".Localized());

            OutputText("Scenes.Izmascene.IzmAnal.BlushingIzmaAsks".Localized());

            OutputText("Scenes.Izmascene.IzmAnal.HandOnHerBottom".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyEnabled) < 1 ? "Common.Text.WithTheHerbsITakeMaking".Localized() : "Common.Text.AndIWontBeAbleTo".Localized())));

            OutputText("Scenes.Izmascene.IzmAnal.DenyingAlphaBold".Localized());

            OutputText("Scenes.Izmascene.IzmAnal.LiftsSkirtToPresent".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Lick".Localized(), IzmAnalLick);

            AddNextButton("Common.Button.Fuck".Localized(), IzmAnalFuck);
        }

        public void IzmAnalLick()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmAnalLick.SubmissiveBetaCare".Localized());

            OutputText("Scenes.Izmascene.IzmAnalLick.PurrsFromOralTongue".Localized((!Save.GetBool("tonguedButt", false) ? ", t-that's some unfamiliar treatment" : ""), (Player.HasCock() ? "Common.Text.CockTwitches".Localized() : "Common.Text.ClitShivers".Localized())));

            OutputText("Scenes.Izmascene.IzmAnalLick.BreathingGrowsRelaxed".Localized());

            Save.SetBool("tonguedButt", true);

            DoNext(() => IzmAnalFuck());
        }

        public void IzmAnalFuck()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmAnalFuck.ArmorOffGenitalsBare".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.SpreadsCheeksReady".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.PressSinkingInside".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.PullBackWithShudders".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.CravesAggression".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.VigorousThrusts".Localized((Player.HasCock() ? "Izma's weighty cock swings up and down, spraying ropes of cum in multiple small spurts, each in a sporadic direction. " : "")));

            OutputText("Scenes.Izmascene.IzmAnalFuck.ScreamsForCum".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.PointOfNoReturn".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.CockToHerMouth".Localized());

            OutputText("Scenes.Izmascene.IzmAnalFuck.SatisfiedCampBreather".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaTalkMenu()
        {

            ClearButtons();

            AddNextButton("Common.Button.JustChat".Localized(), TalkWivIzma);

            AddNextButton(CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyEnabled) == 1 ? "Common.Text.Nokidsplease".Localized() : "Common.Text.HaveKids".Localized(), ChildToggle);

            AddButton(14, "Common.Button.Back".Localized(), () => IzmaFollowerMenu());
        }

        public void TalkWivIzma()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) == 0)
            {
                OutputText("Scenes.Izmascene.TalkWivIzma.AskTellAboutHerself".Localized());

                OutputText("Scenes.Izmascene.TalkWivIzma.LakeWatersEffect".Localized());

                OutputText("Scenes.Izmascene.TalkWivIzma.TeenDemonInvasion".Localized());

                OutputText("Scenes.Izmascene.TalkWivIzma.HandOnShoulderSigh".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkLevel, CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) + 1);

                DoNext(() => ReturnToCamp(1));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) == 1)
                {
                    OutputText("Scenes.Izmascene.TalkWivIzma.InviteToTalk".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.ChooseTopicQuestion".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.RecallsLakePast".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.PlantLikeCreatures".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.OtherLakeCreatures".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.AskPersonalEncounters".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.AnemoneEncounter".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.ArousingMentalImage".Localized());

                    OutputText("Scenes.Izmascene.TalkWivIzma.ThankAndExcuseYourself".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkLevel, CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) + 1);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) == 2)
                    {
                        OutputText("Scenes.Izmascene.TalkWivIzma.IntimidatingTeethArray".Localized());

                        OutputText("Scenes.Izmascene.TalkWivIzma.ScoldsSelfForShock".Localized());

                        OutputText("Scenes.Izmascene.TalkWivIzma.PrefersUpstreamFood".Localized());

                        OutputText("Scenes.Izmascene.TalkWivIzma.NotPickyInstinct".Localized());

                        OutputText("Scenes.Izmascene.TalkWivIzma.DependsWhatYouMean".Localized());

                        OutputText("Scenes.Izmascene.TalkWivIzma.MisreadsYourExpression".Localized());

                        OutputText("Scenes.Izmascene.TalkWivIzma.MeatTasteJoke".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkLevel, CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) + 1);

                        DoNext(() => ReturnToCamp(1));
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) == 3)
                        {
                            OutputText("Scenes.Izmascene.TalkWivIzma.PersonalQuestionAsk".Localized());

                            OutputText("Scenes.Izmascene.TalkWivIzma.ParentsStillAround".Localized());

                            OutputText("Scenes.Izmascene.TalkWivIzma.ThanksForConfiding".Localized());

                            OutputText("Scenes.Izmascene.TalkWivIzma.BirthDependsWink".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 1)
                                OutputText("Scenes.Izmascene.TalkWivIzma.FormerlyHadWord".Localized());
                            else
                                OutputText("Scenes.Izmascene.TalkWivIzma.NowHasWord".Localized());

                            OutputText("Scenes.Izmascene.TalkWivIzma.FunctionalMaleGenitalia".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                                OutputText("Scenes.Izmascene.TalkWivIzma.FormerHermSelfView".Localized());
                            else
                                OutputText("Scenes.Izmascene.TalkWivIzma.HermaphroditeSelfView".Localized());

                            OutputText("Scenes.Izmascene.TalkWivIzma.FlirtOrLeaveChoice".Localized());

                            CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkLevel, CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) + 1);

                            ClearButtons();

                            AddButton(0, "Common.Button.Flirt".Localized(), ChooseToFlirtWithIzma);

                            AddButton(14, "Common.Button.Leave".Localized(), ChooseNotToFlirtWithIzma);
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) == 4)
                            {
                                OutputText("Scenes.Izmascene.TalkWivIzma.AnotherBookNearby".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.AsksAboutStaring".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.ChinScratchMemory".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.BooksCalmHerLust".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.BlushesAndHugsClose".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                                    OutputText("Scenes.Izmascene.TalkWivIzma.CockStiffensOnThigh".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.ReleaseToAvoidArousal".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkLevel, CoC.Instance.Flags.Get(GameFlagIds.IzmaTalkLevel) + 1);

                                DoNext(() => ReturnToCamp(1));
                            }
                            else
                            {
                                OutputText("Scenes.Izmascene.TalkWivIzma.NeverSeenHerLike".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.TigerHermsExplained".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.RubsTemplesToRecall".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.WasIzmaOnceMale".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.GiggleAndNoseSwat".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.ChemicalNatureUnsure".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.WhyHermsSoRare".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.MalesChainedUp".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.DisbeliefVoiceCracks".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.ChainingMaleChildren".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.BreedingInheritanceAsk".Localized());

                                OutputText("Scenes.Izmascene.TalkWivIzma.FarewellByCorruption".Localized());

                                CoC.Instance.Flags.Set(GameFlagIds.IzmaTalkLevel, 0);

                                Save.SetBool("sharkManTalk", true);

                                DoNext(() => ReturnToCamp(1));
                            }
        }

        public void ChooseNotToFlirtWithIzma()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ChooseNotToFlirtWithIzma.ThanksForMeandering".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ChooseToFlirtWithIzma()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.PregnantBeautyPraise".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.CockOverPregnantBelly".Localized());

            OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.MilkFilledWaddle".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.CockOutOfSkirt".Localized());
            else
                OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.RockHardNipples".Localized());

            OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.IzmaVividImageRemark".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.AsksHerMentalImage".Localized());

                if ((int)Player.Gender <= 1)
                {
                    OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.IzmaWeirdConcern".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
                else
                    OutputText("Scenes.Izmascene.ChooseToFlirtWithIzma.PurrsAndAdvances".Localized());
            }

            IzmaSexMenu();
        }

        public void ChildToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyDiscussed) == 0)
            {
                OutputText("Scenes.Izmascene.ChildToggle.AskIzmaAboutChildren".Localized());

                OutputText("Scenes.Izmascene.ChildToggle.IzmaSheepishThenDetermined".Localized());

                OutputText("Scenes.Izmascene.ChildToggle.TellHerStopContraceptives".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), ChildToggle);

                AddButton(1, "Common.Button.No".Localized(), FirstTimeDeclineIzmasKdiddlezlijfhdjkfhslkfbsjkhfbs);

                CoC.Instance.Flags.Set(GameFlagIds.IzmaPregnancyDiscussed, CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyDiscussed) + 1);

                return ;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyEnabled) == 1)
                {
                    OutputText("Scenes.Izmascene.ChildToggle.PutChildrenOnHold".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaPregnancyEnabled, 0);
                }
                else
                {
                    CoC.Instance.Flags.Set(GameFlagIds.IzmaPregnancyEnabled, 1);

                    if (TotalIzmaChildren() > 0)
                        OutputText("Scenes.Izmascene.ChildToggle.ResumeTryingForChildren".Localized());
                    else
                    {
                        OutputText("Scenes.Izmascene.ChildToggle.KnowingLookAboutChildren".Localized());

                        OutputText("Scenes.Izmascene.ChildToggle.DumbfoundedThenGrinning".Localized());
                    }
                }

            DoNext(() => IzmaFollowerMenu());
        }

        public void FirstTimeDeclineIzmasKdiddlezlijfhdjkfhslkfbsjkhfbs()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.FirstTimeDeclineIzmasKdiddlezlijfhdjkfhslkfbsjkhfbs.RefuseParenthood".Localized());

            DoNext(() => IzmaFollowerMenu());
        }

        public void IzmaSpar()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaSpar.ProposeSparring".Localized());

            if (!Save.GetBool("sparred"))
            {
                OutputText("Scenes.Izmascene.IzmaSpar.QuizzicalAlphaResponse".Localized());

                OutputText("Scenes.Izmascene.IzmaSpar.AssureFriendlyBout".Localized());

                OutputText("Scenes.Izmascene.IzmaSpar.AgreesButWorried".Localized());

                OutputText("Scenes.Izmascene.IzmaSpar.BeckonHerForward".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) != 0 ? "Common.Text.APairOfWornDownDented".Localized() : "Common.Text.HerFamiliarPairOf".Localized())));

                OutputText("Scenes.Izmascene.IzmaSpar.CombatStanceHunger".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaGlovesTaken) != 0 ? "[say:Since I gave you mine, Alpha,] she says, [say:these will have to do.] " : "")));

                Save.SetBool("sparred", true);
            }
            else
                OutputText("Scenes.Izmascene.IzmaSpar.GrinShowingTeeth".Localized());

            CurrentMonster = EnemyRegistry.Create(FollowerIds.IzmaLover);

            CurrentMonster!.OnDefeated = _ => IzmaWins();

            CurrentMonster.OnWon = _ => LoseToIzma();

            CombatScene.Start(CurrentMonster, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void WinSpar()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.WinSpar.IzmaFallsOver".Localized((CurrentMonster!.Health < 1 ? "Common.Text.WornOut".Localized() : "Common.Text.TurnedOn".Localized())));

            OutputText("Scenes.Izmascene.WinSpar.BeggingAlphaNeed".Localized());

            OutputText("Scenes.Izmascene.WinSpar.UnfinishedPlea".Localized());

            CombatScene.CleanupAfterCombat();

            IzmaSexMenu();

            SetExitButton("Common.Button.Back".Localized());
        }

        public void LoseSpar(bool hpvictory)
        {
            ClearOutput();

            if (hpvictory)
            {
                OutputText("Scenes.Izmascene.LoseSpar.StumbleBackwards".Localized());

                OutputText("Scenes.Izmascene.LoseSpar.AsksIfTired".Localized());

                OutputText("Scenes.Izmascene.LoseSpar.ConcernedForHealth".Localized());

                CombatScene.CleanupAfterCombat();
            }
            else
            {
                OutputText("Scenes.Izmascene.LoseSpar.FaceHotWithPassion".Localized());

                OutputText("Scenes.Izmascene.LoseSpar.ApologizesForTempting".Localized());

                OutputText("Scenes.Izmascene.LoseSpar.SoundsAgreeable".Localized());

                CombatScene.CleanupAfterCombat();

                IzmaSexMenu();

                HideButton(14);
            }
        }

        public void IzmaPoopsBabies()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.HearSplashingCursing".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.BraveLaugh".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.StandBehindToComfort".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.AsksForFondling".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.IzmaPoopsBabies.JerkingOffOrPhrase".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.PlayingWithCuntHelps".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.NodAndReachAround".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.IzmaPoopsBabies.DivvyRagingErection".Localized());
            else
                OutputText("Scenes.Izmascene.IzmaPoopsBabies.DivvyDilatingCunt".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.LongStrugglingWait".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.ScreamsSkywards".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.GraspBabyHand".Localized());

            if (Random.Shared.Next(100) <= 59)
            {
                OutputText("Scenes.Izmascene.IzmaPoopsBabies.SharkGirlWord".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaChildrenSharkgirls, CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + 1);
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaPoopsBabies.TigersharkWord".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaChildrenTigersharks, CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + 1);
            }

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.NuzzlesAgainstMother".Localized());

            OutputText("Scenes.Izmascene.IzmaPoopsBabies.HelpIzmaAshore".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PcPopsOutASharkTot()
        {

            OutputText("Scenes.Izmascene.PcPopsOutASharkTot.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) != 1)
            {
                if (Player.Vaginas.Count == 0)
                {
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.TerriblePressurePain".Localized());

                    Player.CreateVagina();
                }

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.CrumpleFromPains".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.FirstLaborPain".Localized());

                Player.CuntChange(100, true, true, false);

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.FinallySheIsOut".Localized());

                if (Random.Shared.Next(100) <= 59)
                {
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.SharkGirlWord".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaKidsInWild, CoC.Instance.Flags.Get(GameFlagIds.IzmaKidsInWild) + 1);
                }
                else
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.TigersharkWord".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.GrowingWhileFeeding".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.ScaredEyesUnderstanding".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.QuietWishForChild".Localized());
            }
            else
            {
                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.WakesToStrongPains".Localized());

                if (Player.Vaginas.Count == 0)
                {
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.RenewedRendingPressure".Localized());

                    Player.CreateVagina();
                }

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.MovementUnderSkin".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.FreshLaborPainCry".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.AsksCanYouWalk".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.DirtyLookContractions".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.ChagrinedApology".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.KneelsToStrokeBelly".Localized(
                    Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.LeastSheCanDo".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.OverwhelmedByPainPleasure".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.AndCocksPhrase".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.BirthPangsMoreOrgasmic".Localized());

                Player.CuntChange(100, true, true, false);

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.ParagraphBreakThird".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.SlipperyLittleGirl".Localized());

                float type = 0;

                if (Random.Shared.Next(100) <= 59)
                {
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.GrayWord".Localized());

                    type = 0;

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaChildrenSharkgirls, CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + 1);
                }
                else
                {
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.BlackStripedRedWord".Localized());

                    type = 1;

                    CoC.Instance.Flags.Set(GameFlagIds.IzmaChildrenTigersharks, CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + 1);
                }

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.BabySkinPhrase".Localized());

                if (type == 0)
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.SharkGirlWordAgain".Localized());
                else
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.TigersharkWordAgain".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.CalmsInDaddysArms".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.LatchesAndSuckles".Localized());

                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.ThirdAndFourthBreast".Localized());

                if (Player.BreastRows.Count > 2)
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.StrugglingToFinish".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.WetPoppingRelease".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.NotSoLittleNow".Localized());

                if (type == 0)
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.SharkGirlWordThird".Localized());
                else
                    OutputText("Scenes.Izmascene.PcPopsOutASharkTot.TigersharkWordThird".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.LooksTenToThirteen".Localized());

                OutputText("Scenes.Izmascene.PcPopsOutASharkTot.LooksUpForSupport".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));
            }

            Player.BoostLactation(.01f);
        }

        public void IzmaKidsMenu()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaKidsMenu.ShowsIsolatedBend".Localized((TotalIzmaChildren() == 1 ? "Common.Text.ChildHas".Localized() : "Common.Text.ChildrenHave".Localized())));

            if (TotalIzmaChildren() == 1)
                OutputText("Scenes.Izmascene.IzmaKidsMenu.ThereIsSingular".Localized());
            else
                OutputText("Scenes.Izmascene.IzmaKidsMenu.ThereArePlural".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsMenu.SharkGirlSingular".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaKidsMenu.SharkGirlsPlural".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls))
                    ));

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
                    OutputText("Scenes.Izmascene.IzmaKidsMenu.AndConnector".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsMenu.TigersharkSingular".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaKidsMenu.TigersharksPlural".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks))
                    ));

            OutputText("Scenes.Izmascene.IzmaKidsMenu.FruitOfYourLove".Localized((TotalIzmaChildren() > 1 ? "s" : "")));

            ClearButtons();

            AddNextButton("Common.Button.Watch".Localized(), IzmaKidsPlaytime);

            AddNextButton("Scenes.BeeGirl.BeeEncounterAsBeeMorphFemale.ButtonPlay".Localized(), IzmaPlayWithKids);

            AddButton(14, "Common.Button.Back".Localized(), IzmaFollowerMenu);
        }

        public void IzmaPlayWithKids()
        {
            ClearOutput();

            RegisterMacro("kids", () => TotalIzmaChildren().ToString());

            var choices = new List<int> { 1, 2 };

            if (CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0)
                choices.Add(3);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > Save.GetInt("sharkgirlsDeflowered", 0))
                choices.Add(4);

            if (((Save.GetInt("sharkgirlsDeflowered", 0) != 0 || Save.GetInt("tigersharksDeflowered", 0) != 0) && Player.HasCock() && Player.Lust >= 50) || !false)
                choices.Add(5);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) != 0)
                choices.Add(6);

            switch (choices[Random.Shared.Next(choices.Count)])
            {
                case 1:
                {
                OutputText("Scenes.Izmascene.IzmaPlayWithKids.DecideStreamVisit".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "" : "s"), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.OneOfThem".Localized() : "Common.Text.She".Localized())));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.ShoutSwimOffer".Localized());

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.EnthusiasticDive".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.Sharks".Localized() : "Common.Text.AShark".Localized())));

                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Speed, 1f));

                Player.ChangeFatigue(20);

                Player.HasGottenWashed();
                }

                break;

                case 2:
                {
                OutputText("Scenes.Izmascene.IzmaPlayWithKids.JoinTugOfWar".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.OneOfThem".Localized() : "Common.Text.She".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 5 ? ", especially in such numbers" : "")));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.YoungSharkWhine".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 5 ? " While the others wait, they talk over splitting into two teams so you don't have to pull against your entire brood alone." : "")));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.TossRopeToDad".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 5)
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.DaughtersJoinTeam".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1)
                        OutputText("Scenes.Izmascene.IzmaPlayWithKids.DaughtersGripRope".Localized());
                    else
                        OutputText("Scenes.Izmascene.IzmaPlayWithKids.SheGripsRope".Localized());

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.TuggingBegins".Localized());

                if (Player.Strength < 25)
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.OutmatchedByShark".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.OneOfThem".Localized() : "Common.Text.She".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 5 ? ", especially in such numbers" : "")));
                else
                    if (Player.Strength < 50)
                        OutputText("Scenes.Izmascene.IzmaPlayWithKids.StrongAtFirstPull".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (Player.IsGoo() ? "Common.Text.AndLamentEverBecomingASlime".Localized() : "Common.Text.However".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 5 ? ", the daughters on your team following quickly after" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.Their".Localized() : "Common.Text.Her".Localized())));
                    else
                        OutputText("Scenes.Izmascene.IzmaPlayWithKids.StrainAgainstShark".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.Their".Localized() : "Common.Text.Her".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.S".Localized() : "Common.Text.S".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 5 ? " The sharks on your team, meanwhile, cackle deviously as they aid their [father] in beating the bulk of their siblings." : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.TheFrontOfThePack".Localized() : "Common.Text.She".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.TheWholeBunch".Localized() : "Common.Text.She".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.Us".Localized() : "Common.Text.Me".Localized())));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.HellOfWorkout".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Toughness, 1f));

                Player.ChangeFatigue(20);
                }

                break;

                case 3:
                {
                OutputText("Scenes.Izmascene.IzmaPlayWithKids.WalkToStream".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.One".Localized() : "Common.Text.She".Localized())));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.YellSwimInvite".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.Us".Localized() : "Common.Text.Me".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : "")));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.GreetAnemoneShore".Localized((TotalIzmaChildren() > 1 ? "s" : "")));

                if (CoC.Instance.Flags.Get(GameFlagIds.KidAXp) < 40)
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.SilenceBrokenCall".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "Common.Text.OneOfTheSharks".Localized() : "Common.Text.YourOtherDaughter".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : "")));
                else
                {
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.BallHandsAccept".Localized());

                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.ClosePlayfulSmile".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1 ? "s" : "")));
                }

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.SwimHourSpent".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Speed, 1f));

                Player.ChangeFatigue(20);

                Player.HasGottenWashed();
                }

                break;

                case 4:
                {
                OutputText("Scenes.Izmascene.IzmaPlayWithKids.WadeRiverPlay".Localized());

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.AdultStuffAsk".Localized());

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.AnswerHerQuestion".Localized());

                ClearButtons();
                // 原版 IzmaScene.hx:3128 — allowChild 时才出现「Firsthand」选项
                if (GameSettings.AllowChild)
                    AddNextButton("Common.Button.Firsthand".Localized(), IzmaDaughterFirsthand);

                AddNextButton("Common.Button.Explain".Localized(), IzmaDaughterSecondhand);

                AddNextButton("Common.Button.Escape".Localized(), IzmaDaughterEscape);

                return ;
                }

                case 5:
                {
                RegisterMacro("tigershark", RandomUtils.Choice([(Save.GetInt("tigersharksDeflowered", 0) > 1), false]));

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.AcceptChildPlaytime".Localized());

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.SharksFastInWater".Localized());

                Player.ApplyStats(new DynStat(GameStats.Speed, 1f), new DynStat(GameStats.Toughness, 1f));

                Player.HasGottenWashed();
                // 原版 IzmaScene.hx:3140 的条件是 allowChild（与 minLust 无关）
                if (GameSettings.AllowChild)
                {
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.ArousedDiscomfort".Localized((Player.Libido > 50 ? "Try as you might, you can only get so used to such a sex drive. " : "")));

                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.RacingOfferHelp".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), DaughterBlowjob);

                    AddButton(1, "Common.Button.No".Localized(), DaughterNojob);

                    return ;
                }
                }

                break;

                case 6:
                {
                OutputText("Scenes.Izmascene.IzmaPlayWithKids.DaughtersWranglePlay".Localized());

                OutputText("Scenes.Izmascene.IzmaPlayWithKids.GirlZipsPast".Localized());

                if (Player.Speed + Player.Toughness < 100)
                {
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.CatchAttemptFails".Localized());

                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.CallChildrenBack".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));

                    Player.ChangeFatigue(20);

                    DoNext(() => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.TriumphTackle".Localized());

                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.RuleProtestShout".Localized());

                    OutputText("Scenes.Izmascene.IzmaPlayWithKids.UpsetChoice".Localized());

                    if (Player.Toughness < 80)
                        Player.ApplyStats(new DynStat(GameStats.Toughness, .5f));

                    ClearButtons();

                    AddNextButton("Common.Button.Explain".Localized(), TigerExplain);

                    AddNextButton("Common.Button.Relent".Localized(), TigerRelent);
                    // 原版 IzmaScene.hx:3166 — allowChild 时才出现「Teach Lesson」选项
                    if (GameSettings.AllowChild)
                        AddNextButton("Common.Button.TeachLesson".Localized(), TigerLesson);

                    return ;
                }
                }

                break;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaDaughterEscape()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaDaughterEscape.ExplainTooYoung".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaDaughterSecondhand()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaDaughterSecondhand.LeadDaughterToRock".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterSecondhand.ImpressSeriousness".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterSecondhand.ThanksForAdvice".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaDaughterFirsthand()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.HappyToShow".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.UncertainEager".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.LapSeatInvite".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.RoughSkinCaress".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.ReadyButProper".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.EagerReciprocation".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.ExpressionPleasure".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.BreathAndThanks".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.ChestHandStop".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.BareCrotchShift".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) != 0 || CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 ? "She must have seen her " + (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) != 0 ? "sister" + (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "Common.Text.S".Localized() : "Common.Text.S".Localized()) : "mother's") + " before, but even still," : "You're not sure that she's seen one before, so"
            ));

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.MouthSizeStruggle".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.TakeTaskGusto".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.SilentLongLick".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.CockheadSwallowed".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.CoughAndSpit".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.FunRequestFurther".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand.StaminaOutmatched".Localized());

            Player.Orgasm("Dick");

            DoNext(() => IzmaDaughterFirsthand2());
        }

        public void IzmaDaughterFirsthand2()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.WarmFlatRock".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.MoanedInvitation".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.NodAndContinue".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.ClitAndBreast".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.SilentPleaAfter".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.TwitchEmbrace".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.TightHugPump".Localized());

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.HeavyArmsWrap".Localized(
                Player.Mf("D-Dad", "M-Mom")
            ));

            OutputText("Scenes.Izmascene.IzmaDaughterFirsthand2.RiseSearchArmor".Localized());

            Player.Orgasm("Dick");

            Save.SetInt("sharkgirlsDeflowered", Save.GetInt("sharkgirlsDeflowered", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DaughterBlowjob()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.DaughterBlowjob.GrantHerRequest".Localized());

            OutputText("Scenes.Izmascene.DaughterBlowjob.GentleBaseStroke".Localized());

            OutputText("Scenes.Izmascene.DaughterBlowjob.TongueHeadDeeper".Localized());

            OutputText("Scenes.Izmascene.DaughterBlowjob.GripHipsBobbing".Localized());

            OutputText("Scenes.Izmascene.DaughterBlowjob.HipReleaseRush".Localized());

            OutputText("Scenes.Izmascene.DaughterBlowjob.ThighTenseClimax".Localized());

            OutputText("Scenes.Izmascene.DaughterBlowjob.SurfaceGoodJob".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void DaughterNojob()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.DaughterNojob.NotHerBurden".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TigerExplain()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.TigerExplain.CorrectPoutyBelief".Localized());

            OutputText("Scenes.Izmascene.TigerExplain.PeevedToPumped".Localized());

            OutputText("Scenes.Izmascene.TigerExplain.SatisfiedLessonShown".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TigerRelent()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.TigerRelent.ApologizeRuleGrumbles".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TigerLesson()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.TigerLesson.LiftFromWater".Localized());

            OutputText("Scenes.Izmascene.TigerLesson.ExplainRecognizeLoss".Localized());

            OutputText("Scenes.Izmascene.TigerLesson.DawningRealization".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Izmascene.TigerLesson.HoldBackStrain".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.PraiseGoodGirl".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.SlowSteadyEntry".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.SoftFlankGrip".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.ThrustNearClimax".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.NothingLeftToGive".Localized());
            }
            else
            {
                OutputText("Scenes.Izmascene.TigerLesson.MutualGaze".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.CarryLayOnBack".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.PromptTongueKiss".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.LanguidEscalation".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.DeeperPressTangle".Localized());

                OutputText("Scenes.Izmascene.TigerLesson.SkirtTheEdge".Localized());
            }

            OutputText("Scenes.Izmascene.TigerLesson.AfterglowSlump".Localized());

            OutputText("Scenes.Izmascene.TigerLesson.LazyGazeWorry".Localized());

            OutputText("Scenes.Izmascene.TigerLesson.LessonUncertain".Localized());

            if (RandomUtils.Chance(1 - Save.GetInt("tigersharksDeflowered", 0) / CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks)))
                Save.SetInt("tigersharksDeflowered", Save.GetInt("tigersharksDeflowered", 0) + 1);

            Player.Orgasm("DickVaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaKidsPlaytime()
        {
            ClearOutput();

            var choices = new List<int>();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                choices.Add(16);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
            {
                // 原版 IzmaScene.hx:3295 — allowChild 时列入两个亲子选项
                if (GameSettings.AllowChild)
                {
                    choices.Add(1);

                    choices.Add(2);
                }

                choices.Add(10);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) >= 2)
            {
                choices.Add(3);

                choices.Add(9);

                choices.Add(11);

                choices.Add(14);

                choices.Add(15);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 1)
                choices.Add(6);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) >= 1 && false)
                choices.Add(21);

            choices.Add(4);

            choices.Add(5);
            // 原版 IzmaScene.hx:3315 — allowChild 时列入选项 7
            if (GameSettings.AllowChild)
                choices.Add(7);

            choices.Add(8);

            if (silly && CoC.Instance.Flags.Get(GameFlagIds.JasunMet) > 0)
                choices.Add(13);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) >= 1)
                choices.Add(12);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) >= 10)
                choices.Add(17);

            if (Player.HasItem(ArmorIds.BondageStraps))
                choices.Add(18);

            if (Player.HasKeyItem("All-Natural Onahole") && CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
                choices.Add(19);

            float choice = choices[Random.Shared.Next(choices.Count)];

            if (View.Time.Hours < 10 && CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0 && Random.Shared.Next(2) == 0)
            {
                SceneInstance<AnemoneScene>.Get().KidAWatchesSharks();

                return ;
            }

            switch (choice)
            {
                case 1:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.LoneTigersharkDaughterOff".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) >= 2)
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersSeatedFondling".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
                            OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersSeatedJerking".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouAvertGaze".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaExplainsLibidos".Localized());
                }

                break;

                case 2:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughterCaughtGoblin".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 2)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.OtherKidsGatheredAround".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 2)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ShePromisesTurn".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaShrugsIndifferent".Localized());
                }

                break;

                case 3:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersAsleepInPile".Localized());
                }

                break;

                case 4:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SuddenSplashingErupts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YourDaughter".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.OneOfYourDaughters".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.StrugglingDaughterAshore".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.BiteInterruptedByIzma".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2)
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.OtherChildRunsUp".Localized());
                    else
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.OtherChildrenSurround".Localized());
                }

                break;

                case 5:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YourWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ChildRunsToIzma".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ChildrenRunToIzma".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SheSitsWithBook".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ChildCriesForStory".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ChildrenCryForStory".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouLeaveForDuties".Localized());
                }

                break;

                case 6:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.TwoSharkGirlsHug".Localized());
                // 原版 IzmaScene.hx — 虎鲨孩子超过 2 个
                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 2)
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Watch".Localized(), IzmaKidsLesbianWatch);

                    AddButton(1, "Common.Button.Leave".Localized(), IzmaKidsLesbianLeave);

                    AddNextButton("Common.Button.Demonstrate".Localized(), IzmaKidsLesbianDemonstrate);
                }
                }

                break;

                case 7:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SuddenSplashTwoGirls".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.YourDaughter".Localized() : "Scenes.Izmascene.IzmaKidsPlaytime.OneOfYourDaughters".Localized())));

                if (Random.Shared.Next(2) == 0)
                {
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.HugeCatfishSurfaces".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1)
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.FangsSinkIntoPrey".Localized());
                    else
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersSurroundFisher".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Scenes.Urtapregs.GiveTheKidsABath.OtherChildPhrase".Localized() : "Common.Text.YourOtherChildren".Localized())));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0 && false)
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.AnemoneGirlAttached".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Scenes.Urtapregs.GiveTheKidsABath.OtherChildPhrase".Localized() : "Common.Text.YourOtherChildren".Localized())));
                    else
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.CrayfishClampsTail".Localized());
                }

                break;

                case 8:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.GirlsReadIzmaBooks".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.DaughterIsBusyWhenYouCome".Localized() : "Common.Text.DaughtersAreBusyWhenYouCome".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.SheHasGot".Localized() : "Common.Text.TheyHaveGotten".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.IsBusyTryingToTeachItself".Localized() : "Common.Text.AreBusyTryingToTeachThemselves".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.Her".Localized() : "Common.Text.Them".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.SheMispronounces".Localized() : "Common.Text.TheyMispronounce".Localized())));
                }

                break;

                case 9:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersInBrawl".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2 ? "Common.Text.Two".Localized() : "Common.Text.Some".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2 ? "Common.Text.Both".Localized() : "Common.Text.All".Localized())));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaHaltsTheFight".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 2 ? " and your other daughters are staring at her in shocked horror" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2 ? "Common.Text.Sister".Localized() : "Common.Text.Sisters".Localized())));
                }

                break;

                case 10:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.MedicineTimeAnnounced".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "" : "s")));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SheepishDaughterTakesLeaf".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "s" : ""), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "Common.Text.Their".Localized() : "Common.Text.SHer".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "Common.Text.Their".Localized() : "Common.Text.Her".Localized())));
                }

                break;

                case 11:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersGreetYouBoth".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2 ? "Scenes.Masturbation.CentaurHugeCock.BothWord".Localized() : "Scenes.Masturbation.CentaurHugeCock.AllWordAgain".Localized())));
                }

                break;

                case 12:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaSnarlsAtNursery".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.DaughterIs".Localized() : "Common.Text.DaughtersAre".Localized())));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ImpToutsPotion".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) == 2 ? "Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Sister".Localized() : "Common.Text.Sisters".Localized())));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SheThreatensImp".Localized());

                if (!silly)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaDropkicksImp".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ImpLeersDefiantly".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaScoldsHerChild".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.Daughter".Localized() : "Common.Text.Daughters".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "" : "all of")));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ChildApologizes".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.Im".Localized() : "Common.Text.Were".Localized())));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaSoftensForChild".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.Sweetie".Localized() : "Common.Text.Sweeties".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) == 1 ? "Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Sister".Localized() : "Common.Text.Sisters".Localized())));
                }

                break;

                case 13:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersPumpingIron".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.DaughterIs".Localized() : "Common.Text.DaughtersAre".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 1 ? "Common.Text.Girl".Localized() : "Common.Text.Girls".Localized())));
                }

                break;

                case 14:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersLinedUp".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughterLeapsToWater".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 2)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ChildOnRopeSwing".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 3)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.KidsSplashPlayfully".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaAsksToSwim".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2 ? "Common.Text.OneOfYourChildrenGrabsIzmas".Localized() : "Common.Text.OneOfYourChildrenGrabsIzmas2".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) == 2 ? "Common.Text.SheBegs".Localized() : "Common.Text.TheyBeg".Localized()), (Player.HasUndergarments() ? "Common.Text.Undergarments".Localized() : "Common.Text.BareSkindesc".Localized())));

                float ending = Random.Shared.Next(4);

                if (ending == 4)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.FamilySplashesTogether".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + 2)
                    ));

                if (ending == 3)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SplashDrenchesIzma".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) > 2 ? "But then one of your other daughters sides with you, until eventually it turns into a gigantic free-for-all splashfight." : "")));

                if (ending == 2)
                {
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaVanishesUnderwater".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls))
                    ));

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SheWinksAndSwims".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                }

                if (ending == 1)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersHaveVanished".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls))
                    ));

                if (ending == 0)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaCallsByWhistle".Localized(
                        NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls))
                    ));

                Player.HasGottenWashed();
                }

                break;

                case 15:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughtersWashEachOther".Localized(
                    NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls))
                ));
                }

                break;

                case 16:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.LoneDaughterAsksSibling".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyEnabled) == 0)
                {
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaSaysNotYet".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) == 1 ? "Common.Text.TheTigersharks".Localized() : "Common.Text.TheSharkGirls".Localized())));

                    if (Player.IsPregnant() || Player.Pregnancy.Type == PregnancyIds.Izma)
                    {
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.MoreLittleSharksSoon".Localized());

                        if (Player.IsPregnant() && Player.Pregnancy.Type == PregnancyIds.Izma)
                            OutputText("Scenes.Izmascene.IzmaKidsPlaytime.BothBelliesInTurn".Localized());
                        else
                        {
                            if (Player.IsPregnant())
                                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaPregnantBelly".Localized());

                            if (Player.Pregnancy.Type == PregnancyIds.Izma)
                                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YourPregnantBelly".Localized());
                        }
                    }
                }
                else
                {
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaSmilesAtBaby".Localized());

                    if (Player.IsPregnant() || Player.Pregnancy.Type == PregnancyIds.Izma)
                    {
                        OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SheSmirksAndTouches".Localized());

                        if (Player.IsPregnant() && Player.Pregnancy.Type == PregnancyIds.Izma)
                            OutputText("Scenes.Izmascene.IzmaKidsPlaytime.BothYourBellies".Localized());
                        else
                        {
                            if (Player.IsPregnant())
                                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.HerBelly".Localized());

                            if (Player.Pregnancy.Type == PregnancyIds.Izma)
                                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YourBelly".Localized());
                        }
                    }

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughterCheersWithDelight".Localized());
                }
                }

                break;

                case 17:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouMarvelAtSharks".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0 ? "Common.Text.Tigershark".Localized() : "Common.Text.SharkGirl".Localized())));
                }

                break;

                case 18:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DesireForFamilyTime".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.UnexpectedSight".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SharkgirlInStraps".Localized());

                if (Player.Corruption > 50)
                {
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouUndoHerStraps".Localized());

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SheStandsUnbound".Localized());

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouScoldWithStrap".Localized());

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.PartingStrapSmack".Localized());

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouHeadBackGrinning".Localized());
                }
                else
                {
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.GentleQuickUnbinding".Localized());

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.SternLectureOnTheft".Localized());

                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouHeadBackSmiling".Localized());
                }
                }

                break;

                case 19:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.InterceptedOnTheWay".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 1 ? "Common.Text.OneOfYourTigersharkChildren".Localized() : "Common.Text.YourTigersharkChild".Localized())));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaBringsDaughter".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.BorrowedToyProblem".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouSmileDespiteIt".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouPullOutVial".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.IzmaInspectsOnahole".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.YouHideTheToy".Localized());
                }

                break;

                case 21:
                {
                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.DaughterBeatsGoblin".Localized((TotalIzmaChildren() == 1 ? "Common.Text.DaughterSeems".Localized() : "Common.Text.DaughtersSeem".Localized()), (TotalIzmaChildren() == 1 ? "Common.Text.She".Localized() : "Common.Text.OneOfThem".Localized()), (TotalIzmaChildren() == 1 ? "Common.Text.LittleSharkHoldsHerChinIn".Localized() : "Common.Text.SharkSistersBeginDiscussingAmongEach".Localized())));

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.ImpBegsForRelease".Localized());

                OutputText("Scenes.Izmascene.IzmaKidsPlaytime.NotTheLastTime".Localized((TotalIzmaChildren() == 1 ? "" : "s")));

                ClearButtons();

                AddNextButton("Common.Button.Dominate".Localized(), IzmaKidsLolidom);

                AddNextButton("Common.Button.Nah".Localized(), IzmaKidsNoDom);
                }

                break;
            }

            if ((choice != 6 || !false) && choice != 21)
                DoNext(() => ReturnToCamp(1));
        }

        public void IzmaKidsLesbianWatch()
        {

            OutputText("Scenes.Izmascene.IzmaKidsLesbianWatch.YouTellIzmaDoubtful".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianWatch.EyingSharkgirls".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianWatch.SmallerReturnsFavor".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianWatch.IzmaStaresRedFaced".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 ? "even failing to notice her rigid erection, " : "")));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianWatch.SiblingsFullEmbrace".Localized(((Player.IsFemale() && Player.SexOrientation < 40) ? "Witnessing this before you gives an antsy desire to do the same with your lover. " : "")));

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Lust, 20f));

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaKidsLesbianLeave()
        {

            OutputText("Scenes.Izmascene.IzmaKidsLesbianLeave.YouResignToLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaKidsLesbianDemonstrate()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.TheirOwnDevicesRisk".Localized(
                Player.Mf("their parents", "two mothers")
            ));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.IzmaHandInYours".Localized((Save.GetBool("lezDemonstration", false) ? "Common.Text.EachBreathYouTakeOnlyCauses".Localized() : "Common.Text.BeforeYoureEvenAwareOfHer".Localized())));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.SmallerDaughterPatted".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.YouFetchBlanket".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.GirlsTakeAfterHer".Localized((Player.Corruption > 50 || Player.Libido > 50 ? "Common.Text.You".Localized() : "Common.Text.Izma".Localized())));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.GirlsWatchInSilence".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.ThumpDrawsYourGaze".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.DaughtersBlushCaught".Localized(
                Save.GetBool("lezDemonstration", false) ? "Common.Text.IsThisTheFirstTimeTheyve".Localized() : "Common.Text.TheyLearnSoWellAndIt".Localized(),

                RandomUtils.Choice(["smaller", "bigger"]),

                Player.Body.Get<BodyParts.Vanilla.SkinPart>().Tone == "rough gray" ? "Common.Text.FamiliarRoughSkin".Localized() : "Common.Text.Skindesc".Localized()
            ));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.DifferingTeachingViews".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.YouAssertAsAlpha".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.DaughtersReachTrembling".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.TailCrackCatchesGirls".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.IzmaDivesBetween".Localized(
                RandomUtils.Choice(["older", "younger"])
            ));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.PleasureStartlesGirl".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.HerWorriesFade".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.NoShameInThis".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.WhereIsOtherDaughter".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.LittleSleepTonight".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.IzmaHopesOneDay".Localized((Save.GetBool("lezDemonstration", false) ? "Common.Text.ThatWasIDontKnowWhy".Localized() : "Common.Text.ThatWasEveryTimeTheyGet".Localized()), (TotalIzmaChildren() > 2 ? "Common.Text.TeachTheRest".Localized() : "Common.Text.HaveSomeMore".Localized())));

            OutputText("Scenes.Izmascene.IzmaKidsLesbianDemonstrate.YouLeaveNoncommittal".Localized());

            Player.Orgasm("Vaginal");

            Save.SetBool("lezDemonstration", true);

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaKidsLolidom()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.HandOnShoulder".Localized(
                TotalIzmaChildren() == 1 ? "" : (TotalIzmaChildren() > 2 ? "Common.Text.AndHerSiblings".Localized() : "Common.Text.AndHerSister".Localized())
            ));

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.LittleSharkAsks".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.EmboldenedDaughterSmiles".Localized(
                TotalIzmaChildren() == 1 ? "" : (TotalIzmaChildren() > 2 ? "Common.Text.AsHerSistersWatchOnIn".Localized() : "Common.Text.AsHerSisterWatchesOnIn".Localized())
            ));

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.SharkSilencesGoblin".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.IzmaWatchesGiddily".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 ? "Common.Text.RigidErection".Localized() : "Common.Text.FlushedCheeks".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 ? "Common.Text.SCockToVisiblyPulsateMaking".Localized() : "Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.ToWord".Localized()), (TotalIzmaChildren() == 1 ? "" : " Your other daughter" + (TotalIzmaChildren() > 2 ? "Common.Text.SSeem".Localized() : "Common.Text.Seems".Localized()) + " to share the sentiment, yearning to be the one straddling the territory-invader.")));

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.UnintentionalStimulation".Localized());

            if (OptionStore.Instance.Get("watersports", 0) > 0)
                OutputText("Scenes.Izmascene.IzmaKidsLolidom.OutOfBreathConfession".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.GirlTellsBeta".Localized());

            if (TotalIzmaChildren() > 1)
                OutputText("Scenes.Izmascene.IzmaKidsLolidom.ParagraphPlaceholderToken".Localized((TotalIzmaChildren() > 1 ? "Common.Text.TheOtherSharksAreFarToo".Localized() : "Common.Text.HerSisterFarTooAmpedUp".Localized())));

            OutputText("Scenes.Izmascene.IzmaKidsLolidom.IzmaPrideAndJoy".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaKidsNoDom()
        {

            OutputText("Scenes.Izmascene.IzmaKidsNoDom.LittleToBeGained".Localized());

            OutputText("Scenes.Izmascene.IzmaKidsNoDom.SentimentSharedRetreat".Localized((TotalIzmaChildren() == 1 ? "" : "s")));

            DoNext(() => ReturnToCamp(1));
        }

        public void InCampRideIzmasDickDongTheWitchIsDead()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.FlushedLookAtIzma".Localized());

            if (Player.Wetness() > 3)
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.DrippingWord".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.LieDownInvitation".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.HappySquealResponse".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SensuousArmorRemoval".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            if (Player.HasCock())
            {
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.EachCockToken".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.AndBallsfullFragment".Localized());

                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.MainAttractionApproach".Localized());
            }

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SashayOverTigershark".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaPantsGiggles".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.PursedLipsFeignedIdea".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaPurrsHandsBehind".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.KissingHerBreasts".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.ShaftRubSparks".Localized(
                    Appearance.CockNoun(Player.Cocks[0].Id)
                ));

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SlitherOntoSharkCock".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.Wetness() >= 5)
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.NetherLipsDrooling".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaTensesBeneath".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.FinallyDeciding".Localized());

            if (Player.Corruption < 66)
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.TeasedEnough".Localized());
            else
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.DelayedPleasureLong".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SinkDownStiflingYelp".Localized());

            if (Player.VaginalCapacity() < 20 || Player.Vaginas[0].Virgin)
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.HurtsLikeMotherfucker".Localized());
            else
                if (Player.VaginalCapacity() < 50)
                    OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.GroanShudderDescent".Localized());
                else
                    OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.ShudderLickLips".Localized());

            Player.CuntChange(30, true, true, false);

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.PauseAtVeryBase".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.ErectCockFragment".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.LetterS".Localized());

                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.BouncingAudibleSlaps".Localized());
            }

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SentencePeriodParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaMoansShivers".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaCockTwitches".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.ComplimentGoodBeta".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.MercifulAlmostThere".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.OrgasmFlowsFree".Localized());

            if (Player.Wetness() < 4)
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SpattersWord".Localized());
            else
                if (Player.Wetness() < 5)
                    OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.GushesWord".Localized());
                else
                    OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.FountainsWord".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.OntoTigersharkFragment".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SprayingOverBody".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaRoarsClimax".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.BaskingInAfterglow".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaWeakConsent".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.WickedGrinRhythm".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.SomeTimeLater".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.TiredAndSated".Localized());

            OutputText("Scenes.Izmascene.InCampRideIzmasDickDongTheWitchIsDead.IzmaWavesSmirks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaPregnancyEnabled) > 0)
                Player.KnockUp(PregnancyIds.Izma, PregnancyIds.IzmaIncubation);

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaCampBooks()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaTimeTillNewBookAvailable) > 0)
            {
                OutputText("Scenes.Izmascene.IzmaCampBooks.RequestReadingBook".Localized());

                SharkBookMenus();

                return ;
            }

            OutputText("Scenes.Izmascene.IzmaCampBooks.FeelLikeReading".Localized());

            SharkBookMenus(true);
        }

        public void CampCuntManual()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.CampCuntManual.OfferCombatManual".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimeTillNewBookAvailable, 11);

            float choice = Random.Shared.Next(3);

            if (choice == 0)
            {
                OutputText("Scenes.Izmascene.CampCuntManual.NewGuardingStances".Localized());

                Player.ApplyStats(new DynStat(GameStats.Toughness, 2f));
            }
            else
                if (choice == 1)
                {
                    OutputText("Scenes.Izmascene.CampCuntManual.QuickSkimConclusion".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, 2f));
                }
                else
                {
                    OutputText("Scenes.Izmascene.CampCuntManual.WeightBehindStrikes".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Strength, 2f));
                }

            OutputText("Scenes.Izmascene.CampCuntManual.YawnAndDepart".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void EntropyGuideByStephenHawking()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.EntropyGuideByStephenHawking.OfferEtiquetteGuide".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimeTillNewBookAvailable, 11);

            OutputText("Scenes.Izmascene.EntropyGuideByStephenHawking.PeruseStrangeBook".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Corruption, -2f));

            OutputText("Scenes.Izmascene.EntropyGuideByStephenHawking.ThankAndLeave".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void StephenHawkingPorn()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.StephenHawkingPorn.PointToPornBundle".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.StephenHawkingPorn.KeepBeastPinned".Localized());
            else
                OutputText("Scenes.Izmascene.StephenHawkingPorn.KeepFromMoistening".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaTimeTillNewBookAvailable, 11);

            OutputText("Scenes.Izmascene.StephenHawkingPorn.IzmaTurnedOn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.StephenHawkingPorn.ImpossibleToHideDick".Localized());
            else
                OutputText("Scenes.Izmascene.StephenHawkingPorn.SquirmingLewdSquishes".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void FindLostIzmaKids()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.FindLostIzmaKids.SharkFinApproaches".Localized());

            OutputText("Scenes.Izmascene.FindLostIzmaKids.CorruptedDaughterFound".Localized());

            OutputText("Scenes.Izmascene.FindLostIzmaKids.CallsForMom".Localized());

            DoNext(() => FindLostIzmaKidsII());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaKidsInWild, CoC.Instance.Flags.Get(GameFlagIds.IzmaKidsInWild) - 1);
        }

        public void FindLostIzmaKidsII()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.FindLostIzmaKidsII.EmbraceDaughterTears".Localized());

            OutputText("Scenes.Izmascene.FindLostIzmaKidsII.HappyChainOfThought".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaChildrenSharkgirls, CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void RemoveIzmasPenis(bool forced = false)
        {
            ClearOutput();

            if (forced)
            {
                OutputText("Scenes.Izmascene.RemoveIzmasPenis.IzmaHalfStepBack".Localized());

                OutputText("Scenes.Izmascene.RemoveIzmasPenis.UnquestionedRemovalTalk".Localized());

                OutputText("Scenes.Izmascene.RemoveIzmasPenis.FiveBottlesEnough".Localized());

                OutputText("Scenes.Izmascene.RemoveIzmasPenis.CuttingOffWhining".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AskedIzmaAboutWangRemoval) == 0)
                {
                    OutputText("Scenes.Izmascene.RemoveIzmasPenis.AskToDropDick".Localized());

                    OutputText("Scenes.Izmascene.RemoveIzmasPenis.AssertAlphaOverride".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.AskedIzmaAboutWangRemoval, 1);

                    ClearButtons();

                    AddButton(0, "Common.Button.Okay".Localized(), IzmaFollowerMenu);

                    AddButton(1, "Common.Button.ForceHer".Localized(), () => RemoveIzmasPenis(true));

                    return ;
                }
                else
                {
                    OutputText("Scenes.Izmascene.RemoveIzmasPenis.IzmaSighsWhy".Localized());

                    OutputText("Scenes.Izmascene.RemoveIzmasPenis.PullIntoArms".Localized());

                    OutputText("Scenes.Izmascene.RemoveIzmasPenis.IzmaSnortsResignation".Localized());
                }

            if (Player.ItemCount(ConsumableIds.SuccubiMilk) + Player.ItemCount(ConsumableIds.PurifiedSuccubiMilk) < 5)
            {
                OutputText("Scenes.Izmascene.RemoveIzmasPenis.FindEnoughMilk".Localized());

                DoNext(() => IzmaFollowerMenu());
            }
            else
            {
                OutputText("Scenes.Izmascene.RemoveIzmasPenis.EnoughMilkPrompt".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.RemoveDick".Localized(), IzmaDickToggle);

                AddButton(14, "Common.Button.Back".Localized(), IzmaFollowerMenu);
            }
        }

        public void IzmaDickPrompt()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaDickPrompt.RegrowDickConfirm".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), IzmaDickToggle);

            AddButton(1, "Common.Button.No".Localized(), IzmaFollowerMenu);
        }

        public void IzmaDickToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Izmascene.IzmaDickToggle.FlourishFiveBottles".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.IzmaSatisfiedSip".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.NoteMoistureTrickle".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.CalmingHeavySweat".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.GlassShattersCollapse".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.CradleHeadLastPour".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.JuicesExplodeInWaves".Localized());

                OutputText("Scenes.Izmascene.IzmaDickToggle.CarryToTheStream".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaNoCock, 1);

                var count = 0;

                while (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)) && count < 5)
                {
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

                    count += 1;
                }

                while (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)) && count < 5)
                {
                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));

                    count += 1;

                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));
                }

            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaDickToggle.PermissionToRegrow".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaNoCock, 0);
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckIzmasPussyDominate()
        {
            ClearOutput();

            int x = Player.CockThatFits(65);

            if (x < 0)
                x = 0;

            var y = x + 1;

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.FixatingOnIzma".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.CockWord".Localized());
            else
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.CuntWord".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.MothToOpenFlame".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.LetterEs".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.NowStandCommand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 1)
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.LetterS".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.DroolingInCoolAir".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.TheirLubricants".Localized());
            else
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.ItsLubricant".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.EagerPresentimentAsk".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.AuthoritativeCommand".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 1)
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.SharkGirlWord".Localized());
            else
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.HermaphroditeWord".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.FlirtySmileTurn".Localized());

            if (Player.Pregnancy.Event <= 1)
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.ShoveDownOnStump".Localized(
                    y
                ));
            else
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.LowerCarefullyToStump".Localized(
                    y
                ));

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.TigersharkBatedBreath".Localized());

            if (Player.IsTaur() || Player.IsGoo() || Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.TailAgainstShoulder".Localized());
            else
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.LegOverHerRump".Localized());

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.HeartyRoarPlunge".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.SlowThenSpeedUp".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 1 || (Player.CountCocksOfType("TENTACLE") == 1 && Player.Cocks[x].Id != new Identifier("coc", "tentacle")))
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.CutOperaSingerShort".Localized());

            if (Player.IsGoo() && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
            {
                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.SpiceUpWithAsshole".Localized());

                OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.TendHerDickPrompt".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.GooJob".Localized(), GooJob);

                AddButton(1, "Common.Button.LeaveIt".Localized(), () => RadarIzmaSpanking());
            }

            OutputText("Scenes.Izmascene.FuckIzmasPussyDominate.ParagraphBreak".Localized());

            RadarIzmaGasm();
        }

        public void GooJob()
        {
            ClearOutput();

            gooJobbed = true;

            OutputText("Scenes.Izmascene.GooJob.EnvelopEntireBackside".Localized());

            RadarIzmaSpanking(false);
        }

        public void RadarIzmaSpanking(bool newSentence = true)
        {
            ClearOutput();

            if (newSentence)
                OutputText("Scenes.Izmascene.RadarIzmaSpanking.NowWord".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaSpanking.DoublePenetrationBucks".Localized());

            RadarIzmaGasm();
        }

        public void RadarIzmaGasm()
        {

            OutputText("Scenes.Izmascene.RadarIzmaGasm.IzmaLostInMoment".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaGasm.DemandBeggingWords".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaGasm.HowledSubmission".Localized());

            if (Player.CumQ() <= 750)
                OutputText("Scenes.Izmascene.RadarIzmaGasm.HotLoadIntoFuckhole".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaGasm.SteamyJizzCascade".Localized());

            if (gooJobbed)
            {
                gooJobbed = false;

                OutputText("Scenes.Izmascene.RadarIzmaGasm.InjectedSeedSensation".Localized());
            }

            OutputText("Scenes.Izmascene.RadarIzmaGasm.PartnerSwoonsBelly".Localized());

            if (Player.CumQ() > 750)
                OutputText("Scenes.Izmascene.RadarIzmaGasm.BloatedWord".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaGasm.ClampingLegsShut".Localized());

            if (Player.CumQ() <= 750)
                OutputText("Scenes.Izmascene.RadarIzmaGasm.LetterA".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaGasm.AnUltimatelyFragment".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaGasm.VainContainAttempt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0)
                OutputText("Scenes.Izmascene.RadarIzmaGasm.CockWord".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaGasm.SelfWord".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaGasm.RiverRestDoubt".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            IzmaPreg();

            DoNext(() => ReturnToCamp(1));
        }

        public void RadarIzmaAnalDominant()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.OrderToTreeTrunk".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.RemoveSkimpySkirt".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.ClitFlushesWith".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.EachcockBobsIn".Localized());
                else
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.YouFlushWith".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.ArousedIntrigueNeed".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.DenyHerDominance".Localized(
                Player.AssholeDescript(),

                Player.AssholeDescript()
            ));

            if (Player.AnalCapacity() < 35)
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.AnusPainfulRend".Localized());
            else
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.FleshyGatesSlipping".Localized());

            Player.ButtChange(42, true, true, false);

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.CoolSkittishBreath".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.AndGooFragment".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.RemainStandingStill".Localized());

            if (Player.HasVagina())
                OutputText(Player.ClitDescript());
            else
                if (Player.HasCock())
                    OutputText(Player.CockDescript(0));
                else
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.ViolatedAnus".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.MasturbateWhileFucked".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.NearOrgasmicScare".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.GripFirmAssGyrate".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.PumpingFragment".Localized());

            if (Player.HasVagina())
                OutputText(Player.ClitDescript());
            else
                if (Player.HasCock())
                    OutputText(Player.CockDescript(0));
                else
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.AnusWord".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.FerocityIncreases".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.FingerfuckVagina".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.SwingingWithGyration".Localized());
                else
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.TenderizingThighs".Localized());

            OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.IzmaBreathErratic".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.PullOffAfterFlood".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.BuryClitInFace".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.StrokingToClimax".Localized());

                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.FallingBackRelief".Localized());

                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.CarryOrLeaveHer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.TuckIn".Localized(), RadarIzmaAnalDomResultTuckIn);

                AddButton(1, "Scenes.Dullahan.DefeatedDullahan.ButtonStay".Localized(), RadarIzmaLeaveHerInTheDirtAfterAnalDom);
            }
            else
            {
                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.SlideDownContent".Localized());

                OutputText("Scenes.Izmascene.RadarIzmaAnalDominant.WakingInEmbrace".Localized());

                DoNext(() => ReturnToCamp(1));
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));
        }

        public void RadarIzmaAnalDomResultTuckIn()
        {
            ClearOutput();

            if (Player.Strength < 70)
            {
                OutputText("Scenes.Izmascene.RadarIzmaAnalDomResultTuckIn.DespiteGoodIntentions".Localized());

                if (Player.Fatigue < 70)
                {
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDomResultTuckIn.TuckInSuccess".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Corruption, -2f));

                    Player.ChangeFatigue(30);
                }
                else
                    OutputText("Scenes.Izmascene.RadarIzmaAnalDomResultTuckIn.TuckInExhausted".Localized());
            }
            else
            {
                OutputText("Scenes.Izmascene.RadarIzmaAnalDomResultTuckIn.GreatShowOfForce".Localized());

                Player.ApplyStats(new DynStat(GameStats.Corruption, -2f));
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void RadarIzmaLeaveHerInTheDirtAfterAnalDom()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.RadarIzmaLeaveHerInTheDirtAfterAnalDom.MockHerStamina".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaDomsLatexy()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesIzmaDommedLatexy) == 0)
            {
                OutputText("Scenes.Izmascene.IzmaDomsLatexy.InvestigateLustyPants".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Stop".Localized(), StopIzmaLatexy);

                AddButton(1, "Common.Button.Watch".Localized(), IzmaDomsLatexyPartI);
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaDomsLatexy.OrderHerToFeed".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexy.HurryToWatchShow".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexy.TauntWhileCaressing".Localized());

                IzmaDomsLatexyPartI();
            }
        }

        public void IzmaDomsLatexyPartI()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesIzmaDommedLatexy) != 0)
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.ParagraphBreak".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.GrowlingTaunt".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.BitesHerLowerLip".Localized());

            if (SceneInstance<LatexGirlScene>.Get().GooObedience() > 50)
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.EagerlyWord".Localized());
            else
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SheepishlyWord".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.NodsGushingDribbles".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.TeaseCrinklingNose".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooDickLength) > 0)
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.CockWord".Localized());
            else
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.ClitWord".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.ToePleasuredGasp".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.ShudderingHandPlay".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooObedience) < 50)
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.AnnoyanceFlashes".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GooObedience) <= 80)
                    OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.ConflictedLook".Localized());
                else
                    OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.HungryLookLick".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.PowerfulThrustWhine".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SmileInSatisfaction".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.CroonsHeavyPants".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.GooEyes)
            ));

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.HandsReachQuartette".Localized());

            OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.CommandSoundsLikeBegging".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GooObedience) / 5 + CoC.Instance.Flags.Get(GameFlagIds.GooFluidAmount) / 5 + Random.Shared.Next(20) < 10)
            {
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SucksHarderHollowed".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.CriesWhileCumming".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SlumpsAgainstRock".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SunningWhileDigesting".Localized());
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.ObedientlyRelaxes".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.AssumesPosition".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.RepeatedHipSlaps".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SheathWrinklesPumping".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SlumpsAgainstRockAgain".Localized());

                OutputText("Scenes.Izmascene.IzmaDomsLatexyPartI.SunsWhileDigestingAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesIzmaDommedLatexy, CoC.Instance.Flags.Get(GameFlagIds.TimesIzmaDommedLatexy) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.GooFluidAmount, 100);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 10)));

            DoNext(() => ReturnToCamp(1));
        }

        public void StopIzmaLatexy()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.StopIzmaLatexy.AssertOwnership".Localized());

            OutputText("Scenes.Izmascene.StopIzmaLatexy.ChastisedBacksOff".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.IzmaXLatexyDisabled, 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesIzmaDommedLatexy, CoC.Instance.Flags.Get(GameFlagIds.TimesIzmaDommedLatexy) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaLatexySubmenu()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaLatexySubmenu.TailFlicksReassure".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaXLatexyDisabled) == 1)
                AddButton(0, "Common.Button.Keepherfed".Localized(), IzmaLatexyToggle);
            else
            {
                AddButton(0, "Common.Button.NoFeeding".Localized(), IzmaLatexyToggle);

                AddButton(1, "Common.Button.WatchThem".Localized(), IzmaDomsLatexy);
            }

            AddButton(14, "Common.Button.Back".Localized(), IzmaSexMenu);
        }

        public void IzmaLatexyToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaXLatexyDisabled) == 0)
            {
                OutputText("Scenes.Izmascene.IzmaLatexyToggle.ForbidFeedingHer".Localized());

                OutputText("Scenes.Izmascene.IzmaLatexyToggle.CheersDisappointed".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaXLatexyDisabled, 1);
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaLatexyToggle.ChangedMindAllow".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaXLatexyDisabled, 0);
            }

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), IzmaLatexySubmenu);
        }

        public void IzmaValeriaSubmenu()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaValeriaSubmenu.TailFlicksAskFeeding".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFeedingValeria) == 0)
                AddButton(0, "Common.Button.Keepherfed".Localized(), IzmaValeriaToggle);
            else
                AddButton(0, "Common.Button.NoFeeding".Localized(), IzmaValeriaToggle);

            AddButton(14, "Common.Button.Back".Localized(), IzmaSexMenu);
        }

        public void IzmaValeriaToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFeedingValeria) == 1)
            {
                OutputText("Scenes.Izmascene.IzmaValeriaToggle.ForbidFeedingValeria".Localized());

                OutputText("Scenes.Izmascene.IzmaValeriaToggle.CheersDisappointedValeria".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaFeedingValeria, 0);
            }
            else
            {
                OutputText("Scenes.Izmascene.IzmaValeriaToggle.ApproveFeedingHug".Localized());

                if (Player.Armor.Id == ArmorIds.GooArmor)
                    OutputText("Scenes.Izmascene.IzmaValeriaToggle.GooThanksDecision".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaFeedingValeria, 1);
            }

            ClearButtons();

            AddButton(14, "Common.Button.Back".Localized(), IzmaValeriaSubmenu);
        }

        public void AnemoneWrasslin()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.AnemoneWrasslin.SpotKidAtStream".Localized((TotalIzmaChildren() > 1 ? "Common.Text.OneOfYourSharkDaughters".Localized() : "Common.Text.YourSharkDaughter".Localized())));

            OutputText("Scenes.Izmascene.AnemoneWrasslin.AmbushAtWater".Localized());

            OutputText("Scenes.Izmascene.AnemoneWrasslin.DemandWaterToll".Localized());

            Save.SetInt("daysSinceAneFight", 0);

            ClearButtons();

            AddButton(0, "Common.Button.StopFight".Localized(), StopTheFight);

            AddButton(1, "Common.Button.LetItBe".Localized(), LetItBe);
        }

        public void StopTheFight()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.StopTheFight.CalmLoverEndFight".Localized());

            OutputText("Scenes.Izmascene.StopTheFight.DenyStartingFight".Localized());

            OutputText("Scenes.Izmascene.StopTheFight.HelpKidUpHug".Localized());

            OutputText("Scenes.Izmascene.StopTheFight.ApologyAdmission".Localized());

            OutputText("Scenes.Izmascene.StopTheFight.FamilyHug".Localized());

            OutputText("Scenes.Izmascene.StopTheFight.CalmerIzmaAdores".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void LetItBe()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.LetItBe.LetKidsLearnConflict".Localized());

            OutputText("Scenes.Izmascene.LetItBe.StraddleCallBeta".Localized());

            OutputText("Scenes.Izmascene.LetItBe.ShockThenStruggle".Localized());

            OutputText("Scenes.Izmascene.LetItBe.GradualPenetration".Localized((Save.GetInt("tigersharksDeflowered", 0) < CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) ? "Common.Text.SmallTracesOfBloodCoatHer".Localized() : "Common.Text.TheAnemoneSighsBlissfullyAsShe".Localized())));

            OutputText("Scenes.Izmascene.LetItBe.SqueakyMoansPleasure".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 ? "Common.Text.PulsatingErectionPokingOutFromUnder".Localized() : "Common.Text.DeepBlushTintingHerFaceAs".Localized())));

            OutputText("Scenes.Izmascene.LetItBe.ScreamsDrawAttention".Localized((Save.GetInt("tigersharksDeflowered", 0) < CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) ? "Blood-tinted s" : "S")));

            OutputText("Scenes.Izmascene.LetItBe.LikeMotherLikeDaughter".Localized());

            OutputText("Scenes.Izmascene.LetItBe.ComfortTigershark".Localized());

            OutputText("Scenes.Izmascene.LetItBe.RiledUpReturn".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            if (Save.GetInt("tigersharksDeflowered", 0) < CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks))
                Save.SetInt("tigersharksDeflowered", Save.GetInt("tigersharksDeflowered", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void KidDickBullying()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.KidDickBullying.DaughterTugsArm".Localized());

            OutputText("Scenes.Izmascene.KidDickBullying.GlumDowntrodden".Localized((TotalIzmaChildren() > 2 ? "s" : "")));

            OutputText("Scenes.Izmascene.KidDickBullying.PatientParentAsk".Localized());

            Save.SetBool("kidDick", true);

            ClearButtons();

            AddNextButton("Common.Button.Comfort".Localized(), KidDickComfort);
            // 原版 IzmaScene.hx:4367 — 原版 allowChild 全局开关
            if (GameSettings.AllowChild)
                AddNextButton("Common.Button.Tease".Localized(), KidDickTease);

            AddNextButton("Common.Button.Deflect".Localized(), KidDickDeflect);
        }

        public void KidDickComfort()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.KidDickComfort.NotSmallReassure".Localized((Player.LongestCockLength() < 6 ? "Besides, it's not as if you yourself are packing much, and yet you are the alpha that mated one of the toughest and smartest sharks from the lake. " : "")));

            OutputText("Scenes.Izmascene.KidDickComfort.BrightensWithThanks".Localized());

            DoNext(PlayerMenu);
        }

        public void KidDickTease()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.KidDickTease.GestureToPresent".Localized());

            OutputText("Scenes.Izmascene.KidDickTease.BlushRaisesHands".Localized());

            OutputText("Scenes.Izmascene.KidDickTease.TinyDanglingCock".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) > 0 ? " former tool" : "")));

            OutputText("Scenes.Izmascene.KidDickTease.ShiverHeartbroken".Localized());

            OutputText("Scenes.Izmascene.KidDickTease.GenuineShockSizes".Localized((Player.LongestCockLength() < 5 ? "<i>your</i> endowment and " : "")));

            OutputText("Scenes.Izmascene.KidDickTease.TearsAskWorthless".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), KidDickTeaseYes);

            AddNextButton("Common.Button.No".Localized(), KidDickTeaseNo);
        }

        public void KidDickTeaseYes()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.KidDickTeaseYes.GeneticDeadEnd".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 2 ? "s" : "")));

            OutputText("Scenes.Izmascene.KidDickTeaseYes.BawlingReassurance".Localized((CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 2 ? "Common.Text.Whichever".Localized() : "Common.Text.Your".Localized())));

            DoNext(PlayerMenu);
        }

        public void KidDickTeaseNo()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.KidDickTeaseNo.PatheticButLoved".Localized());

            OutputText("Scenes.Izmascene.KidDickTeaseNo.SnifflingDisbelief".Localized());

            OutputText("Scenes.Izmascene.KidDickTeaseNo.AlphaNeedsBeta".Localized());

            OutputText("Scenes.Izmascene.KidDickTeaseNo.ThumbTwiddling".Localized());

            OutputText("Scenes.Izmascene.KidDickTeaseNo.CoaxBetaMeaning".Localized());

            OutputText("Scenes.Izmascene.KidDickTeaseNo.BetaSharkDeclaration".Localized());

            OutputText("Scenes.Izmascene.KidDickTeaseNo.GoodBetaPraise".Localized());

            DoNext(PlayerMenu);
        }

        public void KidDickDeflect()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.KidDickDeflect.NotYourTerritory".Localized());

            OutputText("Scenes.Izmascene.KidDickDeflect.AshamedApology".Localized());

            DoNext(PlayerMenu);
        }

        public void IzmaCunnilingus()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaCunnilingus.AlphaShowsAffection".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.FirmToneUndress".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.DisplayThenDive".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.EagerPushDown".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.AlphaConfusedSay".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.TongueAnswersLap".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.ZealousPiercing".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.GoalReachedCry".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.MinutesOfExploring".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.FrenziedEating".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.LegsThrashResist".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.ShowMercySlow".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.TongueBackToClit".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.WrithesInBliss".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.ShudderingMess".Localized());

            OutputText("Scenes.Izmascene.IzmaCunnilingus.CleanUpAftercare".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void IzmaCuddle()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.IzmaCuddle.CuddleNotSex".Localized());

            OutputText("Scenes.Izmascene.IzmaCuddle.WetSkinDecline".Localized((Player.Body.Get<BodyParts.Vanilla.SkinPart>().Id == BodyPartTypes.Wooden || Player.Body.Get<BodyParts.Vanilla.SkinPart>().Id == BodyPartTypes.Stalk || Player.Body.Get<BodyParts.Vanilla.SkinPart>().Id == BodyPartTypes.FishScales ? "Common.Text.AsYourSkindescIsJustAs".Localized() : "Common.Text.AsYouCanPutUp".Localized())));

            OutputText("Scenes.Izmascene.IzmaCuddle.FallingIntoBed".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Izma");

            Go<CampScene>();
        }

        public void NoCuddle()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.NoCuddle.BedToYourself".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");

            DoNext(() => IzmaFollowerMenu());
        }

        public void WakeTogether()
        {
            ClearOutput();

            OutputText("Scenes.Izmascene.WakeTogether.WakeToHerGaze".Localized());

            Save.SetBool("izmaMorning", false);

            DoNext(PlayerMenu);
        }

        public bool NightmareCheck()
        {

            return IzmaFollower() && TotalIzmaChildren() > 0 && (View.Time.Days - Save.GetFloat("lastNightmare")) > 14 && CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Izma" && Random.Shared.Next(100) < (5);
        }

        public bool NightmareFollowupTime()
        {

            return Save.GetFloat("lastNightmare") == View.Time.Days;
        }

        public void DaughterNightmare()
        {

            Save.SetFloat("lastNightmare", View.Time.Days);

            var parentChoices = new[] { Player.Mf("daddy", "mommy"), Player.Mf("papa", "mama") };

            int typeCount = 0;

            if (RandomUtils.Chance(CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) / TotalIzmaChildren()))
                nightmareDaughterType = 1;
            else
                nightmareDaughterType = 0;

            typeCount = new[] { CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls), CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) }[nightmareDaughterType];

            if (parentChoices.Length > typeCount)
                parentChoices = parentChoices[..Math.Max(1, typeCount)];

            nightmareParent = parentChoices[Random.Shared.Next(parentChoices.Length)];

            RegisterMacro("tigershark", nightmareDaughterType == 1);

            RegisterMacro("multiple", typeCount > 1);

            RegisterMacro("randomdaddy", nightmareParent);

            OutputText("Scenes.Izmascene.DaughterNightmare.NightAwakening".Localized());

            OutputText("Scenes.Izmascene.DaughterNightmare.BadDreamRequest".Localized());

            OutputText("Scenes.Izmascene.DaughterNightmare.SadDreamExplanation".Localized());

            DoNext(PlayerMenu);
        }

        public void DaughterWakeup()
        {
            ClearOutput();

            Save.SetFloat("lastNightmare", Save.GetFloat("lastNightmare") + 0.5f);

            RegisterMacro("tigershark", nightmareDaughterType == 1);

            RegisterMacro("randomdaddy", nightmareParent);

            OutputText("Scenes.Izmascene.DaughterWakeup.DaughterStillHeld".Localized(
                TotalIzmaChildren() == 1 ? "Common.Text.Izma".Localized() : "Common.Text.Her".Localized() + Pluralize(TotalIzmaChildren() - 1, "sister")
            ));

            DoNext(PlayerMenu);
        }

        public void AskIzmaAboutNobleShirt()
        {
            ClearOutput();

            RegisterMacro("shirtStage", Undergarments.SEA_SHIRT.Save.GetInt("armorStage", 0));

            OutputText("Scenes.Izmascene.AskIzmaAboutNobleShirt.StuckZealotShirt".Localized());

            OutputText("Scenes.Izmascene.AskIzmaAboutNobleShirt.PleasantSurpriseHelp".Localized());

            OutputText("Scenes.Izmascene.AskIzmaAboutNobleShirt.WinceElderStory".Localized());

            OutputText("Scenes.Izmascene.AskIzmaAboutNobleShirt.MagicNotHerForte".Localized());

            OutputText("Scenes.Izmascene.AskIzmaAboutNobleShirt.BewilderedBlink".Localized());

            Save.SetInt("izmaAsked", 1);

            DoNext(() => ReturnToCamp(1));
        }

    }
}